/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace Comp_v1_90
{
    public class CyComparatorParameters
    {
        public CyCompDevParam m_comp_Hysteresis = null;
        public CyCompDevParam m_comp_Polarity = null;
        public CyCompDevParam m_comp_PDOverride = null;
        public CyCompDevParam m_comp_Sync = null;
        public CyCompDevParam m_comp_Speed = null;

        public CyComparatorParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_comp_Hysteresis = inst.GetCommittedParam("Hysteresis");
            m_comp_Polarity = inst.GetCommittedParam("Polarity");
            m_comp_PDOverride = inst.GetCommittedParam("Pd_Override");
            m_comp_Sync = inst.GetCommittedParam("Sync");
            m_comp_Speed = inst.GetCommittedParam("Speed");
        }
        
        
    }
}


//[] END OF FILE