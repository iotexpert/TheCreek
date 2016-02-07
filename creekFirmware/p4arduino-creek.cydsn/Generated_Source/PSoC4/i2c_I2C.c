/*******************************************************************************
* File Name: i2c_I2C.c
* Version 1.10
*
* Description:
*  This file provides the source code to the API for the SCB Component in
*  I2C mode.
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
#include "i2c_I2C_PVT.h"


/***************************************
*      I2C Private Vars
***************************************/

volatile uint8 i2c_state;  /* Current state of I2C FSM */


#if(i2c_SCB_MODE_UNCONFIG_CONST_CFG)

    /***************************************
    *  Config Structure Initialization
    ***************************************/

    /* Constant configuration of I2C */
    const i2c_I2C_INIT_STRUCT i2c_configI2C =
    {
        i2c_I2C_MODE,
        i2c_I2C_OVS_FACTOR_LOW,
        i2c_I2C_OVS_FACTOR_HIGH,
        i2c_I2C_MEDIAN_FILTER_ENABLE,
        i2c_I2C_SLAVE_ADDRESS,
        i2c_I2C_SLAVE_ADDRESS_MASK,
        i2c_I2C_ACCEPT_ADDRESS,
        i2c_I2C_WAKE_ENABLE
    };

    /*******************************************************************************
    * Function Name: i2c_I2CInit
    ********************************************************************************
    *
    * Summary:
    *  Configures the SCB for I2C operation.
    *
    * Parameters:
    *  config:  Pointer to a structure that contains the following ordered list of
    *           fields. These fields match the selections available in the
    *           customizer.
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void i2c_I2CInit(const i2c_I2C_INIT_STRUCT *config)
    {
        if(NULL == config)
        {
            CYASSERT(0u != 0u); /* Halt execution due bad fucntion parameter */
        }
        else
        {
            /* Configure pins */
            i2c_SetPins(i2c_SCB_MODE_I2C, i2c_DUMMY_PARAM,
                                                                    i2c_DUMMY_PARAM);

            /* Store internal configuration */
            i2c_scbMode       = (uint8) i2c_SCB_MODE_I2C;
            i2c_scbEnableWake = (uint8) config->enableWake;
            i2c_scbEnableIntr = (uint8) i2c_SCB_IRQ_INTERNAL;

            i2c_mode          = (uint8) config->mode;
            i2c_acceptAddr    = (uint8) config->acceptAddr;

            /* Configure I2C interface */
            i2c_CTRL_REG     = i2c_GET_CTRL_ADDR_ACCEPT(config->acceptAddr) |
                                            i2c_GET_CTRL_EC_AM_MODE (config->enableWake);

            i2c_I2C_CTRL_REG = i2c_GET_I2C_CTRL_HIGH_PHASE_OVS(config->oversampleHigh) |
                                            i2c_GET_I2C_CTRL_LOW_PHASE_OVS (config->oversampleLow)  |
                                            i2c_GET_I2C_CTRL_SL_MSTR_MODE  (config->mode)           |
                                            i2c_I2C_CTRL;

            /* Adjust SDA filter settigns */
            i2c_SET_I2C_CFG_SDA_FILT_TRIM(i2c_EC_AM_I2C_CFG_SDA_FILT_TRIM);


            /* Configure RX direction */
            i2c_RX_CTRL_REG      = i2c_GET_RX_CTRL_MEDIAN(config->enableMedianFilter) |
                                                i2c_I2C_RX_CTRL;
            i2c_RX_FIFO_CTRL_REG = i2c_CLEAR_REG;

            /* Set default address and mask */
            i2c_RX_MATCH_REG    = ((i2c_I2C_SLAVE) ?
                                                (i2c_GET_I2C_8BIT_ADDRESS(config->slaveAddr) |
                                                 i2c_GET_RX_MATCH_MASK(config->slaveAddrMask)) :
                                                (i2c_CLEAR_REG));


            /* Configure TX direction */
            i2c_TX_CTRL_REG      = i2c_I2C_TX_CTRL;
            i2c_TX_FIFO_CTRL_REG = i2c_CLEAR_REG;

            /* Configure interrupt with I2C handler */
            i2c_SCB_IRQ_Disable();
            i2c_SCB_IRQ_SetVector(&i2c_I2C_ISR);
            i2c_SCB_IRQ_SetPriority((uint8)i2c_SCB_IRQ_INTC_PRIOR_NUMBER);

            
            /* Configure interrupt sources */
            i2c_INTR_I2C_EC_MASK_REG = i2c_NO_INTR_SOURCES;
            i2c_INTR_SPI_EC_MASK_REG = i2c_NO_INTR_SOURCES;
            i2c_INTR_RX_MASK_REG     = i2c_NO_INTR_SOURCES;
            i2c_INTR_TX_MASK_REG     = i2c_NO_INTR_SOURCES;

            i2c_INTR_SLAVE_MASK_REG  = ((i2c_I2C_SLAVE) ?
                                                     (i2c_I2C_INTR_SLAVE_MASK) :
                                                     (i2c_CLEAR_REG));

            i2c_INTR_MASTER_MASK_REG = ((i2c_I2C_MASTER) ?
                                                     (i2c_I2C_INTR_MASTER_MASK) :
                                                     (i2c_CLEAR_REG));

            /* Configure global variables */
            i2c_state = i2c_I2C_FSM_IDLE;

            /* Internal slave variable */
            i2c_slStatus        = 0u;
            i2c_slRdBufIndex    = 0u;
            i2c_slWrBufIndex    = 0u;
            i2c_slOverFlowCount = 0u;

            /* Internal master variable */
            i2c_mstrStatus     = 0u;
            i2c_mstrRdBufIndex = 0u;
            i2c_mstrWrBufIndex = 0u;
        }
    }

#else

    /*******************************************************************************
    * Function Name: i2c_I2CInit
    ********************************************************************************
    *
    * Summary:
    *  Configures the SCB for I2C operation.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void i2c_I2CInit(void)
    {
        /* Configure I2C interface */
        i2c_CTRL_REG     = i2c_I2C_DEFAULT_CTRL;
        i2c_I2C_CTRL_REG = i2c_I2C_DEFAULT_I2C_CTRL;

        /* Adjust SDA filter settigns */
        i2c_SET_I2C_CFG_SDA_FILT_TRIM(i2c_EC_AM_I2C_CFG_SDA_FILT_TRIM);

        /* Configure RX direction */
        i2c_RX_CTRL_REG      = i2c_I2C_DEFAULT_RX_CTRL;
        i2c_RX_FIFO_CTRL_REG = i2c_I2C_DEFAULT_RX_FIFO_CTRL;

        /* Set default address and mask */
        i2c_RX_MATCH_REG     = i2c_I2C_DEFAULT_RX_MATCH;

        /* Configure TX direction */
        i2c_TX_CTRL_REG      = i2c_I2C_DEFAULT_TX_CTRL;
        i2c_TX_FIFO_CTRL_REG = i2c_I2C_DEFAULT_TX_FIFO_CTRL;

        /* Configure interrupt with I2C handler */
        i2c_SCB_IRQ_Disable();
        i2c_SCB_IRQ_SetVector(&i2c_I2C_ISR);
        i2c_SCB_IRQ_SetPriority((uint8)i2c_SCB_IRQ_INTC_PRIOR_NUMBER);
        
        /* Configure interrupt sources */
        i2c_INTR_I2C_EC_MASK_REG = i2c_I2C_DEFAULT_INTR_I2C_EC_MASK;
        i2c_INTR_SPI_EC_MASK_REG = i2c_I2C_DEFAULT_INTR_SPI_EC_MASK;
        i2c_INTR_SLAVE_MASK_REG  = i2c_I2C_DEFAULT_INTR_SLAVE_MASK;
        i2c_INTR_MASTER_MASK_REG = i2c_I2C_DEFAULT_INTR_MASTER_MASK;
        i2c_INTR_RX_MASK_REG     = i2c_I2C_DEFAULT_INTR_RX_MASK;
        i2c_INTR_TX_MASK_REG     = i2c_I2C_DEFAULT_INTR_TX_MASK;

        /* Configure global variables */
        i2c_state = i2c_I2C_FSM_IDLE;

        #if(i2c_I2C_SLAVE)
            /* Internal slave variable */
            i2c_slStatus        = 0u;
            i2c_slRdBufIndex    = 0u;
            i2c_slWrBufIndex    = 0u;
            i2c_slOverFlowCount = 0u;
        #endif /* (i2c_I2C_SLAVE) */

        #if(i2c_I2C_MASTER)
        /* Internal master variable */
            i2c_mstrStatus     = 0u;
            i2c_mstrRdBufIndex = 0u;
            i2c_mstrWrBufIndex = 0u;
        #endif /* (i2c_I2C_MASTER) */
    }
#endif /* (i2c_SCB_MODE_UNCONFIG_CONST_CFG) */


/*******************************************************************************
* Function Name: i2c_I2CStop
********************************************************************************
*
* Summary:
*  Initializes I2C registers with initial values provided from customizer.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*
*
*******************************************************************************/
void i2c_I2CStop(void)
{
    i2c_state = i2c_I2C_FSM_IDLE;
}


#if(i2c_I2C_WAKE_ENABLE_CONST)
    /*******************************************************************************
    * Function Name: i2c_I2CSaveConfig
    ********************************************************************************
    *
    * Summary:
    *  Wakeup disabled: does nothing.
    *  Wakeup  enabled: clears SCB_backup.enableState and enables
    *  SCB_INTR_I2C_EC_WAKE_UP interrupt used for wakeup. This interrupt source
    *  tracks in active mode therefore it does not require to be cleared.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void i2c_I2CSaveConfig(void)
    {
        /* Enable interrupt to wakeup the device */
        i2c_SetI2CExtClkInterruptMode(i2c_INTR_I2C_EC_WAKE_UP);
    }


    /*******************************************************************************
    * Function Name: i2c_I2CRestoreConfig
    ********************************************************************************
    *
    * Summary:
    *  Does nothing.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void i2c_I2CRestoreConfig(void)
    {
        /* Does nothing: as wake is masked-off in the interrupt */
    }
#endif /* (i2c_I2C_WAKE_ENABLE_CONST) */


/* [] END OF FILE */
