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

namespace IDAC8_v1_60
{
    partial class IDACControl
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
            this.components = new System.ComponentModel.Container();
            this.lblIDAC = new System.Windows.Forms.Label();
            this.gBoxPolarity = new System.Windows.Forms.GroupBox();
            this.PolarityNegative = new System.Windows.Forms.RadioButton();
            this.PolarityPositive = new System.Windows.Forms.RadioButton();
            this.gBoxRange = new System.Windows.Forms.GroupBox();
            this.rButtonRange3 = new System.Windows.Forms.RadioButton();
            this.rButtonRange2 = new System.Windows.Forms.RadioButton();
            this.rButtonRange1 = new System.Windows.Forms.RadioButton();
            this.gBoxInitialValue = new System.Windows.Forms.GroupBox();
            this.lblHex = new System.Windows.Forms.Label();
            this.lblUA = new System.Windows.Forms.Label();
            this.lblRange = new System.Windows.Forms.Label();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.gBoxDataSource = new System.Windows.Forms.GroupBox();
            this.rButtonDataBus = new System.Windows.Forms.RadioButton();
            this.rButtonDacBus = new System.Windows.Forms.RadioButton();
            this.gBoxSpeed = new System.Windows.Forms.GroupBox();
            this.rButtonHighSpeed = new System.Windows.Forms.RadioButton();
            this.rButtonLowSpeed = new System.Windows.Forms.RadioButton();
            this.gBoxStrobeMode = new System.Windows.Forms.GroupBox();
            this.rButtonRegister = new System.Windows.Forms.RadioButton();
            this.rButtonExternal = new System.Windows.Forms.RadioButton();
            this.CurrentByteUpDown = new IDAC8_v1_60.CyNumericUpDown();
            this.CurrentValueUpDown = new IDAC8_v1_60.CyNumericUpDown();
            this.CurrentPercentageUpDown = new IDAC8_v1_60.CyNumericUpDown();
            this.gBoxPolarity.SuspendLayout();
            this.gBoxRange.SuspendLayout();
            this.gBoxInitialValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.gBoxDataSource.SuspendLayout();
            this.gBoxSpeed.SuspendLayout();
            this.gBoxStrobeMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentByteUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentValueUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentPercentageUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIDAC
            // 
            this.lblIDAC.AutoSize = true;
            this.lblIDAC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIDAC.Location = new System.Drawing.Point(4, 4);
            this.lblIDAC.Name = "lblIDAC";
            this.lblIDAC.Size = new System.Drawing.Size(39, 16);
            this.lblIDAC.TabIndex = 0;
            this.lblIDAC.Text = "IDAC";
            // 
            // gBoxPolarity
            // 
            this.gBoxPolarity.Controls.Add(this.PolarityNegative);
            this.gBoxPolarity.Controls.Add(this.PolarityPositive);
            this.gBoxPolarity.Location = new System.Drawing.Point(7, 23);
            this.gBoxPolarity.Name = "gBoxPolarity";
            this.gBoxPolarity.Size = new System.Drawing.Size(186, 78);
            this.gBoxPolarity.TabIndex = 1;
            this.gBoxPolarity.TabStop = false;
            this.gBoxPolarity.Text = "Polarity";
            // 
            // PolarityNegative
            // 
            this.PolarityNegative.AutoSize = true;
            this.PolarityNegative.Location = new System.Drawing.Point(7, 43);
            this.PolarityNegative.Name = "PolarityNegative";
            this.PolarityNegative.Size = new System.Drawing.Size(98, 17);
            this.PolarityNegative.TabIndex = 1;
            this.PolarityNegative.TabStop = true;
            this.PolarityNegative.Text = "Negative (Sink)";
            this.PolarityNegative.UseVisualStyleBackColor = true;
            this.PolarityNegative.CheckedChanged += new System.EventHandler(this.PolarityNegative_CheckedChanged);
            // 
            // PolarityPositive
            // 
            this.PolarityPositive.AutoSize = true;
            this.PolarityPositive.Checked = true;
            this.PolarityPositive.Location = new System.Drawing.Point(7, 20);
            this.PolarityPositive.Name = "PolarityPositive";
            this.PolarityPositive.Size = new System.Drawing.Size(105, 17);
            this.PolarityPositive.TabIndex = 0;
            this.PolarityPositive.TabStop = true;
            this.PolarityPositive.Text = "Positive (Source)";
            this.PolarityPositive.UseVisualStyleBackColor = true;
            this.PolarityPositive.CheckedChanged += new System.EventHandler(this.PolarityPositive_CheckedChanged);
            // 
            // gBoxRange
            // 
            this.gBoxRange.Controls.Add(this.rButtonRange3);
            this.gBoxRange.Controls.Add(this.rButtonRange2);
            this.gBoxRange.Controls.Add(this.rButtonRange1);
            this.gBoxRange.Location = new System.Drawing.Point(7, 110);
            this.gBoxRange.Name = "gBoxRange";
            this.gBoxRange.Size = new System.Drawing.Size(186, 92);
            this.gBoxRange.TabIndex = 2;
            this.gBoxRange.TabStop = false;
            this.gBoxRange.Text = "Range";
            // 
            // rButtonRange3
            // 
            this.rButtonRange3.AutoSize = true;
            this.rButtonRange3.Location = new System.Drawing.Point(8, 66);
            this.rButtonRange3.Name = "rButtonRange3";
            this.rButtonRange3.Size = new System.Drawing.Size(80, 17);
            this.rButtonRange3.TabIndex = 2;
            this.rButtonRange3.TabStop = true;
            this.rButtonRange3.Text = "0 - 2040 uA";
            this.rButtonRange3.UseVisualStyleBackColor = true;
            this.rButtonRange3.CheckedChanged += new System.EventHandler(this.rButtonRange3_CheckedChanged);
            // 
            // rButtonRange2
            // 
            this.rButtonRange2.AutoSize = true;
            this.rButtonRange2.Location = new System.Drawing.Point(7, 43);
            this.rButtonRange2.Name = "rButtonRange2";
            this.rButtonRange2.Size = new System.Drawing.Size(74, 17);
            this.rButtonRange2.TabIndex = 1;
            this.rButtonRange2.TabStop = true;
            this.rButtonRange2.Text = "0 - 255 uA";
            this.rButtonRange2.UseVisualStyleBackColor = true;
            this.rButtonRange2.CheckedChanged += new System.EventHandler(this.rButtonRange2_CheckedChanged);
            // 
            // rButtonRange1
            // 
            this.rButtonRange1.AutoSize = true;
            this.rButtonRange1.Checked = true;
            this.rButtonRange1.Location = new System.Drawing.Point(7, 20);
            this.rButtonRange1.Name = "rButtonRange1";
            this.rButtonRange1.Size = new System.Drawing.Size(89, 17);
            this.rButtonRange1.TabIndex = 0;
            this.rButtonRange1.TabStop = true;
            this.rButtonRange1.Text = "0 - 31.875 uA";
            this.rButtonRange1.UseVisualStyleBackColor = true;
            this.rButtonRange1.CheckedChanged += new System.EventHandler(this.rButtonRange1_CheckedChanged);
            // 
            // gBoxInitialValue
            // 
            this.gBoxInitialValue.Controls.Add(this.CurrentByteUpDown);
            this.gBoxInitialValue.Controls.Add(this.CurrentValueUpDown);
            this.gBoxInitialValue.Controls.Add(this.CurrentPercentageUpDown);
            this.gBoxInitialValue.Controls.Add(this.lblHex);
            this.gBoxInitialValue.Controls.Add(this.lblUA);
            this.gBoxInitialValue.Controls.Add(this.lblRange);
            this.gBoxInitialValue.Location = new System.Drawing.Point(7, 208);
            this.gBoxInitialValue.Name = "gBoxInitialValue";
            this.gBoxInitialValue.Size = new System.Drawing.Size(186, 115);
            this.gBoxInitialValue.TabIndex = 3;
            this.gBoxInitialValue.TabStop = false;
            this.gBoxInitialValue.Text = "Initial Value";
            // 
            // lblHex
            // 
            this.lblHex.AutoSize = true;
            this.lblHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHex.Location = new System.Drawing.Point(9, 77);
            this.lblHex.Name = "lblHex";
            this.lblHex.Size = new System.Drawing.Size(49, 13);
            this.lblHex.TabIndex = 5;
            this.lblHex.Text = "8 bit Hex";
            // 
            // lblUA
            // 
            this.lblUA.AutoSize = true;
            this.lblUA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUA.Location = new System.Drawing.Point(9, 51);
            this.lblUA.Name = "lblUA";
            this.lblUA.Size = new System.Drawing.Size(20, 13);
            this.lblUA.TabIndex = 3;
            this.lblUA.Text = "uA";
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRange.Location = new System.Drawing.Point(9, 21);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(50, 13);
            this.lblRange.TabIndex = 1;
            this.lblRange.Text = "% Range";
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // gBoxDataSource
            // 
            this.gBoxDataSource.Controls.Add(this.rButtonDataBus);
            this.gBoxDataSource.Controls.Add(this.rButtonDacBus);
            this.gBoxDataSource.Location = new System.Drawing.Point(225, 23);
            this.gBoxDataSource.Name = "gBoxDataSource";
            this.gBoxDataSource.Size = new System.Drawing.Size(186, 78);
            this.gBoxDataSource.TabIndex = 4;
            this.gBoxDataSource.TabStop = false;
            this.gBoxDataSource.Text = "Data Source";
            // 
            // rButtonDataBus
            // 
            this.rButtonDataBus.AutoSize = true;
            this.rButtonDataBus.Location = new System.Drawing.Point(7, 43);
            this.rButtonDataBus.Name = "rButtonDataBus";
            this.rButtonDataBus.Size = new System.Drawing.Size(139, 17);
            this.rButtonDataBus.TabIndex = 1;
            this.rButtonDataBus.TabStop = true;
            this.rButtonDataBus.Text = "CPU or DMA (Data Bus)";
            this.rButtonDataBus.UseVisualStyleBackColor = true;
            this.rButtonDataBus.CheckedChanged += new System.EventHandler(this.rButtonDataBus_CheckedChanged);
            // 
            // rButtonDacBus
            // 
            this.rButtonDacBus.AutoSize = true;
            this.rButtonDacBus.Location = new System.Drawing.Point(7, 20);
            this.rButtonDacBus.Name = "rButtonDacBus";
            this.rButtonDacBus.Size = new System.Drawing.Size(68, 17);
            this.rButtonDacBus.TabIndex = 0;
            this.rButtonDacBus.Text = "DAC Bus";
            this.rButtonDacBus.UseVisualStyleBackColor = true;
            this.rButtonDacBus.CheckedChanged += new System.EventHandler(this.rButtonDacBus_CheckedChanged);
            // 
            // gBoxSpeed
            // 
            this.gBoxSpeed.Controls.Add(this.rButtonHighSpeed);
            this.gBoxSpeed.Controls.Add(this.rButtonLowSpeed);
            this.gBoxSpeed.Location = new System.Drawing.Point(225, 111);
            this.gBoxSpeed.Name = "gBoxSpeed";
            this.gBoxSpeed.Size = new System.Drawing.Size(186, 78);
            this.gBoxSpeed.TabIndex = 5;
            this.gBoxSpeed.TabStop = false;
            this.gBoxSpeed.Text = "Speed";
            // 
            // rButtonHighSpeed
            // 
            this.rButtonHighSpeed.AutoSize = true;
            this.rButtonHighSpeed.Location = new System.Drawing.Point(7, 43);
            this.rButtonHighSpeed.Name = "rButtonHighSpeed";
            this.rButtonHighSpeed.Size = new System.Drawing.Size(81, 17);
            this.rButtonHighSpeed.TabIndex = 1;
            this.rButtonHighSpeed.TabStop = true;
            this.rButtonHighSpeed.Text = "High Speed";
            this.rButtonHighSpeed.UseVisualStyleBackColor = true;
            this.rButtonHighSpeed.CheckedChanged += new System.EventHandler(this.rButtonHighSpeed_CheckedChanged);
            // 
            // rButtonLowSpeed
            // 
            this.rButtonLowSpeed.AutoSize = true;
            this.rButtonLowSpeed.Location = new System.Drawing.Point(7, 20);
            this.rButtonLowSpeed.Name = "rButtonLowSpeed";
            this.rButtonLowSpeed.Size = new System.Drawing.Size(79, 17);
            this.rButtonLowSpeed.TabIndex = 0;
            this.rButtonLowSpeed.Text = "Low Speed";
            this.rButtonLowSpeed.UseVisualStyleBackColor = true;
            this.rButtonLowSpeed.CheckedChanged += new System.EventHandler(this.rButtonLowSpeed_CheckedChanged);
            // 
            // gBoxStrobeMode
            // 
            this.gBoxStrobeMode.Controls.Add(this.rButtonRegister);
            this.gBoxStrobeMode.Controls.Add(this.rButtonExternal);
            this.gBoxStrobeMode.Location = new System.Drawing.Point(225, 208);
            this.gBoxStrobeMode.Name = "gBoxStrobeMode";
            this.gBoxStrobeMode.Size = new System.Drawing.Size(186, 78);
            this.gBoxStrobeMode.TabIndex = 6;
            this.gBoxStrobeMode.TabStop = false;
            this.gBoxStrobeMode.Text = "Strobe Mode";
            // 
            // rButtonRegister
            // 
            this.rButtonRegister.AutoSize = true;
            this.rButtonRegister.Location = new System.Drawing.Point(7, 43);
            this.rButtonRegister.Name = "rButtonRegister";
            this.rButtonRegister.Size = new System.Drawing.Size(92, 17);
            this.rButtonRegister.TabIndex = 1;
            this.rButtonRegister.Text = "Register Write";
            this.rButtonRegister.UseVisualStyleBackColor = true;
            this.rButtonRegister.CheckedChanged += new System.EventHandler(this.rButtonRegister_CheckedChanged);
            // 
            // rButtonExternal
            // 
            this.rButtonExternal.AutoSize = true;
            this.rButtonExternal.Location = new System.Drawing.Point(7, 20);
            this.rButtonExternal.Name = "rButtonExternal";
            this.rButtonExternal.Size = new System.Drawing.Size(63, 17);
            this.rButtonExternal.TabIndex = 0;
            this.rButtonExternal.Text = "External";
            this.rButtonExternal.UseVisualStyleBackColor = true;
            this.rButtonExternal.CheckedChanged += new System.EventHandler(this.rButtonExternal_CheckedChanged);
            // 
            // CurrentByteUpDown
            // 
            this.CurrentByteUpDown.Hexadecimal = true;
            this.CurrentByteUpDown.Location = new System.Drawing.Point(64, 77);
            this.CurrentByteUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.CurrentByteUpDown.Name = "CurrentByteUpDown";
            this.CurrentByteUpDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CurrentByteUpDown.Size = new System.Drawing.Size(85, 20);
            this.CurrentByteUpDown.TabIndex = 9;
            this.CurrentByteUpDown.ValueChanged += new System.EventHandler(this.CurrentByteUpDown_ValueChanged);
            this.CurrentByteUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CurrentByteUpDown_KeyUp);
            this.CurrentByteUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.CurrentByteUpDown_Validating);
            // 
            // CurrentValueUpDown
            // 
            this.CurrentValueUpDown.Location = new System.Drawing.Point(65, 51);
            this.CurrentValueUpDown.Maximum = new decimal(new int[] {
            2040,
            0,
            0,
            0});
            this.CurrentValueUpDown.Name = "CurrentValueUpDown";
            this.CurrentValueUpDown.Size = new System.Drawing.Size(84, 20);
            this.CurrentValueUpDown.TabIndex = 8;
            this.CurrentValueUpDown.ValueChanged += new System.EventHandler(this.CurrentValueUpDown_ValueChanged);
            this.CurrentValueUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CurrentValue_UpDown_KeyUp);
            this.CurrentValueUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.CurrentValue_Validating);
            // 
            // CurrentPercentageUpDown
            // 
            this.CurrentPercentageUpDown.Location = new System.Drawing.Point(65, 19);
            this.CurrentPercentageUpDown.Name = "CurrentPercentageUpDown";
            this.CurrentPercentageUpDown.Size = new System.Drawing.Size(84, 20);
            this.CurrentPercentageUpDown.TabIndex = 7;
            this.CurrentPercentageUpDown.ValueChanged += new System.EventHandler(this.CurrentPercentageUpDown_ValueChanged);
            this.CurrentPercentageUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Current_UpDown_KeyUp);
            this.CurrentPercentageUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.Current_Validating);
            // 
            // IDACControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gBoxStrobeMode);
            this.Controls.Add(this.gBoxSpeed);
            this.Controls.Add(this.gBoxDataSource);
            this.Controls.Add(this.gBoxInitialValue);
            this.Controls.Add(this.gBoxRange);
            this.Controls.Add(this.gBoxPolarity);
            this.Controls.Add(this.lblIDAC);
            this.Name = "IDACControl";
            this.Size = new System.Drawing.Size(451, 330);
            this.gBoxPolarity.ResumeLayout(false);
            this.gBoxPolarity.PerformLayout();
            this.gBoxRange.ResumeLayout(false);
            this.gBoxRange.PerformLayout();
            this.gBoxInitialValue.ResumeLayout(false);
            this.gBoxInitialValue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.gBoxDataSource.ResumeLayout(false);
            this.gBoxDataSource.PerformLayout();
            this.gBoxSpeed.ResumeLayout(false);
            this.gBoxSpeed.PerformLayout();
            this.gBoxStrobeMode.ResumeLayout(false);
            this.gBoxStrobeMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentByteUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentValueUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentPercentageUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIDAC;
        private System.Windows.Forms.GroupBox gBoxPolarity;
        private System.Windows.Forms.RadioButton PolarityNegative;
        private System.Windows.Forms.RadioButton PolarityPositive;
        private System.Windows.Forms.GroupBox gBoxRange;
        private System.Windows.Forms.RadioButton rButtonRange3;
        private System.Windows.Forms.RadioButton rButtonRange2;
        private System.Windows.Forms.RadioButton rButtonRange1;
        private System.Windows.Forms.GroupBox gBoxInitialValue;
        private System.Windows.Forms.Label lblHex;
        private System.Windows.Forms.Label lblUA;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.ErrorProvider ep_Errors;
        private CyNumericUpDown CurrentByteUpDown;
        private CyNumericUpDown CurrentValueUpDown;
        private CyNumericUpDown CurrentPercentageUpDown;
        private System.Windows.Forms.GroupBox gBoxDataSource;
        private System.Windows.Forms.RadioButton rButtonDataBus;
        private System.Windows.Forms.RadioButton rButtonDacBus;
        private System.Windows.Forms.GroupBox gBoxStrobeMode;
        private System.Windows.Forms.RadioButton rButtonRegister;
        private System.Windows.Forms.RadioButton rButtonExternal;
        private System.Windows.Forms.GroupBox gBoxSpeed;
        private System.Windows.Forms.RadioButton rButtonHighSpeed;
        private System.Windows.Forms.RadioButton rButtonLowSpeed;
    }
}

//[] END OF FILE
