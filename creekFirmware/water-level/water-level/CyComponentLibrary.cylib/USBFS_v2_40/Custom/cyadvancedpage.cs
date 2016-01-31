/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace USBFS_v2_40
{
    public partial class CyAdvancedPage : UserControl
    {
        public CyUSBFSParameters m_parameters; 

        public CyAdvancedPage()
        {
            InitializeComponent();
        }

        public CyAdvancedPage(CyUSBFSParameters parameters)
        {
            InitializeComponent();
            m_parameters = parameters;
            InitFields();
        }

        public void InitFields()
        {
            checkBoxExtClass.Checked = m_parameters.Extern_cls;
            checkBoxExtVendor.Checked = m_parameters.Extern_vnd;
            checkBoxVBusMon.Checked = m_parameters.Mon_vbus;
            checkBoxSofOutput.Checked = m_parameters.Out_sof;
        }

        private void checkBoxExtClass_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == checkBoxExtClass)
            {
                m_parameters.Extern_cls = ((CheckBox)sender).Checked;
            }
            else if (sender == checkBoxExtVendor)
            {
                m_parameters.Extern_vnd = ((CheckBox)sender).Checked;
            }
            else if (sender == checkBoxVBusMon)
            {
                m_parameters.Mon_vbus = ((CheckBox)sender).Checked;
            }
            else if (sender == checkBoxSofOutput)
            {
                m_parameters.Out_sof = ((CheckBox)sender).Checked;
            }
        }
    }
}
