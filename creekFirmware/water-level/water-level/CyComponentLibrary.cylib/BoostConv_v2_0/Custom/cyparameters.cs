/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;
using System.Diagnostics;

namespace BoostConv_v2_0
{
    public class CyParameters
    {
        public ICyInstEdit_v1 m_edit;

        private double m_inputVoltage;
        private byte m_outputCurrent;
        private byte m_outputVoltage;
        private int m_frequency;
        private int m_exClockSrc;
        private bool m_disableAutoBattery;

        public const string P_CONFIG_PARAMETERS_TAB_NAME = "Config";

        public const string INPUT_VOLTAGE = "InputVoltage";
        public const string OUTPUT_CURRENT = "OutCurrent";
        public const string OUTPUT_VOLTAGE = "OutVoltage";
        public const string FREQUENCY = "Frequency";
        public const string EXTERNAL_CLOCK_SRC = "ExtClk_Source";
        public const string DISABLE_AUTO_BATTERY = "DisableAutoBattery";
 
        public CyParameters(ICyInstEdit_v1 edit)
        {
            m_edit = edit;
            LoadParams();
        }

        public double InputVoltage
        {
            get { return m_inputVoltage; }
            set
            {
                if (m_inputVoltage != value)
                {
                    m_inputVoltage = value;
                    SaveParam(INPUT_VOLTAGE, m_inputVoltage.ToString());
                }
            }
        }

        public byte OutCurrent
        {
            get { return m_outputCurrent; }
            set
            {
                if (m_outputCurrent != value)
                {
                    m_outputCurrent = value;
                    SaveParam(OUTPUT_CURRENT, m_outputCurrent.ToString());
                }
            }
        }

        public byte OutVoltage
        {
            get { return m_outputVoltage; }
            set
            {
                if (m_outputVoltage != value)
                {
                    m_outputVoltage = value;
                    SaveParam(OUTPUT_VOLTAGE, m_outputVoltage.ToString());
                }
            }
        }

        public int Frequency
        {
            get { return m_frequency; }
            set
            {
                if (m_frequency != value)
                {
                    m_frequency = value;
                    SaveParam(FREQUENCY, m_frequency.ToString());
                }
            }
        }

        public int ExternalClockSrc
        {
            get { return m_exClockSrc; }
            set
            {
                if (m_exClockSrc != value)
                {
                    m_exClockSrc = value;
                    SaveParam(EXTERNAL_CLOCK_SRC, m_exClockSrc.ToString());
                }
            }
        }

        public bool DisableAutoBattery
        {
            get { return m_disableAutoBattery; }
            set 
            {
                if (m_disableAutoBattery != value)
                {
                    m_disableAutoBattery = value;
                    SaveParam(DISABLE_AUTO_BATTERY, m_disableAutoBattery.ToString());
                }
            }
        }

        public void LoadParams()
        {
            if (m_edit != null)
            {
                CyCustErr err;

                CyCompDevParam param = m_edit.GetCommittedParam(INPUT_VOLTAGE);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<double>(out m_inputVoltage);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(OUTPUT_CURRENT);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<byte>(out m_outputCurrent);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(OUTPUT_VOLTAGE);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<byte>(out m_outputVoltage);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(FREQUENCY);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<int>(out m_frequency);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(EXTERNAL_CLOCK_SRC);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<int>(out m_exClockSrc);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(DISABLE_AUTO_BATTERY);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<bool>(out m_disableAutoBattery);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);
            }
        }


        private void SaveParam(String paramName, String paramValue)
        {
            if (m_edit != null)
            {
                m_edit.SetParamExpr(paramName, paramValue);
                m_edit.CommitParamExprs();
            }
        }
    }
}
