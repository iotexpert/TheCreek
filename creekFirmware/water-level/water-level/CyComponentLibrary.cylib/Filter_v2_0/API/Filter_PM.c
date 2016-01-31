/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the power managaer API source code for the FILT component.
*
* Note:
*  
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`_PVT.h"

static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup = 
{
    `$INSTANCE_NAME`_DISABLED,
    `$INSTANCE_NAME`_RUN_MASK,
    
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the current user configuration.
*  
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup:  This global structure variable is used to store 
*  configuration registers which are non retention whenever user wants to go 
*  sleep mode by calling Sleep() API.
*
* Reentrance:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{
    `$INSTANCE_NAME`_backup.cr = `$INSTANCE_NAME`_CR_REG;
    `$INSTANCE_NAME`_backup.sr = `$INSTANCE_NAME`_SR_REG;
    `$INSTANCE_NAME`_backup.sema = `$INSTANCE_NAME`_SEMA_REG;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores the current user configuration.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup:  This global structure variable is used to restore 
*  configuration registers which are non retention whenever user wants to switch 
*  to active power mode by calling Wakeup() API.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    `$INSTANCE_NAME`_CR_REG = `$INSTANCE_NAME`_backup.cr;
    `$INSTANCE_NAME`_SR_REG = `$INSTANCE_NAME`_backup.sr;
    `$INSTANCE_NAME`_SEMA_REG = `$INSTANCE_NAME`_backup.sema;
    // Restore ACU RAM as this is not retension
    `$INSTANCE_NAME`_RestoreACURam();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreACURAM
********************************************************************************
*
* Summary:
*  Restores the contents of ACU ram.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreACURam() `=ReentrantKeil($INSTANCE_NAME . "_RestoreACURam")`
{
    /* Power on DFB before initializing the RAMs */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_PM_ACT_MSK;

    /* Put DFB RAM on the bus */
    `$INSTANCE_NAME`_RAM_DIR_REG = `$INSTANCE_NAME`_RAM_DIR_BUS;

    /* ACU RAM */
    memcpy(`$INSTANCE_NAME`_ACU_RAM,
        `$INSTANCE_NAME`_acu, `$INSTANCE_NAME`_ACU_RAM_SIZE); 

    /* Take DFB RAM off the bus */
    `$INSTANCE_NAME`_RAM_DIR_REG = `$INSTANCE_NAME`_RAM_DIR_DFB;
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
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup:  The structure field 'enableState' is modified 
*  depending on the enable state of the block before entering to sleep mode.
*
* Reentrance:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    /* Save Filter enable state */
    if(`$INSTANCE_NAME`_PM_ACT_MSK == (`$INSTANCE_NAME`_PM_ACT_CFG_REG & `$INSTANCE_NAME`_PM_ACT_MSK))
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
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup:  The structure field 'enableState' is used to 
*  restore the enable state of block after wakeup from sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    /* Restore the configuration */
    `$INSTANCE_NAME`_RestoreConfig();
    
    /* Enable's the component operation */
    if(`$INSTANCE_NAME`_backup.enableState == `$INSTANCE_NAME`_ENABLED)
    {
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component was disable before */
}


/* [] END OF FILE */
