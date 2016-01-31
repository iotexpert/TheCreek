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

namespace I2C_v2_10
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1, ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            UInt32 busSpeed = 0;
            byte oversampleRate = 16;
            byte clkDiv = 0;
            CyEModeType mode = new CyEModeType();

            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0) paramDict = customizers[0].MacroDictionary;

            double busClock = CyI2CParameters.GetBusClockInKHz(query.DesignQuery);
            UInt32.TryParse(query.GetCommittedParam(CyParamNames.BUS_SPEED_KHZ).Value, out busSpeed);
            try
            {
                mode = (CyEModeType)byte.Parse(query.GetCommittedParam(CyParamNames.I2C_MODE).Value);
            }
            catch (Exception) { }
            if (busSpeed <= 50) oversampleRate = 32;
            else oversampleRate = 16;
            double defaultClockDiv = (double)(busClock / (busSpeed * oversampleRate));


            switch (mode)
            {
                case CyEModeType.Slave_revA:
                    defaultClockDiv = Math.Round(defaultClockDiv);
                    break;
                case CyEModeType.Master_revA:
                case CyEModeType.MultiMaster_revA:
                case CyEModeType.MultiMaster_Slave_revA:
                default:
                    break;
            }

            UInt16 uint16DefaultClockDiv = (UInt16)defaultClockDiv;
            byte byteDefaultClockDiv = (byte)defaultClockDiv;

            for (int clkSel = 0; clkSel <= 6; clkSel++)
            {
                if ((1 << clkSel) >= byteDefaultClockDiv)
                {
                    clkDiv = (byte)clkSel;
                    break;
                }
            }

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
                parameters.GetParams(edit);
            };

            editor.AddCustomPage("Basic Configuration", new CyI2CBasic(parameters),
                configureExpressionViewDataChanged, "Basic Configuration");
            editor.AddDefaultPage("Built-in", "Built-in");
            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(parameters.UpdateClock));

            parameters.GetParams(edit);

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
                    return new CyCustErr(Resource01.DRCRevisionError);
            }
            return CyCustErr.OK;
        }
        #endregion
    }
}