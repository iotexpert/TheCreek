namespace USBFS_v2_30
{
    partial class CyReportList
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
            this.dataGridViewValues = new System.Windows.Forms.DataGridView();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HexValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewValues)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewValues
            // 
            this.dataGridViewValues.AllowUserToAddRows = false;
            this.dataGridViewValues.AllowUserToDeleteRows = false;
            this.dataGridViewValues.AllowUserToResizeRows = false;
            this.dataGridViewValues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewValues.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewValues.ColumnHeadersVisible = false;
            this.dataGridViewValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemName,
            this.HexValue});
            this.dataGridViewValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewValues.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewValues.Name = "dataGridViewValues";
            this.dataGridViewValues.RowHeadersVisible = false;
            this.dataGridViewValues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewValues.Size = new System.Drawing.Size(159, 178);
            this.dataGridViewValues.TabIndex = 0;
            this.dataGridViewValues.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewValues_CellValueChanged);
            this.dataGridViewValues.SelectionChanged += new System.EventHandler(this.dataGridViewValues_SelectionChanged);
            // 
            // ItemName
            // 
            this.ItemName.FillWeight = 75.93922F;
            this.ItemName.HeaderText = "ItemName";
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // HexValue
            // 
            this.HexValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.HexValue.FillWeight = 123.5278F;
            this.HexValue.HeaderText = "HexValue";
            this.HexValue.Name = "HexValue";
            this.HexValue.ReadOnly = true;
            this.HexValue.Width = 60;
            // 
            // CyReportList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewValues);
            this.Name = "CyReportList";
            this.Size = new System.Drawing.Size(159, 178);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewValues)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn HexValue;

    }
}
