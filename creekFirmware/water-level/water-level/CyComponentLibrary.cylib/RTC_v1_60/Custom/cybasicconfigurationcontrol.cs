/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace RTC_v1_60
{
    class CyBasicConfigurationControl : UserControl, ICyParamEditingControl
    {
        private CheckBox m_checkDstEnable;
        private ComboBox m_comboStartDay;
        private Label m_labelStartOfWeek;
        private CyRTCParameters m_parameters;

        #region Constructor(s)
        public CyBasicConfigurationControl(CyRTCParameters parameters)
        {
            InitializeComponent();
            Array weekDays = Enum.GetValues(typeof(DayOfWeek));

            //Fill Combobox with Data
            for (int i = 0; i < weekDays.Length; i++)
            {
                m_comboStartDay.Items.Add(weekDays.GetValue(i));
            }

            m_parameters = parameters;
            UpdateForm();
        }
        #endregion

        public void UpdateForm()
        {
            m_comboStartDay.SelectedItem = m_parameters.StartDayOfWeek;

            m_checkDstEnable.Checked = m_parameters.DstEnable;
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            if (m_parameters.m_edit != null)
            {
                foreach (string paramName in m_parameters.m_edit.GetParamNames())
                {
                    CyCompDevParam param = m_parameters.m_edit.GetCommittedParam(paramName);
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
            }

            return errs;
        }
        #endregion

        #region Event Handlers
        private void check_dstEnable_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.DstEnable = m_checkDstEnable.Checked;
        }

        private void combo_StartDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.StartDayOfWeek = (DayOfWeek)m_comboStartDay.SelectedItem;
        }
        #endregion

        private void InitializeComponent()
        {
            this.m_checkDstEnable = new System.Windows.Forms.CheckBox();
            this.m_comboStartDay = new System.Windows.Forms.ComboBox();
            this.m_labelStartOfWeek = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_checkDstEnable
            // 
            this.m_checkDstEnable.AutoSize = true;
            this.m_checkDstEnable.Location = new System.Drawing.Point(18, 19);
            this.m_checkDstEnable.Name = "m_checkDstEnable";
            this.m_checkDstEnable.Size = new System.Drawing.Size(226, 17);
            this.m_checkDstEnable.TabIndex = 0;
            this.m_checkDstEnable.Text = "Enable Daylight Savings Time functionality";
            this.m_checkDstEnable.UseVisualStyleBackColor = true;
            this.m_checkDstEnable.CheckedChanged += new System.EventHandler(this.check_dstEnable_CheckedChanged);
            // 
            // m_comboStartDay
            // 
            this.m_comboStartDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_comboStartDay.FormattingEnabled = true;
            this.m_comboStartDay.Location = new System.Drawing.Point(187, 55);
            this.m_comboStartDay.Name = "m_comboStartDay";
            this.m_comboStartDay.Size = new System.Drawing.Size(121, 21);
            this.m_comboStartDay.TabIndex = 1;
            this.m_comboStartDay.SelectedIndexChanged += new System.EventHandler(this.combo_StartDay_SelectedIndexChanged);
            // 
            // m_labelStartOfWeek
            // 
            this.m_labelStartOfWeek.AutoSize = true;
            this.m_labelStartOfWeek.Location = new System.Drawing.Point(15, 58);
            this.m_labelStartOfWeek.Name = "m_labelStartOfWeek";
            this.m_labelStartOfWeek.Size = new System.Drawing.Size(70, 13);
            this.m_labelStartOfWeek.TabIndex = 2;
            this.m_labelStartOfWeek.Text = "Start of week";
            // 
            // CyBasicConfigurationControl
            // 
            this.Controls.Add(this.m_comboStartDay);
            this.Controls.Add(this.m_labelStartOfWeek);
            this.Controls.Add(this.m_checkDstEnable);
            this.Name = "CyBasicConfigurationControl";
            this.Size = new System.Drawing.Size(355, 222);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
