/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace Timer_v2_20
{
    public partial class CyTimerControl : UserControl
    {
        const uint CONST_2_8 = 256;
        const uint CONST_2_16 = 65536;
        const uint CONST_2_24 = 16777216;
        const long CONST_2_32 = 4294967296;
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;

        #region Mode Memory Values and Combo Box Item Arrays
        string m_defaultCaptureModeFF = "Rising Edge";
        string m_noCaptureModeFF = "None";
        string m_triggerModeFF = "None";

        List<string> m_triggerModesUDB = new List<string>();
        List<string> m_captureModesUDB = new List<string>();
        List<string> m_enableModesUDB = new List<string>();
        List<string> m_enableModesFF = new List<string>();

        int m_triggerModeUDBIndex = 1;
        int m_captureModeUDBIndex = 1;
        int m_enableModeUDBIndex = 1;
        int m_enableModeFFIndex = 1;
        int m_captureModeFFIndex = 0;
        #endregion

        public CyTimerControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery)
        {
            m_Component = inst;
            m_TermQuery = termQuery;

            InitializeComponent();
            InitializeFormComponents(m_Component);
            /*Side effect of calling this events will be hooked up to listen for control changes made by the user*/
            UpdateFormFromParams(inst);
            //Causing an error when invoked for the 
            //this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
        }

        /// <summary>
        /// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the parameters
        /// Also need to handle CyNumericUpDowns to override the UpButton and DownButtons before the value is changed
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            
        }

        #region Form Initialization
        private void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Initialize Trigger Mode Combo Box with Enumerated Types
            IEnumerable<string> TriggerModeEnums = inst.GetPossibleEnumValues(CyTimerParameters.S_TRIGGERMODE);
            foreach (string str in TriggerModeEnums)
            {
                m_triggerModesUDB.Add(str);
            }

            m_cbTriggerMode.Items.Add(m_triggerModeFF);

            //Initialize Capture Mode Combo Box with Enumerated Types
            IEnumerable<string> CaptureModeEnums = inst.GetPossibleEnumValues(CyTimerParameters.S_CAPTUREMODE);
            foreach (string str in CaptureModeEnums)
            {
                m_captureModesUDB.Add(str);
            }

            m_cbCaptureMode.Items.Add(m_defaultCaptureModeFF);

            //Initialize Enable Mode Combo Box with Enumerated Types
            IEnumerable<string> EnableModeEnums = inst.GetPossibleEnumValues(CyTimerParameters.S_ENABLEMODE);
            foreach (string str in EnableModeEnums)
            {
                if (str != CyTimerParameters.S_HARDWARE_ONLY)
                    m_enableModesFF.Add(str);
                m_enableModesUDB.Add(str);
            }

            m_cbEnableMode.Items.AddRange(m_enableModesFF.ToArray());

            //Initialize Run Mode Combo Box with Enumerated Types
            IEnumerable<string> RunModeEnums = inst.GetPossibleEnumValues(CyTimerParameters.S_RUNMODE);
            foreach (string str in RunModeEnums)
            {
                m_cbRunMode.Items.Add(str);
            }

            m_numPeriod.Maximum = Decimal.MaxValue;
            m_numPeriod.Minimum = Decimal.MinValue;
            m_numCaptureCount.Maximum = Decimal.MaxValue;
            m_numCaptureCount.Minimum = Decimal.MinValue;
            m_numInterruptOnCaptureCount.Maximum = Decimal.MaxValue;
            m_numInterruptOnCaptureCount.Minimum = Decimal.MinValue;
        }
        #endregion

        private void SetAParameter(Control sender, string parameter, string value)
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
                ep_Errors.SetError(sender, string.Format(CyTimerParameters.S_PARAM_MESSAGE1, parameter, value, errors));
            }
        }

        #region Form Update Methods
        void UnhookUpdateEvents()
        {
            m_cbCaptureMode.SelectedIndexChanged -= m_cbCaptureMode_SelectedIndexChanged;
            m_cbTriggerMode.SelectedIndexChanged -= m_cbTriggerMode_SelectedIndexChanged;
            m_cbRunMode.SelectedIndexChanged -= m_cbRunMode_SelectedIndexChanged;
            m_chbxIntSrcCapture.CheckedChanged -= m_chbxIntSrcCapture_CheckedChanged;
            m_chbxIntSrcCapture.EnabledChanged -= m_chbxIntSrcCapture_EnabledChanged;
            m_chbxIntSrcTC.CheckedChanged -= m_chbxIntSrcTC_CheckedChanged;
            m_chbxIntSrcFifoFull.CheckedChanged -= m_chbxIntSrcFifoFull_CheckedChanged;
            m_numInterruptOnCaptureCount.ValueChanged -= m_numInterruptOnCaptureCount_ValueChanged;
            m_numCaptureCount.ValueChanged -= m_numCaptureCount_ValueChanged;
            m_chbxEnableCaptureCounter.VisibleChanged -= m_chbxEnableCaptureCounter_VisibleChanged;
            m_chbxEnableCaptureCounter.CheckedChanged -= m_chbxEnableCaptureCounter_CheckedChanged;
            m_chbxEnableCaptureCounter.EnabledChanged -= m_chbxEnableCaptureCounter_EnabledChanged;
            m_cbEnableMode.SelectedIndexChanged -= m_cbEnableMode_SelectedIndexChanged;
            m_numPeriod.ValueChanged -= m_numPeriod_ValueChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */

            m_numPeriod.UpEvent -= new UpButtonEvent(m_numPeriod_UpEvent);
            m_numPeriod.DownEvent -= new DownButtonEvent(m_numPeriod_DownEvent);
            m_numInterruptOnCaptureCount.UpEvent -= new UpButtonEvent(m_numInterruptOnCaptureCount_UpEvent);
            m_numInterruptOnCaptureCount.DownEvent -= new DownButtonEvent(m_numInterruptOnCaptureCount_DownEvent);
            m_numCaptureCount.UpEvent -= new UpButtonEvent(m_numCaptureCount_UpEvent);
            m_numCaptureCount.DownEvent -= new DownButtonEvent(m_numCaptureCount_DownEvent);

        }

        void HookupUpdateEvents()
        {
            m_cbCaptureMode.SelectedIndexChanged += m_cbCaptureMode_SelectedIndexChanged;
            m_cbTriggerMode.SelectedIndexChanged += m_cbTriggerMode_SelectedIndexChanged;
            m_cbRunMode.SelectedIndexChanged += m_cbRunMode_SelectedIndexChanged;
            m_chbxIntSrcCapture.CheckedChanged += m_chbxIntSrcCapture_CheckedChanged;
            m_chbxIntSrcCapture.EnabledChanged += m_chbxIntSrcCapture_EnabledChanged;
            m_chbxIntSrcTC.CheckedChanged += m_chbxIntSrcTC_CheckedChanged;
            m_chbxIntSrcFifoFull.CheckedChanged += m_chbxIntSrcFifoFull_CheckedChanged;
            m_numInterruptOnCaptureCount.ValueChanged += m_numInterruptOnCaptureCount_ValueChanged;
            m_numCaptureCount.ValueChanged += m_numCaptureCount_ValueChanged;
            m_chbxEnableCaptureCounter.VisibleChanged += m_chbxEnableCaptureCounter_VisibleChanged;
            m_chbxEnableCaptureCounter.CheckedChanged += m_chbxEnableCaptureCounter_CheckedChanged;
            m_chbxEnableCaptureCounter.EnabledChanged += m_chbxEnableCaptureCounter_EnabledChanged;
            m_cbEnableMode.SelectedIndexChanged += m_cbEnableMode_SelectedIndexChanged;
            m_numPeriod.ValueChanged += m_numPeriod_ValueChanged;
            
            m_numPeriod.UpEvent += new UpButtonEvent(m_numPeriod_UpEvent);
            m_numPeriod.DownEvent += new DownButtonEvent(m_numPeriod_DownEvent);
            m_numInterruptOnCaptureCount.UpEvent += new UpButtonEvent(m_numInterruptOnCaptureCount_UpEvent);
            m_numInterruptOnCaptureCount.DownEvent += new DownButtonEvent(m_numInterruptOnCaptureCount_DownEvent);
            m_numCaptureCount.UpEvent += new UpButtonEvent(m_numCaptureCount_UpEvent);
            m_numCaptureCount.DownEvent += new DownButtonEvent(m_numCaptureCount_DownEvent);
            
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookUpdateEvents();   /* Unhook events so that none of the changes made here cause a parameter to be set */
            CyTimerParameters prms = new CyTimerParameters(inst);
            switch (prms.Resolution.Value)
            {
                case "8": m_rbResolution8.Checked = true; break;
                case "16": m_rbResolution16.Checked = true; break;
                case "24": m_rbResolution24.Checked = true; break;
                case "32": m_rbResolution32.Checked = true; break;
                default: 
                    Debug.Fail("unhandled value?");
                    break;
            }
            m_numPeriod.Value = Convert.ToInt64(prms.Period.Value) + 1;
            switch (prms.FixedFunction.Value)
            {
                case "true": m_rbFixedFunction.Checked = true; break;
                case "false": m_rbUDB.Checked = true; break;
                default: 
                    Debug.Fail("Unhandled value?");
                    break;
            }
            switch (prms.FixedFunction.Value)
            {
                case "true": SetFixedFunction(); break;
                case "false": ClearFixedFunction(); break;
                default: 
                    /*Debug.Fail("unhandled value?); */
                    break;
            }

            //Set the Run Modes Combo Box from Enums
            string displayName;
            CyCustErr err = CyTimerParameters.GetRunModeValue(inst, out displayName);
            if (m_cbRunMode.Items.Contains(displayName))
            {
                m_cbRunMode.SelectedItem = displayName;
            }
            string errMsg = (err.IsOk) ? string.Empty : err.Message;
            ep_Errors.SetError(m_cbRunMode, errMsg);
            
            //Set the Trigger Modes Combo Box from Enums
            err = CyTimerParameters.GetTriggerModeValue(inst, out displayName);
            if (m_cbTriggerMode.Items.Contains(displayName))
            {
                m_cbTriggerMode.SelectedItem = displayName;
            }
            errMsg = (err.IsOk) ? string.Empty : err.Message;
            ep_Errors.SetError(m_cbRunMode, errMsg);


            //Set the Enable Modes Combo Box from Enums
            err = CyTimerParameters.GetEnableModeValue(inst, out displayName);
            if (m_cbEnableMode.Items.Contains(displayName))
            {
                m_cbEnableMode.SelectedItem = displayName;
            }
            errMsg = (err.IsOk) ? string.Empty : err.Message;
            ep_Errors.SetError(m_cbRunMode, errMsg);


            //Set the Capture Modes Combo Box from Enums
            err = CyTimerParameters.GetCaptureModeValue(inst, out displayName);
            if (m_cbCaptureMode.Items.Contains(displayName))
            {
                m_cbCaptureMode.SelectedItem = displayName;
            }
            errMsg = (err.IsOk) ? string.Empty : err.Message;
            ep_Errors.SetError(m_cbRunMode, errMsg);


            //Grab Capture Count Information to populate the Capture Type boxes
            m_chbxEnableCaptureCounter.Checked = (prms.CaptureCounterEnabled.Value == "true");
            UpdateCaptureCounter(m_chbxEnableCaptureCounter.Visible, m_chbxEnableCaptureCounter.Checked);
            // Previously the default value of these were 1, now they must be between 2 and 127.
            try
            {
                m_numCaptureCount.Value = Convert.ToDecimal(prms.CaptureCount.Value);
            }
            catch (ArgumentOutOfRangeException)
            {
                m_numCaptureCount.Value = 2;
                ep_Errors.SetError(m_numCaptureCount, CyTimerParameters.S_VALUE_MUST_BE_FROM_2_TO_127);
                
            }
            m_numCaptureCount.Enabled = (m_rbUDB.Checked && m_chbxEnableCaptureCounter.Checked);

            //Setup the interrupt check boxes
            m_chbxIntSrcCapture.Checked = prms.InterruptOnCapture.Value == "true" ? true : false;
            m_chbxIntSrcTC.Checked = prms.InterruptOnTC.Value == "true" ? true : false;
            m_chbxIntSrcFifoFull.Checked = prms.InterruptOnFIFOFull.Value == "true" ? true : false;

            m_numInterruptOnCaptureCount.Value = Convert.ToDecimal(prms.NumberOfCaptures.Value);
            UpdateInterruptOnCapture();

            m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value));
            HookupUpdateEvents();
        }

        /// <summary>
        /// Creates the text of the Period calculation value displayed.
        /// Calculation is based on period value and a query of the input clock frequency.
        /// </summary>
        /// <param name="period">Integer value of the current period setting in the text box </param>
        public string UpdateCalculatedPeriod(long period)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = m_TermQuery.GetClockData(CyTimerParameters.S_CLOCK, 0);
            if (clkdata.Count > 1) 
            {
                return (CyTimerParameters.S_PERIOD_UNKNOWN_SOURCE_FREQ_ERROR);
            }
            else if (clkdata[0].IsFrequencyKnown && (period > 0))
            {
                double infreq = clkdata[0].Frequency;
                switch (clkdata[0].Unit)
                {
                    case CyClockUnit.kHz: infreq = infreq * 1000; break;
                    case CyClockUnit.MHz: infreq = infreq * 1000000; break;
                    case CyClockUnit.Hz: break;
                    default: 
                        Debug.Fail("Unhandled value?");
                        break;
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
                    default: 
                        Debug.Fail("unhandled value?");
                        break;
                }
                return (string.Format(CyTimerParameters.S_PERIOD_MESSAGE, Math.Round(periodtime, 3), time));
            }
            else
            {
                return (CyTimerParameters.S_PERIOD_UNKNOWN_SOURCE_FREQ_ERROR);
            }
        }

        public void UpdateCaptureModeCB(bool fixedFunction)
        {
            if (fixedFunction)
            {
                m_chbxEnableCaptureCounter.Visible = false;
                m_captureModeUDBIndex = m_cbCaptureMode.SelectedIndex;
                m_cbCaptureMode.Items.Clear();
                m_cbCaptureMode.Items.Add(m_defaultCaptureModeFF);
                m_cbCaptureMode.Items.Add(m_noCaptureModeFF);
                if (m_captureModeFFIndex > 0)
                    m_cbCaptureMode.SelectedIndex = m_captureModeFFIndex;
                else
                    m_cbCaptureMode.SelectedIndex = 0;
            }
            else
            {
                m_chbxEnableCaptureCounter.Visible = true;
                m_captureModeFFIndex = m_cbCaptureMode.SelectedIndex;
                m_cbCaptureMode.Items.Clear();
                m_cbCaptureMode.Items.AddRange(m_captureModesUDB.ToArray());

                if (m_captureModeUDBIndex > 0 && m_captureModeUDBIndex <= m_captureModesUDB.Count)
                    m_cbCaptureMode.SelectedIndex = m_captureModeUDBIndex;
                else
                    m_cbCaptureMode.SelectedIndex = 0;

                //m_cbCaptureMode.Enabled = true;

            }
        }

        private void UpdateEnableModeCB(bool fixedFunction)
        {
            if (fixedFunction)
            {
                m_enableModeUDBIndex = m_cbEnableMode.SelectedIndex;
                m_cbEnableMode.Items.Clear();
                m_cbEnableMode.Items.AddRange(m_enableModesFF.ToArray());
                if (m_enableModeFFIndex >= 0 && m_enableModeFFIndex < m_enableModesFF.Count)
                    m_cbEnableMode.SelectedIndex = m_enableModeFFIndex;
                else
                    m_cbEnableMode.SelectedIndex = 0;
            }
            else
            {
                m_enableModeFFIndex = m_cbEnableMode.SelectedIndex;
                m_cbEnableMode.Items.Clear();
                m_cbEnableMode.Items.AddRange(m_enableModesUDB.ToArray());

                if (m_enableModeUDBIndex >= 0 && m_enableModeUDBIndex < m_enableModesUDB.Count)
                    m_cbEnableMode.SelectedIndex = m_enableModeUDBIndex;
                else
                    m_cbEnableMode.SelectedIndex = 0;

            }
        }

        /// <summary>
        /// Change properties of the numeric updown for capture count based on 
        /// m_chbxEnableCaptureCounter Visible and Checked.
        /// </summary>
        /// <param name="visible">Is the enable checkbox visible? </param>
        /// <param name="checked">Is the enable checkbox checked? </param>
        void UpdateCaptureCounter(bool visible, bool isChecked)
        {
            if (visible && isChecked)
            {
                m_numCaptureCount.Visible = true;
                m_numCaptureCount.Enabled = true;
            }
            else if (visible)
            {
                m_numCaptureCount.Visible = true;
                m_numCaptureCount.Enabled = false;
            }
            else
            {
                m_numCaptureCount.Visible = false;
            }
        }

        void UpdateTriggerModesCB(bool fixedFunction)
        {
            if (fixedFunction)
            {
                m_triggerModeUDBIndex = m_cbTriggerMode.SelectedIndex;
                m_cbTriggerMode.Items.Clear();
                m_cbTriggerMode.Items.Add(m_triggerModeFF);
                m_cbTriggerMode.SelectedIndex = 0;
                m_cbTriggerMode.Enabled = false;
            }
            else
            {
                m_cbTriggerMode.Items.Clear();
                m_cbTriggerMode.Items.AddRange(m_triggerModesUDB.ToArray());

                if (m_triggerModeUDBIndex > 0 && m_triggerModeUDBIndex <= m_triggerModesUDB.Count)
                    m_cbTriggerMode.SelectedIndex = m_triggerModeUDBIndex;
                else
                    m_cbTriggerMode.SelectedIndex = 0;

                m_cbTriggerMode.Enabled = true;

            }
        }

        private void SetFixedFunction()
        {
            //Disable 24 and 32 bit functionality and reset Period value if it's too high
            if (m_rbResolution24.Checked || m_rbResolution32.Checked)
                m_rbResolution16_Click(this, EventArgs.Empty);
            m_rbResolution32.Enabled = false;
            m_rbResolution24.Enabled = false;
            m_cbRunMode.SelectedIndex = 0;

            // Trigger Modes
            UpdateTriggerModesCB(m_rbFixedFunction.Checked);

            // Enable modes
            UpdateEnableModeCB(m_rbFixedFunction.Checked);

            // Capture Modes
            UpdateCaptureModeCB(m_rbFixedFunction.Checked);

            // Interrupt on capture
            m_chbxIntSrcCapture.Text = CyTimerParameters.S_ON_CAPTURE;
            UpdateInterruptOnCapture();

            // Interrupt On FIFO Full not available in Fixed Function Mode
            m_chbxIntSrcFifoFull.Checked = false;
            m_chbxIntSrcFifoFull.Enabled = false;
        }

        private void ClearFixedFunction()
        {
            //Enable 24 and 32-bit functionality
            m_rbResolution32.Enabled = true;
            m_rbResolution24.Enabled = true;

            m_chbxIntSrcFifoFull.Enabled = true;

            // Trigger Modes
            UpdateTriggerModesCB(m_rbFixedFunction.Checked);

            // Enable modes
            UpdateEnableModeCB(m_rbFixedFunction.Checked);

            // Capture Modes
            UpdateCaptureModeCB(m_rbFixedFunction.Checked);

            // Interrupt on Capture
            m_chbxIntSrcCapture.Text = CyTimerParameters.S_ON_CAPTURE_MESSAGE;
            UpdateInterruptOnCapture();
        }

        void UpdateInterruptOnCapture()
        {
            if (m_chbxIntSrcCapture.Checked && m_rbUDB.Checked)
            {
                m_numInterruptOnCaptureCount.Visible = true;
                m_numInterruptOnCaptureCount.Enabled = true;
            }
            else if (m_rbUDB.Checked)
            {
                m_numInterruptOnCaptureCount.Enabled = false;
                m_numInterruptOnCaptureCount.Visible = true;
            }
            else
            {
                m_numInterruptOnCaptureCount.Visible = false;
            }
            
            // The Interrupt on Capture option is disabled when CaptureMode 
            // is set to None
            if(m_rbUDB.Checked)
            {
                // When UDB radio box is checked, the CaptureMode = None has 
                //an enumeration value of 0
                if(m_cbCaptureMode.SelectedIndex == 0)
                {
                    m_chbxIntSrcCapture.Checked = false;
                    m_chbxIntSrcCapture.Enabled = false;
                }
            }
            else
            {    // When fixed function radio box is checked, the CaptureMode = RisingEdge
                // has an enumeration value of 0 and CaptureMode = None has enumeration value of 1
                if(m_cbCaptureMode.SelectedIndex == 1)
                {
                    m_chbxIntSrcCapture.Checked = false;
                    m_chbxIntSrcCapture.Enabled = false;
                }
            }
            
        }

        #endregion

        #region Event Handlers
        #region period Up Down Handlers
        private void m_numPeriod_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numPeriod_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter(m_numPeriod, CyTimerParameters.S_PERIOD, (m_numPeriod.Value - 1).ToString() + "u");
                m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value));
            }

        }

        private void m_numPeriod_Validating(object sender, CancelEventArgs e)
        {
            if (m_numPeriod != null)
            {
                if (m_rbResolution8.Checked && ((m_numPeriod.Value < 2) || (m_numPeriod.Value > CONST_2_8)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format(CyTimerParameters.S_8BIT_MESSAGE1, CONST_2_8));
                    e.Cancel = true;
                }
                else if (m_rbResolution16.Checked && ((m_numPeriod.Value < 2) || (m_numPeriod.Value > CONST_2_16)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format(CyTimerParameters.S_16BIT_MESSAGE1, CONST_2_16));
                    e.Cancel = true;
                }
                else if (m_rbResolution24.Checked && ((m_numPeriod.Value < 2) || (m_numPeriod.Value > CONST_2_24)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format(CyTimerParameters.S_24BIT_MESSAGE1, CONST_2_24));
                    e.Cancel = true;
                }
                else if (m_rbResolution32.Checked && ((m_numPeriod.Value < 2) || (m_numPeriod.Value > CONST_2_32)))
                {
                    ep_Errors.SetError(m_numPeriod, string.Format(CyTimerParameters.S_32BIT_MESSAGE1, CONST_2_32));
                    e.Cancel = true;
                }
                else
                    ep_Errors.SetError(m_numPeriod, string.Empty);
            }
            else
            {
                ep_Errors.SetError(m_numPeriod, CyTimerParameters.S_MUST_BE_A_VALID_VALUE);
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
            if (m_numPeriod.Value == 2)
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
#endregion

        #region Radio Button Handlers
        private void m_rbResolution8_Click(object sender, EventArgs e)
        {
            if (!m_rbResolution8.Checked)
            {
                m_rbResolution8.Checked = true;
                m_rbResolution16.Checked = false;
                m_rbResolution24.Checked = false;
                m_rbResolution32.Checked = false;
                //m_numPeriod.Maximum = CONST_2_8;
                m_numPeriod_Validating(sender, new CancelEventArgs());
                SetAParameter(m_numPeriod, CyTimerParameters.S_RESOLUTION, "8");
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
                //m_numPeriod.Maximum = CONST_2_16;
                m_numPeriod_Validating(sender, new CancelEventArgs());
                SetAParameter(m_numPeriod, CyTimerParameters.S_RESOLUTION, "16");

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
                //m_numPeriod.Maximum = CONST_2_24;
                m_numPeriod_Validating(sender, new CancelEventArgs());
                SetAParameter(m_numPeriod, CyTimerParameters.S_RESOLUTION, "24");
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
                //m_numPeriod.Maximum = CONST_2_32;
                m_numPeriod_Validating(sender, new CancelEventArgs());
                SetAParameter(m_numPeriod, CyTimerParameters.S_RESOLUTION, "32");
            }
        }

        private void m_rbFixedFunction_Click(object sender, EventArgs e)
        {
            if (!m_rbFixedFunction.Checked)
            {
                m_rbFixedFunction.Checked = true;
                m_rbUDB.Checked = false;
                SetFixedFunction();
                SetAParameter(m_numPeriod, CyTimerParameters.S_FIXEDFUNCTION, "true");
            }
        }

        private void m_rbUDB_Click(object sender, EventArgs e)
        {
            if (!m_rbUDB.Checked)
            {
                m_rbFixedFunction.Checked = false;
                m_rbUDB.Checked = true;
                ClearFixedFunction();
                SetAParameter(m_rbFixedFunction, CyTimerParameters.S_FIXEDFUNCTION, "false");
            }
        }
        #endregion

        private void m_cbTriggerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(CyTimerParameters.S_TRIGGERMODE, 
                m_cbTriggerMode.Text);
            SetAParameter(m_cbTriggerMode, CyTimerParameters.S_TRIGGERMODE, prm);
        }

        private void m_cbCaptureMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(CyTimerParameters.S_CAPTUREMODE, 
                m_cbCaptureMode.Text);
            SetAParameter(m_cbCaptureMode, CyTimerParameters.S_CAPTUREMODE, prm);
            if (m_cbCaptureMode.Text == "None")
            {
                m_chbxEnableCaptureCounter.Enabled = false;
                m_chbxIntSrcCapture.Enabled = false;
                m_chbxIntSrcCapture.Checked = false;
            }
            else
            {
                m_chbxEnableCaptureCounter.Enabled = true;
                m_chbxIntSrcCapture.Enabled = true;
            }
        }

        private void m_cbRunMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckEnableModeRunModeConflict();
            string prm = m_Component.ResolveEnumDisplayToId(CyTimerParameters.S_RUNMODE, m_cbRunMode.Text);
            SetAParameter(m_cbRunMode, CyTimerParameters.S_RUNMODE, prm);
        }

        private void m_chbxIntSrcCapture_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(m_chbxIntSrcCapture, CyTimerParameters.S_INTERRUPTONCAPTURE, m_chbxIntSrcCapture.Checked ? "true" : "false");
            UpdateInterruptOnCapture();
        }

        private void m_chbxIntSrcTC_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(m_chbxIntSrcTC, CyTimerParameters.S_INTERRUPTONTC, m_chbxIntSrcTC.Checked ? "true" : "false");
        }

        private void m_chbxEnableCaptureCounter_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(m_numCaptureCount, CyTimerParameters.S_CAPTURECOUNTERENABLED, (m_chbxEnableCaptureCounter.Visible && m_chbxEnableCaptureCounter.Checked) ? "true" : "false");
            UpdateCaptureCounter(m_chbxEnableCaptureCounter.Visible, m_chbxEnableCaptureCounter.Checked);
        }

        private void m_chbxEnableCaptureCounter_VisibleChanged(object sender, EventArgs e)
        {
            SetAParameter(m_numCaptureCount, CyTimerParameters.S_CAPTURECOUNTERENABLED, (m_chbxEnableCaptureCounter.Visible && m_chbxEnableCaptureCounter.Checked) ? "true" : "false");
            UpdateCaptureCounter(m_chbxEnableCaptureCounter.Visible, m_chbxEnableCaptureCounter.Checked);
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

        private void m_cbEnableMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckEnableModeRunModeConflict();
            string prm = m_Component.ResolveEnumDisplayToId(CyTimerParameters.S_ENABLEMODE, m_cbEnableMode.Text);
            SetAParameter(m_cbEnableMode, CyTimerParameters.S_ENABLEMODE, prm);
        }

        private void m_chbxIntSrcCapture_EnabledChanged(object sender, EventArgs e)
        {
            UpdateInterruptOnCapture();
        }

        private void m_chbxEnableCaptureCounter_EnabledChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (!cb.Enabled)
                cb.Checked = false;
        }

        private void m_chbxIntSrcFifoFull_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(m_chbxIntSrcFifoFull, CyTimerParameters.S_INTERRUPTONFIFOFULL, m_chbxIntSrcFifoFull.Checked ? "true" : "false");
        }

        private void CheckEnableModeRunModeConflict()
        {
            if ((m_cbEnableMode.Text.Equals(CyTimerParameters.S_HARDWARE_ONLY) ||
                 m_cbEnableMode.Text.Equals(CyTimerParameters.S_SOFTWARE_AND_HARDWARE)) &&
                (m_cbRunMode.Text.Equals(CyTimerParameters.S_ONE_SHOT) ||
                 m_cbRunMode.Text.Equals(CyTimerParameters.S_ONE_SHOT_HALT_ON_INTERRUPT)))
            {
                MessageBox.Show(CyTimerParameters.S_PIN_MESSAGE1,
                    CyTimerParameters.S_WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Numeric Up/Down Handlers
        private void m_numInterruptOnCaptureCount_Validating(object sender, CancelEventArgs e)
        {
            if (m_numInterruptOnCaptureCount != null)
            {
                if (m_numInterruptOnCaptureCount.Value < 1 || m_numInterruptOnCaptureCount.Value > 4)
                {
                    ep_Errors.SetError(m_numInterruptOnCaptureCount, CyTimerParameters.S_VALUE_MUST_BE_FROM_1_TO_4);
                    e.Cancel = true;
                }
                else
                    ep_Errors.SetError(m_numInterruptOnCaptureCount, "");
            }
            else
            {
                ep_Errors.SetError(m_numInterruptOnCaptureCount, CyTimerParameters.S_MUST_BE_A_VALID_VALUE);
                e.Cancel = true;
            }
        }

        private void m_numCaptureCount_Validating(object sender, CancelEventArgs e)
        {
            if (m_numCaptureCount != null)
            {
                if (m_numCaptureCount.Value < 2 || m_numCaptureCount.Value > 127)
                {
                    ep_Errors.SetError(m_numCaptureCount, CyTimerParameters.S_VALUE_MUST_BE_FROM_2_TO_127);
                    e.Cancel = true;
                }
                else
                    ep_Errors.SetError(m_numCaptureCount, "");
            }
            else
            {
                ep_Errors.SetError(m_numCaptureCount, CyTimerParameters.S_MUST_BE_A_VALID_VALUE);
                e.Cancel = true;
            }
        }

        private void m_numInterruptOnCaptureCount_KeyUp(object sender, KeyEventArgs e)
        {
            m_numInterruptOnCaptureCount_Validating(sender, new CancelEventArgs());
        }

        private void m_numCaptureCount_KeyUp(object sender, KeyEventArgs e)
        {
            m_numCaptureCount_Validating(sender, new CancelEventArgs());
        }

        private void m_numInterruptOnCaptureCount_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numInterruptOnCaptureCount_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter(m_numInterruptOnCaptureCount, CyTimerParameters.S_NUMBEROFCAPTURES, m_numInterruptOnCaptureCount.Value.ToString());
            }
        }

        private void m_numCaptureCount_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numCaptureCount_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter(m_numCaptureCount, CyTimerParameters.S_CAPTURECOUNT, m_numCaptureCount.Value.ToString());
            }
        }

        /// <summary>
        /// This event is called when the down button is pressed and before the value is decremented
        /// It allows me to see if I'm at my internal minimum and cancel the increment if I don't want it to happen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_numCaptureCount_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numCaptureCount.Value == 1)
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
        void m_numCaptureCount_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numCaptureCount.Value == 127)
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
        void m_numInterruptOnCaptureCount_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numInterruptOnCaptureCount.Value == 1)
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
        void m_numInterruptOnCaptureCount_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numInterruptOnCaptureCount.Value == 4)
            {
                e.Cancel = true;
                return;
            }
        }
        #endregion

        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            m_lblCalcFrequency.Text = UpdateCalculatedPeriod(Convert.ToInt64(m_numPeriod.Value));
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
            if(!e.Cancel)
                base.UpButton();
        }

        public override void DownButton()
        {
            DownButtonEventArgs e = new DownButtonEventArgs();
            OnDownEvent(e);
            if(!e.Cancel)
                base.DownButton();
        }
    }


}
