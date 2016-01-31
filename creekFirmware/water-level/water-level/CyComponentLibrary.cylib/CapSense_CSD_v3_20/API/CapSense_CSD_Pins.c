/*******************************************************************************
* File Name: `$INSTANCE_NAME`_Pins.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains API to enable firmware control of a Pins component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_Pins.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAllSensorsDriveMode
********************************************************************************
*
* Summary:
*  Sets the drive mode for the all pins used by capacitive sensors within 
*  CapSense component.
* 
* Parameters:  
*  mode: Desired drive mode.
*
* Return: 
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetAllSensorsDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetAllSensorsDriveMode")`
{
    /* Set pins drive mode */
`$writerHsensorsDM`
}



/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAllCmodsDriveMode
********************************************************************************
*
* Summary:
*  Sets the drive mode for the all pins used by Cmod capacitors within CapSense 
*  component.
* 
* Parameters:  
*  mode: Desired drive mode.
*
* Return: 
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetAllCmodsDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetAllCmodsDriveMode")`
{
   /* Set pins drive mode */
`$writerHcmodsDM`
}


#if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetAllRbsDriveMode
    ********************************************************************************
    *
    * Summary:
    *  Sets the drive mode for the all pins used by bleed resistors (Rb) within 
    *  CapSense component. Only available when Current Source is external resistor.
    * 
    * Parameters:  
    *  mode: Desired drive mode.
    *
    * Return: 
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetAllRbsDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetAllRbsDriveMode")`
    {
        /* Set pins drive mode */
    `$writerHrbsDM`  
    }
#endif  /* (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB) */


/* [] END OF FILE */
