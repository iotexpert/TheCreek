/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace ADC_SAR_v1_71
{
    #region Symbol Parameter Names
    public class CyParamNames
    {
        // Formal parameters
        public const string ADC_RESOLUTION = "ADC_Resolution";
        public const string ADC_CLOCK = "ADC_Clock";
        public const string ADC_SAMPLE_MODE = "ADC_SampleMode";
        public const string ADC_INPUT_RANGE = "ADC_Input_Range";
        public const string ADC_REFERENCE = "ADC_Reference";
        public const string SAMPLE_RATE = "Sample_Rate";
        public const string REF_VOLTAGE = "Ref_Voltage";
        public const string SAMPLE_PRECHARGE = "Sample_Precharge";

        // Local parameters
        public const string ADC_CLOCK_FREQUENCY = "ADC_Clock_Frequency";
    }
    #endregion

    #region Symbol Types
    public enum CyEAdcSampleModeType { FreeRunning, Triggered };
    public enum CyEAdcClockSrcType { External, Internal };
    public enum CyEAdcResolution { Bits_8 = 8, Bits_10 = 10, Bits_12 = 12 };
    public enum CyEAdcInputRangeType
    {
        Vss_to_Vref,
        Vssa_to_Vdda,
        Vssa_to_VDAC,
        Vneg_Vref_Diff,
        Vneg_Vdda_Diff,
        Vneg_Vdda_2_Diff,
        Vneg_VDAC_Diff
    };
    public enum CyEAdcRefType { Int_Ref_Not_Bypassed, Int_Ref_Bypass, Ext_Ref };
    public enum CyEDeviceFamilyType { PSoC3, PSoC5 };
    #endregion

    #region Parameter Ranges
    public class CyParamRange
    {
        public const uint SAMPLE_RATE_MIN = 0;

        public const uint CLOCK_FREQ_MIN_MHZ = 1;
        public const uint CLOCK_FREQ_MAX_MHZ = 14;
        public const uint CLOCK_FREQ_MIN_HZ = CLOCK_FREQ_MIN_MHZ * 1000000;
        public const uint CLOCK_FREQ_MAX_HZ = CLOCK_FREQ_MAX_MHZ * 1000000;
        public const uint CLOCK_FREQ_MIN_KHZ = CLOCK_FREQ_MIN_MHZ * 1000;
        public const uint CLOCK_FREQ_MAX_KHZ = CLOCK_FREQ_MAX_MHZ * 1000;

        public const double REF_VOLTAGE_MIN = 0.15;
        public const double REF_VOLTAGE_MAX = 5.5;

        public const uint CONVERSION_RATE_INT_VREF_MAX = 100000;
        public const uint DEFAULT_SAMPLE_RATE = 631579;
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
        public double m_externalClock;
        public double m_systemVdda;

        // Lists contain display names of types taken from symbol,
        // lists are used to fill comboboxes
        public List<string> m_resolutionList;
        public List<string> m_inputRangeList;
        public List<string> m_referenceList;
        // Dictionary is used to keep display names of types taken from symbol,
        // and associate them with the enum fields to simplify access to diaplay names
        public Dictionary<object, string> m_dnDict = new Dictionary<object, string>();

        #region Constructor(s)
        public CySARADCParameters(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery)
        {
            this.m_inst = inst;

            m_resolutionList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.ADC_RESOLUTION));
            m_inputRangeList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.ADC_INPUT_RANGE));
            m_referenceList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.ADC_REFERENCE));
            m_externalClock = CyClockReader.GetExternalClockInKHz(termQuery, CyClockReader.EXTERNAL_CLK);

            try
            {
                m_systemVdda = System.Math.Round(inst.VoltageQuery.VDDA, 4);
            }
            catch
            {
                m_systemVdda = 5;
            }

            // Adding display names to the dictionary to easily operate with enums
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyEAdcInputRangeType), m_inputRangeList);
            CyDictParser.FillDictionary(ref m_dnDict, typeof(CyEAdcRefType), m_referenceList);
        }
        #endregion

        #region Class properties
        public CyEAdcResolution AdcResolution
        {
            get
            {
                return GetValue<CyEAdcResolution>(CyParamNames.ADC_RESOLUTION);
            }
            set
            {
                SetValue(CyParamNames.ADC_RESOLUTION, value);
            }
        }

        public CyEAdcClockSrcType AdcClockSource
        {
            get
            {
                return GetValue<CyEAdcClockSrcType>(CyParamNames.ADC_CLOCK);
            }
            set
            {
                SetValue(CyParamNames.ADC_CLOCK, value);
            }
        }

        public CyEAdcSampleModeType AdcSampleMode
        {
            get
            {
                return GetValue<CyEAdcSampleModeType>(CyParamNames.ADC_SAMPLE_MODE);
            }
            set
            {
                SetValue(CyParamNames.ADC_SAMPLE_MODE, value);
            }
        }

        public CyEAdcInputRangeType AdcInputRange
        {
            get
            {
                return GetValue<CyEAdcInputRangeType>(CyParamNames.ADC_INPUT_RANGE);
            }
            set
            {
                SetValue(CyParamNames.ADC_INPUT_RANGE, value);
            }
        }

        public CyEAdcRefType AdcReference
        {
            get
            {
                return GetValue<CyEAdcRefType>(CyParamNames.ADC_REFERENCE);
            }
            set
            {
                SetValue(CyParamNames.ADC_REFERENCE, value);
            }
        }

        public uint AdcSampleRate
        {
            get
            {
                return GetValue<uint>(CyParamNames.SAMPLE_RATE);
            }
            set
            {
                SetValue(CyParamNames.SAMPLE_RATE, value);
            }
        }

        public double RefVoltage
        {
            get
            {
                return GetValue<double>(CyParamNames.REF_VOLTAGE);
            }
            set
            {
                SetValue(CyParamNames.REF_VOLTAGE, value);
            }
        }

        public int SamplePrecharge
        {
            get
            {
                return GetValue<int>(CyParamNames.SAMPLE_PRECHARGE);
            }
        }
        #endregion

        #region Getting Parameters
        private T GetValue<T>(string paramName)
        {
            T value;
            CyCustErr err = m_inst.GetCommittedParam(paramName).TryGetValueAs<T>(out value);
            if (err.IsOK)
            {
                return value;
            }
            else
            {
                m_control.ShowError(paramName, err);
                return default(T);
            }
        }

        public void LoadParameters()
        {
            m_globalEditMode = false;
            m_control.UpdateUI();
            m_globalEditMode = true;
        }
        #endregion

        #region Setting Parameters
        private void SetValue<T>(string paramName, T value)
        {
            if (m_globalEditMode)
            {
                CyCustErr err = SetParam<T>(paramName, value, false);
                m_inst.CommitParamExprs();
                m_control.ShowError(paramName, err);
            }
        }

        private CyCustErr SetParam<T>(string paramName, T value, bool toLowerCase)
        {
            CyCustErr err = new CyCustErr("Error Setting Parameter.");
            if (m_inst.SetParamExpr(paramName, value.ToString()))
            {
                err = CyCustErr.OK;
            }
            return err;
        }
        #endregion

        #region External Clock Updating
        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            m_externalClock = CyClockReader.GetExternalClockInKHz(termQuery, CyClockReader.EXTERNAL_CLK);
            m_control.UpdateClockData();
            m_control.UpdateSampleRateData();
        }
        #endregion
    }
}
