/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying  
* the software package with which this file was provided.   
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace ADC_SAR_v1_80
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
        private uint m_previousConversionRate = 0;

        #region Constructor(s)
        public CySARADCControl(CySARADCParameters inst, ICyDeviceQuery_v1 deviceQuery)
        {
            inst.m_control = this;

            InitializeComponent();
            this.Dock = DockStyle.Fill;

            m_params = inst;
            m_deviceQuery = deviceQuery;
            m_previousConversionRate = m_params.SampleRateDefault;

            // Create NumericUpDown TextChange Event Handlers
            numSampleRate.TextChanged += new EventHandler(numSampleRate_TextChanged);
            numRefVoltage.TextChanged += new EventHandler(numRefVoltage_TextChanged);

            // Completion of ComboBoxes
            cbResolution.DataSource = m_params.m_resolutionList;
            foreach (string item in m_params.m_referenceList)
            {
                cbReference.Items.Add(item);
            }
            cbInputRange.DataSource = m_params.m_inputRangeList;
            numSampleRate.Minimum = 0;
            numSampleRate.Maximum = uint.MaxValue;
            numRefVoltage.Minimum = decimal.MinValue;
            numRefVoltage.Maximum = decimal.MaxValue;
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
            // Reference
            cbReference.SelectedItem = CyDictParser.GetDictValue(m_params.m_dnDict, m_params.AdcReference);
            // Input Range
            cbInputRange.SelectedItem = CyDictParser.GetDictValue(m_params.m_dnDict, m_params.AdcInputRange);
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
                    if (value < m_params.GetMinSampleRate() || value > m_params.GetMaxSampleRate())
                    {
                        errorProvider.SetError(control, string.Format(Resources.ConversionRateErrorMsg,
                        m_params.GetMinSampleRate(), m_params.GetMaxSampleRate()));
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
            ValidateSampleRate(m_params.AdcSampleRate);
        }

        private void numSampleRate_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            uint value = 0;
            uint.TryParse(currentNumeric.Text, out value);

            m_params.AdcSampleRate = value;
            ValidateSampleRate(value);
            m_clockValue = m_params.GetInternalFreq();
            UpdateClockData();
        }

        private void cbInputRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetInputRange();
            UpdateRefVoltageControl();
            // Removing External Vref from combobox if VDAC selected
            UpdateReferenceCombobox(cbInputRange.SelectedItem.ToString().Contains(RANGE_VDAC));
            numRefVoltage_TextChanged(numRefVoltage, new EventArgs());
        }

        private void cbReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetReference();
            UpdateRefVoltageControl();
            numSampleRate_TextChanged(numSampleRate, e);
            numRefVoltage_TextChanged(numRefVoltage, new EventArgs());
        }

        private void numRefVoltage_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown currentNumeric = (NumericUpDown)sender;
            double value = 0;
            double.TryParse(currentNumeric.Text, out value);

            double maxVoltage;
            string message;
            // External voltage reference should be limited to Vdda/2 for "Vssa to Vdda(Single)" 
            // and "0.0 +/- Vdda/2(Differential)" input ranges
            if (m_params.AdcReference == CyEAdcRefType.Ext_Ref &&
                (m_params.AdcInputRange == CyEAdcInputRangeType.Vssa_to_Vdda ||
                m_params.AdcInputRange == CyEAdcInputRangeType.Vneg_Vdda_2_Diff))
            {
                maxVoltage = m_params.m_systemVdda / 2;
                message = Resources.VoltageReferenceErrorMsgVddaDiv2;
            }
            else
            {
                maxVoltage = m_params.m_systemVdda;
                message = Resources.VoltageReferenceErrorMsg;
            }

            if (value < CyParamRange.REF_VOLTAGE_MIN || value > maxVoltage)
            {
                errorProvider.SetError(currentNumeric, string.Format(message, CyParamRange.REF_VOLTAGE_MIN,
                    maxVoltage));
            }
            else
            {
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
                m_clockValue = m_params.GetInternalFreq();
                if (m_params.AdcSampleRate < m_params.GetMinSampleRate() || m_params.AdcSampleRate > 
                    m_params.GetMaxSampleRate())
                    m_params.AdcSampleRate = m_previousConversionRate;
            }
            else
            {
                m_clockValue = m_params.m_externalClock;
                if (m_params.AdcSampleRate >= m_params.GetMinSampleRate() && m_params.AdcSampleRate <= 
                    m_params.GetMaxSampleRate())
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
            if ((clock < CyParamRange.ClockFreqMinKHz) || (clock > CyParamRange.ClockFreqMaxKHz))
            {
                errorProvider.SetError((TextBox)sender, string.Format(Resources.ClockFrequencyErrorMsg,
                    CyParamRange.ClockFreqMinMHz, CyParamRange.ClockFreqMaxMHz));
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

            // This will set new value (Internal Vref, bypassed) for Reference if External Vref 
            // and VDAC selected simultaneously from Expression View or previous component version
            SetReference();
            // Voltage Reference control have to be updated on load because system VDDA could change
            UpdateRefVoltageControl();
        }

        private void ValidateSampleRate(uint value)
        {
            if (value < m_params.GetMinSampleRate() || value > m_params.GetMaxSampleRate())
            {
                ValidateReference();
                errorProvider.SetError(numSampleRate, string.Format(Resources.ConversionRateErrorMsg,
                    m_params.GetMinSampleRate(), m_params.GetMaxSampleRate()));
            }
            else
            {
                errorProvider.SetError(numSampleRate, string.Empty);
                ValidateReference();
            }
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
                if (IsConversionRateValid())
                    errorProvider.SetError(numSampleRate, string.Empty);
                return true;
            }
        }

        private bool IsConversionRateValid()
        {
            return (m_params.AdcSampleRate >= m_params.GetMinSampleRate() && 
                m_params.AdcSampleRate <= m_params.GetMaxSampleRate());
        }

        private void UpdateReferenceCombobox(bool isVdac)
        {
            string extRefItem = CyDictParser.GetDictValue(m_params.m_dnDict, CyEAdcRefType.Ext_Ref);
            string previouslySelectedItem = extRefItem;

            if (cbReference.SelectedItem != null)
                previouslySelectedItem = cbReference.SelectedItem.ToString();

            if (isVdac)
            {
                if (cbReference.Items.Contains(extRefItem))
                {
                    cbReference.Items.Remove(extRefItem);
                    if (previouslySelectedItem == extRefItem)
                        cbReference.SelectedItem = CyDictParser.GetDictValue(m_params.m_dnDict,
                            CyEAdcRefType.Int_Ref_Bypass);
                }
            }
            else
            {
                if (cbReference.Items.Contains(extRefItem) == false)
                    cbReference.Items.Add(extRefItem);
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
                lblNoFreq.Visible = !vis;
                tbClockFreq.Text = m_params.m_externalClock.ToString();
            }
            else
            {
                if (CySARADCParameters.IsArchMemberAvailable)
                {
                    tbClockFreq.Visible = true;
                    lblNoFreq.Visible = false;
                    m_clockValue = m_params.GetInternalFreq();
                    tbClockFreq.Text = m_clockValue.ToString();
                }
                else
                {
                    tbClockFreq.Visible = false;
                    lblNoFreq.Visible = true;
                }
            }
        }

        public void UpdateSampleRateData()
        {
            if (m_params.AdcClockSource == CyEAdcClockSrcType.External)
            {
                if (CySARADCParameters.IsArchMemberAvailable)
                {
                    bool vis = m_params.m_externalClock >= 0;
                    numSampleRate.Visible = vis;
                    numSampleRate.Enabled = !vis;
                    lblNoRate.Visible = !vis;
                    numSampleRate.Text = m_params.GetExternalSampleRate().ToString();
                }
                else
                {
                    numSampleRate.Visible = false;
                    lblNoRate.Visible = true;
                }
            }
            else
            {
                numSampleRate.Visible = true;
                numSampleRate.Enabled = true;
                lblNoRate.Visible = false;
                numSampleRate.Value = m_params.AdcSampleRate;
            }
        }

        // Check what combination of Input Range and Reference is selected
        private void UpdateRefVoltageControl()
        {
            bool isInternalRef = (cbReference.Text.Contains(REF_EXTERNAL) == false);
            if (cbInputRange.Text.Contains(RANGE_VDDA) || cbInputRange.Text.Contains(RANGE_VDAC))
            {
                numRefVoltage.Enabled = true;
                Volts_label.Text = "Volts";
                if (cbInputRange.Text.Contains(RANGE_VDAC))
                {
                    if (isInternalRef)
                        Volts_label.Text = "Volts (Vdac)";
                }
                else if (cbInputRange.Text.Contains(RANGE_VDDA))
                {
                    if (isInternalRef)
                    {
                        Volts_label.Text = "Volts (Vdda)";
                        numRefVoltage.Enabled = false;

                        // Getting system Vdda and assigning it to voltage reference if Vdda range
                        string vnegVddaDiff = CyDictParser.GetDictValue(m_params.m_dnDict,
                            CyEAdcInputRangeType.Vneg_Vdda_Diff);
                        if (cbInputRange.SelectedItem.ToString() == vnegVddaDiff)
                        {
                            numRefVoltage.Text = m_params.m_systemVdda.ToString();
                        }
                        else
                        {
                            decimal dividedVdda = (decimal)m_params.m_systemVdda / 2;
                            numRefVoltage.Text = dividedVdda.ToString();
                            if (isInternalRef)
                                Volts_label.Text = "Volts (Vdda/2)";
                        }
                    }
                }
                else
                {
                    Volts_label.Text = "Volts";
                }
            }
            else
            {
                Volts_label.Text = "Volts";
                if (isInternalRef)
                {
                    numRefVoltage.Value = 1.024M;
                    numRefVoltage.Enabled = false;
                }
                else
                {
                    numRefVoltage.Enabled = true;
                }
            }
        }
        #endregion
    }
}
