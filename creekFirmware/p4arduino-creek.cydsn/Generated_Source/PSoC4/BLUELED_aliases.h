/*******************************************************************************
* File Name: BLUELED.h  
* Version 2.20
*
* Description:
*  This file contains the Alias definitions for Per-Pin APIs in cypins.h. 
*  Information on using these APIs can be found in the System Reference Guide.
*
* Note:
*
********************************************************************************
* Copyright 2008-2015, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_PINS_BLUELED_ALIASES_H) /* Pins BLUELED_ALIASES_H */
#define CY_PINS_BLUELED_ALIASES_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"


/***************************************
*              Constants        
***************************************/
#define BLUELED_0			(BLUELED__0__PC)
#define BLUELED_0_PS		(BLUELED__0__PS)
#define BLUELED_0_PC		(BLUELED__0__PC)
#define BLUELED_0_DR		(BLUELED__0__DR)
#define BLUELED_0_SHIFT	(BLUELED__0__SHIFT)
#define BLUELED_0_INTR	((uint16)((uint16)0x0003u << (BLUELED__0__SHIFT*2u)))

#define BLUELED_INTR_ALL	 ((uint16)(BLUELED_0_INTR))


#endif /* End Pins BLUELED_ALIASES_H */


/* [] END OF FILE */
