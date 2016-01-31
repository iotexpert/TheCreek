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

namespace CRC_v2_0
{
    [CyCompDevCustomizer()]
    partial class CyCustomizer : ICyParamEditHook_v1
    {
        private const string PARAM_TAB_POLYNOMIAL = "General";
        private const string PARAM_TAB_ADVANCED = "Advanced";

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyCRCParameters parameters = new CyCRCParameters(edit);
            CyConfigurationEditingControl m_editControl = new CyConfigurationEditingControl(parameters);
            CyAdvancedEditingControl m_advControl = new CyAdvancedEditingControl(parameters);

            CyParamExprDelegate ExpView_ParamChanged =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                    {
                        parameters.m_exprViewParam = param;
                        parameters.GetExprParams(param);
                        if (param.TabName == PARAM_TAB_POLYNOMIAL)
                            ((CyCRCControl) (m_editControl.DisplayControl)).UpdateControls();
                        else if (param.TabName == PARAM_TAB_ADVANCED)
                            ((CyAdvancedControl)(m_advControl.DisplayControl)).UpdateForm();
                        parameters.m_exprViewParam = null;
                    };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_POLYNOMIAL, m_editControl, ExpView_ParamChanged,
                                 PARAM_TAB_POLYNOMIAL);
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_ADVANCED, m_advControl, ExpView_ParamChanged,
                                 PARAM_TAB_ADVANCED);
            editor.AddDefaultPage(Properties.Resources.PAGE_TITLE_BUILTIN, "Built-in");
            DialogResult result = editor.ShowDialog();
            return result;
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


    public class CyConfigurationEditingControl : ICyParamEditingControl
    {
        readonly CyCRCControl m_control;

        public CyConfigurationEditingControl(CyCRCParameters parameters)
        {
            Debug.Assert(parameters != null);
            
            m_control = new CyCRCControl(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            CyCustErr error1 = null;

            // Error with Zero Polynomial value
            if (m_control.m_Parameters.PolyValue == 0)
            {
                error1 = new CyCustErr("Polynomial value must be greater than zero");
                return new CyCustErr[] { error1 };
            }

            // Error with Seed value
            UInt64 maxSeed = m_control.GetMaxSeedValue();
            if (m_control.m_Parameters.SeedValue > maxSeed)
            {
                error1 = new CyCustErr("The maximum valid Seed value for N=" + m_control.m_Parameters.Resolution.
                    ToString() + " is 0x" + maxSeed.ToString("X"));
                return new CyCustErr[] { error1 }; 
            }
            
            return new CyCustErr[] {}; 
        }

        #endregion
 
    }

    public class CyAdvancedEditingControl : ICyParamEditingControl
    {
        private readonly CyAdvancedControl m_control;

        public CyAdvancedEditingControl(CyCRCParameters parameters)
        {
            m_control = new CyAdvancedControl(parameters);
            m_control.Dock = DockStyle.Fill;
            parameters.m_advancedPage = m_control;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errors = new List<CyCustErr>();
            // TimeMultiplexing
            string err = m_control.ValidateTimeMultiplexing();
            if (!String.IsNullOrEmpty(err))
                errors.Add(new CyCustErr(err));

            return errors.ToArray();
        }

        #endregion
    }
}
