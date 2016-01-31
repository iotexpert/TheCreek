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
using System.ComponentModel;

namespace CapSense_CSD_v2_0
{
    #region ScanSlot
    [Serializable()]
    public class CyScanSlot
    {
        #region Header
        List<CyTerminal> m_listTerminals = new List<CyTerminal>();
        string m_widgetName = string.Empty;
        List<string> m_listTerminalsNames = new List<string>();

        [XmlAttribute("IDACSettings")]
        [CyCSCompareEnableAttribute()]
        public int m_idacSettings = CyCsConst.C_IDAC_SETTINGS_DEF;

        [XmlAttribute("Sensitivity")]
        [CyCSCompareEnableAttribute()]
        public int m_sensitivity = CyCsConst.C_SENSITIVITY_DEF; 

        [XmlIgnore]
        public CyWidget m_widget;

        [XmlIgnore]
        public CyChannelNumber Channel
        {
            get
            {
                if (m_widget != null)
                    return m_widget.m_channel;
                return CyChannelNumber.First;
            }
        }

        [XmlIgnore]
        public CySensorType WidgetType
        {
            get
            {
                if (m_widget != null)
                    return m_widget.m_type;

                return CySensorType.Button;
            }
        }

        [XmlIgnore]
        public string Name
        {
            get
            {
                string result = string.Empty;
                if (m_widget != null)
                {                    
                    result = CyTerminal.GetName(m_widget.m_name, m_widget.m_type,-1);
                }
                return result;
            }
        }

        [XmlAttribute("WidgetName")]
        public string WidgetName
        {
            get
            {
                if (m_widget == null)
                {
                    return m_widgetName;
                }
                return m_widget.ToString();
            }
            set { m_widgetName = value; }
        }

        [XmlArray("ListTerminalsNames")]
        public List<string> ListTerminalsNames
        {
            get 
            {
                if (m_widget == null)
                {
                    return m_listTerminalsNames;
                }
                List<string> res = new List<string>();
                for (int i = 0; i < m_listTerminals.Count; i++)
                {
                    res.Add(m_listTerminals[i].ToString());
                }
                return res;
            }
            set { m_listTerminalsNames =value; }
        }

        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_TUNING)]
        [DisplayName(CyCsConst.PROPS_NAME_IDAC_VALUE)]
        public int IdacSettings
        {
            get { return m_idacSettings; }
            set { m_idacSettings = value; }
        }

        #endregion

        #region TunerValues
        [XmlIgnore]
        public bool m_isTouched = false;

        [XmlIgnore]
        public UInt16 m_signalValue = 1;

        [XmlIgnore]
        public UInt16 m_rawValue = 10;

        #endregion

        #region Functions
        public CyScanSlot()
        {
        }
        public CyScanSlot(CyWidget wi)
        {
            m_widget = wi;
        } 

        public List<CyTerminal> GetTerminals()
        {            
            return m_listTerminals;
        }

        public override string ToString()
        {
            if (m_widget == null)
            {
                return "<Empty>";
            }
            string res = CyCsConst.HasComplexScanSlot(WidgetType) ? Name + ", " : string.Empty;
            for (int i = 0; i < m_listTerminals.Count; i++)
                if (m_listTerminals[i] != null)
                {
                    res += m_listTerminals[i].ToString() + ", ";
                }
            return res.Length > 2 ? res.Remove(res.Length - 2) : res;
        }

        public bool AddTerminal(CyTerminal term)
        {
            if (term==null || m_listTerminals.Contains(term))
                return false;
            m_listTerminals.Add(term);
            return true;
        }

        public void DeleteTerminal(CyTerminal term)
        {
            m_listTerminals.Remove(term);
        }

        public void ClearAllTerminals()
        {
            if (m_widget != null)
                if (CyCsConst.HasComplexScanSlot(m_widget.m_type))
                {
                    CyTerminal term = GetHeaderTerminal();
                    m_listTerminals.Clear();
                    if (term != null)
                        m_listTerminals.Add(term);
                }
        }
        public void CheckTerminalsChannel()
        {
            if (CyCsConst.HasComplexScanSlot(WidgetType))
            {
                int id = 0;
                while (id < m_listTerminals.Count)                
                    if (m_listTerminals[id].Channel != Channel)
                    {
                        m_listTerminals.RemoveAt(id);
                    }
                    else
                    id++;
            }
            
        }

        public List<string> GetListTerminalsDeserializedAlias()
        {
            return m_listTerminalsNames;
        }

        public bool IsFree()
        {
            //No Widget so it is Free
            if (m_widget == null) return true;

            //No Terminals so it is Free
            if (m_listTerminals.Count == 0 && CyCsConst.HasComplexScanSlot(WidgetType) == false)
                return true;

            return m_widget.m_isDeleted;
        }
        public CyTerminal GetHeaderTerminal()
        {
            CyTerminal head = null;
            for (int k = 0; k < m_listTerminals.Count; k++)
            {
                if (m_listTerminals[k].WidgetName == WidgetName)
                    head = m_listTerminals[k];
            }
            return head;
        }
        public string GetHeaderTerminalName()
        {
            return GetHeaderTerminal() != null ? GetHeaderTerminal().ToString() : string.Empty;
        }
        public bool IsScanSlotComplex()
        {
            return m_listTerminals.Count > 1 || GetHeaderTerminal() == null;
        }

        public string GenHeader()
        {
            string res = GetHeaderTerminalName();
            if (res== string.Empty) 
                res = m_widget.ToString();
            return res;
        }
        public bool IsEmptyComplexScanSlot()
        {
            return (CyCsConst.HasComplexScanSlot(WidgetType) && m_listTerminals.Count == 0);
        }
        #endregion
    }
    #endregion
}
