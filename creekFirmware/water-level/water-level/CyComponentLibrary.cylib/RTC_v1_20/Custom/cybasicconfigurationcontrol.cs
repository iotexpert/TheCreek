/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;

namespace RTC_v1_20
{
    class CyBasicConfigurationControl : UserControl, ICyParamEditingControl
    {
        private CheckBox check_dstEnable;
        private ComboBox combo_StartDay;
        private Label label_StartOfWeek;
        private CyParameters m_parameters;

        public CyBasicConfigurationControl(CyParameters parameters)
        {
            InitializeComponent();

            //Fill Combobox with Data
            foreach (DayOfWeek d in Enum.GetValues(typeof(DayOfWeek)))
            {
                combo_StartDay.Items.Add(d);
            }

            m_parameters = parameters;
            UpdateForm();
        }

        public void UpdateForm()
        {
            combo_StartDay.SelectedItem = m_parameters.StartDayOfWeek;

            check_dstEnable.Checked = m_parameters.DstEnable;
        }

        #region ICyParamEditingControl Members

        Control ICyParamEditingControl.DisplayControl
        {
            get { return this; }
        }

        IEnumerable<CyCustErr> ICyParamEditingControl.GetErrors()
        {
            return new List<CyCustErr>();
        }

        #endregion

        private void InitializeComponent()
        {
            this.check_dstEnable = new System.Windows.Forms.CheckBox();
            this.combo_StartDay = new System.Windows.Forms.ComboBox();
            this.label_StartOfWeek = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // check_dstEnable
            // 
            this.check_dstEnable.AutoSize = true;
            this.check_dstEnable.Location = new System.Drawing.Point(18, 19);
            this.check_dstEnable.Name = "check_dstEnable";
            this.check_dstEnable.Size = new System.Drawing.Size(301, 21);
            this.check_dstEnable.TabIndex = 0;
            this.check_dstEnable.Text = "Enable Daylight Savings Time Functionality";
            this.check_dstEnable.UseVisualStyleBackColor = true;
            this.check_dstEnable.CheckedChanged += new System.EventHandler(this.check_dstEnable_CheckedChanged);
            // 
            // combo_StartDay
            // 
            this.combo_StartDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_StartDay.FormattingEnabled = true;
            this.combo_StartDay.Location = new System.Drawing.Point(187, 55);
            this.combo_StartDay.Name = "combo_StartDay";
            this.combo_StartDay.Size = new System.Drawing.Size(121, 24);
            this.combo_StartDay.TabIndex = 1;
            this.combo_StartDay.SelectedIndexChanged += new System.EventHandler(this.combo_StartDay_SelectedIndexChanged);
            // 
            // label_StartOfWeek
            // 
            this.label_StartOfWeek.AutoSize = true;
            this.label_StartOfWeek.Location = new System.Drawing.Point(15, 58);
            this.label_StartOfWeek.Name = "label_StartOfWeek";
            this.label_StartOfWeek.Size = new System.Drawing.Size(90, 17);
            this.label_StartOfWeek.TabIndex = 2;
            this.label_StartOfWeek.Text = "Start of week";
            // 
            // CyBasicConfigurationControl
            // 
            this.Controls.Add(this.combo_StartDay);
            this.Controls.Add(this.label_StartOfWeek);
            this.Controls.Add(this.check_dstEnable);
            this.Name = "CyBasicConfigurationControl";
            this.Size = new System.Drawing.Size(355, 222);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void check_dstEnable_CheckedChanged(object sender, EventArgs e)
        {
            m_parameters.DstEnable = check_dstEnable.Checked;
        }

        private void combo_StartDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_parameters.StartDayOfWeek = (DayOfWeek)combo_StartDay.SelectedItem;
        }
    }
}
