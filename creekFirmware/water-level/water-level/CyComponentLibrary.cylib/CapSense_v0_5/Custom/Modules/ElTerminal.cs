/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;


namespace  CapSense_v0_5
{
    #region Terminals Classes
    [Serializable()]
    public class ElTerminal
    {

        [XmlAttribute("Name")]
        public string Name;
        [XmlAttribute("Nameindex")]
        public int Nameindex = -1;
        [XmlAttribute("Method")]
        public eCapSenseMode method = eCapSenseMode.CSD;
        [XmlAttribute("Enable")]
        public bool Enable = false;

        [XmlAttribute("IsLeft")]
        public eElSide haveSide = eElSide.None;

        [XmlAttribute("Type")]
        public sensorType type;

        Color color = CyGeneralParams.ColorLeft;

        public Color Color
        {
            get
            {
                switch (haveSide)
                {
                    case eElSide.None:
                        return Color.White;

                    case eElSide.Left:
                        return CyGeneralParams.ColorLeft;

                    case eElSide.Right:
                        return CyGeneralParams.ColorRight;

                    default:
                        return Color.White;
                }
            }

        }


        public ElTerminal()
        {
            this.haveSide = eElSide.None;
        }
        public ElTerminal(eElSide haveSide)
        {
            this.haveSide = haveSide;
        }
        public ElTerminal(string Name, sensorType type)
        {
            this.Name = Name;
            this.type = type;
            this.Enable = true;
            Nameindex = -1;
        }
        public ElTerminal(string Name, sensorType type, int Nameindex)
        {
            this.Name = Name;
            this.type = type;
            this.Enable = true;
            this.Nameindex = Nameindex;

        }
        public ElTerminal(string Name, bool Enable, eElSide haveSide)
        {
            this.Name = Name;
            this.Enable = Enable;
            this.haveSide = haveSide;
        }

        public string GetPref()
        {
            string res = "";
            if (haveSide == eElSide.Right) res = "R_";
            else if (haveSide == eElSide.Left) res = "L_";
            return res;
        }

        public override string ToString()
        {
            return getName();
        }
        public bool IsSameW(ElTerminal term)
        {
            if (Name != term.Name) return false;
            if (type != term.type) return false;
            //if (Nameindex != term.Nameindex) return false;

            return true;
        }
        public bool IsSameFull(ElTerminal term)
        {
            if (Name != term.Name) return false;
            if (type != term.type) return false;
            if (Nameindex != term.Nameindex) return false;
            if (ToString() != term.ToString()) return false;

            return true;
        }
        public string getName()
        {
            string res = "";
            switch (type)
            {
                case sensorType.Button: res = "BTN_" + Name; break;

                case sensorType.Linear_Slider: res = "LS_" + Name + "_e" + Nameindex; break;


                case sensorType.Radial_Slider: res = "RS_" + Name + "_e" + Nameindex; break;

                case sensorType.Touchpad_Col: res = "TP_" + Name + "_Col" + "_" + Nameindex; break;

                case sensorType.Touchpad_Row: res = "TP_" + Name + "_Row" + "_" + Nameindex; break;

                case sensorType.Matrix_Buttons_Col: res = "MB_" + Name + "_Col" + "_" + Nameindex; break;

                case sensorType.Matrix_Buttons_Row: res = "MB_" + Name + "_Row" + "_" + Nameindex; break;

                case sensorType.Proximity: res = "PROX_" + Name +"_"+ Nameindex; break;

                case sensorType.Generic: res = "GEN_" + Name + "_" + Nameindex; break;

                default:
                    break;
            }

            //For Custom Proximity and Generic case
            if (Nameindex == -1)
            {
                switch (type)
                {
                    case sensorType.Proximity: res = "PROX_" + Name; break;

                    case sensorType.Generic: res = "GEN_" + Name ; break;
                }
            }
            return res;

        }
    }

    #region Other
    //[Serializable()]
    //public class ElCMode : ElTerminal
    //{
    //    public ElCMode(eElSide haveSide)
    //        : base("", true, haveSide)
    //    {
    //    }
    //    public override string ToString()
    //    {

    //        return GetPref() + "Cmod";
    //    }
    //}
    //[Serializable()]
    //public class ElRbTerm : ElTerminal
    //{
    //    public ElRbTerm(eElSide haveSide)
    //        : base(haveSide)
    //    {
    //    }

    //    public override string ToString()
    //    {

    //        return GetPref() + "Rb";
    //    }
    //}
    //[Serializable()]
    //public class ElSETerm : ElTerminal
    //{
    //    public int Num = 0;
    //    public ElSETerm(eElSide haveSide, int Num)
    //        : base(haveSide)
    //    {
    //        this.Num = Num;
    //    }
    //    public override string ToString()
    //    {

    //        return GetPref() + "SE" + Num;
    //    }
    //}
    #endregion
    #endregion
}
