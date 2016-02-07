/*******************************************************************************
* File Name: i2c_I2C_INT.c
* Version 1.10
*
* Description:
*  This file provides the source code to the Interrupt Servive Routine for
*  the SCB Component in I2C mode.
*
* Note:
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "i2c_PVT.h"
#include "i2c_I2C_PVT.h"


/*******************************************************************************
* Function Name: i2c_I2C_ISR
********************************************************************************
*
* Summary:
*  Handles Interrupt Service Routine for SCB I2C mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
CY_ISR(i2c_I2C_ISR)
{
    uint32 diffCount;
    uint32 endTransfer;
    uint8 enableInterrupts;

    #if(i2c_CHECK_I2C_ACCEPT_ADDRESS_CONST)
        uint32 address;
    #endif /* (i2c_CHECK_I2C_ACCEPT_ADDRESS_CONST) */

    endTransfer = 0u; /* Continue active transfer */

    /* Call customer routine if registered */
    if(NULL != i2c_customIntrHandler)
    {
        i2c_customIntrHandler();
    }

    if(i2c_CHECK_INTR_I2C_EC_MASKED(i2c_INTR_I2C_EC_WAKE_UP))
    {
        /* Mask-off after wakeup */
        i2c_SetI2CExtClkInterruptMode(i2c_NO_INTR_SOURCES);
    }

    /* Master and Slave error tracking:
    * Add master state check to track only master errors when master is active or track
    * slave errors when slave active or idle.
    * Specil MMS case: on address phase with misplaced Start: master sets LOST_ARB and
    * slave BUS_ERR. The valid event is LOST_ARB from master.
    */
    if(i2c_CHECK_I2C_FSM_MASTER)
    {
        #if(i2c_I2C_MASTER)
        {
            /* INTR_MASTER_I2C_BUS_ERROR:
            * Misplaced Start or Stop condition was occurred on the bus: complete transaction.
            * The interrupt is cleared in the I2C_FSM_EXIT_IDLE.
            */
            if(i2c_CHECK_INTR_MASTER_MASKED(i2c_INTR_MASTER_I2C_BUS_ERROR))
            {
                i2c_mstrStatus |= (uint16) (i2c_I2C_MSTAT_ERR_XFER |
                                                         i2c_I2C_MSTAT_ERR_BUS_ERROR);

                endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
            }

            /* INTR_MASTER_I2C_ARB_LOST:
            * MultiMaster lost arbitrage while transaction: complete transaction.
            * Misplaced Start or Stop condition treats as lost arbitration when master drives SDA.
            * The interrupt is cleared in the I2C_FSM_EXIT_IDLE.
            */
            if(i2c_CHECK_INTR_MASTER_MASKED(i2c_INTR_MASTER_I2C_ARB_LOST))
            {
                i2c_mstrStatus |= (uint16) (i2c_I2C_MSTAT_ERR_XFER |
                                                         i2c_I2C_MSTAT_ERR_ARB_LOST);

                endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
            }

            #if(i2c_I2C_MULTI_MASTER_SLAVE)
            {
                /* I2C_MASTER_CMD_M_START_ON_IDLE:
                * MultiMaster-Slave does not generate start, because Slave was addressed
                * earlier: pass control the slave FSM.
                */
                if(i2c_CHECK_I2C_MASTER_CMD(i2c_I2C_MASTER_CMD_M_START_ON_IDLE))
                {
                    i2c_mstrStatus |= (uint16) (i2c_I2C_MSTAT_ERR_XFER |
                                                             i2c_I2C_MSTAT_ERR_ABORT_XFER);

                    endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
                }
            }
            #endif

            /* Error handling common part:
            * Set completion flag of master transcation and pass control to:
            *  - I2C_FSM_EXIT_IDLE - to complete transcation in case of: ARB_LOST or BUS_ERR.
            *  - I2C_FSM_IDLE      - to take chanse for slave to process incomming transcation.
            */
            if(0u != endTransfer)
            {
                /* Set completion before FSM change */
                i2c_mstrStatus |= (uint16) i2c_GET_I2C_MSTAT_CMPLT;

                #if(i2c_I2C_MULTI_MASTER_SLAVE)
                {
                    if(i2c_CHECK_I2C_FSM_ADDR)
                    {
                        /* The Start generation was set after enother master start accessing the Slave.
                        * Clean-up the master and turn to slave. Set state to IDLE.
                        */
                        if(i2c_CHECK_I2C_MASTER_CMD(i2c_I2C_MASTER_CMD_M_START_ON_IDLE))
                        {
                            i2c_I2C_MASTER_CLEAR_START;

                            endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER; /* Pass control to Slave */
                        }
                        /* The valid arbitration lost on address phase happens only when: master LOST_ARB set and
                        * slave BUS_ERR is cleared. Only in that case set state to IDLE without SCB IP re-enable.
                        */
                        else if((!i2c_CHECK_INTR_SLAVE_MASKED(i2c_INTR_SLAVE_I2C_BUS_ERROR))
                               && i2c_CHECK_INTR_MASTER_MASKED(i2c_INTR_MASTER_I2C_ARB_LOST))
                        {
                            endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER; /* Pass control to Slave */
                        }
                        else
                        {
                            endTransfer = 0u; /* Causes I2C_FSM_EXIT_IDLE to be set below */
                        }

                        if(0u != endTransfer) /* Clean-up master to proceed with slave */
                        {
                            i2c_CLEAR_TX_FIFO; /* Shifter keeps address, clear it */

                            i2c_DISABLE_MASTER_AUTO_DATA_ACK; /* In case of reading disable autoACK */

                            /* Clean-up master interrupt sources */
                            i2c_ClearMasterInterruptSource(i2c_INTR_MASTER_ALL);

                            /* Disable data processing interrupts: they should be cleared before */
                            i2c_SetRxInterruptMode(i2c_NO_INTR_SOURCES);
                            i2c_SetTxInterruptMode(i2c_NO_INTR_SOURCES);

                            i2c_state = i2c_I2C_FSM_IDLE;
                        }
                        else
                        {
                            /* Set I2C_FSM_EXIT_IDLE for BUS_ERR and ARB_LOST (that is really bus error) */
                            i2c_state = i2c_I2C_FSM_EXIT_IDLE;
                        }
                    }
                    else
                    {
                        /* Set I2C_FSM_EXIT_IDLE if any other state than address */
                        i2c_state = i2c_I2C_FSM_EXIT_IDLE;
                    }
                }
                #else
                {
                    /* In case of LOST*/
                    i2c_state = i2c_I2C_FSM_EXIT_IDLE;
                }
                #endif
            }
        }
        #endif
    }
    else /* (i2c_CHECK_I2C_FSM_SLAVE) */
    {
        #if(i2c_I2C_SLAVE)
        {
            /* INTR_SLAVE_I2C_BUS_ERROR or i2c_INTR_SLAVE_I2C_ARB_LOST:
            * Misplaced Start or Stop condition was occurred on the bus: set flag
            * to notify error condition.
            */
            if(i2c_CHECK_INTR_SLAVE_MASKED(i2c_INTR_SLAVE_I2C_BUS_ERROR |
                                                        i2c_INTR_SLAVE_I2C_ARB_LOST))
            {
                if(i2c_CHECK_I2C_FSM_RD)
                {
                    /* TX direction: master reads from slave */
                    i2c_slStatus &= (uint8) ~i2c_I2C_SSTAT_RD_BUSY;
                    i2c_slStatus |= (uint8) (i2c_I2C_SSTAT_RD_ERR |
                                                          i2c_I2C_SSTAT_RD_CMPLT);
                }
                else
                {
                    /* RX direction: master writes into slave */
                    i2c_slStatus &= (uint8) ~i2c_I2C_SSTAT_WR_BUSY;
                    i2c_slStatus |= (uint8) (i2c_I2C_SSTAT_WR_ERR |
                                                          i2c_I2C_SSTAT_WR_CMPLT);
                }

                i2c_state = i2c_I2C_FSM_EXIT_IDLE;
            }
        }
        #endif
    }

    /* States description:
    * Any Master operation starts from: ADDR_RD/WR state as the master generates traffic on the bus.
    * Any Slave operation starts from: IDLE state as slave always waiting actions from the master.
    */

    /* FSM Master */
    if(i2c_CHECK_I2C_FSM_MASTER)
    {
        #if(i2c_I2C_MASTER)
        {
            /* INTR_MASTER_I2C_STOP:
            * Stop condition was generated by the master: end of transction.
            * Set completion flags to notify API.
            */
            if(i2c_CHECK_INTR_MASTER_MASKED(i2c_INTR_MASTER_I2C_STOP))
            {
                i2c_ClearMasterInterruptSource(i2c_INTR_MASTER_I2C_STOP);

                i2c_mstrStatus |= (uint16) i2c_GET_I2C_MSTAT_CMPLT;
                i2c_state       = i2c_I2C_FSM_IDLE;
            }
            else
            {
                if(i2c_CHECK_I2C_FSM_ADDR) /* Address stage */
                {
                    /* INTR_MASTER_I2C_NACK:
                    * Master send address but it was NACKed by the slave: complete transaction.
                    */
                    if(i2c_CHECK_INTR_MASTER_MASKED(i2c_INTR_MASTER_I2C_NACK))
                    {
                        i2c_ClearMasterInterruptSource(i2c_INTR_MASTER_I2C_NACK);

                        i2c_mstrStatus |= (uint16) (i2c_I2C_MSTAT_ERR_XFER |
                                                                 i2c_I2C_MSTAT_ERR_ADDR_NAK);

                        endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
                    }
                    /* INTR_TX_UNDERFLOW. The address byte was sent:
                    *  - TX direction: the clock is stretched after ACK phase, because TX FIFO is
                    *    EMPTY. The TX EMPTY clean all TX interrupt sources.
                    *  - RX direction: the 1st byte is receiving, but there is no ACK permision,
                    *    clock is stretched after 1 byte will be received.
                    */
                    else
                    {
                        if(i2c_CHECK_I2C_FSM_RD) /* Reading */
                        {
                            i2c_state = i2c_I2C_FSM_MSTR_RD_DATA;
                        }
                        else /* Writing */
                        {
                            i2c_state = i2c_I2C_FSM_MSTR_WR_DATA;
                            i2c_SetTxInterruptMode(i2c_INTR_TX_EMPTY);
                        }
                    }
                }

                if(i2c_CHECK_I2C_FSM_DATA) /* Data phase */
                {
                    if(i2c_CHECK_I2C_FSM_RD) /* Reading */
                    {
                        /* INTR_RX_FULL:
                        * RX direction: master received 8 bytes, the 9th byte is receiving.
                        * Get data from RX FIFO and decide whether to ACK or  NACK following bytes.
                        */
                        if(i2c_CHECK_INTR_RX_MASKED(i2c_INTR_RX_FULL))
                        {
                            /* Calculate difference */
                            diffCount =  i2c_mstrRdBufSize -
                                        (i2c_mstrRdBufIndex + i2c_GET_RX_FIFO_ENTRIES);

                            /* Proceed transaction or end it when RX FIFO
                             * become FULL again .
                            */
                            if(diffCount > i2c_FIFO_SIZE)
                            {
                                diffCount = i2c_FIFO_SIZE;
                            }
                            else
                            {
                                if(0u == diffCount)
                                {
                                    i2c_DISABLE_MASTER_AUTO_DATA_ACK;

                                    diffCount   = i2c_FIFO_SIZE;
                                    endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
                                }
                            }

                            for(;(0u != diffCount);diffCount--)
                            {
                                i2c_mstrRdBufPtr[i2c_mstrRdBufIndex] = (uint8)
                                                                                        i2c_RX_FIFO_RD_REG;
                                i2c_mstrRdBufIndex++;
                            }
                        }
                        /* INTR_RX_NOT_EMPTY:
                        * RX direction: master received one data byte need to ACK or NACK.
                        * The last byte is stored and NACKed by the master. The NACK and Stop is
                        * generated by one command generate Stop.
                        */
                        else if(i2c_CHECK_INTR_RX_MASKED(i2c_INTR_RX_NOT_EMPTY))
                        {
                            /* Put data in the component buffer */
                            i2c_mstrRdBufPtr[i2c_mstrRdBufIndex] = (uint8) i2c_RX_FIFO_RD_REG;
                            i2c_mstrRdBufIndex++;

                            if(i2c_mstrRdBufIndex < i2c_mstrRdBufSize)
                            {
                                i2c_I2C_MASTER_GENERATE_ACK;
                            }
                            else
                            {
                               endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
                            }
                        }
                        else
                        {
                            /* Does nothing */
                        }

                        i2c_ClearRxInterruptSource(i2c_INTR_RX_ALL);
                    }
                    else /* Writing */
                    {
                        /* INTR_MASTER_I2C_NACK :
                        * Master writes data to the slave and NACK was received: not all bytes were
                        * written to the slave from TX FIFO. Revert index if there is data in
                        * TX FIFO and pass control to complete transfer.
                        */
                        if(i2c_CHECK_INTR_MASTER_MASKED(i2c_INTR_MASTER_I2C_NACK))
                        {
                            i2c_ClearMasterInterruptSource(i2c_INTR_MASTER_I2C_NACK);

                            /* Rollback the write buffer index: the NACKed byte remains in the shifter */
                            i2c_mstrWrBufIndexTmp -= (i2c_GET_TX_FIFO_ENTRIES +
                                                                   i2c_GET_TX_FIFO_SR_VALID);

                            /* Update number of transfered bytes */
                            i2c_mstrWrBufIndex = i2c_mstrWrBufIndexTmp;

                            i2c_mstrStatus |= (uint16) (i2c_I2C_MSTAT_ERR_XFER |
                                                                     i2c_I2C_MSTAT_ERR_SHORT_XFER);

                            i2c_CLEAR_TX_FIFO;

                            endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
                        }
                        /* INTR_TX_EMPTY :
                        * TX direction: the TX FIFO is EMPTY, the data from buffer need be put there.
                        * When there is no data in the component buffer, underflow interrupt is
                        * enabled to catch when all data will be transfered.
                        */
                        else if(i2c_CHECK_INTR_TX_MASKED(i2c_INTR_TX_EMPTY))
                        {
                            while(i2c_FIFO_SIZE != i2c_GET_TX_FIFO_ENTRIES)
                            {
                                /* The temporary mstrWrBufIndexTmp is used because slave could NACK the byte and index
                                * roll-back required in this case. The mstrWrBufIndex is updated at the end of transfer
                                */
                                if(i2c_mstrWrBufIndexTmp < i2c_mstrWrBufSize)
                                {
                                    /* Put data into TX FIFO */
                                    i2c_TX_FIFO_WR_REG = (uint32) i2c_mstrWrBufPtr[i2c_mstrWrBufIndexTmp];
                                    i2c_mstrWrBufIndexTmp++;
                                }
                                else
                                {
                                    break; /* No more data to put */
                                }
                            }

                            if(i2c_mstrWrBufIndexTmp == i2c_mstrWrBufSize)
                            {
                                i2c_SetTxInterruptMode(i2c_INTR_TX_UNDERFLOW);
                            }

                            i2c_ClearTxInterruptSource(i2c_INTR_TX_ALL);
                        }
                        /* INTR_TX_UNDERFLOW:
                        * TX direction: all data from TX FIFO was transfered to the slave.
                        * The transaction need to be completed.
                        */
                        else if(i2c_CHECK_INTR_TX_MASKED(i2c_INTR_TX_UNDERFLOW))
                        {
                            /* Update number of transfered bytes */
                            i2c_mstrWrBufIndex = i2c_mstrWrBufIndexTmp;

                            endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
                        }
                        else
                        {
                            /* Does nothing */
                        }
                    }
                }

                if(0u != endTransfer) /* Complete transfer */
                {
                    /* Clean-up master after reading: only in case of NACK */
                    i2c_DISABLE_MASTER_AUTO_DATA_ACK;

                    /* Disable data processing interrupts: they should be cleared before */
                    i2c_SetRxInterruptMode(i2c_NO_INTR_SOURCES);
                    i2c_SetTxInterruptMode(i2c_NO_INTR_SOURCES);

                    if(i2c_CHECK_I2C_MODE_NO_STOP(i2c_mstrControl))
                    {
                        /* On-going transaction is suspend: the ReStart is generated by API request */
                        i2c_mstrStatus |= (uint16) (i2c_I2C_MSTAT_XFER_HALT |
                                                                 i2c_GET_I2C_MSTAT_CMPLT);

                        i2c_state = i2c_I2C_FSM_MSTR_HALT;
                    }
                    else
                    {
                        /* Complete transaction: exclude data processing state and generate Stop.
                        * The completion status will be set after Stop generation.
                        * Specail case is read: because NACK and Stop is genered.
                        * The lost arbitration could occur while NACK generation
                        * (other master still reading and ACK is generated)
                        */
                        i2c_I2C_MASTER_GENERATE_STOP;
                    }
                }
            }

        } /* (i2c_I2C_MASTER) */
        #endif

    } /* (i2c_CHECK_I2C_FSM_MASTER) */


    /* FSM Slave */
    else if(i2c_CHECK_I2C_FSM_SLAVE)
    {
        #if(i2c_I2C_SLAVE)
        {
            /* INTR_SLAVE_NACK:
            * The master completes reading the slave: the approprite flags have to be set.
            * The TX FIFO cleared after overflow condition is set.
            */
            if(i2c_CHECK_INTR_SLAVE_MASKED(i2c_INTR_SLAVE_I2C_NACK))
            {
                i2c_ClearSlaveInterruptSource(i2c_INTR_SLAVE_I2C_NACK);

                /* All entries that remain in TX FIFO max value is 9: 8 (FIFO) + 1 (SHIFTER) */
                diffCount = (i2c_GET_TX_FIFO_ENTRIES + i2c_GET_TX_FIFO_SR_VALID);

                if(i2c_slOverFlowCount > diffCount) /* Overflow */
                {
                    i2c_slStatus |= (uint8) i2c_I2C_SSTAT_RD_OVFL;
                }
                else /* No Overflow */
                {
                    /* Roll-back the temporay index */
                    i2c_slRdBufIndexTmp -= (diffCount - i2c_slOverFlowCount);
                }

                /* Update slave of tranfered bytes */
                i2c_slRdBufIndex = i2c_slRdBufIndexTmp;

                /* Clean-up TX FIFO */
                i2c_SetTxInterruptMode(i2c_NO_INTR_SOURCES);
                i2c_slOverFlowCount = 0u;
                i2c_CLEAR_TX_FIFO;

                /* Complete master reading */
                i2c_slStatus &= (uint8) ~i2c_I2C_SSTAT_RD_BUSY;
                i2c_slStatus |= (uint8)  i2c_I2C_SSTAT_RD_CMPLT;
                i2c_state     =  i2c_I2C_FSM_IDLE;
            }


            /* INTR_SLAVE_I2C_WRITE_STOP:
            * The master completes writing to slave: the approprite flags have to be set.
            * The RX FIFO contains 1-8 bytes from previous transcation which need to be read.
            * There is possibility that RX FIFO contains address, it needs to leave it there.
            */
            if(i2c_CHECK_INTR_SLAVE_MASKED(i2c_INTR_SLAVE_I2C_WRITE_STOP))
            {
                i2c_ClearSlaveInterruptSource(i2c_INTR_SLAVE_I2C_WRITE_STOP);

                i2c_DISABLE_SLAVE_AUTO_DATA;

                while(0u != i2c_GET_RX_FIFO_ENTRIES)
                {
                    #if(i2c_CHECK_I2C_ACCEPT_ADDRESS)
                    {
                        if((1u == i2c_GET_RX_FIFO_ENTRIES) &&
                           (i2c_CHECK_INTR_SLAVE_MASKED(i2c_INTR_SLAVE_I2C_ADDR_MATCH)))
                        {
                            break; /* Leave address in RX FIFO */
                        }
                    }
                    #endif

                    /* Put data in the component buffer */
                    i2c_slWrBufPtr[i2c_slWrBufIndex] = (uint8) i2c_RX_FIFO_RD_REG;
                    i2c_slWrBufIndex++;
                }

                if(i2c_CHECK_INTR_RX(i2c_INTR_RX_OVERFLOW))
                {
                    i2c_slStatus |= (uint8) i2c_I2C_SSTAT_WR_OVFL;
                }

                /* Clears RX interrupt sources triggered on data receiving */
                i2c_ClearRxInterruptSource(i2c_INTR_RX_ALL);

                /* Complete master writing */
                i2c_slStatus &= (uint8) ~i2c_I2C_SSTAT_WR_BUSY;
                i2c_slStatus |= (uint8)  i2c_I2C_SSTAT_WR_CMPLT;
                i2c_state     =  i2c_I2C_FSM_IDLE;
            }


            /* INTR_SLAVE_I2C_ADDR_MATCH:
            * The address match event starts the slave operation: after leaving the TX or RX
            * direction has to chosen.
            * The wakeup interrupt must be cleared only after address match is set.
            */
            if(i2c_CHECK_INTR_SLAVE_MASKED(i2c_INTR_SLAVE_I2C_ADDR_MATCH))
            {
                #if(i2c_CHECK_I2C_ACCEPT_ADDRESS) /* Address in RX FIFO */
                {
                    address  = i2c_RX_FIFO_RD_REG;

                    /* Clears RX sources if address was received in the RX FIFO */
                    i2c_ClearRxInterruptSource(i2c_INTR_RX_ALL);

                    if(0u != address)
                    {
                        /* Suppress compiler warning */
                    }
                }
                #endif

                if(i2c_CHECK_I2C_STATUS(i2c_I2C_STATUS_S_READ))
                /* TX direction: master reads from slave */
                {
                    i2c_SetTxInterruptMode(i2c_INTR_TX_EMPTY);

                    /* Set temporary index to address buffer clear from API */
                    i2c_slRdBufIndexTmp = i2c_slRdBufIndex;

                    /* Start master reading */
                    i2c_slStatus |= (uint8) i2c_I2C_SSTAT_RD_BUSY;
                    i2c_state     = i2c_I2C_FSM_SL_RD;
                }
                else
                /* RX direction: master writes into slave */
                {
                    /* Calculate available buffer size */
                    diffCount = (i2c_slWrBufSize - i2c_slWrBufIndex);

                    if(diffCount < i2c_FIFO_SIZE)
                    /* Receive data: byte-by-byte */
                    {
                        i2c_SetRxInterruptMode(i2c_INTR_RX_NOT_EMPTY);
                    }
                    else
                    /* Receive data: into RX FIFO */
                    {
                        if(diffCount == i2c_FIFO_SIZE)
                        {
                            /* NACK when RX FIFO become FULL */
                            i2c_ENABLE_SLAVE_AUTO_DATA;
                        }
                        else
                        {
                            /* Stretch clock when RX FIFO becomes FULL */
                            i2c_ENABLE_SLAVE_AUTO_DATA_ACK;
                            i2c_SetRxInterruptMode(i2c_INTR_RX_FULL);
                        }
                    }

                    /* Start master reading */
                    i2c_slStatus |= (uint8) i2c_I2C_SSTAT_WR_BUSY;
                    i2c_state     = i2c_I2C_FSM_SL_WR;
                }

                /* Clear interrupts before ACK address */
                i2c_ClearI2CExtClkInterruptSource(i2c_INTR_I2C_EC_WAKE_UP);
                i2c_ClearSlaveInterruptSource(i2c_INTR_SLAVE_ALL);

                /* The preparation complete: ACK the address */
                i2c_I2C_SLAVE_GENERATE_ACK;
            }

            /* i2c_INTR_RX_FULL":
            * Get data from RX FIFO and decide whether to ACK or NACK following bytes
            */
            if(i2c_CHECK_INTR_RX_MASKED(i2c_INTR_RX_FULL))
            {
                /* Calculate available buffer size take to account that RX FIFO is FULL */
                diffCount =  i2c_slWrBufSize -
                            (i2c_slWrBufIndex + i2c_FIFO_SIZE);

                if(diffCount > i2c_FIFO_SIZE) /* Proceed transaction */
                {
                    diffCount   = i2c_FIFO_SIZE;
                    endTransfer = 0u;  /* Continue active transfer */
                }
                else /* End when FIFO becomes FULL again */
                {
                    endTransfer = i2c_I2C_CMPLT_ANY_TRANSFER;
                }

                for(;(0u != diffCount);diffCount--)
                {
                    /* Put data in the component buffer */
                    i2c_slWrBufPtr[i2c_slWrBufIndex] = (uint8) i2c_RX_FIFO_RD_REG;
                    i2c_slWrBufIndex++;
                }

                if(0u != endTransfer) /* End transfer sending NACK */
                {
                    i2c_ENABLE_SLAVE_AUTO_DATA_NACK;

                    /* The INTR_RX_FULL triggers earlier then INTR_SLAVE_I2C_STOP:
                    * disable all RX interrupt sources.
                    */
                    i2c_SetRxInterruptMode(i2c_NO_INTR_SOURCES);
                }

                i2c_ClearRxInterruptSource(i2c_INTR_RX_FULL);
            }
            /* i2c_INTR_RX_NOT_EMPTY:
            * The buffer size is less than 8: it requires processing in byte-by-byte mode.
            */
            else if(i2c_CHECK_INTR_RX_MASKED(i2c_INTR_RX_NOT_EMPTY))
            {
                diffCount = i2c_RX_FIFO_RD_REG;

                if(i2c_slWrBufIndex < i2c_slWrBufSize)
                {
                    i2c_I2C_SLAVE_GENERATE_ACK;

                    /* Put data into component buffer */
                    i2c_slWrBufPtr[i2c_slWrBufIndex] = (uint8) diffCount;
                    i2c_slWrBufIndex++;
                }
                else /* Overflow: there is no space in the write buffer */
                {
                    i2c_I2C_SLAVE_GENERATE_NACK;

                    i2c_slStatus |= (uint8) i2c_I2C_SSTAT_WR_OVFL;
                }

                i2c_ClearRxInterruptSource(i2c_INTR_RX_NOT_EMPTY);
            }
            else
            {
                /* Does nothing */
            }


            /* i2c_INTR_TX_EMPTY:
            * Master reads slave: provide data to read or 0xFF in case end of the buffer
            * The overflow condition must be captured, but not set until the end of transaction.
            * There is possibility of false overflow due of TX FIFO utilization.
            */
            if(i2c_CHECK_INTR_TX_MASKED(i2c_INTR_TX_EMPTY))
            {
                while(i2c_FIFO_SIZE != i2c_GET_TX_FIFO_ENTRIES)
                {
                    /* The temporary slRdBufIndexTmp is used because master could NACK the byte and
                    * index roll-back required in this case. The slRdBufIndex is updated at the end
                    * of the read transfer.
                    */
                    if(i2c_slRdBufIndexTmp < i2c_slRdBufSize)
                    /* Data from buffer */
                    {
                        i2c_TX_FIFO_WR_REG = (uint32) i2c_slRdBufPtr[i2c_slRdBufIndexTmp];
                        i2c_slRdBufIndexTmp++;
                    }
                    else
                    /* Probably Overflow */
                    {
                        i2c_TX_FIFO_WR_REG = i2c_I2C_SLAVE_OVFL_RETURN;

                        if(0u == (i2c_INTR_TX_OVERFLOW & i2c_slOverFlowCount))
                        {
                            /* Get counter in range of the byte: the value 10 is overflow */
                            i2c_slOverFlowCount++;
                        }
                    }
                }

                i2c_ClearTxInterruptSource(i2c_INTR_TX_EMPTY);
            }

        }  /* (i2c_I2C_SLAVE) */
        #endif
    }


    /* FSM EXIT:
    * Interrupt sources get here are errors:
    * Slave:  INTR_SLAVE_I2C_BUS_ERROR, INTR_SLAVE_I2C_ARB_LOST
    * Master: INTR_MASTER_I2C_BUS_ERROR, INTR_MASTER_I2C_ARB_LOST.
    */
    else
    {
        /* Clean the Slave and Master sources before reset */
        i2c_ClearSlaveInterruptSource(i2c_INTR_SLAVE_ALL);
        i2c_ClearMasterInterruptSource(i2c_INTR_MASTER_ALL);

        /* Re-enable SCB block: this resets part of functions */
        enableInterrupts = CyEnterCriticalSection();
        i2c_SCB_SW_RESET;
        CyExitCriticalSection(enableInterrupts);

        /* Clenup:
        * All other status and control bits can be cleared later.
        * Slave AUTO data ACK never happens before address ACK.
        * Slave TX and RX sources are used only after address match.
        * Master AUTO data ACK is under API control.
        * Master interrupt sources does not care after any error.
        * Master TX and RX sources are under API control.
        */

        /* Disable auto NACK before clear the FIFOs */
        i2c_DISABLE_SLAVE_AUTO_DATA_ACK;
        i2c_DISABLE_MASTER_AUTO_DATA_ACK;

        i2c_SetRxInterruptMode(i2c_NO_INTR_SOURCES);
        i2c_SetTxInterruptMode(i2c_NO_INTR_SOURCES);
        i2c_ClearTxInterruptSource(i2c_INTR_RX_ALL);
        i2c_ClearRxInterruptSource(i2c_INTR_TX_ALL);

        i2c_state = i2c_I2C_FSM_IDLE;
    }
}


/* [] END OF FILE */
