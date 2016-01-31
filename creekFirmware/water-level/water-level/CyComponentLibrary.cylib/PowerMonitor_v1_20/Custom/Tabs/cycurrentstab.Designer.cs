namespace PowerMonitor_v1_20
{
    partial class CyCurrentsTab
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colPowerConverterNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConverterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNominalOutputVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentMeasurementType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colOcWarningThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOcFaultThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShuntResistorValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCsaGain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cyToolStrip1 = new PowerMonitor_v1_20.CyToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPowerConverterNumber,
            this.colConverterName,
            this.colNominalOutputVoltage,
            this.colCurrentMeasurementType,
            this.colOcWarningThreshold,
            this.colOcFaultThreshold,
            this.colShuntResistorValue,
            this.colCsaGain});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 31);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1127, 447);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // colPowerConverterNumber
            // 
            this.colPowerConverterNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colPowerConverterNumber.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPowerConverterNumber.HeaderText = "Power converter number";
            this.colPowerConverterNumber.Name = "colPowerConverterNumber";
            this.colPowerConverterNumber.ReadOnly = true;
            // 
            // colConverterName
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colConverterName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colConverterName.HeaderText = "Power converter name";
            this.colConverterName.Name = "colConverterName";
            this.colConverterName.ReadOnly = true;
            // 
            // colNominalOutputVoltage
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colNominalOutputVoltage.DefaultCellStyle = dataGridViewCellStyle3;
            this.colNominalOutputVoltage.HeaderText = "Nominal voltage (V)";
            this.colNominalOutputVoltage.Name = "colNominalOutputVoltage";
            this.colNominalOutputVoltage.ReadOnly = true;
            // 
            // colCurrentMeasurementType
            // 
            this.colCurrentMeasurementType.HeaderText = "Current measurement type";
            this.colCurrentMeasurementType.Name = "colCurrentMeasurementType";
            // 
            // colOcWarningThreshold
            // 
            this.colOcWarningThreshold.HeaderText = "OC warning threshold (A)";
            this.colOcWarningThreshold.Name = "colOcWarningThreshold";
            // 
            // colOcFaultThreshold
            // 
            this.colOcFaultThreshold.HeaderText = "OC fault threshold (A)";
            this.colOcFaultThreshold.Name = "colOcFaultThreshold";
            // 
            // colShuntResistorValue
            // 
            this.colShuntResistorValue.HeaderText = "Shunt resistor value (mΩ)";
            this.colShuntResistorValue.Name = "colShuntResistorValue";
            // 
            // colCsaGain
            // 
            this.colCsaGain.HeaderText = "CSA gain (V/ΔV)";
            this.colCsaGain.Name = "colCsaGain";
            // 
            // cyToolStrip1
            // 
            this.cyToolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cyToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.cyToolStrip1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cyToolStrip1.Name = "cyToolStrip1";
            this.cyToolStrip1.Size = new System.Drawing.Size(1127, 31);
            this.cyToolStrip1.TabIndex = 1;
            // 
            // CyCurrentsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cyToolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CyCurrentsTab";
            this.Size = new System.Drawing.Size(1127, 478);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private CyToolStrip cyToolStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPowerConverterNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConverterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNominalOutputVoltage;
        private System.Windows.Forms.DataGridViewComboBoxColumn colCurrentMeasurementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOcWarningThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOcFaultThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShuntResistorValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCsaGain;
    }
}
