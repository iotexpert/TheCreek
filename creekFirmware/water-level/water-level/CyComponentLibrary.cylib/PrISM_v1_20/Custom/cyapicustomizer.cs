/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace PrISM_v1_20
{
    partial class CyCustomizer : ICyAPICustomize_v1
    {
        #region ICyAPICustomize_v1 Members
        static string CS_CFile_Name = "PrISM.c";
        static string CS_HFile_Name = "PrISM.h";
        public const string m_instanceNameParam = "INSTANCE_NAME";
        string m_instanceName = "";

        CyAPICustomizer m_CS_CFile;
        CyAPICustomizer m_CS_HFile;

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
        {
            //Debug.Assert(false);
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            // Get the parameters from the .c file customizer
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(CS_CFile_Name))
                {
                    m_CS_CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(m_instanceNameParam, out m_instanceName);
                }
                else if (api.OriginalName.EndsWith(CS_HFile_Name))
                {
                    m_CS_HFile = api;
                }
            }
            GenerateAPIFiles(ref paramDict);

            //If No Data in Main object than no data will be generate
            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }

            return customizers;
        }
        public void GenerateAPIFiles(ref Dictionary<string, string> paramDict)
        {
            string Resolution;
            string PolyValue;
            string SeedValue;
            string Density0;
            string Density1;
            
            paramDict.TryGetValue("Resolution", out Resolution);
            paramDict.TryGetValue("PolyValue", out PolyValue);
            paramDict.TryGetValue("SeedValue", out SeedValue);
            paramDict.TryGetValue("Density0", out Density0);
            paramDict.TryGetValue("Density1", out Density1);

            int RESOLUTION = int.Parse(Resolution);

            paramDict["SeedValue"] = "0x" + Convert.ToUInt64(SeedValue).ToString("X") + "u";
            paramDict["PolyValue"] = "0x" + Convert.ToUInt64(PolyValue).ToString("X") + "u";
            paramDict["Density0"] = "0x" + Convert.ToUInt64(Density0).ToString("X") + "u";
            paramDict["Density1"] = "0x" + Convert.ToUInt64(Density1).ToString("X") + "u";
        }
        #endregion

    }
}
