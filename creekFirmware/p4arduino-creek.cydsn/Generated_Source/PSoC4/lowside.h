/*******************************************************************************
* File Name: lowside.h  
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

#if !defined(CY_PINS_lowside_H) /* Pins lowside_H */
#define CY_PINS_lowside_H

#include "cytypes.h"
#include "cyfitter.h"
#include "lowside_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    lowside_Write(uint8 value) ;
void    lowside_SetDriveMode(uint8 mode) ;
uint8   lowside_ReadDataReg(void) ;
uint8   lowside_Read(void) ;
uint8   lowside_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define lowside_DRIVE_MODE_BITS        (3)
#define lowside_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - lowside_DRIVE_MODE_BITS))
#define lowside_DRIVE_MODE_SHIFT       (0x00u)
#define lowside_DRIVE_MODE_MASK        (0x07u << lowside_DRIVE_MODE_SHIFT)

#define lowside_DM_ALG_HIZ         (0x00u << lowside_DRIVE_MODE_SHIFT)
#define lowside_DM_DIG_HIZ         (0x01u << lowside_DRIVE_MODE_SHIFT)
#define lowside_DM_RES_UP          (0x02u << lowside_DRIVE_MODE_SHIFT)
#define lowside_DM_RES_DWN         (0x03u << lowside_DRIVE_MODE_SHIFT)
#define lowside_DM_OD_LO           (0x04u << lowside_DRIVE_MODE_SHIFT)
#define lowside_DM_OD_HI           (0x05u << lowside_DRIVE_MODE_SHIFT)
#define lowside_DM_STRONG          (0x06u << lowside_DRIVE_MODE_SHIFT)
#define lowside_DM_RES_UPDWN       (0x07u << lowside_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define lowside_MASK               lowside__MASK
#define lowside_SHIFT              lowside__SHIFT
#define lowside_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define lowside_PS                     (* (reg32 *) lowside__PS)
/* Port Configuration */
#define lowside_PC                     (* (reg32 *) lowside__PC)
/* Data Register */
#define lowside_DR                     (* (reg32 *) lowside__DR)
/* Input Buffer Disable Override */
#define lowside_INP_DIS                (* (reg32 *) lowside__PC2)


#if defined(lowside__INTSTAT)  /* Interrupt Registers */

    #define lowside_INTSTAT                (* (reg32 *) lowside__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins lowside_H */


/* [] END OF FILE */
