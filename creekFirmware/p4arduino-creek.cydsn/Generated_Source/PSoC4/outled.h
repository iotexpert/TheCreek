/*******************************************************************************
* File Name: outled.h  
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

#if !defined(CY_PINS_outled_H) /* Pins outled_H */
#define CY_PINS_outled_H

#include "cytypes.h"
#include "cyfitter.h"
#include "outled_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    outled_Write(uint8 value) ;
void    outled_SetDriveMode(uint8 mode) ;
uint8   outled_ReadDataReg(void) ;
uint8   outled_Read(void) ;
uint8   outled_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define outled_DRIVE_MODE_BITS        (3)
#define outled_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - outled_DRIVE_MODE_BITS))
#define outled_DRIVE_MODE_SHIFT       (0x00u)
#define outled_DRIVE_MODE_MASK        (0x07u << outled_DRIVE_MODE_SHIFT)

#define outled_DM_ALG_HIZ         (0x00u << outled_DRIVE_MODE_SHIFT)
#define outled_DM_DIG_HIZ         (0x01u << outled_DRIVE_MODE_SHIFT)
#define outled_DM_RES_UP          (0x02u << outled_DRIVE_MODE_SHIFT)
#define outled_DM_RES_DWN         (0x03u << outled_DRIVE_MODE_SHIFT)
#define outled_DM_OD_LO           (0x04u << outled_DRIVE_MODE_SHIFT)
#define outled_DM_OD_HI           (0x05u << outled_DRIVE_MODE_SHIFT)
#define outled_DM_STRONG          (0x06u << outled_DRIVE_MODE_SHIFT)
#define outled_DM_RES_UPDWN       (0x07u << outled_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define outled_MASK               outled__MASK
#define outled_SHIFT              outled__SHIFT
#define outled_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define outled_PS                     (* (reg32 *) outled__PS)
/* Port Configuration */
#define outled_PC                     (* (reg32 *) outled__PC)
/* Data Register */
#define outled_DR                     (* (reg32 *) outled__DR)
/* Input Buffer Disable Override */
#define outled_INP_DIS                (* (reg32 *) outled__PC2)


#if defined(outled__INTSTAT)  /* Interrupt Registers */

    #define outled_INTSTAT                (* (reg32 *) outled__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins outled_H */


/* [] END OF FILE */
