/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Contains the function prototypes and constants available to the counter
*   user module.
*
*   Note:
*    None
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
    
#if !defined(CY_COUNTER_`$INSTANCE_NAME`_H)
#define CY_COUNTER_`$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
* Conditional Compilation Parameters
***************************************/

/* PSoC3 ES2 or early */
#define `$INSTANCE_NAME`_PSOC3_ES2  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)    && \
                                    (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_3A_ES2))
/* PSoC5 ES1 or early */
#define `$INSTANCE_NAME`_PSOC5_ES1  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A)    && \
                                    (CYDEV_CHIP_REVISION_USED <= CYDEV_CHIP_REVISION_5A_ES1))
/* PSoC3 ES3 or later */
#define `$INSTANCE_NAME`_PSOC3_ES3  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A)    && \
                                    (CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_3A_ES2))
/* PSoC5 ES2 or later */
#define `$INSTANCE_NAME`_PSOC5_ES2  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A)    && \
                                    (CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_5A_ES1))


/**************************************
*           Parameter Defaults        
**************************************/

#define `$INSTANCE_NAME`_Resolution            `$Resolution`u
#define `$INSTANCE_NAME`_UsingFixedFunction    `$FixedFunctionUsed`u
#define `$INSTANCE_NAME`_ControlRegRemoved     `$ControlRegRemoved`u
#define `$INSTANCE_NAME`_COMPARE_MODE_SOFTWARE `$CompareModeSoftware`u
#define `$INSTANCE_NAME`_CAPTURE_MODE_SOFTWARE `$CaptureModeSoftware`u
#define `$INSTANCE_NAME`_RunModeUsed           `$RunMode`u


/***************************************
*       Type defines
***************************************/

/* Sleep Mode API Support */
typedef struct `$INSTANCE_NAME`_backupStruct
{
    #if (!`$INSTANCE_NAME`_ControlRegRemoved)
        #if (!`$INSTANCE_NAME`_UsingFixedFunction)
            uint8 control;
        #endif
        
        uint8 enableState;
    #endif   
    
    #if (!`$INSTANCE_NAME`_UsingFixedFunction)
        `$RegSizeReplacementString` counterUdb;
        `$RegSizeReplacementString` captureValue;
    #endif
}`$INSTANCE_NAME`_backupStruct;


/**************************************
 *  Function Prototypes
 *************************************/
void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void    `$INSTANCE_NAME`_SetInterruptMode(uint8 interruptsMask) `=ReentrantKeil($INSTANCE_NAME . "_SetInterruptMode")`;
uint8   `$INSTANCE_NAME`_GetInterruptSource(void) `=ReentrantKeil($INSTANCE_NAME . "_GetInterruptSource")`;
uint8   `$INSTANCE_NAME`_ReadStatusRegister(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadStatusRegister")`;
#if(!`$INSTANCE_NAME`_ControlRegRemoved)
    uint8   `$INSTANCE_NAME`_ReadControlRegister(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadControlRegister")`;
    void    `$INSTANCE_NAME`_WriteControlRegister(uint8 control) \
        `=ReentrantKeil($INSTANCE_NAME . "_WriteControlRegister")`;
#endif
void    `$INSTANCE_NAME`_WriteCounter(`$RegSizeReplacementString` counter) \
    `=ReentrantKeil($INSTANCE_NAME . "_WriteCounter")`; 
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCounter(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadCounter")`;
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCapture(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadCapture")`;
void    `$INSTANCE_NAME`_WritePeriod(`$RegSizeReplacementString` period) \
    `=ReentrantKeil($INSTANCE_NAME . "_WritePeriod")`;
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadPeriod( void ) `=ReentrantKeil($INSTANCE_NAME . "_ReadPeriod")`;
#if (!`$INSTANCE_NAME`_UsingFixedFunction)
    void    `$INSTANCE_NAME`_WriteCompare(`$RegSizeReplacementString` compare) \
        `=ReentrantKeil($INSTANCE_NAME . "_WriteCompare")`;
    `$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCompare( void ) \
        `=ReentrantKeil($INSTANCE_NAME . "_ReadCompare")`;
#endif

#if (`$INSTANCE_NAME`_COMPARE_MODE_SOFTWARE)
    void    `$INSTANCE_NAME`_SetCompareMode(uint8 comparemode) `=ReentrantKeil($INSTANCE_NAME . "_SetCompareMode")`;
#endif
#if (`$INSTANCE_NAME`_CAPTURE_MODE_SOFTWARE)
    void    `$INSTANCE_NAME`_SetCaptureMode(uint8 capturemode) `=ReentrantKeil($INSTANCE_NAME . "_SetCaptureMode")`;
#endif
void `$INSTANCE_NAME`_ClearFIFO(void)     `=ReentrantKeil($INSTANCE_NAME . "_ClearFIFO")`;
void `$INSTANCE_NAME`_Init(void)          `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void)        `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void `$INSTANCE_NAME`_SaveConfig(void)    `=ReentrantKeil($INSTANCE_NAME . "_SaveConfig")`;
void `$INSTANCE_NAME`_RestoreConfig(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreConfig")`;
void `$INSTANCE_NAME`_Sleep(void)         `=ReentrantKeil($INSTANCE_NAME . "_Sleep")`;
void `$INSTANCE_NAME`_Wakeup(void)        `=ReentrantKeil($INSTANCE_NAME . "_Wakeup")`;

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
#define `$INSTANCE_NAME`_INIT_PERIOD_VALUE       `$Period`u
#define `$INSTANCE_NAME`_INIT_COUNTER_VALUE      `$InitCounterValue`u
#if (`$INSTANCE_NAME`_UsingFixedFunction)
#define `$INSTANCE_NAME`_INIT_INTERRUPTS_MASK    ((`$InterruptOnTC`u << `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK_SHIFT))
#else
#define `$INSTANCE_NAME`_INIT_COMPARE_VALUE      `$CompareValue`u
#define `$INSTANCE_NAME`_INIT_INTERRUPTS_MASK ((`$InterruptOnTC`u << `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK_SHIFT) | \
        (`$InterruptOnCapture`u << `$INSTANCE_NAME`_STATUS_CAPTURE_INT_EN_MASK_SHIFT) | \
        (`$InterruptOnCompare`u << `$INSTANCE_NAME`_STATUS_CMP_INT_EN_MASK_SHIFT) | \
        (`$InterruptOnOverUnderFlow`u << `$INSTANCE_NAME`_STATUS_OVERFLOW_INT_EN_MASK_SHIFT) | \
        (`$InterruptOnOverUnderFlow`u << `$INSTANCE_NAME`_STATUS_UNDERFLOW_INT_EN_MASK_SHIFT))
#define `$INSTANCE_NAME`_DEFAULT_COMPARE_MODE    (`$CompareMode`u << `$INSTANCE_NAME`_CTRL_CMPMODE0_SHIFT)
#define `$INSTANCE_NAME`_DEFAULT_CAPTURE_MODE    (`$CaptureMode`u << `$INSTANCE_NAME`_CTRL_CAPMODE0_SHIFT)
#endif /* (`$INSTANCE_NAME`_UsingFixedFunction) */

/**************************************
 *  Registers
 *************************************/
#if (`$INSTANCE_NAME`_UsingFixedFunction)
    #define `$INSTANCE_NAME`_STATICCOUNT_LSB     (*(reg16 *) `$INSTANCE_NAME`_CounterHW__CAP0 )
    #define `$INSTANCE_NAME`_STATICCOUNT_LSB_PTR ( (reg16 *) `$INSTANCE_NAME`_CounterHW__CAP0 )
    #define `$INSTANCE_NAME`_PERIOD_LSB          (*(reg16 *) `$INSTANCE_NAME`_CounterHW__PER0 )
    #define `$INSTANCE_NAME`_PERIOD_LSB_PTR      ( (reg16 *) `$INSTANCE_NAME`_CounterHW__PER0 )
    /* MODE must be set to 1 to set the compare value */
    #define `$INSTANCE_NAME`_COMPARE_LSB         (*(reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 )
    #define `$INSTANCE_NAME`_COMPARE_LSB_PTR     ( (reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 )
    /* MODE must be set to 0 to get the count */
    #define `$INSTANCE_NAME`_COUNTER_LSB         (*(reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 )
    #define `$INSTANCE_NAME`_COUNTER_LSB_PTR     ( (reg16 *) `$INSTANCE_NAME`_CounterHW__CNT_CMP0 )
#else
    #define `$INSTANCE_NAME`_STATICCOUNT_LSB     (*(`$RegDefReplacementString` *) \
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__F0_REG )
    #define `$INSTANCE_NAME`_STATICCOUNT_LSB_PTR ( (`$RegDefReplacementString` *) \
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__F0_REG )
    #define `$INSTANCE_NAME`_PERIOD_LSB          (*(`$RegDefReplacementString` *) \
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D0_REG )
    #define `$INSTANCE_NAME`_PERIOD_LSB_PTR      ( (`$RegDefReplacementString` *) \
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D0_REG )
    #define `$INSTANCE_NAME`_COMPARE_LSB         (*(`$RegDefReplacementString` *) \
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D1_REG )
    #define `$INSTANCE_NAME`_COMPARE_LSB_PTR     ( (`$RegDefReplacementString` *) \
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__D1_REG )
    #define `$INSTANCE_NAME`_COUNTER_LSB         (*(`$RegDefReplacementString` *) \
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__A0_REG )
    #define `$INSTANCE_NAME`_COUNTER_LSB_PTR     ( (`$RegDefReplacementString` *)\
        `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__A0_REG )

	#define `$INSTANCE_NAME`_AUX_CONTROLDP0 \
        (*(reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__DP_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_AUX_CONTROLDP0_PTR \
        ( (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u0__DP_AUX_CTL_REG)
    #if (`$INSTANCE_NAME`_Resolution == 16 || `$INSTANCE_NAME`_Resolution == 24 || `$INSTANCE_NAME`_Resolution == 32)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP1 \
           (*(reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u1__DP_AUX_CTL_REG)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP1_PTR \
           ( (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u1__DP_AUX_CTL_REG)
    #endif
    #if (`$INSTANCE_NAME`_Resolution == 24 || `$INSTANCE_NAME`_Resolution == 32)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP2 \
           (*(reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u2__DP_AUX_CTL_REG)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP2_PTR \
           ( (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u2__DP_AUX_CTL_REG)
    #endif
    #if (`$INSTANCE_NAME`_Resolution == 32)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP3 \
           (*(reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u3__DP_AUX_CTL_REG)
       #define `$INSTANCE_NAME`_AUX_CONTROLDP3_PTR \
           ( (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$VerilogSectionReplacementString`_counterdp_u3__DP_AUX_CTL_REG)
    #endif
#endif  /* (`$INSTANCE_NAME`_UsingFixedFunction) */

#if (`$INSTANCE_NAME`_UsingFixedFunction)
    #define `$INSTANCE_NAME`_STATUS         (*(reg8 *) `$INSTANCE_NAME`_CounterHW__SR0 )
    /* In Fixed Function Block Status and Mask are the same register */
    #define `$INSTANCE_NAME`_STATUS_MASK             (*(reg8 *) `$INSTANCE_NAME`_CounterHW__SR0 )
    #define `$INSTANCE_NAME`_STATUS_MASK_PTR         ( (reg8 *) `$INSTANCE_NAME`_CounterHW__SR0 )
    #define `$INSTANCE_NAME`_CONTROL                 (*(reg8 *) `$INSTANCE_NAME`_CounterHW__CFG0)
    #define `$INSTANCE_NAME`_CONTROL_PTR             ( (reg8 *) `$INSTANCE_NAME`_CounterHW__CFG0)
    #define `$INSTANCE_NAME`_CONTROL2                (*(reg8 *) `$INSTANCE_NAME`_CounterHW__CFG1)
    #define `$INSTANCE_NAME`_CONTROL2_PTR            ( (reg8 *) `$INSTANCE_NAME`_CounterHW__CFG1)
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        #define `$INSTANCE_NAME`_CONTROL3       (*(reg8 *) `$INSTANCE_NAME`_CounterHW__CFG2)
        #define `$INSTANCE_NAME`_CONTROL3_PTR   ( (reg8 *) `$INSTANCE_NAME`_CounterHW__CFG2)
    #endif
    #define `$INSTANCE_NAME`_GLOBAL_ENABLE           (*(reg8 *) `$INSTANCE_NAME`_CounterHW__PM_ACT_CFG)
    #define `$INSTANCE_NAME`_GLOBAL_ENABLE_PTR       ( (reg8 *) `$INSTANCE_NAME`_CounterHW__PM_ACT_CFG)
    #define `$INSTANCE_NAME`_GLOBAL_STBY_ENABLE      (*(reg8 *) `$INSTANCE_NAME`_CounterHW__PM_STBY_CFG)
    #define `$INSTANCE_NAME`_GLOBAL_STBY_ENABLE_PTR  ( (reg8 *) `$INSTANCE_NAME`_CounterHW__PM_STBY_CFG)
    
    /********************************
    *    Constants
    ********************************/
    /* Fixed Function Block Chosen */
    #define `$INSTANCE_NAME`_BLOCK_EN_MASK          `$INSTANCE_NAME`_CounterHW__PM_ACT_MSK
    #define `$INSTANCE_NAME`_BLOCK_STBY_EN_MASK     `$INSTANCE_NAME`_CounterHW__PM_STBY_MSK 
    
    /* Control Register Bit Locations */    
    /* As defined in Register Map, part of TMRX_CFG0 register */
    #define `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT      0x00u
    /* Control Register Bit Masks */
    #define `$INSTANCE_NAME`_CTRL_ENABLE            (0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT)         

    /* Control2 Register Bit Masks */
    /* Set the mask for run mode */
    #if (`$INSTANCE_NAME`_PSOC3_ES2 || `$INSTANCE_NAME`_PSOC5_ES1)
        /* Use CFG1 Mode bits to set run mode */
        #define `$INSTANCE_NAME`_CTRL_MODE_SHIFT        0x01u    
        #define `$INSTANCE_NAME`_CTRL_MODE_MASK         (0x07u << `$INSTANCE_NAME`_CTRL_MODE_SHIFT)
    #endif
    #if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
        /* Use CFG2 Mode bits to set run mode */
        #define `$INSTANCE_NAME`_CTRL_MODE_SHIFT        0x00u    
        #define `$INSTANCE_NAME`_CTRL_MODE_MASK         (0x03u << `$INSTANCE_NAME`_CTRL_MODE_SHIFT)
    #endif
    /* Set the mask for interrupt (raw/status register) */
    #define `$INSTANCE_NAME`_CTRL2_IRQ_SEL_SHIFT     0x00u
    #define `$INSTANCE_NAME`_CTRL2_IRQ_SEL          (0x01u << `$INSTANCE_NAME`_CTRL2_IRQ_SEL_SHIFT)     
    
    /* Status Register Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_ZERO_SHIFT      0x07u  /* As defined in Register Map, part of TMRX_SR0 register */ 

    /* Status Register Interrupt Enable Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK_SHIFT      (`$INSTANCE_NAME`_STATUS_ZERO_SHIFT - 0x04u)

    /* Status Register Bit Masks */                           
    #define `$INSTANCE_NAME`_STATUS_ZERO            (0x01u << `$INSTANCE_NAME`_STATUS_ZERO_SHIFT)

    /* Status Register Interrupt Bit Masks*/
    #define `$INSTANCE_NAME`_STATUS_ZERO_INT_EN_MASK       (`$INSTANCE_NAME`_STATUS_ZERO >> 0x04u)

#else /* !`$INSTANCE_NAME`_UsingFixedFunction */
    #define `$INSTANCE_NAME`_STATUS               (* (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$RstStatusReplacementString`_stsreg__STATUS_REG )
    #define `$INSTANCE_NAME`_STATUS_PTR           (  (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$RstStatusReplacementString`_stsreg__STATUS_REG )
    #define `$INSTANCE_NAME`_STATUS_MASK          (* (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$RstStatusReplacementString`_stsreg__MASK_REG )
    #define `$INSTANCE_NAME`_STATUS_MASK_PTR      (  (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$RstStatusReplacementString`_stsreg__MASK_REG )
    #define `$INSTANCE_NAME`_STATUS_AUX_CTRL      (*(reg8 *) `$INSTANCE_NAME`_CounterUDB_`$RstStatusReplacementString`_stsreg__STATUS_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_STATUS_AUX_CTRL_PTR  ( (reg8 *) `$INSTANCE_NAME`_CounterUDB_`$RstStatusReplacementString`_stsreg__STATUS_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_CONTROL              (* (reg8 *) `$INSTANCE_NAME`_CounterUDB_sCTRLReg_ctrlreg__CONTROL_REG )
    #define `$INSTANCE_NAME`_CONTROL_PTR          (  (reg8 *) `$INSTANCE_NAME`_CounterUDB_sCTRLReg_ctrlreg__CONTROL_REG )

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


/* [] END OF FILE */

