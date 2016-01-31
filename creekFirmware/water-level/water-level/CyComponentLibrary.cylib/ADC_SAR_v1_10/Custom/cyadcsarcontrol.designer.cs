namespace ADC_SAR_v1_10
{
    partial class CyADC_SARControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_tbClockFreq = new System.Windows.Forms.TextBox();
            this.m_nudConvRate = new ADC_SAR_v1_10.CyNumericUpDown();
            this.SPS_label = new System.Windows.Forms.Label();
            this.Convertsion_label = new System.Windows.Forms.Label();
            this.kHz_label = new System.Windows.Forms.Label();
            this.Clock_label = new System.Windows.Forms.Label();
            this.m_cbResolution = new System.Windows.Forms.ComboBox();
            this.ADC_Resolution_label = new System.Windows.Forms.Label();
            this.m_cbPower = new System.Windows.Forms.ComboBox();
            this.ADC_Power_Label = new System.Windows.Forms.Label();
            this.m_gbClockSource = new System.Windows.Forms.GroupBox();
            this.m_rbClockExternal = new System.Windows.Forms.RadioButton();
            this.m_rbClockInternal = new System.Windows.Forms.RadioButton();
            this.m_gbSOC = new System.Windows.Forms.GroupBox();
            this.m_rbSocTrigerred = new System.Windows.Forms.RadioButton();
            this.m_rbSocFreeRunninge = new System.Windows.Forms.RadioButton();
            this.m_cbReference = new System.Windows.Forms.ComboBox();
            this.m_cbInputRange = new System.Windows.Forms.ComboBox();
            this.ADC_Modes_groupBox = new System.Windows.Forms.GroupBox();
            this.m_nudRefVoltage = new ADC_SAR_v1_10.CyNumericUpDown();
            this.Volts_label = new System.Windows.Forms.Label();
            this.External_Ref_label = new System.Windows.Forms.Label();
            this.Reference_label = new System.Windows.Forms.Label();
            this.ADC_Input_Range_label = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudConvRate)).BeginInit();
            this.m_gbClockSource.SuspendLayout();
            this.m_gbSOC.SuspendLayout();
            this.ADC_Modes_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRefVoltage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_tbClockFreq);
            this.groupBox1.Controls.Add(this.m_nudConvRate);
            this.groupBox1.Controls.Add(this.SPS_label);
            this.groupBox1.Controls.Add(this.Convertsion_label);
            this.groupBox1.Controls.Add(this.kHz_label);
            this.groupBox1.Controls.Add(this.Clock_label);
            this.groupBox1.Controls.Add(this.m_cbResolution);
            this.groupBox1.Controls.Add(this.ADC_Resolution_label);
            this.groupBox1.Controls.Add(this.m_cbPower);
            this.groupBox1.Controls.Add(this.ADC_Power_Label);
            this.groupBox1.Location = new System.Drawing.Point(13, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 131);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modes";
            // 
            // m_tbClockFreq
            // 
            this.m_tbClockFreq.Location = new System.Drawing.Point(122, 93);
            this.m_tbClockFreq.Name = "m_tbClockFreq";
            this.m_tbClockFreq.ReadOnly = true;
            this.m_tbClockFreq.Size = new System.Drawing.Size(84, 20);
            this.m_tbClockFreq.TabIndex = 15;
            // 
            // m_nudConvRate
            // 
            this.m_nudConvRate.Location = new System.Drawing.Point(121, 68);
            this.m_nudConvRate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.m_nudConvRate.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.m_nudConvRate.Name = "m_nudConvRate";
            this.m_nudConvRate.Size = new System.Drawing.Size(84, 20);
            this.m_nudConvRate.TabIndex = 14;
            this.m_nudConvRate.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.m_nudConvRate.ValueChanged += new System.EventHandler(this.m_nudConvRate_ValueChanged);
            // 
            // SPS_label
            // 
            this.SPS_label.AutoSize = true;
            this.SPS_label.Location = new System.Drawing.Point(210, 72);
            this.SPS_label.Name = "SPS_label";
            this.SPS_label.Size = new System.Drawing.Size(28, 13);
            this.SPS_label.TabIndex = 13;
            this.SPS_label.Text = "SPS";
            // 
            // Convertsion_label
            // 
            this.Convertsion_label.AutoSize = true;
            this.Convertsion_label.Location = new System.Drawing.Point(30, 70);
            this.Convertsion_label.Name = "Convertsion_label";
            this.Convertsion_label.Size = new System.Drawing.Size(86, 13);
            this.Convertsion_label.TabIndex = 12;
            this.Convertsion_label.Text = "Conversion Rate";
            // 
            // kHz_label
            // 
            this.kHz_label.AutoSize = true;
            this.kHz_label.Location = new System.Drawing.Point(211, 95);
            this.kHz_label.Name = "kHz_label";
            this.kHz_label.Size = new System.Drawing.Size(26, 13);
            this.kHz_label.TabIndex = 11;
            this.kHz_label.Text = "kHz";
            // 
            // Clock_label
            // 
            this.Clock_label.AutoSize = true;
            this.Clock_label.Location = new System.Drawing.Point(30, 95);
            this.Clock_label.Name = "Clock_label";
            this.Clock_label.Size = new System.Drawing.Size(87, 13);
            this.Clock_label.TabIndex = 9;
            this.Clock_label.Text = "Clock Frequency";
            // 
            // m_cbResolution
            // 
            this.m_cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbResolution.FormattingEnabled = true;
            this.m_cbResolution.Location = new System.Drawing.Point(121, 41);
            this.m_cbResolution.Name = "m_cbResolution";
            this.m_cbResolution.Size = new System.Drawing.Size(121, 21);
            this.m_cbResolution.TabIndex = 8;
            this.m_cbResolution.SelectedIndexChanged += new System.EventHandler(this.m_cbResolution_SelectedIndexChanged);
            // 
            // ADC_Resolution_label
            // 
            this.ADC_Resolution_label.AutoSize = true;
            this.ADC_Resolution_label.Location = new System.Drawing.Point(51, 44);
            this.ADC_Resolution_label.Name = "ADC_Resolution_label";
            this.ADC_Resolution_label.Size = new System.Drawing.Size(57, 13);
            this.ADC_Resolution_label.TabIndex = 7;
            this.ADC_Resolution_label.Text = "Resolution";
            // 
            // m_cbPower
            // 
            this.m_cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPower.FormattingEnabled = true;
            this.m_cbPower.Location = new System.Drawing.Point(121, 16);
            this.m_cbPower.Name = "m_cbPower";
            this.m_cbPower.Size = new System.Drawing.Size(121, 21);
            this.m_cbPower.TabIndex = 4;
            this.m_cbPower.SelectedIndexChanged += new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
            // 
            // ADC_Power_Label
            // 
            this.ADC_Power_Label.AutoSize = true;
            this.ADC_Power_Label.Location = new System.Drawing.Point(71, 20);
            this.ADC_Power_Label.Name = "ADC_Power_Label";
            this.ADC_Power_Label.Size = new System.Drawing.Size(37, 13);
            this.ADC_Power_Label.TabIndex = 3;
            this.ADC_Power_Label.Text = "Power";
            // 
            // m_gbClockSource
            // 
            this.m_gbClockSource.Controls.Add(this.m_rbClockExternal);
            this.m_gbClockSource.Controls.Add(this.m_rbClockInternal);
            this.m_gbClockSource.Location = new System.Drawing.Point(295, 74);
            this.m_gbClockSource.Name = "m_gbClockSource";
            this.m_gbClockSource.Size = new System.Drawing.Size(114, 59);
            this.m_gbClockSource.TabIndex = 4;
            this.m_gbClockSource.TabStop = false;
            this.m_gbClockSource.Text = "Clock Source";
            // 
            // m_rbClockExternal
            // 
            this.m_rbClockExternal.AutoSize = true;
            this.m_rbClockExternal.Location = new System.Drawing.Point(6, 37);
            this.m_rbClockExternal.Name = "m_rbClockExternal";
            this.m_rbClockExternal.Size = new System.Drawing.Size(63, 17);
            this.m_rbClockExternal.TabIndex = 1;
            this.m_rbClockExternal.Text = "External";
            this.m_rbClockExternal.UseVisualStyleBackColor = true;
            this.m_rbClockExternal.CheckedChanged += new System.EventHandler(this.m_rbClockExternal_CheckedChanged);
            // 
            // m_rbClockInternal
            // 
            this.m_rbClockInternal.AutoSize = true;
            this.m_rbClockInternal.Location = new System.Drawing.Point(6, 16);
            this.m_rbClockInternal.Name = "m_rbClockInternal";
            this.m_rbClockInternal.Size = new System.Drawing.Size(60, 17);
            this.m_rbClockInternal.TabIndex = 0;
            this.m_rbClockInternal.Text = "Internal";
            this.m_rbClockInternal.UseVisualStyleBackColor = true;
            this.m_rbClockInternal.CheckedChanged += new System.EventHandler(this.m_rbClockInternal_CheckedChanged);
            // 
            // m_gbSOC
            // 
            this.m_gbSOC.Controls.Add(this.m_rbSocTrigerred);
            this.m_gbSOC.Controls.Add(this.m_rbSocFreeRunninge);
            this.m_gbSOC.Location = new System.Drawing.Point(295, 3);
            this.m_gbSOC.Name = "m_gbSOC";
            this.m_gbSOC.Size = new System.Drawing.Size(114, 63);
            this.m_gbSOC.TabIndex = 5;
            this.m_gbSOC.TabStop = false;
            this.m_gbSOC.Text = "Sample Mode";
            // 
            // m_rbSocTrigerred
            // 
            this.m_rbSocTrigerred.AutoSize = true;
            this.m_rbSocTrigerred.Location = new System.Drawing.Point(6, 37);
            this.m_rbSocTrigerred.Name = "m_rbSocTrigerred";
            this.m_rbSocTrigerred.Size = new System.Drawing.Size(70, 17);
            this.m_rbSocTrigerred.TabIndex = 1;
            this.m_rbSocTrigerred.Text = "Triggered";
            this.m_rbSocTrigerred.UseVisualStyleBackColor = true;
            this.m_rbSocTrigerred.CheckedChanged += new System.EventHandler(this.m_rbSocHardware_CheckedChanged);
            // 
            // m_rbSocFreeRunninge
            // 
            this.m_rbSocFreeRunninge.AutoSize = true;
            this.m_rbSocFreeRunninge.Location = new System.Drawing.Point(6, 18);
            this.m_rbSocFreeRunninge.Name = "m_rbSocFreeRunninge";
            this.m_rbSocFreeRunninge.Size = new System.Drawing.Size(89, 17);
            this.m_rbSocFreeRunninge.TabIndex = 0;
            this.m_rbSocFreeRunninge.Text = "Free Running";
            this.m_rbSocFreeRunninge.UseVisualStyleBackColor = true;
            this.m_rbSocFreeRunninge.CheckedChanged += new System.EventHandler(this.m_rbSocSoftware_CheckedChanged);
            // 
            // m_cbReference
            // 
            this.m_cbReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbReference.FormattingEnabled = true;
            this.m_cbReference.Location = new System.Drawing.Point(121, 38);
            this.m_cbReference.Name = "m_cbReference";
            this.m_cbReference.Size = new System.Drawing.Size(204, 21);
            this.m_cbReference.TabIndex = 13;
            this.m_cbReference.SelectedIndexChanged += new System.EventHandler(this.m_cbReference_SelectedIndexChanged);
            // 
            // m_cbInputRange
            // 
            this.m_cbInputRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbInputRange.FormattingEnabled = true;
            this.m_cbInputRange.Location = new System.Drawing.Point(121, 12);
            this.m_cbInputRange.Name = "m_cbInputRange";
            this.m_cbInputRange.Size = new System.Drawing.Size(269, 21);
            this.m_cbInputRange.TabIndex = 11;
            this.m_cbInputRange.SelectedIndexChanged += new System.EventHandler(this.m_cbInputRange_SelectedIndexChanged_1);
            // 
            // ADC_Modes_groupBox
            // 
            this.ADC_Modes_groupBox.Controls.Add(this.m_nudRefVoltage);
            this.ADC_Modes_groupBox.Controls.Add(this.Volts_label);
            this.ADC_Modes_groupBox.Controls.Add(this.External_Ref_label);
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbReference);
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbInputRange);
            this.ADC_Modes_groupBox.Controls.Add(this.Reference_label);
            this.ADC_Modes_groupBox.Controls.Add(this.ADC_Input_Range_label);
            this.ADC_Modes_groupBox.Location = new System.Drawing.Point(13, 136);
            this.ADC_Modes_groupBox.Name = "ADC_Modes_groupBox";
            this.ADC_Modes_groupBox.Size = new System.Drawing.Size(396, 88);
            this.ADC_Modes_groupBox.TabIndex = 6;
            this.ADC_Modes_groupBox.TabStop = false;
            this.ADC_Modes_groupBox.Text = "Input";
            // 
            // m_nudRefVoltage
            // 
            this.m_nudRefVoltage.DecimalPlaces = 4;
            this.m_nudRefVoltage.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.m_nudRefVoltage.Location = new System.Drawing.Point(122, 63);
            this.m_nudRefVoltage.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            196608});
            this.m_nudRefVoltage.Name = "m_nudRefVoltage";
            this.m_nudRefVoltage.Size = new System.Drawing.Size(84, 20);
            this.m_nudRefVoltage.TabIndex = 15;
            this.m_nudRefVoltage.Value = new decimal(new int[] {
            1024,
            0,
            0,
            196608});
            this.m_nudRefVoltage.ValueChanged += new System.EventHandler(this.m_nudRefVoltage_ValueChanged);
            // 
            // Volts_label
            // 
            this.Volts_label.AutoSize = true;
            this.Volts_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Volts_label.Location = new System.Drawing.Point(209, 67);
            this.Volts_label.Name = "Volts_label";
            this.Volts_label.Size = new System.Drawing.Size(30, 13);
            this.Volts_label.TabIndex = 13;
            this.Volts_label.Text = "Volts";
            // 
            // External_Ref_label
            // 
            this.External_Ref_label.AutoSize = true;
            this.External_Ref_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.External_Ref_label.Location = new System.Drawing.Point(12, 66);
            this.External_Ref_label.Name = "External_Ref_label";
            this.External_Ref_label.Size = new System.Drawing.Size(96, 13);
            this.External_Ref_label.TabIndex = 14;
            this.External_Ref_label.Text = "Voltage Reference";
            // 
            // Reference_label
            // 
            this.Reference_label.AutoSize = true;
            this.Reference_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Reference_label.Location = new System.Drawing.Point(51, 42);
            this.Reference_label.Name = "Reference_label";
            this.Reference_label.Size = new System.Drawing.Size(57, 13);
            this.Reference_label.TabIndex = 4;
            this.Reference_label.Text = "Reference";
            // 
            // ADC_Input_Range_label
            // 
            this.ADC_Input_Range_label.AutoSize = true;
            this.ADC_Input_Range_label.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ADC_Input_Range_label.Location = new System.Drawing.Point(42, 20);
            this.ADC_Input_Range_label.Name = "ADC_Input_Range_label";
            this.ADC_Input_Range_label.Size = new System.Drawing.Size(66, 13);
            this.ADC_Input_Range_label.TabIndex = 9;
            this.ADC_Input_Range_label.Text = "Input Range";
            // 
            // CyADC_SARControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ADC_Modes_groupBox);
            this.Controls.Add(this.m_gbSOC);
            this.Controls.Add(this.m_gbClockSource);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyADC_SARControl";
            this.Size = new System.Drawing.Size(437, 241);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudConvRate)).EndInit();
            this.m_gbClockSource.ResumeLayout(false);
            this.m_gbClockSource.PerformLayout();
            this.m_gbSOC.ResumeLayout(false);
            this.m_gbSOC.PerformLayout();
            this.ADC_Modes_groupBox.ResumeLayout(false);
            this.ADC_Modes_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRefVoltage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label ADC_Power_Label;
        private System.Windows.Forms.Label ADC_Resolution_label;
        private System.Windows.Forms.ComboBox m_cbPower;
        private System.Windows.Forms.ComboBox m_cbResolution;
        private System.Windows.Forms.GroupBox m_gbClockSource;
        private System.Windows.Forms.RadioButton m_rbClockExternal;
        private System.Windows.Forms.RadioButton m_rbClockInternal;
        private System.Windows.Forms.GroupBox m_gbSOC;
        private System.Windows.Forms.RadioButton m_rbSocFreeRunninge;
        private System.Windows.Forms.RadioButton m_rbSocTrigerred;
        private System.Windows.Forms.Label Clock_label;
        private System.Windows.Forms.Label kHz_label;
        private System.Windows.Forms.Label Convertsion_label;
        private System.Windows.Forms.Label SPS_label;
        private System.Windows.Forms.ComboBox m_cbReference;
        private System.Windows.Forms.ComboBox m_cbInputRange;
        private System.Windows.Forms.GroupBox ADC_Modes_groupBox;
        private System.Windows.Forms.Label Volts_label;
        private System.Windows.Forms.Label External_Ref_label;
        private System.Windows.Forms.Label Reference_label;
        private System.Windows.Forms.Label ADC_Input_Range_label;
        private System.Windows.Forms.TextBox m_tbClockFreq;
        private CyNumericUpDown m_nudConvRate;
        private CyNumericUpDown m_nudRefVoltage;
    }
}
