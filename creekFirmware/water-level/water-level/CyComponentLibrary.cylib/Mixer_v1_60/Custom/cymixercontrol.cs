/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;

namespace Mixer_v1_60
{
    public partial class CyMixerControl : UserControl
    {
        public ICyInstEdit_v1 m_InstEdit = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        const string lo = "LO";

        public CyMixerControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery)
        {
            m_InstEdit = inst;
            m_TermQuery = termquery;
            InitializeComponent();
            

            // Mixer Power
            IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues(cymixerparameters.POWER);
            foreach (string str in PowerEnums)
            {
                m_cbPower.Items.Add(str);
            }

            // Mixer Type
            IEnumerable<string> MixerTypeEnums = inst.GetPossibleEnumValues(cymixerparameters.MIXER_TYPE);
            foreach (string str in MixerTypeEnums)
            {
                m_cbMixerType.Items.Add(str);
            }

            HookAllEvents();
            if (m_InstEdit != null)
            {
                UpdateFormFromParams(inst);
            }
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookAllEvents();
            cymixerparameters prms = new cymixerparameters(inst);

            //Set the Mixer power
            string paramPower = m_InstEdit.ResolveEnumIdToDisplay(cymixerparameters.POWER, 
                prms.Power.Expr);
            if (m_cbPower.Items.Contains(paramPower))
            {
                m_cbPower.SelectedItem = paramPower;
                m_cbPower.Text = prms.Power.Expr;
            }

            /* set the Mixer Type */
            string paramMixerType = m_InstEdit.ResolveEnumIdToDisplay(cymixerparameters.MIXER_TYPE, 
                prms.Mixer_Type.Expr);
            if (m_cbMixerType.Items.Contains(paramMixerType))
            {
                m_cbMixerType.SelectedItem = paramMixerType;
                m_cbMixerType.Text = prms.Mixer_Type.Expr;
            }

            // Update LO_Source radio buttons
            if (m_InstEdit.ResolveEnumIdToDisplay(cymixerparameters.LO_SOURCE, prms.LO_Source.Expr) ==
                cymixerparameters.INTERNAL_LO)
            {
                m_rbInternal_LO.Checked = true;
                m_rbExternal_LO.Checked = false;
                m_tbLO_Frequency.ReadOnly = false;
                label2.Visible = false;
            }
            else
            {
                m_rbInternal_LO.Checked = false;
                m_rbExternal_LO.Checked = true;
                m_tbLO_Frequency.ReadOnly = true;
                label2.Visible = true;
                
            }
            m_tbLO_Frequency_Validating(this, new CancelEventArgs());

            HookAllEvents();
        }

        private void HookAllEvents()
        {
            this.m_cbMixerType.SelectedIndexChanged += new System.EventHandler(this.m_cbMixerType_SelectedIndexChanged);
            this.m_cbPower.SelectedIndexChanged += new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
            this.m_rbInternal_LO.CheckedChanged += new System.EventHandler(this.m_rbInternal_LO_CheckedChanged);
            this.m_rbExternal_LO.CheckedChanged += new System.EventHandler(this.m_rbExternal_LO_CheckedChanged);
            this.m_tbLO_Frequency.TextChanged += new EventHandler(m_tbLO_Frequency_TextChanged);
            this.m_tbLO_Frequency.Validating += new CancelEventHandler(m_tbLO_Frequency_Validating);
        }

        private void UnhookAllEvents()
        {
            this.m_cbMixerType.SelectedIndexChanged -= new System.EventHandler(this.m_cbMixerType_SelectedIndexChanged);
            this.m_cbPower.SelectedIndexChanged -= new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
            this.m_rbInternal_LO.CheckedChanged -= new System.EventHandler(this.m_rbInternal_LO_CheckedChanged);
            this.m_rbExternal_LO.CheckedChanged -= new System.EventHandler(this.m_rbExternal_LO_CheckedChanged);
            this.m_tbLO_Frequency.TextChanged -= new EventHandler(m_tbLO_Frequency_TextChanged);
            this.m_tbLO_Frequency.Validating -= new CancelEventHandler(m_tbLO_Frequency_Validating);
        }

        void m_tbLO_Frequency_Validating(object sender, CancelEventArgs e)
        {
            double LO_ENTERED = 0;
            try
            {
                LO_ENTERED = double.Parse(m_tbLO_Frequency.Text);
            }
            catch (Exception )
            {
                LO_ENTERED = 0;
            }
            LO_ENTERED = LO_ENTERED * 1000.00; // LO Frequency in Hz.

            if (m_cbMixerType.SelectedIndex == cymixerparameters.MIXERTYPE_UP)
            {
                if (LO_ENTERED > cymixerparameters.LO_LIMIT_UPMIXER)
                {
                    string errorMessage = "The LO Frequency should be less than 1MHz";
                    m_errorProvider.SetError(m_tbLO_Frequency, errorMessage);
                }
                else
                {
                    m_errorProvider.SetError(m_tbLO_Frequency, string.Empty);
                }
            }
            else
            {
                if (LO_ENTERED > cymixerparameters.LO_LIMIT_DOWNMIXER)
                {
                    string errorMessage = "The LO Frequency should be less than 4MHz";
                    m_errorProvider.SetError(m_tbLO_Frequency, errorMessage);
                }
                else
                {
                    m_errorProvider.SetError(m_tbLO_Frequency, string.Empty);
                }
            }
        }

        private void m_cbMixerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId(cymixerparameters.MIXER_TYPE, m_cbMixerType.Text);
            SetAParameter(cymixerparameters.MIXER_TYPE, prm, true);
            m_tbLO_Frequency_Validating(this, new CancelEventArgs());
        }

        private void m_cbPower_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId(cymixerparameters.POWER,
                m_cbPower.Text);
            SetAParameter(cymixerparameters.POWER, prm, true);
        }

        private void SetAParameter(string parameter, string value, bool checkFocus)
        {
            if (this.ContainsFocus || !checkFocus)
            {
                //Verify that the parameter was set correctly.
                m_InstEdit.SetParamExpr(parameter, value);
                m_InstEdit.CommitParamExprs();
                if (m_InstEdit.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_InstEdit.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    string errorMessage = string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}",
                        parameter, value, errors);
                }
            }
           
        }

        void m_tbLO_Frequency_TextChanged(object sender, EventArgs e)
        {
            /* Set LO Frequency .*/
            cymixerparameters prms = new cymixerparameters(m_InstEdit);
            SetAParameter(cymixerparameters.LO_CLOCK_FREQ, m_tbLO_Frequency.Text.ToString(), false);
            m_tbLO_Frequency_Validating(this, new CancelEventArgs());
        }

        public void SetActual_Frequency(string result)
        {
            m_InstEdit.SetParamExpr(cymixerparameters.LO_CLOCK_FREQ, result);
            m_InstEdit.CommitParamExprs();
        }

        private void m_rbInternal_LO_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbInternal_LO.Checked)
            {
                SetAParameter(cymixerparameters.LO_SOURCE, cymixerparameters.INTERNAL_LO, true);
                m_tbLO_Frequency.ReadOnly = false;
                label2.Visible = false;
            }
        }

        private void m_rbExternal_LO_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbExternal_LO.Checked)
            {
                SetAParameter(cymixerparameters.LO_SOURCE, cymixerparameters.EXTERNAL_LO, true);
                string frequency = GetClockFreq();
                if (!(string.Equals(frequency, "0")))
                {
                    m_tbLO_Frequency.ReadOnly = true;
                    m_tbLO_Frequency.Text = frequency;
                    m_InstEdit.SetParamExpr(cymixerparameters.LO_CLOCK_FREQ, frequency);
                    m_InstEdit.CommitParamExprs();
                    label2.Visible = false;
                }
                else
                {
                    m_tbLO_Frequency.ReadOnly = false;
                    label2.Visible = true;
                }

            }
        }

        public string GetClockFreq()
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = m_TermQuery.GetClockData(lo, 0);
            if (clkdata[0].IsFrequencyKnown)
            {
                double infreq = clkdata[0].Frequency;
                switch (clkdata[0].Unit)
                {
                    case CyClockUnit.MHz: infreq = infreq * 1000; break;
                    case CyClockUnit.Hz: infreq = infreq / 1000; break;
                    case CyClockUnit.kHz: break;
                }
                return Convert.ToString(Math.Round(infreq, 4));
            }
            else
            {
                 return Convert.ToString(0);
            }
        }

        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            cymixerparameters prms = new cymixerparameters(m_InstEdit);

            if (m_rbInternal_LO.Checked)
            {
                
                m_tbLO_Frequency.Text = prms.LO_clock_freq.Value;
            }
            else
            {
                string frequency = GetClockFreq();
                if (string.Equals(frequency, "0"))
                {
                    m_tbLO_Frequency.Text = prms.LO_clock_freq.Value;
                    label2.Visible = true;
                    m_tbLO_Frequency.ReadOnly = false;
                }
                else
                {
                    m_tbLO_Frequency.Text = GetClockFreq();
                    m_InstEdit.SetParamExpr(cymixerparameters.LO_CLOCK_FREQ, m_tbLO_Frequency.Text).ToString();
                    m_InstEdit.CommitParamExprs().ToString();
                    label2.Visible = false;
                    m_tbLO_Frequency.ReadOnly = true;
                }
            }
        }


        private void CyMixerControl_Load(object sender, EventArgs e)
        {
            cymixerparameters prms = new cymixerparameters(m_InstEdit);

            if (m_rbInternal_LO.Checked)
            {
                m_tbLO_Frequency.Text = prms.LO_clock_freq.Value; 
            }
            else
            {
                string frequency = GetClockFreq();
                if (string.Equals(frequency, "0"))
                {
                    m_tbLO_Frequency.ReadOnly = false;
                    m_tbLO_Frequency.Text = prms.LO_clock_freq.Value; 
                    label2.Visible = true;
                }
                else
                {
                    m_tbLO_Frequency.Text = GetClockFreq();
                    m_InstEdit.SetParamExpr(cymixerparameters.LO_CLOCK_FREQ, m_tbLO_Frequency.Text).ToString();
                    m_InstEdit.CommitParamExprs().ToString();
                    label2.Visible = false;
                }
            }
        }

    }
}
