/*******************************************************************************
 File Name: CyADC_DelSigParameters.cs
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace Comp_v1_70
{
    public class CyComparatorParameters
    {
        public CyCompDevParam m_Comp_Hysteresis = null;
        public CyCompDevParam m_Comp_Polarity = null;
        public CyCompDevParam m_Comp_PDOverride = null;
        public CyCompDevParam m_Comp_Sync = null;
        public CyCompDevParam m_Comp_Speed = null;

        public CyComparatorParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_Comp_Hysteresis = inst.GetCommittedParam("Hysteresis");
            m_Comp_Polarity = inst.GetCommittedParam("Polarity");
            m_Comp_PDOverride = inst.GetCommittedParam("Pd_Override");
            m_Comp_Sync = inst.GetCommittedParam("Sync");
            m_Comp_Speed = inst.GetCommittedParam("Speed");
        }
        
        
    }
}