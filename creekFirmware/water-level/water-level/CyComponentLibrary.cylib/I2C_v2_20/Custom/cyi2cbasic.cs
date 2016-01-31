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
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace I2C_v2_20
{
    public partial class CyI2CBasic : UserControl, ICyParamEditingControl
    {
        private CyI2CParameters m_params;
        private bool m_nonNumberEntered = false;

        #region Constructor(s)
        public CyI2CBasic(CyI2CParameters inst)
        {
            InitializeComponent();

            inst.m_basicTab = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;
            // Assigning ModeType possible values to ComboBox
            cbMode.Items.AddRange(new string[4] {
                CyI2CModesDisplayName.SLAVE,
                CyI2CModesDisplayName.MASTER,
                CyI2CModesDisplayName.MULTI_MASTER,
                CyI2CModesDisplayName.MULTI_MASTER_SLAVE
            });
            chbWakeup.Enabled = rbFixedFunction.Checked;
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
            List<CyCustErr> errs = new List<CyCustErr>();

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals("Basic Configuration"))
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
            // I2C_Mode
            switch (m_params.Mode)
            {
                case CyEModeType.Slave_revA:
                    cbMode.SelectedItem = CyI2CModesDisplayName.SLAVE;
                    break;
                case CyEModeType.Master_revA:
                    cbMode.SelectedItem = CyI2CModesDisplayName.MASTER;
                    break;
                case CyEModeType.MultiMaster_revA:
                    cbMode.SelectedItem = CyI2CModesDisplayName.MULTI_MASTER;
                    break;
                case CyEModeType.MultiMaster_Slave_revA:
                    cbMode.SelectedItem = CyI2CModesDisplayName.MULTI_MASTER_SLAVE;
                    break;
                default:
                    break;
            }

            // BusSpeed
            cbBusSpeed.Text = m_params.BusSpeed.ToString();

            // Implementation
            switch (m_params.Implementation)
            {
                case CyEImplementationType.UDB:
                    rbUDB.Checked = true;
                    break;
                case CyEImplementationType.FixedFunction:
                    rbFixedFunction.Checked = true;
                    break;
                default:
                    break;
            }

            // AddressDecode
            switch (m_params.AddressDecode)
            {
                case CyEAddressDecodeType.Software:
                    rbSoftware.Checked = true;
                    break;
                case CyEAddressDecodeType.Hardware:
                    rbHardware.Checked = true;
                    break;
                default:
                    break;
            }

            // SlaveAddress
            if (m_params.m_hex)
                tbSlaveAddress.Text = IntToHex(m_params.SlaveAddress);
            else
                tbSlaveAddress.Text = m_params.SlaveAddress.ToString();

            // I2CBusPort
            m_cbPort.SelectedItem = m_params.I2CBusPort.ToString();

            // EnableWakeup
            chbWakeup.Checked = m_params.WakeupMode;
        }

        // Updates Input Frequency Based Calculations
        public void UpdateIFBC(CyClockData clock)
        {
            if (clock.IsFrequencyKnown)
            {
                lblSourceFreq.Text = "Source Freq = " + Math.Round(clock.Frequency, 3).ToString() + 
                    " " + clock.Unit.ToString();
                double freqInKHz = clock.Frequency;
                switch (clock.Unit)
                {
                    case CyClockUnit.Hz:
                        freqInKHz /= 1000;
                        break;
                    case CyClockUnit.kHz:
                        break;
                    case CyClockUnit.MHz:
                        freqInKHz *= 1000;
                        break;
                    case CyClockUnit.GHz:
                        freqInKHz *= 10000;
                        break;
                    case CyClockUnit.THz:
                        freqInKHz *= 100000;
                        break;
                    default:
                        break;
                }
                double busSpeed = freqInKHz / 16;
                lblCalcBusSpeed.Text = "Data Rate  = " + Math.Round(busSpeed, 3).ToString() + " kbps";
            }
            else
            {
                lblSourceFreq.Text = "Source Freq = UNKNOWN";
                lblCalcBusSpeed.Text = "Data Rate  = UNKNOWN";
            }
        }
        #endregion

        #region Assigning controls values to parameters
        public void SetI2CMode()
        {
            switch (cbMode.SelectedItem.ToString())
            {
                case CyI2CModesDisplayName.SLAVE:
                    m_params.Mode = CyEModeType.Slave_revA;
                    break;
                case CyI2CModesDisplayName.MASTER:
                    m_params.Mode = CyEModeType.Master_revA;
                    break;
                case CyI2CModesDisplayName.MULTI_MASTER:
                    m_params.Mode = CyEModeType.MultiMaster_revA;
                    break;
                case CyI2CModesDisplayName.MULTI_MASTER_SLAVE:
                    m_params.Mode = CyEModeType.MultiMaster_Slave_revA;
                    break;
                default:
                    break;
            }

            m_params.SetParams(CyParamNames.I2C_MODE);
        }

        private void SetBusSpeed()
        {
            m_params.BusSpeed = int.Parse(cbBusSpeed.Text);
            m_params.SetParams(CyParamNames.BUS_SPEED_KHZ);
        }

        private void SetImplementation()
        {
            m_params.Implementation = rbUDB.Checked
                ? CyEImplementationType.UDB : CyEImplementationType.FixedFunction;
            m_params.SetParams(CyParamNames.IMPLEMENTATION);
        }

        private void SetAddressDecode()
        {
            m_params.AddressDecode = rbHardware.Checked
                ? CyEAddressDecodeType.Hardware : CyEAddressDecodeType.Software;
            m_params.SetParams(CyParamNames.ADDRESS_DECODE);
        }

        private void SetSlaveAddress()
        {
            bool isHex = tbSlaveAddress.Text.StartsWith("0x");

            if (isHex)
                m_params.SlaveAddress = HexToInt(tbSlaveAddress.Text);
            else
                m_params.SlaveAddress = int.Parse(tbSlaveAddress.Text);
            m_params.SetParams(CyParamNames.SLAVE_ADDRESS);
            if (isHex)
                m_params.m_hex = true;
            else
                m_params.m_hex = false;
            m_params.SetParams(CyParamNames.HEX);
        }

        public void SetBusPort()
        {
            m_params.I2CBusPort = (CyEI2CBusPort)m_cbPort.SelectedIndex;
            m_params.SetParams(CyParamNames.I2C_BUS_PORT);
        }

        private void SetWakeupMode()
        {
            m_params.WakeupMode = chbWakeup.Checked;
            m_params.SetParams(CyParamNames.ENABLE_WAKEUP);
        }
        #endregion

        #region Event Handlers
        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMode.SelectedItem.ToString() == CyI2CModesDisplayName.SLAVE ||
                cbMode.SelectedItem.ToString() == CyI2CModesDisplayName.MULTI_MASTER_SLAVE)
            {
                panel2.Enabled = true;
                lblSlaveAddress.Enabled = true;
                tbSlaveAddress.Enabled = true;
                lblHexademical.Enabled = true;
            }
            else
            {
                panel2.Enabled = false;
                lblSlaveAddress.Enabled = false;
                tbSlaveAddress.Enabled = false;
                lblHexademical.Enabled = false;
            }
            SetI2CMode();
            SetWakeupEnabling();
            SetPinSelection();
        }

        private void cbBusSpeed_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            ComboBoxValidating(sender, ce);
            if (ce.Cancel == false)
            {
                SetBusSpeed();
                UpdateIFBC(m_params.m_clock);
            }
        }

        private void rbFixedFunction_CheckedChanged(object sender, EventArgs e)
        {
            gbIFBC.Enabled = rbUDB.Checked;
            SetBusSpeedEnabling();
            SetImplementation();
            SetWakeupEnabling();
            FilterMode();
        }

        private void rbHardware_CheckedChanged(object sender, EventArgs e)
        {
            SetAddressDecode();
            SetWakeupEnabling();
        }

        private void tbSlaveAddress_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ce = new CancelEventArgs();
            TextBoxValidating(sender, ce);
            if (ce.Cancel == false)
                SetSlaveAddress();
        }

        private void m_cbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetBusPort();
            SetWakeupEnabling();
        }

        private void checkBoxWakeup_CheckedChanged(object sender, EventArgs e)
        {
            SetWakeupMode();
        }

        private void TextBoxValidating(object sender, CancelEventArgs e)
        {
            int value = 0;
            string message = "Slave address value must be between 0 and 127";
            bool isHex = tbSlaveAddress.Text.StartsWith("0x");

            if (int.TryParse(tbSlaveAddress.Text, out value) || isHex)
            {
                if (isHex) value = HexToInt(tbSlaveAddress.Text);
                if (value < CyParamRange.SLAVE_ADDRESS_MIN || value > CyParamRange.SLAVE_ADDRESS_MAX)
                {
                    errorProvider.SetError((TextBox)sender, string.Format(message));
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError((TextBox)sender, "");
                }
            }
            else
            {
                errorProvider.SetError((TextBox)sender, string.Format(message));
                e.Cancel = true;
            }
        }

        private void ComboBoxValidating(object sender, CancelEventArgs e)
        {
            if (sender == cbBusSpeed)
            {
                int value = 0;
                string message = "An I2C Data Rate must be between 50 and 1000 kbps";
                if (int.TryParse(cbBusSpeed.Text, out value))
                {
                    if (value < CyParamRange.BUS_SPEED_MIN || value > CyParamRange.BUS_SPEED_MAX)
                    {
                        errorProvider.SetError((ComboBox)sender, string.Format(message));
                        e.Cancel = true;
                    }
                    else
                    {
                        errorProvider.SetError((ComboBox)sender, "");
                    }
                }
                else
                {
                    errorProvider.SetError((ComboBox)sender, string.Format(message));
                    e.Cancel = true;
                }
            }
        }

        private void EditBox_KeyDown(object sender, KeyEventArgs e)
        {
            ValidateInput(e, sender);
        }

        private void EditBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (m_nonNumberEntered) e.Handled = true;
        }

        private void CyI2CBasic_Load(object sender, EventArgs e)
        {
            UpdateIFBC(m_params.m_clock);
            SetBusSpeedEnabling();
            SetWakeupEnabling();
            FilterMode();
        }
        #endregion

        #region Validators
        private void ValidateInput(KeyEventArgs e, object sender)
        {
            m_nonNumberEntered = false;
            TextBox currentTextBox = new TextBox();
            ComboBox currentComboBox = new ComboBox();

            if (sender.GetType().Equals(typeof(TextBox)))
                currentTextBox = (TextBox)sender;
            if (sender.GetType().Equals(typeof(ComboBox)))
                currentComboBox = (ComboBox)sender;

            if (e.KeyCode != Keys.X && e.KeyCode != Keys.A && e.KeyCode != Keys.B &&
                e.KeyCode != Keys.C && e.KeyCode != Keys.D && e.KeyCode != Keys.E &&
                e.KeyCode != Keys.F)
            {
                ValidateKey(e);
            }
            else
            {
                if (sender.GetType().Equals(typeof(TextBox)))
                {
                    if (e.KeyCode == Keys.X && currentTextBox.Text.Contains("x"))
                        m_nonNumberEntered = true;
                    if (e.KeyCode == Keys.X && !currentTextBox.Text.StartsWith("0"))
                        m_nonNumberEntered = true;
                    if ((e.KeyCode == Keys.A || e.KeyCode == Keys.B || e.KeyCode == Keys.C ||
                        e.KeyCode == Keys.D || e.KeyCode == Keys.E || e.KeyCode == Keys.F) &&
                            (!currentTextBox.Text.StartsWith("0x")))
                    {
                        m_nonNumberEntered = true;
                    }
                    if (Control.ModifierKeys == Keys.Shift && e.KeyCode == Keys.X)
                        m_nonNumberEntered = true;
                }
                else
                {
                    ValidateKey(e);
                }
            }
        }

        // Validates whether a number key or other has been pressed.
        private void ValidateKey(KeyEventArgs e)
        {
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

        #region Hex
        public static int HexToInt(object obj)
        {
            int res = 0;
            if (obj.GetType() != typeof(string))
                return res;
            string str = (string)obj;
            if (String.IsNullOrEmpty((string)str) == false)
            {
                if (str.ToLower().StartsWith("0x"))
                    str = str.Substring(2, str.Length - 2);
                int.TryParse(str, System.Globalization.NumberStyles.AllowHexSpecifier, null, out res);
            }

            return res;
        }

        public static string IntToHex(int value)
        {
            return "0x" + value.ToString("X");
        }
        #endregion

        #region Enabling/Disabling controls
        private void SetWakeupEnabling()
        {
            if (m_params.Implementation == CyEImplementationType.FixedFunction
                && m_params.AddressDecode == CyEAddressDecodeType.Hardware
                && m_params.I2CBusPort != CyEI2CBusPort.Any
                && (m_params.Mode == CyEModeType.Slave_revA || m_params.Mode == CyEModeType.MultiMaster_Slave_revA))
            {
                chbWakeup.Enabled = true;
            }
            else
            {
                chbWakeup.Enabled = false;
                chbWakeup.Checked = false;
            }
        }

        private void SetPinSelection()
        {
            bool enabling = (m_params.Mode == CyEModeType.Slave_revA || m_params.Mode 
                == CyEModeType.MultiMaster_Slave_revA);
            m_lblPort.Enabled = enabling;
            m_cbPort.Enabled = enabling;
        }

        private void SetBusSpeedEnabling()
        {
            bool enabling = rbFixedFunction.Checked;
            lblBusSpeed.Enabled = enabling;
            cbBusSpeed.Enabled = enabling;
        }

        private void FilterMode()
        {
            // Remove Multi modes in UDB implementation and add Multi modes in FF
            // implementation. Reset to Slave when switch to UDB and Multi mode selected.
            if (rbUDB.Checked)
            {
                string currentSelectedItem = cbMode.SelectedItem.ToString();
                cbMode.Items.Remove(CyI2CModesDisplayName.MULTI_MASTER);
                cbMode.Items.Remove(CyI2CModesDisplayName.MULTI_MASTER_SLAVE);
                if (currentSelectedItem == CyI2CModesDisplayName.MULTI_MASTER ||
                    currentSelectedItem == CyI2CModesDisplayName.MULTI_MASTER_SLAVE)
                {
                    cbMode.SelectedItem = CyI2CModesDisplayName.SLAVE;
                }
                else
                {
                    cbMode.SelectedItem = currentSelectedItem;
                }
            }
            else
            {
                if (cbMode.Items.Contains(CyI2CModesDisplayName.MULTI_MASTER) == false)
                {
                    cbMode.Items.Add(CyI2CModesDisplayName.MULTI_MASTER);
                    cbMode.Items.Add(CyI2CModesDisplayName.MULTI_MASTER_SLAVE);
                }
            }
        }
        #endregion
    }

    public class CyI2CModesDisplayName
    {
        public const string SLAVE = "Slave";
        public const string MASTER = "Master";
        public const string MULTI_MASTER = "Multi-Master";
        public const string MULTI_MASTER_SLAVE = "Multi-Master-Slave";
    }
}
