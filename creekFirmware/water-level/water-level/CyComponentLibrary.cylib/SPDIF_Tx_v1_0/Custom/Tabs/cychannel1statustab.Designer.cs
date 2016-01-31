namespace SPDIF_Tx_v1_0
{
    partial class CyChannel1StatusTab
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
            this.m_chbxCopyDefaults = new System.Windows.Forms.CheckBox();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // m_groupBox1
            // 
            this.m_groupBox1.Controls.Add(this.m_chbxCopyDefaults);
            this.m_groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_groupBox1.Location = new System.Drawing.Point(0, 0);
            this.m_groupBox1.Name = "m_groupBox1";
            this.m_groupBox1.Size = new System.Drawing.Size(723, 48);
            this.m_groupBox1.TabIndex = 1;
            this.m_groupBox1.TabStop = false;
            // 
            // m_chbxCopyDefaults
            // 
            this.m_chbxCopyDefaults.AutoSize = true;
            this.m_chbxCopyDefaults.Location = new System.Drawing.Point(18, 19);
            this.m_chbxCopyDefaults.Name = "m_chbxCopyDefaults";
            this.m_chbxCopyDefaults.Size = new System.Drawing.Size(164, 17);
            this.m_chbxCopyDefaults.TabIndex = 0;
            this.m_chbxCopyDefaults.Text = "Copy defaults from Channel 0";
            this.m_chbxCopyDefaults.UseVisualStyleBackColor = true;
            this.m_chbxCopyDefaults.CheckedChanged += new System.EventHandler(this.m_chbxCopyDefaults_CheckedChanged);
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyChannel1StatusTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_groupBox1);
            this.Name = "CyChannel1StatusTab";
            this.Size = new System.Drawing.Size(723, 393);
            this.VisibleChanged += new System.EventHandler(this.CyChannel1StatusTab_VisibleChanged);
            this.m_groupBox1.ResumeLayout(false);
            this.m_groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_groupBox1;
        private System.Windows.Forms.CheckBox m_chbxCopyDefaults;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
    }
}
