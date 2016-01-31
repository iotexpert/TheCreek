namespace SPDIF_Tx_v1_0
{
    partial class CySPDifTxBasicTab
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_lblBits = new System.Windows.Forms.Label();
            this.m_cbAudioDataLength = new System.Windows.Forms.ComboBox();
            this.m_lblAudioDataLength = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_panelAudioMode = new System.Windows.Forms.Panel();
            this.m_rbSeparated = new System.Windows.Forms.RadioButton();
            this.m_rbInterleaved = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_chbxManagedDma = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.m_panelAudioMode.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_lblBits);
            this.groupBox1.Controls.Add(this.m_cbAudioDataLength);
            this.groupBox1.Controls.Add(this.m_lblAudioDataLength);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(410, 51);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // m_lblBits
            // 
            this.m_lblBits.AutoSize = true;
            this.m_lblBits.Location = new System.Drawing.Point(266, 21);
            this.m_lblBits.Name = "m_lblBits";
            this.m_lblBits.Size = new System.Drawing.Size(23, 13);
            this.m_lblBits.TabIndex = 5;
            this.m_lblBits.Text = "bits";
            // 
            // m_cbAudioDataLength
            // 
            this.m_cbAudioDataLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbAudioDataLength.FormattingEnabled = true;
            this.m_cbAudioDataLength.Items.AddRange(new object[] {
            "8",
            "16",
            "24"});
            this.m_cbAudioDataLength.Location = new System.Drawing.Point(125, 18);
            this.m_cbAudioDataLength.Name = "m_cbAudioDataLength";
            this.m_cbAudioDataLength.Size = new System.Drawing.Size(128, 21);
            this.m_cbAudioDataLength.TabIndex = 4;
            this.m_cbAudioDataLength.SelectedIndexChanged += new System.EventHandler(this.m_cbAudioDataLength_SelectedIndexChanged);
            // 
            // m_lblAudioDataLength
            // 
            this.m_lblAudioDataLength.AutoSize = true;
            this.m_lblAudioDataLength.Location = new System.Drawing.Point(20, 21);
            this.m_lblAudioDataLength.Name = "m_lblAudioDataLength";
            this.m_lblAudioDataLength.Size = new System.Drawing.Size(99, 13);
            this.m_lblAudioDataLength.TabIndex = 3;
            this.m_lblAudioDataLength.Text = "Audio Data Length:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.m_panelAudioMode);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 51);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(410, 74);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 15);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Audio Mode:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 39);
            this.label1.TabIndex = 7;
            this.label1.Text = "Specifies whether the audio \r\ndata stream will be a single byte \r\nstream or two b" +
                "yte streams.";
            // 
            // m_panelAudioMode
            // 
            this.m_panelAudioMode.Controls.Add(this.m_rbSeparated);
            this.m_panelAudioMode.Controls.Add(this.m_rbInterleaved);
            this.m_panelAudioMode.Location = new System.Drawing.Point(96, 15);
            this.m_panelAudioMode.Name = "m_panelAudioMode";
            this.m_panelAudioMode.Size = new System.Drawing.Size(83, 43);
            this.m_panelAudioMode.TabIndex = 5;
            // 
            // m_rbSeparated
            // 
            this.m_rbSeparated.AutoSize = true;
            this.m_rbSeparated.Location = new System.Drawing.Point(0, 24);
            this.m_rbSeparated.Name = "m_rbSeparated";
            this.m_rbSeparated.Size = new System.Drawing.Size(74, 17);
            this.m_rbSeparated.TabIndex = 1;
            this.m_rbSeparated.TabStop = true;
            this.m_rbSeparated.Text = "Separated";
            this.m_rbSeparated.UseVisualStyleBackColor = true;
            // 
            // m_rbInterleaved
            // 
            this.m_rbInterleaved.AutoSize = true;
            this.m_rbInterleaved.Location = new System.Drawing.Point(0, 0);
            this.m_rbInterleaved.Name = "m_rbInterleaved";
            this.m_rbInterleaved.Size = new System.Drawing.Size(78, 17);
            this.m_rbInterleaved.TabIndex = 0;
            this.m_rbInterleaved.TabStop = true;
            this.m_rbInterleaved.Text = "Interleaved";
            this.m_rbInterleaved.UseVisualStyleBackColor = true;
            this.m_rbInterleaved.CheckedChanged += new System.EventHandler(this.m_rbInterleaved_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.m_chbxManagedDma);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 125);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(410, 60);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "Enables automatic generation of \r\nchannel status for the consumer format.";
            // 
            // m_chbxManagedDma
            // 
            this.m_chbxManagedDma.AutoSize = true;
            this.m_chbxManagedDma.Location = new System.Drawing.Point(22, 18);
            this.m_chbxManagedDma.Name = "m_chbxManagedDma";
            this.m_chbxManagedDma.Size = new System.Drawing.Size(98, 17);
            this.m_chbxManagedDma.TabIndex = 6;
            this.m_chbxManagedDma.Text = "Managed DMA";
            this.m_chbxManagedDma.UseVisualStyleBackColor = true;
            this.m_chbxManagedDma.CheckedChanged += new System.EventHandler(this.m_chbxManagedDma_CheckedChanged);
            // 
            // CySPDifTxBasicTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CySPDifTxBasicTab";
            this.Size = new System.Drawing.Size(410, 204);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_panelAudioMode.ResumeLayout(false);
            this.m_panelAudioMode.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label m_lblBits;
        private System.Windows.Forms.ComboBox m_cbAudioDataLength;
        private System.Windows.Forms.Label m_lblAudioDataLength;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel m_panelAudioMode;
        private System.Windows.Forms.RadioButton m_rbSeparated;
        private System.Windows.Forms.RadioButton m_rbInterleaved;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox m_chbxManagedDma;
        private System.Windows.Forms.Label label3;
    }
}
