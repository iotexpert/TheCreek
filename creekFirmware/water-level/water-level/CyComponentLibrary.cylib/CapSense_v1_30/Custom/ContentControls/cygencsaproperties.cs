/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace CapSense_v1_30
{
    public partial class CyGenCSAProperties : CyGenBaseProperties
    {
        public CyGenCSAProperties()
        {
            InitializeComponent();
            m_Method = E_CAPSENSE_MODE.CSA;            
            pbGraph.SizeMode = PictureBoxSizeMode.Zoom;

            CyGeneralParams.AssingActions(panelMain, new EventHandler(SendProperties));
        }

        public override void SendProperties(object sender, EventArgs e)
        {
            if (m_packParam != null)
                if ((m_cCSHalf != null) && (m_packParam.GlobalEditMode))
                {
                    m_cCSHalf.m_cShieldElectrode.Validate(tbSEC.Text);

                    GraphRePaint();
                    m_packParam.SetCommitParams(null, null);
                }
        }
        public override void GetProperties(CyAmuxBParams m_cCSHalf, CyGeneralParams m_packParam)
        {
            this.m_cCSHalf = null;            
            tbSEC.Text = m_cCSHalf.CountShieldElectrode.ToString();

            this.m_cCSHalf = m_cCSHalf;
            this.m_packParam = m_packParam;
            GraphRePaint();
        }
        void GraphRePaint()
        {
            string strStart = "";
            if (m_cCSHalf != null)
            {
                strStart = m_packParam.GetSidePrefix(m_cCSHalf);
                if (m_packParam.Configuration == E_MAIN_CONFIG.emParallelSynchron) strStart = "l";
                pbGraph.Image = ((System.Drawing.Image)(global:: CapSense_v1_30.Properties.Resources.ResourceManager.
        GetObject(strStart + "idacsourcing")));
            }
        }
    }
}
