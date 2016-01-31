/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace  CapSense_v1_30.ContentControls
{
    public partial class CySSCSAProperties : CySSBaseProperties
    {
        public CySSCSAProperties()
            : base()
        {
            m_Method = E_CAPSENSE_MODE.CSA;
            InitializeComponent();

            cbIDACRange.SelectedIndex = 0;
            this.cbCIS.Items.Clear();
            this.cbCIS.Items.AddRange(CySSProperties.m_listCIS);
            cbCIS.SelectedIndex = 0;
            CyGeneralParams.AssingActions(panelMain, new EventHandler(SendProperties));
            m_Locked = true;
        }
        public override void GetProperties(CyAmuxBParams cch)
        {
            m_amuxparam = cch;
            if (cch != null)
            {
                m_Locked = true;
                tbPrescPer.Enabled = cch.IsPrescaler();
                lPrescPer.Enabled = cch.IsPrescaler();

                //CIS change values range
                this.cbCIS.Items.Clear();
                this.cbCIS.Items.AddRange(CySSProperties.m_listCIS);
                if (cch.IsNotShieldElectrode())
                    this.cbCIS.Items.RemoveAt(CySSProperties.m_removeAtShieldNone);

                m_Locked = false;
                if (cbCIS.SelectedIndex == -1) cbCIS.SelectedIndex = 0;
            }
        }
        public void SendProperties(object sender, EventArgs e)
        {
            if (!m_Locked)
            {
                for (int i = 0; i < m_listProperties.Count; i++)
                {
                    CyIntConverter.SetValue(ref m_listProperties[i].m_baseProps.m_Scanlength, cbScanLength);
                    CyIntConverter.SetValue(ref m_listProperties[i].m_baseProps.m_SettlingTime, tbSettlingTime);
                    CyIntConverter.SetValue(ref m_listProperties[i].m_baseProps.m_PrescPer, tbPrescPer);
                    if (m_isIdac)
                    {
                        CyIntConverter.SetValue(ref m_listProperties[i].m_IDACRange, cbIDACRange.SelectedIndex);
                        CyIntConverter.SetValue(ref m_listProperties[i].m_IDACSettings, cbIDACSetting);
                    }
                    if (cbCIS.SelectedIndex == -1) cbCIS.SelectedIndex = 0;
                    CyIntConverter.SetValue(ref m_listProperties[i].m_CIS, cbCIS.SelectedIndex);
                }
                m_actSaveChanges(null, null);
            }
        }

        public override void UpdateForm()
        {
            if (m_CombinedProperty != null)
            {
                m_Locked = true;
                cbScanLength.Text = CyIntConverter.GetValue(m_CombinedProperty.m_baseProps.m_Scanlength);
                tbSettlingTime.Text = CyIntConverter.GetValue(m_CombinedProperty.m_baseProps.m_SettlingTime);
                tbPrescPer.Text = CyIntConverter.GetValue(m_CombinedProperty.m_baseProps.m_PrescPer);
                cbIDACRange.SelectedIndex = m_CombinedProperty.m_IDACRange;
                cbIDACSetting.Text = CyIntConverter.GetValue(m_CombinedProperty.m_IDACSettings);

                //CIS work
                if (m_amuxparam.IsNotShieldElectrode())
                {
                    if (m_CombinedProperty.m_CIS == CySSProperties.m_removeAtShieldNone) m_CombinedProperty.m_CIS = 0;
                }
                cbCIS.SelectedIndex = Convert.ToInt16(m_CombinedProperty.m_CIS);

                m_Locked = false;
            }
        }
    }
}
