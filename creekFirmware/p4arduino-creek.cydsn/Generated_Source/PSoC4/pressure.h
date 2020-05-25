/*******************************************************************************
* File Name: pressure.h  
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

#if !defined(CY_PINS_pressure_H) /* Pins pressure_H */
#define CY_PINS_pressure_H

#include "cytypes.h"
#include "cyfitter.h"
#include "pressure_aliases.h"


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
} pressure_BACKUP_STRUCT;

/** @} structures */


/***************************************
*        Function Prototypes             
***************************************/
/**
* \addtogroup group_general
* @{
*/
uint8   pressure_Read(void);
void    pressure_Write(uint8 value);
uint8   pressure_ReadDataReg(void);
#if defined(pressure__PC) || (CY_PSOC4_4200L) 
    void    pressure_SetDriveMode(uint8 mode);
#endif
void    pressure_SetInterruptMode(uint16 position, uint16 mode);
uint8   pressure_ClearInterrupt(void);
/** @} general */

/**
* \addtogroup group_power
* @{
*/
void pressure_Sleep(void); 
void pressure_Wakeup(void);
/** @} power */


/***************************************
*           API Constants        
***************************************/
#if defined(pressure__PC) || (CY_PSOC4_4200L) 
    /* Drive Modes */
    #define pressure_DRIVE_MODE_BITS        (3)
    #define pressure_DRIVE_MODE_IND_MASK    (0xFFFFFFFFu >> (32 - pressure_DRIVE_MODE_BITS))

    /**
    * \addtogroup group_constants
    * @{
    */
        /** \addtogroup driveMode Drive mode constants
         * \brief Constants to be passed as "mode" parameter in the pressure_SetDriveMode() function.
         *  @{
         */
        #define pressure_DM_ALG_HIZ         (0x00u) /**< \brief High Impedance Analog   */
        #define pressure_DM_DIG_HIZ         (0x01u) /**< \brief High Impedance Digital  */
        #define pressure_DM_RES_UP          (0x02u) /**< \brief Resistive Pull Up       */
        #define pressure_DM_RES_DWN         (0x03u) /**< \brief Resistive Pull Down     */
        #define pressure_DM_OD_LO           (0x04u) /**< \brief Open Drain, Drives Low  */
        #define pressure_DM_OD_HI           (0x05u) /**< \brief Open Drain, Drives High */
        #define pressure_DM_STRONG          (0x06u) /**< \brief Strong Drive            */
        #define pressure_DM_RES_UPDWN       (0x07u) /**< \brief Resistive Pull Up/Down  */
        /** @} driveMode */
    /** @} group_constants */
#endif

/* Digital Port Constants */
#define pressure_MASK               pressure__MASK
#define pressure_SHIFT              pressure__SHIFT
#define pressure_WIDTH              1u

/**
* \addtogroup group_constants
* @{
*/
    /** \addtogroup intrMode Interrupt constants
     * \brief Constants to be passed as "mode" parameter in pressure_SetInterruptMode() function.
     *  @{
     */
        #define pressure_INTR_NONE      ((uint16)(0x0000u)) /**< \brief Disabled             */
        #define pressure_INTR_RISING    ((uint16)(0x5555u)) /**< \brief Rising edge trigger  */
        #define pressure_INTR_FALLING   ((uint16)(0xaaaau)) /**< \brief Falling edge trigger */
        #define pressure_INTR_BOTH      ((uint16)(0xffffu)) /**< \brief Both edge trigger    */
    /** @} intrMode */
/** @} group_constants */

/* SIO LPM definition */
#if defined(pressure__SIO)
    #define pressure_SIO_LPM_MASK       (0x03u)
#endif

/* USBIO definitions */
#if !defined(pressure__PC) && (CY_PSOC4_4200L)
    #define pressure_USBIO_ENABLE               ((uint32)0x80000000u)
    #define pressure_USBIO_DISABLE              ((uint32)(~pressure_USBIO_ENABLE))
    #define pressure_USBIO_SUSPEND_SHIFT        CYFLD_USBDEVv2_USB_SUSPEND__OFFSET
    #define pressure_USBIO_SUSPEND_DEL_SHIFT    CYFLD_USBDEVv2_USB_SUSPEND_DEL__OFFSET
    #define pressure_USBIO_ENTER_SLEEP          ((uint32)((1u << pressure_USBIO_SUSPEND_SHIFT) \
                                                        | (1u << pressure_USBIO_SUSPEND_DEL_SHIFT)))
    #define pressure_USBIO_EXIT_SLEEP_PH1       ((uint32)~((uint32)(1u << pressure_USBIO_SUSPEND_SHIFT)))
    #define pressure_USBIO_EXIT_SLEEP_PH2       ((uint32)~((uint32)(1u << pressure_USBIO_SUSPEND_DEL_SHIFT)))
    #define pressure_USBIO_CR1_OFF              ((uint32)0xfffffffeu)
#endif


/***************************************
*             Registers        
***************************************/
/* Main Port Registers */
#if defined(pressure__PC)
    /* Port Configuration */
    #define pressure_PC                 (* (reg32 *) pressure__PC)
#endif
/* Pin State */
#define pressure_PS                     (* (reg32 *) pressure__PS)
/* Data Register */
#define pressure_DR                     (* (reg32 *) pressure__DR)
/* Input Buffer Disable Override */
#define pressure_INP_DIS                (* (reg32 *) pressure__PC2)

/* Interrupt configuration Registers */
#define pressure_INTCFG                 (* (reg32 *) pressure__INTCFG)
#define pressure_INTSTAT                (* (reg32 *) pressure__INTSTAT)

/* "Interrupt cause" register for Combined Port Interrupt (AllPortInt) in GSRef component */
#if defined (CYREG_GPIO_INTR_CAUSE)
    #define pressure_INTR_CAUSE         (* (reg32 *) CYREG_GPIO_INTR_CAUSE)
#endif

/* SIO register */
#if defined(pressure__SIO)
    #define pressure_SIO_REG            (* (reg32 *) pressure__SIO)
#endif /* (pressure__SIO_CFG) */

/* USBIO registers */
#if !defined(pressure__PC) && (CY_PSOC4_4200L)
    #define pressure_USB_POWER_REG       (* (reg32 *) CYREG_USBDEVv2_USB_POWER_CTRL)
    #define pressure_CR1_REG             (* (reg32 *) CYREG_USBDEVv2_CR1)
    #define pressure_USBIO_CTRL_REG      (* (reg32 *) CYREG_USBDEVv2_USB_USBIO_CTRL)
#endif    
    
    
/***************************************
* The following code is DEPRECATED and 
* must not be used in new designs.
***************************************/
/**
* \addtogroup group_deprecated
* @{
*/
#define pressure_DRIVE_MODE_SHIFT       (0x00u)
#define pressure_DRIVE_MODE_MASK        (0x07u << pressure_DRIVE_MODE_SHIFT)
/** @} deprecated */

#endif /* End Pins pressure_H */


/* [] END OF FILE */
