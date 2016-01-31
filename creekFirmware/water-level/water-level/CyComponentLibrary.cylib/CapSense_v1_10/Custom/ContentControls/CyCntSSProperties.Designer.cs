namespace  CapSense_v1_10.ContentControls
{
    partial class CyCntSSProperties
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
            this.lCSLeft = new System.Windows.Forms.Label();
            this.cbCustom = new System.Windows.Forms.CheckBox();
            this.panelConteiner = new System.Windows.Forms.Panel();
            this.SSCSAProps = new CapSense_v1_10.ContentControls.CyCntSSCSAProps();
            this.SSCSDProps = new CapSense_v1_10.ContentControls.CyCntSSCSDProps();
            this.panelConteiner.SuspendLayout();
            this.SuspendLayout();
            // 
            // lCSLeft
            // 
            this.lCSLeft.AutoSize = true;
            this.lCSLeft.Location = new System.Drawing.Point(3, 0);
            this.lCSLeft.Name = "lCSLeft";
            this.lCSLeft.Size = new System.Drawing.Size(123, 13);
            this.lCSLeft.TabIndex = 23;
            this.lCSLeft.Text = "CapSense Method: CSD";
            // 
            // cbCustom
            // 
            this.cbCustom.AutoSize = true;
            this.cbCustom.Location = new System.Drawing.Point(189, -1);
            this.cbCustom.Name = "cbCustom";
            this.cbCustom.Size = new System.Drawing.Size(61, 17);
            this.cbCustom.TabIndex = 22;
            this.cbCustom.Text = "Custom";
            this.cbCustom.UseVisualStyleBackColor = true;
            // 
            // panelConteiner
            // 
            this.panelConteiner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelConteiner.Controls.Add(this.SSCSAProps);
            this.panelConteiner.Controls.Add(this.SSCSDProps);
            this.panelConteiner.Location = new System.Drawing.Point(0, 16);
            this.panelConteiner.Name = "panelConteiner";
            this.panelConteiner.Size = new System.Drawing.Size(341, 125);
            this.panelConteiner.TabIndex = 24;
            // 
            // SSCSAProps
            // 
            this.SSCSAProps.AutoScroll = true;
            this.SSCSAProps.Location = new System.Drawing.Point(4, 3);
            this.SSCSAProps.Name = "SSCSAProps";
            this.SSCSAProps.Size = new System.Drawing.Size(353, 70);
            this.SSCSAProps.TabIndex = 0;
            this.SSCSAProps.Visible = false;
            // 
            // SSCSDProps
            // 
            this.SSCSDProps.AutoScroll = true;
            this.SSCSDProps.Location = new System.Drawing.Point(18, 99);
            this.SSCSDProps.Name = "SSCSDProps";
            this.SSCSDProps.Size = new System.Drawing.Size(368, 71);
            this.SSCSDProps.TabIndex = 1;
            this.SSCSDProps.Visible = false;
            // 
            // CyCntSSProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelConteiner);
            this.Controls.Add(this.lCSLeft);
            this.Controls.Add(this.cbCustom);
            this.Name = "CyCntSSProperties";
            this.Size = new System.Drawing.Size(341, 141);
            this.panelConteiner.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CyCntSSCSAProps SSCSAProps;
        private CyCntSSCSDProps SSCSDProps;
        private System.Windows.Forms.Label lCSLeft;
        private System.Windows.Forms.CheckBox cbCustom;
        private System.Windows.Forms.Panel panelConteiner;
    }
}
