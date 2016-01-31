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

namespace  CapSense_v1_10
{

    [Serializable()]
    public partial class CyGeneralTab : CyMyICyParamEditTemplate
    {
        CyGeneralParams m_packParams;
        List<CyCntGenCSProps> m_listProps = new List<CyCntGenCSProps>();

        bool m_LoadingObject = false;

        #region Load
        public CyGeneralTab(CyGeneralParams packParams)
            : base()
        {
            InitializeComponent();
            //Initialization PackParams
            this.m_packParams = packParams;

            //Initialization BackGround
            m_listProps.Add(cntCCSPropsL);
            m_listProps.Add(cntCCSPropsR);
            lLeftHalf.BackColor = CyGeneralParams.m_ColorLeft;
            lRightHalf.BackColor = CyGeneralParams.m_ColorRight;

            lRightHalf.Text = "Right " + CyGeneralParams.m_strABHead;

            foreach (CyCntGenCSProps item in m_listProps)
            {
                item.cbCSMSide.SelectedIndexChanged += new EventHandler(ActCSMethodChange);
            }
            UpdateCurrentControl();
        }
        #endregion

        #region Actions
        private void rbConfiguration_CheckedChanged(object sender, EventArgs e)
        {   
            if (rbParallel.Checked)
            {
                if ((cntCCSPropsL.CurrComponent.m_Method != E_CAPSENSE_MODE.None) 
                    && (cntCCSPropsR.CurrComponent.m_Method != E_CAPSENSE_MODE.None))
                {
                    if (cntCCSPropsL.SelectedIndex != cntCCSPropsR.SelectedIndex)
                    {
                        MessageBox.Show("Right AMUX Bus Scan Slot CapSense Method will change. "+
                            "\nBecause You can't work in Parallel Synchronized Mode with different CapSense Methods.", 
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cntCCSPropsR.SelectedIndex = cntCCSPropsL.SelectedIndex;
                    }
                }
            }
            LoadPropertiesToGeneralParams();
            UpdateCurrentControl();
        }
        void ActCSMethodChange(object sender, EventArgs e)
        {
            //Block CSA Mode
            foreach (CyCntGenCSProps item in m_listProps)
            {
                if (item.CapSenseMode == E_CAPSENSE_MODE.CSA)
                {
                MessageBox.Show(
                "Sorry. But CSA mode is not  suported in this version",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    item.SelectedIndex = 0;
                }
            }

            //Limitations for Paralel Mode
            if (rbParallel.Checked)
            {
                {
                    if (cntCCSPropsL.SelectedIndex != cntCCSPropsR.SelectedIndex)
                    {
                        MessageBox.Show("Configuration Mode will change to Parallel Asynchronous. "+
                            "\nBecause You can't work in Parallel Synchronized Mode with different CapSense Methods.", 
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        rbParallelAsynch.Checked = true;
                    }
                }
            }
            if (rbParallelAsynch.Checked)
            {

                if ((cntCCSPropsL.CapSenseMode == E_CAPSENSE_MODE.None) 
                    && (cntCCSPropsR.CapSenseMode == E_CAPSENSE_MODE.None))
                {
                    MessageBox.Show("You can't have \"None\" value for both CapSense parts.", "Warning", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cntCCSPropsL.SelectedIndex = 0;
                }
            }
            LoadPropertiesToGeneralParams();
        }
        public void CyGeneralTab_Leave(object sender, EventArgs e)
        {
            LoadPropertiesToGeneralParams();
        }
        #endregion

        #region Functions
        void UpdateCurrentControl()
        {
            lLeftHalf.Text = m_packParams.GetLeftLabel();
            if (m_packParams.Configuration == E_MAIN_CONFIG.emSerial)
                tableLayoutMain.ColumnStyles[1].Width = 0;
            else
                tableLayoutMain.ColumnStyles[1].Width = 50;
            //cyGeneralTab_SizeChanged(null, null);
        }
        void LoadPropertiesToGeneralParams()
        {
            if (!m_LoadingObject)
            {
                if (rbSerial.Checked) m_packParams.Configuration = E_MAIN_CONFIG.emSerial;
                if (rbParallel.Checked) m_packParams.Configuration = E_MAIN_CONFIG.emParallelSynchron;
                if (rbParallelAsynch.Checked) m_packParams.Configuration = E_MAIN_CONFIG.emParallelAsynchron;
                for (int i = 0; i < m_listProps.Count; i++)
                {
                    m_listProps[i].SendProperties(m_packParams.m_localParams.m_listCsHalfs[i]);
                }
                m_packParams.m_localParams.DoEventGlobalParamsChange();
            }
        }
        public void LoadFormGeneralParams()
        {
            m_LoadingObject = true;
            switch (m_packParams.Configuration)
            {
                case E_MAIN_CONFIG.emSerial:
                    rbSerial.Checked = true;
                    break;
                case E_MAIN_CONFIG.emParallelSynchron:
                    rbParallel.Checked = true;
                    break;
                case E_MAIN_CONFIG.emParallelAsynchron:
                    rbParallelAsynch.Checked = true;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < m_listProps.Count; i++)
            {
                m_listProps[i].GetProperties(m_packParams.m_localParams.m_listCsHalfs[i],m_packParams);
            }
            m_LoadingObject = false;

            m_packParams.m_localParams.DoEventGlobalParamsChange();
        }
        #endregion
    }
}