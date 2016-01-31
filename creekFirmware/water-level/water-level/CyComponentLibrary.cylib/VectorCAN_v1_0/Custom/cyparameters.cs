using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace VectorCAN_v1_0
{
    public class CyParameters
    {
        public ICyInstEdit_v1 m_edit;
        public ICyTerminalQuery_v1 m_term;
        public static bool GLOBAL_EDIT_MODE = false;
        public const string P_ADD_TRANSCEIVER_ENABLE_SIGNAL = "AddTransceiverEnableSignal";
        public const string P_ENABLE_INTERRUPTS = "EnableInterrupts";
        public const string BUS_CLOCK_NAME = "BUS_CLK";

        public bool? AddTransceiverEnableSignal
        {
            get
            {
                bool val;
                if (GetValue<bool>(P_ADD_TRANSCEIVER_ENABLE_SIGNAL, out val))
                {
                    return val;
                }
                else return null;
            }
            set
            {
                if (value != null && m_edit != null && GLOBAL_EDIT_MODE)
                {
                    m_edit.SetParamExpr(P_ADD_TRANSCEIVER_ENABLE_SIGNAL, value.ToString());
                    m_edit.CommitParamExprs();
                }
            }
        }
        public bool? EnableInterrupts
        {
            get
            {
                bool val;
                if (GetValue<bool>(P_ENABLE_INTERRUPTS, out val))
                {
                    return val;
                }
                else return null;
            }
            set 
            {
                if (value != null && m_edit != null && GLOBAL_EDIT_MODE)
                {
                    m_edit.SetParamExpr(P_ENABLE_INTERRUPTS, value.ToString());
                    m_edit.CommitParamExprs();
                }
            }
        }

        public CyParameters(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 term)
        {
            m_edit = edit;
            m_term = term;
        }
        private bool GetValue<T>(string paramName, out T value)
        {
            if (m_edit != null)
            {
                CyCompDevParam param = m_edit.GetCommittedParam(paramName);
                if (param != null)
                {
                    if (param.TryGetValueAs<T>(out value) == CyCustErr.OK && new List<string>(param.Errors).Count == 0)
                        return true;
                }
                if (param == null || param.ErrorCount == 0)
                    System.Diagnostics.Debug.Assert(false, string.Format(cvresources.GettingPrmFailed, paramName));
            }
            value = default(T);
            return false;
        }
    }
}
