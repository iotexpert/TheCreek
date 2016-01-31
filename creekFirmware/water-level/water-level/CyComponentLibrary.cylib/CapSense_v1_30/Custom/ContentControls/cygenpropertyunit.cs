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

namespace CapSense_v1_30
{
    public partial class CyGenPropertyUnit : UserControl
    {
        List<CyGenBaseProperties> m_listComponents = new List<CyGenBaseProperties>();
        CyAmuxBParams m_CSHalf = null;

        public CyGenBaseProperties m_CurrComponent = null;

        public int SelectedIndex
        {
            get { return cbCSMSide.SelectedIndex; }
            set { cbCSMSide.SelectedIndex = value; }
        }
        public E_CAPSENSE_MODE CurrentMode
        {
            get { return CyStrCapSenseMode.GetEnum(CyIntConverter.CyGetValue(cbCSMSide)); }
        }

        public CyGenPropertyUnit()
        {
            InitializeComponent();

            //Block CSA Mode
            cbCSMSide.Enabled = false;

            m_listComponents.Add(new CyGenCSAProperties());
            m_listComponents.Add(new CyGenCSDProperties());
            int itop = panelTop.Height;
            foreach (CyGenBaseProperties item in m_listComponents)
            {
                panelContainer.Controls.Add(item);
                item.Top = itop;
                itop += item.Top;
                item.Visible = false;
            }
            cbCSMSide.Items.AddRange(CyStrCapSenseMode.m_strCMode);
            cbCSMSide.SelectedIndex = 0;
            cbCSMSide_SelectedIndexChanged(null, null);
        }

        public void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam)
        {
            this.m_CSHalf = cCSHalf;
            CyIntConverter.CySetValue(cbCSMSide, CyStrCapSenseMode.GetStr(cCSHalf.m_Method));
            m_CurrComponent.GetProperties(cCSHalf, packParam);
        }

        public void SendProperties(CyAmuxBParams cCSHalf)
        {
            cCSHalf.m_Method = CurrentMode;
            m_CurrComponent.SendProperties(null, null);
        }

        public void cbCSMSide_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Assing component with correct method
            foreach (CyGenBaseProperties item in m_listComponents)
            {
                item.Visible = false;
            }
            foreach (CyGenBaseProperties item in m_listComponents)
                if (CurrentMode == item.m_Method)
                {
                    item.Visible = true;
                    item.Dock = DockStyle.Fill;
                    item.BringToFront();
                    m_CurrComponent = item;
                    break;
                }
            if (m_CSHalf != null)
                m_CSHalf.m_Method = CurrentMode;
        }
    }

    public class CyGenBaseProperties : UserControl
    {
        public CyAmuxBParams m_cCSHalf = null;
        public CyGeneralParams m_packParam = null;
        public E_CAPSENSE_MODE m_Method = E_CAPSENSE_MODE.None;

        public CyGenBaseProperties()  { }
        public virtual void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam) { }
        public virtual void SendProperties(object sender, EventArgs e) { }
    }
}
