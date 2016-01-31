/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace  CapSense_v0_5
{
    public partial class CyScanSlots
    {
        String textCSMethod = "CapSense Method: ";
        #region actGlobalParamsChange
        void actGlobalParamsChange()
        {
            if (!isLoading)
            {                   
                //Verify Header Label
                    LabelHeadLeft.Text = packParams.GetLeftLabel();

                foreach (CyAmuxBParams curCCSHalf in packParams.localParams.listCsHalfs)
                {
                    //Verify Side
                    eElSide side = curCCSHalf.side;

                    if (packParams.localParams.bCsHalfIsEnable(curCCSHalf))
                    //If side is Enable
                    {

                        if (side == eElSide.Left)
                        {
                            sCMainSS.Panel1Collapsed = false;
                            SSPropsLeft.Method = curCCSHalf.Method;
                            lCSLeft.Text = textCSMethod + strCapSenseMode.GetStr(curCCSHalf.Method);

                        }
                        else
                        {
                            sCMainSS.Panel2Collapsed = false;
                            SSPropsRight.Method = curCCSHalf.Method;
                            lCSRight.Text = textCSMethod + strCapSenseMode.GetStr(curCCSHalf.Method);
                        }

                        //Change state for ScaSlots properties
                        if (side == eElSide.Left)
                            SSPropsLeft.SSCSPropsSW.ChangeState(curCCSHalf);
                        else
                            SSPropsRight.SSCSPropsSW.ChangeState(curCCSHalf);
                    }
                    else
                    //If Disable
                    {
                        if (side == eElSide.Left)
                            sCMainSS.Panel1Collapsed = true;
                        else
                            sCMainSS.Panel2Collapsed = true;
                    }
                }

                //Change Visibility for ColLocation
                ColLocationVisible();

                UpdateDGRowsEnter();

            }
        }

        #endregion

        #region DGRows

        void UpdateDGRowsEnterInternal(DataGridView sender)
        {
            DataGridViewCell curc = sender.CurrentCell;
            if (curc != null)
            {
                dgScanSlots_RowEnter(sender, new DataGridViewCellEventArgs(curc.ColumnIndex, curc.RowIndex));
            }
        }
        public void UpdateDGRowsEnter()
        {
            UpdateDGRowsEnterInternal(dgScanSlotsR);
            UpdateDGRowsEnterInternal(dgScanSlotsL);
        }

        Color rowColor1 = Color.White;
        Color rowColor2 = Color.Gainsboro;
        Color iterColor;
        void ChangeIterColor()
        {
            if (iterColor == rowColor1) iterColor = rowColor2;
            else iterColor = rowColor1;
        }
        void SetRowColor(DataGridView sender, Color color, int rowInd)
        {
            for (int i = 0; i < sender.ColumnCount; i++)
            {
                sender[i, rowInd].Style.BackColor = color;
            }
        }

        public void UpdateDGRowsColors(object sender)
        {
            iterColor = rowColor1;
            DataGridView dgView = (DataGridView)sender;
            for (int i = 0; i < dgView.RowCount; i++)
            {
                if (i - 1 > -1)
                    if (CyGeneralParams.GetDGString(dgView, i, dgScanSlotsL.colType.Index) != CyGeneralParams.GetDGString(dgView, i - 1, dgScanSlotsL.colType.Index))
                        ChangeIterColor();
                SetRowColor(dgView, iterColor, i);
            }
        }
        public void UpdateDGRowsOrder(object sender)
        {
            DataGridView dgView = (DataGridView)sender;
            packParams.cyScanSlotsList.SendProx_GenToBottom(GetSide(sender));

            dgView.Sort(new RowComparer(dgScanSlotsL.colScanOrder.Index, SortOrder.Ascending));
            UpdateDGRowsColors(sender);
        }
        #endregion

        #region WidgetWork

        public void AddWidget(ElWidget wi)
        {
            DataGridView sender = GetSide(wi.side);
            InsertNewScanSlots(sender, wi);
        }

        #endregion

        #region Update & Get

        #region Update
        void UpdateTerminals(object sender)
        {
            int RowIndex = getCurrentRowIndex(sender);
            if (RowIndex > -1)
            {
                UpdateSS(RowIndex, sender);         
            }
        }
        void UpdateSS(int RowIndex,object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            dgInput[dgScanSlotsL.colAssociatedTerminals.Index, RowIndex].Value = "";
            dgInput[dgScanSlotsL.colAssociatedTerminals.Index, RowIndex].Value = packParams.cyScanSlotsList.GetListFromSide(GetSide(sender))[getSSIndex(RowIndex, sender)];
            dgInput.Update();
            dgInput.Invalidate(); 
        }
        void UpdateSSAll(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            for (int i = 0; i < dgInput.RowCount; i++)
            {
                UpdateSS(i,sender);
            }
            UpdateDGRowsColors(sender);
        }
        #endregion

        

        public eElSide GetSide(object sender)
        {
            if (sender == dgScanSlotsR) return eElSide.Right;
            if (sender == dgScanSlotsL) return eElSide.Left;
            return eElSide.None;

        }
        public DataGridView GetSide(eElSide side)
        {
            if (side == eElSide.Left) return dgScanSlotsL;
            if (side == eElSide.Right) return dgScanSlotsR;
            return null;

        }


        #endregion

        #region Indexer Work
        int getCurrentRowIndex(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            int Res = -1;
            if (dgInput.CurrentCellAddress.Y > -1)
                Res = dgInput.CurrentCellAddress.Y;
            return Res;

        }
        int getSSIndex(int RowIndex,object sender)
        {
            return Convert.ToInt32(((DataGridView)sender)[dgScanSlotsL.colScanOrder.Index, RowIndex].Value.ToString());
        }
        int getRowIndex(int SSIndex, object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            for (int i = 0; i < dgInput.RowCount; i++)
                if (Convert.ToInt32(dgInput[dgScanSlotsL.colScanOrder.Index, i].Value.ToString()) == SSIndex)
                    return i;

            return 0;
        }
        #endregion

        #region ScanSlots

        public void InsertNewScanSlots(object sender,ElWidget widget)
        {
            widget.Enable = false;
            List<ElTerminal> listTerm =new List<ElTerminal>(packParams.cyWidgetsList.GetTerminals(widget));
            if (!cySensorType.IsCustomCase(widget.type))
            {
                foreach (ElTerminal item in listTerm)
                {
                    InsertNewScanSlotWithTerm(sender, item);
                }
            }
            else InsertNewScanSlotWithTerm(sender, listTerm[0]);
        }

        public void InsertNewScanSlotWithTerm(object sender, ElTerminal term)
        {
            ElScanSlot ss;
            packParams.cyScanSlotsList.AddNewSS(out ss, GetSide(sender));            
            ss.Widget = packParams.cyWidgetsList.GetWidget(term);
            ss.SSProperties.baseProps = ss.Widget.baseSSProps;
            ss.Type = ss.Widget.type;

            //Adding Terminals to SS
            if (!cySensorType.IsCustomCase(ss.Widget.type))
                ss.listTerminals.Add(term);
            else
            {
                //In Custom case
                foreach (ElTerminal item in packParams.cyWidgetsList.GetTerminals(ss.Widget))
                {
                    ss.listTerminals.Add(item);
                }
            }
            
            ShowScanSlot(ss, sender);
        }
        public void ShowScanSlot(ElScanSlot ss, object sender)
        {
            if (sender != null)
            {
                object[] RowObject = new object[dgScanSlotsL.ColumnCount];
                RowObject[dgScanSlotsL.colScanOrder.Index] = ss.Index;
                RowObject[dgScanSlotsL.colType.Index] = ss.Widget;
                //((cyDGScanSlot)sender).colAssociatedTerminals.Items.Add(ss);
                RowObject[dgScanSlotsL.colAssociatedTerminals.Index] = ss;//.listTerminals[0];
                ((DataGridView)sender).Rows.Add(RowObject);
                UpdateDGRowsOrder(sender);
            }
        }
        public void deleteSS(int ssPos, bool reindexing,object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            dgInput.Rows.RemoveAt(getRowIndex(ssPos, sender));
            packParams.cyScanSlotsList.DeleteSS(ssPos, GetSide(sender));
            if (reindexing)
                packParams.cyScanSlotsList.Reindexing(GetSide(sender));

        }

        public void EraceFreeSS()
        {
            EraceFreeSSInt(dgScanSlotsL, packParams.cyScanSlotsList.listScanSlotsL);
            EraceFreeSSInt(dgScanSlotsR, packParams.cyScanSlotsList.listScanSlotsR);
        }
        void EraceFreeSSInt(object sender, List<ElScanSlot> listScanSlots)
        {
            
            int pos = 0;
            while (pos < listScanSlots.Count)
            {
                if (listScanSlots[pos].IsFree())
                {
                    deleteSS(pos, false, sender);
                }
                pos++;
            }
            packParams.cyScanSlotsList.Reindexing(GetSide(sender));
        }

        #endregion
    }
}

