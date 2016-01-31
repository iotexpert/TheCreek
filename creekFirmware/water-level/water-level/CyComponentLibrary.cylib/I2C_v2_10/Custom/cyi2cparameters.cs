/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Drawing;

namespace I2C_v2_10
{
    #region Component Parameter Names
    public class CyParamNames
    {
        public const string I2C_MODE = "I2C_Mode";
        public const string BUS_SPEED_KHZ = "BusSpeed_kHz";
        public const string IMPLEMENTATION = "Implementation";
        public const string ADDRESS_DECODE = "Address_Decode";
        public const string SLAVE_ADDRESS = "Slave_Address";
        public const string I2C_BUS_PORT = "I2cBusPort";
        public const string ENABLE_WAKEUP = "EnableWakeup";

        // Local
        public const string HEX = "Hex";
    }
    #endregion

    #region Constants For Parameters Range
    public class CyParamRange
    {
        public const int BUS_SPEED_MIN = 50;
        public const int BUS_SPEED_MAX = 1000;
        public const int SLAVE_ADDRESS_MIN = 0;
        public const int SLAVE_ADDRESS_MAX = 127;
    }
    #endregion

    #region Component Enums
    public enum CyEModeType { Slave_revA = 1, Master_revA = 2, MultiMaster_revA = 6, MultiMaster_Slave_revA = 7 }
    public enum CyEImplementationType { UDB, FixedFunction }
    public enum CyEAddressDecodeType { Software, Hardware }
    public enum CyEI2CBusPort { Any, I2C0, I2C1 };
    #endregion

    public class CyI2CParameters
    {
        public ICyInstEdit_v1 m_inst;
        public CyI2CBasic m_basicTab;
        public List<string> m_modesList;
        public List<string> m_portsList;
        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        public bool m_bGlobalEditMode = false;
        // The clock connected to the symbol
        public CyClockData m_clock;

        #region Component Parameters
        private CyEModeType m_mode;
        private int m_busSpeed;
        private CyEImplementationType m_implementation;
        private CyEAddressDecodeType m_addressDecode;
        private int m_slaveAddress;
        private CyEI2CBusPort m_i2c_Bus_Port;
        private bool m_wakeupMode;
        // Local
        public bool m_hex;
        #endregion

        #region Constructor(s)
        public CyI2CParameters(ICyInstEdit_v1 inst, ICyTerminalQuery_v1 termQuery)
        {
            this.m_inst = inst;
            m_modesList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.I2C_MODE));
            m_portsList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.I2C_BUS_PORT));
            m_clock = GetComponentClockInHz(termQuery);
        }
        #endregion

        #region Class Properties
        public CyEModeType Mode
        {
            get
            { return m_mode; }
            set
            {
                if (value == CyEModeType.Slave_revA || value == CyEModeType.Master_revA
                    || value == CyEModeType.MultiMaster_revA || value == CyEModeType.MultiMaster_Slave_revA)
                    m_mode = value;
            }
        }

        public int BusSpeed
        {
            get
            { return m_busSpeed; }
            set
            {
                if (value >= CyParamRange.BUS_SPEED_MIN && value <= CyParamRange.BUS_SPEED_MAX)
                    m_busSpeed = value;
            }
        }

        public CyEImplementationType Implementation
        {
            get
            { return m_implementation; }
            set
            {
                if (value == CyEImplementationType.UDB || value == CyEImplementationType.FixedFunction)
                    m_implementation = value;
            }
        }

        public CyEAddressDecodeType AddressDecode
        {
            get
            { return m_addressDecode; }
            set
            {
                if (value == CyEAddressDecodeType.Hardware || value == CyEAddressDecodeType.Software)
                    m_addressDecode = value;
            }
        }

        public int SlaveAddress
        {
            get
            { return m_slaveAddress; }
            set
            {
                if (value >= CyParamRange.SLAVE_ADDRESS_MIN && value <= CyParamRange.SLAVE_ADDRESS_MAX)
                    m_slaveAddress = value;
            }
        }

        public CyEI2CBusPort I2CBusPort
        {
            get { return m_i2c_Bus_Port; }
            set { m_i2c_Bus_Port = value; }
        }

        public bool WakeupMode
        {
            get { return m_wakeupMode; }
            set { m_wakeupMode = value; }
        }
        #endregion

        #region Getting parameters
        public void GetParams(ICyInstEdit_v1 inst)
        {
            m_bGlobalEditMode = false;
            // Initializing parameters and assigning parameters to controls
            // I2C_Mode
            try
            {
                this.Mode = (CyEModeType)Convert.ToByte(inst.GetCommittedParam(CyParamNames.I2C_MODE).Value);
            }
            catch (Exception) { }

            // BusSpeed_kHz
            try
            {
                this.BusSpeed = int.Parse(inst.GetCommittedParam(CyParamNames.BUS_SPEED_KHZ).Value);
            }
            catch (Exception) { }

            // Implementation
            try
            {
                this.Implementation = (CyEImplementationType)Convert.ToByte(inst.GetCommittedParam(
                CyParamNames.IMPLEMENTATION).Value);
            }
            catch (Exception) { }

            // Address_Decode
            try
            {
                this.AddressDecode = (CyEAddressDecodeType)Convert.ToByte(inst.GetCommittedParam(
                CyParamNames.ADDRESS_DECODE).Value);
            }
            catch (Exception) { }

            // Slave_Address
            try
            {
                this.SlaveAddress = int.Parse(inst.GetCommittedParam(CyParamNames.SLAVE_ADDRESS).Value);
            }
            catch (Exception) { }

            // I2CBusPort
            try
            {
                this.I2CBusPort = (CyEI2CBusPort)Convert.ToByte(inst.GetCommittedParam(
                    CyParamNames.I2C_BUS_PORT).Value);
            }
            catch (Exception) { }

            // EnableWakeup
            try
            {
                this.WakeupMode = Convert.ToBoolean(inst.GetCommittedParam(CyParamNames.ENABLE_WAKEUP).Value);
            }
            catch (Exception) { }

            // Local Variable Hex
            try
            {
                m_hex = Convert.ToBoolean(inst.GetCommittedParam(CyParamNames.HEX).Value);
            }
            catch (Exception) { }

            m_basicTab.GetParams();
            m_bGlobalEditMode = true;
        }
        #endregion

        #region Setting parameters
        public void SetParams(string paramName)
        {
            if (m_bGlobalEditMode)
            {
                string value = null;
                switch (paramName)
                {
                    case CyParamNames.I2C_MODE:
                        value = m_mode.ToString();
                        break;
                    case CyParamNames.BUS_SPEED_KHZ:
                        value = m_busSpeed.ToString();
                        break;
                    case CyParamNames.IMPLEMENTATION:
                        value = m_implementation.ToString();
                        break;
                    case CyParamNames.ADDRESS_DECODE:
                        value = m_addressDecode.ToString();
                        break;
                    case CyParamNames.SLAVE_ADDRESS:
                        value = m_slaveAddress.ToString();
                        break;
                    case CyParamNames.I2C_BUS_PORT:
                        value = m_i2c_Bus_Port.ToString();
                        break;
                    case CyParamNames.ENABLE_WAKEUP:
                        value = m_wakeupMode.ToString().ToLower();
                        break;
                    case CyParamNames.HEX:
                        value = m_hex.ToString().ToLower();
                        break;
                    default:
                        break;
                }
                if (value != null && m_bGlobalEditMode)
                {
                    m_inst.SetParamExpr(paramName, value);
                    m_inst.CommitParamExprs();
                }
            }
        }
        #endregion

        #region Clock reading
        /// <summary>
        /// Returns BusClock value from PSoC Creator in KHz.
        /// </summary>
        public static double GetBusClockInKHz(ICyDesignQuery_v1 DesignQuery)
        {
            List<string> curClockIDs = new List<string>(DesignQuery.ClockIDs);
            double clockfr;
            byte out_b;
            string m_clockID = curClockIDs[0];

            foreach (string clock in curClockIDs)
            {
                if (DesignQuery.GetClockName(clock) == "BUS_CLK")
                    m_clockID = clock;
            }

            DesignQuery.GetClockActualFreq(m_clockID, out clockfr, out out_b);
            double clockfr_o = (double)(clockfr * Math.Pow(10, out_b) / 1000);
            return clockfr_o;
        } 

        /// <summary>
        /// Returns connected clock information
        /// </summary>
        public CyClockData GetComponentClockInHz(ICyTerminalQuery_v1 termQuery)
        {
            List<CyClockData> clkdata = new List<CyClockData>();
            clkdata = termQuery.GetClockData("clock", 0);
            return clkdata[0];
        }

        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            m_clock = GetComponentClockInHz(termQuery);
            m_basicTab.UpdateIFBC(m_clock);
        }
        #endregion
    }
}
