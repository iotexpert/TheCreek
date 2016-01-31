namespace  CapSense_v0_5.ContentControls
{
    partial class cntSSCSAProps
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCIS = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSettlingTime = new System.Windows.Forms.TextBox();
            this.lScanL = new System.Windows.Forms.Label();
            this.tbPrescPer = new System.Windows.Forms.TextBox();
            this.cbScanLengthL = new System.Windows.Forms.TextBox();
            this.lIdacR = new System.Windows.Forms.Label();
            this.cbIDACSettingL = new System.Windows.Forms.TextBox();
            this.lIdadS = new System.Windows.Forms.Label();
            this.lPrescPer = new System.Windows.Forms.Label();
            this.cbIDACrangeL = new System.Windows.Forms.ComboBox();
            this.lSettlingTime = new System.Windows.Forms.Label();
            this.cbRefValueL = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lCSLeft = new System.Windows.Forms.Label();
            this.cbCustom = new System.Windows.Forms.CheckBox();
            this.panelMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.cbCIS);
            this.panelMain.Controls.Add(this.label6);
            this.panelMain.Controls.Add(this.tbSettlingTime);
            this.panelMain.Controls.Add(this.lScanL);
            this.panelMain.Controls.Add(this.tbPrescPer);
            this.panelMain.Controls.Add(this.cbScanLengthL);
            this.panelMain.Controls.Add(this.lIdacR);
            this.panelMain.Controls.Add(this.cbIDACSettingL);
            this.panelMain.Controls.Add(this.lIdadS);
            this.panelMain.Controls.Add(this.lPrescPer);
            this.panelMain.Controls.Add(this.cbIDACrangeL);
            this.panelMain.Controls.Add(this.lSettlingTime);
            this.panelMain.Controls.Add(this.cbRefValueL);
            this.panelMain.Enabled = false;
            this.panelMain.Location = new System.Drawing.Point(0, 21);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(333, 74);
            this.panelMain.TabIndex = 42;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 16);
            this.label4.TabIndex = 44;
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
            this.cbCIS.TabIndex = 43;
            this.cbCIS.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(3, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Ref Value ";
            // 
            // tbSettlingTime
            // 
            this.tbSettlingTime.Location = new System.Drawing.Point(255, 28);
            this.tbSettlingTime.Name = "tbSettlingTime";
            this.tbSettlingTime.Size = new System.Drawing.Size(76, 20);
            this.tbSettlingTime.TabIndex = 39;
            this.tbSettlingTime.Text = "6";
            // 
            // lScanL
            // 
            this.lScanL.AutoSize = true;
            this.lScanL.Location = new System.Drawing.Point(3, 28);
            this.lScanL.Name = "lScanL";
            this.lScanL.Size = new System.Drawing.Size(71, 13);
            this.lScanL.TabIndex = 27;
            this.lScanL.Text = "Scan Length ";
            // 
            // tbPrescPer
            // 
            this.tbPrescPer.Location = new System.Drawing.Point(255, 5);
            this.tbPrescPer.Name = "tbPrescPer";
            this.tbPrescPer.Size = new System.Drawing.Size(76, 20);
            this.tbPrescPer.TabIndex = 37;
            this.tbPrescPer.Text = "1";
            // 
            // cbScanLengthL
            // 
            this.cbScanLengthL.Location = new System.Drawing.Point(80, 28);
            this.cbScanLengthL.Name = "cbScanLengthL";
            this.cbScanLengthL.Size = new System.Drawing.Size(76, 20);
            this.cbScanLengthL.TabIndex = 38;
            this.cbScanLengthL.Text = "6";
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
            // cbIDACSettingL
            // 
            this.cbIDACSettingL.Location = new System.Drawing.Point(255, 51);
            this.cbIDACSettingL.Name = "cbIDACSettingL";
            this.cbIDACSettingL.Size = new System.Drawing.Size(76, 20);
            this.cbIDACSettingL.TabIndex = 40;
            this.cbIDACSettingL.Text = "20";
            // 
            // lIdadS
            // 
            this.lIdadS.AutoSize = true;
            this.lIdadS.Location = new System.Drawing.Point(178, 51);
            this.lIdadS.Name = "lIdadS";
            this.lIdadS.Size = new System.Drawing.Size(71, 13);
            this.lIdadS.TabIndex = 26;
            this.lIdadS.Text = "IDAC Setting ";
            // 
            // lPrescPer
            // 
            this.lPrescPer.AutoSize = true;
            this.lPrescPer.Location = new System.Drawing.Point(161, 5);
            this.lPrescPer.Name = "lPrescPer";
            this.lPrescPer.Size = new System.Drawing.Size(87, 13);
            this.lPrescPer.TabIndex = 29;
            this.lPrescPer.Text = "Prescaler Period ";
            // 
            // cbIDACrangeL
            // 
            this.cbIDACrangeL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIDACrangeL.FormattingEnabled = true;
            this.cbIDACrangeL.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbIDACrangeL.Location = new System.Drawing.Point(80, 51);
            this.cbIDACrangeL.Name = "cbIDACrangeL";
            this.cbIDACrangeL.Size = new System.Drawing.Size(76, 21);
            this.cbIDACrangeL.TabIndex = 34;
            // 
            // lSettlingTime
            // 
            this.lSettlingTime.AutoSize = true;
            this.lSettlingTime.Location = new System.Drawing.Point(178, 28);
            this.lSettlingTime.Name = "lSettlingTime";
            this.lSettlingTime.Size = new System.Drawing.Size(68, 13);
            this.lSettlingTime.TabIndex = 30;
            this.lSettlingTime.Text = "Settling Time";
            // 
            // cbRefValueL
            // 
            this.cbRefValueL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRefValueL.Enabled = false;
            this.cbRefValueL.FormattingEnabled = true;
            this.cbRefValueL.Items.AddRange(new object[] {
            "1.024V"});
            this.cbRefValueL.Location = new System.Drawing.Point(80, 5);
            this.cbRefValueL.Name = "cbRefValueL";
            this.cbRefValueL.Size = new System.Drawing.Size(76, 21);
            this.cbRefValueL.TabIndex = 36;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lCSLeft);
            this.panel2.Controls.Add(this.cbCustom);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 21);
            this.panel2.TabIndex = 43;
            // 
            // lCSLeft
            // 
            this.lCSLeft.AutoSize = true;
            this.lCSLeft.Location = new System.Drawing.Point(3, 1);
            this.lCSLeft.Name = "lCSLeft";
            this.lCSLeft.Size = new System.Drawing.Size(122, 13);
            this.lCSLeft.TabIndex = 21;
            this.lCSLeft.Text = "CapSense Method: CSA";
            // 
            // cbCustom
            // 
            this.cbCustom.AutoSize = true;
            this.cbCustom.Location = new System.Drawing.Point(197, 0);
            this.cbCustom.Name = "cbCustom";
            this.cbCustom.Size = new System.Drawing.Size(61, 17);
            this.cbCustom.TabIndex = 0;
            this.cbCustom.Text = "Custom";
            this.cbCustom.UseVisualStyleBackColor = true;
            this.cbCustom.CheckedChanged += new System.EventHandler(this.cbCustom_CheckedChanged);
            // 
            // cntSSCSAProps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelMain);
            this.Name = "cntSSCSAProps";
            this.Size = new System.Drawing.Size(335, 95);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbSettlingTime;
        private System.Windows.Forms.Label lScanL;
        private System.Windows.Forms.TextBox tbPrescPer;
        private System.Windows.Forms.TextBox cbScanLengthL;
        private System.Windows.Forms.Label lIdacR;
        private System.Windows.Forms.TextBox cbIDACSettingL;
        private System.Windows.Forms.Label lIdadS;
        private System.Windows.Forms.Label lPrescPer;
        private System.Windows.Forms.ComboBox cbIDACrangeL;
        private System.Windows.Forms.Label lSettlingTime;
        private System.Windows.Forms.ComboBox cbRefValueL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCIS;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lCSLeft;
        private System.Windows.Forms.CheckBox cbCustom;
    }
}
