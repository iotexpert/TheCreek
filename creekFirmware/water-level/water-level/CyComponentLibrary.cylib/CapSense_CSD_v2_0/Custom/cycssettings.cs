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
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;

namespace CapSense_CSD_v2_0
{
    #region CySettings
    [Serializable()]
    public class CyCSSettings
    {
        public const string P_NUMBER_OF_CHANNELS = "NumberOfChannels";
        public const string P_CLK_FR = "IntClockFrequency";
        public const string P_CLOCK_SOURCE = "ClockSource";
        public const string P_PRESCALER_OPTIONS = "PrescalerOptions";
        private const string P_CURRENT_SOURCE = "CurrentSource";
        private const string P_PRS_OPTIONS = "PrsOptions";
        private const string P_RB_NUMBER = "RbNumber";
        private const string P_SHIELD_ENABLE = "ShieldEnable";
        private const string P_GUARD_SENSOR_ENABLE = "GuardSensorEnable";
        private const string P_SCAN_SPEED = "ScanSpeed";
        private const string P_SENSOR_AUTO_RESET = "SensorAutoReset";
        private const string P_WIDGET_RESOLUTION = "WidgetResolution";
        private const string P_IDAC_RANGE = "IdacRange";
        private const string P_CONNECT_INACTIVE_SENSORS = "ConnectInactiveSensors";
        private const string P_ANALOG_SWITCH_DIVIDER = "AnalogSwitchDivider";
        private const string P_RAW_DATA_FILTER_TYPE = "RawDataFilterType";
        private const string P_WATER_PROOFING_ENABLED = "WaterProofingEnabled";
        public const string P_TUNING_METHOD = "TuningMethod";
        public const string P_IMPLEMENTATION = "Implementation";
        private const string P_ENABLE_TUNE_HELPER = "EnableTuneHelper";
        private const string P_EZI2C_INSTANCE_NAME = "EzI2CInstanceName";

        private const string P_VREF_OPTIONS = "VrefOptions";
        private const string P_VREF_VALUE = "VrefValue";

        //Parameter values        
        public const string P_ALIAS_RB_VAL = "Rb";
        private const string P_ALIAS_CMOD_VAL = "Cmod";

        [XmlElement("Configuration")]
        public CyChannelConfig m_configuration;

        [XmlElement("Prs")]
        public CyPrsOptions m_prs = CyPrsOptions.Prs_16bits;

        [XmlElement("CurrentSource")]
        public CyCurrentSourceOptions m_currentSource = CyCurrentSourceOptions.Idac_Source;

        [XmlElement("Prescaler")]
        public CyPrescalerOptions m_prescaler = CyPrescalerOptions.Prescaler_FF;

        [XmlElement("CurrectClockValue")]
        public double m_clockFr = CyCsConst.C_SCAN_CLOCK.m_value;

        [XmlElement("ExternalClockEnabled")]
        public CyClockSourceOptions m_clockType = CyClockSourceOptions.External;

        [XmlArray("ListChannels")]
        [XmlArrayItem("CyChannelProperties")]
        public List<CyChannelProperties> m_listChannels;

        [XmlElement("ShieldEnable")]
        public bool m_shieldEnable = false;

        [XmlElement("VrefOption")]
        public CyVrefOptions m_vrefOption = CyVrefOptions.Ref_Vref_1_24;

        [XmlElement("VrefVdacValue")]
        public byte m_vrefVdacValue = (byte)CyCsConst.C_VREF_VALUE.m_value;

        [XmlElement("AnalogSwitchDivider")]
        public byte m_analogSwitchDivider = (byte)CyCsConst.C_ANALOG_SWITCH_DIVIDER.m_value;

        [XmlElement("GuardSensorEnable")]
        public bool m_guardSensorEnable = false;

        [XmlElement("WidgetResolution")]
        public byte m_widgetResolution = 8;

        [XmlElement("SensorAutoReset")]
        public bool m_sensorAutoReset = false;

        [XmlElement("ConnectInactiveSensors")]
        public CyConnectInactiveSensorsOptions m_connectInactiveSensors= CyConnectInactiveSensorsOptions.Ground;

        [XmlElement("IdacRange")]
        public CyIdacRangeOptions m_idacRange = CyIdacRangeOptions.fs_255uA;

        [XmlElement("RawDataFilterType")]
        public CyRawDataFilterOptions m_rawDataFilterType = CyRawDataFilterOptions.FirstOrderIIR0_75;

        [XmlElement("ScanSpeed")]
        public CyScanSpeedOptions m_scanSpeed = CyScanSpeedOptions.Normal;

        [XmlElement("TuningMethod")]
        public CyTuningMethodOptions m_tuningMethod = CyTuningMethodOptions.Tuning_Auto;

        [XmlElement("EnableTuneHelper")]
        public bool m_enableTuneHelper = true;

        [XmlElement("WaterProofing")]
        public bool m_waterProofing = false;

        [XmlElement("EzI2CInstanceName")]
        public string m_ezI2CInstanceName = "EzI2C";


        [XmlIgnore()]
        public EventHandler m_configurationChanged;

        [XmlIgnore()]
        [Browsable(false)]
        public CyChannelConfig Configuration
        {
            get { return m_configuration; }
            set
            {
                if (m_configuration != value)
                {
                    m_configuration = value;
                    ConfigurationChanged();
                }
            }
        }
        [XmlIgnore()]
        [Browsable(false)]
        public CyTuningMethodOptions TuningMethod
        {
            get { return m_tuningMethod; }
            set
            {
                if (m_tuningMethod != value)
                {
                    m_tuningMethod = value;
                    ConfigurationChanged();
                }
            }
        }
        [XmlIgnore()]
        public CyIdacRangeOptions IdacRange
        {
            get { return m_idacRange; }
            set { m_idacRange = value; }
        }

        [XmlIgnore()]
        public byte AnalogSwitchDivider
        {
            get { return m_analogSwitchDivider; }
            set { m_analogSwitchDivider = value; }
        }

        [XmlIgnore()]
        public CyScanSpeedOptions ScanSpeed
        {
            get { return m_scanSpeed; }
            set { m_scanSpeed = value; }
        }

        public void ConfigurationChanged()
        {
            if (m_configurationChanged != null && m_configurationChanged.GetInvocationList().Length > 0)
                m_configurationChanged(null, null);
        }

        public CyCSSettings()
        {
        }

        public void FirstInitialszation()
        {
            m_listChannels = new List<CyChannelProperties>();
            m_listChannels.Add(new CyChannelProperties(CyChannelNumber.First));
            m_listChannels.Add(new CyChannelProperties(CyChannelNumber.Second));
        }

        public void GetParams(CyCSInstParameters inst, CyCompDevParam param)
        {
            object res;
            CyCustErr err;
            if (inst != null)
            {
                //Get NumberOfChannels
                if (ParameterChanged(param, P_NUMBER_OF_CHANNELS))
                {
                    byte ch;
                    if (byte.TryParse(inst.GetCommittedParam(P_NUMBER_OF_CHANNELS).Value, out ch))
                    {
                        m_configuration = ch == 2 ? CyChannelConfig.TWO_CHANNELS : CyChannelConfig.ONE_CHANNEL;
                    }
                    else Debug.Assert(false);
                }
                //CLK_FR
                if (ParameterChanged(param, P_CLK_FR))
                {
                    double cl;
                    if (double.TryParse(inst.GetCommittedParam(P_CLK_FR).Value, out cl))
                        m_clockFr = cl;
                    else Debug.Assert(false);
                }
                //Get ClockSource
                if (ParameterChanged(param, P_CLOCK_SOURCE))
                {
                    try
                    {
                        int val = int.Parse(inst.GetCommittedParam(P_CLOCK_SOURCE).Value);
                        m_clockType = (CyClockSourceOptions)val;
                    }
                    catch (Exception ex)
                    {
                        err = new CyCustErr(ex);
                        Debug.Assert(err.IsOK);
                    }
                }

                //Get WATER_PROOFING_ENABLED
                if (ParameterChanged(param, P_WATER_PROOFING_ENABLED))
                {
                    bool wt;
                    if (bool.TryParse(inst.GetCommittedParam(P_WATER_PROOFING_ENABLED).Value, out wt))
                        m_waterProofing = wt;
                    else Debug.Assert(false);
                }

                //Get GUARD_SENSOR_ENABLE
                if (ParameterChanged(param, P_GUARD_SENSOR_ENABLE))
                {
                    bool wt;
                    if (bool.TryParse(inst.GetCommittedParam(P_GUARD_SENSOR_ENABLE).Value, out wt))
                        m_guardSensorEnable = wt;
                    else Debug.Assert(false);
                }
                //Get AnalogSwitchDivider
                if (ParameterChanged(param, P_ANALOG_SWITCH_DIVIDER))
                {
                    byte wt;
                    if (byte.TryParse(inst.GetCommittedParam(P_ANALOG_SWITCH_DIVIDER).Value, out wt))
                        m_analogSwitchDivider = wt;
                    else Debug.Assert(false);
                }
                //Get WidgetResolution
                if (ParameterChanged(param, P_WIDGET_RESOLUTION))
                {
                    byte wt;
                    if (byte.TryParse(inst.GetCommittedParam(P_WIDGET_RESOLUTION).Value, out wt))
                        m_widgetResolution = wt;
                    else Debug.Assert(false);
                }
                //Get SensorAutoReset
                if (ParameterChanged(param, P_SENSOR_AUTO_RESET))
                {
                    bool wt;
                    if (bool.TryParse(inst.GetCommittedParam(P_SENSOR_AUTO_RESET).Value, out wt))
                        m_sensorAutoReset = wt;
                    else Debug.Assert(false);
                }
                //Get ShieldEnable
                if (ParameterChanged(param, P_SHIELD_ENABLE))
                {
                    bool wt;
                    if (bool.TryParse(inst.GetCommittedParam(P_SHIELD_ENABLE).Value, out wt))
                        m_shieldEnable = wt;
                    else Debug.Assert(false);
                }

                //Get EnableTuneHelper
                if (ParameterChanged(param, P_ENABLE_TUNE_HELPER))
                {
                    bool wt;
                    if (bool.TryParse(inst.GetCommittedParam(P_ENABLE_TUNE_HELPER).Value, out wt))
                        m_enableTuneHelper = wt;
                    else Debug.Assert(false);
                }

                //Get TUNING_METHOD
                if (ParameterChanged(param, P_TUNING_METHOD))
                {
                    string val = inst.GetCommittedParam(P_TUNING_METHOD).Value;
                    if (ParseEnum(val, typeof(CyTuningMethodOptions), out res))
                        m_tuningMethod = (CyTuningMethodOptions)res;
                }

                //Get ConnectInactiveSensors
                if (ParameterChanged(param, P_CONNECT_INACTIVE_SENSORS))
                {
                    string val = inst.GetCommittedParam(P_CONNECT_INACTIVE_SENSORS).Value;
                    if (ParseEnum(val, typeof(CyConnectInactiveSensorsOptions), out res))
                        m_connectInactiveSensors = (CyConnectInactiveSensorsOptions)res;
                }
                //Get IdacRange
                if (ParameterChanged(param, P_IDAC_RANGE))
                {
                    string val = inst.GetCommittedParam(P_IDAC_RANGE).Value;
                    if (ParseEnum(val, typeof(CyIdacRangeOptions), out res))
                        m_idacRange = (CyIdacRangeOptions)res;
                }
                //Get ScanSpeed
                if (ParameterChanged(param, P_SCAN_SPEED))
                {
                    string val = inst.GetCommittedParam(P_SCAN_SPEED).Value;
                    if (ParseEnum(val, typeof(CyScanSpeedOptions), out res))
                        m_scanSpeed = (CyScanSpeedOptions)res;
                }
                //Get RawDataFilterType
                if (ParameterChanged(param, P_RAW_DATA_FILTER_TYPE))
                {
                    string val = inst.GetCommittedParam(P_RAW_DATA_FILTER_TYPE).Value;
                    if (ParseEnum(val, typeof(CyRawDataFilterOptions), out res))
                        m_rawDataFilterType = (CyRawDataFilterOptions)res;
                }
                //Get PRSResolution
                if (ParameterChanged(param, P_PRS_OPTIONS))
                {
                    string val = inst.GetCommittedParam(P_PRS_OPTIONS).Value;
                    if (ParseEnum(val, typeof(CyPrsOptions), out res))
                        m_prs = (CyPrsOptions)res;
                }
                //Get Prescaler
                if (ParameterChanged(param, P_PRESCALER_OPTIONS))
                {
                    string val = inst.GetCommittedParam(P_PRESCALER_OPTIONS).Value;
                    if (ParseEnum(val, typeof(CyPrescalerOptions), out res))
                        m_prescaler = (CyPrescalerOptions)res;
                }
                //Get IdacOptions
                if (ParameterChanged(param, P_CURRENT_SOURCE))
                {
                    string val = inst.GetCommittedParam(P_CURRENT_SOURCE).Value;
                    if (ParseEnum(val, typeof(CyCurrentSourceOptions), out res))
                        m_currentSource = ((CyCurrentSourceOptions)res);
                }
                //Get VrefValue
                if (ParameterChanged(param, P_VREF_VALUE))
                {
                    byte wt;
                    if (byte.TryParse(inst.GetCommittedParam(P_VREF_VALUE).Value, out wt))
                        m_vrefVdacValue = wt;
                    else Debug.Assert(false);
                }

                //Get VrefOptions
                if (ParameterChanged(param, P_VREF_OPTIONS))
                {
                    string val = inst.GetCommittedParam(P_VREF_OPTIONS).Value;
                    if (ParseEnum(val, typeof(CyVrefOptions), out res))
                        m_vrefOption = (CyVrefOptions)res;
                }
                //Get VrefOptions
                if (ParameterChanged(param, P_EZI2C_INSTANCE_NAME))
                {
                    m_ezI2CInstanceName = inst.GetCommittedParam(P_EZI2C_INSTANCE_NAME).Value;
                }

                foreach (CyChannelProperties item in m_listChannels)
                {
                    string paramName;
                    //Get Rb
                    if (m_currentSource == CyCurrentSourceOptions.Idac_None)
                    {
                        paramName = P_RB_NUMBER + item.GetSufix();
                        if (ParameterChanged(param, paramName))
                        {
                            byte rb;
                            if (byte.TryParse(inst.GetCommittedParam(paramName).Value, out rb))
                                if (CyCsConst.C_RB_COUNT.CheckRange(rb))
                                    item.m_rb = rb;                              
                        }
                    }
                    //Get Implementation
                    paramName = P_IMPLEMENTATION + item.GetSufix();
                    if (ParameterChanged(param, paramName))
                    {
                        string val = inst.GetCommittedParam(paramName).Value;
                        if (ParseEnum(val, typeof(CyMeasureImplemetation), out res))
                            item.m_implementation = (CyMeasureImplemetation)res;
                    }
                }
            }
        }

        public void SetParams(CyCSInstParameters inst)
        {
            if (inst != null)
            {
                //Set NUMBER_OF_CHANNELS
                inst.SetParamExpr(P_NUMBER_OF_CHANNELS, ((int)m_configuration + 1).ToString());
                //Set PRSResolution
                inst.SetParamExpr(P_PRS_OPTIONS, m_prs.ToString());
                //Set Clk
                inst.SetParamExpr(P_CLK_FR, m_clockFr.ToString());
                inst.SetParamExpr(P_CLOCK_SOURCE, m_clockType.ToString());

                //Set ShieldEnable
                inst.SetParamExpr(P_SHIELD_ENABLE, m_shieldEnable.ToString());

                //Set WATER_PROOFING_ENABLED
                inst.SetParamExpr(P_WATER_PROOFING_ENABLED, m_waterProofing.ToString());

                //Set TUNING_METHOD
                inst.SetParamExpr(P_TUNING_METHOD, m_tuningMethod.ToString());

                //Set Prescaler
                inst.SetParamExpr(P_PRESCALER_OPTIONS, m_prescaler.ToString());

                //Set IdacOptions
                inst.SetParamExpr(P_CURRENT_SOURCE, m_currentSource.ToString());

                //Set VrefValue
                inst.SetParamExpr(P_VREF_VALUE, m_vrefVdacValue.ToString());

                //Set VrefOptions
                inst.SetParamExpr(P_VREF_OPTIONS, m_vrefOption.ToString());

                //Set ConnectInactiveSensors
                inst.SetParamExpr(P_CONNECT_INACTIVE_SENSORS, m_connectInactiveSensors.ToString());

                //Set IdacRange 
                inst.SetParamExpr(P_IDAC_RANGE, m_idacRange.ToString());

                //Set RawDataFilterType
                inst.SetParamExpr(P_RAW_DATA_FILTER_TYPE, m_rawDataFilterType.ToString());

                //Set ScanSpeed
                inst.SetParamExpr(P_SCAN_SPEED, m_scanSpeed.ToString());

                //Set SensorAutoReset
                inst.SetParamExpr(P_SENSOR_AUTO_RESET, m_sensorAutoReset.ToString());

                //Set WidgetResolution
                inst.SetParamExpr(P_WIDGET_RESOLUTION, m_widgetResolution.ToString());

                //Set GuardSensorEnable
                inst.SetParamExpr(P_GUARD_SENSOR_ENABLE, m_guardSensorEnable.ToString());

                //Set AnalogSwitchDivider
                inst.SetParamExpr(P_ANALOG_SWITCH_DIVIDER, m_analogSwitchDivider.ToString());

                //Set EnableTuneHelper
                inst.SetParamExpr(P_ENABLE_TUNE_HELPER, m_enableTuneHelper.ToString());

                //Set EzI2CInstanceName
                inst.SetParamExpr(P_EZI2C_INSTANCE_NAME, m_ezI2CInstanceName);

                for (int k = 0; k < m_listChannels.Count; k++)
                {
                    CyChannelProperties part = m_listChannels[k];
                    //Set Rb
                    if (m_currentSource == CyCurrentSourceOptions.Idac_None)
                        inst.SetParamExpr(P_RB_NUMBER + part.GetSufix(), part.m_rb.ToString());
                    else
                        inst.SetParamExpr(P_RB_NUMBER + part.GetSufix(), "0");
                    //Set Implementation
                    inst.SetParamExpr(P_IMPLEMENTATION + part.GetSufix(), part.m_implementation.ToString());
                }
            }
        }
        public static bool ParameterChanged(CyCompDevParam param, string param_name)
        {
            if (param != null && param.ErrorCount == 0)
                if (param.Name != param_name) return false;
            return true;
        }
        public static bool ParseEnum(string value, Type _type, out object res)
        {
            try
            {
                res = Enum.Parse(_type, value);
                return true;
            }
            catch
            {
                res = null;
                return false;
            }
        }

        public bool IsIdacInSystem()
        {
            if (m_currentSource != CyCurrentSourceOptions.Idac_None) return true;
            return false;
        }
        public int GetChannelNumber(CyChannelNumber ch)
        {
            return Configuration == CyChannelConfig.ONE_CHANNEL ? 0 : (int)ch;
        }
    }
    #endregion

    #region CyChannelProperties
    [Serializable()]
    public class CyChannelProperties
    {
        [XmlAttribute("Channel")]
        public CyChannelNumber m_channel = CyChannelNumber.First;

        [XmlElement("RbCount")]
        public int m_rb = CyCsConst.C_RB_COUNT_DEF;

        [XmlElement("Implementation")]
        public CyMeasureImplemetation m_implementation = CyMeasureImplemetation.FF_Based;

        public CyChannelProperties()
        {
        }

        public CyChannelProperties(CyChannelNumber ch)
            : this()
        {
            m_channel = ch;
        }
        public int GetChannelNumber()
        {
            return (int)m_channel;
        }
        public string GetChannelSufixAPI()
        {
            return GetSufix() == string.Empty ? string.Empty : "_" + GetSufix();
        }
        public string GetSufix()
        {
            return GetSufix(m_channel);
        }
        public static string GetSufix(CyChannelNumber channel)
        {
            if (channel == CyChannelNumber.Second) return "_CH1";
            if (channel == CyChannelNumber.First) return "_CH0";
            return "";
        }

        public string GetAliasRbByIndex(int i)
        {
            return CyCSSettings.P_ALIAS_RB_VAL + i + GetSufix();
        }
    }
    #endregion
}
