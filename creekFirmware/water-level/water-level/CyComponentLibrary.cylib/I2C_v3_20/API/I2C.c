/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of APIs for the I2C component.
*  Actual protocol and operation code resides in the interrupt service routine
*  file.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/**********************************
*      System variables
**********************************/

uint8 `$INSTANCE_NAME`_initVar = 0u;    /* Defines if component was initialized */

volatile uint8 `$INSTANCE_NAME`_state;  /* Current state of I2C FSM */

extern `$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup;

#if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
    extern void `$INSTANCE_NAME`_TimeoutInit(void)   `=ReentrantKeil($INSTANCE_NAME . "_TimeoutInit")`;
    extern void `$INSTANCE_NAME`_TimeoutEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_TimeoutEnable")`;
    extern void `$INSTANCE_NAME`_TimeoutStop(void)   `=ReentrantKeil($INSTANCE_NAME . "_TimeoutStop")`;
#endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes I2C registers with initial values provided from customizer.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    #if(`$INSTANCE_NAME`_FF_IMPLEMENTED)
        `$INSTANCE_NAME`_CFG_REG  = `$INSTANCE_NAME`_DEFAULT_CFG;
        `$INSTANCE_NAME`_XCFG_REG = `$INSTANCE_NAME`_DEFAULT_XCFG;

        #if(CY_PSOC5A)
            `$INSTANCE_NAME`_CLKDIV_REG  = `$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR;
        #else
            `$INSTANCE_NAME`_CLKDIV1_REG = LO8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR);
            `$INSTANCE_NAME`_CLKDIV2_REG = HI8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR);
        #endif /* End (CY_PSOC5A) */

    #else
        uint8 enableInterrupts;

        `$INSTANCE_NAME`_CFG_REG      = `$INSTANCE_NAME`_DEFAULT_CFG;      /* control  */
        `$INSTANCE_NAME`_INT_MASK_REG = `$INSTANCE_NAME`_DEFAULT_INT_MASK; /* int_mask */

        /* Enable interrupts from block */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_INT_ENABLE_REG |= `$INSTANCE_NAME`_INTR_ENABLE; /* aux_ctl */
        CyExitCriticalSection(enableInterrupts);

        #if(`$INSTANCE_NAME`_MODE_MASTER_ENABLED)
            `$INSTANCE_NAME`_MCLK_PRD_REG = `$INSTANCE_NAME`_DEFAULT_MCLK_PRD;
            `$INSTANCE_NAME`_MCLK_CMP_REG = `$INSTANCE_NAME`_DEFAULT_MCLK_CMP;
         #endif /* End (`$INSTANCE_NAME`_MODE_MASTER_ENABLED) */

        #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
            `$INSTANCE_NAME`_PERIOD_REG = `$INSTANCE_NAME`_DEFAULT_PERIOD;
        #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

    #endif  /* End (`$INSTANCE_NAME`_FF_IMPLEMENTED) */

    #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
        `$INSTANCE_NAME`_TimeoutInit();
    #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */

    /* Disable Interrupt and set vector and priority */
    CyIntDisable    (`$INSTANCE_NAME`_ISR_NUMBER);
    CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);
    #if(`$INSTANCE_NAME`_INTERN_I2C_INTR_HANDLER)
        CyIntSetVector  (`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);
    #endif  /* End (`$INSTANCE_NAME`_INTERN_I2C_INTR_HANDLER) */


    /* Put state machine in idle state */
    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;

    #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
        /* Reset status and buffers index */
        `$INSTANCE_NAME`_SlaveClearReadBuf();
        `$INSTANCE_NAME`_SlaveClearWriteBuf();
        `$INSTANCE_NAME`_SlaveClearReadStatus();
        `$INSTANCE_NAME`_SlaveClearWriteStatus();

        /* Set default address */
        `$INSTANCE_NAME`_SlaveSetAddress(`$INSTANCE_NAME`_DEFAULT_ADDR);
    #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

    #if(`$INSTANCE_NAME`_MODE_MASTER_ENABLED)
        /* Reset status and buffers index */
        `$INSTANCE_NAME`_MasterClearReadBuf();
        `$INSTANCE_NAME`_MasterClearWriteBuf();
        `$INSTANCE_NAME`_MasterClearStatus();
    #endif  /* End (`$INSTANCE_NAME`_MODE_MASTER_ENABLED) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables I2C operations.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    #if(`$INSTANCE_NAME`_FF_IMPLEMENTED)
        uint8 enableInterrupts;

        /* Enable power to I2C FF block */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_ACT_PWR_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_STBY_PWR_EN;
        CyExitCriticalSection(enableInterrupts);

    #else

        #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
            uint8 enableInterrupts;

            /* Enable slave bit counter */
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_COUNTER_AUX_CTL_REG |= `$INSTANCE_NAME`_CNT7_ENABLE;   /* aux_ctl */
            CyExitCriticalSection(enableInterrupts);
        #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

        `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_ENABLE_MS;

    #endif  /* End (`$INSTANCE_NAME`_FF_IMPLEMENTED) */

    #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
        `$INSTANCE_NAME`_TimeoutEnable();
    #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the I2C hardware. Enables Active mode power template bits or clock
*  gating as appropriate. It is required to be executed before I2C bus operation.
*  The I2C interrupt remains disabled after this function call.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  This component automatically enables it's interrupt.  If I2C is enabled
*  without the interrupt enabled, it could lock up the I2C bus.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified
*  on first function call.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    /* Initialize I2C registers, reset I2C buffer index and clears status */
    if(0u == `$INSTANCE_NAME`_initVar)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }

    `$INSTANCE_NAME`_Enable();
    `$INSTANCE_NAME`_EnableInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables I2C hardware and disables I2C interrupt. Disables Active mode power
*  template bits or clock gating as appropriate.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    uint8 enableInterrupts;

    `$INSTANCE_NAME`_DisableInt();

    `$INSTANCE_NAME`_DISABLE_INT_ON_STOP;         /* Interrupt on Stop is enabled by write */
    enableInterrupts = `$INSTANCE_NAME`_CSR_REG;  /* Clear CSR reg */

    #if(`$INSTANCE_NAME`_TIMEOUT_ENABLED)
        `$INSTANCE_NAME`_TimeoutStop();
    #endif  /* End (`$INSTANCE_NAME`_TIMEOUT_ENABLED) */

    #if(`$INSTANCE_NAME`_FF_IMPLEMENTED)
        #if(CY_PSOC3 || CY_PSOC5LP)
            /* Store resgisters which are held in reset when Master and Slave are disabled */
            #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
                `$INSTANCE_NAME`_backup.addr = `$INSTANCE_NAME`_ADDR_REG;
            #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

            `$INSTANCE_NAME`_backup.clkDiv1  = `$INSTANCE_NAME`_CLKDIV1_REG;
            `$INSTANCE_NAME`_backup.clkDiv2  = `$INSTANCE_NAME`_CLKDIV2_REG;


            /* Reset FF block */
            `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_ENABLE_MS;
            CyDelayUs(2);
            `$INSTANCE_NAME`_CFG_REG |=  `$INSTANCE_NAME`_ENABLE_MS;


            /* Restore registers */
            #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
                `$INSTANCE_NAME`_ADDR_REG = `$INSTANCE_NAME`_backup.addr;
            #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

            `$INSTANCE_NAME`_CLKDIV1_REG = `$INSTANCE_NAME`_backup.clkDiv1;
            `$INSTANCE_NAME`_CLKDIV2_REG = `$INSTANCE_NAME`_backup.clkDiv2;

        #endif  /* End (CY_PSOC3 || CY_PSOC5LP) */

        /* Disable power to I2C block */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
        `$INSTANCE_NAME`_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
        CyExitCriticalSection(enableInterrupts);

    #else

        #if(`$INSTANCE_NAME`_MODE_SLAVE_ENABLED)
            /* Disable slave bit counter */
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_COUNTER_AUX_CTL_REG &= ~`$INSTANCE_NAME`_CNT7_ENABLE;
            CyExitCriticalSection(enableInterrupts);
        #endif  /* End (`$INSTANCE_NAME`_MODE_SLAVE_ENABLED) */

        `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_ENABLE_MS;    /* Enable bits clear resets HW */

    #endif  /* End (`$INSTANCE_NAME`_FF_IMPLEMENTED) */

    `$INSTANCE_NAME`_ClearPendingInt();  /* Clear possible interrupt triggers on reset */
        
    `$INSTANCE_NAME`_state = `$INSTANCE_NAME`_SM_IDLE;  /* Reset software FSM */
}


/* [] END OF FILE */
