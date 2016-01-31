namespace VoltageFaultDetector_v1_0
{
    partial class CyBasicTab
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
            this.label1 = new System.Windows.Forms.Label();
            this.numNumVoltages = new System.Windows.Forms.NumericUpDown();
            this.labelCompareType = new System.Windows.Forms.Label();
            this.comboBoxCompareType = new System.Windows.Forms.ComboBox();
            this.labelGFLength = new System.Windows.Forms.Label();
            this.numGFLength = new System.Windows.Forms.NumericUpDown();
            this.checkBoxExtRef = new System.Windows.Forms.CheckBox();
            this.comboBoxDACRange = new System.Windows.Forms.ComboBox();
            this.labelDACRange = new System.Windows.Forms.Label();
            this.labelPhysicalPlasement = new System.Windows.Forms.Label();
            this.comboBoxPhysicalPlacement = new System.Windows.Forms.ComboBox();
            this.labelAnalogBus = new System.Windows.Forms.Label();
            this.comboBoxAnalogBus = new System.Windows.Forms.ComboBox();
            this.lblTimeUnits = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numNumVoltages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGFLength)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of voltages:";
            // 
            // numNumVoltages
            // 
            this.numNumVoltages.Location = new System.Drawing.Point(178, 8);
            this.numNumVoltages.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numNumVoltages.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumVoltages.Name = "numNumVoltages";
            this.numNumVoltages.Size = new System.Drawing.Size(72, 20);
            this.numNumVoltages.TabIndex = 6;
            this.numNumVoltages.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // labelCompareType
            // 
            this.labelCompareType.AutoSize = true;
            this.labelCompareType.Location = new System.Drawing.Point(13, 63);
            this.labelCompareType.Name = "labelCompareType";
            this.labelCompareType.Size = new System.Drawing.Size(75, 13);
            this.labelCompareType.TabIndex = 7;
            this.labelCompareType.Text = "Compare type:";
            // 
            // comboBoxCompareType
            // 
            this.comboBoxCompareType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCompareType.FormattingEnabled = true;
            this.comboBoxCompareType.Items.AddRange(new object[] {
            "OV/UV",
            "OV only",
            "UV only"});
            this.comboBoxCompareType.Location = new System.Drawing.Point(178, 60);
            this.comboBoxCompareType.Name = "comboBoxCompareType";
            this.comboBoxCompareType.Size = new System.Drawing.Size(136, 21);
            this.comboBoxCompareType.TabIndex = 8;
            this.comboBoxCompareType.SelectedIndexChanged += new System.EventHandler(this.comboBoxCompareType_SelectedIndexChanged);
            // 
            // labelGFLength
            // 
            this.labelGFLength.AutoSize = true;
            this.labelGFLength.Location = new System.Drawing.Point(13, 36);
            this.labelGFLength.Name = "labelGFLength";
            this.labelGFLength.Size = new System.Drawing.Size(91, 13);
            this.labelGFLength.TabIndex = 9;
            this.labelGFLength.Text = "Glitch filter length:";
            // 
            // numGFLength
            // 
            this.numGFLength.Location = new System.Drawing.Point(178, 34);
            this.numGFLength.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGFLength.Name = "numGFLength";
            this.numGFLength.Size = new System.Drawing.Size(72, 20);
            this.numGFLength.TabIndex = 10;
            this.numGFLength.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // checkBoxExtRef
            // 
            this.checkBoxExtRef.AutoSize = true;
            this.checkBoxExtRef.Location = new System.Drawing.Point(17, 143);
            this.checkBoxExtRef.Name = "checkBoxExtRef";
            this.checkBoxExtRef.Size = new System.Drawing.Size(112, 17);
            this.checkBoxExtRef.TabIndex = 11;
            this.checkBoxExtRef.Text = "External reference";
            this.checkBoxExtRef.UseVisualStyleBackColor = true;
            this.checkBoxExtRef.CheckedChanged += new System.EventHandler(this.checkBoxExtRef_CheckedChanged);
            // 
            // comboBoxDACRange
            // 
            this.comboBoxDACRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDACRange.FormattingEnabled = true;
            this.comboBoxDACRange.Items.AddRange(new object[] {
            "1V",
            "4V"});
            this.comboBoxDACRange.Location = new System.Drawing.Point(178, 165);
            this.comboBoxDACRange.Name = "comboBoxDACRange";
            this.comboBoxDACRange.Size = new System.Drawing.Size(136, 21);
            this.comboBoxDACRange.TabIndex = 12;
            this.comboBoxDACRange.SelectedIndexChanged += new System.EventHandler(this.comboBoxDACRange_SelectedIndexChanged);
            // 
            // labelDACRange
            // 
            this.labelDACRange.AutoSize = true;
            this.labelDACRange.Location = new System.Drawing.Point(14, 168);
            this.labelDACRange.Name = "labelDACRange";
            this.labelDACRange.Size = new System.Drawing.Size(62, 13);
            this.labelDACRange.TabIndex = 13;
            this.labelDACRange.Text = "DAC range:";
            // 
            // labelPhysicalPlasement
            // 
            this.labelPhysicalPlasement.AutoSize = true;
            this.labelPhysicalPlasement.Location = new System.Drawing.Point(13, 117);
            this.labelPhysicalPlasement.Name = "labelPhysicalPlasement";
            this.labelPhysicalPlasement.Size = new System.Drawing.Size(101, 13);
            this.labelPhysicalPlasement.TabIndex = 14;
            this.labelPhysicalPlasement.Text = "Physical placement:";
            // 
            // comboBoxPhysicalPlacement
            // 
            this.comboBoxPhysicalPlacement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPhysicalPlacement.FormattingEnabled = true;
            this.comboBoxPhysicalPlacement.Location = new System.Drawing.Point(178, 114);
            this.comboBoxPhysicalPlacement.Name = "comboBoxPhysicalPlacement";
            this.comboBoxPhysicalPlacement.Size = new System.Drawing.Size(136, 21);
            this.comboBoxPhysicalPlacement.TabIndex = 15;
            this.comboBoxPhysicalPlacement.SelectedIndexChanged += new System.EventHandler(this.comboBoxPhysicalPlacement_SelectedIndexChanged);
            // 
            // labelAnalogBus
            // 
            this.labelAnalogBus.AutoSize = true;
            this.labelAnalogBus.Location = new System.Drawing.Point(13, 90);
            this.labelAnalogBus.Name = "labelAnalogBus";
            this.labelAnalogBus.Size = new System.Drawing.Size(63, 13);
            this.labelAnalogBus.TabIndex = 16;
            this.labelAnalogBus.Text = "Analog bus:";
            // 
            // comboBoxAnalogBus
            // 
            this.comboBoxAnalogBus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnalogBus.FormattingEnabled = true;
            this.comboBoxAnalogBus.Items.AddRange(new object[] {
            "AMXUBUSR",
            "AMXUBUSL"});
            this.comboBoxAnalogBus.Location = new System.Drawing.Point(178, 87);
            this.comboBoxAnalogBus.Name = "comboBoxAnalogBus";
            this.comboBoxAnalogBus.Size = new System.Drawing.Size(136, 21);
            this.comboBoxAnalogBus.TabIndex = 17;
            this.comboBoxAnalogBus.SelectedIndexChanged += new System.EventHandler(this.comboBoxAnalogBus_SelectedIndexChanged);
            // 
            // lblTimeUnits
            // 
            this.lblTimeUnits.AutoSize = true;
            this.lblTimeUnits.Location = new System.Drawing.Point(256, 36);
            this.lblTimeUnits.Name = "lblTimeUnits";
            this.lblTimeUnits.Size = new System.Drawing.Size(54, 13);
            this.lblTimeUnits.TabIndex = 18;
            this.lblTimeUnits.Text = "N us/step";
            // 
            // CyBasicTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTimeUnits);
            this.Controls.Add(this.comboBoxAnalogBus);
            this.Controls.Add(this.labelAnalogBus);
            this.Controls.Add(this.comboBoxPhysicalPlacement);
            this.Controls.Add(this.labelPhysicalPlasement);
            this.Controls.Add(this.labelDACRange);
            this.Controls.Add(this.comboBoxDACRange);
            this.Controls.Add(this.checkBoxExtRef);
            this.Controls.Add(this.numGFLength);
            this.Controls.Add(this.labelGFLength);
            this.Controls.Add(this.comboBoxCompareType);
            this.Controls.Add(this.labelCompareType);
            this.Controls.Add(this.numNumVoltages);
            this.Controls.Add(this.label1);
            this.Name = "CyBasicTab";
            this.Size = new System.Drawing.Size(567, 426);
            this.Load += new System.EventHandler(this.CyBasicTab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numNumVoltages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGFLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numNumVoltages;
        private System.Windows.Forms.Label labelCompareType;
        private System.Windows.Forms.ComboBox comboBoxCompareType;
        private System.Windows.Forms.Label labelGFLength;
        private System.Windows.Forms.NumericUpDown numGFLength;
        private System.Windows.Forms.CheckBox checkBoxExtRef;
        private System.Windows.Forms.ComboBox comboBoxDACRange;
        private System.Windows.Forms.Label labelDACRange;
        private System.Windows.Forms.Label labelPhysicalPlasement;
        private System.Windows.Forms.ComboBox comboBoxPhysicalPlacement;
        private System.Windows.Forms.Label labelAnalogBus;
        private System.Windows.Forms.ComboBox comboBoxAnalogBus;
        private System.Windows.Forms.Label lblTimeUnits;
    }
}
