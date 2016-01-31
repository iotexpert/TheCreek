/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using System.Diagnostics;

namespace BoostConv_v1_10
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            ICyParamEditingControl confTab = new CyConfigurationTab(parameters);
            CyParamExprDelegate paramExprCh = new CyParamExprDelegate(paramExprChanged);
            editor.AddCustomPage("Boost convertor configuration", confTab, paramExprCh, "");
            editor.AddDefaultPage("Built-in", "Built-in");
            editor.ParamExprCommittedDelegate = paramExprCh;
            paramExprChanged(null,null);
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

        private void paramExprChanged(ICyParamEditor custEditor, CyCompDevParam param)
        {
        }
    }
}
