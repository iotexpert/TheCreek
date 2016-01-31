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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace StaticSegLCD_v1_30
{
    [CyCompDevCustomizer()]
    public partial class  CyCustomizer : ICyParamEditHook_v1
    {
	
        private const bool m_EditParamsOnDrop = false;
        private const CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;
        ICyInstEdit_v1 m_Component = null;
        CyLCDParameters Parameters;
        
        #region ICyParamEditHook_v1 Members

        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            this.m_Component = edit;

            Parameters = new CyLCDParameters(edit);
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
                                                     {
                                                     };
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("Basic Configuration", new CyBasicConfigurationControl(Parameters), ParamCommitted, "");
           // editor.AddCustomPage("Driver Power Settings", new CyDriverPowerSettingsControl(Parameters), ParamCommitted, "");
            editor.AddCustomPage("Display Helpers", new CyHelpersEditingControl(Parameters), ParamCommitted, "");
            editor.AddDefaultPage("Built-in", "Built-in");
            DialogResult result = editor.ShowDialog();
            editor.InterceptHelpRequest = new CyHelpDelegate(InterceptHelp);
            return result;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return m_EditParamsOnDrop;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return m_Mode;
        }

        bool InterceptHelp()
        {
            return true;
        }

        #endregion
    }

    public class CyBasicConfigurationControl : ICyParamEditingControl
    {
        CyBasicConfiguration m_control;

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
            if (m_control.m_ParametersChanged)
            {
                m_control.m_Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(m_control.m_Parameters.m_HelpersConfig);
                m_control.m_ParametersChanged = false;
            }

            return new CyCustErr[] { };    //return an empty array
        }

        #endregion
    }

    public class CyHelpersEditingControl : ICyParamEditingControl
    {
        CyHelpers m_control;

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
            m_control.m_Parameters.SerializedHelpers = HelperInfo.SerializeHelpers(m_control.m_Parameters.m_HelpersConfig);

            List<CyCustErr> errors = new List<CyCustErr>( );

            //Check if every pixel is Assigned
            //CyCustErr err1;
            //for (int i = 0; i < m_control.m_Parameters.m_HelpersConfig.Count; i++)
            //    for (int j = 0; j < m_control.m_Parameters.m_HelpersConfig[i].HelpSegInfo.Count; j++)
            //    {
            //        if ((m_control.m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Common < 0) ||
            //            (m_control.m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Segment < 0))
            //        {
            //            err1 = new CyCustErr(@"Helper pixel " + m_control.m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Name + @" must be assigned to the common and segment lines.");
            //            errors.Add(err1);
            //        }
            //    }

            return errors.ToArray();    
        }

        #endregion
    }
}