namespace SMBusSlave_v1_0
{
    partial class CyToolStrip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyToolStrip));
            this.toolStripGeneral = new System.Windows.Forms.ToolStrip();
            this.tsbLoad = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripExport = new System.Windows.Forms.ToolStrip();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbExportAll = new System.Windows.Forms.ToolStripButton();
            this.tsbImportAll = new System.Windows.Forms.ToolStripButton();
            this.tssPMBus = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHideUncheckedCommands = new System.Windows.Forms.ToolStripButton();
            this.toolStripGeneral.SuspendLayout();
            this.toolStripExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripGeneral
            // 
            this.toolStripGeneral.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripGeneral.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoad,
            this.tsbSave});
            this.toolStripGeneral.Location = new System.Drawing.Point(0, 0);
            this.toolStripGeneral.Name = "toolStripGeneral";
            this.toolStripGeneral.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripGeneral.Size = new System.Drawing.Size(661, 25);
            this.toolStripGeneral.TabIndex = 2;
            this.toolStripGeneral.Text = "toolStrip1";
            // 
            // tsbLoad
            // 
            this.tsbLoad.Image = global::SMBusSlave_v1_0.Resources.Load;
            this.tsbLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoad.Name = "tsbLoad";
            this.tsbLoad.Size = new System.Drawing.Size(116, 22);
            this.tsbLoad.Text = "Load configuration";
            this.tsbLoad.ToolTipText = "Load (Ctrl + O)";
            // 
            // tsbSave
            // 
            this.tsbSave.Image = global::SMBusSlave_v1_0.Resources.Save;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(117, 22);
            this.tsbSave.Text = "Save configuration";
            this.tsbSave.ToolTipText = "Save (Ctrl + S)";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // toolStripExport
            // 
            this.toolStripExport.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCopy,
            this.tsbPaste,
            this.toolStripSeparator1,
            this.tsbImport,
            this.tsbExport,
            this.tsbExportAll,
            this.tsbImportAll,
            this.tssPMBus,
            this.tsbHideUncheckedCommands});
            this.toolStripExport.Location = new System.Drawing.Point(0, 25);
            this.toolStripExport.Name = "toolStripExport";
            this.toolStripExport.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripExport.Size = new System.Drawing.Size(661, 25);
            this.toolStripExport.TabIndex = 3;
            this.toolStripExport.Text = "toolStrip2";
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Image = global::SMBusSlave_v1_0.Resources.Copy;
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "Copy rows (Ctrl + C)";
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Image = global::SMBusSlave_v1_0.Resources.Paste;
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "Paste rows (Ctrl + V)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbImport
            // 
            this.tsbImport.Image = global::SMBusSlave_v1_0.Resources.Import;
            this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Size = new System.Drawing.Size(86, 22);
            this.tsbImport.Text = "Import table";
            this.tsbImport.ToolTipText = "Import table (Ctrl + M)";
            // 
            // tsbExport
            // 
            this.tsbExport.Image = global::SMBusSlave_v1_0.Resources.Export;
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(86, 22);
            this.tsbExport.Text = "Export table";
            this.tsbExport.ToolTipText = "Export table (Ctrl + R)";
            // 
            // tsbExportAll
            // 
            this.tsbExportAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbExportAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportAll.Name = "tsbExportAll";
            this.tsbExportAll.Size = new System.Drawing.Size(56, 22);
            this.tsbExportAll.Text = "Export all";
            this.tsbExportAll.ToolTipText = "Export all (Ctrl + Alt + R)";
            // 
            // tsbImportAll
            // 
            this.tsbImportAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbImportAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportAll.Name = "tsbImportAll";
            this.tsbImportAll.Size = new System.Drawing.Size(56, 22);
            this.tsbImportAll.Text = "Import all";
            this.tsbImportAll.ToolTipText = "Import all (Ctrl + Alt + M)";
            // 
            // tssPMBus
            // 
            this.tssPMBus.Name = "tssPMBus";
            this.tssPMBus.Size = new System.Drawing.Size(6, 25);
            this.tssPMBus.Visible = false;
            // 
            // tsbHideUncheckedCommands
            // 
            this.tsbHideUncheckedCommands.CheckOnClick = true;
            this.tsbHideUncheckedCommands.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;            
            this.tsbHideUncheckedCommands.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHideUncheckedCommands.Name = "tsbHideUncheckedCommands";
            this.tsbHideUncheckedCommands.Size = new System.Drawing.Size(127, 22);
            this.tsbHideUncheckedCommands.Text = "Hide disabled commands";
            this.tsbHideUncheckedCommands.Visible = false;
            // 
            // CyToolStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripExport);
            this.Controls.Add(this.toolStripGeneral);
            this.Name = "CyToolStrip";
            this.Size = new System.Drawing.Size(661, 59);
            this.toolStripGeneral.ResumeLayout(false);
            this.toolStripGeneral.PerformLayout();
            this.toolStripExport.ResumeLayout(false);
            this.toolStripExport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripGeneral;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStrip toolStripExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripButton tsbLoad;
        public System.Windows.Forms.ToolStripButton tsbSave;
        public System.Windows.Forms.ToolStripButton tsbCopy;
        public System.Windows.Forms.ToolStripButton tsbPaste;
        public System.Windows.Forms.ToolStripButton tsbImport;
        public System.Windows.Forms.ToolStripButton tsbExport;
        public System.Windows.Forms.ToolStripButton tsbExportAll;
        public System.Windows.Forms.ToolStripButton tsbImportAll;
        private System.Windows.Forms.ToolStripSeparator tssPMBus;
        public System.Windows.Forms.ToolStripButton tsbHideUncheckedCommands;
    }
}
