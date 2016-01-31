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

namespace StaticSegLCD_v1_30
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
            comboBoxFrameRate.Text = m_Parameters.FrameRate.ToString();
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
                Convert.ToUInt32((m_Parameters.NumCommonLines)*
                                 m_Parameters.FrameRate*512);
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


        private void comboBoxFrameRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Parameters.FrameRate = Convert.ToByte(comboBoxFrameRate.Text);
            SetClockFrequency();
        }

        private void CyBasicConfiguration_Leave(object sender, EventArgs e)
        {
            // Save changes
            if (m_ParametersChanged)
                m_Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(m_Parameters.m_HelpersConfig);
            m_ParametersChanged = false;
        }

        public void SetSegEditable()
        {
            bool isEnabled = true;
            if (m_Parameters.m_HelpersConfig.Count > 1)
                isEnabled = false;
            numUpDownNumSegmentLines.Enabled = isEnabled;
            labelSegEnabled.Visible = !isEnabled;
        }

        private void CyBasicConfiguration_VisibleChanged(object sender, EventArgs e)
        {
            SetSegEditable();
        }
    }
}

