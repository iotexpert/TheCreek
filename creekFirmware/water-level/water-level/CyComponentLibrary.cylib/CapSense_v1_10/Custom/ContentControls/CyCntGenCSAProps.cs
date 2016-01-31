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

namespace  CapSense_v1_10
{
    public partial class CyCntGenCSAProps : CyCntGenCSBaseProps
    {
        public CyCntGenCSAProps()
        {
            InitializeComponent();
            m_Method = E_CAPSENSE_MODE.CSA;
        }

        public override void SendProperties(CyAmuxBParams cCSHalf)
        {
            //Save Data
            if (m_packParam != null)
                m_packParam.SetCommitParams(null, null);
        }
        public override void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam)
        {
            //if (Visible)
            {
                cCSHalf.m_isRbEnable = false;
                cCSHalf.m_isIdac = false;
                cCSHalf.m_cShieldElectrode.Validate(0);

            }
        }
    }
}
