/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CyDesigner.Extensions.Gde;
using System.ComponentModel;

namespace ADC_SAR_v1_50
{
    #region Symbol Parameter Names
    public class CyParamNames
    {
        public const string ADC_POWER = "ADC_Power";
        public const string ADC_RESOLUTION = "ADC_Resolution";
        public const string ADC_CLOCK = "ADC_Clock";
        public const string ADC_CLOCK_FREQUENCY = "ADC_Clock_Frequency";
        public const string ADC_SAMPLE_MODE = "ADC_SampleMode";
        public const string ADC_INPUT_RANGE = "ADC_Input_Range";
        public const string ADC_REFERENCE = "ADC_Reference";
        public const string SAMPLE_RATE = "Sample_Rate";
        public const string REF_VOLTAGE = "Ref_Voltage";
    }
    #endregion

    #region Symbol Types
    public enum E_ADC_POWER_TYPE
    {
        [Description("High Power")]
        HighPower = 0,
        [Description("Medium Power")]
        MedPower = 1,
        [Description("Low Power")]
        LowPower = 2,
        [Description("Minimum Power")]
        MinPower = 3
    };
    public enum E_ADC_SAMPLE_MODE_TYPE { FreeRunning, Triggered };
    public enum E_ADC_CLOCK_SRC_TYPE { External, Internal };
    public enum E_ADC_INPUT_RANGE_TYPE
    {
        [Description("0.0 to 2.048V (Single Ended) 0 to Vref*2")]
        Vss_to_Vref = 0,
        [Description("Vssa to Vdda (Single Ended)")]
        Vssa_to_Vdda = 1,
        [Description("Vssa to VDAC*2 (Single Ended)")]
        Vssa_to_VDAC = 2,
        [Description("0.0 +/- 1.024V (Differential)  -Input +/- Vref")]
        Vneg_Vref_Diff = 3,
        [Description("0.0 +/- Vdda (Differential)  -Input +/- Vdda")]
        Vneg_Vdda_Diff = 4,
        [Description("0.0 +/- Vdda/2 (Differential)  -Input +/- Vdda/2")]
        Vneg_Vdda_2_Diff = 5,
        [Description("0.0 +/- VDAC (Differential)  -Input +/- VDAC")]
        Vneg_VDAC_Diff = 6
    };
    public enum E_ADC_REF_TYPE
    {
        [Description("Internal Vref")]
        Int_Ref_Not_Bypassed = 0,
        [Description("Internal Vref, bypassed")]
        Int_Ref_Bypass = 1,
        [Description("External Vref")]
        Ext_Ref = 2
    };
    #endregion

    #region Parameter Ranges
    public class CyParamRange
    {
        public const uint SAMPLE_RATE_MIN = 0;
        public const uint CLOCK_FREQ_MIN = 1000000;
        public const float REF_VOLTAGE_MIN = 0;
        public const float REF_VOLTAGE_MAX = 5.5f;
    }
    #endregion

    public class CySARADCParameters
    {
        public ICyInstEdit_v1 m_inst;
        /// <summary>
        /// During first getting of parameters this variable is false, what means that assigning
        /// values to form controls will not immediatly overwrite parameters with the same values.
        /// </summary>
        public bool m_globalEditMode = false;
        public CySARADCControl m_control = null;
        public float m_clock;

        public List<string> m_resolutionList;
        public List<string> m_powerList;
        public List<string> m_inputRangeList;
        public List<string> m_referenceList;

        #region Component Parameters
        public E_ADC_POWER_TYPE m_adcPower;
        public UInt16 m_adcResolution;
        public E_ADC_CLOCK_SRC_TYPE m_adcClockSource;
        public E_ADC_SAMPLE_MODE_TYPE m_adcSampleMode;
        public float m_adcClockFrequency;
        public E_ADC_INPUT_RANGE_TYPE m_adcInputRange;
        public E_ADC_REF_TYPE m_adcReference;
        public uint m_adcSampleRate;
        public float m_refVoltage;
        #endregion

        #region Constructor(s)
        public CySARADCParameters(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery)
        {
            this.m_inst = inst;
            m_resolutionList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.ADC_RESOLUTION));
            m_powerList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.ADC_POWER));
            m_inputRangeList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.ADC_INPUT_RANGE));
            m_referenceList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.ADC_REFERENCE));
            m_clock = GetComponentClockKHz(termQuery);
        }
        #endregion

        #region Getting Parameters
        public void GetParams(ICyInstEdit_v1 inst, CyCompDevParam param)
        {
            m_globalEditMode = false;
            if (param != null)
            {
                if (param.ErrorCount == 0)
                {
                    switch (param.Name)
                    {
                        case CyParamNames.ADC_POWER:
                            GetADCPower();
                            break;
                        case CyParamNames.ADC_RESOLUTION:
                            GetADCResolution();
                            break;
                        case CyParamNames.ADC_CLOCK:
                            GetADCClockSource();
                            break;
                        case CyParamNames.ADC_SAMPLE_MODE:
                            GetADCSampleMode();
                            break;
                        case CyParamNames.ADC_INPUT_RANGE:
                            GetADCInputRange();
                            break;
                        case CyParamNames.ADC_REFERENCE:
                            GetADCReference();
                            break;
                        case CyParamNames.REF_VOLTAGE:
                            GetRefVoltage();
                            break;
                        default:
                            GetFrequencyAndSampleRate();
                            break;
                    }
                }
            }
            else
            {
                GetADCPower();
                GetADCResolution();
                GetADCClockSource();
                GetADCSampleMode();
                GetADCInputRange();
                GetADCReference();
                GetRefVoltage();
                GetFrequencyAndSampleRate();
            }
            m_control.UpdateFormFromParams();
            m_globalEditMode = true;
        }

        private void GetADCPower()
        {
            try
            {
                m_adcPower = (E_ADC_POWER_TYPE)byte.Parse(m_inst.GetCommittedParam(CyParamNames.ADC_POWER).Value);
            }
            catch (Exception) { }
        }

        private void GetADCResolution()
        {
            UInt16.TryParse(m_inst.GetCommittedParam(CyParamNames.ADC_RESOLUTION).Value, out m_adcResolution);
        }

        private void GetADCClockSource()
        {
            try
            {
                m_adcClockSource = (E_ADC_CLOCK_SRC_TYPE)byte.Parse(m_inst.GetCommittedParam(
                    CyParamNames.ADC_CLOCK).Value);
            }
            catch (Exception) { }
        }

        private void GetADCSampleMode()
        {
            try
            {
                m_adcSampleMode = (E_ADC_SAMPLE_MODE_TYPE)byte.Parse(m_inst.GetCommittedParam(
                    CyParamNames.ADC_SAMPLE_MODE).Value);
            }
            catch (Exception) { }
        }

        private void GetADCInputRange()
        {
            try
            {
                m_adcInputRange = (E_ADC_INPUT_RANGE_TYPE)byte.Parse(m_inst.GetCommittedParam(
                    CyParamNames.ADC_INPUT_RANGE).Value);
            }
            catch (Exception) { }
        }

        private void GetADCReference()
        {
            try
            {
                m_adcReference = (E_ADC_REF_TYPE)byte.Parse(m_inst.GetCommittedParam(CyParamNames.ADC_REFERENCE).Value);
            }
            catch (Exception) { }
        }

        private void GetRefVoltage()
        {
            float.TryParse(m_inst.GetCommittedParam(CyParamNames.REF_VOLTAGE).Value, out m_refVoltage);
        }

        public void GetFrequencyAndSampleRate()
        {
            if (m_adcClockSource == E_ADC_CLOCK_SRC_TYPE.External)
            {
                if (m_clock >= 0)
                    m_adcClockFrequency = m_clock;
                else
                {
                    if (float.TryParse(m_inst.GetCommittedParam(CyParamNames.ADC_CLOCK_FREQUENCY).Value,
                            out m_adcClockFrequency))
                    {
                        m_adcClockFrequency /= 1000;
                    }
                }
                // Setting new SampleRate value if necessary
                uint tmpSampleRate = 0;
                uint.TryParse(m_inst.GetCommittedParam(CyParamNames.SAMPLE_RATE).Value, out tmpSampleRate);
                m_adcSampleRate = m_control.GetExternalSampleRate(m_adcResolution, m_adcClockFrequency);
                if (tmpSampleRate != m_adcSampleRate)
                    SetParameter(CyParamNames.SAMPLE_RATE);

                // Setting new ClockFrequency value if necessary
                float tmpFreq = 5;
                float.TryParse(m_inst.GetCommittedParam(CyParamNames.ADC_REFERENCE).Value, out tmpFreq);
                if (tmpFreq != m_adcClockFrequency)
                    SetParameter(CyParamNames.ADC_CLOCK_FREQUENCY);
            }
            else
            {
                uint.TryParse(m_inst.GetCommittedParam(CyParamNames.SAMPLE_RATE).Value, out m_adcSampleRate);
                m_adcClockFrequency = m_control.GetInternalFreq(m_adcResolution, m_adcSampleRate);
            }
        }
        #endregion

        #region Setting Parameters
        public void SetParameter(string parameter)
        {
            if ((m_inst != null && m_globalEditMode) || (parameter == CyParamNames.SAMPLE_RATE))
            {
                string value = string.Empty;
                switch (parameter)
                {
                    case CyParamNames.ADC_CLOCK:
                        value = m_adcClockSource.ToString();
                        break;
                    case CyParamNames.ADC_CLOCK_FREQUENCY:
                        //if (m_adcClockFrequency > 0 || m_clock >= 0)
                        value = (m_adcClockFrequency * 1000).ToString();
                        break;
                    case CyParamNames.ADC_INPUT_RANGE:
                        value = m_adcInputRange.ToString();
                        break;
                    case CyParamNames.ADC_POWER:
                        value = m_adcPower.ToString();
                        break;
                    case CyParamNames.ADC_REFERENCE:
                        value = m_adcReference.ToString();
                        break;
                    case CyParamNames.ADC_RESOLUTION:
                        value = m_adcResolution.ToString();
                        break;
                    case CyParamNames.ADC_SAMPLE_MODE:
                        value = m_adcSampleMode.ToString();
                        break;
                    case CyParamNames.REF_VOLTAGE:
                        value = m_refVoltage.ToString();
                        break;
                    case CyParamNames.SAMPLE_RATE:
                        value = m_adcSampleRate.ToString();
                        break;
                    default:
                        break;
                }

                m_inst.SetParamExpr(parameter, value);
                m_inst.CommitParamExprs();
            }
        }
        #endregion

        #region External Clock Updating
        public float GetComponentClockKHz(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData("aclk", 0);
            if (clkdata[0].IsFrequencyKnown)
            {
                float infreq = (float)clkdata[0].Frequency;
                switch (clkdata[0].Unit)
                {
                    case CyClockUnit.Hz:
                        infreq /= 1000;
                        break;
                    case CyClockUnit.MHz:
                        infreq *= 1000;
                        break;
                    case CyClockUnit.GHz:
                        infreq *= 1000000;
                        break;
                    case CyClockUnit.THz:
                        infreq *= 1000000000;
                        break;
                    default:
                        break;
                }
                return infreq;
            }
            return (-1);
        }

        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            m_clock = GetComponentClockKHz(termQuery);
            m_control.UpdateClockLabel(m_clock);
            m_control.UpdateSampleRateLabel(m_clock);
        }
        #endregion
    }
}