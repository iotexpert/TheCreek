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
using System.ComponentModel;

namespace CapSense_CSD_v2_0
{
    #region Enums
    public enum CyChannelNumber { First = 0, Second = 1 }

    public enum CyEnDis { Disabled = 0, Enabled = 1 };

    public enum CyChannelConfig 
    {
        [Description("1")]
        ONE_CHANNEL = 0,
        [Description("2")]
        TWO_CHANNELS = 1 
    }

    public enum CyClockSourceOptions { Internal = 0, External = 1, BusClk = 2 }

    public enum CyFOFilterOptions
    {
        [Description("Disabled")]
        Disabled,
        [Description("0.5 previous + 0.5 current (Default)")]
        Prev0_5,
        [Description("0.75 previous + 0.25 current")]
        Prev0_75
    };

    public enum CyCurrentSourceOptions
    {
        [Description("External Resistor")]
        Idac_None = 0,
        [Description("IDAC Sourcing")]
        Idac_Source = 1,
        [Description("IDAC Sinking")]
        Idac_Sink = 2
    };
    public enum CyPrescalerOptions
    {
        [Description("Direct")]
        Prescaler_None,
        [Description("UDB Timer")]
        Prescaler_UDB,
        [Description("FF Timer")]
        Prescaler_FF
    };
    public enum CySensorType
    {
        Button = 0,
        SliderLinear = 1,
        SliderRadial = 2,
        TouchpadColumn = 4,
        TouchpadRow = 3,
        MatrixButtonsColumn = 6,
        MatrixButtonsRow = 5,
        Proximity = 7,
        Generic = 8
    }
    public enum CyFilterType
    {
        AnyPos,
        JitterFilterPos,
        MedianFilterPos,
        FirstOrderIIRFilter0_5Pos,
        FirstOrderIIRFilter0_75Pos,
        AveragingFilterPos
    }
    public enum CyRawDataFilterOptions
    {
        [Description("None")]
        None = 0x00,
        [Description("Median")]
        Median = 0x01,
        [Description("Averaging")]
        Averaging = 0x02,
        [Description("First Order IIR 1/2")]
        FirstOrderIIR0_5 = 0x04,
        [Description("First Order IIR 1/4")]
        FirstOrderIIR0_75 = 0x08,
        [Description("Jitter")]
        Jitter = 0x10,
        [Description("First Order IIR 1/8")]
        FirstOrderIIR0_125 = 0x20,
        [Description("First Order IIR 1/16")]
        FirstOrderIIR0_0625 = 0x40
    }
    public enum CyPosFilterOptions
    {
        [Description("None")]
        None = 0x00,
        [Description("Median")]
        Median = 0x01,
        [Description("Averaging")]
        Averaging = 0x02,
        [Description("First Order IIR 1/2")]
        FirstOrderIIR0_5 = 0x04,
        [Description("First Order IIR 1/4")]
        FirstOrderIIR0_75 = 0x08,
        [Description("Jitter")] 
        Jitter = 0x10,
    }

    public enum CyPrsOptions
    {
        [Description("Disabled")]
        Prs_None = 0,
        [Description("Enabled 8 bits")]
        Prs_8bits = 1,
        [Description("Enabled 16 bits, full speed")]
        Prs_16bits = 2,
        [Description("Enabled 16 bits, 1/4 speed")]
        Prs_16bits_0_25 = 3
    };
    public enum CyVrefOptions
    {
        Ref_Vref_1_24 = 0,
        Ref_Vdac = 2
    };

    public enum CyConnectInactiveSensorsOptions
    {
        [Description("Ground")]
        Ground = 0,
        [Description("Hi-Z Analog")]
        HiZ_Analog = 1,
        [Description("Shield")]
        Shield = 2
    }
    public enum CyScanSpeedOptions
    {
        [Description("Very Fast")]
        VeryFast = 1,
        [Description("Fast")]
        Fast = 3,
        [Description("Normal")]
        Normal = 7,
        [Description("Slow")]
        Slow = 15,
    }
    public enum CyIdacRangeOptions
    {
        [Description("32 uA")]
        fs_32uA = 0,
        [Description("255 uA")]
        fs_255uA = 4,
        [Description("2.04 mA")]
        fs_2040uA = 8
    }
    public enum CyMeasureImplemetation
    {
        [Description("FF Timer")]
        FF_Based = 0,
        [Description("UDB Timer")]
        UDB_Based = 1,
    }

    public enum CyTuningMethodOptions
    {
        [Description("None")]
        Tuning_None = 0,
        [Description("Manual")]
        Tuning_Manual = 1,
        [Description("Auto (SmartSense)")]
        Tuning_Auto = 2
    }
    public enum CySenseorCount
    {
        [Description("0")]
        _0,
        [Description("1")]
        _1
    };
    public enum CyScanResolutionType
    {
        [Description("8 bits")]
        _8 = 0x00,
        [Description("9 bits")]
        _9 = 0x01,
        [Description("10 bits")]
        _10 = 0x03,
        [Description("11 bits")]
        _11 = 0x07,
        [Description("12 bits")]
        _12 = 0x0F,
        [Description("13 bits")]
        _13 = 0x1F,
        [Description("14 bits")]
        _14 = 0x3F,
        [Description("15 bits")]
        _15 = 0x7F,
        [Description("16 bits")]
        _16 = 0xFF
    }
    public enum CyDiplexed
    {
        [Description("Diplexed")]
        Diplexed,
        [Description("Non diplexed")]
        NonDiplexed
    }
    #endregion
 
    public delegate void CyUpdateWidgetSensorCount(string name, CySensorType type, int oldCount, int newCount);

    #region CyCSEnums
    public static class CyCsConst
    {
        public const string CATEGORY_GENERAL = "General";
        public const string PROPS_DEDICATED_SENSORS = "Number of Dedicated Sensor Elements";
        public const string PROPS_NAME_COL_COUNT = "Number of Sensor Columns";
        public const string PROPS_NAME_ROW_COUNT = "Number of Sensor Rows";
        public const string PROPS_NAME_DIPLEXING = "Diplexing";
        public const string PROPS_NAME_HYSTERESIS = "Hysteresis";
        public const string PROPS_NAME_DEBOUNCE = "Debounce";
        public const string PROPS_NAME_RESOLUTION = "Scan Resolution";
        public const string PROPS_NAME_IDAC_VALUE = "IDAC Value";
        public const string CATEGORY_TUNING = "Tuning";
        public const string CATEGORY_COL_TUNING = "Column Tuning";
        public const string CATEGORY_ROW_TUNING = "Row Tuning";

        public const string P_SAVED_GENERAL_FILTER = "Text files (*.csxml)|*.csxml|All files (*.*)|*.*";
        public const string P_SAVED_WIDGETST_FILTER = "Text files (*.cswxml)|*.cswxml|All files (*.*)|*.*";
        public const string PATTERN_PORT_GENERAL = "<?xml version='1.0'?><{0} Version='1'>{1}</{0}>";
        public const string PATTERN_PIN_LIST = "<pin{0}>{1}</pin{0}>";

        public const string P_WIDGETS_DATA = "WidgetsData";        
        public const string P_SENSOR_NUMBER = "SensorNumber";

        public const string P_ROW = "Row";
        public const string P_COL = "Column";

        public const string P_GUARD_SENSOR = "GuardSensor";

        public const string P_SCAN_ORDER_TAB_NAME = "Scan Order";

        //Default values
        public const ushort C_RB_COUNT_DEF = 1;
        public const ushort C_IDAC_SETTINGS_DEF = 200;
        public const ushort C_SENSITIVITY_DEF = 2;

        public const ushort C_HYSTERESIS_DEF = 10;
        public const ushort C_DEBOUNCE_DEF = 5;
        public const ushort C_FINGER_THRESHOLD_DEF = 100;
        public const ushort C_NOISE_THRESHOLD_DEF = 20;
        public const int C_WIDGET_COUNT_DEF = 5;
        public const int C_WIDGET_RESOLUTION_DEF = 100;

        public const CyPosFilterOptions C_WIDGET_POS_FILTER_DEF = CyPosFilterOptions.FirstOrderIIR0_75;
        public const CyScanResolutionType C_RESOLUTION_DEF = CyScanResolutionType._10;
        
        public static CyCsElement C_DEBOUNCE = new CyCsElement(C_DEBOUNCE_DEF, 1, 255);
        public static CyCsElement C_HYSTERESIS = new CyCsElement(C_HYSTERESIS_DEF, 1, 255);
        public static CyCsElement C_FINGER_THRESHOLD = new CyCsElement(C_FINGER_THRESHOLD_DEF, 1, 250);
        public static CyCsElement C_NOISE_THRESHOLD = new CyCsElement(C_NOISE_THRESHOLD_DEF, 1, 255);
        public static CyCsElement C_WIDGET_COUNT = new CyCsElement(C_WIDGET_COUNT_DEF, 2, 32);
        public static CyCsElement C_WIDGET_RESOLUTION = new CyCsElement(C_WIDGET_RESOLUTION_DEF, 1, 255);

        public static CyCsElement C_RB_COUNT = new CyCsElement(C_RB_COUNT_DEF, 1, 3);
        public static CyCsElement C_IDAC_SETTINGS = new CyCsElement(C_IDAC_SETTINGS_DEF, 0, 255);
        public static CyCsElement C_SENSITIVITY = new CyCsElement(C_SENSITIVITY_DEF, 1, 4);
        public static CyCsElement C_ANALOG_SWITCH_DIVIDER = new CyCsElement(11, 1, 255);
        public static CyCsElement C_SCAN_CLOCK = new CyCsElement(12, 3, 24, CyElementType.Double);
        public static CyCsElement C_VREF_VALUE = new CyCsElement(64, 0, 255);

        public static double C_AUTO_TUNING_ALLOWED_CLOCK_VALUE = 24;


        public static bool HasComplexScanSlot(CySensorType type)
        {
            return (type == CySensorType.Generic) || (type == CySensorType.Proximity);
        }
        public static bool IsMainPartOfWidget(CySensorType type)
        {
            return GetBothParts(type)[0] == type;
        }
        public static List<CySensorType> GetBothParts(CySensorType type)
        {
            List<CySensorType> res = new List<CySensorType>();
            res.Add(type);
            if ((type == CySensorType.MatrixButtonsRow) || (type == CySensorType.MatrixButtonsColumn))
            {
                res.Clear();
                res.Add(CySensorType.MatrixButtonsColumn);
                res.Add(CySensorType.MatrixButtonsRow);
            }
            if ((type == CySensorType.TouchpadColumn) || (type == CySensorType.TouchpadRow))
            {
                res.Clear();
                res.Add(CySensorType.TouchpadColumn);
                res.Add(CySensorType.TouchpadRow);
            }
            return res;
        }
        public static bool IsCentroid(CySensorType type)
        {
            return (type == CySensorType.SliderLinear) || (type == CySensorType.SliderRadial) ||
                (type == CySensorType.TouchpadColumn) || (type == CySensorType.TouchpadRow);                
        }
        public static int GetResolutionBitsValue(CyScanResolutionType res)
        {
            switch (res)
            {
                case CyScanResolutionType._8:
                    return 8;                    
                case CyScanResolutionType._9:
                    return 9;
                case CyScanResolutionType._10:
                    return 10;
                case CyScanResolutionType._11:
                    return 11;
                case CyScanResolutionType._12:
                    return 12;
                case CyScanResolutionType._13:
                    return 13;
                case CyScanResolutionType._14:
                    return 14;
                case CyScanResolutionType._15:
                    return 15;
                case CyScanResolutionType._16:
                    return 16;
                default:
                    return 8;
            }
        }
    }

    #endregion
}
