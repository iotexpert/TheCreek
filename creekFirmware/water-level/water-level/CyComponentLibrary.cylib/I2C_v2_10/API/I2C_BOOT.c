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
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))

/***************************************
*    Bootloader Variables
***************************************/

/* Writes to this buffer */
uint8 `$INSTANCE_NAME`_slReadBuf[`$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER];

/* Reads from this buffer */
uint8 `$INSTANCE_NAME`_slWriteBuf[`$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER];


/***************************************
*    Extern Bootloader Variables
***************************************/

/* Receive buffer index */
extern volatile uint8 `$INSTANCE_NAME`_slRdBufIndex;    /* Slave Transmit buffer Index */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommStart
********************************************************************************
*
* Summary:
*  Starts the communication component and enables the interrupt.
*
* Parameters:
*  None
*
* Return:
*  None 
*
* Side Effects:
*  This component automatically enables it's interrupt. If I2C is enabled
*  without the interrupt enabled, it could lock up the I2C bus.
*  The read buffer initial state is full and the read always is 0xFFu.
* 
* Global variables:
*  `$INSTANCE_NAME`_slWriteBuf - used to store received command.
*  `$INSTANCE_NAME`_slReadBuf - used to store response.
*  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommStart(void)
{   
    /* Init I2C hardware */
    `$INSTANCE_NAME`_Init();

    /* Set Write buffer */
    `$INSTANCE_NAME`_SlaveInitWriteBuf(`$INSTANCE_NAME`_slWriteBuf, `$INSTANCE_NAME`_BTLDR_SIZEOF_WRITE_BUFFER);
    
    /* Make the Read buffer full */
    `$INSTANCE_NAME`_SlaveInitReadBuf(`$INSTANCE_NAME`_slReadBuf, `$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER);
    `$INSTANCE_NAME`_slRdBufIndex = `$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER;
    
    /* Enable power to I2C Module */
    `$INSTANCE_NAME`_Enable();
    
    /* Enable interrupt to start operation */
    `$INSTANCE_NAME`_EnableInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommStop
********************************************************************************
*
* Summary:
*  Disable the communication component and disable the interrupt.
*
* Parameters:
*  None 
*
* Return:
*  None 
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommStop(void) 
{
    /* Disable Interrupt */
    `$INSTANCE_NAME`_DisableInt();

    /* Stop I2C component */
    `$INSTANCE_NAME`_Stop();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommReset
********************************************************************************
*
* Summary:
*  Set buffers to the initial state and reset the status.
*
* Parameters:
*  None 
*
* Return:
*  None
*
* Side Effects:
*  The read buffer initial state is full and on address reception the clock is
*  stretching if the buffer hasn't updated.
*  
* Global variables:
*  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
void `$INSTANCE_NAME`_CyBtldrCommReset(void)
{
    /* Reset Write buffer */
    `$INSTANCE_NAME`_SlaveClearWriteBuf();
    
    /* Make the Read buffer full */
    `$INSTANCE_NAME`_slRdBufIndex = `$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER;
    
    /* Clear read and write status */
    `$INSTANCE_NAME`_SlaveClearReadStatus();
    `$INSTANCE_NAME`_SlaveClearWriteStatus();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommWrite
********************************************************************************
*
* Summary:
*  Transmits the status of executed command to the Host. 
*  On address reception the clock is stretching if the buffer hasn't updated.
*  If this occurs this function updates the buffer and sends one byte releasing 
*  the bus. All other bytes are transmitted by ISR.
*  After exist this function the ISR stretches the clock on address reception.
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
* Side Effects:
*  The read buffer after execution of this function is full and 0xFF is the answer
*  till buffer will be updated with Answer.
*
* Global variables:
*  `$INSTANCE_NAME`_slReadBuf - used to store response.
*  `$INSTANCE_NAME`_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_CyBtldrCommWrite(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut)
{
    cystatus status = CYRET_EMPTY;
    uint16 timeoutMs = 10u * TimeOut;    /* To be in 10mS units, really check 1uS * 1000 * 10 */
    uint16 i;
    
    /* Initialize buffer */
    for (i = 0; i < Size; i++)
    {
        `$INSTANCE_NAME`_slReadBuf[i]= Data[i];    /* copy Answer to I2C buffer */
    }
    
    /* Free buffer to start with Answer */
    `$INSTANCE_NAME`_SlaveClearReadBuf();
  
    /* Wait till Answer will be read */
    while (timeoutMs-- > 0)
    {
        /* Check if Host complete a read */
        if((`$INSTANCE_NAME`_SlaveGetReadBufSize() >= Size) &&
           (`$INSTANCE_NAME`_SlaveStatus() & `$INSTANCE_NAME`_SSTAT_RD_BUSY) == 0u)
        {
            /* How many Host has been read */
            *Count = `$INSTANCE_NAME`_SlaveGetReadBufSize();
            
            /* Make read buffer to be full */
            `$INSTANCE_NAME`_slRdBufIndex = `$INSTANCE_NAME`_BTLDR_SIZEOF_READ_BUFFER;
            status = CYRET_SUCCESS;
            
            break;
        }
        
        CyDelay(1); /* Wait 1mS for data to become available */
    }
    
    return status;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CyBtldrCommRead
********************************************************************************
*
* Summary:
*  Receives the command from the Host.
*  All bytes receive by the ISR and stores in internal buffer. The function 
*  controls status end of transfer and copy data to bootloader buffer.
*  After exist this function the ISR is able to receive more data. 
*
* Parameters:  
*  Data:     pointer to data buffer to store command.
*  Size:     number of bytes required to be received.
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
cystatus `$INSTANCE_NAME`_CyBtldrCommRead(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut)
{
    cystatus status = CYRET_EMPTY;
    uint16 timeoutMs = 10u * TimeOut;   /* To be in 10mS units, really check 1mS * 10 */
    uint16 i;
    
    /* Wait for Command from Host */
    while (timeoutMs-- > 0)
    {
        /* Check if Host complete write */
        if((`$INSTANCE_NAME`_SlaveStatus() & `$INSTANCE_NAME`_SSTAT_WR_CMPT) != 0u)
        {
            /* How many bytes Host has been written */
            *Count = `$INSTANCE_NAME`_SlaveGetWriteBufSize();
            
            /* Copy to bootloader buffer */
            for (i = 0;((i < *Count) && (i < Size)); i++)
            {
                Data[i] = `$INSTANCE_NAME`_slWriteBuf[i];   /* Copy received data */
            }
            
            /* Clear I2C write buffer and status */
            `$INSTANCE_NAME`_SlaveClearWriteStatus();
            `$INSTANCE_NAME`_SlaveClearWriteBuf();
            
            status = CYRET_SUCCESS;
            break;
        }
        
        CyDelay(1); /* Wait 1mS for data to become available */
    }
    
    return status;
}

#endif  /* defined(CYDEV_BOOTLOADER_IO_COMP) && 
                 ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`) || \
                  (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */
/* [] END OF FILE */
