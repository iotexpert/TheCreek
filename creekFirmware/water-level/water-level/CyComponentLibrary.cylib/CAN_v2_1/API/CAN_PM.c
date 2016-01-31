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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
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
*  This function saves the component configuration and nonretention registers.
*  It also saves the current component parameter values, as defined in the 
*  Configure dialog or as modified by appropriate APIs. This function is called
*  by the `$INSTANCE_NAME`_Sleep() function. 
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
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
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
*  This function restores the component configuration and nonretention 
*  registers. It also restores the component parameter values to what they were 
*  before calling the `$INSTANCE_NAME`_Sleep() function.
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
*  This is the preferred routine to prepare the component for sleep. The 
*  `$INSTANCE_NAME`_Sleep() routine saves the current component state. Then it 
*  calls the `$INSTANCE_NAME`_Stop() function and calls 
*  `$INSTANCE_NAME`_SaveConfig() to save the hardware configuration. Call the
*  `$INSTANCE_NAME`_Sleep() function before calling the CyPmSleep() or the 
*  CyPmHibernate() function. See the PSoC Creator SystemReference Guide for more
*  information about power- management functions.
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
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
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
*  This is the preferred routine to restore the component to the state when 
*  `$INSTANCE_NAME`_Sleep() was called. The `$INSTANCE_NAME`_Wakeup() function 
*  calls the `$INSTANCE_NAME`_RestoreConfig() function to restore the 
*  configuration. If the component was enabled before the 
*  `$INSTANCE_NAME`_Sleep() function was called, the `$INSTANCE_NAME`_Wakeup()
*  function also re-enables the component.
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
*  Calling the `$INSTANCE_NAME`_Wakeup() function without first calling the 
*  `$INSTANCE_NAME`_Sleep() or `$INSTANCE_NAME`_SaveConfig() function may 
*  produce unexpected behavior.
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
