/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of Interrupt Service Routine (ISR) 
*  for CapSense Component.
*
* Note:
*
********************************************************************************
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
*  This ISR is executed when PWM window is closed. The widow depends on PWM resolution
*  paramter. This interrupt handler only used in Serial  mode of CapSense operation.
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    /*  Place your Interrupt code here. */
    /* `#START `$INSTANCE_NAME`_ISR` */

    /* `#END` */
    `$INSTANCE_NAME`_status &= ~`$INSTANCE_NAME`_START_CAPSENSING;
    
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
        #if defined(`$INSTANCE_NAME`_CSD_METHOD)
            #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_sbCSD_cISR__ES2_PATCH))      
                  `$INSTANCE_NAME`_ISR_PATCH();
            #endif
        #endif    
        #if defined(`$INSTANCE_NAME`_CSA_METHOD)
            #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_sbCSA_cISR__ES2_PATCH))      
                  `$INSTANCE_NAME`_ISR_PATCH();
            #endif
        #endif
    #endif
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
*  This ISR is executed when PWM window is closed. The widow depends on PWM resolution
*  paramter. This interrupt handler only used in Parallel  mode of CapSense operation.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISRLeft)
{
    /*  Place your Interrupt code here. */
    /* `#START `$INSTANCE_NAME`_ISRLeft` */

    /* `#END` */
    `$INSTANCE_NAME`_statusLeft &= ~`$INSTANCE_NAME`_START_CAPSENSING;
    
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
        #if defined(`$INSTANCE_NAME`_CSD_METHOD_LEFT)
            #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_lbCSD_cISR__ES2_PATCH))      
                  `$INSTANCE_NAME`_ISR_PATCH();
            #endif
        #endif    
        #if defined(`$INSTANCE_NAME`_CSA_METHOD_LEFT)
            #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_lbCSA_cISR__ES2_PATCH))      
                  `$INSTANCE_NAME`_ISR_PATCH();
            #endif
        #endif
    #endif
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
*  This ISR is executed when PWM window is closed. The widow depends on PWM resolution
*  paramter. This interrupt handler only used in Parallel  mode of CapSense operation.
*
* Parameters:  
*  void 
*
* Return: 
*  void
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISRRight)
{
    /*  Place your Interrupt code here. */
    /* `#START `$INSTANCE_NAME`_ISRRight` */

    /* `#END` */
    `$INSTANCE_NAME`_statusRight &= ~`$INSTANCE_NAME`_START_CAPSENSING;
    
    #if(CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)     
        #if defined(`$INSTANCE_NAME`_CSD_METHOD_RIGHT)
            #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_rbCSD_cISR__ES2_PATCH))      
                  `$INSTANCE_NAME`_ISR_PATCH();
            #endif
        #endif    
        #if defined(`$INSTANCE_NAME`_CSA_METHOD_RIGHT)
            #if((CYDEV_CHIP_REV_EXPECT <= CYDEV_CHIP_REV_LEOPARD_ES2) && (`$INSTANCE_NAME`_rbCSA_cISR__ES2_PATCH))      
                  `$INSTANCE_NAME`_ISR_PATCH();
            #endif
        #endif
    #endif
}
#endif


/* [] END OF FILE */
