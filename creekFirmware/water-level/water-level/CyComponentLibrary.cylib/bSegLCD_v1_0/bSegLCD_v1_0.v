
//`#start header` -- edit after this line, do not edit this line
/*******************************************************************************
 *
 * FILENAME:  B_SegLCD_v1_0.v
 * UM Name:   B_SegLCD_v1_0
 *
 * DESCRIPTION:
 *   The Base SegLCD generates six UDB-based signals for PSoC3 LCD subsystem
 *   Implementation done with UDB's.
 
 *------------------------------------------------------------------------------
 *                                IO SIGNALS
 *------------------------------------------------------------------------------
 *   name           IO          Description
 *
 *   clock         input        component clock
 *   chop_clk      output       chop clock (gets directly from the PSoC3 clock source system)
 *   dac_dis       output       disable LCD DAC
 *   data_clk      output       data clock
 *   dma_req,      output       DMA request
 *   drive,        output       drive
 *   en_hi,        output       enable hi drive  
 *   frame         output       frame
 *
 *------------------------------------------------------------------------------
 *                 Control Register definitions
 *------------------------------------------------------------------------------
 *
 *  Control Register Definition
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  |unused | unused | unused | unused | unused | unused | unused |ctrl_res| clk_en |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *
 *    clk_en   =>       0 = clock disable
 *                      1 = clock enable
 *                      
 *    ctrl_res =>       0 = Normal Operation on the component
 *                      1 = Software Reset     
 *  
 *------------------------------------------------------------------------------
 *                 Data Path 1 and Data Path 2 register definitions                
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAME:  bSegLCDdp (Data Path 1) dpCounter_2 (Data Path 2)
 *
 *  DESCRIPTION:
 *    Implement the down counter 8-Bit 
 *
 *  REGISTER USAGE:
 *   F0 => na
 *   F1 => na
 *   D0 => start delay (for Data Path 1) and divider(=127) (For Data Path 2)
 *   D1 => na
 *   A0 => (actual counter)
 *   A1 => na
 *
 *------------------------------------------------------------------------------
 *  Data Path States
 *
 *  0 0 0   0   Load period  A0 <= D0
 *  0 0 1   1   Decrement A0
 *------------------------------------------------------------------------------
 *For Data Path 1 the zero detect condition is used as terminal count to divide 
 *the input frequency by 256. D0 contains the start delay to fit the requirements
 *for DAC disable signal in Transition Active mode.
 *
 *Data path 2 is used as frequency divider by 128. D0 contains the 127 and two 
 conditions (zero detect and compare equal) are OR-ed to form the correct terminal count
 
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef B_SegLCD_v1_0_ALREADY_INCLUDED
`else
`define B_SegLCD_v1_0_ALREADY_INCLUDED

//`#end` -- edit above this line, do not edit this line
// Component: bSegLCD_v1_0
//`#start body` -- edit after this line, do not edit this line

module bSegLCD_v1_0 (
	chop_clk,
	dac_dis,
	data_clk,
	dma_req,
	drive,
	en_hi,
	frame,
	clock
);
	output  chop_clk;
	output  dac_dis;
	output  data_clk;
	output  dma_req;
	output  drive;
	output  en_hi;
	output  frame;
	input   clock;

    /*Component hardware parameters*/
    
    parameter [4:0] NumCommonLines=7'd4;
    parameter DriverPowerMode=1'b1;
    parameter [7:0] FrameRate=8'd30;
    parameter WaveformType=1'b1;
    
	localparam LCD_CTRL_CLK_EN     = 1'h0; /* Clock enable */
    localparam LCD_RESET		   = 1'h1; /*Software reset*/
    localparam LCD_POST_RESET      = 2'h2; /*Post reset*/
	localparam mx_ratio=NumCommonLines-1;		   
	
    wire reset=1'b0;
    wire counter_tc;
    wire counter_tc2;
    wire counter_load=1'b0;
    wire counter_enable=1'b1;
    wire zero_detect2;
    wire final_counter_tc2;
    wire load_count;
    wire ctrl_store_zero;
	wire zero_detect;
	wire zd_tune;
	wire	[7:0]	control; /* Control Register Output    */
    
	wire    ctrl_clk_enable   = control[LCD_CTRL_CLK_EN];
    wire    ctrl_reset		  = control[LCD_RESET];
    wire    ctrl_post_reset   = control[LCD_POST_RESET];
	
	reg     internal_drive;
	reg     intrnl_clock;
	reg     fr_type_b;
	reg     dac_disable;
	reg     [2:0] ctrl_store_sm;
	
	/*when clock is disabled the load_count enables the loading of period for counter7*/
	
	not not_ctrl (load_count, control[LCD_CTRL_CLK_EN]);
  
    /* Control Register instantiation*/
	
  	cy_psoc3_control
    #(.cy_force_order(1))
    ctrlreg(
        /* output	[07:00]	  */  .control(control)
    );
    
   /*Control Store State Machine Implementation (controls address lines of both DPs)*/

   always @(posedge clock) begin
   if(control[LCD_CTRL_CLK_EN]==1)
   	 ctrl_store_sm<=3'b001; 
   else ctrl_store_sm<=3'b000;   
   end
   
    /*First Data Path is used as frequency divider by 256 to generate the en_hi signal*/
    
	cy_psoc3_dp8 #(.cy_dpconfig_a(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset A0<=D0 */
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Decrement A0 */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
		`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
		`CS_CMP_SEL_CFGB, /*CS_REG5 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
		`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
		`CS_CMP_SEL_CFGB, /*CS_REG6 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
		`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
		`CS_CMP_SEL_CFGB, /*CS_REG7 Comment: */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A0_A0, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
		`SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
		`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
		1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	})) bSegLCDdp(
/* input */ .clk(clock), /*Clock*/
/* input [02:00] */ .cs_addr({ctrl_store_sm[2],ctrl_store_sm[1],ctrl_store_sm[0]}), // Control Store RAM address
/* input */ .route_si(1'b0), /* Shift in from routing*/
/* input */ .route_ci(1'b0), /* Carry in from routing*/
/* input */ .f0_load(1'b0),  /* Load FIFO 0*/
/* input */ .f1_load(1'b0),  /* Load FIFO 1*/
/* input */ .d0_load(1'b0),  /* Load Data Register 0*/
/* input */ .d1_load(1'b0),  /* Load Data Register 1*/
/* output */ .ce0(), /* Accumulator 0 = Data register 0*/
/* output */ .cl0(), /* Accumulator 0 < Data register 0*/
/* output */ .z0(zero_detect), /* eh_hi*/
/* output */ .ff0(), /* Accumulator 0 = FF*/
/* output */ .ce1(zd_tune), /* Accumulator [0|1] = Data register 1*/
/* output */ .cl1(), /* Accumulator [0|1] < Data register 1*/
/* output */ .z1(), /* Accumulator 1 = 0*/
/* output */ .ff1(), /* Accumulator 1 = FF*/
/* output */ .ov_msb(), /* Operation over flow*/
/* output */ .co_msb(), /* Carry out*/
/* output */ .cmsb(), /* Carry out*/
/* output */ .so(), /* Shift out*/
/* output */ .f0_bus_stat(), /* FIFO 0 status to uP*/
/* output */ .f0_blk_stat(), /* FIFO 0 status to DP*/
/* output */ .f1_bus_stat(), /* FIFO 1 status to uP*/
/* output */ .f1_blk_stat()  /* FIFO 1 status to DP*/
);

/*counter7 is used for generating the frame signal for Type B waveforms */

cy_psoc3_count7 #(.cy_period(mx_ratio),.cy_route_ld(1),.cy_route_en(1)) 
        DivCounter(
        /*  input		     */  .clock(zero_detect),
        /*  input		     */  .reset(ctrl_reset),
        /*  input		     */  .load(load_count),
        /*  input		     */  .enable(ctrl_clk_enable),
        /*  output	[06:00]	 */  .count(),
        /*  output		     */  .tc(counter_tc)
);

/*The Data Path 2 is used for generating the frequency that needs for DAC disable signal(in Transition Active Mode)*/

cy_psoc3_dp8 #(.cy_dpconfig_a(
{
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset A0<=D0 */
	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Decrement A0 */
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
	`CS_CMP_SEL_CFGB, /*CS_REG5 Comment: */
	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
	`CS_CMP_SEL_CFGB, /*CS_REG6 Comment: */
	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
	`CS_CMP_SEL_CFGB, /*CS_REG7 Comment: */
	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
	`SC_CMPB_A0_A0, `SC_CMPA_A0_A0, `SC_CI_B_REGIS,
	`SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
	`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
	1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
})) dpCounter_2(
/* input */ .clk(clock), // Clock
/* input [02:00] */ .cs_addr({ctrl_store_sm[2],ctrl_store_sm[1],ctrl_store_sm[0]}), // Control Store RAM address
/* input */ .route_si(1'b0), /* Shift in from routing*/
/* input */ .route_ci(1'b0), /* Carry in from routing*/
/* input */ .f0_load(1'b0),  /* Load FIFO 0*/
/* input */ .f1_load(1'b0),  /* Load FIFO 1*/
/* input */ .d0_load(1'b0),  /* Load Data Register 0*/
/* input */ .d1_load(1'b0),  /* Load Data Register 1*/
/* output */ .ce0(counter_tc2), /* Accumulator 0 = Data register 0*/
/* output */ .cl0(), /* Accumulator 0 < Data register 0*/
/* output */ .z0(zero_detect2), /* Accumulator 0 = 0*/
/* output */ .ff0(), /* Accumulator 0 = FF*/
/* output */ .ce1(), /* Accumulator [0|1] = Data register 1*/
/* output */ .cl1(), /* Accumulator [0|1] < Data register 1*/
/* output */ .z1(),  /* Accumulator 1 = 0*/
/* output */ .ff1(), /* Accumulator 1 = FF*/
/* output */ .ov_msb(), /* Operation over flow*/
/* output */ .co_msb(), /* Carry out*/
/* output */ .cmsb(),   /* Carry out*/
/* output */ .so(), /* Shift out*/
/* output */ .f0_bus_stat(), /* FIFO 0 status to uP*/
/* output */ .f0_blk_stat(), /* FIFO 0 status to DP*/
/* output */ .f1_bus_stat(), /* FIFO 1 status to uP*/
/* output */ .f1_blk_stat()  /* FIFO 1 status to DP*/
);

/*Forms the terminal count for Data Path 2 (zero detect and compare equal conditions are OR-ed)*/

or or1(final_counter_tc2, zero_detect2, counter_tc2);

/*Sync. Invertor for generating the Data clock, Frame(Type A)and DMA request signals*/

always @(posedge zero_detect or posedge ctrl_post_reset) begin  
if(ctrl_post_reset) begin 
    intrnl_clock<=1'b0;
end 
else begin if(ctrl_reset) begin
      intrnl_clock<=1'b0;
   end
   else begin
      intrnl_clock<=!intrnl_clock;
   end
   end
end

/*process for generation Frame (Type B)*/

always @(posedge counter_tc or posedge ctrl_post_reset)begin
if(ctrl_post_reset) begin
   fr_type_b<=1'b0;
end
else begin 
   if(ctrl_reset) begin
      fr_type_b<=1'b0;
   end
   else begin
      fr_type_b<=!fr_type_b;
   end
end
end

/*Process for generating the DAC disable signal (for Always Active and Transition Active modes)*/

always @(posedge final_counter_tc2 or posedge ctrl_post_reset) begin
if(ctrl_post_reset) begin
       dac_disable<=1'b0;
   end
else begin if(ctrl_reset) begin
       dac_disable<=1'b0;
    end
	else begin
       dac_disable<=!dac_disable;
	end
	end
end


always @(posedge zero_detect or posedge dac_disable) begin
  if(dac_disable) begin
     internal_drive<=1'b0;
  end
  else begin
     internal_drive<=1'b1;
  end
end

/*Driving Outputs (according to the hardware parameters values)*/

//assign en_hi = zero_detect;
//reg zd;
//always@(posedge clock)begin
//if(zero_detect)
//zd<=1'b1;
//else if(zd_tune)
//zd<=1'b0;
//else zd<=zd;
//
//end

wire zd;
/*cy_srff zd_ff(
.clk(clock),
.s(zero_detect),
.r(zd_tune),
.q(zd)
);
*/
cy_srlch srlch1(
.s(zero_detect),
.r(zd_tune),
.q(zd)
);
assign en_hi=zd;

if (DriverPowerMode==0)begin
 assign drive=ctrl_clk_enable;
 assign dac_dis=1'b0;
end
else begin 
 assign drive = internal_drive;
 assign dac_dis=dac_disable;
end

if(WaveformType==0)begin
 assign frame=intrnl_clock;
 assign data_clk=intrnl_clock;
 //assign dma_req=intrnl_clock ;
 assign dma_req=intrnl_clock & ctrl_clk_enable;
end
else begin
 assign frame=fr_type_b;
 assign data_clk=zd;
 assign dma_req=zd;
end
//`#end` -- edit above this line, do not edit this line
endmodule

`endif /*B_SegLCD_v1_0_ALREADY_INCLUDED*/

//`#start footer` -- edit after this line, do not edit this line
//`#end` -- edit above this line, do not edit this line
