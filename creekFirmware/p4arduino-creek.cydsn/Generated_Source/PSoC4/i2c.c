/*******************************************************************************
* File Name: i2c.c
* Version 1.10
*
* Description:
*  This file provides the source code to the API for the SCB Component.
*
* Note:
*
*******************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

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


/**********************************
*    Run Time Configuration Vars
**********************************/

/* Stores internal component configuration for unconfigured mode */
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    /* Common config vars */
    uint8 i2c_scbMode = i2c_SCB_MODE_UNCONFIG;
    uint8 i2c_scbEnableWake;
    uint8 i2c_scbEnableIntr;

    /* I2C config vars */
    uint8 i2c_mode;
    uint8 i2c_acceptAddr;

    /* SPI/UART config vars */
    volatile uint8 * i2c_rxBuffer;
    uint8  i2c_rxDataBits;
    uint32 i2c_rxBufferSize;

    volatile uint8 * i2c_txBuffer;
    uint8  i2c_txDataBits;
    uint32 i2c_txBufferSize;

    /* EZI2C config vars */
    uint8 i2c_numberOfAddr;
    uint8 i2c_subAddrSize;
#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */


/**********************************
*     Common SCB Vars
**********************************/

uint8 i2c_initVar = 0u;
cyisraddress i2c_customIntrHandler = NULL;


/***************************************
*    Private Function Prototypes
***************************************/

static void i2c_ScbEnableIntr(void);
static void i2c_ScbModeStop(void);


/*******************************************************************************
* Function Name: i2c_Init
********************************************************************************
*
* Summary:
*  Initializes SCB component to operate in one of selected configurations:
*  I2C, SPI, UART, EZI2C or EZSPI.
*  This function does not do any initialization when configuration is set to
*  Unconfigured SCB.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_Init(void)
{
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    if(i2c_SCB_MODE_UNCONFIG_RUNTM_CFG)
    {
        i2c_initVar = 0u; /* Clear init var */
    }
    else
    {
        /* Initialization was done before call */
    }

#elif(i2c_SCB_MODE_I2C_CONST_CFG)
    i2c_I2CInit();

#elif(i2c_SCB_MODE_SPI_CONST_CFG)
    i2c_SpiInit();

#elif(i2c_SCB_MODE_UART_CONST_CFG)
    i2c_UartInit();

#elif(i2c_SCB_MODE_EZI2C_CONST_CFG)
    i2c_EzI2CInit();

#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_Enable
********************************************************************************
*
* Summary:
*  Enables SCB component operation.
*  The SCB configuration should be not changed when the component is enabled.
*  Any configuration changes should be made after disabling the component.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_Enable(void)
{
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    if(!i2c_SCB_MODE_UNCONFIG_RUNTM_CFG)
    {
        /* Enable SCB block, only if already configured */
        i2c_CTRL_REG |= i2c_CTRL_ENABLED;
        
        /* Enable interrupt */
        i2c_ScbEnableIntr();
    }
#else
    i2c_CTRL_REG |= i2c_CTRL_ENABLED; /* Enable SCB block */
    
    i2c_ScbEnableIntr();
#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


/*******************************************************************************
* Function Name: i2c_Start
********************************************************************************
*
* Summary:
*  Invokes SCB_Init() and SCB_Enable().
*  After this function call the component is enabled and ready for operation.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_initVar - used to check initial configuration, modified
*  on first function call.
*
*******************************************************************************/
void i2c_Start(void)
{
    if(0u == i2c_initVar)
    {
        i2c_initVar = 1u; /* Component was initialized */
        i2c_Init();       /* Initialize component      */
    }

    i2c_Enable();
}


/*******************************************************************************
* Function Name: i2c_Stop
********************************************************************************
*
* Summary:
*  Disables the SCB component.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void i2c_Stop(void)
{
#if(i2c_SCB_IRQ_INTERNAL)
    i2c_SCB_IRQ_Disable();     /* Disable interrupt before block */
#endif /* (i2c_SCB_IRQ_INTERNAL) */

    i2c_CTRL_REG &= (uint32) ~i2c_CTRL_ENABLED;  /* Disable SCB block */

#if(i2c_SCB_IRQ_INTERNAL)
    i2c_SCB_IRQ_ClearPending(); /* Clear pending interrupt */
#endif /* (i2c_SCB_IRQ_INTERNAL) */
    
    i2c_ScbModeStop(); /* Calls scbMode specific Stop function */
}


/*******************************************************************************
* Function Name: i2c_SetCustomInterruptHandler
********************************************************************************
*
* Summary:
*  Registers a function to be called by the internal interrupt handler.
*  First the function that is registered is called, then the internal interrupt
*  handler performs any operations such as software buffer management functions
*  before the interrupt returns.  It is user's responsibility to not break the
*  software buffer operations. Only one custom handler is supported, which is
*  the function provided by the most recent call.
*  At initialization time no custom handler is registered.
*
* Parameters:
*  func: Pointer to the function to register.
*        The value NULL indicates to remove the current custom interrupt
*        handler.
*
* Return:
*  None
*
*******************************************************************************/
void i2c_SetCustomInterruptHandler(cyisraddress func)
{
    i2c_customIntrHandler = func; /* Register interrupt handler */
}


/*******************************************************************************
* Function Name: i2c_ScbModeEnableIntr
********************************************************************************
*
* Summary:
*  Enables interrupt for specific mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
static void i2c_ScbEnableIntr(void)
{
#if(i2c_SCB_IRQ_INTERNAL)
    #if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
        /* Enable interrupt source */
        if(0u != i2c_scbEnableIntr)
        {
            i2c_SCB_IRQ_Enable();
        }
    #else
        i2c_SCB_IRQ_Enable();
        
    #endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
#endif /* (i2c_SCB_IRQ_INTERNAL) */
}


/*******************************************************************************
* Function Name: i2c_ScbModeEnableIntr
********************************************************************************
*
* Summary:
*  Calls Stop function for specific operation mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
static void i2c_ScbModeStop(void)
{
#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    if(i2c_SCB_MODE_I2C_RUNTM_CFG)
    {
        i2c_I2CStop();
    }
    else if(i2c_SCB_MODE_EZI2C_RUNTM_CFG)
    {
        i2c_EzI2CStop();
    }
    else
    {
        /* None of modes above */
    }
#elif(i2c_SCB_MODE_I2C_CONST_CFG)
    i2c_I2CStop();

#elif(i2c_SCB_MODE_EZI2C_CONST_CFG)
    i2c_EzI2CStop();

#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */
}


#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)
    /*******************************************************************************
    * Function Name: i2c_SetPins
    ********************************************************************************
    *
    * Summary:
    *  Sets pins settings accordingly to selected operation mode.
    *  Only available in Unconfigured operation mode. The mode specific
    *  initialization function calls it.
    *  Pins configuration is set by PSoC Creator when specific mode of operation
    *  selected in design time.
    *
    * Parameters:
    *  mode:      Mode of SCB operation.
    *  subMode:   Submode of SCB operation. It is only required for SPI and UART
    *             modes.
    *  uartTxRx:  Direction for UART.
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void i2c_SetPins(uint32 mode, uint32 subMode, uint32 uartTxRx)
    {
        uint32 hsiomSel[i2c_SCB_PINS_NUMBER];
        uint32 pinsDm  [i2c_SCB_PINS_NUMBER];
        uint32 i;

        /* Make all unused */
        for(i = 0u; i < i2c_SCB_PINS_NUMBER; i++)
        {
            hsiomSel[i] = i2c_HSIOM_DEF_SEL;
            pinsDm[i]   = i2c_PIN_DM_ALG_HIZ;
        }

        /* Choice the Dm and HSIOM */
        if((i2c_SCB_MODE_I2C   == mode) ||
           (i2c_SCB_MODE_EZI2C == mode))
        {
            hsiomSel[i2c_MOSI_SCL_RX_PIN_INDEX] = i2c_HSIOM_I2C_SEL;
            hsiomSel[i2c_MISO_SDA_TX_PIN_INDEX] = i2c_HSIOM_I2C_SEL;

            pinsDm[i2c_MOSI_SCL_RX_PIN_INDEX] = i2c_PIN_DM_OD_LO;
            pinsDm[i2c_MISO_SDA_TX_PIN_INDEX] = i2c_PIN_DM_OD_LO;
        }
        else if(i2c_SCB_MODE_SPI == mode)
        {
            hsiomSel[i2c_MOSI_SCL_RX_PIN_INDEX] = i2c_HSIOM_SPI_SEL;
            hsiomSel[i2c_MISO_SDA_TX_PIN_INDEX] = i2c_HSIOM_SPI_SEL;
            hsiomSel[i2c_SCLK_PIN_INDEX]        = i2c_HSIOM_SPI_SEL;

            if(i2c_SPI_SLAVE == subMode)
            {
                /* Slave */
                pinsDm[i2c_MOSI_SCL_RX_PIN_INDEX] = i2c_PIN_DM_DIG_HIZ;
                pinsDm[i2c_MISO_SDA_TX_PIN_INDEX] = i2c_PIN_DM_STRONG;
                pinsDm[i2c_SCLK_PIN_INDEX]        = i2c_PIN_DM_DIG_HIZ;

            #if(i2c_SS0_PIN)
                /* Only SS0 is valid choice for Slave */
                hsiomSel[i2c_SS0_PIN_INDEX] = i2c_HSIOM_SPI_SEL;
                pinsDm  [i2c_SS0_PIN_INDEX] = i2c_PIN_DM_DIG_HIZ;
            #endif /* (i2c_SS1_PIN) */
            }
            else /* (Master) */
            {
                pinsDm[i2c_MOSI_SCL_RX_PIN_INDEX] = i2c_PIN_DM_STRONG;
                pinsDm[i2c_MISO_SDA_TX_PIN_INDEX] = i2c_PIN_DM_DIG_HIZ;
                pinsDm[i2c_SCLK_PIN_INDEX]        = i2c_PIN_DM_STRONG;

            #if(i2c_SS0_PIN)
                hsiomSel[i2c_SS0_PIN_INDEX] = i2c_HSIOM_SPI_SEL;
                pinsDm  [i2c_SS0_PIN_INDEX] = i2c_PIN_DM_STRONG;
            #endif /* (i2c_SS0_PIN) */

            #if(i2c_SS1_PIN)
                hsiomSel[i2c_SS1_PIN_INDEX] = i2c_HSIOM_SPI_SEL;
                pinsDm  [i2c_SS1_PIN_INDEX] = i2c_PIN_DM_STRONG;
            #endif /* (i2c_SS1_PIN) */

            #if(i2c_SS2_PIN)
                hsiomSel[i2c_SS2_PIN_INDEX] = i2c_HSIOM_SPI_SEL;
                pinsDm  [i2c_SS2_PIN_INDEX] = i2c_PIN_DM_STRONG;
            #endif /* (i2c_SS2_PIN) */

            #if(i2c_SS3_PIN)
                hsiomSel[i2c_SS3_PIN_INDEX] = i2c_HSIOM_SPI_SEL;
                pinsDm  [i2c_SS3_PIN_INDEX] = i2c_PIN_DM_STRONG;
            #endif /* (i2c_SS2_PIN) */
            }
        }
        else /* UART */
        {
            if(i2c_UART_MODE_SMARTCARD == subMode)
            {
                /* SmartCard */
                hsiomSel[i2c_MISO_SDA_TX_PIN_INDEX] = i2c_HSIOM_UART_SEL;
                pinsDm  [i2c_MISO_SDA_TX_PIN_INDEX] = i2c_PIN_DM_OD_LO;
            }
            else /* Standard or IrDA */
            {
                if(0u != (i2c_UART_RX & uartTxRx))
                {
                    hsiomSel[i2c_MOSI_SCL_RX_PIN_INDEX] = i2c_HSIOM_UART_SEL;
                    pinsDm  [i2c_MOSI_SCL_RX_PIN_INDEX] = i2c_PIN_DM_DIG_HIZ;
                }

                if(0u != (i2c_UART_TX & uartTxRx))
                {
                    hsiomSel[i2c_MISO_SDA_TX_PIN_INDEX] = i2c_HSIOM_UART_SEL;
                    pinsDm  [i2c_MISO_SDA_TX_PIN_INDEX] = i2c_PIN_DM_STRONG;
                }
            }
        }

        /* Condfigure pins: set HSIOM and DM */
        /* Condfigure pins: DR registers configuration remains unchanged for cyfitter_cfg() */

    #if(i2c_MOSI_SCL_RX_PIN)
        i2c_SET_HSIOM_SEL(i2c_MOSI_SCL_RX_HSIOM_REG,
                                       i2c_MOSI_SCL_RX_HSIOM_MASK,
                                       i2c_MOSI_SCL_RX_HSIOM_POS,
                                       hsiomSel[i2c_MOSI_SCL_RX_PIN_INDEX]);
    #endif /* (i2c_MOSI_SCL_RX_PIN) */

    #if(i2c_MOSI_SCL_RX_WAKE_PIN)
        i2c_SET_HSIOM_SEL(i2c_MOSI_SCL_RX_WAKE_HSIOM_REG,
                                       i2c_MOSI_SCL_RX_WAKE_HSIOM_MASK,
                                       i2c_MOSI_SCL_RX_WAKE_HSIOM_POS,
                                       hsiomSel[i2c_MOSI_SCL_RX_WAKE_PIN_INDEX]);
    #endif /* (i2c_MOSI_SCL_RX_WAKE_PIN) */

    #if(i2c_MISO_SDA_TX_PIN)
        i2c_SET_HSIOM_SEL(i2c_MISO_SDA_TX_HSIOM_REG,
                                       i2c_MISO_SDA_TX_HSIOM_MASK,
                                       i2c_MISO_SDA_TX_HSIOM_POS,
                                       hsiomSel[i2c_MISO_SDA_TX_PIN_INDEX]);
    #endif /* (i2c_MOSI_SCL_RX_PIN) */

    #if(i2c_SCLK_PIN)
        i2c_SET_HSIOM_SEL(i2c_SCLK_HSIOM_REG, i2c_SCLK_HSIOM_MASK,
                                       i2c_SCLK_HSIOM_POS, hsiomSel[i2c_SCLK_PIN_INDEX]);
    #endif /* (i2c_SCLK_PIN) */

    #if(i2c_SS0_PIN)
        i2c_SET_HSIOM_SEL(i2c_SS0_HSIOM_REG, i2c_SS0_HSIOM_MASK,
                                       i2c_SS0_HSIOM_POS, hsiomSel[i2c_SS0_PIN_INDEX]);
    #endif /* (i2c_SS1_PIN) */

    #if(i2c_SS1_PIN)
        i2c_SET_HSIOM_SEL(i2c_SS1_HSIOM_REG, i2c_SS1_HSIOM_MASK,
                                       i2c_SS1_HSIOM_POS, hsiomSel[i2c_SS1_PIN_INDEX]);
    #endif /* (i2c_SS1_PIN) */

    #if(i2c_SS2_PIN)
        i2c_SET_HSIOM_SEL(i2c_SS2_HSIOM_REG, i2c_SS2_HSIOM_MASK,
                                       i2c_SS2_HSIOM_POS, hsiomSel[i2c_SS2_PIN_INDEX]);
    #endif /* (i2c_SS2_PIN) */

    #if(i2c_SS3_PIN)
        i2c_SET_HSIOM_SEL(i2c_SS3_HSIOM_REG,  i2c_SS3_HSIOM_MASK,
                                       i2c_SS3_HSIOM_POS, hsiomSel[i2c_SS3_PIN_INDEX]);
    #endif /* (i2c_SS3_PIN) */



    #if(i2c_MOSI_SCL_RX_PIN)
        i2c_spi_mosi_i2c_scl_uart_rx_SetDriveMode((uint8)
                                                               pinsDm[i2c_MOSI_SCL_RX_PIN_INDEX]);
    #endif /* (i2c_MOSI_SCL_RX_PIN) */

    #if(i2c_MOSI_SCL_RX_WAKE_PIN)
    i2c_spi_mosi_i2c_scl_uart_rx_wake_SetDriveMode((uint8)
                                                               pinsDm[i2c_MOSI_SCL_RX_WAKE_PIN_INDEX]);

    /* Set interrupt on rising edge */
    i2c_SET_INCFG_TYPE(i2c_MOSI_SCL_RX_WAKE_INTCFG_REG,
                                    i2c_MOSI_SCL_RX_WAKE_INTCFG_TYPE_MASK,
                                    i2c_MOSI_SCL_RX_WAKE_INTCFG_TYPE_POS,
                                    i2c_INTCFG_TYPE_FALLING_EDGE);

    #endif /* (i2c_MOSI_SCL_RX_WAKE_PIN) */

    #if(i2c_MISO_SDA_TX_PIN)
        i2c_spi_miso_i2c_sda_uart_tx_SetDriveMode((uint8)
                                                                    pinsDm[i2c_MISO_SDA_TX_PIN_INDEX]);
    #endif /* (i2c_MOSI_SCL_RX_PIN) */

    #if(i2c_SCLK_PIN)
        i2c_spi_sclk_SetDriveMode((uint8) pinsDm[i2c_SCLK_PIN_INDEX]);
    #endif /* (i2c_SCLK_PIN) */

    #if(i2c_SS0_PIN)
        i2c_spi_ss0_SetDriveMode((uint8) pinsDm[i2c_SS0_PIN_INDEX]);
    #endif /* (i2c_SS0_PIN) */

    #if(i2c_SS1_PIN)
        i2c_spi_ss1_SetDriveMode((uint8) pinsDm[i2c_SS1_PIN_INDEX]);
    #endif /* (i2c_SS1_PIN) */

    #if(i2c_SS2_PIN)
        i2c_spi_ss2_SetDriveMode((uint8) pinsDm[i2c_SS2_PIN_INDEX]);
    #endif /* (i2c_SS2_PIN) */

    #if(i2c_SS3_PIN)
        i2c_spi_ss3_SetDriveMode((uint8) pinsDm[i2c_SS3_PIN_INDEX]);
    #endif /* (i2c_SS3_PIN) */
    }

#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */


/* [] END OF FILE */
