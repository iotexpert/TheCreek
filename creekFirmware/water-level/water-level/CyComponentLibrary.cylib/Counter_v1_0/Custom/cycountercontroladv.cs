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

namespace Counter_v1_0
{
    public partial class CyCounterControlAdv : UserControl
    {
        public const uint CONST_2_8 = 255;
        public const uint CONST_2_16 = 65535;
        public const uint CONST_2_24 = 16777215;
        public const uint CONST_2_32 = 4294967295;
        public ICyInstEdit_v1 m_Component = null;
        public string LastFreq = "12";
        public ICyTerminalQuery_v1 m_TermQuery = null;

        bool m_ReloadOnCapCheckedUDB = false;
        bool m_ReloadOnCompCheckedUDB = false;
        bool m_ReloadOnResetCheckedUDB = false;
        bool m_ReloadOnTCCheckedUDB = false;

        public CyCounterControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery)
        {
            m_Component = inst;
            m_TermQuery = termQuery;
            InitializeComponent();
            InitializeFormComponents(inst);
            UpdateFormFromParams(m_Component);
        }

        private void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Initialize Capture Mode Combo Box with Enumerated Types
            //Set the Capture Modes Combo Box from Enums
            IEnumerable<string> CaptureModeEnums = inst.GetPossibleEnumValues("CaptureMode");
            foreach (string str in CaptureModeEnums)
            {
                m_cbCaptureMode.Items.Add(str);
            }

            //Initialize Enable Mode Combo Box with Enumerated Types
            //Set the Enable Modes Combo Box from Enums
            IEnumerable<string> EnableModeEnums = inst.GetPossibleEnumValues("EnableMode");
            foreach (string str in EnableModeEnums)
            {
                m_cbEnableMode.Items.Add(str);
            }
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            CounterParameters prms = new CounterParameters(inst);

            //Set the Capture Modes Combo Box from Enums
            IEnumerable<string> CaptureModeEnums = inst.GetPossibleEnumValues("CaptureMode");
            bool Capturefound = false;
            foreach (string str in CaptureModeEnums)
            {
                if (!Capturefound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("CaptureMode", prms.CaptureMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbCaptureMode.SelectedItem = paramcompare;
                        Capturefound = true;
                    }
                }
            }

            //Set the Enable Modes Combo Box from Enums
            IEnumerable<string> EnableModeEnums = inst.GetPossibleEnumValues("EnableMode");
            bool Enablefound = false;
            foreach (string str in EnableModeEnums)
            {
                if (!Enablefound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("EnableMode", prms.EnableMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbEnableMode.SelectedItem = paramcompare;
                        Enablefound = true;
                    }
                }
            }

            // Store old values
            m_ReloadOnCapCheckedUDB = (prms.ReloadOnCapture.Value == "true") ? true : false; 
            m_ReloadOnCompCheckedUDB = (prms.ReloadOnCompare.Value == "true") ? true : false;
            m_ReloadOnResetCheckedUDB = (prms.ReloadOnReset.Value == "true") ? true : false;
            m_ReloadOnTCCheckedUDB = (prms.ReloadOnOverUnder.Value == "true") ? true : false;
            
        

            switch (prms.FixedFunction.Value)
            {
                case "true": SetFixedFunction(); break;
                case "false": ClearFixedFunction(); break;
            }
            prms = new CounterParameters(inst);
            //Set Interrupt on Check Boxes
            m_chbxIntSrcCapture.Checked = (prms.InterruptOnCapture.Value == "true") ? true : false;
            m_chbxIntSrcCompare.Checked = (prms.InterruptOnCompare.Value == "true") ? true : false;
            m_chbxIntSrcTC.Checked = (prms.InterruptOnTC.Value == "true") ? true : false;
        }

        private void SetFixedFunction()
        {
            //Set Capture Mode to None
            string capturemodeff = null;
            IEnumerable<string> CaptureModesEnums = m_Component.GetPossibleEnumValues("CaptureMode");
            foreach (string str in CaptureModesEnums)
            {
                if (str.Contains("None"))
                    capturemodeff = str;
            }
            string prm = m_Component.ResolveEnumDisplayToId("CaptureMode", capturemodeff);
            SetAParameter("CaptureMode", prm);
            m_cbCaptureMode.Enabled = false;

            //Set enable Mode to Software Only
            string enablemodeff = null;
            IEnumerable<string> EnableModesEnums = m_Component.GetPossibleEnumValues("EnableMode");
            foreach (string str in EnableModesEnums)
            {
                if (str.Contains("Software") && str.Contains("Only"))
                    enablemodeff = str;
            }
            string prm2 = m_Component.ResolveEnumDisplayToId("EnableMode", enablemodeff);
            SetAParameter("EnableMode", prm2);
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
            SetAParameter("InterruptOnCapture", "false");
            m_chbxIntSrcCompare.Checked = false;
            m_chbxIntSrcCompare.Enabled = false;
            SetAParameter("InterruptOnCompare", "false");            
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

        void UpdateCheckBoxParameter(string paramName, bool @checked, bool visible)
        {
            if (@checked && visible)
                SetAParameter(paramName, "true");
            else
                SetAParameter(paramName, "false");
        }

        private void m_chbxReloadOnCapture_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter("ReloadOnCapture", cb.Checked, cb.Visible);
        }

        private void m_chbxReloadOnCompare_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter("ReloadOnCompare", cb.Checked, cb.Visible);
        }

        private void m_chbxReloadOnReset_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            SetAParameter("ReloadOnReset", cb.Checked.ToString());
        }

        private void m_chbxReloadOnTC_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            SetAParameter("ReloadOnOverUnder", cb.Checked.ToString());
        }

        private void m_chbxIntSrcTC_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter("InterruptOnTC", cb.Checked, cb.Visible);
        }

        private void m_chbxIntSrcCapture_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter("InterruptOnCapture", cb.Checked, cb.Visible);
        }

        private void m_chbxIntSrcCompare_Changed(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UpdateCheckBoxParameter("InterruptOnCompare", cb.Checked, cb.Visible);
        }

        private void SetAParameter(string parameter, string value)
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

        private void m_cbCaptureMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("CaptureMode", m_cbCaptureMode.Text);
            SetAParameter("CaptureMode", prm);
        }

        private void m_cbEnableMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("EnableMode", m_cbEnableMode.Text);
            SetAParameter("EnableMode", prm);
        }

    }
}

