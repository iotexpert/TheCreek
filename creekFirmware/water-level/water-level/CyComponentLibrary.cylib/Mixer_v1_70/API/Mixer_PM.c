/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the power manager source code to the API for 
*  the MIXER component.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"

static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup =
{
    `$INSTANCE_NAME`_DISABLED
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the current user configuration.
*  
* Parameters:  
*  void.
*
* Return: 
*  void.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{
    /* Nothing to save before entering into sleep mode as all the registers used 
       here are retension registers. */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores the current user configuration.
*
* Parameters:  
*  void.
*
* Return: 
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void)
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
*  `$INSTANCE_NAME`_backup:  The structure field 'enableState' is modified 
*  depending on the enable state of the block before entering to sleep mode.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    /* Save TIA enable state */
    if(`$INSTANCE_NAME`_ACT_PWR_EN == (`$INSTANCE_NAME`_PM_ACT_CFG_REG & `$INSTANCE_NAME`_ACT_PWR_EN))
    {
        /* Component is enabled */
        `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_ENABLED;
    }
    else
    {
        /* Component is disabled */
        `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_DISABLED;
    }

    /* Stop the configuration */
    `$INSTANCE_NAME`_Stop();

    /* Saves the user configuration */
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
*  None
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup:  The structure field 'enableState' is used to 
*  restore the enable state of block after wakeup from sleep mode.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    /* Restore the user configuration */
    `$INSTANCE_NAME`_RestoreConfig();
    
    /* Enable's the component operation */
    if(`$INSTANCE_NAME`_backup.enableState == `$INSTANCE_NAME`_ENABLED)
    {
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component was disable before */
}


/* [] END OF FILE */
