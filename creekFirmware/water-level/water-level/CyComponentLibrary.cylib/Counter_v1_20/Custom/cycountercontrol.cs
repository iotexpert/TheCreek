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
using Counter_v1_20;

namespace Counter_v1_20
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
        /// <summary>
        /// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the parameters
        /// Also need to handle CyNumericUpDowns to override the UpButton and DownButtons before the value is changed
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            m_numPeriod.UpEvent += new UpButtonEvent(m_numPeriod_UpEvent);
            m_numPeriod.DownEvent += new DownButtonEvent(m_numPeriod_DownEvent);
            m_numCompare1.UpEvent += new UpButtonEvent(m_numCompare1_UpEvent);
            m_numCompare1.DownEvent += new DownButtonEvent(m_numCompare1_DownEvent);
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
            m_numPeriod.Minimum = Decimal.MinValue;
            m_numPeriod.Maximum = Decimal.MaxValue;
            m_numCompare1.Minimum = Decimal.MinValue;
            m_numCompare1.Maximum = Decimal.MaxValue;
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            CounterParameters prms = new CounterParameters(inst);
            switch (prms.Resolution.Value)
            {
                case "8": m_rbResolution8.Checked = true; break;
                case "16": m_rbResolution16.Checked = true; break;
                case "24": m_rbResolution24.Checked = true; break;
                case "32": m_rbResolution32.Checked = true; break;
            }
            m_numPeriod.Value = Convert.ToInt64(prms.Period.Value);
            switch (prms.FixedFunction.Value)
            {
                case "true": m_rbFixedFunction.Checked = true; break;
                case "false": m_rbUDB.Checked = true; break;
            }

            if(ep_Errors.GetError(m_numPeriod) == "")
                m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value), "Period:");
            m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_rbResolution8.Checked ? CONST_2_8 : (m_rbResolution16.Checked ? CONST_2_16 : (m_rbResolution24.Checked ? CONST_2_24 : CONST_2_32))), "Max. Period:");
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
                    if (period == 0)
                        return (string.Format("{0} = Counter is Always Zero", name));

                    double periodfreq;
                    if (name == "Bit Resolution:")
                    {
                        periodfreq = infreq / period;
                    }
                    else
                    {
                        periodfreq = infreq / (period + 1);
                    }
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
            CancelEventArgs ce = new CancelEventArgs();
            m_numPeriod_Validating(sender, ce);
            if (!ce.Cancel)
            {
                m_numCompare1.Maximum = m_numPeriod.Value;
                SetAParameter("Period", m_numPeriod.Value.ToString() + "u");
                m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value), "Period:");
            }
        }

        private void m_rbResolution8_Click(object sender, MouseEventArgs e)
        {
            if (!m_rbResolution8.Checked)
            {
                m_rbResolution8.Checked = true;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = false;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CONST_2_8), "Max. Period:");
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
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CONST_2_16), "Max. Period:");
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
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CONST_2_24), "Max. Period:");
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
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CONST_2_32), "Max. Period:");
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
                m_numPeriod.Value = CONST_2_8;
            else if (m_rbResolution16.Checked)
                m_numPeriod.Value = CONST_2_16;
            else if (m_rbResolution24.Checked)
                m_numPeriod.Value = CONST_2_24;
            else
                m_numPeriod.Value = CONST_2_32;
        }

        private void m_bMaxCompareValue_Click(object sender, EventArgs e)
        {
            m_numCompare1.Value = m_numPeriod.Value;
        }

        private void m_numCompare1_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numCompare1_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter("CompareValue", m_numCompare1.Value.ToString() + "u");
            }
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

        private void m_numPeriod_Validating(object sender, CancelEventArgs e)
        {
            if (m_numPeriod != null)
            {
                if (m_rbResolution8.Checked && ((m_numPeriod.Value < 0) || (m_numPeriod.Value > CONST_2_8)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 8-Bit Timer must be between 0 and {0}", CONST_2_8));
                    e.Cancel = true;
                }
                else if (m_rbResolution16.Checked && ((m_numPeriod.Value < 0) || (m_numPeriod.Value > CONST_2_16)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 16-Bit Timer must be between 0 and {0}", CONST_2_16));
                    e.Cancel = true;
                }
                else if (m_rbResolution24.Checked && ((m_numPeriod.Value < 0) || (m_numPeriod.Value > CONST_2_24)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 24-Bit Timer must be between 0 and {0}", CONST_2_24));
                    e.Cancel = true;
                }
                else if (m_rbResolution32.Checked && ((m_numPeriod.Value < 0) || (m_numPeriod.Value > CONST_2_32)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 32-Bit Timer must be between 0 and {0}", CONST_2_32));
                    e.Cancel = true;
                }
                else
                    ep_Errors.SetError(m_numPeriod, "");
            }
            else
            {
                ep_Errors.SetError(m_numPeriod, "Must be a valid value");
            }

            ep_Errors.SetIconAlignment(m_numPeriod, ErrorIconAlignment.MiddleLeft);
        }

        private void m_numPeriod_KeyUp(object sender, KeyEventArgs e)
        {
            m_numPeriod_Validating(sender, new CancelEventArgs());
        }

        /// <summary>
        /// This event is called when the down button is pressed and before the value is decremented
        /// It allows me to see if I'm at my internal minimum and cancel the increment if I don't want it to happen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_numPeriod_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numPeriod.Value == 0)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// This event is called when the up button is pressed and before the value is incremented
        /// It allows me to see if I'm at my internal maximum and cancel the increment if I don't want it to happen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_numPeriod_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_rbResolution8.Checked && (m_numPeriod.Value == CONST_2_8))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution16.Checked && (m_numPeriod.Value == CONST_2_16))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution24.Checked && (m_numPeriod.Value == CONST_2_24))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution32.Checked && (m_numPeriod.Value == CONST_2_32))
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// This event is called when the down button is pressed and before the value is decremented
        /// It allows me to see if I'm at my internal minimum and cancel the increment if I don't want it to happen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_numCompare1_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numCompare1.Value == 0)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// This event is called when the up button is pressed and before the value is incremented
        /// It allows me to see if I'm at my internal maximum and cancel the increment if I don't want it to happen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_numCompare1_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numCompare1.Value == m_numPeriod.Value)
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution8.Checked && (m_numCompare1.Value == CONST_2_8))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution16.Checked && (m_numCompare1.Value == CONST_2_16))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution24.Checked && (m_numCompare1.Value == CONST_2_24))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution32.Checked && (m_numCompare1.Value == CONST_2_32))
            {
                e.Cancel = true;
                return;
            }
        }

        private void m_numCompare1_Validating(object sender, CancelEventArgs e)
        {
            if (m_numCompare1.Value < 0 || m_numCompare1.Value > m_numPeriod.Value)
            {
                ep_Errors.SetError(m_numCompare1, string.Format("Compare Value must be between 0 and the Period Value of {0}", m_numPeriod.Value));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numCompare1, "");
            }
            ep_Errors.SetIconAlignment(m_numCompare1, ErrorIconAlignment.MiddleLeft);
        }

        private void m_numCompare1_KeyUp(object sender, KeyEventArgs e)
        {
            m_numCompare1_Validating(sender, new CancelEventArgs());
        }

    }
    /// <summary>
    /// Declaration of the UpButtonEvent must be outside of the class which is CyNumericUpDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UpButtonEvent(object sender, UpButtonEventArgs e);

    /// <summary>
    /// Declaration of the DownButtonEvent must be outside of the class which is CyNumericUpDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DownButtonEvent(object sender, DownButtonEventArgs e);

    /// <summary>
    /// Custom Event Args for the UpButtonEvent Delegate needs to allow the event to be cancelled before the value is changed
    /// </summary>
    public class UpButtonEventArgs : EventArgs
    {
        public bool Cancel;

        public UpButtonEventArgs()
        {
            Cancel = false;
        }
    }
    /// <summary>
    /// Custom Event Args for the DownButtonEvent Delegate needs to allow the event to be cancelled before the value is changed
    /// </summary>
    public class DownButtonEventArgs : EventArgs
    {
        public bool Cancel;

        public DownButtonEventArgs()
        {
            Cancel = false;
        }
    }

    /// <summary>
    /// Ovverride the base numeric up down so that enter key strokes aren't passed to the parent form.
    /// </summary>
    public class CyNumericUpDown : NumericUpDown
    {
        public event UpButtonEvent UpEvent;
        public event DownButtonEvent DownEvent;

        protected virtual void OnUpEvent(UpButtonEventArgs e)
        {
            UpEvent(this, e);
        }

        protected virtual void OnDownEvent(DownButtonEventArgs e)
        {
            DownEvent(this, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.ValidateEditText();
                SendKeys.Send("{TAB}");
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        public override void UpButton()
        {

            UpButtonEventArgs e = new UpButtonEventArgs();
            OnUpEvent(e);
            if (!e.Cancel)
                base.UpButton();
        }

        public override void DownButton()
        {
            DownButtonEventArgs e = new DownButtonEventArgs();
            OnDownEvent(e);
            if (!e.Cancel)
                base.DownButton();
        }
    }

}
