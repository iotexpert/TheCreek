/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using CapSense_CSD_v2_0;

namespace CapSense_CSD_v2_0.Tabs
{
    public partial class CyWidgetsTab : CyCSParamEditTemplate
    {
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButtonRename;
        public PropertyGrid property;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsbImportTuning;
        private ToolStripButton tsbExportTuning;
        private PictureBox pbInfo;
        #region Windows Form Designer generated code
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyWidgetsTab));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.trvWidgetList = new System.Windows.Forms.TreeView();
            this.m_treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.property = new System.Windows.Forms.PropertyGrid();
            this.toolStripOperations = new System.Windows.Forms.ToolStrip();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorRemove = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRename = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbImportTuning = new System.Windows.Forms.ToolStripButton();
            this.tsbExportTuning = new System.Windows.Forms.ToolStripButton();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.toolStripOperations.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.trvWidgetList);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.AutoScroll = true;
            this.splitContainer.Panel2.Controls.Add(this.pbInfo);
            this.splitContainer.Panel2.Controls.Add(this.property);
            this.splitContainer.Size = new System.Drawing.Size(468, 319);
            this.splitContainer.SplitterDistance = 170;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 2;
            // 
            // trvWidgetList
            // 
            this.trvWidgetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvWidgetList.HideSelection = false;
            this.trvWidgetList.ImageIndex = 0;
            this.trvWidgetList.ImageList = this.m_treeImageList;
            this.trvWidgetList.Location = new System.Drawing.Point(0, 0);
            this.trvWidgetList.MinimumSize = new System.Drawing.Size(138, 4);
            this.trvWidgetList.Name = "trvWidgetList";
            this.trvWidgetList.SelectedImageIndex = 0;
            this.trvWidgetList.Size = new System.Drawing.Size(170, 319);
            this.trvWidgetList.TabIndex = 0;
            this.trvWidgetList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trvWidgetList_MouseDoubleClick);
            this.trvWidgetList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvWidgetList_AfterSelect);
            // 
            // m_treeImageList
            // 
            this.m_treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_treeImageList.ImageStream")));
            this.m_treeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.m_treeImageList.Images.SetKeyName(0, "Symbol Configuration.gif");
            this.m_treeImageList.Images.SetKeyName(1, "Folder.gif");
            // 
            // pbInfo
            // 
            this.pbInfo.Location = new System.Drawing.Point(47, 54);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(120, 79);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbInfo.TabIndex = 3;
            this.pbInfo.TabStop = false;
            // 
            // property
            // 
            this.property.HelpVisible = false;
            this.property.Location = new System.Drawing.Point(98, 170);
            this.property.Margin = new System.Windows.Forms.Padding(4);
            this.property.Name = "property";
            this.property.Size = new System.Drawing.Size(195, 149);
            this.property.TabIndex = 2;
            this.property.ToolbarVisible = false;
            // 
            // toolStripOperations
            // 
            this.toolStripOperations.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripOperations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAdd,
            this.toolStripSeparatorRemove,
            this.toolStripButtonRemove,
            this.toolStripSeparator1,
            this.toolStripButtonRename,
            this.toolStripSeparator2,
            this.tsbImportTuning,
            this.tsbExportTuning});
            this.toolStripOperations.Location = new System.Drawing.Point(0, 0);
            this.toolStripOperations.Name = "toolStripOperations";
            this.toolStripOperations.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripOperations.Size = new System.Drawing.Size(468, 25);
            this.toolStripOperations.TabIndex = 0;
            this.toolStripOperations.Text = "toolStrip1";
            // 
            // tsbAdd
            // 
            this.tsbAdd.Enabled = false;
            this.tsbAdd.Image = global::CapSense_CSD_v2_0.CyCsResource.imadd;
            this.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Size = new System.Drawing.Size(53, 22);
            this.tsbAdd.Text = "Add";
            this.tsbAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripSeparatorRemove
            // 
            this.toolStripSeparatorRemove.Name = "toolStripSeparatorRemove";
            this.toolStripSeparatorRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.Enabled = false;
            this.toolStripButtonRemove.Image = global::CapSense_CSD_v2_0.CyCsResource.imdelete;
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(82, 22);
            this.toolStripButtonRemove.Text = "Remove";
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRename
            // 
            this.toolStripButtonRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRename.Enabled = false;
            this.toolStripButtonRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRename.Name = "toolStripButtonRename";
            this.toolStripButtonRename.Size = new System.Drawing.Size(66, 22);
            this.toolStripButtonRename.Text = "Rename";
            this.toolStripButtonRename.Click += new System.EventHandler(this.toolStripButtonRename_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbImportTuning
            // 
            this.tsbImportTuning.Image = global::CapSense_CSD_v2_0.CyCsResource.imimport;
            this.tsbImportTuning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportTuning.Name = "tsbImportTuning";
            this.tsbImportTuning.Size = new System.Drawing.Size(122, 22);
            this.tsbImportTuning.Text = "Import Tuning";
            this.tsbImportTuning.Click += new System.EventHandler(this.tsbImportTuning_Click);
            // 
            // tsbExportTuning
            // 
            this.tsbExportTuning.Image = global::CapSense_CSD_v2_0.CyCsResource.imsave;
            this.tsbExportTuning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportTuning.Name = "tsbExportTuning";
            this.tsbExportTuning.Size = new System.Drawing.Size(119, 22);
            this.tsbExportTuning.Text = "Export Tuning";
            this.tsbExportTuning.Click += new System.EventHandler(this.tsbExportTuning_Click);
            // 
            // CyWidgetsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStripOperations);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CyWidgetsTab";
            this.Size = new System.Drawing.Size(468, 344);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.toolStripOperations.ResumeLayout(false);
            this.toolStripOperations.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.SplitContainer splitContainer;
        private TreeView trvWidgetList;
        #endregion

        TreeNode m_treeMainNode = new TreeNode("All Widgets");
        TreeNode m_treeButtonsNode = new TreeNode("Buttons", 1, 1);
        TreeNode m_treeLinearSlidersNode = new TreeNode("Linear Sliders", 1, 1);
        TreeNode m_treeRadialSlidersNode = new TreeNode("Radial Sliders", 1, 1);
        TreeNode m_treeTouchPadsNode = new TreeNode("Touch Pads", 1, 1);
        TreeNode m_treeMatrixButtonsNode = new TreeNode("Matrix Buttons", 1, 1);
        TreeNode m_treeProximitySensorsNode = new TreeNode("Proximity Sensors", 1, 1);
        TreeNode m_treeGenericSensorsNode = new TreeNode("Generics", 1, 1);
        TreeNode m_elementGuardSensor = new TreeNode(CyCsConst.P_GUARD_SENSOR);
        private ToolStrip toolStripOperations;
        private ToolStripButton toolStripButtonRemove;
        private ToolStripSeparator toolStripSeparatorRemove;
        private ToolStripButton tsbAdd;
        private ImageList m_treeImageList;

        public CyWidgetsTab(CyCSParameters packParams)
            : base()
        {
            m_packParams = packParams;
            InitializeComponent();

            property.PropertySort = PropertySort.Categorized;
            property.Dock = DockStyle.Fill;
            pbInfo.Dock = DockStyle.Fill;
            pbInfo.Visible = false;
            //pbInfo.

            trvWidgetList.BeginUpdate();
            trvWidgetList.Nodes.Add(m_treeButtonsNode);
            trvWidgetList.Nodes.Add(m_treeLinearSlidersNode);
            trvWidgetList.Nodes.Add(m_treeRadialSlidersNode);
            trvWidgetList.Nodes.Add(m_treeMatrixButtonsNode);
            trvWidgetList.Nodes.Add(m_treeTouchPadsNode);
            trvWidgetList.Nodes.Add(m_treeProximitySensorsNode);
            trvWidgetList.Nodes.Add(m_treeGenericSensorsNode);
            trvWidgetList.EndUpdate();
            property.PropertyValueChanged += new PropertyValueChangedEventHandler(m_packParams.SetCommitParams);
            packParams.m_updateAll += new EventHandler(GetProperties); 
        }
      
        public void GuardSensorVisibleChange(bool enable)
        {
            if (trvWidgetList.Nodes.IndexOf(m_elementGuardSensor) == -1 && enable == false) return;
            if (trvWidgetList.Nodes.IndexOf(m_elementGuardSensor) != -1 && enable) return;
            
            trvWidgetList.BeginUpdate();
            if (enable)
                trvWidgetList.Nodes.Add(m_elementGuardSensor);
            else
                trvWidgetList.Nodes.Remove(m_elementGuardSensor);
            trvWidgetList.ExpandAll();
            trvWidgetList.EndUpdate();
        }

        void GetProperties(object sender, EventArgs e)
        {
            if (m_packParams != null)
            {
                trvWidgetList.BeginUpdate();
                foreach (TreeNode item in trvWidgetList.Nodes)
                {
                    item.Nodes.Clear();
                }
                trvWidgetList.EndUpdate();
                foreach (CyWidget item in m_packParams.m_widgets.GetListWidgets())
                {
                    AddNode(item);
                }
                GuardSensorVisibleChange(m_packParams.m_settings.m_guardSensorEnable);
            }
        }
        void AssignExample(TreeNode node)
        {
            if (node.Parent != null) return;
            CySensorType[] type = GetWidgetType(node);
            pbInfo.Image = CyCsResource.blank;
            if (type == null || type.Length < 1) return;
            switch (type[0])
            {
                case CySensorType.Button:
                    pbInfo.Image = CyCsResource.example_buttons;
                    break;
                case CySensorType.SliderLinear:
                    pbInfo.Image = CyCsResource.example_slider;
                    break;
                case CySensorType.SliderRadial:
                    pbInfo.Image = CyCsResource.example_sliderradial;
                    break;
                case CySensorType.TouchpadColumn:
                case CySensorType.TouchpadRow:
                    pbInfo.Image = CyCsResource.example_touchpad;
                    break;
                case CySensorType.MatrixButtonsColumn:
                case CySensorType.MatrixButtonsRow:
                    pbInfo.Image = CyCsResource.example_matrixbutton;
                    break;
                case CySensorType.Proximity:
                    pbInfo.Image = CyCsResource.example_proximity;
                    break;
                case CySensorType.Generic:
                    pbInfo.Image = CyCsResource.example_generic;
                    break;
                default:
                    return;
            }
            pbInfo.Size = pbInfo.Image.Size;
        }
        void AssignProperties(CyWidget widget, bool guardSensor)
        {
            bool isWidget = (widget != null);
            pbInfo.Visible = isWidget == false;
            property.Visible = isWidget;
            toolStripButtonRemove.Enabled = isWidget && !guardSensor;
            toolStripButtonRename.Enabled = isWidget && !guardSensor;
            property.SelectedObject = widget;
            property.Invalidate();
        }
        TreeNode GetParentNode(CySensorType type)
        {
            switch (type)
            {
                case CySensorType.Button:
                    return m_treeButtonsNode;
                case CySensorType.SliderLinear:
                    return m_treeLinearSlidersNode;
                case CySensorType.SliderRadial:
                    return m_treeRadialSlidersNode;
                case CySensorType.TouchpadColumn:
                    return m_treeTouchPadsNode;
                case CySensorType.MatrixButtonsColumn:
                    return m_treeMatrixButtonsNode;
                case CySensorType.Proximity:
                    return m_treeProximitySensorsNode;
                case CySensorType.Generic:
                    return m_treeGenericSensorsNode;
            }
            return null;
        }
        CySensorType[] GetWidgetType(TreeNode treeNode)
        {
            CySensorType[] res = new CySensorType[0];
            res = treeNode == m_treeButtonsNode ? new CySensorType[] { CySensorType.Button } : res;
            res = treeNode == m_treeLinearSlidersNode ? new CySensorType[] { CySensorType.SliderLinear } : res;
            res = treeNode == m_treeRadialSlidersNode ? new CySensorType[] { CySensorType.SliderRadial } : res;
            res = treeNode == m_treeTouchPadsNode ?
                new CySensorType[] { CySensorType.TouchpadColumn, CySensorType.TouchpadRow } : res;
            res = treeNode == m_treeMatrixButtonsNode ?
                new CySensorType[] { CySensorType.MatrixButtonsColumn, CySensorType.MatrixButtonsRow } : res;
            res = treeNode == m_treeGenericSensorsNode ? new CySensorType[] { CySensorType.Generic } : res;
            res = treeNode == m_treeProximitySensorsNode ? new CySensorType[] { CySensorType.Proximity } : res;
            return res;
        }

        TreeNode AddNode(CyWidget node)
        {
            TreeNode tree = GetParentNode(node.m_type);
            if (tree != null)
            {
                trvWidgetList.BeginUpdate();
                TreeNode treeNode = tree.Nodes.Add(node.ToString(), node.m_name);
                trvWidgetList.ExpandAll();
                trvWidgetList.EndUpdate();
                return treeNode;
            }
            return null;
        }


        private void trvWidgetList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode treeNode = trvWidgetList.SelectedNode;
            CyWidget wi = null;

            //Select Node if nothing selected
            if (treeNode == null)
            {
                AssignProperties(null, false);
                if (trvWidgetList.Nodes.Count > 0)
                    trvWidgetList.SelectedNode = trvWidgetList.Nodes[0];
                return;
            }

            //Take widget            
            if ((treeNode != null) && (treeNode.Parent != null))
            {
                wi = m_packParams.m_widgets.FindWidget(treeNode.Name);
            }
            if (treeNode == m_elementGuardSensor)
                wi = m_packParams.m_widgets.m_guardSensorWidget;

            AssignProperties(wi, treeNode == m_elementGuardSensor);
            AssignExample(treeNode);

            tsbAdd.Enabled = treeNode != m_elementGuardSensor;
            tsbAdd.Text = "Add " + GetWidgetName(treeNode.Parent != null ? treeNode.Parent : treeNode, false);
        }
        

        private void toolStripButtonRename_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = trvWidgetList.SelectedNode;
            if (m_packParams!=null && treeNode != null && treeNode.Parent != null)
            {
                CyWidget wi = m_packParams.m_widgets.FindWidget(treeNode.Name);
                if (wi == null) return;

                Modules.CyAliasDialog dialog = new Modules.CyAliasDialog(m_packParams, wi);
                dialog.Text = string.Format(CyCsResource.ChangeNameHeader, wi.ToString());
                Form parent = this.FindForm();
                if (dialog.ShowDialog(parent) == DialogResult.OK)
                {
                    treeNode.Name = wi.ToString();
                    treeNode.Text = wi.m_name;
                    //Commit parameters
                    m_packParams.SetCommitParams(null, null);
                }
            }
        }
        private void trvWidgetList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            toolStripButtonRename_Click(null, null);
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = trvWidgetList.SelectedNode;
            if (treeNode != null)
            {
                CyChannelNumber chn = CyChannelNumber.First;
                treeNode = treeNode.Parent != null ? treeNode.Parent : treeNode;
                CySensorType[] type = GetWidgetType(treeNode);
                string wi_name = GetNewName(treeNode);
                for (int i = 0; i < type.Length; i++)
                {
                    CyWidget wi = m_packParams.m_widgets.AddWidget(wi_name, type[i], chn, false);
                    if (CyCsConst.IsMainPartOfWidget(wi.m_type))
                        treeNode = AddNode(wi);
                }
                //Commit parameters
                m_packParams.SetCommitParams(null, null);
                //Select current node
                if (treeNode != null)
                    trvWidgetList.SelectedNode = trvWidgetList.Nodes.Find(treeNode.Name, true)[0];
            }
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            TreeNode treeNode = trvWidgetList.SelectedNode;
            if (treeNode != null && treeNode.Parent != null)
            {
                m_packParams.m_widgets.RemoveWidget(treeNode.Name);
                treeNode.Parent.Nodes.Remove(treeNode);                
                //Commit parameters
                m_packParams.SetCommitParams(null, null);

                //Select next node in groupe
                if (trvWidgetList.SelectedNode == null || trvWidgetList.SelectedNode.Parent == null)
                {
                    if (trvWidgetList.SelectedNode.Nodes.Count > 0)
                        trvWidgetList.SelectedNode = trvWidgetList.SelectedNode.Nodes[0];
                }
            }
        }

        #region Service function
        string GetWidgetName(TreeNode parent, bool removeSpaces)
        {
            string basename = string.Empty;
            if (parent != null)
            {
                basename = parent.Text;
                if (string.IsNullOrEmpty(basename) == false && parent != m_elementGuardSensor)
                {
                    basename = basename.Remove(basename.Length - 1);
                    if (removeSpaces)
                        basename = basename.Replace(" ", string.Empty);
                }
            }
            return basename;
        }
        string GetNewName(TreeNode parent)
        {
            if (m_packParams != null)
            {
                string basename = GetWidgetName(parent, true);
                if (basename == string.Empty)
                    basename = "NewName";
                int p = 0;
                while (m_packParams.m_widgets.NameValidating(basename + p) == false) p++;
                return basename + p;
            }
            return string.Empty;
        }
        #endregion

        private void tsbExportTuning_Click(object sender, EventArgs e)
        {
            string content = m_packParams.Serialize(m_packParams.m_widgets);
            CyCSParameters.SaveToFile(content, CyCsConst.P_SAVED_WIDGETST_FILTER);
        }

        private void tsbImportTuning_Click(object sender, EventArgs e)
        {
            string content;
            if (CyCSParameters.ReadFromFile(CyCsConst.P_SAVED_WIDGETST_FILTER, out content))
            {
                CyWidgetsList param = (CyWidgetsList)m_packParams.Deserialize(content, typeof(CyWidgetsList), true);
                CyCSParameters.GLOBAL_EDIT_MODE = false;
                //Save parameters
                m_packParams.m_widgets = param;
                GetProperties(null, null);
                m_packParams.ExecuteUpdateAll();
                CyCSParameters.GLOBAL_EDIT_MODE = true;
                m_packParams.SetCommitParams(null, null);
            }
        }        
    }
}

