/*******************************************************************************
* File Name: ezi2c_EZI2C_INT.c
* Version 3.10
*
* Description:
*  This file provides the source code to the Interrupt Service Routine for
*  the SCB Component in EZI2C mode.
*
* Note:
*
********************************************************************************
* Copyright 2013-2015, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "ezi2c_PVT.h"
#include "ezi2c_EZI2C_PVT.h"


#if(ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST)
    /*******************************************************************************
    * Function Name: ezi2c_EZI2C_STRETCH_ISR
    ********************************************************************************
    *
    * Summary:
    *  Handles the Interrupt Service Routine for the SCB EZI2C mode. The clock stretching is
    *  used during operation.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    CY_ISR_PROTO(ezi2c_EZI2C_STRETCH_ISR)
    {
        static uint16 locBufSize;
        uint32 locIndex;
        uint32 locStatus;

        uint32 endTransfer;
        uint32 fifoIndex;
        uint32 locByte;

        uint32 locIntrCause;
        uint32 locIntrSlave;
        
    #ifdef ezi2c_EZI2C_STRETCH_ISR_ENTRY_CALLBACK
        ezi2c_EZI2C_STRETCH_ISR_EntryCallback();
    #endif /* ezi2c_EZI2C_STRETCH_ISR_ENTRY_CALLBACK */

    #if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
        /* Variable intended to be used with either buffer */
        static volatile uint8 * ezi2c_dataBuffer; /* Pointer to data buffer              */
        static uint16 ezi2c_bufSizeBuf;           /* Size of buffer in bytes             */
        static uint16 ezi2c_protectBuf;           /* Start index of write protected area */

        static uint8 activeAddress;
        uint32 ackResponse;

        ackResponse = ezi2c_EZI2C_ACK_RECEIVED_ADDRESS;
    #endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */

    #if !defined (CY_REMOVE_ezi2c_CUSTOM_INTR_HANDLER)
        if(NULL != ezi2c_customIntrHandler)
        {
            ezi2c_customIntrHandler();
        }
    #else
        CY_ezi2c_CUSTOM_INTR_HANDLER();
    #endif /* !defined (CY_REMOVE_ezi2c_CUSTOM_INTR_HANDLER) */

        /* Make local copy of global variable */
        locIndex = ezi2c_EZI2C_GET_INDEX(activeAddress);

        /* Get interrupt sources */
        locIntrSlave = ezi2c_GetSlaveInterruptSource();
        locIntrCause = ezi2c_GetInterruptCause();

        /* INTR_SLAVE.I2C_ARB_LOST and INTR_SLAVE_I2C.BUS_ERROR */
        /* Handles errors on the bus. There are cases when both bits are set.
        * The error recovery is common: re-enable the scb IP. The content of the RX FIFO is lost.
        */
        if(0u != (locIntrSlave & (ezi2c_INTR_SLAVE_I2C_ARB_LOST |
                                  ezi2c_INTR_SLAVE_I2C_BUS_ERROR)))
        {
            ezi2c_CTRL_REG &= (uint32) ~ezi2c_CTRL_ENABLED; /* Disable SCB block */

        #if(ezi2c_CY_SCBIP_V0)
            if(0u != ((uint8) ezi2c_EZI2C_STATUS_BUSY & ezi2c_curStatus))
        #endif /* (ezi2c_CY_SCBIP_V0) */
            {
                ezi2c_curStatus &= (uint8) ~ezi2c_EZI2C_STATUS_BUSY;
                ezi2c_curStatus |= (uint8)  ezi2c_EZI2C_STATUS_ERR;

                /* INTR_TX_EMPTY is enabled in the address phase to receive data */
                if(0u == (ezi2c_GetTxInterruptMode() & ezi2c_INTR_TX_EMPTY))
                {
                    /* Write complete */
                    if(ezi2c_indexBuf1 != ezi2c_offsetBuf1)
                    {
                        ezi2c_curStatus |= (uint8) ezi2c_INTR_SLAVE_I2C_WRITE_STOP;
                    }
                }
                else
                {
                    /* Read complete */
                    ezi2c_curStatus |= (uint8) ezi2c_INTR_SLAVE_I2C_NACK;
                }
            }

            ezi2c_DISABLE_SLAVE_AUTO_DATA;

            /* Disable TX and RX interrupt sources */
            ezi2c_SetRxInterruptMode(ezi2c_NO_INTR_SOURCES);
            ezi2c_SetTxInterruptMode(ezi2c_NO_INTR_SOURCES);

        #if(ezi2c_CY_SCBIP_V0)
            /* Clear interrupt sources as they are not automatically cleared after SCB is disabled */
            ezi2c_ClearRxInterruptSource(ezi2c_INTR_RX_ALL);
            ezi2c_ClearSlaveInterruptSource(ezi2c_INTR_SLAVE_ALL);
        #endif /* (ezi2c_CY_SCBIP_V0) */

            ezi2c_fsmState = ezi2c_EZI2C_FSM_IDLE;

            ezi2c_CTRL_REG |= (uint32) ezi2c_CTRL_ENABLED;  /* Enable SCB block */
        }
        else
        {
            /* INTR_I2C_EC_WAKE_UP */
            /* Wakes up device from deep sleep */
            if(0u != (locIntrCause & ezi2c_INTR_CAUSE_I2C_EC))
            {
                /* Disables wakeup interrupt source but does not clear it. It is cleared in INTR_SLAVE_I2C_ADDR_MATCH */
                ezi2c_SetI2CExtClkInterruptMode(ezi2c_NO_INTR_SOURCES);
            }

            if(0u != (locIntrCause & (ezi2c_INTR_CAUSE_RX | ezi2c_INTR_CAUSE_SLAVE)))
            {
                /* INTR_RX.NOT_EMPTY */
                /* Receives data byte-by-byte. Does not use RX FIFO capabilities */
                if (0u != (ezi2c_GetRxInterruptSourceMasked() & ezi2c_INTR_RX_NOT_EMPTY))
                {
                #if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
                    /* If I2C_STOP service is delayed to I2C_ADDR_MATCH the address byte is in the RX FIFO and
                    * RX_NOT_EMPTY is enabled. The address byte has to stay into RX FIFO therefore
                    * RX.NOT_EMPTY service has to be skipped. The address byte has to be read by I2C_ADDR_MATCH.
                    */
                    if (0u == (locIntrCause & ezi2c_INTR_CAUSE_SLAVE))
                #endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */
                    {
                        locByte = ezi2c_RX_FIFO_RD_REG;

                        switch(ezi2c_fsmState)
                        {
                        case ezi2c_EZI2C_FSM_BYTE_WRITE:
                            if(0u != locBufSize)
                            {
                                /* Store data byte and ACK */
                                ezi2c_I2C_SLAVE_GENERATE_ACK;

                                ezi2c_dataBuffer1[locIndex] = (uint8) locByte;
                                locIndex++;
                                locBufSize--;
                            }
                            else
                            {
                                /* Discard data byte and NACK */
                                ezi2c_I2C_SLAVE_GENERATE_NACK;
                            }
                            break;

                    #if(ezi2c_SUB_ADDRESS_SIZE16_CONST)
                        case ezi2c_EZI2C_FSM_OFFSET_HI8:

                            ezi2c_I2C_SLAVE_GENERATE_ACK;

                            /* Store offset most significant byre */
                            locBufSize = (uint16) ((uint8) locByte);

                            ezi2c_fsmState = ezi2c_EZI2C_FSM_OFFSET_LO8;

                            break;
                    #endif /* (ezi2c_SUB_ADDRESS_SIZE16_CONST) */

                        case ezi2c_EZI2C_FSM_OFFSET_LO8:

                            #if (ezi2c_SUB_ADDRESS_SIZE16)
                            {
                                /* Collect 2 bytes offset */
                                locByte = ((uint32) ((uint32) locBufSize << 8u)) | locByte;
                            }
                            #endif

                            /* Check offset against buffer size */
                            if(locByte < (uint32) ezi2c_bufSizeBuf1)
                            {
                                ezi2c_I2C_SLAVE_GENERATE_ACK;

                                /* Update local buffer index with new offset */
                                locIndex = locByte;

                                /* Get available buffer size to write */
                                locBufSize = (uint16) ((locByte < ezi2c_protectBuf1) ?
                                                       (ezi2c_protectBuf1 - locByte) : (0u));

                            #if(ezi2c_CY_SCBIP_V0)

                                if(locBufSize < ezi2c_EZI2C_FIFO_SIZE)
                                {
                                    /* Set FSM state to receive byte by byte */
                                    ezi2c_fsmState = ezi2c_EZI2C_FSM_BYTE_WRITE;
                                }
                                /* Receive RX FIFO chunks */
                                else if(locBufSize == ezi2c_EZI2C_FIFO_SIZE)
                                {
                                    ezi2c_ENABLE_SLAVE_AUTO_DATA; /* NACK when RX FIFO is full */
                                    ezi2c_SetRxInterruptMode(ezi2c_NO_INTR_SOURCES);
                                }
                                else
                                {
                                    ezi2c_ENABLE_SLAVE_AUTO_DATA_ACK; /* Stretch when RX FIFO is full */
                                    ezi2c_SetRxInterruptMode(ezi2c_INTR_RX_FULL);
                                }

                            #else

                                #if(ezi2c_SECONDARY_ADDRESS_ENABLE)
                                {
                                    /* Set FSM state to receive byte by byte.
                                    * The byte by byte receive is always chosen for two addresses. Ticket ID#175559.
                                    */
                                    ezi2c_fsmState = ezi2c_EZI2C_FSM_BYTE_WRITE;
                                }
                                #else
                                {
                                    if (locBufSize < ezi2c_EZI2C_FIFO_SIZE)
                                    {
                                        /* Set FSM state to receive byte by byte */
                                        ezi2c_fsmState = ezi2c_EZI2C_FSM_BYTE_WRITE;
                                    }
                                    /* Receive RX FIFO chunks */
                                    else if (locBufSize == ezi2c_EZI2C_FIFO_SIZE)
                                    {
                                        ezi2c_ENABLE_SLAVE_AUTO_DATA; /* NACK when RX FIFO is full */
                                        ezi2c_SetRxInterruptMode(ezi2c_NO_INTR_SOURCES);
                                    }
                                    else
                                    {
                                        ezi2c_ENABLE_SLAVE_AUTO_DATA_ACK; /* Stretch when RX FIFO is full */
                                        ezi2c_SetRxInterruptMode(ezi2c_INTR_RX_FULL);
                                    }
                                }
                                #endif

                            #endif /* (ezi2c_CY_SCBIP_V0) */

                                /* Store local offset into global variable */
                                ezi2c_EZI2C_SET_OFFSET(activeAddress, locIndex);
                            }
                            else
                            {
                                /* Discard offset byte and NACK */
                                ezi2c_I2C_SLAVE_GENERATE_NACK;
                            }
                            break;

                        default:
                            CYASSERT(0u != 0u); /* Should never get there */
                            break;
                        }

                        ezi2c_ClearRxInterruptSource(ezi2c_INTR_RX_NOT_EMPTY);
                    }
                }
                /* INTR_RX.FULL, INTR_SLAVE.I2C_STOP */
                /* Receive FIFO chunks: auto data ACK is enabled */
                else if (0u != (ezi2c_I2C_CTRL_REG & ezi2c_I2C_CTRL_S_READY_DATA_ACK))
                {
                    /* Slave interrupt (I2C_STOP or I2C_ADDR_MATCH) leads to completion of read.
                    * A completion event has a higher priority than the FIFO full.
                    * Read remaining data from RX FIFO.
                    */
                    if(0u != (locIntrCause & ezi2c_INTR_CAUSE_SLAVE))
                    {
                        /* Read remaining bytes from RX FIFO */
                        fifoIndex = ezi2c_GET_RX_FIFO_ENTRIES;

                        #if(ezi2c_SECONDARY_ADDRESS_ENABLE)
                        {
                            /* Update with current address match */
                            if(ezi2c_CHECK_INTR_SLAVE_MASKED(ezi2c_INTR_SLAVE_I2C_ADDR_MATCH))
                            {
                                /* Update RX FIFO entries as address byte is there now */
                                fifoIndex = ezi2c_GET_RX_FIFO_ENTRIES;

                                /* If SR is valid, RX FIFO is full and address is in SHIFTER:
                                * read 8 entries and leave address in RX FIFO for further processing.
                                * If SR is invalid, address is already in RX FIFO: read (entries-1).
                                */
                                fifoIndex -= ((0u != ezi2c_GET_RX_FIFO_SR_VALID) ? (0u) : (1u));
                            }
                        }
                        #endif

                        ezi2c_DISABLE_SLAVE_AUTO_DATA;
                        endTransfer = ezi2c_EZI2C_CONTINUE_TRANSFER;
                    }
                    else
                    /* INTR_RX_FULL */
                    /* Continue transfer or disable INTR_RX_FULL to catch completion event. */
                    {
                        /* Calculate buffer size available to write data into */
                        locBufSize -= (uint16) ezi2c_EZI2C_FIFO_SIZE;

                        if(locBufSize <= ezi2c_EZI2C_FIFO_SIZE)
                        {
                            /* Send NACK when RX FIFO overflow */
                            fifoIndex   = locBufSize;
                            endTransfer = ezi2c_EZI2C_COMPLETE_TRANSFER;
                        }
                        else
                        {
                            /* Continue  transaction */
                            fifoIndex   = ezi2c_EZI2C_FIFO_SIZE;
                            endTransfer = ezi2c_EZI2C_CONTINUE_TRANSFER;
                        }
                    }

                    for(; (0u != fifoIndex); fifoIndex--)
                    {
                        /* Store data in buffer */
                        ezi2c_dataBuffer1[locIndex] = (uint8) ezi2c_RX_FIFO_RD_REG;
                        locIndex++;
                    }

                    /* Complete transfer sending NACK when RX FIFO overflow */
                    if(ezi2c_EZI2C_COMPLETE_TRANSFER == endTransfer)
                    {
                        ezi2c_ENABLE_SLAVE_AUTO_DATA_NACK;

                        /* Disable INTR_RX_FULL during last RX FIFO chunk reception */
                        ezi2c_SetRxInterruptMode(ezi2c_NO_INTR_SOURCES);
                    }

                    ezi2c_ClearRxInterruptSource(ezi2c_INTR_RX_FULL |
                                                            ezi2c_INTR_RX_NOT_EMPTY);
                }
                else
                {
                    /* Exit for slave interrupts which are not active for RX direction:
                    * INTR_SLAVE.I2C_ADDR_MATCH and INTR_SLAVE.I2C_STOP while byte-by-byte reception.
                    */
                }
            }

            if(0u != (locIntrCause & ezi2c_INTR_CAUSE_SLAVE))
            {
                /* INTR_SLAVE.I2C_STOP */
                /* Catch Stop condition: completion of write or read transfer */
            #if(!ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
                if(0u != (locIntrSlave & ezi2c_INTR_SLAVE_I2C_STOP))
            #else
                /* Prevent triggering when matched address was NACKed */
                if((0u != (locIntrSlave & ezi2c_INTR_SLAVE_I2C_STOP)) &&
                   (0u != ((uint8) ezi2c_EZI2C_STATUS_BUSY & ezi2c_curStatus)))
            #endif
                {
                    /* Disable TX and RX interrupt sources */
                    ezi2c_SetRxInterruptMode(ezi2c_NO_INTR_SOURCES);
                    ezi2c_SetTxInterruptMode(ezi2c_NO_INTR_SOURCES);

                    /* Set read completion mask */
                    locStatus = ezi2c_INTR_SLAVE_I2C_NACK;

                    /* Check if buffer content was modified: the address phase resets the locIndex */
                    if(locIndex != ezi2c_EZI2C_GET_OFFSET(activeAddress))
                    {
                        locStatus |= ezi2c_INTR_SLAVE_I2C_WRITE_STOP;
                    }

                    /* Complete read or write transaction */
                    locStatus &= locIntrSlave;
                    ezi2c_EZI2C_UPDATE_LOC_STATUS(activeAddress, locStatus);
                    locStatus |= (uint32)  ezi2c_curStatus;
                    locStatus &= (uint32) ~ezi2c_EZI2C_STATUS_BUSY;
                    ezi2c_curStatus = (uint8) locStatus;

                    ezi2c_fsmState = ezi2c_EZI2C_FSM_IDLE;

                    #if(ezi2c_SECONDARY_ADDRESS_ENABLE)
                    {
                        /* Store local index into global variable, before address phase */
                        ezi2c_EZI2C_SET_INDEX(activeAddress, locIndex);
                    }
                    #endif
                }

                /* INTR_SLAVE.I2C_ADDR_MATCH */
                /* The matched address is received: the slave starts its operation.
                * INTR_SLAVE_I2C_STOP updates the buffer index before the address phase for two addresses mode.
                * This is done to update buffer index correctly before the address phase changes it.
                */
                if(0u != (locIntrSlave & ezi2c_INTR_SLAVE_I2C_ADDR_MATCH))
                {
                    #if(ezi2c_SECONDARY_ADDRESS_ENABLE)
                    {
                        /* Read address byte from RX FIFO */
                        locByte = ezi2c_GET_I2C_7BIT_ADDRESS(ezi2c_RX_FIFO_RD_REG);

                        ezi2c_ClearRxInterruptSource(ezi2c_INTR_RX_NOT_EMPTY);

                        /* Check received address against device addresses */
                        if(ezi2c_addrBuf1 == locByte)
                        {
                            /* Set buffer exposed to primary slave address */
                            ezi2c_dataBuffer = ezi2c_dataBuffer1;
                            ezi2c_bufSizeBuf = ezi2c_bufSizeBuf1;
                            ezi2c_protectBuf = ezi2c_protectBuf1;

                            activeAddress = ezi2c_EZI2C_ACTIVE_ADDRESS1;
                        }
                        else if(ezi2c_addrBuf2 == locByte)
                        {
                            /* Set buffer exposed to secondary slave address */
                            ezi2c_dataBuffer = ezi2c_dataBuffer2;
                            ezi2c_bufSizeBuf = ezi2c_bufSizeBuf2;
                            ezi2c_protectBuf = ezi2c_protectBuf2;

                            activeAddress = ezi2c_EZI2C_ACTIVE_ADDRESS2;
                        }
                        else
                        {
                            /* Address does not match */
                            ackResponse = ezi2c_EZI2C_NACK_RECEIVED_ADDRESS;
                        }
                    }
                    #endif

                #if(ezi2c_SECONDARY_ADDRESS_ENABLE_CONST)
                    if(ezi2c_EZI2C_NACK_RECEIVED_ADDRESS == ackResponse)
                    {
                        /* Clear interrupt sources before NACK address */
                        ezi2c_ClearI2CExtClkInterruptSource(ezi2c_INTR_I2C_EC_WAKE_UP);
                        ezi2c_ClearSlaveInterruptSource(ezi2c_INTR_SLAVE_ALL);

                    #if(!ezi2c_CY_SCBIP_V0)
                        /* Disable INTR_I2C_STOP to not trigger after matched address is NACKed. Ticket ID#156094 */
                        ezi2c_DISABLE_INTR_SLAVE(ezi2c_INTR_SLAVE_I2C_STOP);
                    #endif /* (!ezi2c_CY_SCBIP_V0) */

                        /* NACK address byte: it does not match neither primary nor secondary */
                        ezi2c_I2C_SLAVE_GENERATE_NACK;
                    }
                    else
                #endif /* (ezi2c_SECONDARY_ADDRESS_ENABLE_CONST) */
                    {

                    #if(ezi2c_SCB_MODE_UNCONFIG_CONST_CFG)
                        if(!ezi2c_SECONDARY_ADDRESS_ENABLE)
                        {
                            /* Set buffer exposed to primary slave address */
                            ezi2c_dataBuffer = ezi2c_dataBuffer1;
                            ezi2c_bufSizeBuf = ezi2c_bufSizeBuf1;
                            ezi2c_protectBuf = ezi2c_protectBuf1;

                            activeAddress = ezi2c_EZI2C_ACTIVE_ADDRESS1;
                        }
                    #endif /* (ezi2c_SCB_MODE_UNCONFIG_CONST_CFG) */

                        /* Bus becomes busy after address is received */
                        ezi2c_curStatus |= (uint8) ezi2c_EZI2C_STATUS_BUSY;

                        /* Slave is read or written: set current offset */
                        locIndex = ezi2c_EZI2C_GET_OFFSET(activeAddress);

                        /* Check transaction direction */
                        if(ezi2c_CHECK_I2C_STATUS(ezi2c_I2C_STATUS_S_READ))
                        {
                            /* Calculate slave buffer size */
                            locBufSize = ezi2c_bufSizeBuf1 - (uint16) locIndex;

                            /* Clear TX FIFO to start fill from offset */
                            ezi2c_CLEAR_TX_FIFO;
                            ezi2c_SetTxInterruptMode(ezi2c_INTR_TX_EMPTY);
                        }
                        else
                        {
                            /* Master writes: enable reception interrupt. The FSM state was set in INTR_SLAVE_I2C_STOP */
                            ezi2c_SetRxInterruptMode(ezi2c_INTR_RX_NOT_EMPTY);
                        }

                        /* Clear interrupt sources before ACK address */
                        ezi2c_ClearI2CExtClkInterruptSource(ezi2c_INTR_I2C_EC_WAKE_UP);
                        ezi2c_ClearSlaveInterruptSource(ezi2c_INTR_SLAVE_ALL);

                    #if (!ezi2c_CY_SCBIP_V0)
                        /* Enable STOP to trigger after address match is ACKed. Ticket ID#156094 */
                        ezi2c_ENABLE_INTR_SLAVE(ezi2c_INTR_SLAVE_I2C_STOP);
                    #endif /* (!ezi2c_CY_SCBIP_V0) */

                        /* ACK the address byte */
                        ezi2c_I2C_SLAVE_GENERATE_ACK;
                    }
                }

                /* Clear slave interrupt sources */
                ezi2c_ClearSlaveInterruptSource(locIntrSlave);
            }

            /* INTR_TX.EMPTY */
            /* Transmits data to the master: loads data into the TX FIFO. The 0xFF sends out if the master reads
            * out the buffer. The address reception with a read flag clears the TX FIFO to be loaded with data.
            */
            if(0u != (ezi2c_GetInterruptCause() & ezi2c_INTR_CAUSE_TX))
            {
                /* Put data into TX FIFO until there is a room */
                do
                {
                    /* Check transmit buffer range: locBufSize calculates after address reception */
                    if(0u != locBufSize)
                    {
                        ezi2c_TX_FIFO_WR_REG = (uint32) ezi2c_dataBuffer1[locIndex];
                        locIndex++;
                        locBufSize--;
                    }
                    else
                    {
                        ezi2c_TX_FIFO_WR_REG = ezi2c_EZI2C_OVFL_RETURN;
                    }
                }
                while(ezi2c_EZI2C_FIFO_SIZE != ezi2c_GET_TX_FIFO_ENTRIES);

                ezi2c_ClearTxInterruptSource(ezi2c_INTR_TX_EMPTY);
            }
        }

        /* Store local index copy into global variable */
        ezi2c_EZI2C_SET_INDEX(activeAddress, locIndex);

    #ifdef ezi2c_EZI2C_STRETCH_ISR_EXIT_CALLBACK
        ezi2c_EZI2C_STRETCH_ISR_ExitCallback();
    #endif /* ezi2c_EZI2C_STRETCH_ISR_EXIT_CALLBACK */

    }
#endif /* (ezi2c_EZI2C_SCL_STRETCH_ENABLE_CONST) */


#if(ezi2c_EZI2C_SCL_STRETCH_DISABLE_CONST)
    /*******************************************************************************
    * Function Name: ezi2c_EZI2C_NO_STRETCH_ISR
    ********************************************************************************
    *
    * Summary:
    *  Handles the Interrupt Service Routine for the SCB EZI2C mode. Clock stretching is
    *  NOT used during operation.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    CY_ISR_PROTO(ezi2c_EZI2C_NO_STRETCH_ISR)
    {
    #if(ezi2c_SUB_ADDRESS_SIZE16_CONST)
        static uint8 locOffset;
    #endif /* (ezi2c_SUB_ADDRESS_SIZE16_CONST) */

        uint32 locByte;
        uint32 locStatus;
        uint32 locIntrSlave;
        uint32 locIntrCause;

    #ifdef ezi2c_EZI2C_NO_STRETCH_ISR_ENTRY_CALLBACK
        ezi2c_EZI2C_NO_STRETCH_ISR_EntryCallback();
    #endif /* ezi2c_EZI2C_NO_STRETCH_ISR_ENTRY_CALLBACK */

    #if !defined (CY_REMOVE_ezi2c_CUSTOM_INTR_HANDLER)
        /* Calls registered customer routine to manage interrupt sources */
        if(NULL != ezi2c_customIntrHandler)
        {
            ezi2c_customIntrHandler();
        }
    #else
        CY_ezi2c_CUSTOM_INTR_HANDLER();
    #endif /* !defined (CY_REMOVE_ezi2c_CUSTOM_INTR_HANDLER) */

        locByte = 0u;

        /* Get copy of triggered slave interrupt sources */
        locIntrSlave = ezi2c_GetSlaveInterruptSource();
        locIntrCause = ezi2c_GetInterruptCause();

        /* INTR_SLAVE.I2C_ARB_LOST and INTR_SLAVE.I2C_BUS_ERROR */
        /* Handles errors on the bus: There are cases when both bits are set.
        * The error recovery is common: re-enable the scb IP. The content of the RX FIFO is lost.
        */
        if(0u != (locIntrSlave & (ezi2c_INTR_SLAVE_I2C_ARB_LOST |
                                  ezi2c_INTR_SLAVE_I2C_BUS_ERROR)))
        {
            ezi2c_CTRL_REG &= (uint32) ~ezi2c_CTRL_ENABLED; /* Disable SCB block */

        #if (ezi2c_CY_SCBIP_V0)
            if(0u != (locIntrSlave & ezi2c_INTR_SLAVE_I2C_ADDR_MATCH))
        #endif /* (ezi2c_CY_SCBIP_V0) */
            {
                ezi2c_curStatus |= (uint8) ezi2c_EZI2C_STATUS_ERR;

                if(0u != (ezi2c_EZI2C_FSM_WRITE_MASK & ezi2c_fsmState))
                {
                    /* Write complete */
                    if(ezi2c_indexBuf1 != ezi2c_offsetBuf1)
                    {
                        ezi2c_curStatus |= (uint8) ezi2c_INTR_SLAVE_I2C_WRITE_STOP;
                    }
                }
                else
                {
                    /* Read complete */
                    ezi2c_curStatus |= (uint8) ezi2c_INTR_SLAVE_I2C_NACK;
                }
            }

            /* Clean-up interrupt sources */
            ezi2c_SetTxInterruptMode(ezi2c_NO_INTR_SOURCES);

        #if (ezi2c_CY_SCBIP_V0)
            /* Clear interrupt sources as they are not automatically cleared after SCB is disabled */
            ezi2c_ClearRxInterruptSource(ezi2c_INTR_RX_NOT_EMPTY);
            ezi2c_ClearSlaveInterruptSource(ezi2c_INTR_SLAVE_ALL);
        #endif /* (ezi2c_CY_SCBIP_V0) */

            ezi2c_fsmState = ezi2c_EZI2C_FSM_IDLE;

            ezi2c_CTRL_REG |= (uint32) ezi2c_CTRL_ENABLED;  /* Enable SCB block */
        }
        else
        {
            /* INTR_RX.NOT_EMPTY */
            /* The slave receives data from the master: accepts into the RX FIFO. At least one entry is available to be
            * read. The offset is written first and all the following bytes are data (expected to be put in the buffer).
            * The slave ACKs all bytes, but it discards them if they do not match the write criteria.
            * The slave NACKs the bytes in the case of an RX FIFO overflow.
            */
            if(0u != (locIntrCause & ezi2c_INTR_CAUSE_RX))
            {
                /* Read byte from the RX FIFO */
                locByte = ezi2c_RX_FIFO_RD_REG;

                switch(ezi2c_fsmState)
                {

                case ezi2c_EZI2C_FSM_BYTE_WRITE:
                    /* Check buffer index against protect area */
                    if(ezi2c_indexBuf1 < ezi2c_protectBuf1)
                    {
                        /* Stores received byte into buffer */
                        ezi2c_dataBuffer1[ezi2c_indexBuf1] = (uint8) locByte;
                        ezi2c_indexBuf1++;
                    }
                    else
                    {
                        /* Discard current byte and sets FSM state to discard following bytes */
                        ezi2c_fsmState = ezi2c_EZI2C_FSM_WAIT_STOP;
                    }

                    break;

            #if(ezi2c_SUB_ADDRESS_SIZE16_CONST)
                case ezi2c_EZI2C_FSM_OFFSET_HI8:

                    /* Store high byte of offset */
                    locOffset = (uint8) locByte;

                    ezi2c_fsmState  = ezi2c_EZI2C_FSM_OFFSET_LO8;

                    break;
            #endif /* (ezi2c_SUB_ADDRESS_SIZE16_CONST) */

                case ezi2c_EZI2C_FSM_OFFSET_LO8:

                    #if(ezi2c_SUB_ADDRESS_SIZE16)
                    {
                        /* Append offset with high byte */
                        locByte = ((uint32) ((uint32) locOffset << 8u)) | locByte;
                    }
                    #endif

                    /* Check if offset within buffer range */
                    if(locByte < (uint32) ezi2c_bufSizeBuf1)
                    {
                        /* Store and sets received offset */
                        ezi2c_offsetBuf1 = (uint8) locByte;
                        ezi2c_indexBuf1  = (uint16) locByte;

                        /* Move FSM to data receive state */
                        ezi2c_fsmState = ezi2c_EZI2C_FSM_BYTE_WRITE;
                    }
                    else
                    {
                        /* Reset index due to TX FIFO fill */
                        ezi2c_indexBuf1 = (uint16) ezi2c_offsetBuf1;

                        /* Discard current byte and sets FSM state to default to discard following bytes */
                        ezi2c_fsmState = ezi2c_EZI2C_FSM_WAIT_STOP;
                    }

                    break;

                case ezi2c_EZI2C_FSM_WAIT_STOP:
                    /* Clear RX FIFO to discard all received data */
                    ezi2c_CLEAR_RX_FIFO;

                    break;

                default:
                    CYASSERT(0u != 0u); /* Should never get there */
                    break;
                }

                ezi2c_ClearRxInterruptSource(ezi2c_INTR_RX_NOT_EMPTY);
            }


            /* INTR_SLAVE.I2C_START */
            /* Catches start of transfer to trigger TX FIFO update event */
            if(0u != (locIntrSlave & ezi2c_INTR_SLAVE_I2C_START))
            {
            #if(!ezi2c_CY_SCBIP_V0)
                #if(ezi2c_EZI2C_EC_AM_ENABLE)
                {
                    /* Manage INTR_I2C_EC.WAKE_UP as slave busy status */
                    ezi2c_ClearI2CExtClkInterruptSource(ezi2c_INTR_I2C_EC_WAKE_UP);
                }
                #else
                {
                    /* Manage INTR_SLAVE.I2C_ADDR_MATCH as slave busy status */
                    ezi2c_ClearSlaveInterruptSource(ezi2c_INTR_SLAVE_I2C_ADDR_MATCH);
                }
                #endif
            #else
                /* Manage INTR_SLAVE.I2C_ADDR_MATCH as slave busy status */
                ezi2c_ClearSlaveInterruptSource(ezi2c_INTR_SLAVE_I2C_ADDR_MATCH);
            #endif /* (ezi2c_CY_SCBIP_V0) */

                /* Clear TX FIFO and put a byte */
                ezi2c_FAST_CLEAR_TX_FIFO;
                ezi2c_TX_FIFO_WR_REG = (uint32) ezi2c_dataBuffer1[ezi2c_offsetBuf1];

                /* Store buffer index to be handled by INTR_SLAVE.I2C_STOP */
                locByte = (uint32) ezi2c_indexBuf1;

                /* Update index: one byte is already in the TX FIFO */
                ezi2c_indexBuf1 = (uint16) ezi2c_offsetBuf1 + 1u;

                /* Enable INTR_TX.NOT_FULL to load TX FIFO */
                ezi2c_SetTxInterruptMode(ezi2c_INTR_TX_TRIGGER);

                /* Clear locIntrSlave after INTR.TX_TRIGGER is enabled */
                ezi2c_ClearSlaveInterruptSource(locIntrSlave);

                locIntrCause |= ezi2c_INTR_CAUSE_TX;
            }


            /* INTR_TX.TRIGGER */
            /* Transmits data to the master: loads data into the TX FIFO. The TX FIFO is loaded with data
            *  until used entries are less than ezi2c_TX_LOAD_SIZE. If index reaches end of the
            *  buffer the 0xFF is sent to the end of transfer.
            */
            if(0u != (locIntrCause & ezi2c_INTR_CAUSE_TX))
            {
                /* Put data into TX FIFO until there is room */
                do
                {
                    /* Check transmit buffer range */
                    if(ezi2c_indexBuf1 < ezi2c_bufSizeBuf1)
                    {
                        ezi2c_TX_FIFO_WR_REG = (uint32) ezi2c_dataBuffer1[ezi2c_indexBuf1];
                        ezi2c_indexBuf1++;
                    }
                    else
                    {
                        ezi2c_TX_FIFO_WR_REG = ezi2c_EZI2C_OVFL_RETURN;
                    }

                }
                while(ezi2c_TX_LOAD_SIZE != ezi2c_GET_TX_FIFO_ENTRIES);

                ezi2c_ClearTxInterruptSource(ezi2c_INTR_TX_TRIGGER);
            }


            /* INTR_SLAVE.I2C_STOP */
            /* Catch completion of write or read transfer. */
            if(0u != (locIntrSlave & ezi2c_INTR_SLAVE_I2C_STOP))
            {
                if(0u == (locIntrSlave & ezi2c_INTR_SLAVE_I2C_START))
                {
                #if(!ezi2c_CY_SCBIP_V0)
                    #if(ezi2c_EZI2C_EC_AM_ENABLE)
                    {
                        /* Manage INTR_I2C_EC.WAKE_UP as slave busy status */
                        ezi2c_ClearI2CExtClkInterruptSource(ezi2c_INTR_I2C_EC_WAKE_UP);
                    }
                    #endif
                #endif /* (!ezi2c_CY_SCBIP_V0) */

                    /* Manage INTR_SLAVE.I2C_ADDR_MATCH as slave busy status */
                    ezi2c_ClearSlaveInterruptSource(locIntrSlave);

                    /* Read current buffer index */
                    locByte = (uint32) ezi2c_indexBuf1;
                }

                /* Set read completion mask */
                locStatus = ezi2c_INTR_SLAVE_I2C_NACK;

                if((locByte != ezi2c_offsetBuf1) &&
                   (0u != (ezi2c_EZI2C_FSM_WRITE_MASK & ezi2c_fsmState)))
                {
                    /* Set write completion mask */
                    locStatus |= ezi2c_INTR_SLAVE_I2C_WRITE_STOP;
                }

                /* Set completion flags in the status variable */
                ezi2c_curStatus |= (uint8) (locStatus & locIntrSlave);

                ezi2c_fsmState = ezi2c_EZI2C_FSM_IDLE;
            }


        #if(!ezi2c_CY_SCBIP_V0)
            #if(ezi2c_EZI2C_EC_AM_ENABLE)
            {
                /* INTR_I2C_EC.WAKE_UP */
                /* Wake up device from deep sleep on address match event. The matched address is NACKed */
                if(0u != (locIntrCause & ezi2c_INTR_CAUSE_I2C_EC))
                {
                    ezi2c_I2C_SLAVE_GENERATE_NACK; /* NACK in active mode */
                    ezi2c_ClearI2CExtClkInterruptSource(ezi2c_INTR_I2C_EC_WAKE_UP);
                }
            }
            #endif
        #endif /* (!ezi2c_CY_SCBIP_V0) */
        }
        
    #ifdef ezi2c_EZI2C_NO_STRETCH_ISR_EXIT_CALLBACK
        ezi2c_EZI2C_NO_STRETCH_ISR_ExitCallback();
    #endif /* ezi2c_EZI2C_NO_STRETCH_ISR_EXIT_CALLBACK */
        
    }
#endif /* (ezi2c_EZI2C_SCL_STRETCH_DISABLE_CONST) */


/* [] END OF FILE */
