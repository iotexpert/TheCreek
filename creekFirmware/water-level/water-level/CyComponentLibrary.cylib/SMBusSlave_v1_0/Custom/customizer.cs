/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Drawing;

namespace SMBusSlave_v1_0
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyAPICustomize_v1, ICyDRCProvider_v1
    {
        private CyParameters m_parameters;

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            m_parameters = new CyParameters(edit, termQuery);
            m_parameters.m_inst = edit;
            m_parameters.m_globalEditMode = false;

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            // Create tab pages
            CyGeneralTab generalTab = new CyGeneralTab(m_parameters, termQuery);
            CyPmBusCmdsTab pmBusCmdsTab = new CyPmBusCmdsTab(m_parameters, editor);
            CyCustomCmdsTab customCmdsTab = new CyCustomCmdsTab(m_parameters, edit.DesignQuery.ApplicationType);
            CyI2cConfigTab i2cConfTab = new CyI2cConfigTab(m_parameters);

            CyParamExprDelegate dataChanged = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                generalTab.UpdateUI();
            };

            CyParamExprDelegate i2CDataChanged = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                i2cConfTab.UpdateUI();
            };

            // Add tabs to the customizer 
            editor.AddCustomPage(Resources.GeneralTabDisplayName, generalTab, dataChanged, generalTab.TabName);
            editor.AddCustomPage(Resources.PMBusCmdsTabDisplayName, pmBusCmdsTab, dataChanged, pmBusCmdsTab.TabName);
            pmBusCmdsTab.SetPMBusCmdsTabVisibility(m_parameters.Mode);
            editor.AddCustomPage(Resources.CustomCmdsTabDisplayName, customCmdsTab, dataChanged, customCmdsTab.TabName);
            editor.AddCustomPage(Resources.I2cConfTabDisplayName, i2cConfTab, i2CDataChanged, i2cConfTab.TabName);

            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(m_parameters.UpdateClock));

            editor.AddDefaultPage("Built-in", "Built-in");
            editor.UseBigEditor = true;

            generalTab.UpdateUI();
            i2cConfTab.UpdateUI();

            m_parameters.m_globalEditMode = true;

            return editor.ShowDialog();
        }

        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion

        #region ICyAPICustomize_v1 Members

        #region Defining constants
        private const string UINT8_TYPE = "uint8";
        private const string UINT16_TYPE = "uint16";
        private const string SPACE = " ";
        private const string TAB = "    ";
        private const string DBL_TAB = TAB + TAB;
        private const string TRP_TAB = TAB + TAB + TAB;
        private const string QDR_TAB = TAB + TAB + TAB + TAB;
        private const string LOGIC_OR = " | ";
        private const string UINT8_TYPE_SPACE = UINT8_TYPE + SPACE;
        private const string UINT16_TYPE_SPACE = UINT16_TYPE + SPACE;
        private const string STRUCT_NAME = "REGS";
        private static readonly string NEW_LINE = Environment.NewLine;
        #endregion

        #region Declaring private variables
        private string m_instName;
        private bool m_isPSoC3;
        private bool m_isBootloader;
        private enum CyArrayType { NotArray, OneDimArray, TwoDimArray };
        #endregion

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0) paramDict = customizers[0].MacroDictionary;

            m_parameters = new CyParameters(query);
            m_instName = query.InstanceName;
            m_isPSoC3 = query.DeviceQuery.IsPSoC3;
            m_isBootloader = (query.DesignQuery.ApplicationType == CyApplicationType_v1.Bootloader);

            List<CyArrayType> pmBusGenerateAsArray = new List<CyArrayType>();
            List<CyArrayType> customGenerateAsArray = new List<CyArrayType>();
            List<CyArrayType> tempArray = new List<CyArrayType>();

            // Generate structure
            string regStructure = "typedef struct" + NEW_LINE +
                "{" + NEW_LINE +
                "    uint32 SMBUS_REGS_SIG;" + NEW_LINE;

            string varStructure = string.Empty;


            if (m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE)
            {
                for (int i = 0; i < m_parameters.PmBusTable.Count; i++)
                {
                    regStructure += GetStructureField(null, m_parameters.PagedCommands, m_parameters.PmBusTable[i].Name,
                        m_parameters.PmBusTable[i].Size, m_parameters.PmBusTable[i].m_paged,
                        m_parameters.PmBusTable[i].m_enable, i, ref pmBusGenerateAsArray);

                    varStructure += GetStructureField(m_instName, m_parameters.PagedCommands, m_parameters.PmBusTable[i].Name,
                        m_parameters.PmBusTable[i].Size, m_parameters.PmBusTable[i].m_paged,
                        m_parameters.PmBusTable[i].m_enable, i, ref tempArray);
                }
            }
            for (int i = 0; i < m_parameters.CustomTable.Count; i++)
            {
                if (CustomCmdGenerationAllowed(i))
                {
                    regStructure += GetStructureField(null, m_parameters.PagedCommands, m_parameters.CustomTable[i].m_name,
                        m_parameters.CustomTable[i].m_size, m_parameters.CustomTable[i].m_paged,
                        m_parameters.CustomTable[i].m_enable, i, ref customGenerateAsArray);

                    varStructure += GetStructureField(m_instName, m_parameters.PagedCommands, m_parameters.CustomTable[i].m_name,
                        m_parameters.CustomTable[i].m_size, m_parameters.CustomTable[i].m_paged,
                        m_parameters.CustomTable[i].m_enable, i, ref tempArray);
                }
            }
            regStructure += "    uint16 FLASH_CRC;" + NEW_LINE
                + "} " + m_instName + "_" + STRUCT_NAME + ";";

            // Generate NUM_COMMANDS
            int numCommands = 0;
            if (m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE)
            {
                for (int i = 0; i < m_parameters.PmBusTable.Count; i++)
                {
                    if (m_parameters.PmBusTable[i].m_enable)
                        numCommands++;
                }
            }
            for (int i = 0; i < m_parameters.CustomTable.Count; i++)
            {
                if (m_parameters.CustomTable[i].m_enable)
                    numCommands++;
            }

            // Generate command codes defines
            string strCmdsDefines = string.Empty;
            if (m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE)
            {
                for (int i = 0; i < m_parameters.PmBusTable.Count; i++)
                {
                    if (m_parameters.PmBusTable[i].m_enable)
                        strCmdsDefines += GetCmdDefine(m_instName, m_parameters.PmBusTable[i].Name,
                            m_parameters.PmBusTable[i].m_code);
                }
            }
            for (int i = 0; i < m_parameters.CustomTable.Count; i++)
            {
                if (CustomCmdGenerationAllowed(i))
                {
                    if (m_parameters.CustomTable[i].m_enable)
                        strCmdsDefines += GetCmdDefine(m_instName, m_parameters.CustomTable[i].m_name,
                            m_parameters.CustomTable[i].m_code);
                }
            }

            // Generate CMD_TABLE_ENTRY array
            string strCmdTableEntryArray = "volatile " + m_instName + "_CMD_TABLE_ENTRY " + m_instName +
                "_commands[] = {" + NEW_LINE;
            if (m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE)
            {
                for (int i = 0; i < m_parameters.PmBusTable.Count; i++)
                {
                    string cmdPropStr = GetCmdTableEntryElement(m_parameters.PmBusTable[i].m_enable,
                        m_parameters.PmBusTable[i].Name, m_parameters.PmBusTable[i].Size,
                        m_parameters.PmBusTable[i].Type, m_parameters.PmBusTable[i].WriteConfig,
                        m_parameters.PmBusTable[i].ReadConfig, m_parameters.PmBusTable[i].m_paged,
                        m_parameters.PmBusTable[i].m_format, pmBusGenerateAsArray[i]);

                    strCmdTableEntryArray += cmdPropStr;
                }
            }
            for (int i = 0; i < m_parameters.CustomTable.Count; i++)
            {
                if (CustomCmdGenerationAllowed(i))
                {
                    string cmdPropStr = GetCmdTableEntryElement(m_parameters.CustomTable[i].m_enable,
                        m_parameters.CustomTable[i].m_name, m_parameters.CustomTable[i].m_size,
                        m_parameters.CustomTable[i].m_type, m_parameters.CustomTable[i].m_writeConfig,
                        m_parameters.CustomTable[i].m_readConfig, m_parameters.CustomTable[i].m_paged,
                        m_parameters.CustomTable[i].m_format, customGenerateAsArray[i]);
                    strCmdTableEntryArray += cmdPropStr;
                }
            }
            if (strCmdTableEntryArray.LastIndexOf(",") > 0)
                strCmdTableEntryArray = strCmdTableEntryArray.Remove(strCmdTableEntryArray.LastIndexOf(",")) +
                    NEW_LINE + "};";

            // Genrate Read/Write Handler cases
            string strWriteCases = string.Empty;
            string strReadCases = string.Empty;
            string strCustomWriteCases = string.Empty;
            string strCustomReadCases = string.Empty;

            GenerateReadWriteHandlerCases(ref strWriteCases, ref strReadCases);

            // Generate NULL struct
            string strNullStructure = "= {" + NEW_LINE +
                "    // SMBUS_REGS_SIG (Do not change!)" + NEW_LINE +
                TAB + "{0x000055AA}," + NEW_LINE;

            if (m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE)
            {
                for (int i = 0; i < m_parameters.PmBusTable.Count; i++)
                {
                    strNullStructure += GetNullStructureField(m_parameters.PagedCommands,
                        m_parameters.PmBusTable[i].Name, m_parameters.PmBusTable[i].Size,
                        m_parameters.PmBusTable[i].m_paged, m_parameters.PmBusTable[i].m_enable);
                }
            }
            for (int i = 0; i < m_parameters.CustomTable.Count; i++)
            {
                if (CustomCmdGenerationAllowed(i))
                {
                    strNullStructure += GetNullStructureField(m_parameters.PagedCommands,
                        m_parameters.CustomTable[i].m_name, m_parameters.CustomTable[i].m_size,
                        m_parameters.CustomTable[i].m_paged, m_parameters.CustomTable[i].m_enable);
                }
            }
            strNullStructure += TAB + "// FLASH_CRC" + NEW_LINE + TAB + "{0x0000}" + NEW_LINE
                + "}";

            paramDict.Add("RegsStructElements", regStructure);
            paramDict.Add("StoreComponentAllVar", varStructure);
            paramDict.Add("NullStructure", strNullStructure);
            paramDict.Add("NumCommands", "(" + CyParameters.CellConvertHex(numCommands) + "u)");
            paramDict.Add("CommandsDefines", strCmdsDefines);
            paramDict.Add("CmdTableEntry", strCmdTableEntryArray);
            paramDict.Add("WriteHandlerCases", strWriteCases);
            paramDict.Add("ReadHandlerCases", strReadCases);

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }

        private void GenerateReadWriteHandlerCases(ref string writeCases, ref string readCases)
        {
            // Write handler generation
            string writeHandlerCases_ReadWriteByte = string.Empty;
            string writeHandlerCases_ReadWriteWord = string.Empty;
            string writeHandlerCases_ReadWriteBlock = string.Empty;
            string writeHandlerCases_BlockProcessCall = string.Empty;

            string caseHeader = string.Empty;
            string caseFooter = TRP_TAB + "break;" + NEW_LINE + NEW_LINE;
            bool generatePageCommand = false;
            bool generateQueryCommand = false;
            bool generateBootloadCommand = false;

            bool generationAllowed = true;
            int customTableBegginning = 0;

            List<CyCustomTableRow> combined = new List<CyCustomTableRow>();
            if (m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE)
            {
                for (int i = 0; i < m_parameters.PmBusTable.Count; i++)
                {
                    combined.Add(new CyCustomTableRow());
                    combined[i].m_enable = m_parameters.PmBusTable[i].m_enable;
                    combined[i].m_name = m_parameters.PmBusTable[i].Name;
                    combined[i].m_code = m_parameters.PmBusTable[i].m_code;
                    combined[i].m_type = m_parameters.PmBusTable[i].Type;
                    combined[i].m_format = m_parameters.PmBusTable[i].m_format;
                    combined[i].m_paged = m_parameters.PmBusTable[i].m_paged;
                    combined[i].m_size = m_parameters.PmBusTable[i].Size;
                    combined[i].m_readConfig = m_parameters.PmBusTable[i].ReadConfig;
                    combined[i].m_writeConfig = m_parameters.PmBusTable[i].WriteConfig;
                    combined[i].m_specific = false;
                    customTableBegginning = i + 1;
                }
            }

            foreach (CyCustomTableRow item in m_parameters.CustomTable)
            {
                combined.Add(item);
            }


            for (int i = 0; i < combined.Count; i++)
            {
                if (i >= customTableBegginning)
                {
                    generationAllowed = CustomCmdGenerationAllowed((m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE) ?
                        (i - m_parameters.PmBusTable.Count) : i);
                }

                if (combined[i].m_enable && generationAllowed)
                {
                    if (combined[i].m_writeConfig == CyEReadWriteConfigType.Auto ||
                        combined[i].m_writeConfig == CyEReadWriteConfigType.Manual)
                    {
                        caseHeader = GetCaseHeader(combined[i].m_name);
                        switch (combined[i].m_type)
                        {
                            case CyECmdType.ReadWriteByte:
                                if (combined[i].m_name == CyCustomTable.PAGE)
                                    generatePageCommand = true;
                                else
                                    writeHandlerCases_ReadWriteByte += caseHeader;
                                break;
                            case CyECmdType.ReadWriteWord:
                                writeHandlerCases_ReadWriteWord += caseHeader;
                                break;
                            case CyECmdType.ReadWriteBlock:
                                if (combined[i].m_name != CyCustomTable.BOOTLOAD_READ)
                                {
                                    if (combined[i].m_name == CyCustomTable.BOOTLOAD_WRITE)
                                        generateBootloadCommand = true;
                                    else
                                        writeHandlerCases_ReadWriteBlock += caseHeader;
                                }
                                break;
                            case CyECmdType.BlockProcessCall:
                                if (combined[i].m_name == CyPMBusTable.SMBALERT_MASK)
                                {
                                    writeHandlerCases_BlockProcessCall += caseHeader;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (generatePageCommand)
            {
                writeCases += GetCaseHeader(CyCustomTable.PAGE) + GetWriteHandlerCaseBody_PageCmd() +
                    caseFooter;
            }

            if (generateBootloadCommand)
            {
                writeCases += GetCaseHeader(CyCustomTable.BOOTLOAD_WRITE) +
                    GetWriteHandlerCaseBody_BootloadWriteCmd() + caseFooter;
            }

            if (writeHandlerCases_ReadWriteByte != string.Empty)
                writeHandlerCases_ReadWriteByte += GetWriteHandlerCaseBody_ReadWriteByte() + caseFooter;
            if (writeHandlerCases_ReadWriteWord != string.Empty)
                writeHandlerCases_ReadWriteWord += GetWriteHandlerCaseBody_ReadWriteWord() + caseFooter;
            if (writeHandlerCases_ReadWriteBlock != string.Empty)
                writeHandlerCases_ReadWriteBlock += GetWriteHandlerCaseBody_ReadWriteBlock() + caseFooter;
            if (writeHandlerCases_BlockProcessCall != string.Empty)
                writeHandlerCases_BlockProcessCall += GetWriteHandlerCaseBody_BlockProcessCall() + caseFooter;

            writeCases += writeHandlerCases_ReadWriteByte +
                writeHandlerCases_ReadWriteWord +
                writeHandlerCases_ReadWriteBlock +
                writeHandlerCases_BlockProcessCall;

            // Read handler generation
            string readHandlerCases_ReadWriteByte = string.Empty;
            string readHandlerCases_ReadWriteWord = string.Empty;
            string readHandlerCases_ReadWriteBlock = string.Empty;
            string readHandlerCases_ProcessCall = string.Empty;
            string readHandlerCases_BlockProcessCall = string.Empty;
            generatePageCommand = false;
            generateQueryCommand = false;
            generateBootloadCommand = false;
            generationAllowed = true;
            for (int i = 0; i < combined.Count; i++)
            {
                if (i >= customTableBegginning)
                {
                    generationAllowed = CustomCmdGenerationAllowed((m_parameters.Mode == CyEModeSelType.PMBUS_SLAVE) ?
                        (i - m_parameters.PmBusTable.Count) : i);
                }

                if (combined[i].m_enable && generationAllowed)
                {
                    if (combined[i].m_readConfig == CyEReadWriteConfigType.Auto ||
                        combined[i].m_readConfig == CyEReadWriteConfigType.Manual)
                    {
                        caseHeader = GetCaseHeader(combined[i].m_name);
                        switch (combined[i].m_type)
                        {
                            case CyECmdType.ReadWriteByte:
                                if (combined[i].m_name == CyCustomTable.PAGE)
                                    generatePageCommand = true;
                                else
                                    readHandlerCases_ReadWriteByte += caseHeader;
                                break;
                            case CyECmdType.ReadWriteWord:
                                readHandlerCases_ReadWriteWord += caseHeader;
                                break;
                            case CyECmdType.ReadWriteBlock:
                                if (combined[i].m_name != CyCustomTable.BOOTLOAD_WRITE)
                                {
                                    if (combined[i].m_name == CyCustomTable.BOOTLOAD_READ)
                                        generateBootloadCommand = true;
                                    else
                                        readHandlerCases_ReadWriteBlock += caseHeader;
                                }
                                break;
                            case CyECmdType.BlockProcessCall:
                                if (combined[i].m_name == CyCustomTable.QUERY)
                                {
                                    generateQueryCommand = true;
                                }
                                else
                                {
                                    readHandlerCases_BlockProcessCall += caseHeader;
                                }
                                break;
                            case CyECmdType.ProcessCall:
                                readHandlerCases_ProcessCall += caseHeader;
                                break;
                            default:

                                break;
                        }
                    }
                }
            }

            if (generatePageCommand)
            {
                readCases += GetCaseHeader(CyCustomTable.PAGE) + GetReadHandlerCaseBody_PageCmd() +
                    caseFooter;
            }

            if (generateQueryCommand)
            {
                readCases += GetCaseHeader(CyCustomTable.QUERY) + GetReadHandlerCaseBody_QueryCmd() +
                    caseFooter;
            }

            if (generateBootloadCommand)
            {
                readCases += GetCaseHeader(CyCustomTable.BOOTLOAD_READ) +
                    GetReadHandlerCaseBody_BootloadReadCmd() + caseFooter;
            }

            if (readHandlerCases_ReadWriteByte != string.Empty)
                readHandlerCases_ReadWriteByte += GetReadHandlerCaseBody_ReadWriteByte() + caseFooter;
            if (readHandlerCases_ReadWriteWord != string.Empty)
                readHandlerCases_ReadWriteWord += GetReadHandlerCaseBody_ReadWriteWord() + caseFooter;
            if (readHandlerCases_ReadWriteBlock != string.Empty)
                readHandlerCases_ReadWriteBlock += GetReadHandlerCaseBody_ReadWriteBlock() + caseFooter;
            if (readHandlerCases_ProcessCall != string.Empty)
                readHandlerCases_ProcessCall += GetReadHandlerCaseBody_ProcessCall() + caseFooter;
            if (readHandlerCases_BlockProcessCall != string.Empty)
                readHandlerCases_BlockProcessCall += GetReadHandlerCaseBody_BlockProcessCall() + caseFooter;


            readCases += readHandlerCases_ReadWriteByte +
                readHandlerCases_ReadWriteWord +
                readHandlerCases_ReadWriteBlock +
                readHandlerCases_ProcessCall +
                readHandlerCases_BlockProcessCall;
        }

        #region Write handler functions
        private string GetCaseHeader(string name)
        {
            return DBL_TAB + "case " + m_instName + "_" + name + ":" + NEW_LINE;
        }

        private string GetWriteHandlerCaseBody_PageCmd()
        {
            return TRP_TAB + "/* First byte in the buffer is a new page value */" + NEW_LINE +
                TRP_TAB + m_instName + "_cmdPage = " + m_instName + "_buffer[0u];" + NEW_LINE + NEW_LINE +
                TRP_TAB + "/* Copy new page value into Operating Memory Register Store." + NEW_LINE +
                TRP_TAB + "* `$INSTANCE_NAME`_cmdDataPtr should point to correct location." + NEW_LINE +
                TRP_TAB + "*/" + NEW_LINE +
                TRP_TAB + m_instName + "_cmdDataPtr[0u] = " + m_instName + "_buffer[0u];" + NEW_LINE;
        }

        private string GetWriteHandlerCaseBody_BootloadWriteCmd()
        {
            return TRP_TAB + "byteCount = " + m_instName + "_buffer[0u];" + NEW_LINE + NEW_LINE +
                TRP_TAB + "if (" + m_instName + "_bufferIndex == (byteCount + 1u))" + NEW_LINE +
                TRP_TAB + "{" + NEW_LINE +
                QDR_TAB + "for (i = 0u; i < byteCount; i++)" + NEW_LINE +
                QDR_TAB + "{" + NEW_LINE +
                QDR_TAB + TAB + m_instName + "_btldrWriteBuf[i] = " + m_instName + "_buffer[i + 1u];" + NEW_LINE +
                QDR_TAB + "}" + NEW_LINE +
                QDR_TAB + m_instName + "_btldrStatus |= " + m_instName + "_BTLDR_WR_CMPT;" + NEW_LINE +
                QDR_TAB + m_instName + "_btldrWrBufByteCount = byteCount;" + NEW_LINE +
                TRP_TAB + "}" + NEW_LINE;
        }

        private string GetWriteHandlerCaseBody_ReadWriteByte()
        {
            return TRP_TAB + "/* Update the Register Store data for the specified command */" + NEW_LINE +
                TRP_TAB + m_instName + "_cmdDataPtr[page_offset] = " + m_instName + "_buffer[0u];" + NEW_LINE;
        }

        private string GetWriteHandlerCaseBody_ReadWriteWord()
        {
            string caseBody = TRP_TAB + "/* Update the Register Store data for the specified command */" + NEW_LINE;
            if (m_isPSoC3)
            {
                caseBody += TRP_TAB + m_instName + "_cmdDataPtr[page_offset + 1u] = " + m_instName + "_buffer[0u];" +
                    NEW_LINE +
                    TRP_TAB + m_instName + "_cmdDataPtr[page_offset] = " + m_instName + "_buffer[1u];" + NEW_LINE;
            }
            else
            {
                caseBody += TRP_TAB + m_instName + "_cmdDataPtr[page_offset] = " + m_instName + "_buffer[0u];" +
                    NEW_LINE +
                    TRP_TAB + m_instName + "_cmdDataPtr[page_offset + 1u] = " + m_instName + "_buffer[1u];" + NEW_LINE;
            }
            return caseBody;
        }

        private string GetWriteHandlerCaseBody_BlockProcessCall()
        {
            return GetWriteHandlerCaseBody_ReadWriteWord();
        }

        private string GetWriteHandlerCaseBody_ReadWriteBlock()
        {
            return TRP_TAB + "/* Update the Register Store data for the specified command */" + NEW_LINE +
                TRP_TAB + "for(i = 0u; i < " + m_instName + "_bufferSize; i++)" + NEW_LINE +
                TRP_TAB + "{" + NEW_LINE +
                QDR_TAB + m_instName + "_cmdDataPtr[page_offset + i] = " + m_instName + "_buffer[i];" + NEW_LINE +
                TRP_TAB + "}" + NEW_LINE;
        }
        #endregion

        #region Read handler functions
        private string GetReadHandlerCaseBody_PageCmd()
        {
            return TRP_TAB + "/* Read back page value from Operating Memory Register Store." + NEW_LINE +
                TRP_TAB + "* `$INSTANCE_NAME`_cmdDataPtr should point to correct location." + NEW_LINE +
                TRP_TAB + "*/" + NEW_LINE +
                TRP_TAB + m_instName + "_buffer[0u] = " + m_instName + "_cmdDataPtr[0u];" + NEW_LINE;
        }

        private string GetReadHandlerCaseBody_QueryCmd()
        {
            return TRP_TAB + "/* Search for the comman */" + NEW_LINE +
                TRP_TAB + "tmpCmd = " + m_instName + "_SearchCommand(" + m_instName + "_buffer[1u]);" + NEW_LINE +
                NEW_LINE +
                TRP_TAB + "if(" + m_instName + "_CMD_UNDEFINED != tmpCmd)" + NEW_LINE +
                TRP_TAB + "{" + NEW_LINE +
                QDR_TAB + "/* Update query data command code as we succeeded to find the command */" + NEW_LINE +
                QDR_TAB + m_instName + "_queryCmd = " + m_instName + "_buffer[1u]" + ";" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + "/* Store command properties in variable to save time required for accessing" + NEW_LINE +
                QDR_TAB + "* structure." + NEW_LINE +
                QDR_TAB + "*/" + NEW_LINE +
                QDR_TAB + "tmpProps = " + m_instName + "_commands[tmpCmd].cmdProp;" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + "/* This will clear the previous QUERY data and set a flag that currently" + NEW_LINE +
                QDR_TAB + "* investigated command is supported." + NEW_LINE +
                QDR_TAB + "*/" + NEW_LINE +
                QDR_TAB + "" + m_instName + "_queryData = " + m_instName + "_QRY_CMD_SUPPORTED;" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + "if(0u != (tmpProps & " + m_instName + "_CMD_PROP_WRITE_MASK))" + NEW_LINE +
                QDR_TAB + "{" + NEW_LINE +
                QDR_TAB + TAB + m_instName + "_queryData |= " + m_instName + "_QRY_CMD_WR_SUPPORTED;" + NEW_LINE +
                QDR_TAB + "}" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + "if(0u != (tmpProps & " + m_instName + "_CMD_PROP_READ_MASK))" + NEW_LINE +
                QDR_TAB + "{" + NEW_LINE +
                QDR_TAB + TAB + m_instName + "_queryData |= " + m_instName + "_QRY_CMD_RD_SUPPORTED;" + NEW_LINE +
                QDR_TAB + "}" + NEW_LINE +
                QDR_TAB + "/* Update command properties based on the data from customizer */" + NEW_LINE +
                QDR_TAB + "switch (tmpProps & " + m_instName + "_CMD_PROP_FORMAT_MASK)" + NEW_LINE +
                QDR_TAB + "{" + NEW_LINE +
                QDR_TAB + TAB + "case " + m_instName + "_CMD_PROP_FORMAT_NON_NUMERIC:" + NEW_LINE +
                QDR_TAB + DBL_TAB + m_instName + "_queryData |= " + m_instName + "_QRY_FORMAT_NON_NUMERIC;" + NEW_LINE +
                QDR_TAB + DBL_TAB + "break;" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + TAB + "case " + m_instName + "_CMD_PROP_FORMAT_LINEAR:" + NEW_LINE +
                QDR_TAB + DBL_TAB + m_instName + "_queryData |= " + m_instName + "_QRY_FORMAT_LINEAR;" + NEW_LINE +
                QDR_TAB + DBL_TAB + "break;" + NEW_LINE +
                QDR_TAB + TAB + "case " + m_instName + "_CMD_PROP_FORMAT_SIGNED:" + NEW_LINE +
                QDR_TAB + DBL_TAB + m_instName + "_queryData |= " + m_instName + "_QRY_FORMAT_SIGNED16;" + NEW_LINE +
                QDR_TAB + DBL_TAB + "break;" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + TAB + "case " + m_instName + "_CMD_PROP_FORMAT_DIRECT:" + NEW_LINE +
                QDR_TAB + DBL_TAB + m_instName + "_queryData |= " + m_instName + "_QRY_FORMAT_DIRECT;" + NEW_LINE +
                QDR_TAB + DBL_TAB + "break;" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + TAB + "case " + m_instName + "_CMD_PROP_FORMAT_UNSIGNED:" + NEW_LINE +
                QDR_TAB + DBL_TAB + m_instName + "_queryData |= " + m_instName + "_QRY_FORMAT_UNSIGNED8;" + NEW_LINE +
                QDR_TAB + DBL_TAB + "break;" + NEW_LINE +
                NEW_LINE + QDR_TAB + TAB + "case " + m_instName + "_CMD_PROP_FORMAT_VID_MODE:" + NEW_LINE +
                QDR_TAB + DBL_TAB + m_instName + "_queryData |= " + m_instName + "_QRY_FORMAT_VID_MODE;" + NEW_LINE +
                QDR_TAB + DBL_TAB + "break;" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + TAB + "default:" + NEW_LINE +
                QDR_TAB + DBL_TAB + "/* Should never be here */" + NEW_LINE +
                QDR_TAB + DBL_TAB + "break;" + NEW_LINE + QDR_TAB + "}" + NEW_LINE +
                NEW_LINE +
                QDR_TAB + "/* Copy command code investigated into Operating Memory Register Store." + NEW_LINE +
                QDR_TAB + "* `$INSTANCE_NAME`_cmdDataPtr should point to correct location." + NEW_LINE +
                QDR_TAB + "*/" + NEW_LINE +
                QDR_TAB + "" + m_instName + "_cmdDataPtr[0u] = " + m_instName + "_buffer[0u];" + NEW_LINE +
                QDR_TAB + "" + m_instName + "_cmdDataPtr[1u] = " + m_instName + "_buffer[1u];" + NEW_LINE +
                NEW_LINE + QDR_TAB + "/* Put gathered information about the command into I2C buffer" + NEW_LINE +
                QDR_TAB + "* to send it to master." + NEW_LINE +
                QDR_TAB + "*/" + NEW_LINE +
                QDR_TAB + "" + m_instName + "_buffer[0u] = " + "1u;" + NEW_LINE +
                QDR_TAB + "" + m_instName + "_buffer[1u] = " + m_instName + "_queryData;" + NEW_LINE +
                TRP_TAB + "}" + NEW_LINE +
                TRP_TAB + "else" + NEW_LINE +
                TRP_TAB + "{" + NEW_LINE +
                QDR_TAB + "/* Send zero as command isn't supported. */" + NEW_LINE +
                QDR_TAB + m_instName + "_buffer[0u] = 1u;" + NEW_LINE +
                QDR_TAB + m_instName + "_buffer[1u] = 0u;" + NEW_LINE +
                TRP_TAB + "}" + NEW_LINE;
        }

        private string GetReadHandlerCaseBody_BootloadReadCmd()
        {
            return TRP_TAB + "/* First byte of the buffer contains data size */" + NEW_LINE +
                TRP_TAB + "for(i = 0u; i < "+ m_instName + "_btldrRdBufByteCount; i++)" + NEW_LINE +
                TRP_TAB + "{" + NEW_LINE +
                QDR_TAB + m_instName + "_buffer[i] = " + m_instName + "_btldrReadBuf[i];" + NEW_LINE +
                TRP_TAB + "}" + NEW_LINE + NEW_LINE +
                TRP_TAB + m_instName + "_bufferSize =" + m_instName + "_btldrRdBufByteCount;" + NEW_LINE;
        }

        private string GetReadHandlerCaseBody_ReadWriteByte()
        {
            return TRP_TAB + "/* Update the Register Store data for the specified command */" + NEW_LINE +
                TRP_TAB + m_instName + "_buffer[0u] = " + m_instName + "_cmdDataPtr[page_offset];" + NEW_LINE;
        }

        private string GetReadHandlerCaseBody_ReadWriteWord()
        {
            string caseBody = string.Empty;
            if (m_isPSoC3)
            {
                caseBody += TRP_TAB + m_instName + "_buffer[0u] = " + m_instName + "_cmdDataPtr[page_offset + 1u];" +
                    NEW_LINE +
                    TRP_TAB + m_instName + "_buffer[1u] = " + m_instName + "_cmdDataPtr[page_offset];" + NEW_LINE;
            }
            else
            {
                caseBody += TRP_TAB + m_instName + "_buffer[0u] = " + m_instName + "_cmdDataPtr[page_offset];" +
                   NEW_LINE +
                   TRP_TAB + m_instName + "_buffer[1u] = " + m_instName + "_cmdDataPtr[page_offset + 1u];" + NEW_LINE;
            }
            return caseBody;
        }

        private string GetReadHandlerCaseBody_ReadWriteBlock()
        {
            return TRP_TAB + "for(i = 0u; i < " + m_instName + "_bufferSize; i++)" + NEW_LINE +
                TRP_TAB + "{" + NEW_LINE +
                QDR_TAB + m_instName + "_buffer[i] = " + m_instName + "_cmdDataPtr[page_offset + i];" + NEW_LINE +
                TRP_TAB + "}" + NEW_LINE;
        }

        private string GetReadHandlerCaseBody_BlockProcessCall()
        {
            return TRP_TAB + m_instName + "_bufferIndex = 0u;" + NEW_LINE +
                TRP_TAB + "for(i = 0u; i < " + m_instName + "_bufferSize; i++)" + NEW_LINE +
                TRP_TAB + "{" + NEW_LINE +
                QDR_TAB + m_instName + "_buffer[i] = " + m_instName + "_cmdDataPtr[page_offset + i];" + NEW_LINE +
                TRP_TAB + "}" + NEW_LINE;
        }

        private string GetReadHandlerCaseBody_ProcessCall()
        {
            return GetReadHandlerCaseBody_ReadWriteWord();
        }
        #endregion

        private bool CustomCmdGenerationAllowed(int index)
        {
            // Miss BOOTLOADER commands generation in non-bootloader project
            if ((CyCustomTable.IsCmdBootloader(m_parameters.CustomTable[index].m_name) == false) ||
                (CyCustomTable.IsCmdBootloader(m_parameters.CustomTable[index].m_name) && m_isBootloader))
            {
                return true;
            }
            return false;
        }

        private string GetCmdTableEntryElement(bool enable, string name, byte? size, CyECmdType type,
            CyEReadWriteConfigType writeConfig, CyEReadWriteConfigType readConfig, bool paged, CyEFormatType format,
            CyArrayType generateAsArray)
        {
            string result = string.Empty;
            if (enable)
            {
                if (type == CyECmdType.BlockProcessCall || type == CyECmdType.ReadWriteBlock)
                    size++;
                bool isBootloader = CyCustomTable.IsCmdBootloader(name);

                string cast = (type == CyECmdType.ReadWriteWord) ? "(uint8*) " : string.Empty;
                string arrayZeroElement = string.Empty;
                switch (generateAsArray)
                {
                    case CyArrayType.OneDimArray:
                        arrayZeroElement = "[0]";
                        break;
                    case CyArrayType.TwoDimArray:
                        arrayZeroElement = "[0][0]";
                        break;
                    default:
                        break;
                }

                result += TAB + "{" + NEW_LINE + DBL_TAB + m_instName + "_" + name + "," +
                    NEW_LINE + DBL_TAB + (isBootloader ? "0x80" : CyParameters.CellConvertHex(size)) + "u," + NEW_LINE +
                    DBL_TAB + ((size == 0) ? "NULL" : (cast + "&" + m_instName + (isBootloader ? "_btldrWriteBuf" :
                    "_regs" + "." + name + arrayZeroElement))) + "," + NEW_LINE;

                string cmdPropStr = string.Empty;
                byte cmdPropValue = 0;
                switch (writeConfig)
                {
                    case CyEReadWriteConfigType.None:
                        cmdPropValue += 0x00;
                        break;
                    case CyEReadWriteConfigType.Manual:
                        cmdPropStr += m_instName + "_CMD_PROP_WRITE_MANUAL" + LOGIC_OR;
                        cmdPropValue += 0x01;
                        break;
                    case CyEReadWriteConfigType.Auto:
                        cmdPropStr += m_instName + "_CMD_PROP_WRITE_AUTO" + LOGIC_OR;
                        cmdPropValue += 0x02;
                        break;
                    default:
                        break;
                }
                switch (readConfig)
                {
                    case CyEReadWriteConfigType.None:
                        cmdPropValue += 0x00;
                        break;
                    case CyEReadWriteConfigType.Manual:
                        cmdPropStr += m_instName + "_CMD_PROP_READ_MANUAL" + LOGIC_OR;
                        cmdPropValue += 0x04;
                        break;
                    case CyEReadWriteConfigType.Auto:
                        cmdPropStr += m_instName + "_CMD_PROP_READ_AUTO" + LOGIC_OR;
                        cmdPropValue += 0x06;
                        break;
                    default:
                        break;
                }
                if (paged)
                {
                    cmdPropStr += m_instName + "_CMD_PROP_PAGE_INDEXED" + LOGIC_OR;
                    cmdPropValue += 0x10;
                }
                else
                {
                    cmdPropValue += 0x00;
                }
                switch (format)
                {
                    case CyEFormatType.NonNumeric:
                        cmdPropValue += 0x00;
                        break;
                    case CyEFormatType.Linear:
                        cmdPropStr += m_instName + "_CMD_PROP_FORMAT_LINEAR" + LOGIC_OR;
                        cmdPropValue += 0x10;
                        break;
                    case CyEFormatType.Signed:
                        cmdPropStr += m_instName + "_CMD_PROP_FORMAT_SIGNED" + LOGIC_OR;
                        cmdPropValue += 0x20;
                        break;
                    case CyEFormatType.Direct:
                        cmdPropStr += m_instName + "_CMD_PROP_FORMAT_DIRECT" + LOGIC_OR;
                        cmdPropValue += 0x40;
                        break;
                    case CyEFormatType.Unsigned:
                        cmdPropStr += m_instName + "_CMD_PROP_FORMAT_UNSIGNED" + LOGIC_OR;
                        cmdPropValue += 0x60;
                        break;
                    case CyEFormatType.VidMode:
                        cmdPropStr += m_instName + "_CMD_PROP_FORMAT_VID_MODE" + LOGIC_OR;
                        cmdPropValue += 0x80;
                        break;
                    default:
                        break;
                }

                if (cmdPropStr.EndsWith(LOGIC_OR))
                {
                    if (cmdPropStr.LastIndexOf(LOGIC_OR) > 0)
                        cmdPropStr = cmdPropStr.Remove(cmdPropStr.LastIndexOf(LOGIC_OR));
                }
                cmdPropStr = DBL_TAB + cmdPropStr + NEW_LINE + TAB + "}," + NEW_LINE;
                if (cmdPropValue == 0)
                    cmdPropStr = "0x00";
                result += cmdPropStr;
            }
            return result;
        }

        private static string GetCmdDefine(string instanceName, string name, byte? code)
        {
            return "#define " + instanceName + "_" + name + "    (" + CyParameters.CellConvertHex(code) + "u)" +
                NEW_LINE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instName">Should be null for generation Reg structure and Instance name for 
        /// generation variable</param>
        /// <param name="pagedCommands"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="paged"></param>
        /// <param name="enable"></param>
        /// <param name="index"></param>
        /// <param name="generateAsArray"></param>
        /// <returns></returns>
        private static string GetStructureField(string instName, byte pagedCommands, string name, byte? size, 
            bool paged, bool enable, int index, ref List<CyArrayType> generateAsArray)
        {
            string result = string.Empty;

            string prefix8 = UINT8_TYPE_SPACE;
            string prefix16 = UINT16_TYPE_SPACE;
            string suffix = ";";
            if (instName != null)
            {
                prefix8 = string.Format("{0}_regs.", instName);
                prefix16 = prefix8;
                suffix = string.Empty;
            }

            generateAsArray.Add(new CyArrayType());
            if (enable && size != null)
            {

                switch (size)
                {
                    case 0:
                        break;
                    case 1:
                        if (paged && pagedCommands > CyParamRange.PAGED_CMDS_MIN)
                        {
                            result += prefix8 + name + "[" + pagedCommands + "]" + suffix + NEW_LINE;
                            generateAsArray[index] = CyArrayType.OneDimArray;
                        }
                        else
                        {
                            if (name == CyCustomTable.QUERY) // QUERY is exception. Its size should be incremented by 1
                            {
                                result += prefix8 + name + "[" + (size + 1).ToString() + "]" + suffix + NEW_LINE;
                                generateAsArray[index] = CyArrayType.OneDimArray;
                            }
                            else
                            {
                                result += prefix8 + name + suffix + NEW_LINE;
                                generateAsArray[index] = CyArrayType.NotArray;
                            }
                        }
                        result = TAB + result;
                        break;
                    case 2:
                        if (paged && pagedCommands > CyParamRange.PAGED_CMDS_MIN)
                        {
                            if (name == CyPMBusTable.SMBALERT_MASK) // SMBALERT_MASK is exception. Type is uint8
                            {
                                result += prefix8 + name + "[" + pagedCommands + "][" + (size + 1).ToString() +
                                    "]" + suffix + NEW_LINE;
                                generateAsArray[index] = CyArrayType.TwoDimArray;
                            }
                            else
                            {
                                result += prefix16 + name + "[" + pagedCommands + "]" + suffix + NEW_LINE;
                                generateAsArray[index] = CyArrayType.OneDimArray;
                            }
                        }
                        else
                        {
                            if (name == CyPMBusTable.SMBALERT_MASK) // SMBALERT_MASK is exception. Size incremented by 1
                            {
                                result += prefix8 + name + "[" + (size + 1).ToString() + "]" + suffix + NEW_LINE;
                                generateAsArray[index] = CyArrayType.OneDimArray;
                            }
                            else
                            {
                                result += prefix16 + name + suffix + NEW_LINE;
                                generateAsArray[index] = CyArrayType.NotArray;
                            }
                        }
                        result = TAB + result;
                        break;
                    default:
                        if (paged && pagedCommands > CyParamRange.PAGED_CMDS_MIN)
                        {
                            result += prefix8 + name + "[" + pagedCommands + "][" + (size + 1).ToString() +
                                "];" + NEW_LINE;
                            generateAsArray[index] = CyArrayType.TwoDimArray;
                        }
                        else
                        {
                            result += prefix8 + name + "[" + (size + 1).ToString() + "]" + suffix + NEW_LINE;
                            generateAsArray[index] = CyArrayType.OneDimArray;
                        }
                        result = TAB + result;
                        break;
                }
            }
            return result;
        }

        private static string GetNullStructureField(byte pagedCommands, string name, byte? size, bool paged,
            bool enable)
        {
            string result = string.Empty;
            if (enable && size != null)
            {

                switch (size)
                {
                    case 0:
                        break;
                    case 1:
                        if (paged && pagedCommands > CyParamRange.PAGED_CMDS_MIN)
                        {
                            result += "// " + name + NEW_LINE +
                                TAB + "{";
                            for (int i = 0; i < pagedCommands; i++)
                            {
                                result += "0x00, ";
                            }
                            result = result.TrimEnd(new char[] { ',', ' ' });
                            result += "},";
                        }
                        else
                        {
                            if (name == CyCustomTable.QUERY)
                            {
                                result += "// " + name + NEW_LINE +
                                TAB + "{0x00, 0x00},";
                            }
                            else
                            {
                                result += "// " + name + NEW_LINE +
                                TAB + "{0x00},";
                            }
                        }
                        result = TAB + result + NEW_LINE;
                        break;
                    case 2:
                        if (paged && pagedCommands > CyParamRange.PAGED_CMDS_MIN)
                        {
                            result += "// " + name + NEW_LINE +
                                    TAB + "{";
                            if (name == CyPMBusTable.SMBALERT_MASK)
                            {
                                for (int i = 0; i < pagedCommands; i++)
                                {
                                    result += "{0x00, 0x00, 0x00}, ";
                                }
                            }
                            else
                            {
                                for (int i = 0; i < pagedCommands; i++)
                                {
                                    result += "0x0000, ";
                                }
                            }
                            result = result.TrimEnd(new char[] { ',', ' ' });
                            result += "},";
                        }
                        else
                        {
                            if (name == CyPMBusTable.SMBALERT_MASK)
                            {
                                result += "// " + name + NEW_LINE +
                                    TAB + "{0x00, 0x00, 0x00},";
                            }
                            else
                            {
                                result += "// " + name + NEW_LINE +
                                    TAB + "{0x0000},";
                            }
                        }
                        result = TAB + result + NEW_LINE;
                        break;
                    default:
                        if (paged && pagedCommands > CyParamRange.PAGED_CMDS_MIN)
                        {
                            result += "// " + name + NEW_LINE;
                            for (int i = 0; i < pagedCommands; i++)
                            {
                                result += ((i == 0) ? TAB : SPACE) + "{";
                                for (int j = 0; j < (size + 1); j++)
                                {
                                    result += "0x00, ";
                                }
                                result = result.TrimEnd(new char[] { ',', ' ' });
                                result += "},";
                            }
                        }
                        else
                        {
                            result += "// " + name + NEW_LINE +
                                TAB + "{";
                            for (int i = 0; i < (size + 1); i++)
                            {
                                result += "0x00, ";
                            }
                            result = result.TrimEnd(new char[] { ',', ' ' });
                            result += "},";
                        }
                        result = TAB + result + NEW_LINE;
                        break;
                }
            }
            return result;
        }
        #endregion

        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            m_parameters = new CyParameters(args.InstQueryV1);

            if (m_parameters.CheckCustomTableNullValues() == false)
            {
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, string.Format(Resources.
                    DrcNullValuesError, Resources.CustomCmdsTabDisplayName));
            }
            if (m_parameters.CheckImplementationWithSilicon() == false)
            {
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error,
                    Resources.ImplementationWithSiliconValidator);
            }
        }
        #endregion
    }
}
