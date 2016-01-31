/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for the Interrupt Controller.
*
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
#if !defined(__`$INSTANCE_NAME`_INTC_H__)
#define __`$INSTANCE_NAME`_INTC_H__


#include <cytypes.h>
#include <cyfitter.h>

/* Interrupt Controller API. */
void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_StartEx(cyisraddress address);
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME ."_Stop")`;

CY_ISR_PROTO(`$INSTANCE_NAME`_Interrupt);

void `$INSTANCE_NAME`_SetVector(cyisraddress address) `=ReentrantKeil($INSTANCE_NAME ."_SetVector")`;
cyisraddress `$INSTANCE_NAME`_GetVector(void) `=ReentrantKeil($INSTANCE_NAME ."_GetVector")`;

void `$INSTANCE_NAME`_SetPriority(uint8 priority) `=ReentrantKeil($INSTANCE_NAME ."_SetPriority")`;
uint8 `$INSTANCE_NAME`_GetPriority(void) `=ReentrantKeil($INSTANCE_NAME ."_GetPriority")`;

void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME ."_Enable")`;
uint8 `$INSTANCE_NAME`_GetState(void) `=ReentrantKeil($INSTANCE_NAME ."_GetState")`;
void `$INSTANCE_NAME`_Disable(void) `=ReentrantKeil($INSTANCE_NAME ."_Disable")`;

void `$INSTANCE_NAME`_SetPending(void) `=ReentrantKeil($INSTANCE_NAME ."_SetPending")`;
void `$INSTANCE_NAME`_ClearPending(void) `=ReentrantKeil($INSTANCE_NAME ."_ClearPending")`;


/* Interrupt Controller Constants */

/* Address of the INTC.VECT[x] register that contains the Address of the `$INSTANCE_NAME` ISR. */
#define `$INSTANCE_NAME`_INTC_VECTOR            ((reg32 *) `$INSTANCE_NAME`__INTC_VECT)

/* Address of the `$INSTANCE_NAME` ISR priority. */
#define `$INSTANCE_NAME`_INTC_PRIOR             ((reg8 *) `$INSTANCE_NAME`__INTC_PRIOR_REG)

/* Priority of the `$INSTANCE_NAME` interrupt. */
#define `$INSTANCE_NAME`_INTC_PRIOR_NUMBER      `$INSTANCE_NAME`__INTC_PRIOR_NUM

/* Address of the INTC.SET_EN[x] byte to bit enable `$INSTANCE_NAME` interrupt. */
#define `$INSTANCE_NAME`_INTC_SET_EN            ((reg32 *) `$INSTANCE_NAME`__INTC_SET_EN_REG)

/* Address of the INTC.CLR_EN[x] register to bit clear the `$INSTANCE_NAME` interrupt. */
#define `$INSTANCE_NAME`_INTC_CLR_EN            ((reg32 *) `$INSTANCE_NAME`__INTC_CLR_EN_REG)

/* Address of the INTC.SET_PD[x] register to set the `$INSTANCE_NAME` interrupt state to pending. */
#define `$INSTANCE_NAME`_INTC_SET_PD            ((reg32 *) `$INSTANCE_NAME`__INTC_SET_PD_REG)

/* Address of the INTC.CLR_PD[x] register to clear the `$INSTANCE_NAME` interrupt. */
#define `$INSTANCE_NAME`_INTC_CLR_PD            ((reg32 *) `$INSTANCE_NAME`__INTC_CLR_PD_REG)



/* __`$INSTANCE_NAME`_INTC_H__ */
#endif
