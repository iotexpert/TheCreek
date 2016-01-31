namespace  CapSense_v0_5.ContentControls
{
    partial class cntSSProperties
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
            this.SSCSDProps = new ContentControls.cntSSCSDProps();
            this.SSCSAProps = new ContentControls.cntSSCSAProps();
            this.SuspendLayout();
            // 
            // SSCSDProps
            // 
            this.SSCSDProps.AutoScroll = true;
            this.SSCSDProps.Location = new System.Drawing.Point(3, 90);
            this.SSCSDProps.Name = "SSCSDProps";
            this.SSCSDProps.Size = new System.Drawing.Size(368, 71);
            this.SSCSDProps.TabIndex = 1;
            this.SSCSDProps.Visible = false;
            // 
            // SSCSAProps
            // 
            this.SSCSAProps.AutoScroll = true;
            this.SSCSAProps.Location = new System.Drawing.Point(0, 3);
            this.SSCSAProps.Name = "SSCSAProps";
            this.SSCSAProps.Size = new System.Drawing.Size(353, 70);
            this.SSCSAProps.TabIndex = 0;
            this.SSCSAProps.Visible = false;
            // 
            // cntSSProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SSCSDProps);
            this.Controls.Add(this.SSCSAProps);
            this.Name = "cntSSProperties";
            this.Size = new System.Drawing.Size(421, 221);
            this.ResumeLayout(false);

        }

        #endregion

        private cntSSCSAProps SSCSAProps;
        private cntSSCSDProps SSCSDProps;
    }
}
