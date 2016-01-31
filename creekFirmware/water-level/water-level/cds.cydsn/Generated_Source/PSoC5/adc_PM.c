/*******************************************************************************
* File Name: adc_PM.c  
* Version 2.30
*
* Description:
*  This file provides the power manager source code to the API for the 
*  Delta-Sigma ADC Component.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "adc.h"

static adc_BACKUP_STRUCT adc_backup = 
{
    adc_DISABLED,
    adc_CFG1_DEC_CR
};


/*******************************************************************************  
* Function Name: adc_SaveConfig
********************************************************************************
*
* Summary:
*  Save the register configuration which are not retention.
* 
* Parameters:
*  void
* 
* Return:
*  void
*
* Global variables:
*  adc_backup:  This global structure variable is used to store 
*  configuration registers which are non retention whenever user wants to go 
*  sleep mode by calling Sleep() API.
*
*******************************************************************************/
void adc_SaveConfig(void) 
{
    adc_backup.deccr = adc_DEC_CR_REG;
}


/*******************************************************************************  
* Function Name: adc_RestoreConfig
********************************************************************************
*
* Summary:
*  Restore the register configurations which are not retention.
* 
* Parameters:
*  void
* 
* Return:
*  void
*
* Global variables:
*  adc_backup:  This global structure variable is used to restore 
*  configuration registers which are non retention whenever user wants to switch 
*  to active power mode by calling Wakeup() API.
*
*******************************************************************************/
void adc_RestoreConfig(void) 
{
    adc_DEC_CR_REG = adc_backup.deccr;
}


/******************************************************************************* 
* Function Name: adc_Sleep
********************************************************************************
*
* Summary:
*  Stops the operation of the block and saves the user configuration. 
*  
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  adc_backup:  The structure field 'enableState' is modified 
*  depending on the enable state of the block before entering to sleep mode.
*
*******************************************************************************/
void adc_Sleep(void) 
{
    /* Save ADC enable state */
    if((adc_ACT_PWR_DEC_EN == (adc_PWRMGR_DEC_REG & adc_ACT_PWR_DEC_EN)) &&
       (adc_ACT_PWR_DSM_EN == (adc_PWRMGR_DSM_REG & adc_ACT_PWR_DSM_EN)))
    {
        /* Component is enabled */
        adc_backup.enableState = adc_ENABLED;
    }
    else
    {
        /* Component is disabled */
        adc_backup.enableState = adc_DISABLED;
    }

    /* Stop the configuration */
    adc_Stop();

    /* Save the user configuration */
    adc_SaveConfig();
}


/******************************************************************************* 
* Function Name: adc_Wakeup
********************************************************************************
*
* Summary:
*  Restores the user configuration and enables the power to the block.
*  
* Parameters:  
*  void
*
* Return: 
*  void
*
* Global variables:
*  adc_backup:  The structure field 'enableState' is used to 
*  restore the enable state of block after wakeup from sleep mode.
*
*******************************************************************************/
void adc_Wakeup(void) 
{
    /* Restore the configuration */
    adc_RestoreConfig();

    /* Enable's the component operation */
    if(adc_backup.enableState == adc_ENABLED)
    {
        adc_Enable();

        /* Start the conversion only if conversion is not triggered by the hardware */
        if(!(adc_DEC_CR_REG & adc_DEC_XSTART_EN))
        {
            adc_StartConvert();
        }

    } /* Do nothing if component was disable before */
}


/* [] END OF FILE */
