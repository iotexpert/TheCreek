/*******************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace EMIF_v1_0
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1, ICyAPICustomize_v1
    {
        public const string GENERAL_TAB_NAME = "General";

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyEMIFParameters parameters = new CyEMIFParameters(edit, termQuery);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.LoadParameters();
            };

            editor.AddCustomPage(CyEMIFRresource.GeneralTabCaption, new CyEMIFBasicTab(parameters, termQuery),
                ExprDelegate, GENERAL_TAB_NAME);
            editor.AddDefaultPage(CyEMIFRresource.BuiltInTabCaption, "Built-in");

            parameters.LoadParameters();
            parameters.m_globalEditMode = true;
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

        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0) paramDict = customizers[0].MacroDictionary;
            
            byte emifClkDiv = 0;
            byte freqDiv = 0;
            double rpWaitStates = 0;
            double wpWaitStates = 0;
            double busClkFreq = CyClockReader.GetBusClkInMHz(termQuery);
            string memSpeed = query.GetCommittedParam(CyParamNames.EMIF_MEMSPEED).Value;

            freqDiv = GetFrequencyDivider(busClkFreq);
            emifClkDiv = (byte)(freqDiv - 1);

            paramDict.Add("emifClkDiv", emifClkDiv.ToString());

            double memorySpeed = 0;
            double.TryParse(memSpeed, out memorySpeed);

            rpWaitStates = GetRpWaitStates(memorySpeed, busClkFreq, GetReadCycles(freqDiv));
            if (rpWaitStates <= 7)
                paramDict.Add("emifRpStates", rpWaitStates.ToString());

            wpWaitStates = GetWpWaitStates(memorySpeed, busClkFreq, GetWriteCycles(freqDiv));
            if (wpWaitStates <= 7)
                paramDict.Add("emifWpStates", wpWaitStates.ToString());

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }

        public static double GetRpWaitStates(double memorySpeed, double busClkFreq, int readCycles)
        {
            double emifRpStates = 0;
            double readCyclesNs = readCycles * (1 / (busClkFreq / 1000));
            try
            {
                if (readCyclesNs < memorySpeed)
                    emifRpStates = Math.Ceiling((memorySpeed - readCyclesNs) / ((1 / busClkFreq) * 1000));
            }
            catch (Exception)
            {
                emifRpStates = -1;
            }
            return emifRpStates;
        }

        public static double GetWpWaitStates(double memorySpeed, double busClkFreq, int writeCycles)
        {
            double emifWpStates = 0;
            double writeCyclesNs = writeCycles * (1 / (busClkFreq / 1000));
            try
            {
                if (writeCyclesNs < memorySpeed)
                    emifWpStates = Math.Ceiling((memorySpeed - writeCyclesNs) / ((1 / busClkFreq) * 1000));
            }
            catch (Exception)
            {
                emifWpStates = -1;
            }
            return emifWpStates;
        }

        public static byte GetFrequencyDivider(double busClkFreq)
        {
            byte freqDiv;

            if (busClkFreq <= 33)
            {
                freqDiv = 1;
            }
            else if (busClkFreq > 33 && busClkFreq <= 67)
            {
                freqDiv = 2;
            }
            else if (busClkFreq > 67 && busClkFreq <= 99)
            {
                freqDiv = 3;
            }
            else
            {
                freqDiv = 4;
            }
            return freqDiv;
        }

        public static int GetReadCycles(byte frequencyDivider)
        {
            return (1 + frequencyDivider * 2);
        }

        public static int GetWriteCycles(byte frequencyDivider)
        {
            return (2 + frequencyDivider * 2);
        }
        #endregion

        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            string memSpeed = args.InstQueryV1.GetCommittedParam(CyParamNames.EMIF_MEMSPEED).Value;

            CyCustErr err = VerifyBusClock(args.TermQueryV1, memSpeed);
            if (err.IsOk == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
        }

        CyCustErr VerifyBusClock(ICyTerminalQuery_v1 termQuery, string memSpeed)
        {
            double freq = CyClockReader.GetBusClkInMHz(termQuery);

            if (freq > 99)
            {
                double memorySpeed = 0;
                double.TryParse(memSpeed, out memorySpeed);
                double y = memorySpeed / 1000 * (freq / 4);
                if (y > 7)
                {
                    return new CyCustErr(string.Format(CyEMIFRresource.DRCBusClkInvalidValue,
                        Math.Round((7000 / memorySpeed * 4), 0)));
                }
            }
            return CyCustErr.OK;
        }
        #endregion
    }
}
