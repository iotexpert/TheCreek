namespace SMBusSlave_v1_0
{
    partial class CyI2cConfigTab
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
            this.gbPinConnections = new System.Windows.Forms.GroupBox();
            this.rbI2C1 = new System.Windows.Forms.RadioButton();
            this.rbAny = new System.Windows.Forms.RadioButton();
            this.rbI2C0 = new System.Windows.Forms.RadioButton();
            this.gbUDBClockSource = new System.Windows.Forms.GroupBox();
            this.panelTolerance = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.numPlusTolerance = new SMBusSlave_v1_0.CyPercentageUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numMinusTolerance = new SMBusSlave_v1_0.CyPercentageUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.rbInternalClock = new System.Windows.Forms.RadioButton();
            this.rbExternalClock = new System.Windows.Forms.RadioButton();
            this.gbAddressDecode = new System.Windows.Forms.GroupBox();
            this.rbHardware = new System.Windows.Forms.RadioButton();
            this.rbSoftware = new System.Windows.Forms.RadioButton();
            this.gbImplementation = new System.Windows.Forms.GroupBox();
            this.rbFixedFunction = new System.Windows.Forms.RadioButton();
            this.rbUDB = new System.Windows.Forms.RadioButton();
            this.chbFixedPlacement = new System.Windows.Forms.CheckBox();
            this.m_chbExternalIOBuff = new System.Windows.Forms.CheckBox();
            this.gbPinConnections.SuspendLayout();
            this.gbUDBClockSource.SuspendLayout();
            this.panelTolerance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPlusTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinusTolerance)).BeginInit();
            this.gbAddressDecode.SuspendLayout();
            this.gbImplementation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPinConnections
            // 
            this.gbPinConnections.Controls.Add(this.rbI2C1);
            this.gbPinConnections.Controls.Add(this.rbAny);
            this.gbPinConnections.Controls.Add(this.rbI2C0);
            this.gbPinConnections.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbPinConnections.Location = new System.Drawing.Point(225, 5);
            this.gbPinConnections.Name = "gbPinConnections";
            this.gbPinConnections.Size = new System.Drawing.Size(63, 91);
            this.gbPinConnections.TabIndex = 33;
            this.gbPinConnections.TabStop = false;
            this.gbPinConnections.Text = "Pins";
            // 
            // rbI2C1
            // 
            this.rbI2C1.AutoSize = true;
            this.rbI2C1.Location = new System.Drawing.Point(5, 60);
            this.rbI2C1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbI2C1.Name = "rbI2C1";
            this.rbI2C1.Size = new System.Drawing.Size(35, 14);
            this.rbI2C1.TabIndex = 22;
            this.rbI2C1.TabStop = true;
            this.rbI2C1.Text = "I2C1";
            this.rbI2C1.UseVisualStyleBackColor = true;
            this.rbI2C1.CheckedChanged += new System.EventHandler(this.rbPins_CheckedChanged);
            // 
            // rbAny
            // 
            this.rbAny.AutoSize = true;
            this.rbAny.Checked = true;
            this.rbAny.Location = new System.Drawing.Point(5, 18);
            this.rbAny.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbAny.Name = "rbAny";
            this.rbAny.Size = new System.Drawing.Size(32, 14);
            this.rbAny.TabIndex = 20;
            this.rbAny.TabStop = true;
            this.rbAny.Text = "Any";
            this.rbAny.UseVisualStyleBackColor = true;
            this.rbAny.CheckedChanged += new System.EventHandler(this.rbPins_CheckedChanged);
            // 
            // rbI2C0
            // 
            this.rbI2C0.AutoSize = true;
            this.rbI2C0.Location = new System.Drawing.Point(5, 39);
            this.rbI2C0.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbI2C0.Name = "rbI2C0";
            this.rbI2C0.Size = new System.Drawing.Size(35, 14);
            this.rbI2C0.TabIndex = 21;
            this.rbI2C0.TabStop = true;
            this.rbI2C0.Text = "I2C0";
            this.rbI2C0.UseVisualStyleBackColor = true;
            this.rbI2C0.CheckedChanged += new System.EventHandler(this.rbPins_CheckedChanged);
            // 
            // gbUDBClockSource
            // 
            this.gbUDBClockSource.Controls.Add(this.panelTolerance);
            this.gbUDBClockSource.Controls.Add(this.rbInternalClock);
            this.gbUDBClockSource.Controls.Add(this.rbExternalClock);
            this.gbUDBClockSource.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbUDBClockSource.Location = new System.Drawing.Point(294, 5);
            this.gbUDBClockSource.Name = "gbUDBClockSource";
            this.gbUDBClockSource.Size = new System.Drawing.Size(273, 91);
            this.gbUDBClockSource.TabIndex = 31;
            this.gbUDBClockSource.TabStop = false;
            this.gbUDBClockSource.Text = "UDB source";
            // 
            // panelTolerance
            // 
            this.panelTolerance.Controls.Add(this.label1);
            this.panelTolerance.Controls.Add(this.lblTolerance);
            this.panelTolerance.Controls.Add(this.numPlusTolerance);
            this.panelTolerance.Controls.Add(this.label5);
            this.panelTolerance.Controls.Add(this.numMinusTolerance);
            this.panelTolerance.Controls.Add(this.label9);
            this.panelTolerance.Location = new System.Drawing.Point(53, 57);
            this.panelTolerance.Name = "panelTolerance";
            this.panelTolerance.Size = new System.Drawing.Size(214, 20);
            this.panelTolerance.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = ")";
            // 
            // lblTolerance
            // 
            this.lblTolerance.AutoSize = true;
            this.lblTolerance.Location = new System.Drawing.Point(3, 2);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(64, 13);
            this.lblTolerance.TabIndex = 28;
            this.lblTolerance.Text = "( Tolerance:";
            // 
            // numPlusTolerance
            // 
            this.numPlusTolerance.DecimalPlaces = 1;
            this.numPlusTolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numPlusTolerance.Location = new System.Drawing.Point(154, 0);
            this.numPlusTolerance.Name = "numPlusTolerance";
            this.numPlusTolerance.Size = new System.Drawing.Size(48, 20);
            this.numPlusTolerance.TabIndex = 36;
            this.numPlusTolerance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "-";
            // 
            // numMinusTolerance
            // 
            this.numMinusTolerance.DecimalPlaces = 1;
            this.numMinusTolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numMinusTolerance.Location = new System.Drawing.Point(84, 0);
            this.numMinusTolerance.Name = "numMinusTolerance";
            this.numMinusTolerance.Size = new System.Drawing.Size(48, 20);
            this.numMinusTolerance.TabIndex = 35;
            this.numMinusTolerance.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(140, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "+";
            // 
            // rbInternalClock
            // 
            this.rbInternalClock.AutoSize = true;
            this.rbInternalClock.Location = new System.Drawing.Point(5, 39);
            this.rbInternalClock.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbInternalClock.Name = "rbInternalClock";
            this.rbInternalClock.Size = new System.Drawing.Size(89, 17);
            this.rbInternalClock.TabIndex = 2;
            this.rbInternalClock.Text = "Internal clock";
            this.rbInternalClock.UseVisualStyleBackColor = true;
            // 
            // rbExternalClock
            // 
            this.rbExternalClock.AutoSize = true;
            this.rbExternalClock.Location = new System.Drawing.Point(5, 18);
            this.rbExternalClock.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbExternalClock.Name = "rbExternalClock";
            this.rbExternalClock.Size = new System.Drawing.Size(92, 17);
            this.rbExternalClock.TabIndex = 3;
            this.rbExternalClock.Text = "External clock";
            this.rbExternalClock.UseVisualStyleBackColor = true;
            this.rbExternalClock.CheckedChanged += new System.EventHandler(this.rbExternalClock_CheckedChanged);
            // 
            // gbAddressDecode
            // 
            this.gbAddressDecode.Controls.Add(this.rbHardware);
            this.gbAddressDecode.Controls.Add(this.rbSoftware);
            this.gbAddressDecode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbAddressDecode.Location = new System.Drawing.Point(119, 5);
            this.gbAddressDecode.Name = "gbAddressDecode";
            this.gbAddressDecode.Size = new System.Drawing.Size(100, 91);
            this.gbAddressDecode.TabIndex = 32;
            this.gbAddressDecode.TabStop = false;
            this.gbAddressDecode.Text = "Address decode";
            // 
            // rbHardware
            // 
            this.rbHardware.AutoSize = true;
            this.rbHardware.Location = new System.Drawing.Point(6, 18);
            this.rbHardware.Name = "rbHardware";
            this.rbHardware.Size = new System.Drawing.Size(53, 14);
            this.rbHardware.TabIndex = 0;
            this.rbHardware.TabStop = true;
            this.rbHardware.Text = "Hardware";
            this.rbHardware.UseVisualStyleBackColor = true;
            this.rbHardware.CheckedChanged += new System.EventHandler(this.rbHardware_CheckedChanged);
            // 
            // rbSoftware
            // 
            this.rbSoftware.AutoSize = true;
            this.rbSoftware.Location = new System.Drawing.Point(6, 39);
            this.rbSoftware.Name = "rbSoftware";
            this.rbSoftware.Size = new System.Drawing.Size(50, 14);
            this.rbSoftware.TabIndex = 1;
            this.rbSoftware.TabStop = true;
            this.rbSoftware.Text = "Software";
            this.rbSoftware.UseVisualStyleBackColor = true;
            // 
            // gbImplementation
            // 
            this.gbImplementation.Controls.Add(this.rbFixedFunction);
            this.gbImplementation.Controls.Add(this.rbUDB);
            this.gbImplementation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gbImplementation.Location = new System.Drawing.Point(3, 5);
            this.gbImplementation.Name = "gbImplementation";
            this.gbImplementation.Size = new System.Drawing.Size(110, 91);
            this.gbImplementation.TabIndex = 30;
            this.gbImplementation.TabStop = false;
            this.gbImplementation.Text = "Implementation";
            // 
            // rbFixedFunction
            // 
            this.rbFixedFunction.AutoSize = true;
            this.rbFixedFunction.Location = new System.Drawing.Point(6, 18);
            this.rbFixedFunction.Name = "rbFixedFunction";
            this.rbFixedFunction.Size = new System.Drawing.Size(68, 14);
            this.rbFixedFunction.TabIndex = 0;
            this.rbFixedFunction.TabStop = true;
            this.rbFixedFunction.Text = "Fixed function";
            this.rbFixedFunction.UseVisualStyleBackColor = true;
            this.rbFixedFunction.CheckedChanged += new System.EventHandler(this.rbFixedFunction_CheckedChanged);
            // 
            // rbUDB
            // 
            this.rbUDB.AutoSize = true;
            this.rbUDB.Location = new System.Drawing.Point(6, 39);
            this.rbUDB.Name = "rbUDB";
            this.rbUDB.Size = new System.Drawing.Size(36, 14);
            this.rbUDB.TabIndex = 1;
            this.rbUDB.TabStop = true;
            this.rbUDB.Text = "UDB";
            this.rbUDB.UseVisualStyleBackColor = true;
            // 
            // chbFixedPlacement
            // 
            this.chbFixedPlacement.AutoSize = true;
            this.chbFixedPlacement.Location = new System.Drawing.Point(3, 102);
            this.chbFixedPlacement.Name = "chbFixedPlacement";
            this.chbFixedPlacement.Size = new System.Drawing.Size(190, 17);
            this.chbFixedPlacement.TabIndex = 34;
            this.chbFixedPlacement.Text = "Enable UDB slave fixed placement";
            this.chbFixedPlacement.UseVisualStyleBackColor = true;
            this.chbFixedPlacement.CheckedChanged += new System.EventHandler(this.chbFixedPlacement_CheckedChanged);
            // 
            // m_chbExternalIOBuff
            // 
            this.m_chbExternalIOBuff.AutoSize = true;
            this.m_chbExternalIOBuff.Location = new System.Drawing.Point(225, 102);
            this.m_chbExternalIOBuff.Name = "m_chbExternalIOBuff";
            this.m_chbExternalIOBuff.Size = new System.Drawing.Size(108, 17);
            this.m_chbExternalIOBuff.TabIndex = 35;
            this.m_chbExternalIOBuff.Text = "External IO buffer";
            this.m_chbExternalIOBuff.UseVisualStyleBackColor = true;
            // 
            // CyI2cConfigTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_chbExternalIOBuff);
            this.Controls.Add(this.chbFixedPlacement);
            this.Controls.Add(this.gbPinConnections);
            this.Controls.Add(this.gbUDBClockSource);
            this.Controls.Add(this.gbAddressDecode);
            this.Controls.Add(this.gbImplementation);
            this.Name = "CyI2cConfigTab";
            this.Size = new System.Drawing.Size(723, 363);
            this.gbPinConnections.ResumeLayout(false);
            this.gbPinConnections.PerformLayout();
            this.gbUDBClockSource.ResumeLayout(false);
            this.gbUDBClockSource.PerformLayout();
            this.panelTolerance.ResumeLayout(false);
            this.panelTolerance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPlusTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinusTolerance)).EndInit();
            this.gbAddressDecode.ResumeLayout(false);
            this.gbAddressDecode.PerformLayout();
            this.gbImplementation.ResumeLayout(false);
            this.gbImplementation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPinConnections;
        private System.Windows.Forms.RadioButton rbI2C1;
        private System.Windows.Forms.RadioButton rbAny;
        private System.Windows.Forms.RadioButton rbI2C0;
        private System.Windows.Forms.GroupBox gbUDBClockSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.RadioButton rbInternalClock;
        private System.Windows.Forms.RadioButton rbExternalClock;
        private System.Windows.Forms.GroupBox gbAddressDecode;
        public System.Windows.Forms.RadioButton rbHardware;
        public System.Windows.Forms.RadioButton rbSoftware;
        private System.Windows.Forms.GroupBox gbImplementation;
        public System.Windows.Forms.RadioButton rbFixedFunction;
        public System.Windows.Forms.RadioButton rbUDB;
        private System.Windows.Forms.CheckBox chbFixedPlacement;
        private CyPercentageUpDown numMinusTolerance;
        private CyPercentageUpDown numPlusTolerance;
        private System.Windows.Forms.Panel panelTolerance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox m_chbExternalIOBuff;

    }
}
