/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file contains the function prototypes and constants used in
*  the Power Monitor component.
*
* Note:
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_POWERMONITOR_`$INSTANCE_NAME`_H) 
#define CY_POWERMONITOR_`$INSTANCE_NAME`_H 

#include "cytypes.h"
#include "cylib.h"


/**************************************
* Type defines *
***************************************/
/*
* `$INSTANCE_NAME`_ADC_CTL_STRUCT - ADC filter reading structure (one struct 
*                                   per channel)
*          idx = array index of oldest reading (next to be replaced)
*      filtval = Sum of last _ADC_FILT_SZ samples, divide to get Ave
*/
typedef struct
{
    volatile uint8 idx;                 /* Next sample to be replaced       */
    volatile int32 filtVal;             /* sample totalizer                 */
} `$INSTANCE_NAME`_ADC_CTL_STRUCT;


/*******************************************************************************
* `$INSTANCE_NAME`_WarnWin[] -> Structure to hold the warning thresholds
* `$INSTANCE_NAME`_FaultWin[] -> Structure to hold the fault thresholds
*******************************************************************************/
typedef struct
{
    uint8 enable;   /* Z suppresses warnings (e.g. when rail is OFF) */
    int32 OVWarnThrshldCounts;
    int32 UVWarnThrshldCounts;
    int32 OCWarnThrshldCounts;
    uint16 OVWarnThrshldVolts;
    uint16 UVWarnThrshldVolts;
    float OCWarnThrshldAmps;
} `$INSTANCE_NAME`_WARNWIN_STRUCT;

extern `$INSTANCE_NAME`_WARNWIN_STRUCT CYXDATA `$INSTANCE_NAME`_warnWin[];

typedef struct
{
    uint8 enable;   /* Z suppresses warnings (e.g. when rail is OFF) */
    int32 OVFaultThrshldCounts;
    int32 UVFaultThrshldCounts;
    int32 OCFaultThrshldCounts;
    uint16 OVFaultThrshldVolts;
    uint16 UVFaultThrshldVolts;
    float OCFaultThrshldAmps;
} `$INSTANCE_NAME`_FAULTWIN_STRUCT;

extern `$INSTANCE_NAME`_FAULTWIN_STRUCT CYXDATA `$INSTANCE_NAME`_faultWin[];


/* Variables to access at different source files */
extern volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvNow;
extern volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvNext;
extern volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvNextPreCal;
extern volatile uint8 CYDATA `$INSTANCE_NAME`_adcConvCalType;

/* Calibratin Variables */
/*                                                 ~ VALUE, RANGE Description */
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcZeroCalRawCfg1; 
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg1; 
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcSCCalRawCfg1; 
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcZeroCalRawCfg2; 
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg2a;
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg2b;
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcZeroCalRawCfg3;  
extern volatile int16 CYXDATA `$INSTANCE_NAME`_adcGainCalRawCfg3;  

/* Fault and Warn Mask variables */
extern volatile uint32 CYDATA `$INSTANCE_NAME`_faultMask;
extern volatile uint32 CYDATA `$INSTANCE_NAME`_warnMask;

/* Enable fault and Enable warn variables */
extern CYBIT `$INSTANCE_NAME`_faultEnable;
extern CYBIT `$INSTANCE_NAME`_warnEnable;

/* Fault and Warn source variables */
extern volatile uint8 CYDATA `$INSTANCE_NAME`_warnSources;
extern volatile uint8 CYDATA `$INSTANCE_NAME`_faultSources;

/* Warn and Fault source status */
extern volatile uint8 CYDATA `$INSTANCE_NAME`_warnSourceStatus;
extern volatile uint8 CYDATA `$INSTANCE_NAME`_faultSourceStatus;

/* Warn and Fault status for power monitors */
extern volatile uint32 CYDATA `$INSTANCE_NAME`_OCWarnStatus;
extern volatile uint32 CYDATA `$INSTANCE_NAME`_UVWarnStatus;
extern volatile uint32 CYDATA `$INSTANCE_NAME`_OVWarnStatus;
extern volatile uint32 CYDATA `$INSTANCE_NAME`_OCFaultStatus;
extern volatile uint32 CYDATA `$INSTANCE_NAME`_UVFaultStatus;
extern volatile uint32 CYDATA `$INSTANCE_NAME`_OVFaultStatus;


/*******************************************************************************
* Configuration #1 : 2048 mV differential range
*  adcZeroCfg1  = units are ADC counts when both inputs are at Vssa
*  adcGainCfg1  = units are (1 mV) / (ADC count)
*  adcSeAdjCfg1 = units are mV from 2xVref PGA (added to all voltage rails)
*******************************************************************************/
extern  int16 `$INSTANCE_NAME`_adcZeroCfg1;
extern  float `$INSTANCE_NAME`_adcGainCfg1;
extern  int16 `$INSTANCE_NAME`_adcSeAdjCfg1;


/*******************************************************************************
* Configuration #2 : low-voltage differential range:
*   adcZeroCfg2 = units are ADC counts when both inputs are at Vssa
*   adcGainCfg2 = units are (100 uV) / (ADC count)
*******************************************************************************/
extern int16 `$INSTANCE_NAME`_adcZeroCfg2;
extern float `$INSTANCE_NAME`_adcGainCfg2;


/*******************************************************************************
* Configuration #3 : 1024 mV differential range
*   adcZeroCfg3 = units are ADC counts when both inputs are at Vssa
*   adcGainCfg3 = units are (1 mV) / (ADC count)
*******************************************************************************/
extern int16 `$INSTANCE_NAME`_adcZeroCfg3;
extern float `$INSTANCE_NAME`_adcGainCfg3;


/***************************************
*        Function Prototypes 
***************************************/

void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop (void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_EnableFault(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableFault")`;
void `$INSTANCE_NAME`_DisableFault(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableFault")`;
void `$INSTANCE_NAME`_SetFaultMode(uint8 faultMode) `=ReentrantKeil($INSTANCE_NAME . "_SetFaultMode")`;
uint8 `$INSTANCE_NAME`_GetFaultMode(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultMode")`;
void `$INSTANCE_NAME`_SetFaultMask(uint32 faultMask) `=ReentrantKeil($INSTANCE_NAME . "_SetFaultMask")`;
uint32 `$INSTANCE_NAME`_GetFaultMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultMask")`;
uint8 `$INSTANCE_NAME`_GetFaultSource(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultSource")`;
uint32 `$INSTANCE_NAME`_GetOVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVFaultStatus")`;
uint32 `$INSTANCE_NAME`_GetUVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultStatus")`;
uint32 `$INSTANCE_NAME`_GetOCFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOCFaultStatus")`;
void `$INSTANCE_NAME`_EnableWarn(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableWarn")`;
void `$INSTANCE_NAME`_DisableWarn(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableWarn")`;
void `$INSTANCE_NAME`_SetWarnMode(uint8 warnMode) `=ReentrantKeil($INSTANCE_NAME . "_SetWarnMode")`;
uint8 `$INSTANCE_NAME`_GetWarnMode(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnMode")`;
void `$INSTANCE_NAME`_SetWarnMask(uint32 warnMask) `=ReentrantKeil($INSTANCE_NAME . "_SetWarnMask")`;
uint32 `$INSTANCE_NAME`_GetWarnMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnMask")`;
uint8 `$INSTANCE_NAME`_GetWarnSource(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnSource")`;
uint32 `$INSTANCE_NAME`_GetOVWarnStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVWarnStatus")`;
uint32 `$INSTANCE_NAME`_GetUVWarnStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVWarnStatus")`;
uint32 `$INSTANCE_NAME`_GetOCWarnStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOCWarnStatus")`;
void `$INSTANCE_NAME`_SetUVWarnThreshold(uint8 converterNum, uint16 uvWarnThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetUVWarnThreshold")`;
uint16 `$INSTANCE_NAME`_GetUVWarnThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetUVWarnThreshold")`;
void `$INSTANCE_NAME`_SetOVWarnThreshold(uint8 converterNum, uint16 ovWarnThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOVWarnThreshold")`;
uint16 `$INSTANCE_NAME`_GetOVWarnThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOVWarnThreshold")`;
void `$INSTANCE_NAME`_SetUVFaultThreshold(uint8 converterNum, uint16 uvFaultThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetUVFaultThreshold")`;
uint16 `$INSTANCE_NAME`_GetUVFaultThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultThreshold")`;
void `$INSTANCE_NAME`_SetOVFaultThreshold(uint8 converterNum, uint16 ovFaultThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOVFaultThreshold")`;
uint16 `$INSTANCE_NAME`_GetOVFaultThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOVFaultThreshold")`;
void `$INSTANCE_NAME`_SetOCWarnThreshold(uint8 converterNum, float ocWarnThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOCWarnThreshold")`;
float `$INSTANCE_NAME`_GetOCWarnThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOCWarnThreshold")`;
void `$INSTANCE_NAME`_SetOCFaultThreshold(uint8 converterNum, float ocFaultThreshold) \
     `=ReentrantKeil($INSTANCE_NAME . "_SetOCFaultThreshold")`;
float `$INSTANCE_NAME`_GetOCFaultThreshold(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetOCFaultThreshold")`;
uint16 `$INSTANCE_NAME`_GetConverterVoltage(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetConverterVoltage")`;
float `$INSTANCE_NAME`_GetConverterCurrent(uint8 converterNum) \
       `=ReentrantKeil($INSTANCE_NAME . "_GetConverterCurrent")`;
float `$INSTANCE_NAME`_GetAuxiliaryVoltage(uint8 auxNum) `=ReentrantKeil($INSTANCE_NAME . "_GetAuxiliaryVoltage")`;
void `$INSTANCE_NAME`_IsrStart(void);
void `$INSTANCE_NAME`_Calibrate(void) `=ReentrantKeil($INSTANCE_NAME . "_Calibrate")`;
CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);


/**************************************
*           Parameter Defaults        
**************************************/

/* Default config values from user parameters */

#define `$INSTANCE_NAME`_NUM_CONVERTERS                  (`$NumConverters`u)
#define `$INSTANCE_NAME`_NUM_AUX_INPUTS                  (`$NumAuxChannels`u)
#define `$INSTANCE_NAME`_DEFAULT_OV_FAULT_MODE           (`$FaultSources_OV`u)
#define `$INSTANCE_NAME`_DEFAULT_OC_FAULT_MODE           (`$FaultSources_OC`u)
#define `$INSTANCE_NAME`_DEFAULT_UV_FAULT_MODE           (`$FaultSources_UV`u)
#define `$INSTANCE_NAME`_DEFAULT_OV_WARN_MODE            (`$WarnSources_OV`u)
#define `$INSTANCE_NAME`_DEFAULT_UV_WARN_MODE            (`$WarnSources_UV`u)
#define `$INSTANCE_NAME`_DEFAULT_OC_WARN_MODE            (`$WarnSources_OC`u)
#define `$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE            (`$VoltageFilterType`u)
#define `$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE            (`$CurrentFilterType`u)
#define `$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE         (`$AuxFilterType`u)
#define `$INSTANCE_NAME`_DEFAULT_DIFF_CURRENT_RANGE      (`$DiffCurrentRange`u)
#define `$INSTANCE_NAME`_CAL_PIN_EXPOSED                 (`$ExposeCalPin`u)
#define `$INSTANCE_NAME`_DEFAULT_PGOOD_CONFIG            (`$PgoodConfig`u)


/* Total number of active current sources */
`$NumActiveCurrentSources`

/* structure variable declarations */
extern `$INSTANCE_NAME`_ADC_CTL_STRUCT CYXDATA `$INSTANCE_NAME`_voltCtl[];
#if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
    extern `$INSTANCE_NAME`_ADC_CTL_STRUCT CYXDATA `$INSTANCE_NAME`_ampCtl[];
#endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */

#if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
    extern `$INSTANCE_NAME`_ADC_CTL_STRUCT CYXDATA `$INSTANCE_NAME`_auxVoltCtl[];
#endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */

/* array declarations */
extern uint8 `$INSTANCE_NAME`_VoltageType[];

#if (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0)
    extern uint8 `$INSTANCE_NAME`_CurrentType[];
    extern uint8 `$INSTANCE_NAME`_ActiveCurrentChan[];
#endif /* (`$INSTANCE_NAME`_NUM_CURRENT_SOURCES > 0) */

#if (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0)
    extern uint8 `$INSTANCE_NAME`_AuxVoltageType[];
#endif /* (`$INSTANCE_NAME`_NUM_AUX_INPUTS > 0) */


/**************************************
*           API Constants        
**************************************/

#define `$INSTANCE_NAME`_MAX_CONVERTERS                  (32u)

/* default warn and fault modes */
#define `$INSTANCE_NAME`_DEFAULT_WARN_MODE               (`$INSTANCE_NAME`_DEFAULT_OV_WARN_MODE | \
                                                          (`$INSTANCE_NAME`_DEFAULT_UV_WARN_MODE << 1) | \
                                                          (`$INSTANCE_NAME`_DEFAULT_OC_WARN_MODE << 2))
#define `$INSTANCE_NAME`_DEFAULT_FAULT_MODE              (`$INSTANCE_NAME`_DEFAULT_OV_FAULT_MODE | \
                                                          (`$INSTANCE_NAME`_DEFAULT_UV_FAULT_MODE << 1) | \
                                                          (`$INSTANCE_NAME`_DEFAULT_OC_FAULT_MODE << 2))
#define `$INSTANCE_NAME`_WARN_MODE_MASK                  (0x07u)
#define `$INSTANCE_NAME`_FAULT_MODE_MASK                 (0x07u)

#define `$INSTANCE_NAME`_CYTRUE                           1
#define `$INSTANCE_NAME`_CYFALSE                          0

#define `$INSTANCE_NAME`_MAX_VOLT_CHAN                   (32u)
#define `$INSTANCE_NAME`_MAX_CURRENT_CHAN                (32u)


/* Differential current range defines */
#define `$INSTANCE_NAME`_DIFF_CURRENT_RANGE_64MV         (0u)
#define `$INSTANCE_NAME`_DIFF_CURRENT_RANGE_128MV        (1u)

/* ADC configuration defines */
/* ADC Configuration #1 : +/-2048 mV range      */
#define `$INSTANCE_NAME`_RANGE_2048                      (1u)
/* ADC Configuration #2 or #3 : low-voltage range   */
#if (`$INSTANCE_NAME`_DEFAULT_DIFF_CURRENT_RANGE == `$INSTANCE_NAME`_DIFF_CURRENT_RANGE_64MV)
    #define `$INSTANCE_NAME`_RANGE_LOW                   (2u)
#else
    #define `$INSTANCE_NAME`_RANGE_LOW                   (3u)
#endif /* (`$INSTANCE_NAME`_DEFAULT_DIFF_CURRENT_RANGE == `$INSTANCE_NAME`_DIFF_CURRENT_RANGE_64MV) */
/* ADC Configuration #4 : +/-1024 mV range      */
#define `$INSTANCE_NAME`_RANGE_1024                      (4u)

/* 1st ADC State, reads 1st voltage */
#define `$INSTANCE_NAME`_ISR_STATE_0                     (0u)

/* Priority of the ADC_IRQ interrupt. */
#define `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER           `$INSTANCE_NAME`_ADC_IRQ__INTC_PRIOR_NUM
#define `$INSTANCE_NAME`_IRQ__INTC_NUMBER                `$INSTANCE_NAME`_ADC_IRQ__INTC_NUMBER

/* Default warn and fault masks */
#if (`$INSTANCE_NAME`_NUM_CONVERTERS == `$INSTANCE_NAME`_MAX_CONVERTERS)
    #define `$INSTANCE_NAME`_DEFAULT_WARN_MASK           (0xffffffffu)
    #define `$INSTANCE_NAME`_DEFAULT_FAULT_MASK          (0xffffffffu)
#else
    #define `$INSTANCE_NAME`_DEFAULT_WARN_MASK           ((1u << `$INSTANCE_NAME`_NUM_CONVERTERS) - 1)
    #define `$INSTANCE_NAME`_DEFAULT_FAULT_MASK          ((1u << `$INSTANCE_NAME`_NUM_CONVERTERS) - 1)
#endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS == `$INSTANCE_NAME`_NUM_CONVERTERS) */

/* Filter type defines */
#define `$INSTANCE_NAME`_FILTER_TYPE_NONE                (0u)
#define `$INSTANCE_NAME`_FILTER_TYPE_4                   (1u)
#define `$INSTANCE_NAME`_FILTER_TYPE_8                   (2u)
#define `$INSTANCE_NAME`_FILTER_TYPE_16                  (3u)
#define `$INSTANCE_NAME`_FILTER_TYPE_32                  (4u)

/* Voltage Filter type define */
#if (`$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE)
    #define `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE         (1u)
#elif (`$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_4)
    #define `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE         (4u)
#elif (`$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_8)
    #define `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE         (8u)
#elif (`$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_16)
    #define `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE         (16u)
#else
    #define `$INSTANCE_NAME`_VOLTAGE_FILTER_SIZE         (32u)
#endif /* (`$INSTANCE_NAME`_DEFAULT_VFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE) */

/* Current Filter type define */
#if (`$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE)
    #define `$INSTANCE_NAME`_CURRENT_FILTER_SIZE         (1u)
#elif (`$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_4)
    #define `$INSTANCE_NAME`_CURRENT_FILTER_SIZE         (4u)
#elif (`$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_8)
    #define `$INSTANCE_NAME`_CURRENT_FILTER_SIZE         (8u)
#elif (`$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_16)
    #define `$INSTANCE_NAME`_CURRENT_FILTER_SIZE         (16u)
#else
    #define `$INSTANCE_NAME`_CURRENT_FILTER_SIZE         (32u)
#endif /* (`$INSTANCE_NAME`_DEFAULT_CFILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE) */

/* Auxiliary voltage Filter type define */
#if (`$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE)
    #define `$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE     (1u)
#elif (`$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_4)
    #define `$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE     (4u)
#elif (`$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_8)
    #define `$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE     (8u)
#elif (`$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_16)
    #define `$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE     (16u)
#else
    #define `$INSTANCE_NAME`_AUX_VOLTAGE_FILTER_SIZE     (32u)
#endif /* (`$INSTANCE_NAME`_DEFAULT_AUX_FILTER_TYPE == `$INSTANCE_NAME`_FILTER_TYPE_NONE) */

/* Define for possible max number of combinations */
#define `$INSTANCE_NAME`_MAX_CHANNELS                    (`$INSTANCE_NAME`_NUM_CONVERTERS + \
                                                         `$INSTANCE_NAME`_NUM_CURRENT_SOURCES + \
                                                         `$INSTANCE_NAME`_NUM_AUX_INPUTS)

/* No Warning check for this rail */
#define `$INSTANCE_NAME`_WARN_DISABLED                   (0u)   
/* Start delay until Warning checks begin */
#define `$INSTANCE_NAME`_WARN_TIME_0                     (1u)   
/* Check Warning checks for this ADC channel */
#define `$INSTANCE_NAME`_WARN_FAULT_ENABLED              (20u)  


/********************************************
* Calibration state defines 
********************************************/

/* define to interrupt normal ADC reading and to start calibration */
#define `$INSTANCE_NAME`_STATE_CAL                       (255u)
/* one-time ADC ISR setup at reset */
#define `$INSTANCE_NAME`_CAL_START                       (0u)
/* +/-2048 ZERO */
#define `$INSTANCE_NAME`_CAL_CFG1Z                       (1u)
/* +/-2048 read of -Vref */
#define `$INSTANCE_NAME`_CAL_CFG1G                       (2u)
/* +/-2048 read of 2 x PGA x Vref */
#define `$INSTANCE_NAME`_CAL_PGAZ                        (3u)
/* +/-256  ZERO */
#define `$INSTANCE_NAME`_CAL_CFG2Z                       (4u)
/* +/-1024 read of cal input      */
#define `$INSTANCE_NAME`_CAL_CFG2Ga                      (5u)   
/* +/-64/128mv  read of cal input */
#define `$INSTANCE_NAME`_CAL_CFG2Gb                      (6u)     
#define `$INSTANCE_NAME`_CAL_CFG2G                       (7u)
/* +/-1024 ZERO */
#define `$INSTANCE_NAME`_CAL_CFG3Z                       (8u)
/* +/-1024 read of -Vref */
#define `$INSTANCE_NAME`_CAL_CFG3G                       (9u)


/* Calibration oversample related defines */
#define `$INSTANCE_NAME`_CAL2A_OVER_EXP                  (4u)
/* _FILT_SZ  - number of samples to oversample */
#define `$INSTANCE_NAME`_CAL2A_FILT_SZ                   (1u << `$INSTANCE_NAME`_CAL2A_OVER_EXP)

/*******************************************************************************
* Filtered raw data from the ADC
*  filter = filter - (filter / SCALE_VAL) + newData;
*  output = filter / SCALE_VAL;
*******************************************************************************/
#define `$INSTANCE_NAME`_FILTER_SHIFT                    (4u)
#define `$INSTANCE_NAME`_SCALE_VAL                       (1u << `$INSTANCE_NAME`_FILTER_SHIFT)
#define `$INSTANCE_NAME`_INITIALIZE_IIR_FILTER           (1u)

/* Defines for voltage types */
#define `$INSTANCE_NAME`_VOLTAGE_TYPE_SINGLE             (0u)
#define `$INSTANCE_NAME`_VOLTAGE_TYPE_DIFF               (01u)
/* Defines for current types */
#define `$INSTANCE_NAME`_CURRENT_TYPE_NA                 (0u)
#define `$INSTANCE_NAME`_CURRENT_TYPE_DIRECT             (01u)
#define `$INSTANCE_NAME`_CURRENT_TYPE_CSA                (02u)

/* Defines for Auxiliary voltage types */
#define `$INSTANCE_NAME`_AUX_VOLTAGE_SINGLE              (0u)
#define `$INSTANCE_NAME`_AUX_VOLTAGE_64MV_DIFF           (1u)
#define `$INSTANCE_NAME`_AUX_VOLTAGE_2048MV_DIFF         (2u)
#define `$INSTANCE_NAME`_AUX_VOLTAGE_128MV_DIFF          (3u)

/* Defines for Register masks */
#define `$INSTANCE_NAME`_EOC_MASK                        (0x04u)
#define `$INSTANCE_NAME`_FAULT_MASK                      (0x01u)
#define `$INSTANCE_NAME`_WARN_MASK                       (0x02u)

/* Defines for reseting fault status */
#define `$INSTANCE_NAME`_RESET_OV_FAULT_STATUS           (0x00u)
#define `$INSTANCE_NAME`_RESET_UV_FAULT_STATUS           (0x00u)
#define `$INSTANCE_NAME`_RESET_OC_FAULT_STATUS           (0x00u)

/* Defines for resetting warn status */
#define `$INSTANCE_NAME`_RESET_OV_WARN_STATUS            (0x00u)
#define `$INSTANCE_NAME`_RESET_UV_WARN_STATUS            (0x00u)
#define `$INSTANCE_NAME`_RESET_OC_WARN_STATUS            (0x00u)

/* Channel Type defines */
#define `$INSTANCE_NAME`_VOLTAGE                         (0u)
#define `$INSTANCE_NAME`_CURRENT                         (1u)
#define `$INSTANCE_NAME`_AUXVOLTAGE                      (2u)

/* Threshold maximum range define */
#define `$INSTANCE_NAME`_THRESHOLD_MAX_RANGE             (0xFFFFu)

/* Number of voltage and current measurements */
#define `$INSTANCE_NAME`_TOTAL_V_I_MEASUREMENTS          (`$INSTANCE_NAME`_NUM_CONVERTERS + \
                                                          `$INSTANCE_NAME`_NUM_CURRENT_SOURCES)


/* Warn Source Status bits related defines */
#define `$INSTANCE_NAME`_ENABLE_OV_WARN_SOURCE           (0x01u)
#define `$INSTANCE_NAME`_ENABLE_UV_WARN_SOURCE           (0x02u)
#define `$INSTANCE_NAME`_ENABLE_OC_WARN_SOURCE           (0x04u)

/* Fault Source Status bits related defines */
#define `$INSTANCE_NAME`_ENABLE_OV_FAULT_SOURCE          (0x01u)
#define `$INSTANCE_NAME`_ENABLE_UV_FAULT_SOURCE          (0x02u)
#define `$INSTANCE_NAME`_ENABLE_OC_FAULT_SOURCE          (0x04u)

/* Warn Source Mask */
#define `$INSTANCE_NAME`_OV_WARN_SOURCE_MASK             (0x01u)
#define `$INSTANCE_NAME`_UV_WARN_SOURCE_MASK             (0x02u)
#define `$INSTANCE_NAME`_OC_WARN_SOURCE_MASK             (0x04u)

/* Fault Source Mask */
#define `$INSTANCE_NAME`_OV_FAULT_SOURCE_MASK            (0x01u)
#define `$INSTANCE_NAME`_UV_FAULT_SOURCE_MASK            (0x02u)
#define `$INSTANCE_NAME`_OC_FAULT_SOURCE_MASK            (0x04u)

/* Number of converters related defines */
#define `$INSTANCE_NAME`_CONVERTERS_8                    (8u)
#define `$INSTANCE_NAME`_CONVERTERS_16                   (16u)
#define `$INSTANCE_NAME`_CONVERTERS_24                   (24u)

/* Pgood control register mask */
#define `$INSTANCE_NAME`_PGOOD_CTRL_MASK                 (0xffu)
#if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24)
    #define `$INSTANCE_NAME`_PGOOD_CTRL_25_32_MASK           ((1u <<(`$INSTANCE_NAME`_NUM_CONVERTERS - \
                                                          `$INSTANCE_NAME`_CONVERTERS_24)) - 1) 
#endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_24) */

#if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16)
    #define `$INSTANCE_NAME`_PGOOD_CTRL_17_24_MASK           ((1u <<(`$INSTANCE_NAME`_NUM_CONVERTERS - \
                                                          `$INSTANCE_NAME`_CONVERTERS_16)) - 1) 
#endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_16) */

#if (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8)
    #define `$INSTANCE_NAME`_PGOOD_CTRL_9_16_MASK           ((1u <<(`$INSTANCE_NAME`_NUM_CONVERTERS - \
                                                          `$INSTANCE_NAME`_CONVERTERS_8)) - 1) 
#endif /* (`$INSTANCE_NAME`_NUM_CONVERTERS > `$INSTANCE_NAME`_CONVERTERS_8) */
#define `$INSTANCE_NAME`_PGOOD_CTRL_1_8_MASK           ((1u <<`$INSTANCE_NAME`_NUM_CONVERTERS) - 1)
                                                          
/* PGOOD configuration related defines */
#define `$INSTANCE_NAME`_PGOOD_GLOBAL                  (0u)
#define `$INSTANCE_NAME`_PGOOD_INDIVIDUAL              (1u)

/* Defines for AMux channel numbers */
#define `$INSTANCE_NAME`_PGA_OUT_CHAN                  (37u)
#define `$INSTANCE_NAME`_AUX_IN_CHAN                   (32u)
#define `$INSTANCE_NAME`_CAL_IN_CHAN                   (36u)
#define `$INSTANCE_NAME`_CSA_IN_CHAN                   (37u)

/* ADC register related defines */
#define `$INSTANCE_NAME`_ADC_VN_VSSA                   (0x40u)
#define `$INSTANCE_NAME`_ADC_VN_VSSA_VP_VSSA           (0x44u)
#define `$INSTANCE_NAME`_ADC_VN_VREF_VP_VSSA           (0x24u)
#define `$INSTANCE_NAME`_ADC_S12_ON                    (0x10u)
#define `$INSTANCE_NAME`_ADC_VP_VSSA                   (0x04u)
#define `$INSTANCE_NAME`_ADC_VN_ABUS1                  (0x02u)

/* Voltage scale for auxiliary voltage measurement */
#define `$INSTANCE_NAME`_VOLTAGE_SCALE                 (1000u)


/******************************************************************************
*        Registers          
******************************************************************************/
/* Control register to generate warn, fault and eoc signal */
#define `$INSTANCE_NAME`_CONTROL1_REG        (* (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL1_PTR        ( (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$Ctrl_Reg_RplcmntString`_ctrlreg1__CONTROL_REG)
/* Control register to generate pgood signal for converters between 1 and 8 */
#define `$INSTANCE_NAME`_PGOOD_CONTROL1_REG  (* (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__CONTROL_REG) 
#define `$INSTANCE_NAME`_PGOOD_CONTROL1_PTR  ( (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_1_8_Ctrl1__CONTROL_REG)
/* Control register to generate pgood signal for cnverters between 9 and 16 */
#define `$INSTANCE_NAME`_PGOOD_CONTROL2_REG  (* (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__CONTROL_REG) 
#define `$INSTANCE_NAME`_PGOOD_CONTROL2_PTR  ( (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_9_16_Ctrl2__CONTROL_REG)
/* Control register to generate pgood signal for converters between 17 and 24 */ 
#define `$INSTANCE_NAME`_PGOOD_CONTROL3_REG  (* (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__CONTROL_REG) 
#define `$INSTANCE_NAME`_PGOOD_CONTROL3_PTR  ( (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_17_24_Ctrl3__CONTROL_REG)
/* Control register to generate pgood signal for converters between 15 and 32 */ 
#define `$INSTANCE_NAME`_PGOOD_CONTROL4_REG  (* (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__CONTROL_REG) 
#define `$INSTANCE_NAME`_PGOOD_CONTROL4_PTR  ( (reg8 *) `$INSTANCE_NAME`_B_PowerMonitor_`$CtrlReg_RplcString`_PM_25_32_Ctrl4__CONTROL_REG)

#endif /* CY_POWERMONITOR_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
