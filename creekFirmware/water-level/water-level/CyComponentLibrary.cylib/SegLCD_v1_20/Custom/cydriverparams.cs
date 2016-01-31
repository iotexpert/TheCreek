/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;

namespace SegLCD_v1_20
{
    public partial class CyDriverParams : UserControl
    {
        public CyLCDParameters Parameters;

        public CyDriverParams()
        {
            InitializeComponent();            
            Parameters = new CyLCDParameters();
        }

        public CyDriverParams(CyLCDParameters parameters)
        {
            InitializeComponent();

            this.Parameters = parameters;
            LoadValuesFromParameters();
        }

        private void LoadValuesFromParameters()
        {
            comboBoxDriverPowerMode.SelectedIndex = Parameters.DriverPowerMode;
            LoadDriveTime(numUpDownHiDriveTime, Parameters.HiDriveTime);
            LoadDriveTime(numUpDownLowDriveInitTime, Parameters.LowDriveInitTime);
            UpdateMaximumValues(null, null);
            comboBoxLowDriveMode.SelectedIndex = Parameters.LowDriveMode;
            editClockFrequency.Text = Parameters.ClockFrequency.ToString();
        }

        void InitDriveTime(NumericUpDown numUpDown, byte drTime)
        {
            // If were changes (based on Minimum and formula)
            if (Math.Abs((double)numUpDown.Minimum - Math.Pow(10, 6) / Parameters.ClockFrequency) > 0.001)
            {
                numUpDown.Minimum = 0;
                numUpDown.Maximum = 1000000;
                numUpDown.Minimum = (decimal)(Math.Pow(10, 6) / Parameters.ClockFrequency);
                numUpDown.Value = numUpDown.Minimum * drTime;
                int max_val = 253;
                numUpDown.Maximum = numUpDown.Minimum * max_val;
                numUpDown.Increment = (decimal) (Math.Pow(10, 6)/Parameters.ClockFrequency);
            }
        }

        void LoadDriveTime(NumericUpDown numUpDown,byte drTime)
        {
            numUpDown.Minimum = 0;
            numUpDown.Maximum = 1000000;
            numUpDown.Value = (decimal)(Math.Pow(10, 6) / Parameters.ClockFrequency) * drTime;
            numUpDown.Minimum = (decimal)(Math.Pow(10, 6) / Parameters.ClockFrequency);
            int max_val = 253;
            numUpDown.Maximum = numUpDown.Minimum * max_val;
            numUpDown.Increment = (decimal)(Math.Pow(10, 6) / Parameters.ClockFrequency);
        }

        #region Validation

        private void ValidateDoubleValue(object sender, double minVal, double maxVal)
        {
            double val;

            if (!Double.TryParse(((TextBox)sender).Text, out val))
            {
                errorProvider.SetError((Control)sender, String.Format(CyLCDParameters.FormatErrorMsg, minVal, maxVal));
            }
            else
            {
                if ((val < minVal) || (val > maxVal))
                {
                    errorProvider.SetError((Control) sender,
                                           String.Format(CyLCDParameters.FormatErrorMsg, minVal, maxVal));
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
                                       String.Format(CyLCDParameters.FormatIntErrorMsg, minVal, maxVal));
            }
            else
            {
                if ((val < minVal) || (val > maxVal))
                {
                    errorProvider.SetError((Control) sender,
                                           String.Format(CyLCDParameters.FormatIntErrorMsg, minVal, maxVal));
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
            Parameters.DriverPowerMode = (byte)comboBoxDriverPowerMode.SelectedIndex;
            if (comboBoxDriverPowerMode.SelectedIndex == 0)
            {
                comboBoxLowDriveMode.Enabled = false;
                numUpDownLowDriveInitTime.Enabled = false;
                numUpDownLowDriveInitTime.Value = numUpDownLowDriveInitTime.Minimum;
                Parameters.LowDriveInitTime = 1;                
            }
            else
            {
                comboBoxLowDriveMode.Enabled = true;
                numUpDownLowDriveInitTime.Enabled = true;
            }
            UpdateMaximumValues(null, null);
        }

        private void numUpDownHiDriveTime_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDownHiDriveTime.Minimum > 0)
                Parameters.HiDriveTime =
                    (byte)(Math.Ceiling((double)numUpDownHiDriveTime.Value / (double)numUpDownHiDriveTime.Minimum));
            else if (numUpDownHiDriveTime.Value < 1)
            {
                Parameters.HiDriveTime = 1;
            }
            //UpdateMaximumValues();
        }

        private void numUpDownLowDriveInitTime_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDownLowDriveInitTime.Minimum > 0)
                Parameters.LowDriveInitTime = (byte)(Math.Ceiling((double)numUpDownLowDriveInitTime.Value /
                    (double)numUpDownLowDriveInitTime.Minimum));
            else if (numUpDownLowDriveInitTime.Value < 1)
            {
                Parameters.LowDriveInitTime = 1;
            }
            //UpdateMaximumValues();
        }
        void UpdateMaximumValues(object sender, EventArgs e)
        {
            if (Parameters.DriverPowerMode == 1)
            {
                numUpDownHiDriveTime.Maximum = numUpDownHiDriveTime.Minimum * 250 -
                    numUpDownLowDriveInitTime.Value - Parameters.m_DacDisInitTime * numUpDownHiDriveTime.Minimum;
                numUpDownLowDriveInitTime.Maximum = numUpDownLowDriveInitTime.Minimum * 250 -
                    numUpDownHiDriveTime.Value - Parameters.m_DacDisInitTime * numUpDownLowDriveInitTime.Minimum;
            }
            else
            {
                numUpDownHiDriveTime.Maximum = numUpDownHiDriveTime.Minimum * 255 -
                    numUpDownLowDriveInitTime.Value;
                numUpDownLowDriveInitTime.Maximum = numUpDownLowDriveInitTime.Minimum * 255 -
                    numUpDownHiDriveTime.Value;
            
            
            }
        }

        private void comboBoxLowDriveMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameters.LowDriveMode = (byte)comboBoxLowDriveMode.SelectedIndex;
        }

        private void CyDriverParams_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                editClockFrequency.Text = Parameters.ClockFrequency.ToString();
                InitDriveTime(numUpDownHiDriveTime,Parameters.HiDriveTime);
                InitDriveTime(numUpDownLowDriveInitTime,Parameters.LowDriveInitTime);
                UpdateMaximumValues(null, null);
            }
        }

        
    }
}
