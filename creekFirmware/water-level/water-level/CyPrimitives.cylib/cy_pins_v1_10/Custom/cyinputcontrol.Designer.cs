namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    partial class CyInputControl
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
            this.m_thresholdComboBox = new System.Windows.Forms.ComboBox();
            this.m_hysteresisCheckBox = new System.Windows.Forms.CheckBox();
            this.m_hotSwapCheckBox = new System.Windows.Forms.CheckBox();
            this.m_inputBufferEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.m_inputSyncCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_interruptComboBox = new System.Windows.Forms.ComboBox();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Threshold:";
            // 
            // m_thresholdComboBox
            // 
            this.m_thresholdComboBox.FormattingEnabled = true;
            this.m_thresholdComboBox.Location = new System.Drawing.Point(95, 3);
            this.m_thresholdComboBox.Name = "m_thresholdComboBox";
            this.m_thresholdComboBox.Size = new System.Drawing.Size(141, 24);
            this.m_thresholdComboBox.TabIndex = 0;
            // 
            // m_hysteresisCheckBox
            // 
            this.m_hysteresisCheckBox.AutoSize = true;
            this.m_hysteresisCheckBox.Location = new System.Drawing.Point(259, 5);
            this.m_hysteresisCheckBox.Name = "m_hysteresisCheckBox";
            this.m_hysteresisCheckBox.Size = new System.Drawing.Size(96, 21);
            this.m_hysteresisCheckBox.TabIndex = 1;
            this.m_hysteresisCheckBox.Text = "Hysteresis";
            this.m_hysteresisCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_hotSwapCheckBox
            // 
            this.m_hotSwapCheckBox.AutoSize = true;
            this.m_hotSwapCheckBox.Location = new System.Drawing.Point(95, 63);
            this.m_hotSwapCheckBox.Name = "m_hotSwapCheckBox";
            this.m_hotSwapCheckBox.Size = new System.Drawing.Size(90, 21);
            this.m_hotSwapCheckBox.TabIndex = 3;
            this.m_hotSwapCheckBox.Text = "Hot Swap";
            this.m_hotSwapCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_inputBufferEnabledCheckBox
            // 
            this.m_inputBufferEnabledCheckBox.AutoSize = true;
            this.m_inputBufferEnabledCheckBox.Location = new System.Drawing.Point(95, 90);
            this.m_inputBufferEnabledCheckBox.Name = "m_inputBufferEnabledCheckBox";
            this.m_inputBufferEnabledCheckBox.Size = new System.Drawing.Size(159, 21);
            this.m_inputBufferEnabledCheckBox.TabIndex = 4;
            this.m_inputBufferEnabledCheckBox.Text = "Input Buffer Enabled";
            this.m_inputBufferEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_inputSyncCheckBox
            // 
            this.m_inputSyncCheckBox.AutoSize = true;
            this.m_inputSyncCheckBox.Location = new System.Drawing.Point(95, 117);
            this.m_inputSyncCheckBox.Name = "m_inputSyncCheckBox";
            this.m_inputSyncCheckBox.Size = new System.Drawing.Size(151, 21);
            this.m_inputSyncCheckBox.TabIndex = 5;
            this.m_inputSyncCheckBox.Text = "Input Synchronized";
            this.m_inputSyncCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Interrupt:";
            // 
            // m_interruptComboBox
            // 
            this.m_interruptComboBox.FormattingEnabled = true;
            this.m_interruptComboBox.Location = new System.Drawing.Point(95, 33);
            this.m_interruptComboBox.Name = "m_interruptComboBox";
            this.m_interruptComboBox.Size = new System.Drawing.Size(141, 24);
            this.m_interruptComboBox.TabIndex = 2;
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            // 
            // CyInputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.m_interruptComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_inputSyncCheckBox);
            this.Controls.Add(this.m_inputBufferEnabledCheckBox);
            this.Controls.Add(this.m_hotSwapCheckBox);
            this.Controls.Add(this.m_hysteresisCheckBox);
            this.Controls.Add(this.m_thresholdComboBox);
            this.Controls.Add(this.label1);
            this.Name = "CyInputControl";
            this.Size = new System.Drawing.Size(361, 145);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_thresholdComboBox;
        private System.Windows.Forms.CheckBox m_hysteresisCheckBox;
        private System.Windows.Forms.CheckBox m_hotSwapCheckBox;
        private System.Windows.Forms.CheckBox m_inputBufferEnabledCheckBox;
        private System.Windows.Forms.CheckBox m_inputSyncCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox m_interruptComboBox;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
    }
}
