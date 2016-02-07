/*******************************************************************************
* File Name: vhigh.h  
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

#if !defined(CY_PINS_vhigh_H) /* Pins vhigh_H */
#define CY_PINS_vhigh_H

#include "cytypes.h"
#include "cyfitter.h"
#include "vhigh_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    vhigh_Write(uint8 value) ;
void    vhigh_SetDriveMode(uint8 mode) ;
uint8   vhigh_ReadDataReg(void) ;
uint8   vhigh_Read(void) ;
uint8   vhigh_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define vhigh_DRIVE_MODE_BITS        (3)
#define vhigh_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - vhigh_DRIVE_MODE_BITS))
#define vhigh_DRIVE_MODE_SHIFT       (0x00u)
#define vhigh_DRIVE_MODE_MASK        (0x07u << vhigh_DRIVE_MODE_SHIFT)

#define vhigh_DM_ALG_HIZ         (0x00u << vhigh_DRIVE_MODE_SHIFT)
#define vhigh_DM_DIG_HIZ         (0x01u << vhigh_DRIVE_MODE_SHIFT)
#define vhigh_DM_RES_UP          (0x02u << vhigh_DRIVE_MODE_SHIFT)
#define vhigh_DM_RES_DWN         (0x03u << vhigh_DRIVE_MODE_SHIFT)
#define vhigh_DM_OD_LO           (0x04u << vhigh_DRIVE_MODE_SHIFT)
#define vhigh_DM_OD_HI           (0x05u << vhigh_DRIVE_MODE_SHIFT)
#define vhigh_DM_STRONG          (0x06u << vhigh_DRIVE_MODE_SHIFT)
#define vhigh_DM_RES_UPDWN       (0x07u << vhigh_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define vhigh_MASK               vhigh__MASK
#define vhigh_SHIFT              vhigh__SHIFT
#define vhigh_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define vhigh_PS                     (* (reg32 *) vhigh__PS)
/* Port Configuration */
#define vhigh_PC                     (* (reg32 *) vhigh__PC)
/* Data Register */
#define vhigh_DR                     (* (reg32 *) vhigh__DR)
/* Input Buffer Disable Override */
#define vhigh_INP_DIS                (* (reg32 *) vhigh__PC2)


#if defined(vhigh__INTSTAT)  /* Interrupt Registers */

    #define vhigh_INTSTAT                (* (reg32 *) vhigh__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins vhigh_H */


/* [] END OF FILE */
