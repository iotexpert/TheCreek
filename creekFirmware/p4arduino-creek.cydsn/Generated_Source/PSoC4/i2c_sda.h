/*******************************************************************************
* File Name: i2c_sda.h  
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

#if !defined(CY_PINS_i2c_sda_H) /* Pins i2c_sda_H */
#define CY_PINS_i2c_sda_H

#include "cytypes.h"
#include "cyfitter.h"
#include "i2c_sda_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    i2c_sda_Write(uint8 value) ;
void    i2c_sda_SetDriveMode(uint8 mode) ;
uint8   i2c_sda_ReadDataReg(void) ;
uint8   i2c_sda_Read(void) ;
uint8   i2c_sda_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define i2c_sda_DRIVE_MODE_BITS        (3)
#define i2c_sda_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - i2c_sda_DRIVE_MODE_BITS))
#define i2c_sda_DRIVE_MODE_SHIFT       (0x00u)
#define i2c_sda_DRIVE_MODE_MASK        (0x07u << i2c_sda_DRIVE_MODE_SHIFT)

#define i2c_sda_DM_ALG_HIZ         (0x00u << i2c_sda_DRIVE_MODE_SHIFT)
#define i2c_sda_DM_DIG_HIZ         (0x01u << i2c_sda_DRIVE_MODE_SHIFT)
#define i2c_sda_DM_RES_UP          (0x02u << i2c_sda_DRIVE_MODE_SHIFT)
#define i2c_sda_DM_RES_DWN         (0x03u << i2c_sda_DRIVE_MODE_SHIFT)
#define i2c_sda_DM_OD_LO           (0x04u << i2c_sda_DRIVE_MODE_SHIFT)
#define i2c_sda_DM_OD_HI           (0x05u << i2c_sda_DRIVE_MODE_SHIFT)
#define i2c_sda_DM_STRONG          (0x06u << i2c_sda_DRIVE_MODE_SHIFT)
#define i2c_sda_DM_RES_UPDWN       (0x07u << i2c_sda_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define i2c_sda_MASK               i2c_sda__MASK
#define i2c_sda_SHIFT              i2c_sda__SHIFT
#define i2c_sda_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define i2c_sda_PS                     (* (reg32 *) i2c_sda__PS)
/* Port Configuration */
#define i2c_sda_PC                     (* (reg32 *) i2c_sda__PC)
/* Data Register */
#define i2c_sda_DR                     (* (reg32 *) i2c_sda__DR)
/* Input Buffer Disable Override */
#define i2c_sda_INP_DIS                (* (reg32 *) i2c_sda__PC2)


#if defined(i2c_sda__INTSTAT)  /* Interrupt Registers */

    #define i2c_sda_INTSTAT                (* (reg32 *) i2c_sda__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins i2c_sda_H */


/* [] END OF FILE */
