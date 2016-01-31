namespace SPI_Master_v1_20
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_chbxEnableInternalInterrupt = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnRXNotEmpty = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnByteComp = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnRXEmpty = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnRXOver = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnSPIDone = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnTXNotFull = new System.Windows.Forms.CheckBox();
            this.m_chbxIntOnTXFull = new System.Windows.Forms.CheckBox();
            this.gbClocks = new System.Windows.Forms.GroupBox();
            this.m_rbExternalClock = new System.Windows.Forms.RadioButton();
            this.m_rbInternalClock = new System.Windows.Forms.RadioButton();
            this.m_gbBufferSizes = new System.Windows.Forms.GroupBox();
            this.m_numTXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.m_numRXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.m_lblTXBufferSize = new System.Windows.Forms.Label();
            this.m_lblRXBufferSize = new System.Windows.Forms.Label();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.gbClocks.SuspendLayout();
            this.m_gbBufferSizes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numTXBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.m_chbxEnableInternalInterrupt);
            this.groupBox1.Controls.Add(this.m_chbxIntOnRXNotEmpty);
            this.groupBox1.Controls.Add(this.m_chbxIntOnByteComp);
            this.groupBox1.Controls.Add(this.m_chbxIntOnRXEmpty);
            this.groupBox1.Controls.Add(this.m_chbxIntOnRXOver);
            this.groupBox1.Controls.Add(this.m_chbxIntOnSPIDone);
            this.groupBox1.Controls.Add(this.m_chbxIntOnTXNotFull);
            this.groupBox1.Controls.Add(this.m_chbxIntOnTXFull);
            this.groupBox1.Location = new System.Drawing.Point(14, 143);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 194);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interrupts:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(6, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(393, 2);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            // 
            // m_chbxEnableInternalInterrupt
            // 
            this.m_chbxEnableInternalInterrupt.AutoSize = true;
            this.m_chbxEnableInternalInterrupt.Location = new System.Drawing.Point(8, 19);
            this.m_chbxEnableInternalInterrupt.Name = "m_chbxEnableInternalInterrupt";
            this.m_chbxEnableInternalInterrupt.Size = new System.Drawing.Size(139, 17);
            this.m_chbxEnableInternalInterrupt.TabIndex = 0;
            this.m_chbxEnableInternalInterrupt.Text = "Enable Internal Interrupt";
            this.m_chbxEnableInternalInterrupt.UseVisualStyleBackColor = true;
            this.m_chbxEnableInternalInterrupt.CheckedChanged += new System.EventHandler(this.m_chbxEnableInternalInterrupt_CheckedChanged);
            // 
            // m_chbxIntOnRXNotEmpty
            // 
            this.m_chbxIntOnRXNotEmpty.AutoSize = true;
            this.m_chbxIntOnRXNotEmpty.Location = new System.Drawing.Point(8, 150);
            this.m_chbxIntOnRXNotEmpty.Name = "m_chbxIntOnRXNotEmpty";
            this.m_chbxIntOnRXNotEmpty.Size = new System.Drawing.Size(178, 17);
            this.m_chbxIntOnRXNotEmpty.TabIndex = 6;
            this.m_chbxIntOnRXNotEmpty.Text = "Interrupt On RX FIFO Not Empty";
            this.m_chbxIntOnRXNotEmpty.UseVisualStyleBackColor = true;
            this.m_chbxIntOnRXNotEmpty.CheckedChanged += new System.EventHandler(this.m_chbxIntOnRXNotEmpty_CheckedChanged);
            // 
            // m_chbxIntOnByteComp
            // 
            this.m_chbxIntOnByteComp.AutoSize = true;
            this.m_chbxIntOnByteComp.Location = new System.Drawing.Point(8, 170);
            this.m_chbxIntOnByteComp.Name = "m_chbxIntOnByteComp";
            this.m_chbxIntOnByteComp.Size = new System.Drawing.Size(195, 17);
            this.m_chbxIntOnByteComp.TabIndex = 7;
            this.m_chbxIntOnByteComp.Text = "Interrupt On Byte Transfer Complete";
            this.m_chbxIntOnByteComp.UseVisualStyleBackColor = true;
            this.m_chbxIntOnByteComp.CheckedChanged += new System.EventHandler(this.m_chbxIntOnByteComp_CheckedChanged);
            // 
            // m_chbxIntOnRXEmpty
            // 
            this.m_chbxIntOnRXEmpty.AutoSize = true;
            this.m_chbxIntOnRXEmpty.Location = new System.Drawing.Point(8, 130);
            this.m_chbxIntOnRXEmpty.Name = "m_chbxIntOnRXEmpty";
            this.m_chbxIntOnRXEmpty.Size = new System.Drawing.Size(145, 17);
            this.m_chbxIntOnRXEmpty.TabIndex = 5;
            this.m_chbxIntOnRXEmpty.Text = "Interrupt On RX FIFO Full";
            this.m_chbxIntOnRXEmpty.UseVisualStyleBackColor = true;
            this.m_chbxIntOnRXEmpty.CheckedChanged += new System.EventHandler(this.m_chbxIntOnRXEmpty_CheckedChanged);
            // 
            // m_chbxIntOnRXOver
            // 
            this.m_chbxIntOnRXOver.AutoSize = true;
            this.m_chbxIntOnRXOver.Location = new System.Drawing.Point(8, 110);
            this.m_chbxIntOnRXOver.Name = "m_chbxIntOnRXOver";
            this.m_chbxIntOnRXOver.Size = new System.Drawing.Size(167, 17);
            this.m_chbxIntOnRXOver.TabIndex = 4;
            this.m_chbxIntOnRXOver.Text = "Interrupt On RX FIFO Overrun";
            this.m_chbxIntOnRXOver.UseVisualStyleBackColor = true;
            this.m_chbxIntOnRXOver.CheckedChanged += new System.EventHandler(this.m_chbxIntOnRXOver_CheckedChanged);
            // 
            // m_chbxIntOnSPIDone
            // 
            this.m_chbxIntOnSPIDone.AutoSize = true;
            this.m_chbxIntOnSPIDone.Location = new System.Drawing.Point(8, 90);
            this.m_chbxIntOnSPIDone.Name = "m_chbxIntOnSPIDone";
            this.m_chbxIntOnSPIDone.Size = new System.Drawing.Size(131, 17);
            this.m_chbxIntOnSPIDone.TabIndex = 3;
            this.m_chbxIntOnSPIDone.Text = "Interrupt On SPI Done";
            this.m_chbxIntOnSPIDone.UseVisualStyleBackColor = true;
            this.m_chbxIntOnSPIDone.CheckedChanged += new System.EventHandler(this.m_chbxIntOnSPIDone_CheckedChanged);
            // 
            // m_chbxIntOnTXNotFull
            // 
            this.m_chbxIntOnTXNotFull.AutoSize = true;
            this.m_chbxIntOnTXNotFull.Location = new System.Drawing.Point(8, 70);
            this.m_chbxIntOnTXNotFull.Name = "m_chbxIntOnTXNotFull";
            this.m_chbxIntOnTXNotFull.Size = new System.Drawing.Size(164, 17);
            this.m_chbxIntOnTXNotFull.TabIndex = 2;
            this.m_chbxIntOnTXNotFull.Text = "Interrupt On TX FIFO Not Full";
            this.m_chbxIntOnTXNotFull.UseVisualStyleBackColor = true;
            this.m_chbxIntOnTXNotFull.CheckedChanged += new System.EventHandler(this.m_chbxIntOnTXNotFull_CheckedChanged);
            // 
            // m_chbxIntOnTXFull
            // 
            this.m_chbxIntOnTXFull.AutoSize = true;
            this.m_chbxIntOnTXFull.Location = new System.Drawing.Point(8, 50);
            this.m_chbxIntOnTXFull.Name = "m_chbxIntOnTXFull";
            this.m_chbxIntOnTXFull.Size = new System.Drawing.Size(157, 17);
            this.m_chbxIntOnTXFull.TabIndex = 1;
            this.m_chbxIntOnTXFull.Text = "Interrupt On TX FIFO Empty";
            this.m_chbxIntOnTXFull.UseVisualStyleBackColor = true;
            this.m_chbxIntOnTXFull.CheckedChanged += new System.EventHandler(this.m_chbxIntOnTXFull_CheckedChanged);
            // 
            // gbClocks
            // 
            this.gbClocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbClocks.Controls.Add(this.m_rbExternalClock);
            this.gbClocks.Controls.Add(this.m_rbInternalClock);
            this.gbClocks.Location = new System.Drawing.Point(14, 15);
            this.gbClocks.Margin = new System.Windows.Forms.Padding(2);
            this.gbClocks.Name = "gbClocks";
            this.gbClocks.Padding = new System.Windows.Forms.Padding(2);
            this.gbClocks.Size = new System.Drawing.Size(405, 42);
            this.gbClocks.TabIndex = 0;
            this.gbClocks.TabStop = false;
            this.gbClocks.Text = "Clock Selection:";
            // 
            // m_rbExternalClock
            // 
            this.m_rbExternalClock.AutoSize = true;
            this.m_rbExternalClock.Location = new System.Drawing.Point(129, 17);
            this.m_rbExternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.m_rbExternalClock.Name = "m_rbExternalClock";
            this.m_rbExternalClock.Size = new System.Drawing.Size(93, 17);
            this.m_rbExternalClock.TabIndex = 1;
            this.m_rbExternalClock.Text = "External Clock";
            this.m_rbExternalClock.UseVisualStyleBackColor = true;
            this.m_rbExternalClock.CheckedChanged += new System.EventHandler(this.m_rbExternalClock_CheckedChanged);
            // 
            // m_rbInternalClock
            // 
            this.m_rbInternalClock.AutoSize = true;
            this.m_rbInternalClock.Location = new System.Drawing.Point(4, 17);
            this.m_rbInternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.m_rbInternalClock.Name = "m_rbInternalClock";
            this.m_rbInternalClock.Size = new System.Drawing.Size(90, 17);
            this.m_rbInternalClock.TabIndex = 0;
            this.m_rbInternalClock.Text = "Internal Clock";
            this.m_rbInternalClock.UseVisualStyleBackColor = true;
            this.m_rbInternalClock.CheckedChanged += new System.EventHandler(this.m_rbInternalClock_CheckedChanged);
            // 
            // m_gbBufferSizes
            // 
            this.m_gbBufferSizes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gbBufferSizes.Controls.Add(this.m_numTXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_numRXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_lblTXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_lblRXBufferSize);
            this.m_gbBufferSizes.Location = new System.Drawing.Point(14, 62);
            this.m_gbBufferSizes.Name = "m_gbBufferSizes";
            this.m_gbBufferSizes.Size = new System.Drawing.Size(405, 75);
            this.m_gbBufferSizes.TabIndex = 1;
            this.m_gbBufferSizes.TabStop = false;
            this.m_gbBufferSizes.Text = "Buffer Sizes:";
            // 
            // m_numTXBufferSize
            // 
            this.m_numTXBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numTXBufferSize.Location = new System.Drawing.Point(151, 40);
            this.m_numTXBufferSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numTXBufferSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numTXBufferSize.Name = "m_numTXBufferSize";
            this.m_numTXBufferSize.Size = new System.Drawing.Size(227, 20);
            this.m_numTXBufferSize.TabIndex = 1;
            this.m_numTXBufferSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numTXBufferSize.Validating += new System.ComponentModel.CancelEventHandler(this.bufferSize_Validating);
            // 
            // m_numRXBufferSize
            // 
            this.m_numRXBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numRXBufferSize.Location = new System.Drawing.Point(151, 13);
            this.m_numRXBufferSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numRXBufferSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numRXBufferSize.Name = "m_numRXBufferSize";
            this.m_numRXBufferSize.Size = new System.Drawing.Size(227, 20);
            this.m_numRXBufferSize.TabIndex = 0;
            this.m_numRXBufferSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numRXBufferSize.Validating += new System.ComponentModel.CancelEventHandler(this.bufferSize_Validating);
            // 
            // m_lblTXBufferSize
            // 
            this.m_lblTXBufferSize.AutoSize = true;
            this.m_lblTXBufferSize.Location = new System.Drawing.Point(32, 44);
            this.m_lblTXBufferSize.Name = "m_lblTXBufferSize";
            this.m_lblTXBufferSize.Size = new System.Drawing.Size(112, 13);
            this.m_lblTXBufferSize.TabIndex = 147;
            this.m_lblTXBufferSize.Text = "TX Buffer Size (bytes):";
            this.m_lblTXBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_lblRXBufferSize
            // 
            this.m_lblRXBufferSize.AutoSize = true;
            this.m_lblRXBufferSize.Location = new System.Drawing.Point(32, 17);
            this.m_lblRXBufferSize.Name = "m_lblRXBufferSize";
            this.m_lblRXBufferSize.Size = new System.Drawing.Size(113, 13);
            this.m_lblRXBufferSize.TabIndex = 144;
            this.m_lblRXBufferSize.Text = "RX Buffer Size (bytes):";
            this.m_lblRXBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // CySPIMControlAdv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gbBufferSizes);
            this.Controls.Add(this.gbClocks);
            this.Controls.Add(this.groupBox1);
            this.Name = "CySPIMControlAdv";
            this.Size = new System.Drawing.Size(439, 340);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbClocks.ResumeLayout(false);
            this.gbClocks.PerformLayout();
            this.m_gbBufferSizes.ResumeLayout(false);
            this.m_gbBufferSizes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numTXBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbClocks;
        private System.Windows.Forms.RadioButton m_rbExternalClock;
        private System.Windows.Forms.RadioButton m_rbInternalClock;
        private System.Windows.Forms.GroupBox m_gbBufferSizes;
        private System.Windows.Forms.Label m_lblTXBufferSize;
        private System.Windows.Forms.Label m_lblRXBufferSize;
        private System.Windows.Forms.CheckBox m_chbxIntOnTXFull;
        private System.Windows.Forms.CheckBox m_chbxIntOnByteComp;
        private System.Windows.Forms.CheckBox m_chbxIntOnRXEmpty;
        private System.Windows.Forms.CheckBox m_chbxIntOnRXOver;
        private System.Windows.Forms.CheckBox m_chbxIntOnSPIDone;
        private System.Windows.Forms.CheckBox m_chbxIntOnTXNotFull;
        private System.Windows.Forms.CheckBox m_chbxIntOnRXNotEmpty;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox m_chbxEnableInternalInterrupt;
        private System.Windows.Forms.ErrorProvider ep_Errors;
        private System.Windows.Forms.NumericUpDown m_numRXBufferSize;
        private System.Windows.Forms.NumericUpDown m_numTXBufferSize;
    }
}
