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

namespace Counter_v1_20
{
    class CounterParameters
    {
        public CyCompDevParam CaptureMode = null;
        public CyCompDevParam ClockMode = null;
        public CyCompDevParam CompareMode = null;
        public CyCompDevParam CompareValue = null;
        public CyCompDevParam EnableMode = null;
        public CyCompDevParam FixedFunction = null;
        public CyCompDevParam InterruptOnCapture = null;
        public CyCompDevParam InterruptOnCompare = null;
        public CyCompDevParam InterruptOnOverUnderFlow = null;        
        public CyCompDevParam InterruptOnTC = null;        
        public CyCompDevParam Period = null;
        public CyCompDevParam ReloadOnCapture = null;
        public CyCompDevParam ReloadOnCompare = null;
        public CyCompDevParam ReloadOnOverUnder = null;
        public CyCompDevParam ReloadOnReset = null;
        public CyCompDevParam Resolution = null;

        public CounterParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            CaptureMode = inst.GetCommittedParam("CaptureMode");
            ClockMode = inst.GetCommittedParam("ClockMode");
            CompareMode = inst.GetCommittedParam("CompareMode");
            CompareValue = inst.GetCommittedParam("CompareValue");
            EnableMode = inst.GetCommittedParam("EnableMode");
            FixedFunction = inst.GetCommittedParam("FixedFunction");
            InterruptOnCapture = inst.GetCommittedParam("InterruptOnCapture");
            InterruptOnCompare = inst.GetCommittedParam("InterruptOnCompare");
            InterruptOnOverUnderFlow = inst.GetCommittedParam("InterruptOnOverUnderFlow");
            InterruptOnTC = inst.GetCommittedParam("InterruptOnTC");
            Period = inst.GetCommittedParam("Period");
            ReloadOnCapture = inst.GetCommittedParam("ReloadOnCapture");
            ReloadOnCompare = inst.GetCommittedParam("ReloadOnCompare");
            ReloadOnOverUnder = inst.GetCommittedParam("ReloadOnOverUnder");
            ReloadOnReset = inst.GetCommittedParam("ReloadOnReset");
            Resolution = inst.GetCommittedParam("Resolution");
        }
    }

    
}
