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

#if(`$INSTANCE_NAME`_PSOC32bit)	
/*for 32-bit PSoC5*/
	#if(`$INSTANCE_NAME`_TX_Enabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
	uint8 `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERSIZE];
	uint16 `$INSTANCE_NAME`_TXBUFFERREAD = 0;
	uint16 `$INSTANCE_NAME`_TXBUFFERWRITE = 0;
	#endif
	#if(`$INSTANCE_NAME`_RX_Enabled && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4))
	uint8 `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERSIZE];
	uint16 `$INSTANCE_NAME`_RXBUFFERREAD = 0;
	uint16 `$INSTANCE_NAME`_RXBUFFERWRITE = 0;
	uint8 `$INSTANCE_NAME`_RXBUFFERLOOPDETECT = 0;
    uint8  `$INSTANCE_NAME`_RXBUFFER_OVERFLOW = 0;
	#endif
#else
/*for 8 bit PSoc3*/	
	#if(`$INSTANCE_NAME`_TX_Enabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
	uint8 `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERSIZE];
	uint8 `$INSTANCE_NAME`_TXBUFFERREAD = 0;
	uint8 `$INSTANCE_NAME`_TXBUFFERWRITE = 0;
	#endif
	#if(`$INSTANCE_NAME`_RX_Enabled && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4))
	uint8 `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERSIZE];
	uint8 `$INSTANCE_NAME`_RXBUFFERREAD = 0;
	uint8 `$INSTANCE_NAME`_RXBUFFERWRITE = 0;
	uint8 `$INSTANCE_NAME`_RXBUFFERLOOPDETECT = 0;
    uint8  `$INSTANCE_NAME`_RXBUFFER_OVERFLOW = 0;
	#endif
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
                /* Set the RX Interrupt. */
                CyIntSetVector(`$INSTANCE_NAME`_RX_VECT_NUM,   `$INSTANCE_NAME`_RXISR);
                CyIntSetPriority(`$INSTANCE_NAME`_RX_VECT_NUM, `$INSTANCE_NAME`_RX_PRIOR_NUM);
                CyIntEnable(`$INSTANCE_NAME`_RX_VECT_NUM);
            #endif
           
            #if (`$INSTANCE_NAME`_RXHW_Address_Enabled)
                `$INSTANCE_NAME`_SetRxAddressMode(`$INSTANCE_NAME`_RXAddressMode); 
                `$INSTANCE_NAME`_SetRxAddress1(`$INSTANCE_NAME`_RXHWADDRESS1);
                `$INSTANCE_NAME`_SetRxAddress2(`$INSTANCE_NAME`_RXHWADDRESS2);
            #endif 
            
            /* Configure the Initial RX interrupt mask */
            `$INSTANCE_NAME`_RXSTATUS_MASK  = `$INSTANCE_NAME`_INIT_RX_INTERRUPTS_MASK;
            `$INSTANCE_NAME`_RXSTATUS_ACTL  |= `$INSTANCE_NAME`_INT_ENABLE;
        #endif
        #if(`$INSTANCE_NAME`_TX_Enabled)
            #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
                /* Set the TX Interrupt. */
                CyIntSetVector(`$INSTANCE_NAME`_TX_VECT_NUM,   `$INSTANCE_NAME`_TXISR);
                CyIntSetPriority(`$INSTANCE_NAME`_TX_VECT_NUM, `$INSTANCE_NAME`_TX_PRIOR_NUM);
                CyIntEnable(`$INSTANCE_NAME`_TX_VECT_NUM);
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
        /* Set the bit to enable the clock. */
        *`$INSTANCE_NAME`_IntClock_CLKEN |= `$INSTANCE_NAME`_IntClock_CLKEN_MASK;
    #else
       
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
    /*Write Bit Counter Disable */
   #if(`$INSTANCE_NAME`_RX_Enabled)
	    `$INSTANCE_NAME`_RXBITCTR_CONTROL &= ~`$INSTANCE_NAME`_CNTR_ENABLE;
   #endif	
	 
   #if(`$INSTANCE_NAME`_TX_Enabled)
     #if(`$INSTANCE_NAME`_TX_Enabled)
		#if(`$INSTANCE_NAME`_TXCLKGEN_DP)
		 	// stop DP 
		#else
			`$INSTANCE_NAME`_TXBITCTR_CONTROL &= ~`$INSTANCE_NAME`_CNTR_ENABLE;
		#endif	
	 #endif	
   #endif	
	

   #if(`$INSTANCE_NAME`_InternalClockUsed)
        /* Clear the bit to enable the clock. */
        *`$INSTANCE_NAME`_IntClock_CLKEN &= ~`$INSTANCE_NAME`_IntClock_CLKEN_MASK;
   #else
       
   #endif
   
   
   /*Disable internal interrupt component*/
   #if(`$INSTANCE_NAME`_RX_Enabled)
       #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4))
            `$INSTANCE_NAME`_DisableRxInt();
       #endif
   #endif
   #if(`$INSTANCE_NAME`_TX_Enabled)
       #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
            `$INSTANCE_NAME`_DisableTxInt();
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
    CyIntEnable(`$INSTANCE_NAME`_RX_VECT_NUM);
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
	CyIntDisable(`$INSTANCE_NAME`_RX_VECT_NUM);
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
*  Returns data in RX Data register without checking status register to determine 
*  if data is valid
*
* Parameters:
*  void
*
* Return:
*  uint8: Received data from RX register
*
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

    if( (`$INSTANCE_NAME`_RXBUFFERREAD != `$INSTANCE_NAME`_RXBUFFERWRITE) ||
		(`$INSTANCE_NAME`_RXBUFFERLOOPDETECT > 0) )
    {
        if(`$INSTANCE_NAME`_RXBUFFERLOOPDETECT > 0 ) `$INSTANCE_NAME`_RXBUFFERLOOPDETECT = 0;
		
        if(`$INSTANCE_NAME`_RXBUFFERREAD >= `$INSTANCE_NAME`_RXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_RXBUFFERREAD = 0;
        }

		rxData = `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERREAD];

		`$INSTANCE_NAME`_RXBUFFERREAD++;
    
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
*  Read the current state of the status register
*  And detect software buffer overflow.
*
* Parameters:
*  void
*
* Return:
* uint8: Current state of the status register.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadRxStatus(void)
{
 uint8 status;
    
    status = `$INSTANCE_NAME`_RXSTATUS;
    status &= `$INSTANCE_NAME`_RX_HW_MASK;
    
	#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)
       if( `$INSTANCE_NAME`_RXBUFFER_OVERFLOW )
       {
           status |= `$INSTANCE_NAME`_RX_STS_SOFT_BUFF_OVER;
           `$INSTANCE_NAME`_RXBUFFER_OVERFLOW = 0;
       }
	#endif
    
	return status;
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
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetRxInterruptSource()
{
    return `$INSTANCE_NAME`_RXSTATUS;
}

/*******************************************************************************
* FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetChar(void)
*
* Summary:
*  Reads UART RX buffer immediately, if data is not available or an error condition 
*  exists, zero is returned; otherwise, character is read and returned.
*
* Parameters:
* -None-
*
* Return:
* uint8: Character read from UART RX buffer. ASCII characters from 1 to 255 are valid. 
* A returned zero signifies an error condition or no data available.
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

    if( (`$INSTANCE_NAME`_RXBUFFERREAD != `$INSTANCE_NAME`_RXBUFFERWRITE) ||
		(`$INSTANCE_NAME`_RXBUFFERLOOPDETECT > 0) )
    {
        if(`$INSTANCE_NAME`_RXBUFFERLOOPDETECT > 0 ) `$INSTANCE_NAME`_RXBUFFERLOOPDETECT = 0;
       
        if(`$INSTANCE_NAME`_RXBUFFERREAD >= `$INSTANCE_NAME`_RXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_RXBUFFERREAD = 0;
        }

		rxData = `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERREAD];

		`$INSTANCE_NAME`_RXBUFFERREAD++;
    
		
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
* uint16: MSB contains Status Register and LSB contains UART RX data
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

/*for 32-bit PSoC5*/
#if(`$INSTANCE_NAME`_PSOC32bit)	
/*******************************************************************************
* FUNCTION NAME: uint16 `$INSTANCE_NAME`_GetRxBufferSize(void)
*
* Summary:
* Determine the amount of space left in the RX buffer and return the count in
*   bytes
*
* Parameters:
* -None-
*
* Return:
* uint16: Integer count of the number of bytes left in the RX buffer
*
* Theory:
* Allows the user to find out how full the RX Buffer is.
*
* Side Effects:
* -None-
*******************************************************************************/

uint16 `$INSTANCE_NAME`_GetRxBufferSize(void)
{
    uint16 size;

  #if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)

    /* Disable Rx interrupt. */
    /* Protect variables that could change on interrupt. */
    #if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
        `$INSTANCE_NAME`_DisableRxInt();
    #endif

    if(`$INSTANCE_NAME`_RXBUFFERREAD == `$INSTANCE_NAME`_RXBUFFERWRITE)
    {
        if(`$INSTANCE_NAME`_RXBUFFERLOOPDETECT > 0 ) 
		{
		  size = `$INSTANCE_NAME`_RXBUFFERSIZE;
		}
		else
		{
          size = 0;
		}
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
#else 
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
        if(`$INSTANCE_NAME`_RXBUFFERLOOPDETECT > 0 ) 
		{
		  size = `$INSTANCE_NAME`_RXBUFFERSIZE;
		}
		else
		{
          size = 0;
		}
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

#endif

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
    `$INSTANCE_NAME`_RXBUFFERLOOPDETECT = 0; 
    `$INSTANCE_NAME`_RXBUFFER_OVERFLOW = 0;


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
	#if(`$INSTANCE_NAME`_CONTROL_REMOVED)
		addressMode = addressMode;
	#else
		uint8 tmpCtrl = 0;
		tmpCtrl = `$INSTANCE_NAME`_CONTROL & ~`$INSTANCE_NAME`_CTRL_RXADDR_MODE_MASK;
		tmpCtrl |= ((addressMode << `$INSTANCE_NAME`_CTRL_RXADDR_MODE0_SHIFT) & `$INSTANCE_NAME`_CTRL_RXADDR_MODE_MASK);
		`$INSTANCE_NAME`_CONTROL = tmpCtrl;
	#endif
  #else
    addressMode = addressMode;
  #endif	
	
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_HardwareAddressEnable( uint8 addressEnable )
*
* Summary:
*  Selects the hardware address compares active 
*
* Parameters:
*  Address Compare Enable
*
* Return:
* -None-
*
*******************************************************************************/
void `$INSTANCE_NAME`_HardwareAddressEnable(uint8 addressEnable)
{
    /* It is always enabled if SetRxAddressMode sets not None adress mode*/
    addressEnable = addressEnable;
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

#endif  /* `$INSTANCE_NAME`_RX_Enabled */

#if( (`$INSTANCE_NAME`_TX_Enabled) || (`$INSTANCE_NAME`_HD_Enabled) )
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
	CyIntEnable(`$INSTANCE_NAME`_TX_VECT_NUM);
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
	CyIntDisable(`$INSTANCE_NAME`_TX_VECT_NUM);
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
          (uint8)(`$INSTANCE_NAME`_TXBUFFERWRITE - `$INSTANCE_NAME`_TXBUFFERREAD) == (uint8)(`$INSTANCE_NAME`_TXBUFFERSIZE - 1))
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

        if(`$INSTANCE_NAME`_TXBUFFERWRITE >= `$INSTANCE_NAME`_TXBUFFERSIZE)
		{
            `$INSTANCE_NAME`_TXBUFFERWRITE = 0;
		 }

        `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERWRITE] = txDataByte;

        /* Add to the software buffer. */
        `$INSTANCE_NAME`_TXBUFFERWRITE++;

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
*  Wait to send byte until TX register or buffer has room
*
* Parameters:
*  TxDataByte: The 8-bit data value to send across the UART
*
* Return:
*  void
*
* Theory:
*  Allows the user to transmit any byte of data in a single transfer
*
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
* FUNCTION NAME: void `$INSTANCE_NAME`_PutString( uint8* string )
*
* Summary:
* Write a Sequence of bytes on the Transmit line. Data comes from RAM.
*
* Parameters:
* string: uint8 pointer to character string of Data to Send
*
* Return:
* -None-
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutString(uint8* string)
{
	//This is a blocking function, it will not exit until all data is sent
	while(*string != 0)	
	{
		`$INSTANCE_NAME`_WriteTxData(*string++);
	}
}


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_PutArray( uint8* string, uint8 byteCount )
*
* Summary:
* Write a Sequence of bytes on the Transmit line. Data comes from RAM.
*
* Parameters:
* string: uint8 pointer to character string of Data to Send
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
void `$INSTANCE_NAME`_PutArray(uint8* string, uint8 byteCount)
{
	while(byteCount > 0)
	{
		`$INSTANCE_NAME`_WriteTxData(*string++);
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
/*for 32-bit PSoC5*/
#if(`$INSTANCE_NAME`_PSOC32bit)	
/*******************************************************************************
* FUNCTION NAME: uint16 `$INSTANCE_NAME`_GetTxBufferSize(void)
*
* Summary:
* Determine the amount of space left in the TX buffer and return the count in
*   bytes
*
* Parameters:
* -None-
*
* Return:
* uint16: Integer count of the number of bytes left in the TX buffer
*
* Theory:
* Allows the user to find out how full the TX Buffer is.
*
* Side Effects:
* -None-
*******************************************************************************/
uint16 `$INSTANCE_NAME`_GetTxBufferSize(void)
{
    uint16 size;


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

#else
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
#endif // End `$INSTANCE_NAME`_PSOC32bit

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
	uint8 tmpStat;

#if(`$INSTANCE_NAME`_HD_Enabled)

	 uint8 tmpControl;
	 
     tmpControl = `$INSTANCE_NAME`_ReadControlRegister();
	 `$INSTANCE_NAME`_WriteControlRegister(tmpControl | `$INSTANCE_NAME`_CTRL_HD_SEND_BREAK);

	/* Send zeros*/
	`$INSTANCE_NAME`_TXDATA = 0;

	/*wait till transmit start*/
	do{tmpStat = `$INSTANCE_NAME`_TXSTATUS;
	   }while(tmpStat&`$INSTANCE_NAME`_TX_STS_COMPLETE);

	/*wait till transmit complete*/
	do{tmpStat = `$INSTANCE_NAME`_TXSTATUS;
	   }while(~tmpStat&`$INSTANCE_NAME`_TX_STS_COMPLETE);

	 `$INSTANCE_NAME`_WriteControlRegister(tmpControl);
	 
#else

	uint8 lastPeriod;

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
	do{tmpStat = `$INSTANCE_NAME`_TXSTATUS;
	   }while(tmpStat&`$INSTANCE_NAME`_TX_STS_COMPLETE);

	/*wait till transmit complete*/
	do{tmpStat = `$INSTANCE_NAME`_TXSTATUS;
	   }while(~tmpStat&`$INSTANCE_NAME`_TX_STS_COMPLETE);

	 #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
		`$INSTANCE_NAME`_TXBITCLKTX_COMPLETE = lastPeriod;
	#else
		`$INSTANCE_NAME`_TXBITCTR_PERIOD=lastPeriod;
	#endif	

#endif	

}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_SetTxAddressMode( uint8 addressMode )
*
* Summary:
* Set the transmit addressing mode
*
* Parameters:
* addressMode: 0 -> Space
*              1 -> Mark
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
   uint8 control;
    
	if(`$INSTANCE_NAME`_ParityType == `$INSTANCE_NAME`__B_UART__MARK_SPACE_REVA)
    {
        control = `$INSTANCE_NAME`_ReadControlRegister();
	    /* Mark/Space sending enable*/
        if(addressMode != 0)
        {
	        `$INSTANCE_NAME`_WriteControlRegister(control | `$INSTANCE_NAME`_CTRL_MARK);
        }
        else
        {
	        `$INSTANCE_NAME`_WriteControlRegister(control & ~`$INSTANCE_NAME`_CTRL_MARK);
        }
	 }    
     else
     {
        addressMode = addressMode;
     }
}

#endif  //`$INSTANCE_NAME`_TX_Enabled

#if(`$INSTANCE_NAME`_HD_Enabled) 

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_LoadTxConfig(void)
*
* Summary:
* Unloads the Tx configuration if required and loads the
* Rx configuration. It is the user?s responsibility to ensure that any
* transmission is complete and it is safe to unload the Tx
* configuration.
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory: Valid only for half duplex UART
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_LoadTxConfig(void)
{
uint8 control_tmp;   
	control_tmp = `$INSTANCE_NAME`_ReadControlRegister();
	`$INSTANCE_NAME`_WriteControlRegister( control_tmp | `$INSTANCE_NAME`_CTRL_HD_SEND);
   
   `$INSTANCE_NAME`_RXBITCTR_PERIOD=`$INSTANCE_NAME`_HD_TXBITCTR_INIT;

}

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_LoadRxConfig( void )
*
* Summary:
* Unloads the Rx configuration if required and loads the
* Tx configuration. It is the user?s responsibility to ensure that any
* transmission is complete and it is safe to unload the Rx
* configuration.
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory: Valid only for half duplex UART
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_LoadRxConfig(void)
{
uint8 control_tmp;   
	control_tmp = `$INSTANCE_NAME`_ReadControlRegister();
	`$INSTANCE_NAME`_WriteControlRegister( control_tmp & ~`$INSTANCE_NAME`_CTRL_HD_SEND);
   
   `$INSTANCE_NAME`_RXBITCTR_PERIOD=`$INSTANCE_NAME`_HD_RXBITCTR_INIT;
}
#endif  /* `$INSTANCE_NAME`_HD_Enabled */

