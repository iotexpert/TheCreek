//*******************************************************************************
// File Name: CyADC_DelSigControl1.cs
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
using ADC_DelSig_v1_10;


namespace ADC_DelSig_v1_10
{
    public partial class CyADC_DelSigControl1 : UserControl
    {
        #region Param Name Constants 
        const string CONVERSION_MODE_NAME = "Conversion_Mode";
        const string RESOLUTION_NAME = "ADC_Resolution";
        const string INPUT_RANGE_NAME = "ADC_Input_Range";
        const string INPUT_BUFFER_NAME = "Input_Buffer_Gain";
        const string ADC_POWER_NAME = "ADC_Power";
        const string ADC_REFERENCE_NAME = "ADC_Reference";
        const string REF_VOLTAGE_NAME = "Ref_Voltage";
        const string SAMPLE_RATE_NAME = "Sample_Rate";
        const string ADC_CLOCK_NAME = "ADC_Clock";
        const string START_CONV_NAME = "Start_of_Conversion";

        #endregion

        #region Enumerated Type String Names
        const string RANGE_VSSA_TO_VDDA = "Vssa to Vdda (Single Ended)";
        const string REF_INTERNAL_REF = "Internal Ref";
        
        #region References to Enum Type String Names
        const string REF_EXTERNAL = "External Ref";
        #endregion
        #endregion



        public ICyInstEdit_v1 m_Component = null;

        public CyADC_DelSigControl1(ICyInstEdit_v1 inst)
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

            // Input Buffer Gain
            IEnumerable<string> InputBufferGainEnums = inst.GetPossibleEnumValues(INPUT_BUFFER_NAME);
            foreach (string str in InputBufferGainEnums)
            {
                m_cbInputBufferGain.Items.Add(str);
            }

            // Conversion Mode
            IEnumerable<string> ConversionModeEnums = inst.GetPossibleEnumValues(CONVERSION_MODE_NAME);
            foreach (string str in ConversionModeEnums)
            {
                m_cbConvMode.Items.Add(str);
            }

            // Reference Mode
            IEnumerable<string> ReferenceEnums = inst.GetPossibleEnumValues(ADC_REFERENCE_NAME);
            foreach (string str in ReferenceEnums)
            {
                m_cbReference.Items.Add(str);
                //    MessageBox.Show(string.Format("Options {0} \n\nErrors:\n{2}", str, MessageBoxButtons.OK, MessageBoxIcon.Error));
            }

            if (m_Component != null)
            {
                UpdateFormFromParams(inst);
            }
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            ADC_DelSigParameters prms = new ADC_DelSigParameters(inst);

            //Set the ADC resolution
            IEnumerable<string> ResolutionEnums = inst.GetPossibleEnumValues(RESOLUTION_NAME);
            bool resolutionFound = false;
            foreach (string str in ResolutionEnums)
            {
                if (!resolutionFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(RESOLUTION_NAME, prms.ADC_Resolution.Expr);
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
                m_cbResolution.Text = prms.ADC_Resolution.Expr;
            }


            //Set the ADC Power
            IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues(ADC_POWER_NAME);
            bool powerFound = false;
            foreach (string str in PowerEnums)
            {
                if (!powerFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(ADC_POWER_NAME, prms.ADC_Power.Expr);
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
                m_cbPower.Text = prms.ADC_Power.Expr;
            }

            //Set the ADC Input Range
            IEnumerable<string> InputRangeEnums = inst.GetPossibleEnumValues(INPUT_RANGE_NAME);
            bool inputFound = false;
            foreach (string str in InputRangeEnums)
            {
                if (!inputFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(INPUT_RANGE_NAME, prms.ADC_Input_Range.Expr);
                    //  MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbInputRange.SelectedItem = paramcompare;
                        inputFound = true;
                    }
                }
            }
            if (inputFound == false)
            {
                m_cbInputRange.Text = prms.ADC_Input_Range.Expr;
            }


            //Set the Input Buffer Gain
            IEnumerable<string> InputBufferGainEnums = inst.GetPossibleEnumValues(INPUT_BUFFER_NAME);
            bool inputBufFound = false;
            foreach (string str in InputBufferGainEnums)
            {
                if (!inputBufFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(INPUT_BUFFER_NAME, prms.Input_Buffer_Gain.Expr);
                    //  MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbInputBufferGain.SelectedItem = paramcompare;
                        inputBufFound = true;
                    }
                }
            }
            if (inputBufFound == false)
            {
                m_cbInputBufferGain.Text = prms.Input_Buffer_Gain.Expr;
            }


            //Set the Conversion Mode
            IEnumerable<string> ConversionModeEnums = inst.GetPossibleEnumValues(CONVERSION_MODE_NAME);
            bool convModeFound = false;
            foreach (string str in ConversionModeEnums)
            {
                if (!convModeFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(CONVERSION_MODE_NAME, prms.Conversion_Mode.Expr);
                    //  MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbConvMode.SelectedItem = paramcompare;
                        convModeFound = true;
                    }
                }
            }
            if (convModeFound == false)
            {
                m_cbConvMode.Text = prms.Conversion_Mode.Expr;
            }


            //Set ADC Reference
            IEnumerable<string> ReferenceEnums = inst.GetPossibleEnumValues(ADC_REFERENCE_NAME);
            bool referenceFound = false;
            foreach (string str in ReferenceEnums)
            {
                if (!referenceFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay(ADC_REFERENCE_NAME, prms.ADC_Reference.Expr);
                    //         MessageBox.Show(string.Format("compare {0} with {1}\n\nErrors:\n{2}", str, paramcompare, MessageBoxButtons.OK, MessageBoxIcon.Error));

                    if (paramcompare == str)
                    {
                        m_cbReference.SelectedItem = paramcompare;
                        referenceFound = true;
                    }
                }
            }
            if (referenceFound == false)
            {
                m_cbReference.Text = prms.ADC_Reference.Expr;
            }



            // Clock Source check box
            if (m_Component.ResolveEnumIdToDisplay(ADC_CLOCK_NAME, prms.ADC_Clock.Expr) == "Internal")
            {
                m_rbClockInternal.Checked = true;
                m_rbClockExternal.Checked = false;
            }
            else
            {
                m_rbClockInternal.Checked = false;
                m_rbClockExternal.Checked = true;
            }

            // Start of Conversion check box
            if (m_Component.ResolveEnumIdToDisplay(START_CONV_NAME, prms.Start_of_Conversion.Expr) == "Software")
            {
                m_rbSocSoftware.Checked = true;
                m_rbSocHardware.Checked = false;
                SetAParameter(START_CONV_NAME, "Software", true);
            }
            else
            {
                m_rbSocSoftware.Checked = false;
                m_rbSocHardware.Checked = true;
                SetAParameter(START_CONV_NAME, "Hardware", true);
            }

            // Conversion/Sample Rate
            int temp;
            int.TryParse(prms.Sample_Rate.Value, out temp);
            m_nudConvRate.Value = (decimal)temp;

            float tempFloat;
            float.TryParse(prms.Ref_Voltage.Value, out tempFloat);
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
        private void m_cbInputRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(INPUT_RANGE_NAME, m_cbInputRange.Text);
            SetAParameter(INPUT_RANGE_NAME, prm, true);

            // If "Vssa to Vdda (Single Ended)" is selected then choose refernce to be "Internal Ref"
            if (m_cbInputRange.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference.Text = REF_INTERNAL_REF;
                m_cbReference.Enabled = false;
            }
            else
                m_cbReference.Enabled = true;

            UpdateRefVoltageEnable();
        }

        // Input Buffer Gain
        private void m_cbInputBufferGain_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(INPUT_BUFFER_NAME, m_cbInputBufferGain.Text);
            SetAParameter(INPUT_BUFFER_NAME, prm, true);
        }

        // Conversion Mode
        private void m_cbConvMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(CONVERSION_MODE_NAME, m_cbConvMode.Text);
            SetAParameter(CONVERSION_MODE_NAME, prm, true);
            CheckFreq();
        }
        
        // ADC Reference
        private void m_cbReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_REFERENCE_NAME, m_cbReference.Text);
            SetAParameter(ADC_REFERENCE_NAME, prm, true);

            UpdateRefVoltageEnable();
        }


        // Clock source radio buttons
        // Internal clock
        private void m_rbClockInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockInternal.Checked)
            {
                SetAParameter(ADC_CLOCK_NAME, "Internal", true);
            }
        }

       
        // External clock
        private void m_rbClockExternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockExternal.Checked)
            {
                SetAParameter(ADC_CLOCK_NAME, "External", true);
            }
        }

        // Start of Conversion
        // Software
        private void m_rbSocSoftware_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSocSoftware.Checked)
            {
                SetAParameter(START_CONV_NAME, "Software", true);

            }
        }

        // Hardware
        private void m_rbSocHardware_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSocHardware.Checked)
            {
                SetAParameter(START_CONV_NAME, "Hardware", true);

            }
        }


        // Conversion Rate
        private void m_nudConvRate_ValueChanged(object sender, EventArgs e)
        {
            CheckFreq();   // Check ADC clock frequency range.
            SetAParameter(SAMPLE_RATE_NAME, m_nudConvRate.Value.ToString(), false);
        }

       
        // Reference Voltage
        private void m_nudRefVoltage_ValueChanged(object sender, EventArgs e)
        {
            // TODO
            SetAParameter(REF_VOLTAGE_NAME, m_nudRefVoltage.Value.ToString(), false);
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

        // Get the ADC clock frequency for the current settings.
        private int GetFreq()
        {
            int theFreq;
            theFreq = (int)CyCustomizer.SetClock(
                           uint.Parse(m_Component.GetCommittedParam(RESOLUTION_NAME).Value),
                           (uint)m_nudConvRate.Value,
                           uint.Parse(m_Component.GetCommittedParam(CONVERSION_MODE_NAME).Value));
            return theFreq;
        }

        // Check the current ADC settings for clock frequencies that are beyond the clock specifications.
        private void CheckFreq()
        {
            float theFreq;
            float theFreqKHz;
            int resolution;
            float TheMaxFreq;
            float TheMinFreq;
            float TheMaxFreqKHz;
            float TheMinFreqKHz;
            string errorMessage;

            resolution = int.Parse(m_Component.GetCommittedParam(RESOLUTION_NAME).Value);

            if(resolution > 14)
            {
                TheMaxFreq = (float)rc.MAX_FREQ_15_20_BITS;
                TheMinFreq = (float)rc.MIN_FREQ_15_20_BITS;
            }
            else
            {
                TheMaxFreq = (float)rc.MAX_FREQ_8_14_BITS;
                TheMinFreq = (float)rc.MIN_FREQ_8_14_BITS;
            }

            theFreq = (float)GetFreq();
            theFreqKHz = theFreq / (float)1000.0;
            TheMaxFreqKHz = TheMaxFreq / (float)1000.0;
            TheMinFreqKHz = (float)(TheMinFreq / 1000.0);

            // Compare to valid value
            if (theFreq > TheMaxFreq)
            {
                errorMessage = "The ADC clock frequency of " + theFreqKHz.ToString("0.000") + " kHz has exceeded the maximum limit of " + TheMaxFreqKHz.ToString("0.000") + " kHz for the conversion mode, resolution and sample rate.";
                m_errorProvider.SetError(m_nudConvRate, errorMessage);
                m_errorProvider.SetError(m_cbResolution, errorMessage);
                m_errorProvider.SetError(m_cbConvMode, errorMessage);
                m_errorProvider.SetError(m_tbClockFreq, errorMessage);
            }
            else if (theFreq < TheMinFreq)
            {
                errorMessage = "The clock frequency of " + theFreqKHz.ToString("0.000") + " kHz is below the minimum limit of " + TheMinFreqKHz.ToString("0.000") +  " kHz for the conversion mode, resolution and sample rate.";
                m_errorProvider.SetError(m_nudConvRate, errorMessage);
                m_errorProvider.SetError(m_cbResolution, errorMessage);
                m_errorProvider.SetError(m_cbConvMode, errorMessage);
                m_errorProvider.SetError(m_tbClockFreq, errorMessage);
            }
            else
            {
                m_errorProvider.SetError(m_cbResolution, String.Empty);
                m_errorProvider.SetError(m_cbConvMode, String.Empty);
                m_errorProvider.SetError(m_nudConvRate, String.Empty);
                m_errorProvider.SetError(m_tbClockFreq, String.Empty);
            }

            SetAParameter(SAMPLE_RATE_NAME, m_nudConvRate.Value.ToString(), false);
            m_tbClockFreq.Text = theFreqKHz.ToString("0.000");


        }

        // Check what combination of Input Range and Reference is selected
        private void UpdateRefVoltageEnable()
        {
            if (m_cbReference.Text.Contains(REF_EXTERNAL) || (m_cbInputRange.Text == RANGE_VSSA_TO_VDDA))
            {
                m_nudRefVoltage.Enabled = true;
                if (m_cbInputRange.Text == RANGE_VSSA_TO_VDDA)
                {
                    Volts_label.Text = "Volts (Vdda)";
                    m_nudRefVoltage.Maximum = 5.5M;
                }
                else
                {
                    Volts_label.Text = "Volts";
                    m_nudRefVoltage.Maximum = 1.5M;
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
