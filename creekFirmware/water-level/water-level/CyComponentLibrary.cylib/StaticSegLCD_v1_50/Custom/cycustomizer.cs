/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
 
namespace StaticSegLCD_v1_50
{
    [CyCompDevCustomizer]
    public partial class  CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members

        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, 
                                                    ICyExpressMgr_v1 mgr)
        {
            const string TAB_PARAM_BUILTIN = "Built-in";
            CyLCDParameters parameters = new CyLCDParameters(edit);
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
                                                     {
                                                         parameters.GetExprViewParams(param);
                                                     };
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(Properties.Resources.TAB_BASIC_NAME, new CyBasicConfigurationControl(parameters),
                                 ParamCommitted, CyLCDParameters.EXPR_VIEW_PARAM_TABNAME);
            editor.AddCustomPage(Properties.Resources.TAB_HELPERS_NAME, new CyHelpersEditingControl(parameters), null,
                                 "");
            editor.AddDefaultPage(Properties.Resources.TAB_BUILTIN_NAME, TAB_PARAM_BUILTIN);
            DialogResult result = editor.ShowDialog();
            return result;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        #endregion

        public static List<CyCustErr> GetErrors(ICyInstEdit_v1 edit, string tabName)
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            if (edit != null)
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && param.TabName == tabName)
                    {
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                errs.Add(new CyCustErr(errMsg));
                            }
                        }
                    }
                }

            return errs;
        }
    }

    public class CyBasicConfigurationControl : ICyParamEditingControl
    {
        readonly CyBasicConfiguration m_control;

        public CyBasicConfigurationControl(CyLCDParameters parameters)
        {
            m_control = new CyBasicConfiguration(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, CyLCDParameters.EXPR_VIEW_PARAM_TABNAME);
        }

        #endregion
    }

    public class CyHelpersEditingControl : ICyParamEditingControl
    {
        readonly CyHelpers m_control;

        public CyHelpersEditingControl(CyLCDParameters parameters)
        {
            m_control = new CyHelpers(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            m_control.m_parameters.SerializedHelpers =
                CyHelperInfo.SerializeHelpers(m_control.m_parameters.m_helpersConfig,
                                              m_control.m_parameters.m_serializer, 
                                              m_control.m_parameters.m_customSerNamespace);

            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, ""); 
        }

        #endregion
    }
}