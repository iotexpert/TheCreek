/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace Timer_v1_10
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members
        public ICyInstEdit_v1 m_Component = null;
        CyTimerEditingControl TimerEditingControl = null;
        CyTimerControl m_control = null;

        //public System.Windows.Forms.DialogResult EditParams(
        //    ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        //{
        //    m_Component = edit;
        //    ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
        //    editor.AddCustomPage("Configure", new CyTimerEditingControl(edit), null, "Advanced");
        //    editor.AddDefaultPage("Built-in", "Built-in");
        //    DialogResult result = editor.ShowDialog();
        //    return result;
        //}

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            m_control = new CyTimerControl(edit, termQuery);
            m_Component = edit;
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            TimerEditingControl = new CyTimerEditingControl(edit, termQuery, m_control);
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };
            editor.AddCustomPage("Configure", TimerEditingControl, new CyParamExprDelegate(ExprDelegate), "Basic");
            editor.AddDefaultPage("Built-in", "Built-in");
            //Trace.Assert(Debugger.IsAttached);
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
        public class CyTimerEditingControl : ICyParamEditingControl
        {
            CyTimerControl m_control;
            Panel displayControl = new Panel();

            public CyTimerEditingControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CyTimerControl control)
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
