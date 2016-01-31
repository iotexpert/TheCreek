namespace USBFS_v2_10
{
    partial class CyDetailsLangID
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
            this.groupBoxParams = new System.Windows.Forms.GroupBox();
            this.comboBoxLangID = new System.Windows.Forms.ComboBox();
            this.groupBoxParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxParams.Controls.Add(this.comboBoxLangID);
            this.groupBoxParams.Location = new System.Drawing.Point(0, 9);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Size = new System.Drawing.Size(281, 73);
            this.groupBoxParams.TabIndex = 4;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Select language";
            // 
            // comboBoxLangID
            // 
            this.comboBoxLangID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLangID.FormattingEnabled = true;
            this.comboBoxLangID.Location = new System.Drawing.Point(16, 29);
            this.comboBoxLangID.Name = "comboBoxLangID";
            this.comboBoxLangID.Size = new System.Drawing.Size(248, 21);
            this.comboBoxLangID.TabIndex = 0;
            this.comboBoxLangID.SelectedIndexChanged += new System.EventHandler(this.comboBoxLangID_SelectedIndexChanged);
            // 
            // CyDetailsLangID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxParams);
            this.Name = "CyDetailsLangID";
            this.Size = new System.Drawing.Size(284, 93);
            this.Load += new System.EventHandler(this.CyDetailsLangID_Load);
            this.groupBoxParams.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.ComboBox comboBoxLangID;
    }
}
