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
    public partial class CyDetailsInterface : UserControl
    {
        const byte CLASS_NONE_ITEM = 0;
        const byte CLASS_HID_ITEM = 1;
        const byte CLASS_CDC_ITEM = 100;
        const byte CLASS_AUDIO_ITEM = 101;
        const byte CLASS_VENDORSPEC_ITEM = 2;

        private InterfaceDescriptor Descriptor;
        private USBFSParameters Parameters;
        private CyDeviceDescriptor ParentFrm;
        public delegate void RemoveNodeDelegate();
        public event RemoveNodeDelegate RemoveClassNodeEvent;

        private bool InternalChanges = false;

        public CyDetailsInterface(InterfaceDescriptor descriptor, USBFSParameters parameters, CyDeviceDescriptor parentForm)
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
            numUpDownInterfaceNumber.Value = Descriptor.bInterfaceNumber;
            numUpDownAlternateSettings.Value = Descriptor.bAlternateSetting;

            switch (Descriptor.bInterfaceClass)
            {
                case USBFSParameters.CLASS_NONE:
                    comboBoxClass.SelectedIndex = CLASS_NONE_ITEM;
                    break;
                case USBFSParameters.CLASS_HID:
                    comboBoxClass.SelectedIndex = CLASS_HID_ITEM;
                    break;
                case USBFSParameters.CLASS_CDC:
                    comboBoxClass.SelectedIndex = CLASS_CDC_ITEM;
                    break;
                case USBFSParameters.CLASS_AUDIO:
                    comboBoxClass.SelectedIndex = CLASS_AUDIO_ITEM;
                    ParentFrm.SetBtnAddClass(1);
                    break;
                case USBFSParameters.CLASS_VENDORSPEC:
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
                //if (Descriptor.iwInterface <= comboBoxInterfaceString.Items.Count)
                //    comboBoxInterfaceString.SelectedIndex = (int)Descriptor.iwInterface - 1;
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
                StringDescriptor strDesc = (StringDescriptor)Parameters.StringTree.Nodes[0].Nodes[i].Value;
                if (strDesc.bString != null)
                {
                    comboBoxInterfaceString.Items.Add(strDesc);
                }
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            //// If class was changed from HID or Audio to another, remove class descriptor
            //bool needDeleteClass = false;
            //if ((Descriptor.bInterfaceClass == USBFSParameters.CLASS_HID) && (comboBoxClass.SelectedIndex != CLASS_HID_ITEM))
            //{
            //    needDeleteClass = true;
            //}
            //if ((Descriptor.bInterfaceClass == USBFSParameters.CLASS_AUDIO) && (comboBoxClass.SelectedIndex != CLASS_AUDIO_ITEM))
            //{
            //    needDeleteClass = true;
            //}
            
            //if (needDeleteClass) RemoveClassNodeEvent();

            //switch (comboBoxClass.SelectedIndex)
            //{
            //    case CLASS_NONE_ITEM:
            //        Descriptor.bInterfaceClass = USBFSParameters.CLASS_NONE;
            //        ParentFrm.SetBtnAddClass(false);
            //        break;
            //    case CLASS_HID_ITEM:
            //        if (Descriptor.bInterfaceClass != USBFSParameters.CLASS_HID)
            //        {
            //            Descriptor.bInterfaceClass = USBFSParameters.CLASS_HID;
            //            ParentFrm.AddNode(USBDescriptorType.HID);
            //        }
            //        ParentFrm.SetBtnAddClass(false);
            //        break;
            //    case CLASS_CDC_ITEM:
            //        Descriptor.bInterfaceClass = USBFSParameters.CLASS_CDC;
            //        ParentFrm.SetBtnAddClass(false);
            //        break;
            //    case CLASS_AUDIO_ITEM:
            //        Descriptor.bInterfaceClass = USBFSParameters.CLASS_AUDIO;
            //        ParentFrm.SetBtnAddClass(true);
            //        break;
            //    case CLASS_VENDORSPEC_ITEM:
            //        Descriptor.bInterfaceClass = USBFSParameters.CLASS_VENDORSPEC;
            //        ParentFrm.SetBtnAddClass(false);
            //        break;
            //    default:
            //        break;

            //}

            //Descriptor.iInterface = USBFSParameters.SaveStringDescriptor(comboBoxInterfaceString, Parameters);
            //if (comboBoxProtocol.Visible)
            //    Descriptor.bInterfaceProtocol = (byte)comboBoxProtocol.SelectedIndex;
            //else
            //    Descriptor.bInterfaceProtocol = 0;
        }

        private void CyDetailsInterface_SizeChanged(object sender, EventArgs e)
        {
            buttonApply.Left = (Width - buttonApply.Width) / 2;
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
            //Descriptor.bInterfaceSubClass = 0;
            //comboBoxSubclass.SelectedIndex = 0;
            //SetProtocolField();

            //Save
            bool changed = false;

            // If class was changed from HID or Audio to another, remove class descriptor
            bool needDeleteClass = false;
            if ((Descriptor.bInterfaceClass == USBFSParameters.CLASS_HID) && (comboBoxClass.SelectedIndex != CLASS_HID_ITEM))
            {
                needDeleteClass = true;
            }
            if ((Descriptor.bInterfaceClass == USBFSParameters.CLASS_AUDIO) && (comboBoxClass.SelectedIndex != CLASS_AUDIO_ITEM))
            {
                needDeleteClass = true;
            }
            if ((Descriptor.bInterfaceClass == USBFSParameters.CLASS_CDC) && (comboBoxClass.SelectedIndex != CLASS_CDC_ITEM))
            {
                needDeleteClass = true;
            }

            //if (needDeleteClass) RemoveClassNodeEvent();            

            switch (comboBoxClass.SelectedIndex)
            {
                case CLASS_NONE_ITEM:
                    if (Descriptor.bInterfaceClass != USBFSParameters.CLASS_NONE)
                    {
                        Descriptor.bInterfaceClass = USBFSParameters.CLASS_NONE;
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(0);
                    break;
                case CLASS_HID_ITEM:
                    if (Descriptor.bInterfaceClass != USBFSParameters.CLASS_HID)
                    {
                        Descriptor.bInterfaceClass = USBFSParameters.CLASS_HID;
                        //ParentFrm.AddNode(USBDescriptorType.HID);
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(0);
                    break;
                case CLASS_CDC_ITEM:
                    if (Descriptor.bInterfaceClass != USBFSParameters.CLASS_CDC)
                    {
                        Descriptor.bInterfaceClass = USBFSParameters.CLASS_CDC;
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(2);
                    break;
                case CLASS_AUDIO_ITEM:
                    if (Descriptor.bInterfaceClass != USBFSParameters.CLASS_AUDIO)
                    {
                        Descriptor.bInterfaceClass = USBFSParameters.CLASS_AUDIO;
                        changed = true;
                    }
                    ParentFrm.SetBtnAddClass(1);
                    break;
                case CLASS_VENDORSPEC_ITEM:
                    if (Descriptor.bInterfaceClass != USBFSParameters.CLASS_VENDORSPEC)
                    {
                        Descriptor.bInterfaceClass = USBFSParameters.CLASS_VENDORSPEC;
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
                Descriptor.bInterfaceSubClass = (byte)comboBoxSubclass.SelectedIndex;
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
                    buttonApply.Top = groupBoxParams.Top + groupBoxParams.Height + 10; 
                }
            }
            else
            {
                if (comboBoxProtocol.Visible)
                {
                    comboBoxProtocol.Visible = false;
                    labelProtocol.Visible = false;
                    groupBoxParams.Height = comboBoxSubclass.Top + comboBoxSubclass.Height + 15;
                    buttonApply.Top = groupBoxParams.Top + groupBoxParams.Height + 10;
                    Descriptor.bInterfaceProtocol = 0;
                }   
            }
        }

        #region Validation

        private void comboBoxInterfaceString_Validated(object sender, EventArgs e)
        {
            Descriptor.iwInterface = USBFSParameters.SaveStringDescriptor(comboBoxInterfaceString, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            Descriptor.bInterfaceProtocol = (byte)comboBoxProtocol.SelectedIndex;
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void textBoxAlternateSettings_Validated(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Descriptor.bAlternateSetting != Convert.ToByte(textBoxAlternateSettings.Text))
            //    {
            //        Descriptor.bAlternateSetting = Convert.ToByte(textBoxAlternateSettings.Text);
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    if ((textBoxAlternateSettings.Text == "") && (Descriptor.bAlternateSetting != 0))
            //    {
            //        Descriptor.bAlternateSetting = 0;
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
        }

        private void textBoxInterfaceNumber_Validated(object sender, EventArgs e)
        {
            //try
            //{
            //    if (Descriptor.bInterfaceNumber != Convert.ToByte(textBoxInterfaceNumber.Text))
            //    {
            //        Descriptor.bInterfaceNumber = Convert.ToByte(textBoxInterfaceNumber.Text);
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    if ((textBoxInterfaceNumber.Text == "") && (Descriptor.bInterfaceNumber != 0))
            //    {
            //        Descriptor.bInterfaceNumber = 0;
            //        if (!InternalChanges)
            //            Parameters.ParamsChanged = true;
            //    }
            //}
        }

        private void textBoxInterfaceNumber_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(((TextBox) sender).Text != "")
                {
                    byte res = Convert.ToByte(((TextBox) sender).Text);
                    if (res > 255) throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Interface Number Value (must be 0-255)", "USBFS", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void textBoxAlternateSettings_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text != "")
                {
                    byte res = Convert.ToByte(((TextBox) sender).Text);
                    if (res > 255) throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Alternate Settings Value (must be 0-255)", "USBFS", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void textBoxInterfaceNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar != (char)Keys.Enter) && (e.KeyChar != (char)Keys.Back))
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))
                    e.Handled = true;
            }
        }

        #endregion Validation

        private void numUpDownInterfaceNumber_Validated(object sender, EventArgs e)
        {
            if (Descriptor.bInterfaceNumber != Convert.ToByte(numUpDownInterfaceNumber.Value))
            {
                Descriptor.bInterfaceNumber = Convert.ToByte(numUpDownInterfaceNumber.Value);
                if (!InternalChanges)
                    Parameters.ParamsChanged = true;
            }
        }

        private void numUpDownAlternateSettings_Validated(object sender, EventArgs e)
        {
            if (Descriptor.bAlternateSetting != Convert.ToByte(numUpDownAlternateSettings.Value))
            {
                Descriptor.bAlternateSetting = Convert.ToByte(numUpDownAlternateSettings.Value);
                if (!InternalChanges)
                    Parameters.ParamsChanged = true;
            }
        }
    }
}
