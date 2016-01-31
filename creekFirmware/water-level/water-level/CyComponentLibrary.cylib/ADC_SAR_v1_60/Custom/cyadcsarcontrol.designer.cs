namespace ADC_SAR_v1_60
{
    partial class CySARADCControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblNoRate = new System.Windows.Forms.Label();
            this.lblNoFreq = new System.Windows.Forms.Label();
            this.tbClockFreq = new System.Windows.Forms.TextBox();
            this.numSampleRate = new System.Windows.Forms.NumericUpDown();
            this.sps_label = new System.Windows.Forms.Label();
            this.Convertsion_label = new System.Windows.Forms.Label();
            this.kHz_label = new System.Windows.Forms.Label();
            this.Clock_label = new System.Windows.Forms.Label();
            this.cbResolution = new System.Windows.Forms.ComboBox();
            this.ADC_Resolution_label = new System.Windows.Forms.Label();
            this.m_gbClockSource = new System.Windows.Forms.GroupBox();
            this.rbExternal = new System.Windows.Forms.RadioButton();
            this.rbInternal = new System.Windows.Forms.RadioButton();
            this.m_gbSOC = new System.Windows.Forms.GroupBox();
            this.m_rbSocTrigerred = new System.Windows.Forms.RadioButton();
            this.rbFreeRunning = new System.Windows.Forms.RadioButton();
            this.cbReference = new System.Windows.Forms.ComboBox();
            this.cbInputRange = new System.Windows.Forms.ComboBox();
            this.ADC_Modes_groupBox = new System.Windows.Forms.GroupBox();
            this.numRefVoltage = new System.Windows.Forms.NumericUpDown();
            this.Volts_label = new System.Windows.Forms.Label();
            this.External_Ref_label = new System.Windows.Forms.Label();
            this.Reference_label = new System.Windows.Forms.Label();
            this.ADC_Input_Range_label = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleRate)).BeginInit();
            this.m_gbClockSource.SuspendLayout();
            this.m_gbSOC.SuspendLayout();
            this.ADC_Modes_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRefVoltage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblNoRate);
            this.groupBox1.Controls.Add(this.lblNoFreq);
            this.groupBox1.Controls.Add(this.tbClockFreq);
            this.groupBox1.Controls.Add(this.numSampleRate);
            this.groupBox1.Controls.Add(this.sps_label);
            this.groupBox1.Controls.Add(this.Convertsion_label);
            this.groupBox1.Controls.Add(this.kHz_label);
            this.groupBox1.Controls.Add(this.Clock_label);
            this.groupBox1.Controls.Add(this.cbResolution);
            this.groupBox1.Controls.Add(this.ADC_Resolution_label);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(13, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(219, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "bits";
            // 
            // lblNoRate
            // 
            this.lblNoRate.AutoSize = true;
            this.lblNoRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNoRate.Location = new System.Drawing.Point(122, 53);
            this.lblNoRate.Name = "lblNoRate";
            this.lblNoRate.Size = new System.Drawing.Size(109, 13);
            this.lblNoRate.TabIndex = 17;
            this.lblNoRate.Text = "UNKNOWN RATE";
            // 
            // lblNoFreq
            // 
            this.lblNoFreq.AutoSize = true;
            this.lblNoFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNoFreq.Location = new System.Drawing.Point(122, 87);
            this.lblNoFreq.Name = "lblNoFreq";
            this.lblNoFreq.Size = new System.Drawing.Size(109, 13);
            this.lblNoFreq.TabIndex = 16;
            this.lblNoFreq.Text = "UNKNOWN FREQ";
            // 
            // tbClockFreq
            // 
            this.errorProvider.SetIconAlignment(this.tbClockFreq, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.tbClockFreq.Location = new System.Drawing.Point(124, 84);
            this.tbClockFreq.Name = "tbClockFreq";
            this.tbClockFreq.ReadOnly = true;
            this.tbClockFreq.Size = new System.Drawing.Size(85, 20);
            this.tbClockFreq.TabIndex = 3;
            this.tbClockFreq.Visible = false;
            this.tbClockFreq.TextChanged += new System.EventHandler(this.tbClockFreq_TextChanged);
            // 
            // numSampleRate
            // 
            this.errorProvider.SetIconAlignment(this.numSampleRate, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.numSampleRate.Location = new System.Drawing.Point(125, 51);
            this.numSampleRate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numSampleRate.Name = "numSampleRate";
            this.numSampleRate.Size = new System.Drawing.Size(85, 20);
            this.numSampleRate.TabIndex = 2;
            this.numSampleRate.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // sps_label
            // 
            this.sps_label.AutoSize = true;
            this.sps_label.Location = new System.Drawing.Point(219, 53);
            this.sps_label.Name = "sps_label";
            this.sps_label.Size = new System.Drawing.Size(28, 13);
            this.sps_label.TabIndex = 13;
            this.sps_label.Text = "SPS";
            // 
            // Convertsion_label
            // 
            this.Convertsion_label.AutoSize = true;
            this.Convertsion_label.Location = new System.Drawing.Point(24, 53);
            this.Convertsion_label.Name = "Convertsion_label";
            this.Convertsion_label.Size = new System.Drawing.Size(89, 13);
            this.Convertsion_label.TabIndex = 12;
            this.Convertsion_label.Text = "Conversion Rate:";
            // 
            // kHz_label
            // 
            this.kHz_label.AutoSize = true;
            this.kHz_label.Location = new System.Drawing.Point(219, 87);
            this.kHz_label.Name = "kHz_label";
            this.kHz_label.Size = new System.Drawing.Size(26, 13);
            this.kHz_label.TabIndex = 11;
            this.kHz_label.Text = "kHz";
            this.kHz_label.Visible = false;
            // 
            // Clock_label
            // 
            this.Clock_label.AutoSize = true;
            this.Clock_label.Location = new System.Drawing.Point(23, 87);
            this.Clock_label.Name = "Clock_label";
            this.Clock_label.Size = new System.Drawing.Size(90, 13);
            this.Clock_label.TabIndex = 9;
            this.Clock_label.Text = "Clock Frequency:";
            // 
            // cbResolution
            // 
            this.cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResolution.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbResolution, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbResolution.Location = new System.Drawing.Point(124, 19);
            this.cbResolution.Name = "cbResolution";
            this.cbResolution.Size = new System.Drawing.Size(86, 21);
            this.cbResolution.TabIndex = 1;
            this.cbResolution.SelectedIndexChanged += new System.EventHandler(this.m_cbResolution_SelectedIndexChanged);
            // 
            // ADC_Resolution_label
            // 
            this.ADC_Resolution_label.AutoSize = true;
            this.ADC_Resolution_label.Location = new System.Drawing.Point(53, 22);
            this.ADC_Resolution_label.Name = "ADC_Resolution_label";
            this.ADC_Resolution_label.Size = new System.Drawing.Size(60, 13);
            this.ADC_Resolution_label.TabIndex = 7;
            this.ADC_Resolution_label.Text = "Resolution:";
            // 
            // m_gbClockSource
            // 
            this.m_gbClockSource.Controls.Add(this.rbExternal);
            this.m_gbClockSource.Controls.Add(this.rbInternal);
            this.m_gbClockSource.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_gbClockSource.Location = new System.Drawing.Point(295, 64);
            this.m_gbClockSource.Name = "m_gbClockSource";
            this.m_gbClockSource.Size = new System.Drawing.Size(114, 55);
            this.m_gbClockSource.TabIndex = 3;
            this.m_gbClockSource.TabStop = false;
            this.m_gbClockSource.Text = "Clock Source";
            // 
            // rbExternal
            // 
            this.rbExternal.AutoSize = true;
            this.rbExternal.Location = new System.Drawing.Point(6, 34);
            this.rbExternal.Name = "rbExternal";
            this.rbExternal.Size = new System.Drawing.Size(63, 17);
            this.rbExternal.TabIndex = 1;
            this.rbExternal.Text = "External";
            this.rbExternal.UseVisualStyleBackColor = true;
            // 
            // rbInternal
            // 
            this.rbInternal.AutoSize = true;
            this.rbInternal.Location = new System.Drawing.Point(6, 14);
            this.rbInternal.Name = "rbInternal";
            this.rbInternal.Size = new System.Drawing.Size(60, 17);
            this.rbInternal.TabIndex = 0;
            this.rbInternal.Text = "Internal";
            this.rbInternal.UseVisualStyleBackColor = true;
            this.rbInternal.CheckedChanged += new System.EventHandler(this.rbInternal_CheckedChanged);
            // 
            // m_gbSOC
            // 
            this.m_gbSOC.Controls.Add(this.m_rbSocTrigerred);
            this.m_gbSOC.Controls.Add(this.rbFreeRunning);
            this.m_gbSOC.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_gbSOC.Location = new System.Drawing.Point(295, 3);
            this.m_gbSOC.Name = "m_gbSOC";
            this.m_gbSOC.Size = new System.Drawing.Size(114, 55);
            this.m_gbSOC.TabIndex = 2;
            this.m_gbSOC.TabStop = false;
            this.m_gbSOC.Text = "Sample Mode";
            // 
            // m_rbSocTrigerred
            // 
            this.m_rbSocTrigerred.AutoSize = true;
            this.m_rbSocTrigerred.Location = new System.Drawing.Point(6, 34);
            this.m_rbSocTrigerred.Name = "m_rbSocTrigerred";
            this.m_rbSocTrigerred.Size = new System.Drawing.Size(70, 17);
            this.m_rbSocTrigerred.TabIndex = 1;
            this.m_rbSocTrigerred.Text = "Triggered";
            this.m_rbSocTrigerred.UseVisualStyleBackColor = true;
            // 
            // rbFreeRunning
            // 
            this.rbFreeRunning.AutoSize = true;
            this.rbFreeRunning.Location = new System.Drawing.Point(6, 15);
            this.rbFreeRunning.Name = "rbFreeRunning";
            this.rbFreeRunning.Size = new System.Drawing.Size(89, 17);
            this.rbFreeRunning.TabIndex = 0;
            this.rbFreeRunning.Text = "Free Running";
            this.rbFreeRunning.UseVisualStyleBackColor = true;
            this.rbFreeRunning.CheckedChanged += new System.EventHandler(this.rbFreeRunning_CheckedChanged);
            // 
            // cbReference
            // 
            this.cbReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReference.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbReference, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbReference.Location = new System.Drawing.Point(124, 51);
            this.cbReference.Name = "cbReference";
            this.cbReference.Size = new System.Drawing.Size(204, 21);
            this.cbReference.TabIndex = 1;
            this.cbReference.SelectedIndexChanged += new System.EventHandler(this.cbReference_SelectedIndexChanged);
            // 
            // cbInputRange
            // 
            this.cbInputRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInputRange.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbInputRange, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbInputRange.Location = new System.Drawing.Point(124, 19);
            this.cbInputRange.Name = "cbInputRange";
            this.cbInputRange.Size = new System.Drawing.Size(249, 21);
            this.cbInputRange.TabIndex = 0;
            this.cbInputRange.SelectedIndexChanged += new System.EventHandler(this.cbInputRange_SelectedIndexChanged);
            // 
            // ADC_Modes_groupBox
            // 
            this.ADC_Modes_groupBox.Controls.Add(this.numRefVoltage);
            this.ADC_Modes_groupBox.Controls.Add(this.Volts_label);
            this.ADC_Modes_groupBox.Controls.Add(this.External_Ref_label);
            this.ADC_Modes_groupBox.Controls.Add(this.cbReference);
            this.ADC_Modes_groupBox.Controls.Add(this.cbInputRange);
            this.ADC_Modes_groupBox.Controls.Add(this.Reference_label);
            this.ADC_Modes_groupBox.Controls.Add(this.ADC_Input_Range_label);
            this.ADC_Modes_groupBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ADC_Modes_groupBox.Location = new System.Drawing.Point(13, 119);
            this.ADC_Modes_groupBox.Name = "ADC_Modes_groupBox";
            this.ADC_Modes_groupBox.Size = new System.Drawing.Size(396, 116);
            this.ADC_Modes_groupBox.TabIndex = 4;
            this.ADC_Modes_groupBox.TabStop = false;
            this.ADC_Modes_groupBox.Text = "Input";
            // 
            // numRefVoltage
            // 
            this.numRefVoltage.DecimalPlaces = 4;
            this.errorProvider.SetIconAlignment(this.numRefVoltage, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.numRefVoltage.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numRefVoltage.Location = new System.Drawing.Point(124, 84);
            this.numRefVoltage.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            196608});
            this.numRefVoltage.Name = "numRefVoltage";
            this.numRefVoltage.Size = new System.Drawing.Size(84, 20);
            this.numRefVoltage.TabIndex = 2;
            this.numRefVoltage.Value = new decimal(new int[] {
            1024,
            0,
            0,
            196608});
            // 
            // Volts_label
            // 
            this.Volts_label.AutoSize = true;
            this.Volts_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Volts_label.Location = new System.Drawing.Point(219, 86);
            this.Volts_label.Name = "Volts_label";
            this.Volts_label.Size = new System.Drawing.Size(30, 13);
            this.Volts_label.TabIndex = 13;
            this.Volts_label.Text = "Volts";
            // 
            // External_Ref_label
            // 
            this.External_Ref_label.AutoSize = true;
            this.External_Ref_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.External_Ref_label.Location = new System.Drawing.Point(14, 86);
            this.External_Ref_label.Name = "External_Ref_label";
            this.External_Ref_label.Size = new System.Drawing.Size(99, 13);
            this.External_Ref_label.TabIndex = 14;
            this.External_Ref_label.Text = "Voltage Reference:";
            // 
            // Reference_label
            // 
            this.Reference_label.AutoSize = true;
            this.Reference_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Reference_label.Location = new System.Drawing.Point(53, 54);
            this.Reference_label.Name = "Reference_label";
            this.Reference_label.Size = new System.Drawing.Size(60, 13);
            this.Reference_label.TabIndex = 4;
            this.Reference_label.Text = "Reference:";
            // 
            // ADC_Input_Range_label
            // 
            this.ADC_Input_Range_label.AutoSize = true;
            this.ADC_Input_Range_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ADC_Input_Range_label.Location = new System.Drawing.Point(44, 22);
            this.ADC_Input_Range_label.Name = "ADC_Input_Range_label";
            this.ADC_Input_Range_label.Size = new System.Drawing.Size(69, 13);
            this.ADC_Input_Range_label.TabIndex = 9;
            this.ADC_Input_Range_label.Text = "Input Range:";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // CySARADCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.ADC_Modes_groupBox);
            this.Controls.Add(this.m_gbSOC);
            this.Controls.Add(this.m_gbClockSource);
            this.Controls.Add(this.groupBox1);
            this.Name = "CySARADCControl";
            this.Size = new System.Drawing.Size(437, 241);
            this.Load += new System.EventHandler(this.CySARADCControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSampleRate)).EndInit();
            this.m_gbClockSource.ResumeLayout(false);
            this.m_gbClockSource.PerformLayout();
            this.m_gbSOC.ResumeLayout(false);
            this.m_gbSOC.PerformLayout();
            this.ADC_Modes_groupBox.ResumeLayout(false);
            this.ADC_Modes_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRefVoltage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label ADC_Resolution_label;
        private System.Windows.Forms.ComboBox cbResolution;
        private System.Windows.Forms.GroupBox m_gbClockSource;
        private System.Windows.Forms.RadioButton rbExternal;
        private System.Windows.Forms.RadioButton rbInternal;
        private System.Windows.Forms.GroupBox m_gbSOC;
        private System.Windows.Forms.RadioButton rbFreeRunning;
        private System.Windows.Forms.RadioButton m_rbSocTrigerred;
        private System.Windows.Forms.Label Clock_label;
        private System.Windows.Forms.Label kHz_label;
        private System.Windows.Forms.Label Convertsion_label;
        private System.Windows.Forms.Label sps_label;
        private System.Windows.Forms.ComboBox cbReference;
        private System.Windows.Forms.ComboBox cbInputRange;
        private System.Windows.Forms.GroupBox ADC_Modes_groupBox;
        private System.Windows.Forms.Label Volts_label;
        private System.Windows.Forms.Label External_Ref_label;
        private System.Windows.Forms.Label Reference_label;
        private System.Windows.Forms.Label ADC_Input_Range_label;
        private System.Windows.Forms.TextBox tbClockFreq;
        private System.Windows.Forms.NumericUpDown numSampleRate;
        private System.Windows.Forms.NumericUpDown numRefVoltage;
        private System.Windows.Forms.Label lblNoFreq;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblNoRate;
        private System.Windows.Forms.Label label1;
    }
}
