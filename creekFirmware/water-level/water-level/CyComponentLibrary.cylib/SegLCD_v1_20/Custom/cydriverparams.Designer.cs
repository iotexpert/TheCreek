namespace SegLCD_v1_20
{
    partial class CyDriverParams
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
            this.comboBoxLowDriveMode = new System.Windows.Forms.ComboBox();
            this.numUpDownLowDriveInitTime = new System.Windows.Forms.NumericUpDown();
            this.labelLowDriveInitTime = new System.Windows.Forms.Label();
            this.labelLowDriveMode = new System.Windows.Forms.Label();
            this.labelHiDriverTime = new System.Windows.Forms.Label();
            this.labelLowDriveDutyCycle = new System.Windows.Forms.Label();
            this.comboBoxDriverPowerMode = new System.Windows.Forms.ComboBox();
            this.labelDriverPowerMode = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.labelClockFrequency = new System.Windows.Forms.Label();
            this.editClockFrequency = new System.Windows.Forms.Label();
            this.numUpDownHiDriveTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLowDriveInitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownHiDriveTime)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxLowDriveMode
            // 
            this.comboBoxLowDriveMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLowDriveMode.FormattingEnabled = true;
            this.comboBoxLowDriveMode.Items.AddRange(new object[] {
            "Low range",
            "High range"});
            this.comboBoxLowDriveMode.Location = new System.Drawing.Point(193, 63);
            this.comboBoxLowDriveMode.Name = "comboBoxLowDriveMode";
            this.comboBoxLowDriveMode.Size = new System.Drawing.Size(110, 21);
            this.comboBoxLowDriveMode.TabIndex = 40;
            this.comboBoxLowDriveMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxLowDriveMode_SelectedIndexChanged);
            // 
            // numUpDownLowDriveInitTime
            // 
            this.numUpDownLowDriveInitTime.DecimalPlaces = 1;
            this.numUpDownLowDriveInitTime.Location = new System.Drawing.Point(193, 91);
            this.numUpDownLowDriveInitTime.Name = "numUpDownLowDriveInitTime";
            this.numUpDownLowDriveInitTime.Size = new System.Drawing.Size(110, 20);
            this.numUpDownLowDriveInitTime.TabIndex = 41;
            this.numUpDownLowDriveInitTime.ValueChanged += new System.EventHandler(this.numUpDownLowDriveInitTime_ValueChanged);
            this.numUpDownLowDriveInitTime.Validated += new System.EventHandler(this.UpdateMaximumValues);
            // 
            // labelLowDriveInitTime
            // 
            this.labelLowDriveInitTime.AutoSize = true;
            this.labelLowDriveInitTime.Location = new System.Drawing.Point(11, 92);
            this.labelLowDriveInitTime.Name = "labelLowDriveInitTime";
            this.labelLowDriveInitTime.Size = new System.Drawing.Size(92, 13);
            this.labelLowDriveInitTime.TabIndex = 32;
            this.labelLowDriveInitTime.Text = "Low drive time, μs";
            // 
            // labelLowDriveMode
            // 
            this.labelLowDriveMode.AutoSize = true;
            this.labelLowDriveMode.Location = new System.Drawing.Point(11, 65);
            this.labelLowDriveMode.Name = "labelLowDriveMode";
            this.labelLowDriveMode.Size = new System.Drawing.Size(82, 13);
            this.labelLowDriveMode.TabIndex = 31;
            this.labelLowDriveMode.Text = "Low drive mode";
            // 
            // labelHiDriverTime
            // 
            this.labelHiDriverTime.AutoSize = true;
            this.labelHiDriverTime.Location = new System.Drawing.Point(11, 38);
            this.labelHiDriverTime.Name = "labelHiDriverTime";
            this.labelHiDriverTime.Size = new System.Drawing.Size(82, 13);
            this.labelHiDriverTime.TabIndex = 27;
            this.labelHiDriverTime.Text = "Hi drive time, μs";
            // 
            // labelLowDriveDutyCycle
            // 
            this.labelLowDriveDutyCycle.AutoSize = true;
            this.labelLowDriveDutyCycle.Location = new System.Drawing.Point(11, 119);
            this.labelLowDriveDutyCycle.Name = "labelLowDriveDutyCycle";
            this.labelLowDriveDutyCycle.Size = new System.Drawing.Size(118, 13);
            this.labelLowDriveDutyCycle.TabIndex = 26;
            this.labelLowDriveDutyCycle.Text = "Low drive duty cycle, %";
            this.labelLowDriveDutyCycle.Visible = false;
            // 
            // comboBoxDriverPowerMode
            // 
            this.comboBoxDriverPowerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDriverPowerMode.FormattingEnabled = true;
            this.comboBoxDriverPowerMode.Items.AddRange(new object[] {
            "Always Active",
            "Low Power"});
            this.comboBoxDriverPowerMode.Location = new System.Drawing.Point(193, 8);
            this.comboBoxDriverPowerMode.Name = "comboBoxDriverPowerMode";
            this.comboBoxDriverPowerMode.Size = new System.Drawing.Size(110, 21);
            this.comboBoxDriverPowerMode.TabIndex = 1;
            this.comboBoxDriverPowerMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriverPowerMode_SelectedIndexChanged);
            // 
            // labelDriverPowerMode
            // 
            this.labelDriverPowerMode.AutoSize = true;
            this.labelDriverPowerMode.Location = new System.Drawing.Point(11, 11);
            this.labelDriverPowerMode.Name = "labelDriverPowerMode";
            this.labelDriverPowerMode.Size = new System.Drawing.Size(98, 13);
            this.labelDriverPowerMode.TabIndex = 23;
            this.labelDriverPowerMode.Text = "Driver Power Mode";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // labelClockFrequency
            // 
            this.labelClockFrequency.AutoSize = true;
            this.labelClockFrequency.Enabled = false;
            this.labelClockFrequency.Location = new System.Drawing.Point(190, 160);
            this.labelClockFrequency.Name = "labelClockFrequency";
            this.labelClockFrequency.Size = new System.Drawing.Size(87, 13);
            this.labelClockFrequency.TabIndex = 44;
            this.labelClockFrequency.Text = "Clock Frequency";
            this.labelClockFrequency.Visible = false;
            // 
            // editClockFrequency
            // 
            this.editClockFrequency.BackColor = System.Drawing.SystemColors.Window;
            this.editClockFrequency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.editClockFrequency.Enabled = false;
            this.editClockFrequency.Location = new System.Drawing.Point(193, 177);
            this.editClockFrequency.Name = "editClockFrequency";
            this.editClockFrequency.Size = new System.Drawing.Size(110, 20);
            this.editClockFrequency.TabIndex = 47;
            this.editClockFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editClockFrequency.Visible = false;
            // 
            // numUpDownHiDriveTime
            // 
            this.numUpDownHiDriveTime.DecimalPlaces = 1;
            this.numUpDownHiDriveTime.Increment = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.numUpDownHiDriveTime.Location = new System.Drawing.Point(193, 36);
            this.numUpDownHiDriveTime.Name = "numUpDownHiDriveTime";
            this.numUpDownHiDriveTime.Size = new System.Drawing.Size(110, 20);
            this.numUpDownHiDriveTime.TabIndex = 39;
            this.numUpDownHiDriveTime.ValueChanged += new System.EventHandler(this.numUpDownHiDriveTime_ValueChanged);
            this.numUpDownHiDriveTime.Validated += new System.EventHandler(this.UpdateMaximumValues);
            // 
            // CyDriverParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.editClockFrequency);
            this.Controls.Add(this.labelClockFrequency);
            this.Controls.Add(this.numUpDownHiDriveTime);
            this.Controls.Add(this.comboBoxLowDriveMode);
            this.Controls.Add(this.numUpDownLowDriveInitTime);
            this.Controls.Add(this.labelLowDriveInitTime);
            this.Controls.Add(this.labelLowDriveMode);
            this.Controls.Add(this.labelHiDriverTime);
            this.Controls.Add(this.labelLowDriveDutyCycle);
            this.Controls.Add(this.comboBoxDriverPowerMode);
            this.Controls.Add(this.labelDriverPowerMode);
            this.Name = "CyDriverParams";
            this.Size = new System.Drawing.Size(446, 243);
            this.VisibleChanged += new System.EventHandler(this.CyDriverParams_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLowDriveInitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownHiDriveTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxLowDriveMode;
        private System.Windows.Forms.NumericUpDown numUpDownLowDriveInitTime;
        private System.Windows.Forms.Label labelLowDriveInitTime;
        private System.Windows.Forms.Label labelLowDriveMode;
        private System.Windows.Forms.Label labelHiDriverTime;
        private System.Windows.Forms.Label labelLowDriveDutyCycle;
        private System.Windows.Forms.ComboBox comboBoxDriverPowerMode;
        private System.Windows.Forms.Label labelDriverPowerMode;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label labelClockFrequency;
        private System.Windows.Forms.Label editClockFrequency;
        private System.Windows.Forms.NumericUpDown numUpDownHiDriveTime;
    }
}
