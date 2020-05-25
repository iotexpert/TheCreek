/*******************************************************************************
* File Name: pressure.h  
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

#if !defined(CY_PINS_pressure_ALIASES_H) /* Pins pressure_ALIASES_H */
#define CY_PINS_pressure_ALIASES_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"


/***************************************
*              Constants        
***************************************/
#define pressure_0			(pressure__0__PC)
#define pressure_0_PS		(pressure__0__PS)
#define pressure_0_PC		(pressure__0__PC)
#define pressure_0_DR		(pressure__0__DR)
#define pressure_0_SHIFT	(pressure__0__SHIFT)
#define pressure_0_INTR	((uint16)((uint16)0x0003u << (pressure__0__SHIFT*2u)))

#define pressure_INTR_ALL	 ((uint16)(pressure_0_INTR))


#endif /* End Pins pressure_ALIASES_H */


/* [] END OF FILE */
