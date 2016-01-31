/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;


namespace USBFS_v2_12
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            
            // Get dict from main file. 
            if (customizers.Count > 0)
                paramDict = customizers[0].MacroDictionary;

            //Extract parameters
            CyUSBFSParameters parameters = new CyUSBFSParameters();
            parameters.GetParams(query);

            parameters.SetNodesISerialNumber();

            //Update Parameters to include last changes 
            parameters.RecalcDescriptors(parameters.m_hidReportTree.m_nodes[0]);
            parameters.RecalcDescriptors(parameters.m_deviceTree.m_nodes[0]);

            //Prepare m_hidReportTree for parsing (remove comment items)
            for (int i = 0; i < parameters.m_hidReportTree.m_nodes[0].m_nodes.Count; i++)
                for (int j = 0; j < parameters.m_hidReportTree.m_nodes[0].m_nodes[i].m_nodes.Count; j++)
                {
                    if (CyUSBFSParameters.StringIsComment(
                     ((CyHIDReportItemDescriptor)parameters.m_hidReportTree.m_nodes[0].m_nodes[i].m_nodes[j].m_value).
                     Item.Name))
                    {
                        parameters.m_hidReportTree.m_nodes[0].m_nodes[i].m_nodes.RemoveAt(j--);
                    }
                }

            // Generate the descriptor, storage and other data structures
            string instanceName = query.GetCommittedParam("INSTANCE_NAME").Value;
            CyUSBCodeGenerator g = new CyUSBCodeGenerator(instanceName, parameters);

            foreach (KeyValuePair<string, string> p in g.CodeParameterDictionary)
            {
                paramDict.Add(p.Key, p.Value);
            }

            //Save result
            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }
            return customizers;
        }

        #endregion
    }

    // Start refactoring into a class that returns a set of dictionaries similar to the
    // CyAPICustomizer
    internal class CyUSBCodeGenerator : CyUsbCodePrimitives
    {
        private const string APIGEN_DEFINES = "APIGEN_DEFINES";

        private const string APIGEN_DEVICE_DESCRIPTORS = "APIGEN_DEVICE_DESCRIPTORS";
        private const string APIGEN_DEVICE_TABLES = "APIGEN_DEVICE_TABLES";
        private const string DEF_NUM_DEVICES = "NUM_DEVICES";

        private const string APIGEN_STRING_DESCRIPTORS = "APIGEN_STRING_DESCRIPTORS";
        private const string APIGEN_SN_DESCRIPTOR = "APIGEN_SN_DESCRIPTOR";

        private const string DEF_DESCRIPTOR_STRING = "ENABLE_DESCRIPTOR_STRINGS";

        private const string DEF_ENABLE_STRINGS = "ENABLE_STRINGS";
        private const string DEF_MSOS_STRING = "ENABLE_MSOS_STRING";
        private const string DEF_SN_STRING = "ENABLE_SN_STRING";
        private const string DEF_FWSN_STRING = "ENABLE_FWSN_STRING";
        private const string DEF_IDSN_STRING = "ENABLE_IDSN_STRING";
        private const string DEF_DYN_ALLOCATION = "DYNAMIC_ALLOCATION";
        private const string DEF_MAX_REPORTID_NUMBER = "MAX_REPORTID_NUMBER";

        private const string APIGEN_HIDREPORT_DESCRIPTORS = "APIGEN_HIDREPORT_DESCRIPTORS";
        private const string APIGEN_HIDREPORT_TABLES = "APIGEN_HIDREPORT_TABLES";
        private const string DEF_ENABLE_HID_CLASS = "ENABLE_HID_CLASS";
        private const string DEF_ENABLE_AUDIO_CLASS = "ENABLE_AUDIO_CLASS";
        private const string DEF_ENABLE_AS_CLASS = "ENABLE_AUDIO_STREAMING";
        private const string DEF_ENABLE_MS_CLASS = "ENABLE_MIDI_STREAMING";
        private const string DEF_ENABLE_CDC_CLASS = "ENABLE_CDC_CLASS";
        private const string DEF_MIDI_IN_BUFF_SIZE = "MIDI_IN_BUFF_SIZE";
        private const string DEF_MIDI_OUT_BUFF_SIZE = "MIDI_OUT_BUFF_SIZE";
        //private const string DEF_ENABLE_CDC_CLASS_API = "ENABLE_CDC_CLASS_API";

        private const int SN_INDEX = 0;
        private const int EE_INDEX = 1;

        private byte m_maxReportID = 0;

        private readonly string m_instanceName;
        private readonly CyUSBFSParameters m_parameters;
        private readonly Dictionary<string, string> m_codeParameters = new Dictionary<string, string>();

        public CyUSBCodeGenerator(string InstanceName, CyUSBFSParameters Parameters) : base(InstanceName)
        {
            m_instanceName = InstanceName;
            m_parameters = Parameters;
            GenerateCode();
        }

        private void GenerateCode()
        {
            GenerateDevices();
            GenerateStrings();
            GenerateHIDReports();
        }

        /// <summary>
        /// Generates descriptors from the m_deviceTree
        /// </summary>
        private void GenerateDevices()
        {
            //Generate the descriptors
            CyUsbDescriptorGenTree gtDescr = new CyUsbDescriptorGenTree();
            string deviceDescr = gtDescr.Generate(m_parameters.m_deviceTree.m_nodes[0].m_nodes, m_instanceName, 
                                                  m_instanceName);
            // Calculate the element number in the table and store it in the m_hidClassReferences
            const string HID_CLASS_LABEL = "HID Class Descriptor\r\n";
            int indHid = -1;
            m_hidClassReferences.Clear();
            int indConfig = deviceDescr.IndexOf("Config Descriptor");
            indHid = deviceDescr.IndexOf(HID_CLASS_LABEL, indHid + 1);
            while (indHid >= 0)
            {
                int count = GetElemNumInArray(deviceDescr.Substring(indConfig, indHid - indConfig));
                m_hidClassReferences.Add(count);
                indHid = deviceDescr.IndexOf(HID_CLASS_LABEL, indHid + 1);
            }

            m_codeParameters.Add(APIGEN_DEVICE_DESCRIPTORS, deviceDescr);

            CyUsbDeviceRootTableGen genTable = new CyUsbDeviceRootTableGen(m_instanceName);
            string deviceTables = genTable.Generate(m_parameters.m_deviceTree.m_nodes[0].m_nodes, m_instanceName,
                                                    m_instanceName);
            m_codeParameters.Add(APIGEN_DEVICE_TABLES, deviceTables);
            AddDefine(DEF_NUM_DEVICES, m_parameters.m_deviceTree.m_nodes[0].m_nodes.Count.ToString());
            // Memory Allocation define
            if (m_parameters.EPMemoryAlloc == (byte)CyUSBFSParameters.CyMemoryAllocation.DYNAMIC)
            {
                AddDefine(DEF_DYN_ALLOCATION);
            }
            
            // Add Audio define if necessary
            if (deviceDescr.IndexOf("AUDIO") >= 0)
                AddDefine(DEF_ENABLE_AUDIO_CLASS);

            // Add AudioStreaming define if necessary
            if (deviceDescr.IndexOf("AudioStreaming") >= 0)
                AddDefine(DEF_ENABLE_AS_CLASS);

            // Add MIDI define if necessary
            if (deviceDescr.IndexOf("MIDI") >= 0)
            {
                AddDefine(DEF_ENABLE_MS_CLASS);

                // MIDI enpoints buffer size
                ushort inSize = 0;
                ushort outSize = 0;
                GetMidiBuffSize(m_parameters.m_deviceTree.m_nodes[0], ref inSize, ref outSize);
                AddDefine(DEF_MIDI_IN_BUFF_SIZE, HexWordString(inSize));
                AddDefine(DEF_MIDI_OUT_BUFF_SIZE, HexWordString(outSize));
            }
            // Add CDC define if necessary
            if (deviceDescr.IndexOf("CDC") >= 0)
                AddDefine(DEF_ENABLE_CDC_CLASS);
        }

        /// <summary>
        /// Generates descriptors from the m_stringTree
        /// </summary>
        private void GenerateStrings()
        {
            // String Descriptors
            bool enableStrings = false;
            CyUsbStringDescriptorGenTree gtStrings = new CyUsbStringDescriptorGenTree();
            string stringDescr = gtStrings.Generate(m_parameters.m_stringTree.m_nodes[0].m_nodes, m_instanceName,
                                                    m_instanceName);
            if (m_parameters.m_stringTree.m_nodes[0].m_nodes.Count > 1)
            {
                enableStrings = true;
                AddDefine(DEF_DESCRIPTOR_STRING);
            }
            // Special String support MS OS String (EE)
            CyStringDescriptor s = (CyStringDescriptor) m_parameters.m_stringTree.m_nodes[1].m_nodes[EE_INDEX].m_value;
            if (s.bUsed)
            {
                enableStrings = true;
                AddDefine(DEF_MSOS_STRING);
            }
            // Special String support SN String
            s = (CyStringDescriptor) m_parameters.m_stringTree.m_nodes[1].m_nodes[SN_INDEX].m_value;
            CyUsbStringSNDescriptorGen cg = new CyUsbStringSNDescriptorGen(m_instanceName);
            StringBuilder snCode = new StringBuilder(cg.GenHeader(s, m_instanceName, m_instanceName));
            if (s.bUsed)
            {
                enableStrings = true;
                snCode.Append(cg.GenBody(s, m_instanceName, m_instanceName));
                snCode.Append(cg.GenFooter(s, m_instanceName, m_instanceName));
                m_codeParameters.Add(APIGEN_SN_DESCRIPTOR, snCode.ToString());

                AddDefine(DEF_SN_STRING);
                    
                if (s.snType == CyStringGenerationType.USER_CALL_BACK)
                {
                    AddDefine(DEF_FWSN_STRING);
                }
                else if (s.snType == CyStringGenerationType.SILICON_NUMBER)
                {
                    AddDefine(DEF_IDSN_STRING);
                }
            }
            m_codeParameters.Add(APIGEN_STRING_DESCRIPTORS, stringDescr);
            if (enableStrings)
            {
                AddDefine(DEF_ENABLE_STRINGS);
            }
        }

        /// <summary>
        /// Generates descriptors from the m_hidReportTree
        /// </summary>
        private void GenerateHIDReports()
        {
            CyUsbHIDReportDescriptorGenTree gtHIDReport = new CyUsbHIDReportDescriptorGenTree();
            string HIDReportDescr = gtHIDReport.Generate(m_parameters.m_hidReportTree.m_nodes[0], m_instanceName,
                                                         m_instanceName);
            string HIDReportTables = GenReportTables(m_parameters.m_deviceTree.m_nodes[0], m_instanceName,
                                                     m_instanceName);

            if (HIDReportDescr.Length > 0)
            {
                AddDefine(DEF_ENABLE_HID_CLASS);
                m_codeParameters.Add(APIGEN_HIDREPORT_DESCRIPTORS, HIDReportDescr);
                m_codeParameters.Add(APIGEN_HIDREPORT_TABLES, HIDReportTables);
                for (int i = 0; i < m_parameters.m_hidReportTree.m_nodes[0].m_nodes.Count; i++)
                {
                    int rptIndex = i + 1;
                    AddDefine(string.Format("HID_RPT_{0}_SIZE_LSB", rptIndex),
                              String.Format("{0}", HexByteString((byte)
                                               (ReportDescriptorSize(m_parameters.m_hidReportTree.m_nodes[0].m_nodes[i].
                                                m_nodes) & 0xFF))));
                    AddDefine(string.Format("HID_RPT_{0}_SIZE_MSB", rptIndex),
                              String.Format("{0}", HexByteString((byte) ((ReportDescriptorSize(
                                                                              m_parameters.m_hidReportTree.m_nodes[0].
                                                                                  m_nodes[i].m_nodes) >> 8) & 0xFF))));
                }
            }

            AddDefine(DEF_MAX_REPORTID_NUMBER, m_maxReportID.ToString());
        }

        private string GenReportTables(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                string qual = GenerateQualName(qualName, node.m_nodes[i], i);
                code.Append(GenReportTables(node.m_nodes[i], instName, qual));
            }
            if (node.m_value != null)
            {
                if (node.m_value.bDescriptorType == CyUSBDescriptorType.HID)
                {
                    CyHIDDescriptor h = (CyHIDDescriptor) node.m_value;
                    code.Append(GenReportTable(h.bReportIndex, instName, qualName));
                }
            }
            return code.ToString();
        }

        private string GenReportTable(int rptIndex, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            if ((rptIndex > 0) && (rptIndex <= m_parameters.m_hidReportTree.m_nodes[0].m_nodes.Count))
            {
                CyDescriptorNode ReportTop = m_parameters.m_hidReportTree.m_nodes[0].m_nodes[rptIndex - 1];
                int[] reportIDIndexes = CheckReportIDHidItem(ReportTop);
                for (int i = 0; i < reportIDIndexes.Length; i++)
                {
                    m_maxReportID = (byte)Math.Max(m_maxReportID, reportIDIndexes[i]);
                }

                code.AppendFormat("#if !defined(USER_DEFINE_{0}_HID_RPT_STORAGE)\r\n", qualName);
                if (ReportCount(ReportTop, CyUSBFSParameters.RPTITEM_INPUT) > 0)
                {
                    code.AppendLine("/*********************************************************************");
                    code.AppendLine("* HID Input Report Storage");
                    code.AppendLine("*********************************************************************/");
                    code.AppendFormat("T_{0}_XFER_STATUS_BLOCK {1}_IN_RPT_SCB;{2}", instName, qualName, NEWLINE);
                    code.AppendLine(GenerateAllocateArray(qualName + "_IN_BUF", qualName + "_IN_BUF_SIZE"));
                    AddDefineNoInstance(qualName + "_IN_BUF_SIZE", 
                                        ReportSize(ReportTop, CyUSBFSParameters.RPTITEM_INPUT).ToString());
                    code.AppendLine("/*********************************************************************");
                    code.AppendLine("* HID Input Report TD Table");
                    code.AppendLine("*********************************************************************/");
                    code.Append(GenerateTDHeader(qualName + "_IN_RPT_TABLE[]"));
                    for (int i = 0; i < reportIDIndexes[0]; i++)
                    {
                        code.Append(GenerateTDEntry(NULL_PTR, qualName + "_IN_BUF[0]", NULL_PTR)).
                             Append(COMMA_NEWLINE);
                    }
                    code.AppendLine(GenerateTDEntry(qualName + "_IN_BUF_SIZE", qualName + "_IN_BUF[0]",
                                                    qualName + "_IN_RPT_SCB"));
                    code.AppendFormat("{0};{1}", GenerateFooter(), NEWLINE);
                    AddDefineNoInstance(qualName + "_NUM_IN_RPTS", 
                                        ReportCount(ReportTop, CyUSBFSParameters.RPTITEM_INPUT).ToString());
                }

                if (ReportCount(ReportTop, CyUSBFSParameters.RPTITEM_OUTPUT) > 0)
                {
                    code.AppendLine("/*********************************************************************");
                    code.AppendLine("* HID Output Report Storage");
                    code.AppendLine("*********************************************************************/");
                    code.AppendFormat("T_{0}_XFER_STATUS_BLOCK {1}_OUT_RPT_SCB;{2}", instName, qualName, NEWLINE);
                    code.AppendLine(GenerateAllocateArray(qualName + "_OUT_BUF", qualName + "_OUT_BUF_SIZE"));
                    AddDefineNoInstance(qualName + "_OUT_BUF_SIZE", 
                                        ReportSize(ReportTop, CyUSBFSParameters.RPTITEM_OUTPUT).ToString());
                    code.AppendLine("/*********************************************************************");
                    code.AppendLine("* HID Output Report TD Table");
                    code.AppendLine("*********************************************************************/");
                    code.Append(GenerateTDHeader(qualName + "_OUT_RPT_TABLE[]"));
                    for (int i = 0; i < reportIDIndexes[1]; i++)
                    {
                        code.Append(GenerateTDEntry(NULL_PTR, qualName + "_OUT_BUF[0]", NULL_PTR)).
                             Append(COMMA_NEWLINE);
                    }
                    code.AppendLine(GenerateTDEntry(qualName + "_OUT_BUF_SIZE", qualName + "_OUT_BUF[0]",
                                                    qualName + "_OUT_RPT_SCB"));
                    code.AppendFormat("{0};{1}", GenerateFooter(), NEWLINE);
                    AddDefineNoInstance(qualName + "_NUM_OUT_RPTS", 
                                        ReportCount(ReportTop, CyUSBFSParameters.RPTITEM_OUTPUT).ToString());
                }

                if (ReportCount(ReportTop, CyUSBFSParameters.RPTITEM_FEATURE) > 0)
                {
                    code.AppendLine("/*********************************************************************");
                    code.AppendLine("* HID Feature Report Storage");
                    code.AppendLine("*********************************************************************/");
                    code.AppendFormat("T_{0}_XFER_STATUS_BLOCK {1}_FEATURE_RPT_SCB;{2}", instName, qualName, NEWLINE);
                    code.AppendLine(GenerateAllocateArray(qualName + "_FEATURE_BUF", qualName + "_FEATURE_BUF_SIZE"));
                    AddDefineNoInstance(qualName + "_FEATURE_BUF_SIZE", 
                                        ReportSize(ReportTop, CyUSBFSParameters.RPTITEM_FEATURE).ToString());
                    code.AppendLine("/*********************************************************************");
                    code.AppendLine("* HID Feature Report TD Table");
                    code.AppendLine("*********************************************************************/");
                    code.Append(GenerateTDHeader(qualName + "_FEATURE_RPT_TABLE[]"));
                    for (int i = 0; i < reportIDIndexes[2]; i++)
                    {
                        code.Append(GenerateTDEntry(NULL_PTR, qualName + "_IN_BUF[0]", NULL_PTR)).
                             Append(COMMA_NEWLINE);
                    }
                    code.AppendLine(GenerateTDEntry(qualName + "_FEATURE_BUF_SIZE", qualName + "_FEATURE_BUF[0]",
                                                    qualName + "_FEATURE_RPT_SCB"));
                    code.AppendFormat("{0};{1}", GenerateFooter(), NEWLINE);
                    AddDefineNoInstance(qualName + "_NUM_FEATURE_RPTS", 
                                        ReportCount(ReportTop, CyUSBFSParameters.RPTITEM_FEATURE).ToString());
                }

                code.AppendLine("/*********************************************************************");
                code.AppendLine("* HID Report Look Up Table         This table has four entries:");
                code.AppendLine("*                                        IN Report Table");
                code.AppendLine("*                                        OUT Report Table");
                code.AppendLine("*                                        Feature Report Table");
                code.AppendLine("*                                        HID Report Descriptor");
                code.AppendLine("*                                        HID Class Descriptor");
                code.AppendLine("*********************************************************************/");
                code.Append(GenerateLUTHeader(qualName + "_TABLE[]"));
                AddDefineNoInstance(qualName + "_COUNT", "1"); // HID Class only has one LUT to link to the Interface

                if (ReportCount(ReportTop, "INPUT") > 0)
                {
                    code.Append(GenerateLUTEntry(qualName + "_IN_RPT_TABLE", (byte)(reportIDIndexes[0] + 1)));
                }
                else
                {
                    code.Append(GenerateNullLUTEntry());
                }
                code.Append(COMMA_NEWLINE);
                if (ReportCount(ReportTop, "OUTPUT") > 0)
                {
                    code.Append(GenerateLUTEntry(qualName + "_OUT_RPT_TABLE", (byte)(reportIDIndexes[1] + 1)));
                }
                else
                {
                    code.Append(GenerateNullLUTEntry());
                }
                code.Append(COMMA_NEWLINE);
                if (ReportCount(ReportTop, "FEATURE") > 0)
                {
                    code.Append(GenerateLUTEntry(qualName + "_FEATURE_RPT_TABLE", (byte)(reportIDIndexes[2] + 1)));
                }
                else
                {
                    code.Append(GenerateNullLUTEntry());
                }
                code.Append(COMMA_NEWLINE);
                string s = GenerateLUTEntry(string.Format("{0}_HIDREPORT_DESCRIPTOR{1}[0]", instName, rptIndex), 1);
                s = s.Replace("&", "(void *)&");
                code.Append(s);
                code.Append(COMMA_NEWLINE);
                // Find appropriate reference
                int ind = m_hidClassQualList.IndexOf(qualName.Replace("_HID", ""));
                s = GenerateLUTEntry(string.Format("{0}_DESCR[{1}]", qualName.Remove(qualName.IndexOf("_INTERFACE")),
                                     m_hidClassReferences[ind]), 1);
                s = s.Replace("&", "(void *)&");
                code.Append(s);
                
                code.AppendLine();
                code.AppendFormat("{0};{1}", GenerateFooter(), NEWLINE);
                code.AppendFormat("#endif /* USER_DEFINE_{0}_HID_RPT_STORAGE */{1}", qualName, NEWLINE);
            }
            return code.ToString();
        }

        private static byte ReportCount(CyDescriptorNode node, string rptType)
        {
            byte count = 0;
            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                count += ReportCount(node.m_nodes[i], rptType);
            }
            if (node.m_value != null)
            {
                if (node.m_value.bDescriptorType == CyUSBDescriptorType.HID_REPORT_ITEM)
                {
                    CyHIDReportItemDescriptor h = (CyHIDReportItemDescriptor) node.m_value;
                    if (h.Item.Name == rptType)
                    {
                        count += 1;
                    }
                }
            }
            // For now, only returns one or zero.  Update when we support storage for multiple reports
            if (count > 0)
                count = 1;
            return count;
        }

        private int[] CheckReportIDHidItem(CyDescriptorNode node)
        {
            int[] idValues = new int[] { 0, 0, 0 };
            List<int> idIndexes = new List<int>();
             List<int> idtmpValues = new List<int>();
            int inputIndex=0, outputIndex=0, featureIndex=0;

            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                if (node.m_nodes[i].m_value != null)
                {
                    if (node.m_nodes[i].m_value.bDescriptorType == CyUSBDescriptorType.HID_REPORT_ITEM)
                    {
                        CyHIDReportItemDescriptor h = (CyHIDReportItemDescriptor)node.m_nodes[i].m_value;
                        
                        switch (h.Item.Name)
                        {
                            case CyUSBFSParameters.RPTITEM_INPUT:
                                inputIndex = i;
                                break;
                            case CyUSBFSParameters.RPTITEM_OUTPUT:
                                outputIndex = i;
                                break;
                            case CyUSBFSParameters.RPTITEM_FEATURE:
                                featureIndex = i;
                                break;
                            case CyUSBFSParameters.RPTITEM_REPORT_ID:
                                if (h.Item.m_value.Count >= 2)
                                {
                                    idIndexes.Add(i);
                                    idtmpValues.Add(h.Item.m_value[1]);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (idIndexes.Count > 0)
            {
                for (int i = 0; i < idIndexes.Count; i++)
                {
                    if (idIndexes[i] < inputIndex)
                    {
                        idValues[0] = idtmpValues[i];
                        idValues[1] = idtmpValues[i];
                        idValues[2] = idtmpValues[i];
                    }
                    else if (idIndexes[i] < outputIndex)
                    {
                        idValues[1] = idtmpValues[i];
                        idValues[2] = idtmpValues[i];
                    }
                    else if (idIndexes[i] < featureIndex)
                        idValues[2] = idtmpValues[i];
                }
            }

            return idValues;
        }

        private ushort ReportDescriptorSize(List<CyDescriptorNode> nodes)
        {
            ushort size = 0;
            CyHIDReportItemDescriptor h;
            for (int i = 0; i < nodes.Count; i++)
            {
                h = (CyHIDReportItemDescriptor)nodes[i].m_value;
                size += CalcHIDItemLength(h);
            }
            return size;
        }

        private static uint ReportSize(CyDescriptorNode n, string itemType)
        {
            CyHIDReportItemDescriptor h;
            long fullSize = 0;
            long curSize = 0;
            long curCount = 0;
            for (int i = 0; i < n.m_nodes.Count; i++)
            {
                h = (CyHIDReportItemDescriptor)n.m_nodes[i].m_value;
                if ((h.Item.Name == CyUSBFSParameters.RPTITEM_REPORT_SIZE) ||
                    (h.Item.Name == CyUSBFSParameters.RPTITEM_REPORT_COUNT))
                {
                    List<byte> tmpArr = new List<byte>(h.Item.m_value);
                    if (tmpArr.Count > 0)
                        tmpArr.RemoveAt(0);
                    if (h.Item.Name == CyUSBFSParameters.RPTITEM_REPORT_SIZE)
                        curSize = CyUSBFSParameters.ConvertByteArrayToInt(tmpArr);
                    if (h.Item.Name == CyUSBFSParameters.RPTITEM_REPORT_COUNT)
                        curCount = CyUSBFSParameters.ConvertByteArrayToInt(tmpArr);
                }
                else if (h.Item.Name == itemType)
                {
                    fullSize += curSize * curCount;
                }
            }
            if (fullSize == 0)
                fullSize = curSize * curCount;
            // COnvert from bits to bytes
            if (fullSize % 8 == 0)
            {
                fullSize /= 8;
            }
            else
            {
                fullSize = fullSize/8 + 1;
            }
            // Max size is limited to 64 bytes
            if (fullSize > 64) fullSize = 64;
            // Add 1 byte for Report ID
            fullSize++;
            return (ushort)fullSize;
        }

        private void GetMidiBuffSize(CyDescriptorNode node, ref ushort inSize, ref ushort outSize)
        {
            if ((node.m_value != null) && (node.m_value is CyAudioInterfaceDescriptor) &&
                (((CyAudioInterfaceDescriptor)node.m_value).bInterfaceSubClass ==
                  (byte)CyUSBOtherTypes.CyAudioSubclassCodes.MIDISTREAMING))
            {
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    if ((node.m_nodes[i].m_value != null) && (node.m_nodes[i].m_value is CyEndpointDescriptor))
                    {
                        CyEndpointDescriptor epDesc = (CyEndpointDescriptor)node.m_nodes[i].m_value;
                        if (epDesc.Direction == CyUSBOtherTypes.CyEndptDirections.IN)
                        {
                            if (epDesc.wMaxPacketSize > inSize)
                                inSize = epDesc.wMaxPacketSize;
                        }
                        else
                        {
                            if (epDesc.wMaxPacketSize > outSize)
                                outSize = epDesc.wMaxPacketSize;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    GetMidiBuffSize(node.m_nodes[i], ref inSize, ref outSize);
                }
            }
        }

        /// <summary>
        /// Template for defines generation.
        /// </summary>
        /// <param name="d"></param>
        private void AddDefine(string d)
        {
            AddDefineNoInstance(string.Format("{0}_{1}", m_instanceName, d), "");
        }

        /// <summary>
        /// Template for defines generation.
        /// </summary>
        /// <param name="d"></param>
        private void AddDefine(string d, string v)
        {
            AddDefineNoInstance(string.Format("{0}_{1}", m_instanceName, d), v);
        }

        /// <summary>
        /// Template for defines generation.
        /// </summary>
        /// <param name="d"></param>
        private void AddDefineNoInstance(string d, string v)
        {
            string defs;
            bool noErr = m_codeParameters.TryGetValue(APIGEN_DEFINES, out defs);
            //Debug.Assert(noErr);
            m_codeParameters.Remove(APIGEN_DEFINES);
            defs += string.Format("#define {0}   {1}\r\n", d, v);
            m_codeParameters.Add(APIGEN_DEFINES, defs);
        }

        public Dictionary<string, string> CodeParameterDictionary
        {
            get { return m_codeParameters; }
        }
    }

    /// <summary>
    /// CyUsbCodePrimitives class contains miscellaneous functions for standard code generation 
    /// </summary>
    public class CyUsbCodePrimitives
    {
        private const int MAX_LINE_LENGTH = 120;
        private const int COMMENT_WIDTH = 40;
        public const string COMMA_NEWLINE = ",\r\n";
        public const string NEWLINE = "\r\n";
        public const string FOOTER_BRACKET = NEWLINE + "};" + NEWLINE;
        public const string CODE = "CYCODE";
        private string m_instanceName;

        public static List<int> m_hidClassReferences = new List<int>();
        public static List<string> m_hidClassQualList = new List<string>();

        public CyUsbCodePrimitives(string InstanceName)
        {
            m_instanceName = InstanceName;
        }

        public string GenerateAllocateArray(string n, int s)
        {
            return GenerateAllocateArray(n, s.ToString());
        }

        public string GenerateAllocateArray(string n, string s)
        {
            string code = String.Format("uint8 {0}[{1}            {2}];{1}", n, NEWLINE, s);
            return code;
        }

        public byte MSB(ushort i)
        {
            return (byte) (0xff & (i >> 8));
        }

        public byte LSB(ushort i)
        {
            return (byte) (0xff & i);
        }

        public string HexByteString(byte b)
        {
            return string.Format("0x{0:X2}u", b);
        }

        public string HexWordString(ushort w)
        {
            return string.Format("0x{0:X4}u", w);
        }

        public string GenerateDescrByte(string c, byte b)
        {
            return GenerateDescrByteArray(c, new byte[] {b});
        }

        public string GenerateDescrDWLS(string c, ushort u)
        {
            return GenerateDescrByteArray(c, new byte[] { LSB(u), MSB(u) });
        }

        public string GenerateDescr3Byte(string c, uint u)
        {
            return GenerateDescrByteArray(c, new byte[]
                                             {(byte) (0xff & u), (byte) (0xff & (u >> 8)), (byte) (0xff & (u >> 16))});
        }

        public string GenerateDescrInt(string c, uint u)
        {
            return GenerateDescrByteArray(c, new byte[] { (byte)(0xff & u), (byte)(0xff & (u >> 8)), 
                                                          (byte)(0xff & (u >> 16)), (byte)(0xff & (u >> 24)) });
        }

        public string GenerateDescrByteArray(string c, byte[] b)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("/* {0}*/ ", c.PadRight(COMMENT_WIDTH, ' '));
            if (b.Length > 0)
            {
                for (int i = 0; i < b.Length - 1; i++)
                {
                    s.Append(HexByteString(b[i]));
                    s.Append(", ");
                }
                s.Append(HexByteString(b[b.Length - 1]));
            }
            return s.ToString();
        }

        public string GenerateDescrDWLSArray(string c, ushort[] b)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("/* {0}*/ ", c.PadRight(COMMENT_WIDTH, ' '));
            if (b.Length > 0)
            {
                for (int i = 0; i < b.Length - 1; i++)
                {
                    s.Append(HexByteString(LSB(b[i])));
                    s.Append(", ");
                    s.Append(HexByteString(MSB(b[i])));
                    s.Append(", ");
                }
                s.Append(HexByteString(LSB(b[b.Length - 1])));
                s.Append(", ");
                s.Append(HexByteString(MSB(b[b.Length - 1])));
            }
            return s.ToString();
        }
        
        public string GenerateDescr3ByteArray(string c, uint[] b)
        {
            return GenerateDescrCustomArray(c, b, 3);
        }

        public string GenerateDescrCustomArray(string c, uint[] b, int num)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("/* {0}*/ ", c.PadRight(COMMENT_WIDTH, ' '));
            int len = s.Length;
            if (b.Length > 0)
            {
                for (int i = 0; i < b.Length; i++)
                {
                    for (int j = 0; j < num; j++)
                    {
                        if (j < 4) // max possible num for uint is 4
                            s.Append(HexByteString((byte)(0xff & (b[i] >> (8 * j)))));
                        else
                            s.Append(HexByteString(0x00));
                        s.Append(", ");
                    }
                    if (i < b.Length - 1)
                    {
                        s.AppendLine();
                        s.Append("".PadLeft(len));
                    }
                }

                s = new StringBuilder(s.ToString().TrimEnd(' ', ','));
            }
            return s.ToString();
        }

        public string GeneratePtr(string d)
        {
            string s = string.Format("    &{0}", d);
            return s;
        }

        public string GenerateUsbUnicode(string d)
        {
            StringBuilder s = new StringBuilder();
            string comma = "";
            int lineLength = 0;
            
            if (String.IsNullOrEmpty(d)==false)
            {
                for (int i = 0; i < d.Length; i++)
                {
                    if (lineLength > 72)
                    {
                        s.AppendLine();
                        lineLength = 0;
                    }
                    s.Append(comma);
                    s.AppendFormat("'{0}', 0", d.Substring(i, 1));
                    comma = ",";
                    lineLength += 8;
                }
            }
            return s.ToString();
        }

        public string GenerateVoidPtr(string d)
        {
            string s = string.Format("    (void *)({0})", d);
            return s;
        }

        public string GenerateReportItem(CyHIDReportItemDescriptor d)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("/* {0}*/ ", d.Item.Name.PadRight(COMMENT_WIDTH, ' '));
            for (byte i = 0; i < CalcHIDItemLength(d); i++)
            {
                s.Append(HexByteString(d.Item.m_value[i]) + ", ");
            }
            s.Append(NEWLINE);
            return s.ToString();
        }

        public ushort CalcHIDItemLength(CyHIDReportItemDescriptor d)
        {
            //Per HID 1.1 Secion 6.2.2.2
            ushort r = 0;
            switch (d.Item.m_size)
            {
                case 0:
                    r = 1;
                    break;
                case 1:
                    r = 2;
                    break;
                case 2:
                    r = 3;
                    break;
                case 3:
                    r = 5;
                    break;
            }
            return r;
        }

        public string GenerateSymbolicByte(string c, string sym)
        {
            string s = string.Format("/* {0}*/ ", c.PadRight(COMMENT_WIDTH, ' '));
            s += sym;
            return s;
        }

        public string GenerateWordCount(string c, ushort u)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("/* {0}*/ ", c.PadRight(COMMENT_WIDTH, ' '));
            s.Append(HexByteString(LSB(u)));
            s.Append(", ");
            s.Append(HexByteString(MSB(u)));
            return s.ToString();
        }

        public string GenerateByte(byte b)
        {
            string s = HexByteString(b);
            return s;
        }

        public string GenerateWord(ushort w)
        {
            string s = HexWordString(w);
            return s;
        }

        public string GenerateLUTHeader(string n)
        {
            string s = String.Format("const T_{0}_LUT {1} {2} = {{{3}", INSTANCE_NAME, CODE, n, Environment.NewLine);
            return s;
        }

        public string GenerateLUTEntry(string n, byte c)
        {
            StringBuilder s = new StringBuilder();
            s.Append("    {");
            s.Append(HexByteString(c));
            s.Append(", ");
            s.Append(GeneratePtr(n));
            s.Append("}");
            return s.ToString();
        }

        public string GenerateLUTEntry(string n, string c)
        {
            string s = String.Format("    {{{0}, {1}}}", c, GeneratePtr(n));
            if (s.Length > MAX_LINE_LENGTH)
                s = s.Insert(s.IndexOf(',') + 2, "\r\n");
            return s;
        }

        public string GenerateNullLUTEntry()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("    {{{0},    {1}}}", HexByteString(0), NULL_PTR);
            return s.ToString();
        }

        public string GenerateTDHeader(string n)
        {
            string s = String.Format("const T_{0}_TD {1} {2} = {{{3}", INSTANCE_NAME, CODE, n, NEWLINE);
            return s;
        }

        public string GenerateTDEntry(byte c, string p, string d)
        {
            StringBuilder s = new StringBuilder();
            s.Append("    {{");
            s.AppendFormat("{0}, ", HexByteString(c));
            s.Append(p == NULL_PTR ? NULL_PTR : GeneratePtr(p));
            s.Append(", ");
            s.Append(d == NULL_PTR ? NULL_PTR : GeneratePtr(d));
            s.Append("}}");
            return s.ToString();
        }

        public string GenerateTDEntry(string c, string p, string d)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("    {{{0}{1}", c, COMMA_NEWLINE);
            s.Append(p == NULL_PTR ? ("    " + NULL_PTR) : GeneratePtr(p));
            s.Append(COMMA_NEWLINE);
            s.Append(d == NULL_PTR ? ("    " + NULL_PTR) : GeneratePtr(d));
            s.Append("}");
            return s.ToString();
        }

        public string GenerateNullTDEntry()
        {
            string s = String.Format("    {{{0}, {0}, {0}}}", HexByteString(0));
            return s;
        }

        public string GenerateFooter()
        {
            return "}";
        }

        public static string GenerateQualName(string qual, CyDescriptorNode node, int i)
        {
            if (node.m_value.bDescriptorType == CyUSBDescriptorType.ALTERNATE)
            {
                qual = String.Format("{0}_INTERFACE{1}", qual, i);
            }
            else if (node.m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE)
            {
                qual = String.Format("{0}_ALTERNATE{1}", qual, i);
            }
            else if (node.m_value.bDescriptorType == CyUSBDescriptorType.HID)
            {
                qual = String.Format("{0}_{1}", qual, node.m_value.bDescriptorType);
            }
            else
            {
                qual = String.Format("{0}_{1}", qual, node.m_value.bDescriptorType + i.ToString());
            }
            return qual;
        }

        public static int GetElemNumInArray(string code)
        {
            int sum = -1;
            int ind1 = code.LastIndexOf('{');
            while (ind1 >= 0)
            {
                sum++;
                ind1 = code.IndexOf(',', ind1+1);
            }
            return sum;
        }

        public string INSTANCE_NAME
        {
            get { return m_instanceName; }
            set { m_instanceName = value; }
        }

        public string NULL_PTR
        {
            get { return "NULL"; }
        }
    }

    #region Descriptor Code Generators

    public abstract class CyUsbCodeGen : CyUsbCodePrimitives
    {
        protected string m_nl;

        public CyUsbCodeGen(string InstanceName) : base(InstanceName)
        {
            m_nl = "";
        }

        public virtual string Generate(List<CyDescriptorNode> nodes, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyUsbCodeGenMaker f = new CyUsbCodeGenMaker();
            CyUsbDescriptorGenTree cgt = new CyUsbDescriptorGenTree();

            for (int i = 0; i < nodes.Count; i++)
            {
                string qual = GenerateQualName(qualName,nodes[i], i); 
                CyUsbCodeGen cg = f.MakeDescriptorGenerator(nodes[i].m_value, INSTANCE_NAME);
                code.Append(cg.GenHeader(nodes[i].m_value, instName, qual));
                code.Append(cg.GenBody(nodes[i].m_value, instName, qual));
                code.Append(cgt.Generate(nodes[i].m_nodes, instName, qual));
                code.Append(cg.GenFooter(nodes[i].m_value, instName, qual));
            }
            return code.ToString();
        }

        public virtual string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            return String.Empty;
        }

        public abstract string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName);
        public abstract string GenBody(CyUSBDescriptor ud, string instanceName, string qualName);
        public abstract string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName);

        protected string GenLUTEntry(string instanceName, string tableSuffix, byte c)
        {
            StringBuilder code = new StringBuilder();
            code.Append(m_nl);
            m_nl = COMMA_NEWLINE;
            code.Append(GenerateLUTEntry(instanceName + tableSuffix, c));
            return code.ToString();
        }

        protected string GenNullLUTEntry()
        {
            StringBuilder code = new StringBuilder();
            code.Append(m_nl);
            m_nl = COMMA_NEWLINE;
            code.Append(GenerateNullLUTEntry());
            return code.ToString();
        }
    }

    internal class CyUsbDeviceDescriptorGen : CyUsbCodeGen
    {
        public CyUsbDeviceDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyDeviceDescriptor d = (CyDeviceDescriptor) ud;
            // Header Comment
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Device Descriptors");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("const uint8 {0} {1}_DESCR[] = {{{2}", CODE, qualName, NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("Descriptor Length", d.bLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}",
                              GenerateDescrByte("DescriptorType: DEVICE", (byte) d.bDescriptorType), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrDWLS("bcdUSB (ver 2.0)", 0x0200), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("bDeviceClass", d.bDeviceClass), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("bDeviceSubClass", d.bDeviceSubClass), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("bDeviceProtocol", d.bDeviceProtocol), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("bMaxPacketSize0", 8), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrDWLS("idVendor", d.idVendor), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrDWLS("idProduct", d.idProduct), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrDWLS("bcdDevice", d.bcdDevice),COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("iManufacturer", d.iManufacturer), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("iProduct", d.iProduct), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("iSerialNumber", d.iSerialNumber),COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("bNumConfigurations", d.bNumConfigurations), NEWLINE);
            code.AppendLine("};");

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbConfigDescriptorGen : CyUsbCodeGen
    {
        public CyUsbConfigDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyConfigDescriptor d = (CyConfigDescriptor) ud;
            // Header Comment
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Config Descriptor  ");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("const uint8 {0} {1}_DESCR[] = {{{2}", CODE, qualName, NEWLINE);

            code.AppendFormat("{0}{1}", GenerateDescrByte(" Config Descriptor Length", d.bLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: CONFIG", (byte) d.bDescriptorType),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTotalLength", d.wTotalLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bNumInterfaces", d.bNumInterfaces), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bConfigurationValue", (byte) (d.bConfigurationValue + 1)),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" iConfiguration", d.iConfiguration), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bmAttributes", d.bmAttributes), COMMA_NEWLINE);
            code.Append(GenerateDescrByte(" bMaxPower", d.bMaxPower));

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return FOOTER_BRACKET;
        }
    }

    internal class CyUsbInterfaceGeneralDescriptorGen : CyUsbCodeGen
    {
        public CyUsbInterfaceGeneralDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbInterfaceDescriptorGen : CyUsbCodeGen
    {
        public CyUsbInterfaceDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyInterfaceDescriptor d = (CyInterfaceDescriptor) ud;
            string isAudioCDC = ((ud is CyAudioInterfaceDescriptor) && (d.bInterfaceSubClass == 
                                    (byte)CyUSBOtherTypes.CyAudioSubclassCodes.MIDISTREAMING)) ? "MIDIStreaming " :
                                ((ud is CyAudioInterfaceDescriptor) && (d.bInterfaceSubClass ==
                                    (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOSTREAMING)) ? "AudioStreaming " :
                                    ((ud is CyAudioInterfaceDescriptor) && (d.bInterfaceSubClass ==
                                    (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL)) ? "AudioControl " :
                                (ud is CyAudioInterfaceDescriptor) ? "Audio " :
                                (ud is CyDataInterfaceDescriptor) ? "Data " :
                                (ud is CyCDCInterfaceDescriptor) ? "CDC " : String.Empty;
            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendFormat("* {0}Interface Descriptor", isAudioCDC).AppendLine();
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(" Interface Descriptor Length", d.bLength),COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: INTERFACE", (byte) d.bDescriptorType),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bInterfaceNumber", d.bInterfaceNumber), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bAlternateSetting", d.bAlternateSetting), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bNumEndpoints", d.bNumEndpoints), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bInterfaceClass", d.bInterfaceClass), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bInterfaceSubClass", d.bInterfaceSubClass), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bInterfaceProtocol", d.bInterfaceProtocol),COMMA_NEWLINE);
            code.Append(GenerateDescrByte(" iInterface", d.iInterface));

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbEndpointDescriptorGen : CyUsbCodeGen
    {
        public CyUsbEndpointDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenBody(node.m_value, instName, qualName));
            code.Append(GenFooter(node.m_value, instName, qualName));
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyEndpointDescriptor d = (CyEndpointDescriptor) ud;
            // Fix EndpointAddress issue
            if ((d.bEndpointAddress & 0x0F) == 0)
            {
                d.bEndpointAddress |= 1;
            }
            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Endpoint Descriptor");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(" Endpoint Descriptor Length", d.bLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: ENDPOINT", (byte) d.bDescriptorType),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bEndpointAddress", d.bEndpointAddress), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bmAttributes", d.bmAttributes), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wMaxPacketSize", d.wMaxPacketSize), COMMA_NEWLINE);
            code.Append(GenerateDescrByte(" bInterval", d.bInterval));

            if (ud is CyAudioEndpointDescriptor)
            {
                CyAudioEndpointDescriptor d1 = (CyAudioEndpointDescriptor)ud;
                code.Append(COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bRefresh", d1.bRefresh), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" bSynchAddress", d1.bSynchAddress));
            }

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbASEndpointDescriptorGen : CyUsbCodeGen
    {
        public CyUsbASEndpointDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenBody(node.m_value, instName, qualName));
            code.Append(GenFooter(node.m_value, instName, qualName));
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyASEndpointDescriptor d = (CyASEndpointDescriptor)ud;

            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* AS Endpoint Descriptor");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(" Endpoint Descriptor Length", d.bLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: CS_ENDPOINT", (byte)d.bDescriptorType),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bmAttributes", d.bmAttributes), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bLockDelayUnits", d.bLockDelayUnits), COMMA_NEWLINE);
            code.AppendFormat(GenerateDescrDWLS(" wLockDelay", d.wLockDelay));

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbMSEndpointDescriptorGen : CyUsbCodeGen
    {
        public CyUsbMSEndpointDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenBody(node.m_value, instName, qualName));
            code.Append(GenFooter(node.m_value, instName, qualName));
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyMSEndpointDescriptor d = (CyMSEndpointDescriptor)ud;

            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* MS Bulk Data Endpoint Descriptor");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(" Endpoint Descriptor Length", d.bLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: CS_ENDPOINT", (byte)d.bDescriptorType),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bNumEmbMIDIJack", d.bNumEmbMIDIJack), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baAssocJackID", d.baAssocJackID), "");

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbHIDClassDescriptorGen : CyUsbCodeGen
    {
        public CyUsbHIDClassDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenBody(node.m_value, instName, qualName));
            code.Append(GenFooter(node.m_value, instName, qualName));
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyHIDDescriptor d = (CyHIDDescriptor) ud;
            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* HID Class Descriptor");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(" HID Class Descriptor Length", d.bLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: HID_CLASS", (byte) d.bDescriptorType),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrDWLS(" bcdHID", 0x0111), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bCountryCode", d.bCountryCode), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bNumDescriptors", d.bNumDescriptors), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorType", d.bDescriptorType1), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateSymbolicByte(" wDescriptorLength (LSB)",
                                                             string.Format("{0}_HID_RPT_{1}_SIZE_LSB", instanceName,
                                                                           d.bReportIndex)), COMMA_NEWLINE);
            code.Append(GenerateSymbolicByte(" wDescriptorLength (MSB)",
                                         string.Format("{0}_HID_RPT_{1}_SIZE_MSB", instanceName, d.bReportIndex)));

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbAudioDescriptorGen : CyUsbCodeGen
    {
        public CyUsbAudioDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenBody(node.m_value, instName, qualName));
            code.Append(GenFooter(node.m_value, instName, qualName));
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendFormat("* {0} Descriptor", ud).AppendLine();
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(String.Format(" {0} Descriptor Length", ud), ud.bLength),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: AUDIO", (byte)ud.bDescriptorType),
                                  COMMA_NEWLINE);

            if (ud is CyACHeaderDescriptor)
            {
                CyACHeaderDescriptor d = (CyACHeaderDescriptor) ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" bcdADC", d.bcdADC), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTotalLength", d.wTotalLength), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bInCollection", d.bInCollection),
                                  d.bInCollection > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByteArray(" baInterfaceNr", d.baInterfaceNr));
            }
            else if (ud is CyACInputTerminalDescriptor)
            {
                CyACInputTerminalDescriptor d = (CyACInputTerminalDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bTerminalID", d.bTerminalID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTerminalType", d.wTerminalType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bAssocTerminal", d.bAssocTerminal), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wChannelConfig", d.wChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iTerminal", d.iTerminal));

            }
            else if (ud is CyACOutputTerminalDescriptor)
            {
                CyACOutputTerminalDescriptor d = (CyACOutputTerminalDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bTerminalID", d.bTerminalID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTerminalType", d.wTerminalType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bAssocTerminal", d.bAssocTerminal), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bSourceID", d.bSourceID), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iTerminal", d.iTerminal));

            }
            else if (ud is CyACMixerUnitDescriptor)
            {
                CyACMixerUnitDescriptor d = (CyACMixerUnitDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wChannelConfig", d.wChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" bmControls", d.bmControls.ToArray()),
                                  d.bmControls.Count > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByte(" iMixer", d.iMixer));

            }
            else if (ud is CyACSelectorUnitDescriptor)
            {
                CyACSelectorUnitDescriptor d = (CyACSelectorUnitDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByte(" iSelector", d.iSelector));

            }
            else if (ud is CyACFeatureUnitDescriptor)
            {
                CyACFeatureUnitDescriptor d = (CyACFeatureUnitDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bSourceID", d.bSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bControlSize", d.bControlSize), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrCustomArray(" bmaControls", d.bmaControls, d.bControlSize),
                                  (d.bmaControls.Length > 0) && (d.bControlSize > 0) ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByte(" iFeature", d.iFeature));

            }
            else if (ud is CyACProcessingUnitDescriptor)
            {
                CyACProcessingUnitDescriptor d = (CyACProcessingUnitDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wProcessType", d.wProcessType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wChannelConfig", d.wChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bControlSize", d.bControlSize), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" bmControls", d.bmControls),
                                  d.bmControls.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByte(" iProcessing", d.iProcessing));
            }
            else if (ud is CyACExtensionDescriptor)
            {
                CyACExtensionDescriptor d = (CyACExtensionDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wExtensionCode", d.wExtensionCode), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wChannelConfig", d.wChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bControlSize", d.bControlSize), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" bmControls", d.bmControls),
                                  d.bmControls.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByte(" iExtension", d.iExtension));

            }
            else if (ud is CyASGeneralDescriptor)
            {
                CyASGeneralDescriptor d = (CyASGeneralDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bTerminalLink", d.bTerminalLink), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDelay", d.bDelay), COMMA_NEWLINE);
                code.Append(GenerateDescrDWLS(" wFormatTag", d.wFormatTag));

            }
            else if ((ud is CyASFormatType1Descriptor) || (ud is CyASFormatType2Descriptor))
            {
                CyASFormatTypeBaseDescriptor d = (CyASFormatTypeBaseDescriptor) ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte) d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bFormatType", (byte) d.bFormatType), COMMA_NEWLINE);
                
                if (ud is CyASFormatType1Descriptor)
                {
                    CyASFormatType1Descriptor d1 = (CyASFormatType1Descriptor)ud;

                    code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d1.bNrChannels), COMMA_NEWLINE);
                    code.AppendFormat("{0}{1}", GenerateDescrByte(" bSubframeSize", d1.bSubframeSize), COMMA_NEWLINE);
                    code.AppendFormat("{0}{1}", GenerateDescrByte(" bBitResolution", d1.bBitResolution), COMMA_NEWLINE);
                }
                else if (ud is CyASFormatType2Descriptor)
                {
                    CyASFormatType2Descriptor d1 = (CyASFormatType2Descriptor)ud;

                    code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wMaxBitRate", d1.wMaxBitRate), COMMA_NEWLINE);
                    code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wSamplesPerFrame", d1.wSamplesPerFrame),
                                      COMMA_NEWLINE);
                }

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bSamFreqType", d.bSamFreqType), COMMA_NEWLINE);
                if (d.bSamFreqType == 0)
                {
                    code.AppendFormat("{0}{1}", GenerateDescrByteArray(" tLowerSamFreq", 
                        CyUSBFSParameters.ConvertInt3ToByteArray(d.tLowerSamFreq).ToArray()), COMMA_NEWLINE);
                    code.Append(GenerateDescrByteArray(" tUpperSamFreq",
                        CyUSBFSParameters.ConvertInt3ToByteArray(d.tUpperSamFreq).ToArray()));
                }
                else
                {
                    code.Append(GenerateDescr3ByteArray(" tSamFreq", d.tSamFreq));
                }
            }
            //---------------------------------------------------------------------------------------------------------
            #region v2.0

            if (ud is CyACHeaderDescriptor_v2)
            {
                CyACHeaderDescriptor_v2 d = (CyACHeaderDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" bcdADC", d.bcdADC), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bCategory", d.bCategory), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTotalLength", d.wTotalLength), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" bmControls", d.bmControls));
            }
            else if (ud is CyACClockSourceDescriptor_v2)
            {
                CyACClockSourceDescriptor_v2 d = (CyACClockSourceDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bClockID", d.bClockID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmAttributes", d.bmAttributes), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bAssocTerminal", d.bAssocTerminal), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iClockSource", d.iClockSource));

            }
            else if (ud is CyACClockSelectorDescriptor_v2)
            {
                CyACClockSelectorDescriptor_v2 d = (CyACClockSelectorDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bClockID", d.bClockID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baCSourceID", d.baCSourceID),
                                    d.baCSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iClockSelector", d.iClockSelector));

            }
            else if (ud is CyACClockMultiplierDescriptor_v2)
            {
                CyACClockMultiplierDescriptor_v2 d = (CyACClockMultiplierDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bClockID", d.bClockID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bCSourceID", d.bCSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iClockMultiplier", d.iClockMultiplier));

            }
            else if (ud is CyACInputTerminalDescriptor_v2)
            {
                CyACInputTerminalDescriptor_v2 d = (CyACInputTerminalDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bTerminalID", d.bTerminalID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTerminalType", d.wTerminalType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bAssocTerminal", d.bAssocTerminal), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bCSourceID", d.bCSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrInt(" bmChannelConfig", d.bmChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iTerminal", d.iTerminal));

            }
            else if (ud is CyACOutputTerminalDescriptor_v2)
            {
                CyACOutputTerminalDescriptor_v2 d = (CyACOutputTerminalDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bTerminalID", d.bTerminalID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTerminalType", d.wTerminalType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bAssocTerminal", d.bAssocTerminal), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bSourceID", d.bSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bCSourceID", d.bCSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iTerminal", d.iTerminal));

            }
            else if (ud is CyACMixerUnitDescriptor_v2)
            {
                CyACMixerUnitDescriptor_v2 d = (CyACMixerUnitDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrInt(" bmChannelConfig", d.bmChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" bmMixerControls", d.bmMixerControls.ToArray()),
                                  d.bmMixerControls.Count > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iMixer", d.iMixer));

            }
            else if (ud is CyACSelectorUnitDescriptor_v2)
            {
                CyACSelectorUnitDescriptor_v2 d = (CyACSelectorUnitDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iSelector", d.iSelector));

            }
            else if (ud is CyACFeatureUnitDescriptor_v2)
            {
                CyACFeatureUnitDescriptor_v2 d = (CyACFeatureUnitDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bSourceID", d.bSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrCustomArray(" bmaControls", d.bmaControls, 4),
                                  (d.bmaControls.Length > 0) ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByte(" iFeature", d.iFeature));
            }
            else if (ud is CyACSamplingRateConverterDescriptor_v2)
            {
                CyACSamplingRateConverterDescriptor_v2 d = (CyACSamplingRateConverterDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bSourceID", d.bSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bCSourceInID", d.bCSourceInID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bCSourceOutID", d.bCSourceOutID), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iSRC", d.iSRC));
            }
            else if (ud is CyACEffectUnitDescriptor_v2)
            {
                CyACEffectUnitDescriptor_v2 d = (CyACEffectUnitDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wEffectType", (ushort)d.wEffectType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bSourceID", d.bSourceID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrCustomArray(" bmaControls", d.bmaControls, 4),
                                  d.bmaControls.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByte(" iEffects", d.iEffects));
            }
            else if (ud is CyACProcessingUnitDescriptor_v2)
            {
                CyACProcessingUnitDescriptor_v2 d = (CyACProcessingUnitDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wProcessType", d.wProcessType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrInt(" bmChannelConfig", d.bmChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iProcessing", d.iProcessing));
            }
            else if (ud is CyACExtensionDescriptor_v2)
            {
                CyACExtensionDescriptor_v2 d = (CyACExtensionDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bUnitID", d.bUnitID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wExtensionCode", d.wExtensionCode), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInPins", d.bNrInPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.baSourceID.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrInt(" bmChannelConfig", d.bmChannelConfig), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iChannelNames", d.iChannelNames), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iExtension", d.iExtension));

            }
            else if (ud is CyASGeneralDescriptor_v2)
            {
                CyASGeneralDescriptor_v2 d = (CyASGeneralDescriptor_v2)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bTerminalLink", d.bTerminalLink), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmControls", d.bmControls), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bFormatType", d.bFormatType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrInt(" bmFormats", d.bmFormats), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrChannels", d.bNrChannels), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrInt(" bmChannelConfig", d.bmChannelConfig), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" iChannelNames", d.iChannelNames));

            }
            #endregion

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbMidiDescriptorGen : CyUsbCodeGen
    {
        public CyUsbMidiDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenBody(node.m_value, instName, qualName));
            code.Append(GenFooter(node.m_value, instName, qualName));
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendFormat("* {0} Descriptor", ud).AppendLine();
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(String.Format(" {0} Descriptor Length", ud), ud.bLength),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: AUDIO", (byte)ud.bDescriptorType),
                                  COMMA_NEWLINE);

            if (ud is CyMSHeaderDescriptor)
            {
                CyMSHeaderDescriptor d = (CyMSHeaderDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" bcdADC", d.bcdMSC), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrDWLS(" wTotalLength", d.wTotalLength), "");
            }
            else if (ud is CyMSInJackDescriptor)
            {
                CyMSInJackDescriptor d = (CyMSInJackDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bJackType", (byte)d.bJackType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bJackID", d.bJackID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iJack", d.iJack), "");

            }
            else if (ud is CyMSOutJackDescriptor)
            {
                CyMSOutJackDescriptor d = (CyMSOutJackDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bJackType", (byte)d.bJackType), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bJackID", d.bJackID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInputPins", d.bNrInputPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.bNrInputPins > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourcePin),
                                  d.bNrInputPins > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iJack", d.iJack), "");

            }
            else if (ud is CyMSElementDescriptor)
            {
                CyMSElementDescriptor d = (CyMSElementDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", (byte)d.bDescriptorSubtype),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bElementID", d.bElementID), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrInputPins", d.bNrInputPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourceID),
                                  d.bNrInputPins > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" baSourceID", d.baSourcePin),
                                  d.bNrInputPins > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bNrOutPins", d.bNrOutPins), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bInTerminalLink", d.bInTerminalLink), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bOutTerminalLink", d.bOutTerminalLink), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" bElCapsSize", d.bElCapsSize), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByteArray(" bmElementCaps", d.bmElementCaps),
                                  d.bNrInputPins > 0 ? COMMA_NEWLINE : NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte(" iElement", d.iElement), "");

            }

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbCDCDescriptorGen : CyUsbCodeGen
    {
        public CyUsbCDCDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenBody(node.m_value, instName, qualName));
            code.Append(GenFooter(node.m_value, instName, qualName));
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            // Header Comment
            code.AppendLine(",");
            code.AppendLine("/*********************************************************************");
            code.AppendFormat("* {0} Descriptor", ud).AppendLine();
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("{0}{1}", GenerateDescrByte(String.Format(" {0} Descriptor Length", ud), ud.bLength),
                              COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" DescriptorType: CS_INTERFACE", 0x24), // 0x24 hardcoded
                                  COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte(" bDescriptorSubtype", 
                                        (byte)((CyCDCClassDescriptor)ud).bDescriptorSubtype), COMMA_NEWLINE);

            if (ud is CyCDCHeaderDescriptor)
            {
                CyCDCHeaderDescriptor d = (CyCDCHeaderDescriptor)ud;

                code.Append(GenerateDescrDWLS(" bcdADC", d.bcdADC));
            }
            else if (ud is CyCDCUnionDescriptor)
            {
                CyCDCUnionDescriptor d = (CyCDCUnionDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bControlInterface", d.bControlInterface),
                                    d.bSubordinateInterface.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrByteArray(" bSubordinateInterface", d.bSubordinateInterface));

            }
            else if (ud is CyCDCCountrySelectionDescriptor)
            {
                CyCDCCountrySelectionDescriptor d = (CyCDCCountrySelectionDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" iCountryCodeRelDate", d.iCountryCodeRelDate),
                                            d.wCountryCode.Length > 0 ? COMMA_NEWLINE : NEWLINE);
                code.Append(GenerateDescrDWLSArray(" wCountryCode", d.wCountryCode));

            }
            else if (ud is CyCDCCallManagementDescriptor)
            {
                CyCDCCallManagementDescriptor d = (CyCDCCallManagementDescriptor)ud;

                code.AppendFormat("{0}{1}", GenerateDescrByte(" bmCapabilities", d.bmCapabilities), COMMA_NEWLINE);
                code.Append(GenerateDescrByte(" bDataInterface", d.bDataInterface));
            }
            else if (ud is CyCDCAbstractControlMgmtDescriptor)
            {
                CyCDCAbstractControlMgmtDescriptor d = (CyCDCAbstractControlMgmtDescriptor)ud;

                code.Append(GenerateDescrByte(" bmCapabilities", d.bmCapabilities));
            }
            
            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbStringDescriptorGen : CyUsbCodeGen
    {
        public CyUsbStringDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            CyStringDescriptor d = (CyStringDescriptor) ud;
            StringBuilder code = new StringBuilder();
            // Header Comment
            code.Append(COMMA_NEWLINE);
            code.AppendLine("/*********************************************************************");
            code.AppendFormat("* String Descriptor: \"{0}\"\r\n", d.bString);
            code.AppendLine("*********************************************************************/");
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyStringDescriptor d = (CyStringDescriptor) ud;
            
            if (d.bString != null)
            {
                code.AppendFormat("{0}{1}", GenerateDescrByte("Descriptor Length", (byte) (2 + (d.bString.Length*2))),
                                  COMMA_NEWLINE);
                code.AppendFormat("{0}{1} ", GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType),
                                  COMMA_NEWLINE);
                code.Append(GenerateUsbUnicode(d.bString));
            }
            else
            {
                code.AppendFormat("{0}{1}", GenerateDescrByte("Descriptor Length", 2), COMMA_NEWLINE);
                code.Append(GenerateDescrByte("DescriptorType: STRING",  (byte) d.bDescriptorType));
            }

            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbStringZeroDescriptorGen : CyUsbCodeGen
    {
        public CyUsbStringZeroDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Language ID Descriptor has to be the 0th descriptor, so it will anchor
            // all of the string descriptors
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* String Descriptor Table");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("const uint8 {0} {1}_STRING_DESCRIPTORS[] = {{{2}", CODE, qualName, NEWLINE);
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Language ID Descriptor");
            code.AppendLine("*********************************************************************/");
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            CyStringZeroDescriptor d = (CyStringZeroDescriptor) ud;
            StringBuilder code = new StringBuilder();
            code.AppendFormat("{0}{1}", GenerateDescrByte("Descriptor Length", d.bLength), COMMA_NEWLINE);
            code.AppendFormat("{0}{1}", GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType),
                              COMMA_NEWLINE);
            code.Append(GenerateDescrDWLS("Language Id", d.wLANGID));
            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Language ID Descriptor has to be the 0th descriptor, so it will close
            // all of the string descriptors
            code.Append(COMMA_NEWLINE);
            code.AppendLine("/*********************************************************************/");
            code.AppendLine(GenerateDescrByte("Marks the end of the list.", 0) + "};");
            code.AppendLine("/*********************************************************************/");
            return code.ToString();
        }
    }

    internal class CyUsbStringSNDescriptorGen : CyUsbCodeGen
    {
        public CyUsbStringSNDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Serial Number String Descriptor
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Serial Number String Descriptor");
            code.AppendLine("*********************************************************************/");
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            CyStringDescriptor d = (CyStringDescriptor) ud;
            StringBuilder code = new StringBuilder();
            code.AppendFormat("const uint8 {0} {1}_SN_STRING_DESCRIPTOR[] = {{{2}", CODE, qualName, NEWLINE);
            if (d.bString != null)
            {
                code.AppendFormat("{0}{1}", GenerateDescrByte("Descriptor Length", d.bLength), COMMA_NEWLINE);
                code.AppendFormat("{0}{1}", GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType),
                                  COMMA_NEWLINE);
                code.Append(GenerateUsbUnicode(d.bString));
            }
            else
            {
                code.AppendFormat("{0}{1}", GenerateDescrByte("Descriptor Length", 2), COMMA_NEWLINE);
                code.Append(GenerateDescrByte("DescriptorType: STRING",  (byte) d.bDescriptorType));
            }
            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            // Microsoft Operating System Descriptor
            string code = String.Format("{0}}};{0}", NEWLINE);
            return code;
        }
    }

    internal class CyUsbStringMSOSDescriptorGen : CyUsbCodeGen
    {
        public CyUsbStringMSOSDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Microsoft Operating System Descriptors
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Microsoft Operating System Descriptor");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("const uint8 {0} {1}_MSOS_DESCRIPTOR[] = {{{2}", CODE, qualName, NEWLINE);
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            CyStringDescriptor d = (CyStringDescriptor) ud;
            StringBuilder code = new StringBuilder();
            code.Append(GenerateDescrByte("Descriptor Length", d.bLength));
            code.Append(COMMA_NEWLINE);
            code.Append(GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType));
            code.Append(COMMA_NEWLINE);
            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            // Microsoft Operating System Descriptor
            return FOOTER_BRACKET;
        }
    }

    internal class CyUsbHIDReportDescriptorGen : CyUsbCodeGen
    {
        public CyUsbHIDReportDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyHIDReportDescriptor d = (CyHIDReportDescriptor)ud;
            // Header Comment
            code.AppendLine("/*********************************************************************");
            code.AppendFormat("* HID Report Descriptor: {0}{1}", d.Name, NEWLINE);
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("const uint8 {0} {1} = {{{2}", CODE, qualName, NEWLINE);
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            CyHIDReportDescriptor d = (CyHIDReportDescriptor) ud;
            StringBuilder code = new StringBuilder();
            code.AppendLine("/*********************************************************************");
            code.AppendFormat("* HID Report Descriptor: {0}{1}", d.Name, NEWLINE);
            code.AppendLine("*********************************************************************/");
            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine("/*********************************************************************/");
            code.Append(GenerateDescrDWLS("End of the HID Report Descriptor", 0));
            code.AppendLine("};");
            code.AppendLine("/*********************************************************************/");
            return code.ToString();
        }
    }

    internal class CyUsbHIDReportItemGen : CyUsbCodeGen
    {
        public CyUsbHIDReportItemGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            CyHIDReportItemDescriptor d = (CyHIDReportItemDescriptor) ud;
            return GenerateReportItem(d);
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    internal class CyUsbUnknownDescriptorGen : CyUsbCodeGen
    {
        public CyUsbUnknownDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Header Comment
            code.AppendLine("/*********************************************************************");
            code.AppendLine(string.Format("* Unknown Descriptor {0}:::{1}", ud.bDescriptorType, ud.bLength));
            code.AppendLine("*********************************************************************/");
            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }
    }

    #endregion Descriptor Code Generators

    #region Table Generators

    internal class CyUsbDeviceRootTableGen : CyUsbCodeGen
    {
        public CyUsbDeviceRootTableGen(string InstanceName) : base(InstanceName)
        {
            m_nl = "";
        }

        public override string Generate(List<CyDescriptorNode> nodes, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Generate the children
            for (int i = 0; i < nodes.Count; i++)
            {
                CyUsbDeviceTableGen cgt = new CyUsbDeviceTableGen(INSTANCE_NAME);
                string qual = GenerateQualName(qualName,nodes[i], i);
                code.Append(cgt.Generate(nodes[i], instName, qual));
            }

            code.Append(GenHeader(nodes[0].m_value, instName, instName));
            for (int i = 0; i < nodes.Count; i++)
            {
                string qual = GenerateQualName(qualName,nodes[i], i);
                code.Append(this.GenLUTEntry(qual, "_TABLE", (byte) nodes[i].m_nodes.Count));
            }
            code.Append(GenFooter(nodes[0].m_value, instName, instName));
            return code.ToString();
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            Debug.Assert(false);
            return String.Empty;
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Device Root Table Header
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Device Table -- Indexed by the device number.");
            code.AppendLine("*********************************************************************/");
            code.Append(GenerateLUTHeader(qualName + "_TABLE[]"));
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            Debug.Assert(false);
            return String.Empty;
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return FOOTER_BRACKET;
        }
    }

    internal class CyUsbDeviceTableGen : CyUsbCodeGen
    {
        public CyUsbDeviceTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Generate the children
            if (node.m_nodes.Count > 0)
            {
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    CyUsbConfigTableGen cgt = new CyUsbConfigTableGen(INSTANCE_NAME);
                    string qual = GenerateQualName(qualName, node.m_nodes[i], i);
                    code.Append(cgt.Generate(node.m_nodes[i], instName, qual));
                }
            }

            code.Append(GenHeader(node.m_value, instName, qualName));
            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                string qual = GenerateQualName(qualName, node.m_nodes[i], i);
                code.Append(this.GenLUTEntry(qual, "_TABLE", (byte) node.m_nodes.Count));
            }
            code.Append(GenFooter(node.m_value, instName, qualName));

            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyDeviceDescriptor d = (CyDeviceDescriptor) ud;
            // Device  Table Header
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Device Dispatch Table -- Points to the Device Descriptor and each of");
            code.AppendLine("*                          and Configuration Tables for this Device ");
            code.AppendLine("*********************************************************************/");
            code.Append(GenerateLUTHeader(qualName + "_TABLE[]"));
            code.Append(GenLUTEntry(qualName, "_DESCR", d.bNumConfigurations));
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            Debug.Assert(false);
            return String.Empty;
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return FOOTER_BRACKET;
        }
    }

    internal class CyUsbConfigTableGen : CyUsbCodeGen
    {
        public CyUsbConfigTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            // Generate any child tables
            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                CyUsbInterfaceTableGen cgt = new CyUsbInterfaceTableGen(INSTANCE_NAME);
                string qual = GenerateQualName(qualName, node.m_nodes[i], i);
                code.Append(cgt.Generate(node.m_nodes[i], instName, qual));
                
            }
            // Generate the Endpoint Table
            CyUsbEndportTableGen gtEP = new CyUsbEndportTableGen(INSTANCE_NAME);
            code.Append(gtEP.Generate(node, instName, qualName));


            // Generate the interface class table
            code.AppendFormat("const uint8 {0} {1}_INTERFACE_CLASS[] = {{", CODE, qualName).AppendLine();
            for (int i = 0; i < node.m_nodes.Count; i++)
                for (int j = 0; j < Math.Min(1, node.m_nodes[i].m_nodes.Count); j++)
                {
                    CyDescriptorNode nodeAlternate = node.m_nodes[i].m_nodes[j];
                    if ((nodeAlternate.m_value != null) && (nodeAlternate.m_value is CyInterfaceDescriptor))
                    {
                        CyInterfaceDescriptor desc = (CyInterfaceDescriptor) nodeAlternate.m_value;
                        code.Append(GenerateByte(desc.bInterfaceClass));
                        if ((i != node.m_nodes.Count -1) || (j != node.m_nodes[i].m_nodes.Count - 1))
                            code.Append(", ");
                    }
                }
            code.AppendLine().AppendLine("};");

            code.Append(GenHeader(node.m_value, instName, qualName));
            code.Append(GenLUTEntry(qualName, "_EP_SETTINGS_TABLE", gtEP.GetNumEPDescriptors(node)));

            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                string qual = GenerateQualName(qualName, node.m_nodes[i], i);

                if (node.m_nodes[i].m_nodes.Count > 0)
                {
                    code.Append(GenInterfaceTblPtr(node.m_nodes[i].m_nodes[0], (byte) node.m_nodes[i].m_nodes.Count,
                                               instName, qual));
                }
            }
            code.Append(GenLUTEntry(qualName, "_INTERFACE_CLASS", 0));
            code.Append(GenFooter(node.m_value, instName, qualName));

            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Device  Table Header
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Config Dispatch Table -- Points to the Config Descriptor and each of");
            code.AppendLine("*                          and endpoint setup table and to each");
            code.AppendLine("*                          interface table if it specifies a USB Class");
            code.AppendLine("*********************************************************************/");
            code.Append(GenerateLUTHeader(qualName + "_TABLE[]"));
            code.Append(GenLUTEntry(qualName, "_DESCR", 1));
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return String.Empty;
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            string code = "\r\n};\r\n";
            return code;
        }

        private string GenInterfaceTblPtr(CyDescriptorNode node, byte alternateCount, string instanceName, 
                                          string qualName)
        {
            StringBuilder code = new StringBuilder();
            CyInterfaceDescriptor d = (CyInterfaceDescriptor) node.m_value;
            // If class in not HID, don't generate Interface table
            if (d.bInterfaceClass != CyUSBFSParameters.CLASS_HID)
            {
                code.Append(GenNullLUTEntry());
            }
            else
            {
                code.Append(GenLUTEntry(qualName, "_TABLE", alternateCount));
            }
           
            return code.ToString();
        }
    }

    internal class CyUsbInterfaceTableGen : CyUsbCodeGen
    {
        public CyUsbInterfaceTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            bool foundClassDescr = false;
            for (int j = 0; j < node.m_nodes.Count; j++)
            {
                if (FindClassDescriptor(node.m_nodes[j]))
                {
                    foundClassDescr = true;
                    break;
                }
            }
            if (foundClassDescr)
            {
                code.Append(GenHeader(node.m_value, instName, qualName));
                for (int j = 0; j < node.m_nodes.Count; j++)
                {
                    CyDescriptorNode nodeAlternate = node.m_nodes[j];

                    if (FindClassDescriptor(nodeAlternate))
                    {
                        string qualAlt = GenerateQualName(qualName, nodeAlternate, j);
                        for (int i = 0; i < nodeAlternate.m_nodes.Count; i++)
                        {
                            string qual = GenerateQualName(qualAlt, nodeAlternate.m_nodes[i], i);
                            code.Append(GenBody(nodeAlternate.m_nodes[i].m_value, qual, qual));
                        }
                    }
                }
                code = new StringBuilder(code.ToString().TrimEnd('\r', '\n', ','));
                code.Append(GenFooter(node.m_value, instName, qualName));
            }
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Device  Table Header
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Interface Dispatch Table -- Points to the Class Dispatch Tables");
            code.AppendLine("*********************************************************************/");
            code.Append(GenerateLUTHeader(qualName + "_TABLE[]"));
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            if (ud.bDescriptorType != CyUSBDescriptorType.ENDPOINT)
            {
                code.Append(GenerateLUTEntry(qualName + "_TABLE", qualName + "_COUNT"));
                code.Append(",\r\n");
            }
            return code.ToString();
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            string code = "\r\n};\r\n";
            return code;
        }

        private bool FindClassDescriptor(CyDescriptorNode node)
        {
            bool found = false;

            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                CyDescriptorNode n = node.m_nodes[i];
                if (n.m_nodes.Count > 0)
                {
                    found = FindClassDescriptor(n);
                }
                if (n.m_value.bDescriptorType == CyUSBDescriptorType.HID)
                {
                    found = true;
                }
            }
            return found;
        }
    }

    internal class CyUsbEndportTableGen : CyUsbCodeGen
    {
        public CyUsbEndportTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            bool noEndpoints = true;
            if (node.m_value.bDescriptorType == CyUSBDescriptorType.CONFIGURATION)
            {
                code.Append(GenHeader(node.m_value, instName, qualName));
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    CyDescriptorNode igd = node.m_nodes[i];
                    for (int j = 0; j < igd.m_nodes.Count; j++)
                    {
                        CyDescriptorNode id = igd.m_nodes[j];
                        CyInterfaceDescriptor d = (CyInterfaceDescriptor) id.m_value;
                        byte CurInterface = d.bInterfaceNumber;
                        byte curInterfaceClass = d.bInterfaceClass;
                        byte CurAltSetting = d.bAlternateSetting;
                        for (int k = 0; k < id.m_nodes.Count; k++)
                        {
                            CyDescriptorNode en = id.m_nodes[k];
                            if (en.m_value.bDescriptorType == CyUSBDescriptorType.ENDPOINT)
                            {
                                code.Append(FormatEndpointSetting(CurInterface, CurAltSetting,
                                    (CyEndpointDescriptor)en.m_value, curInterfaceClass));
                                noEndpoints = false;
                            }
                        }
                    }
                }
                if (noEndpoints)
                {
                    // Add {NULL} value to the array
                    code.Append("{" + NULL_PTR + "}");
                }
                code.Append(GenFooter(node.m_value, instName, qualName));
            }
            return code.ToString();
        }

        public override string GenHeader(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            StringBuilder code = new StringBuilder();
            // Table Header
            code.AppendLine("/*********************************************************************");
            code.AppendLine("* Endpoint Setting Table -- This table contain the endpoint setting");
            code.AppendLine("*                           for each endpoint in the configuration. It");
            code.AppendLine("*                           contains the necessary information to");
            code.AppendLine("*                           configure the endpoint hardware for each");
            code.AppendLine("*                           interface and alternate setting.");
            code.AppendLine("*********************************************************************/");
            code.AppendFormat("const T_{0}_EP_SETTINGS_BLOCK {1} {2}_EP_SETTINGS_TABLE[] = {{{3}", INSTANCE_NAME, CODE,
                              qualName, NEWLINE);
            code.AppendLine("/* IFC  ALT    EPAddr bmAttr MaxPktSize Class ********************/");
            return code.ToString();
        }

        public override string GenBody(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            return string.Empty;
        }

        public override string GenFooter(CyUSBDescriptor ud, string instanceName, string qualName)
        {
            string code = String.Format("{0}}};{0}", NEWLINE);
            return code;
        }

        public byte GetNumEPDescriptors(CyDescriptorNode node)
        {
            byte count = 0;
            if ((node.m_value != null) && (node.m_value.bDescriptorType == CyUSBDescriptorType.ENDPOINT))
            {
                count = 1;
            }
            else
            {
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    count += GetNumEPDescriptors(node.m_nodes[i]);
                }
            }
            return count;
        }

        private string FormatEndpointSetting(byte ifc, byte alt, CyEndpointDescriptor d, byte ifclass)
        {
            StringBuilder code = new StringBuilder();
            code.Append(m_nl);
            code.Append("{");
            code.AppendFormat("{0}, ", GenerateByte(ifc));
            code.AppendFormat("{0}, ", GenerateByte(alt));
            code.AppendFormat("{0}, ", GenerateByte(d.bEndpointAddress));
            code.AppendFormat("{0}, ", GenerateByte(d.bmAttributes));
            code.AppendFormat("{0},   ", GenerateWord(d.wMaxPacketSize));
            //code.Append(GenerateByte(d.DoubleBuffer ? (byte) 1 : (byte) 0)); // buffer type
            code.Append(GenerateByte(ifclass));
            code.Append("}");
            m_nl = COMMA_NEWLINE;
            return code.ToString();
        }
    }

    #endregion

    #region Tree Generators

    internal abstract class CyGenTree
    {
        public abstract string Generate(List<CyDescriptorNode> nodes, string instName, string qualName);
        public abstract string Generate(CyDescriptorNode node, string instName, string qualName);
    }

    internal class CyUsbDescriptorGenTree : CyGenTree
    {
        public CyUsbDescriptorGenTree()
        {
        }

        public override string Generate(List<CyDescriptorNode> nodes, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            CyUsbCodeGenMaker f = new CyUsbCodeGenMaker();

            CyUsbDescriptorGenTree cgt = new CyUsbDescriptorGenTree();

            for (int i = 0; i < nodes.Count; i++)
            {
                string qual = CyUsbCodePrimitives.GenerateQualName(qualName, nodes[i], i); 
                CyUsbCodeGen cg = f.MakeDescriptorGenerator(nodes[i].m_value, instName);
                code.Append(cg.GenHeader(nodes[i].m_value, instName, qual));
                code.Append(cg.GenBody(nodes[i].m_value, instName, qual));
                if (nodes[i].m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE)
                {
                    CyDescriptorNode n = nodes[i];

                    // Class Descriptors first
                    for (int j = 0; j < n.m_nodes.Count; j++)
                    {
                        if (n.m_nodes[j].m_value.bDescriptorType != CyUSBDescriptorType.ENDPOINT)
                        {
                            code.Append(cgt.Generate(n.m_nodes[j], instName, qual));

                            if (n.m_nodes[j].m_value.bDescriptorType == CyUSBDescriptorType.HID)
                                CyUsbCodePrimitives.m_hidClassQualList.Add(qual);
                        }
                    }
                    // Now, the endpoints
                    for (int j = 0; j < n.m_nodes.Count; j++)
                    {
                        if (n.m_nodes[j].m_value.bDescriptorType == CyUSBDescriptorType.ENDPOINT)
                        {
                            code.Append(cgt.Generate(n.m_nodes[j], instName, qual));
                            // Class-specific endpoints
                            for (int k = 0; k < n.m_nodes[j].m_nodes.Count; k++)
                            {
                                code.Append(cgt.Generate(n.m_nodes[j].m_nodes[k], instName, qual));
                            }
                        }
                    }
                }
                else
                {
                    code.Append(cgt.Generate(nodes[i].m_nodes, instName, qual));
                }
                code.Append(cg.GenFooter(nodes[i].m_value, instName, qual));
            }
            return code.ToString();
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            CyUsbCodeGenMaker f = new CyUsbCodeGenMaker();
            CyUsbCodeGen cg = f.MakeDescriptorGenerator(node.m_value, instName);
            string code = cg.Generate(node, instName, qualName);

            return code;
        }
    }

    internal class CyUsbStringDescriptorGenTree : CyGenTree
    {
        public CyUsbStringDescriptorGenTree()
        {
        }

        public override string Generate(List<CyDescriptorNode> nodes, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            if (nodes.Count > 1)
            {
                CyUsbCodeGenMaker f = new CyUsbCodeGenMaker();

                //The zeroth string should be the LangID/Zero code generator
                CyUsbCodeGen zcg = f.MakeDescriptorGenerator(nodes[0].m_value, instName);

                code.Append(zcg.GenHeader(nodes[0].m_value, instName, qualName));
                code.Append(zcg.GenBody(nodes[0].m_value, instName, qualName));

                for (int i = 1; i < nodes.Count; i++)
                {
                    CyUsbCodeGen cg = f.MakeDescriptorGenerator(nodes[i].m_value, instName);
                    code.Append(cg.GenHeader(nodes[i].m_value, instName, qualName));
                    code.Append(cg.GenBody(nodes[i].m_value, instName, qualName));
                    code.Append(cg.GenFooter(nodes[i].m_value, instName, qualName));
                }
                code.Append(zcg.GenFooter(nodes[0].m_value, instName, qualName));
            }
            return code.ToString();
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            Debug.Assert(false);
            return String.Empty;
        }
    }

    internal class CyUsbHIDReportDescriptorGenTree : CyGenTree
    {
        public CyUsbHIDReportDescriptorGenTree()
        {
        }

        public override string Generate(List<CyDescriptorNode> nodes, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            if (nodes.Count > 0)
            {
                CyUsbCodeGenMaker f = new CyUsbCodeGenMaker();
                CyUsbCodeGen cg = f.MakeDescriptorGenerator(nodes[0].m_value, instName);
                CyUsbHIDReportItemGenTree hgt = new CyUsbHIDReportItemGenTree();
                CyUsbCodePrimitives prim = new CyUsbCodePrimitives(instName);

                for (int i = 0; i < nodes.Count; i++)
                {
                    int rptIndex = i + 1;
                    string qualHead = string.Format("{0}_HIDREPORT_DESCRIPTOR{1}[]", qualName, rptIndex);
                    code.Append(cg.GenHeader(nodes[i].m_value, instName, qualHead));
                    code.Append(prim.GenerateSymbolicByte(" Descriptor Size (Not part of descriptor)",
                                  qualName + "_HID_RPT_" + rptIndex + "_SIZE_LSB" + CyUsbCodePrimitives.COMMA_NEWLINE +
                                  qualName + "_HID_RPT_" + rptIndex + "_SIZE_MSB") + CyUsbCodePrimitives.COMMA_NEWLINE);
                    code.Append(hgt.Generate(nodes[i], instName, qualName)); // Generate the HID Report Items
                    code.Append(cg.GenFooter(nodes[i].m_value, instName, qualName));
                }
            }
            return code.ToString();
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            return Generate(node.m_nodes, instName, qualName);
        }

        private ushort ReportSize(List<CyDescriptorNode> nodes, CyUsbCodePrimitives p)
        {
            ushort size = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                CyHIDReportItemDescriptor h = (CyHIDReportItemDescriptor) nodes[i].m_value;
                size += p.CalcHIDItemLength(h);
            }
            return size;
        }
    }

    internal class CyUsbHIDReportItemGenTree : CyGenTree
    {
        public CyUsbHIDReportItemGenTree()
        {
        }

        public override string Generate(List<CyDescriptorNode> nodes, string instName, string qualName)
        {
            StringBuilder code = new StringBuilder();

            if (nodes.Count > 0)
            {
                CyUsbCodeGenMaker f = new CyUsbCodeGenMaker();
                CyUsbCodeGen cg = f.MakeDescriptorGenerator(nodes[0].m_value, instName);

                for (int i = 0; i < nodes.Count; i++)
                {
                    code.Append(cg.GenBody(nodes[i].m_value, instName, qualName));
                }
                code.Append(cg.GenFooter(nodes[0].m_value, instName, qualName));
            }
            return code.ToString();
        }

        public override string Generate(CyDescriptorNode node, string instName, string qualName)
        {
            return Generate(node.m_nodes, instName, qualName);
        }
    }

    internal class CyUsbCodeGenMaker
    {
        public CyUsbCodeGenMaker()
        {
        }

        public CyUsbCodeGen MakeDescriptorGenerator(CyUSBDescriptor d, string instName)
        {
            CyUsbCodeGen c;

            Type d_t = d.GetType();

            if (d_t == typeof (CyDeviceDescriptor))
            {
                c = new CyUsbDeviceDescriptorGen(instName);
            }
            else if (d_t == typeof (CyConfigDescriptor))
            {
                c = new CyUsbConfigDescriptorGen(instName);
            }
            else if (d_t == typeof (CyInterfaceGeneralDescriptor))
            {
                c = new CyUsbInterfaceGeneralDescriptorGen(instName);
            }
            else if ((d_t == typeof(CyInterfaceDescriptor)) || (d_t == typeof(CyAudioInterfaceDescriptor)) ||
                (d_t == typeof(CyCommunicationsInterfaceDescriptor)) || (d_t == typeof(CyDataInterfaceDescriptor)))
            {
                c = new CyUsbInterfaceDescriptorGen(instName);
            }
            else if ((d_t == typeof(CyEndpointDescriptor)) || (d_t == typeof(CyAudioEndpointDescriptor)))
            {
                c = new CyUsbEndpointDescriptorGen(instName);
            }
            else if (d_t == typeof(CyASEndpointDescriptor))
            {
                c = new CyUsbASEndpointDescriptorGen(instName);
            }
            else if (d_t == typeof(CyMSEndpointDescriptor))
            {
                c = new CyUsbMSEndpointDescriptorGen(instName);
            }
            else if (d_t == typeof (CyHIDDescriptor))
            {
                c = new CyUsbHIDClassDescriptorGen(instName);
            }
            else if (d_t == typeof (CyStringDescriptor))
            {
                c = new CyUsbStringDescriptorGen(instName);
            }
            else if (d_t == typeof (CyStringZeroDescriptor))
            {
                c = new CyUsbStringZeroDescriptorGen(instName);
            }
            else if (d_t == typeof (CyHIDReportDescriptor))
            {
                c = new CyUsbHIDReportDescriptorGen(instName);
            }
            else if (d_t == typeof (CyHIDReportItemDescriptor))
            {
                c = new CyUsbHIDReportItemGen(instName);
            }
            else if ((d_t == typeof(CyACHeaderDescriptor)) || 
                (d_t == typeof(CyACInputTerminalDescriptor)) || 
                (d_t == typeof(CyACOutputTerminalDescriptor)) || 
                (d_t == typeof(CyACMixerUnitDescriptor)) || 
                (d_t == typeof(CyACSelectorUnitDescriptor)) ||
                (d_t == typeof(CyACFeatureUnitDescriptor)) || 
                (d_t == typeof(CyACProcessingUnitDescriptor)) || 
                (d_t == typeof(CyACExtensionDescriptor)) || 
                (d_t == typeof(CyASGeneralDescriptor)) || 
                (d_t == typeof(CyASFormatType1Descriptor)) ||
                (d_t == typeof(CyASFormatType2Descriptor)) ||

                (d_t == typeof(CyACHeaderDescriptor_v2)) ||
                (d_t == typeof(CyACClockSourceDescriptor_v2)) ||
                (d_t == typeof(CyACClockSelectorDescriptor_v2)) ||
                (d_t == typeof(CyACClockMultiplierDescriptor_v2)) ||
                (d_t == typeof(CyACInputTerminalDescriptor_v2)) ||
                (d_t == typeof(CyACOutputTerminalDescriptor_v2)) ||
                (d_t == typeof(CyACMixerUnitDescriptor_v2)) ||
                (d_t == typeof(CyACSelectorUnitDescriptor_v2)) ||
                (d_t == typeof(CyACFeatureUnitDescriptor_v2)) ||
                (d_t == typeof(CyACSamplingRateConverterDescriptor_v2)) ||
                (d_t == typeof(CyACEffectUnitDescriptor_v2)) ||
                (d_t == typeof(CyACProcessingUnitDescriptor_v2)) ||
                (d_t == typeof(CyACExtensionDescriptor_v2)) ||
                (d_t == typeof(CyASGeneralDescriptor_v2)))
            {
                c = new CyUsbAudioDescriptorGen(instName);
            }
            else if ((d_t == typeof (CyMSHeaderDescriptor)) ||
                (d_t == typeof(CyMSInJackDescriptor)) ||
                (d_t == typeof(CyMSOutJackDescriptor)) ||
                (d_t == typeof(CyMSElementDescriptor)))
            {
                c = new CyUsbMidiDescriptorGen(instName);
            }
            else if ((d_t == typeof(CyCDCHeaderDescriptor)) ||
                (d_t == typeof(CyCDCUnionDescriptor)) ||
                (d_t == typeof(CyCDCCountrySelectionDescriptor)) ||
                (d_t == typeof(CyCDCCallManagementDescriptor)) ||
                (d_t == typeof(CyCDCAbstractControlMgmtDescriptor)))
            {
                c = new CyUsbCDCDescriptorGen(instName);
            }
            else
            {
                Debug.Assert(false);
                c = null;
            }

            return c;
        }
    }

    #endregion Tree Generators
}
