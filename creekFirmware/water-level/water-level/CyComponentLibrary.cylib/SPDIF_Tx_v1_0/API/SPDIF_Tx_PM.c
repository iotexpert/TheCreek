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
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"

#if (0u != `$INSTANCE_NAME`_MANAGED_DMA)
    extern void `$INSTANCE_NAME`_CstDmaInit(void);
#endif /* (0u != `$INSTANCE_NAME`_MANAGED_DMA) */ 

static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup = 
{
    /* Generation of S/PDIF output is disabled */
    `$INSTANCE_NAME`_DISABLED,
    /* By default the interrupt will be generated only for errors */
    #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)
        `$INSTANCE_NAME`_DEFAULT_INT_SRC
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves SPDIF_Tx configuration.
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
    /* PSoC3 ES2 or early, PSoC5 ES1 */
    #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)
        `$INSTANCE_NAME`_backup.interruptMask = `$INSTANCE_NAME`_STATUS_MASK_REG;
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores SPDIF_Tx configuration.
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
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{        
    /* PSoC3 ES2 or early, PSoC5 ES1 */    
    #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)   
        uint8 enableInterrupts;    
        /* Enter critical section */
        enableInterrupts = CyEnterCriticalSection();
        /* Set FIFOs in the Single Buffer Mode */
        `$INSTANCE_NAME`_FCNT_AUX_CTL_REG    |= `$INSTANCE_NAME`_FX_CLEAR; 
        `$INSTANCE_NAME`_PREGEN_AUX_CTL_REG  |= `$INSTANCE_NAME`_FX_CLEAR;  
        /* Exit critical section */
        CyExitCriticalSection(enableInterrupts); 
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */        
    
    /* Restore Frame and Block Intervals */
    /* Preamble and Post Data Period */ 
    `$INSTANCE_NAME`_FCNT_PRE_POST_REG = `$INSTANCE_NAME`_PRE_POST_PERIOD;    
    /* Number of frames in block */
    `$INSTANCE_NAME`_FCNT_BLOCK_PERIOD_REG = `$INSTANCE_NAME`_BLOCK_PERIOD;    
    /* PSoC3 ES2 or early, PSoC5 ES1 */    
    #if(CY_PSOC3_ES2 || CY_PSOC5_ES1) 
        /* Frame Period */
        `$INSTANCE_NAME`_FRAME_PERIOD_REG = `$INSTANCE_NAME`_FRAME_PERIOD;
        /* Audio Sample Word Length */ 
        `$INSTANCE_NAME`_FCNT_AUDIO_LENGTH_REG = `$INSTANCE_NAME`_AUDIO_DATA_PERIOD;  
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */
        
    /* Restore Preamble Patterns */
    /* PSoC3 ES2 or early, PSoC5 ES1 */
    #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)
        `$INSTANCE_NAME`_PREGEN_PREX_PTRN_REG = `$INSTANCE_NAME`_PREAMBLE_X_PATTERN;
        `$INSTANCE_NAME`_PREGEN_PREY_PTRN_REG = `$INSTANCE_NAME`_PREAMBLE_Y_PATTERN;
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */            
    `$INSTANCE_NAME`_PREGEN_PREZ_PTRN_REG = `$INSTANCE_NAME`_PREAMBLE_Z_PATTERN;
    
    /* Restore Interrupt Mask */
    /* PSoC3 ES2 or early, PSoC5 ES1 */
    #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)
        `$INSTANCE_NAME`_STATUS_MASK_REG = `$INSTANCE_NAME`_backup.interruptMask;
    #endif /* (CY_PSOC3_ES2 || CY_PSOC5_ES1) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Prepares SPDIF_Tx goes to sleep.
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
    if(0u != (`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_ENBL))
    {
        `$INSTANCE_NAME`_backup.enableState = 1u;
    }
    else /* The component is disabled */
    {
        `$INSTANCE_NAME`_backup.enableState = 0u;
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
*  Prepares SPDIF_Tx to wake up.
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
    /* Restore registers values */
    `$INSTANCE_NAME`_RestoreConfig();        
}


/* [] END OF FILE */
