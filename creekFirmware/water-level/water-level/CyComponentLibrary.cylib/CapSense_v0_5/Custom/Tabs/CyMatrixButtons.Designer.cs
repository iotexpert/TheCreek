namespace  CapSense_v0_5
{
    partial class CyMatrixButtons
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
            this.dgMatrixButtons = new System.Windows.Forms.DataGridView();
            this.colMatrixButtonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatrixButtonRows = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatrixButtonColumns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.UnitPropertyGrid = new cntUnitPropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.dgMatrixButtons)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgMatrixButtons
            // 
            this.dgMatrixButtons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMatrixButtons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMatrixButtonName,
            this.colMatrixButtonRows,
            this.colMatrixButtonColumns});
            this.dgMatrixButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMatrixButtons.Location = new System.Drawing.Point(0, 0);
            this.dgMatrixButtons.Name = "dgMatrixButtons";
            this.dgMatrixButtons.Size = new System.Drawing.Size(432, 204);
            this.dgMatrixButtons.TabIndex = 4;            
            // 
            // colMatrixButtonName
            // 
            this.colMatrixButtonName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMatrixButtonName.HeaderText = "Button Matrix Name";
            this.colMatrixButtonName.Name = "colMatrixButtonName";
            // 
            // colMatrixButtonRows
            // 
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.colMatrixButtonRows.DefaultCellStyle = dataGridViewCellStyle1;
            this.colMatrixButtonRows.HeaderText = "Number of Rows";
            this.colMatrixButtonRows.MaxInputLength = 2;
            this.colMatrixButtonRows.MinimumWidth = 50;
            this.colMatrixButtonRows.Name = "colMatrixButtonRows";
            this.colMatrixButtonRows.Width = 80;
            // 
            // colMatrixButtonColumns
            // 
            this.colMatrixButtonColumns.HeaderText = "Number of Columns";
            this.colMatrixButtonColumns.MaxInputLength = 2;
            this.colMatrixButtonColumns.MinimumWidth = 50;
            this.colMatrixButtonColumns.Name = "colMatrixButtonColumns";
            this.colMatrixButtonColumns.Width = 80;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgMatrixButtons);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.UnitPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(432, 300);
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 5;
            // 
            // UnitPropertyGrid
            // 
            this.UnitPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnitPropertyGrid.IsTouchPadMode = false;
            this.UnitPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.UnitPropertyGrid.Name = "UnitPropertyGrid";
            this.UnitPropertyGrid.Size = new System.Drawing.Size(432, 92);
            this.UnitPropertyGrid.TabIndex = 0;
            // 
            // CyMatrixButtons
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "CyMatrixButtons";
            this.Size = new System.Drawing.Size(432, 300);
            ((System.ComponentModel.ISupportInitialize)(this.dgMatrixButtons)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgMatrixButtons;
        public System.Windows.Forms.SplitContainer splitContainer1;
        public cntUnitPropertyGrid UnitPropertyGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatrixButtonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatrixButtonRows;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatrixButtonColumns;
    }
}
