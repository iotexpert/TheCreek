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

namespace VoltageFaultDetector_v1_0
{
    #region Component Parameters Names
    public class CyParamNames
    {
        public const string NUM_VOLTAGES = "NumVoltages";
        public const string COMPARE_TYPE = "CompareType";
        public const string GF_LENGTH = "GfLength";
        public const string EXTERNAL_REF = "ExternalRef";
        public const string DAC_RANGE = "DacRange";
        public const string PHYSICAL_PLACEMENT = "PhysicalPlacement";
        public const string ANALOG_BUS = "AnalogBus";
        public const string VOLTAGES_TABLE = "VoltagesTable";
    }
    #endregion

    #region Parameters Ranges
    public class CyParamRanges
    {
        // Basic tab parameters
        public const byte NUM_VOLTAGES_MIN = 1;
        public const byte NUM_VOLTAGES_MAX = 32;
        public const byte GFLENGTH_MIN = 1;
        public const byte GFLENGTH_MAX = 255;

        // Voltages tab parameters
        public const double NOMINAL_VOLTAGE_MIN = 0.01;
        public const double NOMINAL_VOLTAGE_MAX = 65.54;
        public const double UV_FAULT_THRESHOLD_MIN = 0.01;
        public const double UV_FAULT_THRESHOLD_MAX = 65.54;
        public const double OV_FAULT_THRESHOLD_MIN = 0.01;
        public const double OV_FAULT_THRESHOLD_MAX = 65.54;
        public const double INPUT_SCALING_FACTOR_MIN = 0.001;
        public const double INPUT_SCALING_FACTOR_MAX = 1.000;
        public const double OV_FAULT_MULT_SCALE_MAX = 4;
        public const double UV_FAULT_MULT_SCALE_MIN = 0.05;
        public const byte NUM_CYCLE = 16;
    }
    #endregion

    #region Component Enums
    public enum CyCompareType
    {
        [Description("OV/UV")]
        OV_UV,
        [Description("OV only")]
        OV,
        [Description("UV only")]
        UV
    }
    public enum CyDACRangeType
    {
        [Description("1V")]
        V1,
        [Description("4V")]
        V4
    }
    public enum CyPhysicalPlacementType
    {
        [Description("Comp0")]
        Comp0,
        [Description("Comp1")]
        Comp1,
        [Description("Comp2")]
        Comp2,
        [Description("Comp3")]
        Comp3,
        [Description("Comp0+2")]
        Comp02,
        [Description("Comp1+3")]
        Comp13
    }
    public enum CyAnalogBusType
    {
        [Description("AMUXBUSR")]
        AMUXBUSR,
        [Description("AMUXBUSL")]
        AMUXBUSL,
        [Description("Unconstrained")]
        Unconstrained
    }
    #endregion

    public class CyParameters
    {
        // List contains display names of type taken from symbol. It is used to fill combobox.
        public List<string> m_filterTypeList;

        // Dictionary is used to keep display names of types taken from symbol,
        // and associate them with the enum fields to simplify access to diaplay names
        public Dictionary<object, string> m_displayNameDict = new Dictionary<object, string>();

        public ICyInstQuery_v1 m_inst;
        public ICyTerminalQuery_v1 m_termQuery;
        public CyBasicTab m_basicTab;
        public CyVoltagesTab m_voltagesTab;

        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        private bool m_bGlobalEditMode = false;

        private List<CyVoltagesTableRow> m_voltagesTable;
        private bool m_isVoltagesTableDefault = false;
        private byte m_numOfDefaultRows = 0;

        // Xml serialization parameters
        public XmlSerializer m_serializer;
        public XmlSerializerNamespaces m_customSerNamespace;

        #region Constructor(s)
        public CyParameters(ICyInstQuery_v1 inst)
        {
            m_inst = inst;

            // Create XML Serializer
            m_serializer = new XmlSerializer(typeof(CyVoltagesTable));
            m_customSerNamespace = new XmlSerializerNamespaces();
            Type classType = typeof(CyParameters);
            string curNamespace = classType.Namespace;
            string version = curNamespace.Substring(curNamespace.LastIndexOf("_v") + 2);
            m_customSerNamespace.Add("Version", version);

            GetVoltagesTable();
        }

        public CyParameters(ICyInstQuery_v1 inst, ICyTerminalQuery_v1 termQuery)
            : this(inst)
        {
            m_termQuery = termQuery;
        }

        public CyParameters() { }

        #endregion

        #region Class properties
        public bool GlobalEditMode
        {
            get { return m_bGlobalEditMode; }
            set { m_bGlobalEditMode = value; }
        }

        public byte NumVoltages
        {
            get { return GetValue<byte>(CyParamNames.NUM_VOLTAGES); }
            set { SetValue(CyParamNames.NUM_VOLTAGES, value); }
        }

        public CyCompareType CompareType
        {
            get { return GetValue<CyCompareType>(CyParamNames.COMPARE_TYPE); }
            set { SetValue(CyParamNames.COMPARE_TYPE, value); }
        }

        public byte GFLength
        {
            get { return GetValue<byte>(CyParamNames.GF_LENGTH); }
            set { SetValue(CyParamNames.GF_LENGTH, value); }
        }

        public bool ExternalRef
        {
            get { return GetValue<bool>(CyParamNames.EXTERNAL_REF); }
            set { SetValue(CyParamNames.EXTERNAL_REF, value); }
        }

        public CyDACRangeType DACRange
        {
            get { return GetValue<CyDACRangeType>(CyParamNames.DAC_RANGE); }
            set { SetValue(CyParamNames.DAC_RANGE, value); }
        }

        public CyPhysicalPlacementType PhysicalPlacement
        {
            get { return GetValue<CyPhysicalPlacementType>(CyParamNames.PHYSICAL_PLACEMENT); }
            set { SetValue(CyParamNames.PHYSICAL_PLACEMENT, value); }
        }

        public CyAnalogBusType AnalogBus
        {
            get { return GetValue<CyAnalogBusType>(CyParamNames.ANALOG_BUS); }
            set { SetValue(CyParamNames.ANALOG_BUS, value); }
        }

        public List<CyVoltagesTableRow> VoltagesTable
        {
            get { return m_voltagesTable; }
            set { m_voltagesTable = value; }
        }

        public bool IsVoltagesTableDefault
        {
            get { return m_isVoltagesTableDefault; }
            set { m_isVoltagesTableDefault = value; }
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

        public void GetVoltagesTable()
        {
            string xmlData = GetValue<string>(CyParamNames.VOLTAGES_TABLE);
            m_numOfDefaultRows = this.NumVoltages;
            CyVoltagesTable package = (CyVoltagesTable)Deserialize(xmlData, typeof(CyVoltagesTable));
            m_voltagesTable = package.m_voltagesTable;
        }
        #endregion

        #region Import or paste rows
        public bool Import(Control parent, string fileContent, bool pasteMode)
        {
            List<CyVoltagesTableRow> importedTable = CyImportExport.ImportVoltagesTable(NumVoltages,
                CyVoltagesTableRow.COL_COUNT, fileContent, pasteMode);

            if (importedTable != null)
            {
                if (pasteMode)
                {
                    int lastPasteRow = GetLastPasteRow(importedTable.Count, m_voltagesTab.DataGridActiveRowIndex,
                        NumVoltages);

                    int j = 0;
                    for (int i = m_voltagesTab.DataGridActiveRowIndex; i < lastPasteRow; i++)
                    {
                        VoltagesTable[i] = importedTable[j];
                        j++;
                    }
                    m_voltagesTab.UpdateUIFromTable();
                    m_voltagesTab.SelectRow(lastPasteRow-1);
                }
                else
                {
                    VoltagesTable = new List<CyVoltagesTableRow>(importedTable);
                    m_voltagesTab.UpdateUIFromTable();
                }
                SetVoltagesTable();
            }
            else return false;

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

        #region Nullable double operations
        public static double? ParseNullableDouble(string val)
        {
            double? res = null;
            double parsed = 0;
            if (double.TryParse(val, out parsed))
            {
                res = parsed;
            }
            return res;
        }
        public static string NullableDoubleToString(double? val, string format)
        {
            string res = "";
            if (val.HasValue)
                res = val.Value.ToString(format);
            return res;
        }
        public static string CellToString(object cellValue)
        {
            string res = "";
            if (cellValue != null)
                res = cellValue.ToString();
            return res;
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

        public void SetVoltagesTable()
        {
            string xmlData = GetValue<string>(CyParamNames.VOLTAGES_TABLE);
            CyVoltagesTable package = new CyVoltagesTable();
            package.m_voltagesTable = new List<CyVoltagesTableRow>(m_voltagesTable);
            package.m_voltagesTable.RemoveRange(NumVoltages, package.m_voltagesTable.Count - NumVoltages);
            SetValue(CyParamNames.VOLTAGES_TABLE, Serialize(package));
        }
        #endregion

        #region XML Serialization
        public string Serialize(object obj)
        {
            StringBuilder sb = new StringBuilder();

            System.Xml.XmlWriter tw = System.Xml.XmlWriter.Create(sb);
            m_serializer.Serialize(tw, obj, m_customSerNamespace);

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
                //This version information will be used in future versions of Voltage Fault Detector component.
                string ver_info = tr.GetAttribute(0);

                res = m_serializer.Deserialize(tr);
                SetTableDefault(t, false);
            }
            catch
            {
                res = Activator.CreateInstance(t);

                if (String.IsNullOrEmpty(serializedXml))
                {
                    ((ICyTable)res).InitializeTable(m_numOfDefaultRows);
                    SetTableDefault(t, true);
                }
                else
                {
                    MessageBox.Show(Resources.SettingsIncorrectValues,
                        MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return res;
        }

        void SetTableDefault(Type t, bool isDefault)
        {
            if (t == typeof(CyVoltagesTable))
                m_isVoltagesTableDefault = isDefault;
        }
        #endregion

        #region DRCs
        public bool CheckTableNullValues()
        {
            bool isOk = true;
            for (int i = 0; i < NumVoltages; i++)
            {
                if (((VoltagesTable[i].m_uvFaultThreshold == null) && (CompareType != CyCompareType.OV)) ||
                    ((VoltagesTable[i].m_ovFaultThreshold == null) && (CompareType != CyCompareType.UV)) ||
                    ((VoltagesTable[i].m_inputScalingFactor == null) && ExternalRef == false))
                {
                    isOk = false;
                    break;
                }
            }
            return isOk;
        }

        public bool CheckSiliconRevisionVsExtRef(ICyDeviceQuery_v1 deviceQuery)
        {
            bool res = true;
            if ((deviceQuery.ArchFamilyMember == "PSoC5A") && (ExternalRef == false))
            {
                res = false;
            }
            return res;
        }
        #endregion

        #region Clock functions
        // Returns connected to component pin external clock value
        public static CyClockData GetExternalClock(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData("clock", 0);
            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0];
                }
            }
            return null;
        }

        public static double ConvertFreqToHz(CyClockData clock)
        {
            double frequency = clock.Frequency;
            switch (clock.Unit)
            {
                case CyClockUnit.kHz:
                    frequency *= 1000;
                    break;
                case CyClockUnit.MHz:
                    frequency *= 1000000;
                    break;
                case CyClockUnit.GHz:
                    frequency *= 1000000000;
                    break;
                case CyClockUnit.THz:
                    frequency *= 1000000000000;
                    break;
                default:
                    break;
            }
            return frequency;
        }


        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            CyClockData clkData = CyParameters.GetExternalClock(termQuery);
            if (clkData != null)
            {
                // 1/Hz = sec * 10^6 * 16 * NumVoltages = us
                double freq = (1 / ConvertFreqToHz(clkData) * 1000000) * CyParamRanges.NUM_CYCLE * NumVoltages;
                m_basicTab.UpdateTimeUnitsLabel(freq);
            }
            else
                m_basicTab.UpdateTimeUnitsLabel(null);
        }
        #endregion
    }
}
