/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace  CapSense_CSD_v2_0
{ 
    #region CyScanSlotsList
    [Serializable()]
    public class CyScanSlotsList
    {
        #region Header
        [NonSerialized()]
        [XmlIgnore()]
        public CyWidgetsList m_widgets;

        [XmlArray("ListScanSlots")]
        [XmlArrayItem("CyScanSlot")]
        public List<CyScanSlot> m_listScanSlots = new List<CyScanSlot>();

        [XmlElement("GuardSensorScanSlot")]
        public CyScanSlot m_guardSensor;

        public CyScanSlotsList(CyWidgetsList widg)
        {
            this.m_widgets = widg;
        }
        public CyScanSlotsList()
        {
        }
        #endregion

        #region Service function
        public List<CyScanSlot> GetSSList(CyChannelNumber channel)
        {
            List<CyScanSlot> list=new List<CyScanSlot>();
            for (int i = 0; i < m_listScanSlots.Count; i++)
            {
                if (m_listScanSlots[i].Channel == channel)
                    list.Add(m_listScanSlots[i]);
            }
            return list;
        }
        #endregion

        #region ScanSlots Work
        public void AddScanSlotsRange(CyWidget widget)
        {
            List<CyTerminal> listTerm = new List<CyTerminal>(m_widgets.GetTerminals(widget));
            if (CyCsConst.HasComplexScanSlot(widget.m_type))            
                AddScanSlot(widget, null);            
            else
            {
                foreach (CyTerminal item in listTerm)                
                    AddScanSlot(widget, item);                
            }
        }

        public void AddScanSlot(CyWidget widget, CyTerminal term)
        {
            CyScanSlot ss;

            InsertScanSlot(widget, out ss);          

            //Adding Terminals to SS
            if (CyCsConst.HasComplexScanSlot(ss.WidgetType))
                //In Custom case
                foreach (CyTerminal item in m_widgets.GetTerminals(ss.m_widget))
                {
                    ss.AddTerminal(item);
                }
            else
                ss.AddTerminal(term);
        }

        public void AppendTerminalForWidgetScanSlot(CyWidget wi, CyTerminal term)
        {
            List<CyScanSlot> list = m_listScanSlots;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].m_widget==wi)
                {
                    list[i].AddTerminal(term);
                }
            }
        }
        public void CheckTerminalsChannel()
        {
            for (int i = 0; i < m_listScanSlots.Count; i++)
                m_listScanSlots[i].CheckTerminalsChannel();
        }
        public void EraceFreeSS()
        {
            int pos = 0;
            List<CyScanSlot> list = m_listScanSlots;
            while (pos < list.Count)
            {
                CyScanSlot ss = list[pos];
                if (ss.IsFree())
                {
                    m_listScanSlots.Remove(ss);
                }
                else
                    pos++;
            }
        }

        void InsertScanSlot(CyWidget wi, out CyScanSlot nss)
        {
            List<CyScanSlot> list = m_listScanSlots;
            nss = new CyScanSlot(wi);
            //Search correct place to insert ss
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].m_widget == wi)
                {
                    list.Insert(i + 1, nss);
                    return;
                }
            list.Add(nss);
        }

        public void DeleteTerminal(CyTerminal term)
        {
            List<CyScanSlot> list = m_listScanSlots;
            if (term != null)
                //Delete Terminals in Scan Slots Lists
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].DeleteTerminal(term);
                }
        }
        public bool IsComplexScanSlots()
        {
            for (int i = 0; i < m_listScanSlots.Count; i++)
            {
                if (m_listScanSlots[i].GetTerminals().Count > 1) return true;
                CyTerminal term = m_listScanSlots[i].GetHeaderTerminal();
                if (term == null) return true;                
            }
            return false;
        }
        #endregion

        #region Promote/Demote Work
        public void PromoteWidget(CyScanSlot ss,bool singleChannel, bool down)
        {
            if (down == false)
                PromoteWidget(ss, singleChannel);
            else
                DemoteWidget(ss, singleChannel);

        }
        void PromoteWidget(CyScanSlot ss, bool singleChannel)
        {            
            //Get Currect widget
            CyWidget wi = ss.m_widget;
            List<CyScanSlot> _listSS = m_listScanSlots;
            //Get Top item
            int id = GetTopSS(wi);

            if (singleChannel == false)
                //Find previos widget in same channel
                while (id - 1 > -1 && _listSS[id - 1].Channel != wi.m_channel)
                    id--;                            
            if (id != 0)
            {
                //Calculate new position
                int newpos = GetTopSS(_listSS[id - 1].m_widget);
                //While widget elements
                while ((id < _listSS.Count) && (_listSS[id].m_widget == wi))
                    if (PromoteSS(id, newpos)) 
                    { id++; newpos++; }
                    else break;
            }
        }
        void DemoteWidget(CyScanSlot ss, bool singleChannel)
        {
            //Get Currect widget
            CyWidget wi = ss.m_widget;
            List<CyScanSlot> list = m_listScanSlots;
            //Get Bottom item
            int id = GetBottomSS(wi) + 1;
            if (singleChannel == false)
                //Find next widget in same channel
                while (id < list.Count && list[id].Channel != wi.m_channel)
                    id++;
            //Promote item below
            if (id < list.Count)
            {
                PromoteWidget(list[id], singleChannel);
            }
        }
        bool PromoteSS(int id, int newpos)
        {
            List<CyScanSlot> list = m_listScanSlots;
            if ((id > 0) && (id < list.Count))
            {
                CyScanSlot ss = list[id];
                list.Remove(ss);
                list.Insert(newpos, ss);
                return true;
            }
            return false;
        }
        int GetTopSS(CyWidget wi)
        {
            List<CyScanSlot> list = m_listScanSlots;
            int res = 0;
            for (int i = 0; i < list.Count; i++)
                if (list[i].m_widget == wi)
                {
                    res = i;
                    break;
                }
            return res;
        }
        int GetBottomSS(CyWidget wi)
        {
            List<CyScanSlot> list = m_listScanSlots;
            int res = 0;
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].m_widget == wi)
                {
                    res = i;
                    break;
                }
            return res;
        }
        #endregion

        public IEnumerable<CyScanSlot> GetEmptyComplexScanSlot()
        {
            for (int i = 0; i < m_listScanSlots.Count; i++)
                if (m_listScanSlots[i].IsEmptyComplexScanSlot())                
                    yield return m_listScanSlots[i];                
        }
    }
    #endregion
}
