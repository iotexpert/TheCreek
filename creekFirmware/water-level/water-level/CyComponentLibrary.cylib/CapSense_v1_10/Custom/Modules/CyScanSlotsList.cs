/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace  CapSense_v1_10
{
    #region CyScanSlotsList
    [Serializable()]
    public class CyScanSlotsList
    {
        #region Header
        [NonSerialized()]
        [XmlIgnore()]
        public CyGeneralParams m_Base;

        [XmlArray("ListScanSlotsLeft")]
        [XmlArrayItem("ElScanSlot")]
        public List<CyElScanSlot> m_listScanSlotsL = new List<CyElScanSlot>();

        [XmlArray("ListScanSlotsRight")]
        [XmlArrayItem("ElScanSlot")]
        public List<CyElScanSlot> m_listScanSlotsR = new List<CyElScanSlot>();

        int m_maxIndexL = -1;
        int m_maxIndexR = -1;

        int NextIndexInternal(ref int SInd)
        {
            SInd++;
            return SInd;
        }

        public int NextIndex(E_EL_SIDE side)
        {
            if (side == E_EL_SIDE.Left) return NextIndexInternal(ref m_maxIndexL);
            if (side == E_EL_SIDE.Right) return NextIndexInternal(ref m_maxIndexR);
            return 0;
        }

        public CyScanSlotsList(CyGeneralParams Base)
        {
            this.m_Base = Base;
        }
        public CyScanSlotsList()
        {
        }
        #endregion

        #region ScanSlots Work
        public void AddNewSS(CyElWidget WI, out CyElScanSlot nss, E_EL_SIDE side)
        {
            nss = new CyElScanSlot(NextIndex(side), side);            
            for (int i = GetListFromSide(side).Count - 1; i >= 0; i--)
                if (GetListFromSide(side)[i].m_Widget.IsSameNameType(WI))
                {
                    GetListFromSide(side).Insert(i + 1, nss);
                    return;
                }
            GetListFromSide(side).Add(nss);            
        }
        public void DeleteSS(int pos, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);

            for (int i = 0; i < list.Count; i++)
                if (list[i].m_Index.m_index == pos)
                {
                    list.RemoveAt(i);
                    return;
                }

        }
        public void AppendTerminalInSS(CyElWidget wi, CyElTerminal term)
        {
            List<CyElScanSlot> list = GetListFromSide(wi.m_side);
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].m_Widget.IsSameNameType(wi))
                {
                    list[i].AddTerminal(term);
                }
            }
        }
        public void RemoveAllTerminalsAtSS(int id, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            if (CySensorType.IsCustomCase(list[id].m_Widget.m_type))
            {
                CyElTerminal termHead = list[id].m_listTerminals[0];
                list[id].m_listTerminals.Clear();
                list[id].AddTerminal(termHead);
            }
        }


        public void Reindexing(E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].m_Index.m_index = i;
            }
            //Reset Maximum Index
            m_maxIndexL = m_listScanSlotsL.Count - 1;
            m_maxIndexR = m_listScanSlotsR.Count - 1;
        }
        public void SendProx_GenToBottom(E_EL_SIDE side)
        {
            List<CyElScanSlot> listGenProx = new List<CyElScanSlot>();
            List<CyElScanSlot> list = GetListFromSide(side);
            for (int i = 0; i < list.Count; i++)
                if (CySensorType.IsCustomCase(list[i].m_Widget.m_type))
                {
                    listGenProx.Add(list[i]);
                }
            foreach (CyElScanSlot item in listGenProx)
            {
                list.Remove(item);
                list.Add(item);
            }

            Reindexing(side);
        }
        void DeleteTerminalInternal(CyElTerminal term, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            //Delete Terminals in Scan Slots Lists
            foreach (CyElScanSlot ss in list)
            {
                int pos = 0;

                while (pos < ss.m_listTerminals.Count)
                {
                    if (ss.m_listTerminals[pos].IsSameFull(term))
                        ss.m_listTerminals.RemoveAt(pos);
                    else pos++;
                }
            }
        }

        public void DeleteTerminal(CyElTerminal term)
        {
            DeleteTerminalInternal(term, E_EL_SIDE.Left);
            DeleteTerminalInternal(term, E_EL_SIDE.Right);
        }

        public int GetScanSlotCount()
        {
            return m_listScanSlotsL.Count + m_listScanSlotsR.Count;
        }
        public List<CyElScanSlot> GetListFromSide(E_EL_SIDE side)
        {
            List<CyElScanSlot> list = m_listScanSlotsL;
            if (side == E_EL_SIDE.Right) list = m_listScanSlotsR;

            return list;
        }
        public List<CyElScanSlot> GetScanSlotsSorted()
        {
            return new List<CyElScanSlot>(GetScanSlotsSortedInt());
        }
        IEnumerable<CyElScanSlot> GetScanSlotsSortedInt()
        {
            foreach (E_EL_SIDE item in CyGeneralParams.m_sideMass)
            {
                foreach (CyElScanSlot itemInt in GetListFromSide(item))
                    yield return itemInt;
            }
        }
        #endregion

        #region Promote/Demote Work
        bool PromoteSS(int id, int newpos, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            if ((id > 0) && (id < list.Count))
            {
                CyElScanSlot ss = list[id];
                list.Remove(ss);
                list.Insert(newpos, ss);
                Reindexing(side);
                return true;
            }
            return false;
        }
        public void PromoteWidget(int id, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            //Get Currect widget
            CyElWidget wi = list[id].m_Widget;
            //Get Top item
            id = GetTopSS(wi, side);
            if (id != 0)
            {
                //Calculate new position
                int newpos = GetTopSS(list[id - 1].m_Widget, side);
                //While widget elements
                while ((id < list.Count) && (list[id].m_Widget == wi))
                    if (PromoteSS(id, newpos, side)) { id++; newpos++; }
                    else break;
            }
        }
        public void DemoteWidget(int id, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            //Get Currect widget
            CyElWidget wi = list[id].m_Widget;
            //Get Bottom item
            id = GetBottomSS(wi, side);
            //Promote item below
            if (id < list.Count - 1)
            {
                PromoteWidget(id + 1, side);
            }
        }

        int GetTopSS(CyElWidget wi, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            int res = 0;
            for (int i = 0; i < list.Count; i++)
                if (list[i].m_Widget == wi)
                {
                    res = i;
                    break;
                }
            return res;
        }
        int GetBottomSS(CyElWidget wi, E_EL_SIDE side)
        {
            List<CyElScanSlot> list = GetListFromSide(side);
            int res = 0;
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].m_Widget == wi)
                {
                    res = i;
                    break;
                }
            return res;
        }
        #endregion

    }
    #endregion
}
