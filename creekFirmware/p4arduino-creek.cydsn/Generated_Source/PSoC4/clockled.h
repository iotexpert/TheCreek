/*******************************************************************************
* File Name: clockled.h
* Version 2.20
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

#if !defined(CY_CLOCK_clockled_H)
#define CY_CLOCK_clockled_H

#include <cytypes.h>
#include <cyfitter.h>


/***************************************
*        Function Prototypes
***************************************/
#if defined CYREG_PERI_DIV_CMD

void clockled_StartEx(uint32 alignClkDiv);
#define clockled_Start() \
    clockled_StartEx(clockled__PA_DIV_ID)

#else

void clockled_Start(void);

#endif/* CYREG_PERI_DIV_CMD */

void clockled_Stop(void);

void clockled_SetFractionalDividerRegister(uint16 clkDivider, uint8 clkFractional);

uint16 clockled_GetDividerRegister(void);
uint8  clockled_GetFractionalDividerRegister(void);

#define clockled_Enable()                         clockled_Start()
#define clockled_Disable()                        clockled_Stop()
#define clockled_SetDividerRegister(clkDivider, reset)  \
    clockled_SetFractionalDividerRegister((clkDivider), 0u)
#define clockled_SetDivider(clkDivider)           clockled_SetDividerRegister((clkDivider), 1u)
#define clockled_SetDividerValue(clkDivider)      clockled_SetDividerRegister((clkDivider) - 1u, 1u)


/***************************************
*             Registers
***************************************/
#if defined CYREG_PERI_DIV_CMD

#define clockled_DIV_ID     clockled__DIV_ID

#define clockled_CMD_REG    (*(reg32 *)CYREG_PERI_DIV_CMD)
#define clockled_CTRL_REG   (*(reg32 *)clockled__CTRL_REGISTER)
#define clockled_DIV_REG    (*(reg32 *)clockled__DIV_REGISTER)

#define clockled_CMD_DIV_SHIFT          (0u)
#define clockled_CMD_PA_DIV_SHIFT       (8u)
#define clockled_CMD_DISABLE_SHIFT      (30u)
#define clockled_CMD_ENABLE_SHIFT       (31u)

#define clockled_CMD_DISABLE_MASK       ((uint32)((uint32)1u << clockled_CMD_DISABLE_SHIFT))
#define clockled_CMD_ENABLE_MASK        ((uint32)((uint32)1u << clockled_CMD_ENABLE_SHIFT))

#define clockled_DIV_FRAC_MASK  (0x000000F8u)
#define clockled_DIV_FRAC_SHIFT (3u)
#define clockled_DIV_INT_MASK   (0xFFFFFF00u)
#define clockled_DIV_INT_SHIFT  (8u)

#else 

#define clockled_DIV_REG        (*(reg32 *)clockled__REGISTER)
#define clockled_ENABLE_REG     clockled_DIV_REG
#define clockled_DIV_FRAC_MASK  clockled__FRAC_MASK
#define clockled_DIV_FRAC_SHIFT (16u)
#define clockled_DIV_INT_MASK   clockled__DIVIDER_MASK
#define clockled_DIV_INT_SHIFT  (0u)

#endif/* CYREG_PERI_DIV_CMD */

#endif /* !defined(CY_CLOCK_clockled_H) */

/* [] END OF FILE */
