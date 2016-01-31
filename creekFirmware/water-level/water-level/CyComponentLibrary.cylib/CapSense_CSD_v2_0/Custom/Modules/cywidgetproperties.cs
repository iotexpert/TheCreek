/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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
using System.Windows.Forms;


namespace  CapSense_CSD_v2_0
{
    #region HATrProperties
    [Serializable()]
    public class CyTuningProperties
    {
        [XmlElement("Hysteresis")]
        public int m_hysteresis = CyCsConst.C_HYSTERESIS_DEF;
        [XmlElement("Debounce")]
        public int m_debounce = CyCsConst.C_DEBOUNCE_DEF;
        [XmlElement("FingerThreshold")]
        public int m_fingerThreshold = CyCsConst.C_FINGER_THRESHOLD_DEF;
        [XmlElement("NoiseThreshold")]
        public int m_noiseThreshold = CyCsConst.C_NOISE_THRESHOLD_DEF;
        [XmlElement("ScanResolution")]
        public CyScanResolutionType m_scanResolution = CyCsConst.C_RESOLUTION_DEF;

        public CyTuningProperties()
        {
            CyClassPropsComparer.SetDefaultProps(this);
        }

        #region Properies
        [Description("The Finger Detection Threshold.")]
        [DisplayName("Finger Threshold")]
        [DefaultValueAttribute(CyCsConst.C_FINGER_THRESHOLD_DEF)]
        [Category(CyCsConst.CATEGORY_TUNING)]
        [XmlIgnore]
        public virtual int FingerThreshold
        {
            get { return m_fingerThreshold; }
            set
            {
                if (value + m_hysteresis > 255)
                {
                    MessageBox.Show(CyCsResource.HA_FT_HY, CyCsResource.WarningText, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (CyCsConst.C_FINGER_THRESHOLD.CheckRange(value, true))
                    m_fingerThreshold = value;
            }
        }

        [Description("Determines the Noise Threshold")]
        [DisplayName("Noise Threshold")]
        [DefaultValueAttribute(CyCsConst.C_NOISE_THRESHOLD_DEF)]
        [Category(CyCsConst.CATEGORY_TUNING)]
        [XmlIgnore]
        public virtual int NoiseThreshold
        {

            get { return m_noiseThreshold; }
            set { if (CyCsConst.C_NOISE_THRESHOLD.CheckRange(value, true)) m_noiseThreshold = value; }
        }
        [Description("Determines Hysteresis")]
        [DisplayName(CyCsConst.PROPS_NAME_HYSTERESIS)]
        [DefaultValueAttribute(CyCsConst.C_HYSTERESIS_DEF)]
        [Category(CyCsConst.CATEGORY_TUNING)]
        [XmlIgnore]
        public virtual int Hysteresis
        {

            get { return m_hysteresis; }
            set
            {
                if (value + m_fingerThreshold > 255)
                {
                    MessageBox.Show(CyCsResource.HA_FT_HY, CyCsResource.WarningText,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (CyCsConst.C_HYSTERESIS.CheckRange(value, true))
                    m_hysteresis = value;
            }
        }
        [Description("Determines Debounce")]
        [DisplayName(CyCsConst.PROPS_NAME_DEBOUNCE)]
        [DefaultValueAttribute(CyCsConst.C_DEBOUNCE_DEF)]
        [Category(CyCsConst.CATEGORY_TUNING)]
        [XmlIgnore]
        public virtual int Debounce
        {
            get { return m_debounce; }
            set { if (CyCsConst.C_DEBOUNCE.CheckRange(value, true))m_debounce = value; }
        }

        [Description("Determines Scan Resolution")]
        [DisplayName(CyCsConst.PROPS_NAME_RESOLUTION)]
        [DefaultValueAttribute(CyCsConst.C_RESOLUTION_DEF)]
        [Category(CyCsConst.CATEGORY_TUNING)]
        [TypeConverter(typeof(CyCsEnumConverter))]
        [XmlIgnore]
        public virtual CyScanResolutionType ScanResolution
        {
            get { return m_scanResolution; }
            set { m_scanResolution = value; }
        }
        #endregion
    }
  
    public class CyTPRowsProperties : CyTuningProperties
    {
        public CyTPRowsProperties()
            :base()
        {
        }        
        [DisplayName("Row Finger Threshold")]
        [Category(CyCsConst.CATEGORY_ROW_TUNING)]        
        [XmlIgnore]
        public override int FingerThreshold
        {
            get { return base.FingerThreshold; }
            set { base.FingerThreshold = value; }
        }        
        [DisplayName("Row Noise Threshold")]
        [Category(CyCsConst.CATEGORY_ROW_TUNING)]
        [XmlIgnore]
        public override int NoiseThreshold
        {
            get { return base.NoiseThreshold; }
            set { base.NoiseThreshold = value; }
        }        
        [DisplayName(CyCsConst.P_ROW +" "+ CyCsConst.PROPS_NAME_HYSTERESIS)]
        [Category(CyCsConst.CATEGORY_ROW_TUNING)]
        [XmlIgnore]
        public override int Hysteresis
        {

            get { return base.Hysteresis; }
            set { base.Hysteresis = value; }
        }
        [DisplayName(CyCsConst.P_ROW + " " + CyCsConst.PROPS_NAME_DEBOUNCE)]
        [Category(CyCsConst.CATEGORY_ROW_TUNING)]
        [XmlIgnore]
        public override int Debounce
        {
            get { return base.Debounce; }
            set { base.Debounce = value; }
        }        
        [DisplayName("Row Scan Resolution")]
        [Category(CyCsConst.CATEGORY_ROW_TUNING)]
        [XmlIgnore]
        public override CyScanResolutionType ScanResolution
        {
            get { return base.ScanResolution; }
            set { base.ScanResolution = value; }
        }
    }
    public class CyTPColsProperties : CyTuningProperties
    {
        public CyTPColsProperties()
            : base()
        {
        }
        [DisplayName("Column Finger Threshold")]
        [Category(CyCsConst.CATEGORY_COL_TUNING)]
        [XmlIgnore]
        public override int FingerThreshold
        {
            get { return base.FingerThreshold; }
            set { base.FingerThreshold = value; }
        }        
        [DisplayName("Column Noise Threshold")]
        [Category(CyCsConst.CATEGORY_COL_TUNING)]
        [XmlIgnore]
        public override int NoiseThreshold
        {
            get { return base.NoiseThreshold; }
            set { base.NoiseThreshold = value; }
        }
        [DisplayName(CyCsConst.P_COL + " " + CyCsConst.PROPS_NAME_HYSTERESIS)]
        [Category(CyCsConst.CATEGORY_COL_TUNING)]
        [XmlIgnore]
        public override int Hysteresis
        {

            get { return base.Hysteresis; }
            set { base.Hysteresis = value; }
        }
        [DisplayName(CyCsConst.P_COL + " " + CyCsConst.PROPS_NAME_DEBOUNCE)]
        [Category(CyCsConst.CATEGORY_COL_TUNING)]
        [XmlIgnore]
        public override int Debounce
        {
            get { return base.Debounce; }
            set { base.Debounce = value; }
        }
        [DisplayName("Column Scan Resolution")]
        [Category(CyCsConst.CATEGORY_COL_TUNING)]
        [XmlIgnore]
        public override CyScanResolutionType ScanResolution
        {
            get { return base.ScanResolution; }
            set { base.ScanResolution = value; }
        }
    }
    #endregion    
}
