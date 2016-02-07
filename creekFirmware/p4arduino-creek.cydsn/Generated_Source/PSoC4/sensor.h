/*******************************************************************************
* File Name: sensor.h  
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

#if !defined(CY_PINS_sensor_H) /* Pins sensor_H */
#define CY_PINS_sensor_H

#include "cytypes.h"
#include "cyfitter.h"
#include "sensor_aliases.h"


/***************************************
*        Function Prototypes             
***************************************/    

void    sensor_Write(uint8 value) ;
void    sensor_SetDriveMode(uint8 mode) ;
uint8   sensor_ReadDataReg(void) ;
uint8   sensor_Read(void) ;
uint8   sensor_ClearInterrupt(void) ;


/***************************************
*           API Constants        
***************************************/

/* Drive Modes */
#define sensor_DRIVE_MODE_BITS        (3)
#define sensor_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - sensor_DRIVE_MODE_BITS))
#define sensor_DRIVE_MODE_SHIFT       (0x00u)
#define sensor_DRIVE_MODE_MASK        (0x07u << sensor_DRIVE_MODE_SHIFT)

#define sensor_DM_ALG_HIZ         (0x00u << sensor_DRIVE_MODE_SHIFT)
#define sensor_DM_DIG_HIZ         (0x01u << sensor_DRIVE_MODE_SHIFT)
#define sensor_DM_RES_UP          (0x02u << sensor_DRIVE_MODE_SHIFT)
#define sensor_DM_RES_DWN         (0x03u << sensor_DRIVE_MODE_SHIFT)
#define sensor_DM_OD_LO           (0x04u << sensor_DRIVE_MODE_SHIFT)
#define sensor_DM_OD_HI           (0x05u << sensor_DRIVE_MODE_SHIFT)
#define sensor_DM_STRONG          (0x06u << sensor_DRIVE_MODE_SHIFT)
#define sensor_DM_RES_UPDWN       (0x07u << sensor_DRIVE_MODE_SHIFT)

/* Digital Port Constants */
#define sensor_MASK               sensor__MASK
#define sensor_SHIFT              sensor__SHIFT
#define sensor_WIDTH              1u


/***************************************
*             Registers        
***************************************/

/* Main Port Registers */
/* Pin State */
#define sensor_PS                     (* (reg32 *) sensor__PS)
/* Port Configuration */
#define sensor_PC                     (* (reg32 *) sensor__PC)
/* Data Register */
#define sensor_DR                     (* (reg32 *) sensor__DR)
/* Input Buffer Disable Override */
#define sensor_INP_DIS                (* (reg32 *) sensor__PC2)


#if defined(sensor__INTSTAT)  /* Interrupt Registers */

    #define sensor_INTSTAT                (* (reg32 *) sensor__INTSTAT)

#endif /* Interrupt Registers */

#endif /* End Pins sensor_H */


/* [] END OF FILE */
