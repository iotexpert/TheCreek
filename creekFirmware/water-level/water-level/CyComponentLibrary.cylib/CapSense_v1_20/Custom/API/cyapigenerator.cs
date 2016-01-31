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
using  CapSense_v1_20.API;

namespace CapSense_v1_20
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        string m_CS_CFile_Name = "CapSense.c";
        string m_CS_HFile_Name = "CapSense.h";

        CyAPICustomizer m_CS_CFile;
        CyAPICustomizer m_CS_HFile;


        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
        {
            //Debugger.Launch();
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            string instanceName = "";

            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(m_CS_CFile_Name))
                {
                    m_CS_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(CyGeneralParams.p_instanceNameParam, out instanceName);
                }
                else if (api.OriginalName.EndsWith(m_CS_HFile_Name))
                {
                    m_CS_HFile = api;
                }
            }

            string XMLSerializ = "";
            paramDict.TryGetValue(CyGeneralParams.p_XMLMainData, out XMLSerializ);
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
