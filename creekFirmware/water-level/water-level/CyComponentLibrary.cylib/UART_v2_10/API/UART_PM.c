/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides Sleep/WakeUp APIs functionality.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"


/***************************************
* Local data allocation
***************************************/

static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup =
{
    /* enableState - disabled */
    0u,
};        



/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves the current user configuration.
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
    /* PSoC3 ES2 or early, PSoC5 ES1*/
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)

        #if(`$INSTANCE_NAME`_CONTROL_REG_REMOVED == 0u)
            `$INSTANCE_NAME`_backup.cr = `$INSTANCE_NAME`_CONTROL_REG;
        #endif /* End `$INSTANCE_NAME`_CONTROL_REG_REMOVED */

        #if( (`$INSTANCE_NAME`_RX_ENABLED) || (`$INSTANCE_NAME`_HD_ENABLED) )
            `$INSTANCE_NAME`_backup.rx_period = `$INSTANCE_NAME`_RXBITCTR_PERIOD_REG;
            `$INSTANCE_NAME`_backup.rx_mask = `$INSTANCE_NAME`_RXSTATUS_MASK_REG;
            #if (`$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED)
                `$INSTANCE_NAME`_backup.rx_addr1 = `$INSTANCE_NAME`_RXADDRESS1_REG;
                `$INSTANCE_NAME`_backup.rx_addr2 = `$INSTANCE_NAME`_RXADDRESS2_REG;
            #endif /* End `$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED */
        #endif /* End `$INSTANCE_NAME`_RX_ENABLED | `$INSTANCE_NAME`_HD_ENABLED*/

        #if(`$INSTANCE_NAME`_TX_ENABLED)
            #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
                `$INSTANCE_NAME`_backup.tx_clk_ctr = `$INSTANCE_NAME`_TXBITCLKGEN_CTR_REG;
                `$INSTANCE_NAME`_backup.tx_clk_compl = `$INSTANCE_NAME`_TXBITCLKTX_COMPLETE_REG;
            #else
                `$INSTANCE_NAME`_backup.tx_period = `$INSTANCE_NAME`_TXBITCTR_PERIOD_REG;
            #endif /*End `$INSTANCE_NAME`_TXCLKGEN_DP */
            `$INSTANCE_NAME`_backup.tx_mask = `$INSTANCE_NAME`_TXSTATUS_MASK_REG;
        #endif /*End `$INSTANCE_NAME`_TX_ENABLED */

    /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
    #elif (CY_PSOC3_ES3 || CY_PSOC5_ES2)

        #if(`$INSTANCE_NAME`_CONTROL_REG_REMOVED == 0u)
            `$INSTANCE_NAME`_backup.cr = `$INSTANCE_NAME`_CONTROL_REG;
        #endif /* End `$INSTANCE_NAME`_CONTROL_REG_REMOVED */

    #endif  /* End PSOC3_ES3 || PSOC5_ES2 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores the current user configuration.
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
void `$INSTANCE_NAME`_RestoreConfig(void)
{
    /* PSoC3 ES2 or early, PSoC5 ES1*/
    #if (CY_PSOC3_ES2 || CY_PSOC5_ES1)

        #if(`$INSTANCE_NAME`_CONTROL_REG_REMOVED == 0u)
            `$INSTANCE_NAME`_CONTROL_REG = `$INSTANCE_NAME`_backup.cr;
        #endif /* End `$INSTANCE_NAME`_CONTROL_REG_REMOVED */

        #if( (`$INSTANCE_NAME`_RX_ENABLED) || (`$INSTANCE_NAME`_HD_ENABLED) )
            `$INSTANCE_NAME`_RXBITCTR_PERIOD_REG = `$INSTANCE_NAME`_backup.rx_period;
            `$INSTANCE_NAME`_RXSTATUS_MASK_REG = `$INSTANCE_NAME`_backup.rx_mask;
            #if (`$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED)
                `$INSTANCE_NAME`_RXADDRESS1_REG = `$INSTANCE_NAME`_backup.rx_addr1;
                `$INSTANCE_NAME`_RXADDRESS2_REG = `$INSTANCE_NAME`_backup.rx_addr2;
            #endif /* End `$INSTANCE_NAME`_RXHW_ADDRESS_ENABLED */
        #endif  /* End (`$INSTANCE_NAME`_RX_ENABLED) || (`$INSTANCE_NAME`_HD_ENABLED) */

        #if(`$INSTANCE_NAME`_TX_ENABLED)
            #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
                `$INSTANCE_NAME`_TXBITCLKGEN_CTR_REG = `$INSTANCE_NAME`_backup.tx_clk_ctr;
                `$INSTANCE_NAME`_TXBITCLKTX_COMPLETE_REG = `$INSTANCE_NAME`_backup.tx_clk_compl;
            #else
                `$INSTANCE_NAME`_TXBITCTR_PERIOD_REG = `$INSTANCE_NAME`_backup.tx_period;
            #endif /*End `$INSTANCE_NAME`_TXCLKGEN_DP */
            `$INSTANCE_NAME`_TXSTATUS_MASK_REG = `$INSTANCE_NAME`_backup.tx_mask;
        #endif /*End `$INSTANCE_NAME`_TX_ENABLED */

    /* PSoC3 ES3 or later, PSoC5 ES2 or later*/
    #elif (CY_PSOC3_ES3 || CY_PSOC5_ES2)

        #if(`$INSTANCE_NAME`_CONTROL_REG_REMOVED == 0u)
            `$INSTANCE_NAME`_CONTROL_REG = `$INSTANCE_NAME`_backup.cr;
        #endif /* End `$INSTANCE_NAME`_CONTROL_REG_REMOVED */

    #endif  /* End PSOC3_ES3 || PSOC5_ES2 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Stops and saves the user configuration. Should be called 
*  just prior to entering sleep.
*  
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

    #if(`$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED)
        if((`$INSTANCE_NAME`_RXSTATUS_ACTL_REG  & `$INSTANCE_NAME`_INT_ENABLE) != 0u) 
        {
            `$INSTANCE_NAME`_backup.enableState = 1u;
        }
        else
        {
            `$INSTANCE_NAME`_backup.enableState = 0u;
        }
    #else
        if((`$INSTANCE_NAME`_TXSTATUS_ACTL_REG  & `$INSTANCE_NAME`_INT_ENABLE) !=0u)
        {
            `$INSTANCE_NAME`_backup.enableState = 1u;
        }
        else
        {
            `$INSTANCE_NAME`_backup.enableState = 0u;
        }
    #endif /* End `$INSTANCE_NAME`_RX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED*/

    `$INSTANCE_NAME`_Stop();
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Restores and enables the user configuration. Should be called
*  just after awaking from sleep.
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
void `$INSTANCE_NAME`_Wakeup(void)
{
    `$INSTANCE_NAME`_RestoreConfig();
    #if( (`$INSTANCE_NAME`_RX_ENABLED) || (`$INSTANCE_NAME`_HD_ENABLED) )
        `$INSTANCE_NAME`_ClearRxBuffer();
    #endif /* End (`$INSTANCE_NAME`_RX_ENABLED) || (`$INSTANCE_NAME`_HD_ENABLED) */
    #if(`$INSTANCE_NAME`_TX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED)
        `$INSTANCE_NAME`_ClearTxBuffer();
    #endif /* End `$INSTANCE_NAME`_TX_ENABLED || `$INSTANCE_NAME`_HD_ENABLED */

    if(`$INSTANCE_NAME`_backup.enableState != 0u)
    {
        `$INSTANCE_NAME`_Enable();
    } 
}


/* [] END OF FILE */
