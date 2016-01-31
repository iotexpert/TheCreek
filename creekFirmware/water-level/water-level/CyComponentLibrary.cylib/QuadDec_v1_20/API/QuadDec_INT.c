/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the Interrupt Service Routine (ISR) for the Quadrature
*  Decoder component.
*
* Note:
*  None
*   
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

int32 `$INSTANCE_NAME`_Count32SoftPart = 0;
uint8 `$INSTANCE_NAME`_SwStatus;

CY_ISR( `$INSTANCE_NAME`_ISR )
{
    /* User code required at start of ISR */
    /* `#START `$INSTANCE_NAME`_ISR_START` */

    /* `#END` */

    `$INSTANCE_NAME`_SwStatus = `$INSTANCE_NAME`_STATUS;
    
    if(`$INSTANCE_NAME`_SwStatus & `$INSTANCE_NAME`_COUNTER_OVERFLOW)
    {
        `$INSTANCE_NAME`_Count32SoftPart += 0x8000;
    }
    else if(`$INSTANCE_NAME`_SwStatus & `$INSTANCE_NAME`_COUNTER_UNDERFLOW)
    {
        `$INSTANCE_NAME`_Count32SoftPart -= 0x8001;
    }
    if(`$INSTANCE_NAME`_SwStatus & `$INSTANCE_NAME`_COUNTER_RESET)
    {
        `$INSTANCE_NAME`_Count32SoftPart = 0;
    }
    /* User code required at end of ISR */
    /* `#START `$INSTANCE_NAME`_ISR_END` */

    /* `#END` */
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
        #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))                  
            `$INSTANCE_NAME`_ISR_PATCH();
        #endif /* (CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH) */
    #endif /* CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD */    
}


/* [] END OF FILE */
