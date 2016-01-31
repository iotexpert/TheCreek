/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for OpAmp (Analog Buffer) 
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

#include "`$INSTANCE_NAME`.h"
#include <CyLib.h>

uint8 `$INSTANCE_NAME`_initVar = 0u;


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
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    `$INSTANCE_NAME`_SetPower(`$INSTANCE_NAME`_DEFAULT_POWER);
}


/*******************************************************************************   
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the OpAmp block operation
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* Enable negative charge pumps in ANIF */
    `$INSTANCE_NAME`_PUMP_CR1_REG  |= (`$INSTANCE_NAME`_PUMP_CR1_CLKSEL | `$INSTANCE_NAME`_PUMP_CR1_FORCE);

    /* Enable power to buffer in active mode */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACT_PWR_EN;

    /* Enable power to buffer in alternative active mode */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;
}


/*******************************************************************************
* Function Name:   `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the Analog Buffer with the default values and 
*  sets the power to the given level. A power level of 0, is same as 
*  executing the stop function.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_initVar: Used to check the initial configuration, modified 
*  when this function is called for the first time.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_initVar = 1u;
        `$INSTANCE_NAME`_Init();
    }

    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Powers down amplifier to lowest power state.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable negative charge pumps for ANIF only if one ABuf is turned ON */
    if(`$INSTANCE_NAME`_PM_ACT_CFG_REG == `$INSTANCE_NAME`_ACT_PWR_EN)
    {
        `$INSTANCE_NAME`_PUMP_CR1_REG &= ~(`$INSTANCE_NAME`_PUMP_CR1_CLKSEL | `$INSTANCE_NAME`_PUMP_CR1_FORCE);
    }
    
    /* Disable power to buffer in active mode template */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG &= ~`$INSTANCE_NAME`_ACT_PWR_EN;

    /* Disable power to buffer in alternative active mode template */
    `$INSTANCE_NAME`_PM_STBY_CFG_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPower
********************************************************************************
*
* Summary:
*  Sets power level of Analog buffer.
*
* Parameters: 
*  power: PSoC3: Sets power level between low (1) and high power (3).
*         PSoC5: Sets power level High (0)
*
* Return:
*  void
*
**********************************************************************************/
void `$INSTANCE_NAME`_SetPower(uint8 power) `=ReentrantKeil($INSTANCE_NAME . "_SetPower")`
{
    /* Only High power can be used in PSoC5 */
    #if CY_PSOC5A
        CYASSERT(power == `$INSTANCE_NAME`_HIGHPOWER);
    #endif   /* (CY_PSOC5A) */

    `$INSTANCE_NAME`_CR_REG = ((`$INSTANCE_NAME`_CR_REG & ~`$INSTANCE_NAME`_PWR_MASK) | 
                               ( power & `$INSTANCE_NAME`_PWR_MASK));   /* Set device power */
}


/* [] END OF FILE */
