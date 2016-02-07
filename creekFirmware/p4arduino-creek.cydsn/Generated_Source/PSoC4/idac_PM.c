/*******************************************************************************
* File Name: idac_PM.c
* Version 1.0
*
* Description:
*  This file provides Low power mode APIs for IDAC_P4 component.
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "idac.h"


static idac_BACKUP_STRUCT idac_backup;


/*******************************************************************************
* Function Name: idac_SaveConfig
********************************************************************************
*
* Summary:
*  Saves component state before sleep
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  None
*
*******************************************************************************/
void idac_SaveConfig(void)
{
    /* All registers are retention - nothing to save */
}


/*******************************************************************************
* Function Name: idac_Sleep
********************************************************************************
*
* Summary:
*  Calls _SaveConfig() function
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void idac_Sleep(void)
{
        if(0u != (idac_IDAC_CONTROL_REG & ((uint32)idac_IDAC_MODE_MASK <<
        idac_IDAC_MODE_POSITION)))
        {
            idac_backup.enableState = 1u;
        }
        else
        {
            idac_backup.enableState = 0u;
        }

    idac_Stop();
    idac_SaveConfig();
}


/*******************************************************************************
* Function Name: idac_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores component state after wakeup
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  None
*
*******************************************************************************/
void idac_RestoreConfig(void)
{
    /* All registers are retention - nothing to save */
}


/*******************************************************************************
* Function Name: idac_Wakeup
********************************************************************************
*
* Summary:
*  Calls _RestoreConfig() function
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void idac_Wakeup(void)
{
    /* Restore IDAC register settings */
    idac_RestoreConfig();
    if(idac_backup.enableState == 1u)
    {
        /* Enable operation */
        idac_Enable();
    } /* Do nothing if the component was disabled before */

}


/* [] END OF FILE */
