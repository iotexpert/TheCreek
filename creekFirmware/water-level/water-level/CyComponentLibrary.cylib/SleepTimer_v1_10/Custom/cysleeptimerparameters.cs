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

namespace SleepTimer_v1_10
{
    #region Component Parameter Names

    public class CyParamNames
    {
        public const string ENABLE_INT = "EnableInt";
        public const string INTERVAL = "Interval";
    }

    #endregion

    public class CySleepTimerParameters
    {
        private ICyInstEdit_v1 m_inst;
        public CySleepTimerCustomizer m_basicTab;
        public List<string> m_intervalComboList;
        public List<string> m_intervalNames;

        [NonSerialized()]
        public bool m_bGlobalEditMode = false;

        #region Component Parameters

        public bool m_enableInt;
        public string m_interval;

        #endregion

        public CySleepTimerParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
            // Getting possible Interval values
            m_intervalComboList = (List<string>)m_inst.GetPossibleEnumValues(CyParamNames.INTERVAL);
            m_intervalNames = (List<string>)m_inst.GetPossibleEnumValues(CyParamNames.INTERVAL);
        }

        #region Getting parameters

        public void GetParams(ICyInstEdit_v1 inst)
        {
            // EnableInt
            m_enableInt = Convert.ToBoolean(m_inst.GetCommittedParam(CyParamNames.ENABLE_INT).Value);

            // Interval
            if (IntervalValidated(m_inst.GetCommittedParam(CyParamNames.INTERVAL).Expr))
            {
                m_interval = m_inst.GetCommittedParam(CyParamNames.INTERVAL).Expr;
            }

            m_basicTab.GetParams();
        }

        #endregion

        #region Setting parameters

        public void SetParams(string paramName)
        {
            string value = null;
            switch (paramName)
            {
                case CyParamNames.ENABLE_INT:
                    value = m_enableInt.ToString().ToLower();
                    break;
                case CyParamNames.INTERVAL:
                    value = m_interval;
                    break;
                default:
                    break;
            }
            if (value != null && m_bGlobalEditMode)
            {
                m_inst.SetParamExpr(paramName, value);
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

        #region Parameters Validators

        public bool IntervalValidated(string value)
        {
            if (value != "" && value != null)
            {
                for (int i = 0; i < m_intervalNames.Count; i++)
                {
                    if (value == m_intervalNames[i])
                        return true;
                }
                return false;
            }
            else
            { return false; }
        }

        #endregion
    }
}
