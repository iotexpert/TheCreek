/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the power management source code to the API for the 
*  TIA component.
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

static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup;


/*******************************************************************************  
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the current user configuration registers.
*
* Parameters:
*  `$INSTANCE_NAME`_backup - global structure, where configuration registers are
*  stored.
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    /* Nothing to save as registers are System reset on retention flops */
}


/*******************************************************************************  
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores the current user configuration.
*
* Parameters:
*  None
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    /* Nothing to restore */
}


/*******************************************************************************   
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Disables block's operation and saves its configuration. Should be called 
*  just prior to entering sleep.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_backup: The structure field 'enableState' is modified 
*  depending on the enable state of the block before entering to sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    /* Save TIA enable state */
    if(`$INSTANCE_NAME`_ACT_PWR_EN == (`$INSTANCE_NAME`_PM_ACT_CFG_REG & `$INSTANCE_NAME`_ACT_PWR_EN))
    {
        /* Component is enabled */
        `$INSTANCE_NAME`_backup.enableState = 1u;
    }
    else
    {
        /* Component is disabled */
        `$INSTANCE_NAME`_backup.enableState = 0u;
    }

    /* Stop the configuration */
    `$INSTANCE_NAME`_Stop();

    /* Save the configuration */
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************  
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Enables block's operation and restores its configuration. Should be called
*  just after awaking from sleep.
*
* Parameters:
*  None.
*
* Return:
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup: The structure field 'enableState' is used to 
*  restore the enable state of block after wakeup from sleep mode.
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    /* Restore the configuration */
    `$INSTANCE_NAME`_RestoreConfig();

    /* Enable's the component operation */
    if(`$INSTANCE_NAME`_backup.enableState == 1u)
    {
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component was disabled before */
}


/* [] END OF FILE */
