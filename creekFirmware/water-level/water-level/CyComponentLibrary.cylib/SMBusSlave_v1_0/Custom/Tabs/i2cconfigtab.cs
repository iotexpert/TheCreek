/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace SMBusSlave_v1_0
{
    public partial class CyI2cConfigTab : CyTabWrapper
    {
        #region CyTabControlWrapper members
        public override string TabName
        {
            get { return "I2C Configuration"; }
        }
        #endregion

        #region Constructor(s)
        public CyI2cConfigTab(CyParameters parameters)
            : base(parameters)
        {
            m_params.m_i2cConfigTab = this;
            InitializeComponent();

            numMinusTolerance.TextChanged += new EventHandler(numMinusTolerance_TextChanged);
            numPlusTolerance.TextChanged += new EventHandler(numMinusTolerance_TextChanged);

            // Update dependent from implementation and clock source, controls appearence
            rbExternalClock_CheckedChanged(rbExternalClock, new EventArgs());
            rbFixedFunction_CheckedChanged(rbFixedFunction, new EventArgs());

            m_chbExternalIOBuff.CheckedChanged += new EventHandler(m_chbExternalIOBuff_CheckedChanged);
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateUI()
        {
            bool prevGlobalEditMode = m_params.m_globalEditMode;
            m_params.m_globalEditMode = false;

            // Implementation
            switch (m_params.Implementation)
            {
                case CyEImplementationType.I2C__UDB:
                    rbUDB.Checked = true;
                    break;
                case CyEImplementationType.I2C__FixedFunction:
                    rbFixedFunction.Checked = true;
                    break;
                default:
                    break;
            }

            // AddressDecode
            switch (m_params.AddressDecode)
            {
                case CyEAddressDecodeType.I2C__Software:
                    rbSoftware.Checked = true;
                    break;
                case CyEAddressDecodeType.I2C__Hardware:
                    rbHardware.Checked = true;
                    break;
                default:
                    break;
            }

            // I2CBusPort
            switch (m_params.Pins)
            {
                case CyEBusPortType.I2C__Any:
                    rbAny.Checked = true;
                    break;
                case CyEBusPortType.I2C__I2C0:
                    rbI2C0.Checked = true;
                    break;
                case CyEBusPortType.I2C__I2C1:
                    rbI2C1.Checked = true;
                    break;
                default:
                    break;
            }

            // InternalClock
            if (m_params.UdbInternalClock)
                rbInternalClock.Checked = true;
            else
                rbExternalClock.Checked = true;

            // FixedPlacement
            chbFixedPlacement.Checked = m_params.UdbSlaveFixedPlacementEnable;

            // Tolerance
            numMinusTolerance.Text = m_params.MinusTolerance.ToString();
            numPlusTolerance.Text = m_params.PlusTolerance.ToString();

            m_chbExternalIOBuff.Checked = m_params.ExternalIOBuffer;

            UpdateFixedPlacementAppearence();
            ImplementationAppearence();
            m_params.m_globalEditMode = prevGlobalEditMode;
        }
        #endregion

        #region Event handlers
        private void rbExternalClock_CheckedChanged(object sender, EventArgs e)
        {
            bool choise = ((RadioButton)sender).Checked;
            m_params.UdbInternalClock = !choise;

            panelTolerance.Enabled = !choise;
            m_params.m_generalTab.UpdateDataRateAppearence(!choise);
            m_params.UpdateTimeout();
        }

        private void rbFixedFunction_CheckedChanged(object sender, EventArgs e)
        {
            bool choise = ((RadioButton)sender).Checked;
            m_params.Implementation = choise ? CyEImplementationType.I2C__FixedFunction :
                CyEImplementationType.I2C__UDB;

            m_params.m_generalTab.UpdateCalculator();
            gbUDBClockSource.Enabled = !choise;
            gbPinConnections.Enabled = choise;
            UpdateFixedPlacementAppearence();
            ImplementationAppearence();
        }

        private void rbHardware_CheckedChanged(object sender, EventArgs e)
        {
            m_params.AddressDecode = rbHardware.Checked ? CyEAddressDecodeType.I2C__Hardware :
                CyEAddressDecodeType.I2C__Software;
            m_params.m_generalTab.UpdateSmbAlertErrorProvider();
        }

        void m_chbExternalIOBuff_CheckedChanged(object sender, EventArgs e)
        {
            m_params.ExternalIOBuffer = m_chbExternalIOBuff.Checked;
        }

        private void rbPins_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAny.Checked)
                m_params.Pins = CyEBusPortType.I2C__Any;
            else if (rbI2C0.Checked)
                m_params.Pins = CyEBusPortType.I2C__I2C0;
            else
                m_params.Pins = CyEBusPortType.I2C__I2C1;
        }

        private void chbFixedPlacement_CheckedChanged(object sender, EventArgs e)
        {
            bool state = ((CheckBox)sender).Checked;
            m_params.UdbSlaveFixedPlacementEnable = state;
            UpdateFixedPlacementAppearence();
        }

        private void numMinusTolerance_TextChanged(object sender, EventArgs e)
        {
            double minusToleranceValue = 0;
            double plusToleranceValue = 0;

            if (double.TryParse(numMinusTolerance.Text, out minusToleranceValue) &&
                double.TryParse(numPlusTolerance.Text, out plusToleranceValue))
            {
                minusToleranceValue *= -1;

                if ((minusToleranceValue < CyParamRange.TOLERANCE_MIN ||
                    minusToleranceValue > CyParamRange.TOLERANCE_MAX ||
                    plusToleranceValue < CyParamRange.TOLERANCE_MIN ||
                    plusToleranceValue > CyParamRange.TOLERANCE_MAX) &&
                    (m_params.Implementation == CyEImplementationType.I2C__UDB && m_params.UdbInternalClock == true))
                {
                    m_errorProvider.SetError(numPlusTolerance, string.Format(Resources.ToleranceOutOfRange,
                        CyParamRange.TOLERANCE_MIN, CyParamRange.TOLERANCE_MAX));
                }
                else
                    m_errorProvider.SetError(numPlusTolerance, string.Empty);
            }
            else
            {
                m_errorProvider.SetError(numPlusTolerance, string.Format(Resources.ToleranceOutOfRange,
                    CyParamRange.TOLERANCE_MIN, CyParamRange.TOLERANCE_MAX));
            }
            if (sender == numMinusTolerance)
                m_params.MinusTolerance = minusToleranceValue * (-1);
            if (sender == numPlusTolerance)
                m_params.PlusTolerance = plusToleranceValue;
        }
        #endregion

        #region Fixed placement and implementation validation and enabling
        private void UpdateFixedPlacementAppearence()
        {
            if (m_params.UdbSlaveFixedPlacementEnable &&
                (m_params.Implementation == CyEImplementationType.I2C__FixedFunction))
            {
                m_errorProvider.SetError(chbFixedPlacement, Resources.FixedPlacementValidator);
            }
            else
            {
                m_errorProvider.SetError(chbFixedPlacement, string.Empty);
            }

            if (m_params.UdbSlaveFixedPlacementEnable == false &&
                (m_params.Implementation == CyEImplementationType.I2C__FixedFunction))
            {
                chbFixedPlacement.Enabled = false;
            }
            else
            {
                chbFixedPlacement.Enabled = true;
            }
        }

        private void ImplementationAppearence()
        {
            if ((m_params.Implementation == CyEImplementationType.I2C__FixedFunction) && m_params.IsPSoC5A)
            {
                m_errorProvider.SetError(rbFixedFunction, Resources.ImplementationWithSiliconValidator);
            }
            else
            {
                m_errorProvider.SetError(rbFixedFunction, string.Empty);
            }

            if ((m_params.Implementation != CyEImplementationType.I2C__FixedFunction) && m_params.IsPSoC5A)
            {
                rbFixedFunction.Enabled = false;
            }
            else
            {
                rbFixedFunction.Enabled = true;
            }
        }
        #endregion
    }

    public class CyPercentageUpDown : NumericUpDown
    {
        #region Overriden methods
        public override string Text
        {
            get { return base.Text.TrimEnd('%'); }
            set { base.Text = value + "%"; }
        }
        #endregion
    }
}
