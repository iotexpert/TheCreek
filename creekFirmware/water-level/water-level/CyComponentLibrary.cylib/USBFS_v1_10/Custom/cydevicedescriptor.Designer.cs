namespace USBFS_v1_10
{
    partial class CyDeviceDescriptor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyDeviceDescriptor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewDevice = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelDetails = new System.Windows.Forms.Panel();
            this.toolStripOperations = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorRemove = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddClass = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddCDCClass = new System.Windows.Forms.ToolStripButton();
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
            this.splitContainer1.SplitterDistance = 190;
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
            this.treeViewDevice.Size = new System.Drawing.Size(190, 413);
            this.treeViewDevice.TabIndex = 0;
            this.treeViewDevice.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDevice_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "USB Drive.png");
            this.imageList.Images.SetKeyName(2, "Configuration.png");
            // 
            // panelDetails
            // 
            this.panelDetails.Controls.Add(this.toolStripOperations);
            this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetails.Location = new System.Drawing.Point(0, 0);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(237, 413);
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
            this.toolStripButtonAddClass,
            this.toolStripButtonAddCDCClass});
            this.toolStripOperations.Location = new System.Drawing.Point(0, 0);
            this.toolStripOperations.Name = "toolStripOperations";
            this.toolStripOperations.Size = new System.Drawing.Size(237, 25);
            this.toolStripOperations.TabIndex = 0;
            this.toolStripOperations.Text = "toolStrip1";
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemove.Image = global::USBFS_v1_10.Properties.Resources.Symbol_Delete_2;
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
            this.toolStripButtonAdd.Image = global::USBFS_v1_10.Properties.Resources.Symbol_Add_2;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(46, 22);
            this.toolStripButtonAdd.Text = "Add";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
            // 
            // toolStripButtonAddClass
            // 
            this.toolStripButtonAddClass.Image = global::USBFS_v1_10.Properties.Resources.Symbol_Add_2;
            this.toolStripButtonAddClass.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddClass.Name = "toolStripButtonAddClass";
            this.toolStripButtonAddClass.Size = new System.Drawing.Size(128, 22);
            this.toolStripButtonAddClass.Text = "Add Audio Descriptor";
            this.toolStripButtonAddClass.Visible = false;
            this.toolStripButtonAddClass.Click += new System.EventHandler(this.toolStripButtonAddClass_Click);
            // 
            // toolStripButtonAddCDCClass
            // 
            this.toolStripButtonAddCDCClass.Image = global::USBFS_v1_10.Properties.Resources.Symbol_Add_2;
            this.toolStripButtonAddCDCClass.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddCDCClass.Name = "toolStripButtonAddCDCClass";
            this.toolStripButtonAddCDCClass.Size = new System.Drawing.Size(122, 22);
            this.toolStripButtonAddCDCClass.Text = "Add CDC Descriptor";
            this.toolStripButtonAddCDCClass.Visible = false;
            this.toolStripButtonAddCDCClass.Click += new System.EventHandler(this.toolStripButtonAddCDCClass_Click);
            // 
            // CyDeviceDescriptor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CyDeviceDescriptor";
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
        private System.Windows.Forms.ToolStripButton toolStripButtonAddClass;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddCDCClass;
    }
}
