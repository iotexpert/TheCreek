/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

namespace ResistiveTouch_v1_0
{
    partial class CyGeneralPage
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
            this.gbADC = new System.Windows.Forms.GroupBox();
            this.labelADCDesc2 = new System.Windows.Forms.Label();
            this.labelADCDesc1 = new System.Windows.Forms.Label();
            this.rbSAR = new System.Windows.Forms.RadioButton();
            this.rbDeltaSigma = new System.Windows.Forms.RadioButton();
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbADC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // gbADC
            // 
            this.gbADC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbADC.Controls.Add(this.labelADCDesc2);
            this.gbADC.Controls.Add(this.labelADCDesc1);
            this.gbADC.Controls.Add(this.rbSAR);
            this.gbADC.Controls.Add(this.rbDeltaSigma);
            this.gbADC.Location = new System.Drawing.Point(3, 3);
            this.gbADC.Name = "gbADC";
            this.gbADC.Size = new System.Drawing.Size(463, 75);
            this.gbADC.TabIndex = 0;
            this.gbADC.TabStop = false;
            this.gbADC.Text = "ADC";
            // 
            // labelADCDesc2
            // 
            this.labelADCDesc2.AutoSize = true;
            this.labelADCDesc2.Location = new System.Drawing.Point(128, 44);
            this.labelADCDesc2.Name = "labelADCDesc2";
            this.labelADCDesc2.Size = new System.Drawing.Size(310, 13);
            this.labelADCDesc2.TabIndex = 3;
            this.labelADCDesc2.Text = "Notice that the SAR ADC cannot be selected for PSoC3 device.";
            // 
            // labelADCDesc1
            // 
            this.labelADCDesc1.AutoSize = true;
            this.labelADCDesc1.Location = new System.Drawing.Point(128, 21);
            this.labelADCDesc1.Name = "labelADCDesc1";
            this.labelADCDesc1.Size = new System.Drawing.Size(326, 13);
            this.labelADCDesc1.TabIndex = 2;
            this.labelADCDesc1.Text = "Specifies whether the Delta Sigma ADC or SAR ADC will be utilized.";
            // 
            // rbSAR
            // 
            this.rbSAR.AutoSize = true;
            this.rbSAR.Location = new System.Drawing.Point(6, 42);
            this.rbSAR.Name = "rbSAR";
            this.rbSAR.Size = new System.Drawing.Size(72, 17);
            this.rbSAR.TabIndex = 1;
            this.rbSAR.Text = "SAR ADC";
            this.rbSAR.UseVisualStyleBackColor = true;
            this.rbSAR.CheckedChanged += new System.EventHandler(this.rbADC_CheckedChanged);
            // 
            // rbDeltaSigma
            // 
            this.rbDeltaSigma.AutoSize = true;
            this.rbDeltaSigma.Checked = true;
            this.rbDeltaSigma.Location = new System.Drawing.Point(6, 19);
            this.rbDeltaSigma.Name = "rbDeltaSigma";
            this.rbDeltaSigma.Size = new System.Drawing.Size(107, 17);
            this.rbDeltaSigma.TabIndex = 0;
            this.rbDeltaSigma.TabStop = true;
            this.rbDeltaSigma.Text = "Delta Sigma ADC";
            this.rbDeltaSigma.UseVisualStyleBackColor = true;
            this.rbDeltaSigma.CheckedChanged += new System.EventHandler(this.rbADC_CheckedChanged);
            // 
            // errProvider
            // 
            this.errProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errProvider.ContainerControl = this;
            // 
            // CyGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.gbADC);
            this.Name = "CyGeneralPage";
            this.Size = new System.Drawing.Size(469, 150);
            this.gbADC.ResumeLayout(false);
            this.gbADC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbADC;
        private System.Windows.Forms.RadioButton rbDeltaSigma;
        private System.Windows.Forms.RadioButton rbSAR;
        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.Label labelADCDesc1;
        private System.Windows.Forms.Label labelADCDesc2;
    }
}
