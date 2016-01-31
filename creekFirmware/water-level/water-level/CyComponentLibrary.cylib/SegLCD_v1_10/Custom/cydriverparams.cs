/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SegLCD_v1_10
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
            LoadHiDriveTime();
            comboBoxLowDriveMode.SelectedIndex = Parameters.LowDriveMode;
            numUpDownLowDriveInitTime.Value = Parameters.LowDriveInitTime;
            editClockFrequency.Text = Parameters.ClockFrequency.ToString();
        }

        void InitHiDriveTime()
        {
            // If were changes (based on Minimum and formula)
            if (Math.Abs((double)numUpDownHiDriveTime.Minimum - Math.Pow(10, 6) / Parameters.ClockFrequency) > 0.001)
            {
                numUpDownHiDriveTime.Minimum = 0;
                numUpDownHiDriveTime.Maximum = 1000000;
                numUpDownHiDriveTime.Value = (decimal) (Math.Pow(10, 6)/Parameters.ClockFrequency);
                numUpDownHiDriveTime.Minimum = numUpDownHiDriveTime.Value;
                numUpDownHiDriveTime.Maximum = numUpDownHiDriveTime.Minimum*254;
                numUpDownHiDriveTime.Increment = (decimal) (Math.Pow(10, 6)/Parameters.ClockFrequency);
            }
        }

        void LoadHiDriveTime()
        {
            numUpDownHiDriveTime.Minimum = 0;
            numUpDownHiDriveTime.Maximum = 1000000;
            numUpDownHiDriveTime.Value = (decimal)(Math.Pow(10, 6) / Parameters.ClockFrequency) * Parameters.HiDriveTime;
            numUpDownHiDriveTime.Minimum = (decimal)(Math.Pow(10, 6) / Parameters.ClockFrequency);
            numUpDownHiDriveTime.Maximum = numUpDownHiDriveTime.Minimum * 254;
            numUpDownHiDriveTime.Increment = (decimal)(Math.Pow(10, 6) / Parameters.ClockFrequency);
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
            }
            else
            {
                comboBoxLowDriveMode.Enabled = true;
                numUpDownLowDriveInitTime.Enabled = true;
            }
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
        }

        private void comboBoxLowDriveMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameters.LowDriveMode = (byte)comboBoxLowDriveMode.SelectedIndex;
        }

        private void numUpDownLowDriveInitTime_ValueChanged(object sender, EventArgs e)
        {
            Parameters.LowDriveInitTime = (byte)numUpDownLowDriveInitTime.Value;
        }

        private void CyDriverParams_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                editClockFrequency.Text = Parameters.ClockFrequency.ToString();
                InitHiDriveTime();
            }
        }

        
    }
}
