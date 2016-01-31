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


namespace CapSense_CSD_v2_0
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1
    {
        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            //Debug.Assert(false);        
            CyCSParameters.GLOBAL_EDIT_MODE = false;
            CyCSParamEditTemplate.RUN_MODE = true;

            //Deserialization
            CyCSParameters packParams = new CyCSParameters();
            CyCSInstParameters instParam = new CyCSInstParameters(edit);
            packParams.m_edit = instParam;
            packParams.m_termQuery = termQuery;

            CyGeneralTab m_generalTab = new CyGeneralTab(packParams);
            packParams.m_advancedTab = new CyAdvancedTab(packParams);
            packParams.m_widgetsTab = new Tabs.CyWidgetsTab(packParams);
            CyScanSlotsTab m_scanSlotsTab = new CyScanSlotsTab(packParams);
            CyTuningTab m_tuningTab = new CyTuningTab(packParams);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.UseBigEditor = true;

            CyParamExprDelegate dataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                CyCSParameters.GLOBAL_EDIT_MODE = false;
                packParams.m_settings.GetParams(instParam, param);
                packParams.m_advancedTab.GetProperties(null, null);
                m_tuningTab.GetProperties(null, null);
                m_generalTab.GetProperties(null, null);
                CyCSParameters.GLOBAL_EDIT_MODE = true;
            };
            editor.AddCustomPage(CyCsResource.GeneralDisplayName, m_generalTab, dataChanged, m_generalTab.TabName);
            editor.AddCustomPage(CyCsResource.WidgetsDisplayName, packParams.m_widgetsTab, null,
                packParams.m_widgetsTab.TabName);
            editor.AddCustomPage(CyCsResource.ScanOrderDisplayName, m_scanSlotsTab, null, m_scanSlotsTab.TabName);
            editor.AddCustomPage(CyCsResource.AdvancedDisplayName, packParams.m_advancedTab, dataChanged,
                packParams.m_advancedTab.TabName);
            editor.AddCustomPage(CyCsResource.TuneHelperDisplayName, m_tuningTab, dataChanged, m_tuningTab.TabName);
            editor.AddDefaultPage(CyCsResource.BuiltInDisplayName, "Built-in");

            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(m_generalTab.UpdateClock));
            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(m_scanSlotsTab.UpdateClock));

            packParams.GetParams(instParam);
            packParams.ExecuteUpdateAll();

            CyCSParameters.GLOBAL_EDIT_MODE = true;

            DialogResult result = editor.ShowDialog();
            return result;
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
            return CyCompDevParamEditorMode.COMPLETE; ;
        }

        #endregion

        #region ICyDRCProvider_v1 Members

        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            byte ch;
            object res;
            bool ffTimersUsed = false;

            //Get NumberOfChannels
            if (byte.TryParse(args.InstQueryV1.GetCommittedParam(CyCSSettings.P_NUMBER_OF_CHANNELS).Value,
                out ch) == false)
                Debug.Assert(false);
            //CLK_FR
            double clockFr;
            if (double.TryParse(args.InstQueryV1.GetCommittedParam(CyCSSettings.P_CLK_FR).Value, out clockFr) == false)
                Debug.Assert(false);
            //Clock Type
            CyClockSourceOptions clockType;
            try
            {
                int val = int.Parse(args.InstQueryV1.GetCommittedParam(CyCSSettings.P_CLOCK_SOURCE).Value);
                clockType = (CyClockSourceOptions)val;
            }
            catch (Exception ex)
            {
                Debug.Assert((new CyCustErr(ex)).IsOK);
                clockType = CyClockSourceOptions.External;
            }
            //Get Prescaler
            CyPrescalerOptions prescaler;
            string str = args.InstQueryV1.GetCommittedParam(CyCSSettings.P_PRESCALER_OPTIONS).Value;
            if (CyCSSettings.ParseEnum(str, typeof(CyPrescalerOptions), out res))
                prescaler = (CyPrescalerOptions)res;
            else
            {
                Debug.Assert(false);
                prescaler = CyPrescalerOptions.Prescaler_None;
            }
            //Get TUNING_METHOD
            CyTuningMethodOptions tuningMethod;
            str = args.InstQueryV1.GetCommittedParam(CyCSSettings.P_TUNING_METHOD).Value;
            if (CyCSSettings.ParseEnum(str, typeof(CyTuningMethodOptions), out res))
                tuningMethod = (CyTuningMethodOptions)res;
            else
            {
                Debug.Assert(false);
                tuningMethod = CyTuningMethodOptions.Tuning_None;
            }


            //PSoC3 ES3 Silicon limit
            for (int i = 0; i < ch; i++)
            {
                CyChannelNumber channel = i == 0 ? CyChannelNumber.First : CyChannelNumber.Second;
                string paramName = CyCSSettings.P_IMPLEMENTATION + CyChannelProperties.GetSufix(channel);
                string val = args.InstQueryV1.GetCommittedParam(paramName).Value;
                if (CyCSSettings.ParseEnum(val, typeof(CyMeasureImplemetation), out res))
                {
                    if ((CyMeasureImplemetation)res == CyMeasureImplemetation.FF_Based)
                    {
                        if (args.DeviceQueryV1.IsPSoC3 && args.DeviceQueryV1.SiliconRevision >= 3)
                            yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error,
                                CyCsResource.SiliconErrorDRI);
                        ffTimersUsed = true;
                    }
                }
                else
                    Debug.Assert(false);
            }

            //FF timers limits
            CyClkItem bus_clk = CyCSParameters.GetBusClockInMHz(args.InstQueryV1);
            if (ffTimersUsed || prescaler == CyPrescalerOptions.Prescaler_FF)
            {
                if (bus_clk != null && clockType == CyClockSourceOptions.Internal && clockFr >= bus_clk.m_actualFreq)
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error,
                                CyCsResource.MESSAGE_ERROR_FF_BASE_LIMITATION);
            }
            //BUS_CLK Check range
            if (clockType == CyClockSourceOptions.BusClk && bus_clk != null &&
                CyCsConst.C_SCAN_CLOCK.CheckRange(bus_clk.m_actualFreq) == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error
                   , String.Format(CyCsResource.MESSAGE_ERROR_BUS_CLK_FREQUNC_LIMIT, CyCsConst.C_SCAN_CLOCK.m_min
                   , CyCsConst.C_SCAN_CLOCK.m_max));
            //BUS_CLK Check range for Auto Tuning
            if (tuningMethod == CyTuningMethodOptions.Tuning_Auto && clockType == CyClockSourceOptions.BusClk
                && bus_clk != null && bus_clk.m_actualFreq != CyCsConst.C_AUTO_TUNING_ALLOWED_CLOCK_VALUE)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error
                   , String.Format(CyCsResource.MESSAGE_ERROR_BUS_CLK_FREQUNC_LIMIT_AUTO));
        }

        #endregion
    }

    #region CyMyICyParamEditTemplate
    public class CyCSParamEditTemplate : UserControl, ICyParamEditingControl
    {
        public static bool RUN_MODE = false;
        protected CyCSParameters m_packParams = null;

        public virtual string TabName
        {
            get { return "Empty"; }
        }

        public CyCSParamEditTemplate()
        {
            this.Load += new EventHandler(CyMyICyParamEditTemplate_Load);
        }

        void CyMyICyParamEditTemplate_Load(object sender, EventArgs e)
        {
            if (RUN_MODE)
                this.Dock = DockStyle.Fill;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets any errors that exist for parameters on the DisplayControl.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CyCustErr> GetErrors()
        {
            if (m_packParams != null && m_packParams.m_edit != null)
            {
                CyCSInstParameters edit = m_packParams.m_edit;
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && param.TabName == TabName)
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                yield return new CyCustErr(errMsg);
                            }
                        }
                }
                if (TabName == CyCsConst.P_SCAN_ORDER_TAB_NAME)
                    foreach (CyScanSlot item in m_packParams.m_widgets.m_scanSlots.GetEmptyComplexScanSlot())
                    {
                        yield return new CyCustErr(String.Format(CyCsResource.EmptyScanSlot, item.ToString()));
                    }
                else if (TabName == m_packParams.m_advancedTab.TabName)
                {
                    if (m_packParams.m_settings.m_currentSource == CyCurrentSourceOptions.Idac_None &&
                        m_packParams.m_settings.m_tuningMethod == CyTuningMethodOptions.Tuning_Auto)
                        yield return new CyCustErr(CyCsResource.AutoTuningLimitation);
                }
            }
        }

        #endregion
    }
    #endregion
}


