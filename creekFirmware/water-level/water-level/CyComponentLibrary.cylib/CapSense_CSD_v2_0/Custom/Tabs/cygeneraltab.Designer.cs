namespace  CapSense_CSD_v2_0
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
            this.components = new System.ComponentModel.Container();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.cbEnableClkInput = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTuningMethod = new System.Windows.Forms.ComboBox();
            this.cbNumberChannels = new System.Windows.Forms.ComboBox();
            this.lScanClock = new System.Windows.Forms.Label();
            this.cbScanClock = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRawDataNoiseFilter = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbWaterProofing = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLoad,
            this.toolStripButtonSave});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(358, 25);
            this.toolStrip.TabIndex = 8;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripButtonLoad
            // 
            this.toolStripButtonLoad.Image = global::CapSense_CSD_v2_0.CyCsResource.openHS;
            this.toolStripButtonLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoad.Name = "toolStripButtonLoad";
            this.toolStripButtonLoad.Size = new System.Drawing.Size(92, 22);
            this.toolStripButtonLoad.Text = "Load Settings";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::CapSense_CSD_v2_0.CyCsResource.saveHS;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(93, 22);
            this.toolStripButtonSave.Text = "Save Settings";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // cbEnableClkInput
            // 
            this.cbEnableClkInput.AutoSize = true;
            this.cbEnableClkInput.Location = new System.Drawing.Point(14, 146);
            this.cbEnableClkInput.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbEnableClkInput.Name = "cbEnableClkInput";
            this.cbEnableClkInput.Size = new System.Drawing.Size(117, 17);
            this.cbEnableClkInput.TabIndex = 11;
            this.cbEnableClkInput.Text = "Enable clock input ";
            this.cbEnableClkInput.UseVisualStyleBackColor = true;
            this.cbEnableClkInput.CheckedChanged += new System.EventHandler(this.cbEnableClkInput_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Tuning method ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Number of channels";
            // 
            // cbTuningMethod
            // 
            this.cbTuningMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTuningMethod.FormattingEnabled = true;
            this.cbTuningMethod.Items.AddRange(new object[] {
            "Auto (SmartSense)",
            "Manual",
            "None"});
            this.cbTuningMethod.Location = new System.Drawing.Point(155, 19);
            this.cbTuningMethod.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbTuningMethod.Name = "cbTuningMethod";
            this.cbTuningMethod.Size = new System.Drawing.Size(158, 21);
            this.cbTuningMethod.TabIndex = 14;
            this.cbTuningMethod.Validating += new System.ComponentModel.CancelEventHandler(this.cbTuningMethod_Validating);
            this.cbTuningMethod.SelectedIndexChanged += new System.EventHandler(this.cbTuningMethod_SelectedIndexChanged);
            // 
            // cbNumberChannels
            // 
            this.cbNumberChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNumberChannels.FormattingEnabled = true;
            this.cbNumberChannels.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbNumberChannels.Location = new System.Drawing.Point(155, 50);
            this.cbNumberChannels.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbNumberChannels.Name = "cbNumberChannels";
            this.cbNumberChannels.Size = new System.Drawing.Size(158, 21);
            this.cbNumberChannels.TabIndex = 15;
            this.toolTip1.SetToolTip(this.cbNumberChannels, "As a general rule of thumb, 1-20 sensors is good fit for 1 channel, over 20 senso" +
                    "rs is a good fit for 2 channels.");
            // 
            // lScanClock
            // 
            this.lScanClock.AutoSize = true;
            this.lScanClock.Location = new System.Drawing.Point(11, 176);
            this.lScanClock.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lScanClock.Name = "lScanClock";
            this.lScanClock.Size = new System.Drawing.Size(65, 13);
            this.lScanClock.TabIndex = 16;
            this.lScanClock.Text = "Scan Clock ";
            // 
            // cbScanClock
            // 
            this.cbScanClock.FormattingEnabled = true;
            this.cbScanClock.Location = new System.Drawing.Point(155, 173);
            this.cbScanClock.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbScanClock.Name = "cbScanClock";
            this.cbScanClock.Size = new System.Drawing.Size(158, 21);
            this.cbScanClock.TabIndex = 17;
            this.cbScanClock.Text = "24";
            this.cbScanClock.Validating += new System.ComponentModel.CancelEventHandler(this.cbScanClock_Validating);
            this.cbScanClock.TextChanged += new System.EventHandler(this.cbScanClock_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbRawDataNoiseFilter);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbScanClock);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lScanClock);
            this.panel1.Controls.Add(this.cbTuningMethod);
            this.panel1.Controls.Add(this.cbEnableClkInput);
            this.panel1.Controls.Add(this.cbNumberChannels);
            this.panel1.Controls.Add(this.cbWaterProofing);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 297);
            this.panel1.TabIndex = 20;
            // 
            // cbRawDataNoiseFilter
            // 
            this.cbRawDataNoiseFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRawDataNoiseFilter.FormattingEnabled = true;
            this.cbRawDataNoiseFilter.Location = new System.Drawing.Point(155, 83);
            this.cbRawDataNoiseFilter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbRawDataNoiseFilter.Name = "cbRawDataNoiseFilter";
            this.cbRawDataNoiseFilter.Size = new System.Drawing.Size(158, 21);
            this.cbRawDataNoiseFilter.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 85);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Raw Data Noise Filter ";
            // 
            // cbWaterProofing
            // 
            this.cbWaterProofing.AutoSize = true;
            this.cbWaterProofing.Location = new System.Drawing.Point(14, 115);
            this.cbWaterProofing.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbWaterProofing.Name = "cbWaterProofing";
            this.cbWaterProofing.Size = new System.Drawing.Size(167, 17);
            this.cbWaterProofing.TabIndex = 19;
            this.cbWaterProofing.Text = "Water proofing and detection ";
            this.toolTip1.SetToolTip(this.cbWaterProofing, "Water proofing enables the shield output terminal, adds a guard sensor and adds w" +
                    "aterproofing firmware which includes a water detect feature. ");
            this.cbWaterProofing.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(358, 322);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoad;
        private System.Windows.Forms.CheckBox cbEnableClkInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTuningMethod;
        private System.Windows.Forms.ComboBox cbNumberChannels;
        private System.Windows.Forms.Label lScanClock;
        private System.Windows.Forms.ComboBox cbScanClock;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbWaterProofing;
        private System.Windows.Forms.ComboBox cbRawDataNoiseFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
