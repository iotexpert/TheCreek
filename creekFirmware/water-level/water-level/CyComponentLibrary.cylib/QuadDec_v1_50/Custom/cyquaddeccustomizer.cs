/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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
using System.IO;

namespace QuadDec_v1_50
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyAPICustomize_v1
    {
        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters m_parameters = new CyParameters(edit);
            CyTab1 m_tab1 = new CyTab1(m_parameters);
            CyTab2 m_tab2 = new CyTab2(m_parameters);
            CyTab3 m_tab3 = new CyTab3(m_parameters);
            CyTab4 m_tab4 = new CyTab4(m_parameters);

            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_parameters.LoadParam();
                m_tab1.UpdateFromParam();
                m_tab2.UpdateFromParam();
                m_tab3.UpdateFromParam();
                m_tab4.UpdateFromParam();
            };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("Counter Size", m_tab1, ParamCommitted, "CounterSize");
            editor.AddCustomPage("Counter Resolution", m_tab2, ParamCommitted, "CounterResolution");
            editor.AddCustomPage("Use Index Input", m_tab3, ParamCommitted, "UseIndexInput");
            editor.AddCustomPage("Enable Glitch Filtering", m_tab4, ParamCommitted, "EnableGlitchFiltering");
            editor.ParamExprCommittedDelegate = ParamCommitted;

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

        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(
            ICyInstQuery_v1 query,
            ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            CyAPICustomizer file = null;
            for (int i = 0; i < customizers.Count; i++)
            {
                if (customizers[i].OriginalName.EndsWith("QuadDec_INT.c"))
                {
                    file = customizers[i];
                }
            }

            byte m_bit = Convert.ToByte(query.GetCommittedParam(CyParameters.COUNTER_SIZE).Value);

            if (m_bit < 32)
                if (file != null)
                    customizers.Remove(file);
            return customizers;
        }
        #endregion
    }
}
