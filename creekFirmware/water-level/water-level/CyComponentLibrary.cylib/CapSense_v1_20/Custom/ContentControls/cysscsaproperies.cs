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

namespace  CapSense_v1_20.ContentControls
{
    public partial class CySSCSAProperties : CySSBaseProperties
    {
        public CySSCSAProperties()
            : base()
        {
            InitializeComponent();

            cbIDACRange.SelectedIndex = 0;
            //cbIDACSettingL.SelectedIndex = 0;
            this.cbCIS.Items.Clear();
            this.cbCIS.Items.AddRange(CySSProperties.m_listCIS);
            cbCIS.SelectedIndex = 0;
            CyGeneralParams.AssingActions(panelMain, new EventHandler(SendProperties));
            m_isGeneration = true;
        }
        public override void SetItemsVisibility(CyAmuxBParams cch)
        {
            m_currectCch = cch;
            if (cch != null)
            {
                m_isGeneration = true;
                tbPrescPer.Enabled = cch.IsPrescaler();
                lPrescPer.Enabled = cch.IsPrescaler();

                //CIS change values range
                this.cbCIS.Items.Clear();
                this.cbCIS.Items.AddRange(CySSProperties.m_listCIS);
                if (cch.IsNotShieldElectrode())
                    this.cbCIS.Items.RemoveAt(CySSProperties.m_removeAtShieldNone);

                m_isGeneration = false;
                if (cbCIS.SelectedIndex == -1) cbCIS.SelectedIndex = 0;
            }
        }
        public void SendProperties(object sender, EventArgs e)
        {
            if (!m_isGeneration)
            {
                for (int i = 0; i < m_listLastObj.Count; i++)
                {
                    SetValue(ref m_listLastObj[i].m_baseProps.m_Scanlength, cbScanLength);
                    SetValue(ref m_listLastObj[i].m_baseProps.m_SettlingTime, tbSettlingTime);
                    SetValue(ref m_listLastObj[i].m_baseProps.m_PrescPer, tbPrescPer);
                    if (m_isIdac)
                    {
                        SetValue(ref m_listLastObj[i].m_IDACRange, cbIDACRange.SelectedIndex);
                        SetValue(ref m_listLastObj[i].m_IDACSettings, cbIDACSetting);
                    }
                    if (cbCIS.SelectedIndex == -1) cbCIS.SelectedIndex = 0;
                    SetValue(ref m_listLastObj[i].m_CIS, cbCIS.SelectedIndex);
                }
                m_actSaveChanges(null, null);
            }
        }

        public override void ShowData()
        {
            if (m_lastObject != null)
            {
                m_isGeneration = true;
                cbScanLength.Text = GetValue(m_lastObject.m_baseProps.m_Scanlength);
                tbSettlingTime.Text = GetValue(m_lastObject.m_baseProps.m_SettlingTime);
                tbPrescPer.Text = GetValue(m_lastObject.m_baseProps.m_PrescPer);
                cbIDACRange.SelectedIndex = m_lastObject.m_IDACRange;
                cbIDACSetting.Text = GetValue(m_lastObject.m_IDACSettings);

                //CIS work
                if (m_currectCch.IsNotShieldElectrode())
                {
                    if (m_lastObject.m_CIS == CySSProperties.m_removeAtShieldNone) m_lastObject.m_CIS = 0;
                }
                cbCIS.SelectedIndex = Convert.ToInt16(m_lastObject.m_CIS);

                m_isGeneration = false;
            }
        }
    }
}
