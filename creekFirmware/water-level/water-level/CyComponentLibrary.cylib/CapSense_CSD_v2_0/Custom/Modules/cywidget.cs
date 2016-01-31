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
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;


namespace CapSense_CSD_v2_0
{
    #region Widget
    [Serializable()]
    public abstract class CyWidget : CyCsPropsFather
    {
        #region Header

        [XmlAttribute("Name")]
        public string m_name = string.Empty;
        [XmlAttribute("Type")]
        public CySensorType m_type;
        [XmlAttribute("Channel")]
        public CyChannelNumber m_channel = CyChannelNumber.First;
        
        [XmlAttribute("Count")]
        public int m_count = 1;

        [XmlAttribute("Angle")]
        public int m_angle = 0;

        [XmlElement("Location")]
        public Point m_location = new Point(int.MaxValue,int.MaxValue);

        [XmlAttribute("Fliped")]
        public bool m_fliped = false;

        [XmlAttribute("Fliped2D")]
        public bool m_flipedXY = false;

        [XmlAttribute("ScaleFactor")]
        public float m_scaleFactor = 1.0f; 

        [XmlIgnore]
        public CyUpdateWidgetSensorCount m_updateWidgetSensorCountDelegat;

        //Flag which indicates if widget is deleted
        [XmlIgnore]
        public bool m_isDeleted = false;
        #endregion

        #region TunerValues

        [XmlIgnore]
        public UInt16 m_position = 0xFFFF;

        #endregion

        #region Functions
        public CyWidget()
        {
        }

        public CyWidget(string name, CySensorType type, CyUpdateWidgetSensorCount updateDelegate)
        {
            this.m_name = name;
            this.m_type = type;
            m_updateWidgetSensorCountDelegat = updateDelegate;
            AddDefValues();
        }
        protected abstract void AddDefValues();

        public override object[] GetAdditionalProperties() { return new object[]{}; }

        public override string ToString()
        {
            return m_name + "__" + GetSufix(m_type, false);
        }
        public string GetWidgetDefine()
        {
            return m_name + "__" + GetSufix(m_type, true);
        }

        public virtual bool IsSame(CyWidget wi)
        {
            if (wi.m_name != m_name) return false;
            if (wi.m_type != m_type) return false;
            if (wi.ToString() != ToString()) return false;
            if (this.GetCount() != wi.GetCount()) return false;
            return true;
        }

        public virtual int GetCount()
        {
            return m_count;
        }

        public virtual int GetFullResolution()
        {
            return 0;
        }

        public string GetSufix(CySensorType type, bool forAPI)
        {            
            string res = string.Empty;
            if (m_name == CyCsConst.P_GUARD_SENSOR) return "GRD";
            switch (type)
            {
                case CySensorType.Button: res = "BTN"; break;
                case CySensorType.SliderLinear: res = "LS"; break;
                case CySensorType.SliderRadial: res = "RS"; break;
                case CySensorType.TouchpadColumn: res = "TP"; res += forAPI ? string.Empty : "Col"; break;
                case CySensorType.TouchpadRow: res = "TP"; res += forAPI ? string.Empty : "Row"; break;
                case CySensorType.MatrixButtonsColumn: res = "MB"; res += forAPI ? string.Empty : "Col"; break;
                case CySensorType.MatrixButtonsRow: res = "MB"; res += forAPI ? string.Empty : "Row"; break;
                case CySensorType.Proximity: res = "PROX"; break;
                case CySensorType.Generic: res = "GEN"; break;
                default: break;
            }
            return res;
        }

        #endregion
    }
    #endregion

    #region Button
    [Serializable()]
    public class CyButton : CyWidget
    {
        #region Header
        [XmlElement("Properties")]
        public CyTuningProperties m_properties = new CyTuningProperties();

        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName(CyCsConst.PROPS_DEDICATED_SENSORS)]
        [DefaultValue(CySenseorCount._1)]
        [TypeConverter(typeof(CyCsEnumConverter))]
        public CySenseorCount Count_Prox_Generic
        {
            get { return m_count == 0 ? CySenseorCount._0 : CySenseorCount._1; }
            set
            {
                int val = value == CySenseorCount._0 ? 0 : 1;
                if (m_count != val)
                {
                    int old_value = m_count;
                    m_count = val;
                    m_updateWidgetSensorCountDelegat(m_name, m_type, old_value, m_count);
                }
            }
        }

        public CyButton() : base() { }
        public CyButton(string name, CySensorType type, CyUpdateWidgetSensorCount updateDelegate)
            : base(name, type, updateDelegate)
        { }

        #endregion

        #region Functions
        public override object[] GetAdditionalProperties()
        {
            return new object[] { m_properties };
        }
        public override PropertyDescriptorCollection ValidateProperties(PropertyDescriptorCollection pb)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);

            for (int i = 0; i < pb.Count; i++)
            {
                if (m_type == CySensorType.Generic)
                {
                    //Generic only has RESOLUTION propertie
                    if (pb[i].DisplayName == CyCsConst.PROPS_NAME_RESOLUTION)
                        result.Add(pb[i]);
                    continue;
                }

                if (pb[i].DisplayName ==CyCsConst.PROPS_DEDICATED_SENSORS)
                {
                    if (m_type == CySensorType.Proximity) result.Add(pb[i]);
                }
                else
                    result.Add(pb[i]);
            }
            return result;
        }
        protected override void AddDefValues()
        {
            m_count = 1;
        }
        #endregion
    }

    #endregion

    #region Slider
    [Serializable()]
    public class CySlider : CyWidget
    {
        #region Header
        [XmlAttribute("Diplexing")]
        public bool m_diplexing = false;

        [XmlAttribute("Resolution")]
        public int m_resolution = CyCsConst.C_WIDGET_RESOLUTION_DEF;

        [XmlAttribute("FilterPropertiesPos")]
        public CyPosFilterOptions m_filterPropertiesPos = CyCsConst.C_WIDGET_POS_FILTER_DEF;

        [XmlElement("Properties")]
        public CyTuningProperties m_properties = new CyTuningProperties();

        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName("Number of Sensor Elements")]
        [DefaultValue(CyCsConst.C_WIDGET_COUNT_DEF)]
        public int CountSlider
        {
            get { return m_count; }
            set
            {
                if (m_count != value && CyCsConst.C_WIDGET_COUNT.CheckRange(value,true))
                {
                    int old_value = m_count;
                    m_count = value;
                    m_updateWidgetSensorCountDelegat(m_name, m_type, old_value, m_count);
                }
            }
        }

        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DefaultValue(CyCsConst.C_WIDGET_RESOLUTION_DEF)]
        [DisplayName("API Resolution")]
        public int Resolution
        {
            get { return m_resolution; }
            set { if (CyCsConst.C_WIDGET_RESOLUTION.CheckRange(value, true)) m_resolution = value; }
        }

        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DefaultValue(CyDiplexed.NonDiplexed)]
        [DisplayName(CyCsConst.PROPS_NAME_DIPLEXING)]
        [TypeConverter(typeof(CyCsEnumConverter))]
        public CyDiplexed Diplexing
        {
            get { return m_diplexing && m_type == CySensorType.SliderLinear ? CyDiplexed.Diplexed : 
                CyDiplexed.NonDiplexed; }
            set { m_diplexing = value == CyDiplexed.Diplexed; }
        }

        [Description("Determines Position Noise Filter."), DisplayName("Position Noise Filter")]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DefaultValueAttribute(CyCsConst.C_WIDGET_POS_FILTER_DEF)]
        [TypeConverter(typeof(CyCsEnumConverter))]
        [XmlIgnore]
        public CyPosFilterOptions PositionNoiseFilter 
        {
            get { return m_filterPropertiesPos; }
            set
            {
               m_filterPropertiesPos=value;
            }
        }

        public CySlider()
        { }
        public CySlider(string name, CySensorType type, CyUpdateWidgetSensorCount updateDelegate)
            : base(name, type, updateDelegate)
        { }
        #endregion

        #region Functions
        public override object[] GetAdditionalProperties()
        {
            return new object[] { m_properties };
        }
        public override PropertyDescriptorCollection ValidateProperties(PropertyDescriptorCollection pb)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            for (int i = 0; i < pb.Count; i++)
            {
                if ((pb[i].DisplayName == CyCsConst.PROPS_NAME_DIPLEXING) && m_type == CySensorType.SliderRadial)
                    continue;
                if ((pb[i].DisplayName == CyCsConst.PROPS_NAME_HYSTERESIS))
                    continue;
                if ((pb[i].DisplayName == CyCsConst.PROPS_NAME_DEBOUNCE))
                    continue;
                result.Add(pb[i]);
            }
            return result;
        }
        protected override void AddDefValues()
        {
            this.m_count = CyCsConst.C_WIDGET_COUNT_DEF;
        }
        public override int GetFullResolution()
        {
            return CalculateResolution(m_resolution);
        }
        int CalculateResolution(int res)
        {
            if (m_type == CySensorType.SliderRadial)
            {
                return (res * 256) / m_count;
            }
            else
            {
                if (m_diplexing)
                    return (res * 256) / (2 * m_count - 1);
                else
                    return (res * 256) / (m_count - 1);
            }
        }
        public int[] GenerateDiplexIndexes()
        {
            int length = GetCount();
            int i, k;
            int[] arrIndexs = new int[2 * length];
            if ((m_type == CySensorType.SliderLinear))
            {
                i = 0;
                k = 0;
                for (i = 0; i < length; i++)
                {
                    arrIndexs[i] = i;
                }
                //New algorithm
                i = length;
                for (int j = 0; j < 3; j++)
                {
                    k = j;
                    while (k < length)
                    {
                        arrIndexs[i++] = k;
                        k += 3;
                    }
                }
            }
            return arrIndexs;
        }
        #endregion
    }
    #endregion

    #region MatrixButton
    [Serializable()]
    public class CyMatrixButton : CyWidget
    {
        #region Header
        [XmlAttribute("RowCount")]
        public int m_rowCount = CyCsConst.C_WIDGET_COUNT_DEF;

        [XmlAttribute("ColCount")]
        public int m_colCount = CyCsConst.C_WIDGET_COUNT_DEF;

        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName(CyCsConst.PROPS_NAME_ROW_COUNT)]
        [DefaultValue(CyCsConst.C_WIDGET_COUNT_DEF)]
        public int RowCount
        {
            get { return m_rowCount; }
            set 
            {
                if (m_rowCount != value && CyCsConst.C_WIDGET_COUNT.CheckRange(value, true))                    
                {
                    int old_value = m_rowCount;
                    m_rowCount = value;
                    m_updateWidgetSensorCountDelegat(m_name, CySensorType.MatrixButtonsRow, old_value, m_rowCount);
                }
            }
        }
        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName(CyCsConst.PROPS_NAME_COL_COUNT)]
        [DefaultValue(CyCsConst.C_WIDGET_COUNT_DEF)]
        public int ColCount
        {
            get { return m_colCount; }
            set
            {
                if (m_colCount != value && CyCsConst.C_WIDGET_COUNT.CheckRange(value, true))
                {
                    int old_value = m_colCount;
                    m_colCount = value;
                    m_updateWidgetSensorCountDelegat(m_name, CySensorType.MatrixButtonsColumn, old_value, m_colCount);
                }
            }
        }

        [XmlElement("PropertiesRows")]
        public CyTPRowsProperties m_propsRows = new CyTPRowsProperties();
        [XmlElement("PropertiesCols")]
        public CyTPColsProperties m_propsCols = new CyTPColsProperties();

        public CyMatrixButton()
        { }

        public CyMatrixButton(string name, CySensorType type, CyUpdateWidgetSensorCount updateDelegate)
            : base(name, type, updateDelegate)
        {
        }
        #endregion

        #region Functions
        public override object[] GetAdditionalProperties()
        {
            if (CyCsConst.IsMainPartOfWidget(CySensorType.MatrixButtonsColumn))
                return new object[] { m_propsCols, m_propsRows };
            else
                return new object[] { m_propsRows, m_propsCols };
        }
        protected override void AddDefValues()
        {
            this.m_rowCount = CyCsConst.C_WIDGET_COUNT_DEF;
            this.m_colCount = CyCsConst.C_WIDGET_COUNT_DEF;
            if (CyCsConst.IsMainPartOfWidget(m_type) == false)
            {
                //Remove properties objects because all widget properties are stored in Main part. 
                m_propsRows = null;
                m_propsCols = null;
            }
        }
        public override int GetCount()
        {
            return m_type == CySensorType.MatrixButtonsColumn ? m_colCount : m_rowCount;
        }
        #endregion
    }
    #endregion

    #region TouchPad
    [Serializable()]
    public class CyTouchPad : CyWidget
    {
        #region Header
        [XmlAttribute("RowCount")]
        public int m_rowCount = CyCsConst.C_WIDGET_COUNT_DEF;
        [XmlAttribute("ColCount")]
        public int m_colCount = CyCsConst.C_WIDGET_COUNT_DEF;

        [XmlAttribute("RowResolution")]
        public int m_rowResolution = CyCsConst.C_WIDGET_RESOLUTION_DEF;

        [XmlAttribute("ColResolution")]
        public int m_colResolution = CyCsConst.C_WIDGET_RESOLUTION_DEF;

        [XmlAttribute("FilterPropertiesPos")]
        public CyPosFilterOptions m_positionFilter = CyCsConst.C_WIDGET_POS_FILTER_DEF;

        [XmlElement("PropertiesRows")]
        public CyTPRowsProperties m_propsRows = new CyTPRowsProperties();
        [XmlElement("PropertiesCols")]
        public CyTPColsProperties m_propsCols = new CyTPColsProperties();

        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName(CyCsConst.PROPS_NAME_ROW_COUNT)]
        [DefaultValue(CyCsConst.C_WIDGET_COUNT_DEF)]
        public int RowCount
        {
            get { return m_rowCount; }
            set 
            {
                if (m_rowCount != value && CyCsConst.C_WIDGET_COUNT.CheckRange(value, true))                    
                {
                    int old_value = m_rowCount;
                    m_rowCount = value;
                    m_updateWidgetSensorCountDelegat(m_name, CySensorType.TouchpadRow, old_value, m_rowCount);
                }
            }
        }
        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName(CyCsConst.PROPS_NAME_COL_COUNT)]
        [DefaultValue(CyCsConst.C_WIDGET_COUNT_DEF)]
        public int ColCount
        {
            get { return m_colCount; }
            set 
            {
                if (m_colCount != value && CyCsConst.C_WIDGET_COUNT.CheckRange(value, true))
                {
                    int old_value = m_colCount;
                    m_colCount = value;
                    m_updateWidgetSensorCountDelegat(m_name, CySensorType.TouchpadColumn, old_value, m_colCount);
                }
            }
        }
        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName("Row API Resolution")]
        [DefaultValue(CyCsConst.C_WIDGET_RESOLUTION_DEF)]
        public int RowResolution
        {
            get { return m_rowResolution; }
            set { if (CyCsConst.C_WIDGET_RESOLUTION.CheckRange(value, true)) m_rowResolution = value; }
        }
        [XmlIgnore]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DisplayName("Column API Resolution")]
        [DefaultValue(CyCsConst.C_WIDGET_RESOLUTION_DEF)]
        public int ColResolution
        {
            get { return m_colResolution; }
            set { if (CyCsConst.C_WIDGET_RESOLUTION.CheckRange(value, true)) m_colResolution = value; }
        }
        [Description("Determines Position Noise Filter."), DisplayName("Position Noise Filter")]
        [Category(CyCsConst.CATEGORY_GENERAL)]
        [DefaultValueAttribute(CyCsConst.C_WIDGET_POS_FILTER_DEF)]
        [TypeConverter(typeof(CyCsEnumConverter))]
        [XmlIgnore]
        public CyPosFilterOptions PositionNoiseFilter
        {
            get { return m_positionFilter; }
            set
            {
                m_positionFilter = value;
            }
        }

        public CyTouchPad()
        {
        }
        public CyTouchPad(string name, CySensorType type, CyUpdateWidgetSensorCount updateDelegate)
            : base(name, type, updateDelegate)
        {
        }
        public override PropertyDescriptorCollection ValidateProperties(PropertyDescriptorCollection pb)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            for (int i = 0; i < pb.Count; i++)
            {
                if (pb[i].DisplayName.Contains(CyCsConst.PROPS_NAME_DEBOUNCE) ||
                    pb[i].DisplayName.Contains(CyCsConst.PROPS_NAME_HYSTERESIS))
                    continue;
                result.Add(pb[i]);
            }
            return result;
        }
        #endregion

        #region Function
        protected override void AddDefValues()
        {
            this.m_rowCount = CyCsConst.C_WIDGET_COUNT_DEF;
            this.m_colCount = CyCsConst.C_WIDGET_COUNT_DEF;
            this.m_rowResolution = CyCsConst.C_WIDGET_RESOLUTION_DEF;
            this.m_colResolution = CyCsConst.C_WIDGET_RESOLUTION_DEF;
            if (CyCsConst.IsMainPartOfWidget(m_type) == false)
            {
                //Remove properties objects because all widget properties are stored in Main part. 
                m_propsRows = null;
                m_propsCols = null;
            }
        }
        public override object[] GetAdditionalProperties()
        {
            if (CyCsConst.IsMainPartOfWidget(CySensorType.TouchpadColumn))
                return new object[] { m_propsCols, m_propsRows };
            else
                return new object[] { m_propsRows, m_propsCols };
        }
        public override int GetCount()
        {
            return m_type == CySensorType.TouchpadColumn ? m_colCount : m_rowCount;
        }
        public override int GetFullResolution()
        {
            return m_type == CySensorType.TouchpadColumn ? CalculateResolution(m_colResolution, m_colCount) :
            CalculateResolution(m_rowResolution, m_rowCount);
        }
        int CalculateResolution(int res, int num)
        {
            return (res * 256) / (num - 1);
        }
        public int GetResolution()
        {
            return m_type == CySensorType.TouchpadColumn ? m_colResolution : m_rowResolution;
        }
        #endregion
    }
    #endregion
}
