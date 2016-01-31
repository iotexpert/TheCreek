/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values and API definition for the 
*  SPDIF Component
*
* Note:
*
********************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(CY_SPDIFTX_`$INSTANCE_NAME`_H)
#define CY_SPDIFTX_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"

/* Check to see if required defines such as CY_PSOC3 and CY_PSOC5 are available */
/* They are defined starting with cy_boot v2.30 */
#ifndef CY_PSOC3
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v2.30 or later
#endif
                                 
/* Determines if the channel status DMA is managed by the component */
#define `$INSTANCE_NAME`_MANAGED_DMA (`$ManagedChannelStatus`u)                                      

#if (0u != `$INSTANCE_NAME`_MANAGED_DMA)

    #include "`$INSTANCE_NAME`_Cst0_DMA_dma.h"
    #include "`$INSTANCE_NAME`_Cst1_DMA_dma.h"
    
    #define `$INSTANCE_NAME`_CST_DMA_BYTES_PER_BURST       (1u)
    #define `$INSTANCE_NAME`_CST_DMA_REQUEST_PER_BURST     (1u)
    
    #define `$INSTANCE_NAME`_CST_DMA_SRC_BASE (CYDEV_SRAM_BASE)
    #define `$INSTANCE_NAME`_CST_DMA_DST_BASE (CYDEV_PERIPH_BASE)
    
    #ifdef `$INSTANCE_NAME`_Cst0Interrupt__ES2_PATCH
        #if(CY_PSOC3_ES2 && `$INSTANCE_NAME`_Cst0Interrupt__ES2_PATCH)
            #include <intrins.h>
            #define `$INSTANCE_NAME`_Cst0Copy_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); \
                                                      _nop_(); _nop_(); _nop_(); _nop_();
        #endif /* End PSOC3_ES2 */
    #endif /* `$INSTANCE_NAME`_Cst0Interrupt__ES2_PATCH */

    #ifdef `$INSTANCE_NAME`_Cst1Interrupt__ES2_PATCH
        #if(CY_PSOC3_ES2 && `$INSTANCE_NAME`_Cst1Interrupt__ES2_PATCH)
            #include <intrins.h>
            #define `$INSTANCE_NAME`_Cst1Copy_PATCH() _nop_(); _nop_(); _nop_(); _nop_(); \
                                                      _nop_(); _nop_(); _nop_(); _nop_();
        #endif /* End PSOC3_ES2 */
    #endif /* `$INSTANCE_NAME`_Cst1Interrupt__ES2_PATCH */

#endif /* (0u != `$INSTANCE_NAME`_MANAGED_DMA) */


/*************************************** 
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_DATA_INTERLEAVING   (`$InterleavedFifo`u)


/***************************************
*     Data Struct Definitions
***************************************/

/* Low power Mode API Support */
typedef struct _`$INSTANCE_NAME`_backupStruct
{
    uint8 enableState;
    #if(CY_PSOC3_ES2 || CY_PSOC5_ES1)
        uint8 interruptMask;
    #endif /* CY_PSOC3_ES2 || CY_PSOC5_ES1 */
} `$INSTANCE_NAME`_BACKUP_STRUCT;


/***************************************
*        Function Prototypes            
***************************************/

void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Init(void);
void  `$INSTANCE_NAME`_Enable(void);
void  `$INSTANCE_NAME`_Stop(void);                   
void  `$INSTANCE_NAME`_Sleep(void);                  
void  `$INSTANCE_NAME`_Wakeup(void);
void  `$INSTANCE_NAME`_SaveConfig(void);
void  `$INSTANCE_NAME`_RestoreConfig(void)                       `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void  `$INSTANCE_NAME`_EnableTx(void)                            `=ReentrantKeil($INSTANCE_NAME . "_EnableTx")`;
void  `$INSTANCE_NAME`_DisableTx(void)                           `=ReentrantKeil($INSTANCE_NAME . "_DisableTx")`;
void  `$INSTANCE_NAME`_WriteTxByte(uint8 wrData, uint8 channelSelect);
void  `$INSTANCE_NAME`_WriteCstByte(uint8 wrData, uint8 channelSelect);
void  `$INSTANCE_NAME`_ClearTxFIFO(void)                         `=ReentrantKeil($INSTANCE_NAME . "_ClearTxFIFO")`;    
void  `$INSTANCE_NAME`_ClearCstFIFO(void)                        `=ReentrantKeil($INSTANCE_NAME . "_ClearCstFIFO")`;  
uint8 `$INSTANCE_NAME`_ReadStatus(void)                          `=ReentrantKeil($INSTANCE_NAME . "_ReadTxStatus")`;  
void  `$INSTANCE_NAME`_SetInterruptMode(uint8 interruptSource)   `=ReentrantKeil($INSTANCE_NAME . "_SetInterruptMode")`;
#if(0u != `$INSTANCE_NAME`_MANAGED_DMA) 
    void  `$INSTANCE_NAME`_SetChannelStatus(uint8 channel, uint8 byte, uint8 mask, uint8 value);    
    uint8 `$INSTANCE_NAME`_SetFrequency(uint8 frequency);
    CY_ISR_PROTO(`$INSTANCE_NAME`_Cst0Copy);
    CY_ISR_PROTO(`$INSTANCE_NAME`_Cst1Copy);
#endif /* (0u != `$INSTANCE_NAME`_MANAGED_DMA) */


/***************************************
*           API Constants               
***************************************/

/* Channel Name Constants */
#define `$INSTANCE_NAME`_CHANNEL_0      (0u)
#define `$INSTANCE_NAME`_CHANNEL_1      (1u)

/* Frequency constants */
#define `$INSTANCE_NAME`_SPS_UNKNOWN    (0x01u)
#define `$INSTANCE_NAME`_SPS_22KHZ      (0x04u)
#define `$INSTANCE_NAME`_SPS_24KHZ      (0x06u)
#define `$INSTANCE_NAME`_SPS_32KHZ      (0x03u)
#define `$INSTANCE_NAME`_SPS_44KHZ      (0x00u)
#define `$INSTANCE_NAME`_SPS_48KHZ      (0x02u)
#define `$INSTANCE_NAME`_SPS_64KHZ      (0x0Bu)
#define `$INSTANCE_NAME`_SPS_88KHZ      (0x08u)
#define `$INSTANCE_NAME`_SPS_96KHZ      (0x0Au)
#define `$INSTANCE_NAME`_SPS_192KHZ     (0x0Eu)

/* Channel Status Macros */
/* Data Type */
#define `$INSTANCE_NAME`_DATA_TYPE_LINEAR_PCM      (0u), (0x02u), (0x00u)
#define `$INSTANCE_NAME`_DATA_TYPE_OTHERDATA       (0u), (0x02u), (0x02u)
/* Copyright */
#define `$INSTANCE_NAME`_COPY_HAS_CP_RIGHT         (0u), (0x04u), (0x00u)
#define `$INSTANCE_NAME`_COPY_NO_CP_RIGHT          (0u), (0x04u), (0x04u)
/* PCM Pre-emphasis */
#define `$INSTANCE_NAME`_PREEMP_NO_PREEMP          (0u), (0x38u), (0x00u)
#define `$INSTANCE_NAME`_PREEMP_PREEMP50           (0u), (0x38u), (0x08u)
/* Category */
#define `$INSTANCE_NAME`_CAT_GEN                   (1u), (0xFFu), (0x00u)
#define `$INSTANCE_NAME`_CAT_D2D                   (1u), (0xFFu), (0x02u)
/* Source Number */
#define `$INSTANCE_NAME`_SRC_NUM00                 (2u), (0x0Fu), (0x00u)
#define `$INSTANCE_NAME`_SRC_NUM01                 (2u), (0x0Fu), (0x01u)
#define `$INSTANCE_NAME`_SRC_NUM02                 (2u), (0x0Fu), (0x02u)
#define `$INSTANCE_NAME`_SRC_NUM03                 (2u), (0x0Fu), (0x03u)
#define `$INSTANCE_NAME`_SRC_NUM04                 (2u), (0x0Fu), (0x04u)
#define `$INSTANCE_NAME`_SRC_NUM05                 (2u), (0x0Fu), (0x05u)
#define `$INSTANCE_NAME`_SRC_NUM06                 (2u), (0x0Fu), (0x06u)
#define `$INSTANCE_NAME`_SRC_NUM07                 (2u), (0x0Fu), (0x07u)
#define `$INSTANCE_NAME`_SRC_NUM08                 (2u), (0x0Fu), (0x08u)
#define `$INSTANCE_NAME`_SRC_NUM09                 (2u), (0x0Fu), (0x09u)
#define `$INSTANCE_NAME`_SRC_NUM10                 (2u), (0x0Fu), (0x0Au)
#define `$INSTANCE_NAME`_SRC_NUM11                 (2u), (0x0Fu), (0x0Bu)
#define `$INSTANCE_NAME`_SRC_NUM12                 (2u), (0x0Fu), (0x0Cu)
#define `$INSTANCE_NAME`_SRC_NUM13                 (2u), (0x0Fu), (0x0Du)
#define `$INSTANCE_NAME`_SRC_NUM14                 (2u), (0x0Fu), (0x0Eu)
#define `$INSTANCE_NAME`_SRC_NUM15                 (2u), (0x0Fu), (0x0Fu)
/* Channel Number */
#define `$INSTANCE_NAME`_CH_NUM00                  (2u), (0xF0u), (0x00u)
#define `$INSTANCE_NAME`_CH_NUM01                  (2u), (0xF0u), (0x10u)
#define `$INSTANCE_NAME`_CH_NUM02                  (2u), (0xF0u), (0x20u)
#define `$INSTANCE_NAME`_CH_NUM03                  (2u), (0xF0u), (0x30u)
#define `$INSTANCE_NAME`_CH_NUM04                  (2u), (0xF0u), (0x40u)
#define `$INSTANCE_NAME`_CH_NUM05                  (2u), (0xF0u), (0x50u)
#define `$INSTANCE_NAME`_CH_NUM06                  (2u), (0xF0u), (0x60u)
#define `$INSTANCE_NAME`_CH_NUM07                  (2u), (0xF0u), (0x70u)
#define `$INSTANCE_NAME`_CH_NUM08                  (2u), (0xF0u), (0x80u)
#define `$INSTANCE_NAME`_CH_NUM09                  (2u), (0xF0u), (0x90u)
#define `$INSTANCE_NAME`_CH_NUM10                  (2u), (0xF0u), (0xA0u)
#define `$INSTANCE_NAME`_CH_NUM11                  (2u), (0xF0u), (0xB0u)
#define `$INSTANCE_NAME`_CH_NUM12                  (2u), (0xF0u), (0xC0u)
#define `$INSTANCE_NAME`_CH_NUM13                  (2u), (0xF0u), (0xD0u)
#define `$INSTANCE_NAME`_CH_NUM14                  (2u), (0xF0u), (0xE0u)
#define `$INSTANCE_NAME`_CH_NUM15                  (2u), (0xF0u), (0xF0u)
/* Clock Accuracy */
#define `$INSTANCE_NAME`_CLKLVL_1                  (3u), (0x30u), (0x10u)
#define `$INSTANCE_NAME`_CLKLVL_2                  (3u), (0x30u), (0x00u)
#define `$INSTANCE_NAME`_CLKLVL_3                  (3u), (0x30u), (0x20u)
/* Audio Max Word Length */
#define `$INSTANCE_NAME`_STDLEN                    (4u), (0x0Fu), (0x00u)
#define `$INSTANCE_NAME`_24BLEN                    (4u), (0x0Fu), (0x0Bu)
/* Sample Frequency */
#define `$INSTANCE_NAME`_SF_022KHZ                 (3u), (0xCFu), (0x04u)
#define `$INSTANCE_NAME`_SF_024KHZ                 (3u), (0xCFu), (0x06u)
#define `$INSTANCE_NAME`_SF_032KHZ                 (3u), (0xCFu), (0x03u)
#define `$INSTANCE_NAME`_SF_044KHZ                 (3u), (0xCFu), (0x00u)
#define `$INSTANCE_NAME`_SF_048KHZ                 (3u), (0xCFu), (0x02u)
#define `$INSTANCE_NAME`_SF_064KHZ                 (3u), (0xCFu), (0x0Bu)
#define `$INSTANCE_NAME`_SF_088KHZ                 (3u), (0xCFu), (0x08u)
#define `$INSTANCE_NAME`_SF_096KHZ                 (3u), (0xCFu), (0x0Au)
#define `$INSTANCE_NAME`_SF_192KHZ                 (3u), (0xCFu), (0x0Eu)
#define `$INSTANCE_NAME`_SF_NOT_IND                (3u), (0xCFu), (0x01u)

/* Status Masks */  
#define `$INSTANCE_NAME`_AUDIO_FIFO_UNDERFLOW       (0x01u)
#define `$INSTANCE_NAME`_AUDIO_0_FIFO_NOT_FULL      (0x02u)
#define `$INSTANCE_NAME`_AUDIO_1_FIFO_NOT_FULL      (0x04u)
#define `$INSTANCE_NAME`_CHST_FIFO_UNDERFLOW        (0x08u)
#define `$INSTANCE_NAME`_CHST_0_FIFO_NOT_FULL       (0x10u)
#define `$INSTANCE_NAME`_CHST_1_FIFO_NOT_FULL       (0x20u)
#define `$INSTANCE_NAME`_CHST_FIFOS_NOT_FULL        (0x30u)

/* Preamble Patterns */
#define `$INSTANCE_NAME`_PREAMBLE_X_PATTERN         (0xE2u)
#define `$INSTANCE_NAME`_PREAMBLE_Y_PATTERN         (0xE4u)
#define `$INSTANCE_NAME`_PREAMBLE_Z_PATTERN         (0xE8u)

/* Preamble and Post Data Period. Set 2 less than doubled value of bits in 
* corresponding interval. The period is doubled because of 2X clocking, but 
* there is one extra cycle for the load and another extra cycle since the count 
* is from N to 0 (instead of 1). */
#define `$INSTANCE_NAME`_PRE_POST_PERIOD            (6u)

/* Bit Counter Period. Set one less than doubled value of frame length. */
#define `$INSTANCE_NAME`_FRAME_PERIOD               (0x7Fu)

/* Audio Data Length. Set 2 less than doubled value of bits in corresponding
* interval. The period is doubled because of 2X clocking, but there is one extra
* cycle for the load and another extra cycle since the count is from N to 0 
* (instead of 1). */
#define `$INSTANCE_NAME`_AUDIO_DATA_PERIOD          (46u)

/* Period of block. S/PDIF block consist of 192 frames */
#define `$INSTANCE_NAME`_BLOCK_PERIOD               (192u)

/* Channel status block lenght */
#define `$INSTANCE_NAME`_CST_LENGTH                 (24u)
#define `$INSTANCE_NAME`_CST_HALF_LENGTH            (12u)

/* Default interrupt source */
#define `$INSTANCE_NAME`_DEFAULT_INT_SRC            (`$INSTANCE_NAME`_AUDIO_FIFO_UNDERFLOW | \
                                                     `$INSTANCE_NAME`_CHST_FIFO_UNDERFLOW)    

#define `$INSTANCE_NAME`_DISABLED                   (0x00u)

#define `$INSTANCE_NAME`_CST_0_ISR_NUMBER           (`$INSTANCE_NAME`_Cst0Interrupt__INTC_NUMBER)
#define `$INSTANCE_NAME`_CST_1_ISR_NUMBER           (`$INSTANCE_NAME`_Cst1Interrupt__INTC_NUMBER)
#define `$INSTANCE_NAME`_CST_0_ISR_PRIORITY         (`$INSTANCE_NAME`_Cst0Interrupt__INTC_PRIOR_NUM)
#define `$INSTANCE_NAME`_CST_1_ISR_PRIORITY         (`$INSTANCE_NAME`_Cst1Interrupt__INTC_PRIOR_NUM)


/***************************************
*    Enumerated Types and Parameters    
***************************************/


/***************************************
*    Initial Parameter Constants        
***************************************/


/***************************************
*             Registers                 
***************************************/

/* Control register definition */
#define `$INSTANCE_NAME`_CONTROL_REG \
                            (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_`$CtlModeReplacementString`_ControlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR \
                            (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_`$CtlModeReplacementString`_ControlReg__CONTROL_REG)

/* Status register definition */
#define `$INSTANCE_NAME`_STATUS_REG              (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_StatusReg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_PTR              (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_StatusReg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_MASK_REG         (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_StatusReg__MASK_REG)
#define `$INSTANCE_NAME`_STATUS_MASK_PTR         (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_StatusReg__MASK_REG)
#define `$INSTANCE_NAME`_STATUS_AUX_CTL_REG      (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_StatusReg__STATUS_AUX_CTL_REG)
#define `$INSTANCE_NAME`_STATUS_AUX_CTL_PTR      (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_StatusReg__STATUS_AUX_CTL_REG)

/* Bit counter control and period registers definition */ 
#define `$INSTANCE_NAME`_BCNT_AUX_CTL_REG        (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_BitCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_BCNT_AUX_CTL_PTR        (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_BitCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_BCNT_PERIOD_REG         (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_BitCounter__PERIOD_REG)
#define `$INSTANCE_NAME`_BCNT_PERIOD_PTR         (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_BitCounter__PERIOD_REG)

/* Frame Counter Control and Period Registers */
#define `$INSTANCE_NAME`_FCNT_AUX_CTL_REG        (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_FCNT_AUX_CTL_PTR        (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_FCNT_PRE_POST_REG       (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__F0_REG)
#define `$INSTANCE_NAME`_FCNT_PRE_POST_PTR       (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__F0_REG)
#define `$INSTANCE_NAME`_FCNT_AUDIO_LENGTH_REG   (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__D0_REG)
#define `$INSTANCE_NAME`_FCNT_AUDIO_LENGTH_PTR   (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__D0_REG)
#define `$INSTANCE_NAME`_FCNT_BLOCK_PERIOD_REG   (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__F1_REG)
#define `$INSTANCE_NAME`_FCNT_BLOCK_PERIOD_PTR   (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_FrameCounter_u0__F1_REG)
#define `$INSTANCE_NAME`_FRAME_PERIOD_REG        (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_BitCounter__PERIOD_REG)
#define `$INSTANCE_NAME`_FRAME_PERIOD_PTR        (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_BitCounter__PERIOD_REG)

/* Preamble Generator Control and Pattern Registers */
#define `$INSTANCE_NAME`_PREGEN_AUX_CTL_REG       (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_PREGEN_AUX_CTL_PTR       (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_PREGEN_PREZ_PTRN_REG     (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__F0_REG)
#define `$INSTANCE_NAME`_PREGEN_PREZ_PTRN_PTR     (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__F0_REG)
#define `$INSTANCE_NAME`_PREGEN_PREX_PTRN_REG     (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__D0_REG)
#define `$INSTANCE_NAME`_PREGEN_PREX_PTRN_PTR     (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__D0_REG)
#define `$INSTANCE_NAME`_PREGEN_PREY_PTRN_REG     (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__D1_REG)
#define `$INSTANCE_NAME`_PREGEN_PREY_PTRN_PTR     (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_PreambleGen_u0__D1_REG)

/* Audio Data Transmitter Control and Data Registers */
#define `$INSTANCE_NAME`_TX_AUX_CTL_REG           (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_AudioTx_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_TX_AUX_CTL_PTR           (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_AudioTx_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_TX_FIFO_0_REG            (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_AudioTx_u0__F0_REG)
#define `$INSTANCE_NAME`_TX_FIFO_0_PTR            (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_AudioTx_u0__F0_REG)
#define `$INSTANCE_NAME`_TX_FIFO_1_REG            (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_AudioTx_u0__F1_REG)
#define `$INSTANCE_NAME`_TX_FIFO_1_PTR            (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_AudioTx_u0__F1_REG)

/* Channel Status Generator Control and Data Registers */
#define `$INSTANCE_NAME`_CST_AUX_CTL_REG          (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_ChStatusGen_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_CST_AUX_CTL_PTR          (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_ChStatusGen_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_CST_FIFO_0_REG           (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_ChStatusGen_u0__F0_REG)
#define `$INSTANCE_NAME`_CST_FIFO_0_PTR           (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_ChStatusGen_u0__F0_REG)
#define `$INSTANCE_NAME`_CST_FIFO_1_REG           (* (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_ChStatusGen_u0__F1_REG)
#define `$INSTANCE_NAME`_CST_FIFO_1_PTR           (  (reg8 *) `$INSTANCE_NAME`_bSPDIF_Tx_ChStatusGen_u0__F1_REG)


/***************************************
*       Register Constants              
***************************************/

/* Audio data transmission enabling */
#define `$INSTANCE_NAME`_TX_EN                      (0x01u)
/* Preamble and status transmission enabling */
#define `$INSTANCE_NAME`_ENBL                       (0x02u)
/* Bit counter enabling */
#define `$INSTANCE_NAME`_BCNT_EN                    (0x20u) 
/* FIFOs clear bit masks */
#define `$INSTANCE_NAME`_F0_CLEAR                   (0x01u)
#define `$INSTANCE_NAME`_F1_CLEAR                   (0x02u)
#define `$INSTANCE_NAME`_FX_CLEAR                   (0x03u)
/* Interrupt mask and enabling */
#define `$INSTANCE_NAME`_INT_MASK                   (0x3Fu)
#define `$INSTANCE_NAME`_INT_EN                     (0x10u)

#endif /* CY_SPDIFTX_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
