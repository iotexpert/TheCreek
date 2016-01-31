namespace OpAmp_v1_80
{
    partial class OPAMPcontrol
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
            this.mMode = new System.Windows.Forms.ComboBox();
            this.mPower = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mMode
            // 
            this.mMode.FormattingEnabled = true;
            this.mMode.Location = new System.Drawing.Point(85, 13);
            this.mMode.Name = "mMode";
            this.mMode.Size = new System.Drawing.Size(325, 21);
            this.mMode.TabIndex = 0;
            this.mMode.SelectedIndexChanged += new System.EventHandler(this.mMode_SelectedIndexChanged);
            // 
            // mPower
            // 
            this.mPower.FormattingEnabled = true;
            this.mPower.Location = new System.Drawing.Point(85, 53);
            this.mPower.Name = "mPower";
            this.mPower.Size = new System.Drawing.Size(325, 21);
            this.mPower.TabIndex = 1;
            this.mPower.SelectedIndexChanged += new System.EventHandler(this.mPower_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Mode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Power";
            // 
            // OPAMPcontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mPower);
            this.Controls.Add(this.mMode);
            this.Name = "OPAMPcontrol";
            this.Size = new System.Drawing.Size(413, 223);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox mMode;
        private System.Windows.Forms.ComboBox mPower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
