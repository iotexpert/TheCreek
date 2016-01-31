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

namespace  CapSense_v1_10
{
    public partial class CyDGScanSlot : DataGridView
    {
        CheckedListBox cLBMain;
        CyElScanSlot m_curScanSlot;
        //public Control main;
        int m_CurRow = -1;
        int m_iMaxListItems = 6;
        bool m_HideProcessing = false;//To synchronize mHide actions
        public EventHandler m_actSaveChanges;

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

            if (CyMyICyParamEditTemplate.m_FillAll > 0)
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
            this.m_actSaveChanges += new EventHandler(SaveChanges);
        }
        void SaveChanges(object sender, EventArgs e) { }
        public void this_MouseUp(object sender, MouseEventArgs e, E_EL_SIDE side, List<CyElTerminal> listTerm)
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

            if (cLBMain.Visible == true) { MyHide(); return; }

            if (RowIndex == -1) { MyHide(); return; }

            if (ColIndex != colAssociatedTerminals.Index) { MyHide(); return; }

            CyElWidget widg = (CyElWidget)this[colType.Index, RowIndex].Value;
            if (!CySensorType.IsCustomCase(widg.m_type)) { MyHide(); return; }


            m_curScanSlot = (CyElScanSlot)this[ColIndex, RowIndex].Value;

            //OPerations for correct Show
            cLBMain.Items.Clear();
            foreach (CyElTerminal item in listTerm)
                if (CySensorType.IsNotWidgetLabel(item))//Not Header
                    if (item.m_haveSide == side)
                    {
                        foreach (CyElTerminal item_int in m_curScanSlot.m_listTerminals)
                            if (item == item_int)
                            {
                                cLBMain.Items.Add(item, true);
                                continue;
                            }
                        if (!cLBMain.Items.Contains(item)) cLBMain.Items.Add(item, false);
                    }
            m_CurRow = RowIndex;

            //Calulate cLBMain Bounds
            //cLBMain.Width = this[ColIndex, RowIndex].Size.Width;
            cLBMain.Width = 300;

            //Calculate cLBMain Height
            int iCount = cLBMain.Items.Count;
            if (cLBMain.Items.Count > m_iMaxListItems)
                iCount = m_iMaxListItems;
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
            if (!m_HideProcessing)
            {
                if (m_curScanSlot != null)
                {
                    for (int i = 0; i < cLBMain.Items.Count; i++)
                        cLBMain.SetItemCheckState(i, CheckState.Unchecked);
                    cLBMain.Width = this[SelectedColIndex, SelectedRowIndex].Size.Width;
                }
            }
        }
        public void MyHide()
        {
            if (!m_HideProcessing)
            {
                m_HideProcessing = true;//Block action
                if (m_curScanSlot != null)
                {
                    CyElTerminal termHead = m_curScanSlot.m_listTerminals[0];
                    m_curScanSlot.m_listTerminals.Clear();
                    m_curScanSlot.m_listTerminals.Add(termHead);
                    for (int i = 0; i < cLBMain.Items.Count; i++)
                        if (cLBMain.GetItemChecked(i))
                        {
                            m_curScanSlot.m_listTerminals.Add((CyElTerminal)cLBMain.Items[i]);
                        }

                    cLBMain.Visible = false;
                    UpdateSS(m_CurRow);
                    m_curScanSlot = null;
                    m_CurRow = -1;
                    m_actSaveChanges(null, null);
                }
                m_HideProcessing = false;
            }
        }
        void UpdateSS(int RowIndex)
        {
            //Updating column width
            colAssociatedTerminals.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            colAssociatedTerminals.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            this.Invalidate();
        }
        #region Actions
        void cyCellCheckListBox_Leave(object sender, EventArgs e)
        {
            MyHide();
        }

        void cyDGScanSlot_SizeChanged(object sender, EventArgs e)
        {
            MyHide();
        }
        void CyDGScanSlot_Scroll(object sender, ScrollEventArgs e)
        {
            MyHide();
        }
        void cyDGScanSlot_Leave(object sender, EventArgs e)
        {
            if (!cLBMain.Focused)
                MyHide();
        }

        void cyDGScanSlot_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != colAssociatedTerminals.Index) return;

            CyElWidget widg = (CyElWidget)this[colType.Index, e.RowIndex].Value;
            if (!CySensorType.IsCustomCase(widg.m_type)) { return; }

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
