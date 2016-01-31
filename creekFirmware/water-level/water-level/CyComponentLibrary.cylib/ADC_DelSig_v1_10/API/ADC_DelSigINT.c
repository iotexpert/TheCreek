/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the code that operates during the ADC_DelSig interrupt 
*    service routine.  
*
*   Note:
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"  


/**********************************
*      System variables
**********************************/
/* `#START ADC_SYS_VAR`  */


/* `#END`  */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
********************************************************************************
* Summary:
*  Handle Interrupt Service Routine.  
*
* Parameters:  
*  void
*
* Return: 
*  void 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
CY_ISR( `$INSTANCE_NAME`_ISR )
{
	/**********************************************/
 	/* Place user ADC ISR code here.              */
	/* This can be a good place to place code     */
	/* that is used to switch the input to the    */
	/* ADC.  It may be good practice to first     */
	/* Stop the ADC before switching the input    */
	/* then restart the ADC.                      */
 	/**********************************************/
  	/* `#START MAIN_ADC_ISR`  */




  	/* `#END`  */
			
			
			
}

/* [] END OF FILE */
