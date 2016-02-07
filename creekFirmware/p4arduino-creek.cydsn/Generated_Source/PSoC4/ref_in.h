/*******************************************************************************
* File Name: ref_in.h  
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

#if !defined(CY_PINS_ref_in_H) /* Pins ref_in_H */
#define CY_PINS_ref_in_H

#include "cytypes.h"
#include "cyfitter.h"
#include "ref_in_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    ref_in_Write(uint8 value) ;
void    ref_in_SetDriveMode(uint8 mode) ;
uint8   ref_in_ReadDataReg(void) ;
uint8   ref_in_Read(void) ;
uint8   ref_in_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define ref_in_DRIVE_MODE_BITS        (3)
#define ref_in_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - ref_in_DRIVE_MODE_BITS))
#define ref_in_DRIVE_MODE_SHIFT       (0x00u)
#define ref_in_DRIVE_MODE_MASK        (0x07u << ref_in_DRIVE_MODE_SHIFT)

#define ref_in_DM_ALG_HIZ         (0x00u << ref_in_DRIVE_MODE_SHIFT)
#define ref_in_DM_DIG_HIZ         (0x01u << ref_in_DRIVE_MODE_SHIFT)
#define ref_in_DM_RES_UP          (0x02u << ref_in_DRIVE_MODE_SHIFT)
#define ref_in_DM_RES_DWN         (0x03u << ref_in_DRIVE_MODE_SHIFT)
#define ref_in_DM_OD_LO           (0x04u << ref_in_DRIVE_MODE_SHIFT)
#define ref_in_DM_OD_HI           (0x05u << ref_in_DRIVE_MODE_SHIFT)
#define ref_in_DM_STRONG          (0x06u << ref_in_DRIVE_MODE_SHIFT)
#define ref_in_DM_RES_UPDWN       (0x07u << ref_in_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define ref_in_MASK               ref_in__MASK
#define ref_in_SHIFT              ref_in__SHIFT
#define ref_in_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define ref_in_PS                     (* (reg32 *) ref_in__PS)
/* Port Configuration */
#define ref_in_PC                     (* (reg32 *) ref_in__PC)
/* Data Register */
#define ref_in_DR                     (* (reg32 *) ref_in__DR)
/* Input Buffer Disable Override */
#define ref_in_INP_DIS                (* (reg32 *) ref_in__PC2)


#if defined(ref_in__INTSTAT)  /* Interrupt Registers */

    #define ref_in_INTSTAT                (* (reg32 *) ref_in__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins ref_in_H */


/* [] END OF FILE */
