namespace CyCustomizer.ADC_DelSig_v1_0
{
    partial class CyADC_DelSigControl1
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
            this.SamplesPerSec_label = new System.Windows.Forms.Label();
            this.m_nudConvRate = new CyNumericUpDown();
            this.ConvRate_label = new System.Windows.Forms.Label();
            this.m_cbResolution = new System.Windows.Forms.ComboBox();
            this.ADC_Resolution_label = new System.Windows.Forms.Label();
            this.m_cbConvMode = new System.Windows.Forms.ComboBox();
            this.Conv_Mode_label = new System.Windows.Forms.Label();
            this.m_cbPower = new System.Windows.Forms.ComboBox();
            this.ADC_Power_Label = new System.Windows.Forms.Label();
            this.ADC_Modes_groupBox = new System.Windows.Forms.GroupBox();
            this.m_cbReference = new System.Windows.Forms.ComboBox();
            this.m_cbInputBufferGain = new System.Windows.Forms.ComboBox();
            this.m_cbInputRange = new System.Windows.Forms.ComboBox();
            this.Reference_label = new System.Windows.Forms.Label();
            this.Input_Buffer_Gain_label = new System.Windows.Forms.Label();
            this.ADC_Input_Range_label = new System.Windows.Forms.Label();
            this.m_gbClockSource = new System.Windows.Forms.GroupBox();
            this.m_rbClockExternal = new System.Windows.Forms.RadioButton();
            this.m_rbClockInternal = new System.Windows.Forms.RadioButton();
            this.m_gbSOC = new System.Windows.Forms.GroupBox();
            this.m_rbSocHardware = new System.Windows.Forms.RadioButton();
            this.m_rbSocSoftware = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudConvRate)).BeginInit();
            this.ADC_Modes_groupBox.SuspendLayout();
            this.m_gbClockSource.SuspendLayout();
            this.m_gbSOC.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SamplesPerSec_label);
            this.groupBox1.Controls.Add(this.m_nudConvRate);
            this.groupBox1.Controls.Add(this.ConvRate_label);
            this.groupBox1.Controls.Add(this.m_cbResolution);
            this.groupBox1.Controls.Add(this.ADC_Resolution_label);
            this.groupBox1.Controls.Add(this.m_cbConvMode);
            this.groupBox1.Controls.Add(this.Conv_Mode_label);
            this.groupBox1.Controls.Add(this.m_cbPower);
            this.groupBox1.Controls.Add(this.ADC_Power_Label);
            this.groupBox1.Location = new System.Drawing.Point(13, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 131);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Modes";
            // 
            // SamplesPerSec_label
            // 
            this.SamplesPerSec_label.AutoSize = true;
            this.SamplesPerSec_label.Location = new System.Drawing.Point(211, 91);
            this.SamplesPerSec_label.Name = "SamplesPerSec_label";
            this.SamplesPerSec_label.Size = new System.Drawing.Size(28, 13);
            this.SamplesPerSec_label.TabIndex = 11;
            this.SamplesPerSec_label.Text = "SPS";
            // 
            // m_nudConvRate
            // 
            this.m_nudConvRate.Location = new System.Drawing.Point(121, 88);
            this.m_nudConvRate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.m_nudConvRate.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.m_nudConvRate.Name = "m_nudConvRate";
            this.m_nudConvRate.Size = new System.Drawing.Size(84, 20);
            this.m_nudConvRate.TabIndex = 10;
            this.m_nudConvRate.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudConvRate.ValueChanged += new System.EventHandler(this.m_nudConvRate_ValueChanged);
            // 
            // ConvRate_label
            // 
            this.ConvRate_label.AutoSize = true;
            this.ConvRate_label.Location = new System.Drawing.Point(22, 89);
            this.ConvRate_label.Name = "ConvRate_label";
            this.ConvRate_label.Size = new System.Drawing.Size(86, 13);
            this.ConvRate_label.TabIndex = 9;
            this.ConvRate_label.Text = "Conversion Rate";
            // 
            // m_cbResolution
            // 
            this.m_cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbResolution.FormattingEnabled = true;
            this.m_cbResolution.Location = new System.Drawing.Point(121, 61);
            this.m_cbResolution.Name = "m_cbResolution";
            this.m_cbResolution.Size = new System.Drawing.Size(121, 21);
            this.m_cbResolution.TabIndex = 8;
            this.m_cbResolution.SelectedIndexChanged += new System.EventHandler(this.m_cbResolution_SelectedIndexChanged);
            // 
            // ADC_Resolution_label
            // 
            this.ADC_Resolution_label.AutoSize = true;
            this.ADC_Resolution_label.Location = new System.Drawing.Point(51, 64);
            this.ADC_Resolution_label.Name = "ADC_Resolution_label";
            this.ADC_Resolution_label.Size = new System.Drawing.Size(57, 13);
            this.ADC_Resolution_label.TabIndex = 7;
            this.ADC_Resolution_label.Text = "Resolution";
            // 
            // m_cbConvMode
            // 
            this.m_cbConvMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbConvMode.FormattingEnabled = true;
            this.m_cbConvMode.Location = new System.Drawing.Point(121, 38);
            this.m_cbConvMode.Name = "m_cbConvMode";
            this.m_cbConvMode.Size = new System.Drawing.Size(121, 21);
            this.m_cbConvMode.TabIndex = 6;
            this.m_cbConvMode.SelectedIndexChanged += new System.EventHandler(this.m_cbConvMode_SelectedIndexChanged);
            // 
            // Conv_Mode_label
            // 
            this.Conv_Mode_label.AutoSize = true;
            this.Conv_Mode_label.Location = new System.Drawing.Point(18, 41);
            this.Conv_Mode_label.Name = "Conv_Mode_label";
            this.Conv_Mode_label.Size = new System.Drawing.Size(90, 13);
            this.Conv_Mode_label.TabIndex = 5;
            this.Conv_Mode_label.Text = "Conversion Mode";
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
            this.ADC_Power_Label.Location = new System.Drawing.Point(71, 16);
            this.ADC_Power_Label.Name = "ADC_Power_Label";
            this.ADC_Power_Label.Size = new System.Drawing.Size(37, 13);
            this.ADC_Power_Label.TabIndex = 3;
            this.ADC_Power_Label.Text = "Power";
            // 
            // ADC_Modes_groupBox
            // 
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbReference);
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbInputBufferGain);
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbInputRange);
            this.ADC_Modes_groupBox.Controls.Add(this.Reference_label);
            this.ADC_Modes_groupBox.Controls.Add(this.Input_Buffer_Gain_label);
            this.ADC_Modes_groupBox.Controls.Add(this.ADC_Input_Range_label);
            this.ADC_Modes_groupBox.Location = new System.Drawing.Point(13, 140);
            this.ADC_Modes_groupBox.Name = "ADC_Modes_groupBox";
            this.ADC_Modes_groupBox.Size = new System.Drawing.Size(396, 91);
            this.ADC_Modes_groupBox.TabIndex = 3;
            this.ADC_Modes_groupBox.TabStop = false;
            this.ADC_Modes_groupBox.Text = "Input";
            // 
            // m_cbReference
            // 
            this.m_cbReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbReference.FormattingEnabled = true;
            this.m_cbReference.Location = new System.Drawing.Point(121, 52);
            this.m_cbReference.Name = "m_cbReference";
            this.m_cbReference.Size = new System.Drawing.Size(149, 21);
            this.m_cbReference.TabIndex = 13;
            this.m_cbReference.SelectedIndexChanged += new System.EventHandler(this.m_cbReference_SelectedIndexChanged);
            // 
            // m_cbInputBufferGain
            // 
            this.m_cbInputBufferGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbInputBufferGain.FormattingEnabled = true;
            this.m_cbInputBufferGain.Location = new System.Drawing.Point(121, 30);
            this.m_cbInputBufferGain.Name = "m_cbInputBufferGain";
            this.m_cbInputBufferGain.Size = new System.Drawing.Size(149, 21);
            this.m_cbInputBufferGain.TabIndex = 12;
            this.m_cbInputBufferGain.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferGain_SelectedIndexChanged);
            // 
            // m_cbInputRange
            // 
            this.m_cbInputRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbInputRange.FormattingEnabled = true;
            this.m_cbInputRange.Location = new System.Drawing.Point(121, 8);
            this.m_cbInputRange.Name = "m_cbInputRange";
            this.m_cbInputRange.Size = new System.Drawing.Size(269, 21);
            this.m_cbInputRange.TabIndex = 11;
            this.m_cbInputRange.SelectedIndexChanged += new System.EventHandler(this.m_cbInputRange_SelectedIndexChanged);
            // 
            // Reference_label
            // 
            this.Reference_label.AutoSize = true;
            this.Reference_label.Location = new System.Drawing.Point(51, 60);
            this.Reference_label.Name = "Reference_label";
            this.Reference_label.Size = new System.Drawing.Size(57, 13);
            this.Reference_label.TabIndex = 4;
            this.Reference_label.Text = "Reference";
            // 
            // Input_Buffer_Gain_label
            // 
            this.Input_Buffer_Gain_label.AutoSize = true;
            this.Input_Buffer_Gain_label.Location = new System.Drawing.Point(21, 38);
            this.Input_Buffer_Gain_label.Name = "Input_Buffer_Gain_label";
            this.Input_Buffer_Gain_label.Size = new System.Drawing.Size(87, 13);
            this.Input_Buffer_Gain_label.TabIndex = 10;
            this.Input_Buffer_Gain_label.Text = "Input Buffer Gain";
            // 
            // ADC_Input_Range_label
            // 
            this.ADC_Input_Range_label.AutoSize = true;
            this.ADC_Input_Range_label.Location = new System.Drawing.Point(42, 16);
            this.ADC_Input_Range_label.Name = "ADC_Input_Range_label";
            this.ADC_Input_Range_label.Size = new System.Drawing.Size(66, 13);
            this.ADC_Input_Range_label.TabIndex = 9;
            this.ADC_Input_Range_label.Text = "Input Range";
            // 
            // m_gbClockSource
            // 
            this.m_gbClockSource.Controls.Add(this.m_rbClockExternal);
            this.m_gbClockSource.Controls.Add(this.m_rbClockInternal);
            this.m_gbClockSource.Location = new System.Drawing.Point(295, 3);
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
            this.m_gbSOC.Controls.Add(this.m_rbSocHardware);
            this.m_gbSOC.Controls.Add(this.m_rbSocSoftware);
            this.m_gbSOC.Location = new System.Drawing.Point(295, 71);
            this.m_gbSOC.Name = "m_gbSOC";
            this.m_gbSOC.Size = new System.Drawing.Size(114, 63);
            this.m_gbSOC.TabIndex = 5;
            this.m_gbSOC.TabStop = false;
            this.m_gbSOC.Text = "Start of Conversion";
            // 
            // m_rbSocHardware
            // 
            this.m_rbSocHardware.AutoSize = true;
            this.m_rbSocHardware.Location = new System.Drawing.Point(6, 37);
            this.m_rbSocHardware.Name = "m_rbSocHardware";
            this.m_rbSocHardware.Size = new System.Drawing.Size(71, 17);
            this.m_rbSocHardware.TabIndex = 1;
            this.m_rbSocHardware.Text = "Hardware";
            this.m_rbSocHardware.UseVisualStyleBackColor = true;
            this.m_rbSocHardware.CheckedChanged += new System.EventHandler(this.m_rbSocHardware_CheckedChanged);
            // 
            // m_rbSocSoftware
            // 
            this.m_rbSocSoftware.AutoSize = true;
            this.m_rbSocSoftware.Location = new System.Drawing.Point(6, 18);
            this.m_rbSocSoftware.Name = "m_rbSocSoftware";
            this.m_rbSocSoftware.Size = new System.Drawing.Size(67, 17);
            this.m_rbSocSoftware.TabIndex = 0;
            this.m_rbSocSoftware.Text = "Software";
            this.m_rbSocSoftware.UseVisualStyleBackColor = true;
            this.m_rbSocSoftware.CheckedChanged += new System.EventHandler(this.m_rbSocSoftware_CheckedChanged);
            // 
            // CyADC_DelSigControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gbSOC);
            this.Controls.Add(this.m_gbClockSource);
            this.Controls.Add(this.ADC_Modes_groupBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyADC_DelSigControl1";
            this.Size = new System.Drawing.Size(437, 298);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudConvRate)).EndInit();
            this.ADC_Modes_groupBox.ResumeLayout(false);
            this.ADC_Modes_groupBox.PerformLayout();
            this.m_gbClockSource.ResumeLayout(false);
            this.m_gbClockSource.PerformLayout();
            this.m_gbSOC.ResumeLayout(false);
            this.m_gbSOC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox ADC_Modes_groupBox;
        private System.Windows.Forms.Label ADC_Power_Label;
        private System.Windows.Forms.Label ADC_Resolution_label;
        private System.Windows.Forms.ComboBox m_cbConvMode;
        private System.Windows.Forms.Label Conv_Mode_label;
        private System.Windows.Forms.ComboBox m_cbPower;
        private System.Windows.Forms.Label ADC_Input_Range_label;
        private System.Windows.Forms.ComboBox m_cbResolution;
        private System.Windows.Forms.Label Input_Buffer_Gain_label;
        private System.Windows.Forms.ComboBox m_cbReference;
        private System.Windows.Forms.ComboBox m_cbInputBufferGain;
        private System.Windows.Forms.ComboBox m_cbInputRange;
        private System.Windows.Forms.Label Reference_label;
        private System.Windows.Forms.GroupBox m_gbClockSource;
        private System.Windows.Forms.RadioButton m_rbClockExternal;
        private System.Windows.Forms.RadioButton m_rbClockInternal;
        private System.Windows.Forms.GroupBox m_gbSOC;
        private System.Windows.Forms.RadioButton m_rbSocSoftware;
        private System.Windows.Forms.RadioButton m_rbSocHardware;
        private CyNumericUpDown m_nudConvRate;
        private System.Windows.Forms.Label ConvRate_label;
        private System.Windows.Forms.Label SamplesPerSec_label;
    }
}
