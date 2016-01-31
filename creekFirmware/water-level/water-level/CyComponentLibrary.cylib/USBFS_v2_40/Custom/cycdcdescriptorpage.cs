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
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace USBFS_v2_40
{
    public partial class CyCDCDescriptorPage : UserControl
    {
        #region Const

        private const int TREE_LEVEL_ROOT = 0;
        private const int TREE_LEVEL_INTERFACEGEN = 1;
        private const int TREE_LEVEL_INTERFACE = 2;
        private const int TREE_LEVEL_CDC_SPEC = 3;
        private const int TREE_LEVEL_EP_SPEC = 4;

        private const string LISTVIEW_GROUP_INTERACE = "groupInterface";
        private const string LISTVIEW_GROUP_COMM = "groupCommunications";
        private const string LISTVIEW_GROUP_ENDPOINT = "groupEndpoint";
        
        #endregion Const

        #region Variables

        public CyUSBFSParameters m_parameters;
        private List<CyUSBDescriptor> m_ItemsList;

        #endregion Variables

        //------------------------------------------------------------------------------------------------------------

        #region Constructors

        public CyCDCDescriptorPage()
        {
            InitializeComponent();
        }

        public CyCDCDescriptorPage(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            InitEnableAPICheckBox();
            InitItems();
            RefreshCDCTree();
        }

        #endregion Constructors

        //------------------------------------------------------------------------------------------------------------

        #region Private functions

        public void InitEnableAPICheckBox()
        {
            checkBoxEnableCDCApi.Checked = m_parameters.EnableCDCApi;
        }

        /// <summary>
        /// Initializes m_reportItems list and fills listBoxItems listBox.
        /// </summary>
        private void InitItems()
        {
            m_ItemsList = new List<CyUSBDescriptor>();

            m_ItemsList.Add(new CyCommunicationsInterfaceDescriptor());
            m_ItemsList.Add(new CyDataInterfaceDescriptor());

            ((CyCDCInterfaceDescriptor)m_ItemsList[0]).bInterfaceClass = CyUSBFSParameters.CLASS_CDC;
            ((CyCDCInterfaceDescriptor)m_ItemsList[1]).bInterfaceClass = CyUSBFSParameters.CLASS_DATA;

            m_ItemsList.Add(new CyCDCHeaderDescriptor());
            m_ItemsList.Add(new CyCDCUnionDescriptor());
            m_ItemsList.Add(new CyCDCCountrySelectionDescriptor());
            m_ItemsList.Add(new CyCDCCallManagementDescriptor());
            m_ItemsList.Add(new CyCDCAbstractControlMgmtDescriptor());

            m_ItemsList.Add(new CyEndpointDescriptor());
        }

        private void RefreshCDCTree()
        {
            treeViewCDC.BeginUpdate();
            treeViewCDC.Nodes.Clear();
            for (int i = 0; i < m_parameters.m_cdcTree.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor = m_parameters.m_cdcTree.m_nodes[i];
                TreeNode node = treeViewCDC.Nodes.Add(descriptor.Key,
                                                         CyDescriptorNode.GetDescriptorLabel(descriptor), 0, 0);
                RefreshNode(descriptor, node);
            }

            treeViewCDC.ExpandAll();
            treeViewCDC.EndUpdate();
        }

        private void RefreshNode(CyDescriptorNode descriptor, TreeNode treeNode)
        {
            int imageIndex = 0;
            for (int i = 0; i < descriptor.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor_child = descriptor.m_nodes[i];
                TreeNode node;
                
                switch (descriptor_child.m_value.bDescriptorType)
                {
                    case CyUSBDescriptorType.ENDPOINT:
                    case CyUSBDescriptorType.ALTERNATE:
                    case CyUSBDescriptorType.INTERFACE:
                    case CyUSBDescriptorType.CDC:
                        imageIndex = 6;
                        break;
                    default:
                        break;
                }
                
                // Find a place to insert the node
                if (descriptor_child.m_value.bDescriptorType == CyUSBDescriptorType.CDC)
                {
                    int nodeIndex = 0;
                    for (int j = 0; j < descriptor.m_nodes.IndexOf(descriptor_child); j++)
                    {
                        if (descriptor.m_nodes[j].m_value.bDescriptorType == CyUSBDescriptorType.CDC)
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
                
                RefreshNode(descriptor_child, node);
            }
        }

        /// <summary>
        /// Refresh the header text of the Settings panel depending on value of the item that is currently configured
        /// </summary>
        public void RefreshSettingsHeader()
        {
            CyDescriptorNode selectedItem = m_parameters.m_cdcTree.GetNodeByKey(treeViewCDC.SelectedNode.Name);
            CyHidReportItem reportItem;
            if ((selectedItem != null) && (treeViewCDC.SelectedNode.Level > 1))
            {
                reportItem = ((CyHIDReportItemDescriptor) selectedItem.m_value).Item;
                labelItemProperties.Text = string.Format("Item Value ({0})", reportItem.ToString().Trim());
            }
        }

        private void AddCDCNode(CyUSBDescriptor item)
        {
            if (item == null) return;
            
            CyDescriptorNode parentNode, newNode;
            if (item is CyInterfaceDescriptor)
            {
                if (treeViewCDC.SelectedNode != null)
                {
                    if (treeViewCDC.SelectedNode.Level == TREE_LEVEL_ROOT)
                        parentNode = GetParentDescriptorAtLevel(TREE_LEVEL_ROOT);
                    else
                        parentNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACEGEN);
                }
                else parentNode = null;

            }
            else
            {
                parentNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACE);
            }

            newNode = new CyDescriptorNode(item);

            if (parentNode != null)
            {
                if (parentNode.Key == CyUSBFSParameters.NODEKEY_CDC) // Create general interface descriptor first
                {
                    // generate name for interface general descriptor
                    int newIndex = parentNode.m_nodes.Count + 1;
                    CyDescriptorNode generalInterfaceNode =
                        new CyDescriptorNode(new CyInterfaceGeneralDescriptor("CDC Interface " + newIndex));
                    generalInterfaceNode.Key = generalInterfaceNode.Key.Replace(CyUSBFSParameters.NODEKEY_USBDESCRIPTOR,
                                                      CyUSBFSParameters.NODEKEY_INTERFACE);
                    parentNode.m_nodes.Add(generalInterfaceNode);
                    parentNode = parentNode.m_nodes[parentNode.m_nodes.Count - 1];
                }
                parentNode.m_nodes.Add(newNode);

                RefreshCDCTree();
                treeViewCDC.SelectedNode = treeViewCDC.Nodes.Find(newNode.Key, true)[0];
            }
        }

        private CyDescriptorNode GetParentDescriptorAtLevel(int level)
        {
            CyDescriptorNode selectedItem = null;
            TreeNode node;
            if (treeViewCDC.SelectedNode != null)
            {
                node = treeViewCDC.SelectedNode;
                while ((node.Level > 0) && (node.Level > level))
                    node = node.Parent;
                selectedItem = m_parameters.m_cdcTree.GetNodeByKey(node.Name);
            }
            return selectedItem;
        }

        /// <summary>
        /// Displays the information about the selected node of treeViewReport in the setting panel
        /// </summary>
        private void ShowEditItem()
        {
            if (treeViewCDC.SelectedNode == null) return;

            CyDescriptorNode selectedItem = m_parameters.m_cdcTree.GetNodeByKey(treeViewCDC.SelectedNode.Name);
            CyUSBDescriptor cdcItem;
            if ((selectedItem != null) && (treeViewCDC.SelectedNode.Level > 1))
            {
                cdcItem = selectedItem.m_value;
                propertyGridCDC.SelectedObject = cdcItem;
                //Change label text
                labelItemProperties.Text = string.Format("Item Value ({0})", cdcItem.ToString().Trim());
                splitContainerDetails.Panel2.BackColor = SystemColors.Control;
            }
            else
            {
                propertyGridCDC.SelectedObject = null;
                if (selectedItem != null)
                {
                    labelItemProperties.Text = string.Format("Item Value ({0})", "");
                    splitContainerDetails.Panel2.BackColor = SystemColors.Control;
                }
            }
        }

        private void TreeViewSelection()
        {
            if (treeViewCDC.SelectedNode == null) return;

            toolStripButtonRemove.Enabled = treeViewCDC.SelectedNode.Level > 0;

            bool includeInterface = true, includeAC = false, includeEP = false;
            switch (treeViewCDC.SelectedNode.Level)
            {
                case TREE_LEVEL_ROOT:
                    break;
                case TREE_LEVEL_INTERFACEGEN:
                    break;
                case TREE_LEVEL_INTERFACE:
                case TREE_LEVEL_CDC_SPEC:
                    CyDescriptorNode interfaceNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACE);
                    if (interfaceNode != null)
                    {
                        if (interfaceNode.m_value is CyCDCInterfaceDescriptor)
                        {
                            if (((CyCDCInterfaceDescriptor)interfaceNode.m_value).bInterfaceClass ==
                                CyUSBFSParameters.CLASS_CDC)
                            {
                                includeAC = true;
                            }
                        }

                        includeEP = true;
                    }
                    break;
                default:
                    break;
            }

            FillListViewItems(includeInterface, includeAC, includeEP);

            if (treeViewCDC.SelectedNode.Level > TREE_LEVEL_INTERFACEGEN)
            {
                listViewCDCDescList.SelectedItems.Clear();
                ShowEditItem();
            }
            else if (listViewCDCDescList.SelectedIndices.Count == 0)
            {
                ShowEditItem();
            }

            if (listViewCDCDescList.SelectedIndices.Count > 0)
                buttonApply.Enabled = true;
            else
            {
                buttonApply.Enabled = false;
            }
        }

        private void FillListViewItems(bool includeInterface, bool includeAC, bool includeEP)
        {
            listViewCDCDescList.BeginUpdate();
            listViewCDCDescList.Items.Clear();
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                ListViewItem item = new ListViewItem(m_ItemsList[i].ToString()); 
                item.ImageIndex = 5;
                item.Tag = i;

                if (m_ItemsList[i] is CyInterfaceDescriptor && includeInterface)
                {
                    item.Group = listViewCDCDescList.Groups[LISTVIEW_GROUP_INTERACE];
                    item.Text = ((CyInterfaceDescriptor) m_ItemsList[i]).bInterfaceClass ==
                                CyUSBFSParameters.CLASS_CDC
                                    ? "Communications"
                                    : "Data";
                    listViewCDCDescList.Items.Add(item);
                }
                else if (((m_ItemsList[i] is CyCDCHeaderDescriptor) ||
                          (m_ItemsList[i] is CyCDCUnionDescriptor) ||
                          (m_ItemsList[i] is CyCDCCountrySelectionDescriptor) ||
                          (m_ItemsList[i] is CyCDCCallManagementDescriptor) ||
                          (m_ItemsList[i] is CyCDCAbstractControlMgmtDescriptor)) && includeAC)
                {
                    item.Group = listViewCDCDescList.Groups[LISTVIEW_GROUP_COMM];
                    listViewCDCDescList.Items.Add(item);
                }
                else if ((m_ItemsList[i] is CyEndpointDescriptor) && includeEP)
                {
                    item.Group = listViewCDCDescList.Groups[LISTVIEW_GROUP_ENDPOINT];
                    listViewCDCDescList.Items.Add(item);
                }
            }
            listViewCDCDescList.EndUpdate();
        }

        private void RemoveCDCTreeItem()
        {
            if (treeViewCDC.SelectedNode == null) return;

            TreeNode treeNode = treeViewCDC.SelectedNode;
            TreeNode prevNode = treeNode.PrevNode;
            if (prevNode == null)
                prevNode = treeNode.Parent;

            if (treeNode.Level == TREE_LEVEL_ROOT) return;

            TreeNode parent = treeNode.Parent;
            if (parent != null)
                m_parameters.m_cdcTree.RemoveNode(treeNode.Name, parent.Name);
          
            m_parameters.ParamCDCTreeChanged = true;
            RefreshCDCTree();
            //Select previous node
            if (prevNode != null)
                treeViewCDC.SelectedNode = treeViewCDC.Nodes.Find(prevNode.Name, true)[0];
        }

        #endregion Private functions

        #region Events

        private void CyCDCDescriptorPage_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                RefreshCDCTree();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (listViewCDCDescList.SelectedIndices.Count == 0) return;

            // Add to the tree
            AddCDCNode((CyUSBDescriptor)propertyGridCDC.SelectedObject);
          
            m_parameters.ParamCDCTreeChanged = true;
        }

        private void treeViewReport_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewSelection();
        }

        private void treeViewCDC_Enter(object sender, EventArgs e)
        {
            TreeViewSelection();
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            RemoveCDCTreeItem();
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
            if (treeViewCDC.SelectedNode == null)
                return;
            CyDescriptorNode node = m_parameters.m_cdcTree.GetNodeByKey(treeViewCDC.SelectedNode.Name);
            if (e.Label == null)
            {
                treeViewCDC.SelectedNode.Text = ((CyInterfaceGeneralDescriptor)node.m_value).DisplayName;
            }
            else if ((node.m_value is CyInterfaceGeneralDescriptor) && 
                     (((CyInterfaceGeneralDescriptor)node.m_value).DisplayName != e.Label))
            {
                ((CyInterfaceGeneralDescriptor)node.m_value).DisplayName = e.Label;
                m_parameters.ParamCDCTreeChanged = true;
            }
        }

        private void treeViewReport_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if ((e.Item is TreeNode) && (((TreeNode)e.Item).Level > TREE_LEVEL_ROOT))
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
            if ((aNode != null) && (e.Data.GetDataPresent(CyUSBFSParameters.TYPE_TREE_NODE, false)))
            {
                // If the node is a folder, change the color of the background to dark blue to simulate selection
                // Be sure to return the previous node to its original color by copying from a blank node
                if ((m_oldNode != null) && (m_oldNode != aNode))
                {
                    m_oldNode.BackColor = treeViewCDC.Nodes[0].BackColor;
                    m_oldNode.ForeColor = treeViewCDC.Nodes[0].ForeColor;
                }
                TreeNode dragNode = (TreeNode)e.Data.GetData(CyUSBFSParameters.TYPE_TREE_NODE);
                if ((aNode.Level == dragNode.Level) || (aNode.Level == dragNode.Level - 1))
                {
                    aNode.BackColor = Color.LightBlue;
                    aNode.ForeColor = Color.White;
                    m_oldNode = aNode;
                }
            }
            if ((m_oldNode != null) && (m_oldNode != aNode))
            {
                m_oldNode.BackColor = treeViewCDC.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewCDC.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragLeave(object sender, EventArgs e)
        {
            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewCDC.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewCDC.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragDrop(object sender, DragEventArgs e)
        {
            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewCDC.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewCDC.Nodes[0].ForeColor;
            }

            if (e.Data.GetDataPresent(CyUSBFSParameters.TYPE_TREE_NODE, false))
            {
                Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = ((TreeView) sender).GetNodeAt(pt);
                TreeNode newNode = (TreeNode)e.Data.GetData(CyUSBFSParameters.TYPE_TREE_NODE);
                TreeNode sourceParent = newNode.Parent;
                TreeNode destParent;

                if (destinationNode == null)
                    return;

                if (destinationNode.Level == newNode.Level)
                {
                    destParent = destinationNode.Parent;
                }
                else if (destinationNode.Level == newNode.Level - 1)
                {
                    destParent = destinationNode;
                }
                // return if levels are not correspondent
                else return;

                if (destinationNode != newNode)
                {
                    CyDescriptorNode sourceDesc = m_parameters.m_cdcTree.GetNodeByKey(newNode.Name);
                    CyDescriptorNode destinationDesc = m_parameters.m_cdcTree.GetNodeByKey(destinationNode.Name);
                    CyDescriptorNode sourceParentDesc = m_parameters.m_cdcTree.GetNodeByKey(sourceParent.Name);
                    CyDescriptorNode destParentDesc = m_parameters.m_cdcTree.GetNodeByKey(destParent.Name);

                    if ((sourceDesc != null) && (destinationDesc != null) && 
                        (sourceParentDesc != null) && (destParent != null) &&
                        (destParent.Level == sourceParent.Level))
                    {
                        sourceParentDesc.m_nodes.Remove(sourceDesc);
                        if (destinationNode == destParent)
                            destParentDesc.m_nodes.Add(sourceDesc);
                        else
                            destParentDesc.m_nodes.Insert(destParentDesc.m_nodes.IndexOf(destinationDesc), sourceDesc);

                        m_parameters.ParamCDCTreeChanged = true;
                        RefreshCDCTree();
                        treeViewCDC.SelectedNode = treeViewCDC.Nodes.Find(newNode.Name, true)[0];
                    }
                }
            }
        }

        private void treeViewReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveCDCTreeItem();
            }
        }

        private void listViewCDCDescList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCDCDescList.SelectedIndices.Count > 0)
            {
                propertyGridCDC.SelectedObject =
                    m_ItemsList[Convert.ToInt32(listViewCDCDescList.SelectedItems[0].Tag)].Clone();
                buttonApply.Enabled = true;
                splitContainerDetails.Panel2.BackColor = Color.LightSteelBlue;
                labelItemProperties.Text = string.Format("Item Value ({0})", propertyGridCDC.SelectedObject.
                                                                             ToString().Trim());
            }
        }

        private void propertyGridCDC_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Update String Descriptor information if string was changed
            if (e.ChangedItem.Label.StartsWith("i"))
            {
                uint strIndex = m_parameters.SaveStringDescriptor(e.ChangedItem.Value.ToString());
                try
                {
                    Type objType = propertyGridCDC.SelectedObject.GetType();
                    string fName = e.ChangedItem.Label.Remove(0, 1).Insert(0, "iw");
                    FieldInfo fi = objType.GetField(fName);
                    if (fi != null)
                    {
                        fi.SetValue(propertyGridCDC.SelectedObject, strIndex);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.ToString());
                }
            }
            // Change number of alternate interface in the tree
            else if (e.ChangedItem.Label == "bAlternateSetting")
            {
                TreeNode node = treeViewCDC.SelectedNode;
                if (node != null)
                    node.Text = CyDescriptorNode.GetDescriptorLabel(m_parameters.m_cdcTree.GetNodeByKey(node.Name));
            }
            propertyGridCDC.Refresh();
            m_parameters.ParamCDCTreeChanged = true;
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            CyDescriptorNode descNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACEGEN);
            if (descNode != null)
            {
                m_parameters.SaveDescriptorList(descNode, CyUSBDescriptorType.CDC);
            }
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            if (m_parameters.ImportDescriptorList(CyUSBDescriptorType.CDC))
            {
                m_parameters.ParamStringTreeChanged = true;
                m_parameters.ParamCDCTreeChanged = true;
            }
            RefreshCDCTree();
        }

        private void checkBoxEnableCDCApi_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.EnableCDCApi = checkBoxEnableCDCApi.Checked;
        }

        #endregion Events

        #region ProcessCmdKey() override
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                if (toolStripButtonSave.Enabled && toolStripButtonSave.Visible)
                {
                    toolStripButtonSave_Click(null, new EventArgs());
                }
                return true;
            }
            else if (keyData == (Keys.Control | Keys.O))
            {
                if (toolStripButtonImport.Enabled && toolStripButtonImport.Visible)
                {
                    toolStripButtonImport_Click(null, new EventArgs());
                }
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion ProcessCmdKey() override
    }
}
