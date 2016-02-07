/*******************************************************************************
* File Name: eoc.h  
* Version 1.90
*
* Description:
*  This file containts Control Register function prototypes and register defines
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_PINS_eoc_H) /* Pins eoc_H */
#define CY_PINS_eoc_H

#include "cytypes.h"
#include "cyfitter.h"
#include "eoc_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    eoc_Write(uint8 value) ;
void    eoc_SetDriveMode(uint8 mode) ;
uint8   eoc_ReadDataReg(void) ;
uint8   eoc_Read(void) ;
uint8   eoc_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define eoc_DRIVE_MODE_BITS        (3)
#define eoc_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - eoc_DRIVE_MODE_BITS))
#define eoc_DRIVE_MODE_SHIFT       (0x00u)
#define eoc_DRIVE_MODE_MASK        (0x07u << eoc_DRIVE_MODE_SHIFT)

#define eoc_DM_ALG_HIZ         (0x00u << eoc_DRIVE_MODE_SHIFT)
#define eoc_DM_DIG_HIZ         (0x01u << eoc_DRIVE_MODE_SHIFT)
#define eoc_DM_RES_UP          (0x02u << eoc_DRIVE_MODE_SHIFT)
#define eoc_DM_RES_DWN         (0x03u << eoc_DRIVE_MODE_SHIFT)
#define eoc_DM_OD_LO           (0x04u << eoc_DRIVE_MODE_SHIFT)
#define eoc_DM_OD_HI           (0x05u << eoc_DRIVE_MODE_SHIFT)
#define eoc_DM_STRONG          (0x06u << eoc_DRIVE_MODE_SHIFT)
#define eoc_DM_RES_UPDWN       (0x07u << eoc_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define eoc_MASK               eoc__MASK
#define eoc_SHIFT              eoc__SHIFT
#define eoc_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define eoc_PS                     (* (reg32 *) eoc__PS)
/* Port Configuration */
#define eoc_PC                     (* (reg32 *) eoc__PC)
/* Data Register */
#define eoc_DR                     (* (reg32 *) eoc__DR)
/* Input Buffer Disable Override */
#define eoc_INP_DIS                (* (reg32 *) eoc__PC2)


#if defined(eoc__INTSTAT)  /* Interrupt Registers */

    #define eoc_INTSTAT                (* (reg32 *) eoc__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins eoc_H */


/* [] END OF FILE */
