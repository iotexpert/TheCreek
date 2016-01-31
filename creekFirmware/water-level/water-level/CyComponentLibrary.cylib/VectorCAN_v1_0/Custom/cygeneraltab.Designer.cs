namespace VectorCAN_v1_0
{
    partial class CyGeneralTab
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
            this.cbAddTransceiverEnableSignal = new System.Windows.Forms.CheckBox();
            this.cbEnableInterrupts = new System.Windows.Forms.CheckBox();
            this.lclockFr = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbAddTransceiverEnableSignal
            // 
            this.cbAddTransceiverEnableSignal.AutoSize = true;
            this.cbAddTransceiverEnableSignal.Checked = true;
            this.cbAddTransceiverEnableSignal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAddTransceiverEnableSignal.Location = new System.Drawing.Point(3, 74);
            this.cbAddTransceiverEnableSignal.Name = "cbAddTransceiverEnableSignal";
            this.cbAddTransceiverEnableSignal.Size = new System.Drawing.Size(172, 17);
            this.cbAddTransceiverEnableSignal.TabIndex = 0;
            this.cbAddTransceiverEnableSignal.Text = "Add Transceiver Enable Signal";
            this.cbAddTransceiverEnableSignal.UseVisualStyleBackColor = true;
            // 
            // cbEnableInterrupts
            // 
            this.cbEnableInterrupts.AutoSize = true;
            this.cbEnableInterrupts.Checked = true;
            this.cbEnableInterrupts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableInterrupts.Location = new System.Drawing.Point(3, 97);
            this.cbEnableInterrupts.Name = "cbEnableInterrupts";
            this.cbEnableInterrupts.Size = new System.Drawing.Size(106, 17);
            this.cbEnableInterrupts.TabIndex = 0;
            this.cbEnableInterrupts.Text = "Enable Interrupts";
            this.cbEnableInterrupts.UseVisualStyleBackColor = true;
            // 
            // lclockFr
            // 
            this.lclockFr.AutoSize = true;
            this.lclockFr.Location = new System.Drawing.Point(6, 47);
            this.lclockFr.Name = "lclockFr";
            this.lclockFr.Size = new System.Drawing.Size(122, 13);
            this.lclockFr.TabIndex = 0;
            this.lclockFr.Text = "Clock Frequency (KHz): ";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lclockFr);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 65);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CAN System Clock";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "This is the CAN system clock internal to PSoC. This value must match the settings" +
                " in the Vector driver configuration.";
            // 
            // CyGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbAddTransceiverEnableSignal);
            this.Controls.Add(this.cbEnableInterrupts);
            this.Name = "CyGeneralTab";
            this.Size = new System.Drawing.Size(431, 351);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbAddTransceiverEnableSignal;
        private System.Windows.Forms.Label lclockFr;
        private System.Windows.Forms.CheckBox cbEnableInterrupts;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}
