namespace SegLCD_v1_0
{
    partial class CyBasicConfiguration
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
            this.comboBoxBiasVoltage = new System.Windows.Forms.ComboBox();
            this.comboBoxFrameRate = new System.Windows.Forms.ComboBox();
            this.comboBoxWaveformType = new System.Windows.Forms.ComboBox();
            this.labelBiasVoltage = new System.Windows.Forms.Label();
            this.labelFrameRate = new System.Windows.Forms.Label();
            this.labelWaveformType = new System.Windows.Forms.Label();
            this.labelBiasType = new System.Windows.Forms.Label();
            this.numUpDownNumSegmentLines = new System.Windows.Forms.NumericUpDown();
            this.labelNumSegmentLines = new System.Windows.Forms.Label();
            this.labelNumCommonLines = new System.Windows.Forms.Label();
            this.numUpDownNumCommonLines = new System.Windows.Forms.NumericUpDown();
            this.editBiasType = new System.Windows.Forms.Label();
            this.checkBoxGang = new System.Windows.Forms.CheckBox();
            this.checkBoxDebugMode = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumSegmentLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumCommonLines)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxBiasVoltage
            // 
            this.comboBoxBiasVoltage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBiasVoltage.FormattingEnabled = true;
            this.comboBoxBiasVoltage.Location = new System.Drawing.Point(198, 176);
            this.comboBoxBiasVoltage.Name = "comboBoxBiasVoltage";
            this.comboBoxBiasVoltage.Size = new System.Drawing.Size(110, 21);
            this.comboBoxBiasVoltage.TabIndex = 27;
            this.comboBoxBiasVoltage.SelectedIndexChanged += new System.EventHandler(this.comboBoxBiasVoltage_SelectedIndexChanged);
            // 
            // comboBoxFrameRate
            // 
            this.comboBoxFrameRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrameRate.FormattingEnabled = true;
            this.comboBoxFrameRate.Location = new System.Drawing.Point(198, 149);
            this.comboBoxFrameRate.Name = "comboBoxFrameRate";
            this.comboBoxFrameRate.Size = new System.Drawing.Size(110, 21);
            this.comboBoxFrameRate.TabIndex = 26;
            this.comboBoxFrameRate.SelectedIndexChanged += new System.EventHandler(this.comboBoxFrameRate_SelectedIndexChanged);
            // 
            // comboBoxWaveformType
            // 
            this.comboBoxWaveformType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWaveformType.FormattingEnabled = true;
            this.comboBoxWaveformType.Items.AddRange(new object[] {
            "Type A Standard",
            "Type B Low Power"});
            this.comboBoxWaveformType.Location = new System.Drawing.Point(198, 122);
            this.comboBoxWaveformType.Name = "comboBoxWaveformType";
            this.comboBoxWaveformType.Size = new System.Drawing.Size(110, 21);
            this.comboBoxWaveformType.TabIndex = 25;
            this.comboBoxWaveformType.SelectedIndexChanged += new System.EventHandler(this.comboBoxWaveformType_SelectedIndexChanged);
            // 
            // labelBiasVoltage
            // 
            this.labelBiasVoltage.AutoSize = true;
            this.labelBiasVoltage.Location = new System.Drawing.Point(3, 180);
            this.labelBiasVoltage.Name = "labelBiasVoltage";
            this.labelBiasVoltage.Size = new System.Drawing.Size(78, 13);
            this.labelBiasVoltage.TabIndex = 23;
            this.labelBiasVoltage.Text = "Bias voltage, V";
            // 
            // labelFrameRate
            // 
            this.labelFrameRate.AutoSize = true;
            this.labelFrameRate.Location = new System.Drawing.Point(3, 152);
            this.labelFrameRate.Name = "labelFrameRate";
            this.labelFrameRate.Size = new System.Drawing.Size(76, 13);
            this.labelFrameRate.TabIndex = 22;
            this.labelFrameRate.Text = "Frame rate, Hz";
            // 
            // labelWaveformType
            // 
            this.labelWaveformType.AutoSize = true;
            this.labelWaveformType.Location = new System.Drawing.Point(3, 125);
            this.labelWaveformType.Name = "labelWaveformType";
            this.labelWaveformType.Size = new System.Drawing.Size(79, 13);
            this.labelWaveformType.TabIndex = 21;
            this.labelWaveformType.Text = "Waveform type";
            // 
            // labelBiasType
            // 
            this.labelBiasType.AutoSize = true;
            this.labelBiasType.Location = new System.Drawing.Point(3, 102);
            this.labelBiasType.Name = "labelBiasType";
            this.labelBiasType.Size = new System.Drawing.Size(50, 13);
            this.labelBiasType.TabIndex = 20;
            this.labelBiasType.Text = "Bias type";
            // 
            // numUpDownNumSegmentLines
            // 
            this.numUpDownNumSegmentLines.Location = new System.Drawing.Point(198, 60);
            this.numUpDownNumSegmentLines.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.Name = "numUpDownNumSegmentLines";
            this.numUpDownNumSegmentLines.Size = new System.Drawing.Size(110, 20);
            this.numUpDownNumSegmentLines.TabIndex = 17;
            this.numUpDownNumSegmentLines.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.ValueChanged += new System.EventHandler(this.numUpDownNumSegmentLines_ValueChanged);
            // 
            // labelNumSegmentLines
            // 
            this.labelNumSegmentLines.AutoSize = true;
            this.labelNumSegmentLines.Location = new System.Drawing.Point(3, 63);
            this.labelNumSegmentLines.Name = "labelNumSegmentLines";
            this.labelNumSegmentLines.Size = new System.Drawing.Size(123, 13);
            this.labelNumSegmentLines.TabIndex = 16;
            this.labelNumSegmentLines.Text = "Number of segment lines";
            // 
            // labelNumCommonLines
            // 
            this.labelNumCommonLines.AutoSize = true;
            this.labelNumCommonLines.Location = new System.Drawing.Point(3, 10);
            this.labelNumCommonLines.Name = "labelNumCommonLines";
            this.labelNumCommonLines.Size = new System.Drawing.Size(123, 13);
            this.labelNumCommonLines.TabIndex = 15;
            this.labelNumCommonLines.Text = "Number of common lines";
            // 
            // numUpDownNumCommonLines
            // 
            this.numUpDownNumCommonLines.Location = new System.Drawing.Point(198, 8);
            this.numUpDownNumCommonLines.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownNumCommonLines.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numUpDownNumCommonLines.Name = "numUpDownNumCommonLines";
            this.numUpDownNumCommonLines.Size = new System.Drawing.Size(110, 20);
            this.numUpDownNumCommonLines.TabIndex = 14;
            this.numUpDownNumCommonLines.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numUpDownNumCommonLines.ValueChanged += new System.EventHandler(this.numUpDownNumCommonLines_ValueChanged);
            // 
            // editBiasType
            // 
            this.editBiasType.BackColor = System.Drawing.SystemColors.Control;
            this.editBiasType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.editBiasType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.editBiasType.Location = new System.Drawing.Point(198, 98);
            this.editBiasType.Name = "editBiasType";
            this.editBiasType.Size = new System.Drawing.Size(110, 21);
            this.editBiasType.TabIndex = 28;
            this.editBiasType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxGang
            // 
            this.checkBoxGang.AutoSize = true;
            this.checkBoxGang.Location = new System.Drawing.Point(198, 34);
            this.checkBoxGang.Name = "checkBoxGang";
            this.checkBoxGang.Size = new System.Drawing.Size(151, 17);
            this.checkBoxGang.TabIndex = 29;
            this.checkBoxGang.Text = "Enable Ganging Commons";
            this.checkBoxGang.UseVisualStyleBackColor = true;
            this.checkBoxGang.CheckedChanged += new System.EventHandler(this.checkBoxGang_CheckedChanged);
            // 
            // checkBoxDebugMode
            // 
            this.checkBoxDebugMode.AutoSize = true;
            this.checkBoxDebugMode.Location = new System.Drawing.Point(6, 210);
            this.checkBoxDebugMode.Name = "checkBoxDebugMode";
            this.checkBoxDebugMode.Size = new System.Drawing.Size(124, 17);
            this.checkBoxDebugMode.TabIndex = 30;
            this.checkBoxDebugMode.Text = "Enable Debug Mode";
            this.checkBoxDebugMode.UseVisualStyleBackColor = true;
            this.checkBoxDebugMode.CheckedChanged += new System.EventHandler(this.checkBoxDebugMode_CheckedChanged);
            // 
            // CyBasicConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxDebugMode);
            this.Controls.Add(this.checkBoxGang);
            this.Controls.Add(this.editBiasType);
            this.Controls.Add(this.comboBoxBiasVoltage);
            this.Controls.Add(this.comboBoxFrameRate);
            this.Controls.Add(this.comboBoxWaveformType);
            this.Controls.Add(this.labelBiasVoltage);
            this.Controls.Add(this.labelFrameRate);
            this.Controls.Add(this.labelWaveformType);
            this.Controls.Add(this.labelBiasType);
            this.Controls.Add(this.numUpDownNumSegmentLines);
            this.Controls.Add(this.labelNumSegmentLines);
            this.Controls.Add(this.labelNumCommonLines);
            this.Controls.Add(this.numUpDownNumCommonLines);
            this.Name = "CyBasicConfiguration";
            this.Size = new System.Drawing.Size(443, 239);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumSegmentLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumCommonLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxBiasVoltage;
        private System.Windows.Forms.ComboBox comboBoxFrameRate;
        private System.Windows.Forms.ComboBox comboBoxWaveformType;
        private System.Windows.Forms.Label labelBiasVoltage;
        private System.Windows.Forms.Label labelFrameRate;
        private System.Windows.Forms.Label labelWaveformType;
        private System.Windows.Forms.Label labelBiasType;
        private System.Windows.Forms.NumericUpDown numUpDownNumSegmentLines;
        private System.Windows.Forms.Label labelNumSegmentLines;
        private System.Windows.Forms.Label labelNumCommonLines;
        private System.Windows.Forms.NumericUpDown numUpDownNumCommonLines;
        private System.Windows.Forms.Label editBiasType;
        private System.Windows.Forms.CheckBox checkBoxGang;
        private System.Windows.Forms.CheckBox checkBoxDebugMode;
    }
}
