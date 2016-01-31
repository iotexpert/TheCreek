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


namespace CapSense_v1_20
{
    [Serializable()]
    public class CySSProperties : ICyMyPostSerialization
    {
        //Default values
        static ushort m_def_IDACSettings = 20;
        static ushort m_def_IDACRange = 0;
        public static object[] m_listCIS = new object[] {
            "Ground",
            "High-Z",
            "Shield"};
        public static int m_removeAtShieldNone = 2;

        [XmlIgnore()]
        [NonSerialized]
        public CyShareSSProps m_baseProps;// = new clShareSSProps();

        //Idac
        [XmlAttribute("IDACRange")]
        public int m_IDACRange = m_def_IDACRange;

        [XmlElement("IDACSettings")]
        public CyIntElement m_IDACSettings = new CyIntElement(m_def_IDACSettings);

        [XmlAttribute("CIS")]
        public int m_CIS = 0;

        [XmlIgnore()]
        public bool Custom
        {
            get
            {
                if (m_IDACRange != m_def_IDACRange) return true;
                if (m_IDACSettings == null) return true;
                if (m_IDACSettings.m_Value != m_def_IDACSettings) return true;
                return m_baseProps.m_bCustom;
            }
            set
            {
                m_baseProps.m_bCustom = value;
                if (m_baseProps.m_bCustom == false)
                    SetDefaultValues();
            }
        }
        public void SetDefaultValues()
        {
            m_IDACRange = m_def_IDACRange;
            m_IDACSettings.m_Value = m_def_IDACSettings;
            m_CIS = 0;
            m_baseProps.SetDefaultValues();
        }

        public void ExecutePostSerialization() { }

        public CySSProperties() { ExecutePostSerialization(); }

        #region Convert Functions
        public string GetResolution(string instanceName)
        {
            string[] strResolution = new string[]{
            "_PWM_RESOLUTION_8_BITS",
            "_PWM_RESOLUTION_9_BITS",
            "_PWM_RESOLUTION_10_BITS",
            "_PWM_RESOLUTION_11_BITS",
            "_PWM_RESOLUTION_12_BITS",
            "_PWM_RESOLUTION_13_BITS    ",
            "_PWM_RESOLUTION_14_BITS",
            "_PWM_RESOLUTION_15_BITS",
            "_PWM_RESOLUTION_16_BITS"
                };
            return instanceName + strResolution[m_baseProps.m_Resolution];
        }
        public string GetScanSpeed(string instanceName)
        {
            string[] strScanSpeed = new string[]{
                "_SCAN_SPEED_ULTRA_FAST",
                "_SCAN_SPEED_FAST",
                "_SCAN_SPEED_NORMAL",
                "_SCAN_SPEED_SLOW"
                };
            return instanceName + strScanSpeed[m_baseProps.m_ScanSpeed];
        }
        #endregion


    }

    [Serializable()]
    public class CyShareSSProps : ICyMyPostSerialization, ICyStepeble
    {
        [XmlAttribute("RefVal")]
        public int m_RefVal = 0;

        [XmlElement("PrescPer")]
        public CyIntElement m_PrescPer = new CyIntElement(2);

        //CSD
        [XmlAttribute("Resolution")]
        public int m_Resolution = 0;
        [XmlAttribute("ScanSpeed")]
        public int m_ScanSpeed = 0;

        //CSA
        [XmlElement("Scanlength")]
        public CyIntElement m_Scanlength = new CyIntElement(6);

        [XmlElement("SettlingTime")]
        public CyIntElement m_SettlingTime = new CyIntElement(6);

        //Custom params
        [XmlAttribute("Custom")]
        public bool m_bCustom = false;

        public void SetDefaultValues()
        {
            m_PrescPer.m_Value = 2;
            m_Resolution = 0;
            m_ScanSpeed = 0;
        }
        public void ExecutePostSerialization()
        {
            m_PrescPer.TakeMinMax(1, 255);
        }
        public CyShareSSProps()
        {
            ExecutePostSerialization();
        }
    }

}

