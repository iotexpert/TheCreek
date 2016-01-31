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

namespace  CapSense_v0_5
{
    public partial class cntGenCSAProps : cntGenCSBaseProps
    {
        public cntGenCSAProps()
        {
            InitializeComponent();
            Method = eCapSenseMode.CSA;
        }

        public override void SendProperties(CyAmuxBParams cCSHalf)
        {

        }
        public override void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam)
        {
            //if (Visible)
            {
                cCSHalf.isRbEnable = false;
                cCSHalf.isIdac = false;
                cCSHalf.cShieldElectrode.Validate(0);

            }
        }
    }
}
