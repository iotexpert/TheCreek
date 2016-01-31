/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This file provides the source code to the API for the clock component.
*
*  Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include <cydevice_trm.h>
#include "`$INSTANCE_NAME`.h"

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Starts the clock.
*
* Parameters:
*  void
*
* Returns:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    /* Set the bit to enable the clock. */
    `$INSTANCE_NAME`_ENABLE_REG |= `$INSTANCE_NAME`__MASK;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Stops the clock and returns immediately. This API does not require the
*  source clock to be running but may return before the hardware is actually
*  disabled.
*
* Parameters:
*  void
*
* Returns:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    /* Clear the bit to disable the clock. */
    `$INSTANCE_NAME`_ENABLE_REG &= ~`$INSTANCE_NAME`__MASK;
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetDividerRegister
********************************************************************************
* Summary:
*  Modifies the clock divider and, thus, the frequency.
*
* Parameters:
*  clkDivider:  Divider register value (0-65536). This value is NOT the
*    divider; the clock hardware divides by clkDivider plus one. For example,
*    to divide the clock by 2, this parameter should be set to 1.
* Returns:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetDividerRegister(uint16 clkDivider)
{
    uint32 maskVal = `$INSTANCE_NAME`_DIV_REG & `$INSTANCE_NAME`__MASK; // get enable state
    uint32 regVal = clkDivider | maskVal; // combine mask and new divider val into 32-bit value
	`$INSTANCE_NAME`_DIV_REG = regVal;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetDividerRegister
********************************************************************************
* Summary:
*  Gets the clock divider register value.
*
* Parameters:
*  void
*
* Returns:
*  Divide value of the clock minus 1. For example, if the clock is set to
*  divide by 2, the return value will be 1.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetDividerRegister(void)
{
    return `$INSTANCE_NAME`_DIV_REG;
}

/* [] END OF FILE */
