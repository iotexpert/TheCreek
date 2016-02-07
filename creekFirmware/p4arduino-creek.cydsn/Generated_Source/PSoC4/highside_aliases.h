/*******************************************************************************
* File Name: highside.h  
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

#if !defined(CY_PINS_highside_ALIASES_H) /* Pins highside_ALIASES_H */
#define CY_PINS_highside_ALIASES_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"


/***************************************
*              Constants        
***************************************/
#define highside_0			(highside__0__PC)
#define highside_0_PS		(highside__0__PS)
#define highside_0_PC		(highside__0__PC)
#define highside_0_DR		(highside__0__DR)
#define highside_0_SHIFT	(highside__0__SHIFT)
#define highside_0_INTR	((uint16)((uint16)0x0003u << (highside__0__SHIFT*2u)))

#define highside_INTR_ALL	 ((uint16)(highside_0_INTR))


#endif /* End Pins highside_ALIASES_H */


/* [] END OF FILE */
