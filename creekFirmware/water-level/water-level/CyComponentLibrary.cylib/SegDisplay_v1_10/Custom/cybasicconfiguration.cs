/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SegDisplay_v1_10
{
    public partial class CyBasicConfiguration : UserControl
    {
        public CyLCDParameters m_parameters;

        public CyBasicConfiguration()
        {
            InitializeComponent();
        }

        public CyBasicConfiguration(CyLCDParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            m_parameters.m_cyBasicConfigurationTab = this;

            InitComboBoxes();
            LoadValuesFromParameters();
            toolTip.SetToolTip(numUpDownNumSegmentLines, Properties.Resources.NUMLINES_DECREASE_MSG);
            toolTip.SetToolTip(numUpDownNumCommonLines, Properties.Resources.NUMLINES_DECREASE_MSG);
        }

        public void LoadValuesFromParameters()
        {
            numUpDownNumCommonLines.Value = m_parameters.NumCommonLines;
            numUpDownNumSegmentLines.Value = m_parameters.NumSegmentLines;
            comboBoxWaveformType.SelectedIndex = m_parameters.WaveformType;
            comboBoxFrameRate.Text = m_parameters.FrameRate.ToString();
            comboBoxBiasVoltage.SelectedIndex = m_parameters.BiasVoltage;
            checkBoxGang.Checked = m_parameters.Gang;
            checkBoxDebugMode.Checked = m_parameters.DebugMode;
            comboBoxBiasType.SelectedIndex = m_parameters.BiasType;

            toolTip.Active = m_parameters.m_helpersConfig.Count > 1;
        }

        private void InitComboBoxes()
        {
            comboBoxFrameRate.Items.Clear();
            for (int i = 10; i <= 150; i += 10)
                comboBoxFrameRate.Items.Add(i);

            comboBoxBiasVoltage.Items.Clear();
            for (double i = 2.0; i < 5.21; i += 0.05)
                comboBoxBiasVoltage.Items.Add(i.ToString("f2"));
        }

        private void SetBiasType()
        {
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
            m_parameters.ClockFrequency = m_parameters.WaveformType == 0
                                  ? Convert.ToUInt32((m_parameters.NumCommonLines)*m_parameters.FrameRate*512)
                                  : Convert.ToUInt32((m_parameters.NumCommonLines)*m_parameters.FrameRate*256);
        }

        private void numUpDownNumLines_ValueChanged(object sender, EventArgs e)
        {
            bool isCommonChanged = sender == numUpDownNumCommonLines;
            NumericUpDown numUpDown = (NumericUpDown)sender;
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
            }

            if (isCommonChanged)
            {
                SetClockFrequency();
            }

            //Update Empty helper
            if (updateHelpers)
            {
                m_parameters.m_cyHelpersTab.ComSegLinesNumChanged();
            }

        }

        private void comboBoxWaveformType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.WaveformType = (byte)comboBoxWaveformType.SelectedIndex;
            SetClockFrequency();
        }

        private void comboBoxFrameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.FrameRate = Convert.ToByte(comboBoxFrameRate.Text);
            SetClockFrequency();
        }

        private void comboBoxBiasVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.BiasVoltage = (byte)comboBoxBiasVoltage.SelectedIndex;
        }

        private void checkBoxGang_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.Gang = checkBoxGang.Checked;
        }

        private void checkBoxDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.DebugMode = checkBoxDebugMode.Checked;
        }

        private void comboBoxBiasType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.BiasType = (byte)comboBoxBiasType.SelectedIndex;
        }

        private void CyBasicConfiguration_VisibleChanged(object sender, EventArgs e)
        {
            toolTip.Active = m_parameters.m_helpersConfig.Count > 1;
        }
    }
}

