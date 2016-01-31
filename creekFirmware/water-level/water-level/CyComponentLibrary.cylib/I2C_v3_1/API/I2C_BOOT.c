/*******************************************************************************
* File Name: `$INSTANCE_NAME`_BOOT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of bootloader communication APIs for the 
*  I2C component.
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

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (0u != (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE))  && \
                                                ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                                 (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))

/***************************************
*    Bootloader Variables
***************************************/

/* Writes to this buffer */
uint8 XDATA `$INSTANCE_NAME`_slReadBuf[`$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER];

/* Reads from this buffer */
uint8 XDATA `$INSTANCE_NAME`_slWriteBuf[`$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER];


/***************************************
*    Extern Bootloader Variables
***************************************/

extern volatile uint8 `$INSTANCE_NAME`_slStatus;            /* Slave Status  */

/* Transmit buffer variables */
extern volatile uint8   `$INSTANCE_NAME`_slRdBufSize;       /* Slave Transmit buffer size */
extern volatile uint8   `$INSTANCE_NAME`_slRdBufIndex;      /* Slave Transmit buffer Index */

/* Receive buffer variables */
extern volatile uint8   `$INSTANCE_NAME`_slWrBufSize;       /* Slave Receive buffer size */
extern volatile uint8   `$INSTANCE_NAME`_slWrBufIndex;      /* Slave Receive buffer Index */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommStart
********************************************************************************
*
* Summary:
*  Starts the communication component and enables the interrupt.
*  The read buffer initial state is full and the read always is 0xFFu.
*  The write buffer is clear and ready to receive a commmand.
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
* Global variables:
*  `$INSTANCE_NAME`_slWriteBuf - used to store received command.
*  `$INSTANCE_NAME`_slReadBuf - used to store response.
*  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommStart(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommStart")`
{
    /* Set Write buffer */
    `$INSTANCE_NAME`_SlaveInitWriteBuf(`$INSTANCE_NAME`_slWriteBuf, `$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER);
    
    /* Set Read buffer and make it full */
    `$INSTANCE_NAME`_SlaveInitReadBuf(`$INSTANCE_NAME`_slReadBuf, 0u);
    
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
*  The write buffer is clear and ready to receive a commmand.
*
* Parameters:
*  None 
*
* Return:
*  None
*  
* Global variables:
*  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommReset(void) CYSMALL `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommReset")`
{
    /* Make the Read buffer full */
    `$INSTANCE_NAME`_slRdBufSize = 0u;
    
    /* Reset Write buffer and Read buffer */
    `$INSTANCE_NAME`_slRdBufIndex = 0u;
    `$INSTANCE_NAME`_slWrBufIndex = 0u;
       
    /* Clear read and write status */
    `$INSTANCE_NAME`_slStatus = 0u;
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
*  Size:     number of bytes required to be transmitted.
*  Count:    actual size of data was transmitted.
*  TimeOut:  timeout value in tries of 10uS.
*
* Return:
*  Status of transmit operation.
* 
* Global variables:
*  `$INSTANCE_NAME`_slReadBuf - used to store response.
*  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_CyBtldrCommWrite(uint8 * Data, uint16 size, uint16 * count, uint8 timeOut) CYSMALL 
         `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommWrite")`
{
    cystatus status = CYRET_BAD_PARAM;  /* Initialize as bad parameters */
    uint16 timeoutMs;                   /* Timeout in mS */
    
    /* Check that correct buffer is provided by bootloader */
    if ((NULL != Data) && (size > 0u))
    {
        timeoutMs = 10u * timeOut;  /* To be in 10mS units, really check 1mS * 10 */
        status = CYRET_TIMEOUT;     /* Fail due timeout */
        *count = size;              /* The size only be transmitted, all other will be 0xFFu */
        
        /* Copy response to the buffer */
        memcpy(`$INSTANCE_NAME`_slReadBuf, Data, size);
        
        /* The buffer is free now */
        `$INSTANCE_NAME`_slRdBufSize = (uint8) size;
        
        /* Wait till response will be read */
        while (0u != timeoutMs--)
        {
            /* Check if host complete a reading */
            if (`$INSTANCE_NAME`_slRdBufIndex == size)
            {
                `$INSTANCE_NAME`_slRdBufSize = 0u;
                `$INSTANCE_NAME`_slRdBufIndex = 0u;
                
                status = CYRET_SUCCESS;
                break;
            }
            
            CyDelay(1u); /* Wait 1mS for data to become available */
        }
    }
    
    return (status);
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
*  Size:     maximum number of bytes which could to be passed back.
*  Count:    actual size of data was received.
*  TimeOut:  timeout value in tries of 10uS.
*
* Return:
*  Status of receive operation.
*
* Global variables:
*  `$INSTANCE_NAME`_slWriteBuf - used to store received command.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_CyBtldrCommRead(uint8 * Data, uint16 size, uint16 * count, uint8 timeOut) CYSMALL 
         `=ReentrantKeil($INSTANCE_NAME . "_CyBtldrCommRead")`
{
    cystatus status = CYRET_BAD_PARAM;  /* Initialize as bad parameters */
    uint16 timeoutMs;                   /* Timeout in mS */
    uint8 byteCount;                    /* Number of bytes that the host has been written */
    
    /* Check that correct buffer is provided by bootloader */
    if ((NULL != Data) && (size > 0u))
    {
        timeoutMs = 10u * timeOut;  /* To be in 10mS units, really check 1mS * 10 */
        status = CYRET_TIMEOUT;     /* Fail due timeout */
    
        /* Wait for Command from the host */
        while (0u != timeoutMs--)
        {
            /* Check if the host complete write */
            if (0u != (`$INSTANCE_NAME`_slStatus & `$INSTANCE_NAME`_SSTAT_WR_CMPLT))
            {
                /* How many bytes the host has been written */
                byteCount = `$INSTANCE_NAME`_slWrBufIndex;
                *count = (uint16) byteCount;
                
                /* Copy to command for the host to bootloader buffer */
                memcpy(Data, `$INSTANCE_NAME`_slWriteBuf,  `$INSTANCE_NAME`_MIN_UINT16(byteCount, size));
                
                /* Clear I2C write buffer and status */
                `$INSTANCE_NAME`_slStatus = 0u;
                `$INSTANCE_NAME`_slWrBufIndex = 0u;
                
                status = CYRET_SUCCESS;
                break;
            }
            
            CyDelay(1u); /* Wait 1mS for data to become available */
        }
    }
    
    return (status);
}

#endif /* End if (CYDEV_BOOTLOADER_IO_COMP) && (`$INSTANCE_NAME`_MODE == `$INSTANCE_NAME`_MODE_SLAVE) && \
                                               ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                                (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */


/* [] END OF FILE */
