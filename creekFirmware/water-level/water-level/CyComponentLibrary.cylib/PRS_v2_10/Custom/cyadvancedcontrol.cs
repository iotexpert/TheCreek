/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;

namespace PRS_v2_10
{
    public partial class CyAdvancedControl : UserControl
    {
        private const string MSG_DISABLE_TIME_MULTIPLEXING = 
            "For Resolution < 9 Time Multiplexing option should be disabled";
        private const string MSG_ENABLE_TIME_MULTIPLEXING =
            "For Resolution > 32 Time Multiplexing option should be enabled";

        private const byte TM_MIN_CHECKPOINT = 8;
        private const byte TM_MAX_CHECKPOINT = 32;
        private const byte WAKEUP_RESUME = 1;
        private const byte WAKEUP_START = 0;

        public CyPRSParameters m_parameters;

        public CyAdvancedControl()
        {
            InitializeComponent();
        }

        public CyAdvancedControl(CyPRSParameters parameters)
        {
            InitializeComponent();
            m_parameters = parameters;
            UpdateForm();
        }

        public void UpdateForm()
        {
            if (m_parameters.m_timeMultiplexing)
                radioButtonEnabled.Checked = true;
            else
                radioButtonDisabled.Checked = true;

            EnableTimeMultiplexing();

            if (m_parameters.m_wakeupBehaviour == WAKEUP_RESUME)
                radioButtonWakeupResume.Checked = true;
            else
                radioButtonWakeupStart.Checked = true;
        }

        private void EnableTimeMultiplexing()
        {
            bool shouldEnable = m_parameters.m_resolution > TM_MAX_CHECKPOINT;
            bool shouldDisable = m_parameters.m_resolution <= TM_MIN_CHECKPOINT;
            if (shouldEnable)
            {
                if (radioButtonDisabled.Checked)
                {
                    errorProvider.SetError(radioButtonEnabled, MSG_ENABLE_TIME_MULTIPLEXING);
                }
                else
                {
                    errorProvider.SetError(radioButtonEnabled, String.Empty);
                }
            }
            else if (shouldDisable)
            {
                if (radioButtonEnabled.Checked)
                {
                    errorProvider.SetError(radioButtonEnabled, MSG_DISABLE_TIME_MULTIPLEXING);
                }
                else
                {
                    errorProvider.SetError(radioButtonEnabled, String.Empty);
                }
            }
            else
            {
                errorProvider.SetError(radioButtonEnabled, String.Empty);
            }
        }

        public string ValidateTimeMultiplexing()
        {
            string err = "";
            if ((m_parameters.m_resolution <= TM_MIN_CHECKPOINT) && (m_parameters.m_timeMultiplexing))
                err = MSG_DISABLE_TIME_MULTIPLEXING;
            else if ((m_parameters.m_resolution > TM_MAX_CHECKPOINT) && (m_parameters.m_timeMultiplexing == false))
                err = MSG_DISABLE_TIME_MULTIPLEXING;
            return err;
        }

        private void radioButtonEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (m_parameters.m_timeMultiplexing != radioButtonEnabled.Checked)
            {
                m_parameters.m_timeMultiplexing = radioButtonEnabled.Checked;
                m_parameters.SetParam(CyPRSParameters.PARAM_TIMEMULTIPLEXING);

                EnableTimeMultiplexing();
            }
        }

        private void radioButtonWakeupResume_CheckedChanged(object sender, EventArgs e)
        {
            byte newValue = radioButtonWakeupResume.Checked ? WAKEUP_RESUME : WAKEUP_START;
            if (m_parameters.m_wakeupBehaviour != newValue)
            {
                m_parameters.m_wakeupBehaviour = newValue;
                m_parameters.SetParam(CyPRSParameters.PARAM_WAKEUPBEHAVIOUR);
            }
        }

        private void CyAdvancedControl_SizeChanged(object sender, EventArgs e)
        {
            radioButtonDisabled.Left = groupBoxTimeMultiplexing.Width/2;
            radioButtonWakeupStart.Left = groupBoxWakeup.Width / 2;
            labelDisable.Left = radioButtonDisabled.Left;
            labelStart.Left = radioButtonWakeupStart.Left;
            labelEnable.Width = labelDisable.Left - labelEnable.Left - 5;
            labelRestore.Width = labelStart.Left - labelRestore.Left - 5;
            labelDisable.Width = labelEnable.Width;
            labelStart.Width = labelRestore.Width;
        }
    }
}
