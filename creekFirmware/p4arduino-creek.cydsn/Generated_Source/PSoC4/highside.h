/*******************************************************************************
* File Name: highside.h  
* Version 2.20
*
* Description:
*  This file contains Pin function prototypes and register defines
*
********************************************************************************
* Copyright 2008-2015, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_PINS_highside_H) /* Pins highside_H */
#define CY_PINS_highside_H

#include "cytypes.h"
#include "cyfitter.h"
#include "highside_aliases.h"


/***************************************
*     Data Struct Definitions
***************************************/

/**
* \addtogroup group_structures
* @{
*/
    
/* Structure for sleep mode support */
typedef struct
{
    uint32 pcState; /**< State of the port control register */
    uint32 sioState; /**< State of the SIO configuration */
    uint32 usbState; /**< State of the USBIO regulator */
} highside_BACKUP_STRUCT;

/** @} structures */


/***************************************
*        Function Prototypes             
***************************************/
/**
* \addtogroup group_general
* @{
*/
uint8   highside_Read(void);
void    highside_Write(uint8 value);
uint8   highside_ReadDataReg(void);
#if defined(highside__PC) || (CY_PSOC4_4200L) 
    void    highside_SetDriveMode(uint8 mode);
#endif
void    highside_SetInterruptMode(uint16 position, uint16 mode);
uint8   highside_ClearInterrupt(void);
/** @} general */

/**
* \addtogroup group_power
* @{
*/
void highside_Sleep(void); 
void highside_Wakeup(void);
/** @} power */


/***************************************
*           API Constants        
***************************************/
#if defined(highside__PC) || (CY_PSOC4_4200L) 
    /* Drive Modes */
    #define highside_DRIVE_MODE_BITS        (3)
    #define highside_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - highside_DRIVE_MODE_BITS))

    /**
    * \addtogroup group_constants
    * @{
    */
        /** \addtogroup driveMode Drive mode constants
         * \brief Constants to be passed as "mode" parameter in the highside_SetDriveMode() function.
         *  @{
         */
        #define highside_DM_ALG_HIZ         (0x00u) /**< \brief High Impedance Analog   */
        #define highside_DM_DIG_HIZ         (0x01u) /**< \brief High Impedance Digital  */
        #define highside_DM_RES_UP          (0x02u) /**< \brief Resistive Pull Up       */
        #define highside_DM_RES_DWN         (0x03u) /**< \brief Resistive Pull Down     */
        #define highside_DM_OD_LO           (0x04u) /**< \brief Open Drain, Drives Low  */
        #define highside_DM_OD_HI           (0x05u) /**< \brief Open Drain, Drives High */
        #define highside_DM_STRONG          (0x06u) /**< \brief Strong Drive            */
        #define highside_DM_RES_UPDWN       (0x07u) /**< \brief Resistive Pull Up/Down  */
        /** @} driveMode */
    /** @} group_constants */
#endif

/* Digital Port Constants */
#define highside_MASK               highside__MASK
#define highside_SHIFT              highside__SHIFT
#define highside_WIDTH              1u

/**
* \addtogroup group_constants
* @{
*/
    /** \addtogroup intrMode Interrupt constants
     * \brief Constants to be passed as "mode" parameter in highside_SetInterruptMode() function.
     *  @{
     */
        #define highside_INTR_NONE      ((uint16)(0x0000u)) /**< \brief Disabled             */
        #define highside_INTR_RISING    ((uint16)(0x5555u)) /**< \brief Rising edge trigger  */
        #define highside_INTR_FALLING   ((uint16)(0xaaaau)) /**< \brief Falling edge trigger */
        #define highside_INTR_BOTH      ((uint16)(0xffffu)) /**< \brief Both edge trigger    */
    /** @} intrMode */
/** @} group_constants */

/* SIO LPM definition */
#if defined(highside__SIO)
    #define highside_SIO_LPM_MASK       (0x03u)
#endif

/* USBIO definitions */
#if !defined(highside__PC) && (CY_PSOC4_4200L)
    #define highside_USBIO_ENABLE               ((uint32)0x80000000u)
    #define highside_USBIO_DISABLE              ((uint32)(~highside_USBIO_ENABLE))
    #define highside_USBIO_SUSPEND_SHIFT        CYFLD_USBDEVv2_USB_SUSPEND__OFFSET
    #define highside_USBIO_SUSPEND_DEL_SHIFT    CYFLD_USBDEVv2_USB_SUSPEND_DEL__OFFSET
    #define highside_USBIO_ENTER_SLEEP          ((uint32)((1u << highside_USBIO_SUSPEND_SHIFT) \
                                                        | (1u << highside_USBIO_SUSPEND_DEL_SHIFT)))
    #define highside_USBIO_EXIT_SLEEP_PH1       ((uint32)~((uint32)(1u << highside_USBIO_SUSPEND_SHIFT)))
    #define highside_USBIO_EXIT_SLEEP_PH2       ((uint32)~((uint32)(1u << highside_USBIO_SUSPEND_DEL_SHIFT)))
    #define highside_USBIO_CR1_OFF              ((uint32)0xfffffffeu)
#endif


/***************************************
*             Registers        
***************************************/
/* Main Port Registers */
#if defined(highside__PC)
    /* Port Configuration */
    #define highside_PC                 (* (reg32 *) highside__PC)
#endif
/* Pin State */
#define highside_PS                     (* (reg32 *) highside__PS)
/* Data Register */
#define highside_DR                     (* (reg32 *) highside__DR)
/* Input Buffer Disable Override */
#define highside_INP_DIS                (* (reg32 *) highside__PC2)

/* Interrupt configuration Registers */
#define highside_INTCFG                 (* (reg32 *) highside__INTCFG)
#define highside_INTSTAT                (* (reg32 *) highside__INTSTAT)

/* "Interrupt cause" register for Combined Port Interrupt (AllPortInt) in GSRef component */
#if defined (CYREG_GPIO_INTR_CAUSE)
    #define highside_INTR_CAUSE         (* (reg32 *) CYREG_GPIO_INTR_CAUSE)
#endif

/* SIO register */
#if defined(highside__SIO)
    #define highside_SIO_REG            (* (reg32 *) highside__SIO)
#endif /* (highside__SIO_CFG) */

/* USBIO registers */
#if !defined(highside__PC) && (CY_PSOC4_4200L)
    #define highside_USB_POWER_REG       (* (reg32 *) CYREG_USBDEVv2_USB_POWER_CTRL)
    #define highside_CR1_REG             (* (reg32 *) CYREG_USBDEVv2_CR1)
    #define highside_USBIO_CTRL_REG      (* (reg32 *) CYREG_USBDEVv2_USB_USBIO_CTRL)
#endif    
    
    
/***************************************
* The following code is DEPRECATED and 
* must not be used in new designs.
***************************************/
/**
* \addtogroup group_deprecated
* @{
*/
#define highside_DRIVE_MODE_SHIFT       (0x00u)
#define highside_DRIVE_MODE_MASK        (0x07u << highside_DRIVE_MODE_SHIFT)
/** @} deprecated */

#endif /* End Pins highside_H */


/* [] END OF FILE */
