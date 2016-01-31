/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace EZI2C_v1_50
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1
    {
        public const string HEX_PREFIX = "0x";
        public const string BASIC_TABNAME = "Basic Configuration";

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyEZI2CParameters parameters = new CyEZI2CParameters(edit);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate configureExpressionViewDataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GetParams(edit);
            };

            editor.AddCustomPage(Properties.Resources.BasicTabCaption, new CyEZI2CBasic(parameters),
                configureExpressionViewDataChanged, BASIC_TABNAME);
            editor.AddDefaultPage(Properties.Resources.BuiltInTabCaption, "Built-in");

            parameters.GetParams(edit);

            parameters.m_globalEditMode = true;
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
            List<CyCustErr> err = new List<CyCustErr>();
            err.Add(VerifyRevision(args.DeviceQueryV1, args.InstQueryV1));
            err.Add(VerifyI2CBusPort(args.InstQueryV1));
            for (int i = 0; i < err.Count; i++)
            {
                if (err[i].IsOk == false)
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err[i].Message);
            }
        }

        CyCustErr VerifyRevision(ICyDeviceQuery_v1 deviceQuery, ICyInstQuery_v1 instQuery)
        {
            bool enableWakeup;
            CyCompDevParam param = instQuery.GetCommittedParam(CyParamNames.ENABLE_WAKEUP);
            if (param.ErrorCount == 0)
            {
                CyCustErr err = param.TryGetValueAs<bool>(out enableWakeup);
                Debug.Assert(err.IsOk);

                if ((enableWakeup && (deviceQuery.IsPSoC3 && deviceQuery.SiliconRevision < 2))
                    || (enableWakeup && (deviceQuery.IsPSoC5 && deviceQuery.SiliconRevision < 2)))
                    return new CyCustErr(Properties.Resources.DRCRevisionError);
            }
            return CyCustErr.OK;
        }

        CyCustErr VerifyI2CBusPort(ICyInstQuery_v1 instQuery)
        {
            bool enableWakeup = false;
            string i2cBusPort = string.Empty;

            CyCompDevParam param = instQuery.GetCommittedParam(CyParamNames.ENABLE_WAKEUP);
            if (param.ErrorCount == 0)
            {
                CyCustErr err = param.TryGetValueAs<bool>(out enableWakeup);
                Debug.Assert(err.IsOk);
            }
            param = instQuery.GetCommittedParam(CyParamNames.I2C_BUS_PORT);
            if (param.ErrorCount == 0)
            {
                i2cBusPort = param.Expr;
            }

            if (i2cBusPort == CyEI2CBusPort.Any.ToString() && enableWakeup == true)
                return new CyCustErr(Properties.Resources.DRCI2CBusPortError);
            return CyCustErr.OK; 
        }
        #endregion
    }
}