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
                //if (Descriptor.iwConfiguration <= comboBoxConfigString.Items.Count)
                //    comboBoxConfigString.SelectedIndex = (int)Descriptor.iwConfiguration - 1;
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

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Descriptor.bMaxPower = (byte)(Convert.ToUInt16(numUpDownMaxPower.Value) / 2);
            byte devicePower = (byte)comboBoxDevicePower.SelectedIndex;
            byte remoteWakeup = (byte) comboBoxRemoteWakeup.SelectedIndex;
            Descriptor.bmAttributes = (byte)(0x80 | (devicePower << 6) | (remoteWakeup << 5));

            //Configuration string
            Descriptor.iwConfiguration = CyUSBFSParameters.SaveStringDescriptor(comboBoxConfigString, Parameters);
        }

        private void CyDetailsConfig_SizeChanged(object sender, EventArgs e)
        {
            buttonApply.Left = (Width - buttonApply.Width)/2;
        }

        #region Validation

        private void comboBoxConfigString_Validated(object sender, EventArgs e)
        {
            Descriptor.iwConfiguration = CyUSBFSParameters.SaveStringDescriptor(comboBoxConfigString, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void textBoxMaxPower_Validating(object sender, CancelEventArgs e)
        {
            //try
            //{
            //    if (textBoxMaxPower.Text != "")
            //    {
            //        byte res = Convert.ToByte(textBoxMaxPower.Text);
            //        if (res > 255) throw new Exception();
            //    }
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Invalid Max Power Value (must be 0-255)", "USBFS", MessageBoxButtons.OK,
            //                    MessageBoxIcon.Warning);
            //    e.Cancel = true;
            //}
        }

        private void textBoxMaxPower_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))
                    e.Handled = true;
            }
        }

        private void textBoxMaxPower_Validated(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Descriptor.bMaxPower != (byte)(Convert.ToUInt16(textBoxMaxPower.Text) / 2))
            //    {
            //        Descriptor.bMaxPower = (byte) (Convert.ToUInt16(textBoxMaxPower.Text)/2);
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    if ((textBoxMaxPower.Text == "") && (Descriptor.bMaxPower != 0))
            //    {
            //        Descriptor.bMaxPower = 0;
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
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
