//***************************************************************************************
//
// FILENAME:  B_SPI_Slave_v1_0.v
// MODULE NAME:   B_SPI_Slave_v1_0
// COMPONENT NAME: B_SPI_Slave_v1_0
// @Version@
//
// DESCRIPTION:
//   This file provides a structural top level model of the SPI Slave user module
//      defining the controller and datapath instances and all of the necessary
//      interconnect.
//
//-------------------------------------------------------------------
//                 Data Path register definitions  
//-------------------------------------------------------------------
//
//  INSTANCE NAME:  dp
//
//  DESCRIPTION:
//    data path containing the data to send and data Received
//
//  REGISTER USAGE:
//   F0 => Data to Send
//   F1 => 
//   D0 => 
//   D1 => 
//   A0 => Data being shifted out
//   A1 => 
//
//---------------------------------------------------------------------------------------
//  IO SIGNALS:
//   clock        input        User Supplied clock = 2x bitrate of output
//   reset        input        Global system reset signal
//   miso         output       SPI Master In Slave Out Signal
//   mosi         input        SPI Master Out Slave In Signal
//   sclk         input        SPI Master Output Clock Signal
//   ss           input        SPI Master Slave Select Signal
//
//    Todo:
//
//    * Setup shift between MSB and LSB dp's for greater than 8 bits stuff..
//    * Add all of the functionality of having the MSB datapath including 
//          multiple DAT files for all possible # bits etc.
//
//---------------------------------------------------------------------------------------
//  ********************************************************************************
// Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
// You may use this file only in accordance with the license, terms, conditions, 
// disclaimers, and limitations in the end user license agreement accompanying 
// the software package with which this file was provided.
// ********************************************************************************/

//***************************************************************************************
`include "cypress.v"
`ifdef B_SPIS_V_v1_0_ALREADY_INCLUDED
`else
`define B_SPIS_V_v1_0_ALREADY_INCLUDED
module B_SPI_Slave_v1_0(
    input	wire	reset,      //System Reset
    input	wire	clock,      //System Clk - 2x Bit rate
	output  wire    interrupt,	//Status Register Interrupt out
    
    output	wire	miso,       //SPI MISO output
    input	wire	mosi,       //SPI MOSI input
    input	wire	sclk,       //SPI SCLK input
    input	wire	ss          //SPI SS input
);
    
    wire    [6:0]   status;         //Slave Status register input

    wire            dpcounter_zero; //Counter compare to zero use to notify when a transfer is complete
    wire            dpcounter_one;  //Counter compare to 1 used for notification of when data has been received and should be loaded into the FIFO
    
    wire            slavemisoclk;
    wire            slavemisoDCS;
    wire            slave_sclk;
    
    wire            dp8MISO_f0_not_full;
    wire            dp16MISO_f0_not_full;
    wire            dp8MISO_f0_empty;
    wire            dp16MISO_f0_empty;
    wire            dp8MOSI_f0_full;
    wire            dp16MOSI_f0_full;
    wire            dp8MOSI_f0_not_empty;
    wire            dp16MOSI_f0_not_empty;
    
    wire            miso_tx_full;
    wire            miso_tx_empty;
    wire            mosi_rx_full;
    wire            mosi_rx_empty;
    
    wire            miso_tx_not_full;
    wire            mosi_rx_not_empty;
    
    
    wire            miso8;          //MISO from DP8 only used if NumberOfDataBits <= 8
    wire    [1:0]   miso16;         //MISO from DP16 only used if NumberOfDataBits > 8
	
	wire			nc0,nc1,nc2,nc3,nc4;
    
    wire            miso_from_dp;   // MISO selected based on ShiftDir parameter
                                    // Control takes this bit and assigns MISO as necessary
    
    /*The following Two parameters are constants do not modify them*/
    localparam SPIS_MSB_FIRST        =    1'b0;
    localparam SPIS_LSB_FIRST        =    1'b1;
    
    /*This wire is used to know when data has finished being received and should be loaded
    * into the FIFO
    * dpCounter Datapath and it's relations to loading the final data looks like this
    * dp_counter_zero      ss_active       dpCounter Function                   dpMOSI Function
    *     0                   0               Load Counter start value            shift data on sclk
    *     0                   1               Count down to zero                  shift data on sclk
    *     1                   0               Load Counter start value            shift data on sclk
    *     1                   1               unused                              Load Data to FIFO   
    */
    wire    mosi_dp_load;
    assign  mosi_dp_load = dpcounter_zero & !ss;
    wire    mosi_fifo_load;
    reg     counter_one_offset;
    wire    counter_one_offset_masked;
    wire [6:0] count;
    
/*Status register bits */
/* STATUS_BIT: Status */ localparam SPIS_STS_SPI_DONE_BIT                =    3'd0;
/* STATUS_BIT: Status */ localparam SPIS_STS_TX_BUFFER_NOT_FULL_BIT      =    3'd1;
/* STATUS_BIT: Status */ localparam SPIS_STS_TX_BUFFER_FULL_BIT          =    3'd2;
/* STATUS_BIT: Status */ localparam SPIS_STS_RX_BUFFER_NOT_EMPTY_BIT     =    3'd3;
/* STATUS_BIT: Status */ localparam SPIS_STS_RX_BUFFER_EMPTY_BIT         =    3'd4;
/* STATUS_BIT: Status */ localparam SPIS_STS_RX_BUFFER_OVERRUN_BIT       =    3'd5;
/* STATUS_BIT: Status */ localparam SPIS_STS_BYTE_COMPLETE_BIT           =    3'd6;

/***************************************************************************************
 * User parameters used to define how the UM is compiled
***************************************************************************************/
parameter [0:0] ShiftDir           =   1'b0;    //SPIS_MSB_FIRST is default
parameter [4:0] NumberOfDataBits   =   5'd8;   // set to 2-16 bits only. Default is 8 bits
/***************************************************************************************/
/* SPIM_POL = 0, SPIM_CPHA = 0          Rising edge mode; Data latched at rising edge; set at falling edge */
/* SPIM_POL = 1, SPIM_CPHA = 0          Falling edge mode; Data latched at rising edge; set at falling edge */
/* SPIM_POL = 0, SPIM_CPHA = 1          Rising edge mode; Data latched at falling edge; set at rising edge */
/* SPIM_POL = 1, SPIM_CPHA = 1          Falling edge mode; Data latched at falling edge; set at rising edge */
parameter ModeCPHA                 =   1'b0;   /* Default is rising edge mode */
parameter ModePOL                  =   1'b0;   /* Default is rising edge mode */
/****************************************************************************************/

    assign miso_from_dp = (NumberOfDataBits <= 8) ? miso8 : 
                            (ShiftDir == SPIS_LSB_FIRST) ? miso16[0] : miso16[1];
    assign miso = miso_from_dp & !ss; //Drive MISO based on Slave Select
                            
    assign miso_tx_full = (NumberOfDataBits <= 8) ? !dp8MISO_f0_not_full : !dp16MISO_f0_not_full;
    assign miso_tx_empty = (NumberOfDataBits <= 8) ? dp8MISO_f0_empty : dp16MISO_f0_empty;
    assign mosi_rx_full = (NumberOfDataBits <= 8) ? dp8MOSI_f0_full : dp16MOSI_f0_full;
    assign mosi_rx_empty = (NumberOfDataBits <= 8) ? !dp8MOSI_f0_not_empty : !dp16MOSI_f0_not_empty;

    assign miso_tx_not_full = (NumberOfDataBits <= 8) ? dp8MISO_f0_not_full : dp16MISO_f0_not_full;
    assign mosi_rx_not_empty = (NumberOfDataBits <= 8) ? dp8MOSI_f0_not_empty : dp16MOSI_f0_not_empty;
    
    
    assign slave_sclk = (ModePOL != ModeCPHA) ? !sclk : sclk;
    generate 
        if(ModeCPHA == 1'b0) begin
            assign slavemisoclk = (!dpcounter_zero | !miso_tx_empty) & !slave_sclk;
        end
        else begin
            assign slavemisoclk = (ModePOL == 1'b0) ? sclk : !sclk;
        end
    endgenerate
    assign mosi_fifo_load = dpcounter_one;
 
    localparam [2:0] dp8MsbVal = NumberOfDataBits[2:0] - 3'd1;
    localparam [7:0] dp8MOSIMask = (NumberOfDataBits == 8) ? 8'd255 :
                             (NumberOfDataBits == 7) ? 8'd127 :
                             (NumberOfDataBits == 6) ? 8'd63 :
                             (NumberOfDataBits == 5) ? 8'd31 :
                             (NumberOfDataBits == 4) ? 8'd15 :
                             (NumberOfDataBits == 3) ? 8'd7 :
                             /*(NumberOfDataBits == 2) ? */ 8'd3 ;
	localparam [2:0] dp16MsbVal = NumberOfDataBits[3:0] - 4'd9;    /* This value get's truncated to 3 bits */
    localparam [7:0] dp16MOSIMask = (NumberOfDataBits == 16) ? 8'd255 :
                              (NumberOfDataBits == 15) ? 8'd127 :
                              (NumberOfDataBits == 14) ? 8'd63 :
                              (NumberOfDataBits == 13) ? 8'd31 :
                              (NumberOfDataBits == 12) ? 8'd15 :
                              (NumberOfDataBits == 11) ? 8'd7 :
                              (NumberOfDataBits == 10) ? 8'd3 :
                              /*(NumberOfDataBits == 9)  ? */ 8'd1 ;
    localparam [1:0] dynShiftDir =  (ShiftDir == SPIS_MSB_FIRST) ? 2'd1 : 2'd2;
    
    localparam [1:0] dp16MSBSIChoice = (ShiftDir == SPIS_MSB_FIRST) ? `SC_SI_A_CHAIN : `SC_SI_A_ROUTE;
    localparam [1:0] dp16LSBSIChoice = (ShiftDir == SPIS_MSB_FIRST) ? `SC_SI_A_ROUTE : `SC_SI_A_CHAIN;
generate
    if(NumberOfDataBits <= 8) 
    begin : sR8
        // IMPLEM: MISO slave output should get !SS & !SCLK so that it latches data on 0 and shifts on 1
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Shift */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Shift and Load New FIFO Data */
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
        	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
        	1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
        	`SC_MSB_ENBL, dp8MsbVal, `SC_MSB_NOCHN,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
        	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        	`SC_FIFO__EDGE,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) dpMISO(
        /*  input                   */  .clk(slavemisoclk),
        /*  input   [02:00]         */  .cs_addr({2'b0,dpcounter_zero}),
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
        /*  output                  */  .cmsb(),
        /*  output                  */  .so(miso8),
        /*  output                  */  .f0_bus_stat(dp8MISO_f0_not_full),
        /*  output                  */  .f0_blk_stat(dp8MISO_f0_empty),
        /*  output                  */  .f1_bus_stat(),
        /*  output                  */  .f1_blk_stat()
        ); /* dp8MISO */
        
        // IMPLEM: MOSI slave input should get SCLK and shift on posedge clk - load when Counter == 1
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Shift Data In on SCLK always */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Unused */
        	  dp8MOSIMask, 8'h00,	/*SC_REG4	Comment: */
        	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
        	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
        	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
        	1'b0, `SC_FIFO1_ALU, `SC_FIFO0_ALU,
        	`SC_MSB_ENBL, dp8MsbVal, `SC_MSB_NOCHN,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
        	 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_AX,
        	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) dpMOSI(
            /*  input                   */  .clk(slave_sclk),
            /*  input   [02:00]         */  .cs_addr(3'b0),
            /*  input                   */  .route_si(mosi),
            /*  input                   */  .route_ci(1'b0),
            /*  input                   */  .f0_load(mosi_fifo_load),
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
            /*  output                  */  .cmsb(),
            /*  output                  */  .so(),
            /*  output                  */  .f0_bus_stat(dp8MOSI_f0_not_empty),
            /*  output                  */  .f0_blk_stat(dp8MOSI_f0_full),
            /*  output                  */  .f1_bus_stat(),
            /*  output                  */  .f1_blk_stat()
        ); /* dp8MOSI */
    end /* end of DP8_Section */
    else 
    begin : sR16
        cy_psoc3_dp16 #(.cy_dpconfig_a(
        {
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Shift */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Shift and Load New FIFO Data */
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
        	dp16LSBSIChoice, /*SC_REG6 Comment: */
        	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
        	1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
        	`SC_MSB_ENBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
        	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        	`SC_FIFO__EDGE,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
        {
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Shift */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Shift and Load New FIFO Data */
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
        	dp16MSBSIChoice, /*SC_REG6 Comment: */
        	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
        	1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
        	`SC_MSB_ENBL, dp16MsbVal, `SC_MSB_CHNED,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
        	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        	`SC_FIFO__EDGE,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) dpMISO(
        /*  input                   */  .clk(slavemisoclk),
        /*  input   [02:00]         */  .cs_addr({2'b0,dpcounter_zero}),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(1'b0),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output  [01:00]         */  .ce0(),
        /*  output  [01:00]         */  .cl0(),
        /*  output  [01:00]         */  .z0(),
        /*  output  [01:00]         */  .ff0(),
        /*  output  [01:00]         */  .ce1(),
        /*  output  [01:00]         */  .cl1(),
        /*  output  [01:00]         */  .z1(),
        /*  output  [01:00]         */  .ff1(),
        /*  output  [01:00]         */  .ov_msb(),
        /*  output  [01:00]         */  .co_msb(),
        /*  output  [01:00]         */  .cmsb(),
        /*  output  [01:00]         */  .so(miso16),
        /*  output  [01:00]         */  .f0_bus_stat({dp16MISO_f0_not_full,nc1}),
        /*  output  [01:00]         */  .f0_blk_stat({dp16MISO_f0_empty,nc2}),
        /*  output  [01:00]         */  .f1_bus_stat(),
        /*  output  [01:00]         */  .f1_blk_stat()
        );
    
        cy_psoc3_dp16 #(.cy_dpconfig_a(
        {
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Shift Data In on SCLK always */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Unused */
        	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
        	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
        	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        	dp16LSBSIChoice, /*SC_REG6 Comment: */
        	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
        	1'b0, `SC_FIFO1_ALU, `SC_FIFO0_ALU,
        	`SC_MSB_ENBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
        	 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_AX,
        	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
}), .cy_dpconfig_b(
        {
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Shift Data In on SCLK always */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Unused */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Unused */
        	  dp16MOSIMask, 8'h00,	/*SC_REG4	Comment: */
        	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
        	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        	dp16MSBSIChoice, /*SC_REG6 Comment: */
        	`SC_A0_SRC_ACC, ShiftDir, 1'b0,
        	1'b0, `SC_FIFO1_ALU, `SC_FIFO0_ALU,
        	`SC_MSB_ENBL, dp16MsbVal, `SC_MSB_CHNED,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
        	 10'h0, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_AX,
        	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) dpMOSI(
        /*  input                   */  .clk(slave_sclk),
        /*  input   [02:00]         */  .cs_addr(3'b0),//{2'b0,mosi_dp_load}),// TODO: OLD METHOD{2'd0,dpcounter_zero}),
        /*  input                   */  .route_si(mosi),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(mosi_fifo_load),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output  [01:00]         */  .ce0(),
        /*  output  [01:00]         */  .cl0(),
        /*  output  [01:00]         */  .z0(),
        /*  output  [01:00]         */  .ff0(),
        /*  output  [01:00]         */  .ce1(),
        /*  output  [01:00]         */  .cl1(),
        /*  output  [01:00]         */  .z1(),
        /*  output  [01:00]         */  .ff1(),
        /*  output  [01:00]         */  .ov_msb(),
        /*  output  [01:00]         */  .co_msb(),
        /*  output  [01:00]         */  .cmsb(),
        /*  output  [01:00]         */  .so(),
        /*  output  [01:00]         */  .f0_bus_stat({dp16MOSI_f0_not_empty,nc3}),     // TODO: Now this is two bits, figure out which one I want??  Should be based on the note at the top of this document about lower one being written last.
        /*  output  [01:00]         */  .f0_blk_stat({dp16MOSI_f0_full,nc4}),     // TODO: Now this is two bits, figure out which one I want??  Should be based on the note at the top of this document about lower one being written last.
        /*  output  [01:00]         */  .f1_bus_stat(),
        /*  output  [01:00]         */  .f1_blk_stat()
        );
    end /* end of DP16_Section */
endgenerate

    wire ctr_enable;
    wire ctr_load;
    assign ctr_load = ss;
    assign ctr_enable = !ss;
    cy_psoc3_count7 #(.cy_period(NumberOfDataBits-7'd1),.cy_route_ld(1),.cy_route_en(1))
    bitCounter(
    /* input		    */  .clock(slave_sclk),
    /* input		    */  .reset(1'b0),
    /* input		    */  .load(ctr_load),
    /* input		    */  .enable(ctr_enable),
    /* output [06:00]	*/  .count(count),
    /* output		    */  .tc() 
    );
    
    assign dpcounter_zero = (count == 7'd0);
    assign dpcounter_one = (count == 7'd1);
    
    reg dpcounter_one_reg;
    always @(posedge slave_sclk) begin
        dpcounter_one_reg <= dpcounter_one;
    end
    
	/* Instantiate the status register and interrupt hook */
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h60), 
        .cy_int_mask(7'h7F)) 
    stsreg(
        /* input          */  .clock(clock),
        /* input  [06:00] */  .status(status),
        /* output         */  .interrupt(interrupt)
    ); 
    
    assign status[SPIS_STS_TX_BUFFER_NOT_FULL_BIT] = miso_tx_not_full;
    assign status[SPIS_STS_RX_BUFFER_EMPTY_BIT] = mosi_rx_empty;
    assign status[SPIS_STS_TX_BUFFER_FULL_BIT] = miso_tx_full;
    assign status[SPIS_STS_RX_BUFFER_NOT_EMPTY_BIT] = mosi_rx_not_empty;
    
    assign status[SPIS_STS_SPI_DONE_BIT] = (miso_tx_empty & dpcounter_one_reg);
    assign status[SPIS_STS_RX_BUFFER_OVERRUN_BIT] = (mosi_rx_full & mosi_fifo_load);
    assign status[SPIS_STS_BYTE_COMPLETE_BIT] = dpcounter_one_reg;
  
endmodule /* SPI_Slave */
`endif /* SPIS_V_v0_5_ALREADY_INCLUDED */
