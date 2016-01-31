/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Windows.Forms;

namespace SegLCD_v1_50
{
    public partial class CyDriverParams : UserControl
    {
        public CyLCDParameters m_parameters;

        public CyDriverParams()
        {
            InitializeComponent();
        }

        public CyDriverParams(CyLCDParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            m_parameters.m_cyDriverParamsTab = this;
            LoadValuesFromParameters();
        }

        private void LoadValuesFromParameters()
        {
            comboBoxDriverPowerMode.SelectedIndex = m_parameters.DriverPowerMode;
            UpdateNumUpDownDriveTimeData(null, null);
            comboBoxLowDriveMode.SelectedIndex = m_parameters.LowDriveMode;
            editClockFrequency.Text = m_parameters.ClockFrequency.ToString();
        }

        private void comboBoxDriverPowerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.DriverPowerMode = (byte)comboBoxDriverPowerMode.SelectedIndex;
            if (comboBoxDriverPowerMode.SelectedIndex == 0)
            {
                comboBoxLowDriveMode.Enabled = false;
                numUpDownLowDriveInitTime.Enabled = false;
                m_parameters.LowDriveInitTime = 1;
                //Fix CDT 57916
                if (m_parameters.m_globalEditMode)
                {
                    m_parameters.HiDriveTime = 128;
                }                
            }
            else
            {
                comboBoxLowDriveMode.Enabled = true;
                numUpDownLowDriveInitTime.Enabled = true;
                //Fix CDT 57916
                if (m_parameters.m_globalEditMode)
                {
                    m_parameters.HiDriveTime = 64;
                    m_parameters.LowDriveInitTime = 64;
                }
            }
            UpdateNumUpDownDriveTimeData(null, null);
        }

        private void numUpDownHiDriveTime_ValueChanged(object sender, EventArgs e)
        {
            m_parameters.HiDriveTime = ValidateUpDownDriveTimeValue(numUpDownHiDriveTime.Value);
        }

        private void numUpDownLowDriveInitTime_ValueChanged(object sender, EventArgs e)
        {
            m_parameters.LowDriveInitTime = ValidateUpDownDriveTimeValue(numUpDownLowDriveInitTime.Value);
        }

        byte ValidateUpDownDriveTimeValue(decimal sender)
        {
            byte res = 1;
            if (m_parameters.InputClockPeriod > 0)
                res = (byte)(Math.Truncate((double)sender /
                    (double)m_parameters.InputClockPeriod));
            return res;
        }

        /// <summary>
        /// Updates data in numUpDownLowDriveInitTime and numUpDownHiDriveTime
        /// </summary>
        void UpdateNumUpDownDriveTimeData(object sender, EventArgs e)
        {
            NumericUpDown[] listNumericUpDown = new NumericUpDown[]
                                                    {
                                                        numUpDownHiDriveTime, numUpDownLowDriveInitTime
                                                    };
            byte[] listDrTime = new byte[]
                                    {
                                        m_parameters.HiDriveTime, m_parameters.LowDriveInitTime
                                    };

            for (int i = 0; i < listNumericUpDown.Length; i++)
            {
                NumericUpDown numUpDown = listNumericUpDown[i];
                numUpDown.Minimum = m_parameters.InputClockPeriod;

                if (m_parameters.DriverPowerMode == 1)
                {
                    numUpDown.Maximum = ((250 - listDrTime[(i + 1)%2] - m_parameters.m_dacDisInitTime)*
                                         m_parameters.InputClockPeriod);
                }
                else
                {
                    numUpDown.Maximum = (255 - listDrTime[(i + 1) % 2]) * m_parameters.InputClockPeriod;
                }

                numUpDown.Increment = m_parameters.InputClockPeriod;

                if (listDrTime[i] * m_parameters.InputClockPeriod <= numUpDown.Maximum)
                    numUpDown.Value = listDrTime[i] * m_parameters.InputClockPeriod;
                else
                {
                    //Update old value
                    listDrTime[0] = m_parameters.HiDriveTime;
                }
            }
        }

        private void comboBoxLowDriveMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.LowDriveMode = (byte)comboBoxLowDriveMode.SelectedIndex;
        }

        private void CyDriverParams_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                editClockFrequency.Text = m_parameters.ClockFrequency.ToString();
                UpdateNumUpDownDriveTimeData(null, null);
            }
        }
    }
}
