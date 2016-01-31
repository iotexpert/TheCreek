
//`#start header` -- edit after this line, do not edit this line
/*******************************************************************************
 *
 * FILENAME:  B_SegLCD_v1_30.v
 * UM Name:   B_SegLCD_v10_0
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
 *   D0 => start delay (for dac_disable delay) 
 *   D1 => value that adjusts the en_hi period
 *   A0 => (actual counter)
 *   A1 => 127 (for divider by 128)
 *
 *------------------------------------------------------------------------------
 *  Data Path States
 *
 *  0 0 0   0   Load A0 <= D0
 *  0 0 1   1   Load A0 <= D0
 *  0 1 0   2   Decrement A0 (CMP_B)
 *  0 1 1   3   Decrement A0 (CMP_A)
 
 *------------------------------------------------------------------------------
 *Datapath is used for generation of  three conditions (zero_detect and compare equal in two configurations:
 CMP_A (A1_A0) - divider by 256 and CMP_B(A0_D1) - divider by 128) 
 *------------------------------------------------------------------------------
 *  Copyright (c) Cypress Semiconductor 2008.  All Rights Reserved.
 ******************************************************************************/
`include "cypress.v"
`ifdef B_SegLCD_v1_30_ALREADY_INCLUDED
`else
`define B_SegLCD_v1_30_ALREADY_INCLUDED

//`#end` -- edit above this line, do not edit this line
// Component: bSegLCD_v1_30
//`#start body` -- edit after this line, do not edit this line

module bSegLCD_v1_30 (
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
    
    parameter [4:0] NumCommonLines=7'd16;
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
    wire zero_detect2;
    wire final_counter_tc2;
    wire load_count;
    wire ctrl_store_zero;
	wire zero_detect;
	wire lodrive;
	wire zd_tune;
	wire dac_dis_low_pow;
	wire	[7:0]	control; /* Control Register Output    */
    
	wire    ctrl_clk_enable   = control[LCD_CTRL_CLK_EN];
    wire    ctrl_reset		  = control[LCD_RESET];
    wire    ctrl_post_reset   = control[LCD_POST_RESET];
	
	reg     internal_drive;
	wire    counter_enable;
	wire    intrnl_clock;
	wire    fr_type_b;
	wire    dac_disable;
	wire    ctrl_store_sm;
	wire zd;
	wire dc_dis_fin;
	wire change_comm;
	wire not_ctrl_store_sm;
	wire count_out,nc1,nc2,nc3,nc4,nc5,nc6,nc7;
    wire ce_0;
    wire cnt_enable_fin;
	wire internal_clock;
    wire tt12_clk;
	/*when clock is disabled the load_count enables the loading of period for counter7*/
	
	not not_ctrl (load_count, control[LCD_CTRL_CLK_EN]);
  
    /* Control Register instantiation*/
	
  	cy_psoc3_control
    #(.cy_force_order(1))
    ctrlreg(
        /* output	[07:00]	  */  .control(control)
    );
 
 t_ff t_ff1(
 .q(ctrl_store_sm),
 .d(1'b0),
 .reset(ctrl_reset),
 .clk(zd_tune)
 );
   wire not_reset;
   assign not_reset = !ctrl_reset; 

   /* Data Path is used as frequency divider by 256 to generate the en_hi signal
   with ce1 comparator (to adjust en_hi pulse period)
   and as divider by 128 for dac_disable signal generation*/
   
	cy_psoc3_dp8 #(.cy_dpconfig_a(
	{
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset (A0<=D0) */
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Reset (A0<=D0) */
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Decrement A0 */
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGB, /*CS_REG3 Comment:Decrement A0 */
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
		`SC_CMPB_A1_A0, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
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
/* input [02:00] */ .cs_addr({1'b0,control[LCD_CTRL_CLK_EN],ctrl_store_sm}), // Control Store RAM address
/* input */ .route_si(1'b0), /* Shift in from routing*/
/* input */ .route_ci(1'b0), /* Carry in from routing*/
/* input */ .f0_load(1'b0),  /* Load FIFO 0*/
/* input */ .f1_load(1'b0),  /* Load FIFO 1*/
/* input */ .d0_load(1'b0),  /* Load Data Register 0*/
/* input */ .d1_load(1'b0),  /* Load Data Register 1*/
/* output */ .ce0(ce_0), /* Accumulator 0 = Data register 0*/
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
 

 cy_dsrffe srff_1(
  .d(1'b1),
  .s(1'b0),
  .r(ctrl_reset),
  .e(zd),
  .clk(!clock),
  .q(cnt_enable_fin)

 );  
 
  wire temp_wire = ctrl_reset | cnt_enable_fin;
  wire counter_clk;
  wire clk_ff12;
  wire clk_ff15;
  wire ff15_res;
  
 mux2to1 mux3 (
.out(counter_clk),
.in0(zd),
.in1(ctrl_post_reset),
.sel(ctrl_reset)
); 

cy_psoc3_count7 #(.cy_period(mx_ratio),.cy_route_ld(1),.cy_route_en(1)) 
        DivCounter(
        /*  input		     */  .clock(counter_clk),
        /*  input		     */  .reset(1'b0),
        /*  input		     */  .load(ctrl_reset),
        /*  input		     */  .enable(temp_wire),
        /*  output	[06:00]*/    .count({nc6,nc5,nc4,nc3,nc2,nc1,nc7}),
        /*  output		     */  .tc(counter_tc)
);

t_ff t_ff4(
.q(intrnl_clock),
.d(1'b1),
.reset(ctrl_reset),
.clk(counter_clk)
);

/* Generating of Frame (Type B)*/

t_ff t_ff2(

.q(fr_type_b),
.d(1'b0),
.reset(ctrl_reset),
.clk(counter_tc)

);

/*  Generating the DAC disable signal (for Always Active and Transition Active modes)*/

not not_1 (not_ctrl_store_sm, ctrl_store_sm);

if(DriverPowerMode==1) begin:LowPower


cy_psoc3_dp8 #(.cy_dpconfig_a(
{
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset (A0<=D0) */
	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Decrement A0 */
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
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
	`SC_CMPB_A1_A0, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
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
})) bLowPowerdp(
/* input */ .clk(clock), /*Clock*/
/* input [02:00] */ .cs_addr({1'b0,1'b0,change_comm}), // Control Store RAM address
/* input */ .route_si(1'b0), /* Shift in from routing*/
/* input */ .route_ci(1'b0), /* Carry in from routing*/
/* input */ .f0_load(1'b0),  /* Load FIFO 0*/
/* input */ .f1_load(1'b0),  /* Load FIFO 1*/
/* input */ .d0_load(1'b0),  /* Load Data Register 0*/
/* input */ .d1_load(1'b0),  /* Load Data Register 1*/
/* output */ .ce0(lodrive), /* Accumulator 0 = Data register 0*/
/* output */ .cl0(), /* Accumulator 0 < Data register 0*/
/* output */ .z0(zero_detect2), /* eh_hi*/
/* output */ .ff0(), /* Accumulator 0 = FF*/
/* output */ .ce1(), /* Accumulator [0|1] = Data register 1*/
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

or or13 (dac_dis_low_pow, zero_detect2, lodrive);
assign dac_disable = dc_dis_fin;


assign clk_ff12 = zero_detect|zero_detect2;
assign clk_ff15 = (!change_comm)|zero_detect2;
assign ff15_res = ctrl_reset|not_ctrl_store_sm;
t_ff t_ff12(
 .q(change_comm),
 .d(1'b0),
 .reset(ctrl_reset),
 .clk(clk_ff12)
 );

t_ff t_ff15(
 .q(dc_dis_fin),
 .d(1'b0),
 .reset(ff15_res),
 .clk(clk_ff15)
 );

 or or12 (tt12_clk, zero_detect, dac_dis_low_pow);

end else begin
assign clk_ff12 = 1'b0;
assign clk_ff15 = 1'b0;
assign ff15_res = 1'b0;

t_ff t_ff3(

.q(dac_disable),
.d(1'b1),
.reset(ce_0),
.clk(not_ctrl_store_sm)

);

end
always @(posedge zd or posedge dac_disable) begin
  if(dac_disable) begin
     internal_drive<=1'b0;
  end
  else begin
     internal_drive<=1'b1;
  end
end

if(WaveformType==0)begin
mux2to1 mux1 (
.out(internal_clock),
.in0(intrnl_clock),
.in1(ctrl_post_reset),
.sel(ctrl_reset)
);
end
else begin
 mux2to1 mux1 (
.out(internal_clock),
.in0(zd),
.in1(ctrl_post_reset),
.sel(ctrl_reset)
); 
end

t_ff t_ff5(

.q(zd),
.d(1'b0),
.reset(zero_detect),
.clk(ctrl_store_sm)

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
 assign frame=internal_clock;
 assign data_clk=internal_clock;
 assign dma_req=internal_clock;
end

else begin
 assign frame=fr_type_b;
 assign data_clk=internal_clock;
 assign dma_req=internal_clock;

end

endmodule

module t_ff (
	q,
    d,
	clk,
	reset
);
	output reg q;
	input  wire clk;
    input  wire reset;
    input  wire  d;


always@ (posedge reset or posedge clk) begin

if(reset)begin

    q <= d;

end

else begin 

    q <=!q;

end
end

endmodule
module mux2to1 (out,in0, in1, sel);
  output out;
  input in0,in1,sel;
  assign out = sel ? in1:in0;

endmodule 

`endif /*B_SegLCD_v1_0_ALREADY_INCLUDED*/

