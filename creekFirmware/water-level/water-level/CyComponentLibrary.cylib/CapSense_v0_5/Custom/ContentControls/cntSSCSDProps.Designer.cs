namespace  CapSense_v0_5.ContentControls
{
    partial class cntSSCSDProps
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
            this.tbPrescPer = new System.Windows.Forms.TextBox();
            this.cbIDACSettingL = new System.Windows.Forms.TextBox();
            this.cbResolution = new System.Windows.Forms.ComboBox();
            this.cbIDACrangeL = new System.Windows.Forms.ComboBox();
            this.cblScanSpeed = new System.Windows.Forms.ComboBox();
            this.lScanSpeed = new System.Windows.Forms.Label();
            this.lPrescPer = new System.Windows.Forms.Label();
            this.lIdadS = new System.Windows.Forms.Label();
            this.lIdacR = new System.Windows.Forms.Label();
            this.lResolution = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCIS = new System.Windows.Forms.ComboBox();
            this.cbCustom = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lCSLeft = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPrescPer
            // 
            this.tbPrescPer.Location = new System.Drawing.Point(71, 6);
            this.tbPrescPer.Name = "tbPrescPer";
            this.tbPrescPer.Size = new System.Drawing.Size(76, 20);
            this.tbPrescPer.TabIndex = 37;
            this.tbPrescPer.Text = "1";
            // 
            // cbIDACSettingL
            // 
            this.cbIDACSettingL.Location = new System.Drawing.Point(235, 51);
            this.cbIDACSettingL.Name = "cbIDACSettingL";
            this.cbIDACSettingL.Size = new System.Drawing.Size(76, 20);
            this.cbIDACSettingL.TabIndex = 40;
            this.cbIDACSettingL.Text = "20";
            // 
            // cbResolution
            // 
            this.cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResolution.FormattingEnabled = true;
            this.cbResolution.Items.AddRange(new object[] {
            "8 bits",
            "9 bits",
            "10 bits",
            "11 bits",
            "12 bits",
            "13 bits",
            "14 bits",
            "15 bits",
            "16 bits"});
            this.cbResolution.Location = new System.Drawing.Point(71, 28);
            this.cbResolution.Name = "cbResolution";
            this.cbResolution.Size = new System.Drawing.Size(76, 21);
            this.cbResolution.TabIndex = 33;
            // 
            // cbIDACrangeL
            // 
            this.cbIDACrangeL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIDACrangeL.FormattingEnabled = true;
            this.cbIDACrangeL.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbIDACrangeL.Location = new System.Drawing.Point(71, 51);
            this.cbIDACrangeL.Name = "cbIDACrangeL";
            this.cbIDACrangeL.Size = new System.Drawing.Size(76, 21);
            this.cbIDACrangeL.TabIndex = 34;
            // 
            // cblScanSpeed
            // 
            this.cblScanSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cblScanSpeed.FormattingEnabled = true;
            this.cblScanSpeed.Items.AddRange(new object[] {
            "Ultra Fast",
            "Fast",
            "Normal",
            "Slow"});
            this.cblScanSpeed.Location = new System.Drawing.Point(235, 28);
            this.cblScanSpeed.Name = "cblScanSpeed";
            this.cblScanSpeed.Size = new System.Drawing.Size(76, 21);
            this.cblScanSpeed.TabIndex = 35;
            // 
            // lScanSpeed
            // 
            this.lScanSpeed.AutoSize = true;
            this.lScanSpeed.Location = new System.Drawing.Point(160, 28);
            this.lScanSpeed.Name = "lScanSpeed";
            this.lScanSpeed.Size = new System.Drawing.Size(69, 13);
            this.lScanSpeed.TabIndex = 32;
            this.lScanSpeed.Text = "Scan Speed ";
            // 
            // lPrescPer
            // 
            this.lPrescPer.Location = new System.Drawing.Point(4, 2);
            this.lPrescPer.Name = "lPrescPer";
            this.lPrescPer.Size = new System.Drawing.Size(59, 28);
            this.lPrescPer.TabIndex = 29;
            this.lPrescPer.Text = "Prescaler Period ";
            // 
            // lIdadS
            // 
            this.lIdadS.AutoSize = true;
            this.lIdadS.Location = new System.Drawing.Point(158, 51);
            this.lIdadS.Name = "lIdadS";
            this.lIdadS.Size = new System.Drawing.Size(71, 13);
            this.lIdadS.TabIndex = 26;
            this.lIdadS.Text = "IDAC Setting ";
            // 
            // lIdacR
            // 
            this.lIdacR.AutoSize = true;
            this.lIdacR.Location = new System.Drawing.Point(3, 51);
            this.lIdacR.Name = "lIdacR";
            this.lIdacR.Size = new System.Drawing.Size(65, 13);
            this.lIdacR.TabIndex = 25;
            this.lIdacR.Text = "IDAC range ";
            // 
            // lResolution
            // 
            this.lResolution.AutoSize = true;
            this.lResolution.Location = new System.Drawing.Point(3, 28);
            this.lResolution.Name = "lResolution";
            this.lResolution.Size = new System.Drawing.Size(57, 13);
            this.lResolution.TabIndex = 28;
            this.lResolution.Text = "Resolution";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.cbCIS);
            this.panelMain.Controls.Add(this.tbPrescPer);
            this.panelMain.Controls.Add(this.lResolution);
            this.panelMain.Controls.Add(this.lIdacR);
            this.panelMain.Controls.Add(this.cbIDACSettingL);
            this.panelMain.Controls.Add(this.lIdadS);
            this.panelMain.Controls.Add(this.cbResolution);
            this.panelMain.Controls.Add(this.lPrescPer);
            this.panelMain.Controls.Add(this.cbIDACrangeL);
            this.panelMain.Controls.Add(this.lScanSpeed);
            this.panelMain.Controls.Add(this.cblScanSpeed);
            this.panelMain.Enabled = false;
            this.panelMain.Location = new System.Drawing.Point(0, 20);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(317, 73);
            this.panelMain.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 16);
            this.label4.TabIndex = 42;
            this.label4.Text = "Connect Inactive Sensors";
            this.label4.Visible = false;
            // 
            // cbCIS
            // 
            this.cbCIS.FormattingEnabled = true;
            this.cbCIS.Items.AddRange(new object[] {
            "Ground",
            "Hi-Z Analog"});
            this.cbCIS.Location = new System.Drawing.Point(138, 67);
            this.cbCIS.Name = "cbCIS";
            this.cbCIS.Size = new System.Drawing.Size(99, 21);
            this.cbCIS.TabIndex = 41;
            this.cbCIS.Visible = false;
            // 
            // cbCustom
            // 
            this.cbCustom.AutoSize = true;
            this.cbCustom.Location = new System.Drawing.Point(189, 0);
            this.cbCustom.Name = "cbCustom";
            this.cbCustom.Size = new System.Drawing.Size(61, 17);
            this.cbCustom.TabIndex = 0;
            this.cbCustom.Text = "Custom";
            this.cbCustom.UseVisualStyleBackColor = true;
            this.cbCustom.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lCSLeft);
            this.panel2.Controls.Add(this.cbCustom);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(320, 21);
            this.panel2.TabIndex = 44;
            // 
            // lCSLeft
            // 
            this.lCSLeft.AutoSize = true;
            this.lCSLeft.Location = new System.Drawing.Point(3, 1);
            this.lCSLeft.Name = "lCSLeft";
            this.lCSLeft.Size = new System.Drawing.Size(123, 13);
            this.lCSLeft.TabIndex = 21;
            this.lCSLeft.Text = "CapSense Method: CSD";
            // 
            // cntSSCSDProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelMain);
            this.Name = "cntSSCSDProps";
            this.Size = new System.Drawing.Size(320, 94);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbPrescPer;
        private System.Windows.Forms.TextBox cbIDACSettingL;
        private System.Windows.Forms.ComboBox cbResolution;
        private System.Windows.Forms.ComboBox cbIDACrangeL;
        private System.Windows.Forms.ComboBox cblScanSpeed;
        private System.Windows.Forms.Label lScanSpeed;
        private System.Windows.Forms.Label lPrescPer;
        private System.Windows.Forms.Label lIdadS;
        private System.Windows.Forms.Label lIdacR;
        private System.Windows.Forms.Label lResolution;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCIS;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lCSLeft;
        private System.Windows.Forms.CheckBox cbCustom;
    }
}
