/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace  CapSense_v1_10
{
    public partial class CyClockSwitch : CyMyICyParamEditTemplate
    {
        public CyCntClkSwitch m_cntClkL;
        public CyCntClkSwitch m_cntClkR;

        CyGeneralParams m_packParams;
        public CyClockSwitch(CyGeneralParams packParams)
            :base()
        {
            InitializeComponent();
            this.m_packParams = packParams;
            tableLayoutMain.Dock = DockStyle.Fill;
            tableLayoutMain.Margin = new Padding(0);
            lLeftHalf.BackColor = CyGeneralParams.m_ColorLeft;
            lRightHalf.BackColor = CyGeneralParams.m_ColorRight;

            lRightHalf.Text = "Right " + CyGeneralParams.m_strABHead;
            lLeftHalf.Text = CyGeneralParams.m_strABHead;

            //Adding  cntClk
            this.m_cntClkL = new  CapSense_v1_10.CyCntClkSwitch();
            this.m_cntClkR = new  CapSense_v1_10.CyCntClkSwitch();            
            this.tableLayoutMain.Controls.Add(this.m_cntClkL, 0, 1);            
            this.tableLayoutMain.Controls.Add(this.m_cntClkR, 1, 1);
            this.m_cntClkL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_cntClkR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_cntClkL.Margin = new Padding(0);
            this.m_cntClkR.Margin = new Padding(0);
            packParams.m_localParams.m_actGlobalParamsChange += 
                new CyLocalParams.m_NullDelegate(LocalParams_actChangeHalfVisibility);
        }

        void LocalParams_actChangeHalfVisibility()
        {
            for (int i = 0; i < m_packParams.m_localParams.m_listCsHalfs.Count; i++)
            {
                tableLayoutMain.ColumnStyles[i].SizeType = SizeType.Percent;
                bool vis = m_packParams.m_localParams.BCsHalfIsEnable(m_packParams.m_localParams.m_listCsHalfs[i]);

                if (vis)
                    tableLayoutMain.ColumnStyles[i].Width = 50;
                else
                    tableLayoutMain.ColumnStyles[i].Width = 0;
            }
                        
            lLeftHalf.Text = m_packParams.GetLeftLabel();
            m_cntClkL.SetImageTopbGraph();
            m_cntClkR.SetImageTopbGraph();
        }
    }
}

