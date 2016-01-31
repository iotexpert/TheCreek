namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    partial class CyGeneralControl
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
            this.m_driveModeGroupBox = new System.Windows.Forms.GroupBox();
            this.m_driveModeComboBox = new System.Windows.Forms.ComboBox();
            this.m_driveModePictureBox = new System.Windows.Forms.PictureBox();
            this.m_initialStateLabel = new System.Windows.Forms.Label();
            this.m_initStateComboBox = new System.Windows.Forms.ComboBox();
            this.m_supplyVoltageLabel = new System.Windows.Forms.Label();
            this.m_supplyVoltageTextBox = new System.Windows.Forms.TextBox();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_driveModeErrorLocLabel = new System.Windows.Forms.Label();
            this.m_scrollHelperLabel = new System.Windows.Forms.Label();
            this.m_driveModeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_driveModePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // m_driveModeGroupBox
            // 
            this.m_driveModeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_driveModeGroupBox.Controls.Add(this.m_driveModeComboBox);
            this.m_driveModeGroupBox.Controls.Add(this.m_driveModePictureBox);
            this.m_driveModeGroupBox.Location = new System.Drawing.Point(3, 3);
            this.m_driveModeGroupBox.MinimumSize = new System.Drawing.Size(182, 0);
            this.m_driveModeGroupBox.Name = "m_driveModeGroupBox";
            this.m_driveModeGroupBox.Size = new System.Drawing.Size(182, 258);
            this.m_driveModeGroupBox.TabIndex = 0;
            this.m_driveModeGroupBox.TabStop = false;
            this.m_driveModeGroupBox.Text = "Drive Mode";
            // 
            // m_driveModeComboBox
            // 
            this.m_driveModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_driveModeComboBox.FormattingEnabled = true;
            this.m_driveModeComboBox.Location = new System.Drawing.Point(6, 21);
            this.m_driveModeComboBox.Name = "m_driveModeComboBox";
            this.m_driveModeComboBox.Size = new System.Drawing.Size(168, 24);
            this.m_driveModeComboBox.TabIndex = 1;
            // 
            // m_driveModePictureBox
            // 
            this.m_driveModePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_driveModePictureBox.Location = new System.Drawing.Point(6, 51);
            this.m_driveModePictureBox.Name = "m_driveModePictureBox";
            this.m_driveModePictureBox.Size = new System.Drawing.Size(167, 198);
            this.m_driveModePictureBox.TabIndex = 9;
            this.m_driveModePictureBox.TabStop = false;
            // 
            // m_initialStateLabel
            // 
            this.m_initialStateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_initialStateLabel.AutoSize = true;
            this.m_initialStateLabel.Location = new System.Drawing.Point(190, 4);
            this.m_initialStateLabel.Name = "m_initialStateLabel";
            this.m_initialStateLabel.Size = new System.Drawing.Size(81, 17);
            this.m_initialStateLabel.TabIndex = 1;
            this.m_initialStateLabel.Text = "Initial State:";
            // 
            // m_initStateComboBox
            // 
            this.m_initStateComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_initStateComboBox.FormattingEnabled = true;
            this.m_initStateComboBox.Location = new System.Drawing.Point(193, 24);
            this.m_initStateComboBox.Name = "m_initStateComboBox";
            this.m_initStateComboBox.Size = new System.Drawing.Size(104, 24);
            this.m_initStateComboBox.TabIndex = 2;
            // 
            // m_supplyVoltageLabel
            // 
            this.m_supplyVoltageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_supplyVoltageLabel.AutoSize = true;
            this.m_supplyVoltageLabel.Location = new System.Drawing.Point(188, 64);
            this.m_supplyVoltageLabel.Name = "m_supplyVoltageLabel";
            this.m_supplyVoltageLabel.Size = new System.Drawing.Size(166, 17);
            this.m_supplyVoltageLabel.TabIndex = 3;
            this.m_supplyVoltageLabel.Text = "Minimum Supply Voltage:";
            // 
            // m_supplyVoltageTextBox
            // 
            this.m_supplyVoltageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_supplyVoltageTextBox.Location = new System.Drawing.Point(191, 84);
            this.m_supplyVoltageTextBox.Name = "m_supplyVoltageTextBox";
            this.m_supplyVoltageTextBox.Size = new System.Drawing.Size(104, 22);
            this.m_supplyVoltageTextBox.TabIndex = 3;
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            // 
            // m_driveModeErrorLocLabel
            // 
            this.m_driveModeErrorLocLabel.AutoSize = true;
            this.m_driveModeErrorLocLabel.Location = new System.Drawing.Point(91, 3);
            this.m_driveModeErrorLocLabel.Name = "m_driveModeErrorLocLabel";
            this.m_driveModeErrorLocLabel.Size = new System.Drawing.Size(0, 17);
            this.m_driveModeErrorLocLabel.TabIndex = 10;
            // 
            // m_scrollHelperLabel
            // 
            this.m_scrollHelperLabel.AutoSize = true;
            this.m_scrollHelperLabel.Location = new System.Drawing.Point(354, 92);
            this.m_scrollHelperLabel.Name = "m_scrollHelperLabel";
            this.m_scrollHelperLabel.Size = new System.Drawing.Size(0, 17);
            this.m_scrollHelperLabel.TabIndex = 11;
            // 
            // CyGeneralControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.m_scrollHelperLabel);
            this.Controls.Add(this.m_supplyVoltageTextBox);
            this.Controls.Add(this.m_supplyVoltageLabel);
            this.Controls.Add(this.m_initStateComboBox);
            this.Controls.Add(this.m_initialStateLabel);
            this.Controls.Add(this.m_driveModeGroupBox);
            this.Controls.Add(this.m_driveModeErrorLocLabel);
            this.Name = "CyGeneralControl";
            this.Size = new System.Drawing.Size(357, 263);
            this.m_driveModeGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_driveModePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox m_driveModeGroupBox;
        private System.Windows.Forms.PictureBox m_driveModePictureBox;
        private System.Windows.Forms.Label m_initialStateLabel;
        private System.Windows.Forms.ComboBox m_initStateComboBox;
        private System.Windows.Forms.Label m_supplyVoltageLabel;
        private System.Windows.Forms.TextBox m_supplyVoltageTextBox;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
        private System.Windows.Forms.Label m_driveModeErrorLocLabel;
        private System.Windows.Forms.ComboBox m_driveModeComboBox;
        private System.Windows.Forms.Label m_scrollHelperLabel;
    }
}
