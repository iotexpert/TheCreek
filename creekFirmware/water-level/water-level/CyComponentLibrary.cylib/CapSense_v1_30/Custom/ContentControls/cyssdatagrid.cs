/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CapSense_v1_30
{
    public partial class CySSDataGrid : DataGridView
    {
        CheckedListBox m_LBTerminalList;
        CyElScanSlot m_curScanSlot;

        public EventHandler m_actSaveChanges;
        ToolStripControlHost host;
        ToolStripDropDown m_popup;

        public int m_MaxCountInList = 6;
        volatile bool m_ListIsOpen = false;


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
            //host.Dispose();
            //m_popup.Dispose();
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

        public CySSDataGrid()
        {
            InitializeComponent();

            if (CyMyICyParamEditTemplate.m_RunMode)
            //Is Run Time Mode
            {
                this.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    this.colScanOrder,
                    this.colType,
                    this.colAssociatedTerminals});

                colScanOrder.SortMode = DataGridViewColumnSortMode.Programmatic;
                colAssociatedTerminals.SortMode = DataGridViewColumnSortMode.Programmatic;
                this.DoubleBuffered = true;

                //Creating CheckListBox
                m_LBTerminalList = new CheckedListBox();
                m_LBTerminalList.CheckOnClick = true;
                m_LBTerminalList.MaximumSize = new Size(300, 300);

                host = new ToolStripControlHost(m_LBTerminalList);
                host.Margin = Padding.Empty;
                host.Padding = Padding.Empty;
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                this.CellPainting += new DataGridViewCellPaintingEventHandler(cyDGScanSlot_CellPainting);
                this.m_actSaveChanges += new EventHandler(SaveChanges);
                this.CellLeave += new DataGridViewCellEventHandler(CySSDataGrid_CellLeave);
            } 
        }

        void CySSDataGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (CellDoesntContainsCorrectWidget(e.RowIndex, e.ColumnIndex)==false)
                m_first_open = true;
        }

        private void ShowList(MouseEventArgs e, int RowIndex, int ColIndex)
        {
            //Loacation
            Point pt = new Point();
            pt = this.GetCellDisplayRectangle(ColIndex, RowIndex, false).Location;
            pt.Y = pt.Y + this[ColIndex, RowIndex].Size.Height*2-2;

            //Create popup
            m_popup = new ToolStripDropDown();
            m_popup.Margin = Padding.Empty;
            m_popup.Padding = Padding.Empty;
            m_popup.Items.Add(host);
            m_popup.Closed += new ToolStripDropDownClosedEventHandler(HideList);

            m_ListIsOpen = true;
            m_popup.Show(this.Parent, pt, ToolStripDropDownDirection.BelowRight);
        }
        bool m_first_open = true;
        public void this_MouseDown(object sender, MouseEventArgs e, E_EL_SIDE side, List<CyElTerminal> listTerm)
        {
            if (m_ListIsOpen == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int RowIndex = this.HitTest(e.X, e.Y).RowIndex;
                    int ColIndex = this.HitTest(e.X, e.Y).ColumnIndex;

                    if (CellDoesntContainsCorrectWidget(RowIndex, ColIndex)) return;

                    //If first Open
                    if (this[ColIndex, RowIndex] == this.CurrentCell)
                    {
                        if (m_first_open == false)
                        {
                            m_first_open = true; return;
                        }
                    }
                    m_first_open = false;

                    m_curScanSlot = (CyElScanSlot)this[ColIndex, RowIndex].Value;

                    //Load terminals to list
                    m_LBTerminalList.Items.Clear();
                    foreach (CyElTerminal item in listTerm)
                        if (CySensorType.IsNotWidgetLabel(item))//Not Header
                            if (item.m_haveSide == side)
                            {
                                foreach (CyElTerminal item_int in m_curScanSlot.m_listTerminals)
                                    if (item == item_int)
                                    {
                                        m_LBTerminalList.Items.Add(item, true);
                                        continue;
                                    }
                                if (!m_LBTerminalList.Items.Contains(item)) m_LBTerminalList.Items.Add(item, false);
                            }

                    ShowList(e, RowIndex, ColIndex);
                }
            }
        }

        private bool CellDoesntContainsCorrectWidget(int RowIndex, int ColIndex)
        {
            if (RowIndex == -1) return true;

            if (ColIndex != colAssociatedTerminals.Index)  return true;

            //widget validating
            CyElWidget widg = (CyElWidget)this[colType.Index, RowIndex].Value;
            if (!CySensorType.IsCustomCase(widg.m_type)) return true;

            return false;
        }

        void HideList(object sender, EventArgs e)
        {
            if (m_ListIsOpen)
            {
                if (m_curScanSlot != null)
                {
                    CyElTerminal termHead = m_curScanSlot.m_listTerminals[0];
                    m_curScanSlot.m_listTerminals.Clear();
                    m_curScanSlot.m_listTerminals.Add(termHead);
                    for (int i = 0; i < m_LBTerminalList.Items.Count; i++)
                        if (m_LBTerminalList.GetItemChecked(i))
                        {
                            m_curScanSlot.m_listTerminals.Add((CyElTerminal)m_LBTerminalList.Items[i]);
                        }

                    UpdateSS();
                    m_curScanSlot = null;
                    m_actSaveChanges(null, null);
                }
                m_ListIsOpen = false;
            }
        }
        public void EraseTerminalList(int SelectedColIndex, int SelectedRowIndex)
        {
            if (m_ListIsOpen == false)
            {
                if (m_curScanSlot != null)
                {
                    for (int i = 0; i < m_LBTerminalList.Items.Count; i++)
                        m_LBTerminalList.SetItemCheckState(i, CheckState.Unchecked);
                    m_LBTerminalList.Width = this[SelectedColIndex, SelectedRowIndex].Size.Width;
                }
            }
        }
        #region Actions
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
        void UpdateSS()
        {
            //Updating column width
            colAssociatedTerminals.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            colAssociatedTerminals.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.Invalidate();
        }
        void SaveChanges(object sender, EventArgs e) { }
        #endregion
    }

}
