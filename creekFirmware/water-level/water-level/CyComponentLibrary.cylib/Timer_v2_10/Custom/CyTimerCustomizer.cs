/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace Timer_v2_10
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members
        ICyInstEdit_v1 m_component = null;
        private const string m_builtin = "Built-in";

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyTimerControl m_control = null;
            CyTimerEditingControl m_timerEditingControl = null;
            m_component = edit;
            m_control = new CyTimerControl(edit, termQuery);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            m_timerEditingControl = new CyTimerEditingControl(edit, termQuery, m_control);
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };
            editor.AddCustomPage(CyTimerParameters.S_CONFIGURE, m_timerEditingControl, 
                new CyParamExprDelegate(ExprDelegate), CyTimerParameters.S_BASIC);
            editor.AddDefaultPage(m_builtin, m_builtin);
            //Trace.Assert(Debugger.IsAttached);
            
            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(m_control.UpdateClock));
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
