/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SPDIF_Tx_v1_10
{
    public partial class CyChannelControl : UserControl
    {
        private int m_channelIndex = 0;
        private CySPDifTxParameters m_params;
        // Indicates whether negative value tried to be entered
        private bool m_minusEntered = false;

        #region Class Propertie(s)
        public bool FirstGroupBoxEnabled
        {
            get { return m_groupBox2.Enabled; }
            set { m_groupBox2.Enabled = value; }
        }
        #endregion

        #region Constructor(s)
        public CyChannelControl()
        {
            InitializeComponent();
        }
        public CyChannelControl(CySPDifTxParameters inst, CyEChannel channelIndex)
        {
            m_params = inst;
            m_channelIndex = (channelIndex == CyEChannel.Ch0) ? 0 : 1;

            InitializeComponent();

            // Create NumericUpDown TextChange Event Handlers
            m_numSourceNumber.TextChanged += new EventHandler(m_numSourceNumber_TextChanged);
            m_numChannelNumber.TextChanged += new EventHandler(m_numChannelNumber_TextChanged);

            // Filling comboboxes
            m_cbDataType.DataSource = m_params.m_dataTypeList;
            m_cbCopyright.DataSource = m_params.m_copyrightList;
            m_cbPreEmphasis.DataSource = m_params.m_preEmphasisList;
            m_cbCategory.DataSource = m_params.m_categoryList;
            m_cbClockAccuracy.DataSource = m_params.m_clockAccuracyList;
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateUI()
        {
            m_params.m_globalEditMode = false;
            m_cbDataType.Text = CyDictParser.GetDictValue(m_params.m_dnDict,
                m_params.m_channels[m_channelIndex].DataTypeCh);
            m_cbCopyright.Text = CyDictParser.GetDictValue(m_params.m_dnDict,
                m_params.m_channels[m_channelIndex].CopyrightCh);
            m_cbPreEmphasis.Text = CyDictParser.GetDictValue(m_params.m_dnDict,
                m_params.m_channels[m_channelIndex].PreEmphasisCh);
            m_cbCategory.Text = CyDictParser.GetDictValue(m_params.m_dnDict,
                m_params.m_channels[m_channelIndex].CategoryCh);
            m_cbClockAccuracy.Text = CyDictParser.GetDictValue(m_params.m_dnDict,
                m_params.m_channels[m_channelIndex].ClockAccuracyCh);
            m_numSourceNumber.Text = CyDictParser.GetDictValue(m_params.m_dnSrcNumDict,
                m_params.m_channels[m_channelIndex].SourceNumberCh);
            m_numChannelNumber.Text = CyDictParser.GetDictValue(m_params.m_dnChNumDict,
                m_params.m_channels[m_channelIndex].ChannelNumberCh);
            m_params.m_globalEditMode = true;
        }
        #endregion

        #region Event Handlers
        private void m_cbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_params.m_globalEditMode)
                m_params.m_channels[m_channelIndex].DataTypeCh = (CyEDataType)CyDictParser.GetDictKey(
                    m_params.m_dnDict, m_cbDataType.Text);
            if (m_channelIndex == 0 && m_params.CopyDefaults)
            {
                m_params.m_channels[1].DataTypeCh = m_params.m_channels[0].DataTypeCh;
                m_params.m_channels[1].StatusDataCh = m_params.m_channels[1].GetHexStatusDataCh();
            }
        }

        private void m_cbCopyright_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_params.m_globalEditMode)
                m_params.m_channels[m_channelIndex].CopyrightCh = (CyECopyrightType)CyDictParser.GetDictKey(
                    m_params.m_dnDict, m_cbCopyright.Text);
            if (m_channelIndex == 0 && m_params.CopyDefaults)
            {
                m_params.m_channels[1].CopyrightCh = m_params.m_channels[0].CopyrightCh;
                m_params.m_channels[1].StatusDataCh = m_params.m_channels[1].GetHexStatusDataCh();
            }
        }

        private void m_cbPreEmphasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_params.m_globalEditMode)
                m_params.m_channels[m_channelIndex].PreEmphasisCh = (CyEPreEmphasisType)CyDictParser.GetDictKey(
                    m_params.m_dnDict,
                    m_cbPreEmphasis.Text);
            if (m_channelIndex == 0 && m_params.CopyDefaults)
            {
                m_params.m_channels[1].PreEmphasisCh = m_params.m_channels[0].PreEmphasisCh;
                m_params.m_channels[1].StatusDataCh = m_params.m_channels[1].GetHexStatusDataCh();
            }
        }

        private void m_cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_params.m_globalEditMode)
                m_params.m_channels[m_channelIndex].CategoryCh = (CyECategoryType)CyDictParser.GetDictKey(
                    m_params.m_dnDict, m_cbCategory.Text);
            if (m_channelIndex == 0 && m_params.CopyDefaults)
            {
                m_params.m_channels[1].CategoryCh = m_params.m_channels[0].CategoryCh;
                m_params.m_channels[1].StatusDataCh = m_params.m_channels[1].GetHexStatusDataCh();
            }
        }

        private void m_cbClockAccuracy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_params.m_globalEditMode)
                m_params.m_channels[m_channelIndex].ClockAccuracyCh = (CyEClockAccuracyType)CyDictParser.GetDictKey(
                    m_params.m_dnDict,
                    m_cbClockAccuracy.Text);
            if (m_channelIndex == 0 && m_params.CopyDefaults)
            {
                m_params.m_channels[1].ClockAccuracyCh = m_params.m_channels[0].ClockAccuracyCh;
                m_params.m_channels[1].StatusDataCh = m_params.m_channels[1].GetHexStatusDataCh();
            }
        }

        private void m_numSourceNumber_TextChanged(object sender, EventArgs e)
        {
            Control currentControl = (Control)sender;
            try
            {
                byte value = byte.Parse(currentControl.Text); // Convert value that starts with 0
                m_params.m_channels[m_channelIndex].SourceNumberCh = (CyESourceNumberType)CyDictParser.GetDictKey(
                    m_params.m_dnSrcNumDict, value.ToString());
                m_errorProvider.SetError(currentControl, string.Empty);
            }
            catch
            {
                string channel = ((m_channelIndex == 0) ? CyEChannel.Ch0 : CyEChannel.Ch1).ToString();
                m_params.SetValue(CyParamNames.SOURCE_NUMBER + channel, currentControl.Text);
                m_errorProvider.SetError(currentControl, Resource.EPSourceNumberError);
            }
        }

        private void m_numChannelNumber_TextChanged(object sender, EventArgs e)
        {
            Control currentControl = (Control)sender;
            try
            {
                byte value = byte.Parse(currentControl.Text); // Convert value that starts with 0
                m_params.m_channels[m_channelIndex].ChannelNumberCh = (CyEChannelNumberType)CyDictParser.GetDictKey(
                    m_params.m_dnChNumDict, value.ToString());
                m_errorProvider.SetError(currentControl, string.Empty);
            }
            catch
            {
                string channel = ((m_channelIndex == 0) ? CyEChannel.Ch0 : CyEChannel.Ch1).ToString();
                m_params.SetValue(CyParamNames.CHANNEL_NUMBER + channel, currentControl.Text);
                m_errorProvider.SetError(currentControl, Resource.EPChannelNumberError);
            }
        }

        private void numericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            // Do not allow to enter negative value
            if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)
            {
                m_minusEntered = true;
            }
            else
            {
                m_minusEntered = false;
            }
        }

        private void numericUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = m_minusEntered;
        }
        #endregion
    }
}
