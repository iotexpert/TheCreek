/*******************************************************************************
* File Name: i2c_I2C_SLAVE.c
* Version 1.10
*
* Description:
*  This file provides the source code to the API for the SCB Component in
*  I2C Slave mode.
*
* Note:
*
*******************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "i2c_I2C_PVT.h"

#if(i2c_I2C_SLAVE_CONST)

/***************************************
*      I2C Slave Private Vars
***************************************/

volatile uint8 i2c_slStatus;          /* Slave Status */

/* Receive buffer variables */
volatile uint8 * i2c_slWrBufPtr;      /* Pointer to Receive buffer  */
volatile uint32  i2c_slWrBufSize;     /* Slave Receive buffer size  */
volatile uint32  i2c_slWrBufIndex;    /* Slave Receive buffer Index */

/* Transmit buffer variables */
volatile uint8 * i2c_slRdBufPtr;      /* Pointer to Transmit buffer  */
volatile uint32  i2c_slRdBufSize;     /* Slave Transmit buffer size  */
volatile uint32  i2c_slRdBufIndex;    /* Slave Transmit buffer Index */
volatile uint32  i2c_slRdBufIndexTmp; /* Slave Transmit buffer Index Tmp */
volatile uint8   i2c_slOverFlowCount; /* Slave Transmit Overflow counter */


/*******************************************************************************
* Function Name: i2c_I2CSlaveStatus
********************************************************************************
*
* Summary:
*  Returns I2C slave's communication status.
*
* Parameters:
*  None
*
* Return:
*  Current status of I2C slave.
*
* Global variables:
*  i2c_slStatus - used to store current status of I2C slave.
*
*******************************************************************************/
uint32 i2c_I2CSlaveStatus(void)
{
    return((uint32) i2c_slStatus);
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveClearReadStatus
********************************************************************************
*
* Summary:
*  Clears the read status flags and returns their values. No other status flags
*  are affected.
*
* Parameters:
*  None
*
* Return:
*  Current read status of I2C slave.
*
* Global variables:
*  i2c_slStatus  - used to store current status of I2C slave.
*
*******************************************************************************/
uint32 i2c_I2CSlaveClearReadStatus(void)
{
    uint32 status;

    /* Mask of transfer complete flag and Error status */
    status = ((uint32) i2c_slStatus & i2c_I2C_SSTAT_RD_MASK);
    i2c_slStatus &= (uint8) ~i2c_I2C_SSTAT_RD_CLEAR;

    return(status);
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveClearWriteStatus
********************************************************************************
*
* Summary:
*  Clears the write status flags and returns their values. No other status flags
*  are affected.
*
* Parameters:
*  None
*
* Return:
*  Current write status of I2C slave.
*
* Global variables:
*  i2c_slStatus  - used to store current status of I2C slave.
*
*******************************************************************************/
uint32 i2c_I2CSlaveClearWriteStatus(void)
{
    uint32 status;

    /* Mask of transfer complete flag and Error status */
    status = ((uint32) i2c_slStatus & i2c_I2C_SSTAT_WR_MASK);
    i2c_slStatus &= (uint8) ~i2c_I2C_SSTAT_WR_CLEAR;

    return(status);
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveSetAddress
********************************************************************************
*
* Summary:
*  Sets the I2C slave address.
*
* Parameters:
*  address: I2C slave address for the primary device. This value may be any
*  address between 0 and 127.
*
* Return:
*  None
*
*******************************************************************************/
void i2c_I2CSlaveSetAddress(uint32 address)
{
    uint32 matchReg;

    matchReg = i2c_RX_MATCH_REG;

    matchReg &= ((uint32) ~i2c_RX_MATCH_ADDR_MASK); /* Clear address bits */
    matchReg |= ((uint32)  i2c_GET_I2C_8BIT_ADDRESS(address));

    i2c_RX_MATCH_REG = matchReg;
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveSetAddressMask
********************************************************************************
*
* Summary:
*  Sets the I2C slave address mask.
*
* Parameters:
*  addressMask: Address mask.
*   0 - address bit does not care while comparison.
*   1 - address bit is significant while comparison.
*
* Return:
*  None
*
*******************************************************************************/
void i2c_I2CSlaveSetAddressMask(uint32 addressMask)
{
    uint32 matchReg;

    matchReg = i2c_RX_MATCH_REG;

    matchReg &= ((uint32) ~i2c_RX_MATCH_MASK_MASK); /* Clear address mask bits */
    matchReg |= ((uint32) (addressMask << i2c_RX_MATCH_MASK_POS));

    i2c_RX_MATCH_REG = matchReg;
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveInitReadBuf
********************************************************************************
*
* Summary:
*  Sets the buffer pointer and size of the read buffer. This function also
*  resets the transfer count returned with the I2C_SlaveGetReadBufSize function.
*
* Parameters:
*  readBuf:  Pointer to the data buffer to be read by the master.
*  bufSize:  Size of the read buffer exposed to the I2C master.
*
* Return:
*  None
*
* Global variables:
*  i2c_slRdBufPtr   - used to store pointer to slave read buffer.
*  i2c_slRdBufSize  - used to store salve read buffer size.
*  i2c_slRdBufIndex - used to store current index within slave
*  read buffer.
*
* Side Effects:
*  If this function is called during a bus transaction, data from the previous
*  buffer location and the beginning of current buffer may be transmitted.
*
*******************************************************************************/
void i2c_I2CSlaveInitReadBuf(uint8 * rdBuf, uint32 bufSize)
{
    /* Check for proper buffer */
    if(NULL != rdBuf)
    {
        i2c_slRdBufPtr      = (volatile uint8 *) rdBuf; /* Set buffer pointer  */
        i2c_slRdBufSize     = bufSize;                  /* Set buffer size     */
        i2c_slRdBufIndex    = 0u;                       /* Clear buffer index  */
        i2c_slRdBufIndexTmp = 0u;                       /* Clear buffer index  */
    }
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveInitWriteBuf
********************************************************************************
*
* Summary:
*  Sets the buffer pointer and size of the read buffer. This function also
*  resets the transfer count returned with the I2C_SlaveGetReadBufSize function.
*
* Parameters:
*  writeBuf:  Pointer to the data buffer to be read by the master.
*  bufSize:  Size of the buffer exposed to the I2C master.
*
* Return:
*  None
*
* Global variables:
*  i2c_slWrBufPtr   - used to store pointer to slave write buffer.
*  i2c_slWrBufSize  - used to store salve write buffer size.
*  i2c_slWrBufIndex - used to store current index within slave
*  write buffer.
*
* Side Effects:
*  If this function is called during a bus transaction, data from the previous
*  buffer location and the beginning of current buffer may be transmitted.
*
*******************************************************************************/
void i2c_I2CSlaveInitWriteBuf(uint8 * wrBuf, uint32 bufSize)
{
    /* Check buffer pointer */
    if(NULL != wrBuf)
    {
        i2c_slWrBufPtr   = (volatile uint8 *) wrBuf; /* Set buffer pointer  */
        i2c_slWrBufSize  = bufSize;                  /* Set buffer size     */
        i2c_slWrBufIndex = 0u;                       /* Clear buffer index  */
    }
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveGetReadBufSize
********************************************************************************
*
* Summary:
*  Returns the number of bytes read by the I2C master since an
*  I2C_SlaveInitReadBuf or I2C_SlaveClearReadBuf function was executed.
*  The maximum return value will be the size of the read buffer.
*
* Parameters:
*  None
*
* Return:
*  Bytes read by master.
*
* Global variables:
*  i2c_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
uint32 i2c_I2CSlaveGetReadBufSize(void)
{
    return(i2c_slRdBufIndex);
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveGetWriteBufSize
********************************************************************************
*
* Summary:
*  Returns the number of bytes written by the I2C master since an
*  I2C_SlaveInitWriteBuf or I2C_SlaveClearWriteBuf function was executed.
*  The maximum return value will be the size of the write buffer.
*
* Parameters:
*  None
*
* Return:
*  Bytes written by master.
*
* Global variables:
*  i2c_slWrBufIndex - used to store current index within slave
*  write buffer.
*
*******************************************************************************/
uint32 i2c_I2CSlaveGetWriteBufSize(void)
{
    return(i2c_slWrBufIndex);
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveClearReadBuf
********************************************************************************
*
* Summary:
*  Resets the read pointer to the first byte in the read buffer. The next byte
*  read by the master will be the first byte in the read buffer.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_slRdBufIndex - used to store current index within slave
*  read buffer.
*
*******************************************************************************/
void i2c_I2CSlaveClearReadBuf(void)
{
    i2c_slRdBufIndex = 0u;
}


/*******************************************************************************
* Function Name: i2c_I2CSlaveClearWriteBuf
********************************************************************************
*
* Summary:
*  Resets the write pointer to the first byte in the write buffer. The next byte
*  written by the master will be the first byte in the write buffer.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_slWrBufIndex - used to store current index within slave
*  write buffer.
*
*******************************************************************************/
void i2c_I2CSlaveClearWriteBuf(void)
{
    i2c_slWrBufIndex = 0u;
}

#endif /* (i2c_I2C_SLAVE_CONST) */


/* [] END OF FILE */
