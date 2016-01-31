/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



﻿using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace CyCustomizer.PRS_v1_10
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        private const string CS_CFILE_NAME = "PRS.c";
        private const string CS_HFILE_NAME = "PRS.h";
        public const string INSTANCE_NAME_PARAM = "INSTANCE_NAME";
        private string m_instanceName = "";

        CyAPICustomizer m_CS_CFile;
        CyAPICustomizer m_CS_HFile;

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, 
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            //Debugger.Launch();
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            

            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(CS_CFILE_NAME))
                {
                    m_CS_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(INSTANCE_NAME_PARAM, out m_instanceName);
                }
                else if (api.OriginalName.EndsWith(CS_HFILE_NAME))
                {
                    m_CS_HFile = api;
                }
            }
            GenerateHFile(ref paramDict);

            //If No Data in Main object than no data will be generate
            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }
        #endregion

    }
}
