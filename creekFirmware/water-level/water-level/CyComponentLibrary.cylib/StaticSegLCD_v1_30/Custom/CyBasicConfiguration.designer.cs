namespace StaticSegLCD_v1_30
{
    partial class CyBasicConfiguration
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
            this.comboBoxFrameRate = new System.Windows.Forms.ComboBox();
            this.labelFrameRate = new System.Windows.Forms.Label();
            this.numUpDownNumSegmentLines = new System.Windows.Forms.NumericUpDown();
            this.labelNumSegmentLines = new System.Windows.Forms.Label();
            this.labelSegEnabled = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumSegmentLines)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxFrameRate
            // 
            this.comboBoxFrameRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrameRate.FormattingEnabled = true;
            this.comboBoxFrameRate.Location = new System.Drawing.Point(199, 51);
            this.comboBoxFrameRate.Name = "comboBoxFrameRate";
            this.comboBoxFrameRate.Size = new System.Drawing.Size(110, 21);
            this.comboBoxFrameRate.TabIndex = 26;
            this.comboBoxFrameRate.SelectedIndexChanged += new System.EventHandler(this.comboBoxFrameRate_SelectedIndexChanged);
            // 
            // labelFrameRate
            // 
            this.labelFrameRate.AutoSize = true;
            this.labelFrameRate.Location = new System.Drawing.Point(3, 51);
            this.labelFrameRate.Name = "labelFrameRate";
            this.labelFrameRate.Size = new System.Drawing.Size(76, 13);
            this.labelFrameRate.TabIndex = 22;
            this.labelFrameRate.Text = "Frame rate, Hz";
            // 
            // numUpDownNumSegmentLines
            // 
            this.numUpDownNumSegmentLines.Location = new System.Drawing.Point(199, 17);
            this.numUpDownNumSegmentLines.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.Name = "numUpDownNumSegmentLines";
            this.numUpDownNumSegmentLines.Size = new System.Drawing.Size(110, 20);
            this.numUpDownNumSegmentLines.TabIndex = 17;
            this.numUpDownNumSegmentLines.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.ValueChanged += new System.EventHandler(this.numUpDownNumSegmentLines_ValueChanged);
            // 
            // labelNumSegmentLines
            // 
            this.labelNumSegmentLines.AutoSize = true;
            this.labelNumSegmentLines.Location = new System.Drawing.Point(3, 21);
            this.labelNumSegmentLines.Name = "labelNumSegmentLines";
            this.labelNumSegmentLines.Size = new System.Drawing.Size(123, 13);
            this.labelNumSegmentLines.TabIndex = 16;
            this.labelNumSegmentLines.Text = "Number of segment lines";
            // 
            // labelSegEnabled
            // 
            this.labelSegEnabled.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labelSegEnabled.Location = new System.Drawing.Point(3, 84);
            this.labelSegEnabled.Name = "labelSegEnabled";
            this.labelSegEnabled.Size = new System.Drawing.Size(437, 28);
            this.labelSegEnabled.TabIndex = 34;
            this.labelSegEnabled.Text = "You are unable to modify the number of segment lines, because you have added one " +
                "or more Display Helpers. ";
            // 
            // CyBasicConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelSegEnabled);
            this.Controls.Add(this.comboBoxFrameRate);
            this.Controls.Add(this.labelFrameRate);
            this.Controls.Add(this.numUpDownNumSegmentLines);
            this.Controls.Add(this.labelNumSegmentLines);
            this.Name = "CyBasicConfiguration";
            this.Size = new System.Drawing.Size(443, 239);
            this.VisibleChanged += new System.EventHandler(this.CyBasicConfiguration_VisibleChanged);
            this.Leave += new System.EventHandler(this.CyBasicConfiguration_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumSegmentLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFrameRate;
        private System.Windows.Forms.Label labelFrameRate;
        private System.Windows.Forms.NumericUpDown numUpDownNumSegmentLines;
        private System.Windows.Forms.Label labelNumSegmentLines;
        private System.Windows.Forms.Label labelSegEnabled;
    }
}
