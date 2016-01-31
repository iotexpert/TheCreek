/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace ResistiveTouch_v1_10
{
    public class CyParameters
    {
        public const string PARAM_SARADC = "ADC_Select";
        private CyEResistiveTouchADCSelType m_sarAdc;

        public enum CyEResistiveTouchADCSelType { DelSig_ADC, SAR_ADC };

        public readonly ICyInstEdit_v1 m_inst;

        public CyEResistiveTouchADCSelType SarAdc
        {
            get { return m_sarAdc; }
            set
            {
                if (value != m_sarAdc)
                {
                    m_sarAdc = value;
                    SetParam(PARAM_SARADC);
                }
            }
        }

        public CyParameters()
        {
        }

        public CyParameters(ICyInstEdit_v1 inst)
            : this()
        {
            m_inst = inst;
            GetParams(m_inst);
        }
        public void GetParams(ICyInstQuery_v1 inst)
        {
            if (inst != null)
            {
                inst.GetCommittedParam(PARAM_SARADC).TryGetValueAs(out m_sarAdc);
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        public void SetParam(string paramName)
        {
            if (m_inst != null)
            {
                switch (paramName)
                {
                    case PARAM_SARADC:
                        m_inst.SetParamExpr(PARAM_SARADC, SarAdc.ToString());
                        break;
                    default:
                        break;
                }
                CommitParams();
            }
            else
            {
                // Do nothing
            }
        }
        public void CommitParams()
        {
            if (m_inst != null)
            {
                if (!m_inst.CommitParamExprs())
                    ShowAllErrors();
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        private void ShowAllErrors()
        {
            foreach (string paramName in m_inst.GetParamNames())
            {
                CyCompDevParam curParam = m_inst.GetCommittedParam(paramName);
                if (curParam.ErrorCount > 0) ShowParamErrors(curParam);
            }
        }

        private void ShowParamErrors(CyCompDevParam param)
        {
            MessageBox.Show(
                param.ErrorMsgs,
                param.Name + " CommitParams errors",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
