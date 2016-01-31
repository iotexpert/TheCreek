/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation. All rights reserved.
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

namespace Counter_v1_50
{ 
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members
        ICyInstEdit_v1 m_component = null;   
        
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyCounterEditingControl CounterEditingControl = null;
            CyCounterEditingControlAdv CounterEditingControlAdv = null;
            CyCounterControlAdv m_control_adv = null;
            CyCounterControl m_control = null;      
            m_control_adv = new CyCounterControlAdv(edit, termQuery);
            m_control = new CyCounterControl(edit, termQuery, m_control_adv);
            m_component = edit;
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CounterEditingControl = new CyCounterEditingControl(edit, termQuery, m_control);
            CounterEditingControlAdv = new CyCounterEditingControlAdv(edit, termQuery, m_control_adv);
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);                 
            };
            CyParamExprDelegate ExprDelegateAdv = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };
            
            editor.AddCustomPage(CounterParameters.CONFIGURE, CounterEditingControl, 
			                     new CyParamExprDelegate(ExprDelegate), CounterParameters.BASIC);
            editor.AddCustomPage(CounterParameters.ADVANCED, CounterEditingControlAdv, 
			                     new CyParamExprDelegate(ExprDelegateAdv), CounterParameters.ADVANCED);
            editor.AddDefaultPage(CounterParameters.BUILT_IN, CounterParameters.BUILT_IN);
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
        public class CyCounterEditingControlAdv : ICyParamEditingControl
        {
            CyCounterControlAdv m_control;
            Panel displayControl = new Panel();

            public CyCounterEditingControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, 
			CyCounterControlAdv control)
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
                get { return m_control; }
            }

            public IEnumerable<CyCustErr> GetErrors()
            {
                return new CyCustErr[] { };    //return an empty array
            }

            #endregion
        }

        //Create a new control to add to a tab
        public class CyCounterEditingControl : ICyParamEditingControl
        {
            CyCounterControl m_control;
            Panel displayControl = new Panel();

            public CyCounterEditingControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CyCounterControl control)
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
