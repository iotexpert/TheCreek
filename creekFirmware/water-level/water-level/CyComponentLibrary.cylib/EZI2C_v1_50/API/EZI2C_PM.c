/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the API for the proper switching to/from the low power
*  modes.
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

/*  Function Prototypes */
void    `$INSTANCE_NAME`_Enable(void);

/* PSoC3 ES2 or early, PSoC5 ES1*/
#if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
    
    static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup = 
    {
        /* Enable state - disabled */
        `$INSTANCE_NAME`_DISABLED,

        /* xcfg: enabled hardware addr detection */
        `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN,

        /* addr: default address (0x04) */
        `$INSTANCE_NAME`_DEFAULT_ADDR1,

        /* cfg: fast mode clock rate */
        `$INSTANCE_NAME`_CFG_CLK_RATE_400,

        /* clkDiv: set CLK divider to 16 */
        `$INSTANCE_NAME`_CLK_DIV_16
    };

/* PSoC3 ES3 or later, PSoC5 ES2 or later */
#elif (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)

    static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup = 
    {
        /* enable state - disabled */
        `$INSTANCE_NAME`_DISABLED,

        /* xcfg: wakeup disabled, enabled hardware addr detection */
        `$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN,

        /* addr: default address (0x04) */
        `$INSTANCE_NAME`_DEFAULT_ADDR1,

        /* cfg: default bus speed - 100kHz, so write 0 (16 samples/bit) */
        0x00u,

        /* clkDiv1 */
        LO8(BCLK__BUS_CLK__KHZ / `$INSTANCE_NAME`_BUS_SPEED),

        /* clkDiv2 */
        HI8(BCLK__BUS_CLK__KHZ / `$INSTANCE_NAME`_BUS_SPEED)
    };        

#endif /* End of (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the current user configuration of the EZI2C component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_backup - the non retention registers are saved to.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{
    `$INSTANCE_NAME`_backup.xcfg = `$INSTANCE_NAME`_XCFG_REG;
    `$INSTANCE_NAME`_backup.adr  = `$INSTANCE_NAME`_ADDR_REG;
    `$INSTANCE_NAME`_backup.cfg  = `$INSTANCE_NAME`_CFG_REG;

    /* PSoC3 ES2 or early, PSoC5 ES1*/
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)

        `$INSTANCE_NAME`_backup.clkDiv  = `$INSTANCE_NAME`_CLKDIV_REG;

    /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
    #elif(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)

        `$INSTANCE_NAME`_backup.clkDiv1  = `$INSTANCE_NAME`_CLKDIV1_REG;
        `$INSTANCE_NAME`_backup.clkDiv2  = `$INSTANCE_NAME`_CLKDIV2_REG;

    #endif  /* End of (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores the previously saved by `$INSTANCE_NAME`_SaveConfig() or 
*  `$INSTANCE_NAME`_Sleep() configuration of the EZI2C component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_backup - the non retention registers are restored from.
*
* Reentrant:
*  No.
*
* Side Effects:
*  Calling this function before `$INSTANCE_NAME`_SaveConfig() or
*  `$INSTANCE_NAME`_Sleep() will lead to unpredictable results.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void)
{
    `$INSTANCE_NAME`_XCFG_REG = `$INSTANCE_NAME`_backup.xcfg;
    `$INSTANCE_NAME`_ADDR_REG = `$INSTANCE_NAME`_backup.adr;
    
    /* There are master's configuration bits here */
    `$INSTANCE_NAME`_CFG_REG |= (`$INSTANCE_NAME`_backup.cfg & `$INSTANCE_NAME`_I2C_MASTER_MASK);    

    /* PSoC3 ES2 or early, PSoC5 ES1*/
    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)

        `$INSTANCE_NAME`_CLKDIV_REG =`$INSTANCE_NAME`_backup.clkDiv;

    /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
    #elif(`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)

        `$INSTANCE_NAME`_CLKDIV1_REG = `$INSTANCE_NAME`_backup.clkDiv1;
        `$INSTANCE_NAME`_CLKDIV2_REG = `$INSTANCE_NAME`_backup.clkDiv2;

    #endif  /* End of (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Saves component enable state and configuration. Stops component operation.
*  Should be called just prior to entering sleep. If "Enable wakeup from the
*  Sleep mode" is properly configured and enabled, this function should not be
*  called.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_backup - the non retention registers are saved to. Changed
*  by `$INSTANCE_NAME`_SaveConfig() function call.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    if(`$INSTANCE_NAME`_IS_BIT_SET(`$INSTANCE_NAME`_PM_ACT_CFG_REG, `$INSTANCE_NAME`_ACT_PWR_EN))
    {
        `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_ENABLED;
    }
    else /* The I2C block's slave is disabled */
    {
        `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_DISABLED;
    }
    
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
*  Restores component enable state and configuration. Should be called
*  just after awaking from sleep.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_backup - the non retention registers are restored from.
*
* Reentrant:
*  No.
*
* Side Effects:
*  Calling this function before `$INSTANCE_NAME`_SaveConfig() or
*  `$INSTANCE_NAME`_Sleep() will lead to unpredictable results.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void)
{
    /* Restore registers values */
    `$INSTANCE_NAME`_RestoreConfig();
    
    if(`$INSTANCE_NAME`_ENABLED == `$INSTANCE_NAME`_backup.enableState)
    {
        /* Enable component's operation */
        `$INSTANCE_NAME`_Enable();

    } /* Do nothing if component's block was disabled before */
}


/* [] END OF FILE */
