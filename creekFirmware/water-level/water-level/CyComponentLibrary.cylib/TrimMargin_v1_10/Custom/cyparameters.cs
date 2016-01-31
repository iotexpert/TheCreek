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

namespace TrimMargin_v1_10
{
    #region Component Parameters Names
    public class CyParamNames
    {
        public const string NUM_CONVERTERS = "NumConverters";
        public const string PWM_RESOLUTION = "PWMResolution";
        public const string VOLTAGES_TABLE = "VoltagesTable";
        public const string HARDWARE_TABLE = "HardwareTable";
    }
    #endregion

    #region Parameters Ranges
    public class CyParamRanges
    {
        // Basic tab parameters
        public const byte NUM_CONVERTERS_MIN = 1;
        public const byte NUM_CONVERTERS_MAX = 24;
        public const byte PWM_RESOLUTION_MIN = 8;
        public const byte PWM_RESOLUTION_MAX = 10;

        // Voltages tab parameters
        public const double VOLTAGE_MIN = 0.01;
        public const double VOLTAGE_MAX = 12.00;
        public const double MARGIN_LOW_PERCENT_MIN = -100;
        public const double MARGIN_LOW_PERCENT_MAX = 0;
        public const double MARGIN_HIGH_PERCENT_MIN = 0;
        public const double MARGIN_HIGH_PERCENT_MAX = 100;

        // Harware tab parameters
        public const double VDDIO_MIN = 1.8;
        public const double VDDIO_MAX = 5.5;

        public const double CONTROL_VOLTAGE_MIN = 0.01;
        public const double CONTROL_VOLTAGE_MAX = 12.0;

        public const double RESISTOR_MIN = 0.1;
        public const double RESISTOR_MAX = 10000;

        public const double MAX_RIPPLE_MIN = 0.1;
        public const double MAX_RIPPLE_MAX = 100;

        public const double NUM_CONVERTERS_WITH_PWM_RESOLUTION_MAX = 12;
        public const double PWM_RESOLUTION_WITH_NUM_CONVERTERS_MIN = 8;

        public const CyPWMPolarityType R4_RESISTOR_EDITIBLE = CyPWMPolarityType.Positive;
    }
    #endregion

    #region Component Enums
    public enum CyPWMPolarityType
    {
        [Description("Positive")]
        Positive = 0,
        [Description("Negative")]
        Negative = 1
    }
    #endregion

    public class CyParameters
    {
        public static DataGridViewCellStyle DGDisabledStyle;
        public static DataGridViewCellStyle DGEnabledStyle;
        public static List<string> VoltagesHeader = new List<string>();
        public static List<string> HardwareHeader = new List<string>();

        // List contains display names of type taken from symbol. It is used to fill combobox.
        public List<string> m_filterTypeList;

        // Dictionary is used to keep display names of types taken from symbol,
        // and associate them with the enum fields to simplify access to diaplay names
        public Dictionary<object, string> m_displayNameDict = new Dictionary<object, string>();

        public ICyInstQuery_v1 m_inst;
        public ICyTerminalQuery_v1 m_term;
        public CyVoltagesTab m_voltagesTab;
        public CyHardwareTab m_hardwareTab;

        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        private bool m_bGlobalEditMode = false;

        private List<CyVoltagesTableRow> m_voltagesTable;
        private List<CyHardwareTableRow> m_hardwareTable;
        private bool m_isVoltagesTableDefault = false;
        private byte m_numOfDefaultRows = 0;

        // Xml serialization parameters
        public XmlSerializer m_serializerVoltages;
        public XmlSerializer m_serializerHardware;
        public XmlSerializerNamespaces m_customSerNamespace;

        #region Constructor(s)
        public CyParameters(ICyInstQuery_v1 inst)
        {
            m_inst = inst;

            // Create XML Serializer
            m_serializerVoltages = new XmlSerializer(typeof(CyVoltagesTable));
            m_serializerHardware = new XmlSerializer(typeof(CyHardwareTable));
            m_customSerNamespace = new XmlSerializerNamespaces();
            Type classType = typeof(CyParameters);
            string curNamespace = classType.Namespace;
            string version = curNamespace.Substring(curNamespace.LastIndexOf("_v") + 2);
            m_customSerNamespace.Add("Version", version);

            //m_filterTypeList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.VOLTAGE_FILTER_TYPE));
            //CyDictParser.FillDictionary(ref m_displayNameDict, typeof(CyEFilterType), m_filterTypeList);
            GetVoltagesTable();

            GetHardwareTable();
        }

        public CyParameters(ICyInstQuery_v1 inst, ICyTerminalQuery_v1 termQuery)
            : this(inst)
        {
            m_term = termQuery;
        }

        public CyParameters() { }

        #endregion

        #region Class properties
        public bool GlobalEditMode
        {
            get { return m_bGlobalEditMode; }
            set { m_bGlobalEditMode = value; }
        }

        public byte NumConverters
        {
            get { return GetValue<byte>(CyParamNames.NUM_CONVERTERS); }
            set { SetValue(CyParamNames.NUM_CONVERTERS, value); }
        }

        public byte PWMResolution
        {
            get { return GetValue<byte>(CyParamNames.PWM_RESOLUTION); }
            set { SetValue(CyParamNames.PWM_RESOLUTION, value); }
        }

        public List<CyVoltagesTableRow> VoltagesTable
        {
            get { return m_voltagesTable; }
            set { m_voltagesTable = value; }
        }
        public List<CyHardwareTableRow> HardwareTable
        {
            get { return m_hardwareTable; }
            set { m_hardwareTable = value; }
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
            m_numOfDefaultRows = this.NumConverters;
            CyVoltagesTable package = (CyVoltagesTable)Deserialize(xmlData, typeof(CyVoltagesTable));
            m_voltagesTable = package.m_voltagesTable;
        }

        public void GetHardwareTable()
        {
            string xmlData = GetValue<string>(CyParamNames.HARDWARE_TABLE);
            m_numOfDefaultRows = this.NumConverters;
            CyHardwareTable package = (CyHardwareTable)Deserialize(xmlData, typeof(CyHardwareTable));
            m_hardwareTable = package.m_hardwareTable;
        }
        #endregion

        #region Import or paste rows
        public bool Import(Control parent, string fileContent, bool pasteMode)
        {
            if (parent is CyVoltagesTab)
            {
                List<CyVoltagesTableRow> importedTable = CyImportExport.ImportVoltagesTable(NumConverters,
                    CyVoltagesTable.COL_COUNT, fileContent, pasteMode);

                int firstPasteRow = -1;
                int lastPasteRow = -1;
                if (importedTable != null)
                {
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
                        VoltagesTable = new List<CyVoltagesTableRow>(importedTable);
                    }
                    m_voltagesTab.UpdateUIFromTable();
                    m_hardwareTab.UpdateUIFromTable();
                    SetVoltagesTable();
                    SetHardwareTable();
                    m_voltagesTab.ValidateAllTable();
                    m_voltagesTab.SelectDataGridViewRows(firstPasteRow, lastPasteRow - 1);
                }
                else return false;
            }

            if (parent is CyHardwareTab)
            {
                List<CyHardwareTableRow> importedTable = CyImportExport.ImportHardwareTable(NumConverters,
                    CyHardwareTable.COL_COUNT, fileContent, pasteMode);

                int firstPasteRow = -1;
                int lastPasteRow = -1;
                if (importedTable != null)
                {
                    if (pasteMode)
                    {
                        firstPasteRow = m_hardwareTab.DataGridFirstSelectedRow;
                        lastPasteRow = GetLastPasteRow(importedTable.Count, firstPasteRow, NumConverters);

                        int j = 0;
                        for (int i = firstPasteRow; i < lastPasteRow; i++)
                        {
                            HardwareTable[i] = importedTable[j];
                            j++;
                        }
                    }
                    else
                    {
                        HardwareTable = new List<CyHardwareTableRow>(importedTable);
                    }
                    m_hardwareTab.UpdateUIFromTable();
                    SetHardwareTable();
                    m_hardwareTab.ValidateAllTable();
                    m_hardwareTab.SelectDataGridViewRows(firstPasteRow, lastPasteRow - 1);
                }
                else return false;
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
        public static string NullableDoubleToString(double? val)
        {
            return NullableDoubleToString(val, 2);
        }
        public static string NullableDoubleToString(double? val, byte precision)
        {
            string res = "";
            string strPrecision;

            strPrecision = (precision <= 0) ? strPrecision = string.Empty : strPrecision = "f" + precision.ToString();

            if (val.HasValue)
                res = val.Value.ToString(strPrecision);
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
            CyVoltagesTable package = new CyVoltagesTable();
            package.m_voltagesTable = new List<CyVoltagesTableRow>(m_voltagesTable);
            package.m_voltagesTable.RemoveRange(NumConverters, package.m_voltagesTable.Count - NumConverters);
            SetValue(CyParamNames.VOLTAGES_TABLE, Serialize(package));
        }

        public void SetHardwareTable()
        {
            CyHardwareTable package = new CyHardwareTable();
            package.m_hardwareTable = new List<CyHardwareTableRow>(m_hardwareTable);
            package.m_hardwareTable.RemoveRange(NumConverters, package.m_hardwareTable.Count - NumConverters);
            SetValue(CyParamNames.HARDWARE_TABLE, Serialize(package));
        }

        #endregion

        #region XML Serialization

        XmlSerializer GetSerializer(Type type)
        {
            if (type == typeof(CyHardwareTable))
                return m_serializerHardware;
            else if (type == typeof(CyVoltagesTable))
                return m_serializerVoltages;

            return null;
        }

        public string Serialize(object obj)
        {
            StringBuilder sb = new StringBuilder();

            System.Xml.XmlWriter tw = System.Xml.XmlWriter.Create(sb);
            GetSerializer(obj.GetType()).Serialize(tw, obj, m_customSerNamespace);

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

                res = GetSerializer(t).Deserialize(tr);
                SetTableDefault(t, false);
            }
            catch
            {
                res = Activator.CreateInstance(t);

                ((ICyIntTable)res).InitializeTable(m_numOfDefaultRows);
                SetTableDefault(t, true);

                if (String.IsNullOrEmpty(serializedXml) == false)
                {
                    MessageBox.Show(Resources.SettingsIncorrectValues,
                        MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return res;
        }
        #endregion

        #region DataGridView
        void SetTableDefault(Type t, bool isDefault)
        {
            if (t == typeof(CyVoltagesTable))
                m_isVoltagesTableDefault = isDefault;
        }

        public static string IsValueInRange(double? currCellValue, object cellValue, double min, double max,
            string errorMessage)
        {
            string res = string.Empty;
            string message = string.Empty;
            try
            {
                message = string.Format(errorMessage, min, max);

                if ((currCellValue.HasValue) && (currCellValue < min || currCellValue > max))
                    throw new Exception();
                else if ((currCellValue == null) && (IsCellEmpty(cellValue) == false))
                    throw new Exception();
            }
            catch (Exception)
            {
                res = message;
            }
            return res;
        }

        private static bool IsCellEmpty(object val)
        {
            return (val == null) || ((val != null) && (String.IsNullOrEmpty(val.ToString())));
        }

        const double EPS = 0.000001;
        public static string ValueNotGreaterThan(double? value, double? comapreTo, string message)
        {
            string res = string.Empty;
            if (value.HasValue)
            {
                if (comapreTo.HasValue && (value > comapreTo + EPS))
                    res = message;
            }
            return res;
        }

        public static void InitializeDataGrid(DataGridView dgv)
        {
            // Initialize data grid view
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;

            // Prevent data grid from sorting
            foreach (DataGridViewColumn item in dgv.Columns)
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }
        public static void UpdateDGVHeight(DataGridView dgv)
        {
            if (dgv.Rows.Count > 0)
            {
                int height = 2 + dgv.ColumnHeadersHeight + dgv.Rows[0].Height * dgv.Rows.Count;
                dgv.Height = height;
            }
        }

        public bool CheckVoltagesTableNullValues()
        {
            bool isOk = true;
            for (int i = 0; i < NumConverters; i++)
            {
                if ((VoltagesTable[i].m_nominalVoltage == null) ||
                    (VoltagesTable[i].m_minVoltage == null) ||
                    (VoltagesTable[i].m_maxVoltage == null) ||
                    (VoltagesTable[i].MarginLow == null) ||
                    (VoltagesTable[i].MarginHigh == null))
                {
                    isOk = false;
                    break;
                }
            }
            return isOk;
        }

        public bool CheckHardwareTableNullValues()
        {
            bool isOk = true;
            for (int i = 0; i < NumConverters; i++)
            {
                if ((HardwareTable[i].m_controlVoltage == null) ||
                    (HardwareTable[i].m_maxRipple == null) ||
                    ((HardwareTable[i].m_r1 == null) && (HardwareTable[i].m_polarity == CyPWMPolarityType.Negative)) ||
                    ((HardwareTable[i].m_r2 == null) && (HardwareTable[i].m_polarity == CyPWMPolarityType.Negative)) ||
                    ((HardwareTable[i].m_r3 == null) && (HardwareTable[i].m_polarity == CyPWMPolarityType.Negative)) ||
                    (HardwareTable[i].m_vddio == null) ||
                    (HardwareTable[i].m_polarity == CyParamRanges.R4_RESISTOR_EDITIBLE &&
                    HardwareTable[i].m_calculatedR4 == null))
                {
                    isOk = false;
                    break;
                }
            }
            return isOk;
        }

        public static void SetCellReadOnlyState(DataGridView dgv, int row, int col, bool readOnly)
        {
            dgv[col, row].Style = readOnly ? DGDisabledStyle : DGEnabledStyle;
            dgv[col, row].ReadOnly = readOnly;
        }
        public static List<string> GetColNames(DataGridView dgv)
        {
            List<string> colNames = new List<string>();
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                colNames.Add(dgv.Columns[i].HeaderText);
            }
            return colNames;
        }
        #endregion

        #region Value Calculation

        /// <summary>
        /// Returns internal clock value
        /// </summary>
        public static double? GetInternalClock(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();

            clkdata = termQuery.GetClockData("clock", 0);

            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0].Frequency * Math.Pow(10, clkdata[0].UnitAsExponent);
                }
            }
            return null;
        }

        public void CalculateTableValues(int index)
        {
            double? clockFr = GetInternalClock(m_term);

            CyHardwareTableRow row = m_hardwareTable[index];

            if (row.m_r1 != null && row.m_r2 != null && row.m_r3 != null && row.m_controlVoltage != null &&
                m_voltagesTable[index].m_maxVoltage != null)
            {
                double r1 = (double)row.m_r1 * 1000;
                //                double r2 = (double)row.m_r2 * 1000;
                double r3 = (double)row.m_r3 * 1000;
                double vadj = (double)row.m_controlVoltage;
                /* Recalculate R2, take into account internal power convertor resistance */
                try
                {
                    double? r2 = r1 * vadj / ((double)m_voltagesTable[index].m_nominalVoltage - vadj);
                    double? maxVoltage = (double)m_voltagesTable[index].m_maxVoltage;
                    row.m_calculatedR3 = (vadj * r1 * r2) / ((maxVoltage * r2) - vadj * (r1 + r2));
                    row.m_calculatedR3 /= 1000;
                }
                catch (Exception)
                {
                    row.m_calculatedR3 = null;
                }
            }
            else
                row.m_calculatedR3 = null;

            //if (row.m_polarity == CyPWMPolarityType.Negative && row.m_r3 != null)
            //    row.m_calculatedR4 = row.m_r3 / 10;


            if (clockFr != null && row.m_maxRipple != null && row.m_calculatedR4 != null && row.m_maxRipple != null &&
                row.m_vddio != null)
            {
                double mripple = (double)row.m_maxRipple / 1000;
                double fPWMOut = (double)clockFr / Math.Pow(2, (int)PWMResolution);

                double r4 = (double)row.m_calculatedR4 * 1000;

                row.m_calculatedC1 = row.m_vddio / (2 * Math.PI * r4 * fPWMOut * mripple);

                row.m_calculatedC1 *= 1000000;
            }
            else row.m_calculatedC1 = null;

            if (row.m_r1 != null && row.m_r3 != null && row.m_calculatedR4 != null && row.m_controlVoltage != null
                && row.m_vddio != null && VoltagesTable != null && VoltagesTable.Count > index &&
                VoltagesTable[index].m_nominalVoltage != null)
            {
                double vAdj = (double)row.m_controlVoltage;
                double vPwm = (double)row.m_vddio;
                double vNom = (double)VoltagesTable[index].m_nominalVoltage;
                double r2Real = ((vNom - vAdj) == 0) ?
                    (double)row.m_r2 :
                    (double)(vAdj * row.m_r1 / Math.Abs(vNom - vAdj));
                double vMax = (double)(vAdj * row.m_r1 /
                    (r2Real * (row.m_r3 + row.m_calculatedR4) / (r2Real + row.m_r3 + row.m_calculatedR4))
                    + vAdj);
                double countsNom = (Math.Pow(2.0, (double)PWMResolution) - 1) * vAdj / vPwm;
                double trimRes = (vMax - vNom) * 1000.0 / countsNom;
                row.m_calculatedResolution = trimRes;
            }
            else
            {
                row.m_calculatedResolution = null;
            }
        }

        #endregion
    }
}
