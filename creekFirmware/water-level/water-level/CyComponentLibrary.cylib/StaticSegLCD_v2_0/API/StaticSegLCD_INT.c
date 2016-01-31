/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the Interrupt Service Routine (ISR) for the Static
*  Segment LCD Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

/*`#START START_USER_DECLARATIONS_ISR`*/

/*`#END`*/


/*****************************************************************************
* Function Name:   `$INSTANCE_NAME`_ISR
******************************************************************************
*
* Summary:
*  This ISR is executed when the sub-frame completion event occurs. Both 
*  global interrupts and component interrupts must be enabled to invoke this 
*  ISR.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
 ******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    /* User code required at start of ISR */
    /*`#START START_ISR`*/

    /*`#END`*/

    /* PSoC3 ES1, ES2 RTC ISR PATCH */
    #if(CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)
        #if((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) && (`$INSTANCE_NAME`_LCD_ISR__ES2_PATCH))    
            `$INSTANCE_NAME`_ISR_PATCH();
        #endif /* (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2) */
    #endif /* (CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) */
}


/* [] END OF FILE */
