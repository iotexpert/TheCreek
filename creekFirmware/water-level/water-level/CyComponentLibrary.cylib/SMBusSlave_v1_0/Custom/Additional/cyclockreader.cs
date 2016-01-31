/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SMBusSlave_v1_0
{
    public class CyClockReader
    {
        /// <summary>
        /// Returns internal clock value in KHz based on implementation
        /// </summary>
        public static double GetInternalClockInKHz(ICyTerminalQuery_v1 termQuery, CyEImplementationType implementation)
        {
            double clkData;
            if (implementation == CyEImplementationType.I2C__FixedFunction)
                clkData = GetInternalClockInKHz(termQuery, "or_ff");
            else
                clkData = GetInternalClockInKHz(termQuery, "or_udb");
            return clkData;
        }

        /// <summary>
        /// Returns internal clock value in KHz based on OR gate name
        /// </summary>
        public static double GetInternalClockInKHz(ICyTerminalQuery_v1 termQuery, string orGateName)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData(orGateName, "term1", 0);

            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0].Frequency * Math.Pow(10, clkdata[0].UnitAsExponent - 3);
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns connected to component pin clock value in KHz
        /// </summary>
        public static double GetExternalClockInKHz(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData("clock", 0);
            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    return clkdata[0].Frequency * Math.Pow(10, clkdata[0].UnitAsExponent - 3);
                }
            }
            return -1;
        }
    }
}
