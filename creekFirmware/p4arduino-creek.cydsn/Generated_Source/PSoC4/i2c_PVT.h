/*******************************************************************************
* File Name: .h
* Version 1.10
*
* Description:
*  This private file provides constants and parameter values for the
*  SCB Component in I2C mode.
*  Please do not use this file or its content in your project.
*
* Note:
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation. All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SCB_PVT_i2c_H)
#define CY_SCB_PVT_i2c_H

#include "i2c.h"


/***************************************
*     Private Function Prototypes
***************************************/

/* APIs to service INTR_I2C_EC register */
#define i2c_SetI2CExtClkInterruptMode(interruptMask) i2c_WRITE_INTR_I2C_EC_MASK(interruptMask)
#define i2c_ClearI2CExtClkInterruptSource(interruptMask) i2c_CLEAR_INTR_I2C_EC(interruptMask)
#define i2c_GetI2CExtClkInterruptSource()                (i2c_INTR_I2C_EC_REG)
#define i2c_GetI2CExtClkInterruptMode()                  (i2c_INTR_I2C_EC_MASK_REG)
#define i2c_GetI2CExtClkInterruptSourceMasked()          (i2c_INTR_I2C_EC_MASKED_REG)

/* APIs to service INTR_SPI_EC register */
#define i2c_SetSpiExtClkInterruptMode(interruptMask) i2c_WRITE_INTR_SPI_EC_MASK(interruptMask)
#define i2c_ClearSpiExtClkInterruptSource(interruptMask) i2c_CLEAR_INTR_SPI_EC(interruptMask)
#define i2c_GetExtSpiClkInterruptSource()                 (i2c_INTR_SPI_EC_REG)
#define i2c_GetExtSpiClkInterruptMode()                   (i2c_INTR_SPI_EC_MASK_REG)
#define i2c_GetExtSpiClkInterruptSourceMasked()           (i2c_INTR_SPI_EC_MASKED_REG)

#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    extern void i2c_SetPins(uint32 mode, uint32 subMode, uint32 uartTxRx);
#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */

void i2c_DisableTxPinsInputBuffer(void);
void i2c_EnableTxPinsInputBuffer(void);


/**********************************
*     Vars with External Linkage
**********************************/

extern cyisraddress i2c_customIntrHandler;
extern i2c_BACKUP_STRUCT i2c_backup;

#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    /* Common config vars */
    extern uint8 i2c_scbMode;
    extern uint8 i2c_scbEnableWake;
    extern uint8 i2c_scbEnableIntr;

    /* I2C config vars */
    extern uint8 i2c_mode;
    extern uint8 i2c_acceptAddr;

    /* SPI/UART config vars */
    extern volatile uint8 * i2c_rxBuffer;
    extern uint8   i2c_rxDataBits;
    extern uint32  i2c_rxBufferSize;

    extern volatile uint8 * i2c_txBuffer;
    extern uint8   i2c_txDataBits;
    extern uint32  i2c_txBufferSize;

    /* EZI2C config vars */
    extern uint8 i2c_numberOfAddr;
    extern uint8 i2c_subAddrSize;
#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */

#endif /* (CY_SCB_PVT_i2c_H) */


/* [] END OF FILE */
