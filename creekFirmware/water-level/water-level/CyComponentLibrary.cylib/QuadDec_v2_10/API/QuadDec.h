/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the Quadrature
*  Decoder component.
*
* Note:
*  None.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_H)
#define CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_H

#include "cyfitter.h"

/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5LP) */

#define `$INSTANCE_NAME`_COUNTER_SIZE               (`$CounterSize`u)

#if (`$INSTANCE_NAME`_COUNTER_SIZE == 8u)
    #include "`$INSTANCE_NAME`_Cnt8.h"
#else
    #include "`$INSTANCE_NAME`_Cnt16.h"
#endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 8u */


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_COUNTER_RESOLUTION         (`$CounterResolution`u)


/***************************************
*       Data Struct Definition
***************************************/

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8 enableState;
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void    `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void    `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void    `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void    `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
uint8   `$INSTANCE_NAME`_GetEvents(void) `=ReentrantKeil($INSTANCE_NAME . "_GetEvents")`;
void    `$INSTANCE_NAME`_SetInterruptMask(uint8 mask) `=ReentrantKeil($INSTANCE_NAME . "_SetInterruptMask")`;
uint8   `$INSTANCE_NAME`_GetInterruptMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetInterruptMask")`;
`$CounterSizeReplacementString`    `$INSTANCE_NAME`_GetCounter(void) `=ReentrantKeil($INSTANCE_NAME . "_GetCounter")`;
void    `$INSTANCE_NAME`_SetCounter(`$CounterSizeReplacementString` value) 
`=ReentrantKeil($INSTANCE_NAME . "_SetCounter")`;
void    `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void    `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void    `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void    `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);


/***************************************
*           API Constants
***************************************/

#define `$INSTANCE_NAME`_ISR_NUMBER                 (`$INSTANCE_NAME``[isr]`_INTC_NUMBER)
#define `$INSTANCE_NAME`_ISR_PRIORITY               (`$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM)


/***************************************
*    Enumerated Types and Parameters
***************************************/

#define `$INSTANCE_NAME`_GLITCH_FILTERING           (`$UsingGlitchFiltering`u)
#define `$INSTANCE_NAME`_INDEX_INPUT                (`$UsingIndexInput`u)


/***************************************
*    Initial Parameter Constants
***************************************/

#if (`$INSTANCE_NAME`_COUNTER_SIZE == 8u)    
    #define `$INSTANCE_NAME`_COUNTER_INIT_VALUE    (0x80u)
#else /* (`$INSTANCE_NAME`_COUNTER_SIZE == 16u) || (`$INSTANCE_NAME`_COUNTER_SIZE == 32u) */
    #define `$INSTANCE_NAME`_COUNTER_INIT_VALUE    (0x8000u)
#endif /* `$INSTANCE_NAME`_COUNTER_SIZE == 8u */   


/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_STATUS_REG                 (* (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_PTR                 (  (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_MASK                (* (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__MASK_REG)
#define `$INSTANCE_NAME`_STATUS_MASK_PTR            (  (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__MASK_REG)
#define `$INSTANCE_NAME`_SR_AUX_CONTROL             (* (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_SR_AUX_CONTROL_PTR         (  (reg8 *) `$INSTANCE_NAME`_bQuadDec_Stsreg__STATUS_AUX_CTL_REG)


/***************************************
*        Register Constants
***************************************/

#define `$INSTANCE_NAME`_COUNTER_OVERFLOW_SHIFT     (0x00u)
#define `$INSTANCE_NAME`_COUNTER_UNDERFLOW_SHIFT    (0x01u)
#define `$INSTANCE_NAME`_COUNTER_RESET_SHIFT        (0x02u)
#define `$INSTANCE_NAME`_INVALID_IN_SHIFT           (0x03u)
#define `$INSTANCE_NAME`_COUNTER_OVERFLOW           (0x01u << `$INSTANCE_NAME`_COUNTER_OVERFLOW_SHIFT)
#define `$INSTANCE_NAME`_COUNTER_UNDERFLOW          (0x01u << `$INSTANCE_NAME`_COUNTER_UNDERFLOW_SHIFT)
#define `$INSTANCE_NAME`_COUNTER_RESET              (0x01u << `$INSTANCE_NAME`_COUNTER_RESET_SHIFT)
#define `$INSTANCE_NAME`_INVALID_IN                 (0x01u << `$INSTANCE_NAME`_INVALID_IN_SHIFT)

#define `$INSTANCE_NAME`_INTERRUPTS_ENABLE_SHIFT    (0x04u)
#define `$INSTANCE_NAME`_INTERRUPTS_ENABLE          (0x01u << `$INSTANCE_NAME`_INTERRUPTS_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_INIT_INT_MASK              (0x0Fu)
#define `$INSTANCE_NAME`_DISABLE                    (0x00u)     

#endif /* CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
