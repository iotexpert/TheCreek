/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Diagnostics;

namespace ADC_SAR_v1_80
{
    [CyCompDevCustomizer()]
    public class CyCustomizer : ICyParamEditHook_v1, ICyDRCProvider_v1, ICyInstValidateHook_v1
    {
        public const string CONFIGURE_TAB_NAME = "Configure";

        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CySARADCParameters parameters = new CySARADCParameters(edit, termQuery);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate ExprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.LoadParameters();
            };

            editor.AddCustomPage(Resources.ConfigureTabCaption, new CySARADCControl(parameters, edit.DeviceQuery),
                ExprDelegate, CONFIGURE_TAB_NAME);
            editor.AddDefaultPage(Resources.BuiltInTabCaption, "Built-in");

            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(parameters.UpdateClock));

            parameters.LoadParameters();
            parameters.m_globalEditMode = true;
            return editor.ShowDialog();
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion

        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            CySARADCParameters parameters = new CySARADCParameters(args.InstQueryV1, args.TermQueryV1);
            CyCustErr err;

            // These verifications depend on silicon and cannot be calculated when silicon is unknown
            if (CySARADCParameters.IsArchMemberAvailable)
            {
                err = VerifyInternalClock(args.InstQueryV1.DesignQuery, parameters.AdcClockSource,
                    parameters.InstanceName);
                if (err.IsOk == false)
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);

                err = VerifyExternalClock(args.TermQueryV1, parameters.AdcClockSource);
                if (err.IsOk == false)
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
            }

            err = VerifyVoltage(args.InstQueryV1, parameters.RefVoltage, parameters.AdcReference,
                parameters.AdcInputRange);
            if (err.IsOk == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);

            err = VerifyReference(parameters.AdcReference, parameters.AdcClockSource, parameters.AdcSampleRate);
            if (err.IsOk == false)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, err.Message);
        }

        CyCustErr VerifyInternalClock(ICyDesignQuery_v1 designQuery, CyEAdcClockSrcType adcClock, string instanceName)
        {
            double internalClockValue = 0;

            if (adcClock == CyEAdcClockSrcType.Internal)
            {
                internalClockValue = CyClockReader.GetInternalClockInMHz(designQuery, instanceName
                    + CyClockReader.INTERNAL_CLK);

                if (internalClockValue >= 0)
                {
                    // Ten to the six power for converting Hz to MHz
                    const int POW6 = 1000000;
                    // Converting clock name to clock ID for accuracy getting function
                    string clockID = CyClockReader.GetClockID(designQuery, instanceName
                        + CyClockReader.INTERNAL_CLK);
                    // Get accuracy prcent with negative sign
                    double plusAccuracyPercent = designQuery.GetClockPlusAccuracyPercent(clockID);
                    // Get accuracy prcent with plus sign
                    double minusAccuracyPercent = designQuery.GetClockMinusAccuracyPercent(clockID);
                    // Minimum clock value with plus accuracy percent
                    double minClockPlusAccuracy = CyParamRange.ClockFreqMinHz + (CyParamRange.ClockFreqMinHz
                        * plusAccuracyPercent / 100);
                    // Maximum clock value with negative accuracy percent
                    double maxClockMinusAccuracy = CyParamRange.ClockFreqMaxHz - (CyParamRange.ClockFreqMaxHz
                        * minusAccuracyPercent / 100);

                    if (internalClockValue < minClockPlusAccuracy || internalClockValue > maxClockMinusAccuracy)
                    {
                        return new CyCustErr(string.Format(Resources.DRCInternalClockFreqMsg,
                            CyParamRange.ClockFreqMinMHz, plusAccuracyPercent, minClockPlusAccuracy / POW6,
                            CyParamRange.ClockFreqMaxMHz, minusAccuracyPercent, maxClockMinusAccuracy / POW6));
                    }
                    else
                    {
                        double masterClockValue = CyClockReader.GetInternalClockInMHz(designQuery,
                            CyClockReader.MASTER_CLK);
                        if (masterClockValue > 0)
                        {
                            double x = masterClockValue / internalClockValue;
                            double y;
                            if (x == 1)
                            {
                                y = (1 / (masterClockValue / POW6) * ((x / 2))) * 1000;
                            }
                            else
                            {
                                y = (1 / (masterClockValue / POW6) * (Math.Floor(x / 2))) * 1000;
                            }
                            if (y < CyParamRange.MinimumPulseWidth)
                                return new CyCustErr(string.Format(Resources.DRCMinimumPulseWidthMsg,
                                    CyParamRange.MinimumPulseWidth));
                            else
                                return CyCustErr.OK;
                        }
                    }
                }
            }
            return CyCustErr.OK;
        }

        CyCustErr VerifyExternalClock(ICyTerminalQuery_v1 termQuery, CyEAdcClockSrcType clockSource)
        {
            if (clockSource == CyEAdcClockSrcType.External)
            {
                double externalClockInKHz = CyClockReader.GetExternalClockInKHz(termQuery, CyClockReader.EXTERNAL_CLK);
                if (externalClockInKHz > 0)
                {
                    if (externalClockInKHz < CyParamRange.ClockFreqMinKHz ||
                        externalClockInKHz > CyParamRange.ClockFreqMaxKHz)
                    {
                        return new CyCustErr(string.Format(Resources.ClockFrequencyErrorMsg,
                            CyParamRange.ClockFreqMinMHz, CyParamRange.ClockFreqMaxMHz));
                    }
                }
            }
            return CyCustErr.OK;
        }

        CyCustErr VerifyVoltage(ICyInstQuery_v1 instQuery, double refVoltage, CyEAdcRefType reference,
            CyEAdcInputRangeType inputRange)
        {
            double vdda = 0;

            try
            {
                vdda = Convert.ToDouble(Math.Round(instQuery.VoltageQuery.VDDA, 4));
            }
            catch
            {
                vdda = 0;
            }
            if (vdda > 0)
            {
                double maxVoltage;
                string message;
                // External voltage reference should be limited to Vdda/2 for "Vssa to Vdda(Single)" 
                // and "0.0 +/- Vdda/2(Differential)" input ranges

                if (reference == CyEAdcRefType.Ext_Ref && (inputRange == CyEAdcInputRangeType.Vssa_to_Vdda ||
                    inputRange == CyEAdcInputRangeType.Vneg_Vdda_2_Diff))
                {
                    maxVoltage = vdda / 2;
                    message = Resources.VoltageReferenceErrorMsgVddaDiv2;
                }
                else
                {
                    maxVoltage = vdda;
                    message = Resources.VoltageReferenceErrorMsg;
                }

                if (refVoltage < CyParamRange.REF_VOLTAGE_MIN || refVoltage > maxVoltage)
                {
                    return new CyCustErr(message, CyParamRange.REF_VOLTAGE_MIN, maxVoltage);
                }
            }
            return CyCustErr.OK;
        }

        CyCustErr VerifyReference(CyEAdcRefType reference, CyEAdcClockSrcType clockSource, uint sampleRate)
        {
            if (reference == CyEAdcRefType.Int_Ref_Not_Bypassed && clockSource == CyEAdcClockSrcType.Internal &&
                sampleRate > CyParamRange.CONVERSION_RATE_INT_VREF_MAX)
                return new CyCustErr(Resources.InvalidReferenceErrorMsg);
            else
                return CyCustErr.OK;
        }
        #endregion

        #region ICyInstValidateHook_v1 Members
        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            CySARADCParameters parameters = new CySARADCParameters(instVal);
            if (CySARADCParameters.IsArchMemberAvailable)
            {
                // Conversion rate validation
                if (parameters.AdcClockSource == CyEAdcClockSrcType.Internal &&
                    (parameters.AdcSampleRate < parameters.GetMinSampleRate() ||
                    parameters.AdcSampleRate > parameters.GetMaxSampleRate()))
                {
                    CyCustErr err = new CyCustErr(string.Format(Resources.DRCConversionRate,
                        CyParamRange.ClockFreqMinMHz, CyParamRange.ClockFreqMaxMHz));
                    instVal.AddError(CyParamNames.SAMPLE_RATE, err);
                }
            }
        }
        #endregion
    }
}
