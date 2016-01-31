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
using System.Text;
using System.Windows.Forms;

namespace CapSense_CSD_v2_0
{
    public partial class CySSCSDProperties : UserControl
    {
        private CyCSParameters m_packParams = null;
        private CyScanSlot m_combinedSS;
        private List<CyScanSlot> m_listSS = new List<CyScanSlot>();
        private bool m_locked = true;
        public EventHandler m_actSaveChanges;

        public CySSCSDProperties()
            : base()
        {
            InitializeComponent();
            CyCSParameters.AssingActions(this, new EventHandler(SendProperties));           
        }
        #region General Function
        public void ClearSSProperties()
        {
            m_listSS.Clear();
        }

        public void AddSSProperties(CyScanSlot addObject)
        {
            m_listSS.Add(addObject);
        }

        public void CombineSSProperties()
        {
            //Compare objects
            m_combinedSS = (CyScanSlot)CyClassPropsComparer.Comapare(m_listSS.ToArray());
            UpdateForm();
        }        

        public void GetProperties(CyCSParameters packParams)
        {
            m_packParams = packParams;
            if (m_packParams != null)
            {
                //Idac Visible
                bool visible = m_packParams.m_settings.IsIdacInSystem();
                cbIDACSetting.Visible = visible;
                cbIDACSetting.Visible = visible;
                packParams.m_updateAll += new EventHandler(UpdateVisibility);
                packParams.m_settings.m_configurationChanged += new EventHandler(UpdateVisibility);
            }
        }

        public void SendProperties(object sender, EventArgs e)
        {
            if (m_packParams != null)
                if (m_locked == false)
                {
                    for (int i = 0; i < m_listSS.Count; i++)
                    {
                        if (m_packParams.m_settings.IsIdacInSystem())
                        {
                            if (CyCsConst.C_IDAC_SETTINGS.CheckRange(cbIDACSetting.Text))
                                m_listSS[i].m_idacSettings = int.Parse(cbIDACSetting.Text);
                        }
                        if (m_packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_Auto)
                        {
                            if (CyCsConst.C_SENSITIVITY.CheckRange(cbSensitivity.Text))
                                m_listSS[i].m_sensitivity = int.Parse(cbSensitivity.Text);
                        }
                    }
                    //Save Data
                    m_actSaveChanges(null, null);
                }
        }

        #endregion

        private void UpdateForm()
        {
            if (m_packParams != null && m_combinedSS != null)
            {
                m_locked = true;
                cbIDACSetting.Text = m_combinedSS.m_idacSettings == -1 ? "" : m_combinedSS.m_idacSettings.ToString();
                if (m_combinedSS.m_sensitivity == -1)
                    cbSensitivity.SelectedIndex = -1;
                else
                    cbSensitivity.Text = m_combinedSS.m_sensitivity.ToString();
                m_locked = false;
            }
        }
        private void UpdateVisibility(object sender, EventArgs e)
        {
            if (m_packParams != null)
            {
                cbSensitivity.Visible = m_packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_Auto;
                lSensitivity.Visible = m_packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_Auto;
            }
        }

        private void tbIDACSetting_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ex = new CancelEventArgs();
            tbIDACSetting_Validating(sender, ex);
        }

        private void tbIDACSetting_Validating(object sender, CancelEventArgs e)
        {
            double min_val = CyCsConst.C_IDAC_SETTINGS.m_min;
            double max_val = CyCsConst.C_IDAC_SETTINGS.m_max;
            try
            {
                double val = Double.Parse(cbIDACSetting.Text);
                if (val > max_val || val < min_val) throw new Exception();
                errorProvider.SetError((Control)sender, "");
            }
            catch
            {
                errorProvider.SetError((Control)sender, String.Format(CyCsResource.ValueLitation,
    min_val, max_val));
                e.Cancel = true;
            }
        }
    }
}
