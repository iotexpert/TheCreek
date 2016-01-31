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
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace  CapSense_v1_20
{
    public partial class CyScanSlotsTab : CyMyICyParamEditTemplate
    {
        #region Header
        CyGeneralParams m_packParams;
        DataGridView m_dgButtons;
        DataGridView m_dgSliders;
        DataGridView m_dgTouchpads;
        DataGridView m_dgMatrixButtons;
        DataGridView m_dgProximity;
        DataGridView m_dgGeneric;
        string m_strlCSNone = "None";

        public CyScanSlotsTab(CyGeneralParams packParams)
            : base()
        {
            InitializeComponent();
            this.m_packParams = packParams;
            m_dgButtons = packParams.m_cyButtonsTab.dgButtons;
            m_dgSliders = packParams.m_cySlidersTab.dgSliders;            
            m_dgTouchpads = packParams.m_cyTouchPadsTab.dgTouchpads;
            m_dgMatrixButtons = packParams.m_cyMatrixButtonsTab.dgMatrixButtons;
            m_dgProximity = packParams.m_cyProximityTab.dgProximity;
            m_dgGeneric = packParams.m_cyGenericTab.dgGeneric;
            AssigneActions();
            dgScanSlotsL.AllowUserToOrderColumns = false;

            LabelHeadRight.Text = "Right " + CyGeneralParams.m_strABHead;
            LabelHeadRight.BackColor = CyGeneralParams.m_ColorRight;
            LabelHeadLeft.BackColor = CyGeneralParams.m_ColorLeft;

            //For correct datagrid Color
            ChangeIterColor();

            EraseSSLabel(E_EL_SIDE.Left);
            EraseSSLabel(E_EL_SIDE.Right);

            //Assign images
            this.cmdPromoteL.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_20.Properties.Resources.ResourceManager.GetObject("cmdpromote")));
            this.cmdPromoteR.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_20.Properties.Resources.ResourceManager.GetObject("cmdpromote")));
            this.cmdDemoteL.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_20.Properties.Resources.ResourceManager.GetObject("cmddemote")));
            this.cmdDemoteR.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_20.Properties.Resources.ResourceManager.GetObject("cmddemote")));

            packParams.m_localParams.m_actGlobalParamsChange += 
                new EventHandler(ActGlobalParamsChange);
            dgScanSlotsL.m_actSaveChanges += new EventHandler(m_packParams.SetCommitParams);
            dgScanSlotsR.m_actSaveChanges += new EventHandler(m_packParams.SetCommitParams);
            SSPropsLeft.SetSavedHandler(new EventHandler(m_packParams.SetCommitParams));
            SSPropsRight.SetSavedHandler(new EventHandler(m_packParams.SetCommitParams));
        }

        private void CyScanSlots_Load(object sender, EventArgs e)
        {
            sCLeftHalf.SplitterDistance = sCLeftHalf.Height - 118;
            sCRightHalf.SplitterDistance = sCRightHalf.Height - 118;
            sCMainSS.SplitterDistance = sCMainSS.Width / 2;            
            UpdateDGRowsEnter();
        }
        #endregion

        #region dgScanSlots
        private void dgScanSlots_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Delete) || (e.KeyData == Keys.Back))
            {
                EraseTerminalList(sender);
            }
            else if (e.KeyData == Keys.Add)
            {
                DemoteRow(sender);
            }
            else if (e.KeyData == Keys.Subtract)
            {
                PromoteRow(sender);
            }
        }

        private void dgScanSlots_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
                DataGridView dgInput = (DataGridView)sender;
                E_EL_SIDE side = GetSide(sender);
                if (GetSelectedRowsCount(sender) > 0)
                {
                    GetSSProperties(side).Clear();

                    int SelectedSSIndex = GetSSIndex(GetSelectedSSByIndex(sender, 0), dgInput);

                    //Set Label for side
                    if (dgInput.SelectedCells.Count > 1) GetSSLabel(side).Text = "";
                    else
                        GetSSLabel(side).Text = m_packParams.m_cyScanSlotsList.
                            GetSSList(side)[SelectedSSIndex].m_listTerminals[0].ToString();

                    //Set Properties
                    for (int i = 0; i < GetSelectedRowsCount(sender); i++)
                    {
                        SelectedSSIndex = GetSSIndex(GetSelectedSSByIndex(sender, i), dgInput);
                        GetSSProperties(side).AddProperties(m_packParams.m_cyScanSlotsList.
                            GetSSList(side)[SelectedSSIndex]);
                    }
                    GetSSProperties(side).SetObject();
                }
        }

        private void EraseTerminalList(object sender)
        {
            CySSDataGrid dgInput = (CySSDataGrid)sender;
            if (dgInput.SelectedCells.Count > 0)
            {
                int SelectedRowIndex = dgInput.SelectedCells[0].RowIndex;
                int SelectedColIndex = dgInput.SelectedCells[0].ColumnIndex;
                
                if (SelectedColIndex == dgInput.colAssociatedTerminals.Index)
                {   
                    m_packParams.m_cyScanSlotsList.RemoveAllTerminalsFromSS
                        (GetSSIndex(SelectedRowIndex, dgInput), GetSide(sender));                                 
                    dgInput.EraseTerminalList(SelectedColIndex, SelectedRowIndex);
                    dgInput.Update();
                    dgInput.Invalidate();
                }
            }
        }

        //Get selected Items by Index
        int GetSelectedSSByIndex(object sender, int _index)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
                return dgInput.SelectedRows[_index].Index;
            else return dgInput.SelectedCells[_index].RowIndex;
        }

        //Get Count of selected Items
        int GetSelectedRowsCount(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
                return dgInput.SelectedRows.Count;
            else return dgInput.SelectedCells.Count;
        }
        public List<int> GetSelectedRows(object sender, bool reverse)
        {
            DataGridView dgInput = (DataGridView)sender;
            List<int> SelectedRowIndexes = new List<int>();
            if (dgInput.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow item in dgInput.SelectedRows)
                {
                    SelectedRowIndexes.Add(GetSSIndex(item.Index, dgInput));
                }
            }
            else if (dgInput.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell item in dgInput.SelectedCells)
                {
                    int _index = GetSSIndex(item.RowIndex, dgInput);
                    if (SelectedRowIndexes.Contains(_index) == false)
                        SelectedRowIndexes.Add(_index);
                }
            }
            SelectedRowIndexes.Sort();
            if (reverse)
                SelectedRowIndexes.Reverse();
            
            return SelectedRowIndexes;
        }

        private void DemoteRow(object sender)
        {
            //Use only first value
            if (GetSelectedRowsCount(sender) > 0)
                (sender as DataGridView).CurrentCell =(sender as DataGridView)[0,GetSelectedRows(sender,false)[0]];

            foreach (int item in GetSelectedRows(sender,true))
            {
                m_packParams.m_cyScanSlotsList.DemoteWidget(item, GetSide(sender));
            }
            UpdateDGRowsOrder(sender);

            //Commit Parameters
            m_packParams.SetCommitParams(null, null);
        }
        private void PromoteRow(object sender)
        {
            //Use only first value
            if (GetSelectedRowsCount(sender) > 0)
                (sender as DataGridView).CurrentCell = (sender as DataGridView)[0, GetSelectedRows(sender, false)[0]];

            foreach (int item in GetSelectedRows(sender,false))
            {
                m_packParams.m_cyScanSlotsList.PromoteWidget(item, GetSide(sender));
            }

            UpdateDGRowsOrder(sender);
            //Commit Parameters
            m_packParams.SetCommitParams(null, null);
        }

        #region RowComparer
        private class RowComparer : System.Collections.IComparer
        {
            private static int sortOrderModifier1 = 1;
            int col1;

            public RowComparer(int col1, SortOrder sortOrder1)
            {
                if (sortOrder1 == SortOrder.Descending)
                {
                    sortOrderModifier1 = -1;
                }
                this.col1 = col1;
            }

            public int Compare(object x, object y)
            {
                DataGridViewRow DataGridViewRow1 = (DataGridViewRow)x;
                DataGridViewRow DataGridViewRow2 = (DataGridViewRow)y;

                // Try to sort based on the Last Name column.
                int CompareResult = 0;
                int f1 = Convert.ToInt32(DataGridViewRow1.Cells[col1].Value.ToString());
                int f2 = Convert.ToInt32(DataGridViewRow2.Cells[col1].Value.ToString());
                if (f1 > f2) CompareResult = 1;
                if (f1 < f2) CompareResult = -1;
                return CompareResult * sortOrderModifier1;
            }
        }
        #endregion

        private void cmdPromoteL_Click(object sender, EventArgs e)
        {
            PromoteRow(dgScanSlotsL);
        }

        private void cmdDemoteL_Click(object sender, EventArgs e)
        {
            DemoteRow(dgScanSlotsL);
        }
        private void cmdPromoteR_Click(object sender, EventArgs e)
        {
            PromoteRow(dgScanSlotsR);
        }
        private void cmdDemoteR_Click(object sender, EventArgs e)
        {
            DemoteRow(dgScanSlotsR);
        }

        #endregion

        #region SSProperties
        Label GetSSLabel(E_EL_SIDE side)
        {
            Label curLabel = lCSLeft;
            if (side == E_EL_SIDE.Right) { curLabel = lCSRight; }
            return curLabel;
        }
        ContentControls.CySSPropertyUnit GetSSProperties(E_EL_SIDE side)
        {
            ContentControls.CySSPropertyUnit curectProps = SSPropsLeft;
            if (side == E_EL_SIDE.Right) { curectProps = SSPropsRight; }
            return curectProps;
        }
        void EraseSSLabel(E_EL_SIDE side)
        {
            Label curLabel = GetSSLabel(side);
            ContentControls.CySSPropertyUnit curectProps = GetSSProperties(side);
            curLabel.Text = m_strlCSNone;
            curectProps.Visible = false;
        }
        private void dgScanSlots_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateDGRowsColor(sender);
            if (((DataGridView)sender).RowCount != 0) GetSSProperties(GetSide(sender)).Visible = true;
        }
        private void dgScanSlots_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateDGRowsColor(sender);
            if (((DataGridView)sender).RowCount == 0) EraseSSLabel(GetSide(sender));
        }
        void dgScanSlots_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            (sender as CySSDataGrid).this_MouseDown(sender, e, GetSide(sender),
                m_packParams.m_cyWidgetsList.m_listTerminal);

        }
        #endregion
    }
}

