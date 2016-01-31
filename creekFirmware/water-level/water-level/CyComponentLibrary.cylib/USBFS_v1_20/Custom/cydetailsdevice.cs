/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;

namespace USBFS_v1_20
{
    public partial class CyDetailsDevice : UserControl
    {
        private const byte CLASS_NONE_ITEM = 0;
        private const byte CLASS_CDC_ITEM = 100;
        private const byte CLASS_VENDORSPEC_ITEM = 1;

        public DeviceDescriptor Descriptor;
        public CyUSBFSParameters Parameters;
        private bool InternalChanges = false;

        public CyDetailsDevice(DeviceDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            InitFields();
            rbManual.CheckedChanged += new EventHandler(rbMemoryMgmt_CheckedChanged);
            rbDMAManual.CheckedChanged += new EventHandler(rbMemoryMgmt_CheckedChanged);
            rbDMAAutomatic.CheckedChanged += new EventHandler(rbMemoryMgmt_CheckedChanged);
        }

        private void rbMemoryMgmt_CheckedChanged(object sender, EventArgs e)
        {
            byte res = 0;
            if (rbDMAManual == sender) res = 1;
            if (rbDMAAutomatic == sender) res = 2;
            if (!InternalChanges)
            {
                if (((RadioButton) sender).Checked) Descriptor.bMemoryMgmt = res;
                Parameters.ParamsChanged = true;
            }
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
                case CyUSBFSParameters.CLASS_NONE:
                    comboBoxDeviceClass.SelectedIndex = CLASS_NONE_ITEM;
                    break;
                case CyUSBFSParameters.CLASS_CDC:
                    comboBoxDeviceClass.SelectedIndex = CLASS_CDC_ITEM;
                    break;
                case CyUSBFSParameters.CLASS_VENDORSPEC:
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

            // Memory management
            switch (Descriptor.bMemoryMgmt)
            {
                case 0:
                    rbManual.Checked = true;
                    break;
                case 1:
                    rbDMAManual.Checked = true;
                    break;
                case 2:
                    rbDMAAutomatic.Checked = true;
                    break;
                default:
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
                StringDescriptor strDesc = (StringDescriptor) Parameters.StringTree.Nodes[0].Nodes[i].Value;
                if (strDesc.bString != null)
                {
                    comboBoxManufacter.Items.Add(strDesc);
                    comboBoxProductString.Items.Add(strDesc);
                }
            }
            if (Parameters.StringTree.Nodes[1].Nodes.Count > 0)
            {
                StringDescriptor strDesc = (StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[0].Value;
                if (strDesc.bString != null)
                    comboBoxSerial.Items.Add(strDesc);
            }
        }

        #region Validation

        private void comboBoxDeviceClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxDeviceClass.SelectedIndex)
            {
                case CLASS_NONE_ITEM:
                    Descriptor.bDeviceClass = CyUSBFSParameters.CLASS_NONE;
                    break;
                case CLASS_CDC_ITEM:
                    Descriptor.bDeviceClass = CyUSBFSParameters.CLASS_CDC;
                    break;
                case CLASS_VENDORSPEC_ITEM:
                    Descriptor.bDeviceClass = CyUSBFSParameters.CLASS_VENDORSPEC;
                    break;
                default:
                    Descriptor.bDeviceClass = CyUSBFSParameters.CLASS_NONE;
                    break;
            }
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxManufacter_Validated(object sender, EventArgs e)
        {
            Descriptor.iwManufacturer = CyUSBFSParameters.SaveStringDescriptor(comboBoxManufacter, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxProductString_Validated(object sender, EventArgs e)
        {
            Descriptor.iwProduct = CyUSBFSParameters.SaveStringDescriptor(comboBoxProductString, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxSerial_Validated(object sender, EventArgs e)
        {
            Descriptor.iSerialNumber = CyUSBFSParameters.SaveSpecialStringDescriptor(comboBoxSerial, "Serial",
                                                                                     Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
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