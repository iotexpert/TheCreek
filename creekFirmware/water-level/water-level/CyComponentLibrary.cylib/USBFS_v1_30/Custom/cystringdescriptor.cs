/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace USBFS_v1_30
{
    public partial class CyStringDescriptor : UserControl
    {
        public CyUSBFSParameters Parameters;
        private UserControl controlDetails;
        private List<UserControl> unusedControls;
        private bool ShowSerial = false;
        private bool ShowEE = false;

        public CyStringDescriptor()
        {
            InitializeComponent();
            Parameters = new CyUSBFSParameters();
            checkBoxShowSerial.Checked = ShowSerial;
            checkBoxShowEE.Checked = ShowEE;
            unusedControls = new List<UserControl>();
            RefreshStringTree();
        }

        public CyStringDescriptor(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            Parameters = parameters;
            unusedControls = new List<UserControl>();
            UpdateSpecialStrings();
            RefreshStringTree();
        }

        #region General functions

        private void RefreshStringTree()
        {
            treeViewStrings.Nodes.Clear();
            foreach (DescriptorNode descriptor in Parameters.StringTree.Nodes)
            {
                TreeNode node = treeViewStrings.Nodes.Add(descriptor.Key, DescriptorNode.GetDescriptorLabel(descriptor),
                                                          0, 0);
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

                TreeNode node = treeNode.Nodes.Add(descriptor_child.Key,
                                                   DescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex,
                                                   imageIndex);
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
                {
                    panelDetails.Controls.Remove(controlDetails);
                    CyDeviceDescriptor.CleanControls(unusedControls);
                    unusedControls.Add(controlDetails);
                }
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
                                        CyDetailsLangID details_langid =
                                            new CyDetailsLangID((StringZeroDescriptor) descriptor, Parameters);
                                        details_langid.Dock = DockStyle.Fill;
                                        panelDetails.Controls.Add(details_langid);
                                        controlDetails = details_langid;
                                        toolStripButtonRemove.Visible = false;
                                        break;
                                    case "Serial":
                                    case "EE":
                                        CyDetailsString details_string =
                                            new CyDetailsString((StringDescriptor) descriptor, Parameters);
                                        details_string.Dock = DockStyle.Fill;
                                        details_string.UpdateNodeEvent += UpdateNodeText;
                                        if (node.Key == "Serial")
                                            details_string.SetGroupBoxVisible();
                                        if (node.Key == "EE")
                                            details_string.SetTextBoxDefault();
                                        panelDetails.Controls.Add(details_string);
                                        controlDetails = details_string;
                                        toolStripButtonRemove.Visible = false;
                                        break;
                                    default:
                                        CyDetailsString details_string1 =
                                            new CyDetailsString((StringDescriptor) descriptor, Parameters);
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
                    CyDeviceDescriptor.CleanControls(unusedControls);
                    unusedControls.Add(controlDetails);
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
                if (((StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[0].Value).bUsed)
                {
                    checkBoxShowSerial.Checked = true;
                    ShowSerial = true;
                }
                else
                {
                    checkBoxShowSerial.Checked = false;
                    ShowSerial = false;
                }
                if (((StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[1].Value).bUsed)
                {
                    checkBoxShowEE.Checked = true;
                    ShowEE = true;
                }
                else
                {
                    checkBoxShowEE.Checked = false;
                    ShowEE = false;
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
            ((StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[1].Value).bUsed = ShowEE;
            Parameters.ParamsChanged = true;
            RefreshStringTree();
        }

        private void checkBoxShowSerial_CheckedChanged(object sender, EventArgs e)
        {
            ShowSerial = checkBoxShowSerial.Checked;
            ((StringDescriptor) Parameters.StringTree.Nodes[1].Nodes[0].Value).bUsed = ShowSerial;
            if (!ShowSerial)
            {
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