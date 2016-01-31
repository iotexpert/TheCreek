/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_VERSION_MAJOR`.`$CY_VERSION_MINOR`
*
* Description:
*  This file provides constants and parameter values for the Sleep Timer
*  Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_SLEEPTIMER_`$INSTANCE_NAME`_H)
#define CY_SLEEPTIMER_`$INSTANCE_NAME`_H

#include "cydevice_trm.h"
#include "cyfitter.h"
#include "cyPm.h"


/**************************************
*  Function Prototypes
**************************************/

void  `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void  `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void  `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void  `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void  `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
void  `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;
void  `$INSTANCE_NAME`_SetInterval(uint8 interval) `=ReentrantKeil($INSTANCE_NAME . "_SetInterval")`;
uint8 `$INSTANCE_NAME`_GetStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetStatus")`;


/***************************************
*   Enumerated Types and Parameters
***************************************/

/* Enumerated Types SLEEPTIMER_interval_enum, used in parameter Interval*/
`#declare_enum SLEEPTIMER_INTERVAL`


/***************************************
*   API Constants
***************************************/


/***************************************
*   Initialization Values
***************************************/

#define `$INSTANCE_NAME`_ENABLE_INTERRUPT   (`$EnableInt`u)
#define `$INSTANCE_NAME`_INTERVAL           (`$Interval`u)


/**************************************
*   Registers
**************************************/

#define `$INSTANCE_NAME`_CTW_INTERVAL_REG   (* (reg8 *) CYREG_PM_TW_CFG1 )
#define `$INSTANCE_NAME`_CTW_INTERVAL_PTR   (  (reg8 *) CYREG_PM_TW_CFG1 )

#define `$INSTANCE_NAME`_TW_CFG_REG         (* (reg8 *) CYREG_PM_TW_CFG2 )
#define `$INSTANCE_NAME`_TW_CFG_PTR         (  (reg8 *) CYREG_PM_TW_CFG2 )

#define `$INSTANCE_NAME`_ILO_CFG_REG        (* (reg8 *) CYDEV_SLOWCLK_BASE )
#define `$INSTANCE_NAME`_ILO_CFG_PTR        (  (reg8 *) CYDEV_SLOWCLK_BASE )

#define `$INSTANCE_NAME`_INT_SR_REG         (* (reg8 *) CYREG_PM_INT_SR )
#define `$INSTANCE_NAME`_INT_SR_PTR         (  (reg8 *) CYREG_PM_INT_SR )


/**************************************
*   Register Constants
**************************************/

/* Issue interrupt when CTW enabled */
#define `$INSTANCE_NAME`_CTW_IE             (0x08u)

/* CTW enable */
#define `$INSTANCE_NAME`_CTW_EN             (0x04u)

/* 1 kHz ILO enable */
#define `$INSTANCE_NAME`_ILO_1KHZ_EN        (0x02u)

/* CTW interval mask  */
#define `$INSTANCE_NAME`_INTERVAL_MASK      (0x0Fu)

/* A onepps event has occured */
#define `$INSTANCE_NAME`_PM_INT_SR_ONEPPSP  (0x04u)

/* A central timewheel event has occured */
#define `$INSTANCE_NAME`_PM_INT_SR_CTW      (0x02u)

/* A fast timewheel event has occured */
#define `$INSTANCE_NAME`_PM_INT_SR_FTW      (0x01u)

#endif  /* End of CY_SLEEPTIMER_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
