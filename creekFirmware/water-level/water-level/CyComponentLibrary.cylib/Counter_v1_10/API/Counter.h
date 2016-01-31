/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     Contains the function prototypes and constants available to the counter
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

#if !defined(CY_COUNTER_`$INSTANCE_NAME`_H)
#define CY_COUNTER_`$INSTANCE_NAME`_H

#define `$INSTANCE_NAME`_Resolution `$Resolution`
#define `$INSTANCE_NAME`_UsingFixedFunction `$FixedFunctionUsed`
#define `$INSTANCE_NAME`_ControlRegRemoved  `$ControlRegRemoved`
#define `$INSTANCE_NAME`_COMPARE_MODE_SOFTWARE `$CompareModeSoftware`
#define `$INSTANCE_NAME`_CAPTURE_MODE_SOFTWARE `$CaptureModeSoftware`

/**************************************
 *  Function Prototypes
 *************************************/
void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_SetInterruptMode(uint8 interruptsMask);
uint8   `$INSTANCE_NAME`_GetInterruptSource(void);
uint8   `$INSTANCE_NAME`_ReadStatusRegister(void);
#if(!`$INSTANCE_NAME`_ControlRegRemoved)
uint8   `$INSTANCE_NAME`_ReadControlRegister(void);
void    `$INSTANCE_NAME`_WriteControlRegister(uint8 control);
#endif
void    `$INSTANCE_NAME`_WriteCounter(`$RegSizeReplacementString` counter);
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCounter(void);
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCapture(void);
void    `$INSTANCE_NAME`_WritePeriod(`$RegSizeReplacementString` period);
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadPeriod( void );
#if (!`$INSTANCE_NAME`_UsingFixedFunction)
    void    `$INSTANCE_NAME`_WriteCompare(`$RegSizeReplacementString` compare);
    `$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCompare( void );
#endif

#if (`$INSTANCE_NAME`_COMPARE_MODE_SOFTWARE)
    void    `$INSTANCE_NAME`_SetCompareMode(uint8 comparemode);
#endif
#if (`$INSTANCE_NAME`_CAPTURE_MODE_SOFTWARE)
    void    `$INSTANCE_NAME`_SetCaptureMode(uint8 capturemode);
#endif
void `$INSTANCE_NAME`_ClearFIFO(void);

/***************************************
*   Enumerated Types and Parameters
***************************************/

/* Enumerated Type B_Counter__CompareModes, Used in Compare Mode */
`#cy_declare_enum B_Counter__CompareModes`

/* Enumerated Type B_Counter__CaptureModes, Used in Capture Mode */
`#cy_declare_enum B_Counter__CaptureModes`

/***************************************
 *  Initialization Values
 **************************************/
#define `$INSTANCE_NAME`_INIT_PERIOD_VALUE       `$Period`
#define `$INSTANCE_NAME`_INIT_COUNTER_VALUE      `$InitCounterValue`
#define `$INSTANCE_NAME`_INIT_COMPARE_VALUE      `$CompareValue`
#define `$INSTANCE_NAME`_INIT_INTERRUPTS_MASK    ((`$InterruptOnTC` << `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK_SHIFT) | (`$InterruptOnCapture` << `$INSTANCE_NAME`_STATUS_CAPTURE_INT_EN_MASK_SHIFT) | (`$InterruptOnCompare` << `$INSTANCE_NAME`_STATUS_CMP_INT_EN_MASK_SHIFT) | (`$InterruptOnOverUnderFlow` << `$INSTANCE_NAME`_STATUS_OVERFLOW_INT_EN_MASK_SHIFT) | (`$InterruptOnOverUnderFlow` << `$INSTANCE_NAME`_STATUS_UNDERFLOW_INT_EN_MASK_SHIFT))
#define `$INSTANCE_NAME`_DEFAULT_COMPARE_MODE    (`$CompareMode` << `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT)
#define `$INSTANCE_NAME`_DEFAULT_CAPTURE_MODE    (`$CaptureMode` << `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT)

/**************************************
 *  Registers
 *************************************/
#if (`$INSTANCE_NAME`_UsingFixedFunction)
	#define `$INSTANCE_NAME`_STATICCOUNT_LSB     (*(reg16 *) `$INSTANCE_NAME`_CounterHW__CAP0 )
	#define `$INSTANCE_NAME`_STATICCOUNT_LSB_PTR ( (reg16 *) `$INSTANCE_NAME`_CounterHW__CAP0 )
	#define `$INSTANCE_NAME`_PERIOD_LSB          (*(reg16 *) `$INSTANCE_NAME`_CounterHW__PER0 )
	#define `$INSTANCE_NAME`_PERIOD_LSB_PTR      ( (reg16 *) `$INSTANCE_NAME`_CounterHW__PER0 )
	#define `$INSTANCE_NAME`_COMPARE_LSB         (*(reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 ) /* MODE must be set to 1 to set the compare value */
	#define `$INSTANCE_NAME`_COMPARE_LSB_PTR     ( (reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 )
	#define `$INSTANCE_NAME`_COUNTER_LSB         (*(reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 ) /* MODE must be set to 0 to get the count */
	#define `$INSTANCE_NAME`_COUNTER_LSB_PTR     ( (reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 )
#else
	#define `$INSTANCE_NAME`_STATICCOUNT_LSB     (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__F0_REG )
	#define `$INSTANCE_NAME`_STATICCOUNT_LSB_PTR ( (`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__F0_REG )
	#define `$INSTANCE_NAME`_PERIOD_LSB          (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D0_REG )
	#define `$INSTANCE_NAME`_PERIOD_LSB_PTR      ( (`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D0_REG )
	#define `$INSTANCE_NAME`_COMPARE_LSB         (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D1_REG )
	#define `$INSTANCE_NAME`_COMPARE_LSB_PTR     ( (`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D1_REG )
	#define `$INSTANCE_NAME`_COUNTER_LSB         (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__A0_REG )
	#define `$INSTANCE_NAME`_COUNTER_LSB_PTR     ( (`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__A0_REG )
    #define `$INSTANCE_NAME`_AUX_CONTROLDP0	     (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROLDP0_PTR  ( (`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__DP_AUX_CTL_REG)
    #if (`$INSTANCE_NAME`_Resolution == 16 || `$INSTANCE_NAME`_Resolution == 24 || `$INSTANCE_NAME`_Resolution == 32)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP1    (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u1__DP_AUX_CTL_REG)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP1_PTR ((`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u1__DP_AUX_CTL_REG)
    #endif
    #if (`$INSTANCE_NAME`_Resolution == 24 || `$INSTANCE_NAME`_Resolution == 32)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP2    (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u2__DP_AUX_CTL_REG)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP2_PTR ((`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u2__DP_AUX_CTL_REG)
    #endif
    #if (`$INSTANCE_NAME`_Resolution == 32)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP3    (*(`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u3__DP_AUX_CTL_REG)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP3_PTR ((`$RegDefReplacementString` *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u3__DP_AUX_CTL_REG)
    #endif
#endif

#if (`$INSTANCE_NAME`_UsingFixedFunction)
    #define `$INSTANCE_NAME`_STATUS         (*(reg8 *) `$INSTANCE_NAME`_CounterHW__SR0 )
    #define `$INSTANCE_NAME`_STATUS_MASK    (*(reg8 *) `$INSTANCE_NAME`_CounterHW__SR0 ) /* In Fixed Function Block Status and Mask are the same register */
    #define `$INSTANCE_NAME`_CONTROL        (*(reg8 *) `$INSTANCE_NAME`_CounterHW__CFG0)
    #define `$INSTANCE_NAME`_CONTROL2       (*(reg8 *) `$INSTANCE_NAME`_CounterHW__CFG1)
    #define `$INSTANCE_NAME`_GLOBAL_ENABLE  (*(reg8 *) `$INSTANCE_NAME`_CounterHW__PM_ACT_CFG)
    
    /********************************
    *    Constants
    ********************************/
    /* Fixed Function Block Chosen */
	#define `$INSTANCE_NAME`_BLOCK_EN_MASK          `$INSTANCE_NAME`_CounterHW__PM_ACT_MSK
    /* Control Register Bit Locations */
    #define `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT    0x01u       /* Compare is set in the CFG1 register of the Fixed Function Block at this location */
    #define `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT    0x00u       /* No Capture Mode in Fixed Function Block.  Capture is on rising edge of hardware or on software capture only */
    #define `$INSTANCE_NAME`_CTRL_RESET_SHIFT       0x00u       /* No software Reset Mode in Fixed Function Block.  Reset on Hardware input only */
    #define `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT      0x00u       /* As defined in Register Map, part of TMRX_CFG0 register */
    /* Control Register Bit Masks */
    #define `$INSTANCE_NAME`_CTRL_CMPMODE_MASK      (0x07u << `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT)  /* Assumes CFG0 Mode bit is set to 1 for compare mode */
    #define `$INSTANCE_NAME`_CTRL_CAPMODE_MASK      (0x00u << `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT)  /* Not valid for Fixed Function Block */ 
    #define `$INSTANCE_NAME`_CTRL_RESET             (0x00u << `$INSTANCE_NAME`_CTRL_RESET_SHIFT)     /* Not valid for Fixed Function Block */    
    #define `$INSTANCE_NAME`_CTRL_ENABLE            (0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT) 

    /* Control2 Register Bit Masks */
    #define `$INSTANCE_NAME`_CTRL2_IRQ_SEL_SHIFT    0x00u       /* As defined in Register Map, Part of the TMRX_CFG1 register */
    #define `$INSTANCE_NAME`_CTRL2_IRQ_SEL          (0x01u << `$INSTANCE_NAME`_CTRL2_IRQ_SEL_SHIFT)     
    
    /* Status Register Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_CMP_SHIFT       0x06u  /* As defined in Register Map, part of TMRX_SR0 register , Shared with Capture Status */ 
    #define `$INSTANCE_NAME`_STATUS_ZERO_SHIFT      0x07u  /* As defined in Register Map, part of TMRX_SR0 register */ 
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW_SHIFT  0x00u  /* Not valid for Fixed Function Block */ 
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW_SHIFT 0x00u  /* TC in FF block because it is a down counter Underflow is not available then just TC */
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT   0x06u  /* As defined in Register Map, part of TMRX_SR0 register, Shared with Compare Status */
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT  0x00u  /* Not valid for Fixed Function Block */ 
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP_SHIFT  0x00u  /* Not valid for Fixed Function Block */
    /* Status Register Interrupt Enable Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_CMP_INT_EN_MASK_SHIFT       (`$INSTANCE_NAME`_STATUS_CMP_SHIFT - 4)
    #define `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK_SHIFT      (`$INSTANCE_NAME`_STATUS_ZERO_SHIFT - 4)
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW_INT_EN_MASK_SHIFT  (0x00u)
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW_INT_EN_MASK_SHIFT (0x00u)
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_EN_MASK_SHIFT   (`$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT - 4)
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_EN_MASK_SHIFT  (0x00u)
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP_INT_EN_MASK_SHIFT  (0x00u)
    /* Status Register Bit Masks */                           
    #define `$INSTANCE_NAME`_STATUS_CMP             (0x01u << `$INSTANCE_NAME`_STATUS_CMP_SHIFT)
    #define `$INSTANCE_NAME`_STATUS_ZERO            (0x01u << `$INSTANCE_NAME`_STATUS_ZERO_SHIFT)
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW        (0x00u) /* Not available for Fixed Function Block */
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW       (0x00u) /* Not available for Fixed Function Block */
    #define `$INSTANCE_NAME`_STATUS_CAPTURE         (0x01u << `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT)
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL        (0x00u) /* Not available for Fixed Function Block */
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP        (0x00u) /* Not available for Fixed Function Block */
    /* Status Register Interrupt Bit Masks*/
    #define `$INSTANCE_NAME`_STATUS_CMP_INT_EN_MASK        (`$INSTANCE_NAME`_STATUS_CMP >> 4)
    #define `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK       (`$INSTANCE_NAME`_STATUS_ZERO >> 4)
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW_INT_EN_MASK   (0x00u) /* Not available for Fixed Function Block */
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW_INT_EN_MASK  (0x00u) /* Not available for Fixed Function Block */
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_EN_MASK    (`$INSTANCE_NAME`_STATUS_CAPTURE >> 4)
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_EN_MASK   (0x00u) /* Not available for Fixed Function Block */
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP_INT_EN_MASK   (0x00u) /* Not available for Fixed Function Block */
    
    /* Datapath Auxillary Control Register definitions */
    //#define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR     0x00u   /* Not valid for Fixed Function Block */ 
    //#define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_CLR     0x00u   /* Not valid for Fixed Function Block */ 
    //#define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_LVL     0x00u   /* Not valid for Fixed Function Block */ 
    //#define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_LVL     0x00u   /* Not valid for Fixed Function Block */ 
    //#define `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK  0x10u /* Not valid for Fixed Function Block */    
#else /* !`$INSTANCE_NAME`_UsingFixedFunction */
    #define `$INSTANCE_NAME`_STATUS         (* (reg8 *) `$INSTANCE_NAME`_CounterUDB_sSTSReg_stsreg__STATUS_REG )
    #define `$INSTANCE_NAME`_STATUS_MASK    (* (reg8 *) `$INSTANCE_NAME`_CounterUDB_sSTSReg_stsreg__MASK_REG )
    #define `$INSTANCE_NAME`_STATUS_AUX_CTRL (*(reg8 *) `$INSTANCE_NAME`_CounterUDB_sSTSReg_stsreg__STATUS_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_CONTROL        (* (reg8 *) `$INSTANCE_NAME`_CounterUDB_sCTRLReg_ctrlreg__CONTROL_REG )
    
    /********************************
    *    Constants
    ********************************/
    /* Control Register Bit Locations */
    #define `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT    0x00u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT    0x03u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_CTRL_RESET_SHIFT       0x06u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT      0x07u       /* As defined by Verilog Implementation */
    /* Control Register Bit Masks */
    #define `$INSTANCE_NAME`_CTRL_CMPMODE_MASK      (0x07u << `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT)  
    #define `$INSTANCE_NAME`_CTRL_CAPMODE_MASK      (0x03u << `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT)  
    #define `$INSTANCE_NAME`_CTRL_RESET             (0x01u << `$INSTANCE_NAME`_CTRL_RESET_SHIFT)  
    #define `$INSTANCE_NAME`_CTRL_ENABLE            (0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT) 

    /* Status Register Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_CMP_SHIFT       0x00u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_ZERO_SHIFT      0x01u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW_SHIFT  0x02u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW_SHIFT 0x03u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT   0x04u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT  0x05u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP_SHIFT  0x06u       /* As defined by Verilog Implementation */
    /* Status Register Interrupt Enable Bit Locations - UDB Status Interrupt Mask match Status Bit Locations*/
    #define `$INSTANCE_NAME`_STATUS_CMP_INT_EN_MASK_SHIFT       `$INSTANCE_NAME`_STATUS_CMP_SHIFT       
    #define `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK_SHIFT      `$INSTANCE_NAME`_STATUS_ZERO_SHIFT      
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW_INT_EN_MASK_SHIFT  `$INSTANCE_NAME`_STATUS_OVERFLOW_SHIFT  
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW_INT_EN_MASK_SHIFT `$INSTANCE_NAME`_STATUS_UNDERFLOW_SHIFT 
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_EN_MASK_SHIFT   `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT   
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_EN_MASK_SHIFT  `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT  
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP_INT_EN_MASK_SHIFT  `$INSTANCE_NAME`_STATUS_FIFONEMP_SHIFT  
    /* Status Register Bit Masks */                
    #define `$INSTANCE_NAME`_STATUS_CMP             (0x01u << `$INSTANCE_NAME`_STATUS_CMP_SHIFT)  
    #define `$INSTANCE_NAME`_STATUS_ZERO            (0x01u << `$INSTANCE_NAME`_STATUS_ZERO_SHIFT) 
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW        (0x01u << `$INSTANCE_NAME`_STATUS_OVERFLOW_SHIFT) 
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW       (0x01u << `$INSTANCE_NAME`_STATUS_UNDERFLOW_SHIFT) 
    #define `$INSTANCE_NAME`_STATUS_CAPTURE         (0x01u << `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT) 
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL        (0x01u << `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT)
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP        (0x01u << `$INSTANCE_NAME`_STATUS_FIFONEMP_SHIFT)
    /* Status Register Interrupt Bit Masks  - UDB Status Interrupt Mask match Status Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_CMP_INT_EN_MASK            `$INSTANCE_NAME`_STATUS_CMP                    
    #define `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK           `$INSTANCE_NAME`_STATUS_ZERO            
    #define `$INSTANCE_NAME`_STATUS_OVERFLOW_INT_EN_MASK       `$INSTANCE_NAME`_STATUS_OVERFLOW        
    #define `$INSTANCE_NAME`_STATUS_UNDERFLOW_INT_EN_MASK      `$INSTANCE_NAME`_STATUS_UNDERFLOW       
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_EN_MASK        `$INSTANCE_NAME`_STATUS_CAPTURE         
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_EN_MASK       `$INSTANCE_NAME`_STATUS_FIFOFULL        
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP_INT_EN_MASK       `$INSTANCE_NAME`_STATUS_FIFONEMP         
    

    /* StatusI Interrupt Enable bit Location in the Auxilliary Control Register */
    #define `$INSTANCE_NAME`_STATUS_ACTL_INT_EN     0x10u /* As defined for the ACTL Register */
    
    /* Datapath Auxillary Control Register definitions */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR         0x01u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_CLR         0x02u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_LVL         0x04u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_LVL         0x08u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK    0x10u   /* As defined for the ACTL Register */
    
#endif /* `$INSTANCE_NAME`_UsingFixedFunction */

#endif  /* CY_COUNTER_`$INSTANCE_NAME`_H */
