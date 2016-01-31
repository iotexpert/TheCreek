/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description: I2C Slave Specific Functions
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
        `$INSTANCE_NAME`_Address  = address & 0x7Fu;   /* Set Address variable */
    #else
        `$INSTANCE_NAME`_ADDR  = address & 0x7Fu;      /* Set I2C Address register */
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
    /* Component function code */
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
    `$INSTANCE_NAME`_DATA = transmitDataByte;
    `$INSTANCE_NAME`_CSR  = `$INSTANCE_NAME`_CSR_TRANSMIT; 
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
        `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_ACK ; 
    }
    else
    {
        `$INSTANCE_NAME`_CSR = `$INSTANCE_NAME`_CSR_NAK ;  
    }
    return(dataByte);
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
    `$INSTANCE_NAME`_readBufIndex   = 0;
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
    `$INSTANCE_NAME`_writeBufIndex   = 0;
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
    return(`$INSTANCE_NAME`_readBufIndex);
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
   return(`$INSTANCE_NAME`_writeBufIndex);
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

#if defined(CYDEV_BOOTLOADER_IO_COMP) && (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`@INSTANCE_NAME`)
/*******************************************************************************
*
* Bootloadable defines, data and functions.
*
*******************************************************************************/

#if (defined(__C51__))
#include <intrins.h>
#endif // __C51__

#define CyBtLdrStartTimer(X, T)         {UniversalTime = T * 0x55; X = 0;}
#define CyBtLdrCheckTimer(X)            ((X++ < UniversalTime) ? 1:0)

/* Keep track of the checksum as data is read. */
uint16 ReadCheckSum;

/* We write to this. */
uint8 slReadBuf[BTLDR_SIZEOF_READ_BUFFER];

/* We read from this. */
uint8 slWriteBuf[BTLDR_SIZEOF_WRITE_BUFFER];


/* Transmit buffer vars */
uint8   CyBtLdr_readBufIn;      /* Index to add data to the Slave Transmit buffer */
uint8   CyBtLdr_readBufOut;     /* Index to remove data from the Slave Transmit buffer */

/* Receive buffer vars */
uint8   CyBtLdr_writeBufIn;     /* Index to add data to the Slave Receive buffer */
uint8   CyBtLdr_writeBufOut;    /* Index to remove data from the Slave Receive buffer*/

CY_ISR_PROTO(CyBtLdrI2cIsr);

uint32 UniversalTime;

/*******************************************************************************
* Function Name: BtLdrI2c_Start.
********************************************************************************
* Summary:
*  Starts the component and enables the interupt.  
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
*   This component automatically enables it's interrupt.  If I2C is enabled
*   without the interrupt enabled, it could lock up the I2C bus.
*
*******************************************************************************/
void CyBtldrCommStart(void)
{
    uint8 clkSel;

    `$INSTANCE_NAME`_EnableInt();
	`$INSTANCE_NAME`_SlaveInitReadBuf(slReadBuf, BTLDR_SIZEOF_READ_BUFFER);
	`$INSTANCE_NAME`_SlaveInitWriteBuf(slWriteBuf, BTLDR_SIZEOF_WRITE_BUFFER);

    /* Component function code  */
    /* Configure registers */
    /* Set the clock divider to the defined value */
    /* Find the first divider that is >= to the desired divider */
    /* The divider is in a 1,2,4,8,16,32,64 sequence */
    /* Default clock divider will be 16 (4) */
    /* The mode will be set sample at 16x and a prescaler of /4 */
    `$INSTANCE_NAME`_CLKDIV  = `$INSTANCE_NAME`_CLK_DIV_16;

    for(clkSel = 0; clkSel <= 6; clkSel++ )
    {
        if((1 << clkSel) >= `$INSTANCE_NAME`_DEFAULT_CLKDIV)
        {
            `$INSTANCE_NAME`_CLKDIV  = clkSel;
            break;
        }
    }

    /* Set interrupt vector */
    CyIntSetVector(`$INSTANCE_NAME`_I2C_IRQ__INTC_NUMBER, CyBtLdrI2cIsr);

    /* Clear Status register */
    `$INSTANCE_NAME`_CSR  = 0x00;

    /* Set Configuration, slave, rate, IE on stop and bus error, Pins **CHECK** */
    /* Set the prescaler to the 400k bps mode, which is really a divide by 4.  This will give the */
    /* Clock divider sufficient range to work bewteen 50 and 400 kbps.   */
    `$INSTANCE_NAME`_CFG  = `$INSTANCE_NAME`_CFG_CLK_RATE_400 | `$INSTANCE_NAME`_ENABLE_SLAVE | `$INSTANCE_NAME`_ENABLE_MASTER;

    /* Turn on hardware address detection and enable the clock */
    `$INSTANCE_NAME`_XCFG  = `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN | `$INSTANCE_NAME`_XCFG_CLK_EN;

    /* Clear all status flags */
    `$INSTANCE_NAME`_Status = 0x00; 

    /* Put state machine in idle state */
    `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE; 

    /* Enable power to I2C Module */
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;

    /* Set default status */
    CyBtldrCommReset();

    /* Set default address */
    `$INSTANCE_NAME`_ADDR  = `$INSTANCE_NAME`_DEFAULT_ADDR;
}


/*******************************************************************************
* Function Name: BtLdrI2c_Stop.
********************************************************************************
* Summary:
*  Disable the component and disable the interrupt.
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
void CyBtldrCommStop(void) 
{
    /* Disable Interrupt */
    `$INSTANCE_NAME`_DisableInt();

    /* Disable power to I2C Module */
    `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
}

/*******************************************************************************
**
**
**
**
**
*******************************************************************************/
void CyBtldrCommReset(void)
{
    /* Clear Transmit buffer. */
    CyBtLdr_readBufIn = 0;  
    CyBtLdr_readBufOut = 0; 

    /* Clear Recieve buffer. */
    CyBtLdr_writeBufIn = 0; 
    CyBtLdr_writeBufOut = 0;

    `$INSTANCE_NAME`_SlaveClearReadStatus();
    `$INSTANCE_NAME`_SlaveClearWriteStatus();
}


/*******************************************************************************
**
**
**
**
**
*******************************************************************************/
cystatus CyBtldrCommWrite(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut)
{
	uint8 Index;
    uint32 Time;
    cystatus Status = CYRET_SUCCESS; /* this code only sets the status if failed */


    /* Initialize. */
    Index = 0;

    if(Size >= CYBTLDR_TRANSMIT_SIZE)
    {
        Status = CYRET_BAD_PARAM;
    }
    /* Nothing should exist in the host read buffer. */
    else if(CyBtLdr_readBufIn == CyBtLdr_readBufOut)
    {
        /* Disable interrupts until we modify the host read buffer. */
        CYGlobalIntDisable;

        /* Fill the host read buffer. */
        for(; Index < Size; Index++)
        {
            CyBtLdr_readBufIn++;
            CyBtLdr_readBufIn &= CYBTLDR_TRANSMIT_MASK;
                 
            `$INSTANCE_NAME`_readBufPtr[CyBtLdr_readBufIn] = *Data++;
        }

        `$INSTANCE_NAME`_ACK_AND_RECEIVE;
		`$INSTANCE_NAME`_TRANSMIT_DATA;

        /* Enable interrupts so the host can read. */
        CYGlobalIntEnable;

        /* Start a timer to wait on. */
        CyBtLdrStartTimer(Time, TimeOut);

        /* Wait for the master to read it. */
        while(CyBtLdr_readBufIn != CyBtLdr_readBufOut && CyBtLdrCheckTimer(Time));
        {
			CY_NOP;
        }
    }
    else
    {
        Status = CYRET_INVALID_STATE;
    }
    
    /* Set the number of bytes the master read. */
    if(CyBtLdr_readBufIn == CyBtLdr_readBufOut)
    {
        *Count = 0;
    }
    else if(CyBtLdr_readBufOut < CyBtLdr_readBufIn)
    {
        *Count = (CyBtLdr_readBufIn - CyBtLdr_readBufOut);
    }
    else
    {
        *Count = (CYBTLDR_TRANSMIT_SIZE - CyBtLdr_readBufOut) + CyBtLdr_readBufIn;
    }
	
	return Status;
}


/*******************************************************************************
**
**
**
**
**
*******************************************************************************/
cystatus CyBtldrCommRead(uint8 * Data, uint16 Size, uint16 * Count, uint8 TimeOut)
{
	uint8 Value;
	uint16 Number;
    cystatus Status;
    uint32 Time;


	Number = 0;

    /* Start a timer to wait on. */
    CyBtLdrStartTimer(Time, TimeOut);


	while(Size && CyBtLdrCheckTimer(Time))
	{
		if((Number < Size) && (CyBtLdr_writeBufIn != CyBtLdr_writeBufOut))
		{
            /* Disable interrupts until we modify the host read buffer. */
            CYGlobalIntDisable;
        
            CyBtLdr_writeBufOut++;
            CyBtLdr_writeBufOut &= CYBTLDR_TRANSMIT_MASK;
            
			/* Get and save data. */
			Value = `$INSTANCE_NAME`_writeBufPtr[CyBtLdr_writeBufOut];

            /* Enable interrupts so the host can read. */
            CYGlobalIntEnable;

            /* Copy Data to the users buffer. */
			Data[Number++] = Value;

			/* Keep track of the checksum. */
			ReadCheckSum += (uint16) Value;
		
		    if(Number == Size)
		    {
			    break;
		    }
		}
	}

	if(Number || Size == 0)
	{
		*Count = Number;
	    Status = CYRET_SUCCESS;
	}
    else
    {
		*Count = 0;
	    Status = CYRET_EMPTY;
    }

	return Status;
}


/*******************************************************************************
* Function Name: CyBtLdrI2cIsr
********************************************************************************
* Summary:
*  Handle Interrupt Service Routine.  
*
* Parameters:  
*  (void)
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
CY_ISR(CyBtLdrI2cIsr)
{
    uint8  tmp8;
    uint8  tmpCsr;

    /* Entry from interrupt */
    /* In hardware address compare mode, we can assume we only get interrupted when */
    /* a valid address is recognized.  In software address compare mode, we have to */
    /* check every address after a start condition.                                 */

    tmpCsr = `$INSTANCE_NAME`_CSR;             /* Make temp copy so that we can check */
                                               /* for stop condition after we are done */


    if(tmpCsr & `$INSTANCE_NAME`_CSR_ADDRESS)  /* Check to see if a Start/Address is detected */
    {                                                       /* This is a Start or ReStart  */
                                                            /* So Reset the state machine  */
                                                            /* Check for a Read/Write condition  */


        if(`$INSTANCE_NAME`_DATA & `$INSTANCE_NAME`_READ_FLAG)  /* Check for read or write command */
        {
            /* Prepare next opeation to read, Get data and place in data register */
            if(CyBtLdr_readBufOut != CyBtLdr_readBufIn)
            {
                CyBtLdr_readBufOut++;
                CyBtLdr_readBufOut &= CYBTLDR_TRANSMIT_MASK;
                `$INSTANCE_NAME`_DATA = `$INSTANCE_NAME`_readBufPtr[CyBtLdr_readBufOut];   /* Load first data byte  */
                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;  /* ACK and transmit */
                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_BUSY;                         /* Set Read activity */
            }
            else   /* Data overflow */
            {
                `$INSTANCE_NAME`_DATA = 0xFF;    /* Out of range, send 0xFF  */
                /* Special case: udb needs to ack, ff needs to nak. */
                `$INSTANCE_NAME`_ACK_AND_TRANSMIT;  /* ACK and transmit */
                `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_BUSY | `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL; /* Set Read activity */
            }
            
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_SL_RD_DATA;                                /* Prepare for read transaction */
        }
        else  /* Start of a Write transaction, reset pointers, first byte is address */
        {
            /* Prepare next opeation to write offset */
            `$INSTANCE_NAME`_ACK_AND_RECEIVE; /* ACK and ready to receive addr */
            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_BUSY;       /* Set Write activity */
            `$INSTANCE_NAME`_State     = `$INSTANCE_NAME`_SM_SL_WR_DATA;       /* Prepare for read transaction */
            `$INSTANCE_NAME`_ENABLE_INT_ON_STOP;                               /* Enable interrupt on Stop */
        }
    }
    else if(tmpCsr & `$INSTANCE_NAME`_CSR_BYTE_COMPLETE)                          /* Check for data transfer */
    {

        if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_DATA)                /* Data write from Master to Slave. */
        {
            tmp8 = `$INSTANCE_NAME`_DATA;                                        /* Get data, to ACK quickly */
            `$INSTANCE_NAME`_ACK_AND_RECEIVE; /* ACK and ready to receive sub addr */
            CyBtLdr_writeBufIn++;
            CyBtLdr_writeBufIn &= CYBTLDR_RECIVE_MASK;
            `$INSTANCE_NAME`_writeBufPtr[CyBtLdr_writeBufIn] = tmp8; /* Write data to array */
        }
        else if (`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_RD_DATA)           /* Data Read from Slave to Master */
        {

            if((tmpCsr & `$INSTANCE_NAME`_CSR_LRB) == `$INSTANCE_NAME`_CSR_LRB_ACK)  
            {
                if(CyBtLdr_readBufOut != CyBtLdr_readBufIn) 
                {
                    CyBtLdr_readBufOut++;
                    CyBtLdr_readBufOut &= CYBTLDR_TRANSMIT_MASK;
                    `$INSTANCE_NAME`_DATA = `$INSTANCE_NAME`_readBufPtr[CyBtLdr_readBufOut]; /* Get data from array */
 
                    /* Send Data */
                    `$INSTANCE_NAME`_TRANSMIT_DATA;
                }
                else   /* Over flow */
                {
                    `$INSTANCE_NAME`_DATA = 0xFF;                                     /* Get data from array */

                    /* Send Data */
                    `$INSTANCE_NAME`_ACK_AND_TRANSMIT;
                    `$INSTANCE_NAME`_slStatus  |= `$INSTANCE_NAME`_SSTAT_RD_ERR_OVFL; /* Set overflow */
                }
            }
            else  /* Last byte NAKed, done */
            {
                `$INSTANCE_NAME`_DATA = 0xFF;                                   /* Get data from array */

                /* Send Data */
                `$INSTANCE_NAME`_TRANSMIT_DATA;
                `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_RD_BUSY;   /* Clear Busy Flag */
                `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_RD_CMPT;    /* Set complete Flag */
                `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;              /* Return to idle state */
            }

        }
        else  /* This is an invalid state and should not occur  */
        {
            `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;                 /* Invalid state, Reset */
            `$INSTANCE_NAME`_NAK_AND_RECEIVE;
        }  /* End Transfer mode */

    }  
    else if(tmpCsr & `$INSTANCE_NAME`_CSR_BUS_ERROR)                        /* Quick check for Error */
    {
        if( `$INSTANCE_NAME`_CSR & `$INSTANCE_NAME`_CSR_BUS_ERROR) 
        {
            /* May want to reset bus here CHECK */
        }
    }  /* end if */

    if( (`$INSTANCE_NAME`_CSR ) & `$INSTANCE_NAME`_CSR_STOP_STATUS)         /* Check if Stop was detected */
    {
        if(`$INSTANCE_NAME`_State == `$INSTANCE_NAME`_SM_SL_WR_DATA)
        {
            `$INSTANCE_NAME`_slStatus &= ~`$INSTANCE_NAME`_SSTAT_WR_BUSY;   /* Clear Busy Flag */
            `$INSTANCE_NAME`_slStatus |= `$INSTANCE_NAME`_SSTAT_WR_CMPT;    /* Set complete Flag */
            `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;
        }
        `$INSTANCE_NAME`_State = `$INSTANCE_NAME`_SM_IDLE;                  /* Error or Stop, reset state */
    }
}


/* (CYDEV_BOOTLOADER_IO_COMP == `@INSTANCE_NAME`) */
#endif
