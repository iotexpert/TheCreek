namespace USBFS_v1_50
{
    partial class CyDeviceDescriptorPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyDeviceDescriptorPage));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewDevice = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelDetails = new System.Windows.Forms.Panel();
            this.toolStripOperations = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorRemove = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButtonAddAudio = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparatorTemplates = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButtonImport = new System.Windows.Forms.ToolStripSplitButton();
            this.importOneDescriptorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFullConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButtonSave = new System.Windows.Forms.ToolStripSplitButton();
            this.saveOneDescriptorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFullConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelDetails.SuspendLayout();
            this.toolStripOperations.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewDevice);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelDetails);
            this.splitContainer1.Size = new System.Drawing.Size(431, 413);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeViewDevice
            // 
            this.treeViewDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDevice.HideSelection = false;
            this.treeViewDevice.ImageIndex = 0;
            this.treeViewDevice.ImageList = this.imageList;
            this.treeViewDevice.Indent = 19;
            this.treeViewDevice.Location = new System.Drawing.Point(0, 0);
            this.treeViewDevice.Name = "treeViewDevice";
            this.treeViewDevice.SelectedImageIndex = 0;
            this.treeViewDevice.Size = new System.Drawing.Size(188, 413);
            this.treeViewDevice.TabIndex = 0;
            this.treeViewDevice.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDevice_AfterSelect);
            this.treeViewDevice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewDevice_KeyDown);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "USB Drive.png");
            this.imageList.Images.SetKeyName(2, "Configuration.png");
            this.imageList.Images.SetKeyName(3, "Audio.png");
            // 
            // panelDetails
            // 
            this.panelDetails.Controls.Add(this.toolStripOperations);
            this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetails.Location = new System.Drawing.Point(0, 0);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(239, 413);
            this.panelDetails.TabIndex = 0;
            this.panelDetails.Visible = false;
            // 
            // toolStripOperations
            // 
            this.toolStripOperations.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripOperations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRemove,
            this.toolStripSeparatorRemove,
            this.toolStripButtonAdd,
            this.toolStripSplitButtonAddAudio,
            this.toolStripSeparatorTemplates,
            this.toolStripSplitButtonImport,
            this.toolStripSplitButtonSave});
            this.toolStripOperations.Location = new System.Drawing.Point(0, 0);
            this.toolStripOperations.Name = "toolStripOperations";
            this.toolStripOperations.Size = new System.Drawing.Size(239, 25);
            this.toolStripOperations.TabIndex = 0;
            this.toolStripOperations.Text = "toolStrip1";
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemove.Image = global::USBFS_v1_50.Properties.Resources.imdelete;
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRemove.Text = "Remove";
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
            // 
            // toolStripSeparatorRemove
            // 
            this.toolStripSeparatorRemove.Name = "toolStripSeparatorRemove";
            this.toolStripSeparatorRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = global::USBFS_v1_50.Properties.Resources.imadd;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(46, 22);
            this.toolStripButtonAdd.Text = "Add";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripSplitButtonAddAudio
            // 
            this.toolStripSplitButtonAddAudio.Image = global::USBFS_v1_50.Properties.Resources.imadd;
            this.toolStripSplitButtonAddAudio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonAddAudio.Name = "toolStripSplitButtonAddAudio";
            this.toolStripSplitButtonAddAudio.Size = new System.Drawing.Size(136, 22);
            this.toolStripSplitButtonAddAudio.Text = "Add Audio Interface";
            this.toolStripSplitButtonAddAudio.ButtonClick += new System.EventHandler(this.toolStripSplitButton_ButtonClick);
            this.toolStripSplitButtonAddAudio.DropDownOpening += new System.EventHandler(this.toolStripSplitButtonAddAudio_DropDownOpening);
            // 
            // toolStripSeparatorTemplates
            // 
            this.toolStripSeparatorTemplates.Name = "toolStripSeparatorTemplates";
            this.toolStripSeparatorTemplates.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButtonImport
            // 
            this.toolStripSplitButtonImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importOneDescriptorToolStripMenuItem,
            this.importFullConfigurationToolStripMenuItem});
            this.toolStripSplitButtonImport.Image = global::USBFS_v1_50.Properties.Resources.imimport;
            this.toolStripSplitButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonImport.Name = "toolStripSplitButtonImport";
            this.toolStripSplitButtonImport.Size = new System.Drawing.Size(32, 20);
            this.toolStripSplitButtonImport.Text = "toolStripSplitButton1";
            this.toolStripSplitButtonImport.ToolTipText = "Import";
            this.toolStripSplitButtonImport.ButtonClick += new System.EventHandler(this.toolStripSplitButton_ButtonClick);
            this.toolStripSplitButtonImport.DropDownOpening += new System.EventHandler(this.toolStripSplitButton_DropDownOpening);
            // 
            // importOneDescriptorToolStripMenuItem
            // 
            this.importOneDescriptorToolStripMenuItem.Name = "importOneDescriptorToolStripMenuItem";
            this.importOneDescriptorToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.importOneDescriptorToolStripMenuItem.Text = "Import Current Descriptor";
            this.importOneDescriptorToolStripMenuItem.Click += new System.EventHandler(this.importOneDescriptorToolStripMenuItem_Click);
            // 
            // importFullConfigurationToolStripMenuItem
            // 
            this.importFullConfigurationToolStripMenuItem.Name = "importFullConfigurationToolStripMenuItem";
            this.importFullConfigurationToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.importFullConfigurationToolStripMenuItem.Text = "Import Descriptor Root";
            this.importFullConfigurationToolStripMenuItem.Click += new System.EventHandler(this.importFullConfigurationToolStripMenuItem_Click);
            // 
            // toolStripSplitButtonSave
            // 
            this.toolStripSplitButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveOneDescriptorToolStripMenuItem,
            this.saveFullConfigToolStripMenuItem});
            this.toolStripSplitButtonSave.Image = global::USBFS_v1_50.Properties.Resources.imsave;
            this.toolStripSplitButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonSave.Name = "toolStripSplitButtonSave";
            this.toolStripSplitButtonSave.Size = new System.Drawing.Size(32, 20);
            this.toolStripSplitButtonSave.Text = "toolStripSplitButton1";
            this.toolStripSplitButtonSave.ToolTipText = "Save";
            this.toolStripSplitButtonSave.ButtonClick += new System.EventHandler(this.toolStripSplitButton_ButtonClick);
            this.toolStripSplitButtonSave.DropDownOpening += new System.EventHandler(this.toolStripSplitButton_DropDownOpening);
            // 
            // saveOneDescriptorToolStripMenuItem
            // 
            this.saveOneDescriptorToolStripMenuItem.Name = "saveOneDescriptorToolStripMenuItem";
            this.saveOneDescriptorToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.saveOneDescriptorToolStripMenuItem.Text = "Save Current Descriptor";
            this.saveOneDescriptorToolStripMenuItem.Click += new System.EventHandler(this.saveOneDescriptorToolStripMenuItem_Click);
            // 
            // saveFullConfigToolStripMenuItem
            // 
            this.saveFullConfigToolStripMenuItem.Name = "saveFullConfigToolStripMenuItem";
            this.saveFullConfigToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.saveFullConfigToolStripMenuItem.Text = "Save Descriptor Root";
            this.saveFullConfigToolStripMenuItem.Click += new System.EventHandler(this.saveFullConfigToolStripMenuItem_Click);
            // 
            // CyDeviceDescriptorPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CyDeviceDescriptorPage";
            this.Size = new System.Drawing.Size(431, 413);
            this.Load += new System.EventHandler(this.CyDeviceDescriptor_Load);
            this.VisibleChanged += new System.EventHandler(this.CyDeviceDescriptor_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelDetails.ResumeLayout(false);
            this.panelDetails.PerformLayout();
            this.toolStripOperations.ResumeLayout(false);
            this.toolStripOperations.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewDevice;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.ToolStrip toolStripOperations;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRemove;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorTemplates;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonSave;
        private System.Windows.Forms.ToolStripMenuItem saveOneDescriptorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveFullConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonImport;
        private System.Windows.Forms.ToolStripMenuItem importOneDescriptorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFullConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonAddAudio;
    }
}
