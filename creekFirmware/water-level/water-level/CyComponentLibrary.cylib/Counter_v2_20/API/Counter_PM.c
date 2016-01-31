/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file provides the power management source code to API for the
*    Counter.  
*
*   Note:
*     None
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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
*     Save the current user configuration
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup:  Variables of this global structure are modified to 
*  store the values of non retention configuration registers when Sleep() API is 
*  called.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    #if (!`$INSTANCE_NAME`_UsingFixedFunction)
        /* Backup the UDB non-rentention registers for PSoC5A */
        #if (CY_PSOC5A)
            `$INSTANCE_NAME`_backup.CounterUdb = `$INSTANCE_NAME`_ReadCounter();
            `$INSTANCE_NAME`_backup.CounterPeriod = `$INSTANCE_NAME`_ReadPeriod();
            `$INSTANCE_NAME`_backup.CompareValue = `$INSTANCE_NAME`_ReadCompare();
            `$INSTANCE_NAME`_backup.InterruptMaskValue = `$INSTANCE_NAME`_STATUS_MASK;
        #endif /* (CY_PSOC5A) */
        
        #if (CY_PSOC3 || CY_PSOC5LP)
            `$INSTANCE_NAME`_backup.CounterUdb = `$INSTANCE_NAME`_ReadCounter();
            `$INSTANCE_NAME`_backup.InterruptMaskValue = `$INSTANCE_NAME`_STATUS_MASK;
        #endif /* (CY_PSOC3 || CY_PSOC5LP) */
        
        #if(!`$INSTANCE_NAME`_ControlRegRemoved)
            `$INSTANCE_NAME`_backup.CounterControlRegister = `$INSTANCE_NAME`_ReadControlRegister();
        #endif /* (!`$INSTANCE_NAME`_ControlRegRemoved) */
    #endif /* (!`$INSTANCE_NAME`_UsingFixedFunction) */
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
*  `$INSTANCE_NAME`_backup:  Variables of this global structure are used to 
*  restore the values of non retention registers on wakeup from sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{      
    #if (!`$INSTANCE_NAME`_UsingFixedFunction)     
        /* Restore the UDB non-rentention registers for PSoC5A */
        #if (CY_PSOC5A)
            /* Interrupt State Backup for Critical Region*/
            uint8 `$INSTANCE_NAME`_interruptState;
        
            `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_backup.CounterUdb);
            `$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_backup.CounterPeriod);
            `$INSTANCE_NAME`_WriteCompare(`$INSTANCE_NAME`_backup.CompareValue);
            /* Enter Critical Region*/
            `$INSTANCE_NAME`_interruptState = CyEnterCriticalSection();
        
            `$INSTANCE_NAME`_STATUS_AUX_CTRL |= `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK;
            /* Exit Critical Region*/
            CyExitCriticalSection(`$INSTANCE_NAME`_interruptState);
            `$INSTANCE_NAME`_STATUS_MASK = `$INSTANCE_NAME`_backup.InterruptMaskValue;
        #endif  /* (CY_PSOC5A) */
        
        #if (CY_PSOC3 || CY_PSOC5LP)
            `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_backup.CounterUdb);
            `$INSTANCE_NAME`_STATUS_MASK = `$INSTANCE_NAME`_backup.InterruptMaskValue;
        #endif /* (CY_PSOC3 || CY_PSOC5LP) */
        
        #if(!`$INSTANCE_NAME`_ControlRegRemoved)
            `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_backup.CounterControlRegister);
        #endif /* (!`$INSTANCE_NAME`_ControlRegRemoved) */
    #endif /* (!`$INSTANCE_NAME`_UsingFixedFunction) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
* Summary:
*     Stop and Save the user configuration
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup.enableState:  Is modified depending on the enable 
*  state of the block before entering sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    #if(!`$INSTANCE_NAME`_ControlRegRemoved)
        /* Save Counter's enable state */
        if(`$INSTANCE_NAME`_CTRL_ENABLE == (`$INSTANCE_NAME`_CONTROL & `$INSTANCE_NAME`_CTRL_ENABLE))
        {
            /* Counter is enabled */
            `$INSTANCE_NAME`_backup.CounterEnableState = 1u;
        }
        else
        {
            /* Counter is disabled */
            `$INSTANCE_NAME`_backup.CounterEnableState = 0u;
        }
    #endif /* (!`$INSTANCE_NAME`_ControlRegRemoved) */
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
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    #if(!`$INSTANCE_NAME`_ControlRegRemoved)
        if(`$INSTANCE_NAME`_backup.CounterEnableState == 1u)
        {
            /* Enable Counter's operation */
            `$INSTANCE_NAME`_Enable();
        } /* Do nothing if Counter was disabled before */    
    #endif /* (!`$INSTANCE_NAME`_ControlRegRemoved) */
    
}


/* [] END OF FILE */
