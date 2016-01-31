/*******************************************************************************
* FILENAME: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
* `$CYD_VERSION`
*
* DESCRIPTION:
* This file provides all Interrupt Service functionality of the UART component
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


#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"



#if(`$INSTANCE_NAME`_RX_Enabled && (`$INSTANCE_NAME`_RXBUFFERSIZE > 4))
extern uint8 `$INSTANCE_NAME`_RXBUFFER[];
extern uint8 `$INSTANCE_NAME`_RXBUFFERREAD;
extern uint8 `$INSTANCE_NAME`_RXBUFFERWRITE;

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
CY_ISR(`$INSTANCE_NAME`_RXISR)
{
	uint8 readData;

   /* User code required at start of ISR */
   /* `#START `$INSTANCE_NAME`_RXISR_START` */

   /* `#END` */

   /* 
    * User Module interrupt service code
    */

    readData = `$INSTANCE_NAME`_RXSTATUS;

    if(readData & (`$INSTANCE_NAME`_RX_STS_BREAK | `$INSTANCE_NAME`_RX_STS_PAR_ERROR | `$INSTANCE_NAME`_RX_STS_STOP_ERROR | `$INSTANCE_NAME`_RX_STS_OVERRUN))
    {
        /* ERROR condition. */
    }


    while(readData & `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY)
    {
        /* Set next pointer. */
        `$INSTANCE_NAME`_RXBUFFERWRITE++;
        if(`$INSTANCE_NAME`_RXBUFFERWRITE >= `$INSTANCE_NAME`_RXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_RXBUFFERWRITE = 0;
        }

        `$INSTANCE_NAME`_RXBUFFER[`$INSTANCE_NAME`_RXBUFFERWRITE] = `$INSTANCE_NAME`_RXDATA;

        /* Check again if there is data. */        
        readData = `$INSTANCE_NAME`_RXSTATUS;
    }

   /* User code required at end of ISR (Optional) */
   /* `#START `$INSTANCE_NAME`_RXISR_END` */

   /* `#END` */
}

#endif


#if(`$INSTANCE_NAME`_TX_Enabled && (`$INSTANCE_NAME`_TXBUFFERSIZE > 4))
extern uint8 `$INSTANCE_NAME`_TXBUFFER[];
extern uint8 `$INSTANCE_NAME`_TXBUFFERREAD;
extern uint8 `$INSTANCE_NAME`_TXBUFFERWRITE;

/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_TXISR( void )
*
* Summary:
* Interrupt Service Routine for the TX portion of the UART
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
CY_ISR(`$INSTANCE_NAME`_TXISR)
{
    uint8 txStatus;

   /* User code required at start of ISR */
   /* `#START `$INSTANCE_NAME`_TXISR_START` */

   /* `#END` */

   /* 
    * User Module interrupt service code
    */

    txStatus = `$INSTANCE_NAME`_TXSTATUS;

    while((`$INSTANCE_NAME`_TXBUFFERREAD != `$INSTANCE_NAME`_TXBUFFERWRITE) && !(`$INSTANCE_NAME`_TXSTATUS & `$INSTANCE_NAME`_TX_STS_FIFO_FULL))
    {
        `$INSTANCE_NAME`_TXBUFFERREAD++;
    
        if(`$INSTANCE_NAME`_TXBUFFERREAD >= `$INSTANCE_NAME`_TXBUFFERSIZE)
        {
            `$INSTANCE_NAME`_TXBUFFERREAD = 0;
        }

        `$INSTANCE_NAME`_TXDATA = `$INSTANCE_NAME`_TXBUFFER[`$INSTANCE_NAME`_TXBUFFERREAD];
    }

   /* User code required at end of ISR (Optional) */
   /* `#START `$INSTANCE_NAME`_TXISR_END` */

   /* `#END` */
}

#endif


