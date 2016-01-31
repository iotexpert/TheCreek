/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Contains the function prototypes and constants available to the TrimMargin
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/


#include "cytypes.h"
#include "CyLib.h"

#if !defined(`$INSTANCE_NAME`_H)
#define `$INSTANCE_NAME`_H


/***************************************
* Conditional Compilation Parameters
***************************************/
#define `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS       (`$NumConverters`u)
#define `$INSTANCE_NAME`_PWM_RESOLUTION             (`$PWMResolution`u)


#include "`$INSTANCE_NAME`_TrimPWM_1.h"
#include "`$INSTANCE_NAME`_Control_En_Reg.h"
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 3)
    #include "`$INSTANCE_NAME`_TrimPWM_2.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 5)
    #include "`$INSTANCE_NAME`_TrimPWM_3.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 7)
    #include "`$INSTANCE_NAME`_TrimPWM_4.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 9)
    #include "`$INSTANCE_NAME`_TrimPWM_5.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 11)
    #include "`$INSTANCE_NAME`_TrimPWM_6.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 13)
    #include "`$INSTANCE_NAME`_TrimPWM_7.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 15)
    #include "`$INSTANCE_NAME`_TrimPWM_8.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 17)
    #include "`$INSTANCE_NAME`_TrimPWM_9.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 19)
    #include "`$INSTANCE_NAME`_TrimPWM_10.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 21)
    #include "`$INSTANCE_NAME`_TrimPWM_11.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS >= 23)
    #include "`$INSTANCE_NAME`_TrimPWM_12.h"
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS */


/***************************************
*      Data Struct Definition
***************************************/
typedef struct _`$INSTANCE_NAME`_TRIMCTL_STRUCT
{
    uint8 above;        /* Cumulative ADC readings ABOVE rawMax     */
    uint8 below;        /* Cumulative ADC readings BELOW rawMin     */
} `$INSTANCE_NAME`_TRIMCTL_STRUCT;


/***************************************
*       Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_SetMarginHighVoltage(uint8 converterNum, uint16 marginHiVoltage)
                                    `=ReentrantKeil($INSTANCE_NAME . "_SetMarginHighVoltage")`;
uint16 `$INSTANCE_NAME`_GetMarginHighVoltage(uint8 converterNum)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetMarginHighVoltage")`;
void `$INSTANCE_NAME`_SetMarginLowVoltage(uint8 converterNum, uint16 marginLoVoltage)
                                    `=ReentrantKeil($INSTANCE_NAME . "_SetMarginLowVoltage")`;
uint16 `$INSTANCE_NAME`_GetMarginLowVoltage(uint8 converterNum)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetMarginLowVoltage")`;
void `$INSTANCE_NAME`_ActiveTrim(uint8 converterNum, uint16 actualVoltage, uint16 desiredVoltage)
                                    `=ReentrantKeil($INSTANCE_NAME . "_ActiveTrim")`;
void `$INSTANCE_NAME`_SetDutyCycle(uint8 converterNum, `$DutyCycleTypeReplacementString` dutyCycle)
                                    `=ReentrantKeil($INSTANCE_NAME . "_SetDutyCycle")`;
`$DutyCycleTypeReplacementString` `$INSTANCE_NAME`_GetDutyCycle(uint8 converterNum)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetDutyCycle")`;
`$AlertSourceTypeReplacementString` `$INSTANCE_NAME`_GetAlertSource(void)
                                    `=ReentrantKeil($INSTANCE_NAME . "_GetAlertSource")`;


/***************************************
* External references
***************************************/
extern `$DutyCycleTypeReplacementString` `$INSTANCE_NAME`_vMarginHighDutyCycle[];
extern `$DutyCycleTypeReplacementString` `$INSTANCE_NAME`_vMarginLowDutyCycle[];
extern const `$DutyCycleTypeReplacementString` CYCODE `$INSTANCE_NAME`_DUTYCYCLE[];
extern const `$DutyCycleTypeReplacementString` CYCODE `$INSTANCE_NAME`_PRE_RUN_DUTYCYCLE[];
extern const uint16 CYCODE `$INSTANCE_NAME`_VNOMINAL[];


/*******************************************************************************
* Macro: `$INSTANCE_NAME`_MarginHigh/`$INSTANCE_NAME`_MarginLow
********************************************************************************
*
* Summary:
*  Sets the selected power converter output voltage to the desired Margin Low 
*  setting as specified in the Voltages Tab of the customizer or as per the 
*  `$INSTANCE_NAME`_SetMarginLowVoltage() API. 
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*
* Return:
*  void
*
*******************************************************************************/
#define `$INSTANCE_NAME`_MarginHigh(n) `$INSTANCE_NAME`_SetDutyCycle(n, `$INSTANCE_NAME`_vMarginHighDutyCycle[n-1])
#define `$INSTANCE_NAME`_MarginLow(n)  `$INSTANCE_NAME`_SetDutyCycle(n, `$INSTANCE_NAME`_vMarginLowDutyCycle[n-1])


/*******************************************************************************
* Macro: `$INSTANCE_NAME`_SetNominal
********************************************************************************
*
* Summary:
*  Sets the selected power converter output voltage to the Nominal Voltage 
*  setting as specified in the Voltages Tab of the customizer.
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*
* Return:
*  void
*
*******************************************************************************/
#define `$INSTANCE_NAME`_SetNominal(n) `$INSTANCE_NAME`_SetDutyCycle(n, `$INSTANCE_NAME`_DUTYCYCLE[n-1])


/*******************************************************************************
* Macro: `$INSTANCE_NAME`_SetPreRun
********************************************************************************
*
* Summary:
*  Sets the recalculated PWM duty cycle BEFORE power converter is enabled.
*  It is used to archive VADJ voltage with the assumption that
*  the R1 is grounded in parallel with R2.
*
* Parameters:
*  uint8 converterNum: Specifies the power converter number
*                      Valid range: 1..24
*
* Return:
*  void
*
*******************************************************************************/
#define `$INSTANCE_NAME`_SetPreRun(n)  `$INSTANCE_NAME`_SetDutyCycle(n, `$INSTANCE_NAME`_PRE_RUN_DUTYCYCLE[n-1])


/***************************************
*          API Constants
***************************************/
#define `$INSTANCE_NAME`_MAX_NUMBER_OF_CONVERTERS       (24u)
#define `$INSTANCE_NAME`_PWM_MIN                        (0u)
#define `$INSTANCE_NAME`_PWM_MAX                        (0xFFFFu >> (16u - `$INSTANCE_NAME`_PWM_RESOLUTION))

#define `$INSTANCE_NAME`_POLARITY_POSITIVE              (0u)
#define `$INSTANCE_NAME`_POLARITY_NEGATIVE              (1u)

#define `$INSTANCE_NAME`_FALSE                          (0u)
#define `$INSTANCE_NAME`_TRUE                           (1u)


/*******************************************************************************
*
* `$INSTANCE_NAME`_TRIM_SLOW_PERIOD - # of ADC samples before trim PWM
*  adjustment.
*
*  This slows-down the trim process, such that it compensates initial and
*  long-term regulator drift and associated component drift.  This should be
*  long enough to avoid interacting with the regulator's high-speed transient
*  response to dynamic changes in Vin and Iout.
*
*  Units: ADC sample counts
*  (typically 1 to 3 mS per count, depends on ADC cycle scan rate and rail qty)
*******************************************************************************/
#define `$INSTANCE_NAME`_TRIM_SLOW_PERIOD               (20u)


/*******************************************************************************
*
* `$INSTANCE_NAME`_MIN_DELTA -
*  The minimum delta between difference in quantity of measurements that are
*  ABOVE and BELOW the ideal nominal rail voltage before decide to adjust the
*  PWM trim.
*
*  If a rail is noisy, the quantity of measurements ABOVE and BELOW each be
*  large, yet have a similar magnitude and neither will "dominate" (i.e. exceed
*  MIN_DELTA), so no adjustment would made.
*******************************************************************************/
/* A 25% (0.25) majority seems like a reasonable decisive majority  */
#define `$INSTANCE_NAME`_TRIM_MIN_DELTA_FACTOR         0.25         /* Ratio (e.g. 0.25 is 25%)  */
#define `$INSTANCE_NAME`_MIN_DELTA                     ((uint16)(`$INSTANCE_NAME`_TRIM_SLOW_PERIOD * \
                                                       `$INSTANCE_NAME`_TRIM_MIN_DELTA_FACTOR))

/***************************************
*       Register Constants
***************************************/

/* Control Enable Register Bit Masks */
#define `$INSTANCE_NAME`_CTRL_ENABLE                (0x01u)
#define `$INSTANCE_NAME`_CTRL_ALERT                 (0x02u)

#endif  /* `$INSTANCE_NAME`_H */


/* [] END OF FILE */
