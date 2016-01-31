/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using CyDesigner.Extensions.Gde;


namespace StaticSegLCD_v1_20
{
    public class CyLCDParameters
    {
        #region Variables

        readonly ICyInstEdit_v1 m_inst;

        // *m_Parameters:
        // * 
        // * NumCommonLines
        // * NumSegmentLines
        // * BiasType
        // * WaveformType
        // * FrameRate
        // * BiasVoltage
        // * 
        // * DriverPowerMode
        // * HiDriveTime
        // * LowDriveMode
        // * LowDriveInitTime
        // * LowDriveDutyCycleTime
        // * UseInternalClock
        // * ClockFrequency
        // * EnableInterrupt
        // *

        public static string FormatErrorMsg = "The value should be between {0:G} and {1:G}.";
        public static string FormatIntErrorMsg = "The value should be integer between {0:G} and {1:G}.";

        private const byte m_NumCommonLines = 1;
        private byte m_NumSegmentLines = 8;
        private byte m_WaveformType = 0;
        private byte m_FrameRate = 0;

        private byte m_DriverPowerMode = 0;
        private byte m_HiDriveTime = 0;
        private byte m_LowDriveMode = 0;
        private byte m_LowDriveInitTime = 0;
        private byte m_LowDriveDutyCycleTime = 0;

        private bool m_UseInternalClock = false;
        private uint m_ClockFrequency = 800;
        private byte m_DacDisInitTime = 0;
        private bool m_EnableInterrupt = false;

        private bool m_DebugMode;

        public List<HelperInfo> m_HelpersConfig;
        private String m_SerializedHelpers;
        private string m_DisabledCommons;

        public CyColorList m_ColorChooser = new CyColorList();

        public List<int> m_SymbolIndexes_7SEG = new List<int>();
        public List<int> m_SymbolIndexes_14SEG = new List<int>();
        public List<int> m_SymbolIndexes_16SEG = new List<int>();
        public List<int> m_SymbolIndexes_BAR = new List<int>();
        public List<int> m_SymbolIndexes_MATRIX = new List<int>();

        public List<int> m_HelperIndexes_7SEG = new List<int>();
        public List<int> m_HelperIndexes_14SEG = new List<int>();
        public List<int> m_HelperIndexes_16SEG = new List<int>();
        public List<int> m_HelperIndexes_BAR = new List<int>();
        public List<int> m_HelperIndexes_MATRIX = new List<int>();

        private int m_ParametersChanged = 0;

        #endregion Variables

        #region Properties

        public byte NumCommonLines
        {
            get {return m_NumCommonLines;}
        }
        public byte NumSegmentLines
        {
            get { return m_NumSegmentLines; }
            set
            {
                if (value != m_NumSegmentLines)
                {
                    m_NumSegmentLines = value;
                    SetParam("NumSegmentLines");
                    CommitParams();
                }
            }
        }
       
        public byte WaveformType
        {
            get { return m_WaveformType; }
            set
            {
                if (value != m_WaveformType)
                {
                    m_WaveformType = value;
                    SetParam("WaveformType");
                    CommitParams();
                }
            }
        }
        public byte FrameRate
        {
            get { return m_FrameRate; }
            set
            {
                if (value != m_FrameRate)
                {
                    m_FrameRate = value;
                    SetParam("FrameRate");
                    CommitParams();
                }
            }
        }

        public byte DriverPowerMode
        {
            get { return m_DriverPowerMode; }
            set
            {
                if (value != m_DriverPowerMode)
                {
                    m_DriverPowerMode = value;
                    SetParam("DriverPowerMode");
                    CommitParams();
                }
            }
        }
        
        public byte HiDriveTime
        {
            get { return m_HiDriveTime; }
            set
            {
                if (value != m_HiDriveTime)
                {
                    m_HiDriveTime = value;
                    SetParam("HiDriveTime");
                    CommitParams();
                }
            }
        }
        public byte LowDriveMode
        {
            get { return m_LowDriveMode; }
            set
            {
                if (value != m_LowDriveMode)
                {
                    m_LowDriveMode = value;
                    SetParam("LowDriveMode");
                    CommitParams();
                }
            }
        }
        public byte LowDriveInitTime
        {
            get { return m_LowDriveInitTime; }
            set
            {
                if (value != m_LowDriveInitTime)
                {
                    m_LowDriveInitTime = value;
                    SetParam("LowDriveInitTime");
                    CommitParams();
                }
            }
        }
        public byte LowDriveDutyCycleTime
        {
            get { return m_LowDriveDutyCycleTime; }
            set
            {
                if (value != m_LowDriveDutyCycleTime)
                {
                    m_LowDriveDutyCycleTime = value;
                    SetParam("LowDriveDutyCycleTime");
                    CommitParams();
                }
            }
        }

        public bool UseInternalClock
        {
            get { return m_UseInternalClock; }
            set
            {
                if (value != m_UseInternalClock)
                {
                    m_UseInternalClock = value;
                    SetParam("UseInternalClock");
                    CommitParams();
                }
            }
        }

        public uint ClockFrequency
        {
            get { return m_ClockFrequency; }
            set
            {
                if (value != m_ClockFrequency)
                {
                    m_ClockFrequency = value;
                    SetParam("ClockFrequency");

                    m_DacDisInitTime = Convert.ToByte(Math.Ceiling(m_ClockFrequency/Math.Pow(10, 5)));
                    if (m_DacDisInitTime == 0)
                        m_DacDisInitTime = 1;
                    SetParam("DacDisInitTime");

                    CommitParams();
                }
            }
        }

        public bool EnableInterrupt
        {
            get { return m_EnableInterrupt; }
            set
            {
                if (value != m_EnableInterrupt)
                {
                    m_EnableInterrupt = value;
                    SetParam("EnableInterrupt");
                    CommitParams();
                }
            }
        }

        public bool DebugMode
        {
            get { return m_DebugMode; }
            set
            {
                if (value != m_DebugMode)
                {
                    m_DebugMode = value;
                    SetParam("DebugMode");
                    CommitParams();
                }
            }
        }


        public List<int> DisabledCommons
        {
            get
            {
                List<int> res = new List<int>();
                if (m_DisabledCommons != "")
                {
                    string[] nums = m_DisabledCommons.Split(',');
                    for (int i = 0; i < nums.Length; i++)
                        res.Add(Convert.ToInt32(nums[i]));
                }
                return res;
            }
            set
            {
                string res = "";
                for (int i = 0; i < value.Count; i++)
                {
                    res += value[i] + ",";
                }
                res = res.TrimEnd(',');
                if (res != m_DisabledCommons)
                {
                    m_DisabledCommons = res;
                    SetParam("DisabledCommons");
                    CommitParams();
                }
            }
        }

        public string SerializedHelpers
        {
            get { return m_SerializedHelpers; }
            set
            {
                if (value != m_SerializedHelpers)
                {
                    m_SerializedHelpers = value;
                    m_SerializedHelpers = m_SerializedHelpers.Replace("\r\n", " ");
                    SetParam("Helpers");
                    CommitParams();
                }
            }
        }

        public bool ParametersChanged
        {
            set
            {
                m_ParametersChanged++;
                SetParam("ParamChanged");
                CommitParams();
            }
        }

        #endregion Properties

        #region Constructors

        public CyLCDParameters()
        {
            m_HelpersConfig = new List<HelperInfo>();
            DisabledCommons = new List<int>();
            HelperInfo.CreateHelper(CyHelperKind.Empty, this);
        }

        public CyLCDParameters(ICyInstEdit_v1 inst)
        {
            DisabledCommons = new List<int>();
            if (inst != null)
            {
                this.m_inst = inst;
                GetParams();
            }
        }

        #endregion Constructors

        #region Common


        private void GetParams()
        {
            if (m_inst != null)
            {
                m_ParametersChanged = Convert.ToInt32(m_inst.GetCommittedParam("ParamChanged").Value);

                m_NumSegmentLines = GetByteParam("NumSegmentLines");
                m_WaveformType = GetByteParam("WaveformType");
                m_FrameRate = GetByteParam("FrameRate");

                m_DriverPowerMode = GetByteParam("DriverPowerMode");
                m_HiDriveTime = GetByteParam("HiDriveTime");
                m_LowDriveMode = GetByteParam("LowDriveMode");
                m_LowDriveInitTime = GetByteParam("LowDriveInitTime");
                m_LowDriveDutyCycleTime = GetByteParam("LowDriveDutyCycleTime");

                m_UseInternalClock = GetBoolParam("UseInternalClock");
                m_ClockFrequency = GetUintParam("ClockFrequency");
                m_EnableInterrupt = GetBoolParam("EnableInterrupt");
                m_DebugMode = GetBoolParam("DebugMode");
                m_DisabledCommons = GetStringParam("DisabledCommons");
                DeserializeHelpers(GetStringParam("Helpers"));
            }
        }

        private uint GetUintParam(string paramName)
        {
            CyCompDevParam temp = m_inst.GetCommittedParam(paramName);
            uint value = 0;
            if (temp != null)
            {
                value = Convert.ToUInt32(temp.Value);
            }
            return value;
        }

        private string GetStringParam(string paramName)
        {
            CyCompDevParam temp = m_inst.GetCommittedParam(paramName);
            string value = string.Empty;
            if (temp != null)
            {
                value = Convert.ToString(temp.Value);
            }
            return value;
        }

        private bool GetBoolParam(string paramName)
        {
            CyCompDevParam temp = m_inst.GetCommittedParam(paramName);
            bool value = false;
            if (temp != null)
            {
                value = Convert.ToBoolean(temp.Value);
            }
            return value;
        }

        private byte GetByteParam(string paramName)
        {
            CyCompDevParam temp = m_inst.GetCommittedParam(paramName);
            byte value = 0;
            if (temp != null)
            {
                value = Convert.ToByte(temp.Value);
            }
            return value;
        }

        public void DeserializeHelpers(string _SHelpers)
        {
            try
            {
                if (!string.IsNullOrEmpty(_SHelpers))
                {
                    m_HelpersConfig = HelperInfo.DeserializeHelpers(_SHelpers);
                    // Add used helper and symbol indexes to the list
                    for (int i = 0; i < m_HelpersConfig.Count; i++)
                    {
                        switch (m_HelpersConfig[i].Kind)
                        {
                            case CyHelperKind.Segment7:
                                m_HelperIndexes_7SEG.Add(m_HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_7SEG.Contains(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        m_SymbolIndexes_7SEG.Add(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                //For compability remove the 8th segment in symbols if the one exists
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (m_HelpersConfig[i].HelpSegInfo[j].RelativePos == 7)
                                    {
                                        m_HelpersConfig[i].HelpSegInfo.RemoveAt(j--);
                                    }
                                }
                                m_HelpersConfig[i].SegmentCount = 7;
                                break;
                            case CyHelperKind.Segment14:
                                m_HelperIndexes_14SEG.Add(m_HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_14SEG.Contains(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        m_SymbolIndexes_14SEG.Add(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            case CyHelperKind.Segment16:
                                m_HelperIndexes_16SEG.Add(m_HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_16SEG.Contains(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        m_SymbolIndexes_16SEG.Add(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            case CyHelperKind.Bar:
                                m_HelperIndexes_BAR.Add(m_HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_BAR.Contains(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        m_SymbolIndexes_BAR.Add(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            case CyHelperKind.Matrix:
                                m_HelperIndexes_MATRIX.Add(m_HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_MATRIX.Contains(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        m_SymbolIndexes_MATRIX.Add(m_HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    m_HelpersConfig = new List<HelperInfo>();
                    HelperInfo.CreateHelper(CyHelperKind.Empty, this);
                }
            }
            catch
            {
                MessageBox.Show("Error in Helpers parameter", "SegLCD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetParam(string paramName)
        {
            if (m_inst != null)
            {
                switch (paramName)
                {
                    case "NumCommonLines":
                        m_inst.SetParamExpr("NumCommonLines", NumCommonLines.ToString());
                        break;
                    case "NumSegmentLines":
                        m_inst.SetParamExpr("NumSegmentLines", NumSegmentLines.ToString());
                        break;
                    case "WaveformType":
                        m_inst.SetParamExpr("WaveformType", WaveformType.ToString());
                        break;
                    case "FrameRate":
                        m_inst.SetParamExpr("FrameRate", FrameRate.ToString());
                        break;
                    case "DriverPowerMode":
                        m_inst.SetParamExpr("DriverPowerMode", DriverPowerMode.ToString());
                        break;
                    case "HiDriveTime":
                        m_inst.SetParamExpr("HiDriveTime", HiDriveTime.ToString());
                        break;
                    case "LowDriveMode":
                        m_inst.SetParamExpr("LowDriveMode", LowDriveMode.ToString());
                        break;
                    case "LowDriveInitTime":
                        m_inst.SetParamExpr("LowDriveInitTime", LowDriveInitTime.ToString());
                        break;
                    case "LowDriveDutyCycleTime":
                        m_inst.SetParamExpr("LowDriveDutyCycleTime", LowDriveDutyCycleTime.ToString());
                        break;
                    case "UseInternalClock":
                        m_inst.SetParamExpr("UseInternalClock", UseInternalClock.ToString());
                        break;
                    case "ClockFrequency":
                        m_inst.SetParamExpr("ClockFrequency", ClockFrequency.ToString());
                        break;
                    case "DacDisInitTime":
                        m_inst.SetParamExpr("DacDisInitTime", m_DacDisInitTime.ToString());
                        break;
                    case "EnableInterrupt":
                        m_inst.SetParamExpr("EnableInterrupt", EnableInterrupt.ToString());
                        break;
                    case "DebugMode":
                        m_inst.SetParamExpr("DebugMode", DebugMode.ToString().ToLower());
                        break;
                    case "DisabledCommons":
                        m_inst.SetParamExpr("DisabledCommons", m_DisabledCommons);
                        break;
                    case "Helpers":
                        m_inst.SetParamExpr("Helpers", SerializedHelpers);
                        break;
                    case "ParamChanged":
                        m_inst.SetParamExpr("ParamChanged", m_ParametersChanged.ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        public void SetParams()
        {
            if (m_inst != null)
            {
                m_inst.SetParamExpr("NumCommonLines", NumCommonLines.ToString());
                m_inst.SetParamExpr("NumSegmentLines", NumSegmentLines.ToString());
                m_inst.SetParamExpr("WaveformType", WaveformType.ToString());
                m_inst.SetParamExpr("FrameRate", FrameRate.ToString());

                m_inst.SetParamExpr("DriverPowerMode", DriverPowerMode.ToString());
                m_inst.SetParamExpr("HiDriveTime", HiDriveTime.ToString());
                m_inst.SetParamExpr("LowDriveMode", LowDriveMode.ToString());
                m_inst.SetParamExpr("LowDriveInitTime", LowDriveInitTime.ToString());
                m_inst.SetParamExpr("LowDriveDutyCycleTime", LowDriveDutyCycleTime.ToString());
                m_inst.SetParamExpr("UseInternalClock", UseInternalClock.ToString());
                m_inst.SetParamExpr("ClockFrequency", ClockFrequency.ToString());
                m_inst.SetParamExpr("DacDisInitTime", m_DacDisInitTime.ToString());
                m_inst.SetParamExpr("EnableInterrupt", EnableInterrupt.ToString());
                m_inst.SetParamExpr("DebugMode", DebugMode.ToString().ToLower());

                m_inst.SetParamExpr("DisabledCommons", m_DisabledCommons);
                m_inst.SetParamExpr("Helpers", SerializedHelpers);
                m_inst.SetParamExpr("ParamChanged", m_ParametersChanged.ToString());
            }
        }

        public void CommitParams()
        {
            if (m_inst != null)
            {
                if (!m_inst.CommitParamExprs())
                {
                    /*if (m_inst.GetCommittedParam("NumCommonLines").ErrorCount > 0)
                        MessageBox.Show("Error in Committing NumCommonLines");
                    if (m_inst.GetCommittedParam("NumSegmentLines").ErrorCount > 0)
                        MessageBox.Show("Error in Committing NumSegmentLines");
                    if (m_inst.GetCommittedParam("FrameBufferMemoryModel").ErrorCount > 0)
                        MessageBox.Show("Error in Committing FrameBufferMemoryModel");
                    if (m_inst.GetCommittedParam("BiasType").ErrorCount > 0)
                        MessageBox.Show("Error in Committing BiasType");
                    if (m_inst.GetCommittedParam("WaveformType").ErrorCount > 0)
                        MessageBox.Show("Error in Committing WaveformType");
                    if (m_inst.GetCommittedParam("FrameRate").ErrorCount > 0)
                        MessageBox.Show("Error in Committing FrameRate");
                    if (m_inst.GetCommittedParam("BiasVoltage").ErrorCount > 0)
                        MessageBox.Show("Error in Committing BiasVoltage");

                    if (m_inst.GetCommittedParam("DriverPowerMode").ErrorCount > 0)
                        MessageBox.Show("Error in Committing DriverPowerMode");
                    if (m_inst.GetCommittedParam("HiDriveTime").ErrorCount > 0)
                        MessageBox.Show("Error in Committing HiDriveTime");
                    if (m_inst.GetCommittedParam("LowDriveMode").ErrorCount > 0)
                        MessageBox.Show("Error in Committing LowDriveMode");
                    if (m_inst.GetCommittedParam("LowDriveInitTime").ErrorCount > 0)
                        MessageBox.Show("Error in Committing LowDriveInitTime");
                    if (m_inst.GetCommittedParam("LowDriveDutyCycleTime").ErrorCount > 0)
                        MessageBox.Show("Error in Committing LowDriveDutyCycleTime");
                    if (m_inst.GetCommittedParam("UseInternalClock").ErrorCount > 0)
                        MessageBox.Show("Error in Committing UseInternalClock");
                    if (m_inst.GetCommittedParam("ClockFrequency").ErrorCount > 0)
                        MessageBox.Show("Error in Committing ClockFrequency");
                    if (m_inst.GetCommittedParam("EnableInterrupt").ErrorCount > 0)
                        MessageBox.Show("Error in Committing EnableInterrupt");*/
                }
            }
        }

        #endregion Common
    }


    #region Helper classes

    public class CySegmentInfo
    {
        private string m_Name;
        private int m_Common;
        private int m_Segment;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        [XmlAttribute("Com")]
        public int Common
        {
            get { return m_Common; }
            set { m_Common = value; }
        }
        [XmlAttribute("Seg")]
        public int Segment
        {
            get { return m_Segment; }
            set { m_Segment = value; }
        }

        public CySegmentInfo()
        {
            Name = "PIX";
        }

        public CySegmentInfo(string name, int common, int segment)
        {
            Name = name;
            Common = common;
            Segment = segment;
        }
    }

    [XmlRootAttribute("Pixel")]
    public class CyHelperSegmentInfo : CySegmentInfo
    {
        [XmlAttribute("Display")]
        public int DisplayNum;
        [XmlAttribute("Digit")]
        public int DigitNum;
        [XmlAttribute("Helper")]
        public CyHelperKind HelperType;
        [XmlAttribute("RelPos")]
        public int RelativePos;

        public int GlobalDigitNum;

        public CyHelperSegmentInfo()
        {
            HelperType = CyHelperKind.Empty;
        }

        public CyHelperSegmentInfo(string name, int common, int segment, int displayNum, int digitNum, int relPos, 
            CyHelperKind type, int globalDigitNum) : base(name, common, segment)
        {
            DisplayNum = displayNum;
            DigitNum = digitNum;
            RelativePos = relPos;
            HelperType = type;

            GlobalDigitNum = globalDigitNum;
        }
        public int GetIndex(int CommonCount)
        {
            return CommonCount * Segment + Common;
        }

        public int GetMaxElementCount()
        {
            switch (HelperType)
            {
                case CyHelperKind.Segment7:
                    return 7;
                case CyHelperKind.Segment14:
                    return 14;
                case CyHelperKind.Segment16:
                    return 16;
                case CyHelperKind.Bar:
                    return 1;
                case CyHelperKind.Matrix:
                    return 40;
                case CyHelperKind.Empty:
                    break;
                default:
                    break;
            }
            return 0;
        }
    }
    
    [XmlRoot("Helper")]
    public class HelperInfo
    {
        public const int MAX_7SEG_SYMBOLS = 5;
        public const int MAX_14SEG_SYMBOLS = 20;
        public const int MAX_16SEG_SYMBOLS = 20;
        public const int MAX_BAR_SYMBOLS = 255;
        public const int MAX_MATRIX_SYMBOLS = 8;

        public string Name;
        private CyHelperKind m_Kind;
        private int m_SymbolsCount;
        private int m_MaxSymbolsCount;
        private int m_SegmentCount;
        private int m_DisplayNum;
        [XmlElementAttribute("Color")]
        public int m_HelperColor;

        //public static List<int> m_SymbolIndexes_7SEG = new List<int>();
        //public static List<int> m_SymbolIndexes_14SEG = new List<int>();
        //public static List<int> m_SymbolIndexes_16SEG = new List<int>();
        //public static List<int> m_SymbolIndexes_BAR = new List<int>();
        //public static List<int> m_SymbolIndexes_MATRIX = new List<int>();

        //public static List<int> m_HelperIndexes_7SEG = new List<int>();
        //public static List<int> m_HelperIndexes_14SEG = new List<int>();
        //public static List<int> m_HelperIndexes_16SEG = new List<int>();
        //public static List<int> m_HelperIndexes_BAR = new List<int>();
        //public static List<int> m_HelperIndexes_MATRIX = new List<int>();

        public int GlobalHelperIndex;

        [XmlIgnore]
        public Color HelperColor
        {
            get { return Color.FromArgb(m_HelperColor); }
            set { m_HelperColor = value.ToArgb(); }
        }
        [XmlElementAttribute("Kind")]
        public CyHelperKind Kind
        {
            get { return m_Kind; }
            set { m_Kind = value; }
        }
        [XmlElementAttribute("MaxSymbolsCount")]
        public int MaxSymbolsCount
        {
            get { return m_MaxSymbolsCount; }
            set { m_MaxSymbolsCount = value; }
        }
        [XmlElementAttribute("SegmentCount")]
        public int SegmentCount
        {
            get { return m_SegmentCount; }
            set { m_SegmentCount = value; }
        }
        [XmlElementAttribute("DisplayNum")]
        public int DisplayNum
        {
            get { return m_DisplayNum; }
            set { m_DisplayNum = value; }
        }

        [XmlArray("HelpSegInfoArray")]
        [XmlArrayItem("HelperSegmentInfo")]
        public List<CyHelperSegmentInfo> HelpSegInfo;

        public int SymbolsCount
        {
            get { return m_SymbolsCount; }
            set
            {
                m_SymbolsCount = value;
            }
        }

        public HelperInfo()
        {
            HelpSegInfo = new List<CyHelperSegmentInfo>();

            Name = "Empty";
            m_Kind = CyHelperKind.Empty;
            SymbolsCount = 0;
            m_MaxSymbolsCount = 0;
            m_SegmentCount = 0;
        }

        public HelperInfo(string name, CyHelperKind kind, int maxSymbolsCount, int segmentCount,Color color, 
            int helperIndex)
        {
            Name = name;
            m_Kind = kind;
            m_MaxSymbolsCount = maxSymbolsCount;
            m_SegmentCount = segmentCount;
            SymbolsCount = 0;
            HelpSegInfo = new List<CyHelperSegmentInfo>();

            HelperColor = color;
            GlobalHelperIndex = helperIndex;
        }
        
        //---------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return Name;
        }

        public string GetPixelNameByCommonSegment(int common, int segment)
        {
            string name = "";
            for (int i = 0; i < HelpSegInfo.Count; i++)
            {
                if ((HelpSegInfo[i].Common == common) &&
                    (HelpSegInfo[i].Segment == segment))
                {
                    name = HelpSegInfo[i].Name;
                    break;
                }
            }
            
            return name;
        }

        public CySegmentInfo GetPixelByCommonSegment(int common, int segment)
        {
            CySegmentInfo pixel = null;
            for (int i = 0; i < HelpSegInfo.Count; i++)
            {
                if ((HelpSegInfo[i].Common == common) &&
                    (HelpSegInfo[i].Segment == segment))
                {
                    pixel = HelpSegInfo[i];
                    break;
                }
            }

            return pixel;
        }

        public CySegmentInfo GetPixelByName(string name)
        {
            CySegmentInfo pixel = null;
            for (int i = 0; i < HelpSegInfo.Count; i++)
            {
                if (HelpSegInfo[i].Name == name) 
                {
                    pixel = HelpSegInfo[i];
                    break;
                }
            }
            
            return pixel;
        }

        public CyHelperSegmentInfo GetPixelBySymbolSegment(int symbol, int segment)
        {
            CyHelperSegmentInfo pixel = null;
            for (int i = 0; i < HelpSegInfo.Count; i++)
            {
                if ((HelpSegInfo[i].DigitNum == symbol) &&
                    (HelpSegInfo[i].RelativePos == segment))
                {
                    pixel = HelpSegInfo[i];
                    break;
                }
            }
            return pixel;
        }

        public string GetDefaultSymbolName(int symbol)
        {
            string nameTemplate = "H*" + symbol + "_";
            string name = nameTemplate;
            switch (Kind)
            {
                case CyHelperKind.Segment7:
                    name = nameTemplate.Replace("*", "7seg");
                    break;
                case CyHelperKind.Segment14:
                    name = nameTemplate.Replace("*", "14seg");
                    break;
                case CyHelperKind.Segment16:
                    name = nameTemplate.Replace("*", "16seg");
                    break;
                case CyHelperKind.Bar:
                    name = "HBar";
                    break;
                case CyHelperKind.Matrix:
                    name = nameTemplate.Replace("*", "Dot");
                    break;
                default:
                    break;
            }
            return name.ToUpper();
        }

        public void AddSymbol(int symbolGlobalNum)
        {
            if ((SymbolsCount >= MaxSymbolsCount) || (Kind == CyHelperKind.Empty))
                return;

            string nameTemplate = GetDefaultSymbolName(symbolGlobalNum);
            string name = "SEG";
            char letter = 'A';
            for (int i = 0; i < SegmentCount; i++)
            {
                switch (Kind)
                {
                    case CyHelperKind.Segment7:
                        name = nameTemplate + ((char)('A' + i));
                        break;
                    case CyHelperKind.Segment14:
                        name = nameTemplate + ((char)('A' + i));
                        break;
                    case CyHelperKind.Segment16:
                        if ((i == 1))
                        {
                            name = nameTemplate +  "A_";
                        }
                        else if (i == 4)
                        {
                            name = nameTemplate + "D_";                            
                        }
                        else
                        {
                            name = nameTemplate + letter;
                            letter = (char)(letter + 1);
                        }
                        break;
                    case CyHelperKind.Bar:
                        name = nameTemplate + symbolGlobalNum;
                        break;
                    case CyHelperKind.Matrix:
                        name = nameTemplate + (i % 5) + (i / 5);
                        break;
                    default:
                        break;
                }
                int com = -1;
                int seg = -1;
                HelpSegInfo.Add(new CyHelperSegmentInfo(name.ToUpper(), com, seg, 0, SymbolsCount, i, Kind, 
                    symbolGlobalNum));
            }
            
            SymbolsCount++;

        }

        //---------------------------------------------------------------------------------------------------------
        #region Static functions

        public static void CreateHelper(CyHelperKind kind, CyLCDParameters parameters)
        {
            int num = AddNextHelperIndex(kind, parameters);
            HelperInfo helper;
            switch (kind)
            {
                case CyHelperKind.Segment7:
                    helper = new HelperInfo("Helper_7Segment_" + num, kind, MAX_7SEG_SYMBOLS, 7,
                        parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.Segment14:
                    helper = new HelperInfo("Helper_14Segment_" + num, kind, MAX_14SEG_SYMBOLS, 14, 
                        parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.Segment16:
                    helper = new HelperInfo("Helper_16Segment_" + num, kind, MAX_16SEG_SYMBOLS, 16, 
                        parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.Bar:
                    helper = new HelperInfo("Helper_Bar_" + num, kind, MAX_BAR_SYMBOLS, 1, 
                        parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.Matrix:
                    helper = new HelperInfo("Helper_Matrix_" + num, kind, MAX_MATRIX_SYMBOLS, 5 * 8, 
                        parameters.m_ColorChooser.PopCl(), num);
                    break;
                default:
                    helper = new HelperInfo();
                    break;
            }
            helper.DisplayNum = parameters.m_HelpersConfig.Count - 1;
            parameters.m_HelpersConfig.Add(helper);

            //Pixels management
            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            if (kind == CyHelperKind.Empty)
            {
                for (int j = 0; j < NumSegmentLines; j++)
                    for (int i = 0; i < NumCommonLines; i++)
                    {
                        helper.HelpSegInfo.Add(new CyHelperSegmentInfo("PIX" + (j*NumCommonLines + i), i, j, -1, -1, -1,
                                                                       kind, -1));
                    }
            }
        }

        //---------------------------------------------------------------------------------------------------------

        public static void UpdateEmptyHelper(CyLCDParameters parameters)
        {
            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            parameters.m_HelpersConfig[0].HelpSegInfo.Clear();
            for (int j = 0; j < NumSegmentLines; j++)
                for (int i = 0; i < NumCommonLines; i++)
                {
                    parameters.m_HelpersConfig[0].HelpSegInfo.Add(new CyHelperSegmentInfo(
                                                                      "PIX" + (j*NumCommonLines + i), i, j, -1, -1, -1,
                                                                      CyHelperKind.Empty, -1));
                }
        }

        //---------------------------------------------------------------------------------------------------------
        
        public static int GetTotalPixelNumber(CyLCDParameters parameters)
        {
            int totalPixels = 0;
            for (int i = 1; i < parameters.m_HelpersConfig.Count; i++)
            {
                totalPixels += parameters.m_HelpersConfig[i].HelpSegInfo.Count;
            }
            return totalPixels;
        }

        public static bool CheckPixelUniqueName(CyLCDParameters parameters, string name, int n)
        {
            // false - if there are matches
            int times = 0;
            for (int i = 0; i < parameters.m_HelpersConfig.Count; i++)
                for (int j = 0; j < parameters.m_HelpersConfig[i].HelpSegInfo.Count; j++)
                {
                    if (parameters.m_HelpersConfig[i].HelpSegInfo[j].Name.ToUpper() == name.ToUpper())
                    {
                        times++;
                        if (times > n)
                            return false;
                    }
                }
            return true;
        }

        public static bool CheckHelperUniqueName(CyLCDParameters parameters, string name)
        {
            // false - if there are matches
            int times = 0;
            for (int i = 1; i < parameters.m_HelpersConfig.Count; i++)
            {
                if (parameters.m_HelpersConfig[i].Name == name)
                {
                    times++;
                    if (times > 0)
                        return false;
                }
            }
            return true;
        }

        public static int AddNextHelperIndex(CyHelperKind kind, CyLCDParameters parameters)
        {
            int index = 0;
            switch (kind)
            {
                case CyHelperKind.Segment7:
                    while (parameters.m_HelperIndexes_7SEG.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_7SEG.Add(index);
                    break;
                case CyHelperKind.Segment14:
                    while (parameters.m_HelperIndexes_14SEG.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_14SEG.Add(index);
                    break;
                case CyHelperKind.Segment16:
                    while (parameters.m_HelperIndexes_16SEG.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_16SEG.Add(index);
                    break;
                case CyHelperKind.Bar:
                    while (parameters.m_HelperIndexes_BAR.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_BAR.Add(index);
                    break;
                case CyHelperKind.Matrix:
                    while (parameters.m_HelperIndexes_MATRIX.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_MATRIX.Add(index);
                    break;
                default:
                    break;
            }
            return index;
        }

        public static void RemoveHelperIndex(int index, CyHelperKind kind, CyLCDParameters parameters)
        {
            switch (kind)
            {
                case CyHelperKind.Segment7:
                    parameters.m_HelperIndexes_7SEG.Remove(index);
                    break;
                case CyHelperKind.Segment14:
                    parameters.m_HelperIndexes_14SEG.Remove(index);
                    break;
                case CyHelperKind.Segment16:
                    parameters.m_HelperIndexes_16SEG.Remove(index);
                    break;
                case CyHelperKind.Bar:
                    parameters.m_HelperIndexes_BAR.Remove(index);
                    break;
                case CyHelperKind.Matrix:
                    parameters.m_HelperIndexes_MATRIX.Remove(index);
                    break;
                default:
                    break;
            }
        }

        //---------------------------------------------------------------------------------------------------------

        public static string SerializeHelpers(List<HelperInfo> list)
        {
            ArrayList helpers = new ArrayList(list);
            Type[] theExtraTypes = new Type[2];
            theExtraTypes[0] = typeof(HelperInfo);
            theExtraTypes[1] = typeof(ArrayList);
            XmlSerializer s = new XmlSerializer(typeof(ArrayList), theExtraTypes);
            StringWriter sw = new StringWriter();
            s.Serialize(sw, helpers);
            string serializedXml = sw.ToString();
            return serializedXml;
        }

        public static List<HelperInfo> DeserializeHelpers(string serializedXml)
        {
            Type[] theExtraTypes = new Type[2];
            theExtraTypes[0] = typeof(HelperInfo);
            theExtraTypes[1] = typeof(ArrayList);
            XmlSerializer s = new XmlSerializer(typeof(ArrayList), theExtraTypes);
            ArrayList helpers = (ArrayList)s.Deserialize(new StringReader(serializedXml));
            List<HelperInfo> newList = new List<HelperInfo>();
            for (int i = 0; i < helpers.Count; i++)
            {
                newList.Add((HelperInfo) helpers[i]);
            }
            return newList;
        }

        #endregion Static functions
    }

    #endregion Helper classes

    #region CyColorList Class

    public class CyColorList
    {
        readonly List<Color> clArxiv = new List<Color>();
        public CyColorList()
        {
            clArxiv.Add(Color.FromArgb(226,147,147));            
            clArxiv.Add(Color.FromArgb(226,200,147));
            clArxiv.Add(Color.FromArgb(200,226,147));
            clArxiv.Add(Color.FromArgb(147,226,200));
            clArxiv.Add(Color.FromArgb(147,200,226));
            clArxiv.Add(Color.FromArgb(147,147,226));
            clArxiv.Add(Color.FromArgb(200,147,226));
            clArxiv.Add(Color.FromArgb(226,147,200));
        }
        public void PushCl(Color clp)
        {
            if (clp != Color.MintCream)
            {
                clArxiv.Add(clp);
            }
        }
        public Color PopCl()
        {
            Color res = Color.MintCream;
            if (clArxiv.Count > 0)
            {
                res = clArxiv[clArxiv.Count - 1];
                clArxiv.Remove(res);
            }
            return res;
        }

        public void PopCl(Color clp)
        {
            clArxiv.Remove(clp);
        }
    }
    #endregion
}
