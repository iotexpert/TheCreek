/*******************************************************************************
* File Name: PrISM_v2_0.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides a top level model of the PrISM user module
*  defining and all of the necessary interconnect.
*
* Note:
*  The PrISM component provides modulation technology that significantly reduces 
*  low-frequency flicker and radiated electro-magnetic interference (EMI) which
*  are common problems with High Brightness LED designs. PrISM is also useful in 
*  other applications requiring this benefit such as motor control and power 
*  supplies.
*
********************************************************************************
* Control and Status Register definitions
********************************************************************************
* Control Register Definition
*  +======+------+------+------+------+------+---------+---------+--------+
*  |  Bit |  7   |  6   |  5   |  4   |  3   |   2     |   1     |   0    |
*  +======+------+------+------+------+------+---------+---------+--------+
*  | Desc |unused|unused|unused|unused|unused|CompType1|CompType0| Enable | 
*  +======+------+------+------+------+------+---------+---------+--------+
*
*  enable    => 0 = Disable PrISM
*               1 = enable PrISM
*  CompType0 => 0 = Less Than or Equal for PulseDensity0. 
*               1 = Greater Than or Equal for PulseDensity0.
*  CompType1 => 0 = Less Than or Equal for PulseDensity1. 
*               1 = Greater Than or Equal for PulseDensity1.
*                      
********************************************************************************
* Data Path register definitions
********************************************************************************
* INSTANCE NAME: PrISMdp
* DESCRIPTION: Implements the PRiSM 8-Bit U0 only; 16-bit U0 = LSB, U1 = MSB; etc
* REGISTER USAGE:
* F0 => copy from sequence initial value for store to A0(Seed) at Reset time
* F1 => na
* D0 => PulseDensity0 register
* D1 => PulseDensity1 register
* A0 => Contain the initial (Seed) value and PRS residual value at the end of the computation
* A1 => Polynomial register
*
* Data Path States
*  0 0 0   0   Idle
*  0 0 1   1   Computation PRS 
*  0 1 0   2   RESET
*  0 1 1   3   RESET
********************************************************************************
* I*O Signals:
********************************************************************************
* IO SIGNALS:
*  signal name direction Description
*
*  enable      input     Enable input provides synchronized operation with other components
*                        Active - High
*                        Default - Enabled, tied to high logic level
*                        Use  - Hardware enable allowing the PrISM to run
*  kill        input     Kill input disables pulse density outputs and forces them low
*                        Active - High
*                        Default - Not Killed, tied to low logic level
*                        Use - Immediately disables outputs 0 and 1. Outputs are re-enabled when kill is released.
*  reset       input     Reset input allows restart at sequence start value for synchronization with other components
*                        Active - High
*                        Default - Not Reset, tied to low logic level
*                        Use - Reset PrISM on reset pulse and continue running
*  clock       input     Data clock
* 
*  pulse_den0  output    Registered output of the PulseDensity0
*  pulse_den1  output    Registered output of the PulseDensity1
*  bitstream   output    Registered output of the LFSR LSb
*  tc          output    Single Clock pulse on Terminal Count
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef PrISM_v2_0_V_ALREADY_INCLUDED
`else
`define PrISM_v2_0_V_ALREADY_INCLUDED

module PrISM_v2_0
(
    input wire clock,
    input wire enable,
    input wire kill,
    input wire reset,
    output wire bitstream,
    output reg pulse_den0,
    output reg pulse_den1,
    output wire tc
);
    
    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
    localparam PRS_8_BIT  = 6'd8;
    localparam PRS_16_BIT = 6'd16;
    localparam PRS_24_BIT = 6'd24;
    localparam PRS_32_BIT = 6'd32;
    
    localparam LESS_THEN_OR_EQUAL    = 1'd0;
    localparam GREATER_THAN_OR_EQUAL = 1'd1;
    
    parameter  [5:0] Resolution = PRS_8_BIT;    
    parameter CompareType0 = LESS_THEN_OR_EQUAL;
    parameter CompareType1 = LESS_THEN_OR_EQUAL;
    parameter PulseTypeHardcoded = 1'd0;                /*This parameter saves recouses when enabled, 
                                                but release possibility to change Pulse Type by API.*/
    
    /***************************************************************************
    *            Device Family and Silicon Revision definitions 
    ***************************************************************************/
    /* PSoC3 ES2 or earlier */
    localparam PSOC3_ES2  = ((`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_3A) && 
                             (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_3A_ES2));
    /* PSoC5 ES1 or earlier */                        
    localparam PSOC5_ES1  = ((`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A) && 
                             (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_5A_ES1));


    
    /**************************************************************************
    * Control Register Implementation                                        *
    **************************************************************************/
    /* Control Register Bits (Bits 7-3 are unused )*/
    localparam PRS_CTRL_ENABLE        = 3'h0;    /* Enable PRS   */          
    localparam PRS_CTRL_COMPARE_TYPE0 = 3'h1;    /* CompareType0 */   
    localparam PRS_CTRL_COMPARE_TYPE1 = 3'h2;    /* CompareType1 */ 
    
    wire [7:0] control;                          /* Control Register Output */    
    
    /* Control Signals */
    wire       ctrl_enable      = PulseTypeHardcoded ? 1'd1 : control[PRS_CTRL_ENABLE];
    wire       compare_type0    = PulseTypeHardcoded ? CompareType0 : control[PRS_CTRL_COMPARE_TYPE0];
    wire       compare_type1    = PulseTypeHardcoded ? CompareType1 : control[PRS_CTRL_COMPARE_TYPE1];
    
    /* The clock to operate Control Reg for ES3 must be synchronous and run
    *  continuosly. In this case the udb_clock_enable is used only for 
    *  synchronization. The resulted clock is always enabled.
    *  Used also for Pulse Density outputs and Enable input synchronization
    */ 
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) CtlClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(clock_cnt)
    );  

    /* Add support of sync mode for PSoC3 ES3 Rev */
    generate
    if( (PulseTypeHardcoded == 1'd0) & (PSOC3_ES2 || PSOC5_ES1) )
    begin: AsyncCtl
        cy_psoc3_control #(.cy_force_order(1)) ControlReg
        (
            /* output [07:00] */  .control(control)
        );
    end /* AsyncCtl */
    else if(PulseTypeHardcoded == 1'd0) /*support of sync mode for PSoC3 ES3 Rev */
    begin: SyncCtl
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h0), .cy_ctrl_mode_0(8'h7)) ControlReg
        (
            /* input          */  .clock(clock_cnt),
            /* output [07:00] */  .control(control)
        );
    end /* SyncCtl */
    endgenerate
    
    /**************************************************************************/
    /* Enable signal synchonization using clock_cnt                           */
    /**************************************************************************/
    reg enable_final_reg;
    always @(posedge clock_cnt) 
    begin
        enable_final_reg <= ctrl_enable & enable;
    end

   /***************************************************************************
    *         Instantiation of udb_clock_enable  
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable 
    * mechanism and specify the intended synchronization behaviour for the clock 
    * result (operational clock).
    */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(enable_final_reg),
        /* output */    .clock_out(clock_op)
    );       
    
    
    /**************************************************************************/
    /* Reset signal synchonization for DP usage                               */
    /**************************************************************************/
    reg reset_reg;
    always @(posedge clock_op) 
    begin
        reset_reg <= reset;
    end

    
    /**************************************************************************/
    /* Instantiate the data path elements                                     */
    /**************************************************************************/
    localparam [2:0] dpMsbVal = (Resolution <= 8)  ? (Resolution - 6'd1) : 
                                (Resolution <= 16) ? (Resolution - 6'd9) :
                                (Resolution <= 24) ? (Resolution - 6'd17) :
                                (Resolution <= 32) ? (Resolution - 6'd25) : 3'd0;
                            
    wire [2:0] cs_addr = {1'b0, reset_reg, 1'b1};
    

    /**************************************************************************/
    /* Generate Pulse Density signals                                         */
    /**************************************************************************/
    wire ce0,cl0,ce1,cl1;
    wire Pd0a = (ce0|cl0) & ~compare_type0;   /* LESS_THEN_OR_EQUAL */
    wire Pd0b = ~cl0 & compare_type0;         /* GREATER_THAN_OR_EQUAL */
    wire Pd1a = (ce1|cl1) & ~compare_type1;   /* LESS_THEN_OR_EQUAL */
    wire Pd1b = ~cl1 & compare_type1;         /* GREATER_THAN_OR_EQUAL */
    
    always @(posedge clock_op) 
    begin
        if(!kill & !reset_reg)
        begin
            pulse_den0 <= Pd0a | Pd0b;
            pulse_den1 <= Pd1a | Pd1b;
        end
        else
        begin
            pulse_den0 <= 0;
            pulse_den1 <= 0;
        end
    end
    
    generate
    if (Resolution <= PRS_8_BIT) 
    begin : sC8
    cy_psoc3_dp8 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) PrISMdp(
    /*  input                   */  .clk(clock_op),        
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),      
    /*  input                   */  .route_ci(1'b0),    
    /*  input                   */  .f0_load(1'b0),     
    /*  input                   */  .f1_load(1'b0),     
    /*  input                   */  .d0_load(1'b0),     
    /*  input                   */  .d1_load(1'b0),     
    /*  output                  */  .ce0(ce0),             
    /*  output                  */  .cl0(cl0),             
    /*  output                  */  .z0(),              
    /*  output                  */  .ff0_reg(tc),             
    /*  output                  */  .ce1(ce1),
    /*  output                  */  .cl1(cl1),
    /*  output                  */  .z1(),
    /*  output                  */  .ff1(),
    /*  output                  */  .ov_msb(),
    /*  output                  */  .co_msb(),
    /*  output                  */  .cmsb_reg(bitstream),
    /*  output                  */  .so(),
    /*  output                  */  .f0_bus_stat(),     
    /*  output                  */  .f0_blk_stat(),     
    /*  output                  */  .f1_bus_stat(),     
    /*  output                  */  .f1_blk_stat()      
    );
    end /* end of if statement for 1-8 bits section of generate */
    else if(Resolution <= PRS_16_BIT) 
    begin : sC16
    cy_psoc3_dp16 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment:MSB Chain */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment:cmpA=A0<D1 ; SI=CHAIN */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, /*SC_REG7 Comment:FB Chain and MSB enable+CmpCh */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) PrISMdp(
    /* input            */ .clk(clock_op),                 
    /* input  [02:00]   */ .cs_addr(cs_addr),              
    /* input            */ .route_si(1'b0), 
    /* input            */ .route_ci(1'b0),  
    /* input            */ .f0_load(1'b0),     
    /* input            */ .f1_load(1'b0),     
    /* input            */ .d0_load(1'b0),    
    /* input            */ .d1_load(1'b0),     
    /* output [01:00]   */ .ce0({ce0,chained1}), 
    /* output [01:00]   */ .cl0({cl0,chained2}),  
    /* output [01:00]   */ .z0(),  
    /* output [01:00]   */ .ff0_reg({tc, chained3}),  
    /* output [01:00]   */ .ce1({ce1,chained4}),  
    /* output [01:00]   */ .cl1({cl1,chained5}),   
    /* output [01:00]   */ .z1(),    
    /* output [01:00]   */ .ff1(),  
    /* output [01:00]   */ .ov_msb(),      
    /* output [01:00]   */ .co_msb(),    
    /* output [01:00]   */ .cmsb_reg({bitstream, nc1}),    
    /* output [01:00]   */ .so(),        
    /* output [01:00]   */ .f0_bus_stat(),     
    /* output [01:00]   */ .f0_blk_stat(),     
    /* output [01:00]   */ .f1_bus_stat(),    
    /* output [01:00]   */ .f1_blk_stat()    
    );
    end /* end of if statement for 9-16 bits section of generate */
    else if(Resolution <= PRS_24_BIT) 
    begin : sC24
    cy_psoc3_dp24 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_c(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) PrISMdp(
        /* input            */ .clk(clock_op),          
        /* input  [02:00]   */ .cs_addr(cs_addr),  
        /* input            */ .route_si(1'b0),    
        /* input            */ .route_ci(1'b0),  
        /* input            */ .f0_load(1'b0), 
        /* input            */ .f1_load(1'b0),  
        /* input            */ .d0_load(1'b0),   
        /* input            */ .d1_load(1'b0),  
        /* output [02:00]   */ .ce0({ce0,chained1,chained2}),         
        /* output [02:00]   */ .cl0({cl0,chained3,chained4}),        
        /* output [02:00]   */ .z0(),       
        /* output [02:00]   */ .ff0_reg({tc, chained5,chained6}),    
        /* output [02:00]   */ .ce1({ce1,chained7,chained8}), 
        /* output [02:00]   */ .cl1({cl1,chained9,chained10}),  
        /* output [02:00]   */ .z1(),   
        /* output [02:00]   */ .ff1(), 
        /* output [02:00]   */ .ov_msb(),  
        /* output [02:00]   */ .co_msb(),  
        /* output [02:00]   */ .cmsb_reg({bitstream, nc1, nc2}),   
        /* output [02:00]   */ .so(),     
        /* output [02:00]   */ .f0_bus_stat(), 
        /* output [02:00]   */ .f0_blk_stat(),  
        /* output [02:00]   */ .f1_bus_stat(),  
        /* output [02:00]   */ .f1_blk_stat()   
    );
    end /* end of if statement for 17-24 bits section of generate */
    else if(Resolution <= PRS_32_BIT) 
    begin : sC32
    cy_psoc3_dp32 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_c(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_d(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A1,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:PRS */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Reset */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment:cmpA=A0<D1 */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) PrISMdp(
        /* input            */ .clk(clock_op),          
        /* input  [02:00]   */ .cs_addr(cs_addr),   
        /* input            */ .route_si(1'b0),     
        /* input            */ .route_ci(1'b0),    
        /* input            */ .f0_load(1'b0),     
        /* input            */ .f1_load(1'b0),     
        /* input            */ .d0_load(1'b0),    
        /* input            */ .d1_load(1'b0),   
        /* output [03:00]   */ .ce0({ce0,chained1,chained2,chained3}),         
        /* output [03:00]   */ .cl0({cl0,chained11,chained12,chained13}),        
        /* output [03:00]   */ .z0(),        
        /* output [03:00]   */ .ff0_reg({tc,chained21,chained22,chained23}),       
        /* output [03:00]   */ .ce1({ce1,chained31,chained32,chained33}),        
        /* output [03:00]   */ .cl1({cl1,chained41,chained42,chained43}),       
        /* output [03:00]   */ .z1(),       
        /* output [03:00]   */ .ff1(),          
        /* output [03:00]   */ .ov_msb(),       
        /* output [03:00]   */ .co_msb(),      
        /* output [03:00]   */ .cmsb_reg({bitstream, nc1, nc2, nc3}),        
        /* output [03:00]   */ .so(),           
        /* output [03:00]   */ .f0_bus_stat(),  
        /* output [03:00]   */ .f0_blk_stat(),  
        /* output [03:00]   */ .f1_bus_stat(),  
        /* output [03:00]   */ .f1_blk_stat()   
    );
    end /* end of if statement for 25-32 bits section of generate */         
    endgenerate

endmodule   /* PrISM_v2_0 */
`endif      /* PrISM_v2_0_V_ALREADY_INCLUDED */


