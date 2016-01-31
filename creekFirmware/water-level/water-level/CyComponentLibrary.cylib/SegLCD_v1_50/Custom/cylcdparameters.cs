/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using CyDesigner.Extensions.Gde;


namespace SegLCD_v1_50
{
    public class CyLCDParameters
    {
        #region Constants

        public const string ERROR_MSG_TITLE = "Error";
        public const string WARNING_MSG_TITLE = "Warning";
        public const string INFORMATION_MSG_TITLE = "Information";
        public const string QUESTION_MSG_TITLE = "Question";

        // Parameters
        public const string PARAM_NUMCOMMONLINES = "NumCommonLines";
        public const string PARAM_NUMSEGMENTLINES = "NumSegmentLines";
        public const string PARAM_BIASTYPE = "BiasType";
        public const string PARAM_WAVEFORMTYPE = "WaveformType";
        public const string PARAM_FRAMERATE = "FrameRate";
        public const string PARAM_BIASVOLTAGE = "BiasVoltage";
        public const string PARAM_DRIVERPOWERMODE = "DriverPowerMode";
        public const string PARAM_HIDRIVETIME = "HiDriveTime";
        public const string PARAM_LOWDRIVEMODE = "LowDriveMode";
        public const string PARAM_LOWDRIVEINITTIME = "LowDriveInitTime";
        public const string PARAM_CLOCKFREQUENCY = "ClockFrequency";
        public const string PARAM_DACDISINITTIME = "DacDisInitTime";
        public const string PARAM_GANG = "Gang";
        public const string PARAM_DEBUGMODE = "DebugMode";
        public const string PARAM_DISABLEDCOMMONS = "DisabledCommons";
        public const string PARAM_HELPERS = "Helpers";
        public const string PARAM_CUSTOMCHARSLIST = "CustomCharsList";

        public const string UNASSIGNED_PIXEL_NAME = "PIX";

        #endregion Constants

        #region Variables

        public readonly ICyInstEdit_v1 m_inst;

        private byte m_numCommonLines = 4;
        private byte m_numSegmentLines = 8;
        private byte m_biasType;
        private byte m_waveformType;
        private byte m_frameRate;
        private byte m_biasVoltage;
        private byte m_driverPowerMode;
        private byte m_hiDriveTime;
        private byte m_lowDriveMode;
        private byte m_lowDriveInitTime;
        private uint m_clockFrequency = 800;
        public byte m_dacDisInitTime;
        private bool m_gang;
        private bool m_debugMode;

        public List<CyHelperInfo> m_helpersConfig;
        private String m_serializedHelpers;

        public CyColorList m_colorChooser = new CyColorList();
        public XmlSerializer m_serializer;
        public XmlSerializerNamespaces m_customSerNamespace;

        public List<int> m_symbolIndexes_7SEG = new List<int>();
        public List<int> m_symbolIndexes_14SEG = new List<int>();
        public List<int> m_symbolIndexes_16SEG = new List<int>();
        public List<int> m_symbolIndexes_BAR = new List<int>();
        public List<int> m_symbolIndexes_MATRIX = new List<int>();

        public List<int> m_helperIndexes_7SEG = new List<int>();
        public List<int> m_helperIndexes_14SEG = new List<int>();
        public List<int> m_helperIndexes_16SEG = new List<int>();
        public List<int> m_helperIndexes_BAR = new List<int>();
        public List<int> m_helperIndexes_MATRIX = new List<int>();

        public bool m_globalEditMode = false;

        //Tabs
        public CyBasicConfiguration m_cyBasicConfigurationTab;
        public CyDriverParams m_cyDriverParamsTab;
        public CyHelpers m_cyHelpersTab;

        #endregion Variables

        #region Properties

        public byte NumCommonLines
        {
            get { return m_numCommonLines; }
            set
            {
                if (value != m_numCommonLines)
                {
                    m_numCommonLines = value;
                    SetParam(PARAM_NUMCOMMONLINES);
                    CommitParams();
                }
            }
        }

        public byte NumSegmentLines
        {
            get { return m_numSegmentLines; }
            set
            {
                if (value != m_numSegmentLines)
                {
                    m_numSegmentLines = value;
                    SetParam(PARAM_NUMSEGMENTLINES);
                    CommitParams();
                }
            }
        }

        public byte BiasType
        {
            get { return m_biasType; }
            set
            {
                if (value != m_biasType)
                {
                    m_biasType = value;
                    SetParam(PARAM_BIASTYPE);
                    CommitParams();
                }
            }
        }

        public byte WaveformType
        {
            get { return m_waveformType; }
            set
            {
                if (value != m_waveformType)
                {
                    m_waveformType = value;
                    SetParam(PARAM_WAVEFORMTYPE);
                    CommitParams();
                }
            }
        }

        public byte FrameRate
        {
            get { return m_frameRate; }
            set
            {
                if (value != m_frameRate)
                {
                    m_frameRate = value;
                    SetParam(PARAM_FRAMERATE);
                    CommitParams();
                }
            }
        }

        public byte BiasVoltage
        {
            get { return m_biasVoltage; }
            set
            {
                if (value != m_biasVoltage)
                {
                    m_biasVoltage = value;
                    SetParam(PARAM_BIASVOLTAGE);
                    CommitParams();
                }
            }
        }

        public byte DriverPowerMode
        {
            get { return m_driverPowerMode; }
            set
            {
                if (value != m_driverPowerMode)
                {
                    m_driverPowerMode = value;
                    SetParam(PARAM_DRIVERPOWERMODE);
                    CommitParams();
                }
            }
        }

        public byte HiDriveTime
        {
            get { return m_hiDriveTime; }
            set
            {
                if (value != m_hiDriveTime)
                {
                    m_hiDriveTime = value;
                    SetParam(PARAM_HIDRIVETIME);
                    CommitParams();
                }
            }
        }

        public byte LowDriveMode
        {
            get { return m_lowDriveMode; }
            set
            {
                if (value != m_lowDriveMode)
                {
                    m_lowDriveMode = value;
                    SetParam(PARAM_LOWDRIVEMODE);
                    CommitParams();
                }
            }
        }

        public byte LowDriveInitTime
        {
            get { return m_lowDriveInitTime; }
            set
            {
                if (value != m_lowDriveInitTime)
                {
                    m_lowDriveInitTime = value;
                    SetParam(PARAM_LOWDRIVEINITTIME);
                    CommitParams();
                }
            }
        }

        public uint ClockFrequency
        {
            get { return m_clockFrequency; }
            set
            {
                if (value != m_clockFrequency)
                {
                    m_clockFrequency = value;
                    SetParam(PARAM_CLOCKFREQUENCY);

                    m_dacDisInitTime = Convert.ToByte(Math.Ceiling(m_clockFrequency / Math.Pow(10, 5)));
                    if (m_dacDisInitTime == 0)
                        m_dacDisInitTime = 1;
                    SetParam(PARAM_DACDISINITTIME);

                    CommitParams();
                }
            }
        }

        public decimal InputClockPeriod
        {
            get
            {
                //Calculate Specifies period of input clock.
                return (decimal)(Math.Pow(10, 6) / ClockFrequency);
            }
        }

        public bool Gang
        {
            get { return m_gang; }
            set
            {
                if (value != m_gang)
                {
                    m_gang = value;
                    SetParam(PARAM_GANG);
                    CommitParams();
                }
            }
        }

        public bool DebugMode
        {
            get { return m_debugMode; }
            set
            {
                if (value != m_debugMode)
                {
                    m_debugMode = value;
                    SetParam(PARAM_DEBUGMODE);
                    CommitParams();
                }
            }
        }

        public string SerializedHelpers
        {
            get { return m_serializedHelpers; }
            set
            {
                if (value != m_serializedHelpers)
                {
                    m_serializedHelpers = value;
                    m_serializedHelpers = m_serializedHelpers.Replace("\r\n", " ");
                    SetParam(PARAM_HELPERS);
                    CommitParams();
                }
            }
        }

        public bool ParamHelperChanged
        {
            get { return false;}
            set
            {
                SerializedHelpers = CyHelperInfo.SerializeHelpers(m_helpersConfig, m_serializer, m_customSerNamespace);
            }
        }

        #endregion Properties

        #region Constructors

        public CyLCDParameters()
        {
            m_helpersConfig = new List<CyHelperInfo>();
            CyHelperInfo.CreateHelper(CyHelperKind.EMPTY, this);
        }

        public CyLCDParameters(ICyInstEdit_v1 inst)
        {
            if (inst != null)
            {
                this.m_inst = inst;
                GetParams();
            }
            else
            {
                Debug.Assert(false);
            }

            // Create XML Serializer
            Type[] theExtraTypes = new Type[2];
            theExtraTypes[0] = typeof(CyHelperInfo);
            theExtraTypes[1] = typeof(ArrayList);
            m_serializer = new XmlSerializer(typeof(ArrayList), theExtraTypes);
            m_customSerNamespace = new XmlSerializerNamespaces();
            string curNamespace = Assembly.GetExecutingAssembly().FullName;
            string version = curNamespace.Substring(curNamespace.LastIndexOf("_v") + 1);
            m_customSerNamespace.Add("CustomizerVersion", version);
        }

        #endregion Constructors

        #region Common

        private void GetParams()
        {
            if (m_inst != null)
            {
                m_numCommonLines = Convert.ToByte(m_inst.GetCommittedParam(PARAM_NUMCOMMONLINES).Value);
                m_numSegmentLines = Convert.ToByte(m_inst.GetCommittedParam(PARAM_NUMSEGMENTLINES).Value);
                m_biasType = Convert.ToByte(m_inst.GetCommittedParam(PARAM_BIASTYPE).Value);
                m_waveformType = Convert.ToByte(m_inst.GetCommittedParam(PARAM_WAVEFORMTYPE).Value);
                m_frameRate = Convert.ToByte(m_inst.GetCommittedParam(PARAM_FRAMERATE).Value);
                m_biasVoltage = Convert.ToByte(m_inst.GetCommittedParam(PARAM_BIASVOLTAGE).Value);
                m_driverPowerMode = Convert.ToByte(m_inst.GetCommittedParam(PARAM_DRIVERPOWERMODE).Value);
                m_hiDriveTime = Convert.ToByte(m_inst.GetCommittedParam(PARAM_HIDRIVETIME).Value);
                m_lowDriveMode = Convert.ToByte(m_inst.GetCommittedParam(PARAM_LOWDRIVEMODE).Value);
                m_lowDriveInitTime = Convert.ToByte(m_inst.GetCommittedParam(PARAM_LOWDRIVEINITTIME).Value);
                m_clockFrequency = Convert.ToUInt32(m_inst.GetCommittedParam(PARAM_CLOCKFREQUENCY).Value);
                m_gang = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_GANG).Value);
                m_debugMode = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_DEBUGMODE).Value);
                DeserializeHelpers(m_inst.GetCommittedParam(PARAM_HELPERS).Value);

                //Fix issue with default parameters in versions of the component earlier that v1_30
                if (m_lowDriveInitTime == 0) m_lowDriveInitTime = 1;
                if (m_hiDriveTime == 0) m_hiDriveTime = 1;
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        public void GetExprViewParams()
        {
            if (m_inst != null)
            {
                m_biasVoltage = Convert.ToByte(m_inst.GetCommittedParam(PARAM_BIASVOLTAGE).Value);
                m_gang = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_GANG).Value);
                m_debugMode = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_DEBUGMODE).Value);
                m_cyBasicConfigurationTab.LoadValuesFromParameters();
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        /// <summary>
        /// Deserialize the list of Helper functions from the string stored in Parameters
        /// </summary>
        /// <param name="_SHelpers"> XML string representation of the Helpers list</param>
        public void DeserializeHelpers(string _SHelpers)
        {
            try
            {
                if (!string.IsNullOrEmpty(_SHelpers))
                {
                    m_helpersConfig = CyHelperInfo.DeserializeHelpers(_SHelpers);
                    // Add used helper and symbol indexes to the list
                    for (int i = 0; i < m_helpersConfig.Count; i++)
                    {
                        switch (m_helpersConfig[i].Kind)
                        {
                            case CyHelperKind.SEGMENT_7:
                                m_helperIndexes_7SEG.Add(m_helpersConfig[i].m_globalHelperIndex);
                                for (int j = 0; j < m_helpersConfig[i].m_helpSegInfo.Count; j++)
                                {
                                    if (!m_symbolIndexes_7SEG.Contains(
                                        m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum))
                                    {
                                        m_symbolIndexes_7SEG.Add(m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum);
                                    }
                                }
                                //For compatibility remove the 8th segment in symbols if the one exists
                                for (int j = 0; j < m_helpersConfig[i].m_helpSegInfo.Count; j++)
                                {
                                    if (m_helpersConfig[i].m_helpSegInfo[j].m_relativePos == 7)
                                    {
                                        m_helpersConfig[i].m_helpSegInfo.RemoveAt(j--);
                                    }
                                }
                                m_helpersConfig[i].SegmentCount = 7;
                                break;
                            case CyHelperKind.SEGMENT_14:
                                m_helperIndexes_14SEG.Add(m_helpersConfig[i].m_globalHelperIndex);
                                for (int j = 0; j < m_helpersConfig[i].m_helpSegInfo.Count; j++)
                                {
                                    if (!m_symbolIndexes_14SEG.Contains(
                                        m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum))
                                    {
                                        m_symbolIndexes_14SEG.Add(m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum);
                                    }
                                }
                                break;
                            case CyHelperKind.SEGMENT_16:
                                m_helperIndexes_16SEG.Add(m_helpersConfig[i].m_globalHelperIndex);
                                for (int j = 0; j < m_helpersConfig[i].m_helpSegInfo.Count; j++)
                                {
                                    if (
                                        !m_symbolIndexes_16SEG.Contains(
                                        m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum))
                                    {
                                        m_symbolIndexes_16SEG.Add(m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum);
                                    }
                                }
                                break;
                            case CyHelperKind.BAR:
                                m_helperIndexes_BAR.Add(m_helpersConfig[i].m_globalHelperIndex);
                                for (int j = 0; j < m_helpersConfig[i].m_helpSegInfo.Count; j++)
                                {
                                    if (!m_symbolIndexes_BAR.Contains(
                                                                  m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum))
                                        m_symbolIndexes_BAR.Add(m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum);
                                }
                                break;
                            case CyHelperKind.MATRIX:
                                m_helperIndexes_MATRIX.Add(m_helpersConfig[i].m_globalHelperIndex);
                                for (int j = 0; j < m_helpersConfig[i].m_helpSegInfo.Count; j++)
                                {
                                    if (!m_symbolIndexes_MATRIX.Contains(
                                             m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum))
                                        m_symbolIndexes_MATRIX.Add(
                                                                m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum);
                                }
                                break;
                            case CyHelperKind.EMPTY:
                                // For compatibility, add pixel's definitions if there are less than needed.
                                for (int j1 = 0; j1 < NumSegmentLines; j1++)
                                    for (int j2 = 0; j2 < NumCommonLines; j2++)
                                    {
                                        if (j1*NumCommonLines + j2 >= m_helpersConfig[0].m_helpSegInfo.Count)
                                        {
                                            m_helpersConfig[0].m_helpSegInfo.Add(new CyHelperSegmentInfo(
                                                                                   UNASSIGNED_PIXEL_NAME +
                                                                                   (j1*NumCommonLines + j2), j2,
                                                                                   j1, -1, -1, -1, CyHelperKind.EMPTY,
                                                                                   -1));
                                        }
                                    }
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    m_helpersConfig = new List<CyHelperInfo>();
                    CyHelperInfo.CreateHelper(CyHelperKind.EMPTY, this);
                }
            }
            catch
            {
                MessageBox.Show(Properties.Resources.PARAMETERS_LOADING_ERROR_MSG, ERROR_MSG_TITLE,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetParam(string paramName)
        {
            if (m_inst != null)
            {
                switch (paramName)
                {
                    case PARAM_NUMCOMMONLINES:
                        m_inst.SetParamExpr(PARAM_NUMCOMMONLINES, NumCommonLines.ToString());
                        break;
                    case PARAM_NUMSEGMENTLINES:
                        m_inst.SetParamExpr(PARAM_NUMSEGMENTLINES, NumSegmentLines.ToString());
                        break;
                    case PARAM_BIASTYPE:
                        m_inst.SetParamExpr(PARAM_BIASTYPE, BiasType.ToString());
                        break;
                    case PARAM_WAVEFORMTYPE:
                        m_inst.SetParamExpr(PARAM_WAVEFORMTYPE, WaveformType.ToString());
                        break;
                    case PARAM_FRAMERATE:
                        m_inst.SetParamExpr(PARAM_FRAMERATE, FrameRate.ToString());
                        break;
                    case PARAM_BIASVOLTAGE:
                        m_inst.SetParamExpr(PARAM_BIASVOLTAGE, BiasVoltage.ToString());
                        break;
                    case PARAM_DRIVERPOWERMODE:
                        m_inst.SetParamExpr(PARAM_DRIVERPOWERMODE, DriverPowerMode.ToString());
                        break;
                    case PARAM_HIDRIVETIME:
                        m_inst.SetParamExpr(PARAM_HIDRIVETIME, HiDriveTime.ToString());
                        break;
                    case PARAM_LOWDRIVEMODE:
                        m_inst.SetParamExpr(PARAM_LOWDRIVEMODE, LowDriveMode.ToString());
                        break;
                    case PARAM_LOWDRIVEINITTIME:
                        m_inst.SetParamExpr(PARAM_LOWDRIVEINITTIME, LowDriveInitTime.ToString());
                        break;
                    case PARAM_CLOCKFREQUENCY:
                        m_inst.SetParamExpr(PARAM_CLOCKFREQUENCY, ClockFrequency.ToString());
                        break;
                    case PARAM_DACDISINITTIME:
                        m_inst.SetParamExpr(PARAM_DACDISINITTIME, m_dacDisInitTime.ToString());
                        break;
                    case PARAM_GANG:
                        m_inst.SetParamExpr(PARAM_GANG, Gang.ToString().ToLower());
                        break;
                    case PARAM_DEBUGMODE:
                        m_inst.SetParamExpr(PARAM_DEBUGMODE, DebugMode.ToString().ToLower());
                        break;
                    case PARAM_HELPERS:
                        m_inst.SetParamExpr(PARAM_HELPERS, SerializedHelpers);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        public void SetParams()
        {
            if (m_inst != null)
            {
                m_inst.SetParamExpr(PARAM_NUMCOMMONLINES, NumCommonLines.ToString());
                m_inst.SetParamExpr(PARAM_NUMSEGMENTLINES, NumSegmentLines.ToString());
                m_inst.SetParamExpr(PARAM_BIASTYPE, BiasType.ToString());
                m_inst.SetParamExpr(PARAM_WAVEFORMTYPE, WaveformType.ToString());
                m_inst.SetParamExpr(PARAM_FRAMERATE, FrameRate.ToString());
                m_inst.SetParamExpr(PARAM_BIASVOLTAGE, BiasVoltage.ToString());

                m_inst.SetParamExpr(PARAM_DRIVERPOWERMODE, DriverPowerMode.ToString());
                m_inst.SetParamExpr(PARAM_HIDRIVETIME, HiDriveTime.ToString());
                m_inst.SetParamExpr(PARAM_LOWDRIVEMODE, LowDriveMode.ToString());
                m_inst.SetParamExpr(PARAM_LOWDRIVEINITTIME, LowDriveInitTime.ToString());
                m_inst.SetParamExpr(PARAM_CLOCKFREQUENCY, ClockFrequency.ToString());
                m_inst.SetParamExpr(PARAM_DACDISINITTIME, m_dacDisInitTime.ToString());
                m_inst.SetParamExpr(PARAM_GANG, Gang.ToString().ToLower());
                m_inst.SetParamExpr(PARAM_DEBUGMODE, DebugMode.ToString().ToLower());

                m_inst.SetParamExpr(PARAM_HELPERS, SerializedHelpers);
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        public void CommitParams()
        {
            if (m_inst != null)
            {
                m_inst.CommitParamExprs();
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        #endregion Common
    }

    #region Helper classes

    /// <summary>
    /// Class that represents unassigned pixels
    /// </summary>
    public class CySegmentInfo
    {
        private string m_name;
        private int m_common;
        private int m_segment;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        [XmlAttribute("Com")]
        public int Common
        {
            get { return m_common; }
            set { m_common = value; }
        }

        [XmlAttribute("Seg")]
        public int Segment
        {
            get { return m_segment; }
            set { m_segment = value; }
        }

        public CySegmentInfo()
        {
            Name = CyLCDParameters.UNASSIGNED_PIXEL_NAME;
        }

        public CySegmentInfo(string name, int common, int segment)
        {
            Name = name;
            Common = common;
            Segment = segment;
        }
    }

    /// <summary>
    /// Class that represents pixels of the helper
    /// </summary>
    [XmlRootAttribute("Pixel")]
    public class CyHelperSegmentInfo : CySegmentInfo
    {
        [XmlAttribute("Display")] public int m_displayNum;
        [XmlAttribute("Digit")] public int m_digitNum;
        [XmlAttribute("Helper")] public CyHelperKind m_helperType;
        [XmlAttribute("RelPos")] public int m_relativePos;

        [XmlElement("GlobalDigitNum")]
        public int m_globalDigitNum;

        public CyHelperSegmentInfo()
        {
            m_helperType = CyHelperKind.EMPTY;
        }

        public CyHelperSegmentInfo(string name, int common, int segment, int displayNum, int digitNum, int relPos,
                                   CyHelperKind type, int globalDigitNum) : base(name, common, segment)
        {
            m_displayNum = displayNum;
            m_digitNum = digitNum;
            m_relativePos = relPos;
            m_helperType = type;

            m_globalDigitNum = globalDigitNum;
        }

        public int GetIndex(int CommonCount)
        {
            return CommonCount*Segment + Common;
        }

        public static int GetMaxElementCount(CyHelperKind helperType)
        {
            switch (helperType)
            {
                case CyHelperKind.SEGMENT_7:
                    return 7;
                case CyHelperKind.SEGMENT_14:
                    return 14;
                case CyHelperKind.SEGMENT_16:
                    return 16;
                case CyHelperKind.BAR:
                    return 1;
                case CyHelperKind.MATRIX:
                    return 40;
                case CyHelperKind.EMPTY:
                    break;
                default:
                    break;
            }
            return 0;
        }
    }

    /// <summary>
    /// Class that represents a helper function
    /// </summary>
    [XmlType("HelperInfo")]
    public class CyHelperInfo
    {
        public const int PIXELS_COUNT_7SEG = 7;
        public const int PIXELS_COUNT_14SEG = 14;
        public const int PIXELS_COUNT_16SEG = 16;
        public const int PIXELS_COUNT_BAR = 1;
        public const int PIXELS_COUNT_MATRIX = 5*8;

        public const int MAX_7SEG_SYMBOLS = 5;
        public const int MAX_14SEG_SYMBOLS = 20;
        public const int MAX_16SEG_SYMBOLS = 20;
        public const int MAX_BAR_SYMBOLS = 255;
        public const int MAX_MATRIX_SYMBOLS = 8;

        [XmlElement("Name")] 
        public string m_name;

        private CyHelperKind m_kind;
        private int m_symbolsCount;
        private int m_maxSymbolsCount;
        private int m_segmentCount;
        private int m_displayNum;

        [XmlElementAttribute("Color")] 
        public int m_helperColor;

        // m_GlobalHelperIndex is used for the names of the helpers. 
        // Each kind of helpers (7-segment, 14-segment, etc.) has its own numeration.
        [XmlElement("GlobalHelperIndex")] 
        public int m_globalHelperIndex;

        [XmlIgnore]
        public Color HelperColor
        {
            get { return Color.FromArgb(m_helperColor); }
            set { m_helperColor = value.ToArgb(); }
        }

        [XmlElementAttribute("Kind")]
        public CyHelperKind Kind
        {
            get { return m_kind; }
            set { m_kind = value; }
        }

        [XmlElementAttribute("MaxSymbolsCount")]
        public int MaxSymbolsCount
        {
            get { return m_maxSymbolsCount; }
            set { m_maxSymbolsCount = value; }
        }

        [XmlElementAttribute("SegmentCount")]
        public int SegmentCount
        {
            get { return m_segmentCount; }
            set { m_segmentCount = value; }
        }

        [XmlElementAttribute("DisplayNum")]
        public int DisplayNum
        {
            get { return m_displayNum; }
            set { m_displayNum = value; }
        }

        [XmlArray("HelpSegInfoArray")] [XmlArrayItem("HelperSegmentInfo")]
        public List<CyHelperSegmentInfo> m_helpSegInfo;

        public int SymbolsCount
        {
            get { return m_symbolsCount; }
            set { m_symbolsCount = value; }
        }

        public CyHelperInfo()
        {
            m_helpSegInfo = new List<CyHelperSegmentInfo>();

            m_name = "Empty";
            m_kind = CyHelperKind.EMPTY;
            SymbolsCount = 0;
            m_maxSymbolsCount = 0;
            m_segmentCount = 0;
        }

        public CyHelperInfo(string name, CyHelperKind kind, int maxSymbolsCount, int segmentCount, Color color,
                          int helperIndex)
        {
            m_name = name;
            m_kind = kind;
            m_maxSymbolsCount = maxSymbolsCount;
            m_segmentCount = segmentCount;
            SymbolsCount = 0;
            m_helpSegInfo = new List<CyHelperSegmentInfo>();

            HelperColor = color;
            m_globalHelperIndex = helperIndex;
        }

        //--------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return m_name;
        }

        public string GetPixelNameByCommonSegment(int common, int segment)
        {
            string name = "";
            for (int i = 0; i < m_helpSegInfo.Count; i++)
            {
                if ((m_helpSegInfo[i].Common == common) &&
                    (m_helpSegInfo[i].Segment == segment))
                {
                    name = m_helpSegInfo[i].Name;
                    break;
                }
            }

            return name;
        }

        public CySegmentInfo GetPixelByCommonSegment(int common, int segment)
        {
            CySegmentInfo pixel = null;
            for (int i = 0; i < m_helpSegInfo.Count; i++)
            {
                if ((m_helpSegInfo[i].Common == common) &&
                    (m_helpSegInfo[i].Segment == segment))
                {
                    pixel = m_helpSegInfo[i];
                    break;
                }
            }

            return pixel;
        }

        public CySegmentInfo GetPixelByName(string name)
        {
            CySegmentInfo pixel = null;
            for (int i = 0; i < m_helpSegInfo.Count; i++)
            {
                if (m_helpSegInfo[i].Name == name)
                {
                    pixel = m_helpSegInfo[i];
                    break;
                }
            }

            return pixel;
        }

        public CyHelperSegmentInfo GetPixelBySymbolSegment(int symbol, int segment)
        {
            CyHelperSegmentInfo pixel = null;
            for (int i = 0; i < m_helpSegInfo.Count; i++)
            {
                if ((m_helpSegInfo[i].m_digitNum == symbol) &&
                    (m_helpSegInfo[i].m_relativePos == segment))
                {
                    pixel = m_helpSegInfo[i];
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
                case CyHelperKind.SEGMENT_7:
                    name = nameTemplate.Replace("*", "7seg");
                    break;
                case CyHelperKind.SEGMENT_14:
                    name = nameTemplate.Replace("*", "14seg");
                    break;
                case CyHelperKind.SEGMENT_16:
                    name = nameTemplate.Replace("*", "16seg");
                    break;
                case CyHelperKind.BAR:
                    name = "HBar";
                    break;
                case CyHelperKind.MATRIX:
                    name = nameTemplate.Replace("*", "Dot");
                    break;
                default:
                    break;
            }
            return name.ToUpper();
        }

        public void AddSymbol(int symbolGlobalNum)
        {
            if ((SymbolsCount >= MaxSymbolsCount) || (Kind == CyHelperKind.EMPTY))
                return;

            string nameTemplate = GetDefaultSymbolName(symbolGlobalNum);
            string name = "SEG";
            char letter = 'A';
            for (int i = 0; i < SegmentCount; i++)
            {
                switch (Kind)
                {
                    case CyHelperKind.SEGMENT_7:
                        name = nameTemplate + ((char) ('A' + i));
                        break;
                    case CyHelperKind.SEGMENT_14:
                        name = nameTemplate + ((char) ('A' + i));
                        break;
                    case CyHelperKind.SEGMENT_16:
                        if ((i == 1))
                        {
                            name = nameTemplate + "A_";
                        }
                        else if (i == 4)
                        {
                            name = nameTemplate + "D_";
                        }
                        else
                        {
                            name = nameTemplate + letter;
                            letter = (char) (letter + 1);
                        }
                        break;
                    case CyHelperKind.BAR:
                        name = nameTemplate + symbolGlobalNum;
                        break;
                    case CyHelperKind.MATRIX:
                        name = nameTemplate + (i%5) + (i/5);
                        break;
                    default:
                        break;
                }
                int com = -1;
                int seg = -1;
                m_helpSegInfo.Add(new CyHelperSegmentInfo(name.ToUpper(), com, seg, 0, SymbolsCount, i, Kind,
                                                        symbolGlobalNum));
            }

            SymbolsCount++;
        }

        //--------------------------------------------------------------------------------------------------------------

        #region Static functions

        public static void CreateHelper(CyHelperKind kind, CyLCDParameters parameters)
        {
            int num = AddNextHelperIndex(kind, parameters);
            CyHelperInfo helper;
            switch (kind)
            {
                case CyHelperKind.SEGMENT_7:
                    helper = new CyHelperInfo("Helper_7Segment_" + num, kind, MAX_7SEG_SYMBOLS, 7,
                                            parameters.m_colorChooser.PopCl(), num);
                    break;
                case CyHelperKind.SEGMENT_14:
                    helper = new CyHelperInfo("Helper_14Segment_" + num, kind, MAX_14SEG_SYMBOLS, 14,
                                            parameters.m_colorChooser.PopCl(), num);
                    break;
                case CyHelperKind.SEGMENT_16:
                    helper = new CyHelperInfo("Helper_16Segment_" + num, kind, MAX_16SEG_SYMBOLS, 16,
                                            parameters.m_colorChooser.PopCl(), num);
                    break;
                case CyHelperKind.BAR:
                    helper = new CyHelperInfo("Helper_Bar_" + num, kind, MAX_BAR_SYMBOLS, 1,
                                            parameters.m_colorChooser.PopCl(), num);
                    break;
                case CyHelperKind.MATRIX:
                    helper = new CyHelperInfo("Helper_Matrix_" + num, kind, MAX_MATRIX_SYMBOLS, 5*8,
                                            parameters.m_colorChooser.PopCl(), num);
                    break;
                default:
                    helper = new CyHelperInfo();
                    break;
            }
            helper.DisplayNum = parameters.m_helpersConfig.Count - 1;
            parameters.m_helpersConfig.Add(helper);

            //Pixels management
            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            if (kind == CyHelperKind.EMPTY)
            {
                for (int j = 0; j < NumSegmentLines; j++)
                    for (int i = 0; i < NumCommonLines; i++)
                    {
                        helper.m_helpSegInfo.Add(
                            new CyHelperSegmentInfo(CyLCDParameters.UNASSIGNED_PIXEL_NAME + (j*NumCommonLines + i), i, 
                                                    j, -1, -1, -1, kind, -1));
                    }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void UpdateEmptyHelper(CyLCDParameters parameters)
        {
            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            parameters.m_helpersConfig[0].m_helpSegInfo.Clear();
            for (int j = 0; j < NumSegmentLines; j++)
                for (int i = 0; i < NumCommonLines; i++)
                {
                    parameters.m_helpersConfig[0].m_helpSegInfo.Add(new CyHelperSegmentInfo(
                                                                      CyLCDParameters.UNASSIGNED_PIXEL_NAME +
                                                                      (j*NumCommonLines + i), i, j, -1, -1, -1,
                                                                      CyHelperKind.EMPTY, -1));
                }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static int GetTotalPixelNumber(CyLCDParameters parameters)
        {
            int totalPixels = 0;
            for (int i = 1; i < parameters.m_helpersConfig.Count; i++)
            {
                totalPixels += parameters.m_helpersConfig[i].m_helpSegInfo.Count;
            }
            return totalPixels;
        }

        public static bool CheckPixelUniqueName(CyLCDParameters parameters, string name, int n)
        {
            // false - if there are matches
            int times = 0;
            for (int i = 0; i < parameters.m_helpersConfig.Count; i++)
                for (int j = 0; j < parameters.m_helpersConfig[i].m_helpSegInfo.Count; j++)
                {
                    if (parameters.m_helpersConfig[i].m_helpSegInfo[j].Name.ToUpper() == name.ToUpper())
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
            for (int i = 1; i < parameters.m_helpersConfig.Count; i++)
            {
                if (parameters.m_helpersConfig[i].m_name == name)
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
                case CyHelperKind.SEGMENT_7:
                    while (parameters.m_helperIndexes_7SEG.Contains(index))
                        index++;
                    parameters.m_helperIndexes_7SEG.Add(index);
                    break;
                case CyHelperKind.SEGMENT_14:
                    while (parameters.m_helperIndexes_14SEG.Contains(index))
                        index++;
                    parameters.m_helperIndexes_14SEG.Add(index);
                    break;
                case CyHelperKind.SEGMENT_16:
                    while (parameters.m_helperIndexes_16SEG.Contains(index))
                        index++;
                    parameters.m_helperIndexes_16SEG.Add(index);
                    break;
                case CyHelperKind.BAR:
                    while (parameters.m_helperIndexes_BAR.Contains(index))
                        index++;
                    parameters.m_helperIndexes_BAR.Add(index);
                    break;
                case CyHelperKind.MATRIX:
                    while (parameters.m_helperIndexes_MATRIX.Contains(index))
                        index++;
                    parameters.m_helperIndexes_MATRIX.Add(index);
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
                case CyHelperKind.SEGMENT_7:
                    parameters.m_helperIndexes_7SEG.Remove(index);
                    break;
                case CyHelperKind.SEGMENT_14:
                    parameters.m_helperIndexes_14SEG.Remove(index);
                    break;
                case CyHelperKind.SEGMENT_16:
                    parameters.m_helperIndexes_16SEG.Remove(index);
                    break;
                case CyHelperKind.BAR:
                    parameters.m_helperIndexes_BAR.Remove(index);
                    break;
                case CyHelperKind.MATRIX:
                    parameters.m_helperIndexes_MATRIX.Remove(index);
                    break;
                default:
                    break;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static string SerializeHelpers(List<CyHelperInfo> list, XmlSerializer s,
                                              XmlSerializerNamespaces customSerNamespace)
        {
            ArrayList helpers = new ArrayList(list);
            StringWriter sw = new StringWriter();
            s.Serialize(sw, helpers, customSerNamespace);
            string serializedXml = sw.ToString();
            return serializedXml;
        }

        public static List<CyHelperInfo> DeserializeHelpers(string serializedXml)
        {
            Type[] theExtraTypes = new Type[2];
            theExtraTypes[0] = typeof (CyHelperInfo);
            theExtraTypes[1] = typeof (ArrayList);
            XmlSerializer s = new XmlSerializer(typeof (ArrayList), theExtraTypes);
            ArrayList helpers = (ArrayList) s.Deserialize(new StringReader(serializedXml));
            List<CyHelperInfo> newList = new List<CyHelperInfo>();
            for (int i = 0; i < helpers.Count; i++)
            {
                newList.Add((CyHelperInfo) helpers[i]);
            }
            return newList;
        }

        #endregion Static functions
    }

    #endregion Helper classes

    #region CyColorList Class

    /// <summary>
    /// Class that is used for painting the Helper List in different colors
    /// </summary>
    public class CyColorList
    {
        private readonly List<Color> m_ColorsStack = new List<Color>();

        public CyColorList()
        {
            m_ColorsStack.Add(Color.FromArgb(226, 147, 147));
            m_ColorsStack.Add(Color.FromArgb(226, 200, 147));
            m_ColorsStack.Add(Color.FromArgb(200, 226, 147));
            m_ColorsStack.Add(Color.FromArgb(147, 226, 200));
            m_ColorsStack.Add(Color.FromArgb(147, 200, 226));
            m_ColorsStack.Add(Color.FromArgb(147, 147, 226));
            m_ColorsStack.Add(Color.FromArgb(200, 147, 226));
            m_ColorsStack.Add(Color.FromArgb(226, 147, 200));
        }

        public void PushCl(Color clr)
        {
            if (clr != Color.MintCream)
            {
                m_ColorsStack.Add(clr);
            }
        }

        public Color PopCl()
        {
            Color res = Color.MintCream;
            if (m_ColorsStack.Count > 0)
            {
                res = m_ColorsStack[m_ColorsStack.Count - 1];
                m_ColorsStack.Remove(res);
            }
            return res;
        }

        public void PopCl(Color clr)
        {
            m_ColorsStack.Remove(clr);
        }
    }

    #endregion
}