/* ========================================
 *
 * Copyright YOUR COMPANY, THE YEAR
 * All Rights Reserved
 * UNPUBLISHED, LICENSED SOFTWARE.
 *
 * CONFIDENTIAL AND PROPRIETARY INFORMATION
 * WHICH IS THE PROPERTY OF your company.
 *
 * ========================================
*/

namespace PGA_Inv_v1_60
{
    partial class CyPgaInvControl
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
            this.m_cbPower = new System.Windows.Forms.ComboBox();
            this.m_cbInvGain = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Power_label = new System.Windows.Forms.Label();
            this.Inverting_Gain_label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_yMax3 = new System.Windows.Forms.Label();
            this.label_yMax2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_lbXUnit = new System.Windows.Forms.Label();
            this.label_yMax1 = new System.Windows.Forms.Label();
            this.label_yMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TD_pictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TD_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // m_cbPower
            // 
            this.m_cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPower.FormattingEnabled = true;
            this.m_cbPower.Location = new System.Drawing.Point(357, 14);
            this.m_cbPower.Name = "m_cbPower";
            this.m_cbPower.Size = new System.Drawing.Size(121, 21);
            this.m_cbPower.TabIndex = 5;
            // 
            // m_cbInvGain
            // 
            this.m_cbInvGain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbInvGain.FormattingEnabled = true;
            this.m_cbInvGain.Location = new System.Drawing.Point(150, 14);
            this.m_cbInvGain.Name = "m_cbInvGain";
            this.m_cbInvGain.Size = new System.Drawing.Size(121, 21);
            this.m_cbInvGain.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_cbPower);
            this.groupBox1.Controls.Add(this.m_cbInvGain);
            this.groupBox1.Controls.Add(this.Power_label);
            this.groupBox1.Controls.Add(this.Inverting_Gain_label);
            this.groupBox1.Location = new System.Drawing.Point(6, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 39);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            // 
            // Power_label
            // 
            this.Power_label.AutoSize = true;
            this.Power_label.Location = new System.Drawing.Point(314, 17);
            this.Power_label.Name = "Power_label";
            this.Power_label.Size = new System.Drawing.Size(37, 13);
            this.Power_label.TabIndex = 2;
            this.Power_label.Text = "Power";
            // 
            // Inverting_Gain_label
            // 
            this.Inverting_Gain_label.AutoSize = true;
            this.Inverting_Gain_label.Location = new System.Drawing.Point(71, 17);
            this.Inverting_Gain_label.Name = "Inverting_Gain_label";
            this.Inverting_Gain_label.Size = new System.Drawing.Size(73, 13);
            this.Inverting_Gain_label.TabIndex = 0;
            this.Inverting_Gain_label.Text = "Inverting Gain";
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
            this.groupBox2.Location = new System.Drawing.Point(6, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(501, 284);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(363, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 39;
            this.label7.Text = "1MHz";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(263, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "100KHz";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "10KHz";
            // 
            // label_yMax3
            // 
            this.label_yMax3.AutoSize = true;
            this.label_yMax3.Location = new System.Drawing.Point(15, 169);
            this.label_yMax3.Name = "label_yMax3";
            this.label_yMax3.Size = new System.Drawing.Size(13, 13);
            this.label_yMax3.TabIndex = 35;
            this.label_yMax3.Text = "1";
            // 
            // label_yMax2
            // 
            this.label_yMax2.AutoSize = true;
            this.label_yMax2.Location = new System.Drawing.Point(15, 102);
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
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 30);
            this.label6.TabIndex = 32;
            this.label6.Text = "InvGain (dB)";
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
            this.label_yMax1.Location = new System.Drawing.Point(15, 35);
            this.label_yMax1.Name = "label_yMax1";
            this.label_yMax1.Size = new System.Drawing.Size(13, 13);
            this.label_yMax1.TabIndex = 30;
            this.label_yMax1.Text = "1";
            // 
            // label_yMin
            // 
            this.label_yMin.AutoSize = true;
            this.label_yMin.Location = new System.Drawing.Point(15, 238);
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
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "10MHz";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 252);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "1KHz";
            // 
            // TD_pictureBox
            // 
            this.TD_pictureBox.Location = new System.Drawing.Point(65, 35);
            this.TD_pictureBox.Name = "TD_pictureBox";
            this.TD_pictureBox.Size = new System.Drawing.Size(426, 216);
            this.TD_pictureBox.TabIndex = 26;
            this.TD_pictureBox.TabStop = false;
            // 
            // CyPgaInvControl
            // 
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyPgaInvControl";
            this.Size = new System.Drawing.Size(514, 334);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TD_pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox m_cbPower;
        private System.Windows.Forms.ComboBox m_cbInvGain;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Power_label;
        private System.Windows.Forms.Label Inverting_Gain_label;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_yMax3;
        private System.Windows.Forms.Label label_yMax2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label m_lbXUnit;
        private System.Windows.Forms.Label label_yMax1;
        private System.Windows.Forms.Label label_yMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox TD_pictureBox;


    }
}
//[] END OF FILE
