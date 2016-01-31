/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



﻿using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace CyCustomizer.PRS_v0_5
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        string CS_CFile_Name = "PRS.c";
        string CS_HFile_Name = "PRS.h";
        public const string p_instanceNameParam = "INSTANCE_NAME";
        string instanceName = "";

        CyAPICustomizer CS_CFile;
        CyAPICustomizer CS_HFile;

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
        {
            //Debugger.Launch();
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            

            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(CS_CFile_Name))
                {
                    CS_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(p_instanceNameParam, out instanceName);
                }
                else if (api.OriginalName.EndsWith(CS_HFile_Name))
                {
                    CS_HFile = api;
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
