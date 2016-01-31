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

namespace I2C_v1_20
{
    public class CyI2CParameters
    {
        private ICyInstEdit_v1 m_inst;
        public CyI2CBasic m_basicTab;

        [NonSerialized()]
        public bool m_bGlobalEditMode = false;
        private string[,] m_modesList = new string[4, 2];

        #region Component Parameters

        private string m_mode;
        private string m_busSpeed;
        private string m_implementation;
        private string m_addressDecode;
        private string m_slaveAddress;
        private bool m_wakeupMode;

        #endregion

        public CyI2CParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
            InitializeModesList(m_inst);
        }

        #region Getting parameters

        public void GetParams(ICyInstEdit_v1 inst)
        {
            // Initializing parameters and assigning parameters to controls
            // I2C_Mode
            if (ModeValidated(inst.GetCommittedParam("I2C_Mode").Expr.Trim()))
            {
                m_mode = inst.GetCommittedParam("I2C_Mode").Expr.Trim();
                if (m_bGlobalEditMode == false)
                {
                    for (int i = 0; i < m_modesList.GetLength(0); i++)
                    {
                        m_basicTab.cbMode.Items.Add(m_modesList[i, 0]);
                    }
                }
                m_basicTab.cbMode.Text = GetDisplayNameByName(m_mode);
                CancelEventArgs ce = new CancelEventArgs();
                m_basicTab.ControlsValidating(m_basicTab.cbMode, ce);
            }
            // BusSpeed_kHz
            if (BusSpeedValidated(inst.GetCommittedParam("BusSpeed_kHz").Value))
            {
                m_busSpeed = inst.GetCommittedParam("BusSpeed_kHz").Value;
                m_basicTab.cbBusSpeed.Text = m_busSpeed;
            }
            // Implementation
            if (ImplementationValidated(inst.GetCommittedParam("Implementation").Expr))
            {
                m_implementation = inst.GetCommittedParam("Implementation").Expr;
                switch (m_implementation)
                {
                    case "FixedFunction":
                        m_basicTab.rbFixedFunction.Checked = true;
                        break;
                    case "UDB":
                        m_basicTab.rbUDB.Checked = true;
                        break;
                }
            }
            // Address_Decode
            if (AddressDecodeValidated(inst.GetCommittedParam("Address_Decode").Expr))
            {
                m_addressDecode = inst.GetCommittedParam("Address_Decode").Expr;
                switch (m_addressDecode)
                {
                    case "Hardware":
                        m_basicTab.rbHardware.Checked = true;
                        break;
                    case "Software":
                        m_basicTab.rbSoftware.Checked = true;
                        break;
                }
            }
            // Slave_Address
            if (SlaveAddressValidated(inst.GetCommittedParam("Slave_Address").Value))
            {
                m_slaveAddress = inst.GetCommittedParam("Slave_Address").Expr;
                m_basicTab.tbSlaveAddress.Text = m_slaveAddress;
            }
            // EnableWakeup
            m_wakeupMode = Convert.ToBoolean(inst.GetCommittedParam("EnableWakeup").Value);
            m_basicTab.ckbWakeup.Checked = m_wakeupMode;
        }

        #endregion

        #region Setting parameters

        public void SetMode(object sender)
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                CancelEventArgs ce = new CancelEventArgs();
                m_basicTab.ControlsValidating(sender, ce);
                if (!ce.Cancel)
                {
                    m_mode = GetNameByDisplayName(m_basicTab.cbMode.Text);
                    m_inst.SetParamExpr("I2C_Mode", m_mode);
                    CommitParams(m_inst);
                }
            }
        }

        public void SetBusSpeed(object sender)
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                CancelEventArgs ce = new CancelEventArgs();
                m_basicTab.ControlsValidating(sender, ce);
                if (!ce.Cancel)
                {
                    m_busSpeed = m_basicTab.cbBusSpeed.Text;
                    m_inst.SetParamExpr("BusSpeed_kHz", m_busSpeed);
                    CommitParams(m_inst);
                }
            }
        }

        public void SetImplementation()
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                m_implementation = m_basicTab.rbFixedFunction.Checked ? "FixedFunction" : "UDB";
                m_inst.SetParamExpr("Implementation", m_implementation);
                CommitParams(m_inst);
            }
        }

        public void SetAddressDecode()
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                m_addressDecode = m_basicTab.rbHardware.Checked ? "Hardware" : "Software";
                m_inst.SetParamExpr("Address_Decode", m_addressDecode);
                CommitParams(m_inst);
            }
        }

        public void SetSlaveAddress(object sender)
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                CancelEventArgs ce = new CancelEventArgs();
                m_basicTab.ControlsValidating(sender, ce);
                if (!ce.Cancel)
                {
                    m_slaveAddress = m_basicTab.tbSlaveAddress.Text;
                    m_inst.SetParamExpr("Slave_Address", m_slaveAddress);
                    CommitParams(m_inst);
                }
            }
        }

        public void SetWakeupMode()
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                m_wakeupMode = m_basicTab.ckbWakeup.Checked;
                m_inst.SetParamExpr("EnableWakeup", m_wakeupMode.ToString());
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

        public bool BusSpeedValidated(string value)
        {
            if (value != "" && value != null)
            {
                int var = int.Parse(value);
                if (var >= 50 && var <= 400)
                    return true;
                else
                    return false;
            }
            else
            { return false; }
        }

        public bool ModeValidated(string value)
        {
            if (value != "" && value != null)
            {
                for (int i = 0; i < m_modesList.GetLength(0); i++)
                {
                    if (value == m_modesList[i, 1])
                        return true;
                }
                return false;
            }
            else
            { return false; }
        }

        public bool ImplementationValidated(string value)
        {
            if (value != "" && value != null)
            {
                IList<string> list = (IList<string>)m_inst.GetPossibleEnumValues("Implementation");
                for (int i = 0; i < list.Count; i++)
                {
                    if (value == list[i])
                        return true;
                }
                return false;
            }
            else
            { return false; }
        }

        public bool AddressDecodeValidated(string value)
        {
            if (value != "" && value != null)
            {
                IList<string> list = (IList<string>)m_inst.GetPossibleEnumValues("Address_Decode");
                for (int i = 0; i < list.Count; i++)
                {
                    if (value == list[i])
                        return true;
                }
                return false;
            }
            else
            { return false; }
        }

        public bool SlaveAddressValidated(string value)
        {
            if (value != "" && value != null)
            {
                int var = int.Parse(value);
                if (var >= 0 && var < 128)
                    return true;
                else
                    return false;
            }
            else
            { return false; }
        }

        #endregion

        #region Converting Name to DiasplayName and back DiasplayName to Name methods

        private void InitializeModesList(ICyInstEdit_v1 inst)
        {
            IList<string> tmpModesIList = (IList<string>)inst.GetPossibleEnumValues("I2C_Mode");
            for (int i = 0; i < tmpModesIList.Count; i++)
            {
                m_modesList[i, 0] = tmpModesIList[i];
            }
            m_modesList[0, 1] = "Slave";
            m_modesList[1, 1] = "Master";
            m_modesList[2, 1] = "Multi_Master";
            m_modesList[3, 1] = "Multi_Master_Slave";
        }

        // Converts Display Name to Name
        public string GetNameByDisplayName(string displayName)
        {
            for (int i = 0; i < m_modesList.GetLength(0); i++)
            {
                if (displayName == m_modesList[i, 0])
                    return m_modesList[i, 1];
            }
            return null;
        }

        // Converts Name to Display Name
        public string GetDisplayNameByName(string name)
        {
            for (int i = 0; i < m_modesList.GetLength(0); i++)
            {
                if (name == m_modesList[i, 1])
                    return m_modesList[i, 0];
            }
            return null;
        }

        #endregion
    }
}
