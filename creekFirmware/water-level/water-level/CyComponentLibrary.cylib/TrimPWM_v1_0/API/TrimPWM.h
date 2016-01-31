/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the TrimPWM
*  Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(`$INSTANCE_NAME`_H)
#define `$INSTANCE_NAME`_H

#include "cyfitter.h"


/***************************************
*    Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_RESOLUTION             (`$Resolution`u)


/***************************************
*              Registers
***************************************/

#if (`$INSTANCE_NAME`_RESOLUTION  == 8u)    /* 8bit - PWM */
    #define `$INSTANCE_NAME`_COUNTER_PTR        (  (reg8 *) `$INSTANCE_NAME`_PWM8_PWM8dp_u0__A0_REG)
    #define `$INSTANCE_NAME`_COMPARE1_PTR       (  (reg8 *) `$INSTANCE_NAME`_PWM8_PWM8dp_u0__D0_REG)
    #define `$INSTANCE_NAME`_COMPARE2_PTR       (  (reg8 *) `$INSTANCE_NAME`_PWM8_PWM8dp_u0__D1_REG)
#else /* 9 - 16bit - PWM */
    #define `$INSTANCE_NAME`_COUNTER_PTR        (  (reg16 *) `$INSTANCE_NAME`_PWM16_PWM16dp_u0__A0_REG)
    #define `$INSTANCE_NAME`_COMPARE1_PTR       (  (reg16 *) `$INSTANCE_NAME`_PWM16_PWM16dp_u0__D0_REG)
    #define `$INSTANCE_NAME`_COMPARE2_PTR       (  (reg16 *) `$INSTANCE_NAME`_PWM16_PWM16dp_u0__D1_REG)
#endif  /* RESOLUTION */


/***************************************
*        Function Prototypes
***************************************/
void `$INSTANCE_NAME`_Start(void);


/*******************************************************************************
* Macro: `$INSTANCE_NAME`_WriteCompare1/`$INSTANCE_NAME`_WriteCompare2
********************************************************************************
*
* Summary:
*  These macros are used to change the compare1/compare2 values. The compare
*  output will reflect the new value on the next UDB clock. The appropriate
*  compare output will be driven high when the present counter value is less
*  than compare register.
*
* Parameters:
*  compare:  New compare value.
*
* Return:
*  void
*
*******************************************************************************/
#if (`$INSTANCE_NAME`_RESOLUTION  == 8u)    /* 8bit - PWM */
    #define `$INSTANCE_NAME`_WriteCompare1(x)   CY_SET_REG8(`$INSTANCE_NAME`_COMPARE1_PTR, x)
    #define `$INSTANCE_NAME`_WriteCompare2(x)   CY_SET_REG8(`$INSTANCE_NAME`_COMPARE2_PTR, x)
#else  /* 9 - 16bit - PWM */
    #define `$INSTANCE_NAME`_WriteCompare1(x)   CY_SET_REG16(`$INSTANCE_NAME`_COMPARE1_PTR, x)
    #define `$INSTANCE_NAME`_WriteCompare2(x)   CY_SET_REG16(`$INSTANCE_NAME`_COMPARE2_PTR, x)
#endif  /* RESOLUTION */


/*******************************************************************************
* Macro: `$INSTANCE_NAME`_ReadCompare1/`$INSTANCE_NAME`_ReadCompare2
********************************************************************************
*
* Summary:
*  These macros are used to get the compare1/compare2 values.
*
* Parameters:
*  None
*
* Return:
*  Compare value.
*
*******************************************************************************/
#if (`$INSTANCE_NAME`_RESOLUTION  == 8u)    /* 8bit - PWM */
    #define `$INSTANCE_NAME`_ReadCompare1()   CY_GET_REG8(`$INSTANCE_NAME`_COMPARE1_PTR)
    #define `$INSTANCE_NAME`_ReadCompare2()   CY_GET_REG8(`$INSTANCE_NAME`_COMPARE2_PTR)
#else  /* 9 - 16bit - PWM */
    #define `$INSTANCE_NAME`_ReadCompare1()   CY_GET_REG16(`$INSTANCE_NAME`_COMPARE1_PTR)
    #define `$INSTANCE_NAME`_ReadCompare2()   CY_GET_REG16(`$INSTANCE_NAME`_COMPARE2_PTR)
#endif  /* RESOLUTION */


/***************************************
*       Constants
***************************************/

#define `$INSTANCE_NAME`_INIT_DUTY          (0x01u << (`$INSTANCE_NAME`_RESOLUTION - 1))


#endif /* #if !defined(`$INSTANCE_NAME`_H) */

/* [] END OF FILE */
