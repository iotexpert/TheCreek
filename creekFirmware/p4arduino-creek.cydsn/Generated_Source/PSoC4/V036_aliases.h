/*******************************************************************************
* File Name: V036.h  
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

#if !defined(CY_PINS_V036_ALIASES_H) /* Pins V036_ALIASES_H */
#define CY_PINS_V036_ALIASES_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"


/***************************************
*              Constants        
***************************************/
#define V036_0			(V036__0__PC)
#define V036_0_PS		(V036__0__PS)
#define V036_0_PC		(V036__0__PC)
#define V036_0_DR		(V036__0__DR)
#define V036_0_SHIFT	(V036__0__SHIFT)
#define V036_0_INTR	((uint16)((uint16)0x0003u << (V036__0__SHIFT*2u)))

#define V036_INTR_ALL	 ((uint16)(V036_0_INTR))


#endif /* End Pins V036_ALIASES_H */


/* [] END OF FILE */
