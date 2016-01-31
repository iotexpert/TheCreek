/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SPDIF_Tx_v1_10
{
    #region Symbol Parameter Names
    public class CyParamNames
    {
        // Basic tab parameters
        public const string DATA_BITS = "DataBits";
        public const string DATA_INTERLEAVING = "DataInterleaving";
        public const string MANAGED_DMA = "ManagedDma";

        // Channel Status common parameters
        public const string STATUS_DATA = "StatusData";
        public const string DATA_TYPE = "DataType";
        public const string COPYRIGHT = "Copyright";
        public const string PRE_EMPHASIS = "PreEmphasis";
        public const string CATEGORY = "Category";
        public const string CLOCK_ACCURACY = "ClockAccuracy";
        public const string SOURCE_NUMBER = "SourceNumber";
        public const string CHANNEL_NUMBER = "ChannelNumber";

        // Channel 0 Status tab parameters
        public const string SAMPLE_FREQUENCY = "SampleFrequency";

        // Channel 1 Status tab parameters
        public const string COPY_DEFAULTS = "CopyDefaults";
    }
    #endregion

    #region Symbol Types
    public enum CyESampleFrequencyType
    {
        SPS_UNKNOWN = 1,
        SPS_22_KHz = 4,
        SPS_24_KHz = 6,
        SPS_32_KHz = 3,
        SPS_44_KHz = 0,
        SPS_48_KHz = 2,
        SPS_64_KHz = 11,
        SPS_88_KHz = 8,
        SPS_96_KHz = 10,
        SPS_192_KHz = 14
    };
    public enum CyEDataType { PCM = 0, Other = 2 };
    public enum CyECopyrightType { Copyrighted = 0, NotCopyrighted = 4 };
    public enum CyEPreEmphasisType { NoPreemphasis = 0, Preemphasis50 = 8 };
    public enum CyECategoryType { CategoryGen = 0, CategoryD2D = 2 };
    public enum CyEClockAccuracyType { ClkLevel1 = 16, ClkLevel2 = 0, ClkLevel3 = 32 };
    public enum CyESourceNumberType
    {
        SrcNum00 = 0,
        SrcNum01 = 1,
        SrcNum02 = 2,
        SrcNum03 = 3,
        SrcNum04 = 4,
        SrcNum05 = 5,
        SrcNum06 = 6,
        SrcNum07 = 7,
        SrcNum08 = 8,
        SrcNum09 = 9,
        SrcNum10 = 10,
        SrcNum11 = 11,
        SrcNum12 = 12,
        SrcNum13 = 13,
        SrcNum14 = 14,
        SrcNum15 = 15
    };
    public enum CyEChannelNumberType
    {
        ChNum00 = 0,
        ChNum01 = 16,
        ChNum02 = 32,
        ChNum03 = 48,
        ChNum04 = 64,
        ChNum05 = 80,
        ChNum06 = 96,
        ChNum07 = 112,
        ChNum08 = 128,
        ChNum09 = 144,
        ChNum10 = 160,
        ChNum11 = 176,
        ChNum12 = 192,
        ChNum13 = 208,
        ChNum14 = 224,
        ChNum15 = 240
    };
    #endregion

    #region Class Enum(s)
    public enum CyEChannel { Ch0, Ch1 };
    #endregion

    public class CySPDifTxParameters
    {
        #region Public members
        public ICyInstEdit_v1 m_inst;

        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        public bool m_globalEditMode = false;

        // User Controls
        public CySPDifTxBasicTab m_basicTab = null;
        public CyChannel0StatusTab m_channel0StatusTab = null;
        public CyChannel1StatusTab m_channel1StatusTab = null;

        // Lists contain display names of types taken from symbol, lists are used to fill comboboxes
        public List<string> m_frequencyList;
        public List<string> m_dataTypeList;
        public List<string> m_copyrightList;
        public List<string> m_preEmphasisList;
        public List<string> m_categoryList;
        public List<string> m_clockAccuracyList;

        // Dictionary is used to keep display names of types taken from symbol,
        // and associate them with the enum fields to simplify access to diaplay names
        public Dictionary<object, string> m_dnDict = new Dictionary<object, string>();
        public Dictionary<object, string> m_dnSrcNumDict = new Dictionary<object, string>();
        public Dictionary<object, string> m_dnChNumDict = new Dictionary<object, string>();

        public List<CyChannelSettings> m_channels = new List<CyChannelSettings>();
        #endregion

        #region Constructor(s)
        public CySPDifTxParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;

            // Creating channels
            m_channels.Add(new CyChannelSettings(this, CyEChannel.Ch0));
            m_channels.Add(new CyChannelSettings(this, CyEChannel.Ch1));

            // Getting symbol types values
            m_frequencyList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.SAMPLE_FREQUENCY));
            m_dataTypeList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.DATA_TYPE
                + CyEChannel.Ch0.ToString()));
            m_copyrightList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.COPYRIGHT
                + CyEChannel.Ch0.ToString()));
            m_preEmphasisList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.PRE_EMPHASIS
                + CyEChannel.Ch0.ToString()));
            m_categoryList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.CATEGORY
                + CyEChannel.Ch0.ToString()));
            m_clockAccuracyList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.CLOCK_ACCURACY
                + CyEChannel.Ch0.ToString()));
            List<string> m_sourceNumberList = new List<string>(inst.GetPossibleEnumValues(
                CyParamNames.SOURCE_NUMBER + CyEChannel.Ch0.ToString()));
            List<string> m_channelNumberList = new List<string>(inst.GetPossibleEnumValues(
                CyParamNames.CHANNEL_NUMBER + CyEChannel.Ch0.ToString()));

            // Adding display names to the dictionary to easily operate with enums
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyESampleFrequencyType), m_frequencyList);
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyEDataType), m_dataTypeList);
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyECopyrightType), m_copyrightList);
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyEPreEmphasisType), m_preEmphasisList);
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyECategoryType), m_categoryList);
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyEClockAccuracyType), m_clockAccuracyList);
            CyDictParser.FillDictionary(ref m_dnSrcNumDict, typeof(CyESourceNumberType), m_sourceNumberList);
            CyDictParser.FillDictionary(ref m_dnChNumDict, typeof(CyEChannelNumberType), m_channelNumberList);
        }
        #endregion

        #region Class properties

        #region Basic tab properties
        public byte DataBits
        {
            get
            {
                return GetValue<byte>(CyParamNames.DATA_BITS);
            }
            set
            {
                SetValue(CyParamNames.DATA_BITS, value);
                m_channels[0].StatusDataCh = m_channels[0].GetHexStatusDataCh();
                m_channels[1].StatusDataCh = m_channels[1].GetHexStatusDataCh();
            }
        }

        public bool DataInterleaving
        {
            get
            {
                return GetValue<bool>(CyParamNames.DATA_INTERLEAVING);
            }
            set
            {
                SetValue(CyParamNames.DATA_INTERLEAVING, value);
            }
        }

        public bool ManagedDma
        {
            get
            {
                return GetValue<bool>(CyParamNames.MANAGED_DMA);
            }
            set
            {
                SetValue(CyParamNames.MANAGED_DMA, value);
            }
        }
        #endregion

        #region Channel 0 Status tab properties
        public CyESampleFrequencyType Frequency
        {
            get
            {
                return GetValue<CyESampleFrequencyType>(CyParamNames.SAMPLE_FREQUENCY);
            }
            set
            {
                SetValue(CyParamNames.SAMPLE_FREQUENCY, value);
                m_channels[0].StatusDataCh = m_channels[0].GetHexStatusDataCh();
                if (CopyDefaults)
                {
                    m_channels[1].StatusDataCh = m_channels[1].GetHexStatusDataCh();
                }
            }
        }
        #endregion

        #region Channel 1 Status tab properties
        public bool CopyDefaults
        {
            get
            {
                return GetValue<bool>(CyParamNames.COPY_DEFAULTS);
            }
            set
            {
                SetValue(CyParamNames.COPY_DEFAULTS, value);
            }
        }
        #endregion

        #endregion

        #region Getting Parameters
        public T GetValue<T>(string paramName)
        {
            T value;
            CyCompDevParam param = m_inst.GetCommittedParam(paramName);
            if (param != null)
            {
                param.TryGetValueAs<T>(out value);
                bool err = new List<string>(param.Errors).Count > 0;
                if (err == false)
                {
                    return value;
                }
            }
            else
            {
                Debug.Assert(false);
            }
            return default(T);
        }

        public void LoadParameters()
        {
            m_globalEditMode = false;
            m_basicTab.UpdateUI();
            m_channel0StatusTab.UpdateUI();
            m_channel1StatusTab.UpdateUI();
            m_globalEditMode = true;
        }
        #endregion

        #region Setting Parameters
        public void SetValue<T>(string paramName, T value)
        {
            if (m_globalEditMode)
            {
                if (value is bool)
                    m_inst.SetParamExpr(paramName, value.ToString().ToLower());
                else
                    try
                    {
                        m_inst.SetParamExpr(paramName, value.ToString());
                    }
                    catch
                    {
                        Debug.Assert(false);
                    }

                m_inst.CommitParamExprs();
            }
        }
        #endregion
    }
}
