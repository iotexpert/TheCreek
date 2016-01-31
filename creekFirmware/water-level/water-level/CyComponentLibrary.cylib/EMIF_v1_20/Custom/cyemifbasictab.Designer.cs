namespace EMIF_v1_20
{
    partial class CyEMIFBasicTab
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
            this.m_lblExtMemorySpeed = new System.Windows.Forms.Label();
            this.m_numExtMemorySpeed = new System.Windows.Forms.NumericUpDown();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblExternalMemoryType = new System.Windows.Forms.Label();
            this.m_rbCustom = new System.Windows.Forms.RadioButton();
            this.m_rbSynchronous = new System.Windows.Forms.RadioButton();
            this.m_rbAsynchronous = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_rbAddressWidth24 = new System.Windows.Forms.RadioButton();
            this.m_rbAddressWidth8 = new System.Windows.Forms.RadioButton();
            this.m_rbAddressWidth16 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_rbDataWidth8 = new System.Windows.Forms.RadioButton();
            this.m_rbDataWidth16 = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.m_lblReadCycleLengthCalc = new System.Windows.Forms.Label();
            this.m_lblWriteCycleLengthCalc = new System.Windows.Forms.Label();
            this.m_lblOenPulseWidthCalc = new System.Windows.Forms.Label();
            this.m_lblOenPulseWidth = new System.Windows.Forms.Label();
            this.m_lblWenPulseWidthCalc = new System.Windows.Forms.Label();
            this.m_lblWenPulseWidth = new System.Windows.Forms.Label();
            this.m_lblReadCycleLength = new System.Windows.Forms.Label();
            this.m_lblWriteCycleLength = new System.Windows.Forms.Label();
            this.m_lblBusClockFrequency = new System.Windows.Forms.Label();
            this.m_lblBusClockFrequencyCalc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_numExtMemorySpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lblExtMemorySpeed
            // 
            this.m_lblExtMemorySpeed.AutoSize = true;
            this.m_lblExtMemorySpeed.Location = new System.Drawing.Point(7, 153);
            this.m_lblExtMemorySpeed.Name = "m_lblExtMemorySpeed";
            this.m_lblExtMemorySpeed.Size = new System.Drawing.Size(142, 13);
            this.m_lblExtMemorySpeed.TabIndex = 3;
            this.m_lblExtMemorySpeed.Text = "External Memory Speed (ns):";
            // 
            // m_numExtMemorySpeed
            // 
            this.m_errorProvider.SetIconAlignment(this.m_numExtMemorySpeed, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.m_numExtMemorySpeed.Location = new System.Drawing.Point(155, 151);
            this.m_numExtMemorySpeed.Maximum = new decimal(new int[] {
            1661992959,
            1808227885,
            5,
            0});
            this.m_numExtMemorySpeed.Name = "m_numExtMemorySpeed";
            this.m_numExtMemorySpeed.Size = new System.Drawing.Size(43, 20);
            this.m_numExtMemorySpeed.TabIndex = 3;
            this.m_numExtMemorySpeed.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.ContainerControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblExternalMemoryType);
            this.groupBox1.Controls.Add(this.m_rbCustom);
            this.groupBox1.Controls.Add(this.m_rbSynchronous);
            this.groupBox1.Controls.Add(this.m_rbAsynchronous);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(0, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 90);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "External Memory Type";
            // 
            // lblExternalMemoryType
            // 
            this.lblExternalMemoryType.AutoSize = true;
            this.lblExternalMemoryType.Location = new System.Drawing.Point(152, 21);
            this.lblExternalMemoryType.Name = "lblExternalMemoryType";
            this.lblExternalMemoryType.Size = new System.Drawing.Size(116, 13);
            this.lblExternalMemoryType.TabIndex = 9;
            this.lblExternalMemoryType.Text = "lblExternalMemoryType";
            // 
            // m_rbCustom
            // 
            this.m_rbCustom.AutoSize = true;
            this.m_rbCustom.Location = new System.Drawing.Point(10, 65);
            this.m_rbCustom.Name = "m_rbCustom";
            this.m_rbCustom.Size = new System.Drawing.Size(60, 17);
            this.m_rbCustom.TabIndex = 2;
            this.m_rbCustom.TabStop = true;
            this.m_rbCustom.Text = "Custom";
            this.m_rbCustom.UseVisualStyleBackColor = true;
            this.m_rbCustom.CheckedChanged += new System.EventHandler(this.externalMemoryType_CheckedChanged);
            // 
            // m_rbSynchronous
            // 
            this.m_rbSynchronous.AutoSize = true;
            this.m_rbSynchronous.Location = new System.Drawing.Point(10, 42);
            this.m_rbSynchronous.Name = "m_rbSynchronous";
            this.m_rbSynchronous.Size = new System.Drawing.Size(87, 17);
            this.m_rbSynchronous.TabIndex = 1;
            this.m_rbSynchronous.TabStop = true;
            this.m_rbSynchronous.Text = "Synchronous";
            this.m_rbSynchronous.UseVisualStyleBackColor = true;
            this.m_rbSynchronous.CheckedChanged += new System.EventHandler(this.externalMemoryType_CheckedChanged);
            // 
            // m_rbAsynchronous
            // 
            this.m_rbAsynchronous.AutoSize = true;
            this.m_rbAsynchronous.Location = new System.Drawing.Point(10, 19);
            this.m_rbAsynchronous.Name = "m_rbAsynchronous";
            this.m_rbAsynchronous.Size = new System.Drawing.Size(92, 17);
            this.m_rbAsynchronous.TabIndex = 0;
            this.m_rbAsynchronous.TabStop = true;
            this.m_rbAsynchronous.Text = "Asynchronous";
            this.m_rbAsynchronous.UseVisualStyleBackColor = true;
            this.m_rbAsynchronous.CheckedChanged += new System.EventHandler(this.externalMemoryType_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.m_rbAddressWidth24);
            this.groupBox2.Controls.Add(this.m_rbAddressWidth8);
            this.groupBox2.Controls.Add(this.m_rbAddressWidth16);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox2.Location = new System.Drawing.Point(0, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 45);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Address Width (bits)";
            // 
            // m_rbAddressWidth24
            // 
            this.m_rbAddressWidth24.AutoSize = true;
            this.m_rbAddressWidth24.Location = new System.Drawing.Point(10, 20);
            this.m_rbAddressWidth24.Name = "m_rbAddressWidth24";
            this.m_rbAddressWidth24.Size = new System.Drawing.Size(37, 17);
            this.m_rbAddressWidth24.TabIndex = 0;
            this.m_rbAddressWidth24.TabStop = true;
            this.m_rbAddressWidth24.Text = "24";
            this.m_rbAddressWidth24.UseVisualStyleBackColor = true;
            this.m_rbAddressWidth24.CheckedChanged += new System.EventHandler(this.addressWidth_CheckedChanged);
            // 
            // m_rbAddressWidth8
            // 
            this.m_rbAddressWidth8.AutoSize = true;
            this.m_rbAddressWidth8.Location = new System.Drawing.Point(132, 20);
            this.m_rbAddressWidth8.Name = "m_rbAddressWidth8";
            this.m_rbAddressWidth8.Size = new System.Drawing.Size(31, 17);
            this.m_rbAddressWidth8.TabIndex = 2;
            this.m_rbAddressWidth8.TabStop = true;
            this.m_rbAddressWidth8.Text = "8";
            this.m_rbAddressWidth8.UseVisualStyleBackColor = true;
            this.m_rbAddressWidth8.CheckedChanged += new System.EventHandler(this.addressWidth_CheckedChanged);
            // 
            // m_rbAddressWidth16
            // 
            this.m_rbAddressWidth16.AutoSize = true;
            this.m_rbAddressWidth16.Location = new System.Drawing.Point(71, 20);
            this.m_rbAddressWidth16.Name = "m_rbAddressWidth16";
            this.m_rbAddressWidth16.Size = new System.Drawing.Size(37, 17);
            this.m_rbAddressWidth16.TabIndex = 1;
            this.m_rbAddressWidth16.TabStop = true;
            this.m_rbAddressWidth16.Text = "16";
            this.m_rbAddressWidth16.UseVisualStyleBackColor = true;
            this.m_rbAddressWidth16.CheckedChanged += new System.EventHandler(this.addressWidth_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.m_rbDataWidth8);
            this.groupBox3.Controls.Add(this.m_rbDataWidth16);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point(232, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(224, 45);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Data Width (bits)";
            // 
            // m_rbDataWidth8
            // 
            this.m_rbDataWidth8.AutoSize = true;
            this.m_rbDataWidth8.Location = new System.Drawing.Point(71, 20);
            this.m_rbDataWidth8.Name = "m_rbDataWidth8";
            this.m_rbDataWidth8.Size = new System.Drawing.Size(31, 17);
            this.m_rbDataWidth8.TabIndex = 1;
            this.m_rbDataWidth8.TabStop = true;
            this.m_rbDataWidth8.Text = "8";
            this.m_rbDataWidth8.UseVisualStyleBackColor = true;
            this.m_rbDataWidth8.CheckedChanged += new System.EventHandler(this.dataWidth_CheckedChanged);
            // 
            // m_rbDataWidth16
            // 
            this.m_rbDataWidth16.AutoSize = true;
            this.m_rbDataWidth16.Location = new System.Drawing.Point(10, 20);
            this.m_rbDataWidth16.Name = "m_rbDataWidth16";
            this.m_rbDataWidth16.Size = new System.Drawing.Size(37, 17);
            this.m_rbDataWidth16.TabIndex = 0;
            this.m_rbDataWidth16.TabStop = true;
            this.m_rbDataWidth16.Text = "16";
            this.m_rbDataWidth16.UseVisualStyleBackColor = true;
            this.m_rbDataWidth16.CheckedChanged += new System.EventHandler(this.dataWidth_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.m_lblReadCycleLengthCalc);
            this.groupBox5.Controls.Add(this.m_lblWriteCycleLengthCalc);
            this.groupBox5.Controls.Add(this.m_lblOenPulseWidthCalc);
            this.groupBox5.Controls.Add(this.m_lblOenPulseWidth);
            this.groupBox5.Controls.Add(this.m_lblWenPulseWidthCalc);
            this.groupBox5.Controls.Add(this.m_lblWenPulseWidth);
            this.groupBox5.Controls.Add(this.m_lblReadCycleLength);
            this.groupBox5.Controls.Add(this.m_lblWriteCycleLength);
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox5.Location = new System.Drawing.Point(0, 176);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(456, 63);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Memory Transactions";
            // 
            // m_lblReadCycleLengthCalc
            // 
            this.m_lblReadCycleLengthCalc.AutoSize = true;
            this.m_lblReadCycleLengthCalc.Location = new System.Drawing.Point(103, 40);
            this.m_lblReadCycleLengthCalc.Name = "m_lblReadCycleLengthCalc";
            this.m_lblReadCycleLengthCalc.Size = new System.Drawing.Size(82, 13);
            this.m_lblReadCycleLengthCalc.TabIndex = 26;
            this.m_lblReadCycleLengthCalc.Text = "90 ns (3 Cycles)";
            // 
            // m_lblWriteCycleLengthCalc
            // 
            this.m_lblWriteCycleLengthCalc.AutoSize = true;
            this.m_lblWriteCycleLengthCalc.Location = new System.Drawing.Point(103, 20);
            this.m_lblWriteCycleLengthCalc.Name = "m_lblWriteCycleLengthCalc";
            this.m_lblWriteCycleLengthCalc.Size = new System.Drawing.Size(88, 13);
            this.m_lblWriteCycleLengthCalc.TabIndex = 25;
            this.m_lblWriteCycleLengthCalc.Text = "120 ns (4 Cycles)";
            // 
            // m_lblOenPulseWidthCalc
            // 
            this.m_lblOenPulseWidthCalc.AutoSize = true;
            this.m_lblOenPulseWidthCalc.Location = new System.Drawing.Point(402, 40);
            this.m_lblOenPulseWidthCalc.Name = "m_lblOenPulseWidthCalc";
            this.m_lblOenPulseWidthCalc.Size = new System.Drawing.Size(33, 13);
            this.m_lblOenPulseWidthCalc.TabIndex = 24;
            this.m_lblOenPulseWidthCalc.Text = "60 ns";
            // 
            // m_lblOenPulseWidth
            // 
            this.m_lblOenPulseWidth.AutoSize = true;
            this.m_lblOenPulseWidth.Location = new System.Drawing.Point(252, 40);
            this.m_lblOenPulseWidth.Name = "m_lblOenPulseWidth";
            this.m_lblOenPulseWidth.Size = new System.Drawing.Size(150, 13);
            this.m_lblOenPulseWidth.TabIndex = 23;
            this.m_lblOenPulseWidth.Text = "Signal oen Pulse Width (Toel):";
            // 
            // m_lblWenPulseWidthCalc
            // 
            this.m_lblWenPulseWidthCalc.AutoSize = true;
            this.m_lblWenPulseWidthCalc.Location = new System.Drawing.Point(402, 20);
            this.m_lblWenPulseWidthCalc.Name = "m_lblWenPulseWidthCalc";
            this.m_lblWenPulseWidthCalc.Size = new System.Drawing.Size(28, 13);
            this.m_lblWenPulseWidthCalc.TabIndex = 22;
            this.m_lblWenPulseWidthCalc.Text = "X ns";
            // 
            // m_lblWenPulseWidth
            // 
            this.m_lblWenPulseWidth.AutoSize = true;
            this.m_lblWenPulseWidth.Location = new System.Drawing.Point(252, 20);
            this.m_lblWenPulseWidth.Name = "m_lblWenPulseWidth";
            this.m_lblWenPulseWidth.Size = new System.Drawing.Size(154, 13);
            this.m_lblWenPulseWidth.TabIndex = 21;
            this.m_lblWenPulseWidth.Text = "Signal wen Pulse Width (Twel):";
            // 
            // m_lblReadCycleLength
            // 
            this.m_lblReadCycleLength.AutoSize = true;
            this.m_lblReadCycleLength.Location = new System.Drawing.Point(6, 40);
            this.m_lblReadCycleLength.Name = "m_lblReadCycleLength";
            this.m_lblReadCycleLength.Size = new System.Drawing.Size(101, 13);
            this.m_lblReadCycleLength.TabIndex = 1;
            this.m_lblReadCycleLength.Text = "Read Cycle Length:";
            // 
            // m_lblWriteCycleLength
            // 
            this.m_lblWriteCycleLength.AutoSize = true;
            this.m_lblWriteCycleLength.Location = new System.Drawing.Point(6, 20);
            this.m_lblWriteCycleLength.Name = "m_lblWriteCycleLength";
            this.m_lblWriteCycleLength.Size = new System.Drawing.Size(100, 13);
            this.m_lblWriteCycleLength.TabIndex = 0;
            this.m_lblWriteCycleLength.Text = "Write Cycle Length:";
            // 
            // m_lblBusClockFrequency
            // 
            this.m_lblBusClockFrequency.AutoSize = true;
            this.m_lblBusClockFrequency.Location = new System.Drawing.Point(252, 153);
            this.m_lblBusClockFrequency.Name = "m_lblBusClockFrequency";
            this.m_lblBusClockFrequency.Size = new System.Drawing.Size(111, 13);
            this.m_lblBusClockFrequency.TabIndex = 15;
            this.m_lblBusClockFrequency.Text = "Bus Clock Frequency:";
            // 
            // m_lblBusClockFrequencyCalc
            // 
            this.m_lblBusClockFrequencyCalc.AutoSize = true;
            this.m_lblBusClockFrequencyCalc.Location = new System.Drawing.Point(359, 153);
            this.m_lblBusClockFrequencyCalc.Name = "m_lblBusClockFrequencyCalc";
            this.m_lblBusClockFrequencyCalc.Size = new System.Drawing.Size(35, 13);
            this.m_lblBusClockFrequencyCalc.TabIndex = 16;
            this.m_lblBusClockFrequencyCalc.Text = "label5";
            // 
            // CyEMIFBasicTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.m_lblBusClockFrequencyCalc);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.m_lblBusClockFrequency);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_numExtMemorySpeed);
            this.Controls.Add(this.m_lblExtMemorySpeed);
            this.Name = "CyEMIFBasicTab";
            this.Size = new System.Drawing.Size(562, 277);
            ((System.ComponentModel.ISupportInitialize)(this.m_numExtMemorySpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblExtMemorySpeed;
        private System.Windows.Forms.NumericUpDown m_numExtMemorySpeed;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton m_rbDataWidth8;
        private System.Windows.Forms.RadioButton m_rbDataWidth16;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton m_rbAddressWidth24;
        private System.Windows.Forms.RadioButton m_rbAddressWidth8;
        private System.Windows.Forms.RadioButton m_rbAddressWidth16;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton m_rbCustom;
        private System.Windows.Forms.RadioButton m_rbSynchronous;
        private System.Windows.Forms.RadioButton m_rbAsynchronous;
        private System.Windows.Forms.Label lblExternalMemoryType;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label m_lblBusClockFrequency;
        private System.Windows.Forms.Label m_lblReadCycleLength;
        private System.Windows.Forms.Label m_lblWriteCycleLength;
        private System.Windows.Forms.Label m_lblOenPulseWidthCalc;
        private System.Windows.Forms.Label m_lblOenPulseWidth;
        private System.Windows.Forms.Label m_lblWenPulseWidthCalc;
        private System.Windows.Forms.Label m_lblWenPulseWidth;
        private System.Windows.Forms.Label m_lblBusClockFrequencyCalc;
        private System.Windows.Forms.Label m_lblReadCycleLengthCalc;
        private System.Windows.Forms.Label m_lblWriteCycleLengthCalc;
    }
}
