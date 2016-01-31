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

namespace CapSense_v1_30
{
    public partial class CySlidersTab : CyMyICyParamEditTemplate
    {
        public CySlidersTab()
            : base()
        {
            InitializeComponent();
            this.colType.Items.Clear();
            this.colType.Items.AddRange(CyGeneralParams.STR_SLIDER_TYPE);
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
            this.dgSliders = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.UnitPropertyGrid = new CyWidgetPropertyUnit();
            this.colSliderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colNumberOfSliderElements = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSliderDiplexing = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgSliders)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            //
            // dgSliders
            //
            this.dgSliders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSliders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSliderName,
            this.colType,
            this.colNumberOfSliderElements,
            this.colResolution,
            this.colSliderDiplexing});
            this.dgSliders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSliders.Location = new System.Drawing.Point(0, 0);
            this.dgSliders.Name = "dgSliders";
            this.dgSliders.Size = new System.Drawing.Size(603, 135);
            this.dgSliders.TabIndex = 1;
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
            this.splitContainer1.Panel1.Controls.Add(this.dgSliders);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.UnitPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(603, 231);
            this.splitContainer1.SplitterDistance = 135;
            this.splitContainer1.TabIndex = 3;
            //
            // UnitPropertyGrid
            //
            this.UnitPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnitPropertyGrid.IsTouchPadMode = false;
            this.UnitPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.UnitPropertyGrid.Name = "UnitPropertyGrid";
            this.UnitPropertyGrid.Size = new System.Drawing.Size(603, 92);
            this.UnitPropertyGrid.TabIndex = 0;
            //
            // colSliderName
            //
            this.colSliderName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSliderName.HeaderText = "Slider Name";
            this.colSliderName.Name = "colSliderName";
            //
            // colType
            //
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colType.HeaderText = "Type";
            this.colType.Items.AddRange(new object[] {
            "Linear",
            "Radial"});
            this.colType.Name = "colType";
            this.colType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colType.Width = 56;
            //
            // colNumberOfSliderElements
            //
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.colNumberOfSliderElements.DefaultCellStyle = dataGridViewCellStyle1;
            this.colNumberOfSliderElements.HeaderText = "Number of Elements";
            this.colNumberOfSliderElements.MaxInputLength = 2;
            this.colNumberOfSliderElements.Name = "colNumberOfSliderElements";
            this.colNumberOfSliderElements.Width = 80;
            //
            // colResolution
            //
            this.colResolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colResolution.HeaderText = "Resolution";
            this.colResolution.Name = "colResolution";
            this.colResolution.Width = 82;
            //
            // colSliderDiplexing
            //
            this.colSliderDiplexing.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colSliderDiplexing.HeaderText = "Diplexing";
            this.colSliderDiplexing.Name = "colSliderDiplexing";
            this.colSliderDiplexing.Width = 56;
            //
            // CySliders
            //
            this.Controls.Add(this.splitContainer1);
            this.Name = "CySliders";
            this.Size = new System.Drawing.Size(603, 231);
            ((System.ComponentModel.ISupportInitialize)(this.dgSliders)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgSliders;
        public System.Windows.Forms.SplitContainer splitContainer1;
        public CyWidgetPropertyUnit UnitPropertyGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSliderName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumberOfSliderElements;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResolution;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSliderDiplexing;
        #endregion
    }
}

