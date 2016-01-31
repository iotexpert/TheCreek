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

namespace USBFS_v0_2
{
    public partial class CyDetailsLangID : UserControl
    {
        public StringZeroDescriptor Descriptor;
        private USBFSParameters Parameters;

        public CyDetailsLangID(StringZeroDescriptor descriptor, USBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            for (int i = 0; i < USBFSParameters.LangIDNames.Length; i++)
            {
                comboBoxLangID.Items.Add(USBFSParameters.LangIDNames[i]);
            }
        }

        private void CyDetailsLangID_Load(object sender, EventArgs e)
        {
            int index = Array.IndexOf(USBFSParameters.LangIDs, Descriptor.wLANGID);
            comboBoxLangID.Text = comboBoxLangID.Items[index].ToString();
        }

        private void comboBoxLangID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBoxLangID.SelectedIndex >= 0) && (Descriptor.wLANGID != USBFSParameters.LangIDs[comboBoxLangID.SelectedIndex]))
            {
                Descriptor.wLANGID = USBFSParameters.LangIDs[comboBoxLangID.SelectedIndex];
                Parameters.ParamsChanged = true;
            }
        }       
    }
}
