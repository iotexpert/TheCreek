/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides all API functionality of the TrimMargin component
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include <cyfitter.h>
#include "`$INSTANCE_NAME`.h"


/***************************************
* Custom Declarations
***************************************/
/* `#START CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
*  Customizer Generated Constants
***************************************/

/* Initial Margin low output voltage */
`$VMarginLowArray`;

/* Initial Margin high output voltage */
`$VMarginHighArray`;

/* Nominal output voltage */
`$VNominalArray`;

/* Precalculated PWM duty cycle AFTER power converter is enabled.
*  DUTYCYCLE = VADJ / VDDIO * 2 ^ (PWMResolution - 1)
*/
`$PWMNomArray`;

/* Precalculated PWM duty cycle BEFORE power converter is enabled.
*  It is used to archive VADJ voltage with the assumption that
*  the R1 is grounded in parallel with R2.
*  PRE_RUN_DUTYCYCLE = DUTYCYCLE * (R4 / ( R3 + (R2||R1)) + 1)
*/
`$PWMPreArray`;

/* Precalculated output voltage movement(in mV) per half of PWM count
*  First recalculate R2. The real value of the R2 could be different from
*  the value entered by the user. It could be affected by internal
*  power converter impedance which is parallel to R2.
*  For exapmle ADP3331 has R3int + R4int resistances.
*  R2real = VADJ * R1 / (VNOM - VADJ)
*  Now recalculate Maximum output Voltage based on R3 entered by user and
*  recalculated R2real.
*  VMAX = VADJ * R1 / (R2real || (R3+R4)) + VADJ
*  VDELTA = (VMAX - VNOM) / DUTYCYCLE / 2
*/
`$vDeltaArray`;

/* Precalculated PWM duty cycles difference count per 1 Volt movement of the
*  output voltage.   This value is oposite from VDELTA.
*  DUTYCYCLES_PER_VOLT = DUTYCYCLE / (VMAX - VNOM)
*/
`$PWMPerMVArray`;

/* Precalculated PWM duty cycle for Margin Low Voltage.
*  VMARGIN_LOW_DUTYCYCLE = DUTYCYCLE + _DUTYCYCLES_PER_VOLT * 
*                          (VNOM - VMAGRIN_LOW)
*/
`$pwmVMarginLowArray`;

/* Precalculated PWM duty cycle for Margin High Voltage.
*  VMARGIN_HIGH_DUTYCYCLE = DUTYCYCLE - _DUTYCYCLES_PER_VOLT * 
*                            (VMAGRIN_HIGH - VNOM)
*/
`$pwmVMarginHighArray`;

/* Not used in the code constants for familiarization purpose only */
/*`$VDDIOArray`;*/
/*`$R1Array`;*/
/*`$R2Array`;*/
/*`$R3Array`;*/


/***************************************
* Global data allocation
***************************************/
uint8 `$INSTANCE_NAME`_initVar = 0u;

/* Bit mask indicating which PWMs are generating an alert*/
`$AlertSourceTypeReplacementString` `$INSTANCE_NAME`_alertSource;

/* PWM duty cycle for Low/High margin copied from VMARGIN_LOW_DUTYCYCLE and 
*  VMARGIN_HIGH_DUTYCYCLE respectively in the Init() function. These values are 
*  recalculated by SetMarginLowVoltage()/SetMarginHighVoltage() functions when 
*  new margin values are set. Used by  MarginHigh()/MarginLow() to
*  set PWM for open loop margin. 
*/
`$DutyCycleTypeReplacementString` `$INSTANCE_NAME`_vMarginLowDutyCycle[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];
`$DutyCycleTypeReplacementString` `$INSTANCE_NAME`_vMarginHighDutyCycle[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];

/* Margin low output voltage */
uint16 `$INSTANCE_NAME`_vMarginLow[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];

/* Margin high output voltage */
uint16 `$INSTANCE_NAME`_vMarginHigh[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];

/* Trim control */
`$INSTANCE_NAME`_TRIMCTL_STRUCT `$INSTANCE_NAME`_trimCtl[`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS];


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Enables the component. Calls the Init() API if the component has not been
*  initialized before. Calls Enable() API.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  The `$INSTANCE_NAME`_initVar variable is used to indicate initial
*  configuration of this component. The variable is initialized to zero (0u)
*  and set to one (1u) the first time `$INSTANCE_NAME`_Start() is called.
*  This allows for component initialization without re-initialization in all
*  subsequent calls to the `$INSTANCE_NAME`_Start() routine.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initialize component's parameters to the parameters set by user in the
*  customizer of the component placed onto schematic. Usually called in
*  `$INSTANCE_NAME`_Start().
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    uint8 i;
    /* Init local variables from the entered in customizer parameters */
    memcpy(`$INSTANCE_NAME`_vMarginLow, `$INSTANCE_NAME`_VMARGIN_LOW,
                                `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS * sizeof(*`$INSTANCE_NAME`_vMarginLow));
    memcpy(`$INSTANCE_NAME`_vMarginHigh, `$INSTANCE_NAME`_VMARGIN_HIGH,
                                `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS * sizeof(*`$INSTANCE_NAME`_vMarginHigh));
    memcpy(`$INSTANCE_NAME`_vMarginLowDutyCycle, `$INSTANCE_NAME`_VMARGIN_LOW_DUTYCYCLE,
                                `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS * sizeof(*`$INSTANCE_NAME`_vMarginLowDutyCycle));
    memcpy(`$INSTANCE_NAME`_vMarginHighDutyCycle, `$INSTANCE_NAME`_VMARGIN_HIGH_DUTYCYCLE,
                                `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS * sizeof(*`$INSTANCE_NAME`_vMarginHighDutyCycle));

    /* Init PWMs */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 1)
        `$INSTANCE_NAME`_TrimPWM_1_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 3)
        `$INSTANCE_NAME`_TrimPWM_2_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 5)
        `$INSTANCE_NAME`_TrimPWM_3_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 7)
        `$INSTANCE_NAME`_TrimPWM_4_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 9)
        `$INSTANCE_NAME`_TrimPWM_5_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 11)
        `$INSTANCE_NAME`_TrimPWM_6_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 13)
        `$INSTANCE_NAME`_TrimPWM_7_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 15)
        `$INSTANCE_NAME`_TrimPWM_8_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 17)
        `$INSTANCE_NAME`_TrimPWM_9_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 19)
        `$INSTANCE_NAME`_TrimPWM_10_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 21)
        `$INSTANCE_NAME`_TrimPWM_11_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
    #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 23)
        `$INSTANCE_NAME`_TrimPWM_12_Start();
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */

    /* Set desired PWM Prerun duty-cycle */
    for (i = 0; i < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS; i++)
    {
        `$INSTANCE_NAME`_SetDutyCycle(i+1, `$INSTANCE_NAME`_PRE_RUN_DUTYCYCLE[i]);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables and starts the PWMs.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 i;
    for (i = 0; i < `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS; i++)
    {
        /* Clear statistic */
        `$INSTANCE_NAME`_trimCtl[i].above = 0u;
        `$INSTANCE_NAME`_trimCtl[i].below = 0u;
    }

    /* Enable PWMs */
    `$INSTANCE_NAME`_Control_En_Reg_Write(`$INSTANCE_NAME`_Control_En_Reg_Read() | `$INSTANCE_NAME`_CTRL_ENABLE);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables the component. Stops the PWMs.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  trim[x] outputs halted in undefined state. Use the pin-specific API 
*  PinName_SetDriveMode(PIN_DM_DIG_HIZ) to change the drive mode of the 
*  connected to these outputs pins to High Impedance Digital.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable PWMs */
    `$INSTANCE_NAME`_Control_En_Reg_Write(`$INSTANCE_NAME`_Control_En_Reg_Read() & ~`$INSTANCE_NAME`_CTRL_ENABLE);

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetMarginHighVoltage
********************************************************************************
*
* Summary:
*  Sets the margin high output voltage of the specified power converter.
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*  uint16 marginHiVoltage: Specifies the desired power converter output voltage
*                          in mV
*                          Valid range: 1..12,000
*
* Return:
*  None
*
* Global variables:
*  This overrides the vMarginHi[x] setting made in the customizer on the 
*  Voltages Tab and recalculate vMarginHighDutyCycle[x] to be ready for 
*  using by `$INSTANCE_NAME`_MarginHigh() macro
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetMarginHighVoltage(uint8 converterNum, uint16 marginHiVoltage)
                                    `=ReentrantKeil($INSTANCE_NAME . "_SetMarginHighVoltage")`
{
    /* Halt CPU in debug mode if converterNum is out of valid range */
    CYASSERT((converterNum <= `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && ((converterNum != 0u)));
    
    /* Convert converter number to buffer index */
    converterNum -= 1u;     
    /* Save margin voltage */
    `$INSTANCE_NAME`_vMarginHigh[converterNum] = marginHiVoltage;
    /* Recalculate duty cycle for new voltage */
    `$INSTANCE_NAME`_vMarginHighDutyCycle [converterNum] = `$INSTANCE_NAME`_DUTYCYCLE[converterNum] -
                         (((int32)marginHiVoltage - `$INSTANCE_NAME`_VNOMINAL[converterNum]) * 
                          `$INSTANCE_NAME`_DUTYCYCLES_PER_VOLT[converterNum] / 1000u);        
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetMarginHighVoltage
********************************************************************************
*
* Summary:
*  Returns the margin high output of the specified power converter
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*
* Return:
*  uint16: Specifies the desired power converter margin high output voltage in
*          mV
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetMarginHighVoltage(uint8 converterNum)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetMarginHighVoltage")`
{
    /* Halt CPU in debug mode if converterNum is out of valid range */
    CYASSERT((converterNum <= `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && ((converterNum != 0u)));

    return (`$INSTANCE_NAME`_vMarginHigh[converterNum - 1u]);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetMarginLowVoltage
********************************************************************************
*
* Summary:
*  Sets the margin low output voltage of the specified power converter.
*  This overrides the vMarginLo[x] setting made in the customizer on the Voltage
*  Tab. Note that calling this API does NOT cause any change in the PWM output 
*  duty cycle.
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*  uint16 marginLoVoltage: Specifies the desired power converter output margin
*                          low voltage in mV
*                          Valid range: 1..11,999
* Return:
*  None
*
* Global variables:
*  This overrides the vMarginLo[x] setting made in the customizer on the Margin
*  Tab and recalculate vMarginLowDutyCycle[x] to be ready for using by 
*  `$INSTANCE_NAME`_MarginLow() macro.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetMarginLowVoltage(uint8 converterNum, uint16 marginLoVoltage)
                                    `=ReentrantKeil($INSTANCE_NAME . "_SetMarginLowVoltage")`
{
    /* Halt CPU in debug mode if converterNum is out of valid range */
    CYASSERT((converterNum <= `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && ((converterNum != 0u)));

    /* Convert converter number to buffer index */
    converterNum -= 1u;     
    /* Save margin voltage */
    `$INSTANCE_NAME`_vMarginLow[converterNum] = marginLoVoltage;
    /* Recalculate duty cycle for new voltage */
    `$INSTANCE_NAME`_vMarginLowDutyCycle[converterNum] = `$INSTANCE_NAME`_DUTYCYCLE[converterNum] +
                    (((int32)`$INSTANCE_NAME`_VNOMINAL[converterNum] - marginLoVoltage) * 
                     `$INSTANCE_NAME`_DUTYCYCLES_PER_VOLT[converterNum]) / 1000u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetMarginLowVoltage
********************************************************************************
*
* Summary:
*  Returns the margin Low output of the specified power converter
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*
* Return:
*  uint16: Specifies the desired power converter margin low output voltage in
*          mV
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetMarginLowVoltage(uint8 converterNum)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetMarginLowVoltage")`
{
    /* Halt CPU in debug mode if converterNum is out of valid range */
    CYASSERT((converterNum <= `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && ((converterNum != 0u)));

    return (`$INSTANCE_NAME`_vMarginLow[converterNum - 1u]);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ActiveTrim
********************************************************************************
*
* Summary:
*  This API adjusts the PWM duty cycle of the specified power converter to get
*  the power converter actual voltage output closer to the desired voltage
*  output.
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*  uint16 actualVoltage: Specifies the current actual power converter output
*                        voltage reading in mV
*                        Valid range: 1..12,000
*  uint16 desiredVoltage: Specifies the desired power converter output voltage
*                         in mV
*                         Valid range: 1..12,000
*
* Return:
*  None
*
* Side Effects:
*  Calling this API may result in a change of PWM duty cycle driving the
*  control voltage of the selected power converter causing a change in power
*  converter output voltage.
*  If the desiredVoltage cannot be achieved because the PWM duty cycle is at
*  min or max level, the alert signal will be asserted until the alert
*  condition is removed, only possible by calling this API with an achievable
*  desiredVoltage.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ActiveTrim(uint8 converterNum, uint16 actualVoltage, uint16 desiredVoltage)
                                    `=ReentrantKeil($INSTANCE_NAME . "_ActiveTrim")`
{
    uint8 setAlert = `$INSTANCE_NAME`_FALSE;
    int16 deltaV;
    `$AlertSourceTypeReplacementString` alertSource = 0x01u;
    `$DutyCycleTypeReplacementString` currentDutyCycle;
    uint8 actualGTdesired = `$INSTANCE_NAME`_TRUE;      /* default actual > desired */

    /* Halt CPU in debug mode if converterNum is out of valid range */
    CYASSERT((converterNum <= `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS) && ((converterNum != 0u)));

    deltaV = (int16)actualVoltage - (int16)desiredVoltage;
    if (deltaV < 0)                                     /* if actual < desired */
    {
        deltaV = -deltaV;                               /* deltaV = abs(deltaV) */
        actualGTdesired = `$INSTANCE_NAME`_FALSE;       /* actual < desired */
    }

    /***************************************************************************
    * If deltaV is greater than half-bit of PWM,
    *  then adjust Above or Below statistic
    ***************************************************************************/
    if (deltaV > `$INSTANCE_NAME`_VDELTA[converterNum - 1u])
    {
        if (actualGTdesired == `$INSTANCE_NAME`_TRUE)
        {
            `$INSTANCE_NAME`_trimCtl[converterNum - 1u].above++;
        }
        else    /* actualGTdesired = FALSE   */
        {
            `$INSTANCE_NAME`_trimCtl[converterNum - 1u].below++;
        }

        /***************************************************************************
        * When gathered enough Above and/or Below statistics,
        * then maybe make a trim adjustment
        ***************************************************************************/
        if ((`$INSTANCE_NAME`_trimCtl[converterNum - 1u].above +
             `$INSTANCE_NAME`_trimCtl[converterNum - 1u].below) > `$INSTANCE_NAME`_TRIM_SLOW_PERIOD)
        {
            /***********************************************************************
            * If input is fairly quiet, then adjust trim
            ***********************************************************************/
            if ((`$INSTANCE_NAME`_trimCtl[converterNum - 1u].above < `$INSTANCE_NAME`_MIN_DELTA) ||
                (`$INSTANCE_NAME`_trimCtl[converterNum - 1u].below < `$INSTANCE_NAME`_MIN_DELTA))
            {
                /* Read current PWM duty cycle */
                currentDutyCycle = `$INSTANCE_NAME`_GetDutyCycle(converterNum);

                /***************************************************************************
                * If Above dominant, then DECREASE Vout
                ***************************************************************************/
                if (`$INSTANCE_NAME`_trimCtl[converterNum - 1u].above > `$INSTANCE_NAME`_MIN_DELTA)
                {
                    if (currentDutyCycle < `$INSTANCE_NAME`_PWM_MAX)
                    {
                        /* Increase PWM duty cycle to reduce actual voltage
                        *  with negative polarity schematic
                        */
                        currentDutyCycle += 1;
                        `$INSTANCE_NAME`_SetDutyCycle(converterNum, currentDutyCycle);
                    }
                    else
                    {
                        /* Set Alert */
                        setAlert = `$INSTANCE_NAME`_TRUE;
                    }
                }
                /***************************************************************
                * else Below dominant, so INCREASE Vout
                ***************************************************************/
                else
                {
                    if (currentDutyCycle > `$INSTANCE_NAME`_PWM_MIN)
                    {
                        /* Decrease PWM duty cycle to raise actual voltage
                        *  with negative polarity schematic
                        */
                        currentDutyCycle -= 1;
                        `$INSTANCE_NAME`_SetDutyCycle(converterNum, currentDutyCycle);
                    }
                    else
                    {
                        /* Set Alert */
                        setAlert = `$INSTANCE_NAME`_TRUE;
                    }
                }

                /*******************************************************************
                * Update Alert status as directed
                *******************************************************************/
                alertSource <<= (converterNum - 1u);    /* Mask for set/clear alert */
                if (setAlert != `$INSTANCE_NAME`_FALSE)
                {
                    if ((`$INSTANCE_NAME`_alertSource & alertSource) == (`$AlertSourceTypeReplacementString`)(0u))
                    {   /* Set Alert when it was not set before */
                        `$INSTANCE_NAME`_alertSource |= alertSource;
                        `$INSTANCE_NAME`_Control_En_Reg_Write(`$INSTANCE_NAME`_Control_En_Reg_Read() |
                                                              `$INSTANCE_NAME`_CTRL_ALERT);
                    }
                }
                else
                {
                    if ((`$INSTANCE_NAME`_alertSource & alertSource) != (`$AlertSourceTypeReplacementString`)(0u))
                    {   /* Clear Alert when it was set before but not more needed */
                        `$INSTANCE_NAME`_alertSource &= ~alertSource;
                        `$INSTANCE_NAME`_Control_En_Reg_Write(`$INSTANCE_NAME`_Control_En_Reg_Read() &
                                                             ~`$INSTANCE_NAME`_CTRL_ALERT);
                    }
                }
            }   /* endif Adjust Trim*/
            /***********************************************************************
            * Clear all statistics
            ***********************************************************************/
            `$INSTANCE_NAME`_trimCtl[converterNum - 1u].above = 0u;
            `$INSTANCE_NAME`_trimCtl[converterNum - 1u].below = 0u;
        }   /* endif gathered enough statistics */
    } /* If deltaV is greater than half-bit of PWM */    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetDutyCycle
********************************************************************************
*
* Summary:
*  Sets PWM duty cycle of the PWM associated with the specified power converter.
*  The PWM period is always fixed at the maximum value depending on the
*  resolutions set in the customizer.
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*  '$DutyCycleTypeReplacementString' dutyCycle:
*   Specifies the PWM duty cycle in PWM clock counts
*   Valid range: 0..255 or 0..1023 depending on the resolution set in the
*   customizer.
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetDutyCycle(uint8 converterNum, `$DutyCycleTypeReplacementString` dutyCycle)
                                    `=ReentrantKeil($INSTANCE_NAME . "_SetDutyCycle")`
{
    switch (converterNum)
    {
        case 1:
            `$INSTANCE_NAME`_TrimPWM_1_WriteCompare1(dutyCycle);
            break;
        case 2:
            `$INSTANCE_NAME`_TrimPWM_1_WriteCompare2(dutyCycle);
            break;
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 3)
        case 3:
            `$INSTANCE_NAME`_TrimPWM_2_WriteCompare1(dutyCycle);
            break;
        case 4:
            `$INSTANCE_NAME`_TrimPWM_2_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 5)
        case 5:
            `$INSTANCE_NAME`_TrimPWM_3_WriteCompare1(dutyCycle);
            break;
        case 6:
            `$INSTANCE_NAME`_TrimPWM_3_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 7)
        case 7:
            `$INSTANCE_NAME`_TrimPWM_4_WriteCompare1(dutyCycle);
            break;
        case 8:
            `$INSTANCE_NAME`_TrimPWM_4_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 9)
        case 9:
            `$INSTANCE_NAME`_TrimPWM_5_WriteCompare1(dutyCycle);
            break;
        case 10:
            `$INSTANCE_NAME`_TrimPWM_5_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 11)
        case 11:
            `$INSTANCE_NAME`_TrimPWM_6_WriteCompare1(dutyCycle);
            break;
        case 12:
            `$INSTANCE_NAME`_TrimPWM_6_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 13)
        case 13:
            `$INSTANCE_NAME`_TrimPWM_7_WriteCompare1(dutyCycle);
            break;
        case 14:
            `$INSTANCE_NAME`_TrimPWM_7_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 15)
        case 15:
            `$INSTANCE_NAME`_TrimPWM_8_WriteCompare1(dutyCycle);
            break;
        case 16:
            `$INSTANCE_NAME`_TrimPWM_8_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 17)
        case 17:
            `$INSTANCE_NAME`_TrimPWM_9_WriteCompare1(dutyCycle);
            break;
        case 18:
            `$INSTANCE_NAME`_TrimPWM_9_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 19)
        case 19:
            `$INSTANCE_NAME`_TrimPWM_10_WriteCompare1(dutyCycle);
            break;
        case 20:
            `$INSTANCE_NAME`_TrimPWM_10_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 21)
        case 21:
            `$INSTANCE_NAME`_TrimPWM_11_WriteCompare1(dutyCycle);
            break;
        case 22:
            `$INSTANCE_NAME`_TrimPWM_11_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 23)
        case 23:
            `$INSTANCE_NAME`_TrimPWM_12_WriteCompare1(dutyCycle);
            break;
        case 24:
            `$INSTANCE_NAME`_TrimPWM_12_WriteCompare2(dutyCycle);
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        default:
            /* Halt CPU in debug mode if converterNum is out of valid range */
            CYASSERT(0);
            break;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetDutyCycle
********************************************************************************
*
* Summary:
*  Gets the current PWM duty cycle of the PWM associated with the specified
*  power converter. Note that if the `$INSTANCE_NAME`_ActiveTrim() API is being
*  called regularly, the value returned should be expected to change over
*  time.
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
* Return:
*  '$DutyCycleTypeReplacementString':
*   Specifies the PWM duty cycle in PWM clock counts
*
*******************************************************************************/
`$DutyCycleTypeReplacementString` `$INSTANCE_NAME`_GetDutyCycle(uint8 converterNum)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetDutyCycle")`
{
    `$DutyCycleTypeReplacementString` dutyCycle = 0u;
    switch (converterNum)
    {
        case 1:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_1_ReadCompare1();
            break;
        case 2:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_1_ReadCompare2();
            break;
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 3)
        case 3:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_2_ReadCompare1();
            break;
        case 4:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_2_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 5)
        case 5:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_3_ReadCompare1();
            break;
        case 6:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_3_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 7)
        case 7:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_4_ReadCompare1();
            break;
        case 8:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_4_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 9)
        case 9:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_5_ReadCompare1();
            break;
        case 10:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_5_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 11)
        case 11:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_6_ReadCompare1();
            break;
        case 12:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_6_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 13)
        case 13:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_7_ReadCompare1();
            break;
        case 14:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_7_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 15)
        case 15:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_8_ReadCompare1();
            break;
        case 16:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_8_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 17)
        case 17:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_9_ReadCompare1();
            break;
        case 18:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_9_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 19)
        case 19:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_10_ReadCompare1();
            break;
        case 20:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_10_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 21)
        case 21:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_11_ReadCompare1();
            break;
        case 22:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_11_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        #if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 23)
        case 23:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_12_ReadCompare1();
            break;
        case 24:
            dutyCycle = `$INSTANCE_NAME`_TrimPWM_12_ReadCompare2();
            break;
        #endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
        default:
            /* Halt CPU in debug mode if converterNum is out of valid range */
            CYASSERT(0);
            break;
    }
    return (dutyCycle);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetAlertSource
********************************************************************************
*
* Summary:
*  Returns a bit mask indicating which PWMs are generating an alert.
*
* Parameters:
*  None
*
* Return:
*  '$AlertSourceTypeReplacementString':
*  Bit Field Alert Source
*  0         1 - Failure to achieve power converter regulation on trim output 1
*  1         1 - Failure to achieve power converter regulation on trim output 2
*  ...
*  23        1 - Failure to achieve power converter regulation on trim output 24
*
*******************************************************************************/
`$AlertSourceTypeReplacementString` `$INSTANCE_NAME`_GetAlertSource(void)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetAlertSource")`
{
    return (`$INSTANCE_NAME`_alertSource);
}


/* [] END OF FILE */
