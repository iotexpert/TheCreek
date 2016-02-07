/*******************************************************************************
* File Name: V036.h  
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

#if !defined(CY_PINS_V036_H) /* Pins V036_H */
#define CY_PINS_V036_H

#include "cytypes.h"
#include "cyfitter.h"
#include "V036_aliases.h"


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
} V036_BACKUP_STRUCT;

/** @} structures */


/***************************************
*        Function Prototypes             
***************************************/
/**
* \addtogroup group_general
* @{
*/
uint8   V036_Read(void);
void    V036_Write(uint8 value);
uint8   V036_ReadDataReg(void);
#if defined(V036__PC) || (CY_PSOC4_4200L) 
    void    V036_SetDriveMode(uint8 mode);
#endif
void    V036_SetInterruptMode(uint16 position, uint16 mode);
uint8   V036_ClearInterrupt(void);
/** @} general */

/**
* \addtogroup group_power
* @{
*/
void V036_Sleep(void); 
void V036_Wakeup(void);
/** @} power */


/***************************************
*           API Constants        
***************************************/
#if defined(V036__PC) || (CY_PSOC4_4200L) 
    /* Drive Modes */
    #define V036_DRIVE_MODE_BITS        (3)
    #define V036_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - V036_DRIVE_MODE_BITS))

    /**
    * \addtogroup group_constants
    * @{
    */
        /** \addtogroup driveMode Drive mode constants
         * \brief Constants to be passed as "mode" parameter in the V036_SetDriveMode() function.
         *  @{
         */
        #define V036_DM_ALG_HIZ         (0x00u) /**< \brief High Impedance Analog   */
        #define V036_DM_DIG_HIZ         (0x01u) /**< \brief High Impedance Digital  */
        #define V036_DM_RES_UP          (0x02u) /**< \brief Resistive Pull Up       */
        #define V036_DM_RES_DWN         (0x03u) /**< \brief Resistive Pull Down     */
        #define V036_DM_OD_LO           (0x04u) /**< \brief Open Drain, Drives Low  */
        #define V036_DM_OD_HI           (0x05u) /**< \brief Open Drain, Drives High */
        #define V036_DM_STRONG          (0x06u) /**< \brief Strong Drive            */
        #define V036_DM_RES_UPDWN       (0x07u) /**< \brief Resistive Pull Up/Down  */
        /** @} driveMode */
    /** @} group_constants */
#endif

/* Digital Port Constants */
#define V036_MASK               V036__MASK
#define V036_SHIFT              V036__SHIFT
#define V036_WIDTH              1u

/**
* \addtogroup group_constants
* @{
*/
    /** \addtogroup intrMode Interrupt constants
     * \brief Constants to be passed as "mode" parameter in V036_SetInterruptMode() function.
     *  @{
     */
        #define V036_INTR_NONE      ((uint16)(0x0000u)) /**< \brief Disabled             */
        #define V036_INTR_RISING    ((uint16)(0x5555u)) /**< \brief Rising edge trigger  */
        #define V036_INTR_FALLING   ((uint16)(0xaaaau)) /**< \brief Falling edge trigger */
        #define V036_INTR_BOTH      ((uint16)(0xffffu)) /**< \brief Both edge trigger    */
    /** @} intrMode */
/** @} group_constants */

/* SIO LPM definition */
#if defined(V036__SIO)
    #define V036_SIO_LPM_MASK       (0x03u)
#endif

/* USBIO definitions */
#if !defined(V036__PC) && (CY_PSOC4_4200L)
    #define V036_USBIO_ENABLE               ((uint32)0x80000000u)
    #define V036_USBIO_DISABLE              ((uint32)(~V036_USBIO_ENABLE))
    #define V036_USBIO_SUSPEND_SHIFT        CYFLD_USBDEVv2_USB_SUSPEND__OFFSET
    #define V036_USBIO_SUSPEND_DEL_SHIFT    CYFLD_USBDEVv2_USB_SUSPEND_DEL__OFFSET
    #define V036_USBIO_ENTER_SLEEP          ((uint32)((1u << V036_USBIO_SUSPEND_SHIFT) \
                                                        | (1u << V036_USBIO_SUSPEND_DEL_SHIFT)))
    #define V036_USBIO_EXIT_SLEEP_PH1       ((uint32)~((uint32)(1u << V036_USBIO_SUSPEND_SHIFT)))
    #define V036_USBIO_EXIT_SLEEP_PH2       ((uint32)~((uint32)(1u << V036_USBIO_SUSPEND_DEL_SHIFT)))
    #define V036_USBIO_CR1_OFF              ((uint32)0xfffffffeu)
#endif


/***************************************
*             Registers        
***************************************/
/* Main Port Registers */
#if defined(V036__PC)
    /* Port Configuration */
    #define V036_PC                 (* (reg32 *) V036__PC)
#endif
/* Pin State */
#define V036_PS                     (* (reg32 *) V036__PS)
/* Data Register */
#define V036_DR                     (* (reg32 *) V036__DR)
/* Input Buffer Disable Override */
#define V036_INP_DIS                (* (reg32 *) V036__PC2)

/* Interrupt configuration Registers */
#define V036_INTCFG                 (* (reg32 *) V036__INTCFG)
#define V036_INTSTAT                (* (reg32 *) V036__INTSTAT)

/* "Interrupt cause" register for Combined Port Interrupt (AllPortInt) in GSRef component */
#if defined (CYREG_GPIO_INTR_CAUSE)
    #define V036_INTR_CAUSE         (* (reg32 *) CYREG_GPIO_INTR_CAUSE)
#endif

/* SIO register */
#if defined(V036__SIO)
    #define V036_SIO_REG            (* (reg32 *) V036__SIO)
#endif /* (V036__SIO_CFG) */

/* USBIO registers */
#if !defined(V036__PC) && (CY_PSOC4_4200L)
    #define V036_USB_POWER_REG       (* (reg32 *) CYREG_USBDEVv2_USB_POWER_CTRL)
    #define V036_CR1_REG             (* (reg32 *) CYREG_USBDEVv2_CR1)
    #define V036_USBIO_CTRL_REG      (* (reg32 *) CYREG_USBDEVv2_USB_USBIO_CTRL)
#endif    
    
    
/***************************************
* The following code is DEPRECATED and 
* must not be used in new designs.
***************************************/
/**
* \addtogroup group_deprecated
* @{
*/
#define V036_DRIVE_MODE_SHIFT       (0x00u)
#define V036_DRIVE_MODE_MASK        (0x07u << V036_DRIVE_MODE_SHIFT)
/** @} deprecated */

#endif /* End Pins V036_H */


/* [] END OF FILE */
