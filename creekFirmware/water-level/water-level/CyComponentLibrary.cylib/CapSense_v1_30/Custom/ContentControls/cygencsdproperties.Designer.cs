namespace  CapSense_v1_30
{
    partial class CyGenCSDProperties
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.gbtop = new System.Windows.Forms.GroupBox();
            this.gbVref = new System.Windows.Forms.GroupBox();
            this.tbVdacValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbVdac = new System.Windows.Forms.RadioButton();
            this.rbVref = new System.Windows.Forms.RadioButton();
            this.pbGraph = new System.Windows.Forms.PictureBox();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.panelMain.SuspendLayout();
            this.gbtop.SuspendLayout();
            this.gbVref.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).BeginInit();
            this.panelGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSEC
            // 
            this.tbSEC.Location = new System.Drawing.Point(136, 94);
            this.tbSEC.Margin = new System.Windows.Forms.Padding(4);
            this.tbSEC.Name = "tbSEC";
            this.tbSEC.Size = new System.Drawing.Size(92, 22);
            this.tbSEC.TabIndex = 7;
            this.tbSEC.Text = "0";
            this.tbSEC.Validating += new System.ComponentModel.CancelEventHandler(this.tbSEC_Validating);
            // 
            // lSEC
            // 
            this.lSEC.Location = new System.Drawing.Point(7, 88);
            this.lSEC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lSEC.Name = "lSEC";
            this.lSEC.Size = new System.Drawing.Size(115, 34);
            this.lSEC.TabIndex = 2;
            this.lSEC.Text = "Shield Electrode Count";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "PRS";
            // 
            // cbPRS
            // 
            this.cbPRS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPRS.FormattingEnabled = true;
            this.cbPRS.Location = new System.Drawing.Point(136, 68);
            this.cbPRS.Margin = new System.Windows.Forms.Padding(4);
            this.cbPRS.Name = "cbPRS";
            this.cbPRS.Size = new System.Drawing.Size(92, 24);
            this.cbPRS.TabIndex = 5;
            // 
            // rbRBEnable
            // 
            this.rbRBEnable.AutoSize = true;
            this.rbRBEnable.Location = new System.Drawing.Point(7, 47);
            this.rbRBEnable.Margin = new System.Windows.Forms.Padding(4);
            this.rbRBEnable.Name = "rbRBEnable";
            this.rbRBEnable.Size = new System.Drawing.Size(216, 21);
            this.rbRBEnable.TabIndex = 2;
            this.rbRBEnable.Text = "IDAC disable, use external Rb";
            this.rbRBEnable.UseVisualStyleBackColor = true;
            // 
            // rbIDACSinking
            // 
            this.rbIDACSinking.AutoSize = true;
            this.rbIDACSinking.Location = new System.Drawing.Point(7, 28);
            this.rbIDACSinking.Margin = new System.Windows.Forms.Padding(4);
            this.rbIDACSinking.Name = "rbIDACSinking";
            this.rbIDACSinking.Size = new System.Drawing.Size(108, 21);
            this.rbIDACSinking.TabIndex = 1;
            this.rbIDACSinking.Text = "IDAC sinking";
            this.rbIDACSinking.UseVisualStyleBackColor = true;
            // 
            // rbIDACSourcing
            // 
            this.rbIDACSourcing.AutoSize = true;
            this.rbIDACSourcing.Checked = true;
            this.rbIDACSourcing.Location = new System.Drawing.Point(7, 9);
            this.rbIDACSourcing.Margin = new System.Windows.Forms.Padding(4);
            this.rbIDACSourcing.Name = "rbIDACSourcing";
            this.rbIDACSourcing.Size = new System.Drawing.Size(118, 21);
            this.rbIDACSourcing.TabIndex = 0;
            this.rbIDACSourcing.TabStop = true;
            this.rbIDACSourcing.Text = "IDAC sourcing";
            this.rbIDACSourcing.UseVisualStyleBackColor = true;
            // 
            // lRb
            // 
            this.lRb.AutoSize = true;
            this.lRb.Location = new System.Drawing.Point(7, 122);
            this.lRb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lRb.Name = "lRb";
            this.lRb.Size = new System.Drawing.Size(67, 17);
            this.lRb.TabIndex = 2;
            this.lRb.Text = "Rb Count";
            this.lRb.Visible = false;
            // 
            // tbRb
            // 
            this.tbRb.Location = new System.Drawing.Point(136, 118);
            this.tbRb.Margin = new System.Windows.Forms.Padding(4);
            this.tbRb.Name = "tbRb";
            this.tbRb.Size = new System.Drawing.Size(92, 22);
            this.tbRb.TabIndex = 9;
            this.tbRb.Text = "1";
            this.tbRb.Visible = false;
            this.tbRb.Validating += new System.ComponentModel.CancelEventHandler(this.tbRb_Validating);
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.Controls.Add(this.gbtop);
            this.panelMain.Controls.Add(this.gbVref);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelMain.Location = new System.Drawing.Point(0, 11);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(336, 191);
            this.panelMain.TabIndex = 47;
            // 
            // gbtop
            // 
            this.gbtop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbtop.Controls.Add(this.rbIDACSourcing);
            this.gbtop.Controls.Add(this.tbSEC);
            this.gbtop.Controls.Add(this.tbRb);
            this.gbtop.Controls.Add(this.rbIDACSinking);
            this.gbtop.Controls.Add(this.rbRBEnable);
            this.gbtop.Controls.Add(this.label1);
            this.gbtop.Controls.Add(this.lRb);
            this.gbtop.Controls.Add(this.cbPRS);
            this.gbtop.Controls.Add(this.lSEC);
            this.gbtop.Location = new System.Drawing.Point(0, -5);
            this.gbtop.MinimumSize = new System.Drawing.Size(276, 0);
            this.gbtop.Name = "gbtop";
            this.gbtop.Size = new System.Drawing.Size(333, 145);
            this.gbtop.TabIndex = 51;
            this.gbtop.TabStop = false;
            // 
            // gbVref
            // 
            this.gbVref.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbVref.Controls.Add(this.tbVdacValue);
            this.gbVref.Controls.Add(this.label4);
            this.gbVref.Controls.Add(this.label3);
            this.gbVref.Controls.Add(this.rbVdac);
            this.gbVref.Controls.Add(this.rbVref);
            this.gbVref.Location = new System.Drawing.Point(0, 134);
            this.gbVref.MinimumSize = new System.Drawing.Size(276, 0);
            this.gbVref.Name = "gbVref";
            this.gbVref.Size = new System.Drawing.Size(333, 55);
            this.gbVref.TabIndex = 50;
            this.gbVref.TabStop = false;
            // 
            // tbVdacValue
            // 
            this.tbVdacValue.Enabled = false;
            this.tbVdacValue.Location = new System.Drawing.Point(100, 27);
            this.tbVdacValue.Name = "tbVdacValue";
            this.tbVdacValue.Size = new System.Drawing.Size(81, 22);
            this.tbVdacValue.TabIndex = 52;
            this.tbVdacValue.Text = "0";
            this.tbVdacValue.Validated += new System.EventHandler(this.tbVdacValue_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(187, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 17);
            this.label4.TabIndex = 51;
            this.label4.Text = "0.0 V";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 50;
            this.label3.Text = "1.024 V";
            // 
            // rbVdac
            // 
            this.rbVdac.AutoSize = true;
            this.rbVdac.Location = new System.Drawing.Point(6, 29);
            this.rbVdac.Name = "rbVdac";
            this.rbVdac.Size = new System.Drawing.Size(61, 21);
            this.rbVdac.TabIndex = 49;
            this.rbVdac.Text = "Vdac";
            this.rbVdac.UseVisualStyleBackColor = true;
            this.rbVdac.CheckedChanged += new System.EventHandler(this.rbVref_CheckedChanged);
            // 
            // rbVref
            // 
            this.rbVref.AutoSize = true;
            this.rbVref.Checked = true;
            this.rbVref.Location = new System.Drawing.Point(6, 9);
            this.rbVref.Name = "rbVref";
            this.rbVref.Size = new System.Drawing.Size(55, 21);
            this.rbVref.TabIndex = 48;
            this.rbVref.TabStop = true;
            this.rbVref.Text = "Vref";
            this.rbVref.UseVisualStyleBackColor = true;
            this.rbVref.CheckedChanged += new System.EventHandler(this.rbVref_CheckedChanged);
            // 
            // pbGraph
            // 
            this.pbGraph.BackColor = System.Drawing.Color.White;
            this.pbGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraph.Location = new System.Drawing.Point(3, 2);
            this.pbGraph.Margin = new System.Windows.Forms.Padding(4);
            this.pbGraph.Name = "pbGraph";
            this.pbGraph.Size = new System.Drawing.Size(330, 7);
            this.pbGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbGraph.TabIndex = 48;
            this.pbGraph.TabStop = false;
            // 
            // panelGraph
            // 
            this.panelGraph.Controls.Add(this.pbGraph);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(0, 0);
            this.panelGraph.Margin = new System.Windows.Forms.Padding(4);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelGraph.Size = new System.Drawing.Size(336, 11);
            this.panelGraph.TabIndex = 49;
            // 
            // CyGenCSDProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.panelMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CyGenCSDProperties";
            this.Size = new System.Drawing.Size(336, 202);
            this.panelMain.ResumeLayout(false);
            this.gbtop.ResumeLayout(false);
            this.gbtop.PerformLayout();
            this.gbVref.ResumeLayout(false);
            this.gbVref.PerformLayout();
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
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.PictureBox pbGraph;
        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.RadioButton rbVdac;
        private System.Windows.Forms.RadioButton rbVref;
        private System.Windows.Forms.GroupBox gbVref;
        private System.Windows.Forms.TextBox tbVdacValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbtop;
    }
}
