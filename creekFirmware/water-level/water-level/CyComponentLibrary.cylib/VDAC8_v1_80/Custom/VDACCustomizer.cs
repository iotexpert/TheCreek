/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace VDAC8_v1_80
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyExprEval_v2, ICyDRCProvider_v1
    {
        #region ICyParamEditHook_v1 Members
        public ICyInstEdit_v1 m_component = null;
        CyCompEditingControl CompEditingControl = null;
        CyVDACControl m_control = null;
        double range_value;
        const string VoltageRangeNote = @"System VDDA voltage is set less than the voltage range of
        VDAC, the actual range of the VDAC will be 0V to VDDA.";

        //Adds custom page to built in page.
        //Delegate is called to update the custom view when something is changed
        //in the expression view
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            m_control = new CyVDACControl(edit);
            m_component = edit;
                        
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CompEditingControl = new CyCompEditingControl(edit, termQuery, m_control);
                        
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };
            
            editor.AddCustomPage("Configure", CompEditingControl, new CyParamExprDelegate(ExprDelegate), "Basic");
            editor.AddDefaultPage("Built-in", "Built-in");
            DialogResult result = editor.ShowDialog();
            return result;
        }

        
        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        #endregion
        //Generates DRC error when user selects VDAC range more than VDDA 
        //specified  in the dwr file.
        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {            
            string instanceName = args.InstQueryV1.GetCommittedParam("INSTANCE_NAME").Value;
            string param = args.InstQueryV1.GetCommittedParam("VDAC_Range").Value;
                    
            CyCustErr err = VerifyVoltage(args.InstQueryV1.VoltageQuery, instanceName, param);
            if (err.IsOk == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Info, err.Message);
        }
        //Verifies the VDDA with selected VDAC voltage range
        CyCustErr VerifyVoltage(ICyVoltageQuery_v1 voltage, string instancename, string param)
        {            
            Int16 VoltageRange = Int16.Parse(param);
            
            Int16 VDDA = Convert.ToInt16(voltage.VDDA);
             if (VoltageRange == 0)
                 range_value = 1.020;
            else
                 range_value = 4.080;            
            try
            {                
                    if (range_value > VDDA)
                    {                        
                        return new CyCustErr(VoltageRangeNote);
                    }                
            }
            catch
            {
                
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
                    return -1;
            }
            else
                return -1;
        }
        #endregion

        public class CyCompEditingControl : ICyParamEditingControl
        {
            CyVDACControl m_control;
            Panel displayControl = new Panel();

            public CyCompEditingControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CyVDACControl control)
            {
                m_control = control;
                displayControl.Dock = DockStyle.Fill;
                displayControl.AutoScroll = true;
                displayControl.AutoScrollMinSize = m_control.Size;

                m_control.Dock = DockStyle.Fill;
                displayControl.Controls.Add(m_control);
            }

            #region ICyParamEditingControl Members
            public Control DisplayControl
            {
                get { return displayControl; }
            }

            public IEnumerable<CyCustErr> GetErrors()
            {
                return new CyCustErr[] { };    //return an empty array
            }

            #endregion
        }
    }
}


 //[] END OF FILE 