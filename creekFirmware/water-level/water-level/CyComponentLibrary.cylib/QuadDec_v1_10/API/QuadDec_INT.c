/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This file contains the Interrupt Service Routine (ISR) for the Quadrature
*    Decoder component.
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



#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

/* `#START `$INSTANCE_NAME`_intc` */

/* `#END` */

#if(`$INSTANCE_NAME`_CounterSize == 32)

int32 `$INSTANCE_NAME`_Count32SoftPart = 0;
uint8 `$INSTANCE_NAME`_SwStatus;

#endif /*`$INSTANCE_NAME`_CounterSize == 32*/



CY_ISR( `$INSTANCE_NAME`_ISR )
{
    `$writeISR`
    /* `#START `$INSTANCE_NAME`_Interrupt` */
    
    /* `#END` */
    
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
        #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_isr__ES2_PATCH))                  
            `$INSTANCE_NAME`_ISR_PATCH();
        #endif
    #endif
}


/* [] END OF FILE */
