namespace USBFS_v2_30
{
    partial class CyReportCustom
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
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxBase = new System.Windows.Forms.GroupBox();
            this.radioButtonHex = new System.Windows.Forms.RadioButton();
            this.radioButtonDec = new System.Windows.Forms.RadioButton();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.numUpDownPrefix = new System.Windows.Forms.NumericUpDown();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPrefix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxValue
            // 
            this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValue.Location = new System.Drawing.Point(60, 26);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(102, 20);
            this.textBoxValue.TabIndex = 0;
            this.textBoxValue.TextChanged += new System.EventHandler(this.textBoxValue_TextChanged);
            this.textBoxValue.Validated += new System.EventHandler(this.textBoxValue_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Value";
            // 
            // groupBoxBase
            // 
            this.groupBoxBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBase.Controls.Add(this.radioButtonHex);
            this.groupBoxBase.Controls.Add(this.radioButtonDec);
            this.groupBoxBase.Location = new System.Drawing.Point(6, 52);
            this.groupBoxBase.Name = "groupBoxBase";
            this.groupBoxBase.Size = new System.Drawing.Size(156, 60);
            this.groupBoxBase.TabIndex = 2;
            this.groupBoxBase.TabStop = false;
            this.groupBoxBase.Text = "Base";
            // 
            // radioButtonHex
            // 
            this.radioButtonHex.AutoSize = true;
            this.radioButtonHex.Location = new System.Drawing.Point(6, 37);
            this.radioButtonHex.Name = "radioButtonHex";
            this.radioButtonHex.Size = new System.Drawing.Size(86, 17);
            this.radioButtonHex.TabIndex = 1;
            this.radioButtonHex.Text = "Hexadecimal";
            this.radioButtonHex.UseVisualStyleBackColor = true;
            this.radioButtonHex.CheckedChanged += new System.EventHandler(this.radioButtonDec_CheckedChanged);
            // 
            // radioButtonDec
            // 
            this.radioButtonDec.AutoSize = true;
            this.radioButtonDec.Checked = true;
            this.radioButtonDec.Location = new System.Drawing.Point(6, 19);
            this.radioButtonDec.Name = "radioButtonDec";
            this.radioButtonDec.Size = new System.Drawing.Size(63, 17);
            this.radioButtonDec.TabIndex = 0;
            this.radioButtonDec.TabStop = true;
            this.radioButtonDec.Text = "Decimal";
            this.radioButtonDec.UseVisualStyleBackColor = true;
            this.radioButtonDec.CheckedChanged += new System.EventHandler(this.radioButtonDec_CheckedChanged);
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Location = new System.Drawing.Point(3, 10);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(33, 13);
            this.labelPrefix.TabIndex = 3;
            this.labelPrefix.Text = "Prefix";
            // 
            // numUpDownPrefix
            // 
            this.numUpDownPrefix.Location = new System.Drawing.Point(6, 26);
            this.numUpDownPrefix.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.numUpDownPrefix.Name = "numUpDownPrefix";
            this.numUpDownPrefix.Size = new System.Drawing.Size(48, 20);
            this.numUpDownPrefix.TabIndex = 4;
            this.numUpDownPrefix.Validated += new System.EventHandler(this.textBoxValue_Validated);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // CyReportCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numUpDownPrefix);
            this.Controls.Add(this.labelPrefix);
            this.Controls.Add(this.groupBoxBase);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxValue);
            this.Name = "CyReportCustom";
            this.Size = new System.Drawing.Size(182, 115);
            this.groupBoxBase.ResumeLayout(false);
            this.groupBoxBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownPrefix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxBase;
        private System.Windows.Forms.RadioButton radioButtonHex;
        private System.Windows.Forms.RadioButton radioButtonDec;
        private System.Windows.Forms.Label labelPrefix;
        private System.Windows.Forms.NumericUpDown numUpDownPrefix;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
