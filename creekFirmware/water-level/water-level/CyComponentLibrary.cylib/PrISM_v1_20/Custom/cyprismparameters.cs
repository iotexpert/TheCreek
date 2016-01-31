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

namespace PrISM_v1_20
{
    public enum E_CompareType
    {
        GreaterThan_or_Equal = 1,
        LessThan_or_Equal = 0
    }

    public class CyPRISMParameters
    {
        #region Private members
        private ICyInstEdit_v1 m_inst;
        private UInt32 m_seedValue = 1;
        private UInt32 m_polyValue = 0;
        private int m_resolution = 2;
        private UInt32 m_density0 = 0;
        private UInt32 m_density1 = 0;
        private E_CompareType m_compareType0 = (E_CompareType)0;
        private E_CompareType m_compareType1 = (E_CompareType)0;
        private bool m_pulseTypeHardcoded = false;
        private bool m_GlobalEditMode=false;
        #endregion

        #region Fields
        public UInt32 SeedValue
        {
            get { return m_seedValue; }
            set
            {
                if (m_seedValue != value)
                {
                    m_seedValue = value;
                    SetParam("SeedValue");
                    CommitParams();
                }
            }
        }

        public UInt32 PolyValue
        {
            get { return m_polyValue; }
            set
            {
                if (m_polyValue != value)
                {
                    m_polyValue = value;
                    SetParam("PolyValue");
                    CommitParams();
                }
            }
        }

        public int Resolution
        {
            get { return m_resolution; }
            set
            {
                if (m_resolution != value)
                {
                    m_resolution = value;
                    SetParam("Resolution");
                    CommitParams();
                }
            }
        }

        public UInt32 Density0
        {
            get { return m_density0; }
            set
            {
                if (m_density0 != value)
                {
                    m_density0 = value;
                    SetParam("Density0");
                    CommitParams();
                }
            }
        }

        public UInt32 Density1
        {
            get { return m_density1; }
            set
            {
                if (m_density1 != value)
                {
                    m_density1 = value;
                    SetParam("Density1");
                    CommitParams();
                }
            }
        }

        public E_CompareType CompareType0
        {
            get { return m_compareType0; }
            set
            {
                if (m_compareType0 != value)
                {
                    m_compareType0 = value;
                    SetParam("CompareType0");
                    CommitParams();
                }
            }
        }

        public E_CompareType CompareType1
        {
            get { return m_compareType1; }
            set
            {
                if (m_compareType1 != value)
                {
                    m_compareType1 = value;
                    SetParam("CompareType1");
                    CommitParams();
                }
            }
        }

        public bool PulseTypeHardcoded
        {
            get { return m_pulseTypeHardcoded; }
            set
            {
                if (m_pulseTypeHardcoded != value)
                {
                    m_pulseTypeHardcoded = value;
                    SetParam("PulseTypeHardcoded");
                    CommitParams();
                }
            }
        }

        public bool GlobalEditMode
        {
            get { return m_GlobalEditMode; }
            set { m_GlobalEditMode = value; }
        }
        #endregion

        #region Ctors
        public CyPRISMParameters()
        {
        }

        public CyPRISMParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
            GetParams(null);            
        }
        #endregion


        public void GetParams(CyCompDevParam param)
        {
            Resolution = Convert.ToInt32(GetValue(param, "Resolution", Resolution));
            SeedValue = Convert.ToUInt32(GetValue(param, "SeedValue", SeedValue));
            PolyValue = Convert.ToUInt32(GetValue(param, "PolyValue", PolyValue));
            Density0 = Convert.ToUInt32(GetValue(param, "Density0", Density0));
            Density1 = Convert.ToUInt32(GetValue(param, "Density1", Density1));
            PulseTypeHardcoded = Convert.ToBoolean(
                GetValue(param, "PulseTypeHardcoded", PulseTypeHardcoded));
            CompareType0 = (E_CompareType)Enum.Parse(typeof(E_CompareType), 
                GetExpr(param, "CompareType0", CompareType0));
            CompareType1 = (E_CompareType)Enum.Parse(typeof(E_CompareType),
                GetExpr(param, "CompareType1",CompareType1));
        }


        private void SetParam(string paramName)
        {
            try
            {
                if (m_inst != null)
                {
                    switch (paramName)
                    {
                        case "PolyValue":
                            m_inst.SetParamExpr("PolyValue", PolyValue.ToString() + "u");
                            break;
                        case "SeedValue":
                            m_inst.SetParamExpr("SeedValue", SeedValue.ToString() + "u");
                            break;
                        case "Resolution":
                            m_inst.SetParamExpr("Resolution", Resolution.ToString());
                            break;
                        case "Density0":
                            m_inst.SetParamExpr("Density0", Density0.ToString() + "u");
                            break;
                        case "Density1":
                            m_inst.SetParamExpr("Density1", Density1.ToString() + "u");
                            break;
                        case "CompareType0":                            
                                m_inst.SetParamExpr("CompareType0", CompareType0.ToString());
                            break;
                        case "CompareType1":
                            m_inst.SetParamExpr("CompareType1", CompareType1.ToString());
                            break;
                        case "PulseTypeHardcoded":
                            m_inst.SetParamExpr("PulseTypeHardcoded", Convert.ToByte(PulseTypeHardcoded).ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error was occur in SetParam method");
            }
        }

        private void CommitParams()
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
        string GetValue(CyCompDevParam param, string param_name, object obj)
        {
            if (ParameterChanged(param, param_name))
                return m_inst.GetCommittedParam(param_name).Value.ToString();
            return obj.ToString();
        }
        string GetExpr(CyCompDevParam param, string param_name, object obj)
        {
            if (ParameterChanged(param, param_name))
                return m_inst.GetCommittedParam(param_name).Expr.ToString();
            return obj.ToString();
        }
        bool ParameterChanged(CyCompDevParam param, string param_name)
        {
            if (param != null)
                if (param.Name != param_name) return false;
            return true;
        }

    }
}
