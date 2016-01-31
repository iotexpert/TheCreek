namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_20
{
    partial class CyOutputControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_slewRateComboBox = new System.Windows.Forms.ComboBox();
            this.m_driveLevelComboBox = new System.Windows.Forms.ComboBox();
            this.m_currentComboBox = new System.Windows.Forms.ComboBox();
            this.m_outputSyncCheckBox = new System.Windows.Forms.CheckBox();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Slew Rate:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Drive Level:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Current:";
            // 
            // m_slewRateComboBox
            // 
            this.m_slewRateComboBox.FormattingEnabled = true;
            this.m_slewRateComboBox.Location = new System.Drawing.Point(107, 3);
            this.m_slewRateComboBox.Name = "m_slewRateComboBox";
            this.m_slewRateComboBox.Size = new System.Drawing.Size(121, 24);
            this.m_slewRateComboBox.TabIndex = 1;
            // 
            // m_driveLevelComboBox
            // 
            this.m_driveLevelComboBox.FormattingEnabled = true;
            this.m_driveLevelComboBox.Location = new System.Drawing.Point(107, 33);
            this.m_driveLevelComboBox.Name = "m_driveLevelComboBox";
            this.m_driveLevelComboBox.Size = new System.Drawing.Size(121, 24);
            this.m_driveLevelComboBox.TabIndex = 2;
            // 
            // m_currentComboBox
            // 
            this.m_currentComboBox.FormattingEnabled = true;
            this.m_currentComboBox.Location = new System.Drawing.Point(107, 63);
            this.m_currentComboBox.Name = "m_currentComboBox";
            this.m_currentComboBox.Size = new System.Drawing.Size(221, 24);
            this.m_currentComboBox.TabIndex = 3;
            // 
            // m_outputSyncCheckBox
            // 
            this.m_outputSyncCheckBox.AutoSize = true;
            this.m_outputSyncCheckBox.Location = new System.Drawing.Point(107, 93);
            this.m_outputSyncCheckBox.Name = "m_outputSyncCheckBox";
            this.m_outputSyncCheckBox.Size = new System.Drawing.Size(163, 21);
            this.m_outputSyncCheckBox.TabIndex = 4;
            this.m_outputSyncCheckBox.Text = "Output Synchronized";
            this.m_outputSyncCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyOutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.m_outputSyncCheckBox);
            this.Controls.Add(this.m_currentComboBox);
            this.Controls.Add(this.m_driveLevelComboBox);
            this.Controls.Add(this.m_slewRateComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CyOutputControl";
            this.Size = new System.Drawing.Size(369, 131);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox m_slewRateComboBox;
        private System.Windows.Forms.ComboBox m_driveLevelComboBox;
        private System.Windows.Forms.ComboBox m_currentComboBox;
        private System.Windows.Forms.CheckBox m_outputSyncCheckBox;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
    }
}
