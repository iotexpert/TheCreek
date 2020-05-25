/***************************************************************************//**
* \file .h
* \version 4.0
*
* \brief
*  This private file provides constants and parameter values for the
*  SCB Component.
*  Please do not use this file or its content in your project.
*
* Note:
*
********************************************************************************
* \copyright
* Copyright 2013-2017, Cypress Semiconductor Corporation. All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SCB_PVT_ezi2c_H)
#define CY_SCB_PVT_ezi2c_H

#include "ezi2c.h"


/***************************************
*     Private Function Prototypes
***************************************/

/* APIs to service INTR_I2C_EC register */
#define ezi2c_SetI2CExtClkInterruptMode(interruptMask) ezi2c_WRITE_INTR_I2C_EC_MASK(interruptMask)
#define ezi2c_ClearI2CExtClkInterruptSource(interruptMask) ezi2c_CLEAR_INTR_I2C_EC(interruptMask)
#define ezi2c_GetI2CExtClkInterruptSource()                (ezi2c_INTR_I2C_EC_REG)
#define ezi2c_GetI2CExtClkInterruptMode()                  (ezi2c_INTR_I2C_EC_MASK_REG)
#define ezi2c_GetI2CExtClkInterruptSourceMasked()          (ezi2c_INTR_I2C_EC_MASKED_REG)

#if (!ezi2c_CY_SCBIP_V1)
    /* APIs to service INTR_SPI_EC register */
    #define ezi2c_SetSpiExtClkInterruptMode(interruptMask) \
                                                                ezi2c_WRITE_INTR_SPI_EC_MASK(interruptMask)
    #define ezi2c_ClearSpiExtClkInterruptSource(interruptMask) \
                                                                ezi2c_CLEAR_INTR_SPI_EC(interruptMask)
    #define ezi2c_GetExtSpiClkInterruptSource()                 (ezi2c_INTR_SPI_EC_REG)
    #define ezi2c_GetExtSpiClkInterruptMode()                   (ezi2c_INTR_SPI_EC_MASK_REG)
    #define ezi2c_GetExtSpiClkInterruptSourceMasked()           (ezi2c_INTR_SPI_EC_MASKED_REG)
#endif /* (!ezi2c_CY_SCBIP_V1) */

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
    extern void ezi2c_SetPins(uint32 mode, uint32 subMode, uint32 uartEnableMask);
#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */


/***************************************
*     Vars with External Linkage
***************************************/

#if (ezi2c_SCB_IRQ_INTERNAL)
#if !defined (CY_REMOVE_ezi2c_CUSTOM_INTR_HANDLER)
    extern cyisraddress ezi2c_customIntrHandler;
#endif /* !defined (CY_REMOVE_ezi2c_CUSTOM_INTR_HANDLER) */
#endif /* (ezi2c_SCB_IRQ_INTERNAL) */

extern ezi2c_BACKUP_STRUCT ezi2c_backup;

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
    /* Common configuration variables */
    extern uint8 ezi2c_scbMode;
    extern uint8 ezi2c_scbEnableWake;
    extern uint8 ezi2c_scbEnableIntr;

    /* I2C configuration variables */
    extern uint8 ezi2c_mode;
    extern uint8 ezi2c_acceptAddr;

    /* SPI/UART configuration variables */
    extern volatile uint8 * ezi2c_rxBuffer;
    extern uint8   ezi2c_rxDataBits;
    extern uint32  ezi2c_rxBufferSize;

    extern volatile uint8 * ezi2c_txBuffer;
    extern uint8   ezi2c_txDataBits;
    extern uint32  ezi2c_txBufferSize;

    /* EZI2C configuration variables */
    extern uint8 ezi2c_numberOfAddr;
    extern uint8 ezi2c_subAddrSize;
#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */

#if (! (ezi2c_SCB_MODE_I2C_CONST_CFG || \
        ezi2c_SCB_MODE_EZI2C_CONST_CFG))
    extern uint16 ezi2c_IntrTxMask;
#endif /* (! (ezi2c_SCB_MODE_I2C_CONST_CFG || \
              ezi2c_SCB_MODE_EZI2C_CONST_CFG)) */


/***************************************
*        Conditional Macro
****************************************/

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
    /* Defines run time operation mode */
    #define ezi2c_SCB_MODE_I2C_RUNTM_CFG     (ezi2c_SCB_MODE_I2C      == ezi2c_scbMode)
    #define ezi2c_SCB_MODE_SPI_RUNTM_CFG     (ezi2c_SCB_MODE_SPI      == ezi2c_scbMode)
    #define ezi2c_SCB_MODE_UART_RUNTM_CFG    (ezi2c_SCB_MODE_UART     == ezi2c_scbMode)
    #define ezi2c_SCB_MODE_EZI2C_RUNTM_CFG   (ezi2c_SCB_MODE_EZI2C    == ezi2c_scbMode)
    #define ezi2c_SCB_MODE_UNCONFIG_RUNTM_CFG \
                                                        (ezi2c_SCB_MODE_UNCONFIG == ezi2c_scbMode)

    /* Defines wakeup enable */
    #define ezi2c_SCB_WAKE_ENABLE_CHECK       (0u != ezi2c_scbEnableWake)
#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */

/* Defines maximum number of SCB pins */
#if (!ezi2c_CY_SCBIP_V1)
    #define ezi2c_SCB_PINS_NUMBER    (7u)
#else
    #define ezi2c_SCB_PINS_NUMBER    (2u)
#endif /* (!ezi2c_CY_SCBIP_V1) */

#endif /* (CY_SCB_PVT_ezi2c_H) */


/* [] END OF FILE */
