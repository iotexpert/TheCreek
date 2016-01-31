/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the setup, control and status commands to support 
*  component operations in low power mode. 
*
* Note:
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
extern uint8 `$INSTANCE_NAME`_enableVar;

`$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup =
{
    0x0u, /* enableState */
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the configuration of the DelSig ADC or SAR ADC.
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
    #if(`$INSTANCE_NAME`_SAR_SELECT == `$INSTANCE_NAME`_SAR) 
        `$INSTANCE_NAME`_ADC_SAR_SaveConfig();
    #else
        `$INSTANCE_NAME`_ADC_SaveConfig();
    #endif  /* End  (`$INSTANCE_NAME`_SAR_SELECT) */    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores the configuration of the DelSig ADC or SAR ADC.
*
* Parameters:
*  None
*
* Return:
*  None
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    #if(`$INSTANCE_NAME`_SAR_SELECT == `$INSTANCE_NAME`_SAR) 
        `$INSTANCE_NAME`_ADC_SAR_RestoreConfig();
    #else
        `$INSTANCE_NAME`_ADC_RestoreConfig();
    #endif  /* (`$INSTANCE_NAME`_SAR_SELECT) */    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Prepares the DelSig ADC or SAR ADC for low power modes by calling 
*  SaveConfig and Stop functions.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    if(`$INSTANCE_NAME`_enableVar == 1u)       
    {
        `$INSTANCE_NAME`_backup.enableState = 1u;
        `$INSTANCE_NAME`_Stop();
    }
    else /* The ResistiveTouch block is disabled */
    {
        `$INSTANCE_NAME`_backup.enableState = 0u;
    }

    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Restores the DelSig ADC or SAR ADC after waking up from a low power mode.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void)  `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.enableState != 0u)
    {
        /* Enable component's operation */
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component's block was disabled before */
}


/* [] END OF FILE */
