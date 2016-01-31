/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveStatus
********************************************************************************
* Summary:
*  Returns status of the I2C status register. 
*
* Parameters:  
*  void                            
*
* Return: 
*  Returns status of I2C slave status register.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SlaveStatus(void) 
{
    return( `$INSTANCE_NAME`_slStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveClearReadStatus
********************************************************************************
* Summary:
*  Clears the read status bits in the I2C_RsrcStatus register and returns read
*  status.  No other bits are affected.
*
* Parameters:  
*  void
*
* Return: 
*  Return the read status.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SlaveClearReadStatus(void) 
{
    uint8 status;

    status = `$INSTANCE_NAME`_slStatus & `$INSTANCE_NAME`_SSTAT_RD_MASK;

    /* Mask of transfer complete flag and Error status  */
    `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_CLEAR;
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveClearWriteStatus
********************************************************************************
* Summary:
*  Clears the write status bits in the I2C_Status register and returns write
*  status.  No other bits are affected.
*
* Parameters:  
*  void
*
* Return: 
*  Return the write status.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SlaveClearWriteStatus(void)
{
    uint8 status;

    status = `$INSTANCE_NAME`_slStatus & `$INSTANCE_NAME`_SSTAT_WR_MASK;

    /* Mask of transfer complete flag and Error status  */
    `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_CLEAR;
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveSetAddress
********************************************************************************
* Summary:
*  Sets the address for the first device.
*
* Parameters:  
*  (uint8) address:  The slave adderss for the first device.          
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlaveSetAddress(uint8 address) 
{
    #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_SW_DECODE)       
        `$INSTANCE_NAME`_Address = address & 0x7F;  /* Set Address variable */
    #else
        `$INSTANCE_NAME`_ADDRESS = address & 0x7F;   /* Set I2C Address register */
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveSetSleepMode
********************************************************************************
* Summary:
*  Disables the run time EzI2C and enables the sleep Slave I2C.  Should be
*  called just prior to entering sleep.  Only generated if fixed I2C hardware
*  is used.
*
* Parameters:  
*  void  
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlaveSetSleepMode(void) 
{
    /* Component function code  */
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveSetWakeMode
********************************************************************************
* Summary:
*  Disables the sleep EzI2C slave and re-enables the run time I2C.  Should be
*  called just after awaking from sleep.  Must preserve address to continue.  
*  Only generated if fixed I2C hardware is used.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlaveSetWakeMode(void) 
{
    /* Component function code  */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlavePutReadByte
********************************************************************************
* Summary:
*  - For Master Read, sends 1 byte out Slave transmit buffer.
*    Wait to send byte until buffer has room.  Used to preload
*    the transmit buffer.
*
*  - In byte by byte mode if the last byte was ACKed, stall the master
*    (on the first bit of the next byte) if needed until the next byte
*    is PutChared.  If the last byte was NAKed it does not stall the bus
*    because the master will generate a stop or restart condition.
*
*
* Parameters:  
*  (uint8) transmitDataByte: Byte containing the data to transmit.
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlavePutReadByte(uint8 transmitDataByte) 
{
    /* Load the data. */
    `$INSTANCE_NAME`_TX_DATA = transmitDataByte;

    /* Make sure NACK is clear. */
    `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_NACK_MASK); 

    /* Set the Transmit bit. */
    `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK);
    
    /* Now toggle GO. */
    `$INSTANCE_NAME`_GO = 0x00;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveGetWriteByte
********************************************************************************
* Summary:
*  - For a Master Write, ACKs or NAKs the previous byte and reads out the last
*    byte tranmitted.  The first byte read of a packet is the Address byte in 
*    which case there is no previous data so no ACK or NAK is generated.  The
*    bus is stalled until the next GetByte, therefore a GetByte must be executed
*    after the last byte in order to send the final ACK or NAK before the Master
*    can send a Stop or restart condition.
*
*
* Parameters:  
*  ackNak: 1 = ACK, 0 = NAK for the previous byte received.
*
* Return: 
*  Last byte transmitted or last byte in buffer from Master.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SlaveGetWriteByte(uint8 ackNak) 
{
    uint8 dataByte;


    dataByte = `$INSTANCE_NAME`_DATA;

    if(ackNak == `$INSTANCE_NAME`_ACK_DATA)
    {
        /* Make sure Transmit and NACK are clear. */
        `$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK));
    
        /* Now toggle GO. */
        `$INSTANCE_NAME`_GO = 0x00;
    }
    else
    {
        /* Make sure Transmit is clear. */
        `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK);

        /* Set the NACK bit. */
        `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_NACK_MASK;

        /* Now toggle GO. */
        `$INSTANCE_NAME`_GO = 0x00;
    }

    return dataByte;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveInitReadBuf
********************************************************************************
* Summary:
*  This function sets up the buffer in which data will be read by the 
*  Master.  The buffer index will be reset to zero and the status flags
*  will be cleared with this command.
*
*
* Parameters:  
*  readBuf:    Pointer to the array to be sent to the Slave transmit register.
*  bufSize:    Size of the buffer to transfer.
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*     If this function is called during a bus transaction, data from the 
*     previous buffer location and the beginning of this buffer may be
*     transmitted.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlaveInitReadBuf(uint8 * readBuf, uint8 bufSize) 
{
    `$INSTANCE_NAME`_readBufPtr    = readBuf;
    `$INSTANCE_NAME`_readBufIndex  = 0u;
    `$INSTANCE_NAME`_readBufSize   = bufSize;  /* Set buffer size */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveInitWriteBuf
********************************************************************************
* Summary:
*  This function initializes the write buffer.  The write buffer is the array
*  that is written to when the master performs a write operation.
*
* Parameters:  
*   writeBuf:  Pointer to the array used to store the data written by the Master 
*              and read by the Slave.
*   bufSize:   Size of buffer to receive data from master.
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlaveInitWriteBuf(uint8 * writeBuf, uint8 bufSize) 
{
    `$INSTANCE_NAME`_writeBufPtr    = writeBuf;
    `$INSTANCE_NAME`_writeBufIndex  = 0u;
    `$INSTANCE_NAME`_writeBufSize   = bufSize;  /* Set buffer size */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveGetReadBufSize
********************************************************************************
* Summary:
*  Returns the count of bytes read by the Master since the buffer was reset.
*  The maximum return value will be the size of the buffer.
*
* Parameters:  
*   void
*
* Return: 
*  (uint8)  Bytes read by Master.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SlaveGetReadBufSize(void) 
{
    return `$INSTANCE_NAME`_readBufIndex;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveGetWriteBufSize
********************************************************************************
* Summary:
*  Returns the count of bytes written by the I2C Master.  The maximum value
*  that will be returned in the buffer size itself.
*
* Parameters:  
*   void
*
* Return: 
*  The valid number of bytes in Tx buffer.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SlaveGetWriteBufSize(void)
{
   return `$INSTANCE_NAME`_writeBufIndex;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveClearReadBuf
********************************************************************************
* Summary:
*  Sets the buffer read buffer index back to zero.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlaveClearReadBuf(void)
{
    `$INSTANCE_NAME`_readBufIndex = 0;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SlaveClearRxBuf
********************************************************************************
* Summary:
*  Sets the I2C write buffer index to 0.
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SlaveClearWriteBuf(void)
{
    `$INSTANCE_NAME`_writeBufIndex = 0;
}


