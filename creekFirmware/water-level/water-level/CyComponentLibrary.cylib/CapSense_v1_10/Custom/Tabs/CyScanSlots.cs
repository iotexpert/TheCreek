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

namespace  CapSense_v1_10
{
    public partial class CyScanSlots : CyMyICyParamEditTemplate
    {
        bool m_isLoading = false;

        #region Header
        CyGeneralParams m_packParams;
        DataGridView m_dgButtons;
        DataGridView m_dgSliders;
        DataGridView m_dgTouchpads;
        DataGridView m_dgMatrixButtons;
        DataGridView m_dgProximity;
        DataGridView m_dgGeneric;
        string m_strlCSNone = "None";
        string m_strlCSMulti = "Multi Selection";


        public CyScanSlots(CyGeneralParams packParams)
            : base()
        {
            InitializeComponent();
            this.m_packParams = packParams;
            m_dgButtons = packParams.m_cyButtons.dgButtons;
            m_dgSliders = packParams.m_cySliders.dgSliders;            
            m_dgTouchpads = packParams.m_cyTouchPads.dgTouchpads;
            m_dgMatrixButtons = packParams.m_cyMatrixButtons.dgMatrixButtons;
            m_dgProximity = packParams.m_cyProximity.dgProximity;
            m_dgGeneric = packParams.m_cyGeneric.dgGeneric;
            AssigneActions();
            dgScanSlotsL.AllowUserToOrderColumns = false;

            LabelHeadRight.Text = "Right " + CyGeneralParams.m_strABHead;
            LabelHeadRight.BackColor = CyGeneralParams.m_ColorRight;
            LabelHeadLeft.BackColor = CyGeneralParams.m_ColorLeft;

            //For correct datagrid Color
            ChangeIterColor();

            lCSSetNoneSelection(E_EL_SIDE.Left);
            lCSSetNoneSelection(E_EL_SIDE.Right);

            //Assign images
            object obj= global:: CapSense_v1_10.ResAll.cmdDemote;
            this.cmdPromoteL.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_10.ResAll.ResourceManager.GetObject("cmdPromote")));
            this.cmdPromoteR.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_10.ResAll.ResourceManager.GetObject("cmdPromote")));
            this.cmdDemoteL.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_10.ResAll.ResourceManager.GetObject("cmdDemote")));
            this.cmdDemoteR.Image = ((System.Drawing.Image)(
                global:: CapSense_v1_10.ResAll.ResourceManager.GetObject("cmdDemote")));

            packParams.m_localParams.m_actGlobalParamsChange += 
                new CyLocalParams.m_NullDelegate(ActGlobalParamsChange);
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
            splitContainer4.Panel2Collapsed = true;
            UpdateDGRowsEnter();
        }
        #endregion

        #region dgScanSlots
        private void dgScanSlots_KeyDown(object sender, KeyEventArgs e)
        {
            int data = (int)e.KeyData;

            if ((e.KeyData == Keys.Delete) || (e.KeyData == Keys.Back))
            {
                DeleteCell(sender);
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
            if (!m_isLoading)
            {
                DataGridView dgInput = (DataGridView)sender;
                if (LocalGetSelectRowsCount(sender) > 0)
                {
                    SSPropsFromSide(GetSide(sender)).Clear();

                    int SelectedRowIndex = GetSSIndex(LocalGetSelectItemsByIndex(sender,0), sender);

                    //Set Label for lCS
                    if (dgInput.SelectedCells.Count > 1) lCSGetFromSide(GetSide(sender)).Text = m_strlCSMulti;
                    else
                        lCSGetFromSide(GetSide(sender)).Text = m_packParams.m_cyScanSlotsList.
                            GetListFromSide(GetSide(sender))[GetSSIndex(SelectedRowIndex, sender)]
                            .m_listTerminals[0].ToString();

                    //Set Properties
                    for (int i = 0; i < LocalGetSelectRowsCount(sender); i++)
                    {
                        SelectedRowIndex = GetSSIndex(LocalGetSelectItemsByIndex(sender, i), sender);
                        SSPropsFromSide(GetSide(sender)).AddProps(m_packParams.m_cyScanSlotsList.
                            GetListFromSide(GetSide(sender))[GetSSIndex(SelectedRowIndex, sender)]);
                    }
                    SSPropsFromSide(GetSide(sender)).SetObject();
                }
            }

        }

        //Get Count of selected Items
        int LocalGetSelectRowsCount(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
                return dgInput.SelectedRows.Count;
            else return dgInput.SelectedCells.Count;
        }
        //Get selected Items by Index
        int LocalGetSelectItemsByIndex(object sender,int i)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
                return dgInput.SelectedRows[i].Index;
            else return dgInput.SelectedCells[i].RowIndex;
        }


        private void DeleteCell(object sender)
        {
            CyDGScanSlot dgInput = (CyDGScanSlot)sender;
            if (dgInput.SelectedCells.Count > 0)
            {
                int SelectedRowIndex = dgInput.SelectedCells[0].RowIndex;
                int SelectedColIndex = dgInput.SelectedCells[0].ColumnIndex;
                
                if (SelectedColIndex == dgInput.colAssociatedTerminals.Index)
                {   
                    m_packParams.m_cyScanSlotsList.RemoveAllTerminalsAtSS
                        (GetSSIndex(SelectedRowIndex, sender), GetSide(sender));
                    UpdateSS(SelectedRowIndex, sender);
                    dgInput.DeleteCell(SelectedColIndex, SelectedRowIndex);
                }
                else
                {
                    MessageBox.Show("You can not delete this cell.");
                }
            }
        }


        private void DemoteRow(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
            {
                int SelectedRowIndex = GetSSIndex(dgInput.SelectedRows[0].Index, sender);

                if (SelectedRowIndex < (dgInput.RowCount - 1))
                {
                    m_packParams.m_cyScanSlotsList.DemoteWidget(SelectedRowIndex, GetSide(sender));
                }
            }
            else if (dgInput.SelectedCells.Count > 0)
            {
                int SelectedRowIndex = GetSSIndex(dgInput.SelectedCells[0].RowIndex, sender);

                if (SelectedRowIndex < (dgInput.RowCount - 1))
                {
                    m_packParams.m_cyScanSlotsList.DemoteWidget(SelectedRowIndex, GetSide(sender));
                }
            }
            UpdateDGRowsOrder(sender);
            //Commit Parameters
            m_packParams.SetCommitParams(null, null);
        }

        private void PromoteRow(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
            {
                int SelectedRowIndex = GetSSIndex(dgInput.SelectedRows[0].Index, sender);

                if (SelectedRowIndex > 0)
                {
                    for (int i = 0; i < dgInput.SelectedRows.Count; i++)
                    {
                        SelectedRowIndex = GetSSIndex(dgInput.SelectedRows[i].Index, sender);

                        m_packParams.m_cyScanSlotsList.PromoteWidget(SelectedRowIndex, GetSide(sender));
                    }
                }
            }
            else if (dgInput.SelectedCells.Count > 0)
            {
                int SelectedRowIndex = GetSSIndex(dgInput.SelectedCells[0].RowIndex, sender);

                if (SelectedRowIndex > 0)
                {
                    for (int i = 0; i < dgInput.SelectedCells.Count; i++)
                    {
                        SelectedRowIndex = GetSSIndex(dgInput.SelectedCells[i].RowIndex, sender);
                        m_packParams.m_cyScanSlotsList.PromoteWidget(SelectedRowIndex, GetSide(sender));
                    }
                }
            }
            UpdateDGRowsOrder(sender);
            //Commit Parameters
            m_packParams.SetCommitParams(null, null);
        }

        #region RowComparer
        private class RowComparer : System.Collections.IComparer
        {
            private static int sortOrderModifier1 = 1;
            //private static int sortOrderModifier2 = 1;
            //private
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
                //System.String.Compare(DataGridViewRow1.Cells[col1].Value.ToString(),
                //      DataGridViewRow2.Cells[col1].Value.ToString());
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

        #region SSProps
        Label lCSGetFromSide(E_EL_SIDE side)
        {
            Label curLabel = lCSLeft;
            if (side == E_EL_SIDE.Right) { curLabel = lCSRight; }
            return curLabel;
        }
        ContentControls.CyCntSSProperties SSPropsFromSide(E_EL_SIDE side)
        {
            ContentControls.CyCntSSProperties curectProps = SSPropsLeft;
            if (side == E_EL_SIDE.Right) { curectProps = SSPropsRight; }
            return curectProps;
        }
        void lCSSetNoneSelection(E_EL_SIDE side)
        {
            Label curLabel = lCSGetFromSide(side);
            ContentControls.CyCntSSProperties curectProps = SSPropsFromSide(side);
            curLabel.Text = m_strlCSNone;
            curectProps.Visible = false;
        }
        private void dgScanSlots_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateDGRowsColors(sender);
            if (((DataGridView)sender).RowCount != 0) SSPropsFromSide(GetSide(sender)).Visible = true;
        }


        private void dgScanSlots_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateDGRowsColors(sender);
            if (((DataGridView)sender).RowCount == 0) lCSSetNoneSelection(GetSide(sender));
        }

        private void dgScanSlots_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as CyDGScanSlot).this_MouseUp(sender, e, GetSide(sender), 
                m_packParams.m_cyWidgetsList.m_listTerminal);
        }
        #endregion




    }
}

