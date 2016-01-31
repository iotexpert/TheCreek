/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace GlitchFilter_v2_0
{
    public partial class CyGeneralTab : UserControl, ICyParamEditingControl
    {
        protected CyParameters m_params = null;
        protected ErrorProvider m_errorProvider = null;

        private readonly string[] SECONDS_PREFIXES = new string[]
        {
            "", // seconds
            "m", // milliseconds
            "u", // microseconds
            "n" // nanoseconds
        };

        public string TabName
        {
            get { return "Basic"; }
        }

        #region Constructor(s)
        public CyGeneralTab(CyParameters parameters)
        {
            m_params = parameters;

            InitializeComponent();

            #region Add event handlers
            m_numSignalWidth.TextChanged += new EventHandler(m_num_TextChanged);
            m_numGlitchLength.TextChanged += new EventHandler(m_num_TextChanged);

            m_rbNone.CheckedChanged += new EventHandler(m_rb_CheckedChanged);
            m_rbLogicZero.CheckedChanged += new EventHandler(m_rb_CheckedChanged);
            m_rbLogicOne.CheckedChanged += new EventHandler(m_rb_CheckedChanged);
            #endregion

            m_errorProvider = new ErrorProvider();
            m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            Load += delegate(object sender, EventArgs e)
            {
                this.Dock = DockStyle.Fill;
                this.AutoScroll = true;
            };

            UpdateUI();
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            string errorMessage = string.Empty;

            if (m_errorProvider != null)
            {
                // Check controls for errors
                foreach (Control control in this.Controls)
                {
                    errorMessage = m_errorProvider.GetError(control);
                    if (string.IsNullOrEmpty(errorMessage) == false)
                        errs.Add(new CyCustErr(errorMessage));

                    // Check controls inside groupbox
                    foreach (Control internalControl in control.Controls)
                    {
                        errorMessage = m_errorProvider.GetError(internalControl);
                        if (string.IsNullOrEmpty(errorMessage) == false)
                            errs.Add(new CyCustErr(errorMessage));
                    }
                }
            }

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(TabName))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }
            }

            return errs;
        }
        #endregion

        #region Update UI
        public void UpdateUI()
        {
            m_numSignalWidth.Value = m_params.SignalWidth;
            m_numGlitchLength.Value = m_params.GlitchLength;

            switch (m_params.BypassFilterType)
            {
                case CyEBypassFilterType.NONE:
                    m_rbNone.Checked = true;
                    break;
                case CyEBypassFilterType.LOGIC_ZERO:
                    m_rbLogicZero.Checked = true;
                    break;
                case CyEBypassFilterType.LOGIC_ONE:
                    m_rbLogicOne.Checked = true;
                    break;
            }

            UpdateClockDependentData();
        }

        public void UpdateClockDependentData()
        {
            UpdateClockDependentData(m_params.m_inst, m_params.m_term);
        }

        public void UpdateClockDependentData(ICyInstQuery_v1 instQuery, ICyTerminalQuery_v1 termQuery)
        {
            double clkFreq = CyParameters.GetExternalClock(termQuery, CyParameters.CLOCK_NAME);

            if (clkFreq != -1)
            {
                // Read number of samples
                CyCompDevParam glitchLengthValue = instQuery.GetCommittedParam(CyParamNames.GLITCH_LENGTH);
                UInt16 glitchLength = UInt16.Parse(glitchLengthValue.Value);

                m_lblGlitchPulseLength.Text = GetNormalizedTimeString(glitchLength / clkFreq);
            }
            else
            {
                m_lblGlitchPulseLength.Text = "N/A";
            }
        }

        private string GetNormalizedTimeString(double glitchPulseLength)
        {
            double res = glitchPulseLength;

            for (int i = 0; i < SECONDS_PREFIXES.Length; i++)
            {
                if (Math.Truncate(res) > 0)
                {
                    return string.Format("{0:0.##} {1}s", res, SECONDS_PREFIXES[i]);
                }
                res *= 1000;
            }

            return string.Empty;
        }
        #endregion

        #region Event handlers
        void m_num_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown num = sender as NumericUpDown;

            if (num != null)
            {
                if (num == m_numSignalWidth)
                {
                    byte value;

                    if (byte.TryParse(num.Text, out value) && value >= CyParamRanges.MIN_SIGNAL_WIDTH &&
                        value <= CyParamRanges.MAX_SIGNAL_WIDTH)
                    {
                        m_errorProvider.SetError(num, string.Empty);
                        m_params.SignalWidth = value;
                    }
                    else
                    {
                        m_errorProvider.SetError(num, string.Format(Resources.SignalWidthValueErrorDescription,
                            CyParamRanges.MIN_SIGNAL_WIDTH, CyParamRanges.MAX_SIGNAL_WIDTH));
                    }
                }
                else if (num == m_numGlitchLength)
                {
                    UInt16 value;

                    if (UInt16.TryParse(num.Text, out value) && value >= CyParamRanges.MIN_GLITCH_LENGTH &&
                        value <= CyParamRanges.MAX_GLITCH_LENGTH)
                    {
                        m_errorProvider.SetError(num, string.Empty);
                        m_params.GlitchLength = value;
                        UpdateClockDependentData();
                    }
                    else
                    {
                        m_errorProvider.SetError(num, string.Format(Resources.GlitchLengthValueErrorDescription,
                            CyParamRanges.MIN_GLITCH_LENGTH, CyParamRanges.MAX_GLITCH_LENGTH));
                    }
                }
            }
        }

        void m_rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null)
            {
                if (rb == m_rbNone)
                {
                    m_params.BypassFilterType = CyEBypassFilterType.NONE;
                }
                else if (rb == m_rbLogicZero)
                {
                    m_params.BypassFilterType = CyEBypassFilterType.LOGIC_ZERO;
                }
                else if (rb == m_rbLogicOne)
                {
                    m_params.BypassFilterType = CyEBypassFilterType.LOGIC_ONE;
                }
            }
        }
        #endregion
    }
}
