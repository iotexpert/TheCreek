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
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using Cypress.Comps.PinsAndPorts.Common_v1_60;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_60
{
    public partial class CyGeneralControl : UserControl
    {
        CyPerPinDataControl m_perPinDataControl;
        bool m_textChangedSinceLastValidation = false;
        bool m_isM0S8;

        public CyPerPinDataControl PerPinDataControl
        {
            get { return m_perPinDataControl; }
            set { m_perPinDataControl = value; }
        }

        public CyGeneralControl(bool isM0S8)
        {
            m_isM0S8 = isM0S8;
            InitializeComponent();

            m_driveModePictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            m_driveModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.StrongDrive));
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.OpenDrainDriveHigh));
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.OpenDrianDriveLow));
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.ResPullUp));
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.ResPullDown));
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.ResPullUpDown));
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.HighImpedanceDigital));
            m_driveModeComboBox.Items.Add(new CyDriveMode(CyDriveMode.CyMode.HighImpedanceAnalog));

            UpdateDriveModeImage();

            m_initStateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_initStateComboBox.Items.Add(new CyInitState(CyInitState.CyState.Low));
            m_initStateComboBox.Items.Add(new CyInitState(CyInitState.CyState.High));

            m_errorProvider.SetIconAlignment(m_initStateComboBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconAlignment(m_supplyVoltageTextBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconPadding(m_driveModeErrorLocLabel, 3);
            m_errorProvider.SetIconAlignment(m_driveModeErrorLocLabel, ErrorIconAlignment.MiddleRight);

            m_initStateComboBox.SelectedIndexChanged += new EventHandler(m_initStateComboBox_SelectedIndexChanged);
            m_supplyVoltageTextBox.Validated += new EventHandler(m_supplyVoltageTextBox_Validated);
            m_supplyVoltageTextBox.TextChanged += new EventHandler(m_supplyVoltageTextBox_TextChanged);
            m_driveModeComboBox.SelectedIndexChanged += new EventHandler(m_driveModeComboBox_SelectedIndexChanged);
        }

        void m_driveModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                CyParamInfo.Formal_ParamName_DriveMode, DriveMode));

            //Drive Mode determines input buffer for M0S8
            if (m_isM0S8)
            {
                if (DriveMode == CyPortConstants.DriveModeValue_ANALOG_HI_Z)
                {
                    PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                        CyParamInfo.Formal_ParamName_InputBuffersEnabled, 
                        CyPortConstants.InputBufferEnabledValue_FALSE));
                }
                else
                {
                    PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                        CyParamInfo.Formal_ParamName_InputBuffersEnabled, 
                        CyPortConstants.InputBufferEnabledValue_TRUE));
                }
            }
            UpdateDriveModeImage();
        }

        void m_supplyVoltageTextBox_Validated(object sender, EventArgs e)
        {
            if (m_textChangedSinceLastValidation)
            {
                PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                    CyParamInfo.Formal_ParamName_IOVoltages, SupplyVoltage));
            }
            m_textChangedSinceLastValidation = false;
        }

        void m_supplyVoltageTextBox_TextChanged(object sender, EventArgs e)
        {
            m_textChangedSinceLastValidation = true;
        }

        void m_initStateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                CyParamInfo.Formal_ParamName_InitialDriveStates, InitialDriveState));
        }

        private void UpdateDriveModeImage()
        {
            CyDriveMode driveMode = m_driveModeComboBox.SelectedItem as CyDriveMode;
            if (driveMode == null)
            {
                m_driveModePictureBox.Image = null;
            }
            else
            {
                switch (driveMode.DriveMode)
                {
                    case CyDriveMode.CyMode.StrongDrive:
                        using (MemoryStream ms = new MemoryStream(Resource1.StrongDriveEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    case CyDriveMode.CyMode.OpenDrianDriveLow:
                        using (MemoryStream ms = new MemoryStream(Resource1.OpenDrainDrivesLowEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    case CyDriveMode.CyMode.OpenDrainDriveHigh:
                        using (MemoryStream ms = new MemoryStream(Resource1.OpenDrainDrivesHighEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    case CyDriveMode.CyMode.ResPullUp:
                        using (MemoryStream ms = new MemoryStream(Resource1.ResPullUpEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    case CyDriveMode.CyMode.ResPullDown:
                        using (MemoryStream ms = new MemoryStream(Resource1.ResPullDownEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    case CyDriveMode.CyMode.ResPullUpDown:
                        using (MemoryStream ms = new MemoryStream(Resource1.ResPullUpDownEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    case CyDriveMode.CyMode.HighImpedanceAnalog:
                        using (MemoryStream ms = new MemoryStream(Resource1.HiImpedanceAnalogEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    case CyDriveMode.CyMode.HighImpedanceDigital:
                        using (MemoryStream ms = new MemoryStream(Resource1.HiImpedanceDigitalEMF))
                        {
                            m_driveModePictureBox.Image = new Metafile(ms);
                        }
                        break;

                    default:
                        Debug.Fail("unhndled");
                        m_driveModePictureBox.Image = null;
                        break;
                }
            }
        }

        class CyInitState
        {
            public enum CyState { High, Low };

            CyState m_state;

            public CyState State { get { return m_state; } }

            public CyInitState(CyState state)
            {
                m_state = state;
            }

            public string ParamValue
            {
                get
                {
                    switch (State)
                    {
                        case CyState.High:
                            return CyPortConstants.InitialDriveStateValue_HIGH;

                        case CyState.Low:
                            return CyPortConstants.InitialDriveStateValue_LOW;

                        default:
                            Debug.Fail("unhandled");
                            return string.Empty;
                    }
                }
            }

            public override string ToString()
            {
                switch (State)
                {
                    case CyState.High:
                        return "High (1)";

                    case CyState.Low:
                        return "Low (0)";

                    default:
                        Debug.Fail("unhandled");
                        return base.ToString();
                }
            }
        }

        public string DriveMode
        {
            get
            {
                CyDriveMode driveMode = m_driveModeComboBox.SelectedItem as CyDriveMode;
                if (driveMode == null)
                {
                    return null;
                }
                return driveMode.ParamValue;
            }

            set
            {
                m_driveModeComboBox.SelectedIndexChanged -= m_driveModeComboBox_SelectedIndexChanged;
                if (value == null)
                {
                    m_driveModeComboBox.SelectedItem = null;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.DriveModeValue_ANALOG_HI_Z:
                            Select(CyDriveMode.CyMode.HighImpedanceAnalog);
                            break;

                        case CyPortConstants.DriveModeValue_CMOS_OUT:
                            Select(CyDriveMode.CyMode.StrongDrive);
                            break;

                        case CyPortConstants.DriveModeValue_DIGITAL_HI_Z:
                            Select(CyDriveMode.CyMode.HighImpedanceDigital);
                            break;

                        case CyPortConstants.DriveModeValue_OPEN_DRAIN_HI:
                            Select(CyDriveMode.CyMode.OpenDrainDriveHigh);
                            break;

                        case CyPortConstants.DriveModeValue_OPEN_DRAIN_LO:
                            Select(CyDriveMode.CyMode.OpenDrianDriveLow);
                            break;

                        case CyPortConstants.DriveModeValue_RES_PULL_DOWN:
                            Select(CyDriveMode.CyMode.ResPullDown);
                            break;

                        case CyPortConstants.DriveModeValue_RES_PULL_UP:
                            Select(CyDriveMode.CyMode.ResPullUp);
                            break;

                        case CyPortConstants.DriveModeValue_RES_PULL_UP_DOWN:
                            Select(CyDriveMode.CyMode.ResPullUpDown);
                            break;

                        default:
                            m_driveModeComboBox.SelectedItem = null;
                            break;
                    }
                }

                UpdateDriveModeImage();
                m_driveModeComboBox.SelectedIndexChanged += new EventHandler(m_driveModeComboBox_SelectedIndexChanged);
            }
        }

        public string InitialDriveState
        {
            get
            {
                CyInitState state = m_initStateComboBox.SelectedItem as CyInitState;
                if (state == null)
                {
                    //Indeterminate
                    return null;
                }
                else
                {
                    return state.ParamValue;
                }
            }

            set
            {
                m_initStateComboBox.SelectedIndexChanged -= m_initStateComboBox_SelectedIndexChanged;
                if (value == null)
                {
                    //Indeterminate
                    m_initStateComboBox.SelectedItem = null;
                }
                else
                {
                    m_initStateComboBox.SelectedItem = null;
                    foreach(CyInitState state in m_initStateComboBox.Items)
                    {
                        if(state.ParamValue == value)
                        {
                            m_initStateComboBox.SelectedItem = state;
                            break;
                        }
                    }
                }
                m_initStateComboBox.SelectedIndexChanged += new EventHandler(m_initStateComboBox_SelectedIndexChanged);
            }
        }

        public string SupplyVoltage
        {
            get
            {
                if (m_supplyVoltageTextBox.Text.Equals(Resource1.Indeterminate))
                {
                    return null;
                }
                if (string.IsNullOrEmpty(m_supplyVoltageTextBox.Text))
                {
                    return CyPortConstants.DefaultIOVolatageValue;
                }
                return m_supplyVoltageTextBox.Text;
            }
            set
            {
                m_supplyVoltageTextBox.Validated -= m_supplyVoltageTextBox_Validated;
                m_supplyVoltageTextBox.TextChanged -= m_supplyVoltageTextBox_TextChanged;
                if (value == null)
                {
                    //Indeterminate
                    m_supplyVoltageTextBox.Text = Resource1.Indeterminate;
                }
                else if (CyPortConstants.DefaultIOVolatageValue.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    m_supplyVoltageTextBox.Text = string.Empty;
                }
                else
                {
                    m_supplyVoltageTextBox.Text = value;
                }
                m_supplyVoltageTextBox.Validated += new EventHandler(m_supplyVoltageTextBox_Validated);
                m_supplyVoltageTextBox.TextChanged += new EventHandler(m_supplyVoltageTextBox_TextChanged);
            }
        }

        public string DriveModeErrorText
        {
            get
            {
                return m_errorProvider.GetError(m_driveModeErrorLocLabel);
            }
            set
            {
                m_errorProvider.SetError(m_driveModeErrorLocLabel, value);
            }
        }

        public string InitialDriveStateErrorText
        {
            get
            {
                return m_errorProvider.GetError(m_initStateComboBox);
            }
            set
            {
                m_errorProvider.SetError(m_initStateComboBox, value);
            }
        }

        public string SupplyVoltageErrorText
        {
            get
            {
                return m_errorProvider.GetError(m_supplyVoltageTextBox);
            }
            set
            {
                m_errorProvider.SetError(m_supplyVoltageTextBox, value);
            }
        }

        void Select(CyDriveMode.CyMode mode)
        {
            foreach (CyDriveMode driveMode in m_driveModeComboBox.Items)
            {
                if (driveMode.DriveMode == mode)
                {
                    m_driveModeComboBox.SelectedItem = driveMode;
                    break;
                }
            }
        }

        class CyDriveMode
        {
            public enum CyMode 
            { 
                StrongDrive, OpenDrianDriveLow, OpenDrainDriveHigh, ResPullUp, ResPullDown,
                ResPullUpDown, HighImpedanceAnalog, HighImpedanceDigital,
            };

            CyMode m_mode;

            public CyMode DriveMode { get { return m_mode; } }

            public CyDriveMode(CyMode mode)
            {
                m_mode = mode;
            }

            public string ParamValue
            {
                get
                {
                    switch (DriveMode)
                    {
                        case CyMode.StrongDrive:
                            return CyPortConstants.DriveModeValue_CMOS_OUT;

                        case CyMode.OpenDrianDriveLow:
                            return CyPortConstants.DriveModeValue_OPEN_DRAIN_LO;

                        case CyMode.OpenDrainDriveHigh:
                            return CyPortConstants.DriveModeValue_OPEN_DRAIN_HI;

                        case CyMode.ResPullUp:
                            return CyPortConstants.DriveModeValue_RES_PULL_UP;

                        case CyMode.ResPullDown:
                            return CyPortConstants.DriveModeValue_RES_PULL_DOWN;

                        case CyMode.ResPullUpDown:
                            return CyPortConstants.DriveModeValue_RES_PULL_UP_DOWN;

                        case CyMode.HighImpedanceAnalog:
                            return CyPortConstants.DriveModeValue_ANALOG_HI_Z;

                        case CyMode.HighImpedanceDigital:
                            return CyPortConstants.DriveModeValue_DIGITAL_HI_Z;

                        default:
                            Debug.Fail("unhandled");
                            return string.Empty;
                    }
                }
            }

            public override string ToString()
            {
                switch (DriveMode)
                {
                    case CyMode.StrongDrive:
                        return "Strong Drive";

                    case CyMode.OpenDrianDriveLow:
                        return "Open Drain, Drives Low";

                    case CyMode.OpenDrainDriveHigh:
                        return "Open Drain, Drives High";

                    case CyMode.ResPullUp:
                        return "Resistive Pull Up";

                    case CyMode.ResPullDown:
                        return "Resistive Pull Down";

                    case CyMode.ResPullUpDown:
                        return "Resistive Pull Up/Down";

                    case CyMode.HighImpedanceAnalog:
                        return "High Impedance Analog";

                    case CyMode.HighImpedanceDigital:
                        return "High Impedance Digital";

                    default:
                        Debug.Fail("unhandled");
                        return base.ToString();
                }
            }
        }
    }
}
