/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace PrISM_v1_50
{
    public enum E_CompareType
    {
        GreaterThan_or_Equal = 1,
        LessThan_or_Equal = 0
    }

    public class CyPRISMParameters
    {
        #region Private members        
        private UInt32 m_seedValue = 1;
        private UInt32 m_polyValue = 0;
        private UInt32 m_resolution = 2;
        private UInt32 m_density0 = 0;
        private UInt32 m_density1 = 0;
        private E_CompareType m_compareType0 = (E_CompareType)0;
        private E_CompareType m_compareType1 = (E_CompareType)0;
        private bool m_pulseTypeHardcoded = false;
        #endregion

        public ICyInstEdit_v1 m_inst;

        public const string SEED_VALUE = "SeedValue";
        public const string POLY_VALUE = "PolyValue";
        public const string RESOLUTION = "Resolution";
        public const string DENSITY0 = "Density0";
        public const string DENSITY1 = "Density1";
        public const string COMPARE_TYPE0 = "CompareType0";
        public const string COMPARE_TYPE1 = "CompareType1";
        public const string PULSE_TYPE_HARDCODED = "PulseTypeHardcoded";

        public const uint RESOLUTION_MIN = 2;
        public const uint RESOLUTION_MAX = 32;

        #region Fields
        public UInt32 SeedValue
        {
            get { return m_seedValue; }
            set
            {
                if (m_seedValue != value)
                {
                    m_seedValue = value;
                    SetParam(SEED_VALUE);
                    m_inst.CommitParamExprs();
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
                    SetParam(POLY_VALUE);
                    m_inst.CommitParamExprs();
                }
            }
        }

        public uint Resolution
        {
            get { return m_resolution; }
            set
            {
                if (m_resolution != value)
                {
                    m_resolution = value;
                    SetParam(RESOLUTION);
                    m_inst.CommitParamExprs();
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
                    SetParam(DENSITY0);
                    m_inst.CommitParamExprs();
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
                    SetParam(DENSITY1);
                    m_inst.CommitParamExprs();
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
                    SetParam(COMPARE_TYPE0);
                    m_inst.CommitParamExprs();
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
                    SetParam(COMPARE_TYPE1);
                    m_inst.CommitParamExprs();
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
                    SetParam(PULSE_TYPE_HARDCODED);
                    m_inst.CommitParamExprs();
                }
            }
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


        public void GetParams(string paramName)
        {
            if (m_inst != null)
                switch (paramName)
                {
                    case RESOLUTION:
                        UInt32.TryParse(m_inst.GetCommittedParam(RESOLUTION).Value, out m_resolution);
                        break;
                    case SEED_VALUE:
                        uint.TryParse(m_inst.GetCommittedParam(SEED_VALUE).Value, out m_seedValue);
                        break;
                    case POLY_VALUE:
                        UInt32.TryParse(m_inst.GetCommittedParam(POLY_VALUE).Value, out m_polyValue);
                        break;
                    case DENSITY0:
                        uint.TryParse(m_inst.GetCommittedParam(DENSITY0).Value, out m_density0);
                        break;
                    case DENSITY1:
                        uint.TryParse(m_inst.GetCommittedParam(DENSITY1).Value, out m_density1);
                        break;
                    case PULSE_TYPE_HARDCODED:
                        bool.TryParse(m_inst.GetCommittedParam(PULSE_TYPE_HARDCODED).Value, out m_pulseTypeHardcoded);
                        break;
                    case COMPARE_TYPE0:
                        CompareType0 = (E_CompareType)Enum.Parse(
                            typeof(E_CompareType),
                            m_inst.GetCommittedParam(COMPARE_TYPE0).Expr);
                        break;
                    case COMPARE_TYPE1:
                        CompareType1 = (E_CompareType)Enum.Parse(
                            typeof(E_CompareType),
                            m_inst.GetCommittedParam(COMPARE_TYPE1).Expr);
                        break;
                    default:
                        UInt32.TryParse(m_inst.GetCommittedParam(RESOLUTION).Value, out m_resolution);
                        uint.TryParse(m_inst.GetCommittedParam(SEED_VALUE).Value, out m_seedValue);
                        UInt32.TryParse(m_inst.GetCommittedParam(POLY_VALUE).Value, out m_polyValue);
                        uint.TryParse(m_inst.GetCommittedParam(DENSITY0).Value, out m_density0);
                        uint.TryParse(m_inst.GetCommittedParam(DENSITY1).Value, out m_density1);
                        bool.TryParse(m_inst.GetCommittedParam(PULSE_TYPE_HARDCODED).Value, out m_pulseTypeHardcoded);
                        CompareType0 = (E_CompareType)Enum.Parse(typeof(E_CompareType),
                            m_inst.GetCommittedParam(COMPARE_TYPE0).Expr);
                        CompareType1 = (E_CompareType)Enum.Parse(typeof(E_CompareType),
                            m_inst.GetCommittedParam(COMPARE_TYPE1).Expr);
                        break;
                }
        }

        private void SetParam(string paramName)
        {
            if (m_inst != null)
            {
                switch (paramName)
                {
                    case POLY_VALUE:
                        m_inst.SetParamExpr(POLY_VALUE, PolyValue.ToString() + "u");
                        break;
                    case SEED_VALUE:
                        m_inst.SetParamExpr(SEED_VALUE, SeedValue.ToString() + "u");
                        break;
                    case RESOLUTION:
                        m_inst.SetParamExpr(RESOLUTION, Resolution.ToString());
                        break;
                    case DENSITY0:
                        m_inst.SetParamExpr(DENSITY0, Density0.ToString() + "u");
                        break;
                    case DENSITY1:
                        m_inst.SetParamExpr(DENSITY1, Density1.ToString() + "u");
                        break;
                    case COMPARE_TYPE0:
                        m_inst.SetParamExpr(COMPARE_TYPE0, CompareType0.ToString());
                        break;
                    case COMPARE_TYPE1:
                        m_inst.SetParamExpr(COMPARE_TYPE1, CompareType1.ToString());
                        break;
                    case PULSE_TYPE_HARDCODED:
                        m_inst.SetParamExpr(PULSE_TYPE_HARDCODED, Convert.ToByte(PulseTypeHardcoded).ToString());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
