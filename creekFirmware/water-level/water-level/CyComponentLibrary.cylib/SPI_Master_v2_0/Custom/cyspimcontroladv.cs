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

namespace SPI_Master_v2_0
{
    public partial class CySPIMControlAdv : UserControl, ICyParamEditingControl
    {
        private CySPIMParameters m_params;

        #region Constructor(s)
        public CySPIMControlAdv(CySPIMParameters inst)
        {
            InitializeComponent();

            inst.m_advTab = this;
            this.Dock = DockStyle.Fill;
            this.AutoScrollMinSize = new Size(this.Width - 20, this.Height);
            m_params = inst;
            numRXBufferSize.TextChanged += new EventHandler(numRXBufferSize_TextChanged);
            numTXBufferSize.TextChanged += new EventHandler(numTXBufferSize_TextChanged);
            // NumericsUpDown Settings
            numRXBufferSize.Minimum = 0;
            numRXBufferSize.Maximum = uint.MaxValue;
            numTXBufferSize.Minimum = 0;
            numTXBufferSize.Maximum = uint.MaxValue;
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(CyCustomizer.ADVANCED_TABNAME))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            yield return new CyCustErr(errMsg);
                        }
                    }
                }
            }
        }
        #endregion

        #region Assigning parameters values to controls
        public void GetParams()
        {
            // ClockInternal
            rbInternalClock.Checked = m_params.ClockInternal;
            rbExternalClock.Checked = !rbInternalClock.Checked;
            m_params.m_basicTab.SetBitRateAvailability();

            // RxBufferSize
            numRXBufferSize.Value = (decimal)m_params.RxBufferSize;

            // TxBufferSize
            numTXBufferSize.Value = (decimal)m_params.TxBufferSize;

            // UseInternalInterrupt
            chbxEnableTXInternalInterrupt.Checked = m_params.UseTxInternalInterrupt;

            // UseInternalInterrupt
            chbxEnableRXInternalInterrupt.Checked = m_params.UseRxInternalInterrupt;

            // InterruptOnTXFull
            chbxIntOnRXFull.Checked = m_params.InterruptOnRXFull;

            // InterruptOnTXNotFull
            chbxIntOnTXNotFull.Checked = m_params.InterruptOnTXNotFull;

            // InterruptOnSPIDone
            chbxIntOnSPIDone.Checked = m_params.InterruptOnSPIDone;

            // InterruptOnRXOverrun
            chbxIntOnRXOverrun.Checked = m_params.InterruptOnRXOverrun;

            // InterruptOnRXEmpty
            chbxIntOnTXEmpty.Checked = m_params.InterruptOnTXEmpty;

            // InterruptOnRXNotEmpty
            chbxIntOnRXNotEmpty.Checked = m_params.InterruptOnRXNotEmpty;

            // InterruptOnByteComplete
            chbxIntOnByteComplete.Checked = m_params.InterruptOnByteComplete;
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetClockInternal()
        {
            m_params.ClockInternal = rbInternalClock.Checked;
            m_params.SetParams(CyParamNames.CLOCK_INTERNAL);
        }

        private void SetRxBufferSize()
        {
            try
            {
                m_params.RxBufferSize = uint.Parse(numRXBufferSize.Text);
            }
            catch (Exception) 
            {
                m_params.RxBufferSize = null;
            }
            m_params.SetParams(CyParamNames.RX_BUFFER_SIZE);
        }

        private void SetTxBufferSize()
        {
            try
            {
                m_params.TxBufferSize = uint.Parse(numTXBufferSize.Text);
            }
            catch (Exception)
            {
                m_params.TxBufferSize = null;
            }
            m_params.SetParams(CyParamNames.TX_BUFFER_SIZE);
        }

        private void SetUseTXInternalInterrupt()
        {
            m_params.UseTxInternalInterrupt = chbxEnableTXInternalInterrupt.Checked;
            m_params.SetParams(CyParamNames.USE_TX_INTERNAL_INTERRUPT);
        }

        private void SetUseRXInternalInterrupt()
        {
            m_params.UseRxInternalInterrupt = chbxEnableRXInternalInterrupt.Checked;
            m_params.SetParams(CyParamNames.USE_RX_INTERNAL_INTERRUPT);
        }

        private void SetInterruptOnTXFull()
        {
            m_params.InterruptOnRXFull = chbxIntOnRXFull.Checked;
            m_params.SetParams(CyParamNames.INTERRUPT_ON_RX_FULL);
        }
        private void SetInterruptOnTXNotFull()
        {
            m_params.InterruptOnTXNotFull = chbxIntOnTXNotFull.Checked;
            m_params.SetParams(CyParamNames.INTERRUPT_ON_TX_NOT_FULL);
        }

        private void SetInterruptOnSPIDone()
        {
            m_params.InterruptOnSPIDone = chbxIntOnSPIDone.Checked;
            m_params.SetParams(CyParamNames.INTERRUPT_ON_SPI_DONE);
        }

        private void SetInterruptOnRXOverrun()
        {
            m_params.InterruptOnRXOverrun = chbxIntOnRXOverrun.Checked;
            m_params.SetParams(CyParamNames.INTERRUPT_ON_RX_OVERRUN);
        }

        private void SetInterruptOnRXEmpty()
        {
            m_params.InterruptOnTXEmpty = chbxIntOnTXEmpty.Checked;
            m_params.SetParams(CyParamNames.INTERRUPT_ON_TX_EMPTY);
        }

        private void SetInterruptOnRXNotEmpty()
        {
            m_params.InterruptOnRXNotEmpty = chbxIntOnRXNotEmpty.Checked;
            m_params.SetParams(CyParamNames.INTERRUPT_ON_RX_NOT_EMPTY);
        }

        private void SetInterruptOnByteComplete()
        {
            m_params.InterruptOnByteComplete = chbxIntOnByteComplete.Checked;
            m_params.SetParams(CyParamNames.INTERRUPT_ON_BYTE_COMPLETE);
        }
        #endregion

        #region Event Handlers
        private bool NumUpDownValidated(object sender)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            string message = "";
            bool result = false;
            int value = 0;
            if (numericUpDown == numRXBufferSize)
            {
                message = String.Format(Properties.Resources.RXBufferSizeEPMsg,
                    CyParamRange.BUFFER_SIZE_MIN, CyParamRange.BUFFER_SIZE_MAX);
            }
            else if (numericUpDown == numTXBufferSize)
            {
                message = String.Format(Properties.Resources.TXBufferSizeEPMsg,
                    CyParamRange.BUFFER_SIZE_MIN, CyParamRange.BUFFER_SIZE_MAX);
            }
            if (int.TryParse(numericUpDown.Text, out value))
            {
                if (value < CyParamRange.BUFFER_SIZE_MIN || value > CyParamRange.BUFFER_SIZE_MAX)
                {
                    ep_Errors.SetError(numericUpDown, message);
                }
                else
                {
                    ep_Errors.SetError(numericUpDown, "");
                    result = true;
                }
            }
            else
            { ep_Errors.SetError(numericUpDown, message); }
            return result;
        }

        private void numRXBufferSize_TextChanged(object sender, EventArgs e)
        {
            if (NumUpDownValidated(numRXBufferSize))
            {
                int rxVal = int.Parse(numRXBufferSize.Text);

                bool on = (rxVal > 4);

                chbxEnableRXInternalInterrupt.Checked = on;
                chbxEnableRXInternalInterrupt.Enabled = !on;

                chbxIntOnRXNotEmpty.Checked = on;
                chbxIntOnRXNotEmpty.Enabled = !on;
            }
            SetRxBufferSize();
        }

        private void numTXBufferSize_TextChanged(object sender, EventArgs e)
        {
            if (NumUpDownValidated(numTXBufferSize))
            {
                int txVal = int.Parse(numTXBufferSize.Text);

                bool on = (txVal > 4);

                chbxEnableTXInternalInterrupt.Checked = on;
                chbxEnableTXInternalInterrupt.Enabled = !on;

                chbxIntOnTXNotFull.Checked = on;
                chbxIntOnTXNotFull.Enabled = !on;
            }
            SetTxBufferSize();
        }

        private void rbInternalClock_CheckedChanged(object sender, EventArgs e)
        {
            SetClockInternal();
            m_params.m_basicTab.SetBitRateAvailability();
        }

        private void chbxIntOnTXFull_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptOnTXFull();
        }

        private void chbxIntOnTXNotFull_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptOnTXNotFull();
        }

        private void chbxIntOnSPIDone_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptOnSPIDone();
        }

        private void chbxIntOnRXOverrun_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptOnRXOverrun();
        }

        private void chbxIntOnRXEmpty_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptOnRXEmpty();
        }

        private void chbxIntOnRXNotEmpty_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptOnRXNotEmpty();
        }

        private void chbxIntOnByteComplete_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptOnByteComplete();
        }

        private void chbxEnableTXInternalInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            SetUseTXInternalInterrupt();
        }

        private void chbxEnableRXInternalInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            SetUseRXInternalInterrupt();
        }

        private void numUpDown_Validating(object sender, CancelEventArgs e)
        {
            NumUpDownValidated(sender);
        }

        private void CySPIMControlAdv_Load(object sender, EventArgs e)
        {
            // Set UseInternalInterrupt parameter to false to allow customizer ignore it next time 
            if (m_params.m_useInternalInterrupt)
            {
                SetUseRXInternalInterrupt();
                SetUseTXInternalInterrupt();
                m_params.SetParams(CyParamNames.USE_INTERNAL_INTERRUPT);
            }
        }
        #endregion

        #region Control Overrided Methods
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}
