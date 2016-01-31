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

namespace SPI_Master_v2_0
{    
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1
    {
        public const string BASIC_TABNAME = "Basic";
        public const string ADVANCED_TABNAME = "Advanced";

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CySPIMParameters parameters = new CySPIMParameters(edit);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate configureExpressionViewDataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GetParams(edit);
            };
            editor.AddCustomPage(Properties.Resources.BasicTabTitle, new CySPIMControl(parameters),
                configureExpressionViewDataChanged, BASIC_TABNAME);
            editor.AddCustomPage(Properties.Resources.AdvancedTabTitle, new CySPIMControlAdv(parameters),
                configureExpressionViewDataChanged, ADVANCED_TABNAME);
            editor.AddDefaultPage(Properties.Resources.BuiltInTabTitle, "Built-in");
            parameters.GetParams(edit);
            parameters.m_bGlobalEditMode = true;
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
