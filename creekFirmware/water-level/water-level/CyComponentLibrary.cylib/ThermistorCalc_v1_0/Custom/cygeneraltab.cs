/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace ThermistorCalc_v1_0
{
    public partial class CyGeneralTab : UserControl, ICyParamEditingControl
    {
        private bool m_runMode = false;

        protected ErrorProvider m_errorProvider = null;
        protected CyParameters m_params = null;

        List<NumericUpDown> m_tempControls;
        List<NumericUpDown> m_resistControls;

        public bool RunMode
        {
            get { return m_runMode; }
            set { m_runMode = value; }
        }

        public string TabName
        {
            get { return "Basic"; }
        }

        #region Constructor(s)
        public CyGeneralTab(CyParameters parameters)
        {
            m_params = parameters;
            this.Load += delegate(object sender, EventArgs e)
            {
                if (RunMode)
                {
                    this.Dock = DockStyle.Fill;
                    this.AutoScroll = true;
                }
            };

            InitUI();
        }
        #endregion

        #region User interface
        private void InitUI()
        {
            const decimal DEVIATION_VALUE = 100000000000;

            InitializeComponent();

            m_errorProvider = new ErrorProvider();
            m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            #region Initialize auxiliary lists
            m_tempControls = new List<NumericUpDown>(
                new NumericUpDown[]
                {
                    m_numMinTemp,
                    m_numMidTemp,
                    m_numMaxTemp
                }
            );

            m_resistControls = new List<NumericUpDown>(
                new NumericUpDown[]
                {
                    m_numMinResist,
                    m_numMidResist,
                    m_numMaxResist
                }
            );
            #endregion

            #region Configure components
            m_cbCalcResolution.Items.AddRange(m_params.GetEnumDescriptions(CyParamNames.CALCULATION_RESOLUTION));

            this.m_numRefResistor.Minimum = CyParamRanges.MIN_REFERENCE_RESISTANCE - DEVIATION_VALUE;
            this.m_numRefResistor.Maximum = CyParamRanges.MAX_REFERENCE_RESISTANCE + DEVIATION_VALUE;

            foreach (NumericUpDown numControl in m_tempControls)
            {
                numControl.Minimum = CyParamRanges.MIN_TEMPERATURE - DEVIATION_VALUE;
                numControl.Maximum = CyParamRanges.MAX_TEMPERATURE + DEVIATION_VALUE;
            }

            foreach (NumericUpDown numControl in m_resistControls)
            {
                numControl.Minimum = (int)CyParamRanges.MIN_RESISTANCE - DEVIATION_VALUE;
                numControl.Maximum = CyParamRanges.MAX_RESISTANCE + DEVIATION_VALUE;
            }
            #endregion

            #region Assigning event handlers
            this.m_numRefResistor.TextChanged += new EventHandler(m_numRefResistor_TextChanged);
            this.m_cbCalcResolution.SelectedIndexChanged += new EventHandler(m_cbCalcResolution_SelectedIndexChanged);
            
            this.m_rbEquation.CheckedChanged += new EventHandler(m_rbImplementation_CheckedChanged);
            this.m_rbLut.CheckedChanged += new EventHandler(m_rbImplementation_CheckedChanged);

            foreach (NumericUpDown numControl in m_tempControls)
            {
                numControl.TextChanged += new EventHandler(m_numTermParam_TextChanged);
            }
            foreach (NumericUpDown numControl in m_resistControls)
            {
                numControl.TextChanged += new EventHandler(m_numTermParam_TextChanged);
            }
            #endregion

            #region LUT error
            m_errorProvider.SetIconAlignment(labelLutError, ErrorIconAlignment.TopLeft);
            #endregion
        }

        public void UpdateUI()
        {
            m_numRefResistor.Value = m_params.ReferenceResistance;

            // Implementation
            switch (m_params.Implementation)
            {
                case CyEImplementation.Equation:
                    m_rbEquation.Checked = true;
                    break;
                case CyEImplementation.LUT:
                    m_rbLut.Checked = true;
                    break;
            }

            m_numMinTemp.Value = m_params.MinTemperature;
            m_numMidTemp.Value = m_params.MidTemperature;
            m_numMaxTemp.Value = m_params.MaxTemperature;

            m_numMinResist.Value = m_params.MinResistance;
            m_numMidResist.Value = m_params.MidResistance;
            m_numMaxResist.Value = m_params.MaxResistance;

            m_cbCalcResolution.SelectedItem = m_params.GetValueDescription(CyParamNames.CALCULATION_RESOLUTION,
                m_params.CalculationResolutionValue);

            UpdateInfoText();
        }
        #endregion

        #region ICyParamEditingControl members
        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            string errorMessage = string.Empty;

            if (m_errorProvider != null)
            {
                // Check controls for errors
                foreach (Control control in this.Controls)
                {
                    errorMessage = m_errorProvider.GetError(control);
                    if (string.IsNullOrEmpty(errorMessage) == false)
                        errs.Add(new CyCustErr(errorMessage));

                    // Check controls inside groupbox
                    foreach (Control internalControl in control.Controls)
                    {
                        errorMessage = m_errorProvider.GetError(internalControl);
                        if (string.IsNullOrEmpty(errorMessage) == false)
                            errs.Add(new CyCustErr(errorMessage));
                    }
                }
            }

            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(TabName))
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

            return errs;
        }
        #endregion

        #region Event handlers
        void m_numRefResistor_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;

            uint value;

            if (uint.TryParse(control.Text, out value) && value >= CyParamRanges.MIN_REFERENCE_RESISTANCE &&
                value <= CyParamRanges.MAX_REFERENCE_RESISTANCE)
            {
                m_errorProvider.SetError(control, string.Empty);
                m_params.ReferenceResistance = value;
            }
            else
            {
                m_errorProvider.SetError(control, string.Format(Resources.ReferenceResistanceValueErrorDescription,
                    CyParamRanges.MIN_REFERENCE_RESISTANCE, CyParamRanges.MAX_REFERENCE_RESISTANCE));
            }
        }

        void m_cbCalcResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == m_cbCalcResolution)
            {
                m_params.CalculationResolutionValue = m_params.GetEnumValue<CyECalcResolution>(
                    CyParamNames.CALCULATION_RESOLUTION, m_cbCalcResolution.Text);
                ValidateLUTSize();
            }
        }

        void m_rbImplementation_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                if (rb == m_rbEquation)
                {
                    m_params.Implementation = CyEImplementation.Equation;
                    m_cbCalcResolution.Enabled = false;
                    m_lblCalcResolution.Enabled = false;
                }
                else if (rb == m_rbLut)
                {
                    m_params.Implementation = CyEImplementation.LUT;
                    m_cbCalcResolution.Enabled = true;
                    m_lblCalcResolution.Enabled = true;
                }

                ValidateLUTSize();
            }
        }

        void m_numTermParam_TextChanged(object sender, EventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;

            if (m_tempControls.Contains(control))
            {
                ProcessTemperatures(control);
            }
            else if (m_resistControls.Contains(control))
            {
                ProcessResistances(control);
            }
        }

        private void labelLutError_VisibleChanged(object sender, EventArgs e)
        {
            if (labelLutError.Visible)
            {
                m_errorProvider.SetError(labelLutError, Resources.LUTErrorDescription);
            }
            else
            {
                m_errorProvider.SetError(labelLutError, "");
            }
        }
        #endregion

        #region Auxiliary methods
        void UpdateInfoText()
        {
            m_rtbInfo.Text = string.Format(Resources.InfoString, m_params.LUTSize);
            m_rtbInfo.SelectAll();
            m_rtbInfo.SelectionColor = Color.Black;
            m_rtbInfo.Enabled = false;

            UpdateLabelLutError();
        }

        void UpdateLabelLutError()
        {
            labelLutError.Visible = !ValidateLUT() && (m_params.Implementation == CyEImplementation.LUT);
        }

        bool ValidateLUT()
        {
            const double EPS = 1e-10;

            double a, b, c;
            m_params.CalculateSteinhartHartCoefficients(out a, out b, out c);

            if (Math.Abs(c) < EPS)
            {
                return false;
            }

            double t = m_params.MinTemperature;
            double resolution = m_params.CalculationResolution;
            int lutSize = m_params.LUTSize;
            for (int i = 0; i < lutSize; i++)
            {
                double tmp = Math.Pow(b / (3 * c), 3) + Math.Pow(((a - 1 / CyParameters.ToKelvin(t)) / c), 2) / 4;
                if (tmp < 0.0)
                {
                    return false;
                }
                t += resolution;
            }

            return true;
        }

        #region Validating thermistor parameters
        const int MIN = 0;
        const int MID = 1;
        const int MAX = 2;
        const string LESS = "less";
        const string GREAT = "greater";
        readonly string[] PARAM_INDEX_NAME = new string[]
        {
            "min",
            "mid",
            "max"
        };

        void ProcessTemperatures(NumericUpDown control)
        {
            const string VALUE_STRING = "temperature";

            short[] temp = new short[m_tempControls.Count];
            bool[] isValidTemp = new bool[m_tempControls.Count];

            // Get all temperatures values, validating values and ranges
            for (int i = 0; i < m_tempControls.Count; i++)
            {
                isValidTemp[i] = true;
                m_errorProvider.SetError(m_tempControls[i], string.Empty);
                if (!short.TryParse(m_tempControls[i].Text, out temp[i]) || temp[i] < CyParamRanges.MIN_TEMPERATURE ||
                    temp[i] > CyParamRanges.MAX_TEMPERATURE)
                {
                    m_errorProvider.SetError(m_tempControls[i], string.Format(Resources.ResistanceValueError,
                        CyParamRanges.MIN_TEMPERATURE, CyParamRanges.MAX_TEMPERATURE));
                    isValidTemp[i] = false;
                }
            }

            // Validating temperatures correlation
            if (isValidTemp[MIN])
            {
                if (isValidTemp[MID] && temp[MIN] >= temp[MID])
                {
                    m_errorProvider.SetError(m_tempControls[MIN], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MIN], VALUE_STRING, LESS, PARAM_INDEX_NAME[MID]));
                    m_errorProvider.SetError(m_tempControls[MID], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MID], VALUE_STRING, GREAT, PARAM_INDEX_NAME[MIN]));
                }
                else if (isValidTemp[MAX] && temp[MIN] >= temp[MAX])
                {
                    m_errorProvider.SetError(m_tempControls[MIN], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MIN], VALUE_STRING, LESS, PARAM_INDEX_NAME[MAX]));
                    m_errorProvider.SetError(m_tempControls[MAX], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MAX], VALUE_STRING, GREAT, PARAM_INDEX_NAME[MIN]));
                }
            }

            if (isValidTemp[MID] && isValidTemp[MAX] && temp[MID] >= temp[MAX])
            {
                m_errorProvider.SetError(m_tempControls[MID], string.Format(Resources.TermParamCompareError,
                    PARAM_INDEX_NAME[MID], VALUE_STRING, LESS, PARAM_INDEX_NAME[MAX]));
                m_errorProvider.SetError(m_tempControls[MAX], string.Format(Resources.TermParamCompareError,
                    PARAM_INDEX_NAME[MAX], VALUE_STRING, GREAT, PARAM_INDEX_NAME[MID]));
            }

            // Set temperature
            if (string.IsNullOrEmpty(m_errorProvider.GetError(m_numMinTemp)) && m_params.MinTemperature != temp[MIN])
            {
                m_params.MinTemperature = temp[MIN];
            }
            if (string.IsNullOrEmpty(m_errorProvider.GetError(m_numMidTemp)) && m_params.MidTemperature != temp[MID])
            {
                m_params.MidTemperature = temp[MID];
            }
            if (string.IsNullOrEmpty(m_errorProvider.GetError(m_numMaxTemp)) && m_params.MaxTemperature != temp[MAX])
            {
                m_params.MaxTemperature = temp[MAX];
            }
            if (isValidTemp[MIN] && isValidTemp[MID] && isValidTemp[MAX])
            {
                ValidateLUTSize();
            }
        }

        void ProcessResistances(NumericUpDown control)
        {
            const string VALUE_STRING = "resistance";

            uint[] resist = new uint[m_resistControls.Count];
            bool[] isValidResist = new bool[m_resistControls.Count];

            // Get all resistances values, validating values and ranges
            for (int i = 0; i < m_resistControls.Count; i++)
            {
                isValidResist[i] = true;
                m_errorProvider.SetError(m_resistControls[i], string.Empty);
                if (!uint.TryParse(m_resistControls[i].Text, out resist[i]) ||
                    resist[i] < CyParamRanges.MIN_RESISTANCE || resist[i] > CyParamRanges.MAX_RESISTANCE)
                {
                    m_errorProvider.SetError(m_resistControls[i], string.Format(Resources.ResistanceValueError,
                        CyParamRanges.MIN_RESISTANCE, CyParamRanges.MAX_RESISTANCE));
                    isValidResist[i] = false;
                }
            }

            // Validating resistances correlation
            if (isValidResist[MAX])
            {
                if (isValidResist[MID] && resist[MAX] >= resist[MID])
                {
                    m_errorProvider.SetError(m_resistControls[MAX], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MAX], VALUE_STRING, LESS, PARAM_INDEX_NAME[MID]));
                    m_errorProvider.SetError(m_resistControls[MID], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MID], VALUE_STRING, GREAT, PARAM_INDEX_NAME[MAX]));
                }
                else if (isValidResist[MIN] && resist[MAX] >= resist[MIN])
                {
                    m_errorProvider.SetError(m_resistControls[MAX], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MAX], VALUE_STRING, LESS, PARAM_INDEX_NAME[MIN]));
                    m_errorProvider.SetError(m_resistControls[MIN], string.Format(Resources.TermParamCompareError,
                        PARAM_INDEX_NAME[MIN], VALUE_STRING, GREAT, PARAM_INDEX_NAME[MAX]));
                }
            }

            if (isValidResist[MID] && isValidResist[MIN] && resist[MID] >= resist[MIN])
            {
                m_errorProvider.SetError(m_resistControls[MID], string.Format(Resources.TermParamCompareError,
                    PARAM_INDEX_NAME[MID], VALUE_STRING, LESS, PARAM_INDEX_NAME[MIN]));
                m_errorProvider.SetError(m_resistControls[MIN], string.Format(Resources.TermParamCompareError,
                    PARAM_INDEX_NAME[MIN], VALUE_STRING, GREAT, PARAM_INDEX_NAME[MIN]));
            }

            // Set resistance
            if (string.IsNullOrEmpty(m_errorProvider.GetError(m_numMinResist)) && m_params.MinResistance != resist[MIN])
            {
                m_params.MinResistance = resist[MIN];
            }
            if (string.IsNullOrEmpty(m_errorProvider.GetError(m_numMidResist)) && m_params.MidResistance != resist[MID])
            {
                m_params.MidResistance = resist[MID];
            }
            if (string.IsNullOrEmpty(m_errorProvider.GetError(m_numMaxResist)) && m_params.MaxResistance != resist[MAX])
            {
                m_params.MaxResistance = resist[MAX];
            }

            UpdateLabelLutError();
        }

        void ValidateLUTSize()
        {
            string errMsgLutSize = string.Format(Resources.LUTSizeValueError, CyParamRanges.MAX_LUT_SIZE);
            string errMsg = (m_params.LUTSize > CyParamRanges.MAX_LUT_SIZE &&
                m_params.Implementation == CyEImplementation.LUT) ?
                string.Format(Resources.LUTSizeValueError, CyParamRanges.MAX_LUT_SIZE) :
                string.Empty;
            string maxTempErr = m_errorProvider.GetError(m_numMaxTemp);
            string minTempErr = m_errorProvider.GetError(m_numMinTemp);

            m_errorProvider.SetError(m_cbCalcResolution, string.Empty);
            if (string.IsNullOrEmpty(maxTempErr) || maxTempErr == errMsgLutSize)
            {
                m_errorProvider.SetError(m_numMaxTemp, string.Empty);
            }
            if (string.IsNullOrEmpty(minTempErr) || minTempErr == errMsgLutSize)
            {
                m_errorProvider.SetError(m_numMinTemp, string.Empty);
            }
            if (string.IsNullOrEmpty(m_errorProvider.GetError(m_numMaxTemp)) &&
                string.IsNullOrEmpty(m_errorProvider.GetError(m_numMinTemp)))
            {
                m_errorProvider.SetError(m_cbCalcResolution, errMsg);
                m_errorProvider.SetError(m_numMaxTemp, errMsg);
                m_errorProvider.SetError(m_numMinTemp, errMsg);
            }
            UpdateInfoText();
        }
        #endregion

        #endregion
    }
}
