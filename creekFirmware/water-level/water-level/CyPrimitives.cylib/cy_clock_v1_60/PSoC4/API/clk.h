/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function and constant definitions for the clock component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_CLOCK_`$INSTANCE_NAME`_H)
#define CY_CLOCK_`$INSTANCE_NAME`_H

#include <cytypes.h>
#include <cyfitter.h>


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);

void `$INSTANCE_NAME`_SetDividerRegister(uint16 clkDivider);
uint16 `$INSTANCE_NAME`_GetDividerRegister(void);

/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_DIV_REG    (*(reg32 *)`$INSTANCE_NAME`__ENABLE)
#define `$INSTANCE_NAME`_ENABLE_REG `$INSTANCE_NAME`_DIV_REG

#endif /* !defined(CY_CLOCK_`$INSTANCE_NAME`_H) */

/* [] END OF FILE */
