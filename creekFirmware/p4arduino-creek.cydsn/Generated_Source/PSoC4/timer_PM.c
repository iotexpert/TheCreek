/*******************************************************************************
* File Name: timer_PM.c
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

#include "timer.h"

static timer_BACKUP_STRUCT timer_backup;


/*******************************************************************************
* Function Name: timer_SaveConfig
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
void timer_SaveConfig(void)
{

}


/*******************************************************************************
* Function Name: timer_Sleep
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
void timer_Sleep(void)
{
    if(0u != (timer_BLOCK_CONTROL_REG & timer_MASK))
    {
        timer_backup.enableState = 1u;
    }
    else
    {
        timer_backup.enableState = 0u;
    }

    timer_Stop();
    timer_SaveConfig();
}


/*******************************************************************************
* Function Name: timer_RestoreConfig
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
void timer_RestoreConfig(void)
{

}


/*******************************************************************************
* Function Name: timer_Wakeup
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
void timer_Wakeup(void)
{
    timer_RestoreConfig();

    if(0u != timer_backup.enableState)
    {
        timer_Enable();
    }
}


/* [] END OF FILE */
