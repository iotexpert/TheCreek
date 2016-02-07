/*******************************************************************************
* File Name: iout.h  
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

#if !defined(CY_PINS_iout_H) /* Pins iout_H */
#define CY_PINS_iout_H

#include "cytypes.h"
#include "cyfitter.h"
#include "iout_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    iout_Write(uint8 value) ;
void    iout_SetDriveMode(uint8 mode) ;
uint8   iout_ReadDataReg(void) ;
uint8   iout_Read(void) ;
uint8   iout_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define iout_DRIVE_MODE_BITS        (3)
#define iout_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - iout_DRIVE_MODE_BITS))
#define iout_DRIVE_MODE_SHIFT       (0x00u)
#define iout_DRIVE_MODE_MASK        (0x07u << iout_DRIVE_MODE_SHIFT)

#define iout_DM_ALG_HIZ         (0x00u << iout_DRIVE_MODE_SHIFT)
#define iout_DM_DIG_HIZ         (0x01u << iout_DRIVE_MODE_SHIFT)
#define iout_DM_RES_UP          (0x02u << iout_DRIVE_MODE_SHIFT)
#define iout_DM_RES_DWN         (0x03u << iout_DRIVE_MODE_SHIFT)
#define iout_DM_OD_LO           (0x04u << iout_DRIVE_MODE_SHIFT)
#define iout_DM_OD_HI           (0x05u << iout_DRIVE_MODE_SHIFT)
#define iout_DM_STRONG          (0x06u << iout_DRIVE_MODE_SHIFT)
#define iout_DM_RES_UPDWN       (0x07u << iout_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define iout_MASK               iout__MASK
#define iout_SHIFT              iout__SHIFT
#define iout_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define iout_PS                     (* (reg32 *) iout__PS)
/* Port Configuration */
#define iout_PC                     (* (reg32 *) iout__PC)
/* Data Register */
#define iout_DR                     (* (reg32 *) iout__DR)
/* Input Buffer Disable Override */
#define iout_INP_DIS                (* (reg32 *) iout__PC2)


#if defined(iout__INTSTAT)  /* Interrupt Registers */

    #define iout_INTSTAT                (* (reg32 *) iout__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins iout_H */


/* [] END OF FILE */
