/*******************************************************************************
* File Name: `$INSTANCE_NAME`_TX_RX_func.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*	There are fucntions process "Full" Receive and Transmit mailboxes:
*		- `$INSTANCE_NAME`_SendMsg0-7();
*       - `$INSTANCE_NAME`_ReceiveMsg0-15(); 
*   Transmition of message, and receive routine for "Basic" mailboxes:
*		- `$INSTANCE_NAME`_SendMsg();
*    	- `$INSTANCE_NAME`_TxCancel();
*		- `$INSTANCE_NAME`_ReceiveMsg(); 
*
*   Note:
*
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`.h"
/* `#START TX_RX_FUNCTION` */

/* `#END` */
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg(CANTXMsg *message)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function Send Message from one of Basic mailboxes. Function loop through 
 *  the transmit message buffer designed as Basic CAN maiboxes for first free 
 *  available and send from it. The number of retries is limited.
 *
 * Parameters: 
 *  (CANTXMsg *) message: Pointer to structure that contian all required data to send message.
 *
 * Return:
 *  (uint8) Indication if message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *   None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg(CANTXMsg *message)
{
	uint8 i, shift;
	uint8 retry = 0;
	uint8 result = FAIL;
	uint32 reg_temp;
	uint32 id;

	while (retry < `$INSTANCE_NAME`_RETRY_NUMBER)
	{
		shift = 1;
		for (i = 0; i < `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES; i++)
		{
			/* Find Basic TX mailboxes */
			if (!(`$INSTANCE_NAME`_TX_MAILBOX_TYPE & shift))
			{
				/* Find free mailbox */
				if (!(`$INSTANCE_NAME`_BUF_SR.byte[2] & shift))
				{
					reg_temp = 0;
					
					/* Set message parameters */
					if (message->rtr)
					{
						reg_temp |= `$INSTANCE_NAME`_TX_RTR_MASK;
					}	
					if (message->ide)
					{
						reg_temp |= `$INSTANCE_NAME`_TX_IDE_MASK;
					}	
					if (message->dlc <= 8)
					{	
						reg_temp |= ((uint32)message->dlc) << 16;
					}
					else
					{
						reg_temp |= `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE;
					}
					if (message->irq)
					{
						reg_temp |= `$INSTANCE_NAME`_TX_INT_ENABLE_MASK; // Transmit Interrupt Enable
					}
					
					if(message->ide)
					{
						id = (uint32)message->id << 3;
					}
					else
					{
						id = (uint32)message->id << 21;
					}
					
					CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[i].txid, id);
					
					`$INSTANCE_NAME`_TX_DATA_BYTE1(i) = message->msg->byte[0];
					`$INSTANCE_NAME`_TX_DATA_BYTE2(i) = message->msg->byte[1];
					`$INSTANCE_NAME`_TX_DATA_BYTE3(i) = message->msg->byte[2];
					`$INSTANCE_NAME`_TX_DATA_BYTE4(i) = message->msg->byte[3];
					`$INSTANCE_NAME`_TX_DATA_BYTE5(i) = message->msg->byte[4];
					`$INSTANCE_NAME`_TX_DATA_BYTE6(i) = message->msg->byte[5];
					`$INSTANCE_NAME`_TX_DATA_BYTE7(i) = message->msg->byte[6];
					`$INSTANCE_NAME`_TX_DATA_BYTE8(i) = message->msg->byte[7];					
					
					/* WPN[23] and WPN[3] set to 1 for write to CAN Control reg */
					CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[i].txcmd, (reg_temp | `$INSTANCE_NAME`_TX_WPN_SET));					
					CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[i].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);

					result = CYRET_SUCCESS;
				}
			}
			shift <<= 1;
			if(result == CYRET_SUCCESS)
			{
				break;
			}
		}
		if(result == CYRET_SUCCESS)
		{
			break;
		}
		else
		{
			retry++;
		}
	}
	
	return result;
}

 /*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_TxCancel(uint8 bufferld)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function cancel transmition of a message that has been queued for 
 *  transmitted. Values between 0 and 15 are valid.
 *
 * Parameters: 
 *  (uint8) bufferld: Mailbox number. 
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *   None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_TxCancel(uint8 bufferld)
{
  	if (bufferld < `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES)
	{	
		`$INSTANCE_NAME`_TX_ABORT_MESSAGE(bufferld);
  	}
}

#if(`$INSTANCE_NAME`_TX0_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name0`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 0. Function check
 *  if mailbox 0 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name0`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[0].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name0`_TRASMITTED` */
		
		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[0].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

#if(`$INSTANCE_NAME`_TX1_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name1`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 1. Function check
 *  if mailbox 1 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name1`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[1].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name1`_TRASMITTED` */


		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[1].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

#if(`$INSTANCE_NAME`_TX2_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name2`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 2. Function check
 *  if mailbox 2 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name2`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[2].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name2`_TRASMITTED` */


		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[2].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

#if(`$INSTANCE_NAME`_TX3_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name3`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 3. Function check
 *  if mailbox 3 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name3`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[3].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name3`_TRASMITTED` */


		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[3].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

#if(`$INSTANCE_NAME`_TX4_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name4`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 4. Function check
 *  if mailbox 4 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name4`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[4].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name4`_TRASMITTED` */


		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[4].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

#if(`$INSTANCE_NAME`_TX5_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name5`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 5. Function check
 *  if mailbox 5 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name5`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[5].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name5`_TRASMITTED` */


		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[5].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

#if(`$INSTANCE_NAME`_TX6_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name6`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 6. Function check
 *  if mailbox 6 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name6`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[6].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name6`_TRASMITTED` */


		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[6].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

#if(`$INSTANCE_NAME`_TX7_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SendMsg`$TX_name7`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message 7. Function check
 *  if mailbox 7 doesn't already have an un-transmited messages waiting for 
 *  arbitration. If not initiate transmition of the message.
 *  Only generated for Transmit mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication if Message has been sent.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name7`(void)
{

	uint8 result = CYRET_SUCCESS;
	if (`$INSTANCE_NAME`_TX[7].txcmd.byte[0] & `$INSTANCE_NAME`_TX_REQUEST_PENDING)
	{
		result = FAIL;
	}
	else
	{
		/* `#START MESSAGE_`$TX_name7`_TRASMITTED` */


		/* `#END` */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[7].txcmd, `$INSTANCE_NAME`_SEND_MESSAGE);
	}
	
	return result;
}
#endif

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_ReceiveMsg(uint8 rxreg)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message Interrupt for Basic mailboxes. 
 *  Clears Receive particular Message interrupt  flag. Only generated if 
 *  one of Receive mailboxes designed as Basic. 
 *
 * Parameters: 
 *  (uint8) rxreg: Mailbox number that trig Receive Message Interrupt.
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg(uint8 rxreg)
{
	if (`$INSTANCE_NAME`_RX[rxreg].rxcmd.byte[0] & `$INSTANCE_NAME`_RX_ACK_MSG)
	{
		/* `#START MESSAGE_BASIC_RECEIVED` */
		
		/* `#END` */
		`$INSTANCE_NAME`_RX[rxreg].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_ACK_MSG;
	}
}

#if(`$INSTANCE_NAME`_RX0_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name0`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 0 Interrupt. Clears Receive 
 *  Message 0 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name0`(void)
{
	/* `#START MESSAGE_`$RX_name0`_RECEIVED` */


	/* `#END` */

`$RX_ack0`
}
#endif

#if(`$INSTANCE_NAME`_RX1_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name1`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 1 Interrupt. Clears Receive 
 *  Message 1 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name1`(void)
{
	/* `#START MESSAGE_`$RX_name1`_RECEIVED` */


	/* `#END` */

`$RX_ack1`
}
#endif

#if(`$INSTANCE_NAME`_RX2_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name2`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 2 Interrupt. Clears Receive 
 *  Message 2 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name2`(void)
{
	/* `#START MESSAGE_`$RX_name2`_RECEIVED` */


	/* `#END` */

`$RX_ack2`
}
#endif

#if(`$INSTANCE_NAME`_RX3_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name3`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 3 Interrupt. Clears Receive 
 *  Message 3 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name3`(void)
{
	/* `#START MESSAGE_`$RX_name3`_RECEIVED` */


	/* `#END` */

`$RX_ack3`
}
#endif

#if(`$INSTANCE_NAME`_RX4_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name4`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 4 Interrupt. Clears Receive 
 *  Message 4 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name4`(void)
{
	/* `#START MESSAGE_`$RX_name4`_RECEIVED` */


	/* `#END` */

`$RX_ack4`
}
#endif

#if(`$INSTANCE_NAME`_RX5_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name5`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 5 Interrupt. Clears Receive 
 *  Message 5 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name5`(void)
{
	/* `#START MESSAGE_`$RX_name5`_RECEIVED` */


	/* `#END` */

`$RX_ack5`
}
#endif

#if(`$INSTANCE_NAME`_RX6_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name6`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 6 Interrupt. Clears Receive 
 *  Message 6 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name6`(void)
{
	/* `#START MESSAGE_`$RX_name6`_RECEIVED` */


	/* `#END` */

`$RX_ack6`
}
#endif

#if(`$INSTANCE_NAME`_RX7_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name7`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 7 Interrupt. Clears Receive 
 *  Message 7 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name7`(void)
{
	/* `#START MESSAGE_`$RX_name7`_RECEIVED` */


	/* `#END` */

`$RX_ack7`
}
#endif

#if(`$INSTANCE_NAME`_RX8_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name8`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 8 Interrupt. Clears Receive 
 *  Message 8 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name8`(void)
{
	/* `#START MESSAGE_`$RX_name8`_RECEIVED` */


	/* `#END` */

`$RX_ack8`
}
#endif

#if(`$INSTANCE_NAME`_RX9_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name9`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 9 Interrupt. Clears Receive 
 *  Message 9 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name9`(void)
{
	/* `#START MESSAGE_`$RX_name9`_RECEIVED` */


	/* `#END` */

`$RX_ack9`
}
#endif

#if(`$INSTANCE_NAME`_RX10_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name10`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 10 Interrupt. Clears Receive 
 *  Message 10 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name10`(void)
{
	/* `#START MESSAGE_`$RX_name10`_RECEIVED` */


	/* `#END` */

`$RX_ack10`
}
#endif

#if(`$INSTANCE_NAME`_RX11_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name11`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 11 Interrupt. Clears Receive 
 *  Message 11 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name11`(void)
{
	/* `#START MESSAGE_`$RX_name11`_RECEIVED` */


	/* `#END` */

`$RX_ack11`
}
#endif

#if(`$INSTANCE_NAME`_RX12_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name12`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 12 Interrupt. Clears Receive 
 *  Message 12 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name12`(void)
{
	/* `#START MESSAGE_`$RX_name12`_RECEIVED` */


	/* `#END` */

`$RX_ack12`
}
#endif

#if(`$INSTANCE_NAME`_RX13_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name13`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 13 Interrupt. Clears Receive 
 *  Message 13 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name13`(void)
{
	/* `#START MESSAGE_`$RX_name13`_RECEIVED` */


	/* `#END` */

`$RX_ack13`
}
#endif

#if(`$INSTANCE_NAME`_RX14_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name14`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 14 Interrupt. Clears Receive 
 *  Message 14 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name14`(void)
{
	/* `#START MESSAGE_`$RX_name14`_RECEIVED` */


	/* `#END` */

`$RX_ack14`
}
#endif

#if(`$INSTANCE_NAME`_RX15_FUNC_ENABLE)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  void `$INSTANCE_NAME`_ReceiveMsg`$RX_name15`(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message 15 Interrupt. Clears Receive 
 *  Message 15 interrupt flag. Only generated Receive mailbox designed as Full. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name15`(void)
{
	/* `#START MESSAGE_`$RX_name15`_RECEIVED` */


	/* `#END` */

`$RX_ack15`
}
#endif

///* [] END OF FILE */
//
