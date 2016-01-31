/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace I2C_v3_1
{
    public class CyClockReader
    {
        /// <summary>
        /// Returns internal clock value
        /// </summary>
        public static CyClockData GetInternalClock(ICyTerminalQuery_v1 termQuery, CyEImplementationType implementation)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            if (implementation == CyEImplementationType.FixedFunction)
                clkdata = termQuery.GetClockData("or_ff", "term1", 0);
            else
                clkdata = termQuery.GetClockData("or_udb", "term1", 0);

            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0];
                }
            }
            return new CyClockData();
        }

        /// <summary>
        /// Returns connected to component pin clock value
        /// </summary>
        public static CyClockData GetExternalClock(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData("clock", 0);
            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0];
                }
            }
            return new CyClockData();
        }

        /// <summary>
        /// Converts argument frequency value to KHz
        /// </summary>
        public static double ConvertFreqToKHz(CyClockData clock)
        {
            double frequency = clock.Frequency;
            switch (clock.Unit)
            {
                case CyClockUnit.Hz:
                    frequency /= 1000;
                    break;
                case CyClockUnit.MHz:
                    frequency *= 1000;
                    break;
                case CyClockUnit.GHz:
                    frequency *= 1000000;
                    break;
                case CyClockUnit.THz:
                    frequency *= 1000000000;
                    break;
                default:
                    break;
            }
            return frequency;
        }
    }
}
