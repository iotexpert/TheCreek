/*******************************************************************************
* File Name: gnd_in.h  
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

#if !defined(CY_PINS_gnd_in_H) /* Pins gnd_in_H */
#define CY_PINS_gnd_in_H

#include "cytypes.h"
#include "cyfitter.h"
#include "gnd_in_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    gnd_in_Write(uint8 value) ;
void    gnd_in_SetDriveMode(uint8 mode) ;
uint8   gnd_in_ReadDataReg(void) ;
uint8   gnd_in_Read(void) ;
uint8   gnd_in_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define gnd_in_DRIVE_MODE_BITS        (3)
#define gnd_in_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - gnd_in_DRIVE_MODE_BITS))
#define gnd_in_DRIVE_MODE_SHIFT       (0x00u)
#define gnd_in_DRIVE_MODE_MASK        (0x07u << gnd_in_DRIVE_MODE_SHIFT)

#define gnd_in_DM_ALG_HIZ         (0x00u << gnd_in_DRIVE_MODE_SHIFT)
#define gnd_in_DM_DIG_HIZ         (0x01u << gnd_in_DRIVE_MODE_SHIFT)
#define gnd_in_DM_RES_UP          (0x02u << gnd_in_DRIVE_MODE_SHIFT)
#define gnd_in_DM_RES_DWN         (0x03u << gnd_in_DRIVE_MODE_SHIFT)
#define gnd_in_DM_OD_LO           (0x04u << gnd_in_DRIVE_MODE_SHIFT)
#define gnd_in_DM_OD_HI           (0x05u << gnd_in_DRIVE_MODE_SHIFT)
#define gnd_in_DM_STRONG          (0x06u << gnd_in_DRIVE_MODE_SHIFT)
#define gnd_in_DM_RES_UPDWN       (0x07u << gnd_in_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define gnd_in_MASK               gnd_in__MASK
#define gnd_in_SHIFT              gnd_in__SHIFT
#define gnd_in_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define gnd_in_PS                     (* (reg32 *) gnd_in__PS)
/* Port Configuration */
#define gnd_in_PC                     (* (reg32 *) gnd_in__PC)
/* Data Register */
#define gnd_in_DR                     (* (reg32 *) gnd_in__DR)
/* Input Buffer Disable Override */
#define gnd_in_INP_DIS                (* (reg32 *) gnd_in__PC2)


#if defined(gnd_in__INTSTAT)  /* Interrupt Registers */

    #define gnd_in_INTSTAT                (* (reg32 *) gnd_in__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins gnd_in_H */


/* [] END OF FILE */
