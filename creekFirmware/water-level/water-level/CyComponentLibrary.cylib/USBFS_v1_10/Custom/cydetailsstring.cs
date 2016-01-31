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

namespace USBFS_v1_10
{
    public partial class CyDetailsString : UserControl
    {
        public StringDescriptor Descriptor;
        private CyUSBFSParameters Parameters;
        public delegate void UpdateNodeDelegate();
        public event UpdateNodeDelegate UpdateNodeEvent;

        public CyDetailsString(StringDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
        }

        public void SetGroupBoxVisible()
        {
            groupBoxOption.Visible = true;
        }

        private void CyDetailsString_Load(object sender, EventArgs e)
        {
            textBoxString.Text = Descriptor.bString;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Descriptor.bString = textBoxString.Text;
            Descriptor.bLength = (byte)(Descriptor.bString.Length + 2);
        }

        private void CyDetailsString_SizeChanged(object sender, EventArgs e)
        {
            buttonApply.Left = (Width - buttonApply.Width) / 2;
        }

        private void textBoxString_Validated(object sender, EventArgs e)
        {
            Descriptor.bString = textBoxString.Text;
            Descriptor.bLength = (byte)(Descriptor.bString.Length + 2);
            Parameters.ParamsChanged = true;
            UpdateNodeEvent();
        }

        private void radioButtonUser_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;

            if (sender == radioButtonUser)
            {
                textBoxString.Enabled = true;
            }
            else if (sender == radioButtonCallback)
            {
                textBoxString.Enabled = false;
            }
            else if (sender == radioButtonSilicon)
            {
                textBoxString.Enabled = false;
            }
            Parameters.ParamsChanged = true;
        }

        private void textBoxString_EnabledChanged(object sender, EventArgs e)
        {
            if (textBoxString.Enabled)
            {
                Descriptor.bString = textBoxString.Text;
            }
            else
            {
                Descriptor.bString = "";
            }
            Descriptor.bLength = (byte)(Descriptor.bString.Length + 2);
        }
    }
}
