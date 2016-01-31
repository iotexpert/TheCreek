namespace USBFS_v2_11
{
    partial class CyDetailsEndpoint
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
            this.numUpDownMaxPacketSize = new System.Windows.Forms.NumericUpDown();
            this.numUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.comboBoxUsageType = new System.Windows.Forms.ComboBox();
            this.labelUsageType = new System.Windows.Forms.Label();
            this.comboBoxSynchType = new System.Windows.Forms.ComboBox();
            this.labelSynchType = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxEndpointNum = new System.Windows.Forms.ComboBox();
            this.comboBoxTransferType = new System.Windows.Forms.ComboBox();
            this.comboBoxDirection = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxDoubleBuffer = new System.Windows.Forms.CheckBox();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownMaxPacketSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxParams.Controls.Add(this.numUpDownMaxPacketSize);
            this.groupBoxParams.Controls.Add(this.numUpDownInterval);
            this.groupBoxParams.Controls.Add(this.comboBoxUsageType);
            this.groupBoxParams.Controls.Add(this.labelUsageType);
            this.groupBoxParams.Controls.Add(this.comboBoxSynchType);
            this.groupBoxParams.Controls.Add(this.labelSynchType);
            this.groupBoxParams.Controls.Add(this.label5);
            this.groupBoxParams.Controls.Add(this.label2);
            this.groupBoxParams.Controls.Add(this.comboBoxEndpointNum);
            this.groupBoxParams.Controls.Add(this.comboBoxTransferType);
            this.groupBoxParams.Controls.Add(this.comboBoxDirection);
            this.groupBoxParams.Controls.Add(this.label4);
            this.groupBoxParams.Controls.Add(this.label3);
            this.groupBoxParams.Controls.Add(this.label1);
            this.groupBoxParams.Location = new System.Drawing.Point(0, 9);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Size = new System.Drawing.Size(304, 211);
            this.groupBoxParams.TabIndex = 2;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Endpoint Attributes";
            // 
            // numUpDownMaxPacketSize
            // 
            this.numUpDownMaxPacketSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numUpDownMaxPacketSize.Location = new System.Drawing.Point(120, 182);
            this.numUpDownMaxPacketSize.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numUpDownMaxPacketSize.Name = "numUpDownMaxPacketSize";
            this.numUpDownMaxPacketSize.Size = new System.Drawing.Size(120, 20);
            this.numUpDownMaxPacketSize.TabIndex = 6;
            this.numUpDownMaxPacketSize.Validated += new System.EventHandler(this.numUpDownMaxPacketSize_Validated);
            // 
            // numUpDownInterval
            // 
            this.numUpDownInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numUpDownInterval.Location = new System.Drawing.Point(120, 156);
            this.numUpDownInterval.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUpDownInterval.Name = "numUpDownInterval";
            this.numUpDownInterval.Size = new System.Drawing.Size(120, 20);
            this.numUpDownInterval.TabIndex = 5;
            this.numUpDownInterval.Validated += new System.EventHandler(this.numUpDownInterval_Validated);
            // 
            // comboBoxUsageType
            // 
            this.comboBoxUsageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUsageType.FormattingEnabled = true;
            this.comboBoxUsageType.Items.AddRange(new object[] {
            "Data endpoint",
            "Feedback endpoint",
            "Implicit feedback Data endpoint"});
            this.comboBoxUsageType.Location = new System.Drawing.Point(119, 127);
            this.comboBoxUsageType.Name = "comboBoxUsageType";
            this.comboBoxUsageType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxUsageType.TabIndex = 4;
            this.comboBoxUsageType.Validated += new System.EventHandler(this.comboBoxTransferType_Validated);
            // 
            // labelUsageType
            // 
            this.labelUsageType.AutoSize = true;
            this.labelUsageType.Location = new System.Drawing.Point(6, 130);
            this.labelUsageType.Name = "labelUsageType";
            this.labelUsageType.Size = new System.Drawing.Size(65, 13);
            this.labelUsageType.TabIndex = 16;
            this.labelUsageType.Text = "Usage Type";
            // 
            // comboBoxSynchType
            // 
            this.comboBoxSynchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSynchType.FormattingEnabled = true;
            this.comboBoxSynchType.Items.AddRange(new object[] {
            "No synchronization",
            "Asynchronous",
            "Adaptive",
            "Synchronous"});
            this.comboBoxSynchType.Location = new System.Drawing.Point(119, 100);
            this.comboBoxSynchType.Name = "comboBoxSynchType";
            this.comboBoxSynchType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSynchType.TabIndex = 3;
            this.comboBoxSynchType.Validated += new System.EventHandler(this.comboBoxTransferType_Validated);
            // 
            // labelSynchType
            // 
            this.labelSynchType.AutoSize = true;
            this.labelSynchType.Location = new System.Drawing.Point(6, 103);
            this.labelSynchType.Name = "labelSynchType";
            this.labelSynchType.Size = new System.Drawing.Size(64, 13);
            this.labelSynchType.TabIndex = 14;
            this.labelSynchType.Text = "Synch Type";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Interval";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Max Packet Size";
            // 
            // comboBoxEndpointNum
            // 
            this.comboBoxEndpointNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEndpointNum.FormattingEnabled = true;
            this.comboBoxEndpointNum.Location = new System.Drawing.Point(119, 19);
            this.comboBoxEndpointNum.Name = "comboBoxEndpointNum";
            this.comboBoxEndpointNum.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEndpointNum.TabIndex = 0;
            this.comboBoxEndpointNum.SelectedIndexChanged += new System.EventHandler(this.comboBoxEndpointNum_Validated);
            this.comboBoxEndpointNum.Validated += new System.EventHandler(this.comboBoxEndpointNum_Validated);
            // 
            // comboBoxTransferType
            // 
            this.comboBoxTransferType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTransferType.FormattingEnabled = true;
            this.comboBoxTransferType.Items.AddRange(new object[] {
            "CONT",
            "INT",
            "BULK",
            "ISOC"});
            this.comboBoxTransferType.Location = new System.Drawing.Point(119, 73);
            this.comboBoxTransferType.Name = "comboBoxTransferType";
            this.comboBoxTransferType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTransferType.TabIndex = 2;
            this.comboBoxTransferType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTransferType_SelectedIndexChanged);
            this.comboBoxTransferType.Validated += new System.EventHandler(this.comboBoxTransferType_Validated);
            // 
            // comboBoxDirection
            // 
            this.comboBoxDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDirection.FormattingEnabled = true;
            this.comboBoxDirection.Items.AddRange(new object[] {
            "IN",
            "OUT"});
            this.comboBoxDirection.Location = new System.Drawing.Point(119, 46);
            this.comboBoxDirection.Name = "comboBoxDirection";
            this.comboBoxDirection.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDirection.TabIndex = 1;
            this.comboBoxDirection.SelectedIndexChanged += new System.EventHandler(this.comboBoxEndpointNum_Validated);
            this.comboBoxDirection.Validated += new System.EventHandler(this.comboBoxEndpointNum_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Transfer Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Direction";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Endpoint Number";
            // 
            // checkBoxDoubleBuffer
            // 
            this.checkBoxDoubleBuffer.AutoSize = true;
            this.checkBoxDoubleBuffer.Location = new System.Drawing.Point(6, 226);
            this.checkBoxDoubleBuffer.Name = "checkBoxDoubleBuffer";
            this.checkBoxDoubleBuffer.Size = new System.Drawing.Size(91, 17);
            this.checkBoxDoubleBuffer.TabIndex = 7;
            this.checkBoxDoubleBuffer.Text = "Double Buffer";
            this.checkBoxDoubleBuffer.UseVisualStyleBackColor = true;
            this.checkBoxDoubleBuffer.Visible = false;
            this.checkBoxDoubleBuffer.CheckedChanged += new System.EventHandler(this.checkBoxDoubleBuffer_CheckedChanged);
            // 
            // errProvider
            // 
            this.errProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errProvider.ContainerControl = this;
            // 
            // CyDetailsEndpoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.checkBoxDoubleBuffer);
            this.Controls.Add(this.groupBoxParams);
            this.Name = "CyDetailsEndpoint";
            this.Size = new System.Drawing.Size(307, 258);
            this.groupBoxParams.ResumeLayout(false);
            this.groupBoxParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownMaxPacketSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.ComboBox comboBoxEndpointNum;
        private System.Windows.Forms.ComboBox comboBoxTransferType;
        private System.Windows.Forms.ComboBox comboBoxDirection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxUsageType;
        private System.Windows.Forms.Label labelUsageType;
        private System.Windows.Forms.ComboBox comboBoxSynchType;
        private System.Windows.Forms.Label labelSynchType;
        private System.Windows.Forms.CheckBox checkBoxDoubleBuffer;
        private System.Windows.Forms.NumericUpDown numUpDownInterval;
        private System.Windows.Forms.NumericUpDown numUpDownMaxPacketSize;
        private System.Windows.Forms.ErrorProvider errProvider;
    }
}
