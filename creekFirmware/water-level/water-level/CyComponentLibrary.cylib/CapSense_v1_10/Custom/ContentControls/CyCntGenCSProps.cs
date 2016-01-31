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

namespace CapSense_v1_10
{
    public partial class CyCntGenCSProps : UserControl
    {
        List<CyCntGenCSBaseProps> listComponents = new List<CyCntGenCSBaseProps>();

        public CyCntGenCSBaseProps CurrComponent = null;

        public int SelectedIndex
        {
            get { return cbCSMSide.SelectedIndex; }
            set { cbCSMSide.SelectedIndex = value; }
        }
        public E_CAPSENSE_MODE CapSenseMode
        {
            get { return CyStrCapSenseMode.GetEnum(CyCBConverter.CyGetValue(cbCSMSide)); }
            //set { cbCSMSide.SelectedIndex = value; }
        }
        public CyCntGenCSProps()
        {
            InitializeComponent();

            listComponents.Add(new CyCntGenCSAProps());
            listComponents.Add(new CyCntGenCSDProps());
            int itop = panelTop.Height;
            foreach (CyCntGenCSBaseProps item in listComponents)
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
            CyCBConverter.CySetValue(cbCSMSide, CyStrCapSenseMode.GetStr(cCSHalf.m_Method));
            CurrComponent.GetProperties(cCSHalf, packParam);
        }
        public void SendProperties(CyAmuxBParams cCSHalf)
        {
            cCSHalf.m_Method = CapSenseMode;
            CurrComponent.SendProperties(cCSHalf);
        }

        public void cbCSMSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (CyCntGenCSBaseProps item in listComponents)
            {
                item.Visible = false;
            }
            foreach (CyCntGenCSBaseProps item in listComponents)
                if (CyStrCapSenseMode.GetEnum(CyCBConverter.CyGetValue(cbCSMSide)) == item.m_Method)
                {
                    CyControlFunc.cyShow(item);
                    CurrComponent = item;
                    break;
                }
        }

    }

    public class CyCntGenCSBaseProps : UserControl
    {
        protected CyAmuxBParams m_cCSHalf = null;
        protected CyGeneralParams m_packParam = null;

        public E_CAPSENSE_MODE m_Method = E_CAPSENSE_MODE.None;
        public CyCntGenCSBaseProps()
        { }
        public virtual void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam) { }
        public virtual void SendProperties(CyAmuxBParams cCSHalf) { }
    }
    public static class CyControlFunc
    {
        public static void cyShow(Control cnt)
        {
            cnt.Visible = true;
            cnt.Dock = DockStyle.Fill;
            cnt.BringToFront();
        }
    }

}
