/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;

namespace Debouncer_v1_0
{
    public class CyParamNames
    {
        public const string POSITIVE_EDGE_DETECT = "PosEdgeDetect";
        public const string NEGATIVE_EDGE_DETECT = "NegEdgeDetect";
        public const string EITHER_EDGE_DETECT = "EitherEdgeDetect";
        public const string SIGNAL_WIDTH = "SignalWidth";
    }

    public class CyParamRanges
    {
        public const byte MIN_SIGNAL_WIDTH = 1;
        public const byte MAX_SIGNAL_WIDTH = 32;
    }

    public class CyParameters
    {
        public ICyInstQuery_v1 m_inst;
        public ICyTabbedParamEditor m_editor;
        public ICyTerminalQuery_v1 m_term;

        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        private bool m_bGlobalEditMode = false;

        public bool GlobalEditMode
        {
            get { return m_bGlobalEditMode; }
            set { m_bGlobalEditMode = value; }
        }

        #region Constructor(s)
        public CyParameters(ICyInstQuery_v1 inst)
        {
            m_inst = inst;
        }
        #endregion

        #region Getting Parameters
        private T GetValue<T>(string paramName)
        {
            try
            {
                T value;
                CyCustErr err = m_inst.GetCommittedParam(paramName).TryGetValueAs<T>(out value);
                if (err != null && err.IsOK)
                {
                    return value;
                }
                else
                {
                    return default(T);
                }
            }
            catch { }
            return default(T);
        }
        #endregion

        #region Setting Parameters
        private void SetValue<T>(string paramName, T value)
        {
            if (m_bGlobalEditMode)
            {
                if ((m_inst is ICyInstEdit_v1) == false) return;

                string valueToSet = value.ToString();
                if (value is bool)
                    valueToSet = valueToSet.ToLower();
                if ((m_inst as ICyInstEdit_v1).SetParamExpr(paramName, valueToSet.ToString()))
                {
                    (m_inst as ICyInstEdit_v1).CommitParamExprs();
                }
            }
        }
        #endregion

        #region Class properties
        public bool PositiveEdge
        {
            get { return GetValue<bool>(CyParamNames.POSITIVE_EDGE_DETECT); }
            set { SetValue(CyParamNames.POSITIVE_EDGE_DETECT, value); }
        }

        public bool NegativeEdge
        {
            get { return GetValue<bool>(CyParamNames.NEGATIVE_EDGE_DETECT); }
            set { SetValue(CyParamNames.NEGATIVE_EDGE_DETECT, value); }
        }

        public bool EitherEdge
        {
            get { return GetValue<bool>(CyParamNames.EITHER_EDGE_DETECT); }
            set { SetValue(CyParamNames.EITHER_EDGE_DETECT, value); }
        }

        public byte SignalWidth
        {
            get { return GetValue<byte>(CyParamNames.SIGNAL_WIDTH); }
            set { SetValue(CyParamNames.SIGNAL_WIDTH, value); }
        }
        #endregion
    }
}
