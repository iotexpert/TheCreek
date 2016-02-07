/*******************************************************************************
* File Name: vmid2.h  
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

#if !defined(CY_PINS_vmid2_H) /* Pins vmid2_H */
#define CY_PINS_vmid2_H

#include "cytypes.h"
#include "cyfitter.h"
#include "vmid2_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    vmid2_Write(uint8 value) ;
void    vmid2_SetDriveMode(uint8 mode) ;
uint8   vmid2_ReadDataReg(void) ;
uint8   vmid2_Read(void) ;
uint8   vmid2_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define vmid2_DRIVE_MODE_BITS        (3)
#define vmid2_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - vmid2_DRIVE_MODE_BITS))
#define vmid2_DRIVE_MODE_SHIFT       (0x00u)
#define vmid2_DRIVE_MODE_MASK        (0x07u << vmid2_DRIVE_MODE_SHIFT)

#define vmid2_DM_ALG_HIZ         (0x00u << vmid2_DRIVE_MODE_SHIFT)
#define vmid2_DM_DIG_HIZ         (0x01u << vmid2_DRIVE_MODE_SHIFT)
#define vmid2_DM_RES_UP          (0x02u << vmid2_DRIVE_MODE_SHIFT)
#define vmid2_DM_RES_DWN         (0x03u << vmid2_DRIVE_MODE_SHIFT)
#define vmid2_DM_OD_LO           (0x04u << vmid2_DRIVE_MODE_SHIFT)
#define vmid2_DM_OD_HI           (0x05u << vmid2_DRIVE_MODE_SHIFT)
#define vmid2_DM_STRONG          (0x06u << vmid2_DRIVE_MODE_SHIFT)
#define vmid2_DM_RES_UPDWN       (0x07u << vmid2_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define vmid2_MASK               vmid2__MASK
#define vmid2_SHIFT              vmid2__SHIFT
#define vmid2_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define vmid2_PS                     (* (reg32 *) vmid2__PS)
/* Port Configuration */
#define vmid2_PC                     (* (reg32 *) vmid2__PC)
/* Data Register */
#define vmid2_DR                     (* (reg32 *) vmid2__DR)
/* Input Buffer Disable Override */
#define vmid2_INP_DIS                (* (reg32 *) vmid2__PC2)


#if defined(vmid2__INTSTAT)  /* Interrupt Registers */

    #define vmid2_INTSTAT                (* (reg32 *) vmid2__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins vmid2_H */


/* [] END OF FILE */
