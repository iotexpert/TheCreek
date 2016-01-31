namespace CyCustomizer.SPI_Master_v1_10
{
    partial class CySPIMControl
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
            this.m_cbspimMode = new System.Windows.Forms.ComboBox();
            this.m_lblspimMode = new System.Windows.Forms.Label();
            this.m_numDataBits = new CyNumericUpDown();
            this.m_lblNumDataBits = new System.Windows.Forms.Label();
            this.m_pbDrawing = new System.Windows.Forms.PictureBox();
            this.m_lblShiftDir = new System.Windows.Forms.Label();
            this.m_cbShiftDir = new System.Windows.Forms.ComboBox();
            this.m_lblBitRate = new System.Windows.Forms.Label();
            this.m_cbBitRateHertz = new System.Windows.Forms.ComboBox();
            this.m_numBitRateHertz = new CyNumericUpDown();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_lblCalculatedBitRate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDataBits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbDrawing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numBitRateHertz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.SuspendLayout();
            // 
            // m_cbspimMode
            // 
            this.m_cbspimMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbspimMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbspimMode.FormattingEnabled = true;
            this.m_cbspimMode.Location = new System.Drawing.Point(112, 142);
            this.m_cbspimMode.Name = "m_cbspimMode";
            this.m_cbspimMode.Size = new System.Drawing.Size(316, 21);
            this.m_cbspimMode.TabIndex = 0;
            this.m_cbspimMode.SelectedIndexChanged += new System.EventHandler(this.m_cbspimMode_SelectedIndexChanged);
            // 
            // m_lblspimMode
            // 
            this.m_lblspimMode.AutoSize = true;
            this.m_lblspimMode.Location = new System.Drawing.Point(63, 145);
            this.m_lblspimMode.Name = "m_lblspimMode";
            this.m_lblspimMode.Size = new System.Drawing.Size(37, 13);
            this.m_lblspimMode.TabIndex = 2;
            this.m_lblspimMode.Text = "Mode:";
            this.m_lblspimMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.m_numDataBits.Validating += new System.ComponentModel.CancelEventHandler(this.m_numDataBits_Validating);
            this.m_numDataBits.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_numDataBits_KeyUp);
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
            // m_lblBitRate
            // 
            this.m_lblBitRate.AutoSize = true;
            this.m_lblBitRate.Location = new System.Drawing.Point(52, 224);
            this.m_lblBitRate.Name = "m_lblBitRate";
            this.m_lblBitRate.Size = new System.Drawing.Size(48, 13);
            this.m_lblBitRate.TabIndex = 35;
            this.m_lblBitRate.Text = "Bit Rate:";
            this.m_lblBitRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbBitRateHertz
            // 
            this.m_cbBitRateHertz.FormattingEnabled = true;
            this.m_cbBitRateHertz.Items.AddRange(new object[] {
            "KHz",
            "MHz"});
            this.m_cbBitRateHertz.Location = new System.Drawing.Point(218, 221);
            this.m_cbBitRateHertz.Name = "m_cbBitRateHertz";
            this.m_cbBitRateHertz.Size = new System.Drawing.Size(77, 21);
            this.m_cbBitRateHertz.TabIndex = 36;
            this.m_cbBitRateHertz.SelectedIndexChanged += new System.EventHandler(this.m_cbBitRateHertz_SelectedIndexChanged);
            // 
            // m_numBitRateHertz
            // 
            this.m_numBitRateHertz.DecimalPlaces = 3;
            this.m_numBitRateHertz.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.m_numBitRateHertz.Location = new System.Drawing.Point(112, 221);
            this.m_numBitRateHertz.Name = "m_numBitRateHertz";
            this.m_numBitRateHertz.Size = new System.Drawing.Size(100, 20);
            this.m_numBitRateHertz.TabIndex = 37;
            this.m_numBitRateHertz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.m_numBitRateHertz.ThousandsSeparator = true;
            this.m_numBitRateHertz.ValueChanged += new System.EventHandler(this.m_numBitRateHertz_ValueChanged);
            this.m_numBitRateHertz.Validating += new System.ComponentModel.CancelEventHandler(this.m_numBitRateHertz_Validating);
            this.m_numBitRateHertz.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_numBitRateHertz_KeyUp);
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // m_lblCalculatedBitRate
            // 
            this.m_lblCalculatedBitRate.AutoSize = true;
            this.m_lblCalculatedBitRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblCalculatedBitRate.Location = new System.Drawing.Point(109, 224);
            this.m_lblCalculatedBitRate.Name = "m_lblCalculatedBitRate";
            this.m_lblCalculatedBitRate.Size = new System.Drawing.Size(41, 13);
            this.m_lblCalculatedBitRate.TabIndex = 38;
            this.m_lblCalculatedBitRate.Text = "label1";
            // 
            // CySPIMControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.m_lblCalculatedBitRate);
            this.Controls.Add(this.m_numBitRateHertz);
            this.Controls.Add(this.m_cbBitRateHertz);
            this.Controls.Add(this.m_lblBitRate);
            this.Controls.Add(this.m_cbShiftDir);
            this.Controls.Add(this.m_lblShiftDir);
            this.Controls.Add(this.m_pbDrawing);
            this.Controls.Add(this.m_lblNumDataBits);
            this.Controls.Add(this.m_numDataBits);
            this.Controls.Add(this.m_lblspimMode);
            this.Controls.Add(this.m_cbspimMode);
            this.Name = "CySPIMControl";
            this.Size = new System.Drawing.Size(436, 242);
            this.SizeChanged += new System.EventHandler(this.CySPIMControl_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.m_numDataBits)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_pbDrawing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numBitRateHertz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_cbspimMode;
        private System.Windows.Forms.Label m_lblspimMode;
        private CyNumericUpDown m_numDataBits;
        private System.Windows.Forms.Label m_lblNumDataBits;
        private System.Windows.Forms.PictureBox m_pbDrawing;
        private System.Windows.Forms.Label m_lblShiftDir;
        private System.Windows.Forms.ComboBox m_cbShiftDir;
        private System.Windows.Forms.Label m_lblBitRate;
        private System.Windows.Forms.ComboBox m_cbBitRateHertz;
        private CyNumericUpDown m_numBitRateHertz;
        private System.Windows.Forms.ErrorProvider ep_Errors;
        private System.Windows.Forms.Label m_lblCalculatedBitRate;
    }
}
