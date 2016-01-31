/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*   This file provides the source code to the API for the Analog Buffer 
*   User Module.
*
* NOTE:
* 
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

uint8  `$INSTANCE_NAME`_initVar = 0;

/*******************************************************************************
* Funciton Name:   `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  The start function initializes the Analog Buffer with the default values, and 
*  sets the power to the given level.  A power level of 0, is the same as 
*  executing the stop function.
*
* Parameters: 
*
* Return:  
*    (void)
*
* Theory:
*
* Side Effects:
*
**********************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
   if(`$INSTANCE_NAME`_initVar == 0)
   {
      `$INSTANCE_NAME`_initVar = 1;
      `$INSTANCE_NAME`_SetPower(`$INSTANCE_NAME`_DEFAULT_POWER);

      /* Set follower mode if selected */
      `$INSTANCE_NAME`_SW |= `$INSTANCE_NAME`_DEFAULT_MODE;
   }

   /*Enable negative charge pumps in ANIF*/
   `$INSTANCE_NAME`_PUMP_CR1  |= (`$INSTANCE_NAME`_PUMP_CR1_CLKSEL | `$INSTANCE_NAME`_PUMP_CR1_FORCE );

   /* Enable power to buffer */
   `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;

}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Powers down amplifier to lowest power state.
*
* Parameters:  
*   (void)
*
* Return:  
*   (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /*Disable negative charge pumps for ANIF only if the one ABuf is turned ON*/
    if(`$INSTANCE_NAME`_PWRMGR == `$INSTANCE_NAME`_ACT_PWR_EN)
    {
        `$INSTANCE_NAME`_PUMP_CR1 &= ~(`$INSTANCE_NAME`_PUMP_CR1_CLKSEL | `$INSTANCE_NAME`_PUMP_CR1_FORCE );
    }

   /* Disable power to buffer */
   `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
}

/*******************************************************************************
* Funciton Name:   `$INSTANCE_NAME`_SetPower
********************************************************************************
*
* Summary:
*  Sets power level of Analog buffer.
*
* Parameters: 
*    power:   Sets power level between low (1) and high power (3).
*
* Return:  
*    (void)
*
* Theory:
*
* Side Effects:
*
**********************************************************************************/
void `$INSTANCE_NAME`_SetPower(uint8 power) 
{
      `$INSTANCE_NAME`_CR &= ~`$INSTANCE_NAME`_PWR_MASK;             /* Clear power setting  */
      `$INSTANCE_NAME`_CR |= ( power & `$INSTANCE_NAME`_PWR_MASK);   /* Set device power     */
}
/* [] END OF FILE */


