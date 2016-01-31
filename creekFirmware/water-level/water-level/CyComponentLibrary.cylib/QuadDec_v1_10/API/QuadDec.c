/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This file provides the source code to the API for the Quadrature Decoder
*    component.
*
*  Note:
*    None
*   
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#ifndef CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_C
#define CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_C

#include "`$INSTANCE_NAME`.h"
#include "CyLib.h"

#if(`$INSTANCE_NAME`_CounterSize == 32)

extern int32 `$INSTANCE_NAME`_Count32SoftPart;
extern uint8 `$INSTANCE_NAME`_SwStatus;

#endif /*`$INSTANCE_NAME`_CounterResolution == 32*/


uint8 `$INSTANCE_NAME`_initVar = 1;

`$writeCFile`

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetEvents
********************************************************************************
* 
* Summary:
*   Reports the current status of events.
*
* Parameters:  
*  void  
*
* Return: 
*  The events, as bits in an unsigned 8-bit value:
*        Bit        Description
*
*        0        Counter overflow.
*        1        Counter underflow.
*        2        Counter reset due to index, if index input is used.
*        3        Invalid A, B inputs state transition.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetEvents(void)
{   
    return (`$INSTANCE_NAME`_STATUS & `$INSTANCE_NAME`_INIT_INT_MASK);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetInterruptMask
********************************************************************************
* 
* Summary:
*   Reports the current interrupt mask settings.
*
* Parameters:  
*  void
*
* Return: 
*  Enable / disable bits in an 8-bit value, where 1 enables the interrupt.
*  For the 32-bit counter, the overflow and underflow enable bits are always set.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetInterruptMask(void)
{
    return (`$INSTANCE_NAME`_STATUS_MASK);
}


/* [] END OF FILE */

#endif /* CY_QUADRATURE_DECODER_`$INSTANCE_NAME`_C */
