/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Drawing;
using System.Windows.Forms;

namespace USBFS_v1_30
{
    public partial class CyDetailsAudio : UserControl
    {
        private AudioDescriptor Descriptor;
        private CyUSBFSParameters Parameters;

        private TextBox[] Edits;
        private const int TOTAL_EDITS = 32;

        public CyDetailsAudio(AudioDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            numUpDownLength.Maximum = TOTAL_EDITS;
            CreateBoxes();
            FillSybtypes();
            InitValues();
        }

        private void CreateBoxes()
        {
            int row = 0, col = 0;
            Edits = new TextBox[TOTAL_EDITS];
            for (int i = 0; i < TOTAL_EDITS; i++)
            {
                Edits[i] = new TextBox();
                Edits[i].Width = 22;
                if (Edits[i].Width + 5 + (col)*30 + 10 >= groupBoxEdits.Width)
                {
                    row++;
                    col = 0;
                }
                Edits[i].Left = 5 + (col++)*30;
                Edits[i].Top = 20 + row*25;
                Edits[i].Visible = false;
                Edits[i].KeyPress += new KeyPressEventHandler(textBoxes_KeyPress);
                Edits[i].TextChanged += new EventHandler(textBoxes_TextChanged);
                Edits[i].CausesValidation = true;
                groupBoxEdits.Controls.Add(Edits[i]);
            }
            for (int i = 0; i < numUpDownLength.Value; i++)
                Edits[i].Visible = true;
        }

        private void InitValues()
        {
            if (Descriptor.bValues.Count > 0)
                numUpDownLength.Value = Descriptor.bValues.Count;
            else
            {
                numUpDownLength.Value = 5;
            }
            for (int i = 0; i < Descriptor.bValues.Count; i++)
            {
                Edits[i].Text = Descriptor.bValues[i].ToString("X");
            }

            labelSubclass.TextAlign = ContentAlignment.MiddleLeft;
            switch (Descriptor.bAudioDescriptorSubClass)
            {
                case 0:
                    labelSubclass.Text = "No subclass";
                    break;
                case 1:
                    labelSubclass.Text = "Audio Control Interface Subclass";
                    break;
                case 2:
                    labelSubclass.Text = "Audio Streaming Interface Subclass";
                    break;
                case 3:
                    labelSubclass.Text = "MIDI Streaming Interface Subclass";
                    break;
                case 0xFF:
                    labelSubclass.Text = "";
                    int decrement = comboBoxSybtype.Top - labelSubclass.Top;
                    labelSubclass.Visible = false;
                    labelSubclassTitle.Visible = false;
                    labelSubtype.Top -= decrement;
                    comboBoxSybtype.Top -= decrement;
                    groupBoxSybtype.Height -= decrement;
                    labelLength.Top -= decrement;
                    numUpDownLength.Top -= decrement;
                    groupBoxEdits.Top -= decrement;

                    break;
            }
            comboBoxSybtype.SelectedIndex = Descriptor.bAudioDescriptorSubType;
        }

        private void FillSybtypes()
        {
            comboBoxSybtype.Items.Clear();
            switch (Descriptor.bAudioDescriptorSubClass)
            {
                case (byte) USBOtherTypes.AudioSubclassCodes.AUDIOCONTROL:
                    comboBoxSybtype.Items.Add("Undefined");
                    comboBoxSybtype.Items.Add("Audio Control Interface Header Descriptor");
                    comboBoxSybtype.Items.Add("Input Terminal Descriptor");
                    comboBoxSybtype.Items.Add("Output Terminal Descriptor");
                    comboBoxSybtype.Items.Add("Mixer Unit Descriptor");
                    comboBoxSybtype.Items.Add("Selector Unit Descriptor");
                    comboBoxSybtype.Items.Add("Feature Unit Descriptor");
                    comboBoxSybtype.Items.Add("Processing Unit Descriptor");
                    comboBoxSybtype.Items.Add("Extension Unit Descriptor");
                    comboBoxSybtype.Items.Add("Associated Interface Descriptor");
                    break;
                case (byte) USBOtherTypes.AudioSubclassCodes.AUDIOSTREAMING:
                    comboBoxSybtype.Items.Add("Undefined");
                    comboBoxSybtype.Items.Add("Audio Streaming Interface Descriptor");
                    comboBoxSybtype.Items.Add("Audio Streaming Format Type Descriptor");
                    comboBoxSybtype.Items.Add("Audio Streaming Format Specific Descriptor");
                    break;
                case 0xFF:
                    comboBoxSybtype.Items.Add("Undefined");
                    comboBoxSybtype.Items.Add(
                        "Header Functional Descriptor, which marks the beginning of the concatenated set of " + 
                        "functional descriptors for the interface.");
                    comboBoxSybtype.Items.Add("Call Management Functional Descriptor.");
                    comboBoxSybtype.Items.Add("Abstract Control Management Functional Descriptor.");
                    comboBoxSybtype.Items.Add("Direct Line Management Functional Descriptor.");
                    comboBoxSybtype.Items.Add("Telephone Ringer Functional Descriptor.");
                    comboBoxSybtype.Items.Add(
                        "Telephone Call and Line State Reporting Capabilities Functional Descriptor.");
                    comboBoxSybtype.Items.Add("Union Functional Descriptor");
                    comboBoxSybtype.Items.Add("Country Selection Functional Descriptor");
                    comboBoxSybtype.Items.Add("Telephone Operational Modes Functional Descriptor");
                    comboBoxSybtype.Items.Add("USB Terminal Functional Descriptor");
                    comboBoxSybtype.Items.Add("Network Channel Terminal Descriptor");
                    comboBoxSybtype.Items.Add("Protocol Unit Functional Descriptor");
                    comboBoxSybtype.Items.Add("Extension Unit Functional Descriptor");
                    comboBoxSybtype.Items.Add("Multi-Channel Management Functional Descriptor");
                    comboBoxSybtype.Items.Add("CAPI Control Management Functional Descriptor");
                    comboBoxSybtype.Items.Add("Ethernet Networking Functional Descriptor");
                    comboBoxSybtype.Items.Add("ATM Networking Functional Descriptor");
                    comboBoxSybtype.Items.Add("Wireless Handset Control Model Functional Descriptor");
                    break;
                default:
                    comboBoxSybtype.Items.Add("Undefined");
                    break;
            }
        }

        private void textBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0') && (e.KeyChar <= '9')) &&
                !((e.KeyChar >= 'A') && (e.KeyChar <= 'F')) &&
                !((e.KeyChar >= 'a') && (e.KeyChar <= 'f')))
                e.Handled = true;
            if (((TextBox) sender).Text.Length == 2)
                e.Handled = true;
            if (Char.IsControl(e.KeyChar))
                e.Handled = false;
        }

        private void textBoxes_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox) sender).Text.Length == 2)
                groupBoxEdits.SelectNextControl((Control) sender, true, true, false, false);
            if (((TextBox) sender).Text.Length > 2)
            {
                if (((TextBox) (groupBoxEdits.GetNextControl((Control) sender, true))).Enabled)
                    ((TextBox) (groupBoxEdits.GetNextControl((Control) sender, true))).Text +=
                        ((TextBox) sender).Text.Substring(2);
                ((TextBox) sender).Text = ((TextBox) sender).Text.Remove(2);
            }
        }

        private void numUpDownLength_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < numUpDownLength.Value; i++)
                Edits[i].Visible = true;
            for (int i = TOTAL_EDITS - 1; i >= (int) numUpDownLength.Value; i--)
            {
                Edits[i].Visible = false;
                Edits[i].Text = "";
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Descriptor.bValues.Clear();
            for (int i = 0; i < TOTAL_EDITS; i++)
            {
                if (Edits[i].Text == "") break;
                Descriptor.bValues.Add(Convert.ToByte(Edits[i].Text, 16));
            }
            Descriptor.bLength = (byte) (Descriptor.bValues.Count + 3);
        }

        private void comboBoxSybtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            Descriptor.bAudioDescriptorSubType = (byte) comboBoxSybtype.SelectedIndex;
        }
    }
}