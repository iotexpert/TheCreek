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
using CyCustomizer.ADC_DelSig_v1_0;


namespace CyCustomizer.ADC_DelSig_v1_0
{
    public partial class CyADC_DelSigControl1 : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;

        public CyADC_DelSigControl1(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();

            // ADC Resolution
            IEnumerable<string> ResolutionEnums = inst.GetPossibleEnumValues("ADC_Resolution");
            foreach (string str in ResolutionEnums)
            {
                m_cbResolution.Items.Add(str);
            }

            // ADC Power
            IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues("ADC_Power");
            foreach (string str in PowerEnums)
            {
                m_cbPower.Items.Add(str);
            }

            // ADC Input Range
            IEnumerable<string> InputRangeEnums = inst.GetPossibleEnumValues("ADC_Input_Range");
            foreach (string str in InputRangeEnums)
            {
                m_cbInputRange.Items.Add(str);
            }

            // Input Buffer Gain
            IEnumerable<string> InputBufferGainEnums = inst.GetPossibleEnumValues("Input_Buffer_Gain");
            foreach (string str in InputBufferGainEnums)
            {
                m_cbInputBufferGain.Items.Add(str);
            }

            // Conversion Mode
            IEnumerable<string> ConversionModeEnums = inst.GetPossibleEnumValues("Conversion_Mode");
            foreach (string str in ConversionModeEnums)
            {
                m_cbConvMode.Items.Add(str);
            }

            // Reference Mode
            IEnumerable<string> ReferenceEnums = inst.GetPossibleEnumValues("ADC_Reference");
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
            IEnumerable<string> ResolutionEnums = inst.GetPossibleEnumValues("ADC_Resolution");
            bool resolutionFound = false;
            foreach (string str in ResolutionEnums)
            {
                if (!resolutionFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ADC_Resolution", prms.ADC_Resolution.Expr);
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
            IEnumerable<string> PowerEnums = inst.GetPossibleEnumValues("ADC_Power");
            bool powerFound = false;
            foreach (string str in PowerEnums)
            {
                if (!powerFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ADC_Power", prms.ADC_Power.Expr);
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
            IEnumerable<string> InputRangeEnums = inst.GetPossibleEnumValues("ADC_Input_Range");
            bool inputFound = false;
            foreach (string str in InputRangeEnums)
            {
                if (!inputFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ADC_Input_Range", prms.ADC_Input_Range.Expr);
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
            IEnumerable<string> InputBufferGainEnums = inst.GetPossibleEnumValues("Input_Buffer_Gain");
            bool inputBufFound = false;
            foreach (string str in InputBufferGainEnums)
            {
                if (!inputBufFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("Input_Buffer_Gain", prms.Input_Buffer_Gain.Expr);
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
            IEnumerable<string> ConversionModeEnums = inst.GetPossibleEnumValues("Conversion_Mode");
            bool convModeFound = false;
            foreach (string str in ConversionModeEnums)
            {
                if (!convModeFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("Conversion_Mode", prms.Conversion_Mode.Expr);
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
            IEnumerable<string> ReferenceEnums = inst.GetPossibleEnumValues("ADC_Reference");
            bool referenceFound = false;
            foreach (string str in ReferenceEnums)
            {
                if (!referenceFound)
                {
                    string paramcompare = m_Component.ResolveEnumIdToDisplay("ADC_Reference", prms.ADC_Reference.Expr);
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
            if (m_Component.ResolveEnumIdToDisplay("ADC_Clock", prms.ADC_Clock.Expr) == "Internal")
            {
                m_rbClockInternal.Checked = true;
                m_rbClockExternal.Checked = false;
                m_nudConvRate.Enabled = true;
                m_nudConvRate.Visible = true;
                ConvRate_label.Visible = true;
                SamplesPerSec_label.Visible = true;
            }
            else
            {
                m_rbClockInternal.Checked = false;
                m_rbClockExternal.Checked = true;
                m_nudConvRate.Enabled = false;
                m_nudConvRate.Visible = false;
                ConvRate_label.Visible = false;
                SamplesPerSec_label.Visible = false;
            }

            // Start of Conversion check box
            if (m_Component.ResolveEnumIdToDisplay("Start_of_Conversion", prms.Start_of_Conversion.Expr) == "Software")
            {
                m_rbSocSoftware.Checked = true;
                m_rbSocHardware.Checked = false;
                SetAParameter("Start_of_Conversion", "Software", true);
            }
            else
            {
                m_rbSocSoftware.Checked = false;
                m_rbSocHardware.Checked = true;
                SetAParameter("Start_of_Conversion", "Hardware", true);
            }

            // Conversion/Sample Rate
            int temp;
            int.TryParse(prms.Sample_Rate.Value, out temp);
            // Until sample rates are validated, this will suffice.
            if (temp < 20)
            {
                temp = 20;
            }
            if (temp > 1000000)
            {
                temp = 1000000;
            }
            m_nudConvRate.Value = (decimal)temp;


        }


        // Resolution
        private void m_cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ADC_Resolution", m_cbResolution.Text);
            SetAParameter("ADC_Resolution", prm, true);
        }

        // ADC Power
        private void m_cbPower_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ADC_Power", m_cbPower.Text);
            SetAParameter("ADC_Power", prm, true);
        }

        // ADC Input Range
        private void m_cbInputRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ADC_Input_Range", m_cbInputRange.Text);
            SetAParameter("ADC_Input_Range", prm, true);
        }

        // Input Buffer Gain
        private void m_cbInputBufferGain_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("Input_Buffer_Gain", m_cbInputBufferGain.Text);
            SetAParameter("Input_Buffer_Gain", prm, true);
        }

        // Conversion Mode
        private void m_cbConvMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("Conversion_Mode", m_cbConvMode.Text);
            SetAParameter("Conversion_Mode", prm, true);
        }

        // ADC Reference
        private void m_cbReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId("ADC_Reference", m_cbReference.Text);
            SetAParameter("ADC_Reference", prm, true);
        }


        // Clock source radio buttons
        // Internal clock
        private void m_rbClockInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockInternal.Checked)
            {
                SetAParameter("ADC_Clock", "Internal", true);
                m_nudConvRate.Enabled = true;
                m_nudConvRate.Visible = true;
                ConvRate_label.Visible = true;
                SamplesPerSec_label.Visible = true;
            }
        }

        // External clock
        private void m_rbClockExternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockExternal.Checked)
            {
                SetAParameter("ADC_Clock", "External", true);
                m_nudConvRate.Enabled = false;
                m_nudConvRate.Visible = false;
                ConvRate_label.Visible = false;
                SamplesPerSec_label.Visible = false;
            }
        }

        // Start of Conversion
        // Software
        private void m_rbSocSoftware_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSocSoftware.Checked)
            {
                SetAParameter("Start_of_Conversion", "Software", true);

            }
        }

        // Hardware
        private void m_rbSocHardware_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbSocHardware.Checked)
            {
                SetAParameter("Start_of_Conversion", "Hardware", true);

            }
        }



        // Conversion Rate
        private void m_nudConvRate_ValueChanged(object sender, EventArgs e)
        {

            SetAParameter("Sample_Rate", m_nudConvRate.Value.ToString(), false);


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
