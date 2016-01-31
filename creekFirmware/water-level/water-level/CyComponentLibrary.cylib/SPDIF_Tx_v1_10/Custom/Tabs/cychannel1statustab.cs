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
    public partial class CyChannel1StatusTab : CyEditingWrapperControl
    {
        public override string TabName
        {
            get { return CyCustomizer.CHANNEL_1_STATUS_TAB_NAME; }
        }

        private CyChannelControl m_channelControl;

        #region Constructor(s)
        public CyChannel1StatusTab(CySPDifTxParameters inst)
        {
            inst.m_channel1StatusTab = this;

            InitializeComponent();

            m_params = inst;
            m_channelControl = new CyChannelControl(m_params, CyEChannel.Ch1);
            Controls.Add(m_channelControl);
            m_channelControl.Dock = DockStyle.Top;
            m_channelControl.Visible = true;
            m_channelControl.BringToFront();
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateUI()
        {
            m_chbxCopyDefaults.Checked = m_params.CopyDefaults;
            m_channelControl.UpdateUI();
        }
        #endregion

        #region Event Handlers
        private void m_chbxCopyDefaults_CheckedChanged(object sender, EventArgs e)
        {
            m_channelControl.FirstGroupBoxEnabled = !m_chbxCopyDefaults.Checked;
            m_params.CopyDefaults = m_chbxCopyDefaults.Checked;
            if (m_chbxCopyDefaults.Checked)
            {
                m_params.m_channels[1].DataTypeCh = m_params.m_channels[0].DataTypeCh;
                m_params.m_channels[1].CopyrightCh = m_params.m_channels[0].CopyrightCh;
                m_params.m_channels[1].PreEmphasisCh = m_params.m_channels[0].PreEmphasisCh;
                m_params.m_channels[1].CategoryCh = m_params.m_channels[0].CategoryCh;
                m_params.m_channels[1].ClockAccuracyCh = m_params.m_channels[0].ClockAccuracyCh;
                m_channelControl.UpdateUI();
            }
        }

        private void CyChannel1StatusTab_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && m_chbxCopyDefaults.Checked)
            {
                m_channelControl.UpdateUI();
            }
        }
        #endregion
    }
}
