namespace I2C_v2_20
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
            this.m_cbPort = new System.Windows.Forms.ComboBox();
            this.m_lblPort = new System.Windows.Forms.Label();
            this.gbIFBC = new System.Windows.Forms.GroupBox();
            this.lblCalcBusSpeed = new System.Windows.Forms.Label();
            this.lblSourceFreq = new System.Windows.Forms.Label();
            this.lblHexademical = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.rbSoftware = new System.Windows.Forms.RadioButton();
            this.rbHardware = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rbFixedFunction = new System.Windows.Forms.RadioButton();
            this.rbUDB = new System.Windows.Forms.RadioButton();
            this.tbSlaveAddress = new System.Windows.Forms.TextBox();
            this.lblSlaveAddress = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chbWakeup = new System.Windows.Forms.CheckBox();
            this.cbBusSpeed = new System.Windows.Forms.ComboBox();
            this.lblBusSpeed = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbIFBC.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_cbPort);
            this.groupBox1.Controls.Add(this.m_lblPort);
            this.groupBox1.Controls.Add(this.gbIFBC);
            this.groupBox1.Controls.Add(this.lblHexademical);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.tbSlaveAddress);
            this.groupBox1.Controls.Add(this.lblSlaveAddress);
            this.groupBox1.Controls.Add(this.cbMode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chbWakeup);
            this.groupBox1.Controls.Add(this.cbBusSpeed);
            this.groupBox1.Controls.Add(this.lblBusSpeed);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 238);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // m_cbPort
            // 
            this.m_cbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPort.FormattingEnabled = true;
            this.m_cbPort.Location = new System.Drawing.Point(114, 192);
            this.m_cbPort.Name = "m_cbPort";
            this.m_cbPort.Size = new System.Drawing.Size(68, 21);
            this.m_cbPort.TabIndex = 19;
            this.m_cbPort.SelectedIndexChanged += new System.EventHandler(this.m_cbPort_SelectedIndexChanged);
            // 
            // m_lblPort
            // 
            this.m_lblPort.AutoSize = true;
            this.m_lblPort.Location = new System.Drawing.Point(16, 195);
            this.m_lblPort.Name = "m_lblPort";
            this.m_lblPort.Size = new System.Drawing.Size(87, 13);
            this.m_lblPort.TabIndex = 18;
            this.m_lblPort.Text = "Pin Connections:";
            // 
            // gbIFBC
            // 
            this.gbIFBC.Controls.Add(this.lblCalcBusSpeed);
            this.gbIFBC.Controls.Add(this.lblSourceFreq);
            this.gbIFBC.Location = new System.Drawing.Point(114, 65);
            this.gbIFBC.Name = "gbIFBC";
            this.gbIFBC.Size = new System.Drawing.Size(182, 53);
            this.gbIFBC.TabIndex = 17;
            this.gbIFBC.TabStop = false;
            this.gbIFBC.Text = "Input Frequency Calculations:";
            // 
            // lblCalcBusSpeed
            // 
            this.lblCalcBusSpeed.AutoSize = true;
            this.lblCalcBusSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCalcBusSpeed.Location = new System.Drawing.Point(11, 35);
            this.lblCalcBusSpeed.Name = "lblCalcBusSpeed";
            this.lblCalcBusSpeed.Size = new System.Drawing.Size(126, 13);
            this.lblCalcBusSpeed.TabIndex = 1;
            this.lblCalcBusSpeed.Text = "Data Rate = UNKNOWN";
            // 
            // lblSourceFreq
            // 
            this.lblSourceFreq.AutoSize = true;
            this.lblSourceFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSourceFreq.Location = new System.Drawing.Point(11, 16);
            this.lblSourceFreq.Name = "lblSourceFreq";
            this.lblSourceFreq.Size = new System.Drawing.Size(135, 13);
            this.lblSourceFreq.TabIndex = 0;
            this.lblSourceFreq.Text = "Source Freq = UNKNOWN";
            // 
            // lblHexademical
            // 
            this.lblHexademical.AutoSize = true;
            this.lblHexademical.Location = new System.Drawing.Point(166, 172);
            this.lblHexademical.Name = "lblHexademical";
            this.lblHexademical.Size = new System.Drawing.Size(125, 13);
            this.lblHexademical.TabIndex = 16;
            this.lblHexademical.Text = "(use \'0x\' for hexadecimal)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.rbSoftware);
            this.panel2.Controls.Add(this.rbHardware);
            this.panel2.Location = new System.Drawing.Point(3, 145);
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
            this.panel1.Location = new System.Drawing.Point(3, 121);
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
            this.errorProvider.SetIconAlignment(this.tbSlaveAddress, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.tbSlaveAddress.Location = new System.Drawing.Point(114, 169);
            this.tbSlaveAddress.MaxLength = 5;
            this.tbSlaveAddress.Name = "tbSlaveAddress";
            this.tbSlaveAddress.Size = new System.Drawing.Size(50, 20);
            this.tbSlaveAddress.TabIndex = 5;
            this.tbSlaveAddress.TextChanged += new System.EventHandler(this.tbSlaveAddress_TextChanged);
            this.tbSlaveAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.tbSlaveAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.tbSlaveAddress.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxValidating);
            // 
            // lblSlaveAddress
            // 
            this.lblSlaveAddress.AutoSize = true;
            this.lblSlaveAddress.Location = new System.Drawing.Point(25, 172);
            this.lblSlaveAddress.Name = "lblSlaveAddress";
            this.lblSlaveAddress.Size = new System.Drawing.Size(78, 13);
            this.lblSlaveAddress.TabIndex = 15;
            this.lblSlaveAddress.Text = "Slave Address:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbMode, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbMode.Location = new System.Drawing.Point(114, 17);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(134, 21);
            this.cbMode.TabIndex = 0;
            this.cbMode.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
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
            this.label7.Location = new System.Drawing.Point(172, 47);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "kbps";
            // 
            // chbWakeup
            // 
            this.chbWakeup.AutoSize = true;
            this.chbWakeup.Location = new System.Drawing.Point(17, 217);
            this.chbWakeup.Margin = new System.Windows.Forms.Padding(2);
            this.chbWakeup.Name = "chbWakeup";
            this.chbWakeup.Size = new System.Drawing.Size(183, 17);
            this.chbWakeup.TabIndex = 6;
            this.chbWakeup.Text = "Enable wakeup from Sleep Mode";
            this.chbWakeup.UseVisualStyleBackColor = true;
            this.chbWakeup.CheckedChanged += new System.EventHandler(this.checkBoxWakeup_CheckedChanged);
            // 
            // cbBusSpeed
            // 
            this.cbBusSpeed.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbBusSpeed, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbBusSpeed.Items.AddRange(new object[] {
            "50",
            "100",
            "400",
            "1000"});
            this.cbBusSpeed.Location = new System.Drawing.Point(114, 41);
            this.cbBusSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.cbBusSpeed.MaxLength = 5;
            this.cbBusSpeed.Name = "cbBusSpeed";
            this.cbBusSpeed.Size = new System.Drawing.Size(50, 21);
            this.cbBusSpeed.TabIndex = 1;
            this.cbBusSpeed.Validating += new System.ComponentModel.CancelEventHandler(this.ComboBoxValidating);
            this.cbBusSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditBox_KeyPress);
            this.cbBusSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditBox_KeyDown);
            this.cbBusSpeed.TextChanged += new System.EventHandler(this.cbBusSpeed_TextChanged);
            // 
            // lblBusSpeed
            // 
            this.lblBusSpeed.AutoSize = true;
            this.lblBusSpeed.Location = new System.Drawing.Point(44, 44);
            this.lblBusSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBusSpeed.Name = "lblBusSpeed";
            this.lblBusSpeed.Size = new System.Drawing.Size(59, 13);
            this.lblBusSpeed.TabIndex = 0;
            this.lblBusSpeed.Text = "Data Rate:";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(308, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 238);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 16);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(3);
            this.label8.Size = new System.Drawing.Size(159, 219);
            this.label8.TabIndex = 1;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // CyI2CBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyI2CBasic";
            this.Size = new System.Drawing.Size(474, 245);
            this.Load += new System.EventHandler(this.CyI2CBasic_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbIFBC.ResumeLayout(false);
            this.gbIFBC.PerformLayout();
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
        private System.Windows.Forms.Label lblBusSpeed;
        public System.Windows.Forms.ComboBox cbBusSpeed;
        public System.Windows.Forms.CheckBox chbWakeup;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox tbSlaveAddress;
        private System.Windows.Forms.Label lblSlaveAddress;
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
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblHexademical;
        private System.Windows.Forms.GroupBox gbIFBC;
        private System.Windows.Forms.Label lblCalcBusSpeed;
        private System.Windows.Forms.Label lblSourceFreq;
        private System.Windows.Forms.ComboBox m_cbPort;
        private System.Windows.Forms.Label m_lblPort;
    }
}
