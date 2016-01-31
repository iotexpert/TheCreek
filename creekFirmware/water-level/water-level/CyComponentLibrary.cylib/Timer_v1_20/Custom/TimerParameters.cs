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

namespace Timer_v1_20
{
    class TimerParameters
    {
        public CyCompDevParam CaptureAlternatingRise = null;
        public CyCompDevParam CaptureAlternatingFall = null;
        public CyCompDevParam CaptureCount = null;
        public CyCompDevParam CaptureCounterEnabled = null;
        public CyCompDevParam CaptureMode = null;
        public CyCompDevParam EnableMode = null;
        public CyCompDevParam FixedFunction = null;
        public CyCompDevParam InterruptOnCapture = null;
        public CyCompDevParam InterruptOnTC = null;
        public CyCompDevParam InterruptOnFIFOFull = null;
        public CyCompDevParam NumberOfCaptures = null;
        public CyCompDevParam Period = null;
        public CyCompDevParam Resolution = null;
        public CyCompDevParam RunMode = null;
        public CyCompDevParam TriggerMode = null;

        public TimerParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            CaptureAlternatingRise = inst.GetCommittedParam("CaptureAlternatingRise");
            CaptureAlternatingFall = inst.GetCommittedParam("CaptureAlternatingFall");
            CaptureCount = inst.GetCommittedParam("CaptureCount");
            CaptureCounterEnabled = inst.GetCommittedParam("CaptureCounterEnabled");
            CaptureMode = inst.GetCommittedParam("CaptureMode");
            EnableMode = inst.GetCommittedParam("EnableMode");
            FixedFunction = inst.GetCommittedParam("FixedFunction");
            InterruptOnCapture = inst.GetCommittedParam("InterruptOnCapture");
            InterruptOnTC = inst.GetCommittedParam("InterruptOnTC");
            InterruptOnFIFOFull = inst.GetCommittedParam("InterruptOnFIFOFull");
            NumberOfCaptures = inst.GetCommittedParam("NumberOfCaptures");
            Resolution = inst.GetCommittedParam("Resolution");
            Period = inst.GetCommittedParam("Period");
            RunMode = inst.GetCommittedParam("RunMode");
            TriggerMode = inst.GetCommittedParam("TriggerMode");
        }
    }

    
}
