namespace USBFS_v2_10
{
    partial class CyCDCDescriptorPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyCDCDescriptorPage));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("CDC Interfaces", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Functional Descriptors", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Endpoint Descriptors", System.Windows.Forms.HorizontalAlignment.Left);
            this.splitContainerReport = new System.Windows.Forms.SplitContainer();
            this.treeViewCDC = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelCb = new System.Windows.Forms.Panel();
            this.checkBoxEnableCDCApi = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
            this.listViewCDCDescList = new System.Windows.Forms.ListView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelItemProperties = new System.Windows.Forms.Label();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.panelAddbtn = new System.Windows.Forms.Panel();
            this.buttonApply = new System.Windows.Forms.Button();
            this.propertyGridCDC = new System.Windows.Forms.PropertyGrid();
            this.toolTipList = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerReport.Panel1.SuspendLayout();
            this.splitContainerReport.Panel2.SuspendLayout();
            this.splitContainerReport.SuspendLayout();
            this.panelCb.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainerDetails.Panel1.SuspendLayout();
            this.splitContainerDetails.Panel2.SuspendLayout();
            this.splitContainerDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelDetails.SuspendLayout();
            this.panelAddbtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerReport
            // 
            this.splitContainerReport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerReport.Location = new System.Drawing.Point(0, 0);
            this.splitContainerReport.Name = "splitContainerReport";
            // 
            // splitContainerReport.Panel1
            // 
            this.splitContainerReport.Panel1.Controls.Add(this.treeViewCDC);
            this.splitContainerReport.Panel1.Controls.Add(this.panelCb);
            this.splitContainerReport.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainerReport.Panel2
            // 
            this.splitContainerReport.Panel2.Controls.Add(this.splitContainerDetails);
            this.splitContainerReport.Size = new System.Drawing.Size(600, 363);
            this.splitContainerReport.SplitterDistance = 240;
            this.splitContainerReport.TabIndex = 0;
            // 
            // treeViewCDC
            // 
            this.treeViewCDC.AllowDrop = true;
            this.treeViewCDC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCDC.HideSelection = false;
            this.treeViewCDC.ImageIndex = 0;
            this.treeViewCDC.ImageList = this.imageList;
            this.treeViewCDC.LabelEdit = true;
            this.treeViewCDC.Location = new System.Drawing.Point(0, 25);
            this.treeViewCDC.Name = "treeViewCDC";
            this.treeViewCDC.SelectedImageIndex = 0;
            this.treeViewCDC.Size = new System.Drawing.Size(236, 295);
            this.treeViewCDC.TabIndex = 2;
            this.treeViewCDC.DragLeave += new System.EventHandler(this.treeViewReport_DragLeave);
            this.treeViewCDC.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewReport_AfterLabelEdit);
            this.treeViewCDC.Enter += new System.EventHandler(this.treeViewCDC_Enter);
            this.treeViewCDC.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragDrop);
            this.treeViewCDC.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewReport_AfterSelect);
            this.treeViewCDC.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragEnter);
            this.treeViewCDC.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewReport_BeforeLabelEdit);
            this.treeViewCDC.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewReport_KeyDown);
            this.treeViewCDC.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewReport_ItemDrag);
            this.treeViewCDC.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragOver);
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
            this.imageList.Images.SetKeyName(6, "Cable_Modem.png");
            // 
            // panelCb
            // 
            this.panelCb.Controls.Add(this.checkBoxEnableCDCApi);
            this.panelCb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCb.Location = new System.Drawing.Point(0, 320);
            this.panelCb.Name = "panelCb";
            this.panelCb.Size = new System.Drawing.Size(236, 39);
            this.panelCb.TabIndex = 3;
            // 
            // checkBoxEnableCDCApi
            // 
            this.checkBoxEnableCDCApi.AutoSize = true;
            this.checkBoxEnableCDCApi.Location = new System.Drawing.Point(16, 14);
            this.checkBoxEnableCDCApi.Name = "checkBoxEnableCDCApi";
            this.checkBoxEnableCDCApi.Size = new System.Drawing.Size(132, 17);
            this.checkBoxEnableCDCApi.TabIndex = 0;
            this.checkBoxEnableCDCApi.Text = "Enable CDC Class API";
            this.checkBoxEnableCDCApi.UseVisualStyleBackColor = true;
            this.checkBoxEnableCDCApi.CheckedChanged += new System.EventHandler(this.checkBoxEnableCDCApi_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRemove,
            this.toolStripButtonImport,
            this.toolStripButtonSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(236, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip";
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemove.Image = global::USBFS_v2_10.Properties.Resources.imdelete;
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemove.Text = "Delete";
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
            // 
            // toolStripButtonImport
            // 
            this.toolStripButtonImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImport.Image = global::USBFS_v2_10.Properties.Resources.imimport;
            this.toolStripButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImport.Name = "toolStripButtonImport";
            this.toolStripButtonImport.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonImport.Text = "Import CDC Interface";
            this.toolStripButtonImport.Click += new System.EventHandler(this.toolStripButtonImport_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::USBFS_v2_10.Properties.Resources.imsave;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save CDC Interface";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // splitContainerDetails
            // 
            this.splitContainerDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDetails.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDetails.Name = "splitContainerDetails";
            this.splitContainerDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDetails.Panel1
            // 
            this.splitContainerDetails.Panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainerDetails.Panel1.Controls.Add(this.listViewCDCDescList);
            this.splitContainerDetails.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainerDetails.Panel1.Controls.Add(this.label1);
            // 
            // splitContainerDetails.Panel2
            // 
            this.splitContainerDetails.Panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainerDetails.Panel2.Controls.Add(this.labelItemProperties);
            this.splitContainerDetails.Panel2.Controls.Add(this.panelDetails);
            this.splitContainerDetails.Size = new System.Drawing.Size(356, 363);
            this.splitContainerDetails.SplitterDistance = 155;
            this.splitContainerDetails.TabIndex = 0;
            // 
            // listViewCDCDescList
            // 
            this.listViewCDCDescList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            listViewGroup1.Header = "CDC Interfaces";
            listViewGroup1.Name = "groupInterface";
            listViewGroup2.Header = "Functional Descriptors";
            listViewGroup2.Name = "groupCommunications";
            listViewGroup3.Header = "Endpoint Descriptors";
            listViewGroup3.Name = "groupEndpoint";
            this.listViewCDCDescList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.listViewCDCDescList.LargeImageList = this.imageList;
            this.listViewCDCDescList.Location = new System.Drawing.Point(2, 28);
            this.listViewCDCDescList.MultiSelect = false;
            this.listViewCDCDescList.Name = "listViewCDCDescList";
            this.listViewCDCDescList.Size = new System.Drawing.Size(347, 120);
            this.listViewCDCDescList.TabIndex = 11;
            this.listViewCDCDescList.UseCompatibleStateImageBehavior = false;
            this.listViewCDCDescList.SelectedIndexChanged += new System.EventHandler(this.listViewCDCDescList_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::USBFS_v2_10.Properties.Resources.imcomponentadd;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label1.Location = new System.Drawing.Point(28, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "CDC Descriptors List";
            // 
            // labelItemProperties
            // 
            this.labelItemProperties.AutoSize = true;
            this.labelItemProperties.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelItemProperties.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.labelItemProperties.Location = new System.Drawing.Point(2, 3);
            this.labelItemProperties.Name = "labelItemProperties";
            this.labelItemProperties.Size = new System.Drawing.Size(148, 16);
            this.labelItemProperties.TabIndex = 9;
            this.labelItemProperties.Text = "Descriptor Properties";
            // 
            // panelDetails
            // 
            this.panelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetails.BackColor = System.Drawing.Color.Transparent;
            this.panelDetails.Controls.Add(this.panelAddbtn);
            this.panelDetails.Controls.Add(this.propertyGridCDC);
            this.panelDetails.Location = new System.Drawing.Point(2, 22);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(347, 176);
            this.panelDetails.TabIndex = 1;
            // 
            // panelAddbtn
            // 
            this.panelAddbtn.BackColor = System.Drawing.SystemColors.Control;
            this.panelAddbtn.Controls.Add(this.buttonApply);
            this.panelAddbtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAddbtn.Location = new System.Drawing.Point(0, 145);
            this.panelAddbtn.Name = "panelAddbtn";
            this.panelAddbtn.Size = new System.Drawing.Size(347, 31);
            this.panelAddbtn.TabIndex = 1;
            this.panelAddbtn.Resize += new System.EventHandler(this.panelAddbtn_Resize);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(124, 3);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(99, 25);
            this.buttonApply.TabIndex = 8;
            this.buttonApply.Text = "Add";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // propertyGridCDC
            // 
            this.propertyGridCDC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridCDC.HelpVisible = false;
            this.propertyGridCDC.Location = new System.Drawing.Point(0, 0);
            this.propertyGridCDC.Name = "propertyGridCDC";
            this.propertyGridCDC.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridCDC.Size = new System.Drawing.Size(347, 139);
            this.propertyGridCDC.TabIndex = 0;
            this.propertyGridCDC.ToolbarVisible = false;
            this.propertyGridCDC.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridCDC_PropertyValueChanged);
            // 
            // CyCDCDescriptorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerReport);
            this.Name = "CyCDCDescriptorPage";
            this.Size = new System.Drawing.Size(600, 363);
            this.VisibleChanged += new System.EventHandler(this.CyCDCDescriptorPage_VisibleChanged);
            this.splitContainerReport.Panel1.ResumeLayout(false);
            this.splitContainerReport.Panel1.PerformLayout();
            this.splitContainerReport.Panel2.ResumeLayout(false);
            this.splitContainerReport.ResumeLayout(false);
            this.panelCb.ResumeLayout(false);
            this.panelCb.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainerDetails.Panel1.ResumeLayout(false);
            this.splitContainerDetails.Panel1.PerformLayout();
            this.splitContainerDetails.Panel2.ResumeLayout(false);
            this.splitContainerDetails.Panel2.PerformLayout();
            this.splitContainerDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelDetails.ResumeLayout(false);
            this.panelAddbtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerReport;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainerDetails;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TreeView treeViewCDC;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.Label labelItemProperties;
        private System.Windows.Forms.ToolTip toolTipList;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PropertyGrid propertyGridCDC;
        private System.Windows.Forms.ListView listViewCDCDescList;
        private System.Windows.Forms.Panel panelAddbtn;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.ToolStripButton toolStripButtonImport;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Panel panelCb;
        private System.Windows.Forms.CheckBox checkBoxEnableCDCApi;
    }
}
