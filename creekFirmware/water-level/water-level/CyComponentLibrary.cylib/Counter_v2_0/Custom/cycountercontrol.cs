/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace Counter_v2_0
    {
    public partial class CyCounterControl : UserControl
    {
        ICyInstEdit_v1 m_component = null;
        ICyTerminalQuery_v1 m_termQuery = null;
        CyCounterControlAdv m_control_adv;
        const string emptyStr = "";
        const string CLOCK = "clock";
        const string COUNT = "count";
        const string CLOCKWITHUPDOWN = "2";
        private int value = 3;

        public CyCounterControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery, CyCounterControlAdv control_advanced)
        {
            m_component = inst;
            m_termQuery = termQuery;
            m_control_adv = control_advanced;
            InitializeComponent();
            InitializeFormComponents(inst);
            UpdateFormFromParams(m_component);
        }
        ///// <summary>
        ///// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the 
        ///// parameters
        ///// Also need to handle CyNumericUpDowns to override the UpButton and DownButtons before the value is changed
        ///// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        private void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Initialize Clock Mode Combo Box with Enumerated Types
            //Set the Clock Modes Combo Box from Enums
            IEnumerable<string> ClockModeEnums = inst.GetPossibleEnumValues(CounterParameters.CLOCKMODE);
            foreach (string str in ClockModeEnums)
            {
                m_cbClockMode.Items.Add(str);
            }

            //Initialize Compare Mode Combo Box with Enumerated Types
            //Set the Compare Modes Combo Box from Enums
            IEnumerable<string> CompareModeEnums = inst.GetPossibleEnumValues(CounterParameters.COMPAREMODE);
            foreach (string str in CompareModeEnums)
            {
                m_cbCompareMode.Items.Add(str);
            }            
            m_numPeriod.Minimum = Decimal.MinValue;
            m_numPeriod.Maximum = Decimal.MaxValue;
            m_numCompare1.Minimum = Decimal.MinValue;
            m_numCompare1.Maximum = Decimal.MaxValue;
        }

        void UnhookUpdateEvents()
        {
            //First remove event handler to keep from attaching multiple
            m_numPeriod.UpEvent -= new UpButtonEvent(m_numPeriod_UpEvent);
            m_numPeriod.DownEvent -= new DownButtonEvent(m_numPeriod_DownEvent);
            m_numCompare1.UpEvent -= new UpButtonEvent(m_numCompare1_UpEvent);
            m_numCompare1.DownEvent -= new DownButtonEvent(m_numCompare1_DownEvent); ;
            m_numCompare1.ValueChanged -= m_numCompare1_ValueChanged;
            m_numPeriod.ValueChanged -= m_numPeriod_ValueChanged;
        }

        void HookupUpdateEvents()
        {
            //Now attach the event handler 
            m_numPeriod.UpEvent += new UpButtonEvent(m_numPeriod_UpEvent);
            m_numPeriod.DownEvent += new DownButtonEvent(m_numPeriod_DownEvent);
            m_numCompare1.UpEvent += new UpButtonEvent(m_numCompare1_UpEvent);
            m_numCompare1.DownEvent += new DownButtonEvent(m_numCompare1_DownEvent);
            m_numCompare1.ValueChanged += m_numCompare1_ValueChanged;
            m_numPeriod.ValueChanged += m_numPeriod_ValueChanged;
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {        
            UnhookUpdateEvents();
            CounterParameters prms = new CounterParameters(inst);
            switch (prms.Resolution.Value)
            {
                case "8": m_rbResolution8.Checked = true; break;
                case "16": m_rbResolution16.Checked = true; break;
                case "24": m_rbResolution24.Checked = true; break;
                case "32": m_rbResolution32.Checked = true; break;
                default: Debug.Fail(CounterParameters.UNHANDLED_CASE); break;
            }

            m_numPeriod.Value = Convert.ToInt64(prms.Period.Value);
            switch (prms.FixedFunction.Value)
            {
                case CounterParameters.TRUE: m_rbFixedFunction.Checked = true; break;
                case CounterParameters.FALSE: m_rbUDB.Checked = true; break;
                default: Debug.Fail(CounterParameters.UNHANDLED_CASE); break;
            }
            if (string.IsNullOrEmpty(ep_Errors.GetError(m_numPeriod)))
            {
                m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value), 
                                                                 CounterParameters.CAL_PERIOD);
            }
            m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_rbResolution8.Checked ? 
                                      Counter_v2_0.CounterParameters.CONST_2_8 : (m_rbResolution16.Checked ? 
                                      Counter_v2_0.CounterParameters.CONST_2_16 : (m_rbResolution24.Checked ? 
                                      Counter_v2_0.CounterParameters.CONST_2_24 : CounterParameters.CONST_2_32))),
                                      CounterParameters.MAX_PERIOD);
            m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), CounterParameters.BIT_RESOLUTION);

            //Set Compare Value numeric up down
            m_numCompare1.Value = Convert.ToInt64(prms.CompareValue.Value);

            ////Set the Clock Modes Combo Box from Enums            
            string displayName = null;;
            CyCustErr err = CounterParameters.GetClockModeValue(inst, out displayName);
            if (m_cbClockMode.Items.Contains(displayName))
            {
                m_cbClockMode.SelectedItem = displayName;
            }
            string errMsg = (err.IsOk) ? string.Empty : err.Message;
            ep_Errors.SetError(m_cbClockMode, errMsg);
            //Set the Compare Modes Combo Box from Enums  
            string paramName = null;
            CyCustErr error = CounterParameters.GetCompareModeValue(inst, out paramName);
            if (m_cbCompareMode.Items.Contains(paramName))
            {
                m_cbCompareMode.SelectedItem = paramName;
            }
            string errorMsg = (error.IsOk) ? string.Empty : error.Message;
            ep_Errors.SetError(m_cbCompareMode, errorMsg);

            switch (prms.FixedFunction.Value)
            {
                case CounterParameters.TRUE: SetFixedFunction(); break;
                case CounterParameters.FALSE: ClearFixedFunction(); break;
                default: Debug.Fail(CounterParameters.UNHANDLED_CASE); break;
            }
            HookupUpdateEvents();
        }

        public string UpdateCalculatedPeriod(long period, string name)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = m_termQuery.GetClockData(COUNT, 0);
            if (!clkdata[0].IsFrequencyKnown)
            {
                if( m_component.GetCommittedParam(CounterParameters.CLOCKMODE).Value == CLOCKWITHUPDOWN || 
                    m_component.GetCommittedParam(CounterParameters.FIXEDFUNCTION).Value == "true")
                    clkdata = m_termQuery.GetClockData(CLOCK, 0);
            }
            if (clkdata[0].IsFrequencyKnown)
            {
                double infreq = clkdata[0].Frequency;
                switch (clkdata[0].Unit)
                {
                    case CyClockUnit.kHz: infreq = infreq * 1000; break;
                    case CyClockUnit.MHz: infreq = infreq * 1000000; break;
                    case CyClockUnit.Hz: break;
                    default: Debug.Fail(CounterParameters.UNHANDLED_CASE); break;
                }
                if (clkdata.Count > 1)
                {
                    return ("UNKNOWN SOURCE FREQ");
                }
                if (period == 0)
                {
                    return (string.Format("{0} = Counter is Always Zero", name));
                }

                double periodfreq;
                if (name == CounterParameters.BIT_RESOLUTION)
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
                    default: Debug.Fail(CounterParameters.UNHANDLED_CASE); break;
                }
                return (string.Format("{0} = {1}{2}", name, Math.Round(periodtime, 3), time));
            }
            else
            {
                return (string.Format("{0} = UNKNOWN SOURCE FREQ", name));
            }
        }

        private void SetFixedFunction()
        {
            if (m_rbResolution24.Checked || m_rbResolution32.Checked)
                m_rbResolution16_Click(this, EventArgs.Empty);
            m_rbResolution32.Enabled = false;
            m_rbResolution24.Enabled = false;
            string prm = m_component.ResolveEnumDisplayToId(CounterParameters.CLOCKMODE,
                                                            CounterParameters.ClockModeEnumID_DownCounter);
            CounterParameters.SetAParameter(CounterParameters.CLOCKMODE,
                                            prm, m_component); 
            m_cbClockMode.SelectedItem = CounterParameters.ClockModeEnumID_DownCounter;
            m_cbClockMode.Enabled = false;
            m_cbCompareMode.Enabled = false;
            m_bMaxCompareValue.Enabled = false;
            m_numCompare1.Enabled = false;
            CounterParameters.SetAParameter(Counter_v2_0.CounterParameters.INTERRUPT_ON_CAPTURE, 
                                            Counter_v2_0.CounterParameters.FALSE, m_component);
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
                CounterParameters.SetAParameter(CounterParameters.PERIOD, m_numPeriod.Value.ToString() 
                                                + "u", m_component);
                m_numCompare1.Maximum = m_numPeriod.Value;
                m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value), 
                                                                 CounterParameters.PERIOD);
            }
        }

        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value),
                                                             CounterParameters.PERIOD);
        }

        private void m_rbResolution8_Click(object sender, MouseEventArgs e)
        {
            if (!m_rbResolution8.Checked)
            {
                m_rbResolution8.Checked = true;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = false;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CounterParameters.CONST_2_8), 
                                                                 CounterParameters.MAX_PERIOD);
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), 
                                                                     CounterParameters.BIT_RESOLUTION);
                CounterParameters.SetAParameter(Counter_v2_0.CounterParameters.RESOLUTION, 
                                                Counter_v2_0.CounterParameters.EIGHT_BIT, m_component);
            }
            m_numPeriod_Validating(sender, new CancelEventArgs());
            m_numCompare1_Validating(sender, new CancelEventArgs());
        }

        private void m_rbResolution16_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution16.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = true;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = false;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CounterParameters.CONST_2_16), 
                                                                 CounterParameters.MAX_PERIOD);
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), 
                                                                     CounterParameters.BIT_RESOLUTION);
                CounterParameters.SetAParameter(Counter_v2_0.CounterParameters.RESOLUTION, 
                                                Counter_v2_0.CounterParameters.SIXTEEN_BIT, m_component);
            }
            m_numPeriod_Validating(sender, new CancelEventArgs());
            m_numCompare1_Validating(sender, new CancelEventArgs());
        }

        private void m_rbResolution24_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution24.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = true;
                m_rbResolution32.Checked = false;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CounterParameters.CONST_2_24), 
                                                                 CounterParameters.MAX_PERIOD);
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), 
                                                                     CounterParameters.BIT_RESOLUTION);
                CounterParameters.SetAParameter(Counter_v2_0.CounterParameters.RESOLUTION, 
                                                Counter_v2_0.CounterParameters.TWENTYFOUR_BIT, m_component);
            }
            m_numPeriod_Validating(sender, new CancelEventArgs());
            m_numCompare1_Validating(sender, new CancelEventArgs());
        }

        private void m_rbResolution32_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution32.Checked)
            {
                m_rbResolution8.Checked = false;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = true;
                m_lblCalcMaxPeriod.Text = UpdateCalculatedPeriod(Convert.ToInt64(CounterParameters.CONST_2_32), 
                                                                 CounterParameters.MAX_PERIOD);
                m_lblCalcBitResolution.Text = UpdateCalculatedPeriod(Convert.ToInt64(1), 
                                                                     CounterParameters.BIT_RESOLUTION);
                CounterParameters.SetAParameter(Counter_v2_0.CounterParameters.RESOLUTION, 
                                                Counter_v2_0.CounterParameters.THIRTYTWO_BIT, m_component);
            }
            m_numPeriod_Validating(sender, new CancelEventArgs());
            m_numCompare1_Validating(sender, new CancelEventArgs());

        }

        private void m_rbFixedFunction_Click(object sender, EventArgs e)
        {            
            if (!m_rbFixedFunction.Checked)
            {
                m_rbFixedFunction.Checked = true;
                m_rbUDB.Checked = false;
                value = m_cbClockMode.SelectedIndex;
                SetFixedFunction();
                CounterParameters.SetAParameter(Counter_v2_0.CounterParameters.FIXEDFUNCTION, 
                                                Counter_v2_0.CounterParameters.TRUE, m_component);
                UpdateAdvancedForm();
            }               
        }

        private void m_rbUDB_Click(object sender, EventArgs e)
        {     
            if (!m_rbUDB.Checked)
            {
                m_rbFixedFunction.Checked = false;
                m_rbUDB.Checked = true;
                m_cbClockMode.SelectedIndex = value;
                ClearFixedFunction();
                CounterParameters.SetAParameter(Counter_v2_0.CounterParameters.FIXEDFUNCTION, 
                                                Counter_v2_0.CounterParameters.FALSE, m_component);
                UpdateAdvancedForm();
            }
        }

        private void m_bMaxPeriod_Click(object sender, EventArgs e)
        {
            if (m_rbResolution8.Checked)
                m_numPeriod.Value = Counter_v2_0.CounterParameters.CONST_2_8;
            else if (m_rbResolution16.Checked)
                m_numPeriod.Value = Counter_v2_0.CounterParameters.CONST_2_16;
            else if (m_rbResolution24.Checked)
                m_numPeriod.Value = Counter_v2_0.CounterParameters.CONST_2_24;
            else
                m_numPeriod.Value = Counter_v2_0.CounterParameters.CONST_2_32;
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
                CounterParameters.SetAParameter(CounterParameters.COMPAREVALUE, 
                                               m_numCompare1.Value.ToString() + CounterParameters.UNIT, m_component);
            }
        }

        private void UpdateAdvancedForm()
        {
            if (m_control_adv != null)
                m_control_adv.UpdateFormFromParams(m_component);
        }

        private void m_cbClockMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_component.ResolveEnumDisplayToId(CounterParameters.CLOCKMODE, m_cbClockMode.Text);
            CounterParameters.SetAParameter(CounterParameters.CLOCKMODE, prm, m_component);
        }

        private void m_cbCompareMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_component.ResolveEnumDisplayToId(CounterParameters.COMPAREMODE, m_cbCompareMode.Text);
            CounterParameters.SetAParameter(CounterParameters.COMPAREMODE, prm, m_component);
        }

        private void m_numPeriod_Validating(object sender, CancelEventArgs e)
        {
            if (m_numPeriod != null)
            {
                if (m_rbResolution8.Checked && ((m_numPeriod.Value < 0) || 
                   (m_numPeriod.Value > CounterParameters.CONST_2_8)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 8-Bit Timer must be between 0 and {0}", 
                                       CounterParameters.CONST_2_8));
                    e.Cancel = true;
                }
                else if (m_rbResolution16.Checked && ((m_numPeriod.Value < 0) ||
                        (m_numPeriod.Value > CounterParameters.CONST_2_16)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 16-Bit Timer must be between 0 and {0}", 
                                      CounterParameters.CONST_2_16));
                    e.Cancel = true;
                }
                else if (m_rbResolution24.Checked && ((m_numPeriod.Value < 0) ||
                        (m_numPeriod.Value > CounterParameters.CONST_2_24)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 24-Bit Timer must be between 0 and {0}", 
                                       CounterParameters.CONST_2_24));
                    e.Cancel = true;
                }
                else if (m_rbResolution32.Checked && ((m_numPeriod.Value < 0) || (m_numPeriod.Value > 
                         CounterParameters.CONST_2_32)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format("Period of 32-Bit Timer must be between 0 and {0}", 
                                       CounterParameters.CONST_2_32));
                    e.Cancel = true;
                }
                else
                    ep_Errors.SetError(m_numPeriod, emptyStr);
            }
            else
            {
                ep_Errors.SetError(m_numPeriod, "Must be a valid value");
            }
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
        ///// <param name="e"></param>       
        void m_numPeriod_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_rbResolution8.Checked && (m_numPeriod.Value == CounterParameters.CONST_2_8))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution16.Checked && (m_numPeriod.Value == CounterParameters.CONST_2_16))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution24.Checked && (m_numPeriod.Value == CounterParameters.CONST_2_24))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution32.Checked && (m_numPeriod.Value == CounterParameters.CONST_2_32))
            {
                e.Cancel = true;
                return;
            }
        }

        // <summary>
        // This event is called when the down button is pressed and before the value is decremented
        // It allows me to see if I'm at my internal minimum and cancel the increment if I don't want it to happen
        // </summary>
        // <param name="sender"></param>
        //<param name="e"></param>      
        void m_numCompare1_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numCompare1.Value == 0)
            {
                e.Cancel = true;
                return;
            }
        }

        ///// <summary>
        ///// This event is called when the up button is pressed and before the value is incremented
        ///// It allows me to see if I'm at my internal maximum and cancel the increment if I don't want it to happen
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>        
        void m_numCompare1_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numCompare1.Value == m_numPeriod.Value)
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution8.Checked && (m_numCompare1.Value == CounterParameters.CONST_2_8))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution16.Checked && (m_numCompare1.Value == CounterParameters.CONST_2_16))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution24.Checked && (m_numCompare1.Value == CounterParameters.CONST_2_24))
            {
                e.Cancel = true;
                return;
            }
            if (m_rbResolution32.Checked && (m_numCompare1.Value == CounterParameters.CONST_2_32))
            {
                e.Cancel = true;
                return;
            }
        }

        private void m_numCompare1_Validating(object sender, CancelEventArgs e)
        {
            if (m_numCompare1.Value < 0 || m_numCompare1.Value > m_numPeriod.Value)
            {
                ep_Errors.SetError(m_numCompare1, 
                string.Format("Compare Value must be between 0 and the Period Value of {0}", m_numPeriod.Value));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numCompare1, emptyStr);
            }
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
    /// Custom Event Args for the UpButtonEvent Delegate needs to allow the event to be cancelled before the value 
    /// is changed
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
    /// Custom Event Args for the DownButtonEvent Delegate needs to allow the event to be cancelled before the value
    /// is changed
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

