/*******************************************************************************
* File Name: cycle.h  
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

#if !defined(CY_PINS_cycle_H) /* Pins cycle_H */
#define CY_PINS_cycle_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cycle_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    cycle_Write(uint8 value) ;
void    cycle_SetDriveMode(uint8 mode) ;
uint8   cycle_ReadDataReg(void) ;
uint8   cycle_Read(void) ;
uint8   cycle_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define cycle_DRIVE_MODE_BITS        (3)
#define cycle_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - cycle_DRIVE_MODE_BITS))
#define cycle_DRIVE_MODE_SHIFT       (0x00u)
#define cycle_DRIVE_MODE_MASK        (0x07u << cycle_DRIVE_MODE_SHIFT)

#define cycle_DM_ALG_HIZ         (0x00u << cycle_DRIVE_MODE_SHIFT)
#define cycle_DM_DIG_HIZ         (0x01u << cycle_DRIVE_MODE_SHIFT)
#define cycle_DM_RES_UP          (0x02u << cycle_DRIVE_MODE_SHIFT)
#define cycle_DM_RES_DWN         (0x03u << cycle_DRIVE_MODE_SHIFT)
#define cycle_DM_OD_LO           (0x04u << cycle_DRIVE_MODE_SHIFT)
#define cycle_DM_OD_HI           (0x05u << cycle_DRIVE_MODE_SHIFT)
#define cycle_DM_STRONG          (0x06u << cycle_DRIVE_MODE_SHIFT)
#define cycle_DM_RES_UPDWN       (0x07u << cycle_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define cycle_MASK               cycle__MASK
#define cycle_SHIFT              cycle__SHIFT
#define cycle_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define cycle_PS                     (* (reg32 *) cycle__PS)
/* Port Configuration */
#define cycle_PC                     (* (reg32 *) cycle__PC)
/* Data Register */
#define cycle_DR                     (* (reg32 *) cycle__DR)
/* Input Buffer Disable Override */
#define cycle_INP_DIS                (* (reg32 *) cycle__PC2)


#if defined(cycle__INTSTAT)  /* Interrupt Registers */

    #define cycle_INTSTAT                (* (reg32 *) cycle__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins cycle_H */


/* [] END OF FILE */
