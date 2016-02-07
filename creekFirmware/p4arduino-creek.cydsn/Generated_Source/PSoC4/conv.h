/*******************************************************************************
* File Name: conv.h  
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

#if !defined(CY_PINS_conv_H) /* Pins conv_H */
#define CY_PINS_conv_H

#include "cytypes.h"
#include "cyfitter.h"
#include "conv_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    conv_Write(uint8 value) ;
void    conv_SetDriveMode(uint8 mode) ;
uint8   conv_ReadDataReg(void) ;
uint8   conv_Read(void) ;
uint8   conv_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define conv_DRIVE_MODE_BITS        (3)
#define conv_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - conv_DRIVE_MODE_BITS))
#define conv_DRIVE_MODE_SHIFT       (0x00u)
#define conv_DRIVE_MODE_MASK        (0x07u << conv_DRIVE_MODE_SHIFT)

#define conv_DM_ALG_HIZ         (0x00u << conv_DRIVE_MODE_SHIFT)
#define conv_DM_DIG_HIZ         (0x01u << conv_DRIVE_MODE_SHIFT)
#define conv_DM_RES_UP          (0x02u << conv_DRIVE_MODE_SHIFT)
#define conv_DM_RES_DWN         (0x03u << conv_DRIVE_MODE_SHIFT)
#define conv_DM_OD_LO           (0x04u << conv_DRIVE_MODE_SHIFT)
#define conv_DM_OD_HI           (0x05u << conv_DRIVE_MODE_SHIFT)
#define conv_DM_STRONG          (0x06u << conv_DRIVE_MODE_SHIFT)
#define conv_DM_RES_UPDWN       (0x07u << conv_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define conv_MASK               conv__MASK
#define conv_SHIFT              conv__SHIFT
#define conv_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define conv_PS                     (* (reg32 *) conv__PS)
/* Port Configuration */
#define conv_PC                     (* (reg32 *) conv__PC)
/* Data Register */
#define conv_DR                     (* (reg32 *) conv__DR)
/* Input Buffer Disable Override */
#define conv_INP_DIS                (* (reg32 *) conv__PC2)


#if defined(conv__INTSTAT)  /* Interrupt Registers */

    #define conv_INTSTAT                (* (reg32 *) conv__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins conv_H */


/* [] END OF FILE */
