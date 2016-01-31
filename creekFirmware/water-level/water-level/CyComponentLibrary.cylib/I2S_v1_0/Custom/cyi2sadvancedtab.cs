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
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace I2S_v1_0
{
    public partial class cyi2sadvancedtab : UserControl, ICyParamEditingControl
    {
        private Control m_control;
        private CyI2SParameters m_params;

        public cyi2sadvancedtab(CyI2SParameters inst)
        {
            InitializeComponent();

            ((CyI2SParameters)inst).m_advancedParams = this;
            m_control = this;
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
            return new CyCustErr[] { };    //return an empty array
        }

        #endregion

        #region Assigning parameters values to controls

        public void GetParams()
        {
            // Data Interleaving GroupBoxes
            switch (m_params.m_rxDataInterleaving)
            {
                case E_DATA_INTERLEAVING.Interleaved:
                    radioButtonRxInterleaved.Checked = true;
                    break;
                case E_DATA_INTERLEAVING.Separate:
                    radioButtonRxSeparatedLR.Checked = true;
                    break;
            }
            switch (m_params.m_txDataInterleaving)
            {
                case E_DATA_INTERLEAVING.Interleaved:
                    radioButtonTxInterleaved.Checked = true;
                    break;
                case E_DATA_INTERLEAVING.Separate:
                    radioButtonTxSeparatedLR.Checked = true;
                    break;
            }

            // DMA Request GroupBoxes
            switch (m_params.m_rxDMA)
            {
                case E_DMA_PRESENT.DMA:
                    checkBoxRxDMA.Checked = true;
                    break;
                case E_DMA_PRESENT.NO_DMA:
                    checkBoxRxDMA.Checked = false;
                    break;
            }
            switch (m_params.m_txDMA)
            {
                case E_DMA_PRESENT.DMA:
                    checkBoxTxDMA.Checked = true;
                    break;
                case E_DMA_PRESENT.NO_DMA:
                    checkBoxTxDMA.Checked = false;
                    break;
            }

            // Interrupt Source CheckBoxes
            checkBoxISTxUnderflow.Checked = m_params.m_ISReg[0];
            checkBoxTxFIFO0.Checked = m_params.m_ISReg[1];
            checkBoxTxFIFO1.Checked = m_params.m_ISReg[2];
            checkBoxISRxOverflow.Checked = m_params.m_ISReg[3];
            checkBoxRxFIFO0.Checked = m_params.m_ISReg[4];
            checkBoxRxFIFO1.Checked = m_params.m_ISReg[5];

            SetEnableDisable(m_params.m_direction);
        }

        #endregion

        #region Assigning controls values to parameters

        private void SetRxDataInterleaving()
        {
            if (radioButtonRxSeparatedLR.Checked)
                m_params.m_rxDataInterleaving = E_DATA_INTERLEAVING.Separate;
            else
                m_params.m_rxDataInterleaving = E_DATA_INTERLEAVING.Interleaved;
            m_params.SetParams(CyParamNames.RX_DATA_INTERLEAVING);
        }

        private void SetTxDataInterleaving()
        {
            if (radioButtonTxSeparatedLR.Checked)
                m_params.m_txDataInterleaving = E_DATA_INTERLEAVING.Separate;
            else
                m_params.m_txDataInterleaving = E_DATA_INTERLEAVING.Interleaved;
            m_params.SetParams(CyParamNames.TX_DATA_INTERLEAVING);
        }

        private void SetRxDMARequest()
        {
            if (checkBoxRxDMA.Checked)
                m_params.m_rxDMA = E_DMA_PRESENT.DMA;
            else
                m_params.m_rxDMA = E_DMA_PRESENT.NO_DMA;
            m_params.SetParams(CyParamNames.RX_DMA_PRESENT);
        }

        private void SetTxDMARequest()
        {
            if (checkBoxTxDMA.Checked)
                m_params.m_txDMA = E_DMA_PRESENT.DMA;
            else
                m_params.m_txDMA = E_DMA_PRESENT.NO_DMA;
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

            m_params.m_ISReg[num] = currentCheckBox.Checked;

            m_params.m_interruptSource = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (m_params.m_ISReg[i]) m_params.m_interruptSource |= 0x01;
                if (i > 0) m_params.m_interruptSource <<= 1;
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
                    groupBoxDIRX.Enabled = true;
                    radioButtonRxInterleaved.Enabled = true;
                    radioButtonRxSeparatedLR.Enabled = true;
                    checkBoxRxDMA.Enabled = true;
                    groupBoxISRX.Enabled = true;
                    checkBoxISRxOverflow.Enabled = true;
                    checkBoxRxFIFO0.Enabled = true;
                    checkBoxRxFIFO1.Enabled = true;

                    groupBoxDITX.Enabled = false;
                    radioButtonTxInterleaved.Enabled = false;
                    radioButtonTxSeparatedLR.Enabled = false;
                    checkBoxTxDMA.Enabled = false;
                    groupBoxISTX.Enabled = false;
                    checkBoxISTxUnderflow.Enabled = false;
                    checkBoxTxFIFO0.Enabled = false;
                    checkBoxTxFIFO1.Enabled = false;

                    if (radioButtonRxInterleaved.Checked)
                        checkBoxRxFIFO1.Enabled = false;
                    else
                        checkBoxRxFIFO1.Enabled = true;

                    if (radioButtonTxInterleaved.Checked)
                        checkBoxTxFIFO1.Enabled = false;
                    else
                        checkBoxTxFIFO1.Enabled = true;
                    break;
                case E_DIRECTION.Tx:
                    groupBoxDIRX.Enabled = false;
                    radioButtonRxInterleaved.Enabled = false;
                    radioButtonRxSeparatedLR.Enabled = false;
                    checkBoxRxDMA.Enabled = false;
                    groupBoxISRX.Enabled = false;
                    checkBoxISRxOverflow.Enabled = false;
                    checkBoxRxFIFO0.Enabled = false;
                    checkBoxRxFIFO1.Enabled = false;

                    groupBoxDITX.Enabled = true;
                    radioButtonTxInterleaved.Enabled = true;
                    radioButtonTxSeparatedLR.Enabled = true;
                    checkBoxTxDMA.Enabled = true;
                    groupBoxISTX.Enabled = true;
                    checkBoxISTxUnderflow.Enabled = true;
                    checkBoxTxFIFO0.Enabled = true;
                    checkBoxTxFIFO1.Enabled = true;

                    if (radioButtonRxInterleaved.Checked)
                        checkBoxRxFIFO1.Enabled = false;
                    else
                        checkBoxRxFIFO1.Enabled = true;

                    if (radioButtonTxInterleaved.Checked)
                        checkBoxTxFIFO1.Enabled = false;
                    else
                        checkBoxTxFIFO1.Enabled = true;
                    break;
                default:
                    groupBoxDIRX.Enabled = true;
                    radioButtonRxInterleaved.Enabled = true;
                    radioButtonRxSeparatedLR.Enabled = true;
                    checkBoxRxDMA.Enabled = true;
                    groupBoxISRX.Enabled = true;
                    checkBoxISRxOverflow.Enabled = true;
                    checkBoxRxFIFO0.Enabled = true;
                    checkBoxRxFIFO1.Enabled = true;

                    groupBoxDITX.Enabled = true;
                    radioButtonTxInterleaved.Enabled = true;
                    radioButtonTxSeparatedLR.Enabled = true;
                    checkBoxTxDMA.Enabled = true;
                    groupBoxISTX.Enabled = true;
                    checkBoxISTxUnderflow.Enabled = true;
                    checkBoxTxFIFO0.Enabled = true;
                    checkBoxTxFIFO1.Enabled = true;

                    if (radioButtonRxInterleaved.Checked)
                        checkBoxRxFIFO1.Enabled = false;
                    else
                        checkBoxRxFIFO1.Enabled = true;

                    if (radioButtonTxInterleaved.Checked)
                        checkBoxTxFIFO1.Enabled = false;
                    else
                        checkBoxTxFIFO1.Enabled = true;
                    break;
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
