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

namespace  CapSense_v0_5
{
    public partial class CyScanSlots : M_ICyParamEditTemplate
    {
        bool isLoading = false;

        #region Header
        CyGeneralParams packParams;
        DataGridView dgButtons;
        DataGridView dgSliders;

        DataGridView dgTouchpads;
        DataGridView dgMatrixButtons;
        DataGridView dgProximity;
        DataGridView dgGeneric;
        string strlCSNone = "None";
        string strlCSMulti = "Multi Selection";


        public CyScanSlots(CyGeneralParams packParams)
            : base()
        {
            InitializeComponent();
            this.packParams = packParams;
            dgButtons = packParams.cyButtons.dgButtons;
            dgSliders = packParams.cySliders.dgSliders;            
            dgTouchpads = packParams.cyTouchPads.dgTouchpads;
            dgMatrixButtons = packParams.cyMatrixButtons.dgMatrixButtons;
            dgProximity = packParams.cyProximity.dgProximity;
            dgGeneric = packParams.cyGeneric.dgGeneric;
            AssigneActions();
            dgScanSlotsL.AllowUserToOrderColumns = false;

            LabelHeadRight.Text = "Right " + CyGeneralParams.strABHead;
            LabelHeadRight.BackColor = CyGeneralParams.ColorRight;
            LabelHeadLeft.BackColor = CyGeneralParams.ColorLeft;

            //Fore correct datagrid Color
            ChangeIterColor();

            lCSSetNoneSelection(eElSide.Left);
            lCSSetNoneSelection(eElSide.Right);

            //Assigne images
            object obj= global:: CapSense_v0_5.ResAll.cmdDemote;
            this.cmdPromoteL.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject("cmdPromote")));
            this.cmdPromoteR.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject("cmdPromote")));
            this.cmdDemoteL.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject("cmdDemote")));
            this.cmdDemoteR.Image = ((System.Drawing.Image)(global:: CapSense_v0_5.ResAll.ResourceManager.GetObject("cmdDemote")));

            packParams.localParams.actGlobalParamsChange += new CyLocalParams.m_NullDelegate(actGlobalParamsChange);
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
        //private void dgScanSlots_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    deleteSS(getSSIndex(e.RowIndex, sender), true, sender);
        //}
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
            if (!isLoading)
            {
                DataGridView dgInput = (DataGridView)sender;
                if (localGetSelectRowsCount(sender) > 0)
                {

                    SSPropsFromSide(GetSide(sender)).SSCSPropsSW.Clear();

                    int SelectedRowIndex = getSSIndex(localGetSelectItemsByIndex(sender,0), sender);

                    //Set Label for lCS
                    if (dgInput.SelectedCells.Count > 1) lCSGetFromSide(GetSide(sender)).Text = strlCSMulti;
                    else
                        lCSGetFromSide(GetSide(sender)).Text = packParams.cyScanSlotsList.GetListFromSide(GetSide(sender))[getSSIndex(SelectedRowIndex, sender)].listTerminals[0].ToString();

                    //Set Properties
                    for (int i = 0; i < localGetSelectRowsCount(sender); i++)
                    {
                        SelectedRowIndex = getSSIndex(localGetSelectItemsByIndex(sender, i), sender);
                        SSPropsFromSide(GetSide(sender)).SSCSPropsSW.AddProps(packParams.cyScanSlotsList.GetListFromSide(GetSide(sender))[getSSIndex(SelectedRowIndex, sender)]);
                    }
                    SSPropsFromSide(GetSide(sender)).SSCSPropsSW.SetObject();

                }
            }

        }

        //Get Count of selected Items
        int localGetSelectRowsCount(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
                return dgInput.SelectedRows.Count;
            else return dgInput.SelectedCells.Count;
        }
        //Get selected Items by Index
        int localGetSelectItemsByIndex(object sender,int i)
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
                    packParams.cyScanSlotsList.RemoveAllTerminalsAtSS(getSSIndex(SelectedRowIndex, sender), GetSide(sender));
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
                int SelectedRowIndex = getSSIndex(dgInput.SelectedRows[0].Index, sender);

                if (SelectedRowIndex < (dgInput.RowCount - 1))
                {
                    packParams.cyScanSlotsList.DemoteWidget(SelectedRowIndex, GetSide(sender));
                }
            }
            else if (dgInput.SelectedCells.Count > 0)
            {
                int SelectedRowIndex = getSSIndex(dgInput.SelectedCells[0].RowIndex, sender);

                if (SelectedRowIndex < (dgInput.RowCount - 1))
                {
                    packParams.cyScanSlotsList.DemoteWidget(SelectedRowIndex, GetSide(sender));
                }
            }
            UpdateDGRowsOrder(sender);
        }

        private void PromoteRow(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            if (dgInput.SelectedRows.Count > 0)
            {
                int SelectedRowIndex = getSSIndex(dgInput.SelectedRows[0].Index, sender);

                if (SelectedRowIndex > 0)
                {
                    for (int i = 0; i < dgInput.SelectedRows.Count; i++)
                    {
                        SelectedRowIndex = getSSIndex(dgInput.SelectedRows[i].Index, sender);

                        packParams.cyScanSlotsList.PromoteWidget(SelectedRowIndex, GetSide(sender));
                    }

                }
             
            }
            else if (dgInput.SelectedCells.Count > 0)
            {
                int SelectedRowIndex = getSSIndex(dgInput.SelectedCells[0].RowIndex, sender);

                if (SelectedRowIndex > 0)
                {
                    for (int i = 0; i < dgInput.SelectedCells.Count; i++)
                    {
                        SelectedRowIndex = getSSIndex(dgInput.SelectedCells[i].RowIndex, sender);
                        packParams.cyScanSlotsList.PromoteWidget(SelectedRowIndex, GetSide(sender));
                    }
                }

               
            }
            UpdateDGRowsOrder(sender);
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
        Label lCSGetFromSide(eElSide side)
        {
            Label curLabel = lCSLeft;
            if (side == eElSide.Right) { curLabel = lCSRight; }
            return curLabel;
        }
        ContentControls.cntSSProperties SSPropsFromSide(eElSide side)
        {
            ContentControls.cntSSProperties curectProps = SSPropsLeft;
            if (side == eElSide.Right) { curectProps = SSPropsRight; }
            return curectProps;
        }
        void lCSSetNoneSelection(eElSide side)
        {
            Label curLabel = lCSGetFromSide(side);
            ContentControls.cntSSProperties curectProps = SSPropsFromSide(side);
            curLabel.Text = strlCSNone;
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
            (sender as CyDGScanSlot).this_MouseUp(sender, e, GetSide(sender), packParams.cyWidgetsList.listTerminal);
        }
        #endregion




    }
}

