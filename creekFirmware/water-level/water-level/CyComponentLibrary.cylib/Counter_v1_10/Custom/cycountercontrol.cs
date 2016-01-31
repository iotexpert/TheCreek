/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using Counter_v1_10;

namespace Counter_v1_10
{
    public partial class CyCounterControl : UserControl
    {
        public const uint CONST_2_8 = 255;
        public const uint CONST_2_16 = 65535;
        public const uint CONST_2_24 = 16777215;
        public const uint CONST_2_32 = 4294967295;
        public ICyInstEdit_v1 m_Component = null;
        public string LastFreq = "12";
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public CyCounterControlAdv m_control_adv;

        public CyCounterControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery, CyCounterControlAdv control_advanced)
        {
            m_Component = inst;
            m_TermQuery = termQuery;
            //Trace.Assert(Debugger.IsAttached);
            m_control_adv = control_advanced;
            InitializeComponent();
            InitializeFormComponents(inst);
            UpdateFormFromParams(m_Component);
        }

        private void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Initialize Clock Mode Combo Box with Enumerated Types
            //Set the Clock Modes Combo Box from Enums
            IEnumerable<string> ClockModeEnums = inst.GetPossibleEnumValues("ClockMode");
            foreach (string str in ClockModeEnums)
            {
                m_cbClockMode.Items.Add(str);
            }

            //Initialize Compare Mode Combo Box with Enumerated Types
            //Set the Compare Modes Combo Box from Enums
            IEnumerable<string> CompareModeEnums = inst.GetPossibleEnumValues("CompareMode");
            foreach (string str in CompareModeEnums)
            {
                m_cbCompareMode.Items.Add(str);
            }
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            CounterParameters prms = new CounterParameters(inst);
            switch (prms.Resolution.Value)
            {
                case "8": m_rbResolution8.Checked = true; m_numPeriod.Maximum = CONST_2_8; m_numCompare1.Maximum = CONST_2_8; break;
                case "16": m_rbResolution16.Checked = true; m_numPeriod.Maximum = CONST_2_16; m_numCompare1.Maximum = CONST_2_16; break;
                case "24": m_rbResolution24.Checked = true; m_numPeriod.Maximum = CONST_2_24; m_numCompare1.Maximum = CONST_2_24; break;
                case "32": m_rbResolution32.Checked = true; m_numPeriod.Maximum = CONST_2_32; m_numCompare1.Maximum = CONST_2_32; break;
            }
            m_numPeriod.Value = Convert.ToInt64(prms.Period.Value);
            switch (prms.FixedFunction.Value)
            {
                case "true": m_rbFixedFunction.Checked = true; break;
                case "false": m_rbUDB.Checked = true; break;
            }


            m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value), "Period:");
            m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Maximum), "Max. Period:");
            m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1),"Bit Resolution:");

            //Set Compare Value numeric up down
            m_numCompare1.Value = Convert.ToInt64(prms.CompareValue.Value);

            //Set the Clock Modes Combo Box from Enums
            IEnumerable<string> ClockModeEnums = inst.GetPossibleEnumValues("ClockMode");
            bool Clockfound = false;
            foreach (string str in ClockModeEnums)
            {
                if (!Clockfound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ClockMode", prms.ClockMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbClockMode.SelectedItem = paramcompare;
                        Clockfound = true;
                    }
                }
            }

            //Set the Compare Modes Combo Box from Enums
            IEnumerable<string> CompareModeEnums = inst.GetPossibleEnumValues("CompareMode");
            bool Comparefound = false;
            foreach (string str in CompareModeEnums)
            {
                if (!Comparefound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("CompareMode", prms.CompareMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbCompareMode.SelectedItem = paramcompare;
                        Comparefound = true;
                    }
                }
            }

            switch (prms.FixedFunction.Value)
            {
                case "true": SetFixedFunction(); break;
                case "false": ClearFixedFunction(); break;
            }
        }

        public string UpdateCalculatedPeriod(long period, string name)
        {
                List<CyClockData> clkdata = new List<CyClockData>();
                clkdata = m_TermQuery.GetClockData("clock", 0);
                if (!clkdata[0].IsFrequencyKnown)
                {
                    clkdata = m_TermQuery.GetClockData("count", 0);
                }
                if (clkdata[0].IsFrequencyKnown)
                {
                    double infreq = clkdata[0].Frequency;
                    switch (clkdata[0].Unit)
                    {
                        case CyClockUnit.kHz: infreq = infreq * 1000; break;
                        case CyClockUnit.MHz: infreq = infreq * 1000000; break;
                    }
                    double periodfreq = infreq / period;
                    double periodtime = 1 / periodfreq;

                    int i = 0;
                    while (periodtime < 1)
                    {
                        periodtime = periodtime * 1000;
                        i += 3;
                    }
                    string time = "s";
                    switch (i)
                    {
                        case 0: time = "s"; break;
                        case 3: time = "ms"; break;
                        case 6: time = "us"; break;
                        case 9: time = "ns"; break;
                        case 12: time = "ps"; break;
                    }
                    return (string.Format("{0} = {1}{2}",name, Math.Round(periodtime, 3), time));
                }
                else
                {
                    return (string.Format("{0} = UNKNOWN SOURCE FREQ",name));
                }
        }
        
        private void SetFixedFunction()
        {
            //Disable 24 and 32 bit functionality and reset Period value if it's too high
            if (m_rbResolution24.Checked || m_rbResolution32.Checked)
                m_rbResolution16_Click(this, new EventArgs());
            m_rbResolution32.Enabled = false;
            m_rbResolution24.Enabled = false;

            //Set clock Mode to Down Counter Only
            string clockmodeff = null;
            IEnumerable<string> ClockModesEnums = m_Component.GetPossibleEnumValues("ClockMode");
            foreach (string str in ClockModesEnums)
            {
                if (str.Contains("Down") && str.Contains("Counter"))
                    clockmodeff = str;
            }
            string prm = m_Component.ResolveEnumDisplayToId("ClockMode", clockmodeff);
            SetAParameter("ClockMode", prm); 
            m_cbClockMode.Enabled = false;

            m_cbCompareMode.Enabled = false;
            m_bMaxCompareValue.Enabled = false;
            m_numCompare1.Enabled = false;
            SetAParameter("InterruptOnCapture", "false");
        }

        private void ClearFixedFunction()
        {
            //Enable 24 and 32-bit functionality
            m_rbResolution32.Enabled = true;
            m_rbResolution24.Enabled = true;
            m_cbClockMode.Enabled = true;
            m_cbCompareMode.Enabled = true;
            m_bMaxCompareValue.Enabled = true;
            m_numCompare1.Enabled = true;
        }

        private void m_numPeriod_ValueChanged(object sender, EventArgs e)
        {
            m_numCompare1.Maximum = m_numPeriod.Value;
            SetAParameter("Period", m_numPeriod.Value.ToString() + "u");
            m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value), "Period:");
        }

        private void m_rbResolution8_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution8.Checked)
            {
                m_rbResolution8.Checked = true;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = false;
                m_numPeriod.Maximum = CONST_2_8;
                m_numCompare1.Maximum = m_numPeriod.Value;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Maximum), "Max. Period:");
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), "Bit Resolution:");
                SetAParameter("Resolution", "8");
            }
        }

        private void m_rbResolution16_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution16.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = true;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = false;
                m_numPeriod.Maximum = CONST_2_16;
                m_numCompare1.Maximum = m_numPeriod.Value;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Maximum), "Max. Period:");
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), "Bit Resolution:");
                SetAParameter("Resolution", "16");
            }
        }

        private void m_rbResolution24_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution24.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = true;
                m_rbResolution32.Checked = false;
                m_numPeriod.Maximum = CONST_2_24;
                m_numCompare1.Maximum = m_numPeriod.Value;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Maximum), "Max. Period:");
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), "Bit Resolution:");
                SetAParameter("Resolution", "24");
            }
        }

        private void m_rbResolution32_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution32.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = true;
                m_numPeriod.Maximum = CONST_2_32;
                m_numCompare1.Maximum = m_numPeriod.Value;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Maximum), "Max. Period:");
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), "Bit Resolution:");
                SetAParameter("Resolution", "32");
            }
        }

        private void m_rbFixedFunction_Click(object sender, EventArgs e)
        {
            if (!m_rbFixedFunction.Checked)
            {
                m_rbFixedFunction.Checked = true;
                m_rbUDB.Checked = false;
                SetFixedFunction();
                SetAParameter("FixedFunction", "true");
                UpdateAdvancedForm();
            }
        }

        private void m_rbUDB_Click(object sender, EventArgs e)
        {
            if (!m_rbUDB.Checked)
            {
                m_rbFixedFunction.Checked = false;
                m_rbUDB.Checked = true;
                ClearFixedFunction();
                SetAParameter("FixedFunction", "false");
                UpdateAdvancedForm();
            }
        }

        private void m_bMaxPeriod_Click(object sender, EventArgs e)
        {
            if (m_rbResolution8.Checked)
                m_numPeriod.Value = 255;
            else if (m_rbResolution16.Checked)
                m_numPeriod.Value = 65535;
            else if (m_rbResolution24.Checked)
                m_numPeriod.Value = 16777215;
            else
                m_numPeriod.Value = 4294967295;
        }

        private void m_bMaxCompareValue_Click(object sender, EventArgs e)
        {
            m_numCompare1.Value = m_numPeriod.Value;
        }

        private void m_numCompare1_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter("CompareValue", m_numCompare1.Value.ToString() + "u");
        }

        private void SetAParameter(string parameter, string value)
        {
            if (this.ContainsFocus)
            {
                m_Component.SetParamExpr(parameter, value);
                m_Component.CommitParamExprs();
                if (m_Component.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_Component.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateAdvancedForm()
        {
            if (m_control_adv != null)
                m_control_adv.UpdateFormFromParams(m_Component);
        }

        private void m_cbClockMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ClockMode", m_cbClockMode.Text);
            SetAParameter("ClockMode", prm);
        }

        private void m_cbCompareMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("CompareMode", m_cbCompareMode.Text);
            SetAParameter("CompareMode", prm);
        }
    }

    /// <summary>
    /// Ovverride the base numeric up down so that enter key strokes aren't passed to the parent form.
    /// </summary>
    public class CyNumericUpDown : NumericUpDown
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.ValidateEditText();
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
    }


}
