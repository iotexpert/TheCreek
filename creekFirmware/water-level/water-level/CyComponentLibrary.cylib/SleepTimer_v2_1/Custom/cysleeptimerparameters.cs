/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace SleepTimer_v2_1
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
        public ICyInstEdit_v1 m_inst;
        public CySleepTimerCustomizer m_basicTab;
        public List<string> m_intervalComboList;
        public List<string> m_intervalNames;
        /// <summary>
        /// During first getting of parameters this variable is false, what means that assigning
        /// values to form controls will not immediatly overwrite parameters with the same values.
        /// </summary>
        public bool m_bGlobalEditMode = false;

        #region Class readonly values
        public static readonly string PREFIX = "CTW_";
        public static readonly string POSTFIX = "_MS";
        public static readonly string[] ALLOWED_INTERVAL = { "4", "8", "16" };
        #endregion

        #region Component Parameters
        private bool m_enableInt;
        private string m_interval;
        #endregion

        #region Constructor(s)
        public CySleepTimerParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
            // Getting possible Interval values
            m_intervalComboList = new List<string>(m_inst.GetPossibleEnumValues(CyParamNames.INTERVAL));
            m_intervalNames = new List<string>(m_inst.GetPossibleEnumValues(CyParamNames.INTERVAL));
        }
        #endregion

        #region Class Properties
        public bool EnableInt
        {
            get { return m_enableInt; }
            set { m_enableInt = value; }
        }

        public string Interval
        {
            get
            { return m_interval; }
            set
            {
                for (int i = 0; i < m_intervalNames.Count; i++)
                {
                    if (value == m_intervalNames[i])
                        m_interval = value;
                }
            }
        }
        #endregion

        #region Getting parameters
        public void GetParams(ICyInstEdit_v1 inst, CyCompDevParam param)
        {
            if (param != null)
            {
                switch (param.Name)
                {
                    case CyParamNames.ENABLE_INT:
                        GetEnableInt();
                        break;
                    case CyParamNames.INTERVAL:
                        GetInterval();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                GetEnableInt();
                GetInterval();
            }

            m_basicTab.GetParams();
        }

        private void GetEnableInt()
        {
            bool value;
            if (bool.TryParse(m_inst.GetCommittedParam(CyParamNames.ENABLE_INT).Value, out value))
                this.EnableInt = value;
        }

        private void GetInterval()
        {
            CyCompDevParam currentParam = m_inst.GetCommittedParam(CyParamNames.INTERVAL);
            if (currentParam.ErrorCount == 0)
            {
                int pos = currentParam.GetValueAs<int>();
                List<string> list = new List<string>(m_inst.GetPossibleEnumValues(CyParamNames.INTERVAL));
                this.Interval = list[pos - 1].ToString();
            }
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
                    case CyParamNames.ENABLE_INT:
                        value = m_enableInt.ToString().ToLower();
                        break;
                    case CyParamNames.INTERVAL:
                        value = m_interval;
                        break;
                    default:
                        break;
                }
                if (value != null)
                {
                    m_inst.SetParamExpr(paramName, value);
                    m_inst.CommitParamExprs();
                }
            }
        }
        #endregion
    }
}
