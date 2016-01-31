/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace SMBusSlave_v1_0
{
    #region Component Parameters Names
    public class CyParamName
    {
        public const string MODE = "Mode";
        public const string DATA_RATE = "DataRate";
        public const string SLAVE_ADDRESS = "SlaveAddress";
        public const string HEX = "Hex";
        public const string ENABLE_SMB_ALERT_PIN = "EnableSmbAlertPin";
        public const string SMB_ALERT_MODE = "SmbAlertMode";
        public const string PAGED_COMMANDS_SIZE = "PagedCommandsSize";
        public const string ENABLE_RECIEVE_BYTE_PROTOCOL = "EnableRecieveByteProtocol";
        public const string SUPPORT_PAGE_CMD = "SupportPageCmd";
        public const string SUPPORT_QUERY_CMD = "SupportQueryCmd";

        public const string PM_BUS_TABLE = "PmBusTable";
        public const string CUSTOM_TABLE = "CustomTable";

        public const string IMPLEMENTATION = "I2cImplementation";
        public const string ADDRESS_DECODE = "I2cAddressDecode";
        public const string PINS = "I2cPins";
        public const string UDB_INTERNAL_CLOCK = "I2cUdbInternalClock";
        public const string FIXED_PLACEMENT = "I2cUdbSlaveFixedPlacementEnable";
        public const string MINUS_TOLERANCE = "I2cClockMinusTolerance";
        public const string PLUS_TOLERANCE = "I2cClockPlusTolerance";
        public const string EXTERNAL_IO_BUFFER = "ExternalBuffer";
        public const string PM_BUS_HIDE_DISABLED_COMMANDS = "PmBusTableHideDisabledCommands";

        public const string I2C_PRESCALER_ENABLED = "I2cPrescalerEnabled";
        public const string I2C_PRESCALER_PERIOD = "I2cPrescalerPeriod";
        public const string I2C_SCL_TIMEOUT_ENABLED = "I2cSclTimeoutEnabled";
        public const string I2C_SDA_TIMEOUT_ENABLED = "I2cSdaTimeoutEnabled";
        public const string I2C_TIME_OUT_MS = "I2cTimeOutms";
        public const string I2C_TIMEOUT_PERIOD_FF = "I2cTimeoutPeriodff";
        public const string I2C_TIMEOUT_PERIOD_UDB = "I2cTimeoutPeriodUdb";
    }
    #endregion

    #region Component Enums
    public enum CyEModeSelType
    {
        [Description("SMBus Slave")]
        SMBUS_SLAVE,
        [Description("PMBus Slave")]
        PMBUS_SLAVE
    }

    public enum CyESmbAlertModeType
    {
        [Description("Auto Mode")]
        MODE_AUTO = 1,
        [Description("Manual Mode")]
        MODE_MANUAL = 2
    }

    public enum CyECmdType
    {
        [Description("Send Byte")]
        SendByte,
        [Description("Read/Write Byte")]
        ReadWriteByte,
        [Description("Read/Write Word")]
        ReadWriteWord,
        [Description("Read/Write Block")]
        ReadWriteBlock,
        [Description("Process Call")]
        ProcessCall,
        [Description("Block Process Call")]
        BlockProcessCall
    }

    public enum CyEReadWriteConfigType
    {
        [Description("Auto")]
        Auto,
        [Description("Manual")]
        Manual,
        [Description("None")]
        None
    }

    public enum CyEFormatType
    {
        [Description("Non-numeric")]
        NonNumeric,
        [Description("Linear")]
        Linear,
        [Description("Signed")]
        Signed,
        [Description("Direct")]
        Direct,
        [Description("Unsigned")]
        Unsigned,
        [Description("VID Mode")]
        VidMode
    }

    public enum CyECmdGroup
    {           //  WRITE                READ
        GROUP0, // (Auto,Manual,None  /  None)
        GROUP1, // (Auto,Manual,None  /  Auto,Manual,None)
        GROUP2, // (Manual,None       /  None)
        GROUP3, // (None              /  Auto,Manual,None)
        GROUP4, // (Auto,Manual,None  /  Manual,None)
        GROUP5, // (None              /  Manual,None)
        SPECIFIC
    }

    public enum CyEImplementationType { I2C__UDB, I2C__FixedFunction }
    public enum CyEAddressDecodeType { I2C__Software, I2C__Hardware }
    public enum CyEBusPortType { I2C__Any, I2C__I2C0, I2C__I2C1 }
    #endregion

    #region Constants For Parameters Range
    public class CyParamRange
    {
        public const int DATA_RATE_MIN = 1;
        public const int DATA_RATE_MAX = 400;
        public const int SLAVE_ADDRESS_MIN = 0;
        public const int SLAVE_ADDRESS_MAX = 127;
        public const int CMD_CODE_MIN = 0;
        public const int CMD_CODE_MAX = 255;
        public const int PAGED_CMDS_MIN = 1;
        public const int PAGED_CMDS_MAX = 32;

        public const int TOLERANCE_MIN = -5;
        public const int TOLERANCE_MAX = 50;

        public const byte SEND_BYTE_SIZE = 0;
        public const byte READ_WRITE_BYTE_SIZE = 1;
        public const byte READ_WRITE_WORD_SIZE = 2;
        public const byte BLOCK_PROCESS_CALL_SIZE = 16;
        public const byte BOOTLOADER_SIZE = 64;

        public const UInt16 FF_DIVIDER_MAX = 4095;
    }
    #endregion

    [XmlType("SMBusSlave")]
    public class CyParameters
    {
        #region Declaring member variables
        [XmlIgnore]
        public ICyInstQuery_v1 m_inst;
        [XmlIgnore]
        public ICyTerminalQuery_v1 m_termQuery;
        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediately overwrite parameters with the same values.
        [XmlIgnore]
        public bool m_globalEditMode = false;

        [XmlIgnore]
        public CyCustomTableRow m_pageRow;
        [XmlIgnore]
        public CyCustomTableRow m_queryRow;
        [XmlIgnore]
        public CyCustomTableRow m_bootloadWriteRow;
        [XmlIgnore]
        public CyCustomTableRow m_bootloadReadRow;

        [XmlIgnore]
        public double m_extClock;

        // Tabs references
        [XmlIgnore]
        public CyGeneralTab m_generalTab;

        [XmlIgnore]
        public CyCustomCmdsTab m_customCmdsTab;

        [XmlIgnore]
        public CyPmBusCmdsTab m_pmBusCmdsTab;

        [XmlIgnore]
        public CyI2cConfigTab m_i2cConfigTab;

        // List contains display names of types taken from symbol for filling comboboxes
        [XmlIgnore]
        public List<string> m_modeList;

        // Dictionary is used to keep display names of types taken from symbol,
        // and associate them with the enum fields to simplify access to diaplay names
        [XmlIgnore]
        public Dictionary<object, string> m_dnDict = new Dictionary<object, string>();

        public static readonly Color DISABLED_COLUMN_COLOR = SystemColors.ControlLight;
        public static readonly Color ENABLED_COLUMN_COLOR = Color.White;

        public static List<string> CustomTableHeader = new List<string>();
        public static List<string> PMBusCmdsTableHeader = new List<string>();

        // Tables used by the customizer
        [XmlIgnore]
        private CyPMBusTable m_pmBusTable = new CyPMBusTable();
        [XmlIgnore]
        private CyCustomTable m_customTable = new CyCustomTable();

        // Xml serialization parameters
        [XmlIgnore]
        private XmlSerializer m_serializerCyPmBusTable;
        [XmlIgnore]
        private XmlSerializer m_serializerCyCustomTable;
        [XmlIgnore]
        private XmlSerializerNamespaces m_customSerNamespace;

        public static readonly List<byte> ReservedCmdCodes = new List<byte>(new byte[] { 255 });
        public static readonly List<string> ReservedCmdNames = new List<string>(new string[] { 
            CyCustomTable.PAGE, CyCustomTable.QUERY, CyCustomTable.BOOTLOAD_READ, CyCustomTable.BOOTLOAD_WRITE 
        });

        public static readonly List<UInt16> smBusDataRateList = new List<UInt16>(new UInt16[] { 10, 50, 100, 400 });
        public static readonly List<UInt16> pmBusDataRateList = new List<UInt16>(new UInt16[] { 100, 400 });

        [XmlIgnore]
        private Dictionary<string, object> m_restoredParameters = new Dictionary<string, object>();

        [XmlIgnore]
        private bool m_bootloader;
        [XmlIgnore]
        private bool m_customTableIsDefault = false;
        #endregion

        #region Constructor(s)
        public CyParameters() { }

        public CyParameters(ICyInstQuery_v1 inst)
        {
            m_inst = inst;
            m_bootloader = (m_inst.DesignQuery.ApplicationType == CyApplicationType_v1.Bootloader);

            // Create XML Serializer
            m_serializerCyPmBusTable = new XmlSerializer(typeof(CyPMBusTable));
            m_serializerCyCustomTable = new XmlSerializer(typeof(CyCustomTable));

            m_modeList = new List<string>(inst.GetPossibleEnumValues(CyParamName.MODE));

            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyEModeSelType), m_modeList);

            m_customSerNamespace = new XmlSerializerNamespaces();
            string curNamespace = typeof(CyParameters).Namespace;
            string version = curNamespace.Substring(curNamespace.LastIndexOf("_v") + 2);
            m_customSerNamespace.Add("Version", version);

            GetPmBusTable();
            GetCustomTable();
        }

        public CyParameters(ICyInstQuery_v1 inst, ICyTerminalQuery_v1 termQuery)
            : this(inst)
        {
            CyDividerCalculator.CalculateTimeout(inst, termQuery);
            m_termQuery = termQuery;
        }
        #endregion

        #region Class properties

        #region General Tab
        public CyEModeSelType Mode
        {
            get { return GetValue<CyEModeSelType>(CyParamName.MODE); }
            set { SetValue(CyParamName.MODE, value); }
        }

        public UInt16 DataRate
        {
            get { return GetValue<UInt16>(CyParamName.DATA_RATE); }
            set { SetValue(CyParamName.DATA_RATE, value); }
        }

        public UInt16 SlaveAddress
        {
            get { return GetValue<UInt16>(CyParamName.SLAVE_ADDRESS); }
            set { SetValue(CyParamName.SLAVE_ADDRESS, value); }
        }

        public bool EnableSmbAlertPin
        {
            get { return GetValue<bool>(CyParamName.ENABLE_SMB_ALERT_PIN); }
            set { SetValue(CyParamName.ENABLE_SMB_ALERT_PIN, value); }
        }

        public CyESmbAlertModeType SmbAlertMode
        {
            get { return GetValue<CyESmbAlertModeType>(CyParamName.SMB_ALERT_MODE); }
            set { SetValue(CyParamName.SMB_ALERT_MODE, value); }
        }

        public byte PagedCommands
        {
            get { return GetValue<byte>(CyParamName.PAGED_COMMANDS_SIZE); }
            set { SetValue(CyParamName.PAGED_COMMANDS_SIZE, value); }
        }

        public bool EnableRecieveByteProtocol
        {
            get { return GetValue<bool>(CyParamName.ENABLE_RECIEVE_BYTE_PROTOCOL); }
            set { SetValue(CyParamName.ENABLE_RECIEVE_BYTE_PROTOCOL, value); }
        }

        public bool SupportPageCmd
        {
            get { return GetValue<bool>(CyParamName.SUPPORT_PAGE_CMD); }
            set { SetValue(CyParamName.SUPPORT_PAGE_CMD, value); }
        }

        public bool SupportQueryCmd
        {
            get { return GetValue<bool>(CyParamName.SUPPORT_QUERY_CMD); }
            set { SetValue(CyParamName.SUPPORT_QUERY_CMD, value); }
        }
        // Parameters for internal use
        public bool Hex
        {
            get {   return GetValue<bool>(CyParamName.HEX); }
            set { SetValue(CyParamName.HEX, value); }
        }
        #endregion

        #region Tables properties
        public List<CyPMBusTableRow> PmBusTable
        {
            get { return m_pmBusTable.m_pmBusTable; }
            set { m_pmBusTable.m_pmBusTable = value; }
        }

        public List<CyCustomTableRow> CustomTable
        {
            get { return m_customTable.m_customTable; }
            set { m_customTable.m_customTable = value; }
        }
        #endregion

        #region I2C Configuration Tab
        public CyEImplementationType Implementation
        {
            get
            {
                return GetValue<CyEImplementationType>(CyParamName.IMPLEMENTATION);
            }
            set
            {
                SetValue(CyParamName.IMPLEMENTATION, value);
            }
        }

        public CyEAddressDecodeType AddressDecode
        {
            get
            {
                return GetValue<CyEAddressDecodeType>(CyParamName.ADDRESS_DECODE);
            }
            set
            {
                SetValue(CyParamName.ADDRESS_DECODE, value);
            }
        }

        public CyEBusPortType Pins
        {
            get
            {
                return GetValue<CyEBusPortType>(CyParamName.PINS);
            }
            set
            {
                SetValue(CyParamName.PINS, value);
            }
        }

        public bool UdbInternalClock
        {
            get
            {
                return GetValue<bool>(CyParamName.UDB_INTERNAL_CLOCK);
            }
            set
            {
                SetValue(CyParamName.UDB_INTERNAL_CLOCK, value);
            }
        }

        public double MinusTolerance
        {
            get
            {
                return GetValue<double>(CyParamName.MINUS_TOLERANCE);
            }
            set
            {
                SetValue(CyParamName.MINUS_TOLERANCE, value);
            }
        }

        public double PlusTolerance
        {
            get
            {
                return GetValue<double>(CyParamName.PLUS_TOLERANCE);
            }
            set
            {
                SetValue(CyParamName.PLUS_TOLERANCE, value);
            }
        }

        public bool UdbSlaveFixedPlacementEnable
        {
            get
            {
                return GetValue<bool>(CyParamName.FIXED_PLACEMENT);
            }
            set
            {
                SetValue(CyParamName.FIXED_PLACEMENT, value);
            }
        }

        public bool ExternalIOBuffer
        {
            get
            {
                return GetValue<bool>(CyParamName.EXTERNAL_IO_BUFFER);
            }
            set
            {
                SetValue(CyParamName.EXTERNAL_IO_BUFFER, value);
            }
        }

        // Timeout properties

        public bool PrescalerEnabled
        {
            get
            {
                return GetValue<bool>(CyParamName.I2C_PRESCALER_ENABLED);
            }
            set
            {
                SetValue(CyParamName.I2C_PRESCALER_ENABLED, value);
            }
        }

        public byte PrescalerPeriod
        {
            get
            {
                return GetValue<byte>(CyParamName.I2C_PRESCALER_PERIOD);
            }
            set
            {
                SetValue(CyParamName.I2C_PRESCALER_PERIOD, value);
            }
        }

        public bool I2cSclTimeoutEnabled
        {
            get
            {
                return GetValue<bool>(CyParamName.I2C_SCL_TIMEOUT_ENABLED);
            }
            set
            {
                SetValue(CyParamName.I2C_SCL_TIMEOUT_ENABLED, value);
            }
        }

        public bool I2cSdaTimeoutEnabled
        {
            get
            {
                return GetValue<bool>(CyParamName.I2C_SDA_TIMEOUT_ENABLED);
            }
            set
            {
                SetValue(CyParamName.I2C_SDA_TIMEOUT_ENABLED, value);
            }
        }

        public byte TimeOutMs
        {
            get
            {
                return GetValue<byte>(CyParamName.I2C_TIME_OUT_MS);
            }
            set
            {
                SetValue(CyParamName.I2C_TIME_OUT_MS, value);
            }
        }

        public UInt16 TimeoutPeriodFF
        {
            get
            {
                return GetValue<UInt16>(CyParamName.I2C_TIMEOUT_PERIOD_FF);
            }
            set
            {
                SetValue(CyParamName.I2C_TIMEOUT_PERIOD_FF, value);
            }
        }

        public UInt16 TimeoutPeriodUDB
        {
            get
            {
                return GetValue<UInt16>(CyParamName.I2C_TIMEOUT_PERIOD_UDB);
            }
            set
            {
                SetValue(CyParamName.I2C_TIMEOUT_PERIOD_UDB, value);
            }
        }
        #endregion

        #region Others
        public bool PageVisible
        {
            get { return ((Mode == CyEModeSelType.SMBUS_SLAVE) && SupportPageCmd); }
        }

        public bool QueryVisible
        {
            get { return ((Mode == CyEModeSelType.SMBUS_SLAVE) && SupportQueryCmd); }
        }

        public bool IsPSoC5A
        {
            get { return (m_inst.DeviceQuery.ArchFamilyMember == "PSoC5A"); }
        }

        public bool HideDisabledPMBusCommands
        {
            get
            {
                return GetValue<bool>(CyParamName.PM_BUS_HIDE_DISABLED_COMMANDS);
            }
            set
            {
                SetValue(CyParamName.PM_BUS_HIDE_DISABLED_COMMANDS, value);
            }
        }
        
        #endregion

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

        public int GetCustomTableIndexByReference(CyCustomTableRow row)
        {
            for (int i = 0; i < CustomTable.Count; i++)
            {
                if (CustomTable[i] == row)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region Setting Parameters

        private void SetValue<T>(string paramName, T value)
        {
            if (m_globalEditMode)
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

            // Saving data after deserialization
            if ((m_globalEditMode == false) && (m_inst == null))
            {
                if (m_restoredParameters.ContainsKey(paramName))
                {
                    Debug.Assert(false);
                    return;
                }
                m_restoredParameters.Add(paramName, value);
            }
        }

        public void RestoreParameters(Dictionary<string, object> restoreList)
        {
            bool prevGlobalEditMode = m_globalEditMode;
            m_globalEditMode = true;
            foreach (string param_name in restoreList.Keys)
            {
                SetValue<object>(param_name, restoreList[param_name]);
            }
            m_globalEditMode = prevGlobalEditMode;
        }
        #endregion

        #region Getting/Setting parameters with XML Data
        public void GetPmBusTable()
        {
            string xmlData = GetValue<string>(CyParamName.PM_BUS_TABLE);
            CyPMBusTable package = (CyPMBusTable)Deserialize(xmlData, typeof(CyPMBusTable),
                CyCmdData.PMBusCmdList.Length);
            PmBusTable = package.m_pmBusTable;
        }

        public void SetPmBusTable()
        {
            if (m_globalEditMode)
            {
                CyPMBusTable package = new CyPMBusTable();
                package.m_pmBusTable = new List<CyPMBusTableRow>(PmBusTable.ToArray());
                SetValue(CyParamName.PM_BUS_TABLE, Serialize(package));
            }
        }

        public void GetCustomTable()
        {
            string xmlData = GetValue<string>(CyParamName.CUSTOM_TABLE);
            CyCustomTable package = (CyCustomTable)Deserialize(xmlData, typeof(CyCustomTable), 0);
            CustomTable = package.m_customTable;
            // Check if PAGE and QUERY commands have to be added and add if necessary.
            // This applyes for default table only.
            if (m_customTableIsDefault)
            {
                if (PageVisible)
                    CustomTable.Insert(0, CyCustomTable.GetDefaultPageRow());
                if (QueryVisible)
                    CustomTable.Insert((PageVisible ? 1 : 0), CyCustomTable.GetDefaultQueryRow());
            }
            FilterCustomTable();
        }

        public void SetCustomTable()
        {
            if (m_globalEditMode)
            {
                CyCustomTable package = new CyCustomTable();
                // Add bootloader rows to the object before serialization. They are currently
                // not there because of non-bootloader project. They should be serialized to save 
                // user settings from previous change.
                if (m_bootloader == false)
                {
                    CustomTable.Add(m_bootloadReadRow);
                    CustomTable.Add(m_bootloadWriteRow);
                }
                package.m_customTable = new List<CyCustomTableRow>(CustomTable.ToArray());
                SetValue(CyParamName.CUSTOM_TABLE, Serialize(package));

                // There was saved invisible rows to the table to save user specific commands
                // Remove them again after save to avoid their appearence in the datagrid
                FilterCustomTable();
            }
        }

        private void FilterCustomTable()
        {
            // Remove specific extra rows from table according to user settings
            List<int> indexesToRemove = new List<int>();
            for (int i = 0; i < CustomTable.Count; i++)
            {
                if (CustomTable[i].m_specific && CustomTable[i].m_name == CyCustomTable.BOOTLOAD_READ)
                {
                    m_bootloadReadRow = new CyCustomTableRow(CustomTable[i]);
                }
                if (CustomTable[i].m_specific && CustomTable[i].m_name == CyCustomTable.BOOTLOAD_READ &&
                    m_bootloader == false)
                {
                    indexesToRemove.Add(i);
                }

                if (CustomTable[i].m_specific && CustomTable[i].m_name == CyCustomTable.BOOTLOAD_WRITE)
                {
                    m_bootloadWriteRow = new CyCustomTableRow(CustomTable[i]);
                }
                if (CustomTable[i].m_specific && CustomTable[i].m_name == CyCustomTable.BOOTLOAD_WRITE &&
                    m_bootloader == false)
                {
                    indexesToRemove.Add(i);
                }
            }

            if (CustomTable.Contains(null))
            {
                // Custom table may contain null elements if there is no 
                // BOOTLOAD_READ or BOOTLOAD_WRITE command in table
                bool containsBootloadReadCmd = false;
                bool containsBootloadWriteCmd = false;
                foreach (CyCustomTableRow item in CustomTable)
                {
                    if (item.m_name == CyCustomTable.BOOTLOAD_READ)
                        containsBootloadReadCmd = true;
                    if (item.m_name == CyCustomTable.BOOTLOAD_WRITE)
                        containsBootloadWriteCmd = true;
                }
                if (containsBootloadReadCmd == false)
                {
                    Debug.Assert(false, string.Format("Custom commands table does not contain {0} command. {0} " +
                        "command will be created with default values.", CyCustomTable.BOOTLOAD_READ));
                }
                if (containsBootloadWriteCmd == false)
                {
                    Debug.Assert(false, string.Format("Custom commands table does not contain {0} command. {0} " +
                        "command will be created with default values.", CyCustomTable.BOOTLOAD_WRITE));
                }
            }

            for (int i = indexesToRemove.Count - 1; i >= 0; i--)
            {
                CustomTable.RemoveAt(indexesToRemove[i]);
            }
        }
        #endregion

        #region DataGridView
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
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 246, 249);
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }

        public static string IsValueInRange(double? currCellValue, object cellValue, double min, double max,
            string errorMessage, bool displayHex)
        {
            string res = string.Empty;
            string message = string.Empty;
            try
            {
                if (displayHex)
                    message = string.Format(errorMessage, CyParameters.CellConvertHex(min),
                        CyParameters.CellConvertHex(max));
                else
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

        public static List<string> GetColNames(DataGridView dgv)
        {
            List<string> colNames = new List<string>();
            for (int i = 0; i < dgv.ColumnCount; i++)
                if (dgv.Columns[i].Visible)
                {
                    colNames.Add(dgv.Columns[i].HeaderText);
                }
            return colNames;
        }

        private static bool IsCellEmpty(object val)
        {
            return (val == null) || ((val != null) && (String.IsNullOrEmpty(val.ToString())));
        }

        public static void SetReadOnlyState(DataGridViewColumn dgvColumn, bool readOnly)
        {
            dgvColumn.CellTemplate.Style.BackColor = readOnly ? DISABLED_COLUMN_COLOR : ENABLED_COLUMN_COLOR;
            dgvColumn.ReadOnly = readOnly;
        }

        public static void SetReadOnlyState(DataGridView dgv, DataGridViewColumn dgvColumn, int row, bool readOnly)
        {
            dgv[dgvColumn.Index, row].ReadOnly = readOnly;
            dgv[dgvColumn.Index, row].Style.BackColor = readOnly ? DISABLED_COLUMN_COLOR : ENABLED_COLUMN_COLOR;
        }

        public static void SetCellReadOnlyState(DataGridViewCell cell, bool readOnly)
        {
            cell.ReadOnly = readOnly;
            cell.Style.BackColor = readOnly ? DISABLED_COLUMN_COLOR : ENABLED_COLUMN_COLOR;
        }
        #endregion

        #region Nullable operations
        public static string CellToString(object cellValue)
        {
            string res = string.Empty;
            if (cellValue != null)
                res = cellValue.ToString();
            return res;
        }

        public static byte? ParseNullableHexByte(object val)
        {
            string strVal = CellToString(val);
            byte? res = 0;
            try
            {
                res = Convert.ToByte(strVal, 16);
            }
            catch { res = null; }

            return res;
        }

        public static string NullableByteToHex(object value)
        {
            string res = string.Empty;
            byte? convertedValue = CyParameters.ParseNullableByte(value);
            if (convertedValue.HasValue)
                res = convertedValue.Value.ToString("X");

            return res;
        }

        public static byte? ParseNullableByte(object val)
        {
            string strVal = CellToString(val);
            byte? res = null;
            byte parsed = 0;
            if (byte.TryParse(strVal, out parsed))
            {
                res = parsed;
            }
            return res;
        }

        public static bool CellToBool(object val)
        {
            bool res = false;
            try
            {
                res = Convert.ToBoolean(Convert.ToInt32(val));
            }
            catch { }
            return res;
        }

        public static string CellConvertHex(object value)
        {
            string format;
            byte byteValue;
            try
            {
                byteValue = Convert.ToByte(value);
            }
            catch (Exception)
            {
                byteValue = 0;
            }
            if (byteValue < 16)
                format = @"0x0{0}";
            else
                format = @"0x{0:X2}";

            string hexValue = byteValue.ToString("X");

            return string.Format(format, hexValue);
        }

        public static string CellFormatHex(object value)
        {
            string format;
            UInt32 intValue = 0;
            string result = string.Empty;
            try
            {
                intValue = Convert.ToUInt32(value.ToString(), 16);
                if (intValue < 16)
                    format = @"0x0{0}";
                else
                    format = @"0x{0}";
                result = string.Format(format, intValue.ToString("X"));
            }
            catch (Exception)
            {
                result = value.ToString();
            }
            return result;
        }
        #endregion

        #region XML Serialization
        public void LoadFromObject(string fileContent)
        {
            CyParameters imported = (CyParameters)Deserialize(fileContent, typeof(CyParameters), 0);
            if (imported != null)
            {
                RestoreParameters(imported.m_restoredParameters);

                CustomTable = imported.CustomTable;
                PmBusTable = imported.PmBusTable;

                //Commit Tables
                SetCustomTable();
                SetPmBusTable();

                //Update mode
                m_pmBusCmdsTab.SetPMBusCmdsTabVisibility(Mode);

                //Update tabs
                m_generalTab.UpdateUI();
                m_i2cConfigTab.UpdateUI();
                m_pmBusCmdsTab.UpdateUIFromTable();
                m_customCmdsTab.UpdateUIFromTable();

                m_pmBusCmdsTab.ValidateAllTable();
                m_customCmdsTab.ValidateAllTable();
            }
        }

        private XmlSerializer GetSerializer(Type type)
        {
            if (type == typeof(CyPMBusTable))
                return m_serializerCyPmBusTable;
            else if (type == typeof(CyCustomTable))
                return m_serializerCyCustomTable;
            else
                return new XmlSerializer(type);
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

        public object Deserialize(string serializedXml, Type t, int numOfDefaultRows)
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
                //This version information will be used in future versions of component.
                string ver_info = tr.GetAttribute(0);

                res = GetSerializer(t).Deserialize(tr);
            }
            catch
            {
                res = Activator.CreateInstance(t);

                if (res is ICyTable)
                {
                    ((ICyTable)res).InitializeTable(numOfDefaultRows);
                    if (t.FullName == CyCustomTable.GetString())
                    {
                        m_customTableIsDefault = true; // the flag indicates whether Custom table has default values
                    }
                }
                else
                    return null;

                if (String.IsNullOrEmpty(serializedXml) == false)
                {
                    MessageBox.Show(Resources.SettingsIncorrectValues,
                        MessageBoxIcon.Warning.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Mark reserved rows as specific
            if (res != null && t == typeof(CyCustomTable))
                CyCustomTable.SetReservedRowsFlag((res as CyCustomTable).m_customTable);
            else if (res != null && t == typeof(CyParameters))
                CyCustomTable.SetReservedRowsFlag((res as CyParameters).CustomTable);

            return res;
        }
        #endregion

        #region DRC Verification
        public bool CheckCustomTableNullValues()
        {
            bool isOk = true;
            for (int i = 0; i < CustomTable.Count; i++)
            {
                if ((CustomTable[i].m_name == null) ||
                    (CustomTable[i].m_code == null) ||
                    (CustomTable[i].m_size == null))
                {
                    isOk = false;
                    break;
                }
            }
            return isOk;
        }

        public bool CheckImplementationWithSilicon()
        {
            return !((Implementation == CyEImplementationType.I2C__FixedFunction) && IsPSoC5A);
        }
        #endregion

        #region Save/Load file
        public static void SaveToFile(string fileName, string fileContent)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(fileContent);
                }
            }
            catch
            {
                MessageBox.Show(Resources.FileWriteError, fileName, MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        public static string GetFileText(string fileName)
        {
            string result = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch
            {
                MessageBox.Show(Resources.FileReadError, fileName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            return result;
        }
        #endregion

        #region Clock and calculator
        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            m_extClock = CyClockReader.GetExternalClockInKHz(termQuery);
            m_generalTab.UpdateCalculator();
            UpdateTimeout();
        }

        public void UpdateTimeout()
        {
            CyDividerCalculator.CalculateTimeout(m_inst, m_termQuery);
        }
        #endregion
    }

    #region Specification commands
    public class CyCommand
    {
        public byte m_cmdCode;
        public string m_cmdName;
        public CyECmdType m_type;
        public byte? m_size;
        public bool m_sizeUnlocked;
        public CyECmdGroup m_cmdGroup;

        public CyCommand()
        {
        }

        public CyCommand(byte cmdCode, string cmdName, CyECmdType type, byte? size, CyECmdGroup cmdRwType)
        {
            m_cmdCode = cmdCode;
            m_cmdName = cmdName;
            m_type = type;
            m_size = size;
            m_sizeUnlocked = false;
            m_cmdGroup = cmdRwType;
        }

        public CyCommand(byte cmdCode, string cmdName, CyECmdType type, byte? size, CyECmdGroup cmdRwType,
            bool sizeUnlocked)
        {
            m_cmdCode = cmdCode;
            m_cmdName = cmdName;
            m_type = type;
            m_size = size;
            m_sizeUnlocked = sizeUnlocked;
            m_cmdGroup = cmdRwType;
        }
    }

    public static class CyCmdData
    {
        private const byte DEFAULT_BLOCK_SIZE = 16;

        #region Command Summary from PMBus Power System Mgt Protocol Specification
        public readonly static CyCommand[] PMBusCmdList = new CyCommand[]
        { 
            new CyCommand(0x00, "PAGE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.SPECIFIC),
            new CyCommand(0x01, "OPERATION", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x02, "ON_OFF_CONFIG", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x03, "CLEAR_FAULTS", CyECmdType.SendByte, 0, CyECmdGroup.GROUP2),
            new CyCommand(0x04, "PHASE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            // PAGE_PLUS_WRITE and PAGE_PLUS_READ commands are not supported
            new CyCommand(0x10, "WRITE_PROTECT", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x11, "STORE_DEFAULT_ALL", CyECmdType.SendByte, 0, CyECmdGroup.GROUP2),
            new CyCommand(0x12, "RESTORE_DEFAULT_ALL", CyECmdType.SendByte, 0, CyECmdGroup.GROUP2),
            new CyCommand(0x13, "STORE_DEFAULT_CODE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP0),
            new CyCommand(0x14, "RESTORE_DEFAULT_CODE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP0),
            new CyCommand(0x15, "STORE_USER_ALL", CyECmdType.SendByte, 0, CyECmdGroup.GROUP2),
            new CyCommand(0x16, "RESTORE_USER_ALL", CyECmdType.SendByte, 0, CyECmdGroup.GROUP2),
            new CyCommand(0x17, "STORE_USER_CODE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP0),
            new CyCommand(0x18, "RESTORE_USER_CODE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP0),
            new CyCommand(0x19, "CAPABILITY", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP3),
            new CyCommand(0x1A, "QUERY", CyECmdType.BlockProcessCall, 1, CyECmdGroup.SPECIFIC),
            new CyCommand(0x1B, "SMBALERT_MASK", CyECmdType.BlockProcessCall, 2, CyECmdGroup.GROUP4),
            new CyCommand(0x20, "VOUT_MODE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x21, "VOUT_COMMAND", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x22, "VOUT_TRIM", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x23, "VOUT_CAL_OFFSET", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x24, "VOUT_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x25, "VOUT_MARGIN_HIGH", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x26, "VOUT_MARGIN_LOW", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x27, "VOUT_TRANSITION_RATE", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x28, "VOUT_DROOP", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x29, "VOUT_SCALE_LOOP", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x2A, "VOUT_SCALE_MONITOR", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x30, "COEFFICIENTS", CyECmdType.BlockProcessCall, 5, CyECmdGroup.GROUP5),
            new CyCommand(0x31, "POUT_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x32, "MAX_DUTY", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x33, "FREQUENCY_SWITCH", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x35, "VIN_ON", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x36, "VIN_OFF", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x37, "INTERLEAVE", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x38, "IOUT_CAL_GAIN", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x39, "IOUT_CAL_OFFSET", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x3A, "FAN_CONFIG_1_2", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x3B, "FAN_COMMAND_1", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x3C, "FAN_COMMAND_2", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x3D, "FAN_CONFIG_3_4", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x3E, "FAN_COMMAND_3", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x3F, "FAN_COMMAND_4", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x40, "VOUT_OV_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x41, "VOUT_OV_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x42, "VOUT_OV_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x43, "VOUT_UV_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x44, "VOUT_UV_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x45, "VOUT_UV_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x46, "IOUT_OC_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x47, "IOUT_OC_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x48, "IOUT_OC_LV_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x49, "IOUT_OC_LV_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x4A, "IOUT_OC_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x4B, "IOUT_UC_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x4C, "IOUT_UC_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x4F, "OT_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x50, "OT_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x51, "OT_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x52, "UT_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x53, "UT_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x54, "UT_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x55, "VIN_OV_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x56, "VIN_OV_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x57, "VIN_OV_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x58, "VIN_UV_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x59, "VIN_UV_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x5A, "VIN_UV_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x5B, "IIN_OC_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x5C, "IIN_OC_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x5D, "IIN_OC_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x5E, "POWER_GOOD_ON", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x5F, "POWER_GOOD_OFF", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x60, "TON_DELAY", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x61, "TON_RISE", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x62, "TON_MAX_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x63, "TON_MAX_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x64, "TOFF_DELAY", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x65, "TOFF_FALL", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x66, "TOFF_MAX_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x68, "POUT_OP_FAULT_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x69, "POUT_OP_FAULT_RESPONSE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x6A, "POUT_OP_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x6B, "PIN_OP_WARN_LIMIT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x78, "STATUS_BYTE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x79, "STATUS_WORD", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0x7A, "STATUS_VOUT", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x7B, "STATUS_IOUT", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x7C, "STATUS_INPUT", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x7D, "STATUS_TEMPERATURE", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x7E, "STATUS_CML", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x7F, "STATUS_OTHER", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x80, "STATUS_MFR_SPECIFIC", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x81, "STATUS_FANS_1_2", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x82, "STATUS_FANS_3_4", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP1),
            new CyCommand(0x86, "READ_EIN", CyECmdType.ReadWriteBlock, 5, CyECmdGroup.GROUP5),
            new CyCommand(0x87, "READ_EOUT", CyECmdType.ReadWriteBlock, 5, CyECmdGroup.GROUP5),
            new CyCommand(0x88, "READ_VIN", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x89, "READ_IIN", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x8A, "READ_VCAP", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x8B, "READ_VOUT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x8C, "READ_IOUT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x8D, "READ_TEMPERATURE_1", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x8E, "READ_TEMPERATURE_2", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x8F, "READ_TEMPERATURE_3", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x90, "READ_FAN_SPEED_1", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x91, "READ_FAN_SPEED_2", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x92, "READ_FAN_SPEED_3", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x93, "READ_FAN_SPEED_4", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x94, "READ_DUTY_CYCLE", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x95, "READ_FREQUENCY", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x96, "READ_POUT", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x97, "READ_PIN", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0x98, "PMBUS_REVISION", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP3),
            new CyCommand(0x99, "MFR_ID", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, true),
            new CyCommand(0x9A, "MFR_MODEL", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, true),
            new CyCommand(0x9B, "MFR_REVISION", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0x9C, "MFR_LOCATION", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0x9D, "MFR_DATE", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, true),
            new CyCommand(0x9E, "MFR_SERIAL", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, true),
            new CyCommand(0x9F, "APP_PROFILE_SUPPORT", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, 
                CyECmdGroup.GROUP3, true),
            new CyCommand(0xA0, "MFR_VIN_MIN", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA1, "MFR_VIN_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA2, "MFR_IIN_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA3, "MFR_PIN_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA4, "MFR_VOUT_MIN", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA5, "MFR_VOUT_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA6, "MFR_IOUT_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA7, "MFR_POUT_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA8, "MFR_TAMBIENT_MAX", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xA9, "MFR_TAMBIENT_MIN", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP3),
            new CyCommand(0xAA, "MFR_EFFICIENCY_LL", CyECmdType.ReadWriteBlock, 14, CyECmdGroup.GROUP3),
            new CyCommand(0xAB, "MFR_EFFICIENCY_HL", CyECmdType.ReadWriteBlock, 14, CyECmdGroup.GROUP3),
            new CyCommand(0xAC, "MFR_PIN_ACCURACY", CyECmdType.ReadWriteByte, 1, CyECmdGroup.GROUP3),
            new CyCommand(0xAD, "IC_DEVICE_ID", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP3, 
                true),
            new CyCommand(0xAE, "IC_DEVICE_REV", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP3, 
                true),
            new CyCommand(0xB0, "USER_DATA_00", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB1, "USER_DATA_01", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB2, "USER_DATA_02", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB3, "USER_DATA_03", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB4, "USER_DATA_04", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB5, "USER_DATA_05", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB6, "USER_DATA_06", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB7, "USER_DATA_07", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB8, "USER_DATA_08", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xB9, "USER_DATA_09", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xBA, "USER_DATA_10", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xBB, "USER_DATA_11", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xBC, "USER_DATA_12", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xBD, "USER_DATA_13", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xBE, "USER_DATA_14", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xBF, "USER_DATA_15", CyECmdType.ReadWriteBlock, DEFAULT_BLOCK_SIZE, CyECmdGroup.GROUP1, 
                true),
            new CyCommand(0xC0, "MFR_MAX_TEMP_1", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0xC1, "MFR_MAX_TEMP_2", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1),
            new CyCommand(0xC2, "MFR_MAX_TEMP_3", CyECmdType.ReadWriteWord, 2, CyECmdGroup.GROUP1)
        };
        #endregion

        public static CyCommand GetSpecDataByCode(int code)
        {
            for (int i = 0; i < PMBusCmdList.Length; i++)
            {
                if (PMBusCmdList[i].m_cmdCode == code)
                    return PMBusCmdList[i];
            }

            return new CyCommand();
        }

        public static int GetIndexByCode(int code)
        {
            for (int i = 0; i < PMBusCmdList.Length; i++)
            {
                if (PMBusCmdList[i].m_cmdCode == code)
                    return i;
            }

            return -1;
        }

        public static object GetCmdGroupByCode(int code)
        {
            for (int i = 0; i < PMBusCmdList.Length; i++)
            {
                if (PMBusCmdList[i].m_cmdCode == code)
                    return PMBusCmdList[i].m_cmdGroup;
            }

            return null;
        }
    }
    #endregion

    public class CyImportExport
    {
        private static char Separator = ',';
        private static string Pattern = "{0}" + Separator + " ";

        #region Export functions
        public static string Export(CyParameters prms, List<object> exportTable, List<string> colHeaders)
        {
            StringBuilder sb = new StringBuilder();
            if (exportTable == null || exportTable.Count == 0) return string.Empty;

            sb.AppendLine(GetHeaders(colHeaders));

            string line = string.Empty;


            for (int i = 0; i < exportTable.Count; i++)
            {
                if (exportTable[i] is CyCustomTableRow)
                {
                    CyCustomTableRow row = (exportTable[i] as CyCustomTableRow);

                    sb.AppendFormat(Pattern, Convert.ToByte(row.m_enable));
                    sb.AppendFormat(Pattern, row.m_name);
                    sb.AppendFormat(Pattern, GetHexString(row.m_code));
                    sb.AppendFormat(Pattern, row.m_type);
                    sb.AppendFormat(Pattern, row.m_format);
                    sb.AppendFormat(Pattern, CyParameters.CellToString(row.m_size));
                    sb.AppendFormat(Pattern, Convert.ToByte(row.m_paged));
                    sb.AppendFormat(Pattern, row.m_readConfig);
                    sb.AppendFormat(row.m_writeConfig.ToString());
                }
                else if (exportTable[i] is CyPMBusTableRow)
                {
                    CyPMBusTableRow row = (exportTable[i] as CyPMBusTableRow);

                    sb.AppendFormat(Pattern, Convert.ToByte(row.m_enable));
                    sb.AppendFormat(Pattern, row.Name);
                    sb.AppendFormat(Pattern, GetHexString(row.m_code));
                    sb.AppendFormat(Pattern, row.Type);
                    sb.AppendFormat(Pattern, row.m_format);
                    sb.AppendFormat(Pattern, CyParameters.CellToString(row.Size));
                    sb.AppendFormat(Pattern, Convert.ToByte(row.m_paged));
                    sb.AppendFormat(Pattern, row.ReadConfig);
                    sb.AppendFormat(row.WriteConfig.ToString());
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        static string GetHeaders(List<string> colHeaders)
        {
            string header = "";
            for (int i = 0; i < colHeaders.Count; i++)
            {
                header += colHeaders[i] + Separator;
            }
            header = header.TrimEnd(Separator);

            return header;
        }
        #endregion

        #region Import functions
        public static bool Import(CyTabWrapper parent, CyParameters prms, string fileContent, bool pasteMode)
        {
            List<object> importedTable = new List<object>();
            List<ICyRow> elements = new List<ICyRow>();

            if (parent is CyCustomCmdsTab)
            {
                importedTable = CyImportExport.ImportTable<CyCustomTableRow>(
                    CyParameters.CustomTableHeader.Count, fileContent, pasteMode);
            }
            else if (parent is CyPmBusCmdsTab)
            {
                importedTable = CyImportExport.ImportTable<CyPMBusTableRow>(
                    CyParameters.PMBusCmdsTableHeader.Count, fileContent, pasteMode);
            }

            if (importedTable != null && importedTable.Count > 0)
            {
                if (importedTable[0].GetType() == typeof(CyCustomTableRow))
                {
                    ICyRow firstSelectedRow = null;
                    if (pasteMode == false)
                    {
                        CyCustomTable table = new CyCustomTable();
                        table.InitializeTable(0);
                        prms.CustomTable = table.m_customTable;
                    }
                    else
                    {
                        firstSelectedRow = parent.DataGridFirstSelectedRow;
                        List<ICyRow> list = new List<ICyRow>(parent.GetObjectsFromDataGrid());
                        if (list != null && list.Count == prms.CustomTable.Count)
                            for (int i = 0; i < list.Count; i++)
                                prms.CustomTable[i] = (CyCustomTableRow)list[i];
                    }

                    for (int i = 0; i < importedTable.Count; i++)
                    {
                        CyCustomTableRow importedRow = ((CyCustomTableRow)importedTable[i]);

                        if (CyCustomTable.IsCmdSpecific(importedRow.m_name) && (pasteMode == false))
                        {
                            // Handle specific commands
                            for (int j = 0; j < prms.CustomTable.Count; j++)
                            {
                                CyCustomTableRow row = prms.CustomTable[j];
                                if (row.m_name == importedRow.m_name)
                                {
                                    row.m_code = importedRow.m_code;
                                    continue;
                                }
                            }
                        }

                        if (pasteMode)
                        {
                            if (firstSelectedRow != null)
                            {
                                int pos = prms.CustomTable.IndexOf((CyCustomTableRow)firstSelectedRow);

                                prms.CustomTable.Insert(pos, importedRow);

                                firstSelectedRow = null;
                                pos++;
                                if (pos < prms.CustomTable.Count)
                                    firstSelectedRow = prms.CustomTable[pos];
                            }
                            else
                                prms.CustomTable.Add(importedRow);

                            // Add to list of rows which will be selected
                            elements.Add(importedRow);

                        }
                        else
                            prms.CustomTable.Add(importedRow);

                    }

                    // Commit Tables
                    prms.SetCustomTable();
                }
                else if (importedTable[0].GetType() == typeof(CyPMBusTableRow))
                {
                    if (pasteMode == false)
                        if (importedTable.Count != prms.PmBusTable.Count)
                        {
                            MessageBox.Show(Resources.ImportRowCountWarning, Resources.DlgWarning,
                                MessageBoxButtons.YesNo);
                        }

                    for (int i = 0; i < importedTable.Count; i++)
                    {
                        foreach (CyPMBusTableRow row in prms.PmBusTable)
                            if (row.m_code == ((CyPMBusTableRow)importedTable[i]).m_code)
                            {
                                row.AssignValues(((CyPMBusTableRow)importedTable[i]));

                                // Add to list of rows which will be selected
                                elements.Add(row);
                            }

                    }

                    // Commit Tables
                    prms.SetPmBusTable();
                }


                parent.UpdateUIFromTable();
                parent.ValidateAllTable();

                // Select paste rows
                if (pasteMode)
                    parent.SelectRows(elements);
            }
            else
                return false;

            return true;
        }

        public static List<object> ImportTable<T>(int columnCount,
            string fileContent, bool pasteMode) where T : class
        {
            List<string> rows = ValidateTextData(columnCount, fileContent);

            if (rows.Count == 0) return null;

            List<T> importTable = new List<T>();

            int id = 1;
            while (id < rows.Count)
            {
                T el = (T)Activator.CreateInstance(typeof(T));
                try
                {
                    string[] cells = rows[id].Split(new char[] { Separator }, StringSplitOptions.None);

                    if (typeof(T) == typeof(CyCustomTableRow))
                    {
                        int inc = 0;
                        CyCustomTableRow row = (el as CyCustomTableRow);

                        row.m_enable = CyParameters.CellToBool(cells[inc++].Trim().ToString());
                        row.m_name = cells[inc++].Trim().ToString();
                        row.m_code = CyParameters.ParseNullableHexByte(cells[inc++].Trim());
                        row.m_type = (CyECmdType)CyEnumConverter.GetEnumValue(
                                cells[inc++].Trim(), typeof(CyECmdType));
                        row.m_format = (CyEFormatType)CyEnumConverter.GetEnumValue(
                                cells[inc++].Trim(), typeof(CyEFormatType));
                        row.m_size = CyParameters.ParseNullableByte(cells[inc++].Trim());
                        row.m_paged = CyParameters.CellToBool(cells[inc++].Trim().ToString());
                        row.m_readConfig = (CyEReadWriteConfigType)CyEnumConverter.GetEnumValue(
                                cells[inc++].Trim(), typeof(CyEReadWriteConfigType));
                        row.m_writeConfig = (CyEReadWriteConfigType)CyEnumConverter.GetEnumValue(
                                cells[inc++].Trim(), typeof(CyEReadWriteConfigType));
                    }
                    else if (typeof(T) == typeof(CyPMBusTableRow))
                    {
                        int inc = 0;
                        CyPMBusTableRow row = (el as CyPMBusTableRow);

                        row.m_enable = CyParameters.CellToBool(cells[inc++].Trim().ToString());
                        string name = cells[inc++].Trim().ToString();
                        byte? code = CyParameters.ParseNullableHexByte(cells[inc++].Trim());
                        if (code == null)
                            continue;

                        row.m_code = (byte)code;

                        inc++;//m_type
                        row.m_format = (CyEFormatType)CyEnumConverter.GetEnumValue(
                                cells[inc++].Trim(), typeof(CyEFormatType));
                        row.Size = CyParameters.ParseNullableByte(cells[inc++].Trim());
                        row.m_paged = CyParameters.CellToBool(cells[inc++].Trim().ToString());
                        row.ReadConfig = (CyEReadWriteConfigType)CyEnumConverter.GetEnumValue(
                                cells[inc++].Trim(), typeof(CyEReadWriteConfigType));
                        row.WriteConfig = (CyEReadWriteConfigType)CyEnumConverter.GetEnumValue(
                                cells[inc++].Trim(), typeof(CyEReadWriteConfigType));
                    }
                }
                catch (Exception)
                {
                    return null;
                }


                importTable.Add(el);
                id++;
            }

            if (importTable == null) return null;
            return new List<object>(importTable.ToArray());

        }

        private static List<T> CreateList<T>(int count) where T : class
        {
            List<T> list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(null);
            }
            return list;
        }

        private static int GetLastPasteRow(int importRowCount, int datagridActiveRowIndex, int gridRowsCount)
        {
            int lastPasteRow = datagridActiveRowIndex + importRowCount;

            return lastPasteRow <= gridRowsCount ? lastPasteRow : gridRowsCount;
        }

        private static List<string> ValidateTextData(int numberOfColumns, string content)
        {
            List<string> result = new List<string>();
            try
            {
                string[] rows = content.Split(new char[] { '\n' }, StringSplitOptions.None);

                string[] cells;
                for (int i = 0; i < rows.Length; i++)
                {
                    cells = rows[i].Split(new char[] { Separator }, StringSplitOptions.None);

                    //Stop if empty line occurs
                    if (cells.Length == 1)
                        break;

                    //Validate columns count
                    if (cells.Length != numberOfColumns)
                        throw new Exception(Resources.InvalidDataFormat);

                    //Add to results list
                    result.Add(rows[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return result;
        }
        #endregion

        #region Additional
        public static string GetHexString(byte? val)
        {
            string hexStr = string.Empty;
            if (val.HasValue)
            {
                hexStr = string.Format("0x{0:X}", val);
                if (val.Value < 16)
                    hexStr = hexStr.Replace("0x", "0x0");
            }
            return hexStr;
        }
        #endregion
    }

    public class CyHexConvertor
    {
        public static string HEX_PREFIX = "0x";

        public static int HexToInt(string hexString)
        {
            int result = -1;
            try
            {
                result = Convert.ToInt32(hexString, 16);
            }
            catch (Exception) { }
            return result;
        }

        public static string IntToHex(int intValue)
        {
            return (HEX_PREFIX + intValue.ToString("X"));
        }
    }
}
