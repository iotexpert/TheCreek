namespace  CapSense_v1_30.ContentControls
{
    partial class CySSCSAProperties
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
            this.lPrescPer = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCIS = new System.Windows.Forms.ComboBox();
            this.tbSettlingTime = new System.Windows.Forms.TextBox();
            this.lScanL = new System.Windows.Forms.Label();
            this.tbPrescPer = new System.Windows.Forms.TextBox();
            this.cbScanLength = new System.Windows.Forms.TextBox();
            this.lIdacR = new System.Windows.Forms.Label();
            this.cbIDACSetting = new System.Windows.Forms.TextBox();
            this.lIdadS = new System.Windows.Forms.Label();
            this.cbIDACRange = new System.Windows.Forms.ComboBox();
            this.lSettlingTime = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            //
            // panelMain
            //
            this.panelMain.Controls.Add(this.lPrescPer);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.cbCIS);
            this.panelMain.Controls.Add(this.tbSettlingTime);
            this.panelMain.Controls.Add(this.lScanL);
            this.panelMain.Controls.Add(this.tbPrescPer);
            this.panelMain.Controls.Add(this.cbScanLength);
            this.panelMain.Controls.Add(this.lIdacR);
            this.panelMain.Controls.Add(this.cbIDACSetting);
            this.panelMain.Controls.Add(this.lIdadS);
            this.panelMain.Controls.Add(this.cbIDACRange);
            this.panelMain.Controls.Add(this.lSettlingTime);
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(443, 84);
            this.panelMain.TabIndex = 42;
            //
            // lPrescPer
            //
            this.lPrescPer.Location = new System.Drawing.Point(4, 2);
            this.lPrescPer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lPrescPer.Name = "lPrescPer";
            this.lPrescPer.Size = new System.Drawing.Size(79, 34);
            this.lPrescPer.TabIndex = 45;
            this.lPrescPer.Text = "Prescaler Period ";
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(217, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 35);
            this.label4.TabIndex = 44;
            this.label4.Text = "Connect Inactive Sensors";
            //
            // cbCIS
            //
            this.cbCIS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCIS.FormattingEnabled = true;
            this.cbCIS.Items.AddRange(new object[] {
            "Ground",
            "Hi-Z Analog"});
            this.cbCIS.Location = new System.Drawing.Point(333, 7);
            this.cbCIS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCIS.Name = "cbCIS";
            this.cbCIS.Size = new System.Drawing.Size(100, 24);
            this.cbCIS.TabIndex = 43;
            //
            // tbSettlingTime
            //
            this.tbSettlingTime.Location = new System.Drawing.Point(333, 33);
            this.tbSettlingTime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSettlingTime.Name = "tbSettlingTime";
            this.tbSettlingTime.Size = new System.Drawing.Size(100, 22);
            this.tbSettlingTime.TabIndex = 39;
            this.tbSettlingTime.Text = "6";
            //
            // lScanL
            //
            this.lScanL.AutoSize = true;
            this.lScanL.Location = new System.Drawing.Point(4, 36);
            this.lScanL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lScanL.Name = "lScanL";
            this.lScanL.Size = new System.Drawing.Size(92, 17);
            this.lScanL.TabIndex = 27;
            this.lScanL.Text = "Scan Length ";
            //
            // tbPrescPer
            //
            this.tbPrescPer.Location = new System.Drawing.Point(107, 7);
            this.tbPrescPer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPrescPer.Name = "tbPrescPer";
            this.tbPrescPer.Size = new System.Drawing.Size(100, 22);
            this.tbPrescPer.TabIndex = 37;
            this.tbPrescPer.Text = "1";
            //
            // cbScanLength
            //
            this.cbScanLength.Location = new System.Drawing.Point(107, 31);
            this.cbScanLength.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbScanLength.Name = "cbScanLength";
            this.cbScanLength.Size = new System.Drawing.Size(100, 22);
            this.cbScanLength.TabIndex = 38;
            this.cbScanLength.Text = "6";
            //
            // lIdacR
            //
            this.lIdacR.AutoSize = true;
            this.lIdacR.Location = new System.Drawing.Point(4, 59);
            this.lIdacR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdacR.Name = "lIdacR";
            this.lIdacR.Size = new System.Drawing.Size(84, 17);
            this.lIdacR.TabIndex = 25;
            this.lIdacR.Text = "IDAC range ";
            //
            // cbIDACSetting
            //
            this.cbIDACSetting.Location = new System.Drawing.Point(333, 57);
            this.cbIDACSetting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbIDACSetting.Name = "cbIDACSetting";
            this.cbIDACSetting.Size = new System.Drawing.Size(100, 22);
            this.cbIDACSetting.TabIndex = 40;
            this.cbIDACSetting.Text = "20";
            //
            // lIdadS
            //
            this.lIdadS.AutoSize = true;
            this.lIdadS.Location = new System.Drawing.Point(217, 60);
            this.lIdadS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdadS.Name = "lIdadS";
            this.lIdadS.Size = new System.Drawing.Size(91, 17);
            this.lIdadS.TabIndex = 26;
            this.lIdadS.Text = "IDAC Setting ";
            //
            // cbIDACRange
            //
            this.cbIDACRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIDACRange.FormattingEnabled = true;
            this.cbIDACRange.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbIDACRange.Location = new System.Drawing.Point(107, 56);
            this.cbIDACRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbIDACRange.Name = "cbIDACRange";
            this.cbIDACRange.Size = new System.Drawing.Size(100, 24);
            this.cbIDACRange.TabIndex = 34;
            //
            // lSettlingTime
            //
            this.lSettlingTime.AutoSize = true;
            this.lSettlingTime.Location = new System.Drawing.Point(217, 35);
            this.lSettlingTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lSettlingTime.Name = "lSettlingTime";
            this.lSettlingTime.Size = new System.Drawing.Size(90, 17);
            this.lSettlingTime.TabIndex = 30;
            this.lSettlingTime.Text = "Settling Time";
            //
            // CySSCSAProperties
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelMain);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CySSCSAProperties";
            this.Size = new System.Drawing.Size(447, 84);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TextBox tbSettlingTime;
        private System.Windows.Forms.Label lScanL;
        private System.Windows.Forms.TextBox tbPrescPer;
        private System.Windows.Forms.TextBox cbScanLength;
        private System.Windows.Forms.Label lIdacR;
        private System.Windows.Forms.TextBox cbIDACSetting;
        private System.Windows.Forms.Label lIdadS;
        private System.Windows.Forms.ComboBox cbIDACRange;
        private System.Windows.Forms.Label lSettlingTime;
        private System.Windows.Forms.ComboBox cbCIS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lPrescPer;
    }
}
