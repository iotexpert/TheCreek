/*******************************************************************************
* File Name: adcisr.h
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
#if !defined(CY_ISR_adcisr_H)
#define CY_ISR_adcisr_H


#include <cytypes.h>
#include <cyfitter.h>

/* Interrupt Controller API. */
void adcisr_Start(void);
void adcisr_StartEx(cyisraddress address);
void adcisr_Stop(void);

CY_ISR_PROTO(adcisr_Interrupt);

void adcisr_SetVector(cyisraddress address);
cyisraddress adcisr_GetVector(void);

void adcisr_SetPriority(uint8 priority);
uint8 adcisr_GetPriority(void);

void adcisr_Enable(void);
uint8 adcisr_GetState(void);
void adcisr_Disable(void);

void adcisr_SetPending(void);
void adcisr_ClearPending(void);


/* Interrupt Controller Constants */

/* Address of the INTC.VECT[x] register that contains the Address of the adcisr ISR. */
#define adcisr_INTC_VECTOR            ((reg32 *) adcisr__INTC_VECT)

/* Address of the adcisr ISR priority. */
#define adcisr_INTC_PRIOR             ((reg32 *) adcisr__INTC_PRIOR_REG)

/* Priority of the adcisr interrupt. */
#define adcisr_INTC_PRIOR_NUMBER      adcisr__INTC_PRIOR_NUM

/* Address of the INTC.SET_EN[x] byte to bit enable adcisr interrupt. */
#define adcisr_INTC_SET_EN            ((reg32 *) adcisr__INTC_SET_EN_REG)

/* Address of the INTC.CLR_EN[x] register to bit clear the adcisr interrupt. */
#define adcisr_INTC_CLR_EN            ((reg32 *) adcisr__INTC_CLR_EN_REG)

/* Address of the INTC.SET_PD[x] register to set the adcisr interrupt state to pending. */
#define adcisr_INTC_SET_PD            ((reg32 *) adcisr__INTC_SET_PD_REG)

/* Address of the INTC.CLR_PD[x] register to clear the adcisr interrupt. */
#define adcisr_INTC_CLR_PD            ((reg32 *) adcisr__INTC_CLR_PD_REG)



#endif /* CY_ISR_adcisr_H */


/* [] END OF FILE */
