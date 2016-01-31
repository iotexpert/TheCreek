//*******************************************************************************
// File Name: CyADC_SARControl.cs
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
using ADC_SAR_v1_10;


namespace ADC_SAR_v1_10
{
    public partial class CyADC_SARControl : UserControl
    {
        #region Param Name Constants
        const string RESOLUTION_NAME = "ADC_Resolution";
        const string INPUT_RANGE_NAME = "ADC_Input_Range";
        const string ADC_POWER_NAME = "ADC_Power";
        const string ADC_REFERENCE_NAME = "ADC_Reference";
        const string REF_VOLTAGE_NAME = "Ref_Voltage";
        const string SAMPLE_RATE_NAME = "Sample_Rate";
        const string ADC_CLOCK_NAME = "ADC_Clock";
        const string ADC_CLOCK_FREQUENCY_NAME = "ADC_Clock_Frequency";
        const string START_CONV_NAME = "Start_of_Conversion";
        const string ADC_SAMPLE_MODE_NAME = "ADC_SampleMode";
        #endregion

        #region Enumerated Type String Names
        const string RANGE_VSSA_TO_VDDA = "Vssa to Vdda (Single Ended)";
        const string RANGE_VDDA = "Vdda";
        const string RANGE_VDAC = "VDAC";
        const string REF_INTERNAL_REF = "Internal Vref";

        #region References to Enum Type String Names
        const string REF_EXTERNAL = "External Vref";
        const string REF_VDAC = "DAC Vref";
        #endregion
        #endregion

        public ICyInstEdit_v1 m_Component = null;

        public CyADC_SARControl(ICyInstEdit_v1 inst)
        {

            m_Component = inst;

            InitializeComponent();

            // ADC Resolution
            IEnumerable<string> ResolutionEnums = inst.GetPossibleEnumValues(RESOLUTION_NAME);
            foreach (string str in ResolutionEnums)
            {
                m_cbResolution.Items.Add(str);
            }

            // ADC Power
            IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues(ADC_POWER_NAME);
            foreach (string str in PowerEnums)
            {
                m_cbPower.Items.Add(str);
            }

            // ADC Input Range
            IEnumerable<string> InputRangeEnums = inst.GetPossibleEnumValues(INPUT_RANGE_NAME);
            foreach (string str in InputRangeEnums)
            {
                m_cbInputRange.Items.Add(str);
            }

            // Reference Mode
            IEnumerable<string> ReferenceEnums = inst.GetPossibleEnumValues(ADC_REFERENCE_NAME);
            foreach (string str in ReferenceEnums)
            {
                m_cbReference.Items.Add(str);
                //MessageBox.Show(string.Format("Options {0} \n\nErrors:\n{2}", str, MessageBoxButtons.OK, MessageBoxIcon.Error));
            }

            if (m_Component != null)
            {
                UpdateFormFromParams(inst);
            }
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            ADC_SARParameters prms = new ADC_SARParameters(inst);

            //Set the ADC resolution
            IEnumerable<string> ResolutionEnums = inst.GetPossibleEnumValues(RESOLUTION_NAME);
            bool resolutionFound = false;
            foreach (string str in ResolutionEnums)
            {
                if (!resolutionFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(RESOLUTION_NAME, prms.m_ADC_Resolution.Expr);
                    //MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbResolution.SelectedItem = paramcompare;
                        resolutionFound = true;
                    }
                }
            }
            if (resolutionFound == false)
            {
                m_cbResolution.Text = prms.m_ADC_Resolution.Expr;
            }


            //Set the ADC Power
            IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues(ADC_POWER_NAME);
            bool powerFound = false;
            foreach (string str in PowerEnums)
            {
                if (!powerFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(ADC_POWER_NAME, prms.m_ADC_Power.Expr);
                    //MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbPower.SelectedItem = paramcompare;
                        powerFound = true;
                    }
                }
            }
            if (powerFound == false)
            {
                m_cbPower.Text = prms.m_ADC_Power.Expr;
            }

            //Set the ADC Input Range
            IEnumerable<string> InputRangeEnums = inst.GetPossibleEnumValues(INPUT_RANGE_NAME);
            bool inputFound = false;
            foreach (string str in InputRangeEnums)
            {
                if (!inputFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(INPUT_RANGE_NAME, prms.m_ADC_Input_Range.Expr);
                    // MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbInputRange.SelectedItem = paramcompare;
                        inputFound = true;
                    }
                }
            }
            if (inputFound == false)
            {
                m_cbInputRange.Text = prms.m_ADC_Input_Range.Expr;
            }


            //Set ADC Reference
            IEnumerable<string> ReferenceEnums = inst.GetPossibleEnumValues(ADC_REFERENCE_NAME);
            bool referenceFound = false;
            foreach (string str in ReferenceEnums)
            {
                if (!referenceFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(ADC_REFERENCE_NAME, prms.m_ADC_Reference.Expr);
                    //MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbReference.SelectedItem = paramcompare;
                        referenceFound = true;
                    }
                }
            }
            if (referenceFound == false)
            {
                m_cbReference.Text = prms.m_ADC_Reference.Expr;
            }


            // Clock Source check box
            if (m_Component.ResolveEnumIdToDisplay(ADC_CLOCK_NAME, prms.m_ADC_Clock.Expr) == "Internal")
            {
                m_rbClockInternal.Checked = true;
                m_rbClockExternal.Checked = false;
                //m_tbClockFreq.Enabled = true;
                //m_tbClockFreq.Visible = true;
                //Clock_label.Visible = true;
                //kHz_label.Visible = true;
            }
            else
            {
                m_rbClockInternal.Checked = false;
                m_rbClockExternal.Checked = true;
                //m_tbClockFreq.Enabled = false;
                //m_tbClockFreq.Visible = false;
                //Clock_label.Visible = false;
                //kHz_label.Visible = false;
            }

            // Sample Mode check box
            if (m_Component.ResolveEnumIdToDisplay(ADC_SAMPLE_MODE_NAME, prms.m_ADC_SampleMode.Expr) == "FreeRunning")
            {
                m_rbSocFreeRunninge.Checked = true;
                m_rbSocTrigerred.Checked = false;
                //SetAParameter(ADC_SAMPLE_MODE_NAME, "FreeRunning", true);
            }
            else
            {
                m_rbSocFreeRunninge.Checked = false;
                m_rbSocTrigerred.Checked = true;
                //SetAParameter(ADC_SAMPLE_MODE_NAME, "Triggered", true);
            }

            // Conversion/Sample Rate
            int temp;
            int.TryParse(prms.m_Sample_Rate.Value, out temp);
            m_nudConvRate.Value = (decimal)temp;
            //MessageBox.Show(string.Format("SR = {0} ", temp, MessageBoxButtons.OK, MessageBoxIcon.Error));


            float tempFloat;
            float.TryParse(prms.m_Ref_Voltage.Value, out tempFloat);
            m_nudRefVoltage.Value = (decimal)tempFloat;

            CheckFreq();


        }


        // Resolution
        private void m_cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(RESOLUTION_NAME, m_cbResolution.Text);
            SetAParameter(RESOLUTION_NAME, prm, true);
            CheckFreq();
        }

        // ADC Power
        private void m_cbPower_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_POWER_NAME, m_cbPower.Text);
            SetAParameter(ADC_POWER_NAME, prm, true);
        }

        // ADC Input Range
        private void m_cbInputRange_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(INPUT_RANGE_NAME, m_cbInputRange.Text);
            SetAParameter(INPUT_RANGE_NAME, prm, true);

            // If "Vssa to VDAC selected then choose refernce to be "VDAC Ref"
            if (m_cbInputRange.Text.Contains(RANGE_VDAC))
            {
                m_cbReference.Text = REF_INTERNAL_REF; //REF_VDAC; 
                m_cbReference.Enabled = false;
                //MessageBox.Show(m_cbReference.Text);

             }
            // If "Vssa to Vdda (Single Ended)" is selected then choose refernce to be "Internal Ref"
            else if (m_cbInputRange.Text.Contains(RANGE_VDDA))
            {
                m_cbReference.Text = REF_INTERNAL_REF;
                m_cbReference.Enabled = false;
            }
            else
                m_cbReference.Enabled = true;

            UpdateRefVoltageEnable();
        }

        // Clock source radio buttons
        // Internal clock
        private void m_rbClockInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockInternal.Checked)
            {
                SetAParameter(ADC_CLOCK_NAME, "Internal", true);
//                m_tbClockFreq.Enabled = true;
//                m_tbClockFreq.Visible = true;
//                Clock_label.Visible = true;
//                kHz_label.Visible = true;
            }
        }

        // External clock
        private void m_rbClockExternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockExternal.Checked)
            {
                SetAParameter(ADC_CLOCK_NAME, "External", true);
//                m_tbClockFreq.Enabled = false;
//                m_tbClockFreq.Visible = false;
//                Clock_label.Visible = false;
//                kHz_label.Visible = false;
            }
        }

        // Sample Mode
        // Free Running
        private void m_rbSocSoftware_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSocFreeRunninge.Checked)
            {
                SetAParameter(ADC_SAMPLE_MODE_NAME, "FreeRunning", true);

            }
        }

        // Triggered
        private void m_rbSocHardware_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSocTrigerred.Checked)
            {
                SetAParameter(ADC_SAMPLE_MODE_NAME, "Triggered", true);

            }
        }



        // Conversion Rate
        private void m_nudConvRate_ValueChanged(object sender, EventArgs e)
        {

            CheckFreq();   // Check and set ADC clock frequency range.


            // TEST Message to tel me when the conversion rate is being updated
            //           MessageBox.Show(string.Format("New conversion rate {0}\n", m_nudConvRate.Value.ToString()),
            //                      "Conversion rate changed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


        private void SetAParameter(string parameter, string value, bool checkFocus)
        {

            if (this.ContainsFocus || !checkFocus)
            {
                //TODO: Verify that the parameter was set correctly.
                m_Component.SetParamExpr(parameter, value);
                m_Component.CommitParamExprs();

                if (m_Component.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_Component.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ADC_Modes_groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void m_cbReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_REFERENCE_NAME, m_cbReference.Text);
            SetAParameter(ADC_REFERENCE_NAME, prm, true);

            UpdateRefVoltageEnable();

        }

        // Conversion Rate
        private void m_nudConvRate_ValueChanged_1(object sender, EventArgs e)
        {
        }

        // Reference Voltage
        private void m_nudRefVoltage_ValueChanged(object sender, EventArgs e)
        {
            // TODO
            SetAParameter(REF_VOLTAGE_NAME, m_nudRefVoltage.Value.ToString(), false);

        }

        // Get the ADC clock frequency for the current settings.
        private int GetFreq(int resolution)
        {
            int theFreq;
            theFreq = (int)m_nudConvRate.Value * (resolution + 6);
            return theFreq;
        }

        // Check the current ADC settings for clock frequencies that are beyond the clock specifications.
        private void CheckFreq()
        {
            float theFreq;
            float theFreqKHz=0;
            int resolution;
            float TheMaxFreq=18000000;
            float TheMinFreq=1000000;
            float TheMaxFreqKHz;
            float TheMinFreqKHz;
            string errorMessage;

            resolution = int.Parse(m_Component.GetCommittedParam(RESOLUTION_NAME).Value);

            theFreq = (float)GetFreq(resolution);
            theFreqKHz = theFreq / (float)1000.0;
            TheMaxFreqKHz = TheMaxFreq / (float)1000.0;
            TheMinFreqKHz = (float)(TheMinFreq / 1000.0);

            // Compare to valid value
            if (theFreqKHz >= TheMaxFreqKHz)
            {
                errorMessage = "The ADC clock frequency of " + theFreqKHz.ToString("0.000") + " kHz has exceeded the maximum limit of " + TheMaxFreqKHz.ToString("0.000") + " kHz for the conversion mode, resolution and sample rate.";
                //m_errorProvider.SetError(m_nudConvRate, errorMessage);
                //m_errorProvider.SetError(m_cbResolution, errorMessage);
                //m_errorProvider.SetError(m_cbConvMode, errorMessage);
                //m_errorProvider.SetError(m_tbClockFreq, errorMessage);
            }
            else if (theFreqKHz < TheMinFreqKHz)
            {
                errorMessage = "The clock frequency of " + theFreqKHz.ToString("0.000") + " kHz is below the minimum limit of " + TheMinFreqKHz.ToString("0.000") + " kHz for the conversion mode, resolution and sample rate.";
                //m_errorProvider.SetError(m_nudConvRate, errorMessage);
                //m_errorProvider.SetError(m_cbResolution, errorMessage);
                //m_errorProvider.SetError(m_cbConvMode, errorMessage);
                //m_errorProvider.SetError(m_tbClockFreq, errorMessage);
            }
            else
            {
                //m_errorProvider.SetError(m_cbResolution, String.Empty);
                //m_errorProvider.SetError(m_cbConvMode, String.Empty);
                //m_errorProvider.SetError(m_nudConvRate, String.Empty);
                //m_errorProvider.SetError(m_tbClockFreq, String.Empty);
            }


            SetAParameter(SAMPLE_RATE_NAME, m_nudConvRate.Value.ToString(), false);
            m_tbClockFreq.Text = theFreqKHz.ToString("0.000");
            SetAParameter(ADC_CLOCK_FREQUENCY_NAME, theFreq.ToString() , false);


        }

    // Check what combination of Input Range and Reference is selected
        private void UpdateRefVoltageEnable()
        {
            if (m_cbReference.Text.Contains(REF_EXTERNAL) || m_cbInputRange.Text.Contains(RANGE_VDDA) ||
                                                             m_cbInputRange.Text.Contains(RANGE_VDAC))
            {
                m_nudRefVoltage.Enabled = true;
                if (m_cbInputRange.Text.Contains(RANGE_VDAC))
                {
                    Volts_label.Text = "Volts (Vdac)";
                    m_nudRefVoltage.Maximum = 5.5M;
                }
                else if (m_cbInputRange.Text.Contains(RANGE_VDDA))
                {
                    Volts_label.Text = "Volts (Vdda)";
                    m_nudRefVoltage.Maximum = 5.5M;
                }
                else
                {
                    Volts_label.Text = "Volts";
                    m_nudRefVoltage.Maximum = 5.5M;
                }
            }
            else
            {
                m_nudRefVoltage.Maximum = 1.5M;
                Volts_label.Text = "Volts";
                m_nudRefVoltage.Value = 1.024M;
                m_nudRefVoltage.Enabled = false;
            }
        }

    }

    public class CyNumericUpDown : NumericUpDown
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.ValidateEditText();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
