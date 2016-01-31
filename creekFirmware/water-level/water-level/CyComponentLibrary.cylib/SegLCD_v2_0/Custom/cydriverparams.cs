/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SegLCD_v2_0
{
    public partial class CyDriverParams : UserControl
    {
        private enum CyHiDriveStrengthIndex
        {
            S1_C1, S1_C2, S1_C4, S2_C2, S2_C4, S4_C4
        } ;

        public CyLCDParameters m_parameters;
        private bool m_internalChanges;

        public CyDriverParams()
        {
            InitializeComponent();
            m_parameters = new CyLCDParameters();
        }

        public CyDriverParams(CyLCDParameters parameters)
        {
            InitializeComponent();

            this.m_parameters = parameters;
            this.m_parameters.m_cyDriverParamsTab = this;
            m_internalChanges = true;
            LoadValuesFromParameters();
            m_internalChanges = false;
        }

        private void LoadValuesFromParameters()
        {
            numUpDownGlassSize.Value = m_parameters.GlassSize;
            UpdateNumUpDownDriveTimeData(this, new EventArgs());
            checkBoxAdvanced.Checked = m_parameters.AdvancedSettings;
            comboBoxHiDriveStrength.SelectedIndex = m_parameters.HiDriveStrength;
            comboBoxLowDriveStrength.SelectedIndex = m_parameters.LowDriveStrength > 1
                                                         ? m_parameters.LowDriveStrength - 6
                                                         : m_parameters.LowDriveStrength;
            SetAdvParamsVisibility(checkBoxAdvanced.Checked);
        }

        private void numUpDownHiDriveTime_ValueChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

            m_parameters.HiDriveTime = ValidateUpDownDriveTimeValue(numUpDownHiDriveTime.Value);
        }

        private void numUpDownLowDriveInitTime_ValueChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

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
        /// Updates data in numUpDownLowDriveTime and numUpDownHiDriveTime
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
                numUpDown.Maximum = (254 - listDrTime[(i + 1) % 2]) * m_parameters.InputClockPeriod;
               
                numUpDown.Increment = m_parameters.InputClockPeriod;

                decimal val = listDrTime[i]*m_parameters.InputClockPeriod;
                if (val <= numUpDown.Maximum)
                    numUpDown.Value = val;
                else
                {
                    //Update old value
                    listDrTime[0] = m_parameters.HiDriveTime;
                }
            }
        }

        public void UpdateStrength()
        {
            const int GLASS_MIDDLE_VAL = 25;
            const int GLASS_LARGE_VAL = 40;
            int proportion = Convert.ToInt32(
                    Math.Round(m_parameters.NumSegmentLines/(double) (m_parameters.NumCommonLines)));
            if (proportion <= 1)
            {
                if (m_parameters.GlassSize <= GLASS_MIDDLE_VAL)
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S1_C1;
                }
                else if (m_parameters.GlassSize <= GLASS_LARGE_VAL)
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S2_C2;
                }
                else
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S4_C4;
                }
            }
            else if ((proportion == 2) || (proportion == 3))
            {
                if (m_parameters.GlassSize <= GLASS_MIDDLE_VAL)
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S1_C2;
                }
                else if (m_parameters.GlassSize <= GLASS_LARGE_VAL)
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S2_C4;
                }
                else
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S4_C4;
                }
            }
            else if (proportion > 3)
            {
                if (m_parameters.GlassSize <= GLASS_MIDDLE_VAL)
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S1_C4;
                }
                else if (m_parameters.GlassSize <= GLASS_LARGE_VAL)
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S2_C4;
                }
                else
                {
                    comboBoxHiDriveStrength.SelectedIndex = (int)CyHiDriveStrengthIndex.S4_C4;
                }
            }
        }
      
        private void CyDriverParams_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateNumUpDownDriveTimeData(this, new EventArgs());
                SetAdvParamsVisibility(m_parameters.AdvancedSettings);
            }
        }

        private void SetAdvParamsVisibility(bool vis)
        {
            labelHighDriverTime.Enabled = vis;
            labelLowDriveInitTime.Enabled = vis;
            labelHiDriveStrength.Enabled = vis;
            labelLowDriveStrength.Enabled = vis;
            numUpDownHiDriveTime.Enabled = vis;
            numUpDownLowDriveInitTime.Enabled = vis;
            comboBoxHiDriveStrength.Enabled = vis;

            if (m_parameters.DriverPowerMode != (byte)CyBasicConfiguration.CyMode.NOSLEEP)
                numUpDownLowDriveInitTime.Enabled = false;
        }

        private void checkBoxAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;
            
            m_parameters.AdvancedSettings = checkBoxAdvanced.Checked;
            SetAdvParamsVisibility(checkBoxAdvanced.Checked);

            if (m_parameters.AdvancedSettings == false)
            {
                UpdateStrength();
                m_parameters.m_cyBasicConfigurationTab.SetDefaultValuesHiLowDriveTime();
                UpdateNumUpDownDriveTimeData(this, new EventArgs());
            }
        }

        private void numUpDownGlassSize_ValueChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

            m_parameters.GlassSize = (byte)numUpDownGlassSize.Value;
            if (m_parameters.AdvancedSettings == false)
                UpdateStrength();
        }

        private void comboBoxHiDriveStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

            m_parameters.HiDriveStrength = (byte)comboBoxHiDriveStrength.SelectedIndex;
            
            //Set LowDriveStrength
            comboBoxLowDriveStrength.SelectedIndex = m_parameters.HiDriveStrength%2;
        }

        private void comboBoxLowDriveStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

            m_parameters.LowDriveStrength = (byte)(comboBoxLowDriveStrength.SelectedIndex + 6);
        }
    }
}
