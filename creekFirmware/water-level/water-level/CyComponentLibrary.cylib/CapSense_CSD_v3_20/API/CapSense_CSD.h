/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the CapSense CSD
*  component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semicondu)ctor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end u)ser license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CAPSENSE_CSD_`$INSTANCE_NAME`_H)
#define CY_CAPSENSE_CSD_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice_trm.h"
#include "CyLib.h"

`$Include`


/***************************************
*   Condition compilation parameters
***************************************/

/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5LP) */


#define `$INSTANCE_NAME`_DESIGN_TYPE                (`$NumberOfChannels`u)

#define `$INSTANCE_NAME`_CONNECT_INACTIVE_SNS       (`$ConnectInactiveSensors`u)
#define `$INSTANCE_NAME`_IS_COMPLEX_SCANSLOTS       (`$IsComplexScanSlots`u)

#define `$INSTANCE_NAME`_CLOCK_SOURCE               (`$ClockSource`u)

#define `$INSTANCE_NAME`_CURRENT_SOURCE             (`$CurrentSource`u)
#define `$INSTANCE_NAME`_IDAC_RANGE_VALUE           (`$IdacRange`u)

#define `$INSTANCE_NAME`_PRESCALER_OPTIONS          (`$PrescalerOptions`u)
#define `$INSTANCE_NAME`_MULTIPLE_PRESCALER_ENABLED (`$MultipleAnalogSwitchDivider`u)

#define `$INSTANCE_NAME`_PRS_OPTIONS                (`$PrsOptions`u)
#define `$INSTANCE_NAME`_SCANSPEED_VALUE            (`$ScanSpeed`u)

#define `$INSTANCE_NAME`_VREF_OPTIONS               (`$VrefOptions`u)

#define `$INSTANCE_NAME`_WATER_PROOF                (`$WaterProofingEnabled`u)

#define `$INSTANCE_NAME`_TUNING_METHOD              (`$TuningMethod`u)
#define `$INSTANCE_NAME`_TUNER_API_GENERATE         (`$EnableTuneHelper`u)

#define `$INSTANCE_NAME`_IMPLEMENTATION_CH0         (`$Implementation_CH0`u)
#define `$INSTANCE_NAME`_IMPLEMENTATION_CH1         (`$Implementation_CH1`u)

#define `$INSTANCE_NAME`_GUARD_SENSOR               (`$GuardSensorEnable`u)

/* Design types definitions */
#define `$INSTANCE_NAME`_ONE_CHANNEL_DESIGN         (1u)
#define `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN        (2u)

/* Clock sources definitions */
#define `$INSTANCE_NAME`_INTERNAL_CLOCK             (0u)
#define `$INSTANCE_NAME`_EXTERNAL_CLOCK             (1u)

/* Current source definitions */
#define `$INSTANCE_NAME`_EXTERNAL_RB                (0u)
#define `$INSTANCE_NAME`_IDAC_SOURCE                (1u)
#define `$INSTANCE_NAME`_IDAC_SINK                  (2u)

/* Prescaler option definitions */
#define `$INSTANCE_NAME`_PRESCALER_NONE             (0u)
#define `$INSTANCE_NAME`_PRESCALER_UDB              (1u)
#define `$INSTANCE_NAME`_PRESCALER_FF               (2u)

/* Prs options definitions */
#define `$INSTANCE_NAME`_PRS_NONE                   (0u)
#define `$INSTANCE_NAME`_PRS_8BITS                  (1u)
#define `$INSTANCE_NAME`_PRS_16BITS                 (2u)
#define `$INSTANCE_NAME`_PRS_16BITS_4X              (3u)

/* Seed values */
#define `$INSTANCE_NAME`_PRS8_SEED_VALUE            (0xFFu)
#define `$INSTANCE_NAME`_PRS16_SEED_VALUE           (0xFFFFu)

/* Reference source types definitions */
#define `$INSTANCE_NAME`_VREF_REFERENCE_1_024V      (0u)
#define `$INSTANCE_NAME`_VREF_REFERENCE_1_2V        (1u)
#define `$INSTANCE_NAME`_VREF_VDAC                  (2u)

/* Connection of inactive sensors definitions */
#define `$INSTANCE_NAME`_CIS_GND                    (0u)
#define `$INSTANCE_NAME`_CIS_HIGHZ                  (1u)
#define `$INSTANCE_NAME`_CIS_SHIELD                 (2u)

/* Method of tunning */
#define `$INSTANCE_NAME`_NO_TUNING                  (0u)
#define `$INSTANCE_NAME`_MANUAL_TUNING              (1u)
#define `$INSTANCE_NAME`_AUTO_TUNING                (2u)

/* Measure Channel implementation */
#define `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF  (0u)
#define `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_UDB (1u)

/* Guard sensor definition */
#define `$INSTANCE_NAME`_GUARD_SENSOR_DISABLE       (0u)
#define `$INSTANCE_NAME`_GUARD_SENSOR_ENABLE        (1u)


/***************************************
*       Type defines
***************************************/

/* Structure to save registers before go to sleep */
typedef struct _`$INSTANCE_NAME`_BACKUP_STRUCT
{
    uint8 enableState;
    
    /* Set ScanSpeed */
    #if (CY_PSOC5A)
        uint8 scanspeed;
    #endif  /* (CY_PSOC5A) */

    /* Set CONTROL_REG */
    uint8 ctrlreg;
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
uint8 `$INSTANCE_NAME`_IsBusy(void) `=ReentrantKeil($INSTANCE_NAME . "_IsBusy")`;
void `$INSTANCE_NAME`_ScanSensor(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_ScanSensor")`;
void `$INSTANCE_NAME`_ScanEnabledWidgets(void) `=ReentrantKeil($INSTANCE_NAME . "_ScanEnabledWidgets")`;
void `$INSTANCE_NAME`_SetScanSlotSettings(uint8 slot) `=ReentrantKeil($INSTANCE_NAME . "_SetScanSlotSettings")`;
uint16 `$INSTANCE_NAME`_ReadSensorRaw(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_ReadSensorRaw")`;
void `$INSTANCE_NAME`_ClearSensors(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearSensors")`;
void `$INSTANCE_NAME`_EnableSensor(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_EnableSensor")`;
void `$INSTANCE_NAME`_DisableSensor(uint8 sensor) `=ReentrantKeil($INSTANCE_NAME . "_DisableSensor")`;

void `$INSTANCE_NAME`_SetAnalogSwitchesSource(uint8 src) `=ReentrantKeil($INSTANCE_NAME . "_SetAnalogSwitchesSource")`;

#if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB)
    void `$INSTANCE_NAME`_SetRBleed(uint8 rbeed) `=ReentrantKeil($INSTANCE_NAME . "_SetRBleed")`;
#endif  /* (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_EXTERNAL_RB) */

/* Interrupt handler */
CY_ISR_PROTO(`$INSTANCE_NAME`_IsrCH0_ISR);
#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    CY_ISR_PROTO(`$INSTANCE_NAME`_IsrCH1_ISR);
#endif  /* (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN) */


/***************************************
*           API Constants
***************************************/

`$DefineConstants`

/* Vdac value for Vref = Vdac */
#define `$INSTANCE_NAME`_VREF_VDAC_VALUE            (`$VrefValue`u)

/* Scan Speed Type */
#define `$INSTANCE_NAME`_SCAN_SPEED_ULTRA_FAST      (0x01u)
#define `$INSTANCE_NAME`_SCAN_SPEED_FAST            (0x03u)
#define `$INSTANCE_NAME`_SCAN_SPEED_NORMAL          (0x07u)
#define `$INSTANCE_NAME`_SCAN_SPEED_SLOW            (0x0Fu)

/* PWM Resolution */
#define `$INSTANCE_NAME`_PWM_WINDOW_APPEND          (0xFEu)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_8_BITS      (0x00u)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_9_BITS      (0x01u)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_10_BITS     (0x03u)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_11_BITS     (0x07u)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_12_BITS     (0x0Fu)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_13_BITS     (0x1Fu)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_14_BITS     (0x3Fu)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_15_BITS     (0x7Fu)
#define `$INSTANCE_NAME`_PWM_RESOLUTION_16_BITS     (0xFFu)

/* Software Status Register Bit Masks */
#define `$INSTANCE_NAME`_SW_STS_BUSY                (0x01u)
/* Software Status Register Bit Masks */
#define `$INSTANCE_NAME`_SW_CTRL_SINGLE_SCAN        (0x80u)

/* Init Idac current */
#define `$INSTANCE_NAME`_TURN_OFF_IDAC              (0x00u)

/* Rbleed definitions */
#define `$INSTANCE_NAME`_RBLEED1                    (0u)
#define `$INSTANCE_NAME`_RBLEED2                    (1u)
#define `$INSTANCE_NAME`_RBLEED3                    (2u)

/* Flag of complex scan slot */
#define `$INSTANCE_NAME`_COMPLEX_SS_FLAG            (0x80u)
#define `$INSTANCE_NAME`_CHANNEL1_FLAG              (0x80u)


#define `$INSTANCE_NAME`_ANALOG_SWITCHES_SRC_PRESCALER (0x01u)
#define `$INSTANCE_NAME`_ANALOG_SWITCHES_SRC_PRS       (0x02u)


/***************************************
*             Registers
***************************************/

/* Control REG */
#define `$INSTANCE_NAME`_CONTROL_REG    (*(reg8 *) \
                                            `$INSTANCE_NAME`_ClockGen_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG )
#define `$INSTANCE_NAME`_CONTROL_PTR    ( (reg8 *) \
                                            `$INSTANCE_NAME`_ClockGen_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG )

/* Clock Gen - ScanSpeed REGs definitions */
#define `$INSTANCE_NAME`_SCANSPEED_AUX_CONTROL_REG (*(reg8 *) `$INSTANCE_NAME`_ClockGen_ScanSpeed__CONTROL_AUX_CTL_REG )
#define `$INSTANCE_NAME`_SCANSPEED_AUX_CONTROL_PTR ( (reg8 *) `$INSTANCE_NAME`_ClockGen_ScanSpeed__CONTROL_AUX_CTL_REG )

#define `$INSTANCE_NAME`_SCANSPEED_PERIOD_REG      (*(reg8 *) `$INSTANCE_NAME`_ClockGen_ScanSpeed__PERIOD_REG )
#define `$INSTANCE_NAME`_SCANSPEED_PERIOD_PTR      ( (reg8 *) `$INSTANCE_NAME`_ClockGen_ScanSpeed__PERIOD_REG )

#define `$INSTANCE_NAME`_SCANSPEED_COUNTER_REG     (*(reg8 *) `$INSTANCE_NAME`_ClockGen_ScanSpeed__COUNT_REG )
#define `$INSTANCE_NAME`_SCANSPEED_COUNTER_PTR     ( (reg8 *) `$INSTANCE_NAME`_ClockGen_ScanSpeed__COUNT_REG )


/* Clock Gen - Prescaler REGs definitions */
#if (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB)
    #define `$INSTANCE_NAME`_PRESCALER_PERIOD_REG       (*(reg8 *) `$INSTANCE_NAME`_ClockGen_UDB_PrescalerDp_u0__D0_REG)
    #define `$INSTANCE_NAME`_PRESCALER_PERIOD_PTR       ( (reg8 *) `$INSTANCE_NAME`_ClockGen_UDB_PrescalerDp_u0__D0_REG)
    
    #define `$INSTANCE_NAME`_PRESCALER_COMPARE_REG      (*(reg8 *) `$INSTANCE_NAME`_ClockGen_UDB_PrescalerDp_u0__D1_REG)
    #define `$INSTANCE_NAME`_PRESCALER_COMPARE_PTR      ( (reg8 *) `$INSTANCE_NAME`_ClockGen_UDB_PrescalerDp_u0__D1_REG)
    
#elif (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_FF)
    #define `$INSTANCE_NAME`_PRESCALER_PERIOD_PTR       ( (reg16 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__PER0 )
    #define `$INSTANCE_NAME`_PRESCALER_COMPARE_PTR      ( (reg16 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__CNT_CMP0 )
    
    #define `$INSTANCE_NAME`_PRESCALER_CONTROL_REG      (*(reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__CFG0 )
    #define `$INSTANCE_NAME`_PRESCALER_CONTROL_PTR      ( (reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__CFG0 )
    
    #if (CY_PSOC5A)
        #define `$INSTANCE_NAME`_PRESCALER_CONTROL2_REG     (*(reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__CFG1 )
        #define `$INSTANCE_NAME`_PRESCALER_CONTROL2_PTR     ( (reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__CFG1 )
    #else
        #define `$INSTANCE_NAME`_PRESCALER_CONTROL2_REG     (*(reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__CFG2 )
        #define `$INSTANCE_NAME`_PRESCALER_CONTROL2_PTR     ( (reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__CFG2 )
    #endif  /* (CY_PSOC5A) */
    
    #define `$INSTANCE_NAME`_PRESCALER_ACT_PWRMGR_REG   (*(reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_PRESCALER_ACT_PWRMGR_PTR   ( (reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_PRESCALER_ACT_PWR_EN                 (`$INSTANCE_NAME`_ClockGen_FF_Prescaler__PM_ACT_MSK )
    
    #define `$INSTANCE_NAME`_PRESCALER_STBY_PWRMGR_REG  (*(reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_PRESCALER_STBY_PWRMGR_PTR  ( (reg8 *) `$INSTANCE_NAME`_ClockGen_FF_Prescaler__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_PRESCALER_STBY_PWR_EN                (`$INSTANCE_NAME`_ClockGen_FF_Prescaler__PM_STBY_MSK )

#else
    /* No prescaler */
#endif  /* (`$INSTANCE_NAME`_PRESCALER_OPTIONS == `$INSTANCE_NAME`_PRESCALER_UDB) */

/* PRS */
#if (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_8BITS)  
    /* Polynomial */
    #define `$INSTANCE_NAME`_POLYNOM_REG        (*(reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_PTR        ( (reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__D0_REG )
    /* Seed */
    #define `$INSTANCE_NAME`_SEED_REG           (*(reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SEED_PTR           ( (reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__A0_REG )
    /* Seed COPY */
    #define `$INSTANCE_NAME`_SEED_COPY_REG      (*(reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__F0_REG )
    #define `$INSTANCE_NAME`_SEED_COPY_PTR      ( (reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__F0_REG )
    /* Aux control */
    #define `$INSTANCE_NAME`_AUX_CONTROL_A_REG  (*(reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__DP_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_AUX_CONTROL_A_PTR  ( (reg8 *) `$INSTANCE_NAME`_ClockGen_sC8_PRSdp_u0__DP_AUX_CTL_REG )
        
#elif (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_16BITS)
    /* Polynomial */
    #define `$INSTANCE_NAME`_POLYNOM_REG        (*(reg16 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_PTR        ( (reg16 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__D0_REG )
    /* Seed */
    #define `$INSTANCE_NAME`_SEED_REG           (*(reg16 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SEED_PTR           ( (reg16 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__A0_REG )
    /* Seed COPY */
    #define `$INSTANCE_NAME`_SEED_COPY_REG      (*(reg16 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__F0_REG )
    #define `$INSTANCE_NAME`_SEED_COPY_PTR      ( (reg16 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__F0_REG )
    /* Aux control */
    #define `$INSTANCE_NAME`_AUX_CONTROL_A_REG  (*(reg8 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__DP_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_AUX_CONTROL_A_PTR  ( (reg8 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u0__DP_AUX_CTL_REG )
    
    #define `$INSTANCE_NAME`_AUX_CONTROL_B_REG  (*(reg8 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u1__DP_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_AUX_CONTROL_B_PTR  ( (reg8 *) `$INSTANCE_NAME`_ClockGen_sC16_PRSdp_u1__DP_AUX_CTL_REG )
    
#elif (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_16BITS_4X)
    /* Polynomial */   
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_REG      (*(reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__D1_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D1_PTR      ( (reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__D1_REG )
    
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_REG      (*(reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__D0_REG )
    #define `$INSTANCE_NAME`_POLYNOM_A__D0_PTR      ( (reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__D0_REG )
    /* Seed */
    #define `$INSTANCE_NAME`_SEED_A__A1_REG         (*(reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__A1_REG )
    #define `$INSTANCE_NAME`_SEED_A__A1_PTR         ( (reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__A1_REG )
    
    #define `$INSTANCE_NAME`_SEED_A__A0_REG         (*(reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__A0_REG )
    #define `$INSTANCE_NAME`_SEED_A__A0_PTR         ( (reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__A0_REG )
    /* Seed COPY */
    #define `$INSTANCE_NAME`_SEED_COPY_A__F1_REG    (*(reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__F1_REG )
    #define `$INSTANCE_NAME`_SEED_COPY_A__F1_PTR    ( (reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__F1_REG )
    
    #define `$INSTANCE_NAME`_SEED_COPY_A__F0_REG    (*(reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__F0_REG )
    #define `$INSTANCE_NAME`_SEED_COPY_A__F0_PTR    ( (reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__F0_REG )
    /* Aux control */
    #define `$INSTANCE_NAME`_AUX_CONTROL_A_REG  (*(reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__DP_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_AUX_CONTROL_A_PTR  ( (reg8 *) `$INSTANCE_NAME`_ClockGen_b0_PRSdp_a__DP_AUX_CTL_REG )
    
#else
    /* No PRS */
#endif  /* (`$INSTANCE_NAME`_PRS_OPTIONS == `$INSTANCE_NAME`_PRS_8BITS)  */

/* Measure REGs  definitions */
#if (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
    /* Window PWM */
    #define `$INSTANCE_NAME`_PWM_CH0_PERIOD_PTR         ( (reg16 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__PER0 )
    #define `$INSTANCE_NAME`_PWM_CH0_COUNTER_PTR        ( (reg16 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__CNT_CMP0 )
    
    #define `$INSTANCE_NAME`_PWM_CH0_CONTROL_REG        (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__CFG0 )
    #define `$INSTANCE_NAME`_PWM_CH0_CONTROL_PTR        ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__CFG0 )
    
    #if (CY_PSOC5A)
        #define `$INSTANCE_NAME`_PWM_CH0_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__CFG1 )
        #define `$INSTANCE_NAME`_PWM_CH0_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__CFG1 )
    #else
        #define `$INSTANCE_NAME`_PWM_CH0_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__CFG2 )
        #define `$INSTANCE_NAME`_PWM_CH0_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__CFG2 )
    #endif  /* (CY_PSOC5A) */
    
    #define `$INSTANCE_NAME`_PWM_CH0_ACT_PWRMGR_REG     (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_PWM_CH0_ACT_PWRMGR_PTR     ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_PWM_CH0_ACT_PWR_EN                   (`$INSTANCE_NAME`_MeasureCH0_FF_Window__PM_ACT_MSK )
    
    #define `$INSTANCE_NAME`_PWM_CH0_STBY_PWRMGR_REG    (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_PWM_CH0_STBY_PWRMGR_PTR    ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Window__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_PWM_CH0_STBY_PWR_EN                  (`$INSTANCE_NAME`_MeasureCH0_FF_Window__PM_STBY_MSK )
    
    /* Raw Counter */
    #define `$INSTANCE_NAME`_RAW_CH0_PERIOD_PTR         ( (reg16 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__PER0 )
    #define `$INSTANCE_NAME`_RAW_CH0_COUNTER_PTR        ( (reg16 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__CNT_CMP0 )
    
    #define `$INSTANCE_NAME`_RAW_CH0_CONTROL_REG        (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__CFG0 )
    #define `$INSTANCE_NAME`_RAW_CH0_CONTROL_PTR        ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__CFG0 )
    
    #if (CY_PSOC5A)
        #define `$INSTANCE_NAME`_RAW_CH0_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__CFG1 )
        #define `$INSTANCE_NAME`_RAW_CH0_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__CFG1 )
    #else
        #define `$INSTANCE_NAME`_RAW_CH0_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__CFG2 )
        #define `$INSTANCE_NAME`_RAW_CH0_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__CFG2 )
    #endif  /* (CY_PSOC5A) */
    
    #define `$INSTANCE_NAME`_RAW_CH0_ACT_PWRMGR_REG     (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_RAW_CH0_ACT_PWRMGR_PTR     ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_RAW_CH0_ACT_PWR_EN                   (`$INSTANCE_NAME`_MeasureCH0_FF_Counter__PM_ACT_MSK )
    
    #define `$INSTANCE_NAME`_RAW_CH0_STBY_PWRMGR_REG    (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_RAW_CH0_STBY_PWRMGR_PTR    ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_FF_Counter__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_RAW_CH0_STBY_PWR_EN                  (`$INSTANCE_NAME`_MeasureCH0_FF_Counter__PM_STBY_MSK )
#else
     /* Window PWM */
    #define `$INSTANCE_NAME`_PWM_CH0_COUNTER_LO_REG     (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__A0_REG )
    #define `$INSTANCE_NAME`_PWM_CH0_COUNTER_LO_PTR     ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__A0_REG )
    
    #define `$INSTANCE_NAME`_PWM_CH0_COUNTER_HI_REG     (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__A1_REG )
    #define `$INSTANCE_NAME`_PWM_CH0_COUNTER_HI_PTR     ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__A1_REG )
    
    #define `$INSTANCE_NAME`_PWM_CH0_PERIOD_LO_REG      (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__F0_REG )
    #define `$INSTANCE_NAME`_PWM_CH0_PERIOD_LO_PTR      ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__F0_REG )
    
    #define `$INSTANCE_NAME`_PWM_CH0_PERIOD_HI_REG      (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__F1_REG )
    #define `$INSTANCE_NAME`_PWM_CH0_PERIOD_HI_PTR      ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__F1_REG )
    
    #define `$INSTANCE_NAME`_PWM_CH0_ADD_VALUE_REG      (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__D0_REG )
    #define `$INSTANCE_NAME`_PWM_CH0_ADD_VALUE_PTR      ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__D0_REG )
    
    #define `$INSTANCE_NAME`_PWM_CH0_AUX_CONTROL_REG    (*(reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__DP_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_PWM_CH0_AUX_CONTROL_PTR    ( (reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH0_UDB_Window_u0__DP_AUX_CTL_REG )
    
    /* Raw Counter */
    #define `$INSTANCE_NAME`_RAW_CH0_COUNTER_LO_REG      (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__A0_REG )
    #define `$INSTANCE_NAME`_RAW_CH0_COUNTER_LO_PTR      ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__A0_REG )
    
    #define `$INSTANCE_NAME`_RAW_CH0_COUNTER_HI_REG      (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__A1_REG )
    #define `$INSTANCE_NAME`_RAW_CH0_COUNTER_HI_PTR      ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__A1_REG )
    
    #define `$INSTANCE_NAME`_RAW_CH0_PERIOD_LO_REG       (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__F0_REG )
    #define `$INSTANCE_NAME`_RAW_CH0_PERIOD_LO_PTR       ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__F0_REG )
    
    #define `$INSTANCE_NAME`_RAW_CH0_PERIOD_HI_REG       (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__F1_REG )
    #define `$INSTANCE_NAME`_RAW_CH0_PERIOD_HI_PTR       ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__F1_REG )
    
    #define `$INSTANCE_NAME`_RAW_CH0_ADD_VALUE_REG       (*(reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__D0_REG )
    #define `$INSTANCE_NAME`_RAW_CH0_ADD_VALUE_PTR       ( (reg8 *) `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__D0_REG )
    
    #define `$INSTANCE_NAME`_RAW_CH0_AUX_CONTROL_REG     (*(reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__DP_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_RAW_CH0_AUX_CONTROL_PTR     ( (reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH0_UDB_Counter_u0__DP_AUX_CTL_REG )

#endif  /* (`$INSTANCE_NAME`_IMPLEMENTATION_CH0 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF) */
    
#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    #if (`$INSTANCE_NAME`_IMPLEMENTATION_CH1 == `$INSTANCE_NAME`_MEASURE_IMPLEMENTATION_FF)
        /* Window PWM */
        #define `$INSTANCE_NAME`_PWM_CH1_PERIOD_PTR        ( (reg16 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__PER0 )
        #define `$INSTANCE_NAME`_PWM_CH1_COUNTER_PTR       ( (reg16 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__CNT_CMP0 )
        
        #define `$INSTANCE_NAME`_PWM_CH1_CONTROL_REG        (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__CFG0 )
        #define `$INSTANCE_NAME`_PWM_CH1_CONTROL_PTR        ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__CFG0 )
        
        #if (CY_PSOC5A)
            #define `$INSTANCE_NAME`_PWM_CH1_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__CFG1 )
            #define `$INSTANCE_NAME`_PWM_CH1_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__CFG1 )
        #else
            #define `$INSTANCE_NAME`_PWM_CH1_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__CFG2 )
            #define `$INSTANCE_NAME`_PWM_CH1_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__CFG2 )
        #endif  /* (CY_PSOC5A) */
        
        #define `$INSTANCE_NAME`_PWM_CH1_ACT_PWRMGR_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__PM_ACT_CFG )
        #define `$INSTANCE_NAME`_PWM_CH1_ACT_PWRMGR_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__PM_ACT_CFG )
        #define `$INSTANCE_NAME`_PWM_CH1_ACT_PWR_EN                 (`$INSTANCE_NAME`_MeasureCH1_FF_Window__PM_ACT_MSK )
        
        #define `$INSTANCE_NAME`_PWM_CH1_STBY_PWRMGR_REG (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__PM_STBY_CFG )
        #define `$INSTANCE_NAME`_PWM_CH1_STBY_PWRMGR_PTR ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Window__PM_STBY_CFG )
        #define `$INSTANCE_NAME`_PWM_CH1_STBY_PWR_EN               (`$INSTANCE_NAME`_MeasureCH1_FF_Window__PM_STBY_MSK )
            
        /* Raw Counter */
        #define `$INSTANCE_NAME`_RAW_CH1_PERIOD_PTR       ( (reg16 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__PER0 )
        #define `$INSTANCE_NAME`_RAW_CH1_COUNTER_PTR      ( (reg16 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__CNT_CMP0 )
        
        #define `$INSTANCE_NAME`_RAW_CH1_CONTROL_REG      (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__CFG0 )
        #define `$INSTANCE_NAME`_RAW_CH1_CONTROL_PTR      ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__CFG0 )
        
        #if (CY_PSOC5A)
            #define `$INSTANCE_NAME`_RAW_CH1_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__CFG1 )
            #define `$INSTANCE_NAME`_RAW_CH1_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__CFG1 )
        #else
            #define `$INSTANCE_NAME`_RAW_CH1_CONTROL2_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__CFG2 )
            #define `$INSTANCE_NAME`_RAW_CH1_CONTROL2_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__CFG2 )
        #endif  /* (CY_PSOC5A) */
        
        #define `$INSTANCE_NAME`_RAW_CH1_ACT_PWRMGR_REG  (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__PM_ACT_CFG )
        #define `$INSTANCE_NAME`_RAW_CH1_ACT_PWRMGR_PTR  ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__PM_ACT_CFG )
        #define `$INSTANCE_NAME`_RAW_CH1_ACT_PWR_EN                (`$INSTANCE_NAME`_MeasureCH1_FF_Counter__PM_ACT_MSK )
        
        #define `$INSTANCE_NAME`_RAW_CH1_STBY_PWRMGR_REG (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__PM_STBY_CFG)
        #define `$INSTANCE_NAME`_RAW_CH1_STBY_PWRMGR_PTR ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_FF_Counter__PM_STBY_CFG)
        #define `$INSTANCE_NAME`_RAW_CH1_STBY_PWR_EN               (`$INSTANCE_NAME`_MeasureCH1_FF_Counter__PM_STBY_MSK)
    
    #else
        /* Window PWM */
        #define `$INSTANCE_NAME`_PWM_CH1_COUNTER_LO_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__A0_REG )
        #define `$INSTANCE_NAME`_PWM_CH1_COUNTER_LO_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__A0_REG )
        
        #define `$INSTANCE_NAME`_PWM_CH1_COUNTER_HI_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__A1_REG )
        #define `$INSTANCE_NAME`_PWM_CH1_COUNTER_HI_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__A1_REG )
        
        #define `$INSTANCE_NAME`_PWM_CH1_PERIOD_LO_REG    (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__F0_REG )
        #define `$INSTANCE_NAME`_PWM_CH1_PERIOD_LO_PTR    ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__F0_REG )
        
        #define `$INSTANCE_NAME`_PWM_CH1_PERIOD_HI_REG    (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__F1_REG )
        #define `$INSTANCE_NAME`_PWM_CH1_PERIOD_HI_PTR    ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__F1_REG )
        
        #define `$INSTANCE_NAME`_PWM_CH1_ADD_VALUE_REG    (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__D0_REG )
        #define `$INSTANCE_NAME`_PWM_CH1_ADD_VALUE_PTR    ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__D0_REG )
        
        #define `$INSTANCE_NAME`_PWM_CH1_AUX_CONTROL_REG  (*(reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__DP_AUX_CTL_REG )
        #define `$INSTANCE_NAME`_PWM_CH1_AUX_CONTROL_PTR  ( (reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH1_UDB_Window_u0__DP_AUX_CTL_REG )
        
        /* Raw Counter */
        #define `$INSTANCE_NAME`_RAW_CH1_COUNTER_LO_REG  (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__A0_REG )
        #define `$INSTANCE_NAME`_RAW_CH1_COUNTER_LO_PTR  ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__A0_REG )
        
        #define `$INSTANCE_NAME`_RAW_CH1_COUNTER_HI_REG  (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__A1_REG )
        #define `$INSTANCE_NAME`_RAW_CH1_COUNTER_HI_PTR  ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__A1_REG )
        
        #define `$INSTANCE_NAME`_RAW_CH1_PERIOD_LO_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__F0_REG )
        #define `$INSTANCE_NAME`_RAW_CH1_PERIOD_LO_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__F0_REG )
        
        #define `$INSTANCE_NAME`_RAW_CH1_PERIOD_HI_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__F1_REG )
        #define `$INSTANCE_NAME`_RAW_CH1_PERIOD_HI_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__F1_REG )
        
        #define `$INSTANCE_NAME`_RAW_CH1_ADD_VALUE_REG   (*(reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__D0_REG )
        #define `$INSTANCE_NAME`_RAW_CH1_ADD_VALUE_PTR   ( (reg8 *) `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__D0_REG )
        
        #define `$INSTANCE_NAME`_RAW_CH1_AUX_CONTROL_REG  (*(reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__DP_AUX_CTL_REG )
        #define `$INSTANCE_NAME`_RAW_CH1_AUX_CONTROL_PTR  ( (reg8 *) \
                                                            `$INSTANCE_NAME`_MeasureCH1_UDB_Counter_u0__DP_AUX_CTL_REG )
        
    #endif  /* `$INSTANCE_NAME`_DESIGN_TYPE */
    
#endif  /* `$INSTANCE_NAME`_DESIGN_TYPE */


/* CapSense Buffer REGs definitions */
#define `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_REG           (*(reg8 *) `$INSTANCE_NAME`_BufCH0__CFG0 )
#define `$INSTANCE_NAME`_BufCH0_CAPS_CFG0_PTR           ( (reg8 *) `$INSTANCE_NAME`_BufCH0__CFG0 )

#define `$INSTANCE_NAME`_BufCH0_CAPS_CFG1_REG           (*(reg8 *) `$INSTANCE_NAME`_BufCH0__CFG1 )
#define `$INSTANCE_NAME`_BufCH0_CAPS_CFG1_PTR           ( (reg8 *) `$INSTANCE_NAME`_BufCH0__CFG1 )

#define `$INSTANCE_NAME`_BufCH0_ACT_PWRMGR_REG          (*(reg8 *) `$INSTANCE_NAME`_BufCH0__PM_ACT_CFG )
#define `$INSTANCE_NAME`_BufCH0_ACT_PWRMGR_PTR          ( (reg8 *) `$INSTANCE_NAME`_BufCH0__PM_ACT_CFG )
#define `$INSTANCE_NAME`_BufCH0_ACT_PWR_EN                        (`$INSTANCE_NAME`_BufCH0__PM_ACT_MSK )

#define `$INSTANCE_NAME`_BufCH0_STBY_PWRMGR_REG         (*(reg8 *) `$INSTANCE_NAME`_BufCH0__PM_STBY_CFG )
#define `$INSTANCE_NAME`_BufCH0_STBY_PWRMGR_PTR         ( (reg8 *) `$INSTANCE_NAME`_BufCH0__PM_STBY_CFG )
#define `$INSTANCE_NAME`_BufCH0_STBY_PWR_EN                       (`$INSTANCE_NAME`_BufCH0__PM_STBY_MSK )

#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    #define `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_REG       (*(reg8 *) `$INSTANCE_NAME`_BufCH1__CFG0 )
    #define `$INSTANCE_NAME`_BufCH1_CAPS_CFG0_PTR       ( (reg8 *) `$INSTANCE_NAME`_BufCH1__CFG0 )
    
    #define `$INSTANCE_NAME`_BufCH1_CAPS_CFG1_REG       (*(reg8 *) `$INSTANCE_NAME`_BufCH1__CFG1 )
    #define `$INSTANCE_NAME`_BufCH1_CAPS_CFG1_PTR       ( (reg8 *) `$INSTANCE_NAME`_BufCH1__CFG1 )
    
    #define `$INSTANCE_NAME`_BufCH1_ACT_PWRMGR_REG      (*(reg8 *) `$INSTANCE_NAME`_BufCH1__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_BufCH1_ACT_PWRMGR_PTR      ( (reg8 *) `$INSTANCE_NAME`_BufCH1__PM_ACT_CFG )
    #define `$INSTANCE_NAME`_BufCH1_ACT_PWR_EN                    (`$INSTANCE_NAME`_BufCH1__PM_ACT_MSK )
    
    #define `$INSTANCE_NAME`_BufCH1_STBY_PWRMGR_REG     (*(reg8 *) `$INSTANCE_NAME`_BufCH1__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_BufCH1_STBY_PWRMGR_PTR     ( (reg8 *) `$INSTANCE_NAME`_BufCH1__PM_STBY_CFG )
    #define `$INSTANCE_NAME`_BufCH1_STBY_PWR_EN                   (`$INSTANCE_NAME`_BufCH1__PM_STBY_MSK )
#endif  /* `$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN */

/* ISR Number and Priority to work with CyLib functions */
#define `$INSTANCE_NAME`_IsrCH0_ISR_NUMBER        (`$INSTANCE_NAME`_IsrCH0__INTC_NUMBER)
#define `$INSTANCE_NAME`_IsrCH0_ISR_PRIORITY      (`$INSTANCE_NAME`_IsrCH0__INTC_PRIOR_NUM)

#if (`$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN)
    #define `$INSTANCE_NAME`_IsrCH1_ISR_NUMBER        (`$INSTANCE_NAME`_IsrCH1__INTC_NUMBER)
    #define `$INSTANCE_NAME`_IsrCH1_ISR_PRIORITY      (`$INSTANCE_NAME`_IsrCH1__INTC_PRIOR_NUM)
#endif /* `$INSTANCE_NAME`_DESIGN_TYPE == `$INSTANCE_NAME`_TWO_CHANNELS_DESIGN */


/***************************************
*       Register Constants
***************************************/

/* Control Register definitions */
#define `$INSTANCE_NAME`_CTRL_SYNC_EN                       (0x01u)
#define `$INSTANCE_NAME`_CTRL_START                         (0x02u)
#define `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH0                (0x04u)
#define `$INSTANCE_NAME`_CTRL_WINDOW_EN__CH1                (0x08u)
/* Addtional bit to verify if component is enalbed */
#define `$INSTANCE_NAME`_CTRL_CAPSENSE_EN                   (0x80u)

#define `$INSTANCE_NAME`_IS_CAPSENSE_ENABLE(reg)            ( ((reg) & `$INSTANCE_NAME`_CTRL_CAPSENSE_EN) != 0u )

/* ClockGen defines */
/* Prescaler */
#define `$INSTANCE_NAME`_PRESCALER_CTRL_ENABLE          (0x01u)
#define `$INSTANCE_NAME`_PRESCALER_CTRL_MODE_CMP        (0x02u) 
#if (CY_PSOC5A)
    #define `$INSTANCE_NAME`_PRESCALER_CTRL_CMP_MODE_SHIFT  (0x01u)
#else
    #define `$INSTANCE_NAME`_PRESCALER_CTRL_CMP_MODE_SHIFT  (0x04u)
#endif /* (CY_PSOC5A) */
#define `$INSTANCE_NAME`_PRESCALER_CTRL_CMP_LESS_EQ         (0x02u << `$INSTANCE_NAME`_PRESCALER_CTRL_CMP_MODE_SHIFT)

/* Set PRS polynomial */
#define `$INSTANCE_NAME`_PRS8_DEFAULT_POLYNOM           (0xB8u)
#define `$INSTANCE_NAME`_PRS16_DEFAULT_POLYNOM          (0xB400u)

/* Scan Speed */
#define `$INSTANCE_NAME`_SCANSPEED_CTRL_ENABLE          (0x20u)

/* Measure defines */
/* FF Timers */
#define `$INSTANCE_NAME`_MEASURE_FULL_RANGE             (0xFFFFu)
#define `$INSTANCE_NAME`_MEASURE_FULL_RANGE_LOW         (0xFFu)
#define `$INSTANCE_NAME`_MEASURE_CTRL_ENABLE            (0x01u)

#if (CY_PSOC5A)
    #define `$INSTANCE_NAME`_MEASURE_CTRL_MODE_SHIFT        (0x01u)
#else
    #define `$INSTANCE_NAME`_MEASURE_CTRL_MODE_SHIFT        (0x00u)
#endif /* (CY_PSOC5A) */

#define `$INSTANCE_NAME`_MEASURE_CTRL_PULSEWIDTH        (0x01u << `$INSTANCE_NAME`_MEASURE_CTRL_MODE_SHIFT)

/* UDB timers */
#define `$INSTANCE_NAME`_AUXCTRL_FIFO_SINGLE_REG        (0x03u)
 
/* Masks of PTR PC Register */
#define `$INSTANCE_NAME`_DR_MASK            (0x01u)
#define `$INSTANCE_NAME`_DM0_MASK           (0x02u)
#define `$INSTANCE_NAME`_DM1_MASK           (0x04u)
#define `$INSTANCE_NAME`_DM2_MASK           (0x08u)
#define `$INSTANCE_NAME`_BYP_MASK           (0x80u)

#define `$INSTANCE_NAME`_PRT_PC_GND         (`$INSTANCE_NAME`_DM2_MASK)
#define `$INSTANCE_NAME`_PRT_PC_HIGHZ       (`$INSTANCE_NAME`_DM2_MASK |`$INSTANCE_NAME`_DR_MASK)
#define `$INSTANCE_NAME`_PRT_PC_SHIELD      (`$INSTANCE_NAME`_DM2_MASK | `$INSTANCE_NAME`_DM1_MASK | \
                                             `$INSTANCE_NAME`_BYP_MASK)

/* CapSense Buffer definitions */
#define `$INSTANCE_NAME`_CSBUF_BOOST_ENABLE         (0x02u)
#define `$INSTANCE_NAME`_CSBUF_ENABLE               (0x01u)

/* Define direction of Current - Souce as Sink */
#if (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE)
    #define `$INSTANCE_NAME`_IdacCH0_IDIR      (`$INSTANCE_NAME`_IdacCH0_SOURCE)
    #define `$INSTANCE_NAME`_IdacCH1_IDIR      (`$INSTANCE_NAME`_IdacCH1_SOURCE)
#elif (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SINK)
    #define `$INSTANCE_NAME`_IdacCH0_IDIR      (`$INSTANCE_NAME`_IdacCH0_SINK)
    #define `$INSTANCE_NAME`_IdacCH1_IDIR      (`$INSTANCE_NAME`_IdacCH1_SINK)
#else
    /* No Idac - Rb selected */
#endif  /* (`$INSTANCE_NAME`_CURRENT_SOURCE == `$INSTANCE_NAME`_IDAC_SOURCE) */

#endif /* CY_CAPSENSE_CSD_`$INSTANCE_NAME`_H */


 /* [] END OF FILE */
