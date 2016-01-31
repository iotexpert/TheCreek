/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ADC_SAR_v1_50
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1
    {
        public const string CONFIGURE_TAB_NAME = "Configure";
        CySARADCParameters m_parameters;

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            m_parameters = new CySARADCParameters(edit, termQuery);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_parameters.GetParams(edit, param);
            };

            editor.AddCustomPage(Properties.Resources.ConfigureTabCaption, new CySARADCControl(m_parameters),
                ExprDelegate, CONFIGURE_TAB_NAME);
            editor.AddDefaultPage(Properties.Resources.BuiltInTabCaption, "Built-in");

            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(m_parameters.UpdateClock));

            m_parameters.GetParams(edit, null);
            m_parameters.m_globalEditMode = true;
            return editor.ShowDialog();
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion

        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            string instanceName = args.InstQueryV1.GetCommittedParam("INSTANCE_NAME").Value;
            CyCustErr err = VerifyClock(args.InstQueryV1.DesignQuery, instanceName);
            if (err.IsOk == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
        }

        CyCustErr VerifyClock(ICyDesignQuery_v1 designQuery, string instanceName)
        {
            double result = -1;
            try
            {
                if (designQuery.ClockIDs != null)
                {
                    List<string> curClockIDs = new List<string>(designQuery.ClockIDs);
                    double clockfr;
                    byte out_b;
                    string clockID = curClockIDs[0];

                    for (int i = 0; i < curClockIDs.Count; i++)
                    {
                        if (designQuery.GetClockName(curClockIDs[i]) == instanceName + "_theACLK")
                        {
                            clockID = curClockIDs[i];
                        }
                    }

                    designQuery.GetClockActualFreq(clockID, out clockfr, out out_b);
                    result = (double)(clockfr * Math.Pow(10, out_b) / 1000000);
                    result = Math.Round(result, 2);
                }
            }
            catch
            {
                result = -1;
            }
            if (result > 0)
            {
                if (result < 1 || result > 18)
                    return new CyCustErr(Properties.Resources.DRCInternalClockFreqMsg);
            }
            return CyCustErr.OK;
        }
        #endregion
    }
}