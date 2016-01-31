/*******************************************************************************
 *
 * FILENAME:  B_SPI_Master_v1_20.v
 * MODULE NAME: B_SPI_Master_v1_20
 * COMPONENT NAME:   B_SPI_Master_v1_20
 * @Version@
 *
 * DESCRIPTION:
 *   This file provides a structural top level model of the SPI Master user module
 *      defining the controller and datapath instances and all of the necessary
 *      interconnect.
 *
 *
 *-------------------------------------------------------------------
 *                 Data Path register definitions  
 *-------------------------------------------------------------------
 *
 *  INSTANCE NAME:  dp
 *  DATAPATH CONFIGURATION:  

 *
 *  DESCRIPTION:
 *    8 or 16 bit data path containing data shifting out and data shifting in
 *
 *  REGISTER USAGE:
 *   F0 => Data to send
 *   F1 => Data Received
 *   D0 => Unused
 *   D1 => Unused
 *   A0 => Data being shifted out
 *   A1 => Data being shifted in
 *
 *------------------------------------------------------------------------------
 *  IO SIGNALS:
 *   clk          input        User Supplied clock = 2x bitrate of output
 *   reset        input        Global system reset signal
 *   miso         input        SPI Master In Slave Out Signal
 *   mosi         output       SPI Master Out Slave In Signal
 *   sclk         output       SPI Master Output Clock Signal
 *   ss           output       SPI Master Slave Select Signal
 *
 *    Todo:
 *
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef B_SPI_Master_v1_20_ALREADY_INCLUDED
`else
`define B_SPI_Master_v1_20_ALREADY_INCLUDED
module B_SPI_Master_v1_20(
    input	wire	reset,      /* System Reset                               */
    input	wire	clock,      /* User Supplied clock = 2x bitrate of output */
    
    input	wire	miso,       /* SPI MISO input                             */
    output	wire	mosi,       /* SPI MOSI output                            */
    output	wire	sclk,       /* SPI SCLK output                            */
    output	wire	ss,         /* SPI SS output                              */
    output  wire    interrupt   /* Interrupt output signal                   */
	,
	output  wire [2:0] state,
	output  wire 	empty,
	output  wire	empty2,
    output  wire    counter_one
);
    
    wire    [6:0]   status;             /* Status Register inputs from the PLD/DP's */
    wire            sclktmp;            /* a temporary SCLK output used with logic to generate the final SCLK */
    wire            mosi_from_dp;       /*  Master Out Slave In from the Datapath. Selects between mosi_dp8 and mosi_dp16 based on NUM_BITS */
    wire            dpcounter_one;      /* One compare output of the counter which signals when to load received data into the FIFO */
    wire            dpcounter_three;    /* Three compare output fo the counter which is used to determine if more data is ready in the FIFO to send */
    wire    [2:0]   dcs_state;          /* The controller state machine drives the Dynamic Control Store of all DP's */
    wire    [6:0]   count;              /* 7-bit counter output used for a compare to one output */
    
    /* MOSI FIFO Status outputs */
    wire            dpMOSI_fifo_not_empty;
    
	wire			nc1,nc2,nc3,nc4;
    
    /* bit order: default is MSb first (i.e Shift Left and ShiftLeft in static configuration is = 0) */
    /* DO NOT CHANGE these two parameters.  They define constants */
    localparam SPIM_MSB_FIRST        =    1'b0;
    localparam SPIM_LSB_FIRST        =    1'b1;
    parameter HardwareSS             =    1'b1; //Hardware will control the SS output
    parameter AllowRapidFire         =    1'b1; //Allow multiple output transfers to be streamed
    
/* Status register bits */
localparam SPIM_STS_SPI_DONE_BIT                =    3'd0;
localparam SPIM_STS_TX_FIFO_EMPTY_BIT           =    3'd1;
localparam SPIM_STS_TX_FIFO_NOT_FULL_BIT        =    3'd2;
localparam SPIM_STS_RX_FIFO_FULL_BIT            =    3'd3;
localparam SPIM_STS_RX_FIFO_NOT_EMPTY_BIT       =    3'd4;
localparam SPIM_STS_RX_FIFO_OVERRUN_BIT         =    3'd5;
localparam SPIM_STS_BYTE_COMPLETE_BIT           =    3'd6;

/*******************************************************************************
 *User parameters used to define how the component is compiled
 ******************************************************************************/
parameter [0:0] ShiftDir           =   SPIM_MSB_FIRST;
parameter [5:0] NumberOfDataBits   =   5'd8; // set to 2-8 bits only. Default is 8 bits
/****************************************************************************************
 * SPIM_POL = 0, SPIM_CPHA = 0   // Rising edge mode; Data latched at rising edge; set at falling edge
 * SPIM_POL = 1, SPIM_CPHA = 0   // Falling edge mode; Data latched at rising edge; set at falling edge   
 * SPIM_POL = 0, SPIM_CPHA = 1   // Rising edge mode; Data latched at falling edge; set at rising edge
 * SPIM_POL = 1, SPIM_CPHA = 1   // Falling edge mode; Data latched at falling edge; set at rising edge
 ******************************************************************************/
parameter [0:0] ModeCPHA           =   1'b0; /* Default is rising edge mode */
parameter [0:0] ModePOL            =   1'b0; /* Default is rising edge mode */
/******************************************************************************/
    
    localparam [2:0] dpMsbVal = NumberOfDataBits[2:0]-3'b1;
	localparam [7:0] dpMISOMask = (NumberOfDataBits == 8 || NumberOfDataBits == 16) ? 8'b1111_1111 :
                                  (NumberOfDataBits == 7 || NumberOfDataBits == 15) ? 8'b0111_1111 :
                                  (NumberOfDataBits == 6 || NumberOfDataBits == 14) ? 8'b0011_1111 :
                                  (NumberOfDataBits == 5 || NumberOfDataBits == 13) ? 8'b0001_1111 :
                                  (NumberOfDataBits == 4 || NumberOfDataBits == 12) ? 8'b0000_1111 :
                                  (NumberOfDataBits == 3 || NumberOfDataBits == 11) ? 8'b0000_0111 :
                                  (NumberOfDataBits == 2 || NumberOfDataBits == 10) ? 8'b0000_0011 :
                                  (NumberOfDataBits == 9) ? 8'b0000_0001 : 8'b1111_1111;
    localparam [1:0] dynShiftDir =  (ShiftDir == SPIM_MSB_FIRST) ? 2'd1 : 2'd2;
    
    localparam [1:0] dp16MSBSIChoice = (ShiftDir == SPIM_MSB_FIRST) ? `SC_SI_A_CHAIN : `SC_SI_A_ROUTE;
    localparam [1:0] dp16LSBSIChoice = (ShiftDir == SPIM_MSB_FIRST) ? `SC_SI_A_ROUTE : `SC_SI_A_CHAIN;
    
    wire   dpMOSI_fifo_not_full;
    wire   dpMOSI_fifo_empty;
    wire   dpMISO_fifo_not_empty;
    wire   dpMISO_fifo_full;
    wire   miso_buf_overrun;
    wire   spim_done;
    wire   byte_complete;
    wire   counter_load;
    
    assign status[SPIM_STS_TX_FIFO_EMPTY_BIT] = dpMOSI_fifo_empty;
    assign status[SPIM_STS_TX_FIFO_NOT_FULL_BIT] = dpMOSI_fifo_not_full;
    assign status[SPIM_STS_RX_FIFO_NOT_EMPTY_BIT] = dpMISO_fifo_not_empty;
    assign status[SPIM_STS_RX_FIFO_FULL_BIT] = dpMISO_fifo_full;
    assign status[SPIM_STS_RX_FIFO_OVERRUN_BIT] = miso_buf_overrun;
    assign status[SPIM_STS_SPI_DONE_BIT] = spim_done;
    assign status[SPIM_STS_BYTE_COMPLETE_BIT] = byte_complete;

    assign dpMOSI_fifo_not_empty = !dpMOSI_fifo_empty;
    wire TMP1;
    
    generate
    if(NumberOfDataBits <= 8) begin: sR8
    cy_psoc3_dp8 #(.cy_dpconfig_a(
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:IDLE */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:LOAD F0 to A0 */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Change Shift Out */
    	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Capture Shift In */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:END */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:LDSHIFT */    	
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Change Shift Out */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	dpMISOMask, 8'h00,	/*SC_REG4	Comment: */
    	8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
    	1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) dp(
        /*  input                   */  .clk(clock),
        /*  input   [02:00]         */  .cs_addr(dcs_state),
        /*  input                   */  .route_si(miso),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(1'b0),
        /*  input                   */  .f1_load(dpcounter_one),
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
        /*  output                  */  .cmsb(),
        /*  output                  */  .so(mosi_from_dp),
        /*  output                  */  .f0_bus_stat(dpMOSI_fifo_not_full),
        /*  output                  */  .f0_blk_stat(dpMOSI_fifo_empty),
        /*  output                  */  .f1_bus_stat(dpMISO_fifo_not_empty),
        /*  output                  */  .f1_blk_stat(dpMISO_fifo_full)
    );
    end /* NumberOfDataBits <= 8 */
    else if (NumberOfDataBits <= 16) begin : sR16       /* NumberOfDataBits > 8 */
    wire mosi_from_dpL;
    wire mosi_from_dpR;
    cy_psoc3_dp16 #(.cy_dpconfig_a (
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:IDLE */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:LOAD F0 to A0 */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Change Shift Out */
    	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Capture Shift In */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:END */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:LDSHIFT */    	
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Change Shift Out */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	dp16LSBSIChoice, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
    	1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    }), .cy_dpconfig_b (
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:IDLE */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:LOAD F0 to A0 */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Change Shift Out */
    	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Capture Shift In */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:END */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:LDSHIFT */    	
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Change Shift Out */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
    	dpMISOMask, 8'h00,	/*SC_REG4	Comment: */
    	8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	dp16MSBSIChoice, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
    	1'b0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) dp(
        /*  input                   */  .clk(clock),
        /*  input   [02:00]         */  .cs_addr(dcs_state),
        /*  input                   */  .route_si(miso),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(1'b0),
        /*  input                   */  .f1_load(dpcounter_one),
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
        /*  output                  */  .cmsb(),
        /*  output                  */  .so({mosi_from_dpL,mosi_from_dpR}),
        /*  output                  */  .f0_bus_stat({dpMOSI_fifo_not_full, nc1}),
        /*  output                  */  .f0_blk_stat({TMP1, nc2}),
        /*  output                  */  .f1_bus_stat({dpMISO_fifo_not_empty, nc3}),
        /*  output                  */  .f1_blk_stat({dpMISO_fifo_full,nc4})
    );
    
    assign dpMOSI_fifo_empty = (TMP1 | nc2);
    
    assign mosi_from_dp = (ShiftDir == SPIM_MSB_FIRST) ? mosi_from_dpL : mosi_from_dpR;
    end /* sR16 */
    endgenerate
    
    cy_psoc3_count7 #(.cy_period((NumberOfDataBits*7'd2)-7'd1),.cy_route_ld(1),.cy_route_en(1))
    bitCounter(
    /* input		    */  .clock(clock),
    /* input		    */  .reset(reset),
    /* input		    */  .load(counter_load),
    /* input		    */  .enable(1'b1),
    /* output [06:00]	*/  .count(count),
    /* output		    */  .tc() 
    );
    
    assign dpcounter_three = (count == 7'd3);
    assign dpcounter_one = (count == 7'd0);
    
	/* Instantiate the status register and interrupt hook*/
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h61), 
        .cy_int_mask(7'h7F)) 
    stsreg(
        /* input            */  .clock(clock),
        /* input    [06:00] */  .status(status),
        /* output           */  .interrupt(interrupt)
    );
    
    spim_ctrl_v1_20 #(.NumberOfDataBits(NumberOfDataBits), .ModeCPHA(ModeCPHA),
        .HardwareSS(HardwareSS), .AllowRapidFire(AllowRapidFire))
	ctrl(
        /* input                */  .clock(clock),
        /* input                */  .reset(reset),
        /* input                */  .counter_one(dpcounter_one),
        /* input                */  .counter_three(dpcounter_three),
        /* input                */  .mosi_from_dp(mosi_from_dp),
        /* input                */  .data_to_send(dpMOSI_fifo_not_empty),
        /* input                */  .miso_fifo_full(dpMISO_fifo_full),
        /* output               */  .byte_complete(byte_complete),
        /* output               */  .rx_buf_ov(miso_buf_overrun),
        /* output               */  .spi_done(spim_done),
        /* output               */  .miso_fifo_load(),
        /* output               */  .counter_load(counter_load),
        /* output               */  .ss(ss),
        /* output               */  .sck(sclktmp),
        /* output               */  .mosi(mosi),
        /* output [2:0]         */  .state(dcs_state)
    );
    
    assign sclk = (!ModePOL) ? sclktmp : ~sclktmp;
	
	assign state = dcs_state;
	assign empty = dpMOSI_fifo_empty;
	assign empty2 = TMP1;
    assign counter_one = dpcounter_one;
endmodule /* SPI_Master */

/*******************************************************************************
 *
 * MODULE NAME:  spim_ctrl_v1_10
 * COMPONENT NAME:   SPI_Master
 * @Version@
 *
 * DESCRIPTION:
 *   This file provides the controller state machine for the SPI master operation.
 *
 *---------------------------------------------------------------------------------------
 *  IO SIGNALS:
 *   clock          input        User Supplied clock = 2x bitrate of output
 *   reset          input        Global system reset signal
 *   mosi_from_dp   input        Data Shift Out signal from the datapath
 *   data_to_send   input        Signal indicating when data has been written to F0 register and is ready to send
 *   miso_fifo_full input        Signal indicates when F1 register is full, used to determine overflow, and rx buffer full status
 *
 *  Todo:
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`ifdef spim_ctrl_v1_20_ALREADY_INCLUDED
`else
`define spim_ctrl_v1_20_ALREADY_INCLUDED

module spim_ctrl_v1_20 (
    input	wire	clock,   /* input clk is 2x the bit-rate */
    input	wire	reset,
    input	wire	counter_one,
    input   wire    counter_three,
    input	wire	mosi_from_dp,
    input	wire	data_to_send,
    input	wire	miso_fifo_full,
    
    output	wire 	byte_complete               ,
    output	wire	rx_buf_ov                   ,
    output	wire 	spi_done                    ,
    output	wire	miso_fifo_load              ,
    output	wire	ss                          ,
    output  wire    counter_load                ,
    
    output	wire	sck                         ,   /* Output to SCK pin      */
    output	wire  	mosi                        ,   /* Output to MOSI pin     */
    output	reg [2:0]	state                       /* State machine output to control Dynamic Configuration */
);  

   /****************************************************************************
    * state bit assignment  //THESE ARE CONSTANTS DO NOT MODIFY
    ***************************************************************************/
    localparam SPIM_STATE_IDLE           =   3'd0;
    localparam SPIM_STATE_ONE            =   3'd1;
    localparam SPIM_STATE_TWO            =   3'd2;
    localparam SPIM_STATE_THREE          =   3'd3;
    localparam SPIM_STATE_FOUR           =   3'd5;
    localparam SPIM_STATE_FIVE           =   3'd4;
    localparam SPIM_STATE_SIX            =   3'd6;
    localparam SPIM_STATE_SEVEN          =   3'd7;
    /**************************************************************************/

    wire         scktmp;

    parameter [4:0]  NumberOfDataBits = 4'd8;
    parameter ModeCPHA  = 1'b0;
    parameter HardwareSS = 1'b1;
    parameter AllowRapidFire = 1'b0;
	
	assign byte_complete = counter_one;    
    assign spi_done = (state == SPIM_STATE_FIVE);
    
    reg        mosi_tmp;
    
    assign counter_load = (state == SPIM_STATE_IDLE || state == SPIM_STATE_ONE || state == SPIM_STATE_SIX);
    
    wire regreset;
    assign regreset = (state == SPIM_STATE_IDLE);
    always @(posedge clock or posedge regreset) begin
        if(regreset) begin
            mosi_tmp <= 1'b0;
        end
        else begin
            if(state == SPIM_STATE_SIX || state == SPIM_STATE_TWO || state == SPIM_STATE_FOUR) begin
                mosi_tmp <= mosi_from_dp;
            end
        end
    end    
    assign mosi = (state == SPIM_STATE_IDLE || state == SPIM_STATE_FIVE) ? 1'b0 : mosi_tmp;
    
    generate 
        if(ModeCPHA == 1'b0) begin
            assign sck = scktmp & !ss;
        end
        else begin
            assign sck = scktmp & (state != SPIM_STATE_FIVE);
        end
    endgenerate

    assign rx_buf_ov = miso_fifo_full & miso_fifo_load;
    
    assign ss = (state == SPIM_STATE_IDLE);
    assign miso_fifo_load = (state == SPIM_STATE_FIVE) | (state == SPIM_STATE_FOUR);
    
    generate
    reg   data_in_acc;
    if(ModeCPHA == 1'b0) begin : CPHA0_SM
        assign scktmp = (state == SPIM_STATE_TWO || state == SPIM_STATE_FOUR) ? 1'b1 : 1'b0;
        always @(posedge clock or posedge reset) begin
        if (reset) begin
            data_in_acc <= 1'b0;
            state <= SPIM_STATE_IDLE;
            end
        else begin
                case(state)
                SPIM_STATE_IDLE /*0*/: begin
                    data_in_acc <= 1'b0;
                    if(data_to_send) begin      //Message From FIFO saying data is available
                        state <= SPIM_STATE_ONE;
                    end
                    else
                        state <= SPIM_STATE_IDLE;
                end
                SPIM_STATE_ONE: begin
                    state <= SPIM_STATE_SIX;
                end
                SPIM_STATE_TWO: begin
                    if(counter_one == 1'b1) begin
                        if(data_in_acc == 1'b1) begin
                            state <= SPIM_STATE_THREE;
                        end
                        else begin
                            state <= SPIM_STATE_FIVE;
                        end
                    end
                    else begin
                        data_in_acc <= 1'b0;
                        state <= SPIM_STATE_THREE;
                    end
                end
                SPIM_STATE_THREE: begin
                    if(counter_three == 1'b1) begin
                        if(data_to_send == 1'b1) begin
                            state <= SPIM_STATE_FOUR;
                        end
                        else begin
                            state <= SPIM_STATE_TWO;
                        end
                    end
                    else begin
                        state <= SPIM_STATE_TWO;
                    end
                end
                SPIM_STATE_FOUR: begin
                    data_in_acc <= 1'b1;
                    state <= SPIM_STATE_THREE;
                end
                SPIM_STATE_FIVE: begin
                    state <= SPIM_STATE_IDLE;
                end
                SPIM_STATE_SIX: begin
                    state <= SPIM_STATE_THREE;
                end
                default: state <= SPIM_STATE_IDLE;  
                endcase
            end
        end
    end //CPHA0_SM
	else begin : CPHA1_SM
        assign scktmp = (state == SPIM_STATE_THREE) ? 1'b1 : 1'b0;
       
        always @(posedge clock or posedge reset) begin
        if (reset) begin
            data_in_acc <= 1'b0;
            state <= SPIM_STATE_IDLE;
            end
        else begin
                case(state)
                SPIM_STATE_IDLE /*0*/: begin
                    data_in_acc <= 1'b0;
                    if(data_to_send) begin      //Message From FIFO saying data is available
                        state <= SPIM_STATE_ONE;
                    end
                    else
                        state <= SPIM_STATE_IDLE;
                    end
                SPIM_STATE_ONE: begin
                        state <= SPIM_STATE_SIX;
                    end
                SPIM_STATE_TWO: begin
                    if(counter_one == 1'b1) begin
                        state <= SPIM_STATE_FIVE;
                        if(data_to_send == 1'b1) begin
                            state <= SPIM_STATE_THREE;
                        end
                        else if(data_in_acc == 1'b1) begin
                            state <= SPIM_STATE_THREE;
                        end
                    end
                    else begin
                        data_in_acc <= 1'b0;
                        state <= SPIM_STATE_THREE;
                    end
                end
                SPIM_STATE_THREE: begin
                    if(counter_three == 1'b1) begin
                        if(data_to_send == 1'b1) begin
                            state <= SPIM_STATE_FOUR;
                        end
                        else begin
                            state <= SPIM_STATE_TWO;
                        end
                    end
                    else begin
                        state <= SPIM_STATE_TWO;
                    end
                end
                SPIM_STATE_FOUR: begin
                    data_in_acc <= 1'b1;
                    state <= SPIM_STATE_THREE;
                    end
                SPIM_STATE_FIVE: begin
                    state <= SPIM_STATE_IDLE;
                    end
                SPIM_STATE_SIX: begin
                    state <= SPIM_STATE_THREE;
                end
                default: state <= SPIM_STATE_IDLE;  
                endcase
            end
        end
    end //CPHA1_SM
    endgenerate
endmodule
`endif /* spim_ctrl_v1_20_ALREADY_INCLUDED */
`endif /* B_SPIM_V_v1_20_ALREADY_INCLUDED */

