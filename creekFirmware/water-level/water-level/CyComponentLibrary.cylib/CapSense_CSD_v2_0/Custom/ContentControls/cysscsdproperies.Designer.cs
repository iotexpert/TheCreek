namespace  CapSense_CSD_v2_0
{
    partial class CySSCSDProperties
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
            this.cbIDACSetting = new System.Windows.Forms.TextBox();
            this.lIdadS = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbSensitivity = new System.Windows.Forms.ComboBox();
            this.lSensitivity = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cbIDACSetting
            // 
            this.cbIDACSetting.Location = new System.Drawing.Point(97, 10);
            this.cbIDACSetting.Margin = new System.Windows.Forms.Padding(4);
            this.cbIDACSetting.Name = "cbIDACSetting";
            this.cbIDACSetting.Size = new System.Drawing.Size(55, 20);
            this.cbIDACSetting.TabIndex = 12;
            this.cbIDACSetting.Text = "20";
            this.toolTip.SetToolTip(this.cbIDACSetting, " Configures the IDAC output value. The IDAC current = Value*(IDAC range/255).");
            this.cbIDACSetting.TextChanged += new System.EventHandler(this.tbIDACSetting_TextChanged);
            this.cbIDACSetting.Validating += new System.ComponentModel.CancelEventHandler(this.tbIDACSetting_Validating);
            // 
            // lIdadS
            // 
            this.lIdadS.AutoSize = true;
            this.lIdadS.Location = new System.Drawing.Point(6, 13);
            this.lIdadS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdadS.Name = "lIdadS";
            this.lIdadS.Size = new System.Drawing.Size(62, 13);
            this.lIdadS.TabIndex = 26;
            this.lIdadS.Text = "IDAC Value";
            this.toolTip.SetToolTip(this.lIdadS, " Configures the IDAC output value. The IDAC current = Value*(IDAC range/255).");
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cbSensitivity);
            this.groupBox2.Controls.Add(this.lSensitivity);
            this.groupBox2.Controls.Add(this.lIdadS);
            this.groupBox2.Controls.Add(this.cbIDACSetting);
            this.groupBox2.Location = new System.Drawing.Point(0, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(368, 34);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            // 
            // cbSensitivity
            // 
            this.cbSensitivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSensitivity.FormattingEnabled = true;
            this.cbSensitivity.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cbSensitivity.Location = new System.Drawing.Point(274, 10);
            this.cbSensitivity.Margin = new System.Windows.Forms.Padding(2);
            this.cbSensitivity.Name = "cbSensitivity";
            this.cbSensitivity.Size = new System.Drawing.Size(78, 21);
            this.cbSensitivity.TabIndex = 28;
            // 
            // lSensitivity
            // 
            this.lSensitivity.AutoSize = true;
            this.lSensitivity.Location = new System.Drawing.Point(214, 13);
            this.lSensitivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lSensitivity.Name = "lSensitivity";
            this.lSensitivity.Size = new System.Drawing.Size(54, 13);
            this.lSensitivity.TabIndex = 26;
            this.lSensitivity.Text = "Sensitivity";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CySSCSDProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CySSCSDProperties";
            this.Size = new System.Drawing.Size(370, 43);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox cbIDACSetting;
        private System.Windows.Forms.Label lIdadS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox cbSensitivity;
        private System.Windows.Forms.Label lSensitivity;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
