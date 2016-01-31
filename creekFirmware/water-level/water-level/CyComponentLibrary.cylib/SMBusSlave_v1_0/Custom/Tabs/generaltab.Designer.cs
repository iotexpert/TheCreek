namespace SMBusSlave_v1_0
{
    partial class CyGeneralTab
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblDataRate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.cbDataRate = new System.Windows.Forms.ComboBox();
            this.gbSmbAlert = new System.Windows.Forms.GroupBox();
            this.rbManualMode = new System.Windows.Forms.RadioButton();
            this.rbAutoMode = new System.Windows.Forms.RadioButton();
            this.chbEnableSmbAlertPin = new System.Windows.Forms.CheckBox();
            this.numPagedCommands = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblActualDataRateValue = new System.Windows.Forms.Label();
            this.gbSmBus = new System.Windows.Forms.GroupBox();
            this.chbSupportQueryCmd = new System.Windows.Forms.CheckBox();
            this.chbSupportPageCmd = new System.Windows.Forms.CheckBox();
            this.chbEnableRecieveByteProtocol = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cyToolStrip = new SMBusSlave_v1_0.CyToolStrip();
            this.tbSlaveAddress = new System.Windows.Forms.TextBox();
            this.gbSmbAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPagedCommands)).BeginInit();
            this.gbSmBus.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mode:";
            // 
            // lblDataRate
            // 
            this.lblDataRate.AutoSize = true;
            this.lblDataRate.Location = new System.Drawing.Point(3, 60);
            this.lblDataRate.Name = "lblDataRate";
            this.lblDataRate.Size = new System.Drawing.Size(86, 13);
            this.lblDataRate.TabIndex = 1;
            this.lblDataRate.Text = "Data rate (kbps):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Slave address:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "SMBus Slave",
            "PMBus Slave"});
            this.cbMode.Location = new System.Drawing.Point(95, 30);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(121, 21);
            this.cbMode.TabIndex = 3;
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // cbDataRate
            // 
            this.cbDataRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataRate.FormattingEnabled = true;
            this.cbDataRate.Location = new System.Drawing.Point(95, 57);
            this.cbDataRate.Name = "cbDataRate";
            this.cbDataRate.Size = new System.Drawing.Size(61, 21);
            this.cbDataRate.TabIndex = 4;
            this.cbDataRate.SelectedIndexChanged += new System.EventHandler(this.cbDataRate_SelectedIndexChanged);
            // 
            // gbSmbAlert
            // 
            this.gbSmbAlert.Controls.Add(this.rbManualMode);
            this.gbSmbAlert.Controls.Add(this.rbAutoMode);
            this.gbSmbAlert.Controls.Add(this.chbEnableSmbAlertPin);
            this.gbSmbAlert.Location = new System.Drawing.Point(6, 110);
            this.gbSmbAlert.Name = "gbSmbAlert";
            this.gbSmbAlert.Size = new System.Drawing.Size(210, 73);
            this.gbSmbAlert.TabIndex = 6;
            this.gbSmbAlert.TabStop = false;
            // 
            // rbManualMode
            // 
            this.rbManualMode.AutoSize = true;
            this.rbManualMode.Location = new System.Drawing.Point(26, 42);
            this.rbManualMode.Name = "rbManualMode";
            this.rbManualMode.Size = new System.Drawing.Size(89, 17);
            this.rbManualMode.TabIndex = 2;
            this.rbManualMode.TabStop = true;
            this.rbManualMode.Text = "Manual mode";
            this.rbManualMode.UseVisualStyleBackColor = true;
            // 
            // rbAutoMode
            // 
            this.rbAutoMode.AutoSize = true;
            this.rbAutoMode.Location = new System.Drawing.Point(26, 19);
            this.rbAutoMode.Name = "rbAutoMode";
            this.rbAutoMode.Size = new System.Drawing.Size(76, 17);
            this.rbAutoMode.TabIndex = 1;
            this.rbAutoMode.TabStop = true;
            this.rbAutoMode.Text = "Auto mode";
            this.rbAutoMode.UseVisualStyleBackColor = true;
            this.rbAutoMode.CheckedChanged += new System.EventHandler(this.rbAutoMode_CheckedChanged);
            // 
            // chbEnableSmbAlertPin
            // 
            this.chbEnableSmbAlertPin.AutoSize = true;
            this.chbEnableSmbAlertPin.Location = new System.Drawing.Point(6, 0);
            this.chbEnableSmbAlertPin.Name = "chbEnableSmbAlertPin";
            this.chbEnableSmbAlertPin.Size = new System.Drawing.Size(144, 17);
            this.chbEnableSmbAlertPin.TabIndex = 0;
            this.chbEnableSmbAlertPin.Text = "Enable SMBALERT# pin";
            this.chbEnableSmbAlertPin.UseVisualStyleBackColor = true;
            this.chbEnableSmbAlertPin.CheckedChanged += new System.EventHandler(this.chbEnableSmbAlertPin_CheckedChanged);
            // 
            // numPagedCommands
            // 
            this.numPagedCommands.Location = new System.Drawing.Point(152, 188);
            this.numPagedCommands.Name = "numPagedCommands";
            this.numPagedCommands.Size = new System.Drawing.Size(47, 20);
            this.numPagedCommands.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Paged commands array size:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(175, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Actual data rate:";
            // 
            // lblActualDataRateValue
            // 
            this.lblActualDataRateValue.AutoSize = true;
            this.lblActualDataRateValue.Location = new System.Drawing.Point(266, 60);
            this.lblActualDataRateValue.Name = "lblActualDataRateValue";
            this.lblActualDataRateValue.Size = new System.Drawing.Size(65, 13);
            this.lblActualDataRateValue.TabIndex = 10;
            this.lblActualDataRateValue.Text = "UNKNOWN";
            // 
            // gbSmBus
            // 
            this.gbSmBus.Controls.Add(this.chbSupportQueryCmd);
            this.gbSmBus.Controls.Add(this.chbSupportPageCmd);
            this.gbSmBus.Controls.Add(this.chbEnableRecieveByteProtocol);
            this.gbSmBus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbSmBus.Location = new System.Drawing.Point(6, 214);
            this.gbSmBus.Name = "gbSmBus";
            this.gbSmBus.Size = new System.Drawing.Size(210, 98);
            this.gbSmBus.TabIndex = 11;
            this.gbSmBus.TabStop = false;
            this.gbSmBus.Text = "SMBus";
            // 
            // chbSupportQueryCmd
            // 
            this.chbSupportQueryCmd.AutoSize = true;
            this.chbSupportQueryCmd.Location = new System.Drawing.Point(9, 70);
            this.chbSupportQueryCmd.Name = "chbSupportQueryCmd";
            this.chbSupportQueryCmd.Size = new System.Drawing.Size(153, 17);
            this.chbSupportQueryCmd.TabIndex = 4;
            this.chbSupportQueryCmd.Text = "Support QUERY command";
            this.chbSupportQueryCmd.UseVisualStyleBackColor = true;
            this.chbSupportQueryCmd.CheckedChanged += new System.EventHandler(this.chbSupportQueryCmd_CheckedChanged);
            // 
            // chbSupportPageCmd
            // 
            this.chbSupportPageCmd.AutoSize = true;
            this.chbSupportPageCmd.Location = new System.Drawing.Point(9, 47);
            this.chbSupportPageCmd.Name = "chbSupportPageCmd";
            this.chbSupportPageCmd.Size = new System.Drawing.Size(144, 17);
            this.chbSupportPageCmd.TabIndex = 1;
            this.chbSupportPageCmd.Text = "Support PAGE command";
            this.chbSupportPageCmd.UseVisualStyleBackColor = true;
            this.chbSupportPageCmd.CheckedChanged += new System.EventHandler(this.chbSupportPageCmd_CheckedChanged);
            // 
            // chbEnableRecieveByteProtocol
            // 
            this.chbEnableRecieveByteProtocol.AutoSize = true;
            this.chbEnableRecieveByteProtocol.Location = new System.Drawing.Point(9, 24);
            this.chbEnableRecieveByteProtocol.Name = "chbEnableRecieveByteProtocol";
            this.chbEnableRecieveByteProtocol.Size = new System.Drawing.Size(161, 17);
            this.chbEnableRecieveByteProtocol.TabIndex = 0;
            this.chbEnableRecieveByteProtocol.Text = "Enable recieve byte protocol";
            this.chbEnableRecieveByteProtocol.UseVisualStyleBackColor = true;
            this.chbEnableRecieveByteProtocol.CheckedChanged += new System.EventHandler(this.chbEnableRecieveByteProtocol_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(175, 86);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "(Use \"0x\" for hex)";
            // 
            // cyToolStrip
            // 
            this.cyToolStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.cyToolStrip.GeneralView = true;
            this.cyToolStrip.Location = new System.Drawing.Point(0, 0);
            this.cyToolStrip.Margin = new System.Windows.Forms.Padding(4);
            this.cyToolStrip.Name = "cyToolStrip";
            this.cyToolStrip.Size = new System.Drawing.Size(683, 25);
            this.cyToolStrip.TabIndex = 19;
            // 
            // tbSlaveAddress
            // 
            this.tbSlaveAddress.Location = new System.Drawing.Point(95, 83);
            this.tbSlaveAddress.Name = "tbSlaveAddress";
            this.tbSlaveAddress.Size = new System.Drawing.Size(61, 20);
            this.tbSlaveAddress.TabIndex = 20;
            this.tbSlaveAddress.TextChanged += new System.EventHandler(this.tbSlaveAddress_TextChanged);
            this.tbSlaveAddress.Validating += new System.ComponentModel.CancelEventHandler(this.tbSlaveAddress_Validating);
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbSlaveAddress);
            this.Controls.Add(this.numPagedCommands);
            this.Controls.Add(this.cyToolStrip);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.gbSmBus);
            this.Controls.Add(this.lblActualDataRateValue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.gbSmbAlert);
            this.Controls.Add(this.cbDataRate);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDataRate);
            this.Controls.Add(this.label1);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(683, 457);
            this.Load += new System.EventHandler(this.CyGeneralTab_Load);
            this.gbSmbAlert.ResumeLayout(false);
            this.gbSmbAlert.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPagedCommands)).EndInit();
            this.gbSmBus.ResumeLayout(false);
            this.gbSmBus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDataRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.ComboBox cbDataRate;
        private System.Windows.Forms.GroupBox gbSmbAlert;
        private System.Windows.Forms.RadioButton rbManualMode;
        private System.Windows.Forms.RadioButton rbAutoMode;
        private System.Windows.Forms.CheckBox chbEnableSmbAlertPin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblActualDataRateValue;
        private System.Windows.Forms.GroupBox gbSmBus;
        private System.Windows.Forms.CheckBox chbSupportQueryCmd;
        private System.Windows.Forms.CheckBox chbSupportPageCmd;
        private System.Windows.Forms.CheckBox chbEnableRecieveByteProtocol;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numPagedCommands;
        private CyToolStrip cyToolStrip;
        private System.Windows.Forms.TextBox tbSlaveAddress;
    }
}
