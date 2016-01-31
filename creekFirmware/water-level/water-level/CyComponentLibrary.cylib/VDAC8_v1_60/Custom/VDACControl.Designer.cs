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
namespace VDAC8_v1_60
{
    partial class VDACControl
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
            this.Fast = new System.Windows.Forms.RadioButton();
            this.Slow = new System.Windows.Forms.RadioButton();
            this.Value = new System.Windows.Forms.Label();
            this.ValueBox = new System.Windows.Forms.GroupBox();
            this.bytes = new System.Windows.Forms.Label();
            this.Bytes_UpDown = new VDAC8_v1_60.CyNumericUpDown();
            this.mVolts = new System.Windows.Forms.Label();
            this.VoltageValueUpDown = new VDAC8_v1_60.CyNumericUpDown();
            this.ep_Errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.Note = new System.Windows.Forms.Label();
            this.DataSource = new System.Windows.Forms.GroupBox();
            this.m_cbDataSource = new System.Windows.Forms.ComboBox();
            this.StrobeMode = new System.Windows.Forms.GroupBox();
            this.m_cb_StrobeMode = new System.Windows.Forms.ComboBox();
            this.OutPutBox.SuspendLayout();
            this.SpeedBox.SuspendLayout();
            this.ValueBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bytes_UpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoltageValueUpDown)).BeginInit();
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
            this.OutPutBox.Location = new System.Drawing.Point(7, 28);
            this.OutPutBox.Name = "OutPutBox";
            this.OutPutBox.Size = new System.Drawing.Size(189, 76);
            this.OutPutBox.TabIndex = 1;
            this.OutPutBox.TabStop = false;
            this.OutPutBox.Text = "OutPut Range";
            // 
            // OutPutRange2
            // 
            this.OutPutRange2.AutoSize = true;
            this.OutPutRange2.Location = new System.Drawing.Point(6, 48);
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
            this.OutPutRange1.Location = new System.Drawing.Point(6, 19);
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
            this.SpeedBox.Controls.Add(this.Fast);
            this.SpeedBox.Controls.Add(this.Slow);
            this.SpeedBox.Location = new System.Drawing.Point(204, 28);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(138, 76);
            this.SpeedBox.TabIndex = 4;
            this.SpeedBox.TabStop = false;
            this.SpeedBox.Text = "Speed";
            // 
            // Fast
            // 
            this.Fast.AutoSize = true;
            this.Fast.Location = new System.Drawing.Point(77, 31);
            this.Fast.Name = "Fast";
            this.Fast.Size = new System.Drawing.Size(45, 17);
            this.Fast.TabIndex = 1;
            this.Fast.Text = "Fast";
            this.Fast.UseVisualStyleBackColor = true;
            this.Fast.CheckedChanged += new System.EventHandler(this.Fast_CheckedAhanged);
            // 
            // Slow
            // 
            this.Slow.AutoSize = true;
            this.Slow.Location = new System.Drawing.Point(15, 31);
            this.Slow.Name = "Slow";
            this.Slow.Size = new System.Drawing.Size(48, 17);
            this.Slow.TabIndex = 0;
            this.Slow.Text = "Slow";
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
            this.ValueBox.Controls.Add(this.bytes);
            this.ValueBox.Controls.Add(this.Bytes_UpDown);
            this.ValueBox.Controls.Add(this.Note);
            this.ValueBox.Controls.Add(this.mVolts);
            this.ValueBox.Controls.Add(this.VoltageValueUpDown);
            this.ValueBox.Location = new System.Drawing.Point(7, 117);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.Size = new System.Drawing.Size(189, 147);
            this.ValueBox.TabIndex = 6;
            this.ValueBox.TabStop = false;
            this.ValueBox.Text = "Value";
            // 
            // bytes
            // 
            this.bytes.AutoSize = true;
            this.bytes.Location = new System.Drawing.Point(143, 69);
            this.bytes.Name = "bytes";
            this.bytes.Size = new System.Drawing.Size(32, 13);
            this.bytes.TabIndex = 3;
            this.bytes.Text = "bytes";
            // 
            // Bytes_UpDown
            // 
            this.Bytes_UpDown.Cursor = System.Windows.Forms.Cursors.Default;
            this.Bytes_UpDown.Hexadecimal = true;
            this.Bytes_UpDown.Location = new System.Drawing.Point(7, 63);
            this.Bytes_UpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Bytes_UpDown.Name = "Bytes_UpDown";
            this.Bytes_UpDown.Size = new System.Drawing.Size(108, 20);
            this.Bytes_UpDown.TabIndex = 2;
            this.Bytes_UpDown.ValueChanged += new System.EventHandler(this.Bytes_UpDown_ValueChanged);
            this.Bytes_UpDown.Validating += new System.ComponentModel.CancelEventHandler(this.Bytes_Validating);
            this.Bytes_UpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Byte_UpDown_KeyUp);
            // 
            // mVolts
            // 
            this.mVolts.AutoSize = true;
            this.mVolts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mVolts.Location = new System.Drawing.Point(143, 27);
            this.mVolts.Name = "mVolts";
            this.mVolts.Size = new System.Drawing.Size(38, 13);
            this.mVolts.TabIndex = 1;
            this.mVolts.Text = "mVolts";
            // 
            // VoltageValueUpDown
            // 
            this.VoltageValueUpDown.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.VoltageValueUpDown.Location = new System.Drawing.Point(7, 20);
            this.VoltageValueUpDown.Maximum = new decimal(new int[] {
            4080,
            0,
            0,
            0});
            this.VoltageValueUpDown.Name = "VoltageValueUpDown";
            this.VoltageValueUpDown.Size = new System.Drawing.Size(108, 20);
            this.VoltageValueUpDown.TabIndex = 0;
            this.VoltageValueUpDown.ValueChanged += new System.EventHandler(this.Voltage_ValueChanged);
            this.VoltageValueUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.Voltage_Validating);
            this.VoltageValueUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Voltage_UpDown_KeyUp);
            // 
            // ep_Errors
            // 
            this.ep_Errors.ContainerControl = this;
            // 
            // Note
            // 
            this.Note.Location = new System.Drawing.Point(6, 104);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(172, 40);
            this.Note.TabIndex = 7;
            this.Note.Text = "Note: Changing any value field recalculates the others ";
            // 
            // DataSource
            // 
            this.DataSource.Controls.Add(this.m_cbDataSource);
            this.DataSource.Location = new System.Drawing.Point(204, 117);
            this.DataSource.Name = "DataSource";
            this.DataSource.Size = new System.Drawing.Size(138, 60);
            this.DataSource.TabIndex = 8;
            this.DataSource.TabStop = false;
            this.DataSource.Text = "DataSource";
            // 
            // m_cbDataSource
            // 
            this.m_cbDataSource.FormattingEnabled = true;
            this.m_cbDataSource.Location = new System.Drawing.Point(5, 19);
            this.m_cbDataSource.Name = "m_cbDataSource";
            this.m_cbDataSource.Size = new System.Drawing.Size(121, 21);
            this.m_cbDataSource.TabIndex = 0;
            this.m_cbDataSource.SelectedIndexChanged += new System.EventHandler(this.m_cbDataSource_SelectedIndexChanged);
            // 
            // StrobeMode
            // 
            this.StrobeMode.Controls.Add(this.m_cb_StrobeMode);
            this.StrobeMode.Location = new System.Drawing.Point(202, 200);
            this.StrobeMode.Name = "StrobeMode";
            this.StrobeMode.Size = new System.Drawing.Size(138, 64);
            this.StrobeMode.TabIndex = 9;
            this.StrobeMode.TabStop = false;
            this.StrobeMode.Text = "StrobeMode";
            // 
            // m_cb_StrobeMode
            // 
            this.m_cb_StrobeMode.FormattingEnabled = true;
            this.m_cb_StrobeMode.Location = new System.Drawing.Point(5, 23);
            this.m_cb_StrobeMode.Name = "m_cb_StrobeMode";
            this.m_cb_StrobeMode.Size = new System.Drawing.Size(121, 21);
            this.m_cb_StrobeMode.TabIndex = 0;
            this.m_cb_StrobeMode.SelectedIndexChanged += new System.EventHandler(this.m_cb_StrobeMode_SelectedIndexChanged);
            // 
            // VDACControl
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
            this.Name = "VDACControl";
            this.Size = new System.Drawing.Size(358, 287);
            this.OutPutBox.ResumeLayout(false);
            this.OutPutBox.PerformLayout();
            this.SpeedBox.ResumeLayout(false);
            this.SpeedBox.PerformLayout();
            this.ValueBox.ResumeLayout(false);
            this.ValueBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bytes_UpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VoltageValueUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ep_Errors)).EndInit();
            this.DataSource.ResumeLayout(false);
            this.StrobeMode.ResumeLayout(false);
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
        private System.Windows.Forms.RadioButton Fast;
        private System.Windows.Forms.RadioButton Slow;
        private System.Windows.Forms.Label Value;
        private System.Windows.Forms.GroupBox ValueBox;
        private System.Windows.Forms.Label mVolts;
        private CyNumericUpDown VoltageValueUpDown;
        private System.Windows.Forms.ErrorProvider ep_Errors;
        private System.Windows.Forms.Label bytes;
        private CyNumericUpDown Bytes_UpDown;
        private System.Windows.Forms.Label Note;
        private System.Windows.Forms.GroupBox DataSource;
        private System.Windows.Forms.ComboBox m_cbDataSource;
        private System.Windows.Forms.GroupBox StrobeMode;
        private System.Windows.Forms.ComboBox m_cb_StrobeMode;
    }
}
//[] END OF FILE
