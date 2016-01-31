/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* This file provides all Interrupt Service functionality of the SPI Slave component
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
* FUNCTION NAME: void `$INSTANCE_NAME`_ISR( void )
*
* Summary:
* Interrupt Service Routine for RX portion of the SPI Slave
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
   /* `#START `$INSTANCE_NAME`_ISR_START` */
		//Place user code required at the start of the ISR here! (Optional)
   /* `#END` */

   /* User Module interrupt service code */
#if (`$INSTANCE_NAME`_TXBUFFERSIZE > 4) 
    /* See if TX data FIFO is not full and data is in TX FIFO */
    while((`$INSTANCE_NAME`_TXBUFFERREAD != `$INSTANCE_NAME`_TXBUFFERWRITE) && !(`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_STS_TX_BUF_FULL))
    {
        `$INSTANCE_NAME`_TXBUFFERREAD++;
    
        if(`$INSTANCE_NAME`_TXBUFFERREAD >= `$INSTANCE_NAME`_TXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_TXBUFFERREAD = 0;
        }

		/* Move data from the Buffer to the FIFO */
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_TXDATA_PTR,`$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERREAD]);
		
		/* If Buffer is empty then disable TX FIFO status interrupt until there is data in the buffer */
		if(`$INSTANCE_NAME`_TXBUFFERREAD == `$INSTANCE_NAME`_TXBUFFERWRITE)
		{
			`$INSTANCE_NAME`_STATUS_MASK &= ~`$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL;
		}
    }
#endif /* `$INSTANCE_NAME`_TXBUFFERSIZE > 4 */
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
#endif /* (`$INSTANCE_NAME`_RXBUFFERSIZE > 4) */

   /* `#START `$INSTANCE_NAME`_ISR_END` */
		//Place user code required at the end of the ISR here! (Optional)
   /* `#END` */
}
