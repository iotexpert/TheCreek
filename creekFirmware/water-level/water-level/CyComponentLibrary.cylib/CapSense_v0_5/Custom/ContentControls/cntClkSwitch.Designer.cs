namespace  CapSense_v0_5
{
    partial class cntClkSwitch
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
            this.cbCPS_CLK = new System.Windows.Forms.ComboBox();
            this.cbPrescaler = new System.Windows.Forms.ComboBox();
            this.lCPS_CLK = new System.Windows.Forms.Label();
            this.Labe1 = new System.Windows.Forms.Label();
            this.pbGraph = new System.Windows.Forms.PictureBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.cbCPS_CLK);
            this.panelTop.Controls.Add(this.cbPrescaler);
            this.panelTop.Controls.Add(this.lCPS_CLK);
            this.panelTop.Controls.Add(this.Labe1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(234, 56);
            this.panelTop.TabIndex = 0;
            // 
            // cbCPS_CLK
            // 
            this.cbCPS_CLK.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCPS_CLK.FormattingEnabled = true;
            this.cbCPS_CLK.Location = new System.Drawing.Point(70, 31);
            this.cbCPS_CLK.Name = "cbCPS_CLK";
            this.cbCPS_CLK.Size = new System.Drawing.Size(161, 21);
            this.cbCPS_CLK.TabIndex = 3;
            this.cbCPS_CLK.SelectedIndexChanged += new System.EventHandler(this.cb_SelectedIndexChanged);
            // 
            // cbPrescaler
            // 
            this.cbPrescaler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrescaler.FormattingEnabled = true;
            this.cbPrescaler.Location = new System.Drawing.Point(70, 4);
            this.cbPrescaler.Name = "cbPrescaler";
            this.cbPrescaler.Size = new System.Drawing.Size(161, 21);
            this.cbPrescaler.TabIndex = 2;
            this.cbPrescaler.SelectedIndexChanged += new System.EventHandler(this.cbPrescaler_SelectedIndexChanged);
            // 
            // lCPS_CLK
            // 
            this.lCPS_CLK.AutoSize = true;
            this.lCPS_CLK.Location = new System.Drawing.Point(4, 31);
            this.lCPS_CLK.Name = "lCPS_CLK";
            this.lCPS_CLK.Size = new System.Drawing.Size(54, 13);
            this.lCPS_CLK.TabIndex = 1;
            this.lCPS_CLK.Text = "CPS_CLK";
            // 
            // Labe1
            // 
            this.Labe1.AutoSize = true;
            this.Labe1.Location = new System.Drawing.Point(4, 4);
            this.Labe1.Name = "Labe1";
            this.Labe1.Size = new System.Drawing.Size(51, 13);
            this.Labe1.TabIndex = 0;
            this.Labe1.Text = "Prescaler";
            // 
            // pbGraph
            // 
            this.pbGraph.BackColor = System.Drawing.Color.White;
            this.pbGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraph.Location = new System.Drawing.Point(0, 56);
            this.pbGraph.Name = "pbGraph";
            this.pbGraph.Size = new System.Drawing.Size(234, 172);
            this.pbGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbGraph.TabIndex = 1;
            this.pbGraph.TabStop = false;
            // 
            // cntClkSwitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pbGraph);
            this.Controls.Add(this.panelTop);
            this.Name = "cntClkSwitch";
            this.Size = new System.Drawing.Size(234, 228);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox pbGraph;
        private System.Windows.Forms.ComboBox cbCPS_CLK;
        private System.Windows.Forms.ComboBox cbPrescaler;
        private System.Windows.Forms.Label lCPS_CLK;
        private System.Windows.Forms.Label Labe1;
    }
}
