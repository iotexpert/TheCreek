/*******************************************************************************
* File Name: sysclk_PM.c
* Version 1.0
*
* Description:
*  This file contains the setup, control and status commands to support
*  component operations in low power mode.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "sysclk.h"

static sysclk_BACKUP_STRUCT sysclk_backup;


/*******************************************************************************
* Function Name: sysclk_SaveConfig
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
void sysclk_SaveConfig(void)
{

}


/*******************************************************************************
* Function Name: sysclk_Sleep
********************************************************************************
*
* Summary:
*  Stops the component operation and saves the user configuration.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void sysclk_Sleep(void)
{
    if(0u != (sysclk_BLOCK_CONTROL_REG & sysclk_MASK))
    {
        sysclk_backup.enableState = 1u;
    }
    else
    {
        sysclk_backup.enableState = 0u;
    }

    sysclk_Stop();
    sysclk_SaveConfig();
}


/*******************************************************************************
* Function Name: sysclk_RestoreConfig
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
void sysclk_RestoreConfig(void)
{

}


/*******************************************************************************
* Function Name: sysclk_Wakeup
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
void sysclk_Wakeup(void)
{
    sysclk_RestoreConfig();

    if(0u != sysclk_backup.enableState)
    {
        sysclk_Enable();
    }
}


/* [] END OF FILE */
