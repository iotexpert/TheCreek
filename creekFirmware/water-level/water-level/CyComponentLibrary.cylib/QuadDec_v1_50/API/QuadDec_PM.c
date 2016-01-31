/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the setup, control and status commands to support 
*  component operations in low power mode.  
*
* Note:
*  None.
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

static `$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup = {0u};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
* Summary:
*  Saves the current user configuration of the component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 8u)
        `$INSTANCE_NAME`_Cnt8_SaveConfig();
    #else /* (`$INSTANCE_NAME`_COUNTER_SIZE == 16u) || (`$INSTANCE_NAME`_COUNTER_SIZE == 32u) */
        `$INSTANCE_NAME`_Cnt16_SaveConfig();                                          
    #endif /* (`$INSTANCE_NAME`_COUNTER_SIZE == 8u) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
* Summary:
*  Restores the current user configuration of the component.
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
    #if (`$INSTANCE_NAME`_COUNTER_SIZE == 8u)
        `$INSTANCE_NAME`_Cnt8_RestoreConfig();
    #else /* (`$INSTANCE_NAME`_COUNTER_SIZE == 16u) || (`$INSTANCE_NAME`_COUNTER_SIZE == 32u) */
        `$INSTANCE_NAME`_Cnt16_RestoreConfig();                                          
    #endif /* (`$INSTANCE_NAME`_COUNTER_SIZE == 8u) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
* 
* Summary:
*  Prepare Quadrature Decoder Component goes to sleep.
*
* Parameters:  
*  None.  
*
* Return: 
*  None.
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - modified when non-retention registers are saved.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{   
    if((`$INSTANCE_NAME`_SR_AUX_CONTROL & `$INSTANCE_NAME`_INTERRUPTS_ENABLE) == `$INSTANCE_NAME`_INTERRUPTS_ENABLE)
    {
        `$INSTANCE_NAME`_backup.enableState = 1u;
    }
    else /* The Quadrature Decoder Component is disabled */
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
*  Prepare Quadrature Decoder Component to wake up.
*
* Parameters:  
*  None.
*
* Return: 
*  None.
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used when non-retention registers are restored.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{             
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.enableState != 0u)
    {       
        #if (`$INSTANCE_NAME`_COUNTER_SIZE == 8u)
            `$INSTANCE_NAME`_Cnt8_Enable();
        #else /* (`$INSTANCE_NAME`_COUNTER_SIZE == 16u) || (`$INSTANCE_NAME`_COUNTER_SIZE == 32u) */
            `$INSTANCE_NAME`_Cnt16_Enable();                                          
        #endif /* (`$INSTANCE_NAME`_COUNTER_SIZE == 8u) */
        
        /* Enable component's operation */
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component's block was disabled before */
}


/* [] END OF FILE */

