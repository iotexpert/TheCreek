/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the Interrupt Service Routine (ISR) for the Quadrature
*  Decoder component.
*
* Note:
*  None.
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

volatile int32 `$INSTANCE_NAME`_count32SoftPart = 0u;
static uint8 `$INSTANCE_NAME`_swStatus;


/*******************************************************************************
* FUNCTION NAME: void `$INSTANCE_NAME`_ISR
********************************************************************************
*
* Summary:
*  Interrupt Service Routine for Quadrature Decoder Component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_count32SoftPart - modified to update hi 16 bit for current
*  value of the 32-bit counter, when Counter size equal 32-bit.
*  `$INSTANCE_NAME`_swStatus - modified with the updated values of STATUS 
*  register.
*
*******************************************************************************/
CY_ISR( `$INSTANCE_NAME`_ISR )
{
   `$INSTANCE_NAME`_swStatus = `$INSTANCE_NAME`_STATUS_REG;
   
    /* User code required at start of ISR */
    /* `#START `$INSTANCE_NAME`_ISR_START` */

    /* `#END` */
    
    if(`$INSTANCE_NAME`_swStatus & `$INSTANCE_NAME`_COUNTER_OVERFLOW)
    {
        `$INSTANCE_NAME`_count32SoftPart += 0x7FFFu;
    }
    else if(`$INSTANCE_NAME`_swStatus & `$INSTANCE_NAME`_COUNTER_UNDERFLOW)
    {
        `$INSTANCE_NAME`_count32SoftPart -= 0x8000u;
    }
    else
    {
        /* Nothing to do here */
    }
    
    if(`$INSTANCE_NAME`_swStatus & `$INSTANCE_NAME`_COUNTER_RESET)
    {
        `$INSTANCE_NAME`_count32SoftPart = 0u;
    }
    
    /* User code required at end of ISR */
    /* `#START `$INSTANCE_NAME`_ISR_END` */

    /* `#END` */
    
    /* PSoC3 ES1, ES2 `$INSTANCE_NAME` ISR PATCH  */     
    #if(CY_PSOC3_ES2 && (`$INSTANCE_NAME`_isr__ES2_PATCH))
        `$INSTANCE_NAME`_ISR_PATCH();
    #endif /* End CY_PSOC3_ES2 */   
}


/* [] END OF FILE */
