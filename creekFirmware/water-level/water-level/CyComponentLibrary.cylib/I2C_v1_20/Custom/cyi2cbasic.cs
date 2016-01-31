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

namespace I2C_v1_20
{
    public partial class CyI2CBasic : UserControl, ICyParamEditingControl
    {
        private Control m_control;
        private CyI2CParameters m_params;
        private bool m_nonNumberEntered = false;

        public CyI2CBasic(CyI2CParameters inst)
        {
            InitializeComponent();

            ((CyI2CParameters)inst).m_basicTab = this;
            m_control = this;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
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

        #region Event Handlers

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
            errorProvider.SetIconAlignment(cbBusSpeed, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetIconAlignment(cbMode, ErrorIconAlignment.MiddleLeft);
            errorProvider.SetIconAlignment(tbSlaveAddress, ErrorIconAlignment.MiddleLeft);
        }

        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (errorProvider.GetError(cbBusSpeed) != "")
            {
                cbBusSpeed.Focus();
                e.Cancel = true;
            }
            if (errorProvider.GetError(tbSlaveAddress) != "")
            {
                tbSlaveAddress.Focus();
                e.Cancel = true;
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_params.SetMode(sender);
            if (cbMode.Text != "Slave")
            {
                lbSlaveAddress.Enabled = false;
                tbSlaveAddress.Enabled = false;
            }
            else
            {
                lbSlaveAddress.Enabled = true;
                tbSlaveAddress.Enabled = true;
            }
        }

        private void cbBusSpeed_TextChanged(object sender, EventArgs e)
        {
            m_params.SetBusSpeed(sender);
        }

        private void rbFixedFunction_CheckedChanged(object sender, EventArgs e)
        {
            m_params.SetImplementation();
        }

        private void rbHardware_CheckedChanged(object sender, EventArgs e)
        {
            m_params.SetAddressDecode();
        }

        private void tbSlaveAddress_TextChanged(object sender, EventArgs e)
        {
            m_params.SetSlaveAddress(sender);
        }

        private void checkBoxWakeup_CheckedChanged(object sender, EventArgs e)
        {
            m_params.SetWakeupMode();
        }

        public void ControlsValidating(object sender, CancelEventArgs e)
        {
            // ComboBoxes
            if (sender == cbBusSpeed)
            {
                if (m_params.BusSpeedValidated(cbBusSpeed.Text) == false)
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
            if (sender == cbMode)
            {
                if (m_params.GetNameByDisplayName(cbMode.Text) == "Multi_Master_Slave")
                {
                    errorProvider.SetError((ComboBox)sender, string.Format(
                        "It is not supported yet"));
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError((ComboBox)sender, "");
                }
            }
            // TextBoxes
            if (sender == tbSlaveAddress)
            {
                if (m_params.SlaveAddressValidated(tbSlaveAddress.Text) == false)
                {
                    errorProvider.SetError((TextBox)sender, string.Format(
                        "Slave address value must be between 0 and 127"));
                    e.Cancel = true;
                }
                else
                {
                    errorProvider.SetError((TextBox)sender, "");
                }
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
            // Initialize the flag to false.
            m_nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        m_nonNumberEntered = true;
                    }
                }
            }
            //If shift key was pressed, it's not a number.
            if (Control.ModifierKeys == Keys.Shift)
            {
                m_nonNumberEntered = true;
            }
        }

        #endregion
    }
}
