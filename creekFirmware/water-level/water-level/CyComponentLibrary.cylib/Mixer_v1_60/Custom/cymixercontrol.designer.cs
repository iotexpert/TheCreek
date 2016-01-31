namespace Mixer_v1_60
{
    partial class CyMixerControl
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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Setup_groupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_rbInternal_LO = new System.Windows.Forms.RadioButton();
            this.m_rbExternal_LO = new System.Windows.Forms.RadioButton();
            this.LO_Frequency_Label = new System.Windows.Forms.Label();
            this.m_tbLO_Frequency = new System.Windows.Forms.TextBox();
            this.Power_label = new System.Windows.Forms.Label();
            this.m_cbPower = new System.Windows.Forms.ComboBox();
            this.Mixet_Type_label = new System.Windows.Forms.Label();
            this.m_cbMixerType = new System.Windows.Forms.ComboBox();
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.Setup_groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // Setup_groupBox
            // 
            this.Setup_groupBox.Controls.Add(this.label2);
            this.Setup_groupBox.Controls.Add(this.label1);
            this.Setup_groupBox.Controls.Add(this.groupBox1);
            this.Setup_groupBox.Controls.Add(this.LO_Frequency_Label);
            this.Setup_groupBox.Controls.Add(this.m_tbLO_Frequency);
            this.Setup_groupBox.Controls.Add(this.Power_label);
            this.Setup_groupBox.Controls.Add(this.m_cbPower);
            this.Setup_groupBox.Controls.Add(this.Mixet_Type_label);
            this.Setup_groupBox.Controls.Add(this.m_cbMixerType);
            this.Setup_groupBox.Location = new System.Drawing.Point(12, 9);
            this.Setup_groupBox.Name = "Setup_groupBox";
            this.Setup_groupBox.Size = new System.Drawing.Size(291, 274);
            this.Setup_groupBox.TabIndex = 2;
            this.Setup_groupBox.TabStop = false;
            this.Setup_groupBox.Text = "Setup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "LO Frequency";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_rbInternal_LO);
            this.groupBox1.Controls.Add(this.m_rbExternal_LO);
            this.groupBox1.Location = new System.Drawing.Point(10, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LO Source";
            // 
            // m_rbInternal_LO
            // 
            this.m_rbInternal_LO.AutoSize = true;
            this.m_rbInternal_LO.Checked = true;
            this.m_rbInternal_LO.Location = new System.Drawing.Point(6, 30);
            this.m_rbInternal_LO.Name = "m_rbInternal_LO";
            this.m_rbInternal_LO.Size = new System.Drawing.Size(77, 17);
            this.m_rbInternal_LO.TabIndex = 4;
            this.m_rbInternal_LO.TabStop = true;
            this.m_rbInternal_LO.Text = "Internal LO";
            this.m_rbInternal_LO.UseVisualStyleBackColor = true;
            // 
            // m_rbExternal_LO
            // 
            this.m_rbExternal_LO.AutoSize = true;
            this.m_rbExternal_LO.Location = new System.Drawing.Point(6, 64);
            this.m_rbExternal_LO.Name = "m_rbExternal_LO";
            this.m_rbExternal_LO.Size = new System.Drawing.Size(80, 17);
            this.m_rbExternal_LO.TabIndex = 5;
            this.m_rbExternal_LO.TabStop = true;
            this.m_rbExternal_LO.Text = "External LO";
            this.m_rbExternal_LO.UseVisualStyleBackColor = true;
            // 
            // LO_Frequency_Label
            // 
            this.LO_Frequency_Label.AutoSize = true;
            this.LO_Frequency_Label.Location = new System.Drawing.Point(139, 219);
            this.LO_Frequency_Label.Name = "LO_Frequency_Label";
            this.LO_Frequency_Label.Size = new System.Drawing.Size(26, 13);
            this.LO_Frequency_Label.TabIndex = 7;
            this.LO_Frequency_Label.Text = "kHz";
            // 
            // m_tbLO_Frequency
            // 
            this.m_tbLO_Frequency.Location = new System.Drawing.Point(82, 215);
            this.m_tbLO_Frequency.Name = "m_tbLO_Frequency";
            this.m_tbLO_Frequency.Size = new System.Drawing.Size(57, 20);
            this.m_tbLO_Frequency.TabIndex = 6;
            this.m_tbLO_Frequency.Text = "4000.00";
            this.m_tbLO_Frequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Power_label
            // 
            this.Power_label.AutoSize = true;
            this.Power_label.Location = new System.Drawing.Point(29, 67);
            this.Power_label.Name = "Power_label";
            this.Power_label.Size = new System.Drawing.Size(37, 13);
            this.Power_label.TabIndex = 3;
            this.Power_label.Text = "Power";
            // 
            // m_cbPower
            // 
            this.m_cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPower.FormattingEnabled = true;
            this.m_cbPower.Location = new System.Drawing.Point(67, 64);
            this.m_cbPower.Name = "m_cbPower";
            this.m_cbPower.Size = new System.Drawing.Size(131, 21);
            this.m_cbPower.TabIndex = 2;
            // 
            // Mixet_Type_label
            // 
            this.Mixet_Type_label.AutoSize = true;
            this.Mixet_Type_label.Location = new System.Drawing.Point(7, 27);
            this.Mixet_Type_label.Name = "Mixet_Type_label";
            this.Mixet_Type_label.Size = new System.Drawing.Size(59, 13);
            this.Mixet_Type_label.TabIndex = 1;
            this.Mixet_Type_label.Text = "Mixer Type";
            // 
            // m_cbMixerType
            // 
            this.m_cbMixerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbMixerType.FormattingEnabled = true;
            this.m_cbMixerType.Location = new System.Drawing.Point(67, 24);
            this.m_cbMixerType.Name = "m_cbMixerType";
            this.m_cbMixerType.Size = new System.Drawing.Size(131, 21);
            this.m_cbMixerType.TabIndex = 0;
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkRate = 0;
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Lime;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(7, 238);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(278, 33);
            this.label2.TabIndex = 10;
            this.label2.Text = "Frequency of the LO input is not known to the design. Please set proper frequency value in the abo" +
                "ve text box for mixer to operate properly.";
            // 
            // CyMixerControl
            // 
            this.Controls.Add(this.Setup_groupBox);
            this.Name = "CyMixerControl";
            this.Size = new System.Drawing.Size(319, 302);
            this.Load += new System.EventHandler(this.CyMixerControl_Load);
            this.Setup_groupBox.ResumeLayout(false);
            this.Setup_groupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.GroupBox Setup_groupBox;
        private System.Windows.Forms.ComboBox m_cbPower;
        private System.Windows.Forms.Label Mixet_Type_label;
        private System.Windows.Forms.ComboBox m_cbMixerType;
        private System.Windows.Forms.Label LO_Frequency_Label;
        private System.Windows.Forms.TextBox m_tbLO_Frequency;
        private System.Windows.Forms.RadioButton m_rbExternal_LO;
        private System.Windows.Forms.RadioButton m_rbInternal_LO;
        private System.Windows.Forms.Label Power_label;
        private System.Windows.Forms.ErrorProvider m_errorProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;

    }
}
