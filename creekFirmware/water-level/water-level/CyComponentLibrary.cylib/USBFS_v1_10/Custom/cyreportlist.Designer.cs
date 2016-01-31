namespace USBFS_v1_10
{
    partial class CyReportList
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
            this.listBoxValues = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxValues
            // 
            this.listBoxValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxValues.FormattingEnabled = true;
            this.listBoxValues.Location = new System.Drawing.Point(0, 0);
            this.listBoxValues.Name = "listBoxValues";
            this.listBoxValues.Size = new System.Drawing.Size(159, 173);
            this.listBoxValues.TabIndex = 0;
            this.listBoxValues.Validated += new System.EventHandler(this.listBoxValues_Validated);
            this.listBoxValues.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listBoxValues_Format);
            // 
            // CyReportList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxValues);
            this.Name = "CyReportList";
            this.Size = new System.Drawing.Size(159, 178);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxValues;
    }
}
