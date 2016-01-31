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

namespace ADC_SAR_v1_50
{
    public partial class CySARADCControl : UserControl, ICyParamEditingControl
    {
        #region Enumerated Type String Names
        const string RANGE_VDDA = "Vdda";
        const string RANGE_VDAC = "VDAC";
        const string REF_INTERNAL_REF = "Internal Vref";
        #endregion

        #region References to Enum Type String Names
        const string REF_EXTERNAL = "External Vref";
        #endregion

        public CySARADCParameters m_params = null;

        #region Constructor(s)
        public CySARADCControl(CySARADCParameters inst)
        {
            inst.m_control = this;

            InitializeComponent();
            this.Dock = DockStyle.Fill;

            m_params = inst;

            // Completion of ComboBoxes
            cbResolution.DataSource = m_params.m_resolutionList;
            cbPower.DataSource = m_params.m_powerList;
            cbInputRange.DataSource = m_params.m_inputRangeList;
            cbReference.DataSource = m_params.m_referenceList;
            numSampleRate.TextChanged += new EventHandler(numSampleRate_TextChanged);
            numRefVoltage.TextChanged += new EventHandler(numRefVoltage_TextChanged);
            numSampleRate.Minimum = 0;
            numSampleRate.Maximum = uint.MaxValue;
            this.Load += new EventHandler(CySARADCControl_Load);
        }

        void CySARADCControl_Load(object sender, EventArgs e)
        {
            m_params.GetFrequencyAndSampleRate();
        }
        #endregion

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(CyCustomizer.CONFIGURE_TAB_NAME))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            yield return new CyCustErr(errMsg);
                        }
                    }
                }
            }
        }
        #endregion

        #region Assigning parameters values to controls
        public void UpdateFormFromParams()
        {
            // Power
            cbPower.SelectedItem = CyEnumConverter.GetEnumDesc(m_params.m_adcPower);
            // Resolution
            cbResolution.SelectedItem = m_params.m_adcResolution.ToString();
            // Conversion Rate
            numSampleRate.Value = (decimal)m_params.m_adcSampleRate;
            // Clock Frequency
            UpdateClockLabel(m_params.m_clock);
            UpdateSampleRateLabel(m_params.m_clock);
            // Input Range
            cbInputRange.SelectedItem = CyEnumConverter.GetEnumDesc(m_params.m_adcInputRange);
            // Reference
            cbReference.SelectedItem = CyEnumConverter.GetEnumDesc(m_params.m_adcReference);
            // Voltage Reference
            numRefVoltage.Value = (decimal)m_params.m_refVoltage;
            // Sample Mode
            if (m_params.m_adcSampleMode == E_ADC_SAMPLE_MODE_TYPE.FreeRunning)
                rbFreeRunning.Checked = true;
            else
                m_rbSocTrigerred.Checked = true;
            // Clock Source
            if (m_params.m_adcClockSource == E_ADC_CLOCK_SRC_TYPE.Internal)
                rbInternal.Checked = true;
            else
                rbExternal.Checked = true;
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetPower()
        {
            m_params.m_adcPower = (E_ADC_POWER_TYPE)CyEnumConverter.GetEnumValue(cbPower.Text,
                typeof(E_ADC_POWER_TYPE));
            m_params.SetParameter(CyParamNames.ADC_POWER);
        }

        private void SetResolution()
        {
            UInt16.TryParse(cbResolution.Text, out m_params.m_adcResolution);
            m_params.SetParameter(CyParamNames.ADC_RESOLUTION);
        }

        private void SetSampleRate()
        {
            uint.TryParse(numSampleRate.Text, out m_params.m_adcSampleRate);
            m_params.SetParameter(CyParamNames.SAMPLE_RATE);
        }

        private void SetInputRange()
        {
            m_params.m_adcInputRange = (E_ADC_INPUT_RANGE_TYPE)CyEnumConverter.GetEnumValue(cbInputRange.Text,
                typeof(E_ADC_INPUT_RANGE_TYPE));
            m_params.SetParameter(CyParamNames.ADC_INPUT_RANGE);
        }

        private void SetReference()
        {
            m_params.m_adcReference = (E_ADC_REF_TYPE)CyEnumConverter.GetEnumValue(cbReference.Text,
                typeof(E_ADC_REF_TYPE));
            m_params.SetParameter(CyParamNames.ADC_REFERENCE);
        }

        private void SetVoltageReference()
        {
            float.TryParse(numRefVoltage.Text, out m_params.m_refVoltage);
            m_params.SetParameter(CyParamNames.REF_VOLTAGE);
        }

        private void SetSampleMode()
        {
            m_params.m_adcSampleMode = rbFreeRunning.Checked ? E_ADC_SAMPLE_MODE_TYPE.FreeRunning
                : E_ADC_SAMPLE_MODE_TYPE.Triggered;
            m_params.SetParameter(CyParamNames.ADC_SAMPLE_MODE);
        }

        private void SetClockSource()
        {
            m_params.m_adcClockSource = rbInternal.Checked ? E_ADC_CLOCK_SRC_TYPE.Internal
                : E_ADC_CLOCK_SRC_TYPE.External;
            m_params.SetParameter(CyParamNames.ADC_CLOCK);
        }

        private void SetClockFrequency()
        {
            float.TryParse(tbClockFreq.Text, out m_params.m_adcClockFrequency);
            m_params.SetParameter(CyParamNames.ADC_CLOCK_FREQUENCY);
        }
        #endregion

        #region Event Handlers
        private void m_cbPower_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPower();
        }

        private void m_cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetResolution();
            UpdateClockLabel(m_params.m_clock);
            UpdateSampleRateLabel(m_params.m_clock);
            if (m_params.m_adcClockSource == E_ADC_CLOCK_SRC_TYPE.External)
                numSampleRate.Value = (decimal)GetExternalSampleRate(m_params.m_adcResolution, m_params.m_clock);
        }

        private void numSampleRate_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            uint value = 0;
            uint.TryParse(currentNumeric.Text, out value);
            if (value < GetMinSampleRate() || value > GetMaxSampleRate())
            {
                errorProvider.SetError(currentNumeric, string.Format(Properties.Resources.ConversionRateErrorMsg,
                    GetMinSampleRate(), GetMaxSampleRate()));
            }
            else
            {
                m_params.m_adcSampleRate = value;
                errorProvider.SetError(currentNumeric, string.Empty);
                SetSampleRate();
            }
            UpdateClockLabel(m_params.m_clock);
            UpdateSampleRateLabel(m_params.m_clock);
        }

        private void cbInputRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetInputRange();

            // If "Vssa to VDAC selected then choose refernce to be "VDAC Ref"
            if (cbInputRange.Text.Contains(RANGE_VDAC))
            {
                cbReference.Text = REF_INTERNAL_REF; //REF_VDAC; 
                cbReference.Enabled = false;
            }
            // If "Vssa to Vdda (Single Ended)" is selected then choose refernce to be "Internal Ref"
            else if (cbInputRange.Text.Contains(RANGE_VDDA))
            {
                cbReference.Text = REF_INTERNAL_REF;
                cbReference.Enabled = false;
            }
            else
                cbReference.Enabled = true;

            UpdateRefVoltageEnable();
        }

        private void cbReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetReference();
            UpdateRefVoltageEnable();
        }

        private void numRefVoltage_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            float value = 0;
            float.TryParse(currentNumeric.Text, out value);
            if (value < CyParamRange.REF_VOLTAGE_MIN || value > CyParamRange.REF_VOLTAGE_MAX)
            {
                errorProvider.SetError(currentNumeric, Properties.Resources.VoltageReferenceErrorMsg);
            }
            else
            {
                m_params.m_refVoltage = value;
                errorProvider.SetError(currentNumeric, string.Empty);
                SetVoltageReference();
            }
        }

        // Sample Mode
        private void rbFreeRunning_CheckedChanged(object sender, EventArgs e)
        {
            SetSampleMode();
        }

        // Clock source radio buttons
        private void rbInternal_CheckedChanged(object sender, EventArgs e)
        {
            SetClockSource();
            if (m_params.m_adcClockSource == E_ADC_CLOCK_SRC_TYPE.External && m_params.m_clock >= 0)
                m_params.m_adcClockFrequency = m_params.m_clock;

            UpdateClockLabel(m_params.m_clock);
            UpdateSampleRateLabel(m_params.m_clock);
        }

        private void tbClockFreq_TextChanged(object sender, EventArgs e)
        {
            SetClockFrequency();
            if ((m_params.m_adcClockFrequency * 1000) < 1000000 || (m_params.m_adcClockFrequency * 1000) > 18000000)
            {
                errorProvider.SetError((TextBox)sender, Properties.Resources.ClockFrequencyErrorMsg);
            }
            else
            {
                errorProvider.SetError((TextBox)sender, string.Empty);
            }
        }
        #endregion

        #region Updating Labels
        public void UpdateClockLabel(double clock)
        {
            if (m_params.m_adcClockSource == E_ADC_CLOCK_SRC_TYPE.External)
            {
                bool vis = clock >= 0;
                tbClockFreq.Visible = vis;
                kHz_label.Visible = vis;
                lblNoFreq.Visible = !vis;
                numSampleRate.Enabled = !vis;
                if (clock >= 0)
                {
                    m_params.GetFrequencyAndSampleRate();
                    tbClockFreq.Text = m_params.m_adcClockFrequency.ToString();
                    numSampleRate.Value = (decimal)m_params.m_adcSampleRate;
                }
            }
            else
            {
                m_params.GetFrequencyAndSampleRate();
                tbClockFreq.Text = m_params.m_adcClockFrequency.ToString();
                numSampleRate.Value = (decimal)m_params.m_adcSampleRate;
                tbClockFreq.Visible = true;
                kHz_label.Visible = true;
                lblNoFreq.Visible = false;
                numSampleRate.Enabled = true;
            }
        }

        public void UpdateSampleRateLabel(double clock)
        {
            if (m_params.m_adcClockSource == E_ADC_CLOCK_SRC_TYPE.External)
            {
                bool vis = clock >= 0;
                numSampleRate.Visible = vis;
                SPS_label.Visible = vis;
                lblNoRate.Visible = !vis;
            }
            else
            {
                numSampleRate.Visible = true;
                SPS_label.Visible = true;
                lblNoRate.Visible = false;
            }
        }

        // Check what combination of Input Range and Reference is selected
        private void UpdateRefVoltageEnable()
        {
            if (cbReference.Text.Contains(REF_EXTERNAL) || cbInputRange.Text.Contains(RANGE_VDDA) ||
                cbInputRange.Text.Contains(RANGE_VDAC))
            {
                numRefVoltage.Enabled = true;
                if (cbInputRange.Text.Contains(RANGE_VDAC))
                {
                    Volts_label.Text = "Volts (Vdac)";
                    numRefVoltage.Maximum = 5.5M;
                }
                else if (cbInputRange.Text.Contains(RANGE_VDDA))
                {
                    Volts_label.Text = "Volts (Vdda)";
                    numRefVoltage.Maximum = 5.5M;
                }
                else
                {
                    Volts_label.Text = "Volts";
                    numRefVoltage.Maximum = 5.5M;
                }
            }
            else
            {
                numRefVoltage.Maximum = 1.5M;
                Volts_label.Text = "Volts";
                numRefVoltage.Value = 1.024M;
                numRefVoltage.Enabled = false;
            }
        }
        #endregion

        #region Getting Internal Frequency and External Sample Rate
        // Get the ADC clock frequency for the current settings.
        public float GetInternalFreq(int resolution, uint sampleRate)
        {
            return (float)(sampleRate * (resolution + 6)) / 1000;
        }

        public uint GetExternalSampleRate(int resolution, double clock)
        {
            if (clock >= 0)
                return (uint)((clock * 1000) / (resolution + 6));
            else return 0;
        }

        public uint GetMaxSampleRate()
        {
            return (uint)((18000000) / (m_params.m_adcResolution + 6));
        }

        public uint GetMinSampleRate()
        {
            return (uint)((1000000) / (m_params.m_adcResolution + 6));
        }
        #endregion
    }
}
