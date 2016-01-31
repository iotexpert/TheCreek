/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;

namespace PRS_v1_30
{
    public partial class CyCustomizer
    {
        public void GenerateAPIParameters(ref Dictionary<string, string> paramDict)
        {
            string PolyValueLower;
            string PolyValueUpper;
            string SeedValueUpper;
            string SeedValueLower;
            string sPRSSize;

            paramDict.TryGetValue("PolyValueLower", out PolyValueLower);
            paramDict.TryGetValue("PolyValueUpper", out PolyValueUpper);
            paramDict.TryGetValue("SeedValueUpper", out SeedValueUpper);
            paramDict.TryGetValue("SeedValueLower", out SeedValueLower);
            paramDict.TryGetValue("Resolution", out sPRSSize);
            int PRSSize = int.Parse(sPRSSize);

            paramDict["PolyValueLower"] = "0x" + Convert.ToUInt32(PolyValueLower).ToString("X") + "u";
            paramDict["PolyValueUpper"] = "0x" + Convert.ToUInt32(PolyValueUpper).ToString("X") + "u";
            paramDict["SeedValueLower"] = "0x" + Convert.ToUInt32(SeedValueLower).ToString("X") + "u";
            paramDict["SeedValueUpper"] = "0x" + Convert.ToUInt32(SeedValueUpper).ToString("X") + "u";

            int uRegSize = 8;
            if (PRSSize > 16)
                uRegSize = 32;
            else if (PRSSize > 8)
                uRegSize = 16;

            paramDict.Add("RegSize", "uint" + uRegSize);
        }
    }
}
