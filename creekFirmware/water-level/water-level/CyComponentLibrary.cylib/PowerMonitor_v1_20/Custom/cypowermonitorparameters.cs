/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;

namespace PowerMonitor_v1_20
{
    #region Component Parameters Names
    public class CyParamNames
    {
        public const string AUX_FILTER_TYPE = "AuxFilterType";
        public const string CURRENT_FILTER_TYPE = "CurrentFilterType";
        public const string FAULT_SOURCES_OC = "FaultSources_OC";
        public const string FAULT_SOURCES_OV = "FaultSources_OV";
        public const string FAULT_SOURCES_UV = "FaultSources_UV";
        public const string NUM_AUX_CHANNELS = "NumAuxChannels";
        public const string NUM_CONVERTERS = "NumConverters";
        public const string PGOOD_CONFIG = "PgoodConfig";
        public const string VOLTAGE_FILTER_TYPE = "VoltageFilterType";
        public const string LOW_VOLTAGE_MODE = "SEVoltageRange";
        public const string WARN_SOURCES_OC = "WarnSources_OC";
        public const string WARN_SOURCES_OV = "WarnSources_OV";
        public const string WARN_SOURCES_UV = "WarnSources_UV";
        public const string VOLTAGES_TABLE = "VoltagesTable";
        public const string CURRENTS_TABLE = "CurrentsTable";
        public const string AUXILIARY_TABLE = "AuxiliaryTable";
        public const string DIFF_CURRENT_RANGE = "DiffCurrentRange";
        public const string EXPOSE_CAL_PIN = "ExposeCalPin";

        public const string SYMBOL_B_VISIBLE = "SymbolBVisible";
        public static readonly string[] V_SINGLE_ENDED = new string[CyParamRanges.NUM_CONVERTERS_MAX] 
        { 
            "V1SingleEnded", "V2SingleEnded", "V3SingleEnded", "V4SingleEnded",
            "V5SingleEnded", "V6SingleEnded", "V7SingleEnded", "V8SingleEnded",
            "V9SingleEnded", "V10SingleEnded", "V11SingleEnded", "V12SingleEnded",
            "V13SingleEnded", "V14SingleEnded", "V15SingleEnded", "V16SingleEnded",
            "V17SingleEnded", "V18SingleEnded", "V19SingleEnded", "V20SingleEnded",
            "V21SingleEnded", "V22SingleEnded", "V23SingleEnded", "V24SingleEnded",
            "V25SingleEnded", "V26SingleEnded", "V27SingleEnded", "V28SingleEnded",
            "V29SingleEnded", "V30SingleEnded", "V31SingleEnded", "V32SingleEnded"
        };

        public static readonly string[] I_CSA = new string[CyParamRanges.NUM_CONVERTERS_MAX] 
        { 
            "I1CSA", "I2CSA", "I3CSA", "I4CSA",
            "I5CSA", "I6CSA", "I7CSA", "I8CSA",
            "I9CSA", "I10CSA", "I11CSA", "I12CSA",
            "I13CSA", "I14CSA", "I15CSA", "I16CSA",
            "I17CSA", "I18CSA", "I19CSA", "I20CSA",
            "I21CSA", "I22CSA", "I23CSA", "I24CSA",
            "I25CSA", "I26CSA", "I27CSA", "I28CSA",
            "I29CSA", "I30CSA", "I31CSA", "I32CSA"
        };

        public static readonly string[] AUX_SINGLE_ENDED = new string[CyParamRanges.NUM_AUX_CHANNELS_MAX]
        { 
            "Aux1SingleEnded", "Aux2SingleEnded", "Aux3SingleEnded", "Aux4SingleEnded"
        };
    }
    #endregion

    #region Parameters Ranges
    public class CyParamRanges
    {
        public const byte MAX_PINS_IN_SYSTEM = 48;

        // Basic tab parameters
        public const byte NUM_CONVERTERS_MIN = 1;
        public const byte NUM_CONVERTERS_MAX = 32;
        public const byte NUM_AUX_CHANNELS_MIN = 0;
        public const byte NUM_AUX_CHANNELS_MAX = 4;

        // Voltages tab parameters
        public const double NOMINAL_OUTPUT_VOLTAGE_MIN = 0.001;
        public const double NOMINAL_OUTPUT_VOLTAGE_MAX = 65.535;
        public const double UV_FAULT_TRESHOLD_MIN = 0.001;
        public const double UV_FAULT_TRESHOLD_MAX = 65.535;
        public const double UV_WARNING_TRESHOLD_MIN = 0.001;
        public const double UV_WARNING_TRESHOLD_MAX = 65.535;
        public const double OV_FAULT_TRESHOLD_MIN = 0.001;
        public const double OV_FAULT_TRESHOLD_MAX = 65.535;
        public const double OV_WARNING_TRESHOLD_MIN = 0.001;
        public const double OV_WARNING_TRESHOLD_MAX = 65.535;
        public const double INPUT_SCALING_FACTOR_MIN = 0.001;
        public const double INPUT_SCALING_FACTOR_MAX = 1.000;
        public const double OV_FAULT_MULT_SCALE_MAX = 4;
        public const double UV_FAULT_MULT_SCALE_MIN = 0.05;

        // Currents tab parameters
        public const double OC_FAULT_TRESHOLD_MIN = 0.01;
        public const double OC_FAULT_TRESHOLD_MAX = 655.35;
        public const double OC_WARNING_TRESHOLD_MIN = 0.01;
        public const double OC_WARNING_TRESHOLD_MAX = 655.35;
        public const double SHUNT_RESISTOR_MIN = 0.01;
        public const double SHUNT_RESISTOR_MAX = 2500;
        public const double CSA_GAIN_MIN = 1;
        public const double CSA_GAIN_MAX = 500;
        public const double ADC_RANGE_64 = 64;
        public const double ADC_RANGE_128 = 128;
        public static double AdcRange;

        public const double ADC_RANGE_CSA_CHECK = 4096;
    }
    #endregion

    #region Component Enums
    public enum CyEFilterType { None, Average_4, Average_8, Average_16, Average_32 }
    public enum CyEPgoodType
    {
        [Description("Global")]
        global,
        [Description("Individual")]
        individual
    }
    public enum CyEVType { SingleEnded, Differential }
    public enum CyEVInternalType
    {
        [Description("Single Ended")]
        SingleEnded,
        [Description("Differential")]
        Differential
    }
    public enum CyECurrentMeasurementInternalType
    {
        [Description("None")]
        None,
        [Description("Direct")]
        Direct,
        [Description("CSA")]
        CSA
    }
    public enum CyEAdcRangeInternalType
    {
        [Description("Single Ended: 0-4.096 V")]
        SignleEnded_4V,
        [Description("Single Ended: 0-2.048 V")]
        SignleEnded_2V,
        [Description("Differential: +/- 64 mV")]
        Differential_64mV,
        [Description("Differential: +/- 2.048 V")]
        Differential_2048mV,
        [Description("Differential: +/- 128 mV")]
        Differential_128mV
    }
    public enum CyEDiffCurrentRangeType
    {
        [Description("+/- 64 mV Range")]
        Range_64mV,
        [Description("+/- 128 mV Range")]
        Range_128mV
    }

    public enum CyELowVoltageMode
    {
        [Description("0-2.048 V Range")]
        Range_2048mV,
        [Description("0-4.096 V Range")]
        Range_4096mV
    }
    #endregion

    public class CyParameters
    {
        public static DataGridViewCellStyle DGDisabledStyle;
        public static DataGridViewCellStyle DGEnabledStyle;

        // List contains display names of type taken from symbol. It is used to fill combobox.
        public List<string> m_filterTypeList;
        public List<string> m_diffCurrentRangeList;

        // Dictionary is used to keep display names of types taken from symbol,
        // and associate them with the enum fields to simplify access to diaplay names
        public Dictionary<object, string> m_displayNameDict = new Dictionary<object, string>();

        public ICyInstQuery_v1 m_inst;
        public CyGeneralTab m_generalTab;
        public CyVoltagesTab m_voltagesTab;
        public CyCurrentsTab m_currentsTab;
        public CyAuxiliaryTab m_auxTab;

        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        private static bool m_bGlobalEditMode = false;

        private List<CyVoltagesTableRow> m_voltagesTable;
        private List<CyCurrentsTableRow> m_currentsTable;
        private List<CyAuxTableRow> m_auxTable;
        private bool m_isVoltagesTableDefault = false;
        private bool m_isCurrentsTableDefault = false;
        private bool m_isAuxTableDefault = false;
        private byte m_numOfDefaultRows = 0;

        XmlSerializerNamespaces m_customNamespace;
        XmlSerializer m_voltagesSerializer = new XmlSerializer(typeof(CyVoltagesTable));
        XmlSerializer m_currentsSerializer = new XmlSerializer(typeof(CyCurrentsTable));
        XmlSerializer m_auxSerializer = new XmlSerializer(typeof(CyAuxTable));


        #region Constructor(s)
        public CyParameters(ICyInstQuery_v1 inst)
        {
            m_inst = inst;

            //Custom namespaces for serialization
            m_customNamespace = new XmlSerializerNamespaces();
            m_customNamespace.Add("version", "v1.0");

            m_filterTypeList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.VOLTAGE_FILTER_TYPE));
            m_diffCurrentRangeList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.DIFF_CURRENT_RANGE));
            CyDictParser.FillDictionary(ref m_displayNameDict, typeof(CyEFilterType), m_filterTypeList);
            CyDictParser.FillDictionary(ref m_displayNameDict, typeof(CyEDiffCurrentRangeType), m_diffCurrentRangeList);
            GetVoltagesTable();
            GetCurrentsTable();
            GetAuxTable();

        }

        #endregion

        #region Class properties
        public static bool GlobalEditMode
        {
            get { return CyParameters.m_bGlobalEditMode; }
            set { CyParameters.m_bGlobalEditMode = value; }
        }

        public byte NumConverters
        {
            get
            {
                return GetValue<byte>(CyParamNames.NUM_CONVERTERS);
            }
            set
            {
                SetValue(CyParamNames.NUM_CONVERTERS, value);
            }
        }

        public byte NumAuxChannels
        {
            get
            {
                return GetValue<byte>(CyParamNames.NUM_AUX_CHANNELS);
            }
            set
            {
                SetValue(CyParamNames.NUM_AUX_CHANNELS, value);
            }
        }

        public CyEFilterType VoltageFilterType
        {
            get
            {
                return GetValue<CyEFilterType>(CyParamNames.VOLTAGE_FILTER_TYPE);
            }
            set
            {
                SetValue(CyParamNames.VOLTAGE_FILTER_TYPE, value);
            }
        }

        public CyEFilterType CurrentFilterType
        {
            get
            {
                return GetValue<CyEFilterType>(CyParamNames.CURRENT_FILTER_TYPE);
            }
            set
            {
                SetValue(CyParamNames.CURRENT_FILTER_TYPE, value);
            }
        }

        public CyEFilterType AuxFilterType
        {
            get
            {
                return GetValue<CyEFilterType>(CyParamNames.AUX_FILTER_TYPE);
            }
            set
            {
                SetValue(CyParamNames.AUX_FILTER_TYPE, value);
            }
        }

        public CyEDiffCurrentRangeType DiffCurrentRange
        {
            get
            {
                CyEDiffCurrentRangeType result = GetValue<CyEDiffCurrentRangeType>(CyParamNames.DIFF_CURRENT_RANGE);
                SetAdcRange(result);
                return result;
            }
            set
            {
                SetAdcRange(value);
                SetValue(CyParamNames.DIFF_CURRENT_RANGE, value);
            }
        }

        public CyELowVoltageMode LowVoltageMode
        {
            get
            {
                return GetValue<CyELowVoltageMode>(CyParamNames.LOW_VOLTAGE_MODE);
            }
            set
            {
                SetValue(CyParamNames.LOW_VOLTAGE_MODE, value);
            }
        }

        public bool ExposeCalibration
        {
            get
            {
                return GetValue<bool>(CyParamNames.EXPOSE_CAL_PIN);
            }
            set
            {
                SetValue(CyParamNames.EXPOSE_CAL_PIN, value);
            }
        }

        public CyEPgoodType PgoodConfig
        {
            get
            {
                return GetValue<CyEPgoodType>(CyParamNames.PGOOD_CONFIG);
            }
            set
            {
                SetValue(CyParamNames.PGOOD_CONFIG, value);
            }
        }

        public bool FaultSourcesOV
        {
            get
            {
                return GetValue<bool>(CyParamNames.FAULT_SOURCES_OV);
            }
            set
            {
                SetValue(CyParamNames.FAULT_SOURCES_OV, value);
            }
        }

        public bool FaultSourcesUV
        {
            get
            {
                return GetValue<bool>(CyParamNames.FAULT_SOURCES_UV);
            }
            set
            {
                SetValue(CyParamNames.FAULT_SOURCES_UV, value);
            }
        }

        public bool FaultSourcesOC
        {
            get
            {
                return GetValue<bool>(CyParamNames.FAULT_SOURCES_OC);
            }
            set
            {
                SetValue(CyParamNames.FAULT_SOURCES_OC, value);
            }
        }

        public bool WarningSourcesOV
        {
            get
            {
                return GetValue<bool>(CyParamNames.WARN_SOURCES_OV);
            }
            set
            {
                SetValue(CyParamNames.WARN_SOURCES_OV, value);
            }
        }

        public bool WarningSourcesUV
        {
            get
            {
                return GetValue<bool>(CyParamNames.WARN_SOURCES_UV);
            }
            set
            {
                SetValue(CyParamNames.WARN_SOURCES_UV, value);
            }
        }

        public bool WarningSourcesOC
        {
            get
            {
                return GetValue<bool>(CyParamNames.WARN_SOURCES_OC);
            }
            set
            {
                SetValue(CyParamNames.WARN_SOURCES_OC, value);
            }
        }

        public List<CyVoltagesTableRow> VoltagesTable
        {
            get { return m_voltagesTable; }
            set { m_voltagesTable = value; }
        }

        public List<CyCurrentsTableRow> CurrentsTable
        {
            get { return m_currentsTable; }
            set { m_currentsTable = value; }
        }

        public List<CyAuxTableRow> AuxTable
        {
            get { return m_auxTable; }
            set { m_auxTable = value; }
        }

        public bool IsVoltagesTableDefault
        {
            get { return m_isVoltagesTableDefault; }
            set { m_isVoltagesTableDefault = value; }
        }

        public bool IsCurrentsTableDefault
        {
            get { return m_isCurrentsTableDefault; }
            set { m_isCurrentsTableDefault = value; }
        }

        public bool IsAuxTableDefault
        {
            get { return m_isAuxTableDefault; }
            set { m_isAuxTableDefault = value; }
        }

        public bool SymbolBVisible
        {
            get
            {
                return GetValue<bool>(CyParamNames.SYMBOL_B_VISIBLE);
            }
            set
            {
                SetValue(CyParamNames.SYMBOL_B_VISIBLE, value);
            }
        }

        private bool GetSymbolBVisibility()
        {
            bool symbolBVisible = false;

            for (int i = 0; i < m_voltagesTable.Count; i++)
            {
                if (m_voltagesTable[i].m_voltageMeasurementType != CyEVInternalType.SingleEnded)
                {
                    symbolBVisible = true;
                    break;
                }
            }

            for (int i = 0; i < m_currentsTable.Count; i++)
            {
                if (m_currentsTable[i].m_currentMeasurementType != CyECurrentMeasurementInternalType.None)
                {
                    symbolBVisible = true;
                    break;
                }
            }
            return symbolBVisible;
        }

        private static void SetAdcRange(CyEDiffCurrentRangeType value)
        {
            switch (value)
            {
                case CyEDiffCurrentRangeType.Range_64mV:
                    CyParamRanges.AdcRange = CyParamRanges.ADC_RANGE_64;
                    break;
                case CyEDiffCurrentRangeType.Range_128mV:
                    CyParamRanges.AdcRange = CyParamRanges.ADC_RANGE_128;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Getting Parameters
        private T GetValue<T>(string paramName)
        {
            T value;
            CyCustErr err = m_inst.GetCommittedParam(paramName).TryGetValueAs<T>(out value);
            if (err != null && err.IsOK)
            {
                return value;
            }
            else
            {
                return default(T);
            }
        }
        #endregion

        #region Getting/Setting parameters with XML Data
        public void GetVoltagesTable()
        {
            string xmlData = GetValue<string>(CyParamNames.VOLTAGES_TABLE);
            m_numOfDefaultRows = this.NumConverters;
            CyVoltagesTable package = (CyVoltagesTable)Deserialize(xmlData, typeof(CyVoltagesTable));
            m_voltagesTable = package.m_voltagesTable;
        }

        public void SetVoltagesTable() { SetVoltagesTable(true); }
        public void SetVoltagesTable(bool termVisibilityUpdate)
        {
            if (m_bGlobalEditMode)
            {
                CyVoltagesTable package = new CyVoltagesTable();
                package.m_voltagesTable = new List<CyVoltagesTableRow>(m_voltagesTable.ToArray());
                package.m_voltagesTable.RemoveRange(NumConverters, package.m_voltagesTable.Count - NumConverters);

                SetValue(CyParamNames.VOLTAGES_TABLE, Serialize(package));
                this.SymbolBVisible = GetSymbolBVisibility();
                if (termVisibilityUpdate)
                    SetVoltagesTerminalsVisibility(package.m_voltagesTable.Count);
            }
        }

        public void GetCurrentsTable()
        {
            string xmlData = GetValue<string>(CyParamNames.CURRENTS_TABLE);
            m_numOfDefaultRows = this.NumConverters;
            CyCurrentsTable package = (CyCurrentsTable)Deserialize(xmlData, typeof(CyCurrentsTable));
            m_currentsTable = package.m_currentsTable;
        }

        public void SetCurrentsTable() { SetCurrentsTable(true); }
        public void SetCurrentsTable(bool termVisibilityUpdate)
        {
            if (m_bGlobalEditMode)
            {
                CyCurrentsTable package = new CyCurrentsTable();
                package.m_currentsTable = new List<CyCurrentsTableRow>(m_currentsTable.ToArray());
                package.m_currentsTable.RemoveRange(NumConverters, package.m_currentsTable.Count - NumConverters);
                SetValue(CyParamNames.CURRENTS_TABLE, Serialize(package));
                this.SymbolBVisible = GetSymbolBVisibility();
                if (termVisibilityUpdate)
                {
                    SetVoltagesTerminalsVisibility(package.m_currentsTable.Count);
                    SetCurrentTerminalsVisibility(package.m_currentsTable.Count);
                }
            }
        }

        public void GetAuxTable()
        {
            string xmlData = GetValue<string>(CyParamNames.AUXILIARY_TABLE);
            m_numOfDefaultRows = this.NumAuxChannels;
            CyAuxTable package = (CyAuxTable)Deserialize(xmlData, typeof(CyAuxTable));
            m_auxTable = package.m_auxTable;
        }

        public void SetAuxTable()
        {
            if (m_bGlobalEditMode)
            {
                CyAuxTable package = new CyAuxTable();
                package.m_auxTable = new List<CyAuxTableRow>(m_auxTable.ToArray());
                package.m_auxTable.RemoveRange(NumAuxChannels, package.m_auxTable.Count - NumAuxChannels);
                SetValue(CyParamNames.AUXILIARY_TABLE, Serialize(package));
                SetAuxTerminalsVisibility(package.m_auxTable.Count);
            }
        }
        #endregion

        #region Import or paste rows
        public bool Import(Control parent, string fileContent, bool pasteMode)
        {
            if (parent is CyVoltagesTab)
            {
                List<CyVoltagesTableRow> importedTable = CyImportExport.ImportVoltagesTable(NumConverters,
                    CyVoltagesTableRow.COL_COUNT, fileContent, pasteMode);

                if (importedTable != null)
                {
                    int firstPasteRow = -1;
                    int lastPasteRow = -1;
                    if (pasteMode)
                    {
                        firstPasteRow = m_voltagesTab.DataGridFirstSelectedRow;
                        lastPasteRow = GetLastPasteRow(importedTable.Count, firstPasteRow, NumConverters);

                        int j = 0;
                        for (int i = firstPasteRow; i < lastPasteRow; i++)
                        {                            
                            VoltagesTable[i] = importedTable[j];                         
                            j++;
                        }
                    }
                    else
                    {
                        VoltagesTable = importedTable;
                    }
                    m_generalTab.SyncTables();
                    m_voltagesTab.UpdateUIFromTable();
                    m_currentsTab.UpdateUIFromTable();
                    SetVoltagesTable(false);
                    SetCurrentsTable();
                    m_voltagesTab.ValidateAllCells();
                    m_currentsTab.ValidateAllCells();
                    m_voltagesTab.SelectDataGridViewRows(firstPasteRow, lastPasteRow - 1);
                }
                else return false;
            }
            else if (parent is CyCurrentsTab)
            {
                List<CyCurrentsTableRow> importedTable = CyImportExport.ImportCurrentsTable(NumConverters,
                    CyCurrentsTableRow.COL_COUNT, fileContent, pasteMode);

                if (importedTable != null)
                {
                    int firstPasteRow = -1;
                    int lastPasteRow = -1;
                    if (pasteMode)
                    {
                        firstPasteRow = m_currentsTab.DataGridFirstSelectedRow;
                        lastPasteRow = GetLastPasteRow(importedTable.Count, firstPasteRow, NumConverters);

                        int j = 0;
                        for (int i = firstPasteRow; i < lastPasteRow; i++)
                        {
                            CurrentsTable[i] = importedTable[j];
                            j++;
                        }
                    }
                    else
                    {
                        CurrentsTable = importedTable;
                    }

                    m_currentsTab.UpdateUIFromTable();
                    SetCurrentsTable();
                    m_currentsTab.ValidateAllCells();
                    m_currentsTab.SelectDataGridViewRows(firstPasteRow, lastPasteRow - 1);
                }
            }
            else if (parent is CyAuxiliaryTab)
            {
                List<CyAuxTableRow> importedTable = CyImportExport.ImportAuxTable(NumAuxChannels,
                    CyAuxTableRow.COL_COUNT, fileContent, pasteMode);

                if (importedTable != null)
                {
                    int firstPasteRow = -1;
                    int lastPasteRow = -1;
                    if (pasteMode)
                    {
                        firstPasteRow = m_auxTab.DataGridFirstSelectedRow;
                        lastPasteRow = GetLastPasteRow(importedTable.Count, m_auxTab.DataGridFirstSelectedRow,
                            NumAuxChannels);

                        int j = 0;
                        for (int i = m_auxTab.DataGridFirstSelectedRow; i < lastPasteRow; i++)
                        {
                            AuxTable[i] = importedTable[j];
                            m_generalTab.SyncAuxTableItem(i);
                            j++;
                        }
                    }
                    else
                    {
                        AuxTable = importedTable;
                        m_generalTab.SyncAuxTable();
                    }
                    m_auxTab.UpdateUIFromTable();
                    SetAuxTable();
                    m_auxTab.SelectDataGridViewRows(firstPasteRow, lastPasteRow - 1);
                }
            }
            return true;
        }

        private int GetLastPasteRow(int importRowCount, int datagridActiveRowIndex, int gridRowsCount)
        {
            int lastPasteRow;
            if (importRowCount > (gridRowsCount - datagridActiveRowIndex))
            {
                lastPasteRow = datagridActiveRowIndex + (gridRowsCount - datagridActiveRowIndex);
            }
            else
            {
                lastPasteRow = datagridActiveRowIndex + importRowCount;
            }
            return lastPasteRow;
        }
        #endregion

        #region Setting Parameters
        private void SetValue<T>(string paramName, T value)
        {
            if (m_bGlobalEditMode)
            {
                if ((m_inst is ICyInstEdit_v1) == false) return;

                string valueToSet = value.ToString();
                if (value is bool)
                    valueToSet = valueToSet.ToLower();
                if ((m_inst as ICyInstEdit_v1).SetParamExpr(paramName, valueToSet.ToString()))
                {
                    (m_inst as ICyInstEdit_v1).CommitParamExprs();
                }
            }
        }

        private void SetVoltagesTerminalsVisibility(int length)
        {
            // Set available voltages/currents terminals
            for (int i = 0; i < length; i++)
            {
                if (m_voltagesTable[i].m_voltageMeasurementType == CyEVInternalType.SingleEnded &&
                    m_currentsTable[i].m_currentMeasurementType == CyECurrentMeasurementInternalType.None)
                {
                    SetValue(CyParamNames.V_SINGLE_ENDED[i], true);
                }
                else
                {
                    SetValue(CyParamNames.V_SINGLE_ENDED[i], false);
                }
            }
            // Set the rest voltages/currents terminals
            for (int i = length; i < CyParamRanges.NUM_CONVERTERS_MAX; i++)
            {
                SetValue(CyParamNames.V_SINGLE_ENDED[i], false);
            }
        }

        private void SetCurrentTerminalsVisibility(int length)
        {
            // Set available voltages/currents terminals
            for (int i = 0; i < length; i++)
            {
                if (m_currentsTable[i].m_currentMeasurementType == CyECurrentMeasurementInternalType.CSA)
                {
                    SetValue(CyParamNames.I_CSA[i], true);
                }
                else
                {
                    SetValue(CyParamNames.I_CSA[i], false);
                }
            }
            //            // Set the rest voltages/currents terminals
            for (int i = length; i < CyParamRanges.NUM_CONVERTERS_MAX; i++)
            {
                SetValue(CyParamNames.I_CSA[i], false);
            }
        }

        private void SetAuxTerminalsVisibility(int length)
        {
            // Set available auxiliary terminals
            for (int i = 0; i < length; i++)
            {
                if ((m_auxTable[i].m_adcRange == CyEAdcRangeInternalType.SignleEnded_4V) ||
                (m_auxTable[i].m_adcRange == CyEAdcRangeInternalType.SignleEnded_2V))
                {
                    SetValue(CyParamNames.AUX_SINGLE_ENDED[i], true);
                }
                else
                {
                    SetValue(CyParamNames.AUX_SINGLE_ENDED[i], false);
                }
            }
            // Set the rest auxiliary terminals
            for (int i = length; i < CyParamRanges.NUM_AUX_CHANNELS_MAX; i++)
            {
                SetValue(CyParamNames.AUX_SINGLE_ENDED[i], false);
            }
        }
        #endregion

        #region XML Serialization
        public string Serialize(object obj)
        {
            StringBuilder sb = new StringBuilder();

            System.Xml.XmlWriter tw = System.Xml.XmlWriter.Create(sb);
            GetSerializer(obj.GetType()).Serialize(tw, obj, m_customNamespace);
            tw.Close();

            string res = sb.ToString();
            res = res.Replace("\r\n", " ");


            return res;
        }

        public object Deserialize(string serializedXml, Type t)
        {
            object res = null;
            try
            {
                if (String.IsNullOrEmpty(serializedXml)) throw new Exception();

                //Read version information
                XmlReader tr = XmlReader.Create(new StringReader(serializedXml));
                //Remove header <?xml version="1.0" encoding="utf-16" ?> 
                tr.Read();
                tr.Read();
                //Go to first Node with attributes
                while (tr.HasAttributes == false)
                {
                    tr.Read();
                }
                //This version information will be used in future version of Power Monitor component.
                string ver_info = tr.GetAttribute(0);

                res = GetSerializer(t).Deserialize(tr);
                SetTableDefault(t, false);
            }
            catch
            {
                res = Activator.CreateInstance(t);

                ((ICyTable)res).InitializeTable(m_numOfDefaultRows);
                SetTableDefault(t, true);

                if (String.IsNullOrEmpty(serializedXml) == false)
                {
                    MessageBox.Show(Resources.SettingsIncorrectValues,
                        MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return res;
        }

        XmlSerializer GetSerializer(Type t)
        {
            if (t == typeof(CyVoltagesTable))
            {
                return m_voltagesSerializer;
            }
            if (t == typeof(CyCurrentsTable))
            {
                return m_currentsSerializer;
            }
            if (t == typeof(CyAuxTable))
            {
                return m_auxSerializer;
            }

            return null;
        }

        void SetTableDefault(Type t, bool isDefault)
        {
            if (t == typeof(CyVoltagesTable))
                m_isVoltagesTableDefault = isDefault;
            if (t == typeof(CyCurrentsTable))
                m_isCurrentsTableDefault = isDefault;
            if (t == typeof(CyAuxTable))
                m_isAuxTableDefault = isDefault;
        }

        #endregion

        public static bool IsCellValueValid(DataGridView dsg, int rowIndex, int colIndex, double min, 
            double max, string errorMessage)
        {
            bool isCellValid = false;
            string message = string.Format(errorMessage, min, max);
            try
            {
                object cell = dsg[colIndex, rowIndex].Value;
                if ((cell != null) && (dsg[colIndex, rowIndex].ReadOnly == false) && (cell.ToString() != string.Empty))
                {
                    double currCellValue = double.Parse(dsg[colIndex, rowIndex].Value.ToString());

                    if (currCellValue < min || currCellValue > max) throw new Exception();
                }

                dsg[colIndex, rowIndex].ErrorText = string.Empty;
                isCellValid = true;
            }
            catch (Exception)
            {
                dsg[colIndex, rowIndex].ErrorText = message;
            }
            return isCellValid;
        }
        public static void SetCellReadOnlyState(DataGridView dgv, int row, int col, bool readOnly)
        {
            dgv[col, row].Style = readOnly ? DGDisabledStyle : DGEnabledStyle;
            dgv[col, row].ReadOnly = readOnly;
        }
        public static void SetColumnReadOnlyState(DataGridView dgv, int col, bool readOnly)
        {
            dgv.Columns[col].DefaultCellStyle = readOnly ? DGDisabledStyle : DGEnabledStyle;
            dgv.Columns[col].ReadOnly = readOnly;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[col].Style = readOnly ? DGDisabledStyle : DGEnabledStyle;
            }
        }

        public static bool CurrentTableRowHasEmptyCells(CyParameters prms, CyCurrentsTableRow row)
        {
            return row.m_currentMeasurementType != CyECurrentMeasurementInternalType.None && (
                    (row.m_ocFaulthTreshold == null && prms.FaultSourcesOC)|| 
                    (row.m_ocWarningThreshold == null && prms.WarningSourcesOC )|| 
                    row.m_shuntResistorValue == null || 
                    (row.m_csaGain == null && row.m_currentMeasurementType == CyECurrentMeasurementInternalType.CSA)
                    );
        }
        public static bool VoltageTableRowHasEmptyCells(CyParameters prms, CyVoltagesTableRow row)
        {
            return row.m_inputScalingFactor == null ||
                row.m_nominalOutputVoltage == null ||
                (row.m_ovFaultTreshold == null && prms.FaultSourcesOV) ||
                (row.m_ovWarningTreshold == null && prms.WarningSourcesOV) ||
                (row.m_uvFaultTreshold == null && prms.FaultSourcesUV) ||
                (row.m_uvWarningTreshold == null && prms.WarningSourcesUV);
        }
        public static double? ParseNullableDouble(object val)
        {
            return ParseNullableDouble(CellToString(val));
        }
        private static char charsTrim = '\"';
        public static double? ParseNullableDouble(string val)
        {
            val = val.Replace(charsTrim, ' ');
            val = val.Trim();
            double? res = null;
            double parsed = 0;
            if (double.TryParse(val, out parsed))
            {
                res = parsed;
            }
            return res;
        }
        public static double NullableDoubleToDouble(double? val)
        {
            double res = 0;
            if (val.HasValue)
                res = val.Value;
            return res;
        }
        public static string NullableDoubleToString(double? val)
        {
            string res = string.Empty;
            if (val.HasValue)
                res = val.Value.ToString();
            return res;
        }
        public static string CellToString(object cellValue)
        {
            string res = "";
            if (cellValue != null)
                res = cellValue.ToString();
            return res;
        }
    }
}
