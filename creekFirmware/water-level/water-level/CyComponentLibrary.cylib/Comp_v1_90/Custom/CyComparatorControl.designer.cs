/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

namespace Comp_v1_90
{
    partial class CyComparatorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyComparatorControl));
            this.m_errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.m_rbHysteresis_Enable = new System.Windows.Forms.RadioButton();
            this.m_rbHysteresis_Disable = new System.Windows.Forms.RadioButton();
            this.m_rbPDOverride_Enable = new System.Windows.Forms.RadioButton();
            this.m_rbPolarity_NonInve = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_rbPDOverride_Disable = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_rbPolarity_Inve = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.m_rbPower_Fast = new System.Windows.Forms.RadioButton();
            this.m_rbPower_Slow = new System.Windows.Forms.RadioButton();
            this.m_rbPower_Off = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.m_rbSync_Bypass = new System.Windows.Forms.RadioButton();
            this.m_rbSync_Norm = new System.Windows.Forms.RadioButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_errorProvider
            // 
            this.m_errorProvider.BlinkRate = 0;
            this.m_errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.m_errorProvider.ContainerControl = this;
            resources.ApplyResources(this.m_errorProvider, "m_errorProvider");
            // 
            // m_rbHysteresis_Enable
            // 
            resources.ApplyResources(this.m_rbHysteresis_Enable, "m_rbHysteresis_Enable");
            this.m_rbHysteresis_Enable.Name = "m_rbHysteresis_Enable";
            this.m_rbHysteresis_Enable.UseVisualStyleBackColor = true;
            this.m_rbHysteresis_Enable.CheckedChanged += new System.EventHandler(this.m_rbHysteresis_Enable_CheckedChanged);
            // 
            // m_rbHysteresis_Disable
            // 
            resources.ApplyResources(this.m_rbHysteresis_Disable, "m_rbHysteresis_Disable");
            this.m_rbHysteresis_Disable.Name = "m_rbHysteresis_Disable";
            this.m_rbHysteresis_Disable.UseVisualStyleBackColor = true;
            this.m_rbHysteresis_Disable.CheckedChanged += new System.EventHandler(this.m_rbHysteresis_Disable_CheckedChanged);
            // 
            // m_rbPDOverride_Enable
            // 
            resources.ApplyResources(this.m_rbPDOverride_Enable, "m_rbPDOverride_Enable");
            this.m_rbPDOverride_Enable.Name = "m_rbPDOverride_Enable";
            this.m_rbPDOverride_Enable.UseVisualStyleBackColor = true;
            this.m_rbPDOverride_Enable.CheckedChanged += new System.EventHandler(this.m_rbPDOverride_Enable_CheckedChanged);
            // 
            // m_rbPolarity_NonInve
            // 
            resources.ApplyResources(this.m_rbPolarity_NonInve, "m_rbPolarity_NonInve");
            this.m_rbPolarity_NonInve.Name = "m_rbPolarity_NonInve";
            this.m_rbPolarity_NonInve.UseVisualStyleBackColor = true;
            this.m_rbPolarity_NonInve.CheckedChanged += new System.EventHandler(this.m_rbPolarity_NonInve_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_rbHysteresis_Enable);
            this.groupBox1.Controls.Add(this.m_rbHysteresis_Disable);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.m_rbPDOverride_Disable);
            this.groupBox3.Controls.Add(this.m_rbPDOverride_Enable);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_rbPDOverride_Disable
            // 
            resources.ApplyResources(this.m_rbPDOverride_Disable, "m_rbPDOverride_Disable");
            this.m_rbPDOverride_Disable.Name = "m_rbPDOverride_Disable";
            this.m_rbPDOverride_Disable.UseVisualStyleBackColor = true;
            this.m_rbPDOverride_Disable.CheckedChanged += new System.EventHandler(this.m_rbPDOverride_Disable_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.m_rbPolarity_Inve);
            this.groupBox2.Controls.Add(this.m_rbPolarity_NonInve);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_rbPolarity_Inve
            // 
            resources.ApplyResources(this.m_rbPolarity_Inve, "m_rbPolarity_Inve");
            this.m_rbPolarity_Inve.Name = "m_rbPolarity_Inve";
            this.m_rbPolarity_Inve.UseVisualStyleBackColor = true;
            this.m_rbPolarity_Inve.CheckedChanged += new System.EventHandler(this.m_rbPolarity_Inve_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.m_rbPower_Fast);
            this.groupBox4.Controls.Add(this.m_rbPower_Slow);
            this.groupBox4.Controls.Add(this.m_rbPower_Off);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // m_rbPower_Fast
            // 
            resources.ApplyResources(this.m_rbPower_Fast, "m_rbPower_Fast");
            this.m_rbPower_Fast.Name = "m_rbPower_Fast";
            this.m_rbPower_Fast.UseVisualStyleBackColor = true;
            this.m_rbPower_Fast.CheckedChanged += new System.EventHandler(this.m_rbPower_Fast_CheckedChanged);
            // 
            // m_rbPower_Slow
            // 
            resources.ApplyResources(this.m_rbPower_Slow, "m_rbPower_Slow");
            this.m_rbPower_Slow.Name = "m_rbPower_Slow";
            this.m_rbPower_Slow.UseVisualStyleBackColor = true;
            this.m_rbPower_Slow.CheckedChanged += new System.EventHandler(this.m_rbPower_Slow_CheckedChanged);
            // 
            // m_rbPower_Off
            // 
            resources.ApplyResources(this.m_rbPower_Off, "m_rbPower_Off");
            this.m_rbPower_Off.Name = "m_rbPower_Off";
            this.m_rbPower_Off.UseVisualStyleBackColor = true;
            this.m_rbPower_Off.CheckedChanged += new System.EventHandler(this.m_rbPower_Off_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.m_rbSync_Bypass);
            this.groupBox5.Controls.Add(this.m_rbSync_Norm);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // m_rbSync_Bypass
            // 
            resources.ApplyResources(this.m_rbSync_Bypass, "m_rbSync_Bypass");
            this.m_rbSync_Bypass.Name = "m_rbSync_Bypass";
            this.m_rbSync_Bypass.UseVisualStyleBackColor = true;
            this.m_rbSync_Bypass.CheckedChanged += new System.EventHandler(this.m_rbSync_Bypass_CheckedChanged);
            // 
            // m_rbSync_Norm
            // 
            resources.ApplyResources(this.m_rbSync_Norm, "m_rbSync_Norm");
            this.m_rbSync_Norm.Name = "m_rbSync_Norm";
            this.m_rbSync_Norm.UseVisualStyleBackColor = true;
            this.m_rbSync_Norm.CheckedChanged += new System.EventHandler(this.m_rbSync_Norm_CheckedChanged);
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // CyComparatorControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyComparatorControl";
            this.Load += new System.EventHandler(this.CyComparatorControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider m_errorProvider;
        private System.Windows.Forms.RadioButton m_rbPolarity_NonInve;
        private System.Windows.Forms.RadioButton m_rbPDOverride_Enable;
        private System.Windows.Forms.RadioButton m_rbHysteresis_Disable;
        private System.Windows.Forms.RadioButton m_rbHysteresis_Enable;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton m_rbPolarity_Inve;
        private System.Windows.Forms.RadioButton m_rbPDOverride_Disable;
        private System.Windows.Forms.RadioButton m_rbSync_Norm;
        private System.Windows.Forms.RadioButton m_rbSync_Bypass;
        private System.Windows.Forms.RadioButton m_rbPower_Off;
        private System.Windows.Forms.RadioButton m_rbPower_Slow;
        private System.Windows.Forms.RadioButton m_rbPower_Fast;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}


//[] END OF FILE