namespace CyCustomizer.UART_v1_0
{
    partial class cyuartcontrol
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
            this.gbRXTX = new System.Windows.Forms.GroupBox();
            this.m_rbRXTX = new System.Windows.Forms.RadioButton();
            this.m_rbTXOnly = new System.Windows.Forms.RadioButton();
            this.m_rbRXOnly = new System.Windows.Forms.RadioButton();
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.m_lblFlowControl = new System.Windows.Forms.Label();
            this.m_lblStopBits = new System.Windows.Forms.Label();
            this.m_cbFlowControl = new System.Windows.Forms.ComboBox();
            this.m_lblParity = new System.Windows.Forms.Label();
            this.m_cbStopBits = new System.Windows.Forms.ComboBox();
            this.m_cbParity = new System.Windows.Forms.ComboBox();
            this.m_lblDataBits = new System.Windows.Forms.Label();
            this.m_lblBPS = new System.Windows.Forms.Label();
            this.m_cbDataBits = new System.Windows.Forms.ComboBox();
            this.m_cbBitsPerSecond = new System.Windows.Forms.ComboBox();
            this.gbRXTX.SuspendLayout();
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRXTX
            // 
            this.gbRXTX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRXTX.Controls.Add(this.m_rbRXTX);
            this.gbRXTX.Controls.Add(this.m_rbTXOnly);
            this.gbRXTX.Controls.Add(this.m_rbRXOnly);
            this.gbRXTX.Location = new System.Drawing.Point(6, 2);
            this.gbRXTX.Margin = new System.Windows.Forms.Padding(2);
            this.gbRXTX.Name = "gbRXTX";
            this.gbRXTX.Padding = new System.Windows.Forms.Padding(2);
            this.gbRXTX.Size = new System.Drawing.Size(286, 79);
            this.gbRXTX.TabIndex = 139;
            this.gbRXTX.TabStop = false;
            this.gbRXTX.Text = "Mode:";
            // 
            // m_rbRXTX
            // 
            this.m_rbRXTX.AutoCheck = false;
            this.m_rbRXTX.AutoSize = true;
            this.m_rbRXTX.Location = new System.Drawing.Point(4, 55);
            this.m_rbRXTX.Margin = new System.Windows.Forms.Padding(2);
            this.m_rbRXTX.Name = "m_rbRXTX";
            this.m_rbRXTX.Size = new System.Drawing.Size(124, 17);
            this.m_rbRXTX.TabIndex = 2;
            this.m_rbRXTX.TabStop = true;
            this.m_rbRXTX.Text = "Full UART (TX + RX)";
            this.m_rbRXTX.UseVisualStyleBackColor = true;
            this.m_rbRXTX.Click += new System.EventHandler(this.m_rbRXTX_Click);
            // 
            // m_rbTXOnly
            // 
            this.m_rbTXOnly.AutoCheck = false;
            this.m_rbTXOnly.AutoSize = true;
            this.m_rbTXOnly.Location = new System.Drawing.Point(4, 35);
            this.m_rbTXOnly.Margin = new System.Windows.Forms.Padding(2);
            this.m_rbTXOnly.Name = "m_rbTXOnly";
            this.m_rbTXOnly.Size = new System.Drawing.Size(63, 17);
            this.m_rbTXOnly.TabIndex = 1;
            this.m_rbTXOnly.TabStop = true;
            this.m_rbTXOnly.Text = "TX Only";
            this.m_rbTXOnly.UseVisualStyleBackColor = true;
            this.m_rbTXOnly.Click += new System.EventHandler(this.m_rbTXOnly_Click);
            // 
            // m_rbRXOnly
            // 
            this.m_rbRXOnly.AutoCheck = false;
            this.m_rbRXOnly.AutoSize = true;
            this.m_rbRXOnly.Location = new System.Drawing.Point(4, 15);
            this.m_rbRXOnly.Margin = new System.Windows.Forms.Padding(2);
            this.m_rbRXOnly.Name = "m_rbRXOnly";
            this.m_rbRXOnly.Size = new System.Drawing.Size(64, 17);
            this.m_rbRXOnly.TabIndex = 0;
            this.m_rbRXOnly.TabStop = true;
            this.m_rbRXOnly.Text = "RX Only";
            this.m_rbRXOnly.UseVisualStyleBackColor = true;
            this.m_rbRXOnly.Click += new System.EventHandler(this.m_rbRXOnly_Click);
            // 
            // gbMain
            // 
            this.gbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMain.Controls.Add(this.m_lblFlowControl);
            this.gbMain.Controls.Add(this.m_lblStopBits);
            this.gbMain.Controls.Add(this.m_cbFlowControl);
            this.gbMain.Controls.Add(this.m_lblParity);
            this.gbMain.Controls.Add(this.m_cbStopBits);
            this.gbMain.Controls.Add(this.m_cbParity);
            this.gbMain.Controls.Add(this.m_lblDataBits);
            this.gbMain.Controls.Add(this.m_lblBPS);
            this.gbMain.Controls.Add(this.m_cbDataBits);
            this.gbMain.Controls.Add(this.m_cbBitsPerSecond);
            this.gbMain.Location = new System.Drawing.Point(6, 83);
            this.gbMain.Margin = new System.Windows.Forms.Padding(2);
            this.gbMain.Name = "gbMain";
            this.gbMain.Padding = new System.Windows.Forms.Padding(2);
            this.gbMain.Size = new System.Drawing.Size(286, 148);
            this.gbMain.TabIndex = 138;
            this.gbMain.TabStop = false;
            // 
            // m_lblFlowControl
            // 
            this.m_lblFlowControl.AutoSize = true;
            this.m_lblFlowControl.Location = new System.Drawing.Point(48, 120);
            this.m_lblFlowControl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblFlowControl.Name = "m_lblFlowControl";
            this.m_lblFlowControl.Size = new System.Drawing.Size(68, 13);
            this.m_lblFlowControl.TabIndex = 135;
            this.m_lblFlowControl.Text = "Flow Control:";
            this.m_lblFlowControl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_lblStopBits
            // 
            this.m_lblStopBits.AutoSize = true;
            this.m_lblStopBits.Location = new System.Drawing.Point(64, 93);
            this.m_lblStopBits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblStopBits.Name = "m_lblStopBits";
            this.m_lblStopBits.Size = new System.Drawing.Size(51, 13);
            this.m_lblStopBits.TabIndex = 135;
            this.m_lblStopBits.Text = "Stop bits:";
            this.m_lblStopBits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbFlowControl
            // 
            this.m_cbFlowControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbFlowControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbFlowControl.FormattingEnabled = true;
            this.m_cbFlowControl.Location = new System.Drawing.Point(136, 117);
            this.m_cbFlowControl.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbFlowControl.Name = "m_cbFlowControl";
            this.m_cbFlowControl.Size = new System.Drawing.Size(126, 21);
            this.m_cbFlowControl.TabIndex = 134;
            this.m_cbFlowControl.SelectedIndexChanged += new System.EventHandler(this.m_cbFlowControl_SelectedIndexChanged);
            // 
            // m_lblParity
            // 
            this.m_lblParity.AutoSize = true;
            this.m_lblParity.Location = new System.Drawing.Point(78, 67);
            this.m_lblParity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblParity.Name = "m_lblParity";
            this.m_lblParity.Size = new System.Drawing.Size(36, 13);
            this.m_lblParity.TabIndex = 135;
            this.m_lblParity.Text = "Parity:";
            this.m_lblParity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbStopBits
            // 
            this.m_cbStopBits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbStopBits.FormattingEnabled = true;
            this.m_cbStopBits.Location = new System.Drawing.Point(136, 90);
            this.m_cbStopBits.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbStopBits.Name = "m_cbStopBits";
            this.m_cbStopBits.Size = new System.Drawing.Size(126, 21);
            this.m_cbStopBits.TabIndex = 134;
            this.m_cbStopBits.SelectedIndexChanged += new System.EventHandler(this.m_cbStopBits_SelectedIndexChanged);
            // 
            // m_cbParity
            // 
            this.m_cbParity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbParity.FormattingEnabled = true;
            this.m_cbParity.Location = new System.Drawing.Point(136, 63);
            this.m_cbParity.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbParity.Name = "m_cbParity";
            this.m_cbParity.Size = new System.Drawing.Size(126, 21);
            this.m_cbParity.TabIndex = 134;
            this.m_cbParity.SelectedIndexChanged += new System.EventHandler(this.m_cbParity_SelectedIndexChanged);
            // 
            // m_lblDataBits
            // 
            this.m_lblDataBits.AutoSize = true;
            this.m_lblDataBits.Location = new System.Drawing.Point(64, 40);
            this.m_lblDataBits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblDataBits.Name = "m_lblDataBits";
            this.m_lblDataBits.Size = new System.Drawing.Size(52, 13);
            this.m_lblDataBits.TabIndex = 133;
            this.m_lblDataBits.Text = "Data bits:";
            this.m_lblDataBits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_lblBPS
            // 
            this.m_lblBPS.AutoSize = true;
            this.m_lblBPS.Location = new System.Drawing.Point(32, 13);
            this.m_lblBPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.m_lblBPS.Name = "m_lblBPS";
            this.m_lblBPS.Size = new System.Drawing.Size(83, 13);
            this.m_lblBPS.TabIndex = 130;
            this.m_lblBPS.Text = "Bits per second:";
            this.m_lblBPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cbDataBits
            // 
            this.m_cbDataBits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbDataBits.FormattingEnabled = true;
            this.m_cbDataBits.Location = new System.Drawing.Point(136, 37);
            this.m_cbDataBits.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbDataBits.Name = "m_cbDataBits";
            this.m_cbDataBits.Size = new System.Drawing.Size(126, 21);
            this.m_cbDataBits.TabIndex = 132;
            this.m_cbDataBits.SelectedIndexChanged += new System.EventHandler(this.m_cbDataBits_SelectedIndexChanged);
            // 
            // m_cbBitsPerSecond
            // 
            this.m_cbBitsPerSecond.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cbBitsPerSecond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbBitsPerSecond.FormattingEnabled = true;
            this.m_cbBitsPerSecond.Location = new System.Drawing.Point(136, 10);
            this.m_cbBitsPerSecond.Margin = new System.Windows.Forms.Padding(2);
            this.m_cbBitsPerSecond.Name = "m_cbBitsPerSecond";
            this.m_cbBitsPerSecond.Size = new System.Drawing.Size(126, 21);
            this.m_cbBitsPerSecond.TabIndex = 129;
            this.m_cbBitsPerSecond.SelectedIndexChanged += new System.EventHandler(this.m_cbBitsPerSecond_SelectedIndexChanged);
            // 
            // cyuartcontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbRXTX);
            this.Controls.Add(this.gbMain);
            this.Name = "cyuartcontrol";
            this.Size = new System.Drawing.Size(306, 245);
            this.gbRXTX.ResumeLayout(false);
            this.gbRXTX.PerformLayout();
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbRXTX;
        private System.Windows.Forms.RadioButton m_rbRXTX;
        private System.Windows.Forms.RadioButton m_rbTXOnly;
        private System.Windows.Forms.RadioButton m_rbRXOnly;
        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.Label m_lblFlowControl;
        private System.Windows.Forms.Label m_lblStopBits;
        private System.Windows.Forms.ComboBox m_cbFlowControl;
        private System.Windows.Forms.Label m_lblParity;
        private System.Windows.Forms.ComboBox m_cbStopBits;
        private System.Windows.Forms.ComboBox m_cbParity;
        private System.Windows.Forms.Label m_lblDataBits;
        private System.Windows.Forms.Label m_lblBPS;
        private System.Windows.Forms.ComboBox m_cbDataBits;
        private System.Windows.Forms.ComboBox m_cbBitsPerSecond;
    }
}
