/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SegLCD_v2_0
{
    public partial class CyCustomizer :ICyAPICustomize_v2, ICyDRCProvider_v1
    {
        private const string LCD_CFILE_NAME = "LCD.c";
        public const string GROUP_NAME = "SegLcdPort";

        #region ICyAPICustomize_v2 Members

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyAPICustomizeArgs_v2 args,
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            ICyInstQuery_v1 instQuery = args.InstQuery;
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            CyLCDParameters parameters = new CyLCDParameters();
            for (int i = 0; i < customizers.Count; i++)
            {
                CyAPICustomizer api = customizers[i];
            
                // Get dict from main file.
                if (api.OriginalName.EndsWith(LCD_CFILE_NAME))
                {
                    paramDict = api.MacroDictionary;
                }
            }

            string paramHelpers;
            paramDict.TryGetValue(CyLCDParameters.PARAM_HELPERS, out paramHelpers);

            string sNumCommonLines;
            string sNumSegmentLines;

            paramDict.TryGetValue(CyLCDParameters.PARAM_NUMCOMMONLINES, out sNumCommonLines);
            paramDict.TryGetValue(CyLCDParameters.PARAM_NUMSEGMENTLINES, out sNumSegmentLines);

            int pNumCommonLines = Convert.ToInt32(sNumCommonLines);
            int pNumSegmentLines = Convert.ToInt32(sNumSegmentLines);

            parameters.DeserializeHelpers(paramHelpers);

            for (int i = 0; i < parameters.m_helpersConfig.Count; i++)
            {
                for (int j = 0; j < parameters.m_helpersConfig[i].m_helpSegInfo.Count; j++)
                {
                    parameters.m_helpersConfig[i].m_helpSegInfo[j].Name = string.Format("{0}_{1}", 
                        instQuery.InstanceName, parameters.m_helpersConfig[i].m_helpSegInfo[j].Name);
                }
            }

            CyAPIGenerator apiGen = new CyAPIGenerator(instQuery.InstanceName);

            apiGen.CollectApiCore(instQuery, ref parameters.m_helpersConfig, ref paramDict, pNumCommonLines,
                                  pNumSegmentLines);
            apiGen.CollectApiHeader(args, ref parameters.m_helpersConfig, ref paramDict, pNumCommonLines,
                                    pNumSegmentLines);

            for (int i = 0; i < customizers.Count; i++)
            {
                CyAPICustomizer api = customizers[i];
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion

        #region ICyDRCProvider_v1 Members

        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            ICyInstQuery_v1 instQuery = args.InstQueryV1;
            byte m_driverPowerMode =
                Convert.ToByte(instQuery.GetCommittedParam(CyLCDParameters.PARAM_DRIVERPOWERMODE).Value);

            if (m_driverPowerMode == (byte)CyBasicConfiguration.CyMode.LOW_POWER_32K)
            {
                foreach (string item in args.InstQueryV1.DesignQuery.ClockIDs)
                {
                    if (args.InstQueryV1.DesignQuery.GetClockName(item) == "XTAL_32KHZ")
                    {
                        bool isXTALClockEnabled = args.InstQueryV1.DesignQuery.IsClockStartedOnReset(item);
                        if (isXTALClockEnabled == false)
                            yield return
                               new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Properties.Resources.CLOCK_XTAL_ERROR);
                    }
                }
                
            }
            else if (m_driverPowerMode == (byte)CyBasicConfiguration.CyMode.LOW_POWER_ILO)
            {
                foreach (string item in args.InstQueryV1.DesignQuery.ClockIDs)
                {
                    if (args.InstQueryV1.DesignQuery.GetClockName(item) == "ILO")
                    {
                        double freq;
                        byte ext;
                        args.InstQueryV1.DesignQuery.GetClockActualFreq(item, out freq, out ext);
                        if ((freq > 1.0001) || (0.9999 > freq))
                            yield return
                                new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Properties.Resources.CLOCK_ILO_ERROR);
                    }
                }
            }
        }

        #endregion
    }
}