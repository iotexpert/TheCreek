/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code of scanning APIs for the CapSense CSD 
*  component.
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

/* Rb init function */
#if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB)
    void `$INSTANCE_NAME`_InitRb(void);
#endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */ 

#if (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)
    void `$INSTANCE_NAME`_EnableScanSlot(uint8 slot) `=ReentrantKeil($INSTANCE_NAME . "_EnableScanSlot")`;
    void `$INSTANCE_NAME`_DisableScanSlot(uint8 slot) `=ReentrantKeil($INSTANCE_NAME . "_DisableScanSlot")`;
    
#else
    #define `$INSTANCE_NAME`_EnableScanSlot(slot)   `$INSTANCE_NAME`_EnableSensor(slot)
    #define `$INSTANCE_NAME`_DisableScanSlot(slot)  `$INSTANCE_NAME`_DisableSensor(slot)

#endif  /* End `$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS */

/* Helper functions - do nto part of public interface*/

/* Find next sensor for One Channel design */
#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
    uint8 `$INSTANCE_NAME`_FindNextSensor(uint8 snsIndex) `=ReentrantKeil($INSTANCE_NAME . "_FindNextSensor")`;
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */

/* Find next pair for Two Channels design */
 #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    uint8 `$INSTANCE_NAME`_FindNextPair(uint8 snsIndex) `=ReentrantKeil($INSTANCE_NAME . "_FindNextPair")`;
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */

/* Start and Compete the scan */
void `$INSTANCE_NAME`_PreScan(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_PreScan")`;
#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
    void `$INSTANCE_NAME`_PostScan(uint8 sensor);
#else
    void `$INSTANCE_NAME`_PostScanCh0(uint8 sensor);
    void `$INSTANCE_NAME`_PostScanCh1(uint8 sensor);
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */

/* Idac functions definitions */
#if (`$INSTANCE_NAME`_CURRENT_SOURCE)
    void `$INSTANCE_NAME`_SetIdacRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_SetIdacRange")`;
    void `$INSTANCE_NAME`_IdacCH0_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_Init")`;
    void `$INSTANCE_NAME`_IdacCH0_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_Enable")`;
    void `$INSTANCE_NAME`_IdacCH0_SetRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_SetRange")`;
    void `$INSTANCE_NAME`_IdacCH0_DacTrim(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_DacTrim")`;
    void `$INSTANCE_NAME`_IdacCH0_SetValue(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_SetValue")`;
    void `$INSTANCE_NAME`_IdacCH0_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_Stop")`;
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        void `$INSTANCE_NAME`_IdacCH1_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_Init")`;
        void `$INSTANCE_NAME`_IdacCH1_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_Enable")`;
        void `$INSTANCE_NAME`_IdacCH1_SetRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_SetRange")`;
        void `$INSTANCE_NAME`_IdacCH1_DacTrim(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_DacTrim")`;
        void `$INSTANCE_NAME`_IdacCH1_SetValue(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_SetValue")`;
        void `$INSTANCE_NAME`_IdacCH1_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_Stop")`;
    #endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */ 
    
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */

#if (`$INSTANCE_NAME`_PRESCALER_OPTIONS)
    void `$INSTANCE_NAME`_SetPrescaler(uint8 prescaler) `=ReentrantKeil($INSTANCE_NAME . "_SetPrescaler")`;
#endif  /* End `$INSTANCE_NAME`_PRESCALER_OPTIONS */
void `$INSTANCE_NAME`_SetScanSpeed(uint8 scanspeed) `=ReentrantKeil($INSTANCE_NAME . "_SetScanSpeed")`;

/* SmartSense functions */
#if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING)
    extern void `$INSTANCE_NAME`_AutoTune(void);
    uint8 `$INSTANCE_NAME`_lowLevelTuningDone = 0u;
#endif /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING) */

uint8 `$INSTANCE_NAME`_initVar = 0u;
            
/* Global software variables */
volatile uint8 `$INSTANCE_NAME`_csv = 0u;            /* CapSense CSD status, control variable */
volatile uint8 `$INSTANCE_NAME`_sensorIndex = 0u;    /* Index of scannig sensor */

#if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB)
    uint8  `$INSTANCE_NAME`_RbCh0_cur = `$INSTANCE_NAME`_RBLEED1;
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        uint8  `$INSTANCE_NAME`_RbCh1_cur = (`$INSTANCE_NAME`_RBLEED1 + `$INSTANCE_NAME`_TOTAL_RB_NUMBER__CH0);
    #endif /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)*/ 
#endif /* (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB) */ 
        
/* Global array of Raw Counts */
uint16 `$INSTANCE_NAME`_SensorRaw[`$INSTANCE_NAME`_TOTAL_SENSOR_COUNT] = {0u};

`$writerCVariables`

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Inits default CapSense configuration provided with customizer that defines 
*  mode of component operations and resets all sensors to an inactive state.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    #if ( (`$INSTANCE_NAME`_PRS_OPTIONS) || \
          (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB) || \
          ( (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
            (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB)) )
        
        uint8 enableInterrupts;
    #endif /* End ( (`$INSTANCE_NAME`_PRS_OPTIONS) || \
                    (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB) || \
                    ( (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
                      (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB)) ) */
    
    /* Clear all sensors */
    `$INSTANCE_NAME`_ClearSensors();
    
    /* Set Prescaler */
    #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
        `$INSTANCE_NAME`_PRESCALER_PERIOD_REG  = `$INSTANCE_NAME`_PRESCALER_VALUE;
        `$INSTANCE_NAME`_PRESCALER_COMPARE_REG = (`$INSTANCE_NAME`_PRESCALER_VALUE >> 0x01u);
        
    #elif (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_FF)
        `$INSTANCE_NAME`_PRESCALER_CONTROL_REG   = (`$INSTANCE_NAME`_PRESCALER_CTRL_ENABLE |
                                                `$INSTANCE_NAME`_PRESCALER_CTRL_MODE_CMP);
                                               
        `$INSTANCE_NAME`_PRESCALER_CONTROL2_REG |= `$INSTANCE_NAME`_PRESCALER_CTRL_CMP_LESS_EQ;
         
        CY_SET_REG16(`$INSTANCE_NAME`_PRESCALER_PERIOD_PTR, (uint16) `$INSTANCE_NAME`_PRESCALER_VALUE);
        CY_SET_REG16(`$INSTANCE_NAME`_PRESCALER_COMPARE_PTR, (uint16) (`$INSTANCE_NAME`_PRESCALER_VALUE >> 0x01u));
        
    #else
        /* Do nothing = config without prescaler */
    #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */
    
    /* Set PRS */
    #if (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_8BITS)
        /* Aux control set FIFO as REG */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_AUX_CONTROL_A_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
        CyExitCriticalSection(enableInterrupts);
        
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_REG   = `$INSTANCE_NAME`_PRS8_DEFAULT_POLYNOM;
        /* Write FIFO with seed */
        `$INSTANCE_NAME`_SEED_COPY_REG = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        
    #elif (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_16BITS)
        /* Aux control set FIFO as REG */ 
        enableInterrupts = CyEnterCriticalSection();  
        `$INSTANCE_NAME`_AUX_CONTROL_A_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
        `$INSTANCE_NAME`_AUX_CONTROL_B_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
        CyExitCriticalSection(enableInterrupts);
        
        /* Write polynomial */
        CY_SET_REG16(`$INSTANCE_NAME`_POLYNOM_PTR, `$INSTANCE_NAME`_PRS16_DEFAULT_POLYNOM);
        /* Write FIFO with seed */
        CY_SET_REG16(`$INSTANCE_NAME`_SEED_COPY_PTR, `$INSTANCE_NAME`_MEASURE_FULL_RANGE);
                
    #elif (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_16BITS_4X)
        /* Aux control set FIFO as REG */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_AUX_CONTROL_A_REG  |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
        CyExitCriticalSection(enableInterrupts);
        
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_A__D1_REG   = HI8(`$INSTANCE_NAME`_PRS16_DEFAULT_POLYNOM);
        `$INSTANCE_NAME`_POLYNOM_A__D0_REG   = LO8(`$INSTANCE_NAME`_PRS16_DEFAULT_POLYNOM);
        /* Write FIFO with seed */
        `$INSTANCE_NAME`_SEED_COPY_A__F1_REG = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        `$INSTANCE_NAME`_SEED_COPY_A__F0_REG = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW; 
        
    #else
        /* Do nothing = config without PRS */
    #endif  /* End (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_8BITS) */ 
    
    /* Set ScanSpeed */
    `$INSTANCE_NAME`_SCANSPEED_PERIOD_REG = `$INSTANCE_NAME`_SCANSPEED_VALUE;
    
    /* Set the Measure */
    #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
        /* Window PWM */
        `$INSTANCE_NAME`_PWM_CH0_CONTROL_REG      = `$INSTANCE_NAME`_MEASURE_CTRL_ENABLE;
        `$INSTANCE_NAME`_PWM_CH0_CONTROL2_REG    |= `$INSTANCE_NAME`_MEASURE_CTRL_PULSEWIDTH;
        CY_SET_REG16(`$INSTANCE_NAME`_PWM_CH0_COUNTER_PTR, `$INSTANCE_NAME`_MEASURE_FULL_RANGE);
        
        /* Raw Counter */
        `$INSTANCE_NAME`_RAW_CH0_CONTROL_REG      = `$INSTANCE_NAME`_MEASURE_CTRL_ENABLE;
        `$INSTANCE_NAME`_RAW_CH0_CONTROL2_REG    |= `$INSTANCE_NAME`_MEASURE_CTRL_PULSEWIDTH;
        CY_SET_REG16(`$INSTANCE_NAME`_RAW_CH0_PERIOD_PTR, `$INSTANCE_NAME`_MEASURE_FULL_RANGE);
    
    #else
        /*Window PWM and Raw Counter AUX set */
        enableInterrupts = CyEnterCriticalSection();
        `$INSTANCE_NAME`_PWM_CH0_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
        `$INSTANCE_NAME`_RAW_CH0_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
        CyExitCriticalSection(enableInterrupts);
        
        /* Window PWM */
        `$INSTANCE_NAME`_PWM_CH0_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        `$INSTANCE_NAME`_PWM_CH0_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        `$INSTANCE_NAME`_PWM_CH0_COUNTER_LO_REG   = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        
        /* Raw Counter */
        `$INSTANCE_NAME`_RAW_CH0_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        `$INSTANCE_NAME`_RAW_CH0_PERIOD_HI_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        `$INSTANCE_NAME`_RAW_CH0_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
        
    #endif  /* (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */ 
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            /* Window PWM */
            `$INSTANCE_NAME`_PWM_CH1_CONTROL_REG      = `$INSTANCE_NAME`_MEASURE_CTRL_ENABLE;
            `$INSTANCE_NAME`_PWM_CH1_CONTROL2_REG    |= `$INSTANCE_NAME`_MEASURE_CTRL_PULSEWIDTH;
            CY_SET_REG16(`$INSTANCE_NAME`_PWM_CH1_COUNTER_PTR, `$INSTANCE_NAME`_MEASURE_FULL_RANGE);
            
            /* Raw Counter */
            `$INSTANCE_NAME`_RAW_CH1_CONTROL_REG      = `$INSTANCE_NAME`_MEASURE_CTRL_ENABLE;
            `$INSTANCE_NAME`_RAW_CH1_CONTROL2_REG    |= `$INSTANCE_NAME`_MEASURE_CTRL_PULSEWIDTH;
            CY_SET_REG16(`$INSTANCE_NAME`_RAW_CH1_PERIOD_PTR, `$INSTANCE_NAME`_MEASURE_FULL_RANGE);
           
        #else
            /*Window PWM and Raw Counter AUX set */
            enableInterrupts = CyEnterCriticalSection();
            `$INSTANCE_NAME`_PWM_CH1_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
            `$INSTANCE_NAME`_RAW_CH1_AUX_CONTROL_REG |= `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG;
            CyExitCriticalSection(enableInterrupts);
            
            /* Window PWM */
            `$INSTANCE_NAME`_PWM_CH1_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
            `$INSTANCE_NAME`_PWM_CH1_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
            `$INSTANCE_NAME`_PWM_CH1_COUNTER_LO_REG   = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
            
            /* Raw Counter */
            
            `$INSTANCE_NAME`_RAW_CH1_ADD_VALUE_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
            `$INSTANCE_NAME`_RAW_CH1_PERIOD_HI_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
            `$INSTANCE_NAME`_RAW_CH1_PERIOD_LO_REG    = `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW;
            
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */
    
    #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == TWO_CHANNELS_DESIGN)*/
    
    /* Setup ISR */
    CyIntDisable(`$INSTANCE_NAME`_IsrCH0_ISR_NUMBER);
    CyIntSetVector(`$INSTANCE_NAME`_IsrCH0_ISR_NUMBER, `$INSTANCE_NAME`_IsrCH0_ISR);
    CyIntSetPriority(`$INSTANCE_NAME`_IsrCH0_ISR_NUMBER, `$INSTANCE_NAME`_IsrCH0_ISR_PRIORITY);
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        CyIntDisable(`$INSTANCE_NAME`_IsrCH1_ISR_NUMBER);
        CyIntSetVector(`$INSTANCE_NAME`_IsrCH1_ISR_NUMBER, `$INSTANCE_NAME`_IsrCH1_ISR);
        CyIntSetPriority(`$INSTANCE_NAME`_IsrCH1_ISR_NUMBER, `$INSTANCE_NAME`_IsrCH1_ISR_PRIORITY);
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Setup AMux Bus: Connect Cmod, Cmp, Idac */
    `$INSTANCE_NAME`_AMuxCH0_Init();
    `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_CMOD_CHANNEL);
    `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_CMP_VP_CHANNEL);
    #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
        `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_IDAC_CHANNEL);
    #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) 
        `$INSTANCE_NAME`_AMuxCH1_Init();
        `$INSTANCE_NAME`_AMuxCH1_Connect(`$INSTANCE_NAME`_AMuxCH1_CMOD_CHANNEL);
        `$INSTANCE_NAME`_AMuxCH1_Connect(`$INSTANCE_NAME`_AMuxCH1_CMP_VP_CHANNEL);
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            `$INSTANCE_NAME`_AMuxCH1_Connect(`$INSTANCE_NAME`_AMuxCH1_IDAC_CHANNEL);
        #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Int Rb */
    #if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB)
        `$INSTANCE_NAME`_InitRb();
    #endif /* End (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB) */
    
    /* Enable window generation */
    `$INSTANCE_NAME`_CONTROL_REG = `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0;
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1;
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Initialize Cmp and Idac */
    `$INSTANCE_NAME`_CompCH0_Init();
    #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
        `$INSTANCE_NAME`_IdacCH0_Init();
    #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) 
        `$INSTANCE_NAME`_CompCH1_Init();
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            `$INSTANCE_NAME`_IdacCH1_Init();
        #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Initialize Vref if as VDAC */
    #if (`$INSTANCE_NAME`_VREF_OPTIONS == `$INSTANCE_NAME`_VREF_VDAC)
        `$INSTANCE_NAME`_VdacRefCH0_Init();
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_VdacRefCH1_Init();
        #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    #endif  /* End `$INSTANCE_NAME`_VREF_OPTIONS */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables active mode power template bits for number of component used within 
*  CapSense.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    uint8 enableInterrupts;
    
    enableInterrupts = CyEnterCriticalSection();
    
    /* Enable Prescaler */
    #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
        /* Do nothing  for UDB */
    #elif (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_FF)
        `$INSTANCE_NAME`_PRESCALER_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_PRESCALER_ACT_PWR_EN;
        `$INSTANCE_NAME`_PRESCALER_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_PRESCALER_STBY_PWR_EN;
        
    #else
        /* Do nothing = config without prescaler */
    #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */
    
    /* Enable ScanSpeed */
    `$INSTANCE_NAME`_SCANSPEED_AUX_CONTROL_REG |= `$INSTANCE_NAME`_SCANSPEED_CTRL_ENABLE;
    
    /* Enable Measure CH0 */
    #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
        /* Window PWM */
        `$INSTANCE_NAME`_PWM_CH0_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_PWM_CH0_ACT_PWR_EN;
        `$INSTANCE_NAME`_PWM_CH0_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_PWM_CH0_STBY_PWR_EN;
        
        /* Raw Counter */
        `$INSTANCE_NAME`_RAW_CH0_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_RAW_CH0_ACT_PWR_EN;
        `$INSTANCE_NAME`_RAW_CH0_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_RAW_CH0_STBY_PWR_EN;
        
    #else
        /* Window PWM -  Do nothing */
        /* Raw Counter - Do nothing */
        
    #endif  /* (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */ 
    
    /* Enable Measure CH1*/
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            /* Window PWM */
            `$INSTANCE_NAME`_PWM_CH1_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_PWM_CH1_ACT_PWR_EN;
            `$INSTANCE_NAME`_PWM_CH1_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_PWM_CH1_STBY_PWR_EN;
            
            /* Raw Counter */
            `$INSTANCE_NAME`_RAW_CH1_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_RAW_CH1_ACT_PWR_EN;
            `$INSTANCE_NAME`_RAW_CH1_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_RAW_CH1_STBY_PWR_EN;
           
        #else
        /* Window PWM -  Do nothing */
        /* Raw Counter - Do nothing */
        
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */
    
    #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == TWO_CHANNELS_DESIGN)*/
    
    /* Enable the Clock */
    #if (`$INSTANCE_NAME`_CLOCK_SOURCE == `$INSTANCE_NAME`_INTERNAL_CLOCK)
       `$INSTANCE_NAME`_IntClock_Enable();
    #endif  /* End `$INSTANCE_NAME`_CLOCK_SOURCE */
    
    /* Setup Cmp and Idac */
    `$INSTANCE_NAME`_CompCH0_Enable();
    #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
        `$INSTANCE_NAME`_IdacCH0_Enable();
    #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) 
        `$INSTANCE_NAME`_CompCH1_Enable();
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            `$INSTANCE_NAME`_IdacCH1_Enable();
        #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Enable Vref */
    #if (`$INSTANCE_NAME`_VREF_OPTIONS == `$INSTANCE_NAME`_VREF_VDAC)
        `$INSTANCE_NAME`_VdacRefCH0_Enable();
        `$INSTANCE_NAME`_VdacRefCH0_SetValue(`$INSTANCE_NAME`_VdacRefCH0_DEFAULT_DATA);
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_VdacRefCH1_Enable();
            `$INSTANCE_NAME`_VdacRefCH1_SetValue(`$INSTANCE_NAME`_VdacRefCH1_DEFAULT_DATA);
        #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */
    #else
        /* Enable CapSense Buf */
        `$INSTANCE_NAME`_BufCH0_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_BufCH0_STBY_PWR_EN;
        `$INSTANCE_NAME`_BufCH0_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_BufCH0_ACT_PWR_EN;
        
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_BufCH1_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_BufCH1_STBY_PWR_EN;
            `$INSTANCE_NAME`_BufCH1_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_BufCH1_ACT_PWR_EN;
        #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */
    #endif  /* End (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS) */
    
    /* Set reference on AMux Bus */
    #if (`$INSTANCE_NAME`_VREF_OPTIONS == `$INSTANCE_NAME`_VREF_VDAC)
        /* Connect Vdac to AMux Bus */
        `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_VREF_CHANNEL);
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_AMuxCH1_Connect(`$INSTANCE_NAME`_AMuxCH1_VREF_CHANNEL);
        #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */
        
    #else
        /* Enable CapSense Buf */
        `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG |= `$INSTANCE_NAME`_CSBUF_ENABLE;
        
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_REG |= `$INSTANCE_NAME`_CSBUF_ENABLE;
        #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */
    #endif  /* End (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS)*/
    
    CyExitCriticalSection(enableInterrupts);
    
    /* Enable interrupt */
    CyIntEnable(`$INSTANCE_NAME`_IsrCH0_ISR_NUMBER);
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) 
        CyIntEnable(`$INSTANCE_NAME`_IsrCH1_ISR_NUMBER);
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Set CapSense Enable state */
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_CAPSENSE_EN;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Initializes registers and starts the CSD method of CapSense component. Reset 
*  all sensors to an inactive state. Enables interrupts for sensors scanning.
*  When Auto Tuning (SmartSense) mode is selected the tuning procedure is 
*  applied for all sensors.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified on 
*  first function call.
*  `$INSTANCE_NAME`_lowLevelTuningDone - used to notify the Tuner GUI that 
*  tuning of scanning parameters are done.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    if (`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }
    `$INSTANCE_NAME`_Enable();
    
    /* AutoTunning start */
    #if (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING)
        /* AutoTune by sensor or pair of sensor basis */
        `$INSTANCE_NAME`_AutoTune();
        `$INSTANCE_NAME`_lowLevelTuningDone = 1u;
    #endif /* End (`$INSTANCE_NAME`_TUNING_METHOD == `$INSTANCE_NAME`_AUTO_TUNING) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops the sensors scanner, disables internal interrupts, and resets all 
*  sensors to an inactive state. Disables Active mode power template bits for 
*  number of component used within CapSense.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  This function should be called after scans will be completed.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Stop Capsensing */
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_START;
    
    /* Disable interrupt */
    CyIntDisable(`$INSTANCE_NAME`_IsrCH0_ISR_NUMBER);
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) 
        CyIntDisable(`$INSTANCE_NAME`_IsrCH1_ISR_NUMBER);
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Clear all sensors */
    `$INSTANCE_NAME`_ClearSensors();
    
    /* Disable Prescaler */
    #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
        /* Do nothing  for UDB */
    #elif (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_FF)        
        `$INSTANCE_NAME`_PRESCALER_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_PRESCALER_ACT_PWR_EN;
        `$INSTANCE_NAME`_PRESCALER_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_PRESCALER_STBY_PWR_EN;
        
    #else
        /* Do nothing = config without prescaler */
    #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */
    
    /* Disable ScanSpeed */
    `$INSTANCE_NAME`_SCANSPEED_AUX_CONTROL_REG &= ~`$INSTANCE_NAME`_SCANSPEED_CTRL_ENABLE;
    
    /* Disable Measure CH0 */
    #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
        /* Window PWM */
        `$INSTANCE_NAME`_PWM_CH0_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_PWM_CH0_ACT_PWR_EN;
        `$INSTANCE_NAME`_PWM_CH0_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_PWM_CH0_STBY_PWR_EN;

        /* Raw Counter */
        `$INSTANCE_NAME`_RAW_CH0_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_RAW_CH0_ACT_PWR_EN;
        `$INSTANCE_NAME`_RAW_CH0_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_RAW_CH0_STBY_PWR_EN;

    #else
        /* Window PWM -  Do nothing */
        /* Raw Counter - Do nothing */
        
    #endif  /* (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */ 
    
    /* Disable Measure CH1 */
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            /* Window PWM */
            `$INSTANCE_NAME`_PWM_CH1_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_PWM_CH1_ACT_PWR_EN;
            `$INSTANCE_NAME`_PWM_CH1_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_PWM_CH1_STBY_PWR_EN;
    
            /* Raw Counter */
            `$INSTANCE_NAME`_RAW_CH1_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_RAW_CH1_ACT_PWR_EN;
            `$INSTANCE_NAME`_RAW_CH1_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_RAW_CH1_STBY_PWR_EN;
           
        #else
        /* Window PWM -  Do nothing */
        /* Raw Counter - Do nothing */
        
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */
    
    #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == TWO_CHANNELS_DESIGN)*/
    
    /* Disable the Clock */
    #if (`$INSTANCE_NAME`_CLOCK_SOURCE == `$INSTANCE_NAME`_INTERNAL_CLOCK)
       `$INSTANCE_NAME`_IntClock_Stop();
    #endif  /* End `$INSTANCE_NAME`_CLOCK_SOURCE */
    
    /* Disable power from Cmp and Idac */
    `$INSTANCE_NAME`_CompCH0_Stop();
    #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
        `$INSTANCE_NAME`_IdacCH0_Stop();
    #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) 
        `$INSTANCE_NAME`_CompCH1_Stop();
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            `$INSTANCE_NAME`_IdacCH1_Stop();
        #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */    
    
    /* Disable Vref if as VDAC */
    #if (`$INSTANCE_NAME`_VREF_OPTIONS == `$INSTANCE_NAME`_VREF_VDAC)
        `$INSTANCE_NAME`_VdacRefCH0_Stop();
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_VdacRefCH1_Stop();
        #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    #endif  /* End `$INSTANCE_NAME`_VREF_OPTIONS */

    #if (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS)
        /* The Idac turn off before */
    #else
        /* Enable CapSense Buf */
        `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
        `$INSTANCE_NAME`_BufCH0_ACT_PWRMGR_REG &= ~`$INSTANCE_NAME`_BufCH0_ACT_PWR_EN;
        `$INSTANCE_NAME`_BufCH0_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_BufCH0_STBY_PWR_EN;
        
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
            `$INSTANCE_NAME`_BufCH1_ACT_PWRMGR_REG &= ~`$INSTANCE_NAME`_BufCH1_ACT_PWR_EN;
            `$INSTANCE_NAME`_BufCH1_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_BufCH1_STBY_PWR_EN;
        #endif  /* End (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */
    #endif  /* End (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS) */
    
    /* Set CapSense Disable state */
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_CAPSENSE_EN;
}


#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_FindNextSensor
    ********************************************************************************
    *
    * Summary:
    *  Finds next sensor to scan. 
    *
    * Parameters:
    *  snsIndex:  Current index of sensor.
    *
    * Return:
    *  Returns the next sensor index to scan.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_SensorEnableMask[ ] - used to store bit masks of enabled 
    *  sensors.
    *  `$INSTANCE_NAME`_SensorEnableMask[0] contains the masked bits for sensors 0
    *  through 7 (sensor 0 is bit 0, sensor 1 is bit 1).
    *  `$INSTANCE_NAME`_SensorEnableMask[1] contains the masked bits for sensors 
    *  8 through 15 (if needed), and so on.
    *    0 - sensor doesn't scan by `$INSTANCE_NAME`_ScanEnabledWidgets().
    *    1 - sensor scans by `$INSTANCE_NAME`_ScanEnabledWidgets().
    *
    * Note: 
    *  This function has effect on current scanning scanning and should not
    *  be used outisde of component.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_FindNextSensor(uint8 snsIndex) `=ReentrantKeil($INSTANCE_NAME . "_FindNextSensor")`
    {
        uint8 pos;
        uint8 enMask;
        
        /* Check if sensor enabled */
        do
        {
            /* Proceed with the next sensor */
            snsIndex++;
            if(snsIndex == `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT)
            {
                break;
            }
            pos = (snsIndex >> 3u);
            enMask = 0x01u << (snsIndex & 0x07u);
        }    
        while((`$INSTANCE_NAME`_SensorEnableMask[pos] & enMask) == 0u);
        
        return snsIndex;
    }
 #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
 
 
#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_FindNextPair
    ********************************************************************************
    *
    * Summary:
    *  Finds next pair or sensor to scan. Sets condition bits to skip scanning.
    *  
    * Parameters:
    *  snsIndex:  Current index pair of sensors.
    *
    * Return:
    *  Returns the next pair of sensors index to scan.
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_SensorEnableMask[ ] - used to store bit masks of enabled 
    *  sensors.
    *  `$INSTANCE_NAME`_SensorEnableMask[0] contains the masked bits for sensors 0
    *  through 7 (sensor 0 is bit 0, sensor 1 is bit 1).
    *  `$INSTANCE_NAME`_SensorEnableMask[1] contains the masked bits for sensors 
    *  8 through 15 (if needed), and so on.
    *    0 - sensor doesn't scan by `$INSTANCE_NAME`_ScanEnabledWidgets().
    *    1 - sensor scans by `$INSTANCE_NAME`_ScanEnabledWidgets().
    *
    * Note: 
    *  This function has effect on control signals set for scanning and should not
    *  be used outisde of component.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_FindNextPair(uint8 snsIndex) `=ReentrantKeil($INSTANCE_NAME . "_FindNextPair")`
    {
        uint8 posCh;
        uint8 enMaskCh;
        uint8 indexCh0 = snsIndex;
        uint8 indexCh1 = snsIndex + `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0;
        
        /* Find enabled sensor on channel 0 */
        do
        {
            /* Procced the scanning */
            indexCh0++;
            if (indexCh0 >= `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0)
            {
                /* Lets hadle now all from CH1 */
                indexCh0 = `$INSTANCE_NAME`_END_OF_SCAN__CH0;
                break;
            }
            
            posCh = (indexCh0 >> 3u);
            enMaskCh = 0x01u << (indexCh0 & 0x07u);
        }
        while((`$INSTANCE_NAME`_SensorEnableMask[posCh] & enMaskCh) == 0u);
        
        /* Find enabled sensor on channel 1 */
        do
        {
            /* Procced the scanning */
            indexCh1++;        
            if (indexCh1 >= `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT)
            {
                /* Lets hadle now all from CH0 */
                indexCh1 = `$INSTANCE_NAME`_END_OF_SCAN__CH1;
                break;
            }
            
            posCh = (indexCh1 >> 3u);
            enMaskCh = 0x01u << (indexCh1 & 0x07u);
        } 
        while((`$INSTANCE_NAME`_SensorEnableMask[posCh] & enMaskCh) == 0u);
        
        indexCh1 -= `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0;
        
        /* Find the pair to scan */
        if(indexCh0 == indexCh1)
        {
            /* Scans TWO Channels */
            snsIndex = indexCh0;
            
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0;
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1;
        }
        else if(indexCh0 < indexCh1)
        {
           /* Scans Channel ONE only */
           snsIndex = indexCh0;
           
           `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0;
           `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1;
        }
        else
        {
            /* Scans Channel TWO only */
            snsIndex = indexCh1;
            
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1;
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0;
        }
        
        return snsIndex;
    }
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetScanSlotSettings
********************************************************************************
*
* Summary:
*  Sets the scan settings of the selected scan slot (sensor or pair of sensors). 
*  The scan settings incorporate IDAC value (for IDAC configurations) for every 
*  sensor and resolution. The resolution is the same for all sensors within 
*  widget.
*
* Parameters:
*  slot:  Scan slot number (sensor or pair of sensors).
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_idacSettings[] - used to store idac value for every sensor.
*  `$INSTANCE_NAME`_widgetResolution[] - used to store scan resolution of every 
*  widget.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetScanSlotSettings(uint8 slot) `=ReentrantKeil($INSTANCE_NAME . "_SetScanSlotSettings")`
{
    uint8 widget;
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        /* Define widget sensor belongs to */
        widget = `$INSTANCE_NAME`_widgetNumber[slot];
        
        /* Set Idac Value */
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            `$INSTANCE_NAME`_IdacCH0_SetValue(`$INSTANCE_NAME`_idacSettings[slot]);
        #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
        
        /* Window PWM */
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            CY_SET_REG16(`$INSTANCE_NAME`_PWM_CH0_PERIOD_PTR,
                ((uint16) `$INSTANCE_NAME`_widgetResolution[widget] << 8u) | `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW);
        #else
            `$INSTANCE_NAME`_PWM_CH0_PERIOD_HI_REG = `$INSTANCE_NAME`_widgetResolution[widget];
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */ 
        
    #else
        if(slot < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0)
        {
            /* Define widget sensor belongs to */
            widget = `$INSTANCE_NAME`_widgetNumber[slot];
            
            /* Set Idac Value */
            #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
                `$INSTANCE_NAME`_IdacCH0_SetValue(`$INSTANCE_NAME`_idacSettings[slot]);
            #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
            
            /* Set Pwm Resolution */
            #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
                CY_SET_REG16(`$INSTANCE_NAME`_PWM_CH0_PERIOD_PTR,
                  ((uint16) `$INSTANCE_NAME`_widgetResolution[widget] << 8u) | `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW);
            #else
                `$INSTANCE_NAME`_PWM_CH0_PERIOD_HI_REG = `$INSTANCE_NAME`_widgetResolution[widget];
            #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)*/ 
        }
        
        if(slot < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH1)
        {
            slot += `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0;
            widget = `$INSTANCE_NAME`_widgetNumber[slot];
        
            /* Set Idac Value */
            #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
                `$INSTANCE_NAME`_IdacCH1_SetValue(`$INSTANCE_NAME`_idacSettings[slot]);
            #endif  /* End `$INSTANCE_NAME`_CURRENT_SOURCE */
            
            /* Set Pwm Resolution */
            #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
                CY_SET_REG16(`$INSTANCE_NAME`_PWM_CH1_PERIOD_PTR,
                  ((uint16) `$INSTANCE_NAME`_widgetResolution[widget] << 8u) | `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW);
            #else
                `$INSTANCE_NAME`_PWM_CH1_PERIOD_HI_REG = `$INSTANCE_NAME`_widgetResolution[widget];
            #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)*/ 
        }
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ScanSensor
********************************************************************************
*
* Summary:
*  Sets scan settings and starts scanning a sensor or pair of combined sensors
*  on each channel. If two channels are configured, two sensors may be scanned 
*  at the same time. After scanning is complete the isr copies the measured 
*  sensor raw data to the global array. Use of the isr ensures this function 
*  is non-blocking. Each sensor has a unique number within the sensor array. 
*  This number is assigned by the CapSense customizer in sequence.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_csv - used to provide status and mode of scanning process. 
*  Sets busy status(scan in progress) and mode of scan as single scan.
*  For two channel design the additional bits are set to define if scan a 
*  pair of sensors or single one.
*  `$INSTANCE_NAME`_sensorIndex - used to store sensor scanning sensor number.
*  Sets to provided sensor argument.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_ScanSensor(uint8 sensor)
{
    /* Clears status/control variable and set sensorIndex */
    `$INSTANCE_NAME`_csv = 0u;
    `$INSTANCE_NAME`_sensorIndex = sensor;
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        /* Start of sensor scan */
        `$INSTANCE_NAME`_csv = (`$INSTANCE_NAME`_SW_STS_BUSY | `$INSTANCE_NAME`_SW_CTRL_SINGLE_SCAN);
        `$INSTANCE_NAME`_PreScan(sensor);
        
    #else
        /* CH0: check end of scan conditions */
        if(sensor < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0)
        {
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0;
        }
        
        /* CH1: check end of scan conditions */
        if(sensor < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH1)
        {
            `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1;
        }
        else
        {
            `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1;
        }
        
        /* Start sensor scan */
        if( ((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0) != 0u) || 
            ((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1) != 0u) )
        {
        
            `$INSTANCE_NAME`_csv |= (`$INSTANCE_NAME`_SW_STS_BUSY | `$INSTANCE_NAME`_SW_CTRL_SINGLE_SCAN);
            `$INSTANCE_NAME`_PreScan(sensor);
        }
        
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ScanEnableWidgets
********************************************************************************
*
* Summary:
*  Scans all of the enabled widgets. Starts scanning a sensor or pair of sensors 
*  within enabled widget. The isr proceeding scanning next sensor or pair till 
*  all enabled widgets will be scanned. Use of the isr ensures this function is 
*  non-blocking. All widgets are enabled by default except proximity widgets. 
*  Proximity widgets must be manually enabled as their long scan time is 
*  incompatible with fast response desired of other widget types.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_csv - used to provide status and mode of scanning process. 
*  Sets busy status(scan in progress) and clears single scan mode.
*  For two channel design the additional bits are set to define if scan a 
*  pair of sensors or single one. 
*  `$INSTANCE_NAME`_sensorIndex - used to store sensor scanning sensor number.
*  Sets to 0xFF and provided to function `$INSTANCE_NAME`_FindNextSensor or
*  `$INSTANCE_NAME`_FindNextPair, these functions starts with sensor index
*  increment and overflow of uint8 gives desired index 0.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_ScanEnabledWidgets(void)
{
    /* Clears status/control variable and set sensorIndex */
    `$INSTANCE_NAME`_csv = 0u;
    `$INSTANCE_NAME`_sensorIndex = 0xFFu;
    
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        /* Find next sensor */
        `$INSTANCE_NAME`_sensorIndex = `$INSTANCE_NAME`_FindNextSensor(`$INSTANCE_NAME`_sensorIndex);

        /* Check end of scan condition */
        if(`$INSTANCE_NAME`_sensorIndex < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT)
        {
            `$INSTANCE_NAME`_csv |= `$INSTANCE_NAME`_SW_STS_BUSY;
            `$INSTANCE_NAME`_PreScan(`$INSTANCE_NAME`_sensorIndex);
        }
        
    #else
        /* Find next sensor and set proper control register */
        `$INSTANCE_NAME`_sensorIndex = `$INSTANCE_NAME`_FindNextPair(`$INSTANCE_NAME`_sensorIndex);
        
        /* Start sensor scan */
        if((`$INSTANCE_NAME`_sensorIndex < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0) || 
           (`$INSTANCE_NAME`_sensorIndex < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH1))
        {
            `$INSTANCE_NAME`_csv |= `$INSTANCE_NAME`_SW_STS_BUSY;
            `$INSTANCE_NAME`_PreScan(`$INSTANCE_NAME`_sensorIndex);
        }
        
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_IsBusy
********************************************************************************
*
* Summary:
*  Returns the state of CapSense component. The 1 means that scanning in 
*  progress and 0 means that scanning is complete.
*
* Parameters:
*  None
*
* Return:
*  Returns the state of scanning. 1 - scanning in progress, 0 - scanning 
*  completed.
*
* Global Variables:
*  `$INSTANCE_NAME`_csv - used to provide status and mode of scanning process. 
*  Checks the busy status.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_IsBusy(void) `=ReentrantKeil($INSTANCE_NAME . "_IsBusy")`
{
    return ((`$INSTANCE_NAME`_csv & `$INSTANCE_NAME`_SW_STS_BUSY) == 
             `$INSTANCE_NAME`_SW_STS_BUSY) ? 1u : 0u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadSensorRaw
********************************************************************************
*
* Summary:
*  Returns scan sensor raw data from the `$INSTANCE_NAME`_SensorRaw[] array. 
*  Each scan sensor has a unique number within the sensor array. This number 
*  is assigned by the CapSense customizer in sequence.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  Returns current raw data value for defined sensor number.
*
* Global Variables:
*  `$INSTANCE_NAME`_SensorRaw[] - used to store sensors raw data.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_ReadSensorRaw(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_ReadSensorRaw")`
{
    return `$INSTANCE_NAME`_SensorRaw[sensor];
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearSensors
********************************************************************************
*
* Summary:
*  Resets all sensors to the non-sampling state by sequentially disconnecting
*  all sensors from Analog MUX Bus and putting them to inactive state.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearSensors(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearSensors")`
{
    uint8 i;
   
    for (i = 0u; i < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT; i++)
    {
        `$INSTANCE_NAME`_DisableScanSlot(i);
    }
}


#if (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EnableScanSlot
    ********************************************************************************
    *
    * Summary:
    *  Configures the selected slot to measure during the next measurement 
    *  cycle. The corresponding pin/pins are set to Analog High-Z mode and 
    *  connected to the Analog Mux Bus. This also enables the comparator function.
    *
    * Parameters:
    *  slot:  Slot number.
    *
    * Return:
    *  None
    *
    * Global Constants:
    *  `$INSTANCE_NAME`_portTable[]  - used to store the port number that pin 
    *  belongs to for every sensor.
    *  `$INSTANCE_NAME`_maskTable[]  - used to store the pin within the port for 
    *  every sensor.
    *  `$INSTANCE_NAME`_indexTable[] - used to store indexes of complex sensors.
    *  The offset and position in this array are stored in port and mask table for 
    *  complex sensors.
    *  The bit 7 (msb) is used to define the sensor type: single or complex.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_EnableScanSlot(uint8 slot) `=ReentrantKeil($INSTANCE_NAME . "_EnableScanSlot")`
    {
        uint8 j;
        uint8 snsNumber;
        const uint8 CYCODE *index;
        /* Read the sensor type: single or complex */
        uint8 snsType = `$INSTANCE_NAME`_portTable[slot];
        
        /* Check if sensor is complex */
        if ((snsType & `$INSTANCE_NAME`_COMPLEX_SS_FLAG) == 0u)
        {
            /* Enable sensor (signle) */
            `$INSTANCE_NAME`_EnableSensor(slot);
        }
        else
        {
            /* Enable complex sensor */
            snsType &= ~`$INSTANCE_NAME`_COMPLEX_SS_FLAG;
            index = &`$INSTANCE_NAME`_indexTable[snsType];
            snsNumber = `$INSTANCE_NAME`_maskTable[slot];
                        
            for (j=0; j < snsNumber; j++)
            {
                `$INSTANCE_NAME`_EnableSensor(index[j]);
            }
        } 
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_DisableScanSlot
    ********************************************************************************
    *
    * Summary:
    *  Disables the selected slot. The corresponding pin/pis is/are disconnected 
    *  from the Analog Mux Bus and connected to GND, High_Z or Shield electrode.
    *
    * Parameters:
    *  slot:  Slot number.
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_portTable[]  - used to store the port number that pin 
    *  belongs to for every sensor.
    *  `$INSTANCE_NAME`_maskTable[]  - used to store the pin within the port for 
    *  every sensor.
    *  `$INSTANCE_NAME`_indexTable[] - used to store indexes of complex sensors.
    *  The offset and position in this array are stored in port and mask table for 
    *  complex sensors.
    *  The 7bit(msb) is used to define the sensor type: single or complex.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_DisableScanSlot(uint8 slot) `=ReentrantKeil($INSTANCE_NAME . "_DisableScanSlot")`
    {
        uint8 j;
        uint8 snsNumber;
        const uint8 CYCODE *index;
        /* Read the sensor type: single or complex */
        uint8 snsType = `$INSTANCE_NAME`_portTable[slot];
        
        /* Check if sensor is complex */
        if ((snsType & `$INSTANCE_NAME`_COMPLEX_SS_FLAG) == 0u)
        {
            /* Disable sensor (signle) */
            `$INSTANCE_NAME`_DisableSensor(slot);
        }
        else
        {
            /* Disable complex sensor */
            snsType &= ~`$INSTANCE_NAME`_COMPLEX_SS_FLAG;
            index = &`$INSTANCE_NAME`_indexTable[snsType];
            snsNumber = `$INSTANCE_NAME`_maskTable[slot];
                        
            for (j=0; j < snsNumber; j++)
            {
                `$INSTANCE_NAME`_DisableSensor(index[j]);
            }
        } 
    }
#endif  /* End `$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableSensor
********************************************************************************
*
* Summary:
*  Configures the selected sensor to measure during the next measurement cycle.
*  The corresponding pins are set to Analog High-Z mode and connected to the
*  Analog Mux Bus. This also enables the comparator function.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_portTable[] - used to store the port number that pin 
*  belongs to for every sensor.
*  `$INSTANCE_NAME`_maskTable[] - used to store the pin within the port for 
*  every sensor.
*  `$INSTANCE_NAME`_csTable[]   - used to store the pointers to CAPS_SEL 
*  registers for every port.
*  `$INSTANCE_NAME`_pcTable[]   - used to store the pointers to PC pin 
*  register for every sensor.
*  `$INSTANCE_NAME`_amuxIndex[] - used to store corrected AMUX index when 
*  complex sensors are defeined.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableSensor(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_EnableSensor")`
{
    uint8 port = `$INSTANCE_NAME`_portTable[sensor];
    uint8 mask = `$INSTANCE_NAME`_maskTable[sensor];
    
    #if ((`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
         (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS))
        uint8 amuxCh = `$INSTANCE_NAME`_amuxIndex[sensor];
    #endif  /* End ((`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
                   (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)) */
    
    /* Make sensor High-Z */
    *`$INSTANCE_NAME`_pcTable[sensor] = `$INSTANCE_NAME`_PRT_PC_HIGHZ;
    
    /* Connect to DSI output */
    *`$INSTANCE_NAME`_csTable[port] |= mask;
    
    /* Connect to AMUX */
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        #if (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)
            `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_amuxIndex[sensor]);
        #else
            `$INSTANCE_NAME`_AMuxCH0_Connect(sensor);
        #endif  /* End `$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS */
                
    #else
        #if (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)
            if ((amuxCh & `$INSTANCE_NAME`_CHANNEL1_FLAG) == 0u)
            {
                `$INSTANCE_NAME`_AMuxCH0_Connect(amuxCh);
            } 
            else
            {
                amuxCh &= ~ `$INSTANCE_NAME`_CHANNEL1_FLAG;
                `$INSTANCE_NAME`_AMuxCH1_Connect(amuxCh);
            }
            
        #else
            if (sensor < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0) 
            {
                `$INSTANCE_NAME`_AMuxCH0_Connect(sensor);
            } 
            else
            {
                `$INSTANCE_NAME`_AMuxCH1_Connect(sensor - `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0);
            }
            
        #endif  /* End `$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS */
        
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableSensor
********************************************************************************
*
* Summary:
*  Disables the selected sensor. The corresponding pin is disconnected from the
*  Analog Mux Bus and connected to GND, High_Z or Shield electrode.
*
* Parameters:
*  sensor:  Sensor number
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_portTable[] - used to store the port number that pin 
*  belongs to for every sensor.
*  `$INSTANCE_NAME`_maskTable[] - used to store the pin within the port for 
*  every sensor.
*  `$INSTANCE_NAME`_csTable[]   - used to store the pointers to CAPS_SEL 
*  registers for every port.
*  `$INSTANCE_NAME`_pcTable[]   - used to store the pointers to PC pin 
*  register for every sensor.
*  `$INSTANCE_NAME`_amuxIndex[] - used to store corrected AMUX index when 
*  complex sensors are defeined.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableSensor(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_DisableSensor")`
{
    uint8 port = `$INSTANCE_NAME`_portTable[sensor];
    uint8 mask = `$INSTANCE_NAME`_maskTable[sensor];
    
    #if ((`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
         (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS))
        uint8 amuxCh = `$INSTANCE_NAME`_amuxIndex[sensor];
    #endif  /* End ((`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) && \
                   (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)) */
    
    /* Disconnect from AMUX */
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        #if (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)
            `$INSTANCE_NAME`_AMuxCH0_Disconnect(`$INSTANCE_NAME`_amuxIndex[sensor]);
        #else
            `$INSTANCE_NAME`_AMuxCH0_Disconnect(sensor);
        #endif  /* End `$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS */
                
    #else
        #if (`$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS)
            if ((amuxCh & `$INSTANCE_NAME`_CHANNEL1_FLAG) == 0u)
            {
                `$INSTANCE_NAME`_AMuxCH0_Disconnect(amuxCh);
            } 
            else
            {
                amuxCh &= ~ `$INSTANCE_NAME`_CHANNEL1_FLAG;
                `$INSTANCE_NAME`_AMuxCH1_Disconnect(amuxCh);
            }
            
        #else
            if (sensor < `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0) 
            {
                `$INSTANCE_NAME`_AMuxCH0_Disconnect(sensor);
            } 
            else
            {
                `$INSTANCE_NAME`_AMuxCH1_Disconnect(sensor - `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0);
            }
            
        #endif  /* End `$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS */
        
    #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    
    /* Disconnect from DSI output */
    *`$INSTANCE_NAME`_csTable[port] &= ~mask;
    
    /* Set sensor to inactive state */
    #if (`$INSTANCE_NAME`_CONNECT_INACTIVE_SNS == `$INSTANCE_NAME`_CIS_GND)
        *`$INSTANCE_NAME`_pcTable[sensor] = `$INSTANCE_NAME`_PRT_PC_GND;
    #elif (`$INSTANCE_NAME`_CONNECT_INACTIVE_SNS == `$INSTANCE_NAME`_CIS_HIGHZ)
        *`$INSTANCE_NAME`_pcTable[sensor] = `$INSTANCE_NAME`_PRT_PC_HIGHZ;
    #else
        *`$INSTANCE_NAME`_pcTable[sensor] = `$INSTANCE_NAME`_PRT_PC_SHIELD;
    #endif  /* End (`$INSTANCE_NAME`_CONNECT_INACTIVE_SNS == `$INSTANCE_NAME`_CIS_GND) */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_PreScan
********************************************************************************
*
* Summary:
*  Set required settings, enable sensor, remove Vref from AMUX and start the 
*  scanning process of the sensor.
*
* Parameters:
*  sensor:  Sensor number.
*
* Return:
*  None
*
* Global Variables:
*  `$INSTANCE_NAME`_rbTable[] - used to store pointers to PC pin registers for 
*  every bleed resistor (Rb). Only available when Current Source is External 
*  resistor.
*
*******************************************************************************/
void `$INSTANCE_NAME`_PreScan(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_PreScan")`
{
    /* Set Sensor Settings */
    `$INSTANCE_NAME`_SetScanSlotSettings(sensor);
    
    /* Place disable interrupts here to eliminate influence on start of scanning */
    /* `#START `$INSTANCE_NAME`_PreScan_DisableInt` */
    
    /* `#END` */
    
    /* Resets digital and pre-charge clocks */
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_SYNC_EN;
        
    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE)
            /* Disable Vref from AMux */
            #if (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS)
                `$INSTANCE_NAME`_AMuxCH0_Disconnect(`$INSTANCE_NAME`_AMuxCH0_VREF_CHANNEL);
            #else
                `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
            #endif  /*End (`$INSTANCE_NAME`_VREF_VDAC != `$INSTANCE_NAME`_VREF_OPTIONS) */
            
            /* Enable Sensor */
            `$INSTANCE_NAME`_EnableScanSlot(sensor);
            
        #elif (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK)
            /* Connect IDAC */
            `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_IDAC_CHANNEL);
            
            /* Enable Sensor */
            `$INSTANCE_NAME`_EnableScanSlot(sensor);
                
            /* Disable CapSense Buffer */
            `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
            
        #else
            /* Connect DSI output to Rb */
            *`$INSTANCE_NAME`_rbTable[`$INSTANCE_NAME`_RbCh0_cur] |= `$INSTANCE_NAME`_BYP_MASK;
            
            /* Enable Sensor */
            `$INSTANCE_NAME`_EnableScanSlot(sensor);
             
            /* Disable CapSense Buffer */
            `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
        
        #endif  /* (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE) */
        
    #else
        
        if((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0) != 0u)
        {
            #if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE)
                /* Disable Vref from AMux */
                #if (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS)
                    `$INSTANCE_NAME`_AMuxCH0_Disconnect(`$INSTANCE_NAME`_AMuxCH0_VREF_CHANNEL);
                #else
                    `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
                #endif  /*End (`$INSTANCE_NAME`_VREF_VDAC != `$INSTANCE_NAME`_VREF_OPTIONS) */
                
                /* Enable Sensor */
                `$INSTANCE_NAME`_EnableScanSlot(sensor);
                
            #elif (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK)
                /* Connect IDAC */
                `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_IDAC_CHANNEL);
                
                /* Enable Sensor */
                `$INSTANCE_NAME`_EnableScanSlot(sensor);
                    
                /* Disable Vref from AMux */
                `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
                
            #else
                /* Connect DSI output to Rb */
                *`$INSTANCE_NAME`_rbTable[`$INSTANCE_NAME`_RbCh0_cur] |= `$INSTANCE_NAME`_BYP_MASK;
                
                /* Enable Sensor */
                `$INSTANCE_NAME`_EnableScanSlot(sensor);
                    
                /* Disable Vref from AMux */
                `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
            
            #endif  /* (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE) */
            
        }
        
        if((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1) != 0u)
        {
            sensor += `$INSTANCE_NAME`_TOTAL_SENSOR_COUNT__CH0;
            
            #if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE)
                /* Disable Vref from AMux */
                #if (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS)
                   `$INSTANCE_NAME`_AMuxCH1_Disconnect(`$INSTANCE_NAME`_AMuxCH1_VREF_CHANNEL);
                #else 
                    `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
                #endif  /* End (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS) */
                
                /* Enable Sensor */
                `$INSTANCE_NAME`_EnableScanSlot(sensor);
                
            #elif (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK)
                /* Connect IDAC */
                `$INSTANCE_NAME`_AMuxCH1_Connect(`$INSTANCE_NAME`_AMuxCH1_IDAC_CHANNEL);
                
                /* Enable Sensor */
                `$INSTANCE_NAME`_EnableScanSlot(sensor);
                    
                /* Disable Vref from AMux */
                `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
                
            #else
                /* Connect DSI output to Rb */
                *`$INSTANCE_NAME`_rbTable[`$INSTANCE_NAME`_RbCh1_cur] |= `$INSTANCE_NAME`_BYP_MASK;
                
                /* Enable Sensor */
                `$INSTANCE_NAME`_EnableScanSlot(sensor);
                
                /* Disable Vref from AMux */
                `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_REG &= ~`$INSTANCE_NAME`_CSBUF_ENABLE;
            
            #endif  /* (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE) */
        }
    
    #endif  /* (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN) */
    
    /* Start measurament, pre-charge clocks are running and PRS as well */
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CTRL_START;
    
    /* Place enable interrupts here to eliminate influence on start of scanning */
    /* `#START `$INSTANCE_NAME`_PreScan_EnableInt` */
    
    /* `#END` */
}


#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PostScan
    ********************************************************************************
    *
    * Summary:
    *  Store results of measurament in `$INSTANCE_NAME`_SensorResult[] array,
    *  sets scan sensor in none sampling state, turn off Idac(Current Source IDAC),
    *  disconnect IDAC(Sink mode) or bleed resistor (Rb) and apply Vref on AMUX.
    *  Only one channel designs.
    *
    * Parameters:
    *  sensor:  Sensor number.
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_SensorRaw[] - used to store sensors raw data.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PostScan(uint8 sensor)
    {
        /* Stop Capsensing and rearm sync */
        `$INSTANCE_NAME`_CONTROL_REG &= ~(`$INSTANCE_NAME`_CTRL_START | `$INSTANCE_NAME`_CTRL_SYNC_EN);
        
        /* Read SlotResult from Raw Counter */
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            `$INSTANCE_NAME`_SensorRaw[sensor]  = `$INSTANCE_NAME`_MEASURE_FULL_RANGE - 
                                                      CY_GET_REG16(`$INSTANCE_NAME`_RAW_CH0_COUNTER_PTR);
        #else
            `$INSTANCE_NAME`_SensorRaw[sensor]  = ((uint16) `$INSTANCE_NAME`_RAW_CH0_COUNTER_HI_REG << 8u);
            `$INSTANCE_NAME`_SensorRaw[sensor] |= (uint16) `$INSTANCE_NAME`_RAW_CH0_COUNTER_LO_REG;
            `$INSTANCE_NAME`_SensorRaw[sensor]  = `$INSTANCE_NAME`_MEASURE_FULL_RANGE -
                                                      `$INSTANCE_NAME`_SensorRaw[sensor];
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)*/
        
        /* Disable Sensor */
        `$INSTANCE_NAME`_DisableScanSlot(sensor);
        
        #if(`$INSTANCE_NAME`_CURRENT_SOURCE)
            /* Turn off IDAC */
            `$INSTANCE_NAME`_IdacCH0_SetValue(`$INSTANCE_NAME`_TURN_OFF_IDAC);
            #if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK)
                /* Disconnect IDAC */
                `$INSTANCE_NAME`_AMuxCH0_Disconnect(`$INSTANCE_NAME`_AMuxCH0_IDAC_CHANNEL);
            #endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK) */
        #else
            /* Disconnect DSI output from Rb */
            *`$INSTANCE_NAME`_rbTable[`$INSTANCE_NAME`_RbCh0_cur] &= ~`$INSTANCE_NAME`_BYP_MASK; 
        #endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE)*/
            
        /* Enable Vref on AMUX */
        #if (`$INSTANCE_NAME`_VREF_OPTIONS == `$INSTANCE_NAME`_VREF_VDAC)
            `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_VREF_CHANNEL);
        #else
            `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG |= `$INSTANCE_NAME`_CSBUF_ENABLE;
        #endif  /* End (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS) */
    }
    
#else

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PostScan
    ********************************************************************************
    *
    * Summary:
    *  Store results of measurament in `$INSTANCE_NAME`_SensorResult[] array,
    *  sets scan sensor in none sampling state, turn off Idac(Current Source IDAC),
    *  disconnect IDAC(Sink mode) or bleed resistor (Rb) and apply Vref on AMUX.
    *  Only used for channel 0 in two channes designs.
    *
    * Parameters:
    *  sensor:  Sensor number.
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_SensorRaw[] - used to store sensors raw data.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PostScanCh0(uint8 sensor)
    {
        if (((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0) == 0u) && 
            ((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1) == 0u)) 
        {
            /* Stop Capsensing and rearm sync */
            `$INSTANCE_NAME`_CONTROL_REG &= ~(`$INSTANCE_NAME`_CTRL_START | `$INSTANCE_NAME`_CTRL_SYNC_EN);
        }
        
        /* Read SlotResult from Raw Counter */
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            `$INSTANCE_NAME`_SensorRaw[sensor]  = `$INSTANCE_NAME`_MEASURE_FULL_RANGE - 
                                                      CY_GET_REG16(`$INSTANCE_NAME`_RAW_CH0_COUNTER_PTR);
        #else
            `$INSTANCE_NAME`_SensorRaw[sensor]  = ((uint16) `$INSTANCE_NAME`_RAW_CH0_COUNTER_HI_REG << 8u);
            `$INSTANCE_NAME`_SensorRaw[sensor] |= (uint16) `$INSTANCE_NAME`_RAW_CH0_COUNTER_LO_REG;
            `$INSTANCE_NAME`_SensorRaw[sensor]  = `$INSTANCE_NAME`_MEASURE_FULL_RANGE - 
                                                      `$INSTANCE_NAME`_SensorRaw[sensor];
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)*/
        
        /* Disable Sensor */
        `$INSTANCE_NAME`_DisableScanSlot(sensor);
        
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            /* Turn off IDAC */
            `$INSTANCE_NAME`_IdacCH0_SetValue(`$INSTANCE_NAME`_TURN_OFF_IDAC);
            #if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK)
                /* Disconnect IDAC */
                `$INSTANCE_NAME`_AMuxCH0_Disconnect(`$INSTANCE_NAME`_AMuxCH0_IDAC_CHANNEL);
            #endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK) */
        #else
            /* Disconnect DSI output from Rb */
            *`$INSTANCE_NAME`_rbTable[`$INSTANCE_NAME`_RbCh0_cur] &= ~`$INSTANCE_NAME`_BYP_MASK; 
        #endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE)*/
        
        /* Enable Vref on AMUX */
        #if (`$INSTANCE_NAME`_VREF_OPTIONS == `$INSTANCE_NAME`_VREF_VDAC)
            `$INSTANCE_NAME`_AMuxCH0_Connect(`$INSTANCE_NAME`_AMuxCH0_VREF_CHANNEL);
        #else
            `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG |= `$INSTANCE_NAME`_CSBUF_ENABLE;
        #endif  /* End (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS) */
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_PostScanCh1
    ********************************************************************************
    *
    * Summary:
    *  Store results of measurament in `$INSTANCE_NAME`_SensorResult[] array,
    *  sets scan sensor in none sampling state, turn off Idac(Current Source IDAC), 
    *  disconnect IDAC(Sink mode) or bleed resistor (Rb) and apply Vref on AMUX.
    *  Only used for channel 1 in two channes designs.
    *
    * Parameters:
    *  sensor:  Sensor number.
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_SensorRaw[] - used to store sensors raw data.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_PostScanCh1(uint8 sensor)
    {
        if (((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0) == 0u) && 
            ((`$INSTANCE_NAME`_CONTROL_REG & `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1) == 0u))
        {
            /* Stop Capsensing and rearm sync */
            `$INSTANCE_NAME`_CONTROL_REG &= ~(`$INSTANCE_NAME`_CTRL_START | `$INSTANCE_NAME`_CTRL_SYNC_EN);
        }
        
        /* Read SlotResult from Raw Counter */
        #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
            `$INSTANCE_NAME`_SensorRaw[sensor]  = `$INSTANCE_NAME`_MEASURE_FULL_RANGE - 
                                                      CY_GET_REG16(`$INSTANCE_NAME`_RAW_CH1_COUNTER_PTR);
        #else
            `$INSTANCE_NAME`_SensorRaw[sensor]  = ((uint16) `$INSTANCE_NAME`_RAW_CH1_COUNTER_HI_REG << 8u);
            `$INSTANCE_NAME`_SensorRaw[sensor] |= (uint16) `$INSTANCE_NAME`_RAW_CH1_COUNTER_LO_REG;
            `$INSTANCE_NAME`_SensorRaw[sensor]  = `$INSTANCE_NAME`_MEASURE_FULL_RANGE - 
                                                      `$INSTANCE_NAME`_SensorRaw[sensor];
        #endif  /* End (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)*/
        
        /* Disable Sensor */
        `$INSTANCE_NAME`_DisableScanSlot(sensor);
        
        #if (`$INSTANCE_NAME`_CURRENT_SOURCE)
            /* Turn off IDAC */
            `$INSTANCE_NAME`_IdacCH1_SetValue(`$INSTANCE_NAME`_TURN_OFF_IDAC);
            #if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK)
                /* Disconnect IDAC */
                `$INSTANCE_NAME`_AMuxCH1_Disconnect(`$INSTANCE_NAME`_AMuxCH1_IDAC_CHANNEL);
            #endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK) */
        #else
            /* Disconnect DSI output from Rb */
            *`$INSTANCE_NAME`_rbTable[`$INSTANCE_NAME`_RbCh1_cur] &= ~`$INSTANCE_NAME`_BYP_MASK; 
        #endif  /* End (`$INSTANCE_NAME`_CURRENT_SOURCE)*/

        /* Enable Vref on AMUX */
        #if (`$INSTANCE_NAME`_VREF_OPTIONS == `$INSTANCE_NAME`_VREF_VDAC)
            `$INSTANCE_NAME`_AMuxCH1_Connect(`$INSTANCE_NAME`_AMuxCH1_VREF_CHANNEL);
        #else
            `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_REG |= `$INSTANCE_NAME`_CSBUF_ENABLE;
        #endif  /* End (`$INSTANCE_NAME`_VREF_VDAC == `$INSTANCE_NAME`_VREF_OPTIONS) */
    }
    
#endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */


#if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB)
    /*******************************************************************************
    * Function Name:  `$INSTANCE_NAME`_InitRb
    ********************************************************************************
    *
    * Summary:
    *  Sets all Rbleed resistor to High-Z mode. The first Rbleed resistor is active
    *  while next measure.
    *  This function is available only if Current Source is External Resistor.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    ********************************************************************************/
    void `$INSTANCE_NAME`_InitRb(void) `=ReentrantKeil($INSTANCE_NAME . "_InitRb")`
    {
        uint8 i;
        
        /* Disable all Rb */
        for(i=0; i < `$INSTANCE_NAME`_TOTAL_RB_NUMBER; i++)
        {
            /* Make High-Z */
            *`$INSTANCE_NAME`_rbTable[i] = `$INSTANCE_NAME`_PRT_PC_HIGHZ;
        }
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetRBleed
    ********************************************************************************
    *
    * Summary:
    *  Sets the pin to use for the bleed resistor (Rb) connection. This function
    *  can be called at runtime to select the current Rb pin setting from those 
    *  defined customizer. The function overwrites the component parameter setting. 
    *  This function is available only if Current Source is External Resistor.
    * 
    * Parameters:
    *  rbleed:  Ordering number for bleed resistor terminal defined in CapSense
    *  customizer.
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_RbCh0_cur - used to store current number of active 
    *  bleed resistor (Rb) of channel 0.
    *  `$INSTANCE_NAME`_RbCh1_cur - used to store current number of active 
    *  bleed resistor (Rb) of channel 1.
    *  The active bleed resistor (Rb) pin will be used while next measurement  
    *  cycle.
    *
    * Reentrant:
    *  No
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetRBleed(uint8 rbleed)
    {
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN)
            `$INSTANCE_NAME`_RbCh0_cur = rbleed;
            
        #else
            if(rbleed < `$INSTANCE_NAME`_TOTAL_RB_NUMBER__CH0)
            {
                `$INSTANCE_NAME`_RbCh0_cur = rbleed;
            }
            else
            {
                `$INSTANCE_NAME`_RbCh1_cur = (rbleed - `$INSTANCE_NAME`_TOTAL_RB_NUMBER__CH0);   
            }
    
        #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */ 
    }
#endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */ 


#if (`$INSTANCE_NAME`_CURRENT_SOURCE)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IdacCH0_Init
    ********************************************************************************
    *
    * Summary:
    *  Sets VIDAC8 block to: IDAC mode, current direction (Source/Sink), current 
    *  control from UDB (Ioff), IDAC range (32uA/255uA/2048uA) and IDAC value 0.
    *  This function only available when Current Source is IDAC.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_IdacCH0_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_Init")`
    {
        /* Set I mode of operation */
        `$INSTANCE_NAME`_IdacCH0_CR0_REG = `$INSTANCE_NAME`_IDAC_MODE_I;
        
        /* Set Source/Sink direction and source of control */
        `$INSTANCE_NAME`_IdacCH0_CR1_REG = `$INSTANCE_NAME`_IdacCH0_IDIR | `$INSTANCE_NAME`_IDAC_IDIR_CTL_UDB;
            
        /* Set Range and IDAC value */
        `$INSTANCE_NAME`_IdacCH0_SetRange(`$INSTANCE_NAME`_IDAC_RANGE_VALUE);
        `$INSTANCE_NAME`_IdacCH0_SetValue(`$INSTANCE_NAME`_TURN_OFF_IDAC);       
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IdacCH0_Enable
    ********************************************************************************
    *
    * Summary:
    *  Turn on IDAC8.
    *  This function only available when Current Source is IDAC.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_IdacCH0_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_Enable")`
    {
        /* Set I mode of operation */
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_IdacCH0_CR0_REG |= `$INSTANCE_NAME`_IDAC_MODE_I;
        #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) */
        
        /* Enable power to DAC */
        `$INSTANCE_NAME`_IdacCH0_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_IdacCH0_ACT_PWR_EN;
        `$INSTANCE_NAME`_IdacCH0_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_IdacCH0_STBY_PWR_EN;
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IdacCH0_Stop
    ********************************************************************************
    *
    * Summary:
    *  Powers down IDAC8 to lowest power state.
    *  This function only available when Current Source is IDAC.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_IdacCH0_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_Stop")`
    {
        /* Set to V mode */
        #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_IdacCH0_CR0_REG &= ~`$INSTANCE_NAME`_IDAC_MODE_I;
        #endif   /* End (`$INSTANCE_NAME`_PSOC3_ES2 ||`$INSTANCE_NAME`_PSOC5_ES1) */
        
        /* Disble power to DAC */
        `$INSTANCE_NAME`_IdacCH0_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_IdacCH0_ACT_PWR_EN;
        `$INSTANCE_NAME`_IdacCH0_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_IdacCH0_STBY_PWR_EN;
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IdacCH0_SetValue
    ********************************************************************************
    *
    * Summary:
    *  Sets IDAC8 value.
    *  This function only available when Current Source is IDAC.
    *
    * Parameters:
    *  value:  Sets DAC value between 0 and 255.
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_IdacCH0_SetValue(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_SetValue")`
    {
        /* Set Value */
        `$INSTANCE_NAME`_IdacCH0_DATA_REG = value;
        
        #if (`$INSTANCE_NAME`_PSOC3_ES2 ||`$INSTANCE_NAME`_PSOC5_ES1)
            `$INSTANCE_NAME`_IdacCH0_DATA_REG = value;
        #endif  /* End  (`$INSTANCE_NAME`_PSOC3_ES2 ||`$INSTANCE_NAME`_PSOC5_ES1) */
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IdacCH0_DacTrim
    ********************************************************************************
    *
    * Summary:
    *  Sets the trim value for the given range.
    *  This function only available when Current Source is IDAC.
    *
    * Parameters:
    *  None
    * 
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_IdacCH0_DacTrim(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_DacTrim")`
    {
        uint8 mode;
        
        mode = ((`$INSTANCE_NAME`_IdacCH0_CR0_REG & `$INSTANCE_NAME`_IDAC_RANGE_MASK) >> 1u);
        
        if((`$INSTANCE_NAME`_IDAC_IDIR_MASK & `$INSTANCE_NAME`_IdacCH0_CR1_REG) == `$INSTANCE_NAME`_IDAC_IDIR_SINK)
        {
            mode++;
        }
        
        `$INSTANCE_NAME`_IdacCH0_TR_REG = CY_GET_XTND_REG8((uint8 *)(`$INSTANCE_NAME`_IdacCH0_DAC_TRIM_BASE + mode));
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_IdacCH0_SetRange
    ********************************************************************************
    *
    * Summary:
    *  Sets current range.
    *  This function only available when Current Source is IDAC.
    *
    * Parameters:
    *  range:  Sets on of three valid ranges.
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_IdacCH0_SetRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH0_SetRange")`
    {
        /* Clear existing mode */
        `$INSTANCE_NAME`_IdacCH0_CR0_REG &= ~`$INSTANCE_NAME`_IDAC_RANGE_MASK;
        
        /* Set Range */
        `$INSTANCE_NAME`_IdacCH0_CR0_REG |= (range & `$INSTANCE_NAME`_IDAC_RANGE_MASK);
        `$INSTANCE_NAME`_IdacCH0_DacTrim();
    }

    #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_IdacCH1_Init
        ********************************************************************************
        *
        * Summary:
        *  Sets VIDAC8 block to: IDAC mode, current direction (Source/Sink), current 
        *  control from UDB (Ioff), IDAC range (32uA/255uA/2048uA) and IDAC value 0.
        *  This function only available when Current Source is IDAC and two channels 
        *  design is selected.
        *
        * Parameters:
        *  None
        *
        * Return:
        *  None
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_IdacCH1_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_Init")`
        {
            /* Set I mode of operation */
            `$INSTANCE_NAME`_IdacCH1_CR0_REG = `$INSTANCE_NAME`_IDAC_MODE_I;
            
            /* Set Source/Sink direction and source of control */
            `$INSTANCE_NAME`_IdacCH1_CR1_REG = (`$INSTANCE_NAME`_IdacCH1_IDIR | `$INSTANCE_NAME`_IDAC_IDIR_CTL_UDB);
            
            /* Set Range and IDAC value */
            `$INSTANCE_NAME`_IdacCH1_SetRange(`$INSTANCE_NAME`_IDAC_RANGE_VALUE);
            `$INSTANCE_NAME`_IdacCH1_SetValue(`$INSTANCE_NAME`_TURN_OFF_IDAC);
        }
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_IdacCH1_Enable
        ********************************************************************************
        *
        * Summary:
        *  Turn on IDAC8.
        *  This function only available when Current Source is IDAC and two channels 
        *  design is selected.
        *
        * Parameters:
        *  None
        *
        * Return:
        *  None
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_IdacCH1_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_Enable")`
        {
            /* Set I mode of operation */
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_IdacCH1_CR0_REG |= `$INSTANCE_NAME`_IDAC_MODE_I;
            #endif   /* End (`$INSTANCE_NAME`_PSOC3_ES2 ||`$INSTANCE_NAME`_PSOC5_ES1) */
                      
            /* Enable power to DAC */
            `$INSTANCE_NAME`_IdacCH1_ACT_PWRMGR_REG  |= `$INSTANCE_NAME`_IdacCH1_ACT_PWR_EN;
            `$INSTANCE_NAME`_IdacCH1_STBY_PWRMGR_REG |= `$INSTANCE_NAME`_IdacCH1_STBY_PWR_EN;
        }
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_IdacCH1_Stop
        ********************************************************************************
        *
        * Summary:
        *  Powers down IDAC8 to lowest power state.
        *  This function only available when Current Source is IDAC and two channels 
        *  design is selected.
        *
        * Parameters:
        *   None
        *
        * Return:
        *  None
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_IdacCH1_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_Stop")`
        {
            /* Set to V mode */
            #if  (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_IdacCH1_CR0_REG &= ~`$INSTANCE_NAME`_IDAC_MODE_I;
            #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 ||`$INSTANCE_NAME`_PSOC5_ES1) */
            
            /* Disable power from DAC */
            `$INSTANCE_NAME`_IdacCH1_ACT_PWRMGR_REG  &= ~`$INSTANCE_NAME`_IdacCH1_ACT_PWR_EN;
            `$INSTANCE_NAME`_IdacCH1_STBY_PWRMGR_REG &= ~`$INSTANCE_NAME`_IdacCH1_STBY_PWR_EN;
        }
        
            
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_IdacCH1_SetValue
        ********************************************************************************
        *
        * Summary:
        *  Sets IDAC8 value.
        *  This function only available when Current Source is IDAC and two channels 
        *  design is selected.
        *
        * Parameters:
        *  value:  Sets DAC value between 0 and 255.
        *
        * Return:
        *  None
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_IdacCH1_SetValue(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_SetValue")`
        {
            /* Set Value */
            `$INSTANCE_NAME`_IdacCH1_DATA_REG = value;
            
            #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
                `$INSTANCE_NAME`_IdacCH1_DATA_REG = value;
            #endif  /* End (`$INSTANCE_NAME`_PSOC3_ES2 ||`$INSTANCE_NAME`_PSOC5_ES1) */
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_IdacCH1_DacTrim
        ********************************************************************************
        *
        * Summary:
        *  Sets the trim value for the given range.
        *  This function only available when Current Source is IDAC and two channels 
        *  design is selected.
        *
        * Parameters:
        *  None
        * 
        * Return:
        *  None
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_IdacCH1_DacTrim(void) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_DacTrim")`
        {
            uint8 mode;
            
            mode = ((`$INSTANCE_NAME`_IdacCH1_CR0_REG & `$INSTANCE_NAME`_IDAC_RANGE_MASK) >> 1u);
            
            if((`$INSTANCE_NAME`_IDAC_IDIR_MASK & `$INSTANCE_NAME`_IdacCH1_CR1_REG) == `$INSTANCE_NAME`_IDAC_IDIR_SINK)
            {
                mode++;
            }
            
            `$INSTANCE_NAME`_IdacCH1_TR_REG = 
                                 CY_GET_XTND_REG8((uint8 *)(`$INSTANCE_NAME`_IdacCH1_DAC_TRIM_BASE + mode));
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_IdacCH1_SetRange
        ********************************************************************************
        *
        * Summary:
        *  Sets current range.
        *  This function only available when Current Source is IDAC and two channels 
        *  design is selected.
        *
        * Parameters:
        *  range:  Sets on of three valid ranges.
        *
        * Return:
        *  None
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_IdacCH1_SetRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_IdacCH1_SetRange")`
        {
            /* Clear existing mode */
            `$INSTANCE_NAME`_IdacCH1_CR0_REG &= ~`$INSTANCE_NAME`_IDAC_RANGE_MASK;
            
            /* Set Range */
            `$INSTANCE_NAME`_IdacCH1_CR0_REG |= (range & `$INSTANCE_NAME`_IDAC_RANGE_MASK);
            `$INSTANCE_NAME`_IdacCH1_DacTrim();
        }
        
    #endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */ 

#endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */ 


#if (`$INSTANCE_NAME`_CURRENT_SOURCE)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetIdacRange
    ********************************************************************************
    *
    * Summary:
    *  Sets DAC Range for one or two channels, depends on design type.
    *
    * Parameters:
    *  range:  Sets on of three valid ranges.
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetIdacRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_SetIdacRange")`  
    {
        `$INSTANCE_NAME`_IdacCH0_SetRange(range);
        #if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
            `$INSTANCE_NAME`_IdacCH1_SetRange(range);
        #endif  /* End `$INSTANCE_NAME`_DESIGN_TYPE */
    }
#endif /* End `$INSTANCE_NAME`_CURRENT_SOURCE */     


#if (`$INSTANCE_NAME`_PRESCALER_OPTIONS)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetPrescaler
    ********************************************************************************
    *
    * Summary:
    *  Sets analog switch divider.
    *
    * Parameters:
    *  prescaler:  Sets prescaler divider values.
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetPrescaler(uint8 prescaler) `=ReentrantKeil($INSTANCE_NAME . "_SetPrescaler")`
    {
        /* Set Prescaler */
        #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
            `$INSTANCE_NAME`_PRESCALER_PERIOD_REG = prescaler;
            `$INSTANCE_NAME`_PRESCALER_COMPARE_REG = (prescaler >> 0x01u);
            
        #elif (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_FF)
            CY_SET_REG16(`$INSTANCE_NAME`_PRESCALER_PERIOD_PTR, (uint16) prescaler);
            CY_SET_REG16(`$INSTANCE_NAME`_PRESCALER_COMPARE_PTR, (uint16) (prescaler >> 0x01u));
            
        #else
            /* Do nothing = config without prescaler */
        #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetPrescaler
    ********************************************************************************
    *
    * Summary:
    *  Gets analog switch divider.
    *
    * Parameters:
    *  None
    *
    * Return:
    *   Returns the prescaler divider value.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetPrescaler(void) `=ReentrantKeil($INSTANCE_NAME . "_GetPrescaler")`
    {
        uint8 prescaler = 0u;

        /* Get Prescaler */
        #if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
            prescaler = `$INSTANCE_NAME`_PRESCALER_PERIOD_REG;
            
        #elif (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_FF)
            prescaler = (uint8) CY_GET_REG16(`$INSTANCE_NAME`_PRESCALER_PERIOD_PTR);
            
        #else
            /* Do nothing = config without prescaler */
        #endif  /* End (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */
        
        return prescaler;
    }
#endif  /* End `$INSTANCE_NAME`_PRESCALER_OPTIONS */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetScanSpeed
********************************************************************************
*
* Summary:
*  Sets ScanSpeed divider.
*
* Parameters:
*  scanspeed:  Sets ScanSpeed divider.
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetScanSpeed(uint8 scanspeed) `=ReentrantKeil($INSTANCE_NAME . "_SetScanSpeed")`
{
    `$INSTANCE_NAME`_SCANSPEED_PERIOD_REG = scanspeed; 
}


/* [] END OF FILE */
