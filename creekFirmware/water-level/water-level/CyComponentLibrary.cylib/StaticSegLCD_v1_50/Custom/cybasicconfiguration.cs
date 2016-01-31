/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Windows.Forms;

namespace StaticSegLCD_v1_50
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

            toolTip.SetToolTip(numUpDownNumSegmentLines,
                Properties.Resources.SEG_LINES_DECREASE_MSG);

            InitFrameRate();
            LoadValuesFromParameters();
        }

        public void LoadValuesFromParameters()
        {
            numUpDownNumSegmentLines.Value = m_parameters.NumSegmentLines;
            comboBoxFrameRate.Text = m_parameters.FrameRate.ToString();

            toolTip.Active = m_parameters.m_helpersConfig.Count > 1;
        }

        private void InitFrameRate()
        {
            comboBoxFrameRate.Items.Clear();
            for (int i = 10; i <= 150; i += 10)
                comboBoxFrameRate.Items.Add(i);
        }

        private void numUpDownNumSegmentLines_ValueChanged(object sender, EventArgs e)
        {
            if (numUpDownNumSegmentLines.Value != m_parameters.NumSegmentLines)
            {
                m_parameters.NumSegmentLines = (byte) numUpDownNumSegmentLines.Value;
                //Update Helpers mapping
                m_parameters.m_cyHelpersTab.SegLinesNumChanged();
            }
        }

        private void comboBoxFrameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.FrameRate = Convert.ToByte(comboBoxFrameRate.Text);
        }

        private void CyBasicConfiguration_VisibleChanged(object sender, EventArgs e)
        {
            toolTip.Active = m_parameters.m_helpersConfig.Count > 1;
        }
    }
}

