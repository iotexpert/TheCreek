namespace CyCustomizer.UART_v1_0
{
    partial class cyuartadvancedcontrol
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
            this.gbInterrupts = new System.Windows.Forms.GroupBox();
            this.m_chbxInterruptOnTXFifoNotFull = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnTXFifoFull = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnTXFifoEmpty = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnTXComplete = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_chbxInterruptOnAddressDetect = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnAddressMatch = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnOverrunError = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnBreak = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnStopError = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnParityError = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnByteReceived = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_cbTXInternalInterrupt = new System.Windows.Forms.CheckBox();
            this.m_cbRXInternalInterrupt = new System.Windows.Forms.CheckBox();
            this.gbClocks = new System.Windows.Forms.GroupBox();
            this.m_rbExternalClock = new System.Windows.Forms.RadioButton();
            this.m_rbInternalClock = new System.Windows.Forms.RadioButton();
            this.m_lblRXBufferSize = new System.Windows.Forms.Label();
            this.m_gbBufferSizes = new System.Windows.Forms.GroupBox();
            this.m_numTXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.m_lblTXBufferSize = new System.Windows.Forms.Label();
            this.m_numRXBufferSize = new System.Windows.Forms.NumericUpDown();
            this.m_gbRS485 = new System.Windows.Forms.GroupBox();
            this.m_chbxHWTxEnable = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_cbRXAddressMode = new System.Windows.Forms.ComboBox();
            this.m_numRXAddress2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.m_numRXAddress1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gbInterrupts.SuspendLayout();
            this.gbClocks.SuspendLayout();
            this.m_gbBufferSizes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numTXBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXBufferSize)).BeginInit();
            this.m_gbRS485.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXAddress2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXAddress1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbInterrupts
            // 
            this.gbInterrupts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXFifoNotFull);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXFifoFull);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXFifoEmpty);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXComplete);
            this.gbInterrupts.Controls.Add(this.groupBox3);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnAddressDetect);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnAddressMatch);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnOverrunError);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnBreak);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnStopError);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnParityError);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnByteReceived);
            this.gbInterrupts.Controls.Add(this.groupBox1);
            this.gbInterrupts.Controls.Add(this.m_cbTXInternalInterrupt);
            this.gbInterrupts.Controls.Add(this.m_cbRXInternalInterrupt);
            this.gbInterrupts.Location = new System.Drawing.Point(10, 48);
            this.gbInterrupts.Margin = new System.Windows.Forms.Padding(2);
            this.gbInterrupts.Name = "gbInterrupts";
            this.gbInterrupts.Padding = new System.Windows.Forms.Padding(2);
            this.gbInterrupts.Size = new System.Drawing.Size(271, 323);
            this.gbInterrupts.TabIndex = 143;
            this.gbInterrupts.TabStop = false;
            this.gbInterrupts.Text = "Interrupts";
            // 
            // m_chbxInterruptOnTXFifoNotFull
            // 
            this.m_chbxInterruptOnTXFifoNotFull.AutoSize = true;
            this.m_chbxInterruptOnTXFifoNotFull.Location = new System.Drawing.Point(5, 301);
            this.m_chbxInterruptOnTXFifoNotFull.Name = "m_chbxInterruptOnTXFifoNotFull";
            this.m_chbxInterruptOnTXFifoNotFull.Size = new System.Drawing.Size(128, 17);
            this.m_chbxInterruptOnTXFifoNotFull.TabIndex = 19;
            this.m_chbxInterruptOnTXFifoNotFull.Text = "TX - On FIFO Not Full";
            this.m_chbxInterruptOnTXFifoNotFull.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnTXFifoNotFull.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnTXFifoNotFull_CheckedChanged);
            this.m_chbxInterruptOnTXFifoNotFull.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnTXFifoFull
            // 
            this.m_chbxInterruptOnTXFifoFull.AutoSize = true;
            this.m_chbxInterruptOnTXFifoFull.Location = new System.Drawing.Point(5, 280);
            this.m_chbxInterruptOnTXFifoFull.Name = "m_chbxInterruptOnTXFifoFull";
            this.m_chbxInterruptOnTXFifoFull.Size = new System.Drawing.Size(108, 17);
            this.m_chbxInterruptOnTXFifoFull.TabIndex = 19;
            this.m_chbxInterruptOnTXFifoFull.Text = "TX - On FIFO Full";
            this.m_chbxInterruptOnTXFifoFull.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnTXFifoFull.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnTXFifoFull_CheckedChanged);
            this.m_chbxInterruptOnTXFifoFull.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnTXFifoEmpty
            // 
            this.m_chbxInterruptOnTXFifoEmpty.AutoSize = true;
            this.m_chbxInterruptOnTXFifoEmpty.Location = new System.Drawing.Point(5, 259);
            this.m_chbxInterruptOnTXFifoEmpty.Name = "m_chbxInterruptOnTXFifoEmpty";
            this.m_chbxInterruptOnTXFifoEmpty.Size = new System.Drawing.Size(121, 17);
            this.m_chbxInterruptOnTXFifoEmpty.TabIndex = 18;
            this.m_chbxInterruptOnTXFifoEmpty.Text = "TX - On FIFO Empty";
            this.m_chbxInterruptOnTXFifoEmpty.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnTXFifoEmpty.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnTXFifoEmpty_CheckedChanged);
            this.m_chbxInterruptOnTXFifoEmpty.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnTXComplete
            // 
            this.m_chbxInterruptOnTXComplete.AutoSize = true;
            this.m_chbxInterruptOnTXComplete.Location = new System.Drawing.Point(5, 238);
            this.m_chbxInterruptOnTXComplete.Name = "m_chbxInterruptOnTXComplete";
            this.m_chbxInterruptOnTXComplete.Size = new System.Drawing.Size(127, 17);
            this.m_chbxInterruptOnTXComplete.TabIndex = 17;
            this.m_chbxInterruptOnTXComplete.Text = "TX - On TX Complete";
            this.m_chbxInterruptOnTXComplete.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnTXComplete.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnTXComplete_CheckedChanged);
            this.m_chbxInterruptOnTXComplete.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(6, 230);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 2);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            // 
            // m_chbxInterruptOnAddressDetect
            // 
            this.m_chbxInterruptOnAddressDetect.AutoSize = true;
            this.m_chbxInterruptOnAddressDetect.Location = new System.Drawing.Point(5, 207);
            this.m_chbxInterruptOnAddressDetect.Name = "m_chbxInterruptOnAddressDetect";
            this.m_chbxInterruptOnAddressDetect.Size = new System.Drawing.Size(140, 17);
            this.m_chbxInterruptOnAddressDetect.TabIndex = 15;
            this.m_chbxInterruptOnAddressDetect.Text = "RX - On Address Detect";
            this.m_chbxInterruptOnAddressDetect.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnAddressDetect.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnAddressDetect_CheckedChanged);
            this.m_chbxInterruptOnAddressDetect.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnAddressMatch
            // 
            this.m_chbxInterruptOnAddressMatch.AutoSize = true;
            this.m_chbxInterruptOnAddressMatch.Location = new System.Drawing.Point(5, 185);
            this.m_chbxInterruptOnAddressMatch.Name = "m_chbxInterruptOnAddressMatch";
            this.m_chbxInterruptOnAddressMatch.Size = new System.Drawing.Size(135, 17);
            this.m_chbxInterruptOnAddressMatch.TabIndex = 14;
            this.m_chbxInterruptOnAddressMatch.Text = "RX -On Address Match";
            this.m_chbxInterruptOnAddressMatch.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnAddressMatch.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnAddressMatch_CheckedChanged);
            this.m_chbxInterruptOnAddressMatch.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnOverrunError
            // 
            this.m_chbxInterruptOnOverrunError.AutoSize = true;
            this.m_chbxInterruptOnOverrunError.Location = new System.Drawing.Point(5, 162);
            this.m_chbxInterruptOnOverrunError.Name = "m_chbxInterruptOnOverrunError";
            this.m_chbxInterruptOnOverrunError.Size = new System.Drawing.Size(130, 17);
            this.m_chbxInterruptOnOverrunError.TabIndex = 13;
            this.m_chbxInterruptOnOverrunError.Text = "RX - On Overrun Error";
            this.m_chbxInterruptOnOverrunError.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnOverrunError.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnOverrunError_CheckedChanged);
            this.m_chbxInterruptOnOverrunError.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnBreak
            // 
            this.m_chbxInterruptOnBreak.AutoSize = true;
            this.m_chbxInterruptOnBreak.Location = new System.Drawing.Point(5, 139);
            this.m_chbxInterruptOnBreak.Name = "m_chbxInterruptOnBreak";
            this.m_chbxInterruptOnBreak.Size = new System.Drawing.Size(95, 17);
            this.m_chbxInterruptOnBreak.TabIndex = 12;
            this.m_chbxInterruptOnBreak.Text = "RX - On Break";
            this.m_chbxInterruptOnBreak.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnBreak.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnBreak_CheckedChanged);
            this.m_chbxInterruptOnBreak.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnStopError
            // 
            this.m_chbxInterruptOnStopError.AutoSize = true;
            this.m_chbxInterruptOnStopError.Location = new System.Drawing.Point(5, 116);
            this.m_chbxInterruptOnStopError.Name = "m_chbxInterruptOnStopError";
            this.m_chbxInterruptOnStopError.Size = new System.Drawing.Size(114, 17);
            this.m_chbxInterruptOnStopError.TabIndex = 11;
            this.m_chbxInterruptOnStopError.Text = "RX - On Stop Error";
            this.m_chbxInterruptOnStopError.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnStopError.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnStopError_CheckedChanged);
            this.m_chbxInterruptOnStopError.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnParityError
            // 
            this.m_chbxInterruptOnParityError.AutoSize = true;
            this.m_chbxInterruptOnParityError.Location = new System.Drawing.Point(5, 93);
            this.m_chbxInterruptOnParityError.Name = "m_chbxInterruptOnParityError";
            this.m_chbxInterruptOnParityError.Size = new System.Drawing.Size(118, 17);
            this.m_chbxInterruptOnParityError.TabIndex = 10;
            this.m_chbxInterruptOnParityError.Text = "RX - On Parity Error";
            this.m_chbxInterruptOnParityError.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnParityError.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnParityError_CheckedChanged);
            this.m_chbxInterruptOnParityError.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnByteReceived
            // 
            this.m_chbxInterruptOnByteReceived.AutoSize = true;
            this.m_chbxInterruptOnByteReceived.Location = new System.Drawing.Point(5, 70);
            this.m_chbxInterruptOnByteReceived.Name = "m_chbxInterruptOnByteReceived";
            this.m_chbxInterruptOnByteReceived.Size = new System.Drawing.Size(137, 17);
            this.m_chbxInterruptOnByteReceived.TabIndex = 9;
            this.m_chbxInterruptOnByteReceived.Text = "RX - On Byte Received";
            this.m_chbxInterruptOnByteReceived.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnByteReceived.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnByteReceived_CheckedChanged);
            this.m_chbxInterruptOnByteReceived.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(5, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 2);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // m_cbTXInternalInterrupt
            // 
            this.m_cbTXInternalInterrupt.AutoSize = true;
            this.m_cbTXInternalInterrupt.Location = new System.Drawing.Point(4, 18);
            this.m_cbTXInternalInterrupt.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbTXInternalInterrupt.Name = "m_cbTXInternalInterrupt";
            this.m_cbTXInternalInterrupt.Size = new System.Drawing.Size(177, 17);
            this.m_cbTXInternalInterrupt.TabIndex = 5;
            this.m_cbTXInternalInterrupt.Text = "Enable Internal TX Interrupt ISR";
            this.m_cbTXInternalInterrupt.UseVisualStyleBackColor = true;
            this.m_cbTXInternalInterrupt.CheckedChanged += new System.EventHandler(this.m_cbTXInternalInterrupt_CheckedChanged);
            this.m_cbTXInternalInterrupt.EnabledChanged += new System.EventHandler(this.m_cbInternalInterruptsRXTX_EnableChanged);
            // 
            // m_cbRXInternalInterrupt
            // 
            this.m_cbRXInternalInterrupt.AutoSize = true;
            this.m_cbRXInternalInterrupt.Location = new System.Drawing.Point(4, 41);
            this.m_cbRXInternalInterrupt.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbRXInternalInterrupt.Name = "m_cbRXInternalInterrupt";
            this.m_cbRXInternalInterrupt.Size = new System.Drawing.Size(178, 17);
            this.m_cbRXInternalInterrupt.TabIndex = 6;
            this.m_cbRXInternalInterrupt.Text = "Enable Internal RX Interrupt ISR";
            this.m_cbRXInternalInterrupt.UseVisualStyleBackColor = true;
            this.m_cbRXInternalInterrupt.CheckedChanged += new System.EventHandler(this.m_cbRXInternalInterrupt_CheckedChanged);
            this.m_cbRXInternalInterrupt.EnabledChanged += new System.EventHandler(this.m_cbInternalInterruptsRXTX_EnableChanged);
            // 
            // gbClocks
            // 
            this.gbClocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbClocks.Controls.Add(this.m_rbExternalClock);
            this.gbClocks.Controls.Add(this.m_rbInternalClock);
            this.gbClocks.Location = new System.Drawing.Point(10, 2);
            this.gbClocks.Margin = new System.Windows.Forms.Padding(2);
            this.gbClocks.Name = "gbClocks";
            this.gbClocks.Padding = new System.Windows.Forms.Padding(2);
            this.gbClocks.Size = new System.Drawing.Size(271, 42);
            this.gbClocks.TabIndex = 142;
            this.gbClocks.TabStop = false;
            this.gbClocks.Text = "Clock Selection:";
            // 
            // m_rbExternalClock
            // 
            this.m_rbExternalClock.AutoCheck = false;
            this.m_rbExternalClock.AutoSize = true;
            this.m_rbExternalClock.Location = new System.Drawing.Point(129, 17);
            this.m_rbExternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.m_rbExternalClock.Name = "m_rbExternalClock";
            this.m_rbExternalClock.Size = new System.Drawing.Size(93, 17);
            this.m_rbExternalClock.TabIndex = 1;
            this.m_rbExternalClock.TabStop = true;
            this.m_rbExternalClock.Text = "External Clock";
            this.m_rbExternalClock.UseVisualStyleBackColor = true;
            this.m_rbExternalClock.Click += new System.EventHandler(this.m_rbExternalClock_Click);
            // 
            // m_rbInternalClock
            // 
            this.m_rbInternalClock.AutoCheck = false;
            this.m_rbInternalClock.AutoSize = true;
            this.m_rbInternalClock.Location = new System.Drawing.Point(4, 17);
            this.m_rbInternalClock.Margin = new System.Windows.Forms.Padding(2);
            this.m_rbInternalClock.Name = "m_rbInternalClock";
            this.m_rbInternalClock.Size = new System.Drawing.Size(90, 17);
            this.m_rbInternalClock.TabIndex = 0;
            this.m_rbInternalClock.TabStop = true;
            this.m_rbInternalClock.Text = "Internal Clock";
            this.m_rbInternalClock.UseVisualStyleBackColor = true;
            this.m_rbInternalClock.Click += new System.EventHandler(this.m_rbInternalClock_Click);
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
            // m_gbBufferSizes
            // 
            this.m_gbBufferSizes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gbBufferSizes.Controls.Add(this.m_numTXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_lblTXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_numRXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_lblRXBufferSize);
            this.m_gbBufferSizes.Location = new System.Drawing.Point(10, 533);
            this.m_gbBufferSizes.Name = "m_gbBufferSizes";
            this.m_gbBufferSizes.Size = new System.Drawing.Size(271, 75);
            this.m_gbBufferSizes.TabIndex = 145;
            this.m_gbBufferSizes.TabStop = false;
            this.m_gbBufferSizes.Text = "Buffer Sizes:";
            // 
            // m_numTXBufferSize
            // 
            this.m_numTXBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numTXBufferSize.Location = new System.Drawing.Point(151, 40);
            this.m_numTXBufferSize.Name = "m_numTXBufferSize";
            this.m_numTXBufferSize.Size = new System.Drawing.Size(93, 20);
            this.m_numTXBufferSize.TabIndex = 148;
            this.m_numTXBufferSize.ValueChanged += new System.EventHandler(this.m_numTXBufferSize_ValueChanged);
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
            // m_numRXBufferSize
            // 
            this.m_numRXBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numRXBufferSize.Location = new System.Drawing.Point(151, 13);
            this.m_numRXBufferSize.Name = "m_numRXBufferSize";
            this.m_numRXBufferSize.Size = new System.Drawing.Size(93, 20);
            this.m_numRXBufferSize.TabIndex = 146;
            this.m_numRXBufferSize.ValueChanged += new System.EventHandler(this.m_numRXBufferSize_ValueChanged);
            // 
            // m_gbRS485
            // 
            this.m_gbRS485.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gbRS485.Controls.Add(this.m_chbxHWTxEnable);
            this.m_gbRS485.Location = new System.Drawing.Point(10, 481);
            this.m_gbRS485.Name = "m_gbRS485";
            this.m_gbRS485.Size = new System.Drawing.Size(271, 46);
            this.m_gbRS485.TabIndex = 146;
            this.m_gbRS485.TabStop = false;
            this.m_gbRS485.Text = "RS-485 Configuration Options";
            // 
            // m_chbxHWTxEnable
            // 
            this.m_chbxHWTxEnable.AutoSize = true;
            this.m_chbxHWTxEnable.Location = new System.Drawing.Point(5, 19);
            this.m_chbxHWTxEnable.Name = "m_chbxHWTxEnable";
            this.m_chbxHWTxEnable.Size = new System.Drawing.Size(125, 17);
            this.m_chbxHWTxEnable.TabIndex = 0;
            this.m_chbxHWTxEnable.Text = "Hardware TX-Enable";
            this.m_chbxHWTxEnable.UseVisualStyleBackColor = true;
            this.m_chbxHWTxEnable.CheckedChanged += new System.EventHandler(this.m_chbxHWTxEnable_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.m_cbRXAddressMode);
            this.groupBox2.Controls.Add(this.m_numRXAddress2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.m_numRXAddress1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(10, 376);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 99);
            this.groupBox2.TabIndex = 147;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RX Address Configuration";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Address Mode:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbRXAddressMode
            // 
            this.m_cbRXAddressMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbRXAddressMode.FormattingEnabled = true;
            this.m_cbRXAddressMode.Location = new System.Drawing.Point(115, 15);
            this.m_cbRXAddressMode.Name = "m_cbRXAddressMode";
            this.m_cbRXAddressMode.Size = new System.Drawing.Size(129, 21);
            this.m_cbRXAddressMode.TabIndex = 23;
            this.m_cbRXAddressMode.SelectedIndexChanged += new System.EventHandler(this.m_cbRXAddressMode_SelectedIndexChanged);
            // 
            // m_numRXAddress2
            // 
            this.m_numRXAddress2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numRXAddress2.Hexadecimal = true;
            this.m_numRXAddress2.Location = new System.Drawing.Point(115, 67);
            this.m_numRXAddress2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numRXAddress2.Name = "m_numRXAddress2";
            this.m_numRXAddress2.Size = new System.Drawing.Size(129, 20);
            this.m_numRXAddress2.TabIndex = 22;
            this.m_numRXAddress2.ValueChanged += new System.EventHandler(this.m_numRXAddress2_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Address #2:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_numRXAddress1
            // 
            this.m_numRXAddress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numRXAddress1.Hexadecimal = true;
            this.m_numRXAddress1.Location = new System.Drawing.Point(115, 42);
            this.m_numRXAddress1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numRXAddress1.Name = "m_numRXAddress1";
            this.m_numRXAddress1.Size = new System.Drawing.Size(129, 20);
            this.m_numRXAddress1.TabIndex = 20;
            this.m_numRXAddress1.ValueChanged += new System.EventHandler(this.m_numRXAddress1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Address #1:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cyuartadvancedcontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.m_gbRS485);
            this.Controls.Add(this.m_gbBufferSizes);
            this.Controls.Add(this.gbInterrupts);
            this.Controls.Add(this.gbClocks);
            this.Name = "cyuartadvancedcontrol";
            this.Size = new System.Drawing.Size(293, 614);
            this.gbInterrupts.ResumeLayout(false);
            this.gbInterrupts.PerformLayout();
            this.gbClocks.ResumeLayout(false);
            this.gbClocks.PerformLayout();
            this.m_gbBufferSizes.ResumeLayout(false);
            this.m_gbBufferSizes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numTXBufferSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXBufferSize)).EndInit();
            this.m_gbRS485.ResumeLayout(false);
            this.m_gbRS485.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXAddress2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXAddress1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInterrupts;
        private System.Windows.Forms.CheckBox m_cbTXInternalInterrupt;
        private System.Windows.Forms.CheckBox m_cbRXInternalInterrupt;
        private System.Windows.Forms.GroupBox gbClocks;
        private System.Windows.Forms.RadioButton m_rbExternalClock;
        private System.Windows.Forms.RadioButton m_rbInternalClock;
        private System.Windows.Forms.Label m_lblRXBufferSize;
        private System.Windows.Forms.GroupBox m_gbBufferSizes;
        private System.Windows.Forms.NumericUpDown m_numTXBufferSize;
        private System.Windows.Forms.Label m_lblTXBufferSize;
        private System.Windows.Forms.NumericUpDown m_numRXBufferSize;
        private System.Windows.Forms.GroupBox m_gbRS485;
        private System.Windows.Forms.CheckBox m_chbxHWTxEnable;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnStopError;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnParityError;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnByteReceived;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnAddressDetect;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnAddressMatch;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnOverrunError;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnBreak;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown m_numRXAddress2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_numRXAddress1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox m_cbRXAddressMode;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnTXFifoNotFull;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnTXFifoFull;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnTXFifoEmpty;
        private System.Windows.Forms.CheckBox m_chbxInterruptOnTXComplete;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}
