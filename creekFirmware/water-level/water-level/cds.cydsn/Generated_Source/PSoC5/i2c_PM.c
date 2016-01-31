/*******************************************************************************
* File Name: i2c_PM.c
* Version 1.70
*
* Description:
*  This file contains the API for the proper switching to/from the low power
*  modes.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "i2c.h"

/*  Function Prototypes */
void    i2c_Enable(void);

#if(CY_PSOC5A)
    
    i2c_BACKUP_STRUCT  i2c_backup =
    {
        /* Enable state - disabled */
        i2c_DISABLED,

        /* xcfg: enabled hardware addr detection */
        i2c_XCFG_HDWR_ADDR_EN,

        /* addr: default address (0x04) */
        i2c_DEFAULT_ADDR1,

        /* cfg: fast mode clock rate */
        i2c_CFG_CLK_RATE_400,

        /* clkDiv: set CLK divider to 16 */
        i2c_CLK_DIV_16
    };

#else

    i2c_BACKUP_STRUCT  i2c_backup =
    {
        /* enable state - disabled */
        i2c_DISABLED,

        /* xcfg: wakeup disabled, enabled hardware addr detection */
        i2c_XCFG_HDWR_ADDR_EN,

        /* addr: default address (0x04) */
        i2c_DEFAULT_ADDR1,

        /* cfg: default bus speed - 100kHz, so write 0 (16 samples/bit) */
        0x00u,

        /* clkDiv1 */
        LO8(BCLK__BUS_CLK__KHZ / i2c_BUS_SPEED),

        /* clkDiv2 */
        HI8(BCLK__BUS_CLK__KHZ / i2c_BUS_SPEED)
    };        

#endif /* (CY_PSOC5A) */


/*******************************************************************************
* Function Name: i2c_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the current user configuration of the EZI2C component.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_backup - the non retention registers are saved to.
*
* Reentrant:
*  No
*
*******************************************************************************/
void i2c_SaveConfig(void) 
{
    i2c_backup.xcfg = i2c_XCFG_REG;
    i2c_backup.adr  = i2c_ADDR_REG;
    i2c_backup.cfg  = i2c_CFG_REG;

    #if(CY_PSOC5A)
        i2c_backup.clkDiv  = i2c_CLKDIV_REG;
    #else
        i2c_backup.clkDiv1  = i2c_CLKDIV1_REG;
        i2c_backup.clkDiv2  = i2c_CLKDIV2_REG;
    #endif  /* (CY_PSOC5A) */
}


/*******************************************************************************
* Function Name: i2c_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores the previously saved by i2c_SaveConfig() or 
*  i2c_Sleep() configuration of the EZI2C component.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_backup - the non retention registers are restored from.
*
* Reentrant:
*  No
*
* Side Effects:
*  Calling this function before i2c_SaveConfig() or
*  i2c_Sleep() will lead to unpredictable results.
*
*******************************************************************************/
void i2c_RestoreConfig(void) 
{
    /* There are master's configuration bits here */
    i2c_CFG_REG |= (i2c_backup.cfg & i2c_I2C_MASTER_MASK);    
    
    i2c_XCFG_REG = i2c_backup.xcfg;
    i2c_ADDR_REG = i2c_backup.adr;

    #if(CY_PSOC5A)
        i2c_CLKDIV_REG =i2c_backup.clkDiv;
    #else
        i2c_CLKDIV1_REG = i2c_backup.clkDiv1;
        i2c_CLKDIV2_REG = i2c_backup.clkDiv2;
    #endif  /* (CY_PSOC5A) */
}


/*******************************************************************************
* Function Name: i2c_Sleep
********************************************************************************
*
* Summary:
*  Saves component enable state and configuration. Stops component operation.
*  Should be called just prior to entering sleep. If "Enable wakeup from the
*  Sleep mode" is properly configured and enabled, this function should not be
*  called.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_backup - the non retention registers are saved to. Changed
*  by i2c_SaveConfig() function call.
*
* Reentrant:
*  No
*
*******************************************************************************/
void i2c_Sleep(void) 
{
    if(i2c_IS_BIT_SET(i2c_PM_ACT_CFG_REG, i2c_ACT_PWR_EN))
    {
        i2c_backup.enableState = i2c_ENABLED;
    }
    else /* The I2C block's slave is disabled */
    {
        i2c_backup.enableState = i2c_DISABLED;
    }
    
    /* Stop component */
    i2c_Stop();

    /* Save registers configuration */
    i2c_SaveConfig();
}


/*******************************************************************************
* Function Name: i2c_Wakeup
********************************************************************************
*
* Summary:
*  Restores component enable state and configuration. Should be called
*  just after awaking from sleep.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  i2c_backup - the non retention registers are restored from.
*
* Reentrant:
*  No
*
* Side Effects:
*  Calling this function before i2c_SaveConfig() or
*  i2c_Sleep() will lead to unpredictable results.
*
*******************************************************************************/
void i2c_Wakeup(void) 
{
    /* Restore registers values */
    i2c_RestoreConfig();
    
    if(i2c_ENABLED == i2c_backup.enableState)
    {
        /* Enable component's operation */
        i2c_Enable();

    } /* Do nothing if component's block was disabled before */
}


/* [] END OF FILE */
