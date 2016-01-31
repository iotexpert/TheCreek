namespace USBFS_v1_60
{
    partial class CyReportUnit
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
            this.comboBoxSystem = new System.Windows.Forms.ComboBox();
            this.comboBoxQuickUnit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelU1 = new System.Windows.Forms.Label();
            this.numUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelU11 = new System.Windows.Forms.Label();
            this.labelU22 = new System.Windows.Forms.Label();
            this.numUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.labelU2 = new System.Windows.Forms.Label();
            this.labelU33 = new System.Windows.Forms.Label();
            this.numUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.labelU3 = new System.Windows.Forms.Label();
            this.labelU44 = new System.Windows.Forms.Label();
            this.numUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.labelU4 = new System.Windows.Forms.Label();
            this.labelU55 = new System.Windows.Forms.Label();
            this.numUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.label1U5 = new System.Windows.Forms.Label();
            this.labelU66 = new System.Windows.Forms.Label();
            this.numUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.label1U6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown6)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxSystem
            // 
            this.comboBoxSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSystem.FormattingEnabled = true;
            this.comboBoxSystem.Items.AddRange(new object[] {
            "None",
            "SI Linear",
            "SI Rotation",
            "English Linear",
            "English Rotation"});
            this.comboBoxSystem.Location = new System.Drawing.Point(65, 10);
            this.comboBoxSystem.Name = "comboBoxSystem";
            this.comboBoxSystem.Size = new System.Drawing.Size(132, 21);
            this.comboBoxSystem.TabIndex = 0;
            this.comboBoxSystem.SelectedIndexChanged += new System.EventHandler(this.comboBoxSystem_SelectedIndexChanged);
            // 
            // comboBoxQuickUnit
            // 
            this.comboBoxQuickUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxQuickUnit.FormattingEnabled = true;
            this.comboBoxQuickUnit.Location = new System.Drawing.Point(65, 35);
            this.comboBoxQuickUnit.Name = "comboBoxQuickUnit";
            this.comboBoxQuickUnit.Size = new System.Drawing.Size(132, 21);
            this.comboBoxQuickUnit.TabIndex = 1;
            this.comboBoxQuickUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxQuickUnit_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "System";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Quick Unit";
            // 
            // labelU1
            // 
            this.labelU1.AutoSize = true;
            this.labelU1.Location = new System.Drawing.Point(2, 16);
            this.labelU1.Name = "labelU1";
            this.labelU1.Size = new System.Drawing.Size(40, 13);
            this.labelU1.TabIndex = 4;
            this.labelU1.Text = "Length";
            // 
            // numUpDown1
            // 
            this.numUpDown1.Location = new System.Drawing.Point(59, 14);
            this.numUpDown1.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numUpDown1.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.numUpDown1.Name = "numUpDown1";
            this.numUpDown1.Size = new System.Drawing.Size(74, 20);
            this.numUpDown1.TabIndex = 5;
            this.numUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.numUpDown1.Validated += new System.EventHandler(this.numUpDown1_Validated);
            // 
            // labelU11
            // 
            this.labelU11.AutoSize = true;
            this.labelU11.Location = new System.Drawing.Point(139, 16);
            this.labelU11.Name = "labelU11";
            this.labelU11.Size = new System.Drawing.Size(21, 13);
            this.labelU11.TabIndex = 6;
            this.labelU11.Text = "cm";
            // 
            // labelU22
            // 
            this.labelU22.AutoSize = true;
            this.labelU22.Location = new System.Drawing.Point(139, 42);
            this.labelU22.Name = "labelU22";
            this.labelU22.Size = new System.Drawing.Size(13, 13);
            this.labelU22.TabIndex = 9;
            this.labelU22.Text = "g";
            // 
            // numUpDown2
            // 
            this.numUpDown2.Location = new System.Drawing.Point(59, 40);
            this.numUpDown2.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numUpDown2.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.numUpDown2.Name = "numUpDown2";
            this.numUpDown2.Size = new System.Drawing.Size(74, 20);
            this.numUpDown2.TabIndex = 8;
            this.numUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.numUpDown2.Validated += new System.EventHandler(this.numUpDown1_Validated);
            // 
            // labelU2
            // 
            this.labelU2.AutoSize = true;
            this.labelU2.Location = new System.Drawing.Point(2, 42);
            this.labelU2.Name = "labelU2";
            this.labelU2.Size = new System.Drawing.Size(32, 13);
            this.labelU2.TabIndex = 7;
            this.labelU2.Text = "Mass";
            // 
            // labelU33
            // 
            this.labelU33.AutoSize = true;
            this.labelU33.Location = new System.Drawing.Point(139, 68);
            this.labelU33.Name = "labelU33";
            this.labelU33.Size = new System.Drawing.Size(24, 13);
            this.labelU33.TabIndex = 12;
            this.labelU33.Text = "sec";
            // 
            // numUpDown3
            // 
            this.numUpDown3.Location = new System.Drawing.Point(59, 66);
            this.numUpDown3.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numUpDown3.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.numUpDown3.Name = "numUpDown3";
            this.numUpDown3.Size = new System.Drawing.Size(74, 20);
            this.numUpDown3.TabIndex = 11;
            this.numUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.numUpDown3.Validated += new System.EventHandler(this.numUpDown1_Validated);
            // 
            // labelU3
            // 
            this.labelU3.AutoSize = true;
            this.labelU3.Location = new System.Drawing.Point(2, 68);
            this.labelU3.Name = "labelU3";
            this.labelU3.Size = new System.Drawing.Size(30, 13);
            this.labelU3.TabIndex = 10;
            this.labelU3.Text = "Time";
            // 
            // labelU44
            // 
            this.labelU44.AutoSize = true;
            this.labelU44.Location = new System.Drawing.Point(139, 94);
            this.labelU44.Name = "labelU44";
            this.labelU44.Size = new System.Drawing.Size(14, 13);
            this.labelU44.TabIndex = 15;
            this.labelU44.Text = "K";
            // 
            // numUpDown4
            // 
            this.numUpDown4.Location = new System.Drawing.Point(59, 92);
            this.numUpDown4.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numUpDown4.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.numUpDown4.Name = "numUpDown4";
            this.numUpDown4.Size = new System.Drawing.Size(74, 20);
            this.numUpDown4.TabIndex = 14;
            this.numUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.numUpDown4.Validated += new System.EventHandler(this.numUpDown1_Validated);
            // 
            // labelU4
            // 
            this.labelU4.AutoSize = true;
            this.labelU4.Location = new System.Drawing.Point(2, 94);
            this.labelU4.Name = "labelU4";
            this.labelU4.Size = new System.Drawing.Size(34, 13);
            this.labelU4.TabIndex = 13;
            this.labelU4.Text = "Temp";
            // 
            // labelU55
            // 
            this.labelU55.AutoSize = true;
            this.labelU55.Location = new System.Drawing.Point(139, 120);
            this.labelU55.Name = "labelU55";
            this.labelU55.Size = new System.Drawing.Size(14, 13);
            this.labelU55.TabIndex = 18;
            this.labelU55.Text = "A";
            // 
            // numUpDown5
            // 
            this.numUpDown5.Location = new System.Drawing.Point(59, 118);
            this.numUpDown5.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numUpDown5.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.numUpDown5.Name = "numUpDown5";
            this.numUpDown5.Size = new System.Drawing.Size(74, 20);
            this.numUpDown5.TabIndex = 17;
            this.numUpDown5.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.numUpDown5.Validated += new System.EventHandler(this.numUpDown1_Validated);
            // 
            // label1U5
            // 
            this.label1U5.AutoSize = true;
            this.label1U5.Location = new System.Drawing.Point(2, 120);
            this.label1U5.Name = "label1U5";
            this.label1U5.Size = new System.Drawing.Size(41, 13);
            this.label1U5.TabIndex = 16;
            this.label1U5.Text = "Current";
            // 
            // labelU66
            // 
            this.labelU66.AutoSize = true;
            this.labelU66.Location = new System.Drawing.Point(139, 146);
            this.labelU66.Name = "labelU66";
            this.labelU66.Size = new System.Drawing.Size(19, 13);
            this.labelU66.TabIndex = 21;
            this.labelU66.Text = "cd";
            // 
            // numUpDown6
            // 
            this.numUpDown6.Location = new System.Drawing.Point(59, 144);
            this.numUpDown6.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numUpDown6.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.numUpDown6.Name = "numUpDown6";
            this.numUpDown6.Size = new System.Drawing.Size(74, 20);
            this.numUpDown6.TabIndex = 20;
            this.numUpDown6.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            this.numUpDown6.Validated += new System.EventHandler(this.numUpDown1_Validated);
            // 
            // label1U6
            // 
            this.label1U6.AutoSize = true;
            this.label1U6.Location = new System.Drawing.Point(2, 146);
            this.label1U6.Name = "label1U6";
            this.label1U6.Size = new System.Drawing.Size(42, 13);
            this.label1U6.TabIndex = 19;
            this.label1U6.Text = "Lum Int";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelU66);
            this.groupBox1.Controls.Add(this.numUpDown6);
            this.groupBox1.Controls.Add(this.label1U6);
            this.groupBox1.Controls.Add(this.labelU55);
            this.groupBox1.Controls.Add(this.numUpDown5);
            this.groupBox1.Controls.Add(this.label1U5);
            this.groupBox1.Controls.Add(this.labelU44);
            this.groupBox1.Controls.Add(this.numUpDown4);
            this.groupBox1.Controls.Add(this.labelU4);
            this.groupBox1.Controls.Add(this.labelU33);
            this.groupBox1.Controls.Add(this.numUpDown3);
            this.groupBox1.Controls.Add(this.labelU3);
            this.groupBox1.Controls.Add(this.labelU22);
            this.groupBox1.Controls.Add(this.numUpDown2);
            this.groupBox1.Controls.Add(this.labelU2);
            this.groupBox1.Controls.Add(this.labelU11);
            this.groupBox1.Controls.Add(this.numUpDown1);
            this.groupBox1.Controls.Add(this.labelU1);
            this.groupBox1.Location = new System.Drawing.Point(6, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 171);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboBoxQuickUnit);
            this.groupBox2.Controls.Add(this.comboBoxSystem);
            this.groupBox2.Location = new System.Drawing.Point(6, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(206, 62);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            // 
            // CyReportUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CyReportUnit";
            this.Size = new System.Drawing.Size(215, 242);
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown6)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxSystem;
        private System.Windows.Forms.ComboBox comboBoxQuickUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelU1;
        private System.Windows.Forms.NumericUpDown numUpDown1;
        private System.Windows.Forms.Label labelU11;
        private System.Windows.Forms.Label labelU22;
        private System.Windows.Forms.NumericUpDown numUpDown2;
        private System.Windows.Forms.Label labelU2;
        private System.Windows.Forms.Label labelU33;
        private System.Windows.Forms.NumericUpDown numUpDown3;
        private System.Windows.Forms.Label labelU3;
        private System.Windows.Forms.Label labelU44;
        private System.Windows.Forms.NumericUpDown numUpDown4;
        private System.Windows.Forms.Label labelU4;
        private System.Windows.Forms.Label labelU55;
        private System.Windows.Forms.NumericUpDown numUpDown5;
        private System.Windows.Forms.Label label1U5;
        private System.Windows.Forms.Label labelU66;
        private System.Windows.Forms.NumericUpDown numUpDown6;
        private System.Windows.Forms.Label label1U6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
