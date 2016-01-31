/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace BoostConv_v2_10
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyInstValidateHook_v1
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



        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            CyCustErr err;
            double vBatIn = 0;
            byte m_outputVoltage = 0;
            int m_frequency = 0;

            CyCompDevParam param = instVal.GetCommittedParam(CyParameters.INPUT_VOLTAGE);
            if (param != null && param.ErrorCount == 0)
            {
                err = param.TryGetValueAs<double>(out vBatIn);
                Debug.Assert(err.IsOk);
            }
            else Debug.Assert(param != null);

            param = instVal.GetCommittedParam(CyParameters.OUTPUT_VOLTAGE);
            if (param != null && param.ErrorCount == 0)
            {
                err = param.TryGetValueAs<byte>(out m_outputVoltage);
                Debug.Assert(err.IsOk);
            }
            else Debug.Assert(param != null);

            param = instVal.GetCommittedParam(CyParameters.FREQUENCY);
            if (param != null && param.ErrorCount == 0)
            {
                err = param.TryGetValueAs<int>(out m_frequency);
                Debug.Assert(err.IsOk);
            }
            else Debug.Assert(param != null);

            double vOut = Convert.ToDouble(CyParameters.m_outputValuesRange[m_outputVoltage==0?0:m_outputVoltage-2]);
            CyBoostFrequency freq = (CyBoostFrequency)m_frequency;

            if ((freq == CyBoostFrequency.SWITCH_FREQ_2MHZ) && (vOut > 2.0 * vBatIn))
            {
                CyCustErr error = new CyCustErr(Properties.Resources.VoltageErrorMessage2MHz);
                instVal.AddError(CyParameters.INPUT_VOLTAGE, error);
                instVal.AddError(CyParameters.OUTPUT_VOLTAGE, error);
                instVal.AddError(CyParameters.FREQUENCY, error);
            }
            else if ((freq == CyBoostFrequency.SWITCH_FREQ_400KHZ) && (vOut > 4.0 * vBatIn))
            {
                CyCustErr error = new CyCustErr(Properties.Resources.VoltageErrorMessage400kHz);
                instVal.AddError(CyParameters.INPUT_VOLTAGE, error);
                instVal.AddError(CyParameters.OUTPUT_VOLTAGE, error);
                instVal.AddError(CyParameters.FREQUENCY, error);
            }

        }
    }
}
