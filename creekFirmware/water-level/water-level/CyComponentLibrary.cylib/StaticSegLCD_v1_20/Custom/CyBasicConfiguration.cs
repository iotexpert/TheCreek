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

namespace StaticSegLCD_v1_20
{
    public partial class CyBasicConfiguration : UserControl
    {
        public CyLCDParameters m_Parameters;
        public bool m_ParametersChanged;

        public CyBasicConfiguration()
        {
            InitializeComponent();

            this.m_Parameters = new CyLCDParameters();

            InitComboBoxes();

            m_Parameters.NumSegmentLines = (byte)numUpDownNumSegmentLines.Value;
        }


        public CyBasicConfiguration(CyLCDParameters parameters)
        {
            InitializeComponent();

            this.m_Parameters = parameters;

            InitComboBoxes();

            LoadValuesFromParameters();
        }

        private void LoadValuesFromParameters()
        {
            numUpDownNumSegmentLines.Value = m_Parameters.NumSegmentLines;
            comboBoxWaveformType.SelectedIndex = m_Parameters.WaveformType;
            comboBoxFrameRate.Text = m_Parameters.FrameRate.ToString();
            checkBoxDebugMode.Checked = m_Parameters.DebugMode;
        }

        private void InitComboBoxes()
        {
            comboBoxFrameRate.Items.Clear();
            for (int i = 10; i <= 150; i += 10)
                comboBoxFrameRate.Items.Add(i);
        }

        private void SetClockFrequency()
        {
            m_Parameters.ClockFrequency =
                Convert.ToUInt32((m_Parameters.NumCommonLines - m_Parameters.DisabledCommons.Count)*
                                 m_Parameters.FrameRate*512);
        }

        private void UpdateDisabledCommons()
        {
            foreach (int val in m_Parameters.DisabledCommons)
            {
                if (val >= m_Parameters.NumCommonLines)
                {
                    List<int> DisabledCommons = new List<int>(m_Parameters.DisabledCommons);
                    DisabledCommons.Remove(val);
                    m_Parameters.DisabledCommons = DisabledCommons;
                }
            }
        }

        private void numUpDownNumSegmentLines_ValueChanged(object sender, EventArgs e)
        {
            if ((m_Parameters.m_HelpersConfig.Count > 1) && 
                (numUpDownNumSegmentLines.Value != m_Parameters.NumSegmentLines))
            {
                MessageBox.Show("Please, remove helper functions first.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                numUpDownNumSegmentLines.Value = m_Parameters.NumSegmentLines;
                return;
            }

            if (m_Parameters.NumSegmentLines != (byte)numUpDownNumSegmentLines.Value)
                m_Parameters.NumSegmentLines = (byte)numUpDownNumSegmentLines.Value;

            //Update Empty helper
            HelperInfo.UpdateEmptyHelper(m_Parameters);
            m_ParametersChanged = true;
        }

        private void comboBoxWaveformType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Parameters.WaveformType = (byte)comboBoxWaveformType.SelectedIndex;
        }

        private void comboBoxFrameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Parameters.FrameRate = Convert.ToByte(comboBoxFrameRate.Text);
            SetClockFrequency();
        }

        private void checkBoxDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            m_Parameters.DebugMode = checkBoxDebugMode.Checked;
        }

        private void CyBasicConfiguration_Leave(object sender, EventArgs e)
        {
            // Save changes
            if (m_ParametersChanged)
                m_Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(m_Parameters.m_HelpersConfig);
            m_ParametersChanged = false;
        }
    }
}

