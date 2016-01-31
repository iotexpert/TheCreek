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
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace I2S_v2_0
{
    public partial class CyI2SAdvanced : UserControl, ICyParamEditingControl
    {
        private CyI2SParameters m_params;

        public CyI2SAdvanced(CyI2SParameters inst)
        {
            InitializeComponent();

            inst.m_advancedParams = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals("Advanced"))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }
            }

            return errs;
        }
        #endregion

        #region Assigning parameters values to controls
        public void GetParams()
        {
            // Data Interleaving GroupBoxes
            switch (m_params.RxDataInterleaving)
            {
                case E_DATA_INTERLEAVING.Interleaved:
                    radioButtonRxInterleaved.Checked = true;
                    break;
                case E_DATA_INTERLEAVING.Separate:
                    radioButtonRxSeparatedLR.Checked = true;
                    break;
            }
            switch (m_params.TxDataInterleaving)
            {
                case E_DATA_INTERLEAVING.Interleaved:
                    radioButtonTxInterleaved.Checked = true;
                    break;
                case E_DATA_INTERLEAVING.Separate:
                    radioButtonTxSeparatedLR.Checked = true;
                    break;
            }

            // DMA Request GroupBoxes
            switch (m_params.RxDMA)
            {
                case E_DMA_PRESENT.DMA:
                    checkBoxRxDMA.Checked = true;
                    break;
                case E_DMA_PRESENT.NO_DMA:
                    checkBoxRxDMA.Checked = false;
                    break;
            }
            switch (m_params.TxDMA)
            {
                case E_DMA_PRESENT.DMA:
                    checkBoxTxDMA.Checked = true;
                    break;
                case E_DMA_PRESENT.NO_DMA:
                    checkBoxTxDMA.Checked = false;
                    break;
            }

            // Interrupt Source CheckBoxes
            checkBoxISTxUnderflow.Checked = m_params.m_isReg[0];
            checkBoxTxFIFO0.Checked = m_params.m_isReg[1];
            checkBoxTxFIFO1.Checked = m_params.m_isReg[2];
            checkBoxISRxOverflow.Checked = m_params.m_isReg[3];
            checkBoxRxFIFO0.Checked = m_params.m_isReg[4];
            checkBoxRxFIFO1.Checked = m_params.m_isReg[5];

            SetEnableDisable(m_params.Direction);
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetRxDataInterleaving()
        {
            m_params.RxDataInterleaving = radioButtonRxSeparatedLR.Checked ?
                E_DATA_INTERLEAVING.Separate : E_DATA_INTERLEAVING.Interleaved;
            m_params.SetParams(CyParamNames.RX_DATA_INTERLEAVING);
        }

        private void SetTxDataInterleaving()
        {
            m_params.TxDataInterleaving = radioButtonTxSeparatedLR.Checked ?
                E_DATA_INTERLEAVING.Separate : E_DATA_INTERLEAVING.Interleaved;
            m_params.SetParams(CyParamNames.TX_DATA_INTERLEAVING);
        }

        private void SetRxDMARequest()
        {
            m_params.RxDMA = checkBoxRxDMA.Checked ? E_DMA_PRESENT.DMA : E_DMA_PRESENT.NO_DMA;
            m_params.SetParams(CyParamNames.RX_DMA_PRESENT);
        }

        private void SetTxDMARequest()
        {
            m_params.TxDMA = checkBoxTxDMA.Checked ? E_DMA_PRESENT.DMA : E_DMA_PRESENT.NO_DMA;
            m_params.SetParams(CyParamNames.TX_DMA_PRESENT);
        }

        public void SetInterruptSource(object sender)
        {
            CheckBox currentCheckBox = (CheckBox)sender;
            int num = 0;

            if (sender == checkBoxISTxUnderflow) num = 0;
            else if (sender == checkBoxTxFIFO0) num = 1;
            else if (sender == checkBoxTxFIFO1) num = 2;
            else if (sender == checkBoxISRxOverflow) num = 3;
            else if (sender == checkBoxRxFIFO0) num = 4;
            else if (sender == checkBoxRxFIFO1) num = 5;

            m_params.m_isReg[num] = currentCheckBox.Checked;
            // Collecting bits to byte
            m_params.InterruptSource = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (m_params.m_isReg[i]) m_params.InterruptSource |= 0x01;
                if (i > 0) m_params.InterruptSource <<= 1;
            }
            m_params.SetParams(CyParamNames.INTERRUPT_SOURCE);
        }
        #endregion

        #region Event Handlers
        private void radioButtonRxInterleaved_CheckedChanged(object sender, EventArgs e)
        {
            SetRxDataInterleaving();
            if (radioButtonRxInterleaved.Checked)
            {
                checkBoxRxFIFO1.Checked = false;
                checkBoxRxFIFO1.Enabled = false;
            }
            else
                checkBoxRxFIFO1.Enabled = true;
        }

        private void radioButtonTxInterleaved_CheckedChanged(object sender, EventArgs e)
        {
            SetTxDataInterleaving();
            if (radioButtonTxInterleaved.Checked)
            {
                checkBoxTxFIFO1.Checked = false;
                checkBoxTxFIFO1.Enabled = false;
            }
            else
                checkBoxTxFIFO1.Enabled = true;
        }

        private void checkBoxRxDMA_CheckedChanged(object sender, EventArgs e)
        {
            SetRxDMARequest();
        }

        private void checkBoxTxDMA_CheckedChanged(object sender, EventArgs e)
        {
            SetTxDMARequest();
        }

        private void checkBoxInterruptSource_CheckedChanged(object sender, EventArgs e)
        {
            SetInterruptSource(sender);
        }
        #endregion

        #region Enabling/Disabling Controls on Advanced tab
        public void SetEnableDisable(E_DIRECTION value)
        {
            switch (value)
            {
                case E_DIRECTION.Rx:
                    // Hide Tx group (right side)
                    RxGroupVisibility(true);
                    TxGroupVisibility(false);
                    checkBoxRxFIFO1.Enabled = !radioButtonRxInterleaved.Checked;
                    checkBoxTxFIFO1.Enabled = !radioButtonTxInterleaved.Checked;
                    break;
                case E_DIRECTION.Tx:
                    // Hide Rx group (left side)
                    RxGroupVisibility(false);
                    TxGroupVisibility(true);
                    checkBoxRxFIFO1.Enabled = !radioButtonRxInterleaved.Checked;
                    checkBoxTxFIFO1.Enabled = !radioButtonTxInterleaved.Checked;
                    break;
                default:
                    // Show both groups
                    RxGroupVisibility(true);
                    TxGroupVisibility(true);
                    checkBoxRxFIFO1.Enabled = !radioButtonRxInterleaved.Checked;
                    checkBoxTxFIFO1.Enabled = !radioButtonTxInterleaved.Checked;
                    break;
            }
        }

        private void TxGroupVisibility(bool value)
        {
            groupBoxDITX.Enabled = value;
            radioButtonTxInterleaved.Enabled = value;
            radioButtonTxSeparatedLR.Enabled = value;
            checkBoxTxDMA.Enabled = value;
            groupBoxISTX.Enabled = value;
            checkBoxISTxUnderflow.Enabled = value;
            checkBoxTxFIFO0.Enabled = value;
            checkBoxTxFIFO1.Enabled = value;
        }

        private void RxGroupVisibility(bool value)
        {
            groupBoxDIRX.Enabled = value;
            radioButtonRxInterleaved.Enabled = value;
            radioButtonRxSeparatedLR.Enabled = value;
            checkBoxRxDMA.Enabled = value;
            groupBoxISRX.Enabled = value;
            checkBoxISRxOverflow.Enabled = value;
            checkBoxRxFIFO0.Enabled = value;
            checkBoxRxFIFO1.Enabled = value;
        }
        #endregion
    }
}
