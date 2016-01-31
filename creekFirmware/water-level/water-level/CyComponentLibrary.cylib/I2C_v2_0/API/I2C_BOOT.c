/*******************************************************************************
* File Name: `$INSTANCE_NAME`_BOOT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the setup, control and status commands for the I2C
*    component.  Actual protocol and operation code resides in the interrupt
*    service routine file.
*
*   Note: 
*
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h" 

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`)
/***************************************
*    Bootloader Variables
***************************************/

/* We write to this */
uint8 slReadBuf[BTLDR_SIZEOF_READ_BUFFER];

/* We read from this */
uint8 slWriteBuf[BTLDR_SIZEOF_WRITE_BUFFER];


/***************************************
*    Extern Bootloader Variables
***************************************/

/* Slave Status and I2C State variables */
extern volatile uint8 `$INSTANCE_NAME`_slStatus;    /* Slave Status  */   
extern volatile uint8 `$INSTANCE_NAME`_State;       /* Current state of I2C state machine */
extern volatile uint8 `$INSTANCE_NAME`_Status;      /* Status byte */   

/* Transmit and Receive buffer variable */
extern uint8 * `$INSTANCE_NAME`_readBufPtr;         /* Pointer to Transmit buffer */       
extern uint8 * `$INSTANCE_NAME`_writeBufPtr;        /* Pointer to Receive buffer */
extern volatile uint8 `$INSTANCE_NAME`_readBufIndex;/* Slave Transmit buffer Index */


/*******************************************************************************
* Function Name: CyBtldrCommStart
********************************************************************************
*
* Summary:
*  Starts the component and enables the interupt.  
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Side Effects:
*   This component automatically enables it's interrupt.  If I2C is enabled
*   without the interrupt enabled, it could lock up the I2C bus.
*
*******************************************************************************/
void CyBtldrCommStart(void)
{        
    /* Init I2C hardware */
    `$INSTANCE_NAME`_Init();

    /* Set Write buffer */
    `$INSTANCE_NAME`_SlaveInitWriteBuf(slWriteBuf, BTLDR_SIZEOF_WRITE_BUFFER);
    
    /* Make the Read buffer full */
    `$INSTANCE_NAME`_SlaveInitReadBuf(slReadBuf, BTLDR_SIZEOF_READ_BUFFER);
    `$INSTANCE_NAME`_readBufIndex = BTLDR_SIZEOF_READ_BUFFER;
    
    /* Enable power to I2C Module */
    `$INSTANCE_NAME`_Enable();
    
    /* Enable interrupt to start operation */
    `$INSTANCE_NAME`_EnableInt();
}


/*******************************************************************************
* Function Name: CyBtldrCommStop
********************************************************************************
*
* Summary:
*  Disable the component and disable the interrupt.
*
* Parameters:  
*  void 
*
* Return: 
*  void 
*
*******************************************************************************/
void CyBtldrCommStop(void) 
{
    /* Disable Interrupt */
    `$INSTANCE_NAME`_DisableInt();

    /* Stop I2C component */
    `$INSTANCE_NAME`_Stop();
}


/*******************************************************************************
* Function Name: CyBtldrCommReset
********************************************************************************
*
* Summary:
*  Set buffers to the initial state and reset the status.
*
* Parameters:  
*  void 
*
* Return: 
*  void 
*
* Side Effects:
*  
*******************************************************************************/
void CyBtldrCommReset(void)
{
    /* Reset Write buffer */
    `$INSTANCE_NAME`_SlaveClearWriteBuf();
    
    /* Make the Read buffer full */
    `$INSTANCE_NAME`_readBufIndex = BTLDR_SIZEOF_READ_BUFFER;
    
    /* Clear read and write status */
    `$INSTANCE_NAME`_SlaveClearReadStatus();
    `$INSTANCE_NAME`_SlaveClearWriteStatus();
}


/*******************************************************************************
* Function Name: CyBtldrCommWrite
********************************************************************************
*
* Summary:
*  Transmits the command. The I2C Address phase handled by ISR and straching the 
*  clock, till the data won't be provided by this function.
*  The provided buffer is re-used as I2C buffer to transmit data.
*
* Parameters:  
*  (uint8 *) Data: pointer to data buffer with response commnad.
*  (uint16) Size: number of bytes required to be transmited.
*  (uint16) * Count: actual size of data was transmited.
*  (uint8) TimeOut: timeout value in tries of 10uS.
*
* Return: 
*  void 
*
* Side Effects:
*  This function should be called after command was received to unblock the bus.
*  The Exit Bootloader doesn't require this.
*
*******************************************************************************/
cystatus CyBtldrCommWrite(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut)
{
    uint16 i;
    cystatus status = CYRET_EMPTY;
    uint16 timeOutms = 10u * TimeOut;    /* To be in 10mS units, really check 1mS*10 */    
    
    /* Initialize buffer */
    for (i = 0; i < Size; i++)
    {
        slReadBuf[i]= Data[i];    /* Transfer data */
    }
    
    /* Disable Interrupt to properly handle clock stretching */
    `$INSTANCE_NAME`_DisableInt();
    
    if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_RD_DATA) /* Currently clock stretching */
    {
        /* Load 1st byte */
        `$INSTANCE_NAME`_DATA_REG = slReadBuf[0u];
        /* Let ISR to proceed starts with 2nd */
        `$INSTANCE_NAME`_readBufIndex = 1u;
        /* Transmit the 1st byte manual and release the bus */
        `$INSTANCE_NAME`_TRANSMIT_DATA;
    }
    else /* Currenly NOT clock stretching, but probably could occur while we here.
            Disabling of I2C ISR make us safe when clears the buffer */
    {
        `$INSTANCE_NAME`_readBufIndex = 0u;
    }
    
    /* Let ISR proceeds transaction */
    `$INSTANCE_NAME`_EnableInt();
    
    /* Process the buffer */
    while (timeOutms-- > 0)
    {
        /* Wait for I2C master to complete a write, 
           The host should read use one transaction - Start - data - Stop */
        if (`$INSTANCE_NAME`_SlaveStatus() & `$INSTANCE_NAME`_SSTAT_RD_CMPT)
        {            
            *Count = `$INSTANCE_NAME`_SlaveGetReadBufSize();
            /* Make buffer full */
            `$INSTANCE_NAME`_SlaveClearReadStatus();
            `$INSTANCE_NAME`_readBufIndex = BTLDR_SIZEOF_READ_BUFFER;
            
	        status = CYRET_SUCCESS;
            break;
        }
        
        CyDelay(1); /* Wait 1mS for data to become available */
    }
    
    return status;
}


/*******************************************************************************
* Function Name: CyBtldrCommRead
********************************************************************************
*
* Summary:
*  Receives the command. After exist this function the ISR is able to receive more 
*  data. The internal I2C buffer is used receive data.
*
* Parameters:  
*  (uint8 *) Data: pointer to data storage.
*  (uint16) Size: number of bytes required to be read.
*  (uint16) * Count: actual size of data was read.
*  (uint8) TimeOut: timeout value in tries of 10uS.
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
cystatus CyBtldrCommRead(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut)
{
    uint16 i;
    uint16 timeOutms;
        
    cystatus status = CYRET_EMPTY;
    
    /* To be in 10mS units, really check 1mS*10 */    
    timeOutms = 10u * TimeOut;
    
    while (timeOutms-- > 0)
    {
        /* Wait for I2C master to complete a write */
        if (`$INSTANCE_NAME`_SlaveStatus() & `$INSTANCE_NAME`_SSTAT_WR_CMPT)
        {
            *Count = `$INSTANCE_NAME`_SlaveGetWriteBufSize();
                    
            for (i = 0;((i < *Count) && (i < Size)); i++)
            {
                Data[i] = slWriteBuf[i];    /* Transfer data */
            }
            
            `$INSTANCE_NAME`_SlaveClearWriteStatus();
            `$INSTANCE_NAME`_SlaveClearWriteBuf();
            
            status = CYRET_SUCCESS;
            break;
        }
        
        CyDelay(1); /* Wait 1mS for data to become available */
    }
	
	return status;
}


#endif  /* End defined(CYDEV_BOOTLOADER_IO_COMP) && (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`) */
    
    
/* [] END OF FILE */
