/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;

namespace USBFS_v2_30
{
    public partial class CyDetailsHID : UserControl
    {
        public CyHIDDescriptor m_descriptor;
        public CyUSBFSParameters m_parameters;

        private const byte DESCRIPTOR_TYPE_HID_ITEM = 255;
        private const byte DESCRIPTOR_TYPE_REPORT_ITEM = 0;
        private const byte DESCRIPTOR_TYPE_PHYSICAL_ITEM = 1;

        private bool m_internalChanges = false;

        public CyDetailsHID(CyHIDDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            m_descriptor = descriptor;
            m_parameters = parameters;
            InitFields();
        }

        private void InitFields()
        {
            m_internalChanges = true;
            for (int i = 0; i < CyUSBFSParameters.CountryCodes.Length; i++)
            {
                comboBoxCountryCode.Items.Add(CyUSBFSParameters.CountryCodes[i]);
            }
            comboBoxCountryCode.SelectedIndex = m_descriptor.bCountryCode;
            switch (m_descriptor.bDescriptorType1)
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

            if (m_descriptor.wReportIndex > 0)
            {
                string reportKey = CyDescriptorNode.GetKeyByIndex(m_descriptor.wReportIndex);
                CyDescriptorNode node = m_parameters.m_hidReportTree.GetNodeByKey(reportKey);
                if (node != null)
                {
                    comboBoxReport.SelectedItem = node.m_value;
                }
            }
            m_internalChanges = false;
        }

        private void FillHIDReports()
        {
            comboBoxReport.Items.Clear();
            for (int i = 0; i < m_parameters.m_hidReportTree.m_nodes[0].m_nodes.Count; i++)
            {
                CyHIDReportDescriptor report =
                    (CyHIDReportDescriptor) m_parameters.m_hidReportTree.m_nodes[0].m_nodes[i].m_value;
                comboBoxReport.Items.Add(report);
            }
        }

        #region Validation

        private void comboBoxType_Validated(object sender, EventArgs e)
        {
            switch (comboBoxType.SelectedIndex)
            {
                case DESCRIPTOR_TYPE_HID_ITEM:
                    m_descriptor.bDescriptorType1 = 0x21;
                    break;
                case DESCRIPTOR_TYPE_REPORT_ITEM:
                    m_descriptor.bDescriptorType1 = 0x22;
                    break;
                case DESCRIPTOR_TYPE_PHYSICAL_ITEM:
                    m_descriptor.bDescriptorType1 = 0x23;
                    break;
            }
            if (!m_internalChanges)
                m_parameters.ParamDeviceTreeChanged = true;
        }

        private void comboBoxCountryCode_Validated(object sender, EventArgs e)
        {
            m_descriptor.bCountryCode = (byte)comboBoxCountryCode.SelectedIndex;
            if (!m_internalChanges)
                m_parameters.ParamDeviceTreeChanged = true;
        }

        private void comboBoxReport_Validated(object sender, EventArgs e)
        {
            m_descriptor.wReportIndex = CyUSBFSParameters.SaveReportDescriptor(comboBoxReport, m_parameters);
            if (!m_internalChanges)
                m_parameters.ParamDeviceTreeChanged = true;
        }

        #endregion Validation

        private void comboBoxReport_DropDown(object sender, EventArgs e)
        {
            FillHIDReports();
        }

    }
}
