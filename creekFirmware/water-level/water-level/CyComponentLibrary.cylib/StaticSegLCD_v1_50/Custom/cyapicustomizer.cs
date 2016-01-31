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

namespace StaticSegLCD_v1_50
{
    public partial class CyCustomizer :ICyAPICustomize_v2
    {
        #region ICyAPICustomize_v2 Members

        public const string GROUP_NAME = "StSegLcdPort";
        private const string LCD_CFILE_NAME = "LCD.c";

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

            string paramSaved;
            paramDict.TryGetValue(CyLCDParameters.PARAM_HELPERS, out paramSaved);

            string sNumSegmentLines;

            paramDict.TryGetValue(CyLCDParameters.PARAM_NUMSEGMENTLINES, out sNumSegmentLines);

            const int NUM_COMMON_LINES = 1;
            int pNumSegmentLines = Convert.ToInt32(sNumSegmentLines);

            parameters.DeserializeHelpers(paramSaved);

            for (int i = 0; i < parameters.m_helpersConfig.Count; i++)
            {
                for (int j = 0; j < parameters.m_helpersConfig[i].m_helpSegInfo.Count; j++)
                {
                    parameters.m_helpersConfig[i].m_helpSegInfo[j].Name = string.Format("{0}_{1}",
                                        instQuery.InstanceName, parameters.m_helpersConfig[i].m_helpSegInfo[j].Name);
                }
            }

            CyAPIGenerator apiGen = new CyAPIGenerator(instQuery.InstanceName);

            apiGen.CollectApiCore(instQuery, ref parameters.m_helpersConfig, ref paramDict, NUM_COMMON_LINES,
                                  pNumSegmentLines);
            apiGen.CollectApiHeader(args, ref parameters.m_helpersConfig, ref paramDict, NUM_COMMON_LINES,
                                    pNumSegmentLines);

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion
    }
}