namespace SMBusSlave_v1_0
{
    partial class CyPmBusCmdsTab
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCommandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colFormat = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaged = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colReadConfig = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colWriteConfig = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cyToolStrip1 = new SMBusSlave_v1_0.CyToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEnable,
            this.colCommandName,
            this.colReference,
            this.colCode,
            this.colType,
            this.colFormat,
            this.colSize,
            this.colPaged,
            this.colReadConfig,
            this.colWriteConfig});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 25);
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 24;
            this.dgv.Size = new System.Drawing.Size(845, 329);
            this.dgv.TabIndex = 0;
            this.dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            // 
            // colEnable
            // 
            this.colEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colEnable.HeaderText = "Enable";
            this.colEnable.Name = "colEnable";
            this.colEnable.Width = 46;
            // 
            // colCommandName
            // 
            this.colCommandName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCommandName.HeaderText = "Command name";
            this.colCommandName.MaxInputLength = 24;
            this.colCommandName.Name = "colCommandName";
            this.colCommandName.Width = 99;
            // 
            // colReference
            // 
            this.colReference.HeaderText = "Column1";
            this.colReference.Name = "colReference";
            this.colReference.Visible = false;
            // 
            // colCode
            // 
            this.colCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.Width = 57;
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.Width = 37;
            // 
            // colFormat
            // 
            this.colFormat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colFormat.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.colFormat.HeaderText = "Format";
            this.colFormat.Name = "colFormat";
            this.colFormat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFormat.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colFormat.Width = 64;
            // 
            // colSize
            // 
            this.colSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colSize.HeaderText = "Size";
            this.colSize.Name = "colSize";
            this.colSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSize.Width = 33;
            // 
            // colPaged
            // 
            this.colPaged.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colPaged.HeaderText = "Paged";
            this.colPaged.Name = "colPaged";
            this.colPaged.Width = 44;
            // 
            // colReadConfig
            // 
            this.colReadConfig.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colReadConfig.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.colReadConfig.HeaderText = "Read config";
            this.colReadConfig.Name = "colReadConfig";
            this.colReadConfig.Width = 64;
            // 
            // colWriteConfig
            // 
            this.colWriteConfig.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colWriteConfig.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.colWriteConfig.HeaderText = "Write config";
            this.colWriteConfig.Name = "colWriteConfig";
            this.colWriteConfig.Width = 63;
            // 
            // cyToolStrip1
            // 
            this.cyToolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cyToolStrip1.GeneralView = false;
            this.cyToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.cyToolStrip1.Name = "cyToolStrip1";
            this.cyToolStrip1.Size = new System.Drawing.Size(845, 25);
            this.cyToolStrip1.TabIndex = 3;
            // 
            // CyPmBusCmdsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.cyToolStrip1);
            this.Name = "CyPmBusCmdsTab";
            this.Size = new System.Drawing.Size(845, 354);
            ((System.ComponentModel.ISupportInitialize)(this.m_errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCommandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReference;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewComboBoxColumn colType;
        private System.Windows.Forms.DataGridViewComboBoxColumn colFormat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPaged;
        private System.Windows.Forms.DataGridViewComboBoxColumn colReadConfig;
        private System.Windows.Forms.DataGridViewComboBoxColumn colWriteConfig;
        private CyToolStrip cyToolStrip1;
    }
}
