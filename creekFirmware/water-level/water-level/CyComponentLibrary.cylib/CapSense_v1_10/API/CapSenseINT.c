 /*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     This file contains the Interrupt Service Routine (ISR) for the CapSense
*   component. 
*
*  Note:
*     None
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`@INSTANCE_NAME`.h"

#if defined(`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT)
/*******************************************************************************
*  Place your includes, defines and code here
*******************************************************************************/
/* `#START `$INSTANCE_NAME`_ISR_intc` */

/* `#END` */  
  
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
* Summary:
*  This ISR is executed when CAN core generate interrupt on one of evetns:
*  Arb_lost, Overload, Bit_err, Stuff_err, Ack_err, Form_err, Crc_err, 
*  Buss_off, Rxmsg_lost, Tx_msg or Rx_msg. The interrupt sources depend 
*  on Wirard inputs.
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
	/*  Place your Interrupt code here. */
	/* `#START `$INSTANCE_NAME`_ISR` */

	/* `#END` */
	`$INSTANCE_NAME`_status &= ~`$INSTANCE_NAME`_START_CAPSENSING;
}
#endif

#if defined(`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT_LEFT)
/*******************************************************************************
*  Place your includes, defines and code here
*******************************************************************************/
/* `#START `$INSTANCE_NAME`_ISRLeft_intc` */

/* `#END` */  
  
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISRLeft
********************************************************************************
* Summary:
*  This ISR is executed when CAN core generate interrupt on one of evetns:
*  Arb_lost, Overload, Bit_err, Stuff_err, Ack_err, Form_err, Crc_err, 
*  Buss_off, Rxmsg_lost, Tx_msg or Rx_msg. The interrupt sources depend 
*  on Wirard inputs.
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
CY_ISR(`$INSTANCE_NAME`_ISRLeft)
{
	/*  Place your Interrupt code here. */
	/* `#START `$INSTANCE_NAME`_ISRLeft` */

	/* `#END` */
	`$INSTANCE_NAME`_statusLeft &= ~`$INSTANCE_NAME`_START_CAPSENSING;
}
#endif

#if defined(`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT_RIGHT)
/*******************************************************************************
*  Place your includes, defines and code here
*******************************************************************************/
/* `#START `$INSTANCE_NAME`_ISRRight_intc` */

/* `#END` */  
  
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISRRight
********************************************************************************
* Summary:
*  This ISR is executed when CAN core generate interrupt on one of evetns:
*  Arb_lost, Overload, Bit_err, Stuff_err, Ack_err, Form_err, Crc_err, 
*  Buss_off, Rxmsg_lost, Tx_msg or Rx_msg. The interrupt sources depend 
*  on Wirard inputs.
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
CY_ISR(`$INSTANCE_NAME`_ISRRight)
{
	/*  Place your Interrupt code here. */
	/* `#START `$INSTANCE_NAME`_ISRRight` */

	/* `#END` */
	`$INSTANCE_NAME`_statusRight &= ~`$INSTANCE_NAME`_START_CAPSENSING;
}
#endif

///* [] END OF FILE */
//