/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

namespace Sample_Hold_v1_30
{
    partial class CySampleAndHoldControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.m_cbVrefType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_rbNegative = new System.Windows.Forms.RadioButton();
            this.m_rbPositiveAndNegative = new System.Windows.Forms.RadioButton();
            this.m_cbPower = new System.Windows.Forms.ComboBox();
            this.power_label = new System.Windows.Forms.Label();
            this.m_cbSampleMode = new System.Windows.Forms.ComboBox();
            this.SampleMode_label = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Vref";
            // 
            // m_cbVrefType
            // 
            this.m_cbVrefType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbVrefType.FormattingEnabled = true;
            this.m_cbVrefType.Location = new System.Drawing.Point(82, 62);
            this.m_cbVrefType.Name = "m_cbVrefType";
            this.m_cbVrefType.Size = new System.Drawing.Size(131, 21);
            this.m_cbVrefType.TabIndex = 3;
            this.m_cbVrefType.SelectedIndexChanged += new System.EventHandler(this.m_cbVrefType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_rbNegative);
            this.groupBox1.Controls.Add(this.m_rbPositiveAndNegative);
            this.groupBox1.Location = new System.Drawing.Point(6, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 73);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sample Clock Edge";
            // 
            // m_rbNegative
            // 
            this.m_rbNegative.AutoSize = true;
            this.m_rbNegative.Checked = true;
            this.m_rbNegative.Location = new System.Drawing.Point(6, 19);
            this.m_rbNegative.Name = "m_rbNegative";
            this.m_rbNegative.Size = new System.Drawing.Size(68, 17);
            this.m_rbNegative.TabIndex = 4;
            this.m_rbNegative.TabStop = true;
            this.m_rbNegative.Text = "Negative";
            this.m_rbNegative.UseVisualStyleBackColor = true;
            this.m_rbNegative.CheckedChanged += new System.EventHandler(this.m_rbNegative_CheckedChanged);
            // 
            // m_rbPositiveAndNegative
            // 
            this.m_rbPositiveAndNegative.AutoSize = true;
            this.m_rbPositiveAndNegative.Location = new System.Drawing.Point(6, 42);
            this.m_rbPositiveAndNegative.Name = "m_rbPositiveAndNegative";
            this.m_rbPositiveAndNegative.Size = new System.Drawing.Size(129, 17);
            this.m_rbPositiveAndNegative.TabIndex = 5;
            this.m_rbPositiveAndNegative.TabStop = true;
            this.m_rbPositiveAndNegative.Text = "Positive and Negative";
            this.m_rbPositiveAndNegative.UseVisualStyleBackColor = true;
            this.m_rbPositiveAndNegative.CheckedChanged += new System.EventHandler(this.m_rbPositiveAndNegative_CheckedChanged);
            // 
            // m_cbPower
            // 
            this.m_cbPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbPower.FormattingEnabled = true;
            this.m_cbPower.Location = new System.Drawing.Point(82, 36);
            this.m_cbPower.Name = "m_cbPower";
            this.m_cbPower.Size = new System.Drawing.Size(131, 21);
            this.m_cbPower.TabIndex = 2;
            // 
            // power_label
            // 
            this.power_label.AutoSize = true;
            this.power_label.Location = new System.Drawing.Point(40, 39);
            this.power_label.Name = "power_label";
            this.power_label.Size = new System.Drawing.Size(37, 13);
            this.power_label.TabIndex = 4;
            this.power_label.Text = "Power";
            // 
            // m_cbSampleMode
            // 
            this.m_cbSampleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbSampleMode.FormattingEnabled = true;
            this.m_cbSampleMode.Location = new System.Drawing.Point(82, 10);
            this.m_cbSampleMode.Name = "m_cbSampleMode";
            this.m_cbSampleMode.Size = new System.Drawing.Size(131, 21);
            this.m_cbSampleMode.TabIndex = 1;
            this.m_cbSampleMode.SelectedIndexChanged += new System.EventHandler(this.m_cbSampleMode_SelectedIndexChanged_1);
            // 
            // SampleMode_label
            // 
            this.SampleMode_label.AutoSize = true;
            this.SampleMode_label.Location = new System.Drawing.Point(6, 13);
            this.SampleMode_label.Name = "SampleMode_label";
            this.SampleMode_label.Size = new System.Drawing.Size(71, 13);
            this.SampleMode_label.TabIndex = 2;
            this.SampleMode_label.Text = "Sample mode";
            // 
            // CySampleAndHoldControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_cbVrefType);
            this.Controls.Add(this.SampleMode_label);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_cbSampleMode);
            this.Controls.Add(this.m_cbPower);
            this.Controls.Add(this.power_label);
            this.Name = "CySampleAndHoldControl";
            this.Size = new System.Drawing.Size(222, 175);
            this.Load += new System.EventHandler(this.CySampleAndHoldControl_Load_1);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton m_rbNegative;
        private System.Windows.Forms.RadioButton m_rbPositiveAndNegative;
        private System.Windows.Forms.ComboBox m_cbPower;
        private System.Windows.Forms.Label power_label;
        private System.Windows.Forms.ComboBox m_cbSampleMode;
        private System.Windows.Forms.Label SampleMode_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_cbVrefType;
    }
}


/* [] END OF FILE */
