/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides Sleep APIs for EMIF Component.
*
* Note:
*   None    
*
********************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/
#include "`$INSTANCE_NAME`.h"

`$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup =
{
    0x0u, /* enableState; */
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  All configuration registers are retention. Nothing to save here.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Stops the EMIF operation and saves the user configuration along with 
*  the enable state of the EMIF.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used to save component state before enter sleep 
*  mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    if(`$INSTANCE_NAME`_ENABLE_REG == `$INSTANCE_NAME`_ENABLE)       
    {
        `$INSTANCE_NAME`_backup.enableState = 1u;
    }
    else /* The EMIF block is disabled */
    {
        `$INSTANCE_NAME`_backup.enableState = 0u;
    }
    
    `$INSTANCE_NAME`_Stop();
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  All configuration registers are retention. Nothing to restore here. 
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Restores the user configuration and restores the enable state.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.enableState != 0u)
    {
        /* Enable component's operation */
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component's block was disabled before */
}

/* [] END OF FILE */
