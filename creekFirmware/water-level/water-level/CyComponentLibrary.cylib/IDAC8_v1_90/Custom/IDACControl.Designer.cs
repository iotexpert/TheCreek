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

namespace IDAC8_v1_90
{
    partial class CyIDACControl
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
            this.PolarityBox = new System.Windows.Forms.GroupBox();
            this.Hardware_Controlled = new System.Windows.Forms.RadioButton();
            this.PolarityNegative = new System.Windows.Forms.RadioButton();
            this.PolarityPositive = new System.Windows.Forms.RadioButton();
            this.OutPutBox = new System.Windows.Forms.GroupBox();
            this.lbl2mA = new System.Windows.Forms.Label();
            this.lbl255A = new System.Windows.Forms.Label();
            this.lbl32A = new System.Windows.Forms.Label();
            this.OutPutRange3 = new System.Windows.Forms.RadioButton();
            this.OutPutRange2 = new System.Windows.Forms.RadioButton();
            this.OutPutRange1 = new System.Windows.Forms.RadioButton();
            this.SpeedBox = new System.Windows.Forms.GroupBox();
            this.High = new System.Windows.Forms.RadioButton();
            this.Slow = new System.Windows.Forms.RadioButton();
            this.ValueBox = new System.Windows.Forms.GroupBox();
            this.BytesValue = new System.Windows.Forms.TextBox();
            this.AmpsValue = new System.Windows.Forms.TextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblHex = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.DataSource = new System.Windows.Forms.GroupBox();
            this.rButtonDataBus = new System.Windows.Forms.RadioButton();
            this.rButtonDacBus = new System.Windows.Forms.RadioButton();
            this.StrobeMode = new System.Windows.Forms.GroupBox();
            this.rButtonRegister = new System.Windows.Forms.RadioButton();
            this.rButtonExternal = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Hardware_Enable = new System.Windows.Forms.CheckBox();
            this.PolarityBox.SuspendLayout();
            this.OutPutBox.SuspendLayout();
            this.SpeedBox.SuspendLayout();
            this.ValueBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.DataSource.SuspendLayout();
            this.StrobeMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // PolarityBox
            // 
            this.PolarityBox.Controls.Add(this.Hardware_Controlled);
            this.PolarityBox.Controls.Add(this.PolarityNegative);
            this.PolarityBox.Controls.Add(this.PolarityPositive);
            this.PolarityBox.Location = new System.Drawing.Point(7, 39);
            this.PolarityBox.Name = "PolarityBox";
            this.PolarityBox.Size = new System.Drawing.Size(186, 89);
            this.PolarityBox.TabIndex = 1;
            this.PolarityBox.TabStop = false;
            this.PolarityBox.Text = "Polarity";
            // 
            // Hardware_Controlled
            // 
            this.Hardware_Controlled.AutoSize = true;
            this.Hardware_Controlled.Location = new System.Drawing.Point(6, 66);
            this.Hardware_Controlled.Name = "Hardware_Controlled";
            this.Hardware_Controlled.Size = new System.Drawing.Size(121, 17);
            this.Hardware_Controlled.TabIndex = 2;
            this.Hardware_Controlled.TabStop = true;
            this.Hardware_Controlled.Text = "Hardware Controlled";
            this.Hardware_Controlled.UseVisualStyleBackColor = true;
            this.Hardware_Controlled.CheckedChanged += new System.EventHandler(this.Hardware_Controlled_CheckedChanged);
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
            this.PolarityNegative.CheckedChanged += new System.EventHandler(this.PolarityNegative_CheckChanged);
            // 
            // PolarityPositive
            // 
            this.PolarityPositive.AutoSize = true;
            this.PolarityPositive.Location = new System.Drawing.Point(7, 20);
            this.PolarityPositive.Name = "PolarityPositive";
            this.PolarityPositive.Size = new System.Drawing.Size(105, 17);
            this.PolarityPositive.TabIndex = 0;
            this.PolarityPositive.TabStop = true;
            this.PolarityPositive.Text = "Positive (Source)";
            this.PolarityPositive.UseVisualStyleBackColor = true;
            this.PolarityPositive.CheckedChanged += new System.EventHandler(this.PolarityPositive_CheckChanged);
            // 
            // OutPutBox
            // 
            this.OutPutBox.Controls.Add(this.lbl2mA);
            this.OutPutBox.Controls.Add(this.lbl255A);
            this.OutPutBox.Controls.Add(this.lbl32A);
            this.OutPutBox.Controls.Add(this.OutPutRange3);
            this.OutPutBox.Controls.Add(this.OutPutRange2);
            this.OutPutBox.Controls.Add(this.OutPutRange1);
            this.OutPutBox.Location = new System.Drawing.Point(7, 134);
            this.OutPutBox.Name = "OutPutBox";
            this.OutPutBox.Size = new System.Drawing.Size(186, 101);
            this.OutPutBox.TabIndex = 2;
            this.OutPutBox.TabStop = false;
            this.OutPutBox.Text = "Range";
            // 
            // lbl2mA
            // 
            this.lbl2mA.AutoSize = true;
            this.lbl2mA.Location = new System.Drawing.Point(92, 77);
            this.lbl2mA.Name = "lbl2mA";
            this.lbl2mA.Size = new System.Drawing.Size(54, 13);
            this.lbl2mA.TabIndex = 5;
            this.lbl2mA.Text = " (8 uA/bit)";
            // 
            // lbl255A
            // 
            this.lbl255A.AutoSize = true;
            this.lbl255A.Location = new System.Drawing.Point(92, 50);
            this.lbl255A.Name = "lbl255A";
            this.lbl255A.Size = new System.Drawing.Size(54, 13);
            this.lbl255A.TabIndex = 4;
            this.lbl255A.Text = " (1 uA/bit)";
            // 
            // lbl32A
            // 
            this.lbl32A.AutoSize = true;
            this.lbl32A.Location = new System.Drawing.Point(92, 21);
            this.lbl32A.Name = "lbl32A";
            this.lbl32A.Size = new System.Drawing.Size(65, 13);
            this.lbl32A.TabIndex = 3;
            this.lbl32A.Text = " (1/8 uA/bit)";
            // 
            // OutPutRange3
            // 
            this.OutPutRange3.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.OutPutRange3.AutoSize = true;
            this.OutPutRange3.Location = new System.Drawing.Point(6, 77);
            this.OutPutRange3.Name = "OutPutRange3";
            this.OutPutRange3.Size = new System.Drawing.Size(79, 17);
            this.OutPutRange3.TabIndex = 4;
            this.OutPutRange3.TabStop = true;
            this.OutPutRange3.Text = "0 - 2.04 mA";
            this.OutPutRange3.UseVisualStyleBackColor = true;
            this.OutPutRange3.CheckedChanged += new System.EventHandler(this.OutPutRange3_CheckChanged);
            // 
            // OutPutRange2
            // 
            this.OutPutRange2.AutoSize = true;
            this.OutPutRange2.Location = new System.Drawing.Point(7, 48);
            this.OutPutRange2.Name = "OutPutRange2";
            this.OutPutRange2.Size = new System.Drawing.Size(74, 17);
            this.OutPutRange2.TabIndex = 3;
            this.OutPutRange2.TabStop = true;
            this.OutPutRange2.Text = "0 - 255 uA";
            this.OutPutRange2.UseVisualStyleBackColor = true;
            this.OutPutRange2.CheckedChanged += new System.EventHandler(this.OutPutRange2_CheckChanged);
            // 
            // OutPutRange1
            // 
            this.OutPutRange1.AutoSize = true;
            this.OutPutRange1.Checked = true;
            this.OutPutRange1.Location = new System.Drawing.Point(7, 20);
            this.OutPutRange1.Name = "OutPutRange1";
            this.OutPutRange1.Size = new System.Drawing.Size(89, 17);
            this.OutPutRange1.TabIndex = 2;
            this.OutPutRange1.TabStop = true;
            this.OutPutRange1.Text = "0 - 31.875 uA";
            this.OutPutRange1.UseVisualStyleBackColor = true;
            this.OutPutRange1.CheckedChanged += new System.EventHandler(this.OutPutRange1_CheckChanged);
            // 
            // SpeedBox
            // 
            this.SpeedBox.Controls.Add(this.High);
            this.SpeedBox.Controls.Add(this.Slow);
            this.SpeedBox.Location = new System.Drawing.Point(212, 39);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(186, 69);
            this.SpeedBox.TabIndex = 4;
            this.SpeedBox.TabStop = false;
            this.SpeedBox.Text = "Speed";
            // 
            // High
            // 
            this.High.AutoSize = true;
            this.High.Location = new System.Drawing.Point(7, 43);
            this.High.Name = "High";
            this.High.Size = new System.Drawing.Size(81, 17);
            this.High.TabIndex = 8;
            this.High.TabStop = true;
            this.High.Text = "High Speed";
            this.High.UseVisualStyleBackColor = true;
            this.High.CheckedChanged += new System.EventHandler(this.High_CheckChanged);
            // 
            // Slow
            // 
            this.Slow.AutoSize = true;
            this.Slow.Location = new System.Drawing.Point(7, 20);
            this.Slow.Name = "Slow";
            this.Slow.Size = new System.Drawing.Size(79, 17);
            this.Slow.TabIndex = 7;
            this.Slow.TabStop = true;
            this.Slow.Text = "Low Speed";
            this.Slow.UseVisualStyleBackColor = true;
            this.Slow.CheckedChanged += new System.EventHandler(this.Slow_CheckChanged);
            // 
            // ValueBox
            // 
            this.ValueBox.Controls.Add(this.BytesValue);
            this.ValueBox.Controls.Add(this.AmpsValue);
            this.ValueBox.Controls.Add(this.lblNote);
            this.ValueBox.Controls.Add(this.lblHex);
            this.ValueBox.Controls.Add(this.lblA);
            this.ValueBox.Location = new System.Drawing.Point(7, 255);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.Size = new System.Drawing.Size(186, 133);
            this.ValueBox.TabIndex = 3;
            this.ValueBox.TabStop = false;
            this.ValueBox.Text = " Value";
            // 
            // BytesValue
            // 
            this.BytesValue.Location = new System.Drawing.Point(61, 57);
            this.BytesValue.MaxLength = 4;
            this.BytesValue.Name = "BytesValue";
            this.BytesValue.Size = new System.Drawing.Size(100, 20);
            this.BytesValue.TabIndex = 6;
            this.BytesValue.TextChanged += new System.EventHandler(this.BytesValue_TextChanged);
            this.BytesValue.Validating += new System.ComponentModel.CancelEventHandler(this.Bytes_Validating);
            // 
            // AmpsValue
            // 
            this.AmpsValue.Location = new System.Drawing.Point(61, 27);
            this.AmpsValue.MaxLength = 6;
            this.AmpsValue.Name = "AmpsValue";
            this.AmpsValue.Size = new System.Drawing.Size(100, 20);
            this.AmpsValue.TabIndex = 5;
            this.AmpsValue.TextChanged += new System.EventHandler(this.AmpsValue_TextChanged);
            this.AmpsValue.Validating += new System.ComponentModel.CancelEventHandler(this.AmpsValue_Validating);
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(3, 89);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(139, 26);
            this.lblNote.TabIndex = 8;
            this.lblNote.Text = "Note: Changing any value \r\n field recalculates the others";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHex
            // 
            this.lblHex.AutoSize = true;
            this.lblHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHex.Location = new System.Drawing.Point(3, 60);
            this.lblHex.Name = "lblHex";
            this.lblHex.Size = new System.Drawing.Size(52, 13);
            this.lblHex.TabIndex = 5;
            this.lblHex.Text = "8 bit Hex:";
            this.lblHex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblA.Location = new System.Drawing.Point(4, 30);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(23, 13);
            this.lblA.TabIndex = 3;
            this.lblA.Text = "uA:";
            this.lblA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // DataSource
            // 
            this.DataSource.Controls.Add(this.rButtonDataBus);
            this.DataSource.Controls.Add(this.rButtonDacBus);
            this.DataSource.Location = new System.Drawing.Point(212, 114);
            this.DataSource.Name = "DataSource";
            this.DataSource.Size = new System.Drawing.Size(186, 76);
            this.DataSource.TabIndex = 5;
            this.DataSource.TabStop = false;
            this.DataSource.Text = "Data Source";
            // 
            // rButtonDataBus
            // 
            this.rButtonDataBus.AutoSize = true;
            this.rButtonDataBus.Location = new System.Drawing.Point(7, 48);
            this.rButtonDataBus.Name = "rButtonDataBus";
            this.rButtonDataBus.Size = new System.Drawing.Size(139, 17);
            this.rButtonDataBus.TabIndex = 10;
            this.rButtonDataBus.TabStop = true;
            this.rButtonDataBus.Text = "CPU or DMA (Data Bus)";
            this.rButtonDataBus.UseVisualStyleBackColor = true;
            this.rButtonDataBus.CheckedChanged += new System.EventHandler(this.rButtonDataBus_CheckedChanged);
            // 
            // rButtonDacBus
            // 
            this.rButtonDacBus.AccessibleDescription = "";
            this.rButtonDacBus.AutoSize = true;
            this.rButtonDacBus.Location = new System.Drawing.Point(7, 20);
            this.rButtonDacBus.Name = "rButtonDacBus";
            this.rButtonDacBus.Size = new System.Drawing.Size(68, 17);
            this.rButtonDacBus.TabIndex = 9;
            this.rButtonDacBus.TabStop = true;
            this.rButtonDacBus.Text = "DAC Bus";
            this.rButtonDacBus.UseVisualStyleBackColor = true;
            this.rButtonDacBus.CheckedChanged += new System.EventHandler(this.rButtonDacBus_CheckedChanged);
            // 
            // StrobeMode
            // 
            this.StrobeMode.Controls.Add(this.rButtonRegister);
            this.StrobeMode.Controls.Add(this.rButtonExternal);
            this.StrobeMode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.StrobeMode.Location = new System.Drawing.Point(212, 196);
            this.StrobeMode.Name = "StrobeMode";
            this.StrobeMode.Size = new System.Drawing.Size(186, 78);
            this.StrobeMode.TabIndex = 6;
            this.StrobeMode.TabStop = false;
            this.StrobeMode.Text = "Strobe Mode";
            // 
            // rButtonRegister
            // 
            this.rButtonRegister.AutoSize = true;
            this.rButtonRegister.Location = new System.Drawing.Point(7, 43);
            this.rButtonRegister.Name = "rButtonRegister";
            this.rButtonRegister.Size = new System.Drawing.Size(92, 17);
            this.rButtonRegister.TabIndex = 12;
            this.rButtonRegister.TabStop = true;
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
            this.rButtonExternal.TabIndex = 11;
            this.rButtonExternal.TabStop = true;
            this.rButtonExternal.Text = "External";
            this.rButtonExternal.UseVisualStyleBackColor = true;
            this.rButtonExternal.CheckedChanged += new System.EventHandler(this.rButtonExternal_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "IDAC";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Hardware_Enable
            // 
            this.Hardware_Enable.AutoSize = true;
            this.Hardware_Enable.Location = new System.Drawing.Point(212, 284);
            this.Hardware_Enable.Name = "Hardware_Enable";
            this.Hardware_Enable.Size = new System.Drawing.Size(108, 17);
            this.Hardware_Enable.TabIndex = 12;
            this.Hardware_Enable.Text = "Hardware Enable";
            this.Hardware_Enable.UseVisualStyleBackColor = true;
            this.Hardware_Enable.CheckedChanged += new System.EventHandler(this.Hardware_Enable_CheckedChanged);
            // 
            // CyIDACControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Hardware_Enable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StrobeMode);
            this.Controls.Add(this.SpeedBox);
            this.Controls.Add(this.DataSource);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.OutPutBox);
            this.Controls.Add(this.PolarityBox);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "CyIDACControl";
            this.Size = new System.Drawing.Size(428, 391);
            this.Load += new System.EventHandler(this.CyIDACControl_Load);
            this.PolarityBox.ResumeLayout(false);
            this.PolarityBox.PerformLayout();
            this.OutPutBox.ResumeLayout(false);
            this.OutPutBox.PerformLayout();
            this.SpeedBox.ResumeLayout(false);
            this.SpeedBox.PerformLayout();
            this.ValueBox.ResumeLayout(false);
            this.ValueBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.DataSource.ResumeLayout(false);
            this.DataSource.PerformLayout();
            this.StrobeMode.ResumeLayout(false);
            this.StrobeMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox PolarityBox;
        private System.Windows.Forms.RadioButton PolarityNegative;
        private System.Windows.Forms.RadioButton PolarityPositive;
        private System.Windows.Forms.GroupBox OutPutBox;
        private System.Windows.Forms.RadioButton OutPutRange3;
        private System.Windows.Forms.RadioButton OutPutRange2;
        private System.Windows.Forms.RadioButton OutPutRange1;
        private System.Windows.Forms.Label lbl2mA;
        private System.Windows.Forms.Label lbl255A;
        private System.Windows.Forms.Label lbl32A;
        private System.Windows.Forms.GroupBox ValueBox;
        private System.Windows.Forms.Label lblHex;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.ErrorProvider ep_Errors;
       
        private System.Windows.Forms.GroupBox DataSource;
        private System.Windows.Forms.RadioButton rButtonDataBus;
        private System.Windows.Forms.RadioButton rButtonDacBus;
        private System.Windows.Forms.GroupBox StrobeMode;
        private System.Windows.Forms.RadioButton rButtonRegister;
        private System.Windows.Forms.RadioButton rButtonExternal;
        private System.Windows.Forms.GroupBox SpeedBox;
        private System.Windows.Forms.RadioButton High;
        private System.Windows.Forms.RadioButton Slow;
        
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox BytesValue;
        private System.Windows.Forms.TextBox AmpsValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton Hardware_Controlled;
        private System.Windows.Forms.CheckBox Hardware_Enable;
    }
}


//[] END OF FILE
