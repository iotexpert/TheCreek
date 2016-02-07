/*******************************************************************************
* File Name: REDLED.h  
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

#if !defined(CY_PINS_REDLED_ALIASES_H) /* Pins REDLED_ALIASES_H */
#define CY_PINS_REDLED_ALIASES_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"


/***************************************
*              Constants        
***************************************/
#define REDLED_0			(REDLED__0__PC)
#define REDLED_0_PS		(REDLED__0__PS)
#define REDLED_0_PC		(REDLED__0__PC)
#define REDLED_0_DR		(REDLED__0__DR)
#define REDLED_0_SHIFT	(REDLED__0__SHIFT)
#define REDLED_0_INTR	((uint16)((uint16)0x0003u << (REDLED__0__SHIFT*2u)))

#define REDLED_INTR_ALL	 ((uint16)(REDLED_0_INTR))


#endif /* End Pins REDLED_ALIASES_H */


/* [] END OF FILE */
