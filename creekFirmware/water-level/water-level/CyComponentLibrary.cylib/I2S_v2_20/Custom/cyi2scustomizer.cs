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
using System.Runtime.InteropServices;

namespace I2S_v2_20
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyI2SParameters parameters = new CyI2SParameters(edit);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate configureExpressionViewDataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GetParams(edit);
            };

            editor.AddCustomPage(Properties.Resources.BasicTabName, 
                new CyI2SBasic(parameters), configureExpressionViewDataChanged, "Basic");
            editor.AddCustomPage(Properties.Resources.AdvancedTabName, 
                new CyI2SAdvanced(parameters), configureExpressionViewDataChanged, "Advanced");
            editor.AddDefaultPage(Properties.Resources.BuiltInTabName, "Built-in");

            parameters.GetParams(edit);

            parameters.m_bGlobalEditMode = true;
            return editor.ShowDialog();
        }

        public bool EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion
    }
}