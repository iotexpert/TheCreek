/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides Low power mode APIs for I2C component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
    extern void `$INSTANCE_NAME`_TimeoutEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_TimeoutEnable")`;
    extern void `$INSTANCE_NAME`_TimeoutStop(void) `=ReentrantKeil($INSTANCE_NAME . "_TimeoutStop")`;
    extern void `$INSTANCE_NAME`_TimeoutSaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_TimeoutSaveConfig")`;
    extern void `$INSTANCE_NAME`_TimeoutRestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_TimeoutRestoreConfig")`;
#endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */


`$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup =
{
    0u, /* enableState */

    #if(`$INSTANCE_NAME`_FF_IMPLEMENTED)
        `$INSTANCE_NAME`_DEFAULT_XCFG,  /* xcfg */
        `$INSTANCE_NAME`_DEFAULT_CFG,   /* cfg  */

        #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
            `$INSTANCE_NAME`_DEFAULT_ADDR, /* addr */
        #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

        #if(CY_PSOC5A)
           `$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR,      /* div */
        #else
            LO8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR),/* div1 */
            HI8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR),/* div2 */
        #endif  /* End (CY_PSOC5A) */

    #else  /* (`$INSTANCE_NAME`_UDB_IMPLEMENTED) */
        `$INSTANCE_NAME`_DEFAULT_CFG,    /* control */

        #if(CY_UDB_V0)
            `$INSTANCE_NAME`_INT_ENABLE_MASK, /* aux_ctl */

            #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
                `$INSTANCE_NAME`_DEFAULT_ADDR, /* addr_d0 */
            #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */
        #endif  /* End (CY_UDB_V0) */
    #endif  /* End (`$INSTANCE_NAME`_FF_IMPLEMENTED) */

    #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
        `$INSTANCE_NAME`_DEFAULT_TMOUT_PERIOD,
        `$INSTANCE_NAME`_DEFAULT_TMOUT_INTR_MASK,

        #if(`$INSTANCE_NAME`_TIMEOUT_PRESCALER_ENABLED && CY_UDB_V0)
            `$INSTANCE_NAME`_DEFAULT_TMOUT_PRESCALER_PRD,
        #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_PRESCALER_ENABLED) */

    #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */
};

#if((`$INSTANCE_NAME`_FF_IMPLEMENTED) && (`$INSTANCE_NAME`_ENABLE_WAKEUP))
    volatile uint8 `$INSTANCE_NAME`_wakeupSource;
#endif  /* End ((`$INSTANCE_NAME`_FF_IMPLEMENTED) && (`$INSTANCE_NAME`_ENABLE_WAKEUP)) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Wakeup on address match enabled: disables I2C Master(if was enabled before go
*  to sleep), enables I2C backup regulator. Waits while on-going transaction be
*  will completed and I2C will be ready go to sleep. All incoming transaction
*  will be NACKed till power down will be asserted. The address match event
*  wakes up the chip.
*  Wakeup on address match disabled: saves I2C configuration and non-retention
*  register values.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used to save component configuration and none-retention
*  registers before enter sleep mode.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    #if(`$INSTANCE_NAME`_FF_IMPLEMENTED)
        #if(`$INSTANCE_NAME`_ENABLE_WAKEUP)
            uint8 enableInterrupts;
        #endif  /* End (`$INSTANCE_NAME`_ENABLE_WAKEUP) */

        /* Store regiters in either Sleep mode */
        `$INSTANCE_NAME`_backup.cfg  = `$INSTANCE_NAME`_CFG_REG;
        `$INSTANCE_NAME`_backup.xcfg = `$INSTANCE_NAME`_XCFG_REG;

        #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
            `$INSTANCE_NAME`_backup.addr = `$INSTANCE_NAME`_ADDR_REG;
        #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

        #if(CY_PSOC5A)
            `$INSTANCE_NAME`_backup.clkDiv   = `$INSTANCE_NAME`_CLKDIV_REG;
        #else
            `$INSTANCE_NAME`_backup.clkDiv1  = `$INSTANCE_NAME`_CLKDIV1_REG;
            `$INSTANCE_NAME`_backup.clkDiv2  = `$INSTANCE_NAME`_CLKDIV2_REG;
        #endif  /* End (CY_PSOC5A) */

        #if(`$INSTANCE_NAME`_ENABLE_WAKEUP)
            /* Need to disable Master */
            `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_ENABLE_MASTER;

            /* Enable the I2C regulator backup */
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_PWRSYS_CR1_REG |= `$INSTANCE_NAME`_PWRSYS_CR1_I2C_REG_BACKUP;
            CyExitCriticalSection(enableInterrupts);

            /* 1) Set force NACK to ignore I2C transactions
               2) Wait while I2C will be ready go to Sleep
               3) These bits are cleared on wake up */
            `$INSTANCE_NAME`_XCFG_REG |= `$INSTANCE_NAME`_XCFG_FORCE_NACK;
            while(0u == (`$INSTANCE_NAME`_XCFG_REG  & `$INSTANCE_NAME`_XCFG_RDY_TO_SLEEP));

            /* Set-up wakeup interrupt */
            enableInterrupts = CyEnterCriticalSection();
            CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_WAKEUP_ISR);
            `$INSTANCE_NAME`_wakeupSource = 0u;
            CyExitCriticalSection(enableInterrupts);

        #endif  /* End (`$INSTANCE_NAME`_ENABLE_WAKEUP) */

    #else
        /* Store only address match bit */
        `$INSTANCE_NAME`_backup.control = (`$INSTANCE_NAME`_CFG_REG & `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK);

        #if(CY_UDB_V0)
            /* Store interrupt mask bits */
            `$INSTANCE_NAME`_backup.intMask = `$INSTANCE_NAME`_INT_MASK_REG;

            #if(`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                /* Store slave address */
                `$INSTANCE_NAME`_backup.addr = `$INSTANCE_NAME`_ADDR_REG;
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */

        #endif  /* End (CY_UDB_V0) */

    #endif  /* End (`$INSTANCE_NAME`_FF_IMPLEMENTED) */

    #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
        `$INSTANCE_NAME`_TimeoutSaveConfig();   /* Save Timeout config */
    #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Wakeup on address match enabled: All incoming transaction will be NACKed till
*  power down will be asserted. The address match event wakes up the chip.
*  Wakeup on address match disabled: Disables active mode power template bits or
*  clock gating as appropriate. Saves I2C configuration and non-retention
*  register values.
*  Disables I2C interrupt.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`
{
    #if(`$INSTANCE_NAME`_ENABLE_WAKEUP)
        /* The I2C block should be always enabled if used as wakeup source */
        `$INSTANCE_NAME`_backup.enableState = 0u;

        #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
            `$INSTANCE_NAME`_TimeoutStop();
        #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */

    #else

        `$INSTANCE_NAME`_backup.enableState = `$INSTANCE_NAME`_IS_I2C_ENABLED;  /* Store I2C enable state */
        if(`$INSTANCE_NAME`_IS_I2C_ENABLED)
        {
            `$INSTANCE_NAME`_Stop();
        }
    #endif  /* End (`$INSTANCE_NAME`_ENABLE_WAKEUP) */

    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Wakeup on address match enabled: enables I2C Master (if was enabled before go
*  to sleep), disables I2C backup regulator.
*  Wakeup on address match disabled: Restores I2C configuration and
*  non-retention register values.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used to save component configuration and
*  none-retention registers before exit sleep mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    #if(`$INSTANCE_NAME`_FF_IMPLEMENTED)
            uint8 enableInterrupts;

            /* Disable the I2C regulator backup */
            if(0u != (`$INSTANCE_NAME`_PWRSYS_CR1_I2C_REG_BACKUP & `$INSTANCE_NAME`_PWRSYS_CR1_REG))
            {
                enableInterrupts = CyEnterCriticalSection();
                `$INSTANCE_NAME`_PWRSYS_CR1_REG &= ~`$INSTANCE_NAME`_PWRSYS_CR1_I2C_REG_BACKUP;
                CyExitCriticalSection(enableInterrupts);

                /* Re-enable Master */
                `$INSTANCE_NAME`_CFG_REG = `$INSTANCE_NAME`_backup.cfg;
            }
            else /* The I2C_REG_BACKUP was cleaned by PM API: it means Hibernate or wake-up not set */
            {
                #if(`$INSTANCE_NAME`_ENABLE_WAKEUP)
                    /* Disable power to I2C block before register restore */
                    enableInterrupts = CyEnterCriticalSection();
                    `$INSTANCE_NAME`_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
                    `$INSTANCE_NAME`_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
                    CyExitCriticalSection(enableInterrupts);

                    /* Enable component after restore complete */
                    `$INSTANCE_NAME`_backup.enableState = 1u;
                #endif  /* End (`$INSTANCE_NAME`_ENABLE_WAKEUP) */

                /* Restore component registers: Hibernate disable power */
                `$INSTANCE_NAME`_XCFG_REG = `$INSTANCE_NAME`_backup.xcfg;
                `$INSTANCE_NAME`_CFG_REG  = `$INSTANCE_NAME`_backup.cfg;

                #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
                    `$INSTANCE_NAME`_ADDR_REG = `$INSTANCE_NAME`_backup.addr;
                #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

                #if(CY_PSOC5A)
                    `$INSTANCE_NAME`_CLKDIV_REG  = `$INSTANCE_NAME`_backup.clkDiv;
                #else
                    `$INSTANCE_NAME`_CLKDIV1_REG = `$INSTANCE_NAME`_backup.clkDiv1;
                    `$INSTANCE_NAME`_CLKDIV2_REG = `$INSTANCE_NAME`_backup.clkDiv2;
                #endif  /* End (CY_PSOC5A) */
            }

            #if(`$INSTANCE_NAME`_ENABLE_WAKEUP)
                /* Trigger I2C interrupt if it was wakeup source */
                enableInterrupts = CyEnterCriticalSection();
                CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);
                if(0u != `$INSTANCE_NAME`_wakeupSource)
                {
                    `$INSTANCE_NAME`_SetPendingInt();   /* Generate interrupt to unlock bus */
                }
                CyExitCriticalSection(enableInterrupts);
            #endif  /* End (`$INSTANCE_NAME`_ENABLE_WAKEUP) */

    #else

        #if(CY_UDB_V0)
            uint8 enableInterrupts;

            `$INSTANCE_NAME`_INT_MASK_REG |= `$INSTANCE_NAME`_backup.intMask;

            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_INT_ENABLE_REG |= `$INSTANCE_NAME`_INT_ENABLE_MASK;
            CyExitCriticalSection(enableInterrupts);

            #if(`$INSTANCE_NAME`_MODE_MASTER_ENABLED)
                /* Restore Master Clock generator */
                `$INSTANCE_NAME`_MCLK_PRD_REG = `$INSTANCE_NAME`_DEFAULT_MCLK_PRD;
                `$INSTANCE_NAME`_MCLK_CMP_REG = `$INSTANCE_NAME`_DEFAULT_MCLK_CMP;
            #endif /* End (`$INSTANCE_NAME`_MODE_MASTER_ENABLED) */

            #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
                /* Restore slave address */
                `$INSTANCE_NAME`_ADDR_REG = `$INSTANCE_NAME`_backup.addr;

                /* Restore slave bit counter period */
                `$INSTANCE_NAME`_PERIOD_REG = `$INSTANCE_NAME`_DEFAULT_PERIOD;
            #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

        #endif  /* End (CY_UDB_V0) */

        /* Restore CFG register */
        `$INSTANCE_NAME`_CFG_REG = `$INSTANCE_NAME`_backup.control;

    #endif  /* End (`$INSTANCE_NAME`_FF_IMPLEMENTED) */

    #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
        `$INSTANCE_NAME`_TimeoutRestoreConfig();
    #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Wakeup on address match enabled: enables I2C Master (if was enabled before go
*  to sleep) and disables I2C backup regulator.
*  Wakeup on address match disabled: Restores I2C configuration and non-retention
*  register values. Restores Active mode power template bits or clock gating as
*  appropriate.
*  The I2C interrupt remains disabled after function call.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();   /* Restore I2C register settings */

    /* Restore I2C Enable state */
    if(0u != `$INSTANCE_NAME`_backup.enableState)
    {
        `$INSTANCE_NAME`_Enable();
        `$INSTANCE_NAME`_EnableInt();
    }
    else
    {
        #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
            `$INSTANCE_NAME`_TimeoutEnable();
        #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */
    }
}


/* [] END OF FILE */
