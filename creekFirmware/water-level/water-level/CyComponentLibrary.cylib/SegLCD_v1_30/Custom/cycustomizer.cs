// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================

using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;

namespace SegLCD_v1_30
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        private const bool m_EditParamsOnDrop = false;
        private const CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;
        private CyLCDParameters m_Parameters;

        #region ICyParamEditHook_v1 Members

        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
                                                    ICyExpressMgr_v1 mgr)
        {
            m_Parameters = new CyLCDParameters(edit);
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
                                                     {
                                                         m_Parameters.GetExprViewParams();
                                                     };
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("Basic Configuration", new CyBasicConfigurationControl(m_Parameters), ParamCommitted,
                                 "BasicConfiguration");
            editor.AddCustomPage("Driver Power Settings", new CyDriverPowerSettingsControl(m_Parameters), ParamCommitted,
                                 "");
            editor.AddCustomPage("Display Helpers", new CyHelpersEditingControl(m_Parameters), ParamCommitted, "");
            editor.AddDefaultPage("Built-in", "Built-in");
            m_Parameters.m_GlobalEditMode = true;
            DialogResult result = editor.ShowDialog();
            return result;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get { return m_EditParamsOnDrop; }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return m_Mode;
        }

        #endregion
    }

    public class CyBasicConfigurationControl : ICyParamEditingControl
    {
        private CyBasicConfiguration m_control;

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
            m_control.m_Parameters.SerializedHelpers =
                CyHelperInfo.SerializeHelpers(m_control.m_Parameters.m_HelpersConfig);

            return new CyCustErr[] {}; //return an empty array
        }

        #endregion
    }

    public class CyDriverPowerSettingsControl : ICyParamEditingControl
    {
        private CyDriverParams m_control;

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
            return new CyCustErr[] {}; //return an empty array
        }

        #endregion
    }

    public class CyHelpersEditingControl : ICyParamEditingControl
    {
        private CyHelpers m_control;

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
            m_control.m_Parameters.SerializedHelpers =
                CyHelperInfo.SerializeHelpers(m_control.m_Parameters.m_HelpersConfig);

            List<CyCustErr> errors = new List<CyCustErr>();
            return errors.ToArray();
        }

        #endregion
    }
}