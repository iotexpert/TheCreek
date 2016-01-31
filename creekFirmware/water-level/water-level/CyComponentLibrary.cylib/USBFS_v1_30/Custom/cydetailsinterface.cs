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
    public partial class CyDetailsInterface : UserControl
    {
        private const byte CLASS_NONE_ITEM = 0;
        private const byte CLASS_HID_ITEM = 1;
        private const byte CLASS_CDC_ITEM = 100;
        private const byte CLASS_AUDIO_ITEM = 101;
        private const byte CLASS_VENDORSPEC_ITEM = 2;

        private InterfaceDescriptor Descriptor;
        private CyUSBFSParameters Parameters;
        private CyDeviceDescriptor ParentFrm;

        public delegate void RemoveNodeDelegate();

        public event RemoveNodeDelegate RemoveClassNodeEvent;

        private bool InternalChanges = false;

        public CyDetailsInterface(InterfaceDescriptor descriptor, CyUSBFSParameters parameters,
                                  CyDeviceDescriptor parentForm)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            ParentFrm = parentForm;
            InitFields();
        }

        private void InitFields()
        {
            InternalChanges = true;
            FillStrings();
            labelInterfaceNumber.Text = Descriptor.bInterfaceNumber.ToString();
            labelAlternateSettings.Text = Descriptor.bAlternateSetting.ToString();

            switch (Descriptor.bInterfaceClass)
            {
                case CyUSBFSParameters.CLASS_NONE:
                    comboBoxClass.SelectedIndex = CLASS_NONE_ITEM;
                    break;
                case CyUSBFSParameters.CLASS_HID:
                    comboBoxClass.SelectedIndex = CLASS_HID_ITEM;
                    break;
                case CyUSBFSParameters.CLASS_CDC:
                    comboBoxClass.SelectedIndex = CLASS_CDC_ITEM;
                    break;
                case CyUSBFSParameters.CLASS_AUDIO:
                    comboBoxClass.SelectedIndex = CLASS_AUDIO_ITEM;
                    ParentFrm.SetBtnAddClass(1);
                    break;
                case CyUSBFSParameters.CLASS_VENDORSPEC:
                    comboBoxClass.SelectedIndex = CLASS_VENDORSPEC_ITEM;
                    break;
                default:
                    comboBoxClass.SelectedIndex = CLASS_NONE_ITEM;
                    break;
            }

            // Interface string
            if (Descriptor.iwInterface > 0)
            {
                string configStrKey = DescriptorNode.GetKeyByIndex(Descriptor.iwInterface);
                DescriptorNode node = Parameters.StringTree.GetNodeByKey(configStrKey);
                if (node != null)
                {
                    comboBoxInterfaceString.SelectedItem = node.Value;
                }
            }
            else
            {
                comboBoxInterfaceString.Text = "";
            }

            comboBoxSubclass.SelectedIndex = Descriptor.bInterfaceSubClass;
            SetProtocolField();
            comboBoxProtocol.SelectedIndex = Descriptor.bInterfaceProtocol;
            InternalChanges = false;
        }

        private void FillStrings()
        {
            comboBoxInterfaceString.Items.Clear();
            for (int i = 1; i < Parameters.StringTree.Nodes[0].Nodes.Count; i++)
            {
                StringDescriptor strDesc = (StringDescriptor) Parameters.StringTree.Nodes[0].Nodes[i].Value;
                if (strDesc.bString != null)
                {
                    comboBoxInterfaceString.Items.Add(strDesc);
                }
            }
        }

        private void comboBoxClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set Subclasses
            comboBoxSubclass.Items.Clear();
            comboBoxSubclass.Items.Add("No subclass");
            switch (comboBoxClass.SelectedIndex)
            {
                case CLASS_NONE_ITEM:
                    break;
                case CLASS_HID_ITEM:
                    comboBoxSubclass.Items.Add("Boot Interface Subclass");
                    break;
                case CLASS_CDC_ITEM:
                    comboBoxSubclass.Items.Add("Direct Line Control Model");
                    comboBoxSubclass.Items.Add("Abstract Control Model");
                    comboBoxSubclass.Items.Add("Telephone Control Model");
                    comboBoxSubclass.Items.Add("Multi-Channel Control Model");
                    comboBoxSubclass.Items.Add("CAPI Control Model");
                    comboBoxSubclass.Items.Add("Ethernet Networking Control Model");
                    comboBoxSubclass.Items.Add("ATM Networking Control Model");
                    comboBoxSubclass.Items.Add("Wireless Handset Control Model");
                    comboBoxSubclass.Items.Add("Device Management");
                    comboBoxSubclass.Items.Add("Mobile Direct Line Model");
                    comboBoxSubclass.Items.Add("OBEX");
                    comboBoxSubclass.Items.Add("Ethernet Emulation Model");
                    break;
                case CLASS_AUDIO_ITEM:
                    comboBoxSubclass.Items.Add("Audio Control Interface Subclass");
                    comboBoxSubclass.Items.Add("Audio Streaming Interface Subclass");
                    comboBoxSubclass.Items.Add("MIDI Streaming Interface Subclass");
                    break;
                case CLASS_VENDORSPEC_ITEM:
                    break;
                default:
                    break;
            }

            //Save
            bool changed = false;

            // If class was changed from HID or Audio to another, remove class descriptor
            bool needDeleteClass = false;
            if ((Descriptor.bInterfaceClass == CyUSBFSParameters.CLASS_HID) &&
                (comboBoxClass.SelectedIndex != CLASS_HID_ITEM))
            {
                needDeleteClass = true;
            }
            if ((Descriptor.bInterfaceClass == CyUSBFSParameters.CLASS_AUDIO) &&
                (comboBoxClass.SelectedIndex != CLASS_AUDIO_ITEM))
            {
                needDeleteClass = true;
            }
            if ((Descriptor.bInterfaceClass == CyUSBFSParameters.CLASS_CDC) &&
                (comboBoxClass.SelectedIndex != CLASS_CDC_ITEM))
            {
                needDeleteClass = true;
            }

            switch (comboBoxClass.SelectedIndex)
            {
                case CLASS_NONE_ITEM:
                    if (Descriptor.bInterfaceClass != CyUSBFSParameters.CLASS_NONE)
                    {
                        Descriptor.bInterfaceClass = CyUSBFSParameters.CLASS_NONE;
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(0);
                    break;
                case CLASS_HID_ITEM:
                    if (Descriptor.bInterfaceClass != CyUSBFSParameters.CLASS_HID)
                    {
                        Descriptor.bInterfaceClass = CyUSBFSParameters.CLASS_HID;
                        //ParentFrm.AddNode(USBDescriptorType.HID);
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(0);
                    break;
                case CLASS_CDC_ITEM:
                    if (Descriptor.bInterfaceClass != CyUSBFSParameters.CLASS_CDC)
                    {
                        Descriptor.bInterfaceClass = CyUSBFSParameters.CLASS_CDC;
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(2);
                    break;
                case CLASS_AUDIO_ITEM:
                    if (Descriptor.bInterfaceClass != CyUSBFSParameters.CLASS_AUDIO)
                    {
                        Descriptor.bInterfaceClass = CyUSBFSParameters.CLASS_AUDIO;
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(1);
                    break;
                case CLASS_VENDORSPEC_ITEM:
                    if (Descriptor.bInterfaceClass != CyUSBFSParameters.CLASS_VENDORSPEC)
                    {
                        Descriptor.bInterfaceClass = CyUSBFSParameters.CLASS_VENDORSPEC;
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(0);
                    break;
                default:
                    break;
            }
            if (changed)
            {
                comboBoxSubclass.SelectedIndex = Descriptor.bInterfaceSubClass;
                if (needDeleteClass)
                    RemoveClassNodeEvent();
                if (comboBoxClass.SelectedIndex == CLASS_HID_ITEM)
                {
                    ParentFrm.AddNode(USBDescriptorType.HID);
                }
            }
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxSubclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetProtocolField();

            //Save
            if (comboBoxSubclass.SelectedIndex >= 0)
                Descriptor.bInterfaceSubClass = (byte) comboBoxSubclass.SelectedIndex;
            else
            {
                Descriptor.bInterfaceSubClass = 0;
            }
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void SetProtocolField()
        {
            if ((comboBoxClass.SelectedIndex == CLASS_HID_ITEM) && (comboBoxSubclass.SelectedIndex == 1))
            {
                if (!comboBoxProtocol.Visible)
                {
                    comboBoxProtocol.Visible = true;
                    comboBoxProtocol.SelectedIndex = 0;
                    labelProtocol.Visible = true;
                    groupBoxParams.Height = comboBoxProtocol.Top + comboBoxProtocol.Height + 15;
                }
            }
            else
            {
                if (comboBoxProtocol.Visible)
                {
                    comboBoxProtocol.Visible = false;
                    labelProtocol.Visible = false;
                    groupBoxParams.Height = comboBoxSubclass.Top + comboBoxSubclass.Height + 15;
                    Descriptor.bInterfaceProtocol = 0;
                }
            }
        }

        #region Validation

        private void comboBoxInterfaceString_Validated(object sender, EventArgs e)
        {
            Descriptor.iwInterface = CyUSBFSParameters.SaveStringDescriptor(comboBoxInterfaceString, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            Descriptor.bInterfaceProtocol = (byte) comboBoxProtocol.SelectedIndex;
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        #endregion Validation
    }
}