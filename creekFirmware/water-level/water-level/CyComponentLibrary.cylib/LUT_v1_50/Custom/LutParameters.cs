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

namespace LUT_v1_50
{
    class LutParameters
    {
        public CyCompDevParam NumInputs = null;
        public CyCompDevParam NumOutputs = null;
        public CyCompDevParam RegisterOutputs = null;
        public CyCompDevParam BitField = null;


        public LutParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            NumInputs = inst.GetCommittedParam("NumInputs");
            NumOutputs = inst.GetCommittedParam("NumOutputs");
            RegisterOutputs = inst.GetCommittedParam("RegisterOutputs");
            BitField = inst.GetCommittedParam("BitField");
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
