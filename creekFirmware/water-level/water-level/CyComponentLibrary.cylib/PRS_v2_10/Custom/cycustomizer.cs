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

namespace PRS_v2_10
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        private const string PAGE_TITLE_GENERAL = "General";
        private const string PAGE_TITLE_BUILTIN = "Built-in";
        private const string PAGE_TITLE_ADVANCED = "Advanced";

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyPRSParameters m_parameters = new CyPRSParameters(edit);
            CyConfigurationEditingControl m_genCtrl = new CyConfigurationEditingControl(m_parameters);
            CyAdvancedEditingControl m_advControl = new CyAdvancedEditingControl(m_parameters);
            m_parameters.m_editMode = false;
            CyParamExprDelegate ExpView_ParamChanged =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                    {
                        m_parameters.GetParams(param.Name);
                        if (param.TabName == PAGE_TITLE_GENERAL)
                            ((CyPRSControl) (m_genCtrl.DisplayControl)).UpdateForm();
                        else if (param.TabName == PAGE_TITLE_ADVANCED)
                            ((CyAdvancedControl) (m_advControl.DisplayControl)).UpdateForm();
                    };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(PAGE_TITLE_GENERAL, m_genCtrl, ExpView_ParamChanged, PAGE_TITLE_GENERAL);
            editor.AddCustomPage(PAGE_TITLE_ADVANCED, m_advControl, ExpView_ParamChanged, PAGE_TITLE_ADVANCED);
            editor.AddDefaultPage(PAGE_TITLE_BUILTIN, PAGE_TITLE_BUILTIN); 

            m_parameters.m_editMode = true;
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

    public class CyConfigurationEditingControl : ICyParamEditingControl
    {
        private readonly CyPRSControl m_control;

        public CyConfigurationEditingControl(CyPRSParameters parameters)
        {
            m_control = new CyPRSControl(parameters);
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
            ulong maxSeed = (((ulong)1 << (int)m_control.m_parameters.m_resolution) - 1);
            if (m_control.m_parameters.m_resolution == 64)
                maxSeed = ulong.MaxValue;

            if (m_control.m_parameters.SeedValue > maxSeed)
            {
                error1 = new CyCustErr("The maximum valid Seed value for Resolution=" +
                    m_control.m_parameters.m_resolution.ToString() + " is 0x" + maxSeed.ToString("x"));
                return new CyCustErr[] { error1 };
            }
            if (m_control.m_parameters.SeedValue == 0)
            {
                error1 = new CyCustErr("Seed value must be greater than 0");
                return new CyCustErr[] { error1 };
            }
            return new CyCustErr[] { };   
        }

        #endregion
    }

    public class CyAdvancedEditingControl : ICyParamEditingControl
    {
        private readonly CyAdvancedControl m_control;

        public CyAdvancedEditingControl(CyPRSParameters parameters)
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
