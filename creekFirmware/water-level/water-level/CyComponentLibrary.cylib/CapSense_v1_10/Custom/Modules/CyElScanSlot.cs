/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace  CapSense_v1_10
{
    #region ElScanSlot
    [Serializable()]
    public class CyElScanSlot
    {
        [XmlIgnore]
        [NonSerialized]
        public List<CyElTerminal> m_listTerminals = new List<CyElTerminal>();
        public List<CyElTerminal> GetTerminals()
        {
            List<CyElTerminal> res = new List<CyElTerminal>();
            foreach (CyElTerminal item in m_listTerminals)
                if (CySensorType.IsNotWidgetLabel(item))
                {
                    res.Add(item);
                }
            return res;
        }

        [XmlArray("ListTerminalsForSSNames")]
        public List<string> m_srlistTerminalsNames = new List<string>();

        [XmlIgnore]
        [NonSerialized]
        public CyElWidget m_Widget;

        [XmlAttribute("WidgetName")]
        public string m_srWidgetName;


        [XmlAttribute("side")]
        public E_EL_SIDE m_side;

        [XmlElement("SSProperties")]
        public CySSProperties m_SSProperties = new CySSProperties();

        [NonSerialized()]
        [XmlIgnore()]
        public CyIndexer m_Index;

        [XmlAttribute("Type")]
        public E_SENSOR_TYPE m_Type = 0;

        #region Pre/Post Serialization Events
        public void PreSerialization()
        {
            m_srlistTerminalsNames.Clear();
            foreach (CyElTerminal item in m_listTerminals)
            {
                m_srlistTerminalsNames.Add(item.ToString());
            }
            m_srWidgetName = m_Widget.ToString();
        }
        #endregion

        #region Functions
        public CyElScanSlot(int Index, E_EL_SIDE side)
        {
            this.m_Index = new CyIndexer(Index);
            this.m_side = side;
        }
        public CyElScanSlot()
        {
            this.m_Index = new CyIndexer(0);
        }


        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < m_listTerminals.Count; i++)
            {
                res += m_listTerminals[i].ToString();
                if (i != m_listTerminals.Count - 1) res += ", ";
            }
            return res;
        }
        public  string GetHeader()
        {
            string res = "";
            if (m_listTerminals.Count > 0) res = m_listTerminals[0].ToString();
            return res;
        }

        public void GetAssosiateTermin(ref ListBox lbAT)
        {
            lbAT.Items.Clear();
            for (int i = 0; i < m_listTerminals.Count; i++)
            {
                lbAT.Items.Add(m_listTerminals[i]);
            }
        }
        public bool AddTerminal(CyElTerminal term)
        {
            if (m_listTerminals.Contains(term))
                return false;
            m_listTerminals.Add(term);
            return true;

        }
        public bool AddTerminal(CyElTerminal term, int Ind)
        {
            if (m_listTerminals.Contains(term))
                return false;
            m_listTerminals.Insert(Ind, term);
            return true;
        }
        public bool IsFree()
        {
            if (m_listTerminals.Count == 0) return true;
            if (CySensorType.IsCustomCase(m_Type))
            {
                //No Widget so it is Free
                if (m_Widget == null) return true;
                //Widget and terminal with differnt type so it is Free
                if (m_Widget.m_type != m_listTerminals[0].m_type) return true;
            }
            return false;
        }
        #endregion
    }

    #region Indexer
    [Serializable()]
    public class CyIndexer
    {
        public int m_index;
        public CyIndexer(int i)
        {
            m_index = i;
        }

        public override string ToString()
        {
            return m_index.ToString();
        }
        public CyIndexer()
        { }

    }
    #endregion
    #endregion
}
