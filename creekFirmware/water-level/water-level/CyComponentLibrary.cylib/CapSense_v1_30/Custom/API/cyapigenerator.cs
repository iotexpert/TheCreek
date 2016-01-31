/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.IO;
using System.Diagnostics;
using  CapSense_v1_30.API;

namespace CapSense_v1_30
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, 
            IEnumerable<CyAPICustomizer> apis)
        {
            //Debugger.Launch();
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            string instanceName = "";

            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file.
                if ((api.OriginalName.EndsWith("CapSense.c")) || (api.OriginalName.EndsWith("CapSense.h")))
                {
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(CyGeneralParams.C_INSTANCE_NAME, out instanceName);
                }
            }

            string XMLSerializ = "";
            paramDict.TryGetValue(CyGeneralParams.C_XML_MAIN_DATA, out XMLSerializ);
            CyGeneralParams packParams = (CyGeneralParams)CyGeneralParams.Deserialize(XMLSerializ, false);

            packParams.GetGeneralParameters(query,null);

            CyAPIGenerator apiGen = new CyAPIGenerator(instanceName);
            apiGen.CollectApiCFile(query, termQuery, packParams, ref paramDict);
            apiGen.CollectApiHFile(query, termQuery, packParams, ref paramDict);

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
