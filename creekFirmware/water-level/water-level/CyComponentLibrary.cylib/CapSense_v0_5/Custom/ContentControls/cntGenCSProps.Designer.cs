namespace  CapSense_v0_5
{
    partial class cntGenCSProps
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lCSMSide = new System.Windows.Forms.Label();
            this.cbCSMSide = new System.Windows.Forms.ComboBox();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lCSMSide);
            this.panelTop.Controls.Add(this.cbCSMSide);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(238, 28);
            this.panelTop.TabIndex = 3;
            // 
            // lCSMSide
            // 
            this.lCSMSide.AutoSize = true;
            this.lCSMSide.Location = new System.Drawing.Point(3, 6);
            this.lCSMSide.Name = "lCSMSide";
            this.lCSMSide.Size = new System.Drawing.Size(95, 13);
            this.lCSMSide.TabIndex = 1;
            this.lCSMSide.Text = "CapSense Method";
            // 
            // cbCSMSide
            // 
            this.cbCSMSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCSMSide.FormattingEnabled = true;
            this.cbCSMSide.Location = new System.Drawing.Point(104, 3);
            this.cbCSMSide.Name = "cbCSMSide";
            this.cbCSMSide.Size = new System.Drawing.Size(87, 21);
            this.cbCSMSide.TabIndex = 0;
            this.cbCSMSide.SelectedIndexChanged += new System.EventHandler(this.cbCSMSide_SelectedIndexChanged);
            // 
            // panelContainer
            // 
            this.panelContainer.Controls.Add(this.panelTop);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(238, 297);
            this.panelContainer.TabIndex = 5;
            // 
            // cntGenCSProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContainer);
            this.Name = "cntGenCSProps";
            this.Size = new System.Drawing.Size(238, 297);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lCSMSide;
        public System.Windows.Forms.ComboBox cbCSMSide;
        private System.Windows.Forms.Panel panelContainer;
    }
}
