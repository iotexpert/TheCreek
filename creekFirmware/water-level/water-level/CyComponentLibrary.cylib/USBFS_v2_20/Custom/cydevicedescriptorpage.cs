/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace USBFS_v2_20
{
    public partial class CyDeviceDescriptorPage : UserControl
    {
        #region Constants

        private const int LEVEL_DEVICE = 1;
        private const int LEVEL_CONFIGURATION = 2;
        private const int LEVEL_INTERFACE = 4;

        #endregion Constants

        #region Variables

        public CyUSBFSParameters m_parameters;
        private UserControl m_controlDetails;
        private readonly List<UserControl> m_unusedControls;

        #endregion Variables

        #region Constructors

        public CyDeviceDescriptorPage()
        {
            InitializeComponent();
        }

        public CyDeviceDescriptorPage(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            RefreshDeviceTree();

            m_unusedControls = new List<UserControl>();
            ToolStripManager.Renderer =
                new ToolStripProfessionalRenderer(new CyCustomToolStripColors());
        }

        #endregion Constructors

        #region General functions

        /// <summary>
        /// Rebuilds Device Descriptors TreeView based on m_deviceTree
        /// </summary>
        private void RefreshDeviceTree()
        {
            treeViewDevice.BeginUpdate();
            treeViewDevice.Nodes.Clear();
            for (int i = 0; i < m_parameters.m_deviceTree.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor = m_parameters.m_deviceTree.m_nodes[i];
                TreeNode node = treeViewDevice.Nodes.Add(descriptor.Key, 
                                                         CyDescriptorNode.GetDescriptorLabel(descriptor), 0, 0);
                RefreshNode(descriptor, node);
            }
            treeViewDevice.ExpandAll();
            SelectedNodeChanged(treeViewDevice.SelectedNode);
            treeViewDevice.EndUpdate();
        }
        /// <summary>
        /// Recursive function that supplement RefreshDeviceTree() function
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="treeNode"></param>
        private void RefreshNode(CyDescriptorNode descriptor, TreeNode treeNode)
        {
            const int AUDIO_IMAGE_INDEX = 3;
            const int CDC_IMAGE_INDEX = 4;

            int imageIndex = 0;
            for (int i = 0; i < descriptor.m_nodes.Count; i++)
            {
                Color foreColor = SystemColors.ControlText;
                CyDescriptorNode descriptor_child = descriptor.m_nodes[i];
                TreeNode node;
                if (descriptor_child.m_value == null)
                {
                    switch (descriptor_child.Key)
                    {
                        default:
                            if (descriptor_child.Key.Contains(CyUSBFSParameters.NODEKEY_INTERFACE))
                                imageIndex = 2;
                            break;
                    }
                    node = treeNode.Nodes.Add(descriptor_child.Key,
                                              CyDescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex, 
                                              imageIndex);
                }
                else
                {
                    switch (descriptor_child.m_value.bDescriptorType)
                    {
                        case CyUSBDescriptorType.DEVICE:
                            imageIndex = 1;
                            break;
                        case CyUSBDescriptorType.AUDIO:
                        case CyUSBDescriptorType.CS_ENDPOINT:
                            foreColor = SystemColors.GrayText;
                            imageIndex = AUDIO_IMAGE_INDEX;
                            break;
                        case CyUSBDescriptorType.CDC:
                            foreColor = SystemColors.GrayText;
                            imageIndex = CDC_IMAGE_INDEX;
                            break;
                        case CyUSBDescriptorType.INTERFACE:
                             if ((descriptor_child.m_value is CyAudioInterfaceDescriptor) ||
                                 (descriptor_child.m_value is CyCDCInterfaceDescriptor))
                             {
                                 int imgIndex = (descriptor_child.m_value is CyAudioInterfaceDescriptor)
                                                    ? AUDIO_IMAGE_INDEX : CDC_IMAGE_INDEX;
                                 foreColor = SystemColors.GrayText;
                                 imageIndex = imgIndex;
                                 treeNode.ForeColor = foreColor;
                                 treeNode.ImageIndex = imgIndex;
                                 treeNode.SelectedImageIndex = imgIndex;
                             }
                             else
                                 imageIndex = 2;
                            break;
                        case CyUSBDescriptorType.ALTERNATE:
                        case CyUSBDescriptorType.CONFIGURATION:
                        case CyUSBDescriptorType.HID:
                            imageIndex = 2;
                            break;
                        case CyUSBDescriptorType.ENDPOINT:
                            imageIndex = 2;
                            if (descriptor_child.m_value is CyAudioEndpointDescriptor)
                            {
                                imageIndex = AUDIO_IMAGE_INDEX;
                                foreColor = SystemColors.GrayText;
                            }
                            else if ((descriptor.m_value != null) && (descriptor.m_value is CyCDCInterfaceDescriptor))
                            {
                                imageIndex = CDC_IMAGE_INDEX;
                                foreColor = SystemColors.GrayText;
                            }
                            break;
                        default:
                            Debug.Assert(false, "unhandled");
                            break;
                    }


                    if (descriptor_child.m_value.bDescriptorType == CyUSBDescriptorType.HID)
                        node = treeNode.Nodes.Insert(0, descriptor_child.Key,
                                                     CyDescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex,
                                                     imageIndex);
                    else if ((descriptor_child.m_value.bDescriptorType == CyUSBDescriptorType.AUDIO) ||
                             (descriptor_child.m_value.bDescriptorType == CyUSBDescriptorType.CDC))
                    {
                        int nodeIndex = 0;
                        for (int j = 0; j < descriptor.m_nodes.IndexOf(descriptor_child); j++)
                        {
                            if ((descriptor.m_nodes[j].m_value.bDescriptorType == CyUSBDescriptorType.AUDIO) ||
                                (descriptor.m_nodes[j].m_value.bDescriptorType == CyUSBDescriptorType.CDC))
                            {
                                nodeIndex++;
                            }
                        }
                        node = treeNode.Nodes.Insert(nodeIndex, descriptor_child.Key,
                                                     CyDescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex,
                                                     imageIndex);
                    }
                    else
                        node = treeNode.Nodes.Add(descriptor_child.Key,
                                                  CyDescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex,
                                                  imageIndex);
                    node.ForeColor = foreColor;
                }
                RefreshNode(descriptor_child, node);
            }
        }

        /// <summary>
        /// Manages adding a new node to the m_deviceTree operation
        /// </summary>
        /// <param name="type">Defines a type of the node to add. This parameter is significant only for special cases
        /// (HID and AUDIO). In other cases this param is not taken into account</param>
        public void AddNode(CyUSBDescriptorType type)
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            if (treeNode != null)
            {
                if (type == CyUSBDescriptorType.HID)
                    m_parameters.m_deviceTree.AddNode(treeNode.Name, CyUSBDescriptorType.HID);
                else if (type == CyUSBDescriptorType.AUDIO)
                    m_parameters.m_deviceTree.AddNode(treeNode.Name, CyUSBDescriptorType.AUDIO);
                else if (treeNode.Name.Contains(CyUSBFSParameters.NODEKEY_INTERFACE))
                {
                    m_parameters.m_deviceTree.AddNode(treeNode.Name, (byte) treeNode.Index);
                }
                else
                {
                    m_parameters.m_deviceTree.AddNode(treeNode.Name);
                }
            }
            RefreshDeviceTree();
            if (treeNode != null) treeViewDevice.SelectedNode = treeViewDevice.Nodes.Find(treeNode.Name, true)[0];
            SelectedNodeChanged(treeViewDevice.SelectedNode);
        }

        /// <summary>
        /// Removes the selected node from the m_deviceTree
        /// </summary>
        private void RemoveNode()
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            TreeNode parent = treeNode.Parent;

            if (treeNode == null) return;

            try
            {
                CyDescriptorNode descNode = m_parameters.m_deviceTree.GetNodeByKey(treeNode.Name);

                if (descNode.m_value == null)
                {
                    if (descNode.Key.Contains(CyUSBFSParameters.NODEKEY_INTERFACE))
                    {
                        for (int i = treeNode.Index + 1; i < parent.Nodes.Count; i++)
                        {
                            CyDescriptorNode interfaceNode = 
                                m_parameters.m_deviceTree.GetNodeByKey(parent.Nodes[i].Name);
                            for (int j = 0; j < interfaceNode.m_nodes.Count; j++)
                            {
                                if (((CyInterfaceDescriptor)interfaceNode.m_nodes[j].m_value).bInterfaceNumber > 0)
                                    ((CyInterfaceDescriptor) interfaceNode.m_nodes[j].m_value).bInterfaceNumber--;
                            }
                        }
                    }
                }
                // Check if it is HID class. If so, confirm delete and change the Interface class to Undefined
                else if (descNode.m_value.bDescriptorType == CyUSBDescriptorType.HID)
                {
                    if (MessageBox.Show(Properties.Resources.MSG_INTERFACE_CHANGE, CyUSBFSParameters.MSG_TITLE_WARNING, 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        CyDescriptorNode descNodeInterface = m_parameters.m_deviceTree.GetNodeByKey(parent.Name);
                        ((CyInterfaceDescriptor) descNodeInterface.m_value).bInterfaceClass = 0;
                        ((CyInterfaceDescriptor) descNodeInterface.m_value).bInterfaceSubClass = 0;
                        ((CyInterfaceDescriptor) descNodeInterface.m_value).bInterfaceProtocol = 0;
                    }
                    else
                    {
                        return;
                    }
                }

                else if (descNode.m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE)
                {
                    for (int i = parent.Nodes.IndexOf(treeNode) + 1; i < parent.Nodes.Count; i++)
                    {
                        CyDescriptorNode altInterfaceNode = 
                            m_parameters.m_deviceTree.GetNodeByKey(parent.Nodes[i].Name);
                        ((CyInterfaceDescriptor) altInterfaceNode.m_value).bAlternateSetting--;
                    }
                }

                else if (descNode.m_value.bDescriptorType == CyUSBDescriptorType.ALTERNATE)
                {
                    for (int i = treeNode.Index + 1; i < parent.Nodes.Count; i++)
                    {
                        CyDescriptorNode interfaceNode = m_parameters.m_deviceTree.GetNodeByKey(parent.Nodes[i].Name);
                        for (int j = 0; j < interfaceNode.m_nodes.Count; j++)
                        {
                            ((CyInterfaceDescriptor) interfaceNode.m_nodes[j].m_value).bInterfaceNumber--;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Debug.Assert(false);
            }

            if (parent != null)
                m_parameters.m_deviceTree.RemoveNode(treeNode.Name, parent.Name);
            else
                m_parameters.m_deviceTree.RemoveNode(treeNode.Name, "");

            RefreshDeviceTree();
            SelectedNodeChanged(treeViewDevice.SelectedNode);
        }

        /// <summary>
        /// Updates attributes panel and manages tool buttons visibility for the selected node
        /// </summary>
        /// <param name="selectedNode"></param>
        private void SelectedNodeChanged(TreeNode selectedNode)
        {
            if (selectedNode != null)
            {
                if (m_controlDetails != null)
                {
                    panelDetails.Controls.Remove(m_controlDetails);
                    CleanControls(m_unusedControls);
                    m_unusedControls.Add(m_controlDetails);
                }
                panelDetails.Visible = true;
                toolStripOperations.Visible = true;

                //Template toolStripButtons group visibility
                toolStripSplitButtonImport.Visible = true;
                toolStripSplitButtonSave.Visible = true;
                toolStripSeparatorTemplates.Visible = true;
                toolStripSplitButtonAddInterace.Visible = false;

                CyDescriptorNode node = m_parameters.m_deviceTree.GetNodeByKey(selectedNode.Name);
                CyUSBDescriptor descriptor;
                if (node != null)
                {
                    if (node.m_value == null)
                    {
                        switch (node.Key)
                        {
                            case CyUSBFSParameters.NODEKEY_DEVICE:
                                CyDetailsEPMngt details_epmngt = new CyDetailsEPMngt(m_parameters);
                                details_epmngt.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_epmngt);
                                m_controlDetails = details_epmngt;
                                m_controlDetails.BringToFront();

                                toolStripButtonRemove.Enabled = false;
                                toolStripButtonAdd.Visible = true;
                                toolStripButtonAdd.Text = "Add Device";
                                break;
                            default:
                                if (node.Key.Contains(CyUSBFSParameters.NODEKEY_INTERFACE))
                                {
                                    toolStripButtonRemove.Enabled = true;
                                    toolStripButtonAdd.Visible = true;
                                    toolStripButtonAdd.Text = "Add Alternate Setting";
                                }
                                break;
                        }
                    }
                    else
                    {
                        descriptor = node.m_value;
                        switch (descriptor.bDescriptorType)
                        {
                            case CyUSBDescriptorType.DEVICE:
                                CyDetailsDevice details_device = new CyDetailsDevice((CyDeviceDescriptor) descriptor,
                                                                                     m_parameters);
                                details_device.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_device);
                                m_controlDetails = details_device;

                                toolStripButtonAdd.Visible = true;
                                toolStripButtonAdd.Text = "Add Configuration";
                                toolStripButtonRemove.Enabled = true;
                                break;
                            case CyUSBDescriptorType.CONFIGURATION:
                                CyDetailsConfig details_config = new CyDetailsConfig((CyConfigDescriptor) descriptor,
                                                                                     m_parameters);
                                details_config.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_config);
                                m_controlDetails = details_config;

                                toolStripButtonAdd.Visible = false;
                                toolStripButtonAdd.Text = "Add Interface";
                                toolStripButtonRemove.Enabled = true;
                                toolStripSplitButtonAddInterace.Visible = true;
                                break;
                            case CyUSBDescriptorType.ALTERNATE:
                                toolStripButtonRemove.Enabled = true;
                                toolStripButtonAdd.Visible = true;
                                toolStripButtonAdd.Text = "Add Alternate Setting";
                                break;
                            case CyUSBDescriptorType.INTERFACE:
                                CyDetailsInterface details_interface =
                                    new CyDetailsInterface((CyInterfaceDescriptor) descriptor, m_parameters, this);
                                details_interface.Dock = DockStyle.Fill;
                                details_interface.RemoveClassNodeEvent +=
                                    new  EventHandler(details_interface_RemoveClassNodeEvent);
                                panelDetails.Controls.Add(details_interface);
                                m_controlDetails = details_interface;

                                toolStripButtonAdd.Visible = true;
                                toolStripButtonAdd.Text = "Add Endpoint";
                                toolStripButtonRemove.Enabled = true;
                                break;
                            case CyUSBDescriptorType.ENDPOINT:
                                bool enabled = (selectedNode.ForeColor != SystemColors.GrayText);
                                CyDetailsEndpoint details_endpoint =
                                    new CyDetailsEndpoint((CyEndpointDescriptor)descriptor, m_parameters, enabled);
                                details_endpoint.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_endpoint);
                                m_controlDetails = details_endpoint;

                                toolStripButtonAdd.Visible = false;
                                toolStripButtonRemove.Enabled = true;
                                break;
                            case CyUSBDescriptorType.HID:
                                CyDetailsHID details_hid = new CyDetailsHID((CyHIDDescriptor) descriptor, m_parameters);
                                details_hid.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_hid);
                                m_controlDetails = details_hid;

                                toolStripButtonAdd.Visible = false;
                                toolStripButtonRemove.Enabled = true;
                                break;
                            case CyUSBDescriptorType.AUDIO:
                            case CyUSBDescriptorType.CDC:
                            case CyUSBDescriptorType.CS_ENDPOINT:
                                toolStripButtonAdd.Visible = false;
                                toolStripButtonRemove.Enabled = true;
                                break;
                            default:
                                toolStripButtonAdd.Visible = true;
                                toolStripButtonRemove.Enabled = true;
                                break;
                        }
                        if (m_controlDetails != null)
                            m_controlDetails.BringToFront();
                    }
                }
                // Disable adding new items for the audio read-only interfaces
                if (selectedNode.ForeColor == SystemColors.GrayText)
                {
                    toolStripButtonAdd.Visible = false;
                }


                //Set button Add enable/disable
                const int MAX_CHILDREN_NUMBER = 255;
                if (node.m_nodes.Count >= MAX_CHILDREN_NUMBER)
                {
                    toolStripButtonAdd.Enabled = false;
                }
                else
                {
                    toolStripButtonAdd.Enabled = true;
                }
            }
            else
            {
                if (m_controlDetails != null)
                {
                    panelDetails.Controls.Remove(m_controlDetails);
                    CleanControls(m_unusedControls);
                    m_unusedControls.Add(m_controlDetails);
                    m_controlDetails = null;
                }
                panelDetails.Visible = false;
                toolStripSplitButtonImport.Visible = true;
                toolStripSplitButtonSave.Visible = true;
                toolStripSeparatorTemplates.Visible = true;
                toolStripSplitButtonAddInterace.Visible = false;
                toolStripButtonAdd.Visible = false;
                toolStripButtonRemove.Enabled = false;
            }
        }

        /// <summary>
        /// Removed if necessary the HID class node depending on the option selected in the Interface attributes
        /// </summary>
        private void details_interface_RemoveClassNodeEvent(object sender, EventArgs e)
        {
            TreeNode parent = treeViewDevice.SelectedNode;
            for (int i = 0; i < treeViewDevice.SelectedNode.Nodes.Count; i++)
            {
                TreeNode treeNode = treeViewDevice.SelectedNode.Nodes[i];
                CyDescriptorNode descNode = m_parameters.m_deviceTree.GetNodeByKey(treeNode.Name);
                if ((parent != null) && (descNode != null) &&
                    (descNode.m_value.bDescriptorType != CyUSBDescriptorType.ENDPOINT))
                    m_parameters.m_deviceTree.RemoveNode(treeNode.Name, parent.Name);
            }
            RefreshDeviceTree();
            if (parent != null) treeViewDevice.SelectedNode = treeViewDevice.Nodes.Find(parent.Name, true)[0];
            SelectedNodeChanged(treeViewDevice.SelectedNode);
        }

        /// <summary>
        /// Disposes unused controls that could be accumulated during creation of the attributes panel each time the 
        /// selected node is changed.
        /// </summary>
        /// <param name="controlList"></param>
        public static void CleanControls(List<UserControl> controlList)
        {
            for (int i = 0; i < controlList.Count; i++)
            {
                controlList[i].Dispose();
            }
            controlList.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------

        #endregion General functions

        #region Events

        private void CyDeviceDescriptor_Load(object sender, EventArgs e)
        {
            RefreshDeviceTree();
        }

        private void treeViewDevice_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedNodeChanged(e.Node);
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            RemoveNode();
            m_parameters.ParamDeviceTreeChanged = true;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            AddNode(CyUSBDescriptorType.DEVICE); // temporary parameter
            m_parameters.ParamDeviceTreeChanged = true;
        }

        private void toolStripSplitButtonAddInterface_DropDownOpening(object sender, EventArgs e)
        {
            // Audio submenu
            ToolStripItemCollection audioItems = audioToolStripMenuItem.DropDown.Items;
            audioItems.Clear();
            for (int i = 0; i < m_parameters.m_audioTree.m_nodes[0].m_nodes.Count; i++)
            {
                ToolStripItem item = audioItems.Add(m_parameters.m_audioTree.m_nodes[0].m_nodes[i].m_value.ToString());
                item.Click += new EventHandler(addAudioItem_Click);
            }
            if (m_parameters.m_audioTree.m_nodes[0].m_nodes.Count == 0)
            {
                ToolStripItem itemNoneAudio = new ToolStripMenuItem("(none)");
                itemNoneAudio.Enabled = false;
                audioItems.Add(itemNoneAudio);
            }
            // Midi submenu
            ToolStripItemCollection midiItems = mIDIToolStripMenuItem.DropDown.Items;
            midiItems.Clear();
            for (int i = 0; i < m_parameters.m_midiTree.m_nodes[0].m_nodes.Count; i++)
            {
                ToolStripItem item = midiItems.Add(m_parameters.m_midiTree.m_nodes[0].m_nodes[i].m_value.ToString());
                item.Click += new EventHandler(addMidiItem_Click);
            }
            if (m_parameters.m_midiTree.m_nodes[0].m_nodes.Count == 0)
            {
                ToolStripItem itemNoneMidi = new ToolStripMenuItem("(none)");
                itemNoneMidi.Enabled = false;
                midiItems.Add(itemNoneMidi);
            }
            // CDC submenu
            ToolStripItemCollection cdcItems = cdcToolStripMenuItem.DropDown.Items;
            cdcItems.Clear();
            for (int i = 0; i < m_parameters.m_cdcTree.m_nodes[0].m_nodes.Count; i++)
            {
                ToolStripItem item = cdcItems.Add(m_parameters.m_cdcTree.m_nodes[0].m_nodes[i].m_value.ToString());
                item.Click += new EventHandler(addCDCItem_Click);
            }
            if (m_parameters.m_cdcTree.m_nodes[0].m_nodes.Count == 0)
            {
                ToolStripItem itemNoneCDC = new ToolStripMenuItem("(none)");
                itemNoneCDC.Enabled = false;
                cdcItems.Add(itemNoneCDC);
            }
        }

        void addAudioItem_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            if (treeNode != null)
            {
                CyDescriptorNode parentNode = m_parameters.m_deviceTree.GetNodeByKey(treeNode.Name);
                if (parentNode != null)
                {
                    int itemIndex = audioToolStripMenuItem.DropDownItems.IndexOf((ToolStripItem)sender);
                    parentNode.m_nodes.Add(m_parameters.m_audioTree.m_nodes[0].m_nodes[itemIndex]);
                    m_parameters.ParamDeviceTreeChanged = true;
                    RefreshDeviceTree();
                }
            }
        }

        void addMidiItem_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            if (treeNode != null)
            {
                CyDescriptorNode parentNode = m_parameters.m_deviceTree.GetNodeByKey(treeNode.Name);
                if (parentNode != null)
                {
                    int itemIndex = mIDIToolStripMenuItem.DropDownItems.IndexOf((ToolStripItem)sender);
                    parentNode.m_nodes.Add(m_parameters.m_midiTree.m_nodes[0].m_nodes[itemIndex]);
                    m_parameters.ParamDeviceTreeChanged = true;
                    RefreshDeviceTree();
                }
            }
        }

        void addCDCItem_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            if (treeNode != null)
            {
                CyDescriptorNode parentNode = m_parameters.m_deviceTree.GetNodeByKey(treeNode.Name);
                if (parentNode != null)
                {
                    int itemIndex = cdcToolStripMenuItem.DropDownItems.IndexOf((ToolStripItem)sender);
                    parentNode.m_nodes.Add(m_parameters.m_cdcTree.m_nodes[0].m_nodes[itemIndex]);
                    m_parameters.ParamDeviceTreeChanged = true;
                    RefreshDeviceTree();
                }
            }
        }

        private void CyDeviceDescriptor_VisibleChanged(object sender, EventArgs e)
        {
            //Refresh tree (if Audio, String or Report Descriptors were changed)
            if (Visible)
            {
                TreeNode selectedNode = treeViewDevice.SelectedNode;
                RefreshDeviceTree();
                if (selectedNode != null)
                {
                    TreeNode[] n = treeViewDevice.Nodes.Find(selectedNode.Name, true);
                    if (n.Length > 0) treeViewDevice.SelectedNode = n[0];
                }
            }
        }

        private void treeViewDevice_KeyDown(object sender, KeyEventArgs e)
        {
            if (toolStripButtonRemove.Visible && toolStripButtonRemove.Enabled)
                if (e.KeyCode == Keys.Delete)
                {
                    RemoveNode();
                    m_parameters.ParamDeviceTreeChanged = true;
                }
        }

        #endregion Events

        #region Save, Import 

        private void toolStripSplitButton_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender == toolStripSplitButtonSave
                       ? saveOneDescriptorToolStripMenuItem : importOneDescriptorToolStripMenuItem;
            if (treeViewDevice.SelectedNode != null)
            {
                if ((treeViewDevice.SelectedNode.Level == LEVEL_DEVICE) ||
                    (treeViewDevice.SelectedNode.Level == LEVEL_CONFIGURATION) ||
                    (treeViewDevice.SelectedNode.Level == LEVEL_INTERFACE))
                {
                    item.Visible = true;
                }
                else
                {
                    item.Visible = false;
                }
            }
            else
            {
                item.Visible = false;
            }
        }

        private void saveOneDescriptorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMenuItemClick(false);
        }

        private void saveFullConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMenuItemClick(true);
        }

        private void SaveMenuItemClick(bool saveList)
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            if (saveList && (m_parameters.m_deviceTree.m_nodes.Count > 0))
            {
                m_parameters.SaveDescriptorList(m_parameters.m_deviceTree.m_nodes[0], CyUSBDescriptorType.ALL);
            }
            else if (treeNode != null)
            {
                CyDescriptorNode descNode = m_parameters.m_deviceTree.GetNodeByKey(treeNode.Name);
                if ((descNode != null) && (descNode.m_value != null))
                    m_parameters.SaveDescriptor(descNode.m_value);
            }
        }

        private void importOneDescriptorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewDevice.SelectedNode;
            if (selectedNode != null)
            {
                CyDescriptorNode node = m_parameters.m_deviceTree.GetNodeByKey(selectedNode.Name);
                if (node.m_value != null)
                {
                    CyUSBDescriptor descNew = m_parameters.ImportDescriptor(node.m_value.bDescriptorType);
                    if (descNew != null)
                    {
                        node.m_value = (CyUSBDescriptor)descNew.Clone();
                        SelectedNodeChanged(selectedNode);
                    }
                    m_parameters.ParamStringTreeChanged = true;
                    m_parameters.ParamDeviceTreeChanged = true;
                    m_parameters.ParamAudioTreeChanged = true;
                }
            }           
        }

        private void importFullConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_parameters.ImportDescriptorList(CyUSBDescriptorType.ALL))
            {
                m_parameters.ParamStringTreeChanged = true;
                m_parameters.ParamHIDReportTreeChanged = true;
                m_parameters.ParamDeviceTreeChanged = true;
                m_parameters.ParamAudioTreeChanged = true;
            }
            RefreshDeviceTree();
        }

        private void toolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            ((ToolStripSplitButton)sender).ShowDropDown();
        }

        #endregion Save, Import

        #region 'Enter' key override
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion 'Enter' key override

    }
}
