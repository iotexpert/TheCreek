/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description: I2C Master Specific Functions
*
* NOTE:
* 
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterStatus
********************************************************************************
* Summary:
*  Returns status of the I2C Master.
*
* Parameters:  
*  void
*
* Return: 
*  Master status register.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterStatus(void)
{
   if( `$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_IDLE)
   {
       return(`$INSTANCE_NAME`_mstrStatus);
   }
   else
   {
       return(`$INSTANCE_NAME`_mstrStatus | `$INSTANCE_NAME`_MSTAT_XFER_INP);
   }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterClearStatus
********************************************************************************
* Summary:
*  Clears master status flags.
*
* Parameters:  
*  void
*
* Return: 
*   Return the read status.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterClearStatus(void)
{
    uint8 status;
    status = `$INSTANCE_NAME`_mstrStatus ; 
    `$INSTANCE_NAME`_mstrStatus  = `$INSTANCE_NAME`_MSTAT_CLEAR; 
   
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterSendStart
********************************************************************************
* Summary:
*  Sends a start with address and R/W bit.
*
* Parameters:  
*  slaveAddress: Address of slave recipiant. 
*  R_nW:         Send or recieve mode.
*
* Return: 
*  Returns a non-zero value if an error is detected 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterSendStart(uint8 slaveAddress, uint8 R_nW)
{
    uint8 timeOut;
    uint8 errStatus = 0;

    if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE)
    {
        for(timeOut = 0xFFu, errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT; timeOut; timeOut--)
        {
            if((`$INSTANCE_NAME`_MCSR & `$INSTANCE_NAME`_MCSR_BUS_BUSY) == 0)
            {
                errStatus = 0;
                break;
            }
        }

        CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
        slaveAddress = (slaveAddress << 1);

        if(R_nW != 0)
        {
           slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;   /* Set the Read flag */
           `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
        }
        else
        {
           `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
        }

        `$INSTANCE_NAME`_DATA = slaveAddress;            /* Write address to data reg */

        `$INSTANCE_NAME`_MCSR &= `$INSTANCE_NAME`_CTRL_ENABLE_MASK; /* Generate a Start */
        `$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_NACK_MASK) & ~(`$INSTANCE_NAME`_CTRL_STOP_MASK));
        `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK;
        `$INSTANCE_NAME`_GO = 0x00u;

        for(timeOut = 0xFFu, errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT; timeOut; timeOut--)
        {
        
            if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) != 0)
            {
                errStatus = 0;
                break;
            }
        }        

        if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_NAK)
        {
           errStatus = `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK;   /* No device ACKed the Master */
        } 
    }
    else
    {
        errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY ;
    }

   return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterSendRestart
********************************************************************************
* Summary:
*   Sends a restart with address and R/W bit.
*  
*
* Parameters:  
*  slaveAddress: Address of slave recipiant. 
*  R_nW:         Send or recieve mode.
*
* Return: 
*  Returns a non-zero value if an error is detected 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterSendRestart(uint8 slaveAddress, uint8 R_nW)
{
    uint8 timeOut;
    uint8 errStatus = 0;

    if(`$INSTANCE_NAME`_State & `$INSTANCE_NAME`_SM_MASTER)
    {
/*        if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_LOST_ARB) == 0) */
        {
            slaveAddress = (slaveAddress << 1);

            if(R_nW != 0)
            {
                slaveAddress |= `$INSTANCE_NAME`_READ_FLAG;   /* Set the Read flag */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;
            }
            else
            {
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;
            }

            `$INSTANCE_NAME`_DATA = slaveAddress;            /* Write address to data reg */
            //`$INSTANCE_NAME`_MCSR = `$INSTANCE_NAME`_MCSR_RESTART_GEN;  /* Generate a restart */

            `$INSTANCE_NAME`_MCSR |= `$INSTANCE_NAME`_CTRL_RESTART_MASK; /* Generate a ReStart */
            `$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_NACK_MASK) & ~(`$INSTANCE_NAME`_CTRL_STOP_MASK));
            `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_TRANSMIT_MASK;
            `$INSTANCE_NAME`_GO = 0x00u;

            /* Wait for the address to be transfered  */
            for(timeOut = 0xFFu, errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT; timeOut; timeOut--)
            {
                if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) != 0)
                //if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) == 0)
                {
                    errStatus = 0;
                    break;
                }
            }

            if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_NAK)
            {
                errStatus = `$INSTANCE_NAME`_MSTAT_ERR_ADDR_NAK;   /* No device ACKed the Master */
            } 
        }
/*      else
        {
            errStatus = `$INSTANCE_NAME`_MSTAT_ERR_ARB_LOST;
	    } */ 
    }
    else
    {

    }
    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterSendStop
********************************************************************************
* Summary:
*  Sends stop condition.
*
* Parameters:  
*  void
*
* Return: 
*  Returns a non-zero value if an error is detected 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterSendStop(void)
{
    uint8 timeOut;
    uint8 errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;


    /* Wait for the data to be transfered  */
    for(timeOut = 0xFFu, errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT; timeOut; timeOut--)
    {
        if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) != 0)
        {
            errStatus = 0;
            break;
        }
    }

/* FF   if((`$INSTANCE_NAME`_MCSR & `$INSTANCE_NAME`_MCSR_MSTR_MODE) != 0) */
    {

        `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_STOP_MASK | `$INSTANCE_NAME`_CTRL_NACK_MASK;

	    /* Write dummy data to GO (FIFO). */
	    `$INSTANCE_NAME`_GO = 0x00u;

        /* Wait for the go bit to clear the byte complete bit. */
        for(timeOut = 0xFFu; timeOut; timeOut--)
        {
            if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) == 0)
            {
                break;
            }
        }
        
        errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;
    }
    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterWriteBuf
********************************************************************************
* Summary:
*  This function initiates a write transaction with an addressed slave.  Writes
*  one or more bytes (cnt) to the slave I2C device and gets the data from RAM 
*  or ROM array pointed to by the array pointer.  Once this routine is called, 
*  the included ISR will handle further data in byte by byte mode.  
*
*
* Parameters:  
*  slaveAddr: 7-bit slave address
*  xferData:  Pointer to data in array.
*  cnt:       Count of data to write.
*  mode:      Mode of operation.  It defines normal start, restart,
*             stop, no-stop, etc.
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*  //:TODO  incorporate mode
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterWriteBuf(uint8 slaveAddr, uint8 * xferData,
                                       uint8 cnt, uint8 mode)
{
    uint8 tmr;
    uint8 errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;

    /* Determine whether or not to automatically generate a stop condition */
    if(mode & `$INSTANCE_NAME`_MODE_NO_STOP)
    {
       /* Do not generate a Stop at the end of transfer */
       `$INSTANCE_NAME`_mstrControl |= `$INSTANCE_NAME`_MSTR_NO_STOP;
    }
    else  /* Generate a Stop */
    {
       `$INSTANCE_NAME`_mstrControl &= ~`$INSTANCE_NAME`_MSTR_NO_STOP;
    }

    if((`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE) ||(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_HALT))
    {
        errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT;
        if(`$INSTANCE_NAME`_State != `$INSTANCE_NAME`_SM_MSTR_HALT)
        {
            for(tmr = `$INSTANCE_NAME`_MSTR_TIMEOUT; tmr > 0; tmr--)
            {
                if((`$INSTANCE_NAME`_MCSR & `$INSTANCE_NAME`_MCSR_BUS_BUSY) == 0)
                {
                   errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                   break;
                }
                CyDelay(1);    /* Wait 1 mSec */
            }
        }
        else   /* Bus halted waiting for restart */
        {
            errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
            `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT; 
            CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER);
        }

        /* If no timeout error, generate start */
        if(errStatus == `$INSTANCE_NAME`_MSTR_NO_ERROR)
        {
            slaveAddr = (slaveAddr << 1);
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_WR_ADDR;

            `$INSTANCE_NAME`_mstrWrBufPtr   = xferData; /* Set buffer pointer */
            `$INSTANCE_NAME`_mstrWrBufIndex = 0;		/* Start buffer at zero */
            `$INSTANCE_NAME`_mstrWrBufSize  = cnt;      /* Set buffer size */

            `$INSTANCE_NAME`_DATA = slaveAddr;            /* Write address to data reg */

            /* Generate a Start or ReStart depending on flag passed */
            if(mode & `$INSTANCE_NAME`_MODE_REPEAT_START)
            {
                `$INSTANCE_NAME`_MCSR |= `$INSTANCE_NAME`_CTRL_RESTART_MASK; /* Generate a ReStart */

                `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK);

                `$INSTANCE_NAME`_GO = 0x00u;
            }
            else
            {
                `$INSTANCE_NAME`_MCSR &= `$INSTANCE_NAME`_CTRL_ENABLE_MASK; /* Generate a Start */
    
                `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK);

                `$INSTANCE_NAME`_GO = 0x00u;
            }

            `$INSTANCE_NAME`_EnableInt( );   /* IRQ must be enabled for this to work */

            /* Clear write complete flag */
            `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_WR_CMPLT; 
        }
    }
    else
    {
       errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
    }

    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterReadBuf
********************************************************************************
* Summary:
*   The function intiates a read transaction with an addressed slave.  Reads
*   one or more bytes (cnt) from the slave I2C device and writes data to the '
*   array.  Once this routine is called, the included ISR will handle further
*   data in byte by byte mode.
*
* Parameters:  
*  slaveAddr: 7-bit slave address
*  xferData:  Pointer to data in array.
*  cnt:       Count of data to write.
*  mode:      Mode of operation.  It defines normal start, restart,
*             stop, no-stop, etc.
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*  //:TODO  incorporate mode
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterReadBuf(uint8 slaveAddr, uint8 * xferData,
                                      uint8 cnt, uint8 mode )
{
    uint8 tmr;
    uint8 errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;

    /* Determine whether or not to automatically generate a stop condition */
    if(mode & `$INSTANCE_NAME`_MODE_NO_STOP)
    {
       /* Do not generate a Stop at the end of transfer */
       `$INSTANCE_NAME`_mstrControl |= `$INSTANCE_NAME`_MSTR_NO_STOP;
    }
    else  /* Generate a Stop */
    {
       `$INSTANCE_NAME`_mstrControl &= ~`$INSTANCE_NAME`_MSTR_NO_STOP;
    }

    if((`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_IDLE) || (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_MSTR_HALT))
    {
        errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT;

        if(`$INSTANCE_NAME`_State != `$INSTANCE_NAME`_SM_MSTR_HALT)
        {
            for(tmr = `$INSTANCE_NAME`_MSTR_TIMEOUT; tmr > 0; tmr--)
            {
                if((`$INSTANCE_NAME`_MCSR & `$INSTANCE_NAME`_MCSR_BUS_BUSY) == 0)
                {
                   errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
                   break;
                }
                CyDelay(1);    /* Wait 1 mSec */
            }
        } 
        else   /* Bus halted waiting for restart */
        {
            errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;
            `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_XFER_HALT; 
            CyIntClearPending(`$INSTANCE_NAME`_ISR_NUMBER); 
        }

        /* If no timeout error, generate start */
        if(errStatus == `$INSTANCE_NAME`_MSTR_NO_ERROR)
        {
            slaveAddr = (slaveAddr << 1);
            slaveAddr |= `$INSTANCE_NAME`_READ_FLAG;   /* Set the Read flag */
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_MSTR_RD_ADDR;

            `$INSTANCE_NAME`_mstrRdBufPtr    = xferData;
            `$INSTANCE_NAME`_mstrRdBufIndex   = 0;
            `$INSTANCE_NAME`_mstrRdBufSize   = cnt;    /* Set buffer size */

            `$INSTANCE_NAME`_DATA = slaveAddr;         /* Write address to data reg */

            /* Generate a Start or ReStart depending on flag passed */
            if(mode & `$INSTANCE_NAME`_MODE_REPEAT_START)
            {
                `$INSTANCE_NAME`_MCSR |= `$INSTANCE_NAME`_CTRL_RESTART_MASK; /* Generate a ReStart */

                `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK);

                `$INSTANCE_NAME`_GO = 0x00u;
            }
            else
            {
                `$INSTANCE_NAME`_MCSR &= `$INSTANCE_NAME`_CTRL_ENABLE_MASK; /* Generate a Start */
    
                `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK);

                `$INSTANCE_NAME`_GO = 0x00u;
            }

            `$INSTANCE_NAME`_EnableInt( );         /* IRQ must be enabled for this to work */
 
            /* Clear read complete flag */
            `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_RD_CMPLT; 
        }
    }
    else
    {
       errStatus = `$INSTANCE_NAME`_MSTR_SLAVE_BUSY;
    }

    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterWriteByte
********************************************************************************
* Summary:
*  This function sends a single-byte I2C bus write and ACK.  This function does
*  not generate a start or stop condition.  This routine should ONLY be called
*  when a prevous start and address has been generated on the I2Cbus.
*
*
* Parameters:  
*  data:  Byte to be sent to the I2C slave
*
* Return: 
*  The return value is non-zero, if the slave acknowledged the master.
*  The return value is zero, if the slave did not acknoledge the
*  master.  If the slave failed to acknowledged the master, the
*  value will be 0xFF;
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8  `$INSTANCE_NAME`_MasterWriteByte(uint8 theByte)
{
    uint8 timeOut;
    uint8 errStatus = `$INSTANCE_NAME`_MSTR_NO_ERROR;


    /* Make sure the last byte has been transfered first. */
    for(timeOut = 0xFFu, errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT; timeOut; timeOut--)
    {
        if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) != 0)
        {
            errStatus = 0;
            break;
        }
    }

    if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_ACK)
    {
        `$INSTANCE_NAME`_DATA = theByte;
        `$INSTANCE_NAME`_CFG &= ~`$INSTANCE_NAME`_CTRL_NACK_MASK;
        `$INSTANCE_NAME`_CFG |= (`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK);
        `$INSTANCE_NAME`_GO = 0x00u;

        /* Wait for the go bit to clear the byte complete bit. */
        for(timeOut = 0xFF; timeOut; timeOut--)
        {
            if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) == 0)
            {
                break;
            }
        }
    } 
    else
    {
        errStatus = `$INSTANCE_NAME`_MSTR_ERR_LB_NAK;
    }
    return(errStatus);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterReadByte
********************************************************************************
* Summary:
*  This function sends a single-byte I2C bus read and ACK phase.  This function 
*  does not generate a start or stop condition.  This routine should ONLY be 
*  called when a prevous start and address has been generated on the I2Cbus.
*
* Parameters:  
*  acknNak:  If non-zero an ACK will be the response, else a zero will
*                    cause a NAK to be sent.
*
* Return: 
*  Returns the data received from the I2C slave.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_MasterReadByte(uint8 acknNak)
{
    uint8 timeOut;
    uint8 theByte;
/*    uint8 errStatus; */


    `$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK));
    `$INSTANCE_NAME`_GO = 0x00u;

    /* Wait for a byte to come in. */
    for(timeOut = 0xFFu /*, errStatus = `$INSTANCE_NAME`_MSTR_BUS_TIMEOUT */; timeOut; timeOut--)
    {
        if((`$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE) != 0)
        {
/*            errStatus = 0; */
            break;
        }
    }

    theByte = `$INSTANCE_NAME`_DATA;

    if(acknNak != 0)   /* Do ACK */
    {
        `$INSTANCE_NAME`_CFG &= (~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK) & ~(`$INSTANCE_NAME`_CTRL_NACK_MASK));
        `$INSTANCE_NAME`_GO = 0x00u;
    }
    else 
    {
        `$INSTANCE_NAME`_CFG &= ~(`$INSTANCE_NAME`_CTRL_TRANSMIT_MASK);
        `$INSTANCE_NAME`_CFG |= `$INSTANCE_NAME`_CTRL_NACK_MASK;
        `$INSTANCE_NAME`_GO = 0x00u;
        `$INSTANCE_NAME`_State  = `$INSTANCE_NAME`_SM_IDLE;
    }


    return(theByte);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterGetReadBufSize
********************************************************************************
* Summary:
*  Determine the number of bytes used in the RX buffer.  Empty returns 0.
*
* Parameters:  
*  void
*
* Return: 
*  Number of bytes in buffer until full.
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_MasterGetReadBufSize(void)
{
    return(`$INSTANCE_NAME`_mstrRdBufIndex);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterGetWriteBufSize
********************************************************************************
* Summary:
*  Determine the number of bytes used in the TX buffer.  Empty returns 0.
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
uint16 `$INSTANCE_NAME`_MasterGetWriteBufSize(void)
{
    return(`$INSTANCE_NAME`_mstrWrBufIndex);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterClearReadBuf
********************************************************************************
* Summary:
*  Sets the buffer read and write pointers to 0.
*  
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
void `$INSTANCE_NAME`_MasterClearReadBuf(void)
{
    `$INSTANCE_NAME`_mstrRdBufIndex = 0;
    `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_RD_CMPLT;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_MasterClearWriteBuf
********************************************************************************
* Summary:
*   Sets the buffer read and write pointers to 0.
*  
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
void `$INSTANCE_NAME`_MasterClearWriteBuf(void)
{
    `$INSTANCE_NAME`_mstrWrBufIndex = 0;    
    `$INSTANCE_NAME`_mstrStatus &= ~`$INSTANCE_NAME`_MSTAT_WR_CMPLT;
}


/* [] END OF FILE */
