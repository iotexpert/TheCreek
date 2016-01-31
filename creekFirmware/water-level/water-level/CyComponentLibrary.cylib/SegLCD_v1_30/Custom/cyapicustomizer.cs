// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================

using System;
using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;

namespace SegLCD_v1_30
{
    public partial class CyCustomizer :ICyAPICustomize_v1
    {

        private const string INSTANCE_NAME_PARAM = "INSTANCE_NAME";

        #region ICyAPICustomize_v1 Members

        private const string LCD_CFILE_NAME = "LCD.c";

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            string instanceName = "";

            m_Parameters = new CyLCDParameters();
            for (int i = 0; i < customizers.Count; i++)
            {
                CyAPICustomizer api = customizers[i];
            
                // Get dict from main file.
                if (api.OriginalName.EndsWith(LCD_CFILE_NAME))
                {
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(INSTANCE_NAME_PARAM, out instanceName);
                }

            }

            string ParamSaved;
            paramDict.TryGetValue(CyLCDParameters.PARAM_HELPERS, out ParamSaved);

            string sNumCommonLines;
            string sNumSegmentLines;

            paramDict.TryGetValue(CyLCDParameters.PARAM_NUMCOMMONLINES, out sNumCommonLines);
            paramDict.TryGetValue(CyLCDParameters.PARAM_NUMSEGMENTLINES, out sNumSegmentLines);

            int pNumCommonLines = Convert.ToInt32(sNumCommonLines);
            int pNumSegmentLines = Convert.ToInt32(sNumSegmentLines);

            m_Parameters.DeserializeHelpers(ParamSaved);

            for (int i = 0; i < m_Parameters.m_HelpersConfig.Count; i++)
            {
                for (int j = 0; j < m_Parameters.m_HelpersConfig[i].HelpSegInfo.Count; j++)
                {
                    m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Name = instanceName + "_" +
                                                                     m_Parameters.m_HelpersConfig[i].HelpSegInfo[j].Name;
                }
            }

            CyAPIGenerator apiGen = new CyAPIGenerator(instanceName);

            apiGen.CollectApiCore(query, ref m_Parameters.m_HelpersConfig, ref paramDict, pNumCommonLines,
                                  pNumSegmentLines);
            apiGen.CollectApiHeader(query, ref m_Parameters.m_HelpersConfig, ref paramDict, pNumCommonLines,
                                    pNumSegmentLines);

            for (int i = 0; i < customizers.Count; i++)
            {
                CyAPICustomizer api = customizers[i];
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion
    }
}