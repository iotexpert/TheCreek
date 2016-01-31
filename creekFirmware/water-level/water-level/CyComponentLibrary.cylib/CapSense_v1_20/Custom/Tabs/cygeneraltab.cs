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

namespace  CapSense_v1_20
{

    [Serializable()]
    public partial class CyGeneralTab : CyMyICyParamEditTemplate
    {
        CyGeneralParams m_packParams;
        List<CyGenPropertyUnit> m_listProps = new List<CyGenPropertyUnit>();

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
            cntCCSPropsL.TabIndex = 10;
            cntCCSPropsR.TabIndex = 20;

            lRightHalf.Text = "Right " + CyGeneralParams.m_strABHead;

            foreach (CyGenPropertyUnit item in m_listProps)
            {
                item.cbCSMSide.SelectedIndexChanged += new EventHandler(ActCSMethodChange);
            }
            UpdateCurrentControl();
        }
        #endregion

        #region Actions
        private void rbConfiguration_CheckedChanged(object sender, EventArgs e)
        { 
            if (m_packParams.GlobalEditMode)
            {
                if (rbParallel.Checked)
                {
                    if ((cntCCSPropsL.m_CurrComponent.m_Method != E_CAPSENSE_MODE.None)
                        && (cntCCSPropsR.m_CurrComponent.m_Method != E_CAPSENSE_MODE.None))
                    {
                        if (cntCCSPropsL.SelectedIndex != cntCCSPropsR.SelectedIndex)
                        {
                            MessageBox.Show("Right AMUX Bus Scan Slot CapSense Method will change. " +
                                "\n" + CyIntMessages.STR_CONFMODE,
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cntCCSPropsR.SelectedIndex = cntCCSPropsL.SelectedIndex;
                        }
                    }
                }
            }
            LoadPropertiesToGeneralParams();
            UpdateCurrentControl();
        }
        void ActCSMethodChange(object sender, EventArgs e)
        {
            if (m_packParams.GlobalEditMode)
            {
            //Limitations for Paralel Mode            
                if (rbParallel.Checked)
                {
                    {
                        if (cntCCSPropsL.SelectedIndex != cntCCSPropsR.SelectedIndex)
                        {
                            MessageBox.Show("Configuration Mode will change to Parallel Asynchronous. " +
                                "\n" +CyIntMessages.STR_CONFMODE,
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            rbParallelAsynch.Checked = true;
                        }
                    }
                }
                LoadPropertiesToGeneralParams();
            }
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
                    m_listProps[i].GetProperties(m_packParams.m_localParams.m_listCsHalfs[i], m_packParams);
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