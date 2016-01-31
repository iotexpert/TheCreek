//*******************************************************************************
// File Name: CyADC_DelSigParameters.cs
/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace ADC_DelSig_v2_10
{
    class ADC_DelSigParameters
    {
        public const uint MAX_CONFIGS = 4;

        #region Param Name Constants
        public const string ADC_CLOCK = "ADC_Clock";
        public const string INPUT_RANGE = "ADC_Input_Range";
        public const string INPUT_RANGE_CONFIG2 = "ADC_Input_Range_Config2";
        public const string INPUT_RANGE_CONFIG3 = "ADC_Input_Range_Config3";
        public const string INPUT_RANGE_CONFIG4 = "ADC_Input_Range_Config4";
        public const string ADC_REFERENCE = "ADC_Reference";
        public const string ADC_REFERENCE_CONFIG2 = "ADC_Reference_Config2";
        public const string ADC_REFERENCE_CONFIG3 = "ADC_Reference_Config3";
        public const string ADC_REFERENCE_CONFIG4 = "ADC_Reference_Config4";
        public const string RESOLUTION = "ADC_Resolution";
        public const string RESOLUTION_CONFIG2 = "ADC_Resolution_Config2";
        public const string RESOLUTION_CONFIG3 = "ADC_Resolution_Config3";
        public const string RESOLUTION_CONFIG4 = "ADC_Resolution_Config4";
        public const string CONVERSION_MODE = "Conversion_Mode";
        public const string CONVERSION_MODE_CONFIG2 = "Conversion_Mode_Config2";
        public const string CONVERSION_MODE_CONFIG3 = "Conversion_Mode_Config3";
        public const string CONVERSION_MODE_CONFIG4 = "Conversion_Mode_Config4";
        public const string INPUT_BUFFER_GAIN = "Input_Buffer_Gain";
        public const string INPUT_BUFFER_GAIN_CONFIG2 = "Input_Buffer_Gain_Config2";
        public const string INPUT_BUFFER_GAIN_CONFIG3 = "Input_Buffer_Gain_Config3";
        public const string INPUT_BUFFER_GAIN_CONFIG4 = "Input_Buffer_Gain_Config4";
        public const string INPUT_BUFFER_MODE = "Input_Buffer_Mode";
        public const string INPUT_BUFFER_MODE_CONFIG2 = "Input_Buffer_Mode_Config2";
        public const string INPUT_BUFFER_MODE_CONFIG3 = "Input_Buffer_Mode_Config3";
        public const string INPUT_BUFFER_MODE_CONFIG4 = "Input_Buffer_Mode_Config4";
        public const string SAMPLE_RATE = "Sample_Rate";
        public const string SAMPLE_RATE_CONFIG2 = "Sample_Rate_Config2";
        public const string SAMPLE_RATE_CONFIG3 = "Sample_Rate_Config3";
        public const string SAMPLE_RATE_CONFIG4 = "Sample_Rate_Config4";
        public const string REF_VOLTAGE = "Ref_Voltage";
        public const string REF_VOLTAGE_CONFIG2 = "Ref_Voltage_Config2";
        public const string REF_VOLTAGE_CONFIG3 = "Ref_Voltage_Config3";
        public const string REF_VOLTAGE_CONFIG4 = "Ref_Voltage_Config4";
        public const string CLOCK_FREQUENCY = "Clock_Frequency";
        public const string START_OF_CONVERSION = "Start_of_Conversion";
        public const string CONFIGS = "Configs";
        public const string ADC_CHARGE_PUMP_CLOCK = "ADC_Charge_Pump_Clock";
        public const string ADC_INPUT_MODE = "ADC_Input_Mode";
        public const string INPUT_MODE = "Input_Mode";
        public const string INPUT_MODE_CONFIG2 = "ADC_Input_Mode_Config2";
        public const string INPUT_MODE_CONFIG3 = "ADC_Input_Mode_Config3";
        public const string INPUT_MODE_CONFIG4 = "ADC_Input_Mode_Config4";
        public const string INPUT_RANGEVALUE = "ADC_Input_RangeValue";
        public const string INPUT_RANGEVALUE_CONFIG2 = "ADC_Input_RangeValue_Config2";
        public const string INPUT_RANGEVALUE_CONFIG3 = "ADC_Input_RangeValue_Config3";
        public const string INPUT_RANGEVALUE_CONFIG4 = "ADC_Input_RangeValue_Config4";
        public const string ADC_nVref = "Enable_Vref_Vss";
        public const string ADC_Vdda_Value = "Vdda_Value";

        public const string S_SOFTWARE = "Software";
        public const string S_INTERNAL = "Internal";
        public const string S_HARDWARE = "Hardware";
        public const string S_EXTERNAL = "External";
        
        

        #endregion


        /* Member variables. Note: Some of these can be made an array. */
        public CyCompDevParam ADC_Clock = null;
        public CyCompDevParam ADC_Input_Range = null;
        public CyCompDevParam ADC_Input_Range_Config2 = null;
        public CyCompDevParam ADC_Input_Range_Config3 = null;
        public CyCompDevParam ADC_Input_Range_Config4 = null;
        public CyCompDevParam ADC_Reference = null;
        public CyCompDevParam ADC_Reference_Config2 = null;
        public CyCompDevParam ADC_Reference_Config3 = null;
        public CyCompDevParam ADC_Reference_Config4 = null;
        public CyCompDevParam ADC_Resolution = null;
        public CyCompDevParam ADC_Resolution_Config2 = null;
        public CyCompDevParam ADC_Resolution_Config3 = null;
        public CyCompDevParam ADC_Resolution_Config4 = null;
        public CyCompDevParam Conversion_Mode = null;
        public CyCompDevParam Conversion_Mode_Config2 = null;
        public CyCompDevParam Conversion_Mode_Config3 = null;
        public CyCompDevParam Conversion_Mode_Config4 = null;
        public CyCompDevParam Input_Buffer_Gain = null;
        public CyCompDevParam Input_Buffer_Gain_Config2 = null;
        public CyCompDevParam Input_Buffer_Gain_Config3 = null;
        public CyCompDevParam Input_Buffer_Gain_Config4 = null;
        public CyCompDevParam Input_Buffer_Mode = null;
        public CyCompDevParam Input_Buffer_Mode_Config2 = null;
        public CyCompDevParam Input_Buffer_Mode_Config3 = null;
        public CyCompDevParam Input_Buffer_Mode_Config4 = null;
        public CyCompDevParam Sample_Rate = null;
        public CyCompDevParam Sample_Rate_Config2 = null;
        public CyCompDevParam Sample_Rate_Config3 = null;
        public CyCompDevParam Sample_Rate_Config4 = null;
        public CyCompDevParam Ref_Voltage = null;
        public CyCompDevParam Ref_Voltage_Config2 = null;
        public CyCompDevParam Ref_Voltage_Config3 = null;
        public CyCompDevParam Ref_Voltage_Config4 = null;
        public CyCompDevParam Clock_Frequency = null;
        public CyCompDevParam Start_of_Conversion = null;
        public CyCompDevParam Configs = null;
        public CyCompDevParam ADC_Charge_Pump_Clock = null;
        public CyCompDevParam ADC_Input_Mode = null;
        public CyCompDevParam Enable_Vref_Vss = null;
        
        public CyCompDevParam ADC_ExVref_Num = null;

        public CyCompDevParam Vdda_Value = null;

        public ADC_DelSigParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            ADC_Clock = inst.GetCommittedParam(ADC_CLOCK);

            ADC_Input_Range = inst.GetCommittedParam(INPUT_RANGE);
            ADC_Input_Range_Config2 = inst.GetCommittedParam(INPUT_RANGE_CONFIG2);
            ADC_Input_Range_Config3 = inst.GetCommittedParam(INPUT_RANGE_CONFIG3);
            ADC_Input_Range_Config4 = inst.GetCommittedParam(INPUT_RANGE_CONFIG4);

            ADC_Reference = inst.GetCommittedParam(ADC_REFERENCE);
            ADC_Reference_Config2 = inst.GetCommittedParam(ADC_REFERENCE_CONFIG2);
            ADC_Reference_Config3 = inst.GetCommittedParam(ADC_REFERENCE_CONFIG3);
            ADC_Reference_Config4 = inst.GetCommittedParam(ADC_REFERENCE_CONFIG4);

            ADC_Resolution = inst.GetCommittedParam(RESOLUTION);
            ADC_Resolution_Config2 = inst.GetCommittedParam(RESOLUTION_CONFIG2);
            ADC_Resolution_Config3 = inst.GetCommittedParam(RESOLUTION_CONFIG3);
            ADC_Resolution_Config4 = inst.GetCommittedParam(RESOLUTION_CONFIG4);

            Conversion_Mode = inst.GetCommittedParam(CONVERSION_MODE);
            Conversion_Mode_Config2 = inst.GetCommittedParam(CONVERSION_MODE_CONFIG2);
            Conversion_Mode_Config3 = inst.GetCommittedParam(CONVERSION_MODE_CONFIG3);
            Conversion_Mode_Config4 = inst.GetCommittedParam(CONVERSION_MODE_CONFIG4);
            Input_Buffer_Gain = inst.GetCommittedParam(INPUT_BUFFER_GAIN);
            Input_Buffer_Gain_Config2 = inst.GetCommittedParam(INPUT_BUFFER_GAIN_CONFIG2);
            Input_Buffer_Gain_Config3 = inst.GetCommittedParam(INPUT_BUFFER_GAIN_CONFIG3);
            Input_Buffer_Gain_Config4 = inst.GetCommittedParam(INPUT_BUFFER_GAIN_CONFIG4);
            Input_Buffer_Mode = inst.GetCommittedParam(INPUT_BUFFER_MODE);
            Input_Buffer_Mode_Config2 = inst.GetCommittedParam(INPUT_BUFFER_MODE_CONFIG2);
            Input_Buffer_Mode_Config3 = inst.GetCommittedParam(INPUT_BUFFER_MODE_CONFIG3);
            Input_Buffer_Mode_Config4 = inst.GetCommittedParam(INPUT_BUFFER_MODE_CONFIG4);

            Sample_Rate = inst.GetCommittedParam(SAMPLE_RATE);
            Sample_Rate_Config2 = inst.GetCommittedParam(SAMPLE_RATE_CONFIG2);
            Sample_Rate_Config3 = inst.GetCommittedParam(SAMPLE_RATE_CONFIG3);
            Sample_Rate_Config4 = inst.GetCommittedParam(SAMPLE_RATE_CONFIG4);
            Ref_Voltage = inst.GetCommittedParam(REF_VOLTAGE);
            Ref_Voltage_Config2 = inst.GetCommittedParam(REF_VOLTAGE_CONFIG2);
            Ref_Voltage_Config3 = inst.GetCommittedParam(REF_VOLTAGE_CONFIG3);
            Ref_Voltage_Config4 = inst.GetCommittedParam(REF_VOLTAGE_CONFIG4);

            Clock_Frequency = inst.GetCommittedParam(CLOCK_FREQUENCY);
            Start_of_Conversion = inst.GetCommittedParam(START_OF_CONVERSION);
            Configs = inst.GetCommittedParam(CONFIGS);
            ADC_Charge_Pump_Clock = inst.GetCommittedParam(ADC_CHARGE_PUMP_CLOCK);
            ADC_Input_Mode = inst.GetCommittedParam(ADC_INPUT_MODE);
            Enable_Vref_Vss = inst.GetCommittedParam(ADC_nVref);

            ADC_ExVref_Num = inst.GetCommittedParam("ADC_ExVref_Num");

            Vdda_Value = inst.GetCommittedParam(ADC_Vdda_Value);
        }
        
        private void SetParam(ICyInstEdit_v1 inst, string ParamName, string value)
        {

        }

        private void CommitParams(ICyInstEdit_v1 inst)
        {
            inst.CommitParamExprs();
        }
    }
}