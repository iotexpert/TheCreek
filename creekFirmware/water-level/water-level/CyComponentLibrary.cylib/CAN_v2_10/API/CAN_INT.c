/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the Interrupt Service Routine (ISR) for the CAN
*  Component. Interrupt handlers functions generate accordingly to PSoC Creator 
*  Customizer inputs.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

/* `#START CAN_INT_C_CODE_DEFINITION` */

/* `#END` */
`$ISRHandlerFunctions`

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
*
* Summary:
*  This ISR is executed when CAN core generate interrupt on one of events:
*  Arb_lost, Overload, Bit_err, Stuff_err, Ack_err, Form_err, Crc_err,
*  Buss_off, Rx_msg_lost, Tx_msg or Rx_msg. The interrupt sources depends
*  on Customizer inputs.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    /* Place your Interrupt code here. */
    /* `#START CAN_ISR` */
    
    /* `#END` */    
    `$ISRHandlers`
}


/* [] END OF FILE */
