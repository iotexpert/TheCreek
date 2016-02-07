/*******************************************************************************
* File Name: REDLED.h  
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

#if !defined(CY_PINS_REDLED_H) /* Pins REDLED_H */
#define CY_PINS_REDLED_H

#include "cytypes.h"
#include "cyfitter.h"
#include "REDLED_aliases.h"


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
} REDLED_BACKUP_STRUCT;

/** @} structures */


/***************************************
*        Function Prototypes             
***************************************/
/**
* \addtogroup group_general
* @{
*/
uint8   REDLED_Read(void);
void    REDLED_Write(uint8 value);
uint8   REDLED_ReadDataReg(void);
#if defined(REDLED__PC) || (CY_PSOC4_4200L) 
    void    REDLED_SetDriveMode(uint8 mode);
#endif
void    REDLED_SetInterruptMode(uint16 position, uint16 mode);
uint8   REDLED_ClearInterrupt(void);
/** @} general */

/**
* \addtogroup group_power
* @{
*/
void REDLED_Sleep(void); 
void REDLED_Wakeup(void);
/** @} power */


/***************************************
*           API Constants        
***************************************/
#if defined(REDLED__PC) || (CY_PSOC4_4200L) 
    /* Drive Modes */
    #define REDLED_DRIVE_MODE_BITS        (3)
    #define REDLED_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - REDLED_DRIVE_MODE_BITS))

    /**
    * \addtogroup group_constants
    * @{
    */
        /** \addtogroup driveMode Drive mode constants
         * \brief Constants to be passed as "mode" parameter in the REDLED_SetDriveMode() function.
         *  @{
         */
        #define REDLED_DM_ALG_HIZ         (0x00u) /**< \brief High Impedance Analog   */
        #define REDLED_DM_DIG_HIZ         (0x01u) /**< \brief High Impedance Digital  */
        #define REDLED_DM_RES_UP          (0x02u) /**< \brief Resistive Pull Up       */
        #define REDLED_DM_RES_DWN         (0x03u) /**< \brief Resistive Pull Down     */
        #define REDLED_DM_OD_LO           (0x04u) /**< \brief Open Drain, Drives Low  */
        #define REDLED_DM_OD_HI           (0x05u) /**< \brief Open Drain, Drives High */
        #define REDLED_DM_STRONG          (0x06u) /**< \brief Strong Drive            */
        #define REDLED_DM_RES_UPDWN       (0x07u) /**< \brief Resistive Pull Up/Down  */
        /** @} driveMode */
    /** @} group_constants */
#endif

/* Digital Port Constants */
#define REDLED_MASK               REDLED__MASK
#define REDLED_SHIFT              REDLED__SHIFT
#define REDLED_WIDTH              1u

/**
* \addtogroup group_constants
* @{
*/
    /** \addtogroup intrMode Interrupt constants
     * \brief Constants to be passed as "mode" parameter in REDLED_SetInterruptMode() function.
     *  @{
     */
        #define REDLED_INTR_NONE      ((uint16)(0x0000u)) /**< \brief Disabled             */
        #define REDLED_INTR_RISING    ((uint16)(0x5555u)) /**< \brief Rising edge trigger  */
        #define REDLED_INTR_FALLING   ((uint16)(0xaaaau)) /**< \brief Falling edge trigger */
        #define REDLED_INTR_BOTH      ((uint16)(0xffffu)) /**< \brief Both edge trigger    */
    /** @} intrMode */
/** @} group_constants */

/* SIO LPM definition */
#if defined(REDLED__SIO)
    #define REDLED_SIO_LPM_MASK       (0x03u)
#endif

/* USBIO definitions */
#if !defined(REDLED__PC) && (CY_PSOC4_4200L)
    #define REDLED_USBIO_ENABLE               ((uint32)0x80000000u)
    #define REDLED_USBIO_DISABLE              ((uint32)(~REDLED_USBIO_ENABLE))
    #define REDLED_USBIO_SUSPEND_SHIFT        CYFLD_USBDEVv2_USB_SUSPEND__OFFSET
    #define REDLED_USBIO_SUSPEND_DEL_SHIFT    CYFLD_USBDEVv2_USB_SUSPEND_DEL__OFFSET
    #define REDLED_USBIO_ENTER_SLEEP          ((uint32)((1u << REDLED_USBIO_SUSPEND_SHIFT) \
                                                        | (1u << REDLED_USBIO_SUSPEND_DEL_SHIFT)))
    #define REDLED_USBIO_EXIT_SLEEP_PH1       ((uint32)~((uint32)(1u << REDLED_USBIO_SUSPEND_SHIFT)))
    #define REDLED_USBIO_EXIT_SLEEP_PH2       ((uint32)~((uint32)(1u << REDLED_USBIO_SUSPEND_DEL_SHIFT)))
    #define REDLED_USBIO_CR1_OFF              ((uint32)0xfffffffeu)
#endif


/***************************************
*             Registers        
***************************************/
/* Main Port Registers */
#if defined(REDLED__PC)
    /* Port Configuration */
    #define REDLED_PC                 (* (reg32 *) REDLED__PC)
#endif
/* Pin State */
#define REDLED_PS                     (* (reg32 *) REDLED__PS)
/* Data Register */
#define REDLED_DR                     (* (reg32 *) REDLED__DR)
/* Input Buffer Disable Override */
#define REDLED_INP_DIS                (* (reg32 *) REDLED__PC2)

/* Interrupt configuration Registers */
#define REDLED_INTCFG                 (* (reg32 *) REDLED__INTCFG)
#define REDLED_INTSTAT                (* (reg32 *) REDLED__INTSTAT)

/* "Interrupt cause" register for Combined Port Interrupt (AllPortInt) in GSRef component */
#if defined (CYREG_GPIO_INTR_CAUSE)
    #define REDLED_INTR_CAUSE         (* (reg32 *) CYREG_GPIO_INTR_CAUSE)
#endif

/* SIO register */
#if defined(REDLED__SIO)
    #define REDLED_SIO_REG            (* (reg32 *) REDLED__SIO)
#endif /* (REDLED__SIO_CFG) */

/* USBIO registers */
#if !defined(REDLED__PC) && (CY_PSOC4_4200L)
    #define REDLED_USB_POWER_REG       (* (reg32 *) CYREG_USBDEVv2_USB_POWER_CTRL)
    #define REDLED_CR1_REG             (* (reg32 *) CYREG_USBDEVv2_CR1)
    #define REDLED_USBIO_CTRL_REG      (* (reg32 *) CYREG_USBDEVv2_USB_USBIO_CTRL)
#endif    
    
    
/***************************************
* The following code is DEPRECATED and 
* must not be used in new designs.
***************************************/
/**
* \addtogroup group_deprecated
* @{
*/
#define REDLED_DRIVE_MODE_SHIFT       (0x00u)
#define REDLED_DRIVE_MODE_MASK        (0x07u << REDLED_DRIVE_MODE_SHIFT)
/** @} deprecated */

#endif /* End Pins REDLED_H */


/* [] END OF FILE */
