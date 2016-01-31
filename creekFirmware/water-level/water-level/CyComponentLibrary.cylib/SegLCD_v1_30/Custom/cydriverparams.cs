// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================

using System;
using System.Windows.Forms;

namespace SegLCD_v1_30
{
    public partial class CyDriverParams : UserControl
    {
        public CyLCDParameters m_Parameters;

        public CyDriverParams()
        {
            InitializeComponent();
            m_Parameters = new CyLCDParameters();
        }

        public CyDriverParams(CyLCDParameters parameters)
        {
            InitializeComponent();

            this.m_Parameters = parameters;
            this.m_Parameters.m_CyDriverParamsTab = this;
            LoadValuesFromParameters();

        }

        private void LoadValuesFromParameters()
        {
            comboBoxDriverPowerMode.SelectedIndex = m_Parameters.DriverPowerMode;
            comboBoxLowDriveMode.SelectedIndex = m_Parameters.LowDriveMode;
            editClockFrequency.Text = m_Parameters.ClockFrequency.ToString();
        }

        #region Validation

        private void ValidateDoubleValue(object sender, double minVal, double maxVal)
        {
            double val;

            if (!Double.TryParse(((TextBox)sender).Text, out val))
            {
                errorProvider.SetError((Control)sender, String.Format(CyLCDParameters.FORMAT_ERROR_MSG, minVal,
                                                                      maxVal));
            }
            else
            {
                if ((val < minVal) || (val > maxVal))
                {
                    errorProvider.SetError((Control) sender,
                                           String.Format(CyLCDParameters.FORMAT_ERROR_MSG, minVal, maxVal));
                }
                else
                {
                    errorProvider.SetError((Control)sender, "");
                }
            }
        }

        private void ValidateIntValue(object sender, int minVal, int maxVal)
        {
            int val;

            if (!Int32.TryParse(((TextBox)sender).Text, out val))
            {
                errorProvider.SetError((Control) sender,
                                       String.Format(CyLCDParameters.FORMAT_INT_ERROR_MSG, minVal, maxVal));
            }
            else
            {
                if ((val < minVal) || (val > maxVal))
                {
                    errorProvider.SetError((Control) sender,
                                           String.Format(CyLCDParameters.FORMAT_INT_ERROR_MSG, minVal, maxVal));
                }
                else
                {
                    errorProvider.SetError((Control)sender, "");
                }
            }
        }

        #endregion

        private void comboBoxDriverPowerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Parameters.DriverPowerMode = (byte)comboBoxDriverPowerMode.SelectedIndex;
            if (comboBoxDriverPowerMode.SelectedIndex == 0)
            {
                comboBoxLowDriveMode.Enabled = false;
                numUpDownLowDriveInitTime.Enabled = false;
                m_Parameters.LowDriveInitTime = 1;
                //Fix CDT 57916
                if (m_Parameters.m_GlobalEditMode)
                {
                    m_Parameters.HiDriveTime = 128;
                }                
            }
            else
            {
                comboBoxLowDriveMode.Enabled = true;
                numUpDownLowDriveInitTime.Enabled = true;
                //Fix CDT 57916
                if (m_Parameters.m_GlobalEditMode)
                {
                    m_Parameters.HiDriveTime = 64;
                    m_Parameters.LowDriveInitTime = 64;
                }
            }
            UpdateNumUpDownDriveTimeData(null, null);
        }

        private void numUpDownHiDriveTime_ValueChanged(object sender, EventArgs e)
        {
            if (m_Parameters.m_GlobalEditMode)
                m_Parameters.HiDriveTime = ValidateUpDownDriveTimeValue(numUpDownHiDriveTime.Value);
        }

        private void numUpDownLowDriveInitTime_ValueChanged(object sender, EventArgs e)
        {
            if (m_Parameters.m_GlobalEditMode)
                m_Parameters.LowDriveInitTime = ValidateUpDownDriveTimeValue(numUpDownLowDriveInitTime.Value);
        }

        byte ValidateUpDownDriveTimeValue(decimal sender)
        {
            byte res = 1;
            if (m_Parameters.InputClockPeriod > 0)
                res = (byte)(Math.Round((double)sender /
                    (double)m_Parameters.InputClockPeriod));
            return res;
        }

        /// <summary>
        /// Updates data in numUpDownLowDriveInitTime and numUpDownHiDriveTime
        /// </summary>
        void UpdateNumUpDownDriveTimeData(object sender, EventArgs e)
        {
            m_Parameters.m_GlobalEditMode = false;
            NumericUpDown[] listNumericUpDown = new NumericUpDown[]
                                                    {
                                                        numUpDownHiDriveTime, numUpDownLowDriveInitTime
                                                    };
            byte[] listDrTime = new byte[]
                                    {
                                        m_Parameters.HiDriveTime, m_Parameters.LowDriveInitTime
                                    };

            byte[] max_time;
            m_Parameters.UpdateDriveTimeData(out max_time);
            for (int i = 0; i < listNumericUpDown.Length; i++)
            {
                NumericUpDown numUpDown = listNumericUpDown[i];
                numUpDown.Minimum = m_Parameters.InputClockPeriod;
                numUpDown.Maximum = max_time[i] * m_Parameters.InputClockPeriod;
                numUpDown.Increment = m_Parameters.InputClockPeriod;

                decimal newVal = listDrTime[i] * m_Parameters.InputClockPeriod;

                if (newVal <= numUpDown.Maximum)
                    numUpDown.Value = newVal;
                else // Out of limits 
                {
                    System.Diagnostics.Debug.Assert(false);
                }
            }

            m_Parameters.m_GlobalEditMode = true;
        }

        private void comboBoxLowDriveMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Parameters.LowDriveMode = (byte)comboBoxLowDriveMode.SelectedIndex;
        }

        private void CyDriverParams_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                editClockFrequency.Text = m_Parameters.ClockFrequency.ToString();
                UpdateNumUpDownDriveTimeData(null, null);
            }
        }
    }
}
