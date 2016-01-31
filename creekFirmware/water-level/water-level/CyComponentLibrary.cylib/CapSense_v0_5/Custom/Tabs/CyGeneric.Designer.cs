namespace  CapSense_v0_5
{
    partial class CyGeneric
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgGeneric = new System.Windows.Forms.DataGridView();
            this.colGenericSensorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgGeneric)).BeginInit();
            this.SuspendLayout();
            // 
            // dgGeneric
            // 
            this.dgGeneric.AllowDrop = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgGeneric.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgGeneric.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgGeneric.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGenericSensorName,
            this.colNumber});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgGeneric.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgGeneric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgGeneric.Location = new System.Drawing.Point(0, 0);
            this.dgGeneric.Name = "dgGeneric";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgGeneric.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgGeneric.Size = new System.Drawing.Size(545, 294);
            this.dgGeneric.TabIndex = 2;
            // 
            // colGenericSensorName
            // 
            this.colGenericSensorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGenericSensorName.HeaderText = "Sensor Name";
            this.colGenericSensorName.Name = "colGenericSensorName";
            // 
            // colNumber
            // 
            this.colNumber.HeaderText = "Number";
            this.colNumber.Name = "colNumber";
            // 
            // CyGeneric
            // 
            this.Controls.Add(this.dgGeneric);
            this.Name = "CyGeneric";
            this.Size = new System.Drawing.Size(545, 294);
            ((System.ComponentModel.ISupportInitialize)(this.dgGeneric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgGeneric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenericSensorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
    }
}
