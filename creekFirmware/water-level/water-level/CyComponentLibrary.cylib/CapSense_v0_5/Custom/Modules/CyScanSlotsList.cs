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

namespace  CapSense_v0_5
{
    #region CyScanSlotsList
    [Serializable()]
    public class CyScanSlotsList
    {
        #region Header
        [NonSerialized()]
        [XmlIgnore()]
        public CyGeneralParams Base;

        [XmlArray("ListScanSlotsLeft")]

        public List<ElScanSlot> listScanSlotsL = new List<ElScanSlot>();

        [XmlArray("ListScanSlotsRight")]
        public List<ElScanSlot> listScanSlotsR = new List<ElScanSlot>();

        int maxIndexL = -1;
        int maxIndexR = -1;

        int NextIndexInternal(ref int SInd)
        {
            SInd++;
            return SInd;
        }

        public int NextIndex(eElSide side)
        {
            if (side == eElSide.Left) return NextIndexInternal(ref maxIndexL);
            if (side == eElSide.Right) return NextIndexInternal(ref maxIndexR);
            return 0;
        }

        public CyScanSlotsList(CyGeneralParams Base)
        {
            this.Base = Base;
        }
        public CyScanSlotsList()
        {
            //this.Base = null;
        }
        #endregion

        #region ScanSlots Work
        public void AddNewSS(out ElScanSlot nss, eElSide side)
        {
            nss = new ElScanSlot(NextIndex(side), side);
            GetListFromSide(side).Add(nss);

        }
        public void DeleteSS(int pos, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);

            for (int i = 0; i < list.Count; i++)
                if (list[i].Index.index == pos)
                {
                    list.RemoveAt(i);
                    return;
                }

        }

        public bool AppendTerminalInSS(ElTerminal term, eElSide side, int id)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            return list[id].AddTerminal(term);
        }
        public bool AppendTerminalInSS(ElTerminal term, eElSide side, int id, int indexPos)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            return list[id].AddTerminal(term, indexPos);
        }
        public bool RemoveTerminalInSS(ElTerminal term, eElSide side, int id)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            return list[id].listTerminals.Remove(term);
        }
        public void RemoveAllTerminalsAtSS(int id, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            if (cySensorType.IsCustomCase(list[id].Widget.type))
            {
                ElTerminal termHead = list[id].listTerminals[0];
                list[id].listTerminals.Clear();
                list[id].AddTerminal(termHead);
            }
        }


        public void Reindexing(eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Index.index = i;
            }
            //Reset Maximum Index
            maxIndexL = listScanSlotsL.Count - 1;
            maxIndexR = listScanSlotsR.Count - 1;
        }
        public void SendProx_GenToBottom(eElSide side)
        {
            List<ElScanSlot> listGenProx = new List<ElScanSlot>();
            List<ElScanSlot> list = GetListFromSide(side);
            for (int i = 0; i < list.Count; i++)
                if (cySensorType.IsCustomCase(list[i].Widget.type))
                {
                    listGenProx.Add(list[i]);
                }
            foreach (ElScanSlot item in listGenProx)
            {
                list.Remove(item);
                list.Add(item);
            }

            Reindexing(side);
        }
        void DeleteTerminalInternal(ElTerminal term, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            //Delete Terminals in Scan Slots Lists
            foreach (ElScanSlot ss in list)
            {
                int pos = 0;

                while (pos < ss.listTerminals.Count)
                {
                    if (ss.listTerminals[pos].IsSameW(term))
                        ss.listTerminals.RemoveAt(pos);
                    else pos++;
                }
            }
        }

        public void DeleteTerminal(ElTerminal term)
        {
            DeleteTerminalInternal(term, eElSide.Left);
            DeleteTerminalInternal(term, eElSide.Right);
        }

        public int GetScanSlotCount()
        {
            return listScanSlotsL.Count + listScanSlotsR.Count;
        }
        public List<ElScanSlot> GetListFromSide(eElSide side)
        {
            List<ElScanSlot> list = listScanSlotsL;
            if (side == eElSide.Right) list = listScanSlotsR;

            return list;
        }
        public List<ElScanSlot> GetScanSlotsSorted()
        {
            return new List<ElScanSlot>(GetScanSlotsSortedInt());
        }
        IEnumerable<ElScanSlot> GetScanSlotsSortedInt()
        {
            foreach (eElSide item in CyGeneralParams.sideMass)
            {
                foreach (ElScanSlot itemInt in GetListFromSide(item))
                    yield return itemInt;
            }
        }


        #endregion

        #region Promote/Demote Work
        bool PromoteSS(int id, int newpos, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);

            if ((id > 0) && (id < list.Count))
            {
                ElScanSlot ss = list[id];
                list.Remove(ss);
                list.Insert(newpos, ss);
                Reindexing(side);
                return true;
            }
            return false;
        }
        public void PromoteWidget(int id, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            //Get Currect widget
            ElWidget wi = list[id].Widget;
            //Get Top item
            id = GetTopSS(wi, side);
            if (id != 0)
            {
                //Calculate new position
                int newpos = GetTopSS(list[id - 1].Widget, side);
                //While widget elements
                while ((id < list.Count) && (list[id].Widget == wi))
                    if (PromoteSS(id, newpos, side)) { id++; newpos++; }
                    else break;
            }
        }
        public void DemoteWidget(int id, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            //Get Currect widget
            ElWidget wi = list[id].Widget;
            //Get Bottom item
            id = GetBottomSS(wi, side);
            //Promote item below
            if (id < list.Count - 1)
            {
                PromoteWidget(id + 1, side);
            }
        }

        int GetTopSS(ElWidget wi, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            int res = 0;
            for (int i = 0; i < list.Count; i++)
                if (list[i].Widget == wi)
                {
                    res = i;
                    break;
                }
            return res;
        }
        int GetBottomSS(ElWidget wi, eElSide side)
        {
            List<ElScanSlot> list = GetListFromSide(side);
            int res = 0;
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].Widget == wi)
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
