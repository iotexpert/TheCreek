/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Contains the function prototypes, constants and register definition
*  of the Vector CAN Component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_Vector_CAN_`$INSTANCE_NAME`_H)
#define CY_Vector_CAN_`$INSTANCE_NAME`_H

#include "cyfitter.h"
#include "CyLib.h"


/***************************************
*        Data Struct Definition
***************************************/

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8  enableState;
	uint32 intSr;
    uint32 intEn;
    uint32 cmd;
    uint32 cfg;    
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

uint8 `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
uint8 `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
uint8 `$INSTANCE_NAME`_Start(void);
uint8 `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
uint8 `$INSTANCE_NAME`_GlobalIntEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_GlobalIntEnable")`;
uint8 `$INSTANCE_NAME`_GlobalIntDisable(void) `=ReentrantKeil($INSTANCE_NAME . "_GlobalIntDisable")`;
void  `$INSTANCE_NAME`_Sleep(void);
void  `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void  `$INSTANCE_NAME`_SaveConfig(void);
void  `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

/* Interrupt handler */
CY_ISR_PROTO(CanIsr_0);


/***************************************
*           API Constants
***************************************/

/* Number of the `$INSTANCE_NAME`_isr interrupt */
#define `$INSTANCE_NAME`_ISR_NUMBER                 (`$INSTANCE_NAME``[isr]`_INTC_NUMBER)

/* Priority of the `$INSTANCE_NAME`_isr interrupt */
#define `$INSTANCE_NAME`_ISR_PRIORITY               (`$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM)

/* PM_ACT_CFG (Active Power Mode CFG Register) */
#define `$INSTANCE_NAME`_ACT_PWR_EN                 (`$INSTANCE_NAME``[CanIP]`_PM_ACT_MSK)    /* Power enable mask */

/* PM_STBY_CFG (Alternate Active (Standby) Power Mode CFG Register) */
#define `$INSTANCE_NAME`_STBY_PWR_EN                (`$INSTANCE_NAME``[CanIP]`_PM_STBY_MSK)   /* Power enable mask */

/* Function failed. */
#define `$INSTANCE_NAME`_FAIL                       (0x01u)


/***************************************
*    Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_TRANSCEIVER_ENABLE_SIGNAL    (`$AddTransceiverEnableSignal`u) 
#define `$INSTANCE_NAME`_ENABLE_INTERRUPTS            (`$EnableInterrupts`u) 


/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_INT_EN_REG      ( *(reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_EN )
#define `$INSTANCE_NAME`_INT_EN_PTR      (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_EN )
#define `$INSTANCE_NAME`_CMD_REG         ( *(reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_CMD )
#define `$INSTANCE_NAME`_CMD_PTR         (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_CMD )
#define `$INSTANCE_NAME`_CFG_REG         ( *(reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_CFG )
#define `$INSTANCE_NAME`_CFG_PTR         (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_CFG )
#define `$INSTANCE_NAME`_INT_SR_REG      ( *(reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_SR )
#define `$INSTANCE_NAME`_INT_SR_PTR      (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_SR )
#define `$INSTANCE_NAME`_PM_ACT_CFG_REG  ( *(reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_ACT_CFG )    /* Power manager */
#define `$INSTANCE_NAME`_PM_ACT_CFG_PTR  (  (reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_ACT_CFG )    /* Power manager */
#define `$INSTANCE_NAME`_PM_STBY_CFG_REG ( *(reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_STBY_CFG )   /* Power manager */
#define `$INSTANCE_NAME`_PM_STBY_CFG_PTR (  (reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_STBY_CFG )   /* Power manager */ 


/***************************************
*        Register Constants
***************************************/

/* Mask for bits within `$INSTANCE_NAME`_CSR_CMD */
#define `$INSTANCE_NAME`_MODE_SHIFT                    (0u)
/* bit 0 within CSR_CMD */
#define `$INSTANCE_NAME`_MODE_MASK                     ((uint32)0x00000001u << `$INSTANCE_NAME`_MODE_SHIFT)

/* Mask  for bits within `$INSTANCE_NAME`_INT_EN_REG */
#define `$INSTANCE_NAME`_GLOBAL_INT_SHIFT              (0u)
/* bit 0 within INT_EN */
#define `$INSTANCE_NAME`_GLOBAL_INT_MASK               ((uint32)0x00000001u << `$INSTANCE_NAME`_GLOBAL_INT_SHIFT)

#endif /* CY_Vector_CAN_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
