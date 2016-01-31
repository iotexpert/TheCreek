/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;

namespace USBFS_v1_30
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
            switch (Descriptor.snType)
            {
                case StringGenerationType.USER_ENTERED_TEXT:
                    radioButtonUser.Checked = true;
                    break;
                case StringGenerationType.USER_CALL_BACK:
                    radioButtonCallback.Checked = true;
                    break;
                case StringGenerationType.SILICON_NUMBER:
                    radioButtonSilicon.Checked = true;
                    break;
                default:
                    radioButtonUser.Checked = true;
                    break;
            }
        }

        public void SetTextBoxDefault()
        {
            textBoxString.Enabled = false;
        }

        private void CyDetailsString_Load(object sender, EventArgs e)
        {
            textBoxString.Text = Descriptor.bString;
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
                Descriptor.snType = StringGenerationType.USER_ENTERED_TEXT;
            }
            else if (sender == radioButtonCallback)
            {
                textBoxString.Enabled = false;
                Descriptor.snType = StringGenerationType.USER_CALL_BACK;
            }
            else if (sender == radioButtonSilicon)
            {
                textBoxString.Enabled = false;
                Descriptor.snType = StringGenerationType.SILICON_NUMBER;
            }
            Parameters.ParamsChanged = true;
        }
    }
}
