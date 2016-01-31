/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace SleepTimer_v3_10
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1
    {
        public const string BASIC_TABNAME = "Basic Configuration";

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CySleepTimerParameters parameters = new CySleepTimerParameters(edit);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate configureExpressionViewDataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GetParams(edit, param);
            };

            editor.AddCustomPage(Resources.BasicTabCaption, new CySleepTimerCustomizer(parameters, edit.DeviceQuery),
                configureExpressionViewDataChanged, BASIC_TABNAME);
            editor.AddDefaultPage(Resources.BuiltInTabCaption, "Built-in");

            parameters.GetParams(edit, null);
            parameters.m_bGlobalEditMode = true;
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

        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            CyCustErr err = VerifyWakeupInterval(args.InstQueryV1);
            if (err.IsOk == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
        }

        CyCustErr VerifyWakeupInterval(ICyInstQuery_v1 instQuery)
        {
            // For PSoC 5 the supported wakeup intervals are restricted to: 4, 8, 16, 32, 64, 128 or 256 ms.
            if (CySleepTimerParameters.IsPSoC5A(instQuery.DeviceQuery))
            {
                string interval = instQuery.GetCommittedParam(CyParamNames.INTERVAL).Expr;

                if (CySleepTimerParameters.IsIntervalAllowedForPSoC5A(interval))
                    return CyCustErr.OK;
                else
                    return new CyCustErr(CySleepTimerCustomizer.GetWakeupIntervalErrorMessage());
            }
            return CyCustErr.OK;
        }
        #endregion
    }
}
