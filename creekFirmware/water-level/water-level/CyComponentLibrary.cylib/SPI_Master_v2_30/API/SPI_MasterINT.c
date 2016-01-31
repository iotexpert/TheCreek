/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides all Interrupt Service Routine (ISR) for the SPI Master
*  component.
*
* Note:
*  None.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if (`$INSTANCE_NAME`_RXBUFFERSIZE > 4u)

    extern volatile `$RegSizeReplacementString` `$INSTANCE_NAME`_RXBUFFER[];
    extern volatile uint8 `$INSTANCE_NAME`_rxBufferRead;
    extern volatile uint8 `$INSTANCE_NAME`_rxBufferWrite;
    extern volatile uint8 `$INSTANCE_NAME`_rxBufferFull;
    
#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4u */

#if (`$INSTANCE_NAME`_TXBUFFERSIZE > 4u)

    extern volatile `$RegSizeReplacementString` `$INSTANCE_NAME`_TXBUFFER[];
    extern volatile uint8 `$INSTANCE_NAME`_txBufferRead;
    extern volatile uint8 `$INSTANCE_NAME`_txBufferWrite;
    extern volatile uint8 `$INSTANCE_NAME`_txBufferFull;

#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4u */

volatile uint8 `$INSTANCE_NAME`_swStatusTx = 0u;
volatile uint8 `$INSTANCE_NAME`_swStatusRx = 0u;

/* User code required at start of ISR */
/* `#START `$INSTANCE_NAME`_ISR_START_DEF` */

/* `#END` */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_TX_ISR
********************************************************************************
*
* Summary:
*  Interrupt Service Routine for TX portion of the SPI Master.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_txBufferWrite - used for the account of the bytes which
*  have been written down in the TX software buffer.
*  `$INSTANCE_NAME`_txBufferRead - used for the account of the bytes which
*  have been read from the TX software buffer, modified when exist data to 
*  sending and FIFO Not Full.
*  `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERSIZE] - used to store
*  data to sending.
*  All described above Global variables are used when Software Buffer is used.
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_TX_ISR)
{     
    /* User code required at start of ISR */
    /* `#START `$INSTANCE_NAME`_TX_ISR_START` */

    /* `#END` */
    
    #if((`$INSTANCE_NAME`_InternalTxInterruptEnabled) && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4u))
                         
        /* See if TX data buffer is not empty and there is space in TX FIFO */
        while(`$INSTANCE_NAME`_txBufferRead != `$INSTANCE_NAME`_txBufferWrite)
        {
            `$INSTANCE_NAME`_swStatusTx = `$INSTANCE_NAME`_GET_STATUS_TX(`$INSTANCE_NAME`_swStatusTx);
            
            if ((`$INSTANCE_NAME`_swStatusTx & `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL) != 0u)
            {            
                if(`$INSTANCE_NAME`_txBufferFull == 0u)
                {
                   `$INSTANCE_NAME`_txBufferRead++;

                    if(`$INSTANCE_NAME`_txBufferRead >= `$INSTANCE_NAME`_TXBUFFERSIZE)
                    {
                        `$INSTANCE_NAME`_txBufferRead = 0u;
                    }
                }
                else
                {
                    `$INSTANCE_NAME`_txBufferFull = 0u;
                }
            
                /* Move data from the Buffer to the FIFO */
                `$CySetRegReplacementString`(`$INSTANCE_NAME`_TXDATA_PTR,
                    `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_txBufferRead]);
            }
            else
            {
                break;
            }            
        }
            
        /* Disable Interrupt on TX_fifo_not_empty if BUFFER is empty */
        if(`$INSTANCE_NAME`_txBufferRead == `$INSTANCE_NAME`_txBufferWrite)
        {
            `$INSTANCE_NAME`_TX_STATUS_MASK_REG  &= ~`$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL; 
        }                       
        
	#endif /* `$INSTANCE_NAME`_InternalTxInterruptEnabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4u) */
    
    /* User code required at end of ISR (Optional) */
    /* `#START `$INSTANCE_NAME`_TX_ISR_END` */

    /* `#END` */
   
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RX_ISR
********************************************************************************
*
* Summary:
*  Interrupt Service Routine for RX portion of the SPI Master.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_rxBufferWrite - used for the account of the bytes which
*  have been written down in the RX software buffer modified when FIFO contains
*  new data.
*  `$INSTANCE_NAME`_rxBufferRead - used for the account of the bytes which
*  have been read from the RX software buffer, modified when overflow occurred.
*  `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERSIZE] - used to store
*  received data, modified when FIFO contains new data.
*  All described above Global variables are used when Software Buffer is used.
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_RX_ISR)
{     
    #if((`$INSTANCE_NAME`_InternalRxInterruptEnabled) && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4u))
        `$RegSizeReplacementString` rxData = 0u; 
    #endif /* `$INSTANCE_NAME`_InternalRxInterruptEnabled */  
    
    /* User code required at start of ISR */
    /* `#START `$INSTANCE_NAME`_RX_ISR_START` */

    /* `#END` */
    
    #if((`$INSTANCE_NAME`_InternalRxInterruptEnabled) && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4u))
         
        `$INSTANCE_NAME`_swStatusRx = `$INSTANCE_NAME`_GET_STATUS_RX(`$INSTANCE_NAME`_swStatusRx);          
        
        /* See if RX data FIFO has some data and if it can be moved to the RX Buffer */
        while((`$INSTANCE_NAME`_swStatusRx & `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY) == 
                                                                                `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY)
        {
            rxData = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_RXDATA_PTR);
            
            /* Set next pointer. */
            `$INSTANCE_NAME`_rxBufferWrite++;
            if(`$INSTANCE_NAME`_rxBufferWrite >= `$INSTANCE_NAME`_RXBUFFERSIZE)
            {
                `$INSTANCE_NAME`_rxBufferWrite = 0u;
            }
            
            if(`$INSTANCE_NAME`_rxBufferWrite == `$INSTANCE_NAME`_rxBufferRead)
            {
                `$INSTANCE_NAME`_rxBufferRead++;
                if(`$INSTANCE_NAME`_rxBufferRead >= `$INSTANCE_NAME`_RXBUFFERSIZE)
                {
                    `$INSTANCE_NAME`_rxBufferRead = 0u;
                }
                `$INSTANCE_NAME`_rxBufferFull = 1u;
            }
            
            /* Move data from the FIFO to the Buffer */
            `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_rxBufferWrite] = rxData;
                
            `$INSTANCE_NAME`_swStatusRx = `$INSTANCE_NAME`_GET_STATUS_RX(`$INSTANCE_NAME`_swStatusRx);
        }                    
        
	#endif /* `$INSTANCE_NAME`_InternalRxInterruptEnabled  && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4u) */        
    
    /* User code required at end of ISR (Optional) */
    /* `#START `$INSTANCE_NAME`_RX_ISR_END` */

    /* `#END` */
    
}

/* [] END OF FILE */
