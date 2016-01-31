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
using System.Windows.Forms;
using System.Drawing;

namespace QuadDec_v2_10
{
    class CyParameters
    {
        public ICyInstEdit_v1 m_instance;

        private byte m_counterSize;
        private byte m_counterRes;
        private bool m_useIndex;
        private bool m_useGF;

        public const string COUNTER_SIZE = "CounterSize";
        private const string COUNTER_RESOLUTION = "CounterResolution";
        private const string USING_INDEX_INPUT = "UsingIndexInput";
        private const string USING_GL_FILTERING = "UsingGlitchFiltering";

        public CyParameters(ICyInstEdit_v1 edit)
        {
            m_instance = edit;
            LoadParam();
        }

        #region Properties

        public byte CounterSize 
        {
            get { return m_counterSize; }
            set
            {
                m_counterSize = value;
                SaveParam(COUNTER_SIZE, value);
            }
        }

        public byte CounterResolution 
        {
            get { return m_counterRes; }
            set
            {
                m_counterRes = value;
                SaveParam(COUNTER_RESOLUTION, value);
            }
        }

        public bool UseIndex {
            get { return m_useIndex; }
            set
            {
                m_useIndex = value;
                SaveParam(USING_INDEX_INPUT, value);
            }
        }

        public bool UseGF {
            get { return m_useGF; }
            set
            {
                m_useGF = value;
                SaveParam(USING_GL_FILTERING, value);
            }
        }

        #endregion

        public void LoadParam()
        {
            m_instance.GetCommittedParam(COUNTER_SIZE).TryGetValueAs<byte>(out m_counterSize);
            m_instance.GetCommittedParam(COUNTER_RESOLUTION).TryGetValueAs<byte>(out m_counterRes);
            m_instance.GetCommittedParam(USING_INDEX_INPUT).TryGetValueAs<bool>(out m_useIndex);
            m_instance.GetCommittedParam(USING_GL_FILTERING).TryGetValueAs<bool>(out m_useGF);
        }

        private void SaveParam(string pName, object pValue)
        {
            m_instance.SetParamExpr(pName, pValue.ToString());
            m_instance.CommitParamExprs();

            if (m_instance.GetCommittedParam(pName).ErrorCount != 0)
            {
                string errors = m_instance.GetCommittedParam(pName).ErrorMsgs;
                MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}",
                    pName,
                    pValue,
                    errors),
                    "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
