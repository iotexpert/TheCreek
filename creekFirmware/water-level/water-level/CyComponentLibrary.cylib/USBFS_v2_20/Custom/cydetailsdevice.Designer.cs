namespace USBFS_v2_20
{
    partial class CyDetailsDevice
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
            this.groupBoxParams = new System.Windows.Forms.GroupBox();
            this.labelSerial = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numUpDownRelease = new System.Windows.Forms.NumericUpDown();
            this.numUpDownPID = new System.Windows.Forms.NumericUpDown();
            this.numUpDownVID = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label0x = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxProductString = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxDeviceSubclass = new System.Windows.Forms.ComboBox();
            this.comboBoxDeviceClass = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxManufacter = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRelease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownVID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxParams.Controls.Add(this.labelSerial);
            this.groupBoxParams.Controls.Add(this.label4);
            this.groupBoxParams.Controls.Add(this.numUpDownRelease);
            this.groupBoxParams.Controls.Add(this.numUpDownPID);
            this.groupBoxParams.Controls.Add(this.numUpDownVID);
            this.groupBoxParams.Controls.Add(this.label10);
            this.groupBoxParams.Controls.Add(this.label9);
            this.groupBoxParams.Controls.Add(this.label0x);
            this.groupBoxParams.Controls.Add(this.label8);
            this.groupBoxParams.Controls.Add(this.comboBoxProductString);
            this.groupBoxParams.Controls.Add(this.label7);
            this.groupBoxParams.Controls.Add(this.comboBoxDeviceSubclass);
            this.groupBoxParams.Controls.Add(this.comboBoxDeviceClass);
            this.groupBoxParams.Controls.Add(this.label6);
            this.groupBoxParams.Controls.Add(this.label5);
            this.groupBoxParams.Controls.Add(this.comboBoxManufacter);
            this.groupBoxParams.Controls.Add(this.label3);
            this.groupBoxParams.Controls.Add(this.label2);
            this.groupBoxParams.Controls.Add(this.label1);
            this.groupBoxParams.Location = new System.Drawing.Point(3, 9);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Size = new System.Drawing.Size(346, 243);
            this.groupBoxParams.TabIndex = 0;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Device Attributes";
            // 
            // labelSerial
            // 
            this.labelSerial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSerial.Location = new System.Drawing.Point(119, 210);
            this.labelSerial.Name = "labelSerial";
            this.labelSerial.Size = new System.Drawing.Size(110, 21);
            this.labelSerial.TabIndex = 19;
            this.labelSerial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Serial String:";
            // 
            // numUpDownRelease
            // 
            this.numUpDownRelease.Hexadecimal = true;
            this.numUpDownRelease.Location = new System.Drawing.Point(120, 74);
            this.numUpDownRelease.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUpDownRelease.Name = "numUpDownRelease";
            this.numUpDownRelease.Size = new System.Drawing.Size(109, 20);
            this.numUpDownRelease.TabIndex = 3;
            this.numUpDownRelease.Validated += new System.EventHandler(this.numUpDownRelease_Validated);
            // 
            // numUpDownPID
            // 
            this.numUpDownPID.Hexadecimal = true;
            this.numUpDownPID.Location = new System.Drawing.Point(120, 47);
            this.numUpDownPID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUpDownPID.Name = "numUpDownPID";
            this.numUpDownPID.Size = new System.Drawing.Size(109, 20);
            this.numUpDownPID.TabIndex = 2;
            this.numUpDownPID.Validated += new System.EventHandler(this.numUpDownPID_Validated);
            // 
            // numUpDownVID
            // 
            this.numUpDownVID.Hexadecimal = true;
            this.numUpDownVID.Location = new System.Drawing.Point(120, 19);
            this.numUpDownVID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUpDownVID.Name = "numUpDownVID";
            this.numUpDownVID.Size = new System.Drawing.Size(109, 20);
            this.numUpDownVID.TabIndex = 1;
            this.numUpDownVID.Validated += new System.EventHandler(this.numUpDownVID_Validated);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(101, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "0x";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(101, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "0x";
            // 
            // label0x
            // 
            this.label0x.AutoSize = true;
            this.label0x.Location = new System.Drawing.Point(101, 21);
            this.label0x.Name = "label0x";
            this.label0x.Size = new System.Drawing.Size(18, 13);
            this.label0x.TabIndex = 5;
            this.label0x.Text = "0x";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Device Release:";
            // 
            // comboBoxProductString
            // 
            this.comboBoxProductString.FormattingEnabled = true;
            this.comboBoxProductString.Location = new System.Drawing.Point(119, 181);
            this.comboBoxProductString.Name = "comboBoxProductString";
            this.comboBoxProductString.Size = new System.Drawing.Size(110, 21);
            this.comboBoxProductString.TabIndex = 7;
            this.comboBoxProductString.Validated += new System.EventHandler(this.comboBoxProductString_Validated);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Product String:";
            // 
            // comboBoxDeviceSubclass
            // 
            this.comboBoxDeviceSubclass.FormattingEnabled = true;
            this.comboBoxDeviceSubclass.Items.AddRange(new object[] {
            "No subclass"});
            this.comboBoxDeviceSubclass.Location = new System.Drawing.Point(119, 127);
            this.comboBoxDeviceSubclass.Name = "comboBoxDeviceSubclass";
            this.comboBoxDeviceSubclass.Size = new System.Drawing.Size(110, 21);
            this.comboBoxDeviceSubclass.TabIndex = 5;
            this.comboBoxDeviceSubclass.SelectedIndexChanged += new System.EventHandler(this.comboBoxDeviceSubclass_SelectedIndexChanged);
            this.comboBoxDeviceSubclass.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            this.comboBoxDeviceSubclass.Validating += new System.ComponentModel.CancelEventHandler(this.comboBox_Validating);
            // 
            // comboBoxDeviceClass
            // 
            this.comboBoxDeviceClass.DropDownWidth = 150;
            this.comboBoxDeviceClass.FormattingEnabled = true;
            this.comboBoxDeviceClass.Items.AddRange(new object[] {
            "Defined in Interface Descriptor",
            "CDC",
            "Vendor-Specific"});
            this.comboBoxDeviceClass.Location = new System.Drawing.Point(119, 99);
            this.comboBoxDeviceClass.Name = "comboBoxDeviceClass";
            this.comboBoxDeviceClass.Size = new System.Drawing.Size(110, 21);
            this.comboBoxDeviceClass.TabIndex = 4;
            this.comboBoxDeviceClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxDeviceClass_SelectedIndexChanged);
            this.comboBoxDeviceClass.TextChanged += new System.EventHandler(this.comboBox_TextChanged);
            this.comboBoxDeviceClass.Validating += new System.ComponentModel.CancelEventHandler(this.comboBox_Validating);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Device Subclass:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Device Class:";
            // 
            // comboBoxManufacter
            // 
            this.comboBoxManufacter.FormattingEnabled = true;
            this.comboBoxManufacter.Location = new System.Drawing.Point(119, 154);
            this.comboBoxManufacter.Name = "comboBoxManufacter";
            this.comboBoxManufacter.Size = new System.Drawing.Size(110, 21);
            this.comboBoxManufacter.TabIndex = 6;
            this.comboBoxManufacter.Validated += new System.EventHandler(this.comboBoxManufacter_Validated);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Manufacturing String:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Product ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vendor ID:";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // CyDetailsDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBoxParams);
            this.Name = "CyDetailsDevice";
            this.Size = new System.Drawing.Size(352, 419);
            this.groupBoxParams.ResumeLayout(false);
            this.groupBoxParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRelease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownVID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxManufacter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxDeviceSubclass;
        private System.Windows.Forms.ComboBox comboBoxDeviceClass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxProductString;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label0x;
        private System.Windows.Forms.NumericUpDown numUpDownVID;
        private System.Windows.Forms.NumericUpDown numUpDownRelease;
        private System.Windows.Forms.NumericUpDown numUpDownPID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSerial;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
