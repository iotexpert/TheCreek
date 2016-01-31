/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SMBusSlave_v1_0
{
    public class CyDividerCalculator
    {
        public static byte GetPSoC3AndPSoC5LPDivider(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            CyParameters parameters = new CyParameters(query);

            // Get Bus Clock value
            double busClockInKHz = CyClockReader.GetInternalClockInKHz(termQuery, parameters.Implementation);

            byte oversampleRate = (parameters.DataRate <= 50) ? (byte)32 : (byte)16;
            double defaultClockDiv = (double)(busClockInKHz / (parameters.DataRate * oversampleRate));

            return (byte)defaultClockDiv;
        }

        public static byte GetPSoC5ADivider(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            CyParameters parameters = new CyParameters(query);

            // Calculate Over Sample Rate
            byte oversampleRate = (parameters.DataRate <= 50) ? (byte)32 : (byte)16;

            // Get Bus Clock value
            double busClockInKHz = CyClockReader.GetInternalClockInKHz(termQuery, parameters.Implementation);

            // Value for saving power of Divide Factor
            int power = 0;

            // Create possible Baud Rate values
            List<double> baudRate = new List<double>();
            baudRate.Add(busClockInKHz / (1 * oversampleRate));
            baudRate.Add(busClockInKHz / (2 * oversampleRate));
            baudRate.Add(busClockInKHz / (4 * oversampleRate));
            baudRate.Add(busClockInKHz / (8 * oversampleRate));
            baudRate.Add(busClockInKHz / (16 * oversampleRate));
            baudRate.Add(busClockInKHz / (32 * oversampleRate));
            baudRate.Add(busClockInKHz / (64 * oversampleRate));

            // Find most close value (greater or equal) to Data Rate
            for (int i = baudRate.Count - 1; i >= 0; i--)
            {
                if (baudRate[i] >= parameters.DataRate)
                {
                    power = i;
                    break;
                }
            }

            return (byte)power;
        }

        public static void CalculateTimeout(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            CyParameters parameters = new CyParameters(query);

            parameters.m_globalEditMode = true;

            // FF calculation
            double clockInKHz = CyClockReader.GetInternalClockInKHz(termQuery, CyEImplementationType.I2C__FixedFunction);
            if (clockInKHz > 0)
            {
                double fclk = clockInKHz / 1024;
                double tclk = 1 / fclk;
                UInt16 div = 0;
                try
                {
                    div = Convert.ToUInt16(Math.Ceiling(parameters.TimeOutMs / tclk));
                }
                catch { }
                parameters.TimeoutPeriodFF = (div > CyParamRange.FF_DIVIDER_MAX) ? CyParamRange.FF_DIVIDER_MAX : div;
            }

            // UDB calculation
            if (parameters.UdbInternalClock)
            {
                clockInKHz = CyClockReader.GetInternalClockInKHz(termQuery, CyEImplementationType.I2C__UDB);
            }
            else
            {
                clockInKHz = CyClockReader.GetExternalClockInKHz(termQuery);
            }

            if (clockInKHz > 0)
            {
                double count = parameters.TimeOutMs * clockInKHz;
                if (count <= Math.Pow(2, 16))
                {
                    count--;
                    try
                    {
                        parameters.TimeoutPeriodUDB = Convert.ToUInt16(Math.Ceiling(count));
                        parameters.PrescalerEnabled = false;
                    }
                    catch { }
                }
                else
                {
                    parameters.PrescalerEnabled = true;
                    UInt16 prescalerPeriod = 1;
                    while ((count / prescalerPeriod) >= Math.Pow(2, 16))
                    {
                        prescalerPeriod++;
                        if (prescalerPeriod == 256)
                            break;
                    }
                    try
                    {
                        parameters.TimeoutPeriodUDB = Convert.ToUInt16(Math.Ceiling(count / prescalerPeriod));
                    }
                    catch { }
                    parameters.TimeoutPeriodUDB--;
                    prescalerPeriod--;
                    try
                    {
                        parameters.PrescalerPeriod = Convert.ToByte(prescalerPeriod);
                    }
                    catch { }
                }
            }
        }
    }
}
