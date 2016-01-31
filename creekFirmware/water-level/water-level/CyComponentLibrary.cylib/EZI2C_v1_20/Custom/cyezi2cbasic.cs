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

namespace EZI2C_v1_20
{
    public partial class CyEZI2CBasic : UserControl, ICyParamEditingControl
    {
        private Control m_control;
        private CyEZI2CParameters m_params;
        private bool m_nonNumberEntered = false;

        public CyEZI2CBasic(CyEZI2CParameters inst)
        {
            InitializeComponent();

            ((CyEZI2CParameters)inst).m_basicTab = this;
            m_control = this;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
            m_params = inst;
            errorProvider.SetIconAlignment(cbBusSpeed, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetIconAlignment(tbPrimarySlaveAddress, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetIconAlignment(tbSecondarySlaveAddress, ErrorIconAlignment.MiddleLeft);
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            if (errorProvider.GetError(cbBusSpeed) != "")
                return new CyCustErr[] { new CyCustErr(errorProvider.GetError(cbBusSpeed)) };
            if (errorProvider.GetError(tbPrimarySlaveAddress) != "")
                return new CyCustErr[] { new CyCustErr(errorProvider.GetError(tbPrimarySlaveAddress)) };
            if (errorProvider.GetError(tbSecondarySlaveAddress) != "")
                return new CyCustErr[] { new CyCustErr(errorProvider.GetError(tbSecondarySlaveAddress)) };
            return new CyCustErr[] { };    //return an empty array
        }

        #endregion

        #region Assigning parameters values to controls

        public void GetParams()
        {
            // BusSpeed_kHZ
            cbBusSpeed.Text = m_params.m_BusSpeed_kHZ.ToString();
            // I2C_Addresses
            cbNumberOfAddresses.Text = m_params.m_I2C_Addresses.ToString();

            // I2C_Address1
            tbPrimarySlaveAddress.Text = m_params.m_I2C_Address1.ToString();

            // I2C_Address2
            tbSecondarySlaveAddress.Text = m_params.m_I2C_Address2.ToString();

            // Sub_Address_Size
            switch (m_params.m_Sub_Address_Size)
            {
                case E_SUB_ADDRESS_SIZE.Width_8_Bits:
                    cbSubAddressSize.Text = "8";
                    break;
                case E_SUB_ADDRESS_SIZE.Width_16_Bits:
                    cbSubAddressSize.Text = "16";
                    break;
                default:
                    break;
            }

            // EnableWakeup
            checkBoxWakeup.Checked = m_params.m_EnableWakeup;
        }

        #endregion

        #region Assigning controls values to parameters

        private void SetBusSpeed()
        {
            m_params.m_BusSpeed_kHZ = int.Parse(cbBusSpeed.Text);
            m_params.SetParams(CyParamNames.BUS_SPEED_KHZ);
        }

        private void SetNumberOfAddresses()
        {
            m_params.m_I2C_Addresses = int.Parse(cbNumberOfAddresses.Text);
            m_params.SetParams(CyParamNames.I2C_ADDRESSES);
        }

        private void SetPrimarySlaveAddress()
        {
            m_params.m_I2C_Address1 = int.Parse(tbPrimarySlaveAddress.Text);
            m_params.SetParams(CyParamNames.I2C_ADDRESS_1);
        }

        private void SetsecondarySlaveAddress()
        {
            m_params.m_I2C_Address2 = int.Parse(tbSecondarySlaveAddress.Text);
            m_params.SetParams(CyParamNames.I2C_ADDRESS_2);
        }

        public void SetSubAddressSize()
        {
            switch (cbSubAddressSize.Text)
            {
                case "8":
                    m_params.m_Sub_Address_Size = E_SUB_ADDRESS_SIZE.Width_8_Bits;
                    break;
                case "16":
                    m_params.m_Sub_Address_Size = E_SUB_ADDRESS_SIZE.Width_16_Bits;
                    break;
            }
            m_params.SetParams(CyParamNames.SUB_ADDRESS_SIZE);
        }

        public void SetWakeupMode()
        {
            m_params.m_EnableWakeup = checkBoxWakeup.Checked;
            m_params.SetParams(CyParamNames.ENABLE_WAKEUP);
        }

        #endregion

        #region Event Handlers

        private void cbBusSpeed_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            ComboBoxValidating(sender, ce);
            if (ce.Cancel == false)
                SetBusSpeed();
        }

        private void cbNumberOfAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbNumberOfAddresses.SelectedIndex == 1)
            {
                tbSecondarySlaveAddress.Enabled = true;
                checkBoxWakeup.Enabled = false;
            }
            else
            {
                tbSecondarySlaveAddress.Enabled = false;
                checkBoxWakeup.Enabled = true;
            }

            SetNumberOfAddresses();
        }

        private void tbPrimarySlaveAddress_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            TextBoxValidating(sender, ce);
            if (ce.Cancel == false)
                SetPrimarySlaveAddress();
        }

        private void tbSecondarySlaveAddress_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            TextBoxValidating(sender, ce);
            if (ce.Cancel == false)
                SetsecondarySlaveAddress();
        }

        private void cbSubAddressSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSubAddressSize();
        }

        private void checkBoxWakeup_CheckedChanged(object sender, EventArgs e)
        {
            SetWakeupMode();
        }

        public void ComboBoxValidating(object sender, CancelEventArgs e)
        {
            if (sender == cbBusSpeed)
            {
                if (m_params.IntValidated(cbBusSpeed.Text, CyParamNames.BUS_SPEED_KHZ) == false)
                {
                    errorProvider.SetError((ComboBox)sender, string.Format(
                        "An I2C bus speed must be between 50 and 400 kHz"));
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError((ComboBox)sender, "");
                }
            }
        }

        public void TextBoxValidating(object sender, CancelEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;

            if (m_params.IntValidated(currentTextBox.Text, CyParamNames.I2C_ADDRESS_1) == false)
            {
                string message = "";
                if (sender == tbPrimarySlaveAddress)
                    message = "Primary slave address must be between 0 and 127";
                else if (sender == tbSecondarySlaveAddress)
                    message = "Secondary slave address must be between 0 and 127";

                errorProvider.SetError((TextBox)sender, string.Format(message));
                e.Cancel = true;
            }
            else if (tbPrimarySlaveAddress.Text == tbSecondarySlaveAddress.Text)
            {
                errorProvider.SetError((TextBox)sender, string.Format(
                    "Primary slave and Secondary slave addresses cannot be equal"));
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(tbPrimarySlaveAddress, "");
                errorProvider.SetError(tbSecondarySlaveAddress, "");
            }
        }

        private void EditBox_KeyDown(object sender, KeyEventArgs e)
        {
            ValidateKey(e);
        }

        private void EditBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (m_nonNumberEntered) e.Handled = true;
        }

        #endregion

        #region Validators

        /// <summary>
        /// Validates whether a number key or other has been pressed.
        /// </summary>
        /// <param name="e"></param>
        private void ValidateKey(KeyEventArgs e)
        {
            m_nonNumberEntered = false;
            
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back)
                    {
                        m_nonNumberEntered = true;
                    }
                }
            }
            // Not a number, because shift pressed
            if (Control.ModifierKeys == Keys.Shift)
            {
                m_nonNumberEntered = true;
            }
        }

        #endregion
    }
}
