namespace USBFS_v1_20
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
            this.groupBoxParams = new System.Windows.Forms.GroupBox();
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
            this.comboBoxSerial = new System.Windows.Forms.ComboBox();
            this.comboBoxManufacter = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxMemoryMgmt = new System.Windows.Forms.GroupBox();
            this.rbDMAAutomatic = new System.Windows.Forms.RadioButton();
            this.rbDMAManual = new System.Windows.Forms.RadioButton();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.groupBoxParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRelease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownVID)).BeginInit();
            this.groupBoxMemoryMgmt.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBoxParams.Controls.Add(this.comboBoxSerial);
            this.groupBoxParams.Controls.Add(this.comboBoxManufacter);
            this.groupBoxParams.Controls.Add(this.label4);
            this.groupBoxParams.Controls.Add(this.label3);
            this.groupBoxParams.Controls.Add(this.label2);
            this.groupBoxParams.Controls.Add(this.label1);
            this.groupBoxParams.Location = new System.Drawing.Point(0, 11);
            this.groupBoxParams.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxParams.Size = new System.Drawing.Size(330, 292);
            this.groupBoxParams.TabIndex = 0;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Device Attributes";
            // 
            // numUpDownRelease
            // 
            this.numUpDownRelease.Hexadecimal = true;
            this.numUpDownRelease.Location = new System.Drawing.Point(160, 91);
            this.numUpDownRelease.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numUpDownRelease.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUpDownRelease.Name = "numUpDownRelease";
            this.numUpDownRelease.Size = new System.Drawing.Size(145, 22);
            this.numUpDownRelease.TabIndex = 3;
            this.numUpDownRelease.Validated += new System.EventHandler(this.numUpDownRelease_Validated);
            // 
            // numUpDownPID
            // 
            this.numUpDownPID.Hexadecimal = true;
            this.numUpDownPID.Location = new System.Drawing.Point(160, 58);
            this.numUpDownPID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numUpDownPID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUpDownPID.Name = "numUpDownPID";
            this.numUpDownPID.Size = new System.Drawing.Size(145, 22);
            this.numUpDownPID.TabIndex = 2;
            this.numUpDownPID.Validated += new System.EventHandler(this.numUpDownPID_Validated);
            // 
            // numUpDownVID
            // 
            this.numUpDownVID.Hexadecimal = true;
            this.numUpDownVID.Location = new System.Drawing.Point(160, 23);
            this.numUpDownVID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numUpDownVID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUpDownVID.Name = "numUpDownVID";
            this.numUpDownVID.Size = new System.Drawing.Size(145, 22);
            this.numUpDownVID.TabIndex = 1;
            this.numUpDownVID.Validated += new System.EventHandler(this.numUpDownVID_Validated);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(135, 94);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 17);
            this.label10.TabIndex = 17;
            this.label10.Text = "0x";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(135, 60);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "0x";
            // 
            // label0x
            // 
            this.label0x.AutoSize = true;
            this.label0x.Location = new System.Drawing.Point(135, 26);
            this.label0x.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label0x.Name = "label0x";
            this.label0x.Size = new System.Drawing.Size(22, 17);
            this.label0x.TabIndex = 5;
            this.label0x.Text = "0x";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 94);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Device Release";
            // 
            // comboBoxProductString
            // 
            this.comboBoxProductString.FormattingEnabled = true;
            this.comboBoxProductString.Location = new System.Drawing.Point(159, 223);
            this.comboBoxProductString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxProductString.Name = "comboBoxProductString";
            this.comboBoxProductString.Size = new System.Drawing.Size(145, 24);
            this.comboBoxProductString.TabIndex = 7;
            this.comboBoxProductString.Validated += new System.EventHandler(this.comboBoxProductString_Validated);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 226);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "Product String";
            // 
            // comboBoxDeviceSubclass
            // 
            this.comboBoxDeviceSubclass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDeviceSubclass.FormattingEnabled = true;
            this.comboBoxDeviceSubclass.Items.AddRange(new object[] {
            "No subclass"});
            this.comboBoxDeviceSubclass.Location = new System.Drawing.Point(159, 156);
            this.comboBoxDeviceSubclass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxDeviceSubclass.Name = "comboBoxDeviceSubclass";
            this.comboBoxDeviceSubclass.Size = new System.Drawing.Size(145, 24);
            this.comboBoxDeviceSubclass.TabIndex = 5;
            // 
            // comboBoxDeviceClass
            // 
            this.comboBoxDeviceClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDeviceClass.DropDownWidth = 150;
            this.comboBoxDeviceClass.FormattingEnabled = true;
            this.comboBoxDeviceClass.Items.AddRange(new object[] {
            "Defined in Interface Descriptor",
            "Vendor-Specific"});
            this.comboBoxDeviceClass.Location = new System.Drawing.Point(159, 122);
            this.comboBoxDeviceClass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxDeviceClass.Name = "comboBoxDeviceClass";
            this.comboBoxDeviceClass.Size = new System.Drawing.Size(145, 24);
            this.comboBoxDeviceClass.TabIndex = 4;
            this.comboBoxDeviceClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxDeviceClass_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 160);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Device Subclass";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 126);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Device Class";
            // 
            // comboBoxSerial
            // 
            this.comboBoxSerial.FormattingEnabled = true;
            this.comboBoxSerial.Location = new System.Drawing.Point(159, 256);
            this.comboBoxSerial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxSerial.Name = "comboBoxSerial";
            this.comboBoxSerial.Size = new System.Drawing.Size(145, 24);
            this.comboBoxSerial.TabIndex = 8;
            this.comboBoxSerial.Validated += new System.EventHandler(this.comboBoxSerial_Validated);
            // 
            // comboBoxManufacter
            // 
            this.comboBoxManufacter.FormattingEnabled = true;
            this.comboBoxManufacter.Location = new System.Drawing.Point(159, 190);
            this.comboBoxManufacter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxManufacter.Name = "comboBoxManufacter";
            this.comboBoxManufacter.Size = new System.Drawing.Size(145, 24);
            this.comboBoxManufacter.TabIndex = 6;
            this.comboBoxManufacter.Validated += new System.EventHandler(this.comboBoxManufacter_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 260);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Serial Number String";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 193);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Manufacturing String";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Product ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vendor ID";
            // 
            // groupBoxMemoryMgmt
            // 
            this.groupBoxMemoryMgmt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryMgmt.Controls.Add(this.rbDMAAutomatic);
            this.groupBoxMemoryMgmt.Controls.Add(this.rbDMAManual);
            this.groupBoxMemoryMgmt.Controls.Add(this.rbManual);
            this.groupBoxMemoryMgmt.Location = new System.Drawing.Point(0, 310);
            this.groupBoxMemoryMgmt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMemoryMgmt.Name = "groupBoxMemoryMgmt";
            this.groupBoxMemoryMgmt.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMemoryMgmt.Size = new System.Drawing.Size(330, 123);
            this.groupBoxMemoryMgmt.TabIndex = 5;
            this.groupBoxMemoryMgmt.TabStop = false;
            this.groupBoxMemoryMgmt.Text = "Endpoint Memory Management";
            // 
            // rbDMAAutomatic
            // 
            this.rbDMAAutomatic.AutoSize = true;
            this.rbDMAAutomatic.Location = new System.Drawing.Point(16, 81);
            this.rbDMAAutomatic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbDMAAutomatic.Name = "rbDMAAutomatic";
            this.rbDMAAutomatic.Size = new System.Drawing.Size(230, 21);
            this.rbDMAAutomatic.TabIndex = 0;
            this.rbDMAAutomatic.Text = "DMA w/Automatic Memory Mgmt";
            this.rbDMAAutomatic.UseVisualStyleBackColor = true;
            this.rbDMAAutomatic.Visible = false;
            // 
            // rbDMAManual
            // 
            this.rbDMAManual.AutoSize = true;
            this.rbDMAManual.Location = new System.Drawing.Point(16, 53);
            this.rbDMAManual.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbDMAManual.Name = "rbDMAManual";
            this.rbDMAManual.Size = new System.Drawing.Size(214, 21);
            this.rbDMAManual.TabIndex = 0;
            this.rbDMAManual.Text = "DMA w/Manual Memory Mgmt";
            this.rbDMAManual.UseVisualStyleBackColor = true;
            // 
            // rbManual
            // 
            this.rbManual.AutoSize = true;
            this.rbManual.Checked = true;
            this.rbManual.Location = new System.Drawing.Point(16, 25);
            this.rbManual.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbManual.Name = "rbManual";
            this.rbManual.Size = new System.Drawing.Size(132, 21);
            this.rbManual.TabIndex = 0;
            this.rbManual.TabStop = true;
            this.rbManual.Text = "Manual (default)";
            this.rbManual.UseVisualStyleBackColor = true;
            // 
            // CyDetailsDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBoxMemoryMgmt);
            this.Controls.Add(this.groupBoxParams);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CyDetailsDevice";
            this.Size = new System.Drawing.Size(334, 443);
            this.groupBoxParams.ResumeLayout(false);
            this.groupBoxParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownRelease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownVID)).EndInit();
            this.groupBoxMemoryMgmt.ResumeLayout(false);
            this.groupBoxMemoryMgmt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSerial;
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
        private System.Windows.Forms.GroupBox groupBoxMemoryMgmt;
        private System.Windows.Forms.RadioButton rbDMAAutomatic;
        private System.Windows.Forms.RadioButton rbDMAManual;
        private System.Windows.Forms.RadioButton rbManual;
    }
}
