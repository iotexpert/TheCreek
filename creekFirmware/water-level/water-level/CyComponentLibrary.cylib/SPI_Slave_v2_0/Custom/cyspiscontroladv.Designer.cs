namespace SPI_Slave_v2_0
{
    partial class CySPIMControlAdv
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
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.numTXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.numRXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.panelTop = new System.Windows.Forms.Panel();
            this.gbClocks = new System.Windows.Forms.GroupBox();
            this.rbExternalClock = new System.Windows.Forms.RadioButton();
            this.rbInternalClock = new System.Windows.Forms.RadioButton();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.gbBufferSizes = new System.Windows.Forms.GroupBox();
            this.lblTXBufferSize = new System.Windows.Forms.Label();
            this.lblRXBufferSize = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbxMultiSlaveEnable = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbxEnableRXInternalInterrupt = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbxEnableTXInternalInterrupt = new System.Windows.Forms.CheckBox();
            this.chbxIntOnRXNotEmpty = new System.Windows.Forms.CheckBox();
            this.chbxIntOnByteComplete = new System.Windows.Forms.CheckBox();
            this.chbxIntOnRXEmpty = new System.Windows.Forms.CheckBox();
            this.chbxIntOnRXOverrun = new System.Windows.Forms.CheckBox();
            this.chbxIntOnSPIDone = new System.Windows.Forms.CheckBox();
            this.chbxIntOnTXNotFull = new System.Windows.Forms.CheckBox();
            this.chbxIntOnTXEmpty = new System.Windows.Forms.CheckBox();
            this.chbxIntOnRXFull = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTXBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRXBufferSize)).BeginInit();
            this.panelTop.SuspendLayout();
            this.gbClocks.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            this.gbBufferSizes.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // numTXBufferSize
            // 
            this.numTXBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ep_Errors.SetIconAlignment(this.numTXBufferSize, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.numTXBufferSize.Location = new System.Drawing.Point(151, 44);
            this.numTXBufferSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numTXBufferSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTXBufferSize.Name = "numTXBufferSize";
            this.numTXBufferSize.Size = new System.Drawing.Size(249, 20);
            this.numTXBufferSize.TabIndex = 1;
            this.numTXBufferSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTXBufferSize.Validating += new System.ComponentModel.CancelEventHandler(this.numUpDown_Validating);
            // 
            // numRXBufferSize
            // 
            this.numRXBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ep_Errors.SetIconAlignment(this.numRXBufferSize, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.numRXBufferSize.Location = new System.Drawing.Point(151, 17);
            this.numRXBufferSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numRXBufferSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRXBufferSize.Name = "numRXBufferSize";
            this.numRXBufferSize.Size = new System.Drawing.Size(249, 20);
            this.numRXBufferSize.TabIndex = 0;
            this.numRXBufferSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRXBufferSize.Validating += new System.ComponentModel.CancelEventHandler(this.numUpDown_Validating);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.Controls.Add(this.gbClocks);
            this.panelTop.Location = new System.Drawing.Point(6, 6);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(427, 51);
            this.panelTop.TabIndex = 3;
            // 
            // gbClocks
            // 
            this.gbClocks.Controls.Add(this.rbExternalClock);
            this.gbClocks.Controls.Add(this.rbInternalClock);
            this.gbClocks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbClocks.Location = new System.Drawing.Point(0, 0);
            this.gbClocks.Margin = new System.Windows.Forms.Padding(2);
            this.gbClocks.Name = "gbClocks";
            this.gbClocks.Padding = new System.Windows.Forms.Padding(2);
            this.gbClocks.Size = new System.Drawing.Size(427, 51);
            this.gbClocks.TabIndex = 0;
            this.gbClocks.TabStop = false;
            this.gbClocks.Text = "Clock Selection:";
            // 
            // rbExternalClock
            // 
            this.rbExternalClock.AutoSize = true;
            this.rbExternalClock.Location = new System.Drawing.Point(138, 19);
            this.rbExternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.rbExternalClock.Name = "rbExternalClock";
            this.rbExternalClock.Size = new System.Drawing.Size(93, 17);
            this.rbExternalClock.TabIndex = 1;
            this.rbExternalClock.Text = "External Clock";
            this.rbExternalClock.UseVisualStyleBackColor = true;
            // 
            // rbInternalClock
            // 
            this.rbInternalClock.AutoSize = true;
            this.rbInternalClock.Location = new System.Drawing.Point(13, 19);
            this.rbInternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.rbInternalClock.Name = "rbInternalClock";
            this.rbInternalClock.Size = new System.Drawing.Size(90, 17);
            this.rbInternalClock.TabIndex = 0;
            this.rbInternalClock.Text = "Internal Clock";
            this.rbInternalClock.UseVisualStyleBackColor = true;
            this.rbInternalClock.CheckedChanged += new System.EventHandler(this.rbInternalClock_CheckedChanged);
            // 
            // panelMiddle
            // 
            this.panelMiddle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMiddle.Controls.Add(this.gbBufferSizes);
            this.panelMiddle.Location = new System.Drawing.Point(6, 60);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(427, 81);
            this.panelMiddle.TabIndex = 4;
            // 
            // gbBufferSizes
            // 
            this.gbBufferSizes.Controls.Add(this.numTXBufferSize);
            this.gbBufferSizes.Controls.Add(this.numRXBufferSize);
            this.gbBufferSizes.Controls.Add(this.lblTXBufferSize);
            this.gbBufferSizes.Controls.Add(this.lblRXBufferSize);
            this.gbBufferSizes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBufferSizes.Location = new System.Drawing.Point(0, 0);
            this.gbBufferSizes.Name = "gbBufferSizes";
            this.gbBufferSizes.Size = new System.Drawing.Size(427, 81);
            this.gbBufferSizes.TabIndex = 1;
            this.gbBufferSizes.TabStop = false;
            this.gbBufferSizes.Text = "Buffer Sizes:";
            // 
            // lblTXBufferSize
            // 
            this.lblTXBufferSize.AutoSize = true;
            this.lblTXBufferSize.Location = new System.Drawing.Point(28, 48);
            this.lblTXBufferSize.Name = "lblTXBufferSize";
            this.lblTXBufferSize.Size = new System.Drawing.Size(112, 13);
            this.lblTXBufferSize.TabIndex = 147;
            this.lblTXBufferSize.Text = "TX Buffer Size (bytes):";
            this.lblTXBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRXBufferSize
            // 
            this.lblRXBufferSize.AutoSize = true;
            this.lblRXBufferSize.Location = new System.Drawing.Point(27, 21);
            this.lblRXBufferSize.Name = "lblRXBufferSize";
            this.lblRXBufferSize.Size = new System.Drawing.Size(113, 13);
            this.lblRXBufferSize.TabIndex = 144;
            this.lblRXBufferSize.Text = "RX Buffer Size (bytes):";
            this.lblRXBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.Controls.Add(this.groupBox1);
            this.panelBottom.Location = new System.Drawing.Point(6, 144);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(427, 196);
            this.panelBottom.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.chbxIntOnRXFull);
            this.groupBox1.Controls.Add(this.chbxMultiSlaveEnable);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.chbxEnableRXInternalInterrupt);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.chbxEnableTXInternalInterrupt);
            this.groupBox1.Controls.Add(this.chbxIntOnRXNotEmpty);
            this.groupBox1.Controls.Add(this.chbxIntOnByteComplete);
            this.groupBox1.Controls.Add(this.chbxIntOnRXEmpty);
            this.groupBox1.Controls.Add(this.chbxIntOnRXOverrun);
            this.groupBox1.Controls.Add(this.chbxIntOnSPIDone);
            this.groupBox1.Controls.Add(this.chbxIntOnTXNotFull);
            this.groupBox1.Controls.Add(this.chbxIntOnTXEmpty);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(427, 196);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interrupts:";
            // 
            // chbxMultiSlaveEnable
            // 
            this.chbxMultiSlaveEnable.AutoSize = true;
            this.chbxMultiSlaveEnable.Location = new System.Drawing.Point(8, 151);
            this.chbxMultiSlaveEnable.Name = "chbxMultiSlaveEnable";
            this.chbxMultiSlaveEnable.Size = new System.Drawing.Size(143, 17);
            this.chbxMultiSlaveEnable.TabIndex = 9;
            this.chbxMultiSlaveEnable.Text = "Enable Multi-Slave mode";
            this.chbxMultiSlaveEnable.UseVisualStyleBackColor = true;
            this.chbxMultiSlaveEnable.CheckedChanged += new System.EventHandler(this.chbxMultiSlaveEnable_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(6, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(415, 2);
            this.groupBox3.TabIndex = 66;
            this.groupBox3.TabStop = false;
            // 
            // chbxEnableRXInternalInterrupt
            // 
            this.chbxEnableRXInternalInterrupt.AutoSize = true;
            this.chbxEnableRXInternalInterrupt.Location = new System.Drawing.Point(237, 19);
            this.chbxEnableRXInternalInterrupt.Name = "chbxEnableRXInternalInterrupt";
            this.chbxEnableRXInternalInterrupt.Size = new System.Drawing.Size(157, 17);
            this.chbxEnableRXInternalInterrupt.TabIndex = 5;
            this.chbxEnableRXInternalInterrupt.Text = "Enable RX Internal Interrupt";
            this.chbxEnableRXInternalInterrupt.UseVisualStyleBackColor = true;
            this.chbxEnableRXInternalInterrupt.CheckedChanged += new System.EventHandler(this.chbxEnableRXInternalInterrupt_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(6, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(415, 2);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            // 
            // chbxEnableTXInternalInterrupt
            // 
            this.chbxEnableTXInternalInterrupt.AutoSize = true;
            this.chbxEnableTXInternalInterrupt.Location = new System.Drawing.Point(8, 19);
            this.chbxEnableTXInternalInterrupt.Name = "chbxEnableTXInternalInterrupt";
            this.chbxEnableTXInternalInterrupt.Size = new System.Drawing.Size(156, 17);
            this.chbxEnableTXInternalInterrupt.TabIndex = 0;
            this.chbxEnableTXInternalInterrupt.Text = "Enable TX Internal Interrupt";
            this.chbxEnableTXInternalInterrupt.UseVisualStyleBackColor = true;
            this.chbxEnableTXInternalInterrupt.CheckedChanged += new System.EventHandler(this.chbxEnableTXInternalInterrupt_CheckedChanged);
            // 
            // chbxIntOnRXNotEmpty
            // 
            this.chbxIntOnRXNotEmpty.AutoSize = true;
            this.chbxIntOnRXNotEmpty.Location = new System.Drawing.Point(237, 73);
            this.chbxIntOnRXNotEmpty.Name = "chbxIntOnRXNotEmpty";
            this.chbxIntOnRXNotEmpty.Size = new System.Drawing.Size(178, 17);
            this.chbxIntOnRXNotEmpty.TabIndex = 7;
            this.chbxIntOnRXNotEmpty.Text = "Interrupt On RX FIFO Not Empty";
            this.chbxIntOnRXNotEmpty.UseVisualStyleBackColor = true;
            this.chbxIntOnRXNotEmpty.CheckedChanged += new System.EventHandler(this.chbxIntOnRXNotEmpty_CheckedChanged);
            // 
            // chbxIntOnByteComplete
            // 
            this.chbxIntOnByteComplete.AutoSize = true;
            this.chbxIntOnByteComplete.Location = new System.Drawing.Point(8, 119);
            this.chbxIntOnByteComplete.Name = "chbxIntOnByteComplete";
            this.chbxIntOnByteComplete.Size = new System.Drawing.Size(195, 17);
            this.chbxIntOnByteComplete.TabIndex = 4;
            this.chbxIntOnByteComplete.Text = "Interrupt On Byte Transfer Complete";
            this.chbxIntOnByteComplete.UseVisualStyleBackColor = true;
            this.chbxIntOnByteComplete.CheckedChanged += new System.EventHandler(this.chbxIntOnByteComplete_CheckedChanged);
            // 
            // chbxIntOnRXEmpty
            // 
            this.chbxIntOnRXEmpty.AutoSize = true;
            this.chbxIntOnRXEmpty.Location = new System.Drawing.Point(237, 50);
            this.chbxIntOnRXEmpty.Name = "chbxIntOnRXEmpty";
            this.chbxIntOnRXEmpty.Size = new System.Drawing.Size(158, 17);
            this.chbxIntOnRXEmpty.TabIndex = 6;
            this.chbxIntOnRXEmpty.Text = "Interrupt On RX FIFO Empty";
            this.chbxIntOnRXEmpty.UseVisualStyleBackColor = true;
            this.chbxIntOnRXEmpty.CheckedChanged += new System.EventHandler(this.chbxIntOnRXEmpty_CheckedChanged);
            // 
            // chbxIntOnRXOverrun
            // 
            this.chbxIntOnRXOverrun.AutoSize = true;
            this.chbxIntOnRXOverrun.Location = new System.Drawing.Point(237, 96);
            this.chbxIntOnRXOverrun.Name = "chbxIntOnRXOverrun";
            this.chbxIntOnRXOverrun.Size = new System.Drawing.Size(167, 17);
            this.chbxIntOnRXOverrun.TabIndex = 8;
            this.chbxIntOnRXOverrun.Text = "Interrupt On RX FIFO Overrun";
            this.chbxIntOnRXOverrun.UseVisualStyleBackColor = true;
            this.chbxIntOnRXOverrun.CheckedChanged += new System.EventHandler(this.chbxIntOnRXOverrun_CheckedChanged);
            // 
            // chbxIntOnSPIDone
            // 
            this.chbxIntOnSPIDone.AutoSize = true;
            this.chbxIntOnSPIDone.Location = new System.Drawing.Point(8, 50);
            this.chbxIntOnSPIDone.Name = "chbxIntOnSPIDone";
            this.chbxIntOnSPIDone.Size = new System.Drawing.Size(131, 17);
            this.chbxIntOnSPIDone.TabIndex = 1;
            this.chbxIntOnSPIDone.Text = "Interrupt On SPI Done";
            this.chbxIntOnSPIDone.UseVisualStyleBackColor = true;
            this.chbxIntOnSPIDone.CheckedChanged += new System.EventHandler(this.chbxIntOnSPIDone_CheckedChanged);
            // 
            // chbxIntOnTXNotFull
            // 
            this.chbxIntOnTXNotFull.AutoSize = true;
            this.chbxIntOnTXNotFull.Location = new System.Drawing.Point(8, 73);
            this.chbxIntOnTXNotFull.Name = "chbxIntOnTXNotFull";
            this.chbxIntOnTXNotFull.Size = new System.Drawing.Size(164, 17);
            this.chbxIntOnTXNotFull.TabIndex = 2;
            this.chbxIntOnTXNotFull.Text = "Interrupt On TX FIFO Not Full";
            this.chbxIntOnTXNotFull.UseVisualStyleBackColor = true;
            this.chbxIntOnTXNotFull.CheckedChanged += new System.EventHandler(this.chbxIntOnTXNotFull_CheckedChanged);
            // 
            // chbxIntOnTXEmpty
            // 
            this.chbxIntOnTXEmpty.AutoSize = true;
            this.chbxIntOnTXEmpty.Location = new System.Drawing.Point(8, 96);
            this.chbxIntOnTXEmpty.Name = "chbxIntOnTXEmpty";
            this.chbxIntOnTXEmpty.Size = new System.Drawing.Size(157, 17);
            this.chbxIntOnTXEmpty.TabIndex = 3;
            this.chbxIntOnTXEmpty.Text = "Interrupt On TX FIFO Empty";
            this.chbxIntOnTXEmpty.UseVisualStyleBackColor = true;
            this.chbxIntOnTXEmpty.CheckedChanged += new System.EventHandler(this.chbxIntOnTXEmpty_CheckedChanged);
            // 
            // chbxIntOnRXFull
            // 
            this.chbxIntOnRXFull.AutoSize = true;
            this.chbxIntOnRXFull.Location = new System.Drawing.Point(237, 119);
            this.chbxIntOnRXFull.Name = "chbxIntOnRXFull";
            this.chbxIntOnRXFull.Size = new System.Drawing.Size(145, 17);
            this.chbxIntOnRXFull.TabIndex = 67;
            this.chbxIntOnRXFull.Text = "Interrupt On RX FIFO Full";
            this.chbxIntOnRXFull.UseVisualStyleBackColor = true;
            this.chbxIntOnRXFull.CheckedChanged += new System.EventHandler(this.chbxIntOnRXFull_CheckedChanged);
            // 
            // CySPIMControlAdv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelTop);
            this.Name = "CySPIMControlAdv";
            this.Size = new System.Drawing.Size(443, 343);
            this.Load += new System.EventHandler(this.CySPIMControlAdv_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTXBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRXBufferSize)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.gbClocks.ResumeLayout(false);
            this.gbClocks.PerformLayout();
            this.panelMiddle.ResumeLayout(false);
            this.gbBufferSizes.ResumeLayout(false);
            this.gbBufferSizes.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider ep_Errors;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox gbClocks;
        private System.Windows.Forms.RadioButton rbExternalClock;
        private System.Windows.Forms.RadioButton rbInternalClock;
        private System.Windows.Forms.GroupBox gbBufferSizes;
        private System.Windows.Forms.NumericUpDown numTXBufferSize;
        private System.Windows.Forms.NumericUpDown numRXBufferSize;
        private System.Windows.Forms.Label lblTXBufferSize;
        private System.Windows.Forms.Label lblRXBufferSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbxEnableTXInternalInterrupt;
        private System.Windows.Forms.CheckBox chbxIntOnRXNotEmpty;
        private System.Windows.Forms.CheckBox chbxIntOnByteComplete;
        private System.Windows.Forms.CheckBox chbxIntOnRXEmpty;
        private System.Windows.Forms.CheckBox chbxIntOnRXOverrun;
        private System.Windows.Forms.CheckBox chbxIntOnSPIDone;
        private System.Windows.Forms.CheckBox chbxIntOnTXNotFull;
        private System.Windows.Forms.CheckBox chbxIntOnTXEmpty;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chbxEnableRXInternalInterrupt;
        private System.Windows.Forms.CheckBox chbxMultiSlaveEnable;
        private System.Windows.Forms.CheckBox chbxIntOnRXFull;
    }
}
