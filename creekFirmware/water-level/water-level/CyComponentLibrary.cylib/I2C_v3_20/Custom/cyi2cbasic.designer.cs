namespace I2C_v3_20
{
    partial class CyI2CBasic
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelPinConnections = new System.Windows.Forms.GroupBox();
            this.rbI2C1 = new System.Windows.Forms.RadioButton();
            this.rbAny = new System.Windows.Forms.RadioButton();
            this.rbI2C0 = new System.Windows.Forms.RadioButton();
            this.panelUDBClockSource = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPlusTolerance = new System.Windows.Forms.TextBox();
            this.tbMinusTolerance = new System.Windows.Forms.TextBox();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.rbInternalClock = new System.Windows.Forms.RadioButton();
            this.rbExternalClock = new System.Windows.Forms.RadioButton();
            this.panelAddressDecode = new System.Windows.Forms.GroupBox();
            this.rbHardware = new System.Windows.Forms.RadioButton();
            this.rbSoftware = new System.Windows.Forms.RadioButton();
            this.lblCalcDataRateValue = new System.Windows.Forms.Label();
            this.lblCalcDataRate = new System.Windows.Forms.Label();
            this.lblHexademical = new System.Windows.Forms.Label();
            this.panelImplementation = new System.Windows.Forms.GroupBox();
            this.rbFixedFunction = new System.Windows.Forms.RadioButton();
            this.rbUDB = new System.Windows.Forms.RadioButton();
            this.tbSlaveAddress = new System.Windows.Forms.TextBox();
            this.lblSlaveAddress = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.chbWakeup = new System.Windows.Forms.CheckBox();
            this.cbDataRate = new System.Windows.Forms.ComboBox();
            this.lblDataRate = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.chbFixedPlacement = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.panelPinConnections.SuspendLayout();
            this.panelUDBClockSource.SuspendLayout();
            this.panelAddressDecode.SuspendLayout();
            this.panelImplementation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelPinConnections);
            this.groupBox1.Controls.Add(this.panelUDBClockSource);
            this.groupBox1.Controls.Add(this.panelAddressDecode);
            this.groupBox1.Controls.Add(this.lblCalcDataRateValue);
            this.groupBox1.Controls.Add(this.lblCalcDataRate);
            this.groupBox1.Controls.Add(this.lblHexademical);
            this.groupBox1.Controls.Add(this.panelImplementation);
            this.groupBox1.Controls.Add(this.tbSlaveAddress);
            this.groupBox1.Controls.Add(this.lblSlaveAddress);
            this.groupBox1.Controls.Add(this.cbMode);
            this.groupBox1.Controls.Add(this.lblMode);
            this.groupBox1.Controls.Add(this.chbWakeup);
            this.groupBox1.Controls.Add(this.cbDataRate);
            this.groupBox1.Controls.Add(this.lblDataRate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(514, 210);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panelPinConnections
            // 
            this.panelPinConnections.Controls.Add(this.rbI2C1);
            this.panelPinConnections.Controls.Add(this.rbAny);
            this.panelPinConnections.Controls.Add(this.rbI2C0);
            this.panelPinConnections.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelPinConnections.Location = new System.Drawing.Point(224, 112);
            this.panelPinConnections.Name = "panelPinConnections";
            this.panelPinConnections.Size = new System.Drawing.Size(63, 91);
            this.panelPinConnections.TabIndex = 28;
            this.panelPinConnections.TabStop = false;
            this.panelPinConnections.Text = "Pins";
            // 
            // rbI2C1
            // 
            this.rbI2C1.AutoSize = true;
            this.rbI2C1.Location = new System.Drawing.Point(5, 63);
            this.rbI2C1.Margin = new System.Windows.Forms.Padding(2);
            this.rbI2C1.Name = "rbI2C1";
            this.rbI2C1.Size = new System.Drawing.Size(47, 17);
            this.rbI2C1.TabIndex = 22;
            this.rbI2C1.TabStop = true;
            this.rbI2C1.Text = "I2C1";
            this.rbI2C1.UseVisualStyleBackColor = true;
            this.rbI2C1.CheckedChanged += new System.EventHandler(this.rbPinConnections_CheckedChanged);
            // 
            // rbAny
            // 
            this.rbAny.AutoSize = true;
            this.errorProvider.SetIconAlignment(this.rbAny, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.rbAny.Location = new System.Drawing.Point(5, 21);
            this.rbAny.Margin = new System.Windows.Forms.Padding(2);
            this.rbAny.Name = "rbAny";
            this.rbAny.Size = new System.Drawing.Size(43, 17);
            this.rbAny.TabIndex = 20;
            this.rbAny.TabStop = true;
            this.rbAny.Text = "Any";
            this.rbAny.UseVisualStyleBackColor = true;
            this.rbAny.CheckedChanged += new System.EventHandler(this.rbPinConnections_CheckedChanged);
            // 
            // rbI2C0
            // 
            this.rbI2C0.AutoSize = true;
            this.rbI2C0.Location = new System.Drawing.Point(5, 42);
            this.rbI2C0.Margin = new System.Windows.Forms.Padding(2);
            this.rbI2C0.Name = "rbI2C0";
            this.rbI2C0.Size = new System.Drawing.Size(47, 17);
            this.rbI2C0.TabIndex = 21;
            this.rbI2C0.TabStop = true;
            this.rbI2C0.Text = "I2C0";
            this.rbI2C0.UseVisualStyleBackColor = true;
            this.rbI2C0.CheckedChanged += new System.EventHandler(this.rbPinConnections_CheckedChanged);
            // 
            // panelUDBClockSource
            // 
            this.panelUDBClockSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelUDBClockSource.Controls.Add(this.label9);
            this.panelUDBClockSource.Controls.Add(this.label5);
            this.panelUDBClockSource.Controls.Add(this.tbPlusTolerance);
            this.panelUDBClockSource.Controls.Add(this.tbMinusTolerance);
            this.panelUDBClockSource.Controls.Add(this.lblTolerance);
            this.panelUDBClockSource.Controls.Add(this.rbInternalClock);
            this.panelUDBClockSource.Controls.Add(this.rbExternalClock);
            this.panelUDBClockSource.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelUDBClockSource.Location = new System.Drawing.Point(293, 112);
            this.panelUDBClockSource.Name = "panelUDBClockSource";
            this.panelUDBClockSource.Size = new System.Drawing.Size(215, 91);
            this.panelUDBClockSource.TabIndex = 26;
            this.panelUDBClockSource.TabStop = false;
            this.panelUDBClockSource.Text = "UDB Clock Source";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(103, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "-";
            // 
            // tbPlusTolerance
            // 
            this.tbPlusTolerance.Location = new System.Drawing.Point(117, 62);
            this.tbPlusTolerance.Name = "tbPlusTolerance";
            this.tbPlusTolerance.Size = new System.Drawing.Size(31, 20);
            this.tbPlusTolerance.TabIndex = 32;
            this.tbPlusTolerance.TextChanged += new System.EventHandler(this.tbTolerance_TextChanged);
            this.tbPlusTolerance.Validating += new System.ComponentModel.CancelEventHandler(this.tbTolerance_Validating);
            // 
            // tbMinusTolerance
            // 
            this.errorProvider.SetIconAlignment(this.tbMinusTolerance, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.tbMinusTolerance.Location = new System.Drawing.Point(71, 62);
            this.tbMinusTolerance.Name = "tbMinusTolerance";
            this.tbMinusTolerance.Size = new System.Drawing.Size(31, 20);
            this.tbMinusTolerance.TabIndex = 31;
            this.tbMinusTolerance.TextChanged += new System.EventHandler(this.tbTolerance_TextChanged);
            this.tbMinusTolerance.Validating += new System.ComponentModel.CancelEventHandler(this.tbTolerance_Validating);
            // 
            // lblTolerance
            // 
            this.lblTolerance.AutoSize = true;
            this.lblTolerance.Location = new System.Drawing.Point(2, 65);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(58, 13);
            this.lblTolerance.TabIndex = 28;
            this.lblTolerance.Text = "Tolerance:";
            // 
            // rbInternalClock
            // 
            this.rbInternalClock.AutoSize = true;
            this.errorProvider.SetIconAlignment(this.rbInternalClock, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.rbInternalClock.Location = new System.Drawing.Point(5, 42);
            this.rbInternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.rbInternalClock.Name = "rbInternalClock";
            this.rbInternalClock.Size = new System.Drawing.Size(90, 17);
            this.rbInternalClock.TabIndex = 2;
            this.rbInternalClock.Text = "Internal Clock";
            this.rbInternalClock.UseVisualStyleBackColor = true;
            this.rbInternalClock.CheckedChanged += new System.EventHandler(this.rbInternalClock_CheckedChanged);
            // 
            // rbExternalClock
            // 
            this.rbExternalClock.AutoSize = true;
            this.rbExternalClock.Location = new System.Drawing.Point(5, 21);
            this.rbExternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.rbExternalClock.Name = "rbExternalClock";
            this.rbExternalClock.Size = new System.Drawing.Size(93, 17);
            this.rbExternalClock.TabIndex = 3;
            this.rbExternalClock.Text = "External Clock";
            this.rbExternalClock.UseVisualStyleBackColor = true;
            // 
            // panelAddressDecode
            // 
            this.panelAddressDecode.Controls.Add(this.rbHardware);
            this.panelAddressDecode.Controls.Add(this.rbSoftware);
            this.panelAddressDecode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelAddressDecode.Location = new System.Drawing.Point(113, 112);
            this.panelAddressDecode.Name = "panelAddressDecode";
            this.panelAddressDecode.Size = new System.Drawing.Size(105, 91);
            this.panelAddressDecode.TabIndex = 27;
            this.panelAddressDecode.TabStop = false;
            this.panelAddressDecode.Text = "Address Decode";
            // 
            // rbHardware
            // 
            this.rbHardware.AutoSize = true;
            this.rbHardware.Location = new System.Drawing.Point(6, 30);
            this.rbHardware.Name = "rbHardware";
            this.rbHardware.Size = new System.Drawing.Size(71, 17);
            this.rbHardware.TabIndex = 0;
            this.rbHardware.TabStop = true;
            this.rbHardware.Text = "Hardware";
            this.rbHardware.UseVisualStyleBackColor = true;
            this.rbHardware.CheckedChanged += new System.EventHandler(this.rbHardware_CheckedChanged);
            // 
            // rbSoftware
            // 
            this.rbSoftware.AutoSize = true;
            this.rbSoftware.Location = new System.Drawing.Point(6, 53);
            this.rbSoftware.Name = "rbSoftware";
            this.rbSoftware.Size = new System.Drawing.Size(67, 17);
            this.rbSoftware.TabIndex = 1;
            this.rbSoftware.TabStop = true;
            this.rbSoftware.Text = "Software";
            this.rbSoftware.UseVisualStyleBackColor = true;
            // 
            // lblCalcDataRateValue
            // 
            this.lblCalcDataRateValue.AutoSize = true;
            this.lblCalcDataRateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCalcDataRateValue.Location = new System.Drawing.Point(240, 42);
            this.lblCalcDataRateValue.Name = "lblCalcDataRateValue";
            this.lblCalcDataRateValue.Size = new System.Drawing.Size(65, 13);
            this.lblCalcDataRateValue.TabIndex = 27;
            this.lblCalcDataRateValue.Text = "UNKNOWN";
            // 
            // lblCalcDataRate
            // 
            this.lblCalcDataRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCalcDataRate.Location = new System.Drawing.Point(153, 42);
            this.lblCalcDataRate.Name = "lblCalcDataRate";
            this.lblCalcDataRate.Size = new System.Drawing.Size(152, 14);
            this.lblCalcDataRate.TabIndex = 26;
            this.lblCalcDataRate.Text = "Actual Data Rate:";
            // 
            // lblHexademical
            // 
            this.lblHexademical.AutoSize = true;
            this.lblHexademical.Location = new System.Drawing.Point(153, 66);
            this.lblHexademical.Name = "lblHexademical";
            this.lblHexademical.Size = new System.Drawing.Size(83, 13);
            this.lblHexademical.TabIndex = 16;
            this.lblHexademical.Text = "(use \'0x\' for hex)";
            // 
            // panelImplementation
            // 
            this.panelImplementation.Controls.Add(this.rbFixedFunction);
            this.panelImplementation.Controls.Add(this.rbUDB);
            this.panelImplementation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelImplementation.Location = new System.Drawing.Point(6, 112);
            this.panelImplementation.Name = "panelImplementation";
            this.panelImplementation.Size = new System.Drawing.Size(101, 91);
            this.panelImplementation.TabIndex = 25;
            this.panelImplementation.TabStop = false;
            this.panelImplementation.Text = "Implementation";
            // 
            // rbFixedFunction
            // 
            this.rbFixedFunction.AutoSize = true;
            this.rbFixedFunction.Location = new System.Drawing.Point(6, 30);
            this.rbFixedFunction.Name = "rbFixedFunction";
            this.rbFixedFunction.Size = new System.Drawing.Size(94, 17);
            this.rbFixedFunction.TabIndex = 0;
            this.rbFixedFunction.TabStop = true;
            this.rbFixedFunction.Text = "Fixed Function";
            this.rbFixedFunction.UseVisualStyleBackColor = true;
            this.rbFixedFunction.CheckedChanged += new System.EventHandler(this.rbFixedFunction_CheckedChanged);
            // 
            // rbUDB
            // 
            this.rbUDB.AutoSize = true;
            this.rbUDB.Location = new System.Drawing.Point(6, 53);
            this.rbUDB.Name = "rbUDB";
            this.rbUDB.Size = new System.Drawing.Size(48, 17);
            this.rbUDB.TabIndex = 1;
            this.rbUDB.TabStop = true;
            this.rbUDB.Text = "UDB";
            this.rbUDB.UseVisualStyleBackColor = true;
            // 
            // tbSlaveAddress
            // 
            this.errorProvider.SetIconAlignment(this.tbSlaveAddress, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.tbSlaveAddress.Location = new System.Drawing.Point(97, 65);
            this.tbSlaveAddress.MaxLength = 5;
            this.tbSlaveAddress.Name = "tbSlaveAddress";
            this.tbSlaveAddress.Size = new System.Drawing.Size(50, 20);
            this.tbSlaveAddress.TabIndex = 5;
            this.tbSlaveAddress.TextChanged += new System.EventHandler(this.tbSlaveAddress_TextChanged);
            this.tbSlaveAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.tbSlaveAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.tbSlaveAddress.Validating += new System.ComponentModel.CancelEventHandler(this.tbSlaveAddress_Validating);
            // 
            // lblSlaveAddress
            // 
            this.lblSlaveAddress.AutoSize = true;
            this.lblSlaveAddress.Location = new System.Drawing.Point(3, 66);
            this.lblSlaveAddress.Name = "lblSlaveAddress";
            this.lblSlaveAddress.Size = new System.Drawing.Size(78, 13);
            this.lblSlaveAddress.TabIndex = 15;
            this.lblSlaveAddress.Text = "Slave Address:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbMode, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbMode.Location = new System.Drawing.Point(97, 13);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(134, 21);
            this.cbMode.TabIndex = 0;
            this.cbMode.Validating += new System.ComponentModel.CancelEventHandler(this.cbDataRate_Validating);
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(3, 16);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(37, 13);
            this.lblMode.TabIndex = 12;
            this.lblMode.Text = "Mode:";
            // 
            // chbWakeup
            // 
            this.chbWakeup.AutoSize = true;
            this.errorProvider.SetIconAlignment(this.chbWakeup, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.chbWakeup.Location = new System.Drawing.Point(97, 90);
            this.chbWakeup.Margin = new System.Windows.Forms.Padding(2);
            this.chbWakeup.Name = "chbWakeup";
            this.chbWakeup.Size = new System.Drawing.Size(183, 17);
            this.chbWakeup.TabIndex = 6;
            this.chbWakeup.Text = "Enable wakeup from Sleep Mode";
            this.chbWakeup.UseVisualStyleBackColor = true;
            this.chbWakeup.CheckedChanged += new System.EventHandler(this.checkBoxWakeup_CheckedChanged);
            // 
            // cbDataRate
            // 
            this.cbDataRate.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbDataRate, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbDataRate.Items.AddRange(new object[] {
            "50",
            "100",
            "400",
            "1000"});
            this.cbDataRate.Location = new System.Drawing.Point(97, 39);
            this.cbDataRate.Margin = new System.Windows.Forms.Padding(2);
            this.cbDataRate.MaxLength = 10;
            this.cbDataRate.Name = "cbDataRate";
            this.cbDataRate.Size = new System.Drawing.Size(50, 21);
            this.cbDataRate.TabIndex = 1;
            this.cbDataRate.Validating += new System.ComponentModel.CancelEventHandler(this.cbDataRate_Validating);
            this.cbDataRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.cbDataRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.cbDataRate.TextChanged += new System.EventHandler(this.cbDataRate_TextChanged);
            // 
            // lblDataRate
            // 
            this.lblDataRate.AutoSize = true;
            this.lblDataRate.Location = new System.Drawing.Point(3, 42);
            this.lblDataRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDataRate.Name = "lblDataRate";
            this.lblDataRate.Size = new System.Drawing.Size(91, 13);
            this.lblDataRate.TabIndex = 0;
            this.lblDataRate.Text = "Data Rate (kbps):";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // chbFixedPlacement
            // 
            this.chbFixedPlacement.AutoSize = true;
            this.chbFixedPlacement.Location = new System.Drawing.Point(6, 218);
            this.chbFixedPlacement.Name = "chbFixedPlacement";
            this.chbFixedPlacement.Size = new System.Drawing.Size(196, 17);
            this.chbFixedPlacement.TabIndex = 1;
            this.chbFixedPlacement.Text = "Enable UDB Slave Fixed Placement";
            this.chbFixedPlacement.UseVisualStyleBackColor = true;
            this.chbFixedPlacement.CheckedChanged += new System.EventHandler(this.chbFixedPlacement_CheckedChanged);
            // 
            // CyI2CBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.chbFixedPlacement);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyI2CBasic";
            this.Size = new System.Drawing.Size(514, 240);
            this.Load += new System.EventHandler(this.CyI2CBasic_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelPinConnections.ResumeLayout(false);
            this.panelPinConnections.PerformLayout();
            this.panelUDBClockSource.ResumeLayout(false);
            this.panelUDBClockSource.PerformLayout();
            this.panelAddressDecode.ResumeLayout(false);
            this.panelAddressDecode.PerformLayout();
            this.panelImplementation.ResumeLayout(false);
            this.panelImplementation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDataRate;
        public System.Windows.Forms.ComboBox cbDataRate;
        public System.Windows.Forms.CheckBox chbWakeup;
        public System.Windows.Forms.TextBox tbSlaveAddress;
        private System.Windows.Forms.Label lblSlaveAddress;
        public System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label lblMode;
        public System.Windows.Forms.RadioButton rbFixedFunction;
        public System.Windows.Forms.RadioButton rbUDB;
        public System.Windows.Forms.RadioButton rbSoftware;
        public System.Windows.Forms.RadioButton rbHardware;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblHexademical;
        private System.Windows.Forms.RadioButton rbExternalClock;
        private System.Windows.Forms.RadioButton rbInternalClock;
        private System.Windows.Forms.RadioButton rbI2C1;
        private System.Windows.Forms.RadioButton rbI2C0;
        private System.Windows.Forms.RadioButton rbAny;
        private System.Windows.Forms.Label lblCalcDataRateValue;
        private System.Windows.Forms.Label lblCalcDataRate;
        private System.Windows.Forms.GroupBox panelImplementation;
        private System.Windows.Forms.GroupBox panelPinConnections;
        private System.Windows.Forms.GroupBox panelAddressDecode;
        private System.Windows.Forms.GroupBox panelUDBClockSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPlusTolerance;
        private System.Windows.Forms.TextBox tbMinusTolerance;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.CheckBox chbFixedPlacement;
    }
}
