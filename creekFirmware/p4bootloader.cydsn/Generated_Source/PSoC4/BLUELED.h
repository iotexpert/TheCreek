/*******************************************************************************
* File Name: BLUELED.h  
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

#if !defined(CY_PINS_BLUELED_H) /* Pins BLUELED_H */
#define CY_PINS_BLUELED_H

#include "cytypes.h"
#include "cyfitter.h"
#include "BLUELED_aliases.h"


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
} BLUELED_BACKUP_STRUCT;

/** @} structures */


/***************************************
*        Function Prototypes             
***************************************/
/**
* \addtogroup group_general
* @{
*/
uint8   BLUELED_Read(void);
void    BLUELED_Write(uint8 value);
uint8   BLUELED_ReadDataReg(void);
#if defined(BLUELED__PC) || (CY_PSOC4_4200L) 
    void    BLUELED_SetDriveMode(uint8 mode);
#endif
void    BLUELED_SetInterruptMode(uint16 position, uint16 mode);
uint8   BLUELED_ClearInterrupt(void);
/** @} general */

/**
* \addtogroup group_power
* @{
*/
void BLUELED_Sleep(void); 
void BLUELED_Wakeup(void);
/** @} power */


/***************************************
*           API Constants        
***************************************/
#if defined(BLUELED__PC) || (CY_PSOC4_4200L) 
    /* Drive Modes */
    #define BLUELED_DRIVE_MODE_BITS        (3)
    #define BLUELED_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - BLUELED_DRIVE_MODE_BITS))

    /**
    * \addtogroup group_constants
    * @{
    */
        /** \addtogroup driveMode Drive mode constants
         * \brief Constants to be passed as "mode" parameter in the BLUELED_SetDriveMode() function.
         *  @{
         */
        #define BLUELED_DM_ALG_HIZ         (0x00u) /**< \brief High Impedance Analog   */
        #define BLUELED_DM_DIG_HIZ         (0x01u) /**< \brief High Impedance Digital  */
        #define BLUELED_DM_RES_UP          (0x02u) /**< \brief Resistive Pull Up       */
        #define BLUELED_DM_RES_DWN         (0x03u) /**< \brief Resistive Pull Down     */
        #define BLUELED_DM_OD_LO           (0x04u) /**< \brief Open Drain, Drives Low  */
        #define BLUELED_DM_OD_HI           (0x05u) /**< \brief Open Drain, Drives High */
        #define BLUELED_DM_STRONG          (0x06u) /**< \brief Strong Drive            */
        #define BLUELED_DM_RES_UPDWN       (0x07u) /**< \brief Resistive Pull Up/Down  */
        /** @} driveMode */
    /** @} group_constants */
#endif

/* Digital Port Constants */
#define BLUELED_MASK               BLUELED__MASK
#define BLUELED_SHIFT              BLUELED__SHIFT
#define BLUELED_WIDTH              1u

/**
* \addtogroup group_constants
* @{
*/
    /** \addtogroup intrMode Interrupt constants
     * \brief Constants to be passed as "mode" parameter in BLUELED_SetInterruptMode() function.
     *  @{
     */
        #define BLUELED_INTR_NONE      ((uint16)(0x0000u)) /**< \brief Disabled             */
        #define BLUELED_INTR_RISING    ((uint16)(0x5555u)) /**< \brief Rising edge trigger  */
        #define BLUELED_INTR_FALLING   ((uint16)(0xaaaau)) /**< \brief Falling edge trigger */
        #define BLUELED_INTR_BOTH      ((uint16)(0xffffu)) /**< \brief Both edge trigger    */
    /** @} intrMode */
/** @} group_constants */

/* SIO LPM definition */
#if defined(BLUELED__SIO)
    #define BLUELED_SIO_LPM_MASK       (0x03u)
#endif

/* USBIO definitions */
#if !defined(BLUELED__PC) && (CY_PSOC4_4200L)
    #define BLUELED_USBIO_ENABLE               ((uint32)0x80000000u)
    #define BLUELED_USBIO_DISABLE              ((uint32)(~BLUELED_USBIO_ENABLE))
    #define BLUELED_USBIO_SUSPEND_SHIFT        CYFLD_USBDEVv2_USB_SUSPEND__OFFSET
    #define BLUELED_USBIO_SUSPEND_DEL_SHIFT    CYFLD_USBDEVv2_USB_SUSPEND_DEL__OFFSET
    #define BLUELED_USBIO_ENTER_SLEEP          ((uint32)((1u << BLUELED_USBIO_SUSPEND_SHIFT) \
                                                        | (1u << BLUELED_USBIO_SUSPEND_DEL_SHIFT)))
    #define BLUELED_USBIO_EXIT_SLEEP_PH1       ((uint32)~((uint32)(1u << BLUELED_USBIO_SUSPEND_SHIFT)))
    #define BLUELED_USBIO_EXIT_SLEEP_PH2       ((uint32)~((uint32)(1u << BLUELED_USBIO_SUSPEND_DEL_SHIFT)))
    #define BLUELED_USBIO_CR1_OFF              ((uint32)0xfffffffeu)
#endif


/***************************************
*             Registers        
***************************************/
/* Main Port Registers */
#if defined(BLUELED__PC)
    /* Port Configuration */
    #define BLUELED_PC                 (* (reg32 *) BLUELED__PC)
#endif
/* Pin State */
#define BLUELED_PS                     (* (reg32 *) BLUELED__PS)
/* Data Register */
#define BLUELED_DR                     (* (reg32 *) BLUELED__DR)
/* Input Buffer Disable Override */
#define BLUELED_INP_DIS                (* (reg32 *) BLUELED__PC2)

/* Interrupt configuration Registers */
#define BLUELED_INTCFG                 (* (reg32 *) BLUELED__INTCFG)
#define BLUELED_INTSTAT                (* (reg32 *) BLUELED__INTSTAT)

/* "Interrupt cause" register for Combined Port Interrupt (AllPortInt) in GSRef component */
#if defined (CYREG_GPIO_INTR_CAUSE)
    #define BLUELED_INTR_CAUSE         (* (reg32 *) CYREG_GPIO_INTR_CAUSE)
#endif

/* SIO register */
#if defined(BLUELED__SIO)
    #define BLUELED_SIO_REG            (* (reg32 *) BLUELED__SIO)
#endif /* (BLUELED__SIO_CFG) */

/* USBIO registers */
#if !defined(BLUELED__PC) && (CY_PSOC4_4200L)
    #define BLUELED_USB_POWER_REG       (* (reg32 *) CYREG_USBDEVv2_USB_POWER_CTRL)
    #define BLUELED_CR1_REG             (* (reg32 *) CYREG_USBDEVv2_CR1)
    #define BLUELED_USBIO_CTRL_REG      (* (reg32 *) CYREG_USBDEVv2_USB_USBIO_CTRL)
#endif    
    
    
/***************************************
* The following code is DEPRECATED and 
* must not be used in new designs.
***************************************/
/**
* \addtogroup group_deprecated
* @{
*/
#define BLUELED_DRIVE_MODE_SHIFT       (0x00u)
#define BLUELED_DRIVE_MODE_MASK        (0x07u << BLUELED_DRIVE_MODE_SHIFT)
/** @} deprecated */

#endif /* End Pins BLUELED_H */


/* [] END OF FILE */
