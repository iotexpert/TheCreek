/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     Contains the function prototypes, constants and register definition 
*     of the CAN Component.
*
*   Note:
*     None
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(CY_CAN_`$INSTANCE_NAME`_H)
#define CY_CAN_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice_trm.h" 
#include "`$INSTANCE_NAME`_isr.h"

/***************************************
 *  Parameters definition
 ***************************************/
/*-------------------- General ----------------------------*/
#define `$INSTANCE_NAME`_BITRATE		`$Bitrate`
#define `$INSTANCE_NAME`_CFG_TSEG1		(`$tseg1` - 1)
#define `$INSTANCE_NAME`_CFG_TSEG2		(`$tseg2` - 1)
#define `$INSTANCE_NAME`_CFG_SJW		(`$sjw` - 1)
#define `$INSTANCE_NAME`_SAMPLING_MODE	`$sm`

#define `$INSTANCE_NAME`_ARBITER		`$Arbiter`
#define `$INSTANCE_NAME`_RESET_TYPE		`$Reset`
#define `$INSTANCE_NAME`_SYNC_EDGE		`$Edge`

/*------------------- Interrupts -------------------------*/
#define `$INSTANCE_NAME`_INT_ENABLE		`$IntEnable`
#define `$INSTANCE_NAME`_ARB_LOST		`$Arb_lost`
#define `$INSTANCE_NAME`_OVERLOAD		`$Overload`
#define `$INSTANCE_NAME`_BIT_ERR		`$Bit_err`
#define `$INSTANCE_NAME`_STUFF_ERR		`$Stuff_err`
#define `$INSTANCE_NAME`_ACK_ERR		`$Ack_err`
#define `$INSTANCE_NAME`_FORM_ERR		`$Form_err`
#define `$INSTANCE_NAME`_CRC_ERR		`$Crc_err`
#define `$INSTANCE_NAME`_BUS_OFF		`$Buss_off`
#define `$INSTANCE_NAME`_RX_MSG_LOST	`$Rxmsg_lost`
#define `$INSTANCE_NAME`_TX_MSG			`$Tx_msg`
#define `$INSTANCE_NAME`_RX_MSG			`$Rx_msg`
#define `$INSTANCE_NAME`_INIT_INTERRUPT_MASK	  ( (`$INSTANCE_NAME`_INT_ENABLE)  |\
													(`$INSTANCE_NAME`_ARB_LOST << `$INSTANCE_NAME`_ARBITRATION_LOST_SHIFT) |\
													(`$INSTANCE_NAME`_OVERLOAD << `$INSTANCE_NAME`_OVERLOAD_ERROR_SHIFT) |\
													(`$INSTANCE_NAME`_BIT_ERR << `$INSTANCE_NAME`_BIT_ERROR_SHIFT) |\
													(`$INSTANCE_NAME`_STUFF_ERR << `$INSTANCE_NAME`_STUFF_ERROR_SHIFT) |\
													(`$INSTANCE_NAME`_ACK_ERR << `$INSTANCE_NAME`_ACK_ERROR_SHIFT) |\
													(`$INSTANCE_NAME`_FORM_ERR << `$INSTANCE_NAME`_FORM_ERROR_SHIFT) |\
													(`$INSTANCE_NAME`_CRC_ERR << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + `$INSTANCE_NAME`_CRC_ERROR_SHIFT)) |\
													(`$INSTANCE_NAME`_BUS_OFF << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + `$INSTANCE_NAME`_BUS_OFF_SHIFT)) |\
													(`$INSTANCE_NAME`_RX_MSG_LOST << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + `$INSTANCE_NAME`_RX_MSG_LOST_SHIFT)) |\
													(`$INSTANCE_NAME`_TX_MSG << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + `$INSTANCE_NAME`_TX_MSG_SHIFT)) |\
													(`$INSTANCE_NAME`_RX_MSG << (`$INSTANCE_NAME`_ONE_BYTE_OFFSET + `$INSTANCE_NAME`_RX_MSG_SHIFT)) )

/*--------------- TX/RX Function Enable  -----------------*/
`$APIRateGenerationDefineHFile`

/***************************************
 *  Types definition
 ***************************************/ 
typedef struct _DATA_BYTES_MSG
{
	uint8 byte[8];                
}	DATA_BYTES_MSG;

typedef struct _DATA_BYTES
{
	reg8 byte[8];                
}	DATA_BYTES;

typedef struct _CAN_reg32
{
	reg8 byte[4];
} CAN_reg32;

/* Stuct for BASIC CAN to send messages */
typedef struct _CANTXMsg
{
	uint32 id;
	uint8 rtr;
	uint8 ide;
	uint8 dlc;
	uint8 irq;
	DATA_BYTES_MSG *msg;
} CANTXMsg;
 
/* Constant configutaion of CAN RX */
typedef struct _CANRXcfg
{
	uint8 rxmailbox;
	uint32 rxcmd;
	uint32 rxamr;
	uint32 rxacr;
} CANRXcfg;

/* Constant configutaion of CAN TX */
typedef struct _CANTXcfg
{
	uint8 txmailbox;
	uint32 txcmd;
	uint32 txid;
} CANTXcfg;

/* CAN RX mapped registers in RAM */
typedef struct _CANRXstruct
{
	CAN_reg32 rxcmd;
	CAN_reg32 rxid;
	DATA_BYTES rxdata;
	CAN_reg32 rxamr;
	CAN_reg32 rxacr;
	CAN_reg32 rxamrd;
	CAN_reg32 rxacrd;
} CANRXstruct;

/* CAN TX mapped registers in RAM */
typedef struct _CANTXstruct
{
	CAN_reg32 txcmd;
	CAN_reg32 txid;
	DATA_BYTES txdata;
} CANTXstruct;

/***************************************
 *  Function Prototypes
 ***************************************/
uint8 `$INSTANCE_NAME`_Init(void);
uint8 `$INSTANCE_NAME`_Start(void);
uint8 `$INSTANCE_NAME`_Stop(void);
uint8 `$INSTANCE_NAME`_GlobalIntEnable(void);
uint8 `$INSTANCE_NAME`_GlobalIntDisable(void);
uint8 `$INSTANCE_NAME`_SetPreScaler(uint16 bitrate);
uint8 `$INSTANCE_NAME`_SetArbiter(uint8 arbiter);
uint8 `$INSTANCE_NAME`_SetTsegSample(uint8 cfg_tseg1, uint8 cfg_tseg2, uint8 sjw, uint8 sm);
uint8 `$INSTANCE_NAME`_SetRestartType(uint8 reset);
uint8 `$INSTANCE_NAME`_SetEdgeMode(uint8 edge);
uint8 `$INSTANCE_NAME`_SetOpMode(uint8 opmode);
uint8 `$INSTANCE_NAME`_RXRegisterInit(uint32 *reg, uint32 configuration);
uint8 `$INSTANCE_NAME`_WriteIrqMask(uint16 request);
uint8 `$INSTANCE_NAME`_GetTXErrorFlag(void);
uint8 `$INSTANCE_NAME`_GetRXErrorFlag(void);
uint8 `$INSTANCE_NAME`_GetTXErrorCount(void);
uint8 `$INSTANCE_NAME`_GetRXErrorCount(void);
uint8 `$INSTANCE_NAME`_GetRXErrorStat(void);


#if (`$INSTANCE_NAME`_ARB_LOST)
void `$INSTANCE_NAME`_ArbLostIsr(void);
#endif

#if (`$INSTANCE_NAME`_OVERLOAD)
void `$INSTANCE_NAME`_OvrLdErrorIsr(void);
#endif

#if (`$INSTANCE_NAME`_BIT_ERR)
void `$INSTANCE_NAME`_BitErrorIsr(void);
#endif

#if (`$INSTANCE_NAME`_STUFF_ERR)
void `$INSTANCE_NAME`_BitStuffErrorIsr(void);
#endif

#if (`$INSTANCE_NAME`_ACK_ERR)
void `$INSTANCE_NAME`_AckErrorIsr(void);
#endif

#if (`$INSTANCE_NAME`_FORM_ERR)
void `$INSTANCE_NAME`_MsgErrorIsr(void);
#endif

#if (`$INSTANCE_NAME`_CRC_ERR)
void `$INSTANCE_NAME`_CrcErrorIsr(void);
#endif

#if (`$INSTANCE_NAME`_BUS_OFF)
void `$INSTANCE_NAME`_BusOffIsr(void);
#endif

#if (`$INSTANCE_NAME`_RX_MSG_LOST)
void `$INSTANCE_NAME`_MsgLostIsr(void);
#endif

#if (`$INSTANCE_NAME`_TX_MSG)
void `$INSTANCE_NAME`_MsgTXIsr(void);
#endif

#if (`$INSTANCE_NAME`_RX_MSG)
void `$INSTANCE_NAME`_MsgRXIsr(void);
#endif

uint8 `$INSTANCE_NAME`_RxBufConfig(CANRXcfg *rxconfig);
uint8 `$INSTANCE_NAME`_TxBufConfig(CANTXcfg *txconfig);
uint8 `$INSTANCE_NAME`_SendMsg(CANTXMsg *message);
void  `$INSTANCE_NAME`_TxCancel(uint8 bufferld);
void `$INSTANCE_NAME`_ReceiveMsg(uint8 rxreg);

#if(`$INSTANCE_NAME`_TX0_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name0`(void);
#endif

#if(`$INSTANCE_NAME`_TX1_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name1`(void);
#endif

#if(`$INSTANCE_NAME`_TX2_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name2`(void);
#endif

#if(`$INSTANCE_NAME`_TX3_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name3`(void);
#endif

#if(`$INSTANCE_NAME`_TX4_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name4`(void);
#endif

#if(`$INSTANCE_NAME`_TX5_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name5`(void);
#endif

#if(`$INSTANCE_NAME`_TX6_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name6`(void);
#endif

#if(`$INSTANCE_NAME`_TX7_FUNC_ENABLE)
uint8 `$INSTANCE_NAME`_SendMsg`$TX_name7`(void);
#endif

#if(`$INSTANCE_NAME`_RX0_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name0`(void);
#endif

#if(`$INSTANCE_NAME`_RX1_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name1`(void);
#endif

#if(`$INSTANCE_NAME`_RX2_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name2`(void);
#endif

#if(`$INSTANCE_NAME`_RX3_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name3`(void);
#endif

#if(`$INSTANCE_NAME`_RX4_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name4`(void);
#endif

#if(`$INSTANCE_NAME`_RX5_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name5`(void);
#endif

#if(`$INSTANCE_NAME`_RX6_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name6`(void);
#endif

#if(`$INSTANCE_NAME`_RX7_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name7`(void);
#endif

#if(`$INSTANCE_NAME`_RX8_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name8`(void);
#endif

#if(`$INSTANCE_NAME`_RX9_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name9`(void);
#endif

#if(`$INSTANCE_NAME`_RX10_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name10`(void);
#endif

#if(`$INSTANCE_NAME`_RX11_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name11`(void);
#endif

#if(`$INSTANCE_NAME`_RX12_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name12`(void);
#endif

#if(`$INSTANCE_NAME`_RX13_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name13`(void);
#endif

#if(`$INSTANCE_NAME`_RX14_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name14`(void);
#endif

#if(`$INSTANCE_NAME`_RX15_FUNC_ENABLE)
void `$INSTANCE_NAME`_ReceiveMsg`$RX_name15`(void);
#endif

/* Interrupt handler */
CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

/***************************************
 *  Registers
 ***************************************/
#define `$INSTANCE_NAME`_TX			( (CANTXstruct XDATA *) `$INSTANCE_NAME``[CanIP]`_TX0_CMD )
#define `$INSTANCE_NAME`_RX			( (CANRXstruct XDATA *) `$INSTANCE_NAME``[CanIP]`_RX0_CMD )
#define `$INSTANCE_NAME`_INT_SR		( *(CAN_reg32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_SR )
#define `$INSTANCE_NAME`_INT_EN		( *(CAN_reg32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_INT_EN )
#define `$INSTANCE_NAME`_BUF_SR		( *(CAN_reg32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_BUF_SR )
#define `$INSTANCE_NAME`_ERR_SR		( *(CAN_reg32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_ERR_SR )
#define `$INSTANCE_NAME`_CMD		( *(CAN_reg32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_CMD )
#define `$INSTANCE_NAME`_CFG		( *(CAN_reg32 XDATA *) `$INSTANCE_NAME``[CanIP]`_CSR_CFG )
#define `$INSTANCE_NAME`_PWRMGR 	( *(reg8 *) `$INSTANCE_NAME``[CanIP]`_PM_ACT_CFG )  			/* Power manager */

#define `$INSTANCE_NAME`_RX_FIRST_REG	`$INSTANCE_NAME``[CanIP]`_RX0_CMD
#define `$INSTANCE_NAME`_RX_LAST_REG 	`$INSTANCE_NAME``[CanIP]`_RX15_ACRD

/* Priority of the `$INSTANCE_NAME`_isr interrupt. */
#define `$INSTANCE_NAME``[isr]`INTC_PRIOR_NUMBER      `$INSTANCE_NAME``[isr]`_INTC_PRIOR_NUM

/***************************************
 *  Constants
 ***************************************/

/* One or more parameters to the function were invalid. */
#define FAIL                						0x01u
#define OUT_OF_RANGE								0x02u

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_EN   `$INSTANCE_NAME``[CanIP]`_PM_ACT_MSK /* Power enable mask */

/* Number of TX and RX mailboxes */
#define `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES		8
#define `$INSTANCE_NAME`_NUMBER_OF_RX_MAILBOXES		16

/* Error status of CAN */
#define `$INSTANCE_NAME`_ERROR_ACTIVE				0x00u
#define `$INSTANCE_NAME`_ERROR_PASIVE				0x01u
#define `$INSTANCE_NAME`_ERROR_BUS_OFF				0x10u

/* Operation mode */
#define `$INSTANCE_NAME`_ACTIVE_MODE				0
#define `$INSTANCE_NAME`_LISTEN_ONLY				1
/* Run/Stop mode */
#define `$INSTANCE_NAME`_MODE_STOP					0
#define `$INSTANCE_NAME`_MODE_START					1
/* Transmit buffer arbiter */
#define `$INSTANCE_NAME`_ROUND_ROBIN				0
#define `$INSTANCE_NAME`_FIXED_PRIORITY				1
/* Restart type*/
#define `$INSTANCE_NAME`_RESTART_BY_HAND			0
#define `$INSTANCE_NAME`_AUTO_RESTART				1
/* Sampling mode */
#define `$INSTANCE_NAME`_ONE_SAMPLE_POINT			0
#define `$INSTANCE_NAME`_THREE_SAMPLE_POINTS		1
/* Edge mode*/
#define `$INSTANCE_NAME`_EDGE_R_TO_D				0
#define `$INSTANCE_NAME`_BOTH_EDGES					1

/* Write Protect Mask for Basic and Full mailboxes */

#define `$INSTANCE_NAME`_TX_WPN_SET					(((uint32)0x00000001 << `$INSTANCE_NAME`_TX_WPNL_SHIFT)|((uint32)0x00000001 << (`$INSTANCE_NAME`_TWO_BYTE_OFFSET + `$INSTANCE_NAME`_TX_WPNH_SHIFT)))

#define `$INSTANCE_NAME`_TX_WPN_CLEAR				(~ `$INSTANCE_NAME`_TX_WPN_SET)

#define `$INSTANCE_NAME`_RX_WPN_SET					(((uint32)0x00000001 << `$INSTANCE_NAME`_RX_WPNL_SHIFT)|((uint32)0x00000001 << (`$INSTANCE_NAME`_TWO_BYTE_OFFSET + `$INSTANCE_NAME`_RX_WPNH_SHIFT)))

#define `$INSTANCE_NAME`_RX_WPN_CLEAR				(~ `$INSTANCE_NAME`_RX_WPN_SET)

#define `$INSTANCE_NAME`_TX_READ_BACK_MASK			( `$INSTANCE_NAME`_TX_WPN_CLEAR & 0x00FF00FF )

#define `$INSTANCE_NAME`_RX_READ_BACK_MASK			( `$INSTANCE_NAME`_RX_WPN_CLEAR & 0x00FF00FF )

#define `$INSTANCE_NAME`_RX_CMD_REG_WIDTH			0x20u

/* TX send message */
#define `$INSTANCE_NAME`_TX_REQUEST_PENDING			0x01u
#define `$INSTANCE_NAME`_RETRY_NUMBER				3
#define `$INSTANCE_NAME`_SEND_MESSAGE_SHIFT			0
#define `$INSTANCE_NAME`_SEND_MESSAGE				((uint32)0x00000001 << `$INSTANCE_NAME`_SEND_MESSAGE_SHIFT)

/* Offsets to maintian bytes within uint32 */
#define `$INSTANCE_NAME`_ONE_BYTE_OFFSET			8
#define `$INSTANCE_NAME`_TWO_BYTE_OFFSET			16
#define `$INSTANCE_NAME`_TREE_BYTE_OFFSET			24

/* Set/Clear bits macro for `$INSTANCE_NAME`_RX maibox(i) */
#define `$INSTANCE_NAME`_RX_ACK_MSG_SHIFT			0
#define `$INSTANCE_NAME`_RX_ACK_MSG					(0x01u << `$INSTANCE_NAME`_RX_ACK_MSG_SHIFT)			/* bit0 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_ACK_MESSAGE(i)			`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_ACK_MSG
#define `$INSTANCE_NAME`_RX_RTR_ABORT_SHIFT			2
#define `$INSTANCE_NAME`_RX_RTR_ABORT_MASK			(0x01u << `$INSTANCE_NAME`_RX_RTR_ABORT_SHIFT)			/* bit2 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_RTR_ABORT_MESSAGE(i)	`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_RTR_ABORT_MASK
#define `$INSTANCE_NAME`_RX_BUF_ENABLE_SHIFT		3
#define `$INSTANCE_NAME`_RX_BUF_ENABLE_MASK			(0x01u << `$INSTANCE_NAME`_RX_BUF_ENABLE_SHIFT)			/* bit3 within RX_CMD[i] */
#define `$INSTANCE_NAME`_SET_RX_BUF_ENABLE(i)		`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_BUF_ENABLE_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_BUF_DISABLE(i)	`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] &= ~`$INSTANCE_NAME`_RX_BUF_ENABLE_MASK
#define `$INSTANCE_NAME`_RX_RTREPLY_SHIFT			4
#define `$INSTANCE_NAME`_RX_RTREPLY_MASK			(0x01u << `$INSTANCE_NAME`_RX_RTREPLY_SHIFT)			/* bit4 within RX_CMD[i] */
#define `$INSTANCE_NAME`_SET_RX_RTREPLY(i)			`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_RTREPLY_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_RTREPLY(i)		`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] &= ~`$INSTANCE_NAME`_RX_RTREPLY_MASK
#define `$INSTANCE_NAME`_RX_INT_ENABLE_SHIFT		5
#define `$INSTANCE_NAME`_RX_INT_ENABLE_MASK			(0x01u << `$INSTANCE_NAME`_RX_INT_ENABLE_SHIFT)			/* bit5 within RX_CMD[i] */
#define `$INSTANCE_NAME`_RX_INT_ENABLE(i)			`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_INT_ENABLE_MASK
#define `$INSTANCE_NAME`_RX_INT_DISABLE(i)			`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] &= ~`$INSTANCE_NAME`_RX_INT_ENABLE_MASK
#define `$INSTANCE_NAME`_RX_LINKING_SHIFT			6
#define `$INSTANCE_NAME`_RX_LINKING_MASK			(0x01u << `$INSTANCE_NAME`_RX_LINKING_SHIFT)			/* bit6 within RX_CMD[i] */	
#define `$INSTANCE_NAME`_SET_RX_LINKING(i)			`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_LINKING_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_LINKING(i)		`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] &= ~`$INSTANCE_NAME`_RX_LINKING_MASK
#define `$INSTANCE_NAME`_RX_WPNL_SHIFT				7
#define `$INSTANCE_NAME`_RX_WPNL_MASK				(0x01u << `$INSTANCE_NAME`_RX_WPNL_SHIFT)				/* bit7 within RX_CMD[i] */
#define `$INSTANCE_NAME`_SET_RX_WNPL(i)				`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] |= `$INSTANCE_NAME`_RX_WPNL_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_WNPL(i)			`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] &= ~`$INSTANCE_NAME`_RX_WPNL_MASK
#define `$INSTANCE_NAME`_RX_WPNH_SHIFT				7
#define `$INSTANCE_NAME`_RX_WPNH_MASK				(0x01u << `$INSTANCE_NAME`_RX_WPNH_SHIFT)			/* bit 23 within RX_CMD[i] */
#define `$INSTANCE_NAME`_SET_RX_WNPH(i)				`$INSTANCE_NAME`_RX[i].rxcmd.byte[2] |= `$INSTANCE_NAME`_RX_WPNH_MASK
#define `$INSTANCE_NAME`_CLEAR_RX_WNPH(i)			`$INSTANCE_NAME`_RX[i.rxcmd.byte[2] &= ~`$INSTANCE_NAME`_RX_WPNH_MASK

#define `$INSTANCE_NAME`_RX_DLC_VALUE_SHIFT			0
#define `$INSTANCE_NAME`_RX_DLC_VALUE_MASK			(0x0Fu << `$INSTANCE_NAME`_RX_DLC_VALUE_SHIFT) 				/* bits 19-16 within TX_CMD[i] */
#define `$INSTANCE_NAME`_GET_DLC(mailbox)			(`$INSTANCE_NAME`_RX[mailbox].rxcmd.byte[2] & `$INSTANCE_NAME`_RX_DLC_VALUE_MASK)

/* Set/Clear bits macro for `$INSTANCE_NAME`_TX mailbox(i) */
#define `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST_SHIFT	0			
#define `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST		(0x01u << `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST_SHIFT)		/* bit 0 within TX_CMD[i] */			
#define `$INSTANCE_NAME`_TX_TRANSMIT_MESSAGE(i)		`$INSTANCE_NAME`_TX[i].txcmd.byte[0] |= `$INSTANCE_NAME`_TX_TRANSMIT_REQUEST
#define `$INSTANCE_NAME`_TX_ABORT_SHIFT				1
#define `$INSTANCE_NAME`_TX_ABORT_MASK				(0x01u << `$INSTANCE_NAME`_TX_ABORT_SHIFT)					/* bit 1 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_ABORT_MESSAGE(i)		`$INSTANCE_NAME`_TX[i].txcmd.byte[0] |= `$INSTANCE_NAME`_TX_ABORT_MASK
#define `$INSTANCE_NAME`_TX_INT_ENABLE_SHIFT		2
#define `$INSTANCE_NAME`_TX_INT_ENABLE_MASK			((uint32)0x00000001 << `$INSTANCE_NAME`_TX_INT_ENABLE_SHIFT)				/* bit 2 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_INT_ENABLE(i)			`$INSTANCE_NAME`_TX[i].txcmd.byte[0] |= `$INSTANCE_NAME`_TX_INT_ENABLE_MASK
#define `$INSTANCE_NAME`_TX_INT_DISABLE(i)			`$INSTANCE_NAME`_TX[i].txcmd.byte[0] &= ~`$INSTANCE_NAME`_TX_INT_ENABLE_MASK
#define `$INSTANCE_NAME`_TX_WPNL_SHIFT				3
#define `$INSTANCE_NAME`_TX_WPNL_MASK				(0x01u << `$INSTANCE_NAME`_TX_WPNL_SHIFT)					/* bit 3 within TX_CMD[i] */
#define `$INSTANCE_NAME`_SET_TX_WNPL(i)				`$INSTANCE_NAME`_TX[i].txcmd.byte[0] |= `$INSTANCE_NAME`_TX_WPNL_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_WNPL(i)			`$INSTANCE_NAME`_TX[i].txcmd.byte[0] &= ~`$INSTANCE_NAME`_TX_WPNL_MASK
#define `$INSTANCE_NAME`_TX_DLC_VALUE_SHIFT			0
#define `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE_SHIFT	19
#define `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE			((uint32)0x00000001 << `$INSTANCE_NAME`_TX_DLC_UPPER_VALUE_SHIFT)
#define `$INSTANCE_NAME`_TX_DLC_VALUE_MASK			(0x0Fu << `$INSTANCE_NAME`_TX_DLC_VALUE_SHIFT) 				/* bits 19-16 within TX_CMD[i] */
#define `$INSTANCE_NAME`_TX_IDE_SHIFT				20
#define `$INSTANCE_NAME`_TX_IDE_MASK				((uint32)0x00000001 << `$INSTANCE_NAME`_TX_IDE_SHIFT)				/* bit 20 within TX_CMD[i] */
#define `$INSTANCE_NAME`_SET_TX_IDE(i)				`$INSTANCE_NAME`_TX[i].txcmd.byte[2] |= `$INSTANCE_NAME`_TX_IDE_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_IDE(i)			`$INSTANCE_NAME`_TX[i].txcmd.byte[2] &= ~`$INSTANCE_NAME`_TX_IDE_MASK
#define `$INSTANCE_NAME`_TX_RTR_SHIFT				21
#define `$INSTANCE_NAME`_TX_RTR_MASK				((uint32)0x00000001 <<  `$INSTANCE_NAME`_TX_RTR_SHIFT)				/* bit 21 within TX_CMD[i] */
#define `$INSTANCE_NAME`_SET_TX_RTR(i)				`$INSTANCE_NAME`_TX[i].txcmd.byte[2] |= `$INSTANCE_NAME`_TX_RTR_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_RTR(i)			`$INSTANCE_NAME`_TX[i].txcmd.byte[2] &= ~`$INSTANCE_NAME`_TX_RTR_MASK
#define `$INSTANCE_NAME`_TX_WPNH_SHIFT				7
#define `$INSTANCE_NAME`_TX_WPNH_MASK				(0x01u << `$INSTANCE_NAME`_TX_WPNH_SHIFT)					/* bit 23 within TX_CMD[i] */
#define `$INSTANCE_NAME`_SET_TX_WNPH(i)				`$INSTANCE_NAME`_TX[i].txcmd.byte[2] |= `$INSTANCE_NAME`_TX_WPN_MASK
#define `$INSTANCE_NAME`_CLEAR_TX_WNPH(i)			`$INSTANCE_NAME`_TX[i].txcmd.byte[2] &= ~`$INSTANCE_NAME`_TX_WPN_MASK

#define `$INSTANCE_NAME`_RX_IDE_SHIFT				4	/* bit 4 within RX_CMD[i].byte[2] */
#define `$INSTANCE_NAME`_RX_IDE_MASK				(0x01 << `$INSTANCE_NAME`_RX_IDE_SHIFT)
#define `$INSTANCE_NAME`_GET_RX_IDE(i)				(`$INSTANCE_NAME`_RX[i].rxcmd.byte[2] & `$INSTANCE_NAME`_RX_IDE_MASK)
#define `$INSTANCE_NAME`_GET_RX_ID(i)   				((`$INSTANCE_NAME`_GET_RX_IDE(i))? ((CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[i].rxid)) >> 3) \
                                         				:((CY_GET_REG32((reg32 *) & `$INSTANCE_NAME`_RX[i].rxid)) >> 21))

#define `$INSTANCE_NAME`_RX_DATA_BYTE(mailbox,i)		`$INSTANCE_NAME`_RX[mailbox].rxdata.byte[((i) > 3)?(7 - ((i)-4)):(3 - (i))]
#define `$INSTANCE_NAME`_TX_DATA_BYTE(mailbox,i)		`$INSTANCE_NAME`_TX[mailbox].txdata.byte[((i) > 3)?(7 - ((i)-4)):(3 - (i))]

/* Macros for access to RX DATA for mailbox(i) */
#define `$INSTANCE_NAME`_RX_DATA_BYTE1(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[3]
#define `$INSTANCE_NAME`_RX_DATA_BYTE2(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[2]
#define `$INSTANCE_NAME`_RX_DATA_BYTE3(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[1]
#define `$INSTANCE_NAME`_RX_DATA_BYTE4(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[0]
#define `$INSTANCE_NAME`_RX_DATA_BYTE5(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[7]
#define `$INSTANCE_NAME`_RX_DATA_BYTE6(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[6]
#define `$INSTANCE_NAME`_RX_DATA_BYTE7(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[5]
#define `$INSTANCE_NAME`_RX_DATA_BYTE8(i)			`$INSTANCE_NAME`_RX[i].rxdata.byte[4]

/* Macros for access to TX DATA for mailbox(i) */
#define `$INSTANCE_NAME`_TX_DATA_BYTE1(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[3]
#define `$INSTANCE_NAME`_TX_DATA_BYTE2(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[2]
#define `$INSTANCE_NAME`_TX_DATA_BYTE3(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[1]
#define `$INSTANCE_NAME`_TX_DATA_BYTE4(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[0]
#define `$INSTANCE_NAME`_TX_DATA_BYTE5(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[7]
#define `$INSTANCE_NAME`_TX_DATA_BYTE6(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[6]
#define `$INSTANCE_NAME`_TX_DATA_BYTE7(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[5]
#define `$INSTANCE_NAME`_TX_DATA_BYTE8(i)			`$INSTANCE_NAME`_TX[i].txdata.byte[4]

/* Mask for bits within `$INSTANCE_NAME`_CSR_CFG */
#define `$INSTANCE_NAME`_EDGE_MODE_SHIFT				0
#define `$INSTANCE_NAME`_EDGE_MODE_MASK					(0x01u << `$INSTANCE_NAME`_EDGE_MODE_SHIFT)				/* bit 0 within CSR_CFG	*/
#define `$INSTANCE_NAME`_SAMPLE_MODE_SHIFT				1
#define `$INSTANCE_NAME`_SAMPLE_MODE_MASK				(0x01u << `$INSTANCE_NAME`_SAMPLE_MODE_SHIFT)			/* bit 1 within CSR_CFG	*/
#define `$INSTANCE_NAME`_CFG_SJW_SHIFT					2
#define `$INSTANCE_NAME`_CFG_SJW_MASK					(0x03u << `$INSTANCE_NAME`_CFG_SJW_SHIFT)				/* bits 3-2 within CSR_CFG */
#define `$INSTANCE_NAME`_CFG_SJW_LOWER_LIMIT			0x03u			/* the lowest allowed value of cfg_sjw */
#define `$INSTANCE_NAME`_RESET_SHIFT					4	
#define `$INSTANCE_NAME`_RESET_MASK						(0x01u << `$INSTANCE_NAME`_RESET_SHIFT)					/* bit 4 within CSR_CFG	*/
#define `$INSTANCE_NAME`_CFG_TSEG2_SHIFT				5
#define `$INSTANCE_NAME`_CFG_TSEG2_MASK					(0x07u << `$INSTANCE_NAME`_CFG_TSEG2_SHIFT)	/* bits 7-5 within CSR_CFG */
#define `$INSTANCE_NAME`_CFG_TSEG2_UPPER_LIMIT			0x07u			/* the highest allowed value of cfg_tseg2 */
#define `$INSTANCE_NAME`_CFG_TSEG2_LOWER_LIMIT			0x02u			/* the lowest allowed value of cfg_tseg2 */
#define `$INSTANCE_NAME`_CFG_TSEG2_EXCEPTION			0x01u			/* the lowest allowed value of cfg_tseg2 if sample point is one point */
#define `$INSTANCE_NAME`_CFG_TSEG1_MASK					0x0Fu			/* bits 11-8 within CSR_CFG */
#define `$INSTANCE_NAME`_CFG_TSEG1_UPPER_LIMIT			0x0Fu			/* the highest allowed value of cfg_tseg1 */
#define `$INSTANCE_NAME`_CFG_TSEG1_LOWER_LIMIT			0x02u			/* the lowest allowed value of cfg_tseg1 */
#define `$INSTANCE_NAME`_ARBITRATION_SHIFT				4
#define `$INSTANCE_NAME`_ARBITRATION_MASK				(0x01u << `$INSTANCE_NAME`_ARBITRATION_SHIFT)			/* bit 12 within CSR_CFG */
#define `$INSTANCE_NAME`_BITRATE_MASK					0x7FFFu			/* bits 23-16 within CSR_CFG */

/* Mask for bits within `$INSTANCE_NAME`_CSR_CMD */
#define `$INSTANCE_NAME`_MODE_SHIFT						0
#define `$INSTANCE_NAME`_MODE_MASK						(0x01u << `$INSTANCE_NAME`_MODE_SHIFT)  				/* bit 0 within CSR_CMD */
#define `$INSTANCE_NAME`_OPMODE_MASK_SHIFT						1
#define `$INSTANCE_NAME`_OPMODE_MASK					(0x01u << `$INSTANCE_NAME`_OPMODE_MASK_SHIFT)			/* bit 1 within CSR_CMD */

/* Mask for bits within `$INSTANCE_NAME`_CSR_CMD */
#define `$INSTANCE_NAME`_ERROR_STATE_SHIFT				0				
#define `$INSTANCE_NAME`_ERROR_STATE_MASK				(0x03u << `$INSTANCE_NAME`_ERROR_STATE_SHIFT)			/* bit 17-16 within ERR_SR */
#define `$INSTANCE_NAME`_TX_ERROR_FLAG_SHIFT			2
#define `$INSTANCE_NAME`_TX_ERROR_FLAG_MASK				(0x01u << `$INSTANCE_NAME`_TX_ERROR_FLAG_SHIFT)			/* bit 18 within ERR_SR */
#define `$INSTANCE_NAME`_RX_ERROR_FLAG_SHIFT			3
#define `$INSTANCE_NAME`_RX_ERROR_FLAG_MASK				(0x01u << `$INSTANCE_NAME`_RX_ERROR_FLAG_SHIFT)			/* bit 19 within ERR_SR */ 


/* Mask and Macros for bits within `$INSTANCE_NAME`_INT_EN */			
#define `$INSTANCE_NAME`_GLOBAL_INT_SHIFT				0				
#define `$INSTANCE_NAME`_GLOBAL_INT_MASK				(0x01 << `$INSTANCE_NAME`_GLOBAL_INT_SHIFT)				/* bit 0 within INT_EN */
#define `$INSTANCE_NAME`_ARBITRATION_LOST_SHIFT			2				
#define `$INSTANCE_NAME`_ARBITRATION_LOST_MASK			(0x01u << `$INSTANCE_NAME`_ARBITRATION_LOST_SHIFT)		/* bit 2 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_ARBITRATION_LOST_INT_ENABLE	`$INSTANCE_NAME`_INT_EN.byte[0] |= `$INSTANCE_NAME`_ARBITRATION_LOST_MASK
#define `$INSTANCE_NAME`_ARBITRATION_LOST_INT_DISABLE	`$INSTANCE_NAME`_INT_EN.byte[0] &= ~`$INSTANCE_NAME`_ARBITRATION_LOST_MASK
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_SHIFT			3				
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK			(0x01u << `$INSTANCE_NAME`_OVERLOAD_ERROR_SHIFT)		/* bit 3 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_INT_ENABLE		`$INSTANCE_NAME`_INT_EN.byte[0] |= `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK
#define `$INSTANCE_NAME`_OVERLOAD_ERROR_INT_DISABLE		`$INSTANCE_NAME`_INT_EN.byte[0] &= ~`$INSTANCE_NAME`_OVERLOAD_ERROR_MASK
#define `$INSTANCE_NAME`_BIT_ERROR_SHIFT				4				
#define `$INSTANCE_NAME`_BIT_ERROR_MASK					(0x01u << `$INSTANCE_NAME`_BIT_ERROR_SHIFT)			/* bit 4 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_BIT_ERROR_LOST_INT_ENABLE		`$INSTANCE_NAME`_INT_EN.byte[0] |= `$INSTANCE_NAME`_BIT_ERROR_MASK
#define `$INSTANCE_NAME`_BIT_ERROR_LOST_INT_DISABLE		`$INSTANCE_NAME`_INT_EN.byte[0] &= ~`$INSTANCE_NAME`_BIT_ERROR_MASK
#define `$INSTANCE_NAME`_STUFF_ERROR_SHIFT				5				
#define `$INSTANCE_NAME`_STUFF_ERROR_MASK				(0x01u << `$INSTANCE_NAME`_STUFF_ERROR_SHIFT)			/* bit 5 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_STUFF_ERROR_INT_ENABLE			`$INSTANCE_NAME`_INT_EN.byte[0] |= `$INSTANCE_NAME`_STUFF_ERROR_MASK
#define `$INSTANCE_NAME`_STUFF_ERROR_INT_DISABLE		`$INSTANCE_NAME`_INT_EN.byte[0] &= ~`$INSTANCE_NAME`_STUFF_ERROR_MASK
#define `$INSTANCE_NAME`_ACK_ERROR_SHIFT				6				
#define `$INSTANCE_NAME`_ACK_ERROR_MASK					(0x01u << `$INSTANCE_NAME`_ACK_ERROR_SHIFT)				/* bit 6 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_ACK_ERROR_INT_ENABLE			`$INSTANCE_NAME`_INT_EN.byte[0] |= `$INSTANCE_NAME`_ACK_ERROR_MASK
#define `$INSTANCE_NAME`_ACK_ERROR_INT_DISABLE			`$INSTANCE_NAME`_INT_EN.byte[0] &= ~`$INSTANCE_NAME`_ACK_ERROR_MASK
#define `$INSTANCE_NAME`_FORM_ERROR_SHIFT				7				
#define `$INSTANCE_NAME`_FORM_ERROR_MASK				(0x01u << `$INSTANCE_NAME`_FORM_ERROR_SHIFT)			/* bit 7 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_FORM_ERROR_INT_ENABLE			`$INSTANCE_NAME`_INT_EN.byte[0] |= `$INSTANCE_NAME`_FORM_ERROR_MASK
#define `$INSTANCE_NAME`_FORM_ERROR_INT_DISABLE			`$INSTANCE_NAME`_INT_EN.byte[0] &= ~`$INSTANCE_NAME`_FORM_ERROR_MASK
#define `$INSTANCE_NAME`_CRC_ERROR_SHIFT				0			
#define `$INSTANCE_NAME`_CRC_ERROR_MASK					(0x01u << `$INSTANCE_NAME`_CRC_ERROR_SHIFT)				/* bit 8 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_CRC_ERROR_INT_ENABLE			`$INSTANCE_NAME`_INT_EN.byte[1] |= `$INSTANCE_NAME`_CRC_ERROR_MASK
#define `$INSTANCE_NAME`_CRC_ERROR_INT_DISABLE			`$INSTANCE_NAME`_INT_EN.byte[1] &= ~`$INSTANCE_NAME`_CRC_ERROR_MASK
#define `$INSTANCE_NAME`_BUS_OFF_SHIFT					1			
#define `$INSTANCE_NAME`_BUS_OFF_MASK					(0x01u << `$INSTANCE_NAME`_BUS_OFF_SHIFT)				/* bit 9 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_BUS_OFF_INT_ENABLE				`$INSTANCE_NAME`_INT_EN.byte[1] |= `$INSTANCE_NAME`_BUS_OFF_MASK
#define `$INSTANCE_NAME`_BUS_OFF_INT_DISABLE			`$INSTANCE_NAME`_INT_EN.byte[1] &= ~`$INSTANCE_NAME`_BUS_OFF_MASK
#define `$INSTANCE_NAME`_RX_MSG_LOST_SHIFT				2			
#define `$INSTANCE_NAME`_RX_MSG_LOST_MASK				(0x01u << `$INSTANCE_NAME`_RX_MSG_LOST_SHIFT)			/* bit 10 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_RX_MSG_LOST_INT_ENABLE			`$INSTANCE_NAME`_INT_EN.byte[1] |= `$INSTANCE_NAME`_RX_MSG_LOST_MASK
#define `$INSTANCE_NAME`_RX_MSG_LOST_INT_DISABLE		`$INSTANCE_NAME`_INT_EN.byte[1] &= ~`$INSTANCE_NAME`_RX_MSG_LOST_MASK
#define `$INSTANCE_NAME`_TX_MSG_SHIFT					3			
#define `$INSTANCE_NAME`_TX_MSG_MASK					(0x01u << `$INSTANCE_NAME`_TX_MSG_SHIFT)				/* bit 11 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_TX_MSG_INT_ENABLE				`$INSTANCE_NAME`_INT_EN.byte[1] |= `$INSTANCE_NAME`_TX_MSG_MASK
#define `$INSTANCE_NAME`_TX_MSG_INT_DISABLE				`$INSTANCE_NAME`_INT_EN.byte[1] &= ~`$INSTANCE_NAME`_TX_MSG_MASK
#define `$INSTANCE_NAME`_RX_MSG_SHIFT					4			
#define `$INSTANCE_NAME`_RX_MSG_MASK					(0x01u << `$INSTANCE_NAME`_RX_MSG_SHIFT)				/* bit 12 within INT_EN and INT_SR */
#define `$INSTANCE_NAME`_RX_MSG_INT_ENABLE				`$INSTANCE_NAME`_INT_EN.byte[1] |= `$INSTANCE_NAME`_RX_MSG_MASK
#define `$INSTANCE_NAME`_RX_MSG_INT_DISABLE				`$INSTANCE_NAME`_INT_EN.byte[1] &= ~`$INSTANCE_NAME`_RX_MSG_MASK

#endif /* CY_CAN_`$INSTANCE_NAME`_H */

///* [] END OF FILE */
//

