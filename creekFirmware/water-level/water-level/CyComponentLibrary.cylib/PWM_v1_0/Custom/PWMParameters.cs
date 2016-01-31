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

namespace PWM_v1_0
{
    public class PWMParameters
    {
        public CyCompDevParam CaptureMode = null;
        public CyCompDevParam ClockInternal = null;
        public CyCompDevParam CompareType1 = null;
        public CyCompDevParam CompareType2 = null;
        public CyCompDevParam CompareValue1 = null;
        public CyCompDevParam CompareValue2 = null;
        public CyCompDevParam CY_REMOVE = null;
        public CyCompDevParam DeadBand = null;
        public CyCompDevParam DeadTime = null;
        public CyCompDevParam DeadTimeRange = null;
        public CyCompDevParam DitherOffset = null;
        public CyCompDevParam EnableMode = null;
        public CyCompDevParam FixedFunction = null;
        public CyCompDevParam InterruptOnKill = null;
        public CyCompDevParam InterruptOnCMP1 = null;
        public CyCompDevParam InterruptOnCMP2 = null;
        public CyCompDevParam InterruptOnTC = null;
        public CyCompDevParam KillMode = null;
        public CyCompDevParam MinimumKillTime = null;
        public CyCompDevParam Period = null;
        public CyCompDevParam PWMMode = null;
        public CyCompDevParam Resolution = null;
        public CyCompDevParam RunMode = null;
        public CyCompDevParam TriggerMode = null;
		public CyCompDevParam UseInterrupt = null;

        public PWMParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            CaptureMode = inst.GetCommittedParam("CaptureMode");
            CompareType1 = inst.GetCommittedParam("CompareType1");
            CompareType2 = inst.GetCommittedParam("CompareType2");
            CompareValue1 = inst.GetCommittedParam("CompareValue1");
            CompareValue2 = inst.GetCommittedParam("CompareValue2");
            CY_REMOVE = inst.GetCommittedParam("CY_REMOVE");
            DeadBand = inst.GetCommittedParam("DeadBand");
            DeadTime = inst.GetCommittedParam("DeadTime");
            DeadTimeRange = inst.GetCommittedParam("DeadTimeRange");
            DitherOffset = inst.GetCommittedParam("DitherOffset");
            EnableMode = inst.GetCommittedParam("EnableMode");
            FixedFunction = inst.GetCommittedParam("FixedFunction");
            InterruptOnKill = inst.GetCommittedParam("InterruptOnKill");
            InterruptOnCMP1 = inst.GetCommittedParam("InterruptOnCMP1");
            InterruptOnCMP2 = inst.GetCommittedParam("InterruptOnCMP2");
            InterruptOnTC = inst.GetCommittedParam("InterruptOnTC");
            KillMode = inst.GetCommittedParam("KillMode");
            MinimumKillTime = inst.GetCommittedParam("MinimumKillTime");
            Period = inst.GetCommittedParam("Period");
            PWMMode = inst.GetCommittedParam("PWMMode");
            Resolution = inst.GetCommittedParam("Resolution");
            RunMode = inst.GetCommittedParam("RunMode");
            TriggerMode = inst.GetCommittedParam("TriggerMode");
			UseInterrupt = inst.GetCommittedParam("UseInterrupt");
        }
    }
}
