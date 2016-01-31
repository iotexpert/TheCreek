namespace CyCustomizer.SPI_Slave_v1_0
{
    partial class CySPISControl
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
            this.m_cbspisMode = new System.Windows.Forms.ComboBox();
            this.m_lblspisMode = new System.Windows.Forms.Label();
            this.m_numDataBits = new System.Windows.Forms.NumericUpDown();
            this.m_lblNumDataBits = new System.Windows.Forms.Label();
            this.m_pbDrawing = new System.Windows.Forms.PictureBox();
            this.m_lblShiftDir = new System.Windows.Forms.Label();
            this.m_cbShiftDir = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDataBits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbDrawing)).BeginInit();
            this.SuspendLayout();
            // 
            // m_cbspisMode
            // 
            this.m_cbspisMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbspisMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbspisMode.FormattingEnabled = true;
            this.m_cbspisMode.Location = new System.Drawing.Point(112, 142);
            this.m_cbspisMode.Name = "m_cbspisMode";
            this.m_cbspisMode.Size = new System.Drawing.Size(316, 21);
            this.m_cbspisMode.TabIndex = 0;
            this.m_cbspisMode.SelectedIndexChanged += new System.EventHandler(this.m_cbspisMode_SelectedIndexChanged);
            // 
            // m_lblspisMode
            // 
            this.m_lblspisMode.AutoSize = true;
            this.m_lblspisMode.Location = new System.Drawing.Point(63, 145);
            this.m_lblspisMode.Name = "m_lblspisMode";
            this.m_lblspisMode.Size = new System.Drawing.Size(37, 13);
            this.m_lblspisMode.TabIndex = 2;
            this.m_lblspisMode.Text = "Mode:";
            this.m_lblspisMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_numDataBits
            // 
            this.m_numDataBits.Location = new System.Drawing.Point(112, 169);
            this.m_numDataBits.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_numDataBits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numDataBits.Name = "m_numDataBits";
            this.m_numDataBits.Size = new System.Drawing.Size(79, 20);
            this.m_numDataBits.TabIndex = 3;
            this.m_numDataBits.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numDataBits.ValueChanged += new System.EventHandler(this.m_numDataBits_ValueChanged);
            // 
            // m_lblNumDataBits
            // 
            this.m_lblNumDataBits.AutoSize = true;
            this.m_lblNumDataBits.Location = new System.Drawing.Point(47, 173);
            this.m_lblNumDataBits.Name = "m_lblNumDataBits";
            this.m_lblNumDataBits.Size = new System.Drawing.Size(53, 13);
            this.m_lblNumDataBits.TabIndex = 4;
            this.m_lblNumDataBits.Text = "Data Bits:";
            this.m_lblNumDataBits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_pbDrawing
            // 
            this.m_pbDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pbDrawing.BackColor = System.Drawing.Color.White;
            this.m_pbDrawing.Location = new System.Drawing.Point(6, 3);
            this.m_pbDrawing.Name = "m_pbDrawing";
            this.m_pbDrawing.Size = new System.Drawing.Size(422, 133);
            this.m_pbDrawing.TabIndex = 20;
            this.m_pbDrawing.TabStop = false;
            // 
            // m_lblShiftDir
            // 
            this.m_lblShiftDir.AutoSize = true;
            this.m_lblShiftDir.Location = new System.Drawing.Point(24, 198);
            this.m_lblShiftDir.Name = "m_lblShiftDir";
            this.m_lblShiftDir.Size = new System.Drawing.Size(76, 13);
            this.m_lblShiftDir.TabIndex = 27;
            this.m_lblShiftDir.Text = "Shift Direction:";
            this.m_lblShiftDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbShiftDir
            // 
            this.m_cbShiftDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbShiftDir.FormattingEnabled = true;
            this.m_cbShiftDir.Location = new System.Drawing.Point(112, 194);
            this.m_cbShiftDir.Name = "m_cbShiftDir";
            this.m_cbShiftDir.Size = new System.Drawing.Size(124, 21);
            this.m_cbShiftDir.TabIndex = 34;
            this.m_cbShiftDir.SelectedIndexChanged += new System.EventHandler(this.m_cbShiftDir_SelectedIndexChanged);
            // 
            // CySPISControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cbShiftDir);
            this.Controls.Add(this.m_lblShiftDir);
            this.Controls.Add(this.m_pbDrawing);
            this.Controls.Add(this.m_lblNumDataBits);
            this.Controls.Add(this.m_numDataBits);
            this.Controls.Add(this.m_lblspisMode);
            this.Controls.Add(this.m_cbspisMode);
            this.Name = "CySPISControl";
            this.Size = new System.Drawing.Size(436, 224);
            this.SizeChanged += new System.EventHandler(this.CySPISControl_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.m_numDataBits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbDrawing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_cbspisMode;
        private System.Windows.Forms.Label m_lblspisMode;
        private System.Windows.Forms.NumericUpDown m_numDataBits;
        private System.Windows.Forms.Label m_lblNumDataBits;
        private System.Windows.Forms.PictureBox m_pbDrawing;
        private System.Windows.Forms.Label m_lblShiftDir;
        private System.Windows.Forms.ComboBox m_cbShiftDir;
    }
}
