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
    public partial class CyDetailsHID : UserControl
    {
        public HIDDescriptor Descriptor;
        public CyUSBFSParameters Parameters;

        private const byte DESCRIPTOR_TYPE_HID_ITEM = 255;
        private const byte DESCRIPTOR_TYPE_REPORT_ITEM = 0;
        private const byte DESCRIPTOR_TYPE_PHYSICAL_ITEM = 1;

        private bool InternalChanges = false;

        public CyDetailsHID(HIDDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            Descriptor = descriptor;
            Parameters = parameters;
            InitFields();
        }

        private void InitFields()
        {
            InternalChanges = true;
            for (int i = 0; i < CyUSBFSParameters.CountryCodes.Length; i++)
            {
                comboBoxCountryCode.Items.Add(CyUSBFSParameters.CountryCodes[i]);
            }
            comboBoxCountryCode.SelectedIndex = Descriptor.bCountryCode;
            switch (Descriptor.bDescriptorType1)
            {
                case 0x21:
                    comboBoxType.SelectedIndex = DESCRIPTOR_TYPE_HID_ITEM;
                    break;
                case 0x22:
                    comboBoxType.SelectedIndex = DESCRIPTOR_TYPE_REPORT_ITEM;
                    break;
                case 0x23:
                    comboBoxType.SelectedIndex = DESCRIPTOR_TYPE_PHYSICAL_ITEM;
                    break;
                default:
                    comboBoxType.SelectedIndex = DESCRIPTOR_TYPE_REPORT_ITEM;
                    break;
            }

            FillHIDReports();

            if (Descriptor.wReportIndex > 0)
            {
                string reportKey = DescriptorNode.GetKeyByIndex(Descriptor.wReportIndex);
                DescriptorNode node = Parameters.HIDReportTree.GetNodeByKey(reportKey);
                if (node != null)
                {
                    comboBoxReport.SelectedItem = node.Value;
                }
            }
            InternalChanges = false;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Descriptor.bCountryCode = (byte)comboBoxCountryCode.SelectedIndex;
            switch (comboBoxType.SelectedIndex)
            {
                case DESCRIPTOR_TYPE_HID_ITEM:
                    Descriptor.bDescriptorType1 = 0x21;
                    break;
                case DESCRIPTOR_TYPE_REPORT_ITEM:
                    Descriptor.bDescriptorType1 = 0x22;
                    break;
                case DESCRIPTOR_TYPE_PHYSICAL_ITEM:
                    Descriptor.bDescriptorType1 = 0x23;
                    break;
            }
        }

        private void FillHIDReports()
        {
            comboBoxReport.Items.Clear();
            for (int i = 0; i < Parameters.HIDReportTree.Nodes[0].Nodes.Count; i++)
            {
                HIDReportDescriptor report = (HIDReportDescriptor)Parameters.HIDReportTree.Nodes[0].Nodes[i].Value;
                comboBoxReport.Items.Add(report);
            }
        }

        #region Validation

        private void comboBoxType_Validated(object sender, EventArgs e)
        {
            switch (comboBoxType.SelectedIndex)
            {
                case DESCRIPTOR_TYPE_HID_ITEM:
                    Descriptor.bDescriptorType1 = 0x21;
                    break;
                case DESCRIPTOR_TYPE_REPORT_ITEM:
                    Descriptor.bDescriptorType1 = 0x22;
                    break;
                case DESCRIPTOR_TYPE_PHYSICAL_ITEM:
                    Descriptor.bDescriptorType1 = 0x23;
                    break;
            }
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxCountryCode_Validated(object sender, EventArgs e)
        {
            Descriptor.bCountryCode = (byte)comboBoxCountryCode.SelectedIndex;
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        private void comboBoxReport_Validated(object sender, EventArgs e)
        {
            Descriptor.wReportIndex = CyUSBFSParameters.SaveReportDescriptor(comboBoxReport, Parameters);
            if (!InternalChanges)
                Parameters.ParamsChanged = true;
        }

        #endregion Validation

        private void comboBoxReport_DropDown(object sender, EventArgs e)
        {
            FillHIDReports();
        }

    }
}
