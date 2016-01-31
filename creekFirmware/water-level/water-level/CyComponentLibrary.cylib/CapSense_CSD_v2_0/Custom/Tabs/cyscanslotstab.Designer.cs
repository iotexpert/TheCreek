namespace  CapSense_CSD_v2_0
{
    partial class CyScanSlotsTab
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
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.dgScanSlots = new CapSense_CSD_v2_0.CySSDataGrid();
            this.colIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCh0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCh1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbScanTime = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tslSensorScanTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tslTotalScanTime = new System.Windows.Forms.ToolStripLabel();
            this.panelConteiner = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripPromote = new System.Windows.Forms.ToolStripButton();
            this.toolStripDemote = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMoveToChSecond = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveToChFirst = new System.Windows.Forms.ToolStripButton();
            this.tsbLastSeparetor = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelSSName = new System.Windows.Forms.ToolStripLabel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgScanSlots)).BeginInit();
            this.tbScanTime.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // scMain
            // 
            this.scMain.BackColor = System.Drawing.SystemColors.Control;
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.dgScanSlots);
            this.scMain.Panel1.Controls.Add(this.tbScanTime);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.AutoScroll = true;
            this.scMain.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.scMain.Panel2.Controls.Add(this.panelConteiner);
            this.scMain.Size = new System.Drawing.Size(552, 259);
            this.scMain.SplitterDistance = 211;
            this.scMain.TabIndex = 38;
            // 
            // dgScanSlots
            // 
            this.dgScanSlots.AllowDrop = true;
            this.dgScanSlots.AllowUserToAddRows = false;
            this.dgScanSlots.AllowUserToResizeColumns = false;
            this.dgScanSlots.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgScanSlots.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgScanSlots.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIndex,
            this.colCh0,
            this.colCh1});
            this.dgScanSlots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgScanSlots.Location = new System.Drawing.Point(0, 0);
            this.dgScanSlots.Name = "dgScanSlots";
            this.dgScanSlots.RowHeadersVisible = false;
            this.dgScanSlots.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgScanSlots.RowTemplate.Height = 24;
            this.dgScanSlots.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgScanSlots.Size = new System.Drawing.Size(552, 186);
            this.dgScanSlots.TabIndex = 37;
            this.dgScanSlots.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgScanSlots_MouseDown);
            this.dgScanSlots.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgScanSlots_CellPainting);
            this.dgScanSlots.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgScanSlots_KeyDown);
            this.dgScanSlots.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgScanSlots_CellEnter);
            this.dgScanSlots.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgScanSlots_RowsRemoved);
            // 
            // colIndex
            // 
            this.colIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colIndex.HeaderText = "Scan Slot  ";
            this.colIndex.Name = "colIndex";
            this.colIndex.ReadOnly = true;
            this.colIndex.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colIndex.Width = 65;
            // 
            // colCh0
            // 
            this.colCh0.HeaderText = "Ch0 Sensor";
            this.colCh0.Name = "colCh0";
            this.colCh0.ReadOnly = true;
            this.colCh0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCh0.Width = 240;
            // 
            // colCh1
            // 
            this.colCh1.HeaderText = "Ch1 Sensor";
            this.colCh1.Name = "colCh1";
            this.colCh1.ReadOnly = true;
            this.colCh1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colCh1.Width = 240;
            // 
            // tbScanTime
            // 
            this.tbScanTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbScanTime.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tbScanTime.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tslSensorScanTime,
            this.toolStripLabel2,
            this.tslTotalScanTime});
            this.tbScanTime.Location = new System.Drawing.Point(0, 186);
            this.tbScanTime.Name = "tbScanTime";
            this.tbScanTime.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tbScanTime.Size = new System.Drawing.Size(552, 25);
            this.tbScanTime.TabIndex = 38;
            this.tbScanTime.Text = "ScanTime";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(92, 22);
            this.toolStripLabel1.Text = "Sensor scan time:";
            // 
            // tslSensorScanTime
            // 
            this.tslSensorScanTime.AutoSize = false;
            this.tslSensorScanTime.Name = "tslSensorScanTime";
            this.tslSensorScanTime.Size = new System.Drawing.Size(100, 22);
            this.tslSensorScanTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(86, 22);
            this.toolStripLabel2.Text = "Total Scan Time:";
            // 
            // tslTotalScanTime
            // 
            this.tslTotalScanTime.Name = "tslTotalScanTime";
            this.tslTotalScanTime.Size = new System.Drawing.Size(28, 22);
            this.tslTotalScanTime.Text = "       ";
            // 
            // panelConteiner
            // 
            this.panelConteiner.AutoScroll = true;
            this.panelConteiner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConteiner.Location = new System.Drawing.Point(0, 0);
            this.panelConteiner.Margin = new System.Windows.Forms.Padding(4);
            this.panelConteiner.Name = "panelConteiner";
            this.panelConteiner.Size = new System.Drawing.Size(552, 44);
            this.panelConteiner.TabIndex = 24;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPromote,
            this.toolStripDemote,
            this.toolStripSeparator2,
            this.tsbMoveToChSecond,
            this.tsbMoveToChFirst,
            this.tsbLastSeparetor,
            this.toolStripLabelSSName});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(552, 25);
            this.toolStrip2.TabIndex = 38;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripPromote
            // 
            this.toolStripPromote.Image = global::CapSense_CSD_v2_0.CyCsResource.moveup;
            this.toolStripPromote.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPromote.Name = "toolStripPromote";
            this.toolStripPromote.Size = new System.Drawing.Size(67, 22);
            this.toolStripPromote.Text = "Promote";
            this.toolStripPromote.Click += new System.EventHandler(this.toolStripPromoteDemote_Click);
            // 
            // toolStripDemote
            // 
            this.toolStripDemote.Image = global::CapSense_CSD_v2_0.CyCsResource.movedown;
            this.toolStripDemote.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDemote.Name = "toolStripDemote";
            this.toolStripDemote.Size = new System.Drawing.Size(64, 22);
            this.toolStripDemote.Text = "Demote";
            this.toolStripDemote.Click += new System.EventHandler(this.toolStripPromoteDemote_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbMoveToChSecond
            // 
            this.tsbMoveToChSecond.Image = global::CapSense_CSD_v2_0.CyCsResource.movetosecond;
            this.tsbMoveToChSecond.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveToChSecond.Name = "tsbMoveToChSecond";
            this.tsbMoveToChSecond.Size = new System.Drawing.Size(115, 22);
            this.tsbMoveToChSecond.Text = "Move to channel 1";
            this.tsbMoveToChSecond.Click += new System.EventHandler(this.tsbMoveToChannel_Click);
            // 
            // tsbMoveToChFirst
            // 
            this.tsbMoveToChFirst.Image = global::CapSense_CSD_v2_0.CyCsResource.movetofirst;
            this.tsbMoveToChFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveToChFirst.Name = "tsbMoveToChFirst";
            this.tsbMoveToChFirst.Size = new System.Drawing.Size(115, 22);
            this.tsbMoveToChFirst.Text = "Move to channel 0";
            this.tsbMoveToChFirst.Click += new System.EventHandler(this.tsbMoveToChannel_Click);
            // 
            // tsbLastSeparetor
            // 
            this.tsbLastSeparetor.Name = "tsbLastSeparetor";
            this.tsbLastSeparetor.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelSSName
            // 
            this.toolStripLabelSSName.Name = "toolStripLabelSSName";
            this.toolStripLabelSSName.Size = new System.Drawing.Size(0, 22);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewTextBoxColumn1.HeaderText = "Scan Slot  ";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Ch0 Sensor";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 240;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Ch1 Sensor";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 240;
            // 
            // CyScanSlotsTab
            // 
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.toolStrip2);
            this.Name = "CyScanSlotsTab";
            this.Size = new System.Drawing.Size(552, 284);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel1.PerformLayout();
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgScanSlots)).EndInit();
            this.tbScanTime.ResumeLayout(false);
            this.tbScanTime.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer scMain;
        private CySSDataGrid dgScanSlots;
        private System.Windows.Forms.Panel panelConteiner;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripPromote;
        private System.Windows.Forms.ToolStripButton toolStripDemote;
        private System.Windows.Forms.ToolStripSeparator tsbLastSeparetor;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSSName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbMoveToChSecond;
        private System.Windows.Forms.ToolStripButton tsbMoveToChFirst;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCh0;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCh1;
        private System.Windows.Forms.ToolStrip tbScanTime;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel tslSensorScanTime;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel tslTotalScanTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;





    }
}
