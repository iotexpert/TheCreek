/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using System.Diagnostics;
using System.Reflection;

namespace BoostConv_v2_0
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1
    {
        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit);
            CyConfigurationTab configTab = new CyConfigurationTab(parameters);

            CyParamExprDelegate paramDelegate =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                {
                    parameters.LoadParams();
                    configTab.UpdateParam();
                };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(Properties.Resources.TabConfig, configTab, paramDelegate, 
                CyParameters.P_CONFIG_PARAMETERS_TAB_NAME);
            editor.AddDefaultPage(Properties.Resources.TabNameBuildIn, "Built-in");

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

        #region ICyDRCProvider_v1
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            int exClockSrc;
            CyCompDevParam param = args.InstQueryV1.GetCommittedParam(CyParameters.EXTERNAL_CLOCK_SRC);
            if (param.ErrorCount == 0)
            {
                CyCustErr err = param.TryGetValueAs<int>(out exClockSrc);
                Debug.Assert(err.IsOk);
                ICyDeviceQuery_v1 deviceQuery = args.DeviceQueryV1;

                if ((exClockSrc > 0 && (deviceQuery.IsPSoC3 && deviceQuery.SiliconRevision < 2))
                    || (exClockSrc > 0 && (deviceQuery.IsPSoC5 && deviceQuery.SiliconRevision < 2)))
                {
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error,
                        Properties.Resources.DRCMessage_RevisionError);
                }
            }
        }
        #endregion
    }
}
