// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using CyDesigner.Extensions.Gde;


namespace StaticSegLCD_v1_30
{
    public class CyLCDParameters
    {
        #region Variables

        readonly ICyInstEdit_v1 m_inst;

        public static string FormatErrorMsg = "The value should be between {0:G} and {1:G}.";
        public static string FormatIntErrorMsg = "The value should be integer between {0:G} and {1:G}.";

        private byte m_NumSegmentLines = 8;
        private byte m_FrameRate = 0;

        private uint m_ClockFrequency = 800;

        public List<HelperInfo> m_HelpersConfig;
        private String m_SerializedHelpers;

        public CyColorList m_ColorChooser = new CyColorList();

        public List<int> m_SymbolIndexes_7SEG = new List<int>();
        public List<int> m_SymbolIndexes_14SEG = new List<int>();
        public List<int> m_SymbolIndexes_16SEG = new List<int>();
        public List<int> m_SymbolIndexes_BAR = new List<int>();

        public List<int> m_HelperIndexes_7SEG = new List<int>();
        public List<int> m_HelperIndexes_14SEG = new List<int>();
        public List<int> m_HelperIndexes_16SEG = new List<int>();
        public List<int> m_HelperIndexes_BAR = new List<int>();

        private int m_ParametersChanged = 0;

        #endregion Variables

        #region Properties

        public byte NumCommonLines
        {
            get {return 1;}
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

        public uint ClockFrequency
        {
            get { return m_ClockFrequency; }
            set
            {
                if(value!=m_ClockFrequency)
                {
                    m_ClockFrequency = value;
                    SetParam("ClockFrequency");
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
            HelperInfo.CreateHelper(CyHelperKind.Empty, this);
        }

        public CyLCDParameters(ICyInstEdit_v1 inst)
        {
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
                m_FrameRate = GetByteParam("FrameRate");

                m_ClockFrequency = GetUintParam("ClockFrequency");
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
                    case "FrameRate":
                        m_inst.SetParamExpr("FrameRate", FrameRate.ToString());
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
                m_inst.SetParamExpr("FrameRate", FrameRate.ToString());
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

        public string Name;
        private CyHelperKind m_Kind;
        private int m_SymbolsCount;
        private int m_MaxSymbolsCount;
        private int m_SegmentCount;
        private int m_DisplayNum;
        [XmlElementAttribute("Color")]
        public int m_HelperColor;

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
