/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace RTC_v1_70
{
    #region Component Parameters Names
    public class CyParamNames
    {
        public const string DST_ENABLE = "DstEnable";
        public const string START_OF_WEEK = "StartOfWeek";
    }
    #endregion

    class CyRTCParameters
    {
        public ICyInstEdit_v1 m_edit;

        private bool m_dstEnable;
        private DayOfWeek m_day;

        #region Class Properties
        public bool DstEnable
        {
            get { return m_dstEnable; }
            set
            {
                m_dstEnable = value;
                if (m_edit != null)
                {
                    m_edit.SetParamExpr(CyParamNames.DST_ENABLE, m_dstEnable.ToString().ToLower());
                    m_edit.CommitParamExprs();
                }
            }
        }

        public DayOfWeek StartDayOfWeek
        {
            get { return m_day; }
            set
            {
                m_day = value;
                if (m_edit != null)
                {
                    m_edit.SetParamExpr(CyParamNames.START_OF_WEEK, m_day.ToString());
                    m_edit.CommitParamExprs();
                }
            }
        }
        #endregion

        #region Constructor(s)
        public CyRTCParameters(ICyInstEdit_v1 edit)
        {
            m_edit = edit;
            LoadParameters();
        }
        #endregion

        #region Getting parameters
        public void LoadParameters()
        {
            if (m_edit != null)
            {
                try
                {
                    m_dstEnable = bool.Parse(m_edit.GetCommittedParam(CyParamNames.DST_ENABLE).Value);
                }
                catch (Exception) { }
                try
                {
                    m_day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Convert.ToString(m_edit.GetCommittedParam(
                    CyParamNames.START_OF_WEEK).Value));
                }
                catch (Exception) { }
            }
        }
        #endregion
    }
}
