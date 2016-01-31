/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace  CapSense_v0_5
{
    public partial class cntGenCSProps : UserControl
    {
        List<cntGenCSBaseProps> listComponents = new List<cntGenCSBaseProps>();

        public cntGenCSBaseProps CurrComponent = null;

        public int SelectedIndex 
        {
            get { return cbCSMSide.SelectedIndex; }
            set { cbCSMSide.SelectedIndex=value; }
        }
        public eCapSenseMode CapSenseMode
        {
            get { return strCapSenseMode.GetEnum(cyCBConverter.cyGetValue(cbCSMSide)); }
            //set { cbCSMSide.SelectedIndex = value; }
        }
        public cntGenCSProps()
        {
            InitializeComponent();

             listComponents.Add(new cntGenCSAProps());
            listComponents.Add(new cntGenCSDProps());
            int itop = panelTop.Height;
            foreach (cntGenCSBaseProps item in listComponents)
            {
                panelContainer.Controls.Add(item);
                item.Top = itop;
                itop += item.Top;
                item.Visible = false;
            }

            cbCSMSide.Items.AddRange(strCapSenseMode.strCMode);
            cbCSMSide.SelectedIndex = 0;
            cbCSMSide_SelectedIndexChanged(null, null);

 
        }
        public void GetProperties(CyAmuxBParams cCSHalf,CyGeneralParams packParam)
        {
            cyCBConverter.cySetValue(cbCSMSide,strCapSenseMode.GetStr(cCSHalf.Method));
            CurrComponent.GetProperties(cCSHalf, packParam);
        }
        public void SendProperties(CyAmuxBParams cCSHalf)
        {
            cCSHalf.Method=CapSenseMode;
            CurrComponent.SendProperties(cCSHalf);
        }

        public void cbCSMSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (cntGenCSBaseProps item in listComponents)
            {
                item.Visible = false;
            }
            foreach (cntGenCSBaseProps item in listComponents)
                if (strCapSenseMode.GetEnum(cyCBConverter.cyGetValue( cbCSMSide)) == item.Method)
                {
                   ControlFunc.cyShow(item);
                    CurrComponent = item;
                    break;
                }
        }
        
    }

    public  class cntGenCSBaseProps : UserControl
    {
        public eCapSenseMode Method = eCapSenseMode.None;
        public cntGenCSBaseProps()
        {   }
        public virtual void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam) { }
        public virtual void SendProperties(CyAmuxBParams cCSHalf) { }
    }
    public static class ControlFunc
{
        public static void cyShow(Control cnt)
        {
            cnt.Visible = true;
            cnt.Dock = DockStyle.Fill;
            cnt.BringToFront();
        }
}

}
