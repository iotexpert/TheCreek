/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of Interrupt Service Routine (ISR)
*  for SM/PM Bus component.
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_cmd.h"


/**********************************
*      Eexternal functions
**********************************/
extern void `$INSTANCE_NAME`_ResetBus(void) CYREENTRANT;


/**********************************
*      Global variables
**********************************/

volatile uint8 `$INSTANCE_NAME`_isCmdPhase = 0u;          /* Indicates if command receving phase active */
volatile uint8 `$INSTANCE_NAME`_cmdReceived = 0u;         /* Indicates if a command was received in this transaction */
volatile uint8 `$INSTANCE_NAME`_isValidCmd = 0u;          /* A flag indicates that a command is supporetd */

/* Intermediate buffer, where recieved data data is stored until the eand of
* write transaction. Or where the data i copied into when Read transaction
* begins.
*/
volatile uint8 `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_MAX_BUFFER_SIZE];
volatile uint8 `$INSTANCE_NAME`_bufferIndex;               /* Index used to navigate throught the buffer */
volatile uint8 `$INSTANCE_NAME`_bufferSize = 0u;           /* Size of data for last received command */

/**********************************
*      Eexternal variables
**********************************/
extern volatile uint8 `$INSTANCE_NAME`_I2C_state;          /* Current state of I2C state machine */
extern volatile uint8 `$INSTANCE_NAME`_I2C_slStatus;       /* I2C Slave Status byte */

#if (`$INSTANCE_NAME`_I2C_ADDR_DECODE == `$INSTANCE_NAME`_I2C_SW_DECODE)
    extern volatile uint8 `$INSTANCE_NAME`_I2C_slAddress;    /* Software address variable */
#endif  /* (`$INSTANCE_NAME`_I2C_ADDR_DECODE == `$INSTANCE_NAME`_I2C_SW_DECODE) */

/* Global variables is required to store SMB Alert mode and Alert Response
* Address if SMBALERT# pin is enabled .
*/
#if (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED)
    extern volatile uint8 `$INSTANCE_NAME`_smbAlertMode;
#endif /* (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED) */
extern volatile uint8 `$INSTANCE_NAME`_alertResponseAddress;
    

/********************************************************************************
*
* Summary:
*  Interrupt Service Routine for SM/PM bus Interrupt.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    static uint8 tmpByte;    /* Templorary store of I2C received byte */
    static uint8 tmpCsr;     /* Templorary store of I2C Control Status Register */

    /* Entry from interrupt */
    /* In hardware address compare mode, we can assume we only get interrupted when */
    /* a valid address is recognized. In software address compare mode, we have to  */
    /* check every address after a start condition. */

    tmpCsr = `$INSTANCE_NAME`_I2C_CSR_REG;      /* Make temp copy so that we can check */
                                                /* for stop condition after we are done */

    /* Check to see if a Start/ReStart/Address is detected. Start of Slave FSM.
    * FF : sets Addr phase with byte_complete interrupt trigger.
    * UDB: sets Addr phase immediately after Start or ReStart.
    */
    if(0u != (tmpCsr & `$INSTANCE_NAME`_I2C_CSR_ADDRESS))
    {
        /* STOP bit is set on every Stop condition detected on the bus.
        * Clear STOP bit history to properly detect end of write transaction.
        */
        tmpCsr &= ~`$INSTANCE_NAME`_I2C_CSR_STOP_STATUS;

        /* Delayed Stop handling: completes the write transaction if it was not completed before */
        if((`$INSTANCE_NAME`_I2C_SM_SL_WR_DATA == `$INSTANCE_NAME`_I2C_state) &&
            (0u == (`$INSTANCE_NAME`_I2C_DATA_REG & `$INSTANCE_NAME`_I2C_READ_FLAG)))
        {
            if(`$INSTANCE_NAME`_bufferIndex == `$INSTANCE_NAME`_bufferSize)
            {
                `$INSTANCE_NAME`_WriteHandler();            /* Process data received during "Write" transaction */
            }
            else
            {   
                /* Report that host writes to few bytes and ignore command */
                `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_WR_TO_FEW_BYTES);
            }

            `$INSTANCE_NAME`_cmdReceived = 0u;

            `$INSTANCE_NAME`_lastReceivedCmd = `$INSTANCE_NAME`_CMD_UNDEFINED;    /* Invalidate a stored command code */
            `$INSTANCE_NAME`_I2C_slStatus &= ~`$INSTANCE_NAME`_I2C_SSTAT_WR_BUSY; /* Clear WR_BUSY flag */
            `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_WR_CMPLT; /* Set WR_CMPT flag   */
            `$INSTANCE_NAME`_I2C_DISABLE_INT_ON_STOP;                             /* Disable interrupt on STOP */
            `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_IDLE;            /* Return to IDLE */
        }
        /* Restart handling: completes the write transaction */
        else
        {
            `$INSTANCE_NAME`_I2C_slStatus &= ~`$INSTANCE_NAME`_I2C_SSTAT_WR_BUSY;   /* Clear WR_BUSY flag */
            `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_WR_CMPLT;   /* Set WR_CMPT flag   */

            `$INSTANCE_NAME`_I2C_DISABLE_INT_ON_STOP;                               /* Disable interrupt on STOP */
            `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_IDLE;              /* Return to IDLE */
        }

        /* The BYTE_CMPLT ensures that address byte is already in the data register.
        * Start Address match procedure of received address.
        */
        if(`$INSTANCE_NAME`_I2C_CHECK_BYTE_COMPLETE(tmpCsr))
        {
            /* Check for software address detection */
            #if(`$INSTANCE_NAME`_I2C_ADDR_DECODE == `$INSTANCE_NAME`_I2C_SW_DECODE)

                tmpByte = ((`$INSTANCE_NAME`_I2C_DATA_REG >> `$INSTANCE_NAME`_I2C_SLAVE_ADDR_SHIFT) &
                         `$INSTANCE_NAME`_I2C_SLAVE_ADDR_MASK);

                if(tmpByte == `$INSTANCE_NAME`_I2C_slAddress)   /* Check for address match */
                {
                    /* Check for read or write command */
                    if(0u != (`$INSTANCE_NAME`_I2C_DATA_REG & `$INSTANCE_NAME`_I2C_READ_FLAG))
                    {
                        #if(`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`__PMBUS_SLAVE)

                            /***************************************
                            *           PMBus Slave
                            ***************************************/
                            /* Check if command was received */
                            if(`$INSTANCE_NAME`_COMMAND_RECEIVED ==`$INSTANCE_NAME`_cmdReceived)
                            {
                                /* Check for an "Auto" read transaction */
                                if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_AUTO & `$INSTANCE_NAME`_cmdProperties))
                                {
                                    /* Create a read transaction */
                                    `$INSTANCE_NAME`_ReadAutoHandler();

                                    /* Prepare next opeation to read, get data and place in data register */
                                    if(`$INSTANCE_NAME`_bufferIndex < `$INSTANCE_NAME`_bufferSize)
                                    {
                                         /* Load first data byte */
                                        `$INSTANCE_NAME`_I2C_DATA_REG =
                                           `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_bufferIndex];
                                        /* ACK and transmit */
                                        `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;
                                        /* Advance to data location */
                                        `$INSTANCE_NAME`_bufferIndex++;

                                        /* Set READ activity */
                                        `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;
                                    }
                                    else    /* Data overflow */
                                    {
                                        /* Out of range, send 0xFF */
                                        `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;
                                        /* ACK and transmit */
                                        `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;
                                        /* Set Read with Overflow */
                                        `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                            `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                                    }
                                }
                                /* Check for a "Manual" read transaction */
                                else if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_MANUAL & `$INSTANCE_NAME`_cmdProperties))
                                {
                                    /* Handle manual part of read */
                                    `$INSTANCE_NAME`_ReadManualHandler();
                                    /* Set READ activity */
                                    `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;
                                    /* Disable interrupt and wait for `$INSTANCE_NAME`_CompleteTransaction() */
                                    `$INSTANCE_NAME`_DisableInt();
                                }
                                else
                                {
                                    `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;               /* Out of range, send 0xFF */
                                        `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;           /* ACK and transmit */
                                    /* Master atempts to read to many bytes. This happens when Read config.
                                    * set to "None".
                                    */
                                    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES);
                                    /* Set READ activity with OVERFLOW */
                                    `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                        `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                                }
                            }
                            else
                            {
                                `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                                `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                                /* Inform user about the failure */
                                `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_READ_FLAG);
                            }

                            /* Indicate that now we are in the "Read" state */
                            `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_SL_RD_DATA;

                        #else

                            /***************************************
                            *           SMBus Slave
                            ***************************************/
                            /* Check if command was received and if it has correct properties */
                            if(`$INSTANCE_NAME`_COMMAND_RECEIVED == `$INSTANCE_NAME`_cmdReceived)
                            {
                                /* Check for an "Auto" read transaction */
                                if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_AUTO & `$INSTANCE_NAME`_cmdProperties))
                                {
                                    /* Call `$INSTANCE_NAME`_ReadAutoHandler to create a read transaction */
                                    `$INSTANCE_NAME`_ReadAutoHandler();

                                    /* Prepare next opeation to read, get data and place in data register */
                                    if(`$INSTANCE_NAME`_bufferIndex < `$INSTANCE_NAME`_bufferSize)
                                    {
                                        /* Load first data byte */
                                        `$INSTANCE_NAME`_I2C_DATA_REG =
                                           `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_bufferIndex];
                                        `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;           /* ACK and transmit */
                                        `$INSTANCE_NAME`_bufferIndex++;                  /* Advance to data location */

                                        /* Set READ activity */
                                        `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;
                                    }
                                    else    /* Data overflow */
                                    {
                                        `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                                        `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                                        /* Set READ activity with OVERFLOW */
                                        `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                            `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                                        /* Inform user about the failure */
                                        `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_READ_FLAG);
                                    }
                                }
                                /* Check for a "Manual" read transaction */
                                else if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_MANUAL & `$INSTANCE_NAME`_cmdProperties))
                                {
                                    /* Handle manual part of read */
                                    `$INSTANCE_NAME`_ReadManualHandler();

                                    /* Disable interrupt and wait for `$INSTANCE_NAME`_CompleteTransaction() */
                                    `$INSTANCE_NAME`_DisableInt();
                                }
                                else
                                {
                                    `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                                    /* Master atempts to read to many bytes. This happens when Read config.
                                    * set to "None".
                                    */
                                    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES);
                                    /* Set READ activity with OVERFLOW */
                                    `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                        `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                                }
                            }
                            else
                            {
                                #if(`$INSTANCE_NAME`_RECEIVE_BYTE_ENABLED == `$INSTANCE_NAME`_RECEIVE_BYTE_PROTOCOL)

                                    /* If command wasn't received yet it means that receive byte protocol
                                    * shoul be serviced as in other cases `$INSTANCE_NAME`_cmdReceived should
                                    * always be true.
                                    */
                                    /* User code should provide a bute value for this transaction */
                                    `$INSTANCE_NAME`_I2C_DATA_REG = `$INSTANCE_NAME`_GetReceiveByteResponse();
                                    /* ACK and transmit */
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;
                                    /* Set READ activity */
                                    `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;

                                #else

                                    `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                                    /* Inform user about the failure */
                                    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_READ_FLAG);

                                #endif /*`$INSTANCE_NAME`_RECEIVE_BYTE_ENABLED ==
                                       *`$INSTANCE_NAME`_RECEIVE_BYTE_PROTOCOL
                                       */
                            }

                            /* Indicate that now we are in the "Read" state */
                            `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_SL_RD_DATA;

                        #endif /* `$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`__PMBUS_SLAVE */

                    }
                    else  /* Start of a Write transaction, ready to write of the first byte */
                    {
                        /* Prepare to write the first byte */
                        `$INSTANCE_NAME`_I2C_ACK_AND_RECEIVE;
                        /* Prepare for Write transaction */
                        `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_SL_WR_DATA;
                        /* Set this to identify that device expects to receive a command code
                        * with a next byte of a transaction.
                        */
                        `$INSTANCE_NAME`_isCmdPhase = `$INSTANCE_NAME`_PHASE_CMD;
                        /* Set WRITE activity */
                        `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_WR_BUSY;
                        `$INSTANCE_NAME`_I2C_ENABLE_INT_ON_STOP;    /* Enable interrupt on Stop */
                    }
                }  
                /* When SMBALER# pin is exposed in the component it is required
                * to monitor if master is broadcasting Alert Response Address.
                */
                else if(tmpByte == `$INSTANCE_NAME`_alertResponseAddress)
                {
                    #if (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED)
                    
                        /* ARA was detected so need to check if a SMBALERT# is asserted and handle
                        * it properly.
                        */
                        if(0u != (`$INSTANCE_NAME`_SMBALERT_PIN_MASK & `$INSTANCE_NAME`_SMB_ALERT_CONTROL_REG))
                        {
                            /* For this request slave should respond with it's device address */
                            `$INSTANCE_NAME`_I2C_DATA_REG =
                                (`$INSTANCE_NAME`_I2C_slAddress << `$INSTANCE_NAME`_I2C_SLAVE_ADDR_SHIFT);
                            /* ACK and transmit */
                            `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;
                            /* Set READ activity */
                            `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;

                            if(`$INSTANCE_NAME`_AUTO_MODE == `$INSTANCE_NAME`_smbAlertMode)
                            {
                                /* Clear SMBALERT# pin */
                                `$INSTANCE_NAME`_SMB_ALERT_CONTROL_REG = `$INSTANCE_NAME`_STATE_DEASSERTED;
                            }
                            else if(`$INSTANCE_NAME`_FIRMWARE_MODE == `$INSTANCE_NAME`_smbAlertMode)
                            {
                                /* User is responsible for clearing SMBALERT# pin */
                                `$INSTANCE_NAME`_HandleSmbAlertResponse();
                            }
                            else
                            {
                                /* Do nothing. */
                            }
                            
                            /* Need to go to Read Data state to send last byte (0xFF) */
                            `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_SL_RD_DATA;
                        }
                        
                    #endif /* (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED) */
                }
                /* No address match */
                else
                {
                     `$INSTANCE_NAME`_I2C_NAK_AND_RECEIVE;   /* NAK Alert Response Address */
                }

            #else  /* Hardware address detection */

                /* Check for read or write command */
                if(0u != (`$INSTANCE_NAME`_I2C_DATA_REG & `$INSTANCE_NAME`_I2C_READ_FLAG))
                {
				
				    #if(`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`__PMBUS_SLAVE)

                        /***************************************
                        *           PMBus Slave
                        ***************************************/
                        /* Check if command was received and if it has correct properties */
                        if(0u != `$INSTANCE_NAME`_cmdReceived)
                        {
                            /* Check for an "Auto" read transaction */
                            if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_AUTO & `$INSTANCE_NAME`_cmdProperties))
                            {
                                /* Create a read transaction */
                                `$INSTANCE_NAME`_ReadAutoHandler();

                                /* Prepare next opeation to read, get data and place in data register */
                                if(`$INSTANCE_NAME`_bufferIndex < `$INSTANCE_NAME`_bufferSize)
                                {
                                    /* Load first data byte */
                                    `$INSTANCE_NAME`_I2C_DATA_REG =
                                        `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_bufferIndex];
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;  /* ACK and transmit */
                                    `$INSTANCE_NAME`_bufferIndex++;    /* Advance to data location */

                                    /* Set READ activity */
                                    `$INSTANCE_NAME`_I2C_slStatus  |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;
                                }
                                else    /* Data overflow */
                                {
                                    `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;    /* Out of range, send 0xFF  */
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;    /* ACK and transmit */
                                    /* Master atempts to read to many bytes. This happens when Read config.
                                    * set to "None".
                                    */
                                    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES);
                                    /* Set READ activity with OVERFLOW */
                                    `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                        `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                                }

                                }
                            /* Check for a "Manual" read transaction */
                            else if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_MANUAL & `$INSTANCE_NAME`_cmdProperties))
                            {
                                /* Handle manual part of read */
                                `$INSTANCE_NAME`_ReadManualHandler();

                                /* Disable interrupt and wait for `$INSTANCE_NAME`_CompleteTransaction() */
                                `$INSTANCE_NAME`_I2C_DisableInt();
                            }
                            else
                            {
                                `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;               /* Out of range, send 0xFF */
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;           /* ACK and transmit */
                                /* Master atempts to read to many bytes. This happens when Read config.
                                * set to "None".
                                */
                                `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES);
                                /* Set READ activity with OVERFLOW */
                                `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                    `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                            }
                        }
                        else
                        {
                            `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                            `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                            /* Inform user about the failure */
                            `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_READ_FLAG);
                        }

                        /* Prepare for Read transaction */
                        `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_SL_RD_DATA;

                    #else

                        /***************************************
                        *           SMBus Slave
                        ***************************************/
                        /* Check if command was received and if it has correct properties */
                        if(0u !=`$INSTANCE_NAME`_cmdReceived)
                        {
                            /* Check for an "Auto" read transaction */
                            if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_AUTO & `$INSTANCE_NAME`_cmdProperties))
                            {
                                /* Call `$INSTANCE_NAME`_ReadAutoHandler to create a read transaction */
                                `$INSTANCE_NAME`_ReadAutoHandler();

                                /* Prepare next opeation to read, get data and place in data register */
                                if(`$INSTANCE_NAME`_bufferIndex < `$INSTANCE_NAME`_bufferSize)
                                {
                                    /* Load first data byte */
                                    `$INSTANCE_NAME`_I2C_DATA_REG =
                                       `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_bufferIndex];
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;              /* ACK and transmit */
                                    `$INSTANCE_NAME`_bufferIndex++;                     /* Advance to data location */

                                    /* Set READ activity */
                                    `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;
                                }
                                else    /* Data overflow */
                                {
                                    `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                                    `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                                    /* Master atempts to read to many bytes. This happens when Read config.
                                    * set to "None".
                                    */
                                    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES);
                                    /* Set READ activity with OVERFLOW */
                                    `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                        `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                                }
                            }
                            /* Check for a "Manual" read transaction */
                            else if(0u != (`$INSTANCE_NAME`_CMD_PROP_READ_MANUAL & `$INSTANCE_NAME`_cmdProperties))
                            {
                                /* Handle manual part of read */
                                `$INSTANCE_NAME`_ReadManualHandler();

                                /* Disable interrupt and wait for `$INSTANCE_NAME`_CompleteTransaction() */
                                `$INSTANCE_NAME`_DisableInt();
                            }
                            else
                            {
                                `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                                `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                                /* Master atempts to read to many bytes. This happens when Read config.
                                * set to "None".
                                */
                                `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES);
                                /* Set READ activity with OVERFLOW */
                                `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                                    `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
                            }
                        }
                        else
                        {
                            #if(`$INSTANCE_NAME`_RECEIVE_BYTE_ENABLED == `$INSTANCE_NAME`_RECEIVE_BYTE_PROTOCOL)

                                /* If command wasn't received yet it means that receive byte protocol
                                * shoul be serviced as in other cases `$INSTANCE_NAME`_cmdReceived should
                                * always be true.
                                */
                                /* User code should provide a bute value for this transaction */
                                `$INSTANCE_NAME`_I2C_DATA_REG = `$INSTANCE_NAME`_GetReceiveByteResponse();
                                /* ACK and transmit */
                                `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;
                                /* Set READ activity */
                                `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;

                            #else

                                `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;          /* Out of range, send 0xFF */
                                `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;          /* ACK and transmit */
                                /* Inform user about the failure */
                                `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_READ_FLAG);

                            #endif /*`$INSTANCE_NAME`_RECEIVE_BYTE_ENABLED ==
                                   *`$INSTANCE_NAME`_RECEIVE_BYTE_PROTOCOL
                                   */
                        }

                        /* Indicate that now we are in the "Read" state */
                        `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_SL_RD_DATA;

                    #endif /* `$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`__PMBUS_SLAVE */
                }
                else  /* Start of a Write transaction, ready to write of the first byte */
                {
                    /* Prepare to write the first byte */
                    `$INSTANCE_NAME`_I2C_ACK_AND_RECEIVE;                            /* ACK and ready to receive addr */
                    `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_SL_WR_DATA; /* Prepare for write transaction */
                    /* Set this to identify that device expects to receive a command code
                    * with a next byte of a transaction.
                    */
                    `$INSTANCE_NAME`_isCmdPhase = `$INSTANCE_NAME`_PHASE_CMD;
                    /* Set WRITE activity */
                    `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_WR_BUSY;
                    `$INSTANCE_NAME`_I2C_ENABLE_INT_ON_STOP;    /* Enable interrupt on Stop */
                }

            #endif  /* (`$INSTANCE_NAME`_I2C_ADDR_DECODE == `$INSTANCE_NAME`_I2C_SW_DECODE) */
        }
    }
    /* Check for data transfer */
    else if(`$INSTANCE_NAME`_I2C_CHECK_BYTE_COMPLETE(tmpCsr))
    {
        /* Data write from Master to Slave */
        if(`$INSTANCE_NAME`_I2C_state == `$INSTANCE_NAME`_I2C_SM_SL_WR_DATA)
        {
            /* Get data, to analyze it */
            tmpByte = `$INSTANCE_NAME`_I2C_DATA_REG;

            /* Comand phase of a transaction */
            /* Check if command for current transaction was already received */
            if((`$INSTANCE_NAME`_cmdReceived != `$INSTANCE_NAME`_COMMAND_RECEIVED) &&
                (`$INSTANCE_NAME`_PHASE_CMD == `$INSTANCE_NAME`_isCmdPhase))
            {
                /* Indicate that command received */
                `$INSTANCE_NAME`_cmdReceived = `$INSTANCE_NAME`_COMMAND_RECEIVED;

                /* Checks if command is supported and stores the flag in variable for later
                * analisys.
                */
                `$INSTANCE_NAME`_isValidCmd = `$INSTANCE_NAME`_CheckCommand(tmpByte);

                /* Check if command is valid */
                if(`$INSTANCE_NAME`_COMMAND_VALID == `$INSTANCE_NAME`_isValidCmd)
                {
                    /* Store ccommand code that was just received */
                    `$INSTANCE_NAME`_lastReceivedCmd = tmpByte;

                    /* ACK and ready to receive */
                    `$INSTANCE_NAME`_I2C_ACK_AND_RECEIVE;
                }
                else
                {
                    /* Command invalid, Set NACK */
                    `$INSTANCE_NAME`_I2C_NAK_AND_RECEIVE;

                    /* Report user of receiving unsuported command */
                    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_UNSUPORTED_CMD);
                }

                /* Identify that we are in data phase now */
                `$INSTANCE_NAME`_isCmdPhase = `$INSTANCE_NAME`_PHASE_DATA;
            }
            /* Data phase of a transaction */
            else if(`$INSTANCE_NAME`_bufferIndex < `$INSTANCE_NAME`_bufferSize)       /* Check for valid range */
            {
                `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_bufferIndex] = tmpByte;      /* Write data to array */
                `$INSTANCE_NAME`_bufferIndex++;                                       /* Inc pointer */
                `$INSTANCE_NAME`_I2C_ACK_AND_RECEIVE;                                 /* ACK and ready to receive */
            }
            else
            {
                /* Host tries to send more bytes then it is expected */
                `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_WR_TO_MANY_BYTES);
                /* NAK cause beyond write area */
                `$INSTANCE_NAME`_I2C_NAK_AND_RECEIVE;
                /* Set OVERFLOW, write completes on Stop */
                `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_WR_ERR_OVFL;
            }
        }
        /* Data Read from Slave to Master */
        else if(`$INSTANCE_NAME`_I2C_state == `$INSTANCE_NAME`_I2C_SM_SL_RD_DATA)
        {
            if((`$INSTANCE_NAME`_I2C_CHECK_DATA_ACK(tmpCsr)) && (0u != `$INSTANCE_NAME`_cmdReceived))
            {
                if(`$INSTANCE_NAME`_bufferIndex < `$INSTANCE_NAME`_bufferSize)
                {
                     /* Get data from array */
                    `$INSTANCE_NAME`_I2C_DATA_REG = `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_bufferIndex];
                    /* Send Data */
                    `$INSTANCE_NAME`_I2C_TRANSMIT_DATA;
                    /* Inc pointer */
                    `$INSTANCE_NAME`_bufferIndex++;
                }
                else   /* Overflow */
                {
                    /* Master atempts to read to many bytes. This happens when Read config.
                    * set to "None".
                    */
                    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES);
                    `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;                     /* Send 0xFF at the end of the buffer */
                    `$INSTANCE_NAME`_I2C_TRANSMIT_DATA;                                      /* Send Data */
                    `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL; /* Set Overflow */
                }
            }
            else  /* Last byte NAKed, done */
            {
                #if(0u != `$INSTANCE_NAME`_BOOTLOADER_READ_EN)
                   `$INSTANCE_NAME`_btldrStatus |= `$INSTANCE_NAME`_BTLDR_RD_CMPT; 
                #endif /* (0u != `$INSTANCE_NAME`_BOOTLOADER_READ_EN) */
                
                `$INSTANCE_NAME`_cmdReceived = 0u;
                /* Invalidate a stored command code */
                `$INSTANCE_NAME`_lastReceivedCmd = `$INSTANCE_NAME`_CMD_UNDEFINED;
                /* End of read transaction */
                `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;
                /* Clear transmit bit at the end of read transaction */
                `$INSTANCE_NAME`_I2C_NAK_AND_TRANSMIT;

                `$INSTANCE_NAME`_I2C_slStatus &= ~`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;    /* Clear RD_BUSY Flag */
                `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_CMPLT;    /* Set RD_CMPLT Flag */
                `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_IDLE;               /* Return to IDLE state */
            }
        }
        /* This is an invalid state and should not occur */
        else
        {
            /* Invalid state, Reset */
            `$INSTANCE_NAME`_I2C_NAK_AND_RECEIVE;
        }
    }
    else
    {
        /* Nothing should be done here. */
    }

    /* Check if STOP was detected */
    if(`$INSTANCE_NAME`_I2C_CHECK_STOP_STS(tmpCsr))
    {
        /* The write transaction was complete so proceed the data received in
        * this transaction.
        */
        if(`$INSTANCE_NAME`_COMMAND_RECEIVED == `$INSTANCE_NAME`_isValidCmd)
        {
            
            #if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))
            
            /* Bootloader commands can be variable in length so need to handle that */
            if((`$INSTANCE_NAME`_bufferIndex == `$INSTANCE_NAME`_bufferSize) || 
                (`$INSTANCE_NAME`_BOOTLOAD_WRITE == `$INSTANCE_NAME`_lastReceivedCmd))
            {
                `$INSTANCE_NAME`_WriteHandler();            /* Process data received during "Write" transaction */
            }
            
            #else  /* Non bootloader project */
            
            if(`$INSTANCE_NAME`_bufferIndex == `$INSTANCE_NAME`_bufferSize)
            {
                `$INSTANCE_NAME`_WriteHandler();            /* Process data received during "Write" transaction */
            }
            
            #endif /* if defined(CYDEV_BOOTLOADER_IO_COMP) && \
            ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */
            else
            {   
                /* Report that host writes to few bytes and ignore command */
                `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_WR_TO_FEW_BYTES);
            }
        }

        /* Reset command received flag */
        `$INSTANCE_NAME`_cmdReceived = 0u;

        /* Invalidate a stored command code */
        `$INSTANCE_NAME`_lastReceivedCmd = `$INSTANCE_NAME`_CMD_UNDEFINED;

        /* The Write transaction only IE on STOP, so Read never gets here.
        * The WR_BUSY flag will be cleared at the end of "Write-ReStart-Read-Stop"
        * transaction.
        */
        `$INSTANCE_NAME`_I2C_slStatus &= ~`$INSTANCE_NAME`_I2C_SSTAT_WR_BUSY;    /* Clear WR_BUSY Flag */
        `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_WR_CMPLT;    /* Set WR_CMPT Flag */

        `$INSTANCE_NAME`_I2C_DISABLE_INT_ON_STOP;                   /* Disable interrupt on STOP */
        `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_IDLE;  /* Return to IDLE */
    }
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TIMEOUT_ISR
********************************************************************************
*
* Summary:
*  Handles bus timeout occurence event.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_TIMEOUT_ISR)
{
    `$INSTANCE_NAME`_ResetBus();
}


/* [] END OF FILE */
