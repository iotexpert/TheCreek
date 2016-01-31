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
using SPI_Slave_v1_20;

namespace SPI_Slave_v1_20
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members
        public ICyInstEdit_v1 m_Component = null;
        CySPISEditingControl SPISEditingControl = null;
        CySPISEditingControlAdv SPISEditingControlAdv = null;
        CySPISControlAdv m_controladv = null;
        CySPISControl m_control = null;

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            m_control = new CySPISControl(edit, termQuery);
            m_controladv = new CySPISControlAdv(edit, termQuery);
            m_Component = edit;
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            SPISEditingControl = new CySPISEditingControl(edit, termQuery, m_control);
            SPISEditingControlAdv = new CySPISEditingControlAdv(edit, termQuery, m_controladv);
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };
            CyParamExprDelegate ExprDelegateAdv = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_controladv.UpdateFormFromParams(edit);
            };
            editor.AddCustomPage("Configure", SPISEditingControl, new CyParamExprDelegate(ExprDelegate),"Basic");            
            editor.AddCustomPage("Advanced", SPISEditingControlAdv, new CyParamExprDelegate(ExprDelegateAdv), "Advanced");
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
        public class CySPISEditingControl : ICyParamEditingControl
        {
            CySPISControl m_control;
            Panel displayControl = new Panel();

            public CySPISEditingControl(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CySPISControl control)
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
        public class CySPISEditingControlAdv : ICyParamEditingControl
        {
            CySPISControlAdv m_control;
            Panel displayControl = new Panel();

            public CySPISEditingControlAdv(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termquery, CySPISControlAdv control)
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
