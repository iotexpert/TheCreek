/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace  CapSense_CSD_v2_0
{
    #region CyTerminals
    [Serializable()]
    public class CyTerminal
    {
        [XmlAttribute("NameIndex")]
        public int m_nameIndex = 0;

        string m_widgetName = string.Empty;

        [XmlIgnore]
        public CyWidget m_widget = null;

        [XmlIgnore()]
        public string Name
        {
            get
            {
                if (m_widget == null)
                {
                    System.Diagnostics.Debug.Assert(false);
                    return string.Empty;
                }
                return m_widget.m_name;
            }
        }
        [XmlIgnore()]
        public CySensorType WidgetType
        {
            get 
            {
                if (m_widget == null)
                {
                    System.Diagnostics.Debug.Assert(false);
                    return CySensorType.Button;
                }
                return m_widget.m_type; 
            }
        }

        [XmlIgnore]
        public CyChannelNumber Channel
        {
            get
            {
                if (m_widget == null)
                {
                    System.Diagnostics.Debug.Assert(false);
                    return CyChannelNumber.First;
                }
                return m_widget.m_channel;
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

        public CyTerminal()
        {
        }
        public CyTerminal(CyWidget wi,int nameindex)
        {
            this.m_widget = wi;
            this.m_nameIndex = nameindex;
        }
        public static string GetName(string name, CySensorType type, int nameIndex)
        {
            string res = name;
            if (name == CyCsConst.P_GUARD_SENSOR) return res += "__GRD";

            if (CyCsConst.HasComplexScanSlot(type))
                res += nameIndex != -1 ? "_" + nameIndex : string.Empty;
            switch (type)
            {
                case CySensorType.Button: res += "__BTN"; break;
                case CySensorType.SliderLinear: res += "_e" + nameIndex + "__LS"; break;
                case CySensorType.SliderRadial: res += "_e" + nameIndex + "__RS"; break;
                case CySensorType.TouchpadColumn: res += "_Col" + nameIndex + "__TP"; break;
                case CySensorType.TouchpadRow: res += "_Row" + nameIndex + "__TP"; break;
                case CySensorType.MatrixButtonsColumn: res += "_Col" + nameIndex + "__MB"; break;
                case CySensorType.MatrixButtonsRow: res += "_Row"+ nameIndex + "__MB"; break;
                case CySensorType.Proximity: res += "__PROX"; break;
                case CySensorType.Generic: res += "__GEN"; break;

                default:
                    break;
            }
            return res;
        }
        public override string ToString()
        {
            return GetName(Name, WidgetType, m_nameIndex);
        }
        public bool CompareTerminals(CyTerminal term)
        {
            if (m_widget != term.m_widget) return false;
            if (m_nameIndex != term.m_nameIndex) return false;
            if (ToString() != term.ToString()) return false;
            return true;
        }
    }
    #endregion
}
