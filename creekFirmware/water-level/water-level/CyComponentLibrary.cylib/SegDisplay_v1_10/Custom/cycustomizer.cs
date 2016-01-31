/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SegDisplay_v1_10
{
    [CyCompDevCustomizer]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        public const string TABNAME_EXPRVIEW_BASIC = "BasicConfiguration";
        public const string TABNAME_PARAM_BUILTIN = "Built-in";

        #region ICyParamEditHook_v1 Members

        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
                                                    ICyExpressMgr_v1 mgr)
        {
            CyLCDParameters  m_parameters = new CyLCDParameters(edit);
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
                                                     {
                                                         m_parameters.GetExprViewParams();
                                                     };
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_BASIC, new CyBasicConfigurationControl(m_parameters),
                                 ParamCommitted, TABNAME_EXPRVIEW_BASIC);
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_POWERSETTINGS,
                                 new CyDriverPowerSettingsControl(m_parameters), ParamCommitted, "");
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_HELPERS, new CyHelpersEditingControl(m_parameters),
                                 ParamCommitted, "");
            editor.AddDefaultPage(Properties.Resources.PAGE_TITLE_BUILTIN, TABNAME_PARAM_BUILTIN);
            m_parameters.m_globalEditMode = true;
            DialogResult result = editor.ShowDialog();
            return result;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get { return false; }
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
        private readonly CyBasicConfiguration m_control;

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
            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, CyCustomizer.TABNAME_EXPRVIEW_BASIC);
        }

        #endregion
    }

    public class CyDriverPowerSettingsControl : ICyParamEditingControl
    {
        private readonly CyDriverParams m_control;

        public CyDriverPowerSettingsControl(CyLCDParameters parameters)
        {
            m_control = new CyDriverParams(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, "");
        }

        #endregion
    }

    public class CyHelpersEditingControl : ICyParamEditingControl
    {
        private readonly CyHelpers m_control;

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
