/*******************************************************************************
* File Name: countisr.h
* Version 1.70
*
*  Description:
*   Provides the function definitions for the Interrupt Controller.
*
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/
#if !defined(CY_ISR_countisr_H)
#define CY_ISR_countisr_H


#include <cytypes.h>
#include <cyfitter.h>

/* Interrupt Controller API. */
void countisr_Start(void);
void countisr_StartEx(cyisraddress address);
void countisr_Stop(void);

CY_ISR_PROTO(countisr_Interrupt);

void countisr_SetVector(cyisraddress address);
cyisraddress countisr_GetVector(void);

void countisr_SetPriority(uint8 priority);
uint8 countisr_GetPriority(void);

void countisr_Enable(void);
uint8 countisr_GetState(void);
void countisr_Disable(void);

void countisr_SetPending(void);
void countisr_ClearPending(void);


/* Interrupt Controller Constants */

/* Address of the INTC.VECT[x] register that contains the Address of the countisr ISR. */
#define countisr_INTC_VECTOR            ((reg32 *) countisr__INTC_VECT)

/* Address of the countisr ISR priority. */
#define countisr_INTC_PRIOR             ((reg32 *) countisr__INTC_PRIOR_REG)

/* Priority of the countisr interrupt. */
#define countisr_INTC_PRIOR_NUMBER      countisr__INTC_PRIOR_NUM

/* Address of the INTC.SET_EN[x] byte to bit enable countisr interrupt. */
#define countisr_INTC_SET_EN            ((reg32 *) countisr__INTC_SET_EN_REG)

/* Address of the INTC.CLR_EN[x] register to bit clear the countisr interrupt. */
#define countisr_INTC_CLR_EN            ((reg32 *) countisr__INTC_CLR_EN_REG)

/* Address of the INTC.SET_PD[x] register to set the countisr interrupt state to pending. */
#define countisr_INTC_SET_PD            ((reg32 *) countisr__INTC_SET_PD_REG)

/* Address of the INTC.CLR_PD[x] register to clear the countisr interrupt. */
#define countisr_INTC_CLR_PD            ((reg32 *) countisr__INTC_CLR_PD_REG)



#endif /* CY_ISR_countisr_H */


/* [] END OF FILE */
