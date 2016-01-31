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


namespace SegLCD_v1_0
{
    public class LCDParameters
    {
        #region Variables

        readonly ICyInstEdit_v1 inst;

        // *Parameters:
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

        private byte _NumCommonLines = 4;
        private byte _NumSegmentLines = 8;
        private byte _BiasType = 0;
        private byte _WaveformType = 0;
        private byte _FrameRate = 0;
        private byte _BiasVoltage = 0;

        private byte _DriverPowerMode = 0;
        private byte _HiDriveTime = 0;
        private byte _LowDriveMode = 0;
        private byte _LowDriveInitTime = 0;
        private byte _LowDriveDutyCycleTime = 0;

        private bool _UseInternalClock = false;
        private uint _ClockFrequency = 800;
        private byte _DacDisInitTime = 0;
        private bool _EnableInterrupt = false;

        private bool _Gang = false;
        private bool _DebugMode;

        public List<HelperInfo> HelpersConfig;
        private String _SerializedHelpers;
        private string _DisabledCommons;

        public ColorList colorChooser = new ColorList();

        public List<int> SymbolIndexes_7SEG = new List<int>();
        public List<int> SymbolIndexes_14SEG = new List<int>();
        public List<int> SymbolIndexes_16SEG = new List<int>();
        public List<int> SymbolIndexes_BAR = new List<int>();
        public List<int> SymbolIndexes_MATRIX = new List<int>();

        public List<int> HelperIndexes_7SEG = new List<int>();
        public List<int> HelperIndexes_14SEG = new List<int>();
        public List<int> HelperIndexes_16SEG = new List<int>();
        public List<int> HelperIndexes_BAR = new List<int>();
        public List<int> HelperIndexes_MATRIX = new List<int>();

        #endregion Variables

        #region Properties

        public byte NumCommonLines
        {
            get {return _NumCommonLines;}
            set
            {
                if (value != _NumCommonLines)
                {
                    _NumCommonLines = value;
                    SetParam("NumCommonLines");
                    CommitParams();
                }
            }
        }
        public byte NumSegmentLines
        {
            get { return _NumSegmentLines; }
            set
            {
                if (value != _NumSegmentLines)
                {
                    _NumSegmentLines = value;
                    SetParam("NumSegmentLines");
                    CommitParams();
                }
            }
        }
        public byte BiasType
        {
            get { return _BiasType; }
            set
            {
                if (value != _BiasType)
                {
                    _BiasType = value;
                    SetParam("BiasType");
                    CommitParams();
                }
            }
        }
        public byte WaveformType
        {
            get { return _WaveformType; }
            set
            {
                if (value != _WaveformType)
                {
                    _WaveformType = value;
                    SetParam("WaveformType");
                    CommitParams();
                }
            }
        }
        public byte FrameRate
        {
            get { return _FrameRate; }
            set
            {
                if (value != _FrameRate)
                {
                    _FrameRate = value;
                    SetParam("FrameRate");
                    CommitParams();
                }
            }
        }
        public byte BiasVoltage
        {
            get { return _BiasVoltage; }
            set
            {
                if (value != _BiasVoltage)
                {
                    _BiasVoltage = value;
                    SetParam("BiasVoltage");
                    CommitParams();
                }
            }
        }

        public byte DriverPowerMode
        {
            get { return _DriverPowerMode; }
            set
            {
                if (value != _DriverPowerMode)
                {
                    _DriverPowerMode = value;
                    SetParam("DriverPowerMode");
                    CommitParams();
                }
            }
        }
        
        public byte HiDriveTime
        {
            get { return _HiDriveTime; }
            set
            {
                if (value != _HiDriveTime)
                {
                    _HiDriveTime = value;
                    SetParam("HiDriveTime");
                    CommitParams();
                }
            }
        }
        public byte LowDriveMode
        {
            get { return _LowDriveMode; }
            set
            {
                if (value != _LowDriveMode)
                {
                    _LowDriveMode = value;
                    SetParam("LowDriveMode");
                    CommitParams();
                }
            }
        }
        public byte LowDriveInitTime
        {
            get { return _LowDriveInitTime; }
            set
            {
                if (value != _LowDriveInitTime)
                {
                    _LowDriveInitTime = value;
                    SetParam("LowDriveInitTime");
                    CommitParams();
                }
            }
        }
        public byte LowDriveDutyCycleTime
        {
            get { return _LowDriveDutyCycleTime; }
            set
            {
                if (value != _LowDriveDutyCycleTime)
                {
                    _LowDriveDutyCycleTime = value;
                    SetParam("LowDriveDutyCycleTime");
                    CommitParams();
                }
            }
        }

        public bool UseInternalClock
        {
            get { return _UseInternalClock; }
            set
            {
                if (value != _UseInternalClock)
                {
                    _UseInternalClock = value;
                    SetParam("UseInternalClock");
                    CommitParams();
                }
            }
        }

        public uint ClockFrequency
        {
            get { return _ClockFrequency; }
            set
            {
                if (value != _ClockFrequency)
                {
                    _ClockFrequency = value;
                    SetParam("ClockFrequency");

                    _DacDisInitTime = Convert.ToByte(Math.Ceiling(_ClockFrequency/Math.Pow(10, 5)));
                    if (_DacDisInitTime == 0)
                        _DacDisInitTime = 1;
                    SetParam("DacDisInitTime");

                    CommitParams();
                }
            }
        }

        public bool EnableInterrupt
        {
            get { return _EnableInterrupt; }
            set
            {
                if (value != _EnableInterrupt)
                {
                    _EnableInterrupt = value;
                    SetParam("EnableInterrupt");
                    CommitParams();
                }
            }
        }

        public bool Gang
        {
            get { return _Gang; }
            set
            {
                if (value != _Gang)
                {
                    _Gang = value;
                    SetParam("Gang");
                    CommitParams();
                }
            }
        }

        public bool DebugMode
        {
            get { return _DebugMode; }
            set
            {
                if (value != _DebugMode)
                {
                    _DebugMode = value;
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
                if (_DisabledCommons != "")
                {
                    string[] nums = _DisabledCommons.Split(',');
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
                if (res != _DisabledCommons)
                {
                    _DisabledCommons = res;
                    SetParam("DisabledCommons");
                    CommitParams();
                }
            }
        }

        public string SerializedHelpers
        {
            get { return _SerializedHelpers; }
            set
            {
                if (value != _SerializedHelpers)
                {
                    _SerializedHelpers = value;
                    _SerializedHelpers = _SerializedHelpers.Replace("\r\n", " ");
                    SetParam("Helpers");
                    CommitParams();
                }
            }
        }

        #endregion Properties

        #region Constructors

        public LCDParameters()
        {
            HelpersConfig = new List<HelperInfo>();
            DisabledCommons = new List<int>();
            HelperInfo.CreateHelper(HelperKind.Empty, this);
        }

        public LCDParameters(ICyInstEdit_v1 inst)
        {
            DisabledCommons = new List<int>();
            if (inst != null)
            {
                this.inst = inst;
                GetParams();
            }
        }

        #endregion Constructors

        #region Common

        private void GetParams()
        {
            if (inst != null)
            {
                _NumCommonLines = Convert.ToByte(inst.GetCommittedParam("NumCommonLines").Value);
                _NumSegmentLines = Convert.ToByte(inst.GetCommittedParam("NumSegmentLines").Value);
                _BiasType = Convert.ToByte(inst.GetCommittedParam("BiasType").Value);
                _WaveformType = Convert.ToByte(inst.GetCommittedParam("WaveformType").Value);
                _FrameRate = Convert.ToByte(inst.GetCommittedParam("FrameRate").Value);
                _BiasVoltage = Convert.ToByte(inst.GetCommittedParam("BiasVoltage").Value);

                _DriverPowerMode = Convert.ToByte(inst.GetCommittedParam("DriverPowerMode").Value);
                _HiDriveTime = Convert.ToByte(inst.GetCommittedParam("HiDriveTime").Value);
                _LowDriveMode = Convert.ToByte(inst.GetCommittedParam("LowDriveMode").Value);
                _LowDriveInitTime = Convert.ToByte(inst.GetCommittedParam("LowDriveInitTime").Value);
                _LowDriveDutyCycleTime = Convert.ToByte(inst.GetCommittedParam("LowDriveDutyCycleTime").Value);

                _UseInternalClock = Convert.ToBoolean(inst.GetCommittedParam("UseInternalClock").Value);
                _ClockFrequency = Convert.ToUInt32(inst.GetCommittedParam("ClockFrequency").Value);
                _EnableInterrupt = Convert.ToBoolean(inst.GetCommittedParam("EnableInterrupt").Value);
                _Gang = Convert.ToBoolean(inst.GetCommittedParam("Gang").Value);
                _DebugMode = Convert.ToBoolean(inst.GetCommittedParam("DebugMode").Value);
                _DisabledCommons = Convert.ToString(inst.GetCommittedParam("DisabledCommons").Value);
                M_DeserializeHelpers(inst.GetCommittedParam("Helpers").Value);

            }
        }
        public void M_DeserializeHelpers(string _SHelpers)
        {
            try
            {
                if ((_SHelpers != null) && (_SHelpers != ""))
                {
                    HelpersConfig = HelperInfo.DeserializeHelpers(_SHelpers);
                    // Add used helper and symbol indexes to the list
                    for (int i = 0; i < HelpersConfig.Count; i++)
                    {
                        switch (HelpersConfig[i].Kind)
                        {
                            case HelperKind.Segment7:
                                HelperIndexes_7SEG.Add(HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!SymbolIndexes_7SEG.Contains(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        SymbolIndexes_7SEG.Add(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            case HelperKind.Segment14:
                                HelperIndexes_14SEG.Add(HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!SymbolIndexes_14SEG.Contains(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        SymbolIndexes_14SEG.Add(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            case HelperKind.Segment16:
                                HelperIndexes_16SEG.Add(HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!SymbolIndexes_16SEG.Contains(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        SymbolIndexes_16SEG.Add(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            case HelperKind.Bar:
                                HelperIndexes_BAR.Add(HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!SymbolIndexes_BAR.Contains(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        SymbolIndexes_BAR.Add(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            case HelperKind.Matrix:
                                HelperIndexes_MATRIX.Add(HelpersConfig[i].GlobalHelperIndex);
                                for (int j = 0; j < HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!SymbolIndexes_MATRIX.Contains(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum))
                                        SymbolIndexes_MATRIX.Add(HelpersConfig[i].HelpSegInfo[j].GlobalDigitNum);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    HelpersConfig = new List<HelperInfo>();
                    HelperInfo.CreateHelper(HelperKind.Empty, this);
                }
            }
            catch
            {
                MessageBox.Show("Error in Helpers parameter", "SegLCD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetParam(string paramName)
        {
            if (inst != null)
            {
                switch (paramName)
                {
                    case "NumCommonLines":
                        inst.SetParamExpr("NumCommonLines", NumCommonLines.ToString());
                        break;
                    case "NumSegmentLines":
                        inst.SetParamExpr("NumSegmentLines", NumSegmentLines.ToString());
                        break;
                    case "BiasType":
                        inst.SetParamExpr("BiasType", BiasType.ToString());
                        break;
                    case "WaveformType":
                        inst.SetParamExpr("WaveformType", WaveformType.ToString());
                        break;
                    case "FrameRate":
                        inst.SetParamExpr("FrameRate", FrameRate.ToString());
                        break;
                    case "BiasVoltage":
                        inst.SetParamExpr("BiasVoltage", BiasVoltage.ToString());
                        break;
                    case "DriverPowerMode":
                        inst.SetParamExpr("DriverPowerMode", DriverPowerMode.ToString());
                        break;
                    case "HiDriveTime":
                        inst.SetParamExpr("HiDriveTime", HiDriveTime.ToString());
                        break;
                    case "LowDriveMode":
                        inst.SetParamExpr("LowDriveMode", LowDriveMode.ToString());
                        break;
                    case "LowDriveInitTime":
                        inst.SetParamExpr("LowDriveInitTime", LowDriveInitTime.ToString());
                        break;
                    case "LowDriveDutyCycleTime":
                        inst.SetParamExpr("LowDriveDutyCycleTime", LowDriveDutyCycleTime.ToString());
                        break;
                    case "UseInternalClock":
                        inst.SetParamExpr("UseInternalClock", UseInternalClock.ToString());
                        break;
                    case "ClockFrequency":
                        inst.SetParamExpr("ClockFrequency", ClockFrequency.ToString());
                        break;
                    case "DacDisInitTime":
                        inst.SetParamExpr("DacDisInitTime", _DacDisInitTime.ToString());
                        break;
                    case "EnableInterrupt":
                        inst.SetParamExpr("EnableInterrupt", EnableInterrupt.ToString());
                        break;
                    case "Gang":
                        inst.SetParamExpr("Gang", Gang.ToString().ToLower());
                        break;
                    case "DebugMode":
                        inst.SetParamExpr("DebugMode", DebugMode.ToString().ToLower());
                        break;
                    case "DisabledCommons":
                        inst.SetParamExpr("DisabledCommons", _DisabledCommons);
                        break;
                    case "Helpers":
                        inst.SetParamExpr("Helpers", SerializedHelpers);
                        break;

                    default:
                        break;
                }
            }
        }

        public void SetParams()
        {
            if (inst != null)
            {
                inst.SetParamExpr("NumCommonLines", NumCommonLines.ToString());
                inst.SetParamExpr("NumSegmentLines", NumSegmentLines.ToString());
                inst.SetParamExpr("BiasType", BiasType.ToString());
                inst.SetParamExpr("WaveformType", WaveformType.ToString());
                inst.SetParamExpr("FrameRate", FrameRate.ToString());
                inst.SetParamExpr("BiasVoltage", BiasVoltage.ToString());

                inst.SetParamExpr("DriverPowerMode", DriverPowerMode.ToString());
                inst.SetParamExpr("HiDriveTime", HiDriveTime.ToString());
                inst.SetParamExpr("LowDriveMode", LowDriveMode.ToString());
                inst.SetParamExpr("LowDriveInitTime", LowDriveInitTime.ToString());
                inst.SetParamExpr("LowDriveDutyCycleTime", LowDriveDutyCycleTime.ToString());
                inst.SetParamExpr("UseInternalClock", UseInternalClock.ToString());
                inst.SetParamExpr("ClockFrequency", ClockFrequency.ToString());
                inst.SetParamExpr("DacDisInitTime", _DacDisInitTime.ToString());
                inst.SetParamExpr("EnableInterrupt", EnableInterrupt.ToString());
                inst.SetParamExpr("Gang", Gang.ToString().ToLower());
                inst.SetParamExpr("DebugMode", DebugMode.ToString().ToLower());

                inst.SetParamExpr("DisabledCommons", _DisabledCommons);
                inst.SetParamExpr("Helpers", SerializedHelpers);
            }
        }

        public void CommitParams()
        {
            if (inst != null)
            {
                if (!inst.CommitParamExprs())
                {
                    /*if (inst.GetCommittedParam("NumCommonLines").ErrorCount > 0)
                        MessageBox.Show("Error in Committing NumCommonLines");
                    if (inst.GetCommittedParam("NumSegmentLines").ErrorCount > 0)
                        MessageBox.Show("Error in Committing NumSegmentLines");
                    if (inst.GetCommittedParam("FrameBufferMemoryModel").ErrorCount > 0)
                        MessageBox.Show("Error in Committing FrameBufferMemoryModel");
                    if (inst.GetCommittedParam("BiasType").ErrorCount > 0)
                        MessageBox.Show("Error in Committing BiasType");
                    if (inst.GetCommittedParam("WaveformType").ErrorCount > 0)
                        MessageBox.Show("Error in Committing WaveformType");
                    if (inst.GetCommittedParam("FrameRate").ErrorCount > 0)
                        MessageBox.Show("Error in Committing FrameRate");
                    if (inst.GetCommittedParam("BiasVoltage").ErrorCount > 0)
                        MessageBox.Show("Error in Committing BiasVoltage");

                    if (inst.GetCommittedParam("DriverPowerMode").ErrorCount > 0)
                        MessageBox.Show("Error in Committing DriverPowerMode");
                    if (inst.GetCommittedParam("HiDriveTime").ErrorCount > 0)
                        MessageBox.Show("Error in Committing HiDriveTime");
                    if (inst.GetCommittedParam("LowDriveMode").ErrorCount > 0)
                        MessageBox.Show("Error in Committing LowDriveMode");
                    if (inst.GetCommittedParam("LowDriveInitTime").ErrorCount > 0)
                        MessageBox.Show("Error in Committing LowDriveInitTime");
                    if (inst.GetCommittedParam("LowDriveDutyCycleTime").ErrorCount > 0)
                        MessageBox.Show("Error in Committing LowDriveDutyCycleTime");
                    if (inst.GetCommittedParam("UseInternalClock").ErrorCount > 0)
                        MessageBox.Show("Error in Committing UseInternalClock");
                    if (inst.GetCommittedParam("ClockFrequency").ErrorCount > 0)
                        MessageBox.Show("Error in Committing ClockFrequency");
                    if (inst.GetCommittedParam("EnableInterrupt").ErrorCount > 0)
                        MessageBox.Show("Error in Committing EnableInterrupt");*/
                }
            }
        }

        #endregion Common
    }


    #region Helper classes

    public class SegmentInfo
    {
        private string _Name;
        private int _Common;
        private int _Segment;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [XmlAttribute("Com")]
        public int Common
        {
            get { return _Common; }
            set { _Common = value; }
        }
        [XmlAttribute("Seg")]
        public int Segment
        {
            get { return _Segment; }
            set { _Segment = value; }
        }

        public SegmentInfo()
        {
            Name = "SEG";
        }

        public SegmentInfo(string name, int common, int segment)
        {
            Name = name;
            Common = common;
            Segment = segment;
        }
    }

    [XmlRootAttribute("Pixel")]
    public class HelperSegmentInfo : SegmentInfo
    {
        [XmlAttribute("Display")]
        public int DisplayNum;
        [XmlAttribute("Digit")]
        public int DigitNum;
        [XmlAttribute("Helper")]
        public HelperKind HelperType;
        [XmlAttribute("RelPos")]
        public int RelativePos;

        public int GlobalDigitNum;

        public HelperSegmentInfo()
        {
            HelperType = HelperKind.Empty;
        }

        public HelperSegmentInfo(string name, int common, int segment, int displayNum, int digitNum, int relPos, HelperKind type, int globalDigitNum)
            : base(name, common, segment)
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
                case HelperKind.Segment7:
                    return 8;
                case HelperKind.Segment14:
                    return 14;
                case HelperKind.Segment16:
                    return 16;
                case HelperKind.Bar:
                    return 1;
                case HelperKind.Matrix:
                    return 40;
                case HelperKind.Empty:
                    break;
                default:
                    break;
            }
            return 0;
        }
    }
    
    [XmlRootAttribute("Helper")]
    public class HelperInfo
    {
        public const int MAX_7SEG_SYMBOLS = 5;
        public const int MAX_14SEG_SYMBOLS = 20;
        public const int MAX_16SEG_SYMBOLS = 20;
        public const int MAX_BAR_SYMBOLS = 255;
        public const int MAX_MATRIX_SYMBOLS = 8;

        public string Name;
        private HelperKind _Kind;
        private int _SymbolsCount;
        private int _MaxSymbolsCount;
        private int _SegmentCount;
        private int _DisplayNum;
        [XmlElementAttribute("Color")]
        public int _HelperColor;

        //public static List<int> SymbolIndexes_7SEG = new List<int>();
        //public static List<int> SymbolIndexes_14SEG = new List<int>();
        //public static List<int> SymbolIndexes_16SEG = new List<int>();
        //public static List<int> SymbolIndexes_BAR = new List<int>();
        //public static List<int> SymbolIndexes_MATRIX = new List<int>();

        //public static List<int> HelperIndexes_7SEG = new List<int>();
        //public static List<int> HelperIndexes_14SEG = new List<int>();
        //public static List<int> HelperIndexes_16SEG = new List<int>();
        //public static List<int> HelperIndexes_BAR = new List<int>();
        //public static List<int> HelperIndexes_MATRIX = new List<int>();

        public int GlobalHelperIndex;

        [XmlIgnore]
        public Color HelperColor
        {
            get { return Color.FromArgb(_HelperColor); }
            set { _HelperColor = value.ToArgb(); }
        }
        [XmlElementAttribute("Kind")]
        public HelperKind Kind
        {
            get { return _Kind; }
            set { _Kind = value; }
        }
        [XmlElementAttribute("MaxSymbolsCount")]
        public int MaxSymbolsCount
        {
            get { return _MaxSymbolsCount; }
            set { _MaxSymbolsCount = value; }
        }
        [XmlElementAttribute("SegmentCount")]
        public int SegmentCount
        {
            get { return _SegmentCount; }
            set { _SegmentCount = value; }
        }
        [XmlElementAttribute("DisplayNum")]
        public int DisplayNum
        {
            get { return _DisplayNum; }
            set { _DisplayNum = value; }
        }

        [XmlArray("HelpSegInfoArray")]
        public List<HelperSegmentInfo> HelpSegInfo;

        public int SymbolsCount
        {
            get { return _SymbolsCount; }
            set
            {
                _SymbolsCount = value;
            }
        }

        public HelperInfo()
        {
            HelpSegInfo = new List<HelperSegmentInfo>();

            Name = "Empty";
            _Kind = HelperKind.Empty;
            SymbolsCount = 0;
            _MaxSymbolsCount = 0;
            _SegmentCount = 0;
        }

        public HelperInfo(string name, HelperKind kind, int maxSymbolsCount, int segmentCount,Color color, int helperIndex)
        {
            Name = name;
            _Kind = kind;
            _MaxSymbolsCount = maxSymbolsCount;
            _SegmentCount = segmentCount;
            SymbolsCount = 0;
            HelpSegInfo = new List<HelperSegmentInfo>();

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

        public SegmentInfo GetPixelByCommonSegment(int common, int segment)
        {
            SegmentInfo pixel = null;
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

        public SegmentInfo GetPixelByName(string name)
        {
            SegmentInfo pixel = null;
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

        public HelperSegmentInfo GetPixelBySymbolSegment(int symbol, int segment)
        {
            HelperSegmentInfo pixel = null;
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
                case HelperKind.Segment7:
                    name = nameTemplate.Replace("*", "7seg");
                    break;
                case HelperKind.Segment14:
                    name = nameTemplate.Replace("*", "14seg");
                    break;
                case HelperKind.Segment16:
                    name = nameTemplate.Replace("*", "16seg");
                    break;
                case HelperKind.Bar:
                    name = "HBar";
                    break;
                case HelperKind.Matrix:
                    name = nameTemplate.Replace("*", "Dot");
                    break;
                default:
                    break;
            }
            return name.ToUpper();
        }

        public void AddSymbol(int symbolGlobalNum)
        {
            if ((SymbolsCount >= MaxSymbolsCount) || (Kind == HelperKind.Empty))
                return;

            string nameTemplate = GetDefaultSymbolName(symbolGlobalNum);
            string name = "SEG";
            char letter = 'A';
            for (int i = 0; i < SegmentCount; i++)
            {
                switch (Kind)
                {
                    case HelperKind.Segment7:
                        name = nameTemplate + ((char)('A' + i));
                        break;
                    case HelperKind.Segment14:
                        name = nameTemplate + ((char)('A' + i));
                        break;
                    case HelperKind.Segment16:
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
                    case HelperKind.Bar:
                        name = nameTemplate + symbolGlobalNum;
                        break;
                    case HelperKind.Matrix:
                        name = nameTemplate + (i % 5) + (i / 5);
                        break;
                    default:
                        break;
                }
                int com = -1;
                int seg = -1;
                HelpSegInfo.Add(new HelperSegmentInfo(name.ToUpper(), com, seg, 0, SymbolsCount, i, Kind, symbolGlobalNum));
            }
            
            SymbolsCount++;

        }

        //---------------------------------------------------------------------------------------------------------
        #region Static functions

        public static void CreateHelper(HelperKind kind, LCDParameters parameters)
        {
            int num = AddNextHelperIndex(kind, parameters);
            HelperInfo helper;
            switch (kind)
            {
                case HelperKind.Segment7:
                    helper = new HelperInfo("Helper_7Segment_" + num, kind, MAX_7SEG_SYMBOLS, 8,parameters.colorChooser.PopCl(), num);
                    break;
                case HelperKind.Segment14:
                    helper = new HelperInfo("Helper_14Segment_" + num, kind, MAX_14SEG_SYMBOLS, 14, parameters.colorChooser.PopCl(), num);
                    break;
                case HelperKind.Segment16:
                    helper = new HelperInfo("Helper_16Segment_" + num, kind, MAX_16SEG_SYMBOLS, 16, parameters.colorChooser.PopCl(), num);
                    break;
                case HelperKind.Bar:
                    helper = new HelperInfo("Helper_Bar_" + num, kind, MAX_BAR_SYMBOLS, 1, parameters.colorChooser.PopCl(), num);
                    break;
                case HelperKind.Matrix:
                    helper = new HelperInfo("Helper_Matrix_" + num, kind, MAX_MATRIX_SYMBOLS, 5 * 8, parameters.colorChooser.PopCl(), num);
                    break;
                default:
                    helper = new HelperInfo();
                    break;
            }
            helper.DisplayNum = parameters.HelpersConfig.Count - 1;
            parameters.HelpersConfig.Add(helper);

            //Pixels management
            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            if (kind == HelperKind.Empty)
            {
                for (int j = 0; j < NumSegmentLines; j++)
                    for (int i = 0; i < NumCommonLines; i++)
                    {
                        helper.HelpSegInfo.Add(new HelperSegmentInfo("SEG" + (j * NumCommonLines + i), i, j, -1, -1,-1, kind, -1));
                    }
            }
        }

        //---------------------------------------------------------------------------------------------------------

        public static void UpdateEmptyHelper(LCDParameters parameters)
        {

            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            parameters.HelpersConfig[0].HelpSegInfo.Clear();
            for (int j = 0; j < NumSegmentLines; j++)
                for (int i = 0; i < NumCommonLines; i++)
                {
                    parameters.HelpersConfig[0].HelpSegInfo.Add(new HelperSegmentInfo("SEG" + (j * NumCommonLines + i), i, j, -1, -1, -1, HelperKind.Empty, -1));
                }
        }

        //---------------------------------------------------------------------------------------------------------
        
        public static int GetTotalPixelNumber(LCDParameters parameters)
        {
            int totalPixels = 0;
            for (int i = 1; i < parameters.HelpersConfig.Count; i++)
            {
                totalPixels += parameters.HelpersConfig[i].HelpSegInfo.Count;
            }
            return totalPixels;
        }

        public static bool CheckPixelUniqueName(LCDParameters parameters, string name)
        {
            // false - if there are matches
            int times = 0;
            for (int i = 0; i < parameters.HelpersConfig.Count; i++)
                for (int j = 0; j < parameters.HelpersConfig[i].HelpSegInfo.Count; j++)
                {
                    if (parameters.HelpersConfig[i].HelpSegInfo[j].Name == name)
                    {
                        times++;
                        if (times > 1)
                            return false;
                    }
                }
            return true;
        }

        public static bool CheckHelperUniqueName(LCDParameters parameters, string name)
        {
            // false - if there are matches
            int times = 0;
            for (int i = 1; i < parameters.HelpersConfig.Count; i++)
            {
                if (parameters.HelpersConfig[i].Name == name)
                {
                    times++;
                    if (times > 0)
                        return false;
                }
            }
            return true;
        }

        public static int AddNextHelperIndex(HelperKind kind, LCDParameters parameters)
        {
            int index = 0;
            switch (kind)
            {
                case HelperKind.Segment7:
                    while (parameters.HelperIndexes_7SEG.Contains(index))
                        index++;
                    parameters.HelperIndexes_7SEG.Add(index);
                    break;
                case HelperKind.Segment14:
                    while (parameters.HelperIndexes_14SEG.Contains(index))
                        index++;
                    parameters.HelperIndexes_14SEG.Add(index);
                    break;
                case HelperKind.Segment16:
                    while (parameters.HelperIndexes_16SEG.Contains(index))
                        index++;
                    parameters.HelperIndexes_16SEG.Add(index);
                    break;
                case HelperKind.Bar:
                    while (parameters.HelperIndexes_BAR.Contains(index))
                        index++;
                    parameters.HelperIndexes_BAR.Add(index);
                    break;
                case HelperKind.Matrix:
                    while (parameters.HelperIndexes_MATRIX.Contains(index))
                        index++;
                    parameters.HelperIndexes_MATRIX.Add(index);
                    break;
                default:
                    break;
            }
            return index;
        }

        public static void RemoveHelperIndex(int index, HelperKind kind, LCDParameters parameters)
        {
            switch (kind)
            {
                case HelperKind.Segment7:
                    parameters.HelperIndexes_7SEG.Remove(index);
                    break;
                case HelperKind.Segment14:
                    parameters.HelperIndexes_14SEG.Remove(index);
                    break;
                case HelperKind.Segment16:
                    parameters.HelperIndexes_16SEG.Remove(index);
                    break;
                case HelperKind.Bar:
                    parameters.HelperIndexes_BAR.Remove(index);
                    break;
                case HelperKind.Matrix:
                    parameters.HelperIndexes_MATRIX.Remove(index);
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

    #region ColorList Class

    public class ColorList
    {
        readonly List<Color> clArxiv = new List<Color>();
        public ColorList()
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
