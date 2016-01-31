namespace  CapSense_v1_20
{
    partial class CyGenCSAProperties
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.cbReference = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lSEC = new System.Windows.Forms.Label();
            this.tbSEC = new System.Windows.Forms.TextBox();
            this.panelGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).BeginInit();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGraph
            // 
            this.panelGraph.BackColor = System.Drawing.Color.White;
            this.panelGraph.Controls.Add(this.pbGraph);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(0, 0);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Padding = new System.Windows.Forms.Padding(2);
            this.panelGraph.Size = new System.Drawing.Size(208, 193);
            this.panelGraph.TabIndex = 50;
            // 
            // pbGraph
            // 
            this.pbGraph.BackColor = System.Drawing.Color.White;
            this.pbGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraph.Location = new System.Drawing.Point(2, 2);
            this.pbGraph.Margin = new System.Windows.Forms.Padding(0);
            this.pbGraph.Name = "pbGraph";
            this.pbGraph.Size = new System.Drawing.Size(204, 189);
            this.pbGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGraph.TabIndex = 48;
            this.pbGraph.TabStop = false;
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.Controls.Add(this.cbReference);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Controls.Add(this.lSEC);
            this.panelMain.Controls.Add(this.tbSEC);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelMain.Location = new System.Drawing.Point(0, 193);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(208, 52);
            this.panelMain.TabIndex = 51;
            // 
            // cbReference
            // 
            this.cbReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReference.FormattingEnabled = true;
            this.cbReference.Location = new System.Drawing.Point(100, 28);
            this.cbReference.Name = "cbReference";
            this.cbReference.Size = new System.Drawing.Size(99, 21);
            this.cbReference.TabIndex = 5;
            this.cbReference.Visible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Reference ";
            this.label1.Visible = false;
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
            // CyGenCSAProperties
            // 
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.panelMain);
            this.Name = "CyGenCSAProperties";
            this.Size = new System.Drawing.Size(208, 245);
            this.panelGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.PictureBox pbGraph;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lSEC;
        private System.Windows.Forms.TextBox tbSEC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbReference;

    }
}
