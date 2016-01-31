namespace  CapSense_v1_20.ContentControls
{
    partial class CySSCSDProperties
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
            this.cbIDACSetting = new System.Windows.Forms.TextBox();
            this.cbResolution = new System.Windows.Forms.ComboBox();
            this.cbIDACrange = new System.Windows.Forms.ComboBox();
            this.cblScanSpeed = new System.Windows.Forms.ComboBox();
            this.lScanSpeed = new System.Windows.Forms.Label();
            this.lPrescPer = new System.Windows.Forms.Label();
            this.lIdadS = new System.Windows.Forms.Label();
            this.lIdacR = new System.Windows.Forms.Label();
            this.lResolution = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.lNone = new System.Windows.Forms.Label();
            this.cbCIS = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPrescPer
            // 
            this.tbPrescPer.Location = new System.Drawing.Point(88, 5);
            this.tbPrescPer.Margin = new System.Windows.Forms.Padding(4);
            this.tbPrescPer.Name = "tbPrescPer";
            this.tbPrescPer.Size = new System.Drawing.Size(100, 22);
            this.tbPrescPer.TabIndex = 2;
            this.tbPrescPer.Text = "1";
            // 
            // cbIDACSetting
            // 
            this.cbIDACSetting.Location = new System.Drawing.Point(313, 62);
            this.cbIDACSetting.Margin = new System.Windows.Forms.Padding(4);
            this.cbIDACSetting.Name = "cbIDACSetting";
            this.cbIDACSetting.Size = new System.Drawing.Size(100, 22);
            this.cbIDACSetting.TabIndex = 12;
            this.cbIDACSetting.Text = "20";
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
            this.cbResolution.Location = new System.Drawing.Point(88, 32);
            this.cbResolution.Margin = new System.Windows.Forms.Padding(4);
            this.cbResolution.Name = "cbResolution";
            this.cbResolution.Size = new System.Drawing.Size(100, 24);
            this.cbResolution.TabIndex = 4;
            // 
            // cbIDACrange
            // 
            this.cbIDACrange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIDACrange.FormattingEnabled = true;
            this.cbIDACrange.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbIDACrange.Location = new System.Drawing.Point(88, 60);
            this.cbIDACrange.Margin = new System.Windows.Forms.Padding(4);
            this.cbIDACrange.Name = "cbIDACrange";
            this.cbIDACrange.Size = new System.Drawing.Size(100, 24);
            this.cbIDACrange.TabIndex = 6;
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
            this.cblScanSpeed.Location = new System.Drawing.Point(313, 33);
            this.cblScanSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.cblScanSpeed.Name = "cblScanSpeed";
            this.cblScanSpeed.Size = new System.Drawing.Size(100, 24);
            this.cblScanSpeed.TabIndex = 10;
            // 
            // lScanSpeed
            // 
            this.lScanSpeed.AutoSize = true;
            this.lScanSpeed.Location = new System.Drawing.Point(197, 38);
            this.lScanSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lScanSpeed.Name = "lScanSpeed";
            this.lScanSpeed.Size = new System.Drawing.Size(89, 17);
            this.lScanSpeed.TabIndex = 32;
            this.lScanSpeed.Text = "Scan Speed ";
            // 
            // lPrescPer
            // 
            this.lPrescPer.Location = new System.Drawing.Point(5, 2);
            this.lPrescPer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPrescPer.Name = "lPrescPer";
            this.lPrescPer.Size = new System.Drawing.Size(79, 34);
            this.lPrescPer.TabIndex = 29;
            this.lPrescPer.Text = "Prescaler Period ";
            // 
            // lIdadS
            // 
            this.lIdadS.AutoSize = true;
            this.lIdadS.Location = new System.Drawing.Point(199, 63);
            this.lIdadS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdadS.Name = "lIdadS";
            this.lIdadS.Size = new System.Drawing.Size(91, 17);
            this.lIdadS.TabIndex = 26;
            this.lIdadS.Text = "IDAC Setting ";
            // 
            // lIdacR
            // 
            this.lIdacR.AutoSize = true;
            this.lIdacR.Location = new System.Drawing.Point(4, 63);
            this.lIdacR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdacR.Name = "lIdacR";
            this.lIdacR.Size = new System.Drawing.Size(84, 17);
            this.lIdacR.TabIndex = 25;
            this.lIdacR.Text = "IDAC range ";
            // 
            // lResolution
            // 
            this.lResolution.AutoSize = true;
            this.lResolution.Location = new System.Drawing.Point(5, 38);
            this.lResolution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lResolution.Name = "lResolution";
            this.lResolution.Size = new System.Drawing.Size(75, 17);
            this.lResolution.TabIndex = 28;
            this.lResolution.Text = "Resolution";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.lNone);
            this.panelMain.Controls.Add(this.cbIDACrange);
            this.panelMain.Controls.Add(this.cbCIS);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.tbPrescPer);
            this.panelMain.Controls.Add(this.lResolution);
            this.panelMain.Controls.Add(this.lIdacR);
            this.panelMain.Controls.Add(this.cbIDACSetting);
            this.panelMain.Controls.Add(this.lIdadS);
            this.panelMain.Controls.Add(this.cbResolution);
            this.panelMain.Controls.Add(this.lPrescPer);
            this.panelMain.Controls.Add(this.lScanSpeed);
            this.panelMain.Controls.Add(this.cblScanSpeed);
            this.panelMain.Location = new System.Drawing.Point(0, -1);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(423, 87);
            this.panelMain.TabIndex = 41;
            // 
            // lNone
            // 
            this.lNone.AutoSize = true;
            this.lNone.Location = new System.Drawing.Point(88, 6);
            this.lNone.Name = "lNone";
            this.lNone.Size = new System.Drawing.Size(42, 17);
            this.lNone.TabIndex = 43;
            this.lNone.Text = "None";
            this.lNone.Visible = false;
            // 
            // cbCIS
            // 
            this.cbCIS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCIS.FormattingEnabled = true;
            this.cbCIS.Items.AddRange(new object[] {
            "Ground",
            "Hi-Z Analog",
            "Shield"});
            this.cbCIS.Location = new System.Drawing.Point(313, 5);
            this.cbCIS.Margin = new System.Windows.Forms.Padding(4);
            this.cbCIS.Name = "cbCIS";
            this.cbCIS.Size = new System.Drawing.Size(100, 24);
            this.cbCIS.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(197, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 32);
            this.label4.TabIndex = 42;
            this.label4.Text = "Connect Inactive Sensors";
            // 
            // CySSCSDProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CySSCSDProperties";
            this.Size = new System.Drawing.Size(427, 86);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbPrescPer;
        private System.Windows.Forms.TextBox cbIDACSetting;
        private System.Windows.Forms.ComboBox cbResolution;
        private System.Windows.Forms.ComboBox cbIDACrange;
        private System.Windows.Forms.ComboBox cblScanSpeed;
        private System.Windows.Forms.Label lScanSpeed;
        private System.Windows.Forms.Label lPrescPer;
        private System.Windows.Forms.Label lIdadS;
        private System.Windows.Forms.Label lIdacR;
        private System.Windows.Forms.Label lResolution;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCIS;
        private System.Windows.Forms.Label lNone;
    }
}
