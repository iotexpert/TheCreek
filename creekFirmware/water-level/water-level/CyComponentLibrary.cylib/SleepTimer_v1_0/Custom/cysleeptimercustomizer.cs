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

namespace SleepTimer_v1_0
{
    public partial class CySleepTimerCustomizer : UserControl, ICyParamEditingControl
    {
        private Control m_control;
        private CySleepTimerParameters m_params;

        public CySleepTimerCustomizer(CySleepTimerParameters inst)
        {
            InitializeComponent();

            ((CySleepTimerParameters)inst).m_generalParams = this;
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

        #region Event Handlers

        private void checkBoxInterrupt_CheckedChanged(object sender, EventArgs e)
        {
            m_params.SetEnableInterruptCheckBox();
        }

        private void comboBoxInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_params.SetIntervalComboBox();
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
