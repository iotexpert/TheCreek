/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace SleepTimer_v1_10
{
    public partial class CySleepTimerCustomizer : UserControl, ICyParamEditingControl
    {
        private Control m_control;
        private CySleepTimerParameters m_params;

        private const string m_prefix = "CTW_";
        private const string m_postfix = "_MS";

        public CySleepTimerCustomizer(CySleepTimerParameters inst)
        {
            InitializeComponent();

            ((CySleepTimerParameters)inst).m_basicTab = this;
            m_control = this;
            this.Dock = DockStyle.Fill;
            m_params = inst;

            #region Correction autoscroll minimum size for 96 and others dpi
            
            this.AutoScroll = true;
            Graphics g = this.CreateGraphics();
            if (g.DpiX == 96 && g.DpiY == 96)
            {
                this.AutoScrollMinSize = new Size(444,240);
            }
            else
            {
                this.AutoScrollMinSize = new Size(600, 300);
            }

            #endregion
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return new CyCustErr[] { };    //return an empty array
        }

        #endregion

        #region Assigning parameters values to controls

        public void GetParams()
        {
            // EnableInt
            checkBoxInterrupt.Checked = m_params.m_enableInt;

            // Interval
            string comboText = m_params.m_interval.Replace(m_prefix, "");
            comboText = comboText.Replace(m_postfix, "");
            comboBoxInterval.Text = comboText;
        }

        #endregion

        #region Assigning controls values to parameters

        public void SetEnableInterrupt()
        {
            m_params.m_enableInt = checkBoxInterrupt.Checked;
            m_params.SetParams(CyParamNames.ENABLE_INT);
        }

        public void SetInterval()
        {
            m_params.m_interval = m_prefix + comboBoxInterval.SelectedValue.ToString() + m_postfix;
            m_params.SetParams(CyParamNames.INTERVAL);
        }

        #endregion

        #region Event Handlers

        private void CySleepTimerCustomizer_Load(object sender, EventArgs e)
        {
            // Filtering intervalList values and assigning it to ComboBox
            for (int i = 0; i < m_params.m_intervalComboList.Count; i++)
            {
                m_params.m_intervalComboList[i] = m_params.m_intervalComboList[i].Replace(m_prefix, "");
                m_params.m_intervalComboList[i] = m_params.m_intervalComboList[i].Replace(m_postfix, "");
            }
            comboBoxInterval.DataSource = m_params.m_intervalComboList;
        }

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

        #region Control Overrided Methods

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}
