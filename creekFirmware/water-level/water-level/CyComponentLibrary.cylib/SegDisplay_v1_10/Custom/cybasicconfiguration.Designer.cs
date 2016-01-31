/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

namespace SegDisplay_v1_10
{
    partial class CyBasicConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CyBasicConfiguration));
            this.comboBoxBiasVoltage = new System.Windows.Forms.ComboBox();
            this.comboBoxFrameRate = new System.Windows.Forms.ComboBox();
            this.comboBoxWaveformType = new System.Windows.Forms.ComboBox();
            this.labelBiasVoltage = new System.Windows.Forms.Label();
            this.labelFrameRate = new System.Windows.Forms.Label();
            this.labelWaveformType = new System.Windows.Forms.Label();
            this.labelBiasType = new System.Windows.Forms.Label();
            this.numUpDownNumSegmentLines = new System.Windows.Forms.NumericUpDown();
            this.labelNumSegmentLines = new System.Windows.Forms.Label();
            this.labelNumCommonLines = new System.Windows.Forms.Label();
            this.numUpDownNumCommonLines = new System.Windows.Forms.NumericUpDown();
            this.checkBoxGang = new System.Windows.Forms.CheckBox();
            this.checkBoxDebugMode = new System.Windows.Forms.CheckBox();
            this.comboBoxBiasType = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumSegmentLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumCommonLines)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxBiasVoltage
            // 
            this.comboBoxBiasVoltage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBiasVoltage.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxBiasVoltage, "comboBoxBiasVoltage");
            this.comboBoxBiasVoltage.Name = "comboBoxBiasVoltage";
            this.comboBoxBiasVoltage.SelectedIndexChanged += new System.EventHandler(this.comboBoxBiasVoltage_SelectedIndexChanged);
            // 
            // comboBoxFrameRate
            // 
            this.comboBoxFrameRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrameRate.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxFrameRate, "comboBoxFrameRate");
            this.comboBoxFrameRate.Name = "comboBoxFrameRate";
            this.comboBoxFrameRate.SelectedIndexChanged += new System.EventHandler(this.comboBoxFrameRate_SelectedIndexChanged);
            // 
            // comboBoxWaveformType
            // 
            this.comboBoxWaveformType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWaveformType.FormattingEnabled = true;
            this.comboBoxWaveformType.Items.AddRange(new object[] {
            resources.GetString("comboBoxWaveformType.Items"),
            resources.GetString("comboBoxWaveformType.Items1")});
            resources.ApplyResources(this.comboBoxWaveformType, "comboBoxWaveformType");
            this.comboBoxWaveformType.Name = "comboBoxWaveformType";
            this.comboBoxWaveformType.SelectedIndexChanged += new System.EventHandler(this.comboBoxWaveformType_SelectedIndexChanged);
            // 
            // labelBiasVoltage
            // 
            resources.ApplyResources(this.labelBiasVoltage, "labelBiasVoltage");
            this.labelBiasVoltage.Name = "labelBiasVoltage";
            // 
            // labelFrameRate
            // 
            resources.ApplyResources(this.labelFrameRate, "labelFrameRate");
            this.labelFrameRate.Name = "labelFrameRate";
            // 
            // labelWaveformType
            // 
            resources.ApplyResources(this.labelWaveformType, "labelWaveformType");
            this.labelWaveformType.Name = "labelWaveformType";
            // 
            // labelBiasType
            // 
            resources.ApplyResources(this.labelBiasType, "labelBiasType");
            this.labelBiasType.Name = "labelBiasType";
            // 
            // numUpDownNumSegmentLines
            // 
            resources.ApplyResources(this.numUpDownNumSegmentLines, "numUpDownNumSegmentLines");
            this.numUpDownNumSegmentLines.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.Name = "numUpDownNumSegmentLines";
            this.numUpDownNumSegmentLines.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numUpDownNumSegmentLines.ValueChanged += new System.EventHandler(this.numUpDownNumLines_ValueChanged);
            // 
            // labelNumSegmentLines
            // 
            resources.ApplyResources(this.labelNumSegmentLines, "labelNumSegmentLines");
            this.labelNumSegmentLines.Name = "labelNumSegmentLines";
            // 
            // labelNumCommonLines
            // 
            resources.ApplyResources(this.labelNumCommonLines, "labelNumCommonLines");
            this.labelNumCommonLines.Name = "labelNumCommonLines";
            // 
            // numUpDownNumCommonLines
            // 
            resources.ApplyResources(this.numUpDownNumCommonLines, "numUpDownNumCommonLines");
            this.numUpDownNumCommonLines.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numUpDownNumCommonLines.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numUpDownNumCommonLines.Name = "numUpDownNumCommonLines";
            this.numUpDownNumCommonLines.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numUpDownNumCommonLines.ValueChanged += new System.EventHandler(this.numUpDownNumLines_ValueChanged);
            // 
            // checkBoxGang
            // 
            resources.ApplyResources(this.checkBoxGang, "checkBoxGang");
            this.checkBoxGang.Name = "checkBoxGang";
            this.checkBoxGang.UseVisualStyleBackColor = true;
            this.checkBoxGang.CheckedChanged += new System.EventHandler(this.checkBoxGang_CheckedChanged);
            // 
            // checkBoxDebugMode
            // 
            resources.ApplyResources(this.checkBoxDebugMode, "checkBoxDebugMode");
            this.checkBoxDebugMode.Name = "checkBoxDebugMode";
            this.checkBoxDebugMode.UseVisualStyleBackColor = true;
            this.checkBoxDebugMode.CheckedChanged += new System.EventHandler(this.checkBoxDebugMode_CheckedChanged);
            // 
            // comboBoxBiasType
            // 
            this.comboBoxBiasType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBiasType.FormattingEnabled = true;
            this.comboBoxBiasType.Items.AddRange(new object[] {
            resources.GetString("comboBoxBiasType.Items"),
            resources.GetString("comboBoxBiasType.Items1"),
            resources.GetString("comboBoxBiasType.Items2")});
            resources.ApplyResources(this.comboBoxBiasType, "comboBoxBiasType");
            this.comboBoxBiasType.Name = "comboBoxBiasType";
            this.comboBoxBiasType.SelectedIndexChanged += new System.EventHandler(this.comboBoxBiasType_SelectedIndexChanged);
            // 
            // toolTip
            // 
            this.toolTip.Active = false;
            this.toolTip.AutomaticDelay = 100;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.ReshowDelay = 20;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTip.ToolTipTitle = "Warning";
            // 
            // CyBasicConfiguration
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxBiasType);
            this.Controls.Add(this.checkBoxDebugMode);
            this.Controls.Add(this.checkBoxGang);
            this.Controls.Add(this.comboBoxBiasVoltage);
            this.Controls.Add(this.comboBoxFrameRate);
            this.Controls.Add(this.comboBoxWaveformType);
            this.Controls.Add(this.labelBiasVoltage);
            this.Controls.Add(this.labelFrameRate);
            this.Controls.Add(this.labelWaveformType);
            this.Controls.Add(this.labelBiasType);
            this.Controls.Add(this.numUpDownNumSegmentLines);
            this.Controls.Add(this.labelNumSegmentLines);
            this.Controls.Add(this.labelNumCommonLines);
            this.Controls.Add(this.numUpDownNumCommonLines);
            this.Name = "CyBasicConfiguration";
            this.VisibleChanged += new System.EventHandler(this.CyBasicConfiguration_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumSegmentLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNumCommonLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxBiasVoltage;
        private System.Windows.Forms.ComboBox comboBoxFrameRate;
        private System.Windows.Forms.ComboBox comboBoxWaveformType;
        private System.Windows.Forms.Label labelBiasVoltage;
        private System.Windows.Forms.Label labelFrameRate;
        private System.Windows.Forms.Label labelWaveformType;
        private System.Windows.Forms.Label labelBiasType;
        private System.Windows.Forms.NumericUpDown numUpDownNumSegmentLines;
        private System.Windows.Forms.Label labelNumSegmentLines;
        private System.Windows.Forms.Label labelNumCommonLines;
        private System.Windows.Forms.NumericUpDown numUpDownNumCommonLines;
        private System.Windows.Forms.CheckBox checkBoxGang;
        private System.Windows.Forms.CheckBox checkBoxDebugMode;
        private System.Windows.Forms.ComboBox comboBoxBiasType;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
