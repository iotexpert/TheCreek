//*******************************************************************************
// File Name: CyADC_DelSigParameters.cs
/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace ADC_DelSig_v1_30
{
    class ADC_DelSigParameters
    {
        public CyCompDevParam ADC_Clock = null;
        public CyCompDevParam ADC_Power = null;
        public CyCompDevParam ADC_Input_Range = null;
        public CyCompDevParam ADC_ExVref_Num = null;
        public CyCompDevParam ADC_Reference = null;
        public CyCompDevParam ADC_Resolution = null;
        public CyCompDevParam Conversion_Mode = null;
        public CyCompDevParam Input_Buffer_Gain = null;
        public CyCompDevParam Sample_Rate = null;
        public CyCompDevParam Clock_Frequency = null;
        public CyCompDevParam Start_of_Conversion = null;
        public CyCompDevParam Ref_Voltage = null;

        public ADC_DelSigParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            ADC_Clock = inst.GetCommittedParam("ADC_Clock");
            ADC_Input_Range = inst.GetCommittedParam("ADC_Input_Range");
            ADC_Power = inst.GetCommittedParam("ADC_Power");
            ADC_Reference = inst.GetCommittedParam("ADC_Reference");
            ADC_ExVref_Num = inst.GetCommittedParam("ADC_ExVref_Num");
            ADC_Resolution = inst.GetCommittedParam("ADC_Resolution");
            Conversion_Mode = inst.GetCommittedParam("Conversion_Mode");
            Input_Buffer_Gain = inst.GetCommittedParam("Input_Buffer_Gain");
            Sample_Rate = inst.GetCommittedParam("Sample_Rate");
            Clock_Frequency = inst.GetCommittedParam("Clock_Frequency");
            Start_of_Conversion = inst.GetCommittedParam("Start_of_Conversion");
            Ref_Voltage = inst.GetCommittedParam("Ref_Voltage");
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