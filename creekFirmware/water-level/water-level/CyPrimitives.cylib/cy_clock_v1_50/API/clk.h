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

void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME ."_Start")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME ."_Stop")`;

#if(!(CYDEV_CHIP_FAMILY_USED == CYDEV_CHIP_FAMILY_PSOC3 && \
    CYDEV_CHIP_REVISION_USED == CYDEV_CHIP_REVISION_3A_ES2) && \
	!(CYDEV_CHIP_FAMILY_USED == CYDEV_CHIP_FAMILY_PSOC5 && \
	CYDEV_CHIP_REVISION_USED == CYDEV_CHIP_REVISION_5A_ES1))
void `$INSTANCE_NAME`_StopBlock(void) `=ReentrantKeil($INSTANCE_NAME ."_StopBlock")`;
#endif

void `$INSTANCE_NAME`_StandbyPower(uint8 state) `=ReentrantKeil($INSTANCE_NAME ."_StandbyPower")`;
void `$INSTANCE_NAME`_SetDividerRegister(uint16 clkDivider, uint8 reset) `=ReentrantKeil($INSTANCE_NAME ."_SetDividerRegister")`;
uint16 `$INSTANCE_NAME`_GetDividerRegister(void) `=ReentrantKeil($INSTANCE_NAME ."_GetDividerRegister")`;
void `$INSTANCE_NAME`_SetModeRegister(uint8 modeBitMask) `=ReentrantKeil($INSTANCE_NAME ."_SetModeRegister")`;
void `$INSTANCE_NAME`_ClearModeRegister(uint8 modeBitMask) `=ReentrantKeil($INSTANCE_NAME ."_ClearModeRegister")`;
uint8 `$INSTANCE_NAME`_GetModeRegister(void) `=ReentrantKeil($INSTANCE_NAME ."_GetModeRegister")`;
void `$INSTANCE_NAME`_SetSourceRegister(uint8 clkSource) `=ReentrantKeil($INSTANCE_NAME ."_GetSourceRegister")`;
uint8 `$INSTANCE_NAME`_GetSourceRegister(void) `=ReentrantKeil($INSTANCE_NAME ."_GetSourceRegister")`;
#if defined(`$INSTANCE_NAME`__CFG3)
void `$INSTANCE_NAME`_SetPhaseRegister(uint8 clkPhase) `=ReentrantKeil($INSTANCE_NAME ."_SetPhaseRegister")`;
uint8 `$INSTANCE_NAME`_GetPhaseRegister(void) `=ReentrantKeil($INSTANCE_NAME ."_GetPhaseRegister")`;
#endif

#define `$INSTANCE_NAME`_Enable()                       `$INSTANCE_NAME`_Start()
#define `$INSTANCE_NAME`_Disable()                      `$INSTANCE_NAME`_Stop()
#define `$INSTANCE_NAME`_SetDivider(clkDivider)         `$INSTANCE_NAME`_SetDividerRegister(clkDivider, 1)
#define `$INSTANCE_NAME`_SetDividerValue(clkDivider)    `$INSTANCE_NAME`_SetDividerRegister((clkDivider) - 1, 1)
#define `$INSTANCE_NAME`_SetMode(clkMode)               `$INSTANCE_NAME`_SetModeRegister(clkMode)
#define `$INSTANCE_NAME`_SetSource(clkSource)           `$INSTANCE_NAME`_SetSourceRegister(clkSource)
#if defined(`$INSTANCE_NAME`__CFG3)
#define `$INSTANCE_NAME`_SetPhase(clkPhase)             `$INSTANCE_NAME`_SetPhaseRegister(clkPhase)
#define `$INSTANCE_NAME`_SetPhaseValue(clkPhase)        `$INSTANCE_NAME`_SetPhaseRegister((clkPhase) + 1)
#endif


/***************************************
*           API Constants
***************************************/

/* Constants SetPhase(), clkPhase parameter. Only valid for PSoC 3 ES2 and earlier. See datasheet for details. */
#if CYDEV_CHIP_FAMILY_USED == CYDEV_CHIP_FAMILY_PSOC3 && \
   (CYDEV_CHIP_REVISION_USED == CYDEV_CHIP_REVISION_3A_ES1 || \
    CYDEV_CHIP_REVISION_USED == CYDEV_CHIP_REVISION_3A_ES2)
#define CYCLK_2_5NS             0x01 /* 2.5 ns delay. */
#define CYCLK_3_5NS             0x02 /* 3.5 ns delay. */
#define CYCLK_4_5NS             0x03 /* 4.5 ns delay. */
#define CYCLK_5_5NS             0x04 /* 5.5 ns delay. */
#define CYCLK_6_5NS             0x05 /* 6.5 ns delay. */
#define CYCLK_7_5NS             0x06 /* 7.5 ns delay. */
#define CYCLK_8_5NS             0x07 /* 8.5 ns delay. */
#define CYCLK_9_5NS             0x08 /* 9.5 ns delay. */
#define CYCLK_10_5NS            0x09 /* 10.5 ns delay. */
#define CYCLK_11_5NS            0x0A /* 11.5 ns delay. */
#define CYCLK_12_5NS            0x0B /* 12.5 ns delay. */
#endif


/***************************************
*             Registers
***************************************/

/* Register to enable or disable the clock */
#define `$INSTANCE_NAME`_CLKEN              (* (reg8 *) `$INSTANCE_NAME`__PM_ACT_CFG)
#define `$INSTANCE_NAME`_CLKEN_PTR          ((reg8 *) `$INSTANCE_NAME`__PM_ACT_CFG)

/* Register to enable or disable the clock */
#define `$INSTANCE_NAME`_CLKSTBY            (* (reg8 *) `$INSTANCE_NAME`__PM_STBY_CFG)
#define `$INSTANCE_NAME`_CLKSTBY_PTR        ((reg8 *) `$INSTANCE_NAME`__PM_STBY_CFG)

/* Clock LSB divider configuration register. */
#define `$INSTANCE_NAME`_DIV_LSB            (* (reg8 *) `$INSTANCE_NAME`__CFG0)
#define `$INSTANCE_NAME`_DIV_LSB_PTR        ((reg8 *) `$INSTANCE_NAME`__CFG0)
#define `$INSTANCE_NAME`_DIV_PTR            ((reg16 *) `$INSTANCE_NAME`__CFG0)

/* Clock MSB divider configuration register. */
#define `$INSTANCE_NAME`_DIV_MSB            (* (reg8 *) `$INSTANCE_NAME`__CFG1)
#define `$INSTANCE_NAME`_DIV_MSB_PTR        ((reg8 *) `$INSTANCE_NAME`__CFG1)

/* Mode and source configuration register */
#define `$INSTANCE_NAME`_MOD_SRC            (* (reg8 *) `$INSTANCE_NAME`__CFG2)
#define `$INSTANCE_NAME`_MOD_SRC_PTR        ((reg8 *) `$INSTANCE_NAME`__CFG2)

#if defined(`$INSTANCE_NAME`__CFG3)
/* Analog clock phase configuration register */
#define `$INSTANCE_NAME`_PHASE              (* (reg8 *) `$INSTANCE_NAME`__CFG3)
#define `$INSTANCE_NAME`_PHASE_PTR          ((reg8 *) `$INSTANCE_NAME`__CFG3)
#endif


/**************************************
*       Register Constants
**************************************/

/* Power manager register masks */
#define `$INSTANCE_NAME`_CLKEN_MASK         `$INSTANCE_NAME`__PM_ACT_MSK
#define `$INSTANCE_NAME`_CLKSTBY_MASK       `$INSTANCE_NAME`__PM_STBY_MSK

/* CFG2 field masks */
#define `$INSTANCE_NAME`_SRC_SEL_MSK        `$INSTANCE_NAME`__CFG2_SRC_SEL_MASK
#define `$INSTANCE_NAME`_MODE_MASK          (~(`$INSTANCE_NAME`_SRC_SEL_MSK))

#if defined(`$INSTANCE_NAME`__CFG3)
/* CFG3 phase mask */
#define `$INSTANCE_NAME`_PHASE_MASK         `$INSTANCE_NAME`__CFG3_PHASE_DLY_MASK
#endif

#endif /* CY_CLOCK_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
