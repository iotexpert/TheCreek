/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace SleepTimer_v1_50
{
    public partial class CySleepTimerCustomizer : UserControl, ICyParamEditingControl
    {
        private CySleepTimerParameters m_params;

        private const string PREFIX = "CTW_";
        private const string POSTFIX = "_MS";

        #region Constructor(s)
        public CySleepTimerCustomizer(CySleepTimerParameters inst)
        {
            InitializeComponent();

            inst.m_basicTab = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;

            // Filtering intervalList values and assigning it to ComboBox
            for (int i = 0; i < m_params.m_intervalComboList.Count; i++)
            {
                m_params.m_intervalComboList[i] = m_params.m_intervalComboList[i].Replace(PREFIX, "");
                m_params.m_intervalComboList[i] = m_params.m_intervalComboList[i].Replace(POSTFIX, "");
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

            return errs;
        }
        #endregion

        #region Assigning parameters values to controls
        public void GetParams()
        {
            // EnableInt
            checkBoxInterrupt.Checked = m_params.EnableInt;

            // Interval
            string comboText = m_params.Interval.Replace(PREFIX, "");
            comboText = comboText.Replace(POSTFIX, "");
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
            m_params.Interval = PREFIX + comboBoxInterval.SelectedValue.ToString() + POSTFIX;
            m_params.SetParams(CyParamNames.INTERVAL);
        }
        #endregion

        #region Event Handlers
        private void checkBoxInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            SetEnableInterrupt();
        }

        private void comboBoxInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_params.m_bGlobalEditMode)
                SetInterval();
        }
        #endregion
    }
}
