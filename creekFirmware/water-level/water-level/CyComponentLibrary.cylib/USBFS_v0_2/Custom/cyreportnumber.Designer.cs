namespace USBFS_v0_2
{
    partial class CyReportNumber
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
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxBase = new System.Windows.Forms.GroupBox();
            this.radioButtonHex = new System.Windows.Forms.RadioButton();
            this.radioButtonDec = new System.Windows.Forms.RadioButton();
            this.groupBoxBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxValue
            // 
            this.textBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxValue.Location = new System.Drawing.Point(6, 26);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(170, 20);
            this.textBoxValue.TabIndex = 0;
            this.textBoxValue.Validated += new System.EventHandler(this.textBoxValue_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
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
            this.groupBoxBase.Size = new System.Drawing.Size(170, 60);
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
            // 
            // CyReportNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxBase);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxValue);
            this.Name = "CyReportNumber";
            this.Size = new System.Drawing.Size(182, 115);
            this.groupBoxBase.ResumeLayout(false);
            this.groupBoxBase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxBase;
        private System.Windows.Forms.RadioButton radioButtonHex;
        private System.Windows.Forms.RadioButton radioButtonDec;
    }
}
