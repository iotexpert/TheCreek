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

namespace PRS_v1_30
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        bool m_EditParamsOnDrop = false;
        CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;
        ICyInstEdit_v1 m_Component = null;
        CyPRSParameters Parameters;

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
//            Debug.Assert(false);
            this.m_Component = edit;            
            Parameters = new CyPRSParameters(edit);
            Parameters.m_editMode = false;
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                Parameters.GetParams(param);
            };
            
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("General", new CyConfigurationEditingControl(Parameters), ParamCommitted, "General");
            editor.AddDefaultPage("Built-in", "Built-in");            
            editor.ParamExprCommittedDelegate = ParamCommitted;
            Parameters.m_editMode = true;
            DialogResult result = editor.ShowDialog();
            editor.InterceptHelpRequest = new CyHelpDelegate(InterceptHelp);
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

        bool InterceptHelp()
        {
            //Do whatever you want here.
            return true;
        }
    }


    public class CyConfigurationEditingControl : ICyParamEditingControl
    {
        CyPRSControl m_control;

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
            ulong maxSeed = (((ulong)1 << m_control.m_Parameters.m_Resolution) - 1);
            if (m_control.m_Parameters.m_Resolution == 64)
                maxSeed = ulong.MaxValue;

            if (m_control.m_Parameters.m_SeedValue > maxSeed)
            {
                error1 = new CyCustErr("The maximum valid Seed value for Resolution=" +
                    m_control.m_Parameters.m_Resolution.ToString() + " is 0x" + maxSeed.ToString("x"));
                return new CyCustErr[] { error1 };
            }
            if (m_control.m_Parameters.m_SeedValue == 0)
            {
                error1 = new CyCustErr("Seed value must be greater than 0");
                return new CyCustErr[] { error1 };
            }
            return new CyCustErr[] { };   
        }

        #endregion
    }
}
