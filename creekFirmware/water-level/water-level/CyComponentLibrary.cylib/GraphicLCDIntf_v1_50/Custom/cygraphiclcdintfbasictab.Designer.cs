namespace GraphicLCDIntf_v1_50
{
    partial class CyGraphicLCDIntfBasicTab
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
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.numReadHighPulseWidthTime = new System.Windows.Forms.NumericUpDown();
            this.numReadLowPulseWidthTime = new System.Windows.Forms.NumericUpDown();
            this.panelTop = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb16Bit = new System.Windows.Forms.RadioButton();
            this.rb8Bit = new System.Windows.Forms.RadioButton();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadHighPulseWidthTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadLowPulseWidthTime)).BeginInit();
            this.panelTop.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // numReadHighPulseWidthTime
            // 
            this.errorProvider.SetIconAlignment(this.numReadHighPulseWidthTime, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.numReadHighPulseWidthTime.Location = new System.Drawing.Point(218, 45);
            this.numReadHighPulseWidthTime.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numReadHighPulseWidthTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReadHighPulseWidthTime.Name = "numReadHighPulseWidthTime";
            this.numReadHighPulseWidthTime.Size = new System.Drawing.Size(76, 20);
            this.numReadHighPulseWidthTime.TabIndex = 26;
            this.numReadHighPulseWidthTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numReadLowPulseWidthTime
            // 
            this.errorProvider.SetIconAlignment(this.numReadLowPulseWidthTime, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.numReadLowPulseWidthTime.Location = new System.Drawing.Point(218, 22);
            this.numReadLowPulseWidthTime.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numReadLowPulseWidthTime.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numReadLowPulseWidthTime.Name = "numReadLowPulseWidthTime";
            this.numReadLowPulseWidthTime.Size = new System.Drawing.Size(76, 20);
            this.numReadLowPulseWidthTime.TabIndex = 25;
            this.numReadLowPulseWidthTime.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.groupBox2);
            this.panelTop.Controls.Add(this.groupBox1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(521, 97);
            this.panelTop.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numReadHighPulseWidthTime);
            this.groupBox2.Controls.Add(this.numReadLowPulseWidthTime);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(107, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 80);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Read Transaction settings";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(199, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "High Pulse Width Time (in Clock Cycles):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(197, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Low Pulse Width Time (in Clock Cycles):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb16Bit);
            this.groupBox1.Controls.Add(this.rb8Bit);
            this.groupBox1.Location = new System.Drawing.Point(3, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(92, 80);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bus Width";
            // 
            // rb16Bit
            // 
            this.rb16Bit.AutoSize = true;
            this.rb16Bit.Location = new System.Drawing.Point(13, 47);
            this.rb16Bit.Name = "rb16Bit";
            this.rb16Bit.Size = new System.Drawing.Size(51, 17);
            this.rb16Bit.TabIndex = 3;
            this.rb16Bit.TabStop = true;
            this.rb16Bit.Text = "16 bit";
            this.rb16Bit.UseVisualStyleBackColor = true;
            // 
            // rb8Bit
            // 
            this.rb8Bit.AutoSize = true;
            this.rb8Bit.Location = new System.Drawing.Point(13, 24);
            this.rb8Bit.Name = "rb8Bit";
            this.rb8Bit.Size = new System.Drawing.Size(45, 17);
            this.rb8Bit.TabIndex = 2;
            this.rb8Bit.TabStop = true;
            this.rb8Bit.Text = "8 bit";
            this.rb8Bit.UseVisualStyleBackColor = true;
            this.rb8Bit.CheckedChanged += new System.EventHandler(this.rb8Bit_CheckedChanged);
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.pictureBox);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 97);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(521, 209);
            this.panelMiddle.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(521, 209);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // CyGraphicLCDIntfBasicTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelTop);
            this.Name = "CyGraphicLCDIntfBasicTab";
            this.Size = new System.Drawing.Size(521, 306);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadHighPulseWidthTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadLowPulseWidthTime)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb16Bit;
        private System.Windows.Forms.RadioButton rb8Bit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numReadHighPulseWidthTime;
        private System.Windows.Forms.NumericUpDown numReadLowPulseWidthTime;
    }
}
