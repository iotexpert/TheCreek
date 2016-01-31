/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Diagnostics;
using Sample_Hold_v1_10;

namespace Sample_Hold_v1_10
{
    public class CyCustomizer : ICyParamEditHook_v1, ICyExprEval_v2, ICyAPICustomize_v1
    {
        const string BasicTabName = "Basic";
        const string BuiltinTabName = "Built-in";
        const string ConfigureTabName = "Configure";
        const string ClockEdge = "Sample_clock_edge";
        //string SampleHold = "1";
        //string TrackHold = "0";
        //string EdgeNegative = "0";
        //string EdgePositiveNegative = "1";

        CySampleAndHoldControl s_control = null;

        CySampleHold_EditingControl s_editingControl;

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {            
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            s_control = new CySampleAndHoldControl(edit, termQuery);

            s_editingControl = new CySampleHold_EditingControl(edit, s_control, BasicTabName);

            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                s_control.UpdateFormFromParams(edit);
            };

            editor.AddCustomPage(ConfigureTabName, s_editingControl, ExprDelegate, BasicTabName);

            editor.AddDefaultPage(BuiltinTabName, BuiltinTabName);
            editor.UseBigEditor = false;


            DialogResult result = editor.ShowDialog();

            return result;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion


        public class CySampleHold_EditingControl : ICyParamEditingControl
        {
            CySampleAndHoldControl Sample_Control;
            ICyInstEdit_v1 s_instEdit;
            List<string> s_tabNames;

            public CySampleHold_EditingControl(ICyInstEdit_v1 inst, CySampleAndHoldControl userControl, 
                                             params string[] tabNames)
            {
                s_instEdit = inst;
                s_tabNames = new List<string>(tabNames);
                Sample_Control = userControl;
                Sample_Control.Dock = DockStyle.Fill;
                Sample_Control.AutoScroll = true;
            }

            #region ICyParamEditingControl Members

            Control ICyParamEditingControl.DisplayControl
            {
                get
                {
                    return Sample_Control;
                }
            }

            System.Collections.Generic.IEnumerable<CyCustErr> ICyParamEditingControl.GetErrors()
            {
                List<CyCustErr> errs = new List<CyCustErr>();

                foreach (string paramName in s_instEdit.GetParamNames())
                {
                    CyCompDevParam param = s_instEdit.GetCommittedParam(paramName);
                    if (s_tabNames.Contains(param.TabName))
                    {
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                errs.Add(new CyCustErr(errMsg));
                            }
                        }
                    }
                }
                return errs;
            }
            #endregion
        }


        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            string instanceName = args.InstQueryV1.GetCommittedParam("INSTANCE_NAME").Value;
            string Sample_Mode = args.InstQueryV1.GetCommittedParam(Cysampleandholdparameters.SAMPLE_MODE).Value;
            string Clock_Edge = args.InstQueryV1.GetCommittedParam(Cysampleandholdparameters.SAMPLE_CLOCK_EDGE).Value;

            CyCustErr err = VerifyInternalClock(args.InstQueryV1.DesignQuery, instanceName, Sample_Mode);;
            yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
        }

        CyCustErr VerifyInternalClock(ICyDesignQuery_v1 designQuery, string instanceName, string Sample_Mode)
        {
            double result = -1;
            try
            {
                if (designQuery.ClockIDs != null)
                {
                    List<string> curClockIDs = new List<string>(designQuery.ClockIDs);
                    double clockfr;
                    byte out_b;
                    string clockID = curClockIDs[0];

                    for (int i = 0; i < curClockIDs.Count; i++)
                    {
                        if (designQuery.GetClockName(curClockIDs[i]) == instanceName + "_aclk")
                        {
                            clockID = curClockIDs[i];
                        }
                    }
                    designQuery.GetClockActualFreq(clockID, out clockfr, out out_b);
                    result = (double)(clockfr * Math.Pow(10, out_b));
                }
            }
            catch
            {
                result = -1;
            }
            return CyCustErr.OK;
        }

        #endregion

        #region ICyExprEval_v2
        // implement ICyExprEval_v2
        CyExprEvalFuncEx ICyExprEval_v2.GetFunction(string name)
        {
            switch (name)
            {
                case "GetVDDA":
                    return new CyExprEvalFuncEx(GetVDDA);
                default:
                    return null;
            }
        }

        // C# function that backs GetMinVDDA expression system function.
        object GetVDDA(string name, object[] fcnArgs, ICyExprEvalArgs_v2 custArgs, ICyExprTypeConverter typeConverter)
        {
            if (custArgs.InstQuery != null)
            {
                if (custArgs.InstQuery.VoltageQuery != null)
                {
                    return custArgs.InstQuery.VoltageQuery.VDDA;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
        #endregion

        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, 
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            Dictionary<string, string> paramDict = null;
            foreach (CyAPICustomizer api in apis)
            {
                // Get parameter dictionary
                paramDict = api.MacroDictionary;
            }

            // Assign dictionary back to API customizers
            foreach (CyAPICustomizer api in apis)
            {
                // Get parameter dictionary
                api.MacroDictionary = paramDict;
            }
            // Return customizers
            return apis;
        }
        #endregion

        
    }
        
}
