/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace I2C_v3_0
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1, ICyAPICustomize_v1
    {
        public const string BASIC_TAB_NAME = "Basic Configuration";

        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            byte clkDiv = 0;
            byte byteDefaultClockDiv = 0;

            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0) paramDict = customizers[0].MacroDictionary;

            clkDiv = CyDividerCalculator.GetES2Divider(query, termQuery);
            byteDefaultClockDiv = CyDividerCalculator.GetES3Divider(query, termQuery);

            paramDict.Add("ClkDiv", clkDiv.ToString());
            paramDict.Add("ClkDiv1", byteDefaultClockDiv.ToString());
            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }
        #endregion

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyI2CParameters parameters = new CyI2CParameters(edit, termQuery);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate configureExpressionViewDataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.LoadParameters();
            };

            editor.AddCustomPage(Resource.TabBasicConfDisplayName, new CyI2CBasic(parameters, edit, termQuery),
                configureExpressionViewDataChanged, BASIC_TAB_NAME);
            editor.AddDefaultPage(Resource.TabBuiltInDisplayName, "Built-in");
            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(parameters.UpdateClock));

            parameters.LoadParameters();

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
            List<CyCustErr> err = new List<CyCustErr>();
            err.Add(VerifyRevision(args.DeviceQueryV1, args.InstQueryV1));
            err.Add(VerifyDataRate(args.DeviceQueryV1, args.InstQueryV1));
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

                if (enableWakeup && (deviceQuery.IsPSoC3 && deviceQuery.SiliconRevision < 2))
                    return new CyCustErr(Resource.DRCRevisionErrorPSoC3ES2);
                else if (enableWakeup && (deviceQuery.IsPSoC5 && deviceQuery.SiliconRevision < 2))
                    return new CyCustErr(Resource.DRCRevisionErrorPSoC5);
            }
            return CyCustErr.OK;
        }

        CyCustErr VerifyDataRate(ICyDeviceQuery_v1 deviceQuery, ICyInstQuery_v1 instQuery)
        {
            int implementation;
            UInt16 dataRate;

            CyCompDevParam paramImplementation = instQuery.GetCommittedParam(CyParamNames.IMPLEMENTATION);
            CyCompDevParam paramDataRate = instQuery.GetCommittedParam(CyParamNames.DATA_RATE);
            if (paramDataRate.ErrorCount == 0 && paramImplementation.ErrorCount == 0)
            {
                CyCustErr err = paramImplementation.TryGetValueAs<int>(out implementation);
                Debug.Assert(err.IsOk);

                err = paramDataRate.TryGetValueAs<UInt16>(out dataRate);
                Debug.Assert(err.IsOk);
                // The speeds are 50, 100, 400 only for FF block of the PSoC3 ES2 and PSoC5 ES1 
                // silicon revisions which did not have the full divider implemented
                
                if (((deviceQuery.IsPSoC3 && deviceQuery.SiliconRevision < 2) 
                    || (deviceQuery.IsPSoC5 && deviceQuery.SiliconRevision < 2))
                    && implementation == (int)CyEImplementationType.FixedFunction
                    && (dataRate != 50 && dataRate != 100 && dataRate != 400))
                    return new CyCustErr(Resource.DRCDataRateInvalid);
            }
            return CyCustErr.OK;
        }
        #endregion
    }
}