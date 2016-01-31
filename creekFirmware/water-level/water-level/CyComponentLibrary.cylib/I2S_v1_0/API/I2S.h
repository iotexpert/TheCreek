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
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(CY_I2S_`$INSTANCE_NAME`_H)
#define CY_I2S_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice_trm.h"


/*************************************** 
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_DIRECTION `$Direction`

`#DECLARE_ENUM I2S_Direction`
`#DECLARE_ENUM I2S_DataInterleaving` 

/***************************************
*        Function Prototypes            
***************************************/

void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Stop(void);

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
void   `$INSTANCE_NAME`_EnableTx(void);
void   `$INSTANCE_NAME`_DisableTx(void);
void   `$INSTANCE_NAME`_WriteByte(uint8 wrData, uint8 wordSelect);
void   `$INSTANCE_NAME`_ClearTxFIFO(void);
void   `$INSTANCE_NAME`_SetTxInterruptMode(uint8 interruptSource);
uint8  `$INSTANCE_NAME`_ReadTxStatus(void);
#endif    /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */ 

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
void   `$INSTANCE_NAME`_EnableRx(void);
void   `$INSTANCE_NAME`_DisableRx(void);
void   `$INSTANCE_NAME`_SetRxInterruptMode(uint8 interruptSource);
uint8  `$INSTANCE_NAME`_ReadRxStatus(void);
uint8  `$INSTANCE_NAME`_ReadByte(uint8 wordSelect);
void   `$INSTANCE_NAME`_ClearRxFIFO(void);
#endif    /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */


/***************************************
*           API Constants               
***************************************/

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_LEFT_CHANNEL          0x00U
    #define `$INSTANCE_NAME`_TX_RIGHT_CHANNEL         0x01U
    
    #define `$INSTANCE_NAME`_TX_FIFO_UNDEFLOW          0x01U
    #define `$INSTANCE_NAME`_TX_FIFO_0_NOT_FULL        0x02U
    #define `$INSTANCE_NAME`_TX_FIFO_1_NOT_FULL        0x04U
    
    #define `$INSTANCE_NAME`_RET_TX_FIFO_UNDEFLOW      0x01U
    #define `$INSTANCE_NAME`_RET_TX_FIFO_0_NOT_FULL    0x02U
    #define `$INSTANCE_NAME`_RET_TX_FIFO_1_NOT_FULL    0x04U
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    #define `$INSTANCE_NAME`_RX_FIFO_OVERFLOW          0x01U
    #define `$INSTANCE_NAME`_RX_FIFO_0_NOT_EMPTY       0x02U
    #define `$INSTANCE_NAME`_RX_FIFO_1_NOT_EMPTY       0x04U

    #define `$INSTANCE_NAME`_RET_RX_FIFO_OVERFLOW      0x01U
    #define `$INSTANCE_NAME`_RET_RX_FIFO_0_NOT_EMPTY   0x02U
    #define `$INSTANCE_NAME`_RET_RX_FIFO_1_NOT_EMPTY   0x04U
    #define `$INSTANCE_NAME`_RX_LEFT_CHANNEL           0x00U 
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */

#define `$INSTANCE_NAME`_WORD_SEL_MASK              0x01U


/***************************************
*    Enumerated Types and Parameters    
***************************************/

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_DATA_INTERLEAVING   `$TxDataInterleaving`
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    #define `$INSTANCE_NAME`_RX_DATA_INTERLEAVING   `$RxDataInterleaving`
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */


/***************************************
*    Initial Parameter Constants        
***************************************/


/***************************************
*             Registers                 
***************************************/

#define `$INSTANCE_NAME`_CONTROL                    (* (reg8 *) `$INSTANCE_NAME`_ControlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR                ((reg8 *) `$INSTANCE_NAME`_ControlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_AUX_CONTROL                (* (reg8 *) `$INSTANCE_NAME`_BitCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_AUX_CONTROL_PTR            ((reg8 *) `$INSTANCE_NAME`_BitCounter__CONTROL_AUX_CTL_REG)

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_PTR                 ((reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__A0_REG)
    #define `$INSTANCE_NAME`_TX_FIFO_0_PTR          ((reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__F0_REG)
    #define `$INSTANCE_NAME`_TX_FIFO_1_PTR          ((reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__F1_REG)

    #define `$INSTANCE_NAME`_TX_AUX_CONTROL         (* (reg8 *) `$INSTANCE_NAME`_Tx_dpTx_u0__DP_AUX_CTL_REG)

    #define `$INSTANCE_NAME`_TX_STATUS              (* (reg8 *) `$INSTANCE_NAME`_Tx_tx_sts_reg__STATUS_REG)
    #define `$INSTANCE_NAME`_TX_STATUS_MASK         (* (reg8 *) `$INSTANCE_NAME`_Tx_tx_sts_reg__MASK_REG)
    #define `$INSTANCE_NAME`_TX_STATUS_AUX_CONTROL  (* (reg8 *) `$INSTANCE_NAME`_Tx_tx_sts_reg__STATUS_AUX_CTL_REG)

#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX)) 
    #define `$INSTANCE_NAME`_RX_PTR                 ((reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__A0_REG)
    #define `$INSTANCE_NAME`_RX_FIFO_0_PTR          ((reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__F0_REG)
    #define `$INSTANCE_NAME`_RX_FIFO_1_PTR          ((reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__F1_REG)
    
    #define `$INSTANCE_NAME`_RX_AUX_CONTROL         (* (reg8 *) `$INSTANCE_NAME`_Rx_dpRx_u0__DP_AUX_CTL_REG)

    #define `$INSTANCE_NAME`_RX_STATUS              (* (reg8 *) `$INSTANCE_NAME`_Rx_rx_sts_reg__STATUS_REG) 
    #define `$INSTANCE_NAME`_RX_STATUS_MASK         (* (reg8 *) `$INSTANCE_NAME`_Rx_rx_sts_reg__MASK_REG)
    
    #define `$INSTANCE_NAME`_RX_STATUS_AUX_CONTROL  (* (reg8 *) `$INSTANCE_NAME`_Rx_rx_sts_reg__STATUS_AUX_CTL_REG)
    
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */


/***************************************
*       Register Constants              
***************************************/

#define `$INSTANCE_NAME`_TX_EN                      0x01U
#define `$INSTANCE_NAME`_RX_EN                      0x02U
#define `$INSTANCE_NAME`_CNTR_LD                    0x04U
#define `$INSTANCE_NAME`_RST                        0x08U
#define `$INSTANCE_NAME`_CNTR_EN                    0x10U

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_TX_ST_MASK             0x07U
    
    #define `$INSTANCE_NAME`_TX_FIFO_0_CLR          0x01U
    #define `$INSTANCE_NAME`_TX_FIFO_1_CLR          0x02U 
    
    #define `$INSTANCE_NAME`_TX_INT_EN              0x10U
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__TX */

#if ((`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX) || (`$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX_AND_TX))
    #define `$INSTANCE_NAME`_RX_ST_MASK             0x07U
    
    #define `$INSTANCE_NAME`_RX_FIFO_0_CLR          0x01U
    #define `$INSTANCE_NAME`_RX_FIFO_1_CLR          0x02U
    #define `$INSTANCE_NAME`_RX_INT_EN              0x10U
#endif /* `$INSTANCE_NAME`_DIRECTION == `$INSTANCE_NAME`__RX */

#define `$INSTANCE_NAME`_CNTR7_EN                   0x20U

#endif /* CY_I2S_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
