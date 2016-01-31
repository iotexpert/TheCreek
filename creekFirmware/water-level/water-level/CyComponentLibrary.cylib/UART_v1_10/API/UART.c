/*******************************************************************************
* FILENAME: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
* `$CYD_VERSION`
*
* DESCRIPTION:
* This file provides all API functionality of the UART component
*
* NOTE:
* Any unusual or non-standard behavior should be noted here. Other-
* wise, this section should remain blank.
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include "`$INSTANCE_NAME`.h"


#if(`$INSTANCE_NAME`_TX_Enabled)
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
        #include "`$INSTANCE_NAME`_TXInternalInterrupt.h"
    #endif
#endif
#if(`$INSTANCE_NAME`_RX_Enabled)
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        #include "`$INSTANCE_NAME`_RXInternalInterrupt.h"
    #endif
#endif
#if(`$INSTANCE_NAME`_InternalClockUsed)
    #include "`$INSTANCE_NAME`_IntClock.h"
#endif

#if(`$INSTANCE_NAME`_TX_Enabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
uint8 `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERSIZE];
uint8 `$INSTANCE_NAME`_TXBUFFERREAD = 0;
uint8 `$INSTANCE_NAME`_TXBUFFERWRITE = 0;
#endif
#if(`$INSTANCE_NAME`_RX_Enabled && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4))
uint8 `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERSIZE];
uint8 `$INSTANCE_NAME`_RXBUFFERREAD = 0;
uint8 `$INSTANCE_NAME`_RXBUFFERWRITE = 0;
#endif



uint8 `$INSTANCE_NAME`_initvar = 0;
/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_Start(void)
*
* Summary:
* Initialize and Enable the UART component
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Enable the clock input to enable operation
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    if(`$INSTANCE_NAME`_initvar == 0)
	{
		`$INSTANCE_NAME`_initvar = 1;
        
        #if(`$INSTANCE_NAME`_RX_Enabled)
        
            #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4))
				`$INSTANCE_NAME`_RXInternalInterrupt_Start();
                `$INSTANCE_NAME`_RXInternalInterrupt_SetVector(`$INSTANCE_NAME`_RXISR);
            #endif
            
           
            #if (`$INSTANCE_NAME`_RXHW_Address_Enabled)
                `$INSTANCE_NAME`_SetRxAddressMode(0x00); //TODO: Define Address Modes in ENUM
                `$INSTANCE_NAME`_SetRxAddress1(`$INSTANCE_NAME`_RXHWADDRESS1);
                `$INSTANCE_NAME`_SetRxAddress2(`$INSTANCE_NAME`_RXHWADDRESS2);
            #endif 
            
            /* Configure the Initial RX interrupt mask */
            `$INSTANCE_NAME`_RXSTATUS_MASK  = `$INSTANCE_NAME`_INIT_RX_INTERRUPTS_MASK;
            `$INSTANCE_NAME`_RXSTATUS_ACTL  |= `$INSTANCE_NAME`_INT_ENABLE;
        #endif
        #if(`$INSTANCE_NAME`_TX_Enabled)
            #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
				`$INSTANCE_NAME`_TXInternalInterrupt_Start();
                `$INSTANCE_NAME`_TXInternalInterrupt_SetVector(`$INSTANCE_NAME`_TXISR);
            #endif
            
			  //Write Counter Value for TX Bit Clk Generator 
			 #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
				`$INSTANCE_NAME`_TXBITCLKGEN_CTR = `$INSTANCE_NAME`_BIT_CENTER;
				`$INSTANCE_NAME`_TXBITCLKTX_COMPLETE = (`$INSTANCE_NAME`_NUMBER_OF_DATA_BITS +
														`$INSTANCE_NAME`_NUMBER_OF_START_BIT) *
														`$INSTANCE_NAME`_OverSampleCount;
			#endif	
            
            /* Configure the Initial TX interrupt mask */
            #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
                `$INSTANCE_NAME`_TXSTATUS_MASK = `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY;
            #else
                `$INSTANCE_NAME`_TXSTATUS_MASK = `$INSTANCE_NAME`_INIT_TX_INTERRUPTS_MASK;
            #endif
            `$INSTANCE_NAME`_TXSTATUS_ACTL  |= `$INSTANCE_NAME`_INT_ENABLE;

        #endif
    }

    //`$INSTANCE_NAME`_CONTROL |= (`$INSTANCE_NAME`_CTRL_RXRUN | `$INSTANCE_NAME`_CTRL_TXRUN);
    /*Write Bit Counter Enable */
     #if(`$INSTANCE_NAME`_RX_Enabled)
	    `$INSTANCE_NAME`_RXBITCTR_CONTROL |= `$INSTANCE_NAME`_CNTR_ENABLE;
	 #endif	
	 
     #if(`$INSTANCE_NAME`_TX_Enabled)
		 #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
		 	/* TODO: Start DP*/
		#else
			`$INSTANCE_NAME`_TXBITCTR_CONTROL |= `$INSTANCE_NAME`_CNTR_ENABLE;
		#endif	
	 #endif	
	
    #if(`$INSTANCE_NAME`_InternalClockUsed)
        `$INSTANCE_NAME`_IntClock_Enable();
    #else
        //TODO: Enable the External Clock from the information provided?
    #endif
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_Stop(void)
*
* Summary:
* Disable the UART component
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Disable the clock input to enable operation
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
	//`$INSTANCE_NAME`_CONTROL &= ~(`$INSTANCE_NAME`_CTRL_RXRUN | `$INSTANCE_NAME`_CTRL_RXRUN);
    /*Write Bit Counter Disable */
   #if(`$INSTANCE_NAME`_RX_Enabled)
	    `$INSTANCE_NAME`_RXBITCTR_CONTROL &= ~`$INSTANCE_NAME`_CNTR_ENABLE;
   #endif	
	 
   #if(`$INSTANCE_NAME`_TX_Enabled)
     #if(`$INSTANCE_NAME`_TX_Enabled)
		 #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
		 	/* TODO: Stop DP*/
		#else
			`$INSTANCE_NAME`_TXBITCTR_CONTROL &= ~`$INSTANCE_NAME`_CNTR_ENABLE;
		#endif	
	 #endif	
   #endif	
	

   #if(`$INSTANCE_NAME`_InternalClockUsed)
        `$INSTANCE_NAME`_IntClock_Disable();
   #else
        //TODO: Enable the External Clock from the information provided?
   #endif
   
   
   /*Disable internal interrupt component*/
   #if(`$INSTANCE_NAME`_RX_Enabled)
       #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4))
			`$INSTANCE_NAME`_RXInternalInterrupt_Stop();
       #endif
   #endif
   #if(`$INSTANCE_NAME`_TX_Enabled)
       #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
			`$INSTANCE_NAME`_TXInternalInterrupt_Stop();
       #endif
   #endif
	
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_ReadControlRegister(void)
*
* Summary:
* Read the current state of the control register
*
* Parameters:
* -None-
*
* Return:
* uint8: Current state of the control register.
*
* Theory:
*
* Side Effects:
*  -None-
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadControlRegister(void)
{
  #if( `$INSTANCE_NAME`_CONTROL_REMOVED )
    return 0;	
  #else 
    return `$INSTANCE_NAME`_CONTROL;
  #endif
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_WriteControlRegister(void)
*
* Summary:
* Read the current state of the control register
*
* Parameters:
* -None-
*
* Return:
* uint8: Current state of the control register.
*
* Theory:
*
* Side Effects:
*  -None-
*******************************************************************************/
void  `$INSTANCE_NAME`_WriteControlRegister(uint8 control)
{
  #if( `$INSTANCE_NAME`_CONTROL_REMOVED )
  	   control = control;
  #else
       `$INSTANCE_NAME`_CONTROL = control;
  #endif
}


#if(`$INSTANCE_NAME`_RX_Enabled)
#if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_EnableRxInt(void)
*
* Summary:
* Enable RX interrupt generation
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Enable the interrupt output -or- the interrupt component itself
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_EnableRxInt(void)
{
    `$INSTANCE_NAME`_RXInternalInterrupt_Enable();
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_DisableRxInt(void)
*
* Summary:
* Disable RX interrupt generation
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Disable the interrupt output -or- the interrupt component itself
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_DisableRxInt(void)
{
	`$INSTANCE_NAME`_RXInternalInterrupt_Disable();
}
#endif /* `$INSTANCE_NAME`_RX_IntInterruptEnabled */

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetRxInterruptMode(uint8 intSrc)
*
* Summary:
* Configure which status bits trigger an interrupt event
*
* Parameters:
* IntSource: An or'd combination of the desired status bit masks (defined in
*                the header file)
*
* Return:
* -None-
*
* Theory:
* Enables the output of specific status bits to the interrupt controller
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_SetRxInterruptMode(uint8 intSrc)
{
    `$INSTANCE_NAME`_RXSTATUS_MASK  = intSrc;
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_ReadRxData(void)
*
* Summary:
* Grab the next available byte of data from the recieve FIFO    
*
* Parameters:
* -None-
*
* Return:
* uint8: Data byte
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadRxData(void)
{
    uint8 rxData;

#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)

    /* Disable Rx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableRxInt();
    #endif

    if(`$INSTANCE_NAME`_RXBUFFERREAD != `$INSTANCE_NAME`_RXBUFFERWRITE)
    {
        `$INSTANCE_NAME`_RXBUFFERREAD++;
    
        if(`$INSTANCE_NAME`_RXBUFFERREAD >= `$INSTANCE_NAME`_RXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_RXBUFFERREAD = 0;
        }
        
        rxData = `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERREAD];
    }
    else
    {   /* Needs to check status for RX_STS_FIFO_NOTEMPTY bit*/
        rxData = `$INSTANCE_NAME`_RXDATA;
    }

    /* Enable Rx interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_EnableRxInt();
    #endif

#else /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */

    /* Needs to check status for RX_STS_FIFO_NOTEMPTY bit*/
    rxData = `$INSTANCE_NAME`_RXDATA;

#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */

	return rxData;
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_ReadRxStatus(void)
*
* Summary:
* Read the current state of the status register
*
* Parameters:
* -None-
*
* Return:
* uint8: Current state of the status register.
*
* Theory:
*
* Side Effects:
* TODO: Maybe if there are clear on read bits here?
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadRxStatus(void)
{
	return `$INSTANCE_NAME`_RXSTATUS;
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetRxInterruptSource(void)
*
* Summary:
* Read the current state of the status register for interrupt sources
*
* Parameters:
* -None-
*
* Return:
* uint8: Current state of the status register.
*
* Theory:
*
* Side Effects:
* TODO: Should this AND off bits that are enabled as interrupt masks?
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetRxInterruptSource()
{
    return `$INSTANCE_NAME`_RXSTATUS;
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetChar(void)
*
* Summary:
* Grab the next available byte of data from the recieve FIFO
*
* Parameters:
* -None-
*
* Return:
* uint8: Data byte
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetChar(void)
{
    uint8 rxData=0;
	uint8 rxStatus; 


#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)

    /* Disable Rx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableRxInt();
    #endif

    if(`$INSTANCE_NAME`_RXBUFFERREAD != `$INSTANCE_NAME`_RXBUFFERWRITE)
    {
        `$INSTANCE_NAME`_RXBUFFERREAD++;
    
        if(`$INSTANCE_NAME`_RXBUFFERREAD >= `$INSTANCE_NAME`_RXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_RXBUFFERREAD = 0;
        }
        
        rxData = `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERREAD];
    }
    else
    {   rxStatus =`$INSTANCE_NAME`_RXSTATUS;
		if(rxStatus & `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY)
		{/* Read received data from FIFO*/
		 rxData = `$INSTANCE_NAME`_RXDATA;
		 /*Check status on error*/
         if(rxStatus & (`$INSTANCE_NAME`_RX_STS_BREAK | `$INSTANCE_NAME`_RX_STS_PAR_ERROR | 
		 			    `$INSTANCE_NAME`_RX_STS_STOP_ERROR | `$INSTANCE_NAME`_RX_STS_OVERRUN))
		   rxData = 0;
		 }
    }

    /* Enable Rx interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_EnableRxInt();
    #endif

#else /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */

        rxStatus =`$INSTANCE_NAME`_RXSTATUS;
		if(rxStatus & `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY)
		{/* Read received data from FIFO*/
		 rxData = `$INSTANCE_NAME`_RXDATA;
		 /*Check status on error*/
         if(rxStatus & (`$INSTANCE_NAME`_RX_STS_BREAK | `$INSTANCE_NAME`_RX_STS_PAR_ERROR | 
		 			    `$INSTANCE_NAME`_RX_STS_STOP_ERROR | `$INSTANCE_NAME`_RX_STS_OVERRUN))
		   rxData = 0;
		 }

#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */

	return rxData;

}

/*******************************************************************************
* FUNCTION NAME: uint16 `$INSTANCE_NAME`_GetByte(void)
*
* Summary:
* Grab the next available byte of data from the recieve FIFO
*
* Parameters:
* -None-
*
* Return:
* uint16: Status Register Byte and Data byte
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/

uint16 `$INSTANCE_NAME`_GetByte(void)
{

#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)
    /* TODO: change status if local buffer used*/
	return ( (`$INSTANCE_NAME`_RXSTATUS << 8) | `$INSTANCE_NAME`_ReadRxData());
#else
	return ( (`$INSTANCE_NAME`_RXSTATUS << 8) | `$INSTANCE_NAME`_ReadRxData() );
#endif
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetRxBufferSize(void)
*
* Summary:
* Determine the amount of space left in the RX buffer and return the count in
*   bytes
*
* Parameters:
* -None-
*
* Return:
* uint8: Integer count of the number of bytes left in the RX buffer
*
* Theory:
* Allows the user to find out how full the RX Buffer is.
*
* Side Effects:
* -None-
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetRxBufferSize(void)
{
    uint8 size;


#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)

    /* Disable Rx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableRxInt();
    #endif

    if(`$INSTANCE_NAME`_RXBUFFERREAD == `$INSTANCE_NAME`_RXBUFFERWRITE)
    {
        size = 0;
    }
    else if(`$INSTANCE_NAME`_RXBUFFERREAD < `$INSTANCE_NAME`_RXBUFFERWRITE)
    {
        size = (`$INSTANCE_NAME`_RXBUFFERWRITE - `$INSTANCE_NAME`_RXBUFFERREAD);
    }
    else
    {
        size = (`$INSTANCE_NAME`_RXBUFFERSIZE - `$INSTANCE_NAME`_RXBUFFERREAD) + `$INSTANCE_NAME`_RXBUFFERWRITE;
    }

    /* Enable Rx interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_EnableRxInt();
    #endif

#else /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */

    /* We can only know if there is data in the fifo. */
    size = (`$INSTANCE_NAME`_RXSTATUS & `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY) ? 1:0;

#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */


    return size;
}

//for 32-bit PSoC3
//uint16 `$INSTANCE_NAME`_GetRxBufferSize(void)
//{
//}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_ClearRxBuffer(void)
*
* Summary:
* Clear the RX RAM buffer by setting the read and write pointers both to zero.
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Setting the pointers to zero makes the system believe there is no data to read
*        and writing will resume at address 0 overwriting any data that may have
*        remained in the RAM.
*
* Side Effects:
* Any received data not read from the RAM buffer will be lost when overwritten.
*******************************************************************************/
void `$INSTANCE_NAME`_ClearRxBuffer(void)
{
#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)
    /* Disable Rx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableRxInt();
    #endif

    `$INSTANCE_NAME`_RXBUFFERREAD = 0;
    `$INSTANCE_NAME`_RXBUFFERWRITE = 0;

    /* Enable Rx interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_EnableRxInt();
    #endif
#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetRxAddressMode( uint8 addressMode )
*
* Summary:
* Set the receive addressing mode
*
* Parameters:
* addressMode: Use one of the Enumerated Types listed below
        `#cy_declare_enum B_UART__AddressModes`
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_SetRxAddressMode(uint8 addressMode)
{
  #if(`$INSTANCE_NAME`_RXHW_Address_Enabled) 
	uint8 tmpCtrl = 0;
    tmpCtrl = `$INSTANCE_NAME`_CONTROL & ~`$INSTANCE_NAME`_CTRL_RXADDR_MODE_MASK;
    tmpCtrl |= (addressMode & `$INSTANCE_NAME`_CTRL_RXADDR_MODE_MASK);
    `$INSTANCE_NAME`_CONTROL = tmpCtrl;
  #else
    addressMode = addressMode;
  #endif	
	
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetRxAddress1(uint8 address)
*
* Summary:
* Set the first hardware address compare value
*
* Parameters:
* address: 
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_SetRxAddress1(uint8 address)
{
	`$INSTANCE_NAME`_RXADDRESS1 = address;
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetRxAddress2(uint8 address)
*
* Summary:
* Set the second hardware address compare value
*
* Parameters:
* address: 
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_SetRxAddress2(uint8 address)
{
	`$INSTANCE_NAME`_RXADDRESS2 = address;
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_LoadRxConfig( void )
*
* Summary:
* //TODO: ??
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_LoadRxConfig(void)
{
	//TODO: What is the definition of LoadConfig?
}
#endif  /* `$INSTANCE_NAME`_RX_Enabled */

#if(`$INSTANCE_NAME`_TX_Enabled)
#if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_EnableTxInt(void)
*
* Summary:
* Enable TX interrupt generation
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Enable the interrupt output -or- the interrupt component itself
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_EnableTxInt(void)
{
	`$INSTANCE_NAME`_TXInternalInterrupt_Enable();
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_DisableTxInt(void)
*
* Summary:
* Disable TX interrupt generation
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Disable the interrupt output -or- the interrupt component itself
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_DisableTxInt(void)
{
	`$INSTANCE_NAME`_TXInternalInterrupt_Disable();
}
#endif /* `$INSTANCE_NAME`_TX_IntInterruptEnabled */

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetTxInterruptMode(uint8 intSrc)
*
* Summary:
* Configure which status bits trigger an interrupt event
*
* Parameters:
* intSrc: An or'd combination of the desired status bit masks (defined in
*                the header file)
*
* Return:
* -None-
*
* Theory:
* Enables the output of specific status bits to the interrupt controller
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_SetTxInterruptMode(uint8 intSrc)
{
    `$INSTANCE_NAME`_TXSTATUS_MASK = intSrc;
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_WriteTxData(uint8 txDataByte)
*
* Summary:
* Write a byte of data to the Transmit FIFO
*
* Parameters:
* TXDataByte: byte of data to place in the transmit FIFO
*
* Return:
* -None-
*
* Theory:
* Allows the user and the API to read the status register for error detection
*         and flow control.
*
* Side Effects:
* How does this affect the interrupts if it has not been serviced yet?
*******************************************************************************/
void `$INSTANCE_NAME`_WriteTxData(uint8 txDataByte)
{
#if(`$INSTANCE_NAME`_TXBUFFERSIZE > 4)

    /* Block if buffer is full, so we dont overwrite. */
    while(`$INSTANCE_NAME`_TXBUFFERWRITE == (`$INSTANCE_NAME`_TXBUFFERREAD - 1) ||
          (int8)(`$INSTANCE_NAME`_TXBUFFERWRITE - `$INSTANCE_NAME`_TXBUFFERREAD) == (int8)(`$INSTANCE_NAME`_TXBUFFERSIZE - 1))
    {
        /* Software buffer is full. */
    }

    /* Disable Tx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableTxInt();
    #endif

	if((`$INSTANCE_NAME`_TXBUFFERREAD == `$INSTANCE_NAME`_TXBUFFERWRITE) && !(`$INSTANCE_NAME`_TXSTATUS & `$INSTANCE_NAME`_TX_STS_FIFO_FULL))
	{
        /* Add directly to the FIFO. */
		`$INSTANCE_NAME`_TXDATA = txDataByte;
	}
    else
    {
        /* Add to the software buffer. */
        `$INSTANCE_NAME`_TXBUFFERWRITE++;
        if(`$INSTANCE_NAME`_TXBUFFERWRITE >= `$INSTANCE_NAME`_TXBUFFERSIZE)
            `$INSTANCE_NAME`_TXBUFFERWRITE = 0;

        `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERWRITE] = txDataByte;
    }

    /* Enable Rx interrupt. */
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
        `$INSTANCE_NAME`_EnableTxInt();
    #endif

#else /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

    /* Block if there isnt room. */
	while(`$INSTANCE_NAME`_TXSTATUS & `$INSTANCE_NAME`_TX_STS_FIFO_FULL);

    /* Add directly to the FIFO. */
	`$INSTANCE_NAME`_TXDATA = txDataByte;

#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_ReadTxStatus(void)
*
* Summary:
* Read the status register for the component
*
* Parameters:
* -None-
*
* Return:
* uint8: Contents of the status register
*
* Theory:
* Allows the user and the API to read the status register for error detection
*         and flow control.
*
* Side Effects:
* How does this affect the interrupts if it has not been serviced yet?
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadTxStatus(void)
{
	return (`$INSTANCE_NAME`_TXSTATUS);
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetTxInterruptSource(void)
*
* Summary:
* Read the current state of the status register for interrupt sources
*
* Parameters:
* -None-
*
* Return:
* uint8: Current state of the status register.
*
* Theory:
*
* Side Effects:
* TODO: Should this AND off bits that are enabled as interrupt masks?
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetTxInterruptSource()
{
    return `$INSTANCE_NAME`_TXSTATUS;
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_PutChar(uint8 txDataByte)
*
* Summary:
* Write a byte of data to be sent across the UART
*
* Parameters:
* TxDataByte: The 8-bit data value to send across the UART
*
* Return:
* -None-
*
* Theory:
* Allows the user to transmit any byte of data in a single transfer
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutChar(uint8 txDataByte)
{
#if(`$INSTANCE_NAME`_TXBUFFERSIZE > 4)
		`$INSTANCE_NAME`_WriteTxData(txDataByte);
#else /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

    /* Block if there isnt room. */
	while(`$INSTANCE_NAME`_TXSTATUS & `$INSTANCE_NAME`_TX_STS_FIFO_FULL);

    /* Add directly to the FIFO. */
	`$INSTANCE_NAME`_TXDATA = txDataByte;

#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_PutString( uint8* ramString )
*
* Summary:
* Write a Sequence of bytes on the Transmit line. Data comes from RAM.
*
* Parameters:
* ramString: uint8 pointer to character string of Data to Send
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutString(uint8* ramString)
{
	//This is a blocking function, it will not exit until all data is sent
	while(*ramString != 0)	
	{
		`$INSTANCE_NAME`_WriteTxData(*ramString++);
	}
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_PutStringConst( uint8* romString )
*
* Summary:
* Write a Sequence of bytes on the Transmit line. Data comes from ROM.
*
* Parameters:
* romString: uint8 pointer to character string of Data to Send
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutStringConst(uint8* romString)
{
	//This is a blocking function, it will not exit until all data is sent
	while(*romString != 0)	
	{
		`$INSTANCE_NAME`_WriteTxData(*romString++);
	}
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_PutArray( uint8* ramString, int8 byteCount )
*
* Summary:
* Write a Sequence of bytes on the Transmit line. Data comes from RAM.
*
* Parameters:
* ramString: uint8 pointer to character string of Data to Send
* byteCount: Number of Bytes to be transmitted
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutArray(uint8* ramString, int8 byteCount)
{
	while(byteCount > 0)
	{
		`$INSTANCE_NAME`_WriteTxData(*ramString++);
		byteCount--;
	}	
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_PutArrayConst( uint8* romString, int8 byteCount )
*
* Summary:
* Write a Sequence of bytes on the Transmit line. Data comes from ROM.
*
* Parameters:
* ramString: uint8 pointer to character string of Data to Send
* byteCount: Number of Bytes to be transmitted
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutArrayConst(uint8* romString, int8 byteCount)
{
	while(byteCount > 0)
	{
		`$INSTANCE_NAME`_WriteTxData(*romString++);
		byteCount--;
    }
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_PutCRLF( uint8 txDataByte )
*
* Summary:
* Write a character and then carriage return and line feed.
*
* Parameters:
* txDataByte: uint8 Character to send
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutCRLF(uint8 txDataByte)
{
    `$INSTANCE_NAME`_WriteTxData(txDataByte);
    `$INSTANCE_NAME`_WriteTxData(0x0D);
    `$INSTANCE_NAME`_WriteTxData(0x0A);
}

/* 8-bit version */
/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetTxBufferSize(void)
*
* Summary:
* Determine the amount of space left in the TX buffer and return the count in
*   bytes
*
* Parameters:
* -None-
*
* Return:
* uint8: Integer count of the number of bytes left in the TX buffer
*
* Theory:
* Allows the user to find out how full the TX Buffer is.
*
* Side Effects:
* -None-
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetTxBufferSize(void)
{
    uint8 size;


#if(`$INSTANCE_NAME`_TXBUFFERSIZE > 4)

    /* Disable Tx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableTxInt();
    #endif

    if(`$INSTANCE_NAME`_TXBUFFERREAD == `$INSTANCE_NAME`_TXBUFFERWRITE)
    {
        size = 0;
    }
    else if(`$INSTANCE_NAME`_TXBUFFERREAD < `$INSTANCE_NAME`_TXBUFFERWRITE)
    {
        size = (`$INSTANCE_NAME`_TXBUFFERWRITE - `$INSTANCE_NAME`_TXBUFFERREAD);
    }
    else
    {
        size = (`$INSTANCE_NAME`_TXBUFFERSIZE - `$INSTANCE_NAME`_TXBUFFERREAD) + `$INSTANCE_NAME`_TXBUFFERWRITE;
    }

    /* Enable Tx interrupt. */
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
        `$INSTANCE_NAME`_EnableTxInt();
    #endif

#else /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

    size = `$INSTANCE_NAME`_TXSTATUS;

    /* Is the fifo is full. */
    if(size & `$INSTANCE_NAME`_TX_STS_FIFO_FULL)
    {
        size = 4;
    }
    else if(size & `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY)
    {
        size = 0;
    }
    else
    {
        /* We only know there is data in the fifo. */
        size = 1;
    }

#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

    return size;
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_ClearTxBuffer(void)
*
* Summary:
* Clear the TX RAM buffer by setting the read and write pointers both to zero.
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* Setting the pointers to zero makes the system believe there is no data to read
*        and writing will resume at address 0 overwriting any data that may have
*        remained in the RAM.
*
* Side Effects:
* Any received data not read from the RAM buffer will be lost when overwritten.
*******************************************************************************/
void `$INSTANCE_NAME`_ClearTxBuffer(void)
{
#if(`$INSTANCE_NAME`_TXBUFFERSIZE > 4)

    /* Disable Tx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableTxInt();
    #endif

    `$INSTANCE_NAME`_TXBUFFERREAD = 0;
    `$INSTANCE_NAME`_TXBUFFERWRITE = 0;

    /* Enable Tx interrupt. */
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
        `$INSTANCE_NAME`_EnableTxInt();
    #endif

#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SendBreak( void )
*
* Summary:
* Write a Break command to the UART
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_SendBreak(void)
{
    /*Set the Counter to 13-bits and transmit a 00 byte*/
    /*When that is done then reset the counter value back*/
	uint8 lastPeriod;
	uint8 tmpStat=0;

	 #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
	    lastPeriod=`$INSTANCE_NAME`_TXBITCLKTX_COMPLETE;
		`$INSTANCE_NAME`_TXBITCLKTX_COMPLETE = `$INSTANCE_NAME`_TXBITCTR_13BITS;
	#else
		lastPeriod=`$INSTANCE_NAME`_TXBITCTR_PERIOD;
		`$INSTANCE_NAME`_TXBITCTR_PERIOD = `$INSTANCE_NAME`_TXBITCTR_13BITS;
	#endif	

	/* Send zeros*/
	`$INSTANCE_NAME`_TXDATA = 0;

	/*wait till transmit start*/
	do{tmpStat=`$INSTANCE_NAME`_ReadTxStatus();		
	   }while(tmpStat&`$INSTANCE_NAME`_TX_STS_COMPLETE);

	/*wait till transmit complete*/
	do{tmpStat=`$INSTANCE_NAME`_ReadTxStatus();		
	   }while(~tmpStat&`$INSTANCE_NAME`_TX_STS_COMPLETE);

	 #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
		`$INSTANCE_NAME`_TXBITCLKTX_COMPLETE = lastPeriod;
	#else
		`$INSTANCE_NAME`_TXBITCTR_PERIOD=lastPeriod;
	#endif	


}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetTxAddressMode( uint8 addressMode )
*
* Summary:
* Set the transmit addressing mode
*
* Parameters:
* addressMode: Enumerated type?? //TODO:
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_SetTxAddressMode(uint8 addressMode)
{
	//TODO: What modes are there for TX addressing.  You should only send an Address byte
    //      which sets the control register bit then transmits the data.  It should check if
    //      the FIFO has data before doing this so that an unexpected thing doesn't get the address bit.    
	uint8 temp = addressMode;
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_HardwareAddressEnable( uint8 addressEnable )
*
* Summary:
* Enable Hardware Addressing mode 
*
* Parameters:
* addressMode: Address to respond to  //TODO: More Information needed.
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_HardwareAddressEnable(uint8 addressEnable)
{
	//TODO: Hardware Addressing is not yet supported
	uint8 temp = addressEnable;
}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_LoadTxConfig(void)
*
* Summary:
* //TODO: ?? 
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_LoadTxConfig(void)
{
	//TODO: ??
}
#endif  //`$INSTANCE_NAME`_TX_Enabled
