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

namespace  CapSense_v0_5
{

    [Serializable()]
    public partial class CyGeneralTab : M_ICyParamEditTemplate
    {
        CyGeneralParams packParams;
        List<cntGenCSProps> listProps = new List<cntGenCSProps>();

        bool LoadingObject = false;

        #region Load
        public CyGeneralTab(CyGeneralParams packParams)
            : base()
        {
            InitializeComponent();
            //Initialization PackParams
            this.packParams = packParams;

            //Initialization BackGround
            listProps.Add(cntCCSPropsL);
            listProps.Add(cntCCSPropsR);
            lLeftHalf.BackColor = CyGeneralParams.ColorLeft;
            lRightHalf.BackColor = CyGeneralParams.ColorRight;

            lRightHalf.Text = "Right " + CyGeneralParams.strABHead;

            foreach (cntGenCSProps item in listProps)
            {
                item.cbCSMSide.SelectedIndexChanged += new EventHandler(actCSMethodChange);
            }
            UpdateCurrentControl();
        }
        #endregion

        #region Actions
        private void rbConfiguration_CheckedChanged(object sender, EventArgs e)
        {   
            if (rbParallel.Checked)
            {
                if ((cntCCSPropsL.CurrComponent.Method != eCapSenseMode.None) && (cntCCSPropsR.CurrComponent.Method != eCapSenseMode.None))
                {
                    if (cntCCSPropsL.SelectedIndex != cntCCSPropsR.SelectedIndex)
                    {
                        MessageBox.Show("Right AMUX Bus Scan Slot CapSense Method will change. \nBecause You cann't work in Parallel Synchronized Mode with different CapSense Methods.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cntCCSPropsR.SelectedIndex = cntCCSPropsL.SelectedIndex;
                    }
                }
            }
            LoadPropertiesToGeneralParams();
            UpdateCurrentControl();
        }
        void actCSMethodChange(object sender, EventArgs e)
        {
            //Block CSA Mode
            foreach (cntGenCSProps item in listProps)
            {
                if (item.CapSenseMode == eCapSenseMode.CSA)
                {
                    MessageBox.Show("You can not have \"CSA\" mode.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    item.SelectedIndex = 0;
                }
            }

            //Limitations for Paralel Mode
            if (rbParallel.Checked)
            {
                {
                    if (cntCCSPropsL.SelectedIndex != cntCCSPropsR.SelectedIndex)
                    {
                        MessageBox.Show("Configuration Mode will change to Parallel Asynchronous. \nBecause You cann't work in Parallel Synchronized Mode with different CapSense Methods.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        rbParallelAsynch.Checked = true;
                    }
                }
            }
            if (rbParallelAsynch.Checked)
            {

                if ((cntCCSPropsL.CapSenseMode == eCapSenseMode.None) && (cntCCSPropsR.CapSenseMode == eCapSenseMode.None))
                {
                    MessageBox.Show("You cann't have \"None\" valuefor both CapSense parts.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cntCCSPropsL.SelectedIndex = 0;
                }
            }
            LoadPropertiesToGeneralParams();
        }
        public void cyGeneralTab_Leave(object sender, EventArgs e)
        {
            LoadPropertiesToGeneralParams();
        }
        #endregion

        #region Functions
        void UpdateCurrentControl()
        {
            lLeftHalf.Text = packParams.GetLeftLabel();
            if (packParams.Configuration == eMConfiguration.emSerial)
                tableLayoutMain.ColumnStyles[1].Width = 0;
            else
                tableLayoutMain.ColumnStyles[1].Width = 50;
            //cyGeneralTab_SizeChanged(null, null);
        }
        void LoadPropertiesToGeneralParams()
        {
            if (!LoadingObject)
            {
                if (rbSerial.Checked) packParams.Configuration = eMConfiguration.emSerial;
                if (rbParallel.Checked) packParams.Configuration = eMConfiguration.emParallelSynchron;
                if (rbParallelAsynch.Checked) packParams.Configuration = eMConfiguration.emParallelAsynchron;
                for (int i = 0; i < listProps.Count; i++)
                {
                    listProps[i].SendProperties(packParams.localParams.listCsHalfs[i]);
                }
                packParams.localParams.DoEventGlobalParamsChange();
            }
        }
        public void LoadFormGeneralParams()
        {
            LoadingObject = true;
            switch (packParams.Configuration)
            {
                case eMConfiguration.emSerial:
                    rbSerial.Checked = true;
                    break;
                case eMConfiguration.emParallelSynchron:
                    rbParallel.Checked = true;
                    break;
                case eMConfiguration.emParallelAsynchron:
                    rbParallelAsynch.Checked = true;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < listProps.Count; i++)
            {
                listProps[i].GetProperties(packParams.localParams.listCsHalfs[i],packParams);
            }
            LoadingObject = false;

            packParams.localParams.DoEventGlobalParamsChange();
        }
        #endregion
    }
}