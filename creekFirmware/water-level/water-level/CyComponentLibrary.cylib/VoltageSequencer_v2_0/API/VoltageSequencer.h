/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants, parameter values and API function definitions
*  for the VoltageSequencer component.
*
* Note:
*
*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_VOLTAGE_SEQUENCER_`$INSTANCE_NAME`_H)
#define CY_VOLTAGE_SEQUENCER_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"


/*************************************** 
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS       (`$NumCtlInputs`u)
#define `$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS      (`$NumStsOutputs`u)
#define `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS       (`$NumConverters`u)
#define `$INSTANCE_NAME`_DISABLE_WARNINGS           (`$DisableWarn`u)

#if ((`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS > 0u) && !defined(`$INSTANCE_NAME`_VSeq_STS_GeneralStsReg__REMOVED))
    #define `$INSTANCE_NAME`_GENERATE_STATUS            (1u)
#endif /* (`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS > 0u) && !defined(`$INSTANCE_NAME`_VSeq_STS_GeneralStsReg__REMOVED) */

#if ((`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS > 0u) && !defined(`$INSTANCE_NAME`_VSeq_STS_GeneralStsReg__REMOVED))
    #define `$INSTANCE_NAME`_GENERATE_STATUS            (1u)
#endif /* (`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS > 0u) && !defined(`$INSTANCE_NAME`_VSeq_STS_GeneralStsReg__REMOVED) */


/***************************************
*     Data Struct Definitions
***************************************/


/*************************************** 
*        Function Prototypes
***************************************/

/* General APIs */
void   `$INSTANCE_NAME`_Start(void)  `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void   `$INSTANCE_NAME`_Stop (void)  `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void   `$INSTANCE_NAME`_Init(void)   `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void   `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;

/* Control input APIs */
#if(`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
    void   `$INSTANCE_NAME`_SetCtlPolarity(uint8 ctlNum, uint8 ctlPolarity) \
                                                         `=ReentrantKeil($INSTANCE_NAME . "_SetCtlPolarity")`;
    uint8  `$INSTANCE_NAME`_GetCtlPolarity(uint8 ctlNum) `=ReentrantKeil($INSTANCE_NAME . "_GetCtlPolarity")`;
    uint8  `$INSTANCE_NAME`_GetCtlStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetCtlStatus")`;
    void   `$INSTANCE_NAME`_SetCtlShutdownMask(uint8 converterNum, uint8 ctlPinMask) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetCtlShutdownMask")`;
    uint8  `$INSTANCE_NAME`_GetCtlShutdownMask(uint8 converterNum) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_GetCtlShutdownMask")`;
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */

/* Status output APIs */
#if(`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS > 0u)
    void   `$INSTANCE_NAME`_SetStsPgoodMask(uint8 stsNum, uint32 stsPgoodMask) \
                                                              `=ReentrantKeil($INSTANCE_NAME . "_SetStsPgoodMask")`;
    uint32 `$INSTANCE_NAME`_GetStsPgoodMask(uint8 stsNum)     `=ReentrantKeil($INSTANCE_NAME . "_GetStsPgoodMask")`;
    void   `$INSTANCE_NAME`_SetStsPgoodPolarity(uint8 stsNum, uint32 pgoodPolarity) \
                                                              `=ReentrantKeil($INSTANCE_NAME . "_SetStsPgoodPolarity")`;
    uint32 `$INSTANCE_NAME`_GetStsPgoodPolarity(uint8 stsNum) `=ReentrantKeil($INSTANCE_NAME . "_GetStsPgoodPolarity")`;
#endif /* (`$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS > 0u) */

/* Converter Power Up APIs */
void   `$INSTANCE_NAME`_SetPgoodOnThreshold(uint8 converterNum, uint16 onThreshold) \
                                                            `=ReentrantKeil($INSTANCE_NAME . "_SetPgoodOnThreshold")`;
uint16 `$INSTANCE_NAME`_GetPgoodOnThreshold(uint8 converterNum) \
                                                            `=ReentrantKeil($INSTANCE_NAME . "_GetPgoodOnThreshold")`;
void   `$INSTANCE_NAME`_SetEnPinPrereq(uint32 converterMask) `=ReentrantKeil($INSTANCE_NAME . "_SetEnPinPrereq")`;
uint32 `$INSTANCE_NAME`_GetEnPinPrereq(void) `=ReentrantKeil($INSTANCE_NAME . "_GetEnPinPrereq")`;
void   `$INSTANCE_NAME`_SetOnCmdPrereq(uint32 converterMask) `=ReentrantKeil($INSTANCE_NAME . "_SetOnCmdPrereq")`;
uint32 `$INSTANCE_NAME`_GetOnCmdPrereq(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOnCmdPrereq")`;
void   `$INSTANCE_NAME`_SetPgoodPrereq(uint8 converterNum, uint32 pgoodMask) \
                                                                `=ReentrantKeil($INSTANCE_NAME . "_SetPgoodPrereq")`;
uint32 `$INSTANCE_NAME`_GetPgoodPrereq(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetPgoodPrereq")`;
void   `$INSTANCE_NAME`_SetTonDelay(uint8 converterNum, uint16 tonDelay) \
                                                                    `=ReentrantKeil($INSTANCE_NAME . "_SetTonDelay")`;
uint16 `$INSTANCE_NAME`_GetTonDelay(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetTonDelay")`;
void   `$INSTANCE_NAME`_SetTonMax(uint8 converterNum, uint16 tonMax) `=ReentrantKeil($INSTANCE_NAME . "_SetTonMax")`;
uint16 `$INSTANCE_NAME`_GetTonMax(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetTonMax")`;
void   `$INSTANCE_NAME`_ForceOn(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_ForceOn")`;

/* Converter Power Down APIs */
void   `$INSTANCE_NAME`_SetPgoodOffThreshold(uint8 converterNum, uint16 offThreshold) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetPgoodOffThreshold")`;
uint16 `$INSTANCE_NAME`_GetPgoodOffThreshold(uint8 converterNum) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_GetPgoodOffThreshold")`;
void   `$INSTANCE_NAME`_SetPgoodShutdownMask(uint8 converterNum, uint32 pgoodMask) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetPgoodShutdownMask")`;
uint32 `$INSTANCE_NAME`_GetPgoodShutdownMask(uint8 converterNum) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_GetPgoodShutdownMask")`;
void   `$INSTANCE_NAME`_SetToffDelay(uint8 converterNum, uint16 toffDelay) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetToffDelay")`;
uint16 `$INSTANCE_NAME`_GetToffDelay(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetToffDelay")`;
void   `$INSTANCE_NAME`_SetToffMax(uint8 converterNum, uint16 toffMax) `=ReentrantKeil($INSTANCE_NAME . "_SetToffMax")`;
uint16 `$INSTANCE_NAME`_GetToffMax(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetToffMax")`;
void   `$INSTANCE_NAME`_ForceOff(uint8 converterNum, uint8 powerOffMode) `=ReentrantKeil($INSTANCE_NAME . "_ForceOff")`;

/* Converter Re-Sequence APIs */
void   `$INSTANCE_NAME`_SetTonMaxReseqCnt(uint8 converterNum, uint8 ReseqCnt) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetTonMaxReseqCnt")`;
uint8  `$INSTANCE_NAME`_GetTonMaxReseqCnt(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetTonMaxReseqCnt")`;
void   `$INSTANCE_NAME`_SetTonMaxFaultResp(uint8 converterNum, uint8 faultResponse) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetTonMaxFaultResp")`;
uint8  `$INSTANCE_NAME`_GetTonMaxFaultResp(uint8 converterNum) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_GetTonMaxFaultResp")`;
void   `$INSTANCE_NAME`_SetCtlReseqCnt(uint8 converterNum, uint8 reseqCnt) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetCtlReseqCnt")`;
uint8  `$INSTANCE_NAME`_GetCtlReseqCnt(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetCtlReseqCnt")`;
void   `$INSTANCE_NAME`_SetCtlFaultResp(uint8 converterNum, uint8 faultResponse) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetCtlFaultResp")`;
uint8  `$INSTANCE_NAME`_GetCtlFaultResp(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetCtlFaultResp")`;
void   `$INSTANCE_NAME`_SetFaultReseqSrc(uint8 converterNum, uint8 reseqSrc) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetFaultReseqSrc")`;
uint8  `$INSTANCE_NAME`_GetFaultReseqSrc(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultReseqSrc")`;
void   `$INSTANCE_NAME`_SetPgoodReseqCnt(uint8 converterNum, uint8 reseqCnt) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetPgoodReseqCnt")`;
uint8  `$INSTANCE_NAME`_GetPgoodReseqCnt(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetPgoodReseqCnt")`;
void   `$INSTANCE_NAME`_SetPgoodFaultResp(uint8 converterNum, uint8 faultResponse) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetPgoodFaultResp")`;
uint8 `$INSTANCE_NAME`_GetPgoodFaultResp(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetPgoodFaultResp")`;
void   `$INSTANCE_NAME`_SetOvReseqCnt(uint8 converterNum, uint8 reseqCnt) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetOvReseqCnt")`;
uint8  `$INSTANCE_NAME`_GetOvReseqCnt(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetOvReseqCnt")`;
void   `$INSTANCE_NAME`_SetOvFaultResp(uint8 converterNum, uint8 faultResponse) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetOvFaultResp")`;
uint8  `$INSTANCE_NAME`_GetOvFaultResp(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetOvFaultResp")`;
void   `$INSTANCE_NAME`_SetUvReseqCnt(uint8 converterNum, uint8 reseqCnt) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetUvReseqCnt")`;
uint8  `$INSTANCE_NAME`_GetUvReseqCnt(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetUvReseqCnt")`;
void   `$INSTANCE_NAME`_SetUvFaultResp(uint8 converterNum, uint8 faultResponse) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetUvFaultResp")`;
uint8  `$INSTANCE_NAME`_GetUvFaultResp(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetUvFaultResp")`;
void   `$INSTANCE_NAME`_SetOcReseqCnt(uint8 converterNum, uint8 reseqCnt) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetOcReseqCnt")`;
uint8  `$INSTANCE_NAME`_GetOcReseqCnt(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetOcReseqCnt")`;
void   `$INSTANCE_NAME`_SetOcFaultResp(uint8 converterNum, uint8 faultResponse) \
                                                        `=ReentrantKeil($INSTANCE_NAME . "_SetOcFaultResp")`;
uint8  `$INSTANCE_NAME`_GetOcFaultResp(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetOcFaultResp")`;

/* Power System APIs */
void   `$INSTANCE_NAME`_SetSysStableTime(uint16 stableTime) `=ReentrantKeil($INSTANCE_NAME . "_SetSysStableTime")`;
uint16 `$INSTANCE_NAME`_GetSysStableTime(void) `=ReentrantKeil($INSTANCE_NAME . "_GetSysStableTime")`;
void   `$INSTANCE_NAME`_SetReseqDelay(uint16 reseqDelay) `=ReentrantKeil($INSTANCE_NAME . "_SetReseqDelay")`;
uint16 `$INSTANCE_NAME`_GetReseqDelay(void) `=ReentrantKeil($INSTANCE_NAME . "_GetReseqDelay")`;
void   `$INSTANCE_NAME`_EnFaults(uint8 faultEnable) `=ReentrantKeil($INSTANCE_NAME . "_EnFaults")`;
void   `$INSTANCE_NAME`_SetFaultMask(uint32 faultMask) `=ReentrantKeil($INSTANCE_NAME . "_SetFaultMask")`;
uint32 `$INSTANCE_NAME`_GetFaultMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultMask")`;
uint32 `$INSTANCE_NAME`_GetFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetFaultStatus")`;
void   `$INSTANCE_NAME`_SetWarnMask(uint32 warnMask) `=ReentrantKeil($INSTANCE_NAME . "_SetWarnMask")`;
uint32 `$INSTANCE_NAME`_GetWarnMask(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnMask")`;
uint32 `$INSTANCE_NAME`_GetWarnStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetWarnStatus")`;
uint8  `$INSTANCE_NAME`_GetState(uint8 converterNum) `=ReentrantKeil($INSTANCE_NAME . "_GetState")`;
void   `$INSTANCE_NAME`_ForceAllOff(uint8 powerOffMode) `=ReentrantKeil($INSTANCE_NAME . "_ForceAllOff")`;
void   `$INSTANCE_NAME`_ForceAllOn(void) `=ReentrantKeil($INSTANCE_NAME . "_ForceAllOn")`;
#if (`$INSTANCE_NAME`_DISABLE_WARNINGS == 0u)
    void   `$INSTANCE_NAME`_EnWarnings(uint8 warnEnable) `=ReentrantKeil($INSTANCE_NAME . "_EnWarnings")`;
#endif /* `$INSTANCE_NAME`_DISABLE_WARNINGS == 0u */

CY_ISR_PROTO(`$INSTANCE_NAME`_SeqStateMachineIsr);
CY_ISR_PROTO(`$INSTANCE_NAME`_SysStableTimerIsr);
CY_ISR_PROTO(`$INSTANCE_NAME`_FaultHandlerIsr);


/***************************************
*           API Constants               
***************************************/

#define `$INSTANCE_NAME`_INFINITE_RESEQUENCING      (31u)

/* Interrupt numbers and priorities */
#define `$INSTANCE_NAME`_SEQUENCER_ISR_NUMBER       (`$INSTANCE_NAME`_SeqencerISR__INTC_NUMBER)
#define `$INSTANCE_NAME`_STABLE_TIMER_ISR_NUMBER    (`$INSTANCE_NAME`_StableTimerISR__INTC_NUMBER)
#define `$INSTANCE_NAME`_FAULT_ISR_NUMBER           (`$INSTANCE_NAME`_FaultISR__INTC_NUMBER)
#define `$INSTANCE_NAME`_SEQUENCER_ISR_PRIORITY     (`$INSTANCE_NAME`_SeqencerISR__INTC_PRIOR_NUM)
#define `$INSTANCE_NAME`_STABLE_TIMER_ISR_PRIORITY  (`$INSTANCE_NAME`_StableTimerISR__INTC_PRIOR_NUM)
#define `$INSTANCE_NAME`_FAULT_ISR_PRIORITY         (`$INSTANCE_NAME`_FaultISR__INTC_PRIOR_NUM)

#define `$INSTANCE_NAME`_CTL_MASK                   (0x3Fu >> (6u - `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS))
#define `$INSTANCE_NAME`_STS_MASK                   (0x3Fu >> (6u - `$INSTANCE_NAME`_NUMBER_OF_STS_OUTPUTS)) 
#define `$INSTANCE_NAME`_CONVERTER_MASK             (0xFFFFFFFFu >> (32u - `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS))
#define `$INSTANCE_NAME`_NUMBER_OF_GROUPS           (((`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS - 1u) >> 3) + 1u)

/* Sequencer state machine state definition */
#define `$INSTANCE_NAME`_OFF                        (0u)
#define `$INSTANCE_NAME`_PEND_ON                    (1u)
#define `$INSTANCE_NAME`_TON_DELAY                  (2u)
#define `$INSTANCE_NAME`_TON_MAX                    (3u)
#define `$INSTANCE_NAME`_ON                         (4u)
#define `$INSTANCE_NAME`_TOFF_DELAY                 (5u)
#define `$INSTANCE_NAME`_TOFF_MAX_WARN_LIMIT        (6u)
#define `$INSTANCE_NAME`_PEND_RESEQ                 (7u)
#define `$INSTANCE_NAME`_TRESEQ_DELAY               (8u)

#define `$INSTANCE_NAME`_ENABLED                    (1u)
#define `$INSTANCE_NAME`_DISABLED                   (0u)

#define `$INSTANCE_NAME`_SOFT_RESP_MODE             (0x20u)
#define `$INSTANCE_NAME`_IMMEDIATE_RESP_MODE        (0xDFu)
#define `$INSTANCE_NAME`_RESP_CNT_MASK              (0xE0u)
#define `$INSTANCE_NAME`_RESEQ_CNT                  (0x1Fu)
#define `$INSTANCE_NAME`_RESP_MODE_OFFSET           (0x05u)
#define `$INSTANCE_NAME`_SHUTDOWN_MODE              (0x20u)
#define `$INSTANCE_NAME`_IMMEDIATE_OFF              (0u)
#define `$INSTANCE_NAME`_SOFT_OFF                   (1u)
#define `$INSTANCE_NAME`_INIT_RESEQ_CNT             (0xFFu)
#define `$INSTANCE_NAME`_GROUP_SIZE                 (8u)
#if(CY_PSOC3)
    #define `$INSTANCE_NAME`_GROUP1                 (3u)
    #define `$INSTANCE_NAME`_GROUP2                 (2u)
    #define `$INSTANCE_NAME`_GROUP3                 (1u)
    #define `$INSTANCE_NAME`_GROUP4                 (0u)
#elif(CY_PSOC5)
    #define `$INSTANCE_NAME`_GROUP1                 (0u)
    #define `$INSTANCE_NAME`_GROUP2                 (1u)
    #define `$INSTANCE_NAME`_GROUP3                 (2u)
    #define `$INSTANCE_NAME`_GROUP4                 (3u)
#endif

#define `$INSTANCE_NAME`_PGOOD_FAULT_SRC            (0x00u)
#define `$INSTANCE_NAME`_OV_FAULT_SRC               (0x01u)
#define `$INSTANCE_NAME`_UV_FAULT_SRC               (0x02u)
#define `$INSTANCE_NAME`_OC_FAULT_SRC               (0x04u)
#define `$INSTANCE_NAME`_CTL_FAULT_SRC              (0x80u)
#define `$INSTANCE_NAME`_TON_MAX_FAULT_SRC          (0x10u)


/***************************************
*    Enumerated Types and Parameters    
***************************************/


/***************************************
*    Initial Parameter Constants        
***************************************/

#define `$INSTANCE_NAME`_INIT_CTL_POLARITY              (`$initCtlPolarity`)
#define `$INSTANCE_NAME`_INIT_STS_POLARITY              (`$initStsPolarity`)
#define `$INSTANCE_NAME`_INIT_EN_PIN_PREREQ_MASK        (`$initEnPinPrereqMask`)
#define `$INSTANCE_NAME`_INIT_ON_CMD_PREREQ_MASK        (`$initOnCmdPreReqMask`)
#define `$INSTANCE_NAME`_INIT_SYS_STABLE_TIME           (`$SysStableTime`)
#define `$INSTANCE_NAME`_INIT_GLOBAL_RESEQ_DELAY        (`$ReseqDelay`)
#define `$INSTANCE_NAME`_INIT_OV_FAULT_ENABLE           (`$OVFaultEn`)
#define `$INSTANCE_NAME`_INIT_UV_FAULT_ENABLE           (`$UVFaultEn`)
#define `$INSTANCE_NAME`_INIT_OC_FAULT_ENABLE           (`$OCFaultEn`)


/***************************************
*             Registers                 
***************************************/

/* Time tick generation control */
#define `$INSTANCE_NAME`_TICK_TIMER_AUX_CTL_REG     (* (reg8 *) `$INSTANCE_NAME`_VSeq_TickTimer__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_TICK_TIMER_AUX_CTL_PTR     (  (reg8 *) `$INSTANCE_NAME`_VSeq_TickTimer__CONTROL_AUX_CTL_REG)

/* Hardware monitoring of control and enable inputs */
#define `$INSTANCE_NAME`_CTL_LO_REG                 (* (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon1__STATUS_REG)
#define `$INSTANCE_NAME`_CTL_LO_PTR                 (  (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon1__STATUS_REG)
#define `$INSTANCE_NAME`_CTL_LO_MASK_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon1__MASK_REG)
#define `$INSTANCE_NAME`_CTL_LO_MASK_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon1__MASK_REG)
#define `$INSTANCE_NAME`_CTL_LO_AUX_REG             (* (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon1__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_CTL_LO_AUX_PTR             (  (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon1__STATUS_AUX_CTL_REG)

#if (`$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u)
    #define `$INSTANCE_NAME`_CTL_HI_REG             (* (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon2__STATUS_REG)
    #define `$INSTANCE_NAME`_CTL_HI_PTR             (  (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon2__STATUS_REG)
    #define `$INSTANCE_NAME`_CTL_HI_MASK_REG        (* (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon2__MASK_REG)
    #define `$INSTANCE_NAME`_CTL_HI_MASK_PTR        (  (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon2__MASK_REG)
    #define `$INSTANCE_NAME`_CTL_HI_AUX_REG         (* (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon2__STATUS_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_CTL_HI_AUX_PTR         (  (reg8 *) `$INSTANCE_NAME`_VSeq_Ctl_Mon2__STATUS_AUX_CTL_REG)
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CTL_INPUTS > 0u */

/* Status output control register */
#ifdef `$INSTANCE_NAME`_GENERATE_STATUS
    #define `$INSTANCE_NAME`_STS_OUT_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_STS_GeneralStsReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_STS_OUT_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_STS_GeneralStsReg__CONTROL_REG)
#endif /* `$INSTANCE_NAME`_GENERATE_STATUS */

/* Power system status register */
#define `$INSTANCE_NAME`_SYSTEM_STATUS_REG          (* (reg8 *) `$INSTANCE_NAME`_VSeq_SystemStsReg__CONTROL_REG)
#define `$INSTANCE_NAME`_SYSTEM_STATUS_PTR          (  (reg8 *) `$INSTANCE_NAME`_VSeq_SystemStsReg__CONTROL_REG)

/* Converter power good monitoring and enabling registers */
/* Converter 1 through 8 */
#define `$INSTANCE_NAME`_PGOOD_MON1_REG             (* (reg8 *) `$INSTANCE_NAME`_VSeq_PG1_PgoodReg__STATUS_REG)
#define `$INSTANCE_NAME`_PGOOD_MON1_PTR             (  (reg8 *) `$INSTANCE_NAME`_VSeq_PG1_PgoodReg__STATUS_REG)
#define `$INSTANCE_NAME`_EN_CTL1_REG                (* (reg8 *) `$INSTANCE_NAME`_VSeq_EN1_EnReg__CONTROL_REG)
#define `$INSTANCE_NAME`_EN_CTL1_PTR                (  (reg8 *) `$INSTANCE_NAME`_VSeq_EN1_EnReg__CONTROL_REG)
#define `$INSTANCE_NAME`_ON_CTL1_REG                (* (reg8 *) `$INSTANCE_NAME`_VSeq_ON1_OnReg__CONTROL_REG)
#define `$INSTANCE_NAME`_ON_CTL1_PTR                (  (reg8 *) `$INSTANCE_NAME`_VSeq_ON1_OnReg__CONTROL_REG)
/* Converter 9 through 16 */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u)
    #define `$INSTANCE_NAME`_PGOOD_MON2_REG         (* (reg8 *) `$INSTANCE_NAME`_VSeq_PG2_PgoodReg__STATUS_REG)
    #define `$INSTANCE_NAME`_PGOOD_MON2_PTR         (  (reg8 *) `$INSTANCE_NAME`_VSeq_PG2_PgoodReg__STATUS_REG)
    #define `$INSTANCE_NAME`_EN_CTL2_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_EN2_EnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_EN_CTL2_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_EN2_EnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_ON_CTL2_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_ON2_OnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_ON_CTL2_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_ON2_OnReg__CONTROL_REG)
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 8u */
/* Converter 17 through 24 */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 16u)
    #define `$INSTANCE_NAME`_PGOOD_MON3_REG         (* (reg8 *) `$INSTANCE_NAME`_VSeq_PG3_PgoodReg__STATUS_REG)
    #define `$INSTANCE_NAME`_PGOOD_MON3_PTR         (  (reg8 *) `$INSTANCE_NAME`_VSeq_PG3_PgoodReg__STATUS_REG) 
    #define `$INSTANCE_NAME`_EN_CTL3_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_EN3_EnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_EN_CTL3_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_EN3_EnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_ON_CTL3_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_ON3_OnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_ON_CTL3_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_ON3_OnReg__CONTROL_REG)
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 16u */    
/* Converter 25 through 32 */
#if (`$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 24u)
    #define `$INSTANCE_NAME`_PGOOD_MON4_REG         (* (reg8 *) `$INSTANCE_NAME`_VSeq_PG4_PgoodReg__STATUS_REG)
    #define `$INSTANCE_NAME`_PGOOD_MON4_PTR         (  (reg8 *) `$INSTANCE_NAME`_VSeq_PG4_PgoodReg__STATUS_REG)
    #define `$INSTANCE_NAME`_EN_CTL4_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_EN4_EnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_EN_CTL4_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_EN4_EnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_ON_CTL4_REG            (* (reg8 *) `$INSTANCE_NAME`_VSeq_ON4_OnReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_ON_CTL4_PTR            (  (reg8 *) `$INSTANCE_NAME`_VSeq_ON4_OnReg__CONTROL_REG)    
#endif /* `$INSTANCE_NAME`_NUMBER_OF_CONVERTERS > 24u */


/***************************************
*       Register Constants              
***************************************/

/* Tick timer enabling */
#define `$INSTANCE_NAME`_TICK_TIMER_EN          (0x20u)
/* Control input fault detection */
#define `$INSTANCE_NAME`_CTL_LO_FLT_DISBL       (0x00u)
#define `$INSTANCE_NAME`_CTL_HI_FLT_DISBL       (0x40u)
#define `$INSTANCE_NAME`_CTL_MON_EN             (0x10u)
/* Enable pin bit */
#define `$INSTANCE_NAME`_EN_PIN                 (0x40u)

/* System status bits */
#ifndef `$INSTANCE_NAME`_VSeq_SystemStsReg__REMOVED
    #define `$INSTANCE_NAME`_FAULT_MASK             (0x01u)
    #define `$INSTANCE_NAME`_WARN_MASK              (0x02u)
    #define `$INSTANCE_NAME`_SYS_STABLE_MASK        (0x04u)
    #define `$INSTANCE_NAME`_SYS_UP_MASK            (0x08u)
    #define `$INSTANCE_NAME`_SYS_DN_MASK            (0x10u)
#endif /* `$INSTANCE_NAME`_VSeq_SystemStsReg__REMOVED */

#endif /* CY_VOLTAGE_SEQUENCER_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
