/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SPDIF_Tx_v1_10
{
    public class CyChannelSettings
    {
        private CySPDifTxParameters m_params;
        private CyEChannel m_chIndex;

        #region Constructor(s)
        public CyChannelSettings(CySPDifTxParameters param, CyEChannel channel)
        {
            m_params = param;
            m_chIndex = channel;
        }
        #endregion

        #region Private method(s)
        private string GetChannelSufix()
        {
            return m_chIndex.ToString();
        }
        #endregion

        #region Class Properties
        public CyEDataType DataTypeCh
        {
            get
            {
                return m_params.GetValue<CyEDataType>(CyParamNames.DATA_TYPE + GetChannelSufix());
            }
            set
            {
                m_params.SetValue(CyParamNames.DATA_TYPE + GetChannelSufix(), value);
                StatusDataCh = GetHexStatusDataCh();
            }
        }

        public CyECopyrightType CopyrightCh
        {
            get
            {
                return m_params.GetValue<CyECopyrightType>(CyParamNames.COPYRIGHT + GetChannelSufix());
            }
            set
            {
                m_params.SetValue(CyParamNames.COPYRIGHT + GetChannelSufix(), value);
                StatusDataCh = GetHexStatusDataCh();
            }
        }

        public CyEPreEmphasisType PreEmphasisCh
        {
            get
            {
                return m_params.GetValue<CyEPreEmphasisType>(CyParamNames.PRE_EMPHASIS + GetChannelSufix());
            }
            set
            {
                m_params.SetValue(CyParamNames.PRE_EMPHASIS + GetChannelSufix(), value);
                StatusDataCh = GetHexStatusDataCh();
            }
        }

        public CyECategoryType CategoryCh
        {
            get
            {
                return m_params.GetValue<CyECategoryType>(CyParamNames.CATEGORY + GetChannelSufix());
            }
            set
            {
                m_params.SetValue(CyParamNames.CATEGORY + GetChannelSufix(), value);
                StatusDataCh = GetHexStatusDataCh();
            }
        }

        public CyEClockAccuracyType ClockAccuracyCh
        {
            get
            {
                return m_params.GetValue<CyEClockAccuracyType>(CyParamNames.CLOCK_ACCURACY + GetChannelSufix());
            }
            set
            {
                m_params.SetValue(CyParamNames.CLOCK_ACCURACY + GetChannelSufix(), value);
                StatusDataCh = GetHexStatusDataCh();
            }
        }

        public CyESourceNumberType SourceNumberCh
        {
            get
            {
                return m_params.GetValue<CyESourceNumberType>(CyParamNames.SOURCE_NUMBER + GetChannelSufix());
            }
            set
            {
                m_params.SetValue(CyParamNames.SOURCE_NUMBER + GetChannelSufix(), value);
                StatusDataCh = GetHexStatusDataCh();
            }
        }

        public CyEChannelNumberType ChannelNumberCh
        {
            get
            {
                return m_params.GetValue<CyEChannelNumberType>(CyParamNames.CHANNEL_NUMBER + GetChannelSufix());
            }
            set
            {
                m_params.SetValue(CyParamNames.CHANNEL_NUMBER + GetChannelSufix(), value);
                StatusDataCh = GetHexStatusDataCh();
            }
        }

        public string StatusDataCh
        {
            set
            {
                m_params.SetValue(CyParamNames.STATUS_DATA + GetChannelSufix(), value);
            }
        }
        #endregion

        #region Collecting Status Data Array
        public string GetHexStatusDataCh()
        {
            const string HEX_PREFIX = "0x";
            string resultStr = string.Empty;

            // Create zero byte
            byte byte0 = (byte)DataTypeCh.GetHashCode();
            byte0 += (byte)CopyrightCh.GetHashCode();
            byte0 += (byte)PreEmphasisCh.GetHashCode();
            string str0 = (byte0 > 10) ? HEX_PREFIX + byte0.ToString("X") : HEX_PREFIX + "0" + byte0.ToString("X");

            // Create first byte
            byte byte1 = (byte)CategoryCh.GetHashCode();
            string str1 = (byte1 > 10) ? HEX_PREFIX + byte1.ToString("X") : HEX_PREFIX + "0" + byte1.ToString("X");

            // Create second byte
            byte byte2 = (byte)SourceNumberCh.GetHashCode();
            byte2 += (byte)ChannelNumberCh.GetHashCode();
            string str2 = (byte2 > 10) ? HEX_PREFIX + byte2.ToString("X") : HEX_PREFIX + "0" + byte2.ToString("X");

            // Create third byte
            byte byte3 = (byte)ClockAccuracyCh.GetHashCode();
            byte3 += (byte)m_params.Frequency.GetHashCode();
            string str3 = (byte3 > 10) ? HEX_PREFIX + byte3.ToString("X") : HEX_PREFIX + "0" + byte3.ToString("X");

            // Create fouth byte
            string str4 = (m_params.DataBits == 24) ? "0x0B" : "0x00";

            // Concatenate calculated bytes and add the tail with zero bytes
            resultStr = str0 + "," + str1 + "," + str2 + "," + str3 + "," + str4 + ",0x00,0x00,0x00,0x00,0x00,0x00,"
                + "0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00";
            return resultStr;
        }
        #endregion
    }
}
