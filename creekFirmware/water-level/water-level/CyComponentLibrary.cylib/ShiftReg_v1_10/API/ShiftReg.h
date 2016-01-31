/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file contains definitions associated with the Shift Register 
*  component
*
* Note:
* 
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#ifndef CY_SHIFTREG_`$INSTANCE_NAME`_H
#define CY_SHIFTREG_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
*   Conditional Compilation Parameters  
***************************************/

#define `$INSTANCE_NAME`_FIFO_SIZE                `$FifoSize`
#define `$INSTANCE_NAME`_USE_INPUT_FIFO           `$UseInputFifo`
#define `$INSTANCE_NAME`_USE_OUTPUT_FIFO          `$UseOutputFifo`
#define `$INSTANCE_NAME`_SR_SIZE                  `$Length`

#if((`$INSTANCE_NAME`_USE_INPUT_FIFO == 1) || (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1))
#define `$INSTANCE_NAME`_FIFO_USED                0x01U
#endif

/***************************************
*        Function Prototypes            
***************************************/

void     `$INSTANCE_NAME`_Start(void);
void     `$INSTANCE_NAME`_Stop(void);
void     `$INSTANCE_NAME`_EnableInt(void);
void     `$INSTANCE_NAME`_DisableInt(void);
void     `$INSTANCE_NAME`_SetIntMode(uint8 interruptSource);
uint8    `$INSTANCE_NAME`_GetIntStatus(void);
#ifdef `$INSTANCE_NAME`_FIFO_USED
    uint8    `$INSTANCE_NAME`_GetFIFOStatus(uint8 fifoId);
#endif /*`$INSTANCE_NAME`_FIFO_USED*/
#if (`$INSTANCE_NAME`_SR_SIZE <= 8)
    void     `$INSTANCE_NAME`_WriteRegValue(uint8 txDataByte);
    uint8    `$INSTANCE_NAME`_ReadRegValue(void);
#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
    uint8    `$INSTANCE_NAME`_WriteData(uint8 txDataByte);
#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
    uint8    `$INSTANCE_NAME`_ReadData(void);
#endif
#elif (`$INSTANCE_NAME`_SR_SIZE <= 16)
    void     `$INSTANCE_NAME`_WriteRegValue(uint16 txDataByte);
    uint16   `$INSTANCE_NAME`_ReadRegValue(void);
#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
    uint8    `$INSTANCE_NAME`_WriteData(uint16 txDataByte);
#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
    uint16   `$INSTANCE_NAME`_ReadData(void);
#endif
#elif (`$INSTANCE_NAME`_SR_SIZE == 24)
    void     `$INSTANCE_NAME`_WriteRegValue(uint32 txDataByte);
    uint32   `$INSTANCE_NAME`_ReadRegValue(void);
#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
    uint8    `$INSTANCE_NAME`_WriteData(uint32 txDataByte);
#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
    uint32   `$INSTANCE_NAME`_ReadData(void);
#endif
#elif (`$INSTANCE_NAME`_SR_SIZE <= 32)
    void     `$INSTANCE_NAME`_WriteRegValue(uint32 txDataByte);
    uint32   `$INSTANCE_NAME`_ReadRegValue(void);
#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)
    uint8    `$INSTANCE_NAME`_WriteData(uint32 txDataByte);
#endif
#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
    uint32   `$INSTANCE_NAME`_ReadData(void);
#endif
#endif

/***************************************
*           API Constants               
***************************************/

#define `$INSTANCE_NAME`_LOAD                     0x01U
#define `$INSTANCE_NAME`_STORE                    0x02U
#define `$INSTANCE_NAME`_RESET                    0x04U

#define `$INSTANCE_NAME`_IN_FIFO                  0x01U
#define `$INSTANCE_NAME`_OUT_FIFO                 0x02U

#define `$INSTANCE_NAME`_RET_FIFO_FULL            0x00U
#define `$INSTANCE_NAME`_RET_FIFO_NOT_EMPTY       0x01U
#define `$INSTANCE_NAME`_RET_FIFO_EMPTY           0x02U

#define `$INSTANCE_NAME`_RET_FIFO_NOT_DEFINED     0xFEU
#define `$INSTANCE_NAME`_RET_FIFO_BAD_PARAM       0xFFU


/***************************************
*    Enumerated Types and Parameters    
***************************************/

#define `$INSTANCE_NAME`_DIRECTION                `$Direction`


/***************************************
*    Initial Parameter Constants        
***************************************/

#define `$INSTANCE_NAME`_SR_MASK                  0x`$SR_MASK`U
#define `$INSTANCE_NAME`_INT_SRC                  `$InterruptSource`
`#cy_declare_enum Direction`

/***************************************
*             Registers                 
***************************************/

/* Shift Register control register */
#define `$INSTANCE_NAME`_SR_CONTROL               (* (reg8 *) `$INSTANCE_NAME`_bSR_CtrlReg__CONTROL_REG) 

/* Shift Regisster interupt status register */
#define `$INSTANCE_NAME`_SR_STATUS                (* (reg8 *) `$INSTANCE_NAME`_bSR_StsReg__STATUS_REG) 
#define `$INSTANCE_NAME`_SR_STATUS_MASK           (* (reg8 *) `$INSTANCE_NAME`_bSR_StsReg__MASK_REG)
#define `$INSTANCE_NAME`_SR_AUX_CONTROL           (* (reg8 *) `$INSTANCE_NAME`_bSR_StsReg__STATUS_AUX_CTL_REG) 

#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)

    #if (`$INSTANCE_NAME`_SR_SIZE <= 8)
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB          (*(reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__F0_REG )
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR      ((reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__F0_REG )
    #elif (`$INSTANCE_NAME`_SR_SIZE <= 16)
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB          (*(reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__F0_REG )
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR      ((reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__F0_REG )
    #elif (`$INSTANCE_NAME`_SR_SIZE <= 24)
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB          (*(reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__F0_REG )
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR      ((reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__F0_REG )
    #elif (`$INSTANCE_NAME`_SR_SIZE <= 32)
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB          (*(reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__F0_REG )
        #define `$INSTANCE_NAME`_IN_FIFO_VAL_LSB_PTR      ((reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__F0_REG )
    #endif
#endif    
    
    #if (`$INSTANCE_NAME`_SR_SIZE <= 8)       /* 8bit - ShiftReg */

    #define `$INSTANCE_NAME`_SR8_AUX_CONTROL          (*(reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__DP_AUX_CTL_REG) 
	
    #define `$INSTANCE_NAME`_SHIFT_REG_LSB            (*(reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_LSB_PTR        ((reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB      (*(reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR  ((reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_D0_REG_PTR     ((reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__D0_REG)
      
    #define `$INSTANCE_NAME`_OUT_FIFO_VAL_LSB         (*(reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__F1_REG )
    #define `$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR     ((reg8 *) `$INSTANCE_NAME`_bSR_sC8_BShiftRegDp_u0__F1_REG )

#elif (`$INSTANCE_NAME`_SR_SIZE <= 16)    /* 16bit - ShiftReg */

    #define `$INSTANCE_NAME`_SR16_AUX_CONTROL1        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__DP_AUX_CTL_REG) 
    #define `$INSTANCE_NAME`_SR16_AUX_CONTROL2        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u1__DP_AUX_CTL_REG) 

    #define `$INSTANCE_NAME`_SHIFT_REG_LSB            (*(reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_LSB_PTR        ((reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB	     (*(reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR	 ((reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_D0_REG_PTR     ((reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__D0_REG )

    #define `$INSTANCE_NAME`_OUT_FIFO_VAL_LSB         (*(reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__F1_REG )
    #define `$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR     ((reg16 *) `$INSTANCE_NAME`_bSR_sC16_BShiftRegDp_u0__F1_REG )
    
#elif (`$INSTANCE_NAME`_SR_SIZE <= 24)    /* 24bit - ShiftReg */

    #define `$INSTANCE_NAME`_SR24_AUX_CONTROL1        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__DP_AUX_CTL_REG) 
    #define `$INSTANCE_NAME`_SR24_AUX_CONTROL2        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u1__DP_AUX_CTL_REG) 
    #define `$INSTANCE_NAME`_SR24_AUX_CONTROL3        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u2__DP_AUX_CTL_REG) 

    #define `$INSTANCE_NAME`_SHIFT_REG_LSB            (*(reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR	 ((reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB	     (*(reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_LSB_PTR        ((reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_D0_REG_PTR     ((reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__D0_REG )

    #define `$INSTANCE_NAME`_OUT_FIFOVAL_LSB          (*(reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__F1_REG )
    #define `$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR     ((reg32 *) `$INSTANCE_NAME`_bSR_sC24_BShiftRegDp_u0__F1_REG )

#elif (`$INSTANCE_NAME`_SR_SIZE <= 32)    /* 32bit - ShiftReg */

    #define `$INSTANCE_NAME`_SR32_AUX_CONTROL1        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__DP_AUX_CTL_REG) 
    #define `$INSTANCE_NAME`_SR32_AUX_CONTROL2        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u1__DP_AUX_CTL_REG) 
    #define `$INSTANCE_NAME`_SR32_AUX_CONTROL3        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u2__DP_AUX_CTL_REG) 
    #define `$INSTANCE_NAME`_SR32_AUX_CONTROL4        (*(reg8 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u3__DP_AUX_CTL_REG) 

    #define `$INSTANCE_NAME`_SHIFT_REG_LSB            (*(reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_LSB_PTR        ((reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__A0_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB	     (*(reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_VALUE_LSB_PTR	 ((reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__A1_REG )
    #define `$INSTANCE_NAME`_SHIFT_REG_D0_REG_PTR     ((reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__D0_REG )

    #define `$INSTANCE_NAME`_OUT_FIFO_VAL_LSB         (*(reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__F1_REG )
    #define `$INSTANCE_NAME`_OUT_FIFO_VAL_LSB_PTR     ((reg32 *) `$INSTANCE_NAME`_bSR_sC32_BShiftRegDp_u0__F1_REG )	
#endif

/***************************************
*       Register Constants              
***************************************/

#define `$INSTANCE_NAME`_INTERRUPTS_ENABLE        0x10U
#define `$INSTANCE_NAME`_LOAD_INT_EN              0x01U
#define `$INSTANCE_NAME`_STORE_INT_EN             0x02U
#define `$INSTANCE_NAME`_RESET_INT_EN             0x04U
#define `$INSTANCE_NAME`_CLK_EN                   0x01U

#define `$INSTANCE_NAME`_RESET_INT_EN_MASK        0xFBU
#define `$INSTANCE_NAME`_LOAD_INT_EN_MASK         0xFEU
#define `$INSTANCE_NAME`_STORE_INT_EN_MASK        0xFDU
#define `$INSTANCE_NAME`_INTS_EN_MASK             0x07U

#define `$INSTANCE_NAME`_OUT_FIFO_CLR_BIT         0x02U

#if (`$INSTANCE_NAME`_USE_INPUT_FIFO == 1)

    #define `$INSTANCE_NAME`_IN_FIFO_MASK             0x18U

    #define `$INSTANCE_NAME`_IN_FIFO_FULL             0x00U
    #define `$INSTANCE_NAME`_IN_FIFO_EMPTY            0x01U
    #define `$INSTANCE_NAME`_IN_FIFO_NOT_EMPTY        0x02U
#endif

#if (`$INSTANCE_NAME`_USE_OUTPUT_FIFO == 1)
    #define `$INSTANCE_NAME`_OUT_FIFO_MASK            0x60U

    #define `$INSTANCE_NAME`_OUT_FIFO_EMPTY           0x00U
    #define `$INSTANCE_NAME`_OUT_FIFO_FULL            0x01U
    #define `$INSTANCE_NAME`_OUT_FIFO_NOT_EMPTY       0x02U
#endif

#endif /* CY_SHIFTREG_`$INSTANCE_NAME`_H */

//[] END OF FILE
