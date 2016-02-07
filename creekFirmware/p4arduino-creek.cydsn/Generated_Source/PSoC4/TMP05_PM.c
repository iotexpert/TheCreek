/*******************************************************************************
* File Name: TMP05_PM.c
* Version 1.10
*
* Description:
*  This file contains the setup, control and status commands to support
*  component operations in low power mode.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012-2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "TMP05_PVT.h"

static TMP05_BACKUP_STRUCT TMP05_backup;


/*******************************************************************************
* Function Name: TMP05_SaveConfig
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
void TMP05_SaveConfig(void) 
{

}


/*******************************************************************************
* Function Name: TMP05_Sleep
********************************************************************************
*
* Summary:
*  Stops the TMP05 operation and saves the user configuration.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void TMP05_Sleep(void) 
{
    if(0u != (TMP05_CONTROL_REG & TMP05_CTRL_REG_ENABLE))
    {
        TMP05_backup.enableState = 1u;
    }
    else /* The TMP05 block is disabled */
    {
        TMP05_backup.enableState = 0u;
    }

    TMP05_Stop();
    TMP05_SaveConfig();
}


/*******************************************************************************
* Function Name: TMP05_RestoreConfig
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
void TMP05_RestoreConfig(void) 
{

}


/*******************************************************************************
* Function Name: TMP05_Wakeup
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
void TMP05_Wakeup(void) 
{
    TMP05_RestoreConfig();

    if(0u != TMP05_backup.enableState)
    {
        /* Enable component's operation */
        TMP05_Enable();
        TMP05_Trigger();
    } /* Do nothing if component's block was disabled before */
    else
    {
        TMP05_Enable();
    }
}


/* [] END OF FILE */
