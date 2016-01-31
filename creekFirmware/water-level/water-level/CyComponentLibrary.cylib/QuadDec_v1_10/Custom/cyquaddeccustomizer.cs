/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace QuadDec_v1_10
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        private bool m_editParamsOnDrop = false;
        private CyCompDevParamEditorMode m_mode = CyCompDevParamEditorMode.COMPLETE;

        private CyTab1 m_tab1;
        private CyTab2 m_tab2;
        private CyTab3 m_tab3;
        private CyTab4 m_tab4;

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit);
            m_tab1 = new CyTab1(parameters);
            m_tab2 = new CyTab2(parameters);
            m_tab3 = new CyTab3(parameters);
            m_tab4 = new CyTab4(parameters);

            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param) {};

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("Counter Size", m_tab1, ParamCommitted,"CounterSize");
            editor.AddCustomPage("Counter Resolution", m_tab2, ParamCommitted, "CounterResolution");
            editor.AddCustomPage("Use Index Input", m_tab3, ParamCommitted, "UseIndexInput");
            editor.AddCustomPage("Enable Glitch Filtering", m_tab4, ParamCommitted, "EnableGlitchFiltering");
            editor.ParamExprCommittedDelegate = ParamCommitted;

            return editor.ShowDialog();
        }

        public bool EditParamsOnDrop
        {
            get { return m_editParamsOnDrop; }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return m_mode;
        }

        #endregion
        
    }
}
