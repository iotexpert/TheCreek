namespace EZI2C_v1_60
{
    partial class CyEZI2CBasic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyEZI2CBasic));
            this.m_groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_cbPort = new System.Windows.Forms.ComboBox();
            this.m_label9 = new System.Windows.Forms.Label();
            this.m_label8 = new System.Windows.Forms.Label();
            this.m_label7 = new System.Windows.Forms.Label();
            this.m_checkBoxWakeup = new System.Windows.Forms.CheckBox();
            this.m_tbSecondarySlaveAddress = new System.Windows.Forms.TextBox();
            this.m_tbPrimarySlaveAddress = new System.Windows.Forms.TextBox();
            this.m_cbSubAddressSize = new System.Windows.Forms.ComboBox();
            this.m_cbNumberOfAddresses = new System.Windows.Forms.ComboBox();
            this.m_cbBusSpeed = new System.Windows.Forms.ComboBox();
            this.m_label6 = new System.Windows.Forms.Label();
            this.m_label5 = new System.Windows.Forms.Label();
            this.m_label4 = new System.Windows.Forms.Label();
            this.m_label3 = new System.Windows.Forms.Label();
            this.m_label2 = new System.Windows.Forms.Label();
            this.m_groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_label1 = new System.Windows.Forms.Label();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.m_groupBox1.SuspendLayout();
            this.m_groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // m_groupBox1
            // 
            this.m_groupBox1.Controls.Add(this.m_cbPort);
            this.m_groupBox1.Controls.Add(this.m_label9);
            this.m_groupBox1.Controls.Add(this.m_label8);
            this.m_groupBox1.Controls.Add(this.m_label7);
            this.m_groupBox1.Controls.Add(this.m_checkBoxWakeup);
            this.m_groupBox1.Controls.Add(this.m_tbSecondarySlaveAddress);
            this.m_groupBox1.Controls.Add(this.m_tbPrimarySlaveAddress);
            this.m_groupBox1.Controls.Add(this.m_cbSubAddressSize);
            this.m_groupBox1.Controls.Add(this.m_cbNumberOfAddresses);
            this.m_groupBox1.Controls.Add(this.m_cbBusSpeed);
            this.m_groupBox1.Controls.Add(this.m_label6);
            this.m_groupBox1.Controls.Add(this.m_label5);
            this.m_groupBox1.Controls.Add(this.m_label4);
            this.m_groupBox1.Controls.Add(this.m_label3);
            this.m_groupBox1.Controls.Add(this.m_label2);
            this.m_groupBox1.Location = new System.Drawing.Point(0, 0);
            this.m_groupBox1.Name = "m_groupBox1";
            this.m_groupBox1.Size = new System.Drawing.Size(223, 237);
            this.m_groupBox1.TabIndex = 0;
            this.m_groupBox1.TabStop = false;
            // 
            // m_cbPort
            // 
            this.m_cbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPort.FormattingEnabled = true;
            this.m_cbPort.Location = new System.Drawing.Point(136, 180);
            this.m_cbPort.Name = "m_cbPort";
            this.m_cbPort.Size = new System.Drawing.Size(64, 21);
            this.m_cbPort.TabIndex = 5;
            this.m_cbPort.SelectedIndexChanged += new System.EventHandler(this.m_cbPort_SelectedIndexChanged);
            // 
            // m_label9
            // 
            this.m_label9.AutoSize = true;
            this.m_label9.Location = new System.Drawing.Point(16, 183);
            this.m_label9.Name = "m_label9";
            this.m_label9.Size = new System.Drawing.Size(86, 13);
            this.m_label9.TabIndex = 13;
            this.m_label9.Text = "Pin connections:";
            // 
            // m_label8
            // 
            this.m_label8.AutoSize = true;
            this.m_label8.Location = new System.Drawing.Point(202, 150);
            this.m_label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label8.Name = "m_label8";
            this.m_label8.Size = new System.Drawing.Size(18, 13);
            this.m_label8.TabIndex = 12;
            this.m_label8.Text = "bit";
            // 
            // m_label7
            // 
            this.m_label7.AutoSize = true;
            this.m_label7.Location = new System.Drawing.Point(159, 24);
            this.m_label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label7.Name = "m_label7";
            this.m_label7.Size = new System.Drawing.Size(30, 13);
            this.m_label7.TabIndex = 11;
            this.m_label7.Text = "kbps";
            // 
            // m_checkBoxWakeup
            // 
            this.m_checkBoxWakeup.AutoSize = true;
            this.m_checkBoxWakeup.Location = new System.Drawing.Point(19, 214);
            this.m_checkBoxWakeup.Margin = new System.Windows.Forms.Padding(2);
            this.m_checkBoxWakeup.Name = "m_checkBoxWakeup";
            this.m_checkBoxWakeup.Size = new System.Drawing.Size(183, 17);
            this.m_checkBoxWakeup.TabIndex = 6;
            this.m_checkBoxWakeup.Text = "Enable wakeup from Sleep Mode";
            this.m_checkBoxWakeup.UseVisualStyleBackColor = true;
            this.m_checkBoxWakeup.CheckedChanged += new System.EventHandler(this.m_checkBoxWakeup_CheckedChanged);
            // 
            // m_tbSecondarySlaveAddress
            // 
            this.m_errorProvider.SetIconAlignment(this.m_tbSecondarySlaveAddress, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.m_tbSecondarySlaveAddress.Location = new System.Drawing.Point(156, 116);
            this.m_tbSecondarySlaveAddress.Margin = new System.Windows.Forms.Padding(2);
            this.m_tbSecondarySlaveAddress.MaxLength = 10;
            this.m_tbSecondarySlaveAddress.Name = "m_tbSecondarySlaveAddress";
            this.m_tbSecondarySlaveAddress.Size = new System.Drawing.Size(44, 20);
            this.m_tbSecondarySlaveAddress.TabIndex = 3;
            this.m_toolTip1.SetToolTip(this.m_tbSecondarySlaveAddress, "Use \'0x\' prefix for hexadecimals");
            this.m_tbSecondarySlaveAddress.TextChanged += new System.EventHandler(this.m_tbSecondarySlaveAddress_TextChanged);
            // 
            // m_tbPrimarySlaveAddress
            // 
            this.m_errorProvider.SetIconAlignment(this.m_tbPrimarySlaveAddress, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.m_tbPrimarySlaveAddress.Location = new System.Drawing.Point(156, 84);
            this.m_tbPrimarySlaveAddress.Margin = new System.Windows.Forms.Padding(2);
            this.m_tbPrimarySlaveAddress.MaxLength = 10;
            this.m_tbPrimarySlaveAddress.Name = "m_tbPrimarySlaveAddress";
            this.m_tbPrimarySlaveAddress.Size = new System.Drawing.Size(44, 20);
            this.m_tbPrimarySlaveAddress.TabIndex = 2;
            this.m_toolTip1.SetToolTip(this.m_tbPrimarySlaveAddress, "Use \'0x\' prefix for hexadecimals");
            this.m_tbPrimarySlaveAddress.TextChanged += new System.EventHandler(this.m_tbPrimarySlaveAddress_TextChanged);
            // 
            // m_cbSubAddressSize
            // 
            this.m_cbSubAddressSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbSubAddressSize.FormattingEnabled = true;
            this.m_cbSubAddressSize.Items.AddRange(new object[] {
            "8",
            "16"});
            this.m_cbSubAddressSize.Location = new System.Drawing.Point(156, 148);
            this.m_cbSubAddressSize.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbSubAddressSize.Name = "m_cbSubAddressSize";
            this.m_cbSubAddressSize.Size = new System.Drawing.Size(44, 21);
            this.m_cbSubAddressSize.TabIndex = 4;
            this.m_cbSubAddressSize.SelectedIndexChanged += new System.EventHandler(this.m_cbSubAddressSize_SelectedIndexChanged);
            // 
            // m_cbNumberOfAddresses
            // 
            this.m_cbNumberOfAddresses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbNumberOfAddresses.FormattingEnabled = true;
            this.m_cbNumberOfAddresses.Items.AddRange(new object[] {
            "1",
            "2"});
            this.m_cbNumberOfAddresses.Location = new System.Drawing.Point(156, 53);
            this.m_cbNumberOfAddresses.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbNumberOfAddresses.Name = "m_cbNumberOfAddresses";
            this.m_cbNumberOfAddresses.Size = new System.Drawing.Size(44, 21);
            this.m_cbNumberOfAddresses.TabIndex = 1;
            this.m_cbNumberOfAddresses.SelectedIndexChanged += new System.EventHandler(this.m_cbNumberOfAddresses_SelectedIndexChanged);
            // 
            // m_cbBusSpeed
            // 
            this.m_cbBusSpeed.FormattingEnabled = true;
            this.m_errorProvider.SetIconAlignment(this.m_cbBusSpeed, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.m_cbBusSpeed.Items.AddRange(new object[] {
            "50",
            "100",
            "400",
            "1000"});
            this.m_cbBusSpeed.Location = new System.Drawing.Point(106, 21);
            this.m_cbBusSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbBusSpeed.MaxLength = 10;
            this.m_cbBusSpeed.Name = "m_cbBusSpeed";
            this.m_cbBusSpeed.Size = new System.Drawing.Size(50, 21);
            this.m_cbBusSpeed.TabIndex = 0;
            this.m_cbBusSpeed.Validating += new System.ComponentModel.CancelEventHandler(this.m_cbBusSpeed_Validating);
            this.m_cbBusSpeed.TextChanged += new System.EventHandler(this.m_cbBusSpeed_TextChanged);
            // 
            // m_label6
            // 
            this.m_label6.AutoSize = true;
            this.m_label6.Location = new System.Drawing.Point(16, 150);
            this.m_label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label6.Name = "m_label6";
            this.m_label6.Size = new System.Drawing.Size(90, 13);
            this.m_label6.TabIndex = 4;
            this.m_label6.Text = "Sub-address size:";
            // 
            // m_label5
            // 
            this.m_label5.AutoSize = true;
            this.m_label5.Location = new System.Drawing.Point(16, 119);
            this.m_label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label5.Name = "m_label5";
            this.m_label5.Size = new System.Drawing.Size(129, 13);
            this.m_label5.TabIndex = 3;
            this.m_label5.Text = "Secondary slave address:";
            // 
            // m_label4
            // 
            this.m_label4.AutoSize = true;
            this.m_label4.Location = new System.Drawing.Point(16, 87);
            this.m_label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label4.Name = "m_label4";
            this.m_label4.Size = new System.Drawing.Size(112, 13);
            this.m_label4.TabIndex = 2;
            this.m_label4.Text = "Primary slave address:";
            // 
            // m_label3
            // 
            this.m_label3.AutoSize = true;
            this.m_label3.Location = new System.Drawing.Point(16, 55);
            this.m_label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label3.Name = "m_label3";
            this.m_label3.Size = new System.Drawing.Size(110, 13);
            this.m_label3.TabIndex = 1;
            this.m_label3.Text = "Number of addresses:";
            // 
            // m_label2
            // 
            this.m_label2.AutoSize = true;
            this.m_label2.Location = new System.Drawing.Point(16, 24);
            this.m_label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label2.Name = "m_label2";
            this.m_label2.Size = new System.Drawing.Size(54, 13);
            this.m_label2.TabIndex = 0;
            this.m_label2.Text = "Data rate:";
            // 
            // m_groupBox2
            // 
            this.m_groupBox2.Controls.Add(this.m_label1);
            this.m_groupBox2.Location = new System.Drawing.Point(231, 0);
            this.m_groupBox2.Name = "m_groupBox2";
            this.m_groupBox2.Size = new System.Drawing.Size(217, 237);
            this.m_groupBox2.TabIndex = 1;
            this.m_groupBox2.TabStop = false;
            // 
            // m_label1
            // 
            this.m_label1.Location = new System.Drawing.Point(9, 20);
            this.m_label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_label1.Name = "m_label1";
            this.m_label1.Size = new System.Drawing.Size(206, 207);
            this.m_label1.TabIndex = 0;
            this.m_label1.Text = resources.GetString("m_label1.Text");
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyEZI2CBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.Controls.Add(this.m_groupBox2);
            this.Controls.Add(this.m_groupBox1);
            this.Name = "CyEZI2CBasic";
            this.Size = new System.Drawing.Size(471, 259);
            this.Load += new System.EventHandler(this.CyEZI2CBasic_Load);
            this.m_groupBox1.ResumeLayout(false);
            this.m_groupBox1.PerformLayout();
            this.m_groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_groupBox1;
        private System.Windows.Forms.GroupBox m_groupBox2;
        private System.Windows.Forms.Label m_label1;
        private System.Windows.Forms.Label m_label6;
        private System.Windows.Forms.Label m_label5;
        private System.Windows.Forms.Label m_label4;
        private System.Windows.Forms.Label m_label3;
        private System.Windows.Forms.Label m_label2;
        private System.Windows.Forms.TextBox m_tbSecondarySlaveAddress;
        private System.Windows.Forms.TextBox m_tbPrimarySlaveAddress;
        private System.Windows.Forms.ComboBox m_cbSubAddressSize;
        private System.Windows.Forms.ComboBox m_cbNumberOfAddresses;
        private System.Windows.Forms.ComboBox m_cbBusSpeed;
        private System.Windows.Forms.CheckBox m_checkBoxWakeup;
        private System.Windows.Forms.Label m_label7;
        private System.Windows.Forms.Label m_label8;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
        private System.Windows.Forms.ToolTip m_toolTip1;
        private System.Windows.Forms.ComboBox m_cbPort;
        private System.Windows.Forms.Label m_label9;
    }
}
