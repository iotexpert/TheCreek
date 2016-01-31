/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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


namespace StaticSegLCD_v2_10
{
    public class CyLCDParameters
    {
        #region Constants

        // Parameters
        public const string PARAM_NUMSEGMENTLINES = "NumSegmentLines";
        public const string PARAM_FRAMERATE = "FrameRate";
        public const string PARAM_GANG = "Gang";
        public const string PARAM_HELPERS = "Helpers";

        public const string UNASSIGNED_PIXEL_NAME = "PIX";
        public const string EXPR_VIEW_PARAM_TABNAME = "General";
        
        #endregion

        #region Fields

        public readonly ICyInstEdit_v1 m_inst;

        private byte m_numSegmentLines = 8;
        private byte m_frameRate = 0;
        private bool m_gang;

        public List<CyHelperInfo> m_helpersConfig; // List of helpers
        private String m_serializedHelpers; // Xml string that represents serialized helpers and is passed in parameters

        public CyColorList m_colorChooser = new CyColorList();
        public XmlSerializer m_serializer;
        public XmlSerializerNamespaces m_customSerNamespace;

        // These fields are used for tracking the global symbol numeration for each type of helpers
        public List<int> m_symbolIndexes_7SEG = new List<int>();
        public List<int> m_symbolIndexes_14SEG = new List<int>();
        public List<int> m_symbolIndexes_16SEG = new List<int>();
        public List<int> m_symbolIndexes_BAR = new List<int>();
        // These fields are used for tracking the global helpers numeration for each type of helpers
        public List<int> m_helperIndexes_7SEG = new List<int>();
        public List<int> m_helperIndexes_14SEG = new List<int>();
        public List<int> m_helperIndexes_16SEG = new List<int>();
        public List<int> m_helperIndexes_BAR = new List<int>();

        public CyBasicConfiguration m_cyBasicConfigurationTab;
        public CyHelpers m_cyHelpersTab;

        #endregion Fields

        #region Properties

        public byte NumCommonLines
        {
            get {return 1;}
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
                }
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
                    m_serializedHelpers = value.Replace("\r\n", " ");
                    SetParam(PARAM_HELPERS);
                }
            }
        }

        public bool ParamHelperChanged
        {
            get { return false; }
            set { SerializedHelpers = 
                  CyHelperInfo.SerializeHelpers(m_helpersConfig, m_serializer, m_customSerNamespace); }
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
                m_inst = inst;
                GetParams();
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
                m_numSegmentLines = GetByteParam(PARAM_NUMSEGMENTLINES);
                m_frameRate = GetByteParam(PARAM_FRAMERATE);
                m_gang = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_GANG).Value);

                DeserializeHelpers(GetStringParam(PARAM_HELPERS));
            }
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

        public void GetExprViewParams(CyCompDevParam param)
        {
            if (m_inst != null)
            {
                GetParams();
                m_cyBasicConfigurationTab.LoadValuesFromParameters();
            }
        }

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
                                //For compability remove the 8th segment in symbols if the one exists
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
                                        m_symbolIndexes_14SEG.Add(m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum);
                                }
                                break;
                            case CyHelperKind.SEGMENT_16:
                                m_helperIndexes_16SEG.Add(m_helpersConfig[i].m_globalHelperIndex);
                                for (int j = 0; j < m_helpersConfig[i].m_helpSegInfo.Count; j++)
                                {
                                    if (!m_symbolIndexes_16SEG.Contains(
                                                            m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum))
                                        m_symbolIndexes_16SEG.Add(m_helpersConfig[i].m_helpSegInfo[j].m_globalDigitNum);
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
                MessageBox.Show(Properties.Resources.PARAMETERS_LOADING_ERROR_MSG, Properties.Resources.ERROR_MSG_TITLE, 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetParam(string paramName)
        {
            if (m_inst != null)
            {
                switch (paramName)
                {
                    case PARAM_NUMSEGMENTLINES:
                        m_inst.SetParamExpr(PARAM_NUMSEGMENTLINES, NumSegmentLines.ToString());
                        break;
                    case PARAM_FRAMERATE:
                        m_inst.SetParamExpr(PARAM_FRAMERATE, FrameRate.ToString());
                        break;
                    case PARAM_GANG:
                        m_inst.SetParamExpr(PARAM_GANG, Gang.ToString().ToLower());
                        break;
                    case PARAM_HELPERS:
                        m_inst.SetParamExpr(PARAM_HELPERS, SerializedHelpers);
                        break;
                    default:
                        break;
                }
                CommitParams();
            }
        }

        public void CommitParams()
        {
            if (m_inst != null)
                m_inst.CommitParamExprs();
        }
        
        #endregion Common
    }


    #region Helper classes
    /// <summary>
    /// This class is used to store information about pixel's name, common and segment lines to which it is assigned
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

        public int GetIndex(int CommonCount)
        {
            return CommonCount * Segment + Common;
        }
    }

    /// <summary>
    /// This class represents helper's pixels
    /// </summary>
    [XmlRootAttribute("Pixel")]
    public class CyHelperSegmentInfo : CySegmentInfo
    {
        [XmlAttribute("Display")]
        public int m_displayNum; // Helper index
        [XmlAttribute("Digit")]
        public int m_digitNum; // Number of digit in helper
        [XmlAttribute("Helper")]
        public CyHelperKind m_helperType;
        [XmlAttribute("RelPos")]
        public int m_relativePos; // Number of pixel in the symbol
        [XmlElementAttribute("GlobalDigitNum")]
        public int m_globalDigitNum; //Global symbol number in helpers of current type, used in pixel name

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
    }

    /// <summary>
    /// This class represents helpers
    /// </summary>
    [XmlType("HelperInfo")]
    public class CyHelperInfo
    {
        public const int PIXELS_COUNT_7SEG = 7;
        public const int PIXELS_COUNT_14SEG = 14;
        public const int PIXELS_COUNT_16SEG = 16;
        public const int PIXELS_COUNT_BAR = 1;

        public const int MAX_7SEG_SYMBOLS = 5;
        public const int MAX_14SEG_SYMBOLS = 20;
        public const int MAX_16SEG_SYMBOLS = 20;
        public const int MAX_BAR_SYMBOLS = 255;

        [XmlElementAttribute("Name")]
        public string m_name;
        private CyHelperKind m_kind;
        private int m_symbolsCount;
        private int m_maxSymbolsCount;
        private int m_segmentCount;
        private int m_displayNum;
        [XmlElementAttribute("Color")]
        public int m_helperColor;
        [XmlElementAttribute("GlobalHelperIndex")]
        public int m_globalHelperIndex; //Global index among helpers of its type, used in helper's name

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

        [XmlArray("HelpSegInfoArray")]
        [XmlArrayItem("HelperSegmentInfo")]
        public List<CyHelperSegmentInfo> m_helpSegInfo; // Array with pixels

        public int SymbolsCount
        {
            get { return m_symbolsCount; }
            set
            {
                m_symbolsCount = value;
            }
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

        public CyHelperInfo(string name, CyHelperKind kind, int maxSymbolsCount, int segmentCount,Color color, 
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
        
        //---------------------------------------------------------------------------------------------------------
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
                        name = nameTemplate + ((char)('A' + i));
                        break;
                    case CyHelperKind.SEGMENT_14:
                        name = nameTemplate + ((char)('A' + i));
                        break;
                    case CyHelperKind.SEGMENT_16:
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
                    case CyHelperKind.BAR:
                        name = nameTemplate + symbolGlobalNum;
                        break;
                    default:
                        break;
                }
                m_helpSegInfo.Add(new CyHelperSegmentInfo(name.ToUpper(), -1, -1, 0, SymbolsCount, i, Kind, 
                    symbolGlobalNum));
            }
            
            SymbolsCount++;

        }

        //---------------------------------------------------------------------------------------------------------
        #region Static functions

        public static void CreateHelper(CyHelperKind kind, CyLCDParameters parameters)
        {
            int num = AddNextHelperIndex(kind, parameters);
            CyHelperInfo helper;
            switch (kind)
            {
                case CyHelperKind.SEGMENT_7:
                    helper = new CyHelperInfo("Helper_7Segment_" + num, kind, MAX_7SEG_SYMBOLS, PIXELS_COUNT_7SEG,
                        parameters.m_colorChooser.PopCl(), num);
                    break;
                case CyHelperKind.SEGMENT_14:
                    helper = new CyHelperInfo("Helper_14Segment_" + num, kind, MAX_14SEG_SYMBOLS, PIXELS_COUNT_14SEG, 
                        parameters.m_colorChooser.PopCl(), num);
                    break;
                case CyHelperKind.SEGMENT_16:
                    helper = new CyHelperInfo("Helper_16Segment_" + num, kind, MAX_16SEG_SYMBOLS, PIXELS_COUNT_16SEG, 
                        parameters.m_colorChooser.PopCl(), num);
                    break;
                case CyHelperKind.BAR:
                    helper = new CyHelperInfo("Helper_Bar_" + num, kind, MAX_BAR_SYMBOLS, PIXELS_COUNT_BAR, 
                        parameters.m_colorChooser.PopCl(), num);
                    break;
                default:
                    helper = new CyHelperInfo();
                    break;
            }
            helper.DisplayNum = parameters.m_helpersConfig.Count - 1;
            parameters.m_helpersConfig.Add(helper);

            // Create Empty Helper.
            // Empty helper is a fictitious helper that contains Segment*Common number of pixels in their unassigned 
            // state. Its purpose is to store unassigned pixels names. It exists always independently of presence of 
            // other helpers. Its index in m_helpersConfig array is always 0.
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

        //---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Empty helper is a fictitious helper that contains Segment*Common number of pixels in their unassigned state.
        /// Its purpose is to store unassigned pixels names. 
        /// It exists always independently of presence of other helpers. Its index in m_helpersConfig array is always 0.
        /// </summary>
        /// <param name="parameters"></param>
        public static void UpdateEmptyHelper(CyLCDParameters parameters)
        {
            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            parameters.m_helpersConfig[0].m_helpSegInfo.Clear();
            for (int j = 0; j < NumSegmentLines; j++)
                for (int i = 0; i < NumCommonLines; i++)
                {
                    parameters.m_helpersConfig[0].m_helpSegInfo.Add(
                        new CyHelperSegmentInfo(CyLCDParameters.UNASSIGNED_PIXEL_NAME + (j*NumCommonLines + i), i, j, 
                                                -1, -1, -1, CyHelperKind.EMPTY, -1));
                }
        }

        //---------------------------------------------------------------------------------------------------------
        
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
                default:
                    break;
            }
        }

        //---------------------------------------------------------------------------------------------------------

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
            theExtraTypes[0] = typeof(CyHelperInfo);
            theExtraTypes[1] = typeof(ArrayList);
            XmlSerializer s = new XmlSerializer(typeof(ArrayList), theExtraTypes);
            ArrayList helpers = (ArrayList)s.Deserialize(new StringReader(serializedXml));
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
    /// This class is used to display each helper in different color
    /// </summary>
    public class CyColorList
    {
        readonly List<Color> m_clStack = new List<Color>();
        
        public CyColorList()
        {
            m_clStack.Add(Color.FromArgb(226,147,147));            
            m_clStack.Add(Color.FromArgb(226,200,147));
            m_clStack.Add(Color.FromArgb(200,226,147));
            m_clStack.Add(Color.FromArgb(147,226,200));
            m_clStack.Add(Color.FromArgb(147,200,226));
            m_clStack.Add(Color.FromArgb(147,147,226));
            m_clStack.Add(Color.FromArgb(200,147,226));
            m_clStack.Add(Color.FromArgb(226,147,200));
        }
        
        public void PushCl(Color clp)
        {
            if (clp != Color.MintCream)
            {
                m_clStack.Add(clp);
            }
        }
        
        public Color PopCl()
        {
            Color res = Color.MintCream;
            if (m_clStack.Count > 0)
            {
                res = m_clStack[m_clStack.Count - 1];
                m_clStack.Remove(res);
            }
            return res;
        }

        public void PopCl(Color clp)
        {
            m_clStack.Remove(clp);
        }
    }
    #endregion
}
