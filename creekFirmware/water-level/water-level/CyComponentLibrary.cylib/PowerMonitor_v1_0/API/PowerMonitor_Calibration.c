/*******************************************************************************
* File Name: `$INSTANCE_NAME`_Calibration.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the calibration API for the 
*  Power Monitor Component.
*
* Note:
*
*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"


/* Next Calibration type to run.*/
static uint8 `$INSTANCE_NAME`_calType;


/*******************************************************************************
* Configuration #1 : 2048 mV differential range
*      adcZeroCfg1  = units are ADC counts when both inputs are at Vssa
*      adcGainCfg1  = units are (1 mV) / (ADC count)
*      adcSeAdjCfg1 = units are mV from 2xVref PGA (added to all voltage rails)
*******************************************************************************/
int16 `$INSTANCE_NAME`_adcZeroCfg1;   /* nominally 0 counts       */
float `$INSTANCE_NAME`_adcGainCfg1;   /* nominally 1.0 mV/ct      */
int16 `$INSTANCE_NAME`_adcSeAdjCfg1;  /* nominally 2048 mV        */


/*******************************************************************************
* Configuration #2 : differential range:
*          adcZeroCfg2 = units are ADC counts when both inputs are at Vssa
*          adcGainCfg2 = units are (100 uV) / (ADC count)
*******************************************************************************/
int16 `$INSTANCE_NAME`_adcZeroCfg2;   /* nominally 0 counts                       */
float `$INSTANCE_NAME`_adcGainCfg2;   /* nominally 0.03125 mV/ct @  64mV range    */
                                /*           0.12500 mV/ct @256 mV range    */
float `$INSTANCE_NAME`_adcVoltCfg2;      

                                
/*******************************************************************************
* Configuration #3 : 1024 mV differential range
*          adcZeroCfg3 = units are ADC counts when both inputs are at Vssa
*          adcGainCfg3 = units are (1 mV) / (ADC count)
*******************************************************************************/
int16 `$INSTANCE_NAME`_adcZeroCfg3;   /* nominally 0 counts       */
float `$INSTANCE_NAME`_adcGainCfg3;   /* nominally 0.5 mV/ct      */

/* Variables to hold filtered ADC raw data */
static int32 CYXDATA `$INSTANCE_NAME`_filtZeroRaw1;
static int32 CYXDATA `$INSTANCE_NAME`_filtZeroRaw2;
static int32 CYXDATA `$INSTANCE_NAME`_filtZeroRaw3;
static int32 CYXDATA `$INSTANCE_NAME`_filtGainRaw1;
static int32 CYXDATA `$INSTANCE_NAME`_filtGainRaw2a;
static int32 CYXDATA `$INSTANCE_NAME`_filtGainRaw2b;
static int32 CYXDATA `$INSTANCE_NAME`_filtGainRaw3;
static int32 CYXDATA `$INSTANCE_NAME`_filtSCAdjRaw1;

/* Calibration related variables */
int16 `$INSTANCE_NAME`_cal2a_raw[`$INSTANCE_NAME`_CAL2A_FILT_SZ];
uint8 `$INSTANCE_NAME`_cal2a_idx = 0u;
int16 `$INSTANCE_NAME`_cal2a_tot;


/*******************************************************************************
* Local function prototypes
*******************************************************************************/
static void  `$INSTANCE_NAME`_RequestAdcCal (CYBIT iirInit);
static float `$INSTANCE_NAME`_iirFiltAdc    (CYBIT, int16 newData, int32 CYXDATA *filtVal);


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Calibrate()
********************************************************************************
*
* Summary: 
*  Initialize calibration after reset by running a quick series of
*  various calibration operations .
*
* Parameters:
*  None
*
* Return:
*  None
*
* Theory:
*  IDEAL CALIBRATION VALUES
*
* Ideal raw values from the ADC
*   `$INSTANCE_NAME`_adcZeroCalRawCfg1  = 0;
*   `$INSTANCE_NAME`_adcGainCalRawCfg1  = -1024;    Vref measured on +/- range
*   `$INSTANCE_NAME`_adcSCCalRawCfg1 = -2048;    Vref * 2
*   `$INSTANCE_NAME`_adcZeroCalRawCfg2  = 0;
*   `$INSTANCE_NAME`_adcZeroCalRawCfg3  = 0;
*   `$INSTANCE_NAME`_adcGainCalRawCfg3  = -2048; Vref measured on 
*    +/-1024 mV range.
*
* Ideal post-processed values:
*   `$INSTANCE_NAME`_adcZeroCfg1 = 0;         units are adc counts
*   `$INSTANCE_NAME`_adcGainCfg1 = 1.0;       units are 1 mV/count
*   `$INSTANCE_NAME`_adcSeAdjCfg1 = 2048;     units are mV
*   `$INSTANCE_NAME`_adcZeroCfg2 = 0;         units are adc counts
*   `$INSTANCE_NAME`_adcGainCfg2 = 0.03125; units are 1 mV/count 
*        at +/- 64 mV range 0.125; units are 1 mV/count at +/-256 mV range
*   `$INSTANCE_NAME`_adcZeroCfg3 = 0;         units are adc counts
*   `$INSTANCE_NAME`_adcGainCfg3 = 0.5;       units are 1 mV/count
*
*******************************************************************************/
void `$INSTANCE_NAME`_Calibrate(void) `=ReentrantKeil($INSTANCE_NAME . "_Calibrate")`
{
    #if (`$INSTANCE_NAME`_CAL_PIN_EXPOSED)
        uint8 ivar;
    #endif /* `$INSTANCE_NAME`_CAL_PIN_EXPOSED */
    
    /***************************************************************************
    * Measure Config #1 (+/-2048 mV) Zero and Gain
    ***************************************************************************/
    `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG1Z;
    `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);
    /* Measure Vref w/2048mV range  */
    `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG1G; 
    `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);

    /***************************************************************************
    * Measure 2xVref PGA voltage
    * Will be added to all voltage measurements as Config #1 offset)
    ***************************************************************************/
    `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_PGAZ;
    `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);

    /***************************************************************************
    * Measure Config #3 (+/-1024 mV) Zero and Gain
    ***************************************************************************/
    `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG3Z;
    `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);
    `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG3G;  /* Measure Vref w/1024mV range */
    `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);

    
    /***************************************************************************
    * Measure Config #2 (differential low-voltage range) Zero and Gain
    ***************************************************************************/
    `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG2Z;
    `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);
        
    #if (`$INSTANCE_NAME`_CAL_PIN_EXPOSED)

        /* Measure user input on +/-1024 mV range */
        `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG2Ga; 
        `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);
        
           /***********************************************************************
            * Gather enough sample to render a complete oversampled value
            *  when mesuring ~50mV on +/-1024 mV range
            ***********************************************************************/
            
            for (ivar = 0u; ivar < (1u << `$INSTANCE_NAME`_CAL2A_OVER_EXP); ivar++)
            {
                `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);
            }
    
        `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG2Gb; /* Measure cal input on low-voltage range */
        `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);
        
    #else
        /* adcGain for config 2 */
        `$INSTANCE_NAME`_calType = `$INSTANCE_NAME`_CAL_CFG2G; 
        `$INSTANCE_NAME`_RequestAdcCal(`$INSTANCE_NAME`_INITIALIZE_IIR_FILTER);

    #endif /* `$INSTANCE_NAME`_CAL_PIN_EXPOSED */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RequestAdcCal()
********************************************************************************
*
* Summary:
*  Punctuate normal ADC sampling process for selected calibration operation.
*
* Parameters:
*  iirInit = initialize the filter with this reading
*
* Return:
*  Appropriate IIR filter and Supervisor_adc???? variable updated
*
* Side Effects:
*  Waits for ADC interrupt to complete the requested raw calibration operation.
*  THIS IS A CANDIATE FOR A TWO-STATE PROCESS to return to main() with minimal
*  delay.
*
*******************************************************************************/
static void `$INSTANCE_NAME`_RequestAdcCal(CYBIT iirInit)
{
    int CYDATA tmpInt;
    float nominalGain;

    /* Save which sample was next in sequence before calibration interruption */
    CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);

    `$INSTANCE_NAME`_adcConvNextPreCal = `$INSTANCE_NAME`_adcConvNext;
    `$INSTANCE_NAME`_adcConvNext       = `$INSTANCE_NAME`_STATE_CAL;
    `$INSTANCE_NAME`_adcConvCalType    = `$INSTANCE_NAME`_calType;

    CyIntEnable (`$INSTANCE_NAME`_IRQ__INTC_NUMBER);

    /***************************************************************************
    * WAIT FOR ADC ISR TO FINISH GETTING RAW CALIBRATION VALUE.
    * (ADC has cleared calibration process and resumed normal sampling
    ***************************************************************************/
    while ((`$INSTANCE_NAME`_adcConvNext == `$INSTANCE_NAME`_STATE_CAL) ||   /* wait start */
           (`$INSTANCE_NAME`_adcConvNow    == `$INSTANCE_NAME`_STATE_CAL))  /* wait finish*/
        ;

    /***************************************************************************
    * Process "raw" ADC value from the ISR to something useful.
    ***************************************************************************/
    switch (`$INSTANCE_NAME`_calType)
    {
        /* Calibrate ADC Configuration #1 (+/-2048 mV range) */
        case `$INSTANCE_NAME`_CAL_CFG1Z:
            `$INSTANCE_NAME`_adcZeroCfg1 = `$INSTANCE_NAME`_iirFiltAdc(iirInit,`$INSTANCE_NAME`_adcZeroCalRawCfg1,
                                                                       &`$INSTANCE_NAME`_filtZeroRaw1);
            break;

        case `$INSTANCE_NAME`_CAL_CFG1G:
            /***********************************************************************
            * Correct for:   a) Route inversion
            *                b) Cfg #1 zero offset
            ***********************************************************************/
            tmpInt  = `$INSTANCE_NAME`_iirFiltAdc(iirInit,`$INSTANCE_NAME`_adcGainCalRawCfg1, 
                                                  &`$INSTANCE_NAME`_filtGainRaw1);
            tmpInt  = -tmpInt;
            tmpInt -= `$INSTANCE_NAME`_adcZeroCfg1;
            `$INSTANCE_NAME`_adcGainCfg1 = 1024.0 / tmpInt;
            break;

        case `$INSTANCE_NAME`_CAL_PGAZ:
            /***********************************************************************
            * Calibrate 2048 mV PGA voltage (added to all voltage measurements)
            * Ideal = -2048 counts raw
            ***********************************************************************/
            /***********************************************************************
            * Correct for:   a) Route inversion
            *                b) Cfg #1 zero offset
            ***********************************************************************/
            tmpInt  = `$INSTANCE_NAME`_iirFiltAdc(iirInit, `$INSTANCE_NAME`_adcSCCalRawCfg1, 
                                                  &`$INSTANCE_NAME`_filtSCAdjRaw1);
            tmpInt  = -tmpInt;
            tmpInt -= `$INSTANCE_NAME`_adcZeroCfg1;
            `$INSTANCE_NAME`_adcSeAdjCfg1 = tmpInt * `$INSTANCE_NAME`_adcGainCfg1;
            break;

        /***************************************************************************
        * Calibrate Config #2 (low-voltage range)
        * Use VDAC to generate calibration signal in the proper range, then
        *   A) Measure VDAC on   calibrated 1024 mV range (ideal  120 counts raw)
        *   B) Measure VDAC on uncalibrated  low mV range (ideal 1920 counts raw)
        ***************************************************************************/
        case `$INSTANCE_NAME`_CAL_CFG2Z:
            `$INSTANCE_NAME`_adcZeroCfg2 = `$INSTANCE_NAME`_iirFiltAdc(iirInit, `$INSTANCE_NAME`_adcZeroCalRawCfg2, 
                                                                       &`$INSTANCE_NAME`_filtZeroRaw2);
            break;
            
        case `$INSTANCE_NAME`_CAL_CFG2Ga:
            /***********************************************************************
            * The input cal signal is measured on Config #4 range and the 
             * result is result is subsequently used when measuring on 
            * low-voltage range (not now) Oversample 12-bit +/-1024 mV range to 
            * 14-bit resolution to avoid introducing error when applying to 
            * low voltage range.
            *******************************************************************/
            `$INSTANCE_NAME`_cal2a_tot -= `$INSTANCE_NAME`_cal2a_raw[`$INSTANCE_NAME`_cal2a_idx];
            `$INSTANCE_NAME`_cal2a_tot += `$INSTANCE_NAME`_adcGainCalRawCfg2a;
            `$INSTANCE_NAME`_cal2a_raw[`$INSTANCE_NAME`_cal2a_idx] = `$INSTANCE_NAME`_adcGainCalRawCfg2a;
            if (++`$INSTANCE_NAME`_cal2a_idx >= `$INSTANCE_NAME`_CAL2A_FILT_SZ)
            {
                /***************************************************************
                * Store new adcGainRaw2a (14-bit resolution, ~125 uV/ct)
                ***************************************************************/
                `$INSTANCE_NAME`_filtGainRaw2a = `$INSTANCE_NAME`_cal2a_tot / 4;
                `$INSTANCE_NAME`_cal2a_idx = 0u;
            }
        break;

        case `$INSTANCE_NAME`_CAL_CFG2Gb:
            /* #if OVERSAMPLE_CAL2a */
            `$INSTANCE_NAME`_adcVoltCfg2  = (float)`$INSTANCE_NAME`_filtGainRaw2a / 4;
            `$INSTANCE_NAME`_adcVoltCfg2 -= `$INSTANCE_NAME`_adcZeroCfg3;
            `$INSTANCE_NAME`_adcVoltCfg2 *= `$INSTANCE_NAME`_adcGainCfg3;   /* mV VDAC */


            /***********************************************************************
            * Supervisor_adcVdacCfg2 is the measured "test" VDAC output level in mV
            *  (as measured with the 1024 mV ADC range)
            *
            *                 tmpInt = filtered counts of VDAC on low-voltage range
            * Supervisor_adcGainCfg2 = floating point mV/count on low-voltage range
            ***********************************************************************/
            tmpInt  = `$INSTANCE_NAME`_iirFiltAdc(iirInit, `$INSTANCE_NAME`_adcGainCalRawCfg2b, 
                                                  &`$INSTANCE_NAME`_filtGainRaw2b);
            tmpInt -= `$INSTANCE_NAME`_adcZeroCfg2;
            `$INSTANCE_NAME`_adcGainCfg2 = `$INSTANCE_NAME`_adcVoltCfg2 / tmpInt;
        break;
        
        case `$INSTANCE_NAME`_CAL_CFG2G:
            #if (`$INSTANCE_NAME`_DEFAULT_DIFF_CURRENT_RANGE == `$INSTANCE_NAME`_DIFF_CURRENT_RANGE_64MV)
                nominalGain = 0.03125; /* Nominal gain for 64mV ADC range */
            #else
                nominalGain = 0.06250; /* Nominal gain for 128mV ADC range */
            #endif /* (`$INSTANCE_NAME`_DEFAULT_DIFF_CURRENT_RANGE == `$INSTANCE_NAME`_DIFF_CURRENT_RANGE_64MV) */
                `$INSTANCE_NAME`_adcGainCfg2 = nominalGain; /* `$INSTANCE_NAME`_adcZeroCfg2; */
        break;
        
        
        /***************************************************************************
        * Calibrate Config #3 (+/-1024 mV range)  Ideal = -2048 counts raw
        ***************************************************************************/
        case `$INSTANCE_NAME`_CAL_CFG3Z:
            `$INSTANCE_NAME`_adcZeroCfg3 = `$INSTANCE_NAME`_iirFiltAdc(iirInit, `$INSTANCE_NAME`_adcZeroCalRawCfg3,
                                                                       &`$INSTANCE_NAME`_filtZeroRaw3);
            break;

        case `$INSTANCE_NAME`_CAL_CFG3G:
            /***********************************************************************
            * Correct for:   a) Route inversion
            *                b) Cfg #3 zero offset
            ***********************************************************************/
            tmpInt  = `$INSTANCE_NAME`_iirFiltAdc(iirInit, `$INSTANCE_NAME`_adcGainCalRawCfg3, 
                                                  &`$INSTANCE_NAME`_filtGainRaw3);
            tmpInt  = -tmpInt;
            tmpInt -= `$INSTANCE_NAME`_adcZeroCfg3;
            /* Correct for floating mV/count (tmpInt reads perfect 1024.0 mV)     */
            `$INSTANCE_NAME`_adcGainCfg3 = 1024.0 / tmpInt;   /* floating mV/count      */
            break;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_iirFiltAdc()
********************************************************************************
*
* Summary:
*  Recursive Infinite Impulse Response (IIR) filter for raw calibration 
*  readings.
*  Post new data to filter and return filtered result
*
* Parameters:
*  *filtVal = ptr to desired IIR filter variable
*  newData = new reading to be added to filter
*
* Return:
*  filtered result
*
* Theory:
*  The values in each IIR filter register is the (average * SCALE_VAL).
*
*  FILTER_SHIFT  RiseTime Samples     FILTER_SHIFT  RiseTime Samples
*             1 -> 3                              4  -> 34
*             2 -> 8                              5  -> 69
*             3 -> 16                             6  -> 140
*
*******************************************************************************/
static float `$INSTANCE_NAME`_iirFiltAdc(CYBIT iirInit, int16 newData, int32 CYXDATA *filtVal)
{
    CYDATA int32 tmpFilt;
    CYDATA int32 junk;
    /* If filter should be initialized to newData */
    if (iirInit)
    {
        /* start filter with newData */
        tmpFilt = (int32)newData * `$INSTANCE_NAME`_SCALE_VAL;  
    }
    /* Else filter should incorporate newData */
    else
    {
        tmpFilt = *filtVal;
        junk = (tmpFilt + `$INSTANCE_NAME`_SCALE_VAL / 2) / `$INSTANCE_NAME`_SCALE_VAL;
        tmpFilt = tmpFilt - junk + newData;
    }
    *filtVal = tmpFilt;
    return ((float)tmpFilt / `$INSTANCE_NAME`_SCALE_VAL);
}


/* [] END OF FILE */
