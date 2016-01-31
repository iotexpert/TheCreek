/*******************************************************************************
* File Name: `$INSTANCE_NAME`_MASTER.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of APIs for the I2C component Master mode.
*
* Note:
*
*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

extern volatile uint8 `$INSTANCE_NAME`_state;   /* Current state of I2C FSM */

#if(`$INSTANCE_NAME`_MODE_MASTER_ENABLED)
/**********************************
*      System variables
**********************************/

volatile uint8 `$INSTANCE_NAME`_mstrStatus;     /* Master Status byte  */
volatile uint8 `$INSTANCE_NAME`_mstrControl;    /* Master Control byte */

/* Transmit buffer variables */
volatile uint8 * `$INSTANCE_NAME`_mstrRdBufPtr;     /* Pointer to Master Read buffer */
volatile uint8   `$INSTANCE_NAME`_mstrRdBufSize;    /* Master Read buffer size       */
volatile uint8   `$INSTANCE_NAME`_mstrRdBufIndex;   /* Master Read buffer Index      */

/* Receive buffer variables */
volatile uint8 * `$INSTANCE_NAME`_mstrWrBufPtr;     /* Pointer to Master Write buffer */
volatile uint8   `$INSTANCE_NAME`_mstrWrBufSize;    /* Master Write buffer size       */
volatile uint8   `$INSTANCE_NAME`_mstrWrBufIndex;   /* Master Write buffer Index      */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterWriteBuf
********************************************************************************
*
* Summary:
*  Automatically writes an entire buffer of data to a slave device. Once the
*  data transfer is initiated by this function, further data transfer is handled
*  by the included ISR in byte by byte mode.
*
* Parameters:
*  slaveAddr: 7-bit slave address.
*  xferData:  Pointer to buffer of data to be sent.
*  cnt:       Size of buffer to send.
*  mode:      Transfer mode defines: start or restart condition generation at
*             begin of the transfer and complete the transfer or halt before
*             generating a stop.
*
* Return:
*  Status error - zero means no errors.
*
* Side Effects:
*  The included ISR will start transfer after start or restart condition will
*  be generated.
*
* Global variables:
*  `$INSTANCE_NAME`_mstrStatus  - used to store current status of I2C Master.
*  `$INSTANCE_NAME`_state       - used to store current state of software FSM.
*  `$INSTANCE_NAME`_mstrControl - used to control master end of transaction with
*  or without the Stop generation.
*  `$INSTANCE_NAME`_mstrWrBufPtr - used to store pointer to master write buffer.
*  `$INSTANCE_NAME`_mstrWrBufIndex - used to current index within master write
*  buffer.
*  `$INSTANCE_NAME`_mstrWrBufSize - used to store master write buffer size.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterWriteBuf(uint8 slaveAddress, uint8 * xferData, uint8 cnt, uint8 mode)
      `=ReentrantKeil($INSTANCE_NAME . "_MasterWriteBuf")`
{
    uint8 errStatus;

    errStatus = `$INSTANCE_NAME`_MSTR_NOT_READY;

    if(NULL != xferData)
    {
        /* Check I2C state before transfer. Valid are IDLE or HALT */
        if((`$INSTANCE_NAME`_SM_IDLE      == `$INSTANCE_NAME`_state) ||
           (`$INSTANCE_NAME`_SM_MSTR_HALT == `$INSTANCE_NAME`_state))
        {
            if(`$INSTANCE_NAME`_SM_IDLE   == `$INSTANCE_NAME`_state)
            {
                /* Bus IDLE:  check if free */
                if(`$INSTANCE_NAME`_CHECK_BUS_FREE(`$INSTANCE_NAME`_MCSR_REG))
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                }
                else
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_BUS_BUSY;
                }
            }
            else    /* Bus HALT: waiting for ReStart */
            {
                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT;
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
            }


            /* If no errors, generate start */
            if(errStatus == `$INSTANCE_NAME`_MSTR_NO_ERROR)
            {
                `$INSTANCE_NAME`_state    = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
                `$INSTANCE_NAME`_DATA_REG = (slaveAddress << `$INSTANCE_NAME`_SLAVE_ADDR_SHIFT);

                `$INSTANCE_NAME`_mstrWrBufIndex = 0u;
                `$INSTANCE_NAME`_mstrWrBufSize  = cnt;
                `$INSTANCE_NAME`_mstrWrBufPtr   = (volatile uint8 *) xferData;

                `$INSTANCE_NAME`_mstrControl = mode;    /* Save transaction mode */

                /* Generate a Start or ReStart depends on mode */
                if(`$INSTANCE_NAME`_CHECK_RESTART(mode))
                {
                    `$INSTANCE_NAME`_GENERATE_RESTART;
                }
                else
                {
                    `$INSTANCE_NAME`_GENERATE_START;
                }

                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_WR_CMPLT;

                `$INSTANCE_NAME`_EnableInt();   /* Enable intr to complete transfer */
            }
        }
    }

    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterReadBuf
********************************************************************************
*
* Summary:
*  Automatically writes an entire buffer of data to a slave device. Once the
*  data transfer is initiated by this function, further data transfer is handled
*  by the included ISR in byte by byte mode.
*
* Parameters:
*  slaveAddr: 7-bit slave address.
*  xferData:  Pointer to buffer where to put data from slave.
*  cnt:       Size of buffer to read.
*  mode:      Transfer mode defines: start or restart condition generation at
*             begin of the transfer and complete the transfer or halt before
*             generating a stop.
*
* Return:
*  Status error - zero means no errors.
*
* Side Effects:
*  The included ISR will start transfer after start or restart condition will
*  be generated.
*
* Global variables:
*  `$INSTANCE_NAME`_mstrStatus  - used to store current status of I2C Master.
*  `$INSTANCE_NAME`_state       - used to store current state of software FSM.
*  `$INSTANCE_NAME`_mstrControl - used to control master end of transaction with
*  or without the Stop generation.
*  `$INSTANCE_NAME`_mstrRdBufPtr - used to store pointer to master write buffer.
*  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master write
*  buffer.
*  `$INSTANCE_NAME`_mstrRdBufSize - used to store master write buffer size.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterReadBuf(uint8 slaveAddress, uint8 * xferData, uint8 cnt, uint8 mode)
      `=ReentrantKeil($INSTANCE_NAME . "_MasterReadBuf")`
{
    uint8 errStatus;

    errStatus = `$INSTANCE_NAME`_MSTR_NOT_READY;

    if(NULL != xferData)
    {
        /* Check I2C state before transfer. Valid are IDLE or HALT */
        if((`$INSTANCE_NAME`_SM_IDLE      == `$INSTANCE_NAME`_state) ||
           (`$INSTANCE_NAME`_SM_MSTR_HALT == `$INSTANCE_NAME`_state))
        {
            if(`$INSTANCE_NAME`_SM_IDLE   == `$INSTANCE_NAME`_state)
            {
                /* Bus IDLE:  check if free */
                if(`$INSTANCE_NAME`_CHECK_BUS_FREE(`$INSTANCE_NAME`_MCSR_REG))
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                }
                else
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_BUS_BUSY;
                }
            }
            else    /* Bus HALT: waiting for ReStart */
            {
                CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT;
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
            }


            /* If no error, generate Start/ReStart condition */
            if(errStatus == `$INSTANCE_NAME`_MSTR_NO_ERROR)
            {
                `$INSTANCE_NAME`_state    = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
                `$INSTANCE_NAME`_DATA_REG = ((slaveAddress << `$INSTANCE_NAME`_SLAVE_ADDR_SHIFT) |
                                              `$INSTANCE_NAME`_READ_FLAG);

                `$INSTANCE_NAME`_mstrRdBufIndex  = 0u;
                `$INSTANCE_NAME`_mstrRdBufSize   = cnt;
                `$INSTANCE_NAME`_mstrRdBufPtr    = (volatile uint8 *) xferData;

                `$INSTANCE_NAME`_mstrControl = mode;    /* Save transaction mode */

                /* Generate a Start or ReStart depends on mode */
                if(`$INSTANCE_NAME`_CHECK_RESTART(mode))
                {
                    `$INSTANCE_NAME`_GENERATE_RESTART;
                }
                else
                {
                    `$INSTANCE_NAME`_GENERATE_START;
                }

                `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_RD_CMPLT;

                `$INSTANCE_NAME`_EnableInt();   /* Enable intr to complete transfer */
            }
        }
    }

    return (errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterSendStart
********************************************************************************
*
* Summary:
*  Generates Start condition and sends slave address with read/write bit.
*
* Parameters:
*  slaveAddress:  7-bit slave address.
*  R_nW:          Zero, send write command, non-zero send read command.
*
* Return:
*  Status error - zero means no errors.
*
* Side Effects:
*  This function is entered without a 'byte complete' bit set in the I2C_CSR
*  register. It does not exit until it will be set.
*
* Global variables:
*  `$INSTANCE_NAME`_state - used to store current state of software FSM.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterSendStart(uint8 slaveAddress, uint8 R_nW)
      `=ReentrantKeil($INSTANCE_NAME . "_MasterSendStart")`
{
    uint8 errStatus;

    errStatus = `$INSTANCE_NAME`_MSTR_NOT_READY;

    /* If IDLE, check if bus is free */
    if(`$INSTANCE_NAME`_SM_IDLE == `$INSTANCE_NAME`_state)
    {
        /* If bus is free, generate Start condition */
        if(`$INSTANCE_NAME`_CHECK_BUS_FREE(`$INSTANCE_NAME`_MCSR_REG))
        {
            `$INSTANCE_NAME`_DisableInt();  /* Disable ISR for Manual functions */

            slaveAddress <<= `$INSTANCE_NAME`_SLAVE_ADDR_SHIFT; /* Set Address */
            if(0u != R_nW)                                      /* Set the Read/Write flag */
            {
                slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
            }
            else
            {
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
            }
            `$INSTANCE_NAME`_DATA_REG = slaveAddress;   /* Write address to data reg */


            `$INSTANCE_NAME`_GENERATE_START;

            /* Wait for the address to be transfered */
            while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));


            #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE_ENABLED)
                if(`$INSTANCE_NAME`_CHECK_START_GEN(`$INSTANCE_NAME`_MCSR_REG))
                {
                    /* Clear Start Gen bit */
                    `$INSTANCE_NAME`_CLEAR_START_GEN;

                    /* Arbitration has been lost, reset state machine to IDLE */
                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
                    errStatus = `$INSTANCE_NAME`_MSTR_ERR_ABORT_START_GEN;
                }
                else
            #endif  /* End (`$INSTANCE_NAME`_MODE_MULTI_MASTER_SLAVE_ENABLED) */

            #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED)
                /* Check for loss of arbitration */
                if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
                {
                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
                    errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST; /* Master lost arbitrage */
                }
                else
            #endif  /* (`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED) */

                if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(`$INSTANCE_NAME`_CSR_REG))
                {
                    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
                    errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;   /* No device ACKed the Master */
                }
                else
                {
                    errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send Start witout errors */
                }
        }
        else
        {
            errStatus = `$INSTANCE_NAME`_MSTR_BUS_BUSY; /* Bus is busy */
        }
    }

    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterSendRestart
********************************************************************************
*
* Summary:
*  Generates ReStart condition and sends slave address with read/write bit.
*
* Parameters:
*  slaveAddress:  7-bit slave address.
*  R_nW:          Zero, send write command, non-zero send read command.
*
* Return:
*  Status error - zero means no errors.
*
* Side Effects:
*  This function is entered without a 'byte complete' bit set in the I2C_CSR
*  register. It does not exit until it will be set.
*
* Global variables:
*  `$INSTANCE_NAME`_state - used to store current state of software FSM.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterSendRestart(uint8 slaveAddress, uint8 R_nW)
      `=ReentrantKeil($INSTANCE_NAME . "_MasterSendRestart")`
{
    uint8 errStatus;

    errStatus = `$INSTANCE_NAME`_MSTR_NOT_READY;

    /* Check if START condition was generated */
    if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
    {
        slaveAddress <<= `$INSTANCE_NAME`_SLAVE_ADDR_SHIFT; /* Set Address */
        if(0u != R_nW)                                      /* Set the Read/Write flag */
        {
            slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;
            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
        }
        else
        {
            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
        }
        `$INSTANCE_NAME`_DATA_REG = slaveAddress;    /* Write address to data reg */


        `$INSTANCE_NAME`_GENERATE_RESTART_MANUAL;

        /* Wait for the address to be transfered */
        while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));


        #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED)
            /* Check for loss of arbitration */
            if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
            {
                /* Arbitration has been lost, reset state machine to IDLE */
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
                errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST; /* Master lost arbitrage */
            }
            else
        #endif  /* End (`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED) */

            /* Check ACK address if Master mode */
            if(`$INSTANCE_NAME`_CHECK_ADDR_NAK(`$INSTANCE_NAME`_CSR_REG))
            {
                /* Address has been NACKed, reset state machine to IDLE */
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;
                errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;    /* No device ACKed the Master */
            }
            else
            {
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;     /* Send START witout errors */
            }
    }

    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterSendStop
********************************************************************************
*
* Summary:
*  Generates I2C Stop condition on bus. Function do nothing if Start or Restart
*  condition was failed before call this function.
*
* Parameters:
*  None
*
* Return:
*  Status error - zero means no errors.
*
* Side Effects:
*  The Stop generation is required to complete transaction.
*  This function does not wait whileStop condition will be generated.
*
* Global variables:
*  `$INSTANCE_NAME`_state - used to store current state of software FSM.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterSendStop(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterSendStop")`
{
    uint8 errStatus;

    errStatus = `$INSTANCE_NAME`_MSTR_NOT_READY;

    /* Check if START condition was generated */
    if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
    {
        `$INSTANCE_NAME`_GENERATE_STOP_MANUAL;              /* Generate STOP */
        `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;  /* Reset state to IDLE */
        errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;         /* Start send STOP witout errors */


        while(0u == ((`$INSTANCE_NAME`_CSR_BYTE_COMPLETE | `$INSTANCE_NAME`_CSR_STOP_STATUS) &
                                                           `$INSTANCE_NAME`_CSR_REG));

        #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED)
            if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
            {
                errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST; /* NACK was generated instead Stop */
            }
        #endif  /* End (`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED) */
    }

    return (errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterWriteByte
********************************************************************************
*
* Summary:
*  Sends one byte to a slave. A valid Start or ReStart condition must be
*  generated before this call this function. Function do nothing if Start or
*  Restart condition was failed before call this function.
*
* Parameters:
*  data:  The data byte to send to the slave.
*
* Return:
*  Status error - zero means no errors.
*
* Side Effects:
*  This function is entered without a 'byte complete' bit set in the I2C_CSR
*  register. It does not exit until it will be set.
*
* Global variables:
*  `$INSTANCE_NAME`_state - used to store current state of software FSM.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterWriteByte(uint8 theByte) `=ReentrantKeil($INSTANCE_NAME . "_MasterWriteByte")`
{
    uint8 errStatus;

    errStatus = `$INSTANCE_NAME`_MSTR_NOT_READY;

    /* Check if START condition was generated */
    if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
    {
        `$INSTANCE_NAME`_DATA_REG = theByte;                        /* Write DATA register */
        `$INSTANCE_NAME`_TRANSMIT_DATA_MANUAL;                      /* Set transmit mode */
        `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_WR_DATA;  /* Set state WR_DATA */

        /* Make sure the last byte has been transfered first */
        while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));

        #if(`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED)
            /* Check for LOST ARBITRATION */
            if(`$INSTANCE_NAME`_CHECK_LOST_ARB(`$INSTANCE_NAME`_CSR_REG))
            {
                /* Arbitration has been lost, reset state machine to IDLE */
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;          /* Reset state to IDLE */
                errStatus = `$INSTANCE_NAME`_MSTR_ERR_ARB_LOST;             /* The Master LOST ARBITRAGE */
            }
            /* Check LRB bit */
            else
        #endif  /* End (`$INSTANCE_NAME`_MODE_MULTI_MASTER_ENABLED) */

            if(`$INSTANCE_NAME`_CHECK_DATA_ACK(`$INSTANCE_NAME`_CSR_REG))
            {
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_HALT;     /* Set state to HALT */
                errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;                 /* The LRB was ACKed */
            }
            else
            {
                `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_HALT;     /* Set state to HALT */
                errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;               /* The LRB was NACKed */
            }
    }

    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterReadByte
********************************************************************************
*
* Summary:
*  Reads one byte from a slave and ACK or NACK the transfer. A valid Start or
*  ReStart condition must be generated before this call this function. Function
*  do nothing if Start or Restart condition was failed before call this
*  function.
*
* Parameters:
*  acknNack:  Zero, response with NACK, if non-zero response with ACK.
*
* Return:
*  Byte read from slave.
*
* Side Effects:
*  This function is entered without a 'byte complete' bit set in the I2C_CSR
*  register. It does not exit until it will be set.
*
* Global variables:
*  `$INSTANCE_NAME`_state - used to store current state of software FSM.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterReadByte(uint8 acknNak) `=ReentrantKeil($INSTANCE_NAME . "_MasterReadByte")`
{
    uint8 theByte;

    theByte = 0u;

    /* Check if START condition was generated */
    if(`$INSTANCE_NAME`_CHECK_MASTER_MODE(`$INSTANCE_NAME`_MCSR_REG))
    {
        /* When address phase need release the bus and receive the byte, then decide ACK or NACK */
        if(`$INSTANCE_NAME`_state == `$INSTANCE_NAME`_SM_MSTR_RD_ADDR)
        {
            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_RD_DATA;
            `$INSTANCE_NAME`_READY_TO_READ_MANUAL;
        }

        while(`$INSTANCE_NAME`_WAIT_BYTE_COMPLETE(`$INSTANCE_NAME`_CSR_REG));

        theByte = `$INSTANCE_NAME`_DATA_REG;

        /* Now if the ACK flag was set, ACK the data which will release the bus and start the next byte in
           otherwise do NOTHING to the CSR reg.
           This will allow the calling routine to generate a repeat start or a stop depending on it's preference. */
        if(acknNak != 0u)   /* Do ACK */
        {
            `$INSTANCE_NAME`_ACK_AND_RECEIVE_MANUAL;
        }
        else                /* Do NACK */
        {
            /* Do nothing to be able work with ReStart */
            `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_MSTR_HALT;
        }
    }

    return(theByte);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterStatus
********************************************************************************
*
* Summary:
*  Returns the master's communication status.
*
* Parameters:
*  None
*
* Return:
*  Current status of I2C master.
*
* Global variables:
*  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterStatus")`
{
    uint8 status;

    status = `$INSTANCE_NAME`_mstrStatus;

    /* When in Master state only transaction is in progress */
    if(0u != (`$INSTANCE_NAME`_SM_MASTER & `$INSTANCE_NAME`_state))
    {
        /* Add transaction in progress activity to master status */
        status |= `$INSTANCE_NAME`_MSTAT_XFER_INP;
    }
    else
    {
        /* Current master status is valid */
    }

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterClearStatus
********************************************************************************
*
* Summary:
*  Clears all status flags and returns the master status.
*
* Parameters:
*  None
*
* Return:
*  Current status of I2C master.
*
* Global variables:
*  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
*
* Reentrant:
*  No
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterClearStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterClearStatus")`
{
    /* Current master status */
    uint8 status;

    /* Read and clear master status */
    status = `$INSTANCE_NAME`_mstrStatus;
    `$INSTANCE_NAME`_mstrStatus = `$INSTANCE_NAME`_MSTAT_CLEAR;

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterGetReadBufSize
********************************************************************************
*
* Summary:
*  Returns the amount of bytes that has been transferred with an
*  I2C_MasterReadBuf command.
*
* Parameters:
*  None
*
* Return:
*  Byte count of transfer. If the transfer is not yet complete, it will return
*  the byte count transferred so far.
*
* Global variables:
*  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master read
*  buffer.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterGetReadBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterGetReadBufSize")`
{
    return(`$INSTANCE_NAME`_mstrRdBufIndex);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterGetWriteBufSize
********************************************************************************
*
* Summary:
*  Returns the amount of bytes that has been transferred with an
*  I2C_MasterWriteBuf command.
*
* Parameters:
*  None
*
* Return:
*  Byte count of transfer. If the transfer is not yet complete, it will return
*  the byte count transferred so far.
*
* Global variables:
*  `$INSTANCE_NAME`_mstrWrBufIndex - used to current index within master write
*  buffer.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterGetWriteBufSize(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterGetWriteBufSize")`
{
    return(`$INSTANCE_NAME`_mstrWrBufIndex);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterClearReadBuf
********************************************************************************
*
* Summary:
*  Resets the read buffer pointer back to the first byte in the buffer.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master read
*   buffer.
*  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_MasterClearReadBuf(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterClearReadBuf")`
{
    `$INSTANCE_NAME`_mstrRdBufIndex = 0u;
    `$INSTANCE_NAME`_mstrStatus    &= ~`$INSTANCE_NAME`_MSTAT_RD_CMPLT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterClearWriteBuf
********************************************************************************
*
* Summary:
*  Resets the write buffer pointer back to the first byte in the buffer.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_mstrRdBufIndex - used to current index within master read
*   buffer.
*  `$INSTANCE_NAME`_mstrStatus - used to store current status of I2C Master.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_MasterClearWriteBuf(void) `=ReentrantKeil($INSTANCE_NAME . "_MasterClearWriteBuf")`
{
    `$INSTANCE_NAME`_mstrWrBufIndex = 0u;
    `$INSTANCE_NAME`_mstrStatus    &= ~`$INSTANCE_NAME`_MSTAT_WR_CMPLT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Workaround
********************************************************************************
*
* Summary:
*  Do nothing. This fake fuction use as workaround for CDT 78083.
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
void `$INSTANCE_NAME`_Workaround(void) `=ReentrantKeil($INSTANCE_NAME . "_Workaround")`
{

}

#endif  /* End (`$INSTANCE_NAME`_MODE_MASTER_ENABLED) */


/* [] END OF FILE */
