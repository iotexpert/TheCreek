/*******************************************************************************
 *
 * FILENAME:  PRS_v1_20.v
 * UM Name:   PRS_v1_20
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
 *    D0 => Lower half of polynomial register
 *    D1 => Upper half of polynomial register
 *    A0 => Contain the initial (Seed) value and PRS residual value at the end of the computation
 *    A1 => na
 *
 *------------------------------------------------------------------------------
 *  Data Path States
 *
 *  0 0 0   0   Idle
 *  0 0 1   1   Computation PRS (Lower half)
 *  0 1 0   2   Idle
 *  0 1 1   3   Computation PRS (Upper half)
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
`ifdef PRS_v1_20_V_ALREADY_INCLUDED
`else
`define PRS_v1_20_V_ALREADY_INCLUDED

module PRS_v1_20
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
   localparam [7:0]    PRS_40_BIT   = 8'd40;
   localparam [7:0]    PRS_48_BIT   = 8'd48;
   localparam [7:0]    PRS_56_BIT   = 8'd56;
   localparam [7:0]    PRS_64_BIT   = 8'd64;
   parameter  [7:0]    Resolution   = PRS_8_BIT;    
   
   localparam [7:0] PRS_RUNMODE_CLOCKED         = 8'd0;
   localparam [7:0] PRS_RUNMODE_API_SINGLE_STEP = 8'd1;
   parameter [0:0] RunMode = PRS_RUNMODE_CLOCKED;

   /* Internal signals */
   wire       clk;
   wire       so, so_reg, ci, si, cmsb;
   wire       dcfg, dcfg_b, ci_temp, sc_temp, nclk, si_final, so_chain;
   wire       si_d, si_c, si_b, si_a;
   wire       so_d, so_c, so_b, so_a;
   
   wire [7:0] sc_out;
   wire [2:0] cs_addr;   

   /* Dummy connections */
   //   wire   nc1, nc2, nc3;  // Not connected wire

   /**************************************************************************/
   /* Control Register Implementation                                        */
   /**************************************************************************/
   /* Control Register Bits (Bits 7-3 are unused )*/
   localparam PRS_CTRL_ENABLE      = 8'h0;   // Enable PRS             
   localparam PRS_CTRL_RISING_EDGE = 8'h1;   // API single step
   localparam PRS_CTRL_DFF_RESET   = 8'h2;   // DFF reset 
   
   wire [7:0] control;                       //  Control Register Output    

   /* Control Signals */
   wire       ctrl_enable = control[PRS_CTRL_ENABLE];
   wire       api_clock = control[PRS_CTRL_RISING_EDGE];
   wire       dff_reset = control[PRS_CTRL_DFF_RESET];
   
   cy_psoc3_control #(.cy_force_order(1))
   ctrlreg(
	   /* output   [07:00]       */  .control(control)
	   );  

   /**************************************************************************/
   /* Instantiate the data path elements                                     */
   /**************************************************************************/

   localparam [2:0] dpPOVal =  (Resolution <=8) ? (Resolution[7:0]-8'd1): 
                               (Resolution <=16) ? (Resolution[7:0]-8'd8-8'd1):
			       (Resolution <=24) ? (Resolution[7:0]-8'd16-8'd1):
			       (Resolution <=32) ? (Resolution[7:0]-8'd24-8'd1):
			       (Resolution <=40) ? (Resolution[7:0]-8'd32-8'd1):
			       (Resolution <=48) ? (Resolution[7:0]-8'd40-8'd1):
			       (Resolution <=56) ? (Resolution[7:0]-8'd48-8'd1):
			       (Resolution <=64) ? (Resolution[7:0]-8'd56-8'd1): 8'd0;

   localparam [2:0] dpMsbVal = (Resolution < 8 ) ? (Resolution[7:0]-8'd1) : 3'd7;
     
   wire       ctrl_enable_final;

   assign ctrl_enable_final = ctrl_enable & enable;

   generate
      if (RunMode == PRS_RUNMODE_CLOCKED)
	assign clk = clock;
      else
	assign clk = api_clock;
   endgenerate


   localparam dpconfig0 ={
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:LSB */
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
	`CS_ALU_OP__XOR, `CS_SRCA_A1, `CS_SRCB_D1,
	`CS_SHFT_OP___SL, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:MSB */
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
	`SC_CI_A_ROUTE, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_ENBL,
	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
      };
   
   generate
      if (Resolution <= PRS_8_BIT) begin
         assign cs_addr = {1'b0, 1'b0, ctrl_enable_final};
	 assign ci = cmsb;
	 
	 // Bitstream output
	 assign bitstream = cmsb;
      end
      else begin
	 assign cs_addr = {1'b0, dcfg, ctrl_enable_final};

	 // SYNC SO
	 dtrig sync(so_reg, so, 1'b1, dff_reset, clk);
	 
	 // dcfg
	 not n1(nclk, clk);
	 dtrig d0(dcfg, dcfg_b, 1'b0, dff_reset, nclk);
	 not n0(dcfg_b, dcfg);

	 // CI
	 dtrig d1(ci_temp, ci, 1'b1, dff_reset, nclk);
	 assign ci = dcfg ? sc_out[dpPOVal] : ci_temp;

	 // SI 
	 dtrig d2(sc_temp, sc_out[dpPOVal], 1'b1, dff_reset, nclk);
	 assign si = dcfg ? so_reg : sc_temp;

	 // Bitstream output
	 assign bitstream = ci_temp;

	 
      end // else: !if(Resolution <= PRS_8_BIT)
      
   endgenerate


   generate
      if (Resolution <= PRS_8_BIT)
      	assign si_a = cmsb;
      else if (Resolution <= PRS_16_BIT)
	assign si_a = si;
      else if (Resolution <= PRS_24_BIT)
	assign si_b = dcfg ? si : so_a;
      else if (Resolution <= PRS_32_BIT)
	assign si_b = so_a;
      else if (Resolution <= PRS_40_BIT) begin  
	 assign si_c = so_b;
	 assign si_b = dcfg ? si : so_a;
      end
      else if (Resolution <= PRS_48_BIT) begin
	 assign si_c = so_b;
	 assign si_b = so_a;
      end
      else if (Resolution <= PRS_56_BIT) begin
	 assign si_d = so_c;
	 assign si_c = so_b;
	 assign si_b = dcfg ? si : so_a;
      end
      else if (Resolution <= PRS_64_BIT) begin
	 assign si_d = so_c;
	 assign si_c = so_b;
	 assign si_b = so_a;
      end
   endgenerate


   generate
      if(Resolution <= PRS_16_BIT) begin : c1DP
	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_a(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si_a),      
		 /*  input                   */  .route_ci(ci),    
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(cmsb), // Carry out
		 /*  output                  */  .so(so),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),
		 
		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po(sc_out[7:0])          // Parallel data port
		 );
      end // block: c1DP
      else if(Resolution <= PRS_32_BIT) begin : c2DP
	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_b(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si_b),      
		 /*  input                   */  .route_ci(ci),    
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),
		 
		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po(sc_out[7:0])          // Parallel data port
		 );

	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_a(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si),
		 /*  input                   */  .route_ci(ci),
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so_a),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),

		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po()          // Parallel data port
		 );
      end // block: c2DP
      else if(Resolution <= PRS_48_BIT) begin : c3DP
	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_c(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si_c),      
		 /*  input                   */  .route_ci(ci),    
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),

		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po(sc_out[7:0])          // Parallel data port
		 );

	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_b(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si_b),      
		 /*  input                   */  .route_ci(ci),    
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so_b),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),

		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po()          // Parallel data port
		 );

	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_a(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si),
		 /*  input                   */  .route_ci(ci),
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so_a),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),

		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po()          // Parallel data port
		 );
      end // block: c3DP
      else if(Resolution <= PRS_64_BIT) begin : c4DP
	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_d(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si_d),      
		 /*  input                   */  .route_ci(ci),    
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),

		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po(sc_out[7:0])          // Parallel data port
		 );

	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_c(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si_c),
		 /*  input                   */  .route_ci(ci),
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so_c),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),
		 
		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po()          // Parallel data port
		 );

	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_b(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si_b),      
		 /*  input                   */  .route_ci(ci),    
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so_b),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),

		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po()          // Parallel data port
		 );


	 cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
	 PRSdp_a(
		 /*  input                   */  .clk(clk),        
		 /*  input   [02:00]         */  .cs_addr(cs_addr),
		 /*  input                   */  .route_si(si),
		 /*  input                   */  .route_ci(ci),
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
		 /*  output                  */  .co_msb(), // Carry out
		 /*  output                  */  .cmsb(), // Carry out
		 /*  output                  */  .so(so_a),  // Shift out
		 /*  output                  */  .f0_bus_stat(),     
		 /*  output                  */  .f0_blk_stat(),     
		 /*  output                  */  .f1_bus_stat(),     
		 /*  output                  */  .f1_blk_stat(),

		 /* input                    */  .ci(1'b0),     // Carry in from previous stage
		 /* output                   */  .co(),         // Carry out to next stage
		 /* input                    */  .sir(1'b0),    // Shift in from right side
		 /* output                   */  .sor(),        // Shift out to right side
		 /* input                    */  .sil(1'b0),    // Shift in from left side
		 /* output                   */  .sol(),        // Shift out to left side
		 /* input                    */  .msbi(1'b0),   // MSB chain in
		 /* output                   */  .msbo(),       // MSB chain out
		 /* input [01:00]            */  .cei(2'b0),    // Compare equal in from prev stage
		 /* output [01:00]           */  .ceo(),        // Compare equal out to next stage
		 /* input [01:00]            */  .cli(2'b0),    // Compare less than in from prv stage
		 /* output [01:00]           */  .clo(),        // Compare less than out to next stage
		 /* input [01:00]            */  .zi(2'b0),     // Zero detect in from previous stage
		 /* output [01:00]           */  .zo(),         // Zero detect out to next stage
		 /* input [01:00]            */  .fi(2'b0),     // 0xFF detect in from previous stage
		 /* output [01:00]           */  .fo(),         // 0xFF detect out to next stage
		 /* input [01:00]            */  .capi(2'b0),   // Software capture from previous stage
		 /* output [01:00]           */  .capo(),       // Software capture to next stage
		 /* input                    */  .cfbi(1'b0),   // CRC Feedback in from previous stage
		 /* output                   */  .cfbo(),       // CRC Feedback out to next stage
		 /* input [07:00]            */  .pi(8'b0),     // Parallel data port
		 /* output [07:00]           */  .po()          // Parallel data port
		 );
      end // block: c4DP
   endgenerate
endmodule   

module dtrig
  (
   output reg q,
   input wire d,
   input wire reset_val,
   input wire reset,
   input wire clk
   );
     
   always @(posedge clk or posedge reset)
     begin
       if (reset)
	 begin
	    q = reset_val;
	 end
       else
	 begin
	    q = d;
	 end
    end // always @ (posedge clk or posedge reset)
endmodule // dtrig


`endif      /* PRS_v1_20_V_ALREADY_INCLUDED */
