/*******************************************************************************
* File Name: i2c_PM.c
* Version 1.10
*
* Description:
*  This file provides the source code to the Power Management support for
*  the SCB Component.
*
* Note:
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "i2c.h"
#include "i2c_PVT.h"

#if(i2c_SCB_MODE_I2C_INC)
    #include "i2c_I2C_PVT.h"
#endif /* (i2c_SCB_MODE_I2C_INC) */

#if(i2c_SCB_MODE_EZI2C_INC)
    #include "i2c_EZI2C_PVT.h"
#endif /* (i2c_SCB_MODE_EZI2C_INC) */

#if(i2c_SCB_MODE_SPI_INC || i2c_SCB_MODE_UART_INC)
    #include "i2c_SPI_UART_PVT.h"
#endif /* (i2c_SCB_MODE_SPI_INC || i2c_SCB_MODE_UART_INC) */


/***************************************
*   Backup Structure declaration
***************************************/

i2c_BACKUP_STRUCT i2c_backup =
{
    0u, /* enableState */
};


/*******************************************************************************
* Function Name: i2c_Sleep
********************************************************************************
*
* Summary:
*  Calls SaveConfig function fucntion for selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_Sleep(void)
{
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)

    if(0u != i2c_scbEnableWake)
    {
        if(i2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            i2c_I2CSaveConfig();
        }
        else if(i2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            i2c_SpiSaveConfig();
        }
        else if(i2c_SCB_MODE_UART_RUNTM_CFG)
        {
            i2c_UartSaveConfig();
        }
        else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            i2c_EzI2CSaveConfig();
        }
        else
        {
            /* Unknown mode: do nothing */
        }
    }
    else
    {
        i2c_backup.enableState = (uint8) i2c_GET_CTRL_ENABLED;
        
        if(0u != i2c_backup.enableState)
        {
            i2c_Stop();
        }
    }
    
    i2c_DisableTxPinsInputBuffer();
    
#else
    
    #if defined (i2c_I2C_WAKE_ENABLE_CONST) && (i2c_I2C_WAKE_ENABLE_CONST)
        i2c_I2CSaveConfig();
        
    #elif defined (i2c_SPI_WAKE_ENABLE_CONST) && (i2c_SPI_WAKE_ENABLE_CONST)
        i2c_SpiSaveConfig();
        
    #elif defined (i2c_UART_WAKE_ENABLE_CONST) && (i2c_UART_WAKE_ENABLE_CONST)
        i2c_UartSaveConfig();
        
    #elif defined (i2c_EZI2C_WAKE_ENABLE_CONST) && (i2c_EZI2C_WAKE_ENABLE_CONST)
        i2c_EzI2CSaveConfig();
    
    #else
        
        i2c_backup.enableState = (uint8) i2c_GET_CTRL_ENABLED;
        
        /* Check enable state */
        if(0u != i2c_backup.enableState)
        {
            i2c_Stop();
        }
        
    #endif /* defined (i2c_SCB_MODE_I2C_CONST_CFG) && (i2c_I2C_WAKE_ENABLE_CONST) */
    
    i2c_DisableTxPinsInputBuffer();
    
#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_Wakeup
********************************************************************************
*
* Summary:
*  Calls RestoreConfig function fucntion for selected mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_Wakeup(void)
{
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)

    i2c_EnableTxPinsInputBuffer();
        
    if(0u != i2c_scbEnableWake)
    {
        if(i2c_SCB_MODE_I2C_RUNTM_CFG)
        {
            i2c_I2CRestoreConfig();
        }
        else if(i2c_SCB_MODE_SPI_RUNTM_CFG)
        {
            i2c_SpiRestoreConfig();
        }
        else if(i2c_SCB_MODE_UART_RUNTM_CFG)
        {
            i2c_UartRestoreConfig();
        }
        else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
        {
            i2c_EzI2CRestoreConfig();
        }
        else
        {
            /* Unknown mode: do nothing */
        }
    }
    else
    {    
        /* Restore enable state */
        if(0u != i2c_backup.enableState)
        {
            i2c_Enable();
        }
    }

#else
    
    i2c_EnableTxPinsInputBuffer();
        
    #if defined (i2c_I2C_WAKE_ENABLE_CONST) && (i2c_I2C_WAKE_ENABLE_CONST)
        i2c_I2CRestoreConfig();
        
    #elif defined (i2c_SPI_WAKE_ENABLE_CONST) && (i2c_SPI_WAKE_ENABLE_CONST)
        i2c_SpiRestoreConfig();
        
    #elif defined (i2c_UART_WAKE_ENABLE_CONST) && (i2c_UART_WAKE_ENABLE_CONST)
        i2c_UartRestoreConfig();
        
    #elif defined (i2c_EZI2C_WAKE_ENABLE_CONST) && (i2c_EZI2C_WAKE_ENABLE_CONST)
        i2c_EzI2CRestoreConfig();
    
    #else
        /* Check enable state */
        if(0u != i2c_backup.enableState)
        {
            i2c_Enable();
        }
        
    #endif /* (i2c_I2C_WAKE_ENABLE_CONST) */

#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_DisableTxPinsInputBuffer
********************************************************************************
*
* Summary:
*  Disables input buffers for TX pins. This action removes leakage current while
*  low power mode (Cypress ID 149635).
*   SCB mode is I2C and EZI2C: bus is pulled-up. Leave pins as it is.
*   SCB mode SPI:
*     Slave  - disable input buffer for MISO pin.
*     Master - disable input buffer for all pins.
*   SCB mode SmartCard: 
*     Standard and IrDA - disable input buffer for TX pin.
*     SmartCard - RX_TX pin is pulled-up. Leave pin as it is.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_DisableTxPinsInputBuffer(void)
{
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    
    /* SCB mode is I2C and EZI2C: bus is pulled-up. Does nothing */
       
    if(i2c_SCB_MODE_SPI_RUNTM_CFG)
    {
        if(0u != (i2c_SPI_CTRL_REG & i2c_SPI_CTRL_MASTER))
        /* SPI Master */
        {
        #if(i2c_MOSI_SCL_RX_WAKE_PIN)
            i2c_spi_mosi_i2c_scl_uart_rx_wake_INP_DIS |= \
                                                                i2c_spi_mosi_i2c_scl_uart_rx_wake_MASK;
        #endif /* (i2c_MOSI_SCL_RX_WAKE_PIN) */

        #if(i2c_MOSI_SCL_RX_PIN)
            i2c_spi_mosi_i2c_scl_uart_rx_INP_DIS |= i2c_spi_mosi_i2c_scl_uart_rx_MASK;
        #endif /* (i2c_MOSI_SCL_RX_PIN) */

        #if(i2c_MISO_SDA_TX_PIN)
            i2c_spi_miso_i2c_sda_uart_tx_INP_DIS |= i2c_spi_miso_i2c_sda_uart_tx_MASK;
        #endif /* (i2c_MISO_SDA_TX_PIN_PIN) */

        #if(i2c_SCLK_PIN)
            i2c_spi_sclk_INP_DIS |= i2c_spi_sclk_MASK;
        #endif /* (i2c_SCLK_PIN) */

        #if(i2c_SS0_PIN)
            i2c_spi_ss0_INP_DIS |= i2c_spi_ss0_MASK;
        #endif /* (i2c_SS1_PIN) */

        #if(i2c_SS1_PIN)
            i2c_spi_ss1_INP_DIS |= i2c_spi_ss1_MASK;
        #endif /* (i2c_SS1_PIN) */

        #if(i2c_SS2_PIN)
            i2c_spi_ss2_INP_DIS |= i2c_spi_ss2_MASK;
        #endif /* (i2c_SS2_PIN) */

        #if(i2c_SS3_PIN)
            i2c_spi_ss3_INP_DIS |= i2c_spi_ss3_MASK;
        #endif /* (i2c_SS3_PIN) */
        }
        else
        /* SPI Slave */
        {
        #if(i2c_MISO_SDA_TX_PIN)
            i2c_spi_miso_i2c_sda_uart_tx_INP_DIS |= i2c_spi_miso_i2c_sda_uart_tx_MASK;
        #endif /* (i2c_MISO_SDA_TX_PIN_PIN) */
        }
    }
    else if (i2c_SCB_MODE_UART_RUNTM_CFG)
    {
        if(i2c_UART_CTRL_MODE_UART_SMARTCARD != 
            (i2c_UART_CTRL_REG & i2c_UART_CTRL_MODE_MASK))
        /* UART Standard or IrDA */
        {
        #if(i2c_MISO_SDA_TX_PIN)
            i2c_spi_miso_i2c_sda_uart_tx_INP_DIS |= i2c_spi_miso_i2c_sda_uart_tx_MASK;
        #endif /* (i2c_MISO_SDA_TX_PIN_PIN) */
        }
    }
    else
    {
        /* Does nothing */
    }
    
#else
    
    /* SCB mode is I2C and EZI2C: bus is pulled-up. Does nothing */
        
    /* SCB mode is SPI Master */
    #if(i2c_SPI_MASTER_PINS)
        i2c_sclk_m_INP_DIS |= i2c_sclk_m_MASK;
        i2c_mosi_m_INP_DIS |= i2c_mosi_m_MASK;
        i2c_miso_m_INP_DIS |= i2c_miso_m_MASK;
    #endif /* (i2c_SPI_MASTER_PINS) */

    #if(i2c_SPI_MASTER_SS0_PIN)
        i2c_ss0_m_INP_DIS |= i2c_ss0_m_MASK;
    #endif /* (i2c_SPI_MASTER_SS0_PIN) */

    #if(i2c_SPI_MASTER_SS1_PIN)
        i2c_ss1_m_INP_DIS |= i2c_ss1_m_MASK;
    #endif /* (i2c_SPI_MASTER_SS1_PIN) */

    #if(i2c_SPI_MASTER_SS2_PIN)
        i2c_ss2_m_INP_DIS |= i2c_ss2_m_MASK;
    #endif /* (i2c_SPI_MASTER_SS2_PIN) */

    #if(i2c_SPI_MASTER_SS3_PIN)
        i2c_ss3_m_INP_DIS |= i2c_ss3_m_MASK;
    #endif /* (i2c_SPI_MASTER_SS3_PIN) */
    
    /* SCB mode is SPI Slave */
    #if(i2c_SPI_SLAVE_PINS)
        i2c_miso_s_INP_DIS |= i2c_miso_s_MASK;
    #endif /* (i2c_SPI_SLAVE_PINS) */

    /* SCB mode is UART */
    #if(i2c_UART_TX_PIN)
        i2c_tx_INP_DIS |= i2c_tx_MASK;
    #endif /* (i2c_UART_TX_PIN) */

#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_EnableTxPinsInputBuffer
********************************************************************************
*
* Summary:
*  Enables input buffers for TX pins. Restore changes done byte
*i2c_DisableTxPinsInputBuffer.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_EnableTxPinsInputBuffer(void)
{
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    if(i2c_SCB_MODE_SPI_RUNTM_CFG)
    {
        if(0u != (i2c_SPI_CTRL_REG & i2c_SPI_CTRL_MASTER))
        /* SPI Master */
        {
        #if(i2c_MOSI_SCL_RX_WAKE_PIN)
            i2c_spi_mosi_i2c_scl_uart_rx_wake_INP_DIS &= \
                                            (uint32) ~((uint32) i2c_spi_mosi_i2c_scl_uart_rx_wake_MASK);
        #endif /* (i2c_MOSI_SCL_RX_WAKE_PIN) */

        #if(i2c_MOSI_SCL_RX_PIN)
            i2c_spi_mosi_i2c_scl_uart_rx_INP_DIS &= \
                                            (uint32) ~((uint32) i2c_spi_mosi_i2c_scl_uart_rx_MASK);
        #endif /* (i2c_MOSI_SCL_RX_PIN) */

        #if(i2c_MISO_SDA_TX_PIN)
            i2c_spi_miso_i2c_sda_uart_tx_INP_DIS &= \
                                            (uint32) ~((uint32) i2c_spi_miso_i2c_sda_uart_tx_MASK);
        #endif /* (i2c_MISO_SDA_TX_PIN_PIN) */

        #if(i2c_SCLK_PIN)
            i2c_spi_sclk_INP_DIS &= (uint32) ~((uint32) i2c_spi_sclk_MASK);
        #endif /* (i2c_SCLK_PIN) */

        #if(i2c_SS0_PIN)
            i2c_spi_ss0_INP_DIS &= (uint32) ~((uint32) i2c_spi_ss0_MASK);
        #endif /* (i2c_SS1_PIN) */

        #if(i2c_SS1_PIN)
            i2c_spi_ss1_INP_DIS &= (uint32) ~((uint32) i2c_spi_ss1_MASK);
        #endif /* (i2c_SS1_PIN) */

        #if(i2c_SS2_PIN)
            i2c_spi_ss2_INP_DIS &= (uint32) ~((uint32) i2c_spi_ss2_MASK);
        #endif /* (i2c_SS2_PIN) */

        #if(i2c_SS3_PIN)
            i2c_spi_ss3_INP_DIS &= (uint32) ~((uint32) i2c_spi_ss3_MASK);
        #endif /* (i2c_SS3_PIN) */
        }
        else
        /* SPI Slave */
        {
        #if(i2c_MISO_SDA_TX_PIN)
            i2c_spi_miso_i2c_sda_uart_tx_INP_DIS &= \
                                                (uint32) ~((uint32) i2c_spi_miso_i2c_sda_uart_tx_MASK);
        #endif /* (i2c_MISO_SDA_TX_PIN_PIN) */
        }
    }
    else if (i2c_SCB_MODE_UART_RUNTM_CFG)
    {
        if(i2c_UART_CTRL_MODE_UART_SMARTCARD != 
                (i2c_UART_CTRL_REG & i2c_UART_CTRL_MODE_MASK))
        /* UART Standard or IrDA */
        {
        #if(i2c_MISO_SDA_TX_PIN)
            i2c_spi_miso_i2c_sda_uart_tx_INP_DIS &= \
                                                (uint32) ~((uint32) i2c_spi_miso_i2c_sda_uart_tx_MASK);
        #endif /* (i2c_MISO_SDA_TX_PIN_PIN) */
        }
    }
    else
    {
        /* Does nothing */
    }
    
#else
        
    /* SCB mode is SPI Master */
    #if(i2c_SPI_MASTER_PINS)
        i2c_sclk_m_INP_DIS &= (uint32) ~((uint32) i2c_sclk_m_MASK);
        i2c_mosi_m_INP_DIS &= (uint32) ~((uint32) i2c_mosi_m_MASK);
        i2c_miso_m_INP_DIS &= (uint32) ~((uint32) i2c_miso_m_MASK);
    #endif /* (i2c_SPI_MASTER_PINS) */

    #if(i2c_SPI_MASTER_SS0_PIN)
        i2c_ss0_m_INP_DIS &= (uint32) ~((uint32) i2c_ss0_m_MASK);
    #endif /* (i2c_SPI_MASTER_SS0_PIN) */

    #if(i2c_SPI_MASTER_SS1_PIN)
        i2c_ss1_m_INP_DIS &= (uint32) ~((uint32) i2c_ss1_m_MASK);
    #endif /* (i2c_SPI_MASTER_SS1_PIN) */

    #if(i2c_SPI_MASTER_SS2_PIN)
        i2c_ss2_m_INP_DIS &= (uint32) ~((uint32) i2c_ss2_m_MASK);
    #endif /* (i2c_SPI_MASTER_SS2_PIN) */

    #if(i2c_SPI_MASTER_SS3_PIN)
        i2c_ss3_m_INP_DIS &= (uint32) ~((uint32) i2c_ss3_m_MASK);
    #endif /* (i2c_SPI_MASTER_SS3_PIN) */
    
    /* SCB mode is SPI Slave */
    #if(i2c_SPI_SLAVE_PINS)
        i2c_miso_s_INP_DIS &= (uint32) ~((uint32) i2c_miso_s_MASK);
    #endif /* (i2c_SPI_SLAVE_PINS) */

    /* SCB mode is UART */
    #if(i2c_UART_TX_PIN)
        i2c_tx_INP_DIS &= (uint32) ~((uint32) i2c_tx_MASK);
    #endif /* (i2c_UART_TX_PIN) */

#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/* [] END OF FILE */
