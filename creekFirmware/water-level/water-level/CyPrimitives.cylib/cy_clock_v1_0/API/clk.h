/******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for a clock component.
*
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(CY_CLOCK_`$INSTANCE_NAME`_H)
#define CY_CLOCK_`$INSTANCE_NAME`_H

#include <CYTYPES.H>
#include <CYFITTER.H>


/***************************************
*        Function Prototypes            
***************************************/

void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);
void `$INSTANCE_NAME`_StandbyPower(uint8 state);
void `$INSTANCE_NAME`_SetDivider(uint16 clkDivider);
void `$INSTANCE_NAME`_SetMode(uint8 clkMode);
void `$INSTANCE_NAME`_SetSource(uint8 clkSource);

#if defined(`$INSTANCE_NAME`__CFG3)
void `$INSTANCE_NAME`_SetPhase(uint8 clkPhase);
#endif


/* Backward compatibility macros for designs created with PSoC Creator 1.0 Beta 1 */
#define `$INSTANCE_NAME`_Enable `$INSTANCE_NAME`_Start
#define `$INSTANCE_NAME`_Disable `$INSTANCE_NAME`_Stop


/***************************************
*           API Constants        
***************************************/

/* Time values to delay the phase of an analog clock. */
#define CYCLK_2_5NS     		0x01 /* 2.5 ns delay. */
#define CYCLK_3_5NS     		0x02 /* 3.5 ns delay. */
#define CYCLK_4_5NS   			0x03 /* 4.5 ns delay. */ 
#define CYCLK_5_5NS   			0x04 /* 5.5 ns delay. */ 
#define CYCLK_6_5NS   			0x05 /* 6.5 ns delay. */ 
#define CYCLK_7_5NS   			0x06 /* 7.5 ns delay. */ 
#define CYCLK_8_5NS   			0x07 /* 8.5 ns delay. */ 
#define CYCLK_9_5NS   			0x08 /* 9.5 ns delay. */ 
#define CYCLK_10_5NS   			0x09 /* 10.5 ns delay. */
#define CYCLK_11_5NS   			0x0A /* 11.5 ns delay. */
#define CYCLK_12_5NS    		0x0B /* 12.5 ns delay. */
 

/***************************************
*             Registers        
***************************************/

/* Register to enable or disable the digital clocks */
#define `$INSTANCE_NAME`_CLKEN              ((reg8 *) `$INSTANCE_NAME`__PM_ACT_CFG)

/* Clock mask for this clock. */
#define `$INSTANCE_NAME`_CLKEN_MASK         `$INSTANCE_NAME`__PM_ACT_MSK

/* Register to enable or disable the digital clocks */
#define `$INSTANCE_NAME`_CLKSTBY            ((reg8 *) `$INSTANCE_NAME`__PM_STBY_CFG)

/* Clock mask for this clock. */
#define `$INSTANCE_NAME`_CLKSTBY_MASK       `$INSTANCE_NAME`__PM_STBY_MSK

/* Clock LSB divider configuration register. */
#define `$INSTANCE_NAME`_DIV_LSB            ((reg8 *) `$INSTANCE_NAME`__CFG0)

/* Clock MSB divider configuration register. */
#define `$INSTANCE_NAME`_DIV_MSB            ((reg8 *) `$INSTANCE_NAME`__CFG1)

/* Mode and source configuration register */
#define `$INSTANCE_NAME`_MOD_SRC            ((reg8 *) `$INSTANCE_NAME`__CFG2)

#if defined(`$INSTANCE_NAME`__CFG3)
/* Analog clock phase configuration register */
#define `$INSTANCE_NAME`_PHASE              ((reg8 *) `$INSTANCE_NAME`__CFG3)
#endif


/* CY_CLOCK_`$INSTANCE_NAME`_H */
#endif

