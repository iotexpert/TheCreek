/*******************************************************************************
* File Name: ezi2c_BOOT.h
* Version 3.10
*
* Description:
*  This file provides constants and parameter values of the bootloader
*  communication APIs for the SCB Component.
*
* Note:
*
********************************************************************************
* Copyright 2014-2015, Cypress Semiconductor Corporation. All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SCB_BOOT_ezi2c_H)
#define CY_SCB_BOOT_ezi2c_H

#include "ezi2c_PVT.h"

#if (ezi2c_SCB_MODE_I2C_INC)
    #include "ezi2c_I2C.h"
#endif /* (ezi2c_SCB_MODE_I2C_INC) */

#if (ezi2c_SCB_MODE_EZI2C_INC)
    #include "ezi2c_EZI2C.h"
#endif /* (ezi2c_SCB_MODE_EZI2C_INC) */

#if (ezi2c_SCB_MODE_SPI_INC || ezi2c_SCB_MODE_UART_INC)
    #include "ezi2c_SPI_UART.h"
#endif /* (ezi2c_SCB_MODE_SPI_INC || ezi2c_SCB_MODE_UART_INC) */


/***************************************
*  Conditional Compilation Parameters
****************************************/

/* Bootloader communication interface enable */
#define ezi2c_BTLDR_COMM_ENABLED ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_ezi2c) || \
                                             (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))

/* Enable I2C bootloader communication */
#if (ezi2c_SCB_MODE_I2C_INC)
    #define ezi2c_I2C_BTLDR_COMM_ENABLED     (ezi2c_BTLDR_COMM_ENABLED && \
                                                            (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG || \
                                                             ezi2c_I2C_SLAVE_CONST))
#else
     #define ezi2c_I2C_BTLDR_COMM_ENABLED    (0u)
#endif /* (ezi2c_SCB_MODE_I2C_INC) */

/* EZI2C does not support bootloader communication. Provide empty APIs */
#if (ezi2c_SCB_MODE_EZI2C_INC)
    #define ezi2c_EZI2C_BTLDR_COMM_ENABLED   (ezi2c_BTLDR_COMM_ENABLED && \
                                                         ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
#else
    #define ezi2c_EZI2C_BTLDR_COMM_ENABLED   (0u)
#endif /* (ezi2c_EZI2C_BTLDR_COMM_ENABLED) */

/* Enable SPI bootloader communication */
#if (ezi2c_SCB_MODE_SPI_INC)
    #define ezi2c_SPI_BTLDR_COMM_ENABLED     (ezi2c_BTLDR_COMM_ENABLED && \
                                                            (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG || \
                                                             ezi2c_SPI_SLAVE_CONST))
#else
        #define ezi2c_SPI_BTLDR_COMM_ENABLED (0u)
#endif /* (ezi2c_SPI_BTLDR_COMM_ENABLED) */

/* Enable UART bootloader communication */
#if (ezi2c_SCB_MODE_UART_INC)
       #define ezi2c_UART_BTLDR_COMM_ENABLED    (ezi2c_BTLDR_COMM_ENABLED && \
                                                            (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG || \
                                                             (ezi2c_UART_RX_DIRECTION && \
                                                              ezi2c_UART_TX_DIRECTION)))
#else
     #define ezi2c_UART_BTLDR_COMM_ENABLED   (0u)
#endif /* (ezi2c_UART_BTLDR_COMM_ENABLED) */

/* Enable bootloader communication */
#define ezi2c_BTLDR_COMM_MODE_ENABLED    (ezi2c_I2C_BTLDR_COMM_ENABLED   || \
                                                     ezi2c_SPI_BTLDR_COMM_ENABLED   || \
                                                     ezi2c_EZI2C_BTLDR_COMM_ENABLED || \
                                                     ezi2c_UART_BTLDR_COMM_ENABLED)


/***************************************
*        Function Prototypes
***************************************/

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_I2C_BTLDR_COMM_ENABLED)
    /* I2C Bootloader physical layer functions */
    void ezi2c_I2CCyBtldrCommStart(void);
    void ezi2c_I2CCyBtldrCommStop (void);
    void ezi2c_I2CCyBtldrCommReset(void);
    cystatus ezi2c_I2CCyBtldrCommRead       (uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);
    cystatus ezi2c_I2CCyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);

    /* Map I2C specific bootloader communication APIs to SCB specific APIs */
    #if (ezi2c_SCB_MODE_I2C_CONST_CFG)
        #define ezi2c_CyBtldrCommStart   ezi2c_I2CCyBtldrCommStart
        #define ezi2c_CyBtldrCommStop    ezi2c_I2CCyBtldrCommStop
        #define ezi2c_CyBtldrCommReset   ezi2c_I2CCyBtldrCommReset
        #define ezi2c_CyBtldrCommRead    ezi2c_I2CCyBtldrCommRead
        #define ezi2c_CyBtldrCommWrite   ezi2c_I2CCyBtldrCommWrite
    #endif /* (ezi2c_SCB_MODE_I2C_CONST_CFG) */

#endif /* defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_I2C_BTLDR_COMM_ENABLED) */


#if defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_EZI2C_BTLDR_COMM_ENABLED)
    /* Bootloader physical layer functions */
    void ezi2c_EzI2CCyBtldrCommStart(void);
    void ezi2c_EzI2CCyBtldrCommStop (void);
    void ezi2c_EzI2CCyBtldrCommReset(void);
    cystatus ezi2c_EzI2CCyBtldrCommRead       (uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);
    cystatus ezi2c_EzI2CCyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);

    /* Map EZI2C specific bootloader communication APIs to SCB specific APIs */
    #if (ezi2c_SCB_MODE_EZI2C_CONST_CFG)
        #define ezi2c_CyBtldrCommStart   ezi2c_EzI2CCyBtldrCommStart
        #define ezi2c_CyBtldrCommStop    ezi2c_EzI2CCyBtldrCommStop
        #define ezi2c_CyBtldrCommReset   ezi2c_EzI2CCyBtldrCommReset
        #define ezi2c_CyBtldrCommRead    ezi2c_EzI2CCyBtldrCommRead
        #define ezi2c_CyBtldrCommWrite   ezi2c_EzI2CCyBtldrCommWrite
    #endif /* (ezi2c_SCB_MODE_EZI2C_CONST_CFG) */

#endif /* defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_EZI2C_BTLDR_COMM_ENABLED) */

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_SPI_BTLDR_COMM_ENABLED)
    /* SPI Bootloader physical layer functions */
    void ezi2c_SpiCyBtldrCommStart(void);
    void ezi2c_SpiCyBtldrCommStop (void);
    void ezi2c_SpiCyBtldrCommReset(void);
    cystatus ezi2c_SpiCyBtldrCommRead       (uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);
    cystatus ezi2c_SpiCyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);

    /* Map SPI specific bootloader communication APIs to SCB specific APIs */
    #if (ezi2c_SCB_MODE_SPI_CONST_CFG)
        #define ezi2c_CyBtldrCommStart   ezi2c_SpiCyBtldrCommStart
        #define ezi2c_CyBtldrCommStop    ezi2c_SpiCyBtldrCommStop
        #define ezi2c_CyBtldrCommReset   ezi2c_SpiCyBtldrCommReset
        #define ezi2c_CyBtldrCommRead    ezi2c_SpiCyBtldrCommRead
        #define ezi2c_CyBtldrCommWrite   ezi2c_SpiCyBtldrCommWrite
    #endif /* (ezi2c_SCB_MODE_SPI_CONST_CFG) */

#endif /* defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_SPI_BTLDR_COMM_ENABLED) */

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_UART_BTLDR_COMM_ENABLED)
    /* UART Bootloader physical layer functions */
    void ezi2c_UartCyBtldrCommStart(void);
    void ezi2c_UartCyBtldrCommStop (void);
    void ezi2c_UartCyBtldrCommReset(void);
    cystatus ezi2c_UartCyBtldrCommRead       (uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);
    cystatus ezi2c_UartCyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);

    /* Map UART specific bootloader communication APIs to SCB specific APIs */
    #if (ezi2c_SCB_MODE_UART_CONST_CFG)
        #define ezi2c_CyBtldrCommStart   ezi2c_UartCyBtldrCommStart
        #define ezi2c_CyBtldrCommStop    ezi2c_UartCyBtldrCommStop
        #define ezi2c_CyBtldrCommReset   ezi2c_UartCyBtldrCommReset
        #define ezi2c_CyBtldrCommRead    ezi2c_UartCyBtldrCommRead
        #define ezi2c_CyBtldrCommWrite   ezi2c_UartCyBtldrCommWrite
    #endif /* (ezi2c_SCB_MODE_UART_CONST_CFG) */

#endif /* defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_UART_BTLDR_COMM_ENABLED) */

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_BTLDR_COMM_ENABLED)
    #if (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
        /* Bootloader physical layer functions */
        void ezi2c_CyBtldrCommStart(void);
        void ezi2c_CyBtldrCommStop (void);
        void ezi2c_CyBtldrCommReset(void);
        cystatus ezi2c_CyBtldrCommRead       (uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);
        cystatus ezi2c_CyBtldrCommWrite(const uint8 pData[], uint16 size, uint16 * count, uint8 timeOut);
    #endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */

    /* Map SCB specific bootloader communication APIs to common APIs */
    #if (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_ezi2c)
        #define CyBtldrCommStart    ezi2c_CyBtldrCommStart
        #define CyBtldrCommStop     ezi2c_CyBtldrCommStop
        #define CyBtldrCommReset    ezi2c_CyBtldrCommReset
        #define CyBtldrCommWrite    ezi2c_CyBtldrCommWrite
        #define CyBtldrCommRead     ezi2c_CyBtldrCommRead
    #endif /* (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_ezi2c) */

#endif /* defined(CYDEV_BOOTLOADER_IO_COMP) && (ezi2c_BTLDR_COMM_ENABLED) */


/***************************************
*           API Constants
***************************************/

/* Timeout unit in milliseconds */
#define ezi2c_WAIT_1_MS  (1u)

/* Return number of bytes to copy into bootloader buffer */
#define ezi2c_BYTES_TO_COPY(actBufSize, bufSize) \
                            ( ((uint32)(actBufSize) < (uint32)(bufSize)) ? \
                                ((uint32) (actBufSize)) : ((uint32) (bufSize)) )

/* Size of Read/Write buffers for I2C bootloader  */
#define ezi2c_I2C_BTLDR_SIZEOF_READ_BUFFER   (64u)
#define ezi2c_I2C_BTLDR_SIZEOF_WRITE_BUFFER  (64u)

/* Byte to byte time interval: calculated basing on current component
* data rate configuration, can be defined in project if required.
*/
#ifndef ezi2c_SPI_BYTE_TO_BYTE
    #define ezi2c_SPI_BYTE_TO_BYTE   (160u)
#endif

/* Byte to byte time interval: calculated basing on current component
* baud rate configuration, can be defined in the project if required.
*/
#ifndef ezi2c_UART_BYTE_TO_BYTE
    #define ezi2c_UART_BYTE_TO_BYTE  (2500u)
#endif /* ezi2c_UART_BYTE_TO_BYTE */

#endif /* (CY_SCB_BOOT_ezi2c_H) */


/* [] END OF FILE */
