/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CapSense_CSD_v2_0
{
    public partial class CyAdvancedTab : CyCSParamEditTemplate
    {
        #region Windows Form Designer generated code
        #region Windows Form Designer generated code
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
            this.components = new System.ComponentModel.Container();
            this.cbPrescaler = new System.Windows.Forms.ComboBox();
            this.Labe1 = new System.Windows.Forms.Label();
            this.lBleedResistors_CH0 = new System.Windows.Forms.Label();
            this.cbBleedResistors_CH0 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCurrentSource = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPRS_EMIReduction = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSensorAutoReset = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbWidgetResolution = new System.Windows.Forms.ComboBox();
            this.gbVref = new System.Windows.Forms.GroupBox();
            this.lVdacVal = new System.Windows.Forms.Label();
            this.tbVdacValue = new System.Windows.Forms.TextBox();
            this.rbVdac = new System.Windows.Forms.RadioButton();
            this.rbVref1024 = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.cbCIS = new System.Windows.Forms.ComboBox();
            this.pbClk = new System.Windows.Forms.PictureBox();
            this.cbBleedResistors_CH1 = new System.Windows.Forms.ComboBox();
            this.lBleedResistors_CH1 = new System.Windows.Forms.Label();
            this.lShield = new System.Windows.Forms.Label();
            this.cbShield = new System.Windows.Forms.ComboBox();
            this.cbIDACrange = new System.Windows.Forms.ComboBox();
            this.lIdacR = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbGuardSensor = new System.Windows.Forms.ComboBox();
            this.lDRICh0 = new System.Windows.Forms.Label();
            this.cbDigitalResourceImplementationCh0 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbScanSpeed = new System.Windows.Forms.ComboBox();
            this.tbAnalogSwitchDivider = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cbDigitalResourceImplementationCh1 = new System.Windows.Forms.ComboBox();
            this.lDRICh1 = new System.Windows.Forms.Label();
            this.gbImplementation = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pbHardwar0 = new System.Windows.Forms.PictureBox();
            this.pbHardwar1 = new System.Windows.Forms.PictureBox();
            this.lChannel0 = new System.Windows.Forms.Label();
            this.lChannel1 = new System.Windows.Forms.Label();
            this.gbVref.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.gbImplementation.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHardwar0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHardwar1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbPrescaler
            // 
            this.cbPrescaler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrescaler.FormattingEnabled = true;
            this.cbPrescaler.Items.AddRange(new object[] {
            "Direct",
            "UDB Timer",
            "Fixed function Timer"});
            this.cbPrescaler.Location = new System.Drawing.Point(240, 19);
            this.cbPrescaler.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbPrescaler.Name = "cbPrescaler";
            this.cbPrescaler.Size = new System.Drawing.Size(85, 21);
            this.cbPrescaler.TabIndex = 4;
            // 
            // Labe1
            // 
            this.Labe1.AutoSize = true;
            this.Labe1.Location = new System.Drawing.Point(6, 20);
            this.Labe1.Name = "Labe1";
            this.Labe1.Size = new System.Drawing.Size(143, 13);
            this.Labe1.TabIndex = 3;
            this.Labe1.Text = "Analog Switch Drive Source ";
            // 
            // lBleedResistors_CH0
            // 
            this.lBleedResistors_CH0.AutoSize = true;
            this.lBleedResistors_CH0.Location = new System.Drawing.Point(6, 73);
            this.lBleedResistors_CH0.Name = "lBleedResistors_CH0";
            this.lBleedResistors_CH0.Size = new System.Drawing.Size(132, 13);
            this.lBleedResistors_CH0.TabIndex = 3;
            this.lBleedResistors_CH0.Text = "Number of Bleed Resistors";
            // 
            // cbBleedResistors_CH0
            // 
            this.cbBleedResistors_CH0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBleedResistors_CH0.FormattingEnabled = true;
            this.cbBleedResistors_CH0.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbBleedResistors_CH0.Location = new System.Drawing.Point(214, 70);
            this.cbBleedResistors_CH0.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbBleedResistors_CH0.Name = "cbBleedResistors_CH0";
            this.cbBleedResistors_CH0.Size = new System.Drawing.Size(110, 21);
            this.cbBleedResistors_CH0.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Current Source ";
            // 
            // cbCurrentSource
            // 
            this.cbCurrentSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentSource.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.cbCurrentSource, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.cbCurrentSource.Items.AddRange(new object[] {
            "IDAC sourcing",
            "IDAC sinking",
            "External resistor"});
            this.cbCurrentSource.Location = new System.Drawing.Point(214, 18);
            this.cbCurrentSource.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbCurrentSource.Name = "cbCurrentSource";
            this.cbCurrentSource.Size = new System.Drawing.Size(110, 21);
            this.cbCurrentSource.TabIndex = 4;
            this.cbCurrentSource.Validating += new System.ComponentModel.CancelEventHandler(this.cbCurrentSource_Validating);
            this.cbCurrentSource.SelectedIndexChanged += new System.EventHandler(this.cbCurrentSource_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "PRS EMI Reduction ";
            // 
            // cbPRS_EMIReduction
            // 
            this.cbPRS_EMIReduction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPRS_EMIReduction.FormattingEnabled = true;
            this.cbPRS_EMIReduction.Items.AddRange(new object[] {
            "Enabled 16-bit",
            "Enabled 8-bit",
            "Disabled"});
            this.cbPRS_EMIReduction.Location = new System.Drawing.Point(126, 46);
            this.cbPRS_EMIReduction.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbPRS_EMIReduction.Name = "cbPRS_EMIReduction";
            this.cbPRS_EMIReduction.Size = new System.Drawing.Size(117, 21);
            this.cbPRS_EMIReduction.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Sensor Auto Reset ";
            // 
            // cbSensorAutoReset
            // 
            this.cbSensorAutoReset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSensorAutoReset.FormattingEnabled = true;
            this.cbSensorAutoReset.Items.AddRange(new object[] {
            "Disabled",
            "Enabled"});
            this.cbSensorAutoReset.Location = new System.Drawing.Point(126, 17);
            this.cbSensorAutoReset.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbSensorAutoReset.Name = "cbSensorAutoReset";
            this.cbSensorAutoReset.Size = new System.Drawing.Size(117, 21);
            this.cbSensorAutoReset.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Widget Resolution";
            // 
            // cbWidgetResolution
            // 
            this.cbWidgetResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWidgetResolution.FormattingEnabled = true;
            this.cbWidgetResolution.Items.AddRange(new object[] {
            "8-bit",
            "16-bit"});
            this.cbWidgetResolution.Location = new System.Drawing.Point(126, 44);
            this.cbWidgetResolution.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbWidgetResolution.Name = "cbWidgetResolution";
            this.cbWidgetResolution.Size = new System.Drawing.Size(117, 21);
            this.cbWidgetResolution.TabIndex = 4;
            // 
            // gbVref
            // 
            this.gbVref.Controls.Add(this.lVdacVal);
            this.gbVref.Controls.Add(this.tbVdacValue);
            this.gbVref.Controls.Add(this.rbVdac);
            this.gbVref.Controls.Add(this.rbVref1024);
            this.gbVref.Location = new System.Drawing.Point(340, 106);
            this.gbVref.Margin = new System.Windows.Forms.Padding(2);
            this.gbVref.MinimumSize = new System.Drawing.Size(195, 44);
            this.gbVref.Name = "gbVref";
            this.gbVref.Padding = new System.Windows.Forms.Padding(2);
            this.gbVref.Size = new System.Drawing.Size(255, 59);
            this.gbVref.TabIndex = 51;
            this.gbVref.TabStop = false;
            this.gbVref.Text = "Voltage reference source";
            // 
            // lVdacVal
            // 
            this.lVdacVal.AutoSize = true;
            this.lVdacVal.Location = new System.Drawing.Point(199, 36);
            this.lVdacVal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lVdacVal.Name = "lVdacVal";
            this.lVdacVal.Size = new System.Drawing.Size(35, 13);
            this.lVdacVal.TabIndex = 51;
            this.lVdacVal.Text = "label7";
            // 
            // tbVdacValue
            // 
            this.tbVdacValue.Location = new System.Drawing.Point(102, 33);
            this.tbVdacValue.Margin = new System.Windows.Forms.Padding(2);
            this.tbVdacValue.Name = "tbVdacValue";
            this.tbVdacValue.Size = new System.Drawing.Size(93, 20);
            this.tbVdacValue.TabIndex = 50;
            this.tbVdacValue.TextChanged += new System.EventHandler(this.tbVdacValue_TextChanged);
            this.tbVdacValue.Validating += new System.ComponentModel.CancelEventHandler(this.tbVdacValue_Validating);
            // 
            // rbVdac
            // 
            this.rbVdac.AutoSize = true;
            this.rbVdac.Location = new System.Drawing.Point(7, 34);
            this.rbVdac.Margin = new System.Windows.Forms.Padding(2);
            this.rbVdac.Name = "rbVdac";
            this.rbVdac.Size = new System.Drawing.Size(50, 17);
            this.rbVdac.TabIndex = 49;
            this.rbVdac.Text = "Vdac";
            this.rbVdac.UseVisualStyleBackColor = true;
            this.rbVdac.CheckedChanged += new System.EventHandler(this.rbVref_CheckedChanged);
            // 
            // rbVref1024
            // 
            this.rbVref1024.AutoSize = true;
            this.rbVref1024.Checked = true;
            this.rbVref1024.Location = new System.Drawing.Point(7, 17);
            this.rbVref1024.Margin = new System.Windows.Forms.Padding(2);
            this.rbVref1024.Name = "rbVref1024";
            this.rbVref1024.Size = new System.Drawing.Size(81, 17);
            this.rbVref1024.TabIndex = 48;
            this.rbVref1024.TabStop = true;
            this.rbVref1024.Text = "Vref 1.024V";
            this.rbVref1024.UseVisualStyleBackColor = true;
            this.rbVref1024.CheckedChanged += new System.EventHandler(this.rbVref_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 13);
            this.label8.TabIndex = 53;
            this.label8.Text = "Inactive Sensor Connection ";
            // 
            // cbCIS
            // 
            this.cbCIS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCIS.FormattingEnabled = true;
            this.cbCIS.Items.AddRange(new object[] {
            "Ground",
            "Hi-Z Analog",
            "Shield"});
            this.cbCIS.Location = new System.Drawing.Point(214, 42);
            this.cbCIS.Margin = new System.Windows.Forms.Padding(4);
            this.cbCIS.Name = "cbCIS";
            this.cbCIS.Size = new System.Drawing.Size(111, 21);
            this.cbCIS.TabIndex = 52;
            // 
            // pbClk
            // 
            this.pbClk.BackColor = System.Drawing.Color.White;
            this.pbClk.Location = new System.Drawing.Point(4, 338);
            this.pbClk.Margin = new System.Windows.Forms.Padding(2);
            this.pbClk.Name = "pbClk";
            this.pbClk.Size = new System.Drawing.Size(331, 176);
            this.pbClk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbClk.TabIndex = 54;
            this.pbClk.TabStop = false;
            // 
            // cbBleedResistors_CH1
            // 
            this.cbBleedResistors_CH1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBleedResistors_CH1.FormattingEnabled = true;
            this.cbBleedResistors_CH1.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbBleedResistors_CH1.Location = new System.Drawing.Point(214, 96);
            this.cbBleedResistors_CH1.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbBleedResistors_CH1.Name = "cbBleedResistors_CH1";
            this.cbBleedResistors_CH1.Size = new System.Drawing.Size(110, 21);
            this.cbBleedResistors_CH1.TabIndex = 56;
            // 
            // lBleedResistors_CH1
            // 
            this.lBleedResistors_CH1.AutoSize = true;
            this.lBleedResistors_CH1.Location = new System.Drawing.Point(6, 98);
            this.lBleedResistors_CH1.Name = "lBleedResistors_CH1";
            this.lBleedResistors_CH1.Size = new System.Drawing.Size(132, 13);
            this.lBleedResistors_CH1.TabIndex = 55;
            this.lBleedResistors_CH1.Text = "Number of Bleed Resistors";
            // 
            // lShield
            // 
            this.lShield.AutoSize = true;
            this.lShield.Location = new System.Drawing.Point(5, 22);
            this.lShield.Name = "lShield";
            this.lShield.Size = new System.Drawing.Size(36, 13);
            this.lShield.TabIndex = 55;
            this.lShield.Text = "Shield";
            // 
            // cbShield
            // 
            this.cbShield.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShield.FormattingEnabled = true;
            this.cbShield.Items.AddRange(new object[] {
            "Disable",
            "Enable"});
            this.cbShield.Location = new System.Drawing.Point(214, 16);
            this.cbShield.MaximumSize = new System.Drawing.Size(161, 0);
            this.cbShield.Name = "cbShield";
            this.cbShield.Size = new System.Drawing.Size(110, 21);
            this.cbShield.TabIndex = 56;
            // 
            // cbIDACrange
            // 
            this.cbIDACrange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIDACrange.FormattingEnabled = true;
            this.cbIDACrange.Items.AddRange(new object[] {
            "31 uA",
            "255 uA",
            "2.04 mA"});
            this.cbIDACrange.Location = new System.Drawing.Point(214, 44);
            this.cbIDACrange.Margin = new System.Windows.Forms.Padding(4);
            this.cbIDACrange.Name = "cbIDACrange";
            this.cbIDACrange.Size = new System.Drawing.Size(111, 21);
            this.cbIDACrange.TabIndex = 57;
            // 
            // lIdacR
            // 
            this.lIdacR.AutoSize = true;
            this.lIdacR.Location = new System.Drawing.Point(6, 46);
            this.lIdacR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIdacR.Name = "lIdacR";
            this.lIdacR.Size = new System.Drawing.Size(65, 13);
            this.lIdacR.TabIndex = 58;
            this.lIdacR.Text = "IDAC range ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 75);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 58;
            this.label1.Text = "Guard Sensor";
            // 
            // cbGuardSensor
            // 
            this.cbGuardSensor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGuardSensor.FormattingEnabled = true;
            this.cbGuardSensor.Items.AddRange(new object[] {
            "Disabled",
            "Enabled"});
            this.cbGuardSensor.Location = new System.Drawing.Point(214, 70);
            this.cbGuardSensor.Margin = new System.Windows.Forms.Padding(4);
            this.cbGuardSensor.Name = "cbGuardSensor";
            this.cbGuardSensor.Size = new System.Drawing.Size(110, 21);
            this.cbGuardSensor.TabIndex = 57;
            // 
            // lDRICh0
            // 
            this.lDRICh0.AutoSize = true;
            this.lDRICh0.Location = new System.Drawing.Point(6, 46);
            this.lDRICh0.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lDRICh0.Name = "lDRICh0";
            this.lDRICh0.Size = new System.Drawing.Size(162, 13);
            this.lDRICh0.TabIndex = 58;
            this.lDRICh0.Text = "Digital Resource Implementation ";
            // 
            // cbDigitalResourceImplementationCh0
            // 
            this.cbDigitalResourceImplementationCh0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDigitalResourceImplementationCh0.FormattingEnabled = true;
            this.cbDigitalResourceImplementationCh0.Items.AddRange(new object[] {
            "FF Timer",
            "UDB"});
            this.cbDigitalResourceImplementationCh0.Location = new System.Drawing.Point(240, 44);
            this.cbDigitalResourceImplementationCh0.Margin = new System.Windows.Forms.Padding(4);
            this.cbDigitalResourceImplementationCh0.Name = "cbDigitalResourceImplementationCh0";
            this.cbDigitalResourceImplementationCh0.Size = new System.Drawing.Size(85, 21);
            this.cbDigitalResourceImplementationCh0.TabIndex = 57;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Analog Switch Divider ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 74);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = "Scan Speed";
            // 
            // cbScanSpeed
            // 
            this.cbScanSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScanSpeed.FormattingEnabled = true;
            this.cbScanSpeed.Items.AddRange(new object[] {
            "Slow",
            "Normal",
            "Fast",
            "Very Fast"});
            this.cbScanSpeed.Location = new System.Drawing.Point(126, 71);
            this.cbScanSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.cbScanSpeed.Name = "cbScanSpeed";
            this.cbScanSpeed.Size = new System.Drawing.Size(117, 21);
            this.cbScanSpeed.TabIndex = 57;
            // 
            // tbAnalogSwitchDivider
            // 
            this.tbAnalogSwitchDivider.Location = new System.Drawing.Point(126, 18);
            this.tbAnalogSwitchDivider.Margin = new System.Windows.Forms.Padding(2);
            this.tbAnalogSwitchDivider.Name = "tbAnalogSwitchDivider";
            this.tbAnalogSwitchDivider.Size = new System.Drawing.Size(117, 20);
            this.tbAnalogSwitchDivider.TabIndex = 59;
            this.tbAnalogSwitchDivider.Text = "11";
            this.tbAnalogSwitchDivider.TextChanged += new System.EventHandler(this.tbAnalogSwitchDivider_TextChanged);
            this.tbAnalogSwitchDivider.Validating += new System.ComponentModel.CancelEventHandler(this.tbAnalogSwitchDivider_Validating);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cbDigitalResourceImplementationCh1
            // 
            this.cbDigitalResourceImplementationCh1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDigitalResourceImplementationCh1.FormattingEnabled = true;
            this.cbDigitalResourceImplementationCh1.Items.AddRange(new object[] {
            "FF Timer",
            "UDB"});
            this.cbDigitalResourceImplementationCh1.Location = new System.Drawing.Point(240, 71);
            this.cbDigitalResourceImplementationCh1.Margin = new System.Windows.Forms.Padding(4);
            this.cbDigitalResourceImplementationCh1.Name = "cbDigitalResourceImplementationCh1";
            this.cbDigitalResourceImplementationCh1.Size = new System.Drawing.Size(85, 21);
            this.cbDigitalResourceImplementationCh1.TabIndex = 60;
            // 
            // lDRICh1
            // 
            this.lDRICh1.AutoSize = true;
            this.lDRICh1.Location = new System.Drawing.Point(6, 74);
            this.lDRICh1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lDRICh1.Name = "lDRICh1";
            this.lDRICh1.Size = new System.Drawing.Size(162, 13);
            this.lDRICh1.TabIndex = 61;
            this.lDRICh1.Text = "Digital Resource Implementation ";
            // 
            // gbImplementation
            // 
            this.gbImplementation.Controls.Add(this.cbPrescaler);
            this.gbImplementation.Controls.Add(this.cbDigitalResourceImplementationCh1);
            this.gbImplementation.Controls.Add(this.Labe1);
            this.gbImplementation.Controls.Add(this.lDRICh1);
            this.gbImplementation.Controls.Add(this.cbDigitalResourceImplementationCh0);
            this.gbImplementation.Controls.Add(this.lDRICh0);
            this.gbImplementation.Location = new System.Drawing.Point(4, 2);
            this.gbImplementation.Margin = new System.Windows.Forms.Padding(2);
            this.gbImplementation.Name = "gbImplementation";
            this.gbImplementation.Padding = new System.Windows.Forms.Padding(2);
            this.gbImplementation.Size = new System.Drawing.Size(331, 98);
            this.gbImplementation.TabIndex = 62;
            this.gbImplementation.TabStop = false;
            this.gbImplementation.Text = "Implementation";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbCurrentSource);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbBleedResistors_CH1);
            this.groupBox1.Controls.Add(this.lBleedResistors_CH0);
            this.groupBox1.Controls.Add(this.cbBleedResistors_CH0);
            this.groupBox1.Controls.Add(this.cbIDACrange);
            this.groupBox1.Controls.Add(this.lIdacR);
            this.groupBox1.Controls.Add(this.lBleedResistors_CH1);
            this.groupBox1.Location = new System.Drawing.Point(4, 106);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(330, 121);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbWidgetResolution);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbSensorAutoReset);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(340, 169);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(255, 71);
            this.groupBox2.TabIndex = 64;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbCIS);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cbGuardSensor);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbShield);
            this.groupBox3.Controls.Add(this.lShield);
            this.groupBox3.Location = new System.Drawing.Point(4, 230);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(330, 104);
            this.groupBox3.TabIndex = 65;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.cbPRS_EMIReduction);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.cbScanSpeed);
            this.groupBox4.Controls.Add(this.tbAnalogSwitchDivider);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(340, 2);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(255, 98);
            this.groupBox4.TabIndex = 66;
            this.groupBox4.TabStop = false;
            // 
            // pbHardwar0
            // 
            this.pbHardwar0.BackColor = System.Drawing.Color.White;
            this.pbHardwar0.Location = new System.Drawing.Point(4, 537);
            this.pbHardwar0.Margin = new System.Windows.Forms.Padding(2);
            this.pbHardwar0.Name = "pbHardwar0";
            this.pbHardwar0.Size = new System.Drawing.Size(291, 191);
            this.pbHardwar0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbHardwar0.TabIndex = 67;
            this.pbHardwar0.TabStop = false;
            // 
            // pbHardwar1
            // 
            this.pbHardwar1.BackColor = System.Drawing.Color.White;
            this.pbHardwar1.Location = new System.Drawing.Point(304, 537);
            this.pbHardwar1.Margin = new System.Windows.Forms.Padding(2);
            this.pbHardwar1.Name = "pbHardwar1";
            this.pbHardwar1.Size = new System.Drawing.Size(291, 191);
            this.pbHardwar1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbHardwar1.TabIndex = 67;
            this.pbHardwar1.TabStop = false;
            // 
            // lChannel0
            // 
            this.lChannel0.AutoSize = true;
            this.lChannel0.Location = new System.Drawing.Point(4, 520);
            this.lChannel0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lChannel0.Name = "lChannel0";
            this.lChannel0.Size = new System.Drawing.Size(55, 13);
            this.lChannel0.TabIndex = 68;
            this.lChannel0.Text = "Channel 0";
            // 
            // lChannel1
            // 
            this.lChannel1.AutoSize = true;
            this.lChannel1.Location = new System.Drawing.Point(302, 520);
            this.lChannel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lChannel1.Name = "lChannel1";
            this.lChannel1.Size = new System.Drawing.Size(55, 13);
            this.lChannel1.TabIndex = 68;
            this.lChannel1.Text = "Channel 1";
            // 
            // CyAdvancedTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.lChannel1);
            this.Controls.Add(this.lChannel0);
            this.Controls.Add(this.pbHardwar1);
            this.Controls.Add(this.pbHardwar0);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbImplementation);
            this.Controls.Add(this.pbClk);
            this.Controls.Add(this.gbVref);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CyAdvancedTab";
            this.Size = new System.Drawing.Size(551, 672);
            this.gbVref.ResumeLayout(false);
            this.gbVref.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.gbImplementation.ResumeLayout(false);
            this.gbImplementation.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHardwar0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHardwar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #endregion

        private Label Labe1;
        private Label lBleedResistors_CH0;
        private ComboBox cbBleedResistors_CH0;
        private Label label2;
        private ComboBox cbCurrentSource;
        private Label label3;
        private ComboBox cbPRS_EMIReduction;
        private Label label4;
        private ComboBox cbSensorAutoReset;
        private Label label5;
        private ComboBox cbWidgetResolution;
        private GroupBox gbVref;
        private RadioButton rbVdac;
        private RadioButton rbVref1024;
        private Label label8;
        private ComboBox cbCIS;
        private ComboBox cbPrescaler;
        #endregion

        bool m_loading = false;
        private PictureBox pbClk;
        private ComboBox cbBleedResistors_CH1;
        private Label lBleedResistors_CH1;
        private Label lShield;
        private ComboBox cbShield;
        private ComboBox cbIDACrange;
        private Label lIdacR;
        private Label label1;
        private ComboBox cbGuardSensor;
        private Label lDRICh0;
        private ComboBox cbDigitalResourceImplementationCh0;
        private Label label9;
        private Label label10;
        private ComboBox cbScanSpeed;
        private TextBox tbAnalogSwitchDivider;
        private TextBox tbVdacValue;
        private Label lVdacVal;
        private ErrorProvider errorProvider;
        private ComboBox cbDigitalResourceImplementationCh1;
        private Label lDRICh1;
        const string RB_LABEL = "Number of Bleed Resistors";
        const string DRI_LABEL = "Digital Resource Implementation";
        private GroupBox groupBox1;
        private GroupBox gbImplementation;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private PictureBox pbHardwar1;
        private PictureBox pbHardwar0;
        private Label lChannel1;
        private Label lChannel0;
        const string CHANNEL_LABEL = ", channel ";

        public override string TabName
        {
            get { return "Advanced"; }
        }

        public CyAdvancedTab(CyCSParameters packParams)
            : base()
        {
            m_loading = true;
            InitializeComponent();
            this.m_packParams = packParams;

            cbPRS_EMIReduction.Items.Clear();
            cbPRS_EMIReduction.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyPrsOptions)));
            cbPrescaler.Items.Clear();
            cbPrescaler.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyPrescalerOptions)));
            this.cbCIS.Items.Clear();
            this.cbCIS.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyConnectInactiveSensorsOptions)));
            this.cbIDACrange.Items.Clear();
            this.cbIDACrange.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyIdacRangeOptions)));
            this.cbScanSpeed.Items.Clear();
            this.cbScanSpeed.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyScanSpeedOptions)));
            this.cbGuardSensor.Items.Clear();
            this.cbGuardSensor.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyEnDis)));
            this.cbSensorAutoReset.Items.Clear();
            this.cbSensorAutoReset.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyEnDis)));
            this.cbShield.Items.Clear();
            this.cbShield.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyEnDis)));
            this.cbCurrentSource.Items.Clear();
            this.cbCurrentSource.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyCurrentSourceOptions)));

            this.cbDigitalResourceImplementationCh0.Items.Clear();
            this.cbDigitalResourceImplementationCh0.Items.AddRange(CyCsEnumConverter.GetDescriptionList(
                typeof(CyMeasureImplemetation)));
            this.cbDigitalResourceImplementationCh1.Items.Clear();
            this.cbDigitalResourceImplementationCh1.Items.AddRange(CyCsEnumConverter.GetDescriptionList(
                typeof(CyMeasureImplemetation)));

            foreach (Control item in this.Controls)
                if (item.GetType() == typeof(ComboBox))                
                    (item as ComboBox).SelectedIndex = 0;
                
            packParams.m_settings.m_configurationChanged += new EventHandler(UpdateConfiguration);
            packParams.m_updateAll += new EventHandler(GetProperties);
            CyCSParameters.AssingActions(this, new EventHandler(SendProperties));
            m_loading = false;
        }

        public void SendProperties(object sender, EventArgs e)
        {
            if (m_packParams == null || CyCSParameters.GLOBAL_EDIT_MODE == false || m_loading) return;
            m_loading = true;
            CyCSSettings settings = m_packParams.m_settings;
            settings.m_prescaler = (CyPrescalerOptions)CyCsEnumConverter.GetValue(cbPrescaler.Text,
                typeof(CyPrescalerOptions));
            settings.m_idacRange = (CyIdacRangeOptions)CyCsEnumConverter.GetValue(cbIDACrange.Text,
                typeof(CyIdacRangeOptions));
            settings.m_scanSpeed = (CyScanSpeedOptions)CyCsEnumConverter.GetValue(cbScanSpeed.Text,
                typeof(CyScanSpeedOptions));
            if (settings.m_shieldEnable != ((CyEnDis)cbShield.SelectedIndex == CyEnDis.Enabled))
            {
                settings.m_shieldEnable = (CyEnDis)cbShield.SelectedIndex == CyEnDis.Enabled;
                UpdateCIS();
            }
            settings.m_sensorAutoReset = (CyEnDis)cbSensorAutoReset.SelectedIndex == CyEnDis.Enabled;
            settings.m_guardSensorEnable = (CyEnDis)cbGuardSensor.SelectedIndex == CyEnDis.Enabled;
            settings.m_prs = (CyPrsOptions)cbPRS_EMIReduction.SelectedIndex;
            settings.m_widgetResolution = cbWidgetResolution.SelectedIndex == 0 ? (byte)8 : (byte)16;
            settings.m_listChannels[0].m_implementation = (CyMeasureImplemetation)CyCsEnumConverter.GetValue(
                cbDigitalResourceImplementationCh0.Text, typeof(CyMeasureImplemetation));
            settings.m_listChannels[1].m_implementation = (CyMeasureImplemetation)CyCsEnumConverter.GetValue(
                cbDigitalResourceImplementationCh1.Text, typeof(CyMeasureImplemetation));
            settings.m_connectInactiveSensors = (CyConnectInactiveSensorsOptions)cbCIS.SelectedIndex;
            CyCurrentSourceOptions mode = (CyCurrentSourceOptions)CyCsEnumConverter.GetValue(cbCurrentSource.Text,
                typeof(CyCurrentSourceOptions));
            if (mode != CyCurrentSourceOptions.Idac_Source)
                rbVref1024.Checked = true;
            if (settings.m_currentSource != mode)
            {
                settings.m_currentSource = mode;
                settings.ConfigurationChanged();
            }

            settings.m_vrefOption = CyVrefOptions.Ref_Vdac;
            if (rbVref1024.Checked)
                settings.m_vrefOption = CyVrefOptions.Ref_Vref_1_24;

            settings.m_vrefVdacValue = byte.Parse(tbVdacValue.Text);
            if (CyCsConst.C_RB_COUNT.CheckRange(cbBleedResistors_CH0.Text))
                settings.m_listChannels[0].m_rb = int.Parse(cbBleedResistors_CH0.Text);
            if (CyCsConst.C_RB_COUNT.CheckRange(cbBleedResistors_CH1.Text))
                settings.m_listChannels[1].m_rb = int.Parse(cbBleedResistors_CH1.Text);

            byte val;
            if (byte.TryParse(tbAnalogSwitchDivider.Text, out val))
                settings.m_analogSwitchDivider = val;

            UpdateConfiguration(null, null);
            m_packParams.GuardSensorEnableChange(settings.m_guardSensorEnable);
            m_packParams.SetCommitParams(null, null);

            tbAnalogSwitchDivider.Enabled = settings.m_prescaler != CyPrescalerOptions.Prescaler_None;
            m_loading = false;
        }

        public void GetProperties(object sender, EventArgs e)
        {
            if (m_packParams == null) return;

            CyCSSettings settings = m_packParams.m_settings;
            m_loading = true;

            UpdateConfiguration(null, null);

            CyCsEnumConverter.SetValue(cbCurrentSource, CyCsEnumConverter.GetDescription(settings.m_currentSource));
            if (settings.m_currentSource != CyCurrentSourceOptions.Idac_Source)
            {
                rbVref1024.Checked = true;
            }
            CyCsEnumConverter.SetValue(cbScanSpeed, CyCsEnumConverter.GetDescription(settings.m_scanSpeed));
            CyCsEnumConverter.SetValue(cbIDACrange, CyCsEnumConverter.GetDescription(settings.m_idacRange));
            CyCsEnumConverter.SetValue(cbShield, CyCsEnumConverter.GetDescription(settings.m_shieldEnable));
            CyCsEnumConverter.SetValue(cbSensorAutoReset, CyCsEnumConverter.GetDescription(settings.m_sensorAutoReset));
            CyCsEnumConverter.SetValue(cbGuardSensor, CyCsEnumConverter.GetDescription(settings.m_guardSensorEnable));
            CyCsEnumConverter.SetValue(cbPrescaler, CyCsEnumConverter.GetDescription(settings.m_prescaler));
            CyCsEnumConverter.SetValue(cbPRS_EMIReduction, CyCsEnumConverter.GetDescription(settings.m_prs));
            CyCsEnumConverter.SetValue(cbBleedResistors_CH0, settings.m_listChannels[0].m_rb.ToString());
            CyCsEnumConverter.SetValue(cbBleedResistors_CH1, settings.m_listChannels[1].m_rb.ToString());
            CyCsEnumConverter.SetValue(cbDigitalResourceImplementationCh0,
                CyCsEnumConverter.GetDescription(settings.m_listChannels[0].m_implementation));
            CyCsEnumConverter.SetValue(cbDigitalResourceImplementationCh1,
                CyCsEnumConverter.GetDescription(settings.m_listChannels[1].m_implementation));
            rbVref1024.Checked = settings.m_vrefOption == CyVrefOptions.Ref_Vref_1_24;
            rbVdac.Checked = settings.m_vrefOption == CyVrefOptions.Ref_Vdac;
            tbVdacValue.Text = settings.m_vrefVdacValue.ToString();
            tbAnalogSwitchDivider.Text = settings.m_analogSwitchDivider.ToString();

            CyCsEnumConverter.SetValue(cbCIS, CyCsEnumConverter.
               GetDescription(settings.m_connectInactiveSensors));
            UpdateCIS();

            cbWidgetResolution.SelectedIndex = settings.m_widgetResolution == 8 ? 0 : 1;
            m_loading = false;
            if (cbCIS.SelectedIndex == -1)
                cbCIS.SelectedIndex = 0;
            rbVref_CheckedChanged(null, null);
        }
        void UpdateCIS()
        {
            int index = cbCIS.SelectedIndex;
            //CIS change values range
            this.cbCIS.Items.Clear();
            this.cbCIS.Items.AddRange(CyCsEnumConverter.GetDescriptionList(typeof(CyConnectInactiveSensorsOptions)));
            if (m_packParams.m_settings.m_shieldEnable == false)
                this.cbCIS.Items.RemoveAt((int)CyConnectInactiveSensorsOptions.Shield);
            cbCIS.SelectedIndex = index < cbCIS.Items.Count ? index : 0;
        }

        void UpdateConfiguration(object sender, EventArgs e)
        {
            CyCSSettings settings = m_packParams.m_settings;
            bool twochannels = m_packParams.m_settings.Configuration == CyChannelConfig.TWO_CHANNELS;
            bool isRb = m_packParams.m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None;
            cbBleedResistors_CH0.Enabled = isRb;
            lBleedResistors_CH0.Enabled = isRb;
            cbBleedResistors_CH1.Enabled = twochannels && isRb;
            lBleedResistors_CH1.Enabled = twochannels && isRb;
            lBleedResistors_CH0.Text = twochannels ? RB_LABEL + CHANNEL_LABEL + 0 : RB_LABEL;
            lBleedResistors_CH1.Text = RB_LABEL + CHANNEL_LABEL + 1;
            cbIDACrange.Enabled = isRb == false;
            lIdacR.Enabled = isRb == false;
            lDRICh0.Text = twochannels ? DRI_LABEL + CHANNEL_LABEL + 0 : DRI_LABEL;
            lDRICh1.Text = DRI_LABEL + CHANNEL_LABEL + 1; ;
            cbDigitalResourceImplementationCh1.Enabled = twochannels;
            lDRICh1.Enabled = twochannels;
            pbHardwar1.Visible = twochannels;
            lChannel0.Visible = twochannels;
            lChannel1.Visible = twochannels;
            if (twochannels)
                pbHardwar0.Top = lChannel0.Top + lChannel0.Height + 3;
            else
                pbHardwar0.Top = lChannel0.Top;
            pbHardwar1.Top = pbHardwar0.Top;

            bool isGbVref = settings.m_tuningMethod != CyTuningMethodOptions.Tuning_Auto &&
                settings.m_currentSource == CyCurrentSourceOptions.Idac_Source;
            rbVdac.Enabled = isGbVref;
            tbVdacValue.Enabled = isGbVref;
            cbPRS_EMIReduction.Enabled = settings.m_tuningMethod != CyTuningMethodOptions.Tuning_Auto;
            if (settings.m_tuningMethod == CyTuningMethodOptions.Tuning_Auto)
            {
                CyCsEnumConverter.SetValue(cbPRS_EMIReduction,
                    CyCsEnumConverter.GetDescription(CyPrsOptions.Prs_16bits));
                CyCsEnumConverter.SetValue(cbIDACrange, CyCsEnumConverter.GetDescription(CyIdacRangeOptions.fs_255uA));
                rbVref1024.Checked = true;
                cbIDACrange.Enabled = false;
                lIdacR.Enabled = false;
            }
            pbClk.Image = CyCsResource.blank;
            switch (settings.m_prescaler)
            {
                case CyPrescalerOptions.Prescaler_None:
                    if (settings.m_prs == CyPrsOptions.Prs_None)
                        pbClk.Image = CyCsResource.clk;
                    if (settings.m_prs == CyPrsOptions.Prs_8bits)
                        pbClk.Image = CyCsResource.clk_prs8;
                    if (settings.m_prs == CyPrsOptions.Prs_16bits)
                        pbClk.Image = CyCsResource.clk_prs16;
                    if (settings.m_prs == CyPrsOptions.Prs_16bits_0_25)
                        pbClk.Image = CyCsResource.clk_prs16_4;
                    break;
                case CyPrescalerOptions.Prescaler_UDB:
                    if (settings.m_prs == CyPrsOptions.Prs_None)
                        pbClk.Image = CyCsResource.clk_udb;
                    if (settings.m_prs == CyPrsOptions.Prs_8bits)
                        pbClk.Image = CyCsResource.clk_udb_prs8;
                    if (settings.m_prs == CyPrsOptions.Prs_16bits)
                        pbClk.Image = CyCsResource.clk_udb_prs16;
                    if (settings.m_prs == CyPrsOptions.Prs_16bits_0_25)
                        pbClk.Image = CyCsResource.clk_udb_prs16_4;
                    break;
                case CyPrescalerOptions.Prescaler_FF:

                    if (settings.m_prs == CyPrsOptions.Prs_None)
                        pbClk.Image = CyCsResource.clk_ff;
                    if (settings.m_prs == CyPrsOptions.Prs_8bits)
                        pbClk.Image = CyCsResource.clk_ff_prs8;
                    if (settings.m_prs == CyPrsOptions.Prs_16bits)
                        pbClk.Image = CyCsResource.clk_ff_prs16;
                    if (settings.m_prs == CyPrsOptions.Prs_16bits_0_25)
                        pbClk.Image = CyCsResource.clk_ff_prs16_4;
                    break;
                default:
                    break;
            }
            for (int i = 0; i < 1 + Convert.ToInt16(twochannels); i++)
            {
                CyChannelProperties pr = settings.m_listChannels[i];
                PictureBox pb = i == 0 ? pbHardwar0 : pbHardwar1;
                pb.Image = CyCsResource.blank;
                switch (settings.m_currentSource)
                {
                    case CyCurrentSourceOptions.Idac_None:
                        if (pr.m_implementation == CyMeasureImplemetation.FF_Based)
                            pb.Image = CyCsResource.rb_vref_ff;
                        if (pr.m_implementation == CyMeasureImplemetation.UDB_Based)
                            pb.Image = CyCsResource.rb_vref_udb;
                        break;
                    case CyCurrentSourceOptions.Idac_Source:
                        if (pr.m_implementation == CyMeasureImplemetation.FF_Based)
                        {
                            if (settings.m_vrefOption == CyVrefOptions.Ref_Vdac)
                                pb.Image = CyCsResource.sourcing_vdac_ff;
                            if (settings.m_vrefOption == CyVrefOptions.Ref_Vref_1_24)
                                pb.Image = CyCsResource.sourcing_vref_ff;
                        }
                        if (pr.m_implementation == CyMeasureImplemetation.UDB_Based)
                        {
                            if (settings.m_vrefOption == CyVrefOptions.Ref_Vdac)
                                pb.Image = CyCsResource.sourcing_vdac_udb;
                            if (settings.m_vrefOption == CyVrefOptions.Ref_Vref_1_24)
                                pb.Image = CyCsResource.sourcing_vref_udb;
                        }
                        break;
                    case CyCurrentSourceOptions.Idac_Sink:
                        if (pr.m_implementation == CyMeasureImplemetation.FF_Based)
                            pb.Image = CyCsResource.sinking_vref_ff;
                        if (pr.m_implementation == CyMeasureImplemetation.UDB_Based)
                            pb.Image = CyCsResource.sinking_vref_ff;
                        break;
                    default:
                        break;
                }
            }

        }
        private void rbVref_CheckedChanged(object sender, EventArgs e)
        {
            tbVdacValue.Enabled = rbVdac.Checked;
            lVdacVal.Enabled = rbVdac.Checked;
        }

        private void tbAnalogSwitchDivider_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ex = new CancelEventArgs();
            tbAnalogSwitchDivider_Validating(sender, ex);
        }

        private void tbAnalogSwitchDivider_Validating(object sender, CancelEventArgs e)
        {
            if (CyCsConst.C_ANALOG_SWITCH_DIVIDER.CheckRange(tbAnalogSwitchDivider.Text))
                errorProvider.SetError((Control)sender, "");
            else
            {
                errorProvider.SetError((Control)sender, String.Format(CyCsResource.ValueLitation,
    CyCsConst.C_ANALOG_SWITCH_DIVIDER.m_min, CyCsConst.C_ANALOG_SWITCH_DIVIDER.m_max));
                e.Cancel = true;
            }
        }
        private void tbVdacValue_TextChanged(object sender, EventArgs e)
        {
            CancelEventArgs ex = new CancelEventArgs();
            tbVdacValue_Validating(sender, ex);
        }

        private void tbVdacValue_Validating(object sender, CancelEventArgs e)
        {
            if (CyCsConst.C_VREF_VALUE.CheckRange(tbVdacValue.Text))
            {
                errorProvider.SetError((Control)sender, "");
                byte val = byte.Parse(tbVdacValue.Text);
                lVdacVal.Text = Math.Round(val * 0.016, 3).ToString() + " V";
            }
            else
            {
                errorProvider.SetError((Control)sender, String.Format(CyCsResource.ValueLitation,
    CyCsConst.C_VREF_VALUE.m_min, CyCsConst.C_VREF_VALUE.m_max));
                e.Cancel = true;
            }
        }

        private void cbCurrentSource_Validating(object sender, CancelEventArgs e)
        {
            if (cbCurrentSource.SelectedIndex == 0 &&
                  m_packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_Auto)
            {
                errorProvider.SetError(cbCurrentSource, CyCsResource.AutoTuningLimitation);
                e.Cancel = true;
            }
            else
                errorProvider.SetError(cbCurrentSource, string.Empty);
        }

        private void cbCurrentSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelEventArgs ex = new CancelEventArgs();
            cbCurrentSource_Validating(null, ex);
        }
    }
}

