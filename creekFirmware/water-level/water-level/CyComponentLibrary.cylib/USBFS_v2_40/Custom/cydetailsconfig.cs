/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace USBFS_v2_40
{
    public partial class CyDetailsConfig : UserControl
    {
        public CyConfigDescriptor m_descriptor;
        public CyUSBFSParameters m_parameters;
        private bool m_internalChanges = false;

        public CyDetailsConfig(CyConfigDescriptor descriptor, CyUSBFSParameters parameters)
        {
            InitializeComponent();
            m_descriptor = descriptor;
            m_parameters = parameters;
            InitFields();
        }

        private void InitFields()
        {
            m_internalChanges = true;
            comboBoxConfigString.Text = "";
            numUpDownMaxPower.Value = (m_descriptor.bMaxPower * 2);
            comboBoxDevicePower.SelectedIndex = (m_descriptor.bmAttributes & 0x40) == 0 ? 0 : 1;
            comboBoxRemoteWakeup.SelectedIndex = (m_descriptor.bmAttributes & 0x20) == 0 ? 0 : 1;

            // Configuration string
            FillConfigStrings();

            if (m_descriptor.iwConfiguration > 0)
            {
                string configStrKey = CyDescriptorNode.GetKeyByIndex(m_descriptor.iwConfiguration);
                CyDescriptorNode node = m_parameters.m_stringTree.GetNodeByKey(configStrKey);
                if (node != null)
                {
                    comboBoxConfigString.SelectedItem = node.m_value;
                }
            }
            else
            {
                comboBoxConfigString.Text = "";
            }
            m_internalChanges = false;
        }

        private void comboBoxConfigString_DropDown(object sender, EventArgs e)
        {
            FillConfigStrings();
        }

        private void FillConfigStrings()
        {
            comboBoxConfigString.Items.Clear();
            List<CyStringDescriptor> strList = m_parameters.GetStringDescList();
            comboBoxConfigString.Items.AddRange(strList.ToArray());
        }

        #region Validation

        private void comboBoxConfigString_Validated(object sender, EventArgs e)
        {
            m_descriptor.iwConfiguration = CyUSBFSParameters.SaveStringDescriptor(comboBoxConfigString, m_parameters);
            m_descriptor.sConfiguration = comboBoxConfigString.Text;
            if (!m_internalChanges)
                m_parameters.ParamDeviceTreeChanged = true;
        }

        private void comboBoxDevicePower_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBoxDevicePower.SelectedIndex >= 0) && (comboBoxRemoteWakeup.SelectedIndex >= 0))
            {
                byte devicePower = (byte) comboBoxDevicePower.SelectedIndex;
                byte remoteWakeup = (byte) comboBoxRemoteWakeup.SelectedIndex;
                m_descriptor.bmAttributes = (byte) (0x80 | (devicePower << 6) | (remoteWakeup << 5));
                if (!m_internalChanges)
                    m_parameters.ParamDeviceTreeChanged = true;
            }
        }

        private void numUpDownMaxPower_Validated(object sender, EventArgs e)
        {
            if (m_descriptor.bMaxPower != (byte)(Convert.ToUInt16(numUpDownMaxPower.Value) / 2))
            {
                m_descriptor.bMaxPower = (byte)(Convert.ToUInt16(numUpDownMaxPower.Value) / 2);
                if (!m_internalChanges)
                    m_parameters.ParamDeviceTreeChanged = true;
            }
        }
        #endregion Validation
    }
}
