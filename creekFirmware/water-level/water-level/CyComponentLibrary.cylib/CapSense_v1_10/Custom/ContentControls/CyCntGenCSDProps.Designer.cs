namespace  CapSense_v1_10
{
    partial class CyCntGenCSDProps
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
            this.tbSEC = new System.Windows.Forms.TextBox();
            this.lSEC = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPRS = new System.Windows.Forms.ComboBox();
            this.rbRBEnable = new System.Windows.Forms.RadioButton();
            this.rbIDACSinking = new System.Windows.Forms.RadioButton();
            this.rbIDACSourcing = new System.Windows.Forms.RadioButton();
            this.lRb = new System.Windows.Forms.Label();
            this.tbRb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCIS = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbGraph = new System.Windows.Forms.PictureBox();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).BeginInit();
            this.panelGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSEC
            // 
            this.tbSEC.Location = new System.Drawing.Point(100, 78);
            this.tbSEC.Name = "tbSEC";
            this.tbSEC.Size = new System.Drawing.Size(99, 20);
            this.tbSEC.TabIndex = 3;
            this.tbSEC.Text = "0";
            this.tbSEC.Visible = false;
            this.tbSEC.Validating += new System.ComponentModel.CancelEventHandler(this.tbSEC_Validating);
            // 
            // lSEC
            // 
            this.lSEC.Location = new System.Drawing.Point(1, 74);
            this.lSEC.Name = "lSEC";
            this.lSEC.Size = new System.Drawing.Size(93, 27);
            this.lSEC.TabIndex = 2;
            this.lSEC.Text = "Shield Electrode Count";
            this.lSEC.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "PRS";
            // 
            // cbPRS
            // 
            this.cbPRS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPRS.FormattingEnabled = true;
            this.cbPRS.Location = new System.Drawing.Point(100, 55);
            this.cbPRS.Name = "cbPRS";
            this.cbPRS.Size = new System.Drawing.Size(99, 21);
            this.cbPRS.TabIndex = 0;
            this.cbPRS.SelectedIndexChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // rbRBEnable
            // 
            this.rbRBEnable.AutoSize = true;
            this.rbRBEnable.Location = new System.Drawing.Point(3, 36);
            this.rbRBEnable.Name = "rbRBEnable";
            this.rbRBEnable.Size = new System.Drawing.Size(166, 17);
            this.rbRBEnable.TabIndex = 0;
            this.rbRBEnable.Text = "IDAC disable, use external Rb";
            this.rbRBEnable.UseVisualStyleBackColor = true;
            this.rbRBEnable.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // rbIDACSinking
            // 
            this.rbIDACSinking.AutoSize = true;
            this.rbIDACSinking.Location = new System.Drawing.Point(3, 20);
            this.rbIDACSinking.Name = "rbIDACSinking";
            this.rbIDACSinking.Size = new System.Drawing.Size(86, 17);
            this.rbIDACSinking.TabIndex = 0;
            this.rbIDACSinking.Text = "IDAC sinking";
            this.rbIDACSinking.UseVisualStyleBackColor = true;
            this.rbIDACSinking.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // rbIDACSourcing
            // 
            this.rbIDACSourcing.AutoSize = true;
            this.rbIDACSourcing.Checked = true;
            this.rbIDACSourcing.Location = new System.Drawing.Point(3, 4);
            this.rbIDACSourcing.Name = "rbIDACSourcing";
            this.rbIDACSourcing.Size = new System.Drawing.Size(93, 17);
            this.rbIDACSourcing.TabIndex = 0;
            this.rbIDACSourcing.TabStop = true;
            this.rbIDACSourcing.Text = "IDAC sourcing";
            this.rbIDACSourcing.UseVisualStyleBackColor = true;
            this.rbIDACSourcing.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // lRb
            // 
            this.lRb.AutoSize = true;
            this.lRb.Location = new System.Drawing.Point(2, 103);
            this.lRb.Name = "lRb";
            this.lRb.Size = new System.Drawing.Size(52, 13);
            this.lRb.TabIndex = 2;
            this.lRb.Text = "Rb Count";
            this.lRb.Visible = false;
            // 
            // tbRb
            // 
            this.tbRb.Location = new System.Drawing.Point(100, 100);
            this.tbRb.Name = "tbRb";
            this.tbRb.Size = new System.Drawing.Size(99, 20);
            this.tbRb.TabIndex = 3;
            this.tbRb.Text = "1";
            this.tbRb.Visible = false;
            this.tbRb.Validating += new System.ComponentModel.CancelEventHandler(this.tbRb_Validating);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 32);
            this.label4.TabIndex = 46;
            this.label4.Text = "Connect Inactive Sensors";
            this.label4.Visible = false;
            // 
            // cbCIS
            // 
            this.cbCIS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCIS.FormattingEnabled = true;
            this.cbCIS.Items.AddRange(new object[] {
            "Ground",
            "Hi-Z Analog"});
            this.cbCIS.Location = new System.Drawing.Point(100, 122);
            this.cbCIS.Name = "cbCIS";
            this.cbCIS.Size = new System.Drawing.Size(99, 21);
            this.cbCIS.TabIndex = 45;
            this.cbCIS.Visible = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.rbIDACSinking);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbPRS);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lSEC);
            this.panel1.Controls.Add(this.cbCIS);
            this.panel1.Controls.Add(this.lRb);
            this.panel1.Controls.Add(this.rbRBEnable);
            this.panel1.Controls.Add(this.rbIDACSourcing);
            this.panel1.Controls.Add(this.tbRb);
            this.panel1.Controls.Add(this.tbSEC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 124);
            this.panel1.TabIndex = 47;
            // 
            // pbGraph
            // 
            this.pbGraph.BackColor = System.Drawing.Color.White;
            this.pbGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraph.Location = new System.Drawing.Point(0, 0);
            this.pbGraph.Name = "pbGraph";
            this.pbGraph.Size = new System.Drawing.Size(203, 35);
            this.pbGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGraph.TabIndex = 48;
            this.pbGraph.TabStop = false;
            // 
            // panelGraph
            // 
            this.panelGraph.Controls.Add(this.pbGraph);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(0, 0);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(203, 35);
            this.panelGraph.TabIndex = 49;
            // 
            // cntGenCSDProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.panel1);
            this.Name = "cntGenCSDProps";
            this.Size = new System.Drawing.Size(203, 159);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).EndInit();
            this.panelGraph.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbPRS;
        private System.Windows.Forms.RadioButton rbRBEnable;
        private System.Windows.Forms.RadioButton rbIDACSinking;
        private System.Windows.Forms.RadioButton rbIDACSourcing;
        private System.Windows.Forms.Label lSEC;
        private System.Windows.Forms.TextBox tbSEC;
        private System.Windows.Forms.Label lRb;
        private System.Windows.Forms.TextBox tbRb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCIS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbGraph;
        private System.Windows.Forms.Panel panelGraph;
    }
}
