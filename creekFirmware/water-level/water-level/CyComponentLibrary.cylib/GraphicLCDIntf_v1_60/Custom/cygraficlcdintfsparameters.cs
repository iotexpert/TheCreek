/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Drawing;

namespace GraphicLCDIntf_v1_60
{
    #region Component Parameter Names
    public class CyParamNames
    {
        public const string BIT_WIDTH = "BitWidth";
        public const string READ_HI_PULSE = "ReadHiPulse";
        public const string READ_LO_PULSE = "ReadLoPulse";
    }
    #endregion

    #region Parameters Range Constants
    public class CyParamRanges
    {
        public const int READ_HI_PULSE_MIN = 1;
        public const int READ_HI_PULSE_MAX = 255;
        public const int READ_LO_PULSE_MIN = 2;
        public const int READ_LO_PULSE_MAX = 255;
    }
    #endregion

    public class CyGraphicLCDIntfParameters
    {
        public ICyInstEdit_v1 m_inst;
        public CyGraphicLCDIntfBasicTab m_basicTab;
        public double m_clock;
        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        public bool m_bGlobalEditMode = false;

        #region Component Parameters
        private int m_bitWidth;
        private int m_readHiPulse;
        private int m_readLoPulse;
        #endregion

        #region Constructor(s)
        public CyGraphicLCDIntfParameters(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery)
        {
            this.m_inst = inst;
            m_clock = GetComponentClockInMHz(termQuery);
        }
        #endregion

        #region Class Properties
        public int BitWidth
        {
            get { return m_bitWidth; }
            set { m_bitWidth = value; }
        }

        public int ReadHiPulse
        {
            get { return m_readHiPulse; }
            set
            {
                if (value >= CyParamRanges.READ_HI_PULSE_MIN && value <= CyParamRanges.READ_HI_PULSE_MAX)
                { m_readHiPulse = value; }
            }
        }

        public int ReadLoPulse
        {
            get { return m_readLoPulse; }
            set
            {
                if (value >= CyParamRanges.READ_LO_PULSE_MIN && value <= CyParamRanges.READ_LO_PULSE_MAX)
                { m_readLoPulse = value; }
            }
        }
        #endregion

        #region Getting parameters
        public void GetParams(ICyInstEdit_v1 inst)
        {
            int tmpPropValue = 0;
            if (int.TryParse(inst.GetCommittedParam(CyParamNames.BIT_WIDTH).Value, out tmpPropValue))
                this.BitWidth = tmpPropValue;
            if (int.TryParse(inst.GetCommittedParam(CyParamNames.READ_HI_PULSE).Value, out tmpPropValue))
                this.ReadHiPulse = tmpPropValue;
            if (int.TryParse(inst.GetCommittedParam(CyParamNames.READ_LO_PULSE).Value, out tmpPropValue))
                this.ReadLoPulse = tmpPropValue;

            m_basicTab.GetParams();
        }
        #endregion

        #region Setting parameters
        public void SetParams(string paramName)
        {
            if (m_bGlobalEditMode)
            {
                object value = null;
                switch (paramName)
                {
                    case CyParamNames.BIT_WIDTH:
                        value = m_bitWidth;
                        break;
                    case CyParamNames.READ_HI_PULSE:
                        value = m_readHiPulse;
                        break;
                    case CyParamNames.READ_LO_PULSE:
                        value = m_readLoPulse;
                        break;
                    default:
                        return;
                }
                m_inst.SetParamExpr(paramName, value.ToString());
                m_inst.CommitParamExprs();
            }
        }
        #endregion

        #region Calculations
        /// <summary>
        /// Returns component clock value from in KHz.
        /// </summary>
        public double GetComponentClockInMHz(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData("clock", 0);
            if (clkdata[0].IsFrequencyKnown)
            {
                double infreq = clkdata[0].Frequency;
                switch (clkdata[0].Unit)
                {
                    case CyClockUnit.kHz: infreq = infreq * 1000;
                        break;
                    case CyClockUnit.MHz: infreq = infreq * 1000000;
                        break;
                    default:
                        break;
                }
                return clkdata[0].Frequency;
            }
            return (-1);
        }

        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            m_clock = GetComponentClockInMHz(termQuery);
            m_basicTab.ShowImage();
        }
        #endregion
    }
}
