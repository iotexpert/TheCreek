namespace USBFS_v2_40
{
    partial class CyStringDescriptorPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyStringDescriptorPage));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewStrings = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxShowSerial = new System.Windows.Forms.CheckBox();
            this.checkBoxShowEE = new System.Windows.Forms.CheckBox();
            this.toolStripOperations = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStripOperations.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewStrings);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.toolStripOperations);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelDetails);
            this.splitContainer1.Size = new System.Drawing.Size(600, 379);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // treeViewStrings
            // 
            this.treeViewStrings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewStrings.HideSelection = false;
            this.treeViewStrings.ImageIndex = 0;
            this.treeViewStrings.ImageList = this.imageList;
            this.treeViewStrings.Indent = 19;
            this.treeViewStrings.Location = new System.Drawing.Point(0, 25);
            this.treeViewStrings.Name = "treeViewStrings";
            this.treeViewStrings.SelectedImageIndex = 0;
            this.treeViewStrings.Size = new System.Drawing.Size(236, 299);
            this.treeViewStrings.TabIndex = 0;
            this.treeViewStrings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewStrings_AfterSelect);
            this.treeViewStrings.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewStrings_KeyDown);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "Format Font Smaller.png");
            this.imageList.Images.SetKeyName(2, "IP_Address.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxShowSerial);
            this.panel1.Controls.Add(this.checkBoxShowEE);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 324);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 51);
            this.panel1.TabIndex = 3;
            // 
            // checkBoxShowSerial
            // 
            this.checkBoxShowSerial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowSerial.AutoSize = true;
            this.checkBoxShowSerial.Location = new System.Drawing.Point(16, 6);
            this.checkBoxShowSerial.Name = "checkBoxShowSerial";
            this.checkBoxShowSerial.Size = new System.Drawing.Size(160, 17);
            this.checkBoxShowSerial.TabIndex = 1;
            this.checkBoxShowSerial.Text = "Include Serial Number String";
            this.checkBoxShowSerial.UseVisualStyleBackColor = true;
            this.checkBoxShowSerial.CheckedChanged += new System.EventHandler(this.checkBoxShowSerial_CheckedChanged);
            // 
            // checkBoxShowEE
            // 
            this.checkBoxShowEE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowEE.AutoSize = true;
            this.checkBoxShowEE.Location = new System.Drawing.Point(16, 29);
            this.checkBoxShowEE.Name = "checkBoxShowEE";
            this.checkBoxShowEE.Size = new System.Drawing.Size(179, 17);
            this.checkBoxShowEE.TabIndex = 2;
            this.checkBoxShowEE.Text = "Include MS OS String Descriptor";
            this.checkBoxShowEE.UseVisualStyleBackColor = true;
            this.checkBoxShowEE.CheckedChanged += new System.EventHandler(this.checkBoxShowEE_CheckedChanged);
            // 
            // toolStripOperations
            // 
            this.toolStripOperations.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripOperations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonRemove});
            this.toolStripOperations.Location = new System.Drawing.Point(0, 0);
            this.toolStripOperations.Name = "toolStripOperations";
            this.toolStripOperations.Size = new System.Drawing.Size(236, 25);
            this.toolStripOperations.TabIndex = 0;
            this.toolStripOperations.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = global::USBFS_v2_40.Properties.Resources.imadd;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(83, 22);
            this.toolStripButtonAdd.Text = "Add String";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemove.Enabled = false;
            this.toolStripButtonRemove.Image = global::USBFS_v2_40.Properties.Resources.imdelete;
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemove.Text = "Delete";
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
            // 
            // panelDetails
            // 
            this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetails.Location = new System.Drawing.Point(0, 0);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(352, 375);
            this.panelDetails.TabIndex = 1;
            this.panelDetails.Visible = false;
            // 
            // CyStringDescriptorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CyStringDescriptorPage";
            this.Size = new System.Drawing.Size(600, 379);
            this.VisibleChanged += new System.EventHandler(this.CyStringDescriptor_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStripOperations.ResumeLayout(false);
            this.toolStripOperations.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewStrings;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.ToolStrip toolStripOperations;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.CheckBox checkBoxShowEE;
        private System.Windows.Forms.CheckBox checkBoxShowSerial;
        private System.Windows.Forms.Panel panel1;

    }
}
