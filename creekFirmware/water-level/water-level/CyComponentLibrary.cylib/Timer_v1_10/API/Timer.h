/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     Contains the function prototypes and constants available to the timer
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

#if !defined(CY_TIMER_v1_10_`$INSTANCE_NAME`_H)
#define CY_TIMER_v1_10_`$INSTANCE_NAME`_H
#define `$INSTANCE_NAME`_Resolution `$Resolution`

/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_UsingFixedFunction         `$FixedFunctionUsed`
#define `$INSTANCE_NAME`_UsingHWCaptureCounter      `$HWCaptureCounterEnabled`
#define `$INSTANCE_NAME`_SoftwareCaptureMode        `$SoftwareCaptureModeEnabled`
#define `$INSTANCE_NAME`_SoftwareTriggerMode        `$SoftwareTriggerModeEnabled`
#define `$INSTANCE_NAME`_UsingHWEnable              `$UsesHWEnable`
#define `$INSTANCE_NAME`_EnableTriggerMode          `$TriggerInputEnabled` 
#define `$INSTANCE_NAME`_InterruptOnCaptureCount    `$InterruptOnCapture`

/*************************************** 
*       Function Prototypes 
***************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);

void    `$INSTANCE_NAME`_SetInterruptMode(uint8 interruptsource);
uint8   `$INSTANCE_NAME`_GetInterruptSource(void);

uint8   `$INSTANCE_NAME`_ReadStatusRegister(void);
uint8   `$INSTANCE_NAME`_ReadControlRegister(void);
void    `$INSTANCE_NAME`_WriteControlRegister(uint8 control);

`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadPeriod(void);
void    `$INSTANCE_NAME`_WritePeriod(`$RegSizeReplacementString` period);
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCounter(void);
void    `$INSTANCE_NAME`_WriteCounter(`$RegSizeReplacementString` counter);
`$RegSizeReplacementString`  `$INSTANCE_NAME`_ReadCapture(void);
void    `$INSTANCE_NAME`_SoftwareCapture(void);


#if(!`$INSTANCE_NAME`_UsingFixedFunction) /* UDB Prototypes */
    #if (`$INSTANCE_NAME`_SoftwareCaptureMode)
        void    `$INSTANCE_NAME`_SetCaptureMode(uint8 capturemode);
    #endif

    #if (`$INSTANCE_NAME`_SoftwareTriggerMode)
        void    `$INSTANCE_NAME`_SetTriggerMode(uint8 triggermode);
    #endif
    #if (`$INSTANCE_NAME`_EnableTriggerMode)
        void    `$INSTANCE_NAME`_EnableTrigger(void);
        void    `$INSTANCE_NAME`_DisableTrigger(void);
    #endif

    #if(`$INSTANCE_NAME`_InterruptOnCaptureCount)
    void    `$INSTANCE_NAME`_SetInterruptCount(uint8 interruptcount);
    #endif

    #if (`$INSTANCE_NAME`_UsingHWCaptureCounter)
    void    `$INSTANCE_NAME`_SetCaptureCount(uint8 capturecount);
    uint8   `$INSTANCE_NAME`_ReadCaptureCount();
    #endif

    void `$INSTANCE_NAME`_ClearFIFO(void);
#endif /* UDB Prototypes */


/***************************************
*   Enumerated Types and Parameters
***************************************/

/* Enumerated Type B_Timer__CaptureModes, Used in Capture Mode */
`#cy_declare_enum B_Timer__CaptureModes`

/* Enumerated Type B_Timer__TriggerModes, Used in Trigger Mode */
#define `$INSTANCE_NAME`__B_TIMER__TM_NONE 0x00
#define `$INSTANCE_NAME`__B_TIMER__TM_RISINGEDGE 0x04
#define `$INSTANCE_NAME`__B_TIMER__TM_FALLINGEDGE 0x08
#define `$INSTANCE_NAME`__B_TIMER__TM_EITHEREDGE 0x0C
#define `$INSTANCE_NAME`__B_TIMER__TM_SOFTWARE 0x10


/***************************************
*    Initialial Parameter Constants
***************************************/

#define `$INSTANCE_NAME`_INIT_PERIOD               `$Period`
#define `$INSTANCE_NAME`_INIT_CAPTURE_MODE         (`$CaptureMode` << `$INSTANCE_NAME`_CTRL_CAP_MODE_SHIFT)
#define `$INSTANCE_NAME`_INIT_TRIGGER_MODE         (`$TriggerMode` << `$INSTANCE_NAME`_CTRL_TRIG_MODE_SHIFT)
#if (`$INSTANCE_NAME`_UsingFixedFunction)
#define `$INSTANCE_NAME`_INIT_INTERRUPT_MODE       ((`$IntOnTC` << `$INSTANCE_NAME`_STATUS_TC_INT_MASK_SHIFT) | (`$IntOnCapture` << `$INSTANCE_NAME`_STATUS_CAPTURE_INT_MASK_SHIFT))
#else
#define `$INSTANCE_NAME`_INIT_INTERRUPT_MODE       ((`$IntOnTC` << `$INSTANCE_NAME`_STATUS_TC_INT_MASK_SHIFT) | (`$IntOnCapture` << `$INSTANCE_NAME`_STATUS_CAPTURE_INT_MASK_SHIFT) | (`$IntOnFIFOFull` << `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_MASK_SHIFT))
#endif
#define `$INSTANCE_NAME`_INIT_CAPTURE_COUNT        (`$CaptureCount`)
#define `$INSTANCE_NAME`_INIT_INT_CAPTURE_COUNT    ((`$NumberOfCaptures` - 1) << `$INSTANCE_NAME`_CTRL_INTCNT_SHIFT)


/***************************************
*           Registers
***************************************/

#if (`$INSTANCE_NAME`_UsingFixedFunction) /* Implementation Specific Registers and Register Constants */

    
    /***************************************
    *    Fixed Function Registers 
    ***************************************/
    
    #define `$INSTANCE_NAME`_STATUS         (*(reg8 *) `$INSTANCE_NAME`_TimerHW__SR0 )
    #define `$INSTANCE_NAME`_STATUS_MASK    (*(reg8 *) `$INSTANCE_NAME`_TimerHW__SR0 ) /* In Fixed Function Block Status and Mask are the same register */
    #define `$INSTANCE_NAME`_CONTROL        (*(reg8 *) `$INSTANCE_NAME`_TimerHW__CFG0)
    #define `$INSTANCE_NAME`_CONTROL2       (*(reg8 *) `$INSTANCE_NAME`_TimerHW__CFG1)
    #define `$INSTANCE_NAME`_GLOBAL_ENABLE  (*(reg8 *) `$INSTANCE_NAME`_TimerHW__PM_ACT_CFG)
    
    #define `$INSTANCE_NAME`_CAPTURE_LSB         (* (reg16 *) `$INSTANCE_NAME`_TimerHW__CAP0 )
    #define `$INSTANCE_NAME`_CAPTURE_LSB_PTR       ((reg16 *) `$INSTANCE_NAME`_TimerHW__CAP0 )
    #define `$INSTANCE_NAME`_PERIOD_LSB          (* (reg16 *) `$INSTANCE_NAME`_TimerHW__PER0 )
    #define `$INSTANCE_NAME`_PERIOD_LSB_PTR        ((reg16 *) `$INSTANCE_NAME`_TimerHW__PER0 )
    #define `$INSTANCE_NAME`_COUNTER_LSB         (* (reg16 *) `$INSTANCE_NAME`_TimerHW__CNT_CMP0 )
    #define `$INSTANCE_NAME`_COUNTER_LSB_PTR       ((reg16 *) `$INSTANCE_NAME`_TimerHW__CNT_CMP0 )
    
    /***************************************
    *    Register Constants
    ***************************************/
    
    /* Fixed Function Block Chosen */
    #define `$INSTANCE_NAME`_BLOCK_EN_MASK                      `$INSTANCE_NAME`_TimerHW__PM_ACT_MSK
    
    /* Control Register Bit Locations */
    #define `$INSTANCE_NAME`_CTRL_INTCNT_SHIFT                  0x00u       /* Interrupt Count - Not valid for Fixed Function Block */
    #define `$INSTANCE_NAME`_CTRL_TRIG_MODE_SHIFT               0x00u       /* Trigger Polarity - Not valid for Fixed Function Block */
    #define `$INSTANCE_NAME`_CTRL_TRIG_EN_SHIFT                 0x00u       /* Trigger Enable - Not valid for Fixed Function Block */
    #define `$INSTANCE_NAME`_CTRL_CAP_MODE_SHIFT                0x00u       /* Capture Polarity - Not valid for Fixed Function Block */ 
    #define `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT                  0x00u       /* Timer Enable - As defined in Register Map, part of TMRX_CFG0 register */
   
    /* Control Register Bit Masks */
    /* `$INSTANCE_NAME`_CTRL_INTCNT_MASK                        Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_CTRL_TRIG_MODE_MASK                     Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_CTRL_TRIG_EN                            Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_CTRL_CAP_MODE_MASK                      Not valid for Fixed Function Block */    
    #define `$INSTANCE_NAME`_CTRL_ENABLE                        (0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT)
    
    /* Control2 Register Bit Masks */
    #define `$INSTANCE_NAME`_CTRL2_IRQ_SEL_SHIFT                 0x00u       /* As defined in Register Map, Part of the TMRX_CFG1 register */
    #define `$INSTANCE_NAME`_CTRL2_IRQ_SEL                      (0x01u << `$INSTANCE_NAME`_CTRL2_IRQ_SEL_SHIFT) 
    
    #define `$INSTANCE_NAME`_CTRL_MODE_SHIFT                     0x01u       /* As defined by Verilog Implementation */ 
    #define `$INSTANCE_NAME`_CTRL_MODE_MASK                     (0x07u << `$INSTANCE_NAME`_CTRL_MODE_SHIFT) 
    #define `$INSTANCE_NAME`_CTRL_MODE_PULSEWIDTH               (0x01u << `$INSTANCE_NAME`_CTRL_MODE_SHIFT) 
    #define `$INSTANCE_NAME`_CTRL_MODE_PERIOD                   (0x02u << `$INSTANCE_NAME`_CTRL_MODE_SHIFT) 
    #define `$INSTANCE_NAME`_CTRL_MODE_CONTINUOUS               (0x00u << `$INSTANCE_NAME`_CTRL_MODE_SHIFT) 
    
    /* Status Register Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_TC_SHIFT                    0x07u  /* As defined in Register Map, part of TMRX_SR0 register */ 
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT               0x06u  /* As defined in Register Map, part of TMRX_SR0 register, Shared with Compare Status */
    #define `$INSTANCE_NAME`_STATUS_TC_INT_MASK_SHIFT           (`$INSTANCE_NAME`_STATUS_TC_SHIFT - 4)      /* As defined in Register Map, part of TMRX_SR0 register */ 
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_MASK_SHIFT      (`$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT - 4) /* As defined in Register Map, part of TMRX_SR0 register, Shared with Compare Status */ 
    /* `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT                   Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_STATUS_FIFONEMP_SHIFT                   Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_MASK_SHIFT          Not valid for Fixed Function Block */
    
    /* Status Register Bit Masks */
    #define `$INSTANCE_NAME`_STATUS_TC                          (0x01u << `$INSTANCE_NAME`_STATUS_TC_SHIFT)
    #define `$INSTANCE_NAME`_STATUS_CAPTURE                     (0x01u << `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT)
    #define `$INSTANCE_NAME`_STATUS_TC_INT_MASK                 (0x01u << `$INSTANCE_NAME`_STATUS_TC_INT_MASK_SHIFT)      /* Interrupt Enable Bit-Mask */ 
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_MASK            (0x01u << `$INSTANCE_NAME`_STATUS_CAPTURE_INT_MASK_SHIFT) /* Interrupt Enable Bit-Mask */ 
    /* `$INSTANCE_NAME`_STATUS_FIFOFULL                         Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_STATUS_FIFONEMP                         Not valid for Fixed Function Block */
    /* `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_MASK                Not valid for Fixed Function Block */
    
     /* Datapath Auxillary Control Register definitions */
    /* `$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR                      Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_AUX_CTRL_FIFO1_CLR                      Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_AUX_CTRL_FIFO0_LVL                      Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_AUX_CTRL_FIFO1_LVL                      Not valid for Fixed Function Block */ 
    /* `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK                 Not valid for Fixed Function Block */  

#else   /* UDB Registers and Register Constants */

    
    /***************************************
    *           UDB Registers 
    ***************************************/
    
    #define `$INSTANCE_NAME`_STATUS              (* (reg8 *) `$INSTANCE_NAME`_TimerUDB_stsreg__STATUS_REG )
    #define `$INSTANCE_NAME`_STATUS_MASK         (* (reg8 *) `$INSTANCE_NAME`_TimerUDB_stsreg__MASK_REG)
    #define `$INSTANCE_NAME`_STATUS_AUX_CTRL     (* (reg8 *) `$INSTANCE_NAME`_TimerUDB_stsreg__STATUS_AUX_CTL_REG)
    #define `$INSTANCE_NAME`_CONTROL             (* (reg8 *) `$INSTANCE_NAME`_TimerUDB_ctrlreg__CONTROL_REG )
    
    #define `$INSTANCE_NAME`_CAPTURE_LSB         (* (`$RegDefReplacementString` *) `$INSTANCE_NAME`_TimerUDB_`$VerilogSectionReplacementString`_timerdp_u0__F0_REG )
    #define `$INSTANCE_NAME`_CAPTURE_LSB_PTR       ((`$RegDefReplacementString` *) `$INSTANCE_NAME`_TimerUDB_`$VerilogSectionReplacementString`_timerdp_u0__F0_REG )
    #define `$INSTANCE_NAME`_PERIOD_LSB          (* (`$RegDefReplacementString` *) `$INSTANCE_NAME`_TimerUDB_`$VerilogSectionReplacementString`_timerdp_u0__D0_REG )
    #define `$INSTANCE_NAME`_PERIOD_LSB_PTR        ((`$RegDefReplacementString` *) `$INSTANCE_NAME`_TimerUDB_`$VerilogSectionReplacementString`_timerdp_u0__D0_REG )
    #define `$INSTANCE_NAME`_COUNTER_LSB         (* (`$RegDefReplacementString` *) `$INSTANCE_NAME`_TimerUDB_`$VerilogSectionReplacementString`_timerdp_u0__A0_REG )
    #define `$INSTANCE_NAME`_COUNTER_LSB_PTR       ((`$RegDefReplacementString` *) `$INSTANCE_NAME`_TimerUDB_`$VerilogSectionReplacementString`_timerdp_u0__A0_REG )

    #if (`$INSTANCE_NAME`_UsingHWCaptureCounter)
        #define `$INSTANCE_NAME`_CAP_COUNT              (*(reg8 *) `$INSTANCE_NAME`_TimerUDB_sCapCount_counter__PERIOD_REG )
        #define `$INSTANCE_NAME`_CAP_COUNT_PTR          ( (reg8 *) `$INSTANCE_NAME`_TimerUDB_sCapCount_counter__PERIOD_REG )
        #define `$INSTANCE_NAME`_CAPTURE_COUNT_CTRL     (*(reg8 *) `$INSTANCE_NAME`_TimerUDB_sCapCount_counter__CONTROL_AUX_CTL_REG )
        #define `$INSTANCE_NAME`_CAPTURE_COUNT_CTRL_PTR ( (reg8 *) `$INSTANCE_NAME`_TimerUDB_sCapCount_counter__CONTROL_AUX_CTL_REG )
    #endif


    /***************************************
    *       Register Constants
    ***************************************/
    
    /* Control Register Bit Locations */
    #define `$INSTANCE_NAME`_CTRL_INTCNT_SHIFT              0x00u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_CTRL_TRIG_MODE_SHIFT           0x02u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_CTRL_TRIG_EN_SHIFT             0x04u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_CTRL_CAP_MODE_SHIFT            0x05u       /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT              0x07u       /* As defined by Verilog Implementation */
    
    /* Control Register Bit Masks */
    #define `$INSTANCE_NAME`_CTRL_INTCNT_MASK               (0x03u << `$INSTANCE_NAME`_CTRL_INTCNT_SHIFT)
    #define `$INSTANCE_NAME`_CTRL_TRIG_MODE_MASK            (0x03u << `$INSTANCE_NAME`_CTRL_TRIG_MODE_SHIFT)  
    #define `$INSTANCE_NAME`_CTRL_TRIG_EN                   (0x01u << `$INSTANCE_NAME`_CTRL_TRIG_EN_SHIFT)
    #define `$INSTANCE_NAME`_CTRL_CAP_MODE_MASK             (0x03u << `$INSTANCE_NAME`_CTRL_CAP_MODE_SHIFT)   
    #define `$INSTANCE_NAME`_CTRL_ENABLE                    (0x01u << `$INSTANCE_NAME`_CTRL_ENABLE_SHIFT)
    
    /* Bit Counter (7-bit) Control Register Bit Definitions */
    #define `$INSTANCE_NAME`_CNTR_ENABLE                    0x20u   /* As defined by the Register map for the AUX Control Register */
    
    /* Status Register Bit Locations */
    #define `$INSTANCE_NAME`_STATUS_TC_SHIFT                0x00u  /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT           0x01u  /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_TC_INT_MASK_SHIFT       `$INSTANCE_NAME`_STATUS_TC_SHIFT                
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_MASK_SHIFT  `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT          0x02u  /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP_SHIFT          0x03u  /* As defined by Verilog Implementation */
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_MASK_SHIFT `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT

    /* Status Register Bit Masks */
    #define `$INSTANCE_NAME`_STATUS_TC                      (0x01u << `$INSTANCE_NAME`_STATUS_TC_SHIFT)             /* Sticky TC Event Bit-Mask */ 
    #define `$INSTANCE_NAME`_STATUS_CAPTURE                 (0x01u << `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT)        /* Sticky Capture Event Bit-Mask */ 
    #define `$INSTANCE_NAME`_STATUS_TC_INT_MASK             (0x01u << `$INSTANCE_NAME`_STATUS_TC_SHIFT)             /* Interrupt Enable Bit-Mask */            
    #define `$INSTANCE_NAME`_STATUS_CAPTURE_INT_MASK        (0x01u << `$INSTANCE_NAME`_STATUS_CAPTURE_SHIFT)        /* Interrupt Enable Bit-Mask */ 
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL                (0x01u << `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT)       /* NOT-Sticky FIFO Full Bit-Mask */ 
    #define `$INSTANCE_NAME`_STATUS_FIFONEMP                (0x01u << `$INSTANCE_NAME`_STATUS_FIFONEMP_SHIFT)       /* NOT-Sticky FIFO Not Empty Bit-Mask */ 
    #define `$INSTANCE_NAME`_STATUS_FIFOFULL_INT_MASK       (0x01u << `$INSTANCE_NAME`_STATUS_FIFOFULL_SHIFT)       /* Interrupt Enable Bit-Mask */ 
    
    #define `$INSTANCE_NAME`_STATUS_ACTL_INT_EN             0x10u   /* As defined for the ACTL Register */
    
    /* Datapath Auxillary Control Register definitions */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_CLR             0x01u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_CLR             0x02u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO0_LVL             0x04u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_AUX_CTRL_FIFO1_LVL             0x08u   /* As defined by Register map */
    #define `$INSTANCE_NAME`_STATUS_ACTL_INT_EN_MASK        0x10u   /* As defined for the ACTL Register */
    
#endif /* Implementation Specific Registers and Register Constants */

#endif  /* CY_TIMER_v1_10_`$INSTANCE_NAME`_H */

