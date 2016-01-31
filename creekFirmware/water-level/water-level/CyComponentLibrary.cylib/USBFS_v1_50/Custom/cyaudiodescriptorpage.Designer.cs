namespace USBFS_v1_50
{
    partial class CyAudioDescriptorPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyAudioDescriptorPage));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Audio Interfaces", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Audio Control Descriptors", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Audio Streaming Descriptors", System.Windows.Forms.HorizontalAlignment.Left);
            this.splitContainerReport = new System.Windows.Forms.SplitContainer();
            this.treeViewAudio = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
            this.listViewAudioDescList = new System.Windows.Forms.ListView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelItemProperties = new System.Windows.Forms.Label();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.panelAddbtn = new System.Windows.Forms.Panel();
            this.buttonApply = new System.Windows.Forms.Button();
            this.propertyGridAudio = new System.Windows.Forms.PropertyGrid();
            this.toolTipList = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerReport.Panel1.SuspendLayout();
            this.splitContainerReport.Panel2.SuspendLayout();
            this.splitContainerReport.SuspendLayout();
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
            this.splitContainerReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerReport.Location = new System.Drawing.Point(0, 0);
            this.splitContainerReport.Name = "splitContainerReport";
            // 
            // splitContainerReport.Panel1
            // 
            this.splitContainerReport.Panel1.Controls.Add(this.treeViewAudio);
            this.splitContainerReport.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainerReport.Panel2
            // 
            this.splitContainerReport.Panel2.Controls.Add(this.splitContainerDetails);
            this.splitContainerReport.Size = new System.Drawing.Size(601, 363);
            this.splitContainerReport.SplitterDistance = 303;
            this.splitContainerReport.TabIndex = 0;
            // 
            // treeViewAudio
            // 
            this.treeViewAudio.AllowDrop = true;
            this.treeViewAudio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewAudio.HideSelection = false;
            this.treeViewAudio.ImageIndex = 0;
            this.treeViewAudio.ImageList = this.imageList;
            this.treeViewAudio.LabelEdit = true;
            this.treeViewAudio.Location = new System.Drawing.Point(0, 25);
            this.treeViewAudio.Name = "treeViewAudio";
            this.treeViewAudio.SelectedImageIndex = 0;
            this.treeViewAudio.Size = new System.Drawing.Size(303, 338);
            this.treeViewAudio.TabIndex = 2;
            this.treeViewAudio.DragLeave += new System.EventHandler(this.treeViewReport_DragLeave);
            this.treeViewAudio.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewReport_AfterLabelEdit);
            this.treeViewAudio.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragDrop);
            this.treeViewAudio.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewReport_AfterSelect);
            this.treeViewAudio.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragEnter);
            this.treeViewAudio.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeViewReport_BeforeLabelEdit);
            this.treeViewAudio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewReport_KeyDown);
            this.treeViewAudio.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewReport_ItemDrag);
            this.treeViewAudio.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewReport_DragOver);
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
            this.imageList.Images.SetKeyName(6, "AudioHS.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRemove});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(303, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip";
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemove.Image = global::USBFS_v1_50.Properties.Resources.imdelete;
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
            this.splitContainerDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDetails.Panel1
            // 
            this.splitContainerDetails.Panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainerDetails.Panel1.Controls.Add(this.listViewAudioDescList);
            this.splitContainerDetails.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainerDetails.Panel1.Controls.Add(this.label1);
            // 
            // splitContainerDetails.Panel2
            // 
            this.splitContainerDetails.Panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainerDetails.Panel2.Controls.Add(this.labelItemProperties);
            this.splitContainerDetails.Panel2.Controls.Add(this.panelDetails);
            this.splitContainerDetails.Size = new System.Drawing.Size(294, 363);
            this.splitContainerDetails.SplitterDistance = 155;
            this.splitContainerDetails.TabIndex = 0;
            // 
            // listViewAudioDescList
            // 
            this.listViewAudioDescList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            listViewGroup1.Header = "Audio Interfaces";
            listViewGroup1.Name = "groupInterface";
            listViewGroup2.Header = "Audio Control Descriptors";
            listViewGroup2.Name = "groupAC";
            listViewGroup3.Header = "Audio Streaming Descriptors";
            listViewGroup3.Name = "groupAS";
            this.listViewAudioDescList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.listViewAudioDescList.LargeImageList = this.imageList;
            this.listViewAudioDescList.Location = new System.Drawing.Point(2, 28);
            this.listViewAudioDescList.MultiSelect = false;
            this.listViewAudioDescList.Name = "listViewAudioDescList";
            this.listViewAudioDescList.Size = new System.Drawing.Size(289, 124);
            this.listViewAudioDescList.TabIndex = 11;
            this.listViewAudioDescList.UseCompatibleStateImageBehavior = false;
            this.listViewAudioDescList.SelectedIndexChanged += new System.EventHandler(this.listViewAudioDescList_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::USBFS_v1_50.Properties.Resources.imcomponentadd;
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
            this.label1.Size = new System.Drawing.Size(151, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Audio Descriptors List";
            // 
            // labelItemProperties
            // 
            this.labelItemProperties.AutoSize = true;
            this.labelItemProperties.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelItemProperties.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.labelItemProperties.Location = new System.Drawing.Point(2, 6);
            this.labelItemProperties.Name = "labelItemProperties";
            this.labelItemProperties.Size = new System.Drawing.Size(128, 13);
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
            this.panelDetails.Controls.Add(this.propertyGridAudio);
            this.panelDetails.Location = new System.Drawing.Point(2, 22);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(289, 180);
            this.panelDetails.TabIndex = 1;
            // 
            // panelAddbtn
            // 
            this.panelAddbtn.BackColor = System.Drawing.SystemColors.Control;
            this.panelAddbtn.Controls.Add(this.buttonApply);
            this.panelAddbtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAddbtn.Location = new System.Drawing.Point(0, 149);
            this.panelAddbtn.Name = "panelAddbtn";
            this.panelAddbtn.Size = new System.Drawing.Size(289, 31);
            this.panelAddbtn.TabIndex = 1;
            this.panelAddbtn.Resize += new System.EventHandler(this.panelAddbtn_Resize);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(95, 3);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(99, 25);
            this.buttonApply.TabIndex = 8;
            this.buttonApply.Text = "Add";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // propertyGridAudio
            // 
            this.propertyGridAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridAudio.HelpVisible = false;
            this.propertyGridAudio.Location = new System.Drawing.Point(0, 0);
            this.propertyGridAudio.Name = "propertyGridAudio";
            this.propertyGridAudio.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGridAudio.Size = new System.Drawing.Size(289, 143);
            this.propertyGridAudio.TabIndex = 0;
            this.propertyGridAudio.ToolbarVisible = false;
            this.propertyGridAudio.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridAudio_PropertyValueChanged);
            // 
            // CyAudioDescriptorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerReport);
            this.Name = "CyAudioDescriptorPage";
            this.Size = new System.Drawing.Size(601, 363);
            this.VisibleChanged += new System.EventHandler(this.CyAudioDescriptorPage_VisibleChanged);
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
        private System.Windows.Forms.TreeView treeViewAudio;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.Label labelItemProperties;
        private System.Windows.Forms.ToolTip toolTipList;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PropertyGrid propertyGridAudio;
        private System.Windows.Forms.ListView listViewAudioDescList;
        private System.Windows.Forms.Panel panelAddbtn;
        private System.Windows.Forms.Button buttonApply;
    }
}
