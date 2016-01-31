namespace Timer_v1_20
{
    partial class CyTimerControl
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
            this.m_rbResolution16 = new System.Windows.Forms.RadioButton();
            this.m_lblResolution = new System.Windows.Forms.Label();
            this.m_rbResolution32 = new System.Windows.Forms.RadioButton();
            this.m_rbResolution24 = new System.Windows.Forms.RadioButton();
            this.m_rbUDB = new System.Windows.Forms.RadioButton();
            this.m_rbFixedFunction = new System.Windows.Forms.RadioButton();
            this.m_lblImplementation = new System.Windows.Forms.Label();
            this.m_lblCaptureMode = new System.Windows.Forms.Label();
            this.m_cbCaptureMode = new System.Windows.Forms.ComboBox();
            this.m_lblTrigger = new System.Windows.Forms.Label();
            this.m_cbTriggerMode = new System.Windows.Forms.ComboBox();
            this.m_cbRunMode = new System.Windows.Forms.ComboBox();
            this.m_lblRunMode = new System.Windows.Forms.Label();
            this.m_lblCalcFrequency = new System.Windows.Forms.Label();
            this.m_lblPeriod = new System.Windows.Forms.Label();
            this.m_lblInterruptSources = new System.Windows.Forms.Label();
            this.m_chbxIntSrcCapture = new System.Windows.Forms.CheckBox();
            this.m_chbxIntSrcTC = new System.Windows.Forms.CheckBox();
            this.m_tooltips = new System.Windows.Forms.ToolTip(this.components);
            this.m_chbxEnableCaptureCounter = new System.Windows.Forms.CheckBox();
            this.m_bMaxPeriod = new System.Windows.Forms.Button();
            this.m_lblEnableMode = new System.Windows.Forms.Label();
            this.m_rbResolution8 = new System.Windows.Forms.RadioButton();
            this.m_numInterruptOnCaptureCount = new Timer_v1_20.CyNumericUpDown();
            this.m_numPeriod = new Timer_v1_20.CyNumericUpDown();
            this.m_numCaptureCount = new Timer_v1_20.CyNumericUpDown();
            this.m_lblNumEdges = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_cbEnableMode = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.m_chbxIntSrcFifoFull = new System.Windows.Forms.CheckBox();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_numInterruptOnCaptureCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numCaptureCount)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.SuspendLayout();
            // 
            // m_rbResolution16
            // 
            this.m_rbResolution16.AutoCheck = false;
            this.m_rbResolution16.AutoSize = true;
            this.m_rbResolution16.Location = new System.Drawing.Point(70, 6);
            this.m_rbResolution16.Name = "m_rbResolution16";
            this.m_rbResolution16.Size = new System.Drawing.Size(52, 17);
            this.m_rbResolution16.TabIndex = 2;
            this.m_rbResolution16.TabStop = true;
            this.m_rbResolution16.Text = "16-Bit";
            this.m_tooltips.SetToolTip(this.m_rbResolution16, "16-Bit Resolution: Period Counter Value 65,535-0");
            this.m_rbResolution16.UseVisualStyleBackColor = true;
            this.m_rbResolution16.Click += new System.EventHandler(this.m_rbResolution16_Click);
            // 
            // m_lblResolution
            // 
            this.m_lblResolution.AutoSize = true;
            this.m_lblResolution.Location = new System.Drawing.Point(47, 9);
            this.m_lblResolution.Name = "m_lblResolution";
            this.m_lblResolution.Size = new System.Drawing.Size(60, 13);
            this.m_lblResolution.TabIndex = 14;
            this.m_lblResolution.Text = "Resolution:";
            this.m_lblResolution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblResolution, "Defines the maximum Period Counter value.");
            // 
            // m_rbResolution32
            // 
            this.m_rbResolution32.AutoCheck = false;
            this.m_rbResolution32.AutoSize = true;
            this.m_rbResolution32.Location = new System.Drawing.Point(202, 6);
            this.m_rbResolution32.Name = "m_rbResolution32";
            this.m_rbResolution32.Size = new System.Drawing.Size(52, 17);
            this.m_rbResolution32.TabIndex = 4;
            this.m_rbResolution32.TabStop = true;
            this.m_rbResolution32.Text = "32-Bit";
            this.m_tooltips.SetToolTip(this.m_rbResolution32, "32-Bit Resolution: Period Counter Value 4,294,967,295-0");
            this.m_rbResolution32.UseVisualStyleBackColor = true;
            this.m_rbResolution32.Click += new System.EventHandler(this.m_rbResolution32_Click);
            // 
            // m_rbResolution24
            // 
            this.m_rbResolution24.AutoCheck = false;
            this.m_rbResolution24.AutoSize = true;
            this.m_rbResolution24.Location = new System.Drawing.Point(136, 6);
            this.m_rbResolution24.Name = "m_rbResolution24";
            this.m_rbResolution24.Size = new System.Drawing.Size(52, 17);
            this.m_rbResolution24.TabIndex = 3;
            this.m_rbResolution24.TabStop = true;
            this.m_rbResolution24.Text = "24-Bit";
            this.m_tooltips.SetToolTip(this.m_rbResolution24, "24-Bit Resolution: Period Counter Value 16,777,215-0");
            this.m_rbResolution24.UseVisualStyleBackColor = true;
            this.m_rbResolution24.Click += new System.EventHandler(this.m_rbResolution24_Click);
            // 
            // m_rbUDB
            // 
            this.m_rbUDB.AutoCheck = false;
            this.m_rbUDB.AutoSize = true;
            this.m_rbUDB.Location = new System.Drawing.Point(139, 2);
            this.m_rbUDB.Name = "m_rbUDB";
            this.m_rbUDB.Size = new System.Drawing.Size(48, 17);
            this.m_rbUDB.TabIndex = 12;
            this.m_rbUDB.TabStop = true;
            this.m_rbUDB.Text = "UDB";
            this.m_tooltips.SetToolTip(this.m_rbUDB, "Use UDB resources");
            this.m_rbUDB.UseVisualStyleBackColor = true;
            this.m_rbUDB.Click += new System.EventHandler(this.m_rbUDB_Click);
            // 
            // m_rbFixedFunction
            // 
            this.m_rbFixedFunction.AutoCheck = false;
            this.m_rbFixedFunction.AutoSize = true;
            this.m_rbFixedFunction.Location = new System.Drawing.Point(13, 2);
            this.m_rbFixedFunction.Name = "m_rbFixedFunction";
            this.m_rbFixedFunction.Size = new System.Drawing.Size(94, 17);
            this.m_rbFixedFunction.TabIndex = 11;
            this.m_rbFixedFunction.TabStop = true;
            this.m_rbFixedFunction.Text = "Fixed Function";
            this.m_tooltips.SetToolTip(this.m_rbFixedFunction, "Use Fixed Hardware Timer block (with limitations from the UDB Implementations)");
            this.m_rbFixedFunction.UseVisualStyleBackColor = true;
            this.m_rbFixedFunction.Click += new System.EventHandler(this.m_rbFixedFunction_Click);
            // 
            // m_lblImplementation
            // 
            this.m_lblImplementation.AutoSize = true;
            this.m_lblImplementation.Location = new System.Drawing.Point(26, 32);
            this.m_lblImplementation.Name = "m_lblImplementation";
            this.m_lblImplementation.Size = new System.Drawing.Size(81, 13);
            this.m_lblImplementation.TabIndex = 19;
            this.m_lblImplementation.Text = "Implementation:";
            this.m_lblImplementation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblImplementation, "Defines the implementation as either a fixed hardware block (With limitations) or" +
                    " UDB resource usage");
            // 
            // m_lblCaptureMode
            // 
            this.m_lblCaptureMode.AutoSize = true;
            this.m_lblCaptureMode.Location = new System.Drawing.Point(30, 125);
            this.m_lblCaptureMode.Name = "m_lblCaptureMode";
            this.m_lblCaptureMode.Size = new System.Drawing.Size(77, 13);
            this.m_lblCaptureMode.TabIndex = 53;
            this.m_lblCaptureMode.Text = "Capture Mode:";
            this.m_lblCaptureMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblCaptureMode, "Define the hardware capture input required to cause a period capture");
            // 
            // m_cbCaptureMode
            // 
            this.m_cbCaptureMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbCaptureMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbCaptureMode.FormattingEnabled = true;
            this.m_cbCaptureMode.Location = new System.Drawing.Point(129, 108);
            this.m_cbCaptureMode.Name = "m_cbCaptureMode";
            this.m_cbCaptureMode.Size = new System.Drawing.Size(283, 21);
            this.m_cbCaptureMode.TabIndex = 40;
            this.m_cbCaptureMode.SelectedIndexChanged += new System.EventHandler(this.m_cbCaptureMode_SelectedIndexChanged);
            // 
            // m_lblTrigger
            // 
            this.m_lblTrigger.AutoSize = true;
            this.m_lblTrigger.Location = new System.Drawing.Point(34, 83);
            this.m_lblTrigger.Name = "m_lblTrigger";
            this.m_lblTrigger.Size = new System.Drawing.Size(73, 13);
            this.m_lblTrigger.TabIndex = 51;
            this.m_lblTrigger.Text = "Trigger Mode:";
            this.m_lblTrigger.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblTrigger, "Define the trigger input required to enable the timer");
            // 
            // m_cbTriggerMode
            // 
            this.m_cbTriggerMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbTriggerMode.FormattingEnabled = true;
            this.m_cbTriggerMode.Location = new System.Drawing.Point(129, 79);
            this.m_cbTriggerMode.Name = "m_cbTriggerMode";
            this.m_cbTriggerMode.Size = new System.Drawing.Size(283, 21);
            this.m_cbTriggerMode.TabIndex = 30;
            this.m_cbTriggerMode.SelectedIndexChanged += new System.EventHandler(this.m_cbTriggerMode_SelectedIndexChanged);
            // 
            // m_cbRunMode
            // 
            this.m_cbRunMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbRunMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbRunMode.FormattingEnabled = true;
            this.m_cbRunMode.Location = new System.Drawing.Point(129, 189);
            this.m_cbRunMode.Name = "m_cbRunMode";
            this.m_cbRunMode.Size = new System.Drawing.Size(283, 21);
            this.m_cbRunMode.TabIndex = 60;
            this.m_cbRunMode.SelectedIndexChanged += new System.EventHandler(this.m_cbRunMode_SelectedIndexChanged);
            // 
            // m_lblRunMode
            // 
            this.m_lblRunMode.AutoSize = true;
            this.m_lblRunMode.Location = new System.Drawing.Point(47, 193);
            this.m_lblRunMode.Name = "m_lblRunMode";
            this.m_lblRunMode.Size = new System.Drawing.Size(60, 13);
            this.m_lblRunMode.TabIndex = 54;
            this.m_lblRunMode.Text = "Run Mode:";
            this.m_lblRunMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblRunMode, "Define the operating mode of the timer");
            // 
            // m_lblCalcFrequency
            // 
            this.m_lblCalcFrequency.AutoSize = true;
            this.m_lblCalcFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblCalcFrequency.Location = new System.Drawing.Point(287, 57);
            this.m_lblCalcFrequency.Name = "m_lblCalcFrequency";
            this.m_lblCalcFrequency.Size = new System.Drawing.Size(74, 13);
            this.m_lblCalcFrequency.TabIndex = 151;
            this.m_lblCalcFrequency.Text = "Frequency: ";
            // 
            // m_lblPeriod
            // 
            this.m_lblPeriod.AutoSize = true;
            this.m_lblPeriod.Location = new System.Drawing.Point(67, 57);
            this.m_lblPeriod.Name = "m_lblPeriod";
            this.m_lblPeriod.Size = new System.Drawing.Size(40, 13);
            this.m_lblPeriod.TabIndex = 150;
            this.m_lblPeriod.Text = "Period:";
            this.m_lblPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblPeriod, "Define the initial period (reload value of the down counter).  May be changed at " +
                    "any time with API");
            // 
            // m_lblInterruptSources
            // 
            this.m_lblInterruptSources.AutoSize = true;
            this.m_lblInterruptSources.Location = new System.Drawing.Point(53, 221);
            this.m_lblInterruptSources.Name = "m_lblInterruptSources";
            this.m_lblInterruptSources.Size = new System.Drawing.Size(54, 13);
            this.m_lblInterruptSources.TabIndex = 152;
            this.m_lblInterruptSources.Text = "Interrupts:";
            this.m_lblInterruptSources.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblInterruptSources, "Select the initial interrupt sources (May be set at any time with API)");
            // 
            // m_chbxIntSrcCapture
            // 
            this.m_chbxIntSrcCapture.AutoSize = true;
            this.m_chbxIntSrcCapture.Location = new System.Drawing.Point(249, 219);
            this.m_chbxIntSrcCapture.Name = "m_chbxIntSrcCapture";
            this.m_chbxIntSrcCapture.Size = new System.Drawing.Size(104, 17);
            this.m_chbxIntSrcCapture.TabIndex = 71;
            this.m_chbxIntSrcCapture.Text = "On Capture [1-4]";
            this.m_tooltips.SetToolTip(this.m_chbxIntSrcCapture, "Enabled when capture mode is not none.");
            this.m_chbxIntSrcCapture.UseVisualStyleBackColor = true;
            this.m_chbxIntSrcCapture.CheckedChanged += new System.EventHandler(this.m_chbxIntSrcCapture_CheckedChanged);
            this.m_chbxIntSrcCapture.EnabledChanged += new System.EventHandler(this.m_chbxIntSrcCapture_EnabledChanged);
            // 
            // m_chbxIntSrcTC
            // 
            this.m_chbxIntSrcTC.AutoSize = true;
            this.m_chbxIntSrcTC.Location = new System.Drawing.Point(129, 219);
            this.m_chbxIntSrcTC.Name = "m_chbxIntSrcTC";
            this.m_chbxIntSrcTC.Size = new System.Drawing.Size(57, 17);
            this.m_chbxIntSrcTC.TabIndex = 70;
            this.m_chbxIntSrcTC.Text = "On TC";
            this.m_chbxIntSrcTC.UseVisualStyleBackColor = true;
            this.m_chbxIntSrcTC.CheckedChanged += new System.EventHandler(this.m_chbxIntSrcTC_CheckedChanged);
            // 
            // m_chbxEnableCaptureCounter
            // 
            this.m_chbxEnableCaptureCounter.AutoSize = true;
            this.m_chbxEnableCaptureCounter.Location = new System.Drawing.Point(129, 135);
            this.m_chbxEnableCaptureCounter.Name = "m_chbxEnableCaptureCounter";
            this.m_chbxEnableCaptureCounter.Size = new System.Drawing.Size(139, 17);
            this.m_chbxEnableCaptureCounter.TabIndex = 41;
            this.m_chbxEnableCaptureCounter.Text = "Enable Capture Counter";
            this.m_tooltips.SetToolTip(this.m_chbxEnableCaptureCounter, "Enable A counter to wait for X valid capture signals before the capture is actual" +
                    "ly implemented");
            this.m_chbxEnableCaptureCounter.UseVisualStyleBackColor = true;
            this.m_chbxEnableCaptureCounter.VisibleChanged += new System.EventHandler(this.m_chbxEnableCaptureCounter_VisibleChanged);
            this.m_chbxEnableCaptureCounter.CheckedChanged += new System.EventHandler(this.m_chbxEnableCaptureCounter_CheckedChanged);
            this.m_chbxEnableCaptureCounter.EnabledChanged += new System.EventHandler(this.m_chbxEnableCaptureCounter_EnabledChanged);
            // 
            // m_bMaxPeriod
            // 
            this.m_bMaxPeriod.Location = new System.Drawing.Point(226, 51);
            this.m_bMaxPeriod.Name = "m_bMaxPeriod";
            this.m_bMaxPeriod.Size = new System.Drawing.Size(58, 25);
            this.m_bMaxPeriod.TabIndex = 21;
            this.m_bMaxPeriod.Text = "Max";
            this.m_tooltips.SetToolTip(this.m_bMaxPeriod, "Set to the period to the maximum value supported for the resolution setting");
            this.m_bMaxPeriod.UseVisualStyleBackColor = true;
            this.m_bMaxPeriod.Click += new System.EventHandler(this.m_bMaxPeriod_Click);
            // 
            // m_lblEnableMode
            // 
            this.m_lblEnableMode.AutoSize = true;
            this.m_lblEnableMode.Location = new System.Drawing.Point(34, 163);
            this.m_lblEnableMode.Name = "m_lblEnableMode";
            this.m_lblEnableMode.Size = new System.Drawing.Size(73, 13);
            this.m_lblEnableMode.TabIndex = 164;
            this.m_lblEnableMode.Text = "Enable Mode:";
            this.m_lblEnableMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_tooltips.SetToolTip(this.m_lblEnableMode, "Define how & when the timer is enabled");
            // 
            // m_rbResolution8
            // 
            this.m_rbResolution8.AutoCheck = false;
            this.m_rbResolution8.AutoSize = true;
            this.m_rbResolution8.Location = new System.Drawing.Point(10, 6);
            this.m_rbResolution8.Name = "m_rbResolution8";
            this.m_rbResolution8.Size = new System.Drawing.Size(46, 17);
            this.m_rbResolution8.TabIndex = 1;
            this.m_rbResolution8.TabStop = true;
            this.m_rbResolution8.Text = "8-Bit";
            this.m_tooltips.SetToolTip(this.m_rbResolution8, "8-Bit Resolution: Period Counter Value 255-0");
            this.m_rbResolution8.UseVisualStyleBackColor = true;
            this.m_rbResolution8.Click += new System.EventHandler(this.m_rbResolution8_Click);
            // 
            // m_numInterruptOnCaptureCount
            // 
            this.m_numInterruptOnCaptureCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ep_Errors.SetIconPadding(this.m_numInterruptOnCaptureCount, 1);
            this.m_numInterruptOnCaptureCount.Location = new System.Drawing.Point(366, 217);
            this.m_numInterruptOnCaptureCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.m_numInterruptOnCaptureCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numInterruptOnCaptureCount.Name = "m_numInterruptOnCaptureCount";
            this.m_numInterruptOnCaptureCount.Size = new System.Drawing.Size(46, 20);
            this.m_numInterruptOnCaptureCount.TabIndex = 72;
            this.m_tooltips.SetToolTip(this.m_numInterruptOnCaptureCount, "Enabled when in UDB mode and Interrupt on Capture is checked. The fixed function " +
                    "block does not have a capture counter.");
            this.m_numInterruptOnCaptureCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numInterruptOnCaptureCount.ValueChanged += new System.EventHandler(this.m_numInterruptOnCaptureCount_ValueChanged);
            this.m_numInterruptOnCaptureCount.Validating += new System.ComponentModel.CancelEventHandler(this.m_numInterruptOnCaptureCount_Validating);
            this.m_numInterruptOnCaptureCount.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_numInterruptOnCaptureCount_KeyUp);
            // 
            // m_numPeriod
            // 
            this.ep_Errors.SetIconAlignment(this.m_numPeriod, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.m_numPeriod.Location = new System.Drawing.Point(129, 53);
            this.m_numPeriod.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.m_numPeriod.Minimum = new decimal(new int[] {
            0,
            1,
            0,
            -2147483648});
            this.m_numPeriod.Name = "m_numPeriod";
            this.m_numPeriod.Size = new System.Drawing.Size(91, 20);
            this.m_numPeriod.TabIndex = 20;
            this.m_tooltips.SetToolTip(this.m_numPeriod, "Initial Period value loaded into the downcounter at initialization and reloaded a" +
                    "t TC");
            this.m_numPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numPeriod.ValueChanged += new System.EventHandler(this.m_numPeriod_ValueChanged);
            this.m_numPeriod.Validating += new System.ComponentModel.CancelEventHandler(this.m_numPeriod_Validating);
            this.m_numPeriod.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_numPeriod_KeyUp);
            // 
            // m_numCaptureCount
            // 
            this.m_numCaptureCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_numCaptureCount.Location = new System.Drawing.Point(343, 133);
            this.m_numCaptureCount.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.m_numCaptureCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.m_numCaptureCount.Name = "m_numCaptureCount";
            this.m_numCaptureCount.Size = new System.Drawing.Size(69, 20);
            this.m_numCaptureCount.TabIndex = 42;
            this.m_tooltips.SetToolTip(this.m_numCaptureCount, "Number of edges to detect before capturing the counter value. When set to 0 a cap" +
                    "ture will be generated on every detected edge.");
            this.m_numCaptureCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.m_numCaptureCount.ValueChanged += new System.EventHandler(this.m_numCaptureCount_ValueChanged);
            this.m_numCaptureCount.Validating += new System.ComponentModel.CancelEventHandler(this.m_numCaptureCount_Validating);
            this.m_numCaptureCount.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_numCaptureCount_KeyUp);
            // 
            // m_lblNumEdges
            // 
            this.m_lblNumEdges.AutoSize = true;
            this.m_lblNumEdges.Location = new System.Drawing.Point(284, 135);
            this.m_lblNumEdges.Name = "m_lblNumEdges";
            this.m_lblNumEdges.Size = new System.Drawing.Size(0, 13);
            this.m_lblNumEdges.TabIndex = 157;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(21, 213);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 2);
            this.groupBox1.TabIndex = 159;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(22, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 2);
            this.groupBox2.TabIndex = 160;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(22, 103);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(390, 2);
            this.groupBox3.TabIndex = 161;
            this.groupBox3.TabStop = false;
            // 
            // m_cbEnableMode
            // 
            this.m_cbEnableMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbEnableMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbEnableMode.FormattingEnabled = true;
            this.m_cbEnableMode.Location = new System.Drawing.Point(128, 159);
            this.m_cbEnableMode.Name = "m_cbEnableMode";
            this.m_cbEnableMode.Size = new System.Drawing.Size(283, 21);
            this.m_cbEnableMode.TabIndex = 51;
            this.m_cbEnableMode.SelectedIndexChanged += new System.EventHandler(this.m_cbEnableMode_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Location = new System.Drawing.Point(22, 155);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(390, 2);
            this.groupBox4.TabIndex = 161;
            this.groupBox4.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_rbResolution8);
            this.panel1.Controls.Add(this.m_rbResolution24);
            this.panel1.Controls.Add(this.m_rbResolution16);
            this.panel1.Controls.Add(this.m_rbResolution32);
            this.panel1.Location = new System.Drawing.Point(119, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 32);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.m_rbUDB);
            this.panel2.Controls.Add(this.m_rbFixedFunction);
            this.panel2.Location = new System.Drawing.Point(116, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 32);
            this.panel2.TabIndex = 10;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Location = new System.Drawing.Point(22, 26);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(390, 2);
            this.groupBox5.TabIndex = 165;
            this.groupBox5.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Location = new System.Drawing.Point(22, 48);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(390, 2);
            this.groupBox6.TabIndex = 166;
            this.groupBox6.TabStop = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Location = new System.Drawing.Point(22, 76);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(390, 2);
            this.groupBox7.TabIndex = 167;
            this.groupBox7.TabStop = false;
            // 
            // m_chbxIntSrcFifoFull
            // 
            this.m_chbxIntSrcFifoFull.AutoSize = true;
            this.m_chbxIntSrcFifoFull.Location = new System.Drawing.Point(129, 236);
            this.m_chbxIntSrcFifoFull.Name = "m_chbxIntSrcFifoFull";
            this.m_chbxIntSrcFifoFull.Size = new System.Drawing.Size(85, 17);
            this.m_chbxIntSrcFifoFull.TabIndex = 168;
            this.m_chbxIntSrcFifoFull.Text = "On FIFO Full";
            this.m_chbxIntSrcFifoFull.UseVisualStyleBackColor = true;
            this.m_chbxIntSrcFifoFull.CheckedChanged += new System.EventHandler(this.m_chbxIntSrcFifoFull_CheckedChanged);
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // CyTimerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_chbxIntSrcFifoFull);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.m_lblEnableMode);
            this.Controls.Add(this.m_cbEnableMode);
            this.Controls.Add(this.m_bMaxPeriod);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_chbxEnableCaptureCounter);
            this.Controls.Add(this.m_lblNumEdges);
            this.Controls.Add(this.m_numInterruptOnCaptureCount);
            this.Controls.Add(this.m_chbxIntSrcTC);
            this.Controls.Add(this.m_chbxIntSrcCapture);
            this.Controls.Add(this.m_lblInterruptSources);
            this.Controls.Add(this.m_lblCalcFrequency);
            this.Controls.Add(this.m_lblPeriod);
            this.Controls.Add(this.m_numPeriod);
            this.Controls.Add(this.m_numCaptureCount);
            this.Controls.Add(this.m_cbRunMode);
            this.Controls.Add(this.m_lblRunMode);
            this.Controls.Add(this.m_lblCaptureMode);
            this.Controls.Add(this.m_cbCaptureMode);
            this.Controls.Add(this.m_lblTrigger);
            this.Controls.Add(this.m_cbTriggerMode);
            this.Controls.Add(this.m_lblImplementation);
            this.Controls.Add(this.m_lblResolution);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CyTimerControl";
            this.Size = new System.Drawing.Size(429, 254);
            ((System.ComponentModel.ISupportInitialize)(this.m_numInterruptOnCaptureCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numCaptureCount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label m_lblResolution;
        private System.Windows.Forms.RadioButton m_rbUDB;
        private System.Windows.Forms.RadioButton m_rbFixedFunction;
        private System.Windows.Forms.Label m_lblImplementation;
        private System.Windows.Forms.Label m_lblCaptureMode;
        private System.Windows.Forms.ComboBox m_cbCaptureMode;
        private System.Windows.Forms.Label m_lblTrigger;
        private System.Windows.Forms.ComboBox m_cbTriggerMode;
        private System.Windows.Forms.ComboBox m_cbRunMode;
        private System.Windows.Forms.Label m_lblRunMode;
        private CyNumericUpDown m_numCaptureCount;
        private System.Windows.Forms.Label m_lblCalcFrequency;
        private System.Windows.Forms.Label m_lblPeriod;
        private CyNumericUpDown m_numPeriod;
        private System.Windows.Forms.Label m_lblInterruptSources;
        private System.Windows.Forms.CheckBox m_chbxIntSrcCapture;
        private System.Windows.Forms.CheckBox m_chbxIntSrcTC;
        private CyNumericUpDown m_numInterruptOnCaptureCount;
        private System.Windows.Forms.ToolTip m_tooltips;
        private System.Windows.Forms.Label m_lblNumEdges;
        private System.Windows.Forms.CheckBox m_chbxEnableCaptureCounter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton m_rbResolution24;
        private System.Windows.Forms.RadioButton m_rbResolution16;
        private System.Windows.Forms.RadioButton m_rbResolution32;
        private System.Windows.Forms.Button m_bMaxPeriod;
        private System.Windows.Forms.Label m_lblEnableMode;
        private System.Windows.Forms.ComboBox m_cbEnableMode;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton m_rbResolution8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox m_chbxIntSrcFifoFull;
        private System.Windows.Forms.ErrorProvider ep_Errors;
    }
}
