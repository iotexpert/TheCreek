/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace USBFS_v2_0
{
    public partial class CyDetailsEPMngt : UserControl
    {
        public CyUSBFSParameters m_parameters;

        public CyDetailsEPMngt(CyUSBFSParameters parameters)
        {
            InitializeComponent();
            m_parameters = parameters;
            InitFields();

            rbManual.CheckedChanged += new EventHandler(rbMemoryMgmt_CheckedChanged);
            rbDMAManual.CheckedChanged += new EventHandler(rbMemoryMgmt_CheckedChanged);
            rbDMAAutomatic.CheckedChanged += new EventHandler(rbMemoryMgmt_CheckedChanged);
            rbStaticAllocation.CheckedChanged += new EventHandler(rbStaticAllocation_CheckedChanged);
            rbDynamicAllocation.CheckedChanged += new EventHandler(rbStaticAllocation_CheckedChanged);
        }

        private void InitFields()
        {
            // Memory management
            switch (m_parameters.EPMemoryMgmt)
            {
                case (byte)CyUSBFSParameters.CyMemoryManagement.MANUAL:
                    rbManual.Checked = true;
                    break;
                case (byte)CyUSBFSParameters.CyMemoryManagement.DMA_MANUAL_MEMORY:
                    rbDMAManual.Checked = true;
                    break;
                case (byte)CyUSBFSParameters.CyMemoryManagement.DMA_AUTOMATIC_MEMORY:
                    rbDMAAutomatic.Checked = true;
                    break;
                default:
                    break;
            }  

            // Memory allocation
            switch (m_parameters.EPMemoryAlloc)
            {
                case (byte)CyUSBFSParameters.CyMemoryAllocation.STATIC:
                    rbStaticAllocation.Checked = true;
                    break;
                case (byte)CyUSBFSParameters.CyMemoryAllocation.DYNAMIC:
                    rbDynamicAllocation.Checked = true;
                    break;
                default:
                    break;
            }
            SetAllocationGroupVisibility();
        }

        private void SetAllocationGroupVisibility()
        {
            if (rbManual.Checked)
            {
                groupBoxAllocation.Enabled = true;
            }
            else if (rbDMAManual.Checked)
            {
                groupBoxAllocation.Visible = true;
                groupBoxAllocation.Enabled = false;
                rbStaticAllocation.Checked = true;
            }
            else if (rbDMAAutomatic.Checked)
            {
                groupBoxAllocation.Enabled = false;
                rbStaticAllocation.Checked = true;
            }
            groupBoxAllocation.Visible = rbDMAAutomatic.Checked == false;
        }

        void rbStaticAllocation_CheckedChanged(object sender, EventArgs e)
        {
            byte res = 0;
            if (rbStaticAllocation == sender) res = (byte)CyUSBFSParameters.CyMemoryAllocation.STATIC;
            if (rbDynamicAllocation == sender) res = (byte)CyUSBFSParameters.CyMemoryAllocation.DYNAMIC;
            
            if (((RadioButton)sender).Checked) m_parameters.EPMemoryAlloc = res;
        }

        private void rbMemoryMgmt_CheckedChanged(object sender, EventArgs e)
        {
            byte res = 0;
            if (rbDMAManual == sender) res = 1;
            if (rbDMAAutomatic == sender) res = 2;
            SetAllocationGroupVisibility();

            if (((RadioButton)sender).Checked) m_parameters.EPMemoryMgmt = res;
        }

    }
}
