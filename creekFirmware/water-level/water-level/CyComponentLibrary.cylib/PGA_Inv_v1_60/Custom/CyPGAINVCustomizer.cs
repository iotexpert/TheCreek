/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace PGA_Inv_v1_60
{

    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyExprEval_v2
    {
        #region ICyParamEditHook_v1 Members
        public ICyInstEdit_v1 m_Component = null;
        CyCompEditingControl CompEditingControl = null;
        CyPgaInvControl m_control = null;

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,ICyExpressMgr_v1 mgr)
        {
            m_control = new CyPgaInvControl(edit);
            m_Component = edit;

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CompEditingControl = new CyCompEditingControl(edit, termQuery, m_control);
            
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
               // m_control.UpdateFormFromParams(edit);
            };
            editor.AddCustomPage("Basic", CompEditingControl, new CyParamExprDelegate(ExprDelegate), "Basic");
            editor.AddDefaultPage("Built-in", "Built-in");
	        editor.UseBigEditor = false;
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


        //Create a new control to add to a tab
        public class CyCompEditingControl : ICyParamEditingControl
        {
            CyPgaInvControl m_control;
            Panel displayControl = new Panel();

            public CyCompEditingControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CyPgaInvControl control)
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
			    if(custArgs.InstQuery != null)
				{
					if(custArgs.InstQuery.VoltageQuery != null)
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

    }
}