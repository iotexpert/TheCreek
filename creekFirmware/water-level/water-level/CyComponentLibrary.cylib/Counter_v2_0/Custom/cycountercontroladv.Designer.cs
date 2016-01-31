namespace Counter_v2_0
{
    partial class CyCounterControlAdv
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
        this.m_chbxIntSrcCompare = new System.Windows.Forms.CheckBox();
        this.m_chbxReloadOnReset = new System.Windows.Forms.CheckBox();
        this.m_chbxReloadOnTC = new System.Windows.Forms.CheckBox();
        this.m_chbxReloadOnCapture = new System.Windows.Forms.CheckBox();
        this.m_chbxReloadOnCompare = new System.Windows.Forms.CheckBox();
        this.m_lblReloadOn = new System.Windows.Forms.Label();
        this.m_lblEnableMode = new System.Windows.Forms.Label();
        this.m_cbEnableMode = new System.Windows.Forms.ComboBox();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.m_chbxIntSrcTC = new System.Windows.Forms.CheckBox();
        this.m_chbxIntSrcCapture = new System.Windows.Forms.CheckBox();
        this.m_lblInterruptSources = new System.Windows.Forms.Label();
        this.m_lblCaptureMode = new System.Windows.Forms.Label();
        this.m_cbCaptureMode = new System.Windows.Forms.ComboBox();
        this.m_lblRunMode = new System.Windows.Forms.Label();
        this.m_cbRunMode = new System.Windows.Forms.ComboBox();
        this.ep_ErrorAdv = new System.Windows.Forms.ErrorProvider(this.components);
        ((System.ComponentModel.ISupportInitialize)(this.ep_ErrorAdv)).BeginInit();
        this.SuspendLayout();
        // 
        // m_chbxIntSrcCompare
        // 
        this.m_chbxIntSrcCompare.AutoSize = true;
        this.m_chbxIntSrcCompare.Location = new System.Drawing.Point(118, 155);
        this.m_chbxIntSrcCompare.Name = "m_chbxIntSrcCompare";
        this.m_chbxIntSrcCompare.Size = new System.Drawing.Size(85, 17);
        this.m_chbxIntSrcCompare.TabIndex = 187;
        this.m_chbxIntSrcCompare.Text = "On Compare";
        this.m_chbxIntSrcCompare.UseVisualStyleBackColor = true;
        this.m_chbxIntSrcCompare.VisibleChanged += new System.EventHandler(this.m_chbxIntSrcCompare_Changed);
        this.m_chbxIntSrcCompare.CheckedChanged += new System.EventHandler(this.m_chbxIntSrcCompare_Changed);
        // 
        // m_chbxReloadOnReset
        // 
        this.m_chbxReloadOnReset.AutoSize = true;
        this.m_chbxReloadOnReset.Location = new System.Drawing.Point(118, 113);
        this.m_chbxReloadOnReset.Name = "m_chbxReloadOnReset";
        this.m_chbxReloadOnReset.Size = new System.Drawing.Size(71, 17);
        this.m_chbxReloadOnReset.TabIndex = 185;
        this.m_chbxReloadOnReset.Text = "On Reset";
        this.m_chbxReloadOnReset.UseVisualStyleBackColor = true;
        this.m_chbxReloadOnReset.CheckedChanged += new System.EventHandler(this.m_chbxReloadOnReset_Changed);
        // 
        // m_chbxReloadOnTC
        // 
        this.m_chbxReloadOnTC.AutoSize = true;
        this.m_chbxReloadOnTC.Location = new System.Drawing.Point(244, 113);
        this.m_chbxReloadOnTC.Name = "m_chbxReloadOnTC";
        this.m_chbxReloadOnTC.Size = new System.Drawing.Size(57, 17);
        this.m_chbxReloadOnTC.TabIndex = 184;
        this.m_chbxReloadOnTC.Text = "On TC";
        this.m_chbxReloadOnTC.UseVisualStyleBackColor = true;
        this.m_chbxReloadOnTC.CheckedChanged += new System.EventHandler(this.m_chbxReloadOnTC_Changed);
        // 
        // m_chbxReloadOnCapture
        // 
        this.m_chbxReloadOnCapture.AutoSize = true;
        this.m_chbxReloadOnCapture.Location = new System.Drawing.Point(118, 93);
        this.m_chbxReloadOnCapture.Name = "m_chbxReloadOnCapture";
        this.m_chbxReloadOnCapture.Size = new System.Drawing.Size(80, 17);
        this.m_chbxReloadOnCapture.TabIndex = 183;
        this.m_chbxReloadOnCapture.Text = "On Capture";
        this.m_chbxReloadOnCapture.UseVisualStyleBackColor = true;
        this.m_chbxReloadOnCapture.VisibleChanged += new System.EventHandler(this.m_chbxReloadOnCapture_Changed);
        this.m_chbxReloadOnCapture.CheckedChanged += new System.EventHandler(this.m_chbxReloadOnCapture_Changed);
        // 
        // m_chbxReloadOnCompare
        // 
        this.m_chbxReloadOnCompare.AutoSize = true;
        this.m_chbxReloadOnCompare.Location = new System.Drawing.Point(244, 93);
        this.m_chbxReloadOnCompare.Name = "m_chbxReloadOnCompare";
        this.m_chbxReloadOnCompare.Size = new System.Drawing.Size(85, 17);
        this.m_chbxReloadOnCompare.TabIndex = 182;
        this.m_chbxReloadOnCompare.Text = "On Compare";
        this.m_chbxReloadOnCompare.UseVisualStyleBackColor = true;
        this.m_chbxReloadOnCompare.VisibleChanged += new System.EventHandler(this.m_chbxReloadOnCompare_Changed);
        this.m_chbxReloadOnCompare.CheckedChanged += new System.EventHandler(this.m_chbxReloadOnCompare_Changed);
        // 
        // m_lblReloadOn
        // 
        this.m_lblReloadOn.AutoSize = true;
        this.m_lblReloadOn.Location = new System.Drawing.Point(12, 95);
        this.m_lblReloadOn.Name = "m_lblReloadOn";
        this.m_lblReloadOn.Size = new System.Drawing.Size(84, 13);
        this.m_lblReloadOn.TabIndex = 181;
        this.m_lblReloadOn.Text = "Reload Counter:";
        this.m_lblReloadOn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // m_lblEnableMode
        // 
        this.m_lblEnableMode.AutoSize = true;
        this.m_lblEnableMode.Location = new System.Drawing.Point(23, 40);
        this.m_lblEnableMode.Name = "m_lblEnableMode";
        this.m_lblEnableMode.Size = new System.Drawing.Size(73, 13);
        this.m_lblEnableMode.TabIndex = 180;
        this.m_lblEnableMode.Text = "Enable Mode:";
        this.m_lblEnableMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // m_cbEnableMode
        // 
        this.m_cbEnableMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.m_cbEnableMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.m_cbEnableMode.FormattingEnabled = true;
        this.m_cbEnableMode.Location = new System.Drawing.Point(118, 36);
        this.m_cbEnableMode.Name = "m_cbEnableMode";
        this.m_cbEnableMode.Size = new System.Drawing.Size(257, 21);
        this.m_cbEnableMode.TabIndex = 179;
        this.m_cbEnableMode.SelectedIndexChanged += new System.EventHandler(this.m_cbEnableMode_SelectedIndexChanged);
        // 
        // groupBox2
        // 
        this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | 
		                        System.Windows.Forms.AnchorStyles.Left)| System.Windows.Forms.AnchorStyles.Right)));
        this.groupBox2.Location = new System.Drawing.Point(11, 90);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(365, 2);
        this.groupBox2.TabIndex = 178;
        this.groupBox2.TabStop = false;
        // 
        // groupBox1
        // 
        this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | 
		                          System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.groupBox1.Location = new System.Drawing.Point(10, 130);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(365, 2);
        this.groupBox1.TabIndex = 177;
        this.groupBox1.TabStop = false;
        // 
        // m_chbxIntSrcTC
        // 
        this.m_chbxIntSrcTC.AutoSize = true;
        this.m_chbxIntSrcTC.Location = new System.Drawing.Point(118, 136);
        this.m_chbxIntSrcTC.Name = "m_chbxIntSrcTC";
        this.m_chbxIntSrcTC.Size = new System.Drawing.Size(57, 17);
        this.m_chbxIntSrcTC.TabIndex = 176;
        this.m_chbxIntSrcTC.Text = "On TC";
        this.m_chbxIntSrcTC.UseVisualStyleBackColor = true;
        this.m_chbxIntSrcTC.VisibleChanged += new System.EventHandler(this.m_chbxIntSrcTC_Changed);
        this.m_chbxIntSrcTC.CheckedChanged += new System.EventHandler(this.m_chbxIntSrcTC_Changed);
        // 
        // m_chbxIntSrcCapture
        // 
        this.m_chbxIntSrcCapture.AutoSize = true;
        this.m_chbxIntSrcCapture.Location = new System.Drawing.Point(244, 136);
        this.m_chbxIntSrcCapture.Name = "m_chbxIntSrcCapture";
        this.m_chbxIntSrcCapture.Size = new System.Drawing.Size(80, 17);
        this.m_chbxIntSrcCapture.TabIndex = 175;
        this.m_chbxIntSrcCapture.Text = "On Capture";
        this.m_chbxIntSrcCapture.UseVisualStyleBackColor = true;
        this.m_chbxIntSrcCapture.VisibleChanged += new System.EventHandler(this.m_chbxIntSrcCapture_Changed);
        this.m_chbxIntSrcCapture.CheckedChanged += new System.EventHandler(this.m_chbxIntSrcCapture_Changed);
        // 
        // m_lblInterruptSources
        // 
        this.m_lblInterruptSources.AutoSize = true;
        this.m_lblInterruptSources.Location = new System.Drawing.Point(47, 138);
        this.m_lblInterruptSources.Name = "m_lblInterruptSources";
        this.m_lblInterruptSources.Size = new System.Drawing.Size(49, 13);
        this.m_lblInterruptSources.TabIndex = 174;
        this.m_lblInterruptSources.Text = "Interrupt:";
        this.m_lblInterruptSources.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // m_lblCaptureMode
        // 
        this.m_lblCaptureMode.AutoSize = true;
        this.m_lblCaptureMode.Location = new System.Drawing.Point(19, 14);
        this.m_lblCaptureMode.Name = "m_lblCaptureMode";
        this.m_lblCaptureMode.Size = new System.Drawing.Size(77, 13);
        this.m_lblCaptureMode.TabIndex = 173;
        this.m_lblCaptureMode.Text = "Capture Mode:";
        this.m_lblCaptureMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // m_cbCaptureMode
        // 
        this.m_cbCaptureMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | 
		                               System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.m_cbCaptureMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.m_cbCaptureMode.FormattingEnabled = true;
        this.m_cbCaptureMode.Location = new System.Drawing.Point(118, 10);
        this.m_cbCaptureMode.Name = "m_cbCaptureMode";
        this.m_cbCaptureMode.Size = new System.Drawing.Size(257, 21);
        this.m_cbCaptureMode.TabIndex = 172;
        this.m_cbCaptureMode.SelectedIndexChanged += new System.EventHandler(this.m_cbCaptureMode_SelectedIndexChanged);
        // 
        // m_lblRunMode
        // 
        this.m_lblRunMode.AutoSize = true;
        this.m_lblRunMode.Location = new System.Drawing.Point(36, 66);
        this.m_lblRunMode.Name = "m_lblRunMode";
        this.m_lblRunMode.Size = new System.Drawing.Size(60, 13);
        this.m_lblRunMode.TabIndex = 189;
        this.m_lblRunMode.Text = "Run Mode:";
        this.m_lblRunMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // m_cbRunMode
        // 
        this.m_cbRunMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | 
		                          System.Windows.Forms.AnchorStyles.Left)| System.Windows.Forms.AnchorStyles.Right)));
        this.m_cbRunMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.m_cbRunMode.FormattingEnabled = true;
        this.m_cbRunMode.Location = new System.Drawing.Point(118, 62);
        this.m_cbRunMode.Name = "m_cbRunMode";
        this.m_cbRunMode.Size = new System.Drawing.Size(257, 21);
        this.m_cbRunMode.TabIndex = 188;
        this.m_cbRunMode.SelectedIndexChanged += new System.EventHandler(this.m_cbRunMode_SelectedIndexChanged);
        // 
        // ep_ErrorAdv
        // 
        this.ep_ErrorAdv.ContainerControl = this;
        // 
        // CyCounterControlAdv
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.m_lblRunMode);
        this.Controls.Add(this.m_cbRunMode);
        this.Controls.Add(this.m_chbxIntSrcCompare);
        this.Controls.Add(this.m_chbxReloadOnReset);
        this.Controls.Add(this.m_chbxReloadOnTC);
        this.Controls.Add(this.m_chbxReloadOnCapture);
        this.Controls.Add(this.m_chbxReloadOnCompare);
        this.Controls.Add(this.m_lblReloadOn);
        this.Controls.Add(this.m_lblEnableMode);
        this.Controls.Add(this.m_cbEnableMode);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.m_chbxIntSrcTC);
        this.Controls.Add(this.m_chbxIntSrcCapture);
        this.Controls.Add(this.m_lblInterruptSources);
        this.Controls.Add(this.m_lblCaptureMode);
        this.Controls.Add(this.m_cbCaptureMode);
        this.Name = "CyCounterControlAdv";
        this.Size = new System.Drawing.Size(393, 202);
        ((System.ComponentModel.ISupportInitialize)(this.ep_ErrorAdv)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_chbxIntSrcCompare;
        private System.Windows.Forms.CheckBox m_chbxReloadOnReset;
        private System.Windows.Forms.CheckBox m_chbxReloadOnTC;
        private System.Windows.Forms.CheckBox m_chbxReloadOnCapture;
        private System.Windows.Forms.CheckBox m_chbxReloadOnCompare;
        private System.Windows.Forms.Label m_lblReloadOn;
        private System.Windows.Forms.Label m_lblEnableMode;
        private System.Windows.Forms.ComboBox m_cbEnableMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox m_chbxIntSrcTC;
        private System.Windows.Forms.CheckBox m_chbxIntSrcCapture;
        private System.Windows.Forms.Label m_lblInterruptSources;
        private System.Windows.Forms.Label m_lblCaptureMode;
        private System.Windows.Forms.ComboBox m_cbCaptureMode;
        private System.Windows.Forms.Label m_lblRunMode;
        private System.Windows.Forms.ComboBox m_cbRunMode;
        private System.Windows.Forms.ErrorProvider ep_ErrorAdv;
    }
}
