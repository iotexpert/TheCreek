/*******************************************************************************
* File Name: `$INSTANCE_NAME`_BOOT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of bootloader communication APIs for the
*  SM/PM Bus Slave component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_cmd.h"


#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))

/***************************************
*    Bootloader Variables
***************************************/

/* Write to this buffer. `$INSTANCE_NAME`_commands[x].dataPtr will
* point to this buffer.
*/
uint8 `$INSTANCE_NAME`_btldrReadBuf[`$INSTANCE_NAME`_MAX_BUFFER_SIZE];

/* Read from this buffer. `$INSTANCE_NAME`_commands[y].dataPtr will
* point to this buffer.
*/
uint8 `$INSTANCE_NAME`_btldrWriteBuf[`$INSTANCE_NAME`_MAX_BUFFER_SIZE];

uint8 `$INSTANCE_NAME`_btldrStatus;

uint8 `$INSTANCE_NAME`_btldrWrBufByteCount;
uint8 `$INSTANCE_NAME`_btldrRdBufByteCount;

/***************************************
*    Extern Bootloader Variables
***************************************/
extern volatile uint8 `$INSTANCE_NAME`_slStatus;           /* Slave Status */
extern volatile uint8 `$INSTANCE_NAME`_I2C_state;          /* Current state of I2C state machine */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommStart
********************************************************************************
*
* Summary:
*  Starts the communication component and enables the interrupt.
*  The read buffer initial state is full and the read always is 0xFFu.
*  The write buffer is clear and ready to receive a command.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  This fucntion enables component interrupt. If I2C is enabled
*  without the interrupt enabled, it could lock up the I2C bus.
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommStart(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStart")`
{
    /* Enable power to I2C Module */
    `$INSTANCE_NAME`_Start();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommStop
********************************************************************************
*
* Summary:
*  Disables the communication component and disables the interrupt.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommStop(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStop")`
{
    /* Stop I2C component */
    `$INSTANCE_NAME`_Stop();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommReset
********************************************************************************
*
* Summary:
*  Set buffers to the initial state and reset the statuses.
*  The read buffer initial state is full and the read always is 0xFFu.
*  The write buffer is clear and ready to receive a command.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommReset(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommReset")`
{
    `$INSTANCE_NAME`_btldrRdBufByteCount = 0u;
    `$INSTANCE_NAME`_btldrWrBufByteCount = 0u;
    `$INSTANCE_NAME`_btldrStatus = 0u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommWrite
********************************************************************************
*
* Summary:
*  Transmits the status of executed command to the Host.
*  The function updates the I2C read buffer with response and realeases it to
*  the host. All reads return 0xFF till the buffer will be released. All bytes
*  are transfered by the I2C ISR.
*  The function waits with timeout till all bytes will be read.
*  After exist this function the reads return 0xFF.
*
* Parameters:
*  Data:     pointer to data buffer with response command.
*  size:     number of bytes required to be transmitted.
*  count:    actual size of data was transmitted.
*  timeOut:  timeout value in tries of 10mS.
*
* Return:
*  Status of transmit operation.
*   CYRET_EMPTY   - in case data wasn't send.
*   CYRET_SUCCESS - in case data was sent.
*
* Theory:
*  This function should be called after command was received to unblock the bus.
*  The Exit Bootloader doesn't require this.
*  
* Side Effects:
*  Temporary enables I2C interrupt when called on a start of clock stretching
*  during "Bootloader Write" transaction but disables the interrupt before
*  return from this function (to meed manual command handling behaviour). When 
*  called not using "Bootloader Write" - always leaves I2C interrupt enabled.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_CyBtldrCommWrite(uint8 * Data, uint16 size, uint16 * count, uint8 timeOut) CYSMALL
         `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommWrite")`
{
    uint16 i;
    cystatus status = CYRET_EMPTY;
    uint16 timeOutms = 10u * timeOut;    /* To be in 10mS units, really check 1mS*10 */    

    /* There are two timing possibilities upon entering this routine:
    *    1) We have arrived here prior to receiving a PMBUS_CMD_CUSTOM_BOOTLOAD_READ command
    *    2) The BOOTLOAD_READ command has already arrived.  We may or may not yet be clock
    *       stretching on the the read that follows it. We have missed the
    *       opportunity to have the pmbus readhandler copy the data for us, so it needs to 
    *       be done manually.
    */
    
    /* Disable the PMBus interrupt and figure out which case applies. */    
    `$INSTANCE_NAME`_DisableInt();
    
    /* Clear the read done flag */
    `$INSTANCE_NAME`_btldrStatus &= ~`$INSTANCE_NAME`_BTLDR_RD_CMPT;

    /* Currently clock stretching AND handling a BOOTLOAD_READ command*/
    if (`$INSTANCE_NAME`_I2C_state == `$INSTANCE_NAME`_I2C_SM_SL_RD_DATA &&
        `$INSTANCE_NAME`_lastReceivedCmd == `$INSTANCE_NAME`_BOOTLOAD_READ) 
    {
        /* copy the data directly to the PMBus buffer since it is too late to use the pmbus readhandler*/
        `$INSTANCE_NAME`_buffer[0u] = size;
             
        for (i = 0u; i < size; i++)
        {
            `$INSTANCE_NAME`_buffer[i + 1u] = Data[i];
        }
        
        `$INSTANCE_NAME`_bufferSize = size + 1u;
        
        /* Now manually load the 1st byte */
        `$INSTANCE_NAME`_I2C_DATA_REG = `$INSTANCE_NAME`_buffer[0u];
        /* Let ISR to proceed starts with 2nd */
        `$INSTANCE_NAME`_bufferIndex = 1u;
        /* Transmit the 1st byte manual and release the bus */
        /* ACK and transmit */
        `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;
    }
    else
    /* BOOTLOAD_READ command hasn't arrived yet. Just pre-load the pmbus bottloader
    * array and let the automated PMBus code take care of everything */
    {
        `$INSTANCE_NAME`_btldrReadBuf[0u] = size;
        
        for(i = 0u; i < size; i++)
        {
            `$INSTANCE_NAME`_btldrReadBuf[i + 1u] = Data[i];
        }
        `$INSTANCE_NAME`_btldrRdBufByteCount = size + 1u;
    }
    
    `$INSTANCE_NAME`_EnableInt();
    
    /* Process the buffer */
    while(timeOutms-- > 0u)
    {
        /* wait for the transaction to complete */

        if (0u != (`$INSTANCE_NAME`_btldrStatus & `$INSTANCE_NAME`_BTLDR_RD_CMPT))
        {
            *count = (`$INSTANCE_NAME`_bufferIndex - 1u);
            /* Clear the read done flag */
            `$INSTANCE_NAME`_btldrStatus &= ~`$INSTANCE_NAME`_BTLDR_RD_CMPT;
            `$INSTANCE_NAME`_btldrRdBufByteCount = 0u;
            
	        status = CYRET_SUCCESS;
            break;
        }
        
        CyDelay(1u); /* Wait 1mS for data to become available */
    }

    /* Always clear buffer byte count when leaving this function.
    * If the bootloader host hasn't fetched this data yet, we need to 
    * (effectively) delete it. 
    */
    `$INSTANCE_NAME`_btldrRdBufByteCount = 0u;

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommRead
********************************************************************************
*
* Summary:
*  Receives the command from the Host.
*  All bytes are received by the I2C ISR and stored in internal I2C buffer. The
*  function checks status with timeout to detemine the end of transfer and
*  then copy data to bootloader buffer.
*  After exist this function the I2C ISR is able to receive more data.
*
* Parameters:
*  Data:     pointer to data buffer to store command.
*  size:     maximum number of bytes which could to be passed back.
*  count:    actual size of data was received.
*  timeOut:  timeout value in tries of 10mS.
*
* Return:
*  Status of receive operation.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_CyBtldrCommRead(uint8 * Data, uint16 size, uint16 * count, uint8 timeOut) CYSMALL
         `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommRead")`
{
    cystatus status = CYRET_BAD_PARAM;   /* Initialize as bad parameters */
    uint16 timeOutms = 10u * timeOut;    /* To be in 10mS units, really check 1mS*10 */   
    uint8 i;
    
    while (timeOutms-- > 0u)
    {
        /* Wait for SM/PM Bus master to complete a write */
        if(0u != (`$INSTANCE_NAME`_btldrStatus & `$INSTANCE_NAME`_BTLDR_WR_CMPT))
        {
            *count = `$INSTANCE_NAME`_btldrWrBufByteCount;
                    
            for (i = 0u;((i < *count) && (i < size)); i++)
            {
                Data[i] = `$INSTANCE_NAME`_btldrWriteBuf[i];    /* Transfer data */
            }
            
            /* Clear the write complete bit */
            `$INSTANCE_NAME`_btldrStatus &= ~`$INSTANCE_NAME`_BTLDR_WR_CMPT;
            `$INSTANCE_NAME`_btldrWrBufByteCount = 0u;
            
            status = CYRET_SUCCESS;
            break;
        }
        
        CyDelay(1u); /* Wait 1mS for data to become available */
    }

    return(status);
}

#endif /* if defined(CYDEV_BOOTLOADER_IO_COMP) && \
            ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */


/* [] END OF FILE */
