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


namespace SegLCD_v1_30
{
    public class CyLCDParameters
    {
        #region Constants

        // Messages
        public const string FORMAT_ERROR_MSG = "The value should be between {0:G} and {1:G}.";
        public const string FORMAT_INT_ERROR_MSG = "The value should be integer between {0:G} and {1:G}.";
        public const string REMOVE_HELPERS_MSG = "Please, remove helper functions first.";
        public const string HELPERS_LIMIT_MSG = "Maximum number of helpers is {0:G}.\n Helper could not be added.";
        public const string NOT_ENOUGH_SEG_COM_MSG = 
                            "Not enough segment and common lines to manage helpers.\n Helper could not be added.";
        public const string NOT_UNIQUE_PIXEL_NAME_MSG = "The name {0} is not unique. Please, enter another name.";
        public const string NOT_UNIQUE_HELPER_NAME_MSG = "The name {0} is not unique. Please, enter another name.";
        public const string WRONG_SYMBOL_MSG = "The name can contain only letters, digits and '_' symbol.";
        public const string PRINT_ERROR_MSG = "An error during printing occured.";
        public const string PARAMETERS_LOADING_ERROR_MSG = "Error in Helpers parameter";
        public const string PARAMETERS_COMMIT_ERROR_MSG = "Error in commiting parameter";

        public const string ERROR_MSG_TITLE = "Error";
        public const string WARNING_MSG_TITLE = "Warning";
        public const string INFORMATION_MSG_TITLE = "Information";

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
        public const string PARAM_PARAMCHANGED = "ParamChanged";

        public const string UNASSIGNED_PIXEL_NAME = "PIX";

        #endregion Constants

        #region Variables

        private readonly ICyInstEdit_v1 m_inst;

        private byte m_NumCommonLines = 4;
        private byte m_NumSegmentLines = 8;
        private byte m_BiasType;
        private byte m_WaveformType;
        private byte m_FrameRate;
        private byte m_BiasVoltage;
        private byte m_DriverPowerMode;
        private byte m_HiDriveTime;
        private byte m_LowDriveMode;
        private byte m_LowDriveInitTime;
        private uint m_ClockFrequency;
        private byte m_DacDisInitTime;
        private bool m_Gang;
        private bool m_DebugMode;

        public List<CyHelperInfo> m_HelpersConfig;
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

        private int m_ParametersChanged;

        public bool m_GlobalEditMode = false;

        //Tabs
        public CyBasicConfiguration m_CyBasicConfigurationTab;
        public CyDriverParams m_CyDriverParamsTab;
        public CyHelpers m_CyHelpersTab;

        #endregion Variables

        #region Properties

        public byte NumCommonLines
        {
            get { return m_NumCommonLines; }
            set
            {
                if (value != m_NumCommonLines)
                {
                    m_NumCommonLines = value;
                    SetParam(PARAM_NUMCOMMONLINES);
                    CommitParams();
                }
            }
        }

        public byte NumSegmentLines
        {
            get { return m_NumSegmentLines; }
            set
            {
                if (value != m_NumSegmentLines)
                {
                    m_NumSegmentLines = value;
                    SetParam(PARAM_NUMSEGMENTLINES);
                    CommitParams();
                }
            }
        }

        public byte BiasType
        {
            get { return m_BiasType; }
            set
            {
                if (value != m_BiasType)
                {
                    m_BiasType = value;
                    SetParam(PARAM_BIASTYPE);
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
                    SetParam(PARAM_WAVEFORMTYPE);
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
                    SetParam(PARAM_FRAMERATE);
                    CommitParams();
                }
            }
        }

        public byte BiasVoltage
        {
            get { return m_BiasVoltage; }
            set
            {
                if (value != m_BiasVoltage)
                {
                    m_BiasVoltage = value;
                    SetParam(PARAM_BIASVOLTAGE);
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
                    SetParam(PARAM_DRIVERPOWERMODE);
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
                    SetParam(PARAM_HIDRIVETIME);
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
                    SetParam(PARAM_LOWDRIVEMODE);
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
                    SetParam(PARAM_LOWDRIVEINITTIME);
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
                    SetParam(PARAM_CLOCKFREQUENCY);

                    UpdateDacDisInitTime();

                    byte[] m;
                    UpdateDriveTimeData(out m);

                    CommitParams();
                }
            }
        }

        public void UpdateDriveTimeData(out byte[] max_time)
        {
            byte[] listDrTime = new byte[]  { HiDriveTime, LowDriveInitTime };
            max_time=new byte[2]; 

            for (int i = 0; i < listDrTime.Length; i++)
            {
                int j = (i + 1) % 2;
                max_time[i] = 1;
                if (DriverPowerMode == 1)
                {
                    max_time[i] = (byte)(250 - listDrTime[j] - DacDisInitTime);
                }
                else
                {
                    max_time[i] = (byte)(255 - listDrTime[j] - DacDisInitTime);
                }

                if (listDrTime[i] > max_time[i])
                {

                    listDrTime[i] = max_time[i];//Fix value
                }
            }
            HiDriveTime = listDrTime[0];
            LowDriveInitTime = listDrTime[1];
        }

        public void UpdateDacDisInitTime()
        {
            m_DacDisInitTime = Convert.ToByte(Math.Ceiling(m_ClockFrequency / Math.Pow(10, 5)));
            if (m_DacDisInitTime == 0)
                m_DacDisInitTime = 1;
            SetParam(PARAM_DACDISINITTIME);
        }

        public byte DacDisInitTime
        {
            get { return m_DacDisInitTime; }
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
            get { return m_Gang; }
            set
            {
                if (value != m_Gang)
                {
                    m_Gang = value;
                    SetParam(PARAM_GANG);
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
                    SetParam(PARAM_DEBUGMODE);
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
                    SetParam(PARAM_DISABLEDCOMMONS);
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
                    SetParam(PARAM_HELPERS);
                    CommitParams();
                }
            }
        }

        public bool ParametersChanged
        {
            set
            {
                m_ParametersChanged++;
                SetParam(PARAM_PARAMCHANGED);
                CommitParams();
            }
        }

        #endregion Properties

        #region Constructors

        public CyLCDParameters()
        {
            m_HelpersConfig = new List<CyHelperInfo>();
            DisabledCommons = new List<int>();
            CyHelperInfo.CreateHelper(CyHelperKind.EMPTY, this);
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
                m_ParametersChanged = Convert.ToInt32(m_inst.GetCommittedParam(PARAM_PARAMCHANGED).Value);
                if (m_ParametersChanged > 1000) m_ParametersChanged = 0;
                m_NumCommonLines = Convert.ToByte(m_inst.GetCommittedParam(PARAM_NUMCOMMONLINES).Value);
                m_NumSegmentLines = Convert.ToByte(m_inst.GetCommittedParam(PARAM_NUMSEGMENTLINES).Value);
                m_BiasType = Convert.ToByte(m_inst.GetCommittedParam(PARAM_BIASTYPE).Value);
                m_WaveformType = Convert.ToByte(m_inst.GetCommittedParam(PARAM_WAVEFORMTYPE).Value);
                m_FrameRate = Convert.ToByte(m_inst.GetCommittedParam(PARAM_FRAMERATE).Value);
                m_BiasVoltage = Convert.ToByte(m_inst.GetCommittedParam(PARAM_BIASVOLTAGE).Value);

                m_DriverPowerMode = Convert.ToByte(m_inst.GetCommittedParam(PARAM_DRIVERPOWERMODE).Value);
                m_HiDriveTime = Convert.ToByte(m_inst.GetCommittedParam(PARAM_HIDRIVETIME).Value);
                m_LowDriveMode = Convert.ToByte(m_inst.GetCommittedParam(PARAM_LOWDRIVEMODE).Value);
                m_LowDriveInitTime = Convert.ToByte(m_inst.GetCommittedParam(PARAM_LOWDRIVEINITTIME).Value);
                
                m_Gang = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_GANG).Value);
                m_DebugMode = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_DEBUGMODE).Value);
                m_DisabledCommons = Convert.ToString(m_inst.GetCommittedParam(PARAM_DISABLEDCOMMONS).Value);
                
                ClockFrequency = Convert.ToUInt32(m_inst.GetCommittedParam(PARAM_CLOCKFREQUENCY).Value);

                DeserializeHelpers(m_inst.GetCommittedParam(PARAM_HELPERS).Value);

                //Fix issue with default parameters in versions of the component earlier that v1_30
                if (m_LowDriveInitTime == 0) m_LowDriveInitTime = 1;
                if (m_HiDriveTime == 0) m_HiDriveTime = 1;
            }
        }

        public void GetExprViewParams()
        {
            if (m_inst != null)
            {
                m_BiasVoltage = Convert.ToByte(m_inst.GetCommittedParam(PARAM_BIASVOLTAGE).Value);
                m_Gang = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_GANG).Value);
                m_DebugMode = Convert.ToBoolean(m_inst.GetCommittedParam(PARAM_DEBUGMODE).Value);
                m_CyBasicConfigurationTab.LoadValuesFromParameters();
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
                    m_HelpersConfig = CyHelperInfo.DeserializeHelpers(_SHelpers);
                    // Add used helper and symbol indexes to the list
                    for (int i = 0; i < m_HelpersConfig.Count; i++)
                    {
                        switch (m_HelpersConfig[i].Kind)
                        {
                            case CyHelperKind.SEGMENT_7:
                                m_HelperIndexes_7SEG.Add(m_HelpersConfig[i].m_GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_7SEG.Contains(
                                        m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum))
                                    {
                                        m_SymbolIndexes_7SEG.Add(m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum);
                                    }
                                }
                                //For compatibility remove the 8th segment in symbols if the one exists
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (m_HelpersConfig[i].HelpSegInfo[j].m_RelativePos == 7)
                                    {
                                        m_HelpersConfig[i].HelpSegInfo.RemoveAt(j--);
                                    }
                                }
                                m_HelpersConfig[i].SegmentCount = 7;
                                break;
                            case CyHelperKind.SEGMENT_14:
                                m_HelperIndexes_14SEG.Add(m_HelpersConfig[i].m_GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_14SEG.Contains(
                                        m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum))
                                    {
                                        m_SymbolIndexes_14SEG.Add(m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum);
                                    }
                                }
                                break;
                            case CyHelperKind.SEGMENT_16:
                                m_HelperIndexes_16SEG.Add(m_HelpersConfig[i].m_GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (
                                        !m_SymbolIndexes_16SEG.Contains(
                                        m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum))
                                    {
                                        m_SymbolIndexes_16SEG.Add(m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum);
                                    }
                                }
                                break;
                            case CyHelperKind.BAR:
                                m_HelperIndexes_BAR.Add(m_HelpersConfig[i].m_GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (!m_SymbolIndexes_BAR.Contains(m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum))
                                        m_SymbolIndexes_BAR.Add(m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum);
                                }
                                break;
                            case CyHelperKind.MATRIX:
                                m_HelperIndexes_MATRIX.Add(m_HelpersConfig[i].m_GlobalHelperIndex);
                                for (int j = 0; j < m_HelpersConfig[i].HelpSegInfo.Count; j++)
                                {
                                    if (
                                        !m_SymbolIndexes_MATRIX.Contains(
                                             m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum))
                                        m_SymbolIndexes_MATRIX.Add(m_HelpersConfig[i].HelpSegInfo[j].m_GlobalDigitNum);
                                }
                                break;
                            case CyHelperKind.EMPTY:
                                // For compatibility, add pixel's definitions if there are less than needed.
                                for (int j1 = 0; j1 < NumSegmentLines; j1++)
                                    for (int j2 = 0; j2 < NumCommonLines; j2++)
                                    {
                                        if (j1*NumCommonLines + j2 >= m_HelpersConfig[0].HelpSegInfo.Count)
                                        {
                                            m_HelpersConfig[0].HelpSegInfo.Add(new CyHelperSegmentInfo(
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
                    m_HelpersConfig = new List<CyHelperInfo>();
                    CyHelperInfo.CreateHelper(CyHelperKind.EMPTY, this);
                }
            }
            catch
            {
                MessageBox.Show(PARAMETERS_LOADING_ERROR_MSG, ERROR_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        m_inst.SetParamExpr(PARAM_DACDISINITTIME, m_DacDisInitTime.ToString());
                        break;
                    case PARAM_GANG:
                        m_inst.SetParamExpr(PARAM_GANG, Gang.ToString().ToLower());
                        break;
                    case PARAM_DEBUGMODE:
                        m_inst.SetParamExpr(PARAM_DEBUGMODE, DebugMode.ToString().ToLower());
                        break;
                    case PARAM_DISABLEDCOMMONS:
                        m_inst.SetParamExpr(PARAM_DISABLEDCOMMONS, m_DisabledCommons);
                        break;
                    case PARAM_HELPERS:
                        m_inst.SetParamExpr(PARAM_HELPERS, SerializedHelpers);
                        break;
                    case PARAM_PARAMCHANGED:
                        m_inst.SetParamExpr(PARAM_PARAMCHANGED, m_ParametersChanged.ToString());
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
                m_inst.SetParamExpr(PARAM_DACDISINITTIME, m_DacDisInitTime.ToString());
                m_inst.SetParamExpr(PARAM_GANG, Gang.ToString().ToLower());
                m_inst.SetParamExpr(PARAM_DEBUGMODE, DebugMode.ToString().ToLower());

                m_inst.SetParamExpr(PARAM_DISABLEDCOMMONS, m_DisabledCommons);
                m_inst.SetParamExpr(PARAM_HELPERS, SerializedHelpers);
                m_inst.SetParamExpr(PARAM_PARAMCHANGED, m_ParametersChanged.ToString());
            }
        }

        public void CommitParams()
        {
            if (m_inst != null)
            {
                if (!m_inst.CommitParamExprs())
                {
                    if (m_inst.GetCommittedParam(PARAM_NUMCOMMONLINES).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_NUMCOMMONLINES);
                    if (m_inst.GetCommittedParam(PARAM_NUMSEGMENTLINES).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_NUMSEGMENTLINES);
                    if (m_inst.GetCommittedParam(PARAM_BIASTYPE).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_BIASTYPE);
                    if (m_inst.GetCommittedParam(PARAM_WAVEFORMTYPE).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_WAVEFORMTYPE);
                    if (m_inst.GetCommittedParam(PARAM_FRAMERATE).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_FRAMERATE);
                    if (m_inst.GetCommittedParam(PARAM_BIASVOLTAGE).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_BIASVOLTAGE);
                    if (m_inst.GetCommittedParam(PARAM_DRIVERPOWERMODE).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_DRIVERPOWERMODE);
                    if (m_inst.GetCommittedParam(PARAM_HIDRIVETIME).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_HIDRIVETIME);
                    if (m_inst.GetCommittedParam(PARAM_LOWDRIVEMODE).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_LOWDRIVEMODE);
                    if (m_inst.GetCommittedParam(PARAM_LOWDRIVEINITTIME).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_LOWDRIVEINITTIME);
                    if (m_inst.GetCommittedParam(PARAM_CLOCKFREQUENCY).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_CLOCKFREQUENCY);
                    if (m_inst.GetCommittedParam(PARAM_GANG).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_GANG);
                    if (m_inst.GetCommittedParam(PARAM_DACDISINITTIME).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_DACDISINITTIME);
                    if (m_inst.GetCommittedParam(PARAM_DEBUGMODE).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_DEBUGMODE);
                    if (m_inst.GetCommittedParam(PARAM_HELPERS).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_HELPERS);
                    if (m_inst.GetCommittedParam(PARAM_DISABLEDCOMMONS).ErrorCount > 0)
                        MessageBox.Show(PARAMETERS_COMMIT_ERROR_MSG + PARAM_DISABLEDCOMMONS);
                }
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
        [XmlAttribute("Display")] public int m_DisplayNum;
        [XmlAttribute("Digit")] public int m_DigitNum;
        [XmlAttribute("Helper")] public CyHelperKind m_HelperType;
        [XmlAttribute("RelPos")] public int m_RelativePos;

        [XmlElement("GlobalDigitNum")]
        public int m_GlobalDigitNum;

        public CyHelperSegmentInfo()
        {
            m_HelperType = CyHelperKind.EMPTY;
        }

        public CyHelperSegmentInfo(string name, int common, int segment, int displayNum, int digitNum, int relPos,
                                   CyHelperKind type, int globalDigitNum) : base(name, common, segment)
        {
            m_DisplayNum = displayNum;
            m_DigitNum = digitNum;
            m_RelativePos = relPos;
            m_HelperType = type;

            m_GlobalDigitNum = globalDigitNum;
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
        public const int MAX_7SEG_SYMBOLS = 5;
        public const int MAX_14SEG_SYMBOLS = 20;
        public const int MAX_16SEG_SYMBOLS = 20;
        public const int MAX_BAR_SYMBOLS = 255;
        public const int MAX_MATRIX_SYMBOLS = 8;

        [XmlElement("Name")] 
        public string m_Name;

        private CyHelperKind m_Kind;
        private int m_SymbolsCount;
        private int m_MaxSymbolsCount;
        private int m_SegmentCount;
        private int m_DisplayNum;

        [XmlElementAttribute("Color")] 
        public int m_HelperColor;

        // m_GlobalHelperIndex is used for the names of the helpers. 
        // Each kind of helpers (7-segment, 14-segment, etc.) has its own numeration.
        [XmlElement("GlobalHelperIndex")] 
        public int m_GlobalHelperIndex;

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

        [XmlArray("HelpSegInfoArray")] [XmlArrayItem("HelperSegmentInfo")]
        public List<CyHelperSegmentInfo> HelpSegInfo;

        public int SymbolsCount
        {
            get { return m_SymbolsCount; }
            set { m_SymbolsCount = value; }
        }

        public CyHelperInfo()
        {
            HelpSegInfo = new List<CyHelperSegmentInfo>();

            m_Name = "Empty";
            m_Kind = CyHelperKind.EMPTY;
            SymbolsCount = 0;
            m_MaxSymbolsCount = 0;
            m_SegmentCount = 0;
        }

        public CyHelperInfo(string name, CyHelperKind kind, int maxSymbolsCount, int segmentCount, Color color,
                          int helperIndex)
        {
            m_Name = name;
            m_Kind = kind;
            m_MaxSymbolsCount = maxSymbolsCount;
            m_SegmentCount = segmentCount;
            SymbolsCount = 0;
            HelpSegInfo = new List<CyHelperSegmentInfo>();

            HelperColor = color;
            m_GlobalHelperIndex = helperIndex;
        }

        //--------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return m_Name;
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
                if ((HelpSegInfo[i].m_DigitNum == symbol) &&
                    (HelpSegInfo[i].m_RelativePos == segment))
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
                HelpSegInfo.Add(new CyHelperSegmentInfo(name.ToUpper(), com, seg, 0, SymbolsCount, i, Kind,
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
                                            parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.SEGMENT_14:
                    helper = new CyHelperInfo("Helper_14Segment_" + num, kind, MAX_14SEG_SYMBOLS, 14,
                                            parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.SEGMENT_16:
                    helper = new CyHelperInfo("Helper_16Segment_" + num, kind, MAX_16SEG_SYMBOLS, 16,
                                            parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.BAR:
                    helper = new CyHelperInfo("Helper_Bar_" + num, kind, MAX_BAR_SYMBOLS, 1,
                                            parameters.m_ColorChooser.PopCl(), num);
                    break;
                case CyHelperKind.MATRIX:
                    helper = new CyHelperInfo("Helper_Matrix_" + num, kind, MAX_MATRIX_SYMBOLS, 5*8,
                                            parameters.m_ColorChooser.PopCl(), num);
                    break;
                default:
                    helper = new CyHelperInfo();
                    break;
            }
            helper.DisplayNum = parameters.m_HelpersConfig.Count - 1;
            parameters.m_HelpersConfig.Add(helper);

            //Pixels management
            int NumCommonLines = Convert.ToInt32(parameters.NumCommonLines);
            int NumSegmentLines = Convert.ToInt32(parameters.NumSegmentLines);
            if (kind == CyHelperKind.EMPTY)
            {
                for (int j = 0; j < NumSegmentLines; j++)
                    for (int i = 0; i < NumCommonLines; i++)
                    {
                        helper.HelpSegInfo.Add(
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
            parameters.m_HelpersConfig[0].HelpSegInfo.Clear();
            for (int j = 0; j < NumSegmentLines; j++)
                for (int i = 0; i < NumCommonLines; i++)
                {
                    parameters.m_HelpersConfig[0].HelpSegInfo.Add(new CyHelperSegmentInfo(
                                                                     CyLCDParameters.UNASSIGNED_PIXEL_NAME + (j * NumCommonLines + i), i, j, -1, -1, -1,
                                                                      CyHelperKind.EMPTY, -1));
                }
        }

        //--------------------------------------------------------------------------------------------------------------

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
                if (parameters.m_HelpersConfig[i].m_Name == name)
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
                    while (parameters.m_HelperIndexes_7SEG.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_7SEG.Add(index);
                    break;
                case CyHelperKind.SEGMENT_14:
                    while (parameters.m_HelperIndexes_14SEG.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_14SEG.Add(index);
                    break;
                case CyHelperKind.SEGMENT_16:
                    while (parameters.m_HelperIndexes_16SEG.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_16SEG.Add(index);
                    break;
                case CyHelperKind.BAR:
                    while (parameters.m_HelperIndexes_BAR.Contains(index))
                        index++;
                    parameters.m_HelperIndexes_BAR.Add(index);
                    break;
                case CyHelperKind.MATRIX:
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
                case CyHelperKind.SEGMENT_7:
                    parameters.m_HelperIndexes_7SEG.Remove(index);
                    break;
                case CyHelperKind.SEGMENT_14:
                    parameters.m_HelperIndexes_14SEG.Remove(index);
                    break;
                case CyHelperKind.SEGMENT_16:
                    parameters.m_HelperIndexes_16SEG.Remove(index);
                    break;
                case CyHelperKind.BAR:
                    parameters.m_HelperIndexes_BAR.Remove(index);
                    break;
                case CyHelperKind.MATRIX:
                    parameters.m_HelperIndexes_MATRIX.Remove(index);
                    break;
                default:
                    break;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static string SerializeHelpers(List<CyHelperInfo> list)
        {
            ArrayList helpers = new ArrayList(list);
            Type[] theExtraTypes = new Type[2];
            theExtraTypes[0] = typeof (CyHelperInfo);
            theExtraTypes[1] = typeof (ArrayList);
            XmlSerializer s = new XmlSerializer(typeof (ArrayList), theExtraTypes);
            StringWriter sw = new StringWriter();
            s.Serialize(sw, helpers);
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