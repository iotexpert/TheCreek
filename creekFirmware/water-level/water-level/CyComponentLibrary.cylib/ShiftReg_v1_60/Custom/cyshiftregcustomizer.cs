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


namespace ShiftReg_v1_60
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        bool m_EditParamsOnDrop = false;
        CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;
        ICyInstEdit_v1 m_Component = null;
        CySRParameters parameters;

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            //Debug.Assert(false);
            CySRParameters.GLOBAL_EDIT_MODE = false;
            this.m_Component = edit;
            parameters = new CySRParameters(edit);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CyParamExprDelegate configureExpressionViewDataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                if (param.ErrorCount == 0)
                    parameters.GetParams(edit, param);
            };
            editor.AddCustomPage(Resource1.TabGeneral, parameters.m_generalParamsTab = new CyGeneralParamsTab()
                , configureExpressionViewDataChanged, CySRParameters.P_GENERAL_PARMETERS_TAB_NAME);
            editor.AddDefaultPage(Resource1.TabBuiltIn, "Built-in");

            parameters.GetParams(edit, null);
            DialogResult result = editor.ShowDialog();
            return result;
        }
        public bool EditParamsOnDrop
        {
            get
            {
                return m_EditParamsOnDrop;
            }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return m_Mode;
        }

        #endregion
    }


}


