/***************************************************************************//**
* \file ezi2c_PINS.h
* \version 4.0
*
* \brief
*  This file provides constants and parameter values for the pin components
*  buried into SCB Component.
*
* Note:
*
********************************************************************************
* \copyright
* Copyright 2013-2017, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SCB_PINS_ezi2c_H)
#define CY_SCB_PINS_ezi2c_H

#include "cydevice_trm.h"
#include "cyfitter.h"
#include "cytypes.h"


/***************************************
*   Conditional Compilation Parameters
****************************************/

/* Unconfigured pins */
#define ezi2c_REMOVE_RX_WAKE_SCL_MOSI_PIN  (1u)
#define ezi2c_REMOVE_RX_SCL_MOSI_PIN      (1u)
#define ezi2c_REMOVE_TX_SDA_MISO_PIN      (1u)
#define ezi2c_REMOVE_SCLK_PIN      (1u)
#define ezi2c_REMOVE_SS0_PIN      (1u)
#define ezi2c_REMOVE_SS1_PIN                 (1u)
#define ezi2c_REMOVE_SS2_PIN                 (1u)
#define ezi2c_REMOVE_SS3_PIN                 (1u)

/* Mode defined pins */
#define ezi2c_REMOVE_I2C_PINS                (0u)
#define ezi2c_REMOVE_SPI_MASTER_PINS         (1u)
#define ezi2c_REMOVE_SPI_MASTER_SCLK_PIN     (1u)
#define ezi2c_REMOVE_SPI_MASTER_MOSI_PIN     (1u)
#define ezi2c_REMOVE_SPI_MASTER_MISO_PIN     (1u)
#define ezi2c_REMOVE_SPI_MASTER_SS0_PIN      (1u)
#define ezi2c_REMOVE_SPI_MASTER_SS1_PIN      (1u)
#define ezi2c_REMOVE_SPI_MASTER_SS2_PIN      (1u)
#define ezi2c_REMOVE_SPI_MASTER_SS3_PIN      (1u)
#define ezi2c_REMOVE_SPI_SLAVE_PINS          (1u)
#define ezi2c_REMOVE_SPI_SLAVE_MOSI_PIN      (1u)
#define ezi2c_REMOVE_SPI_SLAVE_MISO_PIN      (1u)
#define ezi2c_REMOVE_UART_TX_PIN             (1u)
#define ezi2c_REMOVE_UART_RX_TX_PIN          (1u)
#define ezi2c_REMOVE_UART_RX_PIN             (1u)
#define ezi2c_REMOVE_UART_RX_WAKE_PIN        (1u)
#define ezi2c_REMOVE_UART_RTS_PIN            (1u)
#define ezi2c_REMOVE_UART_CTS_PIN            (1u)

/* Unconfigured pins */
#define ezi2c_RX_WAKE_SCL_MOSI_PIN (0u == ezi2c_REMOVE_RX_WAKE_SCL_MOSI_PIN)
#define ezi2c_RX_SCL_MOSI_PIN     (0u == ezi2c_REMOVE_RX_SCL_MOSI_PIN)
#define ezi2c_TX_SDA_MISO_PIN     (0u == ezi2c_REMOVE_TX_SDA_MISO_PIN)
#define ezi2c_SCLK_PIN     (0u == ezi2c_REMOVE_SCLK_PIN)
#define ezi2c_SS0_PIN     (0u == ezi2c_REMOVE_SS0_PIN)
#define ezi2c_SS1_PIN                (0u == ezi2c_REMOVE_SS1_PIN)
#define ezi2c_SS2_PIN                (0u == ezi2c_REMOVE_SS2_PIN)
#define ezi2c_SS3_PIN                (0u == ezi2c_REMOVE_SS3_PIN)

/* Mode defined pins */
#define ezi2c_I2C_PINS               (0u == ezi2c_REMOVE_I2C_PINS)
#define ezi2c_SPI_MASTER_PINS        (0u == ezi2c_REMOVE_SPI_MASTER_PINS)
#define ezi2c_SPI_MASTER_SCLK_PIN    (0u == ezi2c_REMOVE_SPI_MASTER_SCLK_PIN)
#define ezi2c_SPI_MASTER_MOSI_PIN    (0u == ezi2c_REMOVE_SPI_MASTER_MOSI_PIN)
#define ezi2c_SPI_MASTER_MISO_PIN    (0u == ezi2c_REMOVE_SPI_MASTER_MISO_PIN)
#define ezi2c_SPI_MASTER_SS0_PIN     (0u == ezi2c_REMOVE_SPI_MASTER_SS0_PIN)
#define ezi2c_SPI_MASTER_SS1_PIN     (0u == ezi2c_REMOVE_SPI_MASTER_SS1_PIN)
#define ezi2c_SPI_MASTER_SS2_PIN     (0u == ezi2c_REMOVE_SPI_MASTER_SS2_PIN)
#define ezi2c_SPI_MASTER_SS3_PIN     (0u == ezi2c_REMOVE_SPI_MASTER_SS3_PIN)
#define ezi2c_SPI_SLAVE_PINS         (0u == ezi2c_REMOVE_SPI_SLAVE_PINS)
#define ezi2c_SPI_SLAVE_MOSI_PIN     (0u == ezi2c_REMOVE_SPI_SLAVE_MOSI_PIN)
#define ezi2c_SPI_SLAVE_MISO_PIN     (0u == ezi2c_REMOVE_SPI_SLAVE_MISO_PIN)
#define ezi2c_UART_TX_PIN            (0u == ezi2c_REMOVE_UART_TX_PIN)
#define ezi2c_UART_RX_TX_PIN         (0u == ezi2c_REMOVE_UART_RX_TX_PIN)
#define ezi2c_UART_RX_PIN            (0u == ezi2c_REMOVE_UART_RX_PIN)
#define ezi2c_UART_RX_WAKE_PIN       (0u == ezi2c_REMOVE_UART_RX_WAKE_PIN)
#define ezi2c_UART_RTS_PIN           (0u == ezi2c_REMOVE_UART_RTS_PIN)
#define ezi2c_UART_CTS_PIN           (0u == ezi2c_REMOVE_UART_CTS_PIN)


/***************************************
*             Includes
****************************************/

#if (ezi2c_RX_WAKE_SCL_MOSI_PIN)
    #include "ezi2c_uart_rx_wake_i2c_scl_spi_mosi.h"
#endif /* (ezi2c_RX_SCL_MOSI) */

#if (ezi2c_RX_SCL_MOSI_PIN)
    #include "ezi2c_uart_rx_i2c_scl_spi_mosi.h"
#endif /* (ezi2c_RX_SCL_MOSI) */

#if (ezi2c_TX_SDA_MISO_PIN)
    #include "ezi2c_uart_tx_i2c_sda_spi_miso.h"
#endif /* (ezi2c_TX_SDA_MISO) */

#if (ezi2c_SCLK_PIN)
    #include "ezi2c_spi_sclk.h"
#endif /* (ezi2c_SCLK) */

#if (ezi2c_SS0_PIN)
    #include "ezi2c_spi_ss0.h"
#endif /* (ezi2c_SS0_PIN) */

#if (ezi2c_SS1_PIN)
    #include "ezi2c_spi_ss1.h"
#endif /* (ezi2c_SS1_PIN) */

#if (ezi2c_SS2_PIN)
    #include "ezi2c_spi_ss2.h"
#endif /* (ezi2c_SS2_PIN) */

#if (ezi2c_SS3_PIN)
    #include "ezi2c_spi_ss3.h"
#endif /* (ezi2c_SS3_PIN) */

#if (ezi2c_I2C_PINS)
    #include "ezi2c_scl.h"
    #include "ezi2c_sda.h"
#endif /* (ezi2c_I2C_PINS) */

#if (ezi2c_SPI_MASTER_PINS)
#if (ezi2c_SPI_MASTER_SCLK_PIN)
    #include "ezi2c_sclk_m.h"
#endif /* (ezi2c_SPI_MASTER_SCLK_PIN) */

#if (ezi2c_SPI_MASTER_MOSI_PIN)
    #include "ezi2c_mosi_m.h"
#endif /* (ezi2c_SPI_MASTER_MOSI_PIN) */

#if (ezi2c_SPI_MASTER_MISO_PIN)
    #include "ezi2c_miso_m.h"
#endif /*(ezi2c_SPI_MASTER_MISO_PIN) */
#endif /* (ezi2c_SPI_MASTER_PINS) */

#if (ezi2c_SPI_SLAVE_PINS)
    #include "ezi2c_sclk_s.h"
    #include "ezi2c_ss_s.h"

#if (ezi2c_SPI_SLAVE_MOSI_PIN)
    #include "ezi2c_mosi_s.h"
#endif /* (ezi2c_SPI_SLAVE_MOSI_PIN) */

#if (ezi2c_SPI_SLAVE_MISO_PIN)
    #include "ezi2c_miso_s.h"
#endif /*(ezi2c_SPI_SLAVE_MISO_PIN) */
#endif /* (ezi2c_SPI_SLAVE_PINS) */

#if (ezi2c_SPI_MASTER_SS0_PIN)
    #include "ezi2c_ss0_m.h"
#endif /* (ezi2c_SPI_MASTER_SS0_PIN) */

#if (ezi2c_SPI_MASTER_SS1_PIN)
    #include "ezi2c_ss1_m.h"
#endif /* (ezi2c_SPI_MASTER_SS1_PIN) */

#if (ezi2c_SPI_MASTER_SS2_PIN)
    #include "ezi2c_ss2_m.h"
#endif /* (ezi2c_SPI_MASTER_SS2_PIN) */

#if (ezi2c_SPI_MASTER_SS3_PIN)
    #include "ezi2c_ss3_m.h"
#endif /* (ezi2c_SPI_MASTER_SS3_PIN) */

#if (ezi2c_UART_TX_PIN)
    #include "ezi2c_tx.h"
#endif /* (ezi2c_UART_TX_PIN) */

#if (ezi2c_UART_RX_TX_PIN)
    #include "ezi2c_rx_tx.h"
#endif /* (ezi2c_UART_RX_TX_PIN) */

#if (ezi2c_UART_RX_PIN)
    #include "ezi2c_rx.h"
#endif /* (ezi2c_UART_RX_PIN) */

#if (ezi2c_UART_RX_WAKE_PIN)
    #include "ezi2c_rx_wake.h"
#endif /* (ezi2c_UART_RX_WAKE_PIN) */

#if (ezi2c_UART_RTS_PIN)
    #include "ezi2c_rts.h"
#endif /* (ezi2c_UART_RTS_PIN) */

#if (ezi2c_UART_CTS_PIN)
    #include "ezi2c_cts.h"
#endif /* (ezi2c_UART_CTS_PIN) */


/***************************************
*              Registers
***************************************/

#if (ezi2c_RX_SCL_MOSI_PIN)
    #define ezi2c_RX_SCL_MOSI_HSIOM_REG   (*(reg32 *) ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM)
    #define ezi2c_RX_SCL_MOSI_HSIOM_PTR   ( (reg32 *) ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM)
    
    #define ezi2c_RX_SCL_MOSI_HSIOM_MASK      (ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM_MASK)
    #define ezi2c_RX_SCL_MOSI_HSIOM_POS       (ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM_SHIFT)
    #define ezi2c_RX_SCL_MOSI_HSIOM_SEL_GPIO  (ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM_GPIO)
    #define ezi2c_RX_SCL_MOSI_HSIOM_SEL_I2C   (ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM_I2C)
    #define ezi2c_RX_SCL_MOSI_HSIOM_SEL_SPI   (ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM_SPI)
    #define ezi2c_RX_SCL_MOSI_HSIOM_SEL_UART  (ezi2c_uart_rx_i2c_scl_spi_mosi__0__HSIOM_UART)
    
#elif (ezi2c_RX_WAKE_SCL_MOSI_PIN)
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG   (*(reg32 *) ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM)
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_PTR   ( (reg32 *) ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM)
    
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_MASK      (ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM_MASK)
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_POS       (ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM_SHIFT)
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_SEL_GPIO  (ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM_GPIO)
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_SEL_I2C   (ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM_I2C)
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_SEL_SPI   (ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM_SPI)
    #define ezi2c_RX_WAKE_SCL_MOSI_HSIOM_SEL_UART  (ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__HSIOM_UART)    
   
    #define ezi2c_RX_WAKE_SCL_MOSI_INTCFG_REG (*(reg32 *) ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__INTCFG)
    #define ezi2c_RX_WAKE_SCL_MOSI_INTCFG_PTR ( (reg32 *) ezi2c_uart_rx_wake_i2c_scl_spi_mosi__0__INTCFG)
    #define ezi2c_RX_WAKE_SCL_MOSI_INTCFG_TYPE_POS  (ezi2c_uart_rx_wake_i2c_scl_spi_mosi__SHIFT)
    #define ezi2c_RX_WAKE_SCL_MOSI_INTCFG_TYPE_MASK ((uint32) ezi2c_INTCFG_TYPE_MASK << \
                                                                           ezi2c_RX_WAKE_SCL_MOSI_INTCFG_TYPE_POS)
#else
    /* None of pins ezi2c_RX_SCL_MOSI_PIN or ezi2c_RX_WAKE_SCL_MOSI_PIN present.*/
#endif /* (ezi2c_RX_SCL_MOSI_PIN) */

#if (ezi2c_TX_SDA_MISO_PIN)
    #define ezi2c_TX_SDA_MISO_HSIOM_REG   (*(reg32 *) ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM)
    #define ezi2c_TX_SDA_MISO_HSIOM_PTR   ( (reg32 *) ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM)
    
    #define ezi2c_TX_SDA_MISO_HSIOM_MASK      (ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM_MASK)
    #define ezi2c_TX_SDA_MISO_HSIOM_POS       (ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM_SHIFT)
    #define ezi2c_TX_SDA_MISO_HSIOM_SEL_GPIO  (ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM_GPIO)
    #define ezi2c_TX_SDA_MISO_HSIOM_SEL_I2C   (ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM_I2C)
    #define ezi2c_TX_SDA_MISO_HSIOM_SEL_SPI   (ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM_SPI)
    #define ezi2c_TX_SDA_MISO_HSIOM_SEL_UART  (ezi2c_uart_tx_i2c_sda_spi_miso__0__HSIOM_UART)
#endif /* (ezi2c_TX_SDA_MISO_PIN) */

#if (ezi2c_SCLK_PIN)
    #define ezi2c_SCLK_HSIOM_REG   (*(reg32 *) ezi2c_spi_sclk__0__HSIOM)
    #define ezi2c_SCLK_HSIOM_PTR   ( (reg32 *) ezi2c_spi_sclk__0__HSIOM)
    
    #define ezi2c_SCLK_HSIOM_MASK      (ezi2c_spi_sclk__0__HSIOM_MASK)
    #define ezi2c_SCLK_HSIOM_POS       (ezi2c_spi_sclk__0__HSIOM_SHIFT)
    #define ezi2c_SCLK_HSIOM_SEL_GPIO  (ezi2c_spi_sclk__0__HSIOM_GPIO)
    #define ezi2c_SCLK_HSIOM_SEL_I2C   (ezi2c_spi_sclk__0__HSIOM_I2C)
    #define ezi2c_SCLK_HSIOM_SEL_SPI   (ezi2c_spi_sclk__0__HSIOM_SPI)
    #define ezi2c_SCLK_HSIOM_SEL_UART  (ezi2c_spi_sclk__0__HSIOM_UART)
#endif /* (ezi2c_SCLK_PIN) */

#if (ezi2c_SS0_PIN)
    #define ezi2c_SS0_HSIOM_REG   (*(reg32 *) ezi2c_spi_ss0__0__HSIOM)
    #define ezi2c_SS0_HSIOM_PTR   ( (reg32 *) ezi2c_spi_ss0__0__HSIOM)
    
    #define ezi2c_SS0_HSIOM_MASK      (ezi2c_spi_ss0__0__HSIOM_MASK)
    #define ezi2c_SS0_HSIOM_POS       (ezi2c_spi_ss0__0__HSIOM_SHIFT)
    #define ezi2c_SS0_HSIOM_SEL_GPIO  (ezi2c_spi_ss0__0__HSIOM_GPIO)
    #define ezi2c_SS0_HSIOM_SEL_I2C   (ezi2c_spi_ss0__0__HSIOM_I2C)
    #define ezi2c_SS0_HSIOM_SEL_SPI   (ezi2c_spi_ss0__0__HSIOM_SPI)
#if !(ezi2c_CY_SCBIP_V0 || ezi2c_CY_SCBIP_V1)
    #define ezi2c_SS0_HSIOM_SEL_UART  (ezi2c_spi_ss0__0__HSIOM_UART)
#endif /* !(ezi2c_CY_SCBIP_V0 || ezi2c_CY_SCBIP_V1) */
#endif /* (ezi2c_SS0_PIN) */

#if (ezi2c_SS1_PIN)
    #define ezi2c_SS1_HSIOM_REG  (*(reg32 *) ezi2c_spi_ss1__0__HSIOM)
    #define ezi2c_SS1_HSIOM_PTR  ( (reg32 *) ezi2c_spi_ss1__0__HSIOM)
    
    #define ezi2c_SS1_HSIOM_MASK     (ezi2c_spi_ss1__0__HSIOM_MASK)
    #define ezi2c_SS1_HSIOM_POS      (ezi2c_spi_ss1__0__HSIOM_SHIFT)
    #define ezi2c_SS1_HSIOM_SEL_GPIO (ezi2c_spi_ss1__0__HSIOM_GPIO)
    #define ezi2c_SS1_HSIOM_SEL_I2C  (ezi2c_spi_ss1__0__HSIOM_I2C)
    #define ezi2c_SS1_HSIOM_SEL_SPI  (ezi2c_spi_ss1__0__HSIOM_SPI)
#endif /* (ezi2c_SS1_PIN) */

#if (ezi2c_SS2_PIN)
    #define ezi2c_SS2_HSIOM_REG     (*(reg32 *) ezi2c_spi_ss2__0__HSIOM)
    #define ezi2c_SS2_HSIOM_PTR     ( (reg32 *) ezi2c_spi_ss2__0__HSIOM)
    
    #define ezi2c_SS2_HSIOM_MASK     (ezi2c_spi_ss2__0__HSIOM_MASK)
    #define ezi2c_SS2_HSIOM_POS      (ezi2c_spi_ss2__0__HSIOM_SHIFT)
    #define ezi2c_SS2_HSIOM_SEL_GPIO (ezi2c_spi_ss2__0__HSIOM_GPIO)
    #define ezi2c_SS2_HSIOM_SEL_I2C  (ezi2c_spi_ss2__0__HSIOM_I2C)
    #define ezi2c_SS2_HSIOM_SEL_SPI  (ezi2c_spi_ss2__0__HSIOM_SPI)
#endif /* (ezi2c_SS2_PIN) */

#if (ezi2c_SS3_PIN)
    #define ezi2c_SS3_HSIOM_REG     (*(reg32 *) ezi2c_spi_ss3__0__HSIOM)
    #define ezi2c_SS3_HSIOM_PTR     ( (reg32 *) ezi2c_spi_ss3__0__HSIOM)
    
    #define ezi2c_SS3_HSIOM_MASK     (ezi2c_spi_ss3__0__HSIOM_MASK)
    #define ezi2c_SS3_HSIOM_POS      (ezi2c_spi_ss3__0__HSIOM_SHIFT)
    #define ezi2c_SS3_HSIOM_SEL_GPIO (ezi2c_spi_ss3__0__HSIOM_GPIO)
    #define ezi2c_SS3_HSIOM_SEL_I2C  (ezi2c_spi_ss3__0__HSIOM_I2C)
    #define ezi2c_SS3_HSIOM_SEL_SPI  (ezi2c_spi_ss3__0__HSIOM_SPI)
#endif /* (ezi2c_SS3_PIN) */

#if (ezi2c_I2C_PINS)
    #define ezi2c_SCL_HSIOM_REG  (*(reg32 *) ezi2c_scl__0__HSIOM)
    #define ezi2c_SCL_HSIOM_PTR  ( (reg32 *) ezi2c_scl__0__HSIOM)
    
    #define ezi2c_SCL_HSIOM_MASK     (ezi2c_scl__0__HSIOM_MASK)
    #define ezi2c_SCL_HSIOM_POS      (ezi2c_scl__0__HSIOM_SHIFT)
    #define ezi2c_SCL_HSIOM_SEL_GPIO (ezi2c_sda__0__HSIOM_GPIO)
    #define ezi2c_SCL_HSIOM_SEL_I2C  (ezi2c_sda__0__HSIOM_I2C)
    
    #define ezi2c_SDA_HSIOM_REG  (*(reg32 *) ezi2c_sda__0__HSIOM)
    #define ezi2c_SDA_HSIOM_PTR  ( (reg32 *) ezi2c_sda__0__HSIOM)
    
    #define ezi2c_SDA_HSIOM_MASK     (ezi2c_sda__0__HSIOM_MASK)
    #define ezi2c_SDA_HSIOM_POS      (ezi2c_sda__0__HSIOM_SHIFT)
    #define ezi2c_SDA_HSIOM_SEL_GPIO (ezi2c_sda__0__HSIOM_GPIO)
    #define ezi2c_SDA_HSIOM_SEL_I2C  (ezi2c_sda__0__HSIOM_I2C)
#endif /* (ezi2c_I2C_PINS) */

#if (ezi2c_SPI_SLAVE_PINS)
    #define ezi2c_SCLK_S_HSIOM_REG   (*(reg32 *) ezi2c_sclk_s__0__HSIOM)
    #define ezi2c_SCLK_S_HSIOM_PTR   ( (reg32 *) ezi2c_sclk_s__0__HSIOM)
    
    #define ezi2c_SCLK_S_HSIOM_MASK      (ezi2c_sclk_s__0__HSIOM_MASK)
    #define ezi2c_SCLK_S_HSIOM_POS       (ezi2c_sclk_s__0__HSIOM_SHIFT)
    #define ezi2c_SCLK_S_HSIOM_SEL_GPIO  (ezi2c_sclk_s__0__HSIOM_GPIO)
    #define ezi2c_SCLK_S_HSIOM_SEL_SPI   (ezi2c_sclk_s__0__HSIOM_SPI)
    
    #define ezi2c_SS0_S_HSIOM_REG    (*(reg32 *) ezi2c_ss0_s__0__HSIOM)
    #define ezi2c_SS0_S_HSIOM_PTR    ( (reg32 *) ezi2c_ss0_s__0__HSIOM)
    
    #define ezi2c_SS0_S_HSIOM_MASK       (ezi2c_ss0_s__0__HSIOM_MASK)
    #define ezi2c_SS0_S_HSIOM_POS        (ezi2c_ss0_s__0__HSIOM_SHIFT)
    #define ezi2c_SS0_S_HSIOM_SEL_GPIO   (ezi2c_ss0_s__0__HSIOM_GPIO)  
    #define ezi2c_SS0_S_HSIOM_SEL_SPI    (ezi2c_ss0_s__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_SLAVE_PINS) */

#if (ezi2c_SPI_SLAVE_MOSI_PIN)
    #define ezi2c_MOSI_S_HSIOM_REG   (*(reg32 *) ezi2c_mosi_s__0__HSIOM)
    #define ezi2c_MOSI_S_HSIOM_PTR   ( (reg32 *) ezi2c_mosi_s__0__HSIOM)
    
    #define ezi2c_MOSI_S_HSIOM_MASK      (ezi2c_mosi_s__0__HSIOM_MASK)
    #define ezi2c_MOSI_S_HSIOM_POS       (ezi2c_mosi_s__0__HSIOM_SHIFT)
    #define ezi2c_MOSI_S_HSIOM_SEL_GPIO  (ezi2c_mosi_s__0__HSIOM_GPIO)
    #define ezi2c_MOSI_S_HSIOM_SEL_SPI   (ezi2c_mosi_s__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_SLAVE_MOSI_PIN) */

#if (ezi2c_SPI_SLAVE_MISO_PIN)
    #define ezi2c_MISO_S_HSIOM_REG   (*(reg32 *) ezi2c_miso_s__0__HSIOM)
    #define ezi2c_MISO_S_HSIOM_PTR   ( (reg32 *) ezi2c_miso_s__0__HSIOM)
    
    #define ezi2c_MISO_S_HSIOM_MASK      (ezi2c_miso_s__0__HSIOM_MASK)
    #define ezi2c_MISO_S_HSIOM_POS       (ezi2c_miso_s__0__HSIOM_SHIFT)
    #define ezi2c_MISO_S_HSIOM_SEL_GPIO  (ezi2c_miso_s__0__HSIOM_GPIO)
    #define ezi2c_MISO_S_HSIOM_SEL_SPI   (ezi2c_miso_s__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_SLAVE_MISO_PIN) */

#if (ezi2c_SPI_MASTER_MISO_PIN)
    #define ezi2c_MISO_M_HSIOM_REG   (*(reg32 *) ezi2c_miso_m__0__HSIOM)
    #define ezi2c_MISO_M_HSIOM_PTR   ( (reg32 *) ezi2c_miso_m__0__HSIOM)
    
    #define ezi2c_MISO_M_HSIOM_MASK      (ezi2c_miso_m__0__HSIOM_MASK)
    #define ezi2c_MISO_M_HSIOM_POS       (ezi2c_miso_m__0__HSIOM_SHIFT)
    #define ezi2c_MISO_M_HSIOM_SEL_GPIO  (ezi2c_miso_m__0__HSIOM_GPIO)
    #define ezi2c_MISO_M_HSIOM_SEL_SPI   (ezi2c_miso_m__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_MASTER_MISO_PIN) */

#if (ezi2c_SPI_MASTER_MOSI_PIN)
    #define ezi2c_MOSI_M_HSIOM_REG   (*(reg32 *) ezi2c_mosi_m__0__HSIOM)
    #define ezi2c_MOSI_M_HSIOM_PTR   ( (reg32 *) ezi2c_mosi_m__0__HSIOM)
    
    #define ezi2c_MOSI_M_HSIOM_MASK      (ezi2c_mosi_m__0__HSIOM_MASK)
    #define ezi2c_MOSI_M_HSIOM_POS       (ezi2c_mosi_m__0__HSIOM_SHIFT)
    #define ezi2c_MOSI_M_HSIOM_SEL_GPIO  (ezi2c_mosi_m__0__HSIOM_GPIO)
    #define ezi2c_MOSI_M_HSIOM_SEL_SPI   (ezi2c_mosi_m__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_MASTER_MOSI_PIN) */

#if (ezi2c_SPI_MASTER_SCLK_PIN)
    #define ezi2c_SCLK_M_HSIOM_REG   (*(reg32 *) ezi2c_sclk_m__0__HSIOM)
    #define ezi2c_SCLK_M_HSIOM_PTR   ( (reg32 *) ezi2c_sclk_m__0__HSIOM)
    
    #define ezi2c_SCLK_M_HSIOM_MASK      (ezi2c_sclk_m__0__HSIOM_MASK)
    #define ezi2c_SCLK_M_HSIOM_POS       (ezi2c_sclk_m__0__HSIOM_SHIFT)
    #define ezi2c_SCLK_M_HSIOM_SEL_GPIO  (ezi2c_sclk_m__0__HSIOM_GPIO)
    #define ezi2c_SCLK_M_HSIOM_SEL_SPI   (ezi2c_sclk_m__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_MASTER_SCLK_PIN) */

#if (ezi2c_SPI_MASTER_SS0_PIN)
    #define ezi2c_SS0_M_HSIOM_REG    (*(reg32 *) ezi2c_ss0_m__0__HSIOM)
    #define ezi2c_SS0_M_HSIOM_PTR    ( (reg32 *) ezi2c_ss0_m__0__HSIOM)
    
    #define ezi2c_SS0_M_HSIOM_MASK       (ezi2c_ss0_m__0__HSIOM_MASK)
    #define ezi2c_SS0_M_HSIOM_POS        (ezi2c_ss0_m__0__HSIOM_SHIFT)
    #define ezi2c_SS0_M_HSIOM_SEL_GPIO   (ezi2c_ss0_m__0__HSIOM_GPIO)
    #define ezi2c_SS0_M_HSIOM_SEL_SPI    (ezi2c_ss0_m__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_MASTER_SS0_PIN) */

#if (ezi2c_SPI_MASTER_SS1_PIN)
    #define ezi2c_SS1_M_HSIOM_REG    (*(reg32 *) ezi2c_ss1_m__0__HSIOM)
    #define ezi2c_SS1_M_HSIOM_PTR    ( (reg32 *) ezi2c_ss1_m__0__HSIOM)
    
    #define ezi2c_SS1_M_HSIOM_MASK       (ezi2c_ss1_m__0__HSIOM_MASK)
    #define ezi2c_SS1_M_HSIOM_POS        (ezi2c_ss1_m__0__HSIOM_SHIFT)
    #define ezi2c_SS1_M_HSIOM_SEL_GPIO   (ezi2c_ss1_m__0__HSIOM_GPIO)
    #define ezi2c_SS1_M_HSIOM_SEL_SPI    (ezi2c_ss1_m__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_MASTER_SS1_PIN) */

#if (ezi2c_SPI_MASTER_SS2_PIN)
    #define ezi2c_SS2_M_HSIOM_REG    (*(reg32 *) ezi2c_ss2_m__0__HSIOM)
    #define ezi2c_SS2_M_HSIOM_PTR    ( (reg32 *) ezi2c_ss2_m__0__HSIOM)
    
    #define ezi2c_SS2_M_HSIOM_MASK       (ezi2c_ss2_m__0__HSIOM_MASK)
    #define ezi2c_SS2_M_HSIOM_POS        (ezi2c_ss2_m__0__HSIOM_SHIFT)
    #define ezi2c_SS2_M_HSIOM_SEL_GPIO   (ezi2c_ss2_m__0__HSIOM_GPIO)
    #define ezi2c_SS2_M_HSIOM_SEL_SPI    (ezi2c_ss2_m__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_MASTER_SS2_PIN) */

#if (ezi2c_SPI_MASTER_SS3_PIN)
    #define ezi2c_SS3_M_HSIOM_REG    (*(reg32 *) ezi2c_ss3_m__0__HSIOM)
    #define ezi2c_SS3_M_HSIOM_PTR    ( (reg32 *) ezi2c_ss3_m__0__HSIOM)
    
    #define ezi2c_SS3_M_HSIOM_MASK      (ezi2c_ss3_m__0__HSIOM_MASK)
    #define ezi2c_SS3_M_HSIOM_POS       (ezi2c_ss3_m__0__HSIOM_SHIFT)
    #define ezi2c_SS3_M_HSIOM_SEL_GPIO  (ezi2c_ss3_m__0__HSIOM_GPIO)
    #define ezi2c_SS3_M_HSIOM_SEL_SPI   (ezi2c_ss3_m__0__HSIOM_SPI)
#endif /* (ezi2c_SPI_MASTER_SS3_PIN) */

#if (ezi2c_UART_RX_PIN)
    #define ezi2c_RX_HSIOM_REG   (*(reg32 *) ezi2c_rx__0__HSIOM)
    #define ezi2c_RX_HSIOM_PTR   ( (reg32 *) ezi2c_rx__0__HSIOM)
    
    #define ezi2c_RX_HSIOM_MASK      (ezi2c_rx__0__HSIOM_MASK)
    #define ezi2c_RX_HSIOM_POS       (ezi2c_rx__0__HSIOM_SHIFT)
    #define ezi2c_RX_HSIOM_SEL_GPIO  (ezi2c_rx__0__HSIOM_GPIO)
    #define ezi2c_RX_HSIOM_SEL_UART  (ezi2c_rx__0__HSIOM_UART)
#endif /* (ezi2c_UART_RX_PIN) */

#if (ezi2c_UART_RX_WAKE_PIN)
    #define ezi2c_RX_WAKE_HSIOM_REG   (*(reg32 *) ezi2c_rx_wake__0__HSIOM)
    #define ezi2c_RX_WAKE_HSIOM_PTR   ( (reg32 *) ezi2c_rx_wake__0__HSIOM)
    
    #define ezi2c_RX_WAKE_HSIOM_MASK      (ezi2c_rx_wake__0__HSIOM_MASK)
    #define ezi2c_RX_WAKE_HSIOM_POS       (ezi2c_rx_wake__0__HSIOM_SHIFT)
    #define ezi2c_RX_WAKE_HSIOM_SEL_GPIO  (ezi2c_rx_wake__0__HSIOM_GPIO)
    #define ezi2c_RX_WAKE_HSIOM_SEL_UART  (ezi2c_rx_wake__0__HSIOM_UART)
#endif /* (ezi2c_UART_WAKE_RX_PIN) */

#if (ezi2c_UART_CTS_PIN)
    #define ezi2c_CTS_HSIOM_REG   (*(reg32 *) ezi2c_cts__0__HSIOM)
    #define ezi2c_CTS_HSIOM_PTR   ( (reg32 *) ezi2c_cts__0__HSIOM)
    
    #define ezi2c_CTS_HSIOM_MASK      (ezi2c_cts__0__HSIOM_MASK)
    #define ezi2c_CTS_HSIOM_POS       (ezi2c_cts__0__HSIOM_SHIFT)
    #define ezi2c_CTS_HSIOM_SEL_GPIO  (ezi2c_cts__0__HSIOM_GPIO)
    #define ezi2c_CTS_HSIOM_SEL_UART  (ezi2c_cts__0__HSIOM_UART)
#endif /* (ezi2c_UART_CTS_PIN) */

#if (ezi2c_UART_TX_PIN)
    #define ezi2c_TX_HSIOM_REG   (*(reg32 *) ezi2c_tx__0__HSIOM)
    #define ezi2c_TX_HSIOM_PTR   ( (reg32 *) ezi2c_tx__0__HSIOM)
    
    #define ezi2c_TX_HSIOM_MASK      (ezi2c_tx__0__HSIOM_MASK)
    #define ezi2c_TX_HSIOM_POS       (ezi2c_tx__0__HSIOM_SHIFT)
    #define ezi2c_TX_HSIOM_SEL_GPIO  (ezi2c_tx__0__HSIOM_GPIO)
    #define ezi2c_TX_HSIOM_SEL_UART  (ezi2c_tx__0__HSIOM_UART)
#endif /* (ezi2c_UART_TX_PIN) */

#if (ezi2c_UART_RX_TX_PIN)
    #define ezi2c_RX_TX_HSIOM_REG   (*(reg32 *) ezi2c_rx_tx__0__HSIOM)
    #define ezi2c_RX_TX_HSIOM_PTR   ( (reg32 *) ezi2c_rx_tx__0__HSIOM)
    
    #define ezi2c_RX_TX_HSIOM_MASK      (ezi2c_rx_tx__0__HSIOM_MASK)
    #define ezi2c_RX_TX_HSIOM_POS       (ezi2c_rx_tx__0__HSIOM_SHIFT)
    #define ezi2c_RX_TX_HSIOM_SEL_GPIO  (ezi2c_rx_tx__0__HSIOM_GPIO)
    #define ezi2c_RX_TX_HSIOM_SEL_UART  (ezi2c_rx_tx__0__HSIOM_UART)
#endif /* (ezi2c_UART_RX_TX_PIN) */

#if (ezi2c_UART_RTS_PIN)
    #define ezi2c_RTS_HSIOM_REG      (*(reg32 *) ezi2c_rts__0__HSIOM)
    #define ezi2c_RTS_HSIOM_PTR      ( (reg32 *) ezi2c_rts__0__HSIOM)
    
    #define ezi2c_RTS_HSIOM_MASK     (ezi2c_rts__0__HSIOM_MASK)
    #define ezi2c_RTS_HSIOM_POS      (ezi2c_rts__0__HSIOM_SHIFT)    
    #define ezi2c_RTS_HSIOM_SEL_GPIO (ezi2c_rts__0__HSIOM_GPIO)
    #define ezi2c_RTS_HSIOM_SEL_UART (ezi2c_rts__0__HSIOM_UART)    
#endif /* (ezi2c_UART_RTS_PIN) */


/***************************************
*        Registers Constants
***************************************/

/* HSIOM switch values. */ 
#define ezi2c_HSIOM_DEF_SEL      (0x00u)
#define ezi2c_HSIOM_GPIO_SEL     (0x00u)
/* The HSIOM values provided below are valid only for ezi2c_CY_SCBIP_V0 
* and ezi2c_CY_SCBIP_V1. It is not recommended to use them for 
* ezi2c_CY_SCBIP_V2. Use pin name specific HSIOM constants provided 
* above instead for any SCB IP block version.
*/
#define ezi2c_HSIOM_UART_SEL     (0x09u)
#define ezi2c_HSIOM_I2C_SEL      (0x0Eu)
#define ezi2c_HSIOM_SPI_SEL      (0x0Fu)

/* Pins settings index. */
#define ezi2c_RX_WAKE_SCL_MOSI_PIN_INDEX   (0u)
#define ezi2c_RX_SCL_MOSI_PIN_INDEX       (0u)
#define ezi2c_TX_SDA_MISO_PIN_INDEX       (1u)
#define ezi2c_SCLK_PIN_INDEX       (2u)
#define ezi2c_SS0_PIN_INDEX       (3u)
#define ezi2c_SS1_PIN_INDEX                  (4u)
#define ezi2c_SS2_PIN_INDEX                  (5u)
#define ezi2c_SS3_PIN_INDEX                  (6u)

/* Pins settings mask. */
#define ezi2c_RX_WAKE_SCL_MOSI_PIN_MASK ((uint32) 0x01u << ezi2c_RX_WAKE_SCL_MOSI_PIN_INDEX)
#define ezi2c_RX_SCL_MOSI_PIN_MASK     ((uint32) 0x01u << ezi2c_RX_SCL_MOSI_PIN_INDEX)
#define ezi2c_TX_SDA_MISO_PIN_MASK     ((uint32) 0x01u << ezi2c_TX_SDA_MISO_PIN_INDEX)
#define ezi2c_SCLK_PIN_MASK     ((uint32) 0x01u << ezi2c_SCLK_PIN_INDEX)
#define ezi2c_SS0_PIN_MASK     ((uint32) 0x01u << ezi2c_SS0_PIN_INDEX)
#define ezi2c_SS1_PIN_MASK                ((uint32) 0x01u << ezi2c_SS1_PIN_INDEX)
#define ezi2c_SS2_PIN_MASK                ((uint32) 0x01u << ezi2c_SS2_PIN_INDEX)
#define ezi2c_SS3_PIN_MASK                ((uint32) 0x01u << ezi2c_SS3_PIN_INDEX)

/* Pin interrupt constants. */
#define ezi2c_INTCFG_TYPE_MASK           (0x03u)
#define ezi2c_INTCFG_TYPE_FALLING_EDGE   (0x02u)

/* Pin Drive Mode constants. */
#define ezi2c_PIN_DM_ALG_HIZ  (0u)
#define ezi2c_PIN_DM_DIG_HIZ  (1u)
#define ezi2c_PIN_DM_OD_LO    (4u)
#define ezi2c_PIN_DM_STRONG   (6u)


/***************************************
*          Macro Definitions
***************************************/

/* Return drive mode of the pin */
#define ezi2c_DM_MASK    (0x7u)
#define ezi2c_DM_SIZE    (3u)
#define ezi2c_GET_P4_PIN_DM(reg, pos) \
    ( ((reg) & (uint32) ((uint32) ezi2c_DM_MASK << (ezi2c_DM_SIZE * (pos)))) >> \
                                                              (ezi2c_DM_SIZE * (pos)) )

#if (ezi2c_TX_SDA_MISO_PIN)
    #define ezi2c_CHECK_TX_SDA_MISO_PIN_USED \
                (ezi2c_PIN_DM_ALG_HIZ != \
                    ezi2c_GET_P4_PIN_DM(ezi2c_uart_tx_i2c_sda_spi_miso_PC, \
                                                   ezi2c_uart_tx_i2c_sda_spi_miso_SHIFT))
#endif /* (ezi2c_TX_SDA_MISO_PIN) */

#if (ezi2c_SS0_PIN)
    #define ezi2c_CHECK_SS0_PIN_USED \
                (ezi2c_PIN_DM_ALG_HIZ != \
                    ezi2c_GET_P4_PIN_DM(ezi2c_spi_ss0_PC, \
                                                   ezi2c_spi_ss0_SHIFT))
#endif /* (ezi2c_SS0_PIN) */

/* Set bits-mask in register */
#define ezi2c_SET_REGISTER_BITS(reg, mask, pos, mode) \
                    do                                           \
                    {                                            \
                        (reg) = (((reg) & ((uint32) ~(uint32) (mask))) | ((uint32) ((uint32) (mode) << (pos)))); \
                    }while(0)

/* Set bit in the register */
#define ezi2c_SET_REGISTER_BIT(reg, mask, val) \
                    ((val) ? ((reg) |= (mask)) : ((reg) &= ((uint32) ~((uint32) (mask)))))

#define ezi2c_SET_HSIOM_SEL(reg, mask, pos, sel) ezi2c_SET_REGISTER_BITS(reg, mask, pos, sel)
#define ezi2c_SET_INCFG_TYPE(reg, mask, pos, intType) \
                                                        ezi2c_SET_REGISTER_BITS(reg, mask, pos, intType)
#define ezi2c_SET_INP_DIS(reg, mask, val) ezi2c_SET_REGISTER_BIT(reg, mask, val)

/* ezi2c_SET_I2C_SCL_DR(val) - Sets I2C SCL DR register.
*  ezi2c_SET_I2C_SCL_HSIOM_SEL(sel) - Sets I2C SCL HSIOM settings.
*/
/* SCB I2C: scl signal */
#if (ezi2c_CY_SCBIP_V0)
#if (ezi2c_I2C_PINS)
    #define ezi2c_SET_I2C_SCL_DR(val) ezi2c_scl_Write(val)

    #define ezi2c_SET_I2C_SCL_HSIOM_SEL(sel) \
                          ezi2c_SET_HSIOM_SEL(ezi2c_SCL_HSIOM_REG,  \
                                                         ezi2c_SCL_HSIOM_MASK, \
                                                         ezi2c_SCL_HSIOM_POS,  \
                                                         (sel))
    #define ezi2c_WAIT_SCL_SET_HIGH  (0u == ezi2c_scl_Read())

/* Unconfigured SCB: scl signal */
#elif (ezi2c_RX_WAKE_SCL_MOSI_PIN)
    #define ezi2c_SET_I2C_SCL_DR(val) \
                            ezi2c_uart_rx_wake_i2c_scl_spi_mosi_Write(val)

    #define ezi2c_SET_I2C_SCL_HSIOM_SEL(sel) \
                    ezi2c_SET_HSIOM_SEL(ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG,  \
                                                   ezi2c_RX_WAKE_SCL_MOSI_HSIOM_MASK, \
                                                   ezi2c_RX_WAKE_SCL_MOSI_HSIOM_POS,  \
                                                   (sel))

    #define ezi2c_WAIT_SCL_SET_HIGH  (0u == ezi2c_uart_rx_wake_i2c_scl_spi_mosi_Read())

#elif (ezi2c_RX_SCL_MOSI_PIN)
    #define ezi2c_SET_I2C_SCL_DR(val) \
                            ezi2c_uart_rx_i2c_scl_spi_mosi_Write(val)


    #define ezi2c_SET_I2C_SCL_HSIOM_SEL(sel) \
                            ezi2c_SET_HSIOM_SEL(ezi2c_RX_SCL_MOSI_HSIOM_REG,  \
                                                           ezi2c_RX_SCL_MOSI_HSIOM_MASK, \
                                                           ezi2c_RX_SCL_MOSI_HSIOM_POS,  \
                                                           (sel))

    #define ezi2c_WAIT_SCL_SET_HIGH  (0u == ezi2c_uart_rx_i2c_scl_spi_mosi_Read())

#else
    #define ezi2c_SET_I2C_SCL_DR(val)        do{ /* Does nothing */ }while(0)
    #define ezi2c_SET_I2C_SCL_HSIOM_SEL(sel) do{ /* Does nothing */ }while(0)

    #define ezi2c_WAIT_SCL_SET_HIGH  (0u)
#endif /* (ezi2c_I2C_PINS) */

/* SCB I2C: sda signal */
#if (ezi2c_I2C_PINS)
    #define ezi2c_WAIT_SDA_SET_HIGH  (0u == ezi2c_sda_Read())
/* Unconfigured SCB: sda signal */
#elif (ezi2c_TX_SDA_MISO_PIN)
    #define ezi2c_WAIT_SDA_SET_HIGH  (0u == ezi2c_uart_tx_i2c_sda_spi_miso_Read())
#else
    #define ezi2c_WAIT_SDA_SET_HIGH  (0u)
#endif /* (ezi2c_MOSI_SCL_RX_PIN) */
#endif /* (ezi2c_CY_SCBIP_V0) */

/* Clear UART wakeup source */
#if (ezi2c_RX_SCL_MOSI_PIN)
    #define ezi2c_CLEAR_UART_RX_WAKE_INTR        do{ /* Does nothing */ }while(0)
    
#elif (ezi2c_RX_WAKE_SCL_MOSI_PIN)
    #define ezi2c_CLEAR_UART_RX_WAKE_INTR \
            do{                                      \
                (void) ezi2c_uart_rx_wake_i2c_scl_spi_mosi_ClearInterrupt(); \
            }while(0)

#elif(ezi2c_UART_RX_WAKE_PIN)
    #define ezi2c_CLEAR_UART_RX_WAKE_INTR \
            do{                                      \
                (void) ezi2c_rx_wake_ClearInterrupt(); \
            }while(0)
#else
#endif /* (ezi2c_RX_SCL_MOSI_PIN) */


/***************************************
* The following code is DEPRECATED and
* must not be used.
***************************************/

/* Unconfigured pins */
#define ezi2c_REMOVE_MOSI_SCL_RX_WAKE_PIN    ezi2c_REMOVE_RX_WAKE_SCL_MOSI_PIN
#define ezi2c_REMOVE_MOSI_SCL_RX_PIN         ezi2c_REMOVE_RX_SCL_MOSI_PIN
#define ezi2c_REMOVE_MISO_SDA_TX_PIN         ezi2c_REMOVE_TX_SDA_MISO_PIN
#ifndef ezi2c_REMOVE_SCLK_PIN
#define ezi2c_REMOVE_SCLK_PIN                ezi2c_REMOVE_SCLK_PIN
#endif /* ezi2c_REMOVE_SCLK_PIN */
#ifndef ezi2c_REMOVE_SS0_PIN
#define ezi2c_REMOVE_SS0_PIN                 ezi2c_REMOVE_SS0_PIN
#endif /* ezi2c_REMOVE_SS0_PIN */

/* Unconfigured pins */
#define ezi2c_MOSI_SCL_RX_WAKE_PIN   ezi2c_RX_WAKE_SCL_MOSI_PIN
#define ezi2c_MOSI_SCL_RX_PIN        ezi2c_RX_SCL_MOSI_PIN
#define ezi2c_MISO_SDA_TX_PIN        ezi2c_TX_SDA_MISO_PIN
#ifndef ezi2c_SCLK_PIN
#define ezi2c_SCLK_PIN               ezi2c_SCLK_PIN
#endif /* ezi2c_SCLK_PIN */
#ifndef ezi2c_SS0_PIN
#define ezi2c_SS0_PIN                ezi2c_SS0_PIN
#endif /* ezi2c_SS0_PIN */

#if (ezi2c_MOSI_SCL_RX_WAKE_PIN)
    #define ezi2c_MOSI_SCL_RX_WAKE_HSIOM_REG     ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG
    #define ezi2c_MOSI_SCL_RX_WAKE_HSIOM_PTR     ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG
    #define ezi2c_MOSI_SCL_RX_WAKE_HSIOM_MASK    ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG
    #define ezi2c_MOSI_SCL_RX_WAKE_HSIOM_POS     ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG

    #define ezi2c_MOSI_SCL_RX_WAKE_INTCFG_REG    ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG
    #define ezi2c_MOSI_SCL_RX_WAKE_INTCFG_PTR    ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG

    #define ezi2c_MOSI_SCL_RX_WAKE_INTCFG_TYPE_POS   ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG
    #define ezi2c_MOSI_SCL_RX_WAKE_INTCFG_TYPE_MASK  ezi2c_RX_WAKE_SCL_MOSI_HSIOM_REG
#endif /* (ezi2c_RX_WAKE_SCL_MOSI_PIN) */

#if (ezi2c_MOSI_SCL_RX_PIN)
    #define ezi2c_MOSI_SCL_RX_HSIOM_REG      ezi2c_RX_SCL_MOSI_HSIOM_REG
    #define ezi2c_MOSI_SCL_RX_HSIOM_PTR      ezi2c_RX_SCL_MOSI_HSIOM_PTR
    #define ezi2c_MOSI_SCL_RX_HSIOM_MASK     ezi2c_RX_SCL_MOSI_HSIOM_MASK
    #define ezi2c_MOSI_SCL_RX_HSIOM_POS      ezi2c_RX_SCL_MOSI_HSIOM_POS
#endif /* (ezi2c_MOSI_SCL_RX_PIN) */

#if (ezi2c_MISO_SDA_TX_PIN)
    #define ezi2c_MISO_SDA_TX_HSIOM_REG      ezi2c_TX_SDA_MISO_HSIOM_REG
    #define ezi2c_MISO_SDA_TX_HSIOM_PTR      ezi2c_TX_SDA_MISO_HSIOM_REG
    #define ezi2c_MISO_SDA_TX_HSIOM_MASK     ezi2c_TX_SDA_MISO_HSIOM_REG
    #define ezi2c_MISO_SDA_TX_HSIOM_POS      ezi2c_TX_SDA_MISO_HSIOM_REG
#endif /* (ezi2c_MISO_SDA_TX_PIN_PIN) */

#if (ezi2c_SCLK_PIN)
    #ifndef ezi2c_SCLK_HSIOM_REG
    #define ezi2c_SCLK_HSIOM_REG     ezi2c_SCLK_HSIOM_REG
    #define ezi2c_SCLK_HSIOM_PTR     ezi2c_SCLK_HSIOM_PTR
    #define ezi2c_SCLK_HSIOM_MASK    ezi2c_SCLK_HSIOM_MASK
    #define ezi2c_SCLK_HSIOM_POS     ezi2c_SCLK_HSIOM_POS
    #endif /* ezi2c_SCLK_HSIOM_REG */
#endif /* (ezi2c_SCLK_PIN) */

#if (ezi2c_SS0_PIN)
    #ifndef ezi2c_SS0_HSIOM_REG
    #define ezi2c_SS0_HSIOM_REG      ezi2c_SS0_HSIOM_REG
    #define ezi2c_SS0_HSIOM_PTR      ezi2c_SS0_HSIOM_PTR
    #define ezi2c_SS0_HSIOM_MASK     ezi2c_SS0_HSIOM_MASK
    #define ezi2c_SS0_HSIOM_POS      ezi2c_SS0_HSIOM_POS
    #endif /* ezi2c_SS0_HSIOM_REG */
#endif /* (ezi2c_SS0_PIN) */

#define ezi2c_MOSI_SCL_RX_WAKE_PIN_INDEX ezi2c_RX_WAKE_SCL_MOSI_PIN_INDEX
#define ezi2c_MOSI_SCL_RX_PIN_INDEX      ezi2c_RX_SCL_MOSI_PIN_INDEX
#define ezi2c_MISO_SDA_TX_PIN_INDEX      ezi2c_TX_SDA_MISO_PIN_INDEX
#ifndef ezi2c_SCLK_PIN_INDEX
#define ezi2c_SCLK_PIN_INDEX             ezi2c_SCLK_PIN_INDEX
#endif /* ezi2c_SCLK_PIN_INDEX */
#ifndef ezi2c_SS0_PIN_INDEX
#define ezi2c_SS0_PIN_INDEX              ezi2c_SS0_PIN_INDEX
#endif /* ezi2c_SS0_PIN_INDEX */

#define ezi2c_MOSI_SCL_RX_WAKE_PIN_MASK ezi2c_RX_WAKE_SCL_MOSI_PIN_MASK
#define ezi2c_MOSI_SCL_RX_PIN_MASK      ezi2c_RX_SCL_MOSI_PIN_MASK
#define ezi2c_MISO_SDA_TX_PIN_MASK      ezi2c_TX_SDA_MISO_PIN_MASK
#ifndef ezi2c_SCLK_PIN_MASK
#define ezi2c_SCLK_PIN_MASK             ezi2c_SCLK_PIN_MASK
#endif /* ezi2c_SCLK_PIN_MASK */
#ifndef ezi2c_SS0_PIN_MASK
#define ezi2c_SS0_PIN_MASK              ezi2c_SS0_PIN_MASK
#endif /* ezi2c_SS0_PIN_MASK */

#endif /* (CY_SCB_PINS_ezi2c_H) */


/* [] END OF FILE */
