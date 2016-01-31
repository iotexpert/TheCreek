/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Contains the function prototypes, constants and register definition
*  of the CAN Component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CAN_`$INSTANCE_NAME`_H)
#define CY_CAN_`$INSTANCE_NAME`_H

#include "cyfitter.h"
#include "CyLib.h"

/* Check to see if required defines such as CY_PSOC5LP are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5LP)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5LP) */


#define `$INSTANCE_NAME`_INT_ISR_DISABLE   (`$IntISRDisable`u)


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_ARB_LOST          (`$ArbLost`u)
#define `$INSTANCE_NAME`_OVERLOAD          (`$Overload`u)
#define `$INSTANCE_NAME`_BIT_ERR           (`$BitError`u)
#define `$INSTANCE_NAME`_STUFF_ERR         (`$StuffError`u)
#define `$INSTANCE_NAME`_ACK_ERR           (`$AckError`u)
#define `$INSTANCE_NAME`_FORM_ERR          (`$FormError`u)
#define `$INSTANCE_NAME`_CRC_ERR           (`$CrcError`u)
#define `$INSTANCE_NAME`_BUS_OFF           (`$BussOff`u)
#define `$INSTANCE_NAME`_RX_MSG_LOST       (`$RxMsgLost`u)
#define `$INSTANCE_NAME`_TX_MESSAGE        (`$TxMsg`u)
#define `$INSTANCE_NAME`_RX_MESSAGE        (`$RxMsg`u)
#define `$INSTANCE_NAME`_ARB_LOST_USE_HELPER    (`$ArbLostUseHelper`u)
#define `$INSTANCE_NAME`_OVERLOAD_USE_HELPER    (`$OverloadUseHelper`u)
#define `$INSTANCE_NAME`_BIT_ERR_USE_HELPER     (`$BitErrorUseHelper`u)
#define `$INSTANCE_NAME`_STUFF_ERR_USE_HELPER   (`$StuffErrorUseHelper`u)
#define `$INSTANCE_NAME`_ACK_ERR_USE_HELPER     (`$AckErrorUseHelper`u)
#define `$INSTANCE_NAME`_FORM_ERR_USE_HELPER    (`$FormErrorUseHelper`u)
#define `$INSTANCE_NAME`_CRC_ERR_USE_HELPER     (`$CrcErrorUseHelper`u)
#define `$INSTANCE_NAME`_BUS_OFF_USE_HELPER     (`$BussOffUseHelper`u)
#define `$INSTANCE_NAME`_RX_MSG_LOST_USE_HELPER (`$RxMsgLostUseHelper`u)
#define `$INSTANCE_NAME`_TX_MESSAGE_USE_HELPER  (`$TxMsgUseHelper`u)
#define `$INSTANCE_NAME`_RX_MESSAGE_USE_HELPER  (`$RxMsgUseHelper`u)
#define `$INSTANCE_NAME`_ADVANCED_INTERRUPT_CFG (`$AdvancedInterruptConfig`u)

/* TX/RX Function Enable */
`$APITxRxFunctionEnable`

/***************************************
*        Data Struct Definition
***************************************/

/* Stuct for DATA of BASIC CAN mailbox */
typedef struct _`$INSTANCE_NAME`_dataBytesMsg
{
    uint8 byte[8u];
} `$INSTANCE_NAME`_DATA_BYTES_MSG;

/* Stuct for DATA of CAN RX register */
typedef struct _`$INSTANCE_NAME`_dataBytes
{
    reg8 byte[8u];
} `$INSTANCE_NAME`_DATA_BYTES;

/* Stuct for 32-bit CAN register */
typedef struct _`$INSTANCE_NAME`_reg32
{
    reg8 byte[4u];
} `$INSTANCE_NAME`_REG_32;

/* Stuct for BASIC CAN mailbox to send messages */
typedef struct _`$INSTANCE_NAME`_txMsg
{
    uint32 id;
    uint8 rtr;
    uint8 ide;
    uint8 dlc;
    uint8 irq;
    `$INSTANCE_NAME`_DATA_BYTES_MSG *msg;
} `$INSTANCE_NAME`_TX_MSG;

/* Constant configutaion of CAN RX */
typedef struct _`$INSTANCE_NAME`_rxCfg
{
    uint8 rxmailbox;
    uint32 rxcmd;
    uint32 rxamr;
    uint32 rxacr;
} `$INSTANCE_NAME`_RX_CFG;

/* Constant configutaion of CAN TX */
typedef struct _`$INSTANCE_NAME`_txCfg
{
    uint8 txmailbox;
    uint32 txcmd;
    uint32 txid;
} `$INSTANCE_NAME`_TX_CFG;

/* CAN RX registers */
typedef struct _`$INSTANCE_NAME`_rxStruct
{
    `$INSTANCE_NAME`_REG_32 rxcmd;
    `$INSTANCE_NAME`_REG_32 rxid;
    `$INSTANCE_NAME`_DATA_BYTES rxdata;
    `$INSTANCE_NAME`_REG_32 rxamr;
    `$INSTANCE_NAME`_REG_32 rxacr;
    `$INSTANCE_NAME`_REG_32 rxamrd;
    `$INSTANCE_NAME`_REG_32 rxacrd;
} `$INSTANCE_NAME`_RX_STRUCT;

/* CAN TX registers */
typedef struct _`$INSTANCE_NAME`_txStruct
{
    `$INSTANCE_NAME`_REG_32 txcmd;
    `$INSTANCE_NAME`_REG_32 txid;
    `$INSTANCE_NAME`_DATA_BYTES txdata;
} `$INSTANCE_NAME`_TX_STRUCT;

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8  enableState;
	uint32 intSr;
    uint32 intEn;
    uint32 cmd;
    uint32 cfg;    
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

uint8 `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
uint8 `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
uint8 `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
uint8 `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
uint8 `$INSTANCE_NAME`_GlobalIntEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_GlobalIntEnable")`;
uint8 `$INSTANCE_NAME`_GlobalIntDisable(void) `=ReentrantKeil($INSTANCE_NAME . "_GlobalIntDisable")`;
uint8 `$INSTANCE_NAME`_SetPreScaler(uint16 bitrate) `=ReentrantKeil($INSTANCE_NAME . "_SetPreScaler")`;
uint8 `$INSTANCE_NAME`_SetArbiter(uint8 arbiter) `=ReentrantKeil($INSTANCE_NAME . "_SetArbiter")`;
uint8 `$INSTANCE_NAME`_SetTsegSample(uint8 cfgTseg1, uint8 cfgTseg2, uint8 sjw, uint8 sm)
                                                `=ReentrantKeil($INSTANCE_NAME . "_SetTsegSample")`;
uint8 `$INSTANCE_NAME`_SetRestartType(uint8 reset) `=ReentrantKeil($INSTANCE_NAME . "_SetRestartType")`;
uint8 `$INSTANCE_NAME`_SetEdgeMode(uint8 edge) `=ReentrantKeil($INSTANCE_NAME . "_SetEdgeMode")`;
uint8 `$INSTANCE_NAME`_SetOpMode(uint8 opMode) `=ReentrantKeil($INSTANCE_NAME . "_SetOpMode")`;
uint8 `$INSTANCE_NAME`_RXRegisterInit(reg32 *regAddr, uint32 config)
                                            `=ReentrantKeil($INSTANCE_NAME . "_RXRegisterInit")`;
uint8 `$INSTANCE_NAME`_SetIrqMask(uint16 mask) `=ReentrantKeil($INSTANCE_NAME . "_SetIrqMask")`;
uint8 `$INSTANCE_NAME`_GetTXErrorFlag(void) `=ReentrantKeil($INSTANCE_NAME . "_GetTXErrorFlag")`;
uint8 `$INSTANCE_NAME`_GetRXErrorFlag(void) `=ReentrantKeil($INSTANCE_NAME . "_GetRXErrorFlag")`;
uint8 `$INSTANCE_NAME`_GetTXErrorCount(void) `=ReentrantKeil($INSTANCE_NAME . "_GetTXErrorCount")`;
uint8 `$INSTANCE_NAME`_GetRXErrorCount(void) `=ReentrantKeil($INSTANCE_NAME . "_GetRXErrorCount")`;
uint8 `$INSTANCE_NAME`_GetErrorState(void) `=ReentrantKeil($INSTANCE_NAME . "_GetErrorState")`;
void  `$INSTANCE_NAME`_Sleep(void) `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void  `$INSTANCE_NAME`_Wakeup(void) `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void  `$INSTANCE_NAME`_SaveConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void  `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

#if (`$INSTANCE_NAME`_ARB_LOST)
    void `$INSTANCE_NAME`_ArbLostIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_ArbLostIsr")`;
#endif /* `$INSTANCE_NAME`_ARB_LOST */

#if (`$INSTANCE_NAME`_OVERLOAD)
    void `$INSTANCE_NAME`_OvrLdErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_OvrLdErrorIsr")`;
#endif /* `$INSTANCE_NAME`_OVERLOAD */

#if (`$INSTANCE_NAME`_BIT_ERR)
    void `$INSTANCE_NAME`_BitErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_BitErrorIsr")`;
#endif /* `$INSTANCE_NAME`_BIT_ERR */

#if (`$INSTANCE_NAME`_STUFF_ERR)
    void `$INSTANCE_NAME`_BitStuffErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_BitStuffErrorIsr")`;
#endif /* `$INSTANCE_NAME`_STUFF_ERR */

#if (`$INSTANCE_NAME`_ACK_ERR)
    void `$INSTANCE_NAME`_AckErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_AckErrorIsr")`;
#endif /* `$INSTANCE_NAME`_ACK_ERR */

#if (`$INSTANCE_NAME`_FORM_ERR)
    void `$INSTANCE_NAME`_MsgErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgErrorIsr")`;
#endif /* `$INSTANCE_NAME`_FORM_ERR */

#if (`$INSTANCE_NAME`_CRC_ERR)
    void `$INSTANCE_NAME`_CrcErrorIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_CrcErrorIsr")`;
#endif /* `$INSTANCE_NAME`_CRC_ERR */

#if (`$INSTANCE_NAME`_BUS_OFF)
    void `$INSTANCE_NAME`_BusOffIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_BusOffIsr")`;
#endif /* `$INSTANCE_NAME`_BUS_OFF */

#if (`$INSTANCE_NAME`_RX_MSG_LOST)
    void `$INSTANCE_NAME`_MsgLostIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgLostIsr")`;
#endif /* `$INSTANCE_NAME`_RX_MSG_LOST */

#if (`$INSTANCE_NAME`_TX_MESSAGE)
   void `$INSTANCE_NAME`_MsgTXIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgTXIsr")`;
#endif /* `$INSTANCE_NAME`_TX_MESSAGE */

#if (`$INSTANCE_NAME`_RX_MESSAGE)
    void `$INSTANCE_NAME`_MsgRXIsr(void) `=ReentrantKeil($INSTANCE_NAME . "_MsgRXIsr")`;
#endif /* `$INSTANCE_NAME`_RX_MESSAGE */

uint8 `$INSTANCE_NAME`_RxBufConfig(`$INSTANCE_NAME`_RX_CFG *rxConfig) `=ReentrantKeil($INSTANCE_NAME . "_RxBufConfig")`;
uint8 `$INSTANCE_NAME`_TxBufConfig(`$INSTANCE_NAME`_TX_CFG *txConfig) `=ReentrantKeil($INSTANCE_NAME . "_TxBufConfig")`;
uint8 `$INSTANCE_NAME`_SendMsg(`$INSTANCE_NAME`_TX_MSG *message) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg")`;
void  `$INSTANCE_NAME`_TxCancel(uint8 bufferId) `=ReentrantKeil($INSTANCE_NAME . "_TxCancel")`;
void  `$INSTANCE_NAME`_ReceiveMsg(uint8 rxMailbox) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg")`;

#if (`$INSTANCE_NAME`_TX0_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name0`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name0`")`;
#endif /* `$INSTANCE_NAME`_TX0_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_TX1_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name1`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name1`")`;
#endif /* `$INSTANCE_NAME`_TX1_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_TX2_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name2`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name2`")`;
#endif /* `$INSTANCE_NAME`_TX2_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_TX3_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name3`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name3`")`;
#endif /* `$INSTANCE_NAME`_TX3_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_TX4_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name4`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name4`")`;
#endif /* `$INSTANCE_NAME`_TX4_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_TX5_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name5`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name5`")`;
#endif /* `$INSTANCE_NAME`_TX5_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_TX6_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name6`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name6`")`;
#endif /* `$INSTANCE_NAME`_TX6_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_TX7_FUNC_ENABLE)
    uint8 `$INSTANCE_NAME`_SendMsg`$TX_name7`(void) `=ReentrantKeil($INSTANCE_NAME . "_SendMsg`$TX_name7`")`;
#endif /* `$INSTANCE_NAME`_TX7_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX0_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name0`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name0`")`;
#endif /* `$INSTANCE_NAME`_RX0_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX1_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name1`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name1`")`;
#endif /* `$INSTANCE_NAME`_RX1_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX2_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name2`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name2`")`;
#endif /* `$INSTANCE_NAME`_RX2_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX3_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name3`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name3`")`;
#endif /* `$INSTANCE_NAME`_RX3_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX4_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name4`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name4`")`;
#endif /* `$INSTANCE_NAME`_RX4_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX5_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name5`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name5`")`;
#endif /* `$INSTANCE_NAME`_RX5_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX6_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name6`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name6`")`;
#endif /* `$INSTANCE_NAME`_RX6_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX7_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name7`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name7`")`;
#endif /* `$INSTANCE_NAME`_RX7_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX8_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name8`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name8`")`;
#endif /* `$INSTANCE_NAME`_RX8_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX9_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name9`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name9`")`;
#endif /* `$INSTANCE_NAME`_RX9_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX10_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name10`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name10`")`;
#endif /* `$INSTANCE_NAME`_RX10_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX11_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name11`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name11`")`;
#endif /* `$INSTANCE_NAME`_RX11_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX12_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name12`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name12`")`;
#endif /* `$INSTANCE_NAME`_RX12_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX13_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name13`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name13`")`;
#endif /* `$INSTANCE_NAME`_RX13_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX14_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name14`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name14`")`;
#endif /* `$INSTANCE_NAME`_RX14_FUNC_ENABLE */

#if (`$INSTANCE_NAME`_RX15_FUNC_ENABLE)
    void `$INSTANCE_NAME`_ReceiveMsg`$RX_name15`(void) `=ReentrantKeil($INSTANCE_NAME . "_ReceiveMsg`$RX_name15`")`;
#endif /* `$INSTANCE_NAME`_RX15_FUNC_ENABLE */

#if(!`$INSTANCE_NAME`_INT_ISR_DISABLE)
    /* Interrupt handler */
    CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);
#endif /* End `$INSTANCE_NAME`_INT_ISR_DISABLE */


/***************************************
*           API Constants
***************************************/

#if(!`$INSTANCE_NAME`_INT_ISR_DISABLE)
    /* Number of the `$INSTANCE_NAME`_isr interrupt */
    #define `$INSTANCE_NAME`_ISR_NUMBER                 (`$INSTANCE_NAME``[isr]`_INTC_NUMBER)  
    /* Priority of the `$INSTANCE_NAME`_isr interrupt */
    #define `$INSTANCE_NAME`_ISR_PRIORITY               (`$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM)
#endif /* End `$INSTANCE_NAME`_INT_ISR_DISABLE */

/* One or more parameters to the function were invalid. */
#define `$INSTANCE_NAME`_FAIL                       (0x01u)
#define `$INSTANCE_NAME`_OUT_OF_RANGE               (0x02u)

/* PM_ACT_CFG (Active Power Mode CFG Register) */
#define `$INSTANCE_NAME`_ACT_PWR_EN                 (`$INSTANCE_NAME``[CanIP]`_PM_ACT_MSK)    /* Power enable mask */

/* PM_STBY_CFG (Alternate Active (Standby) Power Mode CFG Register) */
#define `$INSTANCE_NAME`_STBY_PWR_EN                (`$INSTANCE_NAME``[CanIP]`_PM_STBY_MSK)   /* Power enable mask */

/* Number of TX and RX mailboxes */
#define `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES     (8u)
#define `$INSTANCE_NAME`_NUMBER_OF_RX_MAILBOXES     (16u) 

/* Error status of CAN */
#define `$INSTANCE_NAME`_ERROR_ACTIVE               (0x00u)
#define `$INSTANCE_NAME`_ERROR_PASIVE               (0x01u)
#define `$INSTANCE_NAME`_ERROR_BUS_OFF              (0x10u)


/***************************************
*    Initial Parameter Constants
***************************************/

/* General */
#define `$INSTANCE_NAME`_BITRATE           (`$Bitrate`u)
#define `$INSTANCE_NAME`_CFG_REG_TSEG1     (`$Tseg1`u - 1u)
#define `$INSTANCE_NAME`_CFG_REG_TSEG2     (`$Tseg2`u - 1u)
#define `$INSTANCE_NAME`_CFG_REG_SJW       (`$Sjw`u - 1u)
#define `$INSTANCE_NAME`_SAMPLING_MODE     (`$Sm`u)

#define `$INSTANCE_NAME`_ARBITER           (`$Arbiter`u)
#define `$INSTANCE_NAME`_RESET_TYPE        (`$Reset`u)
#define `$INSTANCE_NAME`_SYNC_EDGE         (`$EdgeMode`u)

/* Interrupts */
#define `$INSTANCE_NAME`_INT_ENABLE        (`$IntEnable`u)
#define `$INSTANCE_NAME`_INIT_INTERRUPT_MASK ((`$INSTANCE_NAME`_INT_ENABLE) | \
                                              (`$INSTANCE_NAME`_ARB_LOST << `$INSTANCE_NAME`_ARBITRATION_LOST_SHIFT) | \
                                              (`$INSTANCE_NAME`_OVERLOAD << `$INSTANCE_NAME`_OVERLOAD_ERROR_SHIFT) | \
                                              (`$INSTANCE_NAME`_BIT_ERR  << `$INSTANCE_NAME`_BIT_ERROR_SHIFT) | \
                                              (`$INSTANCE_NAME`_STUFF_ERR << `$INSTANCE_NAME`_STUFF_ERROR_SHIFT) | \
                                              (`$INSTANCE_NAME`_ACK_ERR   << `$INSTANCE_NAME`_ACK_ERROR_SHIFT) | \
                                              (`$INSTANCE_NAME`_FORM_ERR  << `$INSTANCE_NAME`_FORM_ERROR_SHIFT) | \
                                              (`$INSTANCE_NAME`_CRC_ERR   << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + \
                                                                            `$INSTANCE_NAME`_CRC_ERROR_SHIFT)) | \
                                              (`$INSTANCE_NAME`_BUS_OFF   << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + \
                                                                            `$INSTANCE_NAME`_BUS_OFF_SHIFT)) | \
                                              (`$INSTANCE_NAME`_RX_MSG_LOST << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + \
                                                                                `$INSTANCE_NAME`_RX_MSG_LOST_SHIFT)) | \
                                              (`$INSTANCE_NAME`_TX_MESSAGE  << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + \
                                                                           `$INSTANCE_NAME`_TX_MESSAGE_SHIFT)) | \
                                              (`$INSTANCE_NAME`_RX_MESSAGE  << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + \
                                                                           `$INSTANCE_NAME`_RX_MESSAGE_SHIFT)))


/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_TX         ( (volatile `$INSTANCE_NAME`_TX_STRUCT XDATA *) `$INSTANCE_NAME``[CanIP]`_TX0_CMD )
#define `$INSTANCE_NAME`_RX         ( (volatile `$INSTANCE_NAME`_RX_STRUCT XDATA *) `$INSTANCE_NAME``[CanIP]`_RX0_CMD )
#define `$INSTANCE_NAME`_INT_SR_REG ( *(volatile `$INSTANCE_NAME`_REG_32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_SR )
#define `$INSTANCE_NAME`_INT_SR_PTR (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_SR )
#define `$INSTANCE_NAME`_INT_EN_REG ( *(volatile `$INSTANCE_NAME`_REG_32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_EN )
#define `$INSTANCE_NAME`_INT_EN_PTR (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_EN )
#define `$INSTANCE_NAME`_BUF_SR_REG ( *(volatile `$INSTANCE_NAME`_REG_32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_BUF_SR )
#define `$INSTANCE_NAME`_BUF_SR_PTR (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_BUF_SR )
#define `$INSTANCE_NAME`_ERR_SR_REG ( *(volatile `$INSTANCE_NAME`_REG_32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_ERR_SR )
#define `$INSTANCE_NAME`_ERR_SR_PTR (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_ERR_SR )
#define `$INSTANCE_NAME`_CMD_REG    ( *(volatile `$INSTANCE_NAME`_REG_32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_CMD )
#define `$INSTANCE_NAME`_CMD_PTR    (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_CMD )
#define `$INSTANCE_NAME`_CFG_REG    ( *(volatile `$INSTANCE_NAME`_REG_32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_CFG )
#define `$INSTANCE_NAME`_CFG_PTR    (  (reg32 *) `$INSTANCE_NAME``[CanIP]`_CSR_CFG )
#define `$INSTANCE_NAME`_PM_ACT_CFG_REG  ( *(reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_ACT_CFG )    /* Power manager */
#define `$INSTANCE_NAME`_PM_ACT_CFG_PTR  (  (reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_ACT_CFG )    /* Power manager */
#define `$INSTANCE_NAME`_PM_STBY_CFG_REG ( *(reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_STBY_CFG )   /* Power manager */
#define `$INSTANCE_NAME`_PM_STBY_CFG_PTR (  (reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_STBY_CFG )   /* Power manager */

#define `$INSTANCE_NAME`_RX_FIRST_REGISTER_PTR    ( (reg32 *) `$INSTANCE_NAME``[CanIP]`_RX0_CMD)
#define `$INSTANCE_NAME`_RX_LAST_REGISTER_PTR     ( (reg32 *) `$INSTANCE_NAME``[CanIP]`_RX15_ACRD)


/***************************************
*        Register Constants
***************************************/

/* Operation mode */
#define `$INSTANCE_NAME`_ACTIVE_MODE              (0x00u)
#define `$INSTANCE_NAME`_LISTEN_ONLY              (0x01u)

/* Run/Stop mode */
#define `$INSTANCE_NAME`_MODE_STOP                (0x00u)
#define `$INSTANCE_NAME`_MODE_START               (0x01u)

/* Transmit buffer arbiter */
#define `$INSTANCE_NAME`_ROUND_ROBIN              (0x00u)
#define `$INSTANCE_NAME`_FIXED_PRIORITY           (0x01u)

/* Restart type */
#define `$INSTANCE_NAME`_RESTART_BY_HAND          (0x00u)
#define `$INSTANCE_NAME`_AUTO_RESTART             (0x01u)

/* Sampling mode */
#define `$INSTANCE_NAME`_ONE_SAMPLE_POINT         (0x00u)
#define `$INSTANCE_NAME`_THREE_SAMPLE_POINTS      (0x01u)

/* Edge mode */
#define `$INSTANCE_NAME`_EDGE_R_TO_D              (0x00u)
#define `$INSTANCE_NAME`_BOTH_EDGES               (0x01u)

/* Extended identifier */
#define `$INSTANCE_NAME`_STANDARD_MESSAGE         (0x00u)
#define `$INSTANCE_NAME`_EXTENDED_MESSAGE         (0x01u)

/* Write Protect Mask for Basic and Full mailboxes */
#define `$INSTANCE_NAME`_TX_WPN_SET               (((uint32)0x00000001u << `$INSTANCE_NAME`_TX_WPNL_SHIFT) | \
                                                   ((uint32)0x00000001u << (`$INSTANCE_NAME`_TWO_BYTE_OFFSET + \
                                                    `$INSTANCE_NAME`_TX_WPNH_SHIFT)))

#define `$INSTANCE_NAME`_TX_WPN_CLEAR             (~`$INSTANCE_NAME`_TX_WPN_SET)

#define `$INSTANCE_NAME`_RX_WPN_SET               (((uint32)0x00000001u << `$INSTANCE_NAME`_RX_WPNL_SHIFT) | \
                                                   ((uint32)0x00000001u << (`$INSTANCE_NAME`_TWO_BYTE_OFFSET + \
                                                    `$INSTANCE_NAME`_RX_WPNH_SHIFT)))

#define `$INSTANCE_NAME`_RX_WPN_CLEAR             (~`$INSTANCE_NAME`_RX_WPN_SET)

#define `$INSTANCE_NAME`_TX_RSVD_MASK             (0x00FF00FFu)

#define `$INSTANCE_NAME`_TX_READ_BACK_MASK        (`$INSTANCE_NAME`_TX_WPN_CLEAR & `$INSTANCE_NAME`_TX_RSVD_MASK)

#define `$INSTANCE_NAME`_RX_READ_BACK_MASK        (`$INSTANCE_NAME`_RX_WPN_CLEAR & `$INSTANCE_NAME`_TX_RSVD_MASK)

#define `$INSTANCE_NAME`_RX_CMD_REG_WIDTH         (0x20u)

/* TX send message */
#define `$INSTANCE_NAME`_TX_REQUEST_PENDING       (0x01u)
#define `$INSTANCE_NAME`_RETRY_NUMBER             (0x03u)
#define `$INSTANCE_NAME`_SEND_MESSAGE_SHIFT       (0u)
#define `$INSTANCE_NAME`_SEND_MESSAGE             ((uint32)0x00000001u << `$INSTANCE_NAME`_SEND_MESSAGE_SHIFT)

/* Offsets to maintain bytes within uint32 */
#define `$INSTANCE_NAME`_ONE_BYTE_OFFSET          (8u)
#define `$INSTANCE_NAME`_TWO_BYTE_OFFSET          (16u)
#define `$INSTANCE_NAME`_TREE_BYTE_OFFSET         (24u)

/* Set/Clear bits macro for `$INSTANCE_NAME`_RX maibox(i) */
/* bit 0 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_ACK_MSG_SHIFT         (0u)
#define `$INSTANCE_NAME`_RX_ACK_MSG               (0x01u << `$INSTANCE_NAME`_RX_ACK_MSG_SHIFT)
#define `$INSTANCE_NAME`_RX_ACK_MESSAGE(i)        `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] |= `$INSTANCE_NAME`_RX_ACK_MSG
/* bit 2 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_RTR_ABORT_SHIFT       (2u)
#define `$INSTANCE_NAME`_RX_RTR_ABORT_MASK        (0x01u << `$INSTANCE_NAME`_RX_RTR_ABORT_SHIFT)
#define `$INSTANCE_NAME`_RX_RTR_ABORT_MESSAGE(i)  `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] |= \
                                                    `$INSTANCE_NAME`_RX_RTR_ABORT_MASK
/* bit 3 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_BUF_ENABLE_SHIFT      (3u)
#define `$INSTANCE_NAME`_RX_BUF_ENABLE_MASK       (0x01u << `$INSTANCE_NAME`_RX_BUF_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_RX_BUF_ENABLE(i)         `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] |= \
                                                    `$INSTANCE_NAME`_RX_BUF_ENABLE_MASK
#define `$INSTANCE_NAME`_RX_BUF_DISABLE(i)        `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] &= \
                                                    ~`$INSTANCE_NAME`_RX_BUF_ENABLE_MASK
/* bit 4 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_RTRREPLY_SHIFT         (4u)
#define `$INSTANCE_NAME`_RX_RTRREPLY_MASK          (0x01u << `$INSTANCE_NAME`_RX_RTRREPLY_SHIFT)
#define `$INSTANCE_NAME`_SET_RX_RTRREPLY(i)        `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] |= \
                                                    `$INSTANCE_NAME`_RX_RTRREPLY_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_RTRREPLY(i)      `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] &= \
                                                    ~`$INSTANCE_NAME`_RX_RTRREPLY_MASK
/* bit 5 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_INT_ENABLE_SHIFT      (5u)
#define `$INSTANCE_NAME`_RX_INT_ENABLE_MASK       (0x01u << `$INSTANCE_NAME`_RX_INT_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_RX_INT_ENABLE(i)         `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] |= \
                                                    `$INSTANCE_NAME`_RX_INT_ENABLE_MASK
#define `$INSTANCE_NAME`_RX_INT_DISABLE(i)        `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] &= \
                                                    ~`$INSTANCE_NAME`_RX_INT_ENABLE_MASK
/* bit 6 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_LINKING_SHIFT         (6u)
#define `$INSTANCE_NAME`_RX_LINKING_MASK          (0x01u << `$INSTANCE_NAME`_RX_LINKING_SHIFT)
#define `$INSTANCE_NAME`_SET_RX_LINKING(i)        `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] |= \
                                                    `$INSTANCE_NAME`_RX_LINKING_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_LINKING(i)      `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] &= \
                                                    ~`$INSTANCE_NAME`_RX_LINKING_MASK
/* bit 7 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_WPNL_SHIFT           (7u)
#define `$INSTANCE_NAME`_RX_WPNL_MASK            (0x01u << `$INSTANCE_NAME`_RX_WPNL_SHIFT)
#define `$INSTANCE_NAME`_SET_RX_WNPL(i)          `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] |= `$INSTANCE_NAME`_RX_WPNL_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_WNPL(i)        `$INSTANCE_NAME`_RX[i].rxcmd.byte[0u] &= ~`$INSTANCE_NAME`_RX_WPNL_MASK

/* bit 23 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_WPNH_SHIFT           (7u)
#define `$INSTANCE_NAME`_RX_WPNH_MASK            (0x01u << `$INSTANCE_NAME`_RX_WPNH_SHIFT)
#define `$INSTANCE_NAME`_SET_RX_WNPH(i)          `$INSTANCE_NAME`_RX[i].rxcmd.byte[2u] |= `$INSTANCE_NAME`_RX_WPNH_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_WNPH(i)        `$INSTANCE_NAME`_RX[i].rxcmd.byte[2u] &= ~`$INSTANCE_NAME`_RX_WPNH_MASK

/* bits 19-16 within TX_CMD[i] */
#define `$INSTANCE_NAME`_RX_DLC_VALUE_SHIFT      (0u)
#define `$INSTANCE_NAME`_RX_DLC_VALUE_MASK       (0x0Fu << `$INSTANCE_NAME`_RX_DLC_VALUE_SHIFT)
#define `$INSTANCE_NAME`_GET_DLC(mailbox)        (`$INSTANCE_NAME`_RX[mailbox].rxcmd.byte[2u] & \
                                                  `$INSTANCE_NAME`_RX_DLC_VALUE_MASK)

/* Set/Clear bits macro for `$INSTANCE_NAME`_TX mailbox(i) */
/* bit 0 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST_SHIFT (0u)
#define `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST       (0x01u << `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST_SHIFT)
#define `$INSTANCE_NAME`_TX_TRANSMIT_MESSAGE(i)    `$INSTANCE_NAME`_TX[i].txcmd.byte[0u] |= \
                                                    `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST
/* bit 1 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_ABORT_SHIFT          (1u)
#define `$INSTANCE_NAME`_TX_ABORT_MASK           (0x01u << `$INSTANCE_NAME`_TX_ABORT_SHIFT)
#define `$INSTANCE_NAME`_TX_ABORT_MESSAGE(i)     `$INSTANCE_NAME`_TX[i].txcmd.byte[0u] |= `$INSTANCE_NAME`_TX_ABORT_MASK

/* bit 2 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TRANSMIT_INT_ENABLE     (0x01u)
#define `$INSTANCE_NAME`_TRANSMIT_INT_DISABLE    (0x00u)
#define `$INSTANCE_NAME`_TX_INT_ENABLE_SHIFT     (2u)
#define `$INSTANCE_NAME`_TX_INT_ENABLE_MASK      ((uint32)0x00000001u << `$INSTANCE_NAME`_TX_INT_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_TX_INT_ENABLE(i)        `$INSTANCE_NAME`_TX[i].txcmd.byte[0u] |= \
                                                  `$INSTANCE_NAME`_TX_INT_ENABLE_MASK
#define `$INSTANCE_NAME`_TX_INT_DISABLE(i)       `$INSTANCE_NAME`_TX[i].txcmd.byte[0u] &= \
                                                  ~`$INSTANCE_NAME`_TX_INT_ENABLE_MASK
/* bit 3 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_WPNL_SHIFT           (3u)
#define `$INSTANCE_NAME`_TX_WPNL_MASK            (0x01u << `$INSTANCE_NAME`_TX_WPNL_SHIFT)
#define `$INSTANCE_NAME`_SET_TX_WNPL(i)          `$INSTANCE_NAME`_TX[i].txcmd.byte[0u] |= `$INSTANCE_NAME`_TX_WPNL_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_WNPL(i)        `$INSTANCE_NAME`_TX[i].txcmd.byte[0u] &= ~`$INSTANCE_NAME`_TX_WPNL_MASK

/* bits 19-16 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_DLC_VALUE_SHIFT        (0u)
#define `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE_SHIFT  (19u)
#define `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE        ((uint32)0x00000001u << `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE_SHIFT)
#define `$INSTANCE_NAME`_TX_DLC_VALUE_MASK         (0x0Fu << `$INSTANCE_NAME`_TX_DLC_VALUE_SHIFT)
#define `$INSTANCE_NAME`_TX_DLC_MAX_VALUE          (8u)

/* bit 20 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_IDE_SHIFT             (20u)
#define `$INSTANCE_NAME`_TX_IDE_MASK              ((uint32)0x00000001u << `$INSTANCE_NAME`_TX_IDE_SHIFT)
#define `$INSTANCE_NAME`_SET_TX_IDE(i)            `$INSTANCE_NAME`_TX[i].txcmd.byte[2u] |= `$INSTANCE_NAME`_TX_IDE_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_IDE(i)          `$INSTANCE_NAME`_TX[i].txcmd.byte[2u] &= ~`$INSTANCE_NAME`_TX_IDE_MASK

/* bit 21 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_RTR_SHIFT             (21u)
#define `$INSTANCE_NAME`_TX_RTR_MASK              ((uint32)0x00000001u <<  `$INSTANCE_NAME`_TX_RTR_SHIFT)
#define `$INSTANCE_NAME`_SET_TX_RTR(i)            `$INSTANCE_NAME`_TX[i].txcmd.byte[2u] |= `$INSTANCE_NAME`_TX_RTR_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_RTR(i)          `$INSTANCE_NAME`_TX[i].txcmd.byte[2u] &= ~`$INSTANCE_NAME`_TX_RTR_MASK

/* bit 23 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_WPNH_SHIFT            (7u)
#define `$INSTANCE_NAME`_TX_WPNH_MASK             (0x01u << `$INSTANCE_NAME`_TX_WPNH_SHIFT)
#define `$INSTANCE_NAME`_SET_TX_WNPH(i)           `$INSTANCE_NAME`_TX[i].txcmd.byte[2u] |= `$INSTANCE_NAME`_TX_WPN_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_WNPH(i)         `$INSTANCE_NAME`_TX[i].txcmd.byte[2u] &= ~`$INSTANCE_NAME`_TX_WPN_MASK

/* bit 4 within RX_CMD[i].byte[2] */
#define `$INSTANCE_NAME`_RX_IDE_SHIFT             (4u)
#define `$INSTANCE_NAME`_RX_IDE_MASK              (0x01u << `$INSTANCE_NAME`_RX_IDE_SHIFT)
#define `$INSTANCE_NAME`_GET_RX_IDE(i)            (`$INSTANCE_NAME`_RX[i].rxcmd.byte[2u] & `$INSTANCE_NAME`_RX_IDE_MASK)
#define `$INSTANCE_NAME`_GET_RX_ID(i)             ((`$INSTANCE_NAME`_GET_RX_IDE(i)) ? ((CY_GET_REG32((reg32 *) & \
                                                    `$INSTANCE_NAME`_RX[i].rxid)) >> 3u) : ((CY_GET_REG32((reg32 *) & \
                                                    `$INSTANCE_NAME`_RX[i].rxid)) >> 21u))

#define `$INSTANCE_NAME`_RX_DATA_BYTE(mailbox, i) `$INSTANCE_NAME`_RX[mailbox].rxdata.byte[((i) > 3u) ? \
                                                                                         (7u - ((i) - 4u)) : (3u - (i))]
#define `$INSTANCE_NAME`_TX_DATA_BYTE(mailbox, i) `$INSTANCE_NAME`_TX[mailbox].txdata.byte[((i) > 3u) ? \
                                                                                         (7u - ((i) - 4u)) : (3u - (i))]

/* Macros for access to RX DATA for mailbox(i) */
#define `$INSTANCE_NAME`_RX_DATA_BYTE1(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[3u]
#define `$INSTANCE_NAME`_RX_DATA_BYTE2(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[2u]
#define `$INSTANCE_NAME`_RX_DATA_BYTE3(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[1u]
#define `$INSTANCE_NAME`_RX_DATA_BYTE4(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[0u]
#define `$INSTANCE_NAME`_RX_DATA_BYTE5(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[7u]
#define `$INSTANCE_NAME`_RX_DATA_BYTE6(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[6u]
#define `$INSTANCE_NAME`_RX_DATA_BYTE7(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[5u]
#define `$INSTANCE_NAME`_RX_DATA_BYTE8(i)          `$INSTANCE_NAME`_RX[i].rxdata.byte[4u]

/* Macros for access to TX DATA for mailbox(i) */
#define `$INSTANCE_NAME`_TX_DATA_BYTE1(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[3u]
#define `$INSTANCE_NAME`_TX_DATA_BYTE2(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[2u]
#define `$INSTANCE_NAME`_TX_DATA_BYTE3(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[1u]
#define `$INSTANCE_NAME`_TX_DATA_BYTE4(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[0u]
#define `$INSTANCE_NAME`_TX_DATA_BYTE5(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[7u]
#define `$INSTANCE_NAME`_TX_DATA_BYTE6(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[6u]
#define `$INSTANCE_NAME`_TX_DATA_BYTE7(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[5u]
#define `$INSTANCE_NAME`_TX_DATA_BYTE8(i)          `$INSTANCE_NAME`_TX[i].txdata.byte[4u]

/* Macros for set Tx Msg Indentifier in `$INSTANCE_NAME`_TX_ID register */
#define `$INSTANCE_NAME`_SET_TX_ID_STANDARD_MSG_SHIFT   (21u)
#define `$INSTANCE_NAME`_SET_TX_ID_EXTENDED_MSG_SHIFT   (3u)
#define `$INSTANCE_NAME`_SET_TX_ID_STANDARD_MSG(i, id)  CY_SET_REG32( (reg32 *) &`$INSTANCE_NAME`_TX[i].txid, (id << \
                                                        `$INSTANCE_NAME`_SET_TX_ID_STANDARD_MSG_SHIFT) );
#define `$INSTANCE_NAME`_SET_TX_ID_EXTENDED_MSG(i, id)  CY_SET_REG32( (reg32 *) &`$INSTANCE_NAME`_TX[i].txid, (id << \
                                                        `$INSTANCE_NAME`_SET_TX_ID_EXTENDED_MSG_SHIFT) );

/* Mask for bits within `$INSTANCE_NAME`_CSR_CFG */
#define `$INSTANCE_NAME`_EDGE_MODE_SHIFT           (0u)
/* bit 0 within CSR_CFG */
#define `$INSTANCE_NAME`_EDGE_MODE_MASK            (0x01u << `$INSTANCE_NAME`_EDGE_MODE_SHIFT)
#define `$INSTANCE_NAME`_SAMPLE_MODE_SHIFT         (1u)
/* bit 1 within CSR_CFG */
#define `$INSTANCE_NAME`_SAMPLE_MODE_MASK          (0x01u << `$INSTANCE_NAME`_SAMPLE_MODE_SHIFT)
#define `$INSTANCE_NAME`_CFG_REG_SJW_SHIFT         (2u)
/* bits 3-2 within CSR_CFG */
#define `$INSTANCE_NAME`_CFG_REG_SJW_MASK          (0x03u << `$INSTANCE_NAME`_CFG_REG_SJW_SHIFT)
#define `$INSTANCE_NAME`_CFG_REG_SJW_LOWER_LIMIT   (0x03u)  /* the lowest allowed value of cfg_sjw */
#define `$INSTANCE_NAME`_RESET_SHIFT               (4u)
/* bit 4 within CSR_CFG    */
#define `$INSTANCE_NAME`_RESET_MASK                (0x01u << `$INSTANCE_NAME`_RESET_SHIFT)
#define `$INSTANCE_NAME`_CFG_REG_TSEG2_SHIFT       (5u)
/* bits 7-5 within CSR_CFG */
#define `$INSTANCE_NAME`_CFG_REG_TSEG2_MASK        (0x07u << `$INSTANCE_NAME`_CFG_REG_TSEG2_SHIFT)
/* the highest allowed value of cfg_tseg2 */
#define `$INSTANCE_NAME`_CFG_REG_TSEG2_UPPER_LIMIT     (0x07u)
/* the lowest allowed value of cfg_tseg2 */
#define `$INSTANCE_NAME`_CFG_REG_TSEG2_LOWER_LIMIT     (0x02u)
 /* the lowest allowed value of cfg_tseg2 if sample point is one point */
#define `$INSTANCE_NAME`_CFG_REG_TSEG2_EXCEPTION       (0x01u)  
/* bits 11-8 within CSR_CFG */
#define `$INSTANCE_NAME`_CFG_REG_TSEG1_MASK            (0x0Fu)
/* the highest allowed value of cfg_tseg1 */
#define `$INSTANCE_NAME`_CFG_REG_TSEG1_UPPER_LIMIT     (0x0Fu)
/* the lowest allowed value of cfg_tseg1 */
#define `$INSTANCE_NAME`_CFG_REG_TSEG1_LOWER_LIMIT     (0x02u)
#define `$INSTANCE_NAME`_ARBITRATION_SHIFT             (4u)
/* bit 12 within CSR_CFG */
#define `$INSTANCE_NAME`_ARBITRATION_MASK              (0x01u << `$INSTANCE_NAME`_ARBITRATION_SHIFT)
/* bits 23-16 within CSR_CFG */
#define `$INSTANCE_NAME`_BITRATE_MASK                  (0x7FFFu)

/* Mask for bits within `$INSTANCE_NAME`_CSR_CMD */
#define `$INSTANCE_NAME`_MODE_SHIFT                    (0u)
/* bit 0 within CSR_CMD */
#define `$INSTANCE_NAME`_MODE_MASK                     (0x01u << `$INSTANCE_NAME`_MODE_SHIFT)
#define `$INSTANCE_NAME`_OPMODE_MASK_SHIFT             (1u)
/* bit 1 within CSR_CMD */
#define `$INSTANCE_NAME`_OPMODE_MASK                   (0x01u << `$INSTANCE_NAME`_OPMODE_MASK_SHIFT)

/* Mask for bits within `$INSTANCE_NAME`_CSR_CMD */
#define `$INSTANCE_NAME`_ERROR_STATE_SHIFT             (0u)
/* bit 17-16 within ERR_SR */
#define `$INSTANCE_NAME`_ERROR_STATE_MASK              (0x03u << `$INSTANCE_NAME`_ERROR_STATE_SHIFT)
#define `$INSTANCE_NAME`_TX_ERROR_FLAG_SHIFT           (2u)
/* bit 18 within ERR_SR */
#define `$INSTANCE_NAME`_TX_ERROR_FLAG_MASK            (0x01u << `$INSTANCE_NAME`_TX_ERROR_FLAG_SHIFT)
#define `$INSTANCE_NAME`_RX_ERROR_FLAG_SHIFT           (3u)
/* bit 19 within ERR_SR */
#define `$INSTANCE_NAME`_RX_ERROR_FLAG_MASK            (0x01u << `$INSTANCE_NAME`_RX_ERROR_FLAG_SHIFT)

/* Mask and Macros for bits within `$INSTANCE_NAME`_INT_EN_REG */
#define `$INSTANCE_NAME`_GLOBAL_INT_SHIFT              (0u)
/* bit 0 within INT_EN */
#define `$INSTANCE_NAME`_GLOBAL_INT_MASK               (0x01u << `$INSTANCE_NAME`_GLOBAL_INT_SHIFT)
#define `$INSTANCE_NAME`_ARBITRATION_LOST_SHIFT        (2u)
/* bit 2 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_ARBITRATION_LOST_MASK         (0x01u << `$INSTANCE_NAME`_ARBITRATION_LOST_SHIFT)
#define `$INSTANCE_NAME`_ARBITRATION_LOST_INT_ENABLE   (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] |= \
                                                        `$INSTANCE_NAME`_ARBITRATION_LOST_MASK)
#define `$INSTANCE_NAME`_ARBITRATION_LOST_INT_DISABLE  (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] &= \
                                                        ~`$INSTANCE_NAME`_ARBITRATION_LOST_MASK)
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_SHIFT          (3u)
/* bit 3 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK           (0x01u << `$INSTANCE_NAME`_OVERLOAD_ERROR_SHIFT)
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_INT_ENABLE     (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] |= \
                                                        `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK)
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_INT_DISABLE    (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] &= \
                                                        ~`$INSTANCE_NAME`_OVERLOAD_ERROR_MASK)
#define `$INSTANCE_NAME`_BIT_ERROR_SHIFT               (4u)
/* bit 4 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_BIT_ERROR_MASK                (0x01u << `$INSTANCE_NAME`_BIT_ERROR_SHIFT)
#define `$INSTANCE_NAME`_BIT_ERROR_LOST_INT_ENABLE     (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] |= \
                                                        `$INSTANCE_NAME`_BIT_ERROR_MASK)
#define `$INSTANCE_NAME`_BIT_ERROR_LOST_INT_DISABLE    (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] &= \
                                                        ~`$INSTANCE_NAME`_BIT_ERROR_MASK)
#define `$INSTANCE_NAME`_STUFF_ERROR_SHIFT             (5u)
/* bit 5 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_STUFF_ERROR_MASK              (0x01u << `$INSTANCE_NAME`_STUFF_ERROR_SHIFT)
#define `$INSTANCE_NAME`_STUFF_ERROR_INT_ENABLE        (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] |= \
                                                        `$INSTANCE_NAME`_STUFF_ERROR_MASK)
#define `$INSTANCE_NAME`_STUFF_ERROR_INT_DISABLE       (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] &= \
                                                        ~`$INSTANCE_NAME`_STUFF_ERROR_MASK)
#define `$INSTANCE_NAME`_ACK_ERROR_SHIFT               (6u)
/* bit 6 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_ACK_ERROR_MASK                (0x01u << `$INSTANCE_NAME`_ACK_ERROR_SHIFT)
#define `$INSTANCE_NAME`_ACK_ERROR_INT_ENABLE          (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] |= \
                                                        `$INSTANCE_NAME`_ACK_ERROR_MASK)
#define `$INSTANCE_NAME`_ACK_ERROR_INT_DISABLE         (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] &= \
                                                        ~`$INSTANCE_NAME`_ACK_ERROR_MASK)
#define `$INSTANCE_NAME`_FORM_ERROR_SHIFT              (7u)
/* bit 7 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_FORM_ERROR_MASK               (0x01u << `$INSTANCE_NAME`_FORM_ERROR_SHIFT)
#define `$INSTANCE_NAME`_FORM_ERROR_INT_ENABLE         (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] |= \
                                                        `$INSTANCE_NAME`_FORM_ERROR_MASK)
#define `$INSTANCE_NAME`_FORM_ERROR_INT_DISABLE        (`$INSTANCE_NAME`_INT_EN_REG.byte[0u] &= \
                                                        ~`$INSTANCE_NAME`_FORM_ERROR_MASK)
#define `$INSTANCE_NAME`_CRC_ERROR_SHIFT               (0u)
/* bit 8 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_CRC_ERROR_MASK                (0x01u << `$INSTANCE_NAME`_CRC_ERROR_SHIFT)
#define `$INSTANCE_NAME`_CRC_ERROR_INT_ENABLE          (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] |= \
                                                        `$INSTANCE_NAME`_CRC_ERROR_MASK)
#define `$INSTANCE_NAME`_CRC_ERROR_INT_DISABLE         (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] &= \
                                                        ~`$INSTANCE_NAME`_CRC_ERROR_MASK)
#define `$INSTANCE_NAME`_BUS_OFF_SHIFT                 (1u)
/* bit 9 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_BUS_OFF_MASK                  (0x01u << `$INSTANCE_NAME`_BUS_OFF_SHIFT)
#define `$INSTANCE_NAME`_BUS_OFF_INT_ENABLE            (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] |= \
                                                        `$INSTANCE_NAME`_BUS_OFF_MASK)
#define `$INSTANCE_NAME`_BUS_OFF_INT_DISABLE           (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] &= \
                                                        ~`$INSTANCE_NAME`_BUS_OFF_MASK)
#define `$INSTANCE_NAME`_RX_MSG_LOST_SHIFT             (2u)
/* bit 10 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_RX_MSG_LOST_MASK              (0x01u << `$INSTANCE_NAME`_RX_MSG_LOST_SHIFT)
#define `$INSTANCE_NAME`_RX_MSG_LOST_INT_ENABLE        (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] |= \
                                                        `$INSTANCE_NAME`_RX_MSG_LOST_MASK)
#define `$INSTANCE_NAME`_RX_MSG_LOST_INT_DISABLE       (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] &= \
                                                        ~`$INSTANCE_NAME`_RX_MSG_LOST_MASK)
#define `$INSTANCE_NAME`_TX_MESSAGE_SHIFT              (3u)
/* bit 11 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_TX_MESSAGE_MASK               (0x01u << `$INSTANCE_NAME`_TX_MESSAGE_SHIFT)
#define `$INSTANCE_NAME`_TX_MSG_INT_ENABLE             (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] |= \
                                                        `$INSTANCE_NAME`_TX_MESSAGE_MASK)
#define `$INSTANCE_NAME`_TX_MSG_INT_DISABLE            (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] &= \
                                                        ~`$INSTANCE_NAME`_TX_MESSAGE_MASK)
#define `$INSTANCE_NAME`_RX_MESSAGE_SHIFT              (4u)
/* bit 12 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_RX_MESSAGE_MASK               (0x01u << `$INSTANCE_NAME`_RX_MESSAGE_SHIFT)
#define `$INSTANCE_NAME`_RX_MSG_INT_ENABLE             (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] |= \
                                                        `$INSTANCE_NAME`_RX_MESSAGE_MASK)
#define `$INSTANCE_NAME`_RX_MSG_INT_DISABLE            (`$INSTANCE_NAME`_INT_EN_REG.byte[1u] &= \
                                                        ~`$INSTANCE_NAME`_RX_MESSAGE_MASK)

#endif /* CY_CAN_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
