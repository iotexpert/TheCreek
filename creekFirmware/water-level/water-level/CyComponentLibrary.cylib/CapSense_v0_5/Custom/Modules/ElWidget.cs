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

namespace  CapSense_v0_5
{


    #region ElWidget
    [Serializable()]
    public class ElWidget //: IDisposable
    {
        [XmlAttribute("Name")]
        public string Name = "";
        //public bool Visible = true;
        [XmlAttribute("type")]
        public sensorType type;
        [XmlAttribute("side")]
        public eElSide side = eElSide.Left;
        [XmlAttribute("Enable")]
        public bool Enable = true;

        public clShareSSProps baseSSProps = new clShareSSProps();

        public ElWidget()
        {
        }
        public ElWidget(object[] props, sensorType type)
        {
            this.Name = (string)props[0];
            this.type = type;
            this.side = CyColLocation.getSide(props[props.Length - 1]);
        }
        public ElWidget(string props, sensorType type)
        {
            this.Name = props;
            this.type = type;
        }
        public virtual HAProps GetProps() { return null; }

        public virtual void SetProps(object[] props) { }

        public override string ToString()
        {
            return ElWidget.GetPrefixPart(type) + "_" + Name;
        }
        public virtual bool isSame(ElWidget wi)
        {
            if (wi.Name != Name) return false;
            if (wi.type != type) return false;
            if (wi.ToString() != ToString()) return false;
            if (this.GetCount() != wi.GetCount()) return false;
            return true;
        }
        public  bool isAlmostSame(ElWidget wi)
        {
            if (wi.type != type) return false;
            if (wi.side != side) return false;
            if (this.GetCount() != wi.GetCount()) return false;
            return true;
        }
        public virtual int GetCount()
        {
            return 0;
        }
        public virtual int GetFullResolution()
        {
            return 0;
        }
        public static string GetPrefixFull(Type type)
        {
            if (type == typeof(ElUnButton)) return "Btn";
            if (type == typeof(ElUnSlider)) return "Sl";
            if (type == typeof(ElUnTouchPad)) return "TP";
            if (type == typeof(ElUnMatrixButton)) return "MB";
            return "";
        }
        public static string GetPrefixPart(sensorType type)
        {
            string res = "";
            switch (type)
            {
                case sensorType.Button: res = "BTN"; break;

                case sensorType.Linear_Slider: res = "LS" ; break;

                case sensorType.Radial_Slider: res = "RS" ; break;

                case sensorType.Touchpad_Col: res = "TP" + "Col" ; break;

                case sensorType.Touchpad_Row: res = "TP" + "Row" ; break;

                case sensorType.Matrix_Buttons_Col: res = "MB" + "Col" ; break;

                case sensorType.Matrix_Buttons_Row: res = "MB" + "Row" ; break;

                case sensorType.Proximity: res = "PROX" ; break;

                case sensorType.Generic: res = "GEN" ; break;

                default:
                    break;
            }
            return res;
        }

    }
    #endregion

    #region ElUnButton
    [Serializable()]
    public class ElUnButton : ElWidget
    {

        public HAMiscTreshRDFilterProps Properties = new HAMiscTreshRDFilterProps();

        [XmlAttribute("Count")]
        public int Count = 0;

        public ElUnButton(object[] props, sensorType type)
            : base(props, type)
        {
            SetProps(props);
        }
        public ElUnButton()
        { }
        public override HAProps GetProps()
        {
            return Properties;
        }
        public override void SetProps(object[] props)
        {
            Name = Convert.ToString(props[0]);
            if (cySensorType.IsCustomCase(type))
            {
                Count = Convert.ToInt32(props[1]);
            }
        }
        //public override bool isAlmostSame(ElWidget wi)
        //{
        //    return base.isSame(wi);
        //}
        public override int GetCount()
        {
            return Count;
        }

    }

    #endregion

    #region ElUnSlider

    [Serializable()]
    public class ElUnSlider : ElWidget
    {
        [XmlAttribute("diplexing")]
        public bool diplexing = false;
        [XmlAttribute("Count")]
        public int Count = 0;
        [XmlAttribute("Resolution")]
        public int Resolution = 0;
        public HAFilterTreshProps Properties = new HAFilterTreshProps();
        public ElUnSlider()
        { }

        public ElUnSlider(sensorType type, object[] props)
            : base(props, type)
        {
            SetProps(props);
        }

        public override HAProps GetProps()
        {
            return Properties;
        }
        public override void SetProps(object[] props)
        {
            this.Name = Convert.ToString(props[0]);
            this.Count = Convert.ToInt32(props[2]);
            this.Resolution = Convert.ToInt32(props[3]);
            this.diplexing = Convert.ToBoolean(props[4]);
        }
        public override int GetCount()
        {
            return Count;
        }
        public override int GetFullResolution()
        {
            return CalculateResolution(Resolution);
        }
        int CalculateResolution(int res)
        {
            if (type == sensorType.Radial_Slider)
            {
                return res;
            }
            else
            {
                if (diplexing)
                    return (res * 256) / (2 * Count - 1);
                else
                    return (res * 256) / (Count - 1);
            }
        }
    }
    #endregion

    #region ElUnMatrixButton
    [Serializable()]
    public class ElUnMatrixButton : ElWidget
    {
        [XmlAttribute("numElRow")]
        public int numElRow = 0;
        [XmlAttribute("numElCol")]
        public int numElCol = 0;

        public HAMiscTreshRDFilterProps Properties = new HAMiscTreshRDFilterProps();

        public ElUnMatrixButton()
        { }

        public ElUnMatrixButton(object[] props, sensorType type)
            : base((string)props[0], type)
        {
            SetProps(props);
            if (type == sensorType.Matrix_Buttons_Col)
                this.side = CyColLocation.getSide(props[props.Length - 1]);
            else if (type == sensorType.Matrix_Buttons_Row)
            {
                this.side = CyColLocation.getSide(props[props.Length - 2]);
                Properties = null;
            }
        }

        public override HAProps GetProps()
        {
            return Properties;
        }
        public override void SetProps(object[] props)
        {
            this.Name = Convert.ToString(props[0]);
            this.numElRow = Convert.ToInt32(props[1]);
            this.numElCol = Convert.ToInt32(props[2]);
        }
        public override int GetCount()
        {
            if (type == sensorType.Matrix_Buttons_Col) return numElCol;
            if (type == sensorType.Matrix_Buttons_Row) return numElRow;
            return 0;
        }
    }
    #endregion

    #region ElUnTouchPad
    [Serializable()]
    public class ElUnTouchPad : ElWidget
    {
        [XmlAttribute("numElRow")]
        public int numElRow = 0;
        [XmlAttribute("numElCol")]
        public int numElCol = 0;

        [XmlAttribute("resElRow")]
        public int resElRow = 0;
        [XmlAttribute("resElCol")]
        public int resElCol = 0;


        public HAOnlyFilterProps Properties = new HAOnlyFilterProps();
        public HATrProperties PropsRows = new HATrProperties();
        public HATrProperties PropsCols = new HATrProperties();

        public ElUnTouchPad()
        {
        }
        public ElUnTouchPad(object[] props, sensorType type)
            : base((string)props[0], type)
        {
            SetProps(props);
            if (type == sensorType.Touchpad_Col)
                this.side = CyColLocation.getSide(props[props.Length - 1]);
            else if (type == sensorType.Touchpad_Row)
            {
                this.side = CyColLocation.getSide(props[props.Length - 2]);
                Properties = null;
                PropsRows = null;
                PropsCols = null;
            }
        }
        public override void SetProps(object[] props)
        {
            this.Name = Convert.ToString(props[0]);
            this.numElRow = Convert.ToInt32(props[1]);
            this.numElCol = Convert.ToInt32(props[2]);
            this.resElRow = Convert.ToInt32(props[3]);
            this.resElCol = Convert.ToInt32(props[4]);
        }
        public override HAProps GetProps()
        {
            return Properties;
        }
        public HATrProperties GetSeparateProps()
        {
            if (cySensorType.IsRow(type)) return PropsRows;
            else return PropsCols;            
        }
        public string ToHeaderString()
        {
            //string Pref = "Col";
            //if (type == sensorType.Touchpads_Row) Pref = "Row";
            return cySensorType.GetBaseType(type) + "_" + Name;// + "_" +Pref;
        }

        public override int GetCount()
        {
            if (cySensorType.IsRow(type)) return numElRow;
            else
                return numElCol;
        }
        public override int GetFullResolution()
        {
            if (type == sensorType.Touchpad_Col) return CalculateResolution(resElCol,numElCol);
            if (type == sensorType.Touchpad_Row) return CalculateResolution(resElRow,numElRow);
            return 0;
        }
        int CalculateResolution(int res,int num)
        {
            return (res * 256) / (num - 1);
        }
    }
    #endregion


}
