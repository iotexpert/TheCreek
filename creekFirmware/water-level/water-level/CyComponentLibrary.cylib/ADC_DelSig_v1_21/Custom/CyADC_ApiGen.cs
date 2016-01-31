//*******************************************************************************
// File Name: CyADC_ApiGen.cs
/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using ADC_DelSig_v1_21;



namespace ADC_DelSig_v1_21
{
    partial class CyCustomizer : ICyAPICustomize_v1
    {
#region ParameterNamesValues
        string DEC_CR_ParamName     = "DFLT_DEC_CR";
        string DEC_SR_ParamName     = "DFLT_DEC_SR";
        string DEC_SHIFT1_ParamName = "DFLT_DEC_SHIFT1";
        string DEC_SHIFT2_ParamName = "DFLT_DEC_SHIFT2";
        string DEC_DR2_ParamName    = "DFLT_DEC_DR2";
        string DEC_DR2H_ParamName   = "DFLT_DEC_DR2H";
        string DEC_DR1_ParamName    = "DFLT_DEC_DR1";
        string DEC_OCOR_ParamName   = "DFLT_DEC_OCOR";
        string DEC_OCORM_ParamName  = "DFLT_DEC_OCORM";
        string DEC_OCORH_ParamName  = "DFLT_DEC_OCORH";
        string DEC_GVAL_ParamName   = "DFLT_DEC_GVAL";
        string DEC_GCOR_ParamName   = "DFLT_DEC_GCOR";
        string DEC_GCORH_ParamName  = "DFLT_DEC_GCORH";
        string DEC_COHER_ParamName  = "DFLT_DEC_COHER";

        string DSM_CR0_ParamName = "DFLT_DSM_CR0";
        string DSM_CR1_ParamName = "DFLT_DSM_CR1";
        string DSM_CR2_ParamName = "DFLT_DSM_CR2";
        string DSM_CR3_ParamName = "DFLT_DSM_CR3";
        string DSM_CR4_ParamName = "DFLT_DSM_CR4";
        string DSM_CR5_ParamName = "DFLT_DSM_CR5";
        string DSM_CR6_ParamName = "DFLT_DSM_CR6";
        string DSM_CR7_ParamName = "DFLT_DSM_CR7";
        string DSM_CR8_ParamName = "DFLT_DSM_CR8";
        string DSM_CR9_ParamName = "DFLT_DSM_CR9";

        string DSM_CR10_ParamName = "DFLT_DSM_CR10";
        string DSM_CR11_ParamName = "DFLT_DSM_CR11";
        string DSM_CR12_ParamName = "DFLT_DSM_CR12";
        string DSM_CR13_ParamName = "DFLT_DSM_CR13";
        string DSM_CR14_ParamName = "DFLT_DSM_CR14";
        string DSM_CR15_ParamName = "DFLT_DSM_CR15";
        string DSM_CR16_ParamName = "DFLT_DSM_CR16";
        string DSM_CR17_ParamName = "DFLT_DSM_CR17";

        string DSM_REF0_ParamName = "DFLT_DSM_REF0";
        string DSM_REF1_ParamName = "DFLT_DSM_REF1";
        string DSM_REF2_ParamName = "DFLT_DSM_REF2";
        string DSM_REF3_ParamName = "DFLT_DSM_REF3";

        string DSM_DEM0_ParamName = "DFLT_DSM_DEM0";
        string DSM_DEM1_ParamName = "DFLT_DSM_DEM1";
        string DSM_MISC_ParamName = "DFLT_DSM_MISC";
        string DSM_CLK_ParamName  = "DFLT_DSM_CLK";

        string DSM_BUF0_ParamName = "DFLT_DSM_BUF0";
        string DSM_BUF1_ParamName = "DFLT_DSM_BUF1";
        string DSM_BUF2_ParamName = "DFLT_DSM_BUF2";
        string DSM_BUF3_ParamName = "DFLT_DSM_BUF3";
        string DSM_OUT0_ParamName = "DFLT_DSM_OUT0";
        string DSM_OUT1_ParamName = "DFLT_DSM_OUT1";
        string clockFrequency_ParamName = "DFLT_CLK_FREQ";
        string clocksPerSample_ParamName = "DFLT_CLOCKS_PER_SAMPLE";

        string DSM_SW3_ParamName = "DFLT_DSM_SW3";

        string ADC_ClockParamName = "ADC_Clock";
        string ADC_ClockTermName = "aclock";
        string ADC_Input_RangeParamName = "ADC_Input_Range";
        string ADC_PowerParamName = "ADC_Power";
        string ADC_ReferenceParamName = "ADC_Reference";
        string ADC_ResolutionParamName = "ADC_Resolution";
        string Conversion_ModeParamName = "Conversion_Mode";
        string Input_Buffer_GainParamName = "Input_Buffer_Gain";
        string Sample_RateParamName = "Sample_Rate";
        string Start_of_ConversionParamName = "Start_of_Conversion";
        string Ref_VoltageParamName = "Ref_Voltage";
        string CountsPerVoltParamName = "DFLT_COUNTS_PER_VOLT";
        string RefVoltage_ParamName = "DFLT_REF_VOLTAGE";

        uint ADC_Clock;
        uint ADC_Input_Range;
        uint ADC_Power;
        uint ADC_Reference;
        uint ADC_Resolution;
        uint Conversion_Mode;
        uint Input_Buffer_Gain;
        uint Sample_Rate;
        uint Start_of_Conversion;
        float Ref_Voltage;
        uint CountsPerVolt;
 //       uint clocksPerSample = 0;
        

        uint DecCrReg;
        uint DecSrReg;
        uint DecShift1Reg;
        uint DecShift2Reg;
        uint DecDr2Reg;
        uint DecDr2hReg;
        uint DecDr1Reg;
        uint DecOcorReg;
        uint DecOcormReg;
        uint DecOcorhReg;
        uint DecGvalReg;
        uint DecGcorReg;
        uint DecGcorhReg;
        uint DecCoherReg;

        uint DsmCr0Reg;
        uint DsmCr1Reg;
        uint DsmCr2Reg;
        uint DsmCr3Reg;
        uint DsmCr4Reg;
        uint DsmCr5Reg;
        uint DsmCr6Reg;
        uint DsmCr7Reg;
        uint DsmCr8Reg;
        uint DsmCr9Reg;
        uint DsmCr10Reg;
        uint DsmCr11Reg;
        uint DsmCr12Reg;
        uint DsmCr13Reg;
        uint DsmCr14Reg;
        uint DsmCr15Reg;
        uint DsmCr16Reg;
        uint DsmCr17Reg;

        uint DsmRef0Reg;
        uint DsmRef1Reg;
        uint DsmRef2Reg;
        uint DsmRef3Reg;

        uint DsmDem0Reg;
        uint DsmDem1Reg;
        uint DsmMiscReg;
        uint DsmClkReg;

        uint DsmBuf0Reg;
        uint DsmBuf1Reg;
        uint DsmBuf2Reg;
        uint DsmBuf3Reg;
        uint DsmOut0Reg;
        uint DsmOut1Reg;

        uint DsmSw3Reg;

        ICyTerminalQuery_v1 m_termQuery;
        ICyInstQuery_v1 m_instQuery;
  

#endregion

        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, IEnumerable<CyAPICustomizer> apis)
        {
            List<CyClockData> clockStuff = termQuery.GetClockData(ADC_ClockTermName, 3);

            Dictionary<string, string> paramDict = null;
            m_termQuery = termQuery;
            m_instQuery = query;
            foreach (CyAPICustomizer api in apis)
            {
                // Get parameter dictionary
                paramDict = api.MacroDictionary;
            }

            // Calculate values
            CalcValues(ref paramDict);

            // Assign dictionary back to API customizers
            foreach (CyAPICustomizer api in apis)
            {
                // Get parameter dictionary
                api.MacroDictionary = paramDict;
            }
            // Return customizers
            return apis;

        }

       
        #endregion

//*******************************************************************************
// Function Name: SetResolution
//*******************************************************************************
// Summary:
//  Set ADC resolution
//
// Parameters:  
//  gain:  Set the ADC resolution 
//
// Return: 
//  (void) 
//
// Theory: 
//
// Side Effects:
//
//******************************************************************************/
        void SetResolution(uint resolution)
        {
           uint rShiftOffset = 0;
   
            ADC_Resolution = resolution;

            // Offset correction 
            // This could be used to offset result for signed and unsigned results 
            DecOcorReg = 0;
            DecOcormReg  = 0;
            DecOcorhReg  = 0;

            // Gain correction 
            DecGvalReg   = 0;
            DecGcorReg   = 0;
            DecGcorhReg  = 0;

            if(resolution < rc.MIN_RESOLUTION) resolution = rc.MIN_RESOLUTION;
            if(resolution > rc.MAX_RESOLUTION) resolution = rc.MAX_RESOLUTION;


            // If single ended mode, don't shift an additional bit 
            if((ADC_Input_Range == rc.ADC_IR_VSSA_TO_VREF)  | (ADC_Input_Range == rc.ADC_IR_VSSA_TO_2VREF)|(ADC_Input_Range == rc.ADC_IR_VSSA_TO_VDDA) )
            {
                rShiftOffset = 1;
            }

            resolution -= rc.MIN_RESOLUTION;

            // Set resolution constants from table 
            DecDr1Reg     = resSettings.dr1[resolution];  
            DecShift1Reg  = resSettings.shift1[resolution];

            DecDr2Reg     = resSettings.dr2[resolution];  
            DecDr2hReg    = resSettings.dr2h[resolution];  
            DecShift2Reg  = resSettings.shift2[resolution] - rShiftOffset;

        }


//*******************************************************************************
// Function Name: SetBufferGain
//*******************************************************************************
// Summary:
//  Set input buffer range.
//
// Parameters:  
//  gain:  Two bit value to select a gain of 1, 2, 4, or 8.
//
// Return: 
//  (void) 
//
// Theory: 
//
// Side Effects:
//
//******************************************************************************/
void SetBufferGain(uint gain)
{
   uint tmpReg;
   uint gainSetting = 1;
   if (gain != 0)
   {
       switch (gain)
       {
           case 1:
               gainSetting = rc.DSM_GAIN_1X;
               break;
           case 2:
               gainSetting = rc.DSM_GAIN_2X;
               break;
           case 4:
               gainSetting = rc.DSM_GAIN_4X;
               break;
           case 8:
               gainSetting = rc.DSM_GAIN_8X;
               break;
           default:
               gainSetting = rc.ADC_IBG_1X;
               break;

       }

       tmpReg = DsmBuf1Reg & ~rc.DSM_GAIN_MASK;
       tmpReg |= gainSetting;
       DsmBuf1Reg = tmpReg;

   }
   else    // Input buffer is disabled
   {
       DsmBuf0Reg = rc.DSM_BYPASS_P;   // Bypass positive buffer channel
       DsmBuf1Reg = rc.DSM_BYPASS_N;   // Bypass negative buffer channel
       DsmBuf2Reg = 0x00;              // Disable power and RC
       DsmBuf3Reg = 0x00;              // Disable chopper
   }
   
}

//*******************************************************************************
// Function Name: SetRef
//*******************************************************************************
// Summary:
//  Sets reference for ADC
//
// Parameters:  
//  refMode: Reference configuration.
//
// Return: 
//  (void) 
//
// Theory: 
//
// Side Effects:
//
//*****************************************************************************
void SetRef(uint refMode, uint inputMode)
{

    // Mask off reference 
    DsmRef0Reg &= ~rc.DSM_REFMUX_MASK;

    // Connect the switch matrix for the proper reference mode 
    if (inputMode != rc.ADC_IR_VSSA_TO_VDDA)
    {
        switch (refMode)
        {
            case rc.ADC_INT_REF_NOT_BYPASSED:
                DsmRef0Reg |= rc.DSM_REFMUX_1_024V;
                DsmRef2Reg = rc.DSM_REF2_S3_EN | rc.DSM_REF2_S4_EN | rc.DSM_REF2_S6_EN;
                DsmRef3Reg = rc.DSM_REF3_NO_SW;
                DsmCr17Reg |= rc.DSM_EN_BUF_VREF;  // Enable Int Ref Amp 
                break;

            case rc.ADC_INT_REF_BYPASSED_ON_P32:
                DsmRef0Reg |= rc.DSM_REFMUX_1_024V;
                DsmRef2Reg = rc.DSM_REF2_S0_EN | rc.DSM_REF2_S1_EN | rc.DSM_REF2_S2_EN | rc.DSM_REF2_S6_EN;
                DsmRef3Reg = rc.DSM_REF3_NO_SW;
                DsmCr17Reg |= rc.DSM_EN_BUF_VREF;  // Enable Int Ref Amp 
                break;

            case rc.ADC_INT_REF_BYPASSED_ON_P03:
                DsmRef0Reg |= rc.DSM_REFMUX_1_024V;
                DsmRef2Reg = rc.DSM_REF2_S6_EN | rc.DSM_REF2_S7_EN;
                DsmRef3Reg = rc.DSM_REF3_S8_EN | rc.DSM_REF3_S9_EN;
                DsmCr17Reg |= rc.DSM_EN_BUF_VREF;  // Enable Int Ref Amp 
                break;

            case rc.ADC_EXT_REF_ON_P03:
                DsmRef2Reg = rc.DSM_REF2_NO_SW;
                DsmRef3Reg = rc.DSM_REF3_S9_EN | rc.DSM_REF3_S11_EN;
                DsmCr17Reg &= ~rc.DSM_EN_BUF_VREF;  // Disable Int Ref Amp 
                break;

            case rc.ADC_EXT_REF_ON_P32:
                DsmRef2Reg = rc.DSM_REF2_S2_EN | rc.DSM_REF2_S5_EN;
                DsmRef3Reg = rc.DSM_REF3_NO_SW;
                DsmCr17Reg &= ~rc.DSM_EN_BUF_VREF;   // Disable Int Ref Amp 
                break;

            default:

                break;
        }
    }
    else  // VSSA to VDDA
    {
        DsmRef0Reg = (rc.DSM_REFMUX_VDA_4 | rc.DSM_EN_BUF_VREF_INN | rc.DSM_VREF_RES_DIV_EN);
        switch (refMode)
        {
            case rc.ADC_INT_REF_NOT_BYPASSED:
                DsmRef2Reg = rc.DSM_REF2_S3_EN | rc.DSM_REF2_S4_EN | rc.DSM_REF2_S6_EN;
                DsmRef3Reg = rc.DSM_REF3_NO_SW;
                DsmCr17Reg |= rc.DSM_EN_BUF_VREF;  // Enable Int Ref Amp 
                break;

            case rc.ADC_INT_REF_BYPASSED_ON_P32:
                DsmRef2Reg = rc.DSM_REF2_S0_EN | rc.DSM_REF2_S1_EN | rc.DSM_REF2_S2_EN | rc.DSM_REF2_S6_EN;
                DsmRef3Reg = rc.DSM_REF3_NO_SW;
                DsmCr17Reg |= rc.DSM_EN_BUF_VREF;  // Enable Int Ref Amp 
                break;

            case rc.ADC_INT_REF_BYPASSED_ON_P03:
                DsmRef2Reg = rc.DSM_REF2_S6_EN | rc.DSM_REF2_S7_EN;
                DsmRef3Reg = rc.DSM_REF3_S8_EN | rc.DSM_REF3_S9_EN;
                DsmCr17Reg |= rc.DSM_EN_BUF_VREF;  // Enable Int Ref Amp 
                break;

            case rc.ADC_EXT_REF_ON_P03:
            case rc.ADC_EXT_REF_ON_P32:
            default:
                break;

        }
    }
}

// *******************************************************************************
// Function Name: SetInputRange
// ********************************************************************************
// Summary:
//  Sets input range of the ADC independent of the input buffer gain.
//
// Parameters:  
//  mode:  ADC input configuration mode.  
//
// Return: 
//  (void) 
//
// Theory: 
//
// Side Effects:
//
// ******************************************************************************
void SetInputRange(uint inputMode)
{

    // Set modulator gain input caps to a gain of 1 by default
    if (ADC_Resolution > 15)
    {
        DsmCr5Reg = rc.DSM_IPCAP1_3200FF | rc.DSM_IPCAP1_800FF | rc.DSM_IPCAP1_400FF;
        DsmCr6Reg = rc.DSM_DACCAP_3200FF | rc.DSM_DACCAP_800FF | rc.DSM_DACCAP_400FF;
    }
    else
    {
        DsmCr5Reg = rc.DSM_IPCAP1_800FF | rc.DSM_IPCAP1_200FF | rc.DSM_IPCAP1_100FF;
        DsmCr6Reg = rc.DSM_DACCAP_800FF | rc.DSM_DACCAP_192FF | rc.DSM_DACCAP_96FF;
    }

    // Configure Ref switches, reference, and input buffer
    switch (inputMode)
    {
        case rc.ADC_IR_VSSA_TO_VREF: // Single ended, Vss to Vref
            // Set Single Ended input and disable negative buffer
            SetBuffer(rc.BUF_BYPASS_NEG | rc.BUF_RAIL_TO_RAIL);

            // Set reference mux to 1.024V reference
            DsmRef0Reg &= rc.DSM_REFMUX_MASK;
            DsmRef0Reg |= rc.DSM_REFMUX_1_024V;

            // Set modulator gain input caps to a gain of 1
            // Default setting

            // Connect negative input to Vssa
            DsmSw3Reg = rc.DSM_VN_VSSA;

            break;

        case rc.ADC_IR_VSSA_TO_2VREF: // Single ended, Vss to 2*Vref
            // This is the same as Vss to Vref, but input caps are changed for input gain of 1/2 
            // Set Single Ended input and disable negative buffer
            SetBuffer(rc.BUF_BYPASS_NEG | rc.BUF_RAIL_TO_RAIL);

            // Set reference mux to 1.024V reference
            DsmRef0Reg &= rc.DSM_REFMUX_MASK;
            DsmRef0Reg |= rc.DSM_REFMUX_1_024V;

            // Set modulator gain input caps to a gain of 1
            // Default setting

            // Connect negative input to Vssa
            DsmSw3Reg = rc.DSM_VN_VSSA;

            // Set input gain to 2
            DsmCr5Reg = rc.DSM_IPCAP1_1600FF;
            DsmCr6Reg = rc.DSM_DACCAP_3200FF;
            break;

        case rc.ADC_IR_VSSA_TO_VDDA:  // Single ended, Vssa to Vdda
                                      // Vref = Vdda/4, Input gain = 1/2
                                      // INN = Vref, differential mode
            //SetBuffer(rc.BUF_DIFF_MODE | rc.BUF_RAIL_TO_RAIL);
			SetBuffer(rc.BUF_BYPASS_NEG | rc.BUF_RAIL_TO_RAIL);

            // Set reference mux to VDA/4 reference
            DsmRef0Reg = (rc.DSM_REFMUX_VDA_4 | rc.DSM_EN_BUF_VREF_INN | rc.DSM_VREF_RES_DIV_EN | rc.DSM_VCMSEL_0_8V);

            // Connect negative input to Vssa
            DsmSw3Reg = rc.DSM_VN_VSSA;
 
            // Set modulator gain input caps to a gain of 1/4
            if (ADC_Resolution > 15)
            {
                DsmCr5Reg = rc.DSM_IPCAP1_800FF | rc.DSM_IPCAP1_200FF | rc.DSM_IPCAP1_100FF;
                DsmCr6Reg = rc.DSM_DACCAP_3200FF | rc.DSM_DACCAP_800FF | rc.DSM_DACCAP_400FF;
            }
            else
            {
                DsmCr5Reg = rc.DSM_IPCAP1_400FF | rc.DSM_IPCAP1_100FF ;
                DsmCr6Reg = rc.DSM_DACCAP_1600FF | rc.DSM_DACCAP_400FF ;
            }
            break;

        case rc.ADC_IR_VNEG_VREF_DIFF:  // Diff, -Input +/- Vref
            // Set differential input mode
            SetBuffer(rc.BUF_DIFF_MODE);

            // Set reference mux to 1.024V reference
            DsmRef0Reg &= rc.DSM_REFMUX_MASK;
            DsmRef0Reg |= rc.DSM_REFMUX_1_024V;

            // Set modulator gain input caps to a gain of 1

            // disconnect references from -Input
            DsmSw3Reg = 0x00;

            break;

        case rc.ADC_IR_VNEG_2VREF_DIFF:  // Diff, -Input +/- 2*Vref
            // Set differential input mode
            SetBuffer(rc.BUF_DIFF_MODE);

            // Set reference mux to 1.024V reference
            DsmRef0Reg &= rc.DSM_REFMUX_MASK;
            DsmRef0Reg |= rc.DSM_REFMUX_1_024V;

            // Set modulator gain input caps to a gain of 1/2
            if (ADC_Resolution > 15)
            {
                DsmCr5Reg = rc.DSM_IPCAP1_1600FF | rc.DSM_IPCAP1_400FF | rc.DSM_IPCAP1_200FF;
                DsmCr6Reg = rc.DSM_DACCAP_3200FF | rc.DSM_DACCAP_800FF | rc.DSM_DACCAP_400FF;
            }
            else
            {
                DsmCr5Reg = rc.DSM_IPCAP1_800FF | rc.DSM_IPCAP1_200FF | rc.DSM_IPCAP1_100FF;
                DsmCr6Reg = rc.DSM_DACCAP_1600FF | rc.DSM_DACCAP_400FF | rc.DSM_DACCAP_192FF;
            }

            // disconnect references from -Input
            DsmSw3Reg = 0x00;

            break;

        case rc.ADC_IR_VNEG_VREF_2_DIFF:  // Diff, -Input +/- Vref/2
            // Set differential input mode
            SetBuffer(rc.BUF_DIFF_MODE);

            // Set reference mux to 1.024V reference
            DsmRef0Reg &= rc.DSM_REFMUX_MASK;
            DsmRef0Reg |= rc.DSM_REFMUX_1_024V;

            // Set modulator gain input caps to a gain of 1/2
            if (ADC_Resolution > 15)
            {
                DsmCr5Reg = rc.DSM_IPCAP1_3200FF | rc.DSM_IPCAP1_800FF | rc.DSM_IPCAP1_400FF;
                DsmCr6Reg = rc.DSM_DACCAP_1600FF | rc.DSM_DACCAP_400FF | rc.DSM_DACCAP_192FF;
            }
            else
            {
                DsmCr5Reg = rc.DSM_IPCAP1_1600FF | rc.DSM_IPCAP1_400FF | rc.DSM_IPCAP1_200FF;
                DsmCr6Reg = rc.DSM_DACCAP_800FF | rc.DSM_DACCAP_192FF | rc.DSM_DACCAP_96FF;
            }

            // disconnect references from -Input
            DsmSw3Reg = 0x00;
            break;


        case rc.ADC_IR_VNEG_VREF_4_DIFF:  // Diff, -Input +/- Vref/4
            // Set differential input mode
            SetBuffer(rc.BUF_DIFF_MODE);

            // Set reference mux to 1.024V reference
            DsmRef0Reg &= rc.DSM_REFMUX_MASK;
            DsmRef0Reg |= rc.DSM_REFMUX_1_024V;

            // Set modulator gain input caps to a gain of 1/2
            if (ADC_Resolution > 15)
            {
                DsmCr5Reg = rc.DSM_IPCAP1_6400FF | rc.DSM_IPCAP1_1600FF | rc.DSM_IPCAP1_800FF;
                DsmCr6Reg = rc.DSM_DACCAP_1600FF | rc.DSM_DACCAP_400FF | rc.DSM_DACCAP_192FF;
            }
            else
            {
                DsmCr5Reg = rc.DSM_IPCAP1_3200FF | rc.DSM_IPCAP1_800FF | rc.DSM_IPCAP1_400FF;
                DsmCr6Reg = rc.DSM_DACCAP_800FF | rc.DSM_DACCAP_192FF | rc.DSM_DACCAP_96FF;
            }

            // disconnect references from -Input
            DsmSw3Reg = 0x00;
            break;


        default:
            break;
    }
}

//*******************************************************************************
// Function Name: SetBuffer
//********************************************************************************
// Summary:
//  Sets operatonal mode of the Modulator input buffer.
//  Buffer modes   
//     Bypass P input => Bypass P input amp  ( Default, not bypassed )
//     Bypass N input => Bypass N input amp  ( Default, not bypassed )
//     Rail-To-Rail   => Enable mode for inputs to swiing rail to rail ( Default, not rail-to-rail)
//     Enable Filter  => Enable output R/C filter (Default, no filter )
//     Low Power      => Enable low power mode (Defalt, normal power mode )
//
// Parameters:  
//  bufMode:  Buffer mode value
//
// Return: 
//  (void) 
//
// Theory: 
//
// Side Effects:
//
//*******************************************************************************
void SetBuffer(uint bufMode)
{

    uint tmp = 0;

    /* Setup BUF0 Register */
    /* Check for positive buffer being bypassed */
    if ((bufMode & rc.BUF_BYPASS_POS) > 0)
    {
        tmp |= rc.DSM_BYPASS_P;
    }
    else /* POS buffer not bypassed, turn on buffer */
    {
        tmp |= rc.DSM_ENABLE_P;
    }

    /* Check for rail to rail operation */
    if ((bufMode & rc.BUF_RAIL_TO_RAIL) > 0)
    {
        tmp |= rc.DSM_RAIL_RAIL_EN;
    }

    DsmBuf0Reg = tmp;   /* Set Buf0 Register */


    /* Setup BUF1 Register */
    /* Check for negative buffer being bypassed, but don't mess with gain. */
    tmp = DsmBuf1Reg & rc.DSM_GAIN_MASK;
    if ((bufMode & rc.BUF_BYPASS_NEG) > 0)
    {
        tmp |= rc.DSM_BYPASS_N;
    }
    else /* NEG buffer not bypassed, turn on buffer */
    {
        tmp |= rc.DSM_ENABLE_N;
    }
    DsmBuf1Reg = tmp;   /* Set Buf1 Register */



    /* Setup BUF2 Register */
    tmp = 0;
    /* Check for Low Power */
    if ((bufMode & rc.BUF_LOW_PWR)> 0)
    {
        tmp |= rc.DSM_ENABLE_N;
    }

    /* Check for Filer enable */
    if ((bufMode & rc.BUF_FILTER_EN)> 0)
    {
        tmp |= rc.DSM_ADD_EXTRA_RC;
    }
    DsmBuf2Reg = tmp;   /* Set Buf2 Register */

    // If both the positive and negative buffers are enabled, enable chopping.
    if (((DsmBuf0Reg & rc.DSM_ENABLE_P) != 0) & ((DsmBuf1Reg & rc.DSM_ENABLE_N) != 0 ))
    {
        DsmBuf3Reg = rc.DSM_BUF_CHOP_EN | rc.DSM_BUF_FCHOP_FS8;
    }
    else
    {
        DsmBuf3Reg = 0x00;
    }

}

//******************************************************************************
// Function Name: SetPower
//******************************************************************************
// Summary:
//  Sets power mode of ADC
//
// Parameters:  
//  power:  Power setting for ADC
//
// Return: 
//  (void) 
//
// Theory: 
//
// Side Effects:
//
//*****************************************************************************
void SetPower(uint power)
{
    uint tmpReg;

    /* mask off invalid power settings */
    power &= 0x07;

    /* Set Power1 parameter  */
    tmpReg = DsmCr14Reg & ~rc.DSM_POWER1_MASK;
    DsmCr14Reg = tmpReg | power;


    /* Set Power2_3 parameter  */
    tmpReg = DsmCr15Reg & ~rc.DSM_POWER2_3_MASK;
    DsmCr15Reg = tmpReg | (power << 4) | power;

    /* Set Power_sum parameter  */
    tmpReg = DsmCr16Reg & ~rc.DSM_POWER_SUM_MASK;
    DsmCr16Reg = tmpReg | (power << 4) | rc.DSM_CP_ENABLE;
}

//******************************************************************************
// Function Name: SetClock
//******************************************************************************
// Summary:
//  Calculates the frequency required for the given resolution, sample rate,
//  and conversion mode.
//
// Parameters:  
//  power:  
//
// Return: 
//  Desired clock frequency.
//
// Theory: 
//
// Side Effects:
//
//*****************************************************************************
static public uint SetClock(uint resolution, uint sampleRate, uint conversionMode)
{
    uint clockFrequency;
    uint clocksPerSample;

    clocksPerSample = ClocksPerSample(resolution, conversionMode);

    clockFrequency = clocksPerSample * sampleRate;
  
    return clockFrequency;
}

//******************************************************************************
// Function Name: ClocksPerSample
//******************************************************************************
// Summary:
//  Calculates how may ADC clocks are required per sample
//
// Parameters:  
//  resolution: Resolution between 8 and 20
//  conversionMode: Mode of operation
//
// Return: 
//  Clocks required for a single sample.
//
// Theory: 
//
// Side Effects:
//
//*****************************************************************************
static public uint ClocksPerSample(uint resolution, uint conversionMode)
{
    uint resIndex;
    uint clocksPerSample;

    // Make sure resolution in not out of range.
    if (resolution < rc.MIN_RESOLUTION)
    {
        resolution = rc.MIN_RESOLUTION;
    }

    if (resolution > rc.MAX_RESOLUTION)
    {
        resolution = rc.MAX_RESOLUTION;
    }

    resIndex = resolution - rc.MIN_RESOLUTION;
    switch (conversionMode)
    {
        case rc.ADC_CM_SINGLE_SAMPLE:
            clocksPerSample = (((resSettings.dr1[resIndex] + 1) * 4) + 3) * (resSettings.dr2[resIndex] + 1);
            break;

        case rc.ADC_CM_FAST_FILER:
            clocksPerSample = (((resSettings.dr1[resIndex] + 1) * 4) + 3) * (resSettings.dr2[resIndex] + 1);
            break;

        case rc.ADC_CM_CONTINUOUS:
            clocksPerSample = (resSettings.dr1[resIndex] + 1) * (resSettings.dr2[resIndex] + 1);
            break;

        case rc.ADC_CM_FAST_FIR:
            clocksPerSample = (((resSettings.dr2[resIndex] + 1) + 3) * (resSettings.dr1[resIndex] + 1)) + 6;
            break;

        default:
            clocksPerSample = (((resSettings.dr2[resIndex] + 1) + 3) * (resSettings.dr1[resIndex] + 1)) + 4;
            break;
    };

    return clocksPerSample;
}

//******************************************************************************
// Function Name: CalcCountsPerVolt
//******************************************************************************
// Summary:
//  Calculates the clocks per volt scale factor for the voltage conversion functions.
//
// Parameters:  
// resolution: Resolution between 8 and 20 
//
// Return: 
//  Counts per Volt
//
// Theory: 
//
// Side Effects:
//
//*****************************************************************************
static public uint CalcCountsPerVolt(uint resolution, uint inputRange, float refVoltage)
{

    float countsPerVolt;
    uint counts = 1;
    uint i;

    // Raise counts to the 2^resolution
    for(i=0; i < resolution; i++)
    {
        counts *= 2;
    }


    switch (inputRange)
    {
        case rc.ADC_IR_VSSA_TO_VREF:  // Vss to Vref
            countsPerVolt = counts / refVoltage; 
            break;

        case rc.ADC_IR_VSSA_TO_2VREF:   // Vss to 2*Vref
            countsPerVolt = counts / (refVoltage * 2); 
            break;
       

        case rc.ADC_IR_VSSA_TO_VDDA:   // Vss to Vdd
            countsPerVolt = counts / refVoltage; 
            break;

        case rc.ADC_IR_VNEG_VREF_DIFF:   // -Vin +/- Vref (Diff)
            countsPerVolt = counts / ( refVoltage * 2); 
            break;

        case rc.ADC_IR_VNEG_2VREF_DIFF:  // -Vin +/- 2*Vref (Diff)
            countsPerVolt = counts / ( refVoltage * 4); 
            break;

        case rc.ADC_IR_VNEG_VREF_2_DIFF:  // -Vin +/- 0.5Vref (Diff)
            countsPerVolt = (counts * 2) / (refVoltage * 2); 
            break;

        case rc.ADC_IR_VNEG_VREF_4_DIFF:  // -Vin +/- 0.25Vref (Diff)
            countsPerVolt = (counts * 4) / (refVoltage * 2); 
            break;

        default:
            countsPerVolt = (counts*2) / refVoltage;
            break;
    };

    return Convert.ToUInt32(countsPerVolt);
}

//******************************************************************************
// Function Name: CalcValues
//******************************************************************************
// Summary:
//  Calculate all register values.
//
// Parameters:  
//  (void)
//
// Return: 
//  (void) 
//
// Theory: 
//
// Side Effects:
//
//*****************************************************************************
     void CalcValues(ref Dictionary<string, string> paramDict )
        {
            uint clockFrequency;
            uint clocksPerCycle;
            string tmpString = "";
            List<CyClockData> clkdata = m_termQuery.GetClockData("myclock", 0);
            ICyDesignQuery_v1 desquery = m_instQuery.DesignQuery;
 

            // Read all the paramters, since register values are based on them.
            paramDict.TryGetValue(ADC_ClockParamName, out tmpString);
            ADC_Clock = uint.Parse(tmpString);

            paramDict.TryGetValue(ADC_Input_RangeParamName, out tmpString);
            ADC_Input_Range = uint.Parse(tmpString);

            paramDict.TryGetValue(ADC_PowerParamName, out tmpString);
            ADC_Power = uint.Parse(tmpString);

            paramDict.TryGetValue(ADC_ReferenceParamName, out tmpString);
            ADC_Reference = uint.Parse(tmpString);

            paramDict.TryGetValue(ADC_ResolutionParamName, out tmpString);
            ADC_Resolution = uint.Parse(tmpString);

            paramDict.TryGetValue(Conversion_ModeParamName, out tmpString);
            Conversion_Mode = uint.Parse(tmpString);

            paramDict.TryGetValue(Input_Buffer_GainParamName, out tmpString);
            Input_Buffer_Gain = uint.Parse(tmpString);

            paramDict.TryGetValue(Sample_RateParamName, out tmpString);
            Sample_Rate = uint.Parse(tmpString);

            paramDict.TryGetValue(Ref_VoltageParamName, out tmpString);
            Ref_Voltage = float.Parse(tmpString);

            paramDict.TryGetValue(Start_of_ConversionParamName, out tmpString);
            Start_of_Conversion = uint.Parse(tmpString);


            // First, setup the default settings.
            //   DecCrReg    See below
            DecSrReg    = rc.DEC_INTR_PULSE | rc.DEC_INTR_CLEAR;
            DecOcorReg  = 0x00;
            DecOcormReg = 0x00;
            DecOcorhReg = 0x00;
            DecGcorReg  = 0x00;
            DecGcorhReg = 0x00;
            DecGvalReg  = 0x00;
            DecCoherReg = rc.DEC_SAMP_KEY_LOW;     

            DsmCr0Reg  = rc.DSM_QLEV_9 | rc.DSM_NONOV_HIGH;
            DsmCr1Reg  = 0x00;
            // Chop freq must be higher than Decimation rate. (Divider lower) 
            DsmCr2Reg  = (rc.DSM_MOD_CHOP_EN | rc.DSM_FCHOP_DIV8) | rc.DSM_RESET1_EN | rc.DSM_RESET2_EN | rc.DSM_RESET3_EN;
            DsmCr3Reg  = 0x00;
            DsmCr7Reg  = 0x00;
            DsmCr13Reg = 0x00;
            DsmRef0Reg = rc.DSM_VCMSEL_0_8V | rc.DSM_REFMUX_1_024V; 
            DsmRef1Reg = 0x00;
            DsmRef2Reg = 0x00;
            DsmRef3Reg = 0x00;
            DsmDem0Reg = rc.DSM_EN_SCRAMBLER0 | rc.DSM_EN_SCRAMBLER1 | rc.DSM_EN_DEM;
            DsmDem1Reg = 0x00;
            DsmBuf0Reg = rc.DSM_ENABLE_P | rc.DSM_RAIL_RAIL_EN;
            DsmBuf1Reg = rc.DSM_ENABLE_N | (Input_Buffer_Gain << 2);
            DsmBuf2Reg = 0x00;
            DsmBuf3Reg = 0x00;
            DsmMiscReg = rc.DSM_SEL_ICLK_CP;
            DsmClkReg |= (rc.DSM_CLK_BYPASS_SYNC | rc.DSM_CLK_CLK_EN);
            DsmOut0Reg = 0;
            DsmOut1Reg = 0;


            // Setup cap settings.
            if (ADC_Resolution > 15)
            {
                DsmCr4Reg  = rc.DSM_FCAP1_1600FF   | rc.DSM_FCAP1_6400FF;
                DsmCr5Reg  = rc.DSM_IPCAP1_3200FF  | rc.DSM_IPCAP1_800FF | rc.DSM_IPCAP1_400FF;
                DsmCr6Reg  = rc.DSM_DACCAP_3200FF  | rc.DSM_DACCAP_800FF | rc.DSM_DACCAP_400FF;
                DsmCr8Reg  = rc.DSM_IPCAP2_250_FF  | rc.DSM_FCAP2_550_FF;
                DsmCr9Reg  = rc.DSM_IPCAP3_250_FF  | rc.DSM_FCAP3_700_FF;
                DsmCr10Reg = rc.DSM_SUMCAP1_250_FF | rc.DSM_SUMCAP2_250_FF;
                DsmCr11Reg = rc.DSM_SUMCAP3_250_FF | rc.DSM_SUMCAPFB_500_FF;
                DsmCr12Reg = rc.DSM_SUMCAPIN_250_FF;

                /* Power settings */
                DsmCr14Reg = rc.DSM_POWER1_2 | rc.DSM_OPAMP1_BW_0;
                DsmCr15Reg = rc.DSM_POWER_12MHZ | rc.DSM_POWER2_3_HIGH;
                DsmCr16Reg = rc.DSM_POWER_SUM_HIGH | rc.DSM_CP_PWRCTL_DEFAULT;
                DsmCr17Reg = rc.DSM_EN_BUF_VCM | rc.DSM_PWR_CTRL_VREF_2 | rc.DSM_PWR_CTRL_VCM_2 | rc.DSM_PWR_CTRL_VREF_INN_2;
            }
            else
            {
                DsmCr4Reg  = rc.DSM_FCAP1_1600FF  | rc.DSM_FCAP1_400FF;
                DsmCr5Reg  = rc.DSM_IPCAP1_800FF  | rc.DSM_IPCAP1_200FF | rc.DSM_IPCAP1_100FF;
                DsmCr6Reg  = rc.DSM_DACCAP_800FF  | rc.DSM_DACCAP_192FF | rc.DSM_DACCAP_96FF;
                DsmCr8Reg  = rc.DSM_IPCAP2_50_FF  | rc.DSM_FCAP2_50_FF;
                DsmCr9Reg  = rc.DSM_IPCAP3_50_FF  | rc.DSM_FCAP3_150_FF;
                DsmCr10Reg = rc.DSM_SUMCAP1_50_FF | rc.DSM_SUMCAP2_50_FF;
                DsmCr11Reg = rc.DSM_SUMCAP3_50_FF | rc.DSM_SUMCAPFB_100_FF;
                DsmCr12Reg = rc.DSM_SUMCAPIN_50_FF;

                /* Power settings */
                DsmCr14Reg = rc.DSM_POWER1_2 | rc.DSM_OPAMP1_BW_0;
                DsmCr15Reg = rc.DSM_POWER_12MHZ | rc.DSM_POWER2_3_HIGH;
                DsmCr16Reg = rc.DSM_POWER_SUM_HIGH | rc.DSM_CP_PWRCTL_DEFAULT;
                //DsmCr17Reg = 0xAB;  // TODO:  Look into this issue
                DsmCr17Reg = rc.DSM_EN_BUF_VCM | rc.DSM_PWR_CTRL_VREF_2 | rc.DSM_PWR_CTRL_VCM_2 | rc.DSM_PWR_CTRL_VREF_INN_2;
            }
            DecCoherReg = rc.DEC_SAMP_KEY_LOW;

            SetResolution(ADC_Resolution);
            SetBufferGain(Input_Buffer_Gain);
            SetRef(ADC_Reference, ADC_Input_Range);
            SetInputRange(ADC_Input_Range);
            clockFrequency = SetClock(ADC_Resolution, Sample_Rate, Conversion_Mode);
            SetPower(ADC_Power);

            // If the second decimator is not used, don't turn on the FIR filter
            if ((DecDr2Reg == 0) && (DecDr2hReg == 0))
            {
                DecCrReg = (Conversion_Mode << 2) | (Start_of_Conversion << 1) | rc.DEC_OCOR_EN;
            }
            else
            {
                DecCrReg = (Conversion_Mode << 2) | (Start_of_Conversion << 1) | rc.DEC_FIR_EN | rc.DEC_OCOR_EN;
            }

            // Write out the register constants
            paramDict.Add(DEC_CR_ParamName,     "0x" + DecCrReg.ToString("X2") + "u");
            paramDict.Add(DEC_SR_ParamName,     "0x" + DecSrReg.ToString("X2") + "u");
            paramDict.Add(DEC_SHIFT1_ParamName, "0x" + DecShift1Reg.ToString("X2") + "u");
            paramDict.Add(DEC_SHIFT2_ParamName, "0x" + DecShift2Reg.ToString("X2") + "u");
            paramDict.Add(DEC_DR2_ParamName,    "0x" + DecDr2Reg.ToString("X2") + "u");
            paramDict.Add(DEC_DR2H_ParamName,   "0x" + DecDr2hReg.ToString("X2") + "u");
            paramDict.Add(DEC_DR1_ParamName,    "0x" + DecDr1Reg.ToString("X2") + "u");

            paramDict.Add(DEC_OCOR_ParamName,  "0x" + DecOcorReg.ToString("X2") + "u");
            paramDict.Add(DEC_OCORM_ParamName, "0x" + DecOcormReg.ToString("X2") + "u");
            paramDict.Add(DEC_OCORH_ParamName, "0x" + DecOcorhReg.ToString("X2") + "u");

            paramDict.Add(DEC_GVAL_ParamName,  "0x" + DecGvalReg.ToString("X2") + "u");
            paramDict.Add(DEC_GCOR_ParamName,  "0x" + DecGcorReg.ToString("X2") + "u");
            paramDict.Add(DEC_GCORH_ParamName, "0x" + DecGcorhReg.ToString("X2") + "u");
            paramDict.Add(DEC_COHER_ParamName, "0x" + DecCoherReg.ToString("X2") + "u");

            paramDict.Add(DSM_CR0_ParamName, "0x" + DsmCr0Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR1_ParamName, "0x" + DsmCr1Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR2_ParamName, "0x" + DsmCr2Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR3_ParamName, "0x" + DsmCr3Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR4_ParamName, "0x" + DsmCr4Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR5_ParamName, "0x" + DsmCr5Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR6_ParamName, "0x" + DsmCr6Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR7_ParamName, "0x" + DsmCr7Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR8_ParamName, "0x" + DsmCr8Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR9_ParamName, "0x" + DsmCr9Reg.ToString("X2") + "u");

            paramDict.Add(DSM_CR10_ParamName, "0x" + DsmCr10Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR11_ParamName, "0x" + DsmCr11Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR12_ParamName, "0x" + DsmCr12Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR13_ParamName, "0x" + DsmCr13Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR14_ParamName, "0x" + DsmCr14Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR15_ParamName, "0x" + DsmCr15Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR16_ParamName, "0x" + DsmCr16Reg.ToString("X2") + "u");
            paramDict.Add(DSM_CR17_ParamName, "0x" + DsmCr17Reg.ToString("X2") + "u");

            paramDict.Add(DSM_REF0_ParamName, "0x" + DsmRef0Reg.ToString("X2") + "u");
            paramDict.Add(DSM_REF1_ParamName, "0x" + DsmRef1Reg.ToString("X2") + "u");
            paramDict.Add(DSM_REF2_ParamName, "0x" + DsmRef2Reg.ToString("X2") + "u");
            paramDict.Add(DSM_REF3_ParamName, "0x" + DsmRef3Reg.ToString("X2") + "u");

            paramDict.Add(DSM_DEM0_ParamName, "0x" + DsmDem0Reg.ToString("X2") + "u");
            paramDict.Add(DSM_DEM1_ParamName, "0x" + DsmDem1Reg.ToString("X2") + "u");
            paramDict.Add(DSM_MISC_ParamName, "0x" + DsmMiscReg.ToString("X2") + "u");
            paramDict.Add(DSM_CLK_ParamName,  "0x" + DsmClkReg.ToString("X2")  + "u");


            paramDict.Add(DSM_BUF0_ParamName, "0x" + DsmBuf0Reg.ToString("X2") + "u");
            paramDict.Add(DSM_BUF1_ParamName, "0x" + DsmBuf1Reg.ToString("X2") + "u");
            paramDict.Add(DSM_BUF2_ParamName, "0x" + DsmBuf2Reg.ToString("X2") + "u");
            paramDict.Add(DSM_BUF3_ParamName, "0x" + DsmBuf3Reg.ToString("X2") + "u");
            paramDict.Add(DSM_OUT0_ParamName, "0x" + DsmOut0Reg.ToString("X2") + "u");
            paramDict.Add(DSM_OUT1_ParamName, "0x" + DsmOut1Reg.ToString("X2") + "u");

            paramDict.Add(DSM_SW3_ParamName, "0x" + DsmSw3Reg.ToString("X2") + "u");

            // Voltage conversion defaults.
            paramDict.Add(RefVoltage_ParamName, Ref_Voltage.ToString("0.0000"));
            CountsPerVolt = CalcCountsPerVolt(ADC_Resolution, ADC_Input_Range, Ref_Voltage);
            paramDict.Add(CountsPerVoltParamName, CountsPerVolt.ToString());

            paramDict.Add(clockFrequency_ParamName, clockFrequency.ToString());
            clocksPerCycle = ClocksPerSample(ADC_Resolution, Conversion_Mode);
            paramDict.Add(clocksPerSample_ParamName, "0x" + clocksPerCycle.ToString("X4") + "u");

        }
    }

    public static class resSettings
    {
            //                       Resolution    8    9   10   11   12   13    14   15    16    17    18    19    20     21
            public static uint[] dr1 =          { 15,  15,  15,  31,  31,  31,   31,  63,   63,   63,   63,  127,  127,   127 };
            public static uint[] dr2 =          {  0,   0,   0,   0,   0,   0,    0,   0,    0,    3,   15,   63,  127,   127 };
            public static uint[] dr2h =         {  0,   0,   0,   0,   0,   0,    0,   0,    0,    0,    0,    0,    0,     0 };
            public static uint[] shift1 =       {  4,   4,   4,   4,   6,   6,    6,   4,    4,    4,    4,    0,    0,     0 };
            public static uint[] shift2 =       {  7,   6,   5,   8,   9,   8,    7,   8,    7,    8,    9,   10,   10,     9 };     
    }

    // ADC Decimator and Modulator constants
    public static class rc
    {

        public const uint MIN_RESOLUTION = 8;
        public const uint MAX_RESOLUTION = 20;

        public const uint MAX_FREQ_15_20_BITS = 3072000;
        public const uint MAX_FREQ_8_14_BITS = 6144000;

        public const uint MIN_FREQ_15_20_BITS = 128000;
        public const uint MIN_FREQ_8_14_BITS = 128000;

        //*************************************
        //  ADC_Clock Constants
        //*************************************
        public const uint ADC_CLOCK_INTERNAL = 0;
        public const uint ADC_CLOCK_EXTERNAL = 1;

        //*************************************
        //  ADC_Input_Range Constants
        //*************************************
        public const uint ADC_IR_VSSA_TO_VREF    = 0;
        public const uint ADC_IR_VSSA_TO_2VREF   = 1;
        public const uint ADC_IR_VSSA_TO_VDDA    = 2;
        public const uint ADC_IR_VNEG_VREF_DIFF  = 3;
        public const uint ADC_IR_VNEG_2VREF_DIFF = 4;
        public const uint ADC_IR_VNEG_VREF_2_DIFF = 5;
        public const uint ADC_IR_VNEG_VREF_4_DIFF = 6;

        //*************************************
        //  ADC_Power Constants
        //*************************************
        public const uint ADC_LOWPOWER  = 0;
        public const uint ADC_MEDPOWER  = 1;
        public const uint ADC_HIGHPOWER = 2;


        //*************************************
        //  ADC_Reference Constants
        //*************************************
        public const uint ADC_INT_REF_NOT_BYPASSED    = 0;
        public const uint ADC_INT_REF_BYPASSED_ON_P03 = 1;
        public const uint ADC_INT_REF_BYPASSED_ON_P32 = 2;
        public const uint ADC_EXT_REF_ON_P03          = 3;
        public const uint ADC_EXT_REF_ON_P32          = 4;

        //*************************************
        //  Conversion_Mode Constants
        //*************************************
        public const uint ADC_CM_SINGLE_SAMPLE = 0;
        public const uint ADC_CM_FAST_FILER    = 1;
        public const uint ADC_CM_CONTINUOUS    = 2;
        public const uint ADC_CM_FAST_FIR      = 3;

        //*************************************
        //  Input_Buffer_Gain Constants
        //*************************************
        public const uint ADC_IBG_1X = 0;
        public const uint ADC_IBG_2X = 1;
        public const uint ADC_IBG_4X = 2;
        public const uint ADC_IBG_8X = 3;

        //*************************************
        //  Start_of_Conversion Constants
        //*************************************
        public const uint ADC_SOC_SOFTWARE = 0;
        public const uint ADC_SOC_HARDWARE = 1;

        //*************************************
        //  Constants for SetInputRange() bufmode parameter
        //*************************************
        public const uint BUF_DIFF_MODE    = 0x00;  // Differential mode 
        public const uint BUF_BYPASS_POS   = 0x04;  // Bypass and power down positive channel 
        public const uint BUF_BYPASS_NEG   = 0x08;  // Bypass and power down negative channel 
        public const uint BUF_RAIL_TO_RAIL = 0x10;  // Enables Rail-to-rail mode 
        public const uint BUF_FILTER_EN    = 0x20;  // Output filter enable 
        public const uint BUF_LOW_PWR      = 0x40;  // Enable  Low power mode 
        
        
        //*************************************
        //      Register Constants        
        //**************************************


        //*****************************************
        // DSM.CR0 Control Register 0 definitions 
        //*****************************************

        // Bit Field  DSM_NONOV                   
        public const uint DSM_NONOV_MASK  = 0x0C;
        public const uint DSM_NONOV_LOW   = 0x00;
        public const uint DSM_NONOV_MED   = 0x04;
        public const uint DSM_NONOV_HIGH  = 0x08;
        public const uint DSM_NONOV_VHIGH = 0x0C;

        // Bit Field  DSM_QLEV                    
        public const uint DSM_QLEV_MASK = 0x03;
        public const uint DSM_QLEV_2  = 0x00;
        public const uint DSM_QLEV_3  = 0x01;
        public const uint DSM_QLEV_9  = 0x02;
        public const uint DSM_QLEV_9x = 0x03;


        //**************************************** 
        // DSM.CR1 Control Register 1 definitions  
        //**************************************** 
        public const uint DSM_ODET_TH_MASK = 0x1F;
        public const uint DSM_ODEN = 0x20;
        public const uint DSM_DPMODE = 0x40;

        //**************************************** 
        // DSM.CR2 Control Register 2 definitions  
        //**************************************** 
        // Bit Field  DSM_FCHOP                    
        public const uint DSM_FCHOP_MASK   = 0x07;
        public const uint DSM_FCHOP_DIV2   = 0x00;
        public const uint DSM_FCHOP_DIV4   = 0x01;
        public const uint DSM_FCHOP_DIV8   = 0x02;
        public const uint DSM_FCHOP_DIV16  = 0x03;
        public const uint DSM_FCHOP_DIV32  = 0x04;
        public const uint DSM_FCHOP_DIV64  = 0x05;
        public const uint DSM_FCHOP_DIV128 = 0x06;
        public const uint DSM_FCHOP_DIV256 = 0x07;

        // Bit Field  DSM_MOD_CHOP_EN                 
        public const uint DSM_MOD_CHOP_EN = 0x08;

        // Bit Field  DSM_MX_RESET                    
        public const uint DSM_MX_RESET = 0x80;

        // Bit Field  DSM_RESET1_EN                   
        public const uint DSM_RESET1_EN = 0x10;

        // Bit Field  DSM_RESET2_EN                   
        public const uint DSM_RESET2_EN = 0x20;

        // Bit Field  DSM_RESET3_EN                   
        public const uint DSM_RESET3_EN = 0x40;

        //**************************************** 
        // DSM.CR3 Control Register 3 definitions  
        //**************************************** 
        // Bit Field  DSM_SELECT_MOD_BIT           
        public const uint DSM_MODBITIN_MASK = 0x0F;
        public const uint DSM_MODBITIN_LUT0 = 0x00;
        public const uint DSM_MODBITIN_LUT1 = 0x01;
        public const uint DSM_MODBITIN_LUT2 = 0x02;
        public const uint DSM_MODBITIN_LUT3 = 0x03;
        public const uint DSM_MODBITIN_LUT4 = 0x04;
        public const uint DSM_MODBITIN_LUT5 = 0x05;
        public const uint DSM_MODBITIN_LUT6 = 0x06;
        public const uint DSM_MODBITIN_LUT7 = 0x07;
        public const uint DSM_MODBITIN_UDB  = 0x08;

        // Bit Field  DSM_MODBIT_EN                  
        public const uint DSM_MODBIT_EN = 0x10;

        // Bit Field  DSM_MX_DOUT                    
        public const uint DSM_MX_DOUT_8BIT   = 0x00;
        public const uint DSM_MX_DOUT_2SCOMP = 0x20;

        // Bit Field  DSM_MODBIT  TBD                
        public const uint DSM_MODBIT = 0x40;

        // Bit Field  DSM_SIGN                       
        public const uint DSM_SIGN_NINV = 0x00;
        public const uint DSM_SIGN_INV  = 0x80;


        //**************************************** 
        // DSM.CR4 Control Register 4 definitions  
        //**************************************** 
        // Bit Field  DSM_SELECT_FCAP_EN             
        public const uint DSM_FCAP1_EN  = 0x80;
        public const uint DSM_FCAP1_DIS = 0x00;

        // Bit Field  DSM_SELECT_FCAP1              
        public const uint DSM_FCAP1_MASK   = 0x7F;
        public const uint DSM_FCAP1_MIN    = 0x00;
        public const uint DSM_FCAP1_MAX    = 0x7F;
        public const uint DSM_FCAP1_100FF  = 0x01;
        public const uint DSM_FCAP1_200FF  = 0x02;
        public const uint DSM_FCAP1_400FF  = 0x04;
        public const uint DSM_FCAP1_800FF  = 0x08;
        public const uint DSM_FCAP1_1600FF = 0x10;
        public const uint DSM_FCAP1_3200FF = 0x20;
        public const uint DSM_FCAP1_6400FF = 0x40;

        //**************************************** 
        // DSM.CR5 Control Register 5 definitions  
        //**************************************** 
        // Bit Field  DSM_SELECT_IPCAP_EN             
        public const uint DSM_IPCAP1_EN = 0x80;
        public const uint DSM_IPCAP1_DIS = 0x00;

        // Bit Field  DSM_SELECT_IPCAP1              
        public const uint DSM_IPCAP1_MASK   = 0x7F;
        public const uint DSM_IPCAP1_MIN    = 0x00;
        public const uint DSM_IPCAP1_MAX    = 0x7F;
        public const uint DSM_IPCAP1_100FF  = 0x01;
        public const uint DSM_IPCAP1_200FF  = 0x02;
        public const uint DSM_IPCAP1_400FF  = 0x04;
        public const uint DSM_IPCAP1_800FF  = 0x08;
        public const uint DSM_IPCAP1_1600FF = 0x10;
        public const uint DSM_IPCAP1_3200FF = 0x20;
        public const uint DSM_IPCAP1_6400FF = 0x40;


        //**************************************** 
        // DSM.CR6 Control Register 6 definitions  
        //**************************************** 
        // Bit Field  DSM_SELECT_DACCAP_EN             
        public const uint DSM_DACCAP_EN  = 0x40;
        public const uint DSM_DACCAP_DIS = 0x00;

        // Bit Field  DSM_SELECT_DACCAP              
        public const uint DSM_DACCAP_MASK   = 0x3F;
        public const uint DSM_DACCAP_MIN    = 0x00;
        public const uint DSM_DACCAP_MAX    = 0x3F;
        public const uint DSM_DACCAP_96FF   = 0x01;
        public const uint DSM_DACCAP_192FF  = 0x02;
        public const uint DSM_DACCAP_400FF  = 0x04;
        public const uint DSM_DACCAP_800FF  = 0x08;
        public const uint DSM_DACCAP_1600FF = 0x10;
        public const uint DSM_DACCAP_3200FF = 0x20;


        //**************************************** 
        // DSM.CR7 Control Register 7 definitions  
        //**************************************** 
        // Bit Field  DSM_SELECT_RESCAP_EN             
        public const uint DSM_RESCAP_EN = 0x08;
        public const uint DSM_RESCAP_DIS = 0x00;

        // Bit Field  DSM_SELECT_RESCAP              
        public const uint DSM_RESCAP_MASK = 0x07;
        public const uint DSM_RESCAP_MIN  = 0x00;
        public const uint DSM_RESCAP_MAX  = 0x07;
        public const uint DSM_RESCAP_12FF = 0x00;
        public const uint DSM_RESCAP_24FF = 0x01;
        public const uint DSM_RESCAP_36FF = 0x02;
        public const uint DSM_RESCAP_48FF = 0x03;
        public const uint DSM_RESCAP_60FF = 0x04;
        public const uint DSM_RESCAP_72FF = 0x05;
        public const uint DSM_RESCAP_84FF = 0x06;
        public const uint DSM_RESCAP_96FF = 0x07;

        public const uint DSM_FCAP2_DIS = 0x00;
        public const uint DSM_FCAP2_EN  = 0x80;

        public const uint DSM_FCAP3_DIS = 0x00;
        public const uint DSM_FCAP3_EN  = 0x40;

        public const uint DSM_IPCAP1OFFSET = 0x20;
        public const uint DSM_FPCAP1OFFSET = 0x10;


        //**************************************** 
        // DSM.CR8 Control Register 8 definitions  
        //**************************************** 
        public const uint DSM_IPCAP2_EN = 0x80;

        public const uint DSM_IPCAP2_MASK   = 0x70;
        public const uint DSM_IPCAP2_0_FF   = 0x00;
        public const uint DSM_IPCAP2_50_FF  = 0x10;
        public const uint DSM_IPCAP2_100_FF = 0x20;
        public const uint DSM_IPCAP2_150_FF = 0x30;
        public const uint DSM_IPCAP2_200_FF = 0x40;
        public const uint DSM_IPCAP2_250_FF = 0x50;
        public const uint DSM_IPCAP2_300_FF = 0x60;
        public const uint DSM_IPCAP2_350_FF = 0x70;

        public const uint DSM_FCAP2_MASK   = 0x0F;
        public const uint DSM_FCAP2_0_FF   = 0x00;
        public const uint DSM_FCAP2_50_FF  = 0x01;
        public const uint DSM_FCAP2_100_FF = 0x02;
        public const uint DSM_FCAP2_150_FF = 0x03;
        public const uint DSM_FCAP2_200_FF = 0x04;
        public const uint DSM_FCAP2_250_FF = 0x05;
        public const uint DSM_FCAP2_300_FF = 0x06;
        public const uint DSM_FCAP2_350_FF = 0x07;
        public const uint DSM_FCAP2_400_FF = 0x08;
        public const uint DSM_FCAP2_450_FF = 0x09;
        public const uint DSM_FCAP2_500_FF = 0x0A;
        public const uint DSM_FCAP2_550_FF = 0x0B;
        public const uint DSM_FCAP2_600_FF = 0x0C;
        public const uint DSM_FCAP2_650_FF = 0x0D;
        public const uint DSM_FCAP2_700_FF = 0x0E;
        public const uint DSM_FCAP2_750_FF = 0x0F;

        //**************************************** 
        // DSM.CR9 Control Register 9 definitions  
        //**************************************** 
        public const uint DSM_IPCAP3_EN = 0x80;

        public const uint DSM_IPCAP3_MASK   = 0x70;
        public const uint DSM_IPCAP3_0_FF   = 0x00;
        public const uint DSM_IPCAP3_50_FF  = 0x10;
        public const uint DSM_IPCAP3_100_FF = 0x20;
        public const uint DSM_IPCAP3_150_FF = 0x30;
        public const uint DSM_IPCAP3_200_FF = 0x40;
        public const uint DSM_IPCAP3_250_FF = 0x50;
        public const uint DSM_IPCAP3_300_FF = 0x60;
        public const uint DSM_IPCAP3_350_FF = 0x70;

        public const uint DSM_FCAP3_MASK   = 0x0F;
        public const uint DSM_FCAP3_0_FF   = 0x00;
        public const uint DSM_FCAP3_50_FF  = 0x01;
        public const uint DSM_FCAP3_100_FF = 0x02;
        public const uint DSM_FCAP3_150_FF = 0x03;
        public const uint DSM_FCAP3_200_FF = 0x04;
        public const uint DSM_FCAP3_250_FF = 0x05;
        public const uint DSM_FCAP3_300_FF = 0x06;
        public const uint DSM_FCAP3_350_FF = 0x07;
        public const uint DSM_FCAP3_400_FF = 0x08;
        public const uint DSM_FCAP3_450_FF = 0x09;
        public const uint DSM_FCAP3_500_FF = 0x0A;
        public const uint DSM_FCAP3_550_FF = 0x0B;
        public const uint DSM_FCAP3_600_FF = 0x0C;
        public const uint DSM_FCAP3_650_FF = 0x0D;
        public const uint DSM_FCAP3_700_FF = 0x0E;
        public const uint DSM_FCAP3_750_FF = 0x0F;


        //****************************************** 
        // DSM.CR10 Control Register 10 definitions  
        //****************************************** 
        public const uint DSM_SUMCAP1_EN = 0x80;
        public const uint DSM_SUMCAP2_EN = 0x08;

        public const uint DSM_SUMCAP1_MASK   = 0x70;
        public const uint DSM_SUMCAP1_0_FF   = 0x00;
        public const uint DSM_SUMCAP1_50_FF  = 0x10;
        public const uint DSM_SUMCAP1_100_FF = 0x20;
        public const uint DSM_SUMCAP1_150_FF = 0x30;
        public const uint DSM_SUMCAP1_200_FF = 0x40;
        public const uint DSM_SUMCAP1_250_FF = 0x50;
        public const uint DSM_SUMCAP1_300_FF = 0x60;
        public const uint DSM_SUMCAP1_350_FF = 0x70;

        public const uint DSM_SUMCAP2_MASK   = 0x07;
        public const uint DSM_SUMCAP2_0_FF   = 0x00;
        public const uint DSM_SUMCAP2_50_FF  = 0x01;
        public const uint DSM_SUMCAP2_100_FF = 0x02;
        public const uint DSM_SUMCAP2_150_FF = 0x03;
        public const uint DSM_SUMCAP2_200_FF = 0x04;
        public const uint DSM_SUMCAP2_250_FF = 0x05;
        public const uint DSM_SUMCAP2_300_FF = 0x06;
        public const uint DSM_SUMCAP2_350_FF = 0x07;

        //****************************************** 
        // DSM.CR11 Control Register 11 definitions  
        //****************************************** 
        public const uint DSM_SUMCAP3_EN = 0x08;

        public const uint DSM_SUMCAP3_MASK   = 0x70;
        public const uint DSM_SUMCAP3_0_FF   = 0x00;
        public const uint DSM_SUMCAP3_50_FF  = 0x10;
        public const uint DSM_SUMCAP3_100_FF = 0x20;
        public const uint DSM_SUMCAP3_150_FF = 0x30;
        public const uint DSM_SUMCAP3_200_FF = 0x40;
        public const uint DSM_SUMCAP3_250_FF = 0x50;
        public const uint DSM_SUMCAP3_300_FF = 0x60;
        public const uint DSM_SUMCAP3_350_FF = 0x70;

        public const uint DSM_SUMCAPFB_MASK   = 0x07;
        public const uint DSM_SUMCAPFB_0_FF   = 0x00;
        public const uint DSM_SUMCAPFB_50_FF  = 0x01;
        public const uint DSM_SUMCAPFB_100_FF = 0x02;
        public const uint DSM_SUMCAPFB_150_FF = 0x03;
        public const uint DSM_SUMCAPFB_200_FF = 0x04;
        public const uint DSM_SUMCAPFB_250_FF = 0x05;
        public const uint DSM_SUMCAPFB_300_FF = 0x06;
        public const uint DSM_SUMCAPFB_350_FF = 0x07;
        public const uint DSM_SUMCAPFB_400_FF = 0x08;
        public const uint DSM_SUMCAPFB_450_FF = 0x09;
        public const uint DSM_SUMCAPFB_500_FF = 0x0A;
        public const uint DSM_SUMCAPFB_550_FF = 0x0B;
        public const uint DSM_SUMCAPFB_600_FF = 0x0C;
        public const uint DSM_SUMCAPFB_650_FF = 0x0D;
        public const uint DSM_SUMCAPFB_700_FF = 0x0E;
        public const uint DSM_SUMCAPFB_750_FF = 0x0F;

        //****************************************** 
        // DSM.CR12 Control Register 12 definitions  
        //****************************************** 
        public const uint DSM_SUMCAPFB_EN = 0x40;
        public const uint DSM_SUMCAPIN_EN = 0x20;

        public const uint DSM_SUMCAPIN_MASK   = 0x1F;
        public const uint DSM_SUMCAPIN_0_FF   = 0x00;
        public const uint DSM_SUMCAPIN_50_FF  = 0x01;
        public const uint DSM_SUMCAPIN_100_FF = 0x02;
        public const uint DSM_SUMCAPIN_150_FF = 0x03;
        public const uint DSM_SUMCAPIN_200_FF = 0x04;
        public const uint DSM_SUMCAPIN_250_FF = 0x05;
        public const uint DSM_SUMCAPIN_300_FF = 0x06;
        public const uint DSM_SUMCAPIN_350_FF = 0x07;
        public const uint DSM_SUMCAPIN_400_FF = 0x08;
        public const uint DSM_SUMCAPIN_450_FF = 0x09;
        public const uint DSM_SUMCAPIN_500_FF = 0x0A;
        public const uint DSM_SUMCAPIN_550_FF = 0x0B;
        public const uint DSM_SUMCAPIN_600_FF = 0x0C;
        public const uint DSM_SUMCAPIN_650_FF = 0x0D;
        public const uint DSM_SUMCAPIN_700_FF = 0x0E;
        public const uint DSM_SUMCAPIN_750_FF = 0x0F;
        public const uint DSM_SUMCAPIN_800_FF = 0x10;
        public const uint DSM_SUMCAPIN_850_FF = 0x11;
        public const uint DSM_SUMCAPIN_900_FF = 0x12;
        public const uint DSM_SUMCAPIN_950_FF = 0x13;
        public const uint DSM_SUMCAPIN_1000_FF = 0x14;
        public const uint DSM_SUMCAPIN_1050_FF = 0x15;
        public const uint DSM_SUMCAPIN_1100_FF = 0x16;
        public const uint DSM_SUMCAPIN_1150_FF = 0x17;
        public const uint DSM_SUMCAPIN_1200_FF = 0x18;
        public const uint DSM_SUMCAPIN_1250_FF = 0x19;
        public const uint DSM_SUMCAPIN_1300_FF = 0x1A;
        public const uint DSM_SUMCAPIN_1350_FF = 0x1B;
        public const uint DSM_SUMCAPIN_1400_FF = 0x1C;
        public const uint DSM_SUMCAPIN_1450_FF = 0x1D;
        public const uint DSM_SUMCAPIN_1500_FF = 0x1E;
        public const uint DSM_SUMCAPIN_1550_FF = 0x15;


        //****************************************** 
        // DSM.CR13 Control Register 13 definitions  
        //****************************************** 
        public const uint DSM_CR13_RSVD = 0xFF;

        //****************************************** 
        // DSM.CR14 Control Register 14 definitions  
        //****************************************** 

        // Bit Field  DSM_POWER1                     
        public const uint DSM_POWER1_MASK = 0x07;

        public const uint DSM_POWER1_0 = 0x00;
        public const uint DSM_POWER1_1 = 0x01;
        public const uint DSM_POWER1_2 = 0x02;
        public const uint DSM_POWER1_3 = 0x03;
        public const uint DSM_POWER1_4 = 0x04;
        public const uint DSM_POWER1_5 = 0x05;
        public const uint DSM_POWER1_6 = 0x06;
        public const uint DSM_POWER1_7 = 0x07;

        public const uint DSM_POWER1_44UA  = 0x00;
        public const uint DSM_POWER1_123UA = 0x01;
        public const uint DSM_POWER1_492UA = 0x02;
        public const uint DSM_POWER1_750UA = 0x03;
        public const uint DSM_POWER1_1MA   = 0x04;

        // Bit Field  DSM_OPAMP1_BW                  
        public const uint DSM_OPAMP1_BW_MASK = 0xF0;
        public const uint DSM_OPAMP1_BW_0 = 0x00;
        public const uint DSM_OPAMP1_BW_1 = 0x10;
        public const uint DSM_OPAMP1_BW_2 = 0x20;
        public const uint DSM_OPAMP1_BW_3 = 0x30;
        public const uint DSM_OPAMP1_BW_4 = 0x40;
        public const uint DSM_OPAMP1_BW_5 = 0x50;
        public const uint DSM_OPAMP1_BW_6 = 0x60;
        public const uint DSM_OPAMP1_BW_7 = 0x70;
        public const uint DSM_OPAMP1_BW_8 = 0x80;
        public const uint DSM_OPAMP1_BW_9 = 0x90;
        public const uint DSM_OPAMP1_BW_A = 0xA0;
        public const uint DSM_OPAMP1_BW_B = 0xB0;
        public const uint DSM_OPAMP1_BW_C = 0xC0;
        public const uint DSM_OPAMP1_BW_D = 0xD0;
        public const uint DSM_OPAMP1_BW_E = 0xE0;
        public const uint DSM_OPAMP1_BW_F = 0xF0;

        //****************************************** 
        // DSM.CR15 Control Register 15 definitions  
        //****************************************** 

        // Bit Field  DSM_POWER2_3                   
        public const uint DSM_POWER2_3_MASK = 0x07;

        public const uint DSM_POWER2_3_LOW    = 0x00;
        public const uint DSM_POWER2_3_MED    = 0x01;
        public const uint DSM_POWER2_3_HIGH   = 0x02;
        public const uint DSM_POWER2_3_IP5X   = 0x03;
        public const uint DSM_POWER2_3_2X     = 0x04;
        public const uint DSM_POWER2_3_HIGH_5 = 0x05;
        public const uint DSM_POWER2_3_HIGH_6 = 0x06;
        public const uint DSM_POWER2_3_HIGH_7 = 0x07;

        // Bit Field  DSM_POWER_COMP                 
        public const uint DSM_POWER_COMP_MASK = 0x30;

        public const uint DSM_POWER_VERYLOW = 0x00;
        public const uint DSM_POWER_NORMAL  = 0x10;
        public const uint DSM_POWER_6MHZ    = 0x20;
        public const uint DSM_POWER_12MHZ   = 0x30;

        //****************************************** 
        // DSM.CR16 Control Register 16 definitions  
        //****************************************** 

        // Bit Field  DSM_POWER_SUM                  
        public const uint DSM_POWER_SUM_MASK = 0x70;

        public const uint DSM_POWER_SUM_LOW    = 0x00;  
        public const uint DSM_POWER_SUM_MED    = 0x10;
        public const uint DSM_POWER_SUM_HIGH   = 0x20;
        public const uint DSM_POWER_SUM_IP5X   = 0x30;
        public const uint DSM_POWER_SUM_2X     = 0x40;
        public const uint DSM_POWER_SUM_HIGH_5 = 0x50;
        public const uint DSM_POWER_SUM_HIGH_6 = 0x60;
        public const uint DSM_POWER_SUM_HIGH_7 = 0x70;

        public const uint DSM_CP_ENABLE = 0x08;
        public const uint DSM_CP_PWRCTL_MASK = 0x07;
        public const uint DSM_CP_PWRCTL_DEFAULT = 0x0A;

        //****************************************** 
        // DSM.CR17 Control Register 17 definitions  
        //****************************************** 

        // Bit Field  DSM_EN_BUF                     
        public const uint DSM_EN_BUF_VREF = 0x01;
        public const uint DSM_EN_BUF_VCM = 0x02;

        public const uint DSM_PWR_CTRL_VREF_MASK = 0x0C;
        public const uint DSM_PWR_CTRL_VREF_0 = 0x00;
        public const uint DSM_PWR_CTRL_VREF_1 = 0x04;
        public const uint DSM_PWR_CTRL_VREF_2 = 0x08;
        public const uint DSM_PWR_CTRL_VREF_3 = 0x0C;

        public const uint DSM_PWR_CTRL_VCM_MASK = 0x30;
        public const uint DSM_PWR_CTRL_VCM_0    = 0x00;
        public const uint DSM_PWR_CTRL_VCM_1    = 0x10;
        public const uint DSM_PWR_CTRL_VCM_2    = 0x20;
        public const uint DSM_PWR_CTRL_VCM_3    = 0x30;

        public const uint DSM_PWR_CTRL_VREF_INN_MASK = 0xC0;
        public const uint DSM_PWR_CTRL_VREF_INN_0    = 0x00;
        public const uint DSM_PWR_CTRL_VREF_INN_1    = 0x40;
        public const uint DSM_PWR_CTRL_VREF_INN_2    = 0x80;
        public const uint DSM_PWR_CTRL_VREF_INN_3    = 0xC0;

        //******************************************* 
        // DSM.REF0 Reference Register 0 definitions  
        //******************************************* 

        public const uint DSM_REFMUX_MASK   = 0x07;
        public const uint DSM_REFMUX_NONE   = 0x00;
        public const uint DSM_REFMUX_UVCM   = 0x01;
        public const uint DSM_REFMUX_VDA_4  = 0x02;
        public const uint DSM_REFMUX_VDAC0  = 0x03;
        public const uint DSM_REFMUX_1_024V = 0x04;
        public const uint DSM_REFMUX_1_20V  = 0x05;

        public const uint DSM_EN_BUF_VREF_INN = 0x08;
        public const uint DSM_VREF_RES_DIV_EN = 0x10;
        public const uint DSM_VCM_RES_DIV_EN  = 0x20;
        public const uint DSM_VCMSEL_MASK     = 0xC0;
        public const uint DSM_VCMSEL_NOSEL    = 0x00;
        public const uint DSM_VCMSEL_0_8V     = 0x40;
        public const uint DSM_VCMSEL_0_7V     = 0x80;
        public const uint DSM_VCMSEL_VPWRA_2  = 0xC0;

        //******************************************* 
        // DSM.REF1 Reference Register 1 definitions  
        //******************************************* 
        public const uint DSM_REF1_MASK = 0xFF;

        //******************************************* 
        // DSM.REF2 Reference Register 2 definitions  
        //******************************************* 
        public const uint DSM_REF2_MASK  = 0xFF;
        public const uint DSM_REF2_NO_SW = 0x00;
        public const uint DSM_REF2_S0_EN = 0x01;
        public const uint DSM_REF2_S1_EN = 0x02;
        public const uint DSM_REF2_S2_EN = 0x04;
        public const uint DSM_REF2_S3_EN = 0x08;
        public const uint DSM_REF2_S4_EN = 0x10;
        public const uint DSM_REF2_S5_EN = 0x20;
        public const uint DSM_REF2_S6_EN = 0x40;
        public const uint DSM_REF2_S7_EN = 0x80;

        //******************************************* 
        // DSM.REF3 Reference Register 3 definitions  
        //******************************************* 
        public const uint DSM_REF3_MASK   = 0xFF;
        public const uint DSM_REF3_NO_SW  = 0x00;
        public const uint DSM_REF3_S8_EN  = 0x01;
        public const uint DSM_REF3_S9_EN  = 0x02;
        public const uint DSM_REF3_S10_EN = 0x04;
        public const uint DSM_REF3_S11_EN = 0x08;
        public const uint DSM_REF3_S12_EN = 0x10;
        public const uint DSM_REF3_S13_EN = 0x20;


        //********************************************** 
        // DSM.DEM0 Dynamic Element Matching Register 0  
        //********************************************** 
        public const uint DSM_EN_SCRAMBLER0 = 0x01;
        public const uint DSM_EN_SCRAMBLER1 = 0x02;
        public const uint DSM_EN_DEM        = 0x04;
        public const uint DSM_ADC_TEST_EN   = 0x08;
        public const uint DSM_DEMTEST_SRC   = 0x10;

        //********************************************** 
        // DSM.DEM1 Dynamic Element Matching Register 1  
        //********************************************** 
        public const uint DSM_DEM1_MASK = 0xFF;


        //********************************************* 
        // DSM.BUF0 Buffer Register 0                   
        //********************************************* 
        public const uint DSM_ENABLE_P     = 0x01;
        public const uint DSM_BYPASS_P     = 0x02;
        public const uint DSM_RAIL_RAIL_EN = 0x04;

        public const uint DSM_BUFSEL   = 0x08;
        public const uint DSM_BUFSEL_A = 0x00;
        public const uint DSM_BUFSEL_B = 0x08;


        //********************************************* 
        // DSM.BUF1 Buffer Register 1                   
        //********************************************* 
        public const uint DSM_ENABLE_N  = 0x01;
        public const uint DSM_BYPASS_N  = 0x02;
        public const uint DSM_GAIN_MASK = 0x0C;
        public const uint DSM_GAIN_1X   = 0x00;
        public const uint DSM_GAIN_2X   = 0x04;
        public const uint DSM_GAIN_4X   = 0x08;
        public const uint DSM_GAIN_8X   = 0x0C;

        //********************************************* 
        // DSM.BUF2 Buffer Register 2                   
        //********************************************* 
        public const uint DSM_LOWPOWER_EN  = 0x01;
        public const uint DSM_ADD_EXTRA_RC = 0x02;

        //********************************************* 
        // DSM.BUF3 Buffer Register 3                   
        //********************************************* 
        public const uint DSM_BUF_CHOP_EN = 0x08;

        public const uint DSM_BUF_FCHOP_MASK  = 0x07;
        public const uint DSM_BUF_FCHOP_FS2   = 0x00;
        public const uint DSM_BUF_FCHOP_FS4   = 0x01;
        public const uint DSM_BUF_FCHOP_FS8   = 0x02;
        public const uint DSM_BUF_FCHOP_FS16  = 0x03;
        public const uint DSM_BUF_FCHOP_FS32  = 0x04;
        public const uint DSM_BUF_FCHOP_FS64  = 0x05;
        public const uint DSM_BUF_FCHOP_FS128 = 0x06;
        public const uint DSM_BUF_FCHOP_FS256 = 0x07;

        //********************************************* 
        // DSM.MISC Delta Sigma Misc Register           
        //********************************************* 
        public const uint DSM_SEL_ICLK_CP = 0x01;

        //********************************************** 
        // DSM.CLK Delta Sigma Clock selection Register  
        //********************************************** 
        public const uint DSM_CLK_MX_CLK_MSK  = 0x07;
        public const uint DSM_CLK_CLK_EN      = 0x08;
        public const uint DSM_CLK_BYPASS_SYNC = 0x10;


        //********************************************* 
        // DSM.OUT0 Output Register 0                   
        //********************************************* 
        public const uint DSM_DOUT0 = 0xFF;


        //********************************************* 
        // DSM.OUT1 Output Register 1                   
        //********************************************* 
        public const uint DSM_DOUT2SCOMP_MASK = 0x0F;
        public const uint DSM_OVDFLAG         = 0x10;
        public const uint DSM_OVDCAUSE        = 0x20;


        //********************************************* 
        //          Decimator                           
        //********************************************* 


        //********************************************* 
        // DEC.CR Control Register 0                    
        //********************************************* 
        public const uint DEC_SAT_EN  = 0x80; // Enable post process offset correction  
        public const uint DEC_FIR_EN  = 0x40; // Post process FIR enable   
        public const uint DEC_OCOR_EN = 0x20; // Offest correction enable  
        public const uint DEC_GCOR_EN = 0x10; // Enable gain correction feature  

        public const uint MODE_MASK          = 0x0C; // Conversion Mode  
        public const uint MODE_SINGLE_SAMPLE = 0x00;
        public const uint MODE_FAST_FILTER   = 0x04;
        public const uint MODE_CONTINUOUS    = 0x08;
        public const uint MODE_FAST_FIR      = 0x0C;

        public const uint DEC_XSTART_EN  = 0x02; // Enables external start signal  
        public const uint DEC_START_CONV = 0x01; // A write to this bit starts a conversion  


        //********************************************* 
        // DEC.SR Status and Control Register 0         
        //********************************************* 
        public const uint DEC_CORECLK_DISABLE = 0x20; // Controls gating of the Core clock  
        public const uint DEC_INTR_PULSE      = 0x10; // Controls interrupt mode   
        public const uint DEC_OUT_ALIGN       = 0x08; // Controls 8-bit shift of SAMP registers  
        public const uint DEC_INTR_CLEAR      = 0x04; // A write to this bit clears bit0  
        public const uint DEC_INTR_MASK       = 0x02; // Controls the generation of the conversion comp. INTR  
        public const uint DEC_CONV_DONE       = 0x01; // Bit set if conversion has completed   

        //********************************************* 
        // DEC.SHIFT1 Decimator Input Shift Register 1  
        //********************************************* 
        public const uint DEC_SHIFT1_MASK = 0x1F; // Input shift1 control  

        //********************************************* 
        // DEC.SHIFT2 Decimator Input Shift Register 2  
        //********************************************* 
        public const uint DEC_SHIFT2_MASK = 0x0F; // Input shift2 control  

        //************************************************************** 
        // DEC.DR2 Decimator Decimation Rate of FIR Filter Low Register  
        //************************************************************** 
        public const uint DEC_DR2_MASK = 0xFF;

        //**************************************************************** 
        // DEC.DR2H Decimator Decimation Rate of FIR Filter High Register  
        //**************************************************************** 
        public const uint DEC_DR2H_MASK = 0x03;

        //********************************************* 
        // DEC.COHR Decimator Coherency Register        
        //********************************************* 
        public const uint DEC_GCOR_KEY_MASK = 0x30;
        public const uint DEC_GCOR_KEY_OFF  = 0x00;
        public const uint DEC_GCOR_KEY_LOW  = 0x10;
        public const uint DEC_GCOR_KEY_MID  = 0x20;
        public const uint DEC_GCOR_KEY_HIGH = 0x30;

        public const uint DEC_OCOR_KEY_MASK = 0x0C;
        public const uint DEC_OCOR_KEY_OFF  = 0x00;
        public const uint DEC_OCOR_KEY_LOW  = 0x04;
        public const uint DEC_OCOR_KEY_MID  = 0x08;
        public const uint DEC_OCOR_KEY_HIGH = 0x0C;

        public const uint DEC_SAMP_KEY_MASK = 0x03;
        public const uint DEC_SAMP_KEY_OFF  = 0x00;
        public const uint DEC_SAMP_KEY_LOW  = 0x01;
        public const uint DEC_SAMP_KEY_MID  = 0x02;
        public const uint DEC_SAMP_KEY_HIGH = 0x03;

        //********************************************* 
        // DSM.SW3 DSM Analog Routing Register 3        
        //********************************************* 
        public const uint DSM_VN_AMX      = 0x10;
        public const uint DSM_VN_VREF     = 0x20;
        public const uint DSM_VN_VSSA     = 0x40;
        public const uint DSM_VN_REF_MASK = 0x70; 


  
    }
}


