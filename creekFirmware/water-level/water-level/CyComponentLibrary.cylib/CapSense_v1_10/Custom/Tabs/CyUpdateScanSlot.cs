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

namespace  CapSense_v1_10
{
    public partial class CyScanSlots
    {
        String m_textCSMethod = "CapSense Method: ";
        #region actGlobalParamsChange
        void ActGlobalParamsChange()
        {
            if (!m_isLoading)
            {                   
                //Verify Header Label
                    LabelHeadLeft.Text = m_packParams.GetLeftLabel();

                foreach (CyAmuxBParams curCCSHalf in m_packParams.m_localParams.m_listCsHalfs)
                {
                    //Verify Side
                    E_EL_SIDE side = curCCSHalf.m_side;

                    if (m_packParams.m_localParams.BCsHalfIsEnable(curCCSHalf))
                    //If side is Enable
                    {

                        if (side == E_EL_SIDE.Left)
                        {
                            sCMainSS.Panel1Collapsed = false;
                            SSPropsLeft.Method = curCCSHalf.m_Method;
                            lCSLeft.Text = m_textCSMethod + CyStrCapSenseMode.GetStr(curCCSHalf.m_Method);

                        }
                        else
                        {
                            sCMainSS.Panel2Collapsed = false;
                            SSPropsRight.Method = curCCSHalf.m_Method;
                            lCSRight.Text = m_textCSMethod + CyStrCapSenseMode.GetStr(curCCSHalf.m_Method);
                        }

                        //Change state for ScaSlots properties
                        if (side == E_EL_SIDE.Left)
                            SSPropsLeft.GetProperties(curCCSHalf);
                        else
                            SSPropsRight.GetProperties(curCCSHalf);
                    }
                    else
                    //If Disable
                    {
                        if (side == E_EL_SIDE.Left)
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

        Color m_rowColor1 = Color.White;
        Color m_rowColor2 = Color.Gainsboro;
        Color m_iterColor;
        void ChangeIterColor()
        {
            if (m_iterColor == m_rowColor1) m_iterColor = m_rowColor2;
            else m_iterColor = m_rowColor1;
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
            m_iterColor = m_rowColor1;
            DataGridView dgView = (DataGridView)sender;
            for (int i = 0; i < dgView.RowCount; i++)
            {
                if (i - 1 > -1)
                    if (CyGeneralParams.GetDGString(dgView, i, dgScanSlotsL.colType.Index) != 
                        CyGeneralParams.GetDGString(dgView, i - 1, dgScanSlotsL.colType.Index))
                        ChangeIterColor();
                SetRowColor(dgView, m_iterColor, i);
            }
        }
        public void UpdateDGRowsOrder(object sender)
        {
            DataGridView dgView = (DataGridView)sender;
            m_packParams.m_cyScanSlotsList.SendProx_GenToBottom(GetSide(sender));
            dgView.Sort(new RowComparer(dgScanSlotsL.colScanOrder.Index, SortOrder.Ascending));
            UpdateDGRowsColors(sender);
        }
        #endregion

        #region Side Work
        public E_EL_SIDE GetSide(object sender)
        {
            if (sender == dgScanSlotsR) return E_EL_SIDE.Right;
            if (sender == dgScanSlotsL) return E_EL_SIDE.Left;
            return E_EL_SIDE.None;

        }
        public DataGridView GetSide(E_EL_SIDE side)
        {
            if (side == E_EL_SIDE.Left) return dgScanSlotsL;
            if (side == E_EL_SIDE.Right) return dgScanSlotsR;
            return null;

        }
        #endregion

        #region Indexer Work
        int GetCurrentRowIndex(object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            int Res = -1;
            if (dgInput.CurrentCellAddress.Y > -1)
                Res = dgInput.CurrentCellAddress.Y;
            return Res;
        }
        int GetSSIndex(int RowIndex,object sender)
        {
            return Convert.ToInt32(((DataGridView)sender)
                [dgScanSlotsL.colScanOrder.Index, RowIndex].Value.ToString());
        }
        int GetRowIndex(int SSIndex, E_EL_SIDE side)
        {
            DataGridView dgInput = GetSide(side);
            for (int i = 0; i < dgInput.RowCount; i++)
                if (Convert.ToInt32(dgInput[dgScanSlotsL.colScanOrder.Index, i].Value.ToString()) == SSIndex)
                    return i;

            return 0;
        }
        #endregion

        #region ScanSlots
        public void ChangeWidgetSide(CyElWidget old_wi, CyElWidget new_wi)
        {
            List<CyElScanSlot> listOld=m_packParams.m_cyScanSlotsList.GetListFromSide(old_wi.m_side);
            List<CyElScanSlot> listNew=m_packParams.m_cyScanSlotsList.GetListFromSide(new_wi.m_side);
            int i = 0;
            while(i<listOld.Count)           
            {
                CyElScanSlot ss = listOld[i];
                if (ss.m_Widget.IsSame(old_wi))
                {
                    listNew.Add(ss);
                    DeleteSS(i, true, old_wi.m_side);
                    ss.m_side = new_wi.m_side;
                    ShowScanSlot(ss, new_wi.m_side);
                }
                else
                    i++;
            }
        }

        public void InsertNewScanSlots(CyElWidget widget,E_EL_SIDE side)
        {
            widget.m_Enable = false;
            List<CyElTerminal> listTerm =new List<CyElTerminal>(m_packParams.m_cyWidgetsList.GetTerminals(widget));
            if (!CySensorType.IsCustomCase(widget.m_type))
            {
                foreach (CyElTerminal item in listTerm)
                {
                    InsertNewScanSlotWithTerm( item,side);
                }
            }
            else InsertNewScanSlotWithTerm( listTerm[0],side);            
        }

        public void InsertNewScanSlotWithTerm(CyElTerminal term, E_EL_SIDE side)
        {
            CyElScanSlot ss;
            CyElWidget Wi = m_packParams.m_cyWidgetsList.GetWidget(term);
            m_packParams.m_cyScanSlotsList.AddNewSS(Wi, out ss, side);
            ss.m_Widget = Wi;
            ss.m_SSProperties.m_baseProps = ss.m_Widget.m_baseSSProps;
            ss.m_Type = ss.m_Widget.m_type;

            //Adding Terminals to SS
            if (CySensorType.IsCustomCase(ss.m_Widget.m_type))
            {
                //In Custom case
                foreach (CyElTerminal item in m_packParams.m_cyWidgetsList.GetTerminals(ss.m_Widget))
                {
                    ss.m_listTerminals.Add(item);
                }
            }
            else
            {
                ss.m_listTerminals.Add(term);
            }

            ShowScanSlot(ss, side);
        }
        void UpdateSS(int RowIndex, object sender)
        {
            DataGridView dgInput = (DataGridView)sender;
            dgInput[dgScanSlotsL.colAssociatedTerminals.Index, RowIndex].Value = "";
            dgInput[dgScanSlotsL.colAssociatedTerminals.Index, RowIndex].Value =
                m_packParams.m_cyScanSlotsList.GetListFromSide(GetSide(sender))[GetSSIndex(RowIndex, sender)];
            dgInput.Update();
            dgInput.Invalidate();
        }
        public void ShowScanSlot(CyElScanSlot ss, E_EL_SIDE side)
        {            
            object sender = GetSide(side);
            if (sender != null)
            {
                object[] RowObject = new object[dgScanSlotsL.ColumnCount];
                RowObject[dgScanSlotsL.colScanOrder.Index] = ss.m_Index;
                RowObject[dgScanSlotsL.colType.Index] = ss.m_Widget;
                //((cyDGScanSlot)sender).colAssociatedTerminals.Items.Add(ss);
                RowObject[dgScanSlotsL.colAssociatedTerminals.Index] = ss;//.listTerminals[0];
                ((DataGridView)sender).Rows.Add(RowObject);
                UpdateDGRowsOrder(sender);
            }
        }
        public void DeleteSS(int ssPos, bool reindexing,E_EL_SIDE side)
        {
            DataGridView dgInput = GetSide(side);
            dgInput.Rows.RemoveAt(GetRowIndex(ssPos, side));
            m_packParams.m_cyScanSlotsList.DeleteSS(ssPos, side);
            if (reindexing)
                m_packParams.m_cyScanSlotsList.Reindexing(side);

        }
        public void EraceFreeSS()
        {
            EraceFreeSSInt(dgScanSlotsL, m_packParams.m_cyScanSlotsList.m_listScanSlotsL);
            EraceFreeSSInt(dgScanSlotsR, m_packParams.m_cyScanSlotsList.m_listScanSlotsR);
        }
        void EraceFreeSSInt(object sender, List<CyElScanSlot> listScanSlots)
        {
            
            int pos = 0;
            while (pos < listScanSlots.Count)
            {
                if (listScanSlots[pos].IsFree())
                {
                    DeleteSS(pos, false, GetSide(sender));
                }
                pos++;
            }
            m_packParams.m_cyScanSlotsList.Reindexing(GetSide(sender));
        }

        #endregion
    }
}

