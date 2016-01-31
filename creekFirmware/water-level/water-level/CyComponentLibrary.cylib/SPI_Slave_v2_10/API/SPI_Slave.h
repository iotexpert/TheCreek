/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Contains the function prototypes, constants and register definition
*  of the SPI Slave Component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_SPIS_`$INSTANCE_NAME`_H)
#define CY_SPIS_`$INSTANCE_NAME`_H

#include "cyfitter.h"

/* PSoC3 ES2 or earlier*/
#define `$INSTANCE_NAME`_PSOC3_ES2   ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) && \
                                       (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2))

/* PSoC5 ES1 or earlier */
#define `$INSTANCE_NAME`_PSOC5_ES1   ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A) && \
                                       (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_5A_ES1))

#ifdef `$INSTANCE_NAME`_TxInternalInterrupt__ES2_PATCH
    #if(`$INSTANCE_NAME`_PSOC3_ES2 && `$INSTANCE_NAME`_TxInternalInterrupt__ES2_PATCH)
        #include <intrins.h>
        #define `$INSTANCE_NAME`_TX_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif /* End PSOC3_ES2 */
#endif /* `$INSTANCE_NAME`_TxInternalInterrupt__ES2_PATCH */

#ifdef `$INSTANCE_NAME`_RxInternalInterrupt__ES2_PATCH
    #if(`$INSTANCE_NAME`_PSOC3_ES2 && `$INSTANCE_NAME`_RxInternalInterrupt__ES2_PATCH)
        #include <intrins.h>
        #define `$INSTANCE_NAME`_RX_ISR_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_(); _nop_();
    #endif /* End PSOC3_ES2 */
#endif /* `$INSTANCE_NAME`_RxInternalInterrupt__ES2_PATCH */


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_DataWidth                  (`$NumberOfDataBits`u)
#define `$INSTANCE_NAME`_InternalTxInterruptEnabled (`$InternalTxInterruptEnabled`u)
#define `$INSTANCE_NAME`_InternalRxInterruptEnabled (`$InternalRxInterruptEnabled`u)
#define `$INSTANCE_NAME`_ModeUseZero                (`$ModeUseZero`u)
#define `$INSTANCE_NAME`_BidirectionalMode          (`$BidirectMode`u)
#define `$INSTANCE_NAME`_Mode                       (`$Mode`u)

/* Following definitions are for version Compatibility, they are obsolete.
*  Please do not use it in new projects
*/
#define `$INSTANCE_NAME`_DATAWIDHT                (`$INSTANCE_NAME`_DataWidth)
#define `$INSTANCE_NAME`_InternalInterruptEnabled (`$InternalInterruptEnabled`u)


/***************************************
*        Data Struct Definition
***************************************/

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8 enableState;
    uint8 cntrPeriod;

    #if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) /* PSoC3 ES2 or earlier, PSoC5 ES1 */

        `$RegSizeReplacementString` saveSrTxIntMask;
        `$RegSizeReplacementString` saveSrRxIntMask;

    #endif /* End `$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1 */

} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void  `$INSTANCE_NAME`_Init(void);
void  `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void  `$INSTANCE_NAME`_EnableTxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableTxInt")`;
void  `$INSTANCE_NAME`_EnableRxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableRxInt")`;
void  `$INSTANCE_NAME`_DisableTxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableTxInt")`;
void  `$INSTANCE_NAME`_DisableRxInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableRxInt")`;
void  `$INSTANCE_NAME`_SetTxInterruptMode(uint8 intSource) `=ReentrantKeil($INSTANCE_NAME . "_SetTxInterruptMode")`;
void  `$INSTANCE_NAME`_SetRxInterruptMode(uint8 intSource) `=ReentrantKeil($INSTANCE_NAME . "_SetRxInterruptMode")`;
uint8 `$INSTANCE_NAME`_ReadTxStatus(void);
uint8 `$INSTANCE_NAME`_ReadRxStatus(void);
void  `$INSTANCE_NAME`_WriteTxData(`$RegSizeReplacementString` txData);

#if(`$INSTANCE_NAME`_ModeUseZero == 1u)
    void  `$INSTANCE_NAME`_WriteTxDataZero(`$RegSizeReplacementString` txDataByte) \
                                              `=ReentrantKeil($INSTANCE_NAME . "_WriteTxDataZero")`;
#endif /* (`$INSTANCE_NAME`_ModeUseZero == 1u) */

`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadRxData(void);
uint8 `$INSTANCE_NAME`_GetRxBufferSize(void) `=ReentrantKeil($INSTANCE_NAME . "_GetRxBufferSize")`;
uint8 `$INSTANCE_NAME`_GetTxBufferSize(void) `=ReentrantKeil($INSTANCE_NAME . "_GetTxBufferSize")`;
void  `$INSTANCE_NAME`_ClearRxBuffer(void);
void  `$INSTANCE_NAME`_ClearTxBuffer(void);

#if (`$INSTANCE_NAME`_BidirectionalMode == 1u)
    void  `$INSTANCE_NAME`_TxEnable(void) `=ReentrantKeil($INSTANCE_NAME . "_TxEnable")`;
    void  `$INSTANCE_NAME`_TxDisable(void) `=ReentrantKeil($INSTANCE_NAME . "_TxDisable")`;
#endif /* `$INSTANCE_NAME`_BidirectionalMode == 1u */

void  `$INSTANCE_NAME`_PutArray(`$RegSizeReplacementString` *buffer, uint8 byteCount);
void  `$INSTANCE_NAME`_ClearFIFO(void) `=ReentrantKeil($INSTANCE_NAME . "_ClearFIFO")`;
void  `$INSTANCE_NAME`_Sleep(void);
void  `$INSTANCE_NAME`_Wakeup(void);
void  `$INSTANCE_NAME`_SaveConfig(void);
void  `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

CY_ISR_PROTO(`$INSTANCE_NAME`_TX_ISR);
CY_ISR_PROTO(`$INSTANCE_NAME`_RX_ISR);

/* Macros for getting software status of SPIS Statusi Register */
#define `$INSTANCE_NAME`_GET_STATUS_TX(swTxSts) (uint8)(`$INSTANCE_NAME`_TX_STATUS_REG | \
                                                       (swTxSts & `$INSTANCE_NAME`_STS_CLR_ON_RD_BYTES_MASK))
#define `$INSTANCE_NAME`_GET_STATUS_RX(swRxSts) (uint8)(`$INSTANCE_NAME`_RX_STATUS_REG | \
                                                       (swRxSts & `$INSTANCE_NAME`_STS_CLR_ON_RD_BYTES_MASK))

/* Following definitions are for version Compatibility, they are obsolete.
*  Please do not use it in new projects
*/
#define `$INSTANCE_NAME`_WriteByte      (`$INSTANCE_NAME`_WriteTxData)
#define `$INSTANCE_NAME`_ReadByte       (`$INSTANCE_NAME`_ReadRxData)
#define `$INSTANCE_NAME`_WriteByteZero  (`$INSTANCE_NAME`_WriteTxDataZero)
void  `$INSTANCE_NAME`_SetInterruptMode(uint8 intSource) `=ReentrantKeil($INSTANCE_NAME . "_SetInterruptMode")`;
uint8 `$INSTANCE_NAME`_ReadStatus(void);
void  `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
void  `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;


/***************************************
*           API Constants
***************************************/

#define `$INSTANCE_NAME`_TX_ISR_NUMBER     (`$INSTANCE_NAME`_TxInternalInterrupt__INTC_NUMBER)
#define `$INSTANCE_NAME`_RX_ISR_NUMBER     (`$INSTANCE_NAME`_RxInternalInterrupt__INTC_NUMBER)
#define `$INSTANCE_NAME`_TX_ISR_PRIORITY   (`$INSTANCE_NAME`_TxInternalInterrupt__INTC_PRIOR_NUM)
#define `$INSTANCE_NAME`_RX_ISR_PRIORITY   (`$INSTANCE_NAME`_RxInternalInterrupt__INTC_PRIOR_NUM)


/***************************************
*    Initial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_INT_ON_SPI_DONE    (`$IntOnSPIDone`u << `$INSTANCE_NAME`_STS_SPI_DONE_SHIFT)
#define `$INSTANCE_NAME`_INT_ON_TX_EMPTY    (`$InterruptOnTXEmpty`u << `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_INT_ON_TX_NOT_FULL (`$IntOnTXNotFull`u << `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL_SHIFT)
#define `$INSTANCE_NAME`_INT_ON_BYTE_COMP   (`$IntOnByteComp`u << `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT)

#define `$INSTANCE_NAME`_TX_INIT_INTERRUPTS_MASK  (`$INSTANCE_NAME`_INT_ON_SPI_DONE | \
                                            `$INSTANCE_NAME`_INT_ON_TX_EMPTY | `$INSTANCE_NAME`_INT_ON_TX_NOT_FULL | \
                                            `$INSTANCE_NAME`_INT_ON_BYTE_COMP)

#define `$INSTANCE_NAME`_INT_ON_RX_EMPTY     (`$InterruptOnRXEmpty`u << `$INSTANCE_NAME`_STS_RX_FIFO_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_INT_ON_RX_NOT_EMPTY (`$IntOnRXNotEmpty`u << `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_INT_ON_RX_OVER      (`$IntOnRXOver`u << `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN_SHIFT)
#define `$INSTANCE_NAME`_INT_ON_RX_FULL      (`$IntOnRXFull`u << `$INSTANCE_NAME`_STS_RX_FIFO_FULL_SHIFT)

#define `$INSTANCE_NAME`_RX_INIT_INTERRUPTS_MASK (`$INSTANCE_NAME`_INT_ON_RX_EMPTY | \
                                            `$INSTANCE_NAME`_INT_ON_RX_NOT_EMPTY | `$INSTANCE_NAME`_INT_ON_RX_OVER | \
                                            `$INSTANCE_NAME`_INT_ON_RX_FULL)

#define `$INSTANCE_NAME`_BITCTR_INIT           (`$INSTANCE_NAME`_DataWidth - 1u)

`#DECLARE_ENUM SPI_Modes`

#define `$INSTANCE_NAME`_TXBUFFERSIZE          (`$TxBufferSize`u)
#define `$INSTANCE_NAME`_RXBUFFERSIZE          (`$RxBufferSize`u)

/* Following definitions are for version Compatibility, they are obsolete.
*  Please do not use it in new projects
*/
#define `$INSTANCE_NAME`_INIT_INTERRUPTS_MASK  (`$INSTANCE_NAME`_INT_ON_SPI_DONE | `$INSTANCE_NAME`_INT_ON_TX_EMPTY | \
                                            `$INSTANCE_NAME`_INT_ON_TX_NOT_FULL | `$INSTANCE_NAME`_INT_ON_RX_EMPTY | \
                                            `$INSTANCE_NAME`_INT_ON_RX_NOT_EMPTY | `$INSTANCE_NAME`_INT_ON_RX_OVER | \
                                            `$INSTANCE_NAME`_INT_ON_BYTE_COMP)


/***************************************
*             Registers
***************************************/

#if(`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)

    #define `$INSTANCE_NAME`_TXDATA_ZERO_REG           (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u0__A0_REG)

    #define `$INSTANCE_NAME`_TXDATA_ZERO_PTR           (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u0__A0_REG)

    #define `$INSTANCE_NAME`_RXDATA_ZERO_REG           (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMOSI_u0__A0_REG)

    #define `$INSTANCE_NAME`_RXDATA_ZERO_PTR           (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMOSI_u0__A0_REG)

    #define `$INSTANCE_NAME`_TXDATA_REG                (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u0__F0_REG)

    #define `$INSTANCE_NAME`_TXDATA_PTR                (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u0__F0_REG)

    #define `$INSTANCE_NAME`_RXDATA_REG                (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMOSI_u0__F0_REG)

    #define `$INSTANCE_NAME`_RXDATA_PTR                (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMOSI_u0__F0_REG)

    #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP0_REG       (* (reg8 *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP0_PTR       (  (reg8 *) \
            `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u0__DP_AUX_CTL_REG)

    #if(`$INSTANCE_NAME`_DataWidth > 8u)

        #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP1_REG   (* (reg8 *) \
                `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u1__DP_AUX_CTL_REG)
        #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP1_PTR   (  (reg8 *) \
                `$INSTANCE_NAME`_BSPIS_es2_SPISlave_`$VerilogSectionReplacementString`_DpMISO_u1__DP_AUX_CTL_REG)

    #endif /* `$INSTANCE_NAME`_DataWidth > 8u */

#else /* `$INSTANCE_NAME`_PSOC3_ES3  || `$INSTANCE_NAME`_PSOC5_ES2 or later */

    #define `$INSTANCE_NAME`_TXDATA_ZERO_REG          (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__A0_REG)

    #define `$INSTANCE_NAME`_TXDATA_ZERO_PTR           (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__A0_REG)

    #define `$INSTANCE_NAME`_RXDATA_ZERO_REG           (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__A0_REG)

    #define `$INSTANCE_NAME`_RXDATA_ZERO_PTR           (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__A0_REG)

    #define `$INSTANCE_NAME`_TXDATA_REG                (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__F0_REG)

    #define `$INSTANCE_NAME`_TXDATA_PTR                (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__F0_REG)

    #define `$INSTANCE_NAME`_RXDATA_REG                (* (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__F1_REG)

    #define `$INSTANCE_NAME`_RXDATA_PTR                (  (`$RegDefReplacementString`  *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__F1_REG)

    #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP0_REG       (* (reg8 *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP0_PTR       (  (reg8 *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__DP_AUX_CTL_REG)

    #define `$INSTANCE_NAME`_RX_AUX_CONTROL_DP0_REG       (* (reg8 *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_RX_AUX_CONTROL_DP0_PTR       (  (reg8 *) \
            `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u0__DP_AUX_CTL_REG)

    #if(`$INSTANCE_NAME`_DataWidth > 8u)

        #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP1_REG   (* (reg8 *) \
                `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u1__DP_AUX_CTL_REG)
        #define `$INSTANCE_NAME`_TX_AUX_CONTROL_DP1_PTR   (  (reg8 *) \
                `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u1__DP_AUX_CTL_REG)

        #define `$INSTANCE_NAME`_RX_AUX_CONTROL_DP1_REG   (* (reg8 *) \
                `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u1__DP_AUX_CTL_REG)
        #define `$INSTANCE_NAME`_RX_AUX_CONTROL_DP1_PTR   (  (reg8 *) \
                `$INSTANCE_NAME`_BSPIS_es3_SPISlave_`$VerilogSectionReplacementString`_Dp_u1__DP_AUX_CTL_REG)

    #endif /* `$INSTANCE_NAME`_DataWidth > 8u */

#endif /* (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1) or earlier */

#define `$INSTANCE_NAME`_COUNTER_PERIOD_REG    (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_BitCounter__PERIOD_REG)
#define `$INSTANCE_NAME`_COUNTER_PERIOD_PTR    (  (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_BitCounter__PERIOD_REG)

#define `$INSTANCE_NAME`_TX_STATUS_MASK_REG    (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_TxStsReg__MASK_REG)
#define `$INSTANCE_NAME`_TX_STATUS_MASK_PTR    (  (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_TxStsReg__MASK_REG)

#define `$INSTANCE_NAME`_RX_STATUS_MASK_REG    (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_RxStsReg__MASK_REG)
#define `$INSTANCE_NAME`_RX_STATUS_MASK_PTR    (  (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_RxStsReg__MASK_REG)

#define `$INSTANCE_NAME`_ONE_REG               (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_dpCounter_u0__D1_REG)
#define `$INSTANCE_NAME`_ONE_PTR               (  (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_dpCounter_u0__D1_REG)

#define `$INSTANCE_NAME`_TX_STATUS_REG         (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_TxStsReg__STATUS_REG)
#define `$INSTANCE_NAME`_TX_STATUS_PTR         (  (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_TxStsReg__STATUS_REG)

#define `$INSTANCE_NAME`_RX_STATUS_REG         (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_RxStsReg__STATUS_REG)
#define `$INSTANCE_NAME`_RX_STATUS_PTR         (  (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_RxStsReg__STATUS_REG)

#define `$INSTANCE_NAME`_COUNTER_CONTROL_REG   (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_BitCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_COUNTER_CONTROL_PTR   (  (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_BitCounter__CONTROL_AUX_CTL_REG)

#define `$INSTANCE_NAME`_TX_STATUS_ACTL_REG    (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_TxStsReg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_TX_STATUS_ACTL_PTR    (  (reg8 *) \
        `$INSTANCE_NAME`_TX_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_TxStsReg__STATUS_AUX_CTL_REG)

#define `$INSTANCE_NAME`_RX_STATUS_ACTL_REG    (* (reg8 *) \
        `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_RxStsReg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_RX_STATUS_ACTL_PTR    (  (reg8 *) \
        `$INSTANCE_NAME`_RX_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_RxStsReg__STATUS_AUX_CTL_REG)

#if(`$INSTANCE_NAME`_BidirectionalMode)

    #define `$INSTANCE_NAME`_CONTROL_REG       (* (reg8 *) \
   `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG)
    #define `$INSTANCE_NAME`_CONTROL_PTR       (  (reg8 *) \
   `$INSTANCE_NAME`_BSPIS_`$SiliconRevisionReplacementString`_SPISlave_`$CtlModeReplacementString`_CtrlReg__CONTROL_REG)

#endif /* `$INSTANCE_NAME`_BidirectionalMode */

/* Obsolete register names. Not to be used in new designs */
#define `$INSTANCE_NAME`_TXDATA_ZERO               (`$INSTANCE_NAME`_TXDATA_ZERO_REG)
#define `$INSTANCE_NAME`_TXDATA                    (`$INSTANCE_NAME`_TXDATA_REG)
#define `$INSTANCE_NAME`_RXDATA                    (`$INSTANCE_NAME`_RXDATA_REG)
#define `$INSTANCE_NAME`_MISO_AUX_CONTROLDP0       (`$INSTANCE_NAME`_MISO_AUX_CTRL_DP0_REG)
#define `$INSTANCE_NAME`_MOSI_AUX_CONTROLDP0       (`$INSTANCE_NAME`_MOSI_AUX_CTRL_DP0_REG)
#define `$INSTANCE_NAME`_TXBUFFERREAD              (`$INSTANCE_NAME`_txBufferRead)
#define `$INSTANCE_NAME`_TXBUFFERWRITE             (`$INSTANCE_NAME`_txBufferWrite)
#define `$INSTANCE_NAME`_RXBUFFERREAD              (`$INSTANCE_NAME`_rxBufferRead)
#define `$INSTANCE_NAME`_RXBUFFERWRITE             (`$INSTANCE_NAME`_rxBufferWrite)

#if(`$INSTANCE_NAME`_DataWidth > 8u)

    #define `$INSTANCE_NAME`_MISO_AUX_CONTROLDP1   (`$INSTANCE_NAME`_MISO_AUX_CTRL_DP1_REG)
    #define `$INSTANCE_NAME`_MOSI_AUX_CONTROLDP1   (`$INSTANCE_NAME`_MOSI_AUX_CTRL_DP0_REG)

#endif /* `$INSTANCE_NAME`_DataWidth > 8u */

#define `$INSTANCE_NAME`_COUNTER_PERIOD            (`$INSTANCE_NAME`_COUNTER_PERIOD_REG)
#define `$INSTANCE_NAME`_COUNTER_CONTROL           (`$INSTANCE_NAME`_COUNTER_CONTROL_REG)
#define `$INSTANCE_NAME`_ONE                       (`$INSTANCE_NAME`_ONE_REG)
#define `$INSTANCE_NAME`_STATUS                    (`$INSTANCE_NAME`_TX_STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_MASK               (`$INSTANCE_NAME`_TX_STATUS_MASK_REG)
#define `$INSTANCE_NAME`_STATUS_ACTL               (`$INSTANCE_NAME`_TX_STATUS_ACTL_REG)


/***************************************
*       Register Constants
***************************************/

/* Status Register Definitions */
#define `$INSTANCE_NAME`_STS_SPI_DONE_SHIFT             (0x00u)
#define `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL_SHIFT     (0x01u)
#define `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY_SHIFT        (0x02u)
#define `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY_SHIFT    (0x03u)
#define `$INSTANCE_NAME`_STS_RX_FIFO_EMPTY_SHIFT        (0x04u)
#define `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN_SHIFT      (0x05u)
#define `$INSTANCE_NAME`_STS_RX_FIFO_FULL_SHIFT         (0x06u)
#define `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT        (0x06u)

#define `$INSTANCE_NAME`_STS_SPI_DONE                   (0x01u << `$INSTANCE_NAME`_STS_SPI_DONE_SHIFT)
#define `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY              (0x01u << `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL           (0x01u << `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL_SHIFT)
#define `$INSTANCE_NAME`_STS_RX_FIFO_EMPTY              (0x01u << `$INSTANCE_NAME`_STS_RX_FIFO_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY          (0x01u << `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN            (0x01u << `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN_SHIFT)
#define `$INSTANCE_NAME`_STS_RX_FIFO_FULL               (0x01u << `$INSTANCE_NAME`_STS_RX_FIFO_FULL_SHIFT)
#define `$INSTANCE_NAME`_STS_BYTE_COMPLETE              (0x01u << `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT)

#define `$INSTANCE_NAME`_STS_CLR_ON_RD_BYTES_MASK       (0x61u)

/* StatusI Register Interrupt Enable Control Bits */
/* As defined by the Register map for the AUX Control Register */
#define `$INSTANCE_NAME`_INT_ENABLE                     (0x10u)
#define `$INSTANCE_NAME`_FIFO_CLR                       (0x03u)

/* Bit Counter (7-bit) Control Register Bit Definitions */
/* As defined by the Register map for the AUX Control Register */
#define `$INSTANCE_NAME`_CNTR_ENABLE                    (0x20u)

/* Bi-Directional mode control bit */
#define `$INSTANCE_NAME`_CTRL_TX_SIGNAL_EN              (0x01u)

/* Datapath Auxillary Control Register definitions */
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR             (0x00u)
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_CLR             (0x00u)
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_LVL             (0x00u)
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_LVL             (0x00u)
#define `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK        (0x10u)

/* Obosolete Status Register Definitions *DO NOT USE IN NEW DESIGNS* */
#define `$INSTANCE_NAME`_STS_TX_BUF_NOT_FULL_SHIFT      (0x01u)
#define `$INSTANCE_NAME`_STS_TX_BUF_FULL_SHIFT          (0x02u)
#define `$INSTANCE_NAME`_STS_RX_BUF_NOT_EMPTY_SHIFT     (0x03u)
#define `$INSTANCE_NAME`_STS_RX_BUF_EMPTY_SHIFT         (0x04u)
#define `$INSTANCE_NAME`_STS_RX_BUF_OVERRUN_SHIFT       (0x05u)

#define `$INSTANCE_NAME`_STS_TX_BUF_NOT_FULL            (0x01u << `$INSTANCE_NAME`_STS_TX_BUF_NOT_FULL_SHIFT)
#define `$INSTANCE_NAME`_STS_TX_BUF_FULL                (0x01u << `$INSTANCE_NAME`_STS_TX_BUF_FULL_SHIFT)
#define `$INSTANCE_NAME`_STS_RX_BUF_NOT_EMPTY           (0x01u << `$INSTANCE_NAME`_STS_RX_BUF_NOT_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_STS_RX_BUF_EMPTY               (0x01u << `$INSTANCE_NAME`_STS_RX_BUF_EMPTY_SHIFT)
#define `$INSTANCE_NAME`_STS_RX_BUF_OVERRUN             (0x01u << `$INSTANCE_NAME`_STS_RX_BUF_OVERRUN_SHIFT)

#endif  /* CY_SPIS_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
