/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CapSense_CSD_v2_0
{
    public partial class CySSDataGrid : DataGridView
    {
        #region Header
        CheckedListBox m_terminalList;
        CyScanSlot m_curScanSlot;

        public EventHandler m_actSaveChanges;
        ToolStripControlHost m_host;
        ToolStripDropDown m_popup;

        bool m_first_open = true;
        volatile bool m_listIsOpen = false;
        const int COL_SCAN_ORDER = 0;
        const int COL_ASSOCIATED_TERMINALS = 1;

        public CySSDataGrid()
            : base()
        {
            this.AllowDrop = true;
            this.DoubleBuffered = true;
            this.AllowUserToAddRows = false;
            this.AllowUserToResizeRows = false;
            this.AllowUserToResizeColumns = false;
            this.RowHeadersVisible = false;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;

            this.CellPainting += new DataGridViewCellPaintingEventHandler(CySSDataGrid_CellPainting);
            this.m_actSaveChanges += new EventHandler(SaveChanges);
            this.CellLeave += new DataGridViewCellEventHandler(CySSDataGrid_CellLeave);
         
            //Creating CheckListBox
            m_terminalList = new CheckedListBox();
            m_terminalList.CheckOnClick = true;
            m_terminalList.MaximumSize = new Size(300, 300);

            m_host = new ToolStripControlHost(m_terminalList);
            m_host.Margin = Padding.Empty;
            m_host.Padding = Padding.Empty;            
        }        

        #endregion

        #region Functions
        public void MouseDownFunction(object sender, MouseEventArgs e, List<CyTerminal> listTerm)
        {
            if (m_listIsOpen == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int RowIndex = this.HitTest(e.X, e.Y).RowIndex;
                    int ColIndex = this.HitTest(e.X, e.Y).ColumnIndex;

                    if (CellContainsCustomWidget(RowIndex, ColIndex) == false)
                        return;
                    
                    //If second click on same Cell
                    if (m_first_open == false && this[ColIndex, RowIndex] == this.CurrentCell)
                    {
                        m_first_open = true;
                        return;
                    }
                    m_first_open = false;

                    m_curScanSlot = (CyScanSlot)this[ColIndex, RowIndex].Value;

                    //Load terminals to list
                    m_terminalList.Items.Clear();
                    foreach (CyTerminal term in listTerm)
                    {
                        bool isChecked = m_curScanSlot.GetTerminals().Contains(term);
                        if (m_curScanSlot.WidgetName != term.WidgetName)
                            m_terminalList.Items.Add(term, isChecked);
                    }
                    ShowList(e, RowIndex, ColIndex);
                }
            }
        }

        private void ShowList(MouseEventArgs e, int RowIndex, int ColIndex)
        {
            //Loacation
            Point pt = new Point();
            pt = this.GetCellDisplayRectangle(ColIndex, RowIndex, false).Location;
            pt.Y = pt.Y + this[ColIndex, RowIndex].Size.Height;

            //Create popup
            m_popup = new ToolStripDropDown();
            m_popup.Items.Add(m_host);
            m_popup.Closed += new ToolStripDropDownClosedEventHandler(HideList);

            m_listIsOpen = true;
            m_popup.Show(this.Parent, pt, ToolStripDropDownDirection.BelowRight);
        }

        void HideList(object sender, EventArgs e)
        {
            if (m_listIsOpen)
            {
                if (m_curScanSlot != null)
                {
                    m_curScanSlot.ClearAllTerminals();
                    for (int i = 0; i < m_terminalList.Items.Count; i++)
                        if (m_terminalList.GetItemChecked(i))
                        {
                            m_curScanSlot.AddTerminal((CyTerminal)m_terminalList.Items[i]);
                        }

                    this.Refresh();
                    m_curScanSlot = null;
                    m_actSaveChanges(null, null);
                }
                m_listIsOpen = false;
            }
        }
        public void EraseTerminalList(CyScanSlot ss)
        {
            if (ss == null) return;
            //Remove Terminals from scanslot
            ss.ClearAllTerminals();
            //Clear checkbox list
            for (int i = 0; i < m_terminalList.Items.Count; i++)
                m_terminalList.SetItemCheckState(i, CheckState.Unchecked);
            this.Refresh();
        }

        private bool CellContainsCustomWidget(int RowIndex, int ColIndex)
        {
            try
            {
                if (RowIndex == -1) return false;

                object obj = this[ColIndex, RowIndex].Value;
                if (obj == null || (obj is CyScanSlot) == false) return false;

                CyScanSlot ss = (CyScanSlot)obj;

                return CyCsConst.HasComplexScanSlot(ss.WidgetType);
            }
            catch
            {
                return false;
            }
        }
        
        #endregion

        #region Actions

        void CySSDataGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (CellContainsCustomWidget(e.RowIndex, e.ColumnIndex) == false)
                return;

            e.PaintBackground(e.CellBounds, true);

            Rectangle r = e.CellBounds;
            r.Width = e.CellBounds.Height;
            r.Height = e.CellBounds.Height;
            r.X += e.CellBounds.Width - e.CellBounds.Height;
            r.Y += 0;
            e.PaintContent(e.CellBounds);
            ControlPaint.DrawComboButton(e.Graphics, r, ButtonState.Normal);

            //Clarify Error Text for Cell
            if ((this[e.ColumnIndex, e.RowIndex].Value as CyScanSlot).IsEmptyComplexScanSlot())            
                this[e.ColumnIndex, e.RowIndex].ErrorText = String.Format(CyCsResource.EmptyScanSlot, "");            
            else if (this[e.ColumnIndex, e.RowIndex].ErrorText != string.Empty)
                this[e.ColumnIndex, e.RowIndex].ErrorText = string.Empty;

            if (this[e.ColumnIndex, e.RowIndex].ErrorText != string.Empty)
                e.PaintContent(e.CellBounds);
            e.Handled = true;
        }

        void CySSDataGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (CellContainsCustomWidget(e.RowIndex, e.ColumnIndex))
                m_first_open = true;
        }
        void SaveChanges(object sender, EventArgs e) { }
        #endregion
    }
}

