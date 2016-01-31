/*******************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace ADC_SAR_v1_71
{
    public static class CyClockReader
    {
        public const string MASTER_CLK = "MASTER_CLK";
        public const string EXTERNAL_CLK = "aclk";
        public const string INTERNAL_CLK = "_theACLK";

        // Returns internal value in Hz
        public static double GetInternalClockInMHz(ICyDesignQuery_v1 designQuery, string clockName)
        {
            double result = -1;
            try
            {
                if (designQuery.ClockIDs != null)
                {
                    double clockfr;
                    byte out_b;
                    string clockID = GetClockID(designQuery, clockName);

                    designQuery.GetClockActualFreq(clockID, out clockfr, out out_b);
                    result = (double)(clockfr * Math.Pow(10, out_b));
                    result = Math.Round(result, 2);
                }
            }
            catch
            {
                result = -1;
            }
            return result;
        }

        // Returns external clock value in kHz
        public static double GetExternalClockInKHz(ICyTerminalQuery_v1 termQuery, string clockName)
        {
            List<CyClockData> clkdata = termQuery.GetClockData(clockName, 0);
            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    double infreq = clkdata[0].Frequency;
                    switch (clkdata[0].Unit)
                    {
                        case CyClockUnit.Hz:
                            infreq /= 1000;
                            break;
                        case CyClockUnit.MHz:
                            infreq *= 1000;
                            break;
                        case CyClockUnit.GHz:
                            infreq *= 1000000;
                            break;
                        case CyClockUnit.THz:
                            infreq *= 1000000000;
                            break;
                        default:
                            break;
                    }
                    return Math.Round(infreq, 6);
                }
            }
            return (-1);
        }

        public static string GetClockID(ICyDesignQuery_v1 designQuery, string clockName)
        {
            string clockID = string.Empty;
            try
            {
                List<string> clockIDs = new List<string>(designQuery.ClockIDs);

                for (int i = 0; i < clockIDs.Count; i++)
                {
                    if (designQuery.GetClockName(clockIDs[i]) == clockName)
                    {
                        clockID = clockIDs[i];
                        break;
                    }
                }
            }
            catch (Exception)
            {
                clockID = string.Empty;
            }
            return clockID;
        }
    }
}
