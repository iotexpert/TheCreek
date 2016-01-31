namespace EZI2C_v1_20
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxWakeup = new System.Windows.Forms.CheckBox();
            this.tbSecondarySlaveAddress = new System.Windows.Forms.TextBox();
            this.tbPrimarySlaveAddress = new System.Windows.Forms.TextBox();
            this.cbSubAddressSize = new System.Windows.Forms.ComboBox();
            this.cbNumberOfAddresses = new System.Windows.Forms.ComboBox();
            this.cbBusSpeed = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.checkBoxWakeup);
            this.groupBox1.Controls.Add(this.tbSecondarySlaveAddress);
            this.groupBox1.Controls.Add(this.tbPrimarySlaveAddress);
            this.groupBox1.Controls.Add(this.cbSubAddressSize);
            this.groupBox1.Controls.Add(this.cbNumberOfAddresses);
            this.groupBox1.Controls.Add(this.cbBusSpeed);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 240);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(202, 150);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "bit";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(154, 24);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "kHz";
            // 
            // checkBoxWakeup
            // 
            this.checkBoxWakeup.AutoSize = true;
            this.checkBoxWakeup.Location = new System.Drawing.Point(19, 210);
            this.checkBoxWakeup.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxWakeup.Name = "checkBoxWakeup";
            this.checkBoxWakeup.Size = new System.Drawing.Size(150, 17);
            this.checkBoxWakeup.TabIndex = 5;
            this.checkBoxWakeup.Text = "Wakeup from Sleep Mode";
            this.checkBoxWakeup.UseVisualStyleBackColor = true;
            this.checkBoxWakeup.CheckedChanged += new System.EventHandler(this.checkBoxWakeup_CheckedChanged);
            // 
            // tbSecondarySlaveAddress
            // 
            this.tbSecondarySlaveAddress.Location = new System.Drawing.Point(154, 116);
            this.tbSecondarySlaveAddress.Margin = new System.Windows.Forms.Padding(2);
            this.tbSecondarySlaveAddress.MaxLength = 5;
            this.tbSecondarySlaveAddress.Name = "tbSecondarySlaveAddress";
            this.tbSecondarySlaveAddress.Size = new System.Drawing.Size(44, 20);
            this.tbSecondarySlaveAddress.TabIndex = 3;
            this.tbSecondarySlaveAddress.TextChanged += new System.EventHandler(this.tbSecondarySlaveAddress_TextChanged);
            this.tbSecondarySlaveAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.tbSecondarySlaveAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.tbSecondarySlaveAddress.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxValidating);
            // 
            // tbPrimarySlaveAddress
            // 
            this.tbPrimarySlaveAddress.Location = new System.Drawing.Point(154, 84);
            this.tbPrimarySlaveAddress.Margin = new System.Windows.Forms.Padding(2);
            this.tbPrimarySlaveAddress.MaxLength = 5;
            this.tbPrimarySlaveAddress.Name = "tbPrimarySlaveAddress";
            this.tbPrimarySlaveAddress.Size = new System.Drawing.Size(44, 20);
            this.tbPrimarySlaveAddress.TabIndex = 2;
            this.tbPrimarySlaveAddress.TextChanged += new System.EventHandler(this.tbPrimarySlaveAddress_TextChanged);
            this.tbPrimarySlaveAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.tbPrimarySlaveAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.tbPrimarySlaveAddress.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxValidating);
            // 
            // cbSubAddressSize
            // 
            this.cbSubAddressSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubAddressSize.FormattingEnabled = true;
            this.cbSubAddressSize.Items.AddRange(new object[] {
            "8",
            "16"});
            this.cbSubAddressSize.Location = new System.Drawing.Point(154, 148);
            this.cbSubAddressSize.Margin = new System.Windows.Forms.Padding(2);
            this.cbSubAddressSize.Name = "cbSubAddressSize";
            this.cbSubAddressSize.Size = new System.Drawing.Size(44, 21);
            this.cbSubAddressSize.TabIndex = 4;
            this.cbSubAddressSize.SelectedIndexChanged += new System.EventHandler(this.cbSubAddressSize_SelectedIndexChanged);
            // 
            // cbNumberOfAddresses
            // 
            this.cbNumberOfAddresses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNumberOfAddresses.FormattingEnabled = true;
            this.cbNumberOfAddresses.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbNumberOfAddresses.Location = new System.Drawing.Point(154, 53);
            this.cbNumberOfAddresses.Margin = new System.Windows.Forms.Padding(2);
            this.cbNumberOfAddresses.Name = "cbNumberOfAddresses";
            this.cbNumberOfAddresses.Size = new System.Drawing.Size(44, 21);
            this.cbNumberOfAddresses.TabIndex = 1;
            this.cbNumberOfAddresses.SelectedIndexChanged += new System.EventHandler(this.cbNumberOfAddresses_SelectedIndexChanged);
            // 
            // cbBusSpeed
            // 
            this.cbBusSpeed.FormattingEnabled = true;
            this.cbBusSpeed.Items.AddRange(new object[] {
            "50",
            "100",
            "400"});
            this.cbBusSpeed.Location = new System.Drawing.Point(101, 21);
            this.cbBusSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.cbBusSpeed.MaxLength = 5;
            this.cbBusSpeed.Name = "cbBusSpeed";
            this.cbBusSpeed.Size = new System.Drawing.Size(50, 21);
            this.cbBusSpeed.TabIndex = 0;
            this.cbBusSpeed.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            this.cbBusSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.cbBusSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.cbBusSpeed.TextChanged += new System.EventHandler(this.cbBusSpeed_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 150);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Sub-address size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 119);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Secondary slave address:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 87);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Primary slave address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Number of addresses:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "I2C Bus speed:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(231, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 240);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 207);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CyEZI2CBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyEZI2CBasic";
            this.Size = new System.Drawing.Size(471, 259);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tbSecondarySlaveAddress;
        public System.Windows.Forms.TextBox tbPrimarySlaveAddress;
        public System.Windows.Forms.ComboBox cbSubAddressSize;
        public System.Windows.Forms.ComboBox cbNumberOfAddresses;
        public System.Windows.Forms.ComboBox cbBusSpeed;
        public System.Windows.Forms.CheckBox checkBoxWakeup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
