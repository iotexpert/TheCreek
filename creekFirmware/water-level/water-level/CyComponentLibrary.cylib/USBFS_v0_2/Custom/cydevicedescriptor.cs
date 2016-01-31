/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace USBFS_v0_2
{
    public partial class CyDeviceDescriptor : UserControl
    {
        #region Variables

        public USBFSParameters Parameters;
        private UserControl controlDetails;
        #endregion Variables

        #region Constructors

        public CyDeviceDescriptor()
        {
            InitializeComponent();
            Parameters = new USBFSParameters();
            RefreshDeviceTree();

            ToolStripManager.Renderer =
            new ToolStripProfessionalRenderer(new CustomProfessionalColors());
        }

        public CyDeviceDescriptor(USBFSParameters parameters)
        {
            InitializeComponent();
            Parameters = parameters;
            RefreshDeviceTree();

            ToolStripManager.Renderer =
            new ToolStripProfessionalRenderer(new CustomProfessionalColors());
        }
        #endregion Constructors

        #region General functions

        private void RefreshDeviceTree()
        {
            treeViewDevice.BeginUpdate();
            treeViewDevice.Nodes.Clear();
            foreach (DescriptorNode descriptor in Parameters.DeviceTree.Nodes)
            {
                TreeNode node = treeViewDevice.Nodes.Add(descriptor.Key, DescriptorNode.GetDescriptorLabel(descriptor),0,0);
                RefreshNode(descriptor, node);
            }
            treeViewDevice.ExpandAll();
            SelectedNodeChanged(treeViewDevice.SelectedNode);
            treeViewDevice.EndUpdate();
        }

        private void RefreshNode(DescriptorNode descriptor, TreeNode treeNode)
        {
            int imageIndex = 0;
            foreach (DescriptorNode descriptor_child in descriptor.Nodes)
            {
                switch (descriptor_child.Value.bDescriptorType)
                {
                    case USBDescriptorType.DEVICE:
                        imageIndex = 1;
                        break;
                    case USBDescriptorType.CONFIGURATION:
                        imageIndex = 2;
                        break;
                    case USBDescriptorType.INTERFACE:
                        imageIndex = 2;
                        break;
                    case USBDescriptorType.HID:
                        imageIndex = 2;
                        break;
                    case USBDescriptorType.ENDPOINT:
                        imageIndex = 2;
                        break;
                }

                TreeNode node;
                if (descriptor_child.Value.bDescriptorType == USBDescriptorType.HID)
                    node = treeNode.Nodes.Insert(0, descriptor_child.Key, DescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex, imageIndex);
                else if (descriptor_child.Value.bDescriptorType == USBDescriptorType.AUDIO)
                {
                    int nodeIndex = 0;
                    for (int i = 0; i < descriptor.Nodes.Count; i++)
                    {
                        if (descriptor.Nodes[i].Value.bDescriptorType != USBDescriptorType.AUDIO)
                        {
                            nodeIndex = i;
                            break;
                        }
                    }
                    node = treeNode.Nodes.Insert(nodeIndex, descriptor_child.Key,
                                                 DescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex,
                                                 imageIndex);
                }
                else
                    node = treeNode.Nodes.Add(descriptor_child.Key, DescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex, imageIndex);
                RefreshNode(descriptor_child, node);
            }
        }

        public void AddNode(USBDescriptorType type)
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            if (treeNode != null)
            {
                if (type == USBDescriptorType.HID)
                    Parameters.DeviceTree.AddNode(treeNode.Name, USBDescriptorType.HID);
                else if (type == USBDescriptorType.AUDIO)
                    Parameters.DeviceTree.AddNode(treeNode.Name, USBDescriptorType.AUDIO);
                else
                {
                    Parameters.DeviceTree.AddNode(treeNode.Name);
                    Parameters.RecalcDescriptors(Parameters.DeviceTree.Nodes[0]);
                }
            }
            RefreshDeviceTree();
            treeViewDevice.SelectedNode = treeViewDevice.Nodes.Find(treeNode.Name, true)[0];
            SelectedNodeChanged(treeViewDevice.SelectedNode);
        }

        private void RemoveNode()
        {
            TreeNode treeNode = treeViewDevice.SelectedNode;
            TreeNode parent = treeNode.Parent;


            // Check if it is HID class. If so, confirm delete and change the Interface class to Undefined
            try
            {
                DescriptorNode descNode = Parameters.DeviceTree.GetNodeByKey(treeNode.Name);
                if (descNode.Value.bDescriptorType == USBDescriptorType.HID)
                {
                    if (MessageBox.Show("The Interface class would be changed to Undefined. Do you want to continue?", "USBFS", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        DescriptorNode descNodeInterface = Parameters.DeviceTree.GetNodeByKey(parent.Name);
                        ((InterfaceDescriptor) descNodeInterface.Value).bInterfaceClass = 0;
                        ((InterfaceDescriptor)descNodeInterface.Value).bInterfaceSubClass = 0;
                        ((InterfaceDescriptor)descNodeInterface.Value).bInterfaceProtocol = 0;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
           

            if (parent != null)
                Parameters.DeviceTree.RemoveNode(treeNode.Name, parent.Name);
            else
                Parameters.DeviceTree.RemoveNode(treeNode.Name, "");

            RefreshDeviceTree();
            SelectedNodeChanged(treeViewDevice.SelectedNode);
        }

        private void SelectedNodeChanged(TreeNode selectedNode)
        {
            if (selectedNode != null)
            {
                if (controlDetails != null)
                    panelDetails.Controls.Remove(controlDetails);
                panelDetails.Visible = true;
                DescriptorNode node = Parameters.DeviceTree.GetNodeByKey(selectedNode.Name);
                USBDescriptor descriptor;
                if (node != null)
                {
                    if (node.Value == null)
                    {
                        toolStripButtonRemove.Visible = false;
                        toolStripSeparatorRemove.Visible = false;
                        toolStripButtonAdd.Visible = true;
                        toolStripButtonAdd.Text = "Add Device";
                        toolStripButtonAddClass.Visible = false;
                    }
                    else
                    {
                        descriptor = node.Value;
                        switch (descriptor.bDescriptorType)
                        {
                            case USBDescriptorType.DEVICE:
                                CyDetailsDevice details_device = new CyDetailsDevice((DeviceDescriptor)descriptor, Parameters);
                                details_device.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_device);
                                controlDetails = details_device;

                                toolStripButtonAdd.Visible = true;
                                toolStripButtonAdd.Text = "Add Configuration";
                                toolStripButtonRemove.Visible = true;
                                toolStripSeparatorRemove.Visible = true;
                                toolStripButtonAddClass.Visible = false;
                                break;
                            case USBDescriptorType.CONFIGURATION:
                                CyDetailsConfig details_config = new CyDetailsConfig((ConfigDescriptor)descriptor, Parameters);
                                details_config.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_config);
                                controlDetails = details_config;

                                toolStripButtonAdd.Visible = true;
                                toolStripButtonAdd.Text = "Add Interface";
                                toolStripButtonRemove.Visible = true;
                                toolStripSeparatorRemove.Visible = true;
                                toolStripButtonAddClass.Visible = false;
                                break;
                            case USBDescriptorType.INTERFACE:
                                CyDetailsInterface details_interface = new CyDetailsInterface((InterfaceDescriptor)descriptor, Parameters, this);
                                details_interface.Dock = DockStyle.Fill;
                                details_interface.RemoveClassNodeEvent += new CyDetailsInterface.RemoveNodeDelegate(details_interface_RemoveClassNodeEvent);
                                panelDetails.Controls.Add(details_interface);
                                controlDetails = details_interface;

                                toolStripButtonAdd.Visible = true;
                                toolStripButtonAdd.Text = "Add Endpoint";
                                toolStripButtonRemove.Visible = true;
                                toolStripSeparatorRemove.Visible = true;
                                //toolStripButtonAddClass.Visible = false;
                                break;
                            case USBDescriptorType.ENDPOINT:
                                CyDetailsEndpoint details_endpoint = new CyDetailsEndpoint((EndpointDescriptor)descriptor, Parameters);
                                details_endpoint.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_endpoint);
                                controlDetails = details_endpoint;

                                toolStripButtonAdd.Visible = false;
                                toolStripButtonRemove.Visible = true;
                                toolStripSeparatorRemove.Visible = true;
                                toolStripButtonAddClass.Visible = false;
                                break;
                            case USBDescriptorType.HID:
                                CyDetailsHID details_hid = new CyDetailsHID((HIDDescriptor)descriptor, Parameters);
                                details_hid.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_hid);
                                controlDetails = details_hid;

                                toolStripButtonAdd.Visible = false;
                                toolStripButtonRemove.Visible = true;
                                toolStripSeparatorRemove.Visible = true;
                                toolStripButtonAddClass.Visible = false;
                                break;
                            case USBDescriptorType.AUDIO:
                                CyDetailsAudio details_audio = new CyDetailsAudio((AudioDescriptor)descriptor, Parameters);
                                details_audio.Dock = DockStyle.Fill;
                                panelDetails.Controls.Add(details_audio);
                                controlDetails = details_audio;

                                toolStripButtonAdd.Visible = false;
                                toolStripButtonRemove.Visible = true;
                                toolStripSeparatorRemove.Visible = true;
                                toolStripButtonAddClass.Visible = false;
                                break;
                            default:
                                toolStripButtonAdd.Visible = true;
                                toolStripButtonRemove.Visible = true;
                                toolStripSeparatorRemove.Visible = true;
                                toolStripButtonAddClass.Visible = false;
                                break;
                        }
                        if (controlDetails != null)
                            controlDetails.BringToFront();
                    }
                }
            }
            else
            {
                if (controlDetails != null)
                {
                    panelDetails.Controls.Remove(controlDetails);
                    controlDetails = null;
                }
                panelDetails.Visible = false;
            }
        }

        void details_interface_RemoveClassNodeEvent()
        {
            TreeNode parent = treeViewDevice.SelectedNode;
            foreach (TreeNode treeNode in treeViewDevice.SelectedNode.Nodes)
            {
                DescriptorNode descNode = Parameters.DeviceTree.GetNodeByKey(treeNode.Name);
                if ((parent != null) && (descNode != null) && (descNode.Value.bDescriptorType!= USBDescriptorType.ENDPOINT))
                    Parameters.DeviceTree.RemoveNode(treeNode.Name, parent.Name);
            }
            RefreshDeviceTree();
            treeViewDevice.SelectedNode = treeViewDevice.Nodes.Find(parent.Name, true)[0];
            SelectedNodeChanged(treeViewDevice.SelectedNode);
        }

        public void SetBtnAddClass(int visible)
        {
            if (visible == 0)
                toolStripButtonAddClass.Visible = false;
            else if (visible == 1)
            {
                toolStripButtonAddClass.Visible = true;
                toolStripButtonAddClass.Text = "Add Audio Class";
            }
            else if (visible == 2)
            {
                toolStripButtonAddClass.Visible = true;
                toolStripButtonAddClass.Text = "Add CDC Func Desc";
            }
        }

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
            Parameters.ParamsChanged = true;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            AddNode(USBDescriptorType.DEVICE); // temporary parameter
            Parameters.ParamsChanged = true;
        }

        private void toolStripButtonAddClass_Click(object sender, EventArgs e)
        {
            AddNode(USBDescriptorType.AUDIO);
            Parameters.ParamsChanged = true;
        }

        private void toolStripButtonAddCDCClass_Click(object sender, EventArgs e)
        {
            AddNode(USBDescriptorType.AUDIO);
            Parameters.ParamsChanged = true;
        }

        private void CyDeviceDescriptor_VisibleChanged(object sender, EventArgs e)
        {
            //Refresh details (if String Descriptors or Report Descriptors were changed)
            if (Visible)
                SelectedNodeChanged(treeViewDevice.SelectedNode);
        }
        #endregion Events

    }

    class CustomProfessionalColors : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin
        { get { return SystemColors.Control; } }

        public override Color ToolStripGradientMiddle
        { get { return SystemColors.Control; } }

        public override Color ToolStripGradientEnd
        { get { return SystemColors.ControlDark; } }

        public override Color ToolStripBorder
        { get { return SystemColors.ControlDark; } }

        public override Color ToolStripPanelGradientEnd
        { get { return SystemColors.ControlDark; } }
    }

}
