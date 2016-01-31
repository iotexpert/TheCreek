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
    public partial class CyDetailsLangID : UserControl
    {
        public StringZeroDescriptor Descriptor;
        private CyUSBFSParameters Parameters;

        public CyDetailsLangID(StringZeroDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            for (int i = 0; i < CyUSBFSParameters.LangIDNames.Length; i++)
            {
                comboBoxLangID.Items.Add(CyUSBFSParameters.LangIDNames[i]);
            }
        }

        private void CyDetailsLangID_Load(object sender, EventArgs e)
        {
            int index = Array.IndexOf(CyUSBFSParameters.LangIDs, Descriptor.wLANGID);
            comboBoxLangID.Text = comboBoxLangID.Items[index].ToString();
        }

        private void comboBoxLangID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBoxLangID.SelectedIndex >= 0) &&
                (Descriptor.wLANGID != CyUSBFSParameters.LangIDs[comboBoxLangID.SelectedIndex]))
            {
                Descriptor.wLANGID = CyUSBFSParameters.LangIDs[comboBoxLangID.SelectedIndex];
                Parameters.ParamsChanged = true;
            }
        }
    }
}