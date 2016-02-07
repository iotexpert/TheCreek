/*******************************************************************************
* File Name: ezi2c_scl.h  
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

#if !defined(CY_PINS_ezi2c_scl_ALIASES_H) /* Pins ezi2c_scl_ALIASES_H */
#define CY_PINS_ezi2c_scl_ALIASES_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cypins.h"


/***************************************
*              Constants        
***************************************/
#define ezi2c_scl_0			(ezi2c_scl__0__PC)
#define ezi2c_scl_0_PS		(ezi2c_scl__0__PS)
#define ezi2c_scl_0_PC		(ezi2c_scl__0__PC)
#define ezi2c_scl_0_DR		(ezi2c_scl__0__DR)
#define ezi2c_scl_0_SHIFT	(ezi2c_scl__0__SHIFT)
#define ezi2c_scl_0_INTR	((uint16)((uint16)0x0003u << (ezi2c_scl__0__SHIFT*2u)))

#define ezi2c_scl_INTR_ALL	 ((uint16)(ezi2c_scl_0_INTR))


#endif /* End Pins ezi2c_scl_ALIASES_H */


/* [] END OF FILE */
