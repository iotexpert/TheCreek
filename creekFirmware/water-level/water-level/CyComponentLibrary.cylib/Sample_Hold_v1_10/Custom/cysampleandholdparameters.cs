/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using Sample_Hold_v1_10;

namespace Sample_Hold_v1_10
{
    public class Cysampleandholdparameters
    {
        #region Param Name Constants
        public const string POWER = "Power";
        public const string SAMPLE_MODE = "Sample_Mode";
        public const string SAMPLE_CLOCK_EDGE = "Sample_Clock_Edge";
        public const string VREF_TYPE = "Vref_Type";
        public const string NEGATIVE = "Negative";
        public const string POSITIVE_AND_NEGATIVE = "PositiveAndNegative";        
        #endregion

        public CyCompDevParam m_Power = null;
        public CyCompDevParam m_Sample_Mode = null;
        public CyCompDevParam m_Sample_Clock_Edge = null;
        public CyCompDevParam m_Vref_Type = null;
       

        public Cysampleandholdparameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_Power = inst.GetCommittedParam(POWER);
            m_Sample_Mode = inst.GetCommittedParam(SAMPLE_MODE);
            m_Sample_Clock_Edge = inst.GetCommittedParam(SAMPLE_CLOCK_EDGE);
            m_Vref_Type = inst.GetCommittedParam(VREF_TYPE);
        }
    }
}
