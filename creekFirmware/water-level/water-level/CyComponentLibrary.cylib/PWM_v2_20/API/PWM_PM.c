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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    
    #if(!`$INSTANCE_NAME`_UsingFixedFunction)
        #if (CY_PSOC5A)
            `$INSTANCE_NAME`_backup.PWMUdb = `$INSTANCE_NAME`_ReadCounter();
            `$INSTANCE_NAME`_backup.PWMPeriod = `$INSTANCE_NAME`_ReadPeriod();
            #if (`$INSTANCE_NAME`_UseStatus)
                `$INSTANCE_NAME`_backup.InterruptMaskValue = `$INSTANCE_NAME`_STATUS_MASK;
            #endif /* (`$INSTANCE_NAME`_UseStatus) */
            
            #if(`$INSTANCE_NAME`_UseOneCompareMode)
                `$INSTANCE_NAME`_backup.PWMCompareValue = `$INSTANCE_NAME`_ReadCompare();
            #else
                `$INSTANCE_NAME`_backup.PWMCompareValue1 = `$INSTANCE_NAME`_ReadCompare1();
                `$INSTANCE_NAME`_backup.PWMCompareValue2 = `$INSTANCE_NAME`_ReadCompare2();
            #endif /* (`$INSTANCE_NAME`_UseOneCompareMode) */
            
           #if(`$INSTANCE_NAME`_DeadBandUsed)
                `$INSTANCE_NAME`_backup.PWMdeadBandValue = `$INSTANCE_NAME`_ReadDeadTime();
            #endif /* (`$INSTANCE_NAME`_DeadBandUsed) */
          
            #if ( `$INSTANCE_NAME`_KillModeMinTime)
                `$INSTANCE_NAME`_backup.PWMKillCounterPeriod = `$INSTANCE_NAME`_ReadKillTime();
            #endif /* ( `$INSTANCE_NAME`_KillModeMinTime) */
        #endif /* (CY_PSOC5A) */
        
        #if (CY_PSOC3 || CY_PSOC5LP)
            #if(!`$INSTANCE_NAME`_PWMModeIsCenterAligned)
                `$INSTANCE_NAME`_backup.PWMPeriod = `$INSTANCE_NAME`_ReadPeriod();
            #endif /* (!`$INSTANCE_NAME`_PWMModeIsCenterAligned) */
            `$INSTANCE_NAME`_backup.PWMUdb = `$INSTANCE_NAME`_ReadCounter();
            #if (`$INSTANCE_NAME`_UseStatus)
                `$INSTANCE_NAME`_backup.InterruptMaskValue = `$INSTANCE_NAME`_STATUS_MASK;
            #endif /* (`$INSTANCE_NAME`_UseStatus) */
            
            #if(`$INSTANCE_NAME`_DeadBandMode == `$INSTANCE_NAME`__B_PWM__DBM_256_CLOCKS || \
                `$INSTANCE_NAME`_DeadBandMode == `$INSTANCE_NAME`__B_PWM__DBM_2_4_CLOCKS)
                `$INSTANCE_NAME`_backup.PWMdeadBandValue = `$INSTANCE_NAME`_ReadDeadTime();
            #endif /*  deadband count is either 2-4 clocks or 256 clocks */
            
            #if(`$INSTANCE_NAME`_KillModeMinTime)
                 `$INSTANCE_NAME`_backup.PWMKillCounterPeriod = `$INSTANCE_NAME`_ReadKillTime();
            #endif /* (`$INSTANCE_NAME`_KillModeMinTime) */
        #endif /* (CY_PSOC3 || CY_PSOC5LP) */
        
        #if(`$INSTANCE_NAME`_UseControl)
            `$INSTANCE_NAME`_backup.PWMControlRegister = `$INSTANCE_NAME`_ReadControlRegister();
        #endif /* (`$INSTANCE_NAME`_UseControl) */
    #endif  /* (!`$INSTANCE_NAME`_UsingFixedFunction) */
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
*  `$INSTANCE_NAME`_backup:  Variables of this global structure are used to  
*  restore the values of non retention registers on wakeup from sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
        #if(!`$INSTANCE_NAME`_UsingFixedFunction)
            #if (CY_PSOC5A)
                /* Interrupt State Backup for Critical Region*/
                uint8 `$INSTANCE_NAME`_interruptState;
                /* Enter Critical Region*/
                `$INSTANCE_NAME`_interruptState = CyEnterCriticalSection();
                #if (`$INSTANCE_NAME`_UseStatus)
                    /* Use the interrupt output of the status register for IRQ output */
                    `$INSTANCE_NAME`_STATUS_AUX_CTRL |= `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK;
                    
                    `$INSTANCE_NAME`_STATUS_MASK = `$INSTANCE_NAME`_backup.InterruptMaskValue;
                #endif /* (`$INSTANCE_NAME`_UseStatus) */
                
                #if (`$INSTANCE_NAME`_Resolution == 8)
                    /* Set FIFO 0 to 1 byte register for period*/
                    `$INSTANCE_NAME`_AUX_CONTROLDP0 |= (`$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR);
                #else /* (`$INSTANCE_NAME`_Resolution == 16)*/
                    /* Set FIFO 0 to 1 byte register for period */
                    `$INSTANCE_NAME`_AUX_CONTROLDP0 |= (`$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR);
                    `$INSTANCE_NAME`_AUX_CONTROLDP1 |= (`$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR);
                #endif /* (`$INSTANCE_NAME`_Resolution == 8) */
                /* Exit Critical Region*/
                CyExitCriticalSection(`$INSTANCE_NAME`_interruptState);
                
                `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_backup.PWMUdb);
                `$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_backup.PWMPeriod);
                
                #if(`$INSTANCE_NAME`_UseOneCompareMode)
                    `$INSTANCE_NAME`_WriteCompare(`$INSTANCE_NAME`_backup.PWMCompareValue);
                #else
                    `$INSTANCE_NAME`_WriteCompare1(`$INSTANCE_NAME`_backup.PWMCompareValue1);
                    `$INSTANCE_NAME`_WriteCompare2(`$INSTANCE_NAME`_backup.PWMCompareValue2);
                #endif /* (`$INSTANCE_NAME`_UseOneCompareMode) */
                
               #if(`$INSTANCE_NAME`_DeadBandMode == `$INSTANCE_NAME`__B_PWM__DBM_256_CLOCKS || \
                   `$INSTANCE_NAME`_DeadBandMode == `$INSTANCE_NAME`__B_PWM__DBM_2_4_CLOCKS)
                    `$INSTANCE_NAME`_WriteDeadTime(`$INSTANCE_NAME`_backup.PWMdeadBandValue);
                #endif /* deadband count is either 2-4 clocks or 256 clocks */
            
                #if ( `$INSTANCE_NAME`_KillModeMinTime)
                    `$INSTANCE_NAME`_WriteKillTime(`$INSTANCE_NAME`_backup.PWMKillCounterPeriod);
                #endif /* ( `$INSTANCE_NAME`_KillModeMinTime) */
            #endif /* (CY_PSOC5A) */
            
            #if (CY_PSOC3 || CY_PSOC5LP)
                #if(!`$INSTANCE_NAME`_PWMModeIsCenterAligned)
                    `$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_backup.PWMPeriod);
                #endif /* (!`$INSTANCE_NAME`_PWMModeIsCenterAligned) */
                `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_backup.PWMUdb);
                #if (`$INSTANCE_NAME`_UseStatus)
                    `$INSTANCE_NAME`_STATUS_MASK = `$INSTANCE_NAME`_backup.InterruptMaskValue;
                #endif /* (`$INSTANCE_NAME`_UseStatus) */
                
                #if(`$INSTANCE_NAME`_DeadBandMode == `$INSTANCE_NAME`__B_PWM__DBM_256_CLOCKS || \
                    `$INSTANCE_NAME`_DeadBandMode == `$INSTANCE_NAME`__B_PWM__DBM_2_4_CLOCKS)
                    `$INSTANCE_NAME`_WriteDeadTime(`$INSTANCE_NAME`_backup.PWMdeadBandValue);
                #endif /* deadband count is either 2-4 clocks or 256 clocks */
                
                #if(`$INSTANCE_NAME`_KillModeMinTime)
                    `$INSTANCE_NAME`_WriteKillTime(`$INSTANCE_NAME`_backup.PWMKillCounterPeriod);
                #endif /* (`$INSTANCE_NAME`_KillModeMinTime) */
            #endif /* (CY_PSOC3 || CY_PSOC5LP) */
            
            #if(`$INSTANCE_NAME`_UseControl)
                `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_backup.PWMControlRegister); 
            #endif /* (`$INSTANCE_NAME`_UseControl) */
        #endif  /* (!`$INSTANCE_NAME`_UsingFixedFunction) */
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
*  `$INSTANCE_NAME`_backup.PWMEnableState:  Is modified depending on the enable 
*  state of the block before entering sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    #if(`$INSTANCE_NAME`_UseControl)
        if(`$INSTANCE_NAME`_CTRL_ENABLE == (`$INSTANCE_NAME`_CONTROL & `$INSTANCE_NAME`_CTRL_ENABLE))
        {
            /*Component is enabled */
            `$INSTANCE_NAME`_backup.PWMEnableState = 1u;
        }
        else
        {
            /* Component is disabled */
            `$INSTANCE_NAME`_backup.PWMEnableState = 0u;
        }
    #endif /* (`$INSTANCE_NAME`_UseControl) */
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
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
     /* Restore registers values */
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.PWMEnableState != 0u)
    {
        /* Enable component's operation */
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component's block was disabled before */
    
}


/* [] END OF FILE */
