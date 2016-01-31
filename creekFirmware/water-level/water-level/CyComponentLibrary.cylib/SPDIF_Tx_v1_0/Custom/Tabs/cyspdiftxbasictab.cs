/*******************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace SPDIF_Tx_v1_0
{
    public partial class CySPDifTxBasicTab : CyEditingWrapperControl
    {
        public override string TabName
        {
            get { return CyCustomizer.GENERAL_TAB_NAME; }
        }

        #region Constructor(s)
        public CySPDifTxBasicTab(CySPDifTxParameters inst)
        {
            inst.m_basicTab = this;

            InitializeComponent();
            m_params = inst;
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateUI()
        {
            // DataBits
            m_cbAudioDataLength.Text = m_params.DataBits.ToString();
            // DataInterleaving
            if (m_params.DataInterleaving == true) m_rbInterleaved.Checked = true;
            else m_rbSeparated.Checked = true;
            // ManagedDma
            m_chbxManagedDma.Checked = m_params.ManagedDma;
            UpdateChannelStatusTabsEnabling();
        }
        #endregion

        #region Event handlers
        private void m_cbAudioDataLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                m_params.DataBits = Convert.ToByte(m_cbAudioDataLength.Text);
            }
            catch
            {
                System.Diagnostics.Debug.Assert(false);
            }
        }

        private void m_rbInterleaved_CheckedChanged(object sender, EventArgs e)
        {
            m_params.DataInterleaving = m_rbInterleaved.Checked;
        }

        private void m_chbxManagedDma_CheckedChanged(object sender, EventArgs e)
        {
            m_params.ManagedDma = m_chbxManagedDma.Checked;
            UpdateChannelStatusTabsEnabling();
        }

        private void UpdateChannelStatusTabsEnabling()
        {
            m_params.m_channel0StatusTab.Enabled = m_chbxManagedDma.Checked;
            m_params.m_channel1StatusTab.Enabled = m_chbxManagedDma.Checked;
        }
        #endregion
    }
}
