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
using System.Diagnostics;

namespace EZI2C_v1_60
{
    #region Component parameter names
    public class CyParamNames
    {
        public const string BUS_SPEED_KHZ = "BusSpeed_kHz";
        public const string I2C_ADDRESSES = "I2C_Addresses";
        public const string I2C_ADDRESS_1 = "I2C_Address1";
        public const string I2C_ADDRESS_2 = "I2C_Address2";
        public const string SUB_ADDRESS_SIZE = "Sub_Address_Size";
        public const string I2C_BUS_PORT = "I2cBusPort";
        public const string ENABLE_WAKEUP = "EnableWakeup";

        // Local
        public const string HEX1 = "Hex1";
        public const string HEX2 = "Hex2";
    }
    #endregion

    #region Component enums
    public enum CyESubAddressSize { Width_8_Bits, Width_16_Bits }
    public enum CyEI2CBusPort { Any, I2C0, I2C1 };
    #endregion

    public class CyEZI2CParameters
    {
        public ICyInstEdit_v1 m_inst;
        public CyEZI2CBasic m_basicTab;
        public List<string> m_portsList;
        /// <summary>
        /// During first getting of parameters this variable is false, what means that assigning
        /// values to form controls will not immediatly overwrite parameters with the same values.
        /// </summary>
        public bool m_globalEditMode = false;

        #region Component parameters
        public ushort? m_busSpeed_kHZ;
        public int m_i2c_Addresses;
        public int? m_i2c_Address1;
        public int? m_i2c_Address2;
        public CyESubAddressSize m_sub_Address_Size;
        public CyEI2CBusPort m_i2c_Bus_Port;
        public bool m_enableWakeup;
        // Service vars, not visible for user
        public bool m_hex1;
        public bool m_hex2;
        #endregion

        #region Parameters range constants
        public const int BUS_SPEED_MIN = 50;
        public const int BUS_SPEED_MAX = 1000;
        public const int I2C_ADDRESSES_MIN = 1;
        public const int I2C_ADDRESSES_MAX = 2;
        public const int I2C_ADDRESS_MIN = 0;
        public const int I2C_ADDRESS_MAX = 127;
        #endregion

        #region Constructor(s)
        public CyEZI2CParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
            m_portsList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.I2C_BUS_PORT));
        }
        #endregion

        #region Getting parameters
        public void GetParams(ICyInstEdit_v1 inst)
        {
            // BusSpeed
            if (IntValidated(inst.GetCommittedParam(CyParamNames.BUS_SPEED_KHZ).Value, CyParamNames.BUS_SPEED_KHZ))
            {
                m_busSpeed_kHZ = ushort.Parse(inst.GetCommittedParam(CyParamNames.BUS_SPEED_KHZ).Value);
            }

            // I2C_Addresses
            if (IntValidated(inst.GetCommittedParam(CyParamNames.I2C_ADDRESSES).Value, CyParamNames.I2C_ADDRESSES))
            {
                m_i2c_Addresses = int.Parse(inst.GetCommittedParam(CyParamNames.I2C_ADDRESSES).Value);
            }

            // I2C_Address1
            if (IntValidated(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_1).Value, CyParamNames.I2C_ADDRESS_1))
            {
                m_i2c_Address1 = int.Parse(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_1).Value);
            }

            // I2C_Address1
            if (IntValidated(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_2).Value, CyParamNames.I2C_ADDRESS_2))
            {
                m_i2c_Address2 = int.Parse(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_2).Value);
            }

            // Sub_Address_Size
            if (StringValidated(inst.GetCommittedParam(CyParamNames.SUB_ADDRESS_SIZE).Expr, 
                CyParamNames.SUB_ADDRESS_SIZE))
            {
                m_sub_Address_Size = (CyESubAddressSize)Convert.ToByte(
                    inst.GetCommittedParam(CyParamNames.SUB_ADDRESS_SIZE).Value);
            }

            // I2C_BUS_PORT
            if (StringValidated(inst.GetCommittedParam(CyParamNames.I2C_BUS_PORT).Expr, CyParamNames.I2C_BUS_PORT))
            {
                m_i2c_Bus_Port = (CyEI2CBusPort)Convert.ToByte(inst.GetCommittedParam(CyParamNames.I2C_BUS_PORT).Value);
            }

            // EnableWakeup
            inst.GetCommittedParam(CyParamNames.ENABLE_WAKEUP).TryGetValueAs<bool>(out m_enableWakeup);

            // Local Variables which indicates whether hex or decimal is used for addresses
            inst.GetCommittedParam(CyParamNames.HEX1).TryGetValueAs<bool>(out m_hex1);
            inst.GetCommittedParam(CyParamNames.HEX2).TryGetValueAs<bool>(out m_hex2);

            m_basicTab.GetParams();
        }
        #endregion

        #region Setting parameters
        public void SetParams(string paramName)
        {
            if (m_globalEditMode)
            {
                string value = null;
                switch (paramName)
                {
                    case CyParamNames.BUS_SPEED_KHZ:
                        value = m_busSpeed_kHZ.ToString();
                        break;
                    case CyParamNames.I2C_ADDRESSES:
                        value = m_i2c_Addresses.ToString();
                        break;
                    case CyParamNames.I2C_ADDRESS_1:
                        value = m_i2c_Address1.ToString();
                        break;
                    case CyParamNames.I2C_ADDRESS_2:
                        value = m_i2c_Address2.ToString();
                        break;
                    case CyParamNames.SUB_ADDRESS_SIZE:
                        value = m_sub_Address_Size.ToString();
                        break;
                    case CyParamNames.I2C_BUS_PORT:
                        value = m_i2c_Bus_Port.ToString();
                        break;
                    case CyParamNames.ENABLE_WAKEUP:
                        value = m_enableWakeup.ToString().ToLower();
                        break;
                    case CyParamNames.HEX1:
                        value = m_hex1.ToString().ToLower();
                        break;
                    case CyParamNames.HEX2:
                        value = m_hex2.ToString().ToLower();
                        break;
                    default:
                        Debug.Fail("Unhandled case.");
                        break;
                }
                if (value != null && m_inst != null)
                {
                    m_inst.SetParamExpr(paramName, value);
                    m_inst.CommitParamExprs();
                }
            }
        }
        #endregion

        #region Internal parameters validators
        public bool IntValidated(string value, string paramName)
        {
            int min = 0;
            int max = 0;
            int var = -1;
            bool isHex = false;

            if (string.IsNullOrEmpty(value) == false)
            {
                isHex = value.StartsWith(CyCustomizer.HEX_PREFIX);

                if (int.TryParse(value, out var) || isHex)
                {
                    if (isHex) var = CyHexConversion.HexToInt(value);
                    switch (paramName)
                    {
                        case CyParamNames.BUS_SPEED_KHZ:
                            min = BUS_SPEED_MIN;
                            max = BUS_SPEED_MAX;
                            break;
                        case CyParamNames.I2C_ADDRESSES:
                            min = I2C_ADDRESSES_MIN;
                            max = I2C_ADDRESSES_MAX;
                            break;
                        case CyParamNames.I2C_ADDRESS_1:
                            min = I2C_ADDRESS_MIN;
                            max = I2C_ADDRESS_MAX;
                            break;
                        case CyParamNames.I2C_ADDRESS_2:
                            min = I2C_ADDRESS_MIN;
                            max = I2C_ADDRESS_MAX;
                            break;
                        default:
                            Debug.Fail("Unhandled case.");
                            break;
                    }
                    if (var >= min && var <= max)
                        return true;
                    else
                        return false;
                }
                else
                { return false; }
            }
            else
            { return false; }
        }

        public bool StringValidated(string value, string paramName)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(value))
            {
                switch (paramName)
                {
                    case CyParamNames.SUB_ADDRESS_SIZE:
                        result = (value == CyESubAddressSize.Width_8_Bits.ToString() 
                            || value == CyESubAddressSize.Width_16_Bits.ToString());
                        break;
                    case CyParamNames.I2C_BUS_PORT:
                        result = (value == CyEI2CBusPort.Any.ToString() || value == CyEI2CBusPort.I2C0.ToString()
                            || value == CyEI2CBusPort.I2C1.ToString());
                        break;
                    default:
                        Debug.Fail("Unhandled case.");
                        break;
                }

            }
            return result;
        }

        #endregion
    }
}
