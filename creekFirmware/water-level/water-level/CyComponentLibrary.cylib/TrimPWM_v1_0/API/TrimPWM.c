/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of the API for the TrimPWM Component.
*
* Note:
*  The TrimPWM Component consists of a 8 to 16 - bit PWM with maximum period,
*  and configurable duty cycle.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts and enables the component.
*  Clears the Count register.
*  An external Enable should be provided to start counting.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    #if (`$INSTANCE_NAME`_RESOLUTION  == 8u)    /* 8bit - PWM */
        CY_SET_REG8(`$INSTANCE_NAME`_COUNTER_PTR, 0u);
    #else /* 9 - 16bit - PWM */
        CY_SET_REG16(`$INSTANCE_NAME`_COUNTER_PTR, 0u);
    #endif  /* RESOLUTION */
}

/* [] END OF FILE */
