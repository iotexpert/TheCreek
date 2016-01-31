namespace I2C_v1_20
{
    partial class CyI2CBasic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyI2CBasic));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.rbSoftware = new System.Windows.Forms.RadioButton();
            this.rbHardware = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rbFixedFunction = new System.Windows.Forms.RadioButton();
            this.rbUDB = new System.Windows.Forms.RadioButton();
            this.tbSlaveAddress = new System.Windows.Forms.TextBox();
            this.lbSlaveAddress = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ckbWakeup = new System.Windows.Forms.CheckBox();
            this.cbBusSpeed = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.tbSlaveAddress);
            this.groupBox1.Controls.Add(this.lbSlaveAddress);
            this.groupBox1.Controls.Add(this.cbMode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.ckbWakeup);
            this.groupBox1.Controls.Add(this.cbBusSpeed);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 242);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.rbSoftware);
            this.panel2.Controls.Add(this.rbHardware);
            this.panel2.Location = new System.Drawing.Point(3, 96);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(281, 20);
            this.panel2.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Address Decode:";
            // 
            // rbSoftware
            // 
            this.rbSoftware.AutoSize = true;
            this.rbSoftware.Location = new System.Drawing.Point(214, 0);
            this.rbSoftware.Name = "rbSoftware";
            this.rbSoftware.Size = new System.Drawing.Size(67, 17);
            this.rbSoftware.TabIndex = 1;
            this.rbSoftware.TabStop = true;
            this.rbSoftware.Text = "Software";
            this.rbSoftware.UseVisualStyleBackColor = true;
            // 
            // rbHardware
            // 
            this.rbHardware.AutoSize = true;
            this.rbHardware.Location = new System.Drawing.Point(111, 0);
            this.rbHardware.Name = "rbHardware";
            this.rbHardware.Size = new System.Drawing.Size(71, 17);
            this.rbHardware.TabIndex = 0;
            this.rbHardware.TabStop = true;
            this.rbHardware.Text = "Hardware";
            this.rbHardware.UseVisualStyleBackColor = true;
            this.rbHardware.CheckedChanged += new System.EventHandler(this.rbHardware_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.rbFixedFunction);
            this.panel1.Controls.Add(this.rbUDB);
            this.panel1.Location = new System.Drawing.Point(3, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(281, 20);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Implementation:";
            // 
            // rbFixedFunction
            // 
            this.rbFixedFunction.AutoSize = true;
            this.rbFixedFunction.Location = new System.Drawing.Point(111, 0);
            this.rbFixedFunction.Name = "rbFixedFunction";
            this.rbFixedFunction.Size = new System.Drawing.Size(94, 17);
            this.rbFixedFunction.TabIndex = 0;
            this.rbFixedFunction.TabStop = true;
            this.rbFixedFunction.Text = "Fixed Function";
            this.rbFixedFunction.UseVisualStyleBackColor = true;
            this.rbFixedFunction.CheckedChanged += new System.EventHandler(this.rbFixedFunction_CheckedChanged);
            // 
            // rbUDB
            // 
            this.rbUDB.AutoSize = true;
            this.rbUDB.Location = new System.Drawing.Point(214, 0);
            this.rbUDB.Name = "rbUDB";
            this.rbUDB.Size = new System.Drawing.Size(48, 17);
            this.rbUDB.TabIndex = 1;
            this.rbUDB.TabStop = true;
            this.rbUDB.Text = "UDB";
            this.rbUDB.UseVisualStyleBackColor = true;
            // 
            // tbSlaveAddress
            // 
            this.tbSlaveAddress.Location = new System.Drawing.Point(114, 121);
            this.tbSlaveAddress.MaxLength = 5;
            this.tbSlaveAddress.Name = "tbSlaveAddress";
            this.tbSlaveAddress.Size = new System.Drawing.Size(84, 20);
            this.tbSlaveAddress.TabIndex = 5;
            this.tbSlaveAddress.TextChanged += new System.EventHandler(this.tbSlaveAddress_TextChanged);
            this.tbSlaveAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.tbSlaveAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            // 
            // lbSlaveAddress
            // 
            this.lbSlaveAddress.AutoSize = true;
            this.lbSlaveAddress.Location = new System.Drawing.Point(25, 124);
            this.lbSlaveAddress.Name = "lbSlaveAddress";
            this.lbSlaveAddress.Size = new System.Drawing.Size(78, 13);
            this.lbSlaveAddress.TabIndex = 15;
            this.lbSlaveAddress.Text = "Slave Address:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(114, 17);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(134, 21);
            this.cbMode.TabIndex = 0;
            this.cbMode.Validating += new System.ComponentModel.CancelEventHandler(this.ControlsValidating);
            this.cbMode.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Mode:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(172, 49);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "kHz";
            // 
            // ckbWakeup
            // 
            this.ckbWakeup.AutoSize = true;
            this.ckbWakeup.Location = new System.Drawing.Point(11, 217);
            this.ckbWakeup.Margin = new System.Windows.Forms.Padding(2);
            this.ckbWakeup.Name = "ckbWakeup";
            this.ckbWakeup.Size = new System.Drawing.Size(150, 17);
            this.ckbWakeup.TabIndex = 6;
            this.ckbWakeup.Text = "Wakeup from Sleep Mode";
            this.ckbWakeup.UseVisualStyleBackColor = true;
            this.ckbWakeup.CheckedChanged += new System.EventHandler(this.checkBoxWakeup_CheckedChanged);
            // 
            // cbBusSpeed
            // 
            this.cbBusSpeed.FormattingEnabled = true;
            this.cbBusSpeed.Items.AddRange(new object[] {
            "50",
            "100",
            "400"});
            this.cbBusSpeed.Location = new System.Drawing.Point(114, 43);
            this.cbBusSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.cbBusSpeed.MaxLength = 5;
            this.cbBusSpeed.Name = "cbBusSpeed";
            this.cbBusSpeed.Size = new System.Drawing.Size(50, 21);
            this.cbBusSpeed.TabIndex = 1;
            this.cbBusSpeed.Validating += new System.ComponentModel.CancelEventHandler(this.ControlsValidating);
            this.cbBusSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.cbBusSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.cbBusSpeed.TextChanged += new System.EventHandler(this.cbBusSpeed_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Bus speed:";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(294, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(158, 242);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(5, 7);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(148, 207);
            this.label8.TabIndex = 1;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // CyI2CBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyI2CBasic";
            this.Size = new System.Drawing.Size(471, 259);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cbBusSpeed;
        public System.Windows.Forms.CheckBox ckbWakeup;
        public System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox tbSlaveAddress;
        private System.Windows.Forms.Label lbSlaveAddress;
        public System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RadioButton rbFixedFunction;
        public System.Windows.Forms.RadioButton rbUDB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.RadioButton rbSoftware;
        public System.Windows.Forms.RadioButton rbHardware;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
    }
}
