/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace EMIF_v1_20
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

            CyEMIFParameters parameters = new CyEMIFParameters(query as ICyInstEdit_v1, termQuery);

            double busClkFreq = CyClockReader.GetBusClkInMHz(termQuery);

            int writeCPUCycles = 0;
            int readCPUCycles = 0;
            double writeCPUCyclesNs = 0;
            double readCPUCyclesNs = 0;
            int emifWpStates = 0;
            int emifRpStates = 0;
            byte freqDiv = 0;

            CyCustomizer.RWStates(parameters.MemorySpeed, busClkFreq, parameters.m_isPSoC5LP, out writeCPUCycles, 
                out writeCPUCyclesNs, out readCPUCycles, out readCPUCyclesNs, out emifWpStates, out emifRpStates, 
                out freqDiv);
            freqDiv--;
            paramDict.Add("emifRpStates", emifRpStates.ToString());
            paramDict.Add("emifWpStates", emifWpStates.ToString());
            paramDict.Add("emifClkDiv", freqDiv.ToString());

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }

        public static void RWStates(double memorySpeed, double busClkFreq, bool isPSoC5LP, out int writeCycles, 
            out double writeCyclesNs, out int readCycles, out double readCyclesNs, out int emifWpStates, 
            out int emifRpStates, out byte freqDivider)
        {
            emifWpStates = 0;
            freqDivider = CyCustomizer.GetFrequencyDivider(busClkFreq);

            do
            {
                emifWpStates = 0;
                writeCycles = CyCustomizer.GetWriteCycles(freqDivider) + emifWpStates;
                writeCyclesNs = writeCycles * (1 / (busClkFreq / 1000));

                while (writeCyclesNs < memorySpeed && emifWpStates <= CyParamRange.WAIT_STATE_MAX)
                {
                    emifWpStates++;
                    writeCycles = CyCustomizer.GetWriteCycles(freqDivider) + emifWpStates;
                    writeCyclesNs = writeCycles * (1 / (busClkFreq / 1000));
                }

                if (emifWpStates > CyParamRange.WAIT_STATE_MAX)
                {
                    freqDivider++;
                }
            }
            while (emifWpStates > CyParamRange.WAIT_STATE_MAX);
            
            do
            {
                emifRpStates = 0;
                readCycles = CyCustomizer.GetReadCycles(freqDivider) + emifRpStates;
                readCyclesNs = readCycles * (1 / (busClkFreq / 1000));

                while (readCyclesNs < memorySpeed && emifRpStates <= CyParamRange.WAIT_STATE_MAX)
                {
                    emifRpStates++;
                    readCycles = CyCustomizer.GetReadCycles(freqDivider) + emifRpStates;
                    readCyclesNs = readCycles * (1 / (busClkFreq / 1000));
                }

                if (emifRpStates > CyParamRange.WAIT_STATE_MAX)
                {
                    freqDivider++;
                }
            }
            while (emifRpStates > CyParamRange.WAIT_STATE_MAX);

//            emifRpStates = emifWpStates;
//            readCycles = writeCycles - 1;
//            readCyclesNs = readCycles * (1 / (busClkFreq / 1000));

            if ((busClkFreq <= 33 && busClkFreq > 29) ||
                (busClkFreq <= 67 && busClkFreq > 62) ||
                (busClkFreq <= 99 && busClkFreq > 83))
            {
                if (emifWpStates < 1) emifWpStates = 1;
            }

            if ((busClkFreq <= 33 && busClkFreq > 19) ||
                (busClkFreq <= 54 && busClkFreq > 41))
            {
                if (emifRpStates < 1) emifRpStates = 1;
            }
            if ((busClkFreq <= 67 && busClkFreq > 54) ||
                (busClkFreq <= 82 && busClkFreq > 67))
            {
                if (emifRpStates < 2) emifRpStates = 2;
            }
            if ((busClkFreq <= 95 && busClkFreq > 82))
            {
                if (emifRpStates < 3) emifRpStates = 3;
            }
            if ((busClkFreq <= 99 && busClkFreq > 95))
            {
                if (emifRpStates < 4) emifRpStates = 4;
            }

            writeCycles = CyCustomizer.GetWriteCycles(freqDivider) + emifWpStates;
            writeCyclesNs = writeCycles * (1 / (busClkFreq / 1000));
            readCycles = CyCustomizer.GetReadCycles(freqDivider) + emifRpStates;
            readCyclesNs = readCycles * (1 / (busClkFreq / 1000));
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

        public static int GetWriteCycles(byte frequencyDivider)
        {
            return (CyParamRange.READ_WRITE_WAIT_STATE_CONST + frequencyDivider * 2);
        }

        public static int GetReadCycles(byte frequencyDivider)
        {
            return (CyParamRange.READ_WRITE_WAIT_STATE_CONST + frequencyDivider * 2 - 1);
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
