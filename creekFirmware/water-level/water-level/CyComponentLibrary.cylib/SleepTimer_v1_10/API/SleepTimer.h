/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_VERSION_MAJOR`.`$CY_VERSION_MINOR`
*
*  Description:
*     This file provides constants and parameter values for the Sleep Timer
*     Component.
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/


#include "cytypes.h"
#include "cydevice_trm.h"

#if !defined(`$INSTANCE_NAME`_H)
#define `$INSTANCE_NAME`_H


/**************************************
 *  Function Prototypes
 *************************************/

void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Stop(void);
void  `$INSTANCE_NAME`_EnableInt(void);
void  `$INSTANCE_NAME`_DisableInt(void);
void  `$INSTANCE_NAME`_SetInterval(uint8 interval);
uint8 `$INSTANCE_NAME`_GetStatus(void);


/***************************************
*   Enumerated Types and Parameters
***************************************/

/* Enumerated Types SLEEPTIMER_interval_enum, Used in parameter Interval*/
`#declare_enum SLEEPTIMER_INTERVAL`

/***************************************
* API Constants
***************************************/

#define `$INSTANCE_NAME`_INTERRUPT_CHNG_BIT 0x01u
#define `$INSTANCE_NAME`_INTERVAL_CHNG_BIT  0x02u


/***************************************
 *  Initialization Values
 **************************************/

#define `$INSTANCE_NAME`_ENABLE_INTERRUPT    `$EnableInt`
#define `$INSTANCE_NAME`_INTERVAL            `$Interval`


/**************************************
 *  Registers
 *************************************/

#define `$INSTANCE_NAME`_CTW_INTERVAL       (* (reg8 *) CYREG_PM_TW_CFG1 )
#define `$INSTANCE_NAME`_CTW_INTERVAL_PTR   (  (reg8 *) CYREG_PM_TW_CFG1 )

#define `$INSTANCE_NAME`_TW_CFG             (* (reg8 *) CYREG_PM_TW_CFG2 )
#define `$INSTANCE_NAME`_TW_CFG_PTR         (  (reg8 *) CYREG_PM_TW_CFG2 )

#define `$INSTANCE_NAME`_WDT_CFG            (* (reg8 *) CYREG_PM_WDT_CFG )
#define `$INSTANCE_NAME`_WDT_CFG_PTR        (  (reg8 *) CYREG_PM_WDT_CFG )

#define `$INSTANCE_NAME`_ILO_CFG            (* (reg8 *) CYDEV_SLOWCLK_BASE )
#define `$INSTANCE_NAME`_ILO_CFG_PTR        (  (reg8 *) CYDEV_SLOWCLK_BASE )

#define `$INSTANCE_NAME`_INT_SR             (* (reg8 *) CYREG_PM_INT_SR )
#define `$INSTANCE_NAME`_INT_SR_PTR         (  (reg8 *) CYREG_PM_INT_SR )


/**************************************
 *  Register Constants
 *************************************/

/* Issue interrupt when CTW enabled */
#define `$INSTANCE_NAME`_CTW_IE        0x08u
/* CTW enable */
#define `$INSTANCE_NAME`_CTW_EN        0x04u
/* Watch dog reset enable */
#define `$INSTANCE_NAME`_WDR_EN        0x10u
/* CTW reset */
#define `$INSTANCE_NAME`_CTW_RESET     0x80u
/* 1 kHz ILO  */
#define `$INSTANCE_NAME`_ILO_1KHZ_EN   0x02u
/* CTW interval mask  */
#define `$INSTANCE_NAME`_INTERVAL_MASK 0x0Fu

/* Device has moved from limited active to active mode */
#define `$INSTANCE_NAME`_PM_INT_SR_REACT    0x80u
/* A limited active ready event has occured */
#define `$INSTANCE_NAME`_PM_INT_SR_LIMACT   0x08u
/* A onepps event has occured */
#define `$INSTANCE_NAME`_PM_INT_SR_ONEPPSP  0x04u
/* A central timewheel event has occured */
#define `$INSTANCE_NAME`_PM_INT_SR_CTW      0x02u
/* A fast timewheel event has occured */
#define `$INSTANCE_NAME`_PM_INT_SR_FTW      0x01u

#endif  /* `$INSTANCE_NAME`_H */


/* [] END OF FILE */
