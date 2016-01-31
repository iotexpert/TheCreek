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
using System.Windows.Forms;
using System.Drawing;

namespace QuadDec_v1_20
{
    class CyParameters
    {
        private ICyInstEdit_v1 m_edit;
        private byte m_bit;
        private byte m_countRes;
        private bool m_useIndex;
        private bool m_useGF;

        public CyParameters(ICyInstEdit_v1 edit)
        {
            m_edit = edit;
        }

        #region Properties

        public byte CounterSize 
        {
            get
            {
                m_bit = Convert.ToByte(m_edit.GetCommittedParam("CounterSize").Value);
                return m_bit;
            }
            set
            {
                if (m_bit != value)
                {
                    m_bit = value;
                    m_edit.SetParamExpr("CounterSize", m_bit.ToString());
                    m_edit.CommitParamExprs();
                }
            }
        }

        public byte CounterResolution 
        {
            get
            {
                m_countRes = Convert.ToByte(m_edit.GetCommittedParam("CounterResolution").Value);
                return m_countRes;
            }
            set
            {
                if (m_countRes != value)
                {
                    m_countRes = value;
                    m_edit.SetParamExpr("CounterResolution", m_countRes.ToString());
                    m_edit.CommitParamExprs();
                }
            }
        }

        public bool UseIndex {
            get
            {
                m_useIndex = Convert.ToBoolean(m_edit.GetCommittedParam("UsingIndexInput").Value);
                return m_useIndex;
            }
            set
            {
                if (m_useIndex != value)
                {
                    m_useIndex = value;
                    m_edit.SetParamExpr("UsingIndexInput", Convert.ToInt16(m_useIndex).ToString());
                    m_edit.CommitParamExprs();
                }
            }
        }

        public bool UseGF {
            get
            {
                m_useGF = Convert.ToBoolean(m_edit.GetCommittedParam("UsingGlitchFiltering").Value);
                return m_useGF;
            }
            set
            {
                if (m_useGF != value)
                {
                    m_useGF = value;
                    m_edit.SetParamExpr("UsingGlitchFiltering", Convert.ToInt16(m_useGF).ToString());
                    m_edit.CommitParamExprs();
                }
            }
        }

        #endregion
    }
}
