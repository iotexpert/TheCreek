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

namespace PWM_v2_0
{
    public partial class CyPWMControlAdv : UserControl
    {
        public const int MINKILLTIMEMINIMUM = 1;
        public const int MINKILLTIMEMAXIMUM = 255;
        public ICyInstEdit_v1 m_InstEdit = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public CyPWMControl m_control_basic;
        
        public CyPWMControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CyPWMControl control_basic)
        {
            m_InstEdit = inst;
            m_TermQuery = termquery;
            m_control_basic = control_basic;
			m_control_basic.m_control_advanced = this;
            InitializeComponent();
            InitializeFormComponents(inst);
            UpdateFormFromParams(m_InstEdit);
        }

        /// <summary>
        /// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the parameters
        /// Also need to handle CyNumericUpDowns to override the UpButton and DownButtons before the value is changed
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
            m_numMinKillTime.UpEvent += new UpButtonEvent(m_numMinKillTime_UpEvent);
            m_numMinKillTime.DownEvent += new DownButtonEvent(m_numMinKillTime_DownEvent);
            this.GotFocus += new EventHandler(CyPWMControlAdv_GotFocus);
            this.Enter += new EventHandler(CyPWMControlAdv_GotFocus);
        }

        void CyPWMControlAdv_GotFocus(object sender, EventArgs e)
        {
            UnhookUpdateEvents();
            PWMParameters prms = new PWMParameters(m_InstEdit);
            IEnumerable<string> PWMModeEnums = m_InstEdit.GetPossibleEnumValues("PWMMode");
            bool PWMModefound = false;
            foreach (string str in PWMModeEnums)
            {
                if (!PWMModefound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay("PWMMode", prms.PWMMode.Expr);
                    if (paramcompare == str)
                    {
                        if (paramcompare.Contains("One") || paramcompare.Contains("Dither"))
                        {
                            m_chbxIntOnCmp2.Checked = false;
                            m_chbxIntOnCmp2.Enabled = false;
                            SetAParameter("InterruptOnCMP2", "false");
                        }
                        else
                        {
                            m_chbxIntOnCmp2.Enabled = true;
                            m_chbxIntOnCmp2.Checked = (prms.InterruptOnCMP2.Value == "true");
                        }
                        PWMModefound = true;
                    }
                }
            }
            InterruptCheckBoxUpdate();
            HookupUpdateEvents();
        }

        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (ep_Errors.GetError(m_numMinKillTime) != "")
            {
                m_numMinKillTime.Focus();
                e.Cancel = true;
            }
        }

        public void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Initialize Enable Mode Combo Box with Enumerated Types
            //Set the Enable Modes Combo Box from Enums
            IEnumerable<string> EnableModeEnums = inst.GetPossibleEnumValues("EnableMode");
            foreach (string str in EnableModeEnums)
            {
                m_cbEnableMode.Items.Add(str);
            }

            //Initialize Run Mode Combo Box with Enumerated Types
            //Set the Run Modes Combo Box from Enums
            IEnumerable<string> RunModeEnums = inst.GetPossibleEnumValues("RunMode");
            foreach (string str in RunModeEnums)
            {
                m_cbRunMode.Items.Add(str);
            }

            //Initialize Trigger Mode Combo Box with Enumerated Types
            //Set the Trigger Modes Combo Box from Enums
            IEnumerable<string> TriggerModeEnums = inst.GetPossibleEnumValues("TriggerMode");
            foreach (string str in TriggerModeEnums)
            {
                m_cbTriggerMode.Items.Add(str);
            }
            
            //Initialize Kill Mode Combo Box with Enumerated Types
            //Set the Kill Modes Combo Box from Enums
            IEnumerable<string> KillModeEnums = inst.GetPossibleEnumValues("KillMode");
            foreach (string str in KillModeEnums)
            {
                m_cbKillMode.Items.Add(str);
            }

            //Initialize Capture Mode Combo Box with Enumerated Types
            //Set the Capture Modes Combo Box from Enums
            IEnumerable<string> CaptureModeEnums = inst.GetPossibleEnumValues("CaptureMode");
            foreach (string str in CaptureModeEnums)
            {
                m_cbCaptureMode.Items.Add(str);
            }

            m_numMinKillTime.Minimum = Decimal.MinValue;
            m_numMinKillTime.Maximum = Decimal.MaxValue;
        }

        void UnhookUpdateEvents()
        {
            m_cbCaptureMode.SelectedIndexChanged -= m_cbCaptureMode_SelectedIndexChanged;
            m_cbEnableMode.SelectedIndexChanged -= m_cbEnableMode_SelectedIndexChanged;
            m_cbKillMode.SelectedIndexChanged -= m_cbKillMode_SelectedIndexChanged;
            m_cbRunMode.SelectedIndexChanged -= m_cbRunMode_SelectedIndexChanged;
            m_cbTriggerMode.SelectedIndexChanged -= m_cbTriggerMode_SelectedIndexChanged;
            m_chbxIntOnCmp1.CheckedChanged -= m_chbxIntOnCmp1_CheckedChanged;
            m_chbxIntOnCmp2.CheckedChanged -= m_chbxIntOnCmp2_CheckedChanged;
            m_chbxIntOnKill.CheckedChanged -= m_chbxIntOnKill_CheckedChanged;
            m_chbxIntOnTC.CheckedChanged -= m_chbxIntOnTC_CheckedChanged;
            m_chbxNone.CheckedChanged -= m_chbxNone_CheckedChanged;
            m_numMinKillTime.ValueChanged -= m_numMinKillTime_ValueChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */
        }

        void HookupUpdateEvents()
        {
            m_cbCaptureMode.SelectedIndexChanged += m_cbCaptureMode_SelectedIndexChanged;
            m_cbEnableMode.SelectedIndexChanged += m_cbEnableMode_SelectedIndexChanged;
            m_cbKillMode.SelectedIndexChanged += m_cbKillMode_SelectedIndexChanged;
            m_cbRunMode.SelectedIndexChanged += m_cbRunMode_SelectedIndexChanged;
            m_cbTriggerMode.SelectedIndexChanged += m_cbTriggerMode_SelectedIndexChanged;
            m_chbxIntOnCmp1.CheckedChanged += m_chbxIntOnCmp1_CheckedChanged;
            m_chbxIntOnCmp2.CheckedChanged += m_chbxIntOnCmp2_CheckedChanged;
            m_chbxIntOnKill.CheckedChanged += m_chbxIntOnKill_CheckedChanged;
            m_chbxIntOnTC.CheckedChanged += m_chbxIntOnTC_CheckedChanged;
            m_chbxNone.CheckedChanged += m_chbxNone_CheckedChanged;
            m_numMinKillTime.ValueChanged += m_numMinKillTime_ValueChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookUpdateEvents();
            PWMParameters prms = new PWMParameters(inst);

            //Set the Enable Modes Combo Box from Enums
            IEnumerable<string> EnableModeEnums = inst.GetPossibleEnumValues("EnableMode");
            bool Enablefound = false;
            foreach (string str in EnableModeEnums)
            {
                if (!Enablefound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay("EnableMode", prms.EnableMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbEnableMode.SelectedItem = paramcompare;
                        Enablefound = true;
                    }
                }
            }

            //Set the Run Modes Combo Box from Enums
            IEnumerable<string> RunModeEnums = inst.GetPossibleEnumValues("RunMode");
            bool Runfound = false;
            foreach (string str in RunModeEnums)
            {
                if (!Runfound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay("RunMode", prms.RunMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbRunMode.SelectedItem = paramcompare;
                        Runfound = true;
                    }
                }
            }

            //Set the Trigger Modes Combo Box from Enums
            IEnumerable<string> TriggerModeEnums = inst.GetPossibleEnumValues("TriggerMode");
            bool Trigfound = false;
            foreach (string str in TriggerModeEnums)
            {
                if (!Trigfound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay("TriggerMode", prms.TriggerMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbTriggerMode.SelectedItem = paramcompare;
                        Trigfound = true;
                    }
                }
            }

            //Set the Kill Modes Combo Box from Enums
            IEnumerable<string> KillModeEnums = inst.GetPossibleEnumValues("KillMode");
            bool Killfound = false;
            foreach (string str in KillModeEnums)
            {
                if (!Killfound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay("KillMode", prms.KillMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbKillMode.SelectedItem = paramcompare;
                        Killfound = true;
                    }
                }
            }

            //Set the Capture Modes Combo Box from Enums
            IEnumerable<string> CaptureModeEnums = inst.GetPossibleEnumValues("CaptureMode");
            bool Capturefound = false;
            foreach (string str in CaptureModeEnums)
            {
                if (!Capturefound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay("CaptureMode", prms.CaptureMode.Expr);
                    if (paramcompare == str)
                    {
                        m_cbCaptureMode.SelectedItem = paramcompare;
                        Capturefound = true;
                    }
                }
            }
            if (prms.UseInterrupt.Expr == "true")
                m_chbxNone.Checked = false;
				
			//Set the Fixed Function Radio Buttons Accordingly
            switch (prms.FixedFunction.Value)
            {
				case "true": SetFixedFunction(); break;
                case "false": ClearFixedFunction(); break;
            }


            //Setup Interrupt Check Boxes
            m_chbxNone.Checked = (prms.UseInterrupt.Value != "true");
            
            if (prms.KillMode.Value != "0" && (prms.UseInterrupt.Value == "true"))
            {
                m_chbxIntOnKill.Checked = (prms.InterruptOnKill.Value == "true");
                m_chbxIntOnKill.Enabled = true;
            }
            else
            {
                m_chbxIntOnKill.Checked = false;
                m_chbxIntOnKill.Enabled = false;
            }
            if ((prms.UseInterrupt.Value == "true"))
            {
                m_chbxIntOnCmp1.Checked = (prms.InterruptOnCMP1.Value == "true");
            }
            else
            {
                m_chbxIntOnCmp1.Checked = false;
                m_chbxIntOnCmp1.Enabled = false;
            }

            if ((prms.UseInterrupt.Value == "true"))
            {
                m_chbxIntOnTC.Checked = (prms.InterruptOnTC.Value == "true");
            }
            else
            {
                m_chbxIntOnTC.Checked = false;
                m_chbxIntOnTC.Enabled = false;
            }

            IEnumerable<string> PWMModeEnums = inst.GetPossibleEnumValues("PWMMode");
            bool PWMModefound = false;
            foreach (string str in PWMModeEnums)
            {
                if (!PWMModefound)
                {
                    string paramcompare = m_InstEdit.ResolveEnumIdToDisplay("PWMMode", prms.PWMMode.Expr);
                    if (paramcompare == str)
                    {
                        if (paramcompare.Contains("One") || paramcompare.Contains("Dither") || (prms.UseInterrupt.Value == "true"))
                        {
                            m_chbxIntOnCmp2.Checked = false;
                            m_chbxIntOnCmp2.Enabled = false;
                        }
                        else
                        {
                            m_chbxIntOnCmp2.Enabled = true;
                            m_chbxIntOnCmp2.Checked = (prms.InterruptOnCMP2.Value == "true");
                        }
                        PWMModefound = true;
                    }
                }
            }            

            m_numMinKillTime.Value = Convert.ToUInt16(prms.MinimumKillTime.Value);
            if (m_cbKillMode.Text.Contains("Minimum"))
                m_numMinKillTime.Enabled = true;
            else
                m_numMinKillTime.Enabled = false;
            InterruptCheckBoxUpdate();
            HookupUpdateEvents();
        }

        public void SetFixedFunction()
        {
            SetAParameter("FixedFunction", "true");

            string pwmmodeff = null;
            IEnumerable<string> PWMModesEnums = m_InstEdit.GetPossibleEnumValues("PWMMode");
            foreach(string str in PWMModesEnums)
            {
                if(str.Contains("One") && str.Contains("Output"))
                    pwmmodeff = str;
            }

            //Hide all of the fixed function block limitations
            //Set the PWM mode that is expected and then update the basic form with this information
            string prm = m_InstEdit.ResolveEnumDisplayToId("PWMMode", pwmmodeff);
            SetAParameter("PWMMode", prm); //Set to One Output

            string trigmodeff = null;
            IEnumerable<string> TrigModesEnums = m_InstEdit.GetPossibleEnumValues("TriggerMode");
            foreach (string str in TrigModesEnums)
            {
                if (str.Contains("None"))
                    trigmodeff = str;
            }
            if(trigmodeff != null)
                m_cbTriggerMode.SelectedItem =  trigmodeff;
            else
                m_cbTriggerMode.SelectedIndex = 0;
            m_cbTriggerMode.Enabled = false;
            prm = m_InstEdit.ResolveEnumDisplayToId("TriggerMode", trigmodeff);
            SetAParameter("TriggerMode", prm); //Set Trigger Mode to none

            string DBmodeff = null;
            IEnumerable<string> DBModesEnums = m_InstEdit.GetPossibleEnumValues("DeadBand");
            foreach (string str in DBModesEnums)
            {
                if (str.Contains("Disabled"))
                    DBmodeff = str;
            }
            prm = m_InstEdit.ResolveEnumDisplayToId("DeadBand", DBmodeff);
            SetAParameter("DeadBand", prm);  //Set Dead Band to Disabled.


            string killmodeff = null;
            IEnumerable<string> KillModesEnums = m_InstEdit.GetPossibleEnumValues("KillMode");
            foreach (string str in KillModesEnums)
            {
                if (str.Contains("Async"))
                    killmodeff = str;
            }
            if (killmodeff != null)
                m_cbKillMode.SelectedItem = killmodeff;
            else
                m_cbKillMode.SelectedIndex = 0;
            m_cbKillMode.Enabled = false;
            prm = m_InstEdit.ResolveEnumDisplayToId("KillMode", killmodeff);
            SetAParameter("KillMode", prm);
            
            string capmodeff = null;
            IEnumerable<string> CapModesEnums = m_InstEdit.GetPossibleEnumValues("CaptureMode");
            foreach (string str in CapModesEnums)
            {
                if (str.Contains("None"))
                    capmodeff = str;
            }
            if(capmodeff != null)
                m_cbCaptureMode.SelectedItem =  capmodeff;
            else
                m_cbCaptureMode.SelectedIndex = 0;
            m_cbCaptureMode.Enabled = false;
            prm = m_InstEdit.ResolveEnumDisplayToId("CaptureMode", capmodeff);
            SetAParameter("CaptureMode", prm);

            string enablemodeff = null;
            IEnumerable<string> EnableModesEnums = m_InstEdit.GetPossibleEnumValues("EnableMode");
            foreach (string str in EnableModesEnums)
            {
                if (!str.Contains("Hardware"))
                    enablemodeff = str;
            }
            if (enablemodeff != null)
                m_cbEnableMode.SelectedItem = enablemodeff;
            else
                m_cbEnableMode.SelectedIndex = 0;
            m_cbEnableMode.Enabled = false;
            prm = m_InstEdit.ResolveEnumDisplayToId("EnableMode", enablemodeff);
            SetAParameter("EnableMode", prm);

            string runmodeff = null;
            IEnumerable<string> RunModesEnums = m_InstEdit.GetPossibleEnumValues("RunMode");
            foreach (string str in RunModesEnums)
            {
                if (str.Contains("Continuous"))
                    runmodeff = str;
            }
            if (runmodeff != null)
                m_cbRunMode.SelectedItem = runmodeff;
            else
                m_cbRunMode.SelectedIndex = 0;
            m_cbRunMode.Enabled = false;
            prm = m_InstEdit.ResolveEnumDisplayToId("RunMode", runmodeff);
            SetAParameter("RunMode", prm);

            m_chbxIntOnCmp2.Enabled = false;
            m_chbxIntOnCmp2.Checked = false;
            InterruptCheckBoxUpdate();
            UpdateBasicForm();
        }

        public void ClearFixedFunction()
        {
            SetAParameter("FixedFunction", "false");
            m_cbKillMode.Enabled = true;
            //m_cbKillMode.SelectedIndex = 0;
            m_cbCaptureMode.Enabled = true;
            m_cbTriggerMode.Enabled = true;
            m_cbEnableMode.Enabled = true;
            m_cbRunMode.Enabled = true;
            m_chbxIntOnCmp2.Enabled = false;
            InterruptCheckBoxUpdate();
            UpdateBasicForm();
        }


        private void m_cbEnableMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId("EnableMode", m_cbEnableMode.Text);
            SetAParameter("EnableMode", prm);
            UpdateDrawing();
        }

        private void m_cbRunMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId("RunMode", m_cbRunMode.Text);
            SetAParameter("RunMode", prm);
            UpdateDrawing();
        }

        private void m_cbTriggerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId("TriggerMode", m_cbTriggerMode.Text);
            SetAParameter("TriggerMode", prm);
            UpdateDrawing();
        }

        private void m_cbKillMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId("KillMode", m_cbKillMode.Text);
            SetAParameter("KillMode", prm);

            UpdateDrawing();
            if (m_cbKillMode.Text.Contains("Time")) //Min Time
                m_numMinKillTime.Enabled = true;
            else
                m_numMinKillTime.Enabled = false;

            InterruptCheckBoxUpdate();
        }

        #region MinKillTime Numeric Up_Down Event Handlers
        private void m_numMinKillTime_ValueChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            m_numMinKillTime_Validating(sender, ce);
            if (!ce.Cancel)
            {
                SetAParameter("MinimumKillTime", m_numMinKillTime.Value.ToString());
                UpdateDrawing();
            }
        }

        private void m_numMinKillTime_Validating(object sender, CancelEventArgs e)
        {
            if (m_numMinKillTime.Value < MINKILLTIMEMINIMUM || m_numMinKillTime.Value > MINKILLTIMEMAXIMUM)
            {
                ep_Errors.SetError(m_numMinKillTime, string.Format("Kill Time Must Be Between {0} and {1}", MINKILLTIMEMINIMUM, MINKILLTIMEMAXIMUM));
                e.Cancel = true;
            }
            else
                ep_Errors.SetError(m_numMinKillTime, "");
        }

        private void m_numMinKillTime_KeyUp(object sender, KeyEventArgs e)
        {
            m_numMinKillTime_Validating(sender, new CancelEventArgs());
        }

        void m_numMinKillTime_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numMinKillTime.Value == MINKILLTIMEMINIMUM)
                e.Cancel = true;
        }

        void m_numMinKillTime_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numMinKillTime.Value == MINKILLTIMEMAXIMUM)
                e.Cancel = true;
        } 
        #endregion

        private void m_cbCaptureMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_InstEdit.ResolveEnumDisplayToId("CaptureMode", m_cbCaptureMode.Text);
            SetAParameter("CaptureMode", prm);
            UpdateDrawing();
        }

        private void SetAParameter(string parameter, string value)
        {
                m_InstEdit.SetParamExpr(parameter, value);
                m_InstEdit.CommitParamExprs();
                if (m_InstEdit.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_InstEdit.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void UpdateBasicForm()
        {
            if (m_control_basic != null)
                m_control_basic.UpdateFormFromParams(m_InstEdit);
                //m_control_basic.MyUpdate();
        }
        private void UpdateDrawing()
        {
            if (m_control_basic != null)
                m_control_basic.UpdateDrawing(new PWMParameters(m_InstEdit));
        }

        private void m_chbxNone_CheckedChanged(object sender, EventArgs e)
        {
            InterruptCheckBoxUpdate();
        }

        private void InterruptCheckBoxUpdate()
        {
            if (m_control_basic.IsFixedFunction() == true)
            {
                if (m_chbxNone.Checked)
                {
                    m_chbxIntOnTC.Enabled = false;
                    m_chbxIntOnTC.Checked = false;
                    m_chbxIntOnCmp1.Enabled = false;
                    m_chbxIntOnCmp1.Checked = false;
                    m_chbxIntOnCmp2.Enabled = false;
                    m_chbxIntOnCmp2.Checked = false;
                    m_chbxIntOnKill.Enabled = false;
                    m_chbxIntOnKill.Checked = false;
                    SetAParameter("UseInterrupt", "false");
                    SetAParameter("InterruptOnTC", "false");
                    SetAParameter("InterruptOnCMP1", "false");
                    SetAParameter("InterruptOnCMP2", "false");
                    SetAParameter("InterruptOnKill", "false");
                }
                else
                {
                    m_chbxIntOnTC.Enabled = true;
                    m_chbxIntOnCmp1.Enabled = true;
                    m_chbxIntOnCmp2.Enabled = false;
                    m_chbxIntOnKill.Enabled = false;
                    SetAParameter("UseInterrupt", "true");
                }
            }
            else
            {
                if (m_chbxNone.Checked)
                {
                    m_chbxIntOnTC.Enabled = false;
                    m_chbxIntOnTC.Checked = false;
                    m_chbxIntOnCmp1.Enabled = false;
                    m_chbxIntOnCmp1.Checked = false;
                    m_chbxIntOnCmp2.Enabled = false;
                    m_chbxIntOnCmp2.Checked = false;
                    m_chbxIntOnKill.Enabled = false;
                    m_chbxIntOnKill.Checked = false;
                    SetAParameter("UseInterrupt", "false");
                    SetAParameter("InterruptOnTC", "false");
                    SetAParameter("InterruptOnCMP1", "false");
                    SetAParameter("InterruptOnCMP2", "false");
                    SetAParameter("InterruptOnKill", "false");
                }
                else
                {
                    m_chbxIntOnTC.Enabled = true;
                    m_chbxIntOnCmp1.Enabled = true;
                    Control[] cbpwm = m_control_basic.Controls.Find("m_cbPWMMode", true);
                    if( ((ComboBox)cbpwm[0]).Text.Contains("One") || ((ComboBox)cbpwm[0]).Text.Contains("Dither") || ((ComboBox)cbpwm[0]).Text.Contains("Center"))
                        m_chbxIntOnCmp2.Enabled = false;
                    else
                        m_chbxIntOnCmp2.Enabled = true;
                    
                    if (m_cbKillMode.SelectedIndex != 0)
                        m_chbxIntOnKill.Enabled = true;
                    else
                        m_chbxIntOnKill.Enabled = false;

                    SetAParameter("UseInterrupt", "true");
                }
            }
        }

        private void m_chbxIntOnTC_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked)
                SetAParameter("InterruptOnTC", "true");
            else
                SetAParameter("InterruptOnTC", "false");
        }

        private void m_chbxIntOnCmp1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked)
                SetAParameter("InterruptOnCMP1", "true");
            else
                SetAParameter("InterruptOnCMP1", "false");
        }

        private void m_chbxIntOnCmp2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked)
                SetAParameter("InterruptOnCMP2", "true");
            else
                SetAParameter("InterruptOnCMP2", "false");
        }

        private void m_chbxIntOnKill_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked)
                SetAParameter("InterruptOnKill", "true");
            else
                SetAParameter("InterruptOnKill", "false");
        }
    }
}
