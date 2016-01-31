/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the power management source code to API for the
*  PWM.
*
* Note:
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/
#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

static `$INSTANCE_NAME`_backupStruct `$INSTANCE_NAME`_backup;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the current user configuration of the component.
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
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{
        #if(!`$INSTANCE_NAME`_UsingFixedFunction)
        
            #if(!`$INSTANCE_NAME`_PWMModeIsCenterAligned)
                `$INSTANCE_NAME`_backup.periodValue = `$INSTANCE_NAME`_ReadPeriod();
            #endif
            
            #if(`$INSTANCE_NAME`_DeadBand2_4)
                `$INSTANCE_NAME`_backup.deadBandValue = `$INSTANCE_NAME`_ReadDeadTime();
            #endif
          
            `$INSTANCE_NAME`_backup.counterValue = `$INSTANCE_NAME`_ReadCounter();           
            
            #if(`$INSTANCE_NAME`_UseControl)
                `$INSTANCE_NAME`_backup.control = `$INSTANCE_NAME`_ReadControlRegister();
            #endif 
            
        #endif
     
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
* 
* Summary:
*  Restores the current user configuration of the component.
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup:  Variables of this global structure are used to restore 
*  the values of non retention registers on wakeup from sleep mode.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
        #if(!`$INSTANCE_NAME`_UsingFixedFunction)
        
            #if(!`$INSTANCE_NAME`_PWMModeIsCenterAligned)
                `$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_backup.periodValue);
            #endif
            
            #if(`$INSTANCE_NAME`_DeadBand2_4)
                `$INSTANCE_NAME`_WriteDeadTime(`$INSTANCE_NAME`_backup.deadBandValue);
            #endif
            
            #if(`$INSTANCE_NAME`_UseControl)
                `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_backup.control);
            #endif
            
            `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_backup.counterValue);          
            
        #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
* 
* Summary:
*  Disables block's operation and saves the user configuration. Should be called 
*  just prior to entering sleep.
*  
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup.pwmEnable:  Is modified depending on the enable state
*  of the block before entering sleep mode.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    #if(`$INSTANCE_NAME`_UseControl ||`$INSTANCE_NAME`_UsingFixedFunction)
        if(`$INSTANCE_NAME`_CTRL_ENABLE == (`$INSTANCE_NAME`_CONTROL & `$INSTANCE_NAME`_CTRL_ENABLE))
        {
            /*Component is enabled */
            `$INSTANCE_NAME`_backup.pwmEnable = 1u;
        }
        else
        {
            /* Component is disabled */
            `$INSTANCE_NAME`_backup.pwmEnable = 0u;
        }
    #endif
    /* Stop component */
    `$INSTANCE_NAME`_Stop();
    
    /* Save registers configuration */
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
* 
* Summary:
*  Restores and enables the user configuration. Should be called just after 
*  awaking from sleep.
*  
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  `$INSTANCE_NAME`_backup.pwmEnable:  Is used to restore the enable state of 
*  block on wakeup from sleep mode.
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
     /* Restore registers values */
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.pwmEnable != 0u)
    {
        /* Enable component's operation */
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component's block was disabled before */
    
}

/* [] END OF FILE */
