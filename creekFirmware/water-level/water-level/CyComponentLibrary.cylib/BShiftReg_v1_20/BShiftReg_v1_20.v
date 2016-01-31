/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


/*******************************************************************************
 *
 * FILENAME:  B_ShiftReg_v1_0.v
 * UM Name:   B_ShiftReg_v1_0
 *
 * DESCRIPTION:
 *   The Base Shift Register User Module is a simple 1-32-bit shift register
 *   Implementation done with UDB's.  
 	
 *	It has the following features:
 *	
 *	Adjustable shift register size: 1 to 32-bits; 
 *	Simultaneous shift in and shift out; 
 *	Right shift or left shift; 
 *	Shift on clock rising edge; 
 *	Reset input resets Shift register to all 0s; 
 * 	Shift state readable by CPU or DMA; 
 *	Shift state writable by CPU or DMA. 

 *
 *------------------------------------------------------------------------------
 *                                IO SIGNALS
 *------------------------------------------------------------------------------
 *   name           IO          Description
 *
 *   reset_in       input       System reset
 *   clock_in       input       Sytem clock
 *   shift_in       input       Shift input 
 *   load_in        input       Load  Accumulator(s) from input FIFO(s)
 *   store_in       input       Store Accumulator(s) value into the output FIFO(s) 
 
 *   shift_out      output      Shift Out
 *   interrupt      output      Interrupt
 *    
 *------------------------------------------------------------------------------
 *                 Control and Status Register definitions
 *------------------------------------------------------------------------------
 *
 *  Control Register Definition
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  | Desc  | unused | unused | unused | unused |2BF1_RES|r_en_int|F0_FULL | CLK_EN | 
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *
 *    clk_en       =>   0 = Disable shift register
 *                      1 = enable shift register
 *                      
 *    g_en_int     =>   0 = global disable interrupt
 *                      1 = global enable interrupt
 *                      
 *
 *    r_en_int	   =>   0 = disable interrupt on reset event
 *                      1 = enable interrupt on reset event
 
 *    l_en_int     =>   0 = disable interrupt on load event
 *                      1 = enable interrupt on load event
 	  
	  s_en_int     =>   1 = disable interrupt on store event
	  					0 = enable interrupt on store event
 *    f_ld_en      =>   1 = load enable 
 *                      0 = load disable
 *	  f_s_en	   =>   1 = store enable
 *                      0 = store disable  
 
 
 *  Interrupt Status Register Definition
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  | Desc |interrupt|unused  | unused |unused  |unused  |reset   | store  | load   |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *
 *    reset     =>  0 = reset event has not occured 
 *                  1 = reset event has occured
 *
 *    load      =>  0 = load event has not occured
 *                  1 = load event has occured
 *
 *    store     =>  0 = store event has not occured
 *                  1 = store event has occured
 
 
 
 *  FIFO Status Register Definition
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  | Desc  |        |unused  | unused | unused |f1_full f1_n_empt|f0_n_fll|f0_empty|
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *
 *    f0_emp     =>  0 =  F0 fifo is not empty 
 *                   1 =  F0 fifo is empty
 *
 *    f1_n_emp   =>  0 = F1 fifo is empty
 *                   1 = F1 fifo is not empty
 *
 *    2b_full    =>  0 = 2-byte F0 is not full
 *                   1 = 2-byte F0 is full
 
 
 *------------------------------------------------------------------------------
 *                 Data Path register definitions                
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAMES:  ShiftReg8_dp, ShiftReg16_dp, ShiftReg24_dp, ShiftReg32_dp 
 *
 *  DESCRIPTION:
 *    Implements the shift register 8, 16, 24, 32-Bit accordingly
 *
 *  REGISTER USAGE:
 
 *   F0 => input  FIFO buffer
 *   F1 => output FIFO buffer
 *   
     D0 => is used as all 0s source for A0 when the idle or reset event has being initiated
 *   D1 => unused
 *   
     A0 => directly the shift register
 *   
     A1 => unused
 *
 *------------------------------------------------------------------------------
 *  Data Paths States (are general for each Datapath)
 *
 *  0 0 0   0   Shift operation (Left or Right)
 *  0 0 1   1   Reset   (XOR A0 A0)
 *  0 1 0   2   Load A0 (A0<=F0)
 *  0 1 1   3   Reset   (XOR A0 A0)
 *------------------------------------------------------------------------------*/

`include "cypress.v"
`ifdef bShiftReg_v1_10_V_ALREADY_INCLUDED
`else
`define bShiftReg_v1_10_V_ALREADY_INCLUDED

module BShiftReg_v1_20 (
	output wire interrupt,
	output wire shiftOut,
    input  wire clock,
	input  wire load,
	input  wire reset,
	input  wire shiftIn,
	input  wire store
  
    
);

	
	parameter DefSi           = 0;
	parameter Direction       = 0;
	parameter FifoSize        = 0;
	parameter InterruptSource = 0;
	parameter Length          = 0;
	parameter UseInputFifo    = 0;
	parameter UseOutputFifo   = 0;
	parameter UseInterrupt    = 0;

	
	wire f0_blk_stat_8;
    wire f0_bus_stat_8;
	wire f1_blk_stat_8;
	wire f1_bus_stat_8;
	
	wire [1:0] f0_blk_stat_16;
    wire [1:0] f0_bus_stat_16;
	wire [1:0] f1_blk_stat_16;
	wire [1:0] f1_bus_stat_16;
	
	wire [2:0] f0_blk_stat_24;
    wire [2:0] f0_bus_stat_24;
	wire [2:0] f1_blk_stat_24;
	wire [2:0] f1_bus_stat_24;
	
	wire [3:0] f0_blk_stat_32;
    wire [3:0] f0_bus_stat_32;
	wire [3:0] f1_blk_stat_32;
	wire [3:0] f1_bus_stat_32;
	
	wire load_final;


	/**************************************************************************/
    /* Control Register Implementation                                        */
    /**************************************************************************/
	
	/* Control Register Bits (Bit 7 is unused */
	
	localparam SR_CTRL_CLK_EN        = 3'h0;	/* Clock enable */
    localparam SR_CTRL_F0_FULL       = 8'h1;	/* FIFO F0 full */
   
   
	localparam [2:0] dpMsbVal =(Length <=8)  ? (Length-6'd1): 
                               (Length <=16) ? (Length-6'd8-6'd1):
							   (Length <=24) ? (Length-6'd16-6'd1):
							   (Length <=32) ? (Length-6'd24-6'd1): 6'd0;
	        
    localparam dpDynShiftDir         = (Direction) ? `CS_SHFT_OP___SR : `CS_SHFT_OP___SL ;
    localparam dpDynShiftOut         = (Direction) ? `SC_SHIFT_SR : `SC_SHIFT_SL;
    localparam dpDynSIRouteChainA    = (Direction) ? `SC_SI_A_CHAIN : `SC_SI_A_ROUTE;
    localparam dpDynSIRouteChainB    = (Direction) ? `SC_SI_B_CHAIN : `SC_SI_B_ROUTE;
    localparam dpDynSIMSBRouteChainA = (Direction) ? `SC_SI_A_ROUTE : `SC_SI_A_CHAIN;
    localparam dpDynSIMSBRouteChainB = (Direction) ? `SC_SI_B_ROUTE : `SC_SI_B_CHAIN;
    
    localparam dpDynSO16 = (Direction) ? 0 : 1;
    localparam dpDynSO24 = (Direction) ? 0 : 2;
    localparam dpDynSO32 = (Direction) ? 0 : 3;
    
    
    
    wire	[7:0]	control; /* Control Register Output */
    
    wire    ctrl_clk_enable   = control[SR_CTRL_CLK_EN];
    wire    ctrl_f0_full      = control[SR_CTRL_F0_FULL];
	   
    /*Shift out wires of all Datapaths (8, 16, 24, 32 bits width) */
	
	wire so_8;
	wire [1:0] so_16;
	wire [2:0] so_24;
	wire [3:0] so_32;
	
	/* Instantiation of control register */
    
	cy_psoc3_control
    	#(.cy_force_order(1))
    CtrlReg(
        /* output	[07:00]	  */  .control(control)
    );
	
	/**************************************************************************/
   /* Clock enable logic Implementation                                        */
   /**************************************************************************/
	      
	wire f0_bus_stat_final;	
	wire f0_blk_stat_final;
	wire f1_blk_stat_final;
	wire f1_bus_stat_final;
	wire final_store;	
	
    
	
	if(FifoSize==1)begin
         assign final_store = (store & (!f1_bus_stat_final));
    end
    else if(FifoSize==4)begin
         assign final_store = (store & (!f1_blk_stat_final));
    end
    else assign final_store = 1'b0;
    
    
	
	
	/**************************************************************************/
    /* Status Register Implementation                                         */
    /**************************************************************************/
	
	localparam SR_LOAD_EVENT       	= 8'h0; /* Load event */
	localparam SR_STORE_EVENT	    = 8'h1; /* Store event */
    localparam SR_RES_EVENT         = 8'h2; /* Reset event */
    localparam SR_F0_EMPTY          = 8'h3; 
    localparam SR_F0_NOT_FULL       = 8'h4; 
    localparam SR_F1_FULL           = 8'h5;
    localparam SR_F1_NOT_EMPTY      = 8'h6; 
    
    
    wire	[6:0]	status;			            /* Status Register Input */
    
    assign status[SR_RES_EVENT]        = reset;  /* Status */
    assign status[SR_LOAD_EVENT]       = load_final;   /* Register */
    assign status[SR_STORE_EVENT]      = store;  /* connections */
    assign status[SR_F0_EMPTY]         = f0_blk_stat_final; 
	assign status[SR_F0_NOT_FULL]      = f0_bus_stat_final;
    assign status[SR_F1_FULL]          = f1_blk_stat_final;
    assign status[SR_F1_NOT_EMPTY]     = f1_bus_stat_final;
   
	/* Instantiation of status register*/
    
	cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'b0000111), 
        .cy_int_mask(7'b00000111)) 
    StsReg(
    /* input          */  .clock(clock),
    /* input  [06:00] */  .status(status),
    /* output         */  .interrupt(interrupt)
    );
    
    
    /**************************************************************************/
    /* FIFO Status Implementation                                         */
    /**************************************************************************/	

	assign load_final = load & (!f0_blk_stat_final);
    
    generate	
	
    /*8-bit shift register implementation
      shift input is configured as 'routed'*/

	if(Length <= 8) begin:sC8
		
		assign f0_blk_stat_final=f0_blk_stat_8;
        assign f0_bus_stat_final=f0_bus_stat_8;
		assign f1_blk_stat_final=f1_blk_stat_8;
		assign f1_bus_stat_final=f1_bus_stat_8;

	cy_psoc3_dp8 #(.cy_dpconfig_a(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
		`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	})) BShiftRegDp(
			/* input */ .clk(clock), /*Clock*/
			/* input [02:00] */ .cs_addr({ctrl_clk_enable,load_final,reset}), /* Control Store RAM address */
			/* input */ .route_si(shiftIn), /* Shift in from routing */
			/* input */ .route_ci(1'b0), /* Carry in from routing */
			/* input */ .f0_load(1'b0), /* Load FIFO 0 */
			/* input */ .f1_load(final_store), /* Load FIFO 1 */
			/* input */ .d0_load(1'b0), /* Load Data Register 0 */
			/* input */ .d1_load(1'b0), /* Load Data Register 1 */
			/* output */ .ce0(), /* Accumulator 0 = Data register 0 */
			/* output */ .cl0(), /* Accumulator 0 < Data register 0 */
			/* output */ .z0(),  /* Accumulator 0 = 0 */
			/* output */ .ff0(), /* Accumulator 0 = FF */
			/* output */ .ce1(), /* Accumulator [0|1] = Data register 1 */
			/* output */ .cl1(), /* Accumulator [0|1] < Data register 1 */ 
			/* output */ .z1(),  /* Accumulator 1 = 0 */
			/* output */ .ff1(), /* Accumulator 1 = FF */
			/* output */ .ov_msb(), /* Operation over flow */
			/* output */ .co_msb(), /* Carry out */
			/* output */ .cmsb(),   /* Carry out */
			/* output */ .so(so_8), /* Shift out */ 
			/* output */ .f0_bus_stat(f0_bus_stat_8), /* FIFO 0 status to uP */
			/* output */ .f0_blk_stat(f0_blk_stat_8), /* FIFO 0 status to DP */
			/* output */ .f1_bus_stat(f1_bus_stat_8), /* FIFO 1 status to uP */ 
			/* output */ .f1_blk_stat(f1_blk_stat_8) /* FIFO 1 status to DP */ 
			);
		
		/*Driving component 'shift_out' output*/
		
		assign shiftOut=so_8;
	
	end /*End of entire 8 bit section*/
	
	
	/* 16-bit shift register implementation */
	
	else if(Length<=16) begin:sC16
	
		
			assign f0_blk_stat_final=f0_blk_stat_16[1];
            assign f0_bus_stat_final=f0_bus_stat_16[1];
			assign f1_blk_stat_final=f1_blk_stat_16[1];
			assign f1_bus_stat_final=f1_bus_stat_16[1];
		
			
	cy_psoc3_dp16 #(.cy_dpconfig_a(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIRouteChainB,
		dpDynSIRouteChainA, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIMSBRouteChainB,
		dpDynSIMSBRouteChainA, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	})) BShiftRegDp(
			/* input */ .clk(clock), /* Clock */
			/* input [02:00] */ .cs_addr({ctrl_clk_enable,load_final,reset}), /* Control Store RAM address */
			/* input */ .route_si(shiftIn), /* Shift in from routing */
			/* input */ .route_ci(1'b0), /* Carry in from routing */
			/* input */ .f0_load(1'b0), /* Load FIFO 0 */
			/* input */ .f1_load(final_store), /* Load FIFO 1 */
			/* input */ .d0_load(1'b0), /* Load Data Register 0 */
			/* input */ .d1_load(1'b0), /* Load Data Register 1 */
			/* output [01:00] */ .ce0(),/* Accumulator 0 = Data register 0 */
			/* output [01:00] */ .cl0(),/* Accumulator 0 < Data register 0 */
			/* output [01:00] */ .z0(), /* Accumulator 0 = 0 */
			/* output [01:00] */ .ff0(),/* Accumulator 0 = FF */
			/* output [01:00] */ .ce1(), /* Accumulator [0|1] = Data register 1 */
			/* output [01:00] */ .cl1(), /* Accumulator [0|1] < Data register 1 */
			/* output [01:00] */ .z1(),  /* Accumulator 1 = 0 */
			/* output [01:00] */ .ff1(), /* Accumulator 1 = FF */
			/* output [01:00] */ .ov_msb(), /* Operation over flow */
			/* output [01:00] */ .co_msb(), /* Carry out */
			/* output [01:00] */ .cmsb(),   /* Carry out */
			/* output [01:00] */ .so(so_16),/* Shift out */
			/* output [01:00] */ .f0_bus_stat(f0_bus_stat_16), /* FIFO 0 status to uP */
			/* output [01:00] */ .f0_blk_stat(f0_blk_stat_16), /* FIFO 0 status to DP */
			/* output [01:00] */ .f1_bus_stat(f1_bus_stat_16), /* FIFO 1 status to uP */
			/* output [01:00] */ .f1_blk_stat(f1_blk_stat_16) /* FIFO 1 status to DP */
			);
	
	
			
			/*Driving component 'shift_out' output*/
	
			assign shiftOut=so_16[dpDynSO16];
	
	
		
	
		end /*End of entire 16 bit section*/
	
	/* 24-bit shift register implementation*/
	
	else if(Length<=24) begin : sC24
	
	
		assign f0_blk_stat_final=f0_blk_stat_24[2];
        assign f0_bus_stat_final=f0_bus_stat_24[2];
		assign f1_blk_stat_final=f1_blk_stat_24[2];
		assign f1_bus_stat_final=f1_bus_stat_24[2];
		
		
	cy_psoc3_dp24 #(.cy_dpconfig_a(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIRouteChainB,
		dpDynSIRouteChainA, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
		`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_c(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIMSBRouteChainB,
		dpDynSIMSBRouteChainA, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	})) BShiftRegDp(
			/* input */ .clk(clock), /* Clock */
			/* input [02:00] */ .cs_addr({ctrl_clk_enable,load_final,reset}), /* Control Store RAM address */
			/* input */ .route_si(shiftIn), /* Shift in from routing */
			/* input */ .route_ci(1'b0), /* Carry in from routing */
			/* input */ .f0_load(1'b0),  /* Load FIFO 0 */
			/* input */ .f1_load(final_store), /* Load FIFO 1 */
			/* input */ .d0_load(1'b0),  /* Load Data Register 0 */
			/* input */ .d1_load(1'b0),  /* Load Data Register 1 */
			/* output [02:00] */ .ce0(), /* Accumulator 0 = Data register 0 */
			/* output [02:00] */ .cl0(), /* Accumulator 0 < Data register 0 */
			/* output [02:00] */ .z0(),  /* Accumulator 0 = 0 */
			/* output [02:00] */ .ff0(), /* Accumulator 0 = FF */
			/* output [02:00] */ .ce1(), /* Accumulator [0|1] = Data register 1 */
			/* output [02:00] */ .cl1(), /* Accumulator [0|1] < Data register 1 */
			/* output [02:00] */ .z1(),  /* Accumulator 1 = 0 */
			/* output [02:00] */ .ff1(), /* Accumulator 1 = FF */
			/* output [02:00] */ .ov_msb(), /* Operation over flow */
			/* output [02:00] */ .co_msb(), /* Carry out */
			/* output [02:00] */ .cmsb(), /* Carry out */
			/* output [02:00] */ .so(so_24), /* Shift out */
			/* output [02:00] */ .f0_bus_stat(f0_bus_stat_24), /* FIFO 0 status to uP */
			/* output [02:00] */ .f0_blk_stat(f0_blk_stat_24), /* FIFO 0 status to DP */
			/* output [02:00] */ .f1_bus_stat(f1_bus_stat_24), /* FIFO 1 status to uP */
			/* output [02:00] */ .f1_blk_stat(f1_blk_stat_24) /* FIFO 1 status to DP */
			);
	
			/*Driving component 'shift_out' output*/	
			assign shiftOut=so_24[dpDynSO24];
	
	
	
		end /*End of entire 24 bit section*/
	
	/* 32-bit shift register implementation */
	
	else if(Length<=32) begin : sC32
	
				
		assign f0_blk_stat_final=f0_blk_stat_32[3];
        assign f0_bus_stat_final=f0_bus_stat_32[3];
		assign f1_blk_stat_final=f1_blk_stat_32[3];
		assign f1_bus_stat_final=f1_bus_stat_32[3];
		
	cy_psoc3_dp32 #(.cy_dpconfig_a(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIRouteChainB,
		dpDynSIRouteChainA, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
		`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_c(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
		`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_d(
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIMSBRouteChainB,
		dpDynSIMSBRouteChainA, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'b0,
		1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	})) BShiftRegDp(
			/* input */ .clk(clock), /* Clock */
			/* input [02:00] */ .cs_addr({ctrl_clk_enable,load_final,reset}), /* Control Store RAM address */
			/* input */ .route_si(shiftIn), /* Shift in from routing */
			/* input */ .route_ci(1'b0), /* Carry in from routing */ 
			/* input */ .f0_load(1'b0), /* Load FIFO 0 */
			/* input */ .f1_load(final_store), /* Load FIFO 1 */
			/* input */ .d0_load(1'b0), /* Load Data Register 0 */
			/* input */ .d1_load(1'b0), /* Load Data Register 1 */
			/* output [03:00] */ .ce0(), /* Accumulator 0 = Data register 0 */
			/* output [03:00] */ .cl0(), /* Accumulator 0 < Data register 0 */
			/* output [03:00] */ .z0(),  /* Accumulator 0 = 0 */
			/* output [03:00] */ .ff0(), /* Accumulator 0 = FF */
			/* output [03:00] */ .ce1(), /* Accumulator [0|1] = Data register 1 */
			/* output [03:00] */ .cl1(), /* Accumulator [0|1] < Data register 1 */
			/* output [03:00] */ .z1(),  /* Accumulator 1 = 0 */
			/* output [03:00] */ .ff1(), /* Accumulator 1 = FF */
			/* output [03:00] */ .ov_msb(), /* Operation over flow */
			/* output [03:00] */ .co_msb(), /* Carry out */
			/* output [03:00] */ .cmsb(), /* Carry out */
			/* output [03:00] */ .so(so_32), /* Shift out */
			/* output [03:00] */ .f0_bus_stat(f0_bus_stat_32), /* FIFO 0 status to uP */
			/* output [03:00] */ .f0_blk_stat(f0_blk_stat_32), /* FIFO 0 status to DP */
			/* output [03:00] */ .f1_bus_stat(f1_bus_stat_32), /* FIFO 1 status to uP */
			/* output [03:00] */ .f1_blk_stat(f1_blk_stat_32) /* FIFO 1 status to DP */
			);
	
			/*Driving component 'shift_out' output*/
	
			assign shiftOut=so_32[dpDynSO32];
	
	
		end /*End of entire 32 bit section*/
	


    endgenerate
    
    
    
endmodule

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


`endif      /* bShiftReg_v1_10_V_ALREADY_INCLUDED */

