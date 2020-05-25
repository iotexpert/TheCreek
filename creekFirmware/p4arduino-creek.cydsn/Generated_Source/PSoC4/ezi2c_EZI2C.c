/***************************************************************************//**
* \file ezi2c_EZI2C.c
* \version 4.0
*
* \brief
*  This file provides the source code to the API for the SCB Component in
*  EZI2C mode.
*
* Note:
*
*******************************************************************************
* \copyright
* Copyright 2013-2017, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "ezi2c_PVT.h"
#include "ezi2c_EZI2C_PVT.h"


/***************************************
*      EZI2C Private Vars
***************************************/

volatile uint8 ezi2c_curStatus; /* Status byte */
uint8 ezi2c_fsmState;           /* FSM state   */

/* Variables intended to be used with Buffer 1: Primary slave address */
volatile uint8 * ezi2c_dataBuffer1; /* Pointer to data buffer 1 */
uint16 ezi2c_bufSizeBuf1;           /* Size of buffer 1 in bytes      */
uint16 ezi2c_protectBuf1;           /* Start index of write protected area buffer 1 */
uint8 ezi2c_offsetBuf1; /* Current offset within buffer 1 */
uint16 ezi2c_indexBuf1;             /* Current index within buffer 1  */

#if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
    uint8 ezi2c_addrBuf1; /* Primary slave address. Used for software comparison   */
    uint8 ezi2c_addrBuf2; /* Secondary slave address. Used for software comparison */

    /* Variables intended to be used with Buffer 2: Primary slave address */
    volatile uint8 * ezi2c_dataBuffer2; /* Pointer to data buffer 2 */
    uint16 ezi2c_bufSizeBuf2;           /* Size of buffer 2 in bytes */
    uint16 ezi2c_protectBuf2;           /* Start index of write protected area buffer 2 */
    uint8 ezi2c_offsetBuf2; /* Current offset within buffer 2 */
    uint16 ezi2c_indexBuf2;             /* Current index within buffer 2 */
#endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */


/***************************************
*      EZI2C Private Functions
***************************************/

#if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
    static uint32 ezi2c_EzI2CUpdateRxMatchReg(uint32 addr1, uint32 addr2);
#endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */

#if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)

    /***************************************
    *  Configuration Structure Initialization
    ***************************************/

    /* Constant configuration of EZI2C */
    const ezi2c_EZI2C_INIT_STRUCT ezi2c_configEzI2C =
    {
        ezi2c_EZI2C_CLOCK_STRETCHING,
        ezi2c_EZI2C_MEDIAN_FILTER_ENABLE,
        ezi2c_EZI2C_NUMBER_OF_ADDRESSES,
        ezi2c_EZI2C_PRIMARY_SLAVE_ADDRESS,
        ezi2c_EZI2C_SECONDARY_SLAVE_ADDRESS,
        ezi2c_EZI2C_SUB_ADDRESS_SIZE,
        ezi2c_EZI2C_WAKE_ENABLE,
        ezi2c_EZI2C_BYTE_MODE_ENABLE
    };

    /*******************************************************************************
    * Function Name: ezi2c_EzI2CInit
    ****************************************************************************//**
    *
    *  Configures the ezi2c for EZI2C operation.
    *
    *  This function is intended specifically to be used when the ezi2c 
    *  configuration is set to “Unconfigured ezi2c” in the customizer. 
    *  After initializing the ezi2c in EZI2C mode using this function, 
    *  the component can be enabled using the ezi2c_Start() or 
    * ezi2c_Enable() function.
    *  This function uses a pointer to a structure that provides the configuration 
    *  settings. This structure contains the same information that would otherwise 
    *  be provided by the customizer settings.
    *
    * \param config: pointer to a structure that contains the following list of 
    *  fields. These fields match the selections available in the customizer. 
    *  Refer to the customizer for further description of the settings.
    *
    *******************************************************************************/
    void ezi2c_EzI2CInit(const ezi2c_EZI2C_INIT_STRUCT *config)
    {
        if(NULL == config)
        {
            CYASSERT(0u != 0u); /* Halt execution due to bad function parameter */
        }
        else
        {
            /* Configure pins */
            ezi2c_SetPins(ezi2c_SCB_MODE_EZI2C, ezi2c_DUMMY_PARAM,
                                                                      ezi2c_DUMMY_PARAM);

            /* Store internal configuration */
            ezi2c_scbMode       = (uint8) ezi2c_SCB_MODE_EZI2C;
            ezi2c_scbEnableWake = (uint8) config->enableWake;
            ezi2c_scbEnableIntr = (uint8) ezi2c_SCB_IRQ_INTERNAL;

            ezi2c_numberOfAddr  = (uint8) config->numberOfAddresses;
            ezi2c_subAddrSize   = (uint8) config->subAddrSize;

        #if (ezi2c_CY_SCBIP_V0)
            /* Adjust SDA filter settings. Ticket ID#150521 */
            ezi2c_SET_I2C_CFG_SDA_FILT_TRIM(ezi2c_EC_AM_I2C_CFG_SDA_FILT_TRIM);
        #endif /* (ezi2c_CY_SCBIP_V0) */

            /* Adjust AF and DF filter settings, AF = 1, DF = 0. Ticket ID#176179 */
            ezi2c_I2C_CFG_ANALOG_FITER_ENABLE;

            /* Configure I2C interface */
            ezi2c_CTRL_REG     = ezi2c_GET_CTRL_BYTE_MODE  (config->enableByteMode)    |
                                            ezi2c_GET_CTRL_ADDR_ACCEPT(config->numberOfAddresses) |
                                            ezi2c_GET_CTRL_EC_AM_MODE (config->enableWake);

            ezi2c_I2C_CTRL_REG = ezi2c_EZI2C_CTRL;

            /* Configure RX direction */
            ezi2c_RX_CTRL_REG = ezi2c_EZI2C_RX_CTRL |
                                        ezi2c_GET_RX_CTRL_MEDIAN(ezi2c_DIGITAL_FILTER_DISABLE);
                                                ;
            ezi2c_RX_FIFO_CTRL_REG = ezi2c_CLEAR_REG;

            /* Set default address and mask */
            if(ezi2c_EZI2C_PRIMARY_ADDRESS == config->numberOfAddresses)
            {
                ezi2c_RX_MATCH_REG = ezi2c_EzI2CUpdateRxMatchReg(config->primarySlaveAddr,
                                                                                       config->primarySlaveAddr);
            }
            else
            {
                ezi2c_RX_MATCH_REG = ezi2c_EzI2CUpdateRxMatchReg(config->primarySlaveAddr,
                                                                                       config->secondarySlaveAddr);
            }

            /* Configure TX direction */
            ezi2c_TX_CTRL_REG      = ezi2c_EZI2C_TX_CTRL;
            ezi2c_TX_FIFO_CTRL_REG = ((0u != (config->enableClockStretch)) ?
                                                 ezi2c_CLEAR_REG : ezi2c_EZI2C_TX_FIFO_CTRL);

            /* Configure interrupt sources */
        #if (!ezi2c_CY_SCBIP_V1)
           ezi2c_INTR_SPI_EC_MASK_REG = ezi2c_NO_INTR_SOURCES;
        #endif /* (!ezi2c_CY_SCBIP_V1) */

            ezi2c_INTR_I2C_EC_MASK_REG = ezi2c_NO_INTR_SOURCES;
            ezi2c_INTR_MASTER_MASK_REG = ezi2c_NO_INTR_SOURCES;
            ezi2c_INTR_SLAVE_MASK_REG  = ezi2c_EZI2C_INTR_SLAVE_MASK;
            ezi2c_INTR_TX_MASK_REG     = ezi2c_NO_INTR_SOURCES;

            /* Configure interrupt with EZI2C handler but do not enable it */
            CyIntDisable    (ezi2c_ISR_NUMBER);
            CyIntSetPriority(ezi2c_ISR_NUMBER, ezi2c_ISR_PRIORITY);
            (void) CyIntSetVector(ezi2c_ISR_NUMBER, (0u != (config->enableClockStretch)) ?
                                                                      (&ezi2c_EZI2C_STRETCH_ISR) :
                                                                      (&ezi2c_EZI2C_NO_STRETCH_ISR));

            if(0u != (config->enableClockStretch))
            {
                /* Configure interrupt sources */
                ezi2c_INTR_SLAVE_MASK_REG |= ezi2c_INTR_SLAVE_I2C_ADDR_MATCH;
                ezi2c_INTR_RX_MASK_REG     = ezi2c_NO_INTR_SOURCES;
            }
            else
            {
                /* Enable Auto ACK/NACK features */
                ezi2c_I2C_CTRL_REG |= ezi2c_EZI2C_CTRL_AUTO;

                /* Configure RX interrupt source */
                ezi2c_INTR_SLAVE_MASK_REG |= ezi2c_INTR_SLAVE_I2C_START;
                ezi2c_INTR_RX_MASK_REG     = ezi2c_INTR_RX_NOT_EMPTY;
            }

            /* Configure global variables */
            ezi2c_fsmState = ezi2c_EZI2C_FSM_IDLE;

            ezi2c_curStatus  = 0u;
            ezi2c_indexBuf1  = 0u;
            ezi2c_offsetBuf1 = 0u;
            ezi2c_indexBuf2  = 0u;
            ezi2c_offsetBuf2 = 0u;

            ezi2c_addrBuf1 = (uint8) config->primarySlaveAddr;
            ezi2c_addrBuf2 = (uint8) config->secondarySlaveAddr;
        }
    }

#else

    /*******************************************************************************
    * Function Name: ezi2c_EzI2CInit
    ****************************************************************************//**
    *
    *  Configures the SCB for the EZI2C operation.
    *
    *******************************************************************************/
    void ezi2c_EzI2CInit(void)
    {
        /* Configure I2C interface */
        ezi2c_CTRL_REG     = ezi2c_EZI2C_DEFAULT_CTRL;
        ezi2c_I2C_CTRL_REG = ezi2c_EZI2C_DEFAULT_I2C_CTRL;

    #if (ezi2c_CY_SCBIP_V0)
        /* Adjust SDA filter settings. Ticket ID#150521 */
        ezi2c_SET_I2C_CFG_SDA_FILT_TRIM(ezi2c_EC_AM_I2C_CFG_SDA_FILT_TRIM);
    #endif /* (ezi2c_CY_SCBIP_V0) */

        /* Configure RX direction */
        ezi2c_RX_CTRL_REG      = ezi2c_EZI2C_DEFAULT_RX_CTRL;
        ezi2c_RX_FIFO_CTRL_REG = ezi2c_EZI2C_DEFAULT_RX_FIFO_CTRL;

        /* Set default address and mask */
        ezi2c_RX_MATCH_REG     = ezi2c_EZI2C_DEFAULT_RX_MATCH;

        /* Configure TX direction */
        ezi2c_TX_CTRL_REG      = ezi2c_EZI2C_DEFAULT_TX_CTRL;
        ezi2c_TX_FIFO_CTRL_REG = ezi2c_EZI2C_DEFAULT_TX_FIFO_CTRL;

        /* Configure interrupt with EZI2C handler but do not enable it */
    #if !defined (CY_EXTERNAL_INTERRUPT_CONFIG)
        CyIntDisable    (ezi2c_ISR_NUMBER);
        CyIntSetPriority(ezi2c_ISR_NUMBER, ezi2c_ISR_PRIORITY);

    #if (ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST)
        (void) CyIntSetVector(ezi2c_ISR_NUMBER, &ezi2c_EZI2C_STRETCH_ISR);
    #else
        (void) CyIntSetVector(ezi2c_ISR_NUMBER, &ezi2c_EZI2C_NO_STRETCH_ISR);
    #endif /* (ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST) */

    #endif /* !defined (CY_EXTERNAL_INTERRUPT_CONFIG) */

        /* Configure interrupt sources */
    #if (!ezi2c_CY_SCBIP_V1)
        ezi2c_INTR_SPI_EC_MASK_REG = ezi2c_EZI2C_DEFAULT_INTR_SPI_EC_MASK;
    #endif /* (!ezi2c_CY_SCBIP_V1) */

        ezi2c_INTR_I2C_EC_MASK_REG = ezi2c_EZI2C_DEFAULT_INTR_I2C_EC_MASK;
        ezi2c_INTR_SLAVE_MASK_REG  = ezi2c_EZI2C_DEFAULT_INTR_SLAVE_MASK;
        ezi2c_INTR_MASTER_MASK_REG = ezi2c_EZI2C_DEFAULT_INTR_MASTER_MASK;
        ezi2c_INTR_RX_MASK_REG     = ezi2c_EZI2C_DEFAULT_INTR_RX_MASK;
        ezi2c_INTR_TX_MASK_REG     = ezi2c_EZI2C_DEFAULT_INTR_TX_MASK;

        /* Configure global variables */
        ezi2c_fsmState = ezi2c_EZI2C_FSM_IDLE;

        ezi2c_curStatus  = 0u;
        ezi2c_indexBuf1  = 0u;
        ezi2c_offsetBuf1 = 0u;

    #if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
        ezi2c_indexBuf2  = 0u;
        ezi2c_offsetBuf2 = 0u;

        ezi2c_addrBuf1 = ezi2c_EZI2C_PRIMARY_SLAVE_ADDRESS;
        ezi2c_addrBuf2 = ezi2c_EZI2C_SECONDARY_SLAVE_ADDRESS;
    #endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */
    }
#endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */


/*******************************************************************************
* Function Name: ezi2c_EzI2CStop
****************************************************************************//**
*
*  Resets the EZI2C FSM into the default state.
*
*******************************************************************************/
void ezi2c_EzI2CStop(void)
{
    ezi2c_fsmState = ezi2c_EZI2C_FSM_IDLE;
}


/*******************************************************************************
* Function Name: ezi2c_EzI2CGetActivity
****************************************************************************//**
*
*  Returns the EZI2C slave status.
*  The read, write and error status flags reset to zero after this function
*  call. The busy status flag is cleared when the transaction intended for
*  the EZI2C slave completes.
*
* \return
*  Returns the status of the EZI2C Slave activity.
*  - ezi2c_EZI2C_STATUS_READ1 - Read transfer complete. The transfer 
*    used the primary slave address. The error condition status bit must be 
*    checked to ensure that read transfer was completed successfully.
*  - ezi2c_EZI2C_STATUS_WRITE1 - Write transfer complete. The buffer 
*    content was modified. The transfer used the primary slave address. 
*    The error condition status bit must be checked to ensure that write 
*    transfer was completed successfully.
*  - ezi2c_EZI2C_STATUS_READ2 - Read transfer complete. The transfer
*    used the secondary slave address. The error condition status bit must be 
*    checked to ensure that read transfer was completed successfully.
*  - ezi2c_EZI2C_STATUS_WRITE2 - Write transfer complete. The buffer
*    content was modified. The transfer used the secondary slave address. 
*    The error condition status bit must be checked to ensure that write 
*    transfer was completed successfully.
*  - ezi2c_EZI2C_STATUS_BUSY - A transfer intended for the primary 
*    or secondary address is in progress. The status bit is set after an 
*    address match and cleared on a Stop or ReStart condition.
*  - ezi2c_EZI2C_STATUS_ERR - An error occurred during a transfer 
*    intended for the primary or secondary slave address. The sources of error
*    are: misplaced Start or Stop condition or lost arbitration while slave 
*    drives SDA.
*    The write buffer may contain invalid byte or part of the transaction when 
*    ezi2c_EZI2C_STATUS_ERR and ezi2c_EZI2C_STATUS_WRITE1/2 
*    is set. It is recommended to discard buffer content in this case.
*
* \globalvars
*  ezi2c_curStatus - used to store the current status of the EZI2C 
*  slave.
*
*******************************************************************************/
uint32 ezi2c_EzI2CGetActivity(void)
{
    uint32 status;

    ezi2c_DisableInt();  /* Lock from interruption */

    status = ezi2c_curStatus;

    /* Relay on address match event from HW as bus busy status */
    #if(ezi2c_EZI2C_SCL_STRETCH_DISABLE)
    {
        /* For ezi2c_CY_SCBIP_V0 the wake is prohibited by customizer */
        #if(ezi2c_EZI2C_EC_AM_ENABLE)
        {
            status |= ezi2c_CHECK_INTR_I2C_EC(ezi2c_INTR_I2C_EC_WAKE_UP) ?
                        ezi2c_EZI2C_STATUS_BUSY : 0u;
        }
        #else
        {
            status |= ezi2c_CHECK_INTR_SLAVE(ezi2c_INTR_SLAVE_I2C_ADDR_MATCH) ?
                        ezi2c_EZI2C_STATUS_BUSY : 0u;
        }
        #endif
    }
    #endif

    ezi2c_curStatus &= ((uint8) ~ezi2c_EZI2C_CLEAR_STATUS);

    ezi2c_EnableInt();   /* Release lock */

    return(status);
}


/*******************************************************************************
* Function Name: ezi2c_EzI2CSetAddress1
****************************************************************************//**
*
*  Sets the primary EZI2C slave address.
*
* \param address: I2C slave address for the secondary device.
*  This address is the 7-bit right-justified slave address and does not 
*   include the R/W bit.
*
*  The address value is not checked to see if it violates the I2C spec. 
*  The preferred addresses are in the range between 8 and 120 (0x08 to 0x78).
*
* \globalvars
*  ezi2c_addrBuf1 - used to store the primary 7-bit slave address 
*  value.
*
*******************************************************************************/
void ezi2c_EzI2CSetAddress1(uint32 address)
{
    #if(ezi2c_SECONDARY_ADDRESS_ENABLE)
    {
        ezi2c_addrBuf1 = (uint8) address;

        ezi2c_RX_MATCH_REG = 
                        ezi2c_EzI2CUpdateRxMatchReg(address, (uint32) ezi2c_addrBuf2);
    }
    #else
    {
        uint32 matchReg;

        matchReg = ezi2c_RX_MATCH_REG;

        /* Set address. */
        matchReg &= (uint32) ~ezi2c_RX_MATCH_ADDR_MASK;
        matchReg |= (uint32)  ezi2c_GET_I2C_8BIT_ADDRESS(address);

        ezi2c_RX_MATCH_REG = matchReg;
    }
    #endif
}


/*******************************************************************************
* Function Name: ezi2c_EzI2CGetAddress1
****************************************************************************//**
*
*  Returns primary the EZ I2C slave address.
*  This address is the 7-bit right-justified slave address and does not include 
*  the R/W bit.
*
* \return
*  Primary EZI2C slave address.
*
* \globalvars
*  ezi2c_addrBuf1 - used to store the primary 7-bit slave address 
*  value.
*
*******************************************************************************/
uint32 ezi2c_EzI2CGetAddress1(void)
{
    uint32 address;

    #if(ezi2c_SECONDARY_ADDRESS_ENABLE)
    {
        address = (uint32) ezi2c_addrBuf1;
    }
    #else
    {
        address = (ezi2c_GET_RX_MATCH_ADDR(ezi2c_RX_MATCH_REG) >>
                   ezi2c_I2C_SLAVE_ADDR_POS);
    }
    #endif

    return(address);
}


/*******************************************************************************
* Function Name: ezi2c_EzI2CSetBuffer1
****************************************************************************//**
*
*  Sets up the data buffer to be exposed to the I2C master on a primary slave
*  address request.
*
* \param bufSize: Size of the buffer in bytes.
* \param rwBoundary: Number of data bytes starting from the beginning of the 
*  buffer with read and write access. Data bytes located at offset rwBoundary 
*  or greater are read only.
*  This value must be less than or equal to the buffer size.
* \param buffer: Pointer to the data buffer.
*
* \sideeffect
*  Calling this function in the middle of a transaction intended for the 
*  primary slave address leads to unexpected behavior.
*
* \globalvars
*  ezi2c_dataBuffer1 – the pointer to the buffer to be exposed to the
*  master on a primary address.
*  ezi2c_bufSizeBuf1 - the size of the buffer to be exposed to the 
*  master on a primary address.
*  ezi2c_protectBuf1 - the start index of the read-only region in the
*  buffer to be exposed to the master on a primary address. The read-only region
*  continues up to the end the buffer.
*
*******************************************************************************/
void ezi2c_EzI2CSetBuffer1(uint32 bufSize, uint32 rwBoundary, volatile uint8 * buffer)
{
    if (NULL != buffer)
    {
        ezi2c_DisableInt();  /* Lock from interruption */

        ezi2c_dataBuffer1 =  buffer;
        ezi2c_bufSizeBuf1 = (uint16) bufSize;
        ezi2c_protectBuf1 = (uint16) rwBoundary;

        ezi2c_EnableInt();   /* Release lock */
    }
}


/*******************************************************************************
* Function Name: ezi2c_EzI2CSetReadBoundaryBuffer1
****************************************************************************//**
*
*  Sets the read only boundary in the data buffer to be exposed to the I2C
*  master on a primary slave address request.
*
* \param rwBoundry: Number of data bytes starting from the beginning of the 
*  buffer with read and write access. Data bytes located at offset rwBoundary 
*  or greater are read only.
*  This value must be less than or equal to the buffer size.
*
* \sideeffect
*  Calling this function in the middle of a transaction intended for the 
*  primary slave address leads to unexpected behavior.
*
* \globalvars
*  ezi2c_protectBuf1 - the start index of the read-only region in the
*  buffer to be exposed to the master on a primary address. The read-only region
*  continues up to the end the buffer.
*
*******************************************************************************/
void ezi2c_EzI2CSetReadBoundaryBuffer1(uint32 rwBoundary)
{
    ezi2c_protectBuf1 = (uint16) rwBoundary;
}


#if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
    /*******************************************************************************
    * Function Name: ezi2c_EzI2CUpdateRxMatchReg
    ****************************************************************************//**
    *
    *  Returns the value of the RX MATCH register for addr1 and addr2. The addr1 is
    *  accepted as the primary address and it is written to RX_MATCH.ADDRESS
    *  (addr1 << 0x01).
    *  The RX_MATCH.MASK is set as follow: addr1 and addr2 equal bits set to 1
    *  otherwise 0.
    *
    * \param addr1: I2C slave address for the primary device.
    * \param addr2: I2C slave address for the secondary device.
    *  This address is the 7-bit right-justified slave address and does
    *  not include the R/W bit.
    *
    * \return
    *  Value of RX MATCH register.
    *
    *******************************************************************************/
    static uint32 ezi2c_EzI2CUpdateRxMatchReg(uint32 addr1, uint32 addr2)
    {
        uint32 matchReg;

        matchReg  = ~(addr1 ^ addr2); /* If (addr1 and addr2) bit matches - mask bit equals 1, in other case 0 */

        matchReg  = (uint32) (ezi2c_GET_I2C_8BIT_ADDRESS(matchReg) << ezi2c_RX_MATCH_MASK_POS);
        matchReg |= ezi2c_GET_I2C_8BIT_ADDRESS(addr1);

        return(matchReg);
    }

    /*******************************************************************************
    * Function Name: ezi2c_EzI2CSetAddress2
    ****************************************************************************//**
    *
    *  Sets the secondary EZI2C slave address.
    *
    * \param address: secondary I2C slave address.
    *  This address is the 7-bit right-justified slave address and does not 
    *  include the R/W bit.
    *  The address value is not checked to see if it violates the I2C spec. 
    *  The preferred addresses are in the range between 8 and 120 (0x08 to 0x78).
    *
    * \globalvars
    *  ezi2c_addrBuf2 - used to store the secondary 7-bit slave address 
    *  value.
    *
    *******************************************************************************/
    void ezi2c_EzI2CSetAddress2(uint32 address)
    {
        ezi2c_addrBuf2 = (uint8) address;

        ezi2c_RX_MATCH_REG = 
                        ezi2c_EzI2CUpdateRxMatchReg((uint32) ezi2c_addrBuf1, address);
    }


    /*******************************************************************************
    * Function Name: ezi2c_EzI2CGetAddress2
    ****************************************************************************//**
    *
    *  Returns the secondary EZ I2C slave address.
    *  This address is the 7-bit right-justified slave address and does not include 
    *  the R/W bit.
    *
    * \return
    *  Secondary I2C slave address.
    *
    * \globalvars
    *  ezi2c_addrBuf2 - used to store the secondary 7-bit slave address 
    *  value.
    *
    *******************************************************************************/
    uint32 ezi2c_EzI2CGetAddress2(void)
    {
        return((uint32) ezi2c_addrBuf2);
    }


    /*******************************************************************************
    * Function Name: ezi2c_EzI2CSetBuffer2
    ****************************************************************************//**
    *
    *  Sets up the data buffer to be exposed to the I2C master on a secondary slave
    *  address request.
    *
    * \param bufSize: Size of the buffer in bytes.
    * \param rwBoundary: Number of data bytes starting from the beginning of the 
    *  buffer with read and write access. Data bytes located at offset rwBoundary 
    *  or greater are read only.
    *  This value must be less than or equal to the buffer size.
    * \param buffer: Pointer to the data buffer.
    *
    * \sideeffects
    *  Calling this function in the middle of a transaction intended for the 
    *  secondary slave address leads to unexpected behavior.
    *
    * \globalvars
    *  ezi2c_dataBuffer2 – the pointer to the buffer to be exposed to the
    *  master on a secondary address.
    *  ezi2c_bufSizeBuf2 - the size of the buffer to be exposed to the 
    *  master on a secondary address.
    *  ezi2c_protectBuf2 - the start index of the read-only region in the
    *  buffer to be exposed to the master on a secondary address. The read-only 
    *  region continues up to the end the buffer.
    *
    *******************************************************************************/
    void ezi2c_EzI2CSetBuffer2(uint32 bufSize, uint32 rwBoundary, volatile uint8 * buffer)
    {
        if (NULL != buffer)
        {
            ezi2c_DisableInt();  /* Lock from interruption */

            ezi2c_dataBuffer2 =  buffer;
            ezi2c_bufSizeBuf2 = (uint16) bufSize;
            ezi2c_protectBuf2 = (uint16) rwBoundary;

            ezi2c_EnableInt();   /* Release lock */
        }
    }


    /*******************************************************************************
    * Function Name: ezi2c_EzI2CSetReadBoundaryBuffer2
    ****************************************************************************//**
    *
    *  Sets the read only boundary in the data buffer to be exposed to the I2C
    *  master on a secondary address request.
    *
    *  \param rwBoundary: Number of data bytes starting from the beginning of the
    *   buffer with read and write access. Data bytes located at offset rwBoundary 
    *   or greater are read only.
    *   This value must be less than or equal to the buffer size.
    *
    *  \sideeffect
    *   Calling this function in the middle of a transaction intended for the 
    *   secondary slave address leads to unexpected behavior.
    *
    * \globalvars
    *  ezi2c_protectBuf2 - the start index of the read-only region in the
    *  buffer to be exposed to the master on a secondary address. The read-only 
    *  region continues up to the end the buffe
    *
    *******************************************************************************/
    void ezi2c_EzI2CSetReadBoundaryBuffer2(uint32 rwBoundary)
    {
        ezi2c_protectBuf2 = (uint16) rwBoundary;
    }

#endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */


#if(ezi2c_EZI2C_WAKE_ENABLE_CONST)
    /*******************************************************************************
    * Function Name: ezi2c_EzI2CSaveConfig
    ****************************************************************************//**
    *
    *  Clock stretching is  enabled: Enables INTR_I2C_EC.WAKE_UP interrupt source.
    *  It triggers on the slave address match.
    *  Clock stretching is disabled: Waits until the I2C slave becomes free and
    *  disables the block to perform reconfiguration from the active mode operation
    *  to deep sleep with wake up on the address match: enables INTR_I2C_EC.WAKE_UP
    *  interrupt source and disables the INTR_S and INTR_TX interrupt sources.
    *  The block is disabled before reconfiguration and enabled when
    *  it is completed.
    *
    *******************************************************************************/
    void ezi2c_EzI2CSaveConfig(void)
    {
    #if(ezi2c_CY_SCBIP_V0)

        #if(ezi2c_EZI2C_SCL_STRETCH_ENABLE)
        {
            /* Enable wakeup interrupt source on address match */
            ezi2c_SetI2CExtClkInterruptMode(ezi2c_INTR_I2C_EC_WAKE_UP);
        }
        #endif

    #else
        uint8 enableInt;

        enableInt = (uint8) ezi2c_INTR_I2C_EC_WAKE_UP;

        #if(ezi2c_EZI2C_SCL_STRETCH_ENABLE)
        {
        #if (ezi2c_SCB_CLK_INTERNAL)
            /* Disable clock to internal address match logic. Ticket ID #187931 */
            ezi2c_SCBCLK_Stop();
        #endif /* (ezi2c_SCB_CLK_INTERNAL) */

            /* Enable interrupt source to wakeup device */
            ezi2c_SetI2CExtClkInterruptMode(enableInt);
        }
        #else
        {
            for(;;) /* Wait for end of transaction intended to slave */
            {
                if(0u == (ezi2c_GetI2CExtClkInterruptSource() & ezi2c_INTR_I2C_EC_WAKE_UP))
                {
                    enableInt = CyEnterCriticalSection();

                    if(0u == (ezi2c_GetI2CExtClkInterruptSource() & ezi2c_INTR_I2C_EC_WAKE_UP))
                    {
                        /* Attempts to set NACK command before disable block */
                        ezi2c_I2C_SLAVE_GENERATE_NACK;

                        if(0u == (ezi2c_GetI2CExtClkInterruptSource() & ezi2c_INTR_I2C_EC_WAKE_UP))
                        {
                            /* NACK command was set before. It is safe to disable block */
                            ezi2c_CTRL_REG &= (uint32) ~ezi2c_CTRL_ENABLED;
                            ezi2c_DisableInt();

                            CyExitCriticalSection(enableInt);
                            break;
                        }
                        else
                        {
                            /* Clear NACK command to prevent data NACK */
                            ezi2c_I2C_SLAVE_CLEAR_NACK;
                        }
                    }

                    CyExitCriticalSection(enableInt);
                }
            }

            /* Disable all active mode interrupt sources */
            ezi2c_SetTxInterruptMode(ezi2c_NO_INTR_SOURCES);
            ezi2c_SetSlaveInterruptMode(ezi2c_NO_INTR_SOURCES);
            ezi2c_ClearPendingInt();
            ezi2c_EnableInt();

            /* Enable wakeup interrupt on address match */
            ezi2c_SetI2CExtClkInterruptMode(ezi2c_INTR_I2C_EC_WAKE_UP);

            enableInt = CyEnterCriticalSection();

            ezi2c_CTRL_REG |= (uint32) ezi2c_CTRL_ENABLED;
            ezi2c_I2C_SLAVE_GENERATE_NACK;

            CyExitCriticalSection(enableInt);
        }
        #endif
    #endif /* (ezi2c_CY_SCBIP_V0) */
    }


    /*******************************************************************************
    * Function Name: ezi2c_EzI2CRestoreConfig
    ****************************************************************************//**
    *
    *  Clock stretching is  enabled: Disables the INTR_I2C_EC.WAKE_UP interrupt
    *  source.
    *  Clock stretching is disabled: Reconfigures the EZI2C component from
    *  Deep Sleep (wake up on the address match) to active operation: disables
    *  the INTR_I2C_EC.WAKE_UP interrupt source and restores the INTR_S
    *  interrupt sources to operate in the active mode.
    *  The block is disabled before reconfiguration and enabled when
    *  it is completed.
    *
    *******************************************************************************/
    void ezi2c_EzI2CRestoreConfig(void)
    {
    #if(ezi2c_CY_SCBIP_V0)

        #if(ezi2c_EZI2C_SCL_STRETCH_ENABLE)
        {
            /* Disable wakeup interrupt on address match */
            ezi2c_SetI2CExtClkInterruptMode(ezi2c_NO_INTR_SOURCES);
        }
        #endif

    #else

        #if(ezi2c_EZI2C_SCL_STRETCH_ENABLE)
        {
            /* Disable wakeup interrupt source on address match */
            ezi2c_SetI2CExtClkInterruptMode(ezi2c_NO_INTR_SOURCES);

        #if (ezi2c_SCB_CLK_INTERNAL)
            /* Enable clock to internal address match logic. Ticket ID #187931 */
            ezi2c_SCBCLK_Start();
        #endif /* (ezi2c_SCB_CLK_INTERNAL) */
        }
        #else
        {
            /* NACK will be driven on the bus by wakeup or NACK command.
            * It is safe to disable block to restore active mode configuration.
            */
            ezi2c_CTRL_REG &= (uint32) ~ezi2c_CTRL_ENABLED;

            /* Restore active mode interrupt sources */
            ezi2c_SetI2CExtClkInterruptMode(ezi2c_NO_INTR_SOURCES);
            ezi2c_SetSlaveInterruptMode(ezi2c_EZI2C_INTR_SLAVE_MASK |
                                                   ezi2c_INTR_SLAVE_I2C_START);
            ezi2c_ClearPendingInt();

            ezi2c_CTRL_REG |= (uint32) ezi2c_CTRL_ENABLED;
        }
        #endif

    #endif /* (ezi2c_CY_SCBIP_V0) */
    }
#endif /* (ezi2c_EZI2C_WAKE_ENABLE_CONST) */


/* [] END OF FILE */
