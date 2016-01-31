/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace  CapSense_v0_5
{
    public partial class CyDGScanSlot : DataGridView
    {
        CheckedListBox cLBMain;
        ElScanSlot curScanSlot;
        //public Control main;
        int CurRow = -1;
        int iMaxListItems = 6;
        bool mHideProcessing = false;//To synchronize mHide actions

        #region Desinger Code
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
            components = new System.ComponentModel.Container();
            this.colScanOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssociatedTerminals = new System.Windows.Forms.DataGridViewTextBoxColumn();


            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            this.AllowDrop = true;
            this.AllowUserToAddRows = false;
            this.CausesValidation = false;
            // 
            // colScanOrder
            // 
            this.colScanOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colScanOrder.HeaderText = "Scan Order";
            this.colScanOrder.Name = "colScanOrder";
            this.colScanOrder.ReadOnly = true;
            this.colScanOrder.Width = 40;
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colType.HeaderText = "Widget Elements";
            this.colType.MinimumWidth = 25;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colType.Visible = false;
            this.colType.Width = 120;
            // 
            // colAssociatedTerminals
            // 
            this.colAssociatedTerminals.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAssociatedTerminals.HeaderText = "Associated Terminal(s)";
            this.colAssociatedTerminals.MinimumWidth = 25;
            this.colAssociatedTerminals.Name = "colAssociatedTerminals";
            this.colAssociatedTerminals.ReadOnly = true;
            this.colAssociatedTerminals.Width = 126;

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }


        #endregion
        public System.Windows.Forms.DataGridViewTextBoxColumn colScanOrder;
        public System.Windows.Forms.DataGridViewTextBoxColumn colType;
        public System.Windows.Forms.DataGridViewTextBoxColumn colAssociatedTerminals;
        #endregion


        public CyDGScanSlot()
        {
            InitializeComponent();

            if (M_ICyParamEditTemplate.FillAll > 0)
            {
                this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    this.colScanOrder,
                    this.colType,
                    this.colAssociatedTerminals});

                colScanOrder.SortMode = DataGridViewColumnSortMode.Programmatic;                
                colAssociatedTerminals.SortMode = DataGridViewColumnSortMode.Programmatic;

                //Creating CheckListBox
                cLBMain = new CheckedListBox();
                cLBMain.CheckOnClick = true;
                
                cLBMain.Visible = false;
                cLBMain.Leave += new EventHandler(cyCellCheckListBox_Leave);
            }
            this.SizeChanged += new EventHandler(cyDGScanSlot_SizeChanged);
            this.CellPainting += new DataGridViewCellPaintingEventHandler(cyDGScanSlot_CellPainting);
            this.Leave += new EventHandler(cyDGScanSlot_Leave);
            this.Scroll += new ScrollEventHandler(CyDGScanSlot_Scroll);
        }
        public void this_MouseUp(object sender, MouseEventArgs e, eElSide side, List<ElTerminal> listTerm)
        {
            if (!TopLevelControl.Controls.Contains(cLBMain))
            {
                TopLevelControl.Controls.Add(cLBMain);
            }
            Point pt;
            int RowIndex = -1;
            int ColIndex = -1;

            pt = new Point(e.X, e.Y);

            RowIndex = this.HitTest(pt.X, pt.Y).RowIndex;
            ColIndex = this.HitTest(pt.X, pt.Y).ColumnIndex;

            if (cLBMain.Visible == true) { mHide(); return; }

            if (RowIndex == -1) { mHide(); return; }

            if (ColIndex != colAssociatedTerminals.Index) { mHide(); return; }

            ElWidget widg = (ElWidget)this[colType.Index, RowIndex].Value;
            if (!cySensorType.IsCustomCase(widg.type)) { mHide(); return; }


            curScanSlot = (ElScanSlot)this[ColIndex, RowIndex].Value;

            //OPerations for correct Show
            cLBMain.Items.Clear();
            foreach (ElTerminal item in listTerm)
                if (!cySensorType.IsWidgetLabel(item))//Not Header
                    if (item.haveSide == side)
                    {
                        foreach (ElTerminal item_int in curScanSlot.listTerminals)
                            if (item == item_int)
                            {
                                cLBMain.Items.Add(item, true);
                                continue;
                            }
                        if (!cLBMain.Items.Contains(item)) cLBMain.Items.Add(item, false);
                    }
            CurRow = RowIndex;

            //Calulate cLBMain Bounds
            //cLBMain.Width = this[ColIndex, RowIndex].Size.Width;
            cLBMain.Width = 300;

            //Calculate cLBMain Height
            int iCount = cLBMain.Items.Count;
            if (cLBMain.Items.Count > iMaxListItems)
                iCount = iMaxListItems;
            cLBMain.Height = (iCount +1)* cLBMain.ItemHeight;

            //Loacation
            pt = this.GetCellDisplayRectangle(ColIndex, RowIndex, false).Location;
            pt.Y = pt.Y + this[ColIndex, RowIndex].Size.Height;
            pt.X = 0;

            //Bring to Normal Location
            pt = this.PointToScreen(pt);
            pt = TopLevelControl.PointToClient(pt);
            cLBMain.Location = pt;


            cLBMain.Visible = true;
            cLBMain.BringToFront();
        }
        public void DeleteCell(int SelectedColIndex, int SelectedRowIndex)
        {
            if (!mHideProcessing)
            {
                if (curScanSlot != null)
                {
                    for (int i = 0; i < cLBMain.Items.Count; i++)
                        cLBMain.SetItemCheckState(i, CheckState.Unchecked);
                    cLBMain.Width = this[SelectedColIndex, SelectedRowIndex].Size.Width;
                }
            }
        }
        public void mHide()
        {
            if (!mHideProcessing)
            {
                mHideProcessing = true;//Block action
                if (curScanSlot != null)
                {
                    ElTerminal termHead = curScanSlot.listTerminals[0];
                    curScanSlot.listTerminals.Clear();
                    curScanSlot.listTerminals.Add(termHead);
                    for (int i = 0; i < cLBMain.Items.Count; i++)
                        if (cLBMain.GetItemChecked(i))
                        {
                            curScanSlot.listTerminals.Add((ElTerminal)cLBMain.Items[i]);
                        }

                    cLBMain.Visible = false;
                    UpdateSS(CurRow);
                    curScanSlot = null;
                    CurRow = -1;
                }
                mHideProcessing = false;
            }
        }
        void UpdateSS(int RowIndex)
        {
            //if ((RowIndex > -1) && (RowIndex < this.RowCount))
            //{
            //    //this[colAssociatedTerminals.Index, RowIndex].Value = pack;
            //    this[colAssociatedTerminals.Index, RowIndex].Value = curScanSlot;
            //}
            //Updating column width
            colAssociatedTerminals.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            colAssociatedTerminals.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            this.Invalidate();
        }
        #region Actions
        void cyCellCheckListBox_Leave(object sender, EventArgs e)
        {
            mHide();
        }

        void cyDGScanSlot_SizeChanged(object sender, EventArgs e)
        {
            mHide();
        }
        void CyDGScanSlot_Scroll(object sender, ScrollEventArgs e)
        {
            mHide();
        }
        void cyDGScanSlot_Leave(object sender, EventArgs e)
        {
            if (!cLBMain.Focused)
                mHide();
        }

        void cyDGScanSlot_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != colAssociatedTerminals.Index) return;

            ElWidget widg = (ElWidget)this[colType.Index, e.RowIndex].Value;
            if (!cySensorType.IsCustomCase(widg.type)) { return; }

            e.PaintBackground(e.CellBounds, true);

            Rectangle r = e.CellBounds;
            r.Width = e.CellBounds.Height;
            r.Height = e.CellBounds.Height;
            r.X += e.CellBounds.Width - e.CellBounds.Height;
            r.Y += 0;            
            e.PaintContent(e.CellBounds);
            ControlPaint.DrawComboButton(e.Graphics, r, ButtonState.Flat);
            e.Handled = true;
            
        }

        #endregion

    }
}
