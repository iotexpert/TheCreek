namespace SPDIF_Tx_v1_0
{
    partial class CyChannel0StatusTab
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
            this.m_groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_lblFrequency = new System.Windows.Forms.Label();
            this.m_cbFrequency = new System.Windows.Forms.ComboBox();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // m_groupBox1
            // 
            this.m_groupBox1.Controls.Add(this.m_lblFrequency);
            this.m_groupBox1.Controls.Add(this.m_cbFrequency);
            this.m_groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_groupBox1.Location = new System.Drawing.Point(0, 0);
            this.m_groupBox1.Name = "m_groupBox1";
            this.m_groupBox1.Size = new System.Drawing.Size(583, 48);
            this.m_groupBox1.TabIndex = 0;
            this.m_groupBox1.TabStop = false;
            // 
            // m_lblFrequency
            // 
            this.m_lblFrequency.AutoSize = true;
            this.m_lblFrequency.Location = new System.Drawing.Point(15, 20);
            this.m_lblFrequency.Name = "m_lblFrequency";
            this.m_lblFrequency.Size = new System.Drawing.Size(60, 13);
            this.m_lblFrequency.TabIndex = 1;
            this.m_lblFrequency.Text = "Frequency:";
            // 
            // m_cbFrequency
            // 
            this.m_cbFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbFrequency.FormattingEnabled = true;
            this.m_cbFrequency.Location = new System.Drawing.Point(106, 17);
            this.m_cbFrequency.Name = "m_cbFrequency";
            this.m_cbFrequency.Size = new System.Drawing.Size(121, 21);
            this.m_cbFrequency.TabIndex = 0;
            this.m_cbFrequency.SelectedIndexChanged += new System.EventHandler(this.m_cbFrequency_SelectedIndexChanged);
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyChannel0StatusTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_groupBox1);
            this.Name = "CyChannel0StatusTab";
            this.Size = new System.Drawing.Size(583, 293);
            this.m_groupBox1.ResumeLayout(false);
            this.m_groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_groupBox1;
        private System.Windows.Forms.Label m_lblFrequency;
        private System.Windows.Forms.ComboBox m_cbFrequency;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
    }
}
