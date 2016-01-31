//*******************************************************************************
// File Name: CyADC_SARParameters.cs
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

namespace ADC_SAR_v1_10
{
    class ADC_SARParameters
    {
        public CyCompDevParam m_ADC_Power = null;
        public CyCompDevParam m_ADC_Resolution = null;
        public CyCompDevParam m_ADC_Clock = null;
        public CyCompDevParam m_ADC_SampleMode = null;
        public CyCompDevParam m_ADC_Clock_Frequency = null;

        public CyCompDevParam m_ADC_Input_Range = null;
        public CyCompDevParam m_ADC_Reference = null;
        public CyCompDevParam m_Ref_Voltage = null;
        public CyCompDevParam m_Sample_Rate = null;


        public ADC_SARParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_ADC_Power = inst.GetCommittedParam("ADC_Power");
            m_ADC_Resolution = inst.GetCommittedParam("ADC_Resolution");
            m_ADC_Clock = inst.GetCommittedParam("ADC_Clock");
            m_ADC_Clock_Frequency = inst.GetCommittedParam("ADC_Clock_Frequency");
            m_ADC_SampleMode = inst.GetCommittedParam("ADC_SampleMode");
            
            m_ADC_Input_Range = inst.GetCommittedParam("ADC_Input_Range");
            m_ADC_Reference = inst.GetCommittedParam("ADC_Reference");
            m_Sample_Rate = inst.GetCommittedParam("Sample_Rate");
            m_Ref_Voltage = inst.GetCommittedParam("Ref_Voltage");
            
        }
        
        private void SetParam(ICyInstEdit_v1 inst, string paramName, string value)
        {

        }

        private void CommitParams(ICyInstEdit_v1 inst)
        {
            inst.CommitParamExprs();
        }
    }
}