/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying  
* the software package with which this file was provided.   
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace ADC_SAR_v1_60
{
    public partial class CySARADCControl : UserControl, ICyParamEditingControl
    {
        #region Enumerated Type String Names
        public static string RANGE_VDDA = "Vdda";
        public static string RANGE_VDAC = "VDAC";
        public static string REF_INTERNAL_REF = "Internal Vref";
        #endregion

        #region References to Enum Type String Names
        private const string REF_EXTERNAL = "External Vref";
        #endregion

        private CySARADCParameters m_params = null;
        private ICyDeviceQuery_v1 m_deviceQuery;
        private double m_clockValue = 0;
        private uint m_previousConversionRate = CyParamRange.DEFAULT_SAMPLE_RATE;

        #region Constructor(s)
        public CySARADCControl(CySARADCParameters inst, ICyDeviceQuery_v1 deviceQuery)
        {
            inst.m_control = this;

            InitializeComponent();
            this.Dock = DockStyle.Fill;

            m_params = inst;
            m_deviceQuery = deviceQuery;

            // Create NumericUpDown TextChange Event Handlers
            numSampleRate.TextChanged += new EventHandler(numSampleRate_TextChanged);
            numRefVoltage.TextChanged += new EventHandler(numRefVoltage_TextChanged);

            // Completion of ComboBoxes
            cbResolution.DataSource = m_params.m_resolutionList;
            cbInputRange.DataSource = m_params.m_inputRangeList;
            cbReference.DataSource = m_params.m_referenceList;
            numSampleRate.Minimum = 0;
            numSampleRate.Maximum = uint.MaxValue;
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
        public void UpdateUI()
        {
            // Resolution
            cbResolution.SelectedItem = (Convert.ToByte(m_params.AdcResolution)).ToString();
            // Input Range
            cbInputRange.SelectedItem = CyDictParser.GetDictValue(m_params.m_dnDict, m_params.AdcInputRange);
            // Reference
            cbReference.SelectedItem = CyDictParser.GetDictValue(m_params.m_dnDict, m_params.AdcReference);
            // Voltage Reference
            numRefVoltage.Text = m_params.RefVoltage.ToString();
            // Sample Mode
            if (m_params.AdcSampleMode == CyEAdcSampleModeType.FreeRunning)
                rbFreeRunning.Checked = true;
            else
                m_rbSocTrigerred.Checked = true;
            // Clock Source
            if (m_params.AdcClockSource == CyEAdcClockSrcType.Internal)
            {
                rbInternal.Checked = true;
            }
            else
            {
                rbExternal.Checked = true;
            }
            // Clock Frequency
            UpdateClockData();
        }
        #endregion

        #region Assigning controls values to parameters
        private void SetResolution()
        {
            try
            {
                m_params.AdcResolution = (CyEAdcResolution)byte.Parse(cbResolution.Text);
            }
            catch (Exception) { }
        }

        private void SetInputRange()
        {
            m_params.AdcInputRange = (CyEAdcInputRangeType)CyDictParser.GetDictKey(m_params.m_dnDict,
                cbInputRange.Text);
        }

        private void SetReference()
        {
            m_params.AdcReference = (CyEAdcRefType)CyDictParser.GetDictKey(m_params.m_dnDict, cbReference.Text);
        }

        private void SetVoltageReference()
        {
            try
            {
                m_params.RefVoltage = double.Parse(numRefVoltage.Text);
            }
            catch (Exception) { }
        }

        private void SetSampleMode()
        {
            m_params.AdcSampleMode = rbFreeRunning.Checked ? CyEAdcSampleModeType.FreeRunning
                : CyEAdcSampleModeType.Triggered;
        }

        private void SetClockSource()
        {
            m_params.AdcClockSource = rbInternal.Checked ? CyEAdcClockSrcType.Internal
                : CyEAdcClockSrcType.External;
        }
        #endregion

        #region Errors handling
        public void ShowError(string paramName, CyCustErr err)
        {
            Control control = null;
            string errMsg = (err.IsOk) ? string.Empty : err.Message;
            switch (paramName)
            {
                case CyParamNames.ADC_CLOCK:
                    control = rbInternal;
                    break;
                case CyParamNames.ADC_INPUT_RANGE:
                    control = cbInputRange;
                    break;
                case CyParamNames.ADC_REFERENCE:
                    control = cbReference;
                    break;
                case CyParamNames.ADC_RESOLUTION:
                    control = cbResolution;
                    break;
                case CyParamNames.ADC_SAMPLE_MODE:
                    control = rbFreeRunning;
                    break;
                case CyParamNames.REF_VOLTAGE:
                    control = numRefVoltage;
                    break;
                case CyParamNames.SAMPLE_RATE:
                    // Need to check this control one more time because setting parameter on load 
                    // is fine and error message may be overwritten by empty one
                    control = numSampleRate;
                    uint value = 0;
                    uint.TryParse(control.Text, out value);
                    if (value < GetMinSampleRate() || value > GetMaxSampleRate())
                    {
                        errorProvider.SetError(control, string.Format(Resources.ConversionRateErrorMsg,
                        GetMinSampleRate(), GetMaxSampleRate()));
                        return;
                    }
                    if (ValidateReference() == false)
                        return;
                    break;
                default:
                    break;
            }
            errorProvider.SetError(control, errMsg);
        }
        #endregion

        #region Event Handlers
        private void m_cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetResolution();
            UpdateClockData();
            UpdateSampleRateData();
        }

        private void numSampleRate_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            uint value = 0;
            uint.TryParse(currentNumeric.Text, out value);

            m_params.AdcSampleRate = value;

            if (value < GetMinSampleRate() || value > GetMaxSampleRate())
            {
                ValidateReference();
                errorProvider.SetError(currentNumeric, string.Format(Resources.ConversionRateErrorMsg,
                    GetMinSampleRate(), GetMaxSampleRate()));
            }
            else
            {
                errorProvider.SetError(currentNumeric, string.Empty);
                ValidateReference();
            }
            m_clockValue = GetInternalFreq();
            UpdateClockData();
        }

        private void cbInputRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetInputRange();
            UpdateRefVoltageEnable();
        }

        private void cbReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetReference();
            UpdateRefVoltageEnable();
            numSampleRate_TextChanged(numSampleRate, e);
        }

        private void numRefVoltage_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            float value = 0;
            float.TryParse(currentNumeric.Text, out value);
            if (value < CyParamRange.REF_VOLTAGE_MIN || value > CyParamRange.REF_VOLTAGE_MAX)
            {
                errorProvider.SetError(currentNumeric, Resources.VoltageReferenceErrorMsg);
            }
            else
            {
                m_params.RefVoltage = value;
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
            if (rbInternal.Checked)
            {
                m_clockValue = GetInternalFreq();
                if (m_params.AdcSampleRate < GetMinSampleRate() || m_params.AdcSampleRate > GetMaxSampleRate())
                    m_params.AdcSampleRate = m_previousConversionRate;
            }
            else
            {
                m_clockValue = m_params.m_externalClock;
                if (m_params.AdcSampleRate >= GetMinSampleRate() && m_params.AdcSampleRate <= GetMaxSampleRate())
                    m_previousConversionRate = m_params.AdcSampleRate;
            }
            UpdateClockData();
            UpdateSampleRateData();
            ValidateReference();
        }

        private void tbClockFreq_TextChanged(object sender, EventArgs e)
        {
            double clock = m_params.AdcClockSource == CyEAdcClockSrcType.External
                ? m_params.m_externalClock : m_clockValue;
            if ((clock < CyParamRange.CLOCK_FREQ_MIN_KHZ) || (clock > CyParamRange.CLOCK_FREQ_MAX_KHZ))
            {
                errorProvider.SetError((TextBox)sender, string.Format(Resources.ClockFrequencyErrorMsg,
                    CyParamRange.CLOCK_FREQ_MIN_MHZ, CyParamRange.CLOCK_FREQ_MAX_MHZ));
            }
            else
            {
                errorProvider.SetError((TextBox)sender, string.Empty);
            }
        }

        private void CySARADCControl_Load(object sender, EventArgs e)
        {
            uint value = 0;
            uint.TryParse(numSampleRate.Text, out value);
            m_params.AdcSampleRate = value;
        }

        private bool ValidateReference()
        {
            if (m_params.AdcReference == CyEAdcRefType.Int_Ref_Not_Bypassed &&
                m_params.AdcClockSource == CyEAdcClockSrcType.Internal &&
                m_params.AdcSampleRate > CyParamRange.CONVERSION_RATE_INT_VREF_MAX)
            {
                errorProvider.SetError(cbReference, Resources.InvalidReferenceErrorMsg);
                errorProvider.SetError(numSampleRate, Resources.InvalidReferenceErrorMsg);
                return false;
            }
            else
            {
                errorProvider.SetError(cbReference, string.Empty);
                errorProvider.SetError(numSampleRate, string.Empty);
                return true;
            }
        }
        #endregion

        #region Updating Clock and Sample Rate controls and labels
        public void UpdateClockData()
        {
            if (m_params.AdcClockSource == CyEAdcClockSrcType.External)
            {
                bool vis = m_params.m_externalClock >= 0;
                tbClockFreq.Visible = vis;
                kHz_label.Visible = vis;
                lblNoFreq.Visible = !vis;
                tbClockFreq.Text = m_params.m_externalClock.ToString();
            }
            else
            {
                tbClockFreq.Visible = true;
                kHz_label.Visible = true;
                lblNoFreq.Visible = false;
                m_clockValue = GetInternalFreq();
                tbClockFreq.Text = m_clockValue.ToString();
            }
        }

        public void UpdateSampleRateData()
        {
            if (m_params.AdcClockSource == CyEAdcClockSrcType.External)
            {
                bool vis = m_params.m_externalClock >= 0;
                numSampleRate.Visible = vis;
                numSampleRate.Enabled = !vis;
                sps_label.Visible = vis;
                lblNoRate.Visible = !vis;
                numSampleRate.Text = GetExternalSampleRate().ToString();
            }
            else
            {
                numSampleRate.Visible = true;
                numSampleRate.Enabled = true;
                sps_label.Visible = true;
                lblNoRate.Visible = false;
                numSampleRate.Value = m_params.AdcSampleRate;
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
                    // Getting system Vdda and assigning it to voltage reference if Vdda range
                    string item1 = CyDictParser.GetDictValue(m_params.m_dnDict, CyEAdcInputRangeType.Vssa_to_Vdda);
                    string item2 = CyDictParser.GetDictValue(m_params.m_dnDict, CyEAdcInputRangeType.Vneg_Vdda_Diff);
                    if (cbInputRange.SelectedItem.ToString() == item1 || cbInputRange.SelectedItem.ToString() == item2)
                    {
                        numRefVoltage.Text = m_params.m_systemVdda.ToString();
                    }
                    item1 = CyDictParser.GetDictValue(m_params.m_dnDict, CyEAdcInputRangeType.Vneg_Vdda_2_Diff);
                    if (cbInputRange.SelectedItem.ToString() == item1)
                    {
                        double dividedVdda = m_params.m_systemVdda / 2;
                        numRefVoltage.Maximum = (decimal)dividedVdda;
                        numRefVoltage.Text = dividedVdda.ToString();
                        Volts_label.Text = "Volts (Vdda/2)";
                    }
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
        // Get the ADC clock frequency for the current settings in kHz.
        public double GetInternalFreq()
        {
            return (double)(m_params.AdcSampleRate * (GetResolution() + m_params.SamplePrecharge)) / 1000;
        }

        public uint GetExternalSampleRate()
        {
            if (m_params.m_externalClock >= 0)
                return (uint)((m_params.m_externalClock * 1000) / (GetResolution() + m_params.SamplePrecharge));
            else return 0;
        }

        public uint GetMaxSampleRate()
        {
            return (uint)((CyParamRange.CLOCK_FREQ_MAX_HZ) / (GetResolution() + m_params.SamplePrecharge));
        }

        public uint GetMinSampleRate()
        {
            return (uint)((CyParamRange.CLOCK_FREQ_MIN_HZ) / (GetResolution() + m_params.SamplePrecharge));
        }

        // Returns actual resolution value or 12 for PSoC5 ES1
        private byte GetResolution()
        {
            byte resolution = 0;
            if (m_deviceQuery.SiliconRevisionAvailable)
            {
                if (m_deviceQuery.DeviceFamily == CyEDeviceFamilyType.PSoC5.ToString()
                    && m_deviceQuery.SiliconRevision == 1)
                    resolution = 12;
                else
                    try
                    {
                        resolution = Convert.ToByte(m_params.AdcResolution);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.Assert(false);
                    }
            }
            return resolution;
        }
        #endregion
    }
}
