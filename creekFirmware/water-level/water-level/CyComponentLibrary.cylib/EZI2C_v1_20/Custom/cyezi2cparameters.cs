/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace EZI2C_v1_20
{
    #region Component Parameter Names

    public class CyParamNames
    {
        public const string BUS_SPEED_KHZ = "BusSpeed_kHz";
        public const string I2C_ADDRESSES = "I2C_Addresses";
        public const string I2C_ADDRESS_1 = "I2C_Address1";
        public const string I2C_ADDRESS_2 = "I2C_Address2";
        public const string SUB_ADDRESS_SIZE = "Sub_Address_Size";
        public const string ENABLE_WAKEUP = "EnableWakeup";
    }

    #endregion

    #region Component Enums

    public enum E_SUB_ADDRESS_SIZE { Width_8_Bits, Width_16_Bits }

    #endregion

    public class CyEZI2CParameters
    {
        private ICyInstEdit_v1 m_inst;
        public CyEZI2CBasic m_basicTab;

        [NonSerialized()]
        public bool m_bGlobalEditMode = false;

        #region Component Parameters

        public int m_BusSpeed_kHZ;
        public int m_I2C_Addresses;
        public int m_I2C_Address1;
        public int m_I2C_Address2;
        public E_SUB_ADDRESS_SIZE m_Sub_Address_Size;
        public bool m_EnableWakeup;

        #endregion

        #region Component Constants

        public const int BUS_SPEED_MIN = 50;
        public const int BUS_SPEED_MAX = 400;
        public const int I2C_ADDRESSES_MIN = 1;
        public const int I2C_ADDRESSES_MAX = 2;
        public const int I2C_ADDRESS_MIN = 0;
        public const int I2C_ADDRESS_MAX = 127;

        #endregion

        public CyEZI2CParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
        }

        #region Getting parameters

        public void GetParams(ICyInstEdit_v1 inst)
        {
            // BusSpeed
            if (IntValidated(inst.GetCommittedParam(CyParamNames.BUS_SPEED_KHZ).Value, CyParamNames.BUS_SPEED_KHZ))
            {
                m_BusSpeed_kHZ = int.Parse(inst.GetCommittedParam(CyParamNames.BUS_SPEED_KHZ).Value);
            }

            // I2C_Addresses
            if (IntValidated(inst.GetCommittedParam(CyParamNames.I2C_ADDRESSES).Value, CyParamNames.I2C_ADDRESSES))
            {
                m_I2C_Addresses = int.Parse(inst.GetCommittedParam(CyParamNames.I2C_ADDRESSES).Value);
            }

            // I2C_Address1
            if (IntValidated(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_1).Value, CyParamNames.I2C_ADDRESS_1))
            {
                m_I2C_Address1 = int.Parse(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_1).Value);
            }

            // I2C_Address1
            if (IntValidated(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_2).Value, CyParamNames.I2C_ADDRESS_2))
            {
                m_I2C_Address2 = int.Parse(inst.GetCommittedParam(CyParamNames.I2C_ADDRESS_2).Value);
            }

            // Sub_Address_Size
            if (StringValidated(inst.GetCommittedParam(CyParamNames.SUB_ADDRESS_SIZE).Expr))
                m_Sub_Address_Size = (E_SUB_ADDRESS_SIZE)Convert.ToByte(
                    inst.GetCommittedParam(CyParamNames.SUB_ADDRESS_SIZE).Value);

            // EnableWakeup
            m_EnableWakeup = Convert.ToBoolean(inst.GetCommittedParam(CyParamNames.ENABLE_WAKEUP).Value);

            m_basicTab.GetParams();
        }

        #endregion

        #region Setting parameters

        public void SetParams(string paramName)
        {
            object value = null;
            switch (paramName)
            {
                case CyParamNames.BUS_SPEED_KHZ:
                    value = m_BusSpeed_kHZ;
                    break;
                case CyParamNames.I2C_ADDRESSES:
                    value = m_I2C_Addresses;
                    break;
                case CyParamNames.I2C_ADDRESS_1:
                    value = m_I2C_Address1;
                    break;
                case CyParamNames.I2C_ADDRESS_2:
                    value = m_I2C_Address2;
                    break;
                case CyParamNames.SUB_ADDRESS_SIZE:
                    value = m_Sub_Address_Size;
                    break;
                case CyParamNames.ENABLE_WAKEUP:
                    value = m_EnableWakeup;
                    break;
                default:
                    break;
            }
            if (value != null)
            {
                m_inst.SetParamExpr(paramName, value.ToString());
                CommitParams(m_inst);
            }
        }

        public void CommitParams(ICyInstEdit_v1 inst)
        {
            if (!inst.CommitParamExprs())
            {
                MessageBox.Show("Error in Committing Parameters");
            }
        }

        #endregion

        #region Parameters validators

        public bool IntValidated(string value, string paramName)
        {
            int min = 0;
            int max = 0;

            if (value != "" && value != null)
            {
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
                        break;
                }
                int var = int.Parse(value);
                if (var >= min && var <= max)
                    return true;
                else
                    return false;
            }
            else
            { return false; }
        }

        public bool StringValidated(string value)
        {
            if (value != "" && value != null)
            {
                if (value == "Width_8_Bits" || value == "Width_16_Bits")
                    return true;
                else
                    return false;
            }
            else
            { return false; }
        }

        #endregion
    }
}
