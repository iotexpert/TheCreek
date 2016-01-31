/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;

namespace PRS_v1_30
{
    public class CyPRSParameters
    {
        private ICyInstEdit_v1 m_inst;

        public UInt64 m_SeedValue = 1;
        public UInt64 m_PolyValue = 0;
        public int m_RunMode = 1;
        public int m_Resolution = 2;
        public bool m_editMode = false;

        public CyPRSParameters()
        {
        }

        public CyPRSParameters(ICyInstEdit_v1 inst)
        {            
            this.m_inst = inst;
            m_editMode = true;
            GetParams(null);
        }

        public void GetParams(CyCompDevParam param)
        {
            m_Resolution = Convert.ToInt32(GetValue(param, "Resolution", m_Resolution));
            if (m_Resolution > 32)
            {
                if ((ParameterChanged(param, "SeedValueUpper")) || (ParameterChanged(param, "SeedValueLower")))
                    m_SeedValue = (Convert.ToUInt64(m_inst.GetCommittedParam("SeedValueUpper").Value) << 32) +
                                Convert.ToUInt64(m_inst.GetCommittedParam("SeedValueLower").Value);
                if ((ParameterChanged(param, "PolyValueUpper")) || (ParameterChanged(param, "PolyValueLower")))
                    m_PolyValue = (Convert.ToUInt64(m_inst.GetCommittedParam("PolyValueUpper").Value) << 32) +
                                Convert.ToUInt64(m_inst.GetCommittedParam("PolyValueLower").Value);
            }
            else
            {
                m_SeedValue = Convert.ToUInt32(GetValue(param, "SeedValueLower", m_SeedValue));
                m_PolyValue = Convert.ToUInt32(GetValue(param, "PolyValueLower", m_PolyValue));
            }
            m_RunMode = Convert.ToInt32(GetValue(param, "RunMode", m_RunMode));
        }
        string GetValue(CyCompDevParam param, string param_name, object obj)
        {
            if (ParameterChanged(param, param_name))
                return m_inst.GetCommittedParam(param_name).Value.ToString();
            return obj.ToString();
        }
        bool ParameterChanged(CyCompDevParam param, string param_name)
        {
            if (param != null)
                if (param.Name != param_name) return false;
            return true;
        }

        public void SetParam(string paramName)
        {
            try
            {
                if (m_editMode)
                    if (m_inst != null)
                    {
                        switch (paramName)
                        {
                            case "PolyValue":
                                m_inst.SetParamExpr("PolyValueUpper", (m_PolyValue >> 32).ToString() + "u");
                                m_inst.SetParamExpr("PolyValueLower", (m_PolyValue & 0xFFFFFFFF).ToString() + "u");
                                break;
                            case "SeedValue":
                                m_inst.SetParamExpr("SeedValueUpper", (m_SeedValue >> 32).ToString() + "u");
                                m_inst.SetParamExpr("SeedValueLower", (m_SeedValue & 0xFFFFFFFF).ToString() + "u");                                
                                break;
                            case "Resolution":
                                m_inst.SetParamExpr("Resolution", m_Resolution.ToString());
                                break;
                            case "RunMode":
                                if (m_RunMode == 0)
                                {
                                    m_inst.SetParamExpr("RunMode", "Clocked");
                                }
                                else
                                {
                                    m_inst.SetParamExpr("RunMode", "APISingleStep");
                                }
                                break;
                            default:
                                break;
                        }
                    }
            }
            catch
            {
            }
        }

        public void SetParams()
        {
            if (m_inst != null)
            {
                m_inst.SetParamExpr("SeedValueUpper", (m_SeedValue >> 32).ToString() + "u");
                m_inst.SetParamExpr("SeedValueLower", (m_SeedValue & 0xFFFFFFFF).ToString() + "u");
                m_inst.SetParamExpr("PolyValueUpper", (m_PolyValue >> 32).ToString() + "u");
                m_inst.SetParamExpr("PolyValueLower", (m_PolyValue & 0xFFFFFFFF).ToString() + "u");
                if (m_RunMode == 0)
                    m_inst.SetParamExpr("RunMode", "Clocked");
                else
                    m_inst.SetParamExpr("RunMode", "APISingleStep");
                m_inst.SetParamExpr("Resolution", m_Resolution.ToString());
            }
        }

        public void CommitParams()
        {
            if (m_inst != null)
            {                
                if (!m_inst.CommitParamExprs())
                {
                    if (m_inst.GetCommittedParam("SeedValueLower").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("SeedValueLower").ErrorMsgs);

                    if (m_inst.GetCommittedParam("PolyValueLower").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("PolyValueLower").ErrorMsgs);

                    if (m_inst.GetCommittedParam("PolyValueUpper").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("PolyValueUpper").ErrorMsgs);
                }
            }
        }
    }
}
