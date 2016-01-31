/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for Power Management of the Static
*  Segment LCD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


static `$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup;

extern uint8 `$INSTANCE_NAME`_enableState;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Does nothing, provided for consistency.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    /* Nothing to save in current version */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Does nothing, provided for consistency.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    /* Nothing to restore in current version */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Prepares component for entering the sleep mode.
*
* Parameters:
*  None.

*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_enableState - holds a state of the component that will be
*  stored in `$INSTANCE_NAME`_backup.
*
*  `$INSTANCE_NAME`_backup - stores the state of the component.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_enableState;
    `$INSTANCE_NAME`_SaveConfig();
    `$INSTANCE_NAME`_Stop();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Wakes component from sleep mode. Configures DMA and enables the component for
*  normal operation.
*
* Parameters:
*  `$INSTANCE_NAME`_enableState - Global variable.
*
* Global variables:
*  `$INSTANCE_NAME`_backup - this API fetch the component state from the
*  structure to analyze and decide if the component should be enabled or not.
*
* Return:
*  Status one of standard status for PSoC3 Component:
*       CYRET_SUCCESS - Function completed successfully.
*       CYRET_LOCKED - The object was locked, already in use. Some of TDs or
*                      a channel already in use or the component was turned
*                      off by the user before entering to a sleep.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    uint8 status;

    `$INSTANCE_NAME`_RestoreConfig();

    if(`$INSTANCE_NAME`_backup.enableState == 1u)
    {
       status = `$INSTANCE_NAME`_Enable();
    }
    else
    {
        status = CYRET_LOCKED;
    }

    return(status);
}


/* [] END OF FILE */
