/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides Sleep APIs for I2C component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
    #define  `$INSTANCE_NAME`_DEFAULT_XCFG      (`$INSTANCE_NAME`_XCFG_HDWR_ADDR_EN | `$INSTANCE_NAME`_XCFG_CLK_EN)
#else
    #define  `$INSTANCE_NAME`_DEFAULT_XCFG      `$INSTANCE_NAME`_XCFG_CLK_EN
#endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE) */

#define `$INSTANCE_NAME`_DEFAULT_CFG     (`$INSTANCE_NAME`_ENABLE_SLAVE | `$INSTANCE_NAME`_ENABLE_MASTER | \
                                          `$INSTANCE_NAME`_DEFAULT_CLK_RATE)
                                                                                    
/* Define active state */
#if(`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
    #define `$INSTANCE_NAME`_I2C_ENABLE_REG     `$INSTANCE_NAME`_ACT_PWRMGR_REG
    #define `$INSTANCE_NAME`_IS_I2C_ENABLE(reg) ( ((reg) & `$INSTANCE_NAME`_ACT_PWR_EN) != 0u )
#else
    #define `$INSTANCE_NAME`_I2C_ENABLE_REG     `$INSTANCE_NAME`_CFG_REG
    #define `$INSTANCE_NAME`_IS_I2C_ENABLE(reg) ( (((reg) & `$INSTANCE_NAME`_ENABLE_MASTER) != 0u) ? 1u : \
                                                  (((reg)  & `$INSTANCE_NAME`_ENABLE_SLAVE) != 0u) ? 1u : 0u )
#endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)*/

`$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup =
{   
    0x00u, /* enableState; */
        
    #if ((`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) && (`$INSTANCE_NAME`_ENABLE_WAKEUP == 0u))
        `$INSTANCE_NAME`_DEFAULT_XCFG, /* xcfg */
        `$INSTANCE_NAME`_DEFAULT_CFG,  /* cfg */

        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            (`$INSTANCE_NAME`_DEFAULT_ADDR & `$INSTANCE_NAME`_SADDR_MASK), /* addr */
        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
        
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
           `$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR,
        #else
            LO8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR), /*  clk_div1 */
            HI8(`$INSTANCE_NAME`_DEFAULT_DIVIDE_FACTOR), /*  clk_div2 */
        #endif  /* End  (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
    #else
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_INT_ENABLE_MASK, /* int_mask */
            
            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                (`$INSTANCE_NAME`_DEFAULT_ADDR & `$INSTANCE_NAME`_SADDR_MASK), /* addr */
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
        #else
            /* ES3 Saves: 
                - Status Int mask: int_mask;
                - D0 register: addr;
                - Auxiliary Control: aux_ctl;
                - Period Register;
            */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)*/
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)*/
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Save I2C configuration.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_SaveConfig(void)
{     
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
       
        #if (`$INSTANCE_NAME`_ENABLE_WAKEUP)
            uint8 enableInterrupts;
    
            /* Need to disable Master */
            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
                `$INSTANCE_NAME`_CFG_REG &= ~`$INSTANCE_NAME`_CFG_EN_MSTR;
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)*/
            
            enableInterrupts = CyEnterCriticalSection();
            /* Enable the I2C regulator backup */
            `$INSTANCE_NAME`_PWRSYS_CR1_REG |= `$INSTANCE_NAME`_PWRSYS_CR1_I2C_REG_BACKUP;
            CyExitCriticalSection(enableInterrupts);
            
            /* 1) Set force NACK to ignore I2C transactions 
               2) Wait while I2C will be ready to go Sleep 
               3) This bits clears on wake up */
            `$INSTANCE_NAME`_XCFG_REG  |= `$INSTANCE_NAME`_XCFG_FORCE_NACK;
            while ((`$INSTANCE_NAME`_XCFG_REG  & `$INSTANCE_NAME`_XCFG_RDY_TO_SLEEP) == 0u);
                        
        #else
            `$INSTANCE_NAME`_backup.xcfg = `$INSTANCE_NAME`_XCFG_REG ;
            `$INSTANCE_NAME`_backup.cfg  = `$INSTANCE_NAME`_CFG_REG;
            
            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                `$INSTANCE_NAME`_backup.addr = `$INSTANCE_NAME`_ADDR_REG;
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
            
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_backup.clk_div  = `$INSTANCE_NAME`_CLKDIV_REG;
            
            #else
                `$INSTANCE_NAME`_backup.clk_div1  = `$INSTANCE_NAME`_CLKDIV1_REG;
                `$INSTANCE_NAME`_backup.clk_div2  = `$INSTANCE_NAME`_CLKDIV2_REG;
                    
            #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
            
        #endif  /* End ((`$INSTANCE_NAME`_ENABLE_WAKEUP) && (`$INSTANCE_NAME`_I2C_PAIR_SELECTED)) */
        
    #else        
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)

            `$INSTANCE_NAME`_backup.int_mask = `$INSTANCE_NAME`_INT_MASK_REG;
        
            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                `$INSTANCE_NAME`_backup.addr = `$INSTANCE_NAME`_ADDR_REG;
            
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
            
        #else
        
            /* Saves: 
                - Status Int mask: int_mask;
                - D0 register: addr;
                - Auxiliary Control: aux_ctl;
                - Period Register;
                - D0, D1 register: mclk_gen;  
            */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Stops I2C computation and saves CRC configuration.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    #if (`$INSTANCE_NAME`_ENABLE_WAKEUP)
        /* The I2C block should be always enabled if used as 
           wakeup source. */
    
    #else
        /* Store PRS enable state */
        if(`$INSTANCE_NAME`_IS_I2C_ENABLE(`$INSTANCE_NAME`_I2C_ENABLE_REG))
        {
            `$INSTANCE_NAME`_backup.enableState = 1u;
            `$INSTANCE_NAME`_Stop();
        }
        else
        {
            `$INSTANCE_NAME`_backup.enableState = 0u;
        }
    #endif  /* End  (`$INSTANCE_NAME`_ENABLE_WAKEUP) */
    
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restore I2C computation.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    #if (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF)
    
        #if ((`$INSTANCE_NAME`_ENABLE_WAKEUP) && (`$INSTANCE_NAME`_I2C_PAIR_SELECTED))
            uint8 enableInterrupts;
            
            enableInterrupts = CyEnterCriticalSection();
            /* Disable the I2C regulator backup */
            `$INSTANCE_NAME`_PWRSYS_CR1_REG &= ~`$INSTANCE_NAME`_PWRSYS_CR1_I2C_REG_BACKUP;
            CyExitCriticalSection(enableInterrupts);
            
            /* Need to re-enable Master */
            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
                `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CFG_EN_MSTR;
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)*/
            
        #else
            `$INSTANCE_NAME`_XCFG_REG = `$INSTANCE_NAME`_backup.xcfg;
            `$INSTANCE_NAME`_CFG_REG = `$INSTANCE_NAME`_backup.cfg;
        
            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                `$INSTANCE_NAME`_ADDR_REG = `$INSTANCE_NAME`_backup.addr;
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */ 
                
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_CLKDIV_REG =`$INSTANCE_NAME`_backup.clk_div;
            
            #else   
                `$INSTANCE_NAME`_CLKDIV1_REG = `$INSTANCE_NAME`_backup.clk_div1;
                `$INSTANCE_NAME`_CLKDIV2_REG = `$INSTANCE_NAME`_backup.clk_div2;
                    
            #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
        #endif  /* End (`$INSTANCE_NAME`_ENABLE_WAKEUP == 0u) */
        
    #else
        
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_INT_MASK_REG = `$INSTANCE_NAME`_backup.int_mask;
        
            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER)
            
                /* Restore Master Clock generator */
                `$INSTANCE_NAME`_MCLK_PRD_REG = `$INSTANCE_NAME`_MCLK_PERIOD_VALUE;
                `$INSTANCE_NAME`_MCLK_CMP_REG = `$INSTANCE_NAME`_MCLK_COMPARE_VALUE;
           
            #endif /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_MASTER) */

            #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
                `$INSTANCE_NAME`_ADDR_REG = `$INSTANCE_NAME`_backup.addr;
                
                /* Restore Slave bit counter */
                `$INSTANCE_NAME`_PERIOD_REG = `$INSTANCE_NAME`_PERIOD_VALUE;
            
            #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
            
        #else
            /* Saves: 
                - Status Int mask: int_mask;
                - D0 register: addr;
                - Auxiliary Control: aux_ctl;
                - Period Register: always 7;
            */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
        /* Restore Control register */
        #if (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE)
            #if (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE)
                /* Turn off any address match */
                `$INSTANCE_NAME`_CFG_REG &= ~(`$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK);
                
            #else
                /* Turn on any address match */
                `$INSTANCE_NAME`_CFG_REG |= `$INSTANCE_NAME`_CTRL_ANY_ADDRESS_MASK;
                
            #endif  /* End (`$INSTANCE_NAME`_ADDR_DECODE == `$INSTANCE_NAME`_HDWR_DECODE) */

        #endif  /* End (`$INSTANCE_NAME`_MODE & `$INSTANCE_NAME`_MODE_SLAVE) */
        
    #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_FF) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Restores I2C configuration and starts CRC computation on rising edge of 
*  input clock.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Reentrant:
*  No/YES
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    
    /* Restore I2C Enable state */
    if (`$INSTANCE_NAME`_backup.enableState != 0u)
    {
        `$INSTANCE_NAME`_Enable();
    }
}


/* [] END OF FILE */
