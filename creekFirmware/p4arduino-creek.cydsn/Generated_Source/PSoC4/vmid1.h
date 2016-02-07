/*******************************************************************************
* File Name: vmid1.h  
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

#if !defined(CY_PINS_vmid1_H) /* Pins vmid1_H */
#define CY_PINS_vmid1_H

#include "cytypes.h"
#include "cyfitter.h"
#include "vmid1_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    vmid1_Write(uint8 value) ;
void    vmid1_SetDriveMode(uint8 mode) ;
uint8   vmid1_ReadDataReg(void) ;
uint8   vmid1_Read(void) ;
uint8   vmid1_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define vmid1_DRIVE_MODE_BITS        (3)
#define vmid1_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - vmid1_DRIVE_MODE_BITS))
#define vmid1_DRIVE_MODE_SHIFT       (0x00u)
#define vmid1_DRIVE_MODE_MASK        (0x07u << vmid1_DRIVE_MODE_SHIFT)

#define vmid1_DM_ALG_HIZ         (0x00u << vmid1_DRIVE_MODE_SHIFT)
#define vmid1_DM_DIG_HIZ         (0x01u << vmid1_DRIVE_MODE_SHIFT)
#define vmid1_DM_RES_UP          (0x02u << vmid1_DRIVE_MODE_SHIFT)
#define vmid1_DM_RES_DWN         (0x03u << vmid1_DRIVE_MODE_SHIFT)
#define vmid1_DM_OD_LO           (0x04u << vmid1_DRIVE_MODE_SHIFT)
#define vmid1_DM_OD_HI           (0x05u << vmid1_DRIVE_MODE_SHIFT)
#define vmid1_DM_STRONG          (0x06u << vmid1_DRIVE_MODE_SHIFT)
#define vmid1_DM_RES_UPDWN       (0x07u << vmid1_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define vmid1_MASK               vmid1__MASK
#define vmid1_SHIFT              vmid1__SHIFT
#define vmid1_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define vmid1_PS                     (* (reg32 *) vmid1__PS)
/* Port Configuration */
#define vmid1_PC                     (* (reg32 *) vmid1__PC)
/* Data Register */
#define vmid1_DR                     (* (reg32 *) vmid1__DR)
/* Input Buffer Disable Override */
#define vmid1_INP_DIS                (* (reg32 *) vmid1__PC2)


#if defined(vmid1__INTSTAT)  /* Interrupt Registers */

    #define vmid1_INTSTAT                (* (reg32 *) vmid1__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins vmid1_H */


/* [] END OF FILE */
