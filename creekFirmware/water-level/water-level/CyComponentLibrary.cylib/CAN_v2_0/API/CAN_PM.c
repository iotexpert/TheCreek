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
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

static `$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup = { 0u,
                                                                  0u,
                                                                  `$INSTANCE_NAME`_INIT_INTERRUPT_MASK,
                                                                  `$INSTANCE_NAME`_MODE_MASK };


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Save CAN configuration.
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
void `$INSTANCE_NAME`_SaveConfig(void)
{
    `$INSTANCE_NAME`_backup.intSr = (CY_GET_REG32(`$INSTANCE_NAME`_INT_SR_PTR));
    `$INSTANCE_NAME`_backup.intEn = (CY_GET_REG32(`$INSTANCE_NAME`_INT_EN_PTR));
    `$INSTANCE_NAME`_backup.cmd = (CY_GET_REG32(`$INSTANCE_NAME`_CMD_PTR));
    `$INSTANCE_NAME`_backup.cfg = (CY_GET_REG32(`$INSTANCE_NAME`_CFG_PTR));
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restore CAN configuration.
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
* Side Effects:
*  If this API is called without first calling SaveConfig then in the following
*  registers will be default values from Customizer: `$INSTANCE_NAME`_INT_SR, 
*  `$INSTANCE_NAME`_INT_EN, `$INSTANCE_NAME`_CMD, `$INSTANCE_NAME`_CFG.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    CY_SET_REG32(`$INSTANCE_NAME`_INT_SR_PTR, `$INSTANCE_NAME`_backup.intSr);
    CY_SET_REG32(`$INSTANCE_NAME`_INT_EN_PTR, `$INSTANCE_NAME`_backup.intEn);
    CY_SET_REG32(`$INSTANCE_NAME`_CMD_PTR, `$INSTANCE_NAME`_backup.cmd);
    CY_SET_REG32(`$INSTANCE_NAME`_CFG_PTR, `$INSTANCE_NAME`_backup.cfg);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
* 
* Summary:
*  Prepare CAN Component goes to sleep.
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
    if((`$INSTANCE_NAME`_CMD_REG.byte[0u] & `$INSTANCE_NAME`_MODE_MASK) == `$INSTANCE_NAME`_MODE_MASK)       
    {
        `$INSTANCE_NAME`_backup.enableState = 1u;
    }
    else /* The CAN block is disabled */
    {
        `$INSTANCE_NAME`_backup.enableState = 0u;
    }
    
    `$INSTANCE_NAME`_SaveConfig();
    `$INSTANCE_NAME`_Stop();    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
* 
* Summary:
*  Prepare CAN Component to wake up.
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
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{           
    /* Enable power to CAN */
    `$INSTANCE_NAME`_PM_ACT_CFG_REG |= `$INSTANCE_NAME`_ACT_PWR_EN;
    
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_backup.enableState != 0u)
    {
        /* Enable component's operation */
        `$INSTANCE_NAME`_Enable();
    } /* Do nothing if component's block was disabled before */
}


/* [] END OF FILE */
