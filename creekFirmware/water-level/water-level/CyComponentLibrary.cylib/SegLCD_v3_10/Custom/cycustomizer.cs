/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;

namespace SegLCD_v3_10
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1
    {
        public const string PAGE_PARAM_BASIC = "BasicConfiguration";
        private const string PAGE_PARAM_BUILTIN = "Built-in";

        #region ICyParamEditHook_v1 Members

        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
                                                    ICyExpressMgr_v1 mgr)
        {
            CyLCDParameters parameters = new CyLCDParameters(edit);
            CyParamExprDelegate ParamCommitted = delegate { parameters.GetExprViewParams(); };
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_BASIC, new CyBasicConfigurationControl(parameters),
                                 ParamCommitted, PAGE_PARAM_BASIC);
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_POWERSETTINGS,
                                 new CyDriverPowerSettingsControl(parameters), ParamCommitted, "");
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_HELPERS, new CyHelpersEditingControl(parameters),
                                 ParamCommitted, "");
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_CUSTOMCHARS, new CyCustomCharsEditor(parameters),
                                 ParamCommitted, "");
            editor.AddDefaultPage(Properties.Resources.PAGE_TITLE_BUILTIN, PAGE_PARAM_BUILTIN);
            parameters.m_globalEditMode = true;
            DialogResult result = editor.ShowDialog();
            return result;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get { return false; }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        #endregion

        public static List<CyCustErr> GetErrors(ICyInstEdit_v1 edit, string tabName)
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            if (edit != null)
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && param.TabName == tabName)
                    {
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                errs.Add(new CyCustErr(errMsg));
                            }
                        }
                    }
                }

            return errs;
        }

        #region ICyDRCProvider_v1 Members

        IEnumerable<CyDRCInfo_v1> ICyDRCProvider_v1.GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            // Check clock enabled and clock frequency
            ICyInstQuery_v1 instQuery = args.InstQueryV1;
            byte m_driverPowerMode =
                Convert.ToByte(instQuery.GetCommittedParam(CyLCDParameters.PARAM_DRIVERPOWERMODE).Value);

            foreach (string item in args.InstQueryV1.DesignQuery.ClockIDs)
            {
                if (args.InstQueryV1.DesignQuery.GetClockName(item) == "XTAL_32KHZ")
                {
                    if (m_driverPowerMode == (byte)CyBasicConfiguration.CyMode.LOW_POWER_32K)
                    {
                        bool isXTALClockEnabled = args.InstQueryV1.DesignQuery.IsClockStartedOnReset(item);
                        if (isXTALClockEnabled == false)
                            yield return
                               new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Properties.Resources.CLOCK_XTAL_ERROR);
                    }
                }
                if (args.InstQueryV1.DesignQuery.GetClockName(item) == "ILO")
                {
                    double freq;
                    byte ext;
                    args.InstQueryV1.DesignQuery.GetClockActualFreq(item, out freq, out ext);
                    if (m_driverPowerMode == (byte)CyBasicConfiguration.CyMode.LOW_POWER_ILO)
                    {
                        if ((freq > 1.0001) || (0.9999 > freq))
                            yield return
                                new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error,
                                                 Properties.Resources.CLOCK_ILO_ERROR_1KHZ);
                    }
                }
            }
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
            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, CyCustomizer.PAGE_PARAM_BASIC);
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
            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, "");
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
            m_control.m_parameters.SerializedHelpers =
                CyHelperInfo.SerializeHelpers(m_control.m_parameters.m_helpersConfig,
                                              m_control.m_parameters.m_serializer,
                                              m_control.m_parameters.m_customSerNamespace);

            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, "");
        }

        #endregion
    }
}
