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
using CyCustomizer.SPI_Master_v1_10;

namespace CyCustomizer.SPI_Master_v1_10
{
    public partial class CySPIMControlAdv : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;
        public ICyTerminalQuery_v1 m_TermQuery = null;
        public CySPIMControl m_basic_control = null;
        const int SPIM_BUFFERSIZE_MINIMUM = 1;
        const int SPIM_BUFFERSIZE_MAXIMUM = 255;
        const int SPIM_BUFFERSIZE_MAXIMUMARM = 65535;
        
        public CySPIMControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CySPIMControl basic)
        {
            m_Component = inst;
            m_TermQuery = termquery;
            m_basic_control = basic;
            InitializeComponent();
            InitializeFormComponents(m_Component);
            UpdateFormFromParams(m_Component);
        }

        /// <summary>
        /// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the parameters
        /// Also need to handle CyNumericUpDowns to override the UpButton and DownButtons before the value is changed
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.GotFocus += new EventHandler(CySPIMControlAdv_GotFocus);
            this.ParentForm.FormClosing +=new FormClosingEventHandler(ParentForm_FormClosing);
            m_numRXBufferSize.UpEvent += new UpButtonEvent(m_numRXBufferSize_UpEvent);
            m_numRXBufferSize.DownEvent += new DownButtonEvent(m_numRXBufferSize_DownEvent);
            m_numTXBufferSize.UpEvent += new UpButtonEvent(m_numTXBufferSize_UpEvent);
            m_numTXBufferSize.DownEvent += new DownButtonEvent(m_numTXBufferSize_DownEvent);
            m_numRXBufferSize.Maximum = Decimal.MaxValue;
            m_numRXBufferSize.Minimum = Decimal.MinValue;
            m_numTXBufferSize.Minimum = Decimal.MinValue;
            m_numTXBufferSize.Maximum = Decimal.MaxValue;
        }

        void CySPIMControlAdv_GotFocus(object sender, EventArgs e)
        {
            UpdateFormFromParams(m_Component);
        }

        protected void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (ep_Errors.GetError(m_numRXBufferSize) != "")
            {
                m_numRXBufferSize.Focus();
                e.Cancel = true;
            }
            if (ep_Errors.GetError(m_numTXBufferSize) != "")
            {
                m_numTXBufferSize.Focus();
                e.Cancel = true;
            }
        }

        public void InitializeFormComponents(ICyInstEdit_v1 inst)
        {
            //Nothing to Initialize
        }

        void UnhookUpdateEvents()
        {
            m_chbxEnableInternalInterrupt.CheckedChanged -= m_chbxEnableInternalInterrupt_CheckedChanged;
            m_chbxIntOnByteComp.CheckedChanged -= m_chbxIntOnByteComp_CheckedChanged;
            m_chbxIntOnRXEmpty.CheckedChanged -= m_chbxIntOnRXEmpty_CheckedChanged;
            m_chbxIntOnRXNotEmpty.CheckedChanged -= m_chbxIntOnRXNotEmpty_CheckedChanged;
            m_chbxIntOnRXOver.CheckedChanged -= m_chbxIntOnRXOver_CheckedChanged;
            m_chbxIntOnSPIDone.CheckedChanged -= m_chbxIntOnSPIDone_CheckedChanged;
            m_chbxIntOnTXFull.CheckedChanged -= m_chbxIntOnTXFull_CheckedChanged;
            m_chbxIntOnTXNotFull.CheckedChanged -= m_chbxIntOnTXNotFull_CheckedChanged;
            m_numRXBufferSize.ValueChanged -= m_numRXBufferSize_ValueChanged;
            m_numTXBufferSize.ValueChanged -= m_numTXBufferSize_ValueChanged;
            /* Radio buttons are not auto mode so they don't need to be disabled */
        }

        void HookupUpdateEvents()
        {
            m_chbxEnableInternalInterrupt.CheckedChanged += m_chbxEnableInternalInterrupt_CheckedChanged;
            m_chbxIntOnByteComp.CheckedChanged += m_chbxIntOnByteComp_CheckedChanged;
            m_chbxIntOnRXEmpty.CheckedChanged += m_chbxIntOnRXEmpty_CheckedChanged;
            m_chbxIntOnRXNotEmpty.CheckedChanged += m_chbxIntOnRXNotEmpty_CheckedChanged;
            m_chbxIntOnRXOver.CheckedChanged += m_chbxIntOnRXOver_CheckedChanged;
            m_chbxIntOnSPIDone.CheckedChanged += m_chbxIntOnSPIDone_CheckedChanged;
            m_chbxIntOnTXFull.CheckedChanged += m_chbxIntOnTXFull_CheckedChanged;
            m_chbxIntOnTXNotFull.CheckedChanged += m_chbxIntOnTXNotFull_CheckedChanged;
            m_numRXBufferSize.ValueChanged += m_numRXBufferSize_ValueChanged;
            m_numTXBufferSize.ValueChanged += m_numTXBufferSize_ValueChanged;
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookUpdateEvents();
            SPIMParameters prms = new SPIMParameters(m_Component);//inst);
            if (prms.ClockInternal.Value == "true")
                m_rbInternalClock.Checked = true;
            else
                m_rbExternalClock.Checked = true;

            m_numRXBufferSize.Value = Convert.ToInt16(prms.RxBufferSize.Value);
            m_numTXBufferSize.Value = Convert.ToInt16(prms.TxBufferSize.Value);

            m_chbxIntOnByteComp.Checked = prms.InterruptOnByteComplete.Value == "true" ? true : false;
            m_chbxIntOnRXEmpty.Checked = prms.InterruptOnRXEmpty.Value == "true" ? true : false;
            m_chbxIntOnRXOver.Checked = prms.InterruptOnRXOverrun.Value == "true" ? true : false;
            m_chbxIntOnSPIDone.Checked = prms.InterruptOnSPIDone.Value == "true" ? true : false;
            m_chbxIntOnTXNotFull.Checked = prms.InterruptOnTXNotFull.Value == "true" ? true : false;
            m_chbxIntOnTXFull.Checked = prms.InterruptOnTXFull.Value == "true" ? true : false;
            m_chbxIntOnRXNotEmpty.Checked = prms.InterruptOnRXNotEmpty.Value == "true" ? true : false;

            m_chbxEnableInternalInterrupt.Checked = prms.UseInternalInterrupt.Value == "true" ? true : false;

            if (m_numRXBufferSize.Value > 4 || m_numTXBufferSize.Value > 4)
            {
                m_chbxEnableInternalInterrupt.Enabled = false;
                if (m_numRXBufferSize.Value > 4)
                {
                    m_chbxIntOnRXNotEmpty.Enabled = false;
                }
                else
                {
                    m_chbxIntOnRXNotEmpty.Enabled = true;
                }
                if (m_numTXBufferSize.Value > 4)
                {
                    m_chbxIntOnTXNotFull.Enabled = false;
                }
                else
                {
                    m_chbxIntOnTXNotFull.Enabled = true;
                }
            }
            else
            {
                m_chbxEnableInternalInterrupt.Enabled = true;
                m_chbxIntOnRXNotEmpty.Enabled = true;
                m_chbxIntOnTXNotFull.Enabled = true;
            }

            HookupUpdateEvents();
        }

        private void m_rbInternalClock_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbInternalClock.Checked)
            {
                SetAParameter("ClockInternal", "true", false);
                m_basic_control.UpdateFormFromParams(m_Component);
            }
        }

        private void m_rbExternalClock_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbExternalClock.Checked)
            {
                SetAParameter("ClockInternal", "false", false);
                m_basic_control.UpdateFormFromParams(m_Component);
            }
        }

        #region RX Buffer Size Numeric Up_Down
        private void m_numRXBufferSize_ValueChanged(object sender, EventArgs e)
        {
            if (m_numRXBufferSize.Value > 4 || m_numTXBufferSize.Value > 4)
            {
                m_chbxEnableInternalInterrupt.Checked = true;
                m_chbxEnableInternalInterrupt.Enabled = false;
                if (m_numRXBufferSize.Value > 4)
                {
                    m_chbxIntOnRXNotEmpty.Checked = true;
                    m_chbxIntOnRXNotEmpty.Enabled = false;
                }
                else
                {
                    m_chbxIntOnRXNotEmpty.Enabled = true;
                }
            }
            else
            {
                m_chbxEnableInternalInterrupt.Enabled = true;
                m_chbxIntOnRXNotEmpty.Enabled = true;
            }
            CancelEventArgs ce = new CancelEventArgs();
            m_numRXBufferSize_Validating(sender, ce);
            if (!ce.Cancel)
            SetAParameter("RxBufferSize", m_numRXBufferSize.Value.ToString(), false);
        }

        void m_numRXBufferSize_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numRXBufferSize.Value == SPIM_BUFFERSIZE_MINIMUM)
                e.Cancel = true;
        }

        void m_numRXBufferSize_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numRXBufferSize.Value == SPIM_BUFFERSIZE_MAXIMUM)
                e.Cancel = true;
        }
        private void m_numRXBufferSize_KeyUp(object sender, KeyEventArgs e)
        {
            m_numRXBufferSize_Validating(sender, new CancelEventArgs());
        }

        private void m_numRXBufferSize_Validating(object sender, CancelEventArgs e)
        {
            if (m_numRXBufferSize.Value < SPIM_BUFFERSIZE_MINIMUM || m_numRXBufferSize.Value > SPIM_BUFFERSIZE_MAXIMUM)
            {
                ep_Errors.SetError(m_numRXBufferSize, String.Format("RX Buffer Size Must Be Between {0} and {1}", SPIM_BUFFERSIZE_MINIMUM, SPIM_BUFFERSIZE_MAXIMUM));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numRXBufferSize, "");
            }
        } 
        #endregion

        #region TX Buffer Size Numeric Up_Down
        private void m_numTXBufferSize_ValueChanged(object sender, EventArgs e)
        {
            if (m_numRXBufferSize.Value > 4 || m_numTXBufferSize.Value > 4)
            {
                m_chbxEnableInternalInterrupt.Checked = true;
                m_chbxEnableInternalInterrupt.Enabled = false;
                if (m_numTXBufferSize.Value > 4)
                {
                    m_chbxIntOnTXNotFull.Checked = true;
                    m_chbxIntOnTXNotFull.Enabled = false;
                }
                else
                {
                    m_chbxIntOnTXNotFull.Enabled = true;
                }
            }
            else
            {
                m_chbxEnableInternalInterrupt.Enabled = true;
                m_chbxIntOnTXNotFull.Enabled = true;
            }
            CancelEventArgs ce = new CancelEventArgs();
            m_numTXBufferSize_Validating(sender, ce);
            if(!ce.Cancel)
                SetAParameter("TxBufferSize", m_numTXBufferSize.Value.ToString(), false);
        }

        void m_numTXBufferSize_DownEvent(object sender, DownButtonEventArgs e)
        {
            if (m_numTXBufferSize.Value == SPIM_BUFFERSIZE_MINIMUM)
                e.Cancel = true;
        }

        void m_numTXBufferSize_UpEvent(object sender, UpButtonEventArgs e)
        {
            if (m_numTXBufferSize.Value == SPIM_BUFFERSIZE_MAXIMUM)
                e.Cancel = true;
        }

        private void m_numTXBufferSize_Validating(object sender, CancelEventArgs e)
        {
            if (m_numTXBufferSize.Value < SPIM_BUFFERSIZE_MINIMUM || m_numTXBufferSize.Value > SPIM_BUFFERSIZE_MAXIMUM)
            {
                ep_Errors.SetError(m_numTXBufferSize, String.Format("TX Buffer Size Must Be Between {0} and {1}", SPIM_BUFFERSIZE_MINIMUM, SPIM_BUFFERSIZE_MAXIMUM));
                e.Cancel = true;
            }
            else
            {
                ep_Errors.SetError(m_numTXBufferSize, "");
            }
        }

        private void m_numTXBufferSize_KeyUp(object sender, KeyEventArgs e)
        {
            m_numTXBufferSize_Validating(sender, new CancelEventArgs());
        } 
        #endregion


        private void SetAParameter(string parameter, string value, bool CheckFocus)
        {
            if (this.ContainsFocus || !CheckFocus)
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

        private void m_chbxIntOnTXFull_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnTXFull", m_chbxIntOnTXFull.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnTXNotFull_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnTXNotFull", m_chbxIntOnTXNotFull.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnSPIDone_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnSPIDone", m_chbxIntOnSPIDone.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnRXOver_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnRXOverrun", m_chbxIntOnRXOver.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnRXEmpty_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnRXEmpty", m_chbxIntOnRXEmpty.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnByteComp_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnByteComplete", m_chbxIntOnByteComp.Checked ? "true" : "false", false);
        }

        private void m_chbxIntOnRXNotEmpty_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("InterruptOnRXNotEmpty", m_chbxIntOnRXNotEmpty.Checked ? "true" : "false", false);
        }

        private void m_chbxEnableInternalInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter("UseInternalInterrupt", m_chbxEnableInternalInterrupt.Checked ? "true" : "false", false);
        }
    }
}
