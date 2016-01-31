namespace  CapSense_v1_20
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
            this.cbReference = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pbGraph = new System.Windows.Forms.PictureBox();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGraph)).BeginInit();
            this.panelGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSEC
            // 
            this.tbSEC.Location = new System.Drawing.Point(100, 77);
            this.tbSEC.Name = "tbSEC";
            this.tbSEC.Size = new System.Drawing.Size(99, 20);
            this.tbSEC.TabIndex = 7;
            this.tbSEC.Text = "0";
            this.tbSEC.Validating += new System.ComponentModel.CancelEventHandler(this.tbSEC_Validating);
            // 
            // lSEC
            // 
            this.lSEC.Location = new System.Drawing.Point(1, 73);
            this.lSEC.Name = "lSEC";
            this.lSEC.Size = new System.Drawing.Size(93, 27);
            this.lSEC.TabIndex = 2;
            this.lSEC.Text = "Shield Electrode Count";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 59);
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
            this.cbPRS.TabIndex = 5;
            // 
            // rbRBEnable
            // 
            this.rbRBEnable.AutoSize = true;
            this.rbRBEnable.Location = new System.Drawing.Point(3, 36);
            this.rbRBEnable.Name = "rbRBEnable";
            this.rbRBEnable.Size = new System.Drawing.Size(166, 17);
            this.rbRBEnable.TabIndex = 2;
            this.rbRBEnable.Text = "IDAC disable, use external Rb";
            this.rbRBEnable.UseVisualStyleBackColor = true;
            // 
            // rbIDACSinking
            // 
            this.rbIDACSinking.AutoSize = true;
            this.rbIDACSinking.Location = new System.Drawing.Point(3, 20);
            this.rbIDACSinking.Name = "rbIDACSinking";
            this.rbIDACSinking.Size = new System.Drawing.Size(86, 17);
            this.rbIDACSinking.TabIndex = 1;
            this.rbIDACSinking.Text = "IDAC sinking";
            this.rbIDACSinking.UseVisualStyleBackColor = true;
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
            // 
            // lRb
            // 
            this.lRb.AutoSize = true;
            this.lRb.Location = new System.Drawing.Point(2, 101);
            this.lRb.Name = "lRb";
            this.lRb.Size = new System.Drawing.Size(52, 13);
            this.lRb.TabIndex = 2;
            this.lRb.Text = "Rb Count";
            this.lRb.Visible = false;
            // 
            // tbRb
            // 
            this.tbRb.Location = new System.Drawing.Point(100, 98);
            this.tbRb.Name = "tbRb";
            this.tbRb.Size = new System.Drawing.Size(99, 20);
            this.tbRb.TabIndex = 9;
            this.tbRb.Text = "1";
            this.tbRb.Visible = false;
            this.tbRb.Validating += new System.ComponentModel.CancelEventHandler(this.tbRb_Validating);
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.Controls.Add(this.cbReference);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.rbIDACSinking);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Controls.Add(this.cbPRS);
            this.panelMain.Controls.Add(this.lSEC);
            this.panelMain.Controls.Add(this.lRb);
            this.panelMain.Controls.Add(this.rbRBEnable);
            this.panelMain.Controls.Add(this.rbIDACSourcing);
            this.panelMain.Controls.Add(this.tbRb);
            this.panelMain.Controls.Add(this.tbSEC);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelMain.Location = new System.Drawing.Point(0, 112);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(204, 122);
            this.panelMain.TabIndex = 47;
            // 
            // cbReference
            // 
            this.cbReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReference.FormattingEnabled = true;
            this.cbReference.Location = new System.Drawing.Point(100, 119);
            this.cbReference.Name = "cbReference";
            this.cbReference.Size = new System.Drawing.Size(99, 21);
            this.cbReference.TabIndex = 11;
            this.cbReference.Visible = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(2, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 47;
            this.label2.Text = "Reference ";
            this.label2.Visible = false;
            // 
            // pbGraph
            // 
            this.pbGraph.BackColor = System.Drawing.Color.White;
            this.pbGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGraph.Location = new System.Drawing.Point(2, 2);
            this.pbGraph.Name = "pbGraph";
            this.pbGraph.Size = new System.Drawing.Size(200, 108);
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
            this.panelGraph.Padding = new System.Windows.Forms.Padding(2);
            this.panelGraph.Size = new System.Drawing.Size(204, 112);
            this.panelGraph.TabIndex = 49;
            // 
            // CyGenCSDProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.panelMain);
            this.Name = "CyGenCSDProperties";
            this.Size = new System.Drawing.Size(204, 234);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
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
        private System.Windows.Forms.ComboBox cbReference;
        private System.Windows.Forms.Label label2;
    }
}
