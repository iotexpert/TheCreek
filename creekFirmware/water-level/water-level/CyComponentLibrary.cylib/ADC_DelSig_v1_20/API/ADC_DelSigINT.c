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

/*******************************************************************************
* Custom Declarations and Variables
* - add user inlcude files, prototypes and variables between the following 
*   #START and #END tags
*******************************************************************************/
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
	/***************************************************************************
 	 *  Custom Code
     *  - add user ISR code between the following #START and #END tags
     **************************************************************************/
    /* `#START MAIN_ADC_ISR`  */

  	/* `#END`  */
	
	/* PSoC3 ES1, ES2 RTC ISR PATCH  */ 
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)
        #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_IRQ__ES2_PATCH ))      
            `$INSTANCE_NAME`_ISR_PATCH();
        #endif
    #endif
			
}

/* [] END OF FILE */
