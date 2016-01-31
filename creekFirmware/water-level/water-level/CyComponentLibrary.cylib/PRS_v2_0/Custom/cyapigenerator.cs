/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;

namespace PRS_v2_0
{
    public partial class CyCustomizer
    {
        public void GenerateAPIParameters(ref Dictionary<string, string> paramDict)
        {
            string polyValueLower;
            string polyValueUpper;
            string seedValueUpper;
            string seedValueLower;
            string resolution;
            uint mask = 0;
            int j;

            paramDict.TryGetValue(CyPRSParameters.PARAM_POLYVALUELOWER, out polyValueLower);
            paramDict.TryGetValue(CyPRSParameters.PARAM_POLYVALUEUPPER, out polyValueUpper);
            paramDict.TryGetValue(CyPRSParameters.PARAM_SEEDVALUELOWER, out seedValueLower);            
            paramDict.TryGetValue(CyPRSParameters.PARAM_SEEDVALUEUPPER, out seedValueUpper);
            paramDict.TryGetValue(CyPRSParameters.PARAM_RESOLUTION, out resolution);
            int n = int.Parse(resolution);

            //Convert parameters to HEX format
            paramDict[CyPRSParameters.PARAM_POLYVALUELOWER] = "0x" + Convert.ToUInt32(polyValueLower).ToString("X");
            paramDict[CyPRSParameters.PARAM_POLYVALUEUPPER] = "0x" + Convert.ToUInt32(polyValueUpper).ToString("X");
            paramDict[CyPRSParameters.PARAM_SEEDVALUELOWER] = "0x" + Convert.ToUInt32(seedValueLower).ToString("X");
            paramDict[CyPRSParameters.PARAM_SEEDVALUEUPPER] = "0x" + Convert.ToUInt32(seedValueUpper).ToString("X");

            #region  Mask
            mask = 0;
            if (n > 32) n -= 32;

            for (j = 0; j < n; j++)
            {
                mask = (uint)(mask | (uint)(1 << j));
            }

            paramDict.Add("Mask", "0x" + Convert.ToUInt32(mask).ToString("X"));
            #endregion
        }
    }
}
