/*******************************************************************************
* File Name: i2c_SCBCLK.h
* Version 2.0
*
*  Description:
*   Provides the function and constant definitions for the clock component.
*
*  Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CLOCK_i2c_SCBCLK_H)
#define CY_CLOCK_i2c_SCBCLK_H

#include <cytypes.h>
#include <cyfitter.h>


/***************************************
*        Function Prototypes
***************************************/

void i2c_SCBCLK_Start(void);
void i2c_SCBCLK_Stop(void);

void i2c_SCBCLK_SetFractionalDividerRegister(uint16 clkDivider, uint8 clkFractional);

uint16 i2c_SCBCLK_GetDividerRegister(void);
uint8  i2c_SCBCLK_GetFractionalDividerRegister(void);

#define i2c_SCBCLK_Enable()                         i2c_SCBCLK_Start()
#define i2c_SCBCLK_Disable()                        i2c_SCBCLK_Stop()
#define i2c_SCBCLK_SetDividerRegister(clkDivider, reset)  \
                        i2c_SCBCLK_SetFractionalDividerRegister((clkDivider), 0)
#define i2c_SCBCLK_SetDivider(clkDivider)           i2c_SCBCLK_SetDividerRegister((clkDivider), 1)
#define i2c_SCBCLK_SetDividerValue(clkDivider)      i2c_SCBCLK_SetDividerRegister((clkDivider) - 1, 1)


/***************************************
*             Registers
***************************************/

#define i2c_SCBCLK_DIV_REG    (*(reg32 *)i2c_SCBCLK__REGISTER)
#define i2c_SCBCLK_ENABLE_REG i2c_SCBCLK_DIV_REG

#endif /* !defined(CY_CLOCK_i2c_SCBCLK_H) */

/* [] END OF FILE */
