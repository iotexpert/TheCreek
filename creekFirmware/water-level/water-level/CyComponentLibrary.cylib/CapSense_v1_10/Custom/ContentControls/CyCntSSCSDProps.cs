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

namespace  CapSense_v1_10.ContentControls
{
    public partial class CyCntSSCSDProps : CyCntSSCSProps
    {
        public CyCntSSCSDProps()
            :base()
        {
            InitializeComponent();

            cbIDACrange.SelectedIndex = 0;
            cbResolution.SelectedIndex = 0;
            cblScanSpeed.SelectedIndex = 0;
            this.cbCIS.Items.Clear();
            this.cbCIS.Items.AddRange(CySSProperties.m_listCIS);
            cbCIS.SelectedIndex = 0;
            foreach (Control item in panelMain.Controls)
            {
                if ((item.GetType() == typeof(ComboBox))||(item.GetType() == typeof(TextBox)))
                {
                    item.Validated += new EventHandler(SendProperties);
                }
            }
            m_isGeneration = true;
        }

        public override void GetProperties(CyAmuxBParams cch)
        {
            m_currectCch = cch;
            if (cch != null)
            {
                m_isGeneration = true;
                //Idac Visible
                bool vis = cch.m_isIdac;
                m_isIdac = vis;
                lIdacR.Visible = vis;
                lIdadS.Visible = vis;
                cbIDACrange.Visible = vis;
                cbIDACSetting.Visible = vis;
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
                    SetValue(ref m_listLastObj[i].m_baseProps.m_Resolution, cbResolution.SelectedIndex);
                    SetValue(ref m_listLastObj[i].m_baseProps.m_ScanSpeed, cblScanSpeed.SelectedIndex);
                    //SetSmProp(ref listLastObj[i].baseProps.RefVal, cbRefValueL.SelectedIndex);
                    SetValue(ref m_listLastObj[i].m_baseProps.m_PrescPer, tbPrescPer);
                    if (m_isIdac)
                    {
                        SetValue(ref m_listLastObj[i].m_IDACRange, cbIDACrange.SelectedIndex);
                        SetValue(ref m_listLastObj[i].m_IDACSettings, cbIDACSetting);
                    }
                    if (cbCIS.SelectedIndex == -1) cbCIS.SelectedIndex = 0;
                    SetValue(ref m_listLastObj[i].m_CIS, cbCIS.SelectedIndex);

                }
                //Save Data
                m_actSaveChanges(null, null);
            }
        }

        public override void ShowInControl()
        {
            if (m_lastObject != null)
            {
                m_isGeneration = true;
                cbResolution.SelectedIndex = m_lastObject.m_baseProps.m_Resolution;
                cblScanSpeed.SelectedIndex = m_lastObject.m_baseProps.m_ScanSpeed;
                //cbRefValueL.SelectedIndex = lastObject.baseProps.RefVal;
                tbPrescPer.Text = GetValue(m_lastObject.m_baseProps.m_PrescPer);
                cbIDACrange.SelectedIndex = m_lastObject.m_IDACRange;
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
