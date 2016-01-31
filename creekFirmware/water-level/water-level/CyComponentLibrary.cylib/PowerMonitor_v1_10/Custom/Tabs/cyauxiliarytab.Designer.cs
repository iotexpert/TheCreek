namespace PowerMonitor_v1_10
{
    partial class CyAuxiliaryTab
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
            this.colAuxInputNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAuxInputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoltageMeasurementType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cyToolStrip1 = new PowerMonitor_v1_10.CyToolStrip();
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
            this.colAuxInputNumber,
            this.colAuxInputName,
            this.colVoltageMeasurementType});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(812, 412);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);            
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // colAuxInputNumber
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.colAuxInputNumber.DefaultCellStyle = dataGridViewCellStyle1;
            this.colAuxInputNumber.HeaderText = "Aux input number";
            this.colAuxInputNumber.Name = "colAuxInputNumber";
            this.colAuxInputNumber.ReadOnly = true;
            // 
            // colAuxInputName
            // 
            this.colAuxInputName.HeaderText = "Aux input name";
            this.colAuxInputName.MaxInputLength = 16;
            this.colAuxInputName.Name = "colAuxInputName";
            // 
            // colVoltageMeasurementType
            // 
            this.colVoltageMeasurementType.HeaderText = "Voltage measurement type";
            this.colVoltageMeasurementType.Name = "colVoltageMeasurementType";
            this.colVoltageMeasurementType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colVoltageMeasurementType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cyToolStrip1
            // 
            this.cyToolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cyToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.cyToolStrip1.Name = "cyToolStrip1";
            this.cyToolStrip1.Size = new System.Drawing.Size(812, 25);
            this.cyToolStrip1.TabIndex = 1;
            // 
            // CyAuxiliaryTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cyToolStrip1);
            this.Name = "CyAuxiliaryTab";
            this.Size = new System.Drawing.Size(812, 437);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private CyToolStrip cyToolStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAuxInputNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAuxInputName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colVoltageMeasurementType;
    }
}
