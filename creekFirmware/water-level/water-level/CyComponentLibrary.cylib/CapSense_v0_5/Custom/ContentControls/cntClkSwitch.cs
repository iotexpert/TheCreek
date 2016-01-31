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
    public partial class cntClkSwitch : UserControl
    {
        bool Loading = false;
        CyAmuxBParams currentCSHalf = null;
        public CyGeneralParams PackParam = null;
        List<cyClkItem> listStClk = new List<cyClkItem>();
        public cntClkSwitch()
        {
            Loading = true;
            InitializeComponent();
            cbPrescaler.Items.Clear();
            cbPrescaler.Items.AddRange(Enum.GetNames(typeof(enPrescaler)));
            cbPrescaler.SelectedIndex = 0;
            listStClk.Add(new cyClkItem(12));
            listStClk.Add(new cyClkItem(24));
            listStClk.Add(new cyClkItem(48));
            listStClk.Add(new cyClkItem(92));            
            this.Load += new EventHandler(cntClkSwitch_Load);
            Loading = false;
        }

        void cntClkSwitch_Load(object sender, EventArgs e)
        {
            SetImageTopbGraph();
        }
        public void SendProperties()
        {
            if (!Loading)
            {
                if (currentCSHalf != null)
                {
                    //if (PackParam.Configuration == eMConfiguration.emParallelSynchron)
                    //{
                    //    foreach (CyAmuxBParams item in PackParam.localParams.listCsHalfs)
                    //        item.Prescaler = (enPrescaler)Enum.Parse(typeof(enPrescaler), cyCBConverter.cyGetValue(cbPrescaler));
                    //}
                    //else
                    {
                        currentCSHalf.Prescaler = (enPrescaler)Enum.Parse(typeof(enPrescaler), cyCBConverter.cyGetValue(cbPrescaler));
                    }
                    currentCSHalf.currectClk = (cyClkItem)cyCBConverter.cyGetObject(cbCPS_CLK);
                }
            }
        }
        public void GetProperties(CyAmuxBParams cCSHalf, CyGeneralParams packParam)
        {
            Loading = true;
            currentCSHalf = cCSHalf;
            this.PackParam = packParam;

            #region Loading Clk Data
            if (packParam.edit != null)
            {
                List<string> listClkStr;
                if (packParam.edit.DesignQuery != null)
                    listClkStr = new List<string>(packParam.edit.DesignQuery.ClockIDs);
                else
                    listClkStr = new List<string>();
                cbCPS_CLK.Items.Clear();
                foreach (string item in listClkStr)
                {
                    if (packParam.edit.DesignQuery.IsClockLocal(item)) continue;
                    cyClkItem ClkItem = new cyClkItem();
                    byte exp = 0;
                    ClkItem.ClockID = item;
                    ClkItem.ClockName = packParam.edit.DesignQuery.GetClockName(item);
                    packParam.edit.DesignQuery.GetClockActualFreq(item, out ClkItem.ActualFreq, out exp);
                    if (ClkItem.ActualFreq == 0) continue;
                    cbCPS_CLK.Items.Add(ClkItem);

                }
            }
            foreach (cyClkItem item in listStClk)
            {
                cbCPS_CLK.Items.Add(item);
            }
            cbCPS_CLK.SelectedIndex = 0;
            #endregion

            //Chose current clok
            if (cCSHalf.currectClk != null)
            {
                foreach (object item in cbCPS_CLK.Items)
                    if (((cyClkItem)item).IsSame(cCSHalf.currectClk))
                    {
                        cyCBConverter.cySetValue(cbCPS_CLK, item.ToString());
                        break;
                    }                
                cyCBConverter.cySetValue(cbPrescaler,currentCSHalf.Prescaler.ToString());
            }
            Loading = false;

            //For correct settings
            SendProperties();
            SetImageTopbGraph();
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            SendProperties();
        }

        private void cbPrescaler_SelectedIndexChanged(object sender, EventArgs e)
        {
            SendProperties();            
            //Prescaler Property( for ScanSlot) Visible Change
            if (!Loading)
            {
                if (PackParam != null)
                    PackParam.localParams.DoEventGlobalParamsChange();
                SetImageTopbGraph();
            }

            
        }
        public void SetImageTopbGraph()
        {
            string strStart = "";
            strStart = PackParam.GetPrefixForParams(currentCSHalf);
            if (PackParam.Configuration == eMConfiguration.emParallelSynchron)
            {
                strStart = "l";
            }
            bool vis = currentCSHalf.side == eElSide.Left;
            vis = vis || PackParam.Configuration != eMConfiguration.emParallelSynchron;
            lCPS_CLK.Visible = vis;
            cbCPS_CLK.Visible = vis;
            switch (currentCSHalf.Prescaler)
            {
                case enPrescaler.UDB:
                    pbGraph.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject(strStart + "PreScalerUDB")));
                    break;
                case enPrescaler.HW:
                    pbGraph.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject(strStart + "PreScalerHW")));
                    break;
                case enPrescaler.None:
                    pbGraph.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject(strStart + "PreScalerNone")));
                    break;
                default:
                    break;
            }

        }
    }
 

        [Serializable()]
    public class cyClkItem
    {
        public string ClockID = "";
        public double ActualFreq = 0;
        public string ClockName = "";

        public cyClkItem()
        {
        }
        public cyClkItem(double ActualFreq)
        {
            this.ActualFreq = ActualFreq;
        }
        public override string ToString()
        {
            string str = "";
            if (ClockName != "") str = ClockName + " : ";
            return str + ActualFreq + "MHz";
        }
        public bool IsSame(cyClkItem item)
        {
            if (item.ActualFreq != ActualFreq) return false;
            if (item.ClockName != ClockName) return false;
            if (item.ClockID !=ClockID ) return false;            
            return true;
        }
    }
}
