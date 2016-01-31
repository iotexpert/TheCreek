namespace PowerMonitor_v1_0
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
            ((System.ComponentModel.ISupportInitialize)(this.numNumConverters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAuxChannels)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of converters:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of auxilary channels:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Voltage filtering type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Current filtering type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Auxilary voltage filtering type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Pgood terminal:";
            // 
            // numNumConverters
            // 
            this.numNumConverters.Location = new System.Drawing.Point(201, 8);
            this.numNumConverters.Name = "numNumConverters";
            this.numNumConverters.Size = new System.Drawing.Size(113, 20);
            this.numNumConverters.TabIndex = 6;
            // 
            // numAuxChannels
            // 
            this.numAuxChannels.Location = new System.Drawing.Point(201, 34);
            this.numAuxChannels.Name = "numAuxChannels";
            this.numAuxChannels.Size = new System.Drawing.Size(113, 20);
            this.numAuxChannels.TabIndex = 7;
            // 
            // cbVoltageFilterType
            // 
            this.cbVoltageFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVoltageFilterType.FormattingEnabled = true;
            this.cbVoltageFilterType.Location = new System.Drawing.Point(201, 60);
            this.cbVoltageFilterType.Name = "cbVoltageFilterType";
            this.cbVoltageFilterType.Size = new System.Drawing.Size(113, 21);
            this.cbVoltageFilterType.TabIndex = 8;
            this.cbVoltageFilterType.SelectedIndexChanged += new System.EventHandler(this.cbVoltageFilterType_SelectedIndexChanged);
            // 
            // cbCurrentFilterType
            // 
            this.cbCurrentFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentFilterType.FormattingEnabled = true;
            this.cbCurrentFilterType.Location = new System.Drawing.Point(201, 87);
            this.cbCurrentFilterType.Name = "cbCurrentFilterType";
            this.cbCurrentFilterType.Size = new System.Drawing.Size(113, 21);
            this.cbCurrentFilterType.TabIndex = 9;
            this.cbCurrentFilterType.SelectedIndexChanged += new System.EventHandler(this.cbCurrentFilterType_SelectedIndexChanged);
            // 
            // cbAuxFilterType
            // 
            this.cbAuxFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuxFilterType.FormattingEnabled = true;
            this.cbAuxFilterType.Location = new System.Drawing.Point(201, 114);
            this.cbAuxFilterType.Name = "cbAuxFilterType";
            this.cbAuxFilterType.Size = new System.Drawing.Size(113, 21);
            this.cbAuxFilterType.TabIndex = 10;
            this.cbAuxFilterType.SelectedIndexChanged += new System.EventHandler(this.cbAuxFilterType_SelectedIndexChanged);
            // 
            // cbPgoodConfig
            // 
            this.cbPgoodConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPgoodConfig.FormattingEnabled = true;
            this.cbPgoodConfig.Location = new System.Drawing.Point(201, 168);
            this.cbPgoodConfig.Name = "cbPgoodConfig";
            this.cbPgoodConfig.Size = new System.Drawing.Size(113, 21);
            this.cbPgoodConfig.TabIndex = 13;
            this.cbPgoodConfig.SelectedIndexChanged += new System.EventHandler(this.cbPgoodConfig_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbFaultOc);
            this.groupBox1.Controls.Add(this.chbFaultUv);
            this.groupBox1.Controls.Add(this.chbFaultOv);
            this.groupBox1.Location = new System.Drawing.Point(16, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 88);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fault sources:";
            // 
            // chbFaultOc
            // 
            this.chbFaultOc.AutoSize = true;
            this.chbFaultOc.Location = new System.Drawing.Point(6, 65);
            this.chbFaultOc.Name = "chbFaultOc";
            this.chbFaultOc.Size = new System.Drawing.Size(41, 17);
            this.chbFaultOc.TabIndex = 2;
            this.chbFaultOc.Text = "OC";
            this.chbFaultOc.UseVisualStyleBackColor = true;
            this.chbFaultOc.CheckedChanged += new System.EventHandler(this.chbFaultOc_CheckedChanged);
            // 
            // chbFaultUv
            // 
            this.chbFaultUv.AutoSize = true;
            this.chbFaultUv.Location = new System.Drawing.Point(6, 42);
            this.chbFaultUv.Name = "chbFaultUv";
            this.chbFaultUv.Size = new System.Drawing.Size(41, 17);
            this.chbFaultUv.TabIndex = 1;
            this.chbFaultUv.Text = "UV";
            this.chbFaultUv.UseVisualStyleBackColor = true;
            this.chbFaultUv.CheckedChanged += new System.EventHandler(this.chbFaultUv_CheckedChanged);
            // 
            // chbFaultOv
            // 
            this.chbFaultOv.AutoSize = true;
            this.chbFaultOv.Location = new System.Drawing.Point(6, 19);
            this.chbFaultOv.Name = "chbFaultOv";
            this.chbFaultOv.Size = new System.Drawing.Size(41, 17);
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
            this.groupBox2.Location = new System.Drawing.Point(168, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(146, 88);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Warning sources:";
            // 
            // chbWarningOc
            // 
            this.chbWarningOc.AutoSize = true;
            this.chbWarningOc.Location = new System.Drawing.Point(6, 65);
            this.chbWarningOc.Name = "chbWarningOc";
            this.chbWarningOc.Size = new System.Drawing.Size(41, 17);
            this.chbWarningOc.TabIndex = 2;
            this.chbWarningOc.Text = "OC";
            this.chbWarningOc.UseVisualStyleBackColor = true;
            this.chbWarningOc.CheckedChanged += new System.EventHandler(this.chbWarningOc_CheckedChanged);
            // 
            // chbWarningUv
            // 
            this.chbWarningUv.AutoSize = true;
            this.chbWarningUv.Location = new System.Drawing.Point(6, 42);
            this.chbWarningUv.Name = "chbWarningUv";
            this.chbWarningUv.Size = new System.Drawing.Size(41, 17);
            this.chbWarningUv.TabIndex = 1;
            this.chbWarningUv.Text = "UV";
            this.chbWarningUv.UseVisualStyleBackColor = true;
            this.chbWarningUv.CheckedChanged += new System.EventHandler(this.chbWarningUv_CheckedChanged);
            // 
            // chbWarningOv
            // 
            this.chbWarningOv.AutoSize = true;
            this.chbWarningOv.Location = new System.Drawing.Point(6, 19);
            this.chbWarningOv.Name = "chbWarningOv";
            this.chbWarningOv.Size = new System.Drawing.Size(41, 17);
            this.chbWarningOv.TabIndex = 0;
            this.chbWarningOv.Text = "OV";
            this.chbWarningOv.UseVisualStyleBackColor = true;
            this.chbWarningOv.CheckedChanged += new System.EventHandler(this.chbWarningOv_CheckedChanged);
            // 
            // lblPgoodDesc
            // 
            this.lblPgoodDesc.AutoSize = true;
            this.lblPgoodDesc.Location = new System.Drawing.Point(320, 171);
            this.lblPgoodDesc.Name = "lblPgoodDesc";
            this.lblPgoodDesc.Size = new System.Drawing.Size(73, 13);
            this.lblPgoodDesc.TabIndex = 14;
            this.lblPgoodDesc.Text = "lblPgoodDesc";
            // 
            // cbDiffCurrentRange
            // 
            this.cbDiffCurrentRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiffCurrentRange.FormattingEnabled = true;
            this.cbDiffCurrentRange.Location = new System.Drawing.Point(201, 141);
            this.cbDiffCurrentRange.Name = "cbDiffCurrentRange";
            this.cbDiffCurrentRange.Size = new System.Drawing.Size(113, 21);
            this.cbDiffCurrentRange.TabIndex = 11;
            this.cbDiffCurrentRange.SelectedIndexChanged += new System.EventHandler(this.cbDiffCurrentRange_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Current sensing ADC range:";
            // 
            // chbExposeCalibration
            // 
            this.chbExposeCalibration.AutoSize = true;
            this.chbExposeCalibration.Location = new System.Drawing.Point(323, 144);
            this.chbExposeCalibration.Name = "chbExposeCalibration";
            this.chbExposeCalibration.Size = new System.Drawing.Size(112, 17);
            this.chbExposeCalibration.TabIndex = 12;
            this.chbExposeCalibration.Text = "Expose calibration";
            this.chbExposeCalibration.UseVisualStyleBackColor = true;
            this.chbExposeCalibration.CheckedChanged += new System.EventHandler(this.chbExposeCalibration_CheckedChanged);
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(567, 426);
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
    }
}
