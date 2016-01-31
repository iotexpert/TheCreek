/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace  CapSense_v0_5
{
    [Serializable()]
    public class clSSProperties: IMyPostSerialization
    {
        //Default values
        static ushort def_IDACSettings = 20;
        static ushort def_IDACRange = 0;

        [XmlIgnore()]
        [NonSerialized]
        public clShareSSProps baseProps;// = new clShareSSProps();

        //Idac
        [XmlAttribute("IDACRange")]
        public int IDACRange = def_IDACRange;

        //[XmlAttribute("IDACSettings")]
        public IntElement IDACSettings = new IntElement(def_IDACSettings);
        ////Custom params //Not Use
        //[XmlAttribute("Custom")]
        //public bool bCustom = false;

        [XmlIgnore()]
        public bool Custom
        {
            get 
            {
                if (IDACRange != def_IDACRange) return true;
                if (IDACSettings.Value != def_IDACSettings) return true;
                return baseProps.bCustom; 
            }
            set
            {
                baseProps.bCustom = value;
                if (baseProps.bCustom == false)
                    SetDefaultValues();
            }
        }
        public void SetDefaultValues()
        {
            IDACRange = def_IDACRange;
            IDACSettings.Value = def_IDACSettings;
            baseProps.SetDefaultValues();
        }

        public void ExecutePostSerialization()
        {
        }

        public clSSProperties() { ExecutePostSerialization(); }

        #region Convert Functions
        public string GetResolution(string instanceName)
        {
            string[] strResolution = new string[]{
            "_PWM_RESOLUTION_8_BITS",
            "_PWM_RESOLUTION_9_BITS",
            "_PWM_RESOLUTION_10_BITS",
            "_PWM_RESOLUTION_11_BITS",
            "_PWM_RESOLUTION_12_BITS",
            "_PWM_RESOLUTION_13_BITS	",
            "_PWM_RESOLUTION_14_BITS",
            "_PWM_RESOLUTION_15_BITS",
            "_PWM_RESOLUTION_16_BITS"
                };
            return instanceName+strResolution[baseProps.Resolution];
        }
        public string GetScanSpeed(string instanceName)
        {
            string[] strScanSpeed = new string[]{
                "_SCAN_SPEED_ULTRA_FAST",
                "_SCAN_SPEED_FAST",
                "_SCAN_SPEED_NORMAL",
                "_SCAN_SPEED_SLOW"
                };
            return instanceName+strScanSpeed[baseProps.ScanSpeed];
        }
        #endregion


    }

    [Serializable()]
    public class clShareSSProps : IMyPostSerialization
    {
        //Head
        [XmlAttribute("CIS")]
        public int CIS = 0;
        [XmlAttribute("RefVal")]
        public int RefVal = 0;
        //[XmlAttribute("PrescPer")]
        public IntElement PrescPer = new IntElement(2);

        //CSD
        [XmlAttribute("Resolution")]
        public int Resolution = 0;
        [XmlAttribute("ScanSpeed")]
        public int ScanSpeed = 0;

        //CSA
        //[XmlAttribute("Scanlength")]
        public IntElement Scanlength = new IntElement(6);

        //[XmlAttribute("SettlingTime")]
        public IntElement SettlingTime = new IntElement(6);

        //Custom params
        [XmlAttribute("Custom")]
        public bool bCustom = false;

        public void SetDefaultValues()
        {
            PrescPer.Value = 1;
            Resolution = 0;
            ScanSpeed = 0;
        }
        public void ExecutePostSerialization()
        {
            PrescPer.takeMinMax(2, 255);
        }
        public  clShareSSProps() 
        {
            ExecutePostSerialization();
        }
    }

}

