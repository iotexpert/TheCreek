using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace VectorCAN_v1_0
{
    public partial class CyGeneralTab : CyCSParamEditTemplate
    {
        public override string TabName
        {
            get { return "General"; }
        }
        public CyGeneralTab()
            :base()
        {
            InitializeComponent();
            cbEnableInterrupts.CheckedChanged += new EventHandler(cbEnableInterrupts_CheckedChanged);
            cbAddTransceiverEnableSignal.CheckedChanged += new EventHandler(cbAddTransceiverEnableSignal_CheckedChanged);
        }
        public CyGeneralTab(CyParameters packParams)
            : this()
        {
            m_packParams = packParams;
            UpdateClock(m_packParams.m_edit, m_packParams.m_term);
        }
        
        public void UpdateClock(ICyInstQuery_v1 edit, ICyTerminalQuery_v1 termQuery)
        {
            if (m_packParams == null) return;
            //Get Bus_Clock information
            double clockFr = -1;
            List<CyClockData> clkdata = termQuery.GetClockData("or_bclk", "term1", 0);

            if (clkdata.Count == 1)
            {
                if (clkdata[0].IsFrequencyKnown)
                {
                    clockFr = clkdata[0].Frequency * (Math.Pow(10, clkdata[0].UnitAsExponent) / 1000);
                }
            }

            lclockFr.Text = string.Format(cvresources.ClockLabel, clockFr > -1 ? Convert.ToInt16(clockFr).ToString() : 
                        cvresources.ClockNotAvailible);
        }

        void cbAddTransceiverEnableSignal_CheckedChanged(object sender, EventArgs e)
        {
            m_packParams.AddTransceiverEnableSignal = (sender as CheckBox).Checked;
            errorProvider.SetError(sender as Control, string.Empty);
        }

        void cbEnableInterrupts_CheckedChanged(object sender, EventArgs e)
        {
            m_packParams.EnableInterrupts = (sender as CheckBox).Checked;
            errorProvider.SetError(sender as Control, string.Empty);
        }
        public void UpdateUI()
        {
            if (m_packParams.AddTransceiverEnableSignal != null)
                cbAddTransceiverEnableSignal.Checked = m_packParams.AddTransceiverEnableSignal == true;
            else
                errorProvider.SetError(cbAddTransceiverEnableSignal, cvresources.InvalidValue);

            if (m_packParams.EnableInterrupts != null)
                cbEnableInterrupts.Checked = m_packParams.EnableInterrupts == true;
            else
                errorProvider.SetError(cbEnableInterrupts, cvresources.InvalidValue);
        }
    }
}
