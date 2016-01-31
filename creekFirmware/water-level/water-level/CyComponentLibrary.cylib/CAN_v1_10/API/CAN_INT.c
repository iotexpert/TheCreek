 /*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     This file contains the Interrupt Service Routine (ISR) for the CAN
*     Component. 
*
*   Note:
*     None
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`@INSTANCE_NAME`.h"

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
* Summary:
*  This ISR is executed when CAN core generate interrupt on one of evetns:
*  Arb_lost, Overload, Bit_err, Stuff_err, Ack_err, Form_err, Crc_err, 
*  Buss_off, Rxmsg_lost, Tx_msg or Rx_msg. The interrupt sources depend 
*  on Wizard inputs.
*
* Parameters:  
*  void:  
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
/* Arbitration */
#if (`$INSTANCE_NAME`_ARB_LOST)
   	if (`$INSTANCE_NAME`_INT_SR.byte[0] & `$INSTANCE_NAME`_ARBITRATION_LOST_MASK)
	{
		`$INSTANCE_NAME`_ArbLostIsr();
	}
#endif

/* Overload Error */
#if (`$INSTANCE_NAME`_OVERLOAD)
   	if (`$INSTANCE_NAME`_INT_SR.byte[0] & `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK)
	{
		`$INSTANCE_NAME`_OvrLdErrorIsr();
	}
#endif
//
/* Bit Error */
#if (`$INSTANCE_NAME`_BIT_ERR)
   	if (`$INSTANCE_NAME`_INT_SR.byte[0] & `$INSTANCE_NAME`_BIT_ERROR_MASK)
	{
		`$INSTANCE_NAME`_BitErrorIsr();
	}
#endif

/* Bit Staff Error */
#if (`$INSTANCE_NAME`_STUFF_ERR)
   	if (`$INSTANCE_NAME`_INT_SR.byte[0] & `$INSTANCE_NAME`_STUFF_ERROR_MASK)
	{
		`$INSTANCE_NAME`_BitStuffErrorIsr();
	}
#endif

/* ACK Error */
#if (`$INSTANCE_NAME`_ACK_ERR)
   	if (`$INSTANCE_NAME`_INT_SR.byte[0] & `$INSTANCE_NAME`_ACK_ERROR_MASK)
	{
		`$INSTANCE_NAME`_AckErrorIsr();
	}
#endif

/* Form(msg) Error */
#if (`$INSTANCE_NAME`_FORM_ERR)
   	if (`$INSTANCE_NAME`_INT_SR.byte[0] & `$INSTANCE_NAME`_FORM_ERROR_MASK)
	{
		`$INSTANCE_NAME`_MsgErrorIsr();
	}
#endif

/* CRC Error */
#if (`$INSTANCE_NAME`_CRC_ERR)
   	if (`$INSTANCE_NAME`_INT_SR.byte[1] & `$INSTANCE_NAME`_CRC_ERROR_MASK)
	{
		`$INSTANCE_NAME`_CrcErrorIsr();
	}
#endif

/* Bus Off state */
#if (`$INSTANCE_NAME`_BUS_OFF)
   	if (`$INSTANCE_NAME`_INT_SR.byte[1] & `$INSTANCE_NAME`_BUS_OFF_MASK)
	{
		`$INSTANCE_NAME`_BusOffIsr();
	}
#endif

/* Message Lost */
#if (`$INSTANCE_NAME`_RX_MSG_LOST)
   	if (`$INSTANCE_NAME`_INT_SR.byte[1] & `$INSTANCE_NAME`_RX_MSG_LOST_MASK)
	{
		`$INSTANCE_NAME`_MsgLostIsr();
	}
#endif

/* TX Message Send */
#if (`$INSTANCE_NAME`_TX_MSG)
   	if (`$INSTANCE_NAME`_INT_SR.byte[1] & `$INSTANCE_NAME`_TX_MSG_MASK)
	{
		`$INSTANCE_NAME`_MsgTXIsr();
	}
#endif

/* RX Message Available */
#if (`$INSTANCE_NAME`_RX_MSG)
   	if (`$INSTANCE_NAME`_INT_SR.byte[1] & `$INSTANCE_NAME`_RX_MSG_MASK)
	{
		`$INSTANCE_NAME`_MsgRXIsr();
	}
#endif
}

///* [] END OF FILE */
//

