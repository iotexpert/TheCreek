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

namespace Comp_v1_50
{
    public class ComparatorParameters
    {
        public CyCompDevParam Comp_Hysteresis = null;
        public CyCompDevParam Comp_Polarity = null;
        public CyCompDevParam Comp_PDOverride = null;
        public CyCompDevParam Comp_Sync = null;
        public CyCompDevParam Comp_Speed = null;

        public ComparatorParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            Comp_Hysteresis = inst.GetCommittedParam("Hysteresis");
            Comp_Polarity = inst.GetCommittedParam("Polarity");
            Comp_PDOverride = inst.GetCommittedParam("Pd_Override");
            Comp_Sync = inst.GetCommittedParam("Sync");
            Comp_Speed = inst.GetCommittedParam("Speed");
        }
        
        
    }
}