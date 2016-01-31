namespace CyCustomizer.UART_v1_20
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
            this.components = new System.ComponentModel.Container();
            this.gbInterrupts = new System.Windows.Forms.GroupBox();
            this.m_lblRXStatusLabel = new System.Windows.Forms.Label();
            this.m_chbxInterruptOnTXFifoNotFull = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnTXFifoFull = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnTXFifoEmpty = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnTXComplete = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnAddressDetect = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnAddressMatch = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnOverrunError = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnBreak = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnStopError = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnParityError = new System.Windows.Forms.CheckBox();
            this.m_chbxInterruptOnByteReceived = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbClocks = new System.Windows.Forms.GroupBox();
            this.m_lblCalcFrequency = new System.Windows.Forms.Label();
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
            this.m_gbFeatures = new System.Windows.Forms.GroupBox();
            this.m_chbCRCoutputsEn = new System.Windows.Forms.CheckBox();
            this.m_chbUse23Polling = new System.Windows.Forms.CheckBox();
            this.m_chbBreakDetected = new System.Windows.Forms.CheckBox();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_lblTXStatusLabel = new System.Windows.Forms.Label();
            this.m_lblRXStatus = new System.Windows.Forms.Label();
            this.m_lblTXStatus = new System.Windows.Forms.Label();
            this.gbInterrupts.SuspendLayout();
            this.gbClocks.SuspendLayout();
            this.m_gbBufferSizes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numTXBufferSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXBufferSize)).BeginInit();
            this.m_gbRS485.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXAddress2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numRXAddress1)).BeginInit();
            this.m_gbFeatures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.SuspendLayout();
            // 
            // gbInterrupts
            // 
            this.gbInterrupts.Controls.Add(this.m_lblTXStatus);
            this.gbInterrupts.Controls.Add(this.m_lblRXStatus);
            this.gbInterrupts.Controls.Add(this.m_lblTXStatusLabel);
            this.gbInterrupts.Controls.Add(this.m_lblRXStatusLabel);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXFifoNotFull);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXFifoFull);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXFifoEmpty);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnTXComplete);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnAddressDetect);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnAddressMatch);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnOverrunError);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnBreak);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnStopError);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnParityError);
            this.gbInterrupts.Controls.Add(this.m_chbxInterruptOnByteReceived);
            this.gbInterrupts.Controls.Add(this.groupBox1);
            this.gbInterrupts.Location = new System.Drawing.Point(10, 48);
            this.gbInterrupts.Margin = new System.Windows.Forms.Padding(2);
            this.gbInterrupts.Name = "gbInterrupts";
            this.gbInterrupts.Padding = new System.Windows.Forms.Padding(2);
            this.gbInterrupts.Size = new System.Drawing.Size(446, 213);
            this.gbInterrupts.TabIndex = 143;
            this.gbInterrupts.TabStop = false;
            this.gbInterrupts.Text = "Interrupts";
            // 
            // m_lblRXStatusLabel
            // 
            this.m_lblRXStatusLabel.AutoSize = true;
            this.m_lblRXStatusLabel.Location = new System.Drawing.Point(5, 21);
            this.m_lblRXStatusLabel.Name = "m_lblRXStatusLabel";
            this.m_lblRXStatusLabel.Size = new System.Drawing.Size(136, 13);
            this.m_lblRXStatusLabel.TabIndex = 21;
            this.m_lblRXStatusLabel.Text = "Internal RX Interrupt ISR is ";
            // 
            // m_chbxInterruptOnTXFifoNotFull
            // 
            this.m_chbxInterruptOnTXFifoNotFull.AutoSize = true;
            this.m_chbxInterruptOnTXFifoNotFull.Location = new System.Drawing.Point(243, 117);
            this.m_chbxInterruptOnTXFifoNotFull.Name = "m_chbxInterruptOnTXFifoNotFull";
            this.m_chbxInterruptOnTXFifoNotFull.Size = new System.Drawing.Size(128, 17);
            this.m_chbxInterruptOnTXFifoNotFull.TabIndex = 20;
            this.m_chbxInterruptOnTXFifoNotFull.Text = "TX - On FIFO Not Full";
            this.m_chbxInterruptOnTXFifoNotFull.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnTXFifoNotFull.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnTXFifoNotFull_CheckedChanged);
            this.m_chbxInterruptOnTXFifoNotFull.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnTXFifoFull
            // 
            this.m_chbxInterruptOnTXFifoFull.AutoSize = true;
            this.m_chbxInterruptOnTXFifoFull.Location = new System.Drawing.Point(243, 96);
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
            this.m_chbxInterruptOnTXFifoEmpty.Location = new System.Drawing.Point(243, 75);
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
            this.m_chbxInterruptOnTXComplete.Location = new System.Drawing.Point(243, 54);
            this.m_chbxInterruptOnTXComplete.Name = "m_chbxInterruptOnTXComplete";
            this.m_chbxInterruptOnTXComplete.Size = new System.Drawing.Size(127, 17);
            this.m_chbxInterruptOnTXComplete.TabIndex = 17;
            this.m_chbxInterruptOnTXComplete.Text = "TX - On TX Complete";
            this.m_chbxInterruptOnTXComplete.UseVisualStyleBackColor = true;
            this.m_chbxInterruptOnTXComplete.CheckedChanged += new System.EventHandler(this.m_chbxInterruptOnTXComplete_CheckedChanged);
            this.m_chbxInterruptOnTXComplete.EnabledChanged += new System.EventHandler(this.m_cbInterrupts_EnableChanged);
            // 
            // m_chbxInterruptOnAddressDetect
            // 
            this.m_chbxInterruptOnAddressDetect.AutoSize = true;
            this.m_chbxInterruptOnAddressDetect.Location = new System.Drawing.Point(5, 189);
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
            this.m_chbxInterruptOnAddressMatch.Location = new System.Drawing.Point(5, 167);
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
            this.m_chbxInterruptOnOverrunError.Location = new System.Drawing.Point(5, 144);
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
            this.m_chbxInterruptOnBreak.Location = new System.Drawing.Point(5, 121);
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
            this.m_chbxInterruptOnStopError.Location = new System.Drawing.Point(5, 98);
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
            this.m_chbxInterruptOnParityError.Location = new System.Drawing.Point(5, 75);
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
            this.m_chbxInterruptOnByteReceived.Location = new System.Drawing.Point(5, 52);
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
            this.groupBox1.Location = new System.Drawing.Point(5, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 2);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // gbClocks
            // 
            this.gbClocks.Controls.Add(this.m_lblCalcFrequency);
            this.gbClocks.Controls.Add(this.m_rbExternalClock);
            this.gbClocks.Controls.Add(this.m_rbInternalClock);
            this.gbClocks.Location = new System.Drawing.Point(10, 2);
            this.gbClocks.Margin = new System.Windows.Forms.Padding(2);
            this.gbClocks.Name = "gbClocks";
            this.gbClocks.Padding = new System.Windows.Forms.Padding(2);
            this.gbClocks.Size = new System.Drawing.Size(446, 42);
            this.gbClocks.TabIndex = 142;
            this.gbClocks.TabStop = false;
            this.gbClocks.Text = "Clock Selection:";
            // 
            // m_lblCalcFrequency
            // 
            this.m_lblCalcFrequency.AutoSize = true;
            this.m_lblCalcFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_lblCalcFrequency.Location = new System.Drawing.Point(231, 17);
            this.m_lblCalcFrequency.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblCalcFrequency.Name = "m_lblCalcFrequency";
            this.m_lblCalcFrequency.Size = new System.Drawing.Size(91, 13);
            this.m_lblCalcFrequency.TabIndex = 2;
            this.m_lblCalcFrequency.Text = "CalcFrequency";
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
            this.m_rbExternalClock.CheckedChanged += new System.EventHandler(this.m_rbExternalClock_CheckedChanged);
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
            this.m_lblRXBufferSize.Location = new System.Drawing.Point(8, 25);
            this.m_lblRXBufferSize.Name = "m_lblRXBufferSize";
            this.m_lblRXBufferSize.Size = new System.Drawing.Size(113, 13);
            this.m_lblRXBufferSize.TabIndex = 144;
            this.m_lblRXBufferSize.Text = "RX Buffer Size (bytes):";
            this.m_lblRXBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_gbBufferSizes
            // 
            this.m_gbBufferSizes.Controls.Add(this.m_numTXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_lblTXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_numRXBufferSize);
            this.m_gbBufferSizes.Controls.Add(this.m_lblRXBufferSize);
            this.m_gbBufferSizes.Location = new System.Drawing.Point(259, 266);
            this.m_gbBufferSizes.Name = "m_gbBufferSizes";
            this.m_gbBufferSizes.Size = new System.Drawing.Size(197, 99);
            this.m_gbBufferSizes.TabIndex = 148;
            this.m_gbBufferSizes.TabStop = false;
            this.m_gbBufferSizes.Text = "Buffer Sizes:";
            // 
            // m_numTXBufferSize
            // 
            this.m_numTXBufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numTXBufferSize.Location = new System.Drawing.Point(127, 48);
            this.m_numTXBufferSize.Name = "m_numTXBufferSize";
            this.m_numTXBufferSize.Size = new System.Drawing.Size(55, 20);
            this.m_numTXBufferSize.TabIndex = 148;
            this.m_numTXBufferSize.ValueChanged += new System.EventHandler(this.BufferSizeChanged);
            this.m_numTXBufferSize.Validated += new System.EventHandler(this.m_numTXBufferSize_Validated);
            // 
            // m_lblTXBufferSize
            // 
            this.m_lblTXBufferSize.AutoSize = true;
            this.m_lblTXBufferSize.Location = new System.Drawing.Point(8, 52);
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
            this.m_numRXBufferSize.Location = new System.Drawing.Point(127, 21);
            this.m_numRXBufferSize.Name = "m_numRXBufferSize";
            this.m_numRXBufferSize.Size = new System.Drawing.Size(55, 20);
            this.m_numRXBufferSize.TabIndex = 146;
            this.m_numRXBufferSize.ValueChanged += new System.EventHandler(this.BufferSizeChanged);
            this.m_numRXBufferSize.Validated += new System.EventHandler(this.m_numRXBufferSize_Validated);
            // 
            // m_gbRS485
            // 
            this.m_gbRS485.Controls.Add(this.m_chbxHWTxEnable);
            this.m_gbRS485.Location = new System.Drawing.Point(259, 371);
            this.m_gbRS485.Name = "m_gbRS485";
            this.m_gbRS485.Size = new System.Drawing.Size(197, 85);
            this.m_gbRS485.TabIndex = 150;
            this.m_gbRS485.TabStop = false;
            this.m_gbRS485.Text = "RS-485 Configuration Options";
            // 
            // m_chbxHWTxEnable
            // 
            this.m_chbxHWTxEnable.AutoSize = true;
            this.m_chbxHWTxEnable.Location = new System.Drawing.Point(6, 19);
            this.m_chbxHWTxEnable.Name = "m_chbxHWTxEnable";
            this.m_chbxHWTxEnable.Size = new System.Drawing.Size(125, 17);
            this.m_chbxHWTxEnable.TabIndex = 0;
            this.m_chbxHWTxEnable.Text = "Hardware TX-Enable";
            this.m_chbxHWTxEnable.UseVisualStyleBackColor = true;
            this.m_chbxHWTxEnable.CheckedChanged += new System.EventHandler(this.m_chbxHWTxEnable_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.m_cbRXAddressMode);
            this.groupBox2.Controls.Add(this.m_numRXAddress2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.m_numRXAddress1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(10, 266);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(249, 99);
            this.groupBox2.TabIndex = 147;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RX Address Configuration";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 25);
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
            this.m_cbRXAddressMode.Location = new System.Drawing.Point(94, 22);
            this.m_cbRXAddressMode.Name = "m_cbRXAddressMode";
            this.m_cbRXAddressMode.Size = new System.Drawing.Size(138, 21);
            this.m_cbRXAddressMode.TabIndex = 15;
            this.m_cbRXAddressMode.SelectedIndexChanged += new System.EventHandler(this.m_cbRXAddressMode_SelectedIndexChanged);
            // 
            // m_numRXAddress2
            // 
            this.m_numRXAddress2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numRXAddress2.Hexadecimal = true;
            this.m_numRXAddress2.Location = new System.Drawing.Point(94, 74);
            this.m_numRXAddress2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numRXAddress2.Name = "m_numRXAddress2";
            this.m_numRXAddress2.Size = new System.Drawing.Size(139, 20);
            this.m_numRXAddress2.TabIndex = 22;
            this.m_numRXAddress2.ValueChanged += new System.EventHandler(this.m_numRXAddress2_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 76);
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
            this.m_numRXAddress1.Location = new System.Drawing.Point(94, 49);
            this.m_numRXAddress1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numRXAddress1.Name = "m_numRXAddress1";
            this.m_numRXAddress1.Size = new System.Drawing.Size(139, 20);
            this.m_numRXAddress1.TabIndex = 20;
            this.m_numRXAddress1.ValueChanged += new System.EventHandler(this.m_numRXAddress1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Address #1:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_gbFeatures
            // 
            this.m_gbFeatures.Controls.Add(this.m_chbCRCoutputsEn);
            this.m_gbFeatures.Controls.Add(this.m_chbUse23Polling);
            this.m_gbFeatures.Controls.Add(this.m_chbBreakDetected);
            this.m_gbFeatures.Location = new System.Drawing.Point(10, 371);
            this.m_gbFeatures.Name = "m_gbFeatures";
            this.m_gbFeatures.Size = new System.Drawing.Size(249, 85);
            this.m_gbFeatures.TabIndex = 149;
            this.m_gbFeatures.TabStop = false;
            this.m_gbFeatures.Text = "Advanced Features";
            // 
            // m_chbCRCoutputsEn
            // 
            this.m_chbCRCoutputsEn.AutoSize = true;
            this.m_chbCRCoutputsEn.Location = new System.Drawing.Point(5, 65);
            this.m_chbCRCoutputsEn.Name = "m_chbCRCoutputsEn";
            this.m_chbCRCoutputsEn.Size = new System.Drawing.Size(122, 17);
            this.m_chbCRCoutputsEn.TabIndex = 2;
            this.m_chbCRCoutputsEn.Text = "Enable CRC outputs";
            this.m_chbCRCoutputsEn.UseVisualStyleBackColor = true;
            this.m_chbCRCoutputsEn.CheckedChanged += new System.EventHandler(this.m_chbCRCoutputsEn_CheckedChanged);
            // 
            // m_chbUse23Polling
            // 
            this.m_chbUse23Polling.AutoSize = true;
            this.m_chbUse23Polling.Location = new System.Drawing.Point(6, 42);
            this.m_chbUse23Polling.Name = "m_chbUse23Polling";
            this.m_chbUse23Polling.Size = new System.Drawing.Size(171, 17);
            this.m_chbUse23Polling.TabIndex = 1;
            this.m_chbUse23Polling.Text = "Enable 2 out of 3 voting per bit";
            this.m_chbUse23Polling.UseVisualStyleBackColor = true;
            this.m_chbUse23Polling.CheckedChanged += new System.EventHandler(this.m_chbUse23Polling_CheckedChanged);
            // 
            // m_chbBreakDetected
            // 
            this.m_chbBreakDetected.AutoSize = true;
            this.m_chbBreakDetected.Location = new System.Drawing.Point(6, 19);
            this.m_chbBreakDetected.Name = "m_chbBreakDetected";
            this.m_chbBreakDetected.Size = new System.Drawing.Size(240, 17);
            this.m_chbBreakDetected.TabIndex = 0;
            this.m_chbBreakDetected.Text = "Enable break signal generation and detection";
            this.m_chbBreakDetected.UseVisualStyleBackColor = true;
            this.m_chbBreakDetected.CheckedChanged += new System.EventHandler(this.m_chbBreakDetected_CheckedChanged);
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // m_lblTXStatusLabel
            // 
            this.m_lblTXStatusLabel.AutoSize = true;
            this.m_lblTXStatusLabel.Location = new System.Drawing.Point(240, 21);
            this.m_lblTXStatusLabel.Name = "m_lblTXStatusLabel";
            this.m_lblTXStatusLabel.Size = new System.Drawing.Size(135, 13);
            this.m_lblTXStatusLabel.TabIndex = 22;
            this.m_lblTXStatusLabel.Text = "Internal TX Interrupt ISR is ";
            // 
            // m_lblRXStatus
            // 
            this.m_lblRXStatus.AutoSize = true;
            this.m_lblRXStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_lblRXStatus.Location = new System.Drawing.Point(139, 21);
            this.m_lblRXStatus.Name = "m_lblRXStatus";
            this.m_lblRXStatus.Size = new System.Drawing.Size(41, 13);
            this.m_lblRXStatus.TabIndex = 23;
            this.m_lblRXStatus.Text = "status";
            // 
            // m_lblTXStatus
            // 
            this.m_lblTXStatus.AutoSize = true;
            this.m_lblTXStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.m_lblTXStatus.Location = new System.Drawing.Point(373, 21);
            this.m_lblTXStatus.Name = "m_lblTXStatus";
            this.m_lblTXStatus.Size = new System.Drawing.Size(41, 13);
            this.m_lblTXStatus.TabIndex = 24;
            this.m_lblTXStatus.Text = "status";
            // 
            // cyuartadvancedcontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gbFeatures);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.m_gbRS485);
            this.Controls.Add(this.m_gbBufferSizes);
            this.Controls.Add(this.gbInterrupts);
            this.Controls.Add(this.gbClocks);
            this.Name = "cyuartadvancedcontrol";
            this.Size = new System.Drawing.Size(459, 461);
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
            this.m_gbFeatures.ResumeLayout(false);
            this.m_gbFeatures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInterrupts;
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
        private System.Windows.Forms.GroupBox m_gbFeatures;
        private System.Windows.Forms.CheckBox m_chbUse23Polling;
        private System.Windows.Forms.CheckBox m_chbBreakDetected;
        private System.Windows.Forms.CheckBox m_chbCRCoutputsEn;
        private System.Windows.Forms.Label m_lblCalcFrequency;
        private System.Windows.Forms.ErrorProvider ep_Errors;
        private System.Windows.Forms.Label m_lblRXStatusLabel;
        private System.Windows.Forms.Label m_lblTXStatus;
        private System.Windows.Forms.Label m_lblRXStatus;
        private System.Windows.Forms.Label m_lblTXStatusLabel;
    }
}
