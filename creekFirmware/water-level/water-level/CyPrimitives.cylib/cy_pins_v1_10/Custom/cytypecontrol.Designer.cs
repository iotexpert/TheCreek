namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    partial class CyTypeControl
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
            this.m_analogCheckBox = new System.Windows.Forms.CheckBox();
            this.m_digInputCheckBox = new System.Windows.Forms.CheckBox();
            this.m_oeCheckBox = new System.Windows.Forms.CheckBox();
            this.m_digOutputCheckBox = new System.Windows.Forms.CheckBox();
            this.m_displayDigitalOutputCheckBox = new System.Windows.Forms.CheckBox();
            this.m_displayDigitalInputCheckBox = new System.Windows.Forms.CheckBox();
            this.m_pinPicture = new Cypress.Comps.PinsAndPorts.cy_pins_v1_10.CyPinPicture();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_previewLabel = new System.Windows.Forms.Label();
            this.m_bidirCheckBox = new System.Windows.Forms.CheckBox();
            this.m_bidirPanel = new System.Windows.Forms.Panel();
            this.m_inoutPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.m_pinPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.m_bidirPanel.SuspendLayout();
            this.m_inoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_analogCheckBox
            // 
            this.m_analogCheckBox.AutoSize = true;
            this.m_analogCheckBox.Location = new System.Drawing.Point(6, 5);
            this.m_analogCheckBox.Name = "m_analogCheckBox";
            this.m_analogCheckBox.Size = new System.Drawing.Size(74, 21);
            this.m_analogCheckBox.TabIndex = 1;
            this.m_analogCheckBox.Text = "Analog";
            this.m_analogCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_digInputCheckBox
            // 
            this.m_digInputCheckBox.AutoSize = true;
            this.m_digInputCheckBox.Location = new System.Drawing.Point(3, 3);
            this.m_digInputCheckBox.Name = "m_digInputCheckBox";
            this.m_digInputCheckBox.Size = new System.Drawing.Size(104, 21);
            this.m_digInputCheckBox.TabIndex = 2;
            this.m_digInputCheckBox.Text = "Digital Input";
            this.m_digInputCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_oeCheckBox
            // 
            this.m_oeCheckBox.AutoSize = true;
            this.m_oeCheckBox.Location = new System.Drawing.Point(28, 111);
            this.m_oeCheckBox.Name = "m_oeCheckBox";
            this.m_oeCheckBox.Size = new System.Drawing.Size(117, 21);
            this.m_oeCheckBox.TabIndex = 6;
            this.m_oeCheckBox.Text = "Ouput Enable";
            this.m_oeCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_digOutputCheckBox
            // 
            this.m_digOutputCheckBox.AutoSize = true;
            this.m_digOutputCheckBox.Location = new System.Drawing.Point(3, 57);
            this.m_digOutputCheckBox.Name = "m_digOutputCheckBox";
            this.m_digOutputCheckBox.Size = new System.Drawing.Size(116, 21);
            this.m_digOutputCheckBox.TabIndex = 4;
            this.m_digOutputCheckBox.Text = "Digital Output";
            this.m_digOutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_displayDigitalOutputCheckBox
            // 
            this.m_displayDigitalOutputCheckBox.AutoSize = true;
            this.m_displayDigitalOutputCheckBox.Location = new System.Drawing.Point(28, 84);
            this.m_displayDigitalOutputCheckBox.Name = "m_displayDigitalOutputCheckBox";
            this.m_displayDigitalOutputCheckBox.Size = new System.Drawing.Size(151, 21);
            this.m_displayDigitalOutputCheckBox.TabIndex = 5;
            this.m_displayDigitalOutputCheckBox.Text = "HW Connection";
            this.m_displayDigitalOutputCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_displayDigitalInputCheckBox
            // 
            this.m_displayDigitalInputCheckBox.AutoSize = true;
            this.m_displayDigitalInputCheckBox.Location = new System.Drawing.Point(28, 30);
            this.m_displayDigitalInputCheckBox.Name = "m_displayDigitalInputCheckBox";
            this.m_displayDigitalInputCheckBox.Size = new System.Drawing.Size(151, 21);
            this.m_displayDigitalInputCheckBox.TabIndex = 3;
            this.m_displayDigitalInputCheckBox.Text = "HW Connection";
            this.m_displayDigitalInputCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_pinPicture
            // 
            this.m_pinPicture.BackColor = System.Drawing.Color.White;
            this.m_pinPicture.Location = new System.Drawing.Point(228, 30);
            this.m_pinPicture.Name = "m_pinPicture";
            this.m_pinPicture.ShowAnalog = false;
            this.m_pinPicture.ShowBidirectional = false;
            this.m_pinPicture.ShowDigitalInput = false;
            this.m_pinPicture.ShowDigitalInputConnection = false;
            this.m_pinPicture.ShowDigitalOutput = false;
            this.m_pinPicture.ShowDigitalOutputConnection = false;
            this.m_pinPicture.ShowOutputEnable = false;
            this.m_pinPicture.Size = new System.Drawing.Size(200, 72);
            this.m_pinPicture.TabIndex = 8;
            this.m_pinPicture.TabStop = false;
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            // 
            // m_previewLabel
            // 
            this.m_previewLabel.AutoSize = true;
            this.m_previewLabel.Location = new System.Drawing.Point(225, 6);
            this.m_previewLabel.Name = "m_previewLabel";
            this.m_previewLabel.Size = new System.Drawing.Size(61, 17);
            this.m_previewLabel.TabIndex = 9;
            this.m_previewLabel.Text = "Preview:";
            // 
            // m_bidirCheckBox
            // 
            this.m_bidirCheckBox.AutoSize = true;
            this.m_bidirCheckBox.Location = new System.Drawing.Point(3, 3);
            this.m_bidirCheckBox.Name = "m_bidirCheckBox";
            this.m_bidirCheckBox.Size = new System.Drawing.Size(107, 21);
            this.m_bidirCheckBox.TabIndex = 7;
            this.m_bidirCheckBox.Text = "Bidirectional";
            this.m_bidirCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_bidirPanel
            // 
            this.m_bidirPanel.BackColor = System.Drawing.SystemColors.Control;
            this.m_bidirPanel.Controls.Add(this.m_bidirCheckBox);
            this.m_bidirPanel.Location = new System.Drawing.Point(3, 170);
            this.m_bidirPanel.Name = "m_bidirPanel";
            this.m_bidirPanel.Size = new System.Drawing.Size(197, 26);
            this.m_bidirPanel.TabIndex = 12;
            // 
            // m_inoutPanel
            // 
            this.m_inoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.m_inoutPanel.Controls.Add(this.m_digInputCheckBox);
            this.m_inoutPanel.Controls.Add(this.m_digOutputCheckBox);
            this.m_inoutPanel.Controls.Add(this.m_displayDigitalOutputCheckBox);
            this.m_inoutPanel.Controls.Add(this.m_displayDigitalInputCheckBox);
            this.m_inoutPanel.Controls.Add(this.m_oeCheckBox);
            this.m_inoutPanel.Location = new System.Drawing.Point(3, 30);
            this.m_inoutPanel.Name = "m_inoutPanel";
            this.m_inoutPanel.Size = new System.Drawing.Size(197, 134);
            this.m_inoutPanel.TabIndex = 11;
            // 
            // CyTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.m_bidirPanel);
            this.Controls.Add(this.m_inoutPanel);
            this.Controls.Add(this.m_previewLabel);
            this.Controls.Add(this.m_pinPicture);
            this.Controls.Add(this.m_analogCheckBox);
            this.Name = "CyTypeControl";
            this.Size = new System.Drawing.Size(439, 309);
            ((System.ComponentModel.ISupportInitialize)(this.m_pinPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.m_bidirPanel.ResumeLayout(false);
            this.m_bidirPanel.PerformLayout();
            this.m_inoutPanel.ResumeLayout(false);
            this.m_inoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_analogCheckBox;
        private System.Windows.Forms.CheckBox m_digInputCheckBox;
        private System.Windows.Forms.CheckBox m_oeCheckBox;
        private System.Windows.Forms.CheckBox m_digOutputCheckBox;
        private CyPinPicture m_pinPicture;
        private System.Windows.Forms.CheckBox m_displayDigitalOutputCheckBox;
        private System.Windows.Forms.CheckBox m_displayDigitalInputCheckBox;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
        private System.Windows.Forms.Label m_previewLabel;
        private System.Windows.Forms.Panel m_bidirPanel;
        private System.Windows.Forms.CheckBox m_bidirCheckBox;
        private System.Windows.Forms.Panel m_inoutPanel;
    }
}
