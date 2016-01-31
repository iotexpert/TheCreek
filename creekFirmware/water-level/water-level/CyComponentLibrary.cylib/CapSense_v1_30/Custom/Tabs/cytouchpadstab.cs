/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace  CapSense_v1_30
{
    public partial class CyTouchPadsTab : CyMyICyParamEditTemplate
    {
        public CyTouchPadsTab()
            : base()
        {
            InitializeComponent();
        }
        #region Windows Form Designer generated code
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
            this.dgTouchpads = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.UnitPropertyGrid = new  CapSense_v1_30.CyWidgetPropertyUnit();
            this.colTouchpadName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumberOfTouchpadRows = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTouchpadNumberOfColumns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRowsResolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumnsResolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgTouchpads)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            //
            // dgTouchpads
            //
            this.dgTouchpads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTouchpads.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTouchpadName,
            this.colNumberOfTouchpadRows,
            this.colTouchpadNumberOfColumns,
            this.colRowsResolution,
            this.colColumnsResolution});
            this.dgTouchpads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgTouchpads.Location = new System.Drawing.Point(0, 0);
            this.dgTouchpads.Name = "dgTouchpads";
            this.dgTouchpads.Size = new System.Drawing.Size(693, 209);
            this.dgTouchpads.TabIndex = 3;
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
            this.splitContainer1.Panel1.Controls.Add(this.dgTouchpads);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.UnitPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(693, 305);
            this.splitContainer1.SplitterDistance = 209;
            this.splitContainer1.TabIndex = 4;
            //
            // UnitPropertyGrid
            //
            this.UnitPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnitPropertyGrid.IsTouchPadMode = true;
            this.UnitPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.UnitPropertyGrid.Name = "UnitPropertyGrid";
            this.UnitPropertyGrid.Size = new System.Drawing.Size(693, 92);
            this.UnitPropertyGrid.TabIndex = 1;
            //
            // colTouchpadName
            //
            this.colTouchpadName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTouchpadName.HeaderText = "Touchpad Name";
            this.colTouchpadName.Name = "colTouchpadName";
            //
            // colNumberOfTouchpadRows
            //
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.colNumberOfTouchpadRows.DefaultCellStyle = dataGridViewCellStyle1;
            this.colNumberOfTouchpadRows.HeaderText = "Number of Rows";
            this.colNumberOfTouchpadRows.MaxInputLength = 2;
            this.colNumberOfTouchpadRows.MinimumWidth = 50;
            this.colNumberOfTouchpadRows.Name = "colNumberOfTouchpadRows";
            this.colNumberOfTouchpadRows.Width = 70;
            //
            // colTouchpadNumberOfColumns
            //
            this.colTouchpadNumberOfColumns.HeaderText = "Number of Columns";
            this.colTouchpadNumberOfColumns.MaxInputLength = 2;
            this.colTouchpadNumberOfColumns.MinimumWidth = 50;
            this.colTouchpadNumberOfColumns.Name = "colTouchpadNumberOfColumns";
            this.colTouchpadNumberOfColumns.Width = 80;
            //
            // colRowsResolution
            //
            this.colRowsResolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRowsResolution.HeaderText = "Rows Resolution";
            this.colRowsResolution.Name = "colRowsResolution";
            this.colRowsResolution.Width = 60;
            //
            // colColumnsResolution
            //
            this.colColumnsResolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colColumnsResolution.HeaderText = "Columns Resolution";
            this.colColumnsResolution.Name = "colColumnsResolution";
            this.colColumnsResolution.Width = 60;
            //
            // CyTouchPads
            //
            this.Controls.Add(this.splitContainer1);
            this.Name = "CyTouchPads";
            this.Size = new System.Drawing.Size(693, 305);
            ((System.ComponentModel.ISupportInitialize)(this.dgTouchpads)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgTouchpads;
        public System.Windows.Forms.SplitContainer splitContainer1;
        public CyWidgetPropertyUnit UnitPropertyGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTouchpadName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumberOfTouchpadRows;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTouchpadNumberOfColumns;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowsResolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumnsResolution;
        #endregion
    }
}

