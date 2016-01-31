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
using System.Xml.Serialization;


namespace  CapSense_v1_10
{
    #region HAProps
    [Serializable()]
    public class CyHAProps : CyCsPropsFather
    {
        string m_defvalue = "0";
        [XmlElement("pTrProperties")]
        public CyHATrProperties m_TrProperties = new CyHATrProperties();
        [XmlElement("pMiscProperties")]
        public CyHAMiscProperties m_MiscProperties = new CyHAMiscProperties();
        [XmlElement("pFilterPropertiesPos")]
        public CyHAFilterPropertiesPos m_FilterPropertiesPos = new CyHAFilterPropertiesPos();
        [XmlElement("pFilterPropertiesRwDt")]
        public CyHAFilterPropertiesRwDt m_FilterPropertiesRwDt = new CyHAFilterPropertiesRwDt();
        public CyHAProps()
        {
            InitializeListObj();
        }
        public virtual void InitializeListObj()
        { }

        public String GetPropertyByName(Type type, object field)
        {
            foreach (object obj in m_listObjects)
                if (type == obj.GetType())
                {
                    return field.ToString();
                }

            //Getting minimum value for current type
            if (field.GetType() == typeof(CyIntElement))
            {
                return ((CyIntElement)field).m_Min.ToString();
            }
            return m_defvalue;
        }
        public bool IsRwDtFilter()
        {
            foreach (object obj in m_listObjects)
                if (typeof(CyHAFilterPropertiesRwDt) == obj.GetType())
                {
                    if (m_FilterPropertiesRwDt.JitterFilterRwDt == E_ENE_DIS.Enabled) return true;
                    if (m_FilterPropertiesRwDt.MedianFilterRwDt == E_ENE_DIS.Enabled) return true;
                }
            return false;
        }
        public bool IsPosFilter()
        {
            foreach (object obj in m_listObjects)
                if (typeof(CyHAFilterPropertiesPos) == obj.GetType())
                {
                    if (m_FilterPropertiesPos.JitterFilterPos == E_ENE_DIS.Enabled) return true;
                    if (m_FilterPropertiesPos.MedianFilterPos == E_ENE_DIS.Enabled) return true;
                }
            return false;
        }
        public uint GetFilterMask()
        {
            uint FilterConf = 0;
            foreach (object obj in m_listObjects)
                if (typeof(CyHAFilterPropertiesRwDt) == obj.GetType())
                {
                    CyHAFilterPropertiesRwDt filtObj = m_FilterPropertiesRwDt;
                    if (filtObj.JitterFilterRwDt == E_ENE_DIS.Enabled)
                        Cybit_operat.AddBit(ref FilterConf, 0);
                    if (filtObj.MedianFilterRwDt == E_ENE_DIS.Enabled)
                        Cybit_operat.AddBit(ref FilterConf, 1);
                    if (filtObj.AveragingFilterRwDt == E_ENE_DIS.Enabled)
                        Cybit_operat.AddBit(ref FilterConf, 2);
                }
                else if (typeof(CyHAFilterPropertiesPos) == obj.GetType())
                {
                    CyHAFilterPropertiesPos filtObj = m_FilterPropertiesPos;
                    if (filtObj.JitterFilterPos == E_ENE_DIS.Enabled)
                        Cybit_operat.AddBit(ref FilterConf, 3);
                    if (filtObj.MedianFilterPos == E_ENE_DIS.Enabled)
                        Cybit_operat.AddBit(ref FilterConf, 4);
                    if (filtObj.AveragingFilterPos == E_ENE_DIS.Enabled)
                        Cybit_operat.AddBit(ref FilterConf, 5);
                }
            return FilterConf;//"X", System.Windows.Forms.Application.CurrentCulture);
        }
    }
    #endregion

    #region HAMiscTreshProps
    [Serializable()]
    public class CyHAMiscTreshRDFilterProps : CyHAProps
    {
        public CyHAMiscTreshRDFilterProps()
            : base()
        { }
        public override void InitializeListObj()
        {
            m_listObjects.Clear();
            m_listObjects.Add(m_TrProperties);
            m_listObjects.Add(m_MiscProperties);
            m_listObjects.Add(m_FilterPropertiesRwDt);
        }
    }
    #endregion

    #region HAOnlyFilterProps
    [Serializable()]
    public class CyHAOnlyFilterProps : CyHAProps
    {
        public CyHAOnlyFilterProps()
            : base()
        { }
        public override void InitializeListObj()
        {
            m_listObjects.Clear();
            m_listObjects.Add(m_FilterPropertiesRwDt);
            m_listObjects.Add(m_FilterPropertiesPos);
        }
    }
    #endregion

    #region HAFilterTreshProps
    [Serializable()]
    public class CyHAFilterTreshProps : CyHAProps
    {
        public CyHAFilterTreshProps()
            : base()
        { }
        public override void InitializeListObj()
        {
            m_listObjects.Clear();
            m_listObjects.Add(m_TrProperties);
            m_listObjects.Add(m_FilterPropertiesRwDt);
            m_listObjects.Add(m_FilterPropertiesPos);
        }
    }
    #endregion

    #region HAFilterPropertiesPos
    [Serializable()]
    public class CyHAFilterPropertiesPos
    {
        E_ENE_DIS m_medianFilterPos = 0;
        E_ENE_DIS m_jitterFilterPos = 0;

        E_ENE_DIS m_averagingFilterPos = 0;
        public CyHAFilterPropertiesPos()
        {
        }
        [Description("Determines Jitter Filter for Position"),//[0..255]"),
DisplayName("Jitter Filter for Position")]
        [DefaultValueAttribute(E_ENE_DIS.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("JitterFilterPos")]
        public E_ENE_DIS JitterFilterPos
        {

            get { return m_jitterFilterPos; }
            set { m_jitterFilterPos = value; }
        }
        [Description("Determines Median Filter for Position"),//[0..255]"),
DisplayName("Median Filter for Position")]
        [DefaultValueAttribute(E_ENE_DIS.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("MedianFilterPos")]
        public E_ENE_DIS MedianFilterPos
        {

            get { return m_medianFilterPos; }
            set { m_medianFilterPos = value; }
        }

        [Description("Determines Averaging Filter for Position"),//[0..255]"),
DisplayName("Averaging Filter for Position")]
        [DefaultValueAttribute(E_ENE_DIS.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("AveragingFilterPos")]
        public E_ENE_DIS AveragingFilterPos
        {
            get { return m_averagingFilterPos; }
            set { m_averagingFilterPos = value; }
        }
    }

    #endregion

    #region HAFilterPropertiesRwDt
    [Serializable()]
    public class CyHAFilterPropertiesRwDt
    {
        [XmlAttribute("medianFilterRwDt")]
        public E_ENE_DIS m_medianFilterRwDt;
        [XmlAttribute("jitterFilterRwDt")]
        public E_ENE_DIS m_jitterFilterRwDt;
        [XmlAttribute("averagingFilterRwDt")]
        public E_ENE_DIS m_averagingFilterRwDt;
        public CyHAFilterPropertiesRwDt()
        {
            CyClDefProps.SetDefaultProps(this);
        }
        [Description("Determines Jitter Filter for Raw Data"),//[0..255]"),
DisplayName("Jitter Filter for Raw Data")]
        [DefaultValueAttribute(E_ENE_DIS.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("JitterFilterRwDt")]
        public E_ENE_DIS JitterFilterRwDt
        {

            get { return m_jitterFilterRwDt; }
            set { m_jitterFilterRwDt = value; }
        }
        [Description("Determines Median Filter for Raw Data"),//[0..255]"),
DisplayName("Median Filter for Raw Data")]
        [DefaultValueAttribute(E_ENE_DIS.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("MedianFilterRwDt")]
        public E_ENE_DIS MedianFilterRwDt
        {

            get { return m_medianFilterRwDt; }
            set { m_medianFilterRwDt = value; }
        }

        [Description("Determines Averaging Filter for Raw Data"),//[0..255]"),
DisplayName("Averaging Filter for Raw Data")]
        [DefaultValueAttribute(E_ENE_DIS.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("AveragingFilterRwDt")]
        public E_ENE_DIS AveragingFilterRwDt
        {

            get { return m_averagingFilterRwDt; }
            set { m_averagingFilterRwDt = value; }
        }
    }

    #endregion

    #region HAMiscProperties
    [Serializable()]
    public class CyHAMiscProperties : ICyMyPostSerialization
    {
        [XmlElement("m_Hysteresis")]
        public CyIntElement m_Hysteresis = new CyIntElement();
        [XmlElement("m_Debounce")]
        public CyIntElement m_Debounce = new CyIntElement();
        public void ExecutePostSerialization()
        {
            m_Hysteresis.TakeMinMax(0, 255);
            m_Debounce.TakeMinMax(1, 255);
        }
        public CyHAMiscProperties()
        {
            ExecutePostSerialization();
            CyClDefProps.SetDefaultProps(this);
        }
        #region Properties

        [Description("Determines Hysteresis "),//[0..255]"),
DisplayName("Hysteresis  "),
DefaultValueAttribute((byte)5)]
        //[Category("Thresholds")]
        [XmlAttribute("Hysteresis")]
        public int Hysteresis
        {

            get { return m_Hysteresis.m_Value; }
            set
            {
                m_Hysteresis.Validate((UInt16)value);
            }
        }
        [Description("Determines Debounce  "),//[0..255]"),
DisplayName("Debounce   "),
DefaultValueAttribute((byte)5)]
        //[Category("Thresholds")]
        [XmlAttribute("Debounce")]
        public int Debounce
        {
            get { return m_Debounce.m_Value; }
            set { m_Debounce.Validate((UInt16)value); }
        }
        #endregion
    }
    #endregion

    #region HATrProperties
    [Serializable()]
    public class CyHATrProperties : ICyMyPostSerialization
    {
        [XmlElement("m_FingerThreshold")]
        protected CyIntElement m_FingerThreshold = new CyIntElement();
        [XmlElement("m_NegativeNoiseThreshold")]
        protected CyIntElement m_NegativeNoiseThreshold = new CyIntElement();
        [XmlElement("m_NoiseThreshold")]
        protected CyIntElement m_NoiseThreshold = new CyIntElement();
        [XmlElement("m_BaselineUpdateThreshold")]
        protected CyIntElement m_BaselineUpdateThreshold = new CyIntElement();
        public void ExecutePostSerialization()
        {
            m_FingerThreshold.TakeMinMax(0, 250);
            m_NegativeNoiseThreshold.TakeMinMax(0, 255);
            m_NoiseThreshold.TakeMinMax(5, 255);
            m_BaselineUpdateThreshold.TakeMinMax(0, 255);
        }
        public CyHATrProperties()
        {
            ExecutePostSerialization();
            CyClDefProps.SetDefaultProps(this);
        }

        #region Properies
        [Description("The Finger Detection Threshold applied to all sensors of X and Y axis."),// Range is// [5..255]"),
        DisplayName("Finger Threshold"),
      DefaultValueAttribute(20)]
        [Category("Thresholds")]
        [XmlAttribute("FingerThreshold")]
        public int FingerThreshold
        {
            get { return m_FingerThreshold.m_Value; }
            set { m_FingerThreshold.Validate((UInt16)value); }
        }

        [Description("Determines the Noise Threshold"),// [3..255]"),
        DisplayName("Noise Threshold"),
        DefaultValueAttribute((byte)10)]
        [Category("Thresholds")]
        [XmlAttribute("NoiseThreshold")]
        public int NoiseThreshold
        {

            get { return m_NoiseThreshold.m_Value; }
            set { m_NoiseThreshold.Validate((UInt16)value); }
        }
        #endregion

    }
    #endregion

    #region M_bit_operat
    public static class Cybit_operat
    {
        public static void AddBit(ref UInt32 val, int pos)
        {
            UInt32 temp = 1;
            for (int i = 0; i < pos; i++)
                temp = temp << 1;
            val = val | temp;
        }

        public static void AddBit(ref UInt16 val, int pos)
        {
            UInt16 temp = 1;
            for (int i = 0; i < pos; i++)
                temp = (UInt16)(temp << 1);
            val = (UInt16)(val | temp);
        }

        public static void AddBitRange(ref UInt32 val, UInt32 sourse, int pos)
        {
            sourse = sourse << pos;
            val = val | sourse;
        }

        public static bool IsTrue(UInt32 val, int pos)
        {
            UInt32 temp = 1;
            for (int i = 0; i < pos; i++)
                temp = temp << 1;
            if ((temp & val) > 0)
                return true;
            return false;
        }
    }
    #endregion
}
