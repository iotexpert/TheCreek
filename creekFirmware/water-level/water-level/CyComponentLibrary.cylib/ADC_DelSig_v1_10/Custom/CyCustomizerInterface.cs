//*******************************************************************************
// File Name: CyCustomizerInterface.cs
/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
using System;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using ADC_DelSig_v1_10;

namespace ADC_DelSig_v1_10
{
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyExprEval_v1
    {
        private bool m_editParamOnDrop = false;

        #region ICyParamEditHook_v1 Members

        System.Windows.Forms.DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();  
            ADC_EditingControl control = new ADC_EditingControl(edit);
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                control.UpdateFormFromParams();
            };

            editor.AddCustomPage("Configure", new ADC_EditingControl(edit), new CyParamExprDelegate(ExprDelegate), "Basic");
            editor.AddDefaultPage("Built-in", "Built-in");
            DialogResult result = editor.ShowDialog();
            
            // Place calculations here *********************************************************************************************
            
            
        
            edit.CommitParamExprs();

           
     
            // Show all parameter values
            //string temp = "";
            //foreach (string s in edit.GetParamNames())
            //{
            //    temp += edit.GetCommittedParam(s).Name + " = " + edit.GetCommittedParam(s).Value + Environment.NewLine;
            //}

            //MessageBox.Show(temp);


            return result;
          //  return DialogResult.OK;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return m_editParamOnDrop;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

      

        #endregion



        #region ICyExprEval_v1 Members

        CyExprEvalFunc ICyExprEval_v1.GetFunction(string exprFuncName)
        {
            switch (exprFuncName)
            {
                case "AdcClockFrequency":

                    return new CyExprEvalFunc(AdcClockFrequency);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculate the ADC clock frequency as a expression evaluator method.
        /// </summary>
        /// <param name="exprFuncName">Name of the function.</param>
        /// <param name="args">arguments to the function</param>
        /// <param name="typeConverter">Converts types to strings</param>
        /// <returns></returns>
        object AdcClockFrequency(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            if (args.Length != /* How many args do you have? */ 3)
            {
                // TODO :new CyCustErr("The method "+ exprFuncName + " requires __ paramters");
                return 30720000;
            }
            else
            {
             
                uint resolution = typeConverter.GetAsUInt(args[0]);
                uint sampleRate = typeConverter.GetAsUInt(args[1]);
                uint conversionMode = typeConverter.GetAsUInt(args[2]);

                // Call the actual frequency with the given parameters
                uint theFreq = SetClock(resolution, sampleRate, conversionMode);
                return theFreq;
            }
        }

        #endregion
    }


    public class ADC_EditingControl : ICyParamEditingControl
    {
        CyADC_DelSigControl1 ADC_Control;

        public ADC_EditingControl(ICyInstEdit_v1 inst)
        {
            ADC_Control = new CyADC_DelSigControl1(inst);
            ADC_Control.Dock = DockStyle.Fill;
            ADC_Control.AutoScroll = true;
        }

        public void UpdateFormFromParams()
        {
        }

        #region ICyParamEditingControl Members

        Control ICyParamEditingControl.DisplayControl
        {
            get
            {
                return ADC_Control;
            }
        }

        System.Collections.Generic.IEnumerable<CyCustErr> ICyParamEditingControl.GetErrors()
        {
            return new System.Collections.Generic.List<CyCustErr>();
        }

        #endregion
    }

}