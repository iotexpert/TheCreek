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
using System.Diagnostics;

using CyDesigner.Common.Base;
using CyDesigner.Common.Base.Controls;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using Cypress.Comps.PinsAndPorts.Common_v1_20;

using ActiproSoftware.UIStudio.TabStrip;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_20
{
    /// <summary>
    /// Contains a SplitContainer with a tree on the left and params on the right.
    /// </summary>
    public partial class CyPinsControl : UserControl, ICyParamEditingControl
    {
        ICyInstEdit_v1 m_edit;
        readonly int PinImageIndex;
        readonly int SIOPinImageIndex;

        CyNumPinsToolStripTextBox m_numPinsTextBox;
        ToolStripButton m_moveUpButton;
        ToolStripButton m_moveDownButton;
        ToolStripButton m_deleteButton;
        ToolStripButton m_renameButton;
        ToolStripButton m_groupSIOButton;
        ToolStripButton m_ungroupSIOButton;

        const int AllPinsTag = -2;
        const int SIOPairTag = -3;
        CyPerPinDataControl m_perPinDataControl;
        bool m_rebuildingTree = false;

        public CyPinsControl(ICyInstEdit_v1 instEdit)
        {
            InitializeComponent();

            m_edit = instEdit;
            this.Dock = DockStyle.Fill;

            m_splitContainer.FixedPanel = FixedPanel.Panel1;

            m_treeView.CheckBoxes = CyTreeView.CheckBoxDisplay.SHOW_NONE;
            m_treeView.ShowRootLines = false;
            m_treeView.HideSelection = false;
            m_treeView.ShowNodeToolTips = true;
            m_treeView.FocusWhenSelectedNodeChanged = false;
            m_treeView.ImageList = new CyTableViewBase.CyImageList();
            PinImageIndex = m_treeView.ImageList.AddImage(Resource1.PinImage);
            SIOPinImageIndex = m_treeView.ImageList.AddImage(Resource1.SIOPinImage);

            m_toolStrip.CanOverflow = true;
            m_toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            m_toolStrip.RenderMode = ToolStripRenderMode.System;
            m_toolStrip.ShowItemToolTips = true;

            ToolStripLabel numPinsLabel = new ToolStripLabel("Number of Pins:");
            m_toolStrip.Items.Add(numPinsLabel);

            m_numPinsTextBox = new CyNumPinsToolStripTextBox(m_edit);
            m_toolStrip.Items.Add(m_numPinsTextBox);

            m_toolStrip.Items.Add(new ToolStripSeparator());

            m_deleteButton = new ToolStripButton("Delete Selected Pin/s (Delete)", Resource1.DeleteImage,
                DeletePinClicked);
            m_deleteButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            m_toolStrip.Items.Add(m_deleteButton);

            m_renameButton = new ToolStripButton("Change Selected Pin's Alias... (F2)", Resource1.PinAliasImage,
                RenamePinClicked);
            m_renameButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            m_toolStrip.Items.Add(m_renameButton);

            m_moveUpButton = new ToolStripButton("Move Selected Pin/s Up", Resource1.MoveUpImage, MovePinUpClicked);
            m_moveUpButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            m_toolStrip.Items.Add(m_moveUpButton);

            m_moveDownButton = new ToolStripButton("Move Selected Pin/s Down", Resource1.MoveDownImage,
                MovePinDownClicked);
            m_moveDownButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            m_toolStrip.Items.Add(m_moveDownButton);

            m_toolStrip.Items.Add(new ToolStripSeparator());

            m_groupSIOButton = new ToolStripButton("Pair Selected SIOs", Resource1.PairSIOsImage, PairSIOsClicked);
            m_groupSIOButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            m_toolStrip.Items.Add(m_groupSIOButton);

            m_ungroupSIOButton = new ToolStripButton("Un-pair Selected SIOs", Resource1.UnpairSIOsImage,
                UnpairSIOsClicked);
            m_ungroupSIOButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            m_toolStrip.Items.Add(m_ungroupSIOButton);

            m_toolStrip.Items.Add(new ToolStripSeparator());

            m_perPinDataControl = new CyPerPinDataControl(this);
            m_splitContainer.Panel2.Controls.Add(m_perPinDataControl);

            UpdateFromExprs();
            UpdateToolStripItemsEnabled();
            UpdatePerPinDataDisplayed();

            m_treeView.BeforeCollapse += m_treeView_BeforeCollapse;
            m_treeView.AfterMultiSelect += m_treeView_AfterMultiSelect;
            m_treeView.NodeMouseDoubleClick += m_treeView_NodeMouseDoubleClick;
            m_numPinsTextBox.Validated += m_numPinsTextBox_Validated;

        }

        private void PerformDispose()
        {
            m_treeView.BeforeCollapse -= m_treeView_BeforeCollapse;
            m_treeView.AfterMultiSelect -= m_treeView_AfterMultiSelect;
            m_treeView.NodeMouseDoubleClick -= m_treeView_NodeMouseDoubleClick;
            m_numPinsTextBox.Validated -= m_numPinsTextBox_Validated;
        }

        void m_treeView_AfterMultiSelect(object sender, CyMultiSelectTreeViewEventArgs e)
        {
            //The event is raised twice as part of setting the selection. Once to clear the selection and again to 
            //set the selected items. The first call needs to be ignored.
            if (m_rebuildingTree == false && m_treeView.SelectedNodesCount > 0)
            {
                UpdateToolStripItemsEnabled();
                UpdatePerPinDataDisplayed();
            }
        }

        void m_treeView_BeforeCollapse(object sender, CyTreeViewCancelEventArgs e)
        {
            //Don't allow the root node to be collapsed
            if (e.Action == CyTreeViewAction.Collapse && e.Node.FullPath.Equals(Resource1.AllPins))
            {
                e.Cancel = true;
            }
        }

        public void UpdateFromExprs()
        {
            m_rebuildingTree = true;
            m_treeView.SuspendLayout();
            m_treeView.BeginUpdate();

            IEnumerable<int> prevSelectedIndexes = GetPinIndexes(m_treeView.SelectedNodes);
            m_treeView.Nodes.Clear();

            //Need to create [All Pins] node, SIO Pair nodes, SIO nodes, and pin nodes.
            CyTreeNode rootNode = new CyTreeNode(Resource1.AllPins);
            rootNode.Tag = AllPinsTag;

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit, CyParamInfo.Local_ParamName_SIOInfo,
                out data);

            if (err.IsNotOk)
            {
                //Can't do anything in this case except assume everything is not SIO.
                data = new CyStringArrayParamData.CyBitVectorParamData(CyPortConstants.SIOInfoValue_NOT_SIO,
                    CyPortConstants.SIOInfoValue_NOT_SIO);
            }

            byte width;
            err = CyParamInfo.GetNumPinsValue(m_edit, out width);
            if (err.IsNotOk)
            {
                width = 0;
            }

            CyCompDevParam widthParam = m_edit.GetCommittedParam(CyParamInfo.Formal_ParamName_NumPins);
            m_numPinsTextBox.Text = widthParam.Expr;
            if (widthParam.ErrorCount == 0)
            {
                m_numPinsTextBox.BackColor = SystemColors.Window;
                m_numPinsTextBox.ToolTipText = string.Empty;
            }
            else
            {
                m_numPinsTextBox.BackColor = Color.Red;
                m_numPinsTextBox.ToolTipText = widthParam.ErrorMsgs;
            }

            Dictionary<int, string> errors = GetPinsErrorText(0, width - 1);

            CyTreeNode sioGroupNode = null;
            for (int i = 0; i < width; i++)
            {
                string sioInfo = data.GetValue(i);
                CyTreeNode leafNode = new CyTreeNode(CyPortCustomizer.GetPinPoundDefineName(m_edit, i));
                leafNode.Tag = i; //the tag stores the pin index
                leafNode.ErrorText = (errors.ContainsKey(i)) ? errors[i] : string.Empty;
                switch (sioInfo)
                {
                    case CyPortConstants.SIOInfoValue_NOT_SIO:
                        leafNode.ImageIndex = PinImageIndex;
                        rootNode.Nodes.Add(leafNode);
                        sioGroupNode = null;
                        break;

                    case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                        leafNode.ImageIndex = SIOPinImageIndex;
                        rootNode.Nodes.Add(leafNode);
                        sioGroupNode = null;
                        break;

                    case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                        Debug.Assert(sioGroupNode != null);
                        leafNode.ImageIndex = SIOPinImageIndex;
                        sioGroupNode.Nodes.Add(leafNode);
                        break;

                    case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                        leafNode.ImageIndex = SIOPinImageIndex;
                        sioGroupNode = new CyTreeNode(Resource1.SIOPair);
                        sioGroupNode.Tag = SIOPairTag;
                        sioGroupNode.Nodes.Add(leafNode);
                        rootNode.Nodes.Add(sioGroupNode);
                        break;

                    default:
                        break;
                }
            }

            m_treeView.Nodes.Add(rootNode);

            m_rebuildingTree = false;

            SelectPins(prevSelectedIndexes);
            if (m_treeView.SelectedNodesCount == 0)
            {
                m_treeView.SelectedNodes = new CyTreeNode[] { rootNode };
            }

            m_treeView.EndUpdate();
            m_treeView.ResumeLayout();
        }

        bool IsHandledPerPinParam(CyCompDevParam param)
        {
            if (param.TabName == CyCustomizer.ConfigureTabName &&
                CyStringArrayParamData.IsFormalStringArrayParam(param.Name))
            {
                Debug.Assert(param.IsFormal);
                Debug.Assert(param.IsVisible);
                Debug.Assert(param.IsReadOnly == false);
                Debug.Assert(param.IsHardware == false);
                return true;
            }
            return false;
        }

        private Dictionary<int, string> GetPinsErrorText(int startIndex, int endIndex)
        {
            //Key: ParamName, Value: Err msg associcate with this pin for the associated param.
            Dictionary<string, string> errs = new Dictionary<string, string>();

            //Key: pin index, Value: error message
            Dictionary<int, string> errors = new Dictionary<int, string>();
            foreach (string paramName in m_edit.GetParamNames())
            {
                CyCompDevParam param = m_edit.GetCommittedParam(paramName);

                //Get each per pin data and check it is has an error for the pin specified.
                if (IsHandledPerPinParam(param))
                {
                    CyStringArrayParamData data;
                    CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit, paramName, out data);
                    if (err.IsNotOk)
                    {
                        for (int i = startIndex; i <= endIndex; i++)
                        {
                            string pinErrMsg = CyStringArrayParamData.ParseOutErrMsgForIndex(err, i);
                            if (string.IsNullOrEmpty(pinErrMsg) == false)
                            {
                                if (errors.ContainsKey(i) == false)
                                {
                                    errors.Add(i, string.Empty);
                                }
                                errors[i] +=
                                    string.Format("{0} Error: {1}", paramName, pinErrMsg.Trim() + Environment.NewLine);
                            }
                        }
                    }
                }
            }

            return errors;
        }

        int _getPinIndex(CyTreeNode node)
        {
            if (node != null)
            {
                Debug.Assert(node.Tag is int);
                return (int)node.Tag;
            }
            return -1;
        }

        List<int> GetPinIndexes(IEnumerable<CyTreeNode> nodes)
        {
            List<int> indexes = new List<int>();

            foreach (CyTreeNode node in nodes)
            {
                if (node != null)
                {
                    Debug.Assert(node.Tag is int);
                    int tagInt = (int)node.Tag;

                    if (tagInt == AllPinsTag)
                    {
                        int width = GetNumLeafNodes();
                        for (int i = 0; i < width; i++)
                        {
                            if (indexes.Contains(i) == false)
                            {
                                indexes.Add(i);
                            }
                        }
                        break;
                    }
                    else if (tagInt == SIOPairTag)
                    {
                        foreach (CyTreeNode childNode in node.Nodes)
                        {
                            int childIndex = _getPinIndex(childNode);
                            if (indexes.Contains(childIndex) == false)
                            {
                                indexes.Add(childIndex);
                            }
                        }
                    }
                    else
                    {
                        if (indexes.Contains(tagInt) == false)
                        {
                            indexes.Add(tagInt);
                        }
                    }
                }
            }

            return indexes;
        }

        private int GetNumLeafNodes()
        {
            CyTreeView.CyFindDelegate isLeafNode = delegate(CyTreeNode node) { return node.Nodes.Count == 0; };
            IEnumerable<CyTreeNode> leafNodes = m_treeView.Find(isLeafNode);
            int cnt = 0;
            foreach (CyTreeNode node in leafNodes)
            {
                cnt++;
            }
            return cnt;
        }

        private void SelectPin(int index)
        {
            if (index >= 0)
            {
                foreach (CyTreeNode node in m_treeView.GetAllNodesInTree())
                {
                    int nodesPinIndex = _getPinIndex(node);
                    if (nodesPinIndex == index)
                    {
                        m_treeView.SelectedNodes = new CyTreeNode[] { node };
                        return;
                    }
                }
            }

            //Select the root node if the selection failed
            Debug.Assert(m_treeView.Nodes.Count == 1);
            m_treeView.SelectedNodes = new CyTreeNode[] { m_treeView.Nodes[0] };
        }

        private void SelectPins(IEnumerable<int> indexes)
        {
            List<CyTreeNode> nodes = new List<CyTreeNode>();
            foreach (int index in indexes)
            {
                if (index >= 0)
                {
                    foreach (CyTreeNode node in m_treeView.GetAllNodesInTree())
                    {
                        int nodesPinIndex = _getPinIndex(node);
                        if (nodesPinIndex == index)
                        {
                            nodes.Add(node);
                        }
                    }
                }
            }

            if (nodes.Count == 0)
            {
                //Select the root node if the selection failed
                Debug.Assert(m_treeView.Nodes.Count == 1);
                m_treeView.SelectedNodes = new CyTreeNode[] { m_treeView.Nodes[0] };
            }
            else
            {
                m_treeView.SelectedNodes = nodes;
            }
        }

        CyTreeNode GetAssociatedNode(int index)
        {
            foreach (CyTreeNode node in m_treeView.GetAllNodesInTree())
            {
                int nodesPinIndex = _getPinIndex(node);
                if (nodesPinIndex == index)
                {
                   return node;
                }
            }

            return null;
        }

        #region Per Pin Data

        private void UpdatePerPinDataDisplayed()
        {
            CyCustErr err;
            string value;

            List<int> selectedPinIndexes = GetPinIndexes(m_treeView.SelectedNodes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_DriveMode, out value);
            m_perPinDataControl.DriveMode = value;
            m_perPinDataControl.DriveModeErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_InitialDriveStates, out value);
            m_perPinDataControl.InitialDriveState = value;
            m_perPinDataControl.InitialDriveStateErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_IOVoltages, out value);
            m_perPinDataControl.SupplyVoltage = value;
            m_perPinDataControl.SupplyVoltageErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_HotSwaps, out value);
            m_perPinDataControl.HotSwap = value;
            m_perPinDataControl.HotSwapErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_InputBuffersEnabled, out value);
            m_perPinDataControl.InputBufferEnabled = value;
            m_perPinDataControl.InputBufferEnabledErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_InputsSynchronized, out value);
            m_perPinDataControl.InputSynchronized = value;
            m_perPinDataControl.InputSynchronizedErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_InterruptMode, out value);
            m_perPinDataControl.InterruptMode = value;
            m_perPinDataControl.InterruptModeErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_ThresholdLevels, out value);
            m_perPinDataControl.InputThresholdLevel = value;
            m_perPinDataControl.InputThresholdLevelErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_SlewRate, out value);
            m_perPinDataControl.SlewRate = value;
            m_perPinDataControl.SlewRateErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_OutputDriveLevels, out value);
            m_perPinDataControl.OutputDriveLevel = value;
            m_perPinDataControl.OutputDriveLevelErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_DriveCurrents, out value);
            m_perPinDataControl.DriveCurrent = value;
            m_perPinDataControl.DriveCurrentErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_OutputsSynchronized, out value);
            m_perPinDataControl.OutputSynchronized = value;
            m_perPinDataControl.OutputSynchronizedErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            CheckState analogState, inputState, outputState, oeState, bidirState;
            err = GetPinTypeStatesForSelectedNodes(out analogState, out inputState, out outputState,
                out oeState, out bidirState);
            m_perPinDataControl.AnalogState = analogState;
            m_perPinDataControl.InputState = inputState;
            m_perPinDataControl.OutputState = outputState;
            m_perPinDataControl.OEState = oeState;
            m_perPinDataControl.BidirState = bidirState;
            m_perPinDataControl.PinTypeErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_DisplayInputHWConnections, out value);
            m_perPinDataControl.DisplayInputHWConnections = value;
            m_perPinDataControl.DisplayInputHWConnectionsErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            err = GetPerPinParamValuesForSelectedNodes(CyParamInfo.Formal_ParamName_DisplayOutputHWConnections, out value);
            m_perPinDataControl.DisplayOutputHWConnections = value;
            m_perPinDataControl.DisplayOutputHWConnectionsErrorText = (err.IsOk) ?
                string.Empty : CyStringArrayParamData.ParseOutRelatedMsgs(err, selectedPinIndexes);

            m_perPinDataControl.InputTabEnabled = _inputUsed(selectedPinIndexes);
            m_perPinDataControl.OutputTabEnabled = _outputUsed(selectedPinIndexes);
        }

        private bool _outputUsed(List<int> selectedPinIndexes)
        {
            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit, CyParamInfo.Formal_ParamName_PinTypes,
                out data);

            foreach (int i in selectedPinIndexes)
            {
                string pinType = data.GetValue(i);
                bool output = CyPortConstants.IsOutput(pinType) || CyPortConstants.IsBidir(pinType);
                if (output) return true;
            }
            return false;
        }

        private bool _inputUsed(List<int> selectedPinIndexes)
        {
            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit, CyParamInfo.Formal_ParamName_PinTypes,
                out data);

            foreach (int i in selectedPinIndexes)
            {
                string pinType = data.GetValue(i);
                bool input = CyPortConstants.IsInput(pinType) || CyPortConstants.IsBidir(pinType);
                if (input) return true;
            }
            return false;
        }

        /// <summary>
        /// returns null for indeterminate.
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public CyCustErr GetPerPinParamValuesForSelectedNodes(string paramName, out string value)
        {
            value = null; //null means indeterminate

            CyCompDevParam param = m_edit.GetCommittedParam(paramName);

            Debug.Assert(IsHandledPerPinParam(param));
            if (IsHandledPerPinParam(param))
            {
                string dataString;
                CyCustErr err = CyPortCustomizer.GetParamValue<string>(m_edit, paramName, out dataString);

                if (dataString != null) //as long as it is not a syntax error
                {
                    CyStringArrayParamData data;
                    err = CyStringArrayParamData.CreateStringArrayData(m_edit, paramName, out data);

                    IEnumerable<CyTreeNode> selNodes = m_treeView.SelectedNodes;
                    List<int> selectedPinIndexes = GetPinIndexes(selNodes);
                    foreach (int selectedPinIndex in selectedPinIndexes)
                    {
                        Debug.Assert(selectedPinIndex >= 0);
                        if (selectedPinIndex >= 0)
                        {
                            string pinValue;
                            pinValue = data.GetValue(selectedPinIndex);

                            if (value == null)
                            {
                                value = pinValue;
                            }
                            else if (value.Equals(pinValue) == false)
                            {
                                value = null;
                                break;
                            }
                        }
                    }
                    if (err.IsNotOk) return err;
                }
            }
            return CyCustErr.Ok;
        }

        public void SetPerPinParamValueForSelectedNodes(string paramName, string value)
        {
            //If the user entered an indeterminate state there is nothing to do.
            if (value != null)
            {
                CyCompDevParam param = m_edit.GetCommittedParam(paramName);

                Debug.Assert(IsHandledPerPinParam(param));
                if (IsHandledPerPinParam(param))
                {
                    int width = GetNumLeafNodes();
                    CyStringArrayParamData data;
                    CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit, paramName, out data);

                    IEnumerable<CyTreeNode> selNodes = m_treeView.SelectedNodes;
                    if (data != null)
                    {
                        List<int> selectedPinIndexes = GetPinIndexes(selNodes);
                        foreach (int selectedPinIndex in selectedPinIndexes)
                        {
                            Debug.Assert(selectedPinIndex >= 0);

                            //We want future added pins to be the same as the last pin, not what they were before the
                            //pin was added.
                            if (selectedPinIndex == width - 1)
                            {
                                data.SetValue(selectedPinIndex, value);
                            }
                            else if (selectedPinIndex >= 0)
                            {
                                data.SafeSetValue(selectedPinIndex, value);
                            }
                        }

                        m_edit.SetParamExpr(paramName, data.ToString());
                        m_edit.CommitParamExprs();
                    }

                    //The tree needs to be rebuilt because SIO settings/Pairings could have changed.
                    m_treeView.AfterMultiSelect -= m_treeView_AfterMultiSelect;
                    UpdateFromExprs();
                    m_treeView.AfterMultiSelect += m_treeView_AfterMultiSelect;
                    m_treeView.SelectedNodes = selNodes;
                }
            }
        }

        internal void PinTypeChangedByUser(CheckState analogState, CheckState inputState, CheckState outputState,
            CheckState oeState, CheckState bidirState)
        {
            int width = GetNumLeafNodes();
            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit,
                CyParamInfo.Formal_ParamName_PinTypes, out data);

            IEnumerable<CyTreeNode> selNodes = m_treeView.SelectedNodes;
            if (data != null)
            {
                List<int> selectedPinIndexes = GetPinIndexes(selNodes);
                foreach (int selectedPinIndex in selectedPinIndexes)
                {
                    Debug.Assert(selectedPinIndex >= 0);
                    string oldPinType = data.GetValue(selectedPinIndex);
                    string value = CreateNewPinType(oldPinType, analogState, inputState, outputState, oeState,
                        bidirState);

                    //If the new pin type is not a valid option (null is returned) use the old type for that pin.
                    if (value == null)
                    {
                        MessageBox.Show("This operation would result in an invalid Pin Type.",
                            "Operation Not Allowed",
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        value = oldPinType;
                    }

                    //We want future added pins to be the same as the last pin, not what they were before the
                    //pin was added.
                    if (selectedPinIndex == width - 1)
                    {
                        data.SetValue(selectedPinIndex, value);
                    }
                    else if (selectedPinIndex >= 0)
                    {
                        data.SafeSetValue(selectedPinIndex, value);
                    }
                }

                m_edit.SetParamExpr(CyParamInfo.Formal_ParamName_PinTypes, data.ToString());
                m_edit.CommitParamExprs();
            }

            //The tree needs to be rebuilt because SIO settings/Pairings could have changed.
            m_treeView.AfterMultiSelect -= m_treeView_AfterMultiSelect;
            UpdateFromExprs();
            m_treeView.AfterMultiSelect += m_treeView_AfterMultiSelect;
            m_treeView.SelectedNodes = selNodes;
        }

        public CyCustErr GetPinTypeStatesForSelectedNodes(out CheckState analogState, out CheckState inputState,
            out CheckState outputState, out CheckState oeState, out CheckState bidirState)
        {
            bool analogInited = false;
            bool inputInited = false;
            bool outputInited = false;
            bool oeInited = false;
            bool bidirInited = false;
            analogState = CheckState.Indeterminate;
            inputState = CheckState.Indeterminate;
            outputState = CheckState.Indeterminate;
            oeState = CheckState.Indeterminate;
            bidirState = CheckState.Indeterminate;

            string dataString;
            CyCustErr err = CyPortCustomizer.GetParamValue<string>(m_edit, CyParamInfo.Formal_ParamName_PinTypes,
                out dataString);

            if (dataString != null) //as long as it is not a syntax error
            {
                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateStringArrayData(m_edit, CyParamInfo.Formal_ParamName_PinTypes,
                    out data);

                IEnumerable<CyTreeNode> selNodes = m_treeView.SelectedNodes;
                List<int> selectedPinIndexes = GetPinIndexes(selNodes);
                foreach (int selectedPinIndex in selectedPinIndexes)
                {
                    Debug.Assert(selectedPinIndex >= 0);
                    if (selectedPinIndex >= 0)
                    {
                        string pinType = data.GetValue(selectedPinIndex);

                        analogState = _updateState(CyPortConstants.IsAnalog(pinType), analogInited, analogState);
                        inputState = _updateState(CyPortConstants.IsInput(pinType), inputInited, inputState);
                        outputState = _updateState(CyPortConstants.IsOutput(pinType), outputInited, outputState);
                        oeState = _updateState(CyPortConstants.IsOE(pinType), oeInited, oeState);
                        bidirState = _updateState(CyPortConstants.IsBidir(pinType), bidirInited, bidirState);
                        bidirInited = oeInited = outputInited = inputInited = analogInited = true;
                    }
                }
            }
            return err;
        }

        CheckState _updateState(bool value, bool isInitialized, CheckState currentState)
        {
            CheckState newState = (value) ? CheckState.Checked : CheckState.Unchecked;
            if (isInitialized == false) return newState;
            if (newState != currentState) return CheckState.Indeterminate;
            return currentState;
        }

        private string CreateNewPinType(string oldPinType, CheckState analogState, CheckState inputState,
            CheckState outputState, CheckState oeState, CheckState bidirState)
        {
            bool analog = (analogState == CheckState.Indeterminate) ?
                CyPortConstants.IsAnalog(oldPinType) : analogState == CheckState.Checked;

            bool input = (inputState == CheckState.Indeterminate) ?
                CyPortConstants.IsInput(oldPinType) : inputState == CheckState.Checked;

            bool output = (outputState == CheckState.Indeterminate) ?
                CyPortConstants.IsOutput(oldPinType) : outputState == CheckState.Checked;

            bool oe = (oeState == CheckState.Indeterminate) ?
                CyPortConstants.IsOE(oldPinType) : oeState == CheckState.Checked;

            bool bidir = (bidirState == CheckState.Indeterminate) ?
               CyPortConstants.IsBidir(oldPinType) : bidirState == CheckState.Checked;

            if (bidir)
            {
                if (analog)
                {
                    return CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG;
                }
                return CyPortConstants.PinTypesValue_BIDIRECTIONAL;
            }
            if (input)
            {
                if (output)
                {
                    if (oe)
                    {
                        if (analog)
                        {
                            return CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG;
                        }
                        return CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE;
                    }
                    if (analog)
                    {
                        return CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG;
                    }
                    return CyPortConstants.PinTypesValue_DIGOUT_DIGIN;
                }
                if (analog)
                {
                    return CyPortConstants.PinTypesValue_DIGIN_ANALOG;
                }
                return CyPortConstants.PinTypesValue_DIGIN;
            }
            if (output)
            {
                if (oe)
                {
                    if (analog)
                    {
                        return CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG;
                    }
                    return CyPortConstants.PinTypesValue_DIGOUT_OE;
                }
                if (analog)
                {
                    return CyPortConstants.PinTypesValue_DIGOUT_ANALOG;
                }
                return CyPortConstants.PinTypesValue_DIGOUT;
            }
            if (analog)
            {
                return CyPortConstants.PinTypesValue_ANALOG;
            }

            return null;
        }

        internal void PerPinParamDataChangedByUser(CyPerPinDataEventArgs e)
        {
            SetPerPinParamValueForSelectedNodes(e.ParamName, e.Value);
        }

        #endregion

        #region ToolStrip Handlers

        private void UpdateToolStripItemsEnabled()
        {
            int numLeafNodes = GetNumLeafNodes();

            m_deleteButton.Enabled = false;
            m_renameButton.Enabled = false;
            m_moveUpButton.Enabled = false;
            m_moveDownButton.Enabled = false;
            m_groupSIOButton.Enabled = false;
            m_ungroupSIOButton.Enabled = false;

            m_groupSIOButton.ToolTipText =
              "Pair SIOs cannot be performed. SIOs can only be paired if they are adjacent to each other and not already in a pair.";
            m_deleteButton.ToolTipText = string.Empty;

            List<int> selectedPinIndexes = GetPinIndexes(m_treeView.SelectedNodes);
            if (selectedPinIndexes.Count > 0)
            {
                if (selectedPinIndexes.Count == 1)
                {
                    m_renameButton.Enabled = true;
                }
                else if (selectedPinIndexes.Count == 2)
                {
                    CyStringArrayParamData data;
                    CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit,
                        CyParamInfo.Local_ParamName_SIOInfo, out data);
                    Debug.Assert(err.IsOk, err.Message);

                    int index1 = Math.Min(selectedPinIndexes[0], selectedPinIndexes[1]);
                    int index2 = Math.Max(selectedPinIndexes[0], selectedPinIndexes[1]);

                    string sioInfo1 = data.GetValue(index1);
                    string sioInfo2 = data.GetValue(index2);

                    //If adjacent and both single SIOs then they can be paired
                    if ((index2 - index1) == 1 &&
                        sioInfo1 == CyPortConstants.SIOInfoValue_SINGLE_SIO &&
                        sioInfo2 == CyPortConstants.SIOInfoValue_SINGLE_SIO)
                    {
                        m_groupSIOButton.Enabled = true;
                        m_groupSIOButton.ToolTipText = string.Empty;
                    }

                    //If adjacent and the smaller index is FirstInSIOPair and the larger index is SecondInSIOPair
                    //they can be ungrouped.
                    if ((index2 - index1) == 1 &&
                        sioInfo1 == CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR &&
                        sioInfo2 == CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR)
                    {
                        m_ungroupSIOButton.Enabled = true;
                    }
                }

                m_deleteButton.Enabled = numLeafNodes - selectedPinIndexes.Count > 0;
                m_deleteButton.ToolTipText = (m_deleteButton.Enabled) ? string.Empty :
                    "Delete Selected Pin/s cannot be performed. Cannot have less than one pin remaining.";

                //otherwise already as low as it can go
                m_moveDownButton.Enabled = (selectedPinIndexes.Contains(numLeafNodes - 1) == false);

                //otherwise already as high as it can go
                m_moveUpButton.Enabled = (selectedPinIndexes.Contains(0) == false);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F2:
                    m_renameButton.PerformClick();
                    break;

                case Keys.Delete:
                    m_deleteButton.PerformClick();
                    break;

                case Keys.Control | Keys.A:
                    m_treeView.SelectAll();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void m_treeView_NodeMouseDoubleClick(object sender, CyTreeNodeMouseClickEventArgs e)
        {
            m_renameButton.PerformClick();
        }

        void m_numPinsTextBox_Validated(object sender, EventArgs e)
        {
            UpdateFromExprs();
        }

        void RenamePinClicked(object sender, EventArgs e)
        {
            UpdateToolStripItemsEnabled();
            if (m_renameButton.Enabled)
            {
                Debug.Assert(m_treeView.SelectedNodesCount == 1);
                List<int> indexes = GetPinIndexes(m_treeView.SelectedNodes);
                if (indexes.Count == 1)
                {
                    int selectedPinIndex = indexes[0];
                    Debug.Assert(selectedPinIndex >= 0);
                    if (selectedPinIndex >= 0)
                    {
                        CyTreeNode node = GetAssociatedNode(selectedPinIndex);
                        Debug.Assert(node != null);
                        if (node != null)
                        {
                            CyStringArrayParamData data;
                            CyCustErr err = CyStringArrayParamData.CreatePinAliasData(m_edit, true, out data);
                            Debug.Assert(err.IsOk, err.Message);

                            string alias = data.GetValue(selectedPinIndex);

                            CyPinAliasDialog dialog = new CyPinAliasDialog(data, selectedPinIndex);
                            dialog.Text = string.Format("Change Alias [{0}]", node.Text);
                            Form parent = this.FindForm();
                            if (dialog.ShowDialog(parent) == DialogResult.OK)
                            {
                                data = dialog.GetAlias();
                                m_edit.SetParamExpr(CyParamInfo.Formal_ParamName_PinAlisases, data.ToString());
                                m_edit.CommitParamExprs();
                                UpdateFromExprs();
                            }
                        }
                    }
                }
            }
        }

        void DeletePinClicked(object sender, EventArgs e)
        {
            UpdateToolStripItemsEnabled();
            if (m_deleteButton.Enabled)
            {
                List<int> selectedPinIndexes = GetPinIndexes(m_treeView.SelectedNodes);
                Debug.Assert(selectedPinIndexes.Count >= 1);

                byte width;
                CyCustErr err = CyParamInfo.GetNumPinsValue(m_edit, out width);
                Debug.Assert(err.IsOk, err.Message);

                foreach (int selectedPinIndex in selectedPinIndexes)
                {
                    Debug.Assert(selectedPinIndex >= 0);
                    foreach (string paramName in m_edit.GetParamNames())
                    {
                        CyCompDevParam param = m_edit.GetCommittedParam(paramName);

                        if (IsHandledPerPinParam(param))
                        {
                            //Don't get the actual data class because we don't what Input/Onput only 
                            //values to be altered before we do the swap.

                            string dataString;
                            err = CyPortCustomizer.GetParamValue<string>(m_edit, paramName, out dataString);
                            Debug.Assert(dataString != null);

                            CyStringArrayParamData data;
                            if (paramName == CyParamInfo.Formal_ParamName_PinAlisases)
                            {
                                data = new CyStringArrayParamData.CyAliasParamData(dataString);
                            }
                            else if (paramName == CyParamInfo.Formal_ParamName_IOVoltages)
                            {
                                data = new CyStringArrayParamData.CyIOVoltagesParamData(dataString);
                            }
                            else
                            {
                                data = new CyStringArrayParamData.CyBitVectorParamData(dataString, string.Empty);
                            }

                            for (int i = selectedPinIndex; i < width; i++)
                            {
                                string nextValue = data.GetValue(i + 1);
                                data.SafeSetValue(i, nextValue);
                            }

                            m_edit.SetParamExpr(paramName, data.ToString());
                        }
                    }

                    int newWidth = width - selectedPinIndexes.Count;
                    m_edit.SetParamExpr(CyParamInfo.Formal_ParamName_NumPins, newWidth.ToString());
                    m_edit.CommitParamExprs();

                    //The tree needs to be rebuilt because SIO settings/Pairings could have changed.
                    m_treeView.AfterMultiSelect -= m_treeView_AfterMultiSelect;
                    UpdateFromExprs();
                    m_treeView.AfterMultiSelect += m_treeView_AfterMultiSelect;
                    SelectPin(GetMin(selectedPinIndexes));
                }
            }
        }

        int GetMin(IEnumerable<int> nums)
        {
            int min = -1;
            foreach (int i in nums)
            {
                if (min == -1 || min > i)
                {
                    min = i;
                }
            }
            Debug.Assert(min != -1);
            return min;
        }

        void MovePinUpClicked(object sender, EventArgs e)
        {
            UpdateToolStripItemsEnabled();
            if (m_moveUpButton.Enabled)
            {
                List<int> selectedPinIndexes = GetPinIndexes(m_treeView.SelectedNodes);
                Debug.Assert(selectedPinIndexes.Count >= 1);

                selectedPinIndexes.Sort();
                List<int> newSelectedPins = new List<int>();

                foreach (int i in selectedPinIndexes)
                {
                    newSelectedPins.Add(i - 1);
                }

                foreach (string paramName in m_edit.GetParamNames())
                {
                    CyCompDevParam param = m_edit.GetCommittedParam(paramName);

                    if (IsHandledPerPinParam(param))
                    {
                        //Don't get the actual data class because we don't what Input/Onput only values to be altered
                        //before we do the swap.

                        string dataString;
                        CyCustErr err = CyPortCustomizer.GetParamValue<string>(m_edit, paramName, out dataString);
                        Debug.Assert(err.IsOk, err.Message);

                        CyStringArrayParamData data;
                        if (paramName == CyParamInfo.Formal_ParamName_PinAlisases)
                        {
                            data = new CyStringArrayParamData.CyAliasParamData(dataString);
                        }
                        else if (paramName == CyParamInfo.Formal_ParamName_IOVoltages)
                        {
                            data = new CyStringArrayParamData.CyIOVoltagesParamData(dataString);
                        }
                        else
                        {
                            data = new CyStringArrayParamData.CyBitVectorParamData(dataString, string.Empty);
                        }

                        for (int i = 0; i < selectedPinIndexes.Count; i++)
                        {
                            int pin1Index = selectedPinIndexes[i];
                            int pin2Index = pin1Index - 1;

                            string pin1Value = data.GetValue(pin1Index);
                            string pin2Value = data.GetValue(pin2Index);
                            data.SafeSetValue(pin1Index, pin2Value);
                            data.SafeSetValue(pin2Index, pin1Value);
                        }

                        m_edit.SetParamExpr(paramName, data.ToString());
                    }
                }

                m_edit.CommitParamExprs();

                m_treeView.AfterMultiSelect -= m_treeView_AfterMultiSelect;
                UpdateFromExprs();
                m_treeView.AfterMultiSelect += m_treeView_AfterMultiSelect;
                SelectPins(newSelectedPins);
            }
        }

        void MovePinDownClicked(object sender, EventArgs e)
        {
            UpdateToolStripItemsEnabled();
            if (m_moveDownButton.Enabled)
            {
                List<int> selectedPinIndexes = GetPinIndexes(m_treeView.SelectedNodes);
                Debug.Assert(selectedPinIndexes.Count >= 1);

                selectedPinIndexes.Sort();
                List<int> newSelectedPins = new List<int>();

                foreach (int i in selectedPinIndexes)
                {
                    newSelectedPins.Add(i + 1);
                }

                foreach (string paramName in m_edit.GetParamNames())
                {
                    CyCompDevParam param = m_edit.GetCommittedParam(paramName);

                    if (IsHandledPerPinParam(param))
                    {
                        //Don't get the actual data class because we don't what Input/Onput only values to be altered
                        //before we do the swap.

                        string dataString;
                        CyCustErr err = CyPortCustomizer.GetParamValue<string>(m_edit, paramName, out dataString);
                        Debug.Assert(err.IsOk, err.Message);

                        CyStringArrayParamData data;
                        if (paramName == CyParamInfo.Formal_ParamName_PinAlisases)
                        {
                            data = new CyStringArrayParamData.CyAliasParamData(dataString);
                        }
                        else if (paramName == CyParamInfo.Formal_ParamName_IOVoltages)
                        {
                            data = new CyStringArrayParamData.CyIOVoltagesParamData(dataString);
                        }
                        else
                        {
                            data = new CyStringArrayParamData.CyBitVectorParamData(dataString, string.Empty);
                        }

                        for (int i = selectedPinIndexes.Count - 1; i >= 0; i--)
                        {
                            int pin1Index = selectedPinIndexes[i];
                            int pin2Index = pin1Index + 1;

                            string pin1Value = data.GetValue(pin1Index);
                            string pin2Value = data.GetValue(pin2Index);
                            data.SafeSetValue(pin1Index, pin2Value);
                            data.SafeSetValue(pin2Index, pin1Value);
                        }

                        m_edit.SetParamExpr(paramName, data.ToString());
                    }
                }

                m_edit.CommitParamExprs();

                m_treeView.AfterMultiSelect -= m_treeView_AfterMultiSelect;
                UpdateFromExprs();
                m_treeView.AfterMultiSelect += m_treeView_AfterMultiSelect;
                SelectPins(newSelectedPins);
            }
        }

        void PairSIOsClicked(object sender, EventArgs e)
        {
            UpdateToolStripItemsEnabled();
            if (m_groupSIOButton.Enabled)
            {
                _setSIOGroups(CyPortConstants.SIOGroupValue_GROUPED);
            }
        }

        void UnpairSIOsClicked(object sender, EventArgs e)
        {
            UpdateToolStripItemsEnabled();
            if (m_ungroupSIOButton.Enabled)
            {
                _setSIOGroups(CyPortConstants.SIOGroupValue_NOT_GROUPED);
            }
        }

        void _setSIOGroups(string value)
        {
            List<int> selectedPinIndexes = GetPinIndexes(m_treeView.SelectedNodes);
            Debug.Assert(selectedPinIndexes.Count == 2);

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(m_edit,
                CyParamInfo.Formal_ParamName_SIOGroups, out data);

            Debug.Assert(data != null, "a syntax error exists, cannot update");
            if (data != null)
            {
                foreach (int selectedPinIndex in selectedPinIndexes)
                {
                    Debug.Assert(selectedPinIndex >= 0);
                    data.SafeSetValue(selectedPinIndex, value);
                }

                m_edit.SetParamExpr(CyParamInfo.Formal_ParamName_SIOGroups, data.ToString());
                m_edit.CommitParamExprs();

                m_treeView.AfterMultiSelect -= m_treeView_AfterMultiSelect;
                UpdateFromExprs();
                m_treeView.AfterMultiSelect += m_treeView_AfterMultiSelect;
                SelectPins(selectedPinIndexes);
            }
        }

        //void PasteClicked(object sender, EventArgs e)
        //{
        //    IDataObject iData = Clipboard.GetDataObject();
        //    if (iData.GetDataPresent(PinClipboardDataFormat))
        //    {
        //        string values = (string)iData.GetData(PinClipboardDataFormat);
        //        string[] vals = values.Split(';');
        //        int i = 0;
        //        foreach (string s in vals)
        //        {
        //            i++;
        //        }
        //    }
        //}

        //void CopyPinClicked(object sender, EventArgs e)
        //{
        //    int selectedPinIndex = GetPinIndex(m_treeView.SelectedNode);
        //    Debug.Assert(selectedPinIndex != -1);
        //    if (selectedPinIndex != -1)
        //    {
        //        List<string> values = new List<string>();

        //        foreach (string paramName in m_edit.GetParamNames())
        //        {
        //            CyCompDevParam param = m_edit.GetCommittedParam(paramName);
        //            if (IsHandledPerPinParam(param))
        //            {
        //                string dataString;
        //                CyCustErr err = CyPortCustomizer.GetParamValue<string>(m_edit, paramName, out dataString);
        //                Debug.Assert(err.IsOk, err.Message);

        //                CyStringArrayParamData data;
        //                err = CyStringArrayParamData.CreateStringArrayData(m_edit, paramName, out data);
        //                Debug.Assert(err.IsOk, err.Message);

        //                values.Add(string.Format("{0},{1}", paramName, data.GetValue(selectedPinIndex)));
        //            }
        //        }

        //        Clipboard.SetData(PinClipboardDataFormat, string.Join(";", values.ToArray()));
        //    }
        //}

        #endregion

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            foreach (string paramName in m_edit.GetParamNames())
            {
                CyCompDevParam param = m_edit.GetCommittedParam(paramName);
                if (param.TabName == CyCustomizer.ConfigureTabName)
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }
            }

            return errs;
        }

        #endregion
    }

    public class CyNumPinsToolStripTextBox : ToolStripTextBox
    {
        ICyInstEdit_v1 m_edit;

        public CyNumPinsToolStripTextBox(ICyInstEdit_v1 edit)
        {
            m_edit = edit;
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size size = base.GetPreferredSize(constrainingSize);
            size.Width = 40;
            return size;
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            m_edit.SetParamExpr(CyParamInfo.Formal_ParamName_NumPins, Text);
            m_edit.CommitParamExprs();

            CyCompDevParam param = m_edit.GetCommittedParam(CyParamInfo.Formal_ParamName_NumPins);
            e.Cancel = param.ErrorCount > 0;

            if (e.Cancel)
            {
                MessageBox.Show(param.ErrorMsgs, "Invalid Number of Pins", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
            }

            base.OnValidating(e);
        }
    }

    public class CyPerPinDataControl : UserControl
    {
        CyPinsControl m_pinControl;

        CyTypeControl m_typeControl;
        CyGeneralControl m_generalControl;
        CyInputControl m_inputControl;
        CyOutputControl m_outputControl;

        TabStripPage m_typeTab;
        TabStripPage m_generalTab;
        TabStripPage m_inputTab;
        TabStripPage m_outputTab;

        public CyPerPinDataControl(CyPinsControl pinControl)
        {
            this.Dock = DockStyle.Fill;

            m_pinControl = pinControl;
            TabStrip tabControl = new TabStrip();
            tabControl.PageValidationEnabled = true;
            tabControl.Renderer = new VisualStudio2005DocumentWindowTabStripRenderer();

            m_typeTab = new TabStripPage();
            m_typeTab.Text = "Type";
            m_typeControl = new CyTypeControl();
            m_typeControl.PerPinDataControl = this;
            m_typeControl.Dock = DockStyle.Fill;
            m_typeTab.Controls.Add(m_typeControl);
            tabControl.Pages.Add(m_typeTab);

            m_generalTab = new TabStripPage();
            m_generalTab.Text = "General";
            m_generalControl = new CyGeneralControl();
            m_generalControl.PerPinDataControl = this;
            m_generalControl.Dock = DockStyle.Fill;
            m_generalTab.Controls.Add(m_generalControl);
            tabControl.Pages.Add(m_generalTab);

            m_inputTab = new TabStripPage();
            m_inputTab.Text = "Input";
            m_inputControl = new CyInputControl();
            m_inputControl.PerPinDataControl = this;
            m_inputControl.Dock = DockStyle.Fill;
            m_inputTab.Controls.Add(m_inputControl);
            tabControl.Pages.Add(m_inputTab);

            m_outputTab = new TabStripPage();
            m_outputTab.Text = "Output";
            m_outputControl = new CyOutputControl();
            m_outputControl.PerPinDataControl = this;
            m_outputControl.Dock = DockStyle.Fill;
            m_outputTab.Controls.Add(m_outputControl);
            tabControl.Pages.Add(m_outputTab);

            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);
        }

        public bool InputTabEnabled
        {
            get { return m_inputTab.Enabled; }
            set
            {
                m_inputTab.Enabled = value;
                m_inputTab.ToolTipText = (value) ? string.Empty :
                    "None of the selected pins have their PinType configured to use input settings.";
            }
        }

        public bool OutputTabEnabled
        {
            get { return m_outputTab.Enabled; }
            set
            {
                m_outputTab.Enabled = value;
                m_outputTab.ToolTipText = (value) ? string.Empty :
                    "None of the selected pins have their PinType configured to use output settings.";
            }
        }

        #region General Options

        public string DriveMode
        {
            get { return m_generalControl.DriveMode; }
            set { m_generalControl.DriveMode = value; }
        }

        public string InitialDriveState
        {
            get { return m_generalControl.InitialDriveState; }
            set { m_generalControl.InitialDriveState = value; }
        }

        public string SupplyVoltage
        {
            get { return m_generalControl.SupplyVoltage; }
            set { m_generalControl.SupplyVoltage = value; }
        }

        public string DriveModeErrorText
        {
            get { return m_generalControl.DriveModeErrorText; }
            set
            {
                m_generalControl.DriveModeErrorText = value;
                UpdateGeneralTabErrorIcon();
            }
        }

        public string InitialDriveStateErrorText
        {
            get { return m_generalControl.InitialDriveStateErrorText; }
            set
            {
                m_generalControl.InitialDriveStateErrorText = value;
                UpdateGeneralTabErrorIcon();
            }
        }

        public string SupplyVoltageErrorText
        {
            get { return m_generalControl.SupplyVoltageErrorText; }
            set
            {
                m_generalControl.SupplyVoltageErrorText = value;
                UpdateGeneralTabErrorIcon();
            }
        }

        #endregion

        #region Input Options

        public string InterruptMode
        {
            get { return m_inputControl.InterruptMode; }
            set { m_inputControl.InterruptMode = value; }
        }

        public string HotSwap
        {
            get { return m_inputControl.HotSwap; }
            set { m_inputControl.HotSwap = value; }
        }

        public string InputThresholdLevel
        {
            get { return m_inputControl.InputThresholdLevel; }
            set { m_inputControl.InputThresholdLevel = value; }
        }

        public string InputBufferEnabled
        {
            get { return m_inputControl.InputBufferEnabled; }
            set { m_inputControl.InputBufferEnabled = value; }
        }

        public string InputSynchronized
        {
            get { return m_inputControl.InputSynchronized; }
            set { m_inputControl.InputSynchronized = value; }
        }

        public string InterruptModeErrorText
        {
            get { return m_inputControl.InterruptModeErrorText; }
            set
            {
                m_inputControl.InterruptModeErrorText = value;
                UpdateInputTabErrorIcon();
            }
        }

        public string HotSwapErrorText
        {
            get { return m_inputControl.HotSwapErrorText; }
            set
            {
                m_inputControl.HotSwapErrorText = value;
                UpdateInputTabErrorIcon();
            }
        }

        public string InputThresholdLevelErrorText
        {
            get { return m_inputControl.InputThresholdLevelErrorText; }
            set
            {
                m_inputControl.InputThresholdLevelErrorText = value;
                UpdateInputTabErrorIcon();
            }
        }

        public string InputBufferEnabledErrorText
        {
            get { return m_inputControl.InputBufferEnabledErrorText; }
            set
            {
                m_inputControl.InputBufferEnabledErrorText = value;
                UpdateInputTabErrorIcon();
            }
        }

        public string InputSynchronizedErrorText
        {
            get { return m_inputControl.InputSynchronizedErrorText; }
            set
            {
                m_inputControl.InputSynchronizedErrorText = value;
                UpdateInputTabErrorIcon();
            }
        }

        #endregion

        #region Output Options

        public string SlewRate
        {
            get { return m_outputControl.SlewRate; }
            set { m_outputControl.SlewRate = value; }
        }

        public string OutputDriveLevel
        {
            get { return m_outputControl.OutputDriveLevel; }
            set { m_outputControl.OutputDriveLevel = value; }
        }

        public string DriveCurrent
        {
            get { return m_outputControl.OutputDriveCurrent; }
            set { m_outputControl.OutputDriveCurrent = value; }
        }

        public string OutputSynchronized
        {
            get { return m_outputControl.OutputSynchronized; }
            set { m_outputControl.OutputSynchronized = value; }
        }

        public string SlewRateErrorText
        {
            get { return m_outputControl.SlewRateErrorText; }
            set
            {
                m_outputControl.SlewRateErrorText = value;
                UpdateOutputTabErrorIcon();
            }
        }

        public string OutputDriveLevelErrorText
        {
            get { return m_outputControl.OutputDriveLevelErrorText; }
            set
            {
                m_outputControl.OutputDriveLevelErrorText = value;
                UpdateOutputTabErrorIcon();
            }
        }

        public string DriveCurrentErrorText
        {
            get { return m_outputControl.OutputDriveCurrentErrorText; }
            set
            {
                m_outputControl.OutputDriveCurrentErrorText = value;
                UpdateOutputTabErrorIcon();
            }
        }

        public string OutputSynchronizedErrorText
        {
            get { return m_outputControl.OutputSynchronizedErrorText; }
            set
            {
                m_outputControl.OutputSynchronizedErrorText = value;
                UpdateOutputTabErrorIcon();
            }
        }

        #endregion

        #region Type Options

        public CheckState AnalogState
        {
            get { return m_typeControl.AnalogCheckState; }
            set { m_typeControl.AnalogCheckState = value; }
        }

        public CheckState InputState
        {
            get { return m_typeControl.InputCheckState; }
            set { m_typeControl.InputCheckState = value; }
        }

        public CheckState OutputState
        {
            get { return m_typeControl.OutputCheckState; }
            set { m_typeControl.OutputCheckState = value; }
        }

        public CheckState OEState
        {
            get { return m_typeControl.OECheckState; }
            set { m_typeControl.OECheckState = value; }
        }

        public CheckState BidirState
        {
            get { return m_typeControl.BidirCheckState; }
            set { m_typeControl.BidirCheckState = value; }
        }

        public string DisplayInputHWConnections
        {
            get { return m_typeControl.DisplayInputHWConnections; }
            set { m_typeControl.DisplayInputHWConnections = value; }
        }

        public string DisplayOutputHWConnections
        {
            get { return m_typeControl.DisplayOutputHWConnections; }
            set { m_typeControl.DisplayOutputHWConnections = value; }
        }

        public string DisplayInputHWConnectionsErrorText
        {
            get { return m_typeControl.DisplayInputHWConnectionsErrorText; }
            set
            {
                m_typeControl.DisplayInputHWConnectionsErrorText = value;
                UpdateTypeTabErrorIcon();
            }
        }

        public string DisplayOutputHWConnectionsErrorText
        {
            get { return m_typeControl.DisplayOutputConnectionsErrorText; }
            set
            {
                m_typeControl.DisplayOutputConnectionsErrorText = value;
                UpdateTypeTabErrorIcon();
            }
        }

        public string PinTypeErrorText
        {
            get { return m_typeControl.PinTypeErrorText; }
            set
            {
                m_typeControl.PinTypeErrorText = value;
                UpdateTypeTabErrorIcon();
            }
        }

        #endregion

        void UpdateInputTabErrorIcon()
        {
            if (string.IsNullOrEmpty(InterruptModeErrorText) && string.IsNullOrEmpty(HotSwapErrorText) &&
                string.IsNullOrEmpty(InputThresholdLevelErrorText) &&
                string.IsNullOrEmpty(InputBufferEnabledErrorText) && string.IsNullOrEmpty(InputSynchronizedErrorText))
            {
                m_inputTab.ContextImage = null;
            }
            else
            {
                m_inputTab.ContextImage = Resource1.ErrorImage;
            }
        }

        void UpdateOutputTabErrorIcon()
        {
            if (string.IsNullOrEmpty(SlewRateErrorText) && string.IsNullOrEmpty(OutputDriveLevelErrorText) &&
                string.IsNullOrEmpty(DriveCurrentErrorText) && string.IsNullOrEmpty(OutputSynchronizedErrorText))
            {
                m_outputTab.ContextImage = null;
            }
            else
            {
                m_outputTab.ContextImage = Resource1.ErrorImage;
            }
        }

        void UpdateGeneralTabErrorIcon()
        {
            if (string.IsNullOrEmpty(DriveModeErrorText) && string.IsNullOrEmpty(InitialDriveStateErrorText) &&
                string.IsNullOrEmpty(SupplyVoltageErrorText))
            {
                m_generalTab.ContextImage = null;
            }
            else
            {
                m_generalTab.ContextImage = Resource1.ErrorImage;
            }
        }

        void UpdateTypeTabErrorIcon()
        {
            if (string.IsNullOrEmpty(DisplayInputHWConnectionsErrorText) &&
                string.IsNullOrEmpty(DisplayOutputHWConnectionsErrorText) &&
                string.IsNullOrEmpty(PinTypeErrorText))
            {
                m_typeTab.ContextImage = null;
            }
            else
            {
                m_typeTab.ContextImage = Resource1.ErrorImage;
            }
        }

        internal void OnParamDataChangeByUser(CyPerPinDataEventArgs e)
        {
            m_pinControl.PerPinParamDataChangedByUser(e);
        }

        internal void OnPinTypeChanged(CheckState analogState, CheckState inputState, CheckState outputState,
            CheckState oeState, CheckState bidirState)
        {
            m_pinControl.PinTypeChangedByUser(analogState, inputState, outputState, oeState, bidirState);
        }
    }

    public class CyPerPinDataEventArgs : EventArgs
    {
        string m_paramName;
        string m_value;

        public string ParamName { get { return m_paramName; } }
        public string Value { get { return m_value; } }

        public CyPerPinDataEventArgs(string paramName, string value)
        {
            m_paramName = paramName;
            m_value = value;
        }
    }
}
