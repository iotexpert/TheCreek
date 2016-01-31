/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     Contains the function prototypes and constants available to the UART
*     user module.
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


#include "cytypes.h"
#include "cyfitter.h"

#if !defined(CY_UART_`$INSTANCE_NAME`_H)
#define CY_UART_`$INSTANCE_NAME`_H

#define `$INSTANCE_NAME`_RX_Enabled                     `$Enable_RX`
#define `$INSTANCE_NAME`_TX_Enabled                     `$Enable_TX`
#define `$INSTANCE_NAME`_RX_IntInterruptEnabled         `$Enable_RXIntInterrupt`
#define `$INSTANCE_NAME`_TX_IntInterruptEnabled         `$Enable_TXIntInterrupt`
#define `$INSTANCE_NAME`_InternalClockUsed              `$InternalClockUsed`
#define `$INSTANCE_NAME`_RXHW_Address_Enabled           `$EnableHWAddress`
#define `$INSTANCE_NAME`_OverSampleCount                `$OverSamplingRate`
#define `$INSTANCE_NAME`_ParityBits                     `$ParityBitsCount`
#define `$INSTANCE_NAME`_ParityType                     `$ParityType`


/* Use Any Enumerated Types */
`#cy_declare_enum B_UART__AddressModes`

`#cy_declare_enum B_UART__Parity_Type_revA`

/***************************************
 *   Function Prototypes
 **************************************/
void `$INSTANCE_NAME`_Start(void);
void `$INSTANCE_NAME`_Stop(void);
uint8 `$INSTANCE_NAME`_ReadControlRegister(void);
void  `$INSTANCE_NAME`_WriteControlRegister(uint8 control);

/* Only if RX is enabled */
#if(`$INSTANCE_NAME`_RX_Enabled)
#if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
    void  `$INSTANCE_NAME`_EnableRxInt(void);
    void  `$INSTANCE_NAME`_DisableRxInt(void);
#endif /* `$INSTANCE_NAME`_RX_IntInterruptEnabled */
    void  `$INSTANCE_NAME`_SetRxInterruptMode(uint8 intSrc);
    uint8 `$INSTANCE_NAME`_ReadRxData(void);
    uint8 `$INSTANCE_NAME`_ReadRxStatus(void);
    uint8 `$INSTANCE_NAME`_GetRxInterruptSource(void);

//    #define `$INSTANCE_NAME`_GetChar 					`$INSTANCE_NAME`_ReadRxData
    uint8 `$INSTANCE_NAME`_GetChar(void);
    uint16 `$INSTANCE_NAME`_GetByte(void);
    /* for 8-bit PSoC3 */
    uint8 `$INSTANCE_NAME`_GetRxBufferSize(void);
    /* for 32-bit PSoC3 */
    //uint16 `$INSTANCE_NAME`_GetRxBufferSize(void);
    /* for 8-bit PSoC3 */
    void `$INSTANCE_NAME`_ClearRxBuffer(void);
#if (`$INSTANCE_NAME`_RXHW_Address_Enabled)
        void `$INSTANCE_NAME`_SetRxAddressMode(uint8 addressMode);
        void `$INSTANCE_NAME`_SetRxAddress1(uint8 address);
        void `$INSTANCE_NAME`_SetRxAddress2(uint8 address);
#endif
    void `$INSTANCE_NAME`_LoadRxConfig(void);
#if(`$INSTANCE_NAME`_RX_IntInterruptEnabled)
    CY_ISR_PROTO(`$INSTANCE_NAME`_RXISR);
#endif
#endif

/* Only if TX is enabled */
#if(`$INSTANCE_NAME`_TX_Enabled)
    #if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
    void `$INSTANCE_NAME`_EnableTxInt(void);
    void `$INSTANCE_NAME`_DisableTxInt(void);
#endif /* `$INSTANCE_NAME`_TX_IntInterruptEnabled */
    void `$INSTANCE_NAME`_SetTxInterruptMode(uint8 intSrc);
    void `$INSTANCE_NAME`_WriteTxData(uint8 txDataByte);
    uint8 `$INSTANCE_NAME`_ReadTxStatus(void);
    uint8 `$INSTANCE_NAME`_GetTxInterruptSource(void);

//#define `$INSTANCE_NAME`_PutChar					`$INSTANCE_NAME`_WriteTxData

    /* PutString for 8-bit PSoC3 */
    void `$INSTANCE_NAME`_PutChar(uint8 txDataByte);
    /* PutString for 8-bit PSoC3 */
    void `$INSTANCE_NAME`_PutString(uint8* ramString);
    /* PutString for 32-bit PSoC3 */
    //void `$INSTANCE_NAME`_PutString(uint32* ramString);
    /* PutString from ROM for 8-bit PSoC3 */
    void `$INSTANCE_NAME`_PutStringConst(uint8* romString);
    /* PutString from ROM for 32-bit PSoC3 */
    //void `$INSTANCE_NAME`_PutStringConst(uint32* romString);
    /* PutArray for 8-bit PSoC3 */
    void `$INSTANCE_NAME`_PutArray(uint8* ramString,int8 byteCount);
    /* PutArray for 32-bit PSoC3 */
    //void `$INSTANCE_NAME`_PutArray(uint32* ramString,uint8 byteCount);
    /* PutArrayConst for 8-bit PSoC3 */
    void `$INSTANCE_NAME`_PutArrayConst(uint8* romString,int8 byteCount);
    /* PutArrayConst for 32-bit PSoC3 */
    //void `$INSTANCE_NAME`_PutArrayConst(uint32* romString,uint8 byteCount);
    void `$INSTANCE_NAME`_PutCRLF(uint8 txDataByte);
    uint8 `$INSTANCE_NAME`_GetTxBufferSize(void);
    /* 32-bit Version */
    //uint16 `$INSTANCE_NAME`_GetTxBufferSize(void);
    void `$INSTANCE_NAME`_ClearTxBuffer(void);
    void `$INSTANCE_NAME`_SendBreak(void);
    void `$INSTANCE_NAME`_SetTxAddressMode(uint8 addressMode);
    void `$INSTANCE_NAME`_HardwareAddressEnable(uint8 addressEnable);
    void `$INSTANCE_NAME`_LoadTxConfig(void);
#if(`$INSTANCE_NAME`_TX_IntInterruptEnabled)
    CY_ISR_PROTO(`$INSTANCE_NAME`_TXISR);
#endif
#endif

/***********************************
*     Constants
***********************************/                           
/* Status Register definitions */
//TODO: This should be changed to an enumerate type?
#if(`$INSTANCE_NAME`_TX_Enabled)
    #define `$INSTANCE_NAME`_TX_STS_COMPLETE_SHIFT      0x00u
    #define `$INSTANCE_NAME`_TX_STS_COMPLETE           (0x01u << `$INSTANCE_NAME`_TX_STS_COMPLETE_SHIFT)
    #define `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY_SHIFT    0x01u
    #define `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY         (0x01u << `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY_SHIFT)
    #define `$INSTANCE_NAME`_TX_STS_FIFO_FULL_SHIFT     0x02u
    #define `$INSTANCE_NAME`_TX_STS_FIFO_FULL          (0x01u << `$INSTANCE_NAME`_TX_STS_FIFO_FULL_SHIFT)
    #define `$INSTANCE_NAME`_TX_STS_FIFO_NOT_FULL_SHIFT 0x02u
    #define `$INSTANCE_NAME`_TX_STS_FIFO_NOT_FULL      (0x01u << `$INSTANCE_NAME`_TX_STS_FIFO_NOT_FULL_SHIFT)
#endif
#if(`$INSTANCE_NAME`_RX_Enabled)
    #define `$INSTANCE_NAME`_RX_STS_MRKSPC_SHIFT        0x00u
    #define `$INSTANCE_NAME`_RX_STS_BREAK_SHIFT         0x01u
    #define `$INSTANCE_NAME`_RX_STS_PAR_ERROR_SHIFT     0x02u
    #define `$INSTANCE_NAME`_RX_STS_STOP_ERROR_SHIFT    0x03u
    #define `$INSTANCE_NAME`_RX_STS_OVERRUN_SHIFT       0x04u
    #define `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY_SHIFT 0x05u
    #define `$INSTANCE_NAME`_RX_STS_ADDR_MATCH_SHIFT    0x06u
    
    #define `$INSTANCE_NAME`_RX_STS_MRKSPC              (0x01u << `$INSTANCE_NAME`_RX_STS_MRKSPC_SHIFT)   
    #define `$INSTANCE_NAME`_RX_STS_BREAK               (0x01u << `$INSTANCE_NAME`_RX_STS_BREAK_SHIFT)   
    #define `$INSTANCE_NAME`_RX_STS_PAR_ERROR           (0x01u << `$INSTANCE_NAME`_RX_STS_PAR_ERROR_SHIFT) 
    #define `$INSTANCE_NAME`_RX_STS_STOP_ERROR          (0x01u << `$INSTANCE_NAME`_RX_STS_STOP_ERROR_SHIFT)
    #define `$INSTANCE_NAME`_RX_STS_OVERRUN             (0x01u << `$INSTANCE_NAME`_RX_STS_OVERRUN_SHIFT)
    #define `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY       (0x01u << `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY_SHIFT)
    #define `$INSTANCE_NAME`_RX_STS_ADDR_MATCH          (0x01u << `$INSTANCE_NAME`_RX_STS_ADDR_MATCH_SHIFT )
#endif                                                        

/* Control Register definitions */
//TODO: This should be changed to an enumerate type?
#define `$INSTANCE_NAME`_CTRL_TXRUN_SHIFT               0x00u
#define `$INSTANCE_NAME`_CTRL_RXRUN_SHIFT               0x01u
#define `$INSTANCE_NAME`_CTRL_MARK_SHIFT                0x02u /* 1 sets mark, 0 sets space */
#define `$INSTANCE_NAME`_CTRL_PARITYTYPE0_SHIFT         0x03u /* Defines the type of parity implemented */
#define `$INSTANCE_NAME`_CTRL_PARITYTYPE1_SHIFT         0x04u /* Defines the type of parity implemented */
#define `$INSTANCE_NAME`_CTRL_RXADDR_MODE0_SHIFT        0x05u
#define `$INSTANCE_NAME`_CTRL_RXADDR_MODE1_SHIFT        0x06u
#define `$INSTANCE_NAME`_CTRL_RXADDR_MODE2_SHIFT        0x07u
#define `$INSTANCE_NAME`_CTRL_SEND_BREAK_SHIFT        	0x07u	// TEMP use RXADDR mode space

#define `$INSTANCE_NAME`_CTRL_TXRUN                     (0x01u << `$INSTANCE_NAME`_CTRL_TXRUN_SHIFT)
#define `$INSTANCE_NAME`_CTRL_RXRUN                     (0x01u << `$INSTANCE_NAME`_CTRL_RXRUN_SHIFT)
#define `$INSTANCE_NAME`_CTRL_MARK                      (0x01u << `$INSTANCE_NAME`_CTRL_MARK_SHIFT)
#define `$INSTANCE_NAME`_CTRL_PARITYTYPE_MASK           (0x03u << `$INSTANCE_NAME`_CTRL_PARITYTYPE0_SHIFT)
#define `$INSTANCE_NAME`_CTRL_RXADDR_MODE_MASK          (0x07u << `$INSTANCE_NAME`_CTRL_RXADDR_MODE0_SHIFT)
#define `$INSTANCE_NAME`_CTRL_SEND_BREAK          		(0x01u << `$INSTANCE_NAME`_CTRL_SEND_BREAK_SHIFT)

/* StatusI Register Interrupt Enable Control Bits */
#define `$INSTANCE_NAME`_INT_ENABLE                     0x10u   /* As defined by the Register map for the AUX Control Register */

/* Bit Counter (7-bit) Control Register Bit Definitions */
#define `$INSTANCE_NAME`_CNTR_ENABLE                    0x20u   /* As defined by the Register map for the AUX Control Register */


/***************************************
 *    Initialization Values
 **************************************/
 //TODO: Finish this initialization value
#define `$INSTANCE_NAME`_INIT_RX_INTERRUPTS_MASK        (`$InterruptOnByteRcvd` << `$INSTANCE_NAME`_RX_STS_FIFO_NOTEMPTY_SHIFT ) | (`$InterruptOnAddDetect` << `$INSTANCE_NAME`_RX_STS_MRKSPC_SHIFT) | (`$InterruptOnAddressMatch` << `$INSTANCE_NAME`_RX_STS_ADDR_MATCH_SHIFT ) | (`$InterruptOnParityError` << `$INSTANCE_NAME`_RX_STS_PAR_ERROR_SHIFT ) | (`$InterruptOnStopError` << `$INSTANCE_NAME`_RX_STS_STOP_ERROR_SHIFT ) | (`$InterruptOnBreak` << `$INSTANCE_NAME`_RX_STS_BREAK_SHIFT ) | (`$InterruptOnOverrunError` << `$INSTANCE_NAME`_RX_STS_OVERRUN_SHIFT) 
#define `$INSTANCE_NAME`_INIT_TX_INTERRUPTS_MASK        (`$InterruptOnTXComplete` << `$INSTANCE_NAME`_TX_STS_COMPLETE_SHIFT) | (`$InterruptOnTXFifoEmpty` << `$INSTANCE_NAME`_TX_STS_FIFO_EMPTY ) | (`$InterruptOnTXFifoFull` << `$INSTANCE_NAME`_TX_STS_FIFO_FULL_SHIFT ) | (`$InterruptOnTXFifoNotFull` << `$INSTANCE_NAME`_TX_STS_FIFO_NOT_FULL_SHIFT )

/****************************************************
******      Parameter values                    *****
****************************************************/
#define `$INSTANCE_NAME`_TXBUFFERSIZE                   `$TXBufferSize`
#define `$INSTANCE_NAME`_RXBUFFERSIZE                   `$RXBufferSize`

#define `$INSTANCE_NAME`_NUMBER_OF_START_BIT            1
#define `$INSTANCE_NAME`_NUMBER_OF_DATA_BITS            `$NumDataBits`
#define `$INSTANCE_NAME`_NUMBER_OF_STOP_BITS            `$NumStopBits`
//#define `$INSTANCE_NAME`_NUMBER_OF_PARITY_BITS          `$ParityBitsCount`
//#define `$INSTANCE_NAME`_TXBITCTR_INIT                  `$TXCounterInit`
//#define `$INSTANCE_NAME`_RXBITCTR_INIT                  `$RXCounterInit`
#define `$INSTANCE_NAME`_TXBITCTR_13BITS                 (13+1)*`$INSTANCE_NAME`_OverSampleCount-1
#define `$INSTANCE_NAME`_TXCLKGEN_DP                 		`$TXBitClkGenDP`

#if (`$INSTANCE_NAME`_RXHW_Address_Enabled)
    #define `$INSTANCE_NAME`_RXHWADDRESS1               `$Address1`
    #define `$INSTANCE_NAME`_RXHWADDRESS2               `$Address2`
#endif

/****************************************************
******     Registers                            *****
****************************************************/
 #ifdef `$INSTANCE_NAME`_BUART_sCR_ctrl__CONTROL_REG
    #define `$INSTANCE_NAME`_CONTROL                    (* (reg8 *) `$INSTANCE_NAME`_BUART_sCR_ctrl__CONTROL_REG )
	#define `$INSTANCE_NAME`_CONTROL_REMOVED 			0
 #else	
	#define `$INSTANCE_NAME`_CONTROL_REMOVED 			1
 #endif

#if(`$INSTANCE_NAME`_TX_Enabled)
    #define `$INSTANCE_NAME`_TXDATA                     (* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_dpTXShifter_u0__F0_REG )
    #define `$INSTANCE_NAME`_TXDATA_PTR                 ((reg8 *) `$INSTANCE_NAME`_BUART_sTX_dpTXShifter_u0__F0_REG )
    
    #define `$INSTANCE_NAME`_TXSTATUS                   (* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_tx_sts__STATUS_REG )
    #define `$INSTANCE_NAME`_TXSTATUS_MASK              (* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_tx_sts__MASK_REG )
    #define `$INSTANCE_NAME`_TXSTATUS_ACTL              (* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_tx_sts__STATUS_AUX_CTL_REG )
	
			/*DP clock */	
  #if(`$INSTANCE_NAME`_TXCLKGEN_DP)
	#define `$INSTANCE_NAME`_BIT_CENTER                 (`$INSTANCE_NAME`_OverSampleCount-1)
    #define `$INSTANCE_NAME`_TXBITCLKGEN_CTR			(* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_dpTXBitClkGen_u0__D0_REG )
    #define `$INSTANCE_NAME`_TXBITCLKTX_COMPLETE		(* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_dpTXBitClkGen_u0__D1_REG )
  #else 	/* Count7 clock*/
    #define `$INSTANCE_NAME`_TXBITCTR_PERIOD            (* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_TXBitCounter__PERIOD_REG )
    #define `$INSTANCE_NAME`_TXBITCTR_PERIOD_PTR        ((reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_TXBitCounter__PERIOD_REG )    
    #define `$INSTANCE_NAME`_TXBITCTR_CONTROL           (* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_TXBitCounter__CONTROL_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_TXBITCTR_CONTROL_PTR       ((reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_TXBitCounter__CONTROL_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_TXBITCTR_COUNTER           (* (reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_TXBitCounter__COUNT_REG )
    #define `$INSTANCE_NAME`_TXBITCTR_COUNTER_PTR       ((reg8 *) `$INSTANCE_NAME`_BUART_sTX_sCLOCK_TXBitCounter__COUNT_REG )
  #endif

#endif
#if(`$INSTANCE_NAME`_RX_Enabled)
    #define `$INSTANCE_NAME`_RXDATA                     (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_dpRXShifter_u0__F0_REG )
    #define `$INSTANCE_NAME`_RXDATA_PTR                 ((reg8 *) `$INSTANCE_NAME`_BUART_sRX_dpRXShifter_u0__F0_REG )    
    #define `$INSTANCE_NAME`_RXADDRESS1                 (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_dpRXShifter_u0__D0_REG )
    #define `$INSTANCE_NAME`_RXADDRESS1_PTR             ((reg8 *) `$INSTANCE_NAME`_BUART_sRX_dpRXShifter_u0__D0_REG )
    #define `$INSTANCE_NAME`_RXADDRESS2                 (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_dpRXShifter_u0__D1_REG )
    #define `$INSTANCE_NAME`_RXADDRESS2_PTR             ((reg8 *) `$INSTANCE_NAME`_BUART_sRX_dpRXShifter_u0__D1_REG )    
    
    #define `$INSTANCE_NAME`_RXBITCTR_PERIOD            (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_RXBitCounter__PERIOD_REG )
    #define `$INSTANCE_NAME`_RXBITCTR_PERIOD_PTR        ((reg8 *) `$INSTANCE_NAME`_BUART_sRX_RXBitCounter__PERIOD_REG )    
    #define `$INSTANCE_NAME`_RXBITCTR_CONTROL           (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_RXBitCounter__CONTROL_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_RXBITCTR_CONTROL_PTR       ((reg8 *) `$INSTANCE_NAME`_BUART_sRX_RXBitCounter__CONTROL_AUX_CTL_REG )
    #define `$INSTANCE_NAME`_RXBITCTR_COUNTER           (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_RXBitCounter__COUNT_REG )
    #define `$INSTANCE_NAME`_RXBITCTR_COUNTER_PTR       ((reg8 *) `$INSTANCE_NAME`_BUART_sRX_RXBitCounter__COUNT_REG )    
    
    #define `$INSTANCE_NAME`_RXSTATUS                   (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_rx_sts__STATUS_REG )
    #define `$INSTANCE_NAME`_RXSTATUS_MASK              (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_rx_sts__MASK_REG )
    #define `$INSTANCE_NAME`_RXSTATUS_ACTL              (* (reg8 *) `$INSTANCE_NAME`_BUART_sRX_rx_sts__STATUS_AUX_CTL_REG )
#endif
#endif  /* CY_UART_`$INSTANCE_NAME`_H */
