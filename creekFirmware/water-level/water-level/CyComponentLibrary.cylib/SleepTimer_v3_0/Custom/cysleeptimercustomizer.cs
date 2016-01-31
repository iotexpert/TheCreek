/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SleepTimer_v3_0
{
    public partial class CySleepTimerCustomizer : UserControl, ICyParamEditingControl
    {
        private CySleepTimerParameters m_params;
        private ICyDeviceQuery_v1 m_deviceQuery;

        #region Constructor(s)
        public CySleepTimerCustomizer(CySleepTimerParameters inst, ICyDeviceQuery_v1 deviceQuery)
        {
            InitializeComponent();

            inst.m_basicTab = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;
            m_deviceQuery = deviceQuery;

            // Filtering intervalList values and assigning it to ComboBox
            for (int i = 0; i < m_params.m_intervalComboList.Count; i++)
            {
                m_params.m_intervalComboList[i] = m_params.m_intervalComboList[i].Replace(
                    CySleepTimerParameters.PREFIX, string.Empty);
                m_params.m_intervalComboList[i] = m_params.m_intervalComboList[i].Replace(
                    CySleepTimerParameters.POSTFIX, string.Empty);
            }
            comboBoxInterval.DataSource = m_params.m_intervalComboList;
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(CyCustomizer.BASIC_TABNAME))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }
            }

            // Show an error message and prevent customizer from closing 
            // for PSoC5 device if not allowed interval selected.
            if (m_deviceQuery.IsPSoC5 && m_deviceQuery.SiliconRevision < 2)
            {
                if (m_params.IsIntervalAllowedForPSoC5() == false)
                {
                    errs.Add(new CyCustErr(GetWakeupIntervalErrorMessage()));
                }
            }

            return errs;
        }
        #endregion

        #region Assigning parameters values to controls
        public void GetParams()
        {
            // EnableInt
            checkBoxInterrupt.Checked = m_params.EnableInt;

            // Interval
            string comboText = m_params.Interval.Replace(CySleepTimerParameters.PREFIX, string.Empty);
            comboText = comboText.Replace(CySleepTimerParameters.POSTFIX, string.Empty);
            comboBoxInterval.Text = comboText;
        }
        #endregion

        #region Assigning controls values to parameters
        public void SetEnableInterrupt()
        {
            m_params.EnableInt = checkBoxInterrupt.Checked;
            m_params.SetParams(CyParamNames.ENABLE_INT);
        }

        public void SetInterval()
        {
            m_params.Interval = CySleepTimerParameters.PREFIX + comboBoxInterval.SelectedValue.ToString()
                + CySleepTimerParameters.POSTFIX;
            m_params.SetParams(CyParamNames.INTERVAL);
        }
        #endregion

        #region Event handlers
        private void checkBoxInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            SetEnableInterrupt();
        }

        private void comboBoxInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_params.m_bGlobalEditMode)
                SetInterval();

            // For PSoC 5 the supported wakeup intervals are restricted to: 4, 8, 16, 32, 64, 128 or 256 ms.
            if (m_deviceQuery.SiliconRevisionAvailable)
            {
                if (m_deviceQuery.IsPSoC5 && m_deviceQuery.SiliconRevision < 2)
                {
                    if (m_params.IsIntervalAllowedForPSoC5())
                    {
                        errorProvider.SetError((Control)sender, string.Empty);
                    }
                    else
                    {
                        errorProvider.SetError((Control)sender, GetWakeupIntervalErrorMessage());
                    }
                }
            }
        }
        #endregion

        #region Error message(s)
        public static string GetWakeupIntervalErrorMessage()
        {
            return string.Format(Resources.WakeUpIntervalMsg,
                CySleepTimerParameters.ALLOWED_INTERVAL[0],
                CySleepTimerParameters.ALLOWED_INTERVAL[1],
                CySleepTimerParameters.ALLOWED_INTERVAL[2],
                CySleepTimerParameters.ALLOWED_INTERVAL[3],
                CySleepTimerParameters.ALLOWED_INTERVAL[4],
                CySleepTimerParameters.ALLOWED_INTERVAL[5],
                CySleepTimerParameters.ALLOWED_INTERVAL[6]);
        }
        #endregion
    }
}
