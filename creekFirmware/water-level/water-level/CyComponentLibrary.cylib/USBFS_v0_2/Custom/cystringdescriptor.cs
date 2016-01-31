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
    public partial class CyStringDescriptor : UserControl
    {
        public USBFSParameters Parameters;
        private UserControl controlDetails;
        private bool ShowSerial = false;
        private bool ShowEE = false;

        public CyStringDescriptor()
        {
            InitializeComponent();
            Parameters = new USBFSParameters();
            checkBoxShowSerial.Checked = ShowSerial;
            checkBoxShowEE.Checked = ShowEE;
            RefreshStringTree();
        }

        public CyStringDescriptor(USBFSParameters parameters)
        {
            InitializeComponent();
            Parameters = parameters;

            UpdateSpecialStrings();          
           
            RefreshStringTree();
        }

        #region General functions

        private void RefreshStringTree()
        {
            treeViewStrings.Nodes.Clear();
            foreach (DescriptorNode descriptor in Parameters.StringTree.Nodes)
            {
                TreeNode node = treeViewStrings.Nodes.Add(descriptor.Key, DescriptorNode.GetDescriptorLabel(descriptor), 0, 0);
                ////if (ShowSerial || ShowEE)
                ////{
                ////    TreeNode additional = treeViewStrings.Nodes.Add("Special Strings");
                ////    if (ShowSerial)
                ////        additional.Nodes.Add(descriptor.Nodes[1].Key, DescriptorNode.GetDescriptorLabel(descriptor.Nodes[1]), 2, 2);
                ////    if (ShowEE)
                ////        additional.Nodes.Add(descriptor.Nodes[2].Key, DescriptorNode.GetDescriptorLabel(descriptor.Nodes[2]), 2, 2);
                ////}
                RefreshNode(descriptor, node);
            }

            treeViewStrings.ExpandAll();
        }

        private void RefreshNode(DescriptorNode descriptor, TreeNode treeNode)
        {
            int imageIndex = 0;
            foreach (DescriptorNode descriptor_child in descriptor.Nodes)
            {
                switch (descriptor_child.Value.bDescriptorType)
                {
                    case USBDescriptorType.STRING:
                        switch (descriptor_child.Key)
                        {
                            case "LANGID":
                            case "Serial":
                            case "EE":
                                imageIndex = 2;
                                break;
                            default:
                                imageIndex = 1;
                                break;
                        }
                        
                        break;
                }

                if ((descriptor_child.Key == "Serial") && (!ShowSerial)) continue;
                if ((descriptor_child.Key == "EE") && (!ShowEE)) continue;

                TreeNode node = treeNode.Nodes.Add(descriptor_child.Key, DescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex, imageIndex);
                RefreshNode(descriptor_child, node);
            }
        }

        private void AddNode()
        {
            TreeNode selTreeNode = treeViewStrings.SelectedNode;
            TreeNode treeNode = treeViewStrings.Nodes[0];
            if (treeNode != null)
            {
                Parameters.StringTree.AddNode(treeNode.Name);
            }
            RefreshStringTree();
            if (selTreeNode != null)
            {
                treeViewStrings.SelectedNode = treeViewStrings.Nodes.Find(selTreeNode.Name, true)[0];
                SelectedNodeChanged(treeViewStrings.SelectedNode);
            }
        }

        private void RemoveNode()
        {
            TreeNode treeNode = treeViewStrings.SelectedNode;
            TreeNode parent = treeNode.Parent;
            if (parent != null)
                Parameters.StringTree.RemoveNode(treeNode.Name, parent.Name);
            else
                Parameters.StringTree.RemoveNode(treeNode.Name, "");

            RefreshStringTree();
            SelectedNodeChanged(treeViewStrings.SelectedNode);
        }

        private void SelectedNodeChanged(TreeNode selectedNode)
        {
            if (selectedNode != null)
            {
                if (controlDetails != null)
                    panelDetails.Controls.Remove(controlDetails);
                panelDetails.Visible = true;
                DescriptorNode node = Parameters.StringTree.GetNodeByKey(selectedNode.Name);
                USBDescriptor descriptor;
                if (node != null)
                {
                    if (node.Value == null)
                    {
                        toolStripButtonRemove.Visible = false;
                        toolStripButtonAdd.Visible = true;
                    }
                    else
                    {
                        descriptor = node.Value;
                        switch (descriptor.bDescriptorType)
                        {
                            case USBDescriptorType.STRING:
                                //toolStripButtonAdd.Visible = false;
                                switch (node.Key)
                                {
                                    case "LANGID":
                                        CyDetailsLangID details_langid = new CyDetailsLangID((StringZeroDescriptor)descriptor, Parameters);
                                        details_langid.Dock = DockStyle.Fill;
                                        panelDetails.Controls.Add(details_langid);
                                        controlDetails = details_langid;
                                        toolStripButtonRemove.Visible = false;
                                        break;
                                    case "Serial":
                                    case "EE":
                                        CyDetailsString details_string = new CyDetailsString((StringDescriptor)descriptor, Parameters);
                                        details_string.Dock = DockStyle.Fill;
                                        details_string.UpdateNodeEvent += UpdateNodeText;
                                        if (node.Key == "Serial")
                                            details_string.SetGroupBoxVisible();
                                        panelDetails.Controls.Add(details_string);
                                        controlDetails = details_string;
                                        toolStripButtonRemove.Visible = false;
                                        break;
                                    default:
                                        CyDetailsString details_string1 = new CyDetailsString((StringDescriptor)descriptor, Parameters);
                                        details_string1.Dock = DockStyle.Fill;
                                        details_string1.UpdateNodeEvent += UpdateNodeText;
                                        panelDetails.Controls.Add(details_string1);
                                        controlDetails = details_string1;
                                        toolStripButtonRemove.Visible = true;
                                        break;
                                }
                                break;
                            default:
                                toolStripButtonAdd.Visible = true;
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

        private void UpdateNodeText()
        {
            foreach (TreeNode node in treeViewStrings.Nodes[0].Nodes)
            {
                //Not update folders
                if (node.Nodes.Count > 0) continue;
                
                node.Text = DescriptorNode.GetDescriptorLabel(Parameters.StringTree.GetNodeByKey(node.Name));
            }
        }

        private void UpdateSpecialStrings()
        {
            try
            {
                if (String.IsNullOrEmpty(((StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[0].Value).bString))
                {
                    checkBoxShowSerial.Checked = false;
                    ShowSerial = false;
                }
                else
                {
                    checkBoxShowSerial.Checked = true;
                    ShowSerial = true;
                }
                if (String.IsNullOrEmpty(((StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[1].Value).bString))
                {
                    checkBoxShowEE.Checked = false;
                    ShowEE = false;
                }
                else
                {
                    checkBoxShowEE.Checked = true;
                    ShowEE = true;
                }
            }
            catch (Exception)
            {
                checkBoxShowSerial.Checked = ShowSerial;
                checkBoxShowEE.Checked = ShowEE;
            }
        }
        #endregion General functions

        #region Events
        private void CyStringDescriptor_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateSpecialStrings();
                RefreshStringTree();
            }
        }

        private void treeViewStrings_AfterSelect(object sender, TreeViewEventArgs e)
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
            AddNode();
            Parameters.ParamsChanged = true;
        }

        private void checkBoxShowEE_CheckedChanged(object sender, EventArgs e)
        {
            ShowEE = checkBoxShowEE.Checked;
            if (!ShowEE)
                ((StringDescriptor)Parameters.StringTree.Nodes[1].Nodes[1].Value).bString = "";
            Parameters.ParamsChanged = true;
            RefreshStringTree();
        }

        private void checkBoxShowSerial_CheckedChanged(object sender, EventArgs e)
        {
            ShowSerial = checkBoxShowSerial.Checked;
            if (!ShowSerial)
            {
                ((StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[0].Value).bString = "";
                // Set Serial Number in DeviceDescriptor to zero
                try
                {
                    for (int i = 0; i < Parameters.DeviceTree.Nodes[0].Nodes.Count; i++)
                    {
                        ((DeviceDescriptor) Parameters.DeviceTree.Nodes[0].Nodes[i].Value).iSerialNumber = 0;
                    }
                }
                catch (Exception)
                {
                }
            }
            Parameters.ParamsChanged = true;
            RefreshStringTree();
        }
        #endregion Events

        
    }
}
