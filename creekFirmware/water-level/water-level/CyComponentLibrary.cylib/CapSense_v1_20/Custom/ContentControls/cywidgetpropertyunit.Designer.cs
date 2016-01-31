namespace  CapSense_v1_20
{
    partial class CyWidgetPropertyUnit
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.labelTop = new System.Windows.Forms.Label();
            this.sCMain = new System.Windows.Forms.SplitContainer();
            this.sCBottom = new System.Windows.Forms.SplitContainer();
            this.pGRows = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.pGCols = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.sCMain.Panel1.SuspendLayout();
            this.sCMain.Panel2.SuspendLayout();
            this.sCMain.SuspendLayout();
            this.sCBottom.Panel1.SuspendLayout();
            this.sCBottom.Panel2.SuspendLayout();
            this.sCBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(474, 94);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // labelTop
            // 
            this.labelTop.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.labelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTop.Location = new System.Drawing.Point(0, 0);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(474, 22);
            this.labelTop.TabIndex = 37;
            this.labelTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sCMain
            // 
            this.sCMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sCMain.Location = new System.Drawing.Point(0, 22);
            this.sCMain.Name = "sCMain";
            this.sCMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sCMain.Panel1
            // 
            this.sCMain.Panel1.Controls.Add(this.propertyGrid);
            // 
            // sCMain.Panel2
            // 
            this.sCMain.Panel2.Controls.Add(this.sCBottom);
            this.sCMain.Size = new System.Drawing.Size(474, 190);
            this.sCMain.SplitterDistance = 94;
            this.sCMain.TabIndex = 38;
            // 
            // sCBottom
            // 
            this.sCBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sCBottom.Location = new System.Drawing.Point(0, 0);
            this.sCBottom.Name = "sCBottom";
            // 
            // sCBottom.Panel1
            // 
            this.sCBottom.Panel1.Controls.Add(this.pGRows);
            this.sCBottom.Panel1.Controls.Add(this.label1);
            // 
            // sCBottom.Panel2
            // 
            this.sCBottom.Panel2.Controls.Add(this.pGCols);
            this.sCBottom.Panel2.Controls.Add(this.label2);
            this.sCBottom.Size = new System.Drawing.Size(474, 92);
            this.sCBottom.SplitterDistance = 242;
            this.sCBottom.TabIndex = 39;
            // 
            // pGRows
            // 
            this.pGRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGRows.HelpVisible = false;
            this.pGRows.Location = new System.Drawing.Point(0, 19);
            this.pGRows.Name = "pGRows";
            this.pGRows.Size = new System.Drawing.Size(242, 73);
            this.pGRows.TabIndex = 2;
            this.pGRows.ToolbarVisible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 19);
            this.label1.TabIndex = 38;
            this.label1.Text = "Touchpad Rows";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pGCols
            // 
            this.pGCols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGCols.HelpVisible = false;
            this.pGCols.Location = new System.Drawing.Point(0, 19);
            this.pGCols.Name = "pGCols";
            this.pGCols.Size = new System.Drawing.Size(228, 73);
            this.pGCols.TabIndex = 2;
            this.pGCols.ToolbarVisible = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 19);
            this.label2.TabIndex = 38;
            this.label2.Text = "Touchpad Columns";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cntUnitPropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sCMain);
            this.Controls.Add(this.labelTop);
            this.Name = "cntUnitPropertyGrid";
            this.Size = new System.Drawing.Size(474, 212);
            this.sCMain.Panel1.ResumeLayout(false);
            this.sCMain.Panel2.ResumeLayout(false);
            this.sCMain.ResumeLayout(false);
            this.sCBottom.Panel1.ResumeLayout(false);
            this.sCBottom.Panel2.ResumeLayout(false);
            this.sCBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.SplitContainer sCMain;
        private System.Windows.Forms.SplitContainer sCBottom;
        public System.Windows.Forms.PropertyGrid pGRows;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.PropertyGrid pGCols;
        private System.Windows.Forms.Label label2;
    }
}
