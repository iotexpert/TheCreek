namespace ADC_DelSig_v1_20
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyADC_DelSigControl1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_tbClockFreq = new System.Windows.Forms.TextBox();
            this.freq_label = new System.Windows.Forms.Label();
            this.ClockFreqency_Label = new System.Windows.Forms.Label();
            this.SamplesPerSec_label = new System.Windows.Forms.Label();
            this.ConvRate_label = new System.Windows.Forms.Label();
            this.m_cbResolution = new System.Windows.Forms.ComboBox();
            this.ADC_Resolution_label = new System.Windows.Forms.Label();
            this.m_cbConvMode = new System.Windows.Forms.ComboBox();
            this.Conv_Mode_label = new System.Windows.Forms.Label();
            this.m_cbPower = new System.Windows.Forms.ComboBox();
            this.ADC_Power_Label = new System.Windows.Forms.Label();
            this.ADC_Modes_groupBox = new System.Windows.Forms.GroupBox();
            this.Volts_label = new System.Windows.Forms.Label();
            this.External_Ref_label = new System.Windows.Forms.Label();
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
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_nudRefVoltage = new ADC_DelSig_v1_20.CyNumericUpDown();
            this.m_nudConvRate = new ADC_DelSig_v1_20.CyNumericUpDown();
            this.groupBox1.SuspendLayout();
            this.ADC_Modes_groupBox.SuspendLayout();
            this.m_gbClockSource.SuspendLayout();
            this.m_gbSOC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRefVoltage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudConvRate)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_tbClockFreq);
            this.groupBox1.Controls.Add(this.freq_label);
            this.groupBox1.Controls.Add(this.ClockFreqency_Label);
            this.groupBox1.Controls.Add(this.SamplesPerSec_label);
            this.groupBox1.Controls.Add(this.m_nudConvRate);
            this.groupBox1.Controls.Add(this.ConvRate_label);
            this.groupBox1.Controls.Add(this.m_cbResolution);
            this.groupBox1.Controls.Add(this.ADC_Resolution_label);
            this.groupBox1.Controls.Add(this.m_cbConvMode);
            this.groupBox1.Controls.Add(this.Conv_Mode_label);
            this.groupBox1.Controls.Add(this.m_cbPower);
            this.groupBox1.Controls.Add(this.ADC_Power_Label);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_tbClockFreq
            // 
            resources.ApplyResources(this.m_tbClockFreq, "m_tbClockFreq");
            this.m_tbClockFreq.Name = "m_tbClockFreq";
            this.m_tbClockFreq.ReadOnly = true;
            this.m_tbClockFreq.TabStop = false;
            // 
            // freq_label
            // 
            resources.ApplyResources(this.freq_label, "freq_label");
            this.freq_label.Name = "freq_label";
            // 
            // ClockFreqency_Label
            // 
            resources.ApplyResources(this.ClockFreqency_Label, "ClockFreqency_Label");
            this.ClockFreqency_Label.Name = "ClockFreqency_Label";
            // 
            // SamplesPerSec_label
            // 
            resources.ApplyResources(this.SamplesPerSec_label, "SamplesPerSec_label");
            this.SamplesPerSec_label.Name = "SamplesPerSec_label";
            // 
            // ConvRate_label
            // 
            resources.ApplyResources(this.ConvRate_label, "ConvRate_label");
            this.ConvRate_label.Name = "ConvRate_label";
            // 
            // m_cbResolution
            // 
            this.m_cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbResolution.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbResolution, "m_cbResolution");
            this.m_cbResolution.Name = "m_cbResolution";
            this.m_cbResolution.SelectedIndexChanged += new System.EventHandler(this.m_cbResolution_SelectedIndexChanged);
            // 
            // ADC_Resolution_label
            // 
            resources.ApplyResources(this.ADC_Resolution_label, "ADC_Resolution_label");
            this.ADC_Resolution_label.Name = "ADC_Resolution_label";
            // 
            // m_cbConvMode
            // 
            this.m_cbConvMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbConvMode.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbConvMode, "m_cbConvMode");
            this.m_cbConvMode.Name = "m_cbConvMode";
            this.m_cbConvMode.SelectedIndexChanged += new System.EventHandler(this.m_cbConvMode_SelectedIndexChanged);
            // 
            // Conv_Mode_label
            // 
            resources.ApplyResources(this.Conv_Mode_label, "Conv_Mode_label");
            this.Conv_Mode_label.Name = "Conv_Mode_label";
            // 
            // m_cbPower
            // 
            this.m_cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPower.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbPower, "m_cbPower");
            this.m_cbPower.Name = "m_cbPower";
            this.m_cbPower.SelectedIndexChanged += new System.EventHandler(this.m_cbPower_SelectedIndexChanged);
            // 
            // ADC_Power_Label
            // 
            resources.ApplyResources(this.ADC_Power_Label, "ADC_Power_Label");
            this.ADC_Power_Label.Name = "ADC_Power_Label";
            // 
            // ADC_Modes_groupBox
            // 
            this.ADC_Modes_groupBox.Controls.Add(this.m_nudRefVoltage);
            this.ADC_Modes_groupBox.Controls.Add(this.Volts_label);
            this.ADC_Modes_groupBox.Controls.Add(this.External_Ref_label);
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbReference);
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbInputBufferGain);
            this.ADC_Modes_groupBox.Controls.Add(this.m_cbInputRange);
            this.ADC_Modes_groupBox.Controls.Add(this.Reference_label);
            this.ADC_Modes_groupBox.Controls.Add(this.Input_Buffer_Gain_label);
            this.ADC_Modes_groupBox.Controls.Add(this.ADC_Input_Range_label);
            resources.ApplyResources(this.ADC_Modes_groupBox, "ADC_Modes_groupBox");
            this.ADC_Modes_groupBox.Name = "ADC_Modes_groupBox";
            this.ADC_Modes_groupBox.TabStop = false;
            // 
            // Volts_label
            // 
            resources.ApplyResources(this.Volts_label, "Volts_label");
            this.Volts_label.Name = "Volts_label";
            // 
            // External_Ref_label
            // 
            resources.ApplyResources(this.External_Ref_label, "External_Ref_label");
            this.External_Ref_label.Name = "External_Ref_label";
            // 
            // m_cbReference
            // 
            this.m_cbReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbReference.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbReference, "m_cbReference");
            this.m_cbReference.Name = "m_cbReference";
            this.m_cbReference.SelectedIndexChanged += new System.EventHandler(this.m_cbReference_SelectedIndexChanged);
            // 
            // m_cbInputBufferGain
            // 
            this.m_cbInputBufferGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbInputBufferGain.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbInputBufferGain, "m_cbInputBufferGain");
            this.m_cbInputBufferGain.Name = "m_cbInputBufferGain";
            this.m_cbInputBufferGain.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferGain_SelectedIndexChanged);
            // 
            // m_cbInputRange
            // 
            this.m_cbInputRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbInputRange.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbInputRange, "m_cbInputRange");
            this.m_cbInputRange.Name = "m_cbInputRange";
            this.m_cbInputRange.SelectedIndexChanged += new System.EventHandler(this.m_cbInputRange_SelectedIndexChanged);
            // 
            // Reference_label
            // 
            resources.ApplyResources(this.Reference_label, "Reference_label");
            this.Reference_label.Name = "Reference_label";
            // 
            // Input_Buffer_Gain_label
            // 
            resources.ApplyResources(this.Input_Buffer_Gain_label, "Input_Buffer_Gain_label");
            this.Input_Buffer_Gain_label.Name = "Input_Buffer_Gain_label";
            // 
            // ADC_Input_Range_label
            // 
            resources.ApplyResources(this.ADC_Input_Range_label, "ADC_Input_Range_label");
            this.ADC_Input_Range_label.Name = "ADC_Input_Range_label";
            // 
            // m_gbClockSource
            // 
            this.m_gbClockSource.Controls.Add(this.m_rbClockExternal);
            this.m_gbClockSource.Controls.Add(this.m_rbClockInternal);
            resources.ApplyResources(this.m_gbClockSource, "m_gbClockSource");
            this.m_gbClockSource.Name = "m_gbClockSource";
            this.m_gbClockSource.TabStop = false;
            // 
            // m_rbClockExternal
            // 
            resources.ApplyResources(this.m_rbClockExternal, "m_rbClockExternal");
            this.m_rbClockExternal.Name = "m_rbClockExternal";
            this.m_rbClockExternal.TabStop = true;
            this.m_rbClockExternal.UseVisualStyleBackColor = true;
            this.m_rbClockExternal.CheckedChanged += new System.EventHandler(this.m_rbClockExternal_CheckedChanged);
            // 
            // m_rbClockInternal
            // 
            resources.ApplyResources(this.m_rbClockInternal, "m_rbClockInternal");
            this.m_rbClockInternal.Name = "m_rbClockInternal";
            this.m_rbClockInternal.TabStop = true;
            this.m_rbClockInternal.UseVisualStyleBackColor = true;
            this.m_rbClockInternal.CheckedChanged += new System.EventHandler(this.m_rbClockInternal_CheckedChanged);
            // 
            // m_gbSOC
            // 
            this.m_gbSOC.Controls.Add(this.m_rbSocHardware);
            this.m_gbSOC.Controls.Add(this.m_rbSocSoftware);
            resources.ApplyResources(this.m_gbSOC, "m_gbSOC");
            this.m_gbSOC.Name = "m_gbSOC";
            this.m_gbSOC.TabStop = false;
            // 
            // m_rbSocHardware
            // 
            resources.ApplyResources(this.m_rbSocHardware, "m_rbSocHardware");
            this.m_rbSocHardware.Name = "m_rbSocHardware";
            this.m_rbSocHardware.TabStop = true;
            this.m_rbSocHardware.UseVisualStyleBackColor = true;
            this.m_rbSocHardware.CheckedChanged += new System.EventHandler(this.m_rbSocHardware_CheckedChanged);
            // 
            // m_rbSocSoftware
            // 
            resources.ApplyResources(this.m_rbSocSoftware, "m_rbSocSoftware");
            this.m_rbSocSoftware.Name = "m_rbSocSoftware";
            this.m_rbSocSoftware.TabStop = true;
            this.m_rbSocSoftware.UseVisualStyleBackColor = true;
            this.m_rbSocSoftware.CheckedChanged += new System.EventHandler(this.m_rbSocSoftware_CheckedChanged);
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkRate = 0;
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            resources.ApplyResources(this.m_errorProvider, "m_errorProvider");
            // 
            // m_nudRefVoltage
            // 
            this.m_nudRefVoltage.DecimalPlaces = 4;
            this.m_nudRefVoltage.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            resources.ApplyResources(this.m_nudRefVoltage, "m_nudRefVoltage");
            this.m_nudRefVoltage.Maximum = new decimal(new int[] {
            1500,
            0,
            0,
            196608});
            this.m_nudRefVoltage.Name = "m_nudRefVoltage";
            this.m_nudRefVoltage.Value = new decimal(new int[] {
            1024,
            0,
            0,
            196608});
            this.m_nudRefVoltage.ValueChanged += new System.EventHandler(this.m_nudRefVoltage_ValueChanged);
            // 
            // m_nudConvRate
            // 
            resources.ApplyResources(this.m_nudConvRate, "m_nudConvRate");
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
            this.m_nudConvRate.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudConvRate.ValueChanged += new System.EventHandler(this.m_nudConvRate_ValueChanged);
            // 
            // CyADC_DelSigControl1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gbSOC);
            this.Controls.Add(this.m_gbClockSource);
            this.Controls.Add(this.ADC_Modes_groupBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyADC_DelSigControl1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ADC_Modes_groupBox.ResumeLayout(false);
            this.ADC_Modes_groupBox.PerformLayout();
            this.m_gbClockSource.ResumeLayout(false);
            this.m_gbClockSource.PerformLayout();
            this.m_gbSOC.ResumeLayout(false);
            this.m_gbSOC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRefVoltage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudConvRate)).EndInit();
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
        private System.Windows.Forms.ErrorProvider m_errorProvider;
        private System.Windows.Forms.Label freq_label;
        private System.Windows.Forms.Label ClockFreqency_Label;
        private System.Windows.Forms.TextBox m_tbClockFreq;
        private System.Windows.Forms.Label External_Ref_label;
        private System.Windows.Forms.Label Volts_label;
        private CyNumericUpDown m_nudRefVoltage;
    }
}
