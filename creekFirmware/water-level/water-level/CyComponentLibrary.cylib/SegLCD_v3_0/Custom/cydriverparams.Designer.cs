/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


namespace SegLCD_v3_0
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
            this.numUpDownLowDriveInitTime = new System.Windows.Forms.NumericUpDown();
            this.labelLowDriveInitTime = new System.Windows.Forms.Label();
            this.labelHighDriverTime = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.numUpDownHiDriveTime = new System.Windows.Forms.NumericUpDown();
            this.numUpDownGlassSize = new System.Windows.Forms.NumericUpDown();
            this.labelGlassSize = new System.Windows.Forms.Label();
            this.groupBoxAdvanced = new System.Windows.Forms.GroupBox();
            this.comboBoxLowDriveStrength = new System.Windows.Forms.ComboBox();
            this.labelLowDriveStrength = new System.Windows.Forms.Label();
            this.comboBoxHiDriveStrength = new System.Windows.Forms.ComboBox();
            this.labelHiDriveStrength = new System.Windows.Forms.Label();
            this.checkBoxAdvanced = new System.Windows.Forms.CheckBox();
            this.labelDefaultPeriod = new System.Windows.Forms.Label();
            this.numUpDownCustomPeriod = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonCustomStep = new System.Windows.Forms.RadioButton();
            this.radioButtonDefStep = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLowDriveInitTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownHiDriveTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownGlassSize)).BeginInit();
            this.groupBoxAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownCustomPeriod)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numUpDownLowDriveInitTime
            // 
            this.numUpDownLowDriveInitTime.DecimalPlaces = 2;
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
            // labelHighDriverTime
            // 
            resources.ApplyResources(this.labelHighDriverTime, "labelHighDriverTime");
            this.labelHighDriverTime.Name = "labelHighDriverTime";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // numUpDownHiDriveTime
            // 
            this.numUpDownHiDriveTime.DecimalPlaces = 2;
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
            // numUpDownGlassSize
            // 
            resources.ApplyResources(this.numUpDownGlassSize, "numUpDownGlassSize");
            this.numUpDownGlassSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numUpDownGlassSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownGlassSize.Name = "numUpDownGlassSize";
            this.numUpDownGlassSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDownGlassSize.ValueChanged += new System.EventHandler(this.numUpDownGlassSize_ValueChanged);
            // 
            // labelGlassSize
            // 
            resources.ApplyResources(this.labelGlassSize, "labelGlassSize");
            this.labelGlassSize.Name = "labelGlassSize";
            // 
            // groupBoxAdvanced
            // 
            this.groupBoxAdvanced.Controls.Add(this.groupBox1);
            this.groupBoxAdvanced.Controls.Add(this.comboBoxLowDriveStrength);
            this.groupBoxAdvanced.Controls.Add(this.labelLowDriveStrength);
            this.groupBoxAdvanced.Controls.Add(this.comboBoxHiDriveStrength);
            this.groupBoxAdvanced.Controls.Add(this.labelHiDriveStrength);
            this.groupBoxAdvanced.Controls.Add(this.numUpDownLowDriveInitTime);
            this.groupBoxAdvanced.Controls.Add(this.labelLowDriveInitTime);
            resources.ApplyResources(this.groupBoxAdvanced, "groupBoxAdvanced");
            this.groupBoxAdvanced.Name = "groupBoxAdvanced";
            this.groupBoxAdvanced.TabStop = false;
            // 
            // comboBoxLowDriveStrength
            // 
            this.comboBoxLowDriveStrength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboBoxLowDriveStrength, "comboBoxLowDriveStrength");
            this.comboBoxLowDriveStrength.FormattingEnabled = true;
            this.comboBoxLowDriveStrength.Items.AddRange(new object[] {
            resources.GetString("comboBoxLowDriveStrength.Items"),
            resources.GetString("comboBoxLowDriveStrength.Items1")});
            this.comboBoxLowDriveStrength.Name = "comboBoxLowDriveStrength";
            this.comboBoxLowDriveStrength.SelectedIndexChanged += new System.EventHandler(this.comboBoxLowDriveStrength_SelectedIndexChanged);
            // 
            // labelLowDriveStrength
            // 
            resources.ApplyResources(this.labelLowDriveStrength, "labelLowDriveStrength");
            this.labelLowDriveStrength.Name = "labelLowDriveStrength";
            // 
            // comboBoxHiDriveStrength
            // 
            this.comboBoxHiDriveStrength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHiDriveStrength.FormattingEnabled = true;
            this.comboBoxHiDriveStrength.Items.AddRange(new object[] {
            resources.GetString("comboBoxHiDriveStrength.Items"),
            resources.GetString("comboBoxHiDriveStrength.Items1"),
            resources.GetString("comboBoxHiDriveStrength.Items2"),
            resources.GetString("comboBoxHiDriveStrength.Items3"),
            resources.GetString("comboBoxHiDriveStrength.Items4"),
            resources.GetString("comboBoxHiDriveStrength.Items5")});
            resources.ApplyResources(this.comboBoxHiDriveStrength, "comboBoxHiDriveStrength");
            this.comboBoxHiDriveStrength.Name = "comboBoxHiDriveStrength";
            this.comboBoxHiDriveStrength.SelectedIndexChanged += new System.EventHandler(this.comboBoxHiDriveStrength_SelectedIndexChanged);
            // 
            // labelHiDriveStrength
            // 
            resources.ApplyResources(this.labelHiDriveStrength, "labelHiDriveStrength");
            this.labelHiDriveStrength.Name = "labelHiDriveStrength";
            // 
            // checkBoxAdvanced
            // 
            resources.ApplyResources(this.checkBoxAdvanced, "checkBoxAdvanced");
            this.checkBoxAdvanced.Name = "checkBoxAdvanced";
            this.checkBoxAdvanced.UseVisualStyleBackColor = true;
            this.checkBoxAdvanced.CheckedChanged += new System.EventHandler(this.checkBoxAdvanced_CheckedChanged);
            // 
            // labelDefaultPeriod
            // 
            this.labelDefaultPeriod.BackColor = System.Drawing.SystemColors.Window;
            this.labelDefaultPeriod.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.labelDefaultPeriod, "labelDefaultPeriod");
            this.labelDefaultPeriod.Name = "labelDefaultPeriod";
            // 
            // numUpDownCustomPeriod
            // 
            this.numUpDownCustomPeriod.DecimalPlaces = 2;
            resources.ApplyResources(this.numUpDownCustomPeriod, "numUpDownCustomPeriod");
            this.numUpDownCustomPeriod.Name = "numUpDownCustomPeriod";
            this.numUpDownCustomPeriod.ValueChanged += new System.EventHandler(this.numUpDownCustomPeriod_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDefStep);
            this.groupBox1.Controls.Add(this.radioButtonCustomStep);
            this.groupBox1.Controls.Add(this.numUpDownCustomPeriod);
            this.groupBox1.Controls.Add(this.labelDefaultPeriod);
            this.groupBox1.Controls.Add(this.numUpDownHiDriveTime);
            this.groupBox1.Controls.Add(this.labelHighDriverTime);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtonCustomStep
            // 
            resources.ApplyResources(this.radioButtonCustomStep, "radioButtonCustomStep");
            this.radioButtonCustomStep.Name = "radioButtonCustomStep";
            this.radioButtonCustomStep.UseVisualStyleBackColor = true;
            this.radioButtonCustomStep.CheckedChanged += new System.EventHandler(this.radioButtonCustomStep_CheckedChanged);
            // 
            // radioButtonDefStep
            // 
            resources.ApplyResources(this.radioButtonDefStep, "radioButtonDefStep");
            this.radioButtonDefStep.Checked = true;
            this.radioButtonDefStep.Name = "radioButtonDefStep";
            this.radioButtonDefStep.TabStop = true;
            this.radioButtonDefStep.UseVisualStyleBackColor = true;
            // 
            // CyDriverParams
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxAdvanced);
            this.Controls.Add(this.groupBoxAdvanced);
            this.Controls.Add(this.labelGlassSize);
            this.Controls.Add(this.numUpDownGlassSize);
            this.Name = "CyDriverParams";
            this.VisibleChanged += new System.EventHandler(this.CyDriverParams_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownLowDriveInitTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownHiDriveTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownGlassSize)).EndInit();
            this.groupBoxAdvanced.ResumeLayout(false);
            this.groupBoxAdvanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownCustomPeriod)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numUpDownLowDriveInitTime;
        private System.Windows.Forms.Label labelLowDriveInitTime;
        private System.Windows.Forms.Label labelHighDriverTime;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.NumericUpDown numUpDownHiDriveTime;
        private System.Windows.Forms.Label labelGlassSize;
        private System.Windows.Forms.NumericUpDown numUpDownGlassSize;
        private System.Windows.Forms.CheckBox checkBoxAdvanced;
        private System.Windows.Forms.GroupBox groupBoxAdvanced;
        private System.Windows.Forms.ComboBox comboBoxHiDriveStrength;
        private System.Windows.Forms.Label labelHiDriveStrength;
        private System.Windows.Forms.ComboBox comboBoxLowDriveStrength;
        private System.Windows.Forms.Label labelLowDriveStrength;
        private System.Windows.Forms.Label labelDefaultPeriod;
        private System.Windows.Forms.NumericUpDown numUpDownCustomPeriod;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDefStep;
        private System.Windows.Forms.RadioButton radioButtonCustomStep;
    }
}
