/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace CapSense_CSD_v2_0
{
    public partial class CyTuningTab : CyCSParamEditTemplate
    {
        public override string TabName
        {
            get { return "Tune Helper"; }
        }
        public CyTuningTab(CyCSParameters packParams)
            : base()
        {
            InitializeComponent();
            m_packParams = packParams;
            CyCSParameters.AssingActions(this, new EventHandler(SendProperties));
            packParams.m_updateAll += new EventHandler(GetProperties);
            cbEnableTuneHelper.CheckedChanged += new EventHandler(cbEnableTuneHelper_CheckedChanged);
        }

        void cbEnableTuneHelper_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control item in this.Controls)
                if (item != cbEnableTuneHelper)
                    item.Enabled = cbEnableTuneHelper.Checked;
        }
        public void GetProperties(object sender, EventArgs e)
        {
            if (m_packParams == null) return;
            cbEnableTuneHelper.Checked = m_packParams.m_settings.m_enableTuneHelper;
            tbEZI2CInstaceName.Text = m_packParams.m_settings.m_ezI2CInstanceName;
        }
        void SendProperties(object sender, EventArgs e)
        {
            if (m_packParams == null || CyCSParameters.GLOBAL_EDIT_MODE == false) return;
            CyCSSettings settings = m_packParams.m_settings;
            settings.m_enableTuneHelper = cbEnableTuneHelper.Checked;
            settings.m_ezI2CInstanceName = tbEZI2CInstaceName.Text;
            m_packParams.SetCommitParams(null, null);            
        }

        private void tbEZI2CInstaceName_TextChanged(object sender, EventArgs e)
        {
            if (CyWidgetsList.NameTest(tbEZI2CInstaceName.Text) == false)
            {
                errorProvider.SetError(tbEZI2CInstaceName, CyCsResource.InvalidNameError);
            }
            else
                errorProvider.SetError(tbEZI2CInstaceName, string.Empty);

        }

        private void tbEZI2CInstaceName_Validating(object sender, CancelEventArgs e)
        {
            string errMsg = errorProvider.GetError(tbEZI2CInstaceName);
            e.Cancel = (string.IsNullOrEmpty(errMsg) == false);
        }
    }
}
