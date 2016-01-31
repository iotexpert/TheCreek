/*******************************************************************************
 *  FILENAME: `$INSTANCE_NAME`INT.c
 * Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
 *
 *  DESCRIPTION:
 *     This file contains the Interrupt Service Routine (ISR) for the LCD
 *     User Module.
 *
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/


 /*DOM-IGNORE-END*/

#include "cytypes.h"
#include "cyfitter.h"
#include "`$INSTANCE_NAME`.h"

/*DOM-IGNORE-BEGIN
 ******************************************************************************
 * FUNCTION NAME:   void `@INSTANCE_NAME`_ISR( void )
 ******************************************************************************
 *DOM-IGNORE-END
 *
 * Summary:
 * This ISR is executed when a terminal count and/or compare occurs.  Both
 * global interrupts and user module interrupts must be enable to invoke
 * this ISR.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *   None
 *
 ******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
   /* User code required at start of ISR */
   /*`#START START_ISR`*/

   /*`#END`*/

   /*
    * User Module interrupt service code
    */

   /* User code required at end of ISR (Optional) */
   /*`#START END_ISR` */

  /* PSoC3 ES1, ES2 RTC ISR PATCH  */

    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)
       #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_TD_DoneInt__ES2_PATCH))
            `$INSTANCE_NAME`_ISR_PATCH();
      #endif /* CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD */
    #endif /* (CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_TD_DoneInt__ES2_PATCH) */

   /*`#END`*/
}


/* [] END OF FILE */
