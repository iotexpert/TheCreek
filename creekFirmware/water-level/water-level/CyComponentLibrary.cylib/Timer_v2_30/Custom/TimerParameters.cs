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
using CyDesigner.Extensions.Common;

namespace Timer_v2_30
{
    class CyTimerParameters
    {
        #region Header
        public const string S_INSTANCE_NAME = "INSTANCE_NAME";
        public const string S_TRIGGERMODE = "TriggerMode";
        public const string S_CAPTUREMODE = "CaptureMode";
        public const string S_ENABLEMODE = "EnableMode";
        public const string S_RUNMODE = "RunMode";
        public const string S_CAPTURECOUNTERENABLED = "CaptureCounterEnabled";
        public const string S_RESOLUTION = "Resolution";
        public const string S_FIXEDFUNCTION = "FixedFunction";
        public const string S_CAPTUREALTERNATINGRISE = "CaptureAlternatingRise";
        public const string S_CAPTUREALTERNATINGFALL = "CaptureAlternatingFall";
        public const string S_PERIOD = "Period";
        public const string S_INTERRUPTONCAPTURE = "InterruptOnCapture";
        public const string S_INTERRUPTONTC = "InterruptOnTC";
        public const string S_INTERRUPTONFIFOFULL = "InterruptOnFIFOFull";
        public const string S_NUMBEROFCAPTURES = "NumberOfCaptures";
        public const string S_CONFIGURE = "Configure";
        public const string S_BASIC = "Basic";
        public const string S_CAPTURECOUNT = "CaptureCount";
        public const string S_ON_CAPTURE = "On Capture";
        public const string S_MUST_BE_A_VALID_VALUE = "Must be a valid value";
        public const string S_WARNING = "Warning";
        public const string S_VALUE_MUST_BE_FROM_2_TO_127 = "Value must be from 2 to 127";
        public const string S_VALUE_MUST_BE_FROM_1_TO_4 = "Value must be from 1 to 4";
        public const string S_CLOCK = "clock";
        public const string S_HARDWARE_ONLY = "Hardware Only";
        public const string S_SOFTWARE_AND_HARDWARE = "Software and Hardware";
        public const string S_ONE_SHOT = "One Shot";
        public const string S_ONE_SHOT_HALT_ON_INTERRUPT = "One Shot (Halt On Interrupt)";
        public const string S_PERIOD_UNKNOWN_SOURCE_FREQ_ERROR = "Period = UNKNOWN SOURCE FREQ";
        public const string S_PERIOD_MESSAGE = "Period = {0}{1}";
        public const string S_ON_CAPTURE_MESSAGE = "On Capture [1-4]";
        public const string S_8BIT_MESSAGE1 = "Period of 8-Bit Timer must be between 2 and {0}";
        public const string S_16BIT_MESSAGE1 = "Period of 16-Bit Timer must be between 2 and {0}";
        public const string S_24BIT_MESSAGE1 = "Period of 24-Bit Timer must be between 2 and {0}";
        public const string S_32BIT_MESSAGE1 = "Period of 32-Bit Timer must be between 2 and {0}";
        public const string S_PIN_MESSAGE1 = "Please make sure that the 'enable' pin is not tied " + 
                                             "to logic high while enabling the Timer.";
        public const string S_PARAM_MESSAGE1 = "Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}";
        #endregion

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

        public CyTimerParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            CaptureAlternatingRise = inst.GetCommittedParam(S_CAPTUREALTERNATINGRISE);
            CaptureAlternatingFall = inst.GetCommittedParam(S_CAPTUREALTERNATINGFALL);
            CaptureCount = inst.GetCommittedParam(S_CAPTURECOUNT);
            CaptureCounterEnabled = inst.GetCommittedParam(S_CAPTURECOUNTERENABLED);
            CaptureMode = inst.GetCommittedParam(S_CAPTUREMODE);
            EnableMode = inst.GetCommittedParam(S_ENABLEMODE);
            FixedFunction = inst.GetCommittedParam(S_FIXEDFUNCTION);
            InterruptOnCapture = inst.GetCommittedParam(S_INTERRUPTONCAPTURE);
            InterruptOnTC = inst.GetCommittedParam(S_INTERRUPTONTC);
            InterruptOnFIFOFull = inst.GetCommittedParam(S_INTERRUPTONFIFOFULL);
            NumberOfCaptures = inst.GetCommittedParam(S_NUMBEROFCAPTURES);
            Resolution = inst.GetCommittedParam(S_RESOLUTION);
            Period = inst.GetCommittedParam(S_PERIOD);
            RunMode = inst.GetCommittedParam(S_RUNMODE);
            TriggerMode = inst.GetCommittedParam(S_TRIGGERMODE);
        }
        
        public const string CaptureModeEnumID_None = "CM_None";

        static CyCustErr GetParamValue<T>(ICyInstQuery_v1 query, string paramName, out T value)
        {
            CyCustErr err = CyCustErr.Ok;
            value = default(T);
    
            CyCompDevParam param = query.GetCommittedParam(paramName);
            if (param == null)
            {
                return new CyCustErr(string.Format("Unknown Parameter '{0}'", paramName));
            }
    
            if (param.ErrorCount != 0)
            {
                err = new CyCustErr(param.ErrorMsgs);
            }
    
            err = param.TryGetValueAs<T>(out value);
            if (err.IsNotOk)
            {
                value = default(T);
                return err;
            }
    
            return err;
        }
    
        static CyCustErr GetEnumParamValues(ICyInstQuery_v1 query, string paramName, out int value, 
                                            out string displayName,
            out string idName)
        {
            CyCustErr err = GetParamValue<int>(query, paramName, out value);
    
            CyCompDevParam param = query.GetCommittedParam(paramName);
            displayName = query.ResolveEnumIdToDisplay(paramName, param.Expr);
            idName = query.ResolveEnumDisplayToId(paramName, displayName);
    
            return err;
        }
    
        public static CyCustErr GetFixedFunctionValue(ICyInstQuery_v1 query, out bool value)
        {
            return GetParamValue<bool>(query, S_FIXEDFUNCTION, out value);
        }
    
        public static CyCustErr GetRunModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;
    
            return GetEnumParamValues(query, S_RUNMODE, out value, out displayName, out idName);
        }

        public static CyCustErr GetTriggerModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, S_TRIGGERMODE, out value, out displayName, out idName);
        }

        public static CyCustErr GetEnableModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, S_ENABLEMODE, out value, out displayName, out idName);
        }

        public static CyCustErr GetCaptureModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, S_CAPTUREMODE, out value, out displayName, out idName);
        }

    }    
}
