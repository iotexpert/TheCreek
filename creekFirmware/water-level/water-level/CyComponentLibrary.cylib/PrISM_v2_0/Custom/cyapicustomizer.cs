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

namespace PrISM_v2_0
{
    partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(
            ICyInstQuery_v1 query,
            ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            // Get the parameters from the api file customizer
            foreach (CyAPICustomizer api in customizers)
                // Get dict from main file. 
                if (api.OriginalName.EndsWith("PrISM.c") || api.OriginalName.EndsWith("PrISM.h"))
                    paramDict = api.MacroDictionary;

            string polyValue;
            string seedValue;
            string density0;
            string density1;

            paramDict.TryGetValue(CyPRISMParameters.POLY_VALUE, out polyValue);
            paramDict.TryGetValue(CyPRISMParameters.SEED_VALUE, out seedValue);
            paramDict.TryGetValue(CyPRISMParameters.DENSITY0, out density0);
            paramDict.TryGetValue(CyPRISMParameters.DENSITY1, out density1);

            paramDict[CyPRISMParameters.SEED_VALUE] = "0x" + Convert.ToUInt64(seedValue).ToString("X") + "u";
            paramDict[CyPRISMParameters.POLY_VALUE] = "0x" + Convert.ToUInt64(polyValue).ToString("X") + "u";
            paramDict[CyPRISMParameters.DENSITY0] = "0x" + Convert.ToUInt64(density0).ToString("X") + "u";
            paramDict[CyPRISMParameters.DENSITY1] = "0x" + Convert.ToUInt64(density1).ToString("X") + "u";

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
