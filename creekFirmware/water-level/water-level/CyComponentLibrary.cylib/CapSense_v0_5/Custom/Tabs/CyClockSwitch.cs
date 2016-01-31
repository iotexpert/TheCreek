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

namespace  CapSense_v0_5
{
    public partial class CyClockSwitch : M_ICyParamEditTemplate
    {
        public cntClkSwitch cntClkL;
        public cntClkSwitch cntClkR;

        CyGeneralParams packParams;
        public CyClockSwitch(CyGeneralParams packParams)
            :base()
        {
            InitializeComponent();
            this.packParams = packParams;
            tableLayoutMain.Dock = DockStyle.Fill;
            tableLayoutMain.Margin = new Padding(0);
            lLeftHalf.BackColor = CyGeneralParams.ColorLeft;
            lRightHalf.BackColor = CyGeneralParams.ColorRight;

            lRightHalf.Text = "Right " + CyGeneralParams.strABHead;
            lLeftHalf.Text = CyGeneralParams.strABHead;

            //Adding  cntClk
            this.cntClkL = new  CapSense_v0_5.cntClkSwitch();
            this.cntClkR = new  CapSense_v0_5.cntClkSwitch();            
            this.tableLayoutMain.Controls.Add(this.cntClkL, 0, 1);            
            this.tableLayoutMain.Controls.Add(this.cntClkR, 1, 1);
            this.cntClkL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntClkR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cntClkL.Margin = new Padding(0);
            this.cntClkR.Margin = new Padding(0);
            packParams.localParams.actGlobalParamsChange += new CyLocalParams.m_NullDelegate(localParams_actChangeHalfVisibility);
        }

        void localParams_actChangeHalfVisibility()
        {
            //pLeft.Visible = packParams.localParams.bCsHalfIsEnable(packParams.localParams.listCsHalfs[0]);
            //pRight.Visible = packParams.localParams.bCsHalfIsEnable(packParams.localParams.listCsHalfs[1]);
            for (int i = 0; i < packParams.localParams.listCsHalfs.Count; i++)
            {
                tableLayoutMain.ColumnStyles[i].SizeType = SizeType.Percent;
                bool vis = packParams.localParams.bCsHalfIsEnable(packParams.localParams.listCsHalfs[i]);
                //if (packParams.localParams.listCsHalfs[i].side == eElSide.Right)
                //{
                //    if (packParams.Configuration == eMConfiguration.emParallelSynchron) vis = false;
                //}

                if (vis)
                    tableLayoutMain.ColumnStyles[i].Width = 50;
                else
                    tableLayoutMain.ColumnStyles[i].Width = 0;
            }
            //cntClkR.SetImageTopbGraph();
                        
            lLeftHalf.Text = packParams.GetLeftLabel();
            //if (packParams.Configuration == eMConfiguration.emParallelSynchron) lLeftHalf.Text = "";
            cntClkL.SetImageTopbGraph();
            cntClkR.SetImageTopbGraph();
        }
    }
}

