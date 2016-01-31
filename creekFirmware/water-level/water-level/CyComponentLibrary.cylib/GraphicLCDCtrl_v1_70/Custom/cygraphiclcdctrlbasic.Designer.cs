namespace GraphicLCDCtrl_v1_70
{
    partial class CyGraphicLCDCtrlBasic
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
            this.gbSyncPulsePolarity = new System.Windows.Forms.GroupBox();
            this.rbActiveLow = new System.Windows.Forms.RadioButton();
            this.rbActiveHigh = new System.Windows.Forms.RadioButton();
            this.gbTransitionDotclkEdge = new System.Windows.Forms.GroupBox();
            this.rbRisingEdge = new System.Windows.Forms.RadioButton();
            this.rbFallingEdge = new System.Windows.Forms.RadioButton();
            this.gbHorizontalTiming = new System.Windows.Forms.GroupBox();
            this.numHFrontPorch = new System.Windows.Forms.NumericUpDown();
            this.numHActiveRegion = new System.Windows.Forms.NumericUpDown();
            this.numHBackPorch = new System.Windows.Forms.NumericUpDown();
            this.numHSyncWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbVerticalTiming = new System.Windows.Forms.GroupBox();
            this.numVFrontPorch = new System.Windows.Forms.NumericUpDown();
            this.numVActiveRegion = new System.Windows.Forms.NumericUpDown();
            this.numVBackPorch = new System.Windows.Forms.NumericUpDown();
            this.numVSyncWidth = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbVHBlanking = new System.Windows.Forms.RadioButton();
            this.rbVBlanking = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbSyncPulsePolarity.SuspendLayout();
            this.gbTransitionDotclkEdge.SuspendLayout();
            this.gbHorizontalTiming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHFrontPorch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHActiveRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHBackPorch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHSyncWidth)).BeginInit();
            this.gbVerticalTiming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVFrontPorch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVActiveRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVBackPorch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVSyncWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSyncPulsePolarity
            // 
            this.gbSyncPulsePolarity.Controls.Add(this.rbActiveLow);
            this.gbSyncPulsePolarity.Controls.Add(this.rbActiveHigh);
            this.gbSyncPulsePolarity.Location = new System.Drawing.Point(3, 5);
            this.gbSyncPulsePolarity.Name = "gbSyncPulsePolarity";
            this.gbSyncPulsePolarity.Size = new System.Drawing.Size(230, 64);
            this.gbSyncPulsePolarity.TabIndex = 0;
            this.gbSyncPulsePolarity.TabStop = false;
            this.gbSyncPulsePolarity.Text = "Sync Pulse Polarity";
            // 
            // rbActiveLow
            // 
            this.rbActiveLow.AutoSize = true;
            this.rbActiveLow.Location = new System.Drawing.Point(16, 16);
            this.rbActiveLow.Name = "rbActiveLow";
            this.rbActiveLow.Size = new System.Drawing.Size(78, 17);
            this.rbActiveLow.TabIndex = 1;
            this.rbActiveLow.TabStop = true;
            this.rbActiveLow.Text = "Active Low";
            this.rbActiveLow.UseVisualStyleBackColor = true;
            // 
            // rbActiveHigh
            // 
            this.rbActiveHigh.AutoSize = true;
            this.rbActiveHigh.Location = new System.Drawing.Point(16, 39);
            this.rbActiveHigh.Name = "rbActiveHigh";
            this.rbActiveHigh.Size = new System.Drawing.Size(80, 17);
            this.rbActiveHigh.TabIndex = 0;
            this.rbActiveHigh.TabStop = true;
            this.rbActiveHigh.Text = "Active High";
            this.rbActiveHigh.UseVisualStyleBackColor = true;
            this.rbActiveHigh.CheckedChanged += new System.EventHandler(this.rbActiveHigh_CheckedChanged);
            // 
            // gbTransitionDotclkEdge
            // 
            this.gbTransitionDotclkEdge.Controls.Add(this.rbRisingEdge);
            this.gbTransitionDotclkEdge.Controls.Add(this.rbFallingEdge);
            this.gbTransitionDotclkEdge.Location = new System.Drawing.Point(239, 5);
            this.gbTransitionDotclkEdge.Name = "gbTransitionDotclkEdge";
            this.gbTransitionDotclkEdge.Size = new System.Drawing.Size(230, 64);
            this.gbTransitionDotclkEdge.TabIndex = 1;
            this.gbTransitionDotclkEdge.TabStop = false;
            this.gbTransitionDotclkEdge.Text = "Transition Dotclk Edge";
            // 
            // rbRisingEdge
            // 
            this.rbRisingEdge.AutoSize = true;
            this.rbRisingEdge.Location = new System.Drawing.Point(16, 39);
            this.rbRisingEdge.Name = "rbRisingEdge";
            this.rbRisingEdge.Size = new System.Drawing.Size(81, 17);
            this.rbRisingEdge.TabIndex = 1;
            this.rbRisingEdge.TabStop = true;
            this.rbRisingEdge.Text = "Rising edge";
            this.rbRisingEdge.UseVisualStyleBackColor = true;
            // 
            // rbFallingEdge
            // 
            this.rbFallingEdge.AutoSize = true;
            this.rbFallingEdge.Location = new System.Drawing.Point(16, 16);
            this.rbFallingEdge.Name = "rbFallingEdge";
            this.rbFallingEdge.Size = new System.Drawing.Size(82, 17);
            this.rbFallingEdge.TabIndex = 0;
            this.rbFallingEdge.TabStop = true;
            this.rbFallingEdge.Text = "Falling edge";
            this.rbFallingEdge.UseVisualStyleBackColor = true;
            this.rbFallingEdge.CheckedChanged += new System.EventHandler(this.rbFallingEdge_CheckedChanged);
            // 
            // gbHorizontalTiming
            // 
            this.gbHorizontalTiming.Controls.Add(this.numHFrontPorch);
            this.gbHorizontalTiming.Controls.Add(this.numHActiveRegion);
            this.gbHorizontalTiming.Controls.Add(this.numHBackPorch);
            this.gbHorizontalTiming.Controls.Add(this.numHSyncWidth);
            this.gbHorizontalTiming.Controls.Add(this.label4);
            this.gbHorizontalTiming.Controls.Add(this.label3);
            this.gbHorizontalTiming.Controls.Add(this.label2);
            this.gbHorizontalTiming.Controls.Add(this.label1);
            this.gbHorizontalTiming.Location = new System.Drawing.Point(3, 72);
            this.gbHorizontalTiming.Name = "gbHorizontalTiming";
            this.gbHorizontalTiming.Size = new System.Drawing.Size(230, 120);
            this.gbHorizontalTiming.TabIndex = 2;
            this.gbHorizontalTiming.TabStop = false;
            this.gbHorizontalTiming.Text = "Horizontal Timing (dotclk)";
            // 
            // numHFrontPorch
            // 
            this.numHFrontPorch.Location = new System.Drawing.Point(123, 92);
            this.numHFrontPorch.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numHFrontPorch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHFrontPorch.Name = "numHFrontPorch";
            this.numHFrontPorch.Size = new System.Drawing.Size(69, 20);
            this.numHFrontPorch.TabIndex = 7;
            this.numHFrontPorch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numHActiveRegion
            // 
            this.numHActiveRegion.Location = new System.Drawing.Point(123, 66);
            this.numHActiveRegion.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numHActiveRegion.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numHActiveRegion.Name = "numHActiveRegion";
            this.numHActiveRegion.Size = new System.Drawing.Size(69, 20);
            this.numHActiveRegion.TabIndex = 6;
            this.numHActiveRegion.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // numHBackPorch
            // 
            this.numHBackPorch.Location = new System.Drawing.Point(123, 41);
            this.numHBackPorch.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numHBackPorch.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numHBackPorch.Name = "numHBackPorch";
            this.numHBackPorch.Size = new System.Drawing.Size(69, 20);
            this.numHBackPorch.TabIndex = 5;
            this.numHBackPorch.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // numHSyncWidth
            // 
            this.numHSyncWidth.Location = new System.Drawing.Point(123, 18);
            this.numHSyncWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numHSyncWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHSyncWidth.Name = "numHSyncWidth";
            this.numHSyncWidth.Size = new System.Drawing.Size(69, 20);
            this.numHSyncWidth.TabIndex = 4;
            this.numHSyncWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Front Porch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Active Region";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Back Porch";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sync Width";
            // 
            // gbVerticalTiming
            // 
            this.gbVerticalTiming.Controls.Add(this.numVFrontPorch);
            this.gbVerticalTiming.Controls.Add(this.numVActiveRegion);
            this.gbVerticalTiming.Controls.Add(this.numVBackPorch);
            this.gbVerticalTiming.Controls.Add(this.numVSyncWidth);
            this.gbVerticalTiming.Controls.Add(this.label5);
            this.gbVerticalTiming.Controls.Add(this.label6);
            this.gbVerticalTiming.Controls.Add(this.label7);
            this.gbVerticalTiming.Controls.Add(this.label8);
            this.gbVerticalTiming.Location = new System.Drawing.Point(239, 72);
            this.gbVerticalTiming.Name = "gbVerticalTiming";
            this.gbVerticalTiming.Size = new System.Drawing.Size(230, 120);
            this.gbVerticalTiming.TabIndex = 3;
            this.gbVerticalTiming.TabStop = false;
            this.gbVerticalTiming.Text = "Vertical Timing (lines)";
            // 
            // numVFrontPorch
            // 
            this.numVFrontPorch.Location = new System.Drawing.Point(123, 92);
            this.numVFrontPorch.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numVFrontPorch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVFrontPorch.Name = "numVFrontPorch";
            this.numVFrontPorch.Size = new System.Drawing.Size(69, 20);
            this.numVFrontPorch.TabIndex = 15;
            this.numVFrontPorch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numVActiveRegion
            // 
            this.numVActiveRegion.Location = new System.Drawing.Point(123, 66);
            this.numVActiveRegion.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numVActiveRegion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVActiveRegion.Name = "numVActiveRegion";
            this.numVActiveRegion.Size = new System.Drawing.Size(69, 20);
            this.numVActiveRegion.TabIndex = 14;
            this.numVActiveRegion.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numVBackPorch
            // 
            this.numVBackPorch.Location = new System.Drawing.Point(123, 41);
            this.numVBackPorch.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numVBackPorch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVBackPorch.Name = "numVBackPorch";
            this.numVBackPorch.Size = new System.Drawing.Size(69, 20);
            this.numVBackPorch.TabIndex = 13;
            this.numVBackPorch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numVSyncWidth
            // 
            this.numVSyncWidth.Location = new System.Drawing.Point(123, 18);
            this.numVSyncWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numVSyncWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVSyncWidth.Name = "numVSyncWidth";
            this.numVSyncWidth.Size = new System.Drawing.Size(69, 20);
            this.numVSyncWidth.TabIndex = 12;
            this.numVSyncWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Front Porch";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Active Region";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Back Porch";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Sync Width";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbVHBlanking);
            this.groupBox1.Controls.Add(this.rbVBlanking);
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Location = new System.Drawing.Point(3, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 42);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interrupt Generation";
            // 
            // rbVHBlanking
            // 
            this.rbVHBlanking.AutoSize = true;
            this.rbVHBlanking.Location = new System.Drawing.Point(298, 16);
            this.rbVHBlanking.Name = "rbVHBlanking";
            this.rbVHBlanking.Size = new System.Drawing.Size(162, 17);
            this.rbVHBlanking.TabIndex = 2;
            this.rbVHBlanking.TabStop = true;
            this.rbVHBlanking.Text = "Vertical && Horizontal blanking";
            this.rbVHBlanking.UseVisualStyleBackColor = true;
            // 
            // rbVBlanking
            // 
            this.rbVBlanking.AutoSize = true;
            this.rbVBlanking.Location = new System.Drawing.Point(157, 16);
            this.rbVBlanking.Name = "rbVBlanking";
            this.rbVBlanking.Size = new System.Drawing.Size(103, 17);
            this.rbVBlanking.TabIndex = 1;
            this.rbVBlanking.TabStop = true;
            this.rbVBlanking.Text = "Vertical blanking";
            this.rbVBlanking.UseVisualStyleBackColor = true;
            this.rbVBlanking.CheckedChanged += new System.EventHandler(this.rbVBlanking_CheckedChanged);
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(16, 16);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 0;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbNone_CheckedChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CyGraphicLCDCtrlBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbVerticalTiming);
            this.Controls.Add(this.gbHorizontalTiming);
            this.Controls.Add(this.gbTransitionDotclkEdge);
            this.Controls.Add(this.gbSyncPulsePolarity);
            this.Name = "CyGraphicLCDCtrlBasic";
            this.Size = new System.Drawing.Size(483, 267);
            this.gbSyncPulsePolarity.ResumeLayout(false);
            this.gbSyncPulsePolarity.PerformLayout();
            this.gbTransitionDotclkEdge.ResumeLayout(false);
            this.gbTransitionDotclkEdge.PerformLayout();
            this.gbHorizontalTiming.ResumeLayout(false);
            this.gbHorizontalTiming.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHFrontPorch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHActiveRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHBackPorch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHSyncWidth)).EndInit();
            this.gbVerticalTiming.ResumeLayout(false);
            this.gbVerticalTiming.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVFrontPorch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVActiveRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVBackPorch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVSyncWidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSyncPulsePolarity;
        private System.Windows.Forms.GroupBox gbTransitionDotclkEdge;
        private System.Windows.Forms.RadioButton rbActiveLow;
        private System.Windows.Forms.RadioButton rbActiveHigh;
        private System.Windows.Forms.RadioButton rbRisingEdge;
        private System.Windows.Forms.RadioButton rbFallingEdge;
        private System.Windows.Forms.GroupBox gbHorizontalTiming;
        private System.Windows.Forms.GroupBox gbVerticalTiming;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numHFrontPorch;
        private System.Windows.Forms.NumericUpDown numHActiveRegion;
        private System.Windows.Forms.NumericUpDown numHBackPorch;
        private System.Windows.Forms.NumericUpDown numHSyncWidth;
        private System.Windows.Forms.NumericUpDown numVFrontPorch;
        private System.Windows.Forms.NumericUpDown numVActiveRegion;
        private System.Windows.Forms.NumericUpDown numVBackPorch;
        private System.Windows.Forms.NumericUpDown numVSyncWidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbVHBlanking;
        private System.Windows.Forms.RadioButton rbVBlanking;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
