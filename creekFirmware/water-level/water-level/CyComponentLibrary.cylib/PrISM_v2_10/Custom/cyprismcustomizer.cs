/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace PrISM_v2_10
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {        
        #region ICyParamEditHook_v1 Members 
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            //Debug.Assert(false);
            CyPRISMParameters m_params = new CyPRISMParameters(edit);
            CyPRISMControl m_editControl = new CyPRISMControl(m_params);

            CyParamExprDelegate configureExpressionViewDataChanged =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                {
                    if (new List<CyCustErr>(m_editControl.GetErrors()).Count == 0)
                    {
                        m_params.GetParams(null);
                        m_editControl.UpdateFromParam();
                    }
                };
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(Resource1.TAB_NAME_GENERAL, m_editControl, 
                configureExpressionViewDataChanged, "General");
            editor.AddDefaultPage(Resource1.TAB_NAME_BUILT_IN, "Built-in");
            return editor.ShowDialog();
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
    }
}
