/*******************************************************************************
* File Name: `$INSTANCE_NAME`_INT.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the code that operates during the PowerMonitor interrupt 
*  service routine.  
*
* Note:
*
*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_PM_AMux_Voltage.h"
#include "`$INSTANCE_NAME`_PM_AMux_Current.h"
#include "`$INSTANCE_NAME`_ADC.h"


/*******************************************************************************
* Global variables
*******************************************************************************/

/*******************************************************************************
*   `$INSTANCE_NAME`_adcConvNow          - Conversion in progress
*   `$INSTANCE_NAME`_adcConvNext       - Next conversion 
*   `$INSTANCE_NAME`_adcConvNextPreCal - Holds old Next before switching to 
                                         calibration state
*   `$INSTANCE_NAME`_adcConvCalType    - Calibration type is in grogress
*******************************************************************************/
volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvNow;
volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvNext;
volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvNextPreCal;
volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvCalType;


/*******************************************************************************
*  Variables to hold the raw calibration values. The raw readings need to be 
*  subsequently processed to yield the proper calibration adjustments.
*******************************************************************************/
/*                                                 ~ VALUE, RANGE Description */
volatile int16 CYXDATA `$INSTANCE_NAME`_adcZeroCalRawCfg1;  /* ~     0, 2048 zero    */
volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg1;  /* ~ -1024, 2048 DSM Vref */
volatile int16 CYXDATA `$INSTANCE_NAME`_adcSCCalRawCfg1; /* ~ -2048, 2048 PGA   */
volatile int16 CYXDATA `$INSTANCE_NAME`_adcZeroCalRawCfg2;  /* ~     0,   64 zero  */
volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg2a; 
volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg2b; 
volatile int16 CYXDATA `$INSTANCE_NAME`_adcZeroCalRawCfg3;  /* ~     0, 1024 zero    */
volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg3;  /* ~ -2048, 1024 DSM Vref */


/*******************************************************************************
* `$INSTANCE_NAME`_ADC_RAWVOLTS_STRUCT - ADC votage reading structure 
*                                    (one struct per channel)
*        rawVoltage[] = array of raw ADC readings. .
*******************************************************************************/
typedef struct
{
    /* Raw ADC samples      */
    volatile int16 rawVoltage[`$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE];  
} `$INSTANCE_NAME`_ADC_RAWVOLTS_STRUCT;

/* Array to hold ADC raw values of voltage channels of each converter */
static `$INSTANCE_NAME`_ADC_RAWVOLTS_STRUCT CYXDATA `$INSTANCE_NAME`_voltFilt[`$INSTANCE_NAME`_NUM_CONVERTERS];

/* Array to hold filtered ADC value for each voltage inputs */
`$INSTANCE_NAME`_ADC_CTL_STRUCT  CYXDATA `$INSTANCE_NAME`_voltCtl[`$INSTANCE_NAME`_NUM_CONVERTERS];


#if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
/*******************************************************************************
* `$INSTANCE_NAME`_ADC_RAWCURRENT_STRUCT - ADC votage reading structure 
*                                    (one struct per channel)
*        rawVoltage[] = array of raw ADC readings. .
*******************************************************************************/
typedef struct
{
    /* Raw ADC samples      */
    volatile int16 rawVoltage[`$INSTANCE_NAME`_CURRENT_FILTER_SIZE];  
} `$INSTANCE_NAME`_ADC_RAWCURRENT_STRUCT;

/* Array to hold ADC raw values of current channels of each converter */
static `$INSTANCE_NAME`_ADC_RAWCURRENT_STRUCT CYXDATA `$INSTANCE_NAME`_ampFilt[`$INSTANCE_NAME`_NUM_CURRENT_SOURCES];

/* Array to hold filtered ADC value for each current channels */
`$INSTANCE_NAME`_ADC_CTL_STRUCT  CYXDATA `$INSTANCE_NAME`_ampCtl[`$INSTANCE_NAME`_NUM_CURRENT_SOURCES];

#endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */


#if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
/*******************************************************************************
* `$INSTANCE_NAME`_ADC_RAWAUXVOLTS_STRUCT - ADC votage reading structure 
*                                    (one struct per channel)
*        rawVoltage[] = array of raw ADC readings. .
*******************************************************************************/
typedef struct
{
    /* Raw ADC samples      */
    volatile int16 rawVoltage[`$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE];  
} `$INSTANCE_NAME`_ADC_RAWAUXVOLTS_STRUCT;

/* Array to hold ADC raw values of aux voltage channels of each converter */
static `$INSTANCE_NAME`_ADC_RAWAUXVOLTS_STRUCT CYXDATA `$INSTANCE_NAME`_auxVoltFilt[`$INSTANCE_NAME`_NUM_AUX_INPUTS];

/* Array to hold filtered ADC value for each aux inputs */
`$INSTANCE_NAME`_ADC_CTL_STRUCT  CYXDATA `$INSTANCE_NAME`_auxVoltCtl[`$INSTANCE_NAME`_NUM_AUX_INPUTS];
#endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */

/* Supervisor_adcWarnWin[] = table of Min/Max values (scaled to filtered sum) */
`$INSTANCE_NAME`_WARNWIN_STRUCT CYXDATA `$INSTANCE_NAME`_warnWin [`$INSTANCE_NAME`_NUM_CONVERTERS];
`$INSTANCE_NAME`_FAULTWIN_STRUCT CYXDATA `$INSTANCE_NAME`_faultWin [`$INSTANCE_NAME`_NUM_CONVERTERS];

/* last voltage, current annd auxiliary voltage channels */
uint8 CYDATA `$INSTANCE_NAME`_lastVoltageChan;
uint8 CYDATA `$INSTANCE_NAME`_lastCurrentChan;
uint8 CYDATA `$INSTANCE_NAME`_lastAuxVoltChan;

/* Array to hold the pgood status */
#if (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_INDIVIDUAL)
    static uint8 CYPDATA `$INSTANCE_NAME`_faultStatus[`$INSTANCE_NAME`_NUM_CONVERTERS];
#endif /* (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_INDIVIDUAL) */

#if ((`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) || (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0))
    static uint8 CYDATA `$INSTANCE_NAME`_index = 0u;
#endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */

#if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
    static uint8 CYDATA `$INSTANCE_NAME`_auxIndex = 0u;
#endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */


/* variables to store the DSM routing registers */
static uint8 `$INSTANCE_NAME`_keep_CYREG_DSM0_SW0; /* vp_ag[7:0] */
static uint8 `$INSTANCE_NAME`_keep_CYREG_DSM0_SW2; /* vp_abus[2],[0] */
/* _[vn_vssa][vn_vref]_ _[vp_vssa]_[vp_amx] */
static uint8 `$INSTANCE_NAME`_keep_CYREG_DSM0_SW3; 
static uint8 `$INSTANCE_NAME`_keep_CYREG_DSM0_SW4; /* vn_ag[7,5,3,1] */
static uint8 `$INSTANCE_NAME`_keep_CYREG_DSM0_SW6; /* vn_abus[3,1] */


/*******************************************************************************
* ADC_CONFIGURATION macro - Ensures the desired ADC Configuration is selected
*
* ADC_CONFIG_PUSH_NEW(newConfig) - Set new config and copy as being pushed
* ADC_CONFIG_POP     ()          - Pop prior pushed config
* ADC_CONFIG_PUSH_OLD(newConfig) - Push old config and set a new config
*
*******************************************************************************/
static uint8 CYDATA `$INSTANCE_NAME`_adcConfig = 0xffu;
static uint8 CYDATA `$INSTANCE_NAME`_adcConvDoneConfig = 0xffu;
static uint8 CYDATA `$INSTANCE_NAME`_adcConfigCalPush;

#define `$INSTANCE_NAME`_ADC_CONFIG_SET(newConfig)                                     \
    do {                                                                               \
        if (`$INSTANCE_NAME`_adcConfig != newConfig)                                   \
        {                                                                          \
            `$INSTANCE_NAME`_adcConfig = newConfig;                                    \
            `$INSTANCE_NAME`_ADC_SelectConfiguration(newConfig, 0);                    \
            CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR );  \
            `$INSTANCE_NAME`_ADC_Start();                                              \
        }                                                                              \
    } while(0)

#define `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(newConfig)                  \
    do {                                                                 \
        `$INSTANCE_NAME`_ADC_CONFIG_SET(newConfig);                      \
        `$INSTANCE_NAME`_adcConfigCalPush = `$INSTANCE_NAME`_adcConfig;  \
        `$INSTANCE_NAME`_adcCfgPopPend = `$INSTANCE_NAME`_CYTRUE;        \
    } while (0)

#define `$INSTANCE_NAME`_ADC_CONFIG_POP()                                   \
    do {                                                                    \
        `$INSTANCE_NAME`_ADC_CONFIG_SET(`$INSTANCE_NAME`_adcConfigCalPush); \
        `$INSTANCE_NAME`_adcCfgPopPend = `$INSTANCE_NAME`_CYFALSE;          \
    } while(0)

#define `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(newConfig)                  \
    do {                                                                 \
        `$INSTANCE_NAME`_adcConfigCalPush = `$INSTANCE_NAME`_adcConfig;  \
        `$INSTANCE_NAME`_ADC_CONFIG_SET(newConfig);                      \
        `$INSTANCE_NAME`_adcCfgPopPend = `$INSTANCE_NAME`_CYTRUE;        \
    } while(0)


/*******************************************************************************
* Used when punctuated "calibration" didn't really mess with the analog
* switch routing (like when it's the first IRQ after reset).
*******************************************************************************/
static CYBIT `$INSTANCE_NAME`_justReset;
/* AMUX Channel select variable */
static uint8 CYDATA `$INSTANCE_NAME`_amuxChan;
/* Bit variable to decide whether to restore the DSM registers or not */
static CYBIT `$INSTANCE_NAME`_dsmRegRestorePend = `$INSTANCE_NAME`_CYFALSE;
static CYBIT `$INSTANCE_NAME`_adcCfgPopPend     = `$INSTANCE_NAME`_CYFALSE;

#if (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL)
    static CYBIT `$INSTANCE_NAME`_faultDetected;
#endif /* (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL) */
volatile int16 CYXDATA * CYDATA `$INSTANCE_NAME`_pCalStore = 0;


/******************************************************************************
*    Local APIs
******************************************************************************/
static void `$INSTANCE_NAME`_SaveDsmRegsAndIsolate   (void) \
                           `=ReentrantKeil($INSTANCE_NAME . "_SaveDsmRegsAndIsolate")`;
static void `$INSTANCE_NAME`_RestoreDsmRegs          (void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreDsmRegs")`;


/*******************************************************************************
* Function Name: Supervisor_AdcIsrStart()
********************************************************************************
*
* Summary:
*  Set-up and begin the autonomous ADC ISR thread. Starts the ADC operation by
*  initially setting the software interrupt and excecuting the ISR routine.
*
* Parameters:
*  None
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_IsrStart(void) `=ReentrantKeil($INSTANCE_NAME . "_IsrStart")`
{
    /* Disable and set the interrupt priority */
    CyIntDisable      (`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    CyIntSetPriority  (`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER);

    /* Initialize the variables with values */
    `$INSTANCE_NAME`_adcConvNow       = `$INSTANCE_NAME`_STATE_CAL;
    `$INSTANCE_NAME`_adcConvNext    = `$INSTANCE_NAME`_STATE_CAL;
    `$INSTANCE_NAME`_adcConvCalType = `$INSTANCE_NAME`_CAL_START;
    
    /* Setting the interrupt vecotr for software interrupt and enabling it */
    CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR);  
    CyIntSetPending   (`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
    CyIntEnable       (`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
}


/*****************************************************************************
* Function Name: `$INSTANCE_NAME`_ISR
******************************************************************************
*
* Summary:
*  Handle Interrupt Service Routine.  
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
*****************************************************************************/
CY_ISR(`$INSTANCE_NAME`_ISR)
{
    int16 CYDATA rawVal;
    uint8 CYDATA convDone;
    int32 rawFiltVal = 0;
    #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
        uint8 CYDATA iChanNum;
    #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */

    /* Enable the global interrupts */
    CyGlobalIntEnable;
    
    /* Collect the raw reading */
    rawVal = `$INSTANCE_NAME`_ADC_GetResult16();
    
    /* Update the state of flags */
    convDone = `$INSTANCE_NAME`_adcConvNow;
    `$INSTANCE_NAME`_adcConvNow = `$INSTANCE_NAME`_adcConvNext;
    `$INSTANCE_NAME`_adcConvNext++;
    `$INSTANCE_NAME`_adcConvDoneConfig = `$INSTANCE_NAME`_adcConfig;
    
    /* If DSM registers need restoring, do it BEFORE any FastSelects() */
    if (`$INSTANCE_NAME`_dsmRegRestorePend == `$INSTANCE_NAME`_CYTRUE)
    {
        `$INSTANCE_NAME`_RestoreDsmRegs();
    }
    
    /* If ADC configuration needs restoring, do it before proceeding */
    if (`$INSTANCE_NAME`_adcCfgPopPend == `$INSTANCE_NAME`_CYTRUE)
    {
        `$INSTANCE_NAME`_ADC_CONFIG_POP();
    }

    /* Based on the new adc reading, setup either special adc
    *  configuation/routing for next reading or just take the next
    *  sequential measurement 
    */
    
    if (`$INSTANCE_NAME`_adcConvNow < `$INSTANCE_NAME`_lastVoltageChan)
    {
        `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_adcConvNow);
        /* Update AmuxChan flag */
        if (`$INSTANCE_NAME`_VoltageType[`$INSTANCE_NAME`_adcConvNow] == `$INSTANCE_NAME`_VOLTAGE_TYPE_DIFF)
        {
            `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_adcConvNow);
        }
        else
        {
            if ((`$INSTANCE_NAME`_VoltageType[`$INSTANCE_NAME`_adcConvNow] != \
                `$INSTANCE_NAME`_VoltageType[(`$INSTANCE_NAME`_adcConvNow - 1)]) ||
                 (convDone == `$INSTANCE_NAME`_STATE_CAL))
            {
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
            }
        }
    }
    else if (`$INSTANCE_NAME`_adcConvNow == `$INSTANCE_NAME`_lastVoltageChan)
    {
        #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
            `$INSTANCE_NAME`_index = 0u;
            /* Check to see whether current type is differential or not. If it is
            / differential skip amux channels. */
            while (`$INSTANCE_NAME`_CurrentType[`$INSTANCE_NAME`_index] == `$INSTANCE_NAME`_CURRENT_TYPE_NA)
            {
                `$INSTANCE_NAME`_index++;
            }

            /* Set the proper ADC cofiguration */
            if (`$INSTANCE_NAME`_CurrentType[`$INSTANCE_NAME`_index] == `$INSTANCE_NAME`_CURRENT_TYPE_DIRECT)
            {
                /* Change the ADC configuration */
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_LOW);
                /* Update AmuxChan flag */
                `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_index;
                /* Select the proper AMux channels */
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_amuxChan);
            }
            else
            {
                `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_CSA_IN_CHAN + `$INSTANCE_NAME`_index;
                /* Select the proper AMux channels */
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
            }
        #elif (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
            `$INSTANCE_NAME`_index = 0u;
            /* Select the proper channel */
            `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_AUX_IN_CHAN);
            if (`$INSTANCE_NAME`_AuxVoltageType[0] != `$INSTANCE_NAME`_AUX_VOLTAGE_SINGLE)
            {
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_AUX_IN_CHAN);
            }
            else
            {
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
            }
            /* Update AmuxChan flag */
            `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_AUX_IN_CHAN;
            `$INSTANCE_NAME`_auxIndex = `$INSTANCE_NAME`_AUX_IN_CHAN;
            /* Set the proper ADC configuration */
            if ((`$INSTANCE_NAME`_AuxVoltageType[0] == `$INSTANCE_NAME`_AUX_VOLTAGE_64MV_DIFF) ||
                (`$INSTANCE_NAME`_AuxVoltageType[0] == `$INSTANCE_NAME`_AUX_VOLTAGE_128MV_DIFF))
            {
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_LOW);
            }
        #else
            `$INSTANCE_NAME`_adcConvNow = `$INSTANCE_NAME`_ISR_STATE_0;
            `$INSTANCE_NAME`_adcConvNext = `$INSTANCE_NAME`_ISR_STATE_0 + 1;
             /* Set the proper AMux Channel */
            `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_adcConvNow);
            /* Update AmuxChan flag */
            `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_adcConvNow;
            if (`$INSTANCE_NAME`_VoltageType[0] == `$INSTANCE_NAME`_VOLTAGE_TYPE_DIFF)
            {
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_amuxChan);
            }
            else
            {
                if ((`$INSTANCE_NAME`_VoltageType[0] != \
                     `$INSTANCE_NAME`_VoltageType[(`$INSTANCE_NAME`_lastVoltageChan - 1)]) ||
                    (convDone == `$INSTANCE_NAME`_STATE_CAL))
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
                }
            }
        #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */
    }
    #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
        else if (`$INSTANCE_NAME`_adcConvNow == `$INSTANCE_NAME`_lastCurrentChan)
        {
            #if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
        
                /* Set the AMux channel */
                `$INSTANCE_NAME`_auxIndex = `$INSTANCE_NAME`_AUX_IN_CHAN;
                /* Update AmuxChan flag */
                `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_auxIndex;

                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
                
                if (`$INSTANCE_NAME`_AuxVoltageType[0] == `$INSTANCE_NAME`_AUX_VOLTAGE_SINGLE)
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
                }
                else
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_amuxChan);
                }
                /* Set the proper ADC configuration */
                if ((`$INSTANCE_NAME`_AuxVoltageType[0] == `$INSTANCE_NAME`_AUX_VOLTAGE_64MV_DIFF) ||
                    (`$INSTANCE_NAME`_AuxVoltageType[0] == `$INSTANCE_NAME`_AUX_VOLTAGE_128MV_DIFF))
                {
                    `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_LOW);
                }
                else 
                {
                    `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_2048);
                }
            #else
                `$INSTANCE_NAME`_adcConvNow = `$INSTANCE_NAME`_ISR_STATE_0;
                `$INSTANCE_NAME`_adcConvNext = `$INSTANCE_NAME`_ISR_STATE_0 + 1;
                
                /* Set the proper ADC configuration */
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_2048);
                
                /* Update AmuxChan flag */
                `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_adcConvNow;
                /* Select the proper AMux Channel */
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
                
                if (`$INSTANCE_NAME`_VoltageType[0] == `$INSTANCE_NAME`_VOLTAGE_TYPE_SINGLE)
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
                }
                else
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_amuxChan);
                }
            #endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */
           `$INSTANCE_NAME`_index = 0u;
        }
        else if ((`$INSTANCE_NAME`_adcConvNow > `$INSTANCE_NAME`_lastVoltageChan) && 
                 (`$INSTANCE_NAME`_adcConvNow < `$INSTANCE_NAME`_lastCurrentChan))
        {
            `$INSTANCE_NAME`_index++;
        
            while(`$INSTANCE_NAME`_CurrentType[`$INSTANCE_NAME`_index] == `$INSTANCE_NAME`_CURRENT_TYPE_NA)
            {
                `$INSTANCE_NAME`_index++;
            }
            /* Set the proper ADC configuration */
            if (`$INSTANCE_NAME`_CurrentType[`$INSTANCE_NAME`_index] == `$INSTANCE_NAME`_CURRENT_TYPE_DIRECT)
            {
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_LOW);
                /* Update AmuxChan flag */
                `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_index;
                /* Set the Amux channel */
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_amuxChan);
                
            }
            else
            {
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_2048);
                 /* Update AmuxChan flag */
                `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_index + `$INSTANCE_NAME`_CSA_IN_CHAN;
                /* Set the Amux channel */
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
            }
        }
    #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCE > 0) */
    #if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
        
        else if (`$INSTANCE_NAME`_adcConvNow == `$INSTANCE_NAME`_lastAuxVoltChan)
        {
            `$INSTANCE_NAME`_adcConvNow = `$INSTANCE_NAME`_ISR_STATE_0;
            `$INSTANCE_NAME`_adcConvNext = `$INSTANCE_NAME`_ISR_STATE_0 + 1;
            /* Set the proper ADC configuration */
            `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_2048);
            
            /* Update AmuxChan flag */
            `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_adcConvNow;
            /* Select the proper AMux Channel */
            `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
            
            if (`$INSTANCE_NAME`_VoltageType[0] == `$INSTANCE_NAME`_VOLTAGE_TYPE_SINGLE)
            {
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
            }
            else
            {
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_adcConvNow);
            }
            `$INSTANCE_NAME`_index = 0u;
        }
        
        else if ((`$INSTANCE_NAME`_adcConvNow > `$INSTANCE_NAME`_lastCurrentChan) &&
                 (`$INSTANCE_NAME`_adcConvNow < `$INSTANCE_NAME`_lastAuxVoltChan))
        {
            `$INSTANCE_NAME`_index++;
            if ((`$INSTANCE_NAME`_AuxVoltageType[`$INSTANCE_NAME`_index] == `$INSTANCE_NAME`_AUX_VOLTAGE_64MV_DIFF) ||
                (`$INSTANCE_NAME`_AuxVoltageType[`$INSTANCE_NAME`_index] == `$INSTANCE_NAME`_AUX_VOLTAGE_128MV_DIFF))
            {
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_LOW);
            }
            else 
            {
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_NEW(`$INSTANCE_NAME`_RANGE_2048);
            }
            `$INSTANCE_NAME`_auxIndex++;
            /* Update AmuxChan flag */
            `$INSTANCE_NAME`_amuxChan = `$INSTANCE_NAME`_auxIndex;
            /* Select the proper Amux Channel */
            `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_amuxChan);
            
            if (`$INSTANCE_NAME`_AuxVoltageType[`$INSTANCE_NAME`_index] != `$INSTANCE_NAME`_AUX_VOLTAGE_SINGLE)
            {
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_amuxChan);
            }
            else
            {
                if ((`$INSTANCE_NAME`_AuxVoltageType[`$INSTANCE_NAME`_index] != \
                    `$INSTANCE_NAME`_AuxVoltageType[`$INSTANCE_NAME`_index - 1]) || 
                    (convDone == `$INSTANCE_NAME`_STATE_CAL))
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
                }
            }
        }
    #endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */
    else if (`$INSTANCE_NAME`_adcConvNow == `$INSTANCE_NAME`_STATE_CAL)
    {
        /***********************************************************************
        * Going to a TBD Calibration state
        *
        * Skip AMux, routes will be manually set
        *  Next state will be pre-calibration next state
        ***********************************************************************/
        `$INSTANCE_NAME`_adcConvNext = `$INSTANCE_NAME`_adcConvNextPreCal;

        /***********************************************************************
        * Configure for the specific calibration operation.
        * NOTE: at entry DSM regs MUST BE setup as normal AMux sampling
        ***********************************************************************/
        switch (`$INSTANCE_NAME`_adcConvCalType)
        {
            case `$INSTANCE_NAME`_CAL_START:
                /*******************************************************************
                * Startup ADC, only after firmware reset
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_SelectConfiguration(`$INSTANCE_NAME`_RANGE_2048, 0);
                /* Initialize local selection memory    */
                `$INSTANCE_NAME`_adcConfig = `$INSTANCE_NAME`_RANGE_2048; 
                `$INSTANCE_NAME`_adcConvDoneConfig = `$INSTANCE_NAME`_RANGE_2048;
                `$INSTANCE_NAME`_ADC_Start();
                CyIntDisable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
                CyIntSetVector(`$INSTANCE_NAME`_IRQ__INTC_NUMBER, `$INSTANCE_NAME`_ISR );
                CyIntEnable(`$INSTANCE_NAME`_IRQ__INTC_NUMBER);
                `$INSTANCE_NAME`_adcConvNow    = `$INSTANCE_NAME`_ISR_STATE_0;
                `$INSTANCE_NAME`_adcConvNext = `$INSTANCE_NAME`_ISR_STATE_0 + 1;
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_adcConvNow);
                if (`$INSTANCE_NAME`_VoltageType[0] == `$INSTANCE_NAME`_VOLTAGE_TYPE_SINGLE)
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_PGA_OUT_CHAN);
                }
                else
                {
                    `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_adcConvNow);
                }
                /* Don't "restore" DSM regs, not real CAL */
                `$INSTANCE_NAME`_justReset = `$INSTANCE_NAME`_CYTRUE;  
                #if (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL)
                    `$INSTANCE_NAME`_faultDetected = `$INSTANCE_NAME`_CYFALSE;
                #endif /* (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL) */
                break;  

            case `$INSTANCE_NAME`_CAL_CFG1Z:
                /*******************************************************************
                * +/- 2048mV : Calibrate ZERO, VP = VN = Vssa
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_2048);
                `$INSTANCE_NAME`_SaveDsmRegsAndIsolate();
                /* vn_vssa & vp_vssa    */
                `$INSTANCE_NAME`_ADC_DSM_SW3_REG = `$INSTANCE_NAME`_ADC_VN_VSSA_VP_VSSA;  
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcZeroCalRawCfg1;
                break;

            case `$INSTANCE_NAME`_CAL_CFG1G:
                /*******************************************************************
                * +/- 2048mV : Calibrate GAIN, VP = Vssa, VN = Vref
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_2048);
                `$INSTANCE_NAME`_SaveDsmRegsAndIsolate();
                /* vn_vref & vp_vssa  */
                `$INSTANCE_NAME`_ADC_DSM_SW3_REG = `$INSTANCE_NAME`_ADC_VN_VREF_VP_VSSA;
                /*******************************************************************
                * Either use REFBUF1 by enabling it or bypass by using SW12
                *   cf. "Reference Options" in TRM.
                *  SW12 avoids introducing potential REFBUF1 offset error.
                *******************************************************************/
                /* S12 ON so can read Vref */
                `$INSTANCE_NAME`_ADC_DSM_REF3_REG |= `$INSTANCE_NAME`_ADC_S12_ON;
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcGainCalRawCfg1;
                break;

            case `$INSTANCE_NAME`_CAL_PGAZ:
                /*******************************************************************
                * PGAx2 : Calibrate PGA offset, VP = Vssa, VN = ABUSL_1
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_2048);
                `$INSTANCE_NAME`_SaveDsmRegsAndIsolate();
                /* vp_vssa              */
                `$INSTANCE_NAME`_ADC_DSM_SW3_REG = `$INSTANCE_NAME`_ADC_VP_VSSA;
                /* vn_abus1             */
                `$INSTANCE_NAME`_ADC_DSM_SW6_REG = `$INSTANCE_NAME`_ADC_VN_ABUS1;
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcSCCalRawCfg1;
                break;

            case `$INSTANCE_NAME`_CAL_CFG2Z:
                /***************************************************************
                * +/- LOW mV : Calibrate ZERO, VP = Vssa, VN = Vssa
                ***************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_LOW);
                `$INSTANCE_NAME`_SaveDsmRegsAndIsolate();
                /* vn_vssa + vp_vssa    */
                `$INSTANCE_NAME`_ADC_DSM_SW3_REG = `$INSTANCE_NAME`_ADC_VN_VSSA_VP_VSSA; 
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcZeroCalRawCfg2;
                break;
                
            #if (`$INSTANCE_NAME`_CAL_PIN_EXPOSED)
            case `$INSTANCE_NAME`_CAL_CFG2Ga:
                /*******************************************************************
                * Measure cal input using DelSig +/- 1024 mV range
                *   Route: cal input to +ve of ADC and connect GND to -ve input
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_1024);
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_CAL_IN_CHAN);
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_CAL_IN_CHAN);
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcGainCalRawCfg2a;
                break;

            case `$INSTANCE_NAME`_CAL_CFG2Gb:
                /*******************************************************************
                * Measure cal input using DelSig +/- LOW mV range
                *   Route: cal input to +ve of ADC, vssa to -ve of ADC
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_LOW);
                `$INSTANCE_NAME`_PM_AMux_Voltage_FastSelect(`$INSTANCE_NAME`_CAL_IN_CHAN);
                `$INSTANCE_NAME`_PM_AMux_Current_FastSelect(`$INSTANCE_NAME`_CAL_IN_CHAN);
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcGainCalRawCfg2b;
                break;
            #endif /* `$INSTANCE_NAME`_CAL_PIN_EXPOSED */

            case `$INSTANCE_NAME`_CAL_CFG3Z:
                /*******************************************************************
                * +/- 1024mV : Calibrate ZERO, VP = Vssa, VN = Vssa
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_1024);
                `$INSTANCE_NAME`_SaveDsmRegsAndIsolate();
                /* vn_vssa + vp_vssa    */
                `$INSTANCE_NAME`_ADC_DSM_SW3_REG = `$INSTANCE_NAME`_ADC_VN_VSSA_VP_VSSA;
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcZeroCalRawCfg3;
                break;

            case `$INSTANCE_NAME`_CAL_CFG3G:
                /*******************************************************************
                * +/- 1024mV : Calibrate GAIN, VP = Vssa, VN = Vref
                *******************************************************************/
                `$INSTANCE_NAME`_ADC_CONFIG_PUSH_OLD(`$INSTANCE_NAME`_RANGE_1024);
                `$INSTANCE_NAME`_SaveDsmRegsAndIsolate();
                /* vn_vref + vp_vssa  */
                `$INSTANCE_NAME`_ADC_DSM_SW3_REG = `$INSTANCE_NAME`_ADC_VN_VREF_VP_VSSA;
                /* S12 ON so can read Vref */
                `$INSTANCE_NAME`_ADC_DSM_REF3_REG |= `$INSTANCE_NAME`_ADC_S12_ON; 
                `$INSTANCE_NAME`_pCalStore = &`$INSTANCE_NAME`_adcGainCalRawCfg3;
                break;
        }
    }
    
    /* Start the ADC conversion */
    `$INSTANCE_NAME`_ADC_StartConvert();
    
    /* Save the raw ADC readings in its proper destination */
    if (convDone != `$INSTANCE_NAME`_STATE_CAL)
    {
        /* ADC reading is a normal voltage or current sample */
        uint8 CYDATA Idx = 0u;
        volatile int16 CYXDATA * CYDATA pAdcFilt;
        `$INSTANCE_NAME`_ADC_CTL_STRUCT CYXDATA * CYDATA psCtl;
        
        /* initialize the pointers */
        pAdcFilt = &`$INSTANCE_NAME`_voltFilt[0].rawVoltage[Idx];
        psCtl = &`$INSTANCE_NAME`_voltCtl[0];
        
        /***********************************************************************
        * "Normal measurements" are adjusted for "zero" here.
        * Config 2 measurements correct for ROUTE INVERSION here.
        *       These adjustments are applied here because these values are
        *       used in this ISR for trimming and OV/UV/OC/UC testing.
        ***********************************************************************/
        if (`$INSTANCE_NAME`_adcConvDoneConfig == `$INSTANCE_NAME`_RANGE_LOW)
        {
            if (convDone < `$INSTANCE_NAME`_lastCurrentChan)
            {
                rawVal  = -rawVal;                  /* Fix route inversion  */
            }
            rawVal -= `$INSTANCE_NAME`_adcZeroCfg2;   /* Fix zero offset      */
        }
        else
        {
            rawVal -= `$INSTANCE_NAME`_adcZeroCfg1;    /* Fix any zero offset */
        }
      
        if (convDone < `$INSTANCE_NAME`_lastVoltageChan)
        {
            psCtl = &`$INSTANCE_NAME`_voltCtl[convDone];
            Idx = psCtl->idx;
            pAdcFilt = &`$INSTANCE_NAME`_voltFilt[convDone].rawVoltage[Idx];
            #if (`$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE)
                psCtl->idx = 0u;
            #else
                psCtl->idx = (Idx < `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE-1) ? (Idx+1) : 0u;
            #endif /* (`$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE) */
        }    
        #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
            else if ((convDone >= `$INSTANCE_NAME`_lastVoltageChan) && 
                     (convDone < `$INSTANCE_NAME`_lastCurrentChan))
            {
                psCtl = &`$INSTANCE_NAME`_ampCtl[convDone - `$INSTANCE_NAME`_lastVoltageChan];
                Idx = psCtl->idx;
                pAdcFilt = &`$INSTANCE_NAME`_ampFilt[convDone - `$INSTANCE_NAME`_lastVoltageChan].rawVoltage[Idx];
                #if (`$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE)
                    psCtl->idx = 0u;
                #else
                    psCtl->idx = (Idx < `$INSTANCE_NAME`_CURRENT_FILTER_SIZE-1) ? (Idx+1) : 0u;
                #endif /* (`$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE) */
            }
        #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */
        #if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
            else
            {
                psCtl = &`$INSTANCE_NAME`_auxVoltCtl[convDone - `$INSTANCE_NAME`_lastCurrentChan];
                Idx = psCtl->idx;
                pAdcFilt = &`$INSTANCE_NAME`_auxVoltFilt[convDone - `$INSTANCE_NAME`_lastCurrentChan].rawVoltage[Idx];
                #if (`$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE)
                    psCtl->idx = 0u;
                #else
                    psCtl->idx = (Idx < `$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE-1) ? (Idx+1) : 0u;
                #endif /* (`$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE) */
            }
        #endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */
        
        /***********************************************************************
        * Update filter summation (subtract old value and add new value).
        ***********************************************************************/
        rawFiltVal  = psCtl->filtVal;
        rawFiltVal -= *pAdcFilt;
        rawFiltVal += rawVal;
        psCtl->filtVal = rawFiltVal;
        
        /***********************************************************************
        * Finally, store new value in history buffer
        ***********************************************************************/
        *pAdcFilt = rawVal;
    }
    else if (convDone == `$INSTANCE_NAME`_STATE_CAL)
    {
        /*
        * Calibration measurement (these DO NOT have any corrections applied)
        *  so the non-ISR calibration process sees pure raw readings.
        */
        if (!`$INSTANCE_NAME`_justReset)
        {
            /* Store raw DelSig reading in appropriate Calibration location */
            *`$INSTANCE_NAME`_pCalStore = rawVal;
        }
        else
        {
            `$INSTANCE_NAME`_justReset = `$INSTANCE_NAME`_CYFALSE;
        }
    }
    else
    {
        /* No action */
    }
    
    /***********************************************************************
        * Generate the Fault, Warn and Pgood and EOC output signals
        * Also, update the OV, OC and UV status bits and OV, UV and OC source
        * status bits
    ************************************************************************/
    
        /* Generate EOC output */
    #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
        if (convDone == (`$INSTANCE_NAME`_MAX_CHANNELS - 1))
        {
            `$INSTANCE_NAME`_CONTROL1_REG = (`$INSTANCE_NAME`_CONTROL1_REG ^ `$INSTANCE_NAME`_EOC_MASK);
        }
    #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */

    /* Generate pgood, warn and fault signals */
    /* Conv done is for current and voltage channels */
    if (convDone < `$INSTANCE_NAME`_lastCurrentChan) 
    {
        if (`$INSTANCE_NAME`_warnEnable == `$INSTANCE_NAME`_CYTRUE)
        {
            if (convDone < `$INSTANCE_NAME`_lastVoltageChan) /* for voltage channel */
            {
                if (`$INSTANCE_NAME`_warnMask & (1u << convDone))
                {
                    if (`$INSTANCE_NAME`_warnWin[convDone].enable < `$INSTANCE_NAME`_WARN_FAULT_ENABLED)
                    {
                        `$INSTANCE_NAME`_warnWin[convDone].enable++;
                    }
                    else
                    {
                        if (`$INSTANCE_NAME`_warnSources & `$INSTANCE_NAME`_OV_WARN_SOURCE_MASK)
                        {
                              if (rawFiltVal > `$INSTANCE_NAME`_warnWin[convDone].OVWarnThrshldCounts )
                            {
                                /* Write the status to status bits */
                                `$INSTANCE_NAME`_warnSourceStatus |= `$INSTANCE_NAME`_ENABLE_OV_WARN_SOURCE;
                                `$INSTANCE_NAME`_OVWarnStatus |= (1u << convDone);
                                #if !defined(\
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
                                    /* Write the hardware signal */
                                    `$INSTANCE_NAME`_CONTROL1_REG |= `$INSTANCE_NAME`_WARN_MASK;
                                #endif /* (
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
                            }
                        }
                    
                        if (`$INSTANCE_NAME`_warnSources & `$INSTANCE_NAME`_UV_WARN_SOURCE_MASK)
                        {
                            if (rawFiltVal < `$INSTANCE_NAME`_warnWin[convDone].UVWarnThrshldCounts )
                            {
                                /* Write the status to status bits */
                                `$INSTANCE_NAME`_warnSourceStatus |= `$INSTANCE_NAME`_ENABLE_UV_WARN_SOURCE;
                                `$INSTANCE_NAME`_UVWarnStatus |= (1u << convDone);
                                #if !defined(\
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
                                    /* Write the hardware signal */
                                    `$INSTANCE_NAME`_CONTROL1_REG |= `$INSTANCE_NAME`_WARN_MASK;
                                #endif /* (
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
                            }
                        }
                    }
                }
            }
            #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
                else
                {
                    /* Get the index */
                    iChanNum = convDone - `$INSTANCE_NAME`_lastVoltageChan;
                    /* Get the actual current channel number */
                    iChanNum = `$INSTANCE_NAME`_ActiveCurrentChan[iChanNum];
                    /* Converter number is not zero based. So left shift count should
                       be converter number - 1 */
                    if (`$INSTANCE_NAME`_warnMask & (1u << (iChanNum - 1)))
                    {
                        if (`$INSTANCE_NAME`_warnWin[iChanNum - 1].enable < `$INSTANCE_NAME`_WARN_FAULT_ENABLED)
                        {
                            /* Do nothing till enable count reaches the required value */
                        }
                        else
                        {
                            if (`$INSTANCE_NAME`_warnSources & `$INSTANCE_NAME`_OC_WARN_SOURCE_MASK)
                            {
                                if (rawFiltVal > `$INSTANCE_NAME`_warnWin[iChanNum - 1].OCWarnThrshldCounts )
                                {
                                    /* Write the status to status bits */
                                    `$INSTANCE_NAME`_warnSourceStatus |= `$INSTANCE_NAME`_ENABLE_OC_WARN_SOURCE;
                                    `$INSTANCE_NAME`_OCWarnStatus |= (1u << (iChanNum - 1));
                                    #if !defined(\
                                    `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
                                        /* Write the hardware signal */
                                        `$INSTANCE_NAME`_CONTROL1_REG |= `$INSTANCE_NAME`_WARN_MASK;
                                    #endif /* (
                                    `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
                                }
                            }
                        }
                    }
                }
            #endif /* (`$INSTANCE_NAME`_CURRENT_SOURCES > 0) */
        }
            
        if (`$INSTANCE_NAME`_faultEnable == `$INSTANCE_NAME`_CYTRUE)
        {
            if (convDone < `$INSTANCE_NAME`_lastVoltageChan)
            {
                if (`$INSTANCE_NAME`_faultMask & (1u << convDone))
                {
                    if (`$INSTANCE_NAME`_faultWin[convDone].enable < `$INSTANCE_NAME`_WARN_FAULT_ENABLED)
                    {
                        `$INSTANCE_NAME`_faultWin[convDone].enable++;
                    }
                    else
                    {
                        if (`$INSTANCE_NAME`_faultSources & `$INSTANCE_NAME`_OV_FAULT_SOURCE_MASK)
                        {
                            if (rawFiltVal > `$INSTANCE_NAME`_faultWin[convDone].OVFaultThrshldCounts )
                            {
                                /* Write the status to status bits */
                                `$INSTANCE_NAME`_faultSourceStatus |= `$INSTANCE_NAME`_ENABLE_OV_FAULT_SOURCE;
                                `$INSTANCE_NAME`_OVFaultStatus |= (1u << convDone);
                                #if !defined(\
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
                                    /* Write the hardware signal */
                                    `$INSTANCE_NAME`_CONTROL1_REG |= `$INSTANCE_NAME`_FAULT_MASK;
                                #endif /* (
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
                                /* Set the fault detected flag */
                                #if (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL)
                                    `$INSTANCE_NAME`_faultDetected = `$INSTANCE_NAME`_CYTRUE;
                                #else
                                    `$INSTANCE_NAME`_faultStatus[convDone] = `$INSTANCE_NAME`_CYTRUE;
                                #endif /* (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL) */
                            }
                        }
                        
                        if (`$INSTANCE_NAME`_faultSources & `$INSTANCE_NAME`_UV_FAULT_SOURCE_MASK)
                        {
                            if (rawFiltVal < `$INSTANCE_NAME`_faultWin[convDone].UVFaultThrshldCounts )
                            {
                                /* Write the status to status bits */
                                `$INSTANCE_NAME`_faultSourceStatus |= `$INSTANCE_NAME`_ENABLE_UV_FAULT_SOURCE;
                                `$INSTANCE_NAME`_UVFaultStatus |= (1u << convDone);
                                #if !defined(\
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
                                    /* Write the hardware signal */
                                    `$INSTANCE_NAME`_CONTROL1_REG |= `$INSTANCE_NAME`_FAULT_MASK;
                                #endif /* (
                                `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
                                /* Set the fault detected flag */
                                #if (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL)
                                    `$INSTANCE_NAME`_faultDetected = `$INSTANCE_NAME`_CYTRUE;
                                #else
                                    `$INSTANCE_NAME`_faultStatus[convDone] = `$INSTANCE_NAME`_CYTRUE;
                                #endif /* (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL) */
                            }
                        }
                    }
                }
            }
            #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
                else
                {
                    /* Get the index */
                    iChanNum = convDone - `$INSTANCE_NAME`_lastVoltageChan;
                    /* Get the actual current channel number */
                    iChanNum = `$INSTANCE_NAME`_ActiveCurrentChan[iChanNum];
                    /* Converter number is not zero based. So left shitcount should be 
                       converter number - 1 */
                    if (`$INSTANCE_NAME`_faultMask & (1u << (iChanNum - 1)))
                    {
                        if (`$INSTANCE_NAME`_faultWin[iChanNum - 1].enable < `$INSTANCE_NAME`_WARN_FAULT_ENABLED)
                        {
                            /* Do nothing till enable count reaches the required value */
                        }
                        else
                        {
                            if (`$INSTANCE_NAME`_faultSources & `$INSTANCE_NAME`_OC_FAULT_SOURCE_MASK)
                            {
                                if (rawFiltVal > `$INSTANCE_NAME`_faultWin[iChanNum - 1].OCFaultThrshldCounts )
                                {
                                    /* Write the status to status bits */
                                    `$INSTANCE_NAME`_faultSourceStatus |= `$INSTANCE_NAME`_ENABLE_OC_FAULT_SOURCE;
                                    `$INSTANCE_NAME`_OCFaultStatus |= (1u << (iChanNum - 1));
                                    #if !defined(\
                                    `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED)
                                        /* Write the hardware signal */
                                        `$INSTANCE_NAME`_CONTROL1_REG |= `$INSTANCE_NAME`_FAULT_MASK;
                                    #endif /* (
                                    `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__REMOVED) */
                                    /* Set the fault detected flag */
                                    #if (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL)
                                        `$INSTANCE_NAME`_faultDetected = `$INSTANCE_NAME`_CYTRUE;
                                    #else
                                        `$INSTANCE_NAME`_faultStatus[iChanNum - 1] = `$INSTANCE_NAME`_CYTRUE;
                                    #endif /* (
                                    `$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_GLOBAL) */
                                }
                            }
                        }
                    }
                }
            #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */
        }
    }
    
    /* Generate the PGOOD signal based on the pgood terminal configuration */
    #if (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_INDIVIDUAL)
        if (convDone != `$INSTANCE_NAME`_STATE_CAL)
        {
            if (convDone < `$INSTANCE_NAME`_lastVoltageChan)
            {
                #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
                if (!((`$INSTANCE_NAME`_CurrentType[convDone] != `$INSTANCE_NAME`_CURRENT_TYPE_NA) &&
                    (`$INSTANCE_NAME`_faultSources & `$INSTANCE_NAME`_OC_FAULT_SOURCE_MASK)))
                {
                #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */
                    if (!(`$INSTANCE_NAME`_faultStatus[convDone]))
                    {
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED)
                            /* publish pgood */
                            if (convDone < `$INSTANCE_NAME`_CONVERTERS_8)
                            {
                                `$INSTANCE_NAME`_PGOOD_CONTROL1_REG |= (1u << convDone);
                            }
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED) */
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED)
                            #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
                                else if ((convDone >= `$INSTANCE_NAME`_CONVERTERS_8) && 
                                         (convDone < `$INSTANCE_NAME`_CONVERTERS_16)) 
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL2_REG |= 
                                    (1u << (convDone - `$INSTANCE_NAME`_CONVERTERS_8));
                                }
                            #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl1__REMOVED)
                            #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                else if ((convDone >= `$INSTANCE_NAME`_CONVERTERS_16) && 
                                         (convDone < `$INSTANCE_NAME`_CONVERTERS_24)) 
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL3_REG |= 
                                    (1u << (convDone - `$INSTANCE_NAME`_CONVERTERS_16));
                                }
                            #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl1__REMOVED)
                            #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                else  
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL4_REG |= 
                                    (1u << (convDone - `$INSTANCE_NAME`_CONVERTERS_24));
                                }
                            #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                    }
                    else
                    {
                        /* Clear the status */
                        `$INSTANCE_NAME`_faultStatus[convDone] = `$INSTANCE_NAME`_CYFALSE;
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED)
                            /* Clear pgood */
                            if (convDone < `$INSTANCE_NAME`_CONVERTERS_8)
                            {
                                `$INSTANCE_NAME`_PGOOD_CONTROL1_REG &= ~(1u << convDone);
                            }
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED) */
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED)
                            #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
                                else if ((convDone >= `$INSTANCE_NAME`_CONVERTERS_8) && 
                                         (convDone < `$INSTANCE_NAME`_CONVERTERS_16)) 
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL2_REG &= 
                                    ~(1u << (convDone - `$INSTANCE_NAME`_CONVERTERS_8));
                                }
                            #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl1__REMOVED)
                            #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                else if ((convDone >= `$INSTANCE_NAME`_CONVERTERS_16) && 
                                         (convDone < `$INSTANCE_NAME`_CONVERTERS_24)) 
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL3_REG &= 
                                    ~(1u << (convDone - `$INSTANCE_NAME`_CONVERTERS_16));
                                }
                            #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                        #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl1__REMOVED)
                            #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                else  
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL4_REG &= 
                                    ~(1u << (convDone - `$INSTANCE_NAME`_CONVERTERS_24));
                                }
                            #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                        #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                    }
                #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
                }
                #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */
            }
            #if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
                else
                {
                    if ((`$INSTANCE_NAME`_CurrentType[convDone] != `$INSTANCE_NAME`_CURRENT_TYPE_NA) &&
                        (`$INSTANCE_NAME`_faultSources & `$INSTANCE_NAME`_OC_FAULT_SOURCE_MASK))
                    {
                        /* Get the index */
                        iChanNum = convDone - `$INSTANCE_NAME`_lastVoltageChan;
                        /* Get the actual current channel number */
                        iChanNum = (`$INSTANCE_NAME`_ActiveCurrentChan[iChanNum] - 1);
                        if (!(`$INSTANCE_NAME`_faultStatus[iChanNum]))
                        {
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED)
                                /* publish pgood */
                                if (iChanNum < `$INSTANCE_NAME`_CONVERTERS_8)
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL1_REG |= (1u << iChanNum);
                                }
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED) */
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED)
                                #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
                                    else if ((iChanNum >= `$INSTANCE_NAME`_CONVERTERS_8) && 
                                             (iChanNum < `$INSTANCE_NAME`_CONVERTERS_16)) 
                                    {
                                        `$INSTANCE_NAME`_PGOOD_CONTROL2_REG |= 
                                        (1u << (iChanNum - `$INSTANCE_NAME`_CONVERTERS_8));
                                    }
                                #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl1__REMOVED)
                                #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                    else if ((iChanNum >= `$INSTANCE_NAME`_CONVERTERS_16) && 
                                             (iChanNum < `$INSTANCE_NAME`_CONVERTERS_24)) 
                                    {
                                        `$INSTANCE_NAME`_PGOOD_CONTROL3_REG |= 
                                        (1u << (iChanNum - `$INSTANCE_NAME`_CONVERTERS_16));
                                    }
                                #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl1__REMOVED)
                                #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                    else  
                                    {
                                        `$INSTANCE_NAME`_PGOOD_CONTROL4_REG |= 
                                        (1u << (iChanNum - `$INSTANCE_NAME`_CONVERTERS_24));
                                    }
                                #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                        }
                        else
                        {
                            /* Clear the status */
                            `$INSTANCE_NAME`_faultStatus[iChanNum] = `$INSTANCE_NAME`_CYFALSE;
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED)
                                /* Clear pgood */
                                if (iChanNum < `$INSTANCE_NAME`_CONVERTERS_8)
                                {
                                    `$INSTANCE_NAME`_PGOOD_CONTROL1_REG &= ~(1u << iChanNum);
                                }
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED) */
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED)
                                #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
                                    else if ((iChanNum >= `$INSTANCE_NAME`_CONVERTERS_8) && 
                                             (iChanNum < `$INSTANCE_NAME`_CONVERTERS_16)) 
                                    {
                                        `$INSTANCE_NAME`_PGOOD_CONTROL2_REG &= 
                                        ~(1u << (iChanNum - `$INSTANCE_NAME`_CONVERTERS_8));
                                    }
                                #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl1__REMOVED)
                                #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                    else if ((iChanNum >= `$INSTANCE_NAME`_CONVERTERS_16) && 
                                             (iChanNum < `$INSTANCE_NAME`_CONVERTERS_24)) 
                                    {
                                        `$INSTANCE_NAME`_PGOOD_CONTROL3_REG &= 
                                        ~(1u << (iChanNum - `$INSTANCE_NAME`_CONVERTERS_16));
                                    }
                                #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                            #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl1__REMOVED)
                                #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                                    else  
                                    {
                                        `$INSTANCE_NAME`_PGOOD_CONTROL4_REG &= \
                                        ~(1u << (iChanNum - `$INSTANCE_NAME`_CONVERTERS_24));
                                    }
                                #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                            #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl1__REMOVED) */
                        }
                    }
                }
            #endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */
        }
    #else
        /* Generate the PGOOD signal at the end of voltage and current measurements */
        if (convDone == (`$INSTANCE_NAME`_TOTAL_V_I_MEASUREMENTS - 1))
        {
            if (!`$INSTANCE_NAME`_faultDetected)
            {
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__REMOVED)
                    #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24)
                        `$INSTANCE_NAME`_PGOOD_CONTROL4_REG |= `$INSTANCE_NAME`_PGOOD_CTRL_25_32_MASK;
                    #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24) */
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__REMOVED) */
                
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__REMOVED)
                    #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                        `$INSTANCE_NAME`_PGOOD_CONTROL3_REG |= `$INSTANCE_NAME`_PGOOD_CTRL_17_24_MASK; 
                    #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16) */
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__REMOVED) */
                
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__REMOVED)
                    #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
                        `$INSTANCE_NAME`_PGOOD_CONTROL2_REG |= `$INSTANCE_NAME`_PGOOD_CTRL_9_16_MASK;
                    #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__REMOVED) */
                
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED)
                    `$INSTANCE_NAME`_PGOOD_CONTROL1_REG |= `$INSTANCE_NAME`_PGOOD_CTRL_1_8_MASK;
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED) */
            }
            else
            {
                `$INSTANCE_NAME`_faultDetected = `$INSTANCE_NAME`_CYFALSE;
                /* Clear Pgood Signal */
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__REMOVED)
                    #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24)
                        `$INSTANCE_NAME`_PGOOD_CONTROL4_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_25_32_MASK;
                    #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24) */
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__REMOVED) */
                
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__REMOVED)
                    #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
                        `$INSTANCE_NAME`_PGOOD_CONTROL3_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_17_24_MASK; 
                    #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16) */
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__REMOVED) */
                
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__REMOVED)
                    #if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
                        `$INSTANCE_NAME`_PGOOD_CONTROL2_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_9_16_MASK;
                    #endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__REMOVED) */
                
                #if !defined(`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED)
                    `$INSTANCE_NAME`_PGOOD_CONTROL1_REG &= ~`$INSTANCE_NAME`_PGOOD_CTRL_1_8_MASK;
                #endif /* (`$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__REMOVED) */
            }
        }
    #endif /* (`$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG == `$INSTANCE_NAME`_PGOOD_INDIVIDUAL) */
}


/******************************************************************************
* Function Name: `$INSTANCE_NAME`_SaveDsmRegsAndIsolate()
********************************************************************************
*
* Summary:
*  Save DeltaSigma DSM ADC connections to fully isolate before manual routing
*  (bypassing the AMuxSeq Component) for calibration.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Theory:
*  Note the AMuxSeq routing is logical and predictable, EXCEPT that the fitter
*   currently disconnects the 2xVref reference at the PGA, not at the DSM(-)
*   as the schematic leads you to conclude.  This is handled here
*
* Calibration originally used the AMuxSeq component, but uses 3 AMuxSeq
*  input channels.  The AMuxSeq has a max of 32 channels and we want up to
*  16 rails, so do calibration routing outside the AMuxSeq.
*
*  This section MIGHT need occasional tweaking because it relies upon
*  knowledge of how CY Fitter works.  Although CY Fitter should be
*  constrained to a good rendering, it may still do unexpected things.
*  For example, it attaches 2048mV to DelSig(-) by leaving ABUSL1 on the
*  DelSig and controlling the PGA output switch.
*
*******************************************************************************/
static void `$INSTANCE_NAME`_SaveDsmRegsAndIsolate(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveDsmRegsAndIsolate")`
{
    `$INSTANCE_NAME`_keep_CYREG_DSM0_SW0 = `$INSTANCE_NAME`_ADC_DSM_SW0_REG;
    `$INSTANCE_NAME`_keep_CYREG_DSM0_SW2 = `$INSTANCE_NAME`_ADC_DSM_SW2_REG;
    `$INSTANCE_NAME`_keep_CYREG_DSM0_SW3 = `$INSTANCE_NAME`_ADC_DSM_SW3_REG;
    `$INSTANCE_NAME`_keep_CYREG_DSM0_SW4 = `$INSTANCE_NAME`_ADC_DSM_SW4_REG;
    `$INSTANCE_NAME`_keep_CYREG_DSM0_SW6 = `$INSTANCE_NAME`_ADC_DSM_SW6_REG;
    /***************************************************************************
    * Isolate/disconnect DSM inputs
    ***************************************************************************/
    `$INSTANCE_NAME`_ADC_DSM_SW0_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW2_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW3_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW4_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW6_REG = 0x00u;
    
    /* Set the flag after writing ADC switching registers with new values for 
       calibration. This will be used to restore the DSM swithcing registers 
       soon after calibration */
    `$INSTANCE_NAME`_dsmRegRestorePend = `$INSTANCE_NAME`_CYTRUE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreDsmRegs()
********************************************************************************
*
* Summary:
*  Replace DSM default switches as expected by AMuxSeq.  Does the inverse
*  of sameDsmRegsAndIsolate() when leaving calibration and resuming sampling.
*
* Parameters:
*  void
*
* Return:
*  void
*
* Theory:
*
* Side Effects:
*  Note the AMuxSeq routing is logical and predictable, EXCEPT that the fitter
*   currently disconnects the 2xVref reference at the PGA, not at the DSM(-)
*   as the schematic leads you to conclude.  This is handled here
*
*******************************************************************************/
static void `$INSTANCE_NAME`_RestoreDsmRegs(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreDsmRegs")`
{
    /* Isolate/disconnect DSM inputs BEFORE restoring connections */
    `$INSTANCE_NAME`_ADC_DSM_SW0_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW2_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW3_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW4_REG = 0x00u;
    `$INSTANCE_NAME`_ADC_DSM_SW6_REG = 0x00u;
    
    /* Restore original DSM connnections */
    `$INSTANCE_NAME`_ADC_DSM_SW0_REG = `$INSTANCE_NAME`_keep_CYREG_DSM0_SW0;
    `$INSTANCE_NAME`_ADC_DSM_SW2_REG = `$INSTANCE_NAME`_keep_CYREG_DSM0_SW2;
    `$INSTANCE_NAME`_ADC_DSM_SW3_REG = `$INSTANCE_NAME`_keep_CYREG_DSM0_SW3;
    `$INSTANCE_NAME`_ADC_DSM_SW4_REG = `$INSTANCE_NAME`_keep_CYREG_DSM0_SW4;
    `$INSTANCE_NAME`_ADC_DSM_SW6_REG = `$INSTANCE_NAME`_keep_CYREG_DSM0_SW6;
    
    /* Clear the flag after restoring ADC switching registers */
    `$INSTANCE_NAME`_dsmRegRestorePend = `$INSTANCE_NAME`_CYFALSE;
}


/* [] END OF FILE */
