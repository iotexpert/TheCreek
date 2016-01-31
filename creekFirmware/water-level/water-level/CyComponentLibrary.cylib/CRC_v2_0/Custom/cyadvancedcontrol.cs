using System;
using System.Windows.Forms;

namespace CRC_v2_0
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

        public CyCRCParameters m_parameters;

        public CyAdvancedControl()
        {
            InitializeComponent();
        }

        public CyAdvancedControl(CyCRCParameters parameters)
        {
            InitializeComponent();
            m_parameters = parameters;
            UpdateForm();
        }

        public void UpdateForm()
        {
            if (m_parameters.TimeMultiplexing)
                radioButtonEnabled.Checked = true;
            else
                radioButtonDisabled.Checked = true;

            EnableTimeMultiplexing();
        }

        private void EnableTimeMultiplexing()
        {
            bool shouldEnable = m_parameters.Resolution > TM_MAX_CHECKPOINT;
            bool shouldDisable = m_parameters.Resolution <= TM_MIN_CHECKPOINT;
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
            if ((m_parameters.Resolution <= TM_MIN_CHECKPOINT) && (m_parameters.TimeMultiplexing))
                err = MSG_DISABLE_TIME_MULTIPLEXING;
            else if ((m_parameters.Resolution > TM_MAX_CHECKPOINT) && (m_parameters.TimeMultiplexing == false))
                err = MSG_DISABLE_TIME_MULTIPLEXING;
            return err;
        }

        private void radioButtonEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (m_parameters.TimeMultiplexing != radioButtonEnabled.Checked)
            {
                m_parameters.TimeMultiplexing = radioButtonEnabled.Checked;
                m_parameters.SetParam(CyCRCParameters.S_TIMEMULTIPLEXING);

                EnableTimeMultiplexing();
            }
        }

        private void CyAdvancedControl_SizeChanged(object sender, EventArgs e)
        {
            radioButtonDisabled.Left = groupBoxTimeMultiplexing.Width/2;
            labelDisable.Left = radioButtonDisabled.Left;
            labelEnable.Width = labelDisable.Left - labelEnable.Left - 5;
            labelDisable.Width = labelEnable.Width;
        }
    }
}
