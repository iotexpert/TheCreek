/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for sleep mode support for Shift 
*  Register component.
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


static `$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup = \
{
    /* enable state - disabled */
    0u
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves Shift Register configuration.
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
    /* Store A0, A1 and Status Mask registers */
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
       `$INSTANCE_NAME`_backup.saveSrA0Reg   = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR);
       `$INSTANCE_NAME`_backup.saveSrA1Reg   = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR);
       `$INSTANCE_NAME`_backup.saveSrIntMask = `$INSTANCE_NAME`_SR_STATUS_MASK;

    #else
    /* Store A0, A1 only (not need to save Status Mask register  in ES3 silicon) */
       `$INSTANCE_NAME`_backup.saveSrA0Reg   = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR);
       `$INSTANCE_NAME`_backup.saveSrA1Reg   = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR);

    #endif /*(CY_PSOC3_ES2 || CY_PSOC5_ES1)*/
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores Shift Register configuration.
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
    /* Restore tha data, saved by SaveConfig()function */
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR, `$INSTANCE_NAME`_backup.saveSrA0Reg);
            `$CySetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR, `$INSTANCE_NAME`_backup.saveSrA1Reg);
            `$INSTANCE_NAME`_SR_STATUS_MASK = `$INSTANCE_NAME`_backup.saveSrIntMask;
    #else
            `$CySetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_LSB_PTR, `$INSTANCE_NAME`_backup.saveSrA0Reg);
            `$CySetRegReplacementString`(`$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR, `$INSTANCE_NAME`_backup.saveSrA1Reg);

    #endif /*(CY_PSOC3_ES2 || CY_PSOC5_ES1)*/
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
    if((`$INSTANCE_NAME`_SR_CONTROL & `$INSTANCE_NAME`_CLK_EN) == `$INSTANCE_NAME`_CLK_EN)
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
*  Restores and enables the user configuration.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.enableState == 1u)
    {
        `$INSTANCE_NAME`_Enable();   
    }
}

/* [] END OF FILE */
