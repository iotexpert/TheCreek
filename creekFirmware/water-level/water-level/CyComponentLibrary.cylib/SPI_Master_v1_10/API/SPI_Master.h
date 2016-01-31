/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* Contains the function prototypes and constants available to the SPI Master
*     user module.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "cyfitter.h"

#define `$INSTANCE_NAME`_DATAWIDTH      `$NumberOfDataBits`
#define `$INSTANCE_NAME`_InternalClockUsed   `$InternalClockUsed`
#define `$INSTANCE_NAME`_InternalInterruptEnabled `$InternalInterruptEnabled`
#define `$INSTANCE_NAME`_ModeUseZero    `$ModeUseZero`

#if (`$INSTANCE_NAME`_InternalInterruptEnabled)
#include "`$INSTANCE_NAME`_InternalInterrupt.h"
#endif

#if(`$INSTANCE_NAME`_InternalClockUsed)
#include "`$INSTANCE_NAME`_IntClock.h"
#endif


#if !defined(CY_SPIM_v1_10_`$INSTANCE_NAME`_H)
#define CY_SPIM_v1_10_`$INSTANCE_NAME`_H

/* Use the Available Enumerated Types */

/***************************************
 *   Function Prototypes
 **************************************/
void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Stop(void);
void  `$INSTANCE_NAME`_EnableInt(void);
void  `$INSTANCE_NAME`_DisableInt(void);
void  `$INSTANCE_NAME`_SetInterruptMode(uint8 intSource);
uint8 `$INSTANCE_NAME`_ReadStatus(void);
void  `$INSTANCE_NAME`_WriteByte(`$RegSizeReplacementString` txDataByte);
`$RegSizeReplacementString` `$INSTANCE_NAME`_ReadByte(void);
/* 8-bit version */
uint8 `$INSTANCE_NAME`_GetRxBufferSize(void);
/* 32-bit version */
//uint16 `$INSTANCE_NAME`_GetRxBufferSize(void);
/* 8-bit version */
uint8  `$INSTANCE_NAME`_GetTxBufferSize(void);
/* 32-bit version */
//uint16  `$INSTANCE_NAME`_GetTxBufferSize(void);
void  `$INSTANCE_NAME`_ClearRxBuffer(void);
void  `$INSTANCE_NAME`_ClearTxBuffer(void);
void  `$INSTANCE_NAME`_TxEnable(void);
void  `$INSTANCE_NAME`_TxDisable(void);
/* 8-bit version */
void  `$INSTANCE_NAME`_PutArray(uint16 *buffer, uint8 byteCount);
/* 32-bit version */
//void `$INSTANCE_NAME`_PutArray(uint32 *buffer, uint16 byteCount);
void `$INSTANCE_NAME`_ClearFIFO(void);

CY_ISR_PROTO(`$INSTANCE_NAME`_ISR);

/***********************************
*     Constants
***********************************/
/* Status Register Definitions */
#define `$INSTANCE_NAME`_STS_SPI_DONE_SHIFT          0x00u     
#define `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY_SHIFT     0x01u
#define `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL_SHIFT  0x02u
#define `$INSTANCE_NAME`_STS_RX_FIFO_FULL_SHIFT      0x03u
#define `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY_SHIFT 0x04u
#define `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN_SHIFT   0x05u
#define `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT     0x06u

#define `$INSTANCE_NAME`_STS_SPI_DONE               (0x01u << `$INSTANCE_NAME`_STS_SPI_DONE_SHIFT)        
#define `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY          (0x01u << `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY_SHIFT)    
#define `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL       (0x01u << `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL_SHIFT)    
#define `$INSTANCE_NAME`_STS_RX_FIFO_FULL           (0x01u << `$INSTANCE_NAME`_STS_RX_FIFO_FULL_SHIFT)    
#define `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY      (0x01u << `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY_SHIFT)    
#define `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN        (0x01u << `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN_SHIFT)  
#define `$INSTANCE_NAME`_STS_BYTE_COMPLETE          (0x01u << `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT)

/* StatusI Register Interrupt Enable Control Bits */
#define `$INSTANCE_NAME`_INT_ENABLE                     0x10u   /* As defined by the Register map for the AUX Control Register */

/* Bit Counter (7-bit) Control Register Bit Definitions */
#define `$INSTANCE_NAME`_CNTR_ENABLE                    0x20u   /* As defined by the Register map for the AUX Control Register */
                       
/***************************************
 *    Initialization Values
 **************************************/
#define `$INSTANCE_NAME`_INIT_INTERRUPTS_MASK       (`$IntOnSPIDone` << `$INSTANCE_NAME`_STS_SPI_DONE_SHIFT ) | (`$IntOnTXNotFull` << `$INSTANCE_NAME`_STS_TX_FIFO_NOT_FULL_SHIFT) | (`$IntOnTXFull` << `$INSTANCE_NAME`_STS_TX_FIFO_EMPTY_SHIFT ) | (`$IntOnRXNotEmpty` << `$INSTANCE_NAME`_STS_RX_FIFO_NOT_EMPTY_SHIFT ) | (`$IntOnRXEmpty` << `$INSTANCE_NAME`_STS_RX_FIFO_FULL_SHIFT ) | (`$IntOnRXOver` << `$INSTANCE_NAME`_STS_RX_FIFO_OVERRUN_SHIFT ) | (`$IntOnByteComp` << `$INSTANCE_NAME`_STS_BYTE_COMPLETE_SHIFT) 
#define `$INSTANCE_NAME`_BITCTR_INIT                ((`$INSTANCE_NAME`_DATAWIDTH*2)-1)
/***********************************
*     Parameter values
***********************************/
#define `$INSTANCE_NAME`_TXBUFFERSIZE   `$TxBufferSize`
#define `$INSTANCE_NAME`_RXBUFFERSIZE   `$RxBufferSize`

/********************************
*    Constants
********************************/
/* Datapath Auxillary Control Register definitions */
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR         0x01u
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_CLR 	    0x02u
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_LVL 	    0x04u
#define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_LVL 	    0x08u
#define `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK    0x10u 

/***********************************
*     Registers
***********************************/
#define `$INSTANCE_NAME`_TXDATA             (* (`$RegDefReplacementString` *)  `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u0__F0_REG)
#define `$INSTANCE_NAME`_TXDATA_PTR         (  (`$RegDefReplacementString` *)  `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u0__F0_REG)
#define `$INSTANCE_NAME`_RXDATA             (* (`$RegDefReplacementString` *)  `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u0__F1_REG)
#define `$INSTANCE_NAME`_RXDATA_PTR         (  (`$RegDefReplacementString` *)  `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u0__F1_REG)

#define `$INSTANCE_NAME`_AUX_CONTROLDP0	    (* (reg8 *) `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u0__DP_AUX_CTL_REG)
#define `$INSTANCE_NAME`_AUX_CONTROLDP0_PTR (  (reg8 *) `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u0__DP_AUX_CTL_REG)
#if (`$INSTANCE_NAME`_DATAWIDTH > 8)
    #define `$INSTANCE_NAME`_AUX_CONTROLDP1	     (* (reg8 *) `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u1__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROLDP1_PTR       ((reg8 *) `$INSTANCE_NAME`_BSPIM_`$VerilogSectionReplacementString`_dp_u1__DP_AUX_CTL_REG)
#endif


#define `$INSTANCE_NAME`_COUNTER_PERIOD    (* (reg8 *)  `$INSTANCE_NAME`_BSPIM_bitCounter__PERIOD_REG)
#define `$INSTANCE_NAME`_COUNTER_CONTROL   (* (reg8 *)  `$INSTANCE_NAME`_BSPIM_bitCounter__CONTROL_AUX_CTL_REG )
#define `$INSTANCE_NAME`_STATUS            (* (reg8 *)  `$INSTANCE_NAME`_BSPIM_stsreg__STATUS_REG)
#define `$INSTANCE_NAME`_STATUS_MASK       (* (reg8 *)  `$INSTANCE_NAME`_BSPIM_stsreg__MASK_REG )
#define `$INSTANCE_NAME`_STATUS_ACTL       (* (reg8 *)  `$INSTANCE_NAME`_BSPIM_stsreg__STATUS_AUX_CTL_REG )
#endif  /* CY_SPIM_v1_10_`$INSTANCE_NAME`_H */
