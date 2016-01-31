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

namespace Counter_v1_50
{
    public partial class CyCounterControlAdv : UserControl
    {        
        ICyInstEdit_v1 m_component = null;        
        public ICyTerminalQuery_v1 m_termQuery = null;
        bool m_FF = false;
        bool m_ReloadOnCapCheckedUDB = false;
        bool m_ReloadOnCompCheckedUDB = false;
        bool m_ReloadOnResetCheckedUDB = false;
        bool m_ReloadOnTCCheckedUDB = false;
        const string NONE = "None";

        public CyCounterControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery)
        {
            m_component = inst;
            m_termQuery = termQuery;
            InitializeComponent();
            InitializeFormComponents(inst);
            UpdateFormFromParams(m_component);
        }

        private void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Initialize Capture Mode Combo Box with Enumerated Types
            //Set the Capture Modes Combo Box from Enums
            IEnumerable<string> CaptureModeEnums = inst.GetPossibleEnumValues(CounterParameters.CAPTUREMODE);
            foreach (string str in CaptureModeEnums)
            {
                m_cbCaptureMode.Items.Add(str);
            }

            //Initialize Enable Mode Combo Box with Enumerated Types
            //Set the Enable Modes Combo Box from Enums
            IEnumerable<string> EnableModeEnums = inst.GetPossibleEnumValues(CounterParameters.ENABLEMODE);
            foreach (string str in EnableModeEnums)
            {
                m_cbEnableMode.Items.Add(str);
            }

            //Initialize Run Mode Combo Box with Enumerated Types
            //Set the Run Modes Combo Box from Enums
            IEnumerable<string> RunModeEnums = inst.GetPossibleEnumValues(CounterParameters.RUNMODE);
            foreach (string str in RunModeEnums)
            {
                m_cbRunMode.Items.Add(str);
            }
        }
        

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookUpdateEvents();
            CounterParameters prms = new CounterParameters(inst);

            //Set the Capture Modes Combo Box from Enums
            string paramName = null;
            CyCustErr error = CounterParameters.GetCaptureModeValue(inst, out paramName);
            if (m_cbCaptureMode.Items.Contains(paramName))
            {
                m_cbCaptureMode.SelectedItem = paramName;
            }
            string errorMsg = (error.IsOk) ? string.Empty : error.Message;
            ep_ErrorAdv.SetError(m_cbCaptureMode, errorMsg);
            

            //Set the Enable Modes Combo Box from Enums
            paramName = null;
            error = CounterParameters.GetEnableModeValue(inst, out paramName);
            if (m_cbEnableMode.Items.Contains(paramName))
            {
                m_cbEnableMode.SelectedItem = paramName;
            }
            errorMsg = (error.IsOk) ? string.Empty : error.Message;
            ep_ErrorAdv.SetError(m_cbEnableMode, errorMsg);


            //Set the Run Modes Combo Box from Enums
            paramName = null;
            error = CounterParameters.GetRunModeValue(inst, out paramName);
            if (m_cbRunMode.Items.Contains(paramName))
            {
                m_cbRunMode.SelectedItem = paramName;
            }
            errorMsg = (error.IsOk) ? string.Empty : error.Message;
            ep_ErrorAdv.SetError(m_cbRunMode, errorMsg);


            // Store old values
            m_ReloadOnCapCheckedUDB = (prms.ReloadOnCapture.Value == CounterParameters.TRUE) ? true : false;
            m_ReloadOnCompCheckedUDB = (prms.ReloadOnCompare.Value == CounterParameters.TRUE) ? true : false;
            m_ReloadOnResetCheckedUDB = (prms.ReloadOnReset.Value == CounterParameters.TRUE) ? true : false;
            m_ReloadOnTCCheckedUDB = (prms.ReloadOnOverUnder.Value == CounterParameters.TRUE) ? true : false;
            
            switch (prms.FixedFunction.Value)
            {
                case CounterParameters.TRUE: SetFixedFunction(); break;
                case CounterParameters.FALSE: ClearFixedFunction(); break;
                default: Debug.Fail(CounterParameters.UNHANDLED_CASE); break;
            }
            prms = new CounterParameters(inst);

            m_FF = (prms.FixedFunction.Value == Counter_v1_50.CounterParameters.TRUE) ? true : false;

            //Set Interrupt on Check Boxes
            m_chbxIntSrcCapture.Checked = (prms.InterruptOnCapture.Value == CounterParameters.TRUE) ? true : false;
            m_chbxIntSrcCompare.Checked = (prms.InterruptOnCompare.Value == CounterParameters.TRUE) ? true : false;

            if (m_FF)
                m_chbxIntSrcTC.Checked = (prms.InterruptOnTC.Value == CounterParameters.TRUE) ? true : false;
            else
                m_chbxIntSrcTC.Checked = (prms.InterruptOnOverUnderFlow.Value == CounterParameters.TRUE) ? true : false;
            
            HookupUpdateEvents();
        }

        private void SetFixedFunction()
        {
            //Set Capture Mode to None
            string capturemodeff = null;
            IEnumerable<string> CaptureModesEnums = m_component.GetPossibleEnumValues(CounterParameters.CAPTUREMODE);
            foreach (string str in CaptureModesEnums)
            {
                if (str.Contains(NONE))
                    capturemodeff = str;
            }
            string prm = m_component.ResolveEnumDisplayToId(CounterParameters.CAPTUREMODE, capturemodeff);
            CounterParameters.SetAParameter(CounterParameters.CAPTUREMODE, prm, m_component);
            m_cbCaptureMode.Enabled = false;

            //Set enable Mode to Software Only
            string enablemodeff = null;
            IEnumerable<string> EnableModesEnums = m_component.GetPossibleEnumValues(CounterParameters.ENABLEMODE);
            foreach (string str in EnableModesEnums)
            {
                if (str.Contains(CounterParameters.SOFTWARE) && str.Contains(CounterParameters.ONLY))
                    enablemodeff = str;
            }
            string prm2 = m_component.ResolveEnumDisplayToId(CounterParameters.ENABLEMODE, enablemodeff);
            CounterParameters.SetAParameter(CounterParameters.ENABLEMODE, prm2, m_component);
            m_cbEnableMode.Enabled = false;

            //Set Reload On Check Boxes
            m_chbxReloadOnCapture.Visible = false;
            m_chbxReloadOnCompare.Visible = false;
            m_chbxReloadOnTC.Checked = true;
            m_chbxReloadOnTC.Enabled = false;
            m_chbxReloadOnReset.Checked = true;
            m_chbxReloadOnReset.Enabled = false;
            m_chbxIntSrcCapture.Checked = false;
            m_chbxIntSrcCapture.Enabled = false;
            CounterParameters.SetAParameter(CounterParameters.INTERRUPT_ON_CAPTURE, CounterParameters.FALSE,
			                                m_component);
            m_chbxIntSrcCompare.Checked = false;
            m_chbxIntSrcCompare.Enabled = false;
            CounterParameters.SetAParameter(CounterParameters.INTERRUPT_ON_COMPARE, CounterParameters.FALSE,
			                                m_component);
        }

        private void ClearFixedFunction()
        {
            m_cbCaptureMode.Enabled = true;
            m_cbEnableMode.Enabled = true;

            m_chbxReloadOnCapture.Checked = m_ReloadOnCapCheckedUDB;
            m_chbxReloadOnCapture.Visible = true;

            m_chbxReloadOnCompare.Checked = m_ReloadOnCompCheckedUDB;
            m_chbxReloadOnCompare.Visible = true;

            m_chbxReloadOnTC.Checked = m_ReloadOnTCCheckedUDB;
            m_chbxReloadOnTC.Enabled = true;

            m_chbxReloadOnReset.Checked = m_ReloadOnResetCheckedUDB;
            m_chbxReloadOnReset.Enabled = true;

            m_chbxIntSrcCapture.Enabled = true;
            m_chbxIntSrcCompare.Enabled = true;
        }

        void UpdateCheckBoxParameter(string paramName, bool isChecked, bool visible)
        {
            if (isChecked && visible)
                CounterParameters.SetAParameter(paramName, CounterParameters.THIRTYTWO_BIT, m_component);
            else
                CounterParameters.SetAParameter(paramName, CounterParameters.FALSE, m_component);
        }

        private void m_chbxReloadOnCapture_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter(CounterParameters.RELOAD_ON_CAPTURE, cb.Checked, cb.Visible);
        }

        private void m_chbxReloadOnCompare_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter(CounterParameters.RELOAD_ON_COMPARE, cb.Checked, cb.Visible);
        }

        private void m_chbxReloadOnReset_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            CounterParameters.SetAParameter(CounterParameters.RELOAD_ON_RESET, cb.Checked.ToString(), m_component);
        }

        private void m_chbxReloadOnTC_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            CounterParameters.SetAParameter(CounterParameters.RELOAD_ON_OVERUNDER, cb.Checked.ToString(), m_component);
        }

        private void m_chbxIntSrcTC_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            bool bOnTc = false;
            bool bOnOverTc = false;
            if (m_FF)
            {
                bOnTc = cb.Checked;
            }
            else
            {
                bOnOverTc = cb.Checked;
            }

            UpdateCheckBoxParameter(CounterParameters.INTERRUPT_ON_TC, bOnTc, cb.Visible);
            UpdateCheckBoxParameter(CounterParameters.INTERRUPT_ON_OVER_UNDERFLOW, bOnOverTc, cb.Visible);
        }

        private void m_chbxIntSrcCapture_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter(CounterParameters.INTERRUPT_ON_CAPTURE, cb.Checked, cb.Visible);
        }

        private void m_chbxIntSrcCompare_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter(CounterParameters.INTERRUPT_ON_COMPARE, cb.Checked, cb.Visible);
        }

        void UnhookUpdateEvents()
        {
            m_cbCaptureMode.SelectedIndexChanged -= m_cbCaptureMode_SelectedIndexChanged;
            m_cbEnableMode.SelectedIndexChanged -= m_cbEnableMode_SelectedIndexChanged;
            m_cbRunMode.SelectedIndexChanged -= m_cbRunMode_SelectedIndexChanged;
            m_chbxReloadOnCapture.VisibleChanged -= m_chbxReloadOnCapture_Changed;
            m_chbxReloadOnCompare.CheckedChanged -= m_chbxReloadOnCapture_Changed;
            m_chbxReloadOnCompare.VisibleChanged -= m_chbxReloadOnCompare_Changed;
            m_chbxReloadOnCompare.CheckedChanged -= m_chbxReloadOnCompare_Changed;
            m_chbxIntSrcTC.VisibleChanged -= m_chbxIntSrcTC_Changed;
            m_chbxIntSrcTC.CheckedChanged -= m_chbxIntSrcTC_Changed;
            m_chbxIntSrcCapture.VisibleChanged -= m_chbxIntSrcCapture_Changed;
            m_chbxIntSrcCapture.CheckedChanged -= m_chbxIntSrcCapture_Changed;
            m_chbxIntSrcCompare.VisibleChanged -= m_chbxIntSrcCompare_Changed;
            m_chbxIntSrcCompare.CheckedChanged -= m_chbxIntSrcCompare_Changed;
        }
        
        void HookupUpdateEvents()
        {
            m_cbCaptureMode.SelectedIndexChanged += m_cbCaptureMode_SelectedIndexChanged;
            m_cbEnableMode.SelectedIndexChanged += m_cbEnableMode_SelectedIndexChanged;
            m_cbRunMode.SelectedIndexChanged += m_cbRunMode_SelectedIndexChanged;
            m_chbxReloadOnCapture.VisibleChanged += m_chbxReloadOnCapture_Changed;
            m_chbxReloadOnCompare.CheckedChanged += m_chbxReloadOnCapture_Changed;
            m_chbxReloadOnCompare.VisibleChanged += m_chbxReloadOnCompare_Changed;
            m_chbxReloadOnCompare.CheckedChanged += m_chbxReloadOnCompare_Changed;
            m_chbxIntSrcTC.VisibleChanged += m_chbxIntSrcTC_Changed;
            m_chbxIntSrcTC.CheckedChanged += m_chbxIntSrcTC_Changed;
            m_chbxIntSrcCapture.VisibleChanged += m_chbxIntSrcCapture_Changed;
            m_chbxIntSrcCapture.CheckedChanged += m_chbxIntSrcCapture_Changed;
            m_chbxIntSrcCompare.VisibleChanged += m_chbxIntSrcCompare_Changed;
            m_chbxIntSrcCompare.CheckedChanged += m_chbxIntSrcCompare_Changed;
        }


        private void m_cbCaptureMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_component.ResolveEnumDisplayToId(CounterParameters.CAPTUREMODE, m_cbCaptureMode.Text);
            CounterParameters.SetAParameter(CounterParameters.CAPTUREMODE, prm, m_component);
        }

        private void m_cbEnableMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_component.ResolveEnumDisplayToId(CounterParameters.ENABLEMODE, m_cbEnableMode.Text);
            CounterParameters.SetAParameter(CounterParameters.ENABLEMODE, prm, m_component);
        }

        private void m_cbRunMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_component.ResolveEnumDisplayToId(CounterParameters.RUNMODE, m_cbRunMode.Text);
            CounterParameters.SetAParameter(CounterParameters.RUNMODE, prm, m_component);
        }
    }
}


