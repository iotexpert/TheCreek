/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides Sleep/WakeUp APIs functionality of the PrISM component
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


/***************************************
* Forward function references
***************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;


/***************************************
* Local data allocation
***************************************/
static `$INSTANCE_NAME`_BACKUP_STRUCT  `$INSTANCE_NAME`_backup = 
{
    /* enableState */
    0u,
    /* cr */
    #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
        (`$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL == `$CompareType0` ? \
                                                `$INSTANCE_NAME`_CTRL_COMPARE_TYPE0_GREATER_THAN_OR_EQUAL : 0) |
        (`$INSTANCE_NAME`_GREATERTHAN_OR_EQUAL == `$CompareType1` ? \
                                                `$INSTANCE_NAME`_CTRL_COMPARE_TYPE1_GREATER_THAN_OR_EQUAL : 0),
    #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
    /* seed */    
    `$INSTANCE_NAME`_SEED,
    /* seed_copy */    
    `$INSTANCE_NAME`_SEED,
    /* polynom */
    `$INSTANCE_NAME`_POLYNOM,
    #if(CY_UDB_V0)
        /* density0 */
        `$INSTANCE_NAME`_DENSITY0,
        /* density1 */
        `$INSTANCE_NAME`_DENSITY1,
    #endif /*End CY_UDB_V0*/
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
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
{
    #if (CY_UDB_V0)
        #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
            `$INSTANCE_NAME`_backup.cr = `$INSTANCE_NAME`_CONTROL_REG;
        #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
        `$INSTANCE_NAME`_backup.seed = `$INSTANCE_NAME`_ReadSeed();
        `$INSTANCE_NAME`_backup.seed_copy = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_SEED_COPY_PTR);
        `$INSTANCE_NAME`_backup.polynom = `$INSTANCE_NAME`_ReadPolynomial();
        `$INSTANCE_NAME`_backup.density0 = `$INSTANCE_NAME`_ReadPulse0();
        `$INSTANCE_NAME`_backup.density1 = `$INSTANCE_NAME`_ReadPulse1();
    #else /* CY_UDB_V1 */
        #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
            `$INSTANCE_NAME`_backup.cr = `$INSTANCE_NAME`_CONTROL_REG;
        #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
        `$INSTANCE_NAME`_backup.seed = `$INSTANCE_NAME`_ReadSeed();
        `$INSTANCE_NAME`_backup.seed_copy = `$CyGetRegReplacementString`(`$INSTANCE_NAME`_SEED_COPY_PTR);
        `$INSTANCE_NAME`_backup.polynom = `$INSTANCE_NAME`_ReadPolynomial();
    #endif  /* CY_UDB_V0 */
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
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{
    #if (CY_UDB_V0)
    
        uint8 enableInterrupts;
        
        #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
            `$INSTANCE_NAME`_CONTROL_REG = `$INSTANCE_NAME`_backup.cr;
        #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */

        `$CySetRegReplacementString`(`$INSTANCE_NAME`_SEED_COPY_PTR, `$INSTANCE_NAME`_backup.seed_copy);
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_SEED_PTR, `$INSTANCE_NAME`_backup.seed);
        `$INSTANCE_NAME`_WritePolynomial(`$INSTANCE_NAME`_backup.polynom);
        `$INSTANCE_NAME`_WritePulse0(`$INSTANCE_NAME`_backup.density0);
        `$INSTANCE_NAME`_WritePulse1(`$INSTANCE_NAME`_backup.density1);
        
        enableInterrupts = CyEnterCriticalSection();
        /* Set FIFO0_CLR bit to use FIFO0 as a simple one-byte buffer*/
        #if (`$INSTANCE_NAME`_RESOLUTION <= 8u)      /* 8bit - PrISM */
            `$INSTANCE_NAME`_AUX_CONTROL_REG |= `$INSTANCE_NAME`_FIFO0_CLR;
        #elif (`$INSTANCE_NAME`_RESOLUTION <= 16u)   /* 16bit - PrISM */
            CY_SET_REG16(`$INSTANCE_NAME`_AUX_CONTROL_PTR, CY_GET_REG16(`$INSTANCE_NAME`_AUX_CONTROL_PTR) | 
                                            `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u);
        #elif (`$INSTANCE_NAME`_RESOLUTION <= 24)   /* 24bit - PrISM */
            CY_SET_REG24(`$INSTANCE_NAME`_AUX_CONTROL_PTR, CY_GET_REG24(`$INSTANCE_NAME`_AUX_CONTROL_PTR) |
                                            `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u );
            CY_SET_REG24(`$INSTANCE_NAME`_AUX_CONTROL2_PTR, CY_GET_REG24(`$INSTANCE_NAME`_AUX_CONTROL2_PTR) | 
                                            `$INSTANCE_NAME`_FIFO0_CLR );
        #else                                 /* 32bit - PrISM */
            CY_SET_REG32(`$INSTANCE_NAME`_AUX_CONTROL_PTR, CY_GET_REG32(`$INSTANCE_NAME`_AUX_CONTROL_PTR) |
                                            `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u );
            CY_SET_REG32(`$INSTANCE_NAME`_AUX_CONTROL2_PTR, CY_GET_REG32(`$INSTANCE_NAME`_AUX_CONTROL2_PTR) |
                                            `$INSTANCE_NAME`_FIFO0_CLR | `$INSTANCE_NAME`_FIFO0_CLR << 8u );
        #endif                                /* End `$INSTANCE_NAME`_RESOLUTION */
        CyExitCriticalSection(enableInterrupts);
   
    #else   /* CY_UDB_V1 */

        #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
            `$INSTANCE_NAME`_CONTROL_REG = `$INSTANCE_NAME`_backup.cr;
        #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */

        `$CySetRegReplacementString`(`$INSTANCE_NAME`_SEED_COPY_PTR, `$INSTANCE_NAME`_backup.seed_copy);
        `$CySetRegReplacementString`(`$INSTANCE_NAME`_SEED_PTR, `$INSTANCE_NAME`_backup.seed);
        `$INSTANCE_NAME`_WritePolynomial(`$INSTANCE_NAME`_backup.polynom);
    
    #endif  /* End CY_UDB_V0 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Stops and saves the user configuration
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
    #if(!`$INSTANCE_NAME`_PULSE_TYPE_HARDCODED)
        if((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_ENABLE) != 0u) 
        {
            `$INSTANCE_NAME`_backup.enableState = 1u;
        }
        else
        {
            `$INSTANCE_NAME`_backup.enableState = 0u;
        }
    #endif /* End `$INSTANCE_NAME`_PULSE_TYPE_HARDCODED */
    `$INSTANCE_NAME`_Stop();
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Restores and enables the user configuration
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
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    if(`$INSTANCE_NAME`_backup.enableState != 0u)
    {
        `$INSTANCE_NAME`_Enable();
    } 
}


/* [] END OF FILE */
