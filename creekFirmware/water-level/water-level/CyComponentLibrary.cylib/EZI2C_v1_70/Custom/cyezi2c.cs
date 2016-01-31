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

namespace EZI2C_v1_70
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

            editor.AddCustomPage(Resources.BasicTabCaption, new CyEZI2CBasic(parameters, edit.DeviceQuery),
                configureExpressionViewDataChanged, BASIC_TABNAME);
            editor.AddDefaultPage(Resources.BuiltInTabCaption, "Built-in");

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
            err.AddRange(VerifyRevision(args.InstQueryV1));
            err.Add(VerifyI2CBusPort(args.InstQueryV1));
            err.Add(VerifyNumberOfAddresses(args.InstQueryV1));
            err.Add(VerifyEnableWakeup(args.InstQueryV1));
            for (int i = 0; i < err.Count; i++)
            {
                if (err[i].IsOk == false)
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err[i].Message);
            }
        }

        List<CyCustErr> VerifyRevision(ICyInstQuery_v1 instQuery)
        {
            bool enableWakeup;
            instQuery.GetCommittedParam(CyParamNames.ENABLE_WAKEUP).TryGetValueAs<bool>(out enableWakeup);

            CyEI2CBusPort busPort = CyEI2CBusPort.Any;
            try
            {
                busPort = (CyEI2CBusPort)Convert.ToByte(instQuery.GetCommittedParam(CyParamNames.I2C_BUS_PORT).Value);
            }
            catch (Exception)
            {
                Debug.Assert(false);
            }
            List<CyCustErr> list = new List<CyCustErr>();
            if (enableWakeup && IsPSoC5A(instQuery.DeviceQuery))
            {
                list.Add(new CyCustErr(Resources.DRCRevisionError));
            }
            if (busPort != CyEI2CBusPort.Any && IsPSoC5A(instQuery.DeviceQuery))
            {
                list.Add(new CyCustErr(Resources.PinConnectionsPSoC5AEP));
            }
            if (list.Count == 0)
                list.Add(CyCustErr.OK);

            return list;
        }

        CyCustErr VerifyI2CBusPort(ICyInstQuery_v1 instQuery)
        {
            if (IsPSoC5A(instQuery.DeviceQuery) == false)
            {
                int numberOfAddresses;
                CyEI2CBusPort busPort;
                bool enableWakeup;
                UpdateParametersForDRC(instQuery, out numberOfAddresses, out busPort, out enableWakeup);

                if (numberOfAddresses == 1 && busPort == CyEI2CBusPort.Any && enableWakeup == true)
                {
                    return new CyCustErr(Resources.PinConnectionsDRC1);
                }
                else if (busPort != CyEI2CBusPort.Any && enableWakeup == false)
                {
                    return new CyCustErr(Resources.PinConnectionsEPAndDRC2);
                }
            }
            return CyCustErr.OK;
        }

        CyCustErr VerifyNumberOfAddresses(ICyInstQuery_v1 instQuery)
        {
            if (IsPSoC5A(instQuery.DeviceQuery) == false)
            {
                int numberOfAddresses;
                CyEI2CBusPort busPort;
                bool enableWakeup;
                UpdateParametersForDRC(instQuery, out numberOfAddresses, out busPort, out enableWakeup);

                if (numberOfAddresses == 2 && busPort != CyEI2CBusPort.Any && enableWakeup == true)
                {
                    return new CyCustErr(Resources.NumberOfAddressesDRC1);
                }
            }
            return CyCustErr.OK;
        }

        CyCustErr VerifyEnableWakeup(ICyInstQuery_v1 instQuery)
        {
            if (IsPSoC5A(instQuery.DeviceQuery) == false)
            {
                int numberOfAddresses;
                CyEI2CBusPort busPort;
                bool enableWakeup;
                UpdateParametersForDRC(instQuery, out numberOfAddresses, out busPort, out enableWakeup);

                if (numberOfAddresses == 2 && busPort == CyEI2CBusPort.Any && enableWakeup == true)
                {
                    return new CyCustErr(Resources.EnableWakupDRC);
                }
            }
            return CyCustErr.OK;
        }

        private bool IsPSoC5A(ICyDeviceQuery_v1 deviceQuery)
        {
            if (deviceQuery.ArchFamilyMember == "PSoC5A")
                return true;
            else
                return false;
        }

        private static void UpdateParametersForDRC(ICyInstQuery_v1 instQuery, out int numberOfAddresses,
            out CyEI2CBusPort busPort, out bool enableWakeup)
        {
            int.TryParse(instQuery.GetCommittedParam(CyParamNames.I2C_ADDRESSES).Value, out numberOfAddresses);

            busPort = CyEI2CBusPort.Any;
            try
            {
                busPort = (CyEI2CBusPort)Convert.ToByte(instQuery.GetCommittedParam(CyParamNames.I2C_BUS_PORT)
                    .Value);
            }
            catch (Exception)
            {
                Debug.Assert(false);
            }

            instQuery.GetCommittedParam(CyParamNames.ENABLE_WAKEUP).TryGetValueAs<bool>(out enableWakeup);
        }
        #endregion
    }
}
