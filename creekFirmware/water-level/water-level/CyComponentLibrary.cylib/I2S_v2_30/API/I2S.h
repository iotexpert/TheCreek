/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values and API definition for the 
*  I2S Component
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_I2S_`$INSTANCE_NAME`_H)
#define CY_I2S_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"

/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */
#ifndef CY_PSOC5A
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* CY_PSOC5A */


/*************************************** 
*   Conditional Compilation Parameters
***************************************/

`#declare_enum I2S_Direction`
#define `$INSTANCE_NAME`_DIRECTION (`$Direction`u)

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_DATA_INTERLEAVING   (`$TxDataInterleaving`u)
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    #define `$INSTANCE_NAME`_RX_DATA_INTERLEAVING   (`$RxDataInterleaving`u)
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */


/***************************************
*     Data Struct Definitions
***************************************/

/* Sleep Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8 enableState;
    
    uint8 CtrlReg;
    
    #if (CY_UDB_V0)
        uint8 Cnt7Period;          
        #if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
            (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
            uint8 TxIntMask;
        #endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */ 
        
        #if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
            (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
            uint8 RxIntMask;
        #endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */   
    #endif /* (CY_UDB_V0) */    
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes            
***************************************/

void  `$INSTANCE_NAME`_Start(void)                  `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void  `$INSTANCE_NAME`_Stop(void)                   `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void  `$INSTANCE_NAME`_Sleep(void)                  `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;                  
void  `$INSTANCE_NAME`_Wakeup(void)                 `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;
void  `$INSTANCE_NAME`_Enable(void)                 `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void  `$INSTANCE_NAME`_Init(void)                   `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void  `$INSTANCE_NAME`_SaveConfig(void)             `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void  `$INSTANCE_NAME`_RestoreConfig(void)          `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    void   `$INSTANCE_NAME`_EnableTx(void)          `=ReentrantKeil($INSTANCE_NAME . "_EnableTx")`;
    void   `$INSTANCE_NAME`_DisableTx(void)         `=ReentrantKeil($INSTANCE_NAME . "_DisableTx")`;
    void   `$INSTANCE_NAME`_WriteByte(uint8 wrData, uint8 wordSelect) \
                                                    `=ReentrantKeil($INSTANCE_NAME . "_WriteByte")`;
    void   `$INSTANCE_NAME`_ClearTxFIFO(void)       `=ReentrantKeil($INSTANCE_NAME . "_ClearTxFIFO")`;
    void   `$INSTANCE_NAME`_SetTxInterruptMode(uint8 interruptSource) \
                                                    `=ReentrantKeil($INSTANCE_NAME . "_SetTxInterruptMode")`;
    uint8  `$INSTANCE_NAME`_ReadTxStatus(void)      `=ReentrantKeil($INSTANCE_NAME . "_ReadTxStatus")`;
#endif    /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */ 

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    void   `$INSTANCE_NAME`_EnableRx(void)          `=ReentrantKeil($INSTANCE_NAME . "_EnableRx")`;
    void   `$INSTANCE_NAME`_DisableRx(void)         `=ReentrantKeil($INSTANCE_NAME . "_DisableRx")`;
    void   `$INSTANCE_NAME`_SetRxInterruptMode(uint8 interruptSource) \
                                                    `=ReentrantKeil($INSTANCE_NAME . "_SetRxInterruptMode")`;
    uint8  `$INSTANCE_NAME`_ReadRxStatus(void)      `=ReentrantKeil($INSTANCE_NAME . "_ReadRxStatus")`;
    uint8  `$INSTANCE_NAME`_ReadByte(uint8 wordSelect) \
                                                    `=ReentrantKeil($INSTANCE_NAME . "_ReadByte")`;
    void   `$INSTANCE_NAME`_ClearRxFIFO(void)       `=ReentrantKeil($INSTANCE_NAME . "_ClearRxFIFO")`;
#endif    /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */


/***************************************
*           API Constants               
***************************************/

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_LEFT_CHANNEL           (0x00u)
    #define `$INSTANCE_NAME`_TX_RIGHT_CHANNEL          (0x01u)
    
    #define `$INSTANCE_NAME`_TX_FIFO_UNDERFLOW         (0x01u)
    #define `$INSTANCE_NAME`_TX_FIFO_0_NOT_FULL        (0x02u)
    #define `$INSTANCE_NAME`_TX_FIFO_1_NOT_FULL        (0x04u)
    
    #define `$INSTANCE_NAME`_RET_TX_FIFO_UNDERFLOW     (0x01u)
    #define `$INSTANCE_NAME`_RET_TX_FIFO_0_NOT_FULL    (0x02u)
    #define `$INSTANCE_NAME`_RET_TX_FIFO_1_NOT_FULL    (0x04u)
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    #define `$INSTANCE_NAME`_RX_FIFO_OVERFLOW          (0x01u)
    #define `$INSTANCE_NAME`_RX_FIFO_0_NOT_EMPTY       (0x02u)
    #define `$INSTANCE_NAME`_RX_FIFO_1_NOT_EMPTY       (0x04u)

    #define `$INSTANCE_NAME`_RET_RX_FIFO_OVERFLOW      (0x01u)
    #define `$INSTANCE_NAME`_RET_RX_FIFO_0_NOT_EMPTY   (0x02u)
    #define `$INSTANCE_NAME`_RET_RX_FIFO_1_NOT_EMPTY   (0x04u)
    #define `$INSTANCE_NAME`_RX_LEFT_CHANNEL           (0x00u) 
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */

#define `$INSTANCE_NAME`_WORD_SEL_MASK                 (0x01u)
#define `$INSTANCE_NAME`_DISABLED                      (0x00u)


/***************************************
*    Enumerated Types and Parameters    
***************************************/

`#declare_enum I2S_DataInterleaving` 


/***************************************
*    Initial Parameter Constants        
***************************************/

/* Default interrupt source */
#define `$INSTANCE_NAME`_DEFAULT_INT_SOURCE (`$InterruptSource`u)

/* Word Select period. Set as (period * 2) - 1 because of 2X input clock */
#define `$INSTANCE_NAME`_DEFAULT_WS_PERIOD  ((`$WordSelect`u << 1u) - 1u) 


/***************************************
*             Registers                 
***************************************/

#define `$INSTANCE_NAME`_CONTROL_REG   (* (reg8 *) `$INSTANCE_NAME`_`$CtlModeReplacementString`_ControlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR   (  (reg8 *) `$INSTANCE_NAME`_`$CtlModeReplacementString`_ControlReg__CONTROL_REG)

#define `$INSTANCE_NAME`_AUX_CONTROL_REG                (* (reg8 *) `$INSTANCE_NAME`_BitCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_AUX_CONTROL_PTR                (  (reg8 *) `$INSTANCE_NAME`_BitCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_CNT7_PERIOD_REG                (* (reg8 *) `$INSTANCE_NAME`_BitCounter__PERIOD_REG)
#define `$INSTANCE_NAME`_CNT7_PERIOD_PTR                (  (reg8 *) `$INSTANCE_NAME`_BitCounter__PERIOD_REG)

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_REG                     (* (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__A0_REG)
    #define `$INSTANCE_NAME`_TX_PTR                     (  (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__A0_REG)

    #define `$INSTANCE_NAME`_TX_FIFO_0_REG              (* (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__F0_REG)
    #define `$INSTANCE_NAME`_TX_FIFO_0_PTR              (  (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__F0_REG)
    #define `$INSTANCE_NAME`_TX_FIFO_1_REG              (* (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__F1_REG)
    #define `$INSTANCE_NAME`_TX_FIFO_1_PTR              (  (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__F1_REG)

    #define `$INSTANCE_NAME`_TX_AUX_CONTROL_REG         (* (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_TX_AUX_CONTROL_PTR         (  (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__DP_AUX_CTL_REG)
    
    #define `$INSTANCE_NAME`_TX_STATUS_REG              (* (reg8 *) `$INSTANCE_NAME`_Tx_TxStsReg__STATUS_REG)
    #define `$INSTANCE_NAME`_TX_STATUS_PTR              (  (reg8 *) `$INSTANCE_NAME`_Tx_TxStsReg__STATUS_REG)
    #define `$INSTANCE_NAME`_TX_STATUS_MASK_REG         (* (reg8 *) `$INSTANCE_NAME`_Tx_TxStsReg__MASK_REG)
    #define `$INSTANCE_NAME`_TX_STATUS_MASK_PTR         (  (reg8 *) `$INSTANCE_NAME`_Tx_TxStsReg__MASK_REG)
    #define `$INSTANCE_NAME`_TX_STATUS_AUX_CONTROL_REG  (* (reg8 *) `$INSTANCE_NAME`_Tx_TxStsReg__STATUS_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_TX_STATUS_AUX_CONTROL_PTR  (  (reg8 *) `$INSTANCE_NAME`_Tx_TxStsReg__STATUS_AUX_CTL_REG)

#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    #define `$INSTANCE_NAME`_RX_REG                     (* (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__A0_REG)
    #define `$INSTANCE_NAME`_RX_PTR                     (  (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__A0_REG)
    #define `$INSTANCE_NAME`_RX_FIFO_0_REG              (* (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__F0_REG)
    #define `$INSTANCE_NAME`_RX_FIFO_0_PTR              (  (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__F0_REG)
    #define `$INSTANCE_NAME`_RX_FIFO_1_REG              (* (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__F1_REG)
    #define `$INSTANCE_NAME`_RX_FIFO_1_PTR              (  (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__F1_REG)
    
    #define `$INSTANCE_NAME`_RX_AUX_CONTROL_REG         (* (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_RX_AUX_CONTROL_PTR         (  (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__DP_AUX_CTL_REG)

    #define `$INSTANCE_NAME`_RX_STATUS_REG              (* (reg8 *) `$INSTANCE_NAME`_Rx_RxStsReg__STATUS_REG)
    #define `$INSTANCE_NAME`_RX_STATUS_PTR              (  (reg8 *) `$INSTANCE_NAME`_Rx_RxStsReg__STATUS_REG)
    #define `$INSTANCE_NAME`_RX_STATUS_MASK_REG         (* (reg8 *) `$INSTANCE_NAME`_Rx_RxStsReg__MASK_REG)
    #define `$INSTANCE_NAME`_RX_STATUS_MASK_PTR         (  (reg8 *) `$INSTANCE_NAME`_Rx_RxStsReg__MASK_REG)
    
    #define `$INSTANCE_NAME`_RX_STATUS_AUX_CONTROL_REG  (* (reg8 *) `$INSTANCE_NAME`_Rx_RxStsReg__STATUS_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_RX_STATUS_AUX_CONTROL_PTR  (  (reg8 *) `$INSTANCE_NAME`_Rx_RxStsReg__STATUS_AUX_CTL_REG)
    
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */


/***************************************
*       Register Constants              
***************************************/

#define `$INSTANCE_NAME`_TX_EN                      (0x01u)
#define `$INSTANCE_NAME`_RX_EN                      (0x02u)
#define `$INSTANCE_NAME`_EN                         (0x04u)

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_ST_MASK             (0x07u)
    
    #define `$INSTANCE_NAME`_TX_FIFO_0_CLR          (0x01u)
    #define `$INSTANCE_NAME`_TX_FIFO_1_CLR          (0x02u) 
    
    #define `$INSTANCE_NAME`_TX_INT_EN              (0x10u)
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || \
    (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_RX_ST_MASK             (0x07u)
    
    #define `$INSTANCE_NAME`_RX_FIFO_0_CLR          (0x01u)
    #define `$INSTANCE_NAME`_RX_FIFO_1_CLR          (0x02u)
    
    #define `$INSTANCE_NAME`_RX_INT_EN              (0x10u)
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */

#define `$INSTANCE_NAME`_CNTR7_EN                   (0x20u)

#endif /* CY_I2S_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
