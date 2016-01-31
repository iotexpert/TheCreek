/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SegLCD_v2_10
{
    public partial class CyBasicConfiguration : UserControl
    {
        private static readonly byte[] MODE_INDEXES_COMBOBOX = { 2, 0, 1 };
        public enum CyMode
        {
            NOSLEEP, LOW_POWER_ILO, LOW_POWER_32K
        } ;
        private const int FRAME_RATE_MAX = 150;
        private const int BIAS_VOLTAGE_ITEM_COUNT = 64;

        public CyLCDParameters m_parameters;
        private readonly bool m_internalChanges;
        private List<uint> m_frameRateRange = new List<uint>();

        public CyBasicConfiguration()
        {
            InitializeComponent();
        }


        public CyBasicConfiguration(CyLCDParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            m_parameters.m_cyBasicConfigurationTab = this;

            m_internalChanges = true;
            if (m_parameters.SupplyType == 0)
                radioButton3V.Checked = true;
            else
                radioButton5V.Checked = true;
            InitFrameRateComboBox();
            InitBiasVoltageComboBox();
            LoadValuesFromParameters();

            toolTip.SetToolTip(numUpDownNumSegmentLines, Properties.Resources.NUMLINES_DECREASE_MSG);
            toolTip.SetToolTip(numUpDownNumCommonLines, Properties.Resources.NUMLINES_DECREASE_MSG);

            m_internalChanges = false;
        }

        public void LoadValuesFromParameters()
        {
            numUpDownNumCommonLines.Value = m_parameters.NumCommonLines;
            numUpDownNumSegmentLines.Value = m_parameters.NumSegmentLines;
            comboBoxWaveformType.SelectedIndex = m_parameters.WaveformType;

            if (comboBoxFrameRate.Items.Contains(m_parameters.FrameRate))
                comboBoxFrameRate.Text = m_parameters.FrameRate.ToString();
            else
                comboBoxFrameRate.SelectedIndex = comboBoxFrameRate.Items.Count - 1;

            if (m_parameters.BiasVoltage < BIAS_VOLTAGE_ITEM_COUNT - comboBoxBiasVoltage.Items.Count)
            {
                comboBoxBiasVoltage.SelectedIndex = 0;
            }
            else if (m_parameters.BiasVoltage < BIAS_VOLTAGE_ITEM_COUNT )
                comboBoxBiasVoltage.SelectedIndex = m_parameters.BiasVoltage - BIAS_VOLTAGE_ITEM_COUNT +
                                                    comboBoxBiasVoltage.Items.Count;
            else
                comboBoxBiasVoltage.SelectedIndex = comboBoxBiasVoltage.Items.Count - 1;

            checkBoxGang.Checked = m_parameters.Gang;
            comboBoxBiasType.SelectedIndex = m_parameters.BiasType;
            comboBoxDriverPowerMode.SelectedIndex = MODE_INDEXES_COMBOBOX[m_parameters.DriverPowerMode];

            toolTip.Active = m_parameters.m_helpersConfig.Count > 1;
        }

        private void InitFrameRateComboBox()
        {
            double max = GetFrameRateMaximum();
            comboBoxFrameRate.Items.Clear();
            m_frameRateRange.Clear();
            if ((m_parameters.DriverPowerMode == (byte)CyMode.LOW_POWER_32K) ||
                (m_parameters.DriverPowerMode == (byte)CyMode.LOW_POWER_ILO))
            {
                int i = 2;
                while ((max / i > 10) && (i <= 255))
                {
                    m_frameRateRange.Insert(0, (uint)(max / i++));
                    // Check max
                    if (m_frameRateRange[0] <= FRAME_RATE_MAX)
                        // Check step
                        if ((comboBoxFrameRate.Items.Count == 0) ||
                            ((comboBoxFrameRate.Items.Count > 0) &&
                            ((byte)comboBoxFrameRate.Items[0] - m_frameRateRange[0] >= 5)))
                        {
                            comboBoxFrameRate.Items.Insert(0, (byte)m_frameRateRange[0]);
                        }
                }
            }
            else
            {
                for (int i = 10; i <= max; i += 10)
                {
                    m_frameRateRange.Add((uint)i);
                    comboBoxFrameRate.Items.Add((byte) i);
                }
            }
        }

        private void InitBiasVoltageComboBox()
        {
            comboBoxBiasVoltage.Items.Clear();
            const double SUPPLY_3V = 3.0, SUPPLY_5V = 5.5;
            double supply = radioButton3V.Checked ? SUPPLY_3V : SUPPLY_5V;
            double step = 0.0273 * supply / 3;
            for (int i = 0; i < 64; i++)
            {
                double res = supply - i*step;
                string sres = radioButton3V.Checked ? res.ToString("f3") : res.ToString("f2");
                if (res > 2.0 - 0.00000001)
                    comboBoxBiasVoltage.Items.Insert(0, sres);
            }
        }

        private void UpdateFrameRateMax()
        {
            InitFrameRateComboBox();
           
            if (m_parameters.FrameRate >= (byte)comboBoxFrameRate.Items[comboBoxFrameRate.Items.Count-1])
            {
                m_parameters.FrameRate = (byte)comboBoxFrameRate.Items[comboBoxFrameRate.Items.Count - 1];
            }
            else if ((m_parameters.FrameRate <= (byte) comboBoxFrameRate.Items[0]))
            {
                m_parameters.FrameRate = (byte) comboBoxFrameRate.Items[0];
            }
            else
            {
                for (int i = 0; i < comboBoxFrameRate.Items.Count - 1; i++)
                {
                    byte highLimit = Convert.ToByte(comboBoxFrameRate.Items[i + 1]);
                    byte lowLimit = Convert.ToByte(comboBoxFrameRate.Items[i]);
                    if ((m_parameters.FrameRate <= highLimit) && (m_parameters.FrameRate >= lowLimit))
                    {
                        m_parameters.FrameRate = highLimit - m_parameters.FrameRate < m_parameters.FrameRate - lowLimit
                                                     ? highLimit
                                                     : lowLimit;
                        break;
                    }
                }
            }
            comboBoxFrameRate.SelectedIndex = comboBoxFrameRate.Items.IndexOf(m_parameters.FrameRate);
        }

        private void numUpDownNumLines_ValueChanged(object sender, EventArgs e)
        {
            bool isCommonChanged = sender == numUpDownNumCommonLines;
            NumericUpDown numUpDown = (NumericUpDown) sender;
            byte paramValue = isCommonChanged ? m_parameters.NumCommonLines : m_parameters.NumSegmentLines;

            if (isCommonChanged)
            {
                numUpDownNumSegmentLines.Maximum = 64 - (int)numUpDownNumCommonLines.Value;
            }

            bool updateHelpers = numUpDown.Value != paramValue;

            if (paramValue != (byte)numUpDown.Value)
            {
                if (isCommonChanged)
                {
                    m_parameters.NumCommonLines = (byte) numUpDown.Value;
                    SetBiasType();
                }
                else
                    m_parameters.NumSegmentLines = (byte)numUpDown.Value;
                m_parameters.m_cyDriverParamsTab.UpdateStrength();
            }

            if (isCommonChanged)
            {
                UpdateFrameRateMax();
                SetClockFrequency();
            }

            //Update Empty helper
            if (updateHelpers)
            {
                m_parameters.m_cyHelpersTab.ComSegLinesNumChanged();
            }
        }

        private void SetBiasType()
        {
            if (m_internalChanges) return;

            int value = (int)numUpDownNumCommonLines.Value;
            if ((value >= 2) && (value <= 6))
            {
                comboBoxBiasType.SelectedIndex = 0;
            }
            else if ((value >= 7) && (value <= 11))
            {
                comboBoxBiasType.SelectedIndex = 1;
            }
            else // 12-16
            {
                comboBoxBiasType.SelectedIndex = 2;
            }
        }

        private void SetClockFrequency()
        {
            m_parameters.ClockFrequency = Convert.ToUInt32(Math.Ceiling((m_parameters.WaveformType == 0)
                                          ? (m_parameters.NumCommonLines)*m_parameters.FrameRate*512*1.075
                                          : (m_parameters.NumCommonLines)*m_parameters.FrameRate*256*1.075));
        }

        private double GetFrameRateMaximum()
        {
            double max = FRAME_RATE_MAX;
            if (m_parameters.DriverPowerMode == (byte)CyMode.LOW_POWER_ILO)
            {
                if (m_parameters.WaveformType == 0)
                {
                    max = 1000.0 / (2 * m_parameters.NumCommonLines);
                }
                else
                {
                    max = 1000.0 / m_parameters.NumCommonLines;
                }
                     
            }
            else if (m_parameters.DriverPowerMode == (byte)CyMode.LOW_POWER_32K)
            {
                if (m_parameters.WaveformType == 0)
                {
                    max = 8000.0 / (2 * m_parameters.NumCommonLines);
                }
                else
                {
                    max = 8000.0 / m_parameters.NumCommonLines;
                }
            }
            return max;
        }

        private void comboBoxWaveformType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.WaveformType = (byte)comboBoxWaveformType.SelectedIndex;
            SetClockFrequency();
            UpdateFrameRateMax();
        }

        private void comboBoxFrameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.FrameRate = Convert.ToByte(comboBoxFrameRate.Text);
            if (comboBoxFrameRate.SelectedIndex >= 0)
            {
                m_parameters.TimerPeriod =
                    Convert.ToByte(m_frameRateRange.Count - m_frameRateRange.IndexOf(
                                   (byte) comboBoxFrameRate.Items[comboBoxFrameRate.SelectedIndex]));                
            }
            SetClockFrequency();
        }

        private void comboBoxBiasVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.BiasVoltage =
                (byte) (comboBoxBiasVoltage.SelectedIndex + BIAS_VOLTAGE_ITEM_COUNT - comboBoxBiasVoltage.Items.Count);
        }

        private void checkBoxGang_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.Gang = checkBoxGang.Checked;
        }

        private void comboBoxBiasType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.BiasType = (byte)comboBoxBiasType.SelectedIndex;
        }

        private void comboBoxDriverPowerMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.DriverPowerMode =
                (byte)(new List<byte>(MODE_INDEXES_COMBOBOX)).IndexOf((byte)comboBoxDriverPowerMode.SelectedIndex);
            if ((m_parameters.m_globalEditMode))
            {
                SetDefaultValuesHiLowDriveTime();
            }
            UpdateFrameRateMax();
            m_parameters.ClockFrequencyGeneral = 0; // SetParamExpr
        }

        public void SetDefaultValuesHiLowDriveTime()
        {
            if (m_parameters.DriverPowerMode == (byte)CyMode.NOSLEEP)
            {
                m_parameters.LowDriveInitTime = 126;
            }
            else
            {
                m_parameters.LowDriveInitTime = 1;
            }
            m_parameters.HiDriveTime = 128;
        }

        private void CyBasicConfiguration_VisibleChanged(object sender, EventArgs e)
        {
            toolTip.Active = m_parameters.m_helpersConfig.Count > 1;
        }

        private void radioButton3V_CheckedChanged(object sender, EventArgs e)
        {
            if (m_internalChanges) return;

            if (((RadioButton)sender).Checked)
            {
                InitBiasVoltageComboBox();
                if (m_parameters.BiasVoltage < BIAS_VOLTAGE_ITEM_COUNT - comboBoxBiasVoltage.Items.Count)
                    comboBoxBiasVoltage.SelectedIndex = 0;
                else if (m_parameters.BiasVoltage < BIAS_VOLTAGE_ITEM_COUNT)
                    comboBoxBiasVoltage.SelectedIndex = m_parameters.BiasVoltage -
                                                        (BIAS_VOLTAGE_ITEM_COUNT - comboBoxBiasVoltage.Items.Count);
                else
                    comboBoxBiasVoltage.SelectedIndex = comboBoxBiasVoltage.Items.Count - 1;

                m_parameters.SupplyType = radioButton3V.Checked ? (byte)0 : (byte)1;
            }
        }
    }
}

