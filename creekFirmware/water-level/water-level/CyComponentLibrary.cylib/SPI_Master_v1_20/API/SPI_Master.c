/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* This file provides all API functionality of the SPI Master component
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "CyLib.h"
#include "`$INSTANCE_NAME`.h"

#if (`$INSTANCE_NAME`_TXBUFFERSIZE > 4)
`$RegSizeReplacementString` `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERSIZE];
uint8 `$INSTANCE_NAME`_TXBUFFERREAD = 0;
uint8 `$INSTANCE_NAME`_TXBUFFERWRITE = 0;
#endif
#if (`$INSTANCE_NAME`_RXBUFFERSIZE > 4)
`$RegSizeReplacementString` `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERSIZE];
uint8 `$INSTANCE_NAME`_RXBUFFERREAD = 0;
uint8 `$INSTANCE_NAME`_RXBUFFERWRITE = 0;
#endif

uint8 `$INSTANCE_NAME`_initvar = 0;
/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
* Initialize and Enable the SPI Master component
*
* Parameters:
* void
*
* Return:
* void
*
* Theory:
* Enable the clock input to enable operation
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    if(`$INSTANCE_NAME`_initvar == 0)
    {
        `$INSTANCE_NAME`_initvar = 1;
        
        /*Initialize the  to Bit counter */
        `$INSTANCE_NAME`_COUNTER_PERIOD = `$INSTANCE_NAME`_BITCTR_INIT;
        `$INSTANCE_NAME`_COUNTER_CONTROL |= `$INSTANCE_NAME`_CNTR_ENABLE;
        
/********************** ISR initialization  ********************************/  
        #if(`$INSTANCE_NAME`_InternalInterruptEnabled)
        
            CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);

            /* Set the ISR to point to the RTC_SUT_isr Interrupt. */
            CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);

            /* Set the priority. */
            CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);

            /* Enable it. */
            CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);   
        #endif
        
        /* Configure the Initial interrupt mask */
        /* If not using buffers then just enable the init interrupts as defined by the user */
        #if ( (`$INSTANCE_NAME`_TXBUFFERSIZE <= 4) && (`$INSTANCE_NAME`_TXBUFFERSIZE <= 4) )
            `$INSTANCE_NAME`_STATUS_MASK  = `$INSTANCE_NAME`_INIT_INTERRUPTS_MASK;
        #endif
        #if ( (`$INSTANCE_NAME`_RXBUFFERSIZE > 4) )
            /* If using RX Buffer then need interrupt on RX FIFO not emtpy */
            `$INSTANCE_NAME`_STATUS_MASK  = `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY;
        #endif
        `$INSTANCE_NAME`_STATUS_ACTL |= `$INSTANCE_NAME`_INT_ENABLE;
        
        /* Clear any stray data from the RX FIFO */
        `$INSTANCE_NAME`_ClearFIFO();        
        
        `$INSTANCE_NAME`_ReadStatus(); /* Clear any pending status bits */
    }
    
    #if(`$INSTANCE_NAME`_InternalClockUsed)
        `$INSTANCE_NAME`_IntClock_Enable();
    #else
        //TODO: Enable the External Clock from the information provided?
    #endif
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
* Disable the SPI Master component
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
    #if(`$INSTANCE_NAME`_InternalClockUsed)
        `$INSTANCE_NAME`_IntClock_Disable();
    #else
        //TODO: Enable the External Clock from the information provided?
    #endif
    
    #if(`$INSTANCE_NAME`_InternalInterruptEnabled)
        /* Initialize the Interrupt Component */
        CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
    #endif
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
* Enable interrupt generation
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
void `$INSTANCE_NAME`_EnableInt(void)
{
    #if(`$INSTANCE_NAME`_InternalInterruptEnabled)
        CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
    #endif
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
* Disable interrupt generation
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
void `$INSTANCE_NAME`_DisableInt(void)
{
    #if(`$INSTANCE_NAME`_InternalInterruptEnabled)
        CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
    #endif
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_SetInterruptMode
********************************************************************************
*
* Summary:
* Configure which status bits trigger an interrupt event
*
* Parameters:
* intSource: An or'd combination of the desired status bit masks (defined in
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
void `$INSTANCE_NAME`_SetInterruptMode(uint8 intSrc)
{
    `$INSTANCE_NAME`_STATUS_MASK  = intSrc;
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_ReadStatus
********************************************************************************
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
* -None-
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadStatus(void)
{
    return (`$INSTANCE_NAME`_STATUS);
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_WriteByte
********************************************************************************
*
* Summary:
* Write a byte of data to be sent across the SPI
*
* Parameters:
* txDataByte: The data value to send across the SPI
*
* Return:
* -None-
*
* Theory:
* Allows the user to transmit any byte of data in a single transfer
*
* Side Effects:
* If this function is called again before the previous byte is finished then
*        the next byte will be appended to the transfer with no time between
*        the byte transfers.
*******************************************************************************/
void `$INSTANCE_NAME`_WriteByte(`$RegSizeReplacementString` txDataByte)
{
    
#if(`$INSTANCE_NAME`_TXBUFFERSIZE > 4)

    /* Block if buffer is full, so we dont overwrite. */
    while(`$INSTANCE_NAME`_TXBUFFERWRITE == (`$INSTANCE_NAME`_TXBUFFERREAD - 1) ||
          (int8)(`$INSTANCE_NAME`_TXBUFFERWRITE - `$INSTANCE_NAME`_TXBUFFERREAD) == (int8)(`$INSTANCE_NAME`_TXBUFFERSIZE - 1))
    {
        /* Software buffer is full. */
    }

    /* Disable Interrupt. */
    /* Protect variables that could change on interrupt. */
    `$INSTANCE_NAME`_DisableInt();
    
    /* Add to the software buffer. */
    `$INSTANCE_NAME`_TXBUFFERWRITE++;
    if(`$INSTANCE_NAME`_TXBUFFERWRITE >= `$INSTANCE_NAME`_TXBUFFERSIZE)
        `$INSTANCE_NAME`_TXBUFFERWRITE = 0;

    `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERWRITE] = txDataByte;
    
    /* Enable Interrupt on TX_fifo_not Full */
    `$INSTANCE_NAME`_STATUS_MASK  |= `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL;    

    /* Enable Interrupt. */
    `$INSTANCE_NAME`_EnableInt();

#else /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

   	/* Block while FIFO is full */
	while((`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL) == 0);
    
    /* Then write the byte */
    `$CySetRegReplacementString`(`$INSTANCE_NAME`_TXDATA_PTR, txDataByte);

#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_ReadByte
********************************************************************************
*
* Summary:
* Read the next byte of data received across the SPI
*
* Parameters:
* -None-
*
* Return:
* `$RegSizeReplacementString`: The next byte of data read from the FIFO.
*
* Theory:
* Allows the user to read a byte of data received
*
* Side Effects:
* Will return invalid data if the FIFO is empty.  User should poll for FIFO 
*   empty status before calling Read function.
*******************************************************************************/
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadByte(void)
{
    `$RegSizeReplacementString` rxData;

#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)

    /* Disable Interrupt. */
    /* Protect variables that could change on interrupt. */
    `$INSTANCE_NAME`_DisableInt();

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
    {
        rxData = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_RXDATA_PTR);
    }

    /* Enable Interrupt. */
    `$INSTANCE_NAME`_EnableInt();

#else /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */

    rxData = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_RXDATA_PTR);

#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */

	return rxData;
    
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_GetRxBufferSize
********************************************************************************
*
* Summary:
* Returns the number of bytes/words of data currently held in the RX buffer
*
* Parameters:
* -None-
*
* Return:
* uint8: Integer count of the number of bytes/words in the RX buffer
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetRxBufferSize(void)
{
    uint8 size;

#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)

    /* Disable Interrupt. */
    /* Protect variables that could change on interrupt. */
    `$INSTANCE_NAME`_DisableInt();

    if(`$INSTANCE_NAME`_RXBUFFERREAD == `$INSTANCE_NAME`_RXBUFFERWRITE)
    {
        size = 0; /* No data in RX buffer */
    }
    else if(`$INSTANCE_NAME`_RXBUFFERREAD < `$INSTANCE_NAME`_RXBUFFERWRITE)
    {
        size = (`$INSTANCE_NAME`_RXBUFFERWRITE - `$INSTANCE_NAME`_RXBUFFERREAD);
    }
    else
    {
        size = (`$INSTANCE_NAME`_RXBUFFERSIZE - `$INSTANCE_NAME`_RXBUFFERREAD) + `$INSTANCE_NAME`_RXBUFFERWRITE;
    }

    /* Enable interrupt. */
    `$INSTANCE_NAME`_EnableInt();

#else /* `$INSTANCE_NAME`_RXBUFFERSIZE < 4 */

    /* We can only know if there is data in the fifo. */
    size = (`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY) ? 1 : 0;

#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE < 4 */

    return size;
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_GetTxBufferSize
********************************************************************************
*
* Summary:
* Returns the number of bytes/words of data currently held in the TX buffer
*
* Parameters:
* -None-
*
* Return:
* uint8: Integer count of the number of bytes/words in the TX buffer
*
* Theory:
*
* Side Effects:
* -None-
*******************************************************************************/
uint8  `$INSTANCE_NAME`_GetTxBufferSize(void)
{
    uint8 size;


#if(`$INSTANCE_NAME`_TXBUFFERSIZE > 4)

    /* Disable Interrupt. */
    /* Protect variables that could change on interrupt. */
    `$INSTANCE_NAME`_DisableInt();

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

    /* Enable Interrupt. */
    `$INSTANCE_NAME`_EnableInt();

#else /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */

    size = `$INSTANCE_NAME`_STATUS;

    /* Is the fifo is full. */
    if(size & `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY)
    {
        size = 4;
    }
    else if(size & `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL)
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
* FUNCTION NAME: `$INSTANCE_NAME`_ClearRxBuffer
********************************************************************************
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
    `$INSTANCE_NAME`_ClearFIFO();
#if(`$INSTANCE_NAME`_RXBUFFERSIZE > 4)
    /* Disable interrupt. */
    /* Protect variables that could change on interrupt. */
    
    `$INSTANCE_NAME`_DisableInt();

    `$INSTANCE_NAME`_RXBUFFERREAD = 0;
    `$INSTANCE_NAME`_RXBUFFERWRITE = 0;

    /* Enable Rx interrupt. */
    `$INSTANCE_NAME`_EnableInt();
#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_ClearTxBuffer
********************************************************************************
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
* Any data not yet transmitted from the RAM buffer will be lost when overwritten
*******************************************************************************/
void `$INSTANCE_NAME`_ClearTxBuffer(void)
{
#if(`$INSTANCE_NAME`_TXBUFFERSIZE > 4)

    /* Disable Interrupt. */
    /* Protect variables that could change on interrupt. */
    `$INSTANCE_NAME`_DisableInt();

    `$INSTANCE_NAME`_TXBUFFERREAD = 0;
    `$INSTANCE_NAME`_TXBUFFERWRITE = 0;

    /* Enable Interrupt. */
    `$INSTANCE_NAME`_EnableInt();

#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_TxEnable
********************************************************************************
*
* Summary:
* If the SPI master is configured to use a single bi-directional pin then this
*        will set the bi-directional pin to transmit.
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* TODO
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_TxEnable(void)
{
    //TODO: Bi-Directional Mode is not currently supported
    //        and it will require a control register that needs to be added
    // Only used in Bi-Directional mode
    //`$INSTANCE_NAME`_Control |= SPIM_CTRL_TX_SIGNAL_EN;
}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_TxDisable
********************************************************************************
*
* Summary:
* If the SPI master is configured to use a single bi-directional pin then this
*        will set the bi-directional pin to receive.
*
* Parameters:
* -None-
*
* Return:
* -None-
*
* Theory:
* TODO
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_TxDisable(void)
{
    //TODO: Bi-Directional Mode is not currently supported
    //        and it will require a control register that needs to be added
    // Only used in Bi-Directional mode
    //`$INSTANCE_NAME`_Control &= ~SPIM_CTRL_TX_SIGNAL_EN;
}

//8-bit version
/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_PutArray
********************************************************************************                       
*
* Summary:
* Write available data from ROM/RAM to the TX buffer while space is available in 
*    the TX buffer.  Keep trying until all data is passed to the TX buffer.
*
* Parameters:
* *buffer: Pointer to the location in RAM containing the data to send
* byteCount: The number of bytes to move to the transmit buffer
*
* Return:
* -None-
*
* Theory:
* TODO
*
* Side Effects:
* -None-
*******************************************************************************/
void `$INSTANCE_NAME`_PutArray(uint16 *buffer, uint8 byteCount)
{
    while(byteCount > 0)
    {
        `$INSTANCE_NAME`_WriteByte(*buffer++);
        byteCount--;
    }
}

//32-bit version
//void `$INSTANCE_NAME`_PutArray(uint8 SSaddress, uint32 *buffer, uint16 byteCount)
//{
//}

/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_ClearFIFO
********************************************************************************
*
* Summary:
* Clear the Read FIFO's of all data for a fresh start
*
* Parameters:
* None:
*
* Return:
* -None-
*
* Theory:
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearFIFO (void)
{
    while( (`$INSTANCE_NAME`_ReadStatus() & `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY) )
        `$INSTANCE_NAME`_ReadByte();
}
