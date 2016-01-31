/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

namespace SegDisplay_v1_10
{
    partial class CyDriverParams
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyDriverParams));
            this.comboBoxLowDriveMode = new System.Windows.Forms.ComboBox();
            this.numUpDownLowDriveInitTime = new System.Windows.Forms.NumericUpDown();
            this.labelLowDriveInitTime = new System.Windows.Forms.Label();
            this.labelLowDriveMode = new System.Windows.Forms.Label();
            this.labelHiDriverTime = new System.Windows.Forms.Label();
            this.labelLowDriveDutyCycle = new System.Windows.Forms.Label();
            this.comboBoxDriverPowerMode = new System.Windows.Forms.ComboBox();
            this.labelDriverPowerMode = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.labelClockFrequency = new System.Windows.Forms.Label();
            this.editClockFrequency = new System.Windows.Forms.Label();
            this.numUpDownHiDriveTime = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLowDriveInitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownHiDriveTime)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxLowDriveMode
            // 
            this.comboBoxLowDriveMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLowDriveMode.FormattingEnabled = true;
            this.comboBoxLowDriveMode.Items.AddRange(new object[] {
            resources.GetString("comboBoxLowDriveMode.Items"),
            resources.GetString("comboBoxLowDriveMode.Items1")});
            resources.ApplyResources(this.comboBoxLowDriveMode, "comboBoxLowDriveMode");
            this.comboBoxLowDriveMode.Name = "comboBoxLowDriveMode";
            this.comboBoxLowDriveMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxLowDriveMode_SelectedIndexChanged);
            // 
            // numUpDownLowDriveInitTime
            // 
            this.numUpDownLowDriveInitTime.DecimalPlaces = 1;
            resources.ApplyResources(this.numUpDownLowDriveInitTime, "numUpDownLowDriveInitTime");
            this.numUpDownLowDriveInitTime.Name = "numUpDownLowDriveInitTime";
            this.numUpDownLowDriveInitTime.ValueChanged += new System.EventHandler(this.numUpDownLowDriveInitTime_ValueChanged);
            this.numUpDownLowDriveInitTime.Validated += new System.EventHandler(this.UpdateNumUpDownDriveTimeData);
            // 
            // labelLowDriveInitTime
            // 
            resources.ApplyResources(this.labelLowDriveInitTime, "labelLowDriveInitTime");
            this.labelLowDriveInitTime.Name = "labelLowDriveInitTime";
            // 
            // labelLowDriveMode
            // 
            resources.ApplyResources(this.labelLowDriveMode, "labelLowDriveMode");
            this.labelLowDriveMode.Name = "labelLowDriveMode";
            // 
            // labelHiDriverTime
            // 
            resources.ApplyResources(this.labelHiDriverTime, "labelHiDriverTime");
            this.labelHiDriverTime.Name = "labelHiDriverTime";
            // 
            // labelLowDriveDutyCycle
            // 
            resources.ApplyResources(this.labelLowDriveDutyCycle, "labelLowDriveDutyCycle");
            this.labelLowDriveDutyCycle.Name = "labelLowDriveDutyCycle";
            // 
            // comboBoxDriverPowerMode
            // 
            this.comboBoxDriverPowerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDriverPowerMode.FormattingEnabled = true;
            this.comboBoxDriverPowerMode.Items.AddRange(new object[] {
            resources.GetString("comboBoxDriverPowerMode.Items"),
            resources.GetString("comboBoxDriverPowerMode.Items1")});
            resources.ApplyResources(this.comboBoxDriverPowerMode, "comboBoxDriverPowerMode");
            this.comboBoxDriverPowerMode.Name = "comboBoxDriverPowerMode";
            this.comboBoxDriverPowerMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriverPowerMode_SelectedIndexChanged);
            // 
            // labelDriverPowerMode
            // 
            resources.ApplyResources(this.labelDriverPowerMode, "labelDriverPowerMode");
            this.labelDriverPowerMode.Name = "labelDriverPowerMode";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // labelClockFrequency
            // 
            resources.ApplyResources(this.labelClockFrequency, "labelClockFrequency");
            this.labelClockFrequency.Name = "labelClockFrequency";
            // 
            // editClockFrequency
            // 
            this.editClockFrequency.BackColor = System.Drawing.SystemColors.Window;
            this.editClockFrequency.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.editClockFrequency, "editClockFrequency");
            this.editClockFrequency.Name = "editClockFrequency";
            // 
            // numUpDownHiDriveTime
            // 
            this.numUpDownHiDriveTime.DecimalPlaces = 1;
            this.numUpDownHiDriveTime.Increment = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            resources.ApplyResources(this.numUpDownHiDriveTime, "numUpDownHiDriveTime");
            this.numUpDownHiDriveTime.Name = "numUpDownHiDriveTime";
            this.numUpDownHiDriveTime.ValueChanged += new System.EventHandler(this.numUpDownHiDriveTime_ValueChanged);
            this.numUpDownHiDriveTime.Validated += new System.EventHandler(this.UpdateNumUpDownDriveTimeData);
            // 
            // CyDriverParams
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.editClockFrequency);
            this.Controls.Add(this.labelClockFrequency);
            this.Controls.Add(this.numUpDownHiDriveTime);
            this.Controls.Add(this.comboBoxLowDriveMode);
            this.Controls.Add(this.numUpDownLowDriveInitTime);
            this.Controls.Add(this.labelLowDriveInitTime);
            this.Controls.Add(this.labelLowDriveMode);
            this.Controls.Add(this.labelHiDriverTime);
            this.Controls.Add(this.labelLowDriveDutyCycle);
            this.Controls.Add(this.comboBoxDriverPowerMode);
            this.Controls.Add(this.labelDriverPowerMode);
            this.Name = "CyDriverParams";
            this.VisibleChanged += new System.EventHandler(this.CyDriverParams_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLowDriveInitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownHiDriveTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxLowDriveMode;
        private System.Windows.Forms.NumericUpDown numUpDownLowDriveInitTime;
        private System.Windows.Forms.Label labelLowDriveInitTime;
        private System.Windows.Forms.Label labelLowDriveMode;
        private System.Windows.Forms.Label labelHiDriverTime;
        private System.Windows.Forms.Label labelLowDriveDutyCycle;
        private System.Windows.Forms.ComboBox comboBoxDriverPowerMode;
        private System.Windows.Forms.Label labelDriverPowerMode;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label labelClockFrequency;
        private System.Windows.Forms.Label editClockFrequency;
        private System.Windows.Forms.NumericUpDown numUpDownHiDriveTime;
    }
}
