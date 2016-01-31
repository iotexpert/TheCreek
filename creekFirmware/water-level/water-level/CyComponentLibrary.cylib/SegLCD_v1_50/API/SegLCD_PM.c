/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for the Segment LCD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


static `$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup;

/*`#START USER_PM_DECLARATIONS` */

/*`#END`*/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves component configuration.
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
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{
    /* Nothing to save for the component in current implementation. */
    
    /* Optional user code */
    /*`#START` SAVE_CONFIG_USER_CODE */

    /*`#END`*/
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores component configuration.
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
    /* Nothing to restore for the component in current implementation. */
    
    /* Optional user code */
    /*`#START RESTORE_CONFIG_USER_CODE` */

    /*`#END`*/
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Prepare the component to enter a Sleep mode.
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
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    if((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CLK_ENABLE) == `$INSTANCE_NAME`_CLK_ENABLE)
    {
        `$INSTANCE_NAME`_backup.enableState = 1u;
    }
    else
    {
        `$INSTANCE_NAME`_backup.enableState = 0u;
    }
    
    `$INSTANCE_NAME`_Stop();
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Wakes up components from a Sleep mode.
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
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void)
{
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.enableState == 1)
    {
        `$INSTANCE_NAME`_Enable();
    }
}

/* [] END OF FILE */
