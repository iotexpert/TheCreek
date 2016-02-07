/*******************************************************************************
* File Name: sysclk.h
* Version 1.0
*
* Description:
*  This file provides constants and parameter values for the sysclk
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

#if !defined(CY_TCPWM_sysclk_H)
#define CY_TCPWM_sysclk_H

#include "cytypes.h"
#include "cyfitter.h"


/*******************************************************************************
* Internal Type defines
*******************************************************************************/

/* Structure to save state before go to sleep */
typedef struct
{
    uint8  enableState;
} sysclk_BACKUP_STRUCT;


/*******************************************************************************
* Variables
*******************************************************************************/
extern uint8  sysclk_initVar;


/***************************************
*   Conditional Compilation Parameters
****************************************/

/* TCPWM Configuration */
#define sysclk_CONFIG                         (1lu)

/* Quad Mode */
/* Parameters */
#define sysclk_QUAD_ENCODING_MODES            (0lu)

/* Signal modes */
#define sysclk_QUAD_INDEX_SIGNAL_MODE         (0lu)
#define sysclk_QUAD_PHIA_SIGNAL_MODE          (3lu)
#define sysclk_QUAD_PHIB_SIGNAL_MODE          (3lu)
#define sysclk_QUAD_STOP_SIGNAL_MODE          (0lu)

/* Signal present */
#define sysclk_QUAD_INDEX_SIGNAL_PRESENT      (0lu)
#define sysclk_QUAD_STOP_SIGNAL_PRESENT       (0lu)

/* Interrupt Mask */
#define sysclk_QUAD_INTERRUPT_MASK            (1lu)

/* Timer/Counter Mode */
/* Parameters */
#define sysclk_TC_RUN_MODE                    (0lu)
#define sysclk_TC_COUNTER_MODE                (0lu)
#define sysclk_TC_COMP_CAP_MODE               (2lu)
#define sysclk_TC_PRESCALER                   (0lu)

/* Signal modes */
#define sysclk_TC_RELOAD_SIGNAL_MODE          (0lu)
#define sysclk_TC_COUNT_SIGNAL_MODE           (3lu)
#define sysclk_TC_START_SIGNAL_MODE           (0lu)
#define sysclk_TC_STOP_SIGNAL_MODE            (0lu)
#define sysclk_TC_CAPTURE_SIGNAL_MODE         (0lu)

/* Signal present */
#define sysclk_TC_RELOAD_SIGNAL_PRESENT       (0lu)
#define sysclk_TC_COUNT_SIGNAL_PRESENT        (1lu)
#define sysclk_TC_START_SIGNAL_PRESENT        (0lu)
#define sysclk_TC_STOP_SIGNAL_PRESENT         (0lu)
#define sysclk_TC_CAPTURE_SIGNAL_PRESENT      (0lu)

/* Interrupt Mask */
#define sysclk_TC_INTERRUPT_MASK              (0lu)

/* PWM Mode */
/* Parameters */
#define sysclk_PWM_KILL_EVENT                 (0lu)
#define sysclk_PWM_STOP_EVENT                 (0lu)
#define sysclk_PWM_MODE                       (4lu)
#define sysclk_PWM_OUT_N_INVERT               (0lu)
#define sysclk_PWM_OUT_INVERT                 (0lu)
#define sysclk_PWM_ALIGN                      (0lu)
#define sysclk_PWM_RUN_MODE                   (0lu)
#define sysclk_PWM_DEAD_TIME_CYCLE            (0lu)
#define sysclk_PWM_PRESCALER                  (0lu)

/* Signal modes */
#define sysclk_PWM_RELOAD_SIGNAL_MODE         (0lu)
#define sysclk_PWM_COUNT_SIGNAL_MODE          (3lu)
#define sysclk_PWM_START_SIGNAL_MODE          (0lu)
#define sysclk_PWM_STOP_SIGNAL_MODE           (0lu)
#define sysclk_PWM_SWITCH_SIGNAL_MODE         (0lu)

/* Signal present */
#define sysclk_PWM_RELOAD_SIGNAL_PRESENT      (0lu)
#define sysclk_PWM_COUNT_SIGNAL_PRESENT       (0lu)
#define sysclk_PWM_START_SIGNAL_PRESENT       (0lu)
#define sysclk_PWM_STOP_SIGNAL_PRESENT        (0lu)
#define sysclk_PWM_SWITCH_SIGNAL_PRESENT      (0lu)

/* Interrupt Mask */
#define sysclk_PWM_INTERRUPT_MASK             (1lu)


/***************************************
*    Initial Parameter Constants
***************************************/

/* Timer/Counter Mode */
#define sysclk_TC_PERIOD_VALUE                (65535lu)
#define sysclk_TC_COMPARE_VALUE               (65535lu)
#define sysclk_TC_COMPARE_BUF_VALUE           (65535lu)
#define sysclk_TC_COMPARE_SWAP                (0lu)

/* PWM Mode */
#define sysclk_PWM_PERIOD_VALUE               (65535lu)
#define sysclk_PWM_PERIOD_BUF_VALUE           (65535lu)
#define sysclk_PWM_PERIOD_SWAP                (0lu)
#define sysclk_PWM_COMPARE_VALUE              (65535lu)
#define sysclk_PWM_COMPARE_BUF_VALUE          (65535lu)
#define sysclk_PWM_COMPARE_SWAP               (0lu)


/***************************************
*    Enumerated Types and Parameters
***************************************/

#define sysclk__LEFT 0
#define sysclk__RIGHT 1
#define sysclk__CENTER 2
#define sysclk__ASYMMETRIC 3

#define sysclk__X1 0
#define sysclk__X2 1
#define sysclk__X4 2

#define sysclk__PWM 4
#define sysclk__PWM_DT 5
#define sysclk__PWM_PR 6

#define sysclk__INVERSE 1
#define sysclk__DIRECT 0

#define sysclk__CAPTURE 2
#define sysclk__COMPARE 0

#define sysclk__TRIG_LEVEL 3
#define sysclk__TRIG_RISING 0
#define sysclk__TRIG_FALLING 1
#define sysclk__TRIG_BOTH 2

#define sysclk__INTR_MASK_TC 1
#define sysclk__INTR_MASK_CC_MATCH 2
#define sysclk__INTR_MASK_NONE 0
#define sysclk__INTR_MASK_TC_CC 3

#define sysclk__UNCONFIG 8
#define sysclk__TIMER 1
#define sysclk__QUAD 3
#define sysclk__PWM_SEL 7

#define sysclk__COUNT_UP 0
#define sysclk__COUNT_DOWN 1
#define sysclk__COUNT_UPDOWN0 2
#define sysclk__COUNT_UPDOWN1 3


/* Prescaler */
#define sysclk_PRESCALE_DIVBY1                ((uint32)(0u << sysclk_PRESCALER_SHIFT))
#define sysclk_PRESCALE_DIVBY2                ((uint32)(1u << sysclk_PRESCALER_SHIFT))
#define sysclk_PRESCALE_DIVBY4                ((uint32)(2u << sysclk_PRESCALER_SHIFT))
#define sysclk_PRESCALE_DIVBY8                ((uint32)(3u << sysclk_PRESCALER_SHIFT))
#define sysclk_PRESCALE_DIVBY16               ((uint32)(4u << sysclk_PRESCALER_SHIFT))
#define sysclk_PRESCALE_DIVBY32               ((uint32)(5u << sysclk_PRESCALER_SHIFT))
#define sysclk_PRESCALE_DIVBY64               ((uint32)(6u << sysclk_PRESCALER_SHIFT))
#define sysclk_PRESCALE_DIVBY128              ((uint32)(7u << sysclk_PRESCALER_SHIFT))

/* TCPWM set modes */
#define sysclk_MODE_TIMER_COMPARE             ((uint32)(sysclk__COMPARE         <<  \
                                                                  sysclk_MODE_SHIFT))
#define sysclk_MODE_TIMER_CAPTURE             ((uint32)(sysclk__CAPTURE         <<  \
                                                                  sysclk_MODE_SHIFT))
#define sysclk_MODE_QUAD                      ((uint32)(sysclk__QUAD            <<  \
                                                                  sysclk_MODE_SHIFT))
#define sysclk_MODE_PWM                       ((uint32)(sysclk__PWM             <<  \
                                                                  sysclk_MODE_SHIFT))
#define sysclk_MODE_PWM_DT                    ((uint32)(sysclk__PWM_DT          <<  \
                                                                  sysclk_MODE_SHIFT))
#define sysclk_MODE_PWM_PR                    ((uint32)(sysclk__PWM_PR          <<  \
                                                                  sysclk_MODE_SHIFT))

/* Quad Modes */
#define sysclk_MODE_X1                        ((uint32)(sysclk__X1              <<  \
                                                                  sysclk_QUAD_MODE_SHIFT))
#define sysclk_MODE_X2                        ((uint32)(sysclk__X2              <<  \
                                                                  sysclk_QUAD_MODE_SHIFT))
#define sysclk_MODE_X4                        ((uint32)(sysclk__X4              <<  \
                                                                  sysclk_QUAD_MODE_SHIFT))

/* Counter modes */
#define sysclk_COUNT_UP                       ((uint32)(sysclk__COUNT_UP        <<  \
                                                                  sysclk_UPDOWN_SHIFT))
#define sysclk_COUNT_DOWN                     ((uint32)(sysclk__COUNT_DOWN      <<  \
                                                                  sysclk_UPDOWN_SHIFT))
#define sysclk_COUNT_UPDOWN0                  ((uint32)(sysclk__COUNT_UPDOWN0   <<  \
                                                                  sysclk_UPDOWN_SHIFT))
#define sysclk_COUNT_UPDOWN1                  ((uint32)(sysclk__COUNT_UPDOWN1   <<  \
                                                                  sysclk_UPDOWN_SHIFT))

/* PWM output invert */
#define sysclk_INVERT_LINE                    ((uint32)(sysclk__INVERSE         <<  \
                                                                  sysclk_INV_OUT_SHIFT))
#define sysclk_INVERT_LINE_N                  ((uint32)(sysclk__INVERSE         <<  \
                                                                  sysclk_INV_COMPL_OUT_SHIFT))

/* Trigger modes */
#define sysclk_TRIG_RISING                    (sysclk__TRIG_RISING)
#define sysclk_TRIG_FALLING                   (sysclk__TRIG_FALLING)
#define sysclk_TRIG_BOTH                      (sysclk__TRIG_BOTH)
#define sysclk_TRIG_LEVEL                     (sysclk__TRIG_LEVEL)

/* Interrupt mask */
#define sysclk_INTR_MASK_TC                   (sysclk__INTR_MASK_TC)
#define sysclk_INTR_MASK_CC_MATCH             (sysclk__INTR_MASK_CC_MATCH)

/* PWM Output Controls */
#define sysclk_CC_MATCH_SET                   (0x00u)
#define sysclk_CC_MATCH_CLEAR                 (0x01u)
#define sysclk_CC_MATCH_INVERT                (0x02u)
#define sysclk_CC_MATCH_NO_CHANGE             (0x03u)
#define sysclk_OVERLOW_SET                    (0x00u)
#define sysclk_OVERLOW_CLEAR                  (0x04u)
#define sysclk_OVERLOW_INVERT                 (0x08u)
#define sysclk_OVERLOW_NO_CHANGE              (0x0Cu)
#define sysclk_UNDERFLOW_SET                  (0x00u)
#define sysclk_UNDERFLOW_CLEAR                (0x10u)
#define sysclk_UNDERFLOW_INVERT               (0x20u)
#define sysclk_UNDERFLOW_NO_CHANGE            (0x30u)

/* PWM Align */
#define sysclk_PWM_MODE_LEFT                  (sysclk_CC_MATCH_CLEAR        |   \
                                                         sysclk_OVERLOW_SET           |   \
                                                         sysclk_UNDERFLOW_NO_CHANGE)
#define sysclk_PWM_MODE_RIGHT                 (sysclk_CC_MATCH_SET          |   \
                                                         sysclk_OVERLOW_NO_CHANGE     |   \
                                                         sysclk_UNDERFLOW_CLEAR)
#define sysclk_PWM_MODE_CENTER                (sysclk_CC_MATCH_INVERT       |   \
                                                         sysclk_OVERLOW_NO_CHANGE     |   \
                                                         sysclk_UNDERFLOW_CLEAR)
#define sysclk_PWM_MODE_ASYM                  (sysclk_CC_MATCH_NO_CHANGE    |   \
                                                         sysclk_OVERLOW_SET           |   \
                                                         sysclk_UNDERFLOW_CLEAR )

/* Command operations without condition */
#define sysclk_CMD_CAPTURE                    (0u)
#define sysclk_CMD_RELOAD                     (8u)
#define sysclk_CMD_STOP                       (16u)
#define sysclk_CMD_START                      (24u)

/* Status */
#define sysclk_STATUS_DOWN                    (1u)
#define sysclk_STATUS_RUNNING                 (2u)


/***************************************
*        Function Prototypes
****************************************/

void   sysclk_Init(void);
void   sysclk_Enable(void);
void   sysclk_Start(void);
void   sysclk_Stop(void);

void   sysclk_SetMode(uint32 mode);
void   sysclk_SetCounterMode(uint32 counterMode);
void   sysclk_SetPWMMode(uint32 modeMask);
void   sysclk_SetQDMode(uint32 qdMode);

void   sysclk_SetPrescaler(uint32 prescaler);
void   sysclk_TriggerCommand(uint32 mask, uint32 command);
void   sysclk_SetOneShot(uint32 oneShotEnable);
uint32 sysclk_ReadStatus(void);

void   sysclk_SetPWMSyncKill(uint32 syncKillEnable);
void   sysclk_SetPWMStopOnKill(uint32 stopOnKillEnable);
void   sysclk_SetPWMDeadTime(uint32 deadTime);
void   sysclk_SetPWMInvert(uint32 mask);

void   sysclk_SetInterruptMode(uint32 interruptMask);
uint32 sysclk_GetInterruptSourceMasked(void);
uint32 sysclk_GetInterruptSource(void);
void   sysclk_ClearInterrupt(uint32 interruptMask);
void   sysclk_SetInterrupt(uint32 interruptMask);

void   sysclk_WriteCounter(uint32 count);
uint32 sysclk_ReadCounter(void);

uint32 sysclk_ReadCapture(void);
uint32 sysclk_ReadCaptureBuf(void);

void   sysclk_WritePeriod(uint32 period);
uint32 sysclk_ReadPeriod(void);
void   sysclk_WritePeriodBuf(uint32 periodBuf);
uint32 sysclk_ReadPeriodBuf(void);

void   sysclk_WriteCompare(uint32 compare);
uint32 sysclk_ReadCompare(void);
void   sysclk_WriteCompareBuf(uint32 compareBuf);
uint32 sysclk_ReadCompareBuf(void);

void   sysclk_SetPeriodSwap(uint32 swapEnable);
void   sysclk_SetCompareSwap(uint32 swapEnable);

void   sysclk_SetCaptureMode(uint32 triggerMode);
void   sysclk_SetReloadMode(uint32 triggerMode);
void   sysclk_SetStartMode(uint32 triggerMode);
void   sysclk_SetStopMode(uint32 triggerMode);
void   sysclk_SetCountMode(uint32 triggerMode);

void   sysclk_SaveConfig(void);
void   sysclk_RestoreConfig(void);
void   sysclk_Sleep(void);
void   sysclk_Wakeup(void);


/***************************************
*             Registers
***************************************/

#define sysclk_BLOCK_CONTROL_REG              (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__TCPWM_CTRL )
#define sysclk_BLOCK_CONTROL_PTR              ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__TCPWM_CTRL )
#define sysclk_COMMAND_REG                    (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__TCPWM_CMD )
#define sysclk_COMMAND_PTR                    ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__TCPWM_CMD )
#define sysclk_INTRRUPT_CAUSE_REG             (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__TCPWM_INTR_CAUSE )
#define sysclk_INTRRUPT_CAUSE_PTR             ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__TCPWM_INTR_CAUSE )
#define sysclk_CONTROL_REG                    (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__CTRL )
#define sysclk_CONTROL_PTR                    ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__CTRL )
#define sysclk_STATUS_REG                     (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__STATUS )
#define sysclk_STATUS_PTR                     ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__STATUS )
#define sysclk_COUNTER_REG                    (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__COUNTER )
#define sysclk_COUNTER_PTR                    ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__COUNTER )
#define sysclk_COMP_CAP_REG                   (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__CC )
#define sysclk_COMP_CAP_PTR                   ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__CC )
#define sysclk_COMP_CAP_BUF_REG               (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__CC_BUFF )
#define sysclk_COMP_CAP_BUF_PTR               ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__CC_BUFF )
#define sysclk_PERIOD_REG                     (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__PERIOD )
#define sysclk_PERIOD_PTR                     ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__PERIOD )
#define sysclk_PERIOD_BUF_REG                 (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__PERIOD_BUFF )
#define sysclk_PERIOD_BUF_PTR                 ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__PERIOD_BUFF )
#define sysclk_TRIG_CONTROL0_REG              (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__TR_CTRL0 )
#define sysclk_TRIG_CONTROL0_PTR              ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__TR_CTRL0 )
#define sysclk_TRIG_CONTROL1_REG              (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__TR_CTRL1 )
#define sysclk_TRIG_CONTROL1_PTR              ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__TR_CTRL1 )
#define sysclk_TRIG_CONTROL2_REG              (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__TR_CTRL2 )
#define sysclk_TRIG_CONTROL2_PTR              ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__TR_CTRL2 )
#define sysclk_INTERRUPT_REQ_REG              (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR )
#define sysclk_INTERRUPT_REQ_PTR              ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR )
#define sysclk_INTERRUPT_SET_REG              (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR_SET )
#define sysclk_INTERRUPT_SET_PTR              ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR_SET )
#define sysclk_INTERRUPT_MASK_REG             (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR_MASK )
#define sysclk_INTERRUPT_MASK_PTR             ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR_MASK )
#define sysclk_INTERRUPT_MASKED_REG           (*(reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR_MASKED )
#define sysclk_INTERRUPT_MASKED_PTR           ( (reg32 *) sysclk_cy_m0s8_tcpwm_1__INTR_MASKED )


/***************************************
*       Registers Constants
***************************************/

/* Mask */
#define sysclk_MASK                           ((uint32)sysclk_cy_m0s8_tcpwm_1__TCPWM_CTRL_MASK)

/* Shift constants for control register */
#define sysclk_RELOAD_CC_SHIFT                (0u)
#define sysclk_RELOAD_PERIOD_SHIFT            (1u)
#define sysclk_PWM_SYNC_KILL_SHIFT            (2u)
#define sysclk_PWM_STOP_KILL_SHIFT            (3u)
#define sysclk_PRESCALER_SHIFT                (8u)
#define sysclk_UPDOWN_SHIFT                   (16u)
#define sysclk_ONESHOT_SHIFT                  (18u)
#define sysclk_QUAD_MODE_SHIFT                (20u)
#define sysclk_INV_OUT_SHIFT                  (20u)
#define sysclk_INV_COMPL_OUT_SHIFT            (21u)
#define sysclk_MODE_SHIFT                     (24u)

/* Mask constants for control register */
#define sysclk_RELOAD_CC_MASK                 ((uint32)(sysclk_1BIT_MASK        <<  \
                                                                            sysclk_RELOAD_CC_SHIFT))
#define sysclk_RELOAD_PERIOD_MASK             ((uint32)(sysclk_1BIT_MASK        <<  \
                                                                            sysclk_RELOAD_PERIOD_SHIFT))
#define sysclk_PWM_SYNC_KILL_MASK             ((uint32)(sysclk_1BIT_MASK        <<  \
                                                                            sysclk_PWM_SYNC_KILL_SHIFT))
#define sysclk_PWM_STOP_KILL_MASK             ((uint32)(sysclk_1BIT_MASK        <<  \
                                                                            sysclk_PWM_STOP_KILL_SHIFT))
#define sysclk_PRESCALER_MASK                 ((uint32)(sysclk_8BIT_MASK        <<  \
                                                                            sysclk_PRESCALER_SHIFT))
#define sysclk_UPDOWN_MASK                    ((uint32)(sysclk_2BIT_MASK        <<  \
                                                                            sysclk_UPDOWN_SHIFT))
#define sysclk_ONESHOT_MASK                   ((uint32)(sysclk_1BIT_MASK        <<  \
                                                                            sysclk_ONESHOT_SHIFT))
#define sysclk_QUAD_MODE_MASK                 ((uint32)(sysclk_3BIT_MASK        <<  \
                                                                            sysclk_QUAD_MODE_SHIFT))
#define sysclk_INV_OUT_MASK                   ((uint32)(sysclk_2BIT_MASK        <<  \
                                                                            sysclk_INV_OUT_SHIFT))
#define sysclk_MODE_MASK                      ((uint32)(sysclk_3BIT_MASK        <<  \
                                                                            sysclk_MODE_SHIFT))

/* Shift constants for trigger control register 1 */
#define sysclk_CAPTURE_SHIFT                  (0u)
#define sysclk_COUNT_SHIFT                    (2u)
#define sysclk_RELOAD_SHIFT                   (4u)
#define sysclk_STOP_SHIFT                     (6u)
#define sysclk_START_SHIFT                    (8u)

/* Mask constants for trigger control register 1 */
#define sysclk_CAPTURE_MASK                   ((uint32)(sysclk_2BIT_MASK        <<  \
                                                                  sysclk_CAPTURE_SHIFT))
#define sysclk_COUNT_MASK                     ((uint32)(sysclk_2BIT_MASK        <<  \
                                                                  sysclk_COUNT_SHIFT))
#define sysclk_RELOAD_MASK                    ((uint32)(sysclk_2BIT_MASK        <<  \
                                                                  sysclk_RELOAD_SHIFT))
#define sysclk_STOP_MASK                      ((uint32)(sysclk_2BIT_MASK        <<  \
                                                                  sysclk_STOP_SHIFT))
#define sysclk_START_MASK                     ((uint32)(sysclk_2BIT_MASK        <<  \
                                                                  sysclk_START_SHIFT))

/* MASK */
#define sysclk_1BIT_MASK                      ((uint32)0x01u)
#define sysclk_2BIT_MASK                      ((uint32)0x03u)
#define sysclk_3BIT_MASK                      ((uint32)0x07u)
#define sysclk_6BIT_MASK                      ((uint32)0x3Fu)
#define sysclk_8BIT_MASK                      ((uint32)0xFFu)
#define sysclk_16BIT_MASK                     ((uint32)0xFFFFu)

/* Shift constant for status register */
#define sysclk_RUNNING_STATUS_SHIFT           (30u)


/***************************************
*    Initial Constants
***************************************/

#define sysclk_PWM_PR_INIT_VALUE              (1u)

#endif /* End CY_TCPWM_sysclk_H */

/* [] END OF FILE */
