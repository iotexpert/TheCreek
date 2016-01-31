//*******************************************************************************
// File Name: CyADC_DelSigControl1.cs
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
using ADC_DelSig_v2_0;


namespace ADC_DelSig_v2_0
{
    public partial class CyADC_DelSigControl1 : UserControl
    {

        #region Enumerated Type String Names
        const string RANGE_VSSA_TO_VDDA = "Vssa to Vdda";
        const string REF_INTERNAL_REF = "Internal Vref";
        const string BYPASS_BUFFER = "Bypass Buffer";
        const string BYPASS_BUFFER_ENUM = "Bypass_Buffer";
        const string ERROR_INPUT_SINGLE = "You have to choose Single Ended input range.";
        const string ERROR_INPUT_SINGLE_OTHER_CONF = "You have to choose Single Ended input range, in other configuration tab.";
        const string ERROR_INPUT_DIFFERENTIAL = "You have to choose Differential input range.";
        const string ERROR_INPUT_DIFFERENTIAL_OTHER_CONF = "You have to choose Differential input in other configuration tab..";
        const string MESSAGE_RANGE = "Range [ " ;
        const string MESSAGE_RANGE_END = " SPS ]";
        const string ERROR_REF_VOLTAGE = "You can choose either 'Internal Vref' or ";
        const string PIN_P32 = "P3.2";
        const string PIN_P03 = "P0.3";
        const string INTERNAL = "Internal";
        const string LABEL_VOLTS_VDD = "Volts (Vdd)";
        const string LABEL_VOLTS = "Volts";

        #region References to Enum Type String Names
        const string REF_EXTERNAL = "External Vref";
        const string INPUT_DIFFERENTIAL = "Diff" ; //"Differential";
        const string INPUT_SINGLE_TYPE = "Vssa";
        const string INPUT_SINGLE = "Single Ended";
        #endregion

        #endregion

        public ICyInstEdit_v1 m_Component = null;

        public CyADC_DelSigControl1(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();

            // ADC Resolution
            IEnumerable<string> ResolutionEnums = inst.GetPossibleEnumValues(ADC_DelSigParameters.RESOLUTION);
            foreach (string str in ResolutionEnums)
            {
                m_cbResolution.Items.Add(str);
                m_cbResolutionConfig2.Items.Add(str);
                m_cbResolutionConfig3.Items.Add(str);
                m_cbResolutionConfig4.Items.Add(str);
            }

            // Input Buffer Gain
            IEnumerable<string> InputBufferGainEnums = inst.GetPossibleEnumValues(ADC_DelSigParameters.INPUT_BUFFER_GAIN);
            foreach (string str in InputBufferGainEnums)
            {
                m_cbInputBufferGain.Items.Add(str);
                m_cbInputBufferGain2.Items.Add(str);
                m_cbInputBufferGain3.Items.Add(str);
                m_cbInputBufferGain4.Items.Add(str);
            }

            // Conversion Mode
            IEnumerable<string> ConversionModeEnums = inst.GetPossibleEnumValues(ADC_DelSigParameters.CONVERSION_MODE);
            foreach (string str in ConversionModeEnums)
            {
                m_cbConvMode.Items.Add(str);
                m_cbConvMode2.Items.Add(str);
                m_cbConvMode3.Items.Add(str);
                m_cbConvMode4.Items.Add(str);
            }

            // Reference Mode
            IEnumerable<string> ReferenceEnums = inst.GetPossibleEnumValues(ADC_DelSigParameters.ADC_REFERENCE);
            foreach (string str in ReferenceEnums)
            {
                m_cbReference.Items.Add(str);
                m_cbReference2.Items.Add(str);
                m_cbReference3.Items.Add(str);
                m_cbReference4.Items.Add(str);
            }

            //Buffer Mode
            IEnumerable<string> BufferModeEnums = inst.GetPossibleEnumValues(ADC_DelSigParameters.INPUT_BUFFER_MODE);
            foreach (string str in BufferModeEnums)
            {
                m_cbInputBufferMode.Items.Add(str);
                m_cbInputBufferMode2.Items.Add(str);
                m_cbInputBufferMode3.Items.Add(str);
                m_cbInputBufferMode4.Items.Add(str);
            }

            HookAllEvents();
            if (m_Component != null)
            {
                UpdateFormFromParams(inst);
                UpdateADCModes(false);
            }

        }

        /// <summary>
        /// Hook the events triggered by the controls.
        /// </summary>
        private void HookAllEvents()
        {
            this.m_cbConvMode2.SelectedIndexChanged += new System.EventHandler(this.m_cbConvMode2_SelectedIndexChanged);
            this.m_cbInputBufferMode.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferMode_SelectedIndexChanged);
            this.m_cbInputBufferGain.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferGain_SelectedIndexChanged);
            this.m_nudConfig.ValueChanged += new System.EventHandler(this.m_nudConfig_ValueChanged);
            this.m_cbConvMode.SelectedIndexChanged += new System.EventHandler(this.m_cbConvMode_SelectedIndexChanged);
            this.m_cbInputBufferMode2.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferMode2_SelectedIndexChanged);
            this.m_cbReference.SelectedIndexChanged += new System.EventHandler(this.m_cbReference_SelectedIndexChanged);

            this.m_nudRefVoltage.ValueChanged += new System.EventHandler(this.m_nudRefVoltage_ValueChanged);

            this.m_rbModeDifferential.CheckedChanged += new System.EventHandler(this.m_rbModeDifferential_CheckedChanged);
            this.m_cbInputRange.SelectedIndexChanged += new System.EventHandler(this.m_cbInputRange_SelectedIndexChanged);
            this.m_cbInputRange2.SelectedIndexChanged += new System.EventHandler(this.m_cbInputRange2_SelectedIndexChanged);
            this.m_cbInputRange3.SelectedIndexChanged += new System.EventHandler(this.m_cbInputRange3_SelectedIndexChanged);
            this.m_cbInputRange4.SelectedIndexChanged += new System.EventHandler(this.m_cbInputRange4_SelectedIndexChanged);
            
            this.m_rbModeDifferential.Validating += new CancelEventHandler(this.m_rbModeDifferential_Validating);
            this.m_cbInputRange.Validating += new CancelEventHandler(this.m_cbInputRange_Validating);
            this.m_cbInputRange2.Validating += new CancelEventHandler(this.m_cbInputRange2_Validating);
            this.m_cbInputRange3.Validating += new CancelEventHandler(this.m_cbInputRange3_Validating);
            this.m_cbInputRange4.Validating += new CancelEventHandler(this.m_cbInputRange4_Validating);

            this.m_nudConvRate.ValueChanged += new EventHandler(m_nudConvRate_ValueChanged);
            this.m_nudConvRate2.ValueChanged += new EventHandler(m_nudConvRate2_ValueChanged);
            this.m_nudConvRate3.ValueChanged += new EventHandler(m_nudConvRate3_ValueChanged);
            this.m_nudConvRate4.ValueChanged += new EventHandler(m_nudConvRate4_ValueChanged);

            this.m_cbResolution.Validating += new CancelEventHandler(m_cbResolution_Validating);
            this.m_cbResolutionConfig2.Validating += new CancelEventHandler(m_cbResolutionConfig2_Validating);
            this.m_cbResolutionConfig3.Validating += new CancelEventHandler(m_cbResolutionConfig3_Validating);
            this.m_cbResolutionConfig4.Validating += new CancelEventHandler(m_cbResolutionConfig4_Validating);

            this.m_nudRefVoltage2.ValueChanged += new System.EventHandler(this.m_nudRefVoltage2_ValueChanged);
            this.m_cbReference2.SelectedIndexChanged += new System.EventHandler(this.m_cbReference2_SelectedIndexChanged);
            this.m_cbInputBufferGain2.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferGain2_SelectedIndexChanged);
            this.m_nudRefVoltage3.ValueChanged += new System.EventHandler(this.m_nudRefVoltage3_ValueChanged);
            this.m_cbReference3.SelectedIndexChanged += new System.EventHandler(this.m_cbReference3_SelectedIndexChanged);
            this.m_cbInputBufferMode3.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferMode3_SelectedIndexChanged);
            this.m_cbInputBufferGain3.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferGain3_SelectedIndexChanged);
            this.m_cbConvMode3.SelectedIndexChanged += new System.EventHandler(this.m_cbConvMode3_SelectedIndexChanged);
            this.m_nudRefVoltage4.ValueChanged += new System.EventHandler(this.m_nudRefVoltage4_ValueChanged);
            this.m_cbReference4.SelectedIndexChanged += new System.EventHandler(this.m_cbReference4_SelectedIndexChanged);
            this.m_cbInputBufferMode4.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferMode4_SelectedIndexChanged);
            this.m_cbInputBufferGain4.SelectedIndexChanged += new System.EventHandler(this.m_cbInputBufferGain4_SelectedIndexChanged);
            this.m_cbConvMode4.SelectedIndexChanged += new System.EventHandler(this.m_cbConvMode4_SelectedIndexChanged);
            this.m_cbResolution.SelectedIndexChanged += new EventHandler(m_cbResolution_SelectedIndexChanged);
            this.m_cbResolutionConfig2.SelectedIndexChanged += new System.EventHandler(this.m_cbResolutionConfig2_SelectedIndexChanged);
            this.m_cbResolutionConfig3.SelectedIndexChanged += new System.EventHandler(this.m_cbResolutionConfig3_SelectedIndexChanged);
            this.m_cbResolutionConfig4.SelectedIndexChanged += new System.EventHandler(this.m_cbResolutionConfig4_SelectedIndexChanged);
            
            this.m_cbReference.Validating += new CancelEventHandler(m_cbReference_Validating);
            this.m_cbReference2.Validating += new CancelEventHandler(m_cbReference2_Validating);
            this.m_cbReference3.Validating += new CancelEventHandler(m_cbReference3_Validating);
            this.m_cbReference4.Validating += new CancelEventHandler(m_cbReference4_Validating);

            this.m_rbClockInternal.CheckedChanged += new System.EventHandler(this.m_rbClockInternal_CheckedChanged);
            this.m_rbClockExternal.CheckedChanged += new System.EventHandler(this.m_rbClockExternal_CheckedChanged);
            this.m_cbSocHardware.CheckedChanged += new System.EventHandler(this.m_cbSocHardware_CheckedChanged);
            this.m_cbChargePumpClock.CheckedChanged += new System.EventHandler(this.m_cbChargePumpClock_CheckedChanged);
            this.m_cbEnable_Vref_Vss.CheckedChanged += new System.EventHandler(this.m_cbEnable_Vref_Vss_CheckedChanged);
        }


        /// <summary>
        /// Unhook the events triggered by the controls.
        /// </summary>
        private void UnhookAllEvents()
        {
            this.m_cbConvMode2.SelectedIndexChanged -= new System.EventHandler(this.m_cbConvMode2_SelectedIndexChanged);
            this.m_cbInputBufferMode.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferMode_SelectedIndexChanged);
            this.m_cbInputBufferGain.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferGain_SelectedIndexChanged);
            this.m_cbInputRange.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputRange_SelectedIndexChanged);
            this.m_nudConfig.ValueChanged -= new System.EventHandler(this.m_nudConfig_ValueChanged);
            this.m_cbConvMode.SelectedIndexChanged -= new System.EventHandler(this.m_cbConvMode_SelectedIndexChanged);
            this.m_cbInputBufferMode2.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferMode2_SelectedIndexChanged);
            this.m_cbReference.SelectedIndexChanged -= new System.EventHandler(this.m_cbReference_SelectedIndexChanged);

            this.m_nudRefVoltage.ValueChanged -= new System.EventHandler(this.m_nudRefVoltage_ValueChanged);
            this.m_rbModeDifferential.CheckedChanged -= new System.EventHandler(this.m_rbModeDifferential_CheckedChanged);

            this.m_rbModeDifferential.Validating -= new CancelEventHandler(this.m_rbModeDifferential_Validating);
            this.m_cbInputRange.Validating -= new CancelEventHandler(this.m_cbInputRange_Validating);
            this.m_cbInputRange2.Validating -= new CancelEventHandler(this.m_cbInputRange2_Validating);
            this.m_cbInputRange3.Validating -= new CancelEventHandler(this.m_cbInputRange3_Validating);
            this.m_cbInputRange4.Validating -= new CancelEventHandler(this.m_cbInputRange4_Validating);

            this.m_nudConvRate.ValueChanged -= new EventHandler(m_nudConvRate_ValueChanged);
            this.m_nudConvRate2.ValueChanged -= new EventHandler(m_nudConvRate2_ValueChanged);
            this.m_nudConvRate3.ValueChanged -= new EventHandler(m_nudConvRate3_ValueChanged);
            this.m_nudConvRate4.ValueChanged -= new EventHandler(m_nudConvRate4_ValueChanged);

            this.m_cbResolution.Validating -= new CancelEventHandler(m_cbResolution_Validating);
            this.m_cbResolutionConfig2.Validating -= new CancelEventHandler(m_cbResolutionConfig2_Validating);
            this.m_cbResolutionConfig3.Validating -= new CancelEventHandler(m_cbResolutionConfig3_Validating);
            this.m_cbResolutionConfig4.Validating -= new CancelEventHandler(m_cbResolutionConfig4_Validating);

            this.m_cbReference.Validating -= new CancelEventHandler(m_cbReference_Validating);
            this.m_cbReference2.Validating -= new CancelEventHandler(m_cbReference2_Validating);
            this.m_cbReference3.Validating -= new CancelEventHandler(m_cbReference3_Validating);
            this.m_cbReference4.Validating -= new CancelEventHandler(m_cbReference4_Validating);

            this.m_cbResolution.SelectedIndexChanged -= new EventHandler(m_cbResolution_SelectedIndexChanged);

            this.m_nudRefVoltage2.ValueChanged -= new System.EventHandler(this.m_nudRefVoltage2_ValueChanged);
            this.m_cbReference2.SelectedIndexChanged -= new System.EventHandler(this.m_cbReference2_SelectedIndexChanged);
            this.m_cbInputBufferGain2.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferGain2_SelectedIndexChanged);
            this.m_cbInputRange2.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputRange2_SelectedIndexChanged);
            this.m_cbResolutionConfig2.SelectedIndexChanged -= new System.EventHandler(this.m_cbResolutionConfig2_SelectedIndexChanged);
            this.m_nudRefVoltage3.ValueChanged -= new System.EventHandler(this.m_nudRefVoltage3_ValueChanged);
            this.m_cbReference3.SelectedIndexChanged -= new System.EventHandler(this.m_cbReference3_SelectedIndexChanged);
            this.m_cbInputBufferMode3.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferMode3_SelectedIndexChanged);
            this.m_cbInputBufferGain3.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferGain3_SelectedIndexChanged);
            this.m_cbInputRange3.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputRange3_SelectedIndexChanged);
            this.m_cbResolutionConfig3.SelectedIndexChanged -= new System.EventHandler(this.m_cbResolutionConfig3_SelectedIndexChanged);
            this.m_cbConvMode3.SelectedIndexChanged -= new System.EventHandler(this.m_cbConvMode3_SelectedIndexChanged);
            this.m_nudRefVoltage4.ValueChanged -= new System.EventHandler(this.m_nudRefVoltage4_ValueChanged);
            this.m_cbReference4.SelectedIndexChanged -= new System.EventHandler(this.m_cbReference4_SelectedIndexChanged);
            this.m_cbInputBufferMode4.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferMode4_SelectedIndexChanged);
            this.m_cbInputBufferGain4.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputBufferGain4_SelectedIndexChanged);
            this.m_cbInputRange4.SelectedIndexChanged -= new System.EventHandler(this.m_cbInputRange4_SelectedIndexChanged);
            this.m_cbResolutionConfig4.SelectedIndexChanged -= new System.EventHandler(this.m_cbResolutionConfig4_SelectedIndexChanged);
            this.m_cbConvMode4.SelectedIndexChanged -= new System.EventHandler(this.m_cbConvMode4_SelectedIndexChanged);

            
            this.m_rbClockInternal.CheckedChanged -= new System.EventHandler(this.m_rbClockInternal_CheckedChanged);
            this.m_rbClockExternal.CheckedChanged -= new System.EventHandler(this.m_rbClockExternal_CheckedChanged);
            this.m_cbSocHardware.CheckedChanged -= new System.EventHandler(this.m_cbSocHardware_CheckedChanged);
            this.m_cbChargePumpClock.CheckedChanged -= new System.EventHandler(this.m_cbChargePumpClock_CheckedChanged);
            this.m_cbEnable_Vref_Vss.CheckedChanged -= new System.EventHandler(this.m_cbEnable_Vref_Vss_CheckedChanged);

        }

        void m_cbReference4_Validating(object sender, CancelEventArgs e)
        {
            CheckOtherRefVotageConfig(m_cbReference4);
        }

        void m_cbReference3_Validating(object sender, CancelEventArgs e)
        {
            CheckOtherRefVotageConfig(m_cbReference3);
            CheckOtherRefVotageConfig(m_cbReference4);
        }

        void m_cbReference2_Validating(object sender, CancelEventArgs e)
        {
            CheckOtherRefVotageConfig(m_cbReference2);
            CheckOtherRefVotageConfig(m_cbReference3);
            CheckOtherRefVotageConfig(m_cbReference4);
        }

        void m_cbReference_Validating(object sender, CancelEventArgs e)
        {
            CheckOtherRefVotageConfig(m_cbReference2);
            CheckOtherRefVotageConfig(m_cbReference3);
            CheckOtherRefVotageConfig(m_cbReference4);
            
        }


        void m_cbResolutionConfig4_Validating(object sender, CancelEventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolutionConfig4.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION_CONFIG4, prm, true);
            CheckFreqConfig4(true);
        }

        void m_cbResolutionConfig3_Validating(object sender, CancelEventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolutionConfig3.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION_CONFIG3, prm, true);
            CheckFreqConfig3(true);
        }

        void m_cbResolutionConfig2_Validating(object sender, CancelEventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolutionConfig2.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION_CONFIG2, prm, true);
            CheckFreqConfig2(true);
        }

        //Resolution
        void m_cbResolution_Validating(object sender, CancelEventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolution.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION, prm, true);
            CheckFreqConfig1(true);
        }

        private void m_cbResolutionConfig2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update the labels.
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolutionConfig2.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION_CONFIG2, prm, true);
            CheckFreqConfig2(true);
            DisplayInterruptMessage();
        }

        private void m_cbResolutionConfig3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolutionConfig3.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION_CONFIG3, prm, true);
            CheckFreqConfig3(true);
            DisplayInterruptMessage();
        }

        void m_cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolution.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION, prm, true);
            CheckFreqConfig1(true);
            DisplayInterruptMessage();
        }


        /// <summary>
        /// Need to add detection of when the parent form is closing allowing me to cancel if there are errors in the parameters
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            UnhookAllEvents();
            ADC_DelSigParameters prms = new ADC_DelSigParameters(inst);

            #region Config1
            //Set the ADC resolution
            string paramResolution = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.RESOLUTION, 
                prms.ADC_Resolution.Expr);
            if (m_cbResolution.Items.Contains(paramResolution))
            {
                m_cbResolution.SelectedItem = paramResolution;
                m_cbResolution.Text = prms.ADC_Resolution.Expr;
            }

            /*set the ADC mode*/
            string paramAdcMode = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_INPUT_MODE, 
                prms.ADC_Input_Mode.Expr);
            if (!paramAdcMode.Contains(INPUT_DIFFERENTIAL))
            {
                m_rbModeSingle.Checked = true;
            }

            //Set the ADC Input Range
            string paramInputRange = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE, 
                prms.ADC_Input_Range.Expr);
            if (m_cbInputRange.Items.Contains(paramInputRange))
            {
                m_cbInputRange.SelectedItem = paramInputRange;
                m_cbInputRange.Text = prms.ADC_Input_Range.Expr;
            }

            //Set the Input Buffer Gain
            string paramInputBufferGain = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_GAIN, 
                prms.Input_Buffer_Gain.Expr);
            if (m_cbInputBufferGain.Items.Contains(paramInputBufferGain))
            {
                m_cbInputBufferGain.SelectedItem = paramInputBufferGain;
                m_cbInputBufferGain.Text = prms.Input_Buffer_Gain.Expr;
            }

            //Set the Conversion Mode
            string paramConvMode = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.CONVERSION_MODE, 
                prms.Conversion_Mode.Expr);
            if (m_cbConvMode.Items.Contains(paramConvMode))
            {
                m_cbConvMode.SelectedItem = paramConvMode;
                m_cbConvMode.Text = prms.Conversion_Mode.Expr;
            }

            //Set ADC Reference
            string paramReference = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_REFERENCE, 
                prms.ADC_Reference.Expr);
            if (m_cbReference.Items.Contains(paramReference))
            {
                m_cbReference.SelectedItem = paramReference;
                m_cbReference.Text = prms.ADC_Reference.Expr;
            }

            //Set ADC Input Buffer Mode
            string paramBufferMode = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_MODE, 
                prms.Input_Buffer_Mode.Expr);
            if (m_cbInputBufferMode.Items.Contains(paramBufferMode))
            {
                m_cbInputBufferMode.SelectedItem = paramBufferMode;
                m_cbInputBufferMode.Text = prms.Input_Buffer_Mode.Expr;
            }

            //Reference voltage.
            float tempFloat;
            float.TryParse(prms.Ref_Voltage.Value, out tempFloat);
            m_nudRefVoltage.Value = (decimal)tempFloat;

            // Conversion/Sample Rate
            int tempSampleRate;
            int.TryParse(prms.Sample_Rate.Value, out tempSampleRate);
            m_nudConvRate.Value = (decimal)tempSampleRate;

            #endregion

            #region Config2
            //Set the ADC resolution
            string paramResolution2 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.RESOLUTION_CONFIG2, 
                prms.ADC_Resolution_Config2.Expr);
            if (m_cbResolutionConfig2.Items.Contains(paramResolution2))
            {
                m_cbResolutionConfig2.SelectedItem = paramResolution2;
                m_cbResolutionConfig2.Text = prms.ADC_Resolution_Config2.Expr;
            }

            //Set the ADC Input Range
            string paramInputRange2 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE_CONFIG2, 
                prms.ADC_Input_Range_Config2.Expr);
            if (m_cbInputRange2.Items.Contains(paramInputRange2))
            {
                m_cbInputRange2.SelectedItem = paramInputRange2;
                m_cbInputRange2.Text = prms.ADC_Input_Range_Config2.Expr;
            }

            //Set the Input Buffer Gain
            string paramInputBufferGain2 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_GAIN, 
                prms.Input_Buffer_Gain_Config2.Expr);
            if (m_cbInputBufferGain2.Items.Contains(paramInputBufferGain2))
            {
                m_cbInputBufferGain2.SelectedItem = paramInputBufferGain2;
                m_cbInputBufferGain2.Text = prms.Input_Buffer_Gain_Config2.Expr;
            }

            //Set the Conversion Mode
            string paramConvMode2 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.CONVERSION_MODE, 
                prms.Conversion_Mode_Config2.Expr);
            if (m_cbConvMode2.Items.Contains(paramConvMode2))
            {
                m_cbConvMode2.SelectedItem = paramConvMode2;
                m_cbConvMode2.Text = prms.Conversion_Mode_Config2.Expr;
            }

            //Set ADC Reference
            string paramReference2 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_REFERENCE, 
                prms.ADC_Reference_Config2.Expr);
            if (m_cbReference2.Items.Contains(paramReference2))
            {
                m_cbReference2.SelectedItem = paramReference2;
                m_cbReference2.Text = prms.ADC_Reference_Config2.Expr;
            }

            //Set ADC Input Buffer Mode
            string paramBufferMode2 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_MODE,
                prms.Input_Buffer_Mode_Config2.Expr);
            if (m_cbInputBufferMode2.Items.Contains(paramBufferMode2))
            {
                m_cbInputBufferMode2.SelectedItem = paramBufferMode2;
                m_cbInputBufferMode2.Text = prms.Input_Buffer_Mode_Config2.Expr;
            }

            //Reference voltage.
            float.TryParse(prms.Ref_Voltage_Config2.Value, out tempFloat);
            m_nudRefVoltage2.Value = (decimal)tempFloat;

            // Conversion/Sample Rate
            int.TryParse(prms.Sample_Rate_Config2.Value, out tempSampleRate);
            m_nudConvRate2.Value = (decimal)tempSampleRate;
            #endregion

            #region Config3
            //Set the ADC resolution
            string paramResolution3 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.RESOLUTION_CONFIG3,
                prms.ADC_Resolution_Config3.Expr);
            if (m_cbResolutionConfig3.Items.Contains(paramResolution3))
            {
                m_cbResolutionConfig3.SelectedItem = paramResolution3;
                m_cbResolutionConfig3.Text = prms.ADC_Resolution_Config3.Expr;
            }

            //Set the ADC Input Range
            string paramInputRange3 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE_CONFIG3,
                prms.ADC_Input_Range_Config3.Expr);
            if (m_cbInputRange3.Items.Contains(paramInputRange3))
            {
                m_cbInputRange3.SelectedItem = paramInputRange3;
                m_cbInputRange3.Text = prms.ADC_Input_Range_Config3.Expr;
            }

            //Set the Input Buffer Gain
            string paramInputBufferGain3 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_GAIN,
                prms.Input_Buffer_Gain_Config3.Expr);
            if (m_cbInputBufferGain3.Items.Contains(paramInputBufferGain3))
            {
                m_cbInputBufferGain3.SelectedItem = paramInputBufferGain3;
                m_cbInputBufferGain3.Text = prms.Input_Buffer_Gain_Config3.Expr;
            }

            //Set the Conversion Mode
            string paramConvMode3 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.CONVERSION_MODE,
                prms.Conversion_Mode_Config3.Expr);
            if (m_cbConvMode3.Items.Contains(paramConvMode3))
            {
                m_cbConvMode3.SelectedItem = paramConvMode3;
                m_cbConvMode3.Text = prms.Conversion_Mode_Config3.Expr;
            }

            //Set ADC Reference
            string paramReference3 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_REFERENCE,
                prms.ADC_Reference_Config3.Expr);
            if (m_cbReference3.Items.Contains(paramReference3))
            {
                m_cbReference3.SelectedItem = paramReference3;
                m_cbReference3.Text = prms.ADC_Reference_Config3.Expr;
            }

            //Set ADC Input Buffer Mode
            string paramBufferMode3 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_MODE,
                prms.Input_Buffer_Mode_Config3.Expr);
            if (m_cbInputBufferMode3.Items.Contains(paramBufferMode3))
            {
                m_cbInputBufferMode3.SelectedItem = paramBufferMode3;
                m_cbInputBufferMode3.Text = prms.Input_Buffer_Mode_Config3.Expr;
            }

            //Reference voltage.
            float.TryParse(prms.Ref_Voltage_Config3.Value, out tempFloat);
            m_nudRefVoltage3.Value = (decimal)tempFloat;

            // Conversion/Sample Rate
            int.TryParse(prms.Sample_Rate_Config3.Value, out tempSampleRate);
            m_nudConvRate3.Value = (decimal)tempSampleRate;
            #endregion

            #region Config4
            //Set the ADC resolution
            string paramResolution4 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.RESOLUTION_CONFIG4,
                prms.ADC_Resolution_Config4.Expr);
            if (m_cbResolutionConfig4.Items.Contains(paramResolution4))
            {
                m_cbResolutionConfig4.SelectedItem = paramResolution4;
                m_cbResolutionConfig4.Text = prms.ADC_Resolution_Config4.Expr;
            }

            //Set the ADC Input Range
            string paramInputRange4 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE_CONFIG4,
                prms.ADC_Input_Range_Config4.Expr);
            if (m_cbInputRange4.Items.Contains(paramInputRange4))
            {
                m_cbInputRange4.SelectedItem = paramInputRange4;
                m_cbInputRange4.Text = prms.ADC_Input_Range_Config4.Expr;
            }

            //Set the Input Buffer Gain
            string paramInputBufferGain4 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_GAIN,
                prms.Input_Buffer_Gain_Config4.Expr);
            if (m_cbInputBufferGain4.Items.Contains(paramInputBufferGain4))
            {
                m_cbInputBufferGain4.SelectedItem = paramInputBufferGain4;
                m_cbInputBufferGain4.Text = prms.Input_Buffer_Gain_Config4.Expr;
            }

            //Set the Conversion Mode
            string paramConvMode4 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.CONVERSION_MODE,
                prms.Conversion_Mode_Config4.Expr);
            if (m_cbConvMode4.Items.Contains(paramConvMode4))
            {
                m_cbConvMode4.SelectedItem = paramConvMode4;
                m_cbConvMode4.Text = prms.Conversion_Mode_Config4.Expr;
            }

            //Set ADC Reference
            string paramReference4 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_REFERENCE,
                prms.ADC_Reference_Config4.Expr);
            if (m_cbReference4.Items.Contains(paramReference4))
            {
                m_cbReference4.SelectedItem = paramReference4;
                m_cbReference4.Text = prms.ADC_Reference_Config4.Expr;
            }

            //Set ADC Input Buffer Mode
            string paramBufferMode4 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_BUFFER_MODE,
                prms.Input_Buffer_Mode_Config4.Expr);
            if (m_cbInputBufferMode4.Items.Contains(paramBufferMode4))
            {
                m_cbInputBufferMode4.SelectedItem = paramBufferMode4;
                m_cbInputBufferMode4.Text = prms.Input_Buffer_Mode_Config4.Expr;
            }
            //Reference voltage.
            float.TryParse(prms.Ref_Voltage_Config4.Value, out tempFloat);
            m_nudRefVoltage4.Value = (decimal)tempFloat;

            // Conversion/Sample Rate
            int.TryParse(prms.Sample_Rate_Config4.Value, out tempSampleRate);
            m_nudConvRate4.Value = (decimal)tempSampleRate;
            #endregion

            #region Common configuration
            // Clock Source check box
            if (m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_CLOCK, prms.ADC_Clock.Expr) == 
                ADC_DelSigParameters.S_INTERNAL)
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
            if (m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.START_OF_CONVERSION,
                prms.Start_of_Conversion.Expr) == ADC_DelSigParameters.S_SOFTWARE)
            {
                m_cbSocHardware.Checked = false;
            }
            else
            {
                m_cbSocHardware.Checked = true;
            }

            // Configs
            int temp;
            int.TryParse(prms.Configs.Value, out temp);
            m_nudConfig.Value = (decimal)temp;
            UpdateConfigTabs(temp);

            // Low Power Charge Pump
            m_cbChargePumpClock.Checked = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_CHARGE_PUMP_CLOCK,
                prms.ADC_Charge_Pump_Clock.Expr).Equals(Boolean.TrueString);

            // Enable Vref Vss
            m_cbEnable_Vref_Vss.Checked = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.ADC_nVref,
                prms.Enable_Vref_Vss.Expr).Equals(Boolean.TrueString);
            #endregion

            UpdateAllBufferGain();
            CheckAllFreq(false);
            UpdateRefVoltages();
            m_cbReference_Validating(this, new CancelEventArgs());
            DisplayInterruptMessage();
            UpdateRefVoltages();
            HookAllEvents();
        }

        private void UpdateRefVoltages()
        {
            if (m_cbInputRange.Text == RANGE_VSSA_TO_VDDA)   
            {
                m_cbReference.Text = REF_INTERNAL_REF;
                m_cbReference.Enabled = false;
            }
            else
            {
                m_cbReference.Enabled = true;
            }
            if (m_cbInputRange2.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference2.Text = REF_INTERNAL_REF;
                m_cbReference2.Enabled = false;
            }
            else
            {
                m_cbReference2.Enabled = true;
            }
            if (m_cbInputRange3.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference3.Text = REF_INTERNAL_REF;
                m_cbReference3.Enabled = false;
            }
            else
            {
                m_cbReference3.Enabled = true;
            }
            if (m_cbInputRange4.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference4.Text = REF_INTERNAL_REF;
                m_cbReference4.Enabled = false;
            }
            else
            {
                m_cbReference4.Enabled = true;
            }
        }

        private void UpdateConfigTabs(int numberOfTabs)
        {
            /* There will be one extra tab for common settings.*/
            if (numberOfTabs > m_tabConfig.Controls.Count - 1)
            {
                /*Remove the common tab and add it later to keep the order*/
                m_tabConfig.Controls.Remove(m_tabPage5);
                switch (numberOfTabs)
                {
                    case 2:
                        m_tabConfig.Controls.Add(m_tabPage2);
                        break;
                    case 3:
                        m_tabConfig.Controls.Add(m_tabPage3);
                        break;
                    case 4:
                        m_tabConfig.Controls.Add(m_tabPage4);
                        break;
                }
                m_tabConfig.Controls.Add(m_tabPage5);
            }
            else if (numberOfTabs != ADC_DelSigParameters.MAX_CONFIGS)
            {
                /* Remove the tabs */
                if (numberOfTabs <= 3)
                {
                    m_tabConfig.Controls.Remove(m_tabPage4);
                }
                if (numberOfTabs <= 2)
                {
                    m_tabConfig.Controls.Remove(m_tabPage3);
                }
                if (numberOfTabs == 1)
                {
                    m_tabConfig.Controls.Remove(m_tabPage2);
                }
            }            
        }

        void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((Form)sender).DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (CheckAllFreq(true) == false || UpdateADCModes(false) == false)
            {
                /*e.Cancel = true;*/
            }
        }


        void m_nudConvRate4_ValueChanged(object sender, EventArgs e)
        {
            /* Check ADC clock frequency range.*/
            SetAParameter(ADC_DelSigParameters.SAMPLE_RATE_CONFIG4, m_nudConvRate4.Value.ToString(), false);
            CheckFreqConfig4(true);
        }

        void m_nudConvRate3_ValueChanged(object sender, EventArgs e)
        {
            /* Check ADC clock frequency range.*/
            SetAParameter(ADC_DelSigParameters.SAMPLE_RATE_CONFIG3, m_nudConvRate3.Value.ToString(), false);
            CheckFreqConfig3(true);
        }

        void m_nudConvRate2_ValueChanged(object sender, EventArgs e)
        {
            /* Check ADC clock frequency range.*/
            SetAParameter(ADC_DelSigParameters.SAMPLE_RATE_CONFIG2, m_nudConvRate2.Value.ToString(), false);
            CheckFreqConfig2(true);
        }

        void m_nudConvRate_ValueChanged(object sender, EventArgs e)
        {
            /* Check ADC clock frequency range.*/
            SetAParameter(ADC_DelSigParameters.SAMPLE_RATE, m_nudConvRate.Value.ToString(), false);
            CheckFreqConfig1(true);
        }

        // ADC Input Range
        private void m_cbInputRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE, m_cbInputRange.Text);

            // If "Vssa to Vdda (Single Ended)" is selected then choose refernce to be "Internal Ref"
            //m_errorProvider.SetError(m_cbInputRange, m_cbInputRange.Text + " : " + RANGE_VSSA_TO_VDDA);
            if (m_cbInputRange.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference.Text = REF_INTERNAL_REF;
                m_cbReference.Enabled = false; 
            }
            else
            {
                m_cbReference.Enabled = true;
            }

            if (CheckInputRange(m_cbInputRange, true))
            {
                SetAParameter(ADC_DelSigParameters.INPUT_RANGE, prm, true);
            }
            else
            {
                m_errorProvider.SetError(m_cbInputRange, ERROR_INPUT_SINGLE);
            }
            UpdateRefVoltageEnable(m_cbReference, m_cbInputRange, Volts_label, m_nudRefVoltage);

        }

        // Input Buffer Gain
        private void m_cbInputBufferGain_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_GAIN, m_cbInputBufferGain.Text);
              SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_GAIN, prm, true);
              CheckFreqConfig1(true);
        }

        // Conversion Mode
        private void m_cbConvMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.CONVERSION_MODE, m_cbConvMode.Text);
            if (CheckFreqConfig1(true))
            {
                SetAParameter(ADC_DelSigParameters.CONVERSION_MODE, prm, true);
            }
            DisplayInterruptMessage();
        }

        private void DisplayInterruptMessage()
        {
            label_Warning.Visible = false;
            if ((m_cbConvMode.SelectedIndex == 0 && m_cbResolution.SelectedIndex > 8 && m_nudConfig.Value >= 1) 
                || (m_cbConvMode2.SelectedIndex == 0 && m_cbResolutionConfig2.SelectedIndex > 8 && m_nudConfig.Value >= 2) 
                || (m_cbConvMode3.SelectedIndex == 0 && m_cbResolutionConfig3.SelectedIndex > 8 && m_nudConfig.Value >= 3)
                || (m_cbConvMode4.SelectedIndex == 0 && m_cbResolutionConfig4.SelectedIndex > 8 && m_nudConfig.Value == 4))
            {
                label_Warning.Visible = true;
            }
        }
        
        // ADC Reference
        private void m_cbReference_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.ADC_REFERENCE, m_cbReference.Text);
            SetAParameter(ADC_DelSigParameters.ADC_REFERENCE, prm, true);
            CheckOtherRefVotageConfig(m_cbReference2);
            UpdateRefVoltageEnable(m_cbReference, m_cbInputRange, Volts_label, m_nudRefVoltage);
        }

        private bool CheckOtherRefVotageConfig(ComboBox parent)
        {
            bool retVal = false;

            /*Config1 will be the master tab, if it has set to other than 'Internal Vref' then it would be taken as the
              'only' allowed wiring.*/
            string masterConfig = REF_INTERNAL_REF;
            if (m_cbReference.Text.Contains(PIN_P03) || m_cbReference.Text.Contains(PIN_P32))
            {
                masterConfig = m_cbReference.Text;
            }
            else if (m_cbReference2.Text.Contains(PIN_P03) || m_cbReference2.Text.Contains(PIN_P32))
            {
                masterConfig = m_cbReference2.Text;
            }
            else if (m_cbReference3.Text.Contains(PIN_P03) || m_cbReference3.Text.Contains(PIN_P32))
            {
                masterConfig = m_cbReference3.Text;
            }
            else if (m_cbReference4.Text.Contains(PIN_P03) || m_cbReference4.Text.Contains(PIN_P32))
            {
                masterConfig = m_cbReference4.Text;
            }

            /*The wiring can be same as master config or it should be internal reference .*/
            if (!(parent.Text.Equals(masterConfig) || parent.Text.Equals(REF_INTERNAL_REF)))
            {
                /*Set error .*/
                m_errorProvider.SetError(parent, ERROR_REF_VOLTAGE + masterConfig);
                retVal = true;
            }
            else
            {
                m_errorProvider.SetError(parent, string.Empty);
            }
            return retVal;
        }

        // Clock source radio buttons
        // Internal clock
        private void m_rbClockInternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockInternal.Checked)
            {
                SetAParameter(ADC_DelSigParameters.ADC_CLOCK, ADC_DelSigParameters.S_INTERNAL, true);
            }
        }

        // External clock
        private void m_rbClockExternal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbClockExternal.Checked)
            {
                SetAParameter(ADC_DelSigParameters.ADC_CLOCK, ADC_DelSigParameters.S_EXTERNAL, true);
            }
        }

        // Start of Conversion
        // Hardware/Software
        private void m_cbSocHardware_CheckedChanged(object sender, EventArgs e)
        {
            if (m_cbSocHardware.Checked)
            {
                SetAParameter(ADC_DelSigParameters.START_OF_CONVERSION, ADC_DelSigParameters.S_HARDWARE, true);
            }
            else
            {
                SetAParameter(ADC_DelSigParameters.START_OF_CONVERSION, ADC_DelSigParameters.S_SOFTWARE, true);
            }
        }

        // Power Charge Pump 
        // Hardware/Software
        private void m_cbChargePumpClock_CheckedChanged(object sender, EventArgs e)
        {
            //m_errorProvider.SetError(m_cbChargePumpClock, m_errorProvider.GetError(m_cbChargePumpClock).ToString() + m_cbChargePumpClock.Checked.ToString());
            SetAParameter(ADC_DelSigParameters.ADC_CHARGE_PUMP_CLOCK, m_cbChargePumpClock.Checked.ToString(), true);
        }

      
        // Reference Voltage
        private void m_nudRefVoltage_ValueChanged(object sender, EventArgs e)
        {
            SetAParameter(ADC_DelSigParameters.REF_VOLTAGE, m_nudRefVoltage.Value.ToString(), false);
        }

        private void SetAParameter(string parameter, string value, bool checkFocus)
        {
            if (this.ContainsFocus || !checkFocus)
            {
                //Verify that the parameter was set correctly.
                m_Component.SetParamExpr(parameter, value);
                m_Component.CommitParamExprs();

                if (m_Component.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_Component.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    string errorMessage = string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", 
                        parameter, value, errors);
                    m_errorProvider.SetError(m_nudConfig, errorMessage);
                }
            }
        }

        // Get the ADC clock frequency for the current settings.
        private int GetFreq(uint resolution, uint conversionMode, uint convRate)
        {
            int theFreq;
            theFreq = (int)CyCustomizer.SetClock(resolution, convRate, conversionMode);
            return theFreq;
        }

        // Get the Conversion rate for the current settings.
        private uint GetConvRate(float frequency, bool maximum, int resolution, int conversionMode)
        {
            uint theConvRate;
            theConvRate = (uint)CyCustomizer.GetSampleRate((uint)resolution, frequency, (uint)conversionMode, maximum);
            return theConvRate;
        }

        private bool CheckAllFreq(bool saveValue)
        {
            bool retValue;
            retValue = CheckFreqConfig1(saveValue);
            retValue &= CheckFreqConfig2(saveValue);
            retValue &= CheckFreqConfig3(saveValue);
            retValue &= CheckFreqConfig4(saveValue);
            return retValue;
        }

        // Check the current ADC settings for clock frequencies that are beyond the clock specifications.
        private bool CheckFreqConfig1(bool saveValue)
        {
            return CheckFreq(ADC_DelSigParameters.RESOLUTION, ADC_DelSigParameters.INPUT_BUFFER_GAIN,
                ADC_DelSigParameters.SAMPLE_RATE, ADC_DelSigParameters.CONVERSION_MODE,
                m_cbInputBufferMode, m_nudConvRate, m_cbConvMode, m_cbResolution, m_tbClockFreq,
                m_lbSPSRange, saveValue);
        }

        // Check the current ADC settings for clock frequencies that are beyond the clock specifications.
        private bool CheckFreqConfig2(bool saveValue)
        {
            return CheckFreq(ADC_DelSigParameters.RESOLUTION_CONFIG2, ADC_DelSigParameters.INPUT_BUFFER_GAIN_CONFIG2,
                ADC_DelSigParameters.SAMPLE_RATE_CONFIG2, ADC_DelSigParameters.CONVERSION_MODE_CONFIG2,
                m_cbInputBufferMode2, m_nudConvRate2, m_cbConvMode2, m_cbResolutionConfig2, m_tbClockFreq2,
                m_lbSPSRange2, saveValue);
        }

        // Check the current ADC settings for clock frequencies that are beyond the clock specifications.
        private bool CheckFreqConfig3(bool saveValue)
        {
            return CheckFreq(ADC_DelSigParameters.RESOLUTION_CONFIG3, ADC_DelSigParameters.INPUT_BUFFER_GAIN_CONFIG3,
                ADC_DelSigParameters.SAMPLE_RATE_CONFIG3, ADC_DelSigParameters.CONVERSION_MODE_CONFIG3,
                m_cbInputBufferMode3, m_nudConvRate3, m_cbConvMode3, m_cbResolutionConfig3, m_tbClockFreq3,
                m_lbSPSRange3, saveValue);
        }

        // Check the current ADC settings for clock frequencies that are beyond the clock specifications.
        private bool CheckFreqConfig4(bool saveValue)
        {
            return CheckFreq(ADC_DelSigParameters.RESOLUTION_CONFIG4, ADC_DelSigParameters.INPUT_BUFFER_GAIN_CONFIG4,
                ADC_DelSigParameters.SAMPLE_RATE_CONFIG4, ADC_DelSigParameters.CONVERSION_MODE_CONFIG4,
                m_cbInputBufferMode4, m_nudConvRate4, m_cbConvMode4, m_cbResolutionConfig4, m_tbClockFreq4,
                m_lbSPSRange4, saveValue);
        }

        // Check the current ADC settings for clock frequencies that are beyond the clock specifications.
        private bool CheckFreq(string sResolution, string sGain, string sSampleRate, string sConversionMode, 
            ComboBox cbInputBufferMode, CyNumericUpDown nudConvRate, ComboBox cbConvMode, ComboBox cbResolution,
            TextBox tbClockFreq, Label lbSPSRange, bool saveValue)
        {
            float theFreq;
            float theFreqKHz;
            int resolution;
            int gain, conversionMode;
            uint minRate, maxRate;
            float TheMaxFreq;
            float TheMinFreq;
            float TheMaxFreqKHz;
            float TheMinFreqKHz;
            string errorMessage;
            bool retVal;

            retVal = true;
            resolution = int.Parse(cbResolution.Text);// int.Parse(m_Component.GetCommittedParam(sResolution).Value);
            gain = int.Parse(m_Component.GetCommittedParam(sGain).Value);
            conversionMode = (int)cbConvMode.SelectedIndex; //m_Component.GetCommittedParam(sConversionMode).Value);
            if (resolution > 11)
            {
                if (resolution > 15)
                {
                    //If buffer is enabled, for 2x,4x,8x gain, the frequency is halved and 
                    //halved and halved respectively
                    if (cbInputBufferMode.Items[cbInputBufferMode.SelectedIndex].ToString() == BYPASS_BUFFER) //Unbuffered Input
                        TheMaxFreq = (float)rc.MAX_FREQ_16_20_BITS;
                    else
                        TheMaxFreq = (float)rc.MAX_FREQ_16_20_BITS / gain;

                    TheMinFreq = (float)rc.MIN_FREQ_15_20_BITS;
                }
                else
                {
                    if (cbInputBufferMode.Items[cbInputBufferMode.SelectedIndex].ToString() == BYPASS_BUFFER) //Unbuffered Input
                        TheMaxFreq = (float)rc.MAX_FREQ_12_15_BITS_UNBUFFERED;
                    else
                        TheMaxFreq = (float)rc.MAX_FREQ_12_15_BITS_BUFFERED / gain;

                    //Minimum frequency is equal to that for 8-14 bit resolution
                    TheMinFreq = (float)rc.MIN_FREQ_8_14_BITS;
                }
            }
            else
            {
                //If buffer is enabled, for 2x,4x,8x gain, the frequency is halved and 
                //halved and halved respectively
                if (cbInputBufferMode.Items[cbInputBufferMode.SelectedIndex].ToString() == BYPASS_BUFFER) //Unbuffered Input
                    TheMaxFreq = (float)rc.MAX_FREQ_8_11_BITS;
                else
                    TheMaxFreq = (float)rc.MAX_FREQ_8_11_BITS / gain;

                TheMinFreq = (float)rc.MIN_FREQ_8_14_BITS;

            }

            theFreq = (float)GetFreq((uint)resolution, (uint)conversionMode, (uint)nudConvRate.Value);
            theFreqKHz = theFreq / (float)1000.0;
            TheMaxFreqKHz = TheMaxFreq / (float)1000.0;
            TheMinFreqKHz = (float)(TheMinFreq / 1000.0);

            maxRate = GetConvRate(TheMaxFreq, true, resolution, conversionMode);
            minRate = GetConvRate(TheMinFreq, false, resolution, conversionMode);
            
            // Compare to valid value
            if (theFreq > TheMaxFreq)
            {
                errorMessage = "The sample rate of " + (uint)nudConvRate.Value
                    + " SPS has exceeded the maximum limit of "
                    + maxRate.ToString()
                    + " SPS for the conversion mode, resolution and frequency.";
                m_errorProvider.SetError(nudConvRate, errorMessage);
                m_errorProvider.SetError(cbResolution, errorMessage);
                m_errorProvider.SetError(cbConvMode, errorMessage);
                m_errorProvider.SetError(tbClockFreq, errorMessage);
                retVal = false;
            }
            else if (theFreq < TheMinFreq)
            {
                errorMessage = "The sample rate of " + (uint)nudConvRate.Value + " SPS is below the minimum limit of "
                    + minRate.ToString() 
                    + " SPS for the conversion mode, resolution and frequency.";
                m_errorProvider.SetError(nudConvRate, errorMessage);
                m_errorProvider.SetError(cbResolution, errorMessage);
                m_errorProvider.SetError(cbConvMode, errorMessage);
                m_errorProvider.SetError(tbClockFreq, errorMessage);
                retVal = false;
            }
            else
            {
                m_errorProvider.SetError(cbResolution, String.Empty);
                m_errorProvider.SetError(cbConvMode, String.Empty);
                m_errorProvider.SetError(nudConvRate, String.Empty);
                m_errorProvider.SetError(tbClockFreq, String.Empty);
                retVal = true;
            }

            if (saveValue)
            {
                SetAParameter(sSampleRate, nudConvRate.Value.ToString(), false);
            }
            tbClockFreq.Text = theFreqKHz.ToString("0.000");
            lbSPSRange.Text = MESSAGE_RANGE + minRate.ToString() + " - " + maxRate.ToString() + MESSAGE_RANGE_END;
            return retVal;
        }

        // Check what combination of Input Range and Reference is selected
        private void UpdateRefVoltageEnable_obsol()
        {
            m_nudRefVoltage.Enabled = false;
            if (m_cbReference.Text.Contains(REF_EXTERNAL) || (m_cbInputRange.Text.Equals(RANGE_VSSA_TO_VDDA)))
            {
                m_nudRefVoltage.Enabled = true;
                if (m_cbInputRange.Text == RANGE_VSSA_TO_VDDA)
                {
                    Volts_label.Text = LABEL_VOLTS_VDD;
                    m_nudRefVoltage.Maximum = 5.5M;
                }
                else
                {
                    Volts_label.Text = LABEL_VOLTS;
                    m_nudRefVoltage.Maximum = 1.5M;
                }
        
            }
            else
            {
                m_nudRefVoltage.Maximum = 1.5M;
                Volts_label.Text = LABEL_VOLTS;
                m_nudRefVoltage.Value = 1.024M;
                m_nudRefVoltage.Enabled = false;
            }
        }

        // Check what combination of Input Range and Reference is selected
        private void UpdateRefVoltageEnable(Control cbReference, Control cbInputRange, Control label, 
            CyNumericUpDown nudRefVolt)
        {
            nudRefVolt.Enabled = false;
            if (cbReference.Text.Contains(REF_EXTERNAL) || (cbInputRange.Text == RANGE_VSSA_TO_VDDA))
            {
                nudRefVolt.Enabled = true;
                
                if (cbInputRange.Text == RANGE_VSSA_TO_VDDA)
                {
                    label.Text = LABEL_VOLTS_VDD;
                    nudRefVolt.Maximum = 5.5M;
                }
                else
                {
                    label.Text = LABEL_VOLTS;
                    nudRefVolt.Maximum = 1.5M;
                }
            }
            else
            {
                nudRefVolt.Maximum = 1.5M;
                label.Text = LABEL_VOLTS;
                nudRefVolt.Value = 1.024M;
                nudRefVolt.Enabled = false;
            }
        }

        private void m_cbConvMode2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.CONVERSION_MODE, m_cbConvMode2.Text);
            SetAParameter(ADC_DelSigParameters.CONVERSION_MODE_CONFIG2, prm, true);
            CheckFreqConfig2(true);
            DisplayInterruptMessage();
        }

        private void m_cbInputBufferMode2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_MODE_CONFIG2, 
                m_cbInputBufferMode2.Text);
            UpdateBufferGain(m_cbInputBufferMode2.Text, m_cbInputBufferGain2);
            SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_MODE_CONFIG2, prm, true);
            CheckFreqConfig2(true);
        }

        private void m_nudConfig_ValueChanged(object sender, EventArgs e)
        {
            int l_Configs = 4;
            int.TryParse(m_nudConfig.Value.ToString(), out l_Configs);

            UpdateConfigTabs(l_Configs);
            UpdateADCModes(true);
            SetAParameter(ADC_DelSigParameters.CONFIGS, m_nudConfig.Value.ToString(), false);
            DisplayInterruptMessage();
        }

        private void UpdateAllBufferGain()
        {
            UpdateBufferGain(m_cbInputBufferMode.Items[m_cbInputBufferMode.SelectedIndex].ToString(), 
                m_cbInputBufferGain);
            UpdateBufferGain(m_cbInputBufferMode2.Items[m_cbInputBufferMode2.SelectedIndex].ToString(), 
                m_cbInputBufferGain2);
            UpdateBufferGain(m_cbInputBufferMode3.Items[m_cbInputBufferMode3.SelectedIndex].ToString(), 
                m_cbInputBufferGain3);
            UpdateBufferGain(m_cbInputBufferMode4.Items[m_cbInputBufferMode4.SelectedIndex].ToString(), 
                m_cbInputBufferGain4);
        }

        private void UpdateBufferGain(string inputBufferMode, Control inputBufferGain)
        {
            /*Disable the BufferGain if the Buffer mode is set to Bypass Buffer*/
            if (inputBufferMode.Contains(BYPASS_BUFFER) || inputBufferMode.Contains(BYPASS_BUFFER_ENUM))
            {
                inputBufferGain.Enabled = false;
            }
            else
            {
                inputBufferGain.Enabled = true;
            }
        }

        private void m_cbInputBufferMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_MODE, 
                m_cbInputBufferMode.Text);
            UpdateBufferGain(m_cbInputBufferMode.Text, m_cbInputBufferGain);
            SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_MODE, prm, true);
            CheckFreqConfig1(true);

        }

        /*Update the ADC mode on all other configuration tabs*/
        private bool UpdateADCModes(bool saveAllModeConfig)
        {
            bool retVal = true;
            IEnumerable<string> InputRangeEnums = m_Component.GetPossibleEnumValues(ADC_DelSigParameters.INPUT_RANGE);
            ADC_DelSigParameters prms = new ADC_DelSigParameters(m_Component);

            m_cbInputRange.Items.Clear();
            m_cbInputRange2.Items.Clear();
            m_cbInputRange3.Items.Clear();
            m_cbInputRange4.Items.Clear();
            if (m_rbModeDifferential.Checked)
            {
                m_rbModeDifferential2.Checked = true;
                m_rbModeDifferential3.Checked = true;
                m_rbModeDifferential4.Checked = true;
                /*ONLY show the differential options in the "Input Range" paramter.*/
                foreach (string str in InputRangeEnums)
                {
                    if (!str.Contains(INPUT_SINGLE_TYPE))
                    {
                        m_cbInputRange.Items.Add(str);
                        m_cbInputRange2.Items.Add(str);
                        m_cbInputRange3.Items.Add(str);
                        m_cbInputRange4.Items.Add(str);
                    }
                }
            }
            else
            {
                m_rbModeSingle2.Checked = true;
                m_rbModeSingle3.Checked = true;
                m_rbModeSingle4.Checked = true;
                /*ONLY show the single options in the "Input Range" paramter.*/
                foreach (string str in InputRangeEnums)
                {
                    if (str.Contains(INPUT_SINGLE_TYPE))
                    {
                        m_cbInputRange.Items.Add(str);
                        m_cbInputRange2.Items.Add(str);
                        m_cbInputRange3.Items.Add(str);
                        m_cbInputRange4.Items.Add(str);
                    }
                }
            }

            //Set the ADC Input Range
            string paramInputRange = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE,
                prms.ADC_Input_Range.Expr);
            
            if (m_cbInputRange.Items.Contains(paramInputRange))
            {
                m_cbInputRange.SelectedItem = paramInputRange;
                m_cbInputRange.Text = prms.ADC_Input_Range.Expr;
                
            }
            else
            {
                /*Set to a default value.*/
                foreach (string str in m_cbInputRange.Items)
                {
                    /*Config1 input-range list contains a NULL member variable, hence skip that.*/
                    if (str != "")
                    {
                        m_cbInputRange.SelectedItem = str;
                        /*Store the value to symbol*/
                        string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE, 
                            m_cbInputRange.Text);

                        SetAParameter(ADC_DelSigParameters.INPUT_RANGE, prm, true);
                        break;
                    }
                }
            }

            //Set the ADC Input Range
            string paramInputRange2 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE_CONFIG2,
                prms.ADC_Input_Range_Config2.Expr);
            if (m_cbInputRange2.Items.Contains(paramInputRange2))
            {
                m_cbInputRange2.SelectedItem = paramInputRange2;
                m_cbInputRange2.Text = prms.ADC_Input_Range_Config2.Expr;
            }
            else
            {
                /*Set to a default value.*/
                foreach (string str in m_cbInputRange2.Items)
                {
                    m_cbInputRange2.SelectedItem = str;
                    string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE_CONFIG2, 
                            m_cbInputRange2.Text);

                    SetAParameter(ADC_DelSigParameters.INPUT_RANGE_CONFIG2, prm, true);
                    break;
                }
            }

            //Set the ADC Input Range
            string paramInputRange3 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE_CONFIG3,
                prms.ADC_Input_Range_Config3.Expr);
            if (m_cbInputRange3.Items.Contains(paramInputRange3))
            {
                m_cbInputRange3.SelectedItem = paramInputRange3;
                m_cbInputRange3.Text = prms.ADC_Input_Range_Config3.Expr;
            }
            else
            {
                /*Set to a default value.*/
                foreach (string str in m_cbInputRange3.Items)
                {
                    m_cbInputRange3.SelectedItem = str;
                    string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE_CONFIG3, 
                            m_cbInputRange3.Text);

                    SetAParameter(ADC_DelSigParameters.INPUT_RANGE_CONFIG3, prm, true);                    
                    break;
                }
            }

            //Set the ADC Input Range
            string paramInputRange4 = m_Component.ResolveEnumIdToDisplay(ADC_DelSigParameters.INPUT_RANGE_CONFIG4,
                prms.ADC_Input_Range_Config4.Expr);
            if (m_cbInputRange4.Items.Contains(paramInputRange4))
            {
                m_cbInputRange4.SelectedItem = paramInputRange4;
                m_cbInputRange4.Text = prms.ADC_Input_Range_Config4.Expr;
            }
            else
            {
                /*Set to a default value.*/
                foreach (string str in m_cbInputRange4.Items)
                {
                    m_cbInputRange4.SelectedItem = str;
                    string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE_CONFIG4, 
                            m_cbInputRange4.Text);

                    SetAParameter(ADC_DelSigParameters.INPUT_RANGE_CONFIG4, prm, true);                    
                    break;
                }
            }


            if (m_nudConfig.Value > 1)
            {
                CheckInputRange((Control)m_cbInputRange2, true);
                CheckInputRange((Control)m_cbInputRange3, true);
                CheckInputRange((Control)m_cbInputRange4, true);
            }

            return retVal;
        }

        private void m_rbModeDifferential_Validating(object sender, CancelEventArgs e)
        {
            /*The mode changed, ask user to correctly set the Range. */
            //m_cbInputRange.Focus();
        }

        private void m_cbInputRange_Validating(object sender, CancelEventArgs e)
        {
            CheckInputRange((Control)m_cbInputRange, true);
            CheckInputRange((Control)m_cbInputRange2, true);
            CheckInputRange((Control)m_cbInputRange3, true);
            CheckInputRange((Control)m_cbInputRange4, true);
        }

        private void m_cbInputRange2_Validating(object sender, CancelEventArgs e)
        {
            CheckInputRange((Control)m_cbInputRange2, true);
            CheckInputRange((Control)m_cbInputRange3, true);
            CheckInputRange((Control)m_cbInputRange4, true);              
        }

        private void m_cbInputRange3_Validating(object sender, CancelEventArgs e)
        {
            CheckInputRange((Control)m_cbInputRange3, true);
            CheckInputRange((Control)m_cbInputRange4, true);
        }

        private void m_cbInputRange4_Validating(object sender, CancelEventArgs e)
        {
            CheckInputRange((Control)m_cbInputRange4, true);
        }

        /*Handle ADC mode.
         *Either m_rbModeSingle, m_rbModeDifferential is allowed.*/
        private void m_rbModeDifferential_CheckedChanged(object sender, EventArgs e)
        {
            string prm = "";
            if (m_rbModeDifferential.Checked)
            {
                prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.ADC_INPUT_MODE,
                    m_rbModeDifferential.Text);
            }
            else
            {
                prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.ADC_INPUT_MODE,
                    m_rbModeSingle.Text);
            }

            UnhookAllEvents();
            UpdateADCModes(false);
            HookAllEvents();
            SetAParameter(ADC_DelSigParameters.ADC_INPUT_MODE, prm, true);
            UpdateRefVoltages();
            UpdateRefVoltageEnable(m_cbReference, m_cbInputRange, Volts_label, m_nudRefVoltage);
            UpdateRefVoltageEnable(m_cbReference2, m_cbInputRange2, Volts_label2, m_nudRefVoltage2);
            UpdateRefVoltageEnable(m_cbReference3, m_cbInputRange3, Volts_label3, m_nudRefVoltage3);
            UpdateRefVoltageEnable(m_cbReference4, m_cbInputRange4, Volts_label4, m_nudRefVoltage4);
            CheckInputRange(m_cbInputRange, true);
        }

        private bool CheckInputRange(Control parent, bool saveAllModeConfig)
        {
            bool retVal = false;
            /*Check for the Input range validity*/
            if (m_rbModeDifferential.Checked)
            {
                if (((ComboBox)parent).Text.Contains(INPUT_SINGLE_TYPE))
                {
                    m_errorProvider.SetError(parent, ERROR_INPUT_DIFFERENTIAL);
                }
                else
                {
                    m_errorProvider.SetError(parent, String.Empty);
                    retVal = true;
                }
            }
            else
            {
                if (!((ComboBox)parent).Text.Contains(INPUT_SINGLE_TYPE))
                {
                    m_errorProvider.SetError(parent, ERROR_INPUT_SINGLE);
                }
                else
                {
                    m_errorProvider.SetError(parent, String.Empty);
                    retVal = true;
                }
            }
            return retVal;
        }

        private void m_cbInputRange2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE_CONFIG2, 
                m_cbInputRange2.Text);

            // If "Vssa to Vdda (Single Ended)" is selected then choose refernce to be "Internal Ref"*/
            if (m_cbInputRange2.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference2.Text = REF_INTERNAL_REF;
                m_cbReference2.Enabled = false;
            }
            else
            {
                m_cbReference2.Enabled = true;
            }

            if (CheckInputRange(m_cbInputRange2, true))
            {
                SetAParameter(ADC_DelSigParameters.INPUT_RANGE_CONFIG2, prm, true);
            }
            UpdateRefVoltageEnable(m_cbReference2, m_cbInputRange2, Volts_label2, m_nudRefVoltage2);
        }

        private void m_cbInputBufferGain2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_GAIN, 
                m_cbInputBufferGain2.Text);
            SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_GAIN_CONFIG2, prm, true);
            CheckFreqConfig2(true);
        }

        private void m_cbReference2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.ADC_REFERENCE, m_cbReference2.Text);
            SetAParameter(ADC_DelSigParameters.ADC_REFERENCE_CONFIG2, prm, true);
            UpdateRefVoltageEnable(m_cbReference2, m_cbInputRange2, Volts_label2, m_nudRefVoltage2);
        }

        private void m_nudRefVoltage2_ValueChanged(object sender, EventArgs e)
        {
            //Set is the upper limit
            SetAParameter(ADC_DelSigParameters.REF_VOLTAGE_CONFIG2, m_nudRefVoltage2.Value.ToString(), false);
        }

        private void m_cbConvMode3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.CONVERSION_MODE, m_cbConvMode3.Text);
            SetAParameter(ADC_DelSigParameters.CONVERSION_MODE_CONFIG3, prm, true);
            CheckFreqConfig3(true);
            DisplayInterruptMessage();
        }
        
        private void m_cbConvMode4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.CONVERSION_MODE, m_cbConvMode4.Text);
            SetAParameter(ADC_DelSigParameters.CONVERSION_MODE_CONFIG4, prm, true);
            CheckFreqConfig4(true);
            DisplayInterruptMessage();
        }

        private void m_cbInputRange3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE_CONFIG3, 
                m_cbInputRange3.Text);

            // If "Vssa to Vdda (Single Ended)" is selected then choose refernce to be "Internal Ref"
            if (m_cbInputRange3.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference3.Text = REF_INTERNAL_REF;
                m_cbReference3.Enabled = false;
            }
            else
            {
                m_cbReference3.Enabled = true;
            }

            if (CheckInputRange(m_cbInputRange3, true))
            {
                SetAParameter(ADC_DelSigParameters.INPUT_RANGE_CONFIG3, prm, true);
            }
            UpdateRefVoltageEnable(m_cbReference3, m_cbInputRange3, Volts_label3, m_nudRefVoltage3);
        }

        private void m_cbInputBufferGain3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_GAIN,
                m_cbInputBufferGain3.Text);
            SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_GAIN_CONFIG3, prm, true);
            CheckFreqConfig3(true);
        }

        private void m_cbInputBufferMode3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_MODE_CONFIG2,
                m_cbInputBufferMode3.Text);
            UpdateBufferGain(m_cbInputBufferMode3.Text, m_cbInputBufferGain3);
            SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_MODE_CONFIG3, prm, true);
            CheckFreqConfig3(true);
        }

        private void m_cbReference3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.ADC_REFERENCE, m_cbReference3.Text);
            SetAParameter(ADC_DelSigParameters.ADC_REFERENCE_CONFIG3, prm, true);
            UpdateRefVoltageEnable(m_cbReference3, m_cbInputRange3, Volts_label3, m_nudRefVoltage3);
        }

        private void m_nudRefVoltage3_ValueChanged(object sender, EventArgs e)
        {
            //Set is the upper limit
            SetAParameter(ADC_DelSigParameters.REF_VOLTAGE_CONFIG3, m_nudRefVoltage3.Value.ToString(), false);
        }

        private void m_cbResolutionConfig4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.RESOLUTION, m_cbResolutionConfig4.Text);
            SetAParameter(ADC_DelSigParameters.RESOLUTION_CONFIG4, prm, true);
            CheckFreqConfig4(true);
            DisplayInterruptMessage();
        }

        private void m_cbInputRange4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_RANGE_CONFIG4,
                m_cbInputRange4.Text);

            // If "Vssa to Vdda (Single Ended)" is selected then choose refernce to be "Internal Ref"
            if (m_cbInputRange4.Text == RANGE_VSSA_TO_VDDA)
            {
                m_cbReference4.Text = REF_INTERNAL_REF;
                m_cbReference4.Enabled = false;
            }
            else
            {
                m_cbReference4.Enabled = true;
            }

            if (CheckInputRange(m_cbInputRange4, true))
            {
                SetAParameter(ADC_DelSigParameters.INPUT_RANGE_CONFIG4, prm, true);
            }
            UpdateRefVoltageEnable(m_cbReference4, m_cbInputRange4, Volts_label4, m_nudRefVoltage4);
        }

        private void m_cbInputBufferGain4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_GAIN,
                m_cbInputBufferGain4.Text);
            SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_GAIN_CONFIG4, prm, true);
            CheckFreqConfig4(true);            
        }

        private void m_cbInputBufferMode4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.INPUT_BUFFER_MODE_CONFIG2,
                m_cbInputBufferMode4.Text);
            UpdateBufferGain(m_cbInputBufferMode4.Text, m_cbInputBufferGain4);
            SetAParameter(ADC_DelSigParameters.INPUT_BUFFER_MODE_CONFIG4, prm, true);
            CheckFreqConfig4(true);
        }

        private void m_cbReference4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(ADC_DelSigParameters.ADC_REFERENCE, m_cbReference4.Text);
            SetAParameter(ADC_DelSigParameters.ADC_REFERENCE_CONFIG4, prm, true);
            UpdateRefVoltageEnable(m_cbReference4, m_cbInputRange4, Volts_label4, m_nudRefVoltage4);
        }

        private void m_nudRefVoltage4_ValueChanged(object sender, EventArgs e)
        {
            /*Set is the upper limit*/
            SetAParameter(ADC_DelSigParameters.REF_VOLTAGE_CONFIG4, m_nudRefVoltage4.Value.ToString(), false);
        }


        private void m_cbEnable_Vref_Vss_CheckedChanged(object sender, EventArgs e)
        {
            SetAParameter(ADC_DelSigParameters.ADC_nVref, m_cbEnable_Vref_Vss.Checked.ToString(), true);
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
