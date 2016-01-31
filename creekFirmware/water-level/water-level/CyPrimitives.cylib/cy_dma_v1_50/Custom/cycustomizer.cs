/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Components.System.cy_dma_v1_50
{
    public class CyCustomizer :
        ICyVerilogCustomize_v1, ICyParamEditHook_v1, ICyToolTipCustomize_v1
    {
        //-----------------------------

        const string BuiltinTabName = "Built-in";
        public const string BasicTabName = "Basic";

        //-----------------------------

        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            CyCustErr err = CyCustErr.Ok;
            codeSnippet = string.Empty;
            CyVerilogWriter vw = new CyVerilogWriter("cy_dma_v1_0", query.InstanceName);

            //Add Generics.
            foreach (string paramName in query.GetParamNames())
            {
                CyCompDevParam param = query.GetCommittedParam(paramName);
                if (param.IsHardware)
                {
                    vw.AddGeneric(param.Name, param.Value);
                }
            }

            //Add Ports
            foreach (string termName in termQuery.GetTerminalNames())
            {
                string termBaseName = termQuery.GetTermBaseName(termName);
                CyCompDevTermDir termDirection = termQuery.GetTermDirection(termName);
                bool termHasNoDrivers = termQuery.GetHasNoDrivers(termName);

                string value;
                if (termDirection != CyCompDevTermDir.OUTPUT && termHasNoDrivers)
                {
                    value = termQuery.GetTermDefaultVlogExpr(termName);
                }
                else
                {
                    value = termQuery.GetTermSigSegName(termName);
                }
                vw.AddPort(termBaseName, value);
            }

            codeSnippet = vw.ToString();
            return err;
        }

        #endregion

        //-----------------------------

        #region ICyParamEditHook_v1 Members

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CyDMAControl control = new CyDMAControl(edit);
            editor.AddCustomPage(BasicTabName, control, control.Update, BasicTabName);
            editor.AddDefaultPage(BuiltinTabName, BuiltinTabName);
            return editor.ShowDialog();
        }

        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        #endregion

        //-----------------------------

        #region ICyToolTipCustomize_v1 Members

        public string CustomizeToolTip(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            string newToolTip = query.DefaultToolTipText;

            CyDMAParameters.CyHardwareRequestEnum hwReq;
            CyCustErr err = CyDMAParameters.GetHardwareRequestValue(query, out hwReq);

            string hwReqString;
            if (err.IsNotOk)
            {
                hwReqString = string.Format("Error. '{0}'", err.Message);
            }
            else
            {
                switch (hwReq)
                {
                    case CyDMAParameters.CyHardwareRequestEnum.Disabled:
                        hwReqString = "Disabled";
                        break;

                    case CyDMAParameters.CyHardwareRequestEnum.RisingEdge:
                        hwReqString = "Rising Edge";
                        break;

                    case CyDMAParameters.CyHardwareRequestEnum.Level:
                        hwReqString = "Level";
                        break;

                    case CyDMAParameters.CyHardwareRequestEnum.Derived:
                        hwReqString = "Derived";
                        break;

                    default:
                        Debug.Fail("unhandled");
                        hwReqString = string.Empty;
                        break;
                }
            }

            newToolTip += Environment.NewLine;
            newToolTip += string.Format("Hardware Request = {0}", hwReqString);

            return newToolTip;
        }

        #endregion

        //-----------------------------
    }

    public class CyDMAParameters
    {
        const string ParamName_HardwareRequestEnabled = "hw_request_enabled";
        const string ParamName_HardwareTerminationEnabled = "hw_termination_enabled";
        const string ParamName_HardwareRequestType = "hw_request_type";

        const int HarewareRequestTypeEnumValue_Derived = 0;
        const int HarewareRequestTypeEnumValue_RisingEdge = 1;
        const int HarewareRequestTypeEnumValue_Level = 2;

        static CyCustErr GetParamValue<T>(ICyInstQuery_v1 query, string paramName, out T value)
        {
            CyCustErr err = CyCustErr.Ok;
            value = default(T);

            CyCompDevParam param = query.GetCommittedParam(paramName);
            if (param == null)
            {
                return new CyCustErr(string.Format("Unknown parameter '{0}'", paramName));
            }

            if (param.ErrorCount != 0)
            {
                return err = new CyCustErr(param.ErrorMsgs);
            }

            err = param.TryGetValueAs<T>(out value);
            if (err.IsNotOk)
            {
                value = default(T);
                return err;
            }

            return err;
        }

        public static CyCustErr GetHardwareTerminationEnabledValue(ICyInstQuery_v1 query, out bool value)
        {
            return GetParamValue<bool>(query, ParamName_HardwareTerminationEnabled, out value);
        }

        public static CyCustErr GetHardwareRequestValue(ICyInstQuery_v1 query, out CyHardwareRequestEnum value)
        {
            bool enabled;
            int intVal;
            CyCustErr err;

            err = GetParamValue<int>(query, ParamName_HardwareRequestType, out intVal);
            if (err.IsNotOk)
            {
                value = CyHardwareRequestEnum.Derived;
                return err;
            }

            switch (intVal)
            {
                case HarewareRequestTypeEnumValue_Derived:
                    value = CyHardwareRequestEnum.Derived;
                    break;

                case HarewareRequestTypeEnumValue_RisingEdge:
                    value = CyHardwareRequestEnum.RisingEdge;
                    break;

                case HarewareRequestTypeEnumValue_Level:
                    value = CyHardwareRequestEnum.Level;
                    break;

                default:
                    Debug.Fail("unhandled");
                    value = CyHardwareRequestEnum.Derived;
                    if (err.IsOk)
                    {
                        err = new CyCustErr("Unahndled Hardware Request Type '{0}'", intVal);
                    }
                    break;
            }

            err = GetParamValue<bool>(query, ParamName_HardwareRequestEnabled, out enabled);
            if (enabled == false)
            {
                value = CyHardwareRequestEnum.Disabled;
            }

            return err;
        }

        public static void SetHardwareTerminationEnabledValue(ICyInstEdit_v1 edit, bool value)
        {
            edit.SetParamExpr(ParamName_HardwareTerminationEnabled, value.ToString());
            edit.CommitParamExprs();
        }

        public static void SetHardwareRequestValue(ICyInstEdit_v1 edit, CyHardwareRequestEnum value)
        {
            bool hardwareRequestEnabled;
            int hardwareRequestType;

            switch (value)
            {
                case CyHardwareRequestEnum.Disabled:
                    hardwareRequestEnabled = false;
                    hardwareRequestType = HarewareRequestTypeEnumValue_Derived;
                    break;

                case CyHardwareRequestEnum.Derived:
                    hardwareRequestEnabled = true;
                    hardwareRequestType = HarewareRequestTypeEnumValue_Derived;
                    break;

                case CyHardwareRequestEnum.RisingEdge:
                    hardwareRequestEnabled = true;
                    hardwareRequestType = HarewareRequestTypeEnumValue_RisingEdge;
                    break;

                case CyHardwareRequestEnum.Level:
                    hardwareRequestEnabled = true;
                    hardwareRequestType = HarewareRequestTypeEnumValue_Level;
                    break;

                default:
                    Debug.Fail("unhandled");
                    hardwareRequestEnabled = false;
                    hardwareRequestType = HarewareRequestTypeEnumValue_Derived;
                    break;
            }

            edit.SetParamExpr(ParamName_HardwareRequestEnabled, hardwareRequestEnabled.ToString());
            edit.SetParamExpr(ParamName_HardwareRequestType, hardwareRequestType.ToString());
            edit.CommitParamExprs();
        }

        public enum CyHardwareRequestEnum
        {
            Disabled,
            RisingEdge,
            Level,
            Derived,
        }
    }
}