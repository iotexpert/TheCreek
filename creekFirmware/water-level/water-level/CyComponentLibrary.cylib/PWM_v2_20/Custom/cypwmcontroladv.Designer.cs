namespace PWM_v2_20
{
    partial class CyPWMControlAdv
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
            this.m_lblCaptureMode = new System.Windows.Forms.Label();
            this.m_cbCaptureMode = new System.Windows.Forms.ComboBox();
            this.m_lblKillMode = new System.Windows.Forms.Label();
            this.m_cbKillMode = new System.Windows.Forms.ComboBox();
            this.m_cbRunMode = new System.Windows.Forms.ComboBox();
            this.m_lblTrigger = new System.Windows.Forms.Label();
            this.m_cbTriggerMode = new System.Windows.Forms.ComboBox();
            this.m_lblRunMode = new System.Windows.Forms.Label();
            this.m_lblEnableMode = new System.Windows.Forms.Label();
            this.m_cbEnableMode = new System.Windows.Forms.ComboBox();
            this.m_chbxNone = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_chbxIntOnKill = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnCmp2 = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnCmp1 = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnTC = new System.Windows.Forms.CheckBox();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_numMinKillTime = new PWM_v2_20.CyNumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numMinKillTime)).BeginInit();
            this.SuspendLayout();
            // 
            // m_lblCaptureMode
            // 
            this.m_lblCaptureMode.AutoSize = true;
            this.m_lblCaptureMode.Location = new System.Drawing.Point(19, 116);
            this.m_lblCaptureMode.Name = "m_lblCaptureMode";
            this.m_lblCaptureMode.Size = new System.Drawing.Size(77, 13);
            this.m_lblCaptureMode.TabIndex = 54;
            this.m_lblCaptureMode.Text = "Capture Mode:";
            this.m_lblCaptureMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbCaptureMode
            // 
            this.m_cbCaptureMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbCaptureMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbCaptureMode.FormattingEnabled = true;
            this.m_cbCaptureMode.Location = new System.Drawing.Point(117, 112);
            this.m_cbCaptureMode.Name = "m_cbCaptureMode";
            this.m_cbCaptureMode.Size = new System.Drawing.Size(301, 21);
            this.m_cbCaptureMode.TabIndex = 7;
            this.m_cbCaptureMode.SelectedIndexChanged += new System.EventHandler(this.m_cbCaptureMode_SelectedIndexChanged);
            // 
            // m_lblKillMode
            // 
            this.m_lblKillMode.AutoSize = true;
            this.m_lblKillMode.Location = new System.Drawing.Point(43, 88);
            this.m_lblKillMode.Name = "m_lblKillMode";
            this.m_lblKillMode.Size = new System.Drawing.Size(53, 13);
            this.m_lblKillMode.TabIndex = 51;
            this.m_lblKillMode.Text = "Kill Mode:";
            this.m_lblKillMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbKillMode
            // 
            this.m_cbKillMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbKillMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbKillMode.FormattingEnabled = true;
            this.m_cbKillMode.Location = new System.Drawing.Point(117, 84);
            this.m_cbKillMode.Name = "m_cbKillMode";
            this.m_cbKillMode.Size = new System.Drawing.Size(216, 21);
            this.m_cbKillMode.TabIndex = 5;
            this.m_cbKillMode.SelectedIndexChanged += new System.EventHandler(this.m_cbKillMode_SelectedIndexChanged);
            // 
            // m_cbRunMode
            // 
            this.m_cbRunMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbRunMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbRunMode.FormattingEnabled = true;
            this.m_cbRunMode.Location = new System.Drawing.Point(117, 30);
            this.m_cbRunMode.Name = "m_cbRunMode";
            this.m_cbRunMode.Size = new System.Drawing.Size(301, 21);
            this.m_cbRunMode.TabIndex = 3;
            this.m_cbRunMode.SelectedIndexChanged += new System.EventHandler(this.m_cbRunMode_SelectedIndexChanged);
            // 
            // m_lblTrigger
            // 
            this.m_lblTrigger.AutoSize = true;
            this.m_lblTrigger.Location = new System.Drawing.Point(23, 61);
            this.m_lblTrigger.Name = "m_lblTrigger";
            this.m_lblTrigger.Size = new System.Drawing.Size(73, 13);
            this.m_lblTrigger.TabIndex = 48;
            this.m_lblTrigger.Text = "Trigger Mode:";
            this.m_lblTrigger.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbTriggerMode
            // 
            this.m_cbTriggerMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbTriggerMode.FormattingEnabled = true;
            this.m_cbTriggerMode.Location = new System.Drawing.Point(117, 57);
            this.m_cbTriggerMode.Name = "m_cbTriggerMode";
            this.m_cbTriggerMode.Size = new System.Drawing.Size(301, 21);
            this.m_cbTriggerMode.TabIndex = 4;
            this.m_cbTriggerMode.SelectedIndexChanged += new System.EventHandler(this.m_cbTriggerMode_SelectedIndexChanged);
            // 
            // m_lblRunMode
            // 
            this.m_lblRunMode.AutoSize = true;
            this.m_lblRunMode.Location = new System.Drawing.Point(36, 34);
            this.m_lblRunMode.Name = "m_lblRunMode";
            this.m_lblRunMode.Size = new System.Drawing.Size(60, 13);
            this.m_lblRunMode.TabIndex = 46;
            this.m_lblRunMode.Text = "Run Mode:";
            this.m_lblRunMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_lblEnableMode
            // 
            this.m_lblEnableMode.AutoSize = true;
            this.m_lblEnableMode.Location = new System.Drawing.Point(23, 7);
            this.m_lblEnableMode.Name = "m_lblEnableMode";
            this.m_lblEnableMode.Size = new System.Drawing.Size(73, 13);
            this.m_lblEnableMode.TabIndex = 45;
            this.m_lblEnableMode.Text = "Enable Mode:";
            this.m_lblEnableMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbEnableMode
            // 
            this.m_cbEnableMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbEnableMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbEnableMode.FormattingEnabled = true;
            this.m_cbEnableMode.Location = new System.Drawing.Point(117, 3);
            this.m_cbEnableMode.Name = "m_cbEnableMode";
            this.m_cbEnableMode.Size = new System.Drawing.Size(301, 21);
            this.m_cbEnableMode.TabIndex = 2;
            this.m_cbEnableMode.SelectedIndexChanged += new System.EventHandler(this.m_cbEnableMode_SelectedIndexChanged);
            // 
            // m_chbxNone
            // 
            this.m_chbxNone.AutoSize = true;
            this.m_chbxNone.Location = new System.Drawing.Point(8, 16);
            this.m_chbxNone.Name = "m_chbxNone";
            this.m_chbxNone.Size = new System.Drawing.Size(52, 17);
            this.m_chbxNone.TabIndex = 8;
            this.m_chbxNone.Text = "None";
            this.m_chbxNone.UseVisualStyleBackColor = true;
            this.m_chbxNone.CheckedChanged += new System.EventHandler(this.m_chbxNone_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.m_chbxIntOnKill);
            this.groupBox1.Controls.Add(this.m_chbxIntOnCmp2);
            this.groupBox1.Controls.Add(this.m_chbxIntOnCmp1);
            this.groupBox1.Controls.Add(this.m_chbxIntOnTC);
            this.groupBox1.Controls.Add(this.m_chbxNone);
            this.groupBox1.Location = new System.Drawing.Point(117, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 91);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interrupts:";
            // 
            // m_chbxIntOnKill
            // 
            this.m_chbxIntOnKill.AutoSize = true;
            this.m_chbxIntOnKill.Location = new System.Drawing.Point(85, 71);
            this.m_chbxIntOnKill.Name = "m_chbxIntOnKill";
            this.m_chbxIntOnKill.Size = new System.Drawing.Size(129, 17);
            this.m_chbxIntOnKill.TabIndex = 12;
            this.m_chbxIntOnKill.Text = "Interrupt On Kill Event";
            this.m_chbxIntOnKill.UseVisualStyleBackColor = true;
            this.m_chbxIntOnKill.CheckedChanged += new System.EventHandler(this.m_chbxIntOnKill_CheckedChanged);
            // 
            // m_chbxIntOnCmp2
            // 
            this.m_chbxIntOnCmp2.AutoSize = true;
            this.m_chbxIntOnCmp2.Location = new System.Drawing.Point(85, 53);
            this.m_chbxIntOnCmp2.Name = "m_chbxIntOnCmp2";
            this.m_chbxIntOnCmp2.Size = new System.Drawing.Size(167, 17);
            this.m_chbxIntOnCmp2.TabIndex = 11;
            this.m_chbxIntOnCmp2.Text = "Interrupt On Compare 2 Event";
            this.m_chbxIntOnCmp2.UseVisualStyleBackColor = true;
            this.m_chbxIntOnCmp2.CheckedChanged += new System.EventHandler(this.m_chbxIntOnCmp2_CheckedChanged);
            // 
            // m_chbxIntOnCmp1
            // 
            this.m_chbxIntOnCmp1.AutoSize = true;
            this.m_chbxIntOnCmp1.Location = new System.Drawing.Point(85, 35);
            this.m_chbxIntOnCmp1.Name = "m_chbxIntOnCmp1";
            this.m_chbxIntOnCmp1.Size = new System.Drawing.Size(167, 17);
            this.m_chbxIntOnCmp1.TabIndex = 10;
            this.m_chbxIntOnCmp1.Text = "Interrupt On Compare 1 Event";
            this.m_chbxIntOnCmp1.UseVisualStyleBackColor = true;
            this.m_chbxIntOnCmp1.CheckedChanged += new System.EventHandler(this.m_chbxIntOnCmp1_CheckedChanged);
            // 
            // m_chbxIntOnTC
            // 
            this.m_chbxIntOnTC.AutoSize = true;
            this.m_chbxIntOnTC.Location = new System.Drawing.Point(85, 17);
            this.m_chbxIntOnTC.Name = "m_chbxIntOnTC";
            this.m_chbxIntOnTC.Size = new System.Drawing.Size(187, 17);
            this.m_chbxIntOnTC.TabIndex = 9;
            this.m_chbxIntOnTC.Text = "Interrupt On Terminal Count Event";
            this.m_chbxIntOnTC.UseVisualStyleBackColor = true;
            this.m_chbxIntOnTC.CheckedChanged += new System.EventHandler(this.m_chbxIntOnTC_CheckedChanged);
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // m_numMinKillTime
            // 
            this.m_numMinKillTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numMinKillTime.Location = new System.Drawing.Point(339, 85);
            this.m_numMinKillTime.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numMinKillTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numMinKillTime.Name = "m_numMinKillTime";
            this.m_numMinKillTime.Size = new System.Drawing.Size(79, 20);
            this.m_numMinKillTime.TabIndex = 6;
            this.m_numMinKillTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numMinKillTime.ValueChanged += new System.EventHandler(this.m_numMinKillTime_ValueChanged);
            this.m_numMinKillTime.Validating += new System.ComponentModel.CancelEventHandler(this.m_numMinKillTime_Validating);
            this.m_numMinKillTime.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_numMinKillTime_KeyUp);
            // 
            // CyPWMControlAdv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_lblCaptureMode);
            this.Controls.Add(this.m_cbCaptureMode);
            this.Controls.Add(this.m_numMinKillTime);
            this.Controls.Add(this.m_lblKillMode);
            this.Controls.Add(this.m_cbKillMode);
            this.Controls.Add(this.m_cbRunMode);
            this.Controls.Add(this.m_lblTrigger);
            this.Controls.Add(this.m_cbTriggerMode);
            this.Controls.Add(this.m_lblRunMode);
            this.Controls.Add(this.m_lblEnableMode);
            this.Controls.Add(this.m_cbEnableMode);
            this.Name = "CyPWMControlAdv";
            this.Size = new System.Drawing.Size(439, 241);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numMinKillTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblCaptureMode;
        public System.Windows.Forms.ComboBox m_cbCaptureMode;
        private CyNumericUpDown m_numMinKillTime;
        private System.Windows.Forms.Label m_lblKillMode;
        public System.Windows.Forms.ComboBox m_cbKillMode;
        public System.Windows.Forms.ComboBox m_cbRunMode;
        private System.Windows.Forms.Label m_lblTrigger;
        public System.Windows.Forms.ComboBox m_cbTriggerMode;
        private System.Windows.Forms.Label m_lblRunMode;
        private System.Windows.Forms.Label m_lblEnableMode;
        public System.Windows.Forms.ComboBox m_cbEnableMode;
        private System.Windows.Forms.CheckBox m_chbxNone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox m_chbxIntOnCmp2;
        private System.Windows.Forms.CheckBox m_chbxIntOnCmp1;
        private System.Windows.Forms.CheckBox m_chbxIntOnTC;
        private System.Windows.Forms.CheckBox m_chbxIntOnKill;
        private System.Windows.Forms.ErrorProvider ep_Errors;
    }
}
