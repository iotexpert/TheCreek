/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Diagnostics;
using Mixer_v1_60;


namespace Mixer_v1_60
{
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1, ICyExprEval_v2, ICyAPICustomize_v1
    {
        const string BasicTabName = "Basic";
        const string BuiltinTabName = "Built-in";
        const string ConfigureTabName = "Configure";
        const string lo = "LO";
        string DRCUpMixerLOClockErr = "LO frequency for UpMixer should be less than 1MHz";
        string DRCDownMixerLOClockErr = "LO frequency for DownMixer should be less than 4MHz";
        string DRCUnKnownExtLOFreq = "Frequency of the LO input is not known to the design. Please set valid frequency in the text box provided in the configure window for mixer to operate properly.";
        double UpMixer_MaxLO = 1000000.00;
        double DownMixer_MaxLO = 4000000.00;
        string DownMixer = "1";
        string UpMixer = "0";
        string LO_Internal = "0";
        string LO_External = "1";
		double infreq = -1;
        string External_LO_Clk_Freq = "EXT_LO_CLK_FREQ";

        CyMixerControl m_control = null;
        
        Mixer_EditingControl m_editingControl;

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            m_control = new CyMixerControl(edit, termQuery);
            m_editingControl = new Mixer_EditingControl(edit, m_control, BasicTabName);
            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                m_control.UpdateFormFromParams(edit);
            };

            editor.AddCustomPage(ConfigureTabName, m_editingControl, ExprDelegate, "Advanced");
            editor.AddDefaultPage(BuiltinTabName, BuiltinTabName);
            editor.UseBigEditor = false;
            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(m_control.UpdateClock));

            DialogResult result = editor.ShowDialog();

            return result;
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

        public class Mixer_EditingControl : ICyParamEditingControl
        {
            CyMixerControl Mixer_Control;
            ICyInstEdit_v1 m_instEdit;
            List<string> m_tabNames;

            public Mixer_EditingControl(ICyInstEdit_v1 inst, CyMixerControl userControl, params string[] tabNames)
            {
                m_instEdit = inst;
                m_tabNames = new List<string>(tabNames);
                Mixer_Control = userControl;
                Mixer_Control.Dock = DockStyle.Fill;
                Mixer_Control.AutoScroll = true;
            }

            #region ICyParamEditingControl Members

            Control ICyParamEditingControl.DisplayControl
            {
                get
                {
                    return Mixer_Control;
                }
            }

            System.Collections.Generic.IEnumerable<CyCustErr> ICyParamEditingControl.GetErrors()
            {
                List<CyCustErr> errs = new List<CyCustErr>();

                foreach (string paramName in m_instEdit.GetParamNames())
                {
                    CyCompDevParam param = m_instEdit.GetCommittedParam(paramName);
                    if (m_tabNames.Contains(param.TabName))
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
            #endregion
        }

            #region ICyDRCProvider_v1 Members
            public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
            {
                string instanceName = args.InstQueryV1.GetCommittedParam("INSTANCE_NAME").Value;
                string Mixer_Type = args.InstQueryV1.GetCommittedParam(cymixerparameters.MIXER_TYPE).Value;
                string LO_Type = args.InstQueryV1.GetCommittedParam(cymixerparameters.LO_SOURCE).Value;
                string LO_Frequency = args.InstQueryV1.GetCommittedParam(cymixerparameters.LO_CLOCK_FREQ).Value;
                
                if (LO_Type.Equals(LO_Internal))
                {
                    CyCustErr err = VerifyInternalClock(args.InstQueryV1.DesignQuery, instanceName, Mixer_Type);
                    if (err.IsOk == false)
                        yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
                }
                if (LO_Type.Equals(LO_External))
                {
                    CyCustErr err = VerifyExternalClock(args.InstQueryV1.DesignQuery, args.TermQueryV1, Mixer_Type, LO_Frequency);
                    if (err.IsOk == false)
                    {
                        if (err.Message.Equals(DRCUnKnownExtLOFreq))
                        {
                            yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Warning, err.Message);
                        }
                        else
                        {
                            yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
                        }
                    }

                }
            }

            CyCustErr VerifyInternalClock(ICyDesignQuery_v1 designQuery, string instanceName, string Mixer_Type)
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
                            if (designQuery.GetClockName(curClockIDs[i]) == instanceName + "_aclk")
                            {
                                clockID = curClockIDs[i];
                            }
                        }
                            designQuery.GetClockActualFreq(clockID, out clockfr, out out_b);
                            result = (double)(clockfr * Math.Pow(10, out_b));
                    }
                }
                catch
                {
                    result = -1;
                }
                if (Mixer_Type.Equals(UpMixer))
                {
                    if (result > UpMixer_MaxLO)
                        return new CyCustErr(DRCUpMixerLOClockErr);
                }
                if (Mixer_Type.Equals(DownMixer))
                {
                    if (result > DownMixer_MaxLO)
                        return new CyCustErr(DRCDownMixerLOClockErr);
                }
                return CyCustErr.OK;
            }

            private void GetFreq(ICyTerminalQuery_v1 termQuery)
            {
                try
                {
                    List<CyClockData> clkdata = new List<CyClockData>();
                    clkdata = termQuery.GetClockData(lo, 0);
                    if (clkdata[0].IsFrequencyKnown)
                    {
                        infreq = clkdata[0].Frequency;
                        switch (clkdata[0].Unit)
                        {
                            case CyClockUnit.MHz: infreq = infreq * 1000000; break;
                            case CyClockUnit.kHz: infreq = infreq * 1000; break;
                            case CyClockUnit.Hz: break;
                        }
                    }
                    else
                    {
                        infreq = -1;
                    }
                }
                catch
                {
                    infreq = -1;
                }
            }

            CyCustErr VerifyExternalClock(ICyDesignQuery_v1 designQuery, ICyTerminalQuery_v1 termQuery, string Mixer_Type, string LO_Frequency)
            {
                double frequency;
                GetFreq(termQuery);
                if (infreq == -1)
                {
                    frequency = Convert.ToDouble(LO_Frequency) * 1000;
                    if (Mixer_Type.Equals(UpMixer))
                    {
                        if (frequency > UpMixer_MaxLO)
                        { 
                            return new CyCustErr(DRCUpMixerLOClockErr);
                        }
                    }
                    if (Mixer_Type.Equals(DownMixer))
                    {
                        if (frequency > DownMixer_MaxLO)
                        { 
                            return new CyCustErr(DRCDownMixerLOClockErr);
                        }
                    }
                    return new CyCustErr(DRCUnKnownExtLOFreq);
                }
                if (Mixer_Type.Equals(UpMixer))
                {
                    if (infreq > UpMixer_MaxLO)
                        return new CyCustErr(DRCUpMixerLOClockErr);
                }
                if (Mixer_Type.Equals(DownMixer))
                {
                    if (infreq > DownMixer_MaxLO)
                        return new CyCustErr(DRCDownMixerLOClockErr);
                }
                return CyCustErr.OK;
            }
            
            #endregion

            #region ICyExprEval_v2
            // implement ICyExprEval_v2
            CyExprEvalFuncEx ICyExprEval_v2.GetFunction(string name)
            {
                switch (name)
                {
                    case "GetVDDA":
                        return new CyExprEvalFuncEx(GetVDDA);
                    default:
                        return null;
                }
            }
            
            // C# function that backs GetMinVDDA expression system function.
            object GetVDDA(string name, object[] fcnArgs, ICyExprEvalArgs_v2 custArgs, ICyExprTypeConverter typeConverter)
            {
                if (custArgs.InstQuery != null)
                {
                    if (custArgs.InstQuery.VoltageQuery != null)
                    {
                        return custArgs.InstQuery.VoltageQuery.VDDA;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            #endregion

            #region ICyAPICustomize_v1 Members
            public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
            {
                Dictionary<string, string> paramDict = null;
                foreach (CyAPICustomizer api in apis)
                {
                    // Get parameter dictionary
                    paramDict = api.MacroDictionary;
                }

                // Get the clock frequency
                UpdateClock(ref paramDict, termQuery);

                // Assign dictionary back to API customizers
                foreach (CyAPICustomizer api in apis)
                {
                    // Get parameter dictionary
                    api.MacroDictionary = paramDict;
                }
                // Return customizers
                return apis;
            }
            #endregion

            void UpdateClock(ref Dictionary<string, string> paramDict, ICyTerminalQuery_v1 termQuery)
            {
                double infreq = -1;

                GetFreq(termQuery);
                 // Write out the clock data
                 paramDict.Add(External_LO_Clk_Freq, infreq.ToString());
            }
    }
}



