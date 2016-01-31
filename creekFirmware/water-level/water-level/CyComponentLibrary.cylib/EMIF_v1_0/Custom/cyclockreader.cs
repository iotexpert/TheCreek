/*******************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace EMIF_v1_0
{
    public class CyClockReader
    {
        public static double GetBusClkInMHz(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData("or_1", "term1", 0);

            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0].Frequency * (Math.Pow(10, clkdata[0].UnitAsExponent) / 1000000);
                }
            }
            return -1;
        }
    }
}