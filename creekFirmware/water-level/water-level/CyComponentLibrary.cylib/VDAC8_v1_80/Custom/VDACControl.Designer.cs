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
namespace VDAC8_v1_80
{
    partial class CyVDACControl
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
            this.VDAC = new System.Windows.Forms.Label();
            this.OutPutBox = new System.Windows.Forms.GroupBox();
            this.OutPutRange2 = new System.Windows.Forms.RadioButton();
            this.OutPutRange1 = new System.Windows.Forms.RadioButton();
            this.Speed = new System.Windows.Forms.Label();
            this.SpeedBox = new System.Windows.Forms.GroupBox();
            this.High = new System.Windows.Forms.RadioButton();
            this.Slow = new System.Windows.Forms.RadioButton();
            this.Value = new System.Windows.Forms.Label();
            this.ValueBox = new System.Windows.Forms.GroupBox();
            this.Bytes_UpDown = new System.Windows.Forms.TextBox();
            this.VoltageValueUpDown = new System.Windows.Forms.TextBox();
            this.bytes = new System.Windows.Forms.Label();
            this.Note = new System.Windows.Forms.Label();
            this.mVolts = new System.Windows.Forms.Label();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.DataSource = new System.Windows.Forms.GroupBox();
            this.rButtonDataBus = new System.Windows.Forms.RadioButton();
            this.rButtonDacBus = new System.Windows.Forms.RadioButton();
            this.StrobeMode = new System.Windows.Forms.GroupBox();
            this.rButtonRegister = new System.Windows.Forms.RadioButton();
            this.rButtonExternal = new System.Windows.Forms.RadioButton();
            this.OutPutBox.SuspendLayout();
            this.SpeedBox.SuspendLayout();
            this.ValueBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).BeginInit();
            this.DataSource.SuspendLayout();
            this.StrobeMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // VDAC
            // 
            this.VDAC.AutoSize = true;
            this.VDAC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VDAC.Location = new System.Drawing.Point(4, 4);
            this.VDAC.Name = "VDAC";
            this.VDAC.Size = new System.Drawing.Size(45, 17);
            this.VDAC.TabIndex = 0;
            this.VDAC.Text = "VDAC";
            // 
            // OutPutBox
            // 
            this.OutPutBox.Controls.Add(this.OutPutRange2);
            this.OutPutBox.Controls.Add(this.OutPutRange1);
            this.OutPutBox.Location = new System.Drawing.Point(7, 37);
            this.OutPutBox.Name = "OutPutBox";
            this.OutPutBox.Size = new System.Drawing.Size(183, 64);
            this.OutPutBox.TabIndex = 1;
            this.OutPutBox.TabStop = false;
            this.OutPutBox.Text = "Range";
            // 
            // OutPutRange2
            // 
            this.OutPutRange2.AutoSize = true;
            this.OutPutRange2.Location = new System.Drawing.Point(6, 41);
            this.OutPutRange2.Name = "OutPutRange2";
            this.OutPutRange2.Size = new System.Drawing.Size(138, 17);
            this.OutPutRange2.TabIndex = 1;
            this.OutPutRange2.TabStop = true;
            this.OutPutRange2.Text = "0 - 4.080 V (16 mV / bit)";
            this.OutPutRange2.UseVisualStyleBackColor = true;
            this.OutPutRange2.CheckedChanged += new System.EventHandler(this.OutPutRange2_CheckChanged);
            // 
            // OutPutRange1
            // 
            this.OutPutRange1.AutoSize = true;
            this.OutPutRange1.Location = new System.Drawing.Point(6, 18);
            this.OutPutRange1.Name = "OutPutRange1";
            this.OutPutRange1.Size = new System.Drawing.Size(132, 17);
            this.OutPutRange1.TabIndex = 0;
            this.OutPutRange1.TabStop = true;
            this.OutPutRange1.Text = "0 - 1.020 V (4 mV / bit)";
            this.OutPutRange1.UseVisualStyleBackColor = true;
            this.OutPutRange1.CheckedChanged += new System.EventHandler(this.OutPutRange1_CheckChanged);
            // 
            // Speed
            // 
            this.Speed.AutoSize = true;
            this.Speed.Location = new System.Drawing.Point(7, 139);
            this.Speed.Name = "Speed";
            this.Speed.Size = new System.Drawing.Size(0, 13);
            this.Speed.TabIndex = 3;
            // 
            // SpeedBox
            // 
            this.SpeedBox.Controls.Add(this.High);
            this.SpeedBox.Controls.Add(this.Slow);
            this.SpeedBox.Location = new System.Drawing.Point(204, 37);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(157, 64);
            this.SpeedBox.TabIndex = 4;
            this.SpeedBox.TabStop = false;
            this.SpeedBox.Text = "Speed";
            // 
            // High
            // 
            this.High.AutoSize = true;
            this.High.Location = new System.Drawing.Point(16, 41);
            this.High.Name = "High";
            this.High.Size = new System.Drawing.Size(81, 17);
            this.High.TabIndex = 1;
            this.High.Text = "High Speed";
            this.High.UseVisualStyleBackColor = true;
            this.High.CheckedChanged += new System.EventHandler(this.Fast_CheckChanged);
            // 
            // Slow
            // 
            this.Slow.AutoSize = true;
            this.Slow.Location = new System.Drawing.Point(16, 18);
            this.Slow.Name = "Slow";
            this.Slow.Size = new System.Drawing.Size(82, 17);
            this.Slow.TabIndex = 0;
            this.Slow.Text = "Slow Speed";
            this.Slow.UseVisualStyleBackColor = true;
            this.Slow.CheckedChanged += new System.EventHandler(this.Slow_CheckChanged);
            // 
            // Value
            // 
            this.Value.AutoSize = true;
            this.Value.Location = new System.Drawing.Point(7, 225);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(0, 13);
            this.Value.TabIndex = 5;
            // 
            // ValueBox
            // 
            this.ValueBox.Controls.Add(this.Bytes_UpDown);
            this.ValueBox.Controls.Add(this.VoltageValueUpDown);
            this.ValueBox.Controls.Add(this.bytes);
            this.ValueBox.Controls.Add(this.Note);
            this.ValueBox.Controls.Add(this.mVolts);
            this.ValueBox.Location = new System.Drawing.Point(7, 117);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.Size = new System.Drawing.Size(183, 147);
            this.ValueBox.TabIndex = 6;
            this.ValueBox.TabStop = false;
            this.ValueBox.Text = "Value";
            // 
            // Bytes_UpDown
            // 
            this.Bytes_UpDown.Location = new System.Drawing.Point(61, 59);
            this.Bytes_UpDown.MaxLength = 4;
            this.Bytes_UpDown.Name = "Bytes_UpDown";
            this.Bytes_UpDown.Size = new System.Drawing.Size(100, 20);
            this.Bytes_UpDown.TabIndex = 9;
            this.Bytes_UpDown.TextChanged += new System.EventHandler(this.Bytes_UpDown_ValueChanged);
            this.Bytes_UpDown.Validating += new System.ComponentModel.CancelEventHandler(this.Bytes_Validating);
            // 
            // VoltageValueUpDown
            // 
            this.VoltageValueUpDown.Location = new System.Drawing.Point(61, 28);
            this.VoltageValueUpDown.MaxLength = 4;
            this.VoltageValueUpDown.Name = "VoltageValueUpDown";
            this.VoltageValueUpDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.VoltageValueUpDown.Size = new System.Drawing.Size(100, 20);
            this.VoltageValueUpDown.TabIndex = 8;
            this.VoltageValueUpDown.TextChanged += new System.EventHandler(this.Voltage_ValueChanged);
            this.VoltageValueUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.Voltage_Validating);
            // 
            // bytes
            // 
            this.bytes.AutoSize = true;
            this.bytes.Location = new System.Drawing.Point(6, 62);
            this.bytes.Name = "bytes";
            this.bytes.Size = new System.Drawing.Size(52, 13);
            this.bytes.TabIndex = 3;
            this.bytes.Text = "8 bit Hex:";
            this.bytes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Note
            // 
            this.Note.Location = new System.Drawing.Point(6, 104);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(162, 39);
            this.Note.TabIndex = 7;
            this.Note.Text = "Note: Changing any value field recalculates the other";
            // 
            // mVolts
            // 
            this.mVolts.AutoSize = true;
            this.mVolts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mVolts.Location = new System.Drawing.Point(6, 31);
            this.mVolts.Name = "mVolts";
            this.mVolts.Size = new System.Drawing.Size(25, 13);
            this.mVolts.TabIndex = 1;
            this.mVolts.Text = "mV:";
            this.mVolts.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // DataSource
            // 
            this.DataSource.Controls.Add(this.rButtonDataBus);
            this.DataSource.Controls.Add(this.rButtonDacBus);
            this.DataSource.Location = new System.Drawing.Point(204, 117);
            this.DataSource.Name = "DataSource";
            this.DataSource.Size = new System.Drawing.Size(157, 64);
            this.DataSource.TabIndex = 8;
            this.DataSource.TabStop = false;
            this.DataSource.Text = "Data Source";
            // 
            // rButtonDataBus
            // 
            this.rButtonDataBus.AutoSize = true;
            this.rButtonDataBus.Location = new System.Drawing.Point(16, 41);
            this.rButtonDataBus.Name = "rButtonDataBus";
            this.rButtonDataBus.Size = new System.Drawing.Size(139, 17);
            this.rButtonDataBus.TabIndex = 2;
            this.rButtonDataBus.Text = "CPU or DMA (Data Bus)";
            this.rButtonDataBus.UseVisualStyleBackColor = true;
            this.rButtonDataBus.CheckedChanged += new System.EventHandler(this.rButtonDataBus_CheckedChanged);
            // 
            // rButtonDacBus
            // 
            this.rButtonDacBus.AutoSize = true;
            this.rButtonDacBus.Location = new System.Drawing.Point(16, 18);
            this.rButtonDacBus.Name = "rButtonDacBus";
            this.rButtonDacBus.Size = new System.Drawing.Size(68, 17);
            this.rButtonDacBus.TabIndex = 1;
            this.rButtonDacBus.Text = "DAC Bus";
            this.rButtonDacBus.UseVisualStyleBackColor = true;
            this.rButtonDacBus.CheckedChanged += new System.EventHandler(this.rButtonDacBus_CheckedChanged);
            // 
            // StrobeMode
            // 
            this.StrobeMode.Controls.Add(this.rButtonRegister);
            this.StrobeMode.Controls.Add(this.rButtonExternal);
            this.StrobeMode.Location = new System.Drawing.Point(204, 202);
            this.StrobeMode.Name = "StrobeMode";
            this.StrobeMode.Size = new System.Drawing.Size(157, 62);
            this.StrobeMode.TabIndex = 9;
            this.StrobeMode.TabStop = false;
            this.StrobeMode.Text = "Strobe Mode";
            // 
            // rButtonRegister
            // 
            this.rButtonRegister.AutoSize = true;
            this.rButtonRegister.Location = new System.Drawing.Point(16, 38);
            this.rButtonRegister.Name = "rButtonRegister";
            this.rButtonRegister.Size = new System.Drawing.Size(92, 17);
            this.rButtonRegister.TabIndex = 3;
            this.rButtonRegister.Text = "Register Write";
            this.rButtonRegister.UseVisualStyleBackColor = true;
            this.rButtonRegister.CheckedChanged += new System.EventHandler(this.rButtonRegister_CheckedChanged);
            // 
            // rButtonExternal
            // 
            this.rButtonExternal.AutoSize = true;
            this.rButtonExternal.Location = new System.Drawing.Point(16, 15);
            this.rButtonExternal.Name = "rButtonExternal";
            this.rButtonExternal.Size = new System.Drawing.Size(63, 17);
            this.rButtonExternal.TabIndex = 2;
            this.rButtonExternal.Text = "External";
            this.rButtonExternal.UseVisualStyleBackColor = true;
            this.rButtonExternal.CheckedChanged += new System.EventHandler(this.rButtonExternal_CheckedChanged);
            // 
            // CyVDACControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StrobeMode);
            this.Controls.Add(this.DataSource);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.Value);
            this.Controls.Add(this.SpeedBox);
            this.Controls.Add(this.Speed);
            this.Controls.Add(this.OutPutBox);
            this.Controls.Add(this.VDAC);
            this.Name = "CyVDACControl";
            this.Size = new System.Drawing.Size(366, 284);
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

        private System.Windows.Forms.Label VDAC;
        private System.Windows.Forms.GroupBox OutPutBox;
        private System.Windows.Forms.RadioButton OutPutRange1;
        private System.Windows.Forms.RadioButton OutPutRange2;
        private System.Windows.Forms.Label Speed;
        private System.Windows.Forms.GroupBox SpeedBox;
        private System.Windows.Forms.RadioButton High;
        private System.Windows.Forms.RadioButton Slow;
        private System.Windows.Forms.Label Value;
        private System.Windows.Forms.GroupBox ValueBox;
        private System.Windows.Forms.Label mVolts;
        private System.Windows.Forms.ErrorProvider ep_Errors;
        private System.Windows.Forms.Label bytes;
        private System.Windows.Forms.Label Note;
        private System.Windows.Forms.GroupBox DataSource;
        private System.Windows.Forms.GroupBox StrobeMode;
        private System.Windows.Forms.RadioButton rButtonDataBus;
        private System.Windows.Forms.RadioButton rButtonDacBus;
        private System.Windows.Forms.RadioButton rButtonRegister;
        private System.Windows.Forms.RadioButton rButtonExternal;
        private System.Windows.Forms.TextBox VoltageValueUpDown;
        private System.Windows.Forms.TextBox Bytes_UpDown;
    }
}


//[] END OF FILE
