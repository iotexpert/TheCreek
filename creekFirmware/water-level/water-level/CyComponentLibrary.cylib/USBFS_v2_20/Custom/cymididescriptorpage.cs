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

namespace USBFS_v2_20
{
    public partial class CyMidiDescriptorPage : UserControl
    {
        #region Const

        private const int TREE_LEVEL_ROOT = 0;
        private const int TREE_LEVEL_INTERFACEGEN = 1;
        private const int TREE_LEVEL_INTERFACE = 2;
        private const int TREE_LEVEL_AUDIO_SPEC = 3;
        private const int TREE_LEVEL_EP_SPEC = 4;

        private const string LISTVIEW_GROUP_INTERACE = "groupInterface";
        private const string LISTVIEW_GROUP_AC = "groupAC";
        private const string LISTVIEW_GROUP_AC2 = "groupAC2";
        private const string LISTVIEW_GROUP_MS = "groupMS";
        private const string LISTVIEW_GROUP_ENDPOINT = "groupEndpoint";
        
        #endregion Const

        #region Variables

        public CyUSBFSParameters m_parameters;
        private List<CyUSBDescriptor> m_ItemsList;

        #endregion Variables

        //------------------------------------------------------------------------------------------------------------

        #region Constructors

        public CyMidiDescriptorPage()
        {
            InitializeComponent();
        }

        public CyMidiDescriptorPage(CyUSBFSParameters parameters)
        {
            InitializeComponent();

            m_parameters = parameters;
            InitEnableAPICheckBox();
            InitModeCheckBox();
            InitItems();
            RefreshMidiTree();
        }

        #endregion Constructors

        //------------------------------------------------------------------------------------------------------------

        #region Private functions

        public void InitEnableAPICheckBox()
        {
            checkBoxEnableMIDIApi.Checked = m_parameters.EnableMIDIApi;
        }
        public void InitModeCheckBox()
        {
            checkBoxMode.Checked = m_parameters.Mode;
        }

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
                                                 ? (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL
                                                 : (byte)CyUSBOtherTypes.CyAudioSubclassCodes.MIDISTREAMING;
            }

            m_ItemsList.Add(new CyACHeaderDescriptor());
            m_ItemsList.Add(new CyACInputTerminalDescriptor());
            m_ItemsList.Add(new CyACOutputTerminalDescriptor());
            m_ItemsList.Add(new CyACMixerUnitDescriptor());
            m_ItemsList.Add(new CyACSelectorUnitDescriptor());
            m_ItemsList.Add(new CyACFeatureUnitDescriptor());
            m_ItemsList.Add(new CyACProcessingUnitDescriptor());
            m_ItemsList.Add(new CyACExtensionDescriptor());

            m_ItemsList.Add(new CyACHeaderDescriptor_v2());
            m_ItemsList.Add(new CyACClockSourceDescriptor_v2());
            m_ItemsList.Add(new CyACClockSelectorDescriptor_v2());
            m_ItemsList.Add(new CyACClockMultiplierDescriptor_v2());
            m_ItemsList.Add(new CyACInputTerminalDescriptor_v2());
            m_ItemsList.Add(new CyACOutputTerminalDescriptor_v2());
            m_ItemsList.Add(new CyACMixerUnitDescriptor_v2());
            m_ItemsList.Add(new CyACSelectorUnitDescriptor_v2());
            m_ItemsList.Add(new CyACFeatureUnitDescriptor_v2());
            m_ItemsList.Add(new CyACSamplingRateConverterDescriptor_v2());
            m_ItemsList.Add(new CyACEffectUnitDescriptor_v2());
            m_ItemsList.Add(new CyACProcessingUnitDescriptor_v2());
            m_ItemsList.Add(new CyACExtensionDescriptor_v2());

            m_ItemsList.Add(new CyMSHeaderDescriptor());
            m_ItemsList.Add(new CyMSInJackDescriptor());
            m_ItemsList.Add(new CyMSOutJackDescriptor());
            m_ItemsList.Add(new CyMSElementDescriptor());

            m_ItemsList.Add(new CyAudioEndpointDescriptor());
            m_ItemsList.Add(new CyMSEndpointDescriptor());
        }

        private void RefreshMidiTree()
        {
            treeViewMidi.BeginUpdate();
            treeViewMidi.Nodes.Clear();
            for (int i = 0; i < m_parameters.m_midiTree.m_nodes.Count; i++)
            {
                CyDescriptorNode descriptor = m_parameters.m_midiTree.m_nodes[i];
                TreeNode node = treeViewMidi.Nodes.Add(descriptor.Key,
                                                         CyDescriptorNode.GetDescriptorLabel(descriptor), 0, 0);
                RefreshNode(descriptor, node);
            }

            treeViewMidi.ExpandAll();
            treeViewMidi.EndUpdate();
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
                    case CyUSBDescriptorType.CS_ENDPOINT:
                    case CyUSBDescriptorType.ALTERNATE:
                    case CyUSBDescriptorType.INTERFACE:
                    case CyUSBDescriptorType.AUDIO:
                        imageIndex = 6;
                        break;
                    default:
                        break;
                }
                
                // Find a place to insert the node
                if (descriptor_child.m_value.bDescriptorType == CyUSBDescriptorType.AUDIO)
                {
                    int nodeIndex = 0;
                    for (int j = 0; j < descriptor.m_nodes.IndexOf(descriptor_child); j++)
                    {
                        if (descriptor.m_nodes[j].m_value.bDescriptorType == CyUSBDescriptorType.AUDIO)
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
            CyDescriptorNode selectedItem = m_parameters.m_midiTree.GetNodeByKey(treeViewMidi.SelectedNode.Name);
            CyHidReportItem reportItem;
            if ((selectedItem != null) && (treeViewMidi.SelectedNode.Level > 1))
            {
                reportItem = ((CyHIDReportItemDescriptor) selectedItem.m_value).Item;
                labelItemProperties.Text = string.Format("Item Value ({0})", reportItem.ToString().Trim());
            }
        }

        private void AddMidiNode(CyUSBDescriptor item)
        {
            if (item == null) return;
            
            CyDescriptorNode parentNode, newNode;
            if (item is CyInterfaceDescriptor)
            {
                if (treeViewMidi.SelectedNode != null)
                {
                    if (treeViewMidi.SelectedNode.Level == TREE_LEVEL_ROOT)
                        parentNode = GetParentDescriptorAtLevel(TREE_LEVEL_ROOT);
                    else
                        parentNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACEGEN);
                }
                else parentNode = null;

            }
            else if (item is CyMSEndpointDescriptor)
            {
                parentNode = GetParentDescriptorAtLevel(TREE_LEVEL_AUDIO_SPEC);
            }
            else
            {
                parentNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACE);
            }

            newNode = new CyDescriptorNode(item);

            if (parentNode != null)
            {
                if (parentNode.Key == CyUSBFSParameters.NODEKEY_MIDI) // Create general interface descriptor first
                {
                    // generate name for interface general descriptor
                    int newIndex = parentNode.m_nodes.Count + 1;
                    CyDescriptorNode generalInterfaceNode =
                        new CyDescriptorNode(new CyInterfaceGeneralDescriptor("MIDI Interface " + newIndex));
                    generalInterfaceNode.Key = generalInterfaceNode.Key.Replace(CyUSBFSParameters.NODEKEY_USBDESCRIPTOR,
                                                      CyUSBFSParameters.NODEKEY_INTERFACE);
                    parentNode.m_nodes.Add(generalInterfaceNode);
                    parentNode = parentNode.m_nodes[parentNode.m_nodes.Count - 1];
                }
                parentNode.m_nodes.Add(newNode);

                RefreshMidiTree();
                treeViewMidi.SelectedNode = treeViewMidi.Nodes.Find(newNode.Key, true)[0];
            }
        }

        private CyDescriptorNode GetParentDescriptorAtLevel(int level)
        {
            CyDescriptorNode selectedItem = null;
            TreeNode node;
            if (treeViewMidi.SelectedNode != null)
            {
                node = treeViewMidi.SelectedNode;
                while ((node.Level > 0) && (node.Level > level))
                    node = node.Parent;
                selectedItem = m_parameters.m_midiTree.GetNodeByKey(node.Name);
            }
            return selectedItem;
        }

        /// <summary>
        /// Displays the information about the selected node of treeViewMidi in the setting panel
        /// </summary>
        private void ShowEditItem()
        {
            if (treeViewMidi.SelectedNode == null) return;

            CyDescriptorNode selectedItem = m_parameters.m_midiTree.GetNodeByKey(treeViewMidi.SelectedNode.Name);
            CyUSBDescriptor midiItem;
            if ((selectedItem != null) && (treeViewMidi.SelectedNode.Level > 1))
            {
                midiItem = selectedItem.m_value;
                propertyGridMidi.SelectedObject = midiItem;
                //Change label text
                labelItemProperties.Text = string.Format("Item Value ({0})", midiItem.ToString().Trim());
                splitContainerDetails.Panel2.BackColor = SystemColors.Control;
            }
            else
            {
                propertyGridMidi.SelectedObject = null;
                if (selectedItem != null)
                {
                    labelItemProperties.Text = string.Format("Item Value ({0})", "");
                    splitContainerDetails.Panel2.BackColor = SystemColors.Control;
                }
            }
        }

        private void TreeViewSelection()
        {
            if (treeViewMidi.SelectedNode == null) return;

            toolStripButtonRemove.Enabled = treeViewMidi.SelectedNode.Level > 0;

            bool includeInterface = true, includeAC = false, includeMS = false, includeEP = false, includeASEP = false;
            switch (treeViewMidi.SelectedNode.Level)
            {
                case TREE_LEVEL_ROOT:
                    break;
                case TREE_LEVEL_INTERFACEGEN:
                    break;
                case TREE_LEVEL_INTERFACE:
                case TREE_LEVEL_AUDIO_SPEC:
                case TREE_LEVEL_EP_SPEC:
                    CyDescriptorNode interfaceNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACE);
                    if (interfaceNode != null)
                    {
                        if (interfaceNode.m_value is CyAudioInterfaceDescriptor)
                        {
                            if (((CyAudioInterfaceDescriptor)interfaceNode.m_value).bInterfaceSubClass ==
                                (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL)
                                includeAC = true;
                            else if (((CyAudioInterfaceDescriptor) interfaceNode.m_value).bInterfaceSubClass ==
                                (byte) CyUSBOtherTypes.CyAudioSubclassCodes.MIDISTREAMING)
                            {
                                includeMS = true;

                                CyDescriptorNode currNode =
                                    m_parameters.m_midiTree.GetNodeByKey(treeViewMidi.SelectedNode.Name);
                                if ((currNode != null) && (currNode.m_value is CyAudioEndpointDescriptor))
                                    includeASEP = true;
                            }
                        }

                        includeEP = true;
                    }
                    break;
                default:
                    break;
            }

            FillListViewItems(includeInterface, includeAC, includeMS, includeEP, includeASEP);

            if (treeViewMidi.SelectedNode.Level > TREE_LEVEL_INTERFACEGEN)
            {
                listViewMidiDescList.SelectedItems.Clear();
                ShowEditItem();
            }
            else if (listViewMidiDescList.SelectedIndices.Count == 0)
            {
                ShowEditItem();
            }

            if (listViewMidiDescList.SelectedIndices.Count > 0)
                buttonApply.Enabled = true;
            else
            {
                buttonApply.Enabled = false;
            }
        }

        private void FillListViewItems(bool includeInterface, bool includeAC, bool includeMS, bool includeEP,
                                       bool includeASEP)
        {
            listViewMidiDescList.BeginUpdate();
            listViewMidiDescList.Items.Clear();
            for (int i = 0; i < m_ItemsList.Count; i++)
            {
                ListViewItem item = new ListViewItem(m_ItemsList[i].ToString()); 
                item.ImageIndex = 5;
                item.Tag = i;

                if (m_ItemsList[i] is CyInterfaceDescriptor && includeInterface)
                {
                    item.Group = listViewMidiDescList.Groups[LISTVIEW_GROUP_INTERACE];
                    item.Text = ((CyInterfaceDescriptor)m_ItemsList[i]).bInterfaceSubClass ==
                                (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL
                                    ? "Audio Control"
                                    : "MIDI Streaming";
                    listViewMidiDescList.Items.Add(item);
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
                    item.Group = listViewMidiDescList.Groups[LISTVIEW_GROUP_AC];
                    listViewMidiDescList.Items.Add(item);
                }
                else if (((m_ItemsList[i] is CyACHeaderDescriptor_v2) ||
                         (m_ItemsList[i] is CyACInputTerminalDescriptor_v2) ||
                         (m_ItemsList[i] is CyACOutputTerminalDescriptor_v2) ||
                         (m_ItemsList[i] is CyACClockSourceDescriptor_v2) ||
                         (m_ItemsList[i] is CyACClockSelectorDescriptor_v2) ||
                         (m_ItemsList[i] is CyACClockMultiplierDescriptor_v2) ||
                         (m_ItemsList[i] is CyACMixerUnitDescriptor_v2) ||
                         (m_ItemsList[i] is CyACSelectorUnitDescriptor_v2) ||
                         (m_ItemsList[i] is CyACFeatureUnitDescriptor_v2) ||
                         (m_ItemsList[i] is CyACProcessingUnitDescriptor_v2) ||
                         (m_ItemsList[i] is CyACEffectUnitDescriptor_v2) ||
                         (m_ItemsList[i] is CyACSamplingRateConverterDescriptor_v2) ||
                         (m_ItemsList[i] is CyACExtensionDescriptor_v2)) && includeAC)
                {
                    item.Group = listViewMidiDescList.Groups[LISTVIEW_GROUP_AC2];
                    listViewMidiDescList.Items.Add(item);
                }
                else if (((m_ItemsList[i] is CyMSHeaderDescriptor) ||
                         (m_ItemsList[i] is CyMSInJackDescriptor) ||
                         (m_ItemsList[i] is CyMSOutJackDescriptor) ||
                         (m_ItemsList[i] is CyMSElementDescriptor)) && includeMS)
                {
                    item.Group = listViewMidiDescList.Groups[LISTVIEW_GROUP_MS];
                    listViewMidiDescList.Items.Add(item);
                }
                else if ((m_ItemsList[i] is CyAudioEndpointDescriptor) && includeEP) 
                {
                    item.Group = listViewMidiDescList.Groups[LISTVIEW_GROUP_ENDPOINT];
                    listViewMidiDescList.Items.Add(item);
                }
                else if ((m_ItemsList[i] is CyMSEndpointDescriptor) && includeASEP)
                {
                    item.Group = listViewMidiDescList.Groups[LISTVIEW_GROUP_ENDPOINT];
                    listViewMidiDescList.Items.Add(item);
                }
            }
            listViewMidiDescList.EndUpdate();
        }

        private void RemoveMidiTreeItem()
        {
            if (treeViewMidi.SelectedNode == null) return;

            TreeNode treeNode = treeViewMidi.SelectedNode;
            TreeNode prevNode = treeNode.PrevNode;
            if (prevNode == null)
                prevNode = treeNode.Parent;

            if (treeNode.Level == TREE_LEVEL_ROOT) return;

            TreeNode parent = treeNode.Parent;
            if (parent != null)
                m_parameters.m_midiTree.RemoveNode(treeNode.Name, parent.Name);
          
            m_parameters.ParamMidiTreeChanged = true; 
            RefreshMidiTree();
            //Select previous node
            if (prevNode != null)
                treeViewMidi.SelectedNode = treeViewMidi.Nodes.Find(prevNode.Name, true)[0];
        }

        #endregion Private functions

        #region Events

        private void CyMidiDescriptorPage_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                RefreshMidiTree();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (listViewMidiDescList.SelectedIndices.Count == 0) return;

            // Add to the tree
            AddMidiNode((CyUSBDescriptor)propertyGridMidi.SelectedObject);
          
            m_parameters.ParamMidiTreeChanged = true;
        }

        private void treeViewMidi_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewSelection();
        }

        private void treeViewMidi_Enter(object sender, EventArgs e)
        {
            TreeViewSelection();
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            RemoveMidiTreeItem();
        }

        private void treeViewMidi_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Allow only editing of the report titles and comments.
            if (!(((e.Node.Level == 1) ||
                  ((e.Node.Level > 1) && (CyUSBFSParameters.StringIsComment(e.Node.Text))))))
            {
                e.CancelEdit = true;
            }
        }

        private void treeViewMidi_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (treeViewMidi.SelectedNode == null)
                return;
            CyDescriptorNode node = m_parameters.m_midiTree.GetNodeByKey(treeViewMidi.SelectedNode.Name);
            if (e.Label == null)
            {
                treeViewMidi.SelectedNode.Text = ((CyInterfaceGeneralDescriptor)node.m_value).DisplayName;
            }
            else if ((node.m_value is CyInterfaceGeneralDescriptor) && 
                     (((CyInterfaceGeneralDescriptor)node.m_value).DisplayName != e.Label))
            {
                ((CyInterfaceGeneralDescriptor)node.m_value).DisplayName = e.Label;
                m_parameters.ParamMidiTreeChanged = true;
            }
        }

        private void treeViewMidi_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if ((e.Item is TreeNode) && (((TreeNode)e.Item).Level > TREE_LEVEL_ROOT))
                DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewMidi_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private TreeNode m_oldNode;

        private void treeViewMidi_DragOver(object sender, DragEventArgs e)
        {
            Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
            TreeNode aNode = ((TreeView) sender).GetNodeAt(pt);
            if ((aNode != null) && (e.Data.GetDataPresent(CyUSBFSParameters.TYPE_TREE_NODE, false)))
            {
                // If the node is a folder, change the color of the background to dark blue to simulate selection
                // Be sure to return the previous node to its original color by copying from a blank node
                if ((m_oldNode != null) && (m_oldNode != aNode))
                {
                    m_oldNode.BackColor = treeViewMidi.Nodes[0].BackColor;
                    m_oldNode.ForeColor = treeViewMidi.Nodes[0].ForeColor;
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
                m_oldNode.BackColor = treeViewMidi.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewMidi.Nodes[0].ForeColor;
            }
        }

        private void treeViewMidi_DragLeave(object sender, EventArgs e)
        {
            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewMidi.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewMidi.Nodes[0].ForeColor;
            }
        }

        private void treeViewMidi_DragDrop(object sender, DragEventArgs e)
        {
            if (m_oldNode != null)
            {
                m_oldNode.BackColor = treeViewMidi.Nodes[0].BackColor;
                m_oldNode.ForeColor = treeViewMidi.Nodes[0].ForeColor;
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
                    CyDescriptorNode sourceDesc = m_parameters.m_midiTree.GetNodeByKey(newNode.Name);
                    CyDescriptorNode destinationDesc = m_parameters.m_midiTree.GetNodeByKey(destinationNode.Name);
                    CyDescriptorNode sourceParentDesc = m_parameters.m_midiTree.GetNodeByKey(sourceParent.Name);
                    CyDescriptorNode destParentDesc = m_parameters.m_midiTree.GetNodeByKey(destParent.Name);

                    if ((sourceDesc != null) && (destinationDesc != null) && 
                        (sourceParentDesc != null) && (destParent != null) &&
                        (destParent.Level == sourceParent.Level))
                    {
                        sourceParentDesc.m_nodes.Remove(sourceDesc);
                        if (destinationNode == destParent)
                            destParentDesc.m_nodes.Add(sourceDesc);
                        else
                            destParentDesc.m_nodes.Insert(destParentDesc.m_nodes.IndexOf(destinationDesc), sourceDesc);

                        m_parameters.ParamMidiTreeChanged = true;
                        RefreshMidiTree();
                        treeViewMidi.SelectedNode = treeViewMidi.Nodes.Find(newNode.Name, true)[0];
                    }
                }
            }
        }

        private void treeViewMidi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveMidiTreeItem();
            }
        }

        private void listViewMidiDescList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewMidiDescList.SelectedIndices.Count > 0)
            {
                propertyGridMidi.SelectedObject =
                    m_ItemsList[Convert.ToInt32(listViewMidiDescList.SelectedItems[0].Tag)].Clone();
                buttonApply.Enabled = true;
                splitContainerDetails.Panel2.BackColor = Color.LightSteelBlue;
                labelItemProperties.Text = string.Format("Item Value ({0})", propertyGridMidi.SelectedObject.
                                                                             ToString().Trim());
            }
        }

        private void listViewMidiDescList_DoubleClick(object sender, EventArgs e)
        {
            buttonApply_Click(sender, e);
        }


        private void propertyGridMidi_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Update String Descriptor information if string was changed
            if (e.ChangedItem.Label.StartsWith("i"))
            {
                uint strIndex = m_parameters.SaveStringDescriptor(e.ChangedItem.Value.ToString());
                try
                {
                    Type objType = propertyGridMidi.SelectedObject.GetType();
                    string fName = e.ChangedItem.Label.Remove(0, 1).Insert(0, "iw");
                    FieldInfo fi = objType.GetField(fName);
                    if (fi != null)
                    {
                        fi.SetValue(propertyGridMidi.SelectedObject, strIndex);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.ToString());
                }
            }
            else if (e.ChangedItem.Label == "bAlternateSetting")
            {
                TreeNode node = treeViewMidi.SelectedNode;
                if (node != null)
                    node.Text = CyDescriptorNode.GetDescriptorLabel(m_parameters.m_midiTree.GetNodeByKey(node.Name));
            }
            propertyGridMidi.Refresh();
            m_parameters.ParamMidiTreeChanged = true;
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            CyDescriptorNode descNode = GetParentDescriptorAtLevel(TREE_LEVEL_INTERFACEGEN);
            if (descNode != null)
            {
                m_parameters.SaveDescriptorList(descNode, CyUSBDescriptorType.MIDI);
            }
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            if (m_parameters.ImportDescriptorList(CyUSBDescriptorType.MIDI))
            {
                m_parameters.ParamStringTreeChanged = true;
                m_parameters.ParamMidiTreeChanged = true;
            }
            RefreshMidiTree();
        }

        private void checkBoxEnableMIDIApi_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.EnableMIDIApi = checkBoxEnableMIDIApi.Checked;
        }

        private void checkBoxMode_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.Mode = checkBoxMode.Checked;
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
