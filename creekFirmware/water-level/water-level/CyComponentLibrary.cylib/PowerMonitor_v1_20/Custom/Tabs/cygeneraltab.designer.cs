namespace PowerMonitor_v1_20
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numNumConverters = new System.Windows.Forms.NumericUpDown();
            this.numAuxChannels = new System.Windows.Forms.NumericUpDown();
            this.cbVoltageFilterType = new System.Windows.Forms.ComboBox();
            this.cbCurrentFilterType = new System.Windows.Forms.ComboBox();
            this.cbAuxFilterType = new System.Windows.Forms.ComboBox();
            this.cbPgoodConfig = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbFaultOc = new System.Windows.Forms.CheckBox();
            this.chbFaultUv = new System.Windows.Forms.CheckBox();
            this.chbFaultOv = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbWarningOc = new System.Windows.Forms.CheckBox();
            this.chbWarningUv = new System.Windows.Forms.CheckBox();
            this.chbWarningOv = new System.Windows.Forms.CheckBox();
            this.lblPgoodDesc = new System.Windows.Forms.Label();
            this.cbDiffCurrentRange = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chbExposeCalibration = new System.Windows.Forms.CheckBox();
            this.m_lblLowVoltageMode = new System.Windows.Forms.Label();
            this.m_cbLowVoltageMode = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numNumConverters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAuxChannels)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of converters:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of auxilary channels:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Voltage filtering type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 111);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Current filtering type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 144);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Auxilary voltage filtering type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 245);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Pgood terminal:";
            // 
            // numNumConverters
            // 
            this.numNumConverters.Location = new System.Drawing.Point(268, 10);
            this.numNumConverters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numNumConverters.Name = "numNumConverters";
            this.numNumConverters.Size = new System.Drawing.Size(149, 22);
            this.numNumConverters.TabIndex = 6;
            // 
            // numAuxChannels
            // 
            this.numAuxChannels.Location = new System.Drawing.Point(268, 42);
            this.numAuxChannels.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numAuxChannels.Name = "numAuxChannels";
            this.numAuxChannels.Size = new System.Drawing.Size(149, 22);
            this.numAuxChannels.TabIndex = 7;
            // 
            // cbVoltageFilterType
            // 
            this.cbVoltageFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVoltageFilterType.FormattingEnabled = true;
            this.cbVoltageFilterType.Location = new System.Drawing.Point(268, 74);
            this.cbVoltageFilterType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbVoltageFilterType.Name = "cbVoltageFilterType";
            this.cbVoltageFilterType.Size = new System.Drawing.Size(149, 24);
            this.cbVoltageFilterType.TabIndex = 8;
            this.cbVoltageFilterType.SelectedIndexChanged += new System.EventHandler(this.cbVoltageFilterType_SelectedIndexChanged);
            // 
            // cbCurrentFilterType
            // 
            this.cbCurrentFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentFilterType.FormattingEnabled = true;
            this.cbCurrentFilterType.Location = new System.Drawing.Point(268, 107);
            this.cbCurrentFilterType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCurrentFilterType.Name = "cbCurrentFilterType";
            this.cbCurrentFilterType.Size = new System.Drawing.Size(149, 24);
            this.cbCurrentFilterType.TabIndex = 9;
            this.cbCurrentFilterType.SelectedIndexChanged += new System.EventHandler(this.cbCurrentFilterType_SelectedIndexChanged);
            // 
            // cbAuxFilterType
            // 
            this.cbAuxFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuxFilterType.FormattingEnabled = true;
            this.cbAuxFilterType.Location = new System.Drawing.Point(268, 140);
            this.cbAuxFilterType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAuxFilterType.Name = "cbAuxFilterType";
            this.cbAuxFilterType.Size = new System.Drawing.Size(149, 24);
            this.cbAuxFilterType.TabIndex = 10;
            this.cbAuxFilterType.SelectedIndexChanged += new System.EventHandler(this.cbAuxFilterType_SelectedIndexChanged);
            // 
            // cbPgoodConfig
            // 
            this.cbPgoodConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPgoodConfig.FormattingEnabled = true;
            this.cbPgoodConfig.Location = new System.Drawing.Point(268, 241);
            this.cbPgoodConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPgoodConfig.Name = "cbPgoodConfig";
            this.cbPgoodConfig.Size = new System.Drawing.Size(149, 24);
            this.cbPgoodConfig.TabIndex = 13;
            this.cbPgoodConfig.SelectedIndexChanged += new System.EventHandler(this.cbPgoodConfig_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbFaultOc);
            this.groupBox1.Controls.Add(this.chbFaultUv);
            this.groupBox1.Controls.Add(this.chbFaultOv);
            this.groupBox1.Location = new System.Drawing.Point(21, 274);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(195, 108);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fault sources";
            // 
            // chbFaultOc
            // 
            this.chbFaultOc.AutoSize = true;
            this.chbFaultOc.Location = new System.Drawing.Point(8, 80);
            this.chbFaultOc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbFaultOc.Name = "chbFaultOc";
            this.chbFaultOc.Size = new System.Drawing.Size(47, 21);
            this.chbFaultOc.TabIndex = 2;
            this.chbFaultOc.Text = "OC";
            this.chbFaultOc.UseVisualStyleBackColor = true;
            this.chbFaultOc.CheckedChanged += new System.EventHandler(this.chbFaultOc_CheckedChanged);
            // 
            // chbFaultUv
            // 
            this.chbFaultUv.AutoSize = true;
            this.chbFaultUv.Location = new System.Drawing.Point(8, 52);
            this.chbFaultUv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbFaultUv.Name = "chbFaultUv";
            this.chbFaultUv.Size = new System.Drawing.Size(46, 21);
            this.chbFaultUv.TabIndex = 1;
            this.chbFaultUv.Text = "UV";
            this.chbFaultUv.UseVisualStyleBackColor = true;
            this.chbFaultUv.CheckedChanged += new System.EventHandler(this.chbFaultUv_CheckedChanged);
            // 
            // chbFaultOv
            // 
            this.chbFaultOv.AutoSize = true;
            this.chbFaultOv.Location = new System.Drawing.Point(8, 23);
            this.chbFaultOv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbFaultOv.Name = "chbFaultOv";
            this.chbFaultOv.Size = new System.Drawing.Size(47, 21);
            this.chbFaultOv.TabIndex = 0;
            this.chbFaultOv.Text = "OV";
            this.chbFaultOv.UseVisualStyleBackColor = true;
            this.chbFaultOv.CheckedChanged += new System.EventHandler(this.chbFaultOv_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbWarningOc);
            this.groupBox2.Controls.Add(this.chbWarningUv);
            this.groupBox2.Controls.Add(this.chbWarningOv);
            this.groupBox2.Location = new System.Drawing.Point(224, 274);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(195, 108);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Warning sources";
            // 
            // chbWarningOc
            // 
            this.chbWarningOc.AutoSize = true;
            this.chbWarningOc.Location = new System.Drawing.Point(8, 80);
            this.chbWarningOc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbWarningOc.Name = "chbWarningOc";
            this.chbWarningOc.Size = new System.Drawing.Size(47, 21);
            this.chbWarningOc.TabIndex = 2;
            this.chbWarningOc.Text = "OC";
            this.chbWarningOc.UseVisualStyleBackColor = true;
            this.chbWarningOc.CheckedChanged += new System.EventHandler(this.chbWarningOc_CheckedChanged);
            // 
            // chbWarningUv
            // 
            this.chbWarningUv.AutoSize = true;
            this.chbWarningUv.Location = new System.Drawing.Point(8, 52);
            this.chbWarningUv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbWarningUv.Name = "chbWarningUv";
            this.chbWarningUv.Size = new System.Drawing.Size(46, 21);
            this.chbWarningUv.TabIndex = 1;
            this.chbWarningUv.Text = "UV";
            this.chbWarningUv.UseVisualStyleBackColor = true;
            this.chbWarningUv.CheckedChanged += new System.EventHandler(this.chbWarningUv_CheckedChanged);
            // 
            // chbWarningOv
            // 
            this.chbWarningOv.AutoSize = true;
            this.chbWarningOv.Location = new System.Drawing.Point(8, 23);
            this.chbWarningOv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbWarningOv.Name = "chbWarningOv";
            this.chbWarningOv.Size = new System.Drawing.Size(47, 21);
            this.chbWarningOv.TabIndex = 0;
            this.chbWarningOv.Text = "OV";
            this.chbWarningOv.UseVisualStyleBackColor = true;
            this.chbWarningOv.CheckedChanged += new System.EventHandler(this.chbWarningOv_CheckedChanged);
            // 
            // lblPgoodDesc
            // 
            this.lblPgoodDesc.AutoSize = true;
            this.lblPgoodDesc.Location = new System.Drawing.Point(427, 245);
            this.lblPgoodDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPgoodDesc.Name = "lblPgoodDesc";
            this.lblPgoodDesc.Size = new System.Drawing.Size(95, 17);
            this.lblPgoodDesc.TabIndex = 14;
            this.lblPgoodDesc.Text = "lblPgoodDesc";
            // 
            // cbDiffCurrentRange
            // 
            this.cbDiffCurrentRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiffCurrentRange.FormattingEnabled = true;
            this.cbDiffCurrentRange.Location = new System.Drawing.Point(268, 208);
            this.cbDiffCurrentRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDiffCurrentRange.Name = "cbDiffCurrentRange";
            this.cbDiffCurrentRange.Size = new System.Drawing.Size(149, 24);
            this.cbDiffCurrentRange.TabIndex = 11;
            this.cbDiffCurrentRange.SelectedIndexChanged += new System.EventHandler(this.cbDiffCurrentRange_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 212);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Current sensing ADC range:";
            // 
            // chbExposeCalibration
            // 
            this.chbExposeCalibration.AutoSize = true;
            this.chbExposeCalibration.Location = new System.Drawing.Point(431, 212);
            this.chbExposeCalibration.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbExposeCalibration.Name = "chbExposeCalibration";
            this.chbExposeCalibration.Size = new System.Drawing.Size(142, 21);
            this.chbExposeCalibration.TabIndex = 12;
            this.chbExposeCalibration.Text = "Expose calibration";
            this.chbExposeCalibration.UseVisualStyleBackColor = true;
            this.chbExposeCalibration.CheckedChanged += new System.EventHandler(this.chbExposeCalibration_CheckedChanged);
            // 
            // m_lblLowVoltageMode
            // 
            this.m_lblLowVoltageMode.AutoSize = true;
            this.m_lblLowVoltageMode.Location = new System.Drawing.Point(17, 178);
            this.m_lblLowVoltageMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblLowVoltageMode.Name = "m_lblLowVoltageMode";
            this.m_lblLowVoltageMode.Size = new System.Drawing.Size(186, 17);
            this.m_lblLowVoltageMode.TabIndex = 17;
            this.m_lblLowVoltageMode.Text = "Voltage sensing ADC range:";
            // 
            // m_cbLowVoltageMode
            // 
            this.m_cbLowVoltageMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbLowVoltageMode.FormattingEnabled = true;
            this.m_cbLowVoltageMode.Location = new System.Drawing.Point(268, 175);
            this.m_cbLowVoltageMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_cbLowVoltageMode.Name = "m_cbLowVoltageMode";
            this.m_cbLowVoltageMode.Size = new System.Drawing.Size(149, 24);
            this.m_cbLowVoltageMode.TabIndex = 18;
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cbLowVoltageMode);
            this.Controls.Add(this.m_lblLowVoltageMode);
            this.Controls.Add(this.chbExposeCalibration);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbDiffCurrentRange);
            this.Controls.Add(this.lblPgoodDesc);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbPgoodConfig);
            this.Controls.Add(this.cbAuxFilterType);
            this.Controls.Add(this.cbCurrentFilterType);
            this.Controls.Add(this.cbVoltageFilterType);
            this.Controls.Add(this.numAuxChannels);
            this.Controls.Add(this.numNumConverters);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(756, 524);
            this.Load += new System.EventHandler(this.CyBasicTab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numNumConverters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAuxChannels)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numNumConverters;
        private System.Windows.Forms.NumericUpDown numAuxChannels;
        private System.Windows.Forms.ComboBox cbVoltageFilterType;
        private System.Windows.Forms.ComboBox cbCurrentFilterType;
        private System.Windows.Forms.ComboBox cbAuxFilterType;
        private System.Windows.Forms.ComboBox cbPgoodConfig;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbFaultOc;
        private System.Windows.Forms.CheckBox chbFaultUv;
        private System.Windows.Forms.CheckBox chbFaultOv;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbWarningOc;
        private System.Windows.Forms.CheckBox chbWarningUv;
        private System.Windows.Forms.CheckBox chbWarningOv;
        private System.Windows.Forms.Label lblPgoodDesc;
        private System.Windows.Forms.ComboBox cbDiffCurrentRange;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chbExposeCalibration;
        private System.Windows.Forms.Label m_lblLowVoltageMode;
        private System.Windows.Forms.ComboBox m_cbLowVoltageMode;
    }
}
