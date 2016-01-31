/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     This file provides the power management source code to API for the
*     Timer.
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
*
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
* Reentrant:
*    No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{
    #if (!`$INSTANCE_NAME`_UsingFixedFunction)
        /* Backup the UDB non-rentention registers for PSoC3 ES2 and PSoC5 ES1*/
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_backup.TimerUdb = `$INSTANCE_NAME`_ReadCounter();
            `$INSTANCE_NAME`_backup.TimerPeriod = `$INSTANCE_NAME`_ReadPeriod();
            `$INSTANCE_NAME`_backup.InterruptMaskValue = `$INSTANCE_NAME`_STATUS_MASK;
            #if (`$INSTANCE_NAME`_UsingHWCaptureCounter)
                `$INSTANCE_NAME`_backup.TimerCaptureCounter = `$INSTANCE_NAME`_ReadCaptureCount();
            #endif /* Backup the UDB non-rentention register capture counter for PSoC3 ES2 and PSoC5 ES1 */
        #endif /* Backup the UDB non-rentention registers for PSoC3 ES2 and PSoC5 ES1*/

        #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
            `$INSTANCE_NAME`_backup.TimerUdb = `$INSTANCE_NAME`_ReadCounter();
            `$INSTANCE_NAME`_backup.InterruptMaskValue = `$INSTANCE_NAME`_STATUS_MASK;
            #if (`$INSTANCE_NAME`_UsingHWCaptureCounter)
                `$INSTANCE_NAME`_backup.TimerCaptureCounter = `$INSTANCE_NAME`_ReadCaptureCount();
            #endif /* Back Up capture counter register  */
        #endif /* Backup non retention registers, interrupt mask and capture counter for PSoC3ES3 or PSoC5ES2 */

        #if(!`$INSTANCE_NAME`_ControlRegRemoved)
            `$INSTANCE_NAME`_backup.TimerControlRegister = `$INSTANCE_NAME`_ReadControlRegister();
        #endif /* Backup the enable state of the Timer component */
    #endif /* Backup non retention registers in UDB implementation. All fixed function registers are retention */
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
* Reentrant:
*    Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{   
    #if (!`$INSTANCE_NAME`_UsingFixedFunction)
        /* Restore the UDB non-rentention registers for PSoC3 ES2 and PSoC5 ES1*/
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            /* Interrupt State Backup for Critical Region*/
            uint8 `$INSTANCE_NAME`_interruptState;

            `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_backup.TimerUdb);
            `$INSTANCE_NAME`_WritePeriod(`$INSTANCE_NAME`_backup.TimerPeriod);
            /* CyEnterCriticalRegion and CyExitCriticalRegion are used to mark following region critical*/
            /* Enter Critical Region*/
            `$INSTANCE_NAME`_interruptState = CyEnterCriticalSection();
            /* Use the interrupt output of the status register for IRQ output */
            `$INSTANCE_NAME`_STATUS_AUX_CTRL |= `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK;
            /* Exit Critical Region*/
            CyExitCriticalSection(`$INSTANCE_NAME`_interruptState);
            `$INSTANCE_NAME`_STATUS_MASK =`$INSTANCE_NAME`_backup.InterruptMaskValue;
            #if (`$INSTANCE_NAME`_UsingHWCaptureCounter)
                `$INSTANCE_NAME`_SetCaptureCount(`$INSTANCE_NAME`_backup.TimerCaptureCounter);
            #endif /* Restore the UDB non-rentention register capture counter for PSoC3 ES2 and PSoC5 ES1 */
        #endif /* Restore the UDB non-rentention registers for PSoC3 ES2 and PSoC5 ES1*/

        #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
            `$INSTANCE_NAME`_WriteCounter(`$INSTANCE_NAME`_backup.TimerUdb);
            `$INSTANCE_NAME`_STATUS_MASK =`$INSTANCE_NAME`_backup.InterruptMaskValue;
            #if (`$INSTANCE_NAME`_UsingHWCaptureCounter)
                `$INSTANCE_NAME`_SetCaptureCount(`$INSTANCE_NAME`_backup.TimerCaptureCounter);
            #endif /* Restore Capture counter register*/
        #endif /* Restore up non retention registers, interrupt mask and capture counter for PSoC3ES3 or PSoC5ES2 */

        #if(!`$INSTANCE_NAME`_ControlRegRemoved)
            `$INSTANCE_NAME`_WriteControlRegister(`$INSTANCE_NAME`_backup.TimerControlRegister);
        #endif /* Restore the enable state of the Timer component */
    #endif /* Restore non retention registers in the UDB implementation only */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
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
*  `$INSTANCE_NAME`_backup.TimerEnableState:  Is modified depending on the
*  enable state of the block before entering sleep mode.
*
* Reentrant:
*    No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    #if(!`$INSTANCE_NAME`_ControlRegRemoved)
        /* Save Counter's enable state */
        if(`$INSTANCE_NAME`_CTRL_ENABLE == (`$INSTANCE_NAME`_CONTROL & `$INSTANCE_NAME`_CTRL_ENABLE))
        {
            /* Timer is enabled */
            `$INSTANCE_NAME`_backup.TimerEnableState = 1u;
        }
        else
        {
            /* Timer is disabled */
            `$INSTANCE_NAME`_backup.TimerEnableState = 0u;
        }
    #endif /* Back up enable state from the Timer control register */
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
* Reentrant:
*    Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    #if(!`$INSTANCE_NAME`_ControlRegRemoved)
        if(`$INSTANCE_NAME`_backup.TimerEnableState == 1u)
        {     /* Enable Timer's operation */
                `$INSTANCE_NAME`_Enable();
        } /* Do nothing if Timer was disabled before */
    #endif /* Remove this code section if Control register is removed */
}


/* [] END OF FILE */
