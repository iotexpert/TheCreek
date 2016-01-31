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
    public partial class CyChannel0StatusTab : CyEditingWrapperControl
    {
        public override string TabName
        {
            get { return CyCustomizer.CHANNEL_0_STATUS_TAB_NAME; }
        }

        private CyChannelControl m_channelControl;

        #region Constructor(s)
        public CyChannel0StatusTab(CySPDifTxParameters inst)
        {
            inst.m_channel0StatusTab = this;

            InitializeComponent();

            m_params = inst;
            m_channelControl = new CyChannelControl(m_params, CyEChannel.Ch0);
            Controls.Add(m_channelControl);
            m_channelControl.Dock = DockStyle.Top;
            m_channelControl.Visible = true;
            m_channelControl.BringToFront();

            // Filling comboboxes
            m_cbFrequency.DataSource = m_params.m_frequencyList;
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateUI()
        {
            m_cbFrequency.Text = CyDictParser.GetDictValue(m_params.m_dnDict, m_params.Frequency);
            m_channelControl.UpdateUI();
        }
        #endregion

        #region Event Handlers
        private void m_cbFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_params.Frequency = (CyESampleFrequencyType)CyDictParser.GetDictKey(m_params.m_dnDict, m_cbFrequency.Text);
        }
        #endregion
    }
}
