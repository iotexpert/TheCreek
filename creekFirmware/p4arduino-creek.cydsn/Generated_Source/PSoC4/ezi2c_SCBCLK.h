/*******************************************************************************
* File Name: ezi2c_SCBCLK.h
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

#if !defined(CY_CLOCK_ezi2c_SCBCLK_H)
#define CY_CLOCK_ezi2c_SCBCLK_H

#include <cytypes.h>
#include <cyfitter.h>


/***************************************
*        Function Prototypes
***************************************/
#if defined CYREG_PERI_DIV_CMD

void ezi2c_SCBCLK_StartEx(uint32 alignClkDiv);
#define ezi2c_SCBCLK_Start() \
    ezi2c_SCBCLK_StartEx(ezi2c_SCBCLK__PA_DIV_ID)

#else

void ezi2c_SCBCLK_Start(void);

#endif/* CYREG_PERI_DIV_CMD */

void ezi2c_SCBCLK_Stop(void);

void ezi2c_SCBCLK_SetFractionalDividerRegister(uint16 clkDivider, uint8 clkFractional);

uint16 ezi2c_SCBCLK_GetDividerRegister(void);
uint8  ezi2c_SCBCLK_GetFractionalDividerRegister(void);

#define ezi2c_SCBCLK_Enable()                         ezi2c_SCBCLK_Start()
#define ezi2c_SCBCLK_Disable()                        ezi2c_SCBCLK_Stop()
#define ezi2c_SCBCLK_SetDividerRegister(clkDivider, reset)  \
    ezi2c_SCBCLK_SetFractionalDividerRegister((clkDivider), 0u)
#define ezi2c_SCBCLK_SetDivider(clkDivider)           ezi2c_SCBCLK_SetDividerRegister((clkDivider), 1u)
#define ezi2c_SCBCLK_SetDividerValue(clkDivider)      ezi2c_SCBCLK_SetDividerRegister((clkDivider) - 1u, 1u)


/***************************************
*             Registers
***************************************/
#if defined CYREG_PERI_DIV_CMD

#define ezi2c_SCBCLK_DIV_ID     ezi2c_SCBCLK__DIV_ID

#define ezi2c_SCBCLK_CMD_REG    (*(reg32 *)CYREG_PERI_DIV_CMD)
#define ezi2c_SCBCLK_CTRL_REG   (*(reg32 *)ezi2c_SCBCLK__CTRL_REGISTER)
#define ezi2c_SCBCLK_DIV_REG    (*(reg32 *)ezi2c_SCBCLK__DIV_REGISTER)

#define ezi2c_SCBCLK_CMD_DIV_SHIFT          (0u)
#define ezi2c_SCBCLK_CMD_PA_DIV_SHIFT       (8u)
#define ezi2c_SCBCLK_CMD_DISABLE_SHIFT      (30u)
#define ezi2c_SCBCLK_CMD_ENABLE_SHIFT       (31u)

#define ezi2c_SCBCLK_CMD_DISABLE_MASK       ((uint32)((uint32)1u << ezi2c_SCBCLK_CMD_DISABLE_SHIFT))
#define ezi2c_SCBCLK_CMD_ENABLE_MASK        ((uint32)((uint32)1u << ezi2c_SCBCLK_CMD_ENABLE_SHIFT))

#define ezi2c_SCBCLK_DIV_FRAC_MASK  (0x000000F8u)
#define ezi2c_SCBCLK_DIV_FRAC_SHIFT (3u)
#define ezi2c_SCBCLK_DIV_INT_MASK   (0xFFFFFF00u)
#define ezi2c_SCBCLK_DIV_INT_SHIFT  (8u)

#else 

#define ezi2c_SCBCLK_DIV_REG        (*(reg32 *)ezi2c_SCBCLK__REGISTER)
#define ezi2c_SCBCLK_ENABLE_REG     ezi2c_SCBCLK_DIV_REG
#define ezi2c_SCBCLK_DIV_FRAC_MASK  ezi2c_SCBCLK__FRAC_MASK
#define ezi2c_SCBCLK_DIV_FRAC_SHIFT (16u)
#define ezi2c_SCBCLK_DIV_INT_MASK   ezi2c_SCBCLK__DIVIDER_MASK
#define ezi2c_SCBCLK_DIV_INT_SHIFT  (0u)

#endif/* CYREG_PERI_DIV_CMD */

#endif /* !defined(CY_CLOCK_ezi2c_SCBCLK_H) */

/* [] END OF FILE */
