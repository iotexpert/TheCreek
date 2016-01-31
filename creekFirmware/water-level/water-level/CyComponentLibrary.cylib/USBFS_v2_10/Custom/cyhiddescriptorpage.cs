/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace USBFS_v2_10
{
    public partial class CyHidDescriptorPage : UserControl
    {
        #region Variables

        public CyUSBFSParameters m_parameters;
        private UserControl m_controlDetails;
        private List<UserControl> m_unusedControls;

        public List<CyHidReportItem> m_reportItems;

        #endregion Variables

        #region Constructors

        public CyHidDescriptorPage()
        {
            InitializeComponent();
        }

        public CyHidDescriptorPage(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            m_unusedControls = new List<UserControl>();
            InitItems();
            RefreshReportTree();
        }

        #endregion Constructors

        #region Private functions

        /// <summary>
        /// Initializes m_reportItems list and fills listBoxItems listBox.
        /// </summary>
        private void InitItems()
        {
            m_reportItems = new List<CyHidReportItem>();
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_USAGE, 0x08, 2, CyHidReportItemKind.LIST));
            for (int i = 0; i < CyHIDReportItemTables.ValuesGenericDesktopPage.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesGenericDesktopPage[i*2 + 1], 16),
                    CyHIDReportItemTables.ValuesGenericDesktopPage[i*2]);
            }
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_USAGE_PAGE, 0x04, 2,
                                                  CyHidReportItemKind.LIST));
            for (int i = 0; i < CyHIDReportItemTables.ValuesUsagePage.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesUsagePage[i*2 + 1], 16),
                    CyHIDReportItemTables.ValuesUsagePage[i*2]);
            }
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_USAGE_MINIMUM, 0x18, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_USAGE_MAXIMUM, 0x28, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_DESIGNATOR_INDEX, 0x38, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_DESIGNATOR_MINIMUM, 0x48, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_DESIGNATOR_MAXIMUM, 0x58, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_STRING_INDEX, 0x78, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_STRING_MINIMUM, 0x88, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_STRING_MAXIMUM, 0x98, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_COLLECTION, 0xA0, 2,
                                                  CyHidReportItemKind.LIST));
            for (int i = 0; i < CyHIDReportItemTables.ValuesCollection.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesCollection[i * 2 + 1], 16),
                    CyHIDReportItemTables.ValuesCollection[i*2]);
            }
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_END_COLLECTION, 0xC0, 0,
                                                  CyHidReportItemKind.NONE));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_INPUT, 0x80, 2, CyHidReportItemKind.BITS));
            for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                    CyHIDReportItemTables.ValuesInput[i*2]);
            }
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_OUTPUT, 0x90, 2, CyHidReportItemKind.BITS));
            for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                    CyHIDReportItemTables.ValuesInput[i*2]);
            }
            m_reportItems[m_reportItems.Count - 1].m_valuesList[14] = "Non Volatile";
            m_reportItems[m_reportItems.Count - 1].m_valuesList[15] = "Volatile";
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_FEATURE, 0xB0, 2, 
                                                  CyHidReportItemKind.BITS));
            for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                    CyHIDReportItemTables.ValuesInput[i*2]);
            }
            m_reportItems[m_reportItems.Count - 1].m_valuesList[14] = "Non Volatile";
            m_reportItems[m_reportItems.Count - 1].m_valuesList[15] = "Volatile";
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_LOGICAL_MINIMUM, 0x14, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_LOGICAL_MAXIMUM, 0x24, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_PHYSICAL_MINIMUM, 0x34, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_PHYSICAL_MAXIMUM, 0x44, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_UNIT_EXPONENT, 0x54, 2,
                                                  CyHidReportItemKind.LIST));
            for (int i = 0; i < CyHIDReportItemTables.ValuesUnitExponent.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesUnitExponent[i * 2 + 1], 16),
                    CyHIDReportItemTables.ValuesUnitExponent[i*2]);
            }
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_UNIT, 0x64, 2, CyHidReportItemKind.UNIT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_REPORT_SIZE, 0x74, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_REPORT_ID, 0x84, 2, 
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_REPORT_COUNT, 0x94, 2,
                                                  CyHidReportItemKind.INT));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_PUSH, 0xA4, 0, CyHidReportItemKind.NONE));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_POP, 0xB4, 0, CyHidReportItemKind.NONE));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_DELIMITER, 0xA8, 1, 
                                                  CyHidReportItemKind.LIST));
            for (int i = 0; i < CyHIDReportItemTables.ValuesDelimiter.Length / 2; i++)
            {
                m_reportItems[m_reportItems.Count - 1].m_valuesList.Add(
                    Convert.ToUInt16(CyHIDReportItemTables.ValuesDelimiter[i * 2 + 1], 16),
                    CyHIDReportItemTables.ValuesDelimiter[i*2]);
            }
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_CUSTOM, 0x00, 0, 
                                                  CyHidReportItemKind.CUSTOM));
            m_reportItems.Add(new CyHidReportItem(CyUSBFSParameters.RPTITEM_COMMENT, 0x00, 0, 
                                                  CyHidReportItemKind.NONE));

            listBoxItems.Items.Clear();
            for (int i = 0; i < m_reportItems.Count; i++)
            {
                listBoxItems.Items.Add(m_reportItems[i]);
            }
            listBoxItems.SelectedIndex = 0;
        }

        private void RefreshReportTree()
        {
            treeViewReport.BeginUpdate();
            treeViewReport.Nodes.Clear();
            for (int i = 0; i < m_parameters.m_hidReportTree.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor = m_parameters.m_hidReportTree.m_nodes[i];
                TreeNode node = treeViewReport.Nodes.Add(descriptor.Key,
                                                         CyDescriptorNode.GetDescriptorLabel(descriptor), 0, 0);
                RefreshNode(descriptor, node);
            }

            treeViewReport.ExpandAll();
            treeViewReport.EndUpdate();
            if (treeViewReport.Nodes.Count > 0)
            {
                treeViewReport.SelectedNode = treeViewReport.Nodes[0].FirstNode;
            }
        }

        private void RefreshNode(CyDescriptorNode descriptor, TreeNode treeNode)
        {
            int imageIndex = 0;
            Stack<TreeNode> CollectionNodes = new Stack<TreeNode>();
            //CollectionNodes.Push(treeNode);
            TreeNode collectionNode = treeNode;
            for (int i = 0; i < descriptor.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor_child = descriptor.m_nodes[i];
                switch (descriptor_child.m_value.bDescriptorType)
                {
                    case CyUSBDescriptorType.HID_REPORT:
                        imageIndex = 3;
                        break;
                    case CyUSBDescriptorType.HID_REPORT_ITEM:
                        imageIndex = 5;
                        break;
                }

                TreeNode node = collectionNode.Nodes.Add(descriptor_child.Key,
                                                         CyDescriptorNode.GetDescriptorLabel(descriptor_child),
                                                         imageIndex, imageIndex);
                // Draw comments in a different color
                if (CyUSBFSParameters.StringIsComment(node.Text))
                {
                    node.ForeColor = Color.Green;
                    node.ImageIndex = 20;
                    node.SelectedImageIndex = 20;
                }

                if ((descriptor_child.m_value.bDescriptorType == CyUSBDescriptorType.HID_REPORT_ITEM) &&
                    (((CyHIDReportItemDescriptor) descriptor_child.m_value).Item.Name == "COLLECTION"))
                {
                    CollectionNodes.Push(collectionNode);
                    collectionNode = node;
                }
                else if ((descriptor_child.m_value.bDescriptorType == CyUSBDescriptorType.HID_REPORT_ITEM) &&
                         (((CyHIDReportItemDescriptor) descriptor_child.m_value).Item.Name == "END_COLLECTION"))
                {
                    if (CollectionNodes.Count > 0)
                        collectionNode = CollectionNodes.Pop();
                }
                RefreshNode(descriptor_child, node);
            }
        }

        public void RefreshSelectedNode()
        {
            CyDescriptorNode node = m_parameters.m_hidReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            try
            {
                CyHidReportItem item = ((CyHIDReportItemDescriptor)node.m_value).Item;
                if (item.Kind != CyHidReportItemKind.CUSTOM)
                    treeViewReport.SelectedNode.Text = ((CyHIDReportItemDescriptor)node.m_value).Item.ToString();
                else
                    treeViewReport.SelectedNode.Text = ((CyHIDReportItemDescriptor)node.m_value).Item.ToCustomString();
            }
            catch
            {
                // If applied to another item type, do nothing
            }
        }

        /// <summary>
        /// Refresh the header text of the Settings panel depending on value of the item that is currently configured
        /// </summary>
        public void RefreshSettingsHeader()
        {
            CyDescriptorNode selectedItem = m_parameters.m_hidReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            CyHidReportItem reportItem;
            if ((selectedItem != null) && (treeViewReport.SelectedNode.Level > 1))
            {
                reportItem = ((CyHIDReportItemDescriptor) selectedItem.m_value).Item;
                labelItemSettings.Text = string.Format("Item Value ({0})", reportItem.ToString().Trim());
            }
        }

        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxItems.SelectedIndex >= 0)
            {
                if (m_controlDetails != null)
                {
                    panelDetails.Controls.Remove(m_controlDetails);
                    CyDeviceDescriptorPage.CleanControls(m_unusedControls);
                    m_unusedControls.Add(m_controlDetails);
                }
                panelDetails.Visible = true;

                // If Selected Item is USAGE, choose the appropriate list
                if (((CyHidReportItem)listBoxItems.SelectedItem).Name == CyUSBFSParameters.RPTITEM_USAGE)
                {
                    UInt16 lastUsagePage = GetLastUsagePageType();
                    switch (lastUsagePage)
                    {
                        case 0x01: // Generic Desktop Controls
                            SetListForItem((CyHidReportItem) listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGenericDesktopPage);
                            break;
                        case 0x02: // Simulation Controls
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesSimulationControlsPage);
                            break;
                        case 0x03: // VR Controls
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesVRControlPage);
                            break;
                        case 0x04: // Sport Controls
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesSportControlPage);
                            break;
                        case 0x05: // Game Controls
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGameControlsPage);
                            break;
                        case 0x06: // Generic Device Controls
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGenericDeviceControlsPage);
                            break;
                        case 0x07: // Keyboard/Keypad
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesKeyboardPage);
                            break;
                        case 0x08: // LEDs
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesLEDPage);
                            break;
                        case 0x09: // Button
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesButtonPage);
                            break;
                        case 0x0A: // Ordinal
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesOrdinalPage);
                            break;
                        case 0x0B: // Telephony Devices
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesTelephonyDevicePage);
                            break;
                        case 0x0C: // Consumer Devices
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesConsumerPage);
                            break;
                        case 0x0D: // Digitizer
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesDigitizersPage);
                            break;
                        case 0x0F: // Physical Input Device (PID)
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesPIDPage);
                            break;
                        case 0x10: // Unicode
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesUnicodePage);
                            break;
                        case 0x14: // Alphanumeric Display
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesAlphanumericDisplayPage);
                            break;
                        case 0x40: // Medical Instruments
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesMedicalInstrumentPage);
                            break;
                        case 0x80: // Monitor Devices
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesMonitorDevicesPage);
                            break;
                        case 0x81: // Monitor Enumerated Values
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesMonitorEnumeratedValuesPage); 
                            break;
                        case 0x82: // VESA Virtual Controls
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesVESAVirtualControlsPage);
                            break;
                        case 0x84: // Power Device
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesPowerDevicePage);
                            break;
                        case 0x85: // Battery System
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesBatterySystemPage);
                            break;
                        default:
                            SetListForItem((CyHidReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGenericDesktopPage);
                            break;
                    }
                }

                CyReportBase report = null;
                switch (((CyHidReportItem) listBoxItems.SelectedItem).Kind)
                {
                    case CyHidReportItemKind.BITS:
                        report =
                            new CyReportBits(new CyHidReportItem((CyHidReportItem) listBoxItems.SelectedItem), false);
                        break;
                    case CyHidReportItemKind.LIST:
                        report =
                            new CyReportList(new CyHidReportItem((CyHidReportItem) listBoxItems.SelectedItem), false);
                        break;
                    case CyHidReportItemKind.INT:
                        report =
                            new CyReportNumber(new CyHidReportItem((CyHidReportItem) listBoxItems.SelectedItem), false);
                        break;
                    case CyHidReportItemKind.UNIT:
                        report =
                            new CyReportUnit(new CyHidReportItem((CyHidReportItem) listBoxItems.SelectedItem), false);
                        break;
                    case CyHidReportItemKind.NONE:
                        m_controlDetails = null;
                        break;
                    case CyHidReportItemKind.CUSTOM:
                        report =
                            new CyReportCustom(new CyHidReportItem((CyHidReportItem) listBoxItems.SelectedItem), false);
                        break;
                    default:
                        break;
                }
                if (report != null)
                {
                    report.Dock = DockStyle.Fill;
                    panelDetails.Controls.Add(report);
                    m_controlDetails = report;
                }

                if ((treeViewReport.SelectedNode != null) && (treeViewReport.SelectedNode.Level > 0))
                {
                    buttonApply.Enabled = true;
                }
                //Change label text
                labelItemSettings.Text = string.Format("Item Value ({0})", 
                                                       (listBoxItems.SelectedItem).ToString().Trim());
                panelItemValue.BackColor = Color.LightSteelBlue;
            }
            else
            {
                if (m_controlDetails != null)
                {
                    panelDetails.Controls.Remove(m_controlDetails);
                    CyDeviceDescriptorPage.CleanControls(m_unusedControls);
                    m_unusedControls.Add(m_controlDetails);
                    m_controlDetails = null;
                }
                panelDetails.Visible = false;
                buttonApply.Enabled = false;
            }
        }

        private void AddItemNode(CyHidReportItem item)
        {
            if (item == null) return;

            int index;
            CyDescriptorNode descNode = GetSelectedReportDescriptor();
            CyDescriptorNode newNode;
            if (descNode != null)
            {
                //Find index
                CyDescriptorNode currentNode =
                    m_parameters.m_hidReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
                index = descNode.m_nodes.IndexOf(currentNode) + 1;

                #region Rules

                //Check rules
                bool breakRule = false;

                // 1) Can't add END_COLLECTION in the beginning of the report
                if ((item.Name == CyUSBFSParameters.RPTITEM_END_COLLECTION) && (descNode.m_nodes.Count == 0))
                {
                    MessageBox.Show(Properties.Resources.MSG_RPT_WARNING_1, CyUSBFSParameters.MSG_TITLE_WARNING,
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    breakRule = true;
                }

                if (breakRule)
                {
                    return;
                }

                #endregion Rules

                //Create a new node
                newNode = m_parameters.m_hidReportTree.AddNode(descNode.Key);
                ((CyHIDReportItemDescriptor) newNode.m_value).Item = item;

                //Move a new node to index
                if (index > 0)
                {
                    descNode.m_nodes.Remove(newNode);
                    descNode.m_nodes.Insert(index, newNode);
                }

                RefreshReportTree();
                treeViewReport.SelectedNode = treeViewReport.Nodes.Find(newNode.Key, true)[0];
            }
        }

        private CyDescriptorNode GetSelectedReportDescriptor()
        {
            CyDescriptorNode selectedItem = null;
            TreeNode node;
            if ((treeViewReport.SelectedNode != null) && (treeViewReport.SelectedNode.Level > 0))
            {
                node = treeViewReport.SelectedNode;
                while (node.Level > 1)
                    node = node.Parent;
                selectedItem = m_parameters.m_hidReportTree.GetNodeByKey(node.Name);
            }
            return selectedItem;
        }

        /// <summary>
        /// Puts a necessary usage table to be shown for user 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="usageTable"></param>
        private void SetListForItem(CyHidReportItem item, string[] usageTable)
        {
            item.m_valuesList.Clear();
            for (int i = 0; i < usageTable.Length / 2; i++)
            {
                item.m_valuesList.Add(Convert.ToUInt16(usageTable[i * 2 + 1], 16), usageTable[i * 2]);
            }
        }

        /// <summary>
        /// Looks up for the last Usage Page item in the report and return its type. Its used to define which usage
        /// table to show when the Usage item is selected in the listBoxItems.
        /// </summary>
        /// <returns></returns>
        private UInt16 GetLastUsagePageType()
        {
            UInt16 usagePageVal = 0;
            CyDescriptorNode descNode = GetSelectedReportDescriptor();
            if ((descNode != null) && (treeViewReport.SelectedNode != null))
            {
                CyDescriptorNode currentNode =
                    m_parameters.m_hidReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
                int currentIndex = descNode.m_nodes.IndexOf(currentNode);
                for (int i = currentIndex; i >= 0; i--)
                {

                    if (((CyHIDReportItemDescriptor) descNode.m_nodes[i].m_value).Item.Name == "USAGE_PAGE")
                    {
                        CyHidReportItem usagePageItem = ((CyHIDReportItemDescriptor) descNode.m_nodes[i].m_value).Item;
                        if (usagePageItem.m_value.Count == 2)
                        {
                            usagePageVal = usagePageItem.m_value[1];
                        }
                        break;
                    }
                }
            }
            return usagePageVal;
        }

        /// <summary>
        /// Displays the information about the selected node of treeViewReport in the setting panel
        /// </summary>
        private void ShowEditItem()
        {
            if (treeViewReport.SelectedNode == null) return;

            CyDescriptorNode selectedItem = m_parameters.m_hidReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            CyHidReportItem reportItem;
            if ((selectedItem != null) && (treeViewReport.SelectedNode.Level > 1))
            {
                reportItem = ((CyHIDReportItemDescriptor) selectedItem.m_value).Item;
                if (m_controlDetails != null)
                {
                    panelDetails.Controls.Remove(m_controlDetails);
                    CyDeviceDescriptorPage.CleanControls(m_unusedControls);
                    m_unusedControls.Add(m_controlDetails);
                }
                panelDetails.Visible = true;
                CyReportBase report = null;
                switch (reportItem.Kind)
                {
                    case CyHidReportItemKind.BITS:
                        report = new CyReportBits(reportItem, true);
                        break;
                    case CyHidReportItemKind.LIST:
                        // CDT 79806
                        // If value is vendor-defined and has 2 bytes (version 1_20), update the value to 0xFF00
                        if ((reportItem.m_value.Count == 2) && 
                            !reportItem.m_valuesList.ContainsKey(reportItem.m_value[1]))
                        {
                            MessageBox.Show(String.Format(Properties.Resources.MSG_INCORRECT_VENDOR_VALUE,
                                                          "0x" + reportItem.m_value[1].ToString("X2")),
                                            CyUSBFSParameters.MSG_TITLE_INFORMATION, MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            reportItem.m_value[0]++;
                            reportItem.m_value[1] = 0;
                            reportItem.m_value.Add(0xFF);
                            report_UpdatedItemEvent(this, EventArgs.Empty);
                        }

                        report = new CyReportList(reportItem, true);
                        break;
                    case CyHidReportItemKind.INT:
                        report = new CyReportNumber(reportItem, true);
                        break;
                    case CyHidReportItemKind.UNIT:
                        report = new CyReportUnit(reportItem, true);
                        break;
                    case CyHidReportItemKind.NONE:
                        m_controlDetails = null;
                        break;
                    case CyHidReportItemKind.CUSTOM:
                        report = new CyReportCustom(reportItem, true);
                        break;
                    default:
                        break;
                }
                if (report != null)
                {
                    report.Dock = DockStyle.Fill;
                    report.UpdatedItemEvent += new EventHandler(report_UpdatedItemEvent);
                    panelDetails.Controls.Add(report);
                    m_controlDetails = report;
                }
                //Change label text
                labelItemSettings.Text = string.Format("Item Value ({0})", reportItem.ToString().Trim());
                panelItemValue.BackColor = SystemColors.Control;
            }
            else
            {
                if (m_controlDetails != null)
                {
                    panelDetails.Controls.Remove(m_controlDetails);
                    CyDeviceDescriptorPage.CleanControls(m_unusedControls);
                    m_unusedControls.Add(m_controlDetails);
                    m_controlDetails = null;
                }
                panelDetails.Visible = false;
            }
        }

        /// <summary>
        /// Refreshes the node's text in the treeview and refreshes a header of the settings panel. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void report_UpdatedItemEvent(object sender, EventArgs e)
        {
            m_parameters.ParamHIDReportTreeChanged = true;
            RefreshSelectedNode();
            RefreshSettingsHeader();
        }

        private void TreeViewSelection()
        {
            if (treeViewReport.SelectedNode == null) return;

            toolStripButtonRemove.Enabled = treeViewReport.SelectedNode.Level > 0;

            if (treeViewReport.SelectedNode.Level > 1)
            {
                listBoxItems.SelectedIndex = -1;
                ShowEditItem();
            }
            else if (listBoxItems.SelectedIndex == -1)
            {
                ShowEditItem();
            }

            if ((treeViewReport.SelectedNode.Level > 0) && (listBoxItems.SelectedIndex >= 0))
                buttonApply.Enabled = true;
            else
            {
                buttonApply.Enabled = false;
            }
        }

        private void RemoveReportTreeItem()
        {
            if (treeViewReport.SelectedNode == null) return;

            TreeNode treeNode = treeViewReport.SelectedNode;
            TreeNode prevNode = treeNode.PrevNode;
            if (prevNode == null)
                prevNode = treeNode.Parent;

            if (treeNode.Level < 1) return;

            if (treeNode.Level == 1)
            {
                TreeNode parent = treeNode.Parent;
                if (parent != null)
                    m_parameters.m_hidReportTree.RemoveNode(treeNode.Name, parent.Name);
            }
            else if (treeNode.Level > 1)
            {
                CyDescriptorNode selectedDescriptor = GetSelectedReportDescriptor();
                if (selectedDescriptor != null)
                    m_parameters.m_hidReportTree.RemoveNode(treeNode.Name, selectedDescriptor.Key);
            }
            m_parameters.ParamHIDReportTreeChanged = true;
            RefreshReportTree();
            //Select previous node
            if (prevNode != null)
                treeViewReport.SelectedNode = treeViewReport.Nodes.Find(prevNode.Name, true)[0];
        }

        #endregion Private functions

        #region Events

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (listBoxItems.SelectedItem == null) return;

            CyHidReportItem Item = null;
            if (m_controlDetails != null)
            {
                if (m_controlDetails is CyReportBits)
                {
                    ((CyReportBits) m_controlDetails).Apply();
                    Item = new CyHidReportItem(((CyReportBits) m_controlDetails).m_item);
                }
                else if (m_controlDetails is CyReportList)
                {
                    if (!((CyReportList)m_controlDetails).Apply())
                        return;
                    Item = new CyHidReportItem(((CyReportList) m_controlDetails).m_item);
                }
                else if (m_controlDetails is CyReportNumber)
                {
                    if (!((CyReportNumber) m_controlDetails).Apply())
                        return;
                    Item = new CyHidReportItem(((CyReportNumber) m_controlDetails).m_item);
                }
                else if (m_controlDetails is CyReportUnit)
                {
                    if (!((CyReportUnit) m_controlDetails).Apply())
                        return;
                    Item = new CyHidReportItem(((CyReportUnit) m_controlDetails).m_item);
                }
                else if (m_controlDetails is CyReportCustom)
                {
                    if (!((CyReportCustom) m_controlDetails).Apply())
                        return;
                    Item = new CyHidReportItem(((CyReportCustom) m_controlDetails).m_item);
                }

                // Add to the tree
                AddItemNode(Item);
            }
            else // Kind=NONE
            {
                Item = new CyHidReportItem ((CyHidReportItem) listBoxItems.SelectedItem);
                Item.m_value.Clear();
                Item.m_value.Add(Item.m_prefix);

                if (Item.Name == "COMMENT")
                {
                    Item = new CyHidReportItem("", 0x00, 0, CyHidReportItemKind.NONE);
                }

                AddItemNode(Item);
            }
            m_parameters.ParamHIDReportTreeChanged = true;
        }

        private void CyHIDDescriptor_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                RefreshReportTree();
        }

        private void splitContainerDetails_Panel1_SizeChanged(object sender, EventArgs e)
        {
           
        }

        private void treeViewReport_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewSelection();
        }

        private void treeViewReport_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //TreeViewSelection();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (m_parameters.m_hidReportTree.m_nodes.Count > 0)
            {
                m_parameters.m_hidReportTree.AddNode(m_parameters.m_hidReportTree.m_nodes[0].Key);
                m_parameters.ParamHIDReportTreeChanged = true;
            }
            RefreshReportTree();
            int rptcount = m_parameters.m_hidReportTree.m_nodes[0].m_nodes.Count;
            if (rptcount > 0)
                treeViewReport.SelectedNode = treeViewReport.Nodes[0].Nodes[rptcount-1];
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            RemoveReportTreeItem();
        }

        private void treeViewReport_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Allow only editing of the report titles and comments.
            if (!(((e.Node.Level == 1) ||
                  ((e.Node.Level > 1) && (CyUSBFSParameters.StringIsComment(e.Node.Text))))))
            {
                e.CancelEdit = true;
            }
        }

        private void treeViewReport_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (treeViewReport.SelectedNode == null)
                return;
            CyDescriptorNode node = m_parameters.m_hidReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            if (e.Label == null)
            {
                treeViewReport.SelectedNode.Text = ((CyHIDReportDescriptor)node.m_value).Name;
            }
            else if ((node.m_value is CyHIDReportDescriptor) && (((CyHIDReportDescriptor)node.m_value).Name != e.Label))
            {
                ((CyHIDReportDescriptor) node.m_value).Name = e.Label;
                m_parameters.ParamHIDReportTreeChanged = true;
            }
            // If item is a Comment
            else if ((node.m_value is CyHIDReportItemDescriptor))
            {
                if ((e.Label != "") && (!e.Label.StartsWith("//")))
                {
                    e.Node.Text = "//" + e.Label;
                    e.CancelEdit = true;
                }
                else
                {
                    e.Node.Text = e.Label;
                }
                ((CyHIDReportItemDescriptor)node.m_value).Item.Name = e.Node.Text;
                m_parameters.ParamHIDReportTreeChanged = true;
                report_UpdatedItemEvent(sender, new EventArgs());
            }
        }

        private void treeViewReport_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewReport_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private TreeNode m_oldNode;

        private void treeViewReport_DragOver(object sender, DragEventArgs e)
        {
            Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
            TreeNode aNode = ((TreeView) sender).GetNodeAt(pt);
            if (aNode != null)
            {
                // If the node is a folder, change the color of the background to dark blue to simulate selection
                // Be sure to return the previous node to its original color by copying from a blank node
                if ((aNode.Level > 1))
                {
                    aNode.BackColor = SystemColors.Highlight;
                    aNode.ForeColor = SystemColors.HighlightText;
                    if ((m_oldNode != null) && (m_oldNode != aNode))
                    {
                        m_oldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                        m_oldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
                    }
                    m_oldNode = aNode;
                }
            }
            if ((m_oldNode != null) && (m_oldNode != aNode))
            {
                m_oldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragLeave(object sender, EventArgs e)
        {
            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragDrop(object sender, DragEventArgs e)
        {
            const int PARENT_LEVEL = 1;

            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
            }

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView) sender).GetNodeAt(pt);
                TreeNode NewNode = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode");
                TreeNode DestParent = DestinationNode.Parent;
                while (DestParent.Level > PARENT_LEVEL)
                {
                    DestParent = DestParent.Parent;
                }
                if (DestinationNode != NewNode)
                {
                    CyDescriptorNode sourceDesc = m_parameters.m_hidReportTree.GetNodeByKey(NewNode.Name);
                    CyDescriptorNode destinationDesc = m_parameters.m_hidReportTree.GetNodeByKey(DestinationNode.Name);
                    CyDescriptorNode parent = m_parameters.m_hidReportTree.GetNodeByKey(DestParent.Name);

                    if ((sourceDesc != null) && (destinationDesc != null) && (parent != null) &&
                        (DestinationNode.Level > PARENT_LEVEL) && (NewNode.Level > PARENT_LEVEL))
                    {
                        parent.m_nodes.Remove(sourceDesc);
                        parent.m_nodes.Insert(parent.m_nodes.IndexOf(destinationDesc), sourceDesc);

                        m_parameters.ParamHIDReportTreeChanged = true;
                        RefreshReportTree();
                        treeViewReport.SelectedNode = treeViewReport.Nodes.Find(NewNode.Name, true)[0];
                    }
                }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            CyDescriptorNode descNode = GetSelectedReportDescriptor();
            if (descNode != null)
            {
                m_parameters.SaveReport(descNode);
            }
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeViewReport.SelectedNode;
            if (m_parameters.m_hidReportTree.m_nodes.Count > 0)
            {
                if (m_parameters.ImportReport())
                {
                    treeViewReport.BeginUpdate();
                    RefreshReportTree();
                    treeViewReport.EndUpdate();
                    if (treeNode != null)
                        treeViewReport.SelectedNode = treeViewReport.Nodes.Find(treeNode.Name, true)[0];
                }
            }
        }

        private void treeViewReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveReportTreeItem();
            }
        }


        #endregion Events

        private void panelApply_SizeChanged(object sender, EventArgs e)
        {
            buttonApply.Left = (panelApply.Width - buttonApply.Width)/2;
        }
    }
}
