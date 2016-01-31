namespace  CapSense_v1_30.ContentControls
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
            this.lNone = new System.Windows.Forms.Label();
            this.cbCIS = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbPrescPer
            // 
            this.tbPrescPer.Location = new System.Drawing.Point(70, 3);
            this.tbPrescPer.Margin = new System.Windows.Forms.Padding(4);
            this.tbPrescPer.Name = "tbPrescPer";
            this.tbPrescPer.Size = new System.Drawing.Size(61, 20);
            this.tbPrescPer.TabIndex = 2;
            this.tbPrescPer.Text = "1";
            // 
            // cbIDACSetting
            // 
            this.cbIDACSetting.Location = new System.Drawing.Point(225, 49);
            this.cbIDACSetting.Margin = new System.Windows.Forms.Padding(4);
            this.cbIDACSetting.Name = "cbIDACSetting";
            this.cbIDACSetting.Size = new System.Drawing.Size(74, 20);
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
            this.cbResolution.Location = new System.Drawing.Point(70, 24);
            this.cbResolution.Margin = new System.Windows.Forms.Padding(4);
            this.cbResolution.Name = "cbResolution";
            this.cbResolution.Size = new System.Drawing.Size(61, 21);
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
            this.cbIDACrange.Location = new System.Drawing.Point(70, 47);
            this.cbIDACrange.Margin = new System.Windows.Forms.Padding(4);
            this.cbIDACrange.Name = "cbIDACrange";
            this.cbIDACrange.Size = new System.Drawing.Size(61, 21);
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
            this.cblScanSpeed.Location = new System.Drawing.Point(225, 27);
            this.cblScanSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.cblScanSpeed.Name = "cblScanSpeed";
            this.cblScanSpeed.Size = new System.Drawing.Size(74, 21);
            this.cblScanSpeed.TabIndex = 10;
            // 
            // lScanSpeed
            // 
            this.lScanSpeed.AutoSize = true;
            this.lScanSpeed.Location = new System.Drawing.Point(135, 32);
            this.lScanSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lScanSpeed.Name = "lScanSpeed";
            this.lScanSpeed.Size = new System.Drawing.Size(69, 13);
            this.lScanSpeed.TabIndex = 32;
            this.lScanSpeed.Text = "Scan Speed ";
            // 
            // lPrescPer
            // 
            this.lPrescPer.Location = new System.Drawing.Point(4, 0);
            this.lPrescPer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPrescPer.Name = "lPrescPer";
            this.lPrescPer.Size = new System.Drawing.Size(75, 34);
            this.lPrescPer.TabIndex = 29;
            this.lPrescPer.Text = "Prescaler Period ";
            // 
            // lIdadS
            // 
            this.lIdadS.AutoSize = true;
            this.lIdadS.Location = new System.Drawing.Point(135, 52);
            this.lIdadS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdadS.Name = "lIdadS";
            this.lIdadS.Size = new System.Drawing.Size(71, 13);
            this.lIdadS.TabIndex = 26;
            this.lIdadS.Text = "IDAC Setting ";
            // 
            // lIdacR
            // 
            this.lIdacR.AutoSize = true;
            this.lIdacR.Location = new System.Drawing.Point(3, 50);
            this.lIdacR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdacR.Name = "lIdacR";
            this.lIdacR.Size = new System.Drawing.Size(65, 13);
            this.lIdacR.TabIndex = 25;
            this.lIdacR.Text = "IDAC range ";
            // 
            // lResolution
            // 
            this.lResolution.AutoSize = true;
            this.lResolution.Location = new System.Drawing.Point(4, 31);
            this.lResolution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lResolution.Name = "lResolution";
            this.lResolution.Size = new System.Drawing.Size(57, 13);
            this.lResolution.TabIndex = 28;
            this.lResolution.Text = "Resolution";
            // 
            // lNone
            // 
            this.lNone.AutoSize = true;
            this.lNone.Location = new System.Drawing.Point(71, 4);
            this.lNone.Name = "lNone";
            this.lNone.Size = new System.Drawing.Size(33, 13);
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
            this.cbCIS.Location = new System.Drawing.Point(225, 4);
            this.cbCIS.Margin = new System.Windows.Forms.Padding(4);
            this.cbCIS.Name = "cbCIS";
            this.cbCIS.Size = new System.Drawing.Size(74, 21);
            this.cbCIS.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(135, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 34);
            this.label4.TabIndex = 42;
            this.label4.Text = "Connect Inactive Sensors";
            // 
            // CySSCSDProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.cblScanSpeed);
            this.Controls.Add(this.lNone);
            this.Controls.Add(this.cbIDACrange);
            this.Controls.Add(this.lScanSpeed);
            this.Controls.Add(this.cbCIS);
            this.Controls.Add(this.cbResolution);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lIdadS);
            this.Controls.Add(this.tbPrescPer);
            this.Controls.Add(this.cbIDACSetting);
            this.Controls.Add(this.lResolution);
            this.Controls.Add(this.lIdacR);
            this.Controls.Add(this.lPrescPer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(10, 19);
            this.Name = "CySSCSDProperties";
            this.Size = new System.Drawing.Size(413, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCIS;
        private System.Windows.Forms.Label lNone;
    }
}
