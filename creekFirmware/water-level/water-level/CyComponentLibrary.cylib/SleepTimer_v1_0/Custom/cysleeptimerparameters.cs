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
using CyDesigner.Extensions.Gde; 
using System.Windows.Forms;
using System.Drawing;

namespace SleepTimer_v1_0
{
    public class CySleepTimerParameters
    {
        ICyInstEdit_v1 m_inst;
        public CySleepTimerCustomizer m_generalParams;
        public IList<string> m_intervalList;

        [NonSerialized()]
        public bool m_bGlobalEditMode = false;

        #region Component Parameters

        private string m_interval;
        private bool m_enableInt;

        #endregion

        public CySleepTimerParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
        }

        #region Getting parameters

        public void GetParams(ICyInstEdit_v1 inst)
        {
            // Getting values and initializing class parameters
            m_enableInt = Convert.ToBoolean(m_inst.GetCommittedParam("EnableInt").Value);
            m_interval = m_inst.GetCommittedParam("Interval").Value;

            // Assigning parameters to controls
            if (m_bGlobalEditMode == false)
            {
                m_intervalList = (List<string>)m_inst.GetPossibleEnumValues("Interval");
                for (int i = 0; i < m_intervalList.Count; i++)
                {
                    m_intervalList[i] = m_intervalList[i].Replace("CTW_", "");
                    m_intervalList[i] = m_intervalList[i].Replace("_MS", "");
                }
                m_generalParams.comboBoxInterval.DataSource = m_intervalList;
            }
            if (m_interval != null)
                m_generalParams.comboBoxInterval.SelectedIndex = int.Parse(m_interval) - 1;

            if (m_enableInt)
            {
                m_generalParams.checkBoxInterrupt.Checked = true;
            }
            else
            {
                m_generalParams.checkBoxInterrupt.Checked = false;
            }
        }

        #endregion

        #region Setting parameters

        public void SetEnableInterruptCheckBox()
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                m_enableInt = m_generalParams.checkBoxInterrupt.Checked;
                m_inst.SetParamExpr("EnableInt", m_enableInt.ToString());
                CommitParams(m_inst);
            }
        }

        public void SetIntervalComboBox()
        {
            if (m_inst != null && m_bGlobalEditMode)
            {
                m_interval = "CTW_" + m_generalParams.comboBoxInterval.SelectedValue.ToString() + "_MS";
                m_inst.SetParamExpr("Interval", m_interval);
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
    }
}
