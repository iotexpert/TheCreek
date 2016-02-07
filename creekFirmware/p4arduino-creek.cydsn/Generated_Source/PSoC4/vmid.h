/*******************************************************************************
* File Name: vmid.h  
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

#if !defined(CY_PINS_vmid_H) /* Pins vmid_H */
#define CY_PINS_vmid_H

#include "cytypes.h"
#include "cyfitter.h"
#include "vmid_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    vmid_Write(uint8 value) ;
void    vmid_SetDriveMode(uint8 mode) ;
uint8   vmid_ReadDataReg(void) ;
uint8   vmid_Read(void) ;
uint8   vmid_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define vmid_DRIVE_MODE_BITS        (3)
#define vmid_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - vmid_DRIVE_MODE_BITS))
#define vmid_DRIVE_MODE_SHIFT       (0x00u)
#define vmid_DRIVE_MODE_MASK        (0x07u << vmid_DRIVE_MODE_SHIFT)

#define vmid_DM_ALG_HIZ         (0x00u << vmid_DRIVE_MODE_SHIFT)
#define vmid_DM_DIG_HIZ         (0x01u << vmid_DRIVE_MODE_SHIFT)
#define vmid_DM_RES_UP          (0x02u << vmid_DRIVE_MODE_SHIFT)
#define vmid_DM_RES_DWN         (0x03u << vmid_DRIVE_MODE_SHIFT)
#define vmid_DM_OD_LO           (0x04u << vmid_DRIVE_MODE_SHIFT)
#define vmid_DM_OD_HI           (0x05u << vmid_DRIVE_MODE_SHIFT)
#define vmid_DM_STRONG          (0x06u << vmid_DRIVE_MODE_SHIFT)
#define vmid_DM_RES_UPDWN       (0x07u << vmid_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define vmid_MASK               vmid__MASK
#define vmid_SHIFT              vmid__SHIFT
#define vmid_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define vmid_PS                     (* (reg32 *) vmid__PS)
/* Port Configuration */
#define vmid_PC                     (* (reg32 *) vmid__PC)
/* Data Register */
#define vmid_DR                     (* (reg32 *) vmid__DR)
/* Input Buffer Disable Override */
#define vmid_INP_DIS                (* (reg32 *) vmid__PC2)


#if defined(vmid__INTSTAT)  /* Interrupt Registers */

    #define vmid_INTSTAT                (* (reg32 *) vmid__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins vmid_H */


/* [] END OF FILE */
