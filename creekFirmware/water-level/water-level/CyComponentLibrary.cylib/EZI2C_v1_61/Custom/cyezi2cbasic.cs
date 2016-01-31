/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace EZI2C_v1_61
{
    public partial class CyEZI2CBasic : UserControl, ICyParamEditingControl
    {
        private CyEZI2CParameters m_params;
        private ICyDeviceQuery_v1 m_deviceQuery;

        #region Constructor(s)
        public CyEZI2CBasic(CyEZI2CParameters inst, ICyDeviceQuery_v1 deviceQuery)
        {
            InitializeComponent();

            inst.m_basicTab = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;
            m_deviceQuery = deviceQuery;
            m_cbPort.DataSource = m_params.m_portsList;
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            if (m_params.m_inst != null)
            {
                foreach (string paramName in m_params.m_inst.GetParamNames())
                {
                    CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                    if (param.TabName.Equals(CyCustomizer.BASIC_TABNAME))
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
        }
        #endregion

        #region Assigning parameters values to controls
        public void GetParams()
        {
            // BusSpeed_kHZ
            m_cbBusSpeed.Text = m_params.m_busSpeed_kHZ.ToString();
            // I2C_Addresses
            m_cbNumberOfAddresses.Text = m_params.m_i2c_Addresses.ToString();

            // I2C_Address1
            if (m_params.m_hex1 && m_params.m_i2c_Address1.HasValue)
                m_tbPrimarySlaveAddress.Text = CyHexConversion.IntToHex(m_params.m_i2c_Address1.Value);
            else
                m_tbPrimarySlaveAddress.Text = m_params.m_i2c_Address1.Value.ToString();

            // I2C_Address2
            if (m_params.m_hex2 && m_params.m_i2c_Address2.HasValue)
                m_tbSecondarySlaveAddress.Text = CyHexConversion.IntToHex(m_params.m_i2c_Address2.Value);
            else
                m_tbSecondarySlaveAddress.Text = m_params.m_i2c_Address2.Value.ToString();

            // Sub_Address_Size
            switch (m_params.m_sub_Address_Size)
            {
                case CyESubAddressSize.Width_8_Bits:
                    m_cbSubAddressSize.Text = "8";
                    break;
                case CyESubAddressSize.Width_16_Bits:
                    m_cbSubAddressSize.Text = "16";
                    break;
                default:
                    Debug.Fail("Unhandled case.");
                    break;
            }

            // I2C_BUS_PORT
            m_cbPort.SelectedItem = m_params.m_i2c_Bus_Port.ToString();

            // EnableWakeup
            m_checkBoxWakeup.Checked = m_params.m_enableWakeup;
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetBusSpeed()
        {
            ushort tmpValue;
            if (ushort.TryParse(m_cbBusSpeed.Text, out tmpValue))
                m_params.m_busSpeed_kHZ = tmpValue;
            else
                m_params.m_busSpeed_kHZ = null;
            m_params.SetParams(CyParamNames.BUS_SPEED_KHZ);
        }

        private void SetNumberOfAddresses()
        {
            m_params.m_i2c_Addresses = int.Parse(m_cbNumberOfAddresses.Text);
            m_params.SetParams(CyParamNames.I2C_ADDRESSES);
        }

        private void SetPrimarySlaveAddress()
        {
            bool isHex = m_tbPrimarySlaveAddress.Text.StartsWith(CyCustomizer.HEX_PREFIX);

            if (isHex)
            {
                m_params.m_i2c_Address1 = CyHexConversion.HexToInt(m_tbPrimarySlaveAddress.Text);
            }
            else
            {
                int tmpValue;
                if (int.TryParse(m_tbPrimarySlaveAddress.Text, out tmpValue))
                    m_params.m_i2c_Address1 = tmpValue;
                else
                    m_params.m_i2c_Address1 = null;
            }
            m_params.SetParams(CyParamNames.I2C_ADDRESS_1);
            if (isHex)
                m_params.m_hex1 = true;
            else
                m_params.m_hex1 = false;
            m_params.SetParams(CyParamNames.HEX1);
        }

        private void SetsecondarySlaveAddress()
        {
            bool isHex = m_tbSecondarySlaveAddress.Text.StartsWith(CyCustomizer.HEX_PREFIX);

            if (isHex)
            {
                m_params.m_i2c_Address2 = CyHexConversion.HexToInt(m_tbSecondarySlaveAddress.Text);
            }
            else
            {
                int tmpValue;
                if (int.TryParse(m_tbSecondarySlaveAddress.Text, out tmpValue))
                    m_params.m_i2c_Address2 = tmpValue;
                else
                    m_params.m_i2c_Address2 = null;
            }
            m_params.SetParams(CyParamNames.I2C_ADDRESS_2);
            if (isHex)
                m_params.m_hex2 = true;
            else
                m_params.m_hex2 = false;
            m_params.SetParams(CyParamNames.HEX2);
        }

        public void SetSubAddressSize()
        {
            switch (m_cbSubAddressSize.Text)
            {
                case "8":
                    m_params.m_sub_Address_Size = CyESubAddressSize.Width_8_Bits;
                    break;
                case "16":
                    m_params.m_sub_Address_Size = CyESubAddressSize.Width_16_Bits;
                    break;
                default:
                    Debug.Fail("Unhandled case.");
                    break;
            }
            m_params.SetParams(CyParamNames.SUB_ADDRESS_SIZE);
        }

        public void SetBusPort()
        {
            m_params.m_i2c_Bus_Port = (CyEI2CBusPort)m_cbPort.SelectedIndex;
            m_params.SetParams(CyParamNames.I2C_BUS_PORT);
        }

        public void SetWakeupMode()
        {
            m_params.m_enableWakeup = m_checkBoxWakeup.Checked;
            m_params.SetParams(CyParamNames.ENABLE_WAKEUP);
        }
        #endregion

        #region Event Handlers
        private void m_cbBusSpeed_TextChanged(object sender, EventArgs e)
        {
            m_cbBusSpeed_Validating(sender, new CancelEventArgs());
            SetBusSpeed();
        }

        private void m_cbNumberOfAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetNumberOfAddresses();

            m_tbSecondarySlaveAddress.Enabled = (m_cbNumberOfAddresses.SelectedIndex == 1);
            RefreshErrorProviders();
            TextBoxValidating(m_tbPrimarySlaveAddress, new CancelEventArgs());
            TextBoxValidating(m_tbSecondarySlaveAddress, new CancelEventArgs());
        }

        private void m_tbPrimarySlaveAddress_TextChanged(object sender, EventArgs e)
        {
            TextBoxValidating(sender, new CancelEventArgs());
            SetPrimarySlaveAddress();
        }

        private void m_tbSecondarySlaveAddress_TextChanged(object sender, EventArgs e)
        {
            TextBoxValidating(sender, new CancelEventArgs());
            SetsecondarySlaveAddress();
        }

        private void m_cbSubAddressSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSubAddressSize();
        }

        private void m_cbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetBusPort();
            RefreshErrorProviders();
        }

        private void m_checkBoxWakeup_CheckedChanged(object sender, EventArgs e)
        {
            SetWakeupMode();
            RefreshErrorProviders();
        }

        private void m_cbBusSpeed_Validating(object sender, CancelEventArgs e)
        {
            if (m_params.IntValidated(m_cbBusSpeed.Text, CyParamNames.BUS_SPEED_KHZ) == false)
            {
                m_errorProvider.SetError((ComboBox)sender, Resources.BusSpeedEP);
                e.Cancel = true;
            }
            else
            {
                m_errorProvider.SetError((ComboBox)sender, string.Empty);
            }
        }

        private void CyEZI2CBasic_Load(object sender, EventArgs e)
        {
            RefreshErrorProviders();
        }

        public bool TextBoxValidating(object sender, CancelEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            bool hex1 = m_tbPrimarySlaveAddress.Text.StartsWith(CyCustomizer.HEX_PREFIX);
            bool hex2 = m_tbSecondarySlaveAddress.Text.StartsWith(CyCustomizer.HEX_PREFIX);
            bool result = true;

            if (m_params.IntValidated(currentTextBox.Text, CyParamNames.I2C_ADDRESS_1) == false)
            {
                string message = string.Empty;
                if (sender == m_tbPrimarySlaveAddress)
                    message = Resources.PrimarySlaveAddressEP;
                else if (sender == m_tbSecondarySlaveAddress)
                    message = Resources.SecondarySlaveAddressEP;

                m_errorProvider.SetError((TextBox)sender, message);
                e.Cancel = true;
                result = false;
            }
            else if (m_tbPrimarySlaveAddress.Text == m_tbSecondarySlaveAddress.Text && m_params.m_i2c_Addresses == 2)
            {
                m_errorProvider.SetError((TextBox)sender, Resources.PrimarySecondaryEqualEP);
            }
            else if (hex1 != hex2)
            {
                if (m_params.IntValidated(currentTextBox.Text, CyParamNames.I2C_ADDRESS_1))
                {
                    int p = 0; // Primary
                    int s = 0; // Secondary
                    if (sender == m_tbPrimarySlaveAddress)
                    {
                        if (hex1)
                            p = CyHexConversion.HexToInt(m_tbPrimarySlaveAddress.Text);
                        else
                            p = int.Parse(m_tbPrimarySlaveAddress.Text);
                        if (hex2)
                        {
                            s = CyHexConversion.HexToInt(m_tbSecondarySlaveAddress.Text);
                        }
                        else
                        {
                            if (m_params.m_i2c_Address2.HasValue)
                                s = m_params.m_i2c_Address2.Value;
                        }
                    }
                    if (sender == m_tbSecondarySlaveAddress)
                    {
                        if (hex2)
                            s = CyHexConversion.HexToInt(m_tbSecondarySlaveAddress.Text);
                        else
                            s = int.Parse(m_tbSecondarySlaveAddress.Text);
                        if (hex1)
                        {
                            p = CyHexConversion.HexToInt(m_tbPrimarySlaveAddress.Text);
                        }
                        else
                        {
                            if (m_params.m_i2c_Address1.HasValue)
                                p = m_params.m_i2c_Address1.Value;
                        }
                    }
                    if (p == s && m_params.m_i2c_Addresses == 2)
                    {
                        m_errorProvider.SetError((TextBox)sender, Resources.PrimarySecondaryEqualEP);
                    }
                    else
                    {
                        ClearTextBoxErrorProviders();
                    }
                }
                else
                {
                    ClearTextBoxErrorProviders();
                }
            }
            else
            {
                ClearTextBoxErrorProviders();
            }
            return result;
        }

        private void ClearTextBoxErrorProviders()
        {
            m_errorProvider.SetError(m_tbPrimarySlaveAddress, string.Empty);
            m_errorProvider.SetError(m_tbSecondarySlaveAddress, string.Empty);
        }
        #endregion

        #region Validating controls
        private void ValidateWakeup()
        {
            if (IsPSoC3ES3())
            {
                if (m_params.m_i2c_Addresses == 2 && m_params.m_i2c_Bus_Port == CyEI2CBusPort.Any
                    && m_params.m_enableWakeup == true)
                {
                    m_errorProvider.SetError(m_checkBoxWakeup, Resources.EnableWakupEP);
                }
                else
                {
                    m_errorProvider.SetError(m_checkBoxWakeup, string.Empty);
                }
            }
            else
            {
                if (m_params.m_enableWakeup == true)
                {
                    m_errorProvider.SetError(m_checkBoxWakeup, Resources.DRCRevisionError);
                    m_checkBoxWakeup.Enabled = true;
                }
                else
                {
                    m_errorProvider.SetError(m_checkBoxWakeup, string.Empty);
                    m_checkBoxWakeup.Enabled = false;
                }
            }
        }

        private void ValidatePinConnections()
        {
            if (IsPSoC3ES3())
            {
                if (m_params.m_i2c_Addresses == 1 && m_params.m_i2c_Bus_Port == CyEI2CBusPort.Any
                    && m_params.m_enableWakeup == true)
                {
                    m_errorProvider.SetError(m_cbPort, Resources.PinConnectionsEP1);
                }
                else if (m_params.m_i2c_Bus_Port != CyEI2CBusPort.Any && m_params.m_enableWakeup == false)
                {
                    m_errorProvider.SetError(m_cbPort, Resources.PinConnectionsEPAndDRC2);
                }
                else
                {
                    m_errorProvider.SetError(m_cbPort, string.Empty);
                }
            }
            else
            {
                if (m_params.m_i2c_Bus_Port != CyEI2CBusPort.Any)
                {
                    m_errorProvider.SetError(m_cbPort, Resources.PinConnectionsPSoC5AndPSoC3ES2EP);
                    m_cbPort.Enabled = true;
                }
                else
                {
                    m_errorProvider.SetError(m_cbPort, string.Empty);
                    m_cbPort.Enabled = false;
                }
            }
        }

        private void ValidateNumberOfAddresses()
        {
            if (IsPSoC3ES3())
            {
                if (m_params.m_i2c_Addresses == 2 && m_params.m_i2c_Bus_Port != CyEI2CBusPort.Any
                    && m_params.m_enableWakeup == true)
                {
                    m_errorProvider.SetError(m_cbNumberOfAddresses, Resources.NumberOfAddressesEP1);
                }
                else
                {
                    m_errorProvider.SetError(m_cbNumberOfAddresses, string.Empty);
                }
            }
        }

        private void RefreshErrorProviders()
        {
            ValidatePinConnections();
            ValidateWakeup();
            ValidateNumberOfAddresses();
        }

        private bool IsPSoC3ES3()
        {
            if ((m_deviceQuery.IsPSoC3 && m_deviceQuery.SiliconRevision < 2)
                || (m_deviceQuery.IsPSoC5 && m_deviceQuery.SiliconRevision < 2))
                return false;
            else
                return true;
        }
        #endregion
    }
}
