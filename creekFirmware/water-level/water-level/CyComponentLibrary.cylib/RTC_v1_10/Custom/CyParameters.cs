/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace RTC_v1_10
{
    class CyParameters
    {
        private ICyInstEdit_v1 m_edit;
        private bool m_dstEnable;
        private DayOfWeek m_day;

        public bool DstEnable
        {
            get { return m_dstEnable; }
            set
            {
                m_dstEnable = value;
                m_edit.SetParamExpr("DstEnable", m_dstEnable.ToString());
                m_edit.CommitParamExprs();
            }
        }

        public DayOfWeek StartDayOfWeek
        {
            get { return m_day; }
            set
            {
                m_day = value;
                m_edit.SetParamExpr("StartOfWeek", m_day.ToString());
                m_edit.CommitParamExprs();
            }
        }

        public CyParameters(ICyInstEdit_v1 edit)
        {
            m_edit = edit;
            LoadParameters();
        }

        private void LoadParameters()
        {
            if (m_edit != null)
            {
                m_dstEnable = Convert.ToBoolean(m_edit.GetCommittedParam("DstEnable").Value);
                m_day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Convert.ToString(m_edit.GetCommittedParam("StartOfWeek").Value));
            }
        }
    }
}
