namespace VoltageFaultDetector_v2_0
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvVoltages = new System.Windows.Forms.DataGridView();
            this.colVoltageInput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoltageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNominalVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUVFaultThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOVFaultThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInputScalingFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cyToolStrip1 = new VoltageFaultDetector_v2_0.CyToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVoltages)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvVoltages
            // 
            this.dgvVoltages.AllowUserToAddRows = false;
            this.dgvVoltages.AllowUserToDeleteRows = false;
            this.dgvVoltages.AllowUserToResizeColumns = false;
            this.dgvVoltages.AllowUserToResizeRows = false;
            this.dgvVoltages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVoltages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVoltages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVoltageInput,
            this.colVoltageName,
            this.colNominalVoltage,
            this.colUVFaultThreshold,
            this.colOVFaultThreshold,
            this.colInputScalingFactor});
            this.dgvVoltages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVoltages.Location = new System.Drawing.Point(0, 25);
            this.dgvVoltages.Name = "dgvVoltages";
            this.dgvVoltages.RowHeadersWidth = 20;
            this.dgvVoltages.Size = new System.Drawing.Size(950, 325);
            this.dgvVoltages.TabIndex = 0;
            this.dgvVoltages.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvVoltages_CellFormatting);
            this.dgvVoltages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVoltages_CellValueChanged);
            this.dgvVoltages.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvVoltages_CurrentCellDirtyStateChanged);
            this.dgvVoltages.SelectionChanged += new System.EventHandler(this.dgvVoltages_SelectionChanged);
            // 
            // colVoltageInput
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colVoltageInput.DefaultCellStyle = dataGridViewCellStyle1;
            this.colVoltageInput.HeaderText = "Voltage number";
            this.colVoltageInput.Name = "colVoltageInput";
            this.colVoltageInput.ReadOnly = true;
            // 
            // colVoltageName
            // 
            this.colVoltageName.HeaderText = "Voltage name";
            this.colVoltageName.MaxInputLength = 16;
            this.colVoltageName.Name = "colVoltageName";
            // 
            // colNominalVoltage
            // 
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.colNominalVoltage.DefaultCellStyle = dataGridViewCellStyle2;
            this.colNominalVoltage.HeaderText = "Nominal voltage (V)";
            this.colNominalVoltage.Name = "colNominalVoltage";
            // 
            // colUVFaultThreshold
            // 
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.colUVFaultThreshold.DefaultCellStyle = dataGridViewCellStyle3;
            this.colUVFaultThreshold.HeaderText = "UV fault threshold (V)";
            this.colUVFaultThreshold.Name = "colUVFaultThreshold";
            // 
            // colOVFaultThreshold
            // 
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.colOVFaultThreshold.DefaultCellStyle = dataGridViewCellStyle4;
            this.colOVFaultThreshold.HeaderText = "OV fault threshold (V)";
            this.colOVFaultThreshold.Name = "colOVFaultThreshold";
            // 
            // colInputScalingFactor
            // 
            dataGridViewCellStyle5.Format = "N3";
            dataGridViewCellStyle5.NullValue = null;
            this.colInputScalingFactor.DefaultCellStyle = dataGridViewCellStyle5;
            this.colInputScalingFactor.HeaderText = "Input scaling factor";
            this.colInputScalingFactor.Name = "colInputScalingFactor";
            // 
            // cyToolStrip1
            // 
            this.cyToolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cyToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.cyToolStrip1.Name = "cyToolStrip1";
            this.cyToolStrip1.Size = new System.Drawing.Size(950, 25);
            this.cyToolStrip1.TabIndex = 1;
            // 
            // CyVoltagesTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.dgvVoltages);
            this.Controls.Add(this.cyToolStrip1);
            this.Name = "CyVoltagesTab";
            this.Size = new System.Drawing.Size(950, 350);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVoltages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVoltages;
        private CyToolStrip cyToolStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoltageInput;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoltageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNominalVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUVFaultThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOVFaultThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInputScalingFactor;
    }
}
