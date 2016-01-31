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
using System.Drawing;

namespace  CapSense_v1_30
{
    #region ElWidget
    [Serializable()]
    public class CyElWidget
    {
        [XmlAttribute("Name")]
        public string m_Name = "";
        [XmlAttribute("type")]
        public E_SENSOR_TYPE m_type;
        [XmlAttribute("side")]
        public E_EL_SIDE m_side = E_EL_SIDE.Left;
        [XmlAttribute("Enable")]
        public bool m_Enable = true;

        [XmlElement("baseSSProps")]
        public CyShareSSProps m_baseSSProps = new CyShareSSProps();

        [XmlAttribute("Count")]
        public int m_Count = 0;

        public CyElWidget()
        {
        }
        public CyElWidget(string name, E_SENSOR_TYPE type)
        {
            this.m_Name = name;
            this.m_type = type;
        }
        public CyElWidget(object[] props, E_SENSOR_TYPE type)
            :this((string)props[0],type)
        {
            this.m_side = CyColLocation.GetSide(props[props.Length - 1]);
        }

        public virtual CyHAProps GetProperties() { return null; }

        public virtual void SetObjData(object[] props) { }

        public override string ToString()
        {
            return CyElWidget.GetPrefix(m_type) + "_" + m_Name;
        }
        public virtual bool IsSameFull(CyElWidget wi)
        {
            if (wi.m_Name != m_Name) return false;
            if (wi.m_type != m_type) return false;
            if (wi.ToString() != ToString()) return false;
            if (this.GetCount() != wi.GetCount()) return false;
            return true;
        }
        public bool IsSameNameType(CyElWidget wi)
        {
            if (wi.m_Name != m_Name) return false;
            if (wi.m_type != m_type) return false;
            return true;
        }
        public E_COMPARER_TYPE ChangesSearch(CyElWidget wi)
        {
            E_COMPARER_TYPE res = 0;
            if (wi.m_Name != m_Name) res = res | E_COMPARER_TYPE.NameChange;
            if (wi.m_type != m_type) res = res | E_COMPARER_TYPE.TypeChange;
            if (wi.m_side != m_side) res = res | E_COMPARER_TYPE.SideChange;
            if (this.GetCount() != wi.GetCount()) res = res | E_COMPARER_TYPE.CountChange;
            return res;
        }

        public virtual int GetCount()
        {
            return m_Count;
        }
        public virtual int GetFullResolution()
        {
            return 0;
        }
        public static string GetPrefixCSHL(Type type)
        {
            if (type == typeof(CyElUnButton)) return "Btn";
            if (type == typeof(CyElUnSlider)) return "Sl";
            if (type == typeof(CyElUnTouchPad)) return "TP";
            if (type == typeof(CyElUnMatrixButton)) return "MB";
            return "";
        }
        public static string GetPrefix(E_SENSOR_TYPE type)
        {
            string res = "";
            switch (type)
            {
                case E_SENSOR_TYPE.Button: res = "BTN"; break;

                case E_SENSOR_TYPE.Linear_Slider: res = "LS" ; break;

                case E_SENSOR_TYPE.Radial_Slider: res = "RS" ; break;

                case E_SENSOR_TYPE.Touchpad_Col: res = "TP" + "Col" ; break;

                case E_SENSOR_TYPE.Touchpad_Row: res = "TP" + "Row" ; break;

                case E_SENSOR_TYPE.Matrix_Buttons_Col: res = "MB" + "Col" ; break;

                case E_SENSOR_TYPE.Matrix_Buttons_Row: res = "MB" + "Row" ; break;

                case E_SENSOR_TYPE.Proximity: res = "PROX" ; break;

                case E_SENSOR_TYPE.Generic: res = "GEN" ; break;

                default:
                    break;
            }
            return res;
        }

    }
    #endregion

    #region ElUnButton
    [Serializable()]
    public class CyElUnButton : CyElWidget
    {
        [XmlElement("Properties")]
        public CyHAMiscTreshRDFilterProps m_Properties = new CyHAMiscTreshRDFilterProps();

        public CyElUnButton(object[] props, E_SENSOR_TYPE type)
            : base(props, type)
        {
            SetObjData(props);
        }
        public CyElUnButton() { }

        public override CyHAProps GetProperties()
        {
            return m_Properties;
        }
        public override void SetObjData(object[] props)
        {
            m_Name = Convert.ToString(props[0]);

            //For Generic and Proximity types
            if (CySensorType.IsCustomCase(m_type))
            {
                m_Count = Convert.ToInt32(props[1]);
            }
        }
    }

    #endregion

    #region ElUnSlider

    [Serializable()]
    public class CyElUnSlider : CyElWidget
    {
        [XmlAttribute("diplexing")]
        public bool m_diplexing = false;
        [XmlAttribute("Resolution")]
        public int m_Resolution = 0;
        public CyHAFilterTreshProps m_Properties = new CyHAFilterTreshProps();
        public CyElUnSlider()
        { }

        public CyElUnSlider(E_SENSOR_TYPE type, object[] props)
            : base(props, type)
        {
            SetObjData(props);
        }

        public override CyHAProps GetProperties()
        {
            return m_Properties;
        }
        public override void SetObjData(object[] props)
        {
            this.m_Name = Convert.ToString(props[0]);
            this.m_Count = Convert.ToInt32(props[2]);
            this.m_Resolution = Convert.ToInt32(props[3]);
            this.m_diplexing = Convert.ToBoolean(props[4]);
        }
        public override int GetFullResolution()
        {
            return CalculateResolution(m_Resolution);
        }
        int CalculateResolution(int res)
        {
            if (m_type == E_SENSOR_TYPE.Radial_Slider)
            {
                return res;
            }
            else
            {
                if (m_diplexing)
                    return (res * 256) / (2 * m_Count - 1);
                else
                    return (res * 256) / (m_Count - 1);
            }
        }
    }
    #endregion

    #region ElUnMatrixButton
    [Serializable()]
    public class CyElUnMatrixButton : CyElWidget
    {
        [XmlAttribute("numElRow")]
        public int m_numElRow = 0;
        [XmlAttribute("numElCol")]
        public int m_numElCol = 0;

        [XmlElement("Properties")]
        public CyHAMiscTreshRDFilterProps m_Properties = new CyHAMiscTreshRDFilterProps();

        public CyElUnMatrixButton()
        { }

        public CyElUnMatrixButton(object[] props, E_SENSOR_TYPE type)
            : base((string)props[0], type)
        {
            SetObjData(props);
            if (type == E_SENSOR_TYPE.Matrix_Buttons_Col)
                this.m_side = CyColLocation.GetSide(props[props.Length - 1]);
            else if (type == E_SENSOR_TYPE.Matrix_Buttons_Row)
            {
                this.m_side = CyColLocation.GetSide(props[props.Length - 2]);
                m_Properties = null;
            }
        }

        public override CyHAProps GetProperties()
        {
            return m_Properties;
        }
        public override void SetObjData(object[] props)
        {
            this.m_Name = Convert.ToString(props[0]);
            this.m_numElRow = Convert.ToInt32(props[1]);
            this.m_numElCol = Convert.ToInt32(props[2]);
        }
        public override int GetCount()
        {
            if (m_type == E_SENSOR_TYPE.Matrix_Buttons_Col) return m_numElCol;
            if (m_type == E_SENSOR_TYPE.Matrix_Buttons_Row) return m_numElRow;
            return 0;
        }
    }
    #endregion

    #region ElUnTouchPad
    [Serializable()]
    public class CyElUnTouchPad : CyElWidget
    {
        [XmlAttribute("numElRow")]
        public int m_numElRow = 0;
        [XmlAttribute("numElCol")]
        public int m_numElCol = 0;

        [XmlAttribute("resElRow")]
        public int m_resElRow = 0;
        [XmlAttribute("resElCol")]
        public int m_resElCol = 0;

        [XmlElement("Properties")]
        public CyHAOnlyFilterProps m_Properties = new CyHAOnlyFilterProps();
        [XmlElement("PropsRows")]
        public CyHATrProperties m_PropsRows = new CyHATrProperties();
        [XmlElement("PropsCols")]
        public CyHATrProperties m_PropsCols = new CyHATrProperties();

        public CyElUnTouchPad()
        {
        }
        public CyElUnTouchPad(object[] props, E_SENSOR_TYPE type)
            : base((string)props[0], type)
        {
            SetObjData(props);
            if (type == E_SENSOR_TYPE.Touchpad_Col)
                this.m_side = CyColLocation.GetSide(props[props.Length - 1]);
            else if (type == E_SENSOR_TYPE.Touchpad_Row)
            {
                this.m_side = CyColLocation.GetSide(props[props.Length - 2]);
                m_Properties = null;
                m_PropsRows = null;
                m_PropsCols = null;
            }
        }
        public override void SetObjData(object[] props)
        {
            this.m_Name = Convert.ToString(props[0]);
            this.m_numElRow = Convert.ToInt32(props[1]);
            this.m_numElCol = Convert.ToInt32(props[2]);
            this.m_resElRow = Convert.ToInt32(props[3]);
            this.m_resElCol = Convert.ToInt32(props[4]);
        }
        public override CyHAProps GetProperties()
        {
            return m_Properties;
        }
        public CyHATrProperties GetSeparateProperties()
        {
            if (CySensorType.IsRow(m_type)) return m_PropsRows;
            else return m_PropsCols;
        }
        public string ToHeaderString()
        {
            return "TP" + "_" + m_Name;
        }

        public override int GetCount()
        {
            if (CySensorType.IsRow(m_type)) return m_numElRow;
            else
                return m_numElCol;
        }
        public override int GetFullResolution()
        {
            if (m_type == E_SENSOR_TYPE.Touchpad_Col) return CalculateResolution(m_resElCol,m_numElCol);
            if (m_type == E_SENSOR_TYPE.Touchpad_Row) return CalculateResolution(m_resElRow,m_numElRow);
            return 0;
        }
        int CalculateResolution(int res,int num)
        {
            return (res * 256) / (num - 1);
        }
    }
    #endregion


}
