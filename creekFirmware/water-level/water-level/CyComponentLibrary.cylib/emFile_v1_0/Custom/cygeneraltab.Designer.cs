namespace emFile_v1_0
{
    partial class CyEmFileGeneralTab
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
            this.label1 = new System.Windows.Forms.Label();
            this.numMaxSPIFrequency = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chbSDCard0WP = new System.Windows.Forms.CheckBox();
            this.chbSDCard1WP = new System.Windows.Forms.CheckBox();
            this.chbSDCard2WP = new System.Windows.Forms.CheckBox();
            this.chbSDCard3WP = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cbSDCardsNumber = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSPIFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Max SPI frequency (kHz):";
            // 
            // numMaxSPIFrequency
            // 
            this.numMaxSPIFrequency.Location = new System.Drawing.Point(137, 7);
            this.numMaxSPIFrequency.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.numMaxSPIFrequency.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numMaxSPIFrequency.Name = "numMaxSPIFrequency";
            this.numMaxSPIFrequency.Size = new System.Drawing.Size(81, 20);
            this.numMaxSPIFrequency.TabIndex = 1;
            this.numMaxSPIFrequency.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Number of SD cards:";
            // 
            // chbSDCard0WP
            // 
            this.chbSDCard0WP.AutoSize = true;
            this.chbSDCard0WP.Location = new System.Drawing.Point(6, 60);
            this.chbSDCard0WP.Name = "chbSDCard0WP";
            this.chbSDCard0WP.Size = new System.Drawing.Size(149, 17);
            this.chbSDCard0WP.TabIndex = 5;
            this.chbSDCard0WP.Text = "SD card 0 write protection";
            this.chbSDCard0WP.UseVisualStyleBackColor = true;
            // 
            // chbSDCard1WP
            // 
            this.chbSDCard1WP.AutoSize = true;
            this.chbSDCard1WP.Location = new System.Drawing.Point(6, 83);
            this.chbSDCard1WP.Name = "chbSDCard1WP";
            this.chbSDCard1WP.Size = new System.Drawing.Size(149, 17);
            this.chbSDCard1WP.TabIndex = 6;
            this.chbSDCard1WP.Text = "SD card 1 write protection";
            this.chbSDCard1WP.UseVisualStyleBackColor = true;
            // 
            // chbSDCard2WP
            // 
            this.chbSDCard2WP.AutoSize = true;
            this.chbSDCard2WP.Location = new System.Drawing.Point(6, 106);
            this.chbSDCard2WP.Name = "chbSDCard2WP";
            this.chbSDCard2WP.Size = new System.Drawing.Size(149, 17);
            this.chbSDCard2WP.TabIndex = 7;
            this.chbSDCard2WP.Text = "SD card 2 write protection";
            this.chbSDCard2WP.UseVisualStyleBackColor = true;
            // 
            // chbSDCard3WP
            // 
            this.chbSDCard3WP.AutoSize = true;
            this.chbSDCard3WP.Location = new System.Drawing.Point(6, 129);
            this.chbSDCard3WP.Name = "chbSDCard3WP";
            this.chbSDCard3WP.Size = new System.Drawing.Size(149, 17);
            this.chbSDCard3WP.TabIndex = 8;
            this.chbSDCard3WP.Text = "SD card 3 write protection";
            this.chbSDCard3WP.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // cbSDCardsNumber
            // 
            this.cbSDCardsNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSDCardsNumber.FormattingEnabled = true;
            this.cbSDCardsNumber.Location = new System.Drawing.Point(137, 33);
            this.cbSDCardsNumber.Name = "cbSDCardsNumber";
            this.cbSDCardsNumber.Size = new System.Drawing.Size(81, 21);
            this.cbSDCardsNumber.TabIndex = 9;
            // 
            // CyEmFileGeneralTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbSDCardsNumber);
            this.Controls.Add(this.chbSDCard3WP);
            this.Controls.Add(this.chbSDCard2WP);
            this.Controls.Add(this.chbSDCard1WP);
            this.Controls.Add(this.chbSDCard0WP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numMaxSPIFrequency);
            this.Controls.Add(this.label1);
            this.Name = "CyEmFileGeneralTab";
            this.Size = new System.Drawing.Size(456, 263);
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSPIFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMaxSPIFrequency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbSDCard0WP;
        private System.Windows.Forms.CheckBox chbSDCard1WP;
        private System.Windows.Forms.CheckBox chbSDCard2WP;
        private System.Windows.Forms.CheckBox chbSDCard3WP;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox cbSDCardsNumber;
    }
}
