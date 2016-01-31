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
using System.Diagnostics;

using Cypress.Comps.PinsAndPorts.Common;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    public partial class CyOutputControl : UserControl
    {
        CyPerPinDataControl m_perPinDataControl;

        public CyPerPinDataControl PerPinDataControl
        {
            get { return m_perPinDataControl; }
            set { m_perPinDataControl = value; }
        }

        public CyOutputControl()
        {
            InitializeComponent();

            m_slewRateComboBox.Items.Add(new CySlewRate(CySlewRate.CyRate.Slow));
            m_slewRateComboBox.Items.Add(new CySlewRate(CySlewRate.CyRate.Fast));

            m_driveLevelComboBox.Items.Add(new CyDriveLevel(CyDriveLevel.CyLevel.Vddio));
            m_driveLevelComboBox.Items.Add(new CyDriveLevel(CyDriveLevel.CyLevel.Vref));

            m_currentComboBox.Items.Add(new CyDriveCurrent(CyDriveCurrent.CyCurrent.Source4Sink8));
            m_currentComboBox.Items.Add(new CyDriveCurrent(CyDriveCurrent.CyCurrent.Source4Sink25));

            m_outputSyncCheckBox.CheckedChanged += new EventHandler(m_outputSyncCheckBox_CheckedChanged);
            m_slewRateComboBox.SelectedIndexChanged += new EventHandler(m_slewRateComboBox_SelectedIndexChanged);
            m_driveLevelComboBox.SelectedIndexChanged += new EventHandler(m_driveLevelComboBox_SelectedIndexChanged);
            m_currentComboBox.SelectedIndexChanged += new EventHandler(m_currentComboBox_SelectedIndexChanged);

            m_errorProvider.SetIconAlignment(m_outputSyncCheckBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconPadding(m_outputSyncCheckBox, 3);
            m_errorProvider.SetIconAlignment(m_slewRateComboBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconPadding(m_slewRateComboBox, 3);
            m_errorProvider.SetIconAlignment(m_driveLevelComboBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconPadding(m_driveLevelComboBox, 3);
            m_errorProvider.SetIconAlignment(m_currentComboBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconPadding(m_currentComboBox, 3);
        }

        void m_currentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                    CyParamInfo.Formal_ParamName_DriveCurrents, OutputDriveCurrent));
        }

        void m_driveLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                CyParamInfo.Formal_ParamName_OutputDriveLevels, OutputDriveLevel));
        }

        void m_outputSyncCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                CyParamInfo.Formal_ParamName_OutputsSynchronized, OutputSynchronized));
        }

        void m_slewRateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerPinDataControl.OnParamDataChangeByUser(new CyPerPinDataEventArgs(
                CyParamInfo.Formal_ParamName_SlewRate, SlewRate));
        }

        public string OutputSynchronized
        {
            get
            {
                if (m_outputSyncCheckBox.CheckState == CheckState.Indeterminate)
                {
                    return null;
                }
                if (m_outputSyncCheckBox.Checked)
                {
                    return CyPortConstants.OutputsSynchronizedValue_ENABLED;
                }
                return CyPortConstants.OutputsSynchronizedValue_DISABLED;
            }

            set
            {
                m_outputSyncCheckBox.CheckedChanged -= m_outputSyncCheckBox_CheckedChanged;
                if (value == null)
                {
                    m_outputSyncCheckBox.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.OutputsSynchronizedValue_DISABLED:
                            m_outputSyncCheckBox.CheckState = CheckState.Unchecked;
                            m_outputSyncCheckBox.Checked = false;
                            break;

                        case CyPortConstants.OutputsSynchronizedValue_ENABLED:
                            m_outputSyncCheckBox.CheckState = CheckState.Checked;
                            m_outputSyncCheckBox.Checked = true;
                            break;

                        default:
                            m_outputSyncCheckBox.CheckState = CheckState.Indeterminate;
                            break;
                    }
                }
                m_outputSyncCheckBox.CheckedChanged += new EventHandler(m_outputSyncCheckBox_CheckedChanged);
            }
        }

        public string OutputSynchronizedErrorText
        {
            get { return m_errorProvider.GetError(m_outputSyncCheckBox); }
            set { m_errorProvider.SetError(m_outputSyncCheckBox, value); }
        }

        public string SlewRate
        {
            get
            {
                CySlewRate slewRate = m_slewRateComboBox.SelectedItem as CySlewRate;
                if (slewRate == null)
                {
                    return null;
                }
                return slewRate.ParamValue;
            }

            set
            {
                m_slewRateComboBox.SelectedIndexChanged -= m_slewRateComboBox_SelectedIndexChanged;
                if (value == null)
                {
                    m_slewRateComboBox.SelectedItem = null;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.SlewRateValue_FAST:
                            Select(CySlewRate.CyRate.Fast);
                            break;

                        case CyPortConstants.SlewRateValue_SLOW:
                            Select(CySlewRate.CyRate.Slow);
                            break;

                        default:
                            m_slewRateComboBox.SelectedItem = null;
                            break;
                    }
                }
                m_slewRateComboBox.SelectedIndexChanged += new EventHandler(m_slewRateComboBox_SelectedIndexChanged);
            }
        }

        public string SlewRateErrorText
        {
            get { return m_errorProvider.GetError(m_slewRateComboBox); }
            set { m_errorProvider.SetError(m_slewRateComboBox, value); }
        }

        public string OutputDriveLevel
        {
            get
            {
                CyDriveLevel driveLevel = m_driveLevelComboBox.SelectedItem as CyDriveLevel;
                if (driveLevel == null)
                {
                    return null;
                }
                return driveLevel.ParamValue;
            }

            set
            {
                m_driveLevelComboBox.SelectedIndexChanged -= m_driveLevelComboBox_SelectedIndexChanged;
                if (value == null)
                {
                    m_driveLevelComboBox.SelectedItem = null;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.OutputDriveLevelValue_VDDIO:
                            Select(CyDriveLevel.CyLevel.Vddio);
                            break;

                        case CyPortConstants.OutputDriveLevelValue_VREF:
                            Select(CyDriveLevel.CyLevel.Vref);
                            break;

                        default:
                            m_driveLevelComboBox.SelectedItem = null;
                            break;
                    }
                }
                m_driveLevelComboBox.SelectedIndexChanged += 
                    new EventHandler(m_driveLevelComboBox_SelectedIndexChanged);
            }
        }

        public string OutputDriveLevelErrorText
        {
            get { return m_errorProvider.GetError(m_driveLevelComboBox); }
            set { m_errorProvider.SetError(m_driveLevelComboBox, value); }
        }

        public string OutputDriveCurrent
        {
            get
            {
                CyDriveCurrent driveCurrent = m_currentComboBox.SelectedItem as CyDriveCurrent;
                if (driveCurrent == null)
                {
                    return null;
                }
                return driveCurrent.ParamValue;
            }

            set
            {
                m_currentComboBox.SelectedIndexChanged -= m_currentComboBox_SelectedIndexChanged;
                if (value == null)
                {
                    m_currentComboBox.SelectedItem = null;
                }
                else
                {
                    switch (value)
                    {
                        case CyPortConstants.DriveCurrentValue_4SOURCE_25SINK:
                            Select(CyDriveCurrent.CyCurrent.Source4Sink25);
                            break;

                        case CyPortConstants.DriveCurrentValue_4SOURCE_8SINK:
                            Select(CyDriveCurrent.CyCurrent.Source4Sink8);
                            break;

                        default:
                            m_currentComboBox.SelectedItem = null;
                            break;
                    }
                }
                m_currentComboBox.SelectedIndexChanged += new EventHandler(m_currentComboBox_SelectedIndexChanged);
            }
        }

        public string OutputDriveCurrentErrorText
        {
            get { return m_errorProvider.GetError(m_currentComboBox); }
            set { m_errorProvider.SetError(m_currentComboBox, value); }
        }

        void Select(CySlewRate.CyRate rate)
        {
            foreach (CySlewRate slewRate in m_slewRateComboBox.Items)
            {
                if (slewRate.SlewRate == rate)
                {
                    m_slewRateComboBox.SelectedItem = slewRate;
                    break;
                }
            }
        }

        void Select(CyDriveLevel.CyLevel level)
        {
            foreach (CyDriveLevel driveLevel in m_driveLevelComboBox.Items)
            {
                if (driveLevel.DriveLevel == level)
                {
                    m_driveLevelComboBox.SelectedItem = driveLevel;
                    break;
                }
            }
        }

        void Select(CyDriveCurrent.CyCurrent current)
        {
            foreach (CyDriveCurrent driveCurrent in m_currentComboBox.Items)
            {
                if (driveCurrent.DriveCurrent == current)
                {
                    m_currentComboBox.SelectedItem = driveCurrent;
                    break;
                }
            }
        }

        class CySlewRate
        {
            public enum CyRate { Fast, Slow };

            CyRate m_rate;

            public CyRate SlewRate { get { return m_rate; } }

            public CySlewRate(CyRate rate)
            {
                m_rate = rate;
            }

            public string ParamValue
            {
                get
                {
                    switch (SlewRate)
                    {
                        case CyRate.Fast:
                            return CyPortConstants.SlewRateValue_FAST;

                        case CyRate.Slow:
                            return CyPortConstants.SlewRateValue_SLOW;

                        default:
                            Debug.Fail("unhandled");
                            return string.Empty;
                    }
                }
            }

            public override string ToString()
            {
                switch (SlewRate)
                {
                    case CyRate.Fast:
                        return "Fast";

                    case CyRate.Slow:
                        return "Slow";

                    default:
                        Debug.Fail("unhandled");
                        return base.ToString();
                }
            }
        }

        class CyDriveLevel
        {
            public enum CyLevel { Vddio, Vref };

            CyLevel m_level;

            public CyLevel DriveLevel { get { return m_level; } }

            public CyDriveLevel(CyLevel level)
            {
                m_level = level;
            }

            public string ParamValue
            {
                get
                {
                    switch (DriveLevel)
                    {
                        case CyLevel.Vddio:
                            return CyPortConstants.OutputDriveLevelValue_VDDIO;

                        case CyLevel.Vref:
                            return CyPortConstants.OutputDriveLevelValue_VREF;

                        default:
                            Debug.Fail("unhandled");
                            return string.Empty;
                    }
                }
            }

            public override string ToString()
            {
                switch (DriveLevel)
                {
                    case CyLevel.Vddio:
                        return "Vddio";

                    case CyLevel.Vref:
                        return "Vref";

                    default:
                        Debug.Fail("unhandled");
                        return base.ToString();
                }
            }
        }

        class CyDriveCurrent
        {
            public enum CyCurrent { Source4Sink8, Source4Sink25 };

            CyCurrent m_current;

            public CyCurrent DriveCurrent { get { return m_current; } }

            public CyDriveCurrent(CyCurrent current)
            {
                m_current = current;
            }

            public string ParamValue
            {
                get
                {
                    switch (DriveCurrent)
                    {
                        case CyCurrent.Source4Sink25:
                            return CyPortConstants.DriveCurrentValue_4SOURCE_25SINK;

                        case CyCurrent.Source4Sink8:
                            return CyPortConstants.DriveCurrentValue_4SOURCE_8SINK;

                        default:
                            Debug.Fail("unhandled");
                            return string.Empty;
                    }
                }
            }

            public override string ToString()
            {
                switch (DriveCurrent)
                {
                    case CyCurrent.Source4Sink25:
                        return "4mA source, 25mA sink";

                    case CyCurrent.Source4Sink8:
                        return "4mA source, 8mA sink";

                    default:
                        Debug.Fail("unhandled");
                        return base.ToString();
                }
            }
        }
    }
}
