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

namespace PrISM_v1_10
{
    public enum E_CompareType
    {
        GreaterThan_or_Equal = 1,
        LessThan_or_Equal = 0
    }

    public class CyPRISMParameters
    {
        ICyInstEdit_v1 m_inst;

        public UInt32 m_SeedValue = 1;
        public UInt32 m_PolyValue = 0;
        public int m_Resolution = 2;
        public UInt32 m_Density0 = 0;
        public UInt32 m_Density1 = 0;
        public E_CompareType m_CompareType0 = (E_CompareType)0;
        public E_CompareType m_CompareType1 = (E_CompareType)0;
        public bool m_PulseTypeHardcoded = false;

        bool m_GlobalEditMode=false;
        public bool GlobalEditMode
        {
            get
            {
                return m_GlobalEditMode;
            }
            set
            {
                m_GlobalEditMode = value;
            }
               
        }

        public CyPRISMParameters()
        {
        }

        public CyPRISMParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
            this.m_inst = inst;
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            m_Resolution = Convert.ToInt32(inst.GetCommittedParam("Resolution").Value);
            m_SeedValue = Convert.ToUInt32(inst.GetCommittedParam("SeedValue").Value);
            m_PolyValue = Convert.ToUInt32(inst.GetCommittedParam("PolyValue").Value);
            m_Density0 = Convert.ToUInt32(inst.GetCommittedParam("Density0").Value);
            m_Density1 = Convert.ToUInt32(inst.GetCommittedParam("Density1").Value);
            //CyCompDevParam param = inst.GetCommittedParam("PulseTypeHardcoded");
            m_PulseTypeHardcoded =Convert.ToBoolean((
                inst.GetCommittedParam("PulseTypeHardcoded").Value));
            m_CompareType0 = (E_CompareType)Enum.Parse(typeof(E_CompareType), 
                inst.GetCommittedParam("CompareType0").Expr.ToString());
            m_CompareType1 = (E_CompareType)Enum.Parse(typeof(E_CompareType), 
                inst.GetCommittedParam("CompareType1").Expr.ToString());
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
                            m_inst.SetParamExpr("PolyValue", m_PolyValue.ToString() + "u");
                            break;
                        case "SeedValue":
                            m_inst.SetParamExpr("SeedValue", m_SeedValue.ToString() + "u");
                            break;
                        case "Resolution":
                            m_inst.SetParamExpr("Resolution", m_Resolution.ToString());
                            break;
                        case "Density0":
                            m_inst.SetParamExpr("Density0", m_Density0.ToString() + "u");
                            break;
                        case "Density1":
                            m_inst.SetParamExpr("Density1", m_Density1.ToString() + "u");
                            break;
                        case "CompareType0":                            
                                m_inst.SetParamExpr("CompareType0", m_CompareType0.ToString());
                            break;
                        case "CompareType1":
                            m_inst.SetParamExpr("CompareType1", m_CompareType1.ToString());
                            break;
                        case "PulseTypeHardcoded":
                            m_inst.SetParamExpr("PulseTypeHardcoded", Convert.ToByte(m_PulseTypeHardcoded).ToString());
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
                m_inst.SetParamExpr("SeedValue", m_SeedValue.ToString() + "u");
                m_inst.SetParamExpr("PolyValue", m_PolyValue.ToString() + "u");
                m_inst.SetParamExpr("Resolution", m_Resolution.ToString());
                m_inst.SetParamExpr("Density0", m_Density0.ToString());
                m_inst.SetParamExpr("Density1", m_Density1.ToString());
                m_inst.SetParamExpr("PulseTypeHardcoded", Convert.ToByte(m_PulseTypeHardcoded).ToString());
                m_inst.SetParamExpr("CompareType0", m_CompareType0.ToString());
                m_inst.SetParamExpr("CompareType1", m_CompareType1.ToString());
            }
        }

        public void CommitParams()
        {
            if (m_inst != null)
            {
                if (!m_inst.CommitParamExprs())
                {
                    if (m_inst.GetCommittedParam("SeedValue").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("SeedValue").ErrorMsgs);

                    if (m_inst.GetCommittedParam("PolyValue").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("PolyValue").ErrorMsgs);

                    if (m_inst.GetCommittedParam("Resolution").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("Resolution").ErrorMsgs);

                    if (m_inst.GetCommittedParam("Density0").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("Density0").ErrorMsgs);

                    if (m_inst.GetCommittedParam("Density1").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("Density1").ErrorMsgs);

                    if (m_inst.GetCommittedParam("PolyValue").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("PolyValue").ErrorMsgs);

                    if (m_inst.GetCommittedParam("PolyValue").ErrorCount > 0)
                        MessageBox.Show(m_inst.GetCommittedParam("PolyValue").ErrorMsgs);
                }
            }
        }
    }
}
