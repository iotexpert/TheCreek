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

namespace CyCustomizer.PRS_v1_10
{
    public class CyPRSParameters
    {
        private ICyInstEdit_v1 m_inst;

        public UInt64 m_SeedValue = 1;
        public UInt64 m_PolyValue = 0;
        public int m_RunMode = 1;
        public int m_Resolution = 2;

        public CyPRSParameters()
        {
        }

        public CyPRSParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
            this.m_inst = inst;
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_Resolution = Convert.ToInt32(inst.GetCommittedParam("Resolution").Value);
            if (m_Resolution > 32)
            {
                m_SeedValue = (Convert.ToUInt64(inst.GetCommittedParam("SeedValueUpper").Value) << 32) +
                            Convert.ToUInt64(inst.GetCommittedParam("SeedValueLower").Value);
                m_PolyValue = (Convert.ToUInt64(inst.GetCommittedParam("PolyValueUpper").Value) << 32) +
                            Convert.ToUInt64(inst.GetCommittedParam("PolyValueLower").Value);
            }
            else
            {
                m_SeedValue = Convert.ToUInt32(inst.GetCommittedParam("SeedValueLower").Value);
                m_PolyValue = Convert.ToUInt32(inst.GetCommittedParam("PolyValueLower").Value);
            }
            m_RunMode = Convert.ToInt32(inst.GetCommittedParam("RunMode").Value);
            
        }

        public void SetParam(string paramName)
        {
            try
            {
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
