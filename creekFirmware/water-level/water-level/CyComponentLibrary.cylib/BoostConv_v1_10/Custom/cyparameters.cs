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

namespace BoostConv_v1_10
{
    class CyParameters
    {
        private ICyInstEdit_v1 m_edit;
        private Dictionary<string, string> m_outVConvertor;
 
        public CyParameters(ICyInstEdit_v1 edit)
        {
            m_edit = edit;
            m_outVConvertor = new Dictionary<string, string>();
            m_outVConvertor.Add("0", "0");
            m_outVConvertor.Add("3", "1.8");
            m_outVConvertor.Add("4", "1.9");
            m_outVConvertor.Add("5", "2");
            m_outVConvertor.Add("6", "2.1");
            m_outVConvertor.Add("7", "2.2");
            m_outVConvertor.Add("8", "2.3");
            m_outVConvertor.Add("9", "2.4");
            m_outVConvertor.Add("10", "2.5");
            m_outVConvertor.Add("11", "2.6");
            m_outVConvertor.Add("12", "2.7");
            m_outVConvertor.Add("13", "2.8");
            m_outVConvertor.Add("14", "2.9");
            m_outVConvertor.Add("15", "3");
            m_outVConvertor.Add("16", "3.1");
            m_outVConvertor.Add("17", "3.2");
            m_outVConvertor.Add("18", "3.3");
            m_outVConvertor.Add("19", "3.4");
            m_outVConvertor.Add("20", "3.5");
            m_outVConvertor.Add("21", "3.6");
            m_outVConvertor.Add("22", "4");
            m_outVConvertor.Add("23", "4.25");
            m_outVConvertor.Add("24", "4.5");
            m_outVConvertor.Add("25", "4.75");
            m_outVConvertor.Add("26", "5");
            m_outVConvertor.Add("27", "5.25");
        }

        public void SetInputVoltage(string inputVoltage)
        {
            if (!m_edit.SetParamExpr("InputVoltage", inputVoltage))
            {
                throw new Exception("SetParamExpr return false. InputVoltage = " + inputVoltage);
            }
            CommitParameters();
        }

        public void SetOutCurrent(string outCurrent)
        {
            if (!m_edit.SetParamExpr("OutCurrent", outCurrent))
            {
                throw new Exception("SetParamExpr return false. outCurrent = " + outCurrent);
            }
            CommitParameters();
        }

        public void SetOutVoltage(string outVlotage)
        {
            string valueForStore = string.Empty;

            foreach (string key in m_outVConvertor.Keys)
            {
                if (m_outVConvertor[key] == outVlotage)
                {
                    valueForStore = key;
                    break;
                }
            }

            if (valueForStore != string.Empty)
            {
                if (!m_edit.SetParamExpr("OutVoltage", valueForStore))
                {
                    throw new Exception("SetParamExpr return false. OutVoltage = " + valueForStore);
                }
                CommitParameters();
            }
            else
            {
                throw new Exception("valueForStore == string.Empty");
            }

        }

        public void SetFrequency(int freq)
        {
            if (!m_edit.SetParamExpr("Frequency", freq.ToString()))
            {
                throw new Exception("SetParamExpr return false. Frequency = " + freq);
            }
            CommitParameters();
        }

        public string GetInputVoltage()
        {
            return m_edit.GetCommittedParam("InputVoltage").Value;
        }

        public string GetOutCurrent()
        {
            return m_edit.GetCommittedParam("OutCurrent").Value;
        }

        public int GetOutVlotageIndex()
        {
            int result = -1;
            string storedValue = m_edit.GetCommittedParam("OutVoltage").Value;
            
            if (m_outVConvertor.ContainsKey(storedValue))
            {
                result = new List<string>(m_outVConvertor.Keys).IndexOf(storedValue);
            }
            else
            {
                throw new Exception("m_outVConvertor.ContainsKey(" + storedValue + ") return false");
            }

            return result;
        }

        public int GetFrequency()
        {
            return int.Parse(m_edit.GetCommittedParam("Frequency").Value);
        }

        private void CommitParameters()
        {
            if (!m_edit.CommitParamExprs())
            {
                throw new Exception("CommitParamExprs return false.");
            }
        }

    }
}
