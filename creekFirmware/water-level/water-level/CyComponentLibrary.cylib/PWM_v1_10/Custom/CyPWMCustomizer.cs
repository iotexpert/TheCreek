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
using PWM_v1_10;

namespace PWM_v1_10
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members
        CyPWMEditingControl PWMEditingControl = null;
        CyPWMEditingControlAdv PWMEditingControlAdv = null;
        CyPWMControlAdv m_controladv = null;
        CyPWMControl m_control = null;

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            //Trace.Assert(Debugger.IsAttached);
            m_control = new CyPWMControl(edit, termQuery);
            m_controladv = new CyPWMControlAdv(edit, termQuery, m_control);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            PWMEditingControl = new CyPWMEditingControl(edit, termQuery, m_control);
            PWMEditingControlAdv = new CyPWMEditingControlAdv(edit, termQuery, m_controladv);
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };
            CyParamExprDelegate ExprDelegateAdv = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_controladv.UpdateFormFromParams(edit);
            };
            editor.AddCustomPage("Configure", PWMEditingControl, new CyParamExprDelegate(ExprDelegate),"Basic");            
            editor.AddCustomPage("Advanced", PWMEditingControlAdv, new CyParamExprDelegate(ExprDelegateAdv), "Advanced");
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

        //Create a new control to add to a tab
        public class CyPWMEditingControl : ICyParamEditingControl
        {
            CyPWMControl m_control;
            Panel displayControl = new Panel();

            public CyPWMEditingControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CyPWMControl control)
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

        //Create a new control to add to an advanced tab
        public class CyPWMEditingControlAdv : ICyParamEditingControl
        {
            CyPWMControlAdv m_control;
            Panel displayControl = new Panel();

            public CyPWMEditingControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CyPWMControlAdv control)
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
