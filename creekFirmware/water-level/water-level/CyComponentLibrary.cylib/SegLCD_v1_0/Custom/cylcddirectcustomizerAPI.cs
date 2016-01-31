/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;

namespace SegLCD_v1_0
{
    public partial class CyCustomizer :ICyAPICustomize_v1
    {

        string instanceNameParam = "INSTANCE_NAME";

        #region ICyAPICustomize_v1 Members
        string LCD_CFile_Name = "LCD.c";

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            string instanceName = "";

            Parameters = new LCDParameters();
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(LCD_CFile_Name))
                {
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(instanceNameParam, out instanceName);
                }
                
            }

            string ParamSaved;
            paramDict.TryGetValue("Helpers", out ParamSaved);

            string sNumCommonLines;
            string sNumSegmentLines;

            paramDict.TryGetValue("NumCommonLines", out sNumCommonLines);
            paramDict.TryGetValue("NumSegmentLines", out sNumSegmentLines);

            int pNumCommonLines = Convert.ToInt32(sNumCommonLines);
            int pNumSegmentLines = Convert.ToInt32(sNumSegmentLines);

            Parameters.M_DeserializeHelpers(ParamSaved);

            M_APIGenerator apiGen = new M_APIGenerator(instanceName);

            apiGen.CollectApiCore(query, ref Parameters.HelpersConfig, ref paramDict, pNumCommonLines, pNumSegmentLines);
            apiGen.CollectApiHeader(query, ref Parameters.HelpersConfig, ref paramDict, pNumCommonLines, pNumSegmentLines);

            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion
    }
}