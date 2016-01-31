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
    public partial class CyDetailsDevice : UserControl
    {
        const byte CLASS_NONE_ITEM = 0;
        const byte CLASS_CDC_ITEM = 100;
        const byte CLASS_VENDORSPEC_ITEM = 1;

        public DeviceDescriptor Descriptor;
        public USBFSParameters Parameters;
        private bool InternalChanges = false;

        public CyDetailsDevice(DeviceDescriptor descriptor, USBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            InitFields();
        }

        private void InitFields()
        {
            InternalChanges = true;
            FillStrings();
            numUpDownVID.Value = Descriptor.idVendor;
            numUpDownPID.Value = Descriptor.idProduct;
            numUpDownRelease.Value = Descriptor.bcdDevice;

            // Manufacturer string
            if (Descriptor.iwManufacturer > 0)
            {
                string configStrKey = DescriptorNode.GetKeyByIndex(Descriptor.iwManufacturer);
                DescriptorNode node = Parameters.StringTree.GetNodeByKey(configStrKey);
                if (node != null)
                {
                    comboBoxManufacter.SelectedItem = node.Value;
                }
                //if (Descriptor.iwManufacturer <= comboBoxManufacter.Items.Count)
                //    comboBoxManufacter.SelectedIndex = (int)(Descriptor.iwManufacturer - 1);
            }
            else
            {
                comboBoxManufacter.Text = "";
            }

            // Product string
            if (Descriptor.iwProduct > 0)
            {
                string configStrKey = DescriptorNode.GetKeyByIndex(Descriptor.iwProduct);
                DescriptorNode node = Parameters.StringTree.GetNodeByKey(configStrKey);
                if (node != null)
                {
                    comboBoxProductString.SelectedItem = node.Value;
                }
                //if (Descriptor.iwProduct <= comboBoxProductString.Items.Count)
                //    comboBoxProductString.SelectedIndex = (int)(Descriptor.iwProduct - 1);
            }
            else
            {
                comboBoxProductString.Text = "";
            }

            // Serial number string
            if (Descriptor.iSerialNumber > 0)
            {
                string configStrKey = "Serial";
                DescriptorNode node = Parameters.StringTree.GetNodeByKey(configStrKey);
                if (node != null)
                {
                    comboBoxSerial.SelectedItem = node.Value;
                }
            }
            else
            {
                comboBoxSerial.Text = "";
            }

            // Class
            switch (Descriptor.bDeviceClass)
            {
                case USBFSParameters.CLASS_NONE:
                    comboBoxDeviceClass.SelectedIndex = CLASS_NONE_ITEM;
                    break;
                case USBFSParameters.CLASS_CDC:
                    comboBoxDeviceClass.SelectedIndex = CLASS_CDC_ITEM;
                    break;
                case USBFSParameters.CLASS_VENDORSPEC:
                    comboBoxDeviceClass.SelectedIndex = CLASS_VENDORSPEC_ITEM;
                    break;
                default:
                    comboBoxDeviceClass.SelectedIndex = CLASS_NONE_ITEM;
                    break;
            }
            // Subclass
            switch (Descriptor.bDeviceSubClass)
            {
                default:
                    comboBoxDeviceSubclass.SelectedIndex = 0;
                    break;
            }
            InternalChanges = false;
        }

        private void FillStrings()
        {
            comboBoxManufacter.Items.Clear();
            comboBoxProductString.Items.Clear();
            comboBoxSerial.Items.Clear();
            for (int i = 1; i < Parameters.StringTree.Nodes[0].Nodes.Count; i++)
            {
                StringDescriptor strDesc = (StringDescriptor)Parameters.StringTree.Nodes[0].Nodes[i].Value;
                if (strDesc.bString != null)
                {
                    comboBoxManufacter.Items.Add(strDesc);
                    comboBoxProductString.Items.Add(strDesc);
                }
            }
            if (Parameters.StringTree.Nodes[1].Nodes.Count > 0)
            {
                StringDescriptor strDesc = (StringDescriptor)Parameters.StringTree.Nodes[1].Nodes[0].Value;
                if (strDesc.bString != null)
                    comboBoxSerial.Items.Add(strDesc);
            }
        }

        private void comboBoxManufacter_DropDown(object sender, EventArgs e)
        {
            //FillStrings();
        }

        private void comboBoxSerial_DropDown(object sender, EventArgs e)
        {
            //FillStrings();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Descriptor.idVendor = Convert.ToUInt16(numUpDownVID.Value);
            Descriptor.idProduct = Convert.ToUInt16(numUpDownPID.Value);
            switch (comboBoxDeviceClass.SelectedIndex)
            {
                case CLASS_NONE_ITEM:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_NONE;
                    break;
                case CLASS_CDC_ITEM:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_CDC;
                    break;
                case CLASS_VENDORSPEC_ITEM:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_VENDORSPEC;
                    break;
                default:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_NONE;
                    break;
            }
            Descriptor.bDeviceSubClass = 0; // temp

            Descriptor.iwManufacturer = USBFSParameters.SaveStringDescriptor(comboBoxManufacter, Parameters);
            Descriptor.iwProduct = USBFSParameters.SaveStringDescriptor(comboBoxProductString, Parameters);
            Descriptor.iSerialNumber = USBFSParameters.SaveSpecialStringDescriptor(comboBoxSerial, "Serial", Parameters);

        }

        private void CyDetailsDevice_SizeChanged(object sender, EventArgs e)
        {
            buttonApply.Left = (Width - buttonApply.Width) / 2;
        }

        #region Validation

        private void textBoxVID_Validated(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Descriptor.idVendor != Convert.ToUInt16(textBoxVID.Text, 16))
            //    {
            //        Descriptor.idVendor = Convert.ToUInt16(textBoxVID.Text, 16);
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    if ((textBoxVID.Text == "") && (Descriptor.idVendor != 0))
            //    {
            //        Descriptor.idVendor = 0;
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
        }

        private void textBoxPID_Validated(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Descriptor.idProduct != Convert.ToUInt16(textBoxPID.Text, 16))
            //    {
            //        Descriptor.idProduct = Convert.ToUInt16(textBoxPID.Text, 16);
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    if ((textBoxPID.Text == "") && (Descriptor.idProduct != 0))
            //    {
            //        Descriptor.idProduct = 0;
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
        }

        private void comboBoxDeviceClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxDeviceClass.SelectedIndex)
            {
                case CLASS_NONE_ITEM:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_NONE;
                    break;
                case CLASS_CDC_ITEM:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_CDC;
                    break;
                case CLASS_VENDORSPEC_ITEM:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_VENDORSPEC;
                    break;
                default:
                    Descriptor.bDeviceClass = USBFSParameters.CLASS_NONE;
                    break;
            }
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxDeviceSubclass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxManufacter_Validated(object sender, EventArgs e)
        {
            Descriptor.iwManufacturer = USBFSParameters.SaveStringDescriptor(comboBoxManufacter, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxProductString_Validated(object sender, EventArgs e)
        {
            Descriptor.iwProduct = USBFSParameters.SaveStringDescriptor(comboBoxProductString, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxSerial_Validated(object sender, EventArgs e)
        {
            Descriptor.iSerialNumber = USBFSParameters.SaveSpecialStringDescriptor(comboBoxSerial, "Serial", Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void textBoxRelease_Validated(object sender, EventArgs e)
        {
            //try
            //{
            //    Descriptor.bcdDevice = Convert.ToUInt16(textBoxRelease.Text, 16);
            //    if (!InternalChanges)
            //        Parameters.ParamsChanged = true;
            //}
            //catch (Exception)
            //{
            //    if (textBoxRelease.Text == "")
            //    {
            //        Descriptor.bcdDevice = 0;
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
        }

        private void textBoxVID_KeyPress(object sender, KeyPressEventArgs e)
        {
            const int MAX_LENGTH = 4;
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if (((e.KeyChar < '0') || (e.KeyChar > '9')) && ((e.KeyChar < 'A') || (e.KeyChar > 'F')) && ((e.KeyChar < 'a') || (e.KeyChar > 'f')))
                    e.Handled = true;
                if ((((TextBox)sender).Text.Length >= MAX_LENGTH) && (((TextBox)sender).SelectionLength == 0))
                    e.Handled = true;
            }
        }
        
        private void numUpDownVID_Validated(object sender, EventArgs e)
        {
            if (Descriptor.idVendor != Convert.ToUInt16(numUpDownVID.Value))
            {
                Descriptor.idVendor = Convert.ToUInt16(numUpDownVID.Value);
                if (!InternalChanges)
                    Parameters.ParamsChanged = true;
            }
        }

        private void numUpDownPID_Validated(object sender, EventArgs e)
        {
            if (Descriptor.idProduct != Convert.ToUInt16(numUpDownPID.Value))
            {
                Descriptor.idProduct = Convert.ToUInt16(numUpDownPID.Value);
                if (!InternalChanges)
                    Parameters.ParamsChanged = true;
            }
        }

        private void numUpDownRelease_Validated(object sender, EventArgs e)
        {
            if (Descriptor.bcdDevice != Convert.ToUInt16(numUpDownRelease.Value))
            {
                Descriptor.bcdDevice = Convert.ToUInt16(numUpDownRelease.Value);
                if (!InternalChanges)
                    Parameters.ParamsChanged = true;
            }
        }
        #endregion Validation

    }
}
