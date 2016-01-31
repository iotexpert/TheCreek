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

namespace GlitchFilter_v2_0
{
    public class CyParamNames
    {
        public const string BYPASS_FILTER = "BypassFilter";
        public const string GLITCH_LENGTH = "GlitchLength";
        public const string SIGNAL_WIDTH = "SignalWidth";
    }

    public class CyParamRanges
    {
        public const byte MIN_SIGNAL_WIDTH = 1;
        public const byte MAX_SIGNAL_WIDTH = 24;

        public const UInt16 MIN_GLITCH_LENGTH = 1;
        public const UInt16 MAX_GLITCH_LENGTH = 256;
    }

    public enum CyEBypassFilterType { NONE, LOGIC_ONE, LOGIC_ZERO }

    public class CyParameters
    {
        public const string CLOCK_NAME = "clock";

        const string PSOC5A_ARCH_FAMILY_MEMBER = "PSoC5A";

        public ICyInstQuery_v1 m_inst;
        public ICyTerminalQuery_v1 m_term;

        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        private bool m_bGlobalEditMode = false;

        public bool GlobalEditMode
        {
            get { return m_bGlobalEditMode; }
            set { m_bGlobalEditMode = value; }
        }

        #region Constructor(s)
        public CyParameters(ICyInstQuery_v1 inst)
        {
            m_inst = inst;
        }
        #endregion

        #region Getting Parameters
        private T GetValue<T>(string paramName)
        {
            try
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
            catch { }
            return default(T);
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
        #endregion

        #region Class properties
        public UInt16 GlitchLength
        {
            get { return GetValue<UInt16>(CyParamNames.GLITCH_LENGTH); }
            set { SetValue(CyParamNames.GLITCH_LENGTH, value); }
        }

        public CyEBypassFilterType BypassFilterType
        {
            get { return GetValue<CyEBypassFilterType>(CyParamNames.BYPASS_FILTER); }
            set { SetValue(CyParamNames.BYPASS_FILTER, value); }
        }

        public byte SignalWidth
        {
            get { return GetValue<byte>(CyParamNames.SIGNAL_WIDTH); }
            set { SetValue(CyParamNames.SIGNAL_WIDTH, value); }
        }

        public bool IsPSoC5A
        {
            get
            {
                return m_inst.DeviceQuery.IsPSoC5 && m_inst.DeviceQuery.ArchFamilyMember == PSOC5A_ARCH_FAMILY_MEMBER;
            }
        }
        #endregion

        #region Auxiliary operations
        public static double GetExternalClock(ICyTerminalQuery_v1 termQuery, string clockName)
        {
            List<CyClockData> clkdata = termQuery.GetClockData(clockName, 0);
            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0].Frequency * Math.Pow(10, clkdata[0].UnitAsExponent);
                }
            }
            return (-1);
        }
        #endregion

        #region DRCs
        public bool CheckGlitchLength(ICyDeviceQuery_v1 deviceQuery)
        {
            bool res = true;
            if (GlitchLength > 8 && IsPSoC5A)
            {
                res = false;
            }
            return res;
        }
        #endregion
    }
}
