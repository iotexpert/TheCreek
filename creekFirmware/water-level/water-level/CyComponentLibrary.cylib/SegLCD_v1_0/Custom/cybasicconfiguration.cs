/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace SegLCD_v1_0
{
    public partial class CyBasicConfiguration : UserControl
    {
        public LCDParameters Parameters;

        public CyBasicConfiguration()
        {
            InitializeComponent();

            this.Parameters = new LCDParameters();

            InitComboBoxes();
            SetBiasType();

            Parameters.NumCommonLines = (byte)numUpDownNumCommonLines.Value;
            Parameters.NumSegmentLines = (byte)numUpDownNumSegmentLines.Value;
        }


        public CyBasicConfiguration(LCDParameters parameters)
        {
            InitializeComponent();

            this.Parameters = parameters;

            InitComboBoxes();
            SetBiasType();

            LoadValuesFromParameters();
        }

        private void LoadValuesFromParameters()
        {
            numUpDownNumCommonLines.Value = Parameters.NumCommonLines;
            numUpDownNumSegmentLines.Value = Parameters.NumSegmentLines;
            comboBoxWaveformType.SelectedIndex = Parameters.WaveformType;
            comboBoxFrameRate.Text = Parameters.FrameRate.ToString();
            comboBoxBiasVoltage.SelectedIndex = Parameters.BiasVoltage;
            checkBoxGang.Checked = Parameters.Gang;
            checkBoxDebugMode.Checked = Parameters.DebugMode;
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

        private void numUpDownNumCommonLines_ValueChanged(object sender, EventArgs e)
        {
            if ((Parameters.HelpersConfig.Count > 1) && (numUpDownNumCommonLines.Value != Parameters.NumCommonLines))
            {
                MessageBox.Show("Please, remove helper functions first.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                numUpDownNumCommonLines.Value = Parameters.NumCommonLines;
                return;
            }
            numUpDownNumSegmentLines.Maximum = 64 - (int)numUpDownNumCommonLines.Value;
            SetBiasType();
            
            if (Parameters.NumCommonLines != (byte)numUpDownNumCommonLines.Value)
                Parameters.NumCommonLines = (byte)numUpDownNumCommonLines.Value;

            UpdateDisabledCommons();
            SetClockFrequency();

        }

        private void SetBiasType()
        {
            int value = (int)numUpDownNumCommonLines.Value;
            if (value == 1)
            {
                editBiasType.Text = "Direct";
                Parameters.BiasType = 0;
            }
            else if ((value >= 2) && (value <= 6))
            {
                editBiasType.Text = "1/3";
                Parameters.BiasType = 0;
            }
            else if ((value >= 7) && (value <= 11))
            {
                editBiasType.Text = "1/4";
                Parameters.BiasType = 1;
            }
            else // 12-16
            {
                editBiasType.Text = "1/5";
                Parameters.BiasType = 2;
            }
        }

        private void SetClockFrequency()
        {
            Parameters.ClockFrequency = Convert.ToUInt32((Parameters.NumCommonLines-Parameters.DisabledCommons.Count)*Parameters.FrameRate*256);
        }

        private void UpdateDisabledCommons()
        {
            foreach (int val in Parameters.DisabledCommons)
            {
                if (val >= Parameters.NumCommonLines)
                {
                    List<int> DisabledCommons = new List<int>(Parameters.DisabledCommons);
                    DisabledCommons.Remove(val);
                    Parameters.DisabledCommons = DisabledCommons;
                }
            }
        }

        private void numUpDownNumSegmentLines_ValueChanged(object sender, EventArgs e)
        {
            if ((Parameters.HelpersConfig.Count > 1) && (numUpDownNumSegmentLines.Value != Parameters.NumSegmentLines))
            {
                MessageBox.Show("Please, remove helper functions first.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                numUpDownNumSegmentLines.Value = Parameters.NumSegmentLines;
                return;
            }

            if (Parameters.NumSegmentLines != (byte)numUpDownNumSegmentLines.Value)
                Parameters.NumSegmentLines = (byte)numUpDownNumSegmentLines.Value;
        }

        private void comboBoxWaveformType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameters.WaveformType = (byte)comboBoxWaveformType.SelectedIndex;
        }

        private void comboBoxFrameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameters.FrameRate = Convert.ToByte(comboBoxFrameRate.Text);
            SetClockFrequency();
        }

        private void comboBoxBiasVoltage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameters.BiasVoltage = (byte)comboBoxBiasVoltage.SelectedIndex;
        }

        private void checkBoxGang_CheckedChanged(object sender, EventArgs e)
        {
            Parameters.Gang = checkBoxGang.Checked;
        }

        private void checkBoxDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            Parameters.DebugMode = checkBoxDebugMode.Checked;
        }
    }
}

