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
using System.Windows.Forms;

namespace Counter_v2_20
{
    class CounterParameters
    {
        public CyCompDevParam CaptureMode = null;
        public CyCompDevParam ClockMode = null;
        public CyCompDevParam CompareMode = null;
        public CyCompDevParam CompareValue = null;
        public CyCompDevParam EnableMode = null;
        public CyCompDevParam RunMode = null;
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

        public const string CONFIGURE = "Configure";
        public const string ADVANCED = "Advanced";
        public const string BUILT_IN = "Built-in";
        public const string BASIC = "Basic";

        public const string CAL_PERIOD = "Period:";
        public const string BIT_RESOLUTION = "Bit Resolution:";
        public const string CLOCKMODE = "ClockMode";
        public const string COMPAREMODE = "CompareMode";
        public const string INTERRUPT_ON_CAPTURE = "InterruptOnCapture";
        public const string PERIOD = "Period";
        public const string UNIT = "u";
        public const string RESOLUTION = "Resolution";
        public const string EIGHT_BIT = "8";
        public const string SIXTEEN_BIT = "16";
        public const string TWENTYFOUR_BIT = "24";
        public const string THIRTYTWO_BIT = "32";
        public const string FIXEDFUNCTION = "FixedFunction";
        public const string TRUE = "true";
        public const string FALSE = "false";
        public const string COMPAREVALUE = "CompareValue";
        public const string ENABLEMODE = "EnableMode";
        public const string RUNMODE = "RunMode";
        public const string SOFTWARE = "Software";
        public const string ONLY = "Only";
        public const string INTERRUPT_ON_COMPARE = "InterruptOnCompare";
        public const string RELOAD_ON_CAPTURE = "ReloadOnCapture";
        public const string RELOAD_ON_COMPARE = "ReloadOnCompare";
        public const string RELOAD_ON_RESET= "ReloadOnReset";
        public const string RELOAD_ON_OVERUNDER = "ReloadOnOverUnder";
        public const string INTERRUPT_ON_TC = "InterruptOnTC";
        public const string CAPTUREMODE = "CaptureMode";
        public const string INTERRUPT_ON_OVER_UNDERFLOW = "InterruptOnOverUnderFlow";
        public const string UNHANDLED_CASE = "unhandled case";
        public const string MAX_PERIOD = "Max. Period:";

        public const uint CONST_2_8 = 255;
        public const uint CONST_2_16 = 65535;
        public const uint CONST_2_24 = 16777215;
        public const uint CONST_2_32 = 4294967295;

        public const string ClockModeEnumID_DownCounter = "Down Counter";
        public const string CompareModeEnumID_LessThan = "Less Than";     

        public CounterParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);            
        }
       
        private void GetParams(ICyInstEdit_v1 inst)
        {
            CaptureMode = inst.GetCommittedParam(CAPTUREMODE);
            ClockMode = inst.GetCommittedParam(CLOCKMODE);
            CompareMode = inst.GetCommittedParam(COMPAREMODE);
            CompareValue = inst.GetCommittedParam(COMPAREVALUE);
            EnableMode = inst.GetCommittedParam(ENABLEMODE);
            RunMode = inst.GetCommittedParam(RUNMODE);
            FixedFunction = inst.GetCommittedParam(FIXEDFUNCTION);
            InterruptOnCapture = inst.GetCommittedParam(INTERRUPT_ON_CAPTURE);
            InterruptOnCompare = inst.GetCommittedParam(INTERRUPT_ON_COMPARE);
            InterruptOnOverUnderFlow = inst.GetCommittedParam(INTERRUPT_ON_OVER_UNDERFLOW);
            InterruptOnTC = inst.GetCommittedParam(INTERRUPT_ON_TC);
            Period = inst.GetCommittedParam(PERIOD);
            ReloadOnCapture = inst.GetCommittedParam(RELOAD_ON_CAPTURE);
            ReloadOnCompare = inst.GetCommittedParam(RELOAD_ON_COMPARE);
            ReloadOnOverUnder = inst.GetCommittedParam(RELOAD_ON_OVERUNDER);
            ReloadOnReset = inst.GetCommittedParam(RELOAD_ON_RESET);
            Resolution = inst.GetCommittedParam(RESOLUTION);
        }

        static CyCustErr GetParamValue<T>(ICyInstQuery_v1 query, string paramName, out T value)
        {
            CyCustErr err = CyCustErr.Ok;
            value = default(T);

            CyCompDevParam param = query.GetCommittedParam(paramName);
            if (param == null)
            {
                return new CyCustErr(string.Format("Unknown Parameter '{0}'", paramName));
            }

            err = param.TryGetValueAs<T>(out value);
            if (err.IsNotOk)
            {
                value = default(T);
                return err;
            }

            if (param.ErrorCount != 0)
            {
                err = new CyCustErr(param.ErrorMsgs);
            }

            return err;
        }

        public static void SetAParameter(string paramName, string value, ICyInstEdit_v1 inst)
        {
            if (value != null && paramName != null)
            {
                inst.SetParamExpr(paramName, value);
                inst.CommitParamExprs();
            }
            if (inst.GetCommittedParam(paramName).ErrorCount != 0)
            {
                string errors = null;
                foreach (string err in inst.GetCommittedParam(paramName).Errors)
                {
                    errors = errors + err + "\n";
                }
                MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", paramName,
                                value, errors), "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static CyCustErr GetEnumParamValues(ICyInstQuery_v1 query, string paramName, out int value, 
        out string displayName, out string idName)
        {
            CyCustErr err = GetParamValue<int>(query, paramName, out value);

            CyCompDevParam param = query.GetCommittedParam(paramName);
            displayName = query.ResolveEnumIdToDisplay(paramName, param.Expr);
            idName = query.ResolveEnumDisplayToId(paramName, displayName);
            return err;
        }

        public static CyCustErr GetFixedFunctionValue(ICyInstQuery_v1 query, out bool value)
        {
            return GetParamValue<bool>(query, FIXEDFUNCTION, out value);
        }

        public static CyCustErr GetClockModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, CLOCKMODE, out value, out displayName, out idName);
        }

        public static CyCustErr GetCompareModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, COMPAREMODE, out value, out displayName, out idName);
        }

        public static CyCustErr GetCaptureModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, CAPTUREMODE, out value, out displayName, out idName);
        }

        public static CyCustErr GetEnableModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, ENABLEMODE, out value, out displayName, out idName);
        }

        public static CyCustErr GetRunModeValue(ICyInstQuery_v1 query, out string displayName)
        {
            int value;
            string idName;

            return GetEnumParamValues(query, RUNMODE, out value, out displayName, out idName);
        }
    }        
}
