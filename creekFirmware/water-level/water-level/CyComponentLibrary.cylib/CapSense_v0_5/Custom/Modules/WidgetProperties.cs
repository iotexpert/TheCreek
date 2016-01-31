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


namespace  CapSense_v0_5
{
    #region HAProps
    [Serializable()]
    public class HAProps : CsPropsFather
    {
        string defvalue = "0";

        public HATrProperties pTrProperties = new HATrProperties();
        public HAMiscProperties pMiscProperties = new HAMiscProperties();
        public HAFilterPropertiesPos pFilterPropertiesPos = new HAFilterPropertiesPos();
        public HAFilterPropertiesRwDt pFilterPropertiesRwDt = new HAFilterPropertiesRwDt();
        public HAProps()
        {
            InitializeListObj();
        }
        public virtual void InitializeListObj()
        { }

        public String GetPropertyByName(Type type, object field)
        {
            foreach (object obj in listObjects)
                if (type == obj.GetType())
                {
                    return field.ToString();
                }

            //Getting minimum value for current type
            if (field.GetType() == typeof(IntElement))
            {
                return ((IntElement)field).Min.ToString();
            }
            return defvalue;
        }
        public bool isRwDtFilter()
        {
            foreach (object obj in listObjects)
                if (typeof(HAFilterPropertiesRwDt) == obj.GetType())
                {
                    if (pFilterPropertiesRwDt.JitterFilterRwDt == TEneDis.Enabled) return true;
                    if (pFilterPropertiesRwDt.MedianFilterRwDt == TEneDis.Enabled) return true;
                }
            return false;
        }
        public bool isPosFilter()
        {
            foreach (object obj in listObjects)
                if (typeof(HAFilterPropertiesPos) == obj.GetType())
                {
                    if (pFilterPropertiesPos.JitterFilterPos == TEneDis.Enabled) return true;
                    if (pFilterPropertiesPos.MedianFilterPos == TEneDis.Enabled) return true;
                }
            return false;
        }
        public uint GetFilterMask()
        {
            uint FilterConf = 0;
            foreach (object obj in listObjects)
                if (typeof(HAFilterPropertiesRwDt) == obj.GetType())
                {
                    HAFilterPropertiesRwDt filtObj = pFilterPropertiesRwDt;
                    if (filtObj.JitterFilterRwDt == TEneDis.Enabled)
                        M_bit_operat.AddBit(ref FilterConf, 0);
                    if (filtObj.MedianFilterRwDt == TEneDis.Enabled)
                        M_bit_operat.AddBit(ref FilterConf, 1);
                    if (filtObj.AveragingFilterRwDt == TEneDis.Enabled)
                        M_bit_operat.AddBit(ref FilterConf, 2);
                }
                else if (typeof(HAFilterPropertiesPos) == obj.GetType())
                {
                    HAFilterPropertiesPos filtObj = pFilterPropertiesPos;
                    if (filtObj.JitterFilterPos == TEneDis.Enabled)
                        M_bit_operat.AddBit(ref FilterConf, 3);
                    if (filtObj.MedianFilterPos == TEneDis.Enabled)
                        M_bit_operat.AddBit(ref FilterConf, 4);
                    if (filtObj.AveragingFilterPos == TEneDis.Enabled)
                        M_bit_operat.AddBit(ref FilterConf, 5);
                }
            return FilterConf;//"X", System.Windows.Forms.Application.CurrentCulture);
        }
    }
    #endregion

    #region HAMiscTreshProps
    [Serializable()]
    public class HAMiscTreshRDFilterProps : HAProps
    {
        public HAMiscTreshRDFilterProps()
            : base()
        { }
        public override void InitializeListObj()
        {
            listObjects.Clear();
            listObjects.Add(pTrProperties);
            listObjects.Add(pMiscProperties);
            listObjects.Add(pFilterPropertiesRwDt);
        }
    }
    #endregion

    #region HAOnlyFilterProps
    [Serializable()]
    public class HAOnlyFilterProps : HAProps
    {
        public HAOnlyFilterProps()
            : base()
        { }
        public override void InitializeListObj()
        {
            listObjects.Clear();
            listObjects.Add(pFilterPropertiesRwDt);
            listObjects.Add(pFilterPropertiesPos);
        }
    }
    #endregion

    #region HAFilterTreshProps
    [Serializable()]
    public class HAFilterTreshProps : HAProps
    {
        public HAFilterTreshProps()
            : base()
        { }
        public override void InitializeListObj()
        {
            listObjects.Clear();
            listObjects.Add(pTrProperties);
            listObjects.Add(pFilterPropertiesRwDt);
            listObjects.Add(pFilterPropertiesPos);
        }
    }
    #endregion

    #region HAFilterPropertiesPos
    [Serializable()]
    public class HAFilterPropertiesPos
    {
        TEneDis medianFilterPos = 0;
        TEneDis jitterFilterPos = 0;

        TEneDis averagingFilterPos = 0;
        public HAFilterPropertiesPos()
        {
        }
        [Description("Determines Jitter Filter for Position"),//[0..255]"),
DisplayName("Jitter Filter for Position")]
        [DefaultValueAttribute(TEneDis.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("JitterFilterPos")]
        public TEneDis JitterFilterPos
        {

            get { return jitterFilterPos; }
            set { jitterFilterPos = value; }
        }
        [Description("Determines Median Filter for Position"),//[0..255]"),
DisplayName("Median Filter for Position")]
        [DefaultValueAttribute(TEneDis.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("MedianFilterPos")]
        public TEneDis MedianFilterPos
        {

            get { return medianFilterPos; }
            set { medianFilterPos = value; }
        }

        [Description("Determines Averaging Filter for Position"),//[0..255]"),
DisplayName("Averaging Filter for Position")]
        [DefaultValueAttribute(TEneDis.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("AveragingFilterPos")]
        public TEneDis AveragingFilterPos
        {

            get { return averagingFilterPos; }
            set { averagingFilterPos = value; }
        }
    }

    #endregion

    #region HAFilterPropertiesRwDt
    [Serializable()]
    public class HAFilterPropertiesRwDt
    {
        public TEneDis medianFilterRwDt;
        public TEneDis jitterFilterRwDt;
        public TEneDis averagingFilterRwDt;
        public HAFilterPropertiesRwDt()
        {
            clDefProps.SetDefaultProps(this);
        }
        [Description("Determines Jitter Filter for Raw Data"),//[0..255]"),
DisplayName("Jitter Filter for Raw Data")]
        [DefaultValueAttribute(TEneDis.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("JitterFilterRwDt")]
        public TEneDis JitterFilterRwDt
        {

            get { return jitterFilterRwDt; }
            set { jitterFilterRwDt = value; }
        }
        [Description("Determines Median Filter for Raw Data"),//[0..255]"),
DisplayName("Median Filter for Raw Data")]
        [DefaultValueAttribute(TEneDis.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("MedianFilterRwDt")]
        public TEneDis MedianFilterRwDt
        {

            get { return medianFilterRwDt; }
            set { medianFilterRwDt = value; }
        }

        [Description("Determines Averaging Filter for Raw Data"),//[0..255]"),
DisplayName("Averaging Filter for Raw Data")]
        [DefaultValueAttribute(TEneDis.Disabled)]
        [TypeConverter(typeof(EnumConverter))]
        [Category("Filters Configuration")]
        [XmlAttribute("AveragingFilterRwDt")]
        public TEneDis AveragingFilterRwDt
        {

            get { return averagingFilterRwDt; }
            set { averagingFilterRwDt = value; }
        }
    }

    #endregion

    #region HAMiscProperties
    [Serializable()]
    public class HAMiscProperties : IMyPostSerialization
    {
        public IntElement m_Hysteresis = new IntElement();
        public IntElement m_Debounce = new IntElement();
        public void ExecutePostSerialization()
        {
            m_Hysteresis.takeMinMax(0, 255);
            m_Debounce.takeMinMax(1, 255);
        }
        public HAMiscProperties()
        {
            ExecutePostSerialization();
            clDefProps.SetDefaultProps(this);
        }
        #region Properties

        [Description("Determines Hysteresis "),//[0..255]"),
DisplayName("Hysteresis  "),
DefaultValueAttribute((byte)5)]
        //[Category("Thresholds")]
        [XmlAttribute("Hysteresis")]
        public int Hysteresis
        {

            get { return m_Hysteresis.Value; }
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
            get { return m_Debounce.Value; }
            set { m_Debounce.Validate((UInt16)value); }
        }
        #endregion
    }
    #endregion

    #region HATrProperties
    [Serializable()]
    public class HATrProperties : IMyPostSerialization
    {
        protected IntElement m_FingerThreshold = new IntElement();
        protected IntElement m_NegativeNoiseThreshold = new IntElement();
        protected IntElement m_NoiseThreshold = new IntElement();
        protected IntElement m_BaselineUpdateThreshold = new IntElement();
        public void ExecutePostSerialization()
        {
            m_FingerThreshold.takeMinMax(0, 250);
            m_NegativeNoiseThreshold.takeMinMax(0, 255);
            m_NoiseThreshold.takeMinMax(5, 255);
            m_BaselineUpdateThreshold.takeMinMax(0, 255);
        }
        public HATrProperties()
        {
            ExecutePostSerialization();
            clDefProps.SetDefaultProps(this);
        }

        #region Properies
        [Description("The Finger Detection Threshold applied to all sensors of X and Y axis."),// Range is// [5..255]"),
        DisplayName("Finger Threshold"),
      DefaultValueAttribute(20)]
        [Category("Thresholds")]
        [XmlAttribute("FingerThreshold")]
        public int FingerThreshold
        {
            get { return m_FingerThreshold.Value; }
            set { m_FingerThreshold.Validate((UInt16)value); }
        }

        //[Description("Determines the Baseline Update Threshold"),// [0..255]"),
        //DisplayName("Baseline Update Threshold"),
        // DefaultValueAttribute((byte)100)]
        //[Category("Thresholds")]
        //[XmlAttribute("BaselineUpdateThreshold")]
        //public int BaselineUpdateThreshold
        //{

        //    get { return m_BaselineUpdateThreshold.Value; }
        //    set { m_BaselineUpdateThreshold.Validate((UInt16)value); }
        //}
        [Description("Determines the Noise Threshold"),// [3..255]"),
        DisplayName("Noise Threshold"),
        DefaultValueAttribute((byte)10)]
        [Category("Thresholds")]
        [XmlAttribute("NoiseThreshold")]
        public int NoiseThreshold
        {

            get { return m_NoiseThreshold.Value; }
            set { m_NoiseThreshold.Validate((UInt16)value); }
        }

        //[Description("Determines the Negative Noise Threshold"),// [0..255]"),
        //DisplayName("Negative Noise Threshold"),
        //DefaultValueAttribute((byte)10)]
        //[Category("Thresholds")]
        //[XmlAttribute("NegativeNoiseThreshold")]
        //public int NegativeNoiseThreshold
        //{

        //    get { return m_NegativeNoiseThreshold.Value; }
        //    set { m_NegativeNoiseThreshold.Validate((UInt16)value); }
        //}
        #endregion

    }
    #endregion

    #region M_bit_operat
    public static class M_bit_operat
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
