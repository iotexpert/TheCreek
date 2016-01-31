/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     This file provides the power management source code to API for the
*     IDAC8.  
*
*   Note:
*     None
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include "`$INSTANCE_NAME`.h"

static `$INSTANCE_NAME`_backupStruct `$INSTANCE_NAME`_backup;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
* Summary:
*  Save the current user configuration
*
* Parameters:  
*  void  
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{
    `$INSTANCE_NAME`_backup.data_value = `$INSTANCE_NAME`_Data;
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
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void)
{
    `$INSTANCE_NAME`_Data = `$INSTANCE_NAME`_backup.data_value;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
* Summary:
*     Stop and Save the user configuration
*
* Parameters:  
*  void:  
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup.enableState:  Is modified depending on the enable 
*   state of the block before entering sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    if(`$INSTANCE_NAME`_ACT_PWR_EN == (`$INSTANCE_NAME`_PWRMGR & `$INSTANCE_NAME`_ACT_PWR_EN))
    {
        /* IDAC8 is enabled */
        `$INSTANCE_NAME`_backup.enableState = 1u;
    }
    else
    {
        /* IDAC8 is disabled */
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
*  Restores and enables the user configuration
*  
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup.enableState:  Is used to restore the enable state of 
*  block on wakeup from sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void)
{
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.enableState == 1u)
    {
        /* Enable IDAC8's operation */
        `$INSTANCE_NAME`_Enable();
       
        /* Set the data register */
        `$INSTANCE_NAME`_SetValue(`$INSTANCE_NAME`_Data);
    } /* Do nothing if IDAC8 was disabled before */    
}


/* [] END OF FILE */
