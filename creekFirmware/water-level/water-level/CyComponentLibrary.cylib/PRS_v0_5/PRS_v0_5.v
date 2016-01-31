/*******************************************************************************
 *
 * FILENAME:  PRS_v0_5.v
 * UM Name:   PRS_v0_5
 * @Version@
 *
 * DESCRIPTION:
 *   This file provides a top level model of the PRS user module
 *   defining and all of the necessary interconnect.
 *
 *------------------------------------------------------------------------------
 *                                IO SIGNALS
 *------------------------------------------------------------------------------
 *   name           IO          Description
 *
 *   di            input        Input Data
 *   clock         input        Data clock
 *   bitstream     output       Bitstream
 *
 *------------------------------------------------------------------------------
 *                 Control and Status Register definitions
 *------------------------------------------------------------------------------
 * TBD 
 *
 *------------------------------------------------------------------------------
 *                 Data Path register definitions
 *------------------------------------------------------------------------------
 *  INSTANCE NAME:  PRSdp
 *
 *  DESCRIPTION:
 *    Implements the PRS 8-Bit U0 only; 16-bit U0 = LSB, U1 = MSB; etc
 *
 *  REGISTER USAGE:
 *    F0 => na
 *    F1 => na
 *    D0 => Polynomial register
 *    D1 => na
 *    A0 => Contain the initial (Seed) value and PRS residual value at the end of the computation
 *    A1 => na
 *
 *------------------------------------------------------------------------------
 *  Data Path States
 *
 *  0 0 0   0   Idle
 *  0 0 1   1   Computation PRS 
 *  0 1 0   2   Idle
 *  0 1 1   3   Idle
 *  1 0 0   4   Idle
 *  1 0 1   5   Idle
 *  1 1 0   6   Idle
 *  1 1 1   7   Idle

 *------------------------------------------------------------------------------
 *
 * Todo:
 *
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



`include "cypress.v"
`ifdef PRS_v0_5_V_ALREADY_INCLUDED
`else
`define PRS_v0_5_V_ALREADY_INCLUDED

module PRS_v0_5
  (
   input wire clock,
   input wire enable,
   output wire bitstream
   );

   /**************************************************************************/
   /* Parameters                                                             */
   /**************************************************************************/
   localparam [7:0]    PRS_8_BIT    = 8'd8;
   localparam [7:0]    PRS_16_BIT   = 8'd16;
   localparam [7:0]    PRS_24_BIT   = 8'd24;
   localparam [7:0]    PRS_32_BIT   = 8'd32;
   parameter  [7:0]    Resolution   = PRS_8_BIT;    
   
   localparam [7:0] PRS_RUNMODE_CLOCKED         = 8'd0;
   localparam [7:0] PRS_RUNMODE_API_SINGLE_STEP = 8'd1;
   parameter [0:0] RunMode = PRS_RUNMODE_CLOCKED;

   /* Internal signals */
   wire       clk;
   
   /* Dummy connections */
   wire   nc1, nc2, nc3;  // Not connected wire

   /**************************************************************************/
   /* Control Register Implementation                                        */
   /**************************************************************************/
   /* Control Register Bits (Bits 7-3 are unused )*/
   localparam PRS_CTRL_ENABLE      = 8'h0;   // Enable PRS             
   localparam PRS_CTRL_RISING_EDGE = 8'h1;   // API single step        
   wire [7:0] control;                       //  Control Register Output    

   /* Control Signals */
   wire       ctrl_enable = control[PRS_CTRL_ENABLE];
   wire       api_clock = control[PRS_CTRL_RISING_EDGE];
   
   cy_psoc3_control #(.cy_force_order(1))
   ctrlreg(
	   /* output   [07:00]       */  .control(control)
	   );  

   /**************************************************************************/
   /* Instantiate the data path elements                                     */
   /**************************************************************************/
   localparam [2:0] dpMsbVal = (Resolution <=8) ? (Resolution[5:0]-6'd1): 
                               (Resolution <=16) ? (Resolution[5:0]-6'd8-6'd1):
							   (Resolution <=24) ? (Resolution[5:0]-6'd16-6'd1):
							   (Resolution <=32) ? (Resolution[5:0]-6'd24-6'd1): 6'd0;
   wire       ctrl_enable_final;
   wire [2:0] cs_addr = {1'b0, 1'b0, ctrl_enable_final};
   assign ctrl_enable_final = ctrl_enable & enable;
   
   generate
      if (RunMode == PRS_RUNMODE_CLOCKED)
	assign clk = clock;
      else
	assign clk = api_clock;
   endgenerate
 
   generate
      if (Resolution <= PRS_8_BIT) begin : sC8
	/*parameter dpMsbVal = `SC_MSB_BIT0;*/
	//parameter dpMsbVal = `SC_MSB_BIT0;
	 cy_psoc3_dp8 #(.cy_dpconfig_a(
	 {
	 	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	 	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	 	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	 	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
	 	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
	 	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
	 	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	 	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
	 	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	 	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	 	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	 	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
	 	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	 	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	 	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	 	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
	 	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
	 	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
	 	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
	 	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
	 	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
	 	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
	 	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
	 	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
	 	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
	 	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
	 	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
	 	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	 })) PRSdp(
    /*  input                   */  .clk(clk),        
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),      
    /*  input                   */  .route_ci(1'b0),    
    /*  input                   */  .f0_load(1'b0),     
    /*  input                   */  .f1_load(1'b0),     
    /*  input                   */  .d0_load(1'b0),     
    /*  input                   */  .d1_load(1'b0),     
    /*  output                  */  .ce0(),             
    /*  output                  */  .cl0(),             
    /*  output                  */  .z0(),              
    /*  output                  */  .ff0(),             
    /*  output                  */  .ce1(),
    /*  output                  */  .cl1(),
    /*  output                  */  .z1(),
    /*  output                  */  .ff1(),
    /*  output                  */  .ov_msb(),
    /*  output                  */  .co_msb(),
    /*  output                  */  .cmsb(bitstream),
    /*  output                  */  .so(),
    /*  output                  */  .f0_bus_stat(),     
    /*  output                  */  .f0_blk_stat(),     
    /*  output                  */  .f1_bus_stat(),     
    /*  output                  */  .f1_blk_stat()      
    );
  end //end of if statement for 1-8 bits section of generate
  else if(Resolution <= PRS_16_BIT) begin : sC16
    cy_psoc3_dp16 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
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
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment:FB Chain and MSB enable */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) PRSdp(
    /* input            */ .clk(clk),                 
    /* input  [02:00]   */ .cs_addr(cs_addr),              
    /* input            */ .route_si(1'b0), 
    /* input            */ .route_ci(1'b0),  
    /* input            */ .f0_load(1'b0),     
    /* input            */ .f1_load(1'b0),     
    /* input            */ .d0_load(1'b0),    
    /* input            */ .d1_load(1'b0),     
    /* output [01:00]   */ .ce0(), 
    /* output [01:00]   */ .cl0(),  
    /* output [01:00]   */ .z0(),  
    /* output [01:00]   */ .ff0(),  
    /* output [01:00]   */ .ce1(),  
    /* output [01:00]   */ .cl1(),   
    /* output [01:00]   */ .z1(),    
    /* output [01:00]   */ .ff1(),  
    /* output [01:00]   */ .ov_msb(),      
    /* output [01:00]   */ .co_msb(),    
    /* output [01:00]   */ .cmsb({bitstream, nc1}),    
    /* output [01:00]   */ .so(),        
    /* output [01:00]   */ .f0_bus_stat(),     
    /* output [01:00]   */ .f0_blk_stat(),     
    /* output [01:00]   */ .f1_bus_stat(),    
    /* output [01:00]   */ .f1_blk_stat()    
    );
    end //end of if statement for 9-16 bits section of generate
    else if(Resolution <= PRS_24_BIT) begin : sC24
    cy_psoc3_dp24 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
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
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_c(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) PRSdp(
        /* input            */ .clk(clk),          
        /* input  [02:00]   */ .cs_addr(cs_addr),  
        /* input            */ .route_si(1'b0),    
        /* input            */ .route_ci(1'b0),  
        /* input            */ .f0_load(1'b0), 
        /* input            */ .f1_load(1'b0),  
        /* input            */ .d0_load(1'b0),   
        /* input            */ .d1_load(1'b0),  
        /* output [02:00]   */ .ce0(),         
        /* output [02:00]   */ .cl0(),        
        /* output [02:00]   */ .z0(),       
        /* output [02:00]   */ .ff0(),    
        /* output [02:00]   */ .ce1(), 
        /* output [02:00]   */ .cl1(),  
        /* output [02:00]   */ .z1(),   
        /* output [02:00]   */ .ff1(), 
        /* output [02:00]   */ .ov_msb(),  
        /* output [02:00]   */ .co_msb(),  
        /* output [02:00]   */ .cmsb({bitstream, nc1, nc2}),   
        /* output [02:00]   */ .so(),     
        /* output [02:00]   */ .f0_bus_stat(), 
        /* output [02:00]   */ .f0_blk_stat(),  
        /* output [02:00]   */ .f1_bus_stat(),  
        /* output [02:00]   */ .f1_blk_stat()   
    );
    end //end of if statement for 17-24 bits section of generate
    else if(Resolution <= PRS_32_BIT) begin : sC32
    cy_psoc3_dp32 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
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
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_c(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_d(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
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
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) PRSdp(
        /* input            */ .clk(clk),          
        /* input  [02:00]   */ .cs_addr(cs_addr),   
        /* input            */ .route_si(1'b0),     
        /* input            */ .route_ci(1'b0),    
        /* input            */ .f0_load(1'b0),     
        /* input            */ .f1_load(1'b0),     
        /* input            */ .d0_load(1'b0),    
        /* input            */ .d1_load(1'b0),   
        /* output [03:00]   */ .ce0(),         
        /* output [03:00]   */ .cl0(),        
        /* output [03:00]   */ .z0(),        
        /* output [03:00]   */ .ff0(),       
        /* output [03:00]   */ .ce1(),        
        /* output [03:00]   */ .cl1(),       
        /* output [03:00]   */ .z1(),       
        /* output [03:00]   */ .ff1(),          
        /* output [03:00]   */ .ov_msb(),       
        /* output [03:00]   */ .co_msb(),      
        /* output [03:00]   */ .cmsb({bitstream, nc1, nc2, nc3}),        
        /* output [03:00]   */ .so(),           
        /* output [03:00]   */ .f0_bus_stat(),  
        /* output [03:00]   */ .f0_blk_stat(),  
        /* output [03:00]   */ .f1_bus_stat(),  
        /* output [03:00]   */ .f1_blk_stat()   
    );
    end //end of if statement for 25-32 bits section of generate         
    endgenerate


endmodule   /* PRS_v0_5 */
`endif      /* PRS_v0_5_V_ALREADY_INCLUDED */
