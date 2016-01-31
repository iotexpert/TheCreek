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

namespace CRC_v1_10
{
    [CyCompDevCustomizer()]
    partial class  CyCustomizer : ICyParamEditHook_v1
    {
        bool m_EditParamsOnDrop = false;
        CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;
        ICyInstEdit_v1 m_Component = null;
        CyCRCParameters Parameters;

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
        //Debug.Assert(false);
            this.m_Component = edit;

            Parameters = new CyCRCParameters(edit);
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {

            };
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("Polynomial", new CyConfigurationEditingControl(Parameters), null, new string[]{});
            editor.AddDefaultPage("Built-in", "Built-in");
            //editor.AddDefaultPage("Basic", "Basic");
            DialogResult result = editor.ShowDialog();
            
            editor.InterceptHelpRequest = new CyHelpDelegate(InterceptHelp);
            editor.ParamExprCommittedDelegate = ParamCommitted;
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
        CyCRCControl m_control;

        public CyConfigurationEditingControl(CyCRCParameters parameters)
        {
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
            if (m_control.Parameters.m_PolyValue == 0)
            {
                error1 = new CyCustErr("Polynomial value must be greater than zero");
                return new CyCustErr[] { error1 };
            }

            // Error with Seed value
            ulong maxSeed = (((ulong)1 << m_control.Parameters.m_Resolution) - 1);
            if (m_control.Parameters.m_Resolution == 64)
                maxSeed = UInt64.MaxValue;
            if (m_control.Parameters.m_SeedValue > maxSeed)
            {
                error1 = new CyCustErr("The maximum valid Seed value for N=" + m_control.Parameters.m_Resolution.ToString() + " is 0x" + maxSeed.ToString("x"));
                return new CyCustErr[] { error1 }; 
            }
            
            return new CyCustErr[] {}; 
        }

        #endregion
 
    }
}
