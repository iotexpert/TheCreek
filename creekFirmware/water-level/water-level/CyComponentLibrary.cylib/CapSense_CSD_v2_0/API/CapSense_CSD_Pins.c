/*******************************************************************************
* File Name: `$INSTANCE_NAME`_Pins.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains API to enable firmware control of a Pins component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`_Pins.h"


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAllSensorsDriveMode
********************************************************************************
* Summary:
*  Change the drive mode on the pins of the CapSense port.
* 
* Parameters:  
*  mode:  Change the pins to this drive mode.
*
* Return: 
*  void
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
* Summary:
*  Change the drive mode on the pins of the CapSense port.
* 
* Parameters:  
*  mode:  Change the pins to this drive mode.
*
* Return: 
*  void
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
    * Summary:
    *  Change the drive mode on the pins of the CapSense port.
    * 
    * Parameters:  
    *  mode:  Change the pins to this drive mode.
    *
    * Return: 
    *  void
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetAllRbsDriveMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetAllRbsDriveMode")`
    {
        /* Set pins drive mode */
`$writerHrbsDM`  
    }
#endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB) */


/* [] END OF FILE */
