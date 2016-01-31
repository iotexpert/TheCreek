/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace USBFS_v1_30
{
    public partial class CyHIDDescriptor : UserControl
    {
        #region Variables

        public CyUSBFSParameters Parameters;
        private UserControl controlDetails;
        private List<UserControl> unusedControls;

        public List<HIDReportItem> ReportItems;

        #endregion Variables

        #region Constructors

        public CyHIDDescriptor()
        {
            InitializeComponent();

            Parameters = new CyUSBFSParameters();
            unusedControls = new List<UserControl>();
            InitItems();
            RefreshReportTree();
        }

        public CyHIDDescriptor(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            Parameters = parameters;
            unusedControls = new List<UserControl>();
            InitItems();
            RefreshReportTree();
        }

        #endregion Constructors

        #region Private functions

        private void InitItems()
        {
            ReportItems = new List<HIDReportItem>();
            ReportItems.Add(new HIDReportItem("USAGE", 0x08, 2, HIDReportItemKind.List));
            for (int i = 0; i < CyHIDReportItemTables.ValuesGenericDesktopPage.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(
                    Convert.ToByte(CyHIDReportItemTables.ValuesGenericDesktopPage[i * 2 + 1], 16), CyHIDReportItemTables.ValuesGenericDesktopPage[i * 2]);
            }
            ReportItems.Add(new HIDReportItem("USAGE_PAGE", 0x04, 2, HIDReportItemKind.List));
            for (int i = 0; i < CyHIDReportItemTables.ValuesUsagePage.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(Convert.ToUInt16(CyHIDReportItemTables.ValuesUsagePage[i * 2 + 1], 16),
                                                                  CyHIDReportItemTables.ValuesUsagePage[i * 2]);
            }
            ReportItems.Add(new HIDReportItem("USAGE_MINIMUM", 0x18, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("USAGE_MAXIMUM", 0x28, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("DESIGNATOR_INDEX", 0x38, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("DESIGNATOR_MINIMUM", 0x48, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("DESIGNATOR_MAXIMUM", 0x58, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("STRING_INDEX", 0x78, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("STRING_MINIMUM", 0x88, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("STRING_MAXIMUM", 0x98, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("COLLECTION", 0xA0, 2, HIDReportItemKind.List));
            for (int i = 0; i < CyHIDReportItemTables.ValuesCollection.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesCollection[i * 2 + 1], 16),
                                                                  CyHIDReportItemTables.ValuesCollection[i * 2]);
            }
            ReportItems.Add(new HIDReportItem("END_COLLECTION", 0xC0, 0, HIDReportItemKind.None));
            ReportItems.Add(new HIDReportItem("INPUT", 0x80, 2, HIDReportItemKind.Bits));
            for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                                                  CyHIDReportItemTables.ValuesInput[i * 2]);
            }
            ReportItems.Add(new HIDReportItem("OUTPUT", 0x90, 2, HIDReportItemKind.Bits));
            for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                                                  CyHIDReportItemTables.ValuesInput[i * 2]);
            }
            ReportItems[ReportItems.Count - 1].ValuesList[14] = "Non Volatile";
            ReportItems[ReportItems.Count - 1].ValuesList[15] = "Volatile";
            ReportItems.Add(new HIDReportItem("FEATURE", 0xB0, 2, HIDReportItemKind.Bits));
            for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                                                  CyHIDReportItemTables.ValuesInput[i * 2]);
            }
            ReportItems[ReportItems.Count - 1].ValuesList[14] = "Non Volatile";
            ReportItems[ReportItems.Count - 1].ValuesList[15] = "Volatile";
            ReportItems.Add(new HIDReportItem("LOGICAL_MINIMUM", 0x14, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("LOGICAL_MAXIMUM", 0x24, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("PHYSICAL_MINIMUM", 0x34, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("PHYSICAL_MAXIMUM", 0x44, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("UNIT_EXPONENT", 0x54, 2, HIDReportItemKind.List));
            for (int i = 0; i < CyHIDReportItemTables.ValuesUnitExponent.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesUnitExponent[i * 2 + 1], 16),
                                                                  CyHIDReportItemTables.ValuesUnitExponent[i * 2]);
            }
            ReportItems.Add(new HIDReportItem("UNIT", 0x64, 2, HIDReportItemKind.Unit));
            ReportItems.Add(new HIDReportItem("REPORT_SIZE", 0x74, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("REPORT_ID", 0x84, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("REPORT_COUNT", 0x94, 2, HIDReportItemKind.Int));
            ReportItems.Add(new HIDReportItem("PUSH", 0xA4, 0, HIDReportItemKind.None));
            ReportItems.Add(new HIDReportItem("POP", 0xB4, 0, HIDReportItemKind.None));
            ReportItems.Add(new HIDReportItem("DELIMITER", 0xA8, 1, HIDReportItemKind.List));
            for (int i = 0; i < CyHIDReportItemTables.ValuesDelimiter.Length / 2; i++)
            {
                ReportItems[ReportItems.Count - 1].ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesDelimiter[i * 2 + 1], 16),
                                                                  CyHIDReportItemTables.ValuesDelimiter[i * 2]);
            }
            ReportItems.Add(new HIDReportItem("CUSTOM", 0x00, 0, HIDReportItemKind.Custom));
            ReportItems.Add(new HIDReportItem("COMMENT", 0x00, 0, HIDReportItemKind.None));

            listBoxItems.Items.Clear();
            for (int i = 0; i < ReportItems.Count; i++)
            {
                listBoxItems.Items.Add(ReportItems[i]);
            }
            listBoxItems.SelectedIndex = 0;
        }

        private void RefreshReportTree()
        {
            treeViewReport.BeginUpdate();
            treeViewReport.Nodes.Clear();
            foreach (DescriptorNode descriptor in Parameters.HIDReportTree.Nodes)
            {
                TreeNode node = treeViewReport.Nodes.Add(descriptor.Key, DescriptorNode.GetDescriptorLabel(descriptor),
                                                         0, 0);
                RefreshNode(descriptor, node);
            }

            treeViewReport.ExpandAll();
            treeViewReport.EndUpdate();
            if (treeViewReport.Nodes.Count > 0)
            {
                treeViewReport.SelectedNode = treeViewReport.Nodes[0].FirstNode;
            }
        }

        private void RefreshNode(DescriptorNode descriptor, TreeNode treeNode)
        {
            int imageIndex = 0;
            Stack<TreeNode> CollectionNodes = new Stack<TreeNode>();
            //CollectionNodes.Push(treeNode);
            TreeNode collectionNode = treeNode;
            foreach (DescriptorNode descriptor_child in descriptor.Nodes)
            {
                switch (descriptor_child.Value.bDescriptorType)
                {
                    case USBDescriptorType.HID_REPORT:
                        imageIndex = 3;
                        break;
                    case USBDescriptorType.HID_REPORT_ITEM:
                        imageIndex = 5;
                        break;
                }

                TreeNode node = collectionNode.Nodes.Add(descriptor_child.Key,
                                                         DescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex,
                                                         imageIndex);
                // Draw comments in a different color
                if (CyUSBFSParameters.StringIsComment(node.Text))
                {
                    node.ForeColor = Color.Green;
                    node.ImageIndex = 20;
                    node.SelectedImageIndex = 20;
                }

                if ((descriptor_child.Value.bDescriptorType == USBDescriptorType.HID_REPORT_ITEM) &&
                    (((HIDReportItemDescriptor) descriptor_child.Value).Item.Name == "COLLECTION"))
                {
                    CollectionNodes.Push(collectionNode);
                    collectionNode = node;
                }
                else if ((descriptor_child.Value.bDescriptorType == USBDescriptorType.HID_REPORT_ITEM) &&
                         (((HIDReportItemDescriptor) descriptor_child.Value).Item.Name == "END_COLLECTION"))
                {
                    if (CollectionNodes.Count > 0)
                        collectionNode = CollectionNodes.Pop();
                }
                RefreshNode(descriptor_child, node);
            }
        }

        public void RefreshSelectedNode()
        {
            DescriptorNode node = Parameters.HIDReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            try
            {
                HIDReportItem item = ((HIDReportItemDescriptor)node.Value).Item;
                if (item.Kind != HIDReportItemKind.Custom)
                    treeViewReport.SelectedNode.Text = ((HIDReportItemDescriptor)node.Value).Item.ToString();
                else
                    treeViewReport.SelectedNode.Text = ((HIDReportItemDescriptor)node.Value).Item.ToCustomString();
            }
            catch
            {
                // If applied to another item type, do nothing
            }
        }

        public void RefreshSettingsHeader()
        {
            DescriptorNode selectedItem = Parameters.HIDReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            HIDReportItem reportItem;
            if ((selectedItem != null) && (treeViewReport.SelectedNode.Level > 1))
            {
                reportItem = ((HIDReportItemDescriptor) selectedItem.Value).Item;
                labelItemSettings.Text = "Item Value (" + reportItem.ToString().Trim() + ")";
            }
        }

        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxItems.SelectedIndex >= 0)
            {
                if (controlDetails != null)
                {
                    panelDetails.Controls.Remove(controlDetails);
                    CyDeviceDescriptor.CleanControls(unusedControls);
                    unusedControls.Add(controlDetails);
                }
                panelDetails.Visible = true;

                // If Selected Item is USAGE, choose the appropriate list
                 //"Generic Desktop Controls", "01",
                 //"Simulation Controls", "02",
                 //"VR Controls", "03",
                 //"Sport Controls", "04",
                 //"Game Controls", "05",
                 //"Generic Device Controls", "06",
                 //"Keyboard/Keypad", "07",
                 //"LEDs", "08",
                 //"Button", "09",
                 //"Ordinal", "0A",
                 //"Telephony Devices", "0B",
                 //"Consumer Devices", "0C",
                 //"Digitizer", "0D",
                 //"Physical Input Device (PID)", "0F",
                 //"Unicode", "10",
                 //"Alphanumeric Display", "14",
                 //"Medical Instruments", "40",
                 //"Monitor Devices", "80",
                 //"Monitor Enumerated Values", "81",
                 //"VESA Virtual Controls", "82",
                 //"VESA Command", "83", 
                 //"Power Device", "84",
                 //"Battery System", "85", 
                 //"Vendor Defined Page", "FF00"
                if (((HIDReportItem) listBoxItems.SelectedItem).Name  == "USAGE")
                {
                    UInt16 lastUsagePage = GetLastUsagePageType();
                    switch (lastUsagePage)
                    {
                        case 0x01: // Generic Desktop Controls
                            SetListForItem((HIDReportItem) listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGenericDesktopPage);
                            break;
                        case 0x02: // Simulation Controls
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesSimulationControlsPage);
                            break;
                        case 0x03: // VR Controls
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesVRControlPage);
                            break;
                        case 0x04: // Sport Controls
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesSportControlPage);
                            break;
                        case 0x05: // Game Controls
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGameControlsPage);
                            break;
                        case 0x06: // Generic Device Controls
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGenericDeviceControlsPage);
                            break;
                        case 0x07: // Keyboard/Keypad
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesKeyboardPage);
                            break;
                        case 0x08: // LEDs
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesLEDPage);
                            break;
                        case 0x09: // Button
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesButtonPage);
                            break;
                        case 0x0A: // Ordinal
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesOrdinalPage);
                            break;
                        case 0x0B: // Telephony Devices
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesTelephonyDevicePage);
                            break;
                        case 0x0C: // Consumer Devices
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesConsumerPage);
                            break;
                        case 0x0D: // Digitizer
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesDigitizersPage);
                            break;
                        case 0x0F: // Physical Input Device (PID)
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesPIDPage);
                            break;
                        case 0x10: // Unicode
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesUnicodePage);
                            break;
                        case 0x14: // Alphanumeric Display
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesAlphanumericDisplayPage);
                            break;
                        case 0x40: // Medical Instruments
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesMedicalInstrumentPage);
                            break;
                        case 0x80: // Monitor Devices
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesMonitorDevicesPage);
                            break;
                        case 0x81: // Monitor Enumerated Values
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesMonitorEnumeratedValuesPage); 
                            break;
                        case 0x82: // VESA Virtual Controls
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesVESAVirtualControlsPage);
                            break;
                        case 0x84: // Power Device
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesPowerDevicePage);
                            break;
                        case 0x85: // Battery System
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesBatterySystemPage);
                            break;
                        default:
                            SetListForItem((HIDReportItem)listBoxItems.SelectedItem,
                                           CyHIDReportItemTables.ValuesGenericDesktopPage);
                            break;
                    }
                }

                switch (((HIDReportItem) listBoxItems.SelectedItem).Kind)
                {
                    case HIDReportItemKind.Bits:
                        CyReportBits report_bits =
                            new CyReportBits((HIDReportItem) ((HIDReportItem) listBoxItems.SelectedItem).Clone(), false);
                        report_bits.Dock = DockStyle.Fill;
                        panelDetails.Controls.Add(report_bits);
                        controlDetails = report_bits;
                        break;
                    case HIDReportItemKind.List:
                        CyReportList report_list =
                            new CyReportList((HIDReportItem) ((HIDReportItem) listBoxItems.SelectedItem).Clone(), false);
                        report_list.Dock = DockStyle.Fill;
                        panelDetails.Controls.Add(report_list);
                        controlDetails = report_list;
                        break;
                    case HIDReportItemKind.Int:
                        CyReportNumber report_number =
                            new CyReportNumber((HIDReportItem) ((HIDReportItem) listBoxItems.SelectedItem).Clone(),
                                               false);
                        report_number.Dock = DockStyle.Fill;
                        panelDetails.Controls.Add(report_number);
                        controlDetails = report_number;
                        break;
                    case HIDReportItemKind.Unit:
                        CyReportUnit report_unit =
                            new CyReportUnit((HIDReportItem) ((HIDReportItem) listBoxItems.SelectedItem).Clone(), false);
                        report_unit.Dock = DockStyle.Fill;
                        panelDetails.Controls.Add(report_unit);
                        controlDetails = report_unit;
                        break;
                    case HIDReportItemKind.None:
                        controlDetails = null;
                        break;
                    case HIDReportItemKind.Custom:
                        CyReportCustom report_custom =
                            new CyReportCustom((HIDReportItem) ((HIDReportItem) listBoxItems.SelectedItem).Clone(),
                                               false);
                        report_custom.Dock = DockStyle.Fill;
                        panelDetails.Controls.Add(report_custom);
                        controlDetails = report_custom;
                        break;
                    default:
                        break;
                }
                if ((treeViewReport.SelectedNode != null) && (treeViewReport.SelectedNode.Level > 0))
                {
                    buttonApply.Enabled = true;
                }
                //Change label text
                labelItemSettings.Text = "Item Value (" + (listBoxItems.SelectedItem).ToString().Trim() + ")";
                splitContainerDetails.Panel2.BackColor = Color.LightSteelBlue;
            }
            else
            {
                if (controlDetails != null)
                {
                    panelDetails.Controls.Remove(controlDetails);
                    CyDeviceDescriptor.CleanControls(unusedControls);
                    unusedControls.Add(controlDetails);
                    controlDetails = null;
                }
                panelDetails.Visible = false;
                buttonApply.Enabled = false;
            }
        }

        private void AddItemNode(HIDReportItem item)
        {
            if (item == null) return;

            int index;
            DescriptorNode descNode = GetSelectedReportDescriptor();
            DescriptorNode newNode;
            if (descNode != null)
            {
                //Find index
                DescriptorNode currentNode = Parameters.HIDReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
                index = descNode.Nodes.IndexOf(currentNode) + 1;

                #region Rules

                //Check rules
                bool breakRule = false;

                // 1) Can't add END_COLLECTION in the beginning of the report
                if ((item.Name == "END_COLLECTION") && (descNode.Nodes.Count == 0))
                {
                    MessageBox.Show("END_COLLECTION could not be added at the beginning of the report.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    breakRule = true;
                }

                if (breakRule)
                {
                    return;
                }

                #endregion Rules

                //Create a new node
                newNode = Parameters.HIDReportTree.AddNode(descNode.Key);
                ((HIDReportItemDescriptor) newNode.Value).Item = item;

                //Move a new node to index
                if (index > 0)
                {
                    descNode.Nodes.Remove(newNode);
                    descNode.Nodes.Insert(index, newNode);
                }

                RefreshReportTree();
                treeViewReport.SelectedNode = treeViewReport.Nodes.Find(newNode.Key, true)[0];
            }
        }

        private void AddItemNode(HIDReportItem item, DescriptorNode descNode)
        {
            if ((item == null) || (descNode == null)) return;

            descNode = Parameters.HIDReportTree.AddNode(descNode.Key);
            ((HIDReportItemDescriptor) descNode.Value).Item = item;
            RefreshReportTree();
            treeViewReport.SelectedNode = treeViewReport.Nodes.Find(descNode.Key, true)[0];
        }

        private DescriptorNode GetSelectedReportDescriptor()
        {
            DescriptorNode selectedItem = null;
            TreeNode node;
            if ((treeViewReport.SelectedNode != null) && (treeViewReport.SelectedNode.Level > 0))
            {
                node = treeViewReport.SelectedNode;
                while (node.Level > 1)
                    node = node.Parent;
                selectedItem = Parameters.HIDReportTree.GetNodeByKey(node.Name);
            }
            return selectedItem;
        }

        private void SetListForItem(HIDReportItem item, string[] usageTable)
        {
            item.ValuesList.Clear();
            for (int i = 0; i < usageTable.Length / 2; i++)
            {
                item.ValuesList.Add(Convert.ToUInt16(usageTable[i * 2 + 1], 16), usageTable[i * 2]);
            }
        }

        private UInt16 GetLastUsagePageType()
        {
            UInt16 usagePageVal = 0;
            DescriptorNode descNode = GetSelectedReportDescriptor();
            if ((descNode != null) && (treeViewReport.SelectedNode != null))
            {
                DescriptorNode currentNode = Parameters.HIDReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
                int currentIndex = descNode.Nodes.IndexOf(currentNode);
                for (int i = currentIndex; i >= 0; i--)
                {

                    if (((HIDReportItemDescriptor) descNode.Nodes[i].Value).Item.Name == "USAGE_PAGE")
                    {
                        HIDReportItem usagePageItem = ((HIDReportItemDescriptor) descNode.Nodes[i].Value).Item;
                        if (usagePageItem.Value.Count == 2)
                        {
                            usagePageVal = usagePageItem.Value[1];
                        }
                        break;
                    }
                }
            }
            return usagePageVal;
        }

        private void SaveReport(DescriptorNode node_report)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML Files|*.xml";
            saveFileDialog1.Title = "Save a Template File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter strw = new StreamWriter(saveFileDialog1.FileName))
                {
                    ReportTemplate report;
                    XmlSerializer s = new XmlSerializer(typeof (ReportTemplate));
                    StringWriter sw = new StringWriter();
                    string serializedXml;

                    report = new ReportTemplate(
                        ((HIDReportDescriptor) node_report.Value).Name);
                    for (int j = 0; j < node_report.Nodes.Count; j++)
                    {
                        HIDReportItem item =
                            ((HIDReportItemDescriptor) node_report.Nodes[j].Value).Item;
                        List<byte> byteList = new List<byte>(item.Value);
                        if (byteList.Count > 0)
                            byteList.RemoveAt(0);
                        long val = 0;
                        if ((item.Kind == HIDReportItemKind.Int) || (item.Kind == HIDReportItemKind.Custom))
                        {
                            val = CyUSBFSParameters.ConvertByteArrayToInt(byteList);
                        }
                        else
                        {
                            if (BitConverter.IsLittleEndian)
                            {
                                for (int k = 1; k < item.Value.Count; k++)
                                {
                                    val += item.Value[k] << (k - 1)*8;
                                }
                            }
                            else
                            {
                                for (int k = 1; k < item.Value.Count; k++)
                                {
                                    val += item.Value[k] << (item.Value.Count - k - 1)*8;
                                }
                            }
                        }
                        report.Items.Add(new ReportTemplateItem(item.Name, item.Prefix, item.Size, val, 
                                                                item.Description));
                    }
                    s.Serialize(sw, report);
                    serializedXml = sw.ToString();
                    strw.WriteLine(serializedXml);
                }
            }
        }

        /// <summary>
        /// Imports a HID Report from the file.
        /// </summary>
        /// <returns>ReportTemplate class</returns>
        private ReportTemplate ImportReport()
        {
            ReportTemplate result = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML Files|*.xml";
            openFileDialog1.Title = "Open a Template File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line, serializedstr = "";
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = sr.ReadLine()) != null)
                        {
                            serializedstr += line;
                        }

                        XmlSerializer s = new XmlSerializer(typeof (ReportTemplate));
                        result = (ReportTemplate) s.Deserialize(new StringReader(serializedstr));
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to load the report.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return result;
        }

        private void ShowEditItem()
        {
            if (treeViewReport.SelectedNode == null) return;

            DescriptorNode selectedItem = Parameters.HIDReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            HIDReportItem reportItem;
            if ((selectedItem != null) && (treeViewReport.SelectedNode.Level > 1))
            {
                reportItem = ((HIDReportItemDescriptor) selectedItem.Value).Item;
                if (controlDetails != null)
                {
                    panelDetails.Controls.Remove(controlDetails);
                    CyDeviceDescriptor.CleanControls(unusedControls);
                    unusedControls.Add(controlDetails);
                }
                panelDetails.Visible = true;
                switch (reportItem.Kind)
                {
                    case HIDReportItemKind.Bits:
                        CyReportBits report_bits = new CyReportBits(reportItem, true);
                        report_bits.Dock = DockStyle.Fill;
                        report_bits.UpdatedItemEvent += new EventHandler(report_UpdatedItemEvent);
                        panelDetails.Controls.Add(report_bits);
                        controlDetails = report_bits;
                        break;
                    case HIDReportItemKind.List:
                        CyReportList report_list = new CyReportList(reportItem, true);
                        report_list.Dock = DockStyle.Fill;
                        report_list.UpdatedItemEvent += new EventHandler(report_UpdatedItemEvent);
                        panelDetails.Controls.Add(report_list);
                        controlDetails = report_list;
                        break;
                    case HIDReportItemKind.Int:
                        CyReportNumber report_number = new CyReportNumber(reportItem, true);
                        report_number.Dock = DockStyle.Fill;
                        report_number.UpdatedItemEvent += new EventHandler(report_UpdatedItemEvent);
                        panelDetails.Controls.Add(report_number);
                        controlDetails = report_number;
                        break;
                    case HIDReportItemKind.Unit:
                        CyReportUnit report_unit = new CyReportUnit(reportItem, true);
                        report_unit.Dock = DockStyle.Fill;
                        report_unit.UpdatedItemEvent += new EventHandler(report_UpdatedItemEvent);
                        panelDetails.Controls.Add(report_unit);
                        controlDetails = report_unit;
                        break;
                    case HIDReportItemKind.None:
                        controlDetails = null;
                        break;
                    case HIDReportItemKind.Custom:
                        CyReportCustom report_custom = new CyReportCustom(reportItem, true);
                        report_custom.Dock = DockStyle.Fill;
                        report_custom.UpdatedItemEvent += new EventHandler(report_UpdatedItemEvent);
                        panelDetails.Controls.Add(report_custom);
                        controlDetails = report_custom;
                        break;
                    default:
                        break;
                }
                //Change label text
                labelItemSettings.Text = "Item Value (" + reportItem.ToString().Trim() + ")";
                splitContainerDetails.Panel2.BackColor = SystemColors.Control;
            }
            else
            {
                if (controlDetails != null)
                {
                    panelDetails.Controls.Remove(controlDetails);
                    CyDeviceDescriptor.CleanControls(unusedControls);
                    unusedControls.Add(controlDetails);
                    controlDetails = null;
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
            Parameters.ParamsChanged = true;
            RefreshSelectedNode();
            RefreshSettingsHeader();
        }

        private void TreeViewSelection()
        {
            if (treeViewReport.SelectedNode == null) return;

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
                    Parameters.HIDReportTree.RemoveNode(treeNode.Name, parent.Name);
            }
            else if (treeNode.Level > 1)
            {
                DescriptorNode selectedDescriptor = GetSelectedReportDescriptor();
                if (selectedDescriptor != null)
                    Parameters.HIDReportTree.RemoveNode(treeNode.Name, selectedDescriptor.Key);
            }
            Parameters.ParamsChanged = true;
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

            HIDReportItem Item = null;
            if (controlDetails != null)
            {
                if (controlDetails is CyReportBits)
                {
                    ((CyReportBits) controlDetails).Apply();
                    Item = (HIDReportItem) ((CyReportBits) controlDetails).Item.Clone();
                }
                else if (controlDetails is CyReportList)
                {
                    if (!((CyReportList)controlDetails).Apply())
                        return;
                    Item = (HIDReportItem) ((CyReportList) controlDetails).Item.Clone();
                }
                else if (controlDetails is CyReportNumber)
                {
                    if (!((CyReportNumber) controlDetails).Apply())
                        return;
                    Item = (HIDReportItem) ((CyReportNumber) controlDetails).Item.Clone();
                }
                else if (controlDetails is CyReportUnit)
                {
                    if (!((CyReportUnit) controlDetails).Apply())
                        return;
                    Item = (HIDReportItem) ((CyReportUnit) controlDetails).Item.Clone();
                }
                else if (controlDetails is CyReportCustom)
                {
                    if (!((CyReportCustom) controlDetails).Apply())
                        return;
                    Item = (HIDReportItem) ((CyReportCustom) controlDetails).Item.Clone();
                }

                // Add to the tree
                AddItemNode(Item);
            }
            else // Kind=None
            {
                Item = (HIDReportItem) ((HIDReportItem) listBoxItems.SelectedItem).Clone();
                Item.Value.Clear();
                Item.Value.Add(Item.Prefix);

                if (Item.Name == "COMMENT")
                {
                    Item = new HIDReportItem("", 0x00, 0, HIDReportItemKind.None);
                }

                AddItemNode(Item);
            }
            Parameters.ParamsChanged = true;
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
            if (Parameters.HIDReportTree.Nodes.Count > 0)
            {
                Parameters.HIDReportTree.AddNode(Parameters.HIDReportTree.Nodes[0].Key);
                Parameters.ParamsChanged = true;
            }
            RefreshReportTree();
            int rptcount = Parameters.HIDReportTree.Nodes[0].Nodes.Count;
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
            DescriptorNode node = Parameters.HIDReportTree.GetNodeByKey(treeViewReport.SelectedNode.Name);
            if (e.Label == null)
            {
                treeViewReport.SelectedNode.Text = ((HIDReportDescriptor)node.Value).Name;
            }
            else if ((node.Value is HIDReportDescriptor) && (((HIDReportDescriptor)node.Value).Name != e.Label))
            {
                ((HIDReportDescriptor) node.Value).Name = e.Label;
                Parameters.ParamsChanged = true;
            }
            // If item is a Comment
            else if ((node.Value is HIDReportItemDescriptor))
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
                ((HIDReportItemDescriptor)node.Value).Item.Name = e.Node.Text;
                Parameters.ParamsChanged = true;
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

        private TreeNode OldNode;

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
                    aNode.BackColor = Color.LightBlue;
                    aNode.ForeColor = Color.White;
                    if ((OldNode != null) && (OldNode != aNode))
                    {
                        OldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                        OldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
                    }
                    OldNode = aNode;
                }
            }
            if ((OldNode != null) && (OldNode != aNode))
            {
                OldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                OldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragLeave(object sender, EventArgs e)
        {
            if (OldNode != null)
            {
                OldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                OldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragDrop(object sender, DragEventArgs e)
        {
            const int PARENT_LEVEL = 1;

            if (OldNode != null)
            {
                OldNode.BackColor = treeViewReport.Nodes[0].BackColor;
                OldNode.ForeColor = treeViewReport.Nodes[0].ForeColor;
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
                    DescriptorNode sourceDesc = Parameters.HIDReportTree.GetNodeByKey(NewNode.Name);
                    DescriptorNode destinationDesc = Parameters.HIDReportTree.GetNodeByKey(DestinationNode.Name);
                    DescriptorNode parent = Parameters.HIDReportTree.GetNodeByKey(DestParent.Name);

                    if ((sourceDesc != null) && (destinationDesc != null) && (parent != null) &&
                        /*(NewNode.Level == DestinationNode.Level)*/
                        (DestinationNode.Level > PARENT_LEVEL) && (NewNode.Level > PARENT_LEVEL))
                    {
                        parent.Nodes.Remove(sourceDesc);
                        parent.Nodes.Insert(parent.Nodes.IndexOf(destinationDesc), sourceDesc);

                        Parameters.ParamsChanged = true;
                        RefreshReportTree();
                        treeViewReport.SelectedNode = treeViewReport.Nodes.Find(NewNode.Name, true)[0];
                    }
                }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            DescriptorNode descNode = GetSelectedReportDescriptor();
            if (descNode != null)
            {
                SaveReport(descNode);
            }
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            treeViewReport.BeginUpdate();
            TreeNode treeNode = treeViewReport.SelectedNode;
            DescriptorNode newNode;
            ReportTemplate report;
            if (Parameters.HIDReportTree.Nodes.Count > 0)
            {
                report = ImportReport();
                if (report != null)
                {
                    newNode = Parameters.HIDReportTree.AddNode(Parameters.HIDReportTree.Nodes[0].Key);
                    //Report name
                    ((HIDReportDescriptor) newNode.Value).Name = report.ReportName;
                    //Items
                    for (int i = 0; i < report.Items.Count; i++)
                    {
                        HIDReportItem Item = new HIDReportItem(report.Items[i].Type, (byte) report.Items[i].Code,
                                                               report.Items[i].Size, report.Items[i].Value,
                                                               report.Items[i].Description);
                        AddItemNode(Item, newNode);
                    }
                    Parameters.ParamsChanged = true;
                }
            }
            RefreshReportTree();
            treeViewReport.EndUpdate();
            if (treeNode != null)
                treeViewReport.SelectedNode = treeViewReport.Nodes.Find(treeNode.Name, true)[0];
        }

        #region ToolTips for ListBox

        private int lastListBoxIndex = -1;

        private void listBoxItems_MouseMove(object sender, MouseEventArgs e)
        {
            int index = listBoxItems.IndexFromPoint(listBoxItems.PointToClient(MousePosition));
            if (index >= 0)
            {
                if (index != lastListBoxIndex)
                {
                    lastListBoxIndex = index;
                    SizeF strSize =
                        listBoxItems.CreateGraphics().MeasureString(listBoxItems.Items[index].ToString().TrimEnd(),
                                                                    listBoxItems.Font);
                    if (listBoxItems.Width < strSize.Width + 10)
                    {
                        toolTipList.SetToolTip(listBoxItems, listBoxItems.Items[index].ToString().TrimEnd());
                    }
                    else
                    {
                        toolTipList.SetToolTip(listBoxItems, "");
                    }
                }
            }
            else
            {
                toolTipList.SetToolTip(listBoxItems, "");
            }
        }

        private void listBoxItems_MouseLeave(object sender, EventArgs e)
        {
            toolTipList.SetToolTip(listBoxItems, "");
        }

        #endregion ToolTips for ListBox

        private void treeViewReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveReportTreeItem();
            }
        }


        #endregion Events
    }

    #region Report Template

    /// <summary>
    /// Used for saving and importing HID reports.
    /// </summary>
    [XmlRoot("HID_Report")]
    public class ReportTemplate
    {
        [XmlAttribute("Report_Name")] public string ReportName;
        [XmlArray("Items")] [XmlArrayItem("HID_Item")] public List<ReportTemplateItem> Items;

        public ReportTemplate()
        {
            Items = new List<ReportTemplateItem>();
        }

        public ReportTemplate(string name)
        {
            Items = new List<ReportTemplateItem>();
            ReportName = name;
        }
    }

    /// <summary>
    /// Used for saving and importing HID report items.
    /// </summary>
    public class ReportTemplateItem
    {
        [XmlAttribute("Type")] public string Type;
        [XmlAttribute("Code")] public int Code;
        [XmlAttribute("Size")] public int Size;
        [XmlAttribute("Value")] public long Value;
        [XmlAttribute("Desc")] public string Description;

        public ReportTemplateItem()
        {
        }

        public ReportTemplateItem(string type, int code, int size, long value, string description)
        {
            this.Type = type;
            this.Code = code;
            this.Size = size;
            this.Value = value;
            this.Description = description;
        }
    }

    #endregion Report Template
}