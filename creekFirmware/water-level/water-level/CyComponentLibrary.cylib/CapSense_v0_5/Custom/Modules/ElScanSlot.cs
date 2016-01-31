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

namespace  CapSense_v0_5
{
    #region ElScanSlot
    [Serializable()]
    public class ElScanSlot
    {
        //[XmlArray("ListTerminalsForSS")]
        [XmlIgnore]
        [NonSerialized]
        public List<ElTerminal> listTerminals = new List<ElTerminal>();
        public List<ElTerminal> GetTerminals()
        {
            List<ElTerminal> res = new List<ElTerminal>();
            foreach (ElTerminal item in listTerminals)
                if (!cySensorType.IsWidgetLabel(item))
                {
                    res.Add(item);
                }
            return res;
        }

        [XmlArray("ListTerminalsForSSNames")]
        public List<string> srlistTerminalsNames = new List<string>();

        [XmlIgnore]
        [NonSerialized]
        public ElWidget Widget;

        [XmlAttribute("WidgetName")]
        public string srWidgetName;


        [XmlAttribute("side")]
        public eElSide side;

        public clSSProperties SSProperties = new clSSProperties();

        [NonSerialized()]
        [XmlIgnore()]
        public Indexer Index;

        [XmlAttribute("Type")]
        public sensorType Type = 0;

        #region Pre/Post Serialization Events
        public void PreSerialization()
        {
            srlistTerminalsNames.Clear();
            foreach (ElTerminal item in listTerminals)
            {
                srlistTerminalsNames.Add(item.ToString());
            }
            srWidgetName = Widget.ToString();
        }
        #endregion

        #region Functions
        public ElScanSlot(int Index, eElSide side)
        {
            this.Index = new Indexer(Index);
            this.side = side;
        }
        public ElScanSlot()
        {
            this.Index = new Indexer(0);
        }


        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < listTerminals.Count; i++)
            {
                res += listTerminals[i].ToString();
                if (i != listTerminals.Count - 1) res += ", ";
            }
            return res;
        }
        public  string GetHeader()
        {
            string res = "";
            if (listTerminals.Count > 0) res = listTerminals[0].ToString();
            return res;
        }

        public void GetAssosiateTermin(ref ListBox lbAT)
        {
            lbAT.Items.Clear();
            for (int i = 0; i < listTerminals.Count; i++)
            {
                lbAT.Items.Add(listTerminals[i]);
            }
        }
        public bool AddTerminal(ElTerminal term)
        {
            if (listTerminals.Contains(term))
                return false;
            listTerminals.Add(term);
            return true;

        }
        public bool AddTerminal(ElTerminal term, int Ind)
        {
            if (listTerminals.Contains(term))
                return false;
            listTerminals.Insert(Ind, term);
            return true;
        }
        public bool IsFree()
        {
            if (listTerminals.Count == 0) return true;
            if (cySensorType.IsCustomCase(Type))
            {
                if (Widget == null) return true;//No Widget so it is Free
                if (Widget.type != listTerminals[0].type) return true;//Widget and terminal with differnt type so it is Free
            }
            return false;
        }
        #endregion
    }

    #region Indexer
    [Serializable()]
    public class Indexer
    {
        public int index;
        public Indexer(int i)
        {
            index = i;
        }

        public override string ToString()
        {
            return index.ToString();
        }
        public Indexer()
        { }

    }
    #endregion
    #endregion
}
