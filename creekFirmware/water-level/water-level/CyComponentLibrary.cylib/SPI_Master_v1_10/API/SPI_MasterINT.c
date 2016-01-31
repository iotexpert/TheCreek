/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* This file provides all Interrupt Service functionality of the SPI Master component
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
#include "`$INSTANCE_NAME`.h"

extern `$RegSizeReplacementString` `$INSTANCE_NAME`_RXBUFFER[];
extern uint8 `$INSTANCE_NAME`_RXBUFFERREAD;
extern uint8 `$INSTANCE_NAME`_RXBUFFERWRITE;
extern `$RegSizeReplacementString` `$INSTANCE_NAME`_TXBUFFER[];
extern uint8 `$INSTANCE_NAME`_TXBUFFERREAD;
extern uint8 `$INSTANCE_NAME`_TXBUFFERWRITE;

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_RXISR( void )
*
* Summary:
* Interrupt Service Routine for RX portion of the UART
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
CY_ISR(`$INSTANCE_NAME`_ISR)
{
   /* User code required at start of ISR */
   /* `#START `$INSTANCE_NAME`_ISR_START` */

   /* `#END` */

   /* 
    * User Module interrupt service code
    */
#if (`$INSTANCE_NAME`_RXBUFFERSIZE > 4) 
    /* See if RX data FIFO has some data and if it can be moved to the RX Buffer */
    while(`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY)
    {
        /* Set next pointer. */
        `$INSTANCE_NAME`_RXBUFFERWRITE++;
        if(`$INSTANCE_NAME`_RXBUFFERWRITE >= `$INSTANCE_NAME`_RXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_RXBUFFERWRITE = 0;
        }

        `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERWRITE] = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_RXDATA_PTR);
    }
#endif /* `$INSTANCE_NAME`_RXBUFFERSIZE > 4 */
#if (`$INSTANCE_NAME`_TXBUFFERSIZE > 4)  
    /* See if TX data buffer is not empty and there is space in TX FIFO */
    while( (`$INSTANCE_NAME`_TXBUFFERREAD != `$INSTANCE_NAME`_TXBUFFERWRITE) && 
           (`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL))
    {
        `$INSTANCE_NAME`_TXBUFFERREAD++;
    
        if(`$INSTANCE_NAME`_TXBUFFERREAD >= `$INSTANCE_NAME`_TXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_TXBUFFERREAD = 0;
        }
        
        //`$INSTANCE_NAME`_TXDATA = `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERREAD];
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_TXDATA_PTR,`$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERREAD]);
    }
    
    /* Disable Interrupt on TX_fifo_not_empty  if FIFO is empty */
    if( (`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY) )
    {
        `$INSTANCE_NAME`_STATUS_MASK  &= ~`$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL; 
    }
#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */
   /* User code required at end of ISR (Optional) */
   /* `#START `$INSTANCE_NAME`_ISR_END` */

   /* `#END` */
}

