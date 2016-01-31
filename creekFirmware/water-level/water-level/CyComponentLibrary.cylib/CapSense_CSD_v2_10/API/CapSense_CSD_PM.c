/*******************************************************************************
* File Name: `$INSTANCE_NAME`_PM.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides Sleep APIs for CapSense CSD Component.
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

`$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup =
{   
    0x00u, /* enableState; */
    #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_PRESCALER_VALUE, /* prescaler value */ 
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
    #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */
    
    /* Set ScanSpeed */
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        `$INSTANCE_NAME`_SCANSPEED_VALUE,  /* scan speed value */
    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveConfig
********************************************************************************
*
* Summary:
*  Saves customer configuration of CapSense none-retention registers. Resets 
*  all sensors to an inactive state.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used to save component state before enter sleep 
*  mode and none-retention registers.
*
* Reentrant:
*  No - for PSoC3 ES2 and PSoC5 ES1 silicon, Yes - for PSoC3 ES3.
*
*******************************************************************************/
#if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
    void `$INSTANCE_NAME`_SaveConfig(void)
#else
    void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`
#endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
{    
    /* Set Prescaler */
    #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_backup.prescaler  = `$INSTANCE_NAME`_PRESCALER_PERIOD_REG; 
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
    #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */
    
    /* Set ScanSpeed */
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        `$INSTANCE_NAME`_backup.scanspeed = `$INSTANCE_NAME`_SCANSPEED_PERIOD_REG;
    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
    
    /* Clear all sensors */
    `$INSTANCE_NAME`_ClearSensors();
    
    /* The pins disable is customer concern: Cmod and Rb */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Sleep
********************************************************************************
*
* Summary:
*  Disables Active mode power template bits for number of component used within 
*  CapSense. Calls `$INSTANCE_NAME`_SaveConfig() function to save customer 
*  configuration of CapSense none-retention registers and resets all sensors 
*  to an inactive state.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used to save component state before enter sleep 
*  mode and none-retention registers.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Sleep(void)
{
    /* Check and save enable state */
    if(`$INSTANCE_NAME`_IS_CAPSENSE_ENABLE(`$INSTANCE_NAME`_CONTROL_REG))
    {
        `$INSTANCE_NAME`_backup.enableState = 1u;
        `$INSTANCE_NAME`_Stop();
    }
    else
    {
        `$INSTANCE_NAME`_backup.enableState = 0u;
    }
    
    `$INSTANCE_NAME`_SaveConfig();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreConfig
********************************************************************************
*
* Summary:
*  Restores CapSense configuration and non-retention register values.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  Must be called only after `$INSTANCE_NAME`_SaveConfig() routine. Otherwise 
*  the component configuration will be overwritten with its initial setting.  
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used to save component state before enter sleep 
*  mode and none-retention registers.
*
*******************************************************************************/
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`
{   
    #if ( ((`$INSTANCE_NAME`_PRS_OPTIONS) || \
           (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB) || \
           ( (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
             (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB))) && \
          (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) )
        
        uint8 enableInterrupts;
    #endif /* End ( ((`$INSTANCE_NAME`_PRS_OPTIONS) || \
                     (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB) || \
                     ( (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
                       (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB))) && \
                    (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) ) */
    
    /* Set Prescaler */
    #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_PRESCALER_PERIOD_REG  = `$INSTANCE_NAME`_backup.prescaler;              /* D0 register */
            `$INSTANCE_NAME`_PRESCALER_COMPARE_REG = (`$INSTANCE_NAME`_backup.prescaler >> 0x01u);   /* D1 register */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
    #elif (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_FF)
        /* FF Timer register are retention */
    #else
        /* Do nothing = config without prescaler */
    #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */   
    
    /* Set PRS */
    #if (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_8BITS)
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            /* Aux control set FIFO as REG */ 
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_AUX_CONTROL_A_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
            CyExitCriticalSection(enableInterrupts);
            
            /* Write polynomial */
            `$INSTANCE_NAME`_POLYNOM_REG   = `$INSTANCE_NAME`_PRS8_DEFAULT_POLYNOM;             /* D0 register */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
        /* Write FIFO with seed */
        `$INSTANCE_NAME`_SEED_COPY_REG = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;               /* F0 register */
    
    #elif (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_16BITS)
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            /* Aux control set FIFO as REG */
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_AUX_CONTROL_A_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
            `$INSTANCE_NAME`_AUX_CONTROL_B_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
            CyExitCriticalSection(enableInterrupts);
            
            /* Write polynomial */
            CY_SET_REG16(`$INSTANCE_NAME`_POLYNOM_PTR, `$INSTANCE_NAME`_PRS16_DEFAULT_POLYNOM); /* D0 register */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
        /* Write FIFO with seed */
        CY_SET_REG16(`$INSTANCE_NAME`_SEED_COPY_PTR, `$INSTANCE_NAME`_MEASURE_FULL_RANGE);      /* F0 register */
                
    #elif (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_16BITS_4X)
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            /* Aux control set FIFO as REG */
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_AUX_CONTROL_A_REG  |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
            CyExitCriticalSection(enableInterrupts);
            
            /* Write polynomial */
            `$INSTANCE_NAME`_POLYNOM_A__D1_REG   = HI8(`$INSTANCE_NAME`_PRS16_DEFAULT_POLYNOM); /* D0 register */
            `$INSTANCE_NAME`_POLYNOM_A__D0_REG   = LO8(`$INSTANCE_NAME`_PRS16_DEFAULT_POLYNOM); /* D1 register */
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
        /* Write FIFO with seed */
        `$INSTANCE_NAME`_SEED_COPY_A__F1_REG = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;         /* F0 register */
        `$INSTANCE_NAME`_SEED_COPY_A__F0_REG =`$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;          /* F1 register */
        
    #else
        /* Do nothing = config without PRS */
    #endif  /* End (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_8BITS) */
    
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        /* Set ScanSpeed */
        `$INSTANCE_NAME`_SCANSPEED_PERIOD_REG = `$INSTANCE_NAME`_backup.scanspeed;       /* Counter7_PERIOD register */
    #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
    
    /* Set the Measure */
    #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
        /* Window PWM  - FF Timer register are retention */
        /* Raw Counter - FF Timer register are retention */
    #else
        /* Window PWM and Raw Counter AUX and D0 set */ 
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_PWM_CH0_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;   /* AUX register */
            `$INSTANCE_NAME`_RAW_CH0_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;   /* AUX register */
            CyExitCriticalSection(enableInterrupts);
            
            `$INSTANCE_NAME`_PWM_CH0_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;    /* D0 register */
            `$INSTANCE_NAME`_RAW_CH0_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;    /* D0 register */
            
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
        /* Window PWM */
        `$INSTANCE_NAME`_PWM_CH0_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;        /* F0 register */
        
        /* Raw Counter */
        `$INSTANCE_NAME`_RAW_CH0_PERIOD_HI_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;        /* F1 register */
        `$INSTANCE_NAME`_RAW_CH0_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;        /* F0 register */
    
    #endif  /* (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */ 
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            /* Window PWM  - FF Timer register are retention */
            /* Raw Counter - FF Timer register are retention */
        #else
            /* Window PWM and Raw Counter AUX and D0 set */ 
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                enableInterrupts = CyEnterCriticalSection();
                `$INSTANCE_NAME`_PWM_CH1_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG; /* AUX register */
                `$INSTANCE_NAME`_RAW_CH1_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG; /* AUX register */
                CyExitCriticalSection(enableInterrupts);
                
                `$INSTANCE_NAME`_RAW_CH1_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;   /* D0 register */
                `$INSTANCE_NAME`_PWM_CH1_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;   /* D0 register */
            #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
            
            /* Window PWM */
            `$INSTANCE_NAME`_PWM_CH1_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;       /* F0 register */
            
            /* Raw Counter */
            `$INSTANCE_NAME`_RAW_CH1_PERIOD_HI_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;       /* F1 register */
            `$INSTANCE_NAME`_RAW_CH1_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;       /* F0 register */
            
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */
    
    #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == TWO_CHANNELS_DESIGN)*/
    
    /* Enable window generation */
    `$INSTANCE_NAME`_CONTROL_REG = `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0;
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) 
        `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1; 
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
 
    /* The pins enable are customer concern: Cmod and Rb */
 }
 

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Wakeup
********************************************************************************
*
* Summary:
*  Restores CapSense configuration and non-retention register values. 
*  Restores enabled state of component by setting Active mode power template 
*  bits for number of component used within CapSense.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_backup - used to save component state before enter sleep 
*  mode and none-retention registers.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`
{
    `$INSTANCE_NAME`_RestoreConfig();
    
    /* Restore CapSense Enable state */
    if (`$INSTANCE_NAME`_backup.enableState != 0u)
    {
        `$INSTANCE_NAME`_Enable();
    }
}


/* [] END OF FILE */
