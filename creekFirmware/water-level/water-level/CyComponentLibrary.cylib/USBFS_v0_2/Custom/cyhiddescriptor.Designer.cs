namespace USBFS_v0_2
{
    partial class CyHIDDescriptor
    {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyHIDDescriptor));
            this.splitContainerReport = new System.Windows.Forms.SplitContainer();
            this.treeViewReport = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonApply = new System.Windows.Forms.Button();
            this.listBoxItems = new System.Windows.Forms.ListBox();
            this.labelItemSettings = new System.Windows.Forms.Label();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.toolTipList = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerReport.Panel1.SuspendLayout();
            this.splitContainerReport.Panel2.SuspendLayout();
            this.splitContainerReport.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainerDetails.Panel1.SuspendLayout();
            this.splitContainerDetails.Panel2.SuspendLayout();
            this.splitContainerDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerReport
            // 
            this.splitContainerReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerReport.Location = new System.Drawing.Point(0, 0);
            this.splitContainerReport.Name = "splitContainerReport";
            // 
            // splitContainerReport.Panel1
            // 
            this.splitContainerReport.Panel1.Controls.Add(this.treeViewReport);
            this.splitContainerReport.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainerReport.Panel2
            // 
            this.splitContainerReport.Panel2.Controls.Add(this.splitContainerDetails);
            this.splitContainerReport.Size = new System.Drawing.Size(486, 224);
            this.splitContainerReport.SplitterDistance = 246;
            this.splitContainerReport.TabIndex = 0;
            // 
            // treeViewReport
            // 
            this.treeViewReport.AllowDrop = true;
            this.treeViewReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewReport.HideSelection = false;
            this.treeViewReport.ImageIndex = 0;
            this.treeViewReport.ImageList = this.imageList;
            this.treeViewReport.LabelEdit = true;
            this.treeViewReport.Location = new System.Drawing.Point(0, 25);
            this.treeViewReport.Name = "treeViewReport";
            this.treeViewReport.SelectedImageIndex = 0;
            this.treeViewReport.Size = new System.Drawing.Size(246, 199);
            this.treeViewReport.TabIndex = 2;
            this.treeViewReport.DragLeave += new System.EventHandler(this.treeViewReport_DragLeave);
            this.treeViewReport.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewReport_AfterLabelEdit);
            this.treeViewReport.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragDrop);
            this.treeViewReport.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewReport_AfterSelect);
            this.treeViewReport.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragEnter);
            this.treeViewReport.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewReport_NodeMouseClick);
            this.treeViewReport.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewReport_BeforeLabelEdit);
            this.treeViewReport.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewReport_ItemDrag);
            this.treeViewReport.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragOver);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "USB Drive.png");
            this.imageList.Images.SetKeyName(2, "Configuration.png");
            this.imageList.Images.SetKeyName(3, "Script.png");
            this.imageList.Images.SetKeyName(4, "Save.png");
            this.imageList.Images.SetKeyName(5, "Component.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonImport,
            this.toolStripButtonSave,
            this.toolStripButtonRemove});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(246, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAdd.Image = global::USBFS_v0_2.Properties.Resources.Symbol_Add_2;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAdd.Text = "Add Report";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonImport
            // 
            this.toolStripButtonImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImport.Image = global::USBFS_v0_2.Properties.Resources.Import;
            this.toolStripButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImport.Name = "toolStripButtonImport";
            this.toolStripButtonImport.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonImport.Text = "Import Report";
            this.toolStripButtonImport.Click += new System.EventHandler(this.toolStripButtonImport_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::USBFS_v0_2.Properties.Resources.Save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save Report";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemove.Image = global::USBFS_v0_2.Properties.Resources.Symbol_Delete_2;
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemove.Text = "Delete";
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
            // 
            // splitContainerDetails
            // 
            this.splitContainerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDetails.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDetails.Name = "splitContainerDetails";
            // 
            // splitContainerDetails.Panel1
            // 
            this.splitContainerDetails.Panel1.Controls.Add(this.label1);
            this.splitContainerDetails.Panel1.Controls.Add(this.buttonApply);
            this.splitContainerDetails.Panel1.Controls.Add(this.listBoxItems);
            this.splitContainerDetails.Panel1.SizeChanged += new System.EventHandler(this.splitContainerDetails_Panel1_SizeChanged);
            // 
            // splitContainerDetails.Panel2
            // 
            this.splitContainerDetails.Panel2.Controls.Add(this.labelItemSettings);
            this.splitContainerDetails.Panel2.Controls.Add(this.panelDetails);
            this.splitContainerDetails.Size = new System.Drawing.Size(236, 224);
            this.splitContainerDetails.SplitterDistance = 97;
            this.splitContainerDetails.TabIndex = 0;
            this.splitContainerDetails.SizeChanged += new System.EventHandler(this.splitContainerDetails_SizeChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "HID Items List";
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonApply.Location = new System.Drawing.Point(6, 198);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 7;
            this.buttonApply.Text = "Add";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // listBoxItems
            // 
            this.listBoxItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxItems.FormattingEnabled = true;
            this.listBoxItems.Location = new System.Drawing.Point(3, 25);
            this.listBoxItems.Name = "listBoxItems";
            this.listBoxItems.Size = new System.Drawing.Size(91, 160);
            this.listBoxItems.TabIndex = 6;
            this.listBoxItems.SelectedIndexChanged += new System.EventHandler(this.listBoxItems_SelectedIndexChanged);
            this.listBoxItems.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBoxItems_MouseMove);
            this.listBoxItems.MouseLeave += new System.EventHandler(this.listBoxItems_MouseLeave);
            // 
            // labelItemSettings
            // 
            this.labelItemSettings.AutoSize = true;
            this.labelItemSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelItemSettings.Location = new System.Drawing.Point(3, 6);
            this.labelItemSettings.Name = "labelItemSettings";
            this.labelItemSettings.Size = new System.Drawing.Size(57, 13);
            this.labelItemSettings.TabIndex = 9;
            this.labelItemSettings.Text = "Item Value";
            // 
            // panelDetails
            // 
            this.panelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDetails.Location = new System.Drawing.Point(0, 25);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(135, 199);
            this.panelDetails.TabIndex = 1;
            // 
            // CyHIDDescriptor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerReport);
            this.Name = "CyHIDDescriptor";
            this.Size = new System.Drawing.Size(486, 224);
            this.VisibleChanged += new System.EventHandler(this.CyHIDDescriptor_VisibleChanged);
            this.splitContainerReport.Panel1.ResumeLayout(false);
            this.splitContainerReport.Panel1.PerformLayout();
            this.splitContainerReport.Panel2.ResumeLayout(false);
            this.splitContainerReport.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainerDetails.Panel1.ResumeLayout(false);
            this.splitContainerDetails.Panel1.PerformLayout();
            this.splitContainerDetails.Panel2.ResumeLayout(false);
            this.splitContainerDetails.Panel2.PerformLayout();
            this.splitContainerDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerReport;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainerDetails;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonImport;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TreeView treeViewReport;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.ListBox listBoxItems;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Label labelItemSettings;
        private System.Windows.Forms.ToolTip toolTipList;
    }
}
