namespace  CapSense_v1_10
{
    partial class CyCntGenCSAProps
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
            this.panelGraph = new System.Windows.Forms.Panel();
            this.pbGraph = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lSEC = new System.Windows.Forms.Label();
            this.tbSEC = new System.Windows.Forms.TextBox();
            this.panelGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGraph
            // 
            this.panelGraph.Controls.Add(this.pbGraph);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(0, 0);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(219, 41);
            this.panelGraph.TabIndex = 50;
            // 
            // pbGraph
            // 
            this.pbGraph.BackColor = System.Drawing.Color.White;
            this.pbGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraph.Location = new System.Drawing.Point(0, 0);
            this.pbGraph.Name = "pbGraph";
            this.pbGraph.Size = new System.Drawing.Size(219, 41);
            this.pbGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGraph.TabIndex = 48;
            this.pbGraph.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.lSEC);
            this.panel1.Controls.Add(this.tbSEC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 109);
            this.panel1.TabIndex = 51;
            // 
            // lSEC
            // 
            this.lSEC.Location = new System.Drawing.Point(1, 3);
            this.lSEC.Name = "lSEC";
            this.lSEC.Size = new System.Drawing.Size(93, 27);
            this.lSEC.TabIndex = 2;
            this.lSEC.Text = "Shield Electrode Count";
            // 
            // tbSEC
            // 
            this.tbSEC.Location = new System.Drawing.Point(100, 7);
            this.tbSEC.Name = "tbSEC";
            this.tbSEC.Size = new System.Drawing.Size(99, 20);
            this.tbSEC.TabIndex = 3;
            this.tbSEC.Text = "0";
            // 
            // CyCntGenCSAProps
            // 
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.panel1);
            this.Name = "CyCntGenCSAProps";
            this.Size = new System.Drawing.Size(219, 150);
            this.panelGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.PictureBox pbGraph;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lSEC;
        private System.Windows.Forms.TextBox tbSEC;

    }
}
