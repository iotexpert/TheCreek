/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

namespace Bootloadable_v1_0
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
            this.errProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.labelAppVersion = new System.Windows.Forms.Label();
            this.labelAppID = new System.Windows.Forms.Label();
            this.labelAppCustomID = new System.Windows.Forms.Label();
            this.textBoxAppVersion = new System.Windows.Forms.TextBox();
            this.textBoxAppID = new System.Windows.Forms.TextBox();
            this.textBoxAppCustomID = new System.Windows.Forms.TextBox();
            this.checkBoxAutoImage = new System.Windows.Forms.CheckBox();
            this.labelPlacementAddress = new System.Windows.Forms.Label();
            this.textBoxPlacementAddress = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // errProvider
            // 
            this.errProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errProvider.ContainerControl = this;
            // 
            // labelAppVersion
            // 
            this.labelAppVersion.AutoSize = true;
            this.labelAppVersion.Location = new System.Drawing.Point(3, 15);
            this.labelAppVersion.Name = "labelAppVersion";
            this.labelAppVersion.Size = new System.Drawing.Size(99, 13);
            this.labelAppVersion.TabIndex = 0;
            this.labelAppVersion.Text = "Application version:";
            // 
            // labelAppID
            // 
            this.labelAppID.AutoSize = true;
            this.labelAppID.Location = new System.Drawing.Point(3, 39);
            this.labelAppID.Name = "labelAppID";
            this.labelAppID.Size = new System.Drawing.Size(76, 13);
            this.labelAppID.TabIndex = 1;
            this.labelAppID.Text = "Application ID:";
            // 
            // labelAppCustomID
            // 
            this.labelAppCustomID.AutoSize = true;
            this.labelAppCustomID.Location = new System.Drawing.Point(3, 63);
            this.labelAppCustomID.Name = "labelAppCustomID";
            this.labelAppCustomID.Size = new System.Drawing.Size(113, 13);
            this.labelAppCustomID.TabIndex = 2;
            this.labelAppCustomID.Text = "Application custom ID:";
            // 
            // textBoxAppVersion
            // 
            this.textBoxAppVersion.Location = new System.Drawing.Point(128, 12);
            this.textBoxAppVersion.Name = "textBoxAppVersion";
            this.textBoxAppVersion.Size = new System.Drawing.Size(100, 20);
            this.textBoxAppVersion.TabIndex = 3;
            this.textBoxAppVersion.Validated += new System.EventHandler(this.textBoxHex_Validated);
            // 
            // textBoxAppID
            // 
            this.textBoxAppID.Location = new System.Drawing.Point(128, 36);
            this.textBoxAppID.Name = "textBoxAppID";
            this.textBoxAppID.Size = new System.Drawing.Size(100, 20);
            this.textBoxAppID.TabIndex = 4;
            this.textBoxAppID.Validated += new System.EventHandler(this.textBoxHex_Validated);
            // 
            // textBoxAppCustomID
            // 
            this.textBoxAppCustomID.Location = new System.Drawing.Point(128, 60);
            this.textBoxAppCustomID.Name = "textBoxAppCustomID";
            this.textBoxAppCustomID.Size = new System.Drawing.Size(100, 20);
            this.textBoxAppCustomID.TabIndex = 5;
            this.textBoxAppCustomID.Validated += new System.EventHandler(this.textBoxHex_Validated);
            // 
            // checkBoxAutoImage
            // 
            this.checkBoxAutoImage.AutoSize = true;
            this.checkBoxAutoImage.Checked = true;
            this.checkBoxAutoImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoImage.Location = new System.Drawing.Point(6, 98);
            this.checkBoxAutoImage.Name = "checkBoxAutoImage";
            this.checkBoxAutoImage.Size = new System.Drawing.Size(198, 17);
            this.checkBoxAutoImage.TabIndex = 6;
            this.checkBoxAutoImage.Text = "Manual application image placement";
            this.checkBoxAutoImage.UseVisualStyleBackColor = true;
            this.checkBoxAutoImage.CheckedChanged += new System.EventHandler(this.checkBoxAutoImage_CheckedChanged);
            // 
            // labelPlacementAddress
            // 
            this.labelPlacementAddress.AutoSize = true;
            this.labelPlacementAddress.Location = new System.Drawing.Point(22, 124);
            this.labelPlacementAddress.Name = "labelPlacementAddress";
            this.labelPlacementAddress.Size = new System.Drawing.Size(100, 13);
            this.labelPlacementAddress.TabIndex = 7;
            this.labelPlacementAddress.Text = "Placement address:";
            // 
            // textBoxPlacementAddress
            // 
            this.textBoxPlacementAddress.Location = new System.Drawing.Point(128, 121);
            this.textBoxPlacementAddress.Name = "textBoxPlacementAddress";
            this.textBoxPlacementAddress.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlacementAddress.TabIndex = 8;
            this.textBoxPlacementAddress.Validated += new System.EventHandler(this.textBoxHex_Validated);
            // 
            // CyGeneralPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.textBoxPlacementAddress);
            this.Controls.Add(this.labelPlacementAddress);
            this.Controls.Add(this.checkBoxAutoImage);
            this.Controls.Add(this.textBoxAppCustomID);
            this.Controls.Add(this.textBoxAppID);
            this.Controls.Add(this.textBoxAppVersion);
            this.Controls.Add(this.labelAppCustomID);
            this.Controls.Add(this.labelAppID);
            this.Controls.Add(this.labelAppVersion);
            this.Name = "CyGeneralPage";
            this.Size = new System.Drawing.Size(362, 188);
            ((System.ComponentModel.ISupportInitialize)(this.errProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errProvider;
        private System.Windows.Forms.Label labelAppID;
        private System.Windows.Forms.Label labelAppVersion;
        private System.Windows.Forms.TextBox textBoxAppCustomID;
        private System.Windows.Forms.TextBox textBoxAppID;
        private System.Windows.Forms.TextBox textBoxAppVersion;
        private System.Windows.Forms.Label labelAppCustomID;
        private System.Windows.Forms.TextBox textBoxPlacementAddress;
        private System.Windows.Forms.Label labelPlacementAddress;
        private System.Windows.Forms.CheckBox checkBoxAutoImage;
    }
}
