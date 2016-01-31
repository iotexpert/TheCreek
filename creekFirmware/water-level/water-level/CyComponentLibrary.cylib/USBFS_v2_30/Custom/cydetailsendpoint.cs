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
    public partial class CyDetailsEndpoint : UserControl
    {
        private CyEndpointDescriptor m_descriptor;
        private CyUSBFSParameters m_parameters;

        private const int TRANSFERTYPE_CONTROL_ITEM = 0;
        private const int TRANSFERTYPE_ISOCHRONOUS_ITEM = 3;
        private const int TRANSFERTYPE_BULK_ITEM = 2;
        private const int TRANSFERTYPE_INTERRUPT_ITEM = 1;

        private const int SYNCHTYPE_NOSYNCH_ITEM = 0;
        private const int SYNCHTYPE_ASYNCH_ITEM = 1;
        private const int SYNCHTYPE_ADAPTIVE_ITEM = 2;
        private const int SYNCHTYPE_SYNCH_ITEM = 3;

        private const int USAGETYPE_DATAEP_ITEM = 0;
        private const int USAGETYPE_FEEDBACKEP_ITEM = 1;
        private const int USAGETYPE_IMPLICITEEP_ITEM = 2;

        private bool m_internalChanges = false;
        private bool m_enabled;

        public CyDetailsEndpoint(CyEndpointDescriptor descriptor, CyUSBFSParameters parameters, bool enabled)
        {
            InitializeComponent();
            m_descriptor = descriptor;
            m_parameters = parameters;
            m_enabled = enabled;
            InitFields();
        }

        private void InitFields()
        {
            m_internalChanges = true;
            comboBoxEndpointNum.Items.Clear();
            for (int i = 0; i < 8; i++)
            {
                comboBoxEndpointNum.Items.Add("EP" + (i + 1));
            }

            byte endpointNum = (byte) (m_descriptor.bEndpointAddress & 0x0F);
            byte direction = (byte) (m_descriptor.bEndpointAddress >> 7);
            comboBoxEndpointNum.SelectedIndex = endpointNum - 1;
            if (endpointNum - 1 < 0)
                comboBoxEndpointNum.SelectedIndex = 0;
            comboBoxDirection.SelectedIndex = 1 - direction;

            switch (m_descriptor.bmAttributes & 0x03)
            {
                case CyUSBFSParameters.TRANSFERTYPE_CONTROL:
                    comboBoxTransferType.SelectedIndex = TRANSFERTYPE_CONTROL_ITEM;
                    break;
                case CyUSBFSParameters.TRANSFERTYPE_ISOCHRONOUS:
                    comboBoxTransferType.SelectedIndex = TRANSFERTYPE_ISOCHRONOUS_ITEM;
                    switch ((m_descriptor.bmAttributes & 0x0C) >> 2)
                    {
                        case CyUSBFSParameters.SYNCHTYPE_NOSYNCH:
                            comboBoxSynchType.SelectedIndex = SYNCHTYPE_NOSYNCH_ITEM;
                            break;
                        case CyUSBFSParameters.SYNCHTYPE_ASYNCH:
                            comboBoxSynchType.SelectedIndex = SYNCHTYPE_ASYNCH_ITEM;
                            break;
                        case CyUSBFSParameters.SYNCHTYPE_ADAPTIVE:
                            comboBoxSynchType.SelectedIndex = SYNCHTYPE_ADAPTIVE_ITEM;
                            break;
                        case CyUSBFSParameters.SYNCHTYPE_SYNCH:
                            comboBoxSynchType.SelectedIndex = SYNCHTYPE_SYNCH_ITEM;
                            break;
                        default:
                            comboBoxSynchType.SelectedIndex = 0;
                            break;
                    }
                    switch ((m_descriptor.bmAttributes & 0x30) >> 4)
                    {
                        case CyUSBFSParameters.USAGETYPE_DATAEP:
                            comboBoxUsageType.SelectedIndex = USAGETYPE_DATAEP_ITEM;
                            break;
                        case CyUSBFSParameters.USAGETYPE_FEEDBACKEP:
                            comboBoxUsageType.SelectedIndex = USAGETYPE_FEEDBACKEP_ITEM;
                            break;
                        case CyUSBFSParameters.USAGETYPE_IMPLICITEEP:
                            comboBoxUsageType.SelectedIndex = USAGETYPE_IMPLICITEEP_ITEM;
                            break;
                        default:
                            comboBoxUsageType.SelectedIndex = 0;
                            break;
                    }
                    break;
                case CyUSBFSParameters.TRANSFERTYPE_BULK:
                    comboBoxTransferType.SelectedIndex = TRANSFERTYPE_BULK_ITEM;
                    break;
                case CyUSBFSParameters.TRANSFERTYPE_INTERRUPT:
                    comboBoxTransferType.SelectedIndex = TRANSFERTYPE_INTERRUPT_ITEM;
                    break;
                default:
                    break;
            }
            numUpDownInterval.Value = m_descriptor.bInterval;
            
            if (m_descriptor.wMaxPacketSize > numUpDownMaxPacketSize.Maximum)
            {
                m_descriptor.wMaxPacketSize = (ushort)numUpDownMaxPacketSize.Maximum;
            }
            numUpDownMaxPacketSize.Value = m_descriptor.wMaxPacketSize;
            
            checkBoxDoubleBuffer.Checked = m_descriptor.DoubleBuffer;

            if (!m_enabled)
                groupBoxParams.Enabled = false;

            CheckEPMaxPacketSize();

            m_internalChanges = false;
        }

        private void comboBoxTransferType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTransferType.SelectedIndex == TRANSFERTYPE_ISOCHRONOUS_ITEM)
            {
                labelSynchType.Visible = true;
                labelUsageType.Visible = true;
                comboBoxSynchType.Visible = true;
                comboBoxUsageType.Visible = true;
                comboBoxSynchType.SelectedIndex = 0;
                comboBoxUsageType.SelectedIndex = 0;
                numUpDownMaxPacketSize.Maximum = 1023;
                groupBoxParams.Height = comboBoxUsageType.Top + comboBoxUsageType.Height + groupBoxParams.Height -
                                        (numUpDownMaxPacketSize.Top + numUpDownMaxPacketSize.Height) +
                                        (numUpDownMaxPacketSize.Top - numUpDownInterval.Top)*2;
                checkBoxDoubleBuffer.Top = groupBoxParams.Top + groupBoxParams.Height + 7;
            }
            else
            {
                labelSynchType.Visible = false;
                labelUsageType.Visible = false;
                comboBoxSynchType.Visible = false;
                comboBoxUsageType.Visible = false;
                numUpDownMaxPacketSize.Maximum = 64;
                if (!m_internalChanges)
                    numUpDownMaxPacketSize_Validated(this, EventArgs.Empty);
                groupBoxParams.Height = comboBoxUsageType.Top + comboBoxUsageType.Height + groupBoxParams.Height -
                                        (numUpDownMaxPacketSize.Top + numUpDownMaxPacketSize.Height);
                checkBoxDoubleBuffer.Top = groupBoxParams.Top + groupBoxParams.Height + 7;
            }

            if (comboBoxTransferType.SelectedIndex == TRANSFERTYPE_BULK_ITEM)
            {
                numUpDownInterval.Enabled = false;
            }
            else
            {
                numUpDownInterval.Enabled = true;
            }
        }

        private void CheckEPMaxPacketSize()
        {
            if ((m_parameters.EPMemoryMgmt != (byte)CyUSBFSParameters.CyMemoryManagement.DMA_AUTOMATIC_MEMORY) &&
                (numUpDownMaxPacketSize.Value > 512))
            {
                errProvider.SetError(numUpDownMaxPacketSize, Properties.Resources.ERR_EP_MAXPACKETSIZE);
            }
            else
            {
                errProvider.SetError(numUpDownMaxPacketSize, String.Empty);
            }
        }

        #region Validation

        private void comboBoxEndpointNum_Validated(object sender, EventArgs e)
        {
            if ((comboBoxEndpointNum.SelectedIndex >= 0) && (comboBoxDirection.SelectedIndex >= 0))
            {
                byte endpointNum = (byte) (comboBoxEndpointNum.SelectedIndex + 1);
                byte direction = (byte) (1 - comboBoxDirection.SelectedIndex);
                m_descriptor.bEndpointAddress = (byte) (endpointNum | (direction << 7));
                if (!m_internalChanges)
                    m_parameters.ParamDeviceTreeChanged = true;
            }
        }

        private void comboBoxTransferType_Validated(object sender, EventArgs e)
        {
            byte synchType, usageType;
            switch (comboBoxTransferType.SelectedIndex)
            {
                case TRANSFERTYPE_CONTROL_ITEM:
                    m_descriptor.bmAttributes = CyUSBFSParameters.TRANSFERTYPE_CONTROL;
                    break;
                case TRANSFERTYPE_ISOCHRONOUS_ITEM:
                    switch (comboBoxSynchType.SelectedIndex)
                    {
                        case SYNCHTYPE_NOSYNCH_ITEM:
                            synchType = CyUSBFSParameters.SYNCHTYPE_NOSYNCH;
                            break;
                        case SYNCHTYPE_ASYNCH_ITEM:
                            synchType = CyUSBFSParameters.SYNCHTYPE_ASYNCH;
                            break;
                        case SYNCHTYPE_ADAPTIVE_ITEM:
                            synchType = CyUSBFSParameters.SYNCHTYPE_ADAPTIVE;
                            break;
                        case SYNCHTYPE_SYNCH_ITEM:
                            synchType = CyUSBFSParameters.SYNCHTYPE_SYNCH;
                            break;
                        default:
                            synchType = 0;
                            break;
                    }
                    switch (comboBoxUsageType.SelectedIndex)
                    {
                        case USAGETYPE_DATAEP_ITEM:
                            usageType = CyUSBFSParameters.USAGETYPE_DATAEP;
                            break;
                        case USAGETYPE_FEEDBACKEP_ITEM:
                            usageType = CyUSBFSParameters.USAGETYPE_FEEDBACKEP;
                            break;
                        case USAGETYPE_IMPLICITEEP_ITEM:
                            usageType = CyUSBFSParameters.USAGETYPE_IMPLICITEEP;
                            break;
                        default:
                            usageType = 0;
                            break;
                    }
                    m_descriptor.bmAttributes =
                        (byte) (CyUSBFSParameters.TRANSFERTYPE_ISOCHRONOUS | (synchType << 2) | (usageType << 4));
                    break;
                case TRANSFERTYPE_BULK_ITEM:
                    m_descriptor.bmAttributes = CyUSBFSParameters.TRANSFERTYPE_BULK;
                    break;
                case TRANSFERTYPE_INTERRUPT_ITEM:
                    m_descriptor.bmAttributes = CyUSBFSParameters.TRANSFERTYPE_INTERRUPT;
                    break;
                default:
                    break;
            }
            if (!m_internalChanges)
                m_parameters.ParamDeviceTreeChanged = true;
        }

        private void checkBoxDoubleBuffer_CheckedChanged(object sender, EventArgs e)
        {
            m_descriptor.DoubleBuffer = checkBoxDoubleBuffer.Checked;
            if (!m_internalChanges)
                m_parameters.ParamDeviceTreeChanged = true;
        }

        private void numUpDownInterval_Validated(object sender, EventArgs e)
        {
            if (m_descriptor.bInterval != Convert.ToByte(numUpDownInterval.Value))
            {
                m_descriptor.bInterval = Convert.ToByte(numUpDownInterval.Value);
                if (!m_internalChanges)
                    m_parameters.ParamDeviceTreeChanged = true;
            }
        }

        private void numUpDownMaxPacketSize_Validated(object sender, EventArgs e)
        {
            if (m_descriptor.wMaxPacketSize != Convert.ToUInt16(numUpDownMaxPacketSize.Value))
            {
                m_descriptor.wMaxPacketSize = Convert.ToUInt16(numUpDownMaxPacketSize.Value);
                if (!m_internalChanges)
                    m_parameters.ParamDeviceTreeChanged = true;
            }
            CheckEPMaxPacketSize();
        }

        #endregion Validation
    }
}
