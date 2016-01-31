namespace I2S_v2_20
{
    partial class CyI2SAdvanced
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
            this.groupBoxISTX = new System.Windows.Forms.GroupBox();
            this.checkBoxTxFIFO1 = new System.Windows.Forms.CheckBox();
            this.checkBoxTxFIFO0 = new System.Windows.Forms.CheckBox();
            this.checkBoxISTxUnderflow = new System.Windows.Forms.CheckBox();
            this.groupBoxISRX = new System.Windows.Forms.GroupBox();
            this.checkBoxRxFIFO1 = new System.Windows.Forms.CheckBox();
            this.checkBoxRxFIFO0 = new System.Windows.Forms.CheckBox();
            this.checkBoxISRxOverflow = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBoxDMARequest = new System.Windows.Forms.GroupBox();
            this.checkBoxTxDMA = new System.Windows.Forms.CheckBox();
            this.checkBoxRxDMA = new System.Windows.Forms.CheckBox();
            this.groupBoxDITX = new System.Windows.Forms.GroupBox();
            this.radioButtonTxSeparatedLR = new System.Windows.Forms.RadioButton();
            this.radioButtonTxInterleaved = new System.Windows.Forms.RadioButton();
            this.groupBoxDIRX = new System.Windows.Forms.GroupBox();
            this.radioButtonRxSeparatedLR = new System.Windows.Forms.RadioButton();
            this.radioButtonRxInterleaved = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBoxISTX.SuspendLayout();
            this.groupBoxISRX.SuspendLayout();
            this.groupBoxDMARequest.SuspendLayout();
            this.groupBoxDITX.SuspendLayout();
            this.groupBoxDIRX.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxISTX
            // 
            this.groupBoxISTX.Controls.Add(this.checkBoxTxFIFO1);
            this.groupBoxISTX.Controls.Add(this.checkBoxTxFIFO0);
            this.groupBoxISTX.Controls.Add(this.checkBoxISTxUnderflow);
            this.groupBoxISTX.Location = new System.Drawing.Point(216, 162);
            this.groupBoxISTX.Name = "groupBoxISTX";
            this.groupBoxISTX.Size = new System.Drawing.Size(182, 76);
            this.groupBoxISTX.TabIndex = 4;
            this.groupBoxISTX.TabStop = false;
            this.groupBoxISTX.Text = "TX";
            // 
            // checkBoxTxFIFO1
            // 
            this.checkBoxTxFIFO1.AutoSize = true;
            this.checkBoxTxFIFO1.Location = new System.Drawing.Point(16, 53);
            this.checkBoxTxFIFO1.Name = "checkBoxTxFIFO1";
            this.checkBoxTxFIFO1.Size = new System.Drawing.Size(107, 17);
            this.checkBoxTxFIFO1.TabIndex = 11;
            this.checkBoxTxFIFO1.Text = "Tx FIFO 1 not full";
            this.checkBoxTxFIFO1.UseVisualStyleBackColor = true;
            this.checkBoxTxFIFO1.CheckedChanged += new System.EventHandler(this.checkBoxInterruptSource_CheckedChanged);
            // 
            // checkBoxTxFIFO0
            // 
            this.checkBoxTxFIFO0.AutoSize = true;
            this.checkBoxTxFIFO0.Location = new System.Drawing.Point(16, 34);
            this.checkBoxTxFIFO0.Name = "checkBoxTxFIFO0";
            this.checkBoxTxFIFO0.Size = new System.Drawing.Size(107, 17);
            this.checkBoxTxFIFO0.TabIndex = 10;
            this.checkBoxTxFIFO0.Text = "Tx FIFO 0 not full";
            this.checkBoxTxFIFO0.UseVisualStyleBackColor = true;
            this.checkBoxTxFIFO0.CheckedChanged += new System.EventHandler(this.checkBoxInterruptSource_CheckedChanged);
            // 
            // checkBoxISTxUnderflow
            // 
            this.checkBoxISTxUnderflow.AutoSize = true;
            this.checkBoxISTxUnderflow.Location = new System.Drawing.Point(16, 15);
            this.checkBoxISTxUnderflow.Name = "checkBoxISTxUnderflow";
            this.checkBoxISTxUnderflow.Size = new System.Drawing.Size(89, 17);
            this.checkBoxISTxUnderflow.TabIndex = 9;
            this.checkBoxISTxUnderflow.Text = "Tx Underflow";
            this.checkBoxISTxUnderflow.UseVisualStyleBackColor = true;
            this.checkBoxISTxUnderflow.CheckedChanged += new System.EventHandler(this.checkBoxInterruptSource_CheckedChanged);
            // 
            // groupBoxISRX
            // 
            this.groupBoxISRX.Controls.Add(this.checkBoxRxFIFO1);
            this.groupBoxISRX.Controls.Add(this.checkBoxRxFIFO0);
            this.groupBoxISRX.Controls.Add(this.checkBoxISRxOverflow);
            this.groupBoxISRX.Location = new System.Drawing.Point(26, 162);
            this.groupBoxISRX.Name = "groupBoxISRX";
            this.groupBoxISRX.Size = new System.Drawing.Size(182, 76);
            this.groupBoxISRX.TabIndex = 3;
            this.groupBoxISRX.TabStop = false;
            this.groupBoxISRX.Text = "RX";
            // 
            // checkBoxRxFIFO1
            // 
            this.checkBoxRxFIFO1.AutoSize = true;
            this.checkBoxRxFIFO1.Location = new System.Drawing.Point(16, 53);
            this.checkBoxRxFIFO1.Name = "checkBoxRxFIFO1";
            this.checkBoxRxFIFO1.Size = new System.Drawing.Size(123, 17);
            this.checkBoxRxFIFO1.TabIndex = 8;
            this.checkBoxRxFIFO1.Text = "Rx FIFO 1 not empty";
            this.checkBoxRxFIFO1.UseVisualStyleBackColor = true;
            this.checkBoxRxFIFO1.CheckedChanged += new System.EventHandler(this.checkBoxInterruptSource_CheckedChanged);
            // 
            // checkBoxRxFIFO0
            // 
            this.checkBoxRxFIFO0.AutoSize = true;
            this.checkBoxRxFIFO0.Location = new System.Drawing.Point(16, 34);
            this.checkBoxRxFIFO0.Name = "checkBoxRxFIFO0";
            this.checkBoxRxFIFO0.Size = new System.Drawing.Size(123, 17);
            this.checkBoxRxFIFO0.TabIndex = 7;
            this.checkBoxRxFIFO0.Text = "Rx FIFO 0 not empty";
            this.checkBoxRxFIFO0.UseVisualStyleBackColor = true;
            this.checkBoxRxFIFO0.CheckedChanged += new System.EventHandler(this.checkBoxInterruptSource_CheckedChanged);
            // 
            // checkBoxISRxOverflow
            // 
            this.checkBoxISRxOverflow.AutoSize = true;
            this.checkBoxISRxOverflow.Location = new System.Drawing.Point(16, 15);
            this.checkBoxISRxOverflow.Name = "checkBoxISRxOverflow";
            this.checkBoxISRxOverflow.Size = new System.Drawing.Size(84, 17);
            this.checkBoxISRxOverflow.TabIndex = 6;
            this.checkBoxISRxOverflow.Text = "Rx Overflow";
            this.checkBoxISRxOverflow.UseVisualStyleBackColor = true;
            this.checkBoxISRxOverflow.CheckedChanged += new System.EventHandler(this.checkBoxInterruptSource_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 145);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Interrupt Source";
            // 
            // groupBoxDMARequest
            // 
            this.groupBoxDMARequest.Controls.Add(this.checkBoxTxDMA);
            this.groupBoxDMARequest.Controls.Add(this.checkBoxRxDMA);
            this.groupBoxDMARequest.Location = new System.Drawing.Point(26, 92);
            this.groupBoxDMARequest.Name = "groupBoxDMARequest";
            this.groupBoxDMARequest.Size = new System.Drawing.Size(372, 46);
            this.groupBoxDMARequest.TabIndex = 2;
            this.groupBoxDMARequest.TabStop = false;
            this.groupBoxDMARequest.Text = "DMA Request";
            // 
            // checkBoxTxDMA
            // 
            this.checkBoxTxDMA.AutoSize = true;
            this.checkBoxTxDMA.Location = new System.Drawing.Point(207, 19);
            this.checkBoxTxDMA.Name = "checkBoxTxDMA";
            this.checkBoxTxDMA.Size = new System.Drawing.Size(65, 17);
            this.checkBoxTxDMA.TabIndex = 5;
            this.checkBoxTxDMA.Text = "Tx DMA";
            this.checkBoxTxDMA.UseVisualStyleBackColor = true;
            this.checkBoxTxDMA.CheckedChanged += new System.EventHandler(this.checkBoxTxDMA_CheckedChanged);
            // 
            // checkBoxRxDMA
            // 
            this.checkBoxRxDMA.AutoSize = true;
            this.checkBoxRxDMA.Location = new System.Drawing.Point(16, 20);
            this.checkBoxRxDMA.Name = "checkBoxRxDMA";
            this.checkBoxRxDMA.Size = new System.Drawing.Size(66, 17);
            this.checkBoxRxDMA.TabIndex = 4;
            this.checkBoxRxDMA.Text = "Rx DMA";
            this.checkBoxRxDMA.UseVisualStyleBackColor = true;
            this.checkBoxRxDMA.CheckedChanged += new System.EventHandler(this.checkBoxRxDMA_CheckedChanged);
            // 
            // groupBoxDITX
            // 
            this.groupBoxDITX.Controls.Add(this.radioButtonTxSeparatedLR);
            this.groupBoxDITX.Controls.Add(this.radioButtonTxInterleaved);
            this.groupBoxDITX.Location = new System.Drawing.Point(216, 27);
            this.groupBoxDITX.Name = "groupBoxDITX";
            this.groupBoxDITX.Size = new System.Drawing.Size(182, 60);
            this.groupBoxDITX.TabIndex = 1;
            this.groupBoxDITX.TabStop = false;
            this.groupBoxDITX.Text = "TX";
            // 
            // radioButtonTxSeparatedLR
            // 
            this.radioButtonTxSeparatedLR.AutoSize = true;
            this.radioButtonTxSeparatedLR.Location = new System.Drawing.Point(17, 34);
            this.radioButtonTxSeparatedLR.Name = "radioButtonTxSeparatedLR";
            this.radioButtonTxSeparatedLR.Size = new System.Drawing.Size(96, 17);
            this.radioButtonTxSeparatedLR.TabIndex = 3;
            this.radioButtonTxSeparatedLR.TabStop = true;
            this.radioButtonTxSeparatedLR.Text = "Separated L/R";
            this.radioButtonTxSeparatedLR.UseVisualStyleBackColor = true;
            // 
            // radioButtonTxInterleaved
            // 
            this.radioButtonTxInterleaved.AutoSize = true;
            this.radioButtonTxInterleaved.Location = new System.Drawing.Point(17, 15);
            this.radioButtonTxInterleaved.Name = "radioButtonTxInterleaved";
            this.radioButtonTxInterleaved.Size = new System.Drawing.Size(78, 17);
            this.radioButtonTxInterleaved.TabIndex = 2;
            this.radioButtonTxInterleaved.TabStop = true;
            this.radioButtonTxInterleaved.Text = "Interleaved";
            this.radioButtonTxInterleaved.UseVisualStyleBackColor = true;
            this.radioButtonTxInterleaved.CheckedChanged += new System.EventHandler(this.radioButtonTxInterleaved_CheckedChanged);
            // 
            // groupBoxDIRX
            // 
            this.groupBoxDIRX.Controls.Add(this.radioButtonRxSeparatedLR);
            this.groupBoxDIRX.Controls.Add(this.radioButtonRxInterleaved);
            this.groupBoxDIRX.Location = new System.Drawing.Point(26, 27);
            this.groupBoxDIRX.Name = "groupBoxDIRX";
            this.groupBoxDIRX.Size = new System.Drawing.Size(182, 60);
            this.groupBoxDIRX.TabIndex = 0;
            this.groupBoxDIRX.TabStop = false;
            this.groupBoxDIRX.Text = "RX";
            // 
            // radioButtonRxSeparatedLR
            // 
            this.radioButtonRxSeparatedLR.AutoSize = true;
            this.radioButtonRxSeparatedLR.Location = new System.Drawing.Point(17, 34);
            this.radioButtonRxSeparatedLR.Name = "radioButtonRxSeparatedLR";
            this.radioButtonRxSeparatedLR.Size = new System.Drawing.Size(96, 17);
            this.radioButtonRxSeparatedLR.TabIndex = 1;
            this.radioButtonRxSeparatedLR.TabStop = true;
            this.radioButtonRxSeparatedLR.Text = "Separated L/R";
            this.radioButtonRxSeparatedLR.UseVisualStyleBackColor = true;
            // 
            // radioButtonRxInterleaved
            // 
            this.radioButtonRxInterleaved.AutoSize = true;
            this.radioButtonRxInterleaved.Location = new System.Drawing.Point(17, 15);
            this.radioButtonRxInterleaved.Name = "radioButtonRxInterleaved";
            this.radioButtonRxInterleaved.Size = new System.Drawing.Size(78, 17);
            this.radioButtonRxInterleaved.TabIndex = 0;
            this.radioButtonRxInterleaved.TabStop = true;
            this.radioButtonRxInterleaved.Text = "Interleaved";
            this.radioButtonRxInterleaved.UseVisualStyleBackColor = true;
            this.radioButtonRxInterleaved.CheckedChanged += new System.EventHandler(this.radioButtonRxInterleaved_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Data Interleaving";
            // 
            // cyi2sadvancedtab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBoxISTX);
            this.Controls.Add(this.groupBoxISRX);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBoxDMARequest);
            this.Controls.Add(this.groupBoxDITX);
            this.Controls.Add(this.groupBoxDIRX);
            this.Controls.Add(this.label8);
            this.Name = "cyi2sadvancedtab";
            this.Size = new System.Drawing.Size(472, 265);
            this.groupBoxISTX.ResumeLayout(false);
            this.groupBoxISTX.PerformLayout();
            this.groupBoxISRX.ResumeLayout(false);
            this.groupBoxISRX.PerformLayout();
            this.groupBoxDMARequest.ResumeLayout(false);
            this.groupBoxDMARequest.PerformLayout();
            this.groupBoxDITX.ResumeLayout(false);
            this.groupBoxDITX.PerformLayout();
            this.groupBoxDIRX.ResumeLayout(false);
            this.groupBoxDIRX.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBoxISTX;
        public System.Windows.Forms.CheckBox checkBoxTxFIFO1;
        public System.Windows.Forms.CheckBox checkBoxTxFIFO0;
        public System.Windows.Forms.CheckBox checkBoxISTxUnderflow;
        public System.Windows.Forms.GroupBox groupBoxISRX;
        public System.Windows.Forms.CheckBox checkBoxRxFIFO1;
        public System.Windows.Forms.CheckBox checkBoxRxFIFO0;
        public System.Windows.Forms.CheckBox checkBoxISRxOverflow;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.GroupBox groupBoxDMARequest;
        public System.Windows.Forms.CheckBox checkBoxTxDMA;
        public System.Windows.Forms.CheckBox checkBoxRxDMA;
        public System.Windows.Forms.GroupBox groupBoxDITX;
        public System.Windows.Forms.RadioButton radioButtonTxSeparatedLR;
        public System.Windows.Forms.RadioButton radioButtonTxInterleaved;
        public System.Windows.Forms.GroupBox groupBoxDIRX;
        public System.Windows.Forms.RadioButton radioButtonRxSeparatedLR;
        public System.Windows.Forms.RadioButton radioButtonRxInterleaved;
        private System.Windows.Forms.Label label8;
    }
}
