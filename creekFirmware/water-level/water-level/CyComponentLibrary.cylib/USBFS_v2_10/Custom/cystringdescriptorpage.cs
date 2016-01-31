/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace USBFS_v2_10
{
    public partial class CyStringDescriptorPage : UserControl
    {
        #region Fields
        public CyUSBFSParameters m_parameters;
        private UserControl m_controlDetails;
        private readonly List<UserControl> m_unusedControls;
        private bool m_showSerial = false;
        private bool m_showEE = false;
        #endregion Fields

        #region Constructors
        public CyStringDescriptorPage()
        {
            InitializeComponent();
        }

        public CyStringDescriptorPage(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            m_unusedControls = new List<UserControl>();
            UpdateSpecialStrings();
            RefreshStringTree();
        }
        #endregion Constructors

        #region General functions

        /// <summary>
        /// Rebuilds String Descriptors TreeView based on m_stringTree
        /// </summary>
        private void RefreshStringTree()
        {
            treeViewStrings.Nodes.Clear();
            for (int i = 0; i < m_parameters.m_stringTree.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor = m_parameters.m_stringTree.m_nodes[i];
                TreeNode node = treeViewStrings.Nodes.Add(descriptor.Key,
                                                          CyDescriptorNode.GetDescriptorLabel(descriptor), 0, 0);
                RefreshNode(descriptor, node);
            }

            treeViewStrings.ExpandAll();
        }

        /// <summary>
        /// Recursive function that supplement RefreshStringTree() function
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="treeNode"></param>
        private void RefreshNode(CyDescriptorNode descriptor, TreeNode treeNode)
        {
            int imageIndex = 0;
            for (int i = 0; i < descriptor.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor_child = descriptor.m_nodes[i];
                switch (descriptor_child.m_value.bDescriptorType)
                {
                    case CyUSBDescriptorType.STRING:
                        switch (descriptor_child.Key)
                        {
                            case CyUSBFSParameters.NODEKEY_STRING_LANGID:
                            case CyUSBFSParameters.NODEKEY_STRING_SERIAL:
                            case CyUSBFSParameters.NODEKEY_MSOS_STRING:
                                imageIndex = 2;
                                break;
                            default:
                                imageIndex = 1;
                                break;
                        }

                        break;
                }

                if ((descriptor_child.Key == CyUSBFSParameters.NODEKEY_STRING_SERIAL) && (!m_showSerial)) continue;
                if ((descriptor_child.Key == CyUSBFSParameters.NODEKEY_MSOS_STRING) && (!m_showEE)) continue;

                TreeNode node = treeNode.Nodes.Add(descriptor_child.Key,
                                                   CyDescriptorNode.GetDescriptorLabel(descriptor_child), imageIndex,
                                                   imageIndex);
                RefreshNode(descriptor_child, node);
            }
        }

        /// <summary>
        /// Adds a new string descriptor to the m_stringTree
        /// </summary>
        private void AddNode()
        {
            TreeNode selTreeNode = treeViewStrings.SelectedNode;
            TreeNode treeNode = treeViewStrings.Nodes[0];
            if (treeNode != null)
            {
                m_parameters.m_stringTree.AddNode(treeNode.Name);
            }
            RefreshStringTree();
            if (selTreeNode != null)
            {
                treeViewStrings.SelectedNode = treeViewStrings.Nodes.Find(selTreeNode.Name, true)[0];
                SelectedNodeChanged(treeViewStrings.SelectedNode);
            }
        }

        /// <summary>
        /// Removes the selected string descriptor from the m_stringTree
        /// </summary>
        private void RemoveNode()
        {
            TreeNode treeNode = treeViewStrings.SelectedNode;
            TreeNode parent = treeNode.Parent;
            if (parent != null)
                m_parameters.m_stringTree.RemoveNode(treeNode.Name, parent.Name);
            else
                m_parameters.m_stringTree.RemoveNode(treeNode.Name, "");

            RefreshStringTree();
            SelectedNodeChanged(treeViewStrings.SelectedNode);
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
                    CyDeviceDescriptorPage.CleanControls(m_unusedControls);
                    m_unusedControls.Add(m_controlDetails);
                }
                panelDetails.Visible = true;
                CyDescriptorNode node = m_parameters.m_stringTree.GetNodeByKey(selectedNode.Name);
                CyUSBDescriptor descriptor;
                if (node != null)
                {
                    if (node.m_value == null)
                    {
                        toolStripButtonRemove.Visible = false;
                        toolStripButtonAdd.Visible = true;
                    }
                    else
                    {
                        descriptor = node.m_value;
                        switch (descriptor.bDescriptorType)
                        {
                            case CyUSBDescriptorType.STRING:
                                //toolStripButtonAdd.Visible = false;
                                switch (node.Key)
                                {
                                    case CyUSBFSParameters.NODEKEY_STRING_LANGID:
                                        CyDetailsLangID details_langid =
                                            new CyDetailsLangID((CyStringZeroDescriptor) descriptor, m_parameters);
                                        details_langid.Dock = DockStyle.Fill;
                                        panelDetails.Controls.Add(details_langid);
                                        m_controlDetails = details_langid;
                                        toolStripButtonRemove.Visible = false;
                                        break;
                                    case CyUSBFSParameters.NODEKEY_STRING_SERIAL:
                                    case CyUSBFSParameters.NODEKEY_MSOS_STRING:
                                        CyDetailsString details_string =
                                            new CyDetailsString((CyStringDescriptor) descriptor, m_parameters);
                                        details_string.Dock = DockStyle.Fill;
                                        details_string.UpdateNodeEvent += UpdateNodeText;
                                        if (node.Key == CyUSBFSParameters.NODEKEY_STRING_SERIAL)
                                            details_string.SetGroupBoxVisible();
                                        if (node.Key == CyUSBFSParameters.NODEKEY_MSOS_STRING)
                                            details_string.SetTextBoxDefault();
                                        panelDetails.Controls.Add(details_string);
                                        m_controlDetails = details_string;
                                        toolStripButtonRemove.Visible = false;
                                        break;
                                    default:
                                        CyDetailsString details_string1 =
                                            new CyDetailsString((CyStringDescriptor) descriptor, m_parameters);
                                        details_string1.Dock = DockStyle.Fill;
                                        details_string1.UpdateNodeEvent += UpdateNodeText;
                                        panelDetails.Controls.Add(details_string1);
                                        m_controlDetails = details_string1;
                                        toolStripButtonRemove.Visible = true;
                                        break;
                                }
                                break;
                            default:
                                toolStripButtonAdd.Visible = true;
                                break;
                        }
                        if (m_controlDetails != null)
                            m_controlDetails.BringToFront();
                    }
                }
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
        /// Updates the string value of the selected string descriptor in the treeViewStrings if it has been changed
        /// </summary>
        private void UpdateNodeText(object sender, EventArgs e)
        {
            for (int i = 0; i < treeViewStrings.Nodes[0].Nodes.Count; i++)
            {
                TreeNode node = treeViewStrings.Nodes[0].Nodes[i];
                //Not update folders
                if (node.Nodes.Count > 0) continue;

                node.Text = CyDescriptorNode.GetDescriptorLabel(m_parameters.m_stringTree.GetNodeByKey(node.Name));
            }
        }

        /// <summary>
        /// Makes the special strings nodes (Serial Number, MS OS String) visible or invisible
        /// </summary>
        private void UpdateSpecialStrings()
        {
            try
            {
                if ((m_parameters.GetSerialDescriptor()).bUsed)
                {
                    checkBoxShowSerial.Checked = true;
                    m_showSerial = true;
                }
                else
                {
                    checkBoxShowSerial.Checked = false;
                    m_showSerial = false;
                }
                if (((CyStringDescriptor) m_parameters.m_stringTree.m_nodes[1].m_nodes[1].m_value).bUsed)
                {
                    checkBoxShowEE.Checked = true;
                    m_showEE = true;
                }
                else
                {
                    checkBoxShowEE.Checked = false;
                    m_showEE = false;
                }
            }
            catch (Exception)
            {
                checkBoxShowSerial.Checked = m_showSerial;
                checkBoxShowEE.Checked = m_showEE;
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
            m_parameters.ParamStringTreeChanged = true;
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            AddNode();
            m_parameters.ParamStringTreeChanged = true;
        }

        private void checkBoxShowEE_CheckedChanged(object sender, EventArgs e)
        {
            m_showEE = checkBoxShowEE.Checked;
            ((CyStringDescriptor) m_parameters.m_stringTree.m_nodes[1].m_nodes[1].m_value).bUsed = m_showEE;
            m_parameters.ParamStringTreeChanged = true;
            RefreshStringTree();
        }

        private void checkBoxShowSerial_CheckedChanged(object sender, EventArgs e)
        {
            m_showSerial = checkBoxShowSerial.Checked;
            if ((m_parameters.GetSerialDescriptor()).bUsed != m_showSerial)
            {
                (m_parameters.GetSerialDescriptor()).bUsed = m_showSerial;

                m_parameters.SetNodesISerialNumber();

                m_parameters.ParamStringTreeChanged = true;
                RefreshStringTree();
            }
        }

        private void treeViewStrings_KeyDown(object sender, KeyEventArgs e)
        {
            if (toolStripButtonRemove.Visible && toolStripButtonRemove.Enabled)
                if (e.KeyCode == Keys.Delete)
                {
                    RemoveNode();
                    m_parameters.ParamStringTreeChanged = true;
                }
        }

        #endregion Events
    }
}
