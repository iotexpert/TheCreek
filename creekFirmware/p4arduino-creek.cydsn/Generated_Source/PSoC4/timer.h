/*******************************************************************************
* File Name: timer.h
* Version 1.0
*
* Description:
*  This file provides constants and parameter values for the timer
*  component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2013, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_TCPWM_timer_H)
#define CY_TCPWM_timer_H

#include "cytypes.h"
#include "cyfitter.h"


/*******************************************************************************
* Internal Type defines
*******************************************************************************/

/* Structure to save state before go to sleep */
typedef struct
{
    uint8  enableState;
} timer_BACKUP_STRUCT;


/*******************************************************************************
* Variables
*******************************************************************************/
extern uint8  timer_initVar;


/***************************************
*   Conditional Compilation Parameters
****************************************/

/* TCPWM Configuration */
#define timer_CONFIG                         (1lu)

/* Quad Mode */
/* Parameters */
#define timer_QUAD_ENCODING_MODES            (0lu)

/* Signal modes */
#define timer_QUAD_INDEX_SIGNAL_MODE         (0lu)
#define timer_QUAD_PHIA_SIGNAL_MODE          (3lu)
#define timer_QUAD_PHIB_SIGNAL_MODE          (3lu)
#define timer_QUAD_STOP_SIGNAL_MODE          (0lu)

/* Signal present */
#define timer_QUAD_INDEX_SIGNAL_PRESENT      (0lu)
#define timer_QUAD_STOP_SIGNAL_PRESENT       (0lu)

/* Interrupt Mask */
#define timer_QUAD_INTERRUPT_MASK            (1lu)

/* Timer/Counter Mode */
/* Parameters */
#define timer_TC_RUN_MODE                    (0lu)
#define timer_TC_COUNTER_MODE                (0lu)
#define timer_TC_COMP_CAP_MODE               (2lu)
#define timer_TC_PRESCALER                   (0lu)

/* Signal modes */
#define timer_TC_RELOAD_SIGNAL_MODE          (0lu)
#define timer_TC_COUNT_SIGNAL_MODE           (3lu)
#define timer_TC_START_SIGNAL_MODE           (0lu)
#define timer_TC_STOP_SIGNAL_MODE            (0lu)
#define timer_TC_CAPTURE_SIGNAL_MODE         (0lu)

/* Signal present */
#define timer_TC_RELOAD_SIGNAL_PRESENT       (0lu)
#define timer_TC_COUNT_SIGNAL_PRESENT        (0lu)
#define timer_TC_START_SIGNAL_PRESENT        (0lu)
#define timer_TC_STOP_SIGNAL_PRESENT         (0lu)
#define timer_TC_CAPTURE_SIGNAL_PRESENT      (0lu)

/* Interrupt Mask */
#define timer_TC_INTERRUPT_MASK              (2lu)

/* PWM Mode */
/* Parameters */
#define timer_PWM_KILL_EVENT                 (0lu)
#define timer_PWM_STOP_EVENT                 (0lu)
#define timer_PWM_MODE                       (4lu)
#define timer_PWM_OUT_N_INVERT               (0lu)
#define timer_PWM_OUT_INVERT                 (0lu)
#define timer_PWM_ALIGN                      (0lu)
#define timer_PWM_RUN_MODE                   (0lu)
#define timer_PWM_DEAD_TIME_CYCLE            (0lu)
#define timer_PWM_PRESCALER                  (0lu)

/* Signal modes */
#define timer_PWM_RELOAD_SIGNAL_MODE         (0lu)
#define timer_PWM_COUNT_SIGNAL_MODE          (3lu)
#define timer_PWM_START_SIGNAL_MODE          (0lu)
#define timer_PWM_STOP_SIGNAL_MODE           (0lu)
#define timer_PWM_SWITCH_SIGNAL_MODE         (0lu)

/* Signal present */
#define timer_PWM_RELOAD_SIGNAL_PRESENT      (0lu)
#define timer_PWM_COUNT_SIGNAL_PRESENT       (0lu)
#define timer_PWM_START_SIGNAL_PRESENT       (0lu)
#define timer_PWM_STOP_SIGNAL_PRESENT        (0lu)
#define timer_PWM_SWITCH_SIGNAL_PRESENT      (0lu)

/* Interrupt Mask */
#define timer_PWM_INTERRUPT_MASK             (1lu)


/***************************************
*    Initial Parameter Constants
***************************************/

/* Timer/Counter Mode */
#define timer_TC_PERIOD_VALUE                (65535lu)
#define timer_TC_COMPARE_VALUE               (65535lu)
#define timer_TC_COMPARE_BUF_VALUE           (65535lu)
#define timer_TC_COMPARE_SWAP                (0lu)

/* PWM Mode */
#define timer_PWM_PERIOD_VALUE               (65535lu)
#define timer_PWM_PERIOD_BUF_VALUE           (65535lu)
#define timer_PWM_PERIOD_SWAP                (0lu)
#define timer_PWM_COMPARE_VALUE              (65535lu)
#define timer_PWM_COMPARE_BUF_VALUE          (65535lu)
#define timer_PWM_COMPARE_SWAP               (0lu)


/***************************************
*    Enumerated Types and Parameters
***************************************/

#define timer__LEFT 0
#define timer__RIGHT 1
#define timer__CENTER 2
#define timer__ASYMMETRIC 3

#define timer__X1 0
#define timer__X2 1
#define timer__X4 2

#define timer__PWM 4
#define timer__PWM_DT 5
#define timer__PWM_PR 6

#define timer__INVERSE 1
#define timer__DIRECT 0

#define timer__CAPTURE 2
#define timer__COMPARE 0

#define timer__TRIG_LEVEL 3
#define timer__TRIG_RISING 0
#define timer__TRIG_FALLING 1
#define timer__TRIG_BOTH 2

#define timer__INTR_MASK_TC 1
#define timer__INTR_MASK_CC_MATCH 2
#define timer__INTR_MASK_NONE 0
#define timer__INTR_MASK_TC_CC 3

#define timer__UNCONFIG 8
#define timer__TIMER 1
#define timer__QUAD 3
#define timer__PWM_SEL 7

#define timer__COUNT_UP 0
#define timer__COUNT_DOWN 1
#define timer__COUNT_UPDOWN0 2
#define timer__COUNT_UPDOWN1 3


/* Prescaler */
#define timer_PRESCALE_DIVBY1                ((uint32)(0u << timer_PRESCALER_SHIFT))
#define timer_PRESCALE_DIVBY2                ((uint32)(1u << timer_PRESCALER_SHIFT))
#define timer_PRESCALE_DIVBY4                ((uint32)(2u << timer_PRESCALER_SHIFT))
#define timer_PRESCALE_DIVBY8                ((uint32)(3u << timer_PRESCALER_SHIFT))
#define timer_PRESCALE_DIVBY16               ((uint32)(4u << timer_PRESCALER_SHIFT))
#define timer_PRESCALE_DIVBY32               ((uint32)(5u << timer_PRESCALER_SHIFT))
#define timer_PRESCALE_DIVBY64               ((uint32)(6u << timer_PRESCALER_SHIFT))
#define timer_PRESCALE_DIVBY128              ((uint32)(7u << timer_PRESCALER_SHIFT))

/* TCPWM set modes */
#define timer_MODE_TIMER_COMPARE             ((uint32)(timer__COMPARE         <<  \
                                                                  timer_MODE_SHIFT))
#define timer_MODE_TIMER_CAPTURE             ((uint32)(timer__CAPTURE         <<  \
                                                                  timer_MODE_SHIFT))
#define timer_MODE_QUAD                      ((uint32)(timer__QUAD            <<  \
                                                                  timer_MODE_SHIFT))
#define timer_MODE_PWM                       ((uint32)(timer__PWM             <<  \
                                                                  timer_MODE_SHIFT))
#define timer_MODE_PWM_DT                    ((uint32)(timer__PWM_DT          <<  \
                                                                  timer_MODE_SHIFT))
#define timer_MODE_PWM_PR                    ((uint32)(timer__PWM_PR          <<  \
                                                                  timer_MODE_SHIFT))

/* Quad Modes */
#define timer_MODE_X1                        ((uint32)(timer__X1              <<  \
                                                                  timer_QUAD_MODE_SHIFT))
#define timer_MODE_X2                        ((uint32)(timer__X2              <<  \
                                                                  timer_QUAD_MODE_SHIFT))
#define timer_MODE_X4                        ((uint32)(timer__X4              <<  \
                                                                  timer_QUAD_MODE_SHIFT))

/* Counter modes */
#define timer_COUNT_UP                       ((uint32)(timer__COUNT_UP        <<  \
                                                                  timer_UPDOWN_SHIFT))
#define timer_COUNT_DOWN                     ((uint32)(timer__COUNT_DOWN      <<  \
                                                                  timer_UPDOWN_SHIFT))
#define timer_COUNT_UPDOWN0                  ((uint32)(timer__COUNT_UPDOWN0   <<  \
                                                                  timer_UPDOWN_SHIFT))
#define timer_COUNT_UPDOWN1                  ((uint32)(timer__COUNT_UPDOWN1   <<  \
                                                                  timer_UPDOWN_SHIFT))

/* PWM output invert */
#define timer_INVERT_LINE                    ((uint32)(timer__INVERSE         <<  \
                                                                  timer_INV_OUT_SHIFT))
#define timer_INVERT_LINE_N                  ((uint32)(timer__INVERSE         <<  \
                                                                  timer_INV_COMPL_OUT_SHIFT))

/* Trigger modes */
#define timer_TRIG_RISING                    (timer__TRIG_RISING)
#define timer_TRIG_FALLING                   (timer__TRIG_FALLING)
#define timer_TRIG_BOTH                      (timer__TRIG_BOTH)
#define timer_TRIG_LEVEL                     (timer__TRIG_LEVEL)

/* Interrupt mask */
#define timer_INTR_MASK_TC                   (timer__INTR_MASK_TC)
#define timer_INTR_MASK_CC_MATCH             (timer__INTR_MASK_CC_MATCH)

/* PWM Output Controls */
#define timer_CC_MATCH_SET                   (0x00u)
#define timer_CC_MATCH_CLEAR                 (0x01u)
#define timer_CC_MATCH_INVERT                (0x02u)
#define timer_CC_MATCH_NO_CHANGE             (0x03u)
#define timer_OVERLOW_SET                    (0x00u)
#define timer_OVERLOW_CLEAR                  (0x04u)
#define timer_OVERLOW_INVERT                 (0x08u)
#define timer_OVERLOW_NO_CHANGE              (0x0Cu)
#define timer_UNDERFLOW_SET                  (0x00u)
#define timer_UNDERFLOW_CLEAR                (0x10u)
#define timer_UNDERFLOW_INVERT               (0x20u)
#define timer_UNDERFLOW_NO_CHANGE            (0x30u)

/* PWM Align */
#define timer_PWM_MODE_LEFT                  (timer_CC_MATCH_CLEAR        |   \
                                                         timer_OVERLOW_SET           |   \
                                                         timer_UNDERFLOW_NO_CHANGE)
#define timer_PWM_MODE_RIGHT                 (timer_CC_MATCH_SET          |   \
                                                         timer_OVERLOW_NO_CHANGE     |   \
                                                         timer_UNDERFLOW_CLEAR)
#define timer_PWM_MODE_CENTER                (timer_CC_MATCH_INVERT       |   \
                                                         timer_OVERLOW_NO_CHANGE     |   \
                                                         timer_UNDERFLOW_CLEAR)
#define timer_PWM_MODE_ASYM                  (timer_CC_MATCH_NO_CHANGE    |   \
                                                         timer_OVERLOW_SET           |   \
                                                         timer_UNDERFLOW_CLEAR )

/* Command operations without condition */
#define timer_CMD_CAPTURE                    (0u)
#define timer_CMD_RELOAD                     (8u)
#define timer_CMD_STOP                       (16u)
#define timer_CMD_START                      (24u)

/* Status */
#define timer_STATUS_DOWN                    (1u)
#define timer_STATUS_RUNNING                 (2u)


/***************************************
*        Function Prototypes
****************************************/

void   timer_Init(void);
void   timer_Enable(void);
void   timer_Start(void);
void   timer_Stop(void);

void   timer_SetMode(uint32 mode);
void   timer_SetCounterMode(uint32 counterMode);
void   timer_SetPWMMode(uint32 modeMask);
void   timer_SetQDMode(uint32 qdMode);

void   timer_SetPrescaler(uint32 prescaler);
void   timer_TriggerCommand(uint32 mask, uint32 command);
void   timer_SetOneShot(uint32 oneShotEnable);
uint32 timer_ReadStatus(void);

void   timer_SetPWMSyncKill(uint32 syncKillEnable);
void   timer_SetPWMStopOnKill(uint32 stopOnKillEnable);
void   timer_SetPWMDeadTime(uint32 deadTime);
void   timer_SetPWMInvert(uint32 mask);

void   timer_SetInterruptMode(uint32 interruptMask);
uint32 timer_GetInterruptSourceMasked(void);
uint32 timer_GetInterruptSource(void);
void   timer_ClearInterrupt(uint32 interruptMask);
void   timer_SetInterrupt(uint32 interruptMask);

void   timer_WriteCounter(uint32 count);
uint32 timer_ReadCounter(void);

uint32 timer_ReadCapture(void);
uint32 timer_ReadCaptureBuf(void);

void   timer_WritePeriod(uint32 period);
uint32 timer_ReadPeriod(void);
void   timer_WritePeriodBuf(uint32 periodBuf);
uint32 timer_ReadPeriodBuf(void);

void   timer_WriteCompare(uint32 compare);
uint32 timer_ReadCompare(void);
void   timer_WriteCompareBuf(uint32 compareBuf);
uint32 timer_ReadCompareBuf(void);

void   timer_SetPeriodSwap(uint32 swapEnable);
void   timer_SetCompareSwap(uint32 swapEnable);

void   timer_SetCaptureMode(uint32 triggerMode);
void   timer_SetReloadMode(uint32 triggerMode);
void   timer_SetStartMode(uint32 triggerMode);
void   timer_SetStopMode(uint32 triggerMode);
void   timer_SetCountMode(uint32 triggerMode);

void   timer_SaveConfig(void);
void   timer_RestoreConfig(void);
void   timer_Sleep(void);
void   timer_Wakeup(void);


/***************************************
*             Registers
***************************************/

#define timer_BLOCK_CONTROL_REG              (*(reg32 *) timer_cy_m0s8_tcpwm_1__TCPWM_CTRL )
#define timer_BLOCK_CONTROL_PTR              ( (reg32 *) timer_cy_m0s8_tcpwm_1__TCPWM_CTRL )
#define timer_COMMAND_REG                    (*(reg32 *) timer_cy_m0s8_tcpwm_1__TCPWM_CMD )
#define timer_COMMAND_PTR                    ( (reg32 *) timer_cy_m0s8_tcpwm_1__TCPWM_CMD )
#define timer_INTRRUPT_CAUSE_REG             (*(reg32 *) timer_cy_m0s8_tcpwm_1__TCPWM_INTR_CAUSE )
#define timer_INTRRUPT_CAUSE_PTR             ( (reg32 *) timer_cy_m0s8_tcpwm_1__TCPWM_INTR_CAUSE )
#define timer_CONTROL_REG                    (*(reg32 *) timer_cy_m0s8_tcpwm_1__CTRL )
#define timer_CONTROL_PTR                    ( (reg32 *) timer_cy_m0s8_tcpwm_1__CTRL )
#define timer_STATUS_REG                     (*(reg32 *) timer_cy_m0s8_tcpwm_1__STATUS )
#define timer_STATUS_PTR                     ( (reg32 *) timer_cy_m0s8_tcpwm_1__STATUS )
#define timer_COUNTER_REG                    (*(reg32 *) timer_cy_m0s8_tcpwm_1__COUNTER )
#define timer_COUNTER_PTR                    ( (reg32 *) timer_cy_m0s8_tcpwm_1__COUNTER )
#define timer_COMP_CAP_REG                   (*(reg32 *) timer_cy_m0s8_tcpwm_1__CC )
#define timer_COMP_CAP_PTR                   ( (reg32 *) timer_cy_m0s8_tcpwm_1__CC )
#define timer_COMP_CAP_BUF_REG               (*(reg32 *) timer_cy_m0s8_tcpwm_1__CC_BUFF )
#define timer_COMP_CAP_BUF_PTR               ( (reg32 *) timer_cy_m0s8_tcpwm_1__CC_BUFF )
#define timer_PERIOD_REG                     (*(reg32 *) timer_cy_m0s8_tcpwm_1__PERIOD )
#define timer_PERIOD_PTR                     ( (reg32 *) timer_cy_m0s8_tcpwm_1__PERIOD )
#define timer_PERIOD_BUF_REG                 (*(reg32 *) timer_cy_m0s8_tcpwm_1__PERIOD_BUFF )
#define timer_PERIOD_BUF_PTR                 ( (reg32 *) timer_cy_m0s8_tcpwm_1__PERIOD_BUFF )
#define timer_TRIG_CONTROL0_REG              (*(reg32 *) timer_cy_m0s8_tcpwm_1__TR_CTRL0 )
#define timer_TRIG_CONTROL0_PTR              ( (reg32 *) timer_cy_m0s8_tcpwm_1__TR_CTRL0 )
#define timer_TRIG_CONTROL1_REG              (*(reg32 *) timer_cy_m0s8_tcpwm_1__TR_CTRL1 )
#define timer_TRIG_CONTROL1_PTR              ( (reg32 *) timer_cy_m0s8_tcpwm_1__TR_CTRL1 )
#define timer_TRIG_CONTROL2_REG              (*(reg32 *) timer_cy_m0s8_tcpwm_1__TR_CTRL2 )
#define timer_TRIG_CONTROL2_PTR              ( (reg32 *) timer_cy_m0s8_tcpwm_1__TR_CTRL2 )
#define timer_INTERRUPT_REQ_REG              (*(reg32 *) timer_cy_m0s8_tcpwm_1__INTR )
#define timer_INTERRUPT_REQ_PTR              ( (reg32 *) timer_cy_m0s8_tcpwm_1__INTR )
#define timer_INTERRUPT_SET_REG              (*(reg32 *) timer_cy_m0s8_tcpwm_1__INTR_SET )
#define timer_INTERRUPT_SET_PTR              ( (reg32 *) timer_cy_m0s8_tcpwm_1__INTR_SET )
#define timer_INTERRUPT_MASK_REG             (*(reg32 *) timer_cy_m0s8_tcpwm_1__INTR_MASK )
#define timer_INTERRUPT_MASK_PTR             ( (reg32 *) timer_cy_m0s8_tcpwm_1__INTR_MASK )
#define timer_INTERRUPT_MASKED_REG           (*(reg32 *) timer_cy_m0s8_tcpwm_1__INTR_MASKED )
#define timer_INTERRUPT_MASKED_PTR           ( (reg32 *) timer_cy_m0s8_tcpwm_1__INTR_MASKED )


/***************************************
*       Registers Constants
***************************************/

/* Mask */
#define timer_MASK                           ((uint32)timer_cy_m0s8_tcpwm_1__TCPWM_CTRL_MASK)

/* Shift constants for control register */
#define timer_RELOAD_CC_SHIFT                (0u)
#define timer_RELOAD_PERIOD_SHIFT            (1u)
#define timer_PWM_SYNC_KILL_SHIFT            (2u)
#define timer_PWM_STOP_KILL_SHIFT            (3u)
#define timer_PRESCALER_SHIFT                (8u)
#define timer_UPDOWN_SHIFT                   (16u)
#define timer_ONESHOT_SHIFT                  (18u)
#define timer_QUAD_MODE_SHIFT                (20u)
#define timer_INV_OUT_SHIFT                  (20u)
#define timer_INV_COMPL_OUT_SHIFT            (21u)
#define timer_MODE_SHIFT                     (24u)

/* Mask constants for control register */
#define timer_RELOAD_CC_MASK                 ((uint32)(timer_1BIT_MASK        <<  \
                                                                            timer_RELOAD_CC_SHIFT))
#define timer_RELOAD_PERIOD_MASK             ((uint32)(timer_1BIT_MASK        <<  \
                                                                            timer_RELOAD_PERIOD_SHIFT))
#define timer_PWM_SYNC_KILL_MASK             ((uint32)(timer_1BIT_MASK        <<  \
                                                                            timer_PWM_SYNC_KILL_SHIFT))
#define timer_PWM_STOP_KILL_MASK             ((uint32)(timer_1BIT_MASK        <<  \
                                                                            timer_PWM_STOP_KILL_SHIFT))
#define timer_PRESCALER_MASK                 ((uint32)(timer_8BIT_MASK        <<  \
                                                                            timer_PRESCALER_SHIFT))
#define timer_UPDOWN_MASK                    ((uint32)(timer_2BIT_MASK        <<  \
                                                                            timer_UPDOWN_SHIFT))
#define timer_ONESHOT_MASK                   ((uint32)(timer_1BIT_MASK        <<  \
                                                                            timer_ONESHOT_SHIFT))
#define timer_QUAD_MODE_MASK                 ((uint32)(timer_3BIT_MASK        <<  \
                                                                            timer_QUAD_MODE_SHIFT))
#define timer_INV_OUT_MASK                   ((uint32)(timer_2BIT_MASK        <<  \
                                                                            timer_INV_OUT_SHIFT))
#define timer_MODE_MASK                      ((uint32)(timer_3BIT_MASK        <<  \
                                                                            timer_MODE_SHIFT))

/* Shift constants for trigger control register 1 */
#define timer_CAPTURE_SHIFT                  (0u)
#define timer_COUNT_SHIFT                    (2u)
#define timer_RELOAD_SHIFT                   (4u)
#define timer_STOP_SHIFT                     (6u)
#define timer_START_SHIFT                    (8u)

/* Mask constants for trigger control register 1 */
#define timer_CAPTURE_MASK                   ((uint32)(timer_2BIT_MASK        <<  \
                                                                  timer_CAPTURE_SHIFT))
#define timer_COUNT_MASK                     ((uint32)(timer_2BIT_MASK        <<  \
                                                                  timer_COUNT_SHIFT))
#define timer_RELOAD_MASK                    ((uint32)(timer_2BIT_MASK        <<  \
                                                                  timer_RELOAD_SHIFT))
#define timer_STOP_MASK                      ((uint32)(timer_2BIT_MASK        <<  \
                                                                  timer_STOP_SHIFT))
#define timer_START_MASK                     ((uint32)(timer_2BIT_MASK        <<  \
                                                                  timer_START_SHIFT))

/* MASK */
#define timer_1BIT_MASK                      ((uint32)0x01u)
#define timer_2BIT_MASK                      ((uint32)0x03u)
#define timer_3BIT_MASK                      ((uint32)0x07u)
#define timer_6BIT_MASK                      ((uint32)0x3Fu)
#define timer_8BIT_MASK                      ((uint32)0xFFu)
#define timer_16BIT_MASK                     ((uint32)0xFFFFu)

/* Shift constant for status register */
#define timer_RUNNING_STATUS_SHIFT           (30u)


/***************************************
*    Initial Constants
***************************************/

#define timer_PWM_PR_INIT_VALUE              (1u)

#endif /* End CY_TCPWM_timer_H */

/* [] END OF FILE */
