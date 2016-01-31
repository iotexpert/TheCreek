/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace USBFS_v1_50
{
    public partial class CyAudioDescriptorPage : UserControl
    {
        #region Const

        private const int TREE_LEVEL_ROOT = 0;
        private const int TREE_LEVEL_INTERFACEGEN = 1;
        private const int TREE_LEVEL_INTERFACE = 2;
        private const int TREE_LEVEL_AUDIO_SPEC = 3;

        private const string LISTVIEW_GROUP_INTERACE = "groupInterface";
        private const string LISTVIEW_GROUP_AC = "groupAC";
        private const string LISTVIEW_GROUP_AS = "groupAS";
        
        #endregion Const

        #region Variables

        public CyUSBFSParameters m_parameters;
        private List<CyUSBDescriptor> m_ItemsList;

        #endregion Variables

        //------------------------------------------------------------------------------------------------------------

        #region Constructors

        public CyAudioDescriptorPage()
        {
            InitializeComponent();
        }

        public CyAudioDescriptorPage(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            InitItems();
            RefreshAudioTree();
        }

        #endregion Constructors

        //------------------------------------------------------------------------------------------------------------

        #region Private functions

        /// <summary>
        /// Initializes m_reportItems list and fills listBoxItems listBox.
        /// </summary>
        private void InitItems()
        {
            m_ItemsList = new List<CyUSBDescriptor>();

            m_ItemsList.Add(new CyAudioInterfaceDescriptor());
            m_ItemsList.Add(new CyAudioInterfaceDescriptor());

            for (int i = 0; i < 2; i++)
            {
                CyInterfaceDescriptor newDesc = (CyAudioInterfaceDescriptor)m_ItemsList[i];
                newDesc.bInterfaceClass = CyUSBFSParameters.CLASS_AUDIO;
                newDesc.bInterfaceSubClass = (i == 0)
                                                 ? (byte) CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL
                                                 : (byte) CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOSTREAMING;
            }

            m_ItemsList.Add(new CyACHeaderDescriptor());
            m_ItemsList.Add(new CyACInputTerminalDescriptor());
            m_ItemsList.Add(new CyACOutputTerminalDescriptor());
            m_ItemsList.Add(new CyACMixerUnitDescriptor());
            m_ItemsList.Add(new CyACSelectorUnitDescriptor());
            m_ItemsList.Add(new CyACFeatureUnitDescriptor());
            m_ItemsList.Add(new CyACProcessingUnitDescriptor());
            m_ItemsList.Add(new CyACExtensionDescriptor());

            m_ItemsList.Add(new CyASGeneralDescriptor());
            m_ItemsList.Add(new CyASFormatType1Descriptor());
            m_ItemsList.Add(new CyASFormatType2Descriptor());
        }

        private void RefreshAudioTree()
        {
            treeViewAudio.BeginUpdate();
            treeViewAudio.Nodes.Clear();
            for (int i = 0; i < m_parameters.m_audioTree.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor = m_parameters.m_audioTree.m_nodes[i];
                TreeNode node = treeViewAudio.Nodes.Add(descriptor.Key,
                                                         CyDescriptorNode.GetDescriptorLabel(descriptor), 0, 0);
                RefreshNode(descriptor, node);
            }

            treeViewAudio.ExpandAll();
            treeViewAudio.EndUpdate();
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
                        continue;
                    case CyUSBDescriptorType.ALTERNATE:
                    case CyUSBDescriptorType.INTERFACE:
                    case CyUSBDescriptorType.AUDIO:
                        imageIndex = 6;
                        break;
                    default:
                        break;
                }

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
            CyDescriptorNode selectedItem = m_parameters.m_hidReportTree.GetNodeByKey(treeViewAudio.SelectedNode.Name);
            CyHidReportItem reportItem;
            if ((selectedItem != null) && (treeViewAudio.SelectedNode.Level > 1))
            {
                reportItem = ((CyHIDReportItemDescriptor) selectedItem.m_value).Item;
                labelItemProperties.Text = string.Format("Item Value ({0})", reportItem.ToString().Trim());
            }
        }

        private void AddAudioNode(CyUSBDescriptor item)
        {
            if (item == null) return;
            
            CyDescriptorNode parentNode, newNode;
            if (item is CyInterfaceDescriptor)
            {
                if (treeViewAudio.SelectedNode != null)
                {
                    if (treeViewAudio.SelectedNode.Level == TREE_LEVEL_ROOT)
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
                if (parentNode.Key == CyUSBFSParameters.NODEKEY_AUDIO) // Create general interface descriptor first
                {
                    // generate name for interface general descriptor
                    int newIndex = parentNode.m_nodes.Count + 1;
                    CyDescriptorNode generalInterfaceNode =
                        new CyDescriptorNode(new CyInterfaceGeneralDescriptor("Audio Interface " + newIndex));
                    generalInterfaceNode.Key = generalInterfaceNode.Key.Replace(CyUSBFSParameters.NODEKEY_USBDESCRIPTOR,
                                                      CyUSBFSParameters.NODEKEY_INTERFACE);
                    parentNode.m_nodes.Add(generalInterfaceNode);
                    parentNode = parentNode.m_nodes[parentNode.m_nodes.Count - 1];
                }
                parentNode.m_nodes.Add(newNode);

                RefreshAudioTree();
                treeViewAudio.SelectedNode = treeViewAudio.Nodes.Find(newNode.Key, true)[0];
            }
        }

        private CyDescriptorNode GetParentDescriptorAtLevel(int level)
        {
            CyDescriptorNode selectedItem = null;
            TreeNode node;
            if (treeViewAudio.SelectedNode != null)
            {
                node = treeViewAudio.SelectedNode;
                while ((node.Level > 0) && (node.Level > level))
                    node = node.Parent;
                selectedItem = m_parameters.m_audioTree.GetNodeByKey(node.Name);
            }
            return selectedItem;
        }

        /// <summary>
        /// Displays the information about the selected node of treeViewReport in the setting panel
        /// </summary>
        private void ShowEditItem()
        {
            if (treeViewAudio.SelectedNode == null) return;

            CyDescriptorNode selectedItem = m_parameters.m_audioTree.GetNodeByKey(treeViewAudio.SelectedNode.Name);
            CyUSBDescriptor audioItem;
            if ((selectedItem != null) && (treeViewAudio.SelectedNode.Level > 1))
            {
                audioItem = selectedItem.m_value;
                propertyGridAudio.SelectedObject = audioItem;
                //Change label text
                labelItemProperties.Text = string.Format("Item Value ({0})", audioItem.ToString().Trim());
                splitContainerDetails.Panel2.BackColor = SystemColors.Control;
            }
            else
            {
                propertyGridAudio.SelectedObject = null;
                if (selectedItem != null)
                {
                    labelItemProperties.Text = string.Format("Item Value ({0})", "");
                    splitContainerDetails.Panel2.BackColor = SystemColors.Control;
                }
            }
        }

        private void TreeViewSelection()
        {
            if (treeViewAudio.SelectedNode == null) return;

            bool includeInterface = true, includeAC = false, includeAS = false;
            switch (treeViewAudio.SelectedNode.Level)
            {
                case TREE_LEVEL_ROOT:
                    break;
                case TREE_LEVEL_INTERFACEGEN:
                    break;
                case TREE_LEVEL_INTERFACE:
                case TREE_LEVEL_AUDIO_SPEC:
                    CyDescriptorNode interfaceNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACE);
                    if (interfaceNode != null)
                    {
                        if (((CyAudioInterfaceDescriptor)interfaceNode.m_value).bInterfaceSubClass ==
                            (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL)
                            includeAC = true;
                        else if (((CyAudioInterfaceDescriptor)interfaceNode.m_value).bInterfaceSubClass ==
                                 (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOSTREAMING)
                            includeAS = true;
                    }
                    break;
                default:
                    break;
            }

            FillListViewItems(includeInterface, includeAC, includeAS);

            if (treeViewAudio.SelectedNode.Level > TREE_LEVEL_INTERFACEGEN)
            {
                listViewAudioDescList.SelectedItems.Clear();
                ShowEditItem();
            }
            else if (listViewAudioDescList.SelectedIndices.Count == 0)
            {
                ShowEditItem();
            }

            if (listViewAudioDescList.SelectedIndices.Count > 0)
                buttonApply.Enabled = true;
            else
            {
                buttonApply.Enabled = false;
            }
        }

        private void FillListViewItems(bool includeInterface, bool includeAC, bool includeAS)
        {
            listViewAudioDescList.BeginUpdate();
            listViewAudioDescList.Items.Clear();
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                ListViewItem item = new ListViewItem(m_ItemsList[i].ToString()); 
                item.ImageIndex = 5;
                item.Tag = i;

                if (m_ItemsList[i] is CyInterfaceDescriptor && includeInterface)
                {
                    item.Group = listViewAudioDescList.Groups[LISTVIEW_GROUP_INTERACE];
                    item.Text = ((CyInterfaceDescriptor) m_ItemsList[i]).bInterfaceSubClass ==
                                (byte) CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL
                                    ? "Audio Control"
                                    : "Audio Streaming";
                    listViewAudioDescList.Items.Add(item);
                }
                else if (((m_ItemsList[i] is CyACHeaderDescriptor) ||
                         (m_ItemsList[i] is CyACInputTerminalDescriptor) ||
                         (m_ItemsList[i] is CyACOutputTerminalDescriptor) ||
                         (m_ItemsList[i] is CyACMixerUnitDescriptor) ||
                         (m_ItemsList[i] is CyACSelectorUnitDescriptor) ||
                         (m_ItemsList[i] is CyACFeatureUnitDescriptor) ||
                         (m_ItemsList[i] is CyACProcessingUnitDescriptor) ||
                         (m_ItemsList[i] is CyACExtensionDescriptor)) && includeAC)
                {
                    item.Group = listViewAudioDescList.Groups[LISTVIEW_GROUP_AC];
                    listViewAudioDescList.Items.Add(item);
                }
                else if (((m_ItemsList[i] is CyASGeneralDescriptor) || 
                          (m_ItemsList[i] is CyASFormatType1Descriptor) || 
                          (m_ItemsList[i] is CyASFormatType2Descriptor)) && includeAS)
                {
                    item.Group = listViewAudioDescList.Groups[LISTVIEW_GROUP_AS];
                    listViewAudioDescList.Items.Add(item);
                }
            }
            listViewAudioDescList.EndUpdate();
        }

        private void RemoveAudioTreeItem()
        {
            if (treeViewAudio.SelectedNode == null) return;

            TreeNode treeNode = treeViewAudio.SelectedNode;
            TreeNode prevNode = treeNode.PrevNode;
            if (prevNode == null)
                prevNode = treeNode.Parent;

            if (treeNode.Level == TREE_LEVEL_ROOT) return;

            TreeNode parent = treeNode.Parent;
            if (parent != null)
                m_parameters.m_audioTree.RemoveNode(treeNode.Name, parent.Name);
          
            m_parameters.ParamAudioTreeChanged = true;
            RefreshAudioTree();
            //Select previous node
            if (prevNode != null)
                treeViewAudio.SelectedNode = treeViewAudio.Nodes.Find(prevNode.Name, true)[0];
        }

        #endregion Private functions

        #region Events

        private void CyAudioDescriptorPage_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                RefreshAudioTree();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (listViewAudioDescList.SelectedIndices.Count == 0) return;

            // Add to the tree
            AddAudioNode((CyUSBDescriptor)propertyGridAudio.SelectedObject);
          
            m_parameters.ParamAudioTreeChanged = true;
        }

        private void treeViewReport_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewSelection();
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            RemoveAudioTreeItem();
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
            if (treeViewAudio.SelectedNode == null)
                return;
            CyDescriptorNode node = m_parameters.m_audioTree.GetNodeByKey(treeViewAudio.SelectedNode.Name);
            if (e.Label == null)
            {
                treeViewAudio.SelectedNode.Text = ((CyInterfaceGeneralDescriptor)node.m_value).DisplayName;
            }
            else if ((node.m_value is CyInterfaceGeneralDescriptor) && 
                     (((CyInterfaceGeneralDescriptor)node.m_value).DisplayName != e.Label))
            {
                ((CyInterfaceGeneralDescriptor)node.m_value).DisplayName = e.Label;
                m_parameters.ParamAudioTreeChanged = true;
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
                    m_oldNode.BackColor = treeViewAudio.Nodes[0].BackColor;
                    m_oldNode.ForeColor = treeViewAudio.Nodes[0].ForeColor;
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
                m_oldNode.BackColor = treeViewAudio.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewAudio.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragLeave(object sender, EventArgs e)
        {
            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewAudio.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewAudio.Nodes[0].ForeColor;
            }
        }

        private void treeViewReport_DragDrop(object sender, DragEventArgs e)
        {
            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewAudio.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewAudio.Nodes[0].ForeColor;
            }

            if (e.Data.GetDataPresent(CyUSBFSParameters.TYPE_TREE_NODE, false))
            {
                Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = ((TreeView) sender).GetNodeAt(pt);
                TreeNode newNode = (TreeNode)e.Data.GetData(CyUSBFSParameters.TYPE_TREE_NODE);
                TreeNode sourceParent = newNode.Parent;
                TreeNode destParent;
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
                    CyDescriptorNode sourceDesc = m_parameters.m_audioTree.GetNodeByKey(newNode.Name);
                    CyDescriptorNode destinationDesc = m_parameters.m_audioTree.GetNodeByKey(destinationNode.Name);
                    CyDescriptorNode sourceParentDesc = m_parameters.m_audioTree.GetNodeByKey(sourceParent.Name);
                    CyDescriptorNode destParentDesc = m_parameters.m_audioTree.GetNodeByKey(destParent.Name);

                    if ((sourceDesc != null) && (destinationDesc != null) && 
                        (sourceParentDesc != null) && (destParent != null) &&
                        (destParent.Level == sourceParent.Level))
                    {
                        sourceParentDesc.m_nodes.Remove(sourceDesc);
                        if (destinationNode == destParent)
                            destParentDesc.m_nodes.Add(sourceDesc);
                        else
                            destParentDesc.m_nodes.Insert(destParentDesc.m_nodes.IndexOf(destinationDesc), sourceDesc);

                        m_parameters.ParamAudioTreeChanged = true;
                        RefreshAudioTree();
                        treeViewAudio.SelectedNode = treeViewAudio.Nodes.Find(newNode.Name, true)[0];
                    }
                }
            }
        }

        private void treeViewReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveAudioTreeItem();
            }
        }

        private void listViewAudioDescList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAudioDescList.SelectedIndices.Count > 0)
            {
                propertyGridAudio.SelectedObject =
                    m_ItemsList[Convert.ToInt32(listViewAudioDescList.SelectedItems[0].Tag)].Clone();
                buttonApply.Enabled = true;
                splitContainerDetails.Panel2.BackColor = Color.LightSteelBlue;
            }
        }

        private void panelAddbtn_Resize(object sender, EventArgs e)
        {
            buttonApply.Left = (panelAddbtn.Width - buttonApply.Width)/2;
        }

        private void propertyGridAudio_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Update String Descriptor information if string was changed
            if (e.ChangedItem.Label.StartsWith("i"))
            {
                uint strIndex = m_parameters.SaveStringDescriptor(e.ChangedItem.Value.ToString());
                try
                {
                    Type objType = propertyGridAudio.SelectedObject.GetType();
                    string fName = e.ChangedItem.Label.Remove(0, 1).Insert(0, "iw");
                    FieldInfo fi = objType.GetField(fName);
                    if (fi != null)
                    {
                        fi.SetValue(propertyGridAudio.SelectedObject, strIndex);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.ToString());
                }
            }
            else if (e.ChangedItem.Label == "bAlternateSetting")
            {
                TreeNode node = treeViewAudio.SelectedNode;
                if (node != null)
                    node.Text = CyDescriptorNode.GetDescriptorLabel(m_parameters.m_audioTree.GetNodeByKey(node.Name));
            }
            propertyGridAudio.Refresh();
            m_parameters.ParamAudioTreeChanged = true;
        }

        #endregion Events
    }
}