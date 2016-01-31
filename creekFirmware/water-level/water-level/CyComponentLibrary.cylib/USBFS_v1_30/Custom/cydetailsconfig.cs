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
    public partial class CyDetailsConfig : UserControl
    {
        public ConfigDescriptor Descriptor;
        public CyUSBFSParameters Parameters;
        private bool InternalChanges = false;
        public CyDetailsConfig(ConfigDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            InitFields();
        }

        private void InitFields()
        {
            InternalChanges = true;
            comboBoxConfigString.Text = "";
            numUpDownMaxPower.Value = (Descriptor.bMaxPower * 2);
            comboBoxDevicePower.SelectedIndex = (Descriptor.bmAttributes & 0x40) == 0 ? 0 : 1;
            comboBoxRemoteWakeup.SelectedIndex = (Descriptor.bmAttributes & 0x20) == 0 ? 0 : 1;

            // Configuration string
            FillConfigStrings();

            if (Descriptor.iwConfiguration > 0)
            {
                string configStrKey = DescriptorNode.GetKeyByIndex(Descriptor.iwConfiguration);
                DescriptorNode node = Parameters.StringTree.GetNodeByKey(configStrKey);
                if (node != null)
                {
                    comboBoxConfigString.SelectedItem = node.Value;
                }
            }
            else
            {
                comboBoxConfigString.Text = "";
            }
            InternalChanges = false;
        }

        private void comboBoxConfigString_DropDown(object sender, EventArgs e)
        {
            FillConfigStrings();
        }

        private void FillConfigStrings()
        {
            comboBoxConfigString.Items.Clear();
            for (int i = 1; i < Parameters.StringTree.Nodes[0].Nodes.Count; i++)
            {
                StringDescriptor strDesc = (StringDescriptor)Parameters.StringTree.Nodes[0].Nodes[i].Value;
                if (strDesc.bString != null)
                    comboBoxConfigString.Items.Add(strDesc);
            }
        }

        #region Validation

        private void comboBoxConfigString_Validated(object sender, EventArgs e)
        {
            Descriptor.iwConfiguration = CyUSBFSParameters.SaveStringDescriptor(comboBoxConfigString, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxDevicePower_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBoxDevicePower.SelectedIndex >= 0) && (comboBoxRemoteWakeup.SelectedIndex >= 0))
            {
                byte devicePower = (byte) comboBoxDevicePower.SelectedIndex;
                byte remoteWakeup = (byte) comboBoxRemoteWakeup.SelectedIndex;
                Descriptor.bmAttributes = (byte) (0x80 | (devicePower << 6) | (remoteWakeup << 5));
                if (!InternalChanges)
                    Parameters.ParamsChanged = true;
            }
        }

        private void numUpDownMaxPower_Validated(object sender, EventArgs e)
        {
            if (Descriptor.bMaxPower != (byte)(Convert.ToUInt16(numUpDownMaxPower.Value) / 2))
            {
                Descriptor.bMaxPower = (byte)(Convert.ToUInt16(numUpDownMaxPower.Value) / 2);
                if (!InternalChanges)
                    Parameters.ParamsChanged = true;
            }
        }
        #endregion Validation
    }
}
