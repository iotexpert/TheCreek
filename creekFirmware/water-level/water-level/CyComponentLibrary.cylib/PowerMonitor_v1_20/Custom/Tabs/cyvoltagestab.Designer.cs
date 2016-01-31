namespace PowerMonitor_v1_20
{
    partial class CyVoltagesTab
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colPowerConverterNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConverterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNominalOutputVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoltageMeasurementType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colUVFaultThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUVWarningThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOVWarningThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOVFaultThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInputScalingFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cyToolStrip1 = new PowerMonitor_v1_20.CyToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPowerConverterNumber,
            this.colConverterName,
            this.colNominalOutputVoltage,
            this.colVoltageMeasurementType,
            this.colUVFaultThreshold,
            this.colUVWarningThreshold,
            this.colOVWarningThreshold,
            this.colOVFaultThreshold,
            this.colInputScalingFactor});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 31);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1267, 400);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged_1);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // colPowerConverterNumber
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colPowerConverterNumber.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPowerConverterNumber.HeaderText = "Power converter number";
            this.colPowerConverterNumber.Name = "colPowerConverterNumber";
            this.colPowerConverterNumber.ReadOnly = true;
            // 
            // colConverterName
            // 
            this.colConverterName.HeaderText = "Power converter name";
            this.colConverterName.MaxInputLength = 16;
            this.colConverterName.Name = "colConverterName";
            // 
            // colNominalOutputVoltage
            // 
            this.colNominalOutputVoltage.HeaderText = "Nominal voltage (V)";
            this.colNominalOutputVoltage.Name = "colNominalOutputVoltage";
            // 
            // colVoltageMeasurementType
            // 
            this.colVoltageMeasurementType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colVoltageMeasurementType.HeaderText = "Voltage measurement type";
            this.colVoltageMeasurementType.Name = "colVoltageMeasurementType";
            this.colVoltageMeasurementType.Width = 5;
            // 
            // colUVFaultThreshold
            // 
            this.colUVFaultThreshold.HeaderText = "UV fault threshold (V)";
            this.colUVFaultThreshold.Name = "colUVFaultThreshold";
            // 
            // colUVWarningThreshold
            // 
            this.colUVWarningThreshold.HeaderText = "UV warning threshold (V)";
            this.colUVWarningThreshold.Name = "colUVWarningThreshold";
            // 
            // colOVWarningThreshold
            // 
            this.colOVWarningThreshold.HeaderText = "OV warning threshold (V)";
            this.colOVWarningThreshold.Name = "colOVWarningThreshold";
            // 
            // colOVFaultThreshold
            // 
            this.colOVFaultThreshold.HeaderText = "OV fault threshold (V)";
            this.colOVFaultThreshold.Name = "colOVFaultThreshold";
            // 
            // colInputScalingFactor
            // 
            this.colInputScalingFactor.HeaderText = "Input scaling factor";
            this.colInputScalingFactor.Name = "colInputScalingFactor";
            // 
            // cyToolStrip1
            // 
            this.cyToolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cyToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.cyToolStrip1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cyToolStrip1.Name = "cyToolStrip1";
            this.cyToolStrip1.Size = new System.Drawing.Size(1267, 31);
            this.cyToolStrip1.TabIndex = 1;
            // 
            // CyVoltagesTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cyToolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CyVoltagesTab";
            this.Size = new System.Drawing.Size(1267, 431);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private CyToolStrip cyToolStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPowerConverterNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConverterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNominalOutputVoltage;
        private System.Windows.Forms.DataGridViewComboBoxColumn colVoltageMeasurementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUVFaultThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUVWarningThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOVWarningThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOVFaultThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputScalingFactor;
    }
}
