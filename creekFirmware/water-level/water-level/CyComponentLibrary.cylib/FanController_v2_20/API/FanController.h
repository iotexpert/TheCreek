/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  API function prototypes, customizer parameters and other constants for the
*  Fan Controller component.
*
* Note:
*
*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "cytypes.h"
#include "cyfitter.h"
#include <CYDMAC.H>


#if !defined(`$INSTANCE_NAME`_CY_FAN_CONTROLLER_H)
#define `$INSTANCE_NAME`_CY_FAN_CONTROLLER_H


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_FAN_CTL_MODE               (`$FanMode`u)
#define `$INSTANCE_NAME`_NUMBER_OF_FANS             (`$NumberOfFans`u)
#define `$INSTANCE_NAME`_PWMRES                     (`$FanPWMRes`u)

#define `$INSTANCE_NAME`_FANCTLMODE_FIRMWARE        (0u)
#define `$INSTANCE_NAME`_FANCTLMODE_HARDWARE        (1u)

/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5LP) */


/***************************************
*        Function Prototypes
***************************************/

void    `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void    `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void    `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void    `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void    `$INSTANCE_NAME`_EnableAlert(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableAlert")`;
void    `$INSTANCE_NAME`_DisableAlert(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableAlert")`;
void    `$INSTANCE_NAME`_SetAlertMode(uint8 alertMode) `=ReentrantKeil($INSTANCE_NAME . "_SetAlertMode")`;
uint8   `$INSTANCE_NAME`_GetAlertMode(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAlertMode")`;
void    `$INSTANCE_NAME`_SetAlertMask(uint16 alertMask) `=ReentrantKeil($INSTANCE_NAME . "_SetAlertMask")`;
uint16  `$INSTANCE_NAME`_GetAlertMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAlertMask")`;
uint8   `$INSTANCE_NAME`_GetAlertSource(void) `=ReentrantKeil($INSTANCE_NAME . "_GetAlertSource")`;
uint16  `$INSTANCE_NAME`_GetFanStallStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFanStallStatus")`;
void    `$INSTANCE_NAME`_SetDutyCycle(uint8 fanOrBankNumber, uint16 dutyCycle)
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetDutyCycle")`;
uint16  `$INSTANCE_NAME`_GetDutyCycle(uint8 fanOrBankNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetDutyCycle")`;
void    `$INSTANCE_NAME`_SetDesiredSpeed(uint8 fanNumber, uint16 rpm)
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetDesiredSpeed")`;
uint16  `$INSTANCE_NAME`_GetActualSpeed(uint8 fanNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetActualSpeed")`;

#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    uint16  `$INSTANCE_NAME`_GetFanSpeedStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFanSpeedStatus")`;
    uint16  `$INSTANCE_NAME`_GetDesiredSpeed(uint8 fanNumber) `=ReentrantKeil($INSTANCE_NAME . "_GetDesiredSpeed")`;
    void    `$INSTANCE_NAME`_OverrideHardwareControl(uint8 override)
                                                        `=ReentrantKeil($INSTANCE_NAME . "_OverrideHardwareControl")`;
#endif /*  `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/***************************************
*           API Constants
***************************************/

/* Resolution constants */
#define `$INSTANCE_NAME`_PWMRES_EIGHTBIT            (0u)
#define `$INSTANCE_NAME`_PWMRES_TENBIT              (1u)

/* Constants for Acoustic Noise Reduction */
#define `$INSTANCE_NAME`_NOISE_REDUCTION_OFF        (0u)
#define `$INSTANCE_NAME`_NOISE_REDUCTION_ON         (1u)

/* Bit definitions for the Alert Source Status Register (`$INSTANCE_NAME`_GetAlertSource()) */
#define `$INSTANCE_NAME`_STALL_ALERT                (0x01u)
#define `$INSTANCE_NAME`_SPEED_ALERT                (0x02u)

/* End of Conversion constants */
#define `$INSTANCE_NAME`_EOC_HIGH                   (0x01u)
#define `$INSTANCE_NAME`_EOC_LOW                    (0x00u)

/* Fan Control Constants */
#define `$INSTANCE_NAME`_MAX_FANS                   (16u)

/* RPM to Duty Cycle Conversion Constants */
#define `$INSTANCE_NAME`_TACH_CLOCK_FREQ            (500000u)
#define `$INSTANCE_NAME`_RPM_PERIOD_CONV_FACTOR     (`$INSTANCE_NAME`_TACH_CLOCK_FREQ * 60u)

/* PWM Configuration Constants */
#if(`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT)
    #define `$INSTANCE_NAME`_PWM_PERIOD                 (240u)
#else
    #define `$INSTANCE_NAME`_PWM_PERIOD                 (960u)
#endif /*`$INSTANCE_NAME`_PWMRES == `$INSTANCE_NAME`_PWMRES_EIGHTBIT */

#define `$INSTANCE_NAME`_PWM_INIT_DUTY              (`$INSTANCE_NAME`_PWM_PERIOD)
#define `$INSTANCE_NAME`_PWM_DUTY_DIVIDER           (10000u)      /* API Duty Cycle is expressed in 100ths of % */

#if defined(__C51__)
    /* 8051 registers have 16-bit addresses */
    #define `$INSTANCE_NAME`_RegAddrType            uint16
    /* DMA TD endian swap flag - 8051 is big endian */
    #define `$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG    (TD_SWAP_EN)
#else
    /* ARM registers have 32-bit addresses */
    #define `$INSTANCE_NAME`_RegAddrType            uint32
    /* DMA TD endian swap flag - ARM is little endian */
    #define `$INSTANCE_NAME`_TD_SWAP_ENDIAN_FLAG    (0u)
#endif /* __C51__ */

#define `$INSTANCE_NAME`_TachOutDMA__TD_TERMOUT_EN ((`$INSTANCE_NAME`_TachOutDMA__TERMOUT0_EN ? TD_TERMOUT0_EN : 0u) | \
    (`$INSTANCE_NAME`_TachOutDMA__TERMOUT1_EN ? TD_TERMOUT1_EN : 0u))
#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    #define `$INSTANCE_NAME`_TachInDMA__TD_TERMOUT_EN ((`$INSTANCE_NAME`_TachInDMA__TERMOUT0_EN ? \
                                TD_TERMOUT0_EN : 0u) | (`$INSTANCE_NAME`_TachInDMA__TERMOUT1_EN ? TD_TERMOUT1_EN : 0u))
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

extern uint8 `$INSTANCE_NAME`_tachOutDMA_DmaHandle;
#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    extern uint8 `$INSTANCE_NAME`_tachInDMA_DmaHandle;
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/***************************************
*     Data Struct Definitions
***************************************/

/* Hardware and Firmware Fan Control Mode DMA transaction descriptors to store actual fan speeds in SRAM */
typedef struct _`$INSTANCE_NAME`_fanTdOutStruct
{
   uint8     setActualPeriodTD;                           /* Actual tach period from Tach block to SRAM */
} `$INSTANCE_NAME`_fanTdOutStruct;

/* Hardware Fan Control Mode DMA transaction descriptors to read desired speeds and tolerane from SRAM */
#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    typedef struct _`$INSTANCE_NAME`_fanTdInStruc
    {
        uint8     getDesiredPeriodTD;                       /* Desired tach period from SRAM to Tach block */
        uint8     getToleranceTD;                           /* Desired period tolerance from SRAM to Tach block */
    } `$INSTANCE_NAME`_fanTdInStruct;
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

/* DMA Controller SRAM Structure */
typedef struct _`$INSTANCE_NAME`_fanControlStruct
{
    #if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
        uint16  desiredPeriod[`$INSTANCE_NAME`_NUMBER_OF_FANS];
        uint16  tolerance[`$INSTANCE_NAME`_NUMBER_OF_FANS];
    #endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

    uint16  actualPeriod[`$INSTANCE_NAME`_NUMBER_OF_FANS];
} `$INSTANCE_NAME`_fanControlStruct;

extern `$INSTANCE_NAME`_fanControlStruct `$INSTANCE_NAME`_fanControl;

/* PWM configuration structure */
#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)
    typedef struct _`$INSTANCE_NAME`_fanDriverRegsStruct
    {
        `$INSTANCE_NAME`_RegAddrType  pwmPeriodReg;
        `$INSTANCE_NAME`_RegAddrType  pwmMaxDutyReg;
        `$INSTANCE_NAME`_RegAddrType  pwmSetDutyReg;
        `$INSTANCE_NAME`_RegAddrType  errorCountReg;
    } `$INSTANCE_NAME`_fanDriverRegsStruct;

#else

    typedef struct _`$INSTANCE_NAME`_fanDriverRegsStruct
    {
        `$INSTANCE_NAME`_RegAddrType  pwmSetDutyReg;
    } `$INSTANCE_NAME`_fanDriverRegsStruct;

    typedef struct _`$INSTANCE_NAME`_fanPwmInitRegsStruct
    {
        `$INSTANCE_NAME`_RegAddrType  pwmPeriodReg;
        `$INSTANCE_NAME`_RegAddrType  pwmAuxControlReg;
    } `$INSTANCE_NAME`_fanPwmInitRegsStruct;

    #define `$INSTANCE_NAME`_FANPWM_COUNT                   ((`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS / 2u) + \
                (`$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS % 2u))
    #define `$INSTANCE_NAME`_FANPWM_AUX_CTRL_FIFO0_CLR_8    (0x03u)
    #define `$INSTANCE_NAME`_FANPWM_AUX_CTRL_FIFO0_CLR_10   (0x0303u)

    extern `$INSTANCE_NAME`_fanPwmInitRegsStruct `$INSTANCE_NAME`_fanPwmInitRegs[];
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */

extern `$INSTANCE_NAME`_fanDriverRegsStruct `$INSTANCE_NAME`_fanDriverRegs[];

/* Fan Properties Structure. From parameters entered into the customizer */
typedef struct _`$INSTANCE_NAME`_fanPropertiesStruct
{
    uint16 rpmA;
    uint16 rpmB;
    uint16 dutyA;
    uint16 dutyB;
    uint16 dutyRpmSlope;
    uint16 initRpm;
    uint16 initDuty;
} `$INSTANCE_NAME`_fanPropertiesStruct;

extern `$INSTANCE_NAME`_fanPropertiesStruct `$INSTANCE_NAME`_fanProperties[];


/***************************************
* Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_NUMBER_OF_FAN_OUTPUTS       (`$NumberOfFanOutputs`u)
#define `$INSTANCE_NAME`_DAMPING_FACTOR              (`$DampingFactor`u)
#define `$INSTANCE_NAME`_INIT_ALERT_ENABLE           (`$AlertEnable`u)
#define `$INSTANCE_NAME`_NOISE_REDUCTION_MODE        (`$AcousticNoiseReduction`u)
#define `$INSTANCE_NAME`_INIT_ALERT_MASK             (((0x0001u << `$NumberOfFans`u) - 1u))
#define `$INSTANCE_NAME`_DAMPING_FACTOR_PERIOD_LOW   (5000u)
#define `$INSTANCE_NAME`_DAMPING_FACTOR_PERIOD_HIGH  `$INSTANCE_NAME`_DAMPING_FACTOR

/* Convert tolerance parameter to % */
#define `$INSTANCE_NAME`_TOLERANCE_FACTOR            ((`$FanTolerance` * 0.01))


/***************************************
*             Registers
***************************************/

/* Buried control/status registers */
/* PSoC5 ES1 */
#if(CY_PSOC5_ES1)
    /* Global Control Register */
    #define    `$INSTANCE_NAME`_GLOBAL_CONTROL_REG                \
                                        (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AsyncCtl_GlobalControlReg__CONTROL_REG)
    #define    `$INSTANCE_NAME`_GLOBAL_CONTROL_PTR                \
                                        (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AsyncCtl_GlobalControlReg__CONTROL_REG)

    #define    `$INSTANCE_NAME`_EOC_DMA_CONTROL_PTR                \
                                        (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AsyncCtl_DmaControlReg__CONTROL_REG)

    /* Alert Mask LSB Control register */
    #define `$INSTANCE_NAME`_ALERT_MASK_LSB_CONTROL_REG        \
                                        (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AsyncCtl_AlertMaskLSB__CONTROL_REG)
    #define `$INSTANCE_NAME`_ALERT_MASK_LSB_CONTROL_PTR        \
                                        (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AsyncCtl_AlertMaskLSB__CONTROL_REG)

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
        /* Alert Mask MSB Control register */
        #define `$INSTANCE_NAME`_ALERT_MASK_MSB_CONTROL_REG        \
                                (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AsyncCtl_CtrlAlertMSB_AlertMaskMSB__CONTROL_REG)
        #define `$INSTANCE_NAME`_ALERT_MASK_MSB_CONTROL_PTR        \
                                (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AsyncCtl_CtrlAlertMSB_AlertMaskMSB__CONTROL_REG)
    #endif /* (`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u) */

#else   /* PSoC3 ES3 and PSoC 5LP */
    #define    `$INSTANCE_NAME`_GLOBAL_CONTROL_REG                    \
                                        (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SyncCtl_GlobalControlReg__CONTROL_REG)
    #define    `$INSTANCE_NAME`_GLOBAL_CONTROL_PTR                    \
                                        (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SyncCtl_GlobalControlReg__CONTROL_REG)

    #define    `$INSTANCE_NAME`_EOC_DMA_CONTROL_PTR                \
                                        (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SyncCtl_DmaControlReg__CONTROL_REG)
                                        
    /* Alert Mask LSB Control register */
    #define `$INSTANCE_NAME`_ALERT_MASK_LSB_CONTROL_REG            \
                                            (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SyncCtl_AlertMaskLSB__CONTROL_REG)
    #define `$INSTANCE_NAME`_ALERT_MASK_LSB_CONTROL_PTR            \
                                            (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SyncCtl_AlertMaskLSB__CONTROL_REG)

    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
        /* Alert Mask MSB Control register */
        #define `$INSTANCE_NAME`_ALERT_MASK_MSB_CONTROL_REG        \
                               (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SyncCtl_CtrlAlertMSB_AlertMaskMSB__CONTROL_REG)
        #define `$INSTANCE_NAME`_ALERT_MASK_MSB_CONTROL_PTR        \
                               (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SyncCtl_CtrlAlertMSB_AlertMaskMSB__CONTROL_REG)
    #endif /* (`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u) */

#endif /* (CY_PSOC5_ES1) */


#define `$INSTANCE_NAME`_STALL_ERROR_LSB_STATUS_REG            \
                                    (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_StallError_LSB__STATUS_REG)
#define `$INSTANCE_NAME`_STALL_ERROR_LSB_STATUS_PTR            \
                                    (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_StallError_LSB__STATUS_REG)

#if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
    #define `$INSTANCE_NAME`_STALL_ERROR_MSB_STATUS_REG        \
                                    (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_StallStatusMSB_StallError_MSB__STATUS_REG)
    #define `$INSTANCE_NAME`_STALL_ERROR_MSB_STATUS_PTR        \
                                    (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_StallStatusMSB_StallError_MSB__STATUS_REG)
#endif /* (`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u) */

#define `$INSTANCE_NAME`_ALERT_STATUS_REG                      \
                                    (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AlertStatusReg__STATUS_REG)
#define `$INSTANCE_NAME`_ALERT_STATUS_PTR                      \
                                    (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AlertStatusReg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_ALERT_AUX_CTL_REG              \
                            (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AlertStatusReg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_STATUS_ALERT_AUX_CTL_PTR              \
                            (  (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_AlertStatusReg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_TACH_FAN_COUNTR_AUX_CTL_REG           \
                                    (* (reg8 *) `$INSTANCE_NAME`_FanTach_FanCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_TACH_FAN_COUNTR_AUX_CTL_PTR           \
                                    (  (reg8 *) `$INSTANCE_NAME`_FanTach_FanCounter__CONTROL_AUX_CTL_REG)

#define `$INSTANCE_NAME`_TACH_GLITCH_FILTER_AUX_CTL_REG        \
                                    (* (reg8 *) `$INSTANCE_NAME`_FanTach_GlitchFilterTimer__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_TACH_GLITCH_FILTER_AUX_CTL_PTR        \
                                    (  (reg8 *) `$INSTANCE_NAME`_FanTach_GlitchFilterTimer__CONTROL_AUX_CTL_REG)

/* Register defines inside the embedded tach component - these are sources/destinations for DMA controller */
#define `$INSTANCE_NAME`_TACH_TOLERANCE_PTR                  (`$INSTANCE_NAME`_FanTach_FanTachCounter_u0__16BIT_D1_REG)
#define `$INSTANCE_NAME`_TACH_DESIRED_PERIOD_PTR             (`$INSTANCE_NAME`_FanTach_FanTachCounter_u0__16BIT_D0_REG)
#define `$INSTANCE_NAME`_TACH_ACTUAL_PERIOD_PTR              (`$INSTANCE_NAME`_FanTach_FanTachCounter_u0__16BIT_A0_REG)

#if(`$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE)

    /* Damping Factor counter Registers */
    #define `$INSTANCE_NAME`_TACH_DAMPING_PERIOD_LOW_LSB_REG       \
                                    (*  (reg8 *) `$INSTANCE_NAME`_FanTach_DmpgFactor_DmpgTimeCntr_u0__D0_REG)
    #define `$INSTANCE_NAME`_TACH_DAMPING_PERIOD_LOW_LSB_PTR       \
                                    (   (reg8 *) `$INSTANCE_NAME`_FanTach_DmpgFactor_DmpgTimeCntr_u0__D0_REG)
    #define `$INSTANCE_NAME`_TACH_DAMPING_PERIOD_HIGH_LSB_REG      \
                                    (*  (reg8 *) `$INSTANCE_NAME`_FanTach_DmpgFactor_DmpgTimeCntr_u0__D1_REG)
    #define `$INSTANCE_NAME`_TACH_DAMPING_PERIOD_HIGH_LSB_PTR      \
                                    (   (reg8 *) `$INSTANCE_NAME`_FanTach_DmpgFactor_DmpgTimeCntr_u0__D1_REG)

    #define `$INSTANCE_NAME`_SPEED_ERROR_LSB_STATUS_REG           \
                                          (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SpeedAlrt_SpeedError_LSB__STATUS_REG )
    #if(`$INSTANCE_NAME`_NUMBER_OF_FANS > 8u)
        #define `$INSTANCE_NAME`_SPEED_ERROR_MSB_STATUS_REG           \
                                      (* (reg8 *) `$INSTANCE_NAME`_B_FanCtrl_SpeedAlrt_MSB_SpeedError_MSB__STATUS_REG )
    #endif /* `$INSTANCE_NAME`_NUMBER_OF_FANS > 8u */
#endif /* `$INSTANCE_NAME`_FAN_CTL_MODE == `$INSTANCE_NAME`_FANCTLMODE_HARDWARE */


/***************************************
*       Register Constants
***************************************/

#define `$INSTANCE_NAME`_COUNT7_ENABLE              (0x20u)
#define `$INSTANCE_NAME`_STATUS_ALERT_ENABLE        (0x10u)

/* Control Register bit defines */
#define `$INSTANCE_NAME`_ALERT_PIN_ENABLE_SHIFT     (0x00u)
#define `$INSTANCE_NAME`_ALERT_ENABLE_SHIFT         (0x01u)
#define `$INSTANCE_NAME`_STALL_ALERT_ENABLE_SHIFT   (0x01u)
#define `$INSTANCE_NAME`_SPEED_ALERT_ENABLE_SHIFT   (0x02u)
#define `$INSTANCE_NAME`_ALERT_CLEAR_SHIFT          (0x03u)
#define `$INSTANCE_NAME`_STALL_ALERT_CLEAR_SHIFT    (0x03u)
#define `$INSTANCE_NAME`_SPEED_ALERT_CLEAR_SHIFT    (0x04u)
#define `$INSTANCE_NAME`_ENABLE_SHIFT               (0x05u)
#define `$INSTANCE_NAME`_OVERRIDE_SHIFT             (0x06u)
#define `$INSTANCE_NAME`_ALERT_PIN_ENABLE           (0x01u << `$INSTANCE_NAME`_ALERT_PIN_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_ALERT_ENABLE_MASK          (0x03u << `$INSTANCE_NAME`_ALERT_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_STALL_ALERT_ENABLE         (0x01u << `$INSTANCE_NAME`_STALL_ALERT_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_SPEED_ALERT_ENABLE         (0x01u << `$INSTANCE_NAME`_SPEED_ALERT_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_ALERT_CLEAR_MASK           (0x03u << `$INSTANCE_NAME`_ALERT_CLEAR_SHIFT)
#define `$INSTANCE_NAME`_STALL_ALERT_CLEAR          (0x01u << `$INSTANCE_NAME`_STALL_ALERT_CLEAR_SHIFT)
#define `$INSTANCE_NAME`_SPEED_ALERT_CLEAR          (0x01u << `$INSTANCE_NAME`_SPEED_ALERT_CLEAR_SHIFT)
#define `$INSTANCE_NAME`_ENABLE                     (0x01u << `$INSTANCE_NAME`_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_OVERRIDE                   (0x01u << `$INSTANCE_NAME`_OVERRIDE_SHIFT)

#define `$INSTANCE_NAME`_ALERT_STATUS_MASK     (0x03u)

#endif /* `$INSTANCE_NAME`_CY_FAN_CONTROLLER_H */

/* [] END OF FILE */
