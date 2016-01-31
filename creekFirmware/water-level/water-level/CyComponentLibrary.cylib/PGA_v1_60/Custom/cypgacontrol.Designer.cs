namespace PGA_v1_60
{
    partial class CyPgaControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.m_lbXUnit = new System.Windows.Forms.Label();
            this.label_yMax1 = new System.Windows.Forms.Label();
            this.label_yMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TD_pictureBox = new System.Windows.Forms.PictureBox();
            this.m_cbVrefInput = new System.Windows.Forms.ComboBox();
            this.m_cbPower = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_cbGain = new System.Windows.Forms.ComboBox();
            this.PGA_VrefInput_label = new System.Windows.Forms.Label();
            this.PGA_Power_label = new System.Windows.Forms.Label();
            this.PGA_Gain_label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_yMax3 = new System.Windows.Forms.Label();
            this.label_yMax2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TD_pictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(20, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 30);
            this.label6.TabIndex = 32;
            this.label6.Text = "Gain (dB)";
            // 
            // m_lbXUnit
            // 
            this.m_lbXUnit.AutoSize = true;
            this.m_lbXUnit.Location = new System.Drawing.Point(237, 266);
            this.m_lbXUnit.Name = "m_lbXUnit";
            this.m_lbXUnit.Size = new System.Drawing.Size(117, 13);
            this.m_lbXUnit.TabIndex = 31;
            this.m_lbXUnit.Text = "Frequency  (Log) ------->";
            // 
            // label_yMax1
            // 
            this.label_yMax1.AutoSize = true;
            this.label_yMax1.Location = new System.Drawing.Point(20, 35);
            this.label_yMax1.Name = "label_yMax1";
            this.label_yMax1.Size = new System.Drawing.Size(13, 13);
            this.label_yMax1.TabIndex = 30;
            this.label_yMax1.Text = "1";
            // 
            // label_yMin
            // 
            this.label_yMin.AutoSize = true;
            this.label_yMin.Location = new System.Drawing.Point(20, 238);
            this.label_yMin.Name = "label_yMin";
            this.label_yMin.Size = new System.Drawing.Size(13, 13);
            this.label_yMin.TabIndex = 29;
            this.label_yMin.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(459, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "10 MHz";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 252);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "1 KHz";
            // 
            // TD_pictureBox
            // 
            this.TD_pictureBox.Location = new System.Drawing.Point(65, 35);
            this.TD_pictureBox.Name = "TD_pictureBox";
            this.TD_pictureBox.Size = new System.Drawing.Size(426, 216);
            this.TD_pictureBox.TabIndex = 26;
            this.TD_pictureBox.TabStop = false;
            // 
            // m_cbVrefInput
            // 
            this.m_cbVrefInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbVrefInput.FormattingEnabled = true;
            this.m_cbVrefInput.Location = new System.Drawing.Point(86, 102);
            this.m_cbVrefInput.Name = "m_cbVrefInput";
            this.m_cbVrefInput.Size = new System.Drawing.Size(121, 21);
            this.m_cbVrefInput.TabIndex = 7;
            // 
            // m_cbPower
            // 
            this.m_cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPower.FormattingEnabled = true;
            this.m_cbPower.Location = new System.Drawing.Point(86, 59);
            this.m_cbPower.Name = "m_cbPower";
            this.m_cbPower.Size = new System.Drawing.Size(121, 21);
            this.m_cbPower.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_cbVrefInput);
            this.groupBox1.Controls.Add(this.m_cbPower);
            this.groupBox1.Controls.Add(this.m_cbGain);
            this.groupBox1.Controls.Add(this.PGA_VrefInput_label);
            this.groupBox1.Controls.Add(this.PGA_Power_label);
            this.groupBox1.Controls.Add(this.PGA_Gain_label);
            this.groupBox1.Location = new System.Drawing.Point(5, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 149);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            // 
            // m_cbGain
            // 
            this.m_cbGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbGain.FormattingEnabled = true;
            this.m_cbGain.Location = new System.Drawing.Point(86, 20);
            this.m_cbGain.Name = "m_cbGain";
            this.m_cbGain.Size = new System.Drawing.Size(121, 21);
            this.m_cbGain.TabIndex = 4;
            // 
            // PGA_VrefInput_label
            // 
            this.PGA_VrefInput_label.AutoSize = true;
            this.PGA_VrefInput_label.Location = new System.Drawing.Point(4, 102);
            this.PGA_VrefInput_label.Name = "PGA_VrefInput_label";
            this.PGA_VrefInput_label.Size = new System.Drawing.Size(56, 13);
            this.PGA_VrefInput_label.TabIndex = 3;
            this.PGA_VrefInput_label.Text = "Vref_Input";
            // 
            // PGA_Power_label
            // 
            this.PGA_Power_label.AutoSize = true;
            this.PGA_Power_label.Location = new System.Drawing.Point(4, 59);
            this.PGA_Power_label.Name = "PGA_Power_label";
            this.PGA_Power_label.Size = new System.Drawing.Size(37, 13);
            this.PGA_Power_label.TabIndex = 2;
            this.PGA_Power_label.Text = "Power";
            // 
            // PGA_Gain_label
            // 
            this.PGA_Gain_label.AutoSize = true;
            this.PGA_Gain_label.Location = new System.Drawing.Point(4, 20);
            this.PGA_Gain_label.Name = "PGA_Gain_label";
            this.PGA_Gain_label.Size = new System.Drawing.Size(29, 13);
            this.PGA_Gain_label.TabIndex = 0;
            this.PGA_Gain_label.Text = "Gain";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label_yMax3);
            this.groupBox2.Controls.Add(this.label_yMax2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.m_lbXUnit);
            this.groupBox2.Controls.Add(this.label_yMax1);
            this.groupBox2.Controls.Add(this.label_yMin);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.TD_pictureBox);
            this.groupBox2.Location = new System.Drawing.Point(230, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(528, 284);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(363, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "1 MHz";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(263, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "100 KHz";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "10 kHz";
            // 
            // label_yMax3
            // 
            this.label_yMax3.AutoSize = true;
            this.label_yMax3.Location = new System.Drawing.Point(20, 169);
            this.label_yMax3.Name = "label_yMax3";
            this.label_yMax3.Size = new System.Drawing.Size(13, 13);
            this.label_yMax3.TabIndex = 35;
            this.label_yMax3.Text = "1";
            // 
            // label_yMax2
            // 
            this.label_yMax2.AutoSize = true;
            this.label_yMax2.Location = new System.Drawing.Point(20, 102);
            this.label_yMax2.Name = "label_yMax2";
            this.label_yMax2.Size = new System.Drawing.Size(13, 13);
            this.label_yMax2.TabIndex = 34;
            this.label_yMax2.Text = "1";
            this.label_yMax2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(199, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Frequency  Response";
            // 
            // CyPgaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyPgaControl";
            this.Size = new System.Drawing.Size(759, 293);
            ((System.ComponentModel.ISupportInitialize)(this.TD_pictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label m_lbXUnit;
        private System.Windows.Forms.Label label_yMax1;
        private System.Windows.Forms.Label label_yMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox TD_pictureBox;
        private System.Windows.Forms.ComboBox m_cbVrefInput;
        private System.Windows.Forms.ComboBox m_cbPower;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox m_cbGain;
        private System.Windows.Forms.Label PGA_VrefInput_label;
        private System.Windows.Forms.Label PGA_Power_label;
        private System.Windows.Forms.Label PGA_Gain_label;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_yMax2;
        private System.Windows.Forms.Label label_yMax3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;


    }
}
